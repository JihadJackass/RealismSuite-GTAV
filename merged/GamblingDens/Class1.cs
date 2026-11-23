using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;

namespace IllligalGamblingDens
{
	public class Class1 : Script
	{
		public class Enterace
		{
			public Vector3 Ex { get; set; }

			public Vector3 En { get; set; }

			public Blip blip { get; set; }

			public int InteriorType { get; set; }

			public bool IsInterior { get; set; }

			public Enterace()
			{
			}

			public Enterace(Vector3 Enterance, Vector3 Exit)
			{
				En = Enterance;
				Ex = Exit;
			}

			public Enterace(Vector3 Enterance, Vector3 Exit, int IT)
			{
				InteriorType = IT;
				En = Enterance;
				Ex = Exit;
			}
		}

		public List<Prop> Props = new List<Prop>();

		public List<Ped> PedsList = new List<Ped>();

		public List<Enterace> DenEnterances = new List<Enterace>();

		public string ChairPropModelMain = "ex_prop_offchair_exec_01";

		public string ChairPropModelArcadeA = "ex_prop_offchair_exec_01";

		public string ChairPropModelArcadeB = "ex_prop_offchair_exec_01";

		public string ChairPropModelPenthouse = "ex_prop_offchair_exec_01";

		public Prop _8PlayerPokerGameTable1;

		public Prop _8PlayerPokerGameTable2;

		public Prop _8PlayerPokerGameTable3;

		public Prop _8PlayerPokerGameTable4;

		public Prop _8PlayerPokerGameTable5;

		public Prop _8PlayerPokerGameTable6;

		public Prop _8PlayerPokerGameTable7;

		public List<Ped> _8PlayerGamePokerPeds = new List<Ped>();

		public List<Prop> _8PlayerGamePokerProps = new List<Prop>();

		public List<Prop> _8PlayerGamePokerPotChips = new List<Prop>();

		public float _8PlayerGamePoker_PlayerBank;

		public float _8PlayerGamePoker_AI1Bank;

		public float _8PlayerGamePoker_AI2Bank;

		public float _8PlayerGamePoker_AI3Bank;

		public float _8PlayerGamePoker_AI4Bank;

		public float _8PlayerGamePoker_AI5Bank;

		public float _8PlayerGamePoker_AI6Bank;

		public float _8PlayerGamePoker_AI7Bank;

		public float _8PlayerGamePokerPot;

		public Vector3 _8PlayerGameStartMarker;

		public Vector3 _8PlayerGameExitMarker;

		public int AI1_SyncScene;

		public int AI2_SyncScene;

		public int AI3_SyncScene;

		public int AI4_SyncScene;

		public int AI5_SyncScene;

		public int AI6_SyncScene;

		public int AI7_SyncScene;

		public Ped AI_1;

		public Ped AI_2;

		public Ped AI_3;

		public Ped AI_4;

		public Ped AI_5;

		public Ped AI_6;

		public Ped AI_7;

		public bool Player_SkipTurn;

		public bool Player_Bust;

		public bool AI1_SkipTurn;

		public bool AI1_Bust;

		public bool AI2_SkipTurn;

		public bool AI2_Bust;

		public bool AI3_SkipTurn;

		public bool AI3_Bust;

		public bool AI4_SkipTurn;

		public bool AI4_Bust;

		public bool AI5_SkipTurn;

		public bool AI5_Bust;

		public bool AI6_SkipTurn;

		public bool AI6_Bust;

		public bool AI7_SkipTurn;

		public bool AI7_Bust;

		public int _8PlayerGameStage;

		public Ped _8PlayerGameDealer;

		public List<PlayerChips> PlayerChips = new List<PlayerChips>();

		public List<PlayerCards> PlayerCards = new List<PlayerCards>();

		public float LastRaiseAmmount;

		public Prop PlayerCard1;

		public Prop PlayerCard2;

		public Prop PlayerCard3;

		public float _8PlayerGamePoker_PlayerStartingBank;

		public float _8PlayerGamePoker_AI1StartingBank;

		public float _8PlayerGamePoker_AI2StartingBank;

		public float _8PlayerGamePoker_AI3StartingBank;

		public float _8PlayerGamePoker_AI4StartingBank;

		public float _8PlayerGamePoker_AI5StartingBank;

		public float _8PlayerGamePoker_AI6StartingBank;

		public float _8PlayerGamePoker_AI7StartingBank;

		public bool lookingAtCards;

		public float PlayerTotal;

		public float AI1Total;

		public float AI2Total;

		public float AI3Total;

		public float AI4Total;

		public float AI5Total;

		public float AI6Total;

		public float AI7Total;

		public int Scene2;

		public Prop ChipsSpawn;

		public Prop Deck;

		public Prop LastChip;

		public Prop CommunityCard1;

		public float CommunityCard1Value;

		public string CommunityCard1ID;

		public Prop CommunityCard2;

		public float CommunityCard2Value;

		public string CommunityCard2ID;

		public Prop CommunityCard3;

		public float CommunityCard3Value;

		public string CommunityCard3ID;

		public bool IsinBettingRound;

		public float MaxRaiseAmt;

		public int StackChip;

		public float StackChipHeight;

		public bool Playing8PlayerPoker;

		public List<Prop> Tables = new List<Prop>();

		public List<Prop> Tables2 = new List<Prop>();

		public List<Prop> Chips = new List<Prop>();

		public List<Ped> SMPed = new List<Ped>();

		public int PlayerCard_1;

		public int PlayerCard_2;

		public int PlayerCard_3;

		public int PlayerCard_4;

		public int PlayerCard_5;

		public int PlayerCard_6;

		public int DealerCard_1;

		public int DealerCard_2;

		public int DealerCard_3;

		public int DealerCard_4;

		public int DealerCard_5;

		public int DealerCard_6;

		public float Mp = 1f;

		public bool Playing_Poker;

		public bool Playing_BlackJack;

		public bool Playing_SlotMachine;

		public bool Playing_Roulette;

		public bool Placebet;

		public bool Drawcards;

		public bool Editbet;

		public int Playercards;

		public int DealerCards;

		private float Bet;

		public int PlayerDeck = 1;

		public bool Endgame;

		public float Chips_Amount = 500000f;

		public List<string> CasinoSlotsNames = new List<string> { "vw_prop_casino_slot_01a", "vw_prop_casino_slot_02a", "vw_prop_casino_slot_03a", "vw_prop_casino_slot_04a", "vw_prop_casino_slot_05a", "vw_prop_casino_slot_06a", "vw_prop_casino_slot_07a", "vw_prop_casino_slot_08a" };

		private List<string> ClubDancerList = new List<string>();

		public Prop PlayerCardA;

		public Prop PlayerCardB;

		public Prop PlayerCardC;

		public Prop PlayerCardD;

		public Prop PlayerCardE;

		public Prop PlayerCardF;

		public List<SlotsClass> SlotLocH = new List<SlotsClass>();

		public List<Prop> SlotMachineProps = new List<Prop>();

		public Prop CurrentSlotMachine;

		private bool FirsttimeST = false;

		public Prop ReelABase;

		public Prop ReelBBase;

		public Prop ReelCBase;

		public int SlotMachineStage;

		public string SlotDict;

		public bool SlotInUse;

		public string ALandedOn;

		public string BLandedOn;

		public string CLandedOn;

		public bool BetOne;

		public bool BetMax;

		public float BetSlot;

		public float ReelA_SpinAmt;

		public float ReelB_SpinAmt;

		public float ReelC_SpinAmt;

		public bool SpinnedReels;

		public bool FinishedReelA;

		public bool FinishedReelB;

		public bool FinishedReelC;

		public SlotsClass CurrentSlotClass;

		public float MinStartBet;

		public float SlotMultiplier;

		public Prop SlotProp1;

		public Prop SlotProp2;

		public Prop SlotProp3;

		public Vector3 uParam1;

		public float fParam2;

		public int StageA;

		public int StageB;

		public int StageC;

		public int RotateTimerA;

		public int RotateTimerB;

		public int RotateTimerC;

		public List<int> SlotMachineSounds = new List<int>();

		public int Scene1;

		public bool Firsttime;

		public Prop RouletteMarker;

		public Prop RouletteMarker1;

		public Prop RouletteMarker2;

		public int RouletteBetsPlaced;

		public float RouletteBet1;

		public string RouletteP1;

		public float RouletteBet2;

		public string RouletteP2;

		public float RouletteBet3;

		public string RouletteP3;

		public bool PlaceBet_R;

		public string BetPos = "None";

		public int BetPosNum = 0;

		public int MarkerNum;

		public Prop MarkerTomove;

		public Prop TableT;

		public int Vic1;

		public int Vic2;

		public int Vic3;

		public bool CreatedMarker;

		public Prop RouletteTableA;

		public Prop RouletteTableB;

		public Prop RouletteTableC;

		public Prop Ball;

		public List<Vector3> PokerTables = new List<Vector3>();

		public List<float> PokerTablesRot = new List<float>();

		public int CurrentTable;

		public Prop CardA;

		public Prop CardB;

		public Prop CardC;

		public List<Vector3> BlackJackTables = new List<Vector3>();

		public List<float> BlackJackRot = new List<float>();

		public List<Vector3> SlotMachines = new List<Vector3>();

		public List<float> SlotMachinesRot = new List<float>();

		public string RoulleteTable = "A";

		public List<Vector3> RouletteTables = new List<Vector3>();

		public List<float> RouletteTablesRot = new List<float>();

		public Class1()
		{
			base.Tick += OnTick;
			base.Aborted += OnShutdown;
			Setup();
		}

		public void Setup()
		{
			DenEnterances.Add(new Enterace(new Vector3(-1224.634f, -1083.173f, 7.2f), new Vector3(1004.439f, -2997.753f, -48.5f), 1));
			foreach (Enterace denEnterance in DenEnterances)
			{
				if (denEnterance != null)
				{
					if (denEnterance.blip != null)
					{
						denEnterance.blip.Remove();
					}
					denEnterance.blip = World.CreateBlip(denEnterance.En);
					denEnterance.blip.Sprite = BlipSprite.TableGames;
					denEnterance.blip.Name = "Illegal Gambling Den";
					denEnterance.blip.IsShortRange = true;
				}
			}
		}

		private void OnShutdown(object sender, EventArgs e)
		{
			if (1 == 0)
			{
				return;
			}
			if (RouletteMarker2 != null)
			{
				RouletteMarker2.Delete();
			}
			if (RouletteMarker1 != null)
			{
				RouletteMarker1.Delete();
			}
			if (RouletteMarker != null)
			{
				RouletteMarker.Delete();
			}
			foreach (Ped item in SMPed)
			{
				if (item != null)
				{
					item.Delete();
				}
			}
			foreach (Prop item2 in Tables2)
			{
				if (item2 != null)
				{
					item2.Delete();
				}
			}
			foreach (Prop table in Tables)
			{
				if (table != null)
				{
					table.Delete();
				}
			}
			foreach (Prop chip in Chips)
			{
				if (chip != null)
				{
					chip.Delete();
				}
			}
			foreach (Prop item3 in Tables2)
			{
				if (item3 != null)
				{
					item3.Delete();
				}
			}
			foreach (Prop table2 in Tables)
			{
				if (table2 != null)
				{
					table2.Delete();
				}
			}
			foreach (Prop chip2 in Chips)
			{
				if (chip2 != null)
				{
					chip2.Delete();
				}
			}
			if (ReelABase != null)
			{
				ReelABase.Delete();
			}
			if (ReelBBase != null)
			{
				ReelBBase.Delete();
			}
			if (ReelCBase != null)
			{
				ReelCBase.Delete();
			}
			if (SlotProp1 != null)
			{
				SlotProp1.Delete();
			}
			if (SlotProp2 != null)
			{
				SlotProp2.Delete();
			}
			if (SlotProp3 != null)
			{
				SlotProp3.Delete();
			}
			if (ChipsSpawn != null)
			{
				ChipsSpawn.Delete();
			}
			if (RouletteMarker2 != null)
			{
				RouletteMarker2.Delete();
			}
			if (RouletteMarker1 != null)
			{
				RouletteMarker1.Delete();
			}
			if (RouletteMarker != null)
			{
				RouletteMarker.Delete();
			}
			foreach (Ped item4 in SMPed)
			{
				if (item4 != null)
				{
					item4.Delete();
				}
			}
			foreach (Prop item5 in Tables2)
			{
				if (item5 != null)
				{
					item5.Delete();
				}
			}
			foreach (Prop table3 in Tables)
			{
				if (table3 != null)
				{
					table3.Delete();
				}
			}
			foreach (Prop chip3 in Chips)
			{
				if (chip3 != null)
				{
					chip3.Delete();
				}
			}
			if (Ball != null)
			{
				Ball.Delete();
			}
			if (_8PlayerGameDealer != null)
			{
				_8PlayerGameDealer.Delete();
			}
			if (ChipsSpawn != null)
			{
				ChipsSpawn.Delete();
			}
			if (AI_1 != null)
			{
				AI_1.Delete();
			}
			if (AI_2 != null)
			{
				AI_2.Delete();
			}
			if (AI_3 != null)
			{
				AI_3.Delete();
			}
			if (AI_4 != null)
			{
				AI_4.Delete();
			}
			if (AI_5 != null)
			{
				AI_5.Delete();
			}
			if (AI_6 != null)
			{
				AI_6.Delete();
			}
			if (AI_7 != null)
			{
				AI_7.Delete();
			}
			foreach (Prop _8PlayerGamePokerPotChip in _8PlayerGamePokerPotChips)
			{
				if (_8PlayerGamePokerPotChip != null)
				{
					_8PlayerGamePokerPotChip.Delete();
				}
			}
			if (_8PlayerPokerGameTable1 != null)
			{
				_8PlayerPokerGameTable1.Delete();
			}
			if (_8PlayerPokerGameTable2 != null)
			{
				_8PlayerPokerGameTable2.Delete();
			}
			if (_8PlayerPokerGameTable3 != null)
			{
				_8PlayerPokerGameTable3.Delete();
			}
			foreach (Ped _8PlayerGamePokerPed in _8PlayerGamePokerPeds)
			{
				if (_8PlayerGamePokerPed != null)
				{
					_8PlayerGamePokerPed.Delete();
				}
			}
			foreach (Prop _8PlayerGamePokerProp in _8PlayerGamePokerProps)
			{
				if (_8PlayerGamePokerProp != null)
				{
					_8PlayerGamePokerProp.Delete();
				}
			}
			foreach (Ped item6 in SMPed)
			{
				if (item6 != null)
				{
					item6.Delete();
				}
			}
			foreach (Ped peds in PedsList)
			{
				if (peds != null)
				{
					peds.Delete();
				}
			}
			foreach (Prop item7 in Tables2)
			{
				if (item7 != null)
				{
					item7.Delete();
				}
			}
			foreach (Prop table4 in Tables)
			{
				if (table4 != null)
				{
					table4.Delete();
				}
			}
			foreach (Prop chip4 in Chips)
			{
				if (chip4 != null)
				{
					chip4.Delete();
				}
			}
			foreach (Ped peds2 in PedsList)
			{
				if (peds2 != null)
				{
					peds2.Delete();
				}
			}
			foreach (Prop prop in Props)
			{
				if (prop != null)
				{
					prop.Delete();
				}
			}
			foreach (Enterace denEnterance in DenEnterances)
			{
				if (denEnterance != null && denEnterance.blip != null)
				{
					denEnterance.blip.Remove();
				}
			}
		}

		private void DisplayHelpTextThisFrame(string text)
		{
			InputArgument[] arguments = new InputArgument[1] { "STRING" };
			Function.Call(((Hash)0x8509B634FBE7DA11uL), arguments);
			InputArgument[] arguments2 = new InputArgument[1] { text };
			Function.Call(((Hash)0x6C188BE134E074AAuL), arguments2);
			InputArgument[] arguments3 = new InputArgument[4] { 0, 0, 1, -1 };
			Function.Call(((Hash)0x238FFE5C7B0498A6uL), arguments3);
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

		private void DisplayHelpTextThisFrameNoSound(string text)
		{
			InputArgument[] arguments = new InputArgument[1] { "STRING" };
			Function.Call(((Hash)0x8509B634FBE7DA11uL), arguments);
			InputArgument[] arguments2 = new InputArgument[1] { text };
			Function.Call(((Hash)0x6C188BE134E074AAuL), arguments2);
			InputArgument[] arguments3 = new InputArgument[4] { 0, 0, 0, -1 };
			Function.Call(((Hash)0x238FFE5C7B0498A6uL), arguments3);
		}

		public void ShowPlayerBanks()
		{
			int num = Convert.ToInt32(UIMenu.GetScreenResolutionMantainRatio().Width / 2f);
			if (Player_Bust)
			{
				new UIResText("Player ~o~ BUST ~w", new Point(num + 600, 30), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			}
			if (!Player_Bust && Player_SkipTurn)
			{
				new UIResText("Player ~o~ FOLDED ~w", new Point(num + 600, 30), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			}
			if (!Player_Bust && !Player_SkipTurn)
			{
				new UIResText("Player ~g~$" + _8PlayerGamePoker_PlayerBank.ToString("N"), new Point(num + 600, 30), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			}
			if (AI1_Bust)
			{
				new UIResText("AI1 ~o~ BUST ~w", new Point(num + 600, 60), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			}
			if (!AI1_Bust && AI1_SkipTurn)
			{
				new UIResText("AI1 ~o~ FOLDED ~w", new Point(num + 600, 60), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			}
			if (!AI1_Bust && !AI1_SkipTurn)
			{
				new UIResText("AI1 ~g~$" + _8PlayerGamePoker_AI1Bank.ToString("N"), new Point(num + 600, 60), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			}
			if (AI2_Bust)
			{
				new UIResText("AI2 ~o~ BUST ~w", new Point(num + 600, 90), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			}
			if (!AI2_Bust && AI2_SkipTurn)
			{
				new UIResText("AI2 ~o~ FOLDED ~w", new Point(num + 600, 90), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			}
			if (!AI2_Bust && !AI2_SkipTurn)
			{
				new UIResText("AI2 ~g~$" + _8PlayerGamePoker_AI2Bank.ToString("N"), new Point(num + 600, 90), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			}
			if (AI3_Bust)
			{
				new UIResText("AI3 ~o~ BUST ~w", new Point(num + 600, 120), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			}
			if (!AI3_Bust && AI3_SkipTurn)
			{
				new UIResText("AI3 ~o~ FOLDED ~w", new Point(num + 600, 120), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			}
			if (!AI3_Bust && !AI3_SkipTurn)
			{
				new UIResText("AI3 ~g~$" + _8PlayerGamePoker_AI3Bank.ToString("N"), new Point(num + 600, 120), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			}
			if (AI4_Bust)
			{
				new UIResText("AI4 ~o~ BUST ~w", new Point(num + 600, 150), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			}
			if (!AI4_Bust && AI4_SkipTurn)
			{
				new UIResText("AI4 ~o~ FOLDED ~w", new Point(num + 600, 150), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			}
			if (!AI4_Bust && !AI4_SkipTurn)
			{
				new UIResText("AI4 ~g~$" + _8PlayerGamePoker_AI4Bank.ToString("N"), new Point(num + 600, 150), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			}
			if (AI5_Bust)
			{
				new UIResText("AI5 ~o~ BUST ~w", new Point(num + 600, 180), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			}
			if (!AI5_Bust && AI5_SkipTurn)
			{
				new UIResText("AI5 ~o~ FOLDED ~w", new Point(num + 600, 180), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			}
			if (!AI5_Bust && !AI5_SkipTurn)
			{
				new UIResText("AI5 ~g~$" + _8PlayerGamePoker_AI5Bank.ToString("N"), new Point(num + 600, 180), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			}
			if (AI6_Bust)
			{
				new UIResText("AI6 ~o~ BUST ~w", new Point(num + 600, 210), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			}
			if (!AI6_Bust && AI6_SkipTurn)
			{
				new UIResText("AI6 ~o~ FOLDED ~w", new Point(num + 600, 210), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			}
			if (!AI6_Bust && !AI6_SkipTurn)
			{
				new UIResText("AI6 ~g~$" + _8PlayerGamePoker_AI6Bank.ToString("N"), new Point(num + 600, 210), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			}
			if (AI7_Bust)
			{
				new UIResText("AI7 ~o~ BUST ~w", new Point(num + 600, 240), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			}
			if (!AI7_Bust && AI7_SkipTurn)
			{
				new UIResText("AI7 ~o~ FOLDED ~w", new Point(num + 600, 240), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			}
			if (!AI7_Bust && !AI7_SkipTurn)
			{
				new UIResText("AI7 ~g~$" + _8PlayerGamePoker_AI7Bank.ToString("N"), new Point(num + 600, 240), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			}
			Scaleform scaleform = new Scaleform(0);
			scaleform.Load("instructional_buttons");
			scaleform.Render2D();
		}

		public string RaiseBet(float amt)
		{
			string result = "";
			if (amt == 1f)
			{
				result = "vw_prop_chip_1kdollar_x1";
			}
			if (amt == 5f)
			{
				result = "vw_prop_chip_5kdollar_x1";
			}
			if (amt == 10f)
			{
				result = "vw_prop_chip_10dollar_x1";
			}
			if (amt == 50f)
			{
				result = "vw_prop_chip_50dollar_x1";
			}
			if (amt == 100f)
			{
				result = "vw_prop_chip_100dollar_x1";
			}
			if (amt == 500f)
			{
				result = "vw_prop_chip_500dollar_x1";
			}
			if (amt == 1000f)
			{
				result = "vw_prop_plaq_1kdollar_x1";
			}
			if (amt == 5000f)
			{
				result = "vw_prop_plaq_5kdollar_x1";
			}
			if (amt == 10000f)
			{
				result = "vw_prop_plaq_10kdollar_x1";
			}
			if (amt == 50000f)
			{
				result = "vw_prop_plaq_5kdollar_st";
			}
			if (amt == 100000f)
			{
				result = "vw_prop_plaq_10kdollar_st";
			}
			return result;
		}

		public void RunBet(float P)
		{
			Random random = new Random();
			float num = (float)random.Next(0, 10) / 100f;
			float num2 = (float)random.Next(0, 10) / 100f;
			float num3 = P;
			try
			{
				for (int i = 0; i < 500; i++)
				{
					List<Vector3> list = new List<Vector3>();
					Vector3 offsetInWorldCoords = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.3f, 0.15f, StackChipHeight));
					list.Add(offsetInWorldCoords);
					Vector3 offsetInWorldCoords2 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.2f, 0.15f, StackChipHeight));
					list.Add(offsetInWorldCoords2);
					Vector3 offsetInWorldCoords3 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.1f, 0.15f, StackChipHeight));
					list.Add(offsetInWorldCoords3);
					Vector3 offsetInWorldCoords4 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-0f, 0.15f, StackChipHeight));
					list.Add(offsetInWorldCoords4);
					Vector3 offsetInWorldCoords5 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-0.1f, 0.15f, StackChipHeight));
					list.Add(offsetInWorldCoords5);
					Vector3 offsetInWorldCoords6 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.3f, 0.05f, StackChipHeight));
					list.Add(offsetInWorldCoords6);
					Vector3 offsetInWorldCoords7 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.2f, 0.05f, StackChipHeight));
					list.Add(offsetInWorldCoords7);
					Vector3 offsetInWorldCoords8 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.1f, 0.05f, StackChipHeight));
					list.Add(offsetInWorldCoords8);
					Vector3 offsetInWorldCoords9 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-0f, 0.05f, StackChipHeight));
					list.Add(offsetInWorldCoords9);
					Vector3 offsetInWorldCoords10 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-0.1f, 0.05f, StackChipHeight));
					list.Add(offsetInWorldCoords10);
					Vector3 offsetInWorldCoords11 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.3f, -0.05f, StackChipHeight));
					list.Add(offsetInWorldCoords11);
					Vector3 offsetInWorldCoords12 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.2f, -0.05f, StackChipHeight));
					list.Add(offsetInWorldCoords12);
					Vector3 offsetInWorldCoords13 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.1f, -0.05f, StackChipHeight));
					list.Add(offsetInWorldCoords13);
					Vector3 offsetInWorldCoords14 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-0f, -0.05f, StackChipHeight));
					list.Add(offsetInWorldCoords14);
					Vector3 offsetInWorldCoords15 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-0.1f, -0.05f, StackChipHeight));
					list.Add(offsetInWorldCoords15);
					Vector3 offsetInWorldCoords16 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.3f, -0.15f, StackChipHeight));
					list.Add(offsetInWorldCoords16);
					Vector3 offsetInWorldCoords17 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.2f, -0.15f, StackChipHeight));
					list.Add(offsetInWorldCoords17);
					Vector3 offsetInWorldCoords18 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.1f, -0.15f, StackChipHeight));
					list.Add(offsetInWorldCoords18);
					Vector3 offsetInWorldCoords19 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-0f, -0.15f, StackChipHeight));
					list.Add(offsetInWorldCoords19);
					Vector3 offsetInWorldCoords20 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-0.1f, -0.15f, StackChipHeight));
					list.Add(offsetInWorldCoords20);
					StackChip++;
					if (StackChip >= 19)
					{
						StackChipHeight += 0.005f;
						list[StackChip] = new Vector3(list[StackChip].X, list[StackChip].Y, list[StackChip].Z + StackChipHeight);
						StackChip = 0;
					}
					Script.Wait(250);
					if (_8PlayerGamePokerPotChips.Count > 0)
					{
						foreach (Prop _8PlayerGamePokerPotChip in _8PlayerGamePokerPotChips)
						{
							if (_8PlayerGamePokerPotChip != null)
							{
								if (LastChip != null)
								{
									LastChip.SetNoCollision(_8PlayerGamePokerPotChip, true);
									LastChip.SetNoCollision(Deck, true);
								}
								_8PlayerGamePokerPotChip.SetNoCollision(Deck, true);
								Deck.SetNoCollision(_8PlayerGamePokerPotChip, true);
							}
						}
					}
					if (num3 - 10000f >= 0f)
					{
						num3 -= 10000f;
						Prop prop = World.CreateProp(RequestModel(RaiseBet(10000f)), list[StackChip], true, false);
						SetPropCanFall(prop);
						_8PlayerGamePokerPotChips.Add(prop);
						prop.IsPositionFrozen = false;
						prop.Heading = _8PlayerPokerGameTable3.Heading;
					}
					else if (num3 - 5000f >= 0f)
					{
						num3 -= 5000f;
						Prop prop2 = World.CreateProp(RequestModel(RaiseBet(5000f)), list[StackChip], true, false);
						SetPropCanFall(prop2);
						_8PlayerGamePokerPotChips.Add(prop2);
						prop2.IsPositionFrozen = false;
						prop2.Heading = _8PlayerPokerGameTable3.Heading;
					}
					else if (num3 - 1000f >= 0f)
					{
						num3 -= 1000f;
						Prop prop3 = World.CreateProp(RequestModel(RaiseBet(1000f)), list[StackChip], true, false);
						SetPropCanFall(prop3);
						_8PlayerGamePokerPotChips.Add(prop3);
						prop3.IsPositionFrozen = false;
						prop3.Heading = _8PlayerPokerGameTable3.Heading;
					}
					else if (num3 - 500f >= 0f)
					{
						num3 -= 500f;
						Prop prop4 = World.CreateProp(RequestModel(RaiseBet(500f)), list[StackChip], true, false);
						SetPropCanFall(prop4);
						_8PlayerGamePokerPotChips.Add(prop4);
						prop4.IsPositionFrozen = false;
						prop4.Heading = _8PlayerPokerGameTable3.Heading;
					}
					else if (num3 - 100f >= 0f)
					{
						num3 -= 100f;
						Prop prop5 = World.CreateProp(RequestModel(RaiseBet(100f)), list[StackChip], true, false);
						SetPropCanFall(prop5);
						_8PlayerGamePokerPotChips.Add(prop5);
						prop5.IsPositionFrozen = false;
						prop5.Heading = _8PlayerPokerGameTable3.Heading;
					}
					else if (num3 - 50f >= 0f)
					{
						num3 -= 50f;
						Prop prop6 = World.CreateProp(RequestModel(RaiseBet(50f)), list[StackChip], true, false);
						SetPropCanFall(prop6);
						_8PlayerGamePokerPotChips.Add(prop6);
						prop6.IsPositionFrozen = false;
						prop6.Heading = _8PlayerPokerGameTable3.Heading;
					}
					else if (num3 - 10f >= 0f)
					{
						num3 -= 10f;
						Prop prop7 = World.CreateProp(RequestModel(RaiseBet(10f)), list[StackChip], true, false);
						SetPropCanFall(prop7);
						_8PlayerGamePokerPotChips.Add(prop7);
						prop7.IsPositionFrozen = false;
						prop7.Heading = _8PlayerPokerGameTable3.Heading;
					}
					else if (num3 - 5f >= 0f)
					{
						num3 -= 5f;
						Prop prop8 = World.CreateProp(RequestModel(RaiseBet(5f)), list[StackChip], true, false);
						SetPropCanFall(prop8);
						_8PlayerGamePokerPotChips.Add(prop8);
						prop8.IsPositionFrozen = false;
						prop8.Heading = _8PlayerPokerGameTable3.Heading;
					}
					else if (num3 - 1f >= 0f)
					{
						num3 -= 1f;
						Prop prop9 = World.CreateProp(RequestModel(RaiseBet(1f)), list[StackChip], true, false);
						SetPropCanFall(prop9);
						_8PlayerGamePokerPotChips.Add(prop9);
						prop9.IsPositionFrozen = false;
						prop9.Heading = _8PlayerPokerGameTable3.Heading;
					}
					else if (num3 <= 1f)
					{
						break;
					}
				}
			}
			catch
			{
				UI.Notify("Count > 0");
			}
			if (_8PlayerGamePokerPotChips.Count <= 155)
			{
				return;
			}
			UI.Notify("Refreshing Chips!");
			foreach (Prop _8PlayerGamePokerPotChip2 in _8PlayerGamePokerPotChips)
			{
				if (_8PlayerGamePokerPotChip2 != null)
				{
					_8PlayerGamePokerPotChip2.MarkAsNoLongerNeeded();
					_8PlayerGamePokerPotChip2.Delete();
				}
			}
			StackChipHeight = 0.6f;
			StackChip = 0;
			_8PlayerGamePokerPotChips.Clear();
			ReRunBet(_8PlayerGamePokerPot);
		}

		public void SetPropCanFall(Prop F)
		{
			F.HasGravity = true;
			F.IsPositionFrozen = false;
			F.LodDistance = 200;
			F.ApplyForce(Vector3.WorldDown);
		}

		public void ReRunBet(float P)
		{
			Random random = new Random();
			float num = (float)random.Next(0, 10) / 100f;
			float num2 = (float)random.Next(0, 10) / 100f;
			float num3 = P;
			try
			{
				for (int i = 0; i < 500; i++)
				{
					List<Vector3> list = new List<Vector3>();
					Vector3 offsetInWorldCoords = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.3f, 0.15f, StackChipHeight));
					list.Add(offsetInWorldCoords);
					Vector3 offsetInWorldCoords2 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.2f, 0.15f, StackChipHeight));
					list.Add(offsetInWorldCoords2);
					Vector3 offsetInWorldCoords3 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.1f, 0.15f, StackChipHeight));
					list.Add(offsetInWorldCoords3);
					Vector3 offsetInWorldCoords4 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-0f, 0.15f, StackChipHeight));
					list.Add(offsetInWorldCoords4);
					Vector3 offsetInWorldCoords5 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-0.1f, 0.15f, StackChipHeight));
					list.Add(offsetInWorldCoords5);
					Vector3 offsetInWorldCoords6 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.3f, 0.05f, StackChipHeight));
					list.Add(offsetInWorldCoords6);
					Vector3 offsetInWorldCoords7 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.2f, 0.05f, StackChipHeight));
					list.Add(offsetInWorldCoords7);
					Vector3 offsetInWorldCoords8 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.1f, 0.05f, StackChipHeight));
					list.Add(offsetInWorldCoords8);
					Vector3 offsetInWorldCoords9 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-0f, 0.05f, StackChipHeight));
					list.Add(offsetInWorldCoords9);
					Vector3 offsetInWorldCoords10 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-0.1f, 0.05f, StackChipHeight));
					list.Add(offsetInWorldCoords10);
					Vector3 offsetInWorldCoords11 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.3f, -0.05f, StackChipHeight));
					list.Add(offsetInWorldCoords11);
					Vector3 offsetInWorldCoords12 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.2f, -0.05f, StackChipHeight));
					list.Add(offsetInWorldCoords12);
					Vector3 offsetInWorldCoords13 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.1f, -0.05f, StackChipHeight));
					list.Add(offsetInWorldCoords13);
					Vector3 offsetInWorldCoords14 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-0f, -0.05f, StackChipHeight));
					list.Add(offsetInWorldCoords14);
					Vector3 offsetInWorldCoords15 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-0.1f, -0.05f, StackChipHeight));
					list.Add(offsetInWorldCoords15);
					Vector3 offsetInWorldCoords16 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.3f, -0.15f, StackChipHeight));
					list.Add(offsetInWorldCoords16);
					Vector3 offsetInWorldCoords17 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.2f, -0.15f, StackChipHeight));
					list.Add(offsetInWorldCoords17);
					Vector3 offsetInWorldCoords18 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.1f, -0.15f, StackChipHeight));
					list.Add(offsetInWorldCoords18);
					Vector3 offsetInWorldCoords19 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-0f, -0.15f, StackChipHeight));
					list.Add(offsetInWorldCoords19);
					Vector3 offsetInWorldCoords20 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-0.1f, -0.15f, StackChipHeight));
					list.Add(offsetInWorldCoords20);
					StackChip++;
					if (StackChip >= 19)
					{
						StackChipHeight += 0.005f;
						list[StackChip] = new Vector3(list[StackChip].X, list[StackChip].Y, list[StackChip].Z + StackChipHeight);
						StackChip = 0;
					}
					Script.Wait(250);
					if (_8PlayerGamePokerPotChips.Count > 0)
					{
						foreach (Prop _8PlayerGamePokerPotChip in _8PlayerGamePokerPotChips)
						{
							if (_8PlayerGamePokerPotChip != null)
							{
								if (LastChip != null)
								{
									LastChip.SetNoCollision(_8PlayerGamePokerPotChip, true);
									LastChip.SetNoCollision(Deck, true);
								}
								_8PlayerGamePokerPotChip.SetNoCollision(Deck, true);
								Deck.SetNoCollision(_8PlayerGamePokerPotChip, true);
							}
						}
					}
					if (num3 - 10000f >= 0f)
					{
						num3 -= 10000f;
						Prop prop = World.CreateProp(RequestModel(RaiseBet(10000f)), list[StackChip], true, false);
						SetPropCanFall(prop);
						_8PlayerGamePokerPotChips.Add(prop);
						prop.IsPositionFrozen = false;
						prop.Heading = _8PlayerPokerGameTable3.Heading;
					}
					else if (num3 - 5000f >= 0f)
					{
						num3 -= 5000f;
						Prop prop2 = World.CreateProp(RequestModel(RaiseBet(5000f)), list[StackChip], true, false);
						SetPropCanFall(prop2);
						_8PlayerGamePokerPotChips.Add(prop2);
						prop2.IsPositionFrozen = false;
						prop2.Heading = _8PlayerPokerGameTable3.Heading;
					}
					else if (num3 - 1000f >= 0f)
					{
						num3 -= 1000f;
						Prop prop3 = World.CreateProp(RequestModel(RaiseBet(1000f)), list[StackChip], true, false);
						SetPropCanFall(prop3);
						_8PlayerGamePokerPotChips.Add(prop3);
						prop3.IsPositionFrozen = false;
						prop3.Heading = _8PlayerPokerGameTable3.Heading;
					}
					else if (num3 - 500f >= 0f)
					{
						num3 -= 500f;
						Prop prop4 = World.CreateProp(RequestModel(RaiseBet(500f)), list[StackChip], true, false);
						SetPropCanFall(prop4);
						_8PlayerGamePokerPotChips.Add(prop4);
						prop4.IsPositionFrozen = false;
						prop4.Heading = _8PlayerPokerGameTable3.Heading;
					}
					else if (num3 - 100f >= 0f)
					{
						num3 -= 100f;
						Prop prop5 = World.CreateProp(RequestModel(RaiseBet(100f)), list[StackChip], true, false);
						SetPropCanFall(prop5);
						_8PlayerGamePokerPotChips.Add(prop5);
						prop5.IsPositionFrozen = false;
						prop5.Heading = _8PlayerPokerGameTable3.Heading;
					}
					else if (num3 - 50f >= 0f)
					{
						num3 -= 50f;
						Prop prop6 = World.CreateProp(RequestModel(RaiseBet(50f)), list[StackChip], true, false);
						SetPropCanFall(prop6);
						_8PlayerGamePokerPotChips.Add(prop6);
						prop6.IsPositionFrozen = false;
						prop6.Heading = _8PlayerPokerGameTable3.Heading;
					}
					else if (num3 - 10f >= 0f)
					{
						num3 -= 10f;
						Prop prop7 = World.CreateProp(RequestModel(RaiseBet(10f)), list[StackChip], true, false);
						SetPropCanFall(prop7);
						_8PlayerGamePokerPotChips.Add(prop7);
						prop7.IsPositionFrozen = false;
						prop7.Heading = _8PlayerPokerGameTable3.Heading;
					}
					else if (num3 - 5f >= 0f)
					{
						num3 -= 5f;
						Prop prop8 = World.CreateProp(RequestModel(RaiseBet(5f)), list[StackChip], true, false);
						SetPropCanFall(prop8);
						_8PlayerGamePokerPotChips.Add(prop8);
						prop8.IsPositionFrozen = false;
						prop8.Heading = _8PlayerPokerGameTable3.Heading;
					}
					else if (num3 - 1f >= 0f)
					{
						num3 -= 1f;
						Prop prop9 = World.CreateProp(RequestModel(RaiseBet(1f)), list[StackChip], true, false);
						SetPropCanFall(prop9);
						_8PlayerGamePokerPotChips.Add(prop9);
						prop9.IsPositionFrozen = false;
						prop9.Heading = _8PlayerPokerGameTable3.Heading;
					}
					else if (num3 <= 1f)
					{
						break;
					}
				}
			}
			catch
			{
				UI.Notify("Count > 0");
			}
		}

		public void ShowPlayerTotals()
		{
			int num = Convert.ToInt32(UIMenu.GetScreenResolutionMantainRatio().Width / 2f);
			new UIResText("Player Total ~g~" + PlayerTotal, new Point(num + 600, 280), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			new UIResText("AI1 Total ~g~" + AI1Total, new Point(num + 600, 310), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			new UIResText("AI2 Total ~g~" + AI2Total, new Point(num + 600, 340), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			new UIResText("AI3 Total ~g~" + AI3Total, new Point(num + 600, 370), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			new UIResText("AI4 Total ~g~" + AI4Total, new Point(num + 600, 400), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			new UIResText("AI5 Total ~g~" + AI5Total, new Point(num + 600, 430), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			new UIResText("AI6 Total ~g~" + AI6Total, new Point(num + 600, 460), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			new UIResText("AI7 Total ~g~" + AI7Total, new Point(num + 600, 490), 0.65f, Color.White, GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			Scaleform scaleform = new Scaleform(0);
			scaleform.Load("instructional_buttons");
			scaleform.Render2D();
		}

		public void CheckPokerWin()
		{
			List<float> list = new List<float>();
			if (!AI1_Bust && !AI1_SkipTurn)
			{
				list.Add(AI1Total);
			}
			if (!AI2_Bust && !AI2_SkipTurn)
			{
				list.Add(AI2Total);
			}
			if (!AI3_Bust && !AI3_SkipTurn)
			{
				list.Add(AI3Total);
			}
			if (!AI4_Bust && !AI4_SkipTurn)
			{
				list.Add(AI4Total);
			}
			if (!AI5_Bust && !AI5_SkipTurn)
			{
				list.Add(AI5Total);
			}
			if (!AI6_Bust && !AI6_SkipTurn)
			{
				list.Add(AI6Total);
			}
			if (!AI7_Bust && !AI7_SkipTurn)
			{
				list.Add(AI7Total);
			}
			if (!Player_Bust && !Player_SkipTurn)
			{
				list.Add(PlayerTotal);
			}
			float num = 0f;
			foreach (float item in list)
			{
				if (item >= num)
				{
					num = item;
				}
			}
			if (num == AI1Total)
			{
				_8PlayerGamePoker_AI1Bank += _8PlayerGamePokerPot;
				UI.Notify("AI 1, Won with : Cards, " + GetCardName(PlayerCards[0].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[0].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[0].Card3ID));
				_8PlayerGameStage = 1;
			}
			if (num == AI2Total)
			{
				_8PlayerGamePoker_AI2Bank += _8PlayerGamePokerPot;
				UI.Notify("AI 2, Won with : Cards, " + GetCardName(PlayerCards[1].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[1].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[1].Card3ID));
				_8PlayerGameStage = 1;
			}
			if (num == AI3Total)
			{
				_8PlayerGamePoker_AI3Bank += _8PlayerGamePokerPot;
				UI.Notify("AI 3, Won with : Cards, " + GetCardName(PlayerCards[2].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[2].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[2].Card3ID));
				_8PlayerGameStage = 1;
			}
			if (num == AI4Total)
			{
				_8PlayerGamePoker_AI4Bank += _8PlayerGamePokerPot;
				UI.Notify("AI 4, Won with : Cards, " + GetCardName(PlayerCards[3].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[3].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[3].Card3ID));
				_8PlayerGameStage = 1;
			}
			if (num == AI5Total)
			{
				_8PlayerGamePoker_AI5Bank += _8PlayerGamePokerPot;
				UI.Notify("AI 5, Won with : Cards, " + GetCardName(PlayerCards[4].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[4].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[4].Card3ID));
				_8PlayerGameStage = 1;
			}
			if (num == AI6Total)
			{
				_8PlayerGamePoker_AI6Bank += _8PlayerGamePokerPot;
				UI.Notify("AI 6, Won with : Cards, " + GetCardName(PlayerCards[5].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[5].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[5].Card3ID));
				_8PlayerGameStage = 1;
			}
			if (num == AI7Total)
			{
				_8PlayerGamePoker_AI7Bank += _8PlayerGamePokerPot;
				UI.Notify("AI 7, Won with : Cards, " + GetCardName(PlayerCards[6].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[6].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[6].Card3ID));
				_8PlayerGameStage = 1;
			}
			if (num == PlayerTotal)
			{
				_8PlayerGamePoker_PlayerBank += _8PlayerGamePokerPot;
				UI.Notify("Player, Won with : Cards, " + GetCardName(PlayerCards[7].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[7].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[7].Card3ID));
				_8PlayerGameStage = 1;
			}
			UI.Notify("Commuity Cards Were, " + CommunityCard1ID + "~y~,~w~" + CommunityCard2ID + "~y~,~w~" + CommunityCard3ID);
		}

		public void CheckPokerBuyOut()
		{
			List<float> list = new List<float>();
			if (!AI1_Bust && !AI1_SkipTurn)
			{
				list.Add(AI1Total);
			}
			if (!AI2_Bust && !AI2_SkipTurn)
			{
				list.Add(AI2Total);
			}
			if (!AI3_Bust && !AI3_SkipTurn)
			{
				list.Add(AI3Total);
			}
			if (!AI4_Bust && !AI4_SkipTurn)
			{
				list.Add(AI4Total);
			}
			if (!AI5_Bust && !AI5_SkipTurn)
			{
				list.Add(AI5Total);
			}
			if (!AI6_Bust && !AI6_SkipTurn)
			{
				list.Add(AI6Total);
			}
			if (!AI7_Bust && !AI7_SkipTurn)
			{
				list.Add(AI7Total);
			}
			if (!Player_Bust && !Player_SkipTurn)
			{
				list.Add(PlayerTotal);
			}
			float num = 0f;
			num = list[0];
			if (list.Count == 1)
			{
				if (num == AI1Total)
				{
					_8PlayerGamePoker_AI1Bank += _8PlayerGamePokerPot;
					UI.Notify("AI 1, Won with : Cards, " + GetCardName(PlayerCards[0].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[0].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[0].Card3ID));
					_8PlayerGameStage = 1;
				}
				if (num == AI2Total)
				{
					_8PlayerGamePoker_AI2Bank += _8PlayerGamePokerPot;
					UI.Notify("AI 2, Won with : Cards, " + GetCardName(PlayerCards[1].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[1].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[1].Card3ID));
					_8PlayerGameStage = 1;
				}
				if (num == AI3Total)
				{
					_8PlayerGamePoker_AI3Bank += _8PlayerGamePokerPot;
					UI.Notify("AI 3, Won with : Cards, " + GetCardName(PlayerCards[2].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[2].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[2].Card3ID));
					_8PlayerGameStage = 1;
				}
				if (num == AI4Total)
				{
					_8PlayerGamePoker_AI4Bank += _8PlayerGamePokerPot;
					UI.Notify("AI 4, Won with : Cards, " + GetCardName(PlayerCards[3].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[3].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[3].Card3ID));
					_8PlayerGameStage = 1;
				}
				if (num == AI5Total)
				{
					_8PlayerGamePoker_AI5Bank += _8PlayerGamePokerPot;
					UI.Notify("AI 5, Won with : Cards, " + GetCardName(PlayerCards[4].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[4].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[4].Card3ID));
					_8PlayerGameStage = 1;
				}
				if (num == AI6Total)
				{
					_8PlayerGamePoker_AI6Bank += _8PlayerGamePokerPot;
					UI.Notify("AI 6, Won with : Cards, " + GetCardName(PlayerCards[5].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[5].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[5].Card3ID));
					_8PlayerGameStage = 1;
				}
				if (num == AI7Total)
				{
					_8PlayerGamePoker_AI7Bank += _8PlayerGamePokerPot;
					UI.Notify("AI 7, Won with : Cards, " + GetCardName(PlayerCards[6].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[6].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[6].Card3ID));
					_8PlayerGameStage = 1;
				}
				if (num == PlayerTotal)
				{
					_8PlayerGamePoker_PlayerBank += _8PlayerGamePokerPot;
					UI.Notify("Player, Won with : Cards, " + GetCardName(PlayerCards[7].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[7].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[7].Card3ID));
					_8PlayerGameStage = 1;
				}
				UI.Notify("Commuity Cards Were, " + CommunityCard1ID + "~y~,~w~" + CommunityCard2ID + "~y~,~w~" + CommunityCard3ID);
			}
		}

		public bool GetFoldCauseHighBet(float Bet, float Balance)
		{
			bool result = false;
			Random random = new Random();
			int num = 0;
			if (Balance / 100f * 5f > Bet && Balance / 100f * 10f < Bet)
			{
				num = random.Next(0, 100);
				if (num < 3)
				{
					result = true;
				}
			}
			if (Balance / 100f * 10f > Bet && Balance / 100f * 15f < Bet)
			{
				num = random.Next(0, 100);
				if (num < 10)
				{
					result = true;
				}
			}
			if (Balance / 100f * 15f > Bet && Balance / 100f * 20f < Bet)
			{
				num = random.Next(0, 100);
				if (num < 15)
				{
					result = true;
				}
			}
			if (Balance / 100f * 20f > Bet && Balance / 100f * 100f < Bet)
			{
				num = random.Next(0, 100);
				if (num < 25)
				{
					result = true;
				}
			}
			return result;
		}

		public bool GetFoldCauseLowScore(float Score)
		{
			bool result = false;
			Random random = new Random();
			int num = 0;
			if (Score <= 150f)
			{
				num = random.Next(0, 100);
				if (num < 65)
				{
					result = true;
				}
			}
			if (Score > 150f && Score <= 350f)
			{
				num = random.Next(0, 100);
				if (num < 45)
				{
					result = true;
				}
			}
			if (Score > 350f && Score <= 600f)
			{
				num = random.Next(0, 100);
				if (num < 30)
				{
					result = true;
				}
			}
			if (Score > 600f && Score <= 900f)
			{
				num = random.Next(0, 100);
				if (num < 15)
				{
					result = true;
				}
			}
			if (Score > 900f && Score <= 1400f)
			{
				num = random.Next(0, 100);
				if (num < 7)
				{
					result = true;
				}
			}
			if (Score > 1400f && Score < 2000f)
			{
				num = random.Next(0, 100);
				if (num < 4)
				{
					result = true;
				}
			}
			if (Score >= 2000f)
			{
				num = random.Next(0, 100);
				if (num == 1)
				{
					result = true;
				}
			}
			return result;
		}

		public float GetPlayerTotal(string Card1, string Card2, string Card3)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			string text = Card1;
			if (text.Contains("queen") || text.Contains("q_a"))
			{
				num2 = 13f;
			}
			if (text.Contains("king") || text.Contains("k_a"))
			{
				num2 = 12f;
			}
			if (text.Contains("jack") || text.Contains("j_a"))
			{
				num2 = 11f;
			}
			if (text.Contains("ace") || text.Contains("a_a"))
			{
				num2 = 14f;
			}
			if (text.Contains("02"))
			{
				num2 = 2f;
			}
			if (text.Contains("03"))
			{
				num2 = 3f;
			}
			if (text.Contains("04"))
			{
				num2 = 4f;
			}
			if (text.Contains("05"))
			{
				num2 = 5f;
			}
			if (text.Contains("06"))
			{
				num2 = 6f;
			}
			if (text.Contains("07"))
			{
				num2 = 7f;
			}
			if (text.Contains("08"))
			{
				num2 = 8f;
			}
			if (text.Contains("09"))
			{
				num2 = 9f;
			}
			if (text.Contains("10"))
			{
				num2 = 10f;
			}
			text = Card2;
			if (text.Contains("queen") || text.Contains("q_a"))
			{
				num3 = 13f;
			}
			if (text.Contains("king") || text.Contains("k_a"))
			{
				num3 = 12f;
			}
			if (text.Contains("jack") || text.Contains("j_a"))
			{
				num3 = 11f;
			}
			if (text.Contains("ace") || text.Contains("a_a"))
			{
				num3 = 14f;
			}
			if (text.Contains("02"))
			{
				num3 = 2f;
			}
			if (text.Contains("03"))
			{
				num3 = 3f;
			}
			if (text.Contains("04"))
			{
				num3 = 4f;
			}
			if (text.Contains("05"))
			{
				num3 = 5f;
			}
			if (text.Contains("06"))
			{
				num3 = 6f;
			}
			if (text.Contains("07"))
			{
				num3 = 7f;
			}
			if (text.Contains("08"))
			{
				num3 = 8f;
			}
			if (text.Contains("09"))
			{
				num3 = 9f;
			}
			if (text.Contains("10"))
			{
				num3 = 10f;
			}
			text = Card3;
			if (text.Contains("queen") || text.Contains("q_a"))
			{
				num4 = 13f;
			}
			if (text.Contains("king") || text.Contains("k_a"))
			{
				num4 = 12f;
			}
			if (text.Contains("jack") || text.Contains("j_a"))
			{
				num4 = 11f;
			}
			if (text.Contains("ace") || text.Contains("a_a"))
			{
				num4 = 14f;
			}
			if (text.Contains("02"))
			{
				num4 = 2f;
			}
			if (text.Contains("03"))
			{
				num4 = 3f;
			}
			if (text.Contains("04"))
			{
				num4 = 4f;
			}
			if (text.Contains("05"))
			{
				num4 = 5f;
			}
			if (text.Contains("06"))
			{
				num4 = 6f;
			}
			if (text.Contains("07"))
			{
				num4 = 7f;
			}
			if (text.Contains("08"))
			{
				num4 = 8f;
			}
			if (text.Contains("09"))
			{
				num4 = 9f;
			}
			if (text.Contains("10"))
			{
				num4 = 10f;
			}
			if (CommunityCard1Value == num2)
			{
				num2 *= 2f;
			}
			if (CommunityCard1Value == num3)
			{
				num3 *= 2f;
			}
			if (CommunityCard1Value == num4)
			{
				num4 *= 2f;
			}
			if (CommunityCard2Value == num2)
			{
				num2 *= 2f;
			}
			if (CommunityCard2Value == num3)
			{
				num3 *= 2f;
			}
			if (CommunityCard2Value == num4)
			{
				num4 *= 2f;
			}
			if (CommunityCard3Value == num2)
			{
				num2 *= 2f;
			}
			if (CommunityCard3Value == num3)
			{
				num3 *= 2f;
			}
			if (CommunityCard3Value == num4)
			{
				num4 *= 2f;
			}
			return num2 * num3 * num4;
		}

		public float GetCardTotal(string Card1)
		{
			float num = 0f;
			float result = 0f;
			if (Card1.Contains("queen") || Card1.Contains("q_a"))
			{
				result = 13f;
			}
			if (Card1.Contains("king") || Card1.Contains("k_a"))
			{
				result = 12f;
			}
			if (Card1.Contains("jack") || Card1.Contains("j_a"))
			{
				result = 11f;
			}
			if (Card1.Contains("ace") || Card1.Contains("a_a"))
			{
				result = 14f;
			}
			if (Card1.Contains("02"))
			{
				result = 2f;
			}
			if (Card1.Contains("03"))
			{
				result = 3f;
			}
			if (Card1.Contains("04"))
			{
				result = 4f;
			}
			if (Card1.Contains("05"))
			{
				result = 5f;
			}
			if (Card1.Contains("06"))
			{
				result = 6f;
			}
			if (Card1.Contains("07"))
			{
				result = 7f;
			}
			if (Card1.Contains("08"))
			{
				result = 8f;
			}
			if (Card1.Contains("09"))
			{
				result = 9f;
			}
			if (Card1.Contains("10"))
			{
				result = 10f;
			}
			return result;
		}

		public string GetCardName(string Card1)
		{
			string result = "";
			if (Card1.Contains("queen") || Card1.Contains("q_a"))
			{
				result = "Queen";
			}
			if (Card1.Contains("king") || Card1.Contains("k_a"))
			{
				result = "King";
			}
			if (Card1.Contains("jack") || Card1.Contains("j_a"))
			{
				result = "Jack";
			}
			if (Card1.Contains("ace") || Card1.Contains("a_a"))
			{
				result = "Ace";
			}
			if (Card1.Contains("02"))
			{
				result = "2";
			}
			if (Card1.Contains("03"))
			{
				result = "3";
			}
			if (Card1.Contains("04"))
			{
				result = "4";
			}
			if (Card1.Contains("05"))
			{
				result = "5";
			}
			if (Card1.Contains("06"))
			{
				result = "6";
			}
			if (Card1.Contains("07"))
			{
				result = "7";
			}
			if (Card1.Contains("08"))
			{
				result = "8";
			}
			if (Card1.Contains("09"))
			{
				result = "9";
			}
			if (Card1.Contains("10"))
			{
				result = "10";
			}
			return result;
		}

		public string GetEnterAnim()
		{
			string result = "sit_enter_left";
			Random random = new Random();
			int num = random.Next(1, 4);
			if (num == 1)
			{
				result = "sit_enter_left";
			}
			if (num == 2)
			{
				result = "sit_enter_left_side";
			}
			if (num == 3)
			{
				result = "sit_enter_right";
			}
			if (num == 4)
			{
				result = "sit_enter_right_side";
			}
			return result;
		}

		public string GetExitAnim()
		{
			string result = "sit_enter_left";
			Random random = new Random();
			int num = random.Next(1, 2);
			if (num == 1)
			{
				result = "sit_exit_left";
			}
			if (num == 2)
			{
				result = "sit_exit_right";
			}
			return result;
		}

		public void PlaySyncAnim(Ped AI, Entity Center, int SyncScene, string dict, string anim)
		{
			SyncScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
			{
				AI.Position.X,
				AI.Position.Y,
				Center.Position.Z,
				0.0,
				0.0,
				AI.Heading,
				2
			});
			Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
			{
				AI,
				SyncScene,
				LoadDict(dict),
				anim,
				16f,
				-16f,
				0,
				1148846080
			});
			Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
			{
				AI,
				SyncScene,
				anim,
				LoadDict(dict),
				16f,
				-16f,
				0,
				1148846080
			});
		}

		public void PlaySyncExitAnim(Ped AI, Entity Center, int SyncScene, string dict, string anim)
		{
			SyncScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
			{
				AI.Position.X,
				AI.Position.Y,
				Center.Position.Z,
				0.0,
				0.0,
				AI.Heading + 40f,
				2
			});
			Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
			{
				AI,
				SyncScene,
				LoadDict(dict),
				anim,
				16f,
				-16f,
				0,
				1148846080
			});
			Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
			{
				AI,
				SyncScene,
				anim,
				LoadDict(dict),
				16f,
				-16f,
				0,
				1148846080
			});
		}

		public void PlaySyncAnimReverse(Ped AI, Entity Center, int SyncScene, string dict, string anim)
		{
			SyncScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
			{
				AI.Position.X,
				AI.Position.Y,
				Center.Position.Z,
				0.0,
				0.0,
				AI.Heading,
				2
			});
			Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
			{
				AI,
				SyncScene,
				LoadDict(dict),
				anim,
				-16f,
				16,
				0,
				1148846080
			});
			Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
			{
				AI,
				SyncScene,
				anim,
				LoadDict(dict),
				-16f,
				16,
				0,
				1148846080
			});
		}

		public void PlaySyncAnimRotate180(Ped AI, Entity Center, int SyncScene, string dict, string anim)
		{
			SyncScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
			{
				AI.Position.X,
				AI.Position.Y,
				Center.Position.Z,
				0.0,
				0.0,
				AI.Heading - 180f,
				2
			});
			Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
			{
				AI,
				SyncScene,
				LoadDict(dict),
				anim,
				16f,
				-16f,
				0,
				1148846080
			});
			Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
			{
				AI,
				SyncScene,
				anim,
				LoadDict(dict),
				16f,
				-16f,
				0,
				1148846080
			});
		}

		public static string LoadDict(string dict)
		{
			while (!Function.Call<bool>(((Hash)0xD031A9162D01088CuL), new InputArgument[1] { dict }))
			{
				Function.Call(((Hash)0xD3BD40951412FEF6uL), new InputArgument[1] { dict });
				Script.Yield();
			}
			return dict;
		}

		public string GetRandomCardPlayer()
		{
			Random random = new Random();
			List<object> list = new List<object>();
			if (PlayerDeck == 1 || PlayerDeck == 0)
			{
				list.Add("vw_prop_cas_card_club_02");
				list.Add("vw_prop_cas_card_club_03");
				list.Add("vw_prop_cas_card_club_04");
				list.Add("vw_prop_cas_card_club_05");
				list.Add("vw_prop_cas_card_club_06");
				list.Add("vw_prop_cas_card_club_07");
				list.Add("vw_prop_cas_card_club_08");
				list.Add("vw_prop_cas_card_club_09");
				list.Add("vw_prop_cas_card_club_10");
				list.Add("vw_prop_cas_card_club_ace");
				list.Add("vw_prop_cas_card_club_jack");
				list.Add("vw_prop_cas_card_club_king");
				list.Add("vw_prop_cas_card_club_queen");
				list.Add("vw_prop_cas_card_spd_queen");
				list.Add("vw_prop_cas_card_spd_king");
				list.Add("vw_prop_cas_card_spd_jack");
				list.Add("vw_prop_cas_card_spd_ace");
				list.Add("vw_prop_cas_card_spd_10");
				list.Add("vw_prop_cas_card_spd_09");
				list.Add("vw_prop_cas_card_spd_08");
				list.Add("vw_prop_cas_card_spd_07");
				list.Add("vw_prop_cas_card_spd_06");
				list.Add("vw_prop_cas_card_spd_05");
				list.Add("vw_prop_cas_card_spd_04");
				list.Add("vw_prop_cas_card_spd_03");
				list.Add("vw_prop_cas_card_spd_02");
				list.Add("vw_prop_cas_card_hrt_queen");
				list.Add("vw_prop_cas_card_hrt_king");
				list.Add("vw_prop_cas_card_hrt_jack");
				list.Add("vw_prop_cas_card_hrt_ace");
				list.Add("vw_prop_cas_card_hrt_10");
				list.Add("vw_prop_cas_card_hrt_09");
				list.Add("vw_prop_cas_card_hrt_08");
				list.Add("vw_prop_cas_card_hrt_07");
				list.Add("vw_prop_cas_card_hrt_06");
				list.Add("vw_prop_cas_card_hrt_05");
				list.Add("vw_prop_cas_card_hrt_04");
				list.Add("vw_prop_cas_card_hrt_03");
				list.Add("vw_prop_cas_card_hrt_02");
				list.Add("vw_prop_cas_card_dia_queen");
				list.Add("vw_prop_cas_card_dia_king");
				list.Add("vw_prop_cas_card_dia_jack");
				list.Add("vw_prop_cas_card_dia_ace");
				list.Add("vw_prop_cas_card_dia_10");
				list.Add("vw_prop_cas_card_dia_09");
				list.Add("vw_prop_cas_card_dia_08");
				list.Add("vw_prop_cas_card_dia_07");
				list.Add("vw_prop_cas_card_dia_06");
				list.Add("vw_prop_cas_card_dia_05");
				list.Add("vw_prop_cas_card_dia_04");
				list.Add("vw_prop_cas_card_dia_03");
				list.Add("vw_prop_cas_card_dia_02");
			}
			if (PlayerDeck == 2)
			{
				list.Add("vw_prop_vw_club_char_02a");
				list.Add("vw_prop_vw_club_char_03a");
				list.Add("vw_prop_vw_club_char_04a");
				list.Add("vw_prop_vw_club_char_05a");
				list.Add("vw_prop_vw_club_char_06a");
				list.Add("vw_prop_vw_club_char_07a");
				list.Add("vw_prop_vw_club_char_08a");
				list.Add("vw_prop_vw_club_char_09a");
				list.Add("vw_prop_vw_club_char_10a");
				list.Add("vw_prop_vw_club_char_a_a");
				list.Add("vw_prop_vw_club_char_j_a");
				list.Add("vw_prop_vw_club_char_k_a");
				list.Add("vw_prop_vw_club_char_q_a");
				list.Add("vw_prop_vw_dia_char_02a");
				list.Add("vw_prop_vw_dia_char_03a");
				list.Add("vw_prop_vw_dia_char_04a");
				list.Add("vw_prop_vw_dia_char_05a");
				list.Add("vw_prop_vw_dia_char_06a");
				list.Add("vw_prop_vw_dia_char_07a");
				list.Add("vw_prop_vw_dia_char_08a");
				list.Add("vw_prop_vw_dia_char_09a");
				list.Add("vw_prop_vw_dia_char_10a");
				list.Add("vw_prop_vw_dia_char_a_a");
				list.Add("vw_prop_vw_dia_char_j_a");
				list.Add("vw_prop_vw_dia_char_k_a");
				list.Add("vw_prop_vw_dia_char_q_a");
				list.Add("vw_prop_vw_hrt_char_02a");
				list.Add("vw_prop_vw_hrt_char_03a");
				list.Add("vw_prop_vw_hrt_char_04a");
				list.Add("vw_prop_vw_hrt_char_05a");
				list.Add("vw_prop_vw_hrt_char_06a");
				list.Add("vw_prop_vw_hrt_char_07a");
				list.Add("vw_prop_vw_hrt_char_08a");
				list.Add("vw_prop_vw_hrt_char_09a");
				list.Add("vw_prop_vw_hrt_char_10a");
				list.Add("vw_prop_vw_hrt_char_a_a");
				list.Add("vw_prop_vw_hrt_char_j_a");
				list.Add("vw_prop_vw_hrt_char_k_a");
				list.Add("vw_prop_vw_hrt_char_q_a");
				list.Add("vw_prop_vw_spd_char_02a");
				list.Add("vw_prop_vw_spd_char_03a");
				list.Add("vw_prop_vw_spd_char_04a");
				list.Add("vw_prop_vw_spd_char_05a");
				list.Add("vw_prop_vw_spd_char_06a");
				list.Add("vw_prop_vw_spd_char_07a");
				list.Add("vw_prop_vw_spd_char_08a");
				list.Add("vw_prop_vw_spd_char_09a");
				list.Add("vw_prop_vw_spd_char_10a");
				list.Add("vw_prop_vw_spd_char_a_a");
				list.Add("vw_prop_vw_spd_char_j_a");
				list.Add("vw_prop_vw_spd_char_k_a");
				list.Add("vw_prop_vw_spd_char_q_a");
			}
			int index = random.Next(0, list.Count - 1);
			return (dynamic)list[index];
		}

		public string GetRandomCardDealer()
		{
			Random random = new Random();
			List<object> list = new List<object>();
			if (PlayerDeck == 1 || PlayerDeck == 0)
			{
				list.Add("vw_prop_cas_card_club_02");
				list.Add("vw_prop_cas_card_club_03");
				list.Add("vw_prop_cas_card_club_04");
				list.Add("vw_prop_cas_card_club_05");
				list.Add("vw_prop_cas_card_club_06");
				list.Add("vw_prop_cas_card_club_07");
				list.Add("vw_prop_cas_card_club_08");
				list.Add("vw_prop_cas_card_club_09");
				list.Add("vw_prop_cas_card_club_10");
				list.Add("vw_prop_cas_card_club_ace");
				list.Add("vw_prop_cas_card_club_jack");
				list.Add("vw_prop_cas_card_club_king");
				list.Add("vw_prop_cas_card_club_queen");
				list.Add("vw_prop_cas_card_spd_queen");
				list.Add("vw_prop_cas_card_spd_king");
				list.Add("vw_prop_cas_card_spd_jack");
				list.Add("vw_prop_cas_card_spd_ace");
				list.Add("vw_prop_cas_card_spd_10");
				list.Add("vw_prop_cas_card_spd_09");
				list.Add("vw_prop_cas_card_spd_08");
				list.Add("vw_prop_cas_card_spd_07");
				list.Add("vw_prop_cas_card_spd_06");
				list.Add("vw_prop_cas_card_spd_05");
				list.Add("vw_prop_cas_card_spd_04");
				list.Add("vw_prop_cas_card_spd_03");
				list.Add("vw_prop_cas_card_spd_02");
				list.Add("vw_prop_cas_card_hrt_queen");
				list.Add("vw_prop_cas_card_hrt_king");
				list.Add("vw_prop_cas_card_hrt_jack");
				list.Add("vw_prop_cas_card_hrt_ace");
				list.Add("vw_prop_cas_card_hrt_10");
				list.Add("vw_prop_cas_card_hrt_09");
				list.Add("vw_prop_cas_card_hrt_08");
				list.Add("vw_prop_cas_card_hrt_07");
				list.Add("vw_prop_cas_card_hrt_06");
				list.Add("vw_prop_cas_card_hrt_05");
				list.Add("vw_prop_cas_card_hrt_04");
				list.Add("vw_prop_cas_card_hrt_03");
				list.Add("vw_prop_cas_card_hrt_02");
				list.Add("vw_prop_cas_card_dia_queen");
				list.Add("vw_prop_cas_card_dia_king");
				list.Add("vw_prop_cas_card_dia_jack");
				list.Add("vw_prop_cas_card_dia_ace");
				list.Add("vw_prop_cas_card_dia_10");
				list.Add("vw_prop_cas_card_dia_09");
				list.Add("vw_prop_cas_card_dia_08");
				list.Add("vw_prop_cas_card_dia_07");
				list.Add("vw_prop_cas_card_dia_06");
				list.Add("vw_prop_cas_card_dia_05");
				list.Add("vw_prop_cas_card_dia_04");
				list.Add("vw_prop_cas_card_dia_03");
				list.Add("vw_prop_cas_card_dia_02");
			}
			if (PlayerDeck == 2)
			{
				list.Add("vw_prop_vw_club_char_02a");
				list.Add("vw_prop_vw_club_char_03a");
				list.Add("vw_prop_vw_club_char_04a");
				list.Add("vw_prop_vw_club_char_05a");
				list.Add("vw_prop_vw_club_char_06a");
				list.Add("vw_prop_vw_club_char_07a");
				list.Add("vw_prop_vw_club_char_08a");
				list.Add("vw_prop_vw_club_char_09a");
				list.Add("vw_prop_vw_club_char_10a");
				list.Add("vw_prop_vw_club_char_a_a");
				list.Add("vw_prop_vw_club_char_j_a");
				list.Add("vw_prop_vw_club_char_k_a");
				list.Add("vw_prop_vw_club_char_q_a");
				list.Add("vw_prop_vw_dia_char_02a");
				list.Add("vw_prop_vw_dia_char_03a");
				list.Add("vw_prop_vw_dia_char_04a");
				list.Add("vw_prop_vw_dia_char_05a");
				list.Add("vw_prop_vw_dia_char_06a");
				list.Add("vw_prop_vw_dia_char_07a");
				list.Add("vw_prop_vw_dia_char_08a");
				list.Add("vw_prop_vw_dia_char_09a");
				list.Add("vw_prop_vw_dia_char_10a");
				list.Add("vw_prop_vw_dia_char_a_a");
				list.Add("vw_prop_vw_dia_char_j_a");
				list.Add("vw_prop_vw_dia_char_k_a");
				list.Add("vw_prop_vw_dia_char_q_a");
				list.Add("vw_prop_vw_hrt_char_02a");
				list.Add("vw_prop_vw_hrt_char_03a");
				list.Add("vw_prop_vw_hrt_char_04a");
				list.Add("vw_prop_vw_hrt_char_05a");
				list.Add("vw_prop_vw_hrt_char_06a");
				list.Add("vw_prop_vw_hrt_char_07a");
				list.Add("vw_prop_vw_hrt_char_08a");
				list.Add("vw_prop_vw_hrt_char_09a");
				list.Add("vw_prop_vw_hrt_char_10a");
				list.Add("vw_prop_vw_hrt_char_a_a");
				list.Add("vw_prop_vw_hrt_char_j_a");
				list.Add("vw_prop_vw_hrt_char_k_a");
				list.Add("vw_prop_vw_hrt_char_q_a");
				list.Add("vw_prop_vw_spd_char_02a");
				list.Add("vw_prop_vw_spd_char_03a");
				list.Add("vw_prop_vw_spd_char_04a");
				list.Add("vw_prop_vw_spd_char_05a");
				list.Add("vw_prop_vw_spd_char_06a");
				list.Add("vw_prop_vw_spd_char_07a");
				list.Add("vw_prop_vw_spd_char_08a");
				list.Add("vw_prop_vw_spd_char_09a");
				list.Add("vw_prop_vw_spd_char_10a");
				list.Add("vw_prop_vw_spd_char_a_a");
				list.Add("vw_prop_vw_spd_char_j_a");
				list.Add("vw_prop_vw_spd_char_k_a");
				list.Add("vw_prop_vw_spd_char_q_a");
			}
			int index = random.Next(0, list.Count - 1);
			return (dynamic)list[index];
		}

		public void GameFail(string Reason, string MissionName)
		{
			SizeF screenResolutionMantainRatio = UIMenu.GetScreenResolutionMantainRatio();
			int x = Convert.ToInt32(screenResolutionMantainRatio.Width / 2f);
			new Sprite("mpentry", "mp_modenotselected_gradient", new Point(0, 30), new Size(Convert.ToInt32(screenResolutionMantainRatio.Width), 450), 0f, Color.FromArgb(230, 255, 255, 255)).Draw();
			new UIResText(MissionName, new Point(x, 100), 2.5f, Color.FromArgb(255, 148, 27, 46), GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			new UIResText("Reason : " + Reason, new Point(x, 280), 0.5f, Color.White, GTA.UI.Font.ChaletLondon, UIResText.Alignment.Centered).Draw();
			Scaleform scaleform = new Scaleform(0);
			scaleform.Render2D();
		}

		public void GamePassed(string MissionName, int Payout)
		{
			SizeF screenResolutionMantainRatio = UIMenu.GetScreenResolutionMantainRatio();
			int x = Convert.ToInt32(screenResolutionMantainRatio.Width / 2f);
			new Sprite("mpentry", "mp_modenotselected_gradient", new Point(0, 10), new Size(Convert.ToInt32(screenResolutionMantainRatio.Width), 450), 0f, Color.FromArgb(200, 255, 255, 255)).Draw();
			new UIResText(MissionName, new Point(x, 100), 2.5f, Color.FromArgb(255, 199, 168, 87), GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			new UIResText("Payout : $" + Payout.ToString("N"), new Point(x, 300), 0.5f, Color.White, GTA.UI.Font.ChaletLondon, UIResText.Alignment.Centered).Draw();
			Scaleform scaleform = new Scaleform(0);
			scaleform.Render2D();
		}

		public void Poker(string Reason, string MissionName)
		{
			SizeF screenResolutionMantainRatio = UIMenu.GetScreenResolutionMantainRatio();
			int x = Convert.ToInt32(screenResolutionMantainRatio.Width / 2f);
			new Sprite("mpentry", "mp_modenotselected_gradient", new Point(0, 30), new Size(Convert.ToInt32(screenResolutionMantainRatio.Width), 450), 0f, Color.FromArgb(230, 255, 255, 255)).Draw();
			new UIResText(MissionName, new Point(x, 100), 2.5f, Color.FromArgb(255, 148, 27, 46), GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			new UIResText("Reason : " + Reason, new Point(x, 280), 0.5f, Color.White, GTA.UI.Font.ChaletLondon, UIResText.Alignment.Centered).Draw();
			Scaleform scaleform = new Scaleform(0);
			scaleform.Load("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 22, 0 }), "Begin Assault");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 202, 0 }), "Cancel");
			scaleform.CallFunction("ORBITAL_CANNON_CAM", -1);
			scaleform.Render2D();
		}

		public void CalculateBlackJack()
		{
			float num = PlayerCard_1 + PlayerCard_2 + PlayerCard_3 + PlayerCard_4 + PlayerCard_5 + PlayerCard_6;
			float num2 = DealerCard_1 + DealerCard_2 + DealerCard_3 + DealerCard_4 + DealerCard_5 + DealerCard_6;
			Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_male@engaged@01a@play@v02", "collect_chips", 8f, -1, true, -1f);
			UI.Notify("P " + num);
			UI.Notify("D " + num2);
			Drawcards = false;
			if (num2 == 21f && num2 != 21f)
			{
				Game.Player.Money += (int)Bet;
				Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01", "reaction_terrible", 8f, -1, true, -1f);
				for (int i = 0; i < 100; i++)
				{
					GamePassed("BlackJack", (int)Bet);
					Script.Wait(1);
				}
				return;
			}
			if (num2 > 21f && num <= 21f)
			{
				Game.Player.Money += (int)Bet;
				Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01", "reaction_terrible", 8f, -1, true, -1f);
				for (int j = 0; j < 100; j++)
				{
					GamePassed("BlackJack", (int)Bet);
					Script.Wait(1);
				}
				return;
			}
			if (num2 == num && num <= 21f)
			{
				Game.Player.Money += (int)Bet;
				Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01", "reaction_good_var01", 8f, -1, true, -1f);
				for (int k = 0; k < 100; k++)
				{
					GamePassed("BlackJack", (int)Bet);
					Script.Wait(1);
				}
				return;
			}
			if (num > 21f)
			{
				Game.Player.Money -= (int)Bet;
				Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01", "reaction_terrible", 8f, -1, true, -1f);
				for (int l = 0; l < 100; l++)
				{
					GameFail("You went over 21!", "BlackJack");
					Script.Wait(1);
				}
				return;
			}
			if (num < num2 && num2 <= 21f)
			{
				Game.Player.Money += (int)Bet;
				Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01", "reaction_bad_var01", 8f, -1, true, -1f);
				for (int m = 0; m < 100; m++)
				{
					GameFail("The Dealer had a higher Score!", "BlackJack");
					Script.Wait(1);
				}
				return;
			}
			if ((num > num2 && num <= 21f) || num2 > 21f)
			{
				Game.Player.Money += (int)Bet;
				Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01", "reaction_good_var02", 8f, -1, true, -1f);
				for (int n = 0; n < 100; n++)
				{
					GamePassed("BlackJack", (int)Bet);
					Script.Wait(1);
				}
				return;
			}
			if (num > num2 && num2 <= 21f)
			{
				Game.Player.Money += (int)Bet;
				Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01", "reaction_bad_var02", 8f, -1, true, -1f);
				for (int num3 = 0; num3 < 100; num3++)
				{
					GameFail("The Dealer had a better Score!", "BlackJack");
					Script.Wait(1);
				}
				return;
			}
			Script.Wait(200);
			Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_male@engaged@01a@play@v02", "collect_chips", 8f, -1, true, -1f);
			Playercards = 0;
			DealerCards = 0;
			PlayerCard_1 = 0;
			PlayerCard_2 = 0;
			PlayerCard_3 = 0;
			PlayerCard_4 = 0;
			PlayerCard_5 = 0;
			PlayerCard_6 = 0;
			Bet = 0f;
			DealerCard_1 = 0;
			DealerCard_2 = 0;
			DealerCard_3 = 0;
			DealerCard_4 = 0;
			DealerCard_5 = 0;
			DealerCard_6 = 0;
			Endgame = true;
			Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_male@engaged@01a@play@v02", "collect_chips", 8f, -1, true, -1f);
		}

		public void CalculatePoker()
		{
			float num = PlayerCard_1 * PlayerCard_2 * PlayerCard_3;
			float num2 = DealerCard_1 * DealerCard_2 * DealerCard_3;
			Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_male@engaged@01a@play@v02", "collect_chips", 8f, -1, true, -1f);
			UI.Notify("P " + num);
			UI.Notify("D " + num2);
			Drawcards = false;
			if (num > num2)
			{
				Game.Player.Money += (int)Bet;
				Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@threecardpoker@ped_male@engaged@01b@reacts@v01", "reaction_bad_var02", 8f, -1, true, -1f);
				for (int i = 0; i < 100; i++)
				{
					GamePassed("3 Card Poker", (int)Bet);
					Script.Wait(1);
				}
			}
			if (num < num2)
			{
				Game.Player.Money += (int)Bet;
				Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@threecardpoker@ped_male@engaged@01b@reacts@v01", "reaction_good_var02", 8f, -1, true, -1f);
				for (int j = 0; j < 100; j++)
				{
					GameFail("The Dealer had a higher Score!", "3 Card Poker");
					Script.Wait(1);
				}
			}
			Script.Wait(200);
			Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_male@engaged@01a@play@v02", "collect_chips", 8f, -1, true, -1f);
			Playercards = 0;
			DealerCards = 0;
			PlayerCard_1 = 0;
			PlayerCard_2 = 0;
			PlayerCard_3 = 0;
			PlayerCard_4 = 0;
			PlayerCard_5 = 0;
			PlayerCard_6 = 0;
			Bet = 0f;
			DealerCard_1 = 0;
			DealerCard_2 = 0;
			DealerCard_3 = 0;
			DealerCard_4 = 0;
			DealerCard_5 = 0;
			DealerCard_6 = 0;
			Endgame = true;
			Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_male@engaged@01a@play@v02", "collect_chips", 8f, -1, true, -1f);
		}

		public int Set_PlayerCard(string CardId, int Cardnum)
		{
			string text = "num";
			int result = 0;
			if (CardId.Contains("queen") || CardId.Contains("q_a"))
			{
				text = "Queen";
				result = 10;
			}
			if (CardId.Contains("king") || CardId.Contains("k_a"))
			{
				text = "King";
				result = 10;
			}
			if (CardId.Contains("jack") || CardId.Contains("j_a"))
			{
				text = "Jack";
				result = 10;
			}
			if (CardId.Contains("ace") || CardId.Contains("a_a"))
			{
				text = "Ace";
				result = 1;
			}
			if (CardId.Contains("02"))
			{
				result = 2;
			}
			if (CardId.Contains("03"))
			{
				result = 3;
			}
			if (CardId.Contains("04"))
			{
				result = 4;
			}
			if (CardId.Contains("05"))
			{
				result = 5;
			}
			if (CardId.Contains("06"))
			{
				result = 6;
			}
			if (CardId.Contains("07"))
			{
				result = 7;
			}
			if (CardId.Contains("08"))
			{
				result = 8;
			}
			if (CardId.Contains("09"))
			{
				result = 9;
			}
			if (CardId.Contains("10"))
			{
				result = 10;
			}
			if (text.Equals("num"))
			{
				UI.Notify("~g~Player~w~  got Card : " + result);
			}
			else
			{
				UI.Notify("~g~Player~w~ got Card : " + text);
			}
			return result;
		}

		public int Set_DealerCard(string CardId, int Cardnum)
		{
			string text = "num";
			int result = 0;
			if (CardId.Contains("queen") || CardId.Contains("q_a"))
			{
				text = "Queen";
				result = 10;
			}
			if (CardId.Contains("king") || CardId.Contains("k_a"))
			{
				text = "King";
				result = 10;
			}
			if (CardId.Contains("jack") || CardId.Contains("j_a"))
			{
				text = "Jack";
				result = 10;
			}
			if (CardId.Contains("ace") || CardId.Contains("a_a"))
			{
				text = "Ace";
				result = 1;
			}
			if (CardId.Contains("02"))
			{
				result = 2;
			}
			if (CardId.Contains("03"))
			{
				result = 3;
			}
			if (CardId.Contains("04"))
			{
				result = 4;
			}
			if (CardId.Contains("05"))
			{
				result = 5;
			}
			if (CardId.Contains("06"))
			{
				result = 6;
			}
			if (CardId.Contains("07"))
			{
				result = 7;
			}
			if (CardId.Contains("08"))
			{
				result = 8;
			}
			if (CardId.Contains("09"))
			{
				result = 9;
			}
			if (CardId.Contains("10"))
			{
				result = 10;
			}
			if (text.Equals("num"))
			{
				UI.Notify("~p~Dealer~w~ got Card : " + result);
			}
			else
			{
				UI.Notify("~p~Dealer~w~ got Card : " + text);
			}
			return result;
		}

		public int Set_PlayerCardPoker(string CardId, int Cardnum)
		{
			string text = "num";
			int result = 0;
			if (CardId.Contains("queen") || CardId.Contains("q_a"))
			{
				text = "Queen";
				result = 13;
			}
			if (CardId.Contains("king") || CardId.Contains("k_a"))
			{
				text = "King";
				result = 14;
			}
			if (CardId.Contains("jack") || CardId.Contains("j_a"))
			{
				text = "Jack";
				result = 12;
			}
			if (CardId.Contains("ace") || CardId.Contains("a_a"))
			{
				text = "Ace";
				result = 11;
			}
			if (CardId.Contains("02"))
			{
				result = 2;
			}
			if (CardId.Contains("03"))
			{
				result = 3;
			}
			if (CardId.Contains("04"))
			{
				result = 4;
			}
			if (CardId.Contains("05"))
			{
				result = 5;
			}
			if (CardId.Contains("06"))
			{
				result = 6;
			}
			if (CardId.Contains("07"))
			{
				result = 7;
			}
			if (CardId.Contains("08"))
			{
				result = 8;
			}
			if (CardId.Contains("09"))
			{
				result = 9;
			}
			if (CardId.Contains("10"))
			{
				result = 10;
			}
			if (text.Equals("num"))
			{
				UI.Notify("~g~Player~w~  got Card : " + result);
			}
			else
			{
				UI.Notify("~g~Player~w~ got Card : " + text);
			}
			return result;
		}

		public int Set_DealerCardPoker(string CardId, int Cardnum)
		{
			string text = "num";
			int result = 0;
			if (CardId.Contains("queen") || CardId.Contains("q_a"))
			{
				text = "Queen";
				result = 13;
			}
			if (CardId.Contains("king") || CardId.Contains("k_a"))
			{
				text = "King";
				result = 14;
			}
			if (CardId.Contains("jack") || CardId.Contains("j_a"))
			{
				text = "Jack";
				result = 12;
			}
			if (CardId.Contains("ace") || CardId.Contains("a_a"))
			{
				text = "Ace";
				result = 11;
			}
			if (CardId.Contains("02"))
			{
				result = 2;
			}
			if (CardId.Contains("03"))
			{
				result = 3;
			}
			if (CardId.Contains("04"))
			{
				result = 4;
			}
			if (CardId.Contains("05"))
			{
				result = 5;
			}
			if (CardId.Contains("06"))
			{
				result = 6;
			}
			if (CardId.Contains("07"))
			{
				result = 7;
			}
			if (CardId.Contains("08"))
			{
				result = 8;
			}
			if (CardId.Contains("09"))
			{
				result = 9;
			}
			if (CardId.Contains("10"))
			{
				result = 10;
			}
			return result;
		}

		public int Set_8PlayerPokerCards(string CardId, int Cardnum, int PlayerID)
		{
			string text = "num";
			int result = 0;
			if (CardId.Contains("queen") || CardId.Contains("q_a"))
			{
				text = "Queen";
				result = 13;
			}
			if (CardId.Contains("king") || CardId.Contains("k_a"))
			{
				text = "King";
				result = 14;
			}
			if (CardId.Contains("jack") || CardId.Contains("j_a"))
			{
				text = "Jack";
				result = 12;
			}
			if (CardId.Contains("ace") || CardId.Contains("a_a"))
			{
				text = "Ace";
				result = 11;
			}
			if (CardId.Contains("02"))
			{
				result = 2;
			}
			if (CardId.Contains("03"))
			{
				result = 3;
			}
			if (CardId.Contains("04"))
			{
				result = 4;
			}
			if (CardId.Contains("05"))
			{
				result = 5;
			}
			if (CardId.Contains("06"))
			{
				result = 6;
			}
			if (CardId.Contains("07"))
			{
				result = 7;
			}
			if (CardId.Contains("08"))
			{
				result = 8;
			}
			if (CardId.Contains("09"))
			{
				result = 9;
			}
			if (CardId.Contains("10"))
			{
				result = 10;
			}
			return result;
		}

		public void Show_RQE(string R, string Q, string E)
		{
			int num = Convert.ToInt32(UIMenu.GetScreenResolutionMantainRatio().Width / 2f);
			Scaleform scaleform = new Scaleform(1);
			scaleform.Load("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 51, 0 }), E);
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 44, 0 }), Q);
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 80, 0 }), R);
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public void Show_EGQ(string E, string G, string Q)
		{
			int num = Convert.ToInt32(UIMenu.GetScreenResolutionMantainRatio().Width / 2f);
			Scaleform scaleform = new Scaleform(1);
			scaleform.Load("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 51, 0 }), E);
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 58, 0 }), G);
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 44, 0 }), Q);
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public void Show_EGQR(string E, string G, string Q, string R)
		{
			int num = Convert.ToInt32(UIMenu.GetScreenResolutionMantainRatio().Width / 2f);
			Scaleform scaleform = new Scaleform(1);
			scaleform.Load("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 51, 0 }), E);
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 58, 0 }), G);
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 44, 0 }), Q);
			scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 45, 0 }), R);
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public void Show_EGQRX(string E, string G, string Q, string R, string X)
		{
			int num = Convert.ToInt32(UIMenu.GetScreenResolutionMantainRatio().Width / 2f);
			Scaleform scaleform = new Scaleform(1);
			scaleform.Load("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 51, 0 }), E);
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 58, 0 }), G);
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 44, 0 }), Q);
			scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 45, 0 }), R);
			scaleform.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 73, 0 }), X);
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public void Show_Q(string Q)
		{
			int num = Convert.ToInt32(UIMenu.GetScreenResolutionMantainRatio().Width / 2f);
			Scaleform scaleform = new Scaleform(1);
			scaleform.Load("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 44, 0 }), Q);
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public void Show_GQ(string G, string Q)
		{
			int num = Convert.ToInt32(UIMenu.GetScreenResolutionMantainRatio().Width / 2f);
			Scaleform scaleform = new Scaleform(1);
			scaleform.Load("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 58, 0 }), G);
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 44, 0 }), Q);
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public void Show_EQ(string E, string Q)
		{
			int num = Convert.ToInt32(UIMenu.GetScreenResolutionMantainRatio().Width / 2f);
			Scaleform scaleform = new Scaleform(1);
			scaleform.Load("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 51, 0 }), E);
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 44, 0 }), Q);
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public void Show_EQ_Chips(string E, string Q)
		{
			int num = Convert.ToInt32(UIMenu.GetScreenResolutionMantainRatio().Width / 2f);
			Scaleform scaleform = new Scaleform(1);
			scaleform.Load("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 51, 0 }), "Cash " + Chips_Amount + "  |  " + E);
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 44, 0 }), Q);
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public void Show_EG(string E, string G)
		{
			int num = Convert.ToInt32(UIMenu.GetScreenResolutionMantainRatio().Width / 2f);
			Scaleform scaleform = new Scaleform(1);
			scaleform.Load("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 51, 0 }), E);
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 58, 0 }), G);
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public void Roulette_GameFail(string Reason, string vic, string MissionName, int Payout)
		{
			SizeF screenResolutionMantainRatio = UIMenu.GetScreenResolutionMantainRatio();
			int x = Convert.ToInt32(screenResolutionMantainRatio.Width / 2f);
			new Sprite("mpentry", "mp_modenotselected_gradient", new Point(0, 30), new Size(Convert.ToInt32(screenResolutionMantainRatio.Width), 700), 0f, Color.FromArgb(230, 255, 255, 255)).Draw();
			new UIResText(MissionName, new Point(x, 100), 2.5f, Color.FromArgb(255, 148, 27, 46), GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			new UIResText(vic, new Point(x, 200), 1.5f, Color.White, GTA.UI.Font.HouseScript, UIResText.Alignment.Centered).Draw();
			new UIResText("Reason : " + Reason, new Point(x, 280), 0.5f, Color.White, GTA.UI.Font.ChaletLondon, UIResText.Alignment.Centered).Draw();
			new UIResText("Loss : " + Payout.ToString("N") + "c", new Point(x, 340), 0.5f, Color.White, GTA.UI.Font.ChaletLondon, UIResText.Alignment.Centered).Draw();
			Scaleform scaleform = new Scaleform(0);
			scaleform.Render2D();
		}

		public void SetVictory1()
		{
			Vic1 = 1;
		}

		public void SetVictory2()
		{
			Vic2 = 1;
		}

		public void SetVictory3()
		{
			Vic3 = 1;
		}

		public void Roulette_GamePassed(string MissionName, string vic, int Payout, int loss)
		{
			SizeF screenResolutionMantainRatio = UIMenu.GetScreenResolutionMantainRatio();
			int x = Convert.ToInt32(screenResolutionMantainRatio.Width / 2f);
			new Sprite("mpentry", "mp_modenotselected_gradient", new Point(0, 10), new Size(Convert.ToInt32(screenResolutionMantainRatio.Width), 700), 0f, Color.FromArgb(200, 255, 255, 255)).Draw();
			new UIResText(MissionName, new Point(x, 100), 2.5f, Color.FromArgb(255, 199, 168, 87), GTA.UI.Font.Pricedown, UIResText.Alignment.Centered).Draw();
			new UIResText(vic, new Point(x, 200), 1.5f, Color.White, GTA.UI.Font.HouseScript, UIResText.Alignment.Centered).Draw();
			new UIResText("Payout : " + Payout.ToString("N") + "c", new Point(x, 300), 0.5f, Color.White, GTA.UI.Font.ChaletLondon, UIResText.Alignment.Centered).Draw();
			new UIResText("Loss : " + loss.ToString("N") + "c", new Point(x, 380), 0.5f, Color.White, GTA.UI.Font.ChaletLondon, UIResText.Alignment.Centered).Draw();
			Scaleform scaleform = new Scaleform(0);
			scaleform.Render2D();
		}

		public void ShowRouletteOptions(Prop Table)
		{
			int num = Convert.ToInt32(UIMenu.GetScreenResolutionMantainRatio().Width / 2f);
			Scaleform scaleform = new Scaleform(1);
			scaleform.Load("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 51, 0 }), "Quit Roulette");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 44, 0 }), "|   Cash" + Chips_Amount + "    |     Confirm Move");
			if (RouletteBetsPlaced == 2)
			{
				scaleform.CallFunction("SET_DATA_SLOT", 3, "--" + BetPos + "--");
			}
			if (RouletteBetsPlaced < 2)
			{
				scaleform.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 175, 0 }), "");
				scaleform.CallFunction("SET_DATA_SLOT", 5, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 174, 0 }), " Change Marker Pos");
			}
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public void Roulette_Choose_Num(Prop Table)
		{
			int num = 0;
			Function.Call(((Hash)0x7FB218262B810701uL), new InputArgument[9]
			{
				Table,
				"loop_wheel",
				LoadDict("anim_casino_b@amb@casino@games@roulette@table"),
				20f,
				false,
				true,
				0,
				0f,
				8
			});
			Vic1 = 0;
			Vic2 = 0;
			Vic3 = 0;
			Random random = new Random();
			List<object> list = new List<object>();
			list.Add("0");
			list.Add("00");
			list.Add("Red, 1");
			list.Add("Black, 2");
			list.Add("Red, 3");
			list.Add("Black, 4");
			list.Add("Red, 5");
			list.Add("Black, 6");
			list.Add("Red, 7");
			list.Add("Black, 8");
			list.Add("Red, 9");
			list.Add("Black, 10");
			list.Add("Black, 11");
			list.Add("Red, 12");
			list.Add("Black, 13");
			list.Add("Red, 14");
			list.Add("Black, 15");
			list.Add("Red, 16");
			list.Add("Black, 17");
			list.Add("Red, 18");
			list.Add("Red, 19");
			list.Add("Black, 20");
			list.Add("Red, 21");
			list.Add("Black, 22");
			list.Add("Red, 23");
			list.Add("Black, 24");
			list.Add("Red, 25");
			list.Add("Black, 26");
			list.Add("Red, 27");
			list.Add("Black, 28");
			list.Add("Black, 29");
			list.Add("Red, 30");
			list.Add("Black, 31");
			list.Add("Red, 32");
			list.Add("Black, 33");
			list.Add("Red, 34");
			list.Add("Black, 35");
			list.Add("Red, 36");
			list.Add("Green , 0");
			list.Add("Green , 00");
			string text = "";
			float num2 = 0f;
			if (Ball != null)
			{
				Ball.Delete();
			}
			Function.Call(((Hash)0x7FB218262B810701uL), new InputArgument[9]
			{
				Table,
				"intro_wheel",
				LoadDict("anim_casino_b@amb@casino@games@roulette@table"),
				20f,
				false,
				true,
				0,
				0f,
				100
			});
			Function.Call(((Hash)0x7FB218262B810701uL), new InputArgument[9]
			{
				Table,
				"loop_wheel",
				LoadDict("anim_casino_b@amb@casino@games@roulette@table"),
				20f,
				true,
				true,
				0,
				0f,
				1000
			});
			Vector3 offsetInWorldCoords = Table.GetOffsetInWorldCoords(new Vector3(-1f, -0.36f, 1.06f));
			if (RoulleteTable.Equals("A"))
			{
				offsetInWorldCoords = Table.GetOffsetInWorldCoords(new Vector3(-0.65f, -0.45f, 1.06f));
			}
			if (RoulleteTable.Equals("B"))
			{
				offsetInWorldCoords = Table.GetOffsetInWorldCoords(new Vector3(-0.42f, -0.1f, 1.06f));
			}
			if (RoulleteTable.Equals("C"))
			{
				offsetInWorldCoords = Table.GetOffsetInWorldCoords(new Vector3(-1f, -0.36f, 1.06f));
			}
			Ball = World.CreateProp(RequestModel("vw_prop_roulette_ball"), offsetInWorldCoords, new Vector3(0f, 0f, 0f), false, false);
			Ball.IsPositionFrozen = true;
			Ball.IsCollisionEnabled = false;
			Function.Call(((Hash)0x7FB218262B810701uL), new InputArgument[9]
			{
				Ball,
				"loop_ball",
				LoadDict("anim_casino_b@amb@casino@games@roulette@table"),
				10f,
				true,
				true,
				0,
				0f,
				1000
			});
			for (int i = 0; i < random.Next(10, 100); i++)
			{
				for (int j = 0; j < list.Count; j++)
				{
					Script.Wait(1);
					text = (dynamic)list[random.Next(0, list.Count)];
					num2 += 0.25f;
					float num3 = random.Next(70, 100);
					if (num2 > num3)
					{
						UI.Notify("RETURN");
						break;
					}
				}
			}
			DisplayHelpTextThisFrameNoSound("Number : " + text);
			string text2 = "";
			int num4 = 0;
			if (text.Contains(","))
			{
				try
				{
					string[] array = text.Split(new char[1] { ',' });
					text2 = array[0];
					string s = array[1];
					num4 = int.Parse(s);
					if (num4 == 0 || num4 == 0)
					{
						int num5 = random.Next(0, 100);
						if (num5 < 50)
						{
							num = 37;
							Function.Call(((Hash)0x7FB218262B810701uL), new InputArgument[9]
							{
								Table,
								"exit_37_wheel",
								LoadDict("anim_casino_b@amb@casino@games@roulette@table"),
								100f,
								false,
								true,
								0,
								0f,
								2
							});
							Function.Call(((Hash)0x7FB218262B810701uL), new InputArgument[9]
							{
								Ball,
								"exit_37_ball",
								LoadDict("anim_casino_b@amb@casino@games@roulette@table"),
								10f,
								false,
								true,
								0,
								0f,
								2
							});
						}
						if (num5 > 50)
						{
							num = 38;
							Function.Call(((Hash)0x7FB218262B810701uL), new InputArgument[9]
							{
								Table,
								"exit_38_wheel",
								LoadDict("anim_casino_b@amb@casino@games@roulette@table"),
								100f,
								false,
								true,
								0,
								0f,
								2
							});
							Function.Call(((Hash)0x7FB218262B810701uL), new InputArgument[9]
							{
								Ball,
								"exit_38_ball",
								LoadDict("anim_casino_b@amb@casino@games@roulette@table"),
								10f,
								false,
								true,
								0,
								0f,
								2
							});
						}
					}
					else
					{
						num = num4;
						Function.Call(((Hash)0x7FB218262B810701uL), new InputArgument[9]
						{
							Table,
							"exit_" + num4 + "_wheel",
							LoadDict("anim_casino_b@amb@casino@games@roulette@table"),
							100f,
							false,
							true,
							0,
							0f,
							2
						});
						Function.Call(((Hash)0x7FB218262B810701uL), new InputArgument[9]
						{
							Ball,
							"exit_" + num4 + "_ball",
							LoadDict("anim_casino_b@amb@casino@games@roulette@table"),
							10f,
							false,
							true,
							0,
							0f,
							2
						});
						UI.Notify("exit_" + num4 + "_wheel");
					}
				}
				catch
				{
					UI.Notify("error");
				}
			}
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			UI.Notify("Current " + text);
			string text3 = "";
			bool flag = false;
			text3 = RouletteP1;
			string text4 = text2;
			int num9 = num4;
			UI.Notify("Num " + num9);
			UI.Notify("Prize : " + text3);
			if (text3.Equals("0") && text.Contains(text3))
			{
				SetVictory1();
			}
			if (text3.Equals("00") && text.Contains(text3))
			{
				SetVictory1();
			}
			if (text3.Equals("Black") || text3.Equals("Red"))
			{
				if (text.Contains(text3))
				{
					SetVictory1();
				}
			}
			else if (text3.Equals("Odd"))
			{
				if (num9 % 2 != 0)
				{
					SetVictory1();
				}
			}
			else if (text3.Equals("Even"))
			{
				if (num9 % 2 == 0)
				{
					SetVictory1();
				}
			}
			else if (text3.Equals("1st Column"))
			{
				if (num9 == 1 || num9 == 4 || num9 == 7 || num9 == 10 || num9 == 13 || num9 == 16 || num9 == 19 || num9 == 22 || num9 == 25 || num9 == 28 || num9 == 31 || num9 == 34)
				{
					SetVictory1();
				}
			}
			else if (text3.Equals("2nd Column"))
			{
				if (num9 == 3 || num9 == 6 || num9 == 9 || num9 == 12 || num9 == 15 || num9 == 18 || num9 == 21 || num9 == 24 || num9 == 27 || num9 == 30 || num9 == 33 || num9 == 36)
				{
					SetVictory1();
				}
			}
			else if (text3.Equals("3rd Column"))
			{
				if (num9 == 2 || num9 == 5 || num9 == 8 || num9 == 11 || num9 == 14 || num9 == 17 || num9 == 20 || num9 == 23 || num9 == 26 || num9 == 29 || num9 == 32 || num9 == 35)
				{
					SetVictory1();
				}
			}
			else if (text3.Equals("1st 12"))
			{
				for (int k = 1; k < 13; k++)
				{
					if (num9 == k)
					{
						SetVictory1();
					}
				}
			}
			else if (text3.Equals("2nd 12"))
			{
				for (int l = 13; l < 25; l++)
				{
					if (num9 == l)
					{
						SetVictory1();
					}
				}
			}
			else if (text3.Equals("3rd 12"))
			{
				for (int m = 25; m < 37; m++)
				{
					if (num9 == m)
					{
						SetVictory1();
					}
				}
			}
			else if (text3.Equals("1 to 18"))
			{
				for (int n = 1; n < 19; n++)
				{
					if (num9 == n)
					{
						SetVictory1();
					}
				}
			}
			if (text3.Equals("19 to 36"))
			{
				for (int num10 = 19; num10 < 37; num10++)
				{
					if (num9 == num10)
					{
						SetVictory1();
					}
				}
			}
			UI.Notify("V1 : " + Vic1);
			text3 = RouletteP2;
			text4 = text2;
			num9 = num4;
			UI.Notify("Num " + num9);
			UI.Notify("Prize : " + text3);
			if (text3.Equals("0") && text.Contains(text3))
			{
				SetVictory2();
			}
			if (text3.Equals("00") && text.Contains(text3))
			{
				SetVictory2();
			}
			if (text3.Equals("Black") || text3.Equals("Red"))
			{
				if (text.Contains(text3))
				{
					SetVictory2();
				}
			}
			else if (text3.Equals("Odd"))
			{
				if (num9 % 2 != 0)
				{
					SetVictory2();
				}
			}
			else if (text3.Equals("Even"))
			{
				if (num9 % 2 == 0)
				{
					SetVictory2();
				}
			}
			else if (text3.Equals("1st Column"))
			{
				if (num9 == 1 || num9 == 4 || num9 == 7 || num9 == 10 || num9 == 13 || num9 == 16 || num9 == 19 || num9 == 22 || num9 == 25 || num9 == 28 || num9 == 31 || num9 == 34)
				{
					SetVictory2();
				}
			}
			else if (text3.Equals("2nd Column"))
			{
				if (num9 == 3 || num9 == 6 || num9 == 9 || num9 == 12 || num9 == 15 || num9 == 18 || num9 == 21 || num9 == 24 || num9 == 27 || num9 == 30 || num9 == 33 || num9 == 36)
				{
					SetVictory2();
				}
			}
			else if (text3.Equals("3rd Column"))
			{
				if (num9 == 2 || num9 == 5 || num9 == 8 || num9 == 11 || num9 == 14 || num9 == 17 || num9 == 20 || num9 == 23 || num9 == 26 || num9 == 29 || num9 == 32 || num9 == 35)
				{
					SetVictory2();
				}
			}
			else if (text3.Equals("1st 12"))
			{
				for (int num11 = 1; num11 < 13; num11++)
				{
					if (num9 == num11)
					{
						SetVictory2();
					}
				}
			}
			else if (text3.Equals("2nd 12"))
			{
				for (int num12 = 13; num12 < 25; num12++)
				{
					if (num9 == num12)
					{
						SetVictory2();
					}
				}
			}
			else if (text3.Equals("3rd 12"))
			{
				for (int num13 = 25; num13 < 37; num13++)
				{
					if (num9 == num13)
					{
						SetVictory2();
					}
				}
			}
			else if (text3.Equals("1 to 18"))
			{
				for (int num14 = 1; num14 < 19; num14++)
				{
					if (num9 == num14)
					{
						SetVictory2();
					}
				}
			}
			if (text3.Equals("19 to 36"))
			{
				for (int num15 = 19; num15 < 37; num15++)
				{
					if (num9 == num15)
					{
						SetVictory2();
					}
				}
			}
			UI.Notify("V2 : " + Vic2);
			text3 = RouletteP2;
			text4 = text2;
			num9 = num4;
			UI.Notify("Num " + num9);
			UI.Notify("Prize : " + text3);
			if (text3.Equals("0") && text.Contains(text3))
			{
				SetVictory3();
			}
			if (text3.Equals("00") && text.Contains(text3))
			{
				SetVictory3();
			}
			if (text3.Equals("Black") || text3.Equals("Red"))
			{
				if (text.Contains(text3))
				{
					SetVictory3();
				}
			}
			else if (text3.Equals("Odd"))
			{
				if (num9 % 2 != 0)
				{
					SetVictory3();
				}
			}
			else if (text3.Equals("Even"))
			{
				if (num9 % 2 == 0)
				{
					SetVictory3();
				}
			}
			else if (text3.Equals("1st Column"))
			{
				if (num9 == 1 || num9 == 4 || num9 == 7 || num9 == 10 || num9 == 13 || num9 == 16 || num9 == 19 || num9 == 22 || num9 == 25 || num9 == 28 || num9 == 31 || num9 == 34)
				{
					SetVictory3();
				}
			}
			else if (text3.Equals("2nd Column"))
			{
				if (num9 == 3 || num9 == 6 || num9 == 9 || num9 == 12 || num9 == 15 || num9 == 18 || num9 == 21 || num9 == 24 || num9 == 27 || num9 == 30 || num9 == 33 || num9 == 36)
				{
					SetVictory3();
				}
			}
			else if (text3.Equals("3rd Column"))
			{
				if (num9 == 2 || num9 == 5 || num9 == 8 || num9 == 11 || num9 == 14 || num9 == 17 || num9 == 20 || num9 == 23 || num9 == 26 || num9 == 29 || num9 == 32 || num9 == 35)
				{
					SetVictory3();
				}
			}
			else if (text3.Equals("1st 12"))
			{
				for (int num16 = 1; num16 < 13; num16++)
				{
					if (num9 == num16)
					{
						SetVictory3();
					}
				}
			}
			else if (text3.Equals("2nd 12"))
			{
				for (int num17 = 13; num17 < 25; num17++)
				{
					if (num9 == num17)
					{
						SetVictory3();
					}
				}
			}
			else if (text3.Equals("3rd 12"))
			{
				for (int num18 = 25; num18 < 37; num18++)
				{
					if (num9 == num18)
					{
						SetVictory3();
					}
				}
			}
			else if (text3.Equals("1 to 18"))
			{
				for (int num19 = 1; num19 < 19; num19++)
				{
					if (num9 == num19)
					{
						SetVictory3();
					}
				}
			}
			if (text3.Equals("19 to 36"))
			{
				for (int num20 = 19; num20 < 37; num20++)
				{
					if (num9 == num20)
					{
						SetVictory3();
					}
				}
			}
			UI.Notify("V3 : " + Vic3);
			float num21 = Vic1 + Vic2 + Vic3;
			float num22 = 0f;
			float num23 = 0f;
			if (Vic1 == 1)
			{
				num22 += RouletteBet1;
			}
			if (Vic2 == 1)
			{
				num22 += RouletteBet2;
			}
			if (Vic3 == 1)
			{
				num22 += RouletteBet3;
			}
			if (Vic1 == 0)
			{
				num23 += RouletteBet1;
			}
			if (Vic2 == 0)
			{
				num23 += RouletteBet2;
			}
			if (Vic3 == 0)
			{
				num23 += RouletteBet3;
			}
			if (num21 == 0f)
			{
				Script.Wait(1);
				Function.Call(((Hash)0x7FB218262B810701uL), new InputArgument[9]
				{
					Ball,
					"exit_" + num + "_ball",
					LoadDict("anim_casino_b@amb@casino@games@roulette@table"),
					10f,
					false,
					true,
					0,
					0f,
					2
				});
				for (int num24 = 0; num24 < 100; num24++)
				{
					Script.Wait(1);
					Roulette_GameFail("You lost All bets!", "Major Defeat", "Roulette", (int)num23);
				}
				Chips_Amount -= (int)num23;
			}
			if (num21 == 1f)
			{
				Script.Wait(1);
				Function.Call(((Hash)0x7FB218262B810701uL), new InputArgument[9]
				{
					Ball,
					"exit_" + num + "_ball",
					LoadDict("anim_casino_b@amb@casino@games@roulette@table"),
					10f,
					false,
					true,
					0,
					0f,
					2
				});
				for (int num25 = 0; num25 < 100; num25++)
				{
					Script.Wait(1);
					Roulette_GameFail("You lost 2/3", "Minor Defeat", "Roulette ", (int)num23);
				}
				Chips_Amount -= (int)num23;
			}
			if (num21 == 2f)
			{
				Script.Wait(1);
				Function.Call(((Hash)0x7FB218262B810701uL), new InputArgument[9]
				{
					Ball,
					"exit_" + num + "_ball",
					LoadDict("anim_casino_b@amb@casino@games@roulette@table"),
					10f,
					false,
					true,
					0,
					0f,
					2
				});
				for (int num26 = 0; num26 < 100; num26++)
				{
					Roulette_GamePassed("Roulette ", "Minor Victory", (int)num22, (int)num23);
				}
				Chips_Amount += (int)num22;
			}
			if (num21 == 3f)
			{
				Function.Call(((Hash)0x7FB218262B810701uL), new InputArgument[9]
				{
					Ball,
					"exit_" + num + "_ball",
					LoadDict("anim_casino_b@amb@casino@games@roulette@table"),
					10f,
					false,
					true,
					0,
					0f,
					2
				});
				for (int num27 = 0; num27 < 100; num27++)
				{
					Script.Wait(1);
					Roulette_GamePassed("Roulette ", "Major Victory", (int)num22, (int)num23);
				}
				Chips_Amount += (int)num22;
			}
			MarkerNum++;
		}

		public void ShowRouletteOptionsMid(Prop Table)
		{
			int num = Convert.ToInt32(UIMenu.GetScreenResolutionMantainRatio().Width / 2f);
			Scaleform scaleform = new Scaleform(1);
			scaleform.Load("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 51, 0 }), "Quit Roulette");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 44, 0 }), "Procced");
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 80, 0 }), " Set Green Bet " + RouletteBet1 + "c -->");
			scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 58, 0 }), " Set Blue Bet " + RouletteBet2 + "c -->");
			scaleform.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 75, 0 }), " Set Red Bet " + RouletteBet3 + "c -->");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public void ShowRouletteOptionsEnd(Prop Table)
		{
			int num = Convert.ToInt32(UIMenu.GetScreenResolutionMantainRatio().Width / 2f);
			Scaleform scaleform = new Scaleform(1);
			scaleform.Load("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 51, 0 }), "Quit Roulette");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 44, 0 }), "Spin wheel");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public void ShowRouletteOptionsEndGame(Prop Table)
		{
			int num = Convert.ToInt32(UIMenu.GetScreenResolutionMantainRatio().Width / 2f);
			Scaleform scaleform = new Scaleform(1);
			scaleform.Load("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 51, 0 }), "Quit Roulette");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 58, 0 }), "Play Again");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public void SetStage(Prop Slot, int type)
		{
			Vector3 rotation = Slot.Rotation;
			if (type != 1)
			{
				return;
			}
			if (StageA == 0)
			{
				if (RotateTimerA != 12)
				{
					RotateTimerA++;
				}
				if (RotateTimerA == 12)
				{
					StageA = 1;
					RotateTimerA = 0;
					Slot.Rotation = new Vector3(22.5f, 0f, 0f);
				}
			}
			if (StageA == 1)
			{
				if (RotateTimerA != 12)
				{
					RotateTimerA++;
				}
				if (RotateTimerA == 12)
				{
					StageA = 2;
					RotateTimerA = 0;
					Slot.Rotation = new Vector3(45f, 0f, 0f);
				}
			}
			if (StageA == 2)
			{
				if (RotateTimerA != 12)
				{
					RotateTimerA++;
				}
				if (RotateTimerA == 12)
				{
					StageA = 3;
					RotateTimerA = 0;
					Slot.Rotation = new Vector3(67.5f, 0f, 0f);
				}
			}
			if (StageA == 3)
			{
				if (RotateTimerA != 12)
				{
					RotateTimerA++;
				}
				if (RotateTimerA == 12)
				{
					StageA = 4;
					RotateTimerA = 0;
					Slot.Rotation = new Vector3(90f, 0f, 0f);
				}
			}
			if (StageA == 4)
			{
				if (RotateTimerA != 12)
				{
					RotateTimerA++;
				}
				if (RotateTimerA == 12)
				{
					StageA = 5;
					RotateTimerA = 0;
					Slot.Rotation = new Vector3(112.5f, 0f, 0f);
				}
			}
			if (StageA == 5)
			{
				if (RotateTimerA != 12)
				{
					RotateTimerA++;
				}
				if (RotateTimerA == 12)
				{
					StageA = 6;
					RotateTimerA = 0;
					Slot.Rotation = new Vector3(135f, 0f, 0f);
				}
			}
			if (StageA == 6)
			{
				if (RotateTimerA != 12)
				{
					RotateTimerA++;
				}
				if (RotateTimerA == 12)
				{
					StageA = 7;
					RotateTimerA = 0;
					Slot.Rotation = new Vector3(157.5f, 0f, 0f);
				}
			}
			if (StageA == 7)
			{
				if (RotateTimerA != 12)
				{
					RotateTimerA++;
				}
				if (RotateTimerA == 12)
				{
					StageA = 8;
					RotateTimerA = 0;
					Slot.Rotation = new Vector3(180f, 0f, 0f);
				}
			}
			if (StageA == 8)
			{
				if (RotateTimerA != 12)
				{
					RotateTimerA++;
				}
				if (RotateTimerA == 12)
				{
					StageA = 9;
					RotateTimerA = 0;
					Slot.Rotation = new Vector3(202.5f, 0f, 0f);
				}
			}
			if (StageA == 9)
			{
				if (RotateTimerA != 12)
				{
					RotateTimerA++;
				}
				if (RotateTimerA == 12)
				{
					StageA = 10;
					RotateTimerA = 0;
					Slot.Rotation = new Vector3(225f, 0f, 0f);
				}
			}
			if (StageA == 10)
			{
				if (RotateTimerA != 12)
				{
					RotateTimerA++;
				}
				if (RotateTimerA == 12)
				{
					StageA = 11;
					RotateTimerA = 0;
					Slot.Rotation = new Vector3(247.5f, 0f, 0f);
				}
			}
			if (StageA == 11)
			{
				if (RotateTimerA != 12)
				{
					RotateTimerA++;
				}
				if (RotateTimerA == 12)
				{
					StageA = 12;
					RotateTimerA = 0;
					Slot.Rotation = new Vector3(270f, 0f, 0f);
				}
			}
			if (StageA == 12)
			{
				if (RotateTimerA != 12)
				{
					RotateTimerA++;
				}
				if (RotateTimerA == 12)
				{
					StageA = 13;
					RotateTimerA = 0;
					Slot.Rotation = new Vector3(292.5f, 0f, 0f);
				}
			}
			if (StageA == 13)
			{
				if (RotateTimerA != 12)
				{
					RotateTimerA++;
				}
				if (RotateTimerA == 12)
				{
					StageA = 14;
					RotateTimerA = 0;
					Slot.Rotation = new Vector3(315f, 0f, 0f);
				}
			}
			if (StageA == 14)
			{
				if (RotateTimerA != 12)
				{
					RotateTimerA++;
				}
				if (RotateTimerA == 12)
				{
					StageA = 15;
					RotateTimerA = 0;
					Slot.Rotation = new Vector3(337.5f, 0f, 0f);
				}
			}
			if (StageA == 15)
			{
				if (RotateTimerA != 12)
				{
					RotateTimerA++;
				}
				if (RotateTimerA == 12)
				{
					StageA = 0;
					RotateTimerA = 0;
					Slot.Rotation = new Vector3(0f, 0f, 0f);
				}
			}
		}

		public string ConvertSlotHashNametoActualName(string Slot)
		{
			string result = "";
			if (Slot.Equals("vw_prop_casino_slot_01a"))
			{
				result = "Angel and the Knights";
			}
			if (Slot.Equals("vw_prop_casino_slot_02a"))
			{
				result = "Impotent Rage";
			}
			if (Slot.Equals("vw_prop_casino_slot_03a"))
			{
				result = "Republican Space Ranger";
			}
			if (Slot.Equals("vw_prop_casino_slot_04a"))
			{
				result = "No Talent Required";
			}
			if (Slot.Equals("vw_prop_casino_slot_05a"))
			{
				result = "Deity of the Sun";
			}
			if (Slot.Equals("vw_prop_casino_slot_06a"))
			{
				result = "Twilight Knife";
			}
			if (Slot.Equals("vw_prop_casino_slot_07a"))
			{
				result = "Diamond Miner";
			}
			if (Slot.Equals("vw_prop_casino_slot_08a"))
			{
				result = "Evacuator";
			}
			return result;
		}

		public float Rotate(float Z)
		{
			float result = 0f;
			if (Z <= 0f && Z < -90f)
			{
				Z -= 1f;
			}
			return result;
		}

		public string ReturnToken(string TokenReturn, float HitNum, float ActualNum)
		{
			string result = "";
			if (ActualNum >= HitNum - 5f && ActualNum <= HitNum + 5f)
			{
				result = TokenReturn;
			}
			return result;
		}

		public float CalcualteSlotWin(string Machine)
		{
			float num = 0f;
			if (Machine.Equals("vw_prop_casino_slot_01a"))
			{
				if (ALandedOn.Equals("Star") || BLandedOn.Equals("Star") || CLandedOn.Equals("Star"))
				{
					num = 200f;
				}
				if (ALandedOn.Equals("Star") && BLandedOn.Equals("Star"))
				{
					num = 500f;
				}
				if (BLandedOn.Equals("Star") && CLandedOn.Equals("Star"))
				{
					num = 500f;
				}
				if (CLandedOn.Equals("Star") && ALandedOn.Equals("Star"))
				{
					num = 500f;
				}
				if (ALandedOn.Equals("Bell") && BLandedOn.Equals("Bell"))
				{
					num = 1250f;
				}
				if (BLandedOn.Equals("Bell") && CLandedOn.Equals("Bell"))
				{
					num = 1250f;
				}
				if (CLandedOn.Equals("Bell") && ALandedOn.Equals("Bell"))
				{
					num = 1250f;
				}
				if (ALandedOn.Equals("Fruit") && BLandedOn.Equals("Fruit"))
				{
					num = 3350f;
				}
				if (BLandedOn.Equals("Fruit") && CLandedOn.Equals("Fruit"))
				{
					num = 3350f;
				}
				if (CLandedOn.Equals("Fruit") && ALandedOn.Equals("Fruit"))
				{
					num = 3350f;
				}
				if (ALandedOn.Equals("Seven") && BLandedOn.Equals("Seven"))
				{
					num = 14750f;
				}
				if (BLandedOn.Equals("Seven") && CLandedOn.Equals("Seven"))
				{
					num = 14750f;
				}
				if (CLandedOn.Equals("Seven") && ALandedOn.Equals("Seven"))
				{
					num = 14750f;
				}
				if (ALandedOn.Equals("WaterMelon") && BLandedOn.Equals("WaterMelon"))
				{
					num = 750f;
				}
				if (BLandedOn.Equals("WaterMelon") && CLandedOn.Equals("WaterMelon"))
				{
					num = 750f;
				}
				if (CLandedOn.Equals("WaterMelon") && ALandedOn.Equals("WaterMelon"))
				{
					num = 750f;
				}
				if (ALandedOn.Equals("Cherry") && BLandedOn.Equals("Cherry"))
				{
					num = 1550f;
				}
				if (BLandedOn.Equals("Cherry") && CLandedOn.Equals("Cherry"))
				{
					num = 1550f;
				}
				if (CLandedOn.Equals("Cherry") && ALandedOn.Equals("Cherry"))
				{
					num = 1550f;
				}
				if (ALandedOn.Equals("OvenMat") && BLandedOn.Equals("OvenMat"))
				{
					num = 100000f;
				}
				if (BLandedOn.Equals("OvenMat") && CLandedOn.Equals("OvenMat"))
				{
					num = 100000f;
				}
				if (CLandedOn.Equals("OvenMat") && ALandedOn.Equals("OvenMat"))
				{
					num = 100000f;
				}
				if (ALandedOn.Equals("Star") && BLandedOn.Equals("Star") && CLandedOn.Equals("Star"))
				{
					num = 50000f;
				}
				if (ALandedOn.Equals("Cherry") && BLandedOn.Equals("Cherry") && CLandedOn.Equals("Cherry"))
				{
					num = 2500f;
				}
				if (ALandedOn.Equals("Bell") && BLandedOn.Equals("Bell") && CLandedOn.Equals("Bell"))
				{
					num = 10000f;
				}
				if (ALandedOn.Equals("Fruit") && BLandedOn.Equals("Fruit") && CLandedOn.Equals("Fruit"))
				{
					num = 5000f;
				}
				if (ALandedOn.Equals("Seven") && BLandedOn.Equals("Seven") && CLandedOn.Equals("Seven"))
				{
					num = 25000f;
				}
				if (ALandedOn.Equals("WaterMelon") && BLandedOn.Equals("WaterMelon") && CLandedOn.Equals("WaterMelon"))
				{
					num = 7500f;
				}
				if (ALandedOn.Equals("Diamonds") && BLandedOn.Equals("Diamonds") && CLandedOn.Equals("Diamonds"))
				{
					num = 100000f;
				}
			}
			if (Machine.Equals("vw_prop_casino_slot_02a"))
			{
				if (ALandedOn.Equals("Bolt") || BLandedOn.Equals("Bolt") || CLandedOn.Equals("Bolt"))
				{
					num = 50f;
				}
				if (ALandedOn.Equals("Bolt") && BLandedOn.Equals("Bolt"))
				{
					num = 125f;
				}
				if (BLandedOn.Equals("Bolt") && CLandedOn.Equals("Bolt"))
				{
					num = 125f;
				}
				if (CLandedOn.Equals("Bolt") && ALandedOn.Equals("Bolt"))
				{
					num = 125f;
				}
				if (ALandedOn.Equals("Bell") && BLandedOn.Equals("Bell"))
				{
					num = 750f;
				}
				if (BLandedOn.Equals("Bell") && CLandedOn.Equals("Bell"))
				{
					num = 750f;
				}
				if (CLandedOn.Equals("Bell") && ALandedOn.Equals("Bell"))
				{
					num = 750f;
				}
				if (ALandedOn.Equals("Fruit") && BLandedOn.Equals("Fruit"))
				{
					num = 450f;
				}
				if (BLandedOn.Equals("Fruit") && CLandedOn.Equals("Fruit"))
				{
					num = 450f;
				}
				if (CLandedOn.Equals("Fruit") && ALandedOn.Equals("Fruit"))
				{
					num = 450f;
				}
				if (ALandedOn.Equals("Seven") && BLandedOn.Equals("Seven"))
				{
					num = 3750f;
				}
				if (BLandedOn.Equals("Seven") && CLandedOn.Equals("Seven"))
				{
					num = 3750f;
				}
				if (CLandedOn.Equals("Seven") && ALandedOn.Equals("Seven"))
				{
					num = 3750f;
				}
				if (ALandedOn.Equals("WaterMelon") && BLandedOn.Equals("WaterMelon"))
				{
					num = 950f;
				}
				if (BLandedOn.Equals("WaterMelon") && CLandedOn.Equals("WaterMelon"))
				{
					num = 950f;
				}
				if (CLandedOn.Equals("WaterMelon") && ALandedOn.Equals("WaterMelon"))
				{
					num = 950f;
				}
				if (ALandedOn.Equals("Cherry") && BLandedOn.Equals("Cherry"))
				{
					num = 250f;
				}
				if (BLandedOn.Equals("Cherry") && CLandedOn.Equals("Cherry"))
				{
					num = 250f;
				}
				if (CLandedOn.Equals("Cherry") && ALandedOn.Equals("Cherry"))
				{
					num = 250f;
				}
				if (ALandedOn.Equals("Ranger") && BLandedOn.Equals("Ranger"))
				{
					num = 14000f;
				}
				if (BLandedOn.Equals("Ranger") && CLandedOn.Equals("Ranger"))
				{
					num = 14000f;
				}
				if (CLandedOn.Equals("Ranger") && ALandedOn.Equals("Ranger"))
				{
					num = 14000f;
				}
				if (ALandedOn.Equals("Bolt") && BLandedOn.Equals("Bolt") && CLandedOn.Equals("Bolt"))
				{
					num = 12500f;
				}
				if (ALandedOn.Equals("Cherry") && BLandedOn.Equals("Cherry") && CLandedOn.Equals("Cherry"))
				{
					num = 625f;
				}
				if (ALandedOn.Equals("Bell") && BLandedOn.Equals("Bell") && CLandedOn.Equals("Bell"))
				{
					num = 2500f;
				}
				if (ALandedOn.Equals("Fruit") && BLandedOn.Equals("Fruit") && CLandedOn.Equals("Fruit"))
				{
					num = 1250f;
				}
				if (ALandedOn.Equals("Seven") && BLandedOn.Equals("Seven") && CLandedOn.Equals("Seven"))
				{
					num = 6250f;
				}
				if (ALandedOn.Equals("WaterMelon") && BLandedOn.Equals("WaterMelon") && CLandedOn.Equals("WaterMelon"))
				{
					num = 1875f;
				}
				if (ALandedOn.Equals("Ranger") && BLandedOn.Equals("Ranger") && CLandedOn.Equals("Ranger"))
				{
					num = 25000f;
				}
			}
			if (Machine.Equals("vw_prop_casino_slot_03a"))
			{
				if (ALandedOn.Equals("Beer") || BLandedOn.Equals("Beer") || CLandedOn.Equals("Beer"))
				{
					num = 50f;
				}
				if (ALandedOn.Equals("Beer") && BLandedOn.Equals("Beer"))
				{
					num = 125f;
				}
				if (BLandedOn.Equals("Beer") && CLandedOn.Equals("Beer"))
				{
					num = 125f;
				}
				if (CLandedOn.Equals("Beer") && ALandedOn.Equals("Beer"))
				{
					num = 125f;
				}
				if (ALandedOn.Equals("Bell") && BLandedOn.Equals("Bell"))
				{
					num = 750f;
				}
				if (BLandedOn.Equals("Bell") && CLandedOn.Equals("Bell"))
				{
					num = 750f;
				}
				if (CLandedOn.Equals("Bell") && ALandedOn.Equals("Bell"))
				{
					num = 750f;
				}
				if (ALandedOn.Equals("Fruit") && BLandedOn.Equals("Fruit"))
				{
					num = 450f;
				}
				if (BLandedOn.Equals("Fruit") && CLandedOn.Equals("Fruit"))
				{
					num = 450f;
				}
				if (CLandedOn.Equals("Fruit") && ALandedOn.Equals("Fruit"))
				{
					num = 450f;
				}
				if (ALandedOn.Equals("Seven") && BLandedOn.Equals("Seven"))
				{
					num = 3750f;
				}
				if (BLandedOn.Equals("Seven") && CLandedOn.Equals("Seven"))
				{
					num = 3750f;
				}
				if (CLandedOn.Equals("Seven") && ALandedOn.Equals("Seven"))
				{
					num = 3750f;
				}
				if (ALandedOn.Equals("WaterMelon") && BLandedOn.Equals("WaterMelon"))
				{
					num = 950f;
				}
				if (BLandedOn.Equals("WaterMelon") && CLandedOn.Equals("WaterMelon"))
				{
					num = 950f;
				}
				if (CLandedOn.Equals("WaterMelon") && ALandedOn.Equals("WaterMelon"))
				{
					num = 950f;
				}
				if (ALandedOn.Equals("Cherry") && BLandedOn.Equals("Cherry"))
				{
					num = 250f;
				}
				if (BLandedOn.Equals("Cherry") && CLandedOn.Equals("Cherry"))
				{
					num = 250f;
				}
				if (CLandedOn.Equals("Cherry") && ALandedOn.Equals("Cherry"))
				{
					num = 250f;
				}
				if (ALandedOn.Equals("Logo") && BLandedOn.Equals("Logo"))
				{
					num = 14000f;
				}
				if (BLandedOn.Equals("Logo") && CLandedOn.Equals("Logo"))
				{
					num = 14000f;
				}
				if (CLandedOn.Equals("Logo") && ALandedOn.Equals("Logo"))
				{
					num = 14000f;
				}
				if (ALandedOn.Equals("Beer") && BLandedOn.Equals("Beer") && CLandedOn.Equals("Beer"))
				{
					num = 12500f;
				}
				if (ALandedOn.Equals("Cherry") && BLandedOn.Equals("Cherry") && CLandedOn.Equals("Cherry"))
				{
					num = 625f;
				}
				if (ALandedOn.Equals("Bell") && BLandedOn.Equals("Bell") && CLandedOn.Equals("Bell"))
				{
					num = 2500f;
				}
				if (ALandedOn.Equals("Fruit") && BLandedOn.Equals("Fruit") && CLandedOn.Equals("Fruit"))
				{
					num = 1250f;
				}
				if (ALandedOn.Equals("Seven") && BLandedOn.Equals("Seven") && CLandedOn.Equals("Seven"))
				{
					num = 6250f;
				}
				if (ALandedOn.Equals("WaterMelon") && BLandedOn.Equals("WaterMelon") && CLandedOn.Equals("WaterMelon"))
				{
					num = 1875f;
				}
				if (ALandedOn.Equals("Logo") && BLandedOn.Equals("Logo") && CLandedOn.Equals("Logo"))
				{
					num = 25000f;
				}
			}
			if (Machine.Equals("vw_prop_casino_slot_04a"))
			{
				if (ALandedOn.Equals("Mic") || BLandedOn.Equals("Mic") || CLandedOn.Equals("Mic"))
				{
					num = 10f;
				}
				if (ALandedOn.Equals("Mic") && BLandedOn.Equals("Mic"))
				{
					num = 25f;
				}
				if (BLandedOn.Equals("Mic") && CLandedOn.Equals("Mic"))
				{
					num = 25f;
				}
				if (CLandedOn.Equals("Mic") && ALandedOn.Equals("Mic"))
				{
					num = 25f;
				}
				if (ALandedOn.Equals("Bell") && BLandedOn.Equals("Bell"))
				{
					num = 350f;
				}
				if (BLandedOn.Equals("Bell") && CLandedOn.Equals("Bell"))
				{
					num = 350f;
				}
				if (CLandedOn.Equals("Bell") && ALandedOn.Equals("Bell"))
				{
					num = 350f;
				}
				if (ALandedOn.Equals("Fruit") && BLandedOn.Equals("Fruit"))
				{
					num = 175f;
				}
				if (BLandedOn.Equals("Fruit") && CLandedOn.Equals("Fruit"))
				{
					num = 175f;
				}
				if (CLandedOn.Equals("Fruit") && ALandedOn.Equals("Fruit"))
				{
					num = 175f;
				}
				if (ALandedOn.Equals("Seven") && BLandedOn.Equals("Seven"))
				{
					num = 650f;
				}
				if (BLandedOn.Equals("Seven") && CLandedOn.Equals("Seven"))
				{
					num = 650f;
				}
				if (CLandedOn.Equals("Seven") && ALandedOn.Equals("Seven"))
				{
					num = 650f;
				}
				if (ALandedOn.Equals("WaterMelon") && BLandedOn.Equals("WaterMelon"))
				{
					num = 225f;
				}
				if (BLandedOn.Equals("WaterMelon") && CLandedOn.Equals("WaterMelon"))
				{
					num = 225f;
				}
				if (CLandedOn.Equals("WaterMelon") && ALandedOn.Equals("WaterMelon"))
				{
					num = 225f;
				}
				if (ALandedOn.Equals("Cherry") && BLandedOn.Equals("Cherry"))
				{
					num = 75f;
				}
				if (BLandedOn.Equals("Cherry") && CLandedOn.Equals("Cherry"))
				{
					num = 75f;
				}
				if (CLandedOn.Equals("Cherry") && ALandedOn.Equals("Cherry"))
				{
					num = 75f;
				}
				if (ALandedOn.Equals("Logo") && BLandedOn.Equals("Logo"))
				{
					num = 3500f;
				}
				if (BLandedOn.Equals("Logo") && CLandedOn.Equals("Logo"))
				{
					num = 3500f;
				}
				if (CLandedOn.Equals("Logo") && ALandedOn.Equals("Logo"))
				{
					num = 3500f;
				}
				if (ALandedOn.Equals("Mic") && BLandedOn.Equals("Mic") && CLandedOn.Equals("Mic"))
				{
					num = 2500f;
				}
				if (ALandedOn.Equals("Cherry") && BLandedOn.Equals("Cherry") && CLandedOn.Equals("Cherry"))
				{
					num = 125f;
				}
				if (ALandedOn.Equals("Bell") && BLandedOn.Equals("Bell") && CLandedOn.Equals("Bell"))
				{
					num = 500f;
				}
				if (ALandedOn.Equals("Fruit") && BLandedOn.Equals("Fruit") && CLandedOn.Equals("Fruit"))
				{
					num = 250f;
				}
				if (ALandedOn.Equals("Seven") && BLandedOn.Equals("Seven") && CLandedOn.Equals("Seven"))
				{
					num = 1250f;
				}
				if (ALandedOn.Equals("WaterMelon") && BLandedOn.Equals("WaterMelon") && CLandedOn.Equals("WaterMelon"))
				{
					num = 375f;
				}
				if (ALandedOn.Equals("Logo") && BLandedOn.Equals("Logo") && CLandedOn.Equals("Logo"))
				{
					num = 5000f;
				}
			}
			if (Machine.Equals("vw_prop_casino_slot_05a"))
			{
				if (ALandedOn.Equals("Ankh") || BLandedOn.Equals("Ankh") || CLandedOn.Equals("Ankh"))
				{
					num = 1000f;
				}
				if (ALandedOn.Equals("Ankh") && BLandedOn.Equals("Ankh"))
				{
					num = 2500f;
				}
				if (BLandedOn.Equals("Ankh") && CLandedOn.Equals("Ankh"))
				{
					num = 2500f;
				}
				if (CLandedOn.Equals("Ankh") && ALandedOn.Equals("Ankh"))
				{
					num = 2500f;
				}
				if (ALandedOn.Equals("Bell") && BLandedOn.Equals("Bell"))
				{
					num = 3750f;
				}
				if (BLandedOn.Equals("Bell") && CLandedOn.Equals("Bell"))
				{
					num = 3750f;
				}
				if (CLandedOn.Equals("Bell") && ALandedOn.Equals("Bell"))
				{
					num = 3750f;
				}
				if (ALandedOn.Equals("Fruit") && BLandedOn.Equals("Fruit"))
				{
					num = 7750f;
				}
				if (BLandedOn.Equals("Fruit") && CLandedOn.Equals("Fruit"))
				{
					num = 7750f;
				}
				if (CLandedOn.Equals("Fruit") && ALandedOn.Equals("Fruit"))
				{
					num = 7750f;
				}
				if (ALandedOn.Equals("Seven") && BLandedOn.Equals("Seven"))
				{
					num = 11750f;
				}
				if (BLandedOn.Equals("Seven") && CLandedOn.Equals("Seven"))
				{
					num = 11750f;
				}
				if (CLandedOn.Equals("Seven") && ALandedOn.Equals("Seven"))
				{
					num = 11750f;
				}
				if (ALandedOn.Equals("WaterMelon") && BLandedOn.Equals("WaterMelon"))
				{
					num = 22150f;
				}
				if (BLandedOn.Equals("WaterMelon") && CLandedOn.Equals("WaterMelon"))
				{
					num = 22150f;
				}
				if (CLandedOn.Equals("WaterMelon") && ALandedOn.Equals("WaterMelon"))
				{
					num = 22150f;
				}
				if (ALandedOn.Equals("Cherry") && BLandedOn.Equals("Cherry"))
				{
					num = 14550f;
				}
				if (BLandedOn.Equals("Cherry") && CLandedOn.Equals("Cherry"))
				{
					num = 14550f;
				}
				if (CLandedOn.Equals("Cherry") && ALandedOn.Equals("Cherry"))
				{
					num = 14550f;
				}
				if (ALandedOn.Equals("Bug") && BLandedOn.Equals("Bug"))
				{
					num = 125000f;
				}
				if (BLandedOn.Equals("Bug") && CLandedOn.Equals("Bug"))
				{
					num = 125000f;
				}
				if (CLandedOn.Equals("Bug") && ALandedOn.Equals("Bug"))
				{
					num = 125000f;
				}
				if (ALandedOn.Equals("Ankh") && BLandedOn.Equals("Ankh") && CLandedOn.Equals("Ankh"))
				{
					num = 250000f;
				}
				if (ALandedOn.Equals("Cherry") && BLandedOn.Equals("Cherry") && CLandedOn.Equals("Cherry"))
				{
					num = 12500f;
				}
				if (ALandedOn.Equals("Bell") && BLandedOn.Equals("Bell") && CLandedOn.Equals("Bell"))
				{
					num = 50000f;
				}
				if (ALandedOn.Equals("Fruit") && BLandedOn.Equals("Fruit") && CLandedOn.Equals("Fruit"))
				{
					num = 25000f;
				}
				if (ALandedOn.Equals("Seven") && BLandedOn.Equals("Seven") && CLandedOn.Equals("Seven"))
				{
					num = 125000f;
				}
				if (ALandedOn.Equals("WaterMelon") && BLandedOn.Equals("WaterMelon") && CLandedOn.Equals("WaterMelon"))
				{
					num = 37500f;
				}
				if (ALandedOn.Equals("Bug") && BLandedOn.Equals("Bug") && CLandedOn.Equals("Bug"))
				{
					num = 500000f;
				}
			}
			if (Machine.Equals("vw_prop_casino_slot_06a"))
			{
				if (ALandedOn.Equals("Knife") || BLandedOn.Equals("Knife") || CLandedOn.Equals("Knife"))
				{
					num = 200f;
				}
				if (ALandedOn.Equals("Knife") && BLandedOn.Equals("Knife"))
				{
					num = 500f;
				}
				if (BLandedOn.Equals("Knife") && CLandedOn.Equals("Knife"))
				{
					num = 500f;
				}
				if (CLandedOn.Equals("Knife") && ALandedOn.Equals("Knife"))
				{
					num = 500f;
				}
				if (ALandedOn.Equals("Bell") && BLandedOn.Equals("Bell"))
				{
					num = 1250f;
				}
				if (BLandedOn.Equals("Bell") && CLandedOn.Equals("Bell"))
				{
					num = 1250f;
				}
				if (CLandedOn.Equals("Bell") && ALandedOn.Equals("Bell"))
				{
					num = 1250f;
				}
				if (ALandedOn.Equals("Fruit") && BLandedOn.Equals("Fruit"))
				{
					num = 3350f;
				}
				if (BLandedOn.Equals("Fruit") && CLandedOn.Equals("Fruit"))
				{
					num = 3350f;
				}
				if (CLandedOn.Equals("Fruit") && ALandedOn.Equals("Fruit"))
				{
					num = 3350f;
				}
				if (ALandedOn.Equals("Seven") && BLandedOn.Equals("Seven"))
				{
					num = 14750f;
				}
				if (BLandedOn.Equals("Seven") && CLandedOn.Equals("Seven"))
				{
					num = 14750f;
				}
				if (CLandedOn.Equals("Seven") && ALandedOn.Equals("Seven"))
				{
					num = 14750f;
				}
				if (ALandedOn.Equals("WaterMelon") && BLandedOn.Equals("WaterMelon"))
				{
					num = 750f;
				}
				if (BLandedOn.Equals("WaterMelon") && CLandedOn.Equals("WaterMelon"))
				{
					num = 750f;
				}
				if (CLandedOn.Equals("WaterMelon") && ALandedOn.Equals("WaterMelon"))
				{
					num = 750f;
				}
				if (ALandedOn.Equals("Cherry") && BLandedOn.Equals("Cherry"))
				{
					num = 1550f;
				}
				if (BLandedOn.Equals("Cherry") && CLandedOn.Equals("Cherry"))
				{
					num = 1550f;
				}
				if (CLandedOn.Equals("Cherry") && ALandedOn.Equals("Cherry"))
				{
					num = 1550f;
				}
				if (ALandedOn.Equals("ChainSaw") && BLandedOn.Equals("ChainSaw"))
				{
					num = 100000f;
				}
				if (BLandedOn.Equals("ChainSaw") && CLandedOn.Equals("ChainSaw"))
				{
					num = 100000f;
				}
				if (CLandedOn.Equals("ChainSaw") && ALandedOn.Equals("ChainSaw"))
				{
					num = 100000f;
				}
				if (ALandedOn.Equals("Knife") && BLandedOn.Equals("Knife") && CLandedOn.Equals("Knife"))
				{
					num = 50000f;
				}
				if (ALandedOn.Equals("Cherry") && BLandedOn.Equals("Cherry") && CLandedOn.Equals("Cherry"))
				{
					num = 2500f;
				}
				if (ALandedOn.Equals("Bell") && BLandedOn.Equals("Bell") && CLandedOn.Equals("Bell"))
				{
					num = 10000f;
				}
				if (ALandedOn.Equals("Fruit") && BLandedOn.Equals("Fruit") && CLandedOn.Equals("Fruit"))
				{
					num = 5000f;
				}
				if (ALandedOn.Equals("Seven") && BLandedOn.Equals("Seven") && CLandedOn.Equals("Seven"))
				{
					num = 25000f;
				}
				if (ALandedOn.Equals("WaterMelon") && BLandedOn.Equals("WaterMelon") && CLandedOn.Equals("WaterMelon"))
				{
					num = 7500f;
				}
				if (ALandedOn.Equals("ChainSaw") && BLandedOn.Equals("ChainSaw") && CLandedOn.Equals("ChainSaw"))
				{
					num = 100000f;
				}
			}
			if (Machine.Equals("vw_prop_casino_slot_07a"))
			{
				if (ALandedOn.Equals("Diamond") || BLandedOn.Equals("Diamond") || CLandedOn.Equals("Diamond"))
				{
					num = 1000f;
				}
				if (ALandedOn.Equals("Diamond") && BLandedOn.Equals("Diamond"))
				{
					num = 2500f;
				}
				if (BLandedOn.Equals("Diamond") && CLandedOn.Equals("Diamond"))
				{
					num = 2500f;
				}
				if (CLandedOn.Equals("Diamond") && ALandedOn.Equals("Diamond"))
				{
					num = 2500f;
				}
				if (ALandedOn.Equals("Bell") && BLandedOn.Equals("Bell"))
				{
					num = 3750f;
				}
				if (BLandedOn.Equals("Bell") && CLandedOn.Equals("Bell"))
				{
					num = 3750f;
				}
				if (CLandedOn.Equals("Bell") && ALandedOn.Equals("Bell"))
				{
					num = 3750f;
				}
				if (ALandedOn.Equals("Fruit") && BLandedOn.Equals("Fruit"))
				{
					num = 7750f;
				}
				if (BLandedOn.Equals("Fruit") && CLandedOn.Equals("Fruit"))
				{
					num = 7750f;
				}
				if (CLandedOn.Equals("Fruit") && ALandedOn.Equals("Fruit"))
				{
					num = 7750f;
				}
				if (ALandedOn.Equals("Seven") && BLandedOn.Equals("Seven"))
				{
					num = 11750f;
				}
				if (BLandedOn.Equals("Seven") && CLandedOn.Equals("Seven"))
				{
					num = 11750f;
				}
				if (CLandedOn.Equals("Seven") && ALandedOn.Equals("Seven"))
				{
					num = 11750f;
				}
				if (ALandedOn.Equals("WaterMelon") && BLandedOn.Equals("WaterMelon"))
				{
					num = 22150f;
				}
				if (BLandedOn.Equals("WaterMelon") && CLandedOn.Equals("WaterMelon"))
				{
					num = 22150f;
				}
				if (CLandedOn.Equals("WaterMelon") && ALandedOn.Equals("WaterMelon"))
				{
					num = 22150f;
				}
				if (ALandedOn.Equals("Cherry") && BLandedOn.Equals("Cherry"))
				{
					num = 14550f;
				}
				if (BLandedOn.Equals("Cherry") && CLandedOn.Equals("Cherry"))
				{
					num = 14550f;
				}
				if (CLandedOn.Equals("Cherry") && ALandedOn.Equals("Cherry"))
				{
					num = 14550f;
				}
				if (ALandedOn.Equals("Diamonds") && BLandedOn.Equals("Diamonds"))
				{
					num = 125000f;
				}
				if (BLandedOn.Equals("Diamonds") && CLandedOn.Equals("Diamonds"))
				{
					num = 125000f;
				}
				if (CLandedOn.Equals("Diamonds") && ALandedOn.Equals("Diamonds"))
				{
					num = 125000f;
				}
				if (ALandedOn.Equals("Diamond") && BLandedOn.Equals("Diamond") && CLandedOn.Equals("Diamond"))
				{
					num = 250000f;
				}
				if (ALandedOn.Equals("Cherry") && BLandedOn.Equals("Cherry") && CLandedOn.Equals("Cherry"))
				{
					num = 12500f;
				}
				if (ALandedOn.Equals("Bell") && BLandedOn.Equals("Bell") && CLandedOn.Equals("Bell"))
				{
					num = 50000f;
				}
				if (ALandedOn.Equals("Fruit") && BLandedOn.Equals("Fruit") && CLandedOn.Equals("Fruit"))
				{
					num = 25000f;
				}
				if (ALandedOn.Equals("Seven") && BLandedOn.Equals("Seven") && CLandedOn.Equals("Seven"))
				{
					num = 125000f;
				}
				if (ALandedOn.Equals("WaterMelon") && BLandedOn.Equals("WaterMelon") && CLandedOn.Equals("WaterMelon"))
				{
					num = 37500f;
				}
				if (ALandedOn.Equals("Diamonds") && BLandedOn.Equals("Diamonds") && CLandedOn.Equals("Diamonds"))
				{
					num = 500000f;
				}
			}
			if (Machine.Equals("vw_prop_casino_slot_08a"))
			{
				if (ALandedOn.Equals("Sensi") || BLandedOn.Equals("Sensi") || CLandedOn.Equals("Sensi"))
				{
					num = 10f;
				}
				if (ALandedOn.Equals("Sensi") && BLandedOn.Equals("Sensi"))
				{
					num = 25f;
				}
				if (BLandedOn.Equals("Sensi") && CLandedOn.Equals("Sensi"))
				{
					num = 25f;
				}
				if (CLandedOn.Equals("Sensi") && ALandedOn.Equals("Sensi"))
				{
					num = 25f;
				}
				if (ALandedOn.Equals("Bell") && BLandedOn.Equals("Bell"))
				{
					num = 350f;
				}
				if (BLandedOn.Equals("Bell") && CLandedOn.Equals("Bell"))
				{
					num = 350f;
				}
				if (CLandedOn.Equals("Bell") && ALandedOn.Equals("Bell"))
				{
					num = 350f;
				}
				if (ALandedOn.Equals("Fruit") && BLandedOn.Equals("Fruit"))
				{
					num = 175f;
				}
				if (BLandedOn.Equals("Fruit") && CLandedOn.Equals("Fruit"))
				{
					num = 175f;
				}
				if (CLandedOn.Equals("Fruit") && ALandedOn.Equals("Fruit"))
				{
					num = 175f;
				}
				if (ALandedOn.Equals("Seven") && BLandedOn.Equals("Seven"))
				{
					num = 650f;
				}
				if (BLandedOn.Equals("Seven") && CLandedOn.Equals("Seven"))
				{
					num = 650f;
				}
				if (CLandedOn.Equals("Seven") && ALandedOn.Equals("Seven"))
				{
					num = 650f;
				}
				if (ALandedOn.Equals("WaterMelon") && BLandedOn.Equals("WaterMelon"))
				{
					num = 225f;
				}
				if (BLandedOn.Equals("WaterMelon") && CLandedOn.Equals("WaterMelon"))
				{
					num = 225f;
				}
				if (CLandedOn.Equals("WaterMelon") && ALandedOn.Equals("WaterMelon"))
				{
					num = 225f;
				}
				if (ALandedOn.Equals("Grenade") && BLandedOn.Equals("Grenade"))
				{
					num = 75f;
				}
				if (BLandedOn.Equals("Grenade") && CLandedOn.Equals("Grenade"))
				{
					num = 75f;
				}
				if (CLandedOn.Equals("Grenade") && ALandedOn.Equals("Grenade"))
				{
					num = 75f;
				}
				if (ALandedOn.Equals("Mortar") && BLandedOn.Equals("Mortar"))
				{
					num = 3500f;
				}
				if (BLandedOn.Equals("Mortar") && CLandedOn.Equals("Mortar"))
				{
					num = 3500f;
				}
				if (CLandedOn.Equals("Mortar") && ALandedOn.Equals("Mortar"))
				{
					num = 3500f;
				}
				if (ALandedOn.Equals("Sensi") && BLandedOn.Equals("Sensi") && CLandedOn.Equals("Sensi"))
				{
					num = 2500f;
				}
				if (ALandedOn.Equals("Grenade") && BLandedOn.Equals("Grenade") && CLandedOn.Equals("Grenade"))
				{
					num = 125f;
				}
				if (ALandedOn.Equals("Bell") && BLandedOn.Equals("Bell") && CLandedOn.Equals("Bell"))
				{
					num = 500f;
				}
				if (ALandedOn.Equals("Fruit") && BLandedOn.Equals("Fruit") && CLandedOn.Equals("Fruit"))
				{
					num = 250f;
				}
				if (ALandedOn.Equals("Seven") && BLandedOn.Equals("Seven") && CLandedOn.Equals("Seven"))
				{
					num = 1250f;
				}
				if (ALandedOn.Equals("WaterMelon") && BLandedOn.Equals("WaterMelon") && CLandedOn.Equals("WaterMelon"))
				{
					num = 375f;
				}
				if (ALandedOn.Equals("Mortar") && BLandedOn.Equals("Mortar") && CLandedOn.Equals("Mortar"))
				{
					num = 5000f;
				}
			}
			return num * SlotMultiplier;
		}

		public string GetLandedOn(float reelR, string Machine)
		{
			string text = "";
			if (Machine.Equals("vw_prop_casino_slot_01a"))
			{
				if (text.Equals(""))
				{
					text = ReturnToken("Fruit", 115f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("WaterMelon", 135f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Star", 150f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Fruit", -177f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Bell", -155f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Cherry", -134f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Star", -110f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Seven", -89f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Fruit", -66f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Cherry", -44f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("WaterMelon", -22f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Star", 4f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("OvenMat", 25f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Cherry", 46f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Bell", 71f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Seven", 91f, reelR);
				}
			}
			if (Machine.Equals("vw_prop_casino_slot_02a"))
			{
				if (text.Equals(""))
				{
					text = ReturnToken("Fruit", 114f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("WaterMelon", 137f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Bolt", 165f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Fruit", -177f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Bell", -153f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Cherry", -134f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Bolt", -109f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Seven", -87f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Fruit", -65f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Cherry", -44f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("WaterMelon", -20f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Bolt", 4f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Ranger", 24f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Cherry", 45f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Bell", 70f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Seven", 92f, reelR);
				}
			}
			if (Machine.Equals("vw_prop_casino_slot_03a"))
			{
				if (text.Equals(""))
				{
					text = ReturnToken("Fruit", 114f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("WaterMelon", 136f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Beer", 158f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Fruit", -177f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Bell", -155f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Cherry", -134f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Beer", -112f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Seven", -89f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Fruit", -65f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Cherry", -44f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("WateMelon", -20f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Beer", 3f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Logo", 25f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Cherry", 45f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Bell", 70f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Seven", 92f, reelR);
				}
			}
			if (Machine.Equals("vw_prop_casino_slot_04a"))
			{
				if (text.Equals(""))
				{
					text = ReturnToken("Fruit", 114f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("WaterMelon", 135f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Mic", 159f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Fruit", -177f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Bell", -155f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Cherry", -135f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Mic", -110f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Seven", -88f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Fruit", -65f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Cherry", -44f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("WaterMelon", -20f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Mic", 3f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Logo", 25f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Cherry", 45f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Bell", 70f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Seven", 92f, reelR);
				}
			}
			if (Machine.Equals("vw_prop_casino_slot_05a"))
			{
				if (text.Equals(""))
				{
					text = ReturnToken("Fruit", 115f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("WaterMelon", 135f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Ankh", 160f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Fruit", -177f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Bell", -155f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Cherry", -134f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Ankh", -111f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Seven", -89f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Fruit", -65f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Cherry", -42f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("WaterMelon", -20f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Ankh", 3f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Bug", 25f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Cherry", 46f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Bell", 70f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Seven", 92f, reelR);
				}
			}
			if (Machine.Equals("vw_prop_casino_slot_06a"))
			{
				if (text.Equals(""))
				{
					text = ReturnToken("Fruit", 115f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("WaterMelon", 135f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Knife", 160f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Fruit", -177f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Bell", -155f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Cherry", -135f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Knife", -110f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Seven", -88f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Fruit", -65f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Cherry", -43f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("WaterMelon", -20f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Knife", 3f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("ChainSaw", 25f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Cherry", 47f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Bell", 71f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Seven", 92f, reelR);
				}
			}
			if (Machine.Equals("vw_prop_casino_slot_07a"))
			{
				if (text.Equals(""))
				{
					text = ReturnToken("Diamond", -110f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Seven", -89f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Fruit", -64f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Cherry", -55f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("WaterMelon", -19f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Diamond", 3f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Diamonds", 25f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Cherry", 45f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Bell", 68f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Seven", 92f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Fruit", 114f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("WaterMelon", 136f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Diamond", 160f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Fruit", -178f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Bell", -155f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Cherry", -135f, reelR);
				}
			}
			if (Machine.Equals("vw_prop_casino_slot_08a"))
			{
				if (text.Equals(""))
				{
					text = ReturnToken("Fruit", 114f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("WaterMelon", 137f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Sensi", 160f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Fruit", -177f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Bell", -155f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Grenade", -135f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Sensi", -110f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Seven", -88f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Fruit", -65f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Grenade", -44f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Watermelon", -20f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Sensi", 3f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Mortar", 25f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Grenade", 47f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Bell", 70f, reelR);
				}
				if (text.Equals(""))
				{
					text = ReturnToken("Seven", 92f, reelR);
				}
			}
			return text;
		}

		public float GetClosest(float reelR, string Machine)
		{
			float result = 0f;
			if (Machine.Equals("vw_prop_casino_slot_01a"))
			{
				List<int> list = new List<int>();
				list.Add(115);
				list.Add(135);
				list.Add(150);
				list.Add(-177);
				list.Add(-134);
				list.Add(-110);
				list.Add(-89);
				list.Add(-66);
				list.Add(-44);
				list.Add(-22);
				list.Add(4);
				list.Add(25);
				list.Add(46);
				list.Add(71);
				list.Add(91);
				int number = (int)reelR;
				result = list.OrderBy((int item) => Math.Abs(number - item)).First();
			}
			if (Machine.Equals("vw_prop_casino_slot_02a"))
			{
				List<int> list2 = new List<int>();
				list2.Add(115);
				list2.Add(135);
				list2.Add(150);
				list2.Add(-177);
				list2.Add(-134);
				list2.Add(-110);
				list2.Add(-89);
				list2.Add(-66);
				list2.Add(-44);
				list2.Add(-22);
				list2.Add(4);
				list2.Add(25);
				list2.Add(46);
				list2.Add(71);
				list2.Add(91);
				int number2 = (int)reelR;
				result = list2.OrderBy((int item) => Math.Abs(number2 - item)).First();
			}
			if (Machine.Equals("vw_prop_casino_slot_03a"))
			{
				List<int> list3 = new List<int>();
				list3.Add(115);
				list3.Add(135);
				list3.Add(150);
				list3.Add(-177);
				list3.Add(-134);
				list3.Add(-110);
				list3.Add(-89);
				list3.Add(-66);
				list3.Add(-44);
				list3.Add(-22);
				list3.Add(4);
				list3.Add(25);
				list3.Add(46);
				list3.Add(71);
				list3.Add(91);
				int number3 = (int)reelR;
				result = list3.OrderBy((int item) => Math.Abs(number3 - item)).First();
			}
			if (Machine.Equals("vw_prop_casino_slot_04a"))
			{
				List<int> list4 = new List<int>();
				list4.Add(115);
				list4.Add(135);
				list4.Add(150);
				list4.Add(-177);
				list4.Add(-134);
				list4.Add(-110);
				list4.Add(-89);
				list4.Add(-66);
				list4.Add(-44);
				list4.Add(-22);
				list4.Add(4);
				list4.Add(25);
				list4.Add(46);
				list4.Add(71);
				list4.Add(91);
				int number4 = (int)reelR;
				result = list4.OrderBy((int item) => Math.Abs(number4 - item)).First();
			}
			if (Machine.Equals("vw_prop_casino_slot_05a"))
			{
				List<int> list5 = new List<int>();
				list5.Add(115);
				list5.Add(135);
				list5.Add(150);
				list5.Add(-177);
				list5.Add(-134);
				list5.Add(-110);
				list5.Add(-89);
				list5.Add(-66);
				list5.Add(-44);
				list5.Add(-22);
				list5.Add(4);
				list5.Add(25);
				list5.Add(46);
				list5.Add(71);
				list5.Add(91);
				int number5 = (int)reelR;
				result = list5.OrderBy((int item) => Math.Abs(number5 - item)).First();
			}
			if (Machine.Equals("vw_prop_casino_slot_06a"))
			{
				List<int> list6 = new List<int>();
				list6.Add(115);
				list6.Add(135);
				list6.Add(150);
				list6.Add(-177);
				list6.Add(-134);
				list6.Add(-110);
				list6.Add(-89);
				list6.Add(-66);
				list6.Add(-44);
				list6.Add(-22);
				list6.Add(4);
				list6.Add(25);
				list6.Add(46);
				list6.Add(71);
				list6.Add(91);
				int number6 = (int)reelR;
				result = list6.OrderBy((int item) => Math.Abs(number6 - item)).First();
			}
			if (Machine.Equals("vw_prop_casino_slot_07a"))
			{
				List<int> list7 = new List<int>();
				list7.Add(115);
				list7.Add(135);
				list7.Add(150);
				list7.Add(-177);
				list7.Add(-134);
				list7.Add(-110);
				list7.Add(-89);
				list7.Add(-66);
				list7.Add(-44);
				list7.Add(-22);
				list7.Add(4);
				list7.Add(25);
				list7.Add(46);
				list7.Add(71);
				list7.Add(91);
				int number7 = (int)reelR;
				result = list7.OrderBy((int item) => Math.Abs(number7 - item)).First();
			}
			if (Machine.Equals("vw_prop_casino_slot_08a"))
			{
				List<int> list8 = new List<int>();
				list8.Add(115);
				list8.Add(135);
				list8.Add(150);
				list8.Add(-177);
				list8.Add(-134);
				list8.Add(-110);
				list8.Add(-89);
				list8.Add(-66);
				list8.Add(-44);
				list8.Add(-22);
				list8.Add(4);
				list8.Add(25);
				list8.Add(46);
				list8.Add(71);
				list8.Add(91);
				int number8 = (int)reelR;
				result = list8.OrderBy((int item) => Math.Abs(number8 - item)).First();
			}
			return result;
		}

		public void SetRandomOutfit2(Ped Dancer)
		{
			Random random = new Random();
			if (Dancer.Gender == Gender.Female)
			{
				Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5]
				{
					Dancer,
					0,
					random.Next(0, Function.Call<int>(((Hash)0x27561561732A7842uL), new InputArgument[2] { Dancer, 0 })),
					0,
					0
				});
				Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5]
				{
					Dancer,
					2,
					random.Next(0, Function.Call<int>(((Hash)0x27561561732A7842uL), new InputArgument[2] { Dancer, 2 })),
					random.Next(0, Function.Call<int>(((Hash)0x8F7156A3142A6BADuL), new InputArgument[2] { Dancer, 2 })),
					0
				});
				Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5]
				{
					Dancer,
					3,
					random.Next(0, Function.Call<int>(((Hash)0x27561561732A7842uL), new InputArgument[2] { Dancer, 3 })),
					random.Next(0, Function.Call<int>(((Hash)0x8F7156A3142A6BADuL), new InputArgument[2] { Dancer, 3 })),
					0
				});
				Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5]
				{
					Dancer,
					8,
					random.Next(0, Function.Call<int>(((Hash)0x27561561732A7842uL), new InputArgument[2] { Dancer, 8 })),
					random.Next(0, Function.Call<int>(((Hash)0x8F7156A3142A6BADuL), new InputArgument[2] { Dancer, 8 })),
					0
				});
				if (Function.Call<int>(((Hash)0x67F3780DD425D4FCuL), new InputArgument[2] { Dancer, 3 }) == 4)
				{
					Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5]
					{
						Dancer,
						4,
						4,
						random.Next(0, Function.Call<int>(((Hash)0x8F7156A3142A6BADuL), new InputArgument[2] { Dancer, 4 })),
						1
					});
				}
				if (Function.Call<int>(((Hash)0x67F3780DD425D4FCuL), new InputArgument[2] { Dancer, 3 }) != 4)
				{
					int num = random.Next(0, 5);
					if (num == 4)
					{
						int num2 = random.Next(0, 4);
						if (num2 == 0)
						{
							num = 0;
						}
						if (num2 == 1)
						{
							num = 1;
						}
						if (num2 == 2)
						{
							num = 2;
						}
						if (num2 == 3)
						{
							num = 3;
						}
						if (num2 == 4)
						{
							num = 5;
						}
					}
					Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5]
					{
						Dancer,
						4,
						num,
						random.Next(0, Function.Call<int>(((Hash)0x8F7156A3142A6BADuL), new InputArgument[2] { Dancer, 4 })),
						1
					});
				}
				Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5]
				{
					Dancer,
					0,
					Function.Call<int>(((Hash)0x67F3780DD425D4FCuL), new InputArgument[2] { Dancer, 3 }),
					0,
					0
				});
				if (Function.Call<int>(((Hash)0x67F3780DD425D4FCuL), new InputArgument[2] { Dancer, 4 }) == 4)
				{
					Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5] { Dancer, 5, 4, 0, 1 });
				}
			}
			if (Dancer.Gender == Gender.Male)
			{
				Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5]
				{
					Dancer,
					0,
					random.Next(0, Function.Call<int>(((Hash)0x27561561732A7842uL), new InputArgument[2] { Dancer, 0 })),
					random.Next(0, Function.Call<int>(((Hash)0x8F7156A3142A6BADuL), new InputArgument[2] { Dancer, 0 })),
					0
				});
				Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5]
				{
					Dancer,
					2,
					random.Next(0, Function.Call<int>(((Hash)0x27561561732A7842uL), new InputArgument[2] { Dancer, 2 })),
					random.Next(0, Function.Call<int>(((Hash)0x8F7156A3142A6BADuL), new InputArgument[2] { Dancer, 2 })),
					0
				});
				Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5]
				{
					Dancer,
					3,
					random.Next(0, Function.Call<int>(((Hash)0x27561561732A7842uL), new InputArgument[2] { Dancer, 3 })),
					random.Next(0, Function.Call<int>(((Hash)0x8F7156A3142A6BADuL), new InputArgument[2] { Dancer, 3 })),
					0
				});
				Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5]
				{
					Dancer,
					8,
					random.Next(0, Function.Call<int>(((Hash)0x27561561732A7842uL), new InputArgument[2] { Dancer, 8 })),
					random.Next(0, Function.Call<int>(((Hash)0x8F7156A3142A6BADuL), new InputArgument[2] { Dancer, 8 })),
					0
				});
				if (Function.Call<int>(((Hash)0x67F3780DD425D4FCuL), new InputArgument[2] { Dancer, 3 }) == 0)
				{
					int num3 = random.Next(0, 4);
					Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5] { Dancer, 3, 5, num3, 0 });
				}
				if (Function.Call<int>(((Hash)0x67F3780DD425D4FCuL), new InputArgument[2] { Dancer, 3 }) == 1)
				{
					int num4 = random.Next(0, 3);
					Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5] { Dancer, 3, 5, num4, 0 });
				}
				if (Function.Call<int>(((Hash)0x67F3780DD425D4FCuL), new InputArgument[2] { Dancer, 3 }) == 2)
				{
					int num5 = random.Next(0, 3);
					Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5] { Dancer, 3, 5, num5, 0 });
				}
				if (Function.Call<int>(((Hash)0x67F3780DD425D4FCuL), new InputArgument[2] { Dancer, 3 }) == 3)
				{
					int num6 = random.Next(0, 3);
					Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5] { Dancer, 3, 5, num6, 0 });
				}
				if (Function.Call<int>(((Hash)0x67F3780DD425D4FCuL), new InputArgument[2] { Dancer, 3 }) == 4)
				{
					int num7 = random.Next(0, 2);
					Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5] { Dancer, 3, 5, num7, 0 });
				}
				if (Function.Call<int>(((Hash)0x67F3780DD425D4FCuL), new InputArgument[2] { Dancer, 3 }) == 5)
				{
					int num8 = random.Next(0, 2);
					Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5] { Dancer, 3, 5, num8, 0 });
				}
				if (Function.Call<int>(((Hash)0x67F3780DD425D4FCuL), new InputArgument[2] { Dancer, 3 }) == 6)
				{
					int num9 = random.Next(0, 2);
					Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5] { Dancer, 3, 5, num9, 0 });
				}
			}
		}

		public void SetBet_SlotMachine()
		{
			int num = Convert.ToInt32(UIMenu.GetScreenResolutionMantainRatio().Width / 2f);
			Scaleform scaleform = new Scaleform(1);
			scaleform.Load("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 0, 51, 0 }), "Quit");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 1, 76, 0 }), "Bet One");
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 21, 0 }), "");
			scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(((Hash)0x0499D7B09FC9B407uL), new InputArgument[3] { 2, 176, 0 }), "Spin/Bet Max");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public float GetRandomfloat(int minimum, int maximum)
		{
			Random random = new Random();
			int num = random.Next(minimum, maximum);
			return num / 1000;
		}

		private string func_178(int iParam0)
		{
			switch (iParam0)
			{
			case 1:
				return "dlc_vw_casino_slot_machine_ak_player_sounds";
			case 2:
				return "dlc_vw_casino_slot_machine_ir_player_sounds";
			case 3:
				return "dlc_vw_casino_slot_machine_rsr_player_sounds";
			case 4:
				return "dlc_vw_casino_slot_machine_fs_player_sounds";
			case 5:
				return "dlc_vw_casino_slot_machine_ds_player_sounds";
			case 6:
				return "dlc_vw_casino_slot_machine_kd_player_sounds";
			case 7:
				return "dlc_vw_casino_slot_machine_td_player_sounds";
			case 8:
				return "dlc_vw_casino_slot_machine_hz_player_sounds";
			default:
				return "";
			}
		}

		public void SetRandomOutfit(Ped Dancer)
		{
			Random random = new Random();
			if (Dancer.Gender == Gender.Female)
			{
				Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5]
				{
					Dancer,
					0,
					random.Next(0, Function.Call<int>(((Hash)0x27561561732A7842uL), new InputArgument[2] { Dancer, 0 })),
					0,
					0
				});
				Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5]
				{
					Dancer,
					2,
					random.Next(0, Function.Call<int>(((Hash)0x27561561732A7842uL), new InputArgument[2] { Dancer, 2 })),
					random.Next(0, Function.Call<int>(((Hash)0x8F7156A3142A6BADuL), new InputArgument[2] { Dancer, 2 })),
					0
				});
				Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5]
				{
					Dancer,
					3,
					random.Next(0, Function.Call<int>(((Hash)0x27561561732A7842uL), new InputArgument[2] { Dancer, 3 })),
					random.Next(0, Function.Call<int>(((Hash)0x8F7156A3142A6BADuL), new InputArgument[2] { Dancer, 3 })),
					0
				});
				Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5]
				{
					Dancer,
					8,
					random.Next(0, Function.Call<int>(((Hash)0x27561561732A7842uL), new InputArgument[2] { Dancer, 8 })),
					random.Next(0, Function.Call<int>(((Hash)0x8F7156A3142A6BADuL), new InputArgument[2] { Dancer, 8 })),
					0
				});
				if (Function.Call<int>(((Hash)0x67F3780DD425D4FCuL), new InputArgument[2] { Dancer, 3 }) == 4)
				{
					Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5]
					{
						Dancer,
						4,
						4,
						random.Next(0, Function.Call<int>(((Hash)0x8F7156A3142A6BADuL), new InputArgument[2] { Dancer, 4 })),
						1
					});
				}
				if (Function.Call<int>(((Hash)0x67F3780DD425D4FCuL), new InputArgument[2] { Dancer, 3 }) != 4)
				{
					int num = random.Next(0, 5);
					if (num == 4)
					{
						int num2 = random.Next(0, 4);
						if (num2 == 0)
						{
							num = 0;
						}
						if (num2 == 1)
						{
							num = 1;
						}
						if (num2 == 2)
						{
							num = 2;
						}
						if (num2 == 3)
						{
							num = 3;
						}
						if (num2 == 4)
						{
							num = 5;
						}
					}
					Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5]
					{
						Dancer,
						4,
						num,
						random.Next(0, Function.Call<int>(((Hash)0x8F7156A3142A6BADuL), new InputArgument[2] { Dancer, 4 })),
						1
					});
				}
				Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5]
				{
					Dancer,
					0,
					Function.Call<int>(((Hash)0x67F3780DD425D4FCuL), new InputArgument[2] { Dancer, 3 }),
					0,
					0
				});
				if (Function.Call<int>(((Hash)0x67F3780DD425D4FCuL), new InputArgument[2] { Dancer, 4 }) == 4)
				{
					Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5] { Dancer, 5, 4, 0, 1 });
				}
			}
			if (Dancer.Gender == Gender.Male)
			{
				Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5]
				{
					Dancer,
					0,
					random.Next(0, Function.Call<int>(((Hash)0x27561561732A7842uL), new InputArgument[2] { Dancer, 0 })),
					random.Next(0, Function.Call<int>(((Hash)0x8F7156A3142A6BADuL), new InputArgument[2] { Dancer, 0 })),
					0
				});
				Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5]
				{
					Dancer,
					2,
					random.Next(0, Function.Call<int>(((Hash)0x27561561732A7842uL), new InputArgument[2] { Dancer, 2 })),
					random.Next(0, Function.Call<int>(((Hash)0x8F7156A3142A6BADuL), new InputArgument[2] { Dancer, 2 })),
					0
				});
				Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5]
				{
					Dancer,
					3,
					random.Next(0, Function.Call<int>(((Hash)0x27561561732A7842uL), new InputArgument[2] { Dancer, 3 })),
					random.Next(0, Function.Call<int>(((Hash)0x8F7156A3142A6BADuL), new InputArgument[2] { Dancer, 3 })),
					0
				});
				Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5]
				{
					Dancer,
					8,
					random.Next(0, Function.Call<int>(((Hash)0x27561561732A7842uL), new InputArgument[2] { Dancer, 8 })),
					random.Next(0, Function.Call<int>(((Hash)0x8F7156A3142A6BADuL), new InputArgument[2] { Dancer, 8 })),
					0
				});
				if (Function.Call<int>(((Hash)0x67F3780DD425D4FCuL), new InputArgument[2] { Dancer, 3 }) == 0)
				{
					int num3 = random.Next(0, 4);
					Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5] { Dancer, 3, 5, num3, 0 });
				}
				if (Function.Call<int>(((Hash)0x67F3780DD425D4FCuL), new InputArgument[2] { Dancer, 3 }) == 1)
				{
					int num4 = random.Next(0, 3);
					Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5] { Dancer, 3, 5, num4, 0 });
				}
				if (Function.Call<int>(((Hash)0x67F3780DD425D4FCuL), new InputArgument[2] { Dancer, 3 }) == 2)
				{
					int num5 = random.Next(0, 3);
					Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5] { Dancer, 3, 5, num5, 0 });
				}
				if (Function.Call<int>(((Hash)0x67F3780DD425D4FCuL), new InputArgument[2] { Dancer, 3 }) == 3)
				{
					int num6 = random.Next(0, 3);
					Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5] { Dancer, 3, 5, num6, 0 });
				}
				if (Function.Call<int>(((Hash)0x67F3780DD425D4FCuL), new InputArgument[2] { Dancer, 3 }) == 4)
				{
					int num7 = random.Next(0, 2);
					Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5] { Dancer, 3, 5, num7, 0 });
				}
				if (Function.Call<int>(((Hash)0x67F3780DD425D4FCuL), new InputArgument[2] { Dancer, 3 }) == 5)
				{
					int num8 = random.Next(0, 2);
					Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5] { Dancer, 3, 5, num8, 0 });
				}
				if (Function.Call<int>(((Hash)0x67F3780DD425D4FCuL), new InputArgument[2] { Dancer, 3 }) == 6)
				{
					int num9 = random.Next(0, 2);
					Function.Call(((Hash)0x262B14F48D29DE80uL), new InputArgument[5] { Dancer, 3, 5, num9, 0 });
				}
			}
		}

		public string GetHostName()
		{
			return "Dealer";
		}

		public float GetTableHeight_Cards(int Table)
		{
			float result = 0f;
			if (Table < 4)
			{
				result = 0.9f;
			}
			if (Table == 4)
			{
				result = 0.41f;
			}
			return result;
		}

		public float GetTableHeight_Chips(int Table)
		{
			UI.Notify("T " + Table);
			float result = 0f;
			if (Table < 4)
			{
				result = 0.3f;
			}
			if (Table == 4)
			{
				result = 0.6f;
			}
			return result;
		}

		public Model SpawnProp(string Prop)
		{
			Model result = new Model(Prop);
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

		public void SpawnProp(string Prop, Vector3 Pos, Vector3 Rot)
		{
			try
			{
				Model model = new Model(Prop);
				model.Request(250);
				if (model.IsInCdImage && model.IsValid)
				{
					while (!model.IsLoaded)
					{
						Script.Wait(50);
					}
				}
				model.MarkAsNoLongerNeeded();
			}
			catch (Exception)
			{
				UI.Notify("~r~ Prop : ~w~" + Prop + "~r~ Failed to spawn");
			}
		}

		private void OnTick(object sender, EventArgs e)
		{
			Chips_Amount = Game.Player.Money;
			if (Game.IsControlJustPressed(Control.Context))
			{
			}
			if (Firsttime)
			{
				for (int i = 0; i < SlotLocH.Count; i++)
				{
					Prop machineProp = SlotLocH[i].MachineProp;
					Vector3 offsetInWorldCoords = machineProp.GetOffsetInWorldCoords(new Vector3(0f, -0.8f, 0.8f));
					if (!SlotLocH[i].In_use)
					{
					}
				}
				for (int j = 0; j < SlotLocH.Count; j++)
				{
					if (SlotLocH[j].In_use)
					{
						continue;
					}
					SlotsClass slotsClass = SlotLocH[j];
					if (World.GetDistance(Game.Player.Character.Position, slotsClass.Loc) < 2f)
					{
					}
					Prop machineProp2 = SlotLocH[j].MachineProp;
					Vector3 offsetInWorldCoords2 = machineProp2.GetOffsetInWorldCoords(new Vector3(0f, -0.8f, 0.8f));
					Vector3 offsetInWorldCoords3 = machineProp2.GetOffsetInWorldCoords(new Vector3(0f, -1.3f, 0.4f));
					if (World.GetDistance(Game.Player.Character.Position, offsetInWorldCoords3) < 1f)
					{
						if (!SlotInUse && SlotMachineStage == 0)
						{
							if (slotsClass.Machine.Equals("vw_prop_casino_slot_01a"))
							{
								MinStartBet = 500f;
							}
							if (slotsClass.Machine.Equals("vw_prop_casino_slot_02a"))
							{
								MinStartBet = 125f;
							}
							if (slotsClass.Machine.Equals("vw_prop_casino_slot_03a"))
							{
								MinStartBet = 125f;
							}
							if (slotsClass.Machine.Equals("vw_prop_casino_slot_04a"))
							{
								MinStartBet = 25f;
							}
							if (slotsClass.Machine.Equals("vw_prop_casino_slot_05a"))
							{
								MinStartBet = 2500f;
							}
							if (slotsClass.Machine.Equals("vw_prop_casino_slot_06a"))
							{
								MinStartBet = 500f;
							}
							if (slotsClass.Machine.Equals("vw_prop_casino_slot_07a"))
							{
								MinStartBet = 2500f;
							}
							if (slotsClass.Machine.Equals("vw_prop_casino_slot_08a"))
							{
								MinStartBet = 25f;
							}
							if (Chips_Amount >= MinStartBet)
							{
								DisplayHelpTextThisFrameNoSound("Press ~INPUT_CONTEXT~ to play " + ConvertSlotHashNametoActualName(slotsClass.Machine));
							}
							if (Chips_Amount <= MinStartBet)
							{
								DisplayHelpTextThisFrameNoSound("You must have atleast " + MinStartBet + "c to play " + ConvertSlotHashNametoActualName(slotsClass.Machine));
							}
						}
						if (SlotInUse)
						{
							if (SlotMachineStage == 1)
							{
								if (slotsClass.Machine.Equals("vw_prop_casino_slot_01a"))
								{
									DisplayHelpTextThisFrameNoSound("Bet One is 100 chips                             Bet Max is 500 chips");
								}
								if (slotsClass.Machine.Equals("vw_prop_casino_slot_02a"))
								{
									DisplayHelpTextThisFrameNoSound("Bet One is 25 chips                              Bet Max is 125 chips");
								}
								if (slotsClass.Machine.Equals("vw_prop_casino_slot_03a"))
								{
									DisplayHelpTextThisFrameNoSound("Bet One is 25 chips                              Bet Max is 125 chips");
								}
								if (slotsClass.Machine.Equals("vw_prop_casino_slot_04a"))
								{
									DisplayHelpTextThisFrameNoSound("Bet One is 5 chips                                  Bet Max is 25 chips");
								}
								if (slotsClass.Machine.Equals("vw_prop_casino_slot_05a"))
								{
									DisplayHelpTextThisFrameNoSound("Bet One is 500 chips                             Bet Max is 2500 chips");
								}
								if (slotsClass.Machine.Equals("vw_prop_casino_slot_06a"))
								{
									DisplayHelpTextThisFrameNoSound("Bet One is 100 chips                             Bet Max is 500 chips");
								}
								if (slotsClass.Machine.Equals("vw_prop_casino_slot_07a"))
								{
									DisplayHelpTextThisFrameNoSound("Bet One is 500 chips                             Bet Max is 2500 chips");
								}
								if (slotsClass.Machine.Equals("vw_prop_casino_slot_08a"))
								{
									DisplayHelpTextThisFrameNoSound("Bet One is 5 chips                                  Bet Max is 25 chips");
								}
							}
							if (SlotMachineStage == 2)
							{
								SetBet_SlotMachine();
								if (slotsClass.Machine.Equals("vw_prop_casino_slot_01a"))
								{
									if (Game.IsControlPressed(Control.VehicleHandbrake))
									{
										BetSlot = 100f;
										if (Chips_Amount - BetSlot >= BetSlot)
										{
											BetOne = true;
											BetMax = false;
											string dict = "anim_casino_a@amb@casino@games@slots@male";
											LoadDict(dict);
											LoadDict(dict);
											Random random = new Random();
											int num = random.Next(0, 300);
											string text = "";
											if (num >= 0 && num < 100)
											{
												text = "betidle_press_betone_a";
											}
											if (num >= 100 && num < 200)
											{
												text = "betidle_press_betone_b";
											}
											if (num >= 200 && num < 300)
											{
												text = "betidle_press_betone_c";
											}
											Vector3 position = CurrentSlotMachine.Position;
											SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
											{
												position.X,
												position.Y,
												position.Z,
												0.0,
												0.0,
												machineProp2.Rotation.Z,
												2
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict),
												text,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												0.0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												text,
												LoadDict(dict),
												3000.0,
												-2f,
												-2f,
												Game.Player.Character.Rotation.Z,
												1148846080,
												0
											});
											SlotMachineStage = 3;
										}
										else
										{
											UI.Notify(GetHostName() + " you dont have enough chips to play another round of this game!");
										}
									}
									if (Game.IsControlPressed(Control.PhoneSelect))
									{
										BetSlot = 500f;
										if (Chips_Amount - BetSlot >= BetSlot)
										{
											BetOne = false;
											BetMax = true;
											string dict2 = "anim_casino_a@amb@casino@games@slots@male";
											LoadDict(dict2);
											LoadDict(dict2);
											Random random2 = new Random();
											int num2 = random2.Next(0, 300);
											string text2 = "";
											if (num2 >= 0 && num2 < 100)
											{
												text2 = "betidle_press_betmax_a";
											}
											if (num2 >= 100 && num2 < 200)
											{
												text2 = "betidle_press_betmax_b";
											}
											if (num2 >= 200 && num2 < 300)
											{
												text2 = "betidle_press_betmax_c";
											}
											Vector3 position2 = CurrentSlotMachine.Position;
											SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
											{
												position2.X,
												position2.Y,
												position2.Z,
												0.0,
												0.0,
												machineProp2.Rotation.Z,
												2
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict2),
												text2,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												0.0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												text2,
												LoadDict(dict2),
												3000.0,
												-2f,
												-2f,
												Game.Player.Character.Rotation.Z,
												1148846080,
												0
											});
											SlotMachineStage = 3;
										}
										else
										{
											UI.Notify(GetHostName() + " you dont have enough chips to play another round of this game!");
										}
									}
									if (Game.IsControlPressed(Control.Sprint))
									{
										BetSlot = 500f;
										if (Chips_Amount - BetSlot >= BetSlot)
										{
											BetOne = false;
											BetMax = true;
											string dict3 = "anim_casino_a@amb@casino@games@slots@male";
											LoadDict(dict3);
											LoadDict(dict3);
											Random random3 = new Random();
											int num3 = random3.Next(0, 200);
											string text3 = "";
											string text4 = "";
											if (num3 >= 0 && num3 < 100)
											{
												text4 = "betidle_pull_spin_a_slotmachine";
												text3 = "betidle_pull_spin_a";
											}
											if (num3 >= 100 && num3 < 200)
											{
												text4 = "betidle_pull_spin_b_slotmachine";
												text3 = "betidle_pull_spin_b";
											}
											Vector3 position3 = CurrentSlotMachine.Position;
											SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
											{
												position3.X,
												position3.Y,
												position3.Z,
												0.0,
												0.0,
												machineProp2.Rotation.Z,
												2
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict3),
												text3,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												0.0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												text3,
												LoadDict(dict3),
												3000.0,
												-2f,
												-2f,
												Game.Player.Character.Rotation.Z,
												1148846080,
												0
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict3),
												text3,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												CurrentSlotMachine,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict3),
												text4,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												CurrentSlotMachine,
												SlotLocH[j].SlotSyncedScene,
												text4,
												LoadDict(dict3),
												3000.0,
												-2f,
												-2f,
												CurrentSlotMachine.Rotation.Z,
												1148846080,
												0
											});
											SlotMachineStage = 3;
										}
										else
										{
											UI.Notify(GetHostName() + " you dont have enough chips to play another round of this game!");
										}
									}
								}
								if (slotsClass.Machine.Equals("vw_prop_casino_slot_02a"))
								{
									if (Game.IsControlPressed(Control.VehicleHandbrake))
									{
										BetSlot = 25f;
										if (Chips_Amount - BetSlot >= BetSlot)
										{
											BetOne = true;
											BetMax = false;
											string dict4 = "anim_casino_a@amb@casino@games@slots@male";
											LoadDict(dict4);
											LoadDict(dict4);
											Random random4 = new Random();
											int num4 = random4.Next(0, 300);
											string text5 = "";
											if (num4 >= 0 && num4 < 100)
											{
												text5 = "betidle_press_betone_a";
											}
											if (num4 >= 100 && num4 < 200)
											{
												text5 = "betidle_press_betone_b";
											}
											if (num4 >= 200 && num4 < 300)
											{
												text5 = "betidle_press_betone_c";
											}
											Vector3 position4 = CurrentSlotMachine.Position;
											SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
											{
												position4.X,
												position4.Y,
												position4.Z,
												0.0,
												0.0,
												machineProp2.Rotation.Z,
												2
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict4),
												text5,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												0.0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												text5,
												LoadDict(dict4),
												3000.0,
												-2f,
												-2f,
												Game.Player.Character.Rotation.Z,
												1148846080,
												0
											});
											SlotMachineStage = 3;
										}
										else
										{
											UI.Notify(GetHostName() + " you dont have enough chips to play another round of this game!");
										}
									}
									if (Game.IsControlPressed(Control.PhoneSelect))
									{
										BetSlot = 125f;
										if (Chips_Amount - BetSlot >= BetSlot)
										{
											BetOne = false;
											BetMax = true;
											string dict5 = "anim_casino_a@amb@casino@games@slots@male";
											LoadDict(dict5);
											LoadDict(dict5);
											Random random5 = new Random();
											int num5 = random5.Next(0, 300);
											string text6 = "";
											if (num5 >= 0 && num5 < 100)
											{
												text6 = "betidle_press_betmax_a";
											}
											if (num5 >= 100 && num5 < 200)
											{
												text6 = "betidle_press_betmax_b";
											}
											if (num5 >= 200 && num5 < 300)
											{
												text6 = "betidle_press_betmax_c";
											}
											Vector3 position5 = CurrentSlotMachine.Position;
											SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
											{
												position5.X,
												position5.Y,
												position5.Z,
												0.0,
												0.0,
												machineProp2.Rotation.Z,
												2
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict5),
												text6,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												0.0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												text6,
												LoadDict(dict5),
												3000.0,
												-2f,
												-2f,
												Game.Player.Character.Rotation.Z,
												1148846080,
												0
											});
											SlotMachineStage = 3;
										}
										else
										{
											UI.Notify(GetHostName() + " you dont have enough chips to play another round of this game!");
										}
									}
									if (Game.IsControlPressed(Control.Sprint))
									{
										BetSlot = 125f;
										if (Chips_Amount - BetSlot >= BetSlot)
										{
											BetOne = false;
											BetMax = true;
											string dict6 = "anim_casino_a@amb@casino@games@slots@male";
											LoadDict(dict6);
											LoadDict(dict6);
											Random random6 = new Random();
											int num6 = random6.Next(0, 200);
											string text7 = "";
											string text8 = "";
											if (num6 >= 0 && num6 < 100)
											{
												text8 = "betidle_pull_spin_a_slotmachine";
												text7 = "betidle_pull_spin_a";
											}
											if (num6 >= 100 && num6 < 200)
											{
												text8 = "betidle_pull_spin_b_slotmachine";
												text7 = "betidle_pull_spin_b";
											}
											Vector3 position6 = CurrentSlotMachine.Position;
											SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
											{
												position6.X,
												position6.Y,
												position6.Z,
												0.0,
												0.0,
												machineProp2.Rotation.Z,
												2
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict6),
												text7,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												0.0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												text7,
												LoadDict(dict6),
												3000.0,
												-2f,
												-2f,
												Game.Player.Character.Rotation.Z,
												1148846080,
												0
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict6),
												text7,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												CurrentSlotMachine,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict6),
												text8,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												CurrentSlotMachine,
												SlotLocH[j].SlotSyncedScene,
												text8,
												LoadDict(dict6),
												3000.0,
												-2f,
												-2f,
												CurrentSlotMachine.Rotation.Z,
												1148846080,
												0
											});
											SlotMachineStage = 3;
										}
										else
										{
											UI.Notify(GetHostName() + " you dont have enough chips to play another round of this game!");
										}
									}
								}
								if (slotsClass.Machine.Equals("vw_prop_casino_slot_03a"))
								{
									if (Game.IsControlPressed(Control.VehicleHandbrake))
									{
										BetSlot = 25f;
										if (Chips_Amount - BetSlot >= BetSlot)
										{
											BetOne = true;
											BetMax = false;
											string dict7 = "anim_casino_a@amb@casino@games@slots@male";
											LoadDict(dict7);
											LoadDict(dict7);
											Random random7 = new Random();
											int num7 = random7.Next(0, 300);
											string text9 = "";
											if (num7 >= 0 && num7 < 100)
											{
												text9 = "betidle_press_betone_a";
											}
											if (num7 >= 100 && num7 < 200)
											{
												text9 = "betidle_press_betone_b";
											}
											if (num7 >= 200 && num7 < 300)
											{
												text9 = "betidle_press_betone_c";
											}
											Vector3 position7 = CurrentSlotMachine.Position;
											SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
											{
												position7.X,
												position7.Y,
												position7.Z,
												0.0,
												0.0,
												machineProp2.Rotation.Z,
												2
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict7),
												text9,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												0.0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												text9,
												LoadDict(dict7),
												3000.0,
												-2f,
												-2f,
												Game.Player.Character.Rotation.Z,
												1148846080,
												0
											});
											SlotMachineStage = 3;
										}
										else
										{
											UI.Notify(GetHostName() + " you dont have enough chips to play another round of this game!");
										}
									}
									if (Game.IsControlPressed(Control.PhoneSelect))
									{
										BetSlot = 125f;
										if (Chips_Amount - BetSlot >= BetSlot)
										{
											BetOne = false;
											BetMax = true;
											string dict8 = "anim_casino_a@amb@casino@games@slots@male";
											LoadDict(dict8);
											LoadDict(dict8);
											Random random8 = new Random();
											int num8 = random8.Next(0, 300);
											string text10 = "";
											if (num8 >= 0 && num8 < 100)
											{
												text10 = "betidle_press_betmax_a";
											}
											if (num8 >= 100 && num8 < 200)
											{
												text10 = "betidle_press_betmax_b";
											}
											if (num8 >= 200 && num8 < 300)
											{
												text10 = "betidle_press_betmax_c";
											}
											Vector3 position8 = CurrentSlotMachine.Position;
											SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
											{
												position8.X,
												position8.Y,
												position8.Z,
												0.0,
												0.0,
												machineProp2.Rotation.Z,
												2
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict8),
												text10,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												0.0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												text10,
												LoadDict(dict8),
												3000.0,
												-2f,
												-2f,
												Game.Player.Character.Rotation.Z,
												1148846080,
												0
											});
											SlotMachineStage = 3;
										}
										else
										{
											UI.Notify(GetHostName() + " you dont have enough chips to play another round of this game!");
										}
									}
									if (Game.IsControlPressed(Control.Sprint))
									{
										BetSlot = 125f;
										if (Chips_Amount - BetSlot >= BetSlot)
										{
											BetOne = false;
											BetMax = true;
											string dict9 = "anim_casino_a@amb@casino@games@slots@male";
											LoadDict(dict9);
											LoadDict(dict9);
											Random random9 = new Random();
											int num9 = random9.Next(0, 200);
											string text11 = "";
											string text12 = "";
											if (num9 >= 0 && num9 < 100)
											{
												text12 = "betidle_pull_spin_a_slotmachine";
												text11 = "betidle_pull_spin_a";
											}
											if (num9 >= 100 && num9 < 200)
											{
												text12 = "betidle_pull_spin_b_slotmachine";
												text11 = "betidle_pull_spin_b";
											}
											Vector3 position9 = CurrentSlotMachine.Position;
											SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
											{
												position9.X,
												position9.Y,
												position9.Z,
												0.0,
												0.0,
												machineProp2.Rotation.Z,
												2
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict9),
												text11,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												0.0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												text11,
												LoadDict(dict9),
												3000.0,
												-2f,
												-2f,
												Game.Player.Character.Rotation.Z,
												1148846080,
												0
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict9),
												text11,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												CurrentSlotMachine,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict9),
												text12,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												CurrentSlotMachine,
												SlotLocH[j].SlotSyncedScene,
												text12,
												LoadDict(dict9),
												3000.0,
												-2f,
												-2f,
												CurrentSlotMachine.Rotation.Z,
												1148846080,
												0
											});
											SlotMachineStage = 3;
										}
										else
										{
											UI.Notify(GetHostName() + " you dont have enough chips to play another round of this game!");
										}
									}
								}
								if (slotsClass.Machine.Equals("vw_prop_casino_slot_04a"))
								{
									if (Game.IsControlPressed(Control.VehicleHandbrake))
									{
										BetSlot = 5f;
										if (Chips_Amount - BetSlot >= BetSlot)
										{
											BetOne = true;
											BetMax = false;
											string dict10 = "anim_casino_a@amb@casino@games@slots@male";
											LoadDict(dict10);
											LoadDict(dict10);
											Random random10 = new Random();
											int num10 = random10.Next(0, 300);
											string text13 = "";
											if (num10 >= 0 && num10 < 100)
											{
												text13 = "betidle_press_betone_a";
											}
											if (num10 >= 100 && num10 < 200)
											{
												text13 = "betidle_press_betone_b";
											}
											if (num10 >= 200 && num10 < 300)
											{
												text13 = "betidle_press_betone_c";
											}
											Vector3 position10 = CurrentSlotMachine.Position;
											SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
											{
												position10.X,
												position10.Y,
												position10.Z,
												0.0,
												0.0,
												machineProp2.Rotation.Z,
												2
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict10),
												text13,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												0.0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												text13,
												LoadDict(dict10),
												3000.0,
												-2f,
												-2f,
												Game.Player.Character.Rotation.Z,
												1148846080,
												0
											});
											SlotMachineStage = 3;
										}
										else
										{
											UI.Notify(GetHostName() + " you dont have enough chips to play another round of this game!");
										}
									}
									if (Game.IsControlPressed(Control.PhoneSelect))
									{
										BetSlot = 25f;
										if (Chips_Amount - BetSlot >= BetSlot)
										{
											BetOne = false;
											BetMax = true;
											string dict11 = "anim_casino_a@amb@casino@games@slots@male";
											LoadDict(dict11);
											LoadDict(dict11);
											Random random11 = new Random();
											int num11 = random11.Next(0, 300);
											string text14 = "";
											if (num11 >= 0 && num11 < 100)
											{
												text14 = "betidle_press_betmax_a";
											}
											if (num11 >= 100 && num11 < 200)
											{
												text14 = "betidle_press_betmax_b";
											}
											if (num11 >= 200 && num11 < 300)
											{
												text14 = "betidle_press_betmax_c";
											}
											Vector3 position11 = CurrentSlotMachine.Position;
											SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
											{
												position11.X,
												position11.Y,
												position11.Z,
												0.0,
												0.0,
												machineProp2.Rotation.Z,
												2
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict11),
												text14,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												0.0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												text14,
												LoadDict(dict11),
												3000.0,
												-2f,
												-2f,
												Game.Player.Character.Rotation.Z,
												1148846080,
												0
											});
											SlotMachineStage = 3;
										}
										else
										{
											UI.Notify(GetHostName() + " you dont have enough chips to play another round of this game!");
										}
									}
									if (Game.IsControlPressed(Control.Sprint))
									{
										BetSlot = 25f;
										if (Chips_Amount - BetSlot >= BetSlot)
										{
											BetOne = false;
											BetMax = true;
											string dict12 = "anim_casino_a@amb@casino@games@slots@male";
											LoadDict(dict12);
											LoadDict(dict12);
											Random random12 = new Random();
											int num12 = random12.Next(0, 200);
											string text15 = "";
											string text16 = "";
											if (num12 >= 0 && num12 < 100)
											{
												text16 = "betidle_pull_spin_a_slotmachine";
												text15 = "betidle_pull_spin_a";
											}
											if (num12 >= 100 && num12 < 200)
											{
												text16 = "betidle_pull_spin_b_slotmachine";
												text15 = "betidle_pull_spin_b";
											}
											Vector3 position12 = CurrentSlotMachine.Position;
											SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
											{
												position12.X,
												position12.Y,
												position12.Z,
												0.0,
												0.0,
												machineProp2.Rotation.Z,
												2
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict12),
												text15,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												0.0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												text15,
												LoadDict(dict12),
												3000.0,
												-2f,
												-2f,
												Game.Player.Character.Rotation.Z,
												1148846080,
												0
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict12),
												text15,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												CurrentSlotMachine,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict12),
												text16,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												CurrentSlotMachine,
												SlotLocH[j].SlotSyncedScene,
												text16,
												LoadDict(dict12),
												3000.0,
												-2f,
												-2f,
												CurrentSlotMachine.Rotation.Z,
												1148846080,
												0
											});
											SlotMachineStage = 3;
										}
										else
										{
											UI.Notify(GetHostName() + " you dont have enough chips to play another round of this game!");
										}
									}
								}
								if (slotsClass.Machine.Equals("vw_prop_casino_slot_05a"))
								{
									if (Game.IsControlPressed(Control.VehicleHandbrake))
									{
										BetSlot = 500f;
										if (Chips_Amount - BetSlot >= BetSlot)
										{
											BetOne = true;
											BetMax = false;
											string dict13 = "anim_casino_a@amb@casino@games@slots@male";
											LoadDict(dict13);
											LoadDict(dict13);
											Random random13 = new Random();
											int num13 = random13.Next(0, 300);
											string text17 = "";
											if (num13 >= 0 && num13 < 100)
											{
												text17 = "betidle_press_betone_a";
											}
											if (num13 >= 100 && num13 < 200)
											{
												text17 = "betidle_press_betone_b";
											}
											if (num13 >= 200 && num13 < 300)
											{
												text17 = "betidle_press_betone_c";
											}
											Vector3 position13 = CurrentSlotMachine.Position;
											SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
											{
												position13.X,
												position13.Y,
												position13.Z,
												0.0,
												0.0,
												machineProp2.Rotation.Z,
												2
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict13),
												text17,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												0.0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												text17,
												LoadDict(dict13),
												3000.0,
												-2f,
												-2f,
												Game.Player.Character.Rotation.Z,
												1148846080,
												0
											});
											SlotMachineStage = 3;
										}
										else
										{
											UI.Notify(GetHostName() + " you dont have enough chips to play another round of this game!");
										}
									}
									if (Game.IsControlPressed(Control.PhoneSelect))
									{
										BetSlot = 2500f;
										if (Chips_Amount - BetSlot >= BetSlot)
										{
											BetOne = false;
											BetMax = true;
											string dict14 = "anim_casino_a@amb@casino@games@slots@male";
											LoadDict(dict14);
											LoadDict(dict14);
											Random random14 = new Random();
											int num14 = random14.Next(0, 300);
											string text18 = "";
											if (num14 >= 0 && num14 < 100)
											{
												text18 = "betidle_press_betmax_a";
											}
											if (num14 >= 100 && num14 < 200)
											{
												text18 = "betidle_press_betmax_b";
											}
											if (num14 >= 200 && num14 < 300)
											{
												text18 = "betidle_press_betmax_c";
											}
											Vector3 position14 = CurrentSlotMachine.Position;
											SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
											{
												position14.X,
												position14.Y,
												position14.Z,
												0.0,
												0.0,
												machineProp2.Rotation.Z,
												2
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict14),
												text18,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												0.0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												text18,
												LoadDict(dict14),
												3000.0,
												-2f,
												-2f,
												Game.Player.Character.Rotation.Z,
												1148846080,
												0
											});
											SlotMachineStage = 3;
										}
										else
										{
											UI.Notify(GetHostName() + " you dont have enough chips to play another round of this game!");
										}
									}
									if (Game.IsControlPressed(Control.Sprint))
									{
										BetSlot = 2500f;
										if (Chips_Amount - BetSlot >= BetSlot)
										{
											BetOne = false;
											BetMax = true;
											string dict15 = "anim_casino_a@amb@casino@games@slots@male";
											LoadDict(dict15);
											LoadDict(dict15);
											Random random15 = new Random();
											int num15 = random15.Next(0, 200);
											string text19 = "";
											string text20 = "";
											if (num15 >= 0 && num15 < 100)
											{
												text20 = "betidle_pull_spin_a_slotmachine";
												text19 = "betidle_pull_spin_a";
											}
											if (num15 >= 100 && num15 < 200)
											{
												text20 = "betidle_pull_spin_b_slotmachine";
												text19 = "betidle_pull_spin_b";
											}
											Vector3 position15 = CurrentSlotMachine.Position;
											SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
											{
												position15.X,
												position15.Y,
												position15.Z,
												0.0,
												0.0,
												machineProp2.Rotation.Z,
												2
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict15),
												text19,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												0.0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												text19,
												LoadDict(dict15),
												3000.0,
												-2f,
												-2f,
												Game.Player.Character.Rotation.Z,
												1148846080,
												0
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict15),
												text19,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												CurrentSlotMachine,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict15),
												text20,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												CurrentSlotMachine,
												SlotLocH[j].SlotSyncedScene,
												text20,
												LoadDict(dict15),
												3000.0,
												-2f,
												-2f,
												CurrentSlotMachine.Rotation.Z,
												1148846080,
												0
											});
											SlotMachineStage = 3;
										}
										else
										{
											UI.Notify(GetHostName() + " you dont have enough chips to play another round of this game!");
										}
									}
								}
								if (slotsClass.Machine.Equals("vw_prop_casino_slot_06a"))
								{
									if (Game.IsControlPressed(Control.VehicleHandbrake))
									{
										BetSlot = 100f;
										if (Chips_Amount - BetSlot >= BetSlot)
										{
											BetOne = true;
											BetMax = false;
											string dict16 = "anim_casino_a@amb@casino@games@slots@male";
											LoadDict(dict16);
											LoadDict(dict16);
											Random random16 = new Random();
											int num16 = random16.Next(0, 300);
											string text21 = "";
											if (num16 >= 0 && num16 < 100)
											{
												text21 = "betidle_press_betone_a";
											}
											if (num16 >= 100 && num16 < 200)
											{
												text21 = "betidle_press_betone_b";
											}
											if (num16 >= 200 && num16 < 300)
											{
												text21 = "betidle_press_betone_c";
											}
											Vector3 position16 = CurrentSlotMachine.Position;
											SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
											{
												position16.X,
												position16.Y,
												position16.Z,
												0.0,
												0.0,
												machineProp2.Rotation.Z,
												2
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict16),
												text21,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												0.0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												text21,
												LoadDict(dict16),
												3000.0,
												-2f,
												-2f,
												Game.Player.Character.Rotation.Z,
												1148846080,
												0
											});
											SlotMachineStage = 3;
										}
										else
										{
											UI.Notify(GetHostName() + " you dont have enough chips to play another round of this game!");
										}
									}
									if (Game.IsControlPressed(Control.PhoneSelect))
									{
										BetSlot = 500f;
										if (Chips_Amount - BetSlot >= BetSlot)
										{
											BetOne = false;
											BetMax = true;
											string dict17 = "anim_casino_a@amb@casino@games@slots@male";
											LoadDict(dict17);
											LoadDict(dict17);
											Random random17 = new Random();
											int num17 = random17.Next(0, 300);
											string text22 = "";
											if (num17 >= 0 && num17 < 100)
											{
												text22 = "betidle_press_betmax_a";
											}
											if (num17 >= 100 && num17 < 200)
											{
												text22 = "betidle_press_betmax_b";
											}
											if (num17 >= 200 && num17 < 300)
											{
												text22 = "betidle_press_betmax_c";
											}
											Vector3 position17 = CurrentSlotMachine.Position;
											SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
											{
												position17.X,
												position17.Y,
												position17.Z,
												0.0,
												0.0,
												machineProp2.Rotation.Z,
												2
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict17),
												text22,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												0.0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												text22,
												LoadDict(dict17),
												3000.0,
												-2f,
												-2f,
												Game.Player.Character.Rotation.Z,
												1148846080,
												0
											});
											SlotMachineStage = 3;
										}
										else
										{
											UI.Notify(GetHostName() + " you dont have enough chips to play another round of this game!");
										}
									}
									if (Game.IsControlPressed(Control.Sprint))
									{
										BetSlot = 500f;
										if (Chips_Amount - BetSlot >= BetSlot)
										{
											BetOne = false;
											BetMax = true;
											string dict18 = "anim_casino_a@amb@casino@games@slots@male";
											LoadDict(dict18);
											LoadDict(dict18);
											Random random18 = new Random();
											int num18 = random18.Next(0, 200);
											string text23 = "";
											string text24 = "";
											if (num18 >= 0 && num18 < 100)
											{
												text24 = "betidle_pull_spin_a_slotmachine";
												text23 = "betidle_pull_spin_a";
											}
											if (num18 >= 100 && num18 < 200)
											{
												text24 = "betidle_pull_spin_b_slotmachine";
												text23 = "betidle_pull_spin_b";
											}
											Vector3 position18 = CurrentSlotMachine.Position;
											SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
											{
												position18.X,
												position18.Y,
												position18.Z,
												0.0,
												0.0,
												machineProp2.Rotation.Z,
												2
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict18),
												text23,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												0.0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												text23,
												LoadDict(dict18),
												3000.0,
												-2f,
												-2f,
												Game.Player.Character.Rotation.Z,
												1148846080,
												0
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict18),
												text23,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												CurrentSlotMachine,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict18),
												text24,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												CurrentSlotMachine,
												SlotLocH[j].SlotSyncedScene,
												text24,
												LoadDict(dict18),
												3000.0,
												-2f,
												-2f,
												CurrentSlotMachine.Rotation.Z,
												1148846080,
												0
											});
											SlotMachineStage = 3;
										}
										else
										{
											UI.Notify(GetHostName() + " you dont have enough chips to play another round of this game!");
										}
									}
								}
								if (slotsClass.Machine.Equals("vw_prop_casino_slot_07a"))
								{
									if (Game.IsControlPressed(Control.VehicleHandbrake))
									{
										BetSlot = 500f;
										if (Chips_Amount - BetSlot >= BetSlot)
										{
											BetOne = true;
											BetMax = false;
											string dict19 = "anim_casino_a@amb@casino@games@slots@male";
											LoadDict(dict19);
											LoadDict(dict19);
											Random random19 = new Random();
											int num19 = random19.Next(0, 300);
											string text25 = "";
											if (num19 >= 0 && num19 < 100)
											{
												text25 = "betidle_press_betone_a";
											}
											if (num19 >= 100 && num19 < 200)
											{
												text25 = "betidle_press_betone_b";
											}
											if (num19 >= 200 && num19 < 300)
											{
												text25 = "betidle_press_betone_c";
											}
											Vector3 position19 = CurrentSlotMachine.Position;
											SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
											{
												position19.X,
												position19.Y,
												position19.Z,
												0.0,
												0.0,
												machineProp2.Rotation.Z,
												2
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict19),
												text25,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												0.0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												text25,
												LoadDict(dict19),
												3000.0,
												-2f,
												-2f,
												Game.Player.Character.Rotation.Z,
												1148846080,
												0
											});
											SlotMachineStage = 3;
										}
										else
										{
											UI.Notify(GetHostName() + " you dont have enough chips to play another round of this game!");
										}
									}
									if (Game.IsControlPressed(Control.PhoneSelect))
									{
										BetSlot = 2500f;
										if (Chips_Amount - BetSlot >= BetSlot)
										{
											BetOne = false;
											BetMax = true;
											string dict20 = "anim_casino_a@amb@casino@games@slots@male";
											LoadDict(dict20);
											LoadDict(dict20);
											Random random20 = new Random();
											int num20 = random20.Next(0, 300);
											string text26 = "";
											if (num20 >= 0 && num20 < 100)
											{
												text26 = "betidle_press_betmax_a";
											}
											if (num20 >= 100 && num20 < 200)
											{
												text26 = "betidle_press_betmax_b";
											}
											if (num20 >= 200 && num20 < 300)
											{
												text26 = "betidle_press_betmax_c";
											}
											Vector3 position20 = CurrentSlotMachine.Position;
											SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
											{
												position20.X,
												position20.Y,
												position20.Z,
												0.0,
												0.0,
												machineProp2.Rotation.Z,
												2
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict20),
												text26,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												0.0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												text26,
												LoadDict(dict20),
												3000.0,
												-2f,
												-2f,
												Game.Player.Character.Rotation.Z,
												1148846080,
												0
											});
											SlotMachineStage = 3;
										}
										else
										{
											UI.Notify(GetHostName() + " you dont have enough chips to play another round of this game!");
										}
									}
									if (Game.IsControlPressed(Control.Sprint))
									{
										BetSlot = 2500f;
										if (Chips_Amount - BetSlot >= BetSlot)
										{
											BetOne = false;
											BetMax = true;
											string dict21 = "anim_casino_a@amb@casino@games@slots@male";
											LoadDict(dict21);
											LoadDict(dict21);
											Random random21 = new Random();
											int num21 = random21.Next(0, 200);
											string text27 = "";
											string text28 = "";
											if (num21 >= 0 && num21 < 100)
											{
												text28 = "betidle_pull_spin_a_slotmachine";
												text27 = "betidle_pull_spin_a";
											}
											if (num21 >= 100 && num21 < 200)
											{
												text28 = "betidle_pull_spin_b_slotmachine";
												text27 = "betidle_pull_spin_b";
											}
											Vector3 position21 = CurrentSlotMachine.Position;
											SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
											{
												position21.X,
												position21.Y,
												position21.Z,
												0.0,
												0.0,
												machineProp2.Rotation.Z,
												2
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict21),
												text27,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												0.0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												text27,
												LoadDict(dict21),
												3000.0,
												-2f,
												-2f,
												Game.Player.Character.Rotation.Z,
												1148846080,
												0
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict21),
												text27,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												CurrentSlotMachine,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict21),
												text28,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												CurrentSlotMachine,
												SlotLocH[j].SlotSyncedScene,
												text28,
												LoadDict(dict21),
												3000.0,
												-2f,
												-2f,
												CurrentSlotMachine.Rotation.Z,
												1148846080,
												0
											});
											SlotMachineStage = 3;
										}
										else
										{
											UI.Notify(GetHostName() + " you dont have enough chips to play another round of this game!");
										}
									}
								}
								if (slotsClass.Machine.Equals("vw_prop_casino_slot_08a"))
								{
									if (Game.IsControlPressed(Control.VehicleHandbrake))
									{
										BetSlot = 5f;
										if (Chips_Amount - BetSlot >= BetSlot)
										{
											BetOne = true;
											BetMax = false;
											string dict22 = "anim_casino_a@amb@casino@games@slots@male";
											LoadDict(dict22);
											LoadDict(dict22);
											Random random22 = new Random();
											int num22 = random22.Next(0, 300);
											string text29 = "";
											if (num22 >= 0 && num22 < 100)
											{
												text29 = "betidle_press_betone_a";
											}
											if (num22 >= 100 && num22 < 200)
											{
												text29 = "betidle_press_betone_b";
											}
											if (num22 >= 200 && num22 < 300)
											{
												text29 = "betidle_press_betone_c";
											}
											Vector3 position22 = CurrentSlotMachine.Position;
											SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
											{
												position22.X,
												position22.Y,
												position22.Z,
												0.0,
												0.0,
												machineProp2.Rotation.Z,
												2
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict22),
												text29,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												0.0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												text29,
												LoadDict(dict22),
												3000.0,
												-2f,
												-2f,
												Game.Player.Character.Rotation.Z,
												1148846080,
												0
											});
											SlotMachineStage = 3;
										}
										else
										{
											UI.Notify(GetHostName() + " you dont have enough chips to play another round of this game!");
										}
									}
									if (Game.IsControlPressed(Control.PhoneSelect))
									{
										BetSlot = 25f;
										if (Chips_Amount - BetSlot >= BetSlot)
										{
											BetOne = false;
											BetMax = true;
											string dict23 = "anim_casino_a@amb@casino@games@slots@male";
											LoadDict(dict23);
											LoadDict(dict23);
											Random random23 = new Random();
											int num23 = random23.Next(0, 300);
											string text30 = "";
											if (num23 >= 0 && num23 < 100)
											{
												text30 = "betidle_press_betmax_a";
											}
											if (num23 >= 100 && num23 < 200)
											{
												text30 = "betidle_press_betmax_b";
											}
											if (num23 >= 200 && num23 < 300)
											{
												text30 = "betidle_press_betmax_c";
											}
											Vector3 position23 = CurrentSlotMachine.Position;
											SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
											{
												position23.X,
												position23.Y,
												position23.Z,
												0.0,
												0.0,
												machineProp2.Rotation.Z,
												2
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict23),
												text30,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												0.0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												text30,
												LoadDict(dict23),
												3000.0,
												-2f,
												-2f,
												Game.Player.Character.Rotation.Z,
												1148846080,
												0
											});
											SlotMachineStage = 3;
										}
										else
										{
											UI.Notify(GetHostName() + " you dont have enough chips to play another round of this game!");
										}
									}
									if (Game.IsControlPressed(Control.Sprint))
									{
										BetSlot = 25f;
										if (Chips_Amount - BetSlot >= BetSlot)
										{
											BetOne = false;
											BetMax = true;
											string dict24 = "anim_casino_a@amb@casino@games@slots@male";
											LoadDict(dict24);
											LoadDict(dict24);
											Random random24 = new Random();
											int num24 = random24.Next(0, 200);
											string text31 = "";
											string text32 = "";
											if (num24 >= 0 && num24 < 100)
											{
												text32 = "betidle_pull_spin_a_slotmachine";
												text31 = "betidle_pull_spin_a";
											}
											if (num24 >= 100 && num24 < 200)
											{
												text32 = "betidle_pull_spin_b_slotmachine";
												text31 = "betidle_pull_spin_b";
											}
											Vector3 position24 = CurrentSlotMachine.Position;
											SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
											{
												position24.X,
												position24.Y,
												position24.Z,
												0.0,
												0.0,
												machineProp2.Rotation.Z,
												2
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict24),
												text31,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
											});
											Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
											{
												SlotLocH[j].SlotSyncedScene,
												0.0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												text31,
												LoadDict(dict24),
												3000.0,
												-2f,
												-2f,
												Game.Player.Character.Rotation.Z,
												1148846080,
												0
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												Game.Player.Character,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict24),
												text31,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
											{
												CurrentSlotMachine,
												SlotLocH[j].SlotSyncedScene,
												LoadDict(dict24),
												text32,
												3000.0,
												-2.0,
												128,
												0,
												1148846080,
												0
											});
											Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
											{
												CurrentSlotMachine,
												SlotLocH[j].SlotSyncedScene,
												text32,
												LoadDict(dict24),
												3000.0,
												-2f,
												-2f,
												CurrentSlotMachine.Rotation.Z,
												1148846080,
												0
											});
											SlotMachineStage = 3;
										}
										else
										{
											UI.Notify(GetHostName() + " you dont have enough chips to play another round of this game!");
										}
									}
								}
							}
							if (SlotMachineStage == 3)
							{
								if (!SpinnedReels)
								{
									Game.Player.Money -= (int)BetSlot;
									if (BetOne)
									{
										SlotMultiplier = 1f;
									}
									if (BetMax)
									{
										SlotMultiplier = 5f;
									}
									UI.Notify("~y~ " + BetSlot.ToString("N") + "c~w~ deducted from bank");
									Script.Wait(1);
									ReelA_SpinAmt = GetRandomfloat(6500, 15500);
									Script.Wait(1);
									float num25 = GetRandomfloat(6500, 15500);
									if (num25 < ReelA_SpinAmt)
									{
										num25 += GetRandomfloat(6500, 15500);
									}
									ReelB_SpinAmt = num25;
									Script.Wait(1);
									num25 = GetRandomfloat(6500, 15500);
									if (num25 < ReelB_SpinAmt)
									{
										num25 += GetRandomfloat(6500, 15500);
									}
									ReelC_SpinAmt = num25;
									Script.Wait(500);
									SpinnedReels = true;
								}
								if (Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene }) >= 1065353216)
								{
									string dict25 = "anim_casino_a@amb@casino@games@slots@male";
									LoadDict(dict25);
									LoadDict(dict25);
									Random random25 = new Random();
									int num26 = random25.Next(0, 300);
									string text33 = "";
									if (num26 >= 0 && num26 < 100)
									{
										text33 = "spinning_a";
									}
									if (num26 >= 100 && num26 < 200)
									{
										text33 = "spinning_b";
									}
									if (num26 >= 200 && num26 < 300)
									{
										text33 = "spinning_c";
									}
									Vector3 position25 = CurrentSlotMachine.Position;
									SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
									{
										position25.X,
										position25.Y,
										position25.Z,
										0.0,
										0.0,
										machineProp2.Rotation.Z,
										2
									});
									Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
									{
										Game.Player.Character,
										SlotLocH[j].SlotSyncedScene,
										LoadDict(dict25),
										text33,
										3000.0,
										-2.0,
										128,
										0,
										1148846080,
										0
									});
									Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
									{
										SlotLocH[j].SlotSyncedScene,
										Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
									});
									Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
									{
										SlotLocH[j].SlotSyncedScene,
										0.0
									});
									Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
									{
										Game.Player.Character,
										SlotLocH[j].SlotSyncedScene,
										text33,
										LoadDict(dict25),
										3000.0,
										-2f,
										-2f,
										Game.Player.Character.Rotation.Z,
										1148846080,
										0
									});
								}
								if (SpinnedReels)
								{
									if (ReelABase != null && !FinishedReelA)
									{
										Vector3 rotation = ReelABase.Rotation;
										ReelABase.Rotation = new Vector3(rotation.X, rotation.Y += 10f, rotation.Z);
										ReelA_SpinAmt -= 0.05f;
										if (ReelA_SpinAmt < 0.05f)
										{
											ReelA_SpinAmt = 0f;
											FinishedReelA = true;
											ReelABase.Rotation = new Vector3(rotation.X, GetClosest(rotation.Y, CurrentSlotClass.Machine), rotation.Z);
										}
									}
									if (ReelBBase != null)
									{
										if (!FinishedReelB && ReelB_SpinAmt >= 0f)
										{
											Vector3 rotation2 = ReelBBase.Rotation;
											ReelBBase.Rotation = new Vector3(rotation2.X, rotation2.Y += ReelB_SpinAmt, rotation2.Z);
											ReelB_SpinAmt -= 0.05f;
											if (ReelB_SpinAmt < 0.05f)
											{
												ReelA_SpinAmt = 0f;
												ReelB_SpinAmt = 0f;
												FinishedReelB = true;
												ReelBBase.Rotation = new Vector3(rotation2.X, GetClosest(rotation2.Y, CurrentSlotClass.Machine), rotation2.Z);
											}
										}
										if (ReelA_SpinAmt >= 0f)
										{
											Vector3 rotation3 = ReelBBase.Rotation;
											ReelBBase.Rotation = new Vector3(rotation3.X, rotation3.Y += ReelB_SpinAmt, rotation3.Z);
										}
									}
									if (ReelCBase != null)
									{
										if (ReelA_SpinAmt <= 0f && ReelB_SpinAmt <= 0f)
										{
											if (!FinishedReelC)
											{
												Vector3 rotation4 = ReelCBase.Rotation;
												ReelCBase.Rotation = new Vector3(rotation4.X, rotation4.Y += ReelC_SpinAmt, rotation4.Z);
												ReelC_SpinAmt -= 0.05f;
												if (ReelC_SpinAmt < 0.05f)
												{
													ReelA_SpinAmt = 0f;
													ReelB_SpinAmt = 0f;
													ReelC_SpinAmt = 0f;
													FinishedReelC = true;
													ReelCBase.Rotation = new Vector3(rotation4.X, GetClosest(rotation4.Y, CurrentSlotClass.Machine), rotation4.Z);
												}
											}
										}
										else
										{
											Vector3 rotation5 = ReelCBase.Rotation;
											ReelCBase.Rotation = new Vector3(rotation5.X, rotation5.Y += ReelC_SpinAmt, rotation5.Z);
										}
									}
									if (FinishedReelA && FinishedReelB && FinishedReelC)
									{
										Vector3 rotation6 = ReelABase.Rotation;
										Vector3 rotation7 = ReelBBase.Rotation;
										Vector3 rotation8 = ReelCBase.Rotation;
										ALandedOn = GetLandedOn(rotation6.Y, CurrentSlotClass.Machine);
										BLandedOn = GetLandedOn(rotation7.Y, CurrentSlotClass.Machine);
										CLandedOn = GetLandedOn(rotation8.Y, CurrentSlotClass.Machine);
										SlotMachineStage = 4;
									}
								}
							}
							if (SlotMachineStage == 4)
							{
								UI.ShowSubtitle("Calculate Totals (" + ALandedOn + "_" + BLandedOn + "_" + CLandedOn + ")");
								float num27 = CalcualteSlotWin(CurrentSlotClass.Machine);
								if (num27 >= 1f)
								{
									UI.Notify(GetHostName() + " You won ~y~" + num27.ToString("N") + "~c!");
									Game.Player.Money += (int)num27;
									SlotMachineStage = 2;
									SpinnedReels = false;
									ReelA_SpinAmt = GetRandomfloat(6500, 15500);
									Script.Wait(1);
									float num28 = GetRandomfloat(6500, 15500);
									if (num28 < ReelA_SpinAmt)
									{
										num28 += GetRandomfloat(6500, 15500);
									}
									ReelB_SpinAmt = num28;
									Script.Wait(1);
									num28 = GetRandomfloat(6500, 15500);
									if (num28 < ReelB_SpinAmt)
									{
										num28 += GetRandomfloat(6500, 15500);
									}
									ReelC_SpinAmt = num28;
									FinishedReelA = false;
									FinishedReelB = false;
									FinishedReelC = false;
								}
								if (num27 <= 0f)
								{
									UI.Notify(GetHostName() + " You won nothing");
									SlotMachineStage = 2;
									SpinnedReels = false;
									ReelA_SpinAmt = GetRandomfloat(6500, 15500);
									Script.Wait(1);
									float num29 = GetRandomfloat(6500, 15500);
									if (num29 < ReelA_SpinAmt)
									{
										num29 += GetRandomfloat(6500, 15500);
									}
									ReelB_SpinAmt = num29;
									Script.Wait(1);
									num29 = GetRandomfloat(6500, 15500);
									if (num29 < ReelB_SpinAmt)
									{
										num29 += GetRandomfloat(6500, 15500);
									}
									ReelC_SpinAmt = num29;
									FinishedReelA = false;
									FinishedReelB = false;
									FinishedReelC = false;
									FinishedReelA = false;
								}
							}
						}
						if (Game.IsControlPressed(Control.Context))
						{
							if (SlotInUse && SlotMachineStage == 2)
							{
								CurrentSlotClass = null;
								string dict26 = "anim_casino_a@amb@casino@games@slots@male";
								LoadDict(dict26);
								LoadDict(dict26);
								Vector3 position26 = CurrentSlotMachine.Position;
								SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
								{
									position26.X,
									position26.Y,
									position26.Z,
									0.0,
									0.0,
									machineProp2.Rotation.Z,
									2
								});
								Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
								{
									Game.Player.Character,
									SlotLocH[j].SlotSyncedScene,
									LoadDict(dict26),
									"exit_left",
									3000.0,
									-2.0,
									128,
									0,
									1148846080,
									0
								});
								Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
								{
									SlotLocH[j].SlotSyncedScene,
									Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
								});
								Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
								{
									SlotLocH[j].SlotSyncedScene,
									0.0
								});
								Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
								{
									Game.Player.Character,
									SlotLocH[j].SlotSyncedScene,
									"exit_left",
									LoadDict(dict26),
									3000.0,
									-2f,
									-2f,
									Game.Player.Character.Rotation.Z,
									1148846080,
									0
								});
								SlotMachineStage = -1;
								SpinnedReels = false;
								if (ReelABase != null)
								{
									ReelABase.Delete();
								}
								if (ReelBBase != null)
								{
									ReelBBase.Delete();
								}
								if (ReelCBase != null)
								{
									ReelCBase.Delete();
								}
								if (SlotProp1 != null)
								{
									SlotProp1.Delete();
								}
								if (SlotProp2 != null)
								{
									SlotProp2.Delete();
								}
								if (SlotProp3 != null)
								{
									SlotProp3.Delete();
								}
								SlotProp1 = null;
								SlotProp2 = null;
								SlotProp3 = null;
								FinishedReelA = false;
								FinishedReelB = false;
								FinishedReelC = false;
							}
							if (!SlotInUse && SlotMachineStage == 0 && Chips_Amount >= MinStartBet)
							{
								if (ReelABase != null)
								{
									ReelABase.Delete();
								}
								if (ReelBBase != null)
								{
									ReelBBase.Delete();
								}
								if (ReelCBase != null)
								{
									ReelCBase.Delete();
								}
								if (SlotProp1 != null)
								{
									SlotProp1.Delete();
								}
								if (SlotProp2 != null)
								{
									SlotProp2.Delete();
								}
								if (SlotProp3 != null)
								{
									SlotProp3.Delete();
								}
								SlotProp1 = null;
								SlotProp2 = null;
								SlotProp3 = null;
								Vector3 position27 = Function.Call<Vector3>(((Hash)0x163E252DE035A133uL), new InputArgument[7]
								{
									slotsClass.Loc.X,
									slotsClass.Loc.Y,
									slotsClass.Loc.Z,
									slotsClass.Heading,
									-0.115f,
									0.047f,
									0.906f
								});
								Vector3 position28 = Function.Call<Vector3>(((Hash)0x163E252DE035A133uL), new InputArgument[7]
								{
									slotsClass.Loc.X,
									slotsClass.Loc.Y,
									slotsClass.Loc.Z,
									slotsClass.Heading,
									0.005f,
									0.047f,
									0.906f
								});
								Vector3 position29 = Function.Call<Vector3>(((Hash)0x163E252DE035A133uL), new InputArgument[7]
								{
									slotsClass.Loc.X,
									slotsClass.Loc.Y,
									slotsClass.Loc.Z,
									slotsClass.Heading,
									0.125f,
									0.047f,
									0.906f
								});
								if (slotsClass.Machine.Equals("vw_prop_casino_slot_01a"))
								{
									ReelABase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position27.X, position27.Y, position27.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
									ReelABase.IsPositionFrozen = true;
									SlotProp1 = World.CreateProp(RequestModel("vw_prop_casino_slot_01a_reels"), position27, new Vector3(0f, 0f, slotsClass.Heading), false, false);
									SlotProp1.IsPositionFrozen = true;
									SlotProp1.AttachTo(ReelABase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
									ReelBBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position28.X, position28.Y, position28.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
									ReelABase.IsPositionFrozen = true;
									SlotProp2 = World.CreateProp(RequestModel("vw_prop_casino_slot_01a_reels"), position28, new Vector3(0f, 0f, slotsClass.Heading), false, false);
									SlotProp2.IsPositionFrozen = true;
									SlotProp2.AttachTo(ReelBBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
									ReelCBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position29.X, position29.Y, position29.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
									ReelABase.IsPositionFrozen = true;
									SlotProp3 = World.CreateProp(RequestModel("vw_prop_casino_slot_01a_reels"), position29, new Vector3(0f, 0f, slotsClass.Heading), false, false);
									SlotProp3.IsPositionFrozen = true;
									SlotProp3.AttachTo(ReelCBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
								}
								if (slotsClass.Machine.Equals("vw_prop_casino_slot_02a"))
								{
									ReelABase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position27.X, position27.Y, position27.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
									ReelABase.IsPositionFrozen = true;
									SlotProp1 = World.CreateProp(RequestModel("vw_prop_casino_slot_02a_reels"), position27, new Vector3(0f, 0f, slotsClass.Heading), false, false);
									SlotProp1.IsPositionFrozen = true;
									SlotProp1.AttachTo(ReelABase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
									ReelBBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position28.X, position28.Y, position28.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
									ReelABase.IsPositionFrozen = true;
									SlotProp2 = World.CreateProp(RequestModel("vw_prop_casino_slot_02a_reels"), position28, new Vector3(0f, 0f, slotsClass.Heading), false, false);
									SlotProp2.IsPositionFrozen = true;
									SlotProp2.AttachTo(ReelBBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
									ReelCBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position29.X, position29.Y, position29.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
									ReelABase.IsPositionFrozen = true;
									SlotProp3 = World.CreateProp(RequestModel("vw_prop_casino_slot_02a_reels"), position29, new Vector3(0f, 0f, slotsClass.Heading), false, false);
									SlotProp3.IsPositionFrozen = true;
									SlotProp3.AttachTo(ReelCBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
								}
								if (slotsClass.Machine.Equals("vw_prop_casino_slot_03a"))
								{
									ReelABase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position27.X, position27.Y, position27.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
									ReelABase.IsPositionFrozen = true;
									SlotProp1 = World.CreateProp(RequestModel("vw_prop_casino_slot_03a_reels"), position27, new Vector3(0f, 0f, slotsClass.Heading), false, false);
									SlotProp1.IsPositionFrozen = true;
									SlotProp1.AttachTo(ReelABase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
									ReelBBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position28.X, position28.Y, position28.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
									ReelABase.IsPositionFrozen = true;
									SlotProp2 = World.CreateProp(RequestModel("vw_prop_casino_slot_03a_reels"), position28, new Vector3(0f, 0f, slotsClass.Heading), false, false);
									SlotProp2.IsPositionFrozen = true;
									SlotProp2.AttachTo(ReelBBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
									ReelCBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position29.X, position29.Y, position29.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
									ReelABase.IsPositionFrozen = true;
									SlotProp3 = World.CreateProp(RequestModel("vw_prop_casino_slot_03a_reels"), position29, new Vector3(0f, 0f, slotsClass.Heading), false, false);
									SlotProp3.IsPositionFrozen = true;
									SlotProp3.AttachTo(ReelCBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
								}
								if (slotsClass.Machine.Equals("vw_prop_casino_slot_04a"))
								{
									ReelABase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position27.X, position27.Y, position27.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
									ReelABase.IsPositionFrozen = true;
									SlotProp1 = World.CreateProp(RequestModel("vw_prop_casino_slot_04a_reels"), position27, new Vector3(0f, 0f, slotsClass.Heading), false, false);
									SlotProp1.IsPositionFrozen = true;
									SlotProp1.AttachTo(ReelABase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
									ReelBBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position28.X, position28.Y, position28.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
									ReelABase.IsPositionFrozen = true;
									SlotProp2 = World.CreateProp(RequestModel("vw_prop_casino_slot_04a_reels"), position28, new Vector3(0f, 0f, slotsClass.Heading), false, false);
									SlotProp2.IsPositionFrozen = true;
									SlotProp2.AttachTo(ReelBBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
									ReelCBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position29.X, position29.Y, position29.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
									ReelABase.IsPositionFrozen = true;
									SlotProp3 = World.CreateProp(RequestModel("vw_prop_casino_slot_04a_reels"), position29, new Vector3(0f, 0f, slotsClass.Heading), false, false);
									SlotProp3.IsPositionFrozen = true;
									SlotProp3.AttachTo(ReelCBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
								}
								if (slotsClass.Machine.Equals("vw_prop_casino_slot_05a"))
								{
									ReelABase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position27.X, position27.Y, position27.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
									ReelABase.IsPositionFrozen = true;
									SlotProp1 = World.CreateProp(RequestModel("vw_prop_casino_slot_05a_reels"), position27, new Vector3(0f, 0f, slotsClass.Heading), false, false);
									SlotProp1.IsPositionFrozen = true;
									SlotProp1.AttachTo(ReelABase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
									ReelBBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position28.X, position28.Y, position28.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
									ReelABase.IsPositionFrozen = true;
									SlotProp2 = World.CreateProp(RequestModel("vw_prop_casino_slot_05a_reels"), position28, new Vector3(0f, 0f, slotsClass.Heading), false, false);
									SlotProp2.IsPositionFrozen = true;
									SlotProp2.AttachTo(ReelBBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
									ReelCBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position29.X, position29.Y, position29.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
									ReelABase.IsPositionFrozen = true;
									SlotProp3 = World.CreateProp(RequestModel("vw_prop_casino_slot_05a_reels"), position29, new Vector3(0f, 0f, slotsClass.Heading), false, false);
									SlotProp3.IsPositionFrozen = true;
									SlotProp3.AttachTo(ReelCBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
								}
								if (slotsClass.Machine.Equals("vw_prop_casino_slot_06a"))
								{
									ReelABase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position27.X, position27.Y, position27.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
									ReelABase.IsPositionFrozen = true;
									SlotProp1 = World.CreateProp(RequestModel("vw_prop_casino_slot_06a_reels"), position27, new Vector3(0f, 0f, slotsClass.Heading), false, false);
									SlotProp1.IsPositionFrozen = true;
									SlotProp1.AttachTo(ReelABase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
									ReelBBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position28.X, position28.Y, position28.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
									ReelABase.IsPositionFrozen = true;
									SlotProp2 = World.CreateProp(RequestModel("vw_prop_casino_slot_06a_reels"), position28, new Vector3(0f, 0f, slotsClass.Heading), false, false);
									SlotProp2.IsPositionFrozen = true;
									SlotProp2.AttachTo(ReelBBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
									ReelCBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position29.X, position29.Y, position29.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
									ReelABase.IsPositionFrozen = true;
									SlotProp3 = World.CreateProp(RequestModel("vw_prop_casino_slot_06a_reels"), position29, new Vector3(0f, 0f, slotsClass.Heading), false, false);
									SlotProp3.IsPositionFrozen = true;
									SlotProp3.AttachTo(ReelCBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
								}
								if (slotsClass.Machine.Equals("vw_prop_casino_slot_07a"))
								{
									ReelABase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position27.X, position27.Y, position27.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
									ReelABase.IsPositionFrozen = true;
									SlotProp1 = World.CreateProp(RequestModel("vw_prop_casino_slot_07a_reels"), position27, new Vector3(0f, 0f, slotsClass.Heading), false, false);
									SlotProp1.IsPositionFrozen = true;
									SlotProp1.AttachTo(ReelABase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
									ReelBBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position28.X, position28.Y, position28.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
									ReelABase.IsPositionFrozen = true;
									SlotProp2 = World.CreateProp(RequestModel("vw_prop_casino_slot_07a_reels"), position28, new Vector3(0f, 0f, slotsClass.Heading), false, false);
									SlotProp2.IsPositionFrozen = true;
									SlotProp2.AttachTo(ReelBBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
									ReelCBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position29.X, position29.Y, position29.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
									ReelABase.IsPositionFrozen = true;
									SlotProp3 = World.CreateProp(RequestModel("vw_prop_casino_slot_07a_reels"), position29, new Vector3(0f, 0f, slotsClass.Heading), false, false);
									SlotProp3.IsPositionFrozen = true;
									SlotProp3.AttachTo(ReelCBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
								}
								if (slotsClass.Machine.Equals("vw_prop_casino_slot_08a"))
								{
									ReelABase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position27.X, position27.Y, position27.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
									ReelABase.IsPositionFrozen = true;
									SlotProp1 = World.CreateProp(RequestModel("vw_prop_casino_slot_08a_reels"), position27, new Vector3(0f, 0f, slotsClass.Heading), false, false);
									SlotProp1.IsPositionFrozen = true;
									SlotProp1.AttachTo(ReelABase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
									ReelBBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position28.X, position28.Y, position28.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
									ReelABase.IsPositionFrozen = true;
									SlotProp2 = World.CreateProp(RequestModel("vw_prop_casino_slot_08a_reels"), position28, new Vector3(0f, 0f, slotsClass.Heading), false, false);
									SlotProp2.IsPositionFrozen = true;
									SlotProp2.AttachTo(ReelBBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
									ReelCBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position29.X, position29.Y, position29.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
									ReelABase.IsPositionFrozen = true;
									SlotProp3 = World.CreateProp(RequestModel("vw_prop_casino_slot_08a_reels"), position29, new Vector3(0f, 0f, slotsClass.Heading), false, false);
									SlotProp3.IsPositionFrozen = true;
									SlotProp3.AttachTo(ReelCBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
								}
								Script.Wait(500);
								CurrentSlotMachine = machineProp2;
								CurrentSlotClass = slotsClass;
								string dict27 = "anim_casino_a@amb@casino@games@slots@male";
								LoadDict(dict27);
								LoadDict(dict27);
								Vector3 position30 = CurrentSlotMachine.Position;
								SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
								{
									position30.X,
									position30.Y,
									position30.Z,
									0.0,
									0.0,
									machineProp2.Rotation.Z,
									2
								});
								Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
								{
									Game.Player.Character,
									SlotLocH[j].SlotSyncedScene,
									LoadDict(dict27),
									"enter_left",
									3000.0,
									-2.0,
									128,
									0,
									1148846080,
									0
								});
								Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
								{
									SlotLocH[j].SlotSyncedScene,
									Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
								});
								Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
								{
									SlotLocH[j].SlotSyncedScene,
									0.0
								});
								Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
								{
									Game.Player.Character,
									SlotLocH[j].SlotSyncedScene,
									"enter_left",
									LoadDict(dict27),
									3000.0,
									-2f,
									-2f,
									Game.Player.Character.Rotation.Z - 180f,
									1148846080,
									0
								});
								SlotMachineStage = 1;
								SlotInUse = true;
								BetOne = false;
								BetMax = false;
								BetSlot = 0f;
								SpinnedReels = false;
								ReelA_SpinAmt = 0f;
								ReelB_SpinAmt = 0f;
								ReelC_SpinAmt = 0f;
								FinishedReelA = false;
								FinishedReelB = false;
								FinishedReelC = false;
							}
						}
						if (SlotInUse)
						{
							if (SlotMachineStage == -1 && Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene }) >= 1065353216)
							{
								CurrentSlotMachine = null;
								SlotInUse = false;
								Game.Player.Character.Task.ClearAll();
								SlotMachineStage = 0;
							}
							if (SlotMachineStage == 1 && Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene }) >= 1065353216)
							{
								SlotMachineStage = 2;
							}
							if (SlotMachineStage == 2 && Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene }) >= 1065353216)
							{
								string text34 = "";
								Random random26 = new Random();
								int num30 = random26.Next(0, 500);
								if (num30 >= 0 && num30 < 100)
								{
									text34 = "base_idle_a";
								}
								if (num30 >= 100 && num30 < 200)
								{
									text34 = "base_idle_b";
								}
								if (num30 >= 200 && num30 < 300)
								{
									text34 = "base_idle_c";
								}
								if (num30 >= 300 && num30 < 400)
								{
									text34 = "base_idle_d";
								}
								if (num30 >= 400 && num30 < 500)
								{
									text34 = "base_idle_e";
								}
								if (num30 >= 500 && num30 < 600)
								{
									text34 = "base_idle_f";
								}
								SlotMachineStage = 2;
								string dict28 = "anim_casino_a@amb@casino@games@slots@male";
								LoadDict(dict28);
								LoadDict(dict28);
								Vector3 position31 = CurrentSlotMachine.Position;
								SlotLocH[j].SlotSyncedScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
								{
									position31.X,
									position31.Y,
									position31.Z,
									0.0,
									0.0,
									machineProp2.Rotation.Z,
									2
								});
								Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[10]
								{
									Game.Player.Character,
									SlotLocH[j].SlotSyncedScene,
									LoadDict(dict28),
									text34,
									3000.0,
									-2.0,
									128,
									0,
									1148846080,
									0
								});
								Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
								{
									SlotLocH[j].SlotSyncedScene,
									Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { SlotLocH[j].SlotSyncedScene })
								});
								Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
								{
									SlotLocH[j].SlotSyncedScene,
									0.0
								});
								Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[10]
								{
									Game.Player.Character,
									SlotLocH[j].SlotSyncedScene,
									text34,
									LoadDict(dict28),
									3000.0,
									-2f,
									-2f,
									Game.Player.Character.Rotation.Z - 180f,
									1148846080,
									0
								});
							}
						}
					}
					if (!(World.GetDistance(Game.Player.Character.Position, offsetInWorldCoords2) < 2f))
					{
						continue;
					}
					Vector3 position32 = Function.Call<Vector3>(((Hash)0x163E252DE035A133uL), new InputArgument[7]
					{
						slotsClass.Loc.X,
						slotsClass.Loc.Y,
						slotsClass.Loc.Z,
						slotsClass.Heading,
						-0.115f,
						0.047f,
						0.906f
					});
					Vector3 position33 = Function.Call<Vector3>(((Hash)0x163E252DE035A133uL), new InputArgument[7]
					{
						slotsClass.Loc.X,
						slotsClass.Loc.Y,
						slotsClass.Loc.Z,
						slotsClass.Heading,
						0.005f,
						0.047f,
						0.906f
					});
					Vector3 position34 = Function.Call<Vector3>(((Hash)0x163E252DE035A133uL), new InputArgument[7]
					{
						slotsClass.Loc.X,
						slotsClass.Loc.Y,
						slotsClass.Loc.Z,
						slotsClass.Heading,
						0.125f,
						0.047f,
						0.906f
					});
					if (Game.IsControlPressed(Control.VehicleDuck))
					{
						if (ReelABase != null)
						{
							ReelABase.Delete();
						}
						if (ReelBBase != null)
						{
							ReelBBase.Delete();
						}
						if (ReelCBase != null)
						{
							ReelCBase.Delete();
						}
						Prop[] nearbyProps = World.GetNearbyProps(slotsClass.Loc, 1.35f);
						CurrentSlotMachine = World.GetClosest(slotsClass.Loc, Array.Empty<Prop>());
						if (SlotProp1 != null)
						{
							SlotProp1.Delete();
						}
						if (SlotProp2 != null)
						{
							SlotProp2.Delete();
						}
						if (SlotProp3 != null)
						{
							SlotProp3.Delete();
						}
						SlotProp1 = null;
						SlotProp2 = null;
						SlotProp3 = null;
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_01a"))
						{
							ReelABase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position32.X, position32.Y, position32.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							ReelABase.IsPositionFrozen = true;
							SlotProp1 = World.CreateProp(RequestModel("vw_prop_casino_slot_01a_reels"), position32, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							SlotProp1.IsPositionFrozen = true;
							SlotProp1.AttachTo(ReelABase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							ReelBBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position33.X, position33.Y, position33.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							ReelABase.IsPositionFrozen = true;
							SlotProp2 = World.CreateProp(RequestModel("vw_prop_casino_slot_01a_reels"), position33, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							SlotProp2.IsPositionFrozen = true;
							SlotProp2.AttachTo(ReelBBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							ReelCBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position34.X, position34.Y, position34.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							ReelABase.IsPositionFrozen = true;
							SlotProp3 = World.CreateProp(RequestModel("vw_prop_casino_slot_01a_reels"), position34, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							SlotProp3.IsPositionFrozen = true;
							SlotProp3.AttachTo(ReelCBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_02a"))
						{
							ReelABase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position32.X, position32.Y, position32.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							ReelABase.IsPositionFrozen = true;
							SlotProp1 = World.CreateProp(RequestModel("vw_prop_casino_slot_02a_reels"), position32, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							SlotProp1.IsPositionFrozen = true;
							SlotProp1.AttachTo(ReelABase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							ReelBBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position33.X, position33.Y, position33.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							ReelABase.IsPositionFrozen = true;
							SlotProp2 = World.CreateProp(RequestModel("vw_prop_casino_slot_02a_reels"), position33, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							SlotProp2.IsPositionFrozen = true;
							SlotProp2.AttachTo(ReelBBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							ReelCBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position34.X, position34.Y, position34.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							ReelABase.IsPositionFrozen = true;
							SlotProp3 = World.CreateProp(RequestModel("vw_prop_casino_slot_02a_reels"), position34, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							SlotProp3.IsPositionFrozen = true;
							SlotProp3.AttachTo(ReelCBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_03a"))
						{
							ReelABase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position32.X, position32.Y, position32.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							ReelABase.IsPositionFrozen = true;
							SlotProp1 = World.CreateProp(RequestModel("vw_prop_casino_slot_03a_reels"), position32, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							SlotProp1.IsPositionFrozen = true;
							SlotProp1.AttachTo(ReelABase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							ReelBBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position33.X, position33.Y, position33.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							ReelABase.IsPositionFrozen = true;
							SlotProp2 = World.CreateProp(RequestModel("vw_prop_casino_slot_03a_reels"), position33, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							SlotProp2.IsPositionFrozen = true;
							SlotProp2.AttachTo(ReelBBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							ReelCBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position34.X, position34.Y, position34.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							ReelABase.IsPositionFrozen = true;
							SlotProp3 = World.CreateProp(RequestModel("vw_prop_casino_slot_03a_reels"), position34, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							SlotProp3.IsPositionFrozen = true;
							SlotProp3.AttachTo(ReelCBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_04a"))
						{
							ReelABase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position32.X, position32.Y, position32.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							ReelABase.IsPositionFrozen = true;
							SlotProp1 = World.CreateProp(RequestModel("vw_prop_casino_slot_04a_reels"), position32, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							SlotProp1.IsPositionFrozen = true;
							SlotProp1.AttachTo(ReelABase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							ReelBBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position33.X, position33.Y, position33.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							ReelABase.IsPositionFrozen = true;
							SlotProp2 = World.CreateProp(RequestModel("vw_prop_casino_slot_04a_reels"), position33, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							SlotProp2.IsPositionFrozen = true;
							SlotProp2.AttachTo(ReelBBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							ReelCBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position34.X, position34.Y, position34.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							ReelABase.IsPositionFrozen = true;
							SlotProp3 = World.CreateProp(RequestModel("vw_prop_casino_slot_04a_reels"), position34, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							SlotProp3.IsPositionFrozen = true;
							SlotProp3.AttachTo(ReelCBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_05a"))
						{
							ReelABase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position32.X, position32.Y, position32.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							ReelABase.IsPositionFrozen = true;
							SlotProp1 = World.CreateProp(RequestModel("vw_prop_casino_slot_05a_reels"), position32, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							SlotProp1.IsPositionFrozen = true;
							SlotProp1.AttachTo(ReelABase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							ReelBBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position33.X, position33.Y, position33.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							ReelABase.IsPositionFrozen = true;
							SlotProp2 = World.CreateProp(RequestModel("vw_prop_casino_slot_05a_reels"), position33, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							SlotProp2.IsPositionFrozen = true;
							SlotProp2.AttachTo(ReelBBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							ReelCBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position34.X, position34.Y, position34.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							ReelABase.IsPositionFrozen = true;
							SlotProp3 = World.CreateProp(RequestModel("vw_prop_casino_slot_05a_reels"), position34, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							SlotProp3.IsPositionFrozen = true;
							SlotProp3.AttachTo(ReelCBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_06a"))
						{
							ReelABase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position32.X, position32.Y, position32.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							ReelABase.IsPositionFrozen = true;
							SlotProp1 = World.CreateProp(RequestModel("vw_prop_casino_slot_06a_reels"), position32, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							SlotProp1.IsPositionFrozen = true;
							SlotProp1.AttachTo(ReelABase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							ReelBBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position33.X, position33.Y, position33.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							ReelABase.IsPositionFrozen = true;
							SlotProp2 = World.CreateProp(RequestModel("vw_prop_casino_slot_06a_reels"), position33, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							SlotProp2.IsPositionFrozen = true;
							SlotProp2.AttachTo(ReelBBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							ReelCBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position34.X, position34.Y, position34.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							ReelABase.IsPositionFrozen = true;
							SlotProp3 = World.CreateProp(RequestModel("vw_prop_casino_slot_06a_reels"), position34, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							SlotProp3.IsPositionFrozen = true;
							SlotProp3.AttachTo(ReelCBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_07a"))
						{
							ReelABase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position32.X, position32.Y, position32.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							ReelABase.IsPositionFrozen = true;
							SlotProp1 = World.CreateProp(RequestModel("vw_prop_casino_slot_07a_reels"), position32, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							SlotProp1.IsPositionFrozen = true;
							SlotProp1.AttachTo(ReelABase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							ReelBBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position33.X, position33.Y, position33.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							ReelABase.IsPositionFrozen = true;
							SlotProp2 = World.CreateProp(RequestModel("vw_prop_casino_slot_07a_reels"), position33, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							SlotProp2.IsPositionFrozen = true;
							SlotProp2.AttachTo(ReelBBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							ReelCBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position34.X, position34.Y, position34.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							ReelABase.IsPositionFrozen = true;
							SlotProp3 = World.CreateProp(RequestModel("vw_prop_casino_slot_07a_reels"), position34, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							SlotProp3.IsPositionFrozen = true;
							SlotProp3.AttachTo(ReelCBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_08a"))
						{
							ReelABase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position32.X, position32.Y, position32.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							ReelABase.IsPositionFrozen = true;
							SlotProp1 = World.CreateProp(RequestModel("vw_prop_casino_slot_08a_reels"), position32, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							SlotProp1.IsPositionFrozen = true;
							SlotProp1.AttachTo(ReelABase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							ReelBBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position33.X, position33.Y, position33.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							ReelABase.IsPositionFrozen = true;
							SlotProp2 = World.CreateProp(RequestModel("vw_prop_casino_slot_08a_reels"), position33, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							SlotProp2.IsPositionFrozen = true;
							SlotProp2.AttachTo(ReelBBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							ReelCBase = World.CreateProp(RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position34.X, position34.Y, position34.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							ReelABase.IsPositionFrozen = true;
							SlotProp3 = World.CreateProp(RequestModel("vw_prop_casino_slot_08a_reels"), position34, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							SlotProp3.IsPositionFrozen = true;
							SlotProp3.AttachTo(ReelCBase, 1, new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
						}
					}
				}
				if (!(ReelABase != null) || !(ReelBBase != null) || !(ReelCBase != null) || SlotProp1 != null)
				{
				}
				if (SlotProp2 != null)
				{
				}
				if (!(SlotProp3 != null))
				{
				}
			}
			foreach (Enterace denEnterance in DenEnterances)
			{
				if (denEnterance == null)
				{
					continue;
				}
				if (World.GetDistance(Game.Player.Character.Position, denEnterance.En) < 100f)
				{
					World.DrawMarker(MarkerType.VerticalCylinder, denEnterance.En, Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.4f), Color.Lime);
				}
				if (World.GetDistance(Game.Player.Character.Position, denEnterance.En) < 2f)
				{
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Enter Gambling Den");
					if (Game.IsControlJustPressed(Control.Context))
					{
						GTA.UI.Screen.FadeOut(500);
						Script.Wait(500);
						Game.Player.Character.Position = denEnterance.Ex;
						PokerTables.Clear();
						PokerTablesRot.Clear();
						RouletteTables.Clear();
						RouletteTablesRot.Clear();
						BlackJackTables.Clear();
						BlackJackRot.Clear();
						foreach (Prop item6 in Tables2)
						{
							if (item6 != null)
							{
								item6.Delete();
							}
						}
						foreach (Prop table in Tables)
						{
							if (table != null)
							{
								table.Delete();
							}
						}
						foreach (Prop chip in Chips)
						{
							if (chip != null)
							{
								chip.Delete();
							}
						}
						if (ReelABase != null)
						{
							ReelABase.Delete();
						}
						if (ReelBBase != null)
						{
							ReelBBase.Delete();
						}
						if (ReelCBase != null)
						{
							ReelCBase.Delete();
						}
						if (SlotProp1 != null)
						{
							SlotProp1.Delete();
						}
						if (SlotProp2 != null)
						{
							SlotProp2.Delete();
						}
						if (SlotProp3 != null)
						{
							SlotProp3.Delete();
						}
						if (ChipsSpawn != null)
						{
							ChipsSpawn.Delete();
						}
						if (RouletteMarker2 != null)
						{
							RouletteMarker2.Delete();
						}
						if (RouletteMarker1 != null)
						{
							RouletteMarker1.Delete();
						}
						if (RouletteMarker != null)
						{
							RouletteMarker.Delete();
						}
						foreach (Ped item7 in SMPed)
						{
							if (item7 != null)
							{
								item7.Delete();
							}
						}
						foreach (Prop item8 in Tables2)
						{
							if (item8 != null)
							{
								item8.Delete();
							}
						}
						foreach (Prop table2 in Tables)
						{
							if (table2 != null)
							{
								table2.Delete();
							}
						}
						foreach (Prop chip2 in Chips)
						{
							if (chip2 != null)
							{
								chip2.Delete();
							}
						}
						if (Ball != null)
						{
							Ball.Delete();
						}
						if (_8PlayerGameDealer != null)
						{
							_8PlayerGameDealer.Delete();
						}
						if (ChipsSpawn != null)
						{
							ChipsSpawn.Delete();
						}
						if (AI_1 != null)
						{
							AI_1.Delete();
						}
						if (AI_2 != null)
						{
							AI_2.Delete();
						}
						if (AI_3 != null)
						{
							AI_3.Delete();
						}
						if (AI_4 != null)
						{
							AI_4.Delete();
						}
						if (AI_5 != null)
						{
							AI_5.Delete();
						}
						if (AI_6 != null)
						{
							AI_6.Delete();
						}
						if (AI_7 != null)
						{
							AI_7.Delete();
						}
						foreach (Prop _8PlayerGamePokerPotChip in _8PlayerGamePokerPotChips)
						{
							if (_8PlayerGamePokerPotChip != null)
							{
								_8PlayerGamePokerPotChip.Delete();
							}
						}
						if (_8PlayerPokerGameTable1 != null)
						{
							_8PlayerPokerGameTable1.Delete();
						}
						if (_8PlayerPokerGameTable2 != null)
						{
							_8PlayerPokerGameTable2.Delete();
						}
						if (_8PlayerPokerGameTable3 != null)
						{
							_8PlayerPokerGameTable3.Delete();
						}
						foreach (Ped _8PlayerGamePokerPed in _8PlayerGamePokerPeds)
						{
							if (_8PlayerGamePokerPed != null)
							{
								_8PlayerGamePokerPed.Delete();
							}
						}
						foreach (Prop _8PlayerGamePokerProp in _8PlayerGamePokerProps)
						{
							if (_8PlayerGamePokerProp != null)
							{
								_8PlayerGamePokerProp.Delete();
							}
						}
						foreach (Ped item9 in SMPed)
						{
							if (item9 != null)
							{
								item9.Delete();
							}
						}
						foreach (Ped peds in PedsList)
						{
							if (peds != null)
							{
								peds.Delete();
							}
						}
						foreach (Prop item10 in Tables2)
						{
							if (item10 != null)
							{
								item10.Delete();
							}
						}
						foreach (Prop table3 in Tables)
						{
							if (table3 != null)
							{
								table3.Delete();
							}
						}
						foreach (Prop chip3 in Chips)
						{
							if (chip3 != null)
							{
								chip3.Delete();
							}
						}
						foreach (Ped peds2 in PedsList)
						{
							if (peds2 != null)
							{
								peds2.Delete();
							}
						}
						foreach (Prop prop89 in Props)
						{
							if (prop89 != null)
							{
								prop89.Delete();
							}
						}
						denEnterance.IsInterior = true;
						if (denEnterance.InteriorType == 1)
						{
							_8PlayerGamePoker_PlayerStartingBank = 100000f;
							_8PlayerGamePoker_AI1StartingBank = 100000f;
							_8PlayerGamePoker_AI2StartingBank = 100000f;
							_8PlayerGamePoker_AI3StartingBank = 100000f;
							_8PlayerGamePoker_AI4StartingBank = 100000f;
							_8PlayerGamePoker_AI5StartingBank = 100000f;
							_8PlayerGamePoker_AI6StartingBank = 100000f;
							_8PlayerGamePoker_AI7StartingBank = 100000f;
							if (_8PlayerGameDealer != null)
							{
								_8PlayerGameDealer.Delete();
							}
							if (_8PlayerPokerGameTable1 != null)
							{
								_8PlayerPokerGameTable1.Delete();
							}
							if (_8PlayerPokerGameTable2 != null)
							{
								_8PlayerPokerGameTable2.Delete();
							}
							if (_8PlayerPokerGameTable3 != null)
							{
								_8PlayerPokerGameTable3.Delete();
							}
							_8PlayerPokerGameTable1 = World.CreateProp(RequestModel("h4_prop_casino_3cardpoker_01b"), new Vector3(985.87f, -2998.933f, -48.6f), new Vector3(0f, 0f, -138f), false, false);
							_8PlayerPokerGameTable2 = World.CreateProp(RequestModel("h4_prop_casino_3cardpoker_01b"), _8PlayerPokerGameTable1.GetOffsetInWorldCoords(new Vector3(0f, 1.36f, 0f)), new Vector3(0f, 0f, _8PlayerPokerGameTable1.Rotation.Z - 180f), false, false);
							_8PlayerPokerGameTable3 = World.CreateProp(RequestModel("prop_arm_wrestle_01"), _8PlayerPokerGameTable1.GetOffsetInWorldCoords(new Vector3(0f, 0.7f, -0.08f)), new Vector3(0f, 0f, _8PlayerPokerGameTable1.Rotation.Z), false, false);
							_8PlayerGameDealer = World.CreatePed(RequestModel(PedHash.BurgerDrug), _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(2f, 0f, 0f)), _8PlayerPokerGameTable3.Heading - 180f);
							foreach (Ped peds3 in PedsList)
							{
								if (peds3 != null)
								{
									peds3.Delete();
								}
							}
							foreach (Prop prop90 in Props)
							{
								if (prop90 != null)
								{
									prop90.Delete();
								}
							}
							if (SlotLocH.Count > 0)
							{
								SlotLocH.Clear();
							}
							Prop prop = World.CreateProp(RequestModel("prop_temp_block_blocker"), new Vector3(971.2026f, -3000.465f, -48.6f), new Vector3(0f, 0f, 90f), false, false);
							Props.Add(prop);
							Prop item = World.CreateProp(RequestModel("prop_temp_block_blocker"), prop.GetOffsetInWorldCoords(new Vector3(7f, 0f, 0f)), new Vector3(0f, 0f, 90f), false, false);
							Props.Add(item);
							item = World.CreateProp(RequestModel("prop_temp_block_blocker"), prop.GetOffsetInWorldCoords(new Vector3(14f, 0f, 0f)), new Vector3(0f, 0f, 90f), false, false);
							Props.Add(item);
							item = World.CreateProp(RequestModel("prop_temp_block_blocker"), prop.GetOffsetInWorldCoords(new Vector3(14f, -9f, 0f)), new Vector3(0f, 0f, 90f), false, false);
							Props.Add(item);
							item = World.CreateProp(RequestModel("prop_temp_block_blocker"), prop.GetOffsetInWorldCoords(new Vector3(14f, -18.4f, 0f)), new Vector3(0f, 0f, 90f), false, false);
							Props.Add(item);
							item = World.CreateProp(RequestModel("prop_temp_block_blocker"), prop.GetOffsetInWorldCoords(new Vector3(14f, -27.9f, 0f)), new Vector3(0f, 0f, 90f), false, false);
							Props.Add(item);
							item = World.CreateProp(RequestModel("prop_temp_block_blocker"), prop.GetOffsetInWorldCoords(new Vector3(14.01f, -29.5f, 0f)), new Vector3(0f, 0f, 90f), false, false);
							Props.Add(item);
							item = World.CreateProp(RequestModel("prop_temp_block_blocker"), prop.GetOffsetInWorldCoords(new Vector3(-7f, 0f, 0f)), new Vector3(0f, 0f, 90f), false, false);
							Props.Add(item);
							item = World.CreateProp(RequestModel("prop_temp_block_blocker"), prop.GetOffsetInWorldCoords(new Vector3(-14f, 0f, 0f)), new Vector3(0f, 0f, 90f), false, false);
							Props.Add(item);
							item = World.CreateProp(RequestModel("ba_prop_battle_emis_rig_02"), prop.GetOffsetInWorldCoords(new Vector3(1f, -14.5f, 4f)), new Vector3(0f, 0f, 0f), false, false);
							Props.Add(item);
							Random random27 = new Random();
							string text35 = CasinoSlotsNames[random27.Next(0, CasinoSlotsNames.Count)];
							Prop prop2 = World.CreateProp(RequestModel(text35), new Vector3(976.59f, -2990.1f, -48.6f), new Vector3(0f, 0f, 0f), false, false);
							Props.Add(prop2);
							SlotLocH.Add(new SlotsClass(prop2.Position, prop2.Heading, text35));
							text35 = CasinoSlotsNames[random27.Next(0, CasinoSlotsNames.Count)];
							item = World.CreateProp(RequestModel(text35), prop2.GetOffsetInWorldCoords(new Vector3(0.85f, 0f, 0f)), new Vector3(0f, 0f, 0f), false, false);
							Props.Add(item);
							SlotLocH.Add(new SlotsClass(item.Position, item.Heading, text35));
							text35 = CasinoSlotsNames[random27.Next(0, CasinoSlotsNames.Count)];
							item = World.CreateProp(RequestModel(text35), prop2.GetOffsetInWorldCoords(new Vector3(1.7f, 0f, 0f)), new Vector3(0f, 0f, 0f), false, false);
							Props.Add(item);
							SlotLocH.Add(new SlotsClass(item.Position, item.Heading, text35));
							text35 = CasinoSlotsNames[random27.Next(0, CasinoSlotsNames.Count)];
							item = World.CreateProp(RequestModel(text35), prop2.GetOffsetInWorldCoords(new Vector3(2.55000019f, 0f, 0f)), new Vector3(0f, 0f, 0f), false, false);
							Props.Add(item);
							SlotLocH.Add(new SlotsClass(item.Position, item.Heading, text35));
							text35 = CasinoSlotsNames[random27.Next(0, CasinoSlotsNames.Count)];
							item = World.CreateProp(RequestModel(text35), prop2.GetOffsetInWorldCoords(new Vector3(3.4f, 0f, 0f)), new Vector3(0f, 0f, 0f), false, false);
							Props.Add(item);
							text35 = CasinoSlotsNames[random27.Next(0, CasinoSlotsNames.Count)];
							item = World.CreateProp(RequestModel(text35), prop2.GetOffsetInWorldCoords(new Vector3(7.65f, 0f, 0f)), new Vector3(0f, 0f, 0f), false, false);
							Props.Add(item);
							SlotLocH.Add(new SlotsClass(item.Position, item.Heading, text35));
							text35 = CasinoSlotsNames[random27.Next(0, CasinoSlotsNames.Count)];
							item = World.CreateProp(RequestModel(text35), prop2.GetOffsetInWorldCoords(new Vector3(8.5f, 0f, 0f)), new Vector3(0f, 0f, 0f), false, false);
							Props.Add(item);
							SlotLocH.Add(new SlotsClass(item.Position, item.Heading, text35));
							text35 = CasinoSlotsNames[random27.Next(0, CasinoSlotsNames.Count)];
							item = World.CreateProp(RequestModel(text35), prop2.GetOffsetInWorldCoords(new Vector3(9.35f, 0f, 0f)), new Vector3(0f, 0f, 0f), false, false);
							Props.Add(item);
							SlotLocH.Add(new SlotsClass(item.Position, item.Heading, text35));
							text35 = CasinoSlotsNames[random27.Next(0, CasinoSlotsNames.Count)];
							item = World.CreateProp(RequestModel(text35), prop2.GetOffsetInWorldCoords(new Vector3(10.2000008f, 0f, 0f)), new Vector3(0f, 0f, 0f), false, false);
							Props.Add(item);
							SlotLocH.Add(new SlotsClass(item.Position, item.Heading, text35));
							text35 = CasinoSlotsNames[random27.Next(0, CasinoSlotsNames.Count)];
							item = World.CreateProp(RequestModel(text35), prop2.GetOffsetInWorldCoords(new Vector3(11.05f, 0f, 0f)), new Vector3(0f, 0f, 0f), false, false);
							Props.Add(item);
							SlotLocH.Add(new SlotsClass(item.Position, item.Heading, text35));
							text35 = CasinoSlotsNames[random27.Next(0, CasinoSlotsNames.Count)];
							item = World.CreateProp(RequestModel(text35), prop2.GetOffsetInWorldCoords(new Vector3(15.4700012f, 0f, 0f)), new Vector3(0f, 0f, 0f), false, false);
							Props.Add(item);
							SlotLocH.Add(new SlotsClass(item.Position, item.Heading, text35));
							text35 = CasinoSlotsNames[random27.Next(0, CasinoSlotsNames.Count)];
							item = World.CreateProp(RequestModel(text35), prop2.GetOffsetInWorldCoords(new Vector3(16.3200016f, 0f, 0f)), new Vector3(0f, 0f, 0f), false, false);
							Props.Add(item);
							SlotLocH.Add(new SlotsClass(item.Position, item.Heading, text35));
							text35 = CasinoSlotsNames[random27.Next(0, CasinoSlotsNames.Count)];
							item = World.CreateProp(RequestModel(text35), prop2.GetOffsetInWorldCoords(new Vector3(17.170002f, 0f, 0f)), new Vector3(0f, 0f, 0f), false, false);
							Props.Add(item);
							SlotLocH.Add(new SlotsClass(item.Position, item.Heading, text35));
							text35 = CasinoSlotsNames[random27.Next(0, CasinoSlotsNames.Count)];
							item = World.CreateProp(RequestModel(text35), prop2.GetOffsetInWorldCoords(new Vector3(18.02f, 0f, 0f)), new Vector3(0f, 0f, 0f), false, false);
							Props.Add(item);
							SlotLocH.Add(new SlotsClass(item.Position, item.Heading, text35));
							text35 = CasinoSlotsNames[random27.Next(0, CasinoSlotsNames.Count)];
							item = World.CreateProp(RequestModel(text35), prop2.GetOffsetInWorldCoords(new Vector3(18.87f, 0f, 0f)), new Vector3(0f, 0f, 0f), false, false);
							Props.Add(item);
							SlotLocH.Add(new SlotsClass(item.Position, item.Heading, text35));
							text35 = CasinoSlotsNames[random27.Next(0, CasinoSlotsNames.Count)];
							item = World.CreateProp(RequestModel(text35), prop2.GetOffsetInWorldCoords(new Vector3(23.205f, 0f, 0f)), new Vector3(0f, 0f, 0f), false, false);
							Props.Add(item);
							SlotLocH.Add(new SlotsClass(item.Position, item.Heading, text35));
							text35 = CasinoSlotsNames[random27.Next(0, CasinoSlotsNames.Count)];
							item = World.CreateProp(RequestModel(text35), prop2.GetOffsetInWorldCoords(new Vector3(24.055f, 0f, 0f)), new Vector3(0f, 0f, 0f), false, false);
							Props.Add(item);
							SlotLocH.Add(new SlotsClass(item.Position, item.Heading, text35));
							text35 = CasinoSlotsNames[random27.Next(0, CasinoSlotsNames.Count)];
							item = World.CreateProp(RequestModel(text35), prop2.GetOffsetInWorldCoords(new Vector3(24.905f, 0f, 0f)), new Vector3(0f, 0f, 0f), false, false);
							Props.Add(item);
							SlotLocH.Add(new SlotsClass(item.Position, item.Heading, text35));
							text35 = CasinoSlotsNames[random27.Next(0, CasinoSlotsNames.Count)];
							item = World.CreateProp(RequestModel(text35), prop2.GetOffsetInWorldCoords(new Vector3(25.755f, 0f, 0f)), new Vector3(0f, 0f, 0f), false, false);
							Props.Add(item);
							SlotLocH.Add(new SlotsClass(item.Position, item.Heading, text35));
							text35 = CasinoSlotsNames[random27.Next(0, CasinoSlotsNames.Count)];
							item = World.CreateProp(RequestModel(text35), prop2.GetOffsetInWorldCoords(new Vector3(26.605f, 0f, 0f)), new Vector3(0f, 0f, 0f), false, false);
							Props.Add(item);
							SlotLocH.Add(new SlotsClass(item.Position, item.Heading, text35));
							foreach (SlotsClass item11 in SlotLocH)
							{
								int num31 = Function.Call<int>(((Hash)0xD24D37CC275948CCuL), new InputArgument[1] { item11.Machine });
								Prop prop3 = Function.Call<Prop>(((Hash)0xE143FA2249364369uL), new InputArgument[8]
								{
									item11.Loc.X,
									item11.Loc.Y,
									item11.Loc.Z,
									2f,
									num31,
									0,
									0,
									0
								});
								SlotMachineProps.Add(prop3);
								item11.MachineProp = prop3;
							}
							Script.Wait(1);
							for (int k = 0; k < SlotLocH.Count; k++)
							{
								Prop machineProp3 = SlotLocH[k].MachineProp;
								Ped ped = null;
								Vector3 offsetInWorldCoords4 = machineProp3.GetOffsetInWorldCoords(new Vector3(0f, -0.9f, -0.3f));
								Random random28 = new Random();
								int num32 = random28.Next(0, 200);
								int num33 = random28.Next(1, 6);
								if (num32 <= 110)
								{
									ClubDancerList.Add("a_m_y_clubcust_01");
									ClubDancerList.Add("a_m_y_clubcust_01");
									ClubDancerList.Add("a_m_y_clubcust_01");
									ClubDancerList.Add("a_f_y_clubcust_01");
									ClubDancerList.Add("a_f_y_clubcust_01");
									ClubDancerList.Add("a_f_y_clubcust_01");
									ped = World.CreatePed(ClubDancerList[random27.Next(0, ClubDancerList.Count)], offsetInWorldCoords4);
									ped.Heading = machineProp3.Heading;
									Script.Wait(15);
									ped.Position = offsetInWorldCoords4;
									SetRandomOutfit(ped);
									ped.IsPositionFrozen = true;
									ped.AlwaysKeepTask = true;
									ped.IsPersistent = true;
									SMPed.Add(ped);
									if (num33 == 1)
									{
										ped.PlayAnimationOld("anim_casino_a@amb@casino@games@slots@male", "base_idle_a", 16f, -1, true, -1f);
									}
									if (num33 == 2)
									{
										ped.PlayAnimationOld("anim_casino_a@amb@casino@games@slots@male", "base_idle_b", 16f, -1, true, -1f);
									}
									if (num33 == 3)
									{
										ped.PlayAnimationOld("anim_casino_a@amb@casino@games@slots@male", "base_idle_c", 16f, -1, true, -1f);
									}
									if (num33 == 4)
									{
										ped.PlayAnimationOld("anim_casino_a@amb@casino@games@slots@male", "base_idle_d", 16f, -1, true, -1f);
									}
									if (num33 == 5)
									{
										ped.PlayAnimationOld("anim_casino_a@amb@casino@games@slots@male", "base_idle_e", 16f, -1, true, -1f);
									}
									if (num33 == 6)
									{
										ped.PlayAnimationOld("anim_casino_a@amb@casino@games@slots@male", "base_idle_f", 16f, -1, true, -1f);
									}
									if (num33 > 6)
									{
										ped.PlayAnimationOld("anim_casino_a@amb@casino@games@slots@male", "base_idle_b", 16f, -1, true, -1f);
									}
									if (num33 < 0)
									{
										ped.PlayAnimationOld("anim_casino_a@amb@casino@games@slots@male", "base_idle_e", 16f, -1, true, -1f);
									}
									Function.Call(((Hash)0x70A2D1137C8ED7C9uL), new InputArgument[3] { ped, 0, 0 });
									Function.Call(((Hash)0x9F7794730795E019uL), new InputArgument[3] { ped, 17, 1 });
									Function.Call(((Hash)0x9F8AA94D6D97DBF4uL), new InputArgument[2] { ped, 1 });
									Function.Call(((Hash)0x70A2D1137C8ED7C9uL), new InputArgument[3] { ped, 0, 0 });
									Function.Call(((Hash)0x9F7794730795E019uL), new InputArgument[3] { ped, 17, 1 });
									ped.RelationshipGroup = 1;
									ped.AlwaysKeepTask = true;
									SMPed.Add(ped);
									SlotLocH[k].In_use = true;
									SlotLocH[k].SlotPed = ped;
								}
								else
								{
									SlotLocH[k].In_use = false;
								}
								Script.Wait(1);
							}
							Prop item2 = World.CreateProp(RequestModel("h4_prop_casino_blckjack_01c"), new Vector3(1001.776f, -2994.83f, -48.6f), new Vector3(0f, 0f, 0f), false, false);
							Props.Add(item2);
							Prop item3 = World.CreateProp(RequestModel("h4_prop_casino_3cardpoker_01e"), new Vector3(993.92f, -2994.85f, -48.6f), new Vector3(0f, 0f, 0f), false, false);
							Props.Add(item3);
							Prop item4 = World.CreateProp(RequestModel("h4_prop_casino_blckjack_01c"), new Vector3(986.0224f, -2994.76f, -48.6f), new Vector3(0f, 0f, 0f), false, false);
							Props.Add(item4);
							Prop item5 = World.CreateProp(RequestModel("h4_prop_casino_3cardpoker_01e"), new Vector3(978.1927f, -2994.826f, -48.6f), new Vector3(0f, 0f, 0f), false, false);
							Props.Add(item5);
							RouletteTableA = World.CreateProp(RequestModel("vw_prop_casino_roulette_01"), new Vector3(978.7054f, -3006.504f, -48.6f), new Vector3(0f, 0f, 90f), false, false);
							Props.Add(RouletteTableA);
							PokerTables.Add(new Vector3(994.3596f, -2995.392f, -48.4f));
							PokerTablesRot.Add(16.36f);
							PokerTables.Add(new Vector3(978.6652f, -2995.471f, -48.4f));
							PokerTablesRot.Add(19.8f);
							RouletteTables.Add(new Vector3(979.6263f, -3005.737f, -48.4f));
							RouletteTablesRot.Add(96.86f);
							BlackJackTables.Add(new Vector3(1002.217f, -2995.336f, -48.4f));
							BlackJackRot.Add(19.8f);
							BlackJackTables.Add(new Vector3(986.4229f, -2995.265f, -48.4f));
							BlackJackRot.Add(19.8f);
							Firsttime = true;
						}
						if (denEnterance.InteriorType == 2)
						{
						}
						Script.Wait(500);
						GTA.UI.Screen.FadeIn(500);
					}
				}
				if (!denEnterance.IsInterior)
				{
					continue;
				}
				if (World.GetDistance(Game.Player.Character.Position, denEnterance.Ex) < 100f)
				{
					World.DrawMarker(MarkerType.VerticalCylinder, denEnterance.Ex, Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.4f), Color.Lime);
				}
				if (!(World.GetDistance(Game.Player.Character.Position, denEnterance.Ex) < 2f))
				{
					continue;
				}
				DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Exit Gambling Den");
				if (!Game.IsControlJustPressed(Control.Context))
				{
					continue;
				}
				GTA.UI.Screen.FadeOut(500);
				Script.Wait(500);
				Game.Player.Character.Position = denEnterance.En;
				Firsttime = false;
				foreach (Prop item12 in Tables2)
				{
					if (item12 != null)
					{
						item12.Delete();
					}
				}
				foreach (Prop table4 in Tables)
				{
					if (table4 != null)
					{
						table4.Delete();
					}
				}
				foreach (Prop chip4 in Chips)
				{
					if (chip4 != null)
					{
						chip4.Delete();
					}
				}
				if (ReelABase != null)
				{
					ReelABase.Delete();
				}
				if (ReelBBase != null)
				{
					ReelBBase.Delete();
				}
				if (ReelCBase != null)
				{
					ReelCBase.Delete();
				}
				if (SlotProp1 != null)
				{
					SlotProp1.Delete();
				}
				if (SlotProp2 != null)
				{
					SlotProp2.Delete();
				}
				if (SlotProp3 != null)
				{
					SlotProp3.Delete();
				}
				if (ChipsSpawn != null)
				{
					ChipsSpawn.Delete();
				}
				if (RouletteMarker2 != null)
				{
					RouletteMarker2.Delete();
				}
				if (RouletteMarker1 != null)
				{
					RouletteMarker1.Delete();
				}
				if (RouletteMarker != null)
				{
					RouletteMarker.Delete();
				}
				foreach (Ped item13 in SMPed)
				{
					if (item13 != null)
					{
						item13.Delete();
					}
				}
				foreach (Prop item14 in Tables2)
				{
					if (item14 != null)
					{
						item14.Delete();
					}
				}
				foreach (Prop table5 in Tables)
				{
					if (table5 != null)
					{
						table5.Delete();
					}
				}
				foreach (Prop chip5 in Chips)
				{
					if (chip5 != null)
					{
						chip5.Delete();
					}
				}
				if (Ball != null)
				{
					Ball.Delete();
				}
				if (_8PlayerGameDealer != null)
				{
					_8PlayerGameDealer.Delete();
				}
				if (ChipsSpawn != null)
				{
					ChipsSpawn.Delete();
				}
				if (AI_1 != null)
				{
					AI_1.Delete();
				}
				if (AI_2 != null)
				{
					AI_2.Delete();
				}
				if (AI_3 != null)
				{
					AI_3.Delete();
				}
				if (AI_4 != null)
				{
					AI_4.Delete();
				}
				if (AI_5 != null)
				{
					AI_5.Delete();
				}
				if (AI_6 != null)
				{
					AI_6.Delete();
				}
				if (AI_7 != null)
				{
					AI_7.Delete();
				}
				foreach (Prop _8PlayerGamePokerPotChip2 in _8PlayerGamePokerPotChips)
				{
					if (_8PlayerGamePokerPotChip2 != null)
					{
						_8PlayerGamePokerPotChip2.Delete();
					}
				}
				if (_8PlayerPokerGameTable1 != null)
				{
					_8PlayerPokerGameTable1.Delete();
				}
				if (_8PlayerPokerGameTable2 != null)
				{
					_8PlayerPokerGameTable2.Delete();
				}
				if (_8PlayerPokerGameTable3 != null)
				{
					_8PlayerPokerGameTable3.Delete();
				}
				foreach (Ped _8PlayerGamePokerPed2 in _8PlayerGamePokerPeds)
				{
					if (_8PlayerGamePokerPed2 != null)
					{
						_8PlayerGamePokerPed2.Delete();
					}
				}
				foreach (Prop _8PlayerGamePokerProp2 in _8PlayerGamePokerProps)
				{
					if (_8PlayerGamePokerProp2 != null)
					{
						_8PlayerGamePokerProp2.Delete();
					}
				}
				foreach (Ped item15 in SMPed)
				{
					if (item15 != null)
					{
						item15.Delete();
					}
				}
				foreach (Ped peds4 in PedsList)
				{
					if (peds4 != null)
					{
						peds4.Delete();
					}
				}
				foreach (Prop item16 in Tables2)
				{
					if (item16 != null)
					{
						item16.Delete();
					}
				}
				foreach (Prop table6 in Tables)
				{
					if (table6 != null)
					{
						table6.Delete();
					}
				}
				foreach (Prop chip6 in Chips)
				{
					if (chip6 != null)
					{
						chip6.Delete();
					}
				}
				foreach (Ped peds5 in PedsList)
				{
					if (peds5 != null)
					{
						peds5.Delete();
					}
				}
				foreach (Prop prop91 in Props)
				{
					if (prop91 != null)
					{
						prop91.Delete();
					}
				}
				Script.Wait(500);
				GTA.UI.Screen.FadeIn(500);
				denEnterance.IsInterior = false;
			}
			if (_8PlayerPokerGameTable3 != null)
			{
				Vector3 offsetInWorldCoords5 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-1.05f, -0.65f, 0.25f));
				Vector3 offsetInWorldCoords6 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-0.4f, -1.2f, 0.25f));
				Vector3 offsetInWorldCoords7 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.4f, -1.2f, 0.25f));
				Vector3 offsetInWorldCoords8 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(1.05f, -0.7f, 0.25f));
				Vector3 offsetInWorldCoords9 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(1.05f, 0.65f, 0.25f));
				Vector3 offsetInWorldCoords10 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.4f, 1.2f, 0.25f));
				Vector3 offsetInWorldCoords11 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-0.4f, 1.2f, 0.25f));
				Vector3 offsetInWorldCoords12 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-1.05f, 0.65f, 0.25f));
			}
			if (_8PlayerPokerGameTable1 != null)
			{
				Vector3 offsetInWorldCoords13 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-1.05f, -0.65f, 0.25f));
				if (World.GetDistance(Game.Player.Character.Position, offsetInWorldCoords13) < 2.25f)
				{
					if (Playing8PlayerPoker)
					{
						ShowPlayerBanks();
						UI.ShowSubtitle("~o~ Pot : $" + _8PlayerGamePokerPot.ToString("N"));
						if (_8PlayerGameStage == 1)
						{
							Show_Q("Quit");
						}
						if (_8PlayerGameStage == 2)
						{
							Show_EQ("Continue", "Quit");
						}
						if (_8PlayerGameStage == 3)
						{
							Show_EGQRX("Look at Cards", "Place Bet", "Quit", "Fold", "Check");
						}
						if (Player_Bust)
						{
							Show_Q("Quit");
						}
					}
					if (!Playing8PlayerPoker)
					{
						Show_Q("   Current Cash :   " + Game.Player.Money.ToString("N") + "   ");
						DisplayHelpTextThisFrameNoSound("Press ~INPUT_COVER~ to Play 8 Player Poker");
					}
				}
			}
			if (_8PlayerPokerGameTable5 != null)
			{
				Vector3 offsetInWorldCoords14 = _8PlayerPokerGameTable5.GetOffsetInWorldCoords(new Vector3(-1.05f, -0.65f, 0.25f));
				if (World.GetDistance(Game.Player.Character.Position, offsetInWorldCoords14) < 2.25f)
				{
					if (Playing8PlayerPoker)
					{
						ShowPlayerBanks();
						UI.ShowSubtitle("~o~ Pot : $" + _8PlayerGamePokerPot.ToString("N"));
						if (_8PlayerGameStage == 1)
						{
							Show_Q("Quit");
						}
						if (_8PlayerGameStage == 2)
						{
							Show_EQ("Continue", "Quit");
						}
						if (_8PlayerGameStage == 3)
						{
							Show_EGQRX("Look at Cards", "Place Bet", "Quit", "Fold", "Check");
						}
						if (Player_Bust)
						{
							Show_Q("Quit");
						}
					}
					if (!Playing8PlayerPoker)
					{
						Show_Q("   Current Cash :   " + Game.Player.Money.ToString("N") + "   ");
						DisplayHelpTextThisFrameNoSound("Press ~INPUT_COVER~ to Play 8 Player Poker");
					}
				}
			}
			if (_8PlayerPokerGameTable6 != null)
			{
				Vector3 offsetInWorldCoords15 = _8PlayerPokerGameTable6.GetOffsetInWorldCoords(new Vector3(-1.05f, -0.65f, 0.25f));
				if (World.GetDistance(Game.Player.Character.Position, offsetInWorldCoords15) < 2.25f)
				{
					if (Playing8PlayerPoker)
					{
						ShowPlayerBanks();
						UI.ShowSubtitle("~o~ Pot : $" + _8PlayerGamePokerPot.ToString("N"));
						if (_8PlayerGameStage == 1)
						{
							Show_Q("Quit");
						}
						if (_8PlayerGameStage == 2)
						{
							Show_EQ("Continue", "Quit");
						}
						if (_8PlayerGameStage == 3)
						{
							Show_EGQRX("Look at Cards", "Place Bet", "Quit", "Fold", "Check");
						}
						if (Player_Bust)
						{
							Show_Q("Quit");
						}
					}
					if (!Playing8PlayerPoker)
					{
						Show_Q("   Current Cash :   " + Game.Player.Money.ToString("N") + "   ");
						DisplayHelpTextThisFrameNoSound("Press ~INPUT_COVER~ to Play 8 Player Poker");
					}
				}
			}
			if (Playing8PlayerPoker)
			{
				if (_8PlayerGameStage > 1)
				{
					if (_8PlayerGamePoker_PlayerBank <= 0f)
					{
						Player_Bust = true;
					}
					if (_8PlayerGamePoker_AI1Bank <= 0f)
					{
						AI1_Bust = true;
					}
					if (_8PlayerGamePoker_AI2Bank <= 0f)
					{
						AI2_Bust = true;
					}
					if (_8PlayerGamePoker_AI3Bank <= 0f)
					{
						AI3_Bust = true;
					}
					if (_8PlayerGamePoker_AI4Bank <= 0f)
					{
						AI4_Bust = true;
					}
					if (_8PlayerGamePoker_AI5Bank <= 0f)
					{
						AI5_Bust = true;
					}
					if (_8PlayerGamePoker_AI6Bank <= 0f)
					{
						AI6_Bust = true;
					}
					if (_8PlayerGamePoker_AI7Bank <= 0f)
					{
						AI7_Bust = true;
					}
				}
				if (_8PlayerGameStage == 3 && !Player_Bust && !Player_SkipTurn)
				{
					if (Game.IsControlJustPressed(Control.VehicleDuck))
					{
						List<Ped> list = new List<Ped>();
						CommunityCard1.Heading = _8PlayerPokerGameTable3.Heading + 90f;
						CommunityCard2.Heading = _8PlayerPokerGameTable3.Heading + 90f;
						CommunityCard3.Heading = _8PlayerPokerGameTable3.Heading + 90f;
						list.Add(AI_1);
						list.Add(AI_2);
						list.Add(AI_3);
						list.Add(AI_4);
						list.Add(AI_5);
						list.Add(AI_6);
						list.Add(AI_7);
						list.Add(Game.Player.Character);
						int index = 0;
						foreach (Ped item17 in list)
						{
							string dict29 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01a@play@v02";
							string anim = "bet_ante";
							LoadDict(dict29);
							if (item17 != Game.Player.Character)
							{
								Scene2 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
								{
									item17.Position.X,
									item17.Position.Y,
									item17.Position.Z,
									0.0,
									0.0,
									item17.Rotation.Z,
									2
								});
								Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
								{
									Scene2,
									Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { Scene2 })
								});
								PlaySyncAnim(item17, item17, Scene2, dict29, anim);
								Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
								{
									PlayerCards[index].Card1,
									Scene2,
									"bet_ante_card_a",
									LoadDict(dict29),
									16f,
									-16f,
									0,
									1148846080
								});
								Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
								{
									PlayerCards[index].Card1,
									Scene2,
									"bet_ante_card_b",
									LoadDict(dict29),
									16f,
									-16f,
									0,
									1148846080
								});
								Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
								{
									PlayerCards[index].Card1,
									Scene2,
									"bet_ante_card_c",
									LoadDict(dict29),
									16f,
									-16f,
									0,
									1148846080
								});
							}
							if (item17 == Game.Player.Character)
							{
								Scene2 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
								{
									item17.Position.X,
									item17.Position.Y,
									item17.Position.Z,
									0.0,
									0.0,
									item17.Rotation.Z,
									2
								});
								Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
								{
									Scene2,
									Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { Scene2 })
								});
								PlaySyncAnim(item17, item17, Scene2, dict29, anim);
								Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
								{
									PlayerCard1,
									Scene2,
									"bet_ante_card_a",
									LoadDict(dict29),
									16f,
									-16f,
									0,
									1148846080
								});
								Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
								{
									PlayerCard2,
									Scene2,
									"bet_ante_card_b",
									LoadDict(dict29),
									16f,
									-16f,
									0,
									1148846080
								});
								Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
								{
									PlayerCard3,
									Scene2,
									"bet_ante_card_c",
									LoadDict(dict29),
									16f,
									-16f,
									0,
									1148846080
								});
							}
						}
						Script.Wait(2000);
						CheckPokerWin();
					}
					if (Game.IsControlJustPressed(Control.Reload))
					{
						Ped character = Game.Player.Character;
						string dict30 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@play@v01";
						string anim2 = "check_cards_fold";
						LoadDict(dict30);
						Scene2 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
						{
							character.Position.X,
							character.Position.Y,
							character.Position.Z,
							0.0,
							0.0,
							character.Rotation.Z,
							2
						});
						Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
						{
							Scene2,
							Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { Scene2 })
						});
						PlaySyncAnim(character, character, Scene2, dict30, anim2);
						Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
						{
							PlayerCard1,
							Scene2,
							"check_cards_fold_card_a",
							LoadDict(dict30),
							16f,
							-16f,
							0,
							1148846080
						});
						Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
						{
							PlayerCard2,
							Scene2,
							"check_cards_fold_card_b",
							LoadDict(dict30),
							16f,
							-16f,
							0,
							1148846080
						});
						Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
						{
							PlayerCard3,
							Scene2,
							"check_cards_fold_card_c",
							LoadDict(dict30),
							16f,
							-16f,
							0,
							1148846080
						});
						Script.Wait(8000);
						CommunityCard1.Heading = _8PlayerPokerGameTable3.Heading + 90f;
						CommunityCard2.Heading = _8PlayerPokerGameTable3.Heading + 90f;
						CommunityCard3.Heading = _8PlayerPokerGameTable3.Heading + 90f;
						Script.Wait(2000);
						CheckPokerWin();
						_8PlayerGameStage = 1;
						UI.Notify("~g~ HKH191~w~ : Player Has folded!, Starting new Match");
					}
					if (Game.IsControlJustPressed(Control.Context))
					{
						Ped character2 = Game.Player.Character;
						string dict31 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v01";
						string anim3 = "check_cards_fold";
						LoadDict(dict31);
						Scene2 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
						{
							character2.Position.X,
							character2.Position.Y,
							character2.Position.Z,
							0.0,
							0.0,
							character2.Rotation.Z,
							2
						});
						Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
						{
							Scene2,
							Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { Scene2 })
						});
						PlaySyncAnim(character2, character2, Scene2, dict31, anim3);
						Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
						{
							PlayerCard1,
							Scene2,
							"check_cards_fold_card_a",
							LoadDict(dict31),
							16f,
							-16f,
							0,
							1148846080
						});
						Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
						{
							PlayerCard2,
							Scene2,
							"check_cards_fold_card_b",
							LoadDict(dict31),
							16f,
							-16f,
							0,
							1148846080
						});
						Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
						{
							PlayerCard3,
							Scene2,
							"check_cards_fold_card_c",
							LoadDict(dict31),
							16f,
							-16f,
							0,
							1148846080
						});
					}
				}
				if (_8PlayerGameStage == 3 && !IsinBettingRound && Game.IsControlJustPressed(Control.ThrowGrenade))
				{
					try
					{
						string text36 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
						string text37 = "bet_plus";
						int syncScene = 0;
						string userInput = Game.GetUserInput(WindowTitle.EnterEyefindMessage, LastRaiseAmmount.ToString(), 6);
						IsinBettingRound = true;
						Random random29 = new Random();
						float num34 = float.Parse(userInput);
						if (LastRaiseAmmount <= num34)
						{
							text36 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
							text37 = "bet_plus";
							syncScene = 0;
							LastRaiseAmmount = num34;
							if (_8PlayerGamePoker_PlayerBank - num34 > 0f)
							{
								text36 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
								text37 = "bet_plus";
								_8PlayerGamePoker_PlayerBank -= num34;
								_8PlayerGamePokerPot += LastRaiseAmmount;
								PlaySyncAnim(Game.Player.Character, Game.Player.Character, syncScene, text36, text37);
							}
							else
							{
								Ped character3 = Game.Player.Character;
								text36 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@play@v01";
								text37 = "check_cards_fold";
								LoadDict(text36);
								Scene2 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
								{
									character3.Position.X,
									character3.Position.Y,
									character3.Position.Z,
									0.0,
									0.0,
									character3.Rotation.Z,
									2
								});
								Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
								{
									Scene2,
									Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { Scene2 })
								});
								PlaySyncAnim(character3, character3, Scene2, text36, text37);
								Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
								{
									PlayerCard1,
									Scene2,
									"check_cards_fold_card_a",
									LoadDict(text36),
									16f,
									-16f,
									0,
									1148846080
								});
								Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
								{
									PlayerCard2,
									Scene2,
									"check_cards_fold_card_b",
									LoadDict(text36),
									16f,
									-16f,
									0,
									1148846080
								});
								Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
								{
									PlayerCard3,
									Scene2,
									"check_cards_fold_card_c",
									LoadDict(text36),
									16f,
									-16f,
									0,
									1148846080
								});
								UI.Notify("You Folded!, Due to not Being able to Call/Raise Bet");
							}
						}
						else
						{
							UI.Notify("~g~ HKH191~w~ Cannot Enter a bet lower than the Last Bet!, Skippinng Players Turn");
						}
						List<Ped> list2 = new List<Ped>();
						Script.Wait(700);
						RunBet(num34);
						list2.Add(AI_1);
						list2.Add(AI_2);
						list2.Add(AI_3);
						list2.Add(AI_4);
						list2.Add(AI_5);
						list2.Add(AI_6);
						list2.Add(AI_7);
						int index2 = 0;
						foreach (Ped item18 in list2)
						{
							LastRaiseAmmount = num34;
							if (item18 == AI_1 && !AI1_Bust && !AI1_SkipTurn)
							{
								bool foldCauseHighBet = GetFoldCauseHighBet(num34, _8PlayerGamePoker_AI1Bank);
								bool foldCauseLowScore = GetFoldCauseLowScore(AI1Total);
								if (_8PlayerGamePoker_AI1Bank >= num34 && !foldCauseHighBet && !foldCauseLowScore)
								{
									text36 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
									text37 = "bet_plus";
									PlaySyncAnim(item18, item18, syncScene, text36, text37);
									Script.Wait(700);
									int num35 = random29.Next(0, 1000);
									int num36 = random29.Next(0, 600);
									if (num36 > num35)
									{
										float num37 = random29.Next(100, (int)MaxRaiseAmt);
										if (_8PlayerGamePoker_AI1Bank - num34 > 0f)
										{
											num34 += num37;
											UI.Notify("AI 1 ~y~Raised~w~ the Bet");
										}
										else
										{
											UI.Notify("AI 1 ~y~Called~w~ the Bet");
										}
									}
									else
									{
										UI.Notify("AI 1 ~y~Called~w~ the Bet");
									}
									_8PlayerGamePokerPot += num34;
									RunBet(num34);
									_8PlayerGamePoker_AI1Bank -= num34;
								}
								else
								{
									AI1_SkipTurn = true;
									text36 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@play@v01";
									text37 = "check_cards_fold";
									LoadDict(text36);
									Scene2 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
									{
										item18.Position.X,
										item18.Position.Y,
										item18.Position.Z,
										0.0,
										0.0,
										item18.Rotation.Z,
										2
									});
									Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
									{
										Scene2,
										Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { Scene2 })
									});
									PlaySyncAnim(item18, item18, Scene2, text36, text37);
									try
									{
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											PlayerCards[index2].Card1,
											Scene2,
											"check_cards_fold_card_a",
											LoadDict(text36),
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											PlayerCards[index2].Card2,
											Scene2,
											"check_cards_fold_card_b",
											LoadDict(text36),
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											PlayerCards[index2].Card3,
											Scene2,
											"check_cards_fold_card_c",
											LoadDict(text36),
											16f,
											-16f,
											0,
											1148846080
										});
									}
									catch
									{
									}
									Random random30 = new Random();
									int num38 = random30.Next(0, 8);
									if (num38 == 0)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_INSULT_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num38 == 1)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_FUCK_YOU", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num38 == 2)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_INSULT_HIGH", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num38 == 3)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_SHOCKED_HIGH", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num38 == 4)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_SHOCKED_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num38 == 5)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_FRIGHTENED_HIGH", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num38 == 6)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_FRIGHTENED_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num38 == 7)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num38 == 8)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									UI.Notify("AI 1 has ~y~Folded!");
								}
							}
							Script.Wait(1);
							if (item18 == AI_2 && !AI2_Bust && !AI2_SkipTurn)
							{
								bool foldCauseHighBet2 = GetFoldCauseHighBet(num34, _8PlayerGamePoker_AI2Bank);
								bool foldCauseLowScore2 = GetFoldCauseLowScore(AI2Total);
								if (_8PlayerGamePoker_AI2Bank >= num34 && !foldCauseHighBet2 && !foldCauseLowScore2)
								{
									text36 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
									text37 = "bet_plus";
									PlaySyncAnim(item18, item18, syncScene, text36, text37);
									Script.Wait(700);
									int num39 = random29.Next(0, 1000);
									int num40 = random29.Next(0, 600);
									if (num40 > num39)
									{
										float num41 = random29.Next(100, (int)MaxRaiseAmt);
										if (_8PlayerGamePoker_AI2Bank - num34 > 0f)
										{
											num34 += num41;
											UI.Notify("AI 2 ~y~Raised~w~ the Bet");
										}
										else
										{
											UI.Notify("AI 2 ~y~Called~w~ the Bet");
										}
									}
									else
									{
										UI.Notify("AI 2 ~y~Called~w~ the Bet");
									}
									_8PlayerGamePokerPot += num34;
									RunBet(num34);
									_8PlayerGamePoker_AI2Bank -= num34;
								}
								else
								{
									AI2_SkipTurn = true;
									text36 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@play@v01";
									text37 = "check_cards_fold";
									LoadDict(text36);
									Scene2 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
									{
										item18.Position.X,
										item18.Position.Y,
										item18.Position.Z,
										0.0,
										0.0,
										item18.Rotation.Z,
										2
									});
									Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
									{
										Scene2,
										Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { Scene2 })
									});
									PlaySyncAnim(item18, item18, Scene2, text36, text37);
									try
									{
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											PlayerCards[index2].Card1,
											Scene2,
											"check_cards_fold_card_a",
											LoadDict(text36),
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											PlayerCards[index2].Card2,
											Scene2,
											"check_cards_fold_card_b",
											LoadDict(text36),
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											PlayerCards[index2].Card3,
											Scene2,
											"check_cards_fold_card_c",
											LoadDict(text36),
											16f,
											-16f,
											0,
											1148846080
										});
									}
									catch
									{
									}
									Random random31 = new Random();
									int num42 = random31.Next(0, 8);
									if (num42 == 0)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_INSULT_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num42 == 1)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_FUCK_YOU", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num42 == 2)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_INSULT_HIGH", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num42 == 3)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_SHOCKED_HIGH", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num42 == 4)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_SHOCKED_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num42 == 5)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_FRIGHTENED_HIGH", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num42 == 6)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_FRIGHTENED_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num42 == 7)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num42 == 8)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									UI.Notify("AI 2 has ~y~Folded!");
								}
							}
							Script.Wait(1);
							if (item18 == AI_3 && !AI3_Bust && !AI3_SkipTurn)
							{
								bool foldCauseHighBet3 = GetFoldCauseHighBet(num34, _8PlayerGamePoker_AI3Bank);
								bool foldCauseLowScore3 = GetFoldCauseLowScore(AI3Total);
								if (_8PlayerGamePoker_AI3Bank >= num34 && !foldCauseHighBet3 && !foldCauseLowScore3)
								{
									text36 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
									text37 = "bet_plus";
									PlaySyncAnim(item18, item18, syncScene, text36, text37);
									Script.Wait(700);
									int num43 = random29.Next(0, 1000);
									int num44 = random29.Next(0, 600);
									if (num44 > num43)
									{
										float num45 = random29.Next(100, (int)MaxRaiseAmt);
										if (_8PlayerGamePoker_AI3Bank - num34 > 0f)
										{
											num34 += num45;
											UI.Notify("AI 3 ~y~Raised~w~ the Bet");
										}
										else
										{
											UI.Notify("AI 3 ~y~Called~w~ the Bet");
										}
									}
									else
									{
										UI.Notify("AI 3 ~y~Called~w~ the Bet");
									}
									_8PlayerGamePokerPot += num34;
									RunBet(num34);
									_8PlayerGamePoker_AI3Bank -= num34;
								}
								else
								{
									AI3_SkipTurn = true;
									text36 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@play@v01";
									text37 = "check_cards_fold";
									LoadDict(text36);
									Scene2 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
									{
										item18.Position.X,
										item18.Position.Y,
										item18.Position.Z,
										0.0,
										0.0,
										item18.Rotation.Z,
										2
									});
									Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
									{
										Scene2,
										Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { Scene2 })
									});
									PlaySyncAnim(item18, item18, Scene2, text36, text37);
									try
									{
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											PlayerCards[index2].Card1,
											Scene2,
											"check_cards_fold_card_a",
											LoadDict(text36),
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											PlayerCards[index2].Card2,
											Scene2,
											"check_cards_fold_card_b",
											LoadDict(text36),
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											PlayerCards[index2].Card3,
											Scene2,
											"check_cards_fold_card_c",
											LoadDict(text36),
											16f,
											-16f,
											0,
											1148846080
										});
									}
									catch
									{
									}
									Random random32 = new Random();
									int num46 = random32.Next(0, 8);
									if (num46 == 0)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_INSULT_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num46 == 1)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_FUCK_YOU", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num46 == 2)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_INSULT_HIGH", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num46 == 3)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_SHOCKED_HIGH", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num46 == 4)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_SHOCKED_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num46 == 5)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_FRIGHTENED_HIGH", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num46 == 6)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_FRIGHTENED_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num46 == 7)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num46 == 8)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									UI.Notify("AI 3 has ~y~Folded!");
								}
							}
							Script.Wait(1);
							if (item18 == AI_4 && !AI4_Bust && !AI4_SkipTurn)
							{
								bool foldCauseHighBet4 = GetFoldCauseHighBet(num34, _8PlayerGamePoker_AI4Bank);
								bool foldCauseLowScore4 = GetFoldCauseLowScore(AI4Total);
								if (_8PlayerGamePoker_AI4Bank >= num34 && !foldCauseHighBet4 && !foldCauseLowScore4)
								{
									text36 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
									text37 = "bet_plus";
									PlaySyncAnim(item18, item18, syncScene, text36, text37);
									Script.Wait(700);
									int num47 = random29.Next(0, 1000);
									int num48 = random29.Next(0, 600);
									if (num48 > num47)
									{
										float num49 = random29.Next(100, (int)MaxRaiseAmt);
										if (_8PlayerGamePoker_AI4Bank - num34 > 0f)
										{
											num34 += num49;
											UI.Notify("AI 4 ~y~Raised~w~ the Bet");
										}
										else
										{
											UI.Notify("AI 4 ~y~Called~w~ the Bet");
										}
									}
									else
									{
										UI.Notify("AI 4 ~y~Called~w~ the Bet");
									}
									_8PlayerGamePokerPot += num34;
									RunBet(num34);
									_8PlayerGamePoker_AI4Bank -= num34;
								}
								else
								{
									AI4_SkipTurn = true;
									text36 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@play@v01";
									text37 = "check_cards_fold";
									LoadDict(text36);
									Scene2 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
									{
										item18.Position.X,
										item18.Position.Y,
										item18.Position.Z,
										0.0,
										0.0,
										item18.Rotation.Z,
										2
									});
									Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
									{
										Scene2,
										Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { Scene2 })
									});
									PlaySyncAnim(item18, item18, Scene2, text36, text37);
									try
									{
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											PlayerCards[index2].Card1,
											Scene2,
											"check_cards_fold_card_a",
											LoadDict(text36),
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											PlayerCards[index2].Card2,
											Scene2,
											"check_cards_fold_card_b",
											LoadDict(text36),
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											PlayerCards[index2].Card3,
											Scene2,
											"check_cards_fold_card_c",
											LoadDict(text36),
											16f,
											-16f,
											0,
											1148846080
										});
									}
									catch
									{
									}
									Random random33 = new Random();
									int num50 = random33.Next(0, 8);
									if (num50 == 0)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_INSULT_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num50 == 1)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_FUCK_YOU", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num50 == 2)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_INSULT_HIGH", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num50 == 3)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_SHOCKED_HIGH", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num50 == 4)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_SHOCKED_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num50 == 5)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_FRIGHTENED_HIGH", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num50 == 6)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_FRIGHTENED_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num50 == 7)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num50 == 8)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									UI.Notify("AI 4 has ~y~Folded!");
								}
							}
							Script.Wait(1);
							if (item18 == AI_5 && !AI5_Bust && !AI5_SkipTurn)
							{
								bool foldCauseHighBet5 = GetFoldCauseHighBet(num34, _8PlayerGamePoker_AI5Bank);
								bool foldCauseLowScore5 = GetFoldCauseLowScore(AI5Total);
								if (_8PlayerGamePoker_AI5Bank >= num34 && !foldCauseHighBet5 && !foldCauseLowScore5)
								{
									text36 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
									text37 = "bet_plus";
									PlaySyncAnim(item18, item18, syncScene, text36, text37);
									Script.Wait(700);
									int num51 = random29.Next(0, 1000);
									int num52 = random29.Next(0, 600);
									if (num52 > num51)
									{
										float num53 = random29.Next(100, (int)MaxRaiseAmt);
										if (_8PlayerGamePoker_AI5Bank - num34 > 0f)
										{
											num34 += num53;
											UI.Notify("AI 5 ~y~Raised~w~ the Bet");
										}
										else
										{
											UI.Notify("AI 5 ~y~Called~w~ the Bet");
										}
									}
									else
									{
										UI.Notify("AI 5 ~y~Called~w~ the Bet");
									}
									_8PlayerGamePokerPot += num34;
									RunBet(num34);
									_8PlayerGamePoker_AI5Bank -= num34;
								}
								else
								{
									AI5_SkipTurn = true;
									text36 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@play@v01";
									text37 = "check_cards_fold";
									LoadDict(text36);
									Scene2 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
									{
										item18.Position.X,
										item18.Position.Y,
										item18.Position.Z,
										0.0,
										0.0,
										item18.Rotation.Z,
										2
									});
									Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
									{
										Scene2,
										Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { Scene2 })
									});
									PlaySyncAnim(item18, item18, Scene2, text36, text37);
									try
									{
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											PlayerCards[index2].Card1,
											Scene2,
											"check_cards_fold_card_a",
											LoadDict(text36),
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											PlayerCards[index2].Card2,
											Scene2,
											"check_cards_fold_card_b",
											LoadDict(text36),
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											PlayerCards[index2].Card3,
											Scene2,
											"check_cards_fold_card_c",
											LoadDict(text36),
											16f,
											-16f,
											0,
											1148846080
										});
									}
									catch
									{
									}
									Random random34 = new Random();
									int num54 = random34.Next(0, 8);
									if (num54 == 0)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_INSULT_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num54 == 1)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_FUCK_YOU", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num54 == 2)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_INSULT_HIGH", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num54 == 3)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_SHOCKED_HIGH", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num54 == 4)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_SHOCKED_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num54 == 5)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_FRIGHTENED_HIGH", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num54 == 6)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_FRIGHTENED_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num54 == 7)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num54 == 8)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									UI.Notify("AI 5 has ~y~Folded!");
								}
							}
							Script.Wait(1);
							if (item18 == AI_6 && !AI6_Bust && !AI6_SkipTurn)
							{
								bool foldCauseHighBet6 = GetFoldCauseHighBet(num34, _8PlayerGamePoker_AI6Bank);
								bool foldCauseLowScore6 = GetFoldCauseLowScore(AI6Total);
								if (_8PlayerGamePoker_AI6Bank >= num34 && !foldCauseHighBet6 && !foldCauseLowScore6)
								{
									text36 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
									text37 = "bet_plus";
									PlaySyncAnim(item18, item18, syncScene, text36, text37);
									Script.Wait(700);
									int num55 = random29.Next(0, 1000);
									int num56 = random29.Next(0, 600);
									if (num56 > num55)
									{
										float num57 = random29.Next(100, (int)MaxRaiseAmt);
										if (_8PlayerGamePoker_AI6Bank - num34 > 0f)
										{
											num34 += num57;
											UI.Notify("AI 6 ~y~Raised~w~ the Bet");
										}
										else
										{
											UI.Notify("AI 6 ~y~Called~w~ the Bet");
										}
									}
									else
									{
										UI.Notify("AI 6 ~y~Called~w~ the Bet");
									}
									_8PlayerGamePokerPot += num34;
									RunBet(num34);
									_8PlayerGamePoker_AI6Bank -= num34;
								}
								else
								{
									AI6_SkipTurn = true;
									text36 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@play@v01";
									text37 = "check_cards_fold";
									LoadDict(text36);
									Scene2 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
									{
										item18.Position.X,
										item18.Position.Y,
										item18.Position.Z,
										0.0,
										0.0,
										item18.Rotation.Z,
										2
									});
									Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
									{
										Scene2,
										Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { Scene2 })
									});
									PlaySyncAnim(item18, item18, Scene2, text36, text37);
									try
									{
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											PlayerCards[index2].Card1,
											Scene2,
											"check_cards_fold_card_a",
											LoadDict(text36),
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											PlayerCards[index2].Card2,
											Scene2,
											"check_cards_fold_card_b",
											LoadDict(text36),
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											PlayerCards[index2].Card3,
											Scene2,
											"check_cards_fold_card_c",
											LoadDict(text36),
											16f,
											-16f,
											0,
											1148846080
										});
									}
									catch
									{
									}
									Random random35 = new Random();
									int num58 = random35.Next(0, 8);
									if (num58 == 0)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_INSULT_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num58 == 1)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_FUCK_YOU", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num58 == 2)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_INSULT_HIGH", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num58 == 3)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_SHOCKED_HIGH", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num58 == 4)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_SHOCKED_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num58 == 5)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_FRIGHTENED_HIGH", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num58 == 6)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_FRIGHTENED_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num58 == 7)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num58 == 8)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									UI.Notify("AI 6 has ~y~Folded!");
								}
							}
							Script.Wait(1);
							if (item18 == AI_7 && !AI7_Bust && !AI7_SkipTurn)
							{
								bool foldCauseHighBet7 = GetFoldCauseHighBet(num34, _8PlayerGamePoker_AI7Bank);
								bool foldCauseLowScore7 = GetFoldCauseLowScore(AI7Total);
								if (_8PlayerGamePoker_AI7Bank >= num34 && !foldCauseHighBet7 && !foldCauseLowScore7)
								{
									text36 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
									text37 = "bet_plus";
									PlaySyncAnim(item18, item18, syncScene, text36, text37);
									Script.Wait(700);
									int num59 = random29.Next(0, 1000);
									int num60 = random29.Next(0, 600);
									if (num60 > num59)
									{
										float num61 = random29.Next(100, (int)MaxRaiseAmt);
										if (_8PlayerGamePoker_AI7Bank - num34 > 0f)
										{
											num34 += num61;
											UI.Notify("AI 7 ~y~Raised~w~ the Bet");
										}
										else
										{
											UI.Notify("AI 7 ~y~Called~w~ the Bet");
										}
									}
									else
									{
										UI.Notify("AI 7 ~y~Called~w~ the Bet");
									}
									_8PlayerGamePokerPot += num34;
									RunBet(num34);
									_8PlayerGamePoker_AI7Bank -= num34;
								}
								else
								{
									AI7_SkipTurn = true;
									text36 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@play@v01";
									text37 = "check_cards_fold";
									LoadDict(text36);
									Scene2 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
									{
										item18.Position.X,
										item18.Position.Y,
										item18.Position.Z,
										0.0,
										0.0,
										item18.Rotation.Z,
										2
									});
									Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
									{
										Scene2,
										Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { Scene2 })
									});
									PlaySyncAnim(item18, item18, Scene2, text36, text37);
									try
									{
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											PlayerCards[index2].Card1,
											Scene2,
											"check_cards_fold_card_a",
											LoadDict(text36),
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											PlayerCards[index2].Card2,
											Scene2,
											"check_cards_fold_card_b",
											LoadDict(text36),
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											PlayerCards[index2].Card3,
											Scene2,
											"check_cards_fold_card_c",
											LoadDict(text36),
											16f,
											-16f,
											0,
											1148846080
										});
									}
									catch
									{
									}
									Random random36 = new Random();
									int num62 = random36.Next(0, 8);
									if (num62 == 0)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_INSULT_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num62 == 1)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_FUCK_YOU", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num62 == 2)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_INSULT_HIGH", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num62 == 3)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_SHOCKED_HIGH", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num62 == 4)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_SHOCKED_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num62 == 5)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_FRIGHTENED_HIGH", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num62 == 6)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_FRIGHTENED_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num62 == 7)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									if (num62 == 8)
									{
										Function.Call(((Hash)0x8E04FEDD28D42462uL), new InputArgument[3] { item18, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear" });
									}
									UI.Notify("AI 7 has ~y~Folded!");
								}
							}
							index2++;
						}
						_8PlayerGameStage = 2;
					}
					catch
					{
						UI.Notify("~g~ HKH191~w~ Player did not enter a valid Bet!");
						IsinBettingRound = false;
					}
				}
				if (_8PlayerGameStage == 2)
				{
					if (IsinBettingRound)
					{
						_8PlayerGameStage = 3;
						CheckPokerBuyOut();
						UI.Notify("Its your turn, either Bet, Fold or Check");
						IsinBettingRound = false;
					}
					if (Game.IsControlJustPressed(Control.Context))
					{
						IsinBettingRound = false;
						UI.Notify("Its your turn, either Bet, Fold or Check");
						_8PlayerGameStage = 3;
					}
				}
				if (_8PlayerGameStage == 1)
				{
					string dict32 = "anim_casino_b@amb@casino@games@threecardpoker@dealer";
					int num63 = 0;
					string anim4 = "deck_shuffle";
					if (AI1_Bust && AI2_Bust && AI3_Bust && AI4_Bust && AI5_Bust && AI6_Bust && AI7_Bust)
					{
						UI.Notify("Exiting Game, All AI went Bust!");
						StackChipHeight = 0.6f;
						StackChip = 0;
						dict32 = "anim_casino_b@amb@casino@games@shared@player@";
						Playing8PlayerPoker = false;
						Prop prop4 = _8PlayerPokerGameTable1;
						num63 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
						{
							Game.Player.Character.Position.X,
							Game.Player.Character.Position.Y,
							prop4.Position.Z,
							0.0,
							0.0,
							prop4.Rotation.Z,
							2
						});
						Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
						{
							Game.Player.Character,
							num63,
							LoadDict(dict32),
							"sit_exit_left",
							16f,
							-16f,
							0,
							1148846080
						});
						Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
						{
							num63,
							Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { num63 })
						});
						anim4 = GetExitAnim();
						PlaySyncExitAnim(AI_1, prop4, AI1_SyncScene, dict32, anim4);
						PlaySyncExitAnim(AI_2, prop4, AI2_SyncScene, dict32, anim4);
						PlaySyncExitAnim(AI_3, prop4, AI3_SyncScene, dict32, anim4);
						PlaySyncExitAnim(AI_4, prop4, AI4_SyncScene, dict32, anim4);
						PlaySyncExitAnim(AI_5, prop4, AI5_SyncScene, dict32, anim4);
						PlaySyncExitAnim(AI_6, prop4, AI6_SyncScene, dict32, anim4);
						PlaySyncExitAnim(AI_7, prop4, AI7_SyncScene, dict32, anim4);
						Script.Wait(4000);
						Game.Player.Character.Task.ClearAll();
						if (AI_1 != null)
						{
							AI_1.Task.WanderAround();
						}
						if (AI_2 != null)
						{
							AI_2.Task.WanderAround();
						}
						if (AI_3 != null)
						{
							AI_3.Task.WanderAround();
						}
						if (AI_4 != null)
						{
							AI_4.Task.WanderAround();
						}
						if (AI_5 != null)
						{
							AI_5.Task.WanderAround();
						}
						if (AI_6 != null)
						{
							AI_6.Task.WanderAround();
						}
						if (AI_7 != null)
						{
							AI_7.Task.WanderAround();
						}
						foreach (Ped _8PlayerGamePokerPed3 in _8PlayerGamePokerPeds)
						{
							if (_8PlayerGamePokerPed3 != null)
							{
								_8PlayerGamePokerPed3.Task.WanderAround();
							}
						}
					}
					StackChipHeight = 0.6f;
					StackChip = 0;
					Player_SkipTurn = false;
					AI1_SkipTurn = false;
					AI2_SkipTurn = false;
					AI3_SkipTurn = false;
					AI4_SkipTurn = false;
					AI5_SkipTurn = false;
					AI6_SkipTurn = false;
					AI7_SkipTurn = false;
					foreach (Prop _8PlayerGamePokerProp3 in _8PlayerGamePokerProps)
					{
						if (_8PlayerGamePokerProp3 != null)
						{
							_8PlayerGamePokerProp3.Delete();
						}
					}
					foreach (Prop _8PlayerGamePokerPotChip3 in _8PlayerGamePokerPotChips)
					{
						if (_8PlayerGamePokerPotChip3 != null)
						{
							_8PlayerGamePokerPotChip3.Delete();
						}
					}
					_8PlayerGamePokerPot = 0f;
					_8PlayerGamePokerProps.Clear();
					Random random37 = new Random();
					List<Ped> list3 = new List<Ped>();
					list3.Add(AI_1);
					list3.Add(AI_2);
					list3.Add(AI_3);
					list3.Add(AI_4);
					list3.Add(AI_5);
					list3.Add(AI_6);
					list3.Add(AI_7);
					foreach (Ped item19 in list3)
					{
						Prop prop5 = World.CreateProp(RequestModel("vw_prop_chip_5kdollar_st"), item19.GetOffsetInWorldCoords(new Vector3(0.49f, 0.6f, -0.03f)), false, false);
						_8PlayerGamePokerProps.Add(prop5);
						Prop prop6 = World.CreateProp(RequestModel("vw_prop_chip_1kdollar_st"), item19.GetOffsetInWorldCoords(new Vector3(0.49f, 0.55f, -0.03f)), false, false);
						_8PlayerGamePokerProps.Add(prop6);
						Prop prop7 = World.CreateProp(RequestModel("vw_prop_chip_50dollar_st"), item19.GetOffsetInWorldCoords(new Vector3(0.45f, 0.6f, -0.03f)), false, false);
						_8PlayerGamePokerProps.Add(prop7);
						Prop prop8 = World.CreateProp(RequestModel("vw_prop_chip_100dollar_st"), item19.GetOffsetInWorldCoords(new Vector3(0.45f, 0.55f, -0.03f)), false, false);
						_8PlayerGamePokerProps.Add(prop8);
						PlayerChips.Add(new PlayerChips(prop5, prop6, prop7, prop8));
					}
					Prop prop9 = World.CreateProp(RequestModel("vw_prop_chip_5kdollar_st"), Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.29f, 0.6f, -0.03f)), false, false);
					_8PlayerGamePokerProps.Add(prop9);
					Prop prop10 = World.CreateProp(RequestModel("vw_prop_chip_1kdollar_st"), Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.29f, 0.55f, -0.03f)), false, false);
					_8PlayerGamePokerProps.Add(prop10);
					Prop prop11 = World.CreateProp(RequestModel("vw_prop_chip_50dollar_st"), Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.25f, 0.6f, -0.03f)), false, false);
					_8PlayerGamePokerProps.Add(prop11);
					Prop prop12 = World.CreateProp(RequestModel("vw_prop_chip_100dollar_st"), Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.25f, 0.55f, -0.03f)), false, false);
					_8PlayerGamePokerProps.Add(prop12);
					PlayerChips.Add(new PlayerChips(prop9, prop10, prop11, prop12));
					Deck = World.CreateProp(RequestModel("vw_prop_casino_cards_01"), _8PlayerPokerGameTable1.Position, false, false);
					_8PlayerGamePokerProps.Add(Deck);
					Deck.Position = _8PlayerGameDealer.GetBoneCoord((Bone)36029);
					Deck.AttachTo(_8PlayerGameDealer, 36029);
					_8PlayerGameDealer.Position = new Vector3(1136.207f, 236.0575f, -50.4f);
					_8PlayerGameDealer.Heading = -45f;
					PlaySyncAnimRotate180(_8PlayerGameDealer, _8PlayerPokerGameTable1, num63, dict32, anim4);
					anim4 = "deck_shuffle";
					Script.Wait(1000);
					_8PlayerGameDealer.Position = new Vector3(1136.207f, 236.0575f, -50.4f);
					_8PlayerGameDealer.Heading = -45f;
					PlaySyncAnimRotate180(_8PlayerGameDealer, _8PlayerPokerGameTable1, num63, dict32, anim4);
					Script.Wait(1000);
					int num64 = 0;
					list3.Add(Game.Player.Character);
					Script.Wait(1);
					string randomCardPlayer = GetRandomCardPlayer();
					Script.Wait(1);
					string randomCardPlayer2 = GetRandomCardPlayer();
					Script.Wait(1);
					string randomCardPlayer3 = GetRandomCardPlayer();
					CommunityCard1ID = GetCardName(randomCardPlayer);
					CommunityCard2ID = GetCardName(randomCardPlayer2);
					CommunityCard3ID = GetCardName(randomCardPlayer3);
					CommunityCard1 = World.CreateProp(RequestModel(randomCardPlayer), _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-0.35f, 0f, -0.4f)), true, false);
					CommunityCard1Value = GetCardTotal(randomCardPlayer);
					CommunityCard1.Heading = _8PlayerPokerGameTable3.Heading - 90f;
					_8PlayerGamePokerProps.Add(CommunityCard1);
					CommunityCard2 = World.CreateProp(RequestModel(randomCardPlayer2), _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-0.35f, -0.1f, -0.4f)), true, false);
					CommunityCard2Value = GetCardTotal(randomCardPlayer2);
					CommunityCard2.Heading = _8PlayerPokerGameTable3.Heading - 90f;
					_8PlayerGamePokerProps.Add(CommunityCard2);
					CommunityCard3 = World.CreateProp(RequestModel(randomCardPlayer3), _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-0.35f, 0.1f, -0.4f)), true, false);
					CommunityCard3Value = GetCardTotal(randomCardPlayer3);
					CommunityCard3.Heading = _8PlayerPokerGameTable3.Heading - 90f;
					_8PlayerGamePokerProps.Add(CommunityCard3);
					CommunityCard1.Rotation = new Vector3(CommunityCard1.Rotation.X, 180f, CommunityCard1.Rotation.Z);
					CommunityCard2.Rotation = new Vector3(CommunityCard2.Rotation.X, 180f, CommunityCard2.Rotation.Z);
					CommunityCard3.Rotation = new Vector3(CommunityCard3.Rotation.X, 180f, CommunityCard3.Rotation.Z);
					PlayerCards.Clear();
					foreach (Ped item20 in list3)
					{
						if (item20 == AI_1 && !AI1_Bust)
						{
							Script.Wait(1);
							string randomCardPlayer4 = GetRandomCardPlayer();
							Prop prop13 = World.CreateProp(RequestModel(randomCardPlayer4), item20.GetOffsetInWorldCoords(new Vector3(0.08f, 0.5f, -1.05f)), true, false);
							prop13.Heading = item20.Heading;
							_8PlayerGamePokerProps.Add(prop13);
							Script.Wait(1);
							string randomCardPlayer5 = GetRandomCardPlayer();
							Prop prop14 = World.CreateProp(RequestModel(randomCardPlayer5), item20.GetOffsetInWorldCoords(new Vector3(0f, 0.5f, -1.05f)), true, false);
							prop14.Heading = item20.Heading;
							_8PlayerGamePokerProps.Add(prop14);
							Script.Wait(1);
							string randomCardPlayer6 = GetRandomCardPlayer();
							Prop prop15 = World.CreateProp(RequestModel(randomCardPlayer6), item20.GetOffsetInWorldCoords(new Vector3(-0.08f, 0.5f, -1.05f)), true, false);
							prop15.Heading = item20.Heading;
							_8PlayerGamePokerProps.Add(prop15);
							Script.Wait(1);
							int num65 = random37.Next(1, 2);
							Script.Wait(1);
							dict32 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v02";
							anim4 = "check_cards_bet";
							LoadDict(dict32);
							num63 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
							{
								item20.Position.X,
								item20.Position.Y,
								item20.Position.Z,
								0.0,
								0.0,
								item20.Rotation.Z,
								2
							});
							PlaySyncAnim(item20, item20, num63, dict32, anim4);
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								prop13,
								num63,
								"check_cards_bet_card_a",
								LoadDict(dict32),
								16f,
								-16f,
								0,
								1148846080
							});
							_8PlayerGamePokerPot += _8PlayerGamePoker_AI1StartingBank / 100f;
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								prop14,
								num63,
								"check_cards_bet_card_b",
								LoadDict(dict32),
								16f,
								-16f,
								0,
								1148846080
							});
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								prop15,
								num63,
								"check_cards_bet_card_c",
								LoadDict(dict32),
								16f,
								-16f,
								0,
								1148846080
							});
							PlayerCards.Add(new PlayerCards(prop13, prop14, prop15, randomCardPlayer4, randomCardPlayer5, randomCardPlayer6));
							if (item20 == Game.Player.Character)
							{
								PlayerCard1 = prop13;
								PlayerCard2 = prop14;
								PlayerCard3 = prop15;
							}
							AI1Total = GetPlayerTotal(randomCardPlayer4, randomCardPlayer5, randomCardPlayer6);
						}
						if (item20 == AI_2 && !AI2_Bust)
						{
							Script.Wait(1);
							string randomCardPlayer7 = GetRandomCardPlayer();
							Prop prop16 = World.CreateProp(RequestModel(randomCardPlayer7), item20.GetOffsetInWorldCoords(new Vector3(0.08f, 0.5f, -1.05f)), true, false);
							prop16.Heading = item20.Heading;
							_8PlayerGamePokerProps.Add(prop16);
							Script.Wait(1);
							string randomCardPlayer8 = GetRandomCardPlayer();
							Prop prop17 = World.CreateProp(RequestModel(randomCardPlayer8), item20.GetOffsetInWorldCoords(new Vector3(0f, 0.5f, -1.05f)), true, false);
							prop17.Heading = item20.Heading;
							_8PlayerGamePokerProps.Add(prop17);
							Script.Wait(1);
							string randomCardPlayer9 = GetRandomCardPlayer();
							Prop prop18 = World.CreateProp(RequestModel(randomCardPlayer9), item20.GetOffsetInWorldCoords(new Vector3(-0.08f, 0.5f, -1.05f)), true, false);
							prop18.Heading = item20.Heading;
							_8PlayerGamePokerProps.Add(prop18);
							Script.Wait(1);
							int num66 = random37.Next(1, 2);
							Script.Wait(1);
							dict32 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v02";
							anim4 = "check_cards_bet";
							LoadDict(dict32);
							num63 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
							{
								item20.Position.X,
								item20.Position.Y,
								item20.Position.Z,
								0.0,
								0.0,
								item20.Rotation.Z,
								2
							});
							PlaySyncAnim(item20, item20, num63, dict32, anim4);
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								prop16,
								num63,
								"check_cards_bet_card_a",
								LoadDict(dict32),
								16f,
								-16f,
								0,
								1148846080
							});
							_8PlayerGamePokerPot += _8PlayerGamePoker_AI1StartingBank / 100f;
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								prop17,
								num63,
								"check_cards_bet_card_b",
								LoadDict(dict32),
								16f,
								-16f,
								0,
								1148846080
							});
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								prop18,
								num63,
								"check_cards_bet_card_c",
								LoadDict(dict32),
								16f,
								-16f,
								0,
								1148846080
							});
							PlayerCards.Add(new PlayerCards(prop16, prop17, prop18, randomCardPlayer7, randomCardPlayer8, randomCardPlayer9));
							if (item20 == Game.Player.Character)
							{
								PlayerCard1 = prop16;
								PlayerCard2 = prop17;
								PlayerCard3 = prop18;
							}
							AI2Total = GetPlayerTotal(randomCardPlayer7, randomCardPlayer8, randomCardPlayer9);
						}
						if (item20 == AI_3 && !AI3_Bust)
						{
							Script.Wait(1);
							string randomCardPlayer10 = GetRandomCardPlayer();
							Prop prop19 = World.CreateProp(RequestModel(randomCardPlayer10), item20.GetOffsetInWorldCoords(new Vector3(0.08f, 0.5f, -1.05f)), true, false);
							prop19.Heading = item20.Heading;
							_8PlayerGamePokerProps.Add(prop19);
							Script.Wait(1);
							string randomCardPlayer11 = GetRandomCardPlayer();
							Prop prop20 = World.CreateProp(RequestModel(randomCardPlayer11), item20.GetOffsetInWorldCoords(new Vector3(0f, 0.5f, -1.05f)), true, false);
							prop20.Heading = item20.Heading;
							_8PlayerGamePokerProps.Add(prop20);
							Script.Wait(1);
							string randomCardPlayer12 = GetRandomCardPlayer();
							Prop prop21 = World.CreateProp(RequestModel(randomCardPlayer12), item20.GetOffsetInWorldCoords(new Vector3(-0.08f, 0.5f, -1.05f)), true, false);
							prop21.Heading = item20.Heading;
							_8PlayerGamePokerProps.Add(prop21);
							Script.Wait(1);
							int num67 = random37.Next(1, 2);
							Script.Wait(1);
							dict32 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v02";
							anim4 = "check_cards_bet";
							LoadDict(dict32);
							num63 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
							{
								item20.Position.X,
								item20.Position.Y,
								item20.Position.Z,
								0.0,
								0.0,
								item20.Rotation.Z,
								2
							});
							PlaySyncAnim(item20, item20, num63, dict32, anim4);
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								prop19,
								num63,
								"check_cards_bet_card_a",
								LoadDict(dict32),
								16f,
								-16f,
								0,
								1148846080
							});
							_8PlayerGamePokerPot += _8PlayerGamePoker_AI1StartingBank / 100f;
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								prop20,
								num63,
								"check_cards_bet_card_b",
								LoadDict(dict32),
								16f,
								-16f,
								0,
								1148846080
							});
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								prop21,
								num63,
								"check_cards_bet_card_c",
								LoadDict(dict32),
								16f,
								-16f,
								0,
								1148846080
							});
							PlayerCards.Add(new PlayerCards(prop19, prop20, prop21, randomCardPlayer10, randomCardPlayer11, randomCardPlayer12));
							if (item20 == Game.Player.Character)
							{
								PlayerCard1 = prop19;
								PlayerCard2 = prop20;
								PlayerCard3 = prop21;
							}
							AI3Total = GetPlayerTotal(randomCardPlayer10, randomCardPlayer11, randomCardPlayer12);
						}
						if (item20 == AI_4 && !AI4_Bust)
						{
							Script.Wait(1);
							string randomCardPlayer13 = GetRandomCardPlayer();
							Prop prop22 = World.CreateProp(RequestModel(randomCardPlayer13), item20.GetOffsetInWorldCoords(new Vector3(0.08f, 0.5f, -1.05f)), true, false);
							prop22.Heading = item20.Heading;
							_8PlayerGamePokerProps.Add(prop22);
							Script.Wait(1);
							string randomCardPlayer14 = GetRandomCardPlayer();
							Prop prop23 = World.CreateProp(RequestModel(randomCardPlayer14), item20.GetOffsetInWorldCoords(new Vector3(0f, 0.5f, -1.05f)), true, false);
							prop23.Heading = item20.Heading;
							_8PlayerGamePokerProps.Add(prop23);
							Script.Wait(1);
							string randomCardPlayer15 = GetRandomCardPlayer();
							Prop prop24 = World.CreateProp(RequestModel(randomCardPlayer15), item20.GetOffsetInWorldCoords(new Vector3(-0.08f, 0.5f, -1.05f)), true, false);
							prop24.Heading = item20.Heading;
							_8PlayerGamePokerProps.Add(prop24);
							Script.Wait(1);
							int num68 = random37.Next(1, 2);
							Script.Wait(1);
							dict32 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v02";
							anim4 = "check_cards_bet";
							LoadDict(dict32);
							num63 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
							{
								item20.Position.X,
								item20.Position.Y,
								item20.Position.Z,
								0.0,
								0.0,
								item20.Rotation.Z,
								2
							});
							PlaySyncAnim(item20, item20, num63, dict32, anim4);
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								prop22,
								num63,
								"check_cards_bet_card_a",
								LoadDict(dict32),
								16f,
								-16f,
								0,
								1148846080
							});
							_8PlayerGamePokerPot += _8PlayerGamePoker_AI1StartingBank / 100f;
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								prop23,
								num63,
								"check_cards_bet_card_b",
								LoadDict(dict32),
								16f,
								-16f,
								0,
								1148846080
							});
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								prop24,
								num63,
								"check_cards_bet_card_c",
								LoadDict(dict32),
								16f,
								-16f,
								0,
								1148846080
							});
							PlayerCards.Add(new PlayerCards(prop22, prop23, prop24, randomCardPlayer13, randomCardPlayer14, randomCardPlayer15));
							if (item20 == Game.Player.Character)
							{
								PlayerCard1 = prop22;
								PlayerCard2 = prop23;
								PlayerCard3 = prop24;
							}
							AI4Total = GetPlayerTotal(randomCardPlayer13, randomCardPlayer14, randomCardPlayer15);
						}
						if (item20 == AI_5 && !AI5_Bust)
						{
							Script.Wait(1);
							string randomCardPlayer16 = GetRandomCardPlayer();
							Prop prop25 = World.CreateProp(RequestModel(randomCardPlayer16), item20.GetOffsetInWorldCoords(new Vector3(0.08f, 0.5f, -1.05f)), true, false);
							prop25.Heading = item20.Heading;
							_8PlayerGamePokerProps.Add(prop25);
							Script.Wait(1);
							string randomCardPlayer17 = GetRandomCardPlayer();
							Prop prop26 = World.CreateProp(RequestModel(randomCardPlayer17), item20.GetOffsetInWorldCoords(new Vector3(0f, 0.5f, -1.05f)), true, false);
							prop26.Heading = item20.Heading;
							_8PlayerGamePokerProps.Add(prop26);
							Script.Wait(1);
							string randomCardPlayer18 = GetRandomCardPlayer();
							Prop prop27 = World.CreateProp(RequestModel(randomCardPlayer18), item20.GetOffsetInWorldCoords(new Vector3(-0.08f, 0.5f, -1.05f)), true, false);
							prop27.Heading = item20.Heading;
							_8PlayerGamePokerProps.Add(prop27);
							Script.Wait(1);
							int num69 = random37.Next(1, 2);
							Script.Wait(1);
							dict32 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v02";
							anim4 = "check_cards_bet";
							LoadDict(dict32);
							num63 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
							{
								item20.Position.X,
								item20.Position.Y,
								item20.Position.Z,
								0.0,
								0.0,
								item20.Rotation.Z,
								2
							});
							PlaySyncAnim(item20, item20, num63, dict32, anim4);
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								prop25,
								num63,
								"check_cards_bet_card_a",
								LoadDict(dict32),
								16f,
								-16f,
								0,
								1148846080
							});
							_8PlayerGamePokerPot += _8PlayerGamePoker_AI1StartingBank / 100f;
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								prop26,
								num63,
								"check_cards_bet_card_b",
								LoadDict(dict32),
								16f,
								-16f,
								0,
								1148846080
							});
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								prop27,
								num63,
								"check_cards_bet_card_c",
								LoadDict(dict32),
								16f,
								-16f,
								0,
								1148846080
							});
							PlayerCards.Add(new PlayerCards(prop25, prop26, prop27, randomCardPlayer16, randomCardPlayer17, randomCardPlayer18));
							if (item20 == Game.Player.Character)
							{
								PlayerCard1 = prop25;
								PlayerCard2 = prop26;
								PlayerCard3 = prop27;
							}
							AI5Total = GetPlayerTotal(randomCardPlayer16, randomCardPlayer17, randomCardPlayer18);
						}
						if (item20 == AI_6 && !AI6_Bust)
						{
							Script.Wait(1);
							string randomCardPlayer19 = GetRandomCardPlayer();
							Prop prop28 = World.CreateProp(RequestModel(randomCardPlayer19), item20.GetOffsetInWorldCoords(new Vector3(0.08f, 0.5f, -1.05f)), true, false);
							prop28.Heading = item20.Heading;
							_8PlayerGamePokerProps.Add(prop28);
							Script.Wait(1);
							string randomCardPlayer20 = GetRandomCardPlayer();
							Prop prop29 = World.CreateProp(RequestModel(randomCardPlayer20), item20.GetOffsetInWorldCoords(new Vector3(0f, 0.5f, -1.05f)), true, false);
							prop29.Heading = item20.Heading;
							_8PlayerGamePokerProps.Add(prop29);
							Script.Wait(1);
							string randomCardPlayer21 = GetRandomCardPlayer();
							Prop prop30 = World.CreateProp(RequestModel(randomCardPlayer21), item20.GetOffsetInWorldCoords(new Vector3(-0.08f, 0.5f, -1.05f)), true, false);
							prop30.Heading = item20.Heading;
							_8PlayerGamePokerProps.Add(prop30);
							Script.Wait(1);
							int num70 = random37.Next(1, 2);
							Script.Wait(1);
							dict32 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v02";
							anim4 = "check_cards_bet";
							LoadDict(dict32);
							num63 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
							{
								item20.Position.X,
								item20.Position.Y,
								item20.Position.Z,
								0.0,
								0.0,
								item20.Rotation.Z,
								2
							});
							PlaySyncAnim(item20, item20, num63, dict32, anim4);
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								prop28,
								num63,
								"check_cards_bet_card_a",
								LoadDict(dict32),
								16f,
								-16f,
								0,
								1148846080
							});
							_8PlayerGamePokerPot += _8PlayerGamePoker_AI1StartingBank / 100f;
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								prop29,
								num63,
								"check_cards_bet_card_b",
								LoadDict(dict32),
								16f,
								-16f,
								0,
								1148846080
							});
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								prop30,
								num63,
								"check_cards_bet_card_c",
								LoadDict(dict32),
								16f,
								-16f,
								0,
								1148846080
							});
							PlayerCards.Add(new PlayerCards(prop28, prop29, prop30, randomCardPlayer19, randomCardPlayer20, randomCardPlayer21));
							if (item20 == Game.Player.Character)
							{
								PlayerCard1 = prop28;
								PlayerCard2 = prop29;
								PlayerCard3 = prop30;
							}
							AI6Total = GetPlayerTotal(randomCardPlayer19, randomCardPlayer20, randomCardPlayer21);
						}
						if (item20 == AI_7 && !AI7_Bust)
						{
							Script.Wait(1);
							string randomCardPlayer22 = GetRandomCardPlayer();
							Prop prop31 = World.CreateProp(RequestModel(randomCardPlayer22), item20.GetOffsetInWorldCoords(new Vector3(0.08f, 0.5f, -1.05f)), true, false);
							prop31.Heading = item20.Heading;
							_8PlayerGamePokerProps.Add(prop31);
							Script.Wait(1);
							string randomCardPlayer23 = GetRandomCardPlayer();
							Prop prop32 = World.CreateProp(RequestModel(randomCardPlayer23), item20.GetOffsetInWorldCoords(new Vector3(0f, 0.5f, -1.05f)), true, false);
							prop32.Heading = item20.Heading;
							_8PlayerGamePokerProps.Add(prop32);
							Script.Wait(1);
							string randomCardPlayer24 = GetRandomCardPlayer();
							Prop prop33 = World.CreateProp(RequestModel(randomCardPlayer24), item20.GetOffsetInWorldCoords(new Vector3(-0.08f, 0.5f, -1.05f)), true, false);
							prop33.Heading = item20.Heading;
							_8PlayerGamePokerProps.Add(prop33);
							Script.Wait(1);
							int num71 = random37.Next(1, 2);
							Script.Wait(1);
							dict32 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v02";
							anim4 = "check_cards_bet";
							LoadDict(dict32);
							num63 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
							{
								item20.Position.X,
								item20.Position.Y,
								item20.Position.Z,
								0.0,
								0.0,
								item20.Rotation.Z,
								2
							});
							PlaySyncAnim(item20, item20, num63, dict32, anim4);
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								prop31,
								num63,
								"check_cards_bet_card_a",
								LoadDict(dict32),
								16f,
								-16f,
								0,
								1148846080
							});
							_8PlayerGamePokerPot += _8PlayerGamePoker_AI1StartingBank / 100f;
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								prop32,
								num63,
								"check_cards_bet_card_b",
								LoadDict(dict32),
								16f,
								-16f,
								0,
								1148846080
							});
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								prop33,
								num63,
								"check_cards_bet_card_c",
								LoadDict(dict32),
								16f,
								-16f,
								0,
								1148846080
							});
							PlayerCards.Add(new PlayerCards(prop31, prop32, prop33, randomCardPlayer22, randomCardPlayer23, randomCardPlayer24));
							if (item20 == Game.Player.Character)
							{
								PlayerCard1 = prop31;
								PlayerCard2 = prop32;
								PlayerCard3 = prop33;
							}
							AI7Total = GetPlayerTotal(randomCardPlayer22, randomCardPlayer23, randomCardPlayer24);
						}
						if (item20 == Game.Player.Character)
						{
							Script.Wait(1);
							string randomCardPlayer25 = GetRandomCardPlayer();
							Prop prop34 = World.CreateProp(RequestModel(randomCardPlayer25), item20.GetOffsetInWorldCoords(new Vector3(0.08f, 0.5f, -1.05f)), true, false);
							prop34.Heading = item20.Heading;
							_8PlayerGamePokerProps.Add(prop34);
							Script.Wait(1);
							string randomCardPlayer26 = GetRandomCardPlayer();
							Prop prop35 = World.CreateProp(RequestModel(randomCardPlayer26), item20.GetOffsetInWorldCoords(new Vector3(0f, 0.5f, -1.05f)), true, false);
							prop35.Heading = item20.Heading;
							_8PlayerGamePokerProps.Add(prop35);
							Script.Wait(1);
							string randomCardPlayer27 = GetRandomCardPlayer();
							Prop prop36 = World.CreateProp(RequestModel(randomCardPlayer27), item20.GetOffsetInWorldCoords(new Vector3(-0.08f, 0.5f, -1.05f)), true, false);
							prop36.Heading = item20.Heading;
							_8PlayerGamePokerProps.Add(prop36);
							Script.Wait(1);
							int num72 = random37.Next(1, 2);
							Script.Wait(1);
							dict32 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v02";
							anim4 = "check_cards_bet";
							LoadDict(dict32);
							num63 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
							{
								item20.Position.X,
								item20.Position.Y,
								item20.Position.Z,
								0.0,
								0.0,
								item20.Rotation.Z,
								2
							});
							PlaySyncAnim(item20, item20, num63, dict32, anim4);
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								prop34,
								num63,
								"check_cards_bet_card_a",
								LoadDict(dict32),
								16f,
								-16f,
								0,
								1148846080
							});
							_8PlayerGamePokerPot += _8PlayerGamePoker_AI1StartingBank / 100f;
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								prop35,
								num63,
								"check_cards_bet_card_b",
								LoadDict(dict32),
								16f,
								-16f,
								0,
								1148846080
							});
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								prop36,
								num63,
								"check_cards_bet_card_c",
								LoadDict(dict32),
								16f,
								-16f,
								0,
								1148846080
							});
							PlayerCards.Add(new PlayerCards(prop34, prop35, prop36, randomCardPlayer25, randomCardPlayer26, randomCardPlayer27));
							if (item20 == Game.Player.Character)
							{
								PlayerCard1 = prop34;
								PlayerCard2 = prop35;
								PlayerCard3 = prop36;
							}
							PlayerTotal = GetPlayerTotal(randomCardPlayer25, randomCardPlayer26, randomCardPlayer27);
						}
						num64++;
						Script.Wait(100);
					}
					if (Deck != null)
					{
						Deck.Delete();
					}
					Deck = World.CreateProp(RequestModel("vw_prop_casino_cards_01"), _8PlayerPokerGameTable1.Position, false, false);
					_8PlayerGamePokerProps.Add(Deck);
					Script.Wait(10000);
					Deck.Detach();
					if (ChipsSpawn != null)
					{
						ChipsSpawn.Delete();
					}
					ChipsSpawn = World.CreateProp(RequestModel("vw_prop_chip_1kdollar_x1"), _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0f, 0f, 1f)), false, false);
					ChipsSpawn.IsCollisionEnabled = true;
					ChipsSpawn.IsPositionFrozen = true;
					ChipsSpawn.IsVisible = false;
					foreach (Ped item21 in list3)
					{
						if (item21 == AI_1 && !AI1_Bust)
						{
							dict32 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
							anim4 = "bet_plus";
							PlaySyncAnim(item21, item21, num63, dict32, anim4);
							Script.Wait(700);
							RunBet(_8PlayerGamePoker_AI1StartingBank / 100f);
							_8PlayerGamePoker_AI1Bank -= _8PlayerGamePoker_AI1StartingBank / 100f;
						}
						if (item21 == AI_2 && !AI2_Bust)
						{
							dict32 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
							anim4 = "bet_plus";
							PlaySyncAnim(item21, item21, num63, dict32, anim4);
							Script.Wait(700);
							RunBet(_8PlayerGamePoker_AI1StartingBank / 100f);
							_8PlayerGamePoker_AI2Bank -= _8PlayerGamePoker_AI2StartingBank / 100f;
						}
						if (item21 == AI_3 && !AI3_Bust)
						{
							dict32 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
							anim4 = "bet_plus";
							PlaySyncAnim(item21, item21, num63, dict32, anim4);
							Script.Wait(700);
							RunBet(_8PlayerGamePoker_AI1StartingBank / 100f);
							_8PlayerGamePoker_AI3Bank -= _8PlayerGamePoker_AI3StartingBank / 100f;
						}
						if (item21 == AI_4 && !AI4_Bust)
						{
							dict32 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
							anim4 = "bet_plus";
							PlaySyncAnim(item21, item21, num63, dict32, anim4);
							Script.Wait(700);
							RunBet(_8PlayerGamePoker_AI1StartingBank / 100f);
							_8PlayerGamePoker_AI4Bank -= _8PlayerGamePoker_AI4StartingBank / 100f;
						}
						if (item21 == AI_5 && !AI5_Bust)
						{
							dict32 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
							anim4 = "bet_plus";
							PlaySyncAnim(item21, item21, num63, dict32, anim4);
							Script.Wait(700);
							RunBet(_8PlayerGamePoker_AI1StartingBank / 100f);
							_8PlayerGamePoker_AI5Bank -= _8PlayerGamePoker_AI5StartingBank / 100f;
						}
						if (item21 == AI_6 && !AI6_Bust)
						{
							dict32 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
							anim4 = "bet_plus";
							PlaySyncAnim(item21, item21, num63, dict32, anim4);
							Script.Wait(700);
							RunBet(_8PlayerGamePoker_AI1StartingBank / 100f);
							_8PlayerGamePoker_AI6Bank -= _8PlayerGamePoker_AI6StartingBank / 100f;
						}
						if (item21 == AI_7 && !AI7_Bust)
						{
							dict32 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
							anim4 = "bet_plus";
							PlaySyncAnim(item21, item21, num63, dict32, anim4);
							Script.Wait(700);
							RunBet(_8PlayerGamePoker_AI1StartingBank / 100f);
							_8PlayerGamePoker_AI7Bank -= _8PlayerGamePoker_AI7StartingBank / 100f;
						}
						if (item21 == Game.Player.Character)
						{
							dict32 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
							anim4 = "bet_plus";
							PlaySyncAnim(item21, item21, num63, dict32, anim4);
							Script.Wait(700);
							RunBet(_8PlayerGamePoker_AI1StartingBank / 100f);
							_8PlayerGamePoker_PlayerBank -= _8PlayerGamePoker_PlayerStartingBank / 100f;
						}
					}
					LastRaiseAmmount = _8PlayerGamePoker_AI1StartingBank / 100f;
					_8PlayerGameStage = 2;
				}
			}
			if (Game.IsControlJustPressed(Control.Cover))
			{
				if (_8PlayerPokerGameTable1 != null)
				{
					Vector3 offsetInWorldCoords16 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-1.05f, -0.65f, 0.25f));
					if (World.GetDistance(Game.Player.Character.Position, offsetInWorldCoords16) < 2.25f)
					{
						try
						{
							if (!Playing8PlayerPoker)
							{
								if (_8PlayerGamePoker_PlayerStartingBank <= Chips_Amount)
								{
									Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
									Game.Player.Money -= (int)_8PlayerGamePoker_PlayerStartingBank;
									StackChipHeight = 0.6f;
									StackChip = 0;
									Playing8PlayerPoker = true;
									if (ChipsSpawn != null)
									{
										ChipsSpawn.Delete();
									}
									if (AI_1 != null)
									{
										AI_1.Delete();
									}
									if (AI_2 != null)
									{
										AI_2.Delete();
									}
									if (AI_3 != null)
									{
										AI_3.Delete();
									}
									if (AI_4 != null)
									{
										AI_4.Delete();
									}
									if (AI_5 != null)
									{
										AI_5.Delete();
									}
									if (AI_6 != null)
									{
										AI_6.Delete();
									}
									if (AI_7 != null)
									{
										AI_7.Delete();
									}
									foreach (Ped _8PlayerGamePokerPed4 in _8PlayerGamePokerPeds)
									{
										if (_8PlayerGamePokerPed4 != null)
										{
											_8PlayerGamePokerPed4.Delete();
										}
									}
									if (_8PlayerGamePokerPeds.Count > 0)
									{
										_8PlayerGamePokerPeds.Clear();
									}
									foreach (Prop _8PlayerGamePokerProp4 in _8PlayerGamePokerProps)
									{
										if (_8PlayerGamePokerProp4 != null)
										{
											_8PlayerGamePokerProp4.Delete();
										}
									}
									foreach (Prop _8PlayerGamePokerPotChip4 in _8PlayerGamePokerPotChips)
									{
										if (_8PlayerGamePokerPotChip4 != null)
										{
											_8PlayerGamePokerPotChip4.Delete();
										}
									}
									if (PlayerCards.Count > 0)
									{
										PlayerCards.Clear();
									}
									if (PlayerChips.Count > 0)
									{
										PlayerChips.Clear();
									}
									_8PlayerGamePokerPot = 0f;
									if (_8PlayerGamePokerPotChips.Count > 0)
									{
										_8PlayerGamePokerPotChips.Clear();
									}
									if (_8PlayerGamePokerProps.Count > 0)
									{
										_8PlayerGamePokerProps.Clear();
									}
									_8PlayerGamePoker_PlayerStartingBank = 100000f;
									_8PlayerGamePoker_AI1StartingBank = 100000f;
									_8PlayerGamePoker_AI2StartingBank = 100000f;
									_8PlayerGamePoker_AI3StartingBank = 100000f;
									_8PlayerGamePoker_AI4StartingBank = 100000f;
									_8PlayerGamePoker_AI5StartingBank = 100000f;
									_8PlayerGamePoker_AI6StartingBank = 100000f;
									_8PlayerGamePoker_AI7StartingBank = 100000f;
									_8PlayerGamePoker_AI1Bank = _8PlayerGamePoker_AI1StartingBank;
									_8PlayerGamePoker_AI2Bank = _8PlayerGamePoker_AI2StartingBank;
									_8PlayerGamePoker_AI3Bank = _8PlayerGamePoker_AI3StartingBank;
									_8PlayerGamePoker_AI4Bank = _8PlayerGamePoker_AI4StartingBank;
									_8PlayerGamePoker_AI5Bank = _8PlayerGamePoker_AI5StartingBank;
									_8PlayerGamePoker_AI6Bank = _8PlayerGamePoker_AI6StartingBank;
									_8PlayerGamePoker_AI7Bank = _8PlayerGamePoker_AI7StartingBank;
									_8PlayerGamePoker_PlayerBank = _8PlayerGamePoker_PlayerStartingBank;
									Script.Wait(100);
									string text38 = "anim_casino_b@amb@casino@games@shared@player@";
									LoadDict("anim_casino_b@amb@casino@games@shared@player@");
									PedHash[] array = (PedHash[])Enum.GetValues(typeof(PedHash));
									List<object> list4 = new List<object>();
									PedHash[] array2 = array;
									foreach (PedHash pedHash in array2)
									{
										list4.Add(pedHash);
									}
									list4.Remove(PedHash.ChickenHawk);
									list4.Remove(PedHash.Cow);
									list4.Remove(PedHash.Seagull);
									list4.Remove(PedHash.Boar);
									list4.Remove(PedHash.Chimp);
									list4.Remove(PedHash.Rottweiler);
									list4.Remove(PedHash.Poodle);
									list4.Remove(PedHash.KillerWhale);
									list4.Remove(PedHash.HammerShark);
									list4.Remove(PedHash.TigerShark);
									list4.Remove(PedHash.Dolphin);
									list4.Remove(PedHash.Chop);
									list4.Remove(PedHash.Husky);
									list4.Remove(PedHash.Humpback);
									list4.Remove(PedHash.MountainLion);
									list4.Remove(PedHash.Pig);
									list4.Remove(PedHash.Pigeon);
									list4.Remove(PedHash.Poodle);
									list4.Remove(PedHash.Pug);
									list4.Remove(PedHash.Rabbit);
									list4.Remove(PedHash.Rat);
									list4.Remove(PedHash.Retriever);
									list4.Remove(PedHash.Cat);
									list4.Remove(PedHash.Cow);
									list4.Remove(PedHash.Coyote);
									list4.Remove(PedHash.CrisFormage);
									list4.Remove(PedHash.CrisFormageCutscene);
									list4.Remove(PedHash.Deer);
									list4.Remove(PedHash.Dolphin);
									list4.Remove(PedHash.Fish);
									list4.Remove(PedHash.Humpback);
									list4.Remove(PedHash.Retriever);
									list4.Remove(PedHash.Shepherd);
									list4.Remove(PedHash.Stingray);
									list4.Remove(PedHash.Chimp);
									list4.Remove(PedHash.Chop);
									list4.Remove(PedHash.Crow);
									list4.Remove(PedHash.Stingray);
									list4.Remove(PedHash.Stingray);
									list4.Remove(PedHash.Stingray);
									Random random38 = new Random();
									Player_SkipTurn = false;
									AI1_SkipTurn = false;
									AI2_SkipTurn = false;
									AI3_SkipTurn = false;
									AI4_SkipTurn = false;
									AI5_SkipTurn = false;
									AI6_SkipTurn = false;
									AI7_SkipTurn = false;
									Player_Bust = false;
									AI1_Bust = false;
									AI2_Bust = false;
									AI3_Bust = false;
									AI4_Bust = false;
									AI5_Bust = false;
									AI6_Bust = false;
									AI7_Bust = false;
									Vector3 offsetInWorldCoords17 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-1.05f, -0.65f, 0.25f));
									World.DrawMarker(MarkerType.DebugSphere, offsetInWorldCoords17, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Blue);
									Vector3 offsetInWorldCoords18 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-0.4f, -1.2f, 0.25f));
									World.DrawMarker(MarkerType.DebugSphere, offsetInWorldCoords18, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Green);
									Vector3 offsetInWorldCoords19 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.4f, -1.2f, 0.25f));
									World.DrawMarker(MarkerType.DebugSphere, offsetInWorldCoords19, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Red);
									Vector3 offsetInWorldCoords20 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(1.05f, -0.7f, 0.25f));
									World.DrawMarker(MarkerType.DebugSphere, offsetInWorldCoords20, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Orange);
									Vector3 offsetInWorldCoords21 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(1.05f, 0.65f, 0.25f));
									World.DrawMarker(MarkerType.DebugSphere, offsetInWorldCoords21, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.White);
									Vector3 offsetInWorldCoords22 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(0.4f, 1.2f, 0.25f));
									World.DrawMarker(MarkerType.DebugSphere, offsetInWorldCoords22, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Gray);
									Vector3 offsetInWorldCoords23 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-0.4f, 1.2f, 0.25f));
									World.DrawMarker(MarkerType.DebugSphere, offsetInWorldCoords23, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Purple);
									Vector3 offsetInWorldCoords24 = _8PlayerPokerGameTable3.GetOffsetInWorldCoords(new Vector3(-1.05f, 0.65f, 0.25f));
									World.DrawMarker(MarkerType.DebugSphere, offsetInWorldCoords24, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Yellow);
									AI_1 = World.CreatePed(RequestModel((dynamic)list4[random38.Next(0, list4.Count)]), offsetInWorldCoords18, _8PlayerPokerGameTable3.Heading + 60f);
									AI_1.IsVisible = false;
									UI.Notify("Ai 1 " + AI_1.Position.ToString());
									AI_2 = World.CreatePed(RequestModel((dynamic)list4[random38.Next(0, list4.Count)]), offsetInWorldCoords19, _8PlayerPokerGameTable3.Heading + 100f);
									AI_2.IsVisible = false;
									AI_3 = World.CreatePed(RequestModel((dynamic)list4[random38.Next(0, list4.Count)]), offsetInWorldCoords20, _8PlayerPokerGameTable3.Heading + 170f);
									AI_3.IsVisible = false;
									AI_4 = World.CreatePed(RequestModel((dynamic)list4[random38.Next(0, list4.Count)]), offsetInWorldCoords21, _8PlayerPokerGameTable3.Heading + 200f);
									AI_4.IsVisible = false;
									AI_5 = World.CreatePed(RequestModel((dynamic)list4[random38.Next(0, list4.Count)]), offsetInWorldCoords22, _8PlayerPokerGameTable3.Heading + 260f);
									AI_5.IsVisible = false;
									AI_6 = World.CreatePed(RequestModel((dynamic)list4[random38.Next(0, list4.Count)]), offsetInWorldCoords23, _8PlayerPokerGameTable3.Heading + 300f);
									AI_6.IsVisible = false;
									AI_7 = World.CreatePed(RequestModel((dynamic)list4[random38.Next(0, list4.Count)]), offsetInWorldCoords24, _8PlayerPokerGameTable3.Heading + 325f);
									AI_7.IsVisible = false;
									AI_1.IsVisible = true;
									AI_2.IsVisible = true;
									AI_3.IsVisible = true;
									AI_4.IsVisible = true;
									AI_5.IsVisible = true;
									AI_6.IsVisible = true;
									AI_7.IsVisible = true;
									_8PlayerGamePokerPeds.Add(AI_2);
									_8PlayerGamePokerPeds.Add(AI_1);
									_8PlayerGamePokerPeds.Add(AI_3);
									_8PlayerGamePokerPeds.Add(AI_4);
									_8PlayerGamePokerPeds.Add(AI_5);
									_8PlayerGamePokerPeds.Add(AI_6);
									_8PlayerGamePokerPeds.Add(AI_7);
									foreach (Ped _8PlayerGamePokerPed5 in _8PlayerGamePokerPeds)
									{
										if (_8PlayerGamePokerPed5 != null)
										{
											_8PlayerGamePokerPed5.RelationshipGroup = 2;
											_8PlayerGamePokerPed5.AlwaysKeepTask = true;
											_8PlayerGamePokerPed5.BlockPermanentEvents = true;
										}
									}
									if (Function.Call<bool>(((Hash)0xD031A9162D01088CuL), new InputArgument[1] { text38 }))
									{
										Game.Player.Character.Position = offsetInWorldCoords16;
										Game.Player.Character.Heading = _8PlayerPokerGameTable3.Heading + 20f;
										Prop prop37 = _8PlayerPokerGameTable1;
										int num73 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
										{
											Game.Player.Character.Position.X,
											Game.Player.Character.Position.Y,
											prop37.Position.Z,
											0.0,
											0.0,
											prop37.Rotation.Z + 10f,
											2
										});
										Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
										{
											num73,
											Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { num73 })
										});
										string enterAnim = GetEnterAnim();
										Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
										{
											Game.Player.Character,
											num73,
											LoadDict(text38),
											"sit_enter_left",
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											Game.Player.Character,
											num73,
											"sit_enter_left",
											LoadDict(text38),
											16f,
											-16f,
											0,
											1148846080
										});
										enterAnim = GetEnterAnim();
										AI1_SyncScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
										{
											AI_1.Position.X,
											AI_1.Position.Y,
											prop37.Position.Z,
											0.0,
											0.0,
											AI_1.Heading,
											2
										});
										Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
										{
											AI_1,
											AI1_SyncScene,
											LoadDict(text38),
											"sit_enter_left_side",
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											AI_1,
											AI1_SyncScene,
											"sit_enter_left_side",
											LoadDict(text38),
											16f,
											-16f,
											0,
											1148846080
										});
										AI2_SyncScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
										{
											AI_2.Position.X,
											AI_2.Position.Y,
											prop37.Position.Z,
											0.0,
											0.0,
											AI_2.Heading,
											2
										});
										Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
										{
											AI_2,
											AI2_SyncScene,
											LoadDict(text38),
											"sit_enter_left_side",
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											AI_2,
											AI2_SyncScene,
											"sit_enter_left_side",
											LoadDict(text38),
											16f,
											-16f,
											0,
											1148846080
										});
										AI3_SyncScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
										{
											AI_3.Position.X,
											AI_3.Position.Y,
											prop37.Position.Z,
											0.0,
											0.0,
											AI_3.Heading,
											2
										});
										Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
										{
											AI_3,
											AI3_SyncScene,
											LoadDict(text38),
											"sit_enter_left_side",
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											AI_3,
											AI3_SyncScene,
											"sit_enter_left_side",
											LoadDict(text38),
											16f,
											-16f,
											0,
											1148846080
										});
										AI4_SyncScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
										{
											AI_4.Position.X,
											AI_4.Position.Y,
											prop37.Position.Z,
											0.0,
											0.0,
											AI_4.Heading,
											2
										});
										Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
										{
											AI_4,
											AI4_SyncScene,
											LoadDict(text38),
											"sit_enter_left_side",
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											AI_4,
											AI4_SyncScene,
											"sit_enter_left_side",
											LoadDict(text38),
											16f,
											-16f,
											0,
											1148846080
										});
										AI5_SyncScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
										{
											AI_5.Position.X,
											AI_5.Position.Y,
											prop37.Position.Z,
											0.0,
											0.0,
											AI_5.Heading,
											2
										});
										Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
										{
											AI_5,
											AI5_SyncScene,
											LoadDict(text38),
											"sit_enter_left_side",
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											AI_5,
											AI5_SyncScene,
											"sit_enter_left_side",
											LoadDict(text38),
											16f,
											-16f,
											0,
											1148846080
										});
										AI6_SyncScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
										{
											AI_6.Position.X,
											AI_6.Position.Y,
											prop37.Position.Z,
											0.0,
											0.0,
											AI_6.Heading,
											2
										});
										Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
										{
											AI_6,
											AI6_SyncScene,
											LoadDict(text38),
											"sit_enter_left_side",
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											AI_6,
											AI6_SyncScene,
											"sit_enter_left_side",
											LoadDict(text38),
											16f,
											-16f,
											0,
											1148846080
										});
										AI7_SyncScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
										{
											AI_7.Position.X,
											AI_7.Position.Y,
											prop37.Position.Z,
											0.0,
											0.0,
											AI_7.Heading,
											2
										});
										Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
										{
											AI_7,
											AI7_SyncScene,
											LoadDict(text38),
											"sit_enter_left_side",
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											AI_7,
											AI7_SyncScene,
											"sit_enter_left_side",
											LoadDict(text38),
											16f,
											-16f,
											0,
											1148846080
										});
										Script.Wait(1000);
										Player_SkipTurn = false;
										Player_Bust = false;
										AI1_SkipTurn = false;
										AI1_Bust = false;
										AI2_SkipTurn = false;
										AI2_Bust = false;
										AI3_SkipTurn = false;
										AI3_Bust = false;
										AI4_SkipTurn = false;
										AI4_Bust = false;
										AI5_SkipTurn = false;
										AI5_Bust = false;
										AI6_SkipTurn = false;
										AI6_Bust = false;
										AI7_SkipTurn = false;
										AI7_Bust = false;
										_8PlayerGameStage = 1;
									}
								}
								else
								{
									UI.Notify("Dealer : You need atleast ~g~" + _8PlayerGamePoker_PlayerStartingBank.ToString("N") + "c~w~ (Cash), to play 8 Player Poker!");
								}
							}
							else if (Playing8PlayerPoker)
							{
								Game.Player.Money += (int)_8PlayerGamePoker_PlayerBank;
								UI.Notify("Dealer : You have walked away with ~g~" + _8PlayerGamePoker_PlayerBank.ToString("N") + "c~w~ (Cash), from your recent 8 Player Poker Match!");
								StackChipHeight = 0.6f;
								StackChip = 0;
								string dict33 = "anim_casino_b@amb@casino@games@shared@player@";
								Playing8PlayerPoker = false;
								Prop prop38 = _8PlayerPokerGameTable1;
								int num74 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
								{
									Game.Player.Character.Position.X,
									Game.Player.Character.Position.Y,
									prop38.Position.Z,
									0.0,
									0.0,
									prop38.Rotation.Z,
									2
								});
								Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
								{
									Game.Player.Character,
									num74,
									LoadDict(dict33),
									"sit_exit_left",
									16f,
									-16f,
									0,
									1148846080
								});
								Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
								{
									num74,
									Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { num74 })
								});
								string exitAnim = GetExitAnim();
								exitAnim = GetExitAnim();
								PlaySyncExitAnim(AI_1, prop38, AI1_SyncScene, dict33, exitAnim);
								PlaySyncExitAnim(AI_2, prop38, AI2_SyncScene, dict33, exitAnim);
								PlaySyncExitAnim(AI_3, prop38, AI3_SyncScene, dict33, exitAnim);
								PlaySyncExitAnim(AI_4, prop38, AI4_SyncScene, dict33, exitAnim);
								PlaySyncExitAnim(AI_5, prop38, AI5_SyncScene, dict33, exitAnim);
								PlaySyncExitAnim(AI_6, prop38, AI6_SyncScene, dict33, exitAnim);
								PlaySyncExitAnim(AI_7, prop38, AI7_SyncScene, dict33, exitAnim);
								Script.Wait(4000);
								Game.Player.Character.Task.ClearAll();
								if (AI_1 != null)
								{
									AI_1.Task.WanderAround();
								}
								if (AI_2 != null)
								{
									AI_2.Task.WanderAround();
								}
								if (AI_3 != null)
								{
									AI_3.Task.WanderAround();
								}
								if (AI_4 != null)
								{
									AI_4.Task.WanderAround();
								}
								if (AI_5 != null)
								{
									AI_5.Task.WanderAround();
								}
								if (AI_6 != null)
								{
									AI_6.Task.WanderAround();
								}
								if (AI_7 != null)
								{
									AI_7.Task.WanderAround();
								}
								foreach (Ped _8PlayerGamePokerPed6 in _8PlayerGamePokerPeds)
								{
									if (_8PlayerGamePokerPed6 != null)
									{
										_8PlayerGamePokerPed6.Task.WanderAround();
									}
								}
							}
						}
						catch
						{
							Playing8PlayerPoker = false;
							UI.Notify("~r~ Error ~w~ An Error occurred when starting 8 Player Poker, please try again");
						}
					}
				}
				if (_8PlayerPokerGameTable5 != null)
				{
					Vector3 offsetInWorldCoords25 = _8PlayerPokerGameTable5.GetOffsetInWorldCoords(new Vector3(-1.05f, -0.65f, 0.25f));
					if (World.GetDistance(Game.Player.Character.Position, offsetInWorldCoords25) < 2.25f)
					{
						try
						{
							if (!Playing8PlayerPoker)
							{
								if (_8PlayerGamePoker_PlayerStartingBank <= Chips_Amount)
								{
									Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
									Game.Player.Money -= (int)_8PlayerGamePoker_PlayerStartingBank;
									StackChipHeight = 0.6f;
									StackChip = 0;
									Playing8PlayerPoker = true;
									if (ChipsSpawn != null)
									{
										ChipsSpawn.Delete();
									}
									if (AI_1 != null)
									{
										AI_1.Delete();
									}
									if (AI_2 != null)
									{
										AI_2.Delete();
									}
									if (AI_3 != null)
									{
										AI_3.Delete();
									}
									if (AI_4 != null)
									{
										AI_4.Delete();
									}
									if (AI_5 != null)
									{
										AI_5.Delete();
									}
									if (AI_6 != null)
									{
										AI_6.Delete();
									}
									if (AI_7 != null)
									{
										AI_7.Delete();
									}
									foreach (Ped _8PlayerGamePokerPed7 in _8PlayerGamePokerPeds)
									{
										if (_8PlayerGamePokerPed7 != null)
										{
											_8PlayerGamePokerPed7.Delete();
										}
									}
									if (_8PlayerGamePokerPeds.Count > 0)
									{
										_8PlayerGamePokerPeds.Clear();
									}
									foreach (Prop _8PlayerGamePokerProp5 in _8PlayerGamePokerProps)
									{
										if (_8PlayerGamePokerProp5 != null)
										{
											_8PlayerGamePokerProp5.Delete();
										}
									}
									foreach (Prop _8PlayerGamePokerPotChip5 in _8PlayerGamePokerPotChips)
									{
										if (_8PlayerGamePokerPotChip5 != null)
										{
											_8PlayerGamePokerPotChip5.Delete();
										}
									}
									if (PlayerCards.Count > 0)
									{
										PlayerCards.Clear();
									}
									if (PlayerChips.Count > 0)
									{
										PlayerChips.Clear();
									}
									_8PlayerGamePokerPot = 0f;
									if (_8PlayerGamePokerPotChips.Count > 0)
									{
										_8PlayerGamePokerPotChips.Clear();
									}
									if (_8PlayerGamePokerProps.Count > 0)
									{
										_8PlayerGamePokerProps.Clear();
									}
									_8PlayerGamePoker_PlayerStartingBank = 100000f;
									_8PlayerGamePoker_AI1StartingBank = 100000f;
									_8PlayerGamePoker_AI2StartingBank = 100000f;
									_8PlayerGamePoker_AI3StartingBank = 100000f;
									_8PlayerGamePoker_AI4StartingBank = 100000f;
									_8PlayerGamePoker_AI5StartingBank = 100000f;
									_8PlayerGamePoker_AI6StartingBank = 100000f;
									_8PlayerGamePoker_AI7StartingBank = 100000f;
									_8PlayerGamePoker_AI1Bank = _8PlayerGamePoker_AI1StartingBank;
									_8PlayerGamePoker_AI2Bank = _8PlayerGamePoker_AI2StartingBank;
									_8PlayerGamePoker_AI3Bank = _8PlayerGamePoker_AI3StartingBank;
									_8PlayerGamePoker_AI4Bank = _8PlayerGamePoker_AI4StartingBank;
									_8PlayerGamePoker_AI5Bank = _8PlayerGamePoker_AI5StartingBank;
									_8PlayerGamePoker_AI6Bank = _8PlayerGamePoker_AI6StartingBank;
									_8PlayerGamePoker_AI7Bank = _8PlayerGamePoker_AI7StartingBank;
									_8PlayerGamePoker_PlayerBank = _8PlayerGamePoker_PlayerStartingBank;
									Script.Wait(100);
									string text39 = "anim_casino_b@amb@casino@games@shared@player@";
									LoadDict("anim_casino_b@amb@casino@games@shared@player@");
									PedHash[] array3 = (PedHash[])Enum.GetValues(typeof(PedHash));
									List<object> list5 = new List<object>();
									PedHash[] array4 = array3;
									foreach (PedHash pedHash2 in array4)
									{
										list5.Add(pedHash2);
									}
									list5.Remove(PedHash.ChickenHawk);
									list5.Remove(PedHash.Cow);
									list5.Remove(PedHash.Seagull);
									list5.Remove(PedHash.Boar);
									list5.Remove(PedHash.Chimp);
									list5.Remove(PedHash.Rottweiler);
									list5.Remove(PedHash.Poodle);
									list5.Remove(PedHash.KillerWhale);
									list5.Remove(PedHash.HammerShark);
									list5.Remove(PedHash.TigerShark);
									list5.Remove(PedHash.Dolphin);
									list5.Remove(PedHash.Chop);
									list5.Remove(PedHash.Husky);
									list5.Remove(PedHash.Humpback);
									list5.Remove(PedHash.MountainLion);
									list5.Remove(PedHash.Pig);
									list5.Remove(PedHash.Pigeon);
									list5.Remove(PedHash.Poodle);
									list5.Remove(PedHash.Pug);
									list5.Remove(PedHash.Rabbit);
									list5.Remove(PedHash.Rat);
									list5.Remove(PedHash.Retriever);
									list5.Remove(PedHash.Cat);
									list5.Remove(PedHash.Cow);
									list5.Remove(PedHash.Coyote);
									list5.Remove(PedHash.CrisFormage);
									list5.Remove(PedHash.CrisFormageCutscene);
									list5.Remove(PedHash.Deer);
									list5.Remove(PedHash.Dolphin);
									list5.Remove(PedHash.Fish);
									list5.Remove(PedHash.Humpback);
									list5.Remove(PedHash.Retriever);
									list5.Remove(PedHash.Shepherd);
									list5.Remove(PedHash.Stingray);
									list5.Remove(PedHash.Chimp);
									list5.Remove(PedHash.Chop);
									list5.Remove(PedHash.Crow);
									list5.Remove(PedHash.Stingray);
									list5.Remove(PedHash.Stingray);
									list5.Remove(PedHash.Stingray);
									Random random39 = new Random();
									Player_SkipTurn = false;
									AI1_SkipTurn = false;
									AI2_SkipTurn = false;
									AI3_SkipTurn = false;
									AI4_SkipTurn = false;
									AI5_SkipTurn = false;
									AI6_SkipTurn = false;
									AI7_SkipTurn = false;
									Player_Bust = false;
									AI1_Bust = false;
									AI2_Bust = false;
									AI3_Bust = false;
									AI4_Bust = false;
									AI5_Bust = false;
									AI6_Bust = false;
									AI7_Bust = false;
									Vector3 offsetInWorldCoords26 = _8PlayerPokerGameTable5.GetOffsetInWorldCoords(new Vector3(-1.05f, -0.65f, 0.25f));
									World.DrawMarker(MarkerType.DebugSphere, offsetInWorldCoords26, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Blue);
									Vector3 offsetInWorldCoords27 = _8PlayerPokerGameTable5.GetOffsetInWorldCoords(new Vector3(-0.4f, -1.2f, 0.25f));
									World.DrawMarker(MarkerType.DebugSphere, offsetInWorldCoords27, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Green);
									Vector3 offsetInWorldCoords28 = _8PlayerPokerGameTable5.GetOffsetInWorldCoords(new Vector3(0.4f, -1.2f, 0.25f));
									World.DrawMarker(MarkerType.DebugSphere, offsetInWorldCoords28, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Red);
									Vector3 offsetInWorldCoords29 = _8PlayerPokerGameTable5.GetOffsetInWorldCoords(new Vector3(1.05f, -0.7f, 0.25f));
									World.DrawMarker(MarkerType.DebugSphere, offsetInWorldCoords29, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Orange);
									Vector3 offsetInWorldCoords30 = _8PlayerPokerGameTable5.GetOffsetInWorldCoords(new Vector3(1.05f, 0.65f, 0.25f));
									World.DrawMarker(MarkerType.DebugSphere, offsetInWorldCoords30, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.White);
									Vector3 offsetInWorldCoords31 = _8PlayerPokerGameTable5.GetOffsetInWorldCoords(new Vector3(0.4f, 1.2f, 0.25f));
									World.DrawMarker(MarkerType.DebugSphere, offsetInWorldCoords31, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Gray);
									Vector3 offsetInWorldCoords32 = _8PlayerPokerGameTable5.GetOffsetInWorldCoords(new Vector3(-0.4f, 1.2f, 0.25f));
									World.DrawMarker(MarkerType.DebugSphere, offsetInWorldCoords32, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Purple);
									Vector3 offsetInWorldCoords33 = _8PlayerPokerGameTable5.GetOffsetInWorldCoords(new Vector3(-1.05f, 0.65f, 0.25f));
									World.DrawMarker(MarkerType.DebugSphere, offsetInWorldCoords33, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Yellow);
									AI_1 = World.CreatePed(RequestModel((dynamic)list5[random39.Next(0, list5.Count)]), offsetInWorldCoords27, _8PlayerPokerGameTable5.Heading + 60f);
									AI_1.IsVisible = false;
									UI.Notify("Ai 1 " + AI_1.Position.ToString());
									AI_2 = World.CreatePed(RequestModel((dynamic)list5[random39.Next(0, list5.Count)]), offsetInWorldCoords28, _8PlayerPokerGameTable5.Heading + 100f);
									AI_2.IsVisible = false;
									AI_3 = World.CreatePed(RequestModel((dynamic)list5[random39.Next(0, list5.Count)]), offsetInWorldCoords29, _8PlayerPokerGameTable5.Heading + 170f);
									AI_3.IsVisible = false;
									AI_4 = World.CreatePed(RequestModel((dynamic)list5[random39.Next(0, list5.Count)]), offsetInWorldCoords30, _8PlayerPokerGameTable5.Heading + 200f);
									AI_4.IsVisible = false;
									AI_5 = World.CreatePed(RequestModel((dynamic)list5[random39.Next(0, list5.Count)]), offsetInWorldCoords31, _8PlayerPokerGameTable5.Heading + 260f);
									AI_5.IsVisible = false;
									AI_6 = World.CreatePed(RequestModel((dynamic)list5[random39.Next(0, list5.Count)]), offsetInWorldCoords32, _8PlayerPokerGameTable5.Heading + 300f);
									AI_6.IsVisible = false;
									AI_7 = World.CreatePed(RequestModel((dynamic)list5[random39.Next(0, list5.Count)]), offsetInWorldCoords33, _8PlayerPokerGameTable5.Heading + 325f);
									AI_7.IsVisible = false;
									AI_1.IsVisible = true;
									AI_2.IsVisible = true;
									AI_3.IsVisible = true;
									AI_4.IsVisible = true;
									AI_5.IsVisible = true;
									AI_6.IsVisible = true;
									AI_7.IsVisible = true;
									_8PlayerGamePokerPeds.Add(AI_2);
									_8PlayerGamePokerPeds.Add(AI_1);
									_8PlayerGamePokerPeds.Add(AI_3);
									_8PlayerGamePokerPeds.Add(AI_4);
									_8PlayerGamePokerPeds.Add(AI_5);
									_8PlayerGamePokerPeds.Add(AI_6);
									_8PlayerGamePokerPeds.Add(AI_7);
									foreach (Ped _8PlayerGamePokerPed8 in _8PlayerGamePokerPeds)
									{
										if (_8PlayerGamePokerPed8 != null)
										{
											_8PlayerGamePokerPed8.RelationshipGroup = 2;
											_8PlayerGamePokerPed8.AlwaysKeepTask = true;
											_8PlayerGamePokerPed8.BlockPermanentEvents = true;
										}
									}
									if (Function.Call<bool>(((Hash)0xD031A9162D01088CuL), new InputArgument[1] { text39 }))
									{
										Game.Player.Character.Position = offsetInWorldCoords25;
										Game.Player.Character.Heading = _8PlayerPokerGameTable5.Heading + 20f;
										Prop prop39 = _8PlayerPokerGameTable1;
										int num75 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
										{
											Game.Player.Character.Position.X,
											Game.Player.Character.Position.Y,
											prop39.Position.Z,
											0.0,
											0.0,
											prop39.Rotation.Z + 10f,
											2
										});
										Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
										{
											num75,
											Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { num75 })
										});
										string enterAnim2 = GetEnterAnim();
										Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
										{
											Game.Player.Character,
											num75,
											LoadDict(text39),
											"sit_enter_left",
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											Game.Player.Character,
											num75,
											"sit_enter_left",
											LoadDict(text39),
											16f,
											-16f,
											0,
											1148846080
										});
										enterAnim2 = GetEnterAnim();
										AI1_SyncScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
										{
											AI_1.Position.X,
											AI_1.Position.Y,
											prop39.Position.Z,
											0.0,
											0.0,
											AI_1.Heading,
											2
										});
										Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
										{
											AI_1,
											AI1_SyncScene,
											LoadDict(text39),
											"sit_enter_left_side",
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											AI_1,
											AI1_SyncScene,
											"sit_enter_left_side",
											LoadDict(text39),
											16f,
											-16f,
											0,
											1148846080
										});
										AI2_SyncScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
										{
											AI_2.Position.X,
											AI_2.Position.Y,
											prop39.Position.Z,
											0.0,
											0.0,
											AI_2.Heading,
											2
										});
										Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
										{
											AI_2,
											AI2_SyncScene,
											LoadDict(text39),
											"sit_enter_left_side",
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											AI_2,
											AI2_SyncScene,
											"sit_enter_left_side",
											LoadDict(text39),
											16f,
											-16f,
											0,
											1148846080
										});
										AI3_SyncScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
										{
											AI_3.Position.X,
											AI_3.Position.Y,
											prop39.Position.Z,
											0.0,
											0.0,
											AI_3.Heading,
											2
										});
										Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
										{
											AI_3,
											AI3_SyncScene,
											LoadDict(text39),
											"sit_enter_left_side",
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											AI_3,
											AI3_SyncScene,
											"sit_enter_left_side",
											LoadDict(text39),
											16f,
											-16f,
											0,
											1148846080
										});
										AI4_SyncScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
										{
											AI_4.Position.X,
											AI_4.Position.Y,
											prop39.Position.Z,
											0.0,
											0.0,
											AI_4.Heading,
											2
										});
										Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
										{
											AI_4,
											AI4_SyncScene,
											LoadDict(text39),
											"sit_enter_left_side",
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											AI_4,
											AI4_SyncScene,
											"sit_enter_left_side",
											LoadDict(text39),
											16f,
											-16f,
											0,
											1148846080
										});
										AI5_SyncScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
										{
											AI_5.Position.X,
											AI_5.Position.Y,
											prop39.Position.Z,
											0.0,
											0.0,
											AI_5.Heading,
											2
										});
										Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
										{
											AI_5,
											AI5_SyncScene,
											LoadDict(text39),
											"sit_enter_left_side",
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											AI_5,
											AI5_SyncScene,
											"sit_enter_left_side",
											LoadDict(text39),
											16f,
											-16f,
											0,
											1148846080
										});
										AI6_SyncScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
										{
											AI_6.Position.X,
											AI_6.Position.Y,
											prop39.Position.Z,
											0.0,
											0.0,
											AI_6.Heading,
											2
										});
										Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
										{
											AI_6,
											AI6_SyncScene,
											LoadDict(text39),
											"sit_enter_left_side",
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											AI_6,
											AI6_SyncScene,
											"sit_enter_left_side",
											LoadDict(text39),
											16f,
											-16f,
											0,
											1148846080
										});
										AI7_SyncScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
										{
											AI_7.Position.X,
											AI_7.Position.Y,
											prop39.Position.Z,
											0.0,
											0.0,
											AI_7.Heading,
											2
										});
										Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
										{
											AI_7,
											AI7_SyncScene,
											LoadDict(text39),
											"sit_enter_left_side",
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											AI_7,
											AI7_SyncScene,
											"sit_enter_left_side",
											LoadDict(text39),
											16f,
											-16f,
											0,
											1148846080
										});
										Script.Wait(1000);
										Player_SkipTurn = false;
										Player_Bust = false;
										AI1_SkipTurn = false;
										AI1_Bust = false;
										AI2_SkipTurn = false;
										AI2_Bust = false;
										AI3_SkipTurn = false;
										AI3_Bust = false;
										AI4_SkipTurn = false;
										AI4_Bust = false;
										AI5_SkipTurn = false;
										AI5_Bust = false;
										AI6_SkipTurn = false;
										AI6_Bust = false;
										AI7_SkipTurn = false;
										AI7_Bust = false;
										_8PlayerGameStage = 1;
									}
								}
								else
								{
									UI.Notify("Dealer : You need atleast ~g~" + _8PlayerGamePoker_PlayerStartingBank.ToString("N") + "c~w~ (Cash), to play 8 Player Poker!");
								}
							}
							else if (Playing8PlayerPoker)
							{
								Game.Player.Money += (int)_8PlayerGamePoker_PlayerBank;
								UI.Notify("Dealer : You have walked away with ~g~" + _8PlayerGamePoker_PlayerBank.ToString("N") + "c~w~ (Cash), from your recent 8 Player Poker Match!");
								StackChipHeight = 0.6f;
								StackChip = 0;
								string dict34 = "anim_casino_b@amb@casino@games@shared@player@";
								Playing8PlayerPoker = false;
								Prop prop40 = _8PlayerPokerGameTable1;
								int num76 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
								{
									Game.Player.Character.Position.X,
									Game.Player.Character.Position.Y,
									prop40.Position.Z,
									0.0,
									0.0,
									prop40.Rotation.Z,
									2
								});
								Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
								{
									Game.Player.Character,
									num76,
									LoadDict(dict34),
									"sit_exit_left",
									16f,
									-16f,
									0,
									1148846080
								});
								Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
								{
									num76,
									Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { num76 })
								});
								string exitAnim2 = GetExitAnim();
								exitAnim2 = GetExitAnim();
								PlaySyncExitAnim(AI_1, prop40, AI1_SyncScene, dict34, exitAnim2);
								PlaySyncExitAnim(AI_2, prop40, AI2_SyncScene, dict34, exitAnim2);
								PlaySyncExitAnim(AI_3, prop40, AI3_SyncScene, dict34, exitAnim2);
								PlaySyncExitAnim(AI_4, prop40, AI4_SyncScene, dict34, exitAnim2);
								PlaySyncExitAnim(AI_5, prop40, AI5_SyncScene, dict34, exitAnim2);
								PlaySyncExitAnim(AI_6, prop40, AI6_SyncScene, dict34, exitAnim2);
								PlaySyncExitAnim(AI_7, prop40, AI7_SyncScene, dict34, exitAnim2);
								Script.Wait(4000);
								Game.Player.Character.Task.ClearAll();
								if (AI_1 != null)
								{
									AI_1.Task.WanderAround();
								}
								if (AI_2 != null)
								{
									AI_2.Task.WanderAround();
								}
								if (AI_3 != null)
								{
									AI_3.Task.WanderAround();
								}
								if (AI_4 != null)
								{
									AI_4.Task.WanderAround();
								}
								if (AI_5 != null)
								{
									AI_5.Task.WanderAround();
								}
								if (AI_6 != null)
								{
									AI_6.Task.WanderAround();
								}
								if (AI_7 != null)
								{
									AI_7.Task.WanderAround();
								}
								foreach (Ped _8PlayerGamePokerPed9 in _8PlayerGamePokerPeds)
								{
									if (_8PlayerGamePokerPed9 != null)
									{
										_8PlayerGamePokerPed9.Task.WanderAround();
									}
								}
							}
						}
						catch
						{
							Playing8PlayerPoker = false;
							UI.Notify("~r~ Error ~w~ An Error occurred when starting 8 Player Poker, please try again");
						}
					}
				}
				if (_8PlayerPokerGameTable6 != null)
				{
					Vector3 offsetInWorldCoords34 = _8PlayerPokerGameTable6.GetOffsetInWorldCoords(new Vector3(-1.05f, -0.65f, 0.25f));
					if (World.GetDistance(Game.Player.Character.Position, offsetInWorldCoords34) < 2.25f)
					{
						try
						{
							if (!Playing8PlayerPoker)
							{
								if (_8PlayerGamePoker_PlayerStartingBank <= Chips_Amount)
								{
									Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
									Game.Player.Money -= (int)_8PlayerGamePoker_PlayerStartingBank;
									StackChipHeight = 0.6f;
									StackChip = 0;
									Playing8PlayerPoker = true;
									if (ChipsSpawn != null)
									{
										ChipsSpawn.Delete();
									}
									if (AI_1 != null)
									{
										AI_1.Delete();
									}
									if (AI_2 != null)
									{
										AI_2.Delete();
									}
									if (AI_3 != null)
									{
										AI_3.Delete();
									}
									if (AI_4 != null)
									{
										AI_4.Delete();
									}
									if (AI_5 != null)
									{
										AI_5.Delete();
									}
									if (AI_6 != null)
									{
										AI_6.Delete();
									}
									if (AI_7 != null)
									{
										AI_7.Delete();
									}
									foreach (Ped _8PlayerGamePokerPed10 in _8PlayerGamePokerPeds)
									{
										if (_8PlayerGamePokerPed10 != null)
										{
											_8PlayerGamePokerPed10.Delete();
										}
									}
									if (_8PlayerGamePokerPeds.Count > 0)
									{
										_8PlayerGamePokerPeds.Clear();
									}
									foreach (Prop _8PlayerGamePokerProp6 in _8PlayerGamePokerProps)
									{
										if (_8PlayerGamePokerProp6 != null)
										{
											_8PlayerGamePokerProp6.Delete();
										}
									}
									foreach (Prop _8PlayerGamePokerPotChip6 in _8PlayerGamePokerPotChips)
									{
										if (_8PlayerGamePokerPotChip6 != null)
										{
											_8PlayerGamePokerPotChip6.Delete();
										}
									}
									if (PlayerCards.Count > 0)
									{
										PlayerCards.Clear();
									}
									if (PlayerChips.Count > 0)
									{
										PlayerChips.Clear();
									}
									_8PlayerGamePokerPot = 0f;
									if (_8PlayerGamePokerPotChips.Count > 0)
									{
										_8PlayerGamePokerPotChips.Clear();
									}
									if (_8PlayerGamePokerProps.Count > 0)
									{
										_8PlayerGamePokerProps.Clear();
									}
									_8PlayerGamePoker_PlayerStartingBank = 100000f;
									_8PlayerGamePoker_AI1StartingBank = 100000f;
									_8PlayerGamePoker_AI2StartingBank = 100000f;
									_8PlayerGamePoker_AI3StartingBank = 100000f;
									_8PlayerGamePoker_AI4StartingBank = 100000f;
									_8PlayerGamePoker_AI5StartingBank = 100000f;
									_8PlayerGamePoker_AI6StartingBank = 100000f;
									_8PlayerGamePoker_AI7StartingBank = 100000f;
									_8PlayerGamePoker_AI1Bank = _8PlayerGamePoker_AI1StartingBank;
									_8PlayerGamePoker_AI2Bank = _8PlayerGamePoker_AI2StartingBank;
									_8PlayerGamePoker_AI3Bank = _8PlayerGamePoker_AI3StartingBank;
									_8PlayerGamePoker_AI4Bank = _8PlayerGamePoker_AI4StartingBank;
									_8PlayerGamePoker_AI5Bank = _8PlayerGamePoker_AI5StartingBank;
									_8PlayerGamePoker_AI6Bank = _8PlayerGamePoker_AI6StartingBank;
									_8PlayerGamePoker_AI7Bank = _8PlayerGamePoker_AI7StartingBank;
									_8PlayerGamePoker_PlayerBank = _8PlayerGamePoker_PlayerStartingBank;
									Script.Wait(100);
									string text40 = "anim_casino_b@amb@casino@games@shared@player@";
									LoadDict("anim_casino_b@amb@casino@games@shared@player@");
									PedHash[] array5 = (PedHash[])Enum.GetValues(typeof(PedHash));
									List<object> list6 = new List<object>();
									PedHash[] array6 = array5;
									foreach (PedHash pedHash3 in array6)
									{
										list6.Add(pedHash3);
									}
									list6.Remove(PedHash.ChickenHawk);
									list6.Remove(PedHash.Cow);
									list6.Remove(PedHash.Seagull);
									list6.Remove(PedHash.Boar);
									list6.Remove(PedHash.Chimp);
									list6.Remove(PedHash.Rottweiler);
									list6.Remove(PedHash.Poodle);
									list6.Remove(PedHash.KillerWhale);
									list6.Remove(PedHash.HammerShark);
									list6.Remove(PedHash.TigerShark);
									list6.Remove(PedHash.Dolphin);
									list6.Remove(PedHash.Chop);
									list6.Remove(PedHash.Husky);
									list6.Remove(PedHash.Humpback);
									list6.Remove(PedHash.MountainLion);
									list6.Remove(PedHash.Pig);
									list6.Remove(PedHash.Pigeon);
									list6.Remove(PedHash.Poodle);
									list6.Remove(PedHash.Pug);
									list6.Remove(PedHash.Rabbit);
									list6.Remove(PedHash.Rat);
									list6.Remove(PedHash.Retriever);
									list6.Remove(PedHash.Cat);
									list6.Remove(PedHash.Cow);
									list6.Remove(PedHash.Coyote);
									list6.Remove(PedHash.CrisFormage);
									list6.Remove(PedHash.CrisFormageCutscene);
									list6.Remove(PedHash.Deer);
									list6.Remove(PedHash.Dolphin);
									list6.Remove(PedHash.Fish);
									list6.Remove(PedHash.Humpback);
									list6.Remove(PedHash.Retriever);
									list6.Remove(PedHash.Shepherd);
									list6.Remove(PedHash.Stingray);
									list6.Remove(PedHash.Chimp);
									list6.Remove(PedHash.Chop);
									list6.Remove(PedHash.Crow);
									list6.Remove(PedHash.Stingray);
									list6.Remove(PedHash.Stingray);
									list6.Remove(PedHash.Stingray);
									Random random40 = new Random();
									Player_SkipTurn = false;
									AI1_SkipTurn = false;
									AI2_SkipTurn = false;
									AI3_SkipTurn = false;
									AI4_SkipTurn = false;
									AI5_SkipTurn = false;
									AI6_SkipTurn = false;
									AI7_SkipTurn = false;
									Player_Bust = false;
									AI1_Bust = false;
									AI2_Bust = false;
									AI3_Bust = false;
									AI4_Bust = false;
									AI5_Bust = false;
									AI6_Bust = false;
									AI7_Bust = false;
									Vector3 offsetInWorldCoords35 = _8PlayerPokerGameTable6.GetOffsetInWorldCoords(new Vector3(-1.05f, -0.65f, 0.25f));
									World.DrawMarker(MarkerType.DebugSphere, offsetInWorldCoords35, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Blue);
									Vector3 offsetInWorldCoords36 = _8PlayerPokerGameTable6.GetOffsetInWorldCoords(new Vector3(-0.4f, -1.2f, 0.25f));
									World.DrawMarker(MarkerType.DebugSphere, offsetInWorldCoords36, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Green);
									Vector3 offsetInWorldCoords37 = _8PlayerPokerGameTable6.GetOffsetInWorldCoords(new Vector3(0.4f, -1.2f, 0.25f));
									World.DrawMarker(MarkerType.DebugSphere, offsetInWorldCoords37, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Red);
									Vector3 offsetInWorldCoords38 = _8PlayerPokerGameTable6.GetOffsetInWorldCoords(new Vector3(1.05f, -0.7f, 0.25f));
									World.DrawMarker(MarkerType.DebugSphere, offsetInWorldCoords38, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Orange);
									Vector3 offsetInWorldCoords39 = _8PlayerPokerGameTable6.GetOffsetInWorldCoords(new Vector3(1.05f, 0.65f, 0.25f));
									World.DrawMarker(MarkerType.DebugSphere, offsetInWorldCoords39, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.White);
									Vector3 offsetInWorldCoords40 = _8PlayerPokerGameTable6.GetOffsetInWorldCoords(new Vector3(0.4f, 1.2f, 0.25f));
									World.DrawMarker(MarkerType.DebugSphere, offsetInWorldCoords40, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Gray);
									Vector3 offsetInWorldCoords41 = _8PlayerPokerGameTable6.GetOffsetInWorldCoords(new Vector3(-0.4f, 1.2f, 0.25f));
									World.DrawMarker(MarkerType.DebugSphere, offsetInWorldCoords41, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Purple);
									Vector3 offsetInWorldCoords42 = _8PlayerPokerGameTable6.GetOffsetInWorldCoords(new Vector3(-1.05f, 0.65f, 0.25f));
									World.DrawMarker(MarkerType.DebugSphere, offsetInWorldCoords42, Vector3.Zero, Vector3.Zero, new Vector3(0.15f, 0.15f, 0.15f), Color.Yellow);
									AI_1 = World.CreatePed(RequestModel((dynamic)list6[random40.Next(0, list6.Count)]), offsetInWorldCoords36, _8PlayerPokerGameTable6.Heading + 60f);
									AI_1.IsVisible = false;
									UI.Notify("Ai 1 " + AI_1.Position.ToString());
									AI_2 = World.CreatePed(RequestModel((dynamic)list6[random40.Next(0, list6.Count)]), offsetInWorldCoords37, _8PlayerPokerGameTable6.Heading + 100f);
									AI_2.IsVisible = false;
									AI_3 = World.CreatePed(RequestModel((dynamic)list6[random40.Next(0, list6.Count)]), offsetInWorldCoords38, _8PlayerPokerGameTable6.Heading + 170f);
									AI_3.IsVisible = false;
									AI_4 = World.CreatePed(RequestModel((dynamic)list6[random40.Next(0, list6.Count)]), offsetInWorldCoords39, _8PlayerPokerGameTable6.Heading + 200f);
									AI_4.IsVisible = false;
									AI_5 = World.CreatePed(RequestModel((dynamic)list6[random40.Next(0, list6.Count)]), offsetInWorldCoords40, _8PlayerPokerGameTable6.Heading + 260f);
									AI_5.IsVisible = false;
									AI_6 = World.CreatePed(RequestModel((dynamic)list6[random40.Next(0, list6.Count)]), offsetInWorldCoords41, _8PlayerPokerGameTable6.Heading + 300f);
									AI_6.IsVisible = false;
									AI_7 = World.CreatePed(RequestModel((dynamic)list6[random40.Next(0, list6.Count)]), offsetInWorldCoords42, _8PlayerPokerGameTable6.Heading + 325f);
									AI_7.IsVisible = false;
									AI_1.IsVisible = true;
									AI_2.IsVisible = true;
									AI_3.IsVisible = true;
									AI_4.IsVisible = true;
									AI_5.IsVisible = true;
									AI_6.IsVisible = true;
									AI_7.IsVisible = true;
									_8PlayerGamePokerPeds.Add(AI_2);
									_8PlayerGamePokerPeds.Add(AI_1);
									_8PlayerGamePokerPeds.Add(AI_3);
									_8PlayerGamePokerPeds.Add(AI_4);
									_8PlayerGamePokerPeds.Add(AI_5);
									_8PlayerGamePokerPeds.Add(AI_6);
									_8PlayerGamePokerPeds.Add(AI_7);
									foreach (Ped _8PlayerGamePokerPed11 in _8PlayerGamePokerPeds)
									{
										if (_8PlayerGamePokerPed11 != null)
										{
											_8PlayerGamePokerPed11.RelationshipGroup = 2;
											_8PlayerGamePokerPed11.AlwaysKeepTask = true;
											_8PlayerGamePokerPed11.BlockPermanentEvents = true;
										}
									}
									if (Function.Call<bool>(((Hash)0xD031A9162D01088CuL), new InputArgument[1] { text40 }))
									{
										Game.Player.Character.Position = offsetInWorldCoords34;
										Game.Player.Character.Heading = _8PlayerPokerGameTable6.Heading + 20f;
										Prop prop41 = _8PlayerPokerGameTable1;
										int num77 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
										{
											Game.Player.Character.Position.X,
											Game.Player.Character.Position.Y,
											prop41.Position.Z,
											0.0,
											0.0,
											prop41.Rotation.Z + 10f,
											2
										});
										Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
										{
											num77,
											Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { num77 })
										});
										string enterAnim3 = GetEnterAnim();
										Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
										{
											Game.Player.Character,
											num77,
											LoadDict(text40),
											"sit_enter_left",
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											Game.Player.Character,
											num77,
											"sit_enter_left",
											LoadDict(text40),
											16f,
											-16f,
											0,
											1148846080
										});
										enterAnim3 = GetEnterAnim();
										AI1_SyncScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
										{
											AI_1.Position.X,
											AI_1.Position.Y,
											prop41.Position.Z,
											0.0,
											0.0,
											AI_1.Heading,
											2
										});
										Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
										{
											AI_1,
											AI1_SyncScene,
											LoadDict(text40),
											"sit_enter_left_side",
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											AI_1,
											AI1_SyncScene,
											"sit_enter_left_side",
											LoadDict(text40),
											16f,
											-16f,
											0,
											1148846080
										});
										AI2_SyncScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
										{
											AI_2.Position.X,
											AI_2.Position.Y,
											prop41.Position.Z,
											0.0,
											0.0,
											AI_2.Heading,
											2
										});
										Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
										{
											AI_2,
											AI2_SyncScene,
											LoadDict(text40),
											"sit_enter_left_side",
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											AI_2,
											AI2_SyncScene,
											"sit_enter_left_side",
											LoadDict(text40),
											16f,
											-16f,
											0,
											1148846080
										});
										AI3_SyncScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
										{
											AI_3.Position.X,
											AI_3.Position.Y,
											prop41.Position.Z,
											0.0,
											0.0,
											AI_3.Heading,
											2
										});
										Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
										{
											AI_3,
											AI3_SyncScene,
											LoadDict(text40),
											"sit_enter_left_side",
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											AI_3,
											AI3_SyncScene,
											"sit_enter_left_side",
											LoadDict(text40),
											16f,
											-16f,
											0,
											1148846080
										});
										AI4_SyncScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
										{
											AI_4.Position.X,
											AI_4.Position.Y,
											prop41.Position.Z,
											0.0,
											0.0,
											AI_4.Heading,
											2
										});
										Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
										{
											AI_4,
											AI4_SyncScene,
											LoadDict(text40),
											"sit_enter_left_side",
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											AI_4,
											AI4_SyncScene,
											"sit_enter_left_side",
											LoadDict(text40),
											16f,
											-16f,
											0,
											1148846080
										});
										AI5_SyncScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
										{
											AI_5.Position.X,
											AI_5.Position.Y,
											prop41.Position.Z,
											0.0,
											0.0,
											AI_5.Heading,
											2
										});
										Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
										{
											AI_5,
											AI5_SyncScene,
											LoadDict(text40),
											"sit_enter_left_side",
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											AI_5,
											AI5_SyncScene,
											"sit_enter_left_side",
											LoadDict(text40),
											16f,
											-16f,
											0,
											1148846080
										});
										AI6_SyncScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
										{
											AI_6.Position.X,
											AI_6.Position.Y,
											prop41.Position.Z,
											0.0,
											0.0,
											AI_6.Heading,
											2
										});
										Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
										{
											AI_6,
											AI6_SyncScene,
											LoadDict(text40),
											"sit_enter_left_side",
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											AI_6,
											AI6_SyncScene,
											"sit_enter_left_side",
											LoadDict(text40),
											16f,
											-16f,
											0,
											1148846080
										});
										AI7_SyncScene = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
										{
											AI_7.Position.X,
											AI_7.Position.Y,
											prop41.Position.Z,
											0.0,
											0.0,
											AI_7.Heading,
											2
										});
										Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
										{
											AI_7,
											AI7_SyncScene,
											LoadDict(text40),
											"sit_enter_left_side",
											16f,
											-16f,
											0,
											1148846080
										});
										Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
										{
											AI_7,
											AI7_SyncScene,
											"sit_enter_left_side",
											LoadDict(text40),
											16f,
											-16f,
											0,
											1148846080
										});
										Script.Wait(1000);
										Player_SkipTurn = false;
										Player_Bust = false;
										AI1_SkipTurn = false;
										AI1_Bust = false;
										AI2_SkipTurn = false;
										AI2_Bust = false;
										AI3_SkipTurn = false;
										AI3_Bust = false;
										AI4_SkipTurn = false;
										AI4_Bust = false;
										AI5_SkipTurn = false;
										AI5_Bust = false;
										AI6_SkipTurn = false;
										AI6_Bust = false;
										AI7_SkipTurn = false;
										AI7_Bust = false;
										_8PlayerGameStage = 1;
									}
								}
								else
								{
									UI.Notify("Dealer : You need atleast ~g~" + _8PlayerGamePoker_PlayerStartingBank.ToString("N") + "c~w~ (Cash), to play 8 Player Poker!");
								}
							}
							else if (Playing8PlayerPoker)
							{
								Game.Player.Money += (int)_8PlayerGamePoker_PlayerBank;
								UI.Notify("Dealer : You have walked away with ~g~" + _8PlayerGamePoker_PlayerBank.ToString("N") + "c~w~ (Cash), from your recent 8 Player Poker Match!");
								StackChipHeight = 0.6f;
								StackChip = 0;
								string dict35 = "anim_casino_b@amb@casino@games@shared@player@";
								Playing8PlayerPoker = false;
								Prop prop42 = _8PlayerPokerGameTable1;
								int num78 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
								{
									Game.Player.Character.Position.X,
									Game.Player.Character.Position.Y,
									prop42.Position.Z,
									0.0,
									0.0,
									prop42.Rotation.Z,
									2
								});
								Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
								{
									Game.Player.Character,
									num78,
									LoadDict(dict35),
									"sit_exit_left",
									16f,
									-16f,
									0,
									1148846080
								});
								Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
								{
									num78,
									Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { num78 })
								});
								string exitAnim3 = GetExitAnim();
								exitAnim3 = GetExitAnim();
								PlaySyncExitAnim(AI_1, prop42, AI1_SyncScene, dict35, exitAnim3);
								PlaySyncExitAnim(AI_2, prop42, AI2_SyncScene, dict35, exitAnim3);
								PlaySyncExitAnim(AI_3, prop42, AI3_SyncScene, dict35, exitAnim3);
								PlaySyncExitAnim(AI_4, prop42, AI4_SyncScene, dict35, exitAnim3);
								PlaySyncExitAnim(AI_5, prop42, AI5_SyncScene, dict35, exitAnim3);
								PlaySyncExitAnim(AI_6, prop42, AI6_SyncScene, dict35, exitAnim3);
								PlaySyncExitAnim(AI_7, prop42, AI7_SyncScene, dict35, exitAnim3);
								Script.Wait(4000);
								Game.Player.Character.Task.ClearAll();
								if (AI_1 != null)
								{
									AI_1.Task.WanderAround();
								}
								if (AI_2 != null)
								{
									AI_2.Task.WanderAround();
								}
								if (AI_3 != null)
								{
									AI_3.Task.WanderAround();
								}
								if (AI_4 != null)
								{
									AI_4.Task.WanderAround();
								}
								if (AI_5 != null)
								{
									AI_5.Task.WanderAround();
								}
								if (AI_6 != null)
								{
									AI_6.Task.WanderAround();
								}
								if (AI_7 != null)
								{
									AI_7.Task.WanderAround();
								}
								foreach (Ped _8PlayerGamePokerPed12 in _8PlayerGamePokerPeds)
								{
									if (_8PlayerGamePokerPed12 != null)
									{
										_8PlayerGamePokerPed12.Task.WanderAround();
									}
								}
							}
						}
						catch
						{
							Playing8PlayerPoker = false;
							UI.Notify("~r~ Error ~w~ An Error occurred when starting 8 Player Poker, please try again");
						}
					}
				}
			}
			if (Playing_BlackJack)
			{
				Vector3 offsetInWorldCoords43 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(-0.05f, 0.55f, -0.45f));
				Vector3 offsetInWorldCoords44 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.05f, 0.55f, -0.45f));
				Vector3 offsetInWorldCoords45 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(-0.15f, 0.55f, -0.45f));
				Vector3 offsetInWorldCoords46 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.15f, 0.55f, -0.45f));
				Vector3 offsetInWorldCoords47 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(-0.25f, 0.55f, -0.45f));
				Vector3 offsetInWorldCoords48 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.25f, 0.55f, -0.45f));
				Vector3 offsetInWorldCoords49 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.6f, -0.45f));
				Vector3 offsetInWorldCoords50 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.7f, -0.45f));
				if (Endgame && Game.IsControlJustPressed(Control.Cover))
				{
					foreach (Prop chip7 in Chips)
					{
						if (chip7 != null)
						{
							chip7.Delete();
						}
					}
					Placebet = false;
					Drawcards = false;
					Endgame = false;
					Playercards = 0;
					DealerCards = 0;
					PlayerCard_1 = 0;
					PlayerCard_2 = 0;
					PlayerCard_3 = 0;
					PlayerCard_4 = 0;
					PlayerCard_5 = 0;
					PlayerCard_6 = 0;
					Bet = 0f;
					DealerCard_1 = 0;
					DealerCard_2 = 0;
					DealerCard_3 = 0;
					DealerCard_4 = 0;
					DealerCard_5 = 0;
					DealerCard_6 = 0;
				}
				if (Placebet)
				{
					if (Game.IsControlJustPressed(Control.ThrowGrenade))
					{
						if (Playercards >= 2)
						{
							UI.Notify("~g~Calculating Winner~w~");
							Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
							{
								Game.Player.Character,
								Scene1,
								LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base"),
								"base",
								16f,
								-16f,
								0,
								1148846080
							});
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								Game.Player.Character,
								Scene1,
								"base",
								LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base"),
								16f,
								-16f,
								0,
								1148846080
							});
							Drawcards = true;
							Placebet = false;
							Random random41 = new Random();
							int num79 = random41.Next(0, 2000);
							int num80 = 0;
							if (num79 > 0 && num79 < 100)
							{
								num80 = random41.Next(2, 6);
							}
							if (num79 > 100 && num79 < 125)
							{
								num80 = random41.Next(2, 5);
							}
							if (num79 > 125 && num79 < 200)
							{
								num80 = random41.Next(2, 6);
							}
							if (num79 > 200 && num79 < 350)
							{
								num80 = Playercards + random41.Next(-1, 2);
							}
							if (num79 > 350 && num79 < 600)
							{
								num80 = Playercards + random41.Next(-2, 2);
							}
							if (num79 > 600 && num79 < 800)
							{
								num80 = Playercards;
							}
							if (num79 > 800 && num79 < 1150)
							{
								num80 = random41.Next(13, 28);
							}
							if (num79 > 1150 && num79 < 1350)
							{
								num80 = random41.Next(12, 29);
							}
							if (num79 > 1350 && num79 < 1550)
							{
								num80 = random41.Next(13, 28);
							}
							if (num79 > 1550 && num79 < 1800)
							{
								num80 = random41.Next(14, 27);
							}
							if (num79 > 1800 && num79 < 2000)
							{
								num80 = random41.Next(15, 26);
							}
							if (num79 <= 800)
							{
								DealerCards = num80;
								for (int num81 = 1; num81 <= num80; num81++)
								{
									string randomCardPlayer28 = GetRandomCardPlayer();
									if (num81 == 1)
									{
										DealerCard_1 = Set_DealerCard(randomCardPlayer28, 1);
									}
									Script.Wait(100);
									if (num81 == 1)
									{
										DealerCard_2 = Set_DealerCard(randomCardPlayer28, 2);
									}
									Script.Wait(100);
									if (num81 == 1)
									{
										DealerCard_3 = Set_DealerCard(randomCardPlayer28, 3);
									}
									Script.Wait(100);
									if (num81 == 1)
									{
										DealerCard_4 = Set_DealerCard(randomCardPlayer28, 4);
									}
									Script.Wait(100);
									if (num81 == 1)
									{
										DealerCard_5 = Set_DealerCard(randomCardPlayer28, 5);
									}
									Script.Wait(100);
									if (num81 == 1)
									{
										DealerCard_6 = Set_DealerCard(randomCardPlayer28, 6);
									}
									Script.Wait(100);
								}
							}
							if (num79 > 800)
							{
								UI.Notify("Dealer");
								int num82 = num80;
								DealerCards = num80;
								string randomCardDealer = GetRandomCardDealer();
								int num83 = random41.Next(1, 4);
								if (num83 == 1)
								{
									DealerCard_1 = 10;
									UI.Notify("~p~Dealer~w~ got Card : Jack");
								}
								if (num83 == 2)
								{
									DealerCard_1 = 10;
									UI.Notify("~p~Dealer~w~ got Card : 10");
								}
								if (num83 == 3)
								{
									DealerCard_1 = 10;
									UI.Notify("~p~Dealer~w~ got Card : Queen");
								}
								if (num83 == 4)
								{
									DealerCard_1 = 10;
									UI.Notify("~p~Dealer~w~ got Card : King");
								}
								int num84 = num80 - 10;
								int num85 = num82 - num84;
								if (num85 != 0)
								{
									num83 = (DealerCard_2 = random41.Next(1, num85));
									UI.Notify("~p~Dealer~w~ got Card : " + num83);
									num84 = num85 - num83;
								}
								num85 = num84;
								if (num85 != 0)
								{
									num83 = (DealerCard_3 = random41.Next(1, num85));
									UI.Notify("~p~Dealer~w~ got Card : " + num83);
									num84 = num85 - num83;
								}
								num85 = num84;
								if (num85 != 0)
								{
									num83 = (DealerCard_4 = random41.Next(1, num85));
									UI.Notify("~p~Dealer~w~ got Card : " + num83);
									num84 = num85 - num83;
								}
								num85 = num84;
								if (num85 != 0)
								{
									num83 = (DealerCard_5 = random41.Next(1, num85));
									UI.Notify("~p~Dealer~w~ got Card : " + num83);
									num84 = num85 - num83;
								}
								num85 = num84;
								if (num85 != 0)
								{
									num83 = (DealerCard_6 = random41.Next(1, num85));
									UI.Notify("~p~Dealer~w~ got Card : " + num83);
									num84 = num85 - num83;
								}
							}
							Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base", "base", 8f, -1, true, -1f);
							CalculateBlackJack();
							Drawcards = false;
							Endgame = true;
						}
						else
						{
							Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base", "base", 8f, -1, true, -1f);
							UI.Notify("You cannot Accept untill you have a hand of at least 2");
						}
					}
					if (Game.IsControlJustPressed(Control.Cover))
					{
						if (Playercards == 0)
						{
							Script.Wait(800);
							string randomCardPlayer29 = GetRandomCardPlayer();
							PlayerCard_1 = Set_PlayerCard(randomCardPlayer29, 1);
							Playercards++;
							Prop prop43 = World.CreateProp(randomCardPlayer29, offsetInWorldCoords43, true, false);
							prop43.IsPositionFrozen = true;
							Vector3 position35 = prop43.Position;
							PlayerCardA = prop43;
							prop43.Position = new Vector3(position35.X, position35.Y, position35.Z - GetTableHeight_Cards(CurrentTable));
							prop43.Heading = Game.Player.Character.Heading;
							Chips.Add(prop43);
							int num86 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
							{
								Game.Player.Character.Position.X,
								Game.Player.Character.Position.Y,
								Game.Player.Character.Position.Z,
								0.0,
								0.0,
								Game.Player.Character.Heading,
								2
							});
							string dict36 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v02";
							string anim5 = "check_cards_bet";
							LoadDict(dict36);
							Ped character4 = Game.Player.Character;
							num86 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
							{
								character4.Position.X,
								character4.Position.Y,
								character4.Position.Z,
								0.0,
								0.0,
								character4.Rotation.Z,
								2
							});
							PlaySyncAnim(Game.Player.Character, Game.Player.Character, Scene2, dict36, anim5);
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								PlayerCardA,
								num86,
								"check_cards_bet_card_a",
								LoadDict(dict36),
								16f,
								-16f,
								0,
								1148846080
							});
						}
						else if (Playercards == 1)
						{
							string randomCardPlayer30 = GetRandomCardPlayer();
							PlayerCard_2 = Set_PlayerCard(randomCardPlayer30, 2);
							Playercards++;
							Prop prop44 = World.CreateProp(randomCardPlayer30, offsetInWorldCoords44, true, false);
							prop44.IsPositionFrozen = true;
							Vector3 position36 = prop44.Position;
							PlayerCardB = prop44;
							prop44.Position = new Vector3(position36.X, position36.Y, position36.Z - GetTableHeight_Cards(CurrentTable));
							prop44.Heading = Game.Player.Character.Heading;
							Chips.Add(prop44);
							int num87 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
							{
								Game.Player.Character.Position.X,
								Game.Player.Character.Position.Y,
								Game.Player.Character.Position.Z,
								0.0,
								0.0,
								Game.Player.Character.Heading,
								2
							});
							string dict37 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v02";
							string anim6 = "check_cards_bet";
							LoadDict(dict37);
							Ped character5 = Game.Player.Character;
							num87 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
							{
								character5.Position.X,
								character5.Position.Y,
								character5.Position.Z,
								0.0,
								0.0,
								character5.Rotation.Z,
								2
							});
							PlaySyncAnim(Game.Player.Character, Game.Player.Character, Scene2, dict37, anim6);
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								PlayerCardA,
								num87,
								"check_cards_bet_card_a",
								LoadDict(dict37),
								16f,
								-16f,
								0,
								1148846080
							});
							_8PlayerGamePokerPot += 1000f;
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								PlayerCardB,
								num87,
								"check_cards_bet_card_b",
								LoadDict(dict37),
								16f,
								-16f,
								0,
								1148846080
							});
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								PlayerCardC,
								num87,
								"check_cards_bet_card_c",
								LoadDict(dict37),
								16f,
								-16f,
								0,
								1148846080
							});
						}
						else if (Playercards == 2)
						{
							Script.Wait(800);
							string randomCardPlayer31 = GetRandomCardPlayer();
							PlayerCard_3 = Set_PlayerCard(randomCardPlayer31, 3);
							Playercards++;
							Prop prop45 = World.CreateProp(randomCardPlayer31, offsetInWorldCoords45, true, false);
							prop45.IsPositionFrozen = true;
							Vector3 position37 = prop45.Position;
							PlayerCardC = prop45;
							prop45.Position = new Vector3(position37.X, position37.Y, position37.Z - GetTableHeight_Cards(CurrentTable));
							prop45.Heading = Game.Player.Character.Heading;
							Chips.Add(prop45);
							int num88 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
							{
								Game.Player.Character.Position.X,
								Game.Player.Character.Position.Y,
								Game.Player.Character.Position.Z,
								0.0,
								0.0,
								Game.Player.Character.Heading,
								2
							});
							string dict38 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v02";
							string anim7 = "check_cards_bet";
							LoadDict(dict38);
							Ped character6 = Game.Player.Character;
							num88 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
							{
								character6.Position.X,
								character6.Position.Y,
								character6.Position.Z,
								0.0,
								0.0,
								character6.Rotation.Z,
								2
							});
							PlaySyncAnim(Game.Player.Character, Game.Player.Character, Scene2, dict38, anim7);
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								PlayerCardA,
								num88,
								"check_cards_bet_card_a",
								LoadDict(dict38),
								16f,
								-16f,
								0,
								1148846080
							});
							_8PlayerGamePokerPot += 1000f;
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								PlayerCardB,
								num88,
								"check_cards_bet_card_b",
								LoadDict(dict38),
								16f,
								-16f,
								0,
								1148846080
							});
							Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
							{
								PlayerCardC,
								num88,
								"check_cards_bet_card_c",
								LoadDict(dict38),
								16f,
								-16f,
								0,
								1148846080
							});
						}
						else if (Playercards == 3)
						{
							Script.Wait(800);
							string randomCardPlayer32 = GetRandomCardPlayer();
							PlayerCard_4 = Set_PlayerCard(randomCardPlayer32, 4);
							Playercards++;
							Prop prop46 = World.CreateProp(randomCardPlayer32, offsetInWorldCoords46, true, false);
							prop46.IsPositionFrozen = true;
							Vector3 position38 = prop46.Position;
							PlayerCardD = prop46;
							prop46.Position = new Vector3(position38.X, position38.Y, position38.Z - GetTableHeight_Cards(CurrentTable));
							prop46.Heading = Game.Player.Character.Heading;
							Chips.Add(prop46);
							Script.Wait(500);
							Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base", "base", 8f, -1, true, -1f);
						}
						else if (Playercards == 4)
						{
							Script.Wait(800);
							string randomCardPlayer33 = GetRandomCardPlayer();
							PlayerCard_5 = Set_PlayerCard(randomCardPlayer33, 5);
							Playercards++;
							Prop prop47 = World.CreateProp(randomCardPlayer33, offsetInWorldCoords47, true, false);
							prop47.IsPositionFrozen = true;
							Vector3 position39 = prop47.Position;
							PlayerCardE = prop47;
							prop47.Position = new Vector3(position39.X, position39.Y, position39.Z - GetTableHeight_Cards(CurrentTable));
							prop47.Heading = Game.Player.Character.Heading;
							Chips.Add(prop47);
							Script.Wait(500);
							Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base", "base", 8f, -1, true, -1f);
						}
						else if (Playercards == 5)
						{
							Script.Wait(800);
							string randomCardPlayer34 = GetRandomCardPlayer();
							PlayerCard_6 = Set_PlayerCard(randomCardPlayer34, 6);
							Playercards++;
							Prop prop48 = World.CreateProp(randomCardPlayer34, offsetInWorldCoords48, true, false);
							prop48.IsPositionFrozen = true;
							Vector3 position40 = prop48.Position;
							PlayerCardF = prop48;
							prop48.Position = new Vector3(position40.X, position40.Y, position40.Z - GetTableHeight_Cards(CurrentTable));
							prop48.Heading = Game.Player.Character.Heading;
							Chips.Add(prop48);
							Script.Wait(500);
							Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base", "base", 8f, -1, true, -1f);
						}
					}
				}
				if (!Placebet && Game.IsControlJustPressed(Control.Cover))
				{
					try
					{
						if (World.GetDistance(Game.Player.Character.Position, new Vector3(966.9417f, 32.0952f, 115f)) < 6f || World.GetDistance(Game.Player.Character.Position, new Vector3(1146.76f, 248.972f, -51f)) < 6f || World.GetDistance(Game.Player.Character.Position, new Vector3(1131.154f, 265.2554f, -51f)) < 6f)
						{
							string userInput2 = Game.GetUserInput(WindowTitle.EnterEyefindMessage, "5000", 6);
							Bet = float.Parse(userInput2);
						}
						else
						{
							string userInput3 = Game.GetUserInput(WindowTitle.EnterEyefindMessage, "5000", 5);
							Bet = float.Parse(userInput3);
						}
						if (Bet < Chips_Amount)
						{
							UI.Notify("Player bet :" + Bet);
							Placebet = true;
							if (Bet <= 10f)
							{
								Prop prop49 = World.CreateProp("vw_prop_chip_10dollar_st", new Vector3(offsetInWorldCoords49.X, offsetInWorldCoords49.Y, offsetInWorldCoords43.Z), true, false);
								prop49.IsPositionFrozen = true;
								prop49.Position = new Vector3(prop49.Position.X, prop49.Position.Y, prop49.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop49);
							}
							if (Bet > 10f && Bet <= 60f)
							{
								Prop prop50 = World.CreateProp("vw_prop_chip_50dollar_st", new Vector3(offsetInWorldCoords49.X, offsetInWorldCoords49.Y, offsetInWorldCoords43.Z), true, false);
								prop50.IsPositionFrozen = true;
								prop50.Position = new Vector3(prop50.Position.X, prop50.Position.Y, prop50.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop50);
							}
							if (Bet > 50f && Bet <= 100f)
							{
								Prop prop51 = World.CreateProp("vw_prop_chip_100dollar_st", new Vector3(offsetInWorldCoords49.X, offsetInWorldCoords49.Y, offsetInWorldCoords43.Z), true, false);
								prop51.IsPositionFrozen = true;
								prop51.Position = new Vector3(prop51.Position.X, prop51.Position.Y, prop51.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop51);
							}
							if (Bet > 100f && Bet <= 500f)
							{
								Prop prop52 = World.CreateProp("vw_prop_chip_500dollar_st", new Vector3(offsetInWorldCoords49.X, offsetInWorldCoords49.Y, offsetInWorldCoords43.Z), true, false);
								prop52.IsPositionFrozen = true;
								prop52.Position = new Vector3(prop52.Position.X, prop52.Position.Y, prop52.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop52);
							}
							if (Bet > 500f && Bet <= 1000f)
							{
								Prop prop53 = World.CreateProp("vw_prop_chip_1kdollar_st", new Vector3(offsetInWorldCoords49.X, offsetInWorldCoords49.Y, offsetInWorldCoords43.Z), true, false);
								prop53.IsPositionFrozen = true;
								prop53.Position = new Vector3(prop53.Position.X, prop53.Position.Y, prop53.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop53);
								Script.Wait(10);
							}
							if (Bet > 1000f && Bet <= 5000f)
							{
								Prop prop54 = World.CreateProp("vw_prop_plaq_5kdollar_st", new Vector3(offsetInWorldCoords49.X, offsetInWorldCoords49.Y, offsetInWorldCoords43.Z), true, false);
								prop54.IsPositionFrozen = true;
								prop54.Position = new Vector3(prop54.Position.X, prop54.Position.Y, prop54.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop54);
								Script.Wait(10);
							}
							if (Bet > 5000f && Bet <= 10000f)
							{
								Prop prop55 = World.CreateProp("vw_prop_plaq_10kdollar_st", new Vector3(offsetInWorldCoords49.X, offsetInWorldCoords49.Y, offsetInWorldCoords43.Z), true, false);
								prop55.IsPositionFrozen = true;
								prop55.Position = new Vector3(prop55.Position.X, prop55.Position.Y, prop55.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop55);
								Script.Wait(10);
							}
							if (Bet > 10000f && Bet <= 25000f)
							{
								Prop prop56 = World.CreateProp("vw_prop_plaq_10kdollar_st", new Vector3(offsetInWorldCoords49.X, offsetInWorldCoords49.Y, offsetInWorldCoords43.Z), true, false);
								prop56.IsPositionFrozen = true;
								prop56.Position = new Vector3(prop56.Position.X, prop56.Position.Y, prop56.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop56);
								Script.Wait(10);
								prop56 = World.CreateProp("vw_prop_vw_chips_pile_02a", new Vector3(offsetInWorldCoords50.X, offsetInWorldCoords50.Y, offsetInWorldCoords43.Z), true, false);
								prop56.IsPositionFrozen = true;
								prop56.Position = new Vector3(prop56.Position.X, prop56.Position.Y, prop56.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop56);
							}
							if (Bet > 25000f && Bet <= 50000f)
							{
								Prop prop57 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords49.X, offsetInWorldCoords49.Y, offsetInWorldCoords43.Z), true, false);
								prop57.IsPositionFrozen = true;
								prop57.Position = new Vector3(prop57.Position.X, prop57.Position.Y, prop57.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop57);
								prop57 = World.CreateProp("vw_prop_vw_chips_pile_02a", new Vector3(offsetInWorldCoords50.X, offsetInWorldCoords50.Y, offsetInWorldCoords43.Z), true, false);
								prop57.IsPositionFrozen = true;
								prop57.Position = new Vector3(prop57.Position.X, prop57.Position.Y, prop57.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop57);
							}
							if (Bet > 50000f && Bet <= 150000f)
							{
								Prop prop58 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords49.X, offsetInWorldCoords49.Y, offsetInWorldCoords43.Z), true, false);
								prop58.IsPositionFrozen = true;
								prop58.Position = new Vector3(prop58.Position.X, prop58.Position.Y, prop58.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop58);
								Script.Wait(10);
								prop58 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords50.X, offsetInWorldCoords50.Y, offsetInWorldCoords43.Z), true, false);
								prop58.IsPositionFrozen = true;
								prop58.Position = new Vector3(prop58.Position.X, prop58.Position.Y, prop58.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop58);
							}
							if (Bet > 50000f && Bet <= 150000f)
							{
								Prop prop59 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords49.X, offsetInWorldCoords49.Y, offsetInWorldCoords43.Z), true, false);
								prop59.IsPositionFrozen = true;
								prop59.Position = new Vector3(prop59.Position.X, prop59.Position.Y, prop59.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop59);
								Script.Wait(10);
								prop59 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords50.X, offsetInWorldCoords50.Y, offsetInWorldCoords43.Z), true, false);
								prop59.IsPositionFrozen = true;
								prop59.Position = new Vector3(prop59.Position.X, prop59.Position.Y, prop59.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop59);
								Vector3 offsetInWorldCoords51 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.8f, -0.45f));
								prop59 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords51.X, offsetInWorldCoords51.Y, offsetInWorldCoords43.Z), true, false);
								prop59.IsPositionFrozen = true;
								prop59.Position = new Vector3(prop59.Position.X, prop59.Position.Y, prop59.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop59);
								Vector3 offsetInWorldCoords52 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.9f, -0.45f));
								prop59 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords52.X, offsetInWorldCoords52.Y, offsetInWorldCoords43.Z), true, false);
								prop59.IsPositionFrozen = true;
								prop59.Position = new Vector3(prop59.Position.X, prop59.Position.Y, prop59.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop59);
							}
							if (Bet > 150000f && Bet < 5000000f)
							{
								Prop prop60 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords49.X, offsetInWorldCoords49.Y, offsetInWorldCoords43.Z), true, false);
								prop60.IsPositionFrozen = true;
								prop60.Position = new Vector3(prop60.Position.X, prop60.Position.Y, prop60.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop60);
								Script.Wait(10);
								prop60 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords50.X, offsetInWorldCoords50.Y, offsetInWorldCoords43.Z), true, false);
								prop60.IsPositionFrozen = true;
								prop60.Position = new Vector3(prop60.Position.X, prop60.Position.Y, prop60.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop60);
								Vector3 offsetInWorldCoords53 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.8f, -0.45f));
								prop60 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords53.X, offsetInWorldCoords53.Y, offsetInWorldCoords43.Z), true, false);
								prop60.IsPositionFrozen = true;
								prop60.Position = new Vector3(prop60.Position.X, prop60.Position.Y, prop60.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop60);
								Vector3 offsetInWorldCoords54 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.9f, -0.45f));
								prop60 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords54.X, offsetInWorldCoords54.Y, offsetInWorldCoords43.Z), true, false);
								prop60.IsPositionFrozen = true;
								prop60.Position = new Vector3(prop60.Position.X, prop60.Position.Y, prop60.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop60);
								Vector3 offsetInWorldCoords55 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.6f, -0.4f));
								Vector3 offsetInWorldCoords56 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.7f, -0.4f));
								prop60 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords55.X, offsetInWorldCoords55.Y, offsetInWorldCoords43.Z), true, false);
								prop60.IsPositionFrozen = true;
								prop60.Position = new Vector3(prop60.Position.X, prop60.Position.Y, prop60.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop60);
								Script.Wait(10);
								prop60 = World.CreateProp("vw_prop_vw_chips_pile_03a", offsetInWorldCoords56, true, false);
								prop60.IsPositionFrozen = true;
								prop60.Position = new Vector3(prop60.Position.X, prop60.Position.Y, prop60.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop60);
								Vector3 offsetInWorldCoords57 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.8f, -0.4f));
								prop60 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords57.X, offsetInWorldCoords57.Y, offsetInWorldCoords43.Z), true, false);
								prop60.IsPositionFrozen = true;
								prop60.Position = new Vector3(prop60.Position.X, prop60.Position.Y, prop60.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop60);
								Vector3 offsetInWorldCoords58 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.9f, -0.4f));
								prop60 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords58.X, offsetInWorldCoords58.Y, offsetInWorldCoords43.Z), true, false);
								prop60.IsPositionFrozen = true;
								prop60.Position = new Vector3(prop60.Position.X, prop60.Position.Y, prop60.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop60);
							}
							if (Bet > 4999999f)
							{
								Vector3 offsetInWorldCoords59 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.6f, -0.5f));
								Prop prop61 = World.CreateProp("prop_gold_bar", new Vector3(offsetInWorldCoords59.X, offsetInWorldCoords59.Y, offsetInWorldCoords43.Z), true, false);
								prop61.IsPositionFrozen = true;
								prop61.Position = new Vector3(prop61.Position.X, prop61.Position.Y, prop61.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop61);
								Script.Wait(10);
							}
						}
						if (Bet > Chips_Amount)
						{
							UI.Notify("you do not have enough chips for this bet");
							Placebet = false;
						}
						Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
						{
							Game.Player.Character,
							Scene1,
							LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@engaged@01a@play@v02"),
							"place_bet",
							16f,
							-16f,
							0,
							1148846080
						});
						Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
						{
							Game.Player.Character,
							Scene1,
							"place_bet",
							LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@engaged@01a@play@v02"),
							16f,
							-16f,
							0,
							1148846080
						});
					}
					catch (Exception)
					{
						Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base", "base", 8f, -1, true, -1f);
						UI.Notify("Player did not enter a valid bet");
					}
				}
			}
			if (Playing_BlackJack)
			{
				Vector3 offsetInWorldCoords60 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(-0.05f, 0.55f, -0.45f));
				Vector3 offsetInWorldCoords61 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.05f, 0.55f, -0.45f));
				Vector3 offsetInWorldCoords62 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(-0.15f, 0.55f, -0.45f));
				Vector3 offsetInWorldCoords63 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.15f, 0.55f, -0.45f));
				Vector3 offsetInWorldCoords64 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(-0.25f, 0.55f, -0.45f));
				Vector3 offsetInWorldCoords65 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.25f, 0.55f, -0.45f));
				Vector3 offsetInWorldCoords66 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.6f, 0f));
			}
			for (int num89 = 0; num89 < BlackJackTables.Count; num89++)
			{
				if (World.GetDistance(Game.Player.Character.Position, BlackJackTables[num89]) < 2f)
				{
					if (!Playing_BlackJack)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to play BlackJack ");
					}
					if (Playing_BlackJack && !Placebet && !Endgame)
					{
						Show_EQ_Chips("Exit", "Place Bet");
					}
					if (Placebet && !Endgame)
					{
						Show_GQ("Accept Hand", "Draw Card");
					}
					if (Drawcards && !Endgame)
					{
						Show_EQ("Fold", "Play Hand");
					}
					if (Endgame)
					{
						Show_EQ("Exit", "Play Again");
					}
				}
			}
			for (int num90 = 0; num90 < BlackJackTables.Count; num90++)
			{
				if (!(World.GetDistance(Game.Player.Character.Position, BlackJackTables[num90]) < 2f))
				{
					continue;
				}
				if (!Playing_BlackJack && !Placebet)
				{
					if (!Game.IsControlJustPressed(Control.Context))
					{
						continue;
					}
					CurrentTable = num90;
					Game.Player.Character.Position = BlackJackTables[num90];
					string enterAnim4 = GetEnterAnim();
					string dict39 = "anim_casino_b@amb@casino@games@shared@player@";
					LoadDict("anim_casino_b@amb@casino@games@shared@player@");
					LoadDict(dict39);
					int num91 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
					{
						Game.Player.Character.Position.X,
						Game.Player.Character.Position.Y,
						BlackJackTables[num90].Z - 0.25f,
						0.0,
						0.0,
						BlackJackRot[num90] + 90f,
						2
					});
					Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
					{
						Game.Player.Character,
						num91,
						LoadDict(dict39),
						"sit_enter_left",
						16f,
						-16f,
						0,
						1148846080
					});
					Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
					{
						Game.Player.Character,
						num91,
						"sit_enter_left",
						LoadDict(dict39),
						16f,
						-16f,
						0,
						1148846080
					});
					Placebet = false;
					Drawcards = false;
					Endgame = false;
					Playercards = 0;
					DealerCards = 0;
					PlayerCard_1 = 0;
					PlayerCard_2 = 0;
					PlayerCard_3 = 0;
					PlayerCard_4 = 0;
					PlayerCard_5 = 0;
					PlayerCard_6 = 0;
					Bet = 0f;
					DealerCard_1 = 0;
					DealerCard_2 = 0;
					DealerCard_3 = 0;
					DealerCard_4 = 0;
					DealerCard_5 = 0;
					DealerCard_6 = 0;
					foreach (Prop chip8 in Chips)
					{
						if (chip8 != null)
						{
							chip8.Delete();
						}
					}
					Playing_BlackJack = true;
				}
				else
				{
					if (!Playing_BlackJack || Placebet || !Game.IsControlJustPressed(Control.Context))
					{
						continue;
					}
					CurrentTable = num90;
					Game.Player.Character.Position = BlackJackTables[num90];
					string enterAnim5 = GetEnterAnim();
					string dict40 = "anim_casino_b@amb@casino@games@shared@player@";
					LoadDict(dict40);
					int num92 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
					{
						Game.Player.Character.Position.X,
						Game.Player.Character.Position.Y,
						BlackJackTables[num90].Z - 0.25f,
						0.0,
						0.0,
						BlackJackRot[num90] + 90f,
						2
					});
					Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
					{
						Game.Player.Character,
						num92,
						LoadDict(dict40),
						"sit_exit_left",
						16f,
						-16f,
						0,
						1148846080
					});
					Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
					{
						num92,
						Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { num92 })
					});
					Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
					{
						Game.Player.Character,
						num92,
						"sit_exit_left",
						LoadDict(dict40),
						16f,
						-16f,
						0,
						1148846080
					});
					Script.Wait(4000);
					Game.Player.Character.Task.ClearAll();
					Game.Player.Character.IsPositionFrozen = false;
					foreach (Prop chip9 in Chips)
					{
						if (chip9 != null)
						{
							chip9.Delete();
						}
					}
					Playing_BlackJack = false;
					foreach (Prop chip10 in Chips)
					{
						if (chip10 != null)
						{
							chip10.Delete();
						}
					}
					Placebet = false;
					Drawcards = false;
					Endgame = false;
					Playercards = 0;
					DealerCards = 0;
					PlayerCard_1 = 0;
					PlayerCard_2 = 0;
					PlayerCard_3 = 0;
					PlayerCard_4 = 0;
					PlayerCard_5 = 0;
					PlayerCard_6 = 0;
					Bet = 0f;
					DealerCard_1 = 0;
					DealerCard_2 = 0;
					DealerCard_3 = 0;
					DealerCard_4 = 0;
					DealerCard_5 = 0;
					DealerCard_6 = 0;
				}
			}
			if (Playing_Poker)
			{
				Vector3 offsetInWorldCoords67 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(-0.05f, 0.55f, -0.45f));
				Vector3 offsetInWorldCoords68 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.05f, 0.55f, -0.45f));
				Vector3 offsetInWorldCoords69 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(-0.15f, 0.55f, -0.45f));
				Vector3 offsetInWorldCoords70 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.15f, 0.55f, -0.45f));
				Vector3 offsetInWorldCoords71 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(-0.25f, 0.55f, -0.45f));
				Vector3 offsetInWorldCoords72 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.25f, 0.55f, -0.45f));
				Vector3 offsetInWorldCoords73 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.6f, -0.45f));
			}
			for (int num93 = 0; num93 < PokerTables.Count; num93++)
			{
				if (World.GetDistance(Game.Player.Character.Position, PokerTables[num93]) < 2f)
				{
					if (!Playing_Poker)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to play Poker ");
					}
					if (Playing_Poker && !Placebet && !Endgame)
					{
						Show_EQ_Chips("Exit", "Place Bet");
					}
					if (Placebet && !Endgame)
					{
						Show_GQ("Accept Hand", "Draw Card");
					}
					if (Drawcards && !Endgame)
					{
						Show_EGQ("Fold", "Edit Bet", "Play");
					}
					if (Endgame)
					{
						Show_EQ("Exit", "Play Again");
					}
				}
			}
			if (Playing_Poker)
			{
				Vector3 offsetInWorldCoords74 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(-0.05f, 0.9f, -0.45f));
				Vector3 offsetInWorldCoords75 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.05f, 0.9f, -0.45f));
				Vector3 offsetInWorldCoords76 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(-0.15f, 0.9f, -0.45f));
				Vector3 offsetInWorldCoords77 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.15f, 0.9f, -0.45f));
				Vector3 offsetInWorldCoords78 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(-0.25f, 0.9f, -0.45f));
				Vector3 offsetInWorldCoords79 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.25f, 0.9f, -0.45f));
				Vector3 offsetInWorldCoords80 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(-0.05f, 0.55f, -0.45f));
				Vector3 offsetInWorldCoords81 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.05f, 0.55f, -0.45f));
				Vector3 offsetInWorldCoords82 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(-0.15f, 0.55f, -0.45f));
				Vector3 offsetInWorldCoords83 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.15f, 0.55f, -0.45f));
				Vector3 offsetInWorldCoords84 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(-0.25f, 0.55f, -0.45f));
				Vector3 offsetInWorldCoords85 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.25f, 0.55f, -0.45f));
				Vector3 offsetInWorldCoords86 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.6f, 0f));
				Vector3 offsetInWorldCoords87 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.7f, 0f));
				if (Endgame)
				{
					if (Game.IsControlJustPressed(Control.Cover))
					{
						foreach (Prop chip11 in Chips)
						{
							if (chip11 != null)
							{
								chip11.Delete();
							}
						}
						Editbet = false;
						Placebet = false;
						Drawcards = false;
						Endgame = false;
						Playercards = 0;
						DealerCards = 0;
						PlayerCard_1 = 0;
						PlayerCard_2 = 0;
						PlayerCard_3 = 0;
						PlayerCard_4 = 0;
						PlayerCard_5 = 0;
						PlayerCard_6 = 0;
						Bet = 0f;
						DealerCard_1 = 0;
						DealerCard_2 = 0;
						DealerCard_3 = 0;
						DealerCard_4 = 0;
						DealerCard_5 = 0;
						DealerCard_6 = 0;
					}
					if (Game.IsControlJustPressed(Control.Context))
					{
						foreach (Prop chip12 in Chips)
						{
							if (chip12 != null)
							{
								chip12.Delete();
							}
						}
						Editbet = false;
						Placebet = false;
						Drawcards = false;
						Endgame = false;
						Playercards = 0;
						DealerCards = 0;
						PlayerCard_1 = 0;
						PlayerCard_2 = 0;
						PlayerCard_3 = 0;
						PlayerCard_4 = 0;
						PlayerCard_5 = 0;
						PlayerCard_6 = 0;
						Bet = 0f;
						DealerCard_1 = 0;
						DealerCard_2 = 0;
						DealerCard_3 = 0;
						DealerCard_4 = 0;
						DealerCard_5 = 0;
						DealerCard_6 = 0;
					}
				}
				if (Editbet)
				{
					if (Game.IsControlJustPressed(Control.Context))
					{
						Game.Player.Money -= (int)Bet;
						PlayerCardA.Rotation = new Vector3(PlayerCardA.Rotation.X, 180f, PlayerCardA.Rotation.Z);
						Script.Wait(200);
						PlayerCardB.Rotation = new Vector3(PlayerCardB.Rotation.X, 180f, PlayerCardB.Rotation.Z);
						Script.Wait(200);
						PlayerCardC.Rotation = new Vector3(PlayerCardC.Rotation.X, 180f, PlayerCardC.Rotation.Z);
						Script.Wait(1000);
						Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@threecardpoker@ped_male@engaged@01b@reacts@v01", "collect_chips_card_c", 8f, -1, true, -1f);
						CardB.Rotation = new Vector3(CardB.Rotation.X, 180f, CardB.Rotation.Z);
						Script.Wait(200);
						CardC.Rotation = new Vector3(CardC.Rotation.X, 180f, CardC.Rotation.Z);
						Script.Wait(200);
						CardA.Rotation = new Vector3(CardA.Rotation.X, 180f, CardA.Rotation.Z);
						Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_male@engaged@01a@base", "idle_c", 8f, -1, true, -1f);
						for (int num94 = 0; num94 < 100; num94++)
						{
							GameFail("You Folded", "3 Card Poker");
							Script.Wait(1);
						}
						foreach (Prop chip13 in Chips)
						{
							if (chip13 != null)
							{
								chip13.Delete();
							}
						}
						Editbet = false;
						Placebet = false;
						Drawcards = false;
						Endgame = false;
						Playercards = 0;
						DealerCards = 0;
						PlayerCard_1 = 0;
						PlayerCard_2 = 0;
						PlayerCard_3 = 0;
						PlayerCard_4 = 0;
						PlayerCard_5 = 0;
						PlayerCard_6 = 0;
						Bet = 0f;
						DealerCard_1 = 0;
						DealerCard_2 = 0;
						DealerCard_3 = 0;
						DealerCard_4 = 0;
						DealerCard_5 = 0;
						DealerCard_6 = 0;
						Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base", "base", 8f, -1, true, -1f);
					}
					if (Game.IsControlJustPressed(Control.ThrowGrenade))
					{
						try
						{
							UI.Notify("~p~ Dealer ~w~ : Enter an amount to ~b~Increase~w~/~g~Decrease~w~ Bet, Enter ~g~-~w~ to enter a Negative Bet");
							float num95 = 0f;
							if (World.GetDistance(Game.Player.Character.Position, new Vector3(966.9417f, 32.0952f, 115f)) < 6f || World.GetDistance(Game.Player.Character.Position, new Vector3(1146.76f, 248.972f, -51f)) < 6f || World.GetDistance(Game.Player.Character.Position, new Vector3(1131.154f, 265.2554f, -51f)) < 6f)
							{
								string userInput4 = Game.GetUserInput(WindowTitle.EnterEyefindMessage, "5000", 5);
								num95 = float.Parse(userInput4);
							}
							else
							{
								string userInput5 = Game.GetUserInput(WindowTitle.EnterEyefindMessage, "5000", 5);
								num95 = float.Parse(userInput5);
							}
							Bet += num95;
							UI.Notify("~p~ Dealer ~w~ : Total bet : " + Bet + "c");
							if (Bet < Chips_Amount)
							{
								UI.Notify("Player bet :" + Bet);
								Placebet = true;
								if (Bet <= 10f)
								{
									Prop prop62 = World.CreateProp("vw_prop_chip_10dollar_st", offsetInWorldCoords86, true, false);
									prop62.IsPositionFrozen = true;
									prop62.Position = new Vector3(prop62.Position.X, prop62.Position.Y, prop62.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop62);
								}
								if (Bet > 10f && Bet <= 60f)
								{
									Prop prop63 = World.CreateProp("vw_prop_chip_50dollar_st", offsetInWorldCoords86, true, false);
									prop63.IsPositionFrozen = true;
									prop63.Position = new Vector3(prop63.Position.X, prop63.Position.Y, prop63.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop63);
								}
								if (Bet > 50f && Bet <= 100f)
								{
									Prop prop64 = World.CreateProp("vw_prop_chip_100dollar_st", offsetInWorldCoords86, true, false);
									prop64.IsPositionFrozen = true;
									prop64.Position = new Vector3(prop64.Position.X, prop64.Position.Y, prop64.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop64);
								}
								if (Bet > 100f && Bet <= 500f)
								{
									Prop prop65 = World.CreateProp("vw_prop_chip_500dollar_st", offsetInWorldCoords86, true, false);
									prop65.IsPositionFrozen = true;
									prop65.Position = new Vector3(prop65.Position.X, prop65.Position.Y, prop65.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop65);
								}
								if (Bet > 500f && Bet <= 1000f)
								{
									Prop prop66 = World.CreateProp("vw_prop_chip_1kdollar_st", offsetInWorldCoords86, true, false);
									prop66.IsPositionFrozen = true;
									prop66.Position = new Vector3(prop66.Position.X, prop66.Position.Y, prop66.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop66);
									Script.Wait(10);
								}
								if (Bet > 1000f && Bet <= 5000f)
								{
									Prop prop67 = World.CreateProp("vw_prop_plaq_5kdollar_st", offsetInWorldCoords86, true, false);
									prop67.IsPositionFrozen = true;
									prop67.Position = new Vector3(prop67.Position.X, prop67.Position.Y, prop67.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop67);
									Script.Wait(10);
								}
								if (Bet > 5000f && Bet <= 10000f)
								{
									Prop prop68 = World.CreateProp("vw_prop_plaq_10kdollar_st", offsetInWorldCoords86, true, false);
									prop68.IsPositionFrozen = true;
									prop68.Position = new Vector3(prop68.Position.X, prop68.Position.Y, prop68.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop68);
									Script.Wait(10);
								}
								if (Bet > 10000f && Bet <= 25000f)
								{
									Prop prop69 = World.CreateProp("vw_prop_plaq_10kdollar_st", offsetInWorldCoords86, true, false);
									prop69.IsPositionFrozen = true;
									prop69.Position = new Vector3(prop69.Position.X, prop69.Position.Y, prop69.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop69);
									Script.Wait(10);
									prop69 = World.CreateProp("vw_prop_vw_chips_pile_02a", offsetInWorldCoords87, true, false);
									prop69.IsPositionFrozen = true;
									prop69.Position = new Vector3(prop69.Position.X, prop69.Position.Y, prop69.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop69);
								}
								if (Bet > 25000f && Bet <= 50000f)
								{
									Prop prop70 = World.CreateProp("vw_prop_vw_chips_pile_03a", offsetInWorldCoords86, true, false);
									prop70.IsPositionFrozen = true;
									prop70.Position = new Vector3(prop70.Position.X, prop70.Position.Y, prop70.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop70);
									prop70 = World.CreateProp("vw_prop_vw_chips_pile_02a", offsetInWorldCoords87, true, false);
									prop70.IsPositionFrozen = true;
									prop70.Position = new Vector3(prop70.Position.X, prop70.Position.Y, prop70.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop70);
								}
								if (Bet > 50000f && Bet <= 150000f)
								{
									Prop prop71 = World.CreateProp("vw_prop_vw_chips_pile_03a", offsetInWorldCoords86, true, false);
									prop71.IsPositionFrozen = true;
									prop71.Position = new Vector3(prop71.Position.X, prop71.Position.Y, prop71.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop71);
									Script.Wait(10);
									prop71 = World.CreateProp("vw_prop_vw_chips_pile_03a", offsetInWorldCoords87, true, false);
									prop71.IsPositionFrozen = true;
									prop71.Position = new Vector3(prop71.Position.X, prop71.Position.Y, prop71.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop71);
								}
								if (Bet > 50000f && Bet <= 150000f)
								{
									Prop prop72 = World.CreateProp("vw_prop_vw_chips_pile_03a", offsetInWorldCoords86, true, false);
									prop72.IsPositionFrozen = true;
									prop72.Position = new Vector3(prop72.Position.X, prop72.Position.Y, prop72.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop72);
									Script.Wait(10);
									prop72 = World.CreateProp("vw_prop_vw_chips_pile_03a", offsetInWorldCoords87, true, false);
									prop72.IsPositionFrozen = true;
									prop72.Position = new Vector3(prop72.Position.X, prop72.Position.Y, prop72.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop72);
									Vector3 offsetInWorldCoords88 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.8f, -0.45f));
									prop72 = World.CreateProp("vw_prop_vw_chips_pile_03a", offsetInWorldCoords88, true, false);
									prop72.IsPositionFrozen = true;
									prop72.Position = new Vector3(prop72.Position.X, prop72.Position.Y, prop72.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop72);
									Vector3 offsetInWorldCoords89 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.9f, -0.45f));
									prop72 = World.CreateProp("vw_prop_vw_chips_pile_03a", offsetInWorldCoords89, true, false);
									prop72.IsPositionFrozen = true;
									prop72.Position = new Vector3(prop72.Position.X, prop72.Position.Y, prop72.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop72);
								}
								if (Bet > 150000f && Bet < 5000000f)
								{
									Prop prop73 = World.CreateProp("vw_prop_vw_chips_pile_03a", offsetInWorldCoords86, true, false);
									prop73.IsPositionFrozen = true;
									prop73.Position = new Vector3(prop73.Position.X, prop73.Position.Y, prop73.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop73);
									Script.Wait(10);
									prop73 = World.CreateProp("vw_prop_vw_chips_pile_03a", offsetInWorldCoords87, true, false);
									prop73.IsPositionFrozen = true;
									prop73.Position = new Vector3(prop73.Position.X, prop73.Position.Y, prop73.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop73);
									Vector3 offsetInWorldCoords90 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.8f, -0.45f));
									prop73 = World.CreateProp("vw_prop_vw_chips_pile_03a", offsetInWorldCoords90, true, false);
									prop73.IsPositionFrozen = true;
									prop73.Position = new Vector3(prop73.Position.X, prop73.Position.Y, prop73.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop73);
									Vector3 offsetInWorldCoords91 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.9f, -0.45f));
									prop73 = World.CreateProp("vw_prop_vw_chips_pile_03a", offsetInWorldCoords91, true, false);
									prop73.IsPositionFrozen = true;
									prop73.Position = new Vector3(prop73.Position.X, prop73.Position.Y, prop73.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop73);
									Vector3 offsetInWorldCoords92 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.6f, -0.4f));
									Vector3 offsetInWorldCoords93 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.7f, -0.4f));
									prop73 = World.CreateProp("vw_prop_vw_chips_pile_03a", offsetInWorldCoords92, true, false);
									prop73.IsPositionFrozen = true;
									prop73.Position = new Vector3(prop73.Position.X, prop73.Position.Y, prop73.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop73);
									Script.Wait(10);
									prop73 = World.CreateProp("vw_prop_vw_chips_pile_03a", offsetInWorldCoords93, true, false);
									prop73.IsPositionFrozen = true;
									prop73.Position = new Vector3(prop73.Position.X, prop73.Position.Y, prop73.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop73);
									Vector3 offsetInWorldCoords94 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.8f, -0.4f));
									prop73 = World.CreateProp("vw_prop_vw_chips_pile_03a", offsetInWorldCoords94, true, false);
									prop73.IsPositionFrozen = true;
									prop73.Position = new Vector3(prop73.Position.X, prop73.Position.Y, prop73.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop73);
									Vector3 offsetInWorldCoords95 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.9f, -0.4f));
									prop73 = World.CreateProp("vw_prop_vw_chips_pile_03a", offsetInWorldCoords95, true, false);
									prop73.IsPositionFrozen = true;
									prop73.Position = new Vector3(prop73.Position.X, prop73.Position.Y, prop73.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop73);
								}
								if (Bet > 4999999f)
								{
									Vector3 offsetInWorldCoords96 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.6f, -0.5f));
									Prop prop74 = World.CreateProp("prop_gold_bar", offsetInWorldCoords96, true, false);
									prop74.IsPositionFrozen = true;
									prop74.Position = new Vector3(prop74.Position.X, prop74.Position.Y, prop74.Position.Z + GetTableHeight_Chips(CurrentTable));
									Chips.Add(prop74);
									Script.Wait(10);
								}
							}
							if (Bet > Chips_Amount)
							{
								UI.Notify("you do not have enough chips for this bet");
								Placebet = false;
							}
							Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_male@engaged@01a@play@v02", "place_bet", 8f, -1, true, -1f);
							Script.Wait(1000);
							Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base", "base", 8f, -1, true, -1f);
						}
						catch (Exception)
						{
							Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base", "base", 8f, -1, true, -1f);
							UI.Notify("Player did not enter a valid bet");
						}
					}
					if (Game.IsControlJustPressed(Control.Cover))
					{
						if (Playercards == 3)
						{
							UI.Notify("Calculating winner");
							Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base", "base", 8f, -1, true, -1f);
							Editbet = false;
							Endgame = true;
							Placebet = false;
							CardA.Rotation = new Vector3(CardA.Rotation.X, 180f, CardA.Rotation.Z);
							CardB.Rotation = new Vector3(CardB.Rotation.X, 180f, CardB.Rotation.Z);
							CardC.Rotation = new Vector3(CardC.Rotation.X, 180f, CardC.Rotation.Z);
							Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_male@engaged@01a@play@v02", "place_bet", 8f, -1, true, -1f);
							CalculatePoker();
						}
						else
						{
							Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base", "base", 8f, -1, true, -1f);
							UI.Notify("You cannot Accept untill you have a hand of at least 3");
						}
					}
				}
				if (Placebet)
				{
					if (Game.IsControlJustPressed(Control.ThrowGrenade))
					{
						if (Playercards == 3)
						{
							Drawcards = true;
							Endgame = false;
							Editbet = true;
						}
						else
						{
							Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_male@engaged@01a@play@v02", "place_bet", 8f, -1, true, -1f);
							UI.Notify("You cannot Accept untill you have a hand of at least 3");
						}
					}
					if (Game.IsControlJustPressed(Control.Cover) && Playercards < 3)
					{
						string randomCardPlayer35 = GetRandomCardPlayer();
						PlayerCard_1 = Set_PlayerCardPoker(randomCardPlayer35, 1);
						Playercards = 3;
						Prop prop75 = World.CreateProp(randomCardPlayer35, offsetInWorldCoords80, true, false);
						prop75.IsPositionFrozen = true;
						Vector3 position41 = prop75.Position;
						PlayerCardA = prop75;
						prop75.Position = new Vector3(position41.X, position41.Y, position41.Z - GetTableHeight_Cards(CurrentTable));
						prop75.Heading = Game.Player.Character.Heading;
						Chips.Add(prop75);
						Script.Wait(1);
						randomCardPlayer35 = GetRandomCardDealer();
						DealerCard_1 = Set_DealerCardPoker(randomCardPlayer35, 1);
						prop75 = World.CreateProp(randomCardPlayer35, offsetInWorldCoords76, true, false);
						prop75.IsPositionFrozen = true;
						position41 = prop75.Position;
						CardC = prop75;
						prop75.Position = new Vector3(position41.X, position41.Y, position41.Z - GetTableHeight_Cards(CurrentTable));
						prop75.Heading = Game.Player.Character.Heading;
						Chips.Add(prop75);
						CardC.Rotation = new Vector3(CardC.Rotation.X, 180f, CardC.Rotation.Z);
						Script.Wait(1);
						randomCardPlayer35 = GetRandomCardPlayer();
						PlayerCard_2 = Set_PlayerCardPoker(randomCardPlayer35, 2);
						prop75 = World.CreateProp(randomCardPlayer35, offsetInWorldCoords81, true, false);
						prop75.IsPositionFrozen = true;
						PlayerCardB = prop75;
						position41 = prop75.Position;
						prop75.Position = new Vector3(position41.X, position41.Y, position41.Z - GetTableHeight_Cards(CurrentTable));
						prop75.Heading = Game.Player.Character.Heading;
						Chips.Add(prop75);
						Script.Wait(1);
						randomCardPlayer35 = GetRandomCardDealer();
						DealerCard_2 = Set_DealerCardPoker(randomCardPlayer35, 2);
						prop75 = World.CreateProp(randomCardPlayer35, offsetInWorldCoords75, true, false);
						prop75.IsPositionFrozen = true;
						position41 = prop75.Position;
						CardB = prop75;
						prop75.Position = new Vector3(position41.X, position41.Y, position41.Z - GetTableHeight_Cards(CurrentTable));
						prop75.Heading = Game.Player.Character.Heading;
						Chips.Add(prop75);
						CardB.Rotation = new Vector3(CardB.Rotation.X, 180f, CardB.Rotation.Z);
						Script.Wait(1);
						randomCardPlayer35 = GetRandomCardPlayer();
						PlayerCard_3 = Set_PlayerCardPoker(randomCardPlayer35, 3);
						prop75 = World.CreateProp(randomCardPlayer35, offsetInWorldCoords82, true, false);
						prop75.IsPositionFrozen = true;
						PlayerCardC = prop75;
						position41 = prop75.Position;
						prop75.Position = new Vector3(position41.X, position41.Y, position41.Z - GetTableHeight_Cards(CurrentTable));
						prop75.Heading = Game.Player.Character.Heading;
						Chips.Add(prop75);
						Script.Wait(1);
						randomCardPlayer35 = GetRandomCardDealer();
						DealerCard_3 = Set_DealerCardPoker(randomCardPlayer35, 3);
						prop75 = World.CreateProp(randomCardPlayer35, offsetInWorldCoords74, true, false);
						prop75.IsPositionFrozen = true;
						position41 = prop75.Position;
						CardA = prop75;
						prop75.Position = new Vector3(position41.X, position41.Y, position41.Z - GetTableHeight_Cards(CurrentTable));
						prop75.Heading = Game.Player.Character.Heading;
						Chips.Add(prop75);
						CardA.Rotation = new Vector3(CardA.Rotation.X, 180f, CardA.Rotation.Z);
						int num96 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
						{
							Game.Player.Character.Position.X,
							Game.Player.Character.Position.Y,
							Game.Player.Character.Position.Z,
							0.0,
							0.0,
							Game.Player.Character.Heading,
							2
						});
						string dict41 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v02";
						string anim8 = "check_cards_bet";
						LoadDict(dict41);
						Ped character7 = Game.Player.Character;
						num96 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
						{
							character7.Position.X,
							character7.Position.Y,
							character7.Position.Z,
							0.0,
							0.0,
							character7.Rotation.Z,
							2
						});
						PlaySyncAnim(Game.Player.Character, Game.Player.Character, Scene2, dict41, anim8);
						Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
						{
							PlayerCardA,
							num96,
							"check_cards_bet_card_a",
							LoadDict(dict41),
							16f,
							-16f,
							0,
							1148846080
						});
						_8PlayerGamePokerPot += 1000f;
						Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
						{
							PlayerCardB,
							num96,
							"check_cards_bet_card_b",
							LoadDict(dict41),
							16f,
							-16f,
							0,
							1148846080
						});
						Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
						{
							PlayerCardC,
							num96,
							"check_cards_bet_card_c",
							LoadDict(dict41),
							16f,
							-16f,
							0,
							1148846080
						});
					}
				}
				if (!Placebet && Game.IsControlJustPressed(Control.Cover))
				{
					try
					{
						if (World.GetDistance(Game.Player.Character.Position, new Vector3(966.9417f, 32.0952f, 115f)) < 6f || World.GetDistance(Game.Player.Character.Position, new Vector3(1146.76f, 248.972f, -51f)) < 6f || World.GetDistance(Game.Player.Character.Position, new Vector3(1131.154f, 265.2554f, -51f)) < 6f)
						{
							string userInput6 = Game.GetUserInput(WindowTitle.EnterEyefindMessage, "5000", 5);
							Bet = float.Parse(userInput6);
						}
						else
						{
							string userInput7 = Game.GetUserInput(WindowTitle.EnterEyefindMessage, "5000", 5);
							Bet = float.Parse(userInput7);
						}
						if (Bet < Chips_Amount)
						{
							UI.Notify("Player bet :" + Bet);
							Placebet = true;
							if (Bet <= 10f)
							{
								Prop prop76 = World.CreateProp("vw_prop_chip_10dollar_st", new Vector3(offsetInWorldCoords86.X, offsetInWorldCoords86.Y, offsetInWorldCoords80.Z), true, false);
								prop76.IsPositionFrozen = true;
								prop76.Position = new Vector3(prop76.Position.X, prop76.Position.Y, prop76.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop76);
							}
							if (Bet > 10f && Bet <= 60f)
							{
								Prop prop77 = World.CreateProp("vw_prop_chip_50dollar_st", new Vector3(offsetInWorldCoords86.X, offsetInWorldCoords86.Y, offsetInWorldCoords80.Z), true, false);
								prop77.IsPositionFrozen = true;
								prop77.Position = new Vector3(prop77.Position.X, prop77.Position.Y, prop77.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop77);
							}
							if (Bet > 50f && Bet <= 100f)
							{
								Prop prop78 = World.CreateProp("vw_prop_chip_100dollar_st", new Vector3(offsetInWorldCoords86.X, offsetInWorldCoords86.Y, offsetInWorldCoords80.Z), true, false);
								prop78.IsPositionFrozen = true;
								prop78.Position = new Vector3(prop78.Position.X, prop78.Position.Y, prop78.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop78);
							}
							if (Bet > 100f && Bet <= 500f)
							{
								Prop prop79 = World.CreateProp("vw_prop_chip_500dollar_st", new Vector3(offsetInWorldCoords86.X, offsetInWorldCoords86.Y, offsetInWorldCoords80.Z), true, false);
								prop79.IsPositionFrozen = true;
								prop79.Position = new Vector3(prop79.Position.X, prop79.Position.Y, prop79.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop79);
							}
							if (Bet > 500f && Bet <= 1000f)
							{
								Prop prop80 = World.CreateProp("vw_prop_chip_1kdollar_st", new Vector3(offsetInWorldCoords86.X, offsetInWorldCoords86.Y, offsetInWorldCoords80.Z), true, false);
								prop80.IsPositionFrozen = true;
								prop80.Position = new Vector3(prop80.Position.X, prop80.Position.Y, prop80.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop80);
								Script.Wait(10);
							}
							if (Bet > 1000f && Bet <= 5000f)
							{
								Prop prop81 = World.CreateProp("vw_prop_plaq_5kdollar_st", new Vector3(offsetInWorldCoords86.X, offsetInWorldCoords86.Y, offsetInWorldCoords80.Z), true, false);
								prop81.IsPositionFrozen = true;
								prop81.Position = new Vector3(prop81.Position.X, prop81.Position.Y, prop81.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop81);
								Script.Wait(10);
							}
							if (Bet > 5000f && Bet <= 10000f)
							{
								Prop prop82 = World.CreateProp("vw_prop_plaq_10kdollar_st", new Vector3(offsetInWorldCoords86.X, offsetInWorldCoords86.Y, offsetInWorldCoords80.Z), true, false);
								prop82.IsPositionFrozen = true;
								prop82.Position = new Vector3(prop82.Position.X, prop82.Position.Y, prop82.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop82);
								Script.Wait(10);
							}
							if (Bet > 10000f && Bet <= 25000f)
							{
								Prop prop83 = World.CreateProp("vw_prop_plaq_10kdollar_st", new Vector3(offsetInWorldCoords86.X, offsetInWorldCoords86.Y, offsetInWorldCoords80.Z), true, false);
								prop83.IsPositionFrozen = true;
								prop83.Position = new Vector3(prop83.Position.X, prop83.Position.Y, prop83.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop83);
								Script.Wait(10);
								prop83 = World.CreateProp("vw_prop_vw_chips_pile_02a", new Vector3(offsetInWorldCoords87.X, offsetInWorldCoords87.Y, offsetInWorldCoords80.Z), true, false);
								prop83.IsPositionFrozen = true;
								prop83.Position = new Vector3(prop83.Position.X, prop83.Position.Y, prop83.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop83);
							}
							if (Bet > 25000f && Bet <= 50000f)
							{
								Prop prop84 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords86.X, offsetInWorldCoords86.Y, offsetInWorldCoords80.Z), true, false);
								prop84.IsPositionFrozen = true;
								prop84.Position = new Vector3(prop84.Position.X, prop84.Position.Y, prop84.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop84);
								prop84 = World.CreateProp("vw_prop_vw_chips_pile_02a", new Vector3(offsetInWorldCoords87.X, offsetInWorldCoords87.Y, offsetInWorldCoords80.Z), true, false);
								prop84.IsPositionFrozen = true;
								prop84.Position = new Vector3(prop84.Position.X, prop84.Position.Y, prop84.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop84);
							}
							if (Bet > 50000f && Bet <= 150000f)
							{
								Prop prop85 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords86.X, offsetInWorldCoords86.Y, offsetInWorldCoords80.Z), true, false);
								prop85.IsPositionFrozen = true;
								prop85.Position = new Vector3(prop85.Position.X, prop85.Position.Y, prop85.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop85);
								Script.Wait(10);
								prop85 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords87.X, offsetInWorldCoords87.Y, offsetInWorldCoords80.Z), true, false);
								prop85.IsPositionFrozen = true;
								prop85.Position = new Vector3(prop85.Position.X, prop85.Position.Y, prop85.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop85);
							}
							if (Bet > 50000f && Bet <= 150000f)
							{
								Prop prop86 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords86.X, offsetInWorldCoords86.Y, offsetInWorldCoords80.Z), true, false);
								prop86.IsPositionFrozen = true;
								prop86.Position = new Vector3(prop86.Position.X, prop86.Position.Y, prop86.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop86);
								Script.Wait(10);
								prop86 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords87.X, offsetInWorldCoords87.Y, offsetInWorldCoords80.Z), true, false);
								prop86.IsPositionFrozen = true;
								prop86.Position = new Vector3(prop86.Position.X, prop86.Position.Y, prop86.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop86);
								Vector3 offsetInWorldCoords97 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.8f, -0.45f));
								prop86 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords97.X, offsetInWorldCoords97.Y, offsetInWorldCoords80.Z), true, false);
								prop86.IsPositionFrozen = true;
								prop86.Position = new Vector3(prop86.Position.X, prop86.Position.Y, prop86.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop86);
								Vector3 offsetInWorldCoords98 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.9f, -0.45f));
								prop86 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords98.X, offsetInWorldCoords98.Y, offsetInWorldCoords80.Z), true, false);
								prop86.IsPositionFrozen = true;
								prop86.Position = new Vector3(prop86.Position.X, prop86.Position.Y, prop86.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop86);
							}
							if (Bet > 150000f && Bet < 5000000f)
							{
								Prop prop87 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords86.X, offsetInWorldCoords86.Y, offsetInWorldCoords80.Z), true, false);
								prop87.IsPositionFrozen = true;
								prop87.Position = new Vector3(prop87.Position.X, prop87.Position.Y, prop87.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop87);
								Script.Wait(10);
								prop87 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords87.X, offsetInWorldCoords87.Y, offsetInWorldCoords80.Z), true, false);
								prop87.IsPositionFrozen = true;
								prop87.Position = new Vector3(prop87.Position.X, prop87.Position.Y, prop87.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop87);
								Vector3 offsetInWorldCoords99 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.8f, -0.45f));
								prop87 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords99.X, offsetInWorldCoords99.Y, offsetInWorldCoords80.Z), true, false);
								prop87.IsPositionFrozen = true;
								prop87.Position = new Vector3(prop87.Position.X, prop87.Position.Y, prop87.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop87);
								Vector3 offsetInWorldCoords100 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.9f, -0.45f));
								prop87 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords100.X, offsetInWorldCoords100.Y, offsetInWorldCoords80.Z), true, false);
								prop87.IsPositionFrozen = true;
								prop87.Position = new Vector3(prop87.Position.X, prop87.Position.Y, prop87.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop87);
								Vector3 offsetInWorldCoords101 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.6f, -0.4f));
								Vector3 offsetInWorldCoords102 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.7f, -0.4f));
								prop87 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords101.X, offsetInWorldCoords101.Y, offsetInWorldCoords80.Z), true, false);
								prop87.IsPositionFrozen = true;
								prop87.Position = new Vector3(prop87.Position.X, prop87.Position.Y, prop87.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop87);
								Script.Wait(10);
								prop87 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords102.X, offsetInWorldCoords102.Y, offsetInWorldCoords80.Z), true, false);
								prop87.IsPositionFrozen = true;
								prop87.Position = new Vector3(prop87.Position.X, prop87.Position.Y, prop87.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop87);
								Vector3 offsetInWorldCoords103 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.8f, -0.4f));
								prop87 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords103.X, offsetInWorldCoords103.Y, offsetInWorldCoords80.Z), true, false);
								prop87.IsPositionFrozen = true;
								prop87.Position = new Vector3(prop87.Position.X, prop87.Position.Y, prop87.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop87);
								Vector3 offsetInWorldCoords104 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.9f, -0.4f));
								prop87 = World.CreateProp("vw_prop_vw_chips_pile_03a", new Vector3(offsetInWorldCoords104.X, offsetInWorldCoords104.Y, offsetInWorldCoords80.Z), true, false);
								prop87.IsPositionFrozen = true;
								prop87.Position = new Vector3(prop87.Position.X, prop87.Position.Y, prop87.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop87);
							}
							if (Bet > 4999999f)
							{
								Vector3 offsetInWorldCoords105 = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.4f, 0.6f, -0.5f));
								Prop prop88 = World.CreateProp("prop_gold_bar", new Vector3(offsetInWorldCoords105.X, offsetInWorldCoords105.Y, offsetInWorldCoords80.Z), true, false);
								prop88.IsPositionFrozen = true;
								prop88.Position = new Vector3(prop88.Position.X, prop88.Position.Y, prop88.Position.Z + GetTableHeight_Chips(CurrentTable));
								Chips.Add(prop88);
								Script.Wait(10);
							}
						}
						if (Bet > Chips_Amount)
						{
							UI.Notify("you do not have enough chips for this bet");
							Placebet = false;
						}
						Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_male@engaged@01a@play@v02", "place_bet", 8f, -1, true, -1f);
						Script.Wait(1000);
						Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base", "base", 8f, -1, true, -1f);
					}
					catch (Exception)
					{
						Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base", "base", 8f, -1, true, -1f);
						UI.Notify("Player did not enter a valid bet");
					}
				}
			}
			for (int num97 = 0; num97 < PokerTables.Count; num97++)
			{
				if (!(World.GetDistance(Game.Player.Character.Position, PokerTables[num97]) < 2f))
				{
					continue;
				}
				if (!Playing_Poker && !Placebet)
				{
					if (!Game.IsControlJustPressed(Control.Context))
					{
						continue;
					}
					Game.Player.Character.Position = PokerTables[num97];
					string enterAnim6 = GetEnterAnim();
					string dict42 = "anim_casino_b@amb@casino@games@shared@player@";
					LoadDict("anim_casino_b@amb@casino@games@shared@player@");
					LoadDict(dict42);
					int num98 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
					{
						Game.Player.Character.Position.X,
						Game.Player.Character.Position.Y,
						PokerTables[num97].Z - 0.25f,
						0.0,
						0.0,
						PokerTablesRot[num97] + 90f,
						2
					});
					Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
					{
						Game.Player.Character,
						num98,
						LoadDict(dict42),
						"sit_enter_left",
						16f,
						-16f,
						0,
						1148846080
					});
					Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
					{
						Game.Player.Character,
						num98,
						"sit_enter_left",
						LoadDict(dict42),
						16f,
						-16f,
						0,
						1148846080
					});
					Editbet = false;
					Placebet = false;
					Drawcards = false;
					Endgame = false;
					Playercards = 0;
					DealerCards = 0;
					PlayerCard_1 = 0;
					PlayerCard_2 = 0;
					PlayerCard_3 = 0;
					PlayerCard_4 = 0;
					PlayerCard_5 = 0;
					PlayerCard_6 = 0;
					Bet = 0f;
					DealerCard_1 = 0;
					DealerCard_2 = 0;
					DealerCard_3 = 0;
					DealerCard_4 = 0;
					DealerCard_5 = 0;
					DealerCard_6 = 0;
					CurrentTable = num97;
					foreach (Prop chip14 in Chips)
					{
						if (chip14 != null)
						{
							chip14.Delete();
						}
					}
					Playing_Poker = true;
				}
				else
				{
					if (!Playing_Poker || Placebet || !Game.IsControlJustPressed(Control.Context))
					{
						continue;
					}
					CurrentTable = num97;
					string enterAnim7 = GetEnterAnim();
					string dict43 = "anim_casino_b@amb@casino@games@shared@player@";
					LoadDict(dict43);
					int num99 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
					{
						Game.Player.Character.Position.X,
						Game.Player.Character.Position.Y,
						PokerTables[num97].Z - 0.25f,
						0.0,
						0.0,
						PokerTablesRot[num97] + 90f,
						2
					});
					Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
					{
						Game.Player.Character,
						num99,
						LoadDict(dict43),
						"sit_exit_left",
						16f,
						-16f,
						0,
						1148846080
					});
					Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
					{
						num99,
						Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { num99 })
					});
					Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
					{
						Game.Player.Character,
						num99,
						"sit_exit_left",
						LoadDict(dict43),
						16f,
						-16f,
						0,
						1148846080
					});
					Script.Wait(4000);
					Game.Player.Character.Task.ClearAll();
					Game.Player.Character.IsPositionFrozen = false;
					foreach (Prop chip15 in Chips)
					{
						if (chip15 != null)
						{
							chip15.Delete();
						}
					}
					Playing_Poker = false;
					foreach (Prop chip16 in Chips)
					{
						if (chip16 != null)
						{
							chip16.Delete();
						}
					}
					Editbet = false;
					Placebet = false;
					Drawcards = false;
					Endgame = false;
					Playercards = 0;
					DealerCards = 0;
					PlayerCard_1 = 0;
					PlayerCard_2 = 0;
					PlayerCard_3 = 0;
					PlayerCard_4 = 0;
					PlayerCard_5 = 0;
					PlayerCard_6 = 0;
					Bet = 0f;
					DealerCard_1 = 0;
					DealerCard_2 = 0;
					DealerCard_3 = 0;
					DealerCard_4 = 0;
					DealerCard_5 = 0;
					DealerCard_6 = 0;
				}
			}
			if (Playing_Roulette)
			{
				if (RouletteTableA != null && World.GetDistance(Game.Player.Character.Position, RouletteTableA.Position) < 5f)
				{
					TableT = RouletteTableA;
					Prop tableT = TableT;
					RoulleteTable = "A";
				}
				if (RouletteTableB != null && World.GetDistance(Game.Player.Character.Position, RouletteTableB.Position) < 5f)
				{
					TableT = RouletteTableB;
					Prop tableT2 = TableT;
					RoulleteTable = "B";
				}
				if (RouletteTableC != null)
				{
					if (World.GetDistance(Game.Player.Character.Position, RouletteTableC.Position) < 5f)
					{
						TableT = RouletteTableC;
						Prop tableT3 = TableT;
						RoulleteTable = "C";
					}
				}
				else
				{
					UI.Notify("~DC&R MAJOR Error ~W~ : One of The Roulette Tables has failed to spawn, please restart your game");
				}
				if (TableT != null)
				{
					Prop tableT4 = TableT;
					float num100 = 0.95f;
					if (MarkerNum < 3)
					{
						ShowRouletteOptions(TableT);
					}
					if (MarkerNum == 3)
					{
						ShowRouletteOptionsMid(TableT);
					}
					if (MarkerNum == 4)
					{
						ShowRouletteOptionsEnd(TableT);
					}
					if (MarkerNum == 5)
					{
						ShowRouletteOptionsEndGame(TableT);
						if (Game.IsControlJustPressed(Control.ThrowGrenade))
						{
							Editbet = false;
							Placebet = false;
							Drawcards = false;
							TableT = null;
							Endgame = false;
							CreatedMarker = false;
							PlaceBet_R = false;
							Playercards = 0;
							DealerCards = 0;
							PlayerCard_1 = 0;
							PlayerCard_2 = 0;
							PlayerCard_3 = 0;
							PlayerCard_4 = 0;
							PlayerCard_5 = 0;
							PlayerCard_6 = 0;
							Bet = 0f;
							DealerCard_1 = 0;
							DealerCard_2 = 0;
							DealerCard_3 = 0;
							DealerCard_4 = 0;
							DealerCard_5 = 0;
							DealerCard_6 = 0;
							BetPos = "None";
							BetPosNum = 0;
							MarkerNum = 0;
							if (RouletteMarker2 != null)
							{
								RouletteMarker2.Delete();
							}
							if (RouletteMarker1 != null)
							{
								RouletteMarker1.Delete();
							}
							if (RouletteMarker != null)
							{
								RouletteMarker.Delete();
							}
							MarkerNum = 0;
							RouletteBet1 = 0f;
							RouletteBet2 = 0f;
							RouletteBet3 = 0f;
							foreach (Prop chip17 in Chips)
							{
								if (chip17 != null)
								{
									chip17.Delete();
								}
							}
							Game.Player.Character.PlayAnimationOld("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base", "base", 8f, -1, true, -1f);
							Playing_Roulette = true;
						}
					}
					if (Ball != null)
					{
						World.DrawMarker(MarkerType.VerticalCylinder, Ball.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.01f, 0.01f, 0.25f), Color.Blue);
					}
					float z = 0.95f;
					Vector3 offsetInWorldCoords106 = tableT4.GetOffsetInWorldCoords(new Vector3(-0.14f, 0.11f, z));
					Vector3 offsetInWorldCoords107 = tableT4.GetOffsetInWorldCoords(new Vector3(-0.14f, -0.14f, z));
					Vector3 offsetInWorldCoords108 = tableT4.GetOffsetInWorldCoords(new Vector3(-0.05f, -0.185f, z));
					Vector3 offsetInWorldCoords109 = tableT4.GetOffsetInWorldCoords(new Vector3(-0.05f, -0.025f, z));
					Vector3 offsetInWorldCoords110 = tableT4.GetOffsetInWorldCoords(new Vector3(-0.05f, 0.14f, z));
					Vector3 offsetInWorldCoords111 = tableT4.GetOffsetInWorldCoords(new Vector3(0.03f, -0.185f, z));
					Vector3 offsetInWorldCoords112 = tableT4.GetOffsetInWorldCoords(new Vector3(0.03f, -0.025f, z));
					Vector3 offsetInWorldCoords113 = tableT4.GetOffsetInWorldCoords(new Vector3(0.03f, 0.14f, z));
					Vector3 offsetInWorldCoords114 = tableT4.GetOffsetInWorldCoords(new Vector3(0.1f, -0.185f, z));
					Vector3 offsetInWorldCoords115 = tableT4.GetOffsetInWorldCoords(new Vector3(0.1f, -0.025f, z));
					Vector3 offsetInWorldCoords116 = tableT4.GetOffsetInWorldCoords(new Vector3(0.1f, 0.14f, z));
					Vector3 offsetInWorldCoords117 = tableT4.GetOffsetInWorldCoords(new Vector3(0.19f, -0.185f, z));
					Vector3 offsetInWorldCoords118 = tableT4.GetOffsetInWorldCoords(new Vector3(0.19f, -0.025f, z));
					Vector3 offsetInWorldCoords119 = tableT4.GetOffsetInWorldCoords(new Vector3(0.19f, 0.14f, z));
					Vector3 offsetInWorldCoords120 = tableT4.GetOffsetInWorldCoords(new Vector3(0.27f, -0.185f, z));
					Vector3 offsetInWorldCoords121 = tableT4.GetOffsetInWorldCoords(new Vector3(0.27f, -0.025f, z));
					Vector3 offsetInWorldCoords122 = tableT4.GetOffsetInWorldCoords(new Vector3(0.27f, 0.14f, z));
					Vector3 offsetInWorldCoords123 = tableT4.GetOffsetInWorldCoords(new Vector3(0.35f, -0.185f, z));
					Vector3 offsetInWorldCoords124 = tableT4.GetOffsetInWorldCoords(new Vector3(0.35f, -0.025f, z));
					Vector3 offsetInWorldCoords125 = tableT4.GetOffsetInWorldCoords(new Vector3(0.35f, 0.14f, z));
					Vector3 offsetInWorldCoords126 = tableT4.GetOffsetInWorldCoords(new Vector3(0.43f, -0.185f, z));
					Vector3 offsetInWorldCoords127 = tableT4.GetOffsetInWorldCoords(new Vector3(0.43f, -0.025f, z));
					Vector3 offsetInWorldCoords128 = tableT4.GetOffsetInWorldCoords(new Vector3(0.43f, 0.14f, z));
					Vector3 offsetInWorldCoords129 = tableT4.GetOffsetInWorldCoords(new Vector3(0.51f, -0.185f, z));
					Vector3 offsetInWorldCoords130 = tableT4.GetOffsetInWorldCoords(new Vector3(0.51f, -0.025f, z));
					Vector3 offsetInWorldCoords131 = tableT4.GetOffsetInWorldCoords(new Vector3(0.51f, 0.14f, z));
					Vector3 offsetInWorldCoords132 = tableT4.GetOffsetInWorldCoords(new Vector3(0.59f, -0.185f, z));
					Vector3 offsetInWorldCoords133 = tableT4.GetOffsetInWorldCoords(new Vector3(0.59f, -0.025f, z));
					Vector3 offsetInWorldCoords134 = tableT4.GetOffsetInWorldCoords(new Vector3(0.59f, 0.14f, z));
					Vector3 offsetInWorldCoords135 = tableT4.GetOffsetInWorldCoords(new Vector3(0.67f, -0.185f, z));
					Vector3 offsetInWorldCoords136 = tableT4.GetOffsetInWorldCoords(new Vector3(0.67f, -0.025f, z));
					Vector3 offsetInWorldCoords137 = tableT4.GetOffsetInWorldCoords(new Vector3(0.67f, 0.14f, z));
					Vector3 offsetInWorldCoords138 = tableT4.GetOffsetInWorldCoords(new Vector3(0.75f, -0.185f, z));
					Vector3 offsetInWorldCoords139 = tableT4.GetOffsetInWorldCoords(new Vector3(0.75f, -0.025f, z));
					Vector3 offsetInWorldCoords140 = tableT4.GetOffsetInWorldCoords(new Vector3(0.75f, 0.14f, z));
					Vector3 offsetInWorldCoords141 = tableT4.GetOffsetInWorldCoords(new Vector3(0.83f, -0.185f, z));
					Vector3 offsetInWorldCoords142 = tableT4.GetOffsetInWorldCoords(new Vector3(0.83f, -0.025f, z));
					Vector3 offsetInWorldCoords143 = tableT4.GetOffsetInWorldCoords(new Vector3(0.83f, 0.14f, z));
					Vector3 offsetInWorldCoords144 = tableT4.GetOffsetInWorldCoords(new Vector3(0.91f, -0.185f, z));
					Vector3 offsetInWorldCoords145 = tableT4.GetOffsetInWorldCoords(new Vector3(0.91f, -0.025f, z));
					Vector3 offsetInWorldCoords146 = tableT4.GetOffsetInWorldCoords(new Vector3(0.91f, 0.14f, z));
					Vector3 offsetInWorldCoords147 = tableT4.GetOffsetInWorldCoords(new Vector3(0.83f, -0.35f, z));
					Vector3 offsetInWorldCoords148 = tableT4.GetOffsetInWorldCoords(new Vector3(0.59f, -0.35f, z));
					Vector3 offsetInWorldCoords149 = tableT4.GetOffsetInWorldCoords(new Vector3(-0.05f, -0.35f, z));
					Vector3 offsetInWorldCoords150 = tableT4.GetOffsetInWorldCoords(new Vector3(0.19f, -0.35f, z));
					Vector3 offsetInWorldCoords151 = tableT4.GetOffsetInWorldCoords(new Vector3(0.27f, -0.35f, z));
					Vector3 offsetInWorldCoords152 = tableT4.GetOffsetInWorldCoords(new Vector3(0.51f, -0.35f, z));
					Vector3 offsetInWorldCoords153 = tableT4.GetOffsetInWorldCoords(new Vector3(0.27f, -0.3f, z));
					Vector3 offsetInWorldCoords154 = tableT4.GetOffsetInWorldCoords(new Vector3(-0.05f, -0.3f, z));
					Vector3 offsetInWorldCoords155 = tableT4.GetOffsetInWorldCoords(new Vector3(0.83f, -0.3f, z));
					Vector3 offsetInWorldCoords156 = tableT4.GetOffsetInWorldCoords(new Vector3(-0.14f, -0.3f, z));
					if (!CreatedMarker)
					{
						if (Ball != null)
						{
							Ball.Delete();
						}
						Vector3 offsetInWorldCoords157 = tableT4.GetOffsetInWorldCoords(new Vector3(-1f, -0.36f, 1.06f));
						if (RoulleteTable.Equals("A"))
						{
							offsetInWorldCoords157 = tableT4.GetOffsetInWorldCoords(new Vector3(-0.65f, -0.45f, 1.06f));
						}
						if (RoulleteTable.Equals("B"))
						{
							offsetInWorldCoords157 = tableT4.GetOffsetInWorldCoords(new Vector3(-0.42f, -0.1f, 1.06f));
						}
						if (RoulleteTable.Equals("C"))
						{
							offsetInWorldCoords157 = tableT4.GetOffsetInWorldCoords(new Vector3(-1f, -0.36f, 1.06f));
						}
						Ball = World.CreateProp(RequestModel("vw_prop_roulette_ball"), offsetInWorldCoords157, new Vector3(0f, 0f, 0f), false, false);
						Ball.IsPositionFrozen = true;
						Ball.IsCollisionEnabled = false;
						CreatedMarker = true;
						RouletteMarker = World.CreateProp(SpawnProp("vw_prop_roulette_marker"), offsetInWorldCoords156, true, false);
						RouletteMarker.IsPositionFrozen = false;
						RouletteMarker1 = World.CreateProp(SpawnProp("vw_prop_roulette_marker"), offsetInWorldCoords156, true, false);
						RouletteMarker1.IsPositionFrozen = false;
						RouletteMarker2 = World.CreateProp(SpawnProp("vw_prop_roulette_marker"), offsetInWorldCoords156, true, false);
						RouletteMarker2.IsPositionFrozen = false;
					}
					if (MarkerNum == 4 && Game.IsControlJustPressed(Control.Cover))
					{
						Roulette_Choose_Num(tableT4);
					}
					if (MarkerNum == 0 && RouletteMarker1 != null)
					{
						World.DrawMarker(MarkerType.VerticalCylinder, RouletteMarker1.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.01f, 0.01f, 0.25f), Color.Red);
					}
					if (MarkerNum == 3)
					{
						if (Game.IsControlJustPressed(Control.Cover))
						{
							if (RouletteBet1 + RouletteBet2 + RouletteBet3 < Chips_Amount)
							{
								MarkerNum++;
							}
							if (RouletteBet1 + RouletteBet2 + RouletteBet3 > Chips_Amount)
							{
								DisplayHelpTextThisFrame("The Bet Amount Exceeds your Amount of Chips, please go lower");
							}
						}
						if (RouletteMarker1 != null)
						{
							World.DrawMarker(MarkerType.VerticalCylinder, RouletteMarker1.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.01f, 0.01f, 0.25f), Color.Red);
						}
						if (RouletteMarker2 != null)
						{
							World.DrawMarker(MarkerType.VerticalCylinder, RouletteMarker2.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.01f, 0.01f, 0.25f), Color.Green);
						}
						if (RouletteMarker != null)
						{
							World.DrawMarker(MarkerType.VerticalCylinder, RouletteMarker.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.01f, 0.01f, 0.25f), Color.Blue);
						}
						if (Game.IsControlJustPressed(Control.VehicleCinCam))
						{
							try
							{
								string userInput8 = OldApi.GetUserInput(5);
								RouletteBet1 = float.Parse(userInput8);
								UI.Notify("~g~Green~w~ Bet, set to :" + RouletteBet1);
							}
							catch (Exception)
							{
								UI.Notify("Player did not enter a valid bet");
							}
						}
						if (Game.IsControlJustPressed(Control.ThrowGrenade))
						{
							try
							{
								string userInput9 = OldApi.GetUserInput(5);
								RouletteBet2 = float.Parse(userInput9);
								UI.Notify("~b~Blue~w~ Bet, set to :" + RouletteBet2);
							}
							catch (Exception)
							{
								UI.Notify("Player did not enter a valid bet");
							}
						}
						if (Game.IsControlJustPressed(Control.VehicleExit))
						{
							try
							{
								string userInput10 = OldApi.GetUserInput(5);
								RouletteBet3 = float.Parse(userInput10);
								UI.Notify("~r~Red~w~ Bed, set to :" + RouletteBet3);
							}
							catch (Exception)
							{
								UI.Notify("Player did not enter a valid bet");
							}
						}
					}
					if (MarkerNum < 3 && MarkerTomove != null)
					{
						World.DrawMarker(MarkerType.VerticalCylinder, MarkerTomove.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.01f, 0.01f, 0.25f), Color.Red);
					}
					if (MarkerNum < 3)
					{
						if (Game.IsControlJustPressed(Control.PhoneRight))
						{
							UI.Notify("test1");
							if (BetPosNum >= 0)
							{
								BetPosNum++;
							}
							if (BetPosNum > 50)
							{
								BetPosNum = 0;
							}
							if (BetPosNum < 0)
							{
								BetPosNum = 50;
							}
							if (MarkerNum == 0)
							{
								MarkerTomove = RouletteMarker;
							}
							if (MarkerNum == 1)
							{
								MarkerTomove = RouletteMarker2;
							}
							if (MarkerNum == 2)
							{
								MarkerTomove = RouletteMarker1;
							}
							if (BetPosNum == 0)
							{
								BetPos = "None";
								MarkerTomove.Position = offsetInWorldCoords156;
							}
							if (MarkerTomove == null)
							{
								UI.Notify("MK null");
							}
							if (BetPosNum == 1)
							{
								BetPos = "0";
								MarkerTomove.Position = offsetInWorldCoords107;
							}
							if (BetPosNum == 2)
							{
								BetPos = "00";
								MarkerTomove.Position = offsetInWorldCoords106;
							}
							if (BetPosNum == 3)
							{
								BetPos = "Red, 1";
								MarkerTomove.Position = offsetInWorldCoords108;
							}
							if (BetPosNum == 4)
							{
								BetPos = "Black, 2";
								MarkerTomove.Position = offsetInWorldCoords109;
							}
							if (BetPosNum == 5)
							{
								BetPos = "Red, 3";
								MarkerTomove.Position = offsetInWorldCoords110;
							}
							if (BetPosNum == 6)
							{
								BetPos = "Black, 4";
								MarkerTomove.Position = offsetInWorldCoords111;
							}
							if (BetPosNum == 7)
							{
								BetPos = "Red, 5";
								MarkerTomove.Position = offsetInWorldCoords112;
							}
							if (BetPosNum == 8)
							{
								BetPos = "Black, 6";
								MarkerTomove.Position = offsetInWorldCoords113;
							}
							if (BetPosNum == 9)
							{
								BetPos = "Red, 7";
								MarkerTomove.Position = offsetInWorldCoords114;
							}
							if (BetPosNum == 10)
							{
								BetPos = "Black, 8";
								MarkerTomove.Position = offsetInWorldCoords115;
							}
							if (BetPosNum == 11)
							{
								BetPos = "Red, 9";
								MarkerTomove.Position = offsetInWorldCoords116;
							}
							if (BetPosNum == 12)
							{
								BetPos = "Black, 10";
								MarkerTomove.Position = offsetInWorldCoords117;
							}
							if (BetPosNum == 13)
							{
								BetPos = "Black, 11";
								MarkerTomove.Position = offsetInWorldCoords118;
							}
							if (BetPosNum == 14)
							{
								BetPos = "Red, 12";
								MarkerTomove.Position = offsetInWorldCoords119;
							}
							if (BetPosNum == 15)
							{
								BetPos = "Black, 13";
								MarkerTomove.Position = offsetInWorldCoords120;
							}
							if (BetPosNum == 16)
							{
								BetPos = "Red, 14";
								MarkerTomove.Position = offsetInWorldCoords121;
							}
							if (BetPosNum == 17)
							{
								BetPos = "Black, 15";
								MarkerTomove.Position = offsetInWorldCoords122;
							}
							if (BetPosNum == 18)
							{
								BetPos = "Red, 16";
								MarkerTomove.Position = offsetInWorldCoords123;
							}
							if (BetPosNum == 19)
							{
								BetPos = "Black, 17";
								MarkerTomove.Position = offsetInWorldCoords124;
							}
							if (BetPosNum == 20)
							{
								BetPos = "Red, 18";
								MarkerTomove.Position = offsetInWorldCoords125;
							}
							if (BetPosNum == 21)
							{
								BetPos = "Red, 19";
								MarkerTomove.Position = offsetInWorldCoords126;
							}
							if (BetPosNum == 22)
							{
								BetPos = "Black, 20";
								MarkerTomove.Position = offsetInWorldCoords127;
							}
							if (BetPosNum == 23)
							{
								BetPos = "Red, 21";
								MarkerTomove.Position = offsetInWorldCoords128;
							}
							if (BetPosNum == 24)
							{
								BetPos = "Black, 22";
								MarkerTomove.Position = offsetInWorldCoords129;
							}
							if (BetPosNum == 25)
							{
								BetPos = "Red, 23";
								MarkerTomove.Position = offsetInWorldCoords130;
							}
							if (BetPosNum == 26)
							{
								BetPos = "Black, 24";
								MarkerTomove.Position = offsetInWorldCoords131;
							}
							if (BetPosNum == 27)
							{
								BetPos = "Red, 25";
								MarkerTomove.Position = offsetInWorldCoords132;
							}
							if (BetPosNum == 28)
							{
								BetPos = "Black, 26";
								MarkerTomove.Position = offsetInWorldCoords133;
							}
							if (BetPosNum == 29)
							{
								BetPos = "Red, 27";
								MarkerTomove.Position = offsetInWorldCoords134;
							}
							if (BetPosNum == 30)
							{
								BetPos = "Black, 28";
								MarkerTomove.Position = offsetInWorldCoords135;
							}
							if (BetPosNum == 31)
							{
								BetPos = "Black, 29";
								MarkerTomove.Position = offsetInWorldCoords136;
							}
							if (BetPosNum == 32)
							{
								BetPos = "Red, 30";
								MarkerTomove.Position = offsetInWorldCoords137;
							}
							if (BetPosNum == 33)
							{
								BetPos = "Black, 31";
								MarkerTomove.Position = offsetInWorldCoords138;
							}
							if (BetPosNum == 34)
							{
								BetPos = "Red, 32";
								MarkerTomove.Position = offsetInWorldCoords139;
							}
							if (BetPosNum == 35)
							{
								BetPos = "Black, 33";
								MarkerTomove.Position = offsetInWorldCoords140;
							}
							if (BetPosNum == 36)
							{
								BetPos = "Red, 34";
								MarkerTomove.Position = offsetInWorldCoords141;
							}
							if (BetPosNum == 37)
							{
								BetPos = "Black, 35";
								MarkerTomove.Position = offsetInWorldCoords142;
							}
							if (BetPosNum == 38)
							{
								BetPos = "Red, 36";
								MarkerTomove.Position = offsetInWorldCoords143;
							}
							if (BetPosNum == 39)
							{
								BetPos = "1st Column";
								MarkerTomove.Position = offsetInWorldCoords144;
							}
							if (BetPosNum == 40)
							{
								BetPos = "2nd Column";
								MarkerTomove.Position = offsetInWorldCoords145;
							}
							if (BetPosNum == 41)
							{
								BetPos = "3rd Column";
								MarkerTomove.Position = offsetInWorldCoords146;
							}
							if (BetPosNum == 42)
							{
								BetPos = "3rd 12";
								MarkerTomove.Position = offsetInWorldCoords155;
							}
							if (BetPosNum == 43)
							{
								BetPos = "2nd 12";
								MarkerTomove.Position = offsetInWorldCoords154;
							}
							if (BetPosNum == 44)
							{
								BetPos = "1st 12";
								MarkerTomove.Position = offsetInWorldCoords153;
							}
							if (BetPosNum == 45)
							{
								BetPos = "1 to 18";
								MarkerTomove.Position = offsetInWorldCoords149;
							}
							if (BetPosNum == 46)
							{
								BetPos = "Even";
								MarkerTomove.Position = offsetInWorldCoords150;
							}
							if (BetPosNum == 47)
							{
								BetPos = "Red";
								MarkerTomove.Position = offsetInWorldCoords151;
							}
							if (BetPosNum == 48)
							{
								BetPos = "Black";
								MarkerTomove.Position = offsetInWorldCoords152;
							}
							if (BetPosNum == 49)
							{
								BetPos = "Odd";
								MarkerTomove.Position = offsetInWorldCoords148;
							}
							if (BetPosNum == 50)
							{
								BetPos = "19 to 36";
								MarkerTomove.Position = offsetInWorldCoords147;
							}
						}
						if (Game.IsControlJustPressed(Control.PhoneLeft))
						{
							UI.Notify("test2");
							if (BetPosNum < 51)
							{
								BetPosNum--;
							}
							if (BetPosNum > 50)
							{
								BetPosNum = 0;
							}
							if (BetPosNum < 0)
							{
								BetPosNum = 50;
							}
							if (MarkerNum == 0)
							{
								MarkerTomove = RouletteMarker;
							}
							if (MarkerNum == 1)
							{
								MarkerTomove = RouletteMarker2;
							}
							if (MarkerNum == 2)
							{
								MarkerTomove = RouletteMarker1;
							}
							if (BetPosNum == 0)
							{
								BetPos = "None";
								MarkerTomove.Position = offsetInWorldCoords156;
							}
							if (MarkerTomove == null)
							{
								UI.Notify("MK null");
							}
							if (BetPosNum == 1)
							{
								BetPos = "0";
								MarkerTomove.Position = offsetInWorldCoords107;
							}
							if (BetPosNum == 2)
							{
								BetPos = "00";
								MarkerTomove.Position = offsetInWorldCoords106;
							}
							if (BetPosNum == 3)
							{
								BetPos = "Red, 1";
								MarkerTomove.Position = offsetInWorldCoords108;
							}
							if (BetPosNum == 4)
							{
								BetPos = "Black, 2";
								MarkerTomove.Position = offsetInWorldCoords109;
							}
							if (BetPosNum == 5)
							{
								BetPos = "Red, 3";
								MarkerTomove.Position = offsetInWorldCoords110;
							}
							if (BetPosNum == 6)
							{
								BetPos = "Black, 4";
								MarkerTomove.Position = offsetInWorldCoords111;
							}
							if (BetPosNum == 7)
							{
								BetPos = "Red, 5";
								MarkerTomove.Position = offsetInWorldCoords112;
							}
							if (BetPosNum == 8)
							{
								BetPos = "Black, 6";
								MarkerTomove.Position = offsetInWorldCoords113;
							}
							if (BetPosNum == 9)
							{
								BetPos = "Red, 7";
								MarkerTomove.Position = offsetInWorldCoords114;
							}
							if (BetPosNum == 10)
							{
								BetPos = "Black, 8";
								MarkerTomove.Position = offsetInWorldCoords115;
							}
							if (BetPosNum == 11)
							{
								BetPos = "Red, 9";
								MarkerTomove.Position = offsetInWorldCoords116;
							}
							if (BetPosNum == 12)
							{
								BetPos = "Black, 10";
								MarkerTomove.Position = offsetInWorldCoords117;
							}
							if (BetPosNum == 13)
							{
								BetPos = "Black, 11";
								MarkerTomove.Position = offsetInWorldCoords118;
							}
							if (BetPosNum == 14)
							{
								BetPos = "Red, 12";
								MarkerTomove.Position = offsetInWorldCoords119;
							}
							if (BetPosNum == 15)
							{
								BetPos = "Black, 13";
								MarkerTomove.Position = offsetInWorldCoords120;
							}
							if (BetPosNum == 16)
							{
								BetPos = "Red, 14";
								MarkerTomove.Position = offsetInWorldCoords121;
							}
							if (BetPosNum == 17)
							{
								BetPos = "Black, 15";
								MarkerTomove.Position = offsetInWorldCoords122;
							}
							if (BetPosNum == 18)
							{
								BetPos = "Red, 16";
								MarkerTomove.Position = offsetInWorldCoords123;
							}
							if (BetPosNum == 19)
							{
								BetPos = "Black, 17";
								MarkerTomove.Position = offsetInWorldCoords124;
							}
							if (BetPosNum == 20)
							{
								BetPos = "Red, 18";
								MarkerTomove.Position = offsetInWorldCoords125;
							}
							if (BetPosNum == 21)
							{
								BetPos = "Red, 19";
								MarkerTomove.Position = offsetInWorldCoords126;
							}
							if (BetPosNum == 22)
							{
								BetPos = "Black, 20";
								MarkerTomove.Position = offsetInWorldCoords127;
							}
							if (BetPosNum == 23)
							{
								BetPos = "Red, 21";
								MarkerTomove.Position = offsetInWorldCoords128;
							}
							if (BetPosNum == 24)
							{
								BetPos = "Black, 22";
								MarkerTomove.Position = offsetInWorldCoords129;
							}
							if (BetPosNum == 25)
							{
								BetPos = "Red, 23";
								MarkerTomove.Position = offsetInWorldCoords130;
							}
							if (BetPosNum == 26)
							{
								BetPos = "Black, 24";
								MarkerTomove.Position = offsetInWorldCoords131;
							}
							if (BetPosNum == 27)
							{
								BetPos = "Red, 25";
								MarkerTomove.Position = offsetInWorldCoords132;
							}
							if (BetPosNum == 28)
							{
								BetPos = "Black, 26";
								MarkerTomove.Position = offsetInWorldCoords133;
							}
							if (BetPosNum == 29)
							{
								BetPos = "Red, 27";
								MarkerTomove.Position = offsetInWorldCoords134;
							}
							if (BetPosNum == 30)
							{
								BetPos = "Black, 28";
								MarkerTomove.Position = offsetInWorldCoords135;
							}
							if (BetPosNum == 31)
							{
								BetPos = "Black, 29";
								MarkerTomove.Position = offsetInWorldCoords136;
							}
							if (BetPosNum == 32)
							{
								BetPos = "Red, 30";
								MarkerTomove.Position = offsetInWorldCoords137;
							}
							if (BetPosNum == 33)
							{
								BetPos = "Black, 31";
								MarkerTomove.Position = offsetInWorldCoords138;
							}
							if (BetPosNum == 34)
							{
								BetPos = "Red, 32";
								MarkerTomove.Position = offsetInWorldCoords139;
							}
							if (BetPosNum == 35)
							{
								BetPos = "Black, 33";
								MarkerTomove.Position = offsetInWorldCoords140;
							}
							if (BetPosNum == 36)
							{
								BetPos = "Red, 34";
								MarkerTomove.Position = offsetInWorldCoords141;
							}
							if (BetPosNum == 37)
							{
								BetPos = "Black, 35";
								MarkerTomove.Position = offsetInWorldCoords142;
							}
							if (BetPosNum == 38)
							{
								BetPos = "Red, 36";
								MarkerTomove.Position = offsetInWorldCoords143;
							}
							if (BetPosNum == 39)
							{
								BetPos = "1st Column";
								MarkerTomove.Position = offsetInWorldCoords144;
							}
							if (BetPosNum == 40)
							{
								BetPos = "2nd Column";
								MarkerTomove.Position = offsetInWorldCoords145;
							}
							if (BetPosNum == 41)
							{
								BetPos = "3rd Column";
								MarkerTomove.Position = offsetInWorldCoords146;
							}
							if (BetPosNum == 42)
							{
								BetPos = "3rd 12";
								MarkerTomove.Position = offsetInWorldCoords155;
							}
							if (BetPosNum == 43)
							{
								BetPos = "2nd 12";
								MarkerTomove.Position = offsetInWorldCoords154;
							}
							if (BetPosNum == 44)
							{
								BetPos = "1st 12";
								MarkerTomove.Position = offsetInWorldCoords153;
							}
							if (BetPosNum == 45)
							{
								BetPos = "1 to 18";
								MarkerTomove.Position = offsetInWorldCoords149;
							}
							if (BetPosNum == 46)
							{
								BetPos = "Even";
								MarkerTomove.Position = offsetInWorldCoords150;
							}
							if (BetPosNum == 47)
							{
								BetPos = "Red";
								MarkerTomove.Position = offsetInWorldCoords151;
							}
							if (BetPosNum == 48)
							{
								BetPos = "Black";
								MarkerTomove.Position = offsetInWorldCoords152;
							}
							if (BetPosNum == 49)
							{
								BetPos = "Odd";
								MarkerTomove.Position = offsetInWorldCoords148;
							}
							if (BetPosNum == 50)
							{
								BetPos = "19 to 36";
								MarkerTomove.Position = offsetInWorldCoords147;
							}
						}
						if (Game.IsControlJustPressed(Control.Cover))
						{
							MarkerNum++;
							if (MarkerNum == 1)
							{
								RouletteP1 = BetPos;
							}
							if (MarkerNum == 2)
							{
								RouletteP2 = BetPos;
							}
							if (MarkerNum == 3)
							{
								RouletteP3 = BetPos;
							}
							if (MarkerNum == 3)
							{
								UI.Notify("~g~ Bet 1 :~w~ " + RouletteP1);
								UI.Notify("~b~ Bet 2 :~w~ " + RouletteP2);
								UI.Notify("~r~ Bet 3 :~w~ " + RouletteP3);
							}
						}
					}
				}
			}
			for (int num101 = 0; num101 < RouletteTables.Count; num101++)
			{
				if (World.GetDistance(Game.Player.Character.Position, RouletteTables[num101]) < 2f && !Playing_Roulette)
				{
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to play Roulette");
				}
			}
			for (int num102 = 0; num102 < RouletteTables.Count; num102++)
			{
				if (!(World.GetDistance(Game.Player.Character.Position, RouletteTables[num102]) < 2f) || !Game.IsControlJustPressed(Control.Context))
				{
					continue;
				}
				if (!Playing_Roulette && !Placebet)
				{
					Game.Player.Character.Position = RouletteTables[num102];
					string enterAnim8 = GetEnterAnim();
					string dict44 = "anim_casino_b@amb@casino@games@shared@player@";
					LoadDict("anim_casino_b@amb@casino@games@shared@player@");
					LoadDict(dict44);
					int num103 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
					{
						Game.Player.Character.Position.X,
						Game.Player.Character.Position.Y,
						RouletteTables[num102].Z - 0.25f,
						0.0,
						0.0,
						RouletteTablesRot[num102] + 90f,
						2
					});
					Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
					{
						Game.Player.Character,
						num103,
						LoadDict(dict44),
						"sit_enter_left",
						16f,
						-16f,
						0,
						1148846080
					});
					Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
					{
						Game.Player.Character,
						num103,
						"sit_enter_left",
						LoadDict(dict44),
						16f,
						-16f,
						0,
						1148846080
					});
					Editbet = false;
					Placebet = false;
					Drawcards = false;
					TableT = null;
					Endgame = false;
					CreatedMarker = false;
					PlaceBet_R = false;
					Playercards = 0;
					DealerCards = 0;
					PlayerCard_1 = 0;
					PlayerCard_2 = 0;
					PlayerCard_3 = 0;
					PlayerCard_4 = 0;
					PlayerCard_5 = 0;
					PlayerCard_6 = 0;
					Bet = 0f;
					DealerCard_1 = 0;
					DealerCard_2 = 0;
					DealerCard_3 = 0;
					DealerCard_4 = 0;
					DealerCard_5 = 0;
					DealerCard_6 = 0;
					CurrentTable = num102;
					BetPos = "None";
					BetPosNum = 0;
					MarkerNum = 0;
					if (RouletteMarker2 != null)
					{
						RouletteMarker2.Delete();
					}
					if (RouletteMarker1 != null)
					{
						RouletteMarker1.Delete();
					}
					if (RouletteMarker != null)
					{
						RouletteMarker.Delete();
					}
					MarkerNum = 0;
					RouletteBet1 = 0f;
					RouletteBet2 = 0f;
					RouletteBet3 = 0f;
					foreach (Prop chip18 in Chips)
					{
						if (chip18 != null)
						{
							chip18.Delete();
						}
					}
					Script.Wait(1);
					Playing_Roulette = true;
				}
				else
				{
					if (!Playing_Roulette || Placebet)
					{
						continue;
					}
					TableT = null;
					CurrentTable = num102;
					Game.Player.Character.Position = RouletteTables[num102];
					string enterAnim9 = GetEnterAnim();
					string dict45 = "anim_casino_b@amb@casino@games@shared@player@";
					LoadDict(dict45);
					int num104 = Function.Call<int>(((Hash)0x8C18E0F9080ADD73uL), new InputArgument[7]
					{
						Game.Player.Character.Position.X,
						Game.Player.Character.Position.Y,
						RouletteTables[num102].Z - 0.25f,
						0.0,
						0.0,
						RouletteTablesRot[num102] + 90f,
						2
					});
					Function.Call(((Hash)0xEEA929141F699854uL), new InputArgument[8]
					{
						Game.Player.Character,
						num104,
						LoadDict(dict45),
						"sit_exit_left",
						16f,
						-16f,
						0,
						1148846080
					});
					Function.Call(((Hash)0x734292F4F0ABF6D0uL), new InputArgument[2]
					{
						num104,
						Function.Call<int>(((Hash)0xE4A310B1D7FA73CCuL), new InputArgument[1] { num104 })
					});
					Function.Call(((Hash)0xC77720A12FE14A86uL), new InputArgument[8]
					{
						Game.Player.Character,
						num104,
						"sit_exit_left",
						LoadDict(dict45),
						16f,
						-16f,
						0,
						1148846080
					});
					Script.Wait(4000);
					Game.Player.Character.Task.ClearAll();
					Game.Player.Character.IsPositionFrozen = false;
					CreatedMarker = false;
					Game.Player.Character.Heading = RouletteTablesRot[num102];
					PlaceBet_R = false;
					foreach (Prop chip19 in Chips)
					{
						if (chip19 != null)
						{
							chip19.Delete();
						}
					}
					if (RouletteMarker2 != null)
					{
						RouletteMarker2.Delete();
					}
					if (RouletteMarker1 != null)
					{
						RouletteMarker1.Delete();
					}
					if (RouletteMarker != null)
					{
						RouletteMarker.Delete();
					}
					BetPos = "None";
					MarkerNum = 0;
					BetPosNum = 0;
					RouletteBet1 = 0f;
					RouletteBet2 = 0f;
					RouletteBet3 = 0f;
					Script.Wait(1);
					Playing_Roulette = false;
					foreach (Prop chip20 in Chips)
					{
						if (chip20 != null)
						{
							chip20.Delete();
						}
					}
					Editbet = false;
					Placebet = false;
					Drawcards = false;
					Endgame = false;
					Playercards = 0;
					DealerCards = 0;
					PlayerCard_1 = 0;
					PlayerCard_2 = 0;
					PlayerCard_3 = 0;
					PlayerCard_4 = 0;
					PlayerCard_5 = 0;
					PlayerCard_6 = 0;
					Bet = 0f;
					DealerCard_1 = 0;
					DealerCard_2 = 0;
					DealerCard_3 = 0;
					DealerCard_4 = 0;
					DealerCard_5 = 0;
					DealerCard_6 = 0;
				}
			}
			if (!Playing_BlackJack && !Playing_Poker)
			{
				return;
			}
			for (int num105 = 0; num105 < Tables.Count; num105++)
			{
				if (Tables[num105] != null && World.GetDistance(Game.Player.Character.Position, Tables[num105].Position) < 3f)
				{
					Vector3 offsetInWorldCoords158 = Tables[num105].GetOffsetInWorldCoords(new Vector3(0.39f, -0.18f, 0.95f));
					Vector3 offsetInWorldCoords159 = Tables[num105].GetOffsetInWorldCoords(new Vector3(0.29f, -0.22f, 0.95f));
					Vector3 offsetInWorldCoords160 = Tables[num105].GetOffsetInWorldCoords(new Vector3(0.19f, -0.25f, 0.95f));
					Vector3 offsetInWorldCoords161 = Tables[num105].GetOffsetInWorldCoords(new Vector3(0.16f, -0.15f, 0.95f));
					Vector3 offsetInWorldCoords162 = Tables[num105].GetOffsetInWorldCoords(new Vector3(0.24f, -0.11f, 0.95f));
					Vector3 offsetInWorldCoords163 = Tables[num105].GetOffsetInWorldCoords(new Vector3(0.35f, -0.08f, 0.95f));
					Vector3 offsetInWorldCoords164 = Tables[num105].GetOffsetInWorldCoords(new Vector3(0.15f, 0.13f, 0.95f));
					Vector3 offsetInWorldCoords165 = Tables[num105].GetOffsetInWorldCoords(new Vector3(0f, 0.1f, 0.95f));
					Vector3 offsetInWorldCoords166 = Tables[num105].GetOffsetInWorldCoords(new Vector3(-0.15f, 0.13f, 0.95f));
					if (PlayerCardA != null)
					{
						PlayerCardA.Position = offsetInWorldCoords160;
					}
					if (PlayerCardB != null)
					{
						PlayerCardB.Position = offsetInWorldCoords159;
					}
					if (PlayerCardC != null)
					{
						PlayerCardC.Position = offsetInWorldCoords158;
					}
					if (PlayerCardD != null)
					{
						PlayerCardD.Position = offsetInWorldCoords161;
					}
					if (PlayerCardE != null)
					{
						PlayerCardE.Position = offsetInWorldCoords162;
					}
					if (PlayerCardF != null)
					{
						PlayerCardF.Position = offsetInWorldCoords163;
					}
					if (CardA != null)
					{
						CardA.Position = offsetInWorldCoords164;
						CardA.Rotation = new Vector3(CardA.Rotation.X, CardA.Rotation.Y, Tables[num105].Rotation.Z + 20f);
					}
					if (CardB != null)
					{
						CardB.Position = offsetInWorldCoords165;
						CardB.Rotation = new Vector3(CardB.Rotation.X, CardB.Rotation.Y, Tables[num105].Rotation.Z);
					}
					if (CardC != null)
					{
						CardC.Position = offsetInWorldCoords166;
						CardC.Rotation = new Vector3(CardC.Rotation.X, CardC.Rotation.Y, Tables[num105].Rotation.Z - 20f);
					}
				}
			}
		}
	}
}
