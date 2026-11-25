using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using LemonUI;

namespace GTAO_DiamondCasino
{
	public class CasinoGames : Script
	{
		public enum BetTypeID
		{
			Ante,
			PairPlus,
			Play
		}

		public enum RouletteAnimClips
		{
			ROULETTE_ANIM_CLIP_INVALID = -1,
			ROULETTE_ANIM_CLIP_PLAYER_IDLE_LOOP,
			ROULETTE_ANIM_CLIP_PLAYER_IDLE_ONE_SHOT,
			ROULETTE_ANIM_CLIP_PLAYER_SEAT_ENTER_BACK,
			ROULETTE_ANIM_CLIP_PLAYER_SEAT_ENTER_RIGHT,
			ROULETTE_ANIM_CLIP_PLAYER_SEAT_ENTER_LEFT,
			ROULETTE_ANIM_CLIP_PLAYER_EXIT_LEFT,
			ROULETTE_ANIM_CLIP_PLAYER_EXIT_RIGHT,
			ROULETTE_ANIM_CLIP_PLAYER_REACTION_IMPARTIAL,
			ROULETTE_ANIM_CLIP_PLAYER_REACTION_GOOD,
			ROULETTE_ANIM_CLIP_PLAYER_REACTION_GREAT,
			ROULETTE_ANIM_CLIP_PLAYER_REACTION_BAD,
			ROULETTE_ANIM_CLIP_PLAYER_REACTION_TERRIBLE,
			ROULETTE_ANIM_CLIP_PLAYER_REACTION_DEFLATED,
			ROULETTE_ANIM_CLIP_PLAYER_READY_UP_INTRO_NO_BET,
			ROULETTE_ANIM_CLIP_PLAYER_READY_UP_INTRO_MID_BET,
			ROULETTE_ANIM_CLIP_PLAYER_READY_UP_INTRO_HIGH_BET,
			ROULETTE_ANIM_CLIP_PLAYER_READY_UP_LOOP_NO_BET,
			ROULETTE_ANIM_CLIP_PLAYER_READY_UP_LOOP_MID_BET,
			ROULETTE_ANIM_CLIP_PLAYER_READY_UP_LOOP_HIGH_BET,
			ROULETTE_ANIM_CLIP_DEALER_IDLE,
			ROULETTE_ANIM_CLIP_DEALER_IDLE_ONE_SHOT,
			ROULETTE_ANIM_CLIP_DEALER_IDLE_EMPTY_TABLE_ONE_SHOT,
			ROULETTE_ANIM_CLIP_DEALER_SPIN,
			ROULETTE_ANIM_CLIP_DEALER_NO_MORE_BETS,
			ROULETTE_ANIM_CLIP_DEALER_CLEAR_CHIPS_ZONE_ONE,
			ROULETTE_ANIM_CLIP_DEALER_CLEAR_CHIPS_ZONE_TWO,
			ROULETTE_ANIM_CLIP_DEALER_CLEAR_CHIPS_ZONE_THREE,
			ROULETTE_ANIM_CLIP_DEALER_CLEAR_CHIPS_INTRO,
			ROULETTE_ANIM_CLIP_DEALER_CLEAR_CHIPS_OUTRO,
			ROULETTE_ANIM_CLIP_PROP_WHEEL_SPIN_INTRO,
			ROULETTE_ANIM_CLIP_PROP_WHEEL_SPIN_LOOP,
			ROULETTE_ANIM_CLIP_PROP_WHEEL_SPIN_OUTRO,
			ROULETTE_ANIM_CLIP_PROP_BALL_SPIN_INTRO,
			ROULETTE_ANIM_CLIP_PROP_BALL_SPIN_LOOP,
			ROULETTE_ANIM_CLIP_PROP_BALL_SPIN_OUTRO
		}

		public enum RouletteBetPosition
		{
			None,
			Num_00,
			Num_0,
			Num_1,
			Num_2,
			Num_3,
			Num_4,
			Num_5,
			Num_6,
			Num_7,
			Num_8,
			Num_9,
			Num_10,
			Num_11,
			Num_12,
			Num_13,
			Num_14,
			Num_15,
			Num_16,
			Num_17,
			Num_18,
			Num_19,
			Num_20,
			Num_21,
			Num_22,
			Num_23,
			Num_24,
			Num_25,
			Num_26,
			Num_27,
			Num_28,
			Num_29,
			Num_30,
			Num_31,
			Num_32,
			Num_33,
			Num_34,
			Num_35,
			Num_36,
			Col_1,
			Col_2,
			Col_3,
			_19to36,
			_Odd,
			_1to18,
			_Even,
			_Red,
			_Black,
			_1st12,
			_2nd12,
			_3nd12,
			_1_2_4_5,
			_2_3_5_6,
			_4_5_7_8,
			_5_6_8_9,
			_7_8_10_11,
			_8_9_11_12,
			_10_11_13_14,
			_11_12_14_15,
			_13_14_16_17,
			_14_15_17_18,
			_16_17_19_20,
			_17_18_20_21,
			_19_20_22_23,
			_20_21_23_24,
			_22_23_25_26,
			_23_24_26_27,
			_25_26_28_29,
			_26_27_29_30,
			_28_29_31_32,
			_29_30_32_33,
			_31_32_34_35,
			_32_33_35_36,
			Row_1,
			Segment_1,
			Row_2,
			Segment_2,
			Row_3,
			Segment_3,
			Row_4,
			Segment_4,
			Row_5,
			Segment_5,
			Row_6,
			Segment_6,
			Row_7,
			Segment_7,
			Row_8,
			Segment_8,
			Row_9,
			Segment_9,
			Row_10,
			Segment_10,
			Row_11,
			Segment_11,
			Row_12,
			_1_2_3_00_0,
			_1_2,
			_2_3,
			_4_5,
			_5_6,
			_7_8,
			_8_9,
			_10_11,
			_11_12,
			_13_14,
			_14_15,
			_16_17,
			_17_18,
			_19_20,
			_20_21,
			_22_23,
			_23_24,
			_25_26,
			_26_27,
			_28_29,
			_29_30,
			_31_32,
			_32_33,
			_34_35,
			_35_36
		}

		public enum GetDistSign
		{
			LessEqual,
			Less,
			GreaterEqual,
			Greater
		}

		public enum CardVarient
		{
			Club,
			Spade,
			Heart,
			Diamond,
			Any
		}

		public enum CardID
		{
			None = -1,
			Two,
			Three,
			Four,
			Five,
			Six,
			Seven,
			Eight,
			Nine,
			Ten,
			Jack,
			Queen,
			King,
			Ace
		}

		public class BlackjackCards
		{
			public string Card1 { get; set; }

			public string Card2 { get; set; }

			public string Card3 { get; set; }

			public string Card4 { get; set; }

			public string Card5 { get; set; }

			public string Card6 { get; set; }

			public BlackjackCards()
			{
			}

			public BlackjackCards(string C1, string C2, string C3)
			{
				Card1 = C1;
				Card2 = C2;
				Card3 = C3;
			}
		}

		public enum CardType
		{
			Ace,
			_2_Two,
			_3_Three,
			_4_Four,
			_5_Five,
			_6_Six,
			_7_Seven,
			_8_Eight,
			_9_Nine,
			_10_Ten,
			King,
			Queen,
			Jack
		}

		public class _8PlayerPoker
		{
			public Prop Table1 { get; set; }

			public Prop Table2 { get; set; }

			public Prop Table3 { get; set; }

			public _8PlayerPoker()
			{
			}

			public _8PlayerPoker(Prop T1, Prop T2, Prop T3)
			{
				Table1 = T1;
				Table2 = T2;
				Table3 = T3;
			}
		}

		public class RouletteBet
		{
			public string BetName { get; set; }

			public Prop BetProp { get; set; }

			public int BetAmount { get; set; }

			public int BetPosition { get; set; }

			public Vector3 BetVector3Pos { get; set; }

			public RouletteBet()
			{
			}

			public RouletteBet(string Name, int Amt, int positionID, Vector3 Position, Prop NewMarker)
			{
				BetName = Name;
				BetAmount = Amt;
				BetPosition = positionID;
				BetVector3Pos = Position;
				BetProp = NewMarker;
			}
		}

		public class ChipStack
		{
			public List<Prop> ChipStackProps = new List<Prop>();

			public List<string> ChipStackModels = new List<string>();

			public Vector3 ChipStackOffset { get; set; }

			public bool ChipsStackFull { get; set; }

			public ChipStack()
			{
			}

			public ChipStack(Vector3 Offset)
			{
				ChipStackOffset = Offset;
			}
		}

		public class _8PlayerPokerChipsBank
		{
			public List<ChipStack> Stack = new List<ChipStack>();

			public float Offset_Stack = 0.06f;

			public float Offset_Chip = 0.004f;

			public float BankAmount { get; set; }

			public float AmountInBankBeforePlace { get; set; }

			public static int MaxChips { get; set; }

			public int MaxStacks { get; set; }

			public Prop AttachementProp { get; set; }

			public Vector3 CentreOffset { get; set; }

			public int GetStackAmt(List<string> StackList)
			{
				int num = 0;
				if (StackList.Count > 0)
				{
					foreach (string Stack in StackList)
					{
						if (Stack.Contains("st"))
						{
							num++;
						}
					}
					return num;
				}
				return num;
			}

			public string ReturnChipAdd(float Amt)
			{
				string result = "";
				if (Amt == 10f)
				{
					result = "vw_prop_chip_10dollar_x1";
				}
				if (Amt == 50f)
				{
					result = "vw_prop_chip_50dollar_x1";
				}
				if (Amt == 100f)
				{
					result = "vw_prop_chip_100dollar_x1";
				}
				if (Amt == 500f)
				{
					result = "vw_prop_chip_500dollar_x1";
				}
				if (Amt == 1000f)
				{
					result = "vw_prop_chip_1kdollar_x1";
				}
				if (Amt == 5000f)
				{
					result = "vw_prop_chip_5kdollar_x1";
				}
				if (Amt == 10000f)
				{
					result = "vw_prop_chip_10kdollar_x1";
				}
				if (Amt == 50000f)
				{
					result = "vw_prop_chip_5kdollar_st";
				}
				if (Amt == 100000f)
				{
					result = "vw_prop_chip_10kdollar_st";
				}
				return result;
			}

			public Model RequestModel(string Prop)
			{
				Model result = new Model(Prop);
				result.Request(250);
				if (result.IsInCdImage && result.IsValid)
				{
					while (!result.IsLoaded)
					{
						Script.Wait(1);
					}
					return result;
				}
				result.MarkAsNoLongerNeeded();
				return result;
			}

			public _8PlayerPokerChipsBank()
			{
			}

			public void SpawnNewChip(float AmmtLeft)
			{
				Script.Wait(1);
				for (int i = 0; i < Stack.Count; i++)
				{
					ChipStack chipStack = Stack[i];
					if (i == 0)
					{
						if (chipStack.ChipStackProps.Count >= MaxChips || GetStackAmt(chipStack.ChipStackModels) >= MaxStacks)
						{
							chipStack.ChipsStackFull = true;
							continue;
						}
						if (chipStack.ChipStackProps.Count == 0)
						{
							string text = ReturnChipAdd(AmmtLeft);
							Prop item = GlobalHelper.CreateProp(RequestModel(text), AttachementProp.GetOffsetPosition(chipStack.ChipStackOffset), false, false);
							chipStack.ChipStackProps.Add(item);
							chipStack.ChipStackModels.Add(text);
							continue;
						}
						string text2 = ReturnChipAdd(AmmtLeft);
						string text3 = chipStack.ChipStackModels[chipStack.ChipStackProps.Count - 1];
						Prop prop = chipStack.ChipStackProps[chipStack.ChipStackProps.Count - 1];
						if (text3.Contains("st"))
						{
							Prop item2 = GlobalHelper.CreateProp(RequestModel(text2), prop.GetOffsetPosition(new Vector3(0f, 0f, Offset_Stack)), false, false);
							chipStack.ChipStackProps.Add(item2);
							chipStack.ChipStackModels.Add(text2);
						}
						if (text3.Contains("x1"))
						{
							Prop item3 = GlobalHelper.CreateProp(RequestModel(text2), prop.GetOffsetPosition(new Vector3(0f, 0f, Offset_Chip)), false, false);
							chipStack.ChipStackProps.Add(item3);
							chipStack.ChipStackModels.Add(text2);
						}
					}
					else
					{
						if (!Stack[i - 1].ChipsStackFull)
						{
							continue;
						}
						if (chipStack.ChipStackProps.Count >= MaxChips || GetStackAmt(chipStack.ChipStackModels) >= MaxStacks)
						{
							chipStack.ChipsStackFull = true;
							continue;
						}
						if (chipStack.ChipStackProps.Count == 0)
						{
							string text4 = ReturnChipAdd(AmmtLeft);
							Prop item4 = GlobalHelper.CreateProp(RequestModel(text4), AttachementProp.GetOffsetPosition(chipStack.ChipStackOffset), false, false);
							chipStack.ChipStackProps.Add(item4);
							chipStack.ChipStackModels.Add(text4);
							continue;
						}
						string text5 = ReturnChipAdd(AmmtLeft);
						string text6 = chipStack.ChipStackModels[chipStack.ChipStackProps.Count - 1];
						Prop prop2 = chipStack.ChipStackProps[chipStack.ChipStackProps.Count - 1];
						if (text6.Contains("st"))
						{
							Prop item5 = GlobalHelper.CreateProp(RequestModel(text5), prop2.GetOffsetPosition(new Vector3(0f, 0f, Offset_Stack)), false, false);
							chipStack.ChipStackProps.Add(item5);
							chipStack.ChipStackModels.Add(text5);
						}
						if (text6.Contains("x1"))
						{
							Prop item6 = GlobalHelper.CreateProp(RequestModel(text5), prop2.GetOffsetPosition(new Vector3(0f, 0f, Offset_Chip)), false, false);
							chipStack.ChipStackProps.Add(item6);
							chipStack.ChipStackModels.Add(text5);
						}
					}
				}
			}

			public void DeleteAllChips()
			{
				BankAmount = 0f;
				foreach (ChipStack item in Stack)
				{
					foreach (Prop chipStackProp in item.ChipStackProps)
					{
						if (chipStackProp != null)
						{
							chipStackProp.Delete();
						}
					}
					item.ChipStackProps.Clear();
					item.ChipStackModels.Clear();
					item.ChipsStackFull = false;
				}
			}

			public void SpawnChips(float Amt)
			{
				AmountInBankBeforePlace = Amt;
				DeleteAllChips();
				BankAmount = Amt;
				bool flag = true;
				foreach (ChipStack item in Stack)
				{
					if (!item.ChipsStackFull)
					{
						flag = true;
					}
				}
				if (BankAmount <= 10f && flag)
				{
					Notification.Show("Chips Spawned Max");
					return;
				}
				while (BankAmount - 100000f >= 100000f)
				{
					SpawnNewChip(100000f);
					BankAmount -= 100000f;
				}
				while (BankAmount - 50000f >= 50000f)
				{
					SpawnNewChip(50000f);
					BankAmount -= 50000f;
				}
				while (BankAmount - 10000f >= 10000f)
				{
					SpawnNewChip(10000f);
					BankAmount -= 10000f;
				}
				while (BankAmount - 5000f >= 5000f)
				{
					SpawnNewChip(5000f);
					BankAmount -= 5000f;
				}
				while (BankAmount - 1000f >= 1000f)
				{
					SpawnNewChip(1000f);
					BankAmount -= 1000f;
				}
				while (BankAmount - 500f >= 500f)
				{
					SpawnNewChip(500f);
					BankAmount -= 500f;
				}
				while (BankAmount - 100f >= 100f)
				{
					SpawnNewChip(100f);
					BankAmount -= 100f;
				}
				while (BankAmount - 50f >= 50f)
				{
					SpawnNewChip(50f);
					BankAmount -= 50f;
				}
				while (BankAmount - 10f >= 10f)
				{
					SpawnNewChip(10f);
					BankAmount -= 10f;
				}
			}

			public _8PlayerPokerChipsBank(bool IsPlayerBank, Prop AttachProp, Vector3 O1, float Amt, int MC, int MS, int SUD, int SLR)
			{
				CentreOffset = O1;
				AmountInBankBeforePlace = Amt;
				MaxChips = MC;
				MaxStacks = MS;
				BankAmount = Amt;
				AttachementProp = AttachProp;
				if (IsPlayerBank)
				{
					Stack.Add(new ChipStack(O1));
					Stack.Add(new ChipStack(new Vector3(O1.X - 0.05f, O1.Y, O1.Z)));
					Stack.Add(new ChipStack(new Vector3(O1.X + 0.05f, O1.Y, O1.Z)));
					Stack.Add(new ChipStack(new Vector3(O1.X, O1.Y + 0.05f, O1.Z)));
					Stack.Add(new ChipStack(new Vector3(O1.X, O1.Y - 0.05f, O1.Z)));
					Stack.Add(new ChipStack(new Vector3(O1.X - 0.05f, O1.Y - 0.05f, O1.Z)));
					Stack.Add(new ChipStack(new Vector3(O1.X - 0.05f, O1.Y + 0.05f, O1.Z)));
					Stack.Add(new ChipStack(new Vector3(O1.X + 0.05f, O1.Y + 0.05f, O1.Z)));
					Stack.Add(new ChipStack(new Vector3(O1.X + 0.05f, O1.Y - 0.05f, O1.Z)));
				}
				if (!IsPlayerBank)
				{
					Stack.Add(new ChipStack(O1));
					for (int i = 1; i < SUD; i++)
					{
						Stack.Add(new ChipStack(new Vector3(O1.X - 0.05f * (float)i, O1.Y, O1.Z)));
						Stack.Add(new ChipStack(new Vector3(O1.X + 0.05f * (float)i, O1.Y, O1.Z)));
						Stack.Add(new ChipStack(new Vector3(O1.X, O1.Y - 0.05f * (float)i, O1.Z)));
						Stack.Add(new ChipStack(new Vector3(O1.X, O1.Y + 0.05f * (float)i, O1.Z)));
						for (int j = 1; j < SUD; j++)
						{
							Stack.Add(new ChipStack(new Vector3(O1.X - 0.05f * (float)i, O1.Y - 0.05f * (float)j, O1.Z)));
							Stack.Add(new ChipStack(new Vector3(O1.X + 0.05f * (float)i, O1.Y - 0.05f * (float)j, O1.Z)));
							Stack.Add(new ChipStack(new Vector3(O1.X - 0.05f * (float)i, O1.Y + 0.05f * (float)j, O1.Z)));
							Stack.Add(new ChipStack(new Vector3(O1.X + 0.05f * (float)i, O1.Y + 0.05f * (float)j, O1.Z)));
						}
					}
				}
				SpawnChips(BankAmount);
			}
		}

		public enum BetTypeId
		{
			AnteBet,
			PairPlusBet,
			PlayBet,
			BlackJack
		}

		public enum ROULETTE_AUDIO_CLIP
		{
			RAC_DEALER_GREET_PLAYER,
			RAC_DEALER_GREET_DRUNK_PLAYER,
			RAC_DEALER_GREET_MALE_PLAYER,
			RAC_DEALER_GREET_FEMALE_PLAYER,
			RAC_DEALER_GREET_REJOINING_PLAYER,
			RAC_DEALER_LEAVE_NEUTRAL,
			RAC_DEALER_LEAVE_GOOD,
			RAC_DEALER_LEAVE_BAD,
			RAC_DEALER_CALL_WINNING_NUMBER,
			RAC_DEALER_PLACE_YOUR_BETS,
			RAC_DEALER_REFUSE_BETS,
			RAC_DEALER_NO_MORE_BETS,
			RAC_DEALER_ANOTHER_ROUND,
			RAC_SFX_INVALID_ACTION,
			RAC_SFX_HIGHLIGHT,
			RAC_SFX_ADJUST_CHIPS_UP,
			RAC_SFX_ADJUST_CHIPS_DOWN,
			RAC_SFX_SHOW_RULES,
			RAC_SFX_CONTINUE,
			RAC_SFX_BET_MAX,
			RAC_SFX_REMOVE_BET,
			RAC_SFX_WIN,
			RAC_SFX_PLACE_CHIPS,
			RAC_SFX_PLACE_CHIPS_MODEL_SWAP,
			RAC_SFX_CLEAR_CHIPS_SMALL,
			RAC_SFX_CLEAR_CHIPS_MEDIUM,
			RAC_SFX_CLEAR_CHIPS_LARGE,
			RAC_SFX_BALL_LOOP,
			RAC_SFX_BALL_EXIT
		}

		public enum ROULETTE_AUDIO_SET
		{
			RAS_TABLE_GAME_SOUNDS,
			RAS_FRONTEND_SOUNDS,
			RAS_BALL_EXIT_SOUNDS
		}

		public enum ModelNames
		{
			VW_PROP_CHIP_10DOLLAR_X1,
			VW_PROP_CHIP_10DOLLAR_ST,
			VW_PROP_CHIP_50DOLLAR_X1,
			VW_PROP_CHIP_50DOLLAR_ST,
			VW_PROP_CHIP_100DOLLAR_X1,
			VW_PROP_CHIP_100DOLLAR_ST,
			VW_PROP_CHIP_500DOLLAR_X1,
			VW_PROP_CHIP_500DOLLAR_ST,
			VW_PROP_CHIP_1KDOLLAR_X1,
			VW_PROP_CHIP_1KDOLLAR_ST,
			VW_PROP_CHIP_5KDOLLAR_X1,
			VW_PROP_CHIP_5KDOLLAR_ST,
			VW_PROP_CHIP_10KDOLLAR_X1,
			VW_PROP_CHIP_10KDOLLAR_ST
		}

		public enum TableType
		{
			Blackjack,
			Poker,
			Roulette
		}

		public enum CasinoGamePlaying
		{
			None,
			Poker,
			Blackjack,
			EightCardPoker,
			Slots,
			Roulette
		}

		public enum CardMove
		{
			Disabled,
			Active
		}

		public const int CasinoInteriorID = 275201;

		public const int PenthouseInteriorID = 274689;

		public static bool isPlaceableCasinoGames = false;

		public static Prop RouletteMarker1;

		public const int ROULETTE_DOUBLE_ZERO_IDENTIFIER = 37;

		public const int ROULETTE_SINGLE_ZERO_IDENTIFIER = 0;

		public static float InitialWheelRotation;

		public static float RouletteBox_Height = 0.041f;

		public static float RouletteBox_Width = 0.0835f;

		public static string CurrentHelpMessage = "";

		public static int HelpMessageShowTime;

		public static int Previous_Chips;

		public static int ShowChangeTime = -1;

		public static List<Prop> BetPosMarkers = new List<Prop>();

		public static int PrevBetPos;

		public static Prop BlackjackPlayerInitialBet;

		public static Prop PokerPlayerPlayBet;

		public static Prop DealerDeckOfCards;

		public static List<string> CardDeck = new List<string>();

		public static int BlackjackBackupDealerCard1_ThreeCards;

		public static int BlackjackBackupDealerCard1_FourCards;

		public static int BlackjackBackupDealerCard1_FiveCards;

		public static int BlackjackBackupDealerCard1_SixCards;

		public static Prop Backface_DealerCard1;

		public static string FrontFace_DealerCard1;

		public static List<BlackjackCards> DealerPresetCars = new List<BlackjackCards>();

		public static int PresetChoosen;

		public static int ShowChangeChipsDelay;

		public static List<_8PlayerPoker> _8PlayerPokerTables = new List<_8PlayerPoker>();

		public static Vector3 NewMarkerRoulette;

		public static int RouletteSpinScene;

		public static string CurrentNum;

		public static string RoulleteTable = "A";

		public static int HighLimitTableColor = 2;

		public static int NormalTableColor = 1;

		public static int _8PlayerPokerTableColor = 2;

		public static Prop HighLimitPokerTable1;

		public static Prop HighLimitBlackjackTable1;

		public static Prop HighLimitRouleteTable1;

		public static Prop HighLimitPokerTable2;

		public static Prop HighLimitBlackjackTable2;

		public static Prop HighLimitRouleteTable2;

		public static Prop NormalPokerTable1;

		public static Prop NormalBlackjackTable1;

		public static Prop NormalRouleteTable1;

		public static Prop NormalBlackjackTable2;

		public static Prop NormalPokerTable2;

		public static int CurrentTable;

		public static Camera RoulleteCam;

		public static bool CreatedRouletteCam;

		public static Vector2 ControllerMousePos;

		public static int PlayerDeck = 1;

		public static int EliteDeckUnlocked = 1;

		public static int TDMPrice;

		public static int TMDUnlocked;

		public static bool TMD_UseNormalDeckCards;

		public static bool TMD_UseEliteDeckCards;

		public static bool TMD_UseAce;

		public static bool TMD_UseKing;

		public static bool TMD_UseQueen;

		public static bool TMD_UseJack;

		public static Prop RouletteTableA;

		public static Prop RouletteTableB;

		public static Prop RouletteTableC;

		public static bool TMD_Use02;

		public static bool TMD_Use03;

		public static bool TMD_Use04;

		public static bool TMD_Use05;

		public static bool TMD_Use06;

		public static bool TMD_Use07;

		public static bool TMD_Use08;

		public static bool TMD_Use09;

		public static bool TMD_Use10;

		public static bool TMD_Dealer;

		public static string BlackJackCard1;

		public static string BlackJackCard2;

		public static string BlackJackCard3;

		public static string BlackJackCard4;

		public static string BlackJackCard5;

		public static Prop RouletteBetMarker_00;

		public static int RouletteStartScene;

		public static string CardId;

		public static int TimeSinceLastCard;

		public static float Stage;

		public static int BallLoopTime;

		public static Prop FakeBall;

		public static int DrawCardTime;

		public static int DealerCardsDrawn;

		public static bool BlackJackSplit;

		public static bool BlackJackDoubleDown;

		public static bool LookingAtDealerCards;

		public static bool LookingAtWheel;

		public static int SeatSittingIn = 2;

		public static int Scene1;

		public static int Scene2;

		public static int Scene3;

		public static int Scene4;

		public static Ped ClosestDealer;

		public static DealerTable ClosestDealerTable;

		public static Prop ClosestTable;

		public static bool BetOne;

		public static bool BetMax;

		public static float BetSlot;

		public static float ReelA_SpinAmt;

		public static float ReelB_SpinAmt;

		public static float ReelC_SpinAmt;

		public static bool SpinnedReels;

		public static bool FinishedReelA;

		public static bool FinishedReelB;

		public static bool FinishedReelC;

		public static SlotsClass CurrentSlotClass;

		public static float MinStartBet;

		public static float SlotMultiplier;

		public static Prop Ball;

		public static string ALandedOn;

		public static string BLandedOn;

		public static string CLandedOn;

		public static List<RouletteBet> RouletteBets = new List<RouletteBet>();

		public static int RouletteBetsPlaced;

		public static bool PlaceBet_R;

		public static string BetPos = "None";

		public static int BetPosNum = 0;

		public static int MarkerNum;

		public static Prop MarkerTomove;

		public static Prop TableT;

		public static int Vic1;

		public static int Vic2;

		public static int Vic3;

		public static int BetType;

		public static int Bet_PairPlus;

		public static int Bet_Ante;

		public static int Bet_Play;

		public static int Bet_PairPlus_Index = -1;

		public static int Bet_Ante_Index;

		public static int Bet_Play_Index;

		public static List<Vector3> PokerTables = new List<Vector3>();

		public static List<float> PokerTablesRot = new List<float>();

		public static List<DealerTable> PokerDealerTables = new List<DealerTable>();

		public static List<Vector3> BlackJackTables = new List<Vector3>();

		public static List<float> BlackJackRot = new List<float>();

		public static List<DealerTable> BlackjackDealerTables = new List<DealerTable>();

		public static List<Vector3> SlotMachines = new List<Vector3>();

		public static List<float> SlotMachinesRot = new List<float>();

		public static List<Vector3> RouletteTables = new List<Vector3>();

		public static List<float> RouletteTablesRot = new List<float>();

		public static List<DealerTable> RouletteDealerTables = new List<DealerTable>();

		public static List<Prop> Tables = new List<Prop>();

		public static List<Prop> Tables2 = new List<Prop>();

		public static List<Prop> Chips = new List<Prop>();

		public static Vector3 LuckyWheel = new Vector3(1111.045f, 228.6406f, -50.5f);

		public static List<Ped> SlotMachinePeds = new List<Ped>();

		public static int PlayerCard_1;

		public static int PlayerCard_2;

		public static int PlayerCard_3;

		public static int PlayerCard_4;

		public static int PlayerCard_5;

		public static int PlayerCard_6;

		public static string PlayerCard_1_ID;

		public static string PlayerCard_2_ID;

		public static string PlayerCard_3_ID;

		public static string PlayerCard_4_ID;

		public static string PlayerCard_5_ID;

		public static string PlayerCard_6_ID;

		public static int DealerCard_1;

		public static int DealerCard_2;

		public static int DealerCard_3;

		public static int DealerCard_4;

		public static int DealerCard_5;

		public static int DealerCard_6;

		public static float Mp = 1f;

		public static bool Playing_Poker;

		public static bool Playing_BlackJack;

		public bool Playing_SlotMachine;

		public static bool Playing_Roulette;

		public static Prop CardA;

		public static Prop CardB;

		public static Prop CardC;

		public static bool Placebet;

		public static bool Placebet2;

		public static bool Drawcards;

		public static bool Editbet;

		public static int LastRecordedChips;

		public static int LastDiff;

		public static int Playercards;

		public static bool BlockGenericNotifsNow;

		public static int BlockGenericNotifsTime;

		public static int DealerCards;

		public static float Bet;

		public static int RouletteBetIndex;

		public static int BetAmtIndex;

		public static float InitalBet;

		public static int TimeBetweenBets;

		public static int Set_TimeBetweenBets = 90;

		public static float Bet2;

		public static string A_roulette = "";

		public static int B_roulette = 0;

		public static int Ball_Outro_Anim_Index;

		public static int Chips_Amount = 100000;

		public static Prop _8PlayerPokerGameTable1;

		public static Prop _8PlayerPokerGameTable2;

		public static Prop _8PlayerPokerGameTable3;

		public static Prop _8PlayerPokerGameTable4;

		public static Prop _8PlayerPokerGameTable5;

		public static Prop _8PlayerPokerGameTable6;

		public static Prop _8PlayerPokerGameTable7;

		public static List<Ped> _8PlayerGamePokerPeds = new List<Ped>();

		public static List<Prop> _8PlayerGamePokerProps = new List<Prop>();

		public static List<Prop> _8PlayerGamePokerPotChips = new List<Prop>();

		public static float _8PlayerGamePoker_PlayerBank;

		public static float _8PlayerGamePoker_AI1Bank;

		public static float _8PlayerGamePoker_AI2Bank;

		public static float _8PlayerGamePoker_AI3Bank;

		public static float _8PlayerGamePoker_AI4Bank;

		public static float _8PlayerGamePoker_AI5Bank;

		public static float _8PlayerGamePoker_AI6Bank;

		public static float _8PlayerGamePoker_AI7Bank;

		public static float _8PlayerGamePokerPot;

		public static Vector3 _8PlayerGameStartMarker;

		public static Vector3 _8PlayerGameExitMarker;

		public static int Player_SyncScene;

		public static Vector3 AnteBet_Offset = new Vector3(0.249f, -0.12f, 0.949f);

		public static Vector3 PlayBet_Offset = new Vector3(0.284f, -0.209f, 0.949f);

		public static Vector3 PairPlusBet_Offset = new Vector3(0.214f, -0.039f, 0.949f);

		public static Vector3 Blackjack_ChipOffset = new Vector3(0.284f, -0.224f, 0.949f);

		public static float PokerChip_RotationOffset = -336f;

		public static float BlackJackChip_RotationOffset = -24f;

		public static float Rotationoffset;

		public static int AI1_SyncScene;

		public static int AI2_SyncScene;

		public static int AI3_SyncScene;

		public static int AI4_SyncScene;

		public static int AI5_SyncScene;

		public static int AI6_SyncScene;

		public static int AI7_SyncScene;

		public static Ped AI_1;

		public static Ped AI_2;

		public static Ped AI_3;

		public static Ped AI_4;

		public static Ped AI_5;

		public static Ped AI_6;

		public static Ped AI_7;

		public static bool Player_SkipTurn;

		public static bool Player_Bust;

		public static bool AI1_SkipTurn;

		public static bool AI1_Bust;

		public static bool AI2_SkipTurn;

		public static bool AI2_Bust;

		public static bool AI3_SkipTurn;

		public static bool AI3_Bust;

		public static bool AI4_SkipTurn;

		public static bool AI4_Bust;

		public static bool AI5_SkipTurn;

		public static bool AI5_Bust;

		public static bool AI6_SkipTurn;

		public static bool AI6_Bust;

		public static bool AI7_SkipTurn;

		public static bool AI7_Bust;

		public static int _8PlayerGameStage;

		public static Ped _8PlayerGameDealer;

		public static List<PlayerChips> PlayerChips = new List<PlayerChips>();

		public static List<PlayerCards> PlayerCards = new List<PlayerCards>();

		public static float LastRaiseAmmount;

		public static Prop PlayerCard1;

		public static Prop PlayerCard2;

		public static Prop PlayerCard3;

		public static float _8PlayerGamePoker_PlayerStartingBank = 50000f;

		public static float _8PlayerGamePoker_AI1StartingBank = 50000f;

		public static float _8PlayerGamePoker_AI2StartingBank = 50000f;

		public static float _8PlayerGamePoker_AI3StartingBank = 50000f;

		public static float _8PlayerGamePoker_AI4StartingBank = 50000f;

		public static float _8PlayerGamePoker_AI5StartingBank = 50000f;

		public static float _8PlayerGamePoker_AI6StartingBank = 50000f;

		public static float _8PlayerGamePoker_AI7StartingBank = 50000f;

		public static bool lookingAtCards;

		public static float PlayerTotal;

		public static float AI1Total;

		public static float AI2Total;

		public static float AI3Total;

		public static float AI4Total;

		public static float AI5Total;

		public static float AI6Total;

		public static float AI7Total;

		public static int Player_IdleScenesPlayed;

		public static int AI1_IdleScenesPlayed;

		public static int AI2_IdleScenesPlayed;

		public static int AI3_IdleScenesPlayed;

		public static int AI4_IdleScenesPlayed;

		public static int AI5_IdleScenesPlayed;

		public static int AI6_IdleScenesPlayed;

		public static int AI7_IdleScenesPlayed;

		public static int Player_IdleScenesToPlayBeforeChange;

		public static int AI1_IdleScenesToPlayBeforeChange;

		public static int AI2_IdleScenesToPlayBeforeChange;

		public static int AI3_IdleScenesToPlayBeforeChange;

		public static int AI4_IdleScenesToPlayBeforeChange;

		public static int AI5_IdleScenesToPlayBeforeChange;

		public static int AI6_IdleScenesToPlayBeforeChange;

		public static int AI7_IdleScenesToPlayBeforeChange;

		public static bool AreBetsBeingPlaced;

		public static int BettingRoundStage;

		public static float BettingRoundStartingBet;

		public static bool AI1_HasPlayedBettingRoundMove;

		public static bool AI2_HasPlayedBettingRoundMove;

		public static bool AI3_HasPlayedBettingRoundMove;

		public static bool AI4_HasPlayedBettingRoundMove;

		public static bool AI5_HasPlayedBettingRoundMove;

		public static bool AI6_HasPlayedBettingRoundMove;

		public static bool AI7_HasPlayedBettingRoundMove;

		public static Prop ChipsSpawn;

		public static Prop Deck;

		public static Prop LastChip;

		public static Prop CommunityCard1;

		public static float CommunityCard1Value;

		public static string CommunityCard1ID;

		public static Prop CommunityCard2;

		public static float CommunityCard2Value;

		public static string CommunityCard2ID;

		public static Prop CommunityCard3;

		public static float CommunityCard3Value;

		public static string CommunityCard3ID;

		public static bool IsinBettingRound;

		public static float MaxRaiseAmt;

		public static bool Playing8PlayerPoker;

		public static _8PlayerPokerChipsBank Player_VisualBank;

		public static _8PlayerPokerChipsBank AI_1_VisualBank;

		public static _8PlayerPokerChipsBank AI_2_VisualBank;

		public static _8PlayerPokerChipsBank AI_3_VisualBank;

		public static _8PlayerPokerChipsBank AI_4_VisualBank;

		public static _8PlayerPokerChipsBank AI_5_VisualBank;

		public static _8PlayerPokerChipsBank AI_6_VisualBank;

		public static _8PlayerPokerChipsBank AI_7_VisualBank;

		public static _8PlayerPokerChipsBank _8PlayerPokerPot;

		public static Vector3 _8PlayerPokerChair1_ChipOffset = new Vector3(-0.689f, 0.21f, 0.959f);

		public static float _8PlayerPokerChair1_ChipRotation = -67f;

		public static Vector3 _8PlayerPokerChair2_ChipOffset = new Vector3(-0.31f, -0.21f, 0.959f);

		public static float _8PlayerPokerChair2_ChipRotation = -19f;

		public static Vector3 _8PlayerPokerChair3_ChipOffset = new Vector3(0.29f, -0.21f, 0.959f);

		public static float _8PlayerPokerChair3_ChipRotation = 21f;

		public static Vector3 _8PlayerPokerChair4_ChipOffset = new Vector3(0.679f, 0.18f, 0.959f);

		public static float _8PlayerPokerChair4_ChipRotation = 72f;

		public static int StackChip;

		public static float StackChipHeight;

		public static int CheckDealerTables;

		public static bool CreatedMarker;

		public static string LastRouletteBallNum;

		public static bool ReadyForRouletteEnd;

		public static int RouletteSpinningSound;

		public static int RouletteNextSoundTimer;

		public static Prop PlayerCardA;

		public static Prop PlayerCardB;

		public static Prop PlayerCardC;

		public static Prop PlayerCardD;

		public static Prop PlayerCardE;

		public static Prop PlayerCardF;

		public static int SlotMachineSpinningSoundFx;

		private int ScreenScaleformStage;

		public static int ScreenScaleformID;

		public static int ScreenNameMovieID;

		public static int SlotMachineStage;

		public string SlotDict;

		public static bool SlotInUse;

		public static Vector3 PlayerOriginPosition;

		public static float PlayerOriginHeading;

		public static Prop SlotProp1;

		public static Prop SlotProp2;

		public static Prop SlotProp3;

		public static Vector3 uParam1;

		public static float fParam2;

		public static int StageA;

		public static int StageB;

		public static int StageC;

		public static int RotateTimerA;

		public static int RotateTimerB;

		public static int RotateTimerC;

		public static List<SlotsClass> SlotLocH2 = new List<SlotsClass>();

		public static List<SlotsClass> SlotLocH = new List<SlotsClass>();

		public static List<Prop> SlotMachineProps = new List<Prop>();

		public static Prop CurrentSlotMachine;

		public static bool Endgame;

		public static Prop ReelABase;

		public static Prop ReelBBase;

		public static Prop ReelCBase;

		public static bool CreatedSlotMachines;

		public static List<int> SlotMachineSounds = new List<int>();

		public static bool SittingOnInsideTrackChair;

		public static int InsideTrackChairScene;

		public static int Horse1Color = -1;

		public static int Horse2Color = -1;

		public static int Horse3Color = -1;

		public static int Horse4Color = -1;

		public static int Horse5Color = -1;

		public static int Horse6Color = -1;

		public static int GUIMT;

		public static float GUIPosY;

		public static float GUIPosX;

		public static bool GUIOn;

		public static int Horse1Id;

		public static int Horse2Id;

		public static int Horse3Id;

		public static int Horse4Id;

		public static int Horse5Id;

		public static int Horse6Id;

		public static float Horse1Entries;

		public static float Horse2Entries;

		public static float Horse3Entries;

		public static float Horse4Entries;

		public static float Horse5Entries;

		public static float Horse6Entries;

		public static int Horse1Odds;

		public static int Horse2Odds;

		public static int Horse3Odds;

		public static int Horse4Odds;

		public static int Horse5Odds;

		public static int Horse6Odds;

		public static int RacingBet;

		public static List<string> HorseNamesAndOdds = new List<string>
		{
			"A Tethered End, 27/1", "Bad Egg, 29/1", "Banana Hammock, 16/1", "Better than Nothing, 15/1", "Black Rock Rooster, 18/1", "Bleet Me Baby, 7/1", "Blue Dream, 2/1", "Borrowed Sorrow, 12/1", "Bouncy Blessed, 5/1", "Cancelled Check, 25/1",
			"Can't Be Wronger, 28/1", "Clapback Charlie, 10/1", "Constant Brag, 3/1", "Country Stuck, 21/1", "Crackers and Please, 3/1", "Creepy Dentist, 7/1", "Crock Janley, 8/1", "Dancin' Pole, 9/1", "Dancin' Shoes, 5/1", "Darling Ricki, 22/1",
			"Dead Fam, 26/1", "Dead Heat Hattie, 17/1", "Dexie Runner, 6/1", "Divorced Doctor, 7/1", "Doozy Floozy, 13/1", "Downtown Renown, 4/1", "Dr. Deez Reins, 5/1", "Dream Shatterer, 2/1", "Drone Warning, 8/1", "Drunken Brandee, 14/1",
			"Durban Poison, 20/1", "Feed the Trolls, 30/1", "Fire Hazards, 24/1", "Flipped Wig, 12/1", "Friendly Fire, 9/1", "Getting Haughty, 3/1", "Ghost Dank, 10/1", "Glass or Tina, 23/1", "Hard Time Done, 18/1", "Hell for Weather, 2/1",
			"Hennigan's Steed, 4/1", "Hippie Crack, 23/1", "Hot & Bothered, 2/1", "Invade Grenade, 13/1", "It's a Trap, 2/1", "Kraff Running, 14/1", "Lead Is out, 3/1", "Lit as Truck, 2/1", "Lonely Stepbrother, 3/1", "Los Santos Savior, 5/1",
			"Lover's Speed, 2/1", "Measles Smeezles, 15/1", "Micro Aggression, 8/1", "Minimum Wager, 26/1", "Miss Mary John, 22/1", "Miss Triggered, 19/1", "Mister Redacted, 19/1", "Mister Scissors, 7/1", "Money to Burn, 30/1", "Moon Rocks, 4/1",
			"Mr. Worthwhile, 2/1", "Mud Dragon, 5/1", "Night-time Mare, 16/1", "Northern Lights, 15/1", "Nuns Orders, 9/1", "Ol' Skag, 28/1", "Old Ill Will, 29/1", "Omens and Ice, 3/1", "Pedestrian, 25/1", "Pretty as a Pistol, 4/1",
			"Questionable Dignity, 12/1", "Reach Around Town, 6/1", "Robocall, 4/1", "Salt 'n' Sauce, 2/1", "Salty and Woke, 17/1", "Scrawny Nag, 30/1", "Sir Scrambled, 26/1", "Sizzurp, 13/1", "Snatched Your Mama, 2/1", "Social Media Warrior, 27/1",
			"Square to Go, 6/1", "Study Buddy, 15/1", "Stupid Money, 30/1", "Sumptin Saucy, 2/1", "Sweet Releaf, 4/1", "Tax the Poor, 13/1", "Tea Ache Sea, 24/1", "Tenpenny, 10/1", "There She Blows, 2/1", "Throwing Shady, 21/1",
			"Thunder Skunk, 20/1", "Total Belter, EVENS", "Turnt Mood, 12/1", "Uptown Rider, 14/1", "Wage of Consent, 21/1", "Wee Scunner, 8/1", "Worth a Kingdom, 2/1", "Yay Yo Let's Go, 3/1", "Yellow Sunshine, 5/1"
		};

		public static int RaceCompletedReturn;

		public static int HorsePosReturn;

		public static int RacingSoundLoop;

		public static int HorsePositions;

		public static bool RaceComplete;

		public static int GUITab;

		public static int Horse_Color_primaryColour;

		public static int Horse_Color_secondaryColour;

		public static int Horse_Color_horseColour;

		public static int Horse_Color_maneColour;

		public static int GUISeclection;

		public static int HorseRacingStage;

		public static bool HorsesAreRacing;

		public static bool ShowingEndScrene;

		public static int EndOfRaceDelay;

		public static int EndScreenDelay;

		public static string LastWin;

		public static int Horse1Place;

		public static int Horse2Place;

		public static int Horse3Place;

		public static int Horse4Place;

		public static int Horse5Place;

		public static int Horse6Place;

		public static int RaceSeed;

		public static int SelectedHorse;

		public static int ChipsToAddRemove;

		public static bool RunningInsideTrackScaleform;

		public static int LastGUIButton;

		public static int frame;

		public static float Screen;

		public static float Tab;

		public static int GUIAPP;

		public static int GUIBIX;

		public static bool GUICheckBool;

		public static Control Key = Control.Attack;

		public static Control BackKey = Control.Aim;

		public static int SelectedMCBusiness;

		public static int SelectedScreenID;

		public static int SelectedRollover;

		public static int SelectedBus;

		public static int SelectedRoll;

		public static int SelectedScr;

		public static bool ShowDebug;

		public static bool ShowingOverlay;

		public static Scaleform GUIscaleform;

		public static int HorseRacingPayout;

		public static bool CasinoScreen = true;

		public static bool CasinoScreen1 = true;

		public static int CasScreen;

		public static int CasScreen1;

		public static int CasScreen2;

		public static int CasScreen3;

		public static int CasScreen4;

		public static int CasScreen5;

		public static int CasScreen6;

		public static int CasScreen7;

		public static int CasScreen8;

		public static int CasScreen9;

		public static int CasScreen10;

		public static int CasScreen11;

		public static int CasScreen12;

		public static int CasScreen13;

		public static int CasScreen14;

		public static int CasScreen15;

		public static int CasScreen16;

		public static int CasMovie;

		public static int CasStage;

		public static bool CleanUpCas;

		public static int Interior107;

		public static string Audio_Frontend = "dlc_vw_table_games_frontend_sounds";

		public static string Audio_TableGames = "dlc_vw_table_games_sounds";

		public static Prop TempCard;

		public static CardMove CardMoveStatus = CardMove.Disabled;

		public static CasinoGamePlaying CurrentGame = CasinoGamePlaying.None;

		public static bool CanRunModRightNow = true;

		public static string GetCurrentRouletteBetChipModel()
		{
			return GetBetChipModel(ReturnClosestBetAmtFromCustomBet((int)Bet), false);
		}

		public static Vector3 THREE_CARD_POKER_GET_PLAYER_BET_POSITION_OFFSET(int iBetAmount, BetTypeID BetID, int iPlayerID, bool bHighLimitTable, bool bPlayBet = false)
		{
			float z = 0f;
			Vector3 result = default(Vector3);
			if (!bHighLimitTable)
			{
				switch (iBetAmount)
				{
				case 10:
					z = 0.95f;
					break;
				case 20:
					z = 0.896f;
					break;
				case 30:
					z = 0.901f;
					break;
				case 40:
					z = 0.907f;
					break;
				case 50:
					z = 0.95f;
					break;
				case 60:
					z = 0.917f;
					break;
				case 70:
					z = 0.922f;
					break;
				case 80:
					z = 0.927f;
					break;
				case 90:
					z = 0.932f;
					break;
				case 100:
					z = 0.95f;
					break;
				case 150:
					z = 0.904f;
					break;
				case 200:
					z = 0.899f;
					break;
				case 250:
					z = 0.914f;
					break;
				case 300:
					z = 0.904f;
					break;
				case 350:
					z = 0.924f;
					break;
				case 400:
					z = 0.91f;
					break;
				case 450:
					z = 0.935f;
					break;
				case 500:
					z = 0.95f;
					break;
				case 1000:
					z = 0.95f;
					break;
				case 1500:
					z = 0.904f;
					break;
				case 2000:
					z = 0.899f;
					break;
				case 2500:
					z = 0.915f;
					break;
				case 3000:
					z = 0.904f;
					break;
				case 3500:
					z = 0.925f;
					break;
				case 4000:
					z = 0.91f;
					break;
				case 4500:
					z = 0.935f;
					break;
				case 5000:
					z = 0.95f;
					break;
				case 6000:
					z = 0.919f;
					break;
				case 7000:
					z = 0.924f;
					break;
				case 8000:
					z = 0.93f;
					break;
				case 9000:
					z = 0.935f;
					break;
				case 10000:
					z = 0.95f;
					break;
				case 15000:
					z = 0.902f;
					break;
				case 20000:
					z = 0.897f;
					break;
				case 25000:
					z = 0.912f;
					break;
				case 30000:
					z = 0.902f;
					break;
				case 35000:
					z = 0.922f;
					break;
				case 40000:
					z = 0.907f;
					break;
				case 45000:
					z = 0.932f;
					break;
				case 50000:
					z = 0.912f;
					break;
				}
				switch (iPlayerID)
				{
				case 0:
					switch (BetID)
					{
					case BetTypeID.Ante:
						result = new Vector3(0.59535f, 0.200875f, 0f);
						break;
					case BetTypeID.PairPlus:
						result = new Vector3(0.51655f, 0.2268f, 0f);
						break;
					case BetTypeID.Play:
						result = new Vector3(0.689125f, 0.171575f, 0f);
						break;
					}
					break;
				case 1:
					switch (BetID)
					{
					case BetTypeID.Ante:
						result = new Vector3(0.247825f, -0.123625f, 0f);
						break;
					case BetTypeID.PairPlus:
						result = new Vector3(0.2163f, -0.04745f, 0f);
						break;
					case BetTypeID.Play:
						result = new Vector3(0.2869f, -0.211925f, 0f);
						break;
					}
					break;
				case 2:
					switch (BetID)
					{
					case BetTypeID.Ante:
						result = new Vector3(-0.2804f, -0.109775f, 0f);
						break;
					case BetTypeID.PairPlus:
						result = new Vector3(-0.2552f, -0.031225f, 0f);
						break;
					case BetTypeID.Play:
						result = new Vector3(-0.30935f, -0.205675f, 0f);
						break;
					}
					break;
				case 3:
					switch (BetID)
					{
					case BetTypeID.Ante:
						result = new Vector3(-0.606975f, 0.249675f, 0f);
						break;
					case BetTypeID.PairPlus:
						result = new Vector3(-0.529875f, 0.281425f, 0f);
						break;
					case BetTypeID.Play:
						result = new Vector3(-0.69795f, 0.211525f, 0f);
						break;
					}
					break;
				}
			}
			else
			{
				switch (iBetAmount)
				{
				case 10:
					z = 0.95f;
					break;
				case 20:
					z = 0.896f;
					break;
				case 30:
					z = 0.901f;
					break;
				case 40:
					z = 0.907f;
					break;
				case 50:
					z = 0.95f;
					break;
				case 60:
					z = 0.917f;
					break;
				case 70:
					z = 0.922f;
					break;
				case 80:
					z = 0.927f;
					break;
				case 90:
					z = 0.932f;
					break;
				case 100:
					z = 0.95f;
					break;
				case 150:
					z = 0.904f;
					break;
				case 200:
					z = 0.899f;
					break;
				case 250:
					z = 0.914f;
					break;
				case 300:
					z = 0.904f;
					break;
				case 350:
					z = 0.924f;
					break;
				case 400:
					z = 0.91f;
					break;
				case 450:
					z = 0.935f;
					break;
				case 500:
					z = 0.95f;
					break;
				case 1000:
					z = 0.95f;
					break;
				case 1500:
					z = 0.904f;
					break;
				case 2000:
					z = 0.899f;
					break;
				case 2500:
					z = 0.915f;
					break;
				case 3000:
					z = 0.904f;
					break;
				case 3500:
					z = 0.925f;
					break;
				case 4000:
					z = 0.91f;
					break;
				case 4500:
					z = 0.935f;
					break;
				case 5000:
					z = 0.953f;
					break;
				case 6000:
					z = 0.919f;
					break;
				case 7000:
					z = 0.924f;
					break;
				case 8000:
					z = 0.93f;
					break;
				case 9000:
					z = 0.935f;
					break;
				case 10000:
					z = 0.95f;
					break;
				case 15000:
					z = 0.902f;
					break;
				case 20000:
					z = 0.897f;
					break;
				case 25000:
					z = 0.912f;
					break;
				case 30000:
					z = 0.902f;
					break;
				case 35000:
					z = 0.922f;
					break;
				case 40000:
					z = 0.907f;
					break;
				case 45000:
					z = 0.932f;
					break;
				case 50000:
					z = 0.912f;
					break;
				}
				switch (iBetAmount)
				{
				case 5000:
				case 10000:
				case 15000:
				case 20000:
				case 25000:
				case 30000:
				case 35000:
				case 40000:
				case 45000:
				case 50000:
					switch (iPlayerID)
					{
					case 0:
						switch (BetID)
						{
						case BetTypeID.Ante:
							result = new Vector3(0.597825f, 0.20105f, 0f);
							break;
						case BetTypeID.PairPlus:
							result = new Vector3(0.5151f, 0.22775f, 0f);
							break;
						case BetTypeID.Play:
							result = new Vector3(0.686125f, 0.171625f, 0f);
							break;
						}
						break;
					case 1:
						switch (BetID)
						{
						case BetTypeID.Ante:
							result = new Vector3(0.248425f, -0.126325f, 0f);
							break;
						case BetTypeID.PairPlus:
							result = new Vector3(0.2144f, -0.04415f, 0f);
							break;
						case BetTypeID.Play:
							result = new Vector3(0.285325f, -0.212f, 0f);
							break;
						}
						break;
					case 2:
						switch (BetID)
						{
						case BetTypeID.Ante:
							result = new Vector3(-0.281f, -0.111875f, 0f);
							break;
						case BetTypeID.PairPlus:
							result = new Vector3(-0.254775f, -0.029175f, 0f);
							break;
						case BetTypeID.Play:
							result = new Vector3(-0.308175f, -0.199325f, 0f);
							break;
						}
						break;
					case 3:
						switch (BetID)
						{
						case BetTypeID.Ante:
							result = new Vector3(-0.61015f, 0.24815f, 0f);
							break;
						case BetTypeID.PairPlus:
							result = new Vector3(-0.527375f, 0.283575f, 0f);
							break;
						case BetTypeID.Play:
							result = new Vector3(-0.694475f, 0.214225f, 0f);
							break;
						}
						break;
					}
					break;
				case 10:
				case 20:
				case 30:
				case 40:
				case 50:
				case 60:
				case 70:
				case 80:
				case 90:
				case 100:
				case 150:
				case 200:
				case 250:
				case 300:
				case 350:
				case 400:
				case 450:
				case 500:
				case 1000:
				case 1500:
				case 2000:
				case 2500:
				case 3000:
				case 3500:
				case 4000:
				case 4500:
				case 6000:
				case 7000:
				case 8000:
				case 9000:
					switch (iPlayerID)
					{
					case 0:
						switch (BetID)
						{
						case BetTypeID.Ante:
							result = new Vector3(0.59535f, 0.200875f, 0f);
							break;
						case BetTypeID.PairPlus:
							result = new Vector3(0.51655f, 0.2268f, 0f);
							break;
						case BetTypeID.Play:
							result = new Vector3(0.689125f, 0.171575f, 0f);
							break;
						}
						break;
					case 1:
						switch (BetID)
						{
						case BetTypeID.Ante:
							result = new Vector3(0.247825f, -0.123625f, 0f);
							break;
						case BetTypeID.PairPlus:
							result = new Vector3(0.2163f, -0.04745f, 0f);
							break;
						case BetTypeID.Play:
							result = new Vector3(0.2869f, -0.211925f, 0f);
							break;
						}
						break;
					case 2:
						switch (BetID)
						{
						case BetTypeID.Ante:
							result = new Vector3(-0.2804f, -0.109775f, 0f);
							break;
						case BetTypeID.PairPlus:
							result = new Vector3(-0.2552f, -0.031225f, 0f);
							break;
						case BetTypeID.Play:
							result = new Vector3(-0.30935f, -0.205675f, 0f);
							break;
						}
						break;
					case 3:
						switch (BetID)
						{
						case BetTypeID.Ante:
							result = new Vector3(-0.606975f, 0.249675f, 0f);
							break;
						case BetTypeID.PairPlus:
							result = new Vector3(-0.529875f, 0.281425f, 0f);
							break;
						case BetTypeID.Play:
							result = new Vector3(-0.69795f, 0.211525f, 0f);
							break;
						}
						break;
					}
					break;
				}
			}
			if (!bPlayBet)
			{
				result.Z = z;
			}
			else
			{
				result.Z = z;
				switch (iBetAmount)
				{
				case 10:
				case 50:
				case 100:
				case 500:
				case 1000:
				case 5000:
				case 10000:
					result.Z += 0.004f;
					break;
				}
			}
			return result;
		}

		public static Vector3 THREE_CARD_POKER_GET_PLAYER_BET_ROTATION_OFFSET(int iBetAmount, BetTypeID BetID, int iPlayerID, bool bHighLimitTable)
		{
			if (!bHighLimitTable)
			{
				switch (iPlayerID)
				{
				case 0:
					switch (BetID)
					{
					case BetTypeID.Ante:
						return new Vector3(0f, 0f, 66.96f);
					case BetTypeID.PairPlus:
						return new Vector3(0f, 0f, 66.96f);
					case BetTypeID.Play:
						return new Vector3(0f, 0f, 64.08f);
					}
					break;
				case 1:
					switch (BetID)
					{
					case BetTypeID.Ante:
						return new Vector3(0f, 0f, 21.24f);
					case BetTypeID.PairPlus:
						return new Vector3(0f, 0f, 15.48f);
					case BetTypeID.Play:
						return new Vector3(0f, 0f, 20.16f);
					}
					break;
				case 2:
					switch (BetID)
					{
					case BetTypeID.Ante:
						return new Vector3(0f, 0f, -21.6f);
					case BetTypeID.PairPlus:
						return new Vector3(0f, 0f, -25.56f);
					case BetTypeID.Play:
						return new Vector3(0f, 0f, -21.96f);
					}
					break;
				case 3:
					switch (BetID)
					{
					case BetTypeID.Ante:
						return new Vector3(0f, 0f, -71.64f);
					case BetTypeID.PairPlus:
						return new Vector3(0f, 0f, -70.2f);
					case BetTypeID.Play:
						return new Vector3(0f, 0f, -68.4f);
					}
					break;
				}
			}
			else
			{
				switch (iBetAmount)
				{
				case 5000:
				case 10000:
				case 15000:
				case 20000:
				case 25000:
				case 30000:
				case 35000:
				case 40000:
				case 45000:
				case 50000:
					switch (iPlayerID)
					{
					case 0:
						switch (BetID)
						{
						case BetTypeID.Ante:
							return new Vector3(0f, 0f, 68.76f);
						case BetTypeID.PairPlus:
							return new Vector3(0f, 0f, 68.76f);
						case BetTypeID.Play:
							return new Vector3(0f, 0f, 69.48f);
						}
						break;
					case 1:
						switch (BetID)
						{
						case BetTypeID.Ante:
							return new Vector3(0f, 0f, 22.68f);
						case BetTypeID.PairPlus:
							return new Vector3(0f, 0f, 21.96f);
						case BetTypeID.Play:
							return new Vector3(0f, 0f, 22.68f);
						}
						break;
					case 2:
						switch (BetID)
						{
						case BetTypeID.Ante:
							return new Vector3(0f, 0f, -21.96f);
						case BetTypeID.PairPlus:
							return new Vector3(0f, 0f, -23.04f);
						case BetTypeID.Play:
							return new Vector3(0f, 0f, -21.6f);
						}
						break;
					case 3:
						switch (BetID)
						{
						case BetTypeID.Ante:
							return new Vector3(0f, 0f, -68.04f);
						case BetTypeID.PairPlus:
							return new Vector3(0f, 0f, -69.84f);
						case BetTypeID.Play:
							return new Vector3(0f, 0f, -69.84f);
						}
						break;
					}
					break;
				case 10:
				case 20:
				case 30:
				case 40:
				case 50:
				case 60:
				case 70:
				case 80:
				case 90:
				case 100:
				case 150:
				case 200:
				case 250:
				case 300:
				case 350:
				case 400:
				case 450:
				case 500:
				case 1000:
				case 1500:
				case 2000:
				case 2500:
				case 3000:
				case 3500:
				case 4000:
				case 4500:
				case 6000:
				case 7000:
				case 8000:
				case 9000:
					switch (iPlayerID)
					{
					case 0:
						switch (BetID)
						{
						case BetTypeID.Ante:
							return new Vector3(0f, 0f, 66.96f);
						case BetTypeID.PairPlus:
							return new Vector3(0f, 0f, 66.96f);
						case BetTypeID.Play:
							return new Vector3(0f, 0f, 64.08f);
						}
						break;
					case 1:
						switch (BetID)
						{
						case BetTypeID.Ante:
							return new Vector3(0f, 0f, 21.24f);
						case BetTypeID.PairPlus:
							return new Vector3(0f, 0f, 15.48f);
						case BetTypeID.Play:
							return new Vector3(0f, 0f, 20.16f);
						}
						break;
					case 2:
						switch (BetID)
						{
						case BetTypeID.Ante:
							return new Vector3(0f, 0f, -21.6f);
						case BetTypeID.PairPlus:
							return new Vector3(0f, 0f, -25.56f);
						case BetTypeID.Play:
							return new Vector3(0f, 0f, -21.96f);
						}
						break;
					case 3:
						switch (BetID)
						{
						case BetTypeID.Ante:
							return new Vector3(0f, 0f, -71.64f);
						case BetTypeID.PairPlus:
							return new Vector3(0f, 0f, -70.2f);
						case BetTypeID.Play:
							return new Vector3(0f, 0f, -68.4f);
						}
						break;
					}
					break;
				}
			}
			return new Vector3(0f, 0f, 0f);
		}

		public static int ROULETTE_GET_BALL_LAND_INDEX_FROM_ANIM_INDEX(int iRouletteNumber)
		{
			switch (iRouletteNumber)
			{
			case 1:
				return 0;
			case 2:
				return 28;
			case 3:
				return 9;
			case 4:
				return 26;
			case 5:
				return 30;
			case 6:
				return 11;
			case 7:
				return 7;
			case 8:
				return 20;
			case 9:
				return 32;
			case 10:
				return 17;
			case 11:
				return 5;
			case 12:
				return 22;
			case 13:
				return 34;
			case 14:
				return 15;
			case 15:
				return 3;
			case 16:
				return 24;
			case 17:
				return 36;
			case 18:
				return 13;
			case 19:
				return 1;
			case 20:
				return 37;
			case 21:
				return 27;
			case 22:
				return 10;
			case 23:
				return 25;
			case 24:
				return 29;
			case 25:
				return 12;
			case 26:
				return 8;
			case 27:
				return 19;
			case 28:
				return 31;
			case 30:
				return 18;
			case 31:
				return 21;
			case 32:
				return 6;
			case 33:
				return 16;
			case 34:
				return 33;
			case 35:
				return 23;
			case 36:
				return 4;
			case 37:
				return 35;
			case 38:
				return 2;
			case 39:
				return 14;
			default:
				return -1;
			}
		}

		public static int ROULETTE_GET_ANIMATION_INDEX_FOR_ROULETTE_NUMBER(int iRouletteNumber)
		{
			switch (iRouletteNumber)
			{
			case 0:
				return 20;
			case 28:
				return 21;
			case 9:
				return 22;
			case 26:
				return 23;
			case 30:
				return 24;
			case 11:
				return 25;
			case 7:
				return 26;
			case 20:
				return 27;
			case 32:
				return 28;
			case 17:
				return 29;
			case 5:
				return 30;
			case 22:
				return 31;
			case 34:
				return 32;
			case 15:
				return 33;
			case 3:
				return 34;
			case 24:
				return 35;
			case 36:
				return 36;
			case 13:
				return 37;
			case 1:
				return 38;
			case 37:
				return 1;
			case 27:
				return 2;
			case 10:
				return 3;
			case 25:
				return 4;
			case 29:
				return 5;
			case 12:
				return 6;
			case 8:
				return 7;
			case 19:
				return 8;
			case 31:
				return 9;
			case 18:
				return 10;
			case 6:
				return 11;
			case 21:
				return 12;
			case 33:
				return 13;
			case 16:
				return 14;
			case 4:
				return 15;
			case 23:
				return 16;
			case 35:
				return 17;
			case 14:
				return 18;
			case 2:
				return 19;
			default:
				return -1;
			}
		}

		public static void SetBallToStartTransform(Entity objBall, Prop Table)
		{
			Vector3 vTableWheelPos = ROULETTE_PROPS_GET_WHEEL_BONE_POSITION(Table);
			Vector3 vTableWheelRot = ROULETTE_ANIM_GET_WHEEL_START_ROTATION(Table);
			SET_ENTITY_COORDS(objBall, vTableWheelPos);
			SET_ENTITY_ROTATION(objBall, vTableWheelRot);
		}

		public static Vector3 ROULETTE_PROPS_GET_WHEEL_BONE_POSITION(Entity objTable)
		{
			string strBoneName = ROULETTE_ANIM_GET_WHEEL_BONE_NAME();
			return ROULETTE_PROPS_GET_ENTITY_BONE_POSITION_SAFELY(objTable, strBoneName);
		}

		public static string ROULETTE_ANIM_GET_PROP_CLIP(RouletteAnimClips eAnimClipToGet, int iVariation = 0)
		{
			switch (eAnimClipToGet)
			{
			case RouletteAnimClips.ROULETTE_ANIM_CLIP_PROP_WHEEL_SPIN_INTRO:
				return "intro_wheel";
			case RouletteAnimClips.ROULETTE_ANIM_CLIP_PROP_WHEEL_SPIN_LOOP:
				return "loop_wheel";
			case RouletteAnimClips.ROULETTE_ANIM_CLIP_PROP_WHEEL_SPIN_OUTRO:
				return ROULETTE_ANIM_GET_TABLE_WHEEL_SPIN_EXIT_VARIATION(iVariation);
			case RouletteAnimClips.ROULETTE_ANIM_CLIP_PROP_BALL_SPIN_INTRO:
				return "intro_ball";
			case RouletteAnimClips.ROULETTE_ANIM_CLIP_PROP_BALL_SPIN_LOOP:
				return "loop_ball";
			case RouletteAnimClips.ROULETTE_ANIM_CLIP_PROP_BALL_SPIN_OUTRO:
				return ROULETTE_ANIM_GET_TABLE_BALL_SPIN_EXIT_VARIATION(iVariation);
			default:
				return "";
			}
		}

		public static Vector3 ROULETTE_PROPS_GET_ENTITY_BONE_POSITION_SAFELY(Entity objWithBone, string strBoneName)
		{
			if (!ROULETTE_PROPS_IS_ENTITY_BONE_SAFE_TO_ACCESS(objWithBone, strBoneName))
			{
				return new Vector3(0f, 0f, 0f);
			}
			int iBoneIndex = GET_ENTITY_BONE_INDEX_BY_NAME(objWithBone, strBoneName);
			return GET_ENTITY_BONE_POSTION(objWithBone, iBoneIndex);
		}

		public static Vector3 ROULETTE_PROPS_GET_ENTITY_BONE_ROTATION_SAFELY(Entity objWithBone, string strBoneName)
		{
			if (!ROULETTE_PROPS_IS_ENTITY_BONE_SAFE_TO_ACCESS(objWithBone, strBoneName))
			{
				return new Vector3(0f, 0f, 0f);
			}
			int bone = GET_ENTITY_BONE_INDEX_BY_NAME(objWithBone, strBoneName);
			return GET_ENTITY_BONE_ROTATION((Prop)objWithBone, bone);
		}

		public static Vector3 ROULETTE_ANIM_GET_WHEEL_START_ROTATION(Prop Table)
		{
			return ROULETTE_PROPS_GET_WHEEL_CURRENT_ROTATION(Table);
		}

		public static Vector3 ROULETTE_PROPS_GET_WHEEL_CURRENT_ROTATION(Entity objWheel)
		{
			string strBoneName = ROULETTE_ANIM_GET_WHEEL_BONE_NAME();
			return ROULETTE_PROPS_GET_ENTITY_BONE_ROTATION_SAFELY(objWheel, strBoneName);
		}

		public static void SET_ENTITY_COORDS(Entity objBall, Vector3 vTableWheelPos)
		{
			objBall.PositionNoOffset = vTableWheelPos;
		}

		public static void SET_ENTITY_ROTATION(Entity objBall, Vector3 vTableWheelRot)
		{
			Function.Call(Hash.SET_ENTITY_ROTATION, objBall, vTableWheelRot.X, vTableWheelRot.Y, vTableWheelRot.Z);
		}

		public static string ROULETTE_ANIM_GET_WHEEL_BONE_NAME()
		{
			return "Roulette_Wheel";
		}

		public static string ROULETTE_ANIM_GET_TABLE_WHEEL_SPIN_EXIT_VARIATION(int iSlotAnimIndex)
		{
			switch (iSlotAnimIndex)
			{
			case 1:
				return "exit_1_wheel";
			case 2:
				return "exit_2_wheel";
			case 3:
				return "exit_3_wheel";
			case 4:
				return "exit_4_wheel";
			case 5:
				return "exit_5_wheel";
			case 6:
				return "exit_6_wheel";
			case 7:
				return "exit_7_wheel";
			case 8:
				return "exit_8_wheel";
			case 9:
				return "exit_9_wheel";
			case 10:
				return "exit_10_wheel";
			case 11:
				return "exit_11_wheel";
			case 12:
				return "exit_12_wheel";
			case 13:
				return "exit_13_wheel";
			case 14:
				return "exit_14_wheel";
			case 15:
				return "exit_15_wheel";
			case 16:
				return "exit_16_wheel";
			case 17:
				return "exit_17_wheel";
			case 18:
				return "exit_18_wheel";
			case 19:
				return "exit_19_wheel";
			case 20:
				return "exit_20_wheel";
			case 21:
				return "exit_21_wheel";
			case 22:
				return "exit_22_wheel";
			case 23:
				return "exit_23_wheel";
			case 24:
				return "exit_24_wheel";
			case 25:
				return "exit_25_wheel";
			case 26:
				return "exit_26_wheel";
			case 27:
				return "exit_27_wheel";
			case 28:
				return "exit_28_wheel";
			case 29:
				return "exit_29_wheel";
			case 30:
				return "exit_30_wheel";
			case 31:
				return "exit_31_wheel";
			case 32:
				return "exit_32_wheel";
			case 33:
				return "exit_33_wheel";
			case 34:
				return "exit_34_wheel";
			case 35:
				return "exit_35_wheel";
			case 36:
				return "exit_36_wheel";
			case 37:
				return "exit_37_wheel";
			case 38:
				return "exit_38_wheel";
			default:
				return "Error";
			}
		}

		public static string ROULETTE_ANIM_GET_TABLE_BALL_SPIN_EXIT_VARIATION(int iSlotAnimIndex)
		{
			switch (iSlotAnimIndex)
			{
			case 1:
				return "exit_1_ball";
			case 2:
				return "exit_2_ball";
			case 3:
				return "exit_3_ball";
			case 4:
				return "exit_4_ball";
			case 5:
				return "exit_5_ball";
			case 6:
				return "exit_6_ball";
			case 7:
				return "exit_7_ball";
			case 8:
				return "exit_8_ball";
			case 9:
				return "exit_9_ball";
			case 10:
				return "exit_10_ball";
			case 11:
				return "exit_11_ball";
			case 12:
				return "exit_12_ball";
			case 13:
				return "exit_13_ball";
			case 14:
				return "exit_14_ball";
			case 15:
				return "exit_15_ball";
			case 16:
				return "exit_16_ball";
			case 17:
				return "exit_17_ball";
			case 18:
				return "exit_18_ball";
			case 19:
				return "exit_19_ball";
			case 20:
				return "exit_20_ball";
			case 21:
				return "exit_21_ball";
			case 22:
				return "exit_22_ball";
			case 23:
				return "exit_23_ball";
			case 24:
				return "exit_24_ball";
			case 25:
				return "exit_25_ball";
			case 26:
				return "exit_26_ball";
			case 27:
				return "exit_27_ball";
			case 28:
				return "exit_28_ball";
			case 29:
				return "exit_29_ball";
			case 30:
				return "exit_30_ball";
			case 31:
				return "exit_31_ball";
			case 32:
				return "exit_32_ball";
			case 33:
				return "exit_33_ball";
			case 34:
				return "exit_34_ball";
			case 35:
				return "exit_35_ball";
			case 36:
				return "exit_36_ball";
			case 37:
				return "exit_37_ball";
			case 38:
				return "exit_38_ball";
			default:
				return "Error";
			}
		}

		public static bool ROULETTE_PROPS_IS_ENTITY_BONE_SAFE_TO_ACCESS(Entity objWithBone, string strBoneName)
		{
			if (string.IsNullOrEmpty(strBoneName))
			{
				return false;
			}
			if (!objWithBone.Exists())
			{
				return false;
			}
			if (!Function.Call<bool>(Hash.DOES_ENTITY_HAVE_DRAWABLE, objWithBone))
			{
				return false;
			}
			if (GET_ENTITY_BONE_INDEX_BY_NAME(objWithBone, strBoneName) == -1)
			{
				return false;
			}
			return true;
		}

		public static int GET_ENTITY_BONE_INDEX_BY_NAME(Entity objWithBone, string strBoneName)
		{
			return Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, objWithBone, strBoneName);
		}

		public static Vector3 GET_ENTITY_BONE_POSTION(Entity objWithBone, int iBoneIndex)
		{
			return Function.Call<Vector3>(Hash.GET_ENTITY_BONE_POSTION, objWithBone, iBoneIndex);
		}

		public static bool IsCasinoChipInArea(Prop C, Vector3 X1, Vector3 X2)
		{
			return Function.Call<bool>(Hash.IS_ENTITY_IN_AREA, C, X1.X, X1.Y, X1.Z, X2.X, X2.Y, X2.Z, 0, 1, 0);
		}

		public static bool DoesObjectExist(Prop Marker)
		{
			bool result = true;
			if (Marker == null)
			{
				result = false;
			}
			else if (!Marker.Exists())
			{
				result = false;
			}
			return result;
		}

		public static Vector3 GetRouletteBetPosition(Prop Table, RouletteBetPosition Position)
		{
			float z = 0.935f;
			Vector3 offsetPosition = Table.GetOffsetPosition(new Vector3(-0.14f, 0.11f, z));
			Vector3 offsetPosition2 = Table.GetOffsetPosition(new Vector3(-0.14f, -0.14f, z));
			Vector3 offsetPosition3 = Table.GetOffsetPosition(new Vector3(-0.059f, -0.191f, z));
			Vector3 offsetPosition4 = Table.GetOffsetPosition(new Vector3(-0.059f, -0.024f, z));
			Vector3 offsetPosition5 = Table.GetOffsetPosition(new Vector3(-0.059f, 0.142f, z));
			Vector3 offsetPosition6 = Table.GetOffsetPosition(new Vector3(0.022f, -0.191f, z));
			Vector3 offsetPosition7 = Table.GetOffsetPosition(new Vector3(0.022f, -0.024f, z));
			Vector3 offsetPosition8 = Table.GetOffsetPosition(new Vector3(0.022f, 0.142f, z));
			Vector3 offsetPosition9 = Table.GetOffsetPosition(new Vector3(0.103f, -0.191f, z));
			Vector3 offsetPosition10 = Table.GetOffsetPosition(new Vector3(0.103f, -0.024f, z));
			Vector3 offsetPosition11 = Table.GetOffsetPosition(new Vector3(0.103f, 0.142f, z));
			Vector3 offsetPosition12 = Table.GetOffsetPosition(new Vector3(0.185f, -0.191f, z));
			Vector3 offsetPosition13 = Table.GetOffsetPosition(new Vector3(0.185f, -0.024f, z));
			Vector3 offsetPosition14 = Table.GetOffsetPosition(new Vector3(0.185f, 0.142f, z));
			Vector3 offsetPosition15 = Table.GetOffsetPosition(new Vector3(0.266f, -0.191f, z));
			Vector3 offsetPosition16 = Table.GetOffsetPosition(new Vector3(0.266f, -0.024f, z));
			Vector3 offsetPosition17 = Table.GetOffsetPosition(new Vector3(0.266f, 0.142f, z));
			Vector3 offsetPosition18 = Table.GetOffsetPosition(new Vector3(0.348f, -0.191f, z));
			Vector3 offsetPosition19 = Table.GetOffsetPosition(new Vector3(0.348f, -0.024f, z));
			Vector3 offsetPosition20 = Table.GetOffsetPosition(new Vector3(0.348f, 0.142f, z));
			Vector3 offsetPosition21 = Table.GetOffsetPosition(new Vector3(0.43f, -0.191f, z));
			Vector3 offsetPosition22 = Table.GetOffsetPosition(new Vector3(0.43f, -0.024f, z));
			Vector3 offsetPosition23 = Table.GetOffsetPosition(new Vector3(0.43f, 0.142f, z));
			Vector3 offsetPosition24 = Table.GetOffsetPosition(new Vector3(0.51f, -0.191f, z));
			Vector3 offsetPosition25 = Table.GetOffsetPosition(new Vector3(0.51f, -0.024f, z));
			Vector3 offsetPosition26 = Table.GetOffsetPosition(new Vector3(0.51f, 0.142f, z));
			Vector3 offsetPosition27 = Table.GetOffsetPosition(new Vector3(0.592f, -0.191f, z));
			Vector3 offsetPosition28 = Table.GetOffsetPosition(new Vector3(0.592f, -0.024f, z));
			Vector3 offsetPosition29 = Table.GetOffsetPosition(new Vector3(0.592f, 0.142f, z));
			Vector3 offsetPosition30 = Table.GetOffsetPosition(new Vector3(0.674f, -0.191f, z));
			Vector3 offsetPosition31 = Table.GetOffsetPosition(new Vector3(0.674f, -0.024f, z));
			Vector3 offsetPosition32 = Table.GetOffsetPosition(new Vector3(0.674f, 0.142f, z));
			Vector3 offsetPosition33 = Table.GetOffsetPosition(new Vector3(0.756f, -0.191f, z));
			Vector3 offsetPosition34 = Table.GetOffsetPosition(new Vector3(0.756f, -0.024f, z));
			Vector3 offsetPosition35 = Table.GetOffsetPosition(new Vector3(0.756f, 0.142f, z));
			Vector3 offsetPosition36 = Table.GetOffsetPosition(new Vector3(0.837f, -0.191f, z));
			Vector3 offsetPosition37 = Table.GetOffsetPosition(new Vector3(0.837f, -0.024f, z));
			Vector3 offsetPosition38 = Table.GetOffsetPosition(new Vector3(0.837f, 0.142f, z));
			Vector3 offsetPosition39 = Table.GetOffsetPosition(new Vector3(0.91f, -0.185f, z));
			Vector3 offsetPosition40 = Table.GetOffsetPosition(new Vector3(0.91f, -0.025f, z));
			Vector3 offsetPosition41 = Table.GetOffsetPosition(new Vector3(0.91f, 0.14f, z));
			Vector3 offsetPosition42 = Table.GetOffsetPosition(new Vector3(0.794f, -0.374f, z));
			Vector3 offsetPosition43 = Table.GetOffsetPosition(new Vector3(0.634f, -0.374f, z));
			Vector3 offsetPosition44 = Table.GetOffsetPosition(new Vector3(-0.02f, -0.374f, z));
			Vector3 offsetPosition45 = Table.GetOffsetPosition(new Vector3(0.135f, -0.374f, z));
			Vector3 offsetPosition46 = Table.GetOffsetPosition(new Vector3(0.309f, -0.374f, z));
			Vector3 offsetPosition47 = Table.GetOffsetPosition(new Vector3(0.474f, -0.374f, z));
			Vector3 offsetPosition48 = Table.GetOffsetPosition(new Vector3(0.0699f, -0.304f, z));
			Vector3 offsetPosition49 = Table.GetOffsetPosition(new Vector3(0.389f, -0.304f, z));
			Vector3 offsetPosition50 = Table.GetOffsetPosition(new Vector3(0.719f, -0.304f, z));
			Vector3 offsetPosition51 = Table.GetOffsetPosition(new Vector3(-0.017f, -0.11f, z));
			Vector3 offsetPosition52 = Table.GetOffsetPosition(new Vector3(-0.017f, 0.058f, z));
			Vector3 offsetPosition53 = Table.GetOffsetPosition(new Vector3(0.066f, -0.11f, z));
			Vector3 offsetPosition54 = Table.GetOffsetPosition(new Vector3(0.066f, 0.058f, z));
			Vector3 offsetPosition55 = Table.GetOffsetPosition(new Vector3(0.146f, -0.11f, z));
			Vector3 offsetPosition56 = Table.GetOffsetPosition(new Vector3(0.146f, 0.058f, z));
			Vector3 offsetPosition57 = Table.GetOffsetPosition(new Vector3(0.227f, -0.11f, z));
			Vector3 offsetPosition58 = Table.GetOffsetPosition(new Vector3(0.227f, 0.058f, z));
			Vector3 offsetPosition59 = Table.GetOffsetPosition(new Vector3(0.309f, -0.11f, z));
			Vector3 offsetPosition60 = Table.GetOffsetPosition(new Vector3(0.309f, 0.058f, z));
			Vector3 offsetPosition61 = Table.GetOffsetPosition(new Vector3(0.391f, -0.11f, z));
			Vector3 offsetPosition62 = Table.GetOffsetPosition(new Vector3(0.391f, 0.058f, z));
			Vector3 offsetPosition63 = Table.GetOffsetPosition(new Vector3(0.472f, -0.11f, z));
			Vector3 offsetPosition64 = Table.GetOffsetPosition(new Vector3(0.472f, 0.058f, z));
			Vector3 offsetPosition65 = Table.GetOffsetPosition(new Vector3(0.553f, -0.11f, z));
			Vector3 offsetPosition66 = Table.GetOffsetPosition(new Vector3(0.553f, 0.058f, z));
			Vector3 offsetPosition67 = Table.GetOffsetPosition(new Vector3(0.634f, -0.11f, z));
			Vector3 offsetPosition68 = Table.GetOffsetPosition(new Vector3(0.634f, 0.058f, z));
			Vector3 offsetPosition69 = Table.GetOffsetPosition(new Vector3(0.715f, -0.11f, z));
			Vector3 offsetPosition70 = Table.GetOffsetPosition(new Vector3(0.715f, 0.058f, z));
			Vector3 offsetPosition71 = Table.GetOffsetPosition(new Vector3(0.793f, -0.11f, z));
			Vector3 offsetPosition72 = Table.GetOffsetPosition(new Vector3(0.793f, 0.058f, z));
			Vector3 offsetPosition73 = Table.GetOffsetPosition(new Vector3(-0.057f, -0.275f, z));
			Vector3 offsetPosition74 = Table.GetOffsetPosition(new Vector3(-0.016f, -0.275f, z));
			Vector3 offsetPosition75 = Table.GetOffsetPosition(new Vector3(0.025f, -0.275f, z));
			Vector3 offsetPosition76 = Table.GetOffsetPosition(new Vector3(0.066f, -0.275f, z));
			Vector3 offsetPosition77 = Table.GetOffsetPosition(new Vector3(0.105f, -0.275f, z));
			Vector3 offsetPosition78 = Table.GetOffsetPosition(new Vector3(0.145f, -0.275f, z));
			Vector3 offsetPosition79 = Table.GetOffsetPosition(new Vector3(0.187f, -0.275f, z));
			Vector3 offsetPosition80 = Table.GetOffsetPosition(new Vector3(0.226f, -0.275f, z));
			Vector3 offsetPosition81 = Table.GetOffsetPosition(new Vector3(0.27f, -0.275f, z));
			Vector3 offsetPosition82 = Table.GetOffsetPosition(new Vector3(0.308f, -0.275f, z));
			Vector3 offsetPosition83 = Table.GetOffsetPosition(new Vector3(0.348f, -0.275f, z));
			Vector3 offsetPosition84 = Table.GetOffsetPosition(new Vector3(0.39f, -0.275f, z));
			Vector3 offsetPosition85 = Table.GetOffsetPosition(new Vector3(0.432f, -0.275f, z));
			Vector3 offsetPosition86 = Table.GetOffsetPosition(new Vector3(0.47f, -0.275f, z));
			Vector3 offsetPosition87 = Table.GetOffsetPosition(new Vector3(0.512f, -0.275f, z));
			Vector3 offsetPosition88 = Table.GetOffsetPosition(new Vector3(0.553f, -0.275f, z));
			Vector3 offsetPosition89 = Table.GetOffsetPosition(new Vector3(0.594f, -0.275f, z));
			Vector3 offsetPosition90 = Table.GetOffsetPosition(new Vector3(0.634f, -0.275f, z));
			Vector3 offsetPosition91 = Table.GetOffsetPosition(new Vector3(0.676f, -0.275f, z));
			Vector3 offsetPosition92 = Table.GetOffsetPosition(new Vector3(0.713f, -0.275f, z));
			Vector3 offsetPosition93 = Table.GetOffsetPosition(new Vector3(0.755f, -0.275f, z));
			Vector3 offsetPosition94 = Table.GetOffsetPosition(new Vector3(0.793f, -0.275f, z));
			Vector3 offsetPosition95 = Table.GetOffsetPosition(new Vector3(0.836f, -0.275f, z));
			Table.GetOffsetPosition(new Vector3(-0.14f, -0.3f, z));
			Vector3 offsetPosition96 = Table.GetOffsetPosition(new Vector3(-0.1042f, -0.02f, z));
			Vector3 offsetPosition97 = Table.GetOffsetPosition(new Vector3(-0.057f, -0.1073f, z));
			Vector3 offsetPosition98 = Table.GetOffsetPosition(new Vector3(-0.057f, 0.0587f, z));
			Vector3 offsetPosition99 = Table.GetOffsetPosition(new Vector3(0.025f, -0.1073f, z));
			Vector3 offsetPosition100 = Table.GetOffsetPosition(new Vector3(0.025f, 0.0587f, z));
			Vector3 offsetPosition101 = Table.GetOffsetPosition(new Vector3(0.105f, -0.1073f, z));
			Vector3 offsetPosition102 = Table.GetOffsetPosition(new Vector3(0.105f, 0.0587f, z));
			Vector3 offsetPosition103 = Table.GetOffsetPosition(new Vector3(0.1855f, -0.1073f, z));
			Vector3 offsetPosition104 = Table.GetOffsetPosition(new Vector3(0.1855f, 0.0587f, z));
			Vector3 offsetPosition105 = Table.GetOffsetPosition(new Vector3(0.27f, -0.1073f, z));
			Vector3 offsetPosition106 = Table.GetOffsetPosition(new Vector3(0.27f, 0.0587f, z));
			Vector3 offsetPosition107 = Table.GetOffsetPosition(new Vector3(0.348f, -0.1073f, z));
			Vector3 offsetPosition108 = Table.GetOffsetPosition(new Vector3(0.348f, 0.0587f, z));
			Vector3 offsetPosition109 = Table.GetOffsetPosition(new Vector3(0.432f, -0.1073f, z));
			Vector3 offsetPosition110 = Table.GetOffsetPosition(new Vector3(0.432f, 0.0587f, z));
			Vector3 offsetPosition111 = Table.GetOffsetPosition(new Vector3(0.5155f, -0.1073f, z));
			Vector3 offsetPosition112 = Table.GetOffsetPosition(new Vector3(0.5155f, 0.0587f, z));
			Vector3 offsetPosition113 = Table.GetOffsetPosition(new Vector3(0.594f, -0.1073f, z));
			Vector3 offsetPosition114 = Table.GetOffsetPosition(new Vector3(0.594f, 0.0587f, z));
			Vector3 offsetPosition115 = Table.GetOffsetPosition(new Vector3(0.676f, -0.1073f, z));
			Vector3 offsetPosition116 = Table.GetOffsetPosition(new Vector3(0.676f, 0.0587f, z));
			Vector3 offsetPosition117 = Table.GetOffsetPosition(new Vector3(0.755f, -0.1073f, z));
			Vector3 offsetPosition118 = Table.GetOffsetPosition(new Vector3(0.755f, 0.0587f, z));
			Vector3 offsetPosition119 = Table.GetOffsetPosition(new Vector3(0.836f, -0.1073f, z));
			Vector3 offsetPosition120 = Table.GetOffsetPosition(new Vector3(0.836f, 0.0587f, z));
			switch (Position)
			{
			case RouletteBetPosition.Num_00:
				return offsetPosition;
			case RouletteBetPosition.Num_0:
				return offsetPosition2;
			case RouletteBetPosition.Num_1:
				return offsetPosition3;
			case RouletteBetPosition.Num_2:
				return offsetPosition4;
			case RouletteBetPosition.Num_3:
				return offsetPosition5;
			case RouletteBetPosition.Num_4:
				return offsetPosition6;
			case RouletteBetPosition.Num_5:
				return offsetPosition7;
			case RouletteBetPosition.Num_6:
				return offsetPosition8;
			case RouletteBetPosition.Num_7:
				return offsetPosition9;
			case RouletteBetPosition.Num_8:
				return offsetPosition10;
			case RouletteBetPosition.Num_9:
				return offsetPosition11;
			case RouletteBetPosition.Num_10:
				return offsetPosition12;
			case RouletteBetPosition.Num_11:
				return offsetPosition13;
			case RouletteBetPosition.Num_12:
				return offsetPosition14;
			case RouletteBetPosition.Num_13:
				return offsetPosition15;
			case RouletteBetPosition.Num_14:
				return offsetPosition16;
			case RouletteBetPosition.Num_15:
				return offsetPosition17;
			case RouletteBetPosition.Num_16:
				return offsetPosition18;
			case RouletteBetPosition.Num_17:
				return offsetPosition19;
			case RouletteBetPosition.Num_18:
				return offsetPosition20;
			case RouletteBetPosition.Num_19:
				return offsetPosition21;
			case RouletteBetPosition.Num_20:
				return offsetPosition22;
			case RouletteBetPosition.Num_21:
				return offsetPosition23;
			case RouletteBetPosition.Num_22:
				return offsetPosition24;
			case RouletteBetPosition.Num_23:
				return offsetPosition25;
			case RouletteBetPosition.Num_24:
				return offsetPosition26;
			case RouletteBetPosition.Num_25:
				return offsetPosition27;
			case RouletteBetPosition.Num_26:
				return offsetPosition28;
			case RouletteBetPosition.Num_27:
				return offsetPosition29;
			case RouletteBetPosition.Num_28:
				return offsetPosition30;
			case RouletteBetPosition.Num_29:
				return offsetPosition31;
			case RouletteBetPosition.Num_30:
				return offsetPosition32;
			case RouletteBetPosition.Num_31:
				return offsetPosition33;
			case RouletteBetPosition.Num_32:
				return offsetPosition34;
			case RouletteBetPosition.Num_33:
				return offsetPosition35;
			case RouletteBetPosition.Num_34:
				return offsetPosition36;
			case RouletteBetPosition.Num_35:
				return offsetPosition37;
			case RouletteBetPosition.Num_36:
				return offsetPosition38;
			case RouletteBetPosition.Col_1:
				return offsetPosition39;
			case RouletteBetPosition.Col_2:
				return offsetPosition40;
			case RouletteBetPosition.Col_3:
				return offsetPosition41;
			case RouletteBetPosition._19to36:
				return offsetPosition42;
			case RouletteBetPosition._Odd:
				return offsetPosition43;
			case RouletteBetPosition._1to18:
				return offsetPosition44;
			case RouletteBetPosition._Even:
				return offsetPosition45;
			case RouletteBetPosition._Red:
				return offsetPosition46;
			case RouletteBetPosition._Black:
				return offsetPosition47;
			case RouletteBetPosition._1st12:
				return offsetPosition48;
			case RouletteBetPosition._2nd12:
				return offsetPosition49;
			case RouletteBetPosition._3nd12:
				return offsetPosition50;
			case RouletteBetPosition._1_2_4_5:
				return offsetPosition51;
			case RouletteBetPosition._2_3_5_6:
				return offsetPosition52;
			case RouletteBetPosition._4_5_7_8:
				return offsetPosition53;
			case RouletteBetPosition._5_6_8_9:
				return offsetPosition54;
			case RouletteBetPosition._7_8_10_11:
				return offsetPosition55;
			case RouletteBetPosition._8_9_11_12:
				return offsetPosition56;
			case RouletteBetPosition._10_11_13_14:
				return offsetPosition57;
			case RouletteBetPosition._11_12_14_15:
				return offsetPosition58;
			case RouletteBetPosition._13_14_16_17:
				return offsetPosition59;
			case RouletteBetPosition._14_15_17_18:
				return offsetPosition60;
			case RouletteBetPosition._16_17_19_20:
				return offsetPosition61;
			case RouletteBetPosition._17_18_20_21:
				return offsetPosition62;
			case RouletteBetPosition._19_20_22_23:
				return offsetPosition63;
			case RouletteBetPosition._20_21_23_24:
				return offsetPosition64;
			case RouletteBetPosition._22_23_25_26:
				return offsetPosition65;
			case RouletteBetPosition._23_24_26_27:
				return offsetPosition66;
			case RouletteBetPosition._25_26_28_29:
				return offsetPosition67;
			case RouletteBetPosition._26_27_29_30:
				return offsetPosition68;
			case RouletteBetPosition._28_29_31_32:
				return offsetPosition69;
			case RouletteBetPosition._29_30_32_33:
				return offsetPosition70;
			case RouletteBetPosition._31_32_34_35:
				return offsetPosition71;
			case RouletteBetPosition._32_33_35_36:
				return offsetPosition72;
			case RouletteBetPosition.Row_1:
				return offsetPosition73;
			case RouletteBetPosition.Segment_1:
				return offsetPosition74;
			case RouletteBetPosition.Row_2:
				return offsetPosition75;
			case RouletteBetPosition.Segment_2:
				return offsetPosition76;
			case RouletteBetPosition.Row_3:
				return offsetPosition77;
			case RouletteBetPosition.Segment_3:
				return offsetPosition78;
			case RouletteBetPosition.Row_4:
				return offsetPosition79;
			case RouletteBetPosition.Segment_4:
				return offsetPosition80;
			case RouletteBetPosition.Row_5:
				return offsetPosition81;
			case RouletteBetPosition.Segment_5:
				return offsetPosition82;
			case RouletteBetPosition.Row_6:
				return offsetPosition83;
			case RouletteBetPosition.Segment_6:
				return offsetPosition84;
			case RouletteBetPosition.Row_7:
				return offsetPosition85;
			case RouletteBetPosition.Segment_7:
				return offsetPosition86;
			case RouletteBetPosition.Row_8:
				return offsetPosition87;
			case RouletteBetPosition.Segment_8:
				return offsetPosition88;
			case RouletteBetPosition.Row_9:
				return offsetPosition89;
			case RouletteBetPosition.Segment_9:
				return offsetPosition90;
			case RouletteBetPosition.Row_10:
				return offsetPosition91;
			case RouletteBetPosition.Segment_10:
				return offsetPosition92;
			case RouletteBetPosition.Row_11:
				return offsetPosition93;
			case RouletteBetPosition.Segment_11:
				return offsetPosition94;
			case RouletteBetPosition.Row_12:
				return offsetPosition95;
			case RouletteBetPosition._1_2_3_00_0:
				return offsetPosition96;
			case RouletteBetPosition._1_2:
				return offsetPosition97;
			case RouletteBetPosition._2_3:
				return offsetPosition98;
			case RouletteBetPosition._4_5:
				return offsetPosition99;
			case RouletteBetPosition._5_6:
				return offsetPosition100;
			case RouletteBetPosition._7_8:
				return offsetPosition101;
			case RouletteBetPosition._8_9:
				return offsetPosition102;
			case RouletteBetPosition._10_11:
				return offsetPosition103;
			case RouletteBetPosition._11_12:
				return offsetPosition104;
			case RouletteBetPosition._13_14:
				return offsetPosition105;
			case RouletteBetPosition._14_15:
				return offsetPosition106;
			case RouletteBetPosition._16_17:
				return offsetPosition107;
			case RouletteBetPosition._17_18:
				return offsetPosition108;
			case RouletteBetPosition._19_20:
				return offsetPosition109;
			case RouletteBetPosition._20_21:
				return offsetPosition110;
			case RouletteBetPosition._22_23:
				return offsetPosition111;
			case RouletteBetPosition._23_24:
				return offsetPosition112;
			case RouletteBetPosition._25_26:
				return offsetPosition113;
			case RouletteBetPosition._26_27:
				return offsetPosition114;
			case RouletteBetPosition._28_29:
				return offsetPosition115;
			case RouletteBetPosition._29_30:
				return offsetPosition116;
			case RouletteBetPosition._31_32:
				return offsetPosition117;
			case RouletteBetPosition._32_33:
				return offsetPosition118;
			case RouletteBetPosition._34_35:
				return offsetPosition119;
			case RouletteBetPosition._35_36:
				return offsetPosition120;
			default:
				return offsetPosition3;
			}
		}

		public static int GetBetMultiplierAtPosition(RouletteBetPosition Position)
		{
			int result = 1;
			switch (Position)
			{
			case RouletteBetPosition.Num_00:
				return 35;
			case RouletteBetPosition.Num_0:
				return 35;
			case RouletteBetPosition.Num_1:
				return 35;
			case RouletteBetPosition.Num_2:
				return 35;
			case RouletteBetPosition.Num_3:
				return 35;
			case RouletteBetPosition.Num_4:
				return 35;
			case RouletteBetPosition.Num_5:
				return 35;
			case RouletteBetPosition.Num_6:
				return 35;
			case RouletteBetPosition.Num_7:
				return 35;
			case RouletteBetPosition.Num_8:
				return 35;
			case RouletteBetPosition.Num_9:
				return 35;
			case RouletteBetPosition.Num_10:
				return 35;
			case RouletteBetPosition.Num_11:
				return 35;
			case RouletteBetPosition.Num_12:
				return 35;
			case RouletteBetPosition.Num_13:
				return 35;
			case RouletteBetPosition.Num_14:
				return 35;
			case RouletteBetPosition.Num_15:
				return 35;
			case RouletteBetPosition.Num_16:
				return 35;
			case RouletteBetPosition.Num_17:
				return 35;
			case RouletteBetPosition.Num_18:
				return 35;
			case RouletteBetPosition.Num_19:
				return 35;
			case RouletteBetPosition.Num_20:
				return 35;
			case RouletteBetPosition.Num_21:
				return 35;
			case RouletteBetPosition.Num_22:
				return 35;
			case RouletteBetPosition.Num_23:
				return 35;
			case RouletteBetPosition.Num_24:
				return 35;
			case RouletteBetPosition.Num_25:
				return 35;
			case RouletteBetPosition.Num_26:
				return 35;
			case RouletteBetPosition.Num_27:
				return 35;
			case RouletteBetPosition.Num_28:
				return 35;
			case RouletteBetPosition.Num_29:
				return 35;
			case RouletteBetPosition.Num_30:
				return 35;
			case RouletteBetPosition.Num_31:
				return 35;
			case RouletteBetPosition.Num_32:
				return 35;
			case RouletteBetPosition.Num_33:
				return 35;
			case RouletteBetPosition.Num_34:
				return 35;
			case RouletteBetPosition.Num_35:
				return 35;
			case RouletteBetPosition.Num_36:
				return 35;
			case RouletteBetPosition.Col_1:
				return 2;
			case RouletteBetPosition.Col_2:
				return 2;
			case RouletteBetPosition.Col_3:
				return 2;
			case RouletteBetPosition._19to36:
				return 1;
			case RouletteBetPosition._Odd:
				return 1;
			case RouletteBetPosition._1to18:
				return 1;
			case RouletteBetPosition._Even:
				return 1;
			case RouletteBetPosition._Red:
				return 1;
			case RouletteBetPosition._Black:
				return 1;
			case RouletteBetPosition._1st12:
				return 2;
			case RouletteBetPosition._2nd12:
				return 2;
			case RouletteBetPosition._3nd12:
				return 2;
			case RouletteBetPosition._1_2_4_5:
				return 8;
			case RouletteBetPosition._2_3_5_6:
				return 8;
			case RouletteBetPosition._4_5_7_8:
				return 8;
			case RouletteBetPosition._5_6_8_9:
				return 8;
			case RouletteBetPosition._7_8_10_11:
				return 8;
			case RouletteBetPosition._8_9_11_12:
				return 8;
			case RouletteBetPosition._10_11_13_14:
				return 8;
			case RouletteBetPosition._11_12_14_15:
				return 8;
			case RouletteBetPosition._13_14_16_17:
				return 8;
			case RouletteBetPosition._14_15_17_18:
				return 8;
			case RouletteBetPosition._16_17_19_20:
				return 8;
			case RouletteBetPosition._17_18_20_21:
				return 8;
			case RouletteBetPosition._19_20_22_23:
				return 8;
			case RouletteBetPosition._20_21_23_24:
				return 8;
			case RouletteBetPosition._22_23_25_26:
				return 8;
			case RouletteBetPosition._23_24_26_27:
				return 8;
			case RouletteBetPosition._25_26_28_29:
				return 8;
			case RouletteBetPosition._26_27_29_30:
				return 8;
			case RouletteBetPosition._28_29_31_32:
				return 8;
			case RouletteBetPosition._29_30_32_33:
				return 8;
			case RouletteBetPosition._31_32_34_35:
				return 8;
			case RouletteBetPosition._32_33_35_36:
				return 8;
			case RouletteBetPosition.Row_1:
				return 11;
			case RouletteBetPosition.Segment_1:
				return 5;
			case RouletteBetPosition.Row_2:
				return 11;
			case RouletteBetPosition.Segment_2:
				return 5;
			case RouletteBetPosition.Row_3:
				return 11;
			case RouletteBetPosition.Segment_3:
				return 5;
			case RouletteBetPosition.Row_4:
				return 11;
			case RouletteBetPosition.Segment_4:
				return 5;
			case RouletteBetPosition.Row_5:
				return 11;
			case RouletteBetPosition.Segment_5:
				return 5;
			case RouletteBetPosition.Row_6:
				return 11;
			case RouletteBetPosition.Segment_6:
				return 5;
			case RouletteBetPosition.Row_7:
				return 11;
			case RouletteBetPosition.Segment_7:
				return 5;
			case RouletteBetPosition.Row_8:
				return 11;
			case RouletteBetPosition.Segment_8:
				return 5;
			case RouletteBetPosition.Row_9:
				return 11;
			case RouletteBetPosition.Segment_9:
				return 5;
			case RouletteBetPosition.Row_10:
				return 11;
			case RouletteBetPosition.Segment_10:
				return 5;
			case RouletteBetPosition.Row_11:
				return 11;
			case RouletteBetPosition.Segment_11:
				return 5;
			case RouletteBetPosition.Row_12:
				return 11;
			case RouletteBetPosition._1_2_3_00_0:
				return 6;
			case RouletteBetPosition._1_2:
				return 17;
			case RouletteBetPosition._2_3:
				return 17;
			case RouletteBetPosition._4_5:
				return 17;
			case RouletteBetPosition._5_6:
				return 17;
			case RouletteBetPosition._7_8:
				return 17;
			case RouletteBetPosition._8_9:
				return 17;
			case RouletteBetPosition._10_11:
				return 17;
			case RouletteBetPosition._11_12:
				return 17;
			case RouletteBetPosition._13_14:
				return 17;
			case RouletteBetPosition._14_15:
				return 17;
			case RouletteBetPosition._16_17:
				return 17;
			case RouletteBetPosition._17_18:
				return 17;
			case RouletteBetPosition._19_20:
				return 17;
			case RouletteBetPosition._20_21:
				return 17;
			case RouletteBetPosition._22_23:
				return 17;
			case RouletteBetPosition._23_24:
				return 17;
			case RouletteBetPosition._25_26:
				return 17;
			case RouletteBetPosition._26_27:
				return 17;
			case RouletteBetPosition._28_29:
				return 17;
			case RouletteBetPosition._29_30:
				return 17;
			case RouletteBetPosition._31_32:
				return 17;
			case RouletteBetPosition._32_33:
				return 17;
			case RouletteBetPosition._34_35:
				return 17;
			case RouletteBetPosition._35_36:
				return 17;
			default:
				return result;
			}
		}

		public static string GetBetNotificationAtPosition(RouletteBetPosition Position)
		{
			string result = "";
			switch (Position)
			{
			case RouletteBetPosition.Num_00:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_0:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_1:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_2:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_3:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_4:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_5:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_6:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_7:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_8:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_9:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_10:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_11:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_12:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_13:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_14:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_15:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_16:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_17:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_18:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_19:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_20:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_21:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_22:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_23:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_24:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_25:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_26:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_27:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_28:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_29:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_30:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_31:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_32:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_33:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_34:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_35:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Num_36:
				return "Straight-up: A bet on an individual number. This pays 35 to 1.";
			case RouletteBetPosition.Col_1:
				return "Column: A twelve-number bet that covers all of the numbers in the corresponding column. This pays 2 to 1.";
			case RouletteBetPosition.Col_2:
				return "Column: A twelve-number bet that covers all of the numbers in the corresponding column. This pays 2 to 1.";
			case RouletteBetPosition.Col_3:
				return "Column: A twelve-number bet that covers all of the numbers in the corresponding column. This pays 2 to 1.";
			case RouletteBetPosition._19to36:
				return "Halves: An eighteen-number bet that covers the 1st half (1-18) or the 2nd half (19-36). This pays 1 to 1.";
			case RouletteBetPosition._Odd:
				return "Odd: An eighteen-number bet that covers all of the odd numbers. This pays 1 to 1.";
			case RouletteBetPosition._1to18:
				return "Halves: An eighteen-number bet that covers the 1st half (1-18) or the 2nd half (19-36). This pays 1 to 1.";
			case RouletteBetPosition._Even:
				return "Even: An eighteen-number bet that covers all of the even numbers. This pays 1 to 1.";
			case RouletteBetPosition._Red:
				return "Red: An eighteen-number bet that covers all of the red numbers. This pays 1 to 1.";
			case RouletteBetPosition._Black:
				return "Black: An eighteen-number bet that covers all of the black numbers. This pays 1 to 1.";
			case RouletteBetPosition._1st12:
				return "Dozen: A twelve-number bet that covers 1-12, 13-24, or 25-36. This pays 2 to 1.";
			case RouletteBetPosition._2nd12:
				return "Dozen: A twelve-number bet that covers 1-12, 13-24, or 25-36. This pays 2 to 1.";
			case RouletteBetPosition._3nd12:
				return "Dozen: A twelve-number bet that covers 1-12, 13-24, or 25-36. This pays 2 to 1.";
			case RouletteBetPosition._1_2_4_5:
				return "Corner: A four-number bet that's placed at the corner of four numbers. This pays 8 to 1.";
			case RouletteBetPosition._2_3_5_6:
				return "Corner: A four-number bet that's placed at the corner of four numbers. This pays 8 to 1.";
			case RouletteBetPosition._4_5_7_8:
				return "Corner: A four-number bet that's placed at the corner of four numbers. This pays 8 to 1.";
			case RouletteBetPosition._5_6_8_9:
				return "Corner: A four-number bet that's placed at the corner of four numbers. This pays 8 to 1.";
			case RouletteBetPosition._7_8_10_11:
				return "Corner: A four-number bet that's placed at the corner of four numbers. This pays 8 to 1.";
			case RouletteBetPosition._8_9_11_12:
				return "Corner: A four-number bet that's placed at the corner of four numbers. This pays 8 to 1.";
			case RouletteBetPosition._10_11_13_14:
				return "Corner: A four-number bet that's placed at the corner of four numbers. This pays 8 to 1.";
			case RouletteBetPosition._11_12_14_15:
				return "Corner: A four-number bet that's placed at the corner of four numbers. This pays 8 to 1.";
			case RouletteBetPosition._13_14_16_17:
				return "Corner: A four-number bet that's placed at the corner of four numbers. This pays 8 to 1.";
			case RouletteBetPosition._14_15_17_18:
				return "Corner: A four-number bet that's placed at the corner of four numbers. This pays 8 to 1.";
			case RouletteBetPosition._16_17_19_20:
				return "Corner: A four-number bet that's placed at the corner of four numbers. This pays 8 to 1.";
			case RouletteBetPosition._17_18_20_21:
				return "Corner: A four-number bet that's placed at the corner of four numbers. This pays 8 to 1.";
			case RouletteBetPosition._19_20_22_23:
				return "Corner: A four-number bet that's placed at the corner of four numbers. This pays 8 to 1.";
			case RouletteBetPosition._20_21_23_24:
				return "Corner: A four-number bet that's placed at the corner of four numbers. This pays 8 to 1.";
			case RouletteBetPosition._22_23_25_26:
				return "Corner: A four-number bet that's placed at the corner of four numbers. This pays 8 to 1.";
			case RouletteBetPosition._23_24_26_27:
				return "Corner: A four-number bet that's placed at the corner of four numbers. This pays 8 to 1.";
			case RouletteBetPosition._25_26_28_29:
				return "Corner: A four-number bet that's placed at the corner of four numbers. This pays 8 to 1.";
			case RouletteBetPosition._26_27_29_30:
				return "Corner: A four-number bet that's placed at the corner of four numbers. This pays 8 to 1.";
			case RouletteBetPosition._28_29_31_32:
				return "Corner: A four-number bet that's placed at the corner of four numbers. This pays 8 to 1.";
			case RouletteBetPosition._29_30_32_33:
				return "Corner: A four-number bet that's placed at the corner of four numbers. This pays 8 to 1.";
			case RouletteBetPosition._31_32_34_35:
				return "Corner: A four-number bet that's placed at the corner of four numbers. This pays 8 to 1.";
			case RouletteBetPosition._32_33_35_36:
				return "Corner: A four-number bet that's placed at the corner of four numbers. This pays 8 to 1.";
			case RouletteBetPosition.Row_1:
				return "Street: A three-number bet that's placed at the end of a row of numbers. This pays 11 to 1.";
			case RouletteBetPosition.Segment_1:
				return "Line: A six-number bet that's placed at the junction of two street bets. This pays 5 to 1.";
			case RouletteBetPosition.Row_2:
				return "Street: A three-number bet that's placed at the end of a row of numbers. This pays 11 to 1.";
			case RouletteBetPosition.Segment_2:
				return "Line: A six-number bet that's placed at the junction of two street bets. This pays 5 to 1.";
			case RouletteBetPosition.Row_3:
				return "Street: A three-number bet that's placed at the end of a row of numbers. This pays 11 to 1.";
			case RouletteBetPosition.Segment_3:
				return "Line: A six-number bet that's placed at the junction of two street bets. This pays 5 to 1.";
			case RouletteBetPosition.Row_4:
				return "Street: A three-number bet that's placed at the end of a row of numbers. This pays 11 to 1.";
			case RouletteBetPosition.Segment_4:
				return "Line: A six-number bet that's placed at the junction of two street bets. This pays 5 to 1.";
			case RouletteBetPosition.Row_5:
				return "Street: A three-number bet that's placed at the end of a row of numbers. This pays 11 to 1.";
			case RouletteBetPosition.Segment_5:
				return "Line: A six-number bet that's placed at the junction of two street bets. This pays 5 to 1.";
			case RouletteBetPosition.Row_6:
				return "Street: A three-number bet that's placed at the end of a row of numbers. This pays 11 to 1.";
			case RouletteBetPosition.Segment_6:
				return "Line: A six-number bet that's placed at the junction of two street bets. This pays 5 to 1.";
			case RouletteBetPosition.Row_7:
				return "Street: A three-number bet that's placed at the end of a row of numbers. This pays 11 to 1.";
			case RouletteBetPosition.Segment_7:
				return "Line: A six-number bet that's placed at the junction of two street bets. This pays 5 to 1.";
			case RouletteBetPosition.Row_8:
				return "Street: A three-number bet that's placed at the end of a row of numbers. This pays 11 to 1.";
			case RouletteBetPosition.Segment_8:
				return "Line: A six-number bet that's placed at the junction of two street bets. This pays 5 to 1.";
			case RouletteBetPosition.Row_9:
				return "Street: A three-number bet that's placed at the end of a row of numbers. This pays 11 to 1.";
			case RouletteBetPosition.Segment_9:
				return "Line: A six-number bet that's placed at the junction of two street bets. This pays 5 to 1.";
			case RouletteBetPosition.Row_10:
				return "Street: A three-number bet that's placed at the end of a row of numbers. This pays 11 to 1.";
			case RouletteBetPosition.Segment_10:
				return "Line: A six-number bet that's placed at the junction of two street bets. This pays 5 to 1.";
			case RouletteBetPosition.Row_11:
				return "Street: A three-number bet that's placed at the end of a row of numbers. This pays 11 to 1.";
			case RouletteBetPosition.Segment_11:
				return "Line: A six-number bet that's placed at the junction of two street bets. This pays 5 to 1.";
			case RouletteBetPosition.Row_12:
				return "Street: A three-number bet that's placed at the end of a row of numbers. This pays 11 to 1.";
			case RouletteBetPosition._1_2_3_00_0:
				return "Five-Number: A five-number bet covering 0, 00, 1, 2, and 3. This pays 6 to 1.";
			case RouletteBetPosition._1_2:
				return "Split: A two-number bet that's placed on the line connecting two numbers. This pays 17 to 1.";
			case RouletteBetPosition._2_3:
				return "Split: A two-number bet that's placed on the line connecting two numbers. This pays 17 to 1.";
			case RouletteBetPosition._4_5:
				return "Split: A two-number bet that's placed on the line connecting two numbers. This pays 17 to 1.";
			case RouletteBetPosition._5_6:
				return "Split: A two-number bet that's placed on the line connecting two numbers. This pays 17 to 1.";
			case RouletteBetPosition._7_8:
				return "Split: A two-number bet that's placed on the line connecting two numbers. This pays 17 to 1.";
			case RouletteBetPosition._8_9:
				return "Split: A two-number bet that's placed on the line connecting two numbers. This pays 17 to 1.";
			case RouletteBetPosition._10_11:
				return "Split: A two-number bet that's placed on the line connecting two numbers. This pays 17 to 1.";
			case RouletteBetPosition._11_12:
				return "Split: A two-number bet that's placed on the line connecting two numbers. This pays 17 to 1.";
			case RouletteBetPosition._13_14:
				return "Split: A two-number bet that's placed on the line connecting two numbers. This pays 17 to 1.";
			case RouletteBetPosition._14_15:
				return "Split: A two-number bet that's placed on the line connecting two numbers. This pays 17 to 1.";
			case RouletteBetPosition._16_17:
				return "Split: A two-number bet that's placed on the line connecting two numbers. This pays 17 to 1.";
			case RouletteBetPosition._17_18:
				return "Split: A two-number bet that's placed on the line connecting two numbers. This pays 17 to 1.";
			case RouletteBetPosition._19_20:
				return "Split: A two-number bet that's placed on the line connecting two numbers. This pays 17 to 1.";
			case RouletteBetPosition._20_21:
				return "Split: A two-number bet that's placed on the line connecting two numbers. This pays 17 to 1.";
			case RouletteBetPosition._22_23:
				return "Split: A two-number bet that's placed on the line connecting two numbers. This pays 17 to 1.";
			case RouletteBetPosition._23_24:
				return "Split: A two-number bet that's placed on the line connecting two numbers. This pays 17 to 1.";
			case RouletteBetPosition._25_26:
				return "Split: A two-number bet that's placed on the line connecting two numbers. This pays 17 to 1.";
			case RouletteBetPosition._26_27:
				return "Split: A two-number bet that's placed on the line connecting two numbers. This pays 17 to 1.";
			case RouletteBetPosition._28_29:
				return "Split: A two-number bet that's placed on the line connecting two numbers. This pays 17 to 1.";
			case RouletteBetPosition._29_30:
				return "Split: A two-number bet that's placed on the line connecting two numbers. This pays 17 to 1.";
			case RouletteBetPosition._31_32:
				return "Split: A two-number bet that's placed on the line connecting two numbers. This pays 17 to 1.";
			case RouletteBetPosition._32_33:
				return "Split: A two-number bet that's placed on the line connecting two numbers. This pays 17 to 1.";
			case RouletteBetPosition._34_35:
				return "Split: A two-number bet that's placed on the line connecting two numbers. This pays 17 to 1.";
			case RouletteBetPosition._35_36:
				return "Split: A two-number bet that's placed on the line connecting two numbers. This pays 17 to 1.";
			default:
				return result;
			}
		}

		public static string ConvertRouletteIDtoPositionName(int BetPosNum)
		{
			string result = "None";
			if (BetPosNum == 1)
			{
				result = "0";
			}
			if (BetPosNum == 2)
			{
				result = "00";
			}
			if (BetPosNum == 3)
			{
				result = "Red, 1";
			}
			if (BetPosNum == 4)
			{
				result = "Black, 2";
			}
			if (BetPosNum == 5)
			{
				result = "Red, 3";
			}
			if (BetPosNum == 6)
			{
				result = "Black, 4";
			}
			if (BetPosNum == 7)
			{
				result = "Red, 5";
			}
			if (BetPosNum == 8)
			{
				result = "Black, 6";
			}
			if (BetPosNum == 9)
			{
				result = "Red, 7";
			}
			if (BetPosNum == 10)
			{
				result = "Black, 8";
			}
			if (BetPosNum == 11)
			{
				result = "Red, 9";
			}
			if (BetPosNum == 12)
			{
				result = "Black, 10";
			}
			if (BetPosNum == 13)
			{
				result = "Black, 11";
			}
			if (BetPosNum == 14)
			{
				result = "Red, 12";
			}
			if (BetPosNum == 15)
			{
				result = "Black, 13";
			}
			if (BetPosNum == 16)
			{
				result = "Red, 14";
			}
			if (BetPosNum == 17)
			{
				result = "Black, 15";
			}
			if (BetPosNum == 18)
			{
				result = "Red, 16";
			}
			if (BetPosNum == 19)
			{
				result = "Black, 17";
			}
			if (BetPosNum == 20)
			{
				result = "Red, 18";
			}
			if (BetPosNum == 21)
			{
				result = "Red, 19";
			}
			if (BetPosNum == 22)
			{
				result = "Black, 20";
			}
			if (BetPosNum == 23)
			{
				result = "Red, 21";
			}
			if (BetPosNum == 24)
			{
				result = "Black, 22";
			}
			if (BetPosNum == 25)
			{
				result = "Red, 23";
			}
			if (BetPosNum == 26)
			{
				result = "Black, 24";
			}
			if (BetPosNum == 27)
			{
				result = "Red, 25";
			}
			if (BetPosNum == 28)
			{
				result = "Black, 26";
			}
			if (BetPosNum == 29)
			{
				result = "Red, 27";
			}
			if (BetPosNum == 30)
			{
				result = "Black, 28";
			}
			if (BetPosNum == 31)
			{
				result = "Black, 29";
			}
			if (BetPosNum == 32)
			{
				result = "Red, 30";
			}
			if (BetPosNum == 33)
			{
				result = "Black, 31";
			}
			if (BetPosNum == 34)
			{
				result = "Red, 32";
			}
			if (BetPosNum == 35)
			{
				result = "Black, 33";
			}
			if (BetPosNum == 36)
			{
				result = "Red, 34";
			}
			if (BetPosNum == 37)
			{
				result = "Black, 35";
			}
			if (BetPosNum == 38)
			{
				result = "Red, 36";
			}
			if (BetPosNum == 39)
			{
				result = "1st Column";
			}
			if (BetPosNum == 40)
			{
				result = "2nd Column";
			}
			if (BetPosNum == 41)
			{
				result = "3rd Column";
			}
			if (BetPosNum == 42)
			{
				result = "3rd 12";
			}
			if (BetPosNum == 43)
			{
				result = "2nd 12";
			}
			if (BetPosNum == 44)
			{
				result = "1st 12";
			}
			if (BetPosNum == 45)
			{
				result = "1 to 18";
			}
			if (BetPosNum == 46)
			{
				result = "Even";
			}
			if (BetPosNum == 47)
			{
				result = "Red";
			}
			if (BetPosNum == 48)
			{
				result = "Black";
			}
			if (BetPosNum == 49)
			{
				result = "Odd";
			}
			if (BetPosNum == 50)
			{
				result = "19 to 36";
			}
			if (BetPosNum == 51)
			{
				result = "Numbers 1, 2, 4, 5";
			}
			if (BetPosNum == 52)
			{
				result = "Numbers 2, 3, 5, 6";
			}
			if (BetPosNum == 53)
			{
				result = "Numbers 4, 5, 7, 8";
			}
			if (BetPosNum == 54)
			{
				result = "Numbers 5, 6, 8, 9";
			}
			if (BetPosNum == 55)
			{
				result = "Numbers 7, 8, 10, 11";
			}
			if (BetPosNum == 56)
			{
				result = "Numbers 8, 9, 11, 12";
			}
			if (BetPosNum == 57)
			{
				result = "Numbers 10, 11, 13, 14";
			}
			if (BetPosNum == 58)
			{
				result = "Numbers 11, 12, 14, 15";
			}
			if (BetPosNum == 59)
			{
				result = "Numbers 13, 14, 16, 17";
			}
			if (BetPosNum == 60)
			{
				result = "Numbers 14, 15, 17, 18";
			}
			if (BetPosNum == 61)
			{
				result = "Numbers 16, 17, 19, 20";
			}
			if (BetPosNum == 62)
			{
				result = "Numbers 17, 18, 20, 21";
			}
			if (BetPosNum == 63)
			{
				result = "Numbers 19, 20, 22, 23";
			}
			if (BetPosNum == 64)
			{
				result = "Numbers 20, 21, 23, 24";
			}
			if (BetPosNum == 65)
			{
				result = "Numbers 22, 23, 25, 26";
			}
			if (BetPosNum == 66)
			{
				result = "Numbers 23, 24, 26, 27";
			}
			if (BetPosNum == 67)
			{
				result = "Numbers 25, 26, 28, 29";
			}
			if (BetPosNum == 68)
			{
				result = "Numbers 26, 27, 29, 30";
			}
			if (BetPosNum == 69)
			{
				result = "Numbers 28, 29, 31, 32";
			}
			if (BetPosNum == 70)
			{
				result = "Numbers 29, 30, 31, 32";
			}
			if (BetPosNum == 71)
			{
				result = "Numbers 31, 32, 34, 35";
			}
			if (BetPosNum == 72)
			{
				result = "Numbers 32, 33, 35, 36";
			}
			if (BetPosNum == 73)
			{
				result = "Row#1 1, 2, 3";
			}
			if (BetPosNum == 74)
			{
				result = "Segment#1 1, 2, 3, 4, 5, 6";
			}
			if (BetPosNum == 75)
			{
				result = "Row#2 4, 5, 6";
			}
			if (BetPosNum == 76)
			{
				result = "Segment#2 4, 5, 6, 7, 8, 9";
			}
			if (BetPosNum == 77)
			{
				result = "Row#3 7, 8, 9";
			}
			if (BetPosNum == 78)
			{
				result = "Segment#3 7, 8, 9, 10, 11, 12";
			}
			if (BetPosNum == 79)
			{
				result = "Row#4 10, 11, 12";
			}
			if (BetPosNum == 80)
			{
				result = "Segment#4 10, 11, 12, 13, 14, 15";
			}
			if (BetPosNum == 81)
			{
				result = "Row#5 13, 14, 15";
			}
			if (BetPosNum == 82)
			{
				result = "Segment#5 13, 14, 15, 16, 17, 18";
			}
			if (BetPosNum == 83)
			{
				result = "Row#6 16, 17, 18";
			}
			if (BetPosNum == 84)
			{
				result = "Segment#6 16, 17, 18, 19, 20, 21";
			}
			if (BetPosNum == 85)
			{
				result = "Row#7 19, 20, 21";
			}
			if (BetPosNum == 86)
			{
				result = "Segment#7 19, 20, 21, 22, 23, 24";
			}
			if (BetPosNum == 87)
			{
				result = "Row#8 22, 23, 24";
			}
			if (BetPosNum == 88)
			{
				result = "Segment#8 22, 23, 24, 25, 26, 27";
			}
			if (BetPosNum == 89)
			{
				result = "Row#9 25, 26, 27";
			}
			if (BetPosNum == 90)
			{
				result = "Segment#9 25, 26, 27, 28, 29, 30";
			}
			if (BetPosNum == 91)
			{
				result = "Row#10 28, 29, 30";
			}
			if (BetPosNum == 92)
			{
				result = "Segment#10 28, 29, 30, 31, 32, 33";
			}
			if (BetPosNum == 93)
			{
				result = "Row#11 31, 32, 33";
			}
			if (BetPosNum == 94)
			{
				result = "Segment#11 31, 32, 33, 34, 35, 36";
			}
			if (BetPosNum == 95)
			{
				result = "Row#12 34, 35, 36";
			}
			if (BetPosNum == 96)
			{
				result = "Numbers 1, 2, 3, 0, 00";
			}
			if (BetPosNum == 97)
			{
				result = "Numbers 1 & 2";
			}
			if (BetPosNum == 98)
			{
				result = "Numbers 2 & 3";
			}
			if (BetPosNum == 99)
			{
				result = "Numbers 4 & 5";
			}
			if (BetPosNum == 100)
			{
				result = "Numbers 5 & 6";
			}
			if (BetPosNum == 101)
			{
				result = "Numbers 7 & 8";
			}
			if (BetPosNum == 102)
			{
				result = "Numbers 8 & 9";
			}
			if (BetPosNum == 103)
			{
				result = "Numbers 10 & 11";
			}
			if (BetPosNum == 104)
			{
				result = "Numbers 11 & 12";
			}
			if (BetPosNum == 105)
			{
				result = "Numbers 13 & 14";
			}
			if (BetPosNum == 106)
			{
				result = "Numbers 14 & 15";
			}
			if (BetPosNum == 107)
			{
				result = "Numbers 16 & 17";
			}
			if (BetPosNum == 108)
			{
				result = "Numbers 17 & 18";
			}
			if (BetPosNum == 109)
			{
				result = "Numbers 19 & 20";
			}
			if (BetPosNum == 110)
			{
				result = "Numbers 20 & 21";
			}
			if (BetPosNum == 111)
			{
				result = "Numbers 22 & 23";
			}
			if (BetPosNum == 112)
			{
				result = "Numbers 23 & 24";
			}
			if (BetPosNum == 113)
			{
				result = "Numbers 25 & 26";
			}
			if (BetPosNum == 114)
			{
				result = "Numbers 26 & 27";
			}
			if (BetPosNum == 115)
			{
				result = "Numbers 28 & 29";
			}
			if (BetPosNum == 116)
			{
				result = "Numbers 29 & 30";
			}
			if (BetPosNum == 117)
			{
				result = "Numbers 31 & 32";
			}
			if (BetPosNum == 118)
			{
				result = "Numbers 32 & 33";
			}
			if (BetPosNum == 119)
			{
				result = "Numbers 34 & 35";
			}
			if (BetPosNum == 120)
			{
				result = "Numbers 35 & 36";
			}
			return result;
		}

		public static RouletteBetPosition GetRouletteBetPosition(int BetPosNum, Prop Table)
		{
			RouletteBetPosition result = RouletteBetPosition.None;
			if (BetPosNum == 1)
			{
				result = RouletteBetPosition.Num_0;
			}
			if (BetPosNum == 2)
			{
				result = RouletteBetPosition.Num_00;
			}
			if (BetPosNum == 3)
			{
				result = RouletteBetPosition.Num_1;
			}
			if (BetPosNum == 4)
			{
				result = RouletteBetPosition.Num_2;
			}
			if (BetPosNum == 5)
			{
				result = RouletteBetPosition.Num_3;
			}
			if (BetPosNum == 6)
			{
				result = RouletteBetPosition.Num_4;
			}
			if (BetPosNum == 7)
			{
				result = RouletteBetPosition.Num_5;
			}
			if (BetPosNum == 8)
			{
				result = RouletteBetPosition.Num_6;
			}
			if (BetPosNum == 9)
			{
				result = RouletteBetPosition.Num_7;
			}
			if (BetPosNum == 10)
			{
				result = RouletteBetPosition.Num_8;
			}
			if (BetPosNum == 11)
			{
				result = RouletteBetPosition.Num_9;
			}
			if (BetPosNum == 12)
			{
				result = RouletteBetPosition.Num_10;
			}
			if (BetPosNum == 13)
			{
				result = RouletteBetPosition.Num_11;
			}
			if (BetPosNum == 14)
			{
				result = RouletteBetPosition.Num_12;
			}
			if (BetPosNum == 15)
			{
				result = RouletteBetPosition.Num_13;
			}
			if (BetPosNum == 16)
			{
				result = RouletteBetPosition.Num_14;
			}
			if (BetPosNum == 17)
			{
				result = RouletteBetPosition.Num_15;
			}
			if (BetPosNum == 18)
			{
				result = RouletteBetPosition.Num_16;
			}
			if (BetPosNum == 19)
			{
				result = RouletteBetPosition.Num_17;
			}
			if (BetPosNum == 20)
			{
				result = RouletteBetPosition.Num_18;
			}
			if (BetPosNum == 21)
			{
				result = RouletteBetPosition.Num_19;
			}
			if (BetPosNum == 22)
			{
				result = RouletteBetPosition.Num_20;
			}
			if (BetPosNum == 23)
			{
				result = RouletteBetPosition.Num_21;
			}
			if (BetPosNum == 24)
			{
				result = RouletteBetPosition.Num_22;
			}
			if (BetPosNum == 25)
			{
				result = RouletteBetPosition.Num_23;
			}
			if (BetPosNum == 26)
			{
				result = RouletteBetPosition.Num_24;
			}
			if (BetPosNum == 27)
			{
				result = RouletteBetPosition.Num_25;
			}
			if (BetPosNum == 28)
			{
				result = RouletteBetPosition.Num_26;
			}
			if (BetPosNum == 29)
			{
				result = RouletteBetPosition.Num_27;
			}
			if (BetPosNum == 30)
			{
				result = RouletteBetPosition.Num_28;
			}
			if (BetPosNum == 31)
			{
				result = RouletteBetPosition.Num_29;
			}
			if (BetPosNum == 32)
			{
				result = RouletteBetPosition.Num_30;
			}
			if (BetPosNum == 33)
			{
				result = RouletteBetPosition.Num_31;
			}
			if (BetPosNum == 34)
			{
				result = RouletteBetPosition.Num_32;
			}
			if (BetPosNum == 35)
			{
				result = RouletteBetPosition.Num_33;
			}
			if (BetPosNum == 36)
			{
				result = RouletteBetPosition.Num_34;
			}
			if (BetPosNum == 37)
			{
				result = RouletteBetPosition.Num_35;
			}
			if (BetPosNum == 38)
			{
				result = RouletteBetPosition.Num_36;
			}
			if (BetPosNum == 39)
			{
				result = RouletteBetPosition.Col_1;
			}
			if (BetPosNum == 40)
			{
				result = RouletteBetPosition.Col_2;
			}
			if (BetPosNum == 41)
			{
				result = RouletteBetPosition.Col_3;
			}
			if (BetPosNum == 42)
			{
				result = RouletteBetPosition._3nd12;
			}
			if (BetPosNum == 43)
			{
				result = RouletteBetPosition._2nd12;
			}
			if (BetPosNum == 44)
			{
				result = RouletteBetPosition._1st12;
			}
			if (BetPosNum == 45)
			{
				result = RouletteBetPosition._1to18;
			}
			if (BetPosNum == 46)
			{
				result = RouletteBetPosition._Even;
			}
			if (BetPosNum == 47)
			{
				result = RouletteBetPosition._Red;
			}
			if (BetPosNum == 48)
			{
				result = RouletteBetPosition._Black;
			}
			if (BetPosNum == 49)
			{
				result = RouletteBetPosition._Odd;
			}
			if (BetPosNum == 50)
			{
				result = RouletteBetPosition._19to36;
			}
			if (BetPosNum == 51)
			{
				result = RouletteBetPosition._1_2_4_5;
			}
			if (BetPosNum == 52)
			{
				result = RouletteBetPosition._2_3_5_6;
			}
			if (BetPosNum == 53)
			{
				result = RouletteBetPosition._4_5_7_8;
			}
			if (BetPosNum == 54)
			{
				result = RouletteBetPosition._5_6_8_9;
			}
			if (BetPosNum == 55)
			{
				result = RouletteBetPosition._7_8_10_11;
			}
			if (BetPosNum == 56)
			{
				result = RouletteBetPosition._8_9_11_12;
			}
			if (BetPosNum == 57)
			{
				result = RouletteBetPosition._10_11_13_14;
			}
			if (BetPosNum == 58)
			{
				result = RouletteBetPosition._11_12_14_15;
			}
			if (BetPosNum == 59)
			{
				result = RouletteBetPosition._13_14_16_17;
			}
			if (BetPosNum == 60)
			{
				result = RouletteBetPosition._14_15_17_18;
			}
			if (BetPosNum == 61)
			{
				result = RouletteBetPosition._16_17_19_20;
			}
			if (BetPosNum == 62)
			{
				result = RouletteBetPosition._17_18_20_21;
			}
			if (BetPosNum == 63)
			{
				result = RouletteBetPosition._19_20_22_23;
			}
			if (BetPosNum == 64)
			{
				result = RouletteBetPosition._20_21_23_24;
			}
			if (BetPosNum == 65)
			{
				result = RouletteBetPosition._22_23_25_26;
			}
			if (BetPosNum == 66)
			{
				result = RouletteBetPosition._23_24_26_27;
			}
			if (BetPosNum == 67)
			{
				result = RouletteBetPosition._25_26_28_29;
			}
			if (BetPosNum == 68)
			{
				result = RouletteBetPosition._26_27_29_30;
			}
			if (BetPosNum == 69)
			{
				result = RouletteBetPosition._28_29_31_32;
			}
			if (BetPosNum == 70)
			{
				result = RouletteBetPosition._29_30_32_33;
			}
			if (BetPosNum == 71)
			{
				result = RouletteBetPosition._31_32_34_35;
			}
			if (BetPosNum == 72)
			{
				result = RouletteBetPosition._32_33_35_36;
			}
			if (BetPosNum == 73)
			{
				result = RouletteBetPosition.Row_1;
			}
			if (BetPosNum == 74)
			{
				result = RouletteBetPosition.Segment_1;
			}
			if (BetPosNum == 75)
			{
				result = RouletteBetPosition.Row_2;
			}
			if (BetPosNum == 76)
			{
				result = RouletteBetPosition.Segment_2;
			}
			if (BetPosNum == 77)
			{
				result = RouletteBetPosition.Row_3;
			}
			if (BetPosNum == 78)
			{
				result = RouletteBetPosition.Segment_3;
			}
			if (BetPosNum == 79)
			{
				result = RouletteBetPosition.Row_4;
			}
			if (BetPosNum == 80)
			{
				result = RouletteBetPosition.Segment_4;
			}
			if (BetPosNum == 81)
			{
				result = RouletteBetPosition.Row_5;
			}
			if (BetPosNum == 82)
			{
				result = RouletteBetPosition.Segment_5;
			}
			if (BetPosNum == 83)
			{
				result = RouletteBetPosition.Row_6;
			}
			if (BetPosNum == 84)
			{
				result = RouletteBetPosition.Segment_6;
			}
			if (BetPosNum == 85)
			{
				result = RouletteBetPosition.Row_7;
			}
			if (BetPosNum == 86)
			{
				result = RouletteBetPosition.Segment_7;
			}
			if (BetPosNum == 87)
			{
				result = RouletteBetPosition.Row_8;
			}
			if (BetPosNum == 88)
			{
				result = RouletteBetPosition.Segment_8;
			}
			if (BetPosNum == 89)
			{
				result = RouletteBetPosition.Row_9;
			}
			if (BetPosNum == 90)
			{
				result = RouletteBetPosition.Segment_9;
			}
			if (BetPosNum == 91)
			{
				result = RouletteBetPosition.Row_10;
			}
			if (BetPosNum == 92)
			{
				result = RouletteBetPosition.Segment_10;
			}
			if (BetPosNum == 93)
			{
				result = RouletteBetPosition.Row_11;
			}
			if (BetPosNum == 94)
			{
				result = RouletteBetPosition.Segment_11;
			}
			if (BetPosNum == 95)
			{
				result = RouletteBetPosition.Row_12;
			}
			if (BetPosNum == 96)
			{
				result = RouletteBetPosition._1_2_3_00_0;
			}
			if (BetPosNum == 97)
			{
				result = RouletteBetPosition._1_2;
			}
			if (BetPosNum == 98)
			{
				result = RouletteBetPosition._2_3;
			}
			if (BetPosNum == 99)
			{
				result = RouletteBetPosition._4_5;
			}
			if (BetPosNum == 100)
			{
				result = RouletteBetPosition._5_6;
			}
			if (BetPosNum == 101)
			{
				result = RouletteBetPosition._7_8;
			}
			if (BetPosNum == 102)
			{
				result = RouletteBetPosition._8_9;
			}
			if (BetPosNum == 103)
			{
				result = RouletteBetPosition._10_11;
			}
			if (BetPosNum == 104)
			{
				result = RouletteBetPosition._11_12;
			}
			if (BetPosNum == 105)
			{
				result = RouletteBetPosition._13_14;
			}
			if (BetPosNum == 106)
			{
				result = RouletteBetPosition._14_15;
			}
			if (BetPosNum == 107)
			{
				result = RouletteBetPosition._16_17;
			}
			if (BetPosNum == 108)
			{
				result = RouletteBetPosition._17_18;
			}
			if (BetPosNum == 109)
			{
				result = RouletteBetPosition._19_20;
			}
			if (BetPosNum == 110)
			{
				result = RouletteBetPosition._20_21;
			}
			if (BetPosNum == 111)
			{
				result = RouletteBetPosition._22_23;
			}
			if (BetPosNum == 112)
			{
				result = RouletteBetPosition._23_24;
			}
			if (BetPosNum == 113)
			{
				result = RouletteBetPosition._25_26;
			}
			if (BetPosNum == 114)
			{
				result = RouletteBetPosition._26_27;
			}
			if (BetPosNum == 115)
			{
				result = RouletteBetPosition._28_29;
			}
			if (BetPosNum == 116)
			{
				result = RouletteBetPosition._29_30;
			}
			if (BetPosNum == 117)
			{
				result = RouletteBetPosition._31_32;
			}
			if (BetPosNum == 118)
			{
				result = RouletteBetPosition._32_33;
			}
			if (BetPosNum == 119)
			{
				result = RouletteBetPosition._34_35;
			}
			if (BetPosNum == 120)
			{
				result = RouletteBetPosition._35_36;
			}
			return result;
		}

		public static Vector3 GetOffsetPosition(Vector3 Coord, float Heading, Vector3 Offset)
		{
			return Function.Call<Vector3>(Hash.GET_OFFSET_FROM_COORD_AND_HEADING_IN_WORLD_COORDS, Coord.X, Coord.Y, Coord.Z, Heading, Offset.X, Offset.Y, Offset.Z);
		}

		public static void DrawPoly(Vector3 a, Vector3 b, Vector3 c, Color col)
		{
			Function.Call(Hash.DRAW_POLY, a.X, a.Y, a.Z, b.X, b.Y, b.Z, c.X, c.Y, c.Z, col.R, col.G, col.B, col.A);
		}

		public static void DrawRouletteBoxes(RouletteBetPosition Pos, Prop Table)
		{
			if (Pos == RouletteBetPosition.Num_1)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_1), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_2)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_2), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_3)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_3), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_4)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_4), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_5)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_5), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_6)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_6), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_7)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_7), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_8)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_8), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_9)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_9), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_10)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_10), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_11)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_11), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_12)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_12), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_13)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_13), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_14)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_14), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_15)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_15), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_16)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_16), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_17)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_17), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_18)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_18), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_19)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_19), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_20)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_20), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_21)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_21), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_22)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_22), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_23)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_23), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_24)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_24), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_25)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_25), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_26)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_26), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_27)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_27), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_28)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_28), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_29)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_29), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_30)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_30), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_31)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_31), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_32)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_32), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_33)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_33), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_34)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_34), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_35)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_35), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Num_36)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_36), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Col_1)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_1), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_4), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_7), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_10), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_13), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_16), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_19), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_22), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_25), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_28), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_31), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_34), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Col_2)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_2), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_5), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_8), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_11), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_14), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_17), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_20), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_23), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_26), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_29), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_32), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_35), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Col_3)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_3), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_6), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_9), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_12), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_15), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_18), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_21), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_24), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_27), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_30), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_33), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_36), Table.Heading);
			}
			if (Pos == RouletteBetPosition._1to18)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_1), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_2), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_3), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_4), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_5), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_6), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_7), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_8), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_9), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_10), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_11), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_12), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_13), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_14), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_15), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_16), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_17), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_18), Table.Heading);
			}
			if (Pos == RouletteBetPosition._19to36)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_19), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_20), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_21), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_22), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_23), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_24), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_25), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_26), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_27), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_28), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_29), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_30), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_31), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_32), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_33), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_34), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_35), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_36), Table.Heading);
			}
			if (Pos == RouletteBetPosition._1st12)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_1), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_2), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_3), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_4), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_5), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_6), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_7), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_8), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_9), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_10), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_11), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_12), Table.Heading);
			}
			if (Pos == RouletteBetPosition._2nd12)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_13), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_14), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_15), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_16), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_17), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_18), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_19), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_20), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_21), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_22), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_23), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_24), Table.Heading);
			}
			if (Pos == RouletteBetPosition._3nd12)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_25), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_26), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_27), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_28), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_29), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_30), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_31), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_32), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_33), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_34), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_35), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_36), Table.Heading);
			}
			if (Pos == RouletteBetPosition._Black)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_2), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_4), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_6), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_8), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_10), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_11), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_13), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_15), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_17), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_20), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_22), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_24), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_26), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_28), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_29), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_31), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_33), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_35), Table.Heading);
			}
			if (Pos == RouletteBetPosition._Red)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_1), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_3), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_5), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_7), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_9), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_12), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_14), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_16), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_18), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_19), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_21), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_23), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_25), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_27), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_30), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_32), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_34), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_36), Table.Heading);
			}
			if (Pos == RouletteBetPosition._Odd)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_1), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_3), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_5), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_7), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_9), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_11), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_13), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_15), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_17), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_19), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_21), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_23), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_25), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_27), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_29), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_31), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_33), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_35), Table.Heading);
			}
			if (Pos == RouletteBetPosition._Even)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_2), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_4), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_6), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_8), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_10), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_12), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_14), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_16), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_18), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_20), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_22), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_24), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_26), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_28), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_30), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_32), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_34), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_36), Table.Heading);
			}
			if (Pos == RouletteBetPosition._1_2_4_5)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_1), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_2), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_4), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_5), Table.Heading);
			}
			if (Pos == RouletteBetPosition._2_3_5_6)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_2), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_3), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_5), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_6), Table.Heading);
			}
			if (Pos == RouletteBetPosition._4_5_7_8)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_4), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_5), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_7), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_8), Table.Heading);
			}
			if (Pos == RouletteBetPosition._5_6_8_9)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_5), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_6), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_8), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_9), Table.Heading);
			}
			if (Pos == RouletteBetPosition._7_8_10_11)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_7), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_8), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_10), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_11), Table.Heading);
			}
			if (Pos == RouletteBetPosition._8_9_11_12)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_8), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_9), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_11), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_12), Table.Heading);
			}
			if (Pos == RouletteBetPosition._10_11_13_14)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_10), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_11), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_13), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_14), Table.Heading);
			}
			if (Pos == RouletteBetPosition._11_12_14_15)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_11), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_12), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_14), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_15), Table.Heading);
			}
			if (Pos == RouletteBetPosition._13_14_16_17)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_13), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_14), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_16), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_17), Table.Heading);
			}
			if (Pos == RouletteBetPosition._14_15_17_18)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_14), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_15), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_17), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_18), Table.Heading);
			}
			if (Pos == RouletteBetPosition._16_17_19_20)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_16), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_17), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_19), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_20), Table.Heading);
			}
			if (Pos == RouletteBetPosition._17_18_20_21)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_17), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_18), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_20), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_21), Table.Heading);
			}
			if (Pos == RouletteBetPosition._19_20_22_23)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_19), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_20), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_22), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_23), Table.Heading);
			}
			if (Pos == RouletteBetPosition._20_21_23_24)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_20), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_21), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_23), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_24), Table.Heading);
			}
			if (Pos == RouletteBetPosition._22_23_25_26)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_22), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_23), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_25), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_26), Table.Heading);
			}
			if (Pos == RouletteBetPosition._23_24_26_27)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_23), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_24), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_26), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_27), Table.Heading);
			}
			if (Pos == RouletteBetPosition._25_26_28_29)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_25), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_26), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_28), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_29), Table.Heading);
			}
			if (Pos == RouletteBetPosition._26_27_29_30)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_26), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_27), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_29), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_30), Table.Heading);
			}
			if (Pos == RouletteBetPosition._28_29_31_32)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_28), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_29), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_31), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_32), Table.Heading);
			}
			if (Pos == RouletteBetPosition._29_30_32_33)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_29), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_30), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_32), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_33), Table.Heading);
			}
			if (Pos == RouletteBetPosition._31_32_34_35)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_31), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_32), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_34), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_35), Table.Heading);
			}
			if (Pos == RouletteBetPosition._32_33_35_36)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_32), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_33), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_35), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_36), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Row_1)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_1), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_2), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_3), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Segment_1)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_1), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_2), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_3), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_4), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_5), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_6), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Row_2)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_4), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_5), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_6), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Segment_2)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_4), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_5), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_6), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_7), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_8), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_9), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Row_3)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_7), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_8), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_9), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Segment_3)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_7), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_8), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_9), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_10), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_11), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_12), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Row_4)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_10), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_11), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_12), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Segment_4)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_10), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_11), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_12), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_13), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_14), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_15), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Row_5)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_13), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_14), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_15), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Segment_5)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_13), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_14), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_15), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_16), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_17), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_18), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Row_6)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_16), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_17), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_18), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Segment_6)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_16), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_17), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_18), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_19), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_20), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_21), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Row_7)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_19), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_20), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_21), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Segment_7)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_19), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_20), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_21), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_22), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_23), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_24), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Row_8)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_22), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_23), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_24), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Segment_8)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_22), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_23), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_24), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_25), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_26), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_27), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Row_9)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_25), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_26), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_27), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Segment_9)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_25), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_26), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_27), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_28), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_29), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_30), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Row_10)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_28), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_29), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_30), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Segment_10)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_28), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_29), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_30), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_31), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_32), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_33), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Row_11)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_31), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_32), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_33), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Segment_11)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_31), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_32), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_33), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_34), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_35), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_36), Table.Heading);
			}
			if (Pos == RouletteBetPosition.Row_12)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_34), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_35), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_36), Table.Heading);
			}
			if (Pos == RouletteBetPosition._1_2_3_00_0)
			{
				Vector3 offsetPosition = Table.GetOffsetPosition(new Vector3(-0.1365f, -0.1516f, 0.935f));
				float heading = Table.Heading;
				Prop marker = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.75f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_marker_02a"), 0, 0, 0);
				if (!DoesObjectExist(marker))
				{
					marker = GlobalHelper.CreateProp(RequestModel("vw_prop_vw_marker_01a"), offsetPosition, new Vector3(0f, 0f, heading), false, false);
					BetPosMarkers.Add(marker);
				}
				offsetPosition = Table.GetOffsetPosition(new Vector3(-0.14f, 0.103f, 0.935f));
				heading = Table.Heading;
				marker = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.75f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_marker_02a"), 0, 0, 0);
				if (!DoesObjectExist(marker))
				{
					marker = GlobalHelper.CreateProp(RequestModel("vw_prop_vw_marker_01a"), offsetPosition, new Vector3(0f, 0f, heading), false, false);
					BetPosMarkers.Add(marker);
				}
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_1), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_2), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_3), Table.Heading);
			}
			if (Pos == RouletteBetPosition._1_2)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_1), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_2), Table.Heading);
			}
			if (Pos == RouletteBetPosition._2_3)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_2), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_3), Table.Heading);
			}
			if (Pos == RouletteBetPosition._4_5)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_4), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_5), Table.Heading);
			}
			if (Pos == RouletteBetPosition._5_6)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_5), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_6), Table.Heading);
			}
			if (Pos == RouletteBetPosition._7_8)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_7), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_8), Table.Heading);
			}
			if (Pos == RouletteBetPosition._8_9)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_8), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_9), Table.Heading);
			}
			if (Pos == RouletteBetPosition._10_11)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_10), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_11), Table.Heading);
			}
			if (Pos == RouletteBetPosition._11_12)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_11), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_12), Table.Heading);
			}
			if (Pos == RouletteBetPosition._13_14)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_13), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_14), Table.Heading);
			}
			if (Pos == RouletteBetPosition._14_15)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_14), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_15), Table.Heading);
			}
			if (Pos == RouletteBetPosition._16_17)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_16), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_17), Table.Heading);
			}
			if (Pos == RouletteBetPosition._17_18)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_17), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_18), Table.Heading);
			}
			if (Pos == RouletteBetPosition._19_20)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_19), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_20), Table.Heading);
			}
			if (Pos == RouletteBetPosition._20_21)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_20), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_21), Table.Heading);
			}
			if (Pos == RouletteBetPosition._22_23)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_22), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_23), Table.Heading);
			}
			if (Pos == RouletteBetPosition._23_24)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_23), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_24), Table.Heading);
			}
			if (Pos == RouletteBetPosition._25_26)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_25), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_26), Table.Heading);
			}
			if (Pos == RouletteBetPosition._26_27)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_26), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_27), Table.Heading);
			}
			if (Pos == RouletteBetPosition._28_29)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_28), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_29), Table.Heading);
			}
			if (Pos == RouletteBetPosition._29_30)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_29), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_30), Table.Heading);
			}
			if (Pos == RouletteBetPosition._31_32)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_31), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_32), Table.Heading);
			}
			if (Pos == RouletteBetPosition._32_33)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_32), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_33), Table.Heading);
			}
			if (Pos == RouletteBetPosition._34_35)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_34), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_35), Table.Heading);
			}
			if (Pos == RouletteBetPosition._35_36)
			{
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_35), Table.Heading);
				DrawRouletteBox(GetRouletteBetPosition(Table, RouletteBetPosition.Num_36), Table.Heading);
			}
		}

		public static void DrawRouletteBox(Vector3 Pos, float Rotation)
		{
			Color.FromArgb(150, 252, 186, 3);
			GetOffsetPosition(Pos, Rotation, new Vector3(0f - RouletteBox_Height, RouletteBox_Width, 0.0125f));
			Vector3 offsetPosition = GetOffsetPosition(Pos, Rotation, new Vector3(0f - RouletteBox_Height, 0f - RouletteBox_Width, 0.0125f));
			GetOffsetPosition(Pos, Rotation, new Vector3(RouletteBox_Height, RouletteBox_Width, 0.0125f));
			GetOffsetPosition(Pos, Rotation, new Vector3(RouletteBox_Height, 0f - RouletteBox_Width, 0.0125f));
			GetOffsetPosition(Pos, Rotation, new Vector3(0f - RouletteBox_Height, 0f - RouletteBox_Width, 0.0125f));
			offsetPosition = GetOffsetPosition(Pos, Rotation, new Vector3(0f - RouletteBox_Height, RouletteBox_Width, 0.0125f));
			GetOffsetPosition(Pos, Rotation, new Vector3(RouletteBox_Height, 0f - RouletteBox_Width, 0.0125f));
			GetOffsetPosition(Pos, Rotation, new Vector3(RouletteBox_Height, RouletteBox_Width, 0.0125f));
			Prop marker = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, Pos.X, Pos.Y, Pos.Z, 0.025f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_marker_02a"), 0, 0, 0);
			if (!DoesObjectExist(marker))
			{
				marker = GlobalHelper.CreateProp(RequestModel("vw_prop_vw_marker_02a"), Pos, new Vector3(0f, 0f, Rotation), false, false);
				BetPosMarkers.Add(marker);
			}
		}

		public static string GetHostName()
		{
			return "Dealer";
		}

		public static float GetDistance(Vector3 _1, Vector3 _2)
		{
			return (float)Math.Sqrt(Function.Call<float>(Hash.VDIST2, _1.X, _1.Y, _1.Z, _2.X, _2.Y, _2.Z));
		}

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

		public static void DisplayHelpTextThisFrameNoSound(string helpText)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 0, 1);
		}

		public static void ShowHelpMessage(string helptext, int showtime = 5000)
		{
			CurrentHelpMessage = helptext;
			HelpMessageShowTime = Game.GameTime + showtime;
		}

		public static void DisplayHelpTextThisFrame(string helpText)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, 1);
		}

		public static bool GetDistance_VDIST2(Vector3 _1, Vector3 _2, GetDistSign Sign, float Radius)
		{
			bool result = false;
			float num = Radius * Radius;
			float num2 = _1.DistanceToSquared(_2);
			if (Sign == GetDistSign.LessEqual && num2 <= num)
			{
				result = true;
			}
			if (Sign == GetDistSign.Less && num2 < num)
			{
				result = true;
			}
			if (Sign == GetDistSign.GreaterEqual && num2 >= num)
			{
				result = true;
			}
			if (Sign == GetDistSign.Greater && num2 > num)
			{
				result = true;
			}
			return result;
		}

		public static Model RequestModel(int Prop)
		{
			Model result = new Model(Prop);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(1);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public static Model RequestModel(string Prop)
		{
			Model result = new Model(Prop);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(1);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public static Model RequestModel(VehicleHash Prop)
		{
			Model result = new Model(Prop);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(1);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public static Model RequestModel(PedHash Prop)
		{
			Model result = new Model(Prop);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(1);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
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

		public static void drawText(string text, Vector2 Xy, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, Xy.X, Xy.Y, 0.1);
		}

		public static void drawText(string text, float X, float Y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, X, Y, 0.1);
		}

		public static void RunBet(float P)
		{
			_8PlayerPokerPot.SpawnChips(_8PlayerGamePokerPot);
		}

		public static void ReRunBet(float P)
		{
			_8PlayerPokerPot.SpawnChips(_8PlayerGamePokerPot);
		}

		public static void CheckPokerWin()
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
				Notification.Show("AI 1, Won with : Cards, " + GetCardName(PlayerCards[0].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[0].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[0].Card3ID));
				_8PlayerGameStage = 1;
			}
			if (num == AI2Total)
			{
				_8PlayerGamePoker_AI2Bank += _8PlayerGamePokerPot;
				Notification.Show("AI 2, Won with : Cards, " + GetCardName(PlayerCards[1].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[1].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[1].Card3ID));
				_8PlayerGameStage = 1;
			}
			if (num == AI3Total)
			{
				_8PlayerGamePoker_AI3Bank += _8PlayerGamePokerPot;
				Notification.Show("AI 3, Won with : Cards, " + GetCardName(PlayerCards[2].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[2].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[2].Card3ID));
				_8PlayerGameStage = 1;
			}
			if (num == AI4Total)
			{
				_8PlayerGamePoker_AI4Bank += _8PlayerGamePokerPot;
				Notification.Show("AI 4, Won with : Cards, " + GetCardName(PlayerCards[3].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[3].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[3].Card3ID));
				_8PlayerGameStage = 1;
			}
			if (num == AI5Total)
			{
				_8PlayerGamePoker_AI5Bank += _8PlayerGamePokerPot;
				Notification.Show("AI 5, Won with : Cards, " + GetCardName(PlayerCards[4].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[4].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[4].Card3ID));
				_8PlayerGameStage = 1;
			}
			if (num == AI6Total)
			{
				_8PlayerGamePoker_AI6Bank += _8PlayerGamePokerPot;
				Notification.Show("AI 6, Won with : Cards, " + GetCardName(PlayerCards[5].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[5].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[5].Card3ID));
				_8PlayerGameStage = 1;
			}
			if (num == AI7Total)
			{
				_8PlayerGamePoker_AI7Bank += _8PlayerGamePokerPot;
				Notification.Show("AI 7, Won with : Cards, " + GetCardName(PlayerCards[6].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[6].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[6].Card3ID));
				_8PlayerGameStage = 1;
			}
			if (num == PlayerTotal)
			{
				_8PlayerGamePoker_PlayerBank += _8PlayerGamePokerPot;
				Notification.Show("Player, Won with : Cards, " + GetCardName(PlayerCards[7].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[7].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[7].Card3ID));
				_8PlayerGameStage = 1;
			}
			Notification.Show("Commuity Cards Were, " + CommunityCard1ID + "~y~,~w~" + CommunityCard2ID + "~y~,~w~" + CommunityCard3ID);
		}

		public static void CheckPokerBuyOut()
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
					Notification.Show("AI 1, Won with : Cards, " + GetCardName(PlayerCards[0].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[0].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[0].Card3ID));
					_8PlayerGameStage = 1;
				}
				if (num == AI2Total)
				{
					_8PlayerGamePoker_AI2Bank += _8PlayerGamePokerPot;
					Notification.Show("AI 2, Won with : Cards, " + GetCardName(PlayerCards[1].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[1].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[1].Card3ID));
					_8PlayerGameStage = 1;
				}
				if (num == AI3Total)
				{
					_8PlayerGamePoker_AI3Bank += _8PlayerGamePokerPot;
					Notification.Show("AI 3, Won with : Cards, " + GetCardName(PlayerCards[2].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[2].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[2].Card3ID));
					_8PlayerGameStage = 1;
				}
				if (num == AI4Total)
				{
					_8PlayerGamePoker_AI4Bank += _8PlayerGamePokerPot;
					Notification.Show("AI 4, Won with : Cards, " + GetCardName(PlayerCards[3].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[3].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[3].Card3ID));
					_8PlayerGameStage = 1;
				}
				if (num == AI5Total)
				{
					_8PlayerGamePoker_AI5Bank += _8PlayerGamePokerPot;
					Notification.Show("AI 5, Won with : Cards, " + GetCardName(PlayerCards[4].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[4].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[4].Card3ID));
					_8PlayerGameStage = 1;
				}
				if (num == AI6Total)
				{
					_8PlayerGamePoker_AI6Bank += _8PlayerGamePokerPot;
					Notification.Show("AI 6, Won with : Cards, " + GetCardName(PlayerCards[5].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[5].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[5].Card3ID));
					_8PlayerGameStage = 1;
				}
				if (num == AI7Total)
				{
					_8PlayerGamePoker_AI7Bank += _8PlayerGamePokerPot;
					Notification.Show("AI 7, Won with : Cards, " + GetCardName(PlayerCards[6].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[6].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[6].Card3ID));
					_8PlayerGameStage = 1;
				}
				if (num == PlayerTotal)
				{
					_8PlayerGamePoker_PlayerBank += _8PlayerGamePokerPot;
					Notification.Show("Player, Won with : Cards, " + GetCardName(PlayerCards[7].Card1ID) + "~y~,~w~" + GetCardName(PlayerCards[7].Card2ID) + "~y~,~w~" + GetCardName(PlayerCards[7].Card3ID));
					_8PlayerGameStage = 1;
				}
				Notification.Show("Commuity Cards Were, " + CommunityCard1ID + "~y~,~w~" + CommunityCard2ID + "~y~,~w~" + CommunityCard3ID);
			}
		}

		public static void ShowPlayerBanks()
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			Convert.ToInt32(absoluteX / 2f);
			GetFoldCauseHighBet(LastRaiseAmmount, _8PlayerGamePoker_AI1Bank);
			GetFoldCauseLowScore(AI1Total);
			if (Player_Bust)
			{
				drawText("Player ~o~ BUST ~w", new Vector2(0.4f, 0.9f), 0.65f, 255, 255, 255);
			}
			if (!Player_Bust && Player_SkipTurn)
			{
				drawText("Player ~o~ FOLDED ~w", new Vector2(0.4f, 0.9f), 0.65f, 255, 255, 255);
			}
			if (!Player_Bust && !Player_SkipTurn)
			{
				drawText("Player ~g~$" + _8PlayerGamePoker_PlayerBank.ToString("N"), new Vector2(0.4f, 0.9f), 0.65f, 255, 255, 255);
			}
			Vector2 xy = World3DToScreen2d(new Vector3(AI_1.Position.X, AI_1.Position.Y, AI_1.Position.Z + 0.65f));
			if (AI1_Bust)
			{
				drawText("AI1\n ~o~ BUST ~w", xy, 0.25f, 255, 255, 255);
			}
			if (!AI1_Bust && AI1_SkipTurn)
			{
				drawText("AI1\n ~o~ FOLDED ~w", xy, 0.25f, 255, 255, 255);
			}
			if (!AI1_Bust && !AI1_SkipTurn)
			{
				drawText("AI1\n ~g~$" + _8PlayerGamePoker_AI1Bank.ToString("N"), xy, 0.25f, 255, 255, 255);
			}
			GetFoldCauseHighBet(LastRaiseAmmount, _8PlayerGamePoker_AI2Bank);
			GetFoldCauseLowScore(AI2Total);
			Vector2 xy2 = World3DToScreen2d(new Vector3(AI_2.Position.X, AI_2.Position.Y, AI_2.Position.Z + 1f));
			if (AI2_Bust)
			{
				drawText("AI2\n ~o~ BUST ~w", xy2, 0.25f, 255, 255, 255);
			}
			if (!AI2_Bust && AI2_SkipTurn)
			{
				drawText("AI2\n ~o~ FOLDED ~w", xy2, 0.25f, 255, 255, 255);
			}
			if (!AI2_Bust && !AI2_SkipTurn)
			{
				drawText("AI2\n ~g~$" + _8PlayerGamePoker_AI2Bank.ToString("N"), xy2, 0.25f, 255, 255, 255);
			}
			GetFoldCauseHighBet(LastRaiseAmmount, _8PlayerGamePoker_AI3Bank);
			GetFoldCauseLowScore(AI3Total);
			Vector2 xy3 = World3DToScreen2d(new Vector3(AI_3.Position.X, AI_3.Position.Y, AI_3.Position.Z + 1f));
			if (AI3_Bust)
			{
				drawText("AI3\n ~o~ BUST ~w", xy3, 0.25f, 255, 255, 255);
			}
			if (!AI3_Bust && AI3_SkipTurn)
			{
				drawText("AI3\n ~o~ FOLDED ~w", xy3, 0.25f, 255, 255, 255);
			}
			if (!AI3_Bust && !AI3_SkipTurn)
			{
				drawText("AI3\n ~g~$" + _8PlayerGamePoker_AI3Bank.ToString("N"), xy3, 0.25f, 255, 255, 255);
			}
			GetFoldCauseHighBet(LastRaiseAmmount, _8PlayerGamePoker_AI4Bank);
			GetFoldCauseLowScore(AI4Total);
			Vector2 xy4 = World3DToScreen2d(new Vector3(AI_4.Position.X, AI_4.Position.Y, AI_4.Position.Z + 1f));
			if (AI4_Bust)
			{
				drawText("AI4\n ~o~ BUST ~w", xy4, 0.25f, 255, 255, 255);
			}
			if (!AI4_Bust && AI4_SkipTurn)
			{
				drawText("AI4\n ~o~ FOLDED ~w", xy4, 0.25f, 255, 255, 255);
			}
			if (!AI4_Bust && !AI4_SkipTurn)
			{
				drawText("AI4\n ~g~$" + _8PlayerGamePoker_AI4Bank.ToString("N"), xy4, 0.25f, 255, 255, 255);
			}
			GetFoldCauseHighBet(LastRaiseAmmount, _8PlayerGamePoker_AI5Bank);
			GetFoldCauseLowScore(AI5Total);
			Vector2 xy5 = World3DToScreen2d(new Vector3(AI_5.Position.X, AI_5.Position.Y, AI_5.Position.Z + 1f));
			if (AI5_Bust)
			{
				drawText("AI5\n ~o~ BUST ~w", xy5, 0.25f, 255, 255, 255);
			}
			if (!AI5_Bust && AI5_SkipTurn)
			{
				drawText("AI5\n ~o~ FOLDED ~w", xy5, 0.25f, 255, 255, 255);
			}
			if (!AI5_Bust && !AI5_SkipTurn)
			{
				drawText("AI5\n ~g~$" + _8PlayerGamePoker_AI5Bank.ToString("N"), xy5, 0.25f, 255, 255, 255);
			}
			GetFoldCauseHighBet(LastRaiseAmmount, _8PlayerGamePoker_AI6Bank);
			GetFoldCauseLowScore(AI6Total);
			Vector2 xy6 = World3DToScreen2d(new Vector3(AI_6.Position.X, AI_6.Position.Y, AI_6.Position.Z + 1f));
			if (AI6_Bust)
			{
				drawText("AI6\n ~o~ BUST ~w", xy6, 0.25f, 255, 255, 255);
			}
			if (!AI6_Bust && AI6_SkipTurn)
			{
				drawText("AI6\n ~o~ FOLDED ~w", xy6, 0.25f, 255, 255, 255);
			}
			if (!AI6_Bust && !AI6_SkipTurn)
			{
				drawText("AI6\n ~g~$" + _8PlayerGamePoker_AI6Bank.ToString("N"), xy6, 0.25f, 255, 255, 255);
			}
			GetFoldCauseHighBet(LastRaiseAmmount, _8PlayerGamePoker_AI7Bank);
			GetFoldCauseLowScore(AI7Total);
			Vector2 xy7 = World3DToScreen2d(new Vector3(AI_7.Position.X, AI_7.Position.Y, AI_7.Position.Z + 1f));
			if (AI7_Bust)
			{
				drawText("AI7\n ~o~ BUST ~w", xy7, 0.25f, 255, 255, 255);
			}
			if (!AI7_Bust && AI7_SkipTurn)
			{
				drawText("AI7\n ~o~ FOLDED ~w", xy7, 0.25f, 255, 255, 255);
			}
			if (!AI7_Bust && !AI7_SkipTurn)
			{
				drawText("AI7\n ~g~$" + _8PlayerGamePoker_AI7Bank.ToString("N"), xy7, 0.25f, 255, 255, 255);
			}
			new Scaleform("instructional_buttons").Render2D();
		}

		public static void ShowPlayerTotals()
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			Convert.ToInt32(absoluteX / 2f);
			new Scaleform("instructional_buttons").Render2D();
		}

		protected static void pushArgs(object[] args)
		{
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
		}

		public static void CallHudFunction(int component, string name, params object[] args)
		{
			while (!Function.Call<bool>(Hash.BEGIN_SCALEFORM_SCRIPT_HUD_MOVIE_METHOD, component, name))
			{
				Script.Wait(0);
			}
			pushArgs(args);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		public static void ShowChipsChange(int Value, bool DeIncrease)
		{
			if (Function.Call<bool>(Hash.HAS_SCALEFORM_SCRIPT_HUD_MOVIE_LOADED, 22))
			{
				Function.Call(Hash.BEGIN_SCALEFORM_SCRIPT_HUD_MOVIE_METHOD, 22, "SET_PLAYER_CHIP_CHANGE");
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, Value);
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, DeIncrease);
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			}
			else
			{
				Function.Call(Hash.REQUEST_SCALEFORM_SCRIPT_HUD_MOVIE, 22);
			}
		}

		public static void HideChips()
		{
			if (Function.Call<bool>(Hash.HAS_SCALEFORM_SCRIPT_HUD_MOVIE_LOADED, 21))
			{
				Function.Call(Hash.HIDE_SCRIPTED_HUD_COMPONENT_THIS_FRAME, 21);
			}
		}

		public static void DisplayChips(int amt)
		{
			if (Function.Call<bool>(Hash.HAS_SCALEFORM_SCRIPT_HUD_MOVIE_LOADED, 21))
			{
				Function.Call(Hash.BEGIN_SCALEFORM_SCRIPT_HUD_MOVIE_METHOD, 21, "SET_PLAYER_CHIPS");
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, amt);
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
				if (Previous_Chips > Chips_Amount)
				{
					if (ShowChangeTime == -1)
					{
						ShowChangeTime = Game.GameTime + 5000;
						return;
					}
					if (Game.GameTime < ShowChangeTime)
					{
						ShowChipsChange(Previous_Chips - Chips_Amount, false);
						return;
					}
					Function.Call(Hash.REMOVE_SCALEFORM_SCRIPT_HUD_MOVIE, 22);
					ShowChangeTime = -1;
					Previous_Chips = Chips_Amount;
				}
				else if (Previous_Chips < Chips_Amount)
				{
					if (ShowChangeTime == -1)
					{
						ShowChangeTime = Game.GameTime + 5000;
						return;
					}
					if (Game.GameTime < ShowChangeTime)
					{
						ShowChipsChange(Chips_Amount - Previous_Chips, true);
						return;
					}
					Function.Call(Hash.REMOVE_SCALEFORM_SCRIPT_HUD_MOVIE, 22);
					ShowChangeTime = -1;
					Previous_Chips = Chips_Amount;
				}
			}
			else
			{
				Function.Call(Hash.REQUEST_SCALEFORM_SCRIPT_HUD_MOVIE, 21);
			}
		}

		public static void InsideTrackStuff()
		{
			Vector3 position = Game.Player.Character.Position;
			int num = Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_track_chair_01");
			Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 5f, num, 0, 0, 0);
			if (GetDistance_VDIST2(prop.Position, position, GetDistSign.LessEqual, 1.5f))
			{
				if (!SittingOnInsideTrackChair)
				{
					DisplayHelpTextThisFrame("Press ~INPUT_COVER~ to sit on chair to play Inside Track.");
				}
				if (SittingOnInsideTrackChair && !RunningInsideTrackScaleform)
				{
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Play Inside Track.\nPress ~INPUT_COVER~ to get up to play Inside Track. ");
				}
				if (Game.IsControlJustPressed(Control.Cover))
				{
					if (!SittingOnInsideTrackChair)
					{
						SittingOnInsideTrackChair = true;
						string dict = "anim_casino_a@amb@casino@games@insidetrack@male";
						string text = "enter_left_readyidle";
						InsideTrackChairScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, prop.Position.X, prop.Position.Y, prop.Position.Z, 0.0, 0.0, prop.Heading, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, InsideTrackChairScene, LoadDict(dict), text, 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, InsideTrackChairScene, text, LoadDict(dict), 16f, -16f, 0, 1148846080);
					}
					else
					{
						Function.Call(Hash.STOP_SOUND, RacingSoundLoop);
						Function.Call(Hash.RELEASE_SOUND_ID, RacingSoundLoop);
						GUIOn = false;
						RunningInsideTrackScaleform = false;
						HorsesAreRacing = false;
						ShowingEndScrene = false;
						RacingBet = 100;
						Screen = 0f;
						HorseRacingStage = 0;
						Function.Call(Hash.DISPLAY_RADAR, true);
						SittingOnInsideTrackChair = false;
						string dict2 = "anim_casino_a@amb@casino@games@insidetrack@male";
						int num2 = new Random().Next(0, 100);
						string text2 = "exit_left";
						text2 = ((num2 >= 50) ? "exit_right" : "exit_left");
						InsideTrackChairScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, prop.Position.X, prop.Position.Y, prop.Position.Z, 0.0, 0.0, prop.Heading, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, InsideTrackChairScene, LoadDict(dict2), text2, 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, InsideTrackChairScene, text2, LoadDict(dict2), 16f, -16f, 0, 1148846080);
					}
				}
			}
			if (!SittingOnInsideTrackChair)
			{
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, InsideTrackChairScene) >= 1065353216)
				{
					Game.Player.Character.Task.ClearAll();
					InsideTrackChairScene = -1;
				}
				return;
			}
			if (Game.IsControlJustPressed(Control.Context))
			{
				if (!GUIOn)
				{
					HorsesAreRacing = false;
					ShowingEndScrene = false;
					RacingBet = 100;
					Screen = 0f;
					HorseRacingStage = 0;
				}
				GUIOn = !GUIOn;
				RunningInsideTrackScaleform = !RunningInsideTrackScaleform;
				if (!GUIOn)
				{
					Function.Call(Hash.DISPLAY_RADAR, true);
				}
			}
			if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, InsideTrackChairScene) >= 1065353216 && GetDistance_VDIST2(prop.Position, position, GetDistSign.LessEqual, 1.5f))
			{
				string dict3 = "anim_casino_a@amb@casino@games@insidetrack@male";
				string text3 = "watchidle_a_var_01";
				InsideTrackChairScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, prop.Position.X, prop.Position.Y, prop.Position.Z, 0.0, 0.0, prop.Heading, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, InsideTrackChairScene, LoadDict(dict3), text3, 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, InsideTrackChairScene, text3, LoadDict(dict3), 16f, -16f, 0, 1148846080);
			}
		}

		public static void SetupGames()
		{
			SlotMachines.Add(new Vector3(1105.959f, 228.7872f, -49.8f));
			SlotMachinesRot.Add(35f);
			SlotMachines.Add(new Vector3(1106.344f, 230.4661f, -49.8f));
			SlotMachinesRot.Add(108f);
			SlotMachines.Add(new Vector3(1105.047f, 231.4999f, -49.8f));
			SlotMachinesRot.Add(178f);
			SlotMachines.Add(new Vector3(1103.892f, 230.5018f, -49.8f));
			SlotMachinesRot.Add(-107f);
			SlotMachines.Add(new Vector3(1101.564f, 230.6477f, -49.8f));
			SlotMachinesRot.Add(63f);
			SlotMachines.Add(new Vector3(1100.822f, 230.105f, -49.8f));
			SlotMachinesRot.Add(45f);
			SlotMachines.Add(new Vector3(1101.661f, 231.5672f, -49.8f));
			SlotMachinesRot.Add(73f);
			SlotMachines.Add(new Vector3(1101.94f, 232.4414f, -49.8f));
			SlotMachinesRot.Add(87f);
			SlotMachines.Add(new Vector3(1101.775f, 233.3168f, -49.8f));
			SlotMachinesRot.Add(101f);
			SlotMachines.Add(new Vector3(1107.15f, 234.9417f, -49.8f));
			SlotMachinesRot.Add(-111f);
			SlotMachines.Add(new Vector3(1107.558f, 233.311f, -49.8f));
			SlotMachinesRot.Add(-34f);
			SlotMachines.Add(new Vector3(1109.176f, 233.5511f, -49.8f));
			SlotMachinesRot.Add(32f);
			SlotMachines.Add(new Vector3(1109.793f, 234.9457f, -49.8f));
			SlotMachinesRot.Add(109f);
			SlotMachines.Add(new Vector3(1108.582f, 239.1074f, -49.8f));
			SlotMachinesRot.Add(-48f);
			SlotMachines.Add(new Vector3(1109.164f, 238.4004f, -49.8f));
			SlotMachinesRot.Add(-34f);
			SlotMachines.Add(new Vector3(1110.082f, 238.1253f, -49.8f));
			SlotMachinesRot.Add(-20f);
			SlotMachines.Add(new Vector3(1110.956f, 238.1476f, -49.8f));
			SlotMachinesRot.Add(-3f);
			SlotMachines.Add(new Vector3(1111.801f, 238.1584f, -49.8f));
			SlotMachinesRot.Add(25f);
			SlotMachines.Add(new Vector3(1112.645f, 238.5698f, -49.8f));
			SlotMachinesRot.Add(32f);
			SlotMachines.Add(new Vector3(1113.419f, 238.9962f, -49.8f));
			SlotMachinesRot.Add(46f);
			SlotMachines.Add(new Vector3(1115.422f, 234.7188f, -49.8f));
			SlotMachinesRot.Add(109f);
			SlotMachines.Add(new Vector3(1114.047f, 235.7146f, -49.8f));
			SlotMachinesRot.Add(-170f);
			SlotMachines.Add(new Vector3(1112.701f, 234.7498f, -49.8f));
			SlotMachinesRot.Add(-107f);
			SlotMachines.Add(new Vector3(1113.335f, 233.1384f, -49.8f));
			SlotMachinesRot.Add(-37f);
			SlotMachines.Add(new Vector3(1114.964f, 233.0724f, -49.8f));
			SlotMachinesRot.Add(25f);
			SlotMachines.Add(new Vector3(1116.269f, 228.2968f, -49.8f));
			SlotMachinesRot.Add(-39f);
			SlotMachines.Add(new Vector3(1117.877f, 2278.35742f, -49.8f));
			SlotMachinesRot.Add(44f);
			SlotMachines.Add(new Vector3(1118.452f, 229.9547f, -49.8f));
			SlotMachinesRot.Add(103f);
			SlotMachines.Add(new Vector3(1117.169f, 230.8778f, -49.8f));
			SlotMachinesRot.Add(-175f);
			SlotMachines.Add(new Vector3(1117.169f, 230.8778f, -49.8f));
			SlotMachinesRot.Add(-175f);
			SlotMachines.Add(new Vector3(1129.584f, 250.9299f, -51.2f));
			SlotMachinesRot.Add(-173f);
			SlotMachines.Add(new Vector3(1130.55f, 251.0233f, -51.2f));
			SlotMachinesRot.Add(165f);
			SlotMachines.Add(new Vector3(1131.325f, 250.5148f, -51.2f));
			SlotMachinesRot.Add(154f);
			SlotMachines.Add(new Vector3(1132.006f, 249.9666f, -51.2f));
			SlotMachinesRot.Add(133f);
			SlotMachines.Add(new Vector3(1132.541f, 249.3008f, -51.2f));
			SlotMachinesRot.Add(119f);
			SlotMachines.Add(new Vector3(1132.924f, 248.4754f, -51.2f));
			SlotMachinesRot.Add(105f);
			SlotMachines.Add(new Vector3(1133.166f, 247.5615f, -51.2f));
			SlotMachinesRot.Add(84f);
			SlotMachines.Add(new Vector3(1137.762f, 251.4668f, -51.2f));
			SlotMachinesRot.Add(-44f);
			SlotMachines.Add(new Vector3(1139.381f, 251.0917f, -51.2f));
			SlotMachinesRot.Add(25f);
			SlotMachines.Add(new Vector3(1140.171f, 252.5691f, -51.2f));
			SlotMachinesRot.Add(102f);
			SlotMachines.Add(new Vector3(1138.972f, 253.6587f, -51.2f));
			SlotMachinesRot.Add(175f);
			SlotMachines.Add(new Vector3(1135.809f, 256.7634f, -51.2f));
			SlotMachinesRot.Add(98f);
			SlotMachines.Add(new Vector3(1134.669f, 257.789f, -51.2f));
			SlotMachinesRot.Add(168f);
			SlotMachines.Add(new Vector3(1133.367f, 257.0611f, -51.2f));
			SlotMachinesRot.Add(-117f);
		}

		public static Vector2 World3DToScreen2d(Vector3 pos)
		{
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, pos.X, pos.Y, pos.Z, outputArgument, outputArgument2);
			return new Vector2(outputArgument.GetResult<float>(), outputArgument2.GetResult<float>());
		}

		public void Show_RQE(string R, string Q, string E)
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			Convert.ToInt32(absoluteX / 2f);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), E);
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 44, 0), Q);
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 80, 0), R);
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public static void Show_EGQ(string E, string G, string Q)
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			Convert.ToInt32(absoluteX / 2f);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), E);
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 58, 0), G);
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 44, 0), Q);
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public static void ShowRouletteOptions(Prop Table)
		{
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			if (PrevBetPos != BetPosNum)
			{
				foreach (Prop betPosMarker in BetPosMarkers)
				{
					if (betPosMarker != null)
					{
						betPosMarker.Delete();
					}
				}
				BetPosMarkers.Clear();
				PrevBetPos = BetPosNum;
			}
			if (BetPosNum == 1)
			{
				Vector3 offsetPosition = Table.GetOffsetPosition(new Vector3(-0.1365f, -0.1516f, 0.935f));
				float heading = Table.Heading;
				Prop marker = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.75f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_marker_02a"), 0, 0, 0);
				if (!DoesObjectExist(marker))
				{
					marker = GlobalHelper.CreateProp(RequestModel("vw_prop_vw_marker_01a"), offsetPosition, new Vector3(0f, 0f, heading), false, false);
					BetPosMarkers.Add(marker);
				}
			}
			if (BetPosNum == 2)
			{
				Vector3 offsetPosition2 = Table.GetOffsetPosition(new Vector3(-0.14f, 0.103f, 0.935f));
				float heading2 = Table.Heading;
				Prop marker2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, offsetPosition2.X, offsetPosition2.Y, offsetPosition2.Z, 0.75f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_marker_02a"), 0, 0, 0);
				if (!DoesObjectExist(marker2))
				{
					marker2 = GlobalHelper.CreateProp(RequestModel("vw_prop_vw_marker_01a"), offsetPosition2, new Vector3(0f, 0f, heading2), false, false);
					BetPosMarkers.Add(marker2);
				}
			}
			if (BetPosNum >= 3)
			{
				DrawRouletteBoxes(GetRouletteBetPosition(BetPosNum, Table), Table);
			}
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Quit Roulette");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 24, 0), "Place Bet of $" + Bet.ToString("N") + " on " + ConvertRouletteIDtoPositionName(BetPosNum));
			if (RouletteBets.Count >= 1)
			{
				scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 76, 0), "Spin Roulette Wheel");
			}
			scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 172, 0), "");
			scaleform.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 173, 0), "-/+ Bet");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
			int count = RouletteBets.Count;
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.85f, 0.21f, 0.04f);
			drawText("BETS MADE:", 0.88f, 0.841f, 0.3f, 255, 255, 255);
			drawNumberStr(count.ToString(), 0.950000048f, 0.832f, 0.48f, 255, 255, 255);
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.890000045f, 0.21f, 0.04f);
			drawText("BET ON MARKER:", 0.88f, 0.881000042f, 0.3f, 255, 255, 255);
			drawNumber((int)Bet, 0.950000048f, 0.872000039f, 0.48f, 255, 255, 255);
			if (Game.GameTime >= TimeBetweenBets)
			{
				if (Game.IsControlPressed(Control.PhoneUp))
				{
					RouletteBetIndex++;
					Bet = GetBetFromIndex(RouletteBetIndex);
					TimeBetweenBets = Game.GameTime + Set_TimeBetweenBets;
					if (RouletteMarker1 != null)
					{
						RouletteMarker1.Delete();
					}
					Vector3 offsetPosition3 = Table.GetOffsetPosition(new Vector3(-0.14f, -0.3f, 0.935f));
					RouletteMarker1 = GlobalHelper.CreateProp(RequestModel(GetCurrentRouletteBetChipModel()), offsetPosition3, true, false);
					RouletteMarker1.IsPositionFrozen = false;
				}
				if (Game.IsControlPressed(Control.PhoneDown))
				{
					if (RouletteBetIndex >= 1)
					{
						RouletteBetIndex--;
					}
					if (Bet >= 100f)
					{
						Bet = GetBetFromIndex(RouletteBetIndex);
						if (RouletteMarker1 != null)
						{
							RouletteMarker1.Delete();
						}
						Vector3 offsetPosition4 = Table.GetOffsetPosition(new Vector3(-0.14f, -0.3f, 0.935f));
						RouletteMarker1 = GlobalHelper.CreateProp(RequestModel(GetCurrentRouletteBetChipModel()), offsetPosition4, true, false);
						RouletteMarker1.IsPositionFrozen = false;
					}
				}
			}
			if (Bet <= 0f)
			{
				Bet = 100f;
			}
		}

		public static void ShowRouletteOptionsEnd(Prop Table)
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			Convert.ToInt32(absoluteX / 2f);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Quit Roulette");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 44, 0), "Spin Roulette wheel");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public static void ShowRouletteOptionsEndGame(Prop Table)
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			Convert.ToInt32(absoluteX / 2f);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Quit Roulette");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 58, 0), "Play Again");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public static void Show_EGQR(string E, string G, string Q, string R)
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			Convert.ToInt32(absoluteX / 2f);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), E);
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 58, 0), G);
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 44, 0), Q);
			scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 45, 0), R);
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public static void Show_EGQRX(string E, string G, string Q, string R, string X)
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			Convert.ToInt32(absoluteX / 2f);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), E);
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 58, 0), G);
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 44, 0), Q);
			scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 45, 0), R);
			scaleform.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 73, 0), X);
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public static void Show_Q(string Q)
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			Convert.ToInt32(absoluteX / 2f);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 44, 0), Q);
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public static void Show_GQ(string G, string Q)
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			Convert.ToInt32(absoluteX / 2f);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 58, 0), G);
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 44, 0), Q);
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public static void Show_EQ(string E, string Q)
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			Convert.ToInt32(absoluteX / 2f);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), E);
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 44, 0), Q);
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public static void Show_G(string G)
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			Convert.ToInt32(absoluteX / 2f);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 58, 0), G);
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public static void Show_EQ_Chips(string E, string Q)
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			Convert.ToInt32(absoluteX / 2f);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "AVL Chips " + Chips_Amount + "  |  " + E);
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 44, 0), Q);
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public void Show_EG(string E, string G)
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			Convert.ToInt32(absoluteX / 2f);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), E);
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 58, 0), G);
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public static float GetTableHeight_Cards(int Table)
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

		public static void SetRandomOutfit(Ped Dancer)
		{
			Random random = new Random();
			if (Dancer.Gender == Gender.Female)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 0, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 0)), 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 2, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 2)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 2)), 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 3)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 3)), 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 8, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 8)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 8)), 0);
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 4)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 4, 4, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 4)), 1);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) != 4)
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
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 4, num, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 4)), 1);
				}
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 0, Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3), 0, 0);
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 4) == 4)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 5, 4, 0, 1);
				}
			}
			if (Dancer.Gender == Gender.Male)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 0, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 0)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 0)), 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 2, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 2)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 2)), 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 3)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 3)), 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 8, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 8)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 8)), 0);
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 0)
				{
					int num3 = random.Next(0, 4);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num3, 0);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 1)
				{
					int num4 = random.Next(0, 3);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num4, 0);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 2)
				{
					int num5 = random.Next(0, 3);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num5, 0);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 3)
				{
					int num6 = random.Next(0, 3);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num6, 0);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 4)
				{
					int num7 = random.Next(0, 2);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num7, 0);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 5)
				{
					int num8 = random.Next(0, 2);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num8, 0);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 6)
				{
					int num9 = random.Next(0, 2);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num9, 0);
				}
			}
		}

		public static string RectifyBallPos_Prize(int B)
		{
			string result = "";
			if (B == 38)
			{
				result = ReturnRouletteNumString(38);
			}
			if (B == 38)
			{
				result = ReturnRouletteNumString(37);
			}
			if (B == 1)
			{
				result = ReturnRouletteNumString(21);
			}
			if (B == 2)
			{
				result = ReturnRouletteNumString(6);
			}
			if (B == 3)
			{
				result = ReturnRouletteNumString(33);
			}
			if (B == 4)
			{
				result = ReturnRouletteNumString(16);
			}
			if (B == 5)
			{
				result = ReturnRouletteNumString(4);
			}
			if (B == 6)
			{
				result = ReturnRouletteNumString(23);
			}
			if (B == 7)
			{
				result = ReturnRouletteNumString(23);
			}
			if (B == 8)
			{
				result = ReturnRouletteNumString(35);
			}
			if (B == 9)
			{
				result = ReturnRouletteNumString(14);
			}
			if (B == 10)
			{
				result = ReturnRouletteNumString(2);
			}
			if (B == 11)
			{
				result = ReturnRouletteNumString(37);
			}
			if (B == 12)
			{
				result = ReturnRouletteNumString(28);
			}
			if (B == 13)
			{
				result = ReturnRouletteNumString(9);
			}
			if (B == 14)
			{
				result = ReturnRouletteNumString(26);
			}
			if (B == 15)
			{
				result = ReturnRouletteNumString(30);
			}
			if (B == 16)
			{
				result = ReturnRouletteNumString(11);
			}
			if (B == 17)
			{
				result = ReturnRouletteNumString(11);
			}
			if (B == 18)
			{
				result = ReturnRouletteNumString(7);
			}
			if (B == 19)
			{
				result = ReturnRouletteNumString(20);
			}
			if (B == 20)
			{
				result = ReturnRouletteNumString(17);
			}
			if (B == 21)
			{
				result = ReturnRouletteNumString(5);
			}
			if (B == 22)
			{
				result = ReturnRouletteNumString(34);
			}
			if (B == 23)
			{
				result = ReturnRouletteNumString(22);
			}
			if (B == 24)
			{
				result = ReturnRouletteNumString(15);
			}
			if (B == 25)
			{
				result = ReturnRouletteNumString(25);
			}
			if (B == 26)
			{
				result = ReturnRouletteNumString(36);
			}
			if (B == 27)
			{
				result = ReturnRouletteNumString(13);
			}
			if (B == 28)
			{
				result = ReturnRouletteNumString(1);
			}
			if (B == 29)
			{
				result = ReturnRouletteNumString(27);
			}
			if (B == 30)
			{
				result = ReturnRouletteNumString(10);
			}
			if (B == 31)
			{
				result = ReturnRouletteNumString(25);
			}
			if (B == 32)
			{
				result = ReturnRouletteNumString(29);
			}
			if (B == 33)
			{
				result = ReturnRouletteNumString(12);
			}
			if (B == 34)
			{
				result = ReturnRouletteNumString(8);
			}
			if (B == 35)
			{
				result = ReturnRouletteNumString(19);
			}
			if (B == 36)
			{
				result = ReturnRouletteNumString(31);
			}
			return result;
		}

		public static int RectifyBallPos_Num(int B)
		{
			int result = 0;
			if (B == 38)
			{
				ReturnRouletteNumString(38);
				result = 38;
			}
			if (B == 38)
			{
				ReturnRouletteNumString(37);
				result = 37;
			}
			if (B == 1)
			{
				ReturnRouletteNumString(21);
				result = 21;
			}
			if (B == 2)
			{
				ReturnRouletteNumString(6);
				result = 6;
			}
			if (B == 3)
			{
				ReturnRouletteNumString(33);
				result = 33;
			}
			if (B == 4)
			{
				ReturnRouletteNumString(16);
				result = 16;
			}
			if (B == 5)
			{
				ReturnRouletteNumString(4);
				result = 4;
			}
			if (B == 6)
			{
				ReturnRouletteNumString(23);
				result = 23;
			}
			if (B == 7)
			{
				ReturnRouletteNumString(23);
				result = 23;
			}
			if (B == 8)
			{
				ReturnRouletteNumString(35);
				result = 35;
			}
			if (B == 9)
			{
				ReturnRouletteNumString(14);
				result = 14;
			}
			if (B == 10)
			{
				ReturnRouletteNumString(2);
				result = 2;
			}
			if (B == 11)
			{
				ReturnRouletteNumString(37);
				result = 37;
			}
			if (B == 12)
			{
				ReturnRouletteNumString(28);
				result = 28;
			}
			if (B == 13)
			{
				ReturnRouletteNumString(9);
				result = 9;
			}
			if (B == 14)
			{
				ReturnRouletteNumString(26);
				result = 26;
			}
			if (B == 15)
			{
				ReturnRouletteNumString(30);
				result = 30;
			}
			if (B == 16)
			{
				ReturnRouletteNumString(11);
				result = 11;
			}
			if (B == 17)
			{
				ReturnRouletteNumString(11);
				result = 11;
			}
			if (B == 18)
			{
				ReturnRouletteNumString(7);
				result = 7;
			}
			if (B == 19)
			{
				ReturnRouletteNumString(20);
				result = 20;
			}
			if (B == 20)
			{
				ReturnRouletteNumString(17);
				result = 17;
			}
			if (B == 21)
			{
				ReturnRouletteNumString(5);
				result = 5;
			}
			if (B == 22)
			{
				ReturnRouletteNumString(34);
				result = 34;
			}
			if (B == 23)
			{
				ReturnRouletteNumString(22);
				result = 22;
			}
			if (B == 24)
			{
				ReturnRouletteNumString(15);
				result = 15;
			}
			if (B == 25)
			{
				ReturnRouletteNumString(25);
				result = 25;
			}
			if (B == 26)
			{
				ReturnRouletteNumString(36);
				result = 36;
			}
			if (B == 27)
			{
				ReturnRouletteNumString(13);
				result = 13;
			}
			if (B == 28)
			{
				ReturnRouletteNumString(1);
				result = 1;
			}
			if (B == 29)
			{
				ReturnRouletteNumString(27);
				result = 27;
			}
			if (B == 30)
			{
				ReturnRouletteNumString(10);
				result = 10;
			}
			if (B == 31)
			{
				ReturnRouletteNumString(25);
				result = 25;
			}
			if (B == 32)
			{
				ReturnRouletteNumString(29);
				result = 29;
			}
			if (B == 33)
			{
				ReturnRouletteNumString(12);
				result = 12;
			}
			if (B == 34)
			{
				ReturnRouletteNumString(8);
				result = 8;
			}
			if (B == 35)
			{
				ReturnRouletteNumString(19);
				result = 19;
			}
			if (B == 36)
			{
				ReturnRouletteNumString(31);
				result = 31;
			}
			return result;
		}

		public static string ReturnRouletteNumString(int num)
		{
			string result = "";
			if (num == 1)
			{
				result = "Red, 1";
			}
			if (num == 2)
			{
				result = "Black, 2";
			}
			if (num == 3)
			{
				result = "Red, 3";
			}
			if (num == 4)
			{
				result = "Black, 4";
			}
			if (num == 5)
			{
				result = "Red, 5";
			}
			if (num == 6)
			{
				result = "Black, 6";
			}
			if (num == 7)
			{
				result = "Red, 7";
			}
			if (num == 8)
			{
				result = "Black, 8";
			}
			if (num == 9)
			{
				result = "Red, 9";
			}
			if (num == 10)
			{
				result = "Black, 10";
			}
			if (num == 11)
			{
				result = "Black, 11";
			}
			if (num == 12)
			{
				result = "Red, 12";
			}
			if (num == 13)
			{
				result = "Black, 13";
			}
			if (num == 14)
			{
				result = "Red, 13";
			}
			if (num == 15)
			{
				result = "Black, 15";
			}
			if (num == 16)
			{
				result = "Red, 16";
			}
			if (num == 17)
			{
				result = "Black, 17";
			}
			if (num == 18)
			{
				result = "Red, 18";
			}
			if (num == 19)
			{
				result = "Red, 19";
			}
			if (num == 20)
			{
				result = "Black, 20";
			}
			if (num == 21)
			{
				result = "Red, 21";
			}
			if (num == 22)
			{
				result = "Black, 22";
			}
			if (num == 23)
			{
				result = "Red, 23";
			}
			if (num == 24)
			{
				result = "Black, 24";
			}
			if (num == 25)
			{
				result = "Red, 25";
			}
			if (num == 26)
			{
				result = "Black, 26";
			}
			if (num == 27)
			{
				result = "Red, 27";
			}
			if (num == 28)
			{
				result = "Black, 28";
			}
			if (num == 29)
			{
				result = "Black, 29";
			}
			if (num == 30)
			{
				result = "Red, 30";
			}
			if (num == 31)
			{
				result = "Black, 31";
			}
			if (num == 32)
			{
				result = "Red, 32";
			}
			if (num == 33)
			{
				result = "Black, 33";
			}
			if (num == 34)
			{
				result = "Red, 34";
			}
			if (num == 35)
			{
				result = "Black, 35";
			}
			if (num == 36)
			{
				result = "Red, 36";
			}
			if (num == 37)
			{
				result = "Green, 0";
			}
			if (num == 38)
			{
				result = "Green, 00";
			}
			return result;
		}

		public static void ResetDealerPos(DealerTable T)
		{
			if (T.Dealer != null)
			{
				T.Dealer.IsPositionFrozen = false;
				T.Dealer.Task.ClearAll();
				Script.Wait(1);
				T.Dealer.Position = T.DealerOriginPosition;
				T.Dealer.Heading = T.DealerOriginHeading;
				Script.Wait(1);
				PlayDealerAnim(T.Dealer);
				T.Dealer.IsPositionFrozen = true;
			}
		}

		public static int Roulette_Choose_Num(Prop Table)
		{
			ClosestTable = GetClosestTable(ClosestDealer.Position, RouletteDealerTables);
			int result = 1;
			Vic1 = 0;
			Vic2 = 0;
			Vic3 = 0;
			Random random = new Random();
			List<object> list = new List<object>();
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
			CurrentNum = "";
			Table.GetOffsetPosition(new Vector3(-1f, -0.36f, 1.06f));
			if (RoulleteTable.Equals("A"))
			{
				Table.GetOffsetPosition(new Vector3(-0.65f, -0.45f, 1.06f));
			}
			if (RoulleteTable.Equals("B"))
			{
				Table.GetOffsetPosition(new Vector3(-0.42f, -0.1f, 1.06f));
			}
			if (RoulleteTable.Equals("C"))
			{
				Table.GetOffsetPosition(new Vector3(-1f, -0.36f, 1.06f));
			}
			CurrentNum = (dynamic)list[random.Next(0, list.Count)];
			Function.Call(Hash.STOP_SOUND, RouletteSpinningSound);
			Function.Call(Hash.RELEASE_SOUND_ID, RouletteSpinningSound);
			if (!CurrentNum.Contains(","))
			{
				result = 38;
				B_roulette = 38;
			}
			if (CurrentNum.Contains(","))
			{
				try
				{
					string[] array = CurrentNum.Split(new char[1] { ',' });
					A_roulette = array[0];
					string text = array[1];
					B_roulette = int.Parse(text);
					if (text.Equals("0"))
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "dlc_vw_roulette_exit_37", "dlc_vw_table_games_roulette_exit_sounds", true);
						result = 37;
						B_roulette = 37;
					}
					if (text.Equals("00"))
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "dlc_vw_roulette_exit_38", "dlc_vw_table_games_roulette_exit_sounds", true);
						result = 38;
						LastRouletteBallNum = result.ToString();
						B_roulette = 38;
					}
					if (text.Equals("00"))
					{
						return result;
					}
					if (text.Equals("0"))
					{
						return result;
					}
					LastRouletteBallNum = B_roulette.ToString();
					result = B_roulette;
					return result;
				}
				catch
				{
					Notification.Show("error on spinwheel");
					return result;
				}
			}
			return result;
		}

		public static bool GetHasStraightFlush()
		{
			List<int> list = new List<int>();
			list.Add(PlayerCard_1);
			list.Add(PlayerCard_2);
			list.Add(PlayerCard_3);
			bool flag = false;
			if (list.Contains(1) && list.Contains(2) && list.Contains(3) && AreAllThreeCardsOfSameType())
			{
				flag = true;
			}
			if (list.Contains(2) && list.Contains(3) && list.Contains(4) && AreAllThreeCardsOfSameType())
			{
				flag = true;
			}
			if (list.Contains(3) && list.Contains(4) && list.Contains(5) && AreAllThreeCardsOfSameType())
			{
				flag = true;
			}
			if (list.Contains(4) && list.Contains(5) && list.Contains(6) && AreAllThreeCardsOfSameType())
			{
				flag = true;
			}
			if (list.Contains(5) && list.Contains(6) && list.Contains(7) && AreAllThreeCardsOfSameType())
			{
				flag = true;
			}
			if (list.Contains(6) && list.Contains(7) && list.Contains(8) && AreAllThreeCardsOfSameType())
			{
				flag = true;
			}
			if (list.Contains(7) && list.Contains(8) && list.Contains(9) && AreAllThreeCardsOfSameType())
			{
				flag = true;
			}
			if (list.Contains(8) && list.Contains(9) && list.Contains(10) && AreAllThreeCardsOfSameType())
			{
				flag = true;
			}
			if (list.Contains(9) && list.Contains(10) && list.Contains(11) && AreAllThreeCardsOfSameType())
			{
				flag = true;
			}
			if (list.Contains(10) && list.Contains(11) && list.Contains(12) && AreAllThreeCardsOfSameType())
			{
				flag = true;
			}
			if (list.Contains(11) && list.Contains(12) && list.Contains(13) && AreAllThreeCardsOfSameType())
			{
				flag = true;
			}
			if (flag)
			{
				bool flag2 = false;
				if (PlayerCard_1_ID.Contains("hrt") && PlayerCard_2_ID.Contains("hrt") && PlayerCard_3_ID.Contains("hrt"))
				{
					flag2 = true;
				}
				if (PlayerCard_1_ID.Contains("spd") && PlayerCard_2_ID.Contains("spd") && PlayerCard_3_ID.Contains("spd"))
				{
					flag2 = true;
				}
				if (PlayerCard_1_ID.Contains("dia") && PlayerCard_2_ID.Contains("dia") && PlayerCard_3_ID.Contains("dia"))
				{
					flag2 = true;
				}
				if (PlayerCard_1_ID.Contains("club") && PlayerCard_2_ID.Contains("club") && PlayerCard_3_ID.Contains("club"))
				{
					flag2 = true;
				}
				if (!flag2)
				{
					flag = false;
				}
			}
			return flag;
		}

		public static bool GetHasStraight(bool StraightFlush)
		{
			List<int> list = new List<int>();
			list.Add(PlayerCard_1);
			list.Add(PlayerCard_2);
			list.Add(PlayerCard_3);
			bool result = false;
			if (!StraightFlush)
			{
				if (list.Contains(1) && list.Contains(2) && list.Contains(3))
				{
					result = true;
				}
				if (list.Contains(2) && list.Contains(3) && list.Contains(4))
				{
					result = true;
				}
				if (list.Contains(3) && list.Contains(4) && list.Contains(5))
				{
					result = true;
				}
				if (list.Contains(4) && list.Contains(5) && list.Contains(6))
				{
					result = true;
				}
				if (list.Contains(5) && list.Contains(6) && list.Contains(7))
				{
					result = true;
				}
				if (list.Contains(6) && list.Contains(7) && list.Contains(8))
				{
					result = true;
				}
				if (list.Contains(7) && list.Contains(8) && list.Contains(9))
				{
					result = true;
				}
				if (list.Contains(8) && list.Contains(9) && list.Contains(10))
				{
					result = true;
				}
				if (list.Contains(9) && list.Contains(10) && list.Contains(11))
				{
					StraightFlush = true;
				}
				if (list.Contains(10) && list.Contains(11) && list.Contains(12))
				{
					StraightFlush = true;
				}
				if (list.Contains(11) && list.Contains(12) && list.Contains(13))
				{
					StraightFlush = true;
				}
			}
			return result;
		}

		public static bool GetHasThreeOfAkind()
		{
			List<int> list = new List<int>();
			list.Add(PlayerCard_1);
			list.Add(PlayerCard_2);
			list.Add(PlayerCard_3);
			bool result = false;
			if (list[0] == list[1] && list[1] == list[2])
			{
				result = true;
			}
			return result;
		}

		public static bool GetHasPair(bool ThreeOfAKind)
		{
			List<int> list = new List<int>();
			list.Add(PlayerCard_1);
			list.Add(PlayerCard_2);
			list.Add(PlayerCard_3);
			bool result = false;
			if (!ThreeOfAKind)
			{
				if (list[0] == list[1])
				{
					result = true;
				}
				if (list[0] == list[2])
				{
					result = true;
				}
				if (list[1] == list[2])
				{
					result = true;
				}
			}
			return result;
		}

		public static bool GetHasFlush()
		{
			new List<int> { PlayerCard_1, PlayerCard_2, PlayerCard_3 };
			return AreAllThreeCardsOfSameType();
		}

		public static bool AreAllThreeCardsOfSameType()
		{
			bool result = false;
			if (PlayerCard_1_ID.Contains("hrt") && PlayerCard_2_ID.Contains("hrt") && PlayerCard_3_ID.Contains("hrt"))
			{
				result = true;
			}
			if (PlayerCard_1_ID.Contains("spd") && PlayerCard_2_ID.Contains("spd") && PlayerCard_3_ID.Contains("spd"))
			{
				result = true;
			}
			if (PlayerCard_1_ID.Contains("dia") && PlayerCard_2_ID.Contains("dia") && PlayerCard_3_ID.Contains("dia"))
			{
				result = true;
			}
			if (PlayerCard_1_ID.Contains("club") && PlayerCard_2_ID.Contains("club") && PlayerCard_3_ID.Contains("club"))
			{
				result = true;
			}
			return result;
		}

		public static void CalculatePoker()
		{
			float num = PlayerCard_1 * PlayerCard_2 * PlayerCard_3;
			float num2 = DealerCard_1 * DealerCard_2 * DealerCard_3;
			Game.Player.Character.Task.PlayAnimation("anim_casino_b@amb@casino@games@blackjack@ped_male@engaged@01a@play@v02", "collect_chips", 8f, 1f, -1, AnimationFlags.Loop, -1f);
			new List<int> { PlayerCard_1, PlayerCard_2, PlayerCard_3 };
			bool hasFlush = GetHasFlush();
			bool hasThreeOfAkind = GetHasThreeOfAkind();
			bool hasPair = GetHasPair(hasThreeOfAkind);
			bool hasStraightFlush = GetHasStraightFlush();
			bool hasStraight = GetHasStraight(hasStraightFlush);
			Bet = Bet_Ante + Bet_PairPlus + Bet_Play;
			int num3 = Bet_Ante + Bet_PairPlus + Bet_Play;
			if (!hasStraightFlush && !hasStraight && !hasThreeOfAkind && !hasPair && !hasFlush && Bet_PairPlus > 0)
			{
				Bet -= Bet_PairPlus;
				Notification.Show("~t~3 Card Poker~w~ Player did not get a Pair or better,'Pair Plus' Bet $" + Bet_PairPlus.ToString("N") + " lost");
			}
			if (hasStraightFlush)
			{
				Bet = Bet_Ante * 5 + Bet_PairPlus * 40 + Bet_Play;
				Notification.Show("~t~3 Card Poker~w~ Player has a Straight Flush! Bet raised to $" + Bet.ToString("N"));
			}
			if (hasStraight)
			{
				Bet = Bet_Ante + Bet_PairPlus * 6 + Bet_Play;
				Notification.Show("~t~3 Card Poker~w~ Player has a Straight! Bet raised to $" + Bet.ToString("N"));
			}
			if (hasThreeOfAkind)
			{
				Bet = Bet_Ante * 4 + Bet_PairPlus * 30 + Bet_Play;
				Notification.Show("~t~3 Card Poker~w~ Player has a Three Of A Kind! Bet raised to $" + Bet.ToString("N"));
			}
			if (hasPair)
			{
				Bet = Bet_Ante + Bet_PairPlus + Bet_Play;
				Notification.Show("~t~3 Card Poker~w~ Player has a Pair! Bet raised to $" + Bet.ToString("N"));
			}
			if (hasFlush)
			{
				Bet = Bet_Ante + Bet_PairPlus * 4 + Bet_Play;
				Notification.Show("~t~3 Card Poker~w~ Player has a Flush! Bet raised to $" + Bet.ToString("N"));
			}
			Drawcards = false;
			bool flag;
			int num5;
			if (DealerCard_1 >= 11 && DealerCard_2 >= 11)
			{
				flag = DealerCard_3 >= 11;
			}
			else
				num5 = 0;
			int num4 = 0;
			if (DealerCard_1 < 12 && DealerCard_2 < 12 && DealerCard_3 < 12)
			{
				ShowHelpMessage("The Dealer does not have a Queen-high and does not play. The hand is a push.");
				Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, GetClosestDealer(Game.Player.Character.Position, PokerDealerTables), "MINIGAME_DEALER_ANOTHER_GO", "SPEECH_PARAMS_STANDARD", false);
				Chips_Amount += Bet_Ante;
				Notification.Show("~t~3 Card Poker~w~ Dealer pushed hand.  'Ante Bet' payed out 1 to 1, Chips chips earned $" + Bet_Ante.ToString("N"));
				Class1.Config.SetValue("Setup", "Chips_Amount", Chips_Amount);
				Class1.Config.Save();
				Bet_Play = 0;
			}
			else
			{
				if (num > num2)
				{
					ShowHelpMessage("The Dealer has lower hand than yours. You Win.");
					num4 = 3;
					if (hasStraightFlush || hasStraight || hasThreeOfAkind || hasPair || hasFlush)
					{
						num4 = 4;
					}
					List<int> list = new List<int>();
					list.Add(PlayerCard_1);
					list.Add(PlayerCard_2);
					list.Add(PlayerCard_3);
					list.Sort();
					List<int> list2 = new List<int>();
					list2.Add(DealerCard_1);
					list2.Add(DealerCard_2);
					list2.Add(DealerCard_3);
					list2.Sort();
					Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, GetClosestDealer(Game.Player.Character.Position, PokerDealerTables), "MINIGAME_DEALER_ANOTHER_GO", "SPEECH_PARAMS_STANDARD", false);
					Notification.Show("~t~3 Card Poker~w~ Player beat the dealer, 'Play Bet' won and 'Ante Bet' won, Chips chips earned $" + Bet.ToString("N"));
					Chips_Amount += (int)Bet;
					Class1.Config.SetValue("Setup", "Chips_Amount", Chips_Amount);
					Class1.Config.Save();
				}
				if (num < num2)
				{
					ShowHelpMessage("The Dealer has higher hand than yours. You lose.");
					if (Bet_Ante + Bet_Play >= 5000)
					{
						num4 = 1;
					}
					if (Bet_Ante + Bet_Play >= 10000)
					{
						num4 = 2;
					}
					if (Bet_Ante + Bet_Play >= 25000)
					{
						num4 = 0;
					}
					List<int> list3 = new List<int>();
					list3.Add(PlayerCard_1);
					list3.Add(PlayerCard_2);
					list3.Add(PlayerCard_3);
					list3.Sort();
					List<int> list4 = new List<int>();
					list4.Add(DealerCard_1);
					list4.Add(DealerCard_2);
					list4.Add(DealerCard_3);
					list4.Sort();
					Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, GetClosestDealer(Game.Player.Character.Position, PokerDealerTables), "MINIGAME_DEALER_WINS", "SPEECH_PARAMS_STANDARD", false);
					Notification.Show("~t~3 Card Poker~w~ Player failed to beat the dealer, 'Play Ante' & 'Bet Play' lost, Chips lost " + num3);
					Chips_Amount -= num3;
					Class1.Config.SetValue("Setup", "Chips_Amount", Chips_Amount);
					Class1.Config.Save();
				}
			}
			string text = "terrible";
			if (num4 <= 0)
			{
				text = "terrible";
			}
			if (num4 == 1)
			{
				text = "bad";
			}
			if (num4 == 2)
			{
				text = "impartial";
			}
			if (num4 == 3)
			{
				text = "good";
			}
			if (num4 >= 4)
			{
				text = "great";
			}
			Random random = new Random();
			List<string> list5 = new List<string> { "1", "2", "3", "4" };
			string dict = "anim_casino_b@amb@casino@games@shared@player@";
			string text2 = "reaction_" + text + "_var_0" + list5[random.Next(0, list5.Count)];
			LoadDict(dict);
			Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, PlayerOriginPosition.X, PlayerOriginPosition.Y, PlayerOriginPosition.Z - 1f, 0.0, 0.0, PlayerOriginHeading, 2);
			Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene2, LoadDict(dict), text2, 16f, -16f, 0, 1148846080);
			Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene2, text2, LoadDict(dict), 16f, -16f, 0, 1148846080);
			string dict2 = "anim_casino_b@amb@casino@games@blackjack@dealer";
			string text3 = "idle";
			Vector3 dealerAnimOrigin = GetDealerAnimOrigin();
			Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin.X, dealerAnimOrigin.Y, dealerAnimOrigin.Z, 0.0, 0.0, ClosestDealer.Rotation.Z - 180f, 2);
			Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, ClosestDealer, Scene1, LoadDict(dict2), text3, 16f, -16f, 0, 1148846080);
			Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ClosestDealer, Scene1, text3, LoadDict(dict2), 16f, -16f, 0, 1148846080);
			Endgame = true;
			Stage = 50f;
			Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, GetClosestDealer(Game.Player.Character.Position, BlackjackDealerTables), "MINIGAME_DEALER_ANOTHER_GO", "SPEECH_PARAMS_STANDARD", false);
			DealerCardsDrawn = 0;
			Playercards = 0;
			DealerCards = 0;
			PlayerCard_1 = 0;
			PlayerCard_2 = 0;
			PlayerCard_3 = 0;
			PlayerCard_4 = 0;
			PlayerCard_5 = 0;
			PlayerCard_6 = 0;
			Bet = 100f;
			DealerCard_1 = 0;
			DealerCard_2 = 0;
			DealerCard_3 = 0;
			DealerCard_4 = 0;
			DealerCard_5 = 0;
			DealerCard_6 = 0;
			Endgame = true;
			Bet_PairPlus = 0;
			Bet_Ante = 100;
			Bet_Play = 0;
		}

		public static int Set_PlayerCard_Blackjack(string CardId, int Cardnum)
		{
			int result = 0;
			if (CardId.Contains("ace") || CardId.Contains("a_a"))
			{
				result = 11;
			}
			if (CardId.Contains("king") || CardId.Contains("k_a"))
			{
				result = 10;
			}
			if (CardId.Contains("queen") || CardId.Contains("q_a"))
			{
				result = 10;
			}
			if (CardId.Contains("jack") || CardId.Contains("j_a"))
			{
				result = 10;
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

		public static int Set_DealerCard_Blackjack(string CardId, int Cardnum)
		{
			int result = 0;
			if (CardId.Contains("ace") || CardId.Contains("a_a"))
			{
				result = 11;
			}
			if (CardId.Contains("king") || CardId.Contains("k_a"))
			{
				result = 10;
			}
			if (CardId.Contains("queen") || CardId.Contains("q_a"))
			{
				result = 10;
			}
			if (CardId.Contains("jack") || CardId.Contains("j_a"))
			{
				result = 10;
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

		public static int Set_PlayerCard(string CardId, int Cardnum)
		{
			int result = 0;
			if (CardId.Contains("ace") || CardId.Contains("a_a"))
			{
				result = 14;
			}
			if (CardId.Contains("king") || CardId.Contains("k_a"))
			{
				result = 13;
			}
			if (CardId.Contains("queen") || CardId.Contains("q_a"))
			{
				result = 12;
			}
			if (CardId.Contains("jack") || CardId.Contains("j_a"))
			{
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

		public static int Set_DealerCard(string CardId, int Cardnum)
		{
			int result = 0;
			if (CardId.Contains("ace") || CardId.Contains("a_a"))
			{
				result = 14;
			}
			if (CardId.Contains("king") || CardId.Contains("k_a"))
			{
				result = 13;
			}
			if (CardId.Contains("queen") || CardId.Contains("q_a"))
			{
				result = 12;
			}
			if (CardId.Contains("jack") || CardId.Contains("j_a"))
			{
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

		public static int Set_PlayerCardPoker(string CardId, int Cardnum)
		{
			int result = 0;
			if (CardId.Contains("ace") || CardId.Contains("a_a"))
			{
				result = 14;
			}
			if (CardId.Contains("king") || CardId.Contains("k_a"))
			{
				result = 13;
			}
			if (CardId.Contains("queen") || CardId.Contains("q_a"))
			{
				result = 12;
			}
			if (CardId.Contains("jack") || CardId.Contains("j_a"))
			{
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

		public static int Get_PlayerCardPoker(string CardId)
		{
			string text = "num";
			int result = 0;
			if (CardId.Contains("ace") || CardId.Contains("a_a"))
			{
				text = "Ace";
				result = 14;
			}
			if (CardId.Contains("king") || CardId.Contains("k_a"))
			{
				text = "King";
				result = 13;
			}
			if (CardId.Contains("queen") || CardId.Contains("q_a"))
			{
				text = "Queen";
				result = 12;
			}
			if (CardId.Contains("jack") || CardId.Contains("j_a"))
			{
				text = "Jack";
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
				Notification.Show("~g~Player~w~  got Card : " + result);
			}
			else
			{
				Notification.Show("~g~Player~w~ got Card : " + text);
			}
			return result;
		}

		public static int Set_DealerCardPoker(string CardId, int Cardnum)
		{
			int result = 0;
			if (CardId.Contains("ace") || CardId.Contains("a_a"))
			{
				result = 14;
			}
			if (CardId.Contains("king") || CardId.Contains("k_a"))
			{
				result = 13;
			}
			if (CardId.Contains("queen") || CardId.Contains("q_a"))
			{
				result = 12;
			}
			if (CardId.Contains("jack") || CardId.Contains("j_a"))
			{
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

		public static int Set_8PlayerPokerCards(string CardId, int Cardnum, int PlayerID)
		{
			int result = 0;
			if (CardId.Contains("ace") || CardId.Contains("a_a"))
			{
				result = 14;
			}
			if (CardId.Contains("king") || CardId.Contains("k_a"))
			{
				result = 13;
			}
			if (CardId.Contains("queen") || CardId.Contains("q_a"))
			{
				result = 12;
			}
			if (CardId.Contains("jack") || CardId.Contains("j_a"))
			{
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

		public static bool GetFoldCauseHighBet(float Bet, float Balance)
		{
			bool result = false;
			Random random = new Random();
			if (Balance / 100f * 5f > Bet && Balance / 100f * 10f < Bet && random.Next(0, 100) < 3)
			{
				result = true;
			}
			if (Balance / 100f * 10f > Bet && Balance / 100f * 15f < Bet && random.Next(0, 100) < 10)
			{
				result = true;
			}
			if (Balance / 100f * 15f > Bet && Balance / 100f * 20f < Bet && random.Next(0, 100) < 15)
			{
				result = true;
			}
			if (Balance / 100f * 20f > Bet && Balance / 100f * 100f < Bet && random.Next(0, 100) < 25)
			{
				result = true;
			}
			return result;
		}

		public static bool GetFoldCauseLowScore(float Score)
		{
			bool result = false;
			Random random = new Random();
			if (Score <= 150f && random.Next(0, 100) < 65)
			{
				result = true;
			}
			if (Score > 150f && Score <= 350f && random.Next(0, 100) < 45)
			{
				result = true;
			}
			if (Score > 350f && Score <= 600f && random.Next(0, 100) < 30)
			{
				result = true;
			}
			if (Score > 600f && Score <= 900f && random.Next(0, 100) < 15)
			{
				result = true;
			}
			if (Score > 900f && Score <= 1400f && random.Next(0, 100) < 7)
			{
				result = true;
			}
			if (Score > 1400f && Score < 2000f && random.Next(0, 100) < 4)
			{
				result = true;
			}
			if (Score >= 2000f && random.Next(0, 100) == 1)
			{
				result = true;
			}
			return result;
		}

		public static float GetPlayerTotal(string Card1, string Card2, string Card3)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			string text = Card1;
			if (text.Contains("queen") || text.Contains("q_a"))
			{
				num = 13f;
			}
			if (text.Contains("king") || text.Contains("k_a"))
			{
				num = 12f;
			}
			if (text.Contains("jack") || text.Contains("j_a"))
			{
				num = 11f;
			}
			if (text.Contains("ace") || text.Contains("a_a"))
			{
				num = 14f;
			}
			if (text.Contains("02"))
			{
				num = 2f;
			}
			if (text.Contains("03"))
			{
				num = 3f;
			}
			if (text.Contains("04"))
			{
				num = 4f;
			}
			if (text.Contains("05"))
			{
				num = 5f;
			}
			if (text.Contains("06"))
			{
				num = 6f;
			}
			if (text.Contains("07"))
			{
				num = 7f;
			}
			if (text.Contains("08"))
			{
				num = 8f;
			}
			if (text.Contains("09"))
			{
				num = 9f;
			}
			if (text.Contains("10"))
			{
				num = 10f;
			}
			text = Card2;
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
			text = Card3;
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
			if (CommunityCard1Value == num)
			{
				num *= 2f;
			}
			if (CommunityCard1Value == num2)
			{
				num2 *= 2f;
			}
			if (CommunityCard1Value == num3)
			{
				num3 *= 2f;
			}
			if (CommunityCard2Value == num)
			{
				num *= 2f;
			}
			if (CommunityCard2Value == num2)
			{
				num2 *= 2f;
			}
			if (CommunityCard2Value == num3)
			{
				num3 *= 2f;
			}
			if (CommunityCard3Value == num)
			{
				num *= 2f;
			}
			if (CommunityCard3Value == num2)
			{
				num2 *= 2f;
			}
			if (CommunityCard3Value == num3)
			{
				num3 *= 2f;
			}
			return num * num2 * num3;
		}

		public static float GetCardTotal(string Card1)
		{
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

		public static string GetCardName(string Card1)
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

		public static string GetEnterAnim()
		{
			string result = "sit_enter_left";
			int num = new Random().Next(1, 4);
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

		public static string GetExitAnim()
		{
			string result = "sit_enter_left";
			int num = new Random().Next(1, 2);
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

		public static float ReturnChipAngle(List<DealerTable> T)
		{
			return GetClosestTable(Game.Player.Character.Position, T).Heading + PokerChip_RotationOffset;
		}

		public static Vector3 ReturnChipPosition(List<DealerTable> T, BetTypeId Type)
		{
			Vector3 result = default(Vector3);
			if (Type == BetTypeId.PairPlusBet)
			{
				result = GetClosestTable(Game.Player.Character.Position, T).GetOffsetPosition(PairPlusBet_Offset);
			}
			if (Type == BetTypeId.AnteBet)
			{
				result = GetClosestTable(Game.Player.Character.Position, T).GetOffsetPosition(AnteBet_Offset);
			}
			if (Type == BetTypeId.PlayBet)
			{
				result = GetClosestTable(Game.Player.Character.Position, T).GetOffsetPosition(PlayBet_Offset);
			}
			if (Type == BetTypeId.BlackJack)
			{
				result = GetClosestTable(Game.Player.Character.Position, T).GetOffsetPosition(Blackjack_ChipOffset);
			}
			return result;
		}

		public static void PlaySyncAnim(Ped AI, Entity Center, int SyncScene, string dict, string anim)
		{
			SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, AI.Position.X, AI.Position.Y, Center.Position.Z, 0.0, 0.0, AI.Heading, 2);
			Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, AI, SyncScene, LoadDict(dict), anim, 16f, -16f, 0, 1148846080);
			Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, AI, SyncScene, anim, LoadDict(dict), 16f, -16f, 0, 1148846080);
		}

		public static void PlaySyncExitAnim(Ped AI, Entity Center, int SyncScene, string dict, string anim)
		{
			SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, AI.Position.X, AI.Position.Y, Center.Position.Z, 0.0, 0.0, AI.Heading + 40f, 2);
			Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, AI, SyncScene, LoadDict(dict), anim, 16f, -16f, 0, 1148846080);
			Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, AI, SyncScene, anim, LoadDict(dict), 16f, -16f, 0, 1148846080);
		}

		public static void PlaySyncAnimReverse(Ped AI, Entity Center, int SyncScene, string dict, string anim)
		{
			SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, AI.Position.X, AI.Position.Y, Center.Position.Z, 0.0, 0.0, AI.Heading, 2);
			Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, AI, SyncScene, LoadDict(dict), anim, -16f, 16, 0, 1148846080);
			Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, AI, SyncScene, anim, LoadDict(dict), -16f, 16, 0, 1148846080);
		}

		public static void PlaySyncAnimRotate180(Ped AI, Entity Center, int SyncScene, string dict, string anim)
		{
			SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, AI.Position.X, AI.Position.Y, Center.Position.Z, 0.0, 0.0, AI.Heading - 180f, 2);
			Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, AI, SyncScene, LoadDict(dict), anim, 16f, -16f, 0, 1148846080);
			Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, AI, SyncScene, anim, LoadDict(dict), 16f, -16f, 0, 1148846080);
		}

		public static Prop GetClosestTable(Vector3 vec, List<DealerTable> List)
		{
			Prop table = _8PlayerPokerGameTable1;
			float radius = 9999f;
			foreach (DealerTable item in List)
			{
				if (GetDistance_VDIST2(vec, item.Table.Position, GetDistSign.LessEqual, radius))
				{
					radius = GetDistance(vec, item.Table.Position);
					table = item.Table;
				}
			}
			return table;
		}

		public static DealerTable GetClosestDealerTable(Vector3 vec, List<DealerTable> List)
		{
			if (List.Count == 0)
			{
				return null;
			}
			DealerTable result = List[0];
			float radius = 9999f;
			foreach (DealerTable item in List)
			{
				if (GetDistance_VDIST2(vec, item.Table.Position, GetDistSign.LessEqual, radius))
				{
					radius = GetDistance(vec, item.Table.Position);
					result = item;
				}
			}
			return result;
		}

		public static string GetRandomDealerModel()
		{
			string text = "s_m_y_casino_01";
			int num = new Random().Next(0, 100);
			if (num < 50)
			{
				text = "s_m_y_casino_01";
			}
			if (num > 50)
			{
				text = "s_f_y_casino_01";
			}
			if (text == null || text.Equals(""))
			{
				text = "s_m_y_casino_01";
			}
			Script.Wait(1);
			return text;
		}

		public static ValueTuple<Vector3, Vector3, int> GetClosestSeat(Prop Table)
		{
			Vector3 vector = GET_PLAYER_CHAIR_POSITION(1, Table);
			Vector3 item = GET_PLAYER_CHAIR_POSITION(2, Table);
			Vector3 item2 = GET_PLAYER_CHAIR_POSITION(3, Table);
			Vector3 item3 = GET_PLAYER_CHAIR_POSITION(4, Table);
			List<Vector3> list = new List<Vector3>();
			list.Add(vector);
			list.Add(item);
			list.Add(item2);
			list.Add(item3);
			int num = 1;
			Vector3 item4 = vector;
			float radius = 10f;
			for (int i = 0; i < list.Count; i++)
			{
				int num2 = i + 1;
				Vector3 vector2 = list[i];
				if (GetDistance_VDIST2(Game.Player.Character.Position, vector2, GetDistSign.Less, radius))
				{
					item4 = vector2;
					num = num2;
					radius = GetDistance(Game.Player.Character.Position, vector2);
				}
			}
			return new ValueTuple<Vector3, Vector3, int>(item4, GET_PLAYER_CHAIR_ROTATION(num, Table), num);
		}

		public static Prop GetClosesBlackjackTable()
		{
			Vector3 position = Game.Player.Character.Position;
			int num = Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_blckjack_01b");
			Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 10f, num, 0, 0, 0);
			int num2 = Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_blckjack_01");
			Prop prop2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 10f, num2, 0, 0, 0);
			Prop result = null;
			if (prop != null)
			{
				result = prop;
			}
			if (prop2 != null)
			{
				result = prop2;
			}
			if (prop != null && prop2 != null)
			{
				result = ((!GetDistance_VDIST2(Game.Player.Character.Position, prop.Position, GetDistSign.LessEqual, GetDistance(Game.Player.Character.Position, prop2.Position))) ? prop2 : prop);
			}
			return result;
		}

		public static Prop GetClosesPokerTable()
		{
			Vector3 position = Game.Player.Character.Position;
			int num = Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_3cardpoker_01b");
			Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 10f, num, 0, 0, 0);
			int num2 = Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_3cardpoker_01");
			Prop prop2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 10f, num2, 0, 0, 0);
			Prop prop3 = null;
			if (prop != null)
			{
				prop3 = prop;
			}
			if (prop2 != null)
			{
				prop3 = prop2;
			}
			if (prop != null && prop2 != null)
			{
				prop3 = ((!GetDistance_VDIST2(Game.Player.Character.Position, prop.Position, GetDistSign.LessEqual, GetDistance(Game.Player.Character.Position, prop2.Position))) ? prop2 : prop);
			}
			if (prop3 == _8PlayerPokerGameTable1 || prop3 == _8PlayerPokerGameTable2 || prop3 == _8PlayerPokerGameTable3)
			{
				prop3 = null;
			}
			return prop3;
		}

		public static Prop GetClosesRouletteTable()
		{
			Vector3 position = Game.Player.Character.Position;
			return Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_roulette_01b"), 0, 0, 0);
		}

		public static Ped GetClosestDealer(Vector3 vec, List<DealerTable> List)
		{
			Ped result = Game.Player.Character;
			float radius = 9999f;
			foreach (DealerTable item in List)
			{
				if (item.Dealer == null)
				{
					item.Dealer = GlobalHelper.CreatePed(RequestModel(GetRandomDealerModel()), item.Table.GetOffsetPosition(new Vector3(0f, 0.8f, 0.02f)), item.Table.Heading - 180f);
					item.Dealer.IsPositionFrozen = true;
					Class1.SetPedOutfit(item.Dealer);
					Class1.MainPed.Add(item.Dealer);
					PlayDealerAnim(item.Dealer);
					item.Dealer.RelationshipGroup = 2;
					item.Dealer.AlwaysKeepTask = true;
					item.Dealer.BlockPermanentEvents = true;
					item.Dealer.Position = SetDealerPositionAtTable(item.Table, item.TableType).Item1;
					item.Dealer.Rotation = SetDealerPositionAtTable(item.Table, item.TableType).Item2;
					item.DealerOriginHeading = item.Dealer.Heading;
					item.DealerOriginPosition = new Vector3(item.Dealer.Position.X, item.Dealer.Position.Y, item.Dealer.Position.Z - 1f);
				}
				if (item.Dealer != null && GetDistance_VDIST2(vec, item.Dealer.Position, GetDistSign.LessEqual, radius))
				{
					radius = GetDistance(vec, item.Dealer.Position);
					result = item.Dealer;
				}
			}
			return result;
		}

		public static ValueTuple<Vector3, Vector3> SetDealerPositionAtTable(Prop Table, GTAO_DiamondCasino.TableType tableType)
		{
			Vector3 item = Vector3.Zero;
			Vector3 item2 = Vector3.Zero;
			if (tableType == GTAO_DiamondCasino.TableType.Blackjack || tableType == GTAO_DiamondCasino.TableType.Poker)
			{
				item = Table.GetOffsetPosition(new Vector3(0f, 0.8f, 0.02f));
				item2 = new Vector3(0f, 0f, Table.Heading - 180f);
			}
			if (tableType == GTAO_DiamondCasino.TableType.Roulette)
			{
				item = Table.GetOffsetPosition(new Vector3(-0.14f, 0.61f, 0.02f));
				item2 = new Vector3(0f, 0f, Table.Heading + 180f);
			}
			return new ValueTuple<Vector3, Vector3>(item, item2);
		}

		public static string func_440(int iParam0)
		{
			switch (iParam0)
			{
			case 1:
				return "MINIGAME_NO";
			case 2:
				return "MINIGAME_YES";
			case 3:
				return "MINIGAME_BUST";
			case 4:
				return "MINIGAME_BIG_WIN";
			case 5:
				return "MINIGAME_SMALL_WIN";
			case 6:
				return "MINIGAME_BIG_LOSS";
			case 7:
				return "MINIGAME_SMALL_LOSS";
			case 8:
				return "MINIGAME_MONO_WILLING";
			case 9:
				return "MINIGAME_MONO_GOOD_GAME";
			case 10:
				return "MINIGAME_MONO_BAD_GAME";
			case 11:
				return "TCPOKER_FOLD";
			case 12:
				return "TCPOKER_HAND_STRAIGHT_FLUSH";
			case 13:
				return "TCPOKER_HAND_THREE_KIND";
			case 14:
				return "TCPOKER_HAND_STRAIGHT";
			case 15:
				return "TCPOKER_HAND_FLUSH";
			case 16:
				return "TCPOKER_HAND_PAIR";
			case 17:
				return "TCPOKER_HAND_HIGH_CARD";
			case 18:
				return "ROULETTE_BET_ODDS";
			case 19:
				return "ROULETTE_BET_EVENS";
			case 20:
				return "ROULETTE_BET_RED";
			case 21:
				return "ROULETTE_BET_BLACK";
			case 22:
				return "BJACK_STAND";
			case 23:
				return "BJACK_HIT";
			case 24:
				return "BJACK_SPLIT";
			case 25:
				return "BJACK_DOUBLE";
			default:
				return "";
			}
		}

		public static string getSpeechParam(int iParam0)
		{
			switch (iParam0)
			{
			case 0:
				return "SPEECH_PARAMS_STANDARD";
			case 1:
				return "SPEECH_PARAMS_ALLOW_REPEAT";
			case 2:
				return "SPEECH_PARAMS_BEAT";
			case 3:
				return "SPEECH_PARAMS_FORCE";
			case 4:
				return "SPEECH_PARAMS_FORCE_FRONTEND";
			case 5:
				return "SPEECH_PARAMS_FORCE_NO_REPEAT_FRONTEND";
			case 6:
				return "SPEECH_PARAMS_FORCE_NORMAL";
			case 7:
				return "SPEECH_PARAMS_FORCE_NORMAL_CLEAR";
			case 8:
				return "SPEECH_PARAMS_FORCE_NORMAL_CRITICAL";
			case 9:
				return "SPEECH_PARAMS_FORCE_SHOUTED";
			case 10:
				return "SPEECH_PARAMS_FORCE_SHOUTED_CLEAR";
			case 11:
				return "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL";
			case 12:
				return "SPEECH_PARAMS_FORCE_PRELOAD_ONLY";
			case 13:
				return "SPEECH_PARAMS_MEGAPHONE";
			case 14:
				return "SPEECH_PARAMS_HELI";
			case 15:
				return "SPEECH_PARAMS_FORCE_MEGAPHONE";
			case 16:
				return "SPEECH_PARAMS_FORCE_HELI";
			case 17:
				return "SPEECH_PARAMS_INTERRUPT";
			case 18:
				return "SPEECH_PARAMS_INTERRUPT_SHOUTED";
			case 19:
				return "SPEECH_PARAMS_INTERRUPT_SHOUTED_CLEAR";
			case 20:
				return "SPEECH_PARAMS_INTERRUPT_SHOUTED_CRITICAL";
			case 21:
				return "SPEECH_PARAMS_INTERRUPT_NO_FORCE";
			case 22:
				return "SPEECH_PARAMS_INTERRUPT_FRONTEND";
			case 23:
				return "SPEECH_PARAMS_INTERRUPT_NO_FORCE_FRONTEND";
			case 24:
				return "SPEECH_PARAMS_ADD_BLIP";
			case 25:
				return "SPEECH_PARAMS_ADD_BLIP_ALLOW_REPEAT";
			case 26:
				return "SPEECH_PARAMS_ADD_BLIP_FORCE";
			case 27:
				return "SPEECH_PARAMS_ADD_BLIP_SHOUTED";
			case 28:
				return "SPEECH_PARAMS_ADD_BLIP_SHOUTED_FORCE";
			case 29:
				return "SPEECH_PARAMS_ADD_BLIP_INTERRUPT";
			case 30:
				return "SPEECH_PARAMS_ADD_BLIP_INTERRUPT_FORCE";
			case 31:
				return "SPEECH_PARAMS_FORCE_PRELOAD_ONLY_SHOUTED";
			case 32:
				return "SPEECH_PARAMS_FORCE_PRELOAD_ONLY_SHOUTED_CLEAR";
			case 33:
				return "SPEECH_PARAMS_FORCE_PRELOAD_ONLY_SHOUTED_CRITICAL";
			case 34:
				return "SPEECH_PARAMS_SHOUTED";
			case 35:
				return "SPEECH_PARAMS_SHOUTED_CLEAR";
			case 36:
				return "SPEECH_PARAMS_SHOUTED_CRITICAL";
			default:
				return "";
			}
		}

		public static string func_435(int iParam0)
		{
			switch (iParam0)
			{
			case 0:
				return "SPEECH_PARAMS_STANDARD";
			case 1:
				return "SPEECH_PARAMS_ALLOW_REPEAT";
			case 2:
				return "SPEECH_PARAMS_BEAT";
			case 3:
				return "SPEECH_PARAMS_FORCE";
			case 4:
				return "SPEECH_PARAMS_FORCE_FRONTEND";
			case 5:
				return "SPEECH_PARAMS_FORCE_NO_REPEAT_FRONTEND";
			case 6:
				return "SPEECH_PARAMS_FORCE_NORMAL";
			case 7:
				return "SPEECH_PARAMS_FORCE_NORMAL_CLEAR";
			case 8:
				return "SPEECH_PARAMS_FORCE_NORMAL_CRITICAL";
			case 9:
				return "SPEECH_PARAMS_FORCE_SHOUTED";
			case 10:
				return "SPEECH_PARAMS_FORCE_SHOUTED_CLEAR";
			case 11:
				return "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL";
			case 12:
				return "SPEECH_PARAMS_FORCE_PRELOAD_ONLY";
			case 13:
				return "SPEECH_PARAMS_MEGAPHONE";
			case 14:
				return "SPEECH_PARAMS_HELI";
			case 15:
				return "SPEECH_PARAMS_FORCE_MEGAPHONE";
			case 16:
				return "SPEECH_PARAMS_FORCE_HELI";
			case 17:
				return "SPEECH_PARAMS_INTERRUPT";
			case 18:
				return "SPEECH_PARAMS_INTERRUPT_SHOUTED";
			case 19:
				return "SPEECH_PARAMS_INTERRUPT_SHOUTED_CLEAR";
			case 20:
				return "SPEECH_PARAMS_INTERRUPT_SHOUTED_CRITICAL";
			case 21:
				return "SPEECH_PARAMS_INTERRUPT_NO_FORCE";
			case 22:
				return "SPEECH_PARAMS_INTERRUPT_FRONTEND";
			case 23:
				return "SPEECH_PARAMS_INTERRUPT_NO_FORCE_FRONTEND";
			case 24:
				return "SPEECH_PARAMS_ADD_BLIP";
			case 25:
				return "SPEECH_PARAMS_ADD_BLIP_ALLOW_REPEAT";
			case 26:
				return "SPEECH_PARAMS_ADD_BLIP_FORCE";
			case 27:
				return "SPEECH_PARAMS_ADD_BLIP_SHOUTED";
			case 28:
				return "SPEECH_PARAMS_ADD_BLIP_SHOUTED_FORCE";
			case 29:
				return "SPEECH_PARAMS_ADD_BLIP_INTERRUPT";
			case 30:
				return "SPEECH_PARAMS_ADD_BLIP_INTERRUPT_FORCE";
			case 31:
				return "SPEECH_PARAMS_FORCE_PRELOAD_ONLY_SHOUTED";
			case 32:
				return "SPEECH_PARAMS_FORCE_PRELOAD_ONLY_SHOUTED_CLEAR";
			case 33:
				return "SPEECH_PARAMS_FORCE_PRELOAD_ONLY_SHOUTED_CRITICAL";
			case 34:
				return "SPEECH_PARAMS_SHOUTED";
			case 35:
				return "SPEECH_PARAMS_SHOUTED_CLEAR";
			case 36:
				return "SPEECH_PARAMS_SHOUTED_CRITICAL";
			default:
				return "";
			}
		}

		public static void PlayDealerAnim(Ped ped)
		{
			Random random = new Random();
			float num = random.Next(1, 27);
			ped.BlockPermanentEvents = true;
			if (num < 2f)
			{
				ped.Task.PlayAnimation("anim_casino_b@amb@casino@games@threecardpoker@dealer", "deck_shuffle", random.Next(64, 320), 1f, -1, AnimationFlags.Loop, -1f);
			}
			if (num >= 2f && num < 5f)
			{
				ped.Task.PlayAnimation("anim_casino_b@amb@casino@games@blackjack@dealer_female", "dealer_idle", random.Next(64, 320), 1f, -1, AnimationFlags.Loop, -1f);
			}
			if (num >= 5f && num < 7f)
			{
				ped.Task.PlayAnimation("anim_casino_b@amb@casino@games@blackjack@dealer_female", "dealer_focus_player_01_idle", random.Next(64, 320), 1f, -1, AnimationFlags.Loop, -1f);
			}
			if (num >= 7f && num < 10f)
			{
				ped.Task.PlayAnimation("anim_casino_b@amb@casino@games@shared@dealer@", "female_idle_var_01", random.Next(64, 320), 1f, -1, AnimationFlags.Loop, -1f);
			}
			if (num >= 10f && num < 12f)
			{
				ped.Task.PlayAnimation("anim_casino_b@amb@casino@games@shared@dealer@", "female_idle_var_02", random.Next(64, 320), 1f, -1, AnimationFlags.Loop, -1f);
			}
			if (num >= 12f && num < 14f)
			{
				ped.Task.PlayAnimation("anim_casino_b@amb@casino@games@shared@dealer@", "female_idle_var_03", random.Next(64, 320), 1f, -1, AnimationFlags.Loop, -1f);
			}
			if (num >= 14f && num < 17f)
			{
				ped.Task.PlayAnimation("anim_casino_b@amb@casino@games@shared@dealer@", "female_idle_var_04", random.Next(64, 320), 1f, -1, AnimationFlags.Loop, -1f);
			}
			if (num >= 17f && num < 20f)
			{
				ped.Task.PlayAnimation("anim_casino_b@amb@casino@games@shared@dealer@", "female_idle_var_05", random.Next(64, 320), 1f, -1, AnimationFlags.Loop, -1f);
			}
			if (num >= 20f && num < 23f)
			{
				ped.Task.PlayAnimation("anim_casino_b@amb@casino@games@shared@dealer@", "female_idle_var_06", random.Next(64, 320), 1f, -1, AnimationFlags.Loop, -1f);
			}
			if (num >= 23f && num < 25f)
			{
				ped.Task.PlayAnimation("anim_casino_b@amb@casino@games@shared@dealer@", "female_idle_var_07", random.Next(64, 320), 1f, -1, AnimationFlags.Loop, -1f);
			}
			if (num >= 25f && num < 28f)
			{
				ped.Task.PlayAnimation("anim_casino_b@amb@casino@games@shared@dealer@", "female_idle_var_08", random.Next(64, 320), 1f, -1, AnimationFlags.Loop, -1f);
			}
			ped.AlwaysKeepTask = true;
		}

		public static void ResetCardDeck()
		{
			CardDeck.Clear();
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
			foreach (object item2 in list)
			{
				string item = (string)(dynamic)item2;
				CardDeck.Add(item);
			}
		}

		public static CardID CardNumberToID(int ID)
		{
			CardID result = CardID.Two;
			if (ID == 2)
			{
				result = CardID.Two;
			}
			if (ID == 3)
			{
				result = CardID.Three;
			}
			if (ID == 4)
			{
				result = CardID.Four;
			}
			if (ID == 5)
			{
				result = CardID.Five;
			}
			if (ID == 6)
			{
				result = CardID.Six;
			}
			if (ID == 7)
			{
				result = CardID.Seven;
			}
			if (ID == 8)
			{
				result = CardID.Eight;
			}
			if (ID == 9)
			{
				result = CardID.Nine;
			}
			if (ID == 10)
			{
				float num = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 100f);
				if (num >= 0f && num <= 25f)
				{
					result = CardID.Ten;
				}
				if (num > 25f && num <= 50f)
				{
					result = CardID.Jack;
				}
				if (num > 50f && num <= 75f)
				{
					result = CardID.Queen;
				}
				if (num > 75f && num <= 100f)
				{
					result = CardID.King;
				}
			}
			if (ID == 11)
			{
				result = CardID.Ace;
			}
			return result;
		}

		public static CardID GetRandomCard_Two_to_Nine()
		{
			float num = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 10f, 90f);
			CardID result = CardID.Two;
			if (num > 10f && num <= 25f)
			{
				result = CardID.Two;
			}
			if (num > 25f && num <= 35f)
			{
				result = CardID.Three;
			}
			if (num > 35f && num <= 50f)
			{
				result = CardID.Four;
			}
			if (num > 50f && num <= 60f)
			{
				result = CardID.Five;
			}
			if (num > 60f && num <= 70f)
			{
				result = CardID.Six;
			}
			if (num > 70f && num <= 75f)
			{
				result = CardID.Seven;
			}
			if (num > 75f && num <= 80f)
			{
				result = CardID.Eight;
			}
			if (num > 80f && num <= 90f)
			{
				result = CardID.Nine;
			}
			return result;
		}

		public static CardID GetRandomCard_Two_to_Six()
		{
			float num = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 10f, 70f);
			CardID result = CardID.Two;
			if (num > 10f && num <= 25f)
			{
				result = CardID.Two;
			}
			if (num > 25f && num <= 35f)
			{
				result = CardID.Three;
			}
			if (num > 35f && num <= 50f)
			{
				result = CardID.Four;
			}
			if (num > 50f && num <= 60f)
			{
				result = CardID.Five;
			}
			if (num > 60f && num <= 70f)
			{
				result = CardID.Six;
			}
			return result;
		}

		public static CardID GetRandomCard_Two_to_Five()
		{
			float num = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 10f, 60f);
			CardID result = CardID.Two;
			if (num > 10f && num <= 25f)
			{
				result = CardID.Two;
			}
			if (num > 25f && num <= 35f)
			{
				result = CardID.Three;
			}
			if (num > 35f && num <= 50f)
			{
				result = CardID.Four;
			}
			if (num > 50f && num <= 60f)
			{
				result = CardID.Five;
			}
			return result;
		}

		public static CardID GetRandomCard_Two_to_Four()
		{
			float num = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 10f, 60f);
			CardID result = CardID.Two;
			if (num > 10f && num <= 30f)
			{
				result = CardID.Two;
			}
			if (num > 30f && num <= 45f)
			{
				result = CardID.Three;
			}
			if (num > 45f && num <= 60f)
			{
				result = CardID.Four;
			}
			return result;
		}

		public static CardID GetRandomCard_Two_to_Three()
		{
			float num = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 50.0);
			CardID result = CardID.Ace;
			if (num >= 0f && num <= 25f)
			{
				result = CardID.Two;
			}
			if (num > 25f && num <= 50f)
			{
				result = CardID.Three;
			}
			return result;
		}

		public static CardID ReturnBlackjackDealerCard(int Total)
		{
			CardID result = GetRandomCard_Two_to_Nine();
			if (Total <= 14)
			{
				result = GetRandomCard_Two_to_Nine();
			}
			if (Total == 15)
			{
				result = GetRandomCard_Two_to_Six();
			}
			if (Total == 16)
			{
				result = GetRandomCard_Two_to_Five();
			}
			if (Total == 17)
			{
				result = GetRandomCard_Two_to_Four();
			}
			if (Total >= 18)
			{
				result = GetRandomCard_Two_to_Three();
			}
			return result;
		}

		public static string GetRandomCard_Blackjack_Dealer(int Total)
		{
			if (CardDeck.Count == 0)
			{
				ResetCardDeck();
			}
			CardID cardID = ReturnBlackjackDealerCard(Total);
			string text = "";
			Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 1f, 52f);
			text = GetCardOfType(CardVarient.Any, cardID);
			while (GetCardTypeCount(cardID) == 0)
			{
				drawText("~r~Stuck in GetRandomCard_Blackjack_Dealer 1 Loop  " + text, 0.1f, 0.1f, 0.5f, 255, 0, 0);
				cardID = ReturnBlackjackDealerCard(Total);
				Script.Wait(1);
			}
			while (!CardDeck.Contains(text))
			{
				drawText("~r~Stuck in GetRandomCard_Blackjack_Dealer 2 Loop  " + text, 0.1f, 0.1f, 0.5f, 255, 0, 0);
				Script.Wait(1);
				text = GetCardOfType(CardVarient.Any, cardID);
			}
			CardDeck.Remove(text);
			return text;
		}

		public static string GetRandomCard_Blackjack()
		{
			if (CardDeck.Count == 0)
			{
				ResetCardDeck();
			}
			string text = "";
			if (Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 1f, 52f) <= 16f)
			{
				CardID iD = CardID.Ten;
				float num = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 100f);
				if (num <= 25f)
				{
					iD = CardID.Ten;
				}
				if (num >= 25f && num <= 50f)
				{
					iD = CardID.Jack;
				}
				if (num >= 50f && num <= 75f)
				{
					iD = CardID.King;
				}
				if (num >= 75f && num <= 100f)
				{
					iD = CardID.Queen;
				}
				text = GetCardOfType(CardVarient.Any, iD);
				CardID randomCard_Two_to_Nine = GetRandomCard_Two_to_Nine();
				text = GetCardOfType(CardVarient.Any, randomCard_Two_to_Nine);
				while (GetCardTypeCount(randomCard_Two_to_Nine) == 0)
				{
					iD = CardID.Ten;
					num = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 100f);
					if (num <= 25f)
					{
						iD = CardID.Ten;
					}
					if (num >= 25f && num <= 50f)
					{
						iD = CardID.Jack;
					}
					if (num >= 50f && num <= 75f)
					{
						iD = CardID.King;
					}
					if (num >= 75f && num <= 100f)
					{
						iD = CardID.Queen;
					}
					Script.Wait(1);
				}
				while (!CardDeck.Contains(text))
				{
					drawText("~r~Stuck in GetRandomCard_Blackjack 1 Loop  " + text, 0.1f, 0.1f, 0.5f, 255, 0, 0);
					Script.Wait(1);
					text = GetCardOfType(CardVarient.Any, iD);
				}
			}
			else if (Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 1f, 52f) <= 7.68f && GetCardTypeCount(CardID.Ace) > 0)
			{
				CardID cardID = CardID.Ace;
				text = GetCardOfType(CardVarient.Any, cardID);
				while (GetCardTypeCount(cardID) == 0)
				{
					drawText("~r~Stuck in GetRandomCard_Blackjack 2 Loop " + text + "/" + GetCardTypeCount(cardID), 0.1f, 0.1f, 0.5f, 255, 0, 0);
					cardID = GetRandomCard_Two_to_Nine();
					Script.Wait(1);
				}
				while (!CardDeck.Contains(text))
				{
					drawText("~r~Stuck in GetRandomCard_Blackjack 3 Loop " + text + "/" + GetCardTypeCount(cardID), 0.1f, 0.1f, 0.5f, 255, 0, 0);
					Script.Wait(1);
					text = GetCardOfType(CardVarient.Any, cardID);
				}
			}
			else
			{
				CardID randomCard_Two_to_Nine2 = GetRandomCard_Two_to_Nine();
				text = GetCardOfType(CardVarient.Any, randomCard_Two_to_Nine2);
				while (GetCardTypeCount(randomCard_Two_to_Nine2) == 0)
				{
					drawText("~r~Stuck in GetRandomCard_Blackjack 2 Loop " + text + "/" + GetCardTypeCount(randomCard_Two_to_Nine2), 0.1f, 0.1f, 0.5f, 255, 0, 0);
					randomCard_Two_to_Nine2 = GetRandomCard_Two_to_Nine();
					Script.Wait(1);
				}
				while (!CardDeck.Contains(text))
				{
					drawText("~r~Stuck in GetRandomCard_Blackjack 3 Loop " + text + "/" + GetCardTypeCount(randomCard_Two_to_Nine2), 0.1f, 0.1f, 0.5f, 255, 0, 0);
					Script.Wait(1);
					text = GetCardOfType(CardVarient.Any, randomCard_Two_to_Nine2);
				}
			}
			CardDeck.Remove(text);
			return text;
		}

		public static string GetRandomCardOfType(CardID Card_ID)
		{
			if (CardDeck.Count == 0)
			{
				ResetCardDeck();
			}
			string text = "";
			text = GetCardOfType(CardVarient.Any, Card_ID);
			while (!CardDeck.Contains(text))
			{
				drawText("~r~Stuck in GetRandomCardOfType " + Card_ID, 0.1f, 0.15f, 0.5f, 255, 0, 0);
				Script.Wait(1);
				text = GetCardOfType(CardVarient.Any, Card_ID);
			}
			CardDeck.Remove(text);
			return text;
		}

		public static int GetCardTypeCount(CardID Card_ID)
		{
			int num = 0;
			if (CardDeck.Count == 0)
			{
				ResetCardDeck();
			}
			string cardOfType = GetCardOfType(CardVarient.Club, Card_ID);
			if (CardDeck.Contains(cardOfType))
			{
				num++;
			}
			cardOfType = GetCardOfType(CardVarient.Spade, Card_ID);
			if (CardDeck.Contains(cardOfType))
			{
				num++;
			}
			cardOfType = GetCardOfType(CardVarient.Heart, Card_ID);
			if (CardDeck.Contains(cardOfType))
			{
				num++;
			}
			cardOfType = GetCardOfType(CardVarient.Diamond, Card_ID);
			if (CardDeck.Contains(cardOfType))
			{
				num++;
			}
			return num;
		}

		public static string GetRandomCardPlayer()
		{
			new Random();
			List<string> cardDeck = CardDeck;
			string text = "";
			if (cardDeck.Count == 0)
			{
				ResetCardDeck();
			}
			CardID cardID = CardID.None;
			while (cardID == CardID.None)
			{
				Script.Wait(1);
				float num = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 120f);
				if (num <= 5f && GetCardTypeCount(CardID.Two) > 0)
				{
					cardID = CardID.Two;
					text = GetCardOfType(CardVarient.Any, cardID);
				}
				if (num > 5f && num <= 10f && GetCardTypeCount(CardID.Three) > 0)
				{
					cardID = CardID.Three;
					text = GetCardOfType(CardVarient.Any, cardID);
				}
				if (num > 10f && num <= 18f && GetCardTypeCount(CardID.Four) > 0)
				{
					cardID = CardID.Four;
					text = GetCardOfType(CardVarient.Any, cardID);
				}
				if (num > 18f && num <= 26f && GetCardTypeCount(cardID = CardID.Five) > 0)
				{
					cardID = CardID.Five;
					text = GetCardOfType(CardVarient.Any, cardID);
				}
				if (num > 26f && num <= 38f && GetCardTypeCount(cardID = CardID.Six) > 0)
				{
					cardID = CardID.Six;
					text = GetCardOfType(CardVarient.Any, cardID);
				}
				if (num > 38f && num <= 52f && GetCardTypeCount(cardID = CardID.Seven) > 0)
				{
					cardID = CardID.Seven;
					text = GetCardOfType(CardVarient.Any, cardID);
				}
				if (num > 52f && num <= 72f && GetCardTypeCount(cardID = CardID.Eight) > 0)
				{
					cardID = CardID.Eight;
					text = GetCardOfType(CardVarient.Any, cardID);
				}
				if (num > 72f && num <= 82f && GetCardTypeCount(cardID = CardID.Nine) > 0)
				{
					cardID = CardID.Nine;
					text = GetCardOfType(CardVarient.Any, cardID);
				}
				if (num > 82f && num <= 90f && GetCardTypeCount(cardID = CardID.Ten) > 0)
				{
					cardID = CardID.Ten;
					text = GetCardOfType(CardVarient.Any, cardID);
				}
				if (num > 90f && num <= 98f && GetCardTypeCount(cardID = CardID.Ace) > 0)
				{
					cardID = CardID.Ace;
					text = GetCardOfType(CardVarient.Any, cardID);
				}
				if (num > 98f && num <= 106f && GetCardTypeCount(cardID = CardID.Jack) > 0)
				{
					cardID = CardID.Jack;
					text = GetCardOfType(CardVarient.Any, cardID);
				}
				if (num > 106f && num <= 112f && GetCardTypeCount(cardID = CardID.King) > 0)
				{
					cardID = CardID.King;
					text = GetCardOfType(CardVarient.Any, cardID);
				}
				if (num > 112f && num <= 120f && GetCardTypeCount(cardID = CardID.Queen) > 0)
				{
					cardID = CardID.Queen;
					text = GetCardOfType(CardVarient.Any, cardID);
				}
			}
			CardDeck.Remove(text);
			return text;
		}

		public static CardID GetRandomCardPlayer_ReturnCardID()
		{
			new Random();
			List<string> cardDeck = CardDeck;
			string item = "";
			if (cardDeck.Count == 0)
			{
				ResetCardDeck();
			}
			CardID cardID = CardID.None;
			while (cardID == CardID.None)
			{
				Script.Wait(1);
				float num = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 120f);
				if (num <= 5f && GetCardTypeCount(CardID.Two) > 0)
				{
					cardID = CardID.Two;
					item = GetCardOfType(CardVarient.Any, cardID);
				}
				if (num > 5f && num <= 10f && GetCardTypeCount(CardID.Three) > 0)
				{
					cardID = CardID.Three;
					item = GetCardOfType(CardVarient.Any, cardID);
				}
				if (num > 10f && num <= 18f && GetCardTypeCount(CardID.Four) > 0)
				{
					cardID = CardID.Four;
					item = GetCardOfType(CardVarient.Any, cardID);
				}
				if (num > 18f && num <= 26f && GetCardTypeCount(cardID = CardID.Five) > 0)
				{
					cardID = CardID.Five;
					item = GetCardOfType(CardVarient.Any, cardID);
				}
				if (num > 26f && num <= 38f && GetCardTypeCount(cardID = CardID.Six) > 0)
				{
					cardID = CardID.Six;
					item = GetCardOfType(CardVarient.Any, cardID);
				}
				if (num > 38f && num <= 52f && GetCardTypeCount(cardID = CardID.Seven) > 0)
				{
					cardID = CardID.Seven;
					item = GetCardOfType(CardVarient.Any, cardID);
				}
				if (num > 52f && num <= 72f && GetCardTypeCount(cardID = CardID.Eight) > 0)
				{
					cardID = CardID.Eight;
					item = GetCardOfType(CardVarient.Any, cardID);
				}
				if (num > 72f && num <= 82f && GetCardTypeCount(cardID = CardID.Nine) > 0)
				{
					cardID = CardID.Nine;
					item = GetCardOfType(CardVarient.Any, cardID);
				}
				if (num > 82f && num <= 90f && GetCardTypeCount(cardID = CardID.Ten) > 0)
				{
					cardID = CardID.Ten;
					item = GetCardOfType(CardVarient.Any, cardID);
				}
				if (num > 90f && num <= 98f && GetCardTypeCount(cardID = CardID.Ace) > 0)
				{
					cardID = CardID.Ace;
					item = GetCardOfType(CardVarient.Any, cardID);
				}
				if (num > 98f && num <= 106f && GetCardTypeCount(cardID = CardID.Jack) > 0)
				{
					cardID = CardID.Jack;
					item = GetCardOfType(CardVarient.Any, cardID);
				}
				if (num > 106f && num <= 112f && GetCardTypeCount(cardID = CardID.King) > 0)
				{
					cardID = CardID.King;
					item = GetCardOfType(CardVarient.Any, cardID);
				}
				if (num > 112f && num <= 120f && GetCardTypeCount(cardID = CardID.Queen) > 0)
				{
					cardID = CardID.Queen;
					item = GetCardOfType(CardVarient.Any, cardID);
				}
			}
			CardDeck.Remove(item);
			return cardID;
		}

		public static int GetRandomCardPlayer_ReturnID()
		{
			new Random();
			List<string> cardDeck = CardDeck;
			string item = "";
			if (cardDeck.Count == 0)
			{
				ResetCardDeck();
			}
			int result = 0;
			CardID cardID = CardID.None;
			while (cardID == CardID.None)
			{
				Script.Wait(1);
				float num = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 120f);
				if (num <= 5f && GetCardTypeCount(CardID.Two) > 0)
				{
					cardID = CardID.Two;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 0;
				}
				if (num > 5f && num <= 10f && GetCardTypeCount(CardID.Three) > 0)
				{
					cardID = CardID.Three;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 1;
				}
				if (num > 10f && num <= 18f && GetCardTypeCount(CardID.Four) > 0)
				{
					cardID = CardID.Four;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 2;
				}
				if (num > 18f && num <= 26f && GetCardTypeCount(cardID = CardID.Five) > 0)
				{
					cardID = CardID.Five;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 3;
				}
				if (num > 26f && num <= 38f && GetCardTypeCount(cardID = CardID.Six) > 0)
				{
					cardID = CardID.Six;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 4;
				}
				if (num > 38f && num <= 52f && GetCardTypeCount(cardID = CardID.Seven) > 0)
				{
					cardID = CardID.Seven;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 5;
				}
				if (num > 52f && num <= 72f && GetCardTypeCount(cardID = CardID.Eight) > 0)
				{
					cardID = CardID.Eight;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 6;
				}
				if (num > 72f && num <= 82f && GetCardTypeCount(cardID = CardID.Nine) > 0)
				{
					cardID = CardID.Nine;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 7;
				}
				if (num > 82f && num <= 90f && GetCardTypeCount(cardID = CardID.Ten) > 0)
				{
					cardID = CardID.Ten;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 8;
				}
				if (num > 90f && num <= 98f && GetCardTypeCount(cardID = CardID.Jack) > 0)
				{
					cardID = CardID.Jack;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 9;
				}
				if (num > 98f && num <= 106f && GetCardTypeCount(cardID = CardID.Queen) > 0)
				{
					cardID = CardID.Queen;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 10;
				}
				if (num > 106f && num <= 112f && GetCardTypeCount(cardID = CardID.King) > 0)
				{
					cardID = CardID.King;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 11;
				}
				if (num > 112f && num <= 120f && GetCardTypeCount(cardID = CardID.Ace) > 0)
				{
					cardID = CardID.Ace;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 12;
				}
			}
			CardDeck.Remove(item);
			return result;
		}

		public static int GetRandomCardPlayer_ReturnID_Max_Queen()
		{
			new Random();
			List<string> cardDeck = CardDeck;
			string item = "";
			if (cardDeck.Count == 0)
			{
				ResetCardDeck();
			}
			int result = 0;
			CardID cardID = CardID.None;
			while (cardID == CardID.None)
			{
				Script.Wait(1);
				float num = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 106f);
				if (num <= 5f && GetCardTypeCount(CardID.Two) > 0)
				{
					cardID = CardID.Two;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 0;
				}
				if (num > 5f && num <= 10f && GetCardTypeCount(CardID.Three) > 0)
				{
					cardID = CardID.Three;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 1;
				}
				if (num > 10f && num <= 18f && GetCardTypeCount(CardID.Four) > 0)
				{
					cardID = CardID.Four;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 2;
				}
				if (num > 18f && num <= 26f && GetCardTypeCount(cardID = CardID.Five) > 0)
				{
					cardID = CardID.Five;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 3;
				}
				if (num > 26f && num <= 38f && GetCardTypeCount(cardID = CardID.Six) > 0)
				{
					cardID = CardID.Six;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 4;
				}
				if (num > 38f && num <= 52f && GetCardTypeCount(cardID = CardID.Seven) > 0)
				{
					cardID = CardID.Seven;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 5;
				}
				if (num > 52f && num <= 72f && GetCardTypeCount(cardID = CardID.Eight) > 0)
				{
					cardID = CardID.Eight;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 6;
				}
				if (num > 72f && num <= 82f && GetCardTypeCount(cardID = CardID.Nine) > 0)
				{
					cardID = CardID.Nine;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 7;
				}
				if (num > 82f && num <= 90f && GetCardTypeCount(cardID = CardID.Ten) > 0)
				{
					cardID = CardID.Ten;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 8;
				}
				if (num > 90f && num <= 98f && GetCardTypeCount(cardID = CardID.Jack) > 0)
				{
					cardID = CardID.Jack;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 9;
				}
				if (num > 98f && num <= 106f && GetCardTypeCount(cardID = CardID.Queen) > 0)
				{
					cardID = CardID.Queen;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 10;
				}
			}
			CardDeck.Remove(item);
			return result;
		}

		public static int GetRandomCardPlayer_ReturnID(CardID MinimumCard)
		{
			new Random();
			List<string> cardDeck = CardDeck;
			string item = "";
			if (cardDeck.Count == 0)
			{
				ResetCardDeck();
			}
			int result = 0;
			CardID cardID = CardID.None;
			while (cardID == CardID.None)
			{
				Script.Wait(1);
				float num = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 120f);
				if (num <= 5f && MinimumCard >= CardID.Two && GetCardTypeCount(CardID.Two) > 0)
				{
					cardID = CardID.Two;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 0;
				}
				if (num > 5f && num <= 10f && MinimumCard >= CardID.Three && GetCardTypeCount(CardID.Three) > 0)
				{
					cardID = CardID.Three;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 1;
				}
				if (num > 10f && num <= 18f && MinimumCard >= CardID.Four && GetCardTypeCount(CardID.Four) > 0)
				{
					cardID = CardID.Four;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 2;
				}
				if (num > 18f && num <= 26f && MinimumCard >= CardID.Five && GetCardTypeCount(cardID = CardID.Five) > 0)
				{
					cardID = CardID.Five;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 3;
				}
				if (num > 26f && num <= 38f && MinimumCard >= CardID.Six && GetCardTypeCount(cardID = CardID.Six) > 0)
				{
					cardID = CardID.Six;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 4;
				}
				if (num > 38f && num <= 52f && MinimumCard >= CardID.Seven && GetCardTypeCount(cardID = CardID.Seven) > 0)
				{
					cardID = CardID.Seven;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 5;
				}
				if (num > 52f && num <= 72f && MinimumCard >= CardID.Eight && GetCardTypeCount(cardID = CardID.Eight) > 0)
				{
					cardID = CardID.Eight;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 6;
				}
				if (num > 72f && num <= 82f && MinimumCard >= CardID.Nine && GetCardTypeCount(cardID = CardID.Nine) > 0)
				{
					cardID = CardID.Nine;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 7;
				}
				if (num > 82f && num <= 90f && MinimumCard >= CardID.Ten && GetCardTypeCount(cardID = CardID.Ten) > 0)
				{
					cardID = CardID.Ten;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 8;
				}
				if (num > 90f && num <= 98f && MinimumCard >= CardID.Jack && GetCardTypeCount(cardID = CardID.Jack) > 0)
				{
					cardID = CardID.Jack;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 9;
				}
				if (num > 98f && num <= 106f && MinimumCard >= CardID.Queen && GetCardTypeCount(cardID = CardID.Queen) > 0)
				{
					cardID = CardID.Queen;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 10;
				}
				if (num > 112f && num <= 120f && MinimumCard >= CardID.King && GetCardTypeCount(cardID = CardID.King) > 0)
				{
					cardID = CardID.King;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 11;
				}
				if (num > 106f && num <= 112f && MinimumCard >= CardID.Ace && GetCardTypeCount(cardID = CardID.Ace) > 0)
				{
					cardID = CardID.Ace;
					item = GetCardOfType(CardVarient.Any, cardID);
					result = 12;
				}
			}
			CardDeck.Remove(item);
			return result;
		}

		public static int GetRandomCardPlayer_ReturnID_ForceEqualChance(CardID MinCard, CardID MaxCard)
		{
			new Random();
			List<string> cardDeck = CardDeck;
			string item = "";
			if (cardDeck.Count == 0)
			{
				ResetCardDeck();
			}
			int result = 0;
			CardID cardID = CardID.None;
			while (cardID == CardID.None)
			{
				Script.Wait(1);
				float num = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, (float)MinCard * 10f, 10f * (float)MaxCard);
				for (int i = 0; i < (int)MaxCard; i++)
				{
					float num2 = 10f * (float)i;
					float num3 = 10f * (float)(i + 1);
					if (num >= num2 && num2 <= num3)
					{
						CardID cardID2 = (CardID)i;
						if (GetCardTypeCount(cardID2) > 0)
						{
							cardID = cardID2;
							item = GetCardOfType(CardVarient.Any, cardID);
							result = (int)cardID2;
						}
					}
				}
			}
			CardDeck.Remove(item);
			return result;
		}

		public static CardVarient GetCardRandomVarient()
		{
			CardVarient result = CardVarient.Club;
			float num = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 100f);
			if (num <= 25f)
			{
				result = CardVarient.Club;
			}
			if (num >= 26f && num <= 50f)
			{
				result = CardVarient.Spade;
			}
			if (num >= 51f && num <= 75f)
			{
				result = CardVarient.Heart;
			}
			if (num >= 75f && num <= 100f)
			{
				result = CardVarient.Diamond;
			}
			return result;
		}

		public static string GetCardOfType(CardVarient Type, CardID ID)
		{
			if (Type == CardVarient.Any)
			{
				Type = GetCardRandomVarient();
			}
			List<string> list = new List<string>();
			if (PlayerDeck == 1 || PlayerDeck == 0 || PlayerDeck < 0 || PlayerDeck > 3)
			{
				if (Type == CardVarient.Club)
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
				}
				if (Type == CardVarient.Spade)
				{
					list.Add("vw_prop_cas_card_spd_02");
					list.Add("vw_prop_cas_card_spd_03");
					list.Add("vw_prop_cas_card_spd_04");
					list.Add("vw_prop_cas_card_spd_05");
					list.Add("vw_prop_cas_card_spd_06");
					list.Add("vw_prop_cas_card_spd_07");
					list.Add("vw_prop_cas_card_spd_08");
					list.Add("vw_prop_cas_card_spd_09");
					list.Add("vw_prop_cas_card_spd_10");
					list.Add("vw_prop_cas_card_spd_ace");
					list.Add("vw_prop_cas_card_spd_jack");
					list.Add("vw_prop_cas_card_spd_king");
					list.Add("vw_prop_cas_card_spd_queen");
				}
				if (Type == CardVarient.Heart)
				{
					list.Add("vw_prop_cas_card_hrt_02");
					list.Add("vw_prop_cas_card_hrt_03");
					list.Add("vw_prop_cas_card_hrt_04");
					list.Add("vw_prop_cas_card_hrt_05");
					list.Add("vw_prop_cas_card_hrt_06");
					list.Add("vw_prop_cas_card_hrt_07");
					list.Add("vw_prop_cas_card_hrt_08");
					list.Add("vw_prop_cas_card_hrt_09");
					list.Add("vw_prop_cas_card_hrt_10");
					list.Add("vw_prop_cas_card_hrt_ace");
					list.Add("vw_prop_cas_card_hrt_jack");
					list.Add("vw_prop_cas_card_hrt_king");
					list.Add("vw_prop_cas_card_hrt_queen");
				}
				if (Type == CardVarient.Diamond)
				{
					list.Add("vw_prop_cas_card_dia_02");
					list.Add("vw_prop_cas_card_dia_03");
					list.Add("vw_prop_cas_card_dia_04");
					list.Add("vw_prop_cas_card_dia_05");
					list.Add("vw_prop_cas_card_dia_06");
					list.Add("vw_prop_cas_card_dia_07");
					list.Add("vw_prop_cas_card_dia_08");
					list.Add("vw_prop_cas_card_dia_09");
					list.Add("vw_prop_cas_card_dia_10");
					list.Add("vw_prop_cas_card_dia_ace");
					list.Add("vw_prop_cas_card_dia_jack");
					list.Add("vw_prop_cas_card_dia_king");
					list.Add("vw_prop_cas_card_dia_queen");
				}
			}
			if (PlayerDeck == 2)
			{
				if (Type == CardVarient.Club)
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
				}
				if (Type == CardVarient.Diamond)
				{
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
				}
				if (Type == CardVarient.Heart)
				{
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
				}
				if (Type == CardVarient.Spade)
				{
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
			}
			return list[(int)ID];
		}

		public static string GetRandomCardDealer()
		{
			Random random = new Random();
			List<object> list = new List<object>();
			if (PlayerDeck == 1 || PlayerDeck == 0 || PlayerDeck < 0 || PlayerDeck > 3)
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
			if (PlayerDeck == 3)
			{
				if (TMD_Dealer)
				{
					if (PlayerDeck == 3)
					{
						if (TMD_UseNormalDeckCards)
						{
							if (TMD_Use02)
							{
								list.Add("vw_prop_cas_card_club_02");
							}
							if (TMD_Use03)
							{
								list.Add("vw_prop_cas_card_club_03");
							}
							if (TMD_Use04)
							{
								list.Add("vw_prop_cas_card_club_04");
							}
							if (TMD_Use05)
							{
								list.Add("vw_prop_cas_card_club_05");
							}
							if (TMD_Use06)
							{
								list.Add("vw_prop_cas_card_club_06");
							}
							if (TMD_Use07)
							{
								list.Add("vw_prop_cas_card_club_07");
							}
							if (TMD_Use08)
							{
								list.Add("vw_prop_cas_card_club_08");
							}
							if (TMD_Use09)
							{
								list.Add("vw_prop_cas_card_club_09");
							}
							if (TMD_Use10)
							{
								list.Add("vw_prop_cas_card_club_10");
							}
							if (TMD_UseAce)
							{
								list.Add("vw_prop_cas_card_club_ace");
							}
							if (TMD_UseJack)
							{
								list.Add("vw_prop_cas_card_club_jack");
							}
							if (TMD_UseKing)
							{
								list.Add("vw_prop_cas_card_club_king");
							}
							if (TMD_UseQueen)
							{
								list.Add("vw_prop_cas_card_club_queen");
							}
							if (TMD_UseQueen)
							{
								list.Add("vw_prop_cas_card_spd_queen");
							}
							if (TMD_UseKing)
							{
								list.Add("vw_prop_cas_card_spd_king");
							}
							if (TMD_UseJack)
							{
								list.Add("vw_prop_cas_card_spd_jack");
							}
							if (TMD_UseAce)
							{
								list.Add("vw_prop_cas_card_spd_ace");
							}
							if (TMD_Use10)
							{
								list.Add("vw_prop_cas_card_spd_10");
							}
							if (TMD_Use09)
							{
								list.Add("vw_prop_cas_card_spd_09");
							}
							if (TMD_Use08)
							{
								list.Add("vw_prop_cas_card_spd_08");
							}
							if (TMD_Use07)
							{
								list.Add("vw_prop_cas_card_spd_07");
							}
							if (TMD_Use06)
							{
								list.Add("vw_prop_cas_card_spd_06");
							}
							if (TMD_Use05)
							{
								list.Add("vw_prop_cas_card_spd_05");
							}
							if (TMD_Use04)
							{
								list.Add("vw_prop_cas_card_spd_04");
							}
							if (TMD_Use03)
							{
								list.Add("vw_prop_cas_card_spd_03");
							}
							if (TMD_Use02)
							{
								list.Add("vw_prop_cas_card_spd_02");
							}
							if (TMD_UseQueen)
							{
								list.Add("vw_prop_cas_card_hrt_queen");
							}
							if (TMD_UseKing)
							{
								list.Add("vw_prop_cas_card_hrt_king");
							}
							if (TMD_UseJack)
							{
								list.Add("vw_prop_cas_card_hrt_jack");
							}
							if (TMD_UseAce)
							{
								list.Add("vw_prop_cas_card_hrt_ace");
							}
							if (TMD_Use10)
							{
								list.Add("vw_prop_cas_card_hrt_10");
							}
							if (TMD_Use09)
							{
								list.Add("vw_prop_cas_card_hrt_09");
							}
							if (TMD_Use08)
							{
								list.Add("vw_prop_cas_card_hrt_08");
							}
							if (TMD_Use07)
							{
								list.Add("vw_prop_cas_card_hrt_07");
							}
							if (TMD_Use06)
							{
								list.Add("vw_prop_cas_card_hrt_06");
							}
							if (TMD_Use05)
							{
								list.Add("vw_prop_cas_card_hrt_05");
							}
							if (TMD_Use04)
							{
								list.Add("vw_prop_cas_card_hrt_04");
							}
							if (TMD_Use03)
							{
								list.Add("vw_prop_cas_card_hrt_03");
							}
							if (TMD_Use02)
							{
								list.Add("vw_prop_cas_card_hrt_02");
							}
							if (TMD_UseQueen)
							{
								list.Add("vw_prop_cas_card_dia_queen");
							}
							if (TMD_UseKing)
							{
								list.Add("vw_prop_cas_card_dia_king");
							}
							if (TMD_UseJack)
							{
								list.Add("vw_prop_cas_card_dia_jack");
							}
							if (TMD_UseAce)
							{
								list.Add("vw_prop_cas_card_dia_ace");
							}
							if (TMD_Use10)
							{
								list.Add("vw_prop_cas_card_dia_10");
							}
							if (TMD_Use09)
							{
								list.Add("vw_prop_cas_card_dia_09");
							}
							if (TMD_Use08)
							{
								list.Add("vw_prop_cas_card_dia_08");
							}
							if (TMD_Use07)
							{
								list.Add("vw_prop_cas_card_dia_07");
							}
							if (TMD_Use06)
							{
								list.Add("vw_prop_cas_card_dia_06");
							}
							if (TMD_Use05)
							{
								list.Add("vw_prop_cas_card_dia_05");
							}
							if (TMD_Use04)
							{
								list.Add("vw_prop_cas_card_dia_04");
							}
							if (TMD_Use03)
							{
								list.Add("vw_prop_cas_card_dia_03");
							}
							if (TMD_Use02)
							{
								list.Add("vw_prop_cas_card_dia_02");
							}
						}
						if (TMD_UseEliteDeckCards)
						{
							if (TMD_Use02)
							{
								list.Add("vw_prop_vw_club_char_02a");
							}
							if (TMD_Use03)
							{
								list.Add("vw_prop_vw_club_char_03a");
							}
							if (TMD_Use04)
							{
								list.Add("vw_prop_vw_club_char_04a");
							}
							if (TMD_Use05)
							{
								list.Add("vw_prop_vw_club_char_05a");
							}
							if (TMD_Use06)
							{
								list.Add("vw_prop_vw_club_char_06a");
							}
							if (TMD_Use07)
							{
								list.Add("vw_prop_vw_club_char_07a");
							}
							if (TMD_Use08)
							{
								list.Add("vw_prop_vw_club_char_08a");
							}
							if (TMD_Use09)
							{
								list.Add("vw_prop_vw_club_char_09a");
							}
							if (TMD_Use10)
							{
								list.Add("vw_prop_vw_club_char_10a");
							}
							if (TMD_UseAce)
							{
								list.Add("vw_prop_vw_club_char_a_a");
							}
							if (TMD_UseJack)
							{
								list.Add("vw_prop_vw_club_char_j_a");
							}
							if (TMD_UseKing)
							{
								list.Add("vw_prop_vw_club_char_k_a");
							}
							if (TMD_UseQueen)
							{
								list.Add("vw_prop_vw_club_char_q_a");
							}
							if (TMD_Use02)
							{
								list.Add("vw_prop_vw_dia_char_02a");
							}
							if (TMD_Use03)
							{
								list.Add("vw_prop_vw_dia_char_03a");
							}
							if (TMD_Use04)
							{
								list.Add("vw_prop_vw_dia_char_04a");
							}
							if (TMD_Use05)
							{
								list.Add("vw_prop_vw_dia_char_05a");
							}
							if (TMD_Use06)
							{
								list.Add("vw_prop_vw_dia_char_06a");
							}
							if (TMD_Use07)
							{
								list.Add("vw_prop_vw_dia_char_07a");
							}
							if (TMD_Use08)
							{
								list.Add("vw_prop_vw_dia_char_08a");
							}
							if (TMD_Use09)
							{
								list.Add("vw_prop_vw_dia_char_09a");
							}
							if (TMD_Use10)
							{
								list.Add("vw_prop_vw_dia_char_10a");
							}
							if (TMD_UseAce)
							{
								list.Add("vw_prop_vw_dia_char_a_a");
							}
							if (TMD_UseJack)
							{
								list.Add("vw_prop_vw_dia_char_j_a");
							}
							if (TMD_UseKing)
							{
								list.Add("vw_prop_vw_dia_char_k_a");
							}
							if (TMD_UseQueen)
							{
								list.Add("vw_prop_vw_dia_char_q_a");
							}
							if (TMD_Use02)
							{
								list.Add("vw_prop_vw_hrt_char_02a");
							}
							if (TMD_Use03)
							{
								list.Add("vw_prop_vw_hrt_char_03a");
							}
							if (TMD_Use04)
							{
								list.Add("vw_prop_vw_hrt_char_04a");
							}
							if (TMD_Use05)
							{
								list.Add("vw_prop_vw_hrt_char_05a");
							}
							if (TMD_Use06)
							{
								list.Add("vw_prop_vw_hrt_char_06a");
							}
							if (TMD_Use07)
							{
								list.Add("vw_prop_vw_hrt_char_07a");
							}
							if (TMD_Use08)
							{
								list.Add("vw_prop_vw_hrt_char_08a");
							}
							if (TMD_Use09)
							{
								list.Add("vw_prop_vw_hrt_char_09a");
							}
							if (TMD_Use10)
							{
								list.Add("vw_prop_vw_hrt_char_10a");
							}
							if (TMD_UseAce)
							{
								list.Add("vw_prop_vw_hrt_char_a_a");
							}
							if (TMD_UseJack)
							{
								list.Add("vw_prop_vw_hrt_char_j_a");
							}
							if (TMD_UseKing)
							{
								list.Add("vw_prop_vw_hrt_char_k_a");
							}
							if (TMD_UseQueen)
							{
								list.Add("vw_prop_vw_hrt_char_q_a");
							}
							if (TMD_Use02)
							{
								list.Add("vw_prop_vw_spd_char_02a");
							}
							if (TMD_Use03)
							{
								list.Add("vw_prop_vw_spd_char_03a");
							}
							if (TMD_Use04)
							{
								list.Add("vw_prop_vw_spd_char_04a");
							}
							if (TMD_Use05)
							{
								list.Add("vw_prop_vw_spd_char_05a");
							}
							if (TMD_Use06)
							{
								list.Add("vw_prop_vw_spd_char_06a");
							}
							if (TMD_Use07)
							{
								list.Add("vw_prop_vw_spd_char_07a");
							}
							if (TMD_Use08)
							{
								list.Add("vw_prop_vw_spd_char_08a");
							}
							if (TMD_Use09)
							{
								list.Add("vw_prop_vw_spd_char_09a");
							}
							if (TMD_Use10)
							{
								list.Add("vw_prop_vw_spd_char_10a");
							}
							if (TMD_UseAce)
							{
								list.Add("vw_prop_vw_spd_char_a_a");
							}
							if (TMD_UseJack)
							{
								list.Add("vw_prop_vw_spd_char_j_a");
							}
							if (TMD_UseKing)
							{
								list.Add("vw_prop_vw_spd_char_k_a");
							}
							if (TMD_UseQueen)
							{
								list.Add("vw_prop_vw_spd_char_q_a");
							}
						}
					}
				}
				else
				{
					if (TMD_UseNormalDeckCards)
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
					if (TMD_UseEliteDeckCards)
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
				}
			}
			if (list.Count == 0)
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
			int index = random.Next(0, list.Count - 1);
			return (dynamic)list[index];
		}

		public static string GetRandomCardDealer_BlackJack()
		{
			Random random = new Random();
			List<object> list = new List<object>();
			int num = random.Next(0, 52);
			if (num < 16)
			{
				if (PlayerDeck == 1 || PlayerDeck == 0)
				{
					list.Add("vw_prop_cas_card_club_10");
					list.Add("vw_prop_cas_card_club_jack");
					list.Add("vw_prop_cas_card_club_king");
					list.Add("vw_prop_cas_card_club_queen");
					list.Add("vw_prop_cas_card_spd_queen");
					list.Add("vw_prop_cas_card_spd_king");
					list.Add("vw_prop_cas_card_spd_jack");
					list.Add("vw_prop_cas_card_spd_10");
					list.Add("vw_prop_cas_card_hrt_queen");
					list.Add("vw_prop_cas_card_hrt_king");
					list.Add("vw_prop_cas_card_hrt_jack");
					list.Add("vw_prop_cas_card_hrt_10");
					list.Add("vw_prop_cas_card_dia_queen");
					list.Add("vw_prop_cas_card_dia_king");
					list.Add("vw_prop_cas_card_dia_jack");
					list.Add("vw_prop_cas_card_dia_10");
				}
				if (PlayerDeck == 2)
				{
					list.Add("vw_prop_vw_club_char_10a");
					list.Add("vw_prop_vw_club_char_j_a");
					list.Add("vw_prop_vw_club_char_k_a");
					list.Add("vw_prop_vw_club_char_q_a");
					list.Add("vw_prop_vw_dia_char_10a");
					list.Add("vw_prop_vw_dia_char_j_a");
					list.Add("vw_prop_vw_dia_char_k_a");
					list.Add("vw_prop_vw_dia_char_q_a");
					list.Add("vw_prop_vw_hrt_char_10a");
					list.Add("vw_prop_vw_hrt_char_j_a");
					list.Add("vw_prop_vw_hrt_char_k_a");
					list.Add("vw_prop_vw_hrt_char_q_a");
					list.Add("vw_prop_vw_spd_char_10a");
					list.Add("vw_prop_vw_spd_char_j_a");
					list.Add("vw_prop_vw_spd_char_k_a");
					list.Add("vw_prop_vw_spd_char_q_a");
				}
				if (PlayerDeck == 3)
				{
					if (TMD_Dealer)
					{
						if (PlayerDeck == 3)
						{
							if (TMD_UseNormalDeckCards)
							{
								if (TMD_Use02)
								{
									list.Add("vw_prop_cas_card_club_02");
								}
								if (TMD_Use03)
								{
									list.Add("vw_prop_cas_card_club_03");
								}
								if (TMD_Use04)
								{
									list.Add("vw_prop_cas_card_club_04");
								}
								if (TMD_Use05)
								{
									list.Add("vw_prop_cas_card_club_05");
								}
								if (TMD_Use06)
								{
									list.Add("vw_prop_cas_card_club_06");
								}
								if (TMD_Use07)
								{
									list.Add("vw_prop_cas_card_club_07");
								}
								if (TMD_Use08)
								{
									list.Add("vw_prop_cas_card_club_08");
								}
								if (TMD_Use09)
								{
									list.Add("vw_prop_cas_card_club_09");
								}
								if (TMD_Use10)
								{
									list.Add("vw_prop_cas_card_club_10");
								}
								if (TMD_UseAce)
								{
									list.Add("vw_prop_cas_card_club_ace");
								}
								if (TMD_UseJack)
								{
									list.Add("vw_prop_cas_card_club_jack");
								}
								if (TMD_UseKing)
								{
									list.Add("vw_prop_cas_card_club_king");
								}
								if (TMD_UseQueen)
								{
									list.Add("vw_prop_cas_card_club_queen");
								}
								if (TMD_UseQueen)
								{
									list.Add("vw_prop_cas_card_spd_queen");
								}
								if (TMD_UseKing)
								{
									list.Add("vw_prop_cas_card_spd_king");
								}
								if (TMD_UseJack)
								{
									list.Add("vw_prop_cas_card_spd_jack");
								}
								if (TMD_UseAce)
								{
									list.Add("vw_prop_cas_card_spd_ace");
								}
								if (TMD_Use10)
								{
									list.Add("vw_prop_cas_card_spd_10");
								}
								if (TMD_Use09)
								{
									list.Add("vw_prop_cas_card_spd_09");
								}
								if (TMD_Use08)
								{
									list.Add("vw_prop_cas_card_spd_08");
								}
								if (TMD_Use07)
								{
									list.Add("vw_prop_cas_card_spd_07");
								}
								if (TMD_Use06)
								{
									list.Add("vw_prop_cas_card_spd_06");
								}
								if (TMD_Use05)
								{
									list.Add("vw_prop_cas_card_spd_05");
								}
								if (TMD_Use04)
								{
									list.Add("vw_prop_cas_card_spd_04");
								}
								if (TMD_Use03)
								{
									list.Add("vw_prop_cas_card_spd_03");
								}
								if (TMD_Use02)
								{
									list.Add("vw_prop_cas_card_spd_02");
								}
								if (TMD_UseQueen)
								{
									list.Add("vw_prop_cas_card_hrt_queen");
								}
								if (TMD_UseKing)
								{
									list.Add("vw_prop_cas_card_hrt_king");
								}
								if (TMD_UseJack)
								{
									list.Add("vw_prop_cas_card_hrt_jack");
								}
								if (TMD_UseAce)
								{
									list.Add("vw_prop_cas_card_hrt_ace");
								}
								if (TMD_Use10)
								{
									list.Add("vw_prop_cas_card_hrt_10");
								}
								if (TMD_Use09)
								{
									list.Add("vw_prop_cas_card_hrt_09");
								}
								if (TMD_Use08)
								{
									list.Add("vw_prop_cas_card_hrt_08");
								}
								if (TMD_Use07)
								{
									list.Add("vw_prop_cas_card_hrt_07");
								}
								if (TMD_Use06)
								{
									list.Add("vw_prop_cas_card_hrt_06");
								}
								if (TMD_Use05)
								{
									list.Add("vw_prop_cas_card_hrt_05");
								}
								if (TMD_Use04)
								{
									list.Add("vw_prop_cas_card_hrt_04");
								}
								if (TMD_Use03)
								{
									list.Add("vw_prop_cas_card_hrt_03");
								}
								if (TMD_Use02)
								{
									list.Add("vw_prop_cas_card_hrt_02");
								}
								if (TMD_UseQueen)
								{
									list.Add("vw_prop_cas_card_dia_queen");
								}
								if (TMD_UseKing)
								{
									list.Add("vw_prop_cas_card_dia_king");
								}
								if (TMD_UseJack)
								{
									list.Add("vw_prop_cas_card_dia_jack");
								}
								if (TMD_UseAce)
								{
									list.Add("vw_prop_cas_card_dia_ace");
								}
								if (TMD_Use10)
								{
									list.Add("vw_prop_cas_card_dia_10");
								}
								if (TMD_Use09)
								{
									list.Add("vw_prop_cas_card_dia_09");
								}
								if (TMD_Use08)
								{
									list.Add("vw_prop_cas_card_dia_08");
								}
								if (TMD_Use07)
								{
									list.Add("vw_prop_cas_card_dia_07");
								}
								if (TMD_Use06)
								{
									list.Add("vw_prop_cas_card_dia_06");
								}
								if (TMD_Use05)
								{
									list.Add("vw_prop_cas_card_dia_05");
								}
								if (TMD_Use04)
								{
									list.Add("vw_prop_cas_card_dia_04");
								}
								if (TMD_Use03)
								{
									list.Add("vw_prop_cas_card_dia_03");
								}
								if (TMD_Use02)
								{
									list.Add("vw_prop_cas_card_dia_02");
								}
							}
							if (TMD_UseEliteDeckCards)
							{
								if (TMD_Use02)
								{
									list.Add("vw_prop_vw_club_char_02a");
								}
								if (TMD_Use03)
								{
									list.Add("vw_prop_vw_club_char_03a");
								}
								if (TMD_Use04)
								{
									list.Add("vw_prop_vw_club_char_04a");
								}
								if (TMD_Use05)
								{
									list.Add("vw_prop_vw_club_char_05a");
								}
								if (TMD_Use06)
								{
									list.Add("vw_prop_vw_club_char_06a");
								}
								if (TMD_Use07)
								{
									list.Add("vw_prop_vw_club_char_07a");
								}
								if (TMD_Use08)
								{
									list.Add("vw_prop_vw_club_char_08a");
								}
								if (TMD_Use09)
								{
									list.Add("vw_prop_vw_club_char_09a");
								}
								if (TMD_Use10)
								{
									list.Add("vw_prop_vw_club_char_10a");
								}
								if (TMD_UseAce)
								{
									list.Add("vw_prop_vw_club_char_a_a");
								}
								if (TMD_UseJack)
								{
									list.Add("vw_prop_vw_club_char_j_a");
								}
								if (TMD_UseKing)
								{
									list.Add("vw_prop_vw_club_char_k_a");
								}
								if (TMD_UseQueen)
								{
									list.Add("vw_prop_vw_club_char_q_a");
								}
								if (TMD_Use02)
								{
									list.Add("vw_prop_vw_dia_char_02a");
								}
								if (TMD_Use03)
								{
									list.Add("vw_prop_vw_dia_char_03a");
								}
								if (TMD_Use04)
								{
									list.Add("vw_prop_vw_dia_char_04a");
								}
								if (TMD_Use05)
								{
									list.Add("vw_prop_vw_dia_char_05a");
								}
								if (TMD_Use06)
								{
									list.Add("vw_prop_vw_dia_char_06a");
								}
								if (TMD_Use07)
								{
									list.Add("vw_prop_vw_dia_char_07a");
								}
								if (TMD_Use08)
								{
									list.Add("vw_prop_vw_dia_char_08a");
								}
								if (TMD_Use09)
								{
									list.Add("vw_prop_vw_dia_char_09a");
								}
								if (TMD_Use10)
								{
									list.Add("vw_prop_vw_dia_char_10a");
								}
								if (TMD_UseAce)
								{
									list.Add("vw_prop_vw_dia_char_a_a");
								}
								if (TMD_UseJack)
								{
									list.Add("vw_prop_vw_dia_char_j_a");
								}
								if (TMD_UseKing)
								{
									list.Add("vw_prop_vw_dia_char_k_a");
								}
								if (TMD_UseQueen)
								{
									list.Add("vw_prop_vw_dia_char_q_a");
								}
								if (TMD_Use02)
								{
									list.Add("vw_prop_vw_hrt_char_02a");
								}
								if (TMD_Use03)
								{
									list.Add("vw_prop_vw_hrt_char_03a");
								}
								if (TMD_Use04)
								{
									list.Add("vw_prop_vw_hrt_char_04a");
								}
								if (TMD_Use05)
								{
									list.Add("vw_prop_vw_hrt_char_05a");
								}
								if (TMD_Use06)
								{
									list.Add("vw_prop_vw_hrt_char_06a");
								}
								if (TMD_Use07)
								{
									list.Add("vw_prop_vw_hrt_char_07a");
								}
								if (TMD_Use08)
								{
									list.Add("vw_prop_vw_hrt_char_08a");
								}
								if (TMD_Use09)
								{
									list.Add("vw_prop_vw_hrt_char_09a");
								}
								if (TMD_Use10)
								{
									list.Add("vw_prop_vw_hrt_char_10a");
								}
								if (TMD_UseAce)
								{
									list.Add("vw_prop_vw_hrt_char_a_a");
								}
								if (TMD_UseJack)
								{
									list.Add("vw_prop_vw_hrt_char_j_a");
								}
								if (TMD_UseKing)
								{
									list.Add("vw_prop_vw_hrt_char_k_a");
								}
								if (TMD_UseQueen)
								{
									list.Add("vw_prop_vw_hrt_char_q_a");
								}
								if (TMD_Use02)
								{
									list.Add("vw_prop_vw_spd_char_02a");
								}
								if (TMD_Use03)
								{
									list.Add("vw_prop_vw_spd_char_03a");
								}
								if (TMD_Use04)
								{
									list.Add("vw_prop_vw_spd_char_04a");
								}
								if (TMD_Use05)
								{
									list.Add("vw_prop_vw_spd_char_05a");
								}
								if (TMD_Use06)
								{
									list.Add("vw_prop_vw_spd_char_06a");
								}
								if (TMD_Use07)
								{
									list.Add("vw_prop_vw_spd_char_07a");
								}
								if (TMD_Use08)
								{
									list.Add("vw_prop_vw_spd_char_08a");
								}
								if (TMD_Use09)
								{
									list.Add("vw_prop_vw_spd_char_09a");
								}
								if (TMD_Use10)
								{
									list.Add("vw_prop_vw_spd_char_10a");
								}
								if (TMD_UseAce)
								{
									list.Add("vw_prop_vw_spd_char_a_a");
								}
								if (TMD_UseJack)
								{
									list.Add("vw_prop_vw_spd_char_j_a");
								}
								if (TMD_UseKing)
								{
									list.Add("vw_prop_vw_spd_char_k_a");
								}
								if (TMD_UseQueen)
								{
									list.Add("vw_prop_vw_spd_char_q_a");
								}
							}
						}
					}
					else
					{
						if (TMD_UseNormalDeckCards)
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
						if (TMD_UseEliteDeckCards)
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
					}
				}
			}
			if (num > 16)
			{
				if (PlayerDeck == 1 || PlayerDeck == 0)
				{
					list.Add("vw_prop_cas_card_club_ace");
					list.Add("vw_prop_cas_card_club_02");
					list.Add("vw_prop_cas_card_club_03");
					list.Add("vw_prop_cas_card_club_04");
					list.Add("vw_prop_cas_card_club_05");
					list.Add("vw_prop_cas_card_club_06");
					list.Add("vw_prop_cas_card_club_07");
					list.Add("vw_prop_cas_card_club_08");
					list.Add("vw_prop_cas_card_club_09");
					list.Add("vw_prop_cas_card_spd_ace");
					list.Add("vw_prop_cas_card_spd_09");
					list.Add("vw_prop_cas_card_spd_08");
					list.Add("vw_prop_cas_card_spd_07");
					list.Add("vw_prop_cas_card_spd_06");
					list.Add("vw_prop_cas_card_spd_05");
					list.Add("vw_prop_cas_card_spd_04");
					list.Add("vw_prop_cas_card_spd_03");
					list.Add("vw_prop_cas_card_spd_02");
					list.Add("vw_prop_cas_card_hrt_ace");
					list.Add("vw_prop_cas_card_hrt_09");
					list.Add("vw_prop_cas_card_hrt_08");
					list.Add("vw_prop_cas_card_hrt_07");
					list.Add("vw_prop_cas_card_hrt_06");
					list.Add("vw_prop_cas_card_hrt_05");
					list.Add("vw_prop_cas_card_hrt_04");
					list.Add("vw_prop_cas_card_hrt_03");
					list.Add("vw_prop_cas_card_hrt_02");
					list.Add("vw_prop_cas_card_dia_ace");
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
					list.Add("vw_prop_vw_club_char_a_a");
					list.Add("vw_prop_vw_dia_char_02a");
					list.Add("vw_prop_vw_dia_char_03a");
					list.Add("vw_prop_vw_dia_char_04a");
					list.Add("vw_prop_vw_dia_char_05a");
					list.Add("vw_prop_vw_dia_char_06a");
					list.Add("vw_prop_vw_dia_char_07a");
					list.Add("vw_prop_vw_dia_char_08a");
					list.Add("vw_prop_vw_dia_char_09a");
					list.Add("vw_prop_vw_dia_char_a_a");
					list.Add("vw_prop_vw_hrt_char_02a");
					list.Add("vw_prop_vw_hrt_char_03a");
					list.Add("vw_prop_vw_hrt_char_04a");
					list.Add("vw_prop_vw_hrt_char_05a");
					list.Add("vw_prop_vw_hrt_char_06a");
					list.Add("vw_prop_vw_hrt_char_07a");
					list.Add("vw_prop_vw_hrt_char_08a");
					list.Add("vw_prop_vw_hrt_char_09a");
					list.Add("vw_prop_vw_hrt_char_a_a");
					list.Add("vw_prop_vw_spd_char_02a");
					list.Add("vw_prop_vw_spd_char_03a");
					list.Add("vw_prop_vw_spd_char_04a");
					list.Add("vw_prop_vw_spd_char_05a");
					list.Add("vw_prop_vw_spd_char_06a");
					list.Add("vw_prop_vw_spd_char_07a");
					list.Add("vw_prop_vw_spd_char_08a");
					list.Add("vw_prop_vw_spd_char_09a");
					list.Add("vw_prop_vw_spd_char_a_a");
				}
				if (PlayerDeck == 3)
				{
					if (TMD_Dealer)
					{
						if (PlayerDeck == 3)
						{
							if (TMD_UseNormalDeckCards)
							{
								if (TMD_Use02)
								{
									list.Add("vw_prop_cas_card_club_02");
								}
								if (TMD_Use03)
								{
									list.Add("vw_prop_cas_card_club_03");
								}
								if (TMD_Use04)
								{
									list.Add("vw_prop_cas_card_club_04");
								}
								if (TMD_Use05)
								{
									list.Add("vw_prop_cas_card_club_05");
								}
								if (TMD_Use06)
								{
									list.Add("vw_prop_cas_card_club_06");
								}
								if (TMD_Use07)
								{
									list.Add("vw_prop_cas_card_club_07");
								}
								if (TMD_Use08)
								{
									list.Add("vw_prop_cas_card_club_08");
								}
								if (TMD_Use09)
								{
									list.Add("vw_prop_cas_card_club_09");
								}
								if (TMD_Use10)
								{
									list.Add("vw_prop_cas_card_club_10");
								}
								if (TMD_UseAce)
								{
									list.Add("vw_prop_cas_card_club_ace");
								}
								if (TMD_UseJack)
								{
									list.Add("vw_prop_cas_card_club_jack");
								}
								if (TMD_UseKing)
								{
									list.Add("vw_prop_cas_card_club_king");
								}
								if (TMD_UseQueen)
								{
									list.Add("vw_prop_cas_card_club_queen");
								}
								if (TMD_UseQueen)
								{
									list.Add("vw_prop_cas_card_spd_queen");
								}
								if (TMD_UseKing)
								{
									list.Add("vw_prop_cas_card_spd_king");
								}
								if (TMD_UseJack)
								{
									list.Add("vw_prop_cas_card_spd_jack");
								}
								if (TMD_UseAce)
								{
									list.Add("vw_prop_cas_card_spd_ace");
								}
								if (TMD_Use10)
								{
									list.Add("vw_prop_cas_card_spd_10");
								}
								if (TMD_Use09)
								{
									list.Add("vw_prop_cas_card_spd_09");
								}
								if (TMD_Use08)
								{
									list.Add("vw_prop_cas_card_spd_08");
								}
								if (TMD_Use07)
								{
									list.Add("vw_prop_cas_card_spd_07");
								}
								if (TMD_Use06)
								{
									list.Add("vw_prop_cas_card_spd_06");
								}
								if (TMD_Use05)
								{
									list.Add("vw_prop_cas_card_spd_05");
								}
								if (TMD_Use04)
								{
									list.Add("vw_prop_cas_card_spd_04");
								}
								if (TMD_Use03)
								{
									list.Add("vw_prop_cas_card_spd_03");
								}
								if (TMD_Use02)
								{
									list.Add("vw_prop_cas_card_spd_02");
								}
								if (TMD_UseQueen)
								{
									list.Add("vw_prop_cas_card_hrt_queen");
								}
								if (TMD_UseKing)
								{
									list.Add("vw_prop_cas_card_hrt_king");
								}
								if (TMD_UseJack)
								{
									list.Add("vw_prop_cas_card_hrt_jack");
								}
								if (TMD_UseAce)
								{
									list.Add("vw_prop_cas_card_hrt_ace");
								}
								if (TMD_Use10)
								{
									list.Add("vw_prop_cas_card_hrt_10");
								}
								if (TMD_Use09)
								{
									list.Add("vw_prop_cas_card_hrt_09");
								}
								if (TMD_Use08)
								{
									list.Add("vw_prop_cas_card_hrt_08");
								}
								if (TMD_Use07)
								{
									list.Add("vw_prop_cas_card_hrt_07");
								}
								if (TMD_Use06)
								{
									list.Add("vw_prop_cas_card_hrt_06");
								}
								if (TMD_Use05)
								{
									list.Add("vw_prop_cas_card_hrt_05");
								}
								if (TMD_Use04)
								{
									list.Add("vw_prop_cas_card_hrt_04");
								}
								if (TMD_Use03)
								{
									list.Add("vw_prop_cas_card_hrt_03");
								}
								if (TMD_Use02)
								{
									list.Add("vw_prop_cas_card_hrt_02");
								}
								if (TMD_UseQueen)
								{
									list.Add("vw_prop_cas_card_dia_queen");
								}
								if (TMD_UseKing)
								{
									list.Add("vw_prop_cas_card_dia_king");
								}
								if (TMD_UseJack)
								{
									list.Add("vw_prop_cas_card_dia_jack");
								}
								if (TMD_UseAce)
								{
									list.Add("vw_prop_cas_card_dia_ace");
								}
								if (TMD_Use10)
								{
									list.Add("vw_prop_cas_card_dia_10");
								}
								if (TMD_Use09)
								{
									list.Add("vw_prop_cas_card_dia_09");
								}
								if (TMD_Use08)
								{
									list.Add("vw_prop_cas_card_dia_08");
								}
								if (TMD_Use07)
								{
									list.Add("vw_prop_cas_card_dia_07");
								}
								if (TMD_Use06)
								{
									list.Add("vw_prop_cas_card_dia_06");
								}
								if (TMD_Use05)
								{
									list.Add("vw_prop_cas_card_dia_05");
								}
								if (TMD_Use04)
								{
									list.Add("vw_prop_cas_card_dia_04");
								}
								if (TMD_Use03)
								{
									list.Add("vw_prop_cas_card_dia_03");
								}
								if (TMD_Use02)
								{
									list.Add("vw_prop_cas_card_dia_02");
								}
							}
							if (TMD_UseEliteDeckCards)
							{
								if (TMD_Use02)
								{
									list.Add("vw_prop_vw_club_char_02a");
								}
								if (TMD_Use03)
								{
									list.Add("vw_prop_vw_club_char_03a");
								}
								if (TMD_Use04)
								{
									list.Add("vw_prop_vw_club_char_04a");
								}
								if (TMD_Use05)
								{
									list.Add("vw_prop_vw_club_char_05a");
								}
								if (TMD_Use06)
								{
									list.Add("vw_prop_vw_club_char_06a");
								}
								if (TMD_Use07)
								{
									list.Add("vw_prop_vw_club_char_07a");
								}
								if (TMD_Use08)
								{
									list.Add("vw_prop_vw_club_char_08a");
								}
								if (TMD_Use09)
								{
									list.Add("vw_prop_vw_club_char_09a");
								}
								if (TMD_Use10)
								{
									list.Add("vw_prop_vw_club_char_10a");
								}
								if (TMD_UseAce)
								{
									list.Add("vw_prop_vw_club_char_a_a");
								}
								if (TMD_UseJack)
								{
									list.Add("vw_prop_vw_club_char_j_a");
								}
								if (TMD_UseKing)
								{
									list.Add("vw_prop_vw_club_char_k_a");
								}
								if (TMD_UseQueen)
								{
									list.Add("vw_prop_vw_club_char_q_a");
								}
								if (TMD_Use02)
								{
									list.Add("vw_prop_vw_dia_char_02a");
								}
								if (TMD_Use03)
								{
									list.Add("vw_prop_vw_dia_char_03a");
								}
								if (TMD_Use04)
								{
									list.Add("vw_prop_vw_dia_char_04a");
								}
								if (TMD_Use05)
								{
									list.Add("vw_prop_vw_dia_char_05a");
								}
								if (TMD_Use06)
								{
									list.Add("vw_prop_vw_dia_char_06a");
								}
								if (TMD_Use07)
								{
									list.Add("vw_prop_vw_dia_char_07a");
								}
								if (TMD_Use08)
								{
									list.Add("vw_prop_vw_dia_char_08a");
								}
								if (TMD_Use09)
								{
									list.Add("vw_prop_vw_dia_char_09a");
								}
								if (TMD_Use10)
								{
									list.Add("vw_prop_vw_dia_char_10a");
								}
								if (TMD_UseAce)
								{
									list.Add("vw_prop_vw_dia_char_a_a");
								}
								if (TMD_UseJack)
								{
									list.Add("vw_prop_vw_dia_char_j_a");
								}
								if (TMD_UseKing)
								{
									list.Add("vw_prop_vw_dia_char_k_a");
								}
								if (TMD_UseQueen)
								{
									list.Add("vw_prop_vw_dia_char_q_a");
								}
								if (TMD_Use02)
								{
									list.Add("vw_prop_vw_hrt_char_02a");
								}
								if (TMD_Use03)
								{
									list.Add("vw_prop_vw_hrt_char_03a");
								}
								if (TMD_Use04)
								{
									list.Add("vw_prop_vw_hrt_char_04a");
								}
								if (TMD_Use05)
								{
									list.Add("vw_prop_vw_hrt_char_05a");
								}
								if (TMD_Use06)
								{
									list.Add("vw_prop_vw_hrt_char_06a");
								}
								if (TMD_Use07)
								{
									list.Add("vw_prop_vw_hrt_char_07a");
								}
								if (TMD_Use08)
								{
									list.Add("vw_prop_vw_hrt_char_08a");
								}
								if (TMD_Use09)
								{
									list.Add("vw_prop_vw_hrt_char_09a");
								}
								if (TMD_Use10)
								{
									list.Add("vw_prop_vw_hrt_char_10a");
								}
								if (TMD_UseAce)
								{
									list.Add("vw_prop_vw_hrt_char_a_a");
								}
								if (TMD_UseJack)
								{
									list.Add("vw_prop_vw_hrt_char_j_a");
								}
								if (TMD_UseKing)
								{
									list.Add("vw_prop_vw_hrt_char_k_a");
								}
								if (TMD_UseQueen)
								{
									list.Add("vw_prop_vw_hrt_char_q_a");
								}
								if (TMD_Use02)
								{
									list.Add("vw_prop_vw_spd_char_02a");
								}
								if (TMD_Use03)
								{
									list.Add("vw_prop_vw_spd_char_03a");
								}
								if (TMD_Use04)
								{
									list.Add("vw_prop_vw_spd_char_04a");
								}
								if (TMD_Use05)
								{
									list.Add("vw_prop_vw_spd_char_05a");
								}
								if (TMD_Use06)
								{
									list.Add("vw_prop_vw_spd_char_06a");
								}
								if (TMD_Use07)
								{
									list.Add("vw_prop_vw_spd_char_07a");
								}
								if (TMD_Use08)
								{
									list.Add("vw_prop_vw_spd_char_08a");
								}
								if (TMD_Use09)
								{
									list.Add("vw_prop_vw_spd_char_09a");
								}
								if (TMD_Use10)
								{
									list.Add("vw_prop_vw_spd_char_10a");
								}
								if (TMD_UseAce)
								{
									list.Add("vw_prop_vw_spd_char_a_a");
								}
								if (TMD_UseJack)
								{
									list.Add("vw_prop_vw_spd_char_j_a");
								}
								if (TMD_UseKing)
								{
									list.Add("vw_prop_vw_spd_char_k_a");
								}
								if (TMD_UseQueen)
								{
									list.Add("vw_prop_vw_spd_char_q_a");
								}
							}
						}
					}
					else
					{
						if (TMD_UseNormalDeckCards)
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
						if (TMD_UseEliteDeckCards)
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
					}
				}
			}
			if (list.Count == 0)
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
			int index = random.Next(0, list.Count - 1);
			return (dynamic)list[index];
		}

		public static void CreateBlackJackPresets(List<BlackjackCards> DealerPresetCars)
		{
			for (int i = 0; i < 5; i++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._10_Ten), ReturnRandomCardType(CardType._4_Four), ReturnRandomCardType(CardType.King)));
			}
			for (int j = 0; j < 5; j++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._10_Ten), ReturnRandomCardType(CardType._4_Four), ReturnRandomCardType(CardType._9_Nine)));
			}
			for (int k = 0; k < 5; k++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._7_Seven), ReturnRandomCardType(CardType._10_Ten), ReturnRandomCardType(CardType.King)));
			}
			for (int l = 0; l < 5; l++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType.Queen), ReturnRandomCardType(CardType._7_Seven), ReturnRandomCardType(CardType.King)));
			}
			for (int m = 0; m < 5; m++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._2_Two), ReturnRandomCardType(CardType._10_Ten), ReturnRandomCardType(CardType.King)));
			}
			for (int n = 0; n < 5; n++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._3_Three), ReturnRandomCardType(CardType._9_Nine), ReturnRandomCardType(CardType._10_Ten)));
			}
			for (int num = 0; num < 5; num++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._10_Ten), ReturnRandomCardType(CardType.Ace), ReturnRandomCardType(CardType.King)));
			}
			for (int num2 = 0; num2 < 5; num2++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._10_Ten), ReturnRandomCardType(CardType.Queen), ReturnRandomCardType(CardType.Ace)));
			}
			for (int num3 = 0; num3 < 5; num3++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType.King), ReturnRandomCardType(CardType.Ace), ReturnRandomCardType(CardType.Jack)));
			}
			for (int num4 = 0; num4 < 5; num4++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._10_Ten), ReturnRandomCardType(CardType._5_Five), ReturnRandomCardType(CardType._6_Six)));
			}
			for (int num5 = 0; num5 < 5; num5++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._8_Eight), ReturnRandomCardType(CardType._7_Seven), ReturnRandomCardType(CardType._6_Six)));
			}
			for (int num6 = 0; num6 < 7; num6++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._10_Ten), ReturnRandomCardType(CardType._9_Nine), ReturnRandomCardType(CardType.Ace)));
			}
			for (int num7 = 0; num7 < 7; num7++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._5_Five), ReturnRandomCardType(CardType._4_Four), ReturnRandomCardType(CardType._6_Six)));
			}
			for (int num8 = 0; num8 < 7; num8++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._5_Five), ReturnRandomCardType(CardType._4_Four), ReturnRandomCardType(CardType._6_Six)));
			}
			for (int num9 = 0; num9 < 7; num9++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._4_Four), ReturnRandomCardType(CardType._7_Seven), ReturnRandomCardType(CardType._9_Nine)));
			}
			for (int num10 = 0; num10 < 7; num10++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._4_Four), ReturnRandomCardType(CardType._7_Seven), ReturnRandomCardType(CardType._9_Nine)));
			}
			for (int num11 = 0; num11 < 10; num11++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._10_Ten), ReturnRandomCardType(CardType._4_Four), ReturnRandomCardType(CardType._5_Five)));
			}
			for (int num12 = 0; num12 < 10; num12++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType.Queen), ReturnRandomCardType(CardType.Ace), ReturnRandomCardType(CardType._8_Eight)));
			}
			for (int num13 = 0; num13 < 10; num13++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType.King), ReturnRandomCardType(CardType._2_Two), ReturnRandomCardType(CardType._7_Seven)));
			}
			for (int num14 = 0; num14 < 10; num14++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._7_Seven), ReturnRandomCardType(CardType._9_Nine), ReturnRandomCardType(CardType._3_Three)));
			}
			for (int num15 = 0; num15 < 10; num15++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._3_Three), ReturnRandomCardType(CardType._6_Six), ReturnRandomCardType(CardType.King)));
			}
			for (int num16 = 0; num16 < 10; num16++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._3_Three), ReturnRandomCardType(CardType._6_Six), ReturnRandomCardType(CardType.Queen)));
			}
			for (int num17 = 0; num17 < 10; num17++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._2_Two), ReturnRandomCardType(CardType._7_Seven), ReturnRandomCardType(CardType._10_Ten)));
			}
			for (int num18 = 0; num18 < 10; num18++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType.Ace), ReturnRandomCardType(CardType._8_Eight), ReturnRandomCardType(CardType.Jack)));
			}
			for (int num19 = 0; num19 < 15; num19++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType.Ace), ReturnRandomCardType(CardType._7_Seven), ReturnRandomCardType(CardType.Jack)));
			}
			for (int num20 = 0; num20 < 15; num20++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType.Queen), ReturnRandomCardType(CardType._6_Six), ReturnRandomCardType(CardType._2_Two)));
			}
			for (int num21 = 0; num21 < 15; num21++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._8_Eight), ReturnRandomCardType(CardType._3_Three), ReturnRandomCardType(CardType._7_Seven)));
			}
			for (int num22 = 0; num22 < 15; num22++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._6_Six), ReturnRandomCardType(CardType._7_Seven), ReturnRandomCardType(CardType._5_Five)));
			}
			for (int num23 = 0; num23 < 15; num23++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._3_Three), ReturnRandomCardType(CardType._10_Ten), ReturnRandomCardType(CardType._5_Five)));
			}
			for (int num24 = 0; num24 < 20; num24++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._3_Three), ReturnRandomCardType(CardType._9_Nine), ReturnRandomCardType(CardType._5_Five)));
			}
			for (int num25 = 0; num25 < 20; num25++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._5_Five), ReturnRandomCardType(CardType.Queen), ReturnRandomCardType(CardType._2_Two)));
			}
			for (int num26 = 0; num26 < 20; num26++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._2_Two), ReturnRandomCardType(CardType._9_Nine), ReturnRandomCardType(CardType._6_Six)));
			}
			for (int num27 = 0; num27 < 20; num27++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType.Queen), ReturnRandomCardType(CardType.Ace), ReturnRandomCardType(CardType._5_Five)));
			}
			for (int num28 = 0; num28 < 25; num28++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._9_Nine), ReturnRandomCardType(CardType.Ace), ReturnRandomCardType(CardType._5_Five)));
			}
			for (int num29 = 0; num29 < 25; num29++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._9_Nine), ReturnRandomCardType(CardType._2_Two), ReturnRandomCardType(CardType._4_Four)));
			}
			for (int num30 = 0; num30 < 25; num30++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._6_Six), ReturnRandomCardType(CardType._7_Seven), ReturnRandomCardType(CardType._3_Three)));
			}
			for (int num31 = 0; num31 < 25; num31++)
			{
				DealerPresetCars.Add(new BlackjackCards(ReturnRandomCardType(CardType._2_Two), ReturnRandomCardType(CardType._8_Eight), ReturnRandomCardType(CardType._6_Six)));
			}
			for (int num32 = 0; num32 < 100; num32++)
			{
				string randomCardDealer_BlackJack = GetRandomCardDealer_BlackJack();
				Script.Wait(0);
				string randomCardDealer_BlackJack2 = GetRandomCardDealer_BlackJack();
				Script.Wait(0);
				string randomCardDealer_BlackJack3 = GetRandomCardDealer_BlackJack();
				DealerPresetCars.Add(new BlackjackCards(randomCardDealer_BlackJack, randomCardDealer_BlackJack2, randomCardDealer_BlackJack3));
			}
		}

		public static string ReturnRandomCardType(CardType Type)
		{
			List<string> list = new List<string>();
			if (Type == CardType.Ace)
			{
				if (PlayerDeck == 1 || PlayerDeck == 0)
				{
					list.Add("vw_prop_cas_card_club_ace");
					list.Add("vw_prop_cas_card_spd_ace");
					list.Add("vw_prop_cas_card_hrt_ace");
					list.Add("vw_prop_cas_card_dia_ace");
				}
				if (PlayerDeck == 2)
				{
					list.Add("vw_prop_vw_club_char_a_a");
					list.Add("vw_prop_vw_dia_char_a_a");
					list.Add("vw_prop_vw_hrt_char_a_a");
					list.Add("vw_prop_vw_spd_char_a_a");
				}
				if (PlayerDeck == 3 && TMD_Dealer)
				{
					list.Add("vw_prop_cas_card_club_ace");
					list.Add("vw_prop_cas_card_spd_ace");
					list.Add("vw_prop_cas_card_hrt_ace");
					list.Add("vw_prop_cas_card_dia_ace");
					list.Add("vw_prop_vw_club_char_a_a");
					list.Add("vw_prop_vw_dia_char_a_a");
					list.Add("vw_prop_vw_hrt_char_a_a");
					list.Add("vw_prop_vw_spd_char_a_a");
				}
			}
			if (Type == CardType.King)
			{
				if (PlayerDeck == 1 || PlayerDeck == 0)
				{
					list.Add("vw_prop_cas_card_club_king");
					list.Add("vw_prop_cas_card_spd_king");
					list.Add("vw_prop_cas_card_hrt_king");
					list.Add("vw_prop_cas_card_dia_king");
				}
				if (PlayerDeck == 2)
				{
					list.Add("vw_prop_vw_club_char_k_a");
					list.Add("vw_prop_vw_dia_char_k_a");
					list.Add("vw_prop_vw_hrt_char_k_a");
					list.Add("vw_prop_vw_spd_char_k_a");
				}
				if (PlayerDeck == 3 && TMD_Dealer)
				{
					list.Add("vw_prop_cas_card_club_king");
					list.Add("vw_prop_cas_card_spd_king");
					list.Add("vw_prop_cas_card_hrt_king");
					list.Add("vw_prop_cas_card_dia_king");
					list.Add("vw_prop_vw_club_char_k_a");
					list.Add("vw_prop_vw_dia_char_k_a");
					list.Add("vw_prop_vw_hrt_char_k_a");
					list.Add("vw_prop_vw_spd_char_k_a");
				}
			}
			if (Type == CardType.Queen)
			{
				if (PlayerDeck == 1 || PlayerDeck == 0)
				{
					list.Add("vw_prop_cas_card_club_queen");
					list.Add("vw_prop_cas_card_spd_queen");
					list.Add("vw_prop_cas_card_hrt_queen");
					list.Add("vw_prop_cas_card_dia_queen");
				}
				if (PlayerDeck == 2)
				{
					list.Add("vw_prop_vw_club_char_q_a");
					list.Add("vw_prop_vw_dia_char_q_a");
					list.Add("vw_prop_vw_hrt_char_q_a");
					list.Add("vw_prop_vw_spd_char_q_a");
				}
				if (PlayerDeck == 3 && TMD_Dealer)
				{
					list.Add("vw_prop_cas_card_club_queen");
					list.Add("vw_prop_cas_card_spd_queen");
					list.Add("vw_prop_cas_card_hrt_queen");
					list.Add("vw_prop_cas_card_dia_queen");
					list.Add("vw_prop_vw_club_char_q_a");
					list.Add("vw_prop_vw_dia_char_q_a");
					list.Add("vw_prop_vw_hrt_char_q_a");
					list.Add("vw_prop_vw_spd_char_q_a");
				}
			}
			if (Type == CardType.Jack)
			{
				if (PlayerDeck == 1 || PlayerDeck == 0)
				{
					list.Add("vw_prop_cas_card_club_jack");
					list.Add("vw_prop_cas_card_spd_jack");
					list.Add("vw_prop_cas_card_hrt_jack");
					list.Add("vw_prop_cas_card_dia_jack");
				}
				if (PlayerDeck == 2)
				{
					list.Add("vw_prop_vw_club_char_j_a");
					list.Add("vw_prop_vw_dia_char_j_a");
					list.Add("vw_prop_vw_hrt_char_j_a");
					list.Add("vw_prop_vw_spd_char_j_a");
				}
				if (PlayerDeck == 3 && TMD_Dealer)
				{
					list.Add("vw_prop_cas_card_club_jack");
					list.Add("vw_prop_cas_card_spd_jack");
					list.Add("vw_prop_cas_card_hrt_jack");
					list.Add("vw_prop_cas_card_dia_jack");
					list.Add("vw_prop_vw_club_char_j_a");
					list.Add("vw_prop_vw_dia_char_j_a");
					list.Add("vw_prop_vw_hrt_char_j_a");
					list.Add("vw_prop_vw_spd_char_j_a");
				}
			}
			if (Type == CardType._2_Two)
			{
				if (PlayerDeck == 1 || PlayerDeck == 0)
				{
					list.Add("vw_prop_cas_card_club_02");
					list.Add("vw_prop_cas_card_spd_02");
					list.Add("vw_prop_cas_card_hrt_02");
					list.Add("vw_prop_cas_card_dia_02");
				}
				if (PlayerDeck == 2)
				{
					list.Add("vw_prop_vw_club_char_02a");
					list.Add("vw_prop_vw_dia_char_02a");
					list.Add("vw_prop_vw_hrt_char_02a");
					list.Add("vw_prop_vw_spd_char_02a");
				}
				if (PlayerDeck == 3 && TMD_Dealer)
				{
					list.Add("vw_prop_cas_card_club_02");
					list.Add("vw_prop_cas_card_spd_02");
					list.Add("vw_prop_cas_card_hrt_02");
					list.Add("vw_prop_cas_card_dia_02");
					list.Add("vw_prop_vw_club_char_02a");
					list.Add("vw_prop_vw_dia_char_02a");
					list.Add("vw_prop_vw_hrt_char_02a");
					list.Add("vw_prop_vw_spd_char_02a");
				}
			}
			if (Type == CardType._3_Three)
			{
				if (PlayerDeck == 1 || PlayerDeck == 0)
				{
					list.Add("vw_prop_cas_card_club_03");
					list.Add("vw_prop_cas_card_spd_03");
					list.Add("vw_prop_cas_card_hrt_03");
					list.Add("vw_prop_cas_card_dia_03");
				}
				if (PlayerDeck == 2)
				{
					list.Add("vw_prop_vw_club_char_03a");
					list.Add("vw_prop_vw_dia_char_03a");
					list.Add("vw_prop_vw_hrt_char_03a");
					list.Add("vw_prop_vw_spd_char_03a");
				}
				if (PlayerDeck == 3 && TMD_Dealer)
				{
					list.Add("vw_prop_cas_card_club_03");
					list.Add("vw_prop_cas_card_spd_03");
					list.Add("vw_prop_cas_card_hrt_03");
					list.Add("vw_prop_cas_card_dia_03");
					list.Add("vw_prop_vw_club_char_03a");
					list.Add("vw_prop_vw_dia_char_03a");
					list.Add("vw_prop_vw_hrt_char_03a");
					list.Add("vw_prop_vw_spd_char_03a");
				}
			}
			if (Type == CardType._4_Four)
			{
				if (PlayerDeck == 1 || PlayerDeck == 0)
				{
					list.Add("vw_prop_cas_card_club_04");
					list.Add("vw_prop_cas_card_spd_04");
					list.Add("vw_prop_cas_card_hrt_04");
					list.Add("vw_prop_cas_card_dia_04");
				}
				if (PlayerDeck == 2)
				{
					list.Add("vw_prop_vw_club_char_04a");
					list.Add("vw_prop_vw_dia_char_04a");
					list.Add("vw_prop_vw_hrt_char_04a");
					list.Add("vw_prop_vw_spd_char_04a");
				}
				if (PlayerDeck == 3 && TMD_Dealer)
				{
					list.Add("vw_prop_cas_card_club_04");
					list.Add("vw_prop_cas_card_spd_04");
					list.Add("vw_prop_cas_card_hrt_04");
					list.Add("vw_prop_cas_card_dia_04");
					list.Add("vw_prop_vw_club_char_04a");
					list.Add("vw_prop_vw_dia_char_04a");
					list.Add("vw_prop_vw_hrt_char_04a");
					list.Add("vw_prop_vw_spd_char_04a");
				}
			}
			if (Type == CardType._5_Five)
			{
				if (PlayerDeck == 1 || PlayerDeck == 0)
				{
					list.Add("vw_prop_cas_card_club_05");
					list.Add("vw_prop_cas_card_spd_05");
					list.Add("vw_prop_cas_card_hrt_05");
					list.Add("vw_prop_cas_card_dia_05");
				}
				if (PlayerDeck == 2)
				{
					list.Add("vw_prop_vw_club_char_05a");
					list.Add("vw_prop_vw_dia_char_05a");
					list.Add("vw_prop_vw_hrt_char_05a");
					list.Add("vw_prop_vw_spd_char_05a");
				}
				if (PlayerDeck == 3 && TMD_Dealer)
				{
					list.Add("vw_prop_cas_card_club_05");
					list.Add("vw_prop_cas_card_spd_05");
					list.Add("vw_prop_cas_card_hrt_05");
					list.Add("vw_prop_cas_card_dia_05");
					list.Add("vw_prop_vw_club_char_05a");
					list.Add("vw_prop_vw_dia_char_05a");
					list.Add("vw_prop_vw_hrt_char_05a");
					list.Add("vw_prop_vw_spd_char_05a");
				}
			}
			if (Type == CardType._6_Six)
			{
				if (PlayerDeck == 1 || PlayerDeck == 0)
				{
					list.Add("vw_prop_cas_card_club_06");
					list.Add("vw_prop_cas_card_spd_06");
					list.Add("vw_prop_cas_card_hrt_06");
					list.Add("vw_prop_cas_card_dia_06");
				}
				if (PlayerDeck == 2)
				{
					list.Add("vw_prop_vw_club_char_06a");
					list.Add("vw_prop_vw_dia_char_06a");
					list.Add("vw_prop_vw_hrt_char_06a");
					list.Add("vw_prop_vw_spd_char_06a");
				}
				if (PlayerDeck == 3 && TMD_Dealer)
				{
					list.Add("vw_prop_cas_card_club_06");
					list.Add("vw_prop_cas_card_spd_06");
					list.Add("vw_prop_cas_card_hrt_06");
					list.Add("vw_prop_cas_card_dia_06");
					list.Add("vw_prop_vw_club_char_06a");
					list.Add("vw_prop_vw_dia_char_06a");
					list.Add("vw_prop_vw_hrt_char_06a");
					list.Add("vw_prop_vw_spd_char_06a");
				}
			}
			if (Type == CardType._7_Seven)
			{
				if (PlayerDeck == 1 || PlayerDeck == 0)
				{
					list.Add("vw_prop_cas_card_club_07");
					list.Add("vw_prop_cas_card_spd_07");
					list.Add("vw_prop_cas_card_hrt_07");
					list.Add("vw_prop_cas_card_dia_07");
				}
				if (PlayerDeck == 2)
				{
					list.Add("vw_prop_vw_club_char_07a");
					list.Add("vw_prop_vw_dia_char_07a");
					list.Add("vw_prop_vw_hrt_char_07a");
					list.Add("vw_prop_vw_spd_char_07a");
				}
				if (PlayerDeck == 3 && TMD_Dealer)
				{
					list.Add("vw_prop_cas_card_club_07");
					list.Add("vw_prop_cas_card_spd_07");
					list.Add("vw_prop_cas_card_hrt_07");
					list.Add("vw_prop_cas_card_dia_07");
					list.Add("vw_prop_vw_club_char_07a");
					list.Add("vw_prop_vw_dia_char_07a");
					list.Add("vw_prop_vw_hrt_char_07a");
					list.Add("vw_prop_vw_spd_char_07a");
				}
			}
			if (Type == CardType._8_Eight)
			{
				if (PlayerDeck == 1 || PlayerDeck == 0)
				{
					list.Add("vw_prop_cas_card_club_08");
					list.Add("vw_prop_cas_card_spd_08");
					list.Add("vw_prop_cas_card_hrt_08");
					list.Add("vw_prop_cas_card_dia_08");
				}
				if (PlayerDeck == 2)
				{
					list.Add("vw_prop_vw_club_char_08a");
					list.Add("vw_prop_vw_dia_char_08a");
					list.Add("vw_prop_vw_hrt_char_08a");
					list.Add("vw_prop_vw_spd_char_08a");
				}
				if (PlayerDeck == 3 && TMD_Dealer)
				{
					list.Add("vw_prop_cas_card_club_08");
					list.Add("vw_prop_cas_card_spd_08");
					list.Add("vw_prop_cas_card_hrt_08");
					list.Add("vw_prop_cas_card_dia_08");
					list.Add("vw_prop_vw_club_char_08a");
					list.Add("vw_prop_vw_dia_char_08a");
					list.Add("vw_prop_vw_hrt_char_08a");
					list.Add("vw_prop_vw_spd_char_08a");
				}
			}
			if (Type == CardType._9_Nine)
			{
				if (PlayerDeck == 1 || PlayerDeck == 0)
				{
					list.Add("vw_prop_cas_card_club_09");
					list.Add("vw_prop_cas_card_spd_09");
					list.Add("vw_prop_cas_card_hrt_09");
					list.Add("vw_prop_cas_card_dia_09");
				}
				if (PlayerDeck == 2)
				{
					list.Add("vw_prop_vw_club_char_09a");
					list.Add("vw_prop_vw_dia_char_09a");
					list.Add("vw_prop_vw_hrt_char_09a");
					list.Add("vw_prop_vw_spd_char_09a");
				}
				if (PlayerDeck == 3 && TMD_Dealer)
				{
					list.Add("vw_prop_cas_card_club_09");
					list.Add("vw_prop_cas_card_spd_09");
					list.Add("vw_prop_cas_card_hrt_09");
					list.Add("vw_prop_cas_card_dia_09");
					list.Add("vw_prop_vw_club_char_09a");
					list.Add("vw_prop_vw_dia_char_09a");
					list.Add("vw_prop_vw_hrt_char_09a");
					list.Add("vw_prop_vw_spd_char_09a");
				}
			}
			if (Type == CardType._10_Ten)
			{
				if (PlayerDeck == 1 || PlayerDeck == 0)
				{
					list.Add("vw_prop_cas_card_club_10");
					list.Add("vw_prop_cas_card_spd_10");
					list.Add("vw_prop_cas_card_hrt_10");
					list.Add("vw_prop_cas_card_dia_10");
				}
				if (PlayerDeck == 2)
				{
					list.Add("vw_prop_vw_club_char_10a");
					list.Add("vw_prop_vw_dia_char_10a");
					list.Add("vw_prop_vw_hrt_char_10a");
					list.Add("vw_prop_vw_spd_char_10a");
				}
				if (PlayerDeck == 3 && TMD_Dealer)
				{
					list.Add("vw_prop_cas_card_club_10");
					list.Add("vw_prop_cas_card_spd_10");
					list.Add("vw_prop_cas_card_hrt_10");
					list.Add("vw_prop_cas_card_dia_10");
					list.Add("vw_prop_vw_club_char_10a");
					list.Add("vw_prop_vw_dia_char_10a");
					list.Add("vw_prop_vw_hrt_char_10a");
					list.Add("vw_prop_vw_spd_char_10a");
				}
			}
			return list[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list.Count)];
		}

		public static void CalculateBlackJack(DealerTable Dealer)
		{
			if (PlayerCardA != null)
			{
				PlayerCardA.Position = ClosestDealerTable.Table.GetOffsetPosition(GetPlayerCardPositionOffset(0, SeatSittingIn - 1)) + new Vector3(0f, 0f, 0.01f);
				PlayerCardA.Rotation = ClosestDealerTable.Table.Rotation + GetPlayerCardRotationOffset(0, SeatSittingIn - 1);
			}
			if (PlayerCardB != null)
			{
				PlayerCardB.Position = ClosestDealerTable.Table.GetOffsetPosition(GetPlayerCardPositionOffset(1, SeatSittingIn - 1, BlackJackSplit)) + new Vector3(0f, 0f, 0.01f);
				PlayerCardB.Rotation = ClosestDealerTable.Table.Rotation + GetPlayerCardRotationOffset(1, SeatSittingIn - 1, BlackJackSplit);
			}
			if (PlayerCardC != null)
			{
				PlayerCardC.Position = ClosestDealerTable.Table.GetOffsetPosition(GetPlayerCardPositionOffset(2, SeatSittingIn - 1)) + new Vector3(0f, 0f, 0.01f);
				PlayerCardC.Rotation = ClosestDealerTable.Table.Rotation + GetPlayerCardRotationOffset(2, SeatSittingIn - 1);
			}
			if (PlayerCardD != null)
			{
				PlayerCardD.Position = ClosestDealerTable.Table.GetOffsetPosition(GetPlayerCardPositionOffset(3, SeatSittingIn - 1, BlackJackSplit)) + new Vector3(0f, 0f, 0.01f);
				PlayerCardD.Rotation = ClosestDealerTable.Table.Rotation + GetPlayerCardRotationOffset(3, SeatSittingIn - 1, BlackJackSplit);
			}
			if (PlayerCardE != null)
			{
				PlayerCardE.Position = ClosestDealerTable.Table.GetOffsetPosition(GetPlayerCardPositionOffset(4, SeatSittingIn - 1)) + new Vector3(0f, 0f, 0.01f);
				PlayerCardE.Rotation = ClosestDealerTable.Table.Rotation + GetPlayerCardRotationOffset(4, SeatSittingIn - 1, false, BlackJackSplit);
			}
			if (PlayerCardF != null)
			{
				PlayerCardF.Position = ClosestDealerTable.Table.GetOffsetPosition(GetPlayerCardPositionOffset(5, SeatSittingIn - 1, BlackJackSplit)) + new Vector3(0f, 0f, 0.01f);
				PlayerCardF.Rotation = ClosestDealerTable.Table.Rotation + GetPlayerCardRotationOffset(5, SeatSittingIn - 1, BlackJackSplit, true);
			}
			float num = PlayerCard_1 + PlayerCard_2 + PlayerCard_3 + PlayerCard_4 + PlayerCard_5 + PlayerCard_6;
			float num2 = DealerCard_1 + DealerCard_2 + DealerCard_3 + DealerCard_4 + DealerCard_5 + DealerCard_6;
			if (Playercards >= 2)
			{
				num = PlayerCard_1 + PlayerCard_2;
			}
			if (DealerCards >= 2)
			{
				num2 = DealerCard_1 + DealerCard_2;
			}
			if (Playercards >= 3)
			{
				num = PlayerCard_1 + PlayerCard_2 + PlayerCard_3;
			}
			if (DealerCards >= 3)
			{
				num2 = DealerCard_1 + DealerCard_2 + DealerCard_3;
			}
			if (Playercards >= 4)
			{
				num = PlayerCard_1 + PlayerCard_2 + PlayerCard_3 + PlayerCard_4;
			}
			if (DealerCards >= 4)
			{
				num2 = DealerCard_1 + DealerCard_2 + DealerCard_3 + PlayerCard_4;
			}
			if (Playercards >= 5)
			{
				num = PlayerCard_1 + PlayerCard_2 + PlayerCard_3 + PlayerCard_4 + PlayerCard_5;
			}
			if (DealerCards >= 5)
			{
				num2 = DealerCard_1 + DealerCard_2 + DealerCard_3 + DealerCard_4 + DealerCard_5;
			}
			if (Playercards >= 6)
			{
				num = PlayerCard_1 + PlayerCard_2 + PlayerCard_3 + PlayerCard_4 + PlayerCard_5 + PlayerCard_6;
			}
			if (DealerCards >= 6)
			{
				num2 = DealerCard_1 + DealerCard_2 + DealerCard_3 + DealerCard_4 + DealerCard_5 + DealerCard_6;
			}
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			if (!BlackJackSplit)
			{
				if (num > 21f)
				{
					ShowHelpMessage("You have gone bust.");
					if (BlackJackDoubleDown)
					{
						num3 = 0;
					}
					if (BlackJackDoubleDown)
					{
						num3 = 1;
					}
					Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, Dealer.Dealer, "MINIGAME_BJACK_DEALER_PLAYER_BUST", "SPEECH_PARAMS_STANDARD", false);
					Chips_Amount -= (int)Bet;
					Class1.Config.SetValue("Setup", "Chips_Amount", Chips_Amount);
					Class1.Config.Save();
					Game.Player.Character.Task.PlayAnimation("anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01", "reaction_terrible", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					Notification.Show("~t~Blackjack~w~ Player went Bust, Loss ~g~$" + ((int)Bet).ToString("N"));
				}
				else if (num2 > 21f && num < 21f)
				{
					ShowHelpMessage("The Dealer has gone bust.");
					num3 = 3;
					Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, Dealer.Dealer, "MINIGAME_DEALER_BUSTS", "SPEECH_PARAMS_STANDARD", false);
					Chips_Amount += (int)Bet;
					Class1.Config.SetValue("Setup", "Chips_Amount", Chips_Amount);
					Class1.Config.Save();
					Game.Player.Character.Task.PlayAnimation("anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01", "reaction_terrible", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					Notification.Show("~t~Blackjack~w~ Dealer went bust, Payout ~g~$" + ((int)Bet).ToString("N"));
				}
				else if (num == 21f && num2 != 21f)
				{
					ShowHelpMessage("You've won with Blackjack.");
					num3 = 4;
					Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, Dealer.Dealer, "MINIGAME_BJACK_DEALER_BLACKJACK", "SPEECH_PARAMS_STANDARD", false);
					Chips_Amount += (int)(Bet * 1.5f);
					Class1.Config.SetValue("Setup", "Chips_Amount", Chips_Amount);
					Class1.Config.Save();
					Game.Player.Character.Task.PlayAnimation("anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01", "reaction_terrible", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					Notification.Show("~t~Blackjack~w~ Player Won, Dealer had a lower hand, Payout ~g~$" + ((int)(Bet * 1.5f)).ToString("N"));
				}
				else if (num2 == num && num <= 21f)
				{
					ShowHelpMessage("You and the Dealer have the same hands. The hand is a push.");
					num3 = 2;
					Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, Dealer.Dealer, "MINIGAME_DEALER_WINS", "SPEECH_PARAMS_STANDARD", false);
					Game.Player.Character.Task.PlayAnimation("anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01", "reaction_good_var01", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					Notification.Show("~t~Blackjack~w~ Dealer and Player tied, Bet has been pushed (returned) to Player, Payout ~g~$0.00");
				}
				else if (num < num2 && num2 <= 21f)
				{
					string text = num2.ToString();
					if (num2 == 21f)
					{
						text = "Blackjack";
					}
					ShowHelpMessage("The Dealer has won with a " + text + ".");
					if (BlackJackDoubleDown)
					{
						num3 = 0;
					}
					if (BlackJackDoubleDown)
					{
						num3 = 1;
					}
					Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, Dealer.Dealer, "MINIGAME_DEALER_WINS", "SPEECH_PARAMS_STANDARD", false);
					Chips_Amount -= (int)Bet;
					Class1.Config.SetValue("Setup", "Chips_Amount", Chips_Amount);
					Class1.Config.Save();
					Game.Player.Character.Task.PlayAnimation("anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01", "reaction_bad_var01", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					Notification.Show("~t~Blackjack~w~ Dealer had a better hand, Loss ~g~$" + ((int)Bet).ToString("N"));
				}
				else if ((num > num2 && num <= 21f) || num2 > 21f)
				{
					num3 = 3;
					if (num == 21f)
					{
						ShowHelpMessage("You've won with Blackjack.");
						Chips_Amount += (int)(Bet * 1.5f);
					}
					else
					{
						ShowHelpMessage("You have " + num + ". You win " + Bet.ToString("N") + " Chips!");
						Chips_Amount += (int)Bet;
					}
					Class1.Config.SetValue("Setup", "Chips_Amount", Chips_Amount);
					Class1.Config.Save();
					Game.Player.Character.Task.PlayAnimation("anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01", "reaction_good_var02", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					if (num2 > 21f)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, Dealer.Dealer, "MINIGAME_DEALER_BUSTS", "SPEECH_PARAMS_STANDARD", false);
						if (num == 21f)
						{
							Notification.Show("~t~Blackjack~w~ Player Won, Dealer went bust, Payout ~g~$" + ((int)(Bet * 1.5f)).ToString("N"));
						}
						else
						{
							Notification.Show("~t~Blackjack~w~ Player Won, Dealer went bust, Payout ~g~$" + ((int)Bet).ToString("N"));
						}
					}
					else
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, Dealer.Dealer, "MINIGAME_DEALER_ANOTHER_GO", "SPEECH_PARAMS_STANDARD", false);
						if (num == 21f)
						{
							Notification.Show("~t~Blackjack~w~ Player Won, Dealer had a lower hand, Payout ~g~$" + ((int)(Bet * 1.5f)).ToString("N"));
						}
						else
						{
							Notification.Show("~t~Blackjack~w~ Player Won, Dealer had a lower hand, Payout ~g~$" + ((int)Bet).ToString("N"));
						}
					}
				}
			}
			else
			{
				int left_AHandTotal = CasinoGames_Blackjack.Get_Left_AHandTotal();
				int right_B_HandTotal = CasinoGames_Blackjack.Get_Right_B_HandTotal();
				int num6 = left_AHandTotal;
				if (Game.Player.Name.ToUpper().Equals("CINNBARGNU"))
				{
					Notification.Show("TotalA " + left_AHandTotal + "/" + num2);
					Notification.Show("TotalB " + right_B_HandTotal + "/" + num2);
				}
				if (num6 > 21)
				{
					if (BlackJackDoubleDown)
					{
						num4 = 0;
					}
					if (BlackJackDoubleDown)
					{
						num4 = 1;
					}
					Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, Dealer.Dealer, "MINIGAME_BJACK_DEALER_PLAYER_BUST", "SPEECH_PARAMS_STANDARD", false);
					Chips_Amount -= (int)Bet;
					Class1.Config.SetValue("Setup", "Chips_Amount", Chips_Amount);
					Class1.Config.Save();
					Game.Player.Character.Task.PlayAnimation("anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01", "reaction_terrible", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					Notification.Show("~t~Blackjack~w~ Player went Bust on Left Hand, Loss ~g~$" + ((int)Bet).ToString("N"));
				}
				else if (num2 > 21f && num6 < 21)
				{
					num4 = 3;
					Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, Dealer.Dealer, "MINIGAME_DEALER_BUSTS", "SPEECH_PARAMS_STANDARD", false);
					Chips_Amount += (int)Bet;
					Class1.Config.SetValue("Setup", "Chips_Amount", Chips_Amount);
					Class1.Config.Save();
					Game.Player.Character.Task.PlayAnimation("anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01", "reaction_terrible", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					Notification.Show("~t~Blackjack~w~ Dealer went bust, Payout for Left Hand ~g~$" + ((int)Bet).ToString("N"));
				}
				else if (num6 == 21 && num2 != 21f)
				{
					num4 = 4;
					Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, Dealer.Dealer, "MINIGAME_BJACK_DEALER_BLACKJACK", "SPEECH_PARAMS_STANDARD", false);
					Chips_Amount += (int)(Bet * 1.5f);
					Class1.Config.SetValue("Setup", "Chips_Amount", Chips_Amount);
					Class1.Config.Save();
					Game.Player.Character.Task.PlayAnimation("anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01", "reaction_terrible", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					Notification.Show("~t~Blackjack~w~ Player Won Left Hand, Dealer had a lower hand, Payout ~g~$" + ((int)(Bet * 1.5f)).ToString("N"));
				}
				else if (num2 == (float)num6 && num6 <= 21)
				{
					num4 = 2;
					Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, Dealer.Dealer, "MINIGAME_DEALER_WINS", "SPEECH_PARAMS_STANDARD", false);
					Game.Player.Character.Task.PlayAnimation("anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01", "reaction_good_var01", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					Notification.Show("~t~Blackjack~w~ Dealer Hand and Player  Left Hand tied, Bet has been pushed (returned) to Player, Payout ~g~$0.00");
				}
				else if ((float)num6 < num2 && num2 <= 21f)
				{
					if (BlackJackDoubleDown)
					{
						num4 = 0;
					}
					if (BlackJackDoubleDown)
					{
						num4 = 1;
					}
					Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, Dealer.Dealer, "MINIGAME_DEALER_WINS", "SPEECH_PARAMS_STANDARD", false);
					Chips_Amount -= (int)Bet;
					Class1.Config.SetValue("Setup", "Chips_Amount", Chips_Amount);
					Class1.Config.Save();
					Game.Player.Character.Task.PlayAnimation("anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01", "reaction_bad_var01", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					Notification.Show("~t~Blackjack~w~ Dealer had a better hand than players Left Hand, Loss ~g~$" + ((int)Bet).ToString("N"));
				}
				else if (((float)num6 > num2 && num6 <= 21) || num2 > 21f)
				{
					num4 = 3;
					if (num6 == 21)
					{
						Chips_Amount += (int)(Bet * 1.5f);
					}
					else
					{
						Chips_Amount += (int)Bet;
					}
					Class1.Config.SetValue("Setup", "Chips_Amount", Chips_Amount);
					Class1.Config.Save();
					Game.Player.Character.Task.PlayAnimation("anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01", "reaction_good_var02", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					if (num2 > 21f)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, Dealer.Dealer, "MINIGAME_DEALER_BUSTS", "SPEECH_PARAMS_STANDARD", false);
						if (num6 == 21)
						{
							Notification.Show("~t~Blackjack~w~ Player Won Left Hand, Dealer went bust, Payout ~g~$" + ((int)(Bet * 1.5f)).ToString("N"));
						}
						else
						{
							Notification.Show("~t~Blackjack~w~ Player Won Left Hand, Dealer went bust, Payout ~g~$" + ((int)Bet).ToString("N"));
						}
					}
					else
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, Dealer.Dealer, "MINIGAME_DEALER_ANOTHER_GO", "SPEECH_PARAMS_STANDARD", false);
						if (num6 == 21)
						{
							Notification.Show("~t~Blackjack~w~ Player Won Left Hand, Dealer had a lower hand, Payout ~g~$" + ((int)(Bet * 1.5f)).ToString("N"));
						}
						else
						{
							Notification.Show("~t~Blackjack~w~ Player Won Left Hand, Dealer had a lower hand, Payout ~g~$" + ((int)Bet).ToString("N"));
						}
					}
				}
				int num7 = Game.GameTime + 1000;
				if (Game.GameTime < num7)
				{
					Script.Wait(1);
				}
				num6 = right_B_HandTotal;
				if (num6 > 21)
				{
					if (BlackJackDoubleDown)
					{
						num5 = 0;
					}
					if (BlackJackDoubleDown)
					{
						num5 = 1;
					}
					Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, Dealer.Dealer, "MINIGAME_BJACK_DEALER_PLAYER_BUST", "SPEECH_PARAMS_STANDARD", false);
					Chips_Amount -= (int)Bet;
					Class1.Config.SetValue("Setup", "Chips_Amount", Chips_Amount);
					Class1.Config.Save();
					Game.Player.Character.Task.PlayAnimation("anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01", "reaction_terrible", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					Notification.Show("~t~Blackjack~w~ Player went Bust on Right Hand, Loss ~g~$" + ((int)Bet).ToString("N"));
				}
				else if (num2 > 21f && num6 < 21)
				{
					num5 = 3;
					Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, Dealer.Dealer, "MINIGAME_DEALER_BUSTS", "SPEECH_PARAMS_STANDARD", false);
					Chips_Amount += (int)Bet;
					Class1.Config.SetValue("Setup", "Chips_Amount", Chips_Amount);
					Class1.Config.Save();
					Game.Player.Character.Task.PlayAnimation("anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01", "reaction_terrible", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					Notification.Show("~t~Blackjack~w~ Dealer went bust, Payout for Right Hand ~g~$" + ((int)Bet).ToString("N"));
				}
				else if (num6 == 21 && num2 != 21f)
				{
					num5 = 4;
					Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, Dealer.Dealer, "MINIGAME_BJACK_DEALER_BLACKJACK", "SPEECH_PARAMS_STANDARD", false);
					Chips_Amount += (int)(Bet * 1.5f);
					Class1.Config.SetValue("Setup", "Chips_Amount", Chips_Amount);
					Class1.Config.Save();
					Game.Player.Character.Task.PlayAnimation("anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01", "reaction_terrible", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					Notification.Show("~t~Blackjack~w~ Player Won Right Hand, Dealer had a lower hand, Payout ~g~$" + ((int)(Bet * 1.5f)).ToString("N"));
				}
				else if (num2 == (float)num6 && num6 <= 21)
				{
					num5 = 2;
					Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, Dealer.Dealer, "MINIGAME_DEALER_WINS", "SPEECH_PARAMS_STANDARD", false);
					Game.Player.Character.Task.PlayAnimation("anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01", "reaction_good_var01", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					Notification.Show("~t~Blackjack~w~ Dealer Hand and Player Right Hand tied, Bet has been pushed (returned) to Player, Payout ~g~$0.00");
				}
				else if ((float)num6 < num2 && num2 <= 21f)
				{
					if (BlackJackDoubleDown)
					{
						num5 = 0;
					}
					if (BlackJackDoubleDown)
					{
						num5 = 1;
					}
					Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, Dealer.Dealer, "MINIGAME_DEALER_WINS", "SPEECH_PARAMS_STANDARD", false);
					Chips_Amount -= (int)Bet;
					Class1.Config.SetValue("Setup", "Chips_Amount", Chips_Amount);
					Class1.Config.Save();
					Game.Player.Character.Task.PlayAnimation("anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01", "reaction_bad_var01", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					Notification.Show("~t~Blackjack~w~ Dealer had a better hand than players Right Hand, Loss ~g~$" + ((int)Bet).ToString("N"));
				}
				else if (((float)num6 > num2 && num6 <= 21) || num2 > 21f)
				{
					num5 = 3;
					if (num6 == 21)
					{
						Chips_Amount += (int)(Bet * 1.5f);
					}
					else
					{
						Chips_Amount += (int)Bet;
					}
					Class1.Config.SetValue("Setup", "Chips_Amount", Chips_Amount);
					Class1.Config.Save();
					Game.Player.Character.Task.PlayAnimation("anim_casino_b@amb@casino@games@blackjack@ped_female@engaged@01b@reacts@v01", "reaction_good_var02", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					if (num2 > 21f)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, Dealer.Dealer, "MINIGAME_DEALER_BUSTS", "SPEECH_PARAMS_STANDARD", false);
						if (num6 == 21)
						{
							Notification.Show("~t~Blackjack~w~ Player Won Right Hand, Dealer went bust, Payout ~g~$" + ((int)(Bet * 1.5f)).ToString("N"));
						}
						else
						{
							Notification.Show("~t~Blackjack~w~ Player Won Right Hand, Dealer went bust, Payout ~g~$" + ((int)Bet).ToString("N"));
						}
					}
					else
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, Dealer.Dealer, "MINIGAME_DEALER_ANOTHER_GO", "SPEECH_PARAMS_STANDARD", false);
						if (num6 == 21)
						{
							Notification.Show("~t~Blackjack~w~ Player Won Right Hand, Dealer had a lower hand, Payout ~g~$" + ((int)(Bet * 1.5f)).ToString("N"));
						}
						else
						{
							Notification.Show("~t~Blackjack~w~ Player Won Right Hand, Dealer had a lower hand, Payout ~g~$" + ((int)Bet).ToString("N"));
						}
					}
				}
				if (left_AHandTotal > 21 && right_B_HandTotal > 21)
				{
					ShowHelpMessage("You have gone bust in both hands.");
				}
				else if (left_AHandTotal < 21 && right_B_HandTotal > 21)
				{
					ShowHelpMessage("You have " + left_AHandTotal + " in the left hand and gone bust in the right hand. You win " + Bet.ToString("N") + " Chips!");
				}
				else if (left_AHandTotal > 21 && right_B_HandTotal < 21)
				{
					ShowHelpMessage("You have gone bust in the left hand and have " + right_B_HandTotal + " in the right hand. You win " + Bet.ToString("N") + " Chips!");
				}
				if (left_AHandTotal == 21 && right_B_HandTotal > 21)
				{
					ShowHelpMessage("You have Blackjack in the left hand and gone bust in the right hand. You win " + (Bet * 1.5f).ToString("N") + " Chips!");
				}
				else if (left_AHandTotal > 21 && right_B_HandTotal == 21)
				{
					ShowHelpMessage("You have gone bust in the left hand and have Blackjack in the right hand. You win " + (Bet * 1.5f).ToString("N") + " Chips!");
				}
				else if (left_AHandTotal == 21 && right_B_HandTotal == 21)
				{
					ShowHelpMessage("You have Blackjack in the left hand and have Blackjack in the right hand. You win " + (Bet * 2f * 1.5f).ToString("N") + " Chips!");
				}
			}
			Drawcards = false;
			string text2 = "terrible";
			if (!BlackJackSplit)
			{
				if (num3 <= 0)
				{
					text2 = "terrible";
				}
				if (num3 == 1)
				{
					text2 = "bad";
				}
				if (num3 == 2)
				{
					text2 = "impartial";
				}
				if (num3 == 3)
				{
					text2 = "good";
				}
				if (num3 >= 4)
				{
					text2 = "great";
				}
			}
			else
			{
				num3 = num4 + num5;
				if (num3 <= 0)
				{
					text2 = "terrible";
				}
				if (num3 == 1)
				{
					text2 = "bad";
				}
				if (num3 == 2)
				{
					text2 = "impartial";
				}
				if (num3 == 3)
				{
					text2 = "good";
				}
				if (num3 >= 4)
				{
					text2 = "great";
				}
			}
			Random random = new Random();
			List<string> list = new List<string> { "1", "2", "3", "4" };
			string dict = "anim_casino_b@amb@casino@games@shared@player@";
			string text3 = "reaction_" + text2 + "_var_0" + list[random.Next(0, list.Count)];
			LoadDict(dict);
			Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, PlayerOriginPosition.X, PlayerOriginPosition.Y, PlayerOriginPosition.Z - 1f, 0.0, 0.0, PlayerOriginHeading, 2);
			Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene2, LoadDict(dict), text3, 16f, -16f, 0, 1148846080);
			Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene2, text3, LoadDict(dict), 16f, -16f, 0, 1148846080);
			string dict2 = "anim_casino_b@amb@casino@games@blackjack@dealer";
			string text4 = "idle";
			Vector3 dealerAnimOrigin = GetDealerAnimOrigin();
			Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin.X, dealerAnimOrigin.Y, dealerAnimOrigin.Z, 0.0, 0.0, ClosestDealer.Rotation.Z - 180f, 2);
			Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, ClosestDealer, Scene1, LoadDict(dict2), text4, 16f, -16f, 0, 1148846080);
			Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ClosestDealer, Scene1, text4, LoadDict(dict2), 16f, -16f, 0, 1148846080);
			Endgame = true;
			Stage = 50f;
			Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, GetClosestDealer(Game.Player.Character.Position, BlackjackDealerTables), "MINIGAME_DEALER_ANOTHER_GO", "SPEECH_PARAMS_STANDARD", false);
		}

		public static string GetSlotMachineAudioDict(string PropModel)
		{
			string result = "";
			if (PropModel.Equals("vw_prop_casino_slot_01a"))
			{
				result = "dlc_vw_casino_slot_machine_ak_npc_sounds";
			}
			if (PropModel.Equals("vw_prop_casino_slot_02a"))
			{
				result = "dlc_vw_casino_slot_machine_rsr_npc_sounds";
			}
			if (PropModel.Equals("vw_prop_casino_slot_03a"))
			{
				result = "dlc_vw_casino_slot_machine_rsr_npc_sounds";
			}
			if (PropModel.Equals("vw_prop_casino_slot_04a"))
			{
				result = "dlc_vw_casino_slot_machine_fs_npc_sounds";
			}
			if (PropModel.Equals("vw_prop_casino_slot_05a"))
			{
				result = "dlc_vw_casino_slot_machine_ds_npc_sounds";
			}
			if (PropModel.Equals("vw_prop_casino_slot_06a"))
			{
				result = "dlc_vw_casino_slot_machine_kd_npc_sounds";
			}
			if (PropModel.Equals("vw_prop_casino_slot_07a"))
			{
				result = "dlc_vw_casino_slot_machine_td_npc_sounds";
			}
			if (PropModel.Equals("vw_prop_casino_slot_08a"))
			{
				result = "dlc_vw_casino_slot_machine_hz_npc_sounds";
			}
			return result;
		}

		public static void SetStage(Prop Slot, int type)
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

		public static string ConvertSlotHashNametoActualName(string Slot)
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
			if (Z <= 0f && Z < -90f)
			{
				Z -= 1f;
			}
			return 0f;
		}

		public static string ReturnToken(string TokenReturn, float HitNum, float ActualNum)
		{
			string result = "";
			if (ActualNum >= HitNum - 5f && ActualNum <= HitNum + 5f)
			{
				result = TokenReturn;
			}
			return result;
		}

		public static float CalcualteSlotWin(string Machine)
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

		public static string GetLandedOn(float reelR, string Machine)
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

		public static float GetClosest(float reelR, string Machine)
		{
			float result = 0f;
			if (Machine.Equals("vw_prop_casino_slot_01a"))
			{
				List<int> source = new List<int>
				{
					115, 135, 150, -177, -134, -110, -89, -66, -44, -22,
					4, 25, 46, 71, 91
				};
				int number = (int)reelR;
				result = source.OrderBy((int item) => Math.Abs(number - item)).First();
			}
			if (Machine.Equals("vw_prop_casino_slot_02a"))
			{
				List<int> source2 = new List<int>
				{
					115, 135, 150, -177, -134, -110, -89, -66, -44, -22,
					4, 25, 46, 71, 91
				};
				int number2 = (int)reelR;
				result = source2.OrderBy((int item) => Math.Abs(number2 - item)).First();
			}
			if (Machine.Equals("vw_prop_casino_slot_03a"))
			{
				List<int> source3 = new List<int>
				{
					115, 135, 150, -177, -134, -110, -89, -66, -44, -22,
					4, 25, 46, 71, 91
				};
				int number3 = (int)reelR;
				result = source3.OrderBy((int item) => Math.Abs(number3 - item)).First();
			}
			if (Machine.Equals("vw_prop_casino_slot_04a"))
			{
				List<int> source4 = new List<int>
				{
					115, 135, 150, -177, -134, -110, -89, -66, -44, -22,
					4, 25, 46, 71, 91
				};
				int number4 = (int)reelR;
				result = source4.OrderBy((int item) => Math.Abs(number4 - item)).First();
			}
			if (Machine.Equals("vw_prop_casino_slot_05a"))
			{
				List<int> source5 = new List<int>
				{
					115, 135, 150, -177, -134, -110, -89, -66, -44, -22,
					4, 25, 46, 71, 91
				};
				int number5 = (int)reelR;
				result = source5.OrderBy((int item) => Math.Abs(number5 - item)).First();
			}
			if (Machine.Equals("vw_prop_casino_slot_06a"))
			{
				List<int> source6 = new List<int>
				{
					115, 135, 150, -177, -134, -110, -89, -66, -44, -22,
					4, 25, 46, 71, 91
				};
				int number6 = (int)reelR;
				result = source6.OrderBy((int item) => Math.Abs(number6 - item)).First();
			}
			if (Machine.Equals("vw_prop_casino_slot_07a"))
			{
				List<int> source7 = new List<int>
				{
					115, 135, 150, -177, -134, -110, -89, -66, -44, -22,
					4, 25, 46, 71, 91
				};
				int number7 = (int)reelR;
				result = source7.OrderBy((int item) => Math.Abs(number7 - item)).First();
			}
			if (Machine.Equals("vw_prop_casino_slot_08a"))
			{
				List<int> source8 = new List<int>
				{
					115, 135, 150, -177, -134, -110, -89, -66, -44, -22,
					4, 25, 46, 71, 91
				};
				int number8 = (int)reelR;
				result = source8.OrderBy((int item) => Math.Abs(number8 - item)).First();
			}
			return result;
		}

		public static void SetRandomOutfit2(Ped Dancer)
		{
			Random random = new Random();
			if (Dancer.Gender == Gender.Female)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 0, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 0)), 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 2, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 2)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 2)), 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 3)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 3)), 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 8, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 8)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 8)), 0);
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 4)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 4, 4, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 4)), 1);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) != 4)
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
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 4, num, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 4)), 1);
				}
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 0, Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3), 0, 0);
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 4) == 4)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 5, 4, 0, 1);
				}
			}
			if (Dancer.Gender == Gender.Male)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 0, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 0)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 0)), 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 2, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 2)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 2)), 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 3)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 3)), 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 8, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 8)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 8)), 0);
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 0)
				{
					int num3 = random.Next(0, 4);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num3, 0);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 1)
				{
					int num4 = random.Next(0, 3);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num4, 0);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 2)
				{
					int num5 = random.Next(0, 3);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num5, 0);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 3)
				{
					int num6 = random.Next(0, 3);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num6, 0);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 4)
				{
					int num7 = random.Next(0, 2);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num7, 0);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 5)
				{
					int num8 = random.Next(0, 2);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num8, 0);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 6)
				{
					int num9 = random.Next(0, 2);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num9, 0);
				}
			}
		}

		public static void SetBet_SlotMachine()
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			Convert.ToInt32(absoluteX / 2f);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 0, 51, 0), "Quit");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 1, 76, 0), "Bet One");
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 21, 0), "");
			scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 176, 0), "Spin/Bet Max");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public static float GetRandomfloat(int minimum, int maximum)
		{
			return new Random().Next(minimum, maximum) / 1000;
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

		public static int func_6397(int iParam0)
		{
			switch (iParam0)
			{
			case 1:
				Horse_Color_primaryColour = 15553363;
				Horse_Color_secondaryColour = 5474797;
				Horse_Color_horseColour = 9858144;
				Horse_Color_maneColour = 4671302;
				return 1;
			case 2:
				Horse_Color_primaryColour = 16724530;
				Horse_Color_secondaryColour = 3684408;
				Horse_Color_horseColour = 14807026;
				Horse_Color_maneColour = 16777215;
				return 1;
			case 3:
				Horse_Color_primaryColour = 13560920;
				Horse_Color_secondaryColour = 15582764;
				Horse_Color_horseColour = 16770746;
				Horse_Color_maneColour = 7500402;
				return 1;
			case 4:
				Horse_Color_primaryColour = 16558591;
				Horse_Color_secondaryColour = 5090807;
				Horse_Color_horseColour = 10446437;
				Horse_Color_maneColour = 7493977;
				return 1;
			case 5:
				Horse_Color_primaryColour = 5090807;
				Horse_Color_secondaryColour = 16558591;
				Horse_Color_horseColour = 3815994;
				Horse_Color_maneColour = 9393493;
				return 1;
			case 6:
				Horse_Color_primaryColour = 16269415;
				Horse_Color_secondaryColour = 16767010;
				Horse_Color_horseColour = 10329501;
				Horse_Color_maneColour = 16777215;
				return 1;
			case 7:
				Horse_Color_primaryColour = 2263807;
				Horse_Color_secondaryColour = 16777215;
				Horse_Color_horseColour = 9086907;
				Horse_Color_maneColour = 3815994;
				return 1;
			case 8:
				Horse_Color_primaryColour = 4879871;
				Horse_Color_secondaryColour = 16715535;
				Horse_Color_horseColour = 3815994;
				Horse_Color_maneColour = 16777215;
				return 1;
			case 9:
				Horse_Color_primaryColour = 16777215;
				Horse_Color_secondaryColour = 2263807;
				Horse_Color_horseColour = 16769737;
				Horse_Color_maneColour = 15197642;
				return 1;
			case 11:
				Horse_Color_primaryColour = 16777215;
				Horse_Color_secondaryColour = 16559849;
				Horse_Color_horseColour = 5716493;
				Horse_Color_maneColour = 3815994;
				return 1;
			case 10:
				Horse_Color_primaryColour = 16338779;
				Horse_Color_secondaryColour = 16777215;
				Horse_Color_horseColour = 11166563;
				Horse_Color_maneColour = 6974058;
				return 1;
			case 12:
				Horse_Color_primaryColour = 16760644;
				Horse_Color_secondaryColour = 3387257;
				Horse_Color_horseColour = 16701597;
				Horse_Color_maneColour = 16777215;
				return 1;
			case 13:
				Horse_Color_primaryColour = 6538729;
				Horse_Color_secondaryColour = 2249420;
				Horse_Color_horseColour = 16777215;
				Horse_Color_maneColour = 3815994;
				return 1;
			case 14:
				Horse_Color_primaryColour = 15913534;
				Horse_Color_secondaryColour = 15913534;
				Horse_Color_horseColour = 16304787;
				Horse_Color_maneColour = 15985375;
				return 1;
			case 15:
				Horse_Color_primaryColour = 15655629;
				Horse_Color_secondaryColour = 16240452;
				Horse_Color_horseColour = 16760474;
				Horse_Color_maneColour = 13664854;
				return 1;
			case 16:
				Horse_Color_primaryColour = 16320263;
				Horse_Color_secondaryColour = 16777215;
				Horse_Color_horseColour = 14920312;
				Horse_Color_maneColour = 16773316;
				return 1;
			case 17:
				Horse_Color_primaryColour = 7176404;
				Horse_Color_secondaryColour = 15138618;
				Horse_Color_horseColour = 6308658;
				Horse_Color_maneColour = 13664854;
				return 1;
			case 18:
				Horse_Color_primaryColour = 4879871;
				Horse_Color_secondaryColour = 8453903;
				Horse_Color_horseColour = 11382189;
				Horse_Color_maneColour = 15724527;
				return 1;
			case 19:
				Horse_Color_primaryColour = 16777215;
				Horse_Color_secondaryColour = 16777215;
				Horse_Color_horseColour = 16754809;
				Horse_Color_maneColour = 16777215;
				return 1;
			case 20:
				Horse_Color_primaryColour = 16732497;
				Horse_Color_secondaryColour = 16732497;
				Horse_Color_horseColour = 3815994;
				Horse_Color_maneColour = 16777215;
				return 1;
			case 21:
				Horse_Color_primaryColour = 5739220;
				Horse_Color_secondaryColour = 5739220;
				Horse_Color_horseColour = 11382189;
				Horse_Color_maneColour = 15724527;
				return 1;
			case 22:
				Horse_Color_primaryColour = 16712909;
				Horse_Color_secondaryColour = 6935639;
				Horse_Color_horseColour = 8742735;
				Horse_Color_maneColour = 3877137;
				return 1;
			case 23:
				Horse_Color_primaryColour = 2136867;
				Horse_Color_secondaryColour = 16777215;
				Horse_Color_horseColour = 16761488;
				Horse_Color_maneColour = 3877137;
				return 1;
			case 24:
				Horse_Color_primaryColour = 3118422;
				Horse_Color_secondaryColour = 10019244;
				Horse_Color_horseColour = 14932209;
				Horse_Color_maneColour = 6121086;
				return 1;
			case 25:
				Horse_Color_primaryColour = 2136867;
				Horse_Color_secondaryColour = 10241979;
				Horse_Color_horseColour = 8081664;
				Horse_Color_maneColour = 3815994;
				return 1;
			case 26:
				Horse_Color_primaryColour = 16769271;
				Horse_Color_secondaryColour = 13724403;
				Horse_Color_horseColour = 9852728;
				Horse_Color_maneColour = 14138263;
				return 1;
			case 27:
				Horse_Color_primaryColour = 13724403;
				Horse_Color_secondaryColour = 16769271;
				Horse_Color_horseColour = 6444881;
				Horse_Color_maneColour = 14138263;
				return 1;
			case 28:
				Horse_Color_primaryColour = 10017279;
				Horse_Color_secondaryColour = 4291288;
				Horse_Color_horseColour = 16304787;
				Horse_Color_maneColour = 15985375;
				return 1;
			case 29:
				Horse_Color_primaryColour = 1071491;
				Horse_Color_secondaryColour = 4315247;
				Horse_Color_horseColour = 14935011;
				Horse_Color_maneColour = 6121086;
				return 1;
			case 30:
				Horse_Color_primaryColour = 3861944;
				Horse_Color_secondaryColour = 16627705;
				Horse_Color_horseColour = 14932209;
				Horse_Color_maneColour = 6121086;
				return 1;
			case 31:
				Horse_Color_primaryColour = 15583546;
				Horse_Color_secondaryColour = 4671303;
				Horse_Color_horseColour = 11836798;
				Horse_Color_maneColour = 3090459;
				return 1;
			case 32:
				Horse_Color_primaryColour = 15567418;
				Horse_Color_secondaryColour = 4671303;
				Horse_Color_horseColour = 9985296;
				Horse_Color_maneColour = 3815994;
				return 1;
			case 33:
				Horse_Color_primaryColour = 5701417;
				Horse_Color_secondaryColour = 16711680;
				Horse_Color_horseColour = 16771760;
				Horse_Color_maneColour = 6970713;
				return 1;
			case 34:
				Horse_Color_primaryColour = 16760303;
				Horse_Color_secondaryColour = 5986951;
				Horse_Color_horseColour = 12353664;
				Horse_Color_maneColour = 15395562;
				return 1;
			case 35:
				Horse_Color_primaryColour = 8907670;
				Horse_Color_secondaryColour = 2709022;
				Horse_Color_horseColour = 9475214;
				Horse_Color_maneColour = 4278081;
				return 1;
			case 36:
				Horse_Color_primaryColour = 5429688;
				Horse_Color_secondaryColour = 6400829;
				Horse_Color_horseColour = 16777215;
				Horse_Color_maneColour = 16773316;
				return 1;
			case 37:
				Horse_Color_primaryColour = 15138618;
				Horse_Color_secondaryColour = 5272210;
				Horse_Color_horseColour = 14920312;
				Horse_Color_maneColour = 16773316;
				return 1;
			case 38:
				Horse_Color_primaryColour = 10241979;
				Horse_Color_secondaryColour = 12396337;
				Horse_Color_horseColour = 14920312;
				Horse_Color_maneColour = 15395562;
				return 1;
			case 39:
				Horse_Color_primaryColour = 16777215;
				Horse_Color_secondaryColour = 13481261;
				Horse_Color_horseColour = 13667152;
				Horse_Color_maneColour = 3815994;
				return 1;
			case 40:
				Horse_Color_primaryColour = 5077874;
				Horse_Color_secondaryColour = 16777215;
				Horse_Color_horseColour = 15444592;
				Horse_Color_maneColour = 7820105;
				return 1;
			case 41:
				Horse_Color_primaryColour = 10408040;
				Horse_Color_secondaryColour = 2960685;
				Horse_Color_horseColour = 7424036;
				Horse_Color_maneColour = 10129549;
				return 1;
			case 42:
				Horse_Color_primaryColour = 7754308;
				Horse_Color_secondaryColour = 16777215;
				Horse_Color_horseColour = 12944259;
				Horse_Color_maneColour = 3815994;
				return 1;
			case 43:
				Horse_Color_primaryColour = 16736955;
				Horse_Color_secondaryColour = 16106560;
				Horse_Color_horseColour = 16771760;
				Horse_Color_maneColour = 6970713;
				return 1;
			case 44:
				Horse_Color_primaryColour = 16106560;
				Horse_Color_secondaryColour = 16770224;
				Horse_Color_horseColour = 16767659;
				Horse_Color_maneColour = 15843765;
				return 1;
			case 45:
				Horse_Color_primaryColour = 9573241;
				Horse_Color_secondaryColour = 14703194;
				Horse_Color_horseColour = 9789279;
				Horse_Color_maneColour = 3815994;
				return 1;
			case 46:
				Horse_Color_primaryColour = 44799;
				Horse_Color_secondaryColour = 14703194;
				Horse_Color_horseColour = 10968156;
				Horse_Color_maneColour = 16777215;
				return 1;
			case 47:
				Horse_Color_primaryColour = 7143224;
				Horse_Color_secondaryColour = 16753956;
				Horse_Color_horseColour = 10975076;
				Horse_Color_maneColour = 4210752;
				return 1;
			case 48:
				Horse_Color_primaryColour = 7895160;
				Horse_Color_secondaryColour = 4013373;
				Horse_Color_horseColour = 5855577;
				Horse_Color_maneColour = 11645361;
				return 1;
			case 49:
				Horse_Color_primaryColour = 16075595;
				Horse_Color_secondaryColour = 6869196;
				Horse_Color_horseColour = 13530742;
				Horse_Color_maneColour = 7105644;
				return 1;
			case 50:
				Horse_Color_primaryColour = 16090955;
				Horse_Color_secondaryColour = 6272992;
				Horse_Color_horseColour = 16777215;
				Horse_Color_maneColour = 16777215;
				return 1;
			case 51:
				Horse_Color_primaryColour = 13313356;
				Horse_Color_secondaryColour = 13313356;
				Horse_Color_horseColour = 5849409;
				Horse_Color_maneColour = 11623516;
				return 1;
			case 52:
				Horse_Color_primaryColour = 13911070;
				Horse_Color_secondaryColour = 5583427;
				Horse_Color_horseColour = 14935011;
				Horse_Color_maneColour = 6121086;
				return 1;
			case 53:
				Horse_Color_primaryColour = 8604661;
				Horse_Color_secondaryColour = 10408040;
				Horse_Color_horseColour = 12944259;
				Horse_Color_maneColour = 3815994;
				return 1;
			case 54:
				Horse_Color_primaryColour = 9716612;
				Horse_Color_secondaryColour = 2960685;
				Horse_Color_horseColour = 16767659;
				Horse_Color_maneColour = 6708313;
				return 1;
			case 55:
				Horse_Color_primaryColour = 7806040;
				Horse_Color_secondaryColour = 16777215;
				Horse_Color_horseColour = 16765601;
				Horse_Color_maneColour = 14144436;
				return 1;
			case 56:
				Horse_Color_primaryColour = 15632075;
				Horse_Color_secondaryColour = 11221989;
				Horse_Color_horseColour = 16777215;
				Horse_Color_maneColour = 16770037;
				return 1;
			case 57:
				Horse_Color_primaryColour = 1936722;
				Horse_Color_secondaryColour = 14654697;
				Horse_Color_horseColour = 16763851;
				Horse_Color_maneColour = 3815994;
				return 1;
			case 58:
				Horse_Color_primaryColour = 10377543;
				Horse_Color_secondaryColour = 3815994;
				Horse_Color_horseColour = 14807026;
				Horse_Color_maneColour = 16777215;
				return 1;
			case 59:
				Horse_Color_primaryColour = 16775067;
				Horse_Color_secondaryColour = 11067903;
				Horse_Color_horseColour = 16770746;
				Horse_Color_maneColour = 7500402;
				return 1;
			case 60:
				Horse_Color_primaryColour = 16741712;
				Horse_Color_secondaryColour = 8669718;
				Horse_Color_horseColour = 16777215;
				Horse_Color_maneColour = 16777215;
				return 1;
			case 61:
				Horse_Color_primaryColour = 16515280;
				Horse_Color_secondaryColour = 6318459;
				Horse_Color_horseColour = 3815994;
				Horse_Color_maneColour = 9393493;
				return 1;
			case 62:
				Horse_Color_primaryColour = 65526;
				Horse_Color_secondaryColour = 16515280;
				Horse_Color_horseColour = 10329501;
				Horse_Color_maneColour = 16777215;
				return 1;
			case 63:
				Horse_Color_primaryColour = 16711680;
				Horse_Color_secondaryColour = 4783925;
				Horse_Color_horseColour = 3815994;
				Horse_Color_maneColour = 3815994;
				return 1;
			case 64:
				Horse_Color_primaryColour = 65532;
				Horse_Color_secondaryColour = 4783925;
				Horse_Color_horseColour = 16766671;
				Horse_Color_maneColour = 15197642;
				return 1;
			case 65:
				Horse_Color_primaryColour = 16760303;
				Horse_Color_secondaryColour = 16760303;
				Horse_Color_horseColour = 3815994;
				Horse_Color_maneColour = 14207663;
				return 1;
			case 66:
				Horse_Color_primaryColour = 16770048;
				Horse_Color_secondaryColour = 16770048;
				Horse_Color_horseColour = 3815994;
				Horse_Color_maneColour = 3815994;
				return 1;
			case 67:
				Horse_Color_primaryColour = 16737792;
				Horse_Color_secondaryColour = 16737792;
				Horse_Color_horseColour = 11166563;
				Horse_Color_maneColour = 6974058;
				return 1;
			case 68:
				Horse_Color_primaryColour = 12773119;
				Horse_Color_secondaryColour = 12773119;
				Horse_Color_horseColour = 5716493;
				Horse_Color_maneColour = 3815994;
				return 1;
			case 69:
				Horse_Color_primaryColour = 16777215;
				Horse_Color_secondaryColour = 16763043;
				Horse_Color_horseColour = 16701597;
				Horse_Color_maneColour = 16777215;
				return 1;
			case 70:
				Horse_Color_primaryColour = 6587161;
				Horse_Color_secondaryColour = 6587161;
				Horse_Color_horseColour = 16777215;
				Horse_Color_maneColour = 3815994;
				return 1;
			case 71:
				Horse_Color_primaryColour = 6329328;
				Horse_Color_secondaryColour = 16749602;
				Horse_Color_horseColour = 3815994;
				Horse_Color_maneColour = 3815994;
				return 1;
			case 72:
				Horse_Color_primaryColour = 15793920;
				Horse_Color_secondaryColour = 16519679;
				Horse_Color_horseColour = 14920312;
				Horse_Color_maneColour = 15395562;
				return 1;
			case 73:
				Horse_Color_primaryColour = 15466636;
				Horse_Color_secondaryColour = 10724259;
				Horse_Color_horseColour = 16760474;
				Horse_Color_maneColour = 13664854;
				return 1;
			case 74:
				Horse_Color_primaryColour = 11563263;
				Horse_Color_secondaryColour = 327629;
				Horse_Color_horseColour = 6308658;
				Horse_Color_maneColour = 13664854;
				return 1;
			case 75:
				Horse_Color_primaryColour = 58867;
				Horse_Color_secondaryColour = 16777215;
				Horse_Color_horseColour = 16754809;
				Horse_Color_maneColour = 8082236;
				return 1;
			case 76:
				Horse_Color_primaryColour = 4909311;
				Horse_Color_secondaryColour = 16777215;
				Horse_Color_horseColour = 5849409;
				Horse_Color_maneColour = 11623516;
				return 1;
			case 77:
				Horse_Color_primaryColour = 3700643;
				Horse_Color_secondaryColour = 7602233;
				Horse_Color_horseColour = 9852728;
				Horse_Color_maneColour = 14138263;
				return 1;
			case 78:
				Horse_Color_primaryColour = 16777215;
				Horse_Color_secondaryColour = 1017599;
				Horse_Color_horseColour = 8742735;
				Horse_Color_maneColour = 3877137;
				return 1;
			case 100:
				Horse_Color_primaryColour = 16772022;
				Horse_Color_secondaryColour = 16772022;
				Horse_Color_horseColour = 16761488;
				Horse_Color_maneColour = 3877137;
				return 1;
			case 80:
				Horse_Color_primaryColour = 7849983;
				Horse_Color_secondaryColour = 5067443;
				Horse_Color_horseColour = 8081664;
				Horse_Color_maneColour = 3815994;
				return 1;
			case 81:
				Horse_Color_primaryColour = 15913534;
				Horse_Color_secondaryColour = 7602233;
				Horse_Color_horseColour = 6444881;
				Horse_Color_maneColour = 14138263;
				return 1;
			case 82:
				Horse_Color_primaryColour = 12320733;
				Horse_Color_secondaryColour = 16775618;
				Horse_Color_horseColour = 11836798;
				Horse_Color_maneColour = 3090459;
				return 1;
			case 83:
				Horse_Color_primaryColour = 15240846;
				Horse_Color_secondaryColour = 16777215;
				Horse_Color_horseColour = 9985296;
				Horse_Color_maneColour = 3815994;
				return 1;
			case 84:
				Horse_Color_primaryColour = 14967137;
				Horse_Color_secondaryColour = 3702939;
				Horse_Color_horseColour = 3815994;
				Horse_Color_maneColour = 14207663;
				return 1;
			case 85:
				Horse_Color_primaryColour = 6343571;
				Horse_Color_secondaryColour = 3702939;
				Horse_Color_horseColour = 12353664;
				Horse_Color_maneColour = 15395562;
				return 1;
			case 86:
				Horse_Color_primaryColour = 16761374;
				Horse_Color_secondaryColour = 15018024;
				Horse_Color_horseColour = 9475214;
				Horse_Color_maneColour = 4278081;
				return 1;
			case 87:
				Horse_Color_primaryColour = 16743936;
				Horse_Color_secondaryColour = 3756172;
				Horse_Color_horseColour = 16777215;
				Horse_Color_maneColour = 16773316;
				return 1;
			case 88:
				Horse_Color_primaryColour = 2899345;
				Horse_Color_secondaryColour = 5393472;
				Horse_Color_horseColour = 16777215;
				Horse_Color_maneColour = 4210752;
				return 1;
			case 89:
				Horse_Color_primaryColour = 11645361;
				Horse_Color_secondaryColour = 16777215;
				Horse_Color_horseColour = 16771542;
				Horse_Color_maneColour = 10123632;
				return 1;
			case 90:
				Horse_Color_primaryColour = 3421236;
				Horse_Color_secondaryColour = 5958825;
				Horse_Color_horseColour = 16771542;
				Horse_Color_maneColour = 3815994;
				return 1;
			case 91:
				Horse_Color_primaryColour = 15851871;
				Horse_Color_secondaryColour = 5395026;
				Horse_Color_horseColour = 15444592;
				Horse_Color_maneColour = 7820105;
				return 1;
			case 92:
				Horse_Color_primaryColour = 16777215;
				Horse_Color_secondaryColour = 9463517;
				Horse_Color_horseColour = 7424036;
				Horse_Color_maneColour = 10129549;
				return 1;
			case 93:
				Horse_Color_primaryColour = 16760556;
				Horse_Color_secondaryColour = 16733184;
				Horse_Color_horseColour = 16767659;
				Horse_Color_maneColour = 15843765;
				return 1;
			case 94:
				Horse_Color_primaryColour = 4781311;
				Horse_Color_secondaryColour = 15771930;
				Horse_Color_horseColour = 16765601;
				Horse_Color_maneColour = 14144436;
				return 1;
			case 95:
				Horse_Color_primaryColour = 16760556;
				Horse_Color_secondaryColour = 10287103;
				Horse_Color_horseColour = 16767659;
				Horse_Color_maneColour = 6708313;
				return 1;
			case 96:
				Horse_Color_primaryColour = 13083490;
				Horse_Color_secondaryColour = 16777215;
				Horse_Color_horseColour = 9789279;
				Horse_Color_maneColour = 3815994;
				return 1;
			case 97:
				Horse_Color_primaryColour = 13810226;
				Horse_Color_secondaryColour = 9115524;
				Horse_Color_horseColour = 5855577;
				Horse_Color_maneColour = 11645361;
				return 1;
			case 98:
				Horse_Color_primaryColour = 14176336;
				Horse_Color_secondaryColour = 9115524;
				Horse_Color_horseColour = 13530742;
				Horse_Color_maneColour = 7105644;
				return 1;
			case 99:
				Horse_Color_primaryColour = 16770310;
				Horse_Color_secondaryColour = 16751169;
				Horse_Color_horseColour = 16772294;
				Horse_Color_maneColour = 16777215;
				return 1;
			default:
				Horse_Color_primaryColour = -1;
				Horse_Color_secondaryColour = -1;
				Horse_Color_horseColour = -1;
				Horse_Color_maneColour = -1;
				return 0;
			}
		}

		public static int END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE()
		{
			return Function.Call<int>(Hash.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE);
		}

		public static bool IS_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_READY(int ID)
		{
			return Function.Call<bool>(Hash.IS_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_READY, ID);
		}

		public int GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_INT(int ID)
		{
			return Function.Call<int>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_INT, ID);
		}

		public bool GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_BOOL(int ID)
		{
			return Function.Call<bool>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_BOOL, ID);
		}

		public static bool IS_NAMED_RENDERTARGET_REGISTERED(string S)
		{
			return Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, S);
		}

		public static bool IS_NAMED_RENDERTARGET_LINKED(int S)
		{
			return Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_LINKED, S);
		}

		public static void REGISTER_NAMED_RENDERTARGET(string s, int i)
		{
			Function.Call(Hash.REGISTER_NAMED_RENDERTARGET, s, i);
		}

		public static void LINK_NAMED_RENDERTARGET(int i)
		{
			Function.Call(Hash.LINK_NAMED_RENDERTARGET, i);
		}

		public static void BEGIN_SCALEFORM_MOVIE_METHOD(int scaleform, string methodName)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, scaleform, methodName);
		}

		public static void _ADD_SCALEFORM_MOVIE_METHOD_PARAMETER_INT(int i)
		{
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, i);
		}

		public static void END_SCALEFORM_MOVIE_METHOD()
		{
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		public static void SET_TEXT_RENDER_ID(int renderId)
		{
			Function.Call(Hash.SET_TEXT_RENDER_ID, renderId);
		}

		public static void FuncSET_SCRIPT_GFX_DRAW_ORDER(int drawOrder)
		{
			Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, drawOrder);
		}

		public static void Func_0xC6372ECD45D73BCD(int b)
		{
			Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, b);
		}

		public static void Func_0x2BC54A8188768488(string textureDict, string textureName, float screenX, float screenY, float width, float height, float heading, int red, int green, int blue, int alpha)
		{
			Function.Call(Hash.DRAW_SPRITE_NAMED_RENDERTARGET, textureDict, textureName, screenX, screenY, width, height, heading, red, green, blue, alpha);
		}

		public static void _SCREEN_DRAW_POSITION_BEGIN(int horizontalAlign, int verticalAlign)
		{
			Function.Call(Hash.SET_SCRIPT_GFX_ALIGN, horizontalAlign, verticalAlign);
		}

		public static void _SCREEN_DRAW_POSITION_END()
		{
			Function.Call(Hash.RESET_SCRIPT_GFX_ALIGN);
		}

		public static void RELEASE_NAMED_RENDERTARGET(string name)
		{
			Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, name);
		}

		public static void CreateCasscreen()
		{
			switch (CasStage)
			{
			case 0:
				DeleteCasScreens();
				CasStage = 1;
				break;
			case 1:
				CasMovie = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "HORSE_RACING_WALL");
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_02"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_02", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(-214651601))
				{
					LINK_NAMED_RENDERTARGET(-214651601);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_03"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_03", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(903186242))
				{
					LINK_NAMED_RENDERTARGET(903186242);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_04"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_04", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(1144202237))
				{
					LINK_NAMED_RENDERTARGET(1144202237);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_05"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_05", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(-519873125))
				{
					LINK_NAMED_RENDERTARGET(-519873125);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_06"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_06", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(-292161344))
				{
					LINK_NAMED_RENDERTARGET(-292161344);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_07"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_07", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(2004912791))
				{
					LINK_NAMED_RENDERTARGET(2004912791);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_08"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_08", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(-1960136213))
				{
					LINK_NAMED_RENDERTARGET(-1960136213);
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_09"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_09", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(-1003510800))
				{
					LINK_NAMED_RENDERTARGET(-1003510800);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_10"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_10", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(425283146))
				{
					LINK_NAMED_RENDERTARGET(425283146);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_11"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_11", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(-563586975))
				{
					LINK_NAMED_RENDERTARGET(-563586975);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_12"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_12", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(1712925937))
				{
					LINK_NAMED_RENDERTARGET(1712925937);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_13"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_13", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(796540856))
				{
					LINK_NAMED_RENDERTARGET(796540856);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_14"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_14", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(499686485))
				{
					LINK_NAMED_RENDERTARGET(499686485);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_15"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_15", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(1207038119))
				{
					LINK_NAMED_RENDERTARGET(1207038119);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_16"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_16", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(977491274))
				{
					LINK_NAMED_RENDERTARGET(977491274);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_17"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_17", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(-424104402))
				{
					LINK_NAMED_RENDERTARGET(-424104402);
				}
				if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_18"))
				{
					REGISTER_NAMED_RENDERTARGET("CasinoScreen_18", 0);
				}
				if (!IS_NAMED_RENDERTARGET_LINKED(-731707005))
				{
					LINK_NAMED_RENDERTARGET(-731707005);
				}
				if (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "Prop_Screen_VW_InsideTrack"))
				{
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "Prop_Screen_VW_InsideTrack", 0);
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_18") && IS_NAMED_RENDERTARGET_LINKED(-731707005))
				{
					CasScreen = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_02");
					CasScreen1 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_03");
					CasScreen2 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_04");
					CasScreen3 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_05");
					CasScreen4 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_06");
					CasScreen5 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_07");
					CasScreen6 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_08");
					CasScreen7 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_09");
					CasScreen8 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_10");
					CasScreen9 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_11");
					CasScreen10 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_12");
					CasScreen11 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_13");
					CasScreen12 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_14");
					CasScreen13 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_15");
					CasScreen14 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_16");
					CasScreen15 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_17");
					CasScreen16 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "CasinoScreen_18");
					if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "Prop_Screen_VW_InsideTrack"))
					{
						CasStage = 2;
					}
				}
				break;
			case 2:
				if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, CasMovie))
				{
					if (!IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_02"))
					{
						REGISTER_NAMED_RENDERTARGET("CasinoScreen_02", 0);
					}
					if (!IS_NAMED_RENDERTARGET_LINKED(-214651601))
					{
						LINK_NAMED_RENDERTARGET(-214651601);
					}
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, CasMovie, "SHOW_SCREEN");
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 1);
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					Function.Call(Hash.SET_SCALEFORM_MOVIE_TO_USE_SUPER_LARGE_RT, CasMovie, true);
					CasStage = 3;
				}
				else
				{
					CasMovie = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "HORSE_RACING_WALL");
				}
				break;
			case 3:
				SET_TEXT_RENDER_ID(CasScreen1);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen2);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_GENERIC_ORANGE", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen3);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_GENERIC_PURPLE", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen4);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_MAIN", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen5);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_SINGLE", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen6);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_GENERIC_ORANGE", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen7);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_GENERIC_PURPLE", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen8);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen9);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_MAIN", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen10);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_SINGLE", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen11);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_GENERIC_ORANGE", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen12);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_GENERIC_PURPLE", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen13);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_SINGLE", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen14);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_MAIN", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen15);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_GENERIC_PURPLE", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen16);
				_SCREEN_DRAW_POSITION_BEGIN(73, 73);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Func_0x2BC54A8188768488("Prop_Screen_VW_InsideTrack", "BETTING_SINGLE", 0.5f, 0.5f, 1f, 1f, 1f, 255, 255, 255, 255);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				_SCREEN_DRAW_POSITION_END();
				SET_TEXT_RENDER_ID(CasScreen);
				FuncSET_SCRIPT_GFX_DRAW_ORDER(4);
				Func_0xC6372ECD45D73BCD(1);
				Function.Call(Hash.DRAW_SCALEFORM_MOVIE, CasMovie, 0.5f, 0.5f, 1f, 1f, 255, 255, 255, 255, 0);
				SET_TEXT_RENDER_ID(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				break;
			}
		}

		public static void DeleteCasScreens()
		{
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_02"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_02");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_03"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_03");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_04"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_04");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_05"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_05");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_06"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_06");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_07"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_07");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_08"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_08");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_09"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_09");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_10"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_10");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_11"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_11");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_12"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_12");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_13"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_13");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_14"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_14");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_15"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_15");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_16"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_16");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_17"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_17");
			}
			if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_18"))
			{
				RELEASE_NAMED_RENDERTARGET("CasinoScreen_18");
			}
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "Prop_Screen_VW_InsideTrack"))
			{
				Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, "Prop_Screen_VW_InsideTrack");
			}
		}

		public static void DeleteCasScreens1()
		{
			if (!CleanUpCas)
			{
				return;
			}
			Ped ped = Function.Call<Ped>(Hash.PLAYER_PED_ID);
			int num = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, ped);
			if (num == 0 || num != Interior107)
			{
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_02"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_02");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_03"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_03");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_04"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_04");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_05"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_05");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_06"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_06");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_07"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_07");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_08"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_08");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_09"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_09");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_10"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_10");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_11"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_11");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_12"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_12");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_13"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_13");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_14"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_14");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_15"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_15");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_16"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_16");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_17"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_17");
				}
				if (IS_NAMED_RENDERTARGET_REGISTERED("CasinoScreen_18"))
				{
					RELEASE_NAMED_RENDERTARGET("CasinoScreen_18");
				}
				if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "Prop_Screen_VW_InsideTrack"))
				{
					Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, "Prop_Screen_VW_InsideTrack");
				}
				CleanUpCas = false;
			}
		}

		public static string ROULETTE_AUDIO_GET_WINNING_NUMBER_CLIP(int iVariation)
		{
			return $"MINIGAME_DEALER_WINNING_NUMBER_{iVariation}";
		}

		public static string ROULETTE_AUDIO_GET_BALL_EXIT_VARIATION(int iVariation)
		{
			return $"DLC_VW_ROULETTE_BALL_EXIT_{iVariation}";
		}

		public static string ROULETTE_AUDIO_GET_CLIP(ROULETTE_AUDIO_CLIP eAudioClip, int iVariation = 0)
		{
			switch (eAudioClip)
			{
			case ROULETTE_AUDIO_CLIP.RAC_DEALER_GREET_PLAYER:
				return "MINIGAME_DEALER_GREET";
			case ROULETTE_AUDIO_CLIP.RAC_DEALER_GREET_DRUNK_PLAYER:
				return "MINIGAME_DEALER_GREET_DRUNK";
			case ROULETTE_AUDIO_CLIP.RAC_DEALER_GREET_MALE_PLAYER:
				return "MINIGAME_DEALER_GREET_MALE";
			case ROULETTE_AUDIO_CLIP.RAC_DEALER_GREET_FEMALE_PLAYER:
				return "MINIGAME_DEALER_GREET_FEMALE";
			case ROULETTE_AUDIO_CLIP.RAC_DEALER_GREET_REJOINING_PLAYER:
				return "MINIGAME_DEALER_REJOIN_TABLE";
			case ROULETTE_AUDIO_CLIP.RAC_DEALER_LEAVE_NEUTRAL:
				return "MINIGAME_DEALER_LEAVE_NEUTRAL_GAME";
			case ROULETTE_AUDIO_CLIP.RAC_DEALER_LEAVE_GOOD:
				return "MINIGAME_DEALER_LEAVE_GOOD_GAME";
			case ROULETTE_AUDIO_CLIP.RAC_DEALER_LEAVE_BAD:
				return "MINIGAME_DEALER_LEAVE_BAD_GAME";
			case ROULETTE_AUDIO_CLIP.RAC_DEALER_CALL_WINNING_NUMBER:
				return ROULETTE_AUDIO_GET_WINNING_NUMBER_CLIP(iVariation);
			case ROULETTE_AUDIO_CLIP.RAC_DEALER_PLACE_YOUR_BETS:
				return "MINIGAME_DEALER_PLACE_BET";
			case ROULETTE_AUDIO_CLIP.RAC_DEALER_REFUSE_BETS:
				return "MINIGAME_DEALER_REFUSE_BETS";
			case ROULETTE_AUDIO_CLIP.RAC_DEALER_NO_MORE_BETS:
				return "MINIGAME_DEALER_CLOSED_BETS";
			case ROULETTE_AUDIO_CLIP.RAC_DEALER_ANOTHER_ROUND:
				return "MINIGAME_DEALER_ANOTHER_GO";
			case ROULETTE_AUDIO_CLIP.RAC_SFX_INVALID_ACTION:
				return "DLC_VW_ERROR_MAX";
			case ROULETTE_AUDIO_CLIP.RAC_SFX_HIGHLIGHT:
				return "DLC_VW_BET_HIGHLIGHT";
			case ROULETTE_AUDIO_CLIP.RAC_SFX_ADJUST_CHIPS_UP:
				return "DLC_VW_BET_UP";
			case ROULETTE_AUDIO_CLIP.RAC_SFX_ADJUST_CHIPS_DOWN:
				return "DLC_VW_BET_DOWN";
			case ROULETTE_AUDIO_CLIP.RAC_SFX_SHOW_RULES:
				return "DLC_VW_RULES";
			case ROULETTE_AUDIO_CLIP.RAC_SFX_CONTINUE:
				return "DLC_VW_CONTINUE";
			case ROULETTE_AUDIO_CLIP.RAC_SFX_BET_MAX:
				return "DLC_VW_BET_MAX";
			case ROULETTE_AUDIO_CLIP.RAC_SFX_REMOVE_BET:
				return "DLC_VW_REMOVE_BET";
			case ROULETTE_AUDIO_CLIP.RAC_SFX_WIN:
				return "DLC_VW_WIN_CHIPS";
			case ROULETTE_AUDIO_CLIP.RAC_SFX_PLACE_CHIPS:
				return "DLC_VW_BET_50";
			case ROULETTE_AUDIO_CLIP.RAC_SFX_PLACE_CHIPS_MODEL_SWAP:
				return "DLC_VW_BET_100";
			case ROULETTE_AUDIO_CLIP.RAC_SFX_CLEAR_CHIPS_SMALL:
				return "DLC_VW_CHIP_DRAG_POT_SML";
			case ROULETTE_AUDIO_CLIP.RAC_SFX_CLEAR_CHIPS_MEDIUM:
				return "DLC_VW_CHIP_DRAG_POT_MED";
			case ROULETTE_AUDIO_CLIP.RAC_SFX_CLEAR_CHIPS_LARGE:
				return "DLC_VW_CHIP_DRAG_POT_LRG";
			case ROULETTE_AUDIO_CLIP.RAC_SFX_BALL_LOOP:
				return "DLC_VW_ROULETTE_BALL_LOOP";
			case ROULETTE_AUDIO_CLIP.RAC_SFX_BALL_EXIT:
				return ROULETTE_AUDIO_GET_BALL_EXIT_VARIATION(iVariation);
			default:
				return "";
			}
		}

		public static string GET_CHIPS_SOUND_NAME(int iBetValue)
		{
			switch (iBetValue)
			{
			case 10:
				return "DLC_VW_CHIP_BET_SML_SINGLE";
			case 20:
				return "DLC_VW_CHIP_BET_SML_SMALL";
			case 30:
				return "DLC_VW_CHIP_BET_SML_SMALL";
			case 40:
				return "DLC_VW_CHIP_BET_SML_MEDIUM";
			case 50:
				return "DLC_VW_CHIP_BET_SML_MEDIUM";
			case 60:
				return "DLC_VW_CHIP_BET_SML_MEDIUM";
			case 70:
				return "DLC_VW_CHIP_BET_SML_LARGE";
			case 80:
				return "DLC_VW_CHIP_BET_SML_LARGE";
			case 90:
				return "DLC_VW_CHIP_BET_SML_LARGE";
			case 100:
				return "DLC_VW_CHIP_BET_SML_SINGLE";
			case 150:
				return "DLC_VW_CHIP_BET_SML_SMALL";
			case 200:
				return "DLC_VW_CHIP_BET_SML_SMALL";
			case 250:
				return "DLC_VW_CHIP_BET_SML_MEDIUM";
			case 300:
				return "DLC_VW_CHIP_BET_SML_SMALL";
			case 350:
				return "DLC_VW_CHIP_BET_SML_LARGE";
			case 400:
				return "DLC_VW_CHIP_BET_SML_MEDIUM";
			case 450:
				return "DLC_VW_CHIP_BET_SML_LARGE";
			case 500:
				return "DLC_VW_CHIP_BET_SML_SINGLE";
			case 1000:
				return "DLC_VW_CHIP_BET_SML_SINGLE";
			case 1500:
				return "DLC_VW_CHIP_BET_SML_SMALL";
			case 2000:
				return "DLC_VW_CHIP_BET_SML_SMALL";
			case 2500:
				return "DLC_VW_CHIP_BET_SML_MEDIUM";
			case 3000:
				return "DLC_VW_CHIP_BET_SML_SMALL";
			case 3500:
				return "DLC_VW_CHIP_BET_SML_MEDIUM";
			case 4000:
				return "DLC_VW_CHIP_BET_SML_MEDIUM";
			case 4500:
				return "DLC_VW_CHIP_BET_SML_LARGE";
			case 5000:
				return "DLC_VW_CHIP_BET_LRG_SINGLE";
			case 6000:
				return "DLC_VW_CHIP_BET_SML_MEDIUM";
			case 7000:
				return "DLC_VW_CHIP_BET_SML_LARGE";
			case 8000:
				return "DLC_VW_CHIP_BET_SML_LARGE";
			case 9000:
				return "DLC_VW_CHIP_BET_SML_LARGE";
			case 10000:
				return "DLC_VW_CHIP_BET_LRG_SINGLE";
			case 15000:
				return "DLC_VW_CHIP_BET_LRG_SMALL";
			case 20000:
				return "DLC_VW_CHIP_BET_LRG_SMALL";
			case 25000:
				return "DLC_VW_CHIP_BET_LRG_MEDIUM";
			case 30000:
				return "DLC_VW_CHIP_BET_LRG_SMALL";
			case 35000:
				return "DLC_VW_CHIP_BET_LRG_MEDIUM";
			case 40000:
				return "DLC_VW_CHIP_BET_LRG_MEDIUM";
			case 45000:
				return "DLC_VW_CHIP_BET_LRG_LARGE";
			case 50000:
				return "DLC_VW_CHIP_BET_LRG_MEDIUM";
			default:
				return "";
			}
		}

		public static void ROULETTE_AUDIO_PLAY_SFX_INVALID_ACTION()
		{
			Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, ROULETTE_AUDIO_GET_CLIP(ROULETTE_AUDIO_CLIP.RAC_SFX_INVALID_ACTION), ROULETTE_AUDIO_GET_SOUND_SET(ROULETTE_AUDIO_SET.RAS_FRONTEND_SOUNDS));
		}

		public static void ROULETTE_AUDIO_PLAY_SFX_HIGHLIGHT()
		{
			Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, ROULETTE_AUDIO_GET_CLIP(ROULETTE_AUDIO_CLIP.RAC_SFX_HIGHLIGHT), ROULETTE_AUDIO_GET_SOUND_SET(ROULETTE_AUDIO_SET.RAS_FRONTEND_SOUNDS));
		}

		public static void ROULETTE_AUDIO_PLAY_SFX_ADJUST_CHIPS_UP()
		{
			Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, ROULETTE_AUDIO_GET_CLIP(ROULETTE_AUDIO_CLIP.RAC_SFX_ADJUST_CHIPS_UP), ROULETTE_AUDIO_GET_SOUND_SET(ROULETTE_AUDIO_SET.RAS_FRONTEND_SOUNDS));
		}

		public static void ROULETTE_AUDIO_PLAY_SFX_ADJUST_CHIPS_DOWN()
		{
			Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, ROULETTE_AUDIO_GET_CLIP(ROULETTE_AUDIO_CLIP.RAC_SFX_ADJUST_CHIPS_DOWN), ROULETTE_AUDIO_GET_SOUND_SET(ROULETTE_AUDIO_SET.RAS_FRONTEND_SOUNDS));
		}

		public static void ROULETTE_AUDIO_PLAY_SFX_CONTINUE()
		{
			Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, ROULETTE_AUDIO_GET_CLIP(ROULETTE_AUDIO_CLIP.RAC_SFX_CONTINUE), ROULETTE_AUDIO_GET_SOUND_SET(ROULETTE_AUDIO_SET.RAS_FRONTEND_SOUNDS));
		}

		public static void ROULETTE_AUDIO_PLAY_SFX_BET_MAX()
		{
			Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, ROULETTE_AUDIO_GET_CLIP(ROULETTE_AUDIO_CLIP.RAC_SFX_BET_MAX), ROULETTE_AUDIO_GET_SOUND_SET(ROULETTE_AUDIO_SET.RAS_FRONTEND_SOUNDS));
		}

		public static void ROULETTE_AUDIO_PLAY_SFX_REMOVE_BET()
		{
			Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, ROULETTE_AUDIO_GET_CLIP(ROULETTE_AUDIO_CLIP.RAC_SFX_REMOVE_BET), ROULETTE_AUDIO_GET_SOUND_SET(ROULETTE_AUDIO_SET.RAS_FRONTEND_SOUNDS));
		}

		public static void ROULETTE_AUDIO_PLAY_SFX_WIN()
		{
			Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, ROULETTE_AUDIO_GET_CLIP(ROULETTE_AUDIO_CLIP.RAC_SFX_WIN), ROULETTE_AUDIO_GET_SOUND_SET(ROULETTE_AUDIO_SET.RAS_FRONTEND_SOUNDS));
		}

		public static void ROULETTE_AUDIO_PLAY_SFX_PLACE_CHIPS(Vector3 vPos)
		{
			Function.Call(Hash.PLAY_SOUND_FROM_COORD, -1, ROULETTE_AUDIO_GET_CLIP(ROULETTE_AUDIO_CLIP.RAC_SFX_PLACE_CHIPS), vPos.X, vPos.Y, vPos.Z, ROULETTE_AUDIO_GET_SOUND_SET(ROULETTE_AUDIO_SET.RAS_TABLE_GAME_SOUNDS), false);
		}

		public static void ROULETTE_AUDIO_PLAY_SFX_PLACE_CHIPS_SWAPPED_MODEL(Vector3 vPos)
		{
			Function.Call(Hash.PLAY_SOUND_FROM_COORD, -1, ROULETTE_AUDIO_GET_CLIP(ROULETTE_AUDIO_CLIP.RAC_SFX_PLACE_CHIPS_MODEL_SWAP), vPos.X, vPos.Y, vPos.Z, ROULETTE_AUDIO_GET_SOUND_SET(ROULETTE_AUDIO_SET.RAS_TABLE_GAME_SOUNDS), false);
		}

		public static void ROULETTE_AUDIO_PLAY_SFX_CLEAR_CHIPS_SMALL(Vector3 vPos)
		{
			Function.Call(Hash.PLAY_SOUND_FROM_COORD, -1, ROULETTE_AUDIO_GET_CLIP(ROULETTE_AUDIO_CLIP.RAC_SFX_CLEAR_CHIPS_SMALL), vPos.X, vPos.Y, vPos.Z, ROULETTE_AUDIO_GET_SOUND_SET(ROULETTE_AUDIO_SET.RAS_TABLE_GAME_SOUNDS), false);
		}

		public static void ROULETTE_AUDIO_PLAY_SFX_CLEAR_CHIPS_MEDIUM(Vector3 vPos)
		{
			Function.Call(Hash.PLAY_SOUND_FROM_COORD, -1, ROULETTE_AUDIO_GET_CLIP(ROULETTE_AUDIO_CLIP.RAC_SFX_CLEAR_CHIPS_MEDIUM), vPos.X, vPos.Y, vPos.Z, ROULETTE_AUDIO_GET_SOUND_SET(ROULETTE_AUDIO_SET.RAS_TABLE_GAME_SOUNDS), false);
		}

		public static void ROULETTE_AUDIO_PLAY_SFX_CLEAR_CHIPS_LARGE(Vector3 vPos)
		{
			Function.Call(Hash.PLAY_SOUND_FROM_COORD, -1, ROULETTE_AUDIO_GET_CLIP(ROULETTE_AUDIO_CLIP.RAC_SFX_CLEAR_CHIPS_LARGE), vPos.X, vPos.Y, vPos.Z, ROULETTE_AUDIO_GET_SOUND_SET(ROULETTE_AUDIO_SET.RAS_TABLE_GAME_SOUNDS), false);
		}

		public static string ROULETTE_AUDIO_GET_SOUND_SET(ROULETTE_AUDIO_SET eSoundSet)
		{
			switch (eSoundSet)
			{
			case ROULETTE_AUDIO_SET.RAS_TABLE_GAME_SOUNDS:
				return "dlc_vw_table_games_sounds";
			case ROULETTE_AUDIO_SET.RAS_FRONTEND_SOUNDS:
				return "dlc_vw_table_games_frontend_sounds";
			case ROULETTE_AUDIO_SET.RAS_BALL_EXIT_SOUNDS:
				return "dlc_vw_table_games_roulette_exit_sounds";
			default:
				return "";
			}
		}

		public static void PLAY_SOUND_CHIPS_PLACED(Prop Chips, int iRealBetValue)
		{
			Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, GET_CHIPS_SOUND_NAME(iRealBetValue), Chips, "dlc_vw_table_games_sounds");
		}

		public static Vector3 GetCardDeckPosition()
		{
			return Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "anim_casino_b@amb@casino@games@threecardpoker@dealer", "deck_pick_up_deck", GetDealerAnimOrigin().X, GetDealerAnimOrigin().Y, GetDealerAnimOrigin().Z, 0f, 0f, ClosestDealerTable.Table.Rotation.Z, 0.01f);
		}

		public static Vector3 GetCardDeckRotation()
		{
			return Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "anim_casino_b@amb@casino@games@threecardpoker@dealer", "deck_pick_up_deck", GetDealerAnimOrigin().X, GetDealerAnimOrigin().Y, GetDealerAnimOrigin().Z, 0f, 0f, ClosestDealerTable.Table.Rotation.Z, 0.01f);
		}

		public static string GET_DEALER_DECK_PICK_UP_CLIP(Ped Dealer)
		{
			if (Dealer == null)
			{
				return "";
			}
			if (Dealer.Gender == Gender.Female)
			{
				return "female_deck_pick_up";
			}
			return "deck_pick_up";
		}

		public static string GET_DEALER_DECK_PUT_DOWN_CLIP(Ped Dealer)
		{
			if (Dealer == null)
			{
				return "";
			}
			if (Dealer.Gender == Gender.Female)
			{
				return "female_deck_put_down";
			}
			return "deck_put_down";
		}

		public static string GET_DEALER_SHUFFLE_CLIP(Ped Dealer)
		{
			if (Dealer == null)
			{
				return "deck_shuffle";
			}
			if (Dealer.Gender == Gender.Female)
			{
				return "female_deck_shuffle";
			}
			return "deck_shuffle";
		}

		public static string GET_DEALER_RETRIEVE_01_CLIP(Ped Dealer)
		{
			if (Dealer == null)
			{
				return "deck_shuffle";
			}
			if (Dealer.Gender == Gender.Female)
			{
				return "female_retrieve_cards_player_01";
			}
			return "retrieve_cards_player_01";
		}

		public static string GET_DEALER_RETRIEVE_02_CLIP(Ped Dealer)
		{
			if (Dealer == null)
			{
				return "deck_shuffle";
			}
			if (Dealer.Gender == Gender.Female)
			{
				return "female_retrieve_cards_player_02";
			}
			return "retrieve_cards_player_02";
		}

		public static string GET_DEALER_RETRIEVE_03_CLIP(Ped Dealer)
		{
			if (Dealer == null)
			{
				return "deck_shuffle";
			}
			if (Dealer.Gender == Gender.Female)
			{
				return "female_retrieve_cards_player_03";
			}
			return "retrieve_cards_player_03";
		}

		public static string GET_DEALER_RETRIEVE_04_CLIP(Ped Dealer)
		{
			if (Dealer == null)
			{
				return "deck_shuffle";
			}
			if (Dealer.Gender == Gender.Female)
			{
				return "female_retrieve_cards_player_04";
			}
			return "retrieve_cards_player_04";
		}

		public static void Poker_ManageDealerCardDeck()
		{
			bool flag = false;
			if (!(ClosestDealer != null))
			{
				return;
			}
			if (DealerDeckOfCards == null)
			{
				CreateCardDeck();
			}
			if (!(DealerDeckOfCards != null))
			{
				return;
			}
			if (!DealerDeckOfCards.IsAttached())
			{
				if (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, ClosestDealer, "anim_casino_b@amb@casino@games@threecardpoker@dealer", GET_DEALER_DECK_PICK_UP_CLIP(ClosestDealer), 3))
				{
					if (Function.Call<bool>(Hash.HAS_ANIM_EVENT_FIRED, ClosestDealer, Function.Call<int>(Hash.GET_HASH_KEY, "Pick_Up_Deck")))
					{
						flag = true;
					}
				}
				else
				{
					flag = false;
				}
				if ((flag || Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, ClosestDealer, "anim_casino_b@amb@casino@games@threecardpoker@dealer", GET_DEALER_SHUFFLE_CLIP(ClosestDealer), 3) || Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, ClosestDealer, "anim_casino_b@amb@casino@games@threecardpoker@dealer", IsDealerFemale() + "deck_deal_p02", 3) || Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, ClosestDealer, "anim_casino_b@amb@casino@games@threecardpoker@dealer", IsDealerFemale() + "deck_deal_self", 3)) && !Function.Call<bool>(Hash.IS_ENTITY_ATTACHED_TO_ANY_PED, DealerDeckOfCards))
				{
					Function.Call(Hash.FREEZE_ENTITY_POSITION, DealerDeckOfCards, false);
					Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, DealerDeckOfCards, ClosestDealer, Function.Call<int>(Hash.GET_PED_BONE_INDEX, ClosestDealer, 60309), 0f, 0f, 0f, 0f, 0f, 0f, true, true, false, true, 1, true);
				}
			}
			else if (((Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, ClosestDealer, "anim_casino_b@amb@casino@games@threecardpoker@dealer", GET_DEALER_DECK_PUT_DOWN_CLIP(ClosestDealer), 3) && Function.Call<bool>(Hash.HAS_ANIM_EVENT_FIRED, ClosestDealer, Function.Call<int>(Hash.GET_HASH_KEY, "Put_Down_Deck"))) || (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, ClosestDealer, "anim_casino_b@amb@casino@games@threecardpoker@dealer", IsDealerFemale() + "deck_put_down", 3) && !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, ClosestDealer, "anim_casino_b@amb@casino@games@threecardpoker@dealer", GET_DEALER_DECK_PICK_UP_CLIP(ClosestDealer), 3) && !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, ClosestDealer, "anim_casino_b@amb@casino@games@threecardpoker@dealer", GET_DEALER_SHUFFLE_CLIP(ClosestDealer), 3))) && Function.Call<bool>(Hash.IS_ENTITY_ATTACHED_TO_ANY_PED, DealerDeckOfCards))
			{
				Function.Call(Hash.DETACH_ENTITY, DealerDeckOfCards);
				Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, DealerDeckOfCards, GetCardDeckPosition().X, GetCardDeckPosition().Y, GetCardDeckPosition().Z);
				Function.Call(Hash.SET_ENTITY_ROTATION, DealerDeckOfCards, GetCardDeckRotation().X, GetCardDeckRotation().Y, GetCardDeckRotation().Z);
				Function.Call(Hash.FREEZE_ENTITY_POSITION, DealerDeckOfCards, true);
			}
		}

		public static void Blackjack_ManageDealerCards()
		{
			if (TempCard == null)
			{
				TempCard = GlobalHelper.CreateProp(RequestModel("vw_prop_casino_cards_single"), new Vector3(0f, 0f, 0f), false, false);
				return;
			}
			TempCard.Opacity = 0;
			if (ClosestDealer != null)
			{
				if (Function.Call<bool>(Hash.HAS_ANIM_EVENT_FIRED, ClosestDealer, Function.Call<int>(Hash.GET_HASH_KEY, "HideCard")) && TempCard.IsAttached())
				{
					DetatchAllCards();
					TempCard.Detach();
					TempCard.IsPositionFrozen = true;
				}
				if (Function.Call<bool>(Hash.HAS_ANIM_EVENT_FIRED, ClosestDealer, Function.Call<int>(Hash.GET_HASH_KEY, "ShowCard")) && !TempCard.IsAttached())
				{
					Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, TempCard, ClosestDealer, ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
				}
			}
		}

		public static void CreateCardDeck()
		{
			string text = "";
			text = ((PlayerDeck != 1 && PlayerDeck != 0 && PlayerDeck >= 0 && PlayerDeck <= 3) ? "vw_prop_vw_casino_cards_01" : "vw_prop_casino_cards_01");
			LoadDict("anim_casino_b@amb@casino@games@threecardpoker@dealer");
			DealerDeckOfCards = GlobalHelper.CreateProp(RequestModel(text), new Vector3(0f, 0f, 0f), false, false);
			if (DealerDeckOfCards != null && DealerDeckOfCards.Exists())
			{
				Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, DealerDeckOfCards, GetCardDeckPosition().X, GetCardDeckPosition().Y, GetCardDeckPosition().Z);
				Function.Call(Hash.SET_ENTITY_ROTATION, DealerDeckOfCards, GetCardDeckRotation().X, GetCardDeckRotation().Y, GetCardDeckRotation().Z);
				DealerDeckOfCards.IsPositionFrozen = true;
			}
		}

		public static void DeleteCardDeck()
		{
			if (DealerDeckOfCards != null)
			{
				DealerDeckOfCards.Delete();
				DealerDeckOfCards = null;
			}
		}

		public static string IsDealerFemale()
		{
			string result = "";
			if (ClosestDealer != null && ClosestDealer.Gender == Gender.Female)
			{
				result = "female_";
			}
			return result;
		}

		public unsafe static void OpenSequenceTask(int* taskID)
		{
			Function.Call(Hash.OPEN_SEQUENCE_TASK, taskID);
		}

		public static void TaskPedSlideToCoord(Ped ped, Vector3 pos, float heading, float speed = 0.75f)
		{
			Function.Call(Hash.TASK_PED_SLIDE_TO_COORD, ped, pos.X, pos.Y, pos.Z, heading, speed);
		}

		public static void TaskPedSlideToCoord(int ped, Vector3 pos, float heading, float speed = 0.75f)
		{
			Function.Call(Hash.TASK_PED_SLIDE_TO_COORD, ped, pos.X, pos.Y, pos.Z, heading, speed);
		}

		public static void CloseSequenceTask(int taskID)
		{
			Function.Call(Hash.CLOSE_SEQUENCE_TASK, taskID);
		}

		public static void SetSequenceRepeat(int taskID, bool repeat)
		{
			Function.Call(Hash.SET_SEQUENCE_TO_REPEAT, taskID, repeat);
		}

		public unsafe static void ClearSequenceTask(int* taskID)
		{
			Function.Call(Hash.CLEAR_SEQUENCE_TASK, taskID);
		}

		public static void TaskPerformSequence(Ped ped, int taskID)
		{
			Function.Call(Hash.TASK_PERFORM_SEQUENCE, ped, taskID);
		}

		public static void ShowCards(bool Debug)
		{
			if (PlayerCardA != null)
			{
				PlayerCardA.Position = ClosestDealerTable.Table.GetOffsetPosition(GetPlayerCardPositionOffset(0, SeatSittingIn - 1)) + new Vector3(0f, 0f, 0.01f);
				PlayerCardA.Rotation = ClosestDealerTable.Table.Rotation + GetPlayerCardRotationOffset(0, SeatSittingIn - 1);
				if (PlayerCardA.Opacity != 0 && Debug)
				{
					World.DrawMarker(MarkerType.Cylinder, PlayerCardA.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.005f, 0.005f, 0.5f), Color.MediumSlateBlue);
				}
			}
			if (PlayerCardB != null)
			{
				PlayerCardB.Position = ClosestDealerTable.Table.GetOffsetPosition(GetPlayerCardPositionOffset(1, SeatSittingIn - 1, BlackJackSplit)) + new Vector3(0f, 0f, 0.01f);
				PlayerCardB.Rotation = ClosestDealerTable.Table.Rotation + GetPlayerCardRotationOffset(1, SeatSittingIn - 1, BlackJackSplit);
				if (PlayerCardB.Opacity != 0 && Debug)
				{
					World.DrawMarker(MarkerType.Cylinder, PlayerCardB.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.005f, 0.005f, 0.5f), Color.GhostWhite);
				}
			}
			if (PlayerCardC != null)
			{
				PlayerCardC.Position = ClosestDealerTable.Table.GetOffsetPosition(GetPlayerCardPositionOffset(2, SeatSittingIn - 1)) + new Vector3(0f, 0f, 0.01f);
				PlayerCardC.Rotation = ClosestDealerTable.Table.Rotation + GetPlayerCardRotationOffset(2, SeatSittingIn - 1);
				if (PlayerCardC.Opacity != 0 && Debug)
				{
					World.DrawMarker(MarkerType.Cylinder, PlayerCardC.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.005f, 0.005f, 0.5f), Color.Navy);
				}
			}
			if (PlayerCardD != null)
			{
				PlayerCardD.Position = ClosestDealerTable.Table.GetOffsetPosition(GetPlayerCardPositionOffset(3, SeatSittingIn - 1, BlackJackSplit)) + new Vector3(0f, 0f, 0.01f);
				PlayerCardD.Rotation = ClosestDealerTable.Table.Rotation + GetPlayerCardRotationOffset(3, SeatSittingIn - 1, BlackJackSplit);
				if (PlayerCardD.Opacity != 0 && Debug)
				{
					World.DrawMarker(MarkerType.Cylinder, PlayerCardD.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.005f, 0.005f, 0.5f), Color.SkyBlue);
				}
			}
			if (PlayerCardE != null)
			{
				PlayerCardE.Position = ClosestDealerTable.Table.GetOffsetPosition(GetPlayerCardPositionOffset(4, SeatSittingIn - 1)) + new Vector3(0f, 0f, 0.01f);
				PlayerCardE.Rotation = ClosestDealerTable.Table.Rotation + GetPlayerCardRotationOffset(4, SeatSittingIn - 1, false, BlackJackSplit);
				if (PlayerCardE.Opacity != 0 && Debug)
				{
					World.DrawMarker(MarkerType.Cylinder, PlayerCardE.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.005f, 0.005f, 0.5f), Color.Orange);
				}
			}
			if (PlayerCardF != null)
			{
				PlayerCardF.Position = ClosestDealerTable.Table.GetOffsetPosition(GetPlayerCardPositionOffset(5, SeatSittingIn - 1, BlackJackSplit)) + new Vector3(0f, 0f, 0.01f);
				PlayerCardF.Rotation = ClosestDealerTable.Table.Rotation + GetPlayerCardRotationOffset(5, SeatSittingIn - 1, BlackJackSplit, true);
				if (PlayerCardF.Opacity != 0 && Debug)
				{
					World.DrawMarker(MarkerType.Cylinder, PlayerCardF.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.005f, 0.005f, 0.5f), Color.DarkGoldenrod);
				}
			}
			if (ClosestDealerTable.Card1 != null)
			{
				ClosestDealerTable.Card1.Position = ClosestDealerTable.Table.GetOffsetPosition(GetDealerCardPositionOffset(0)) + new Vector3(0f, 0f, 0.01f);
				ClosestDealerTable.Card1.Rotation = new Vector3(0f, 180f, ClosestDealerTable.Table.Rotation.Z);
				if (ClosestDealerTable.Card1.Opacity != 0 && Debug)
				{
					World.DrawMarker(MarkerType.Cylinder, ClosestDealerTable.Card1.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.005f, 0.005f, 0.5f), Color.Red);
				}
				if (DealerCards < 1)
				{
					ClosestDealerTable.Card1.Opacity = 0;
				}
				else
				{
					ClosestDealerTable.Card1.Opacity = 255;
				}
				if (Stage == 100f || Stage == 150f || Stage == 300f || Stage == 50f || Stage == 51f || Stage == 52f)
				{
					ClosestDealerTable.Card1.Rotation = new Vector3(0f, 0f, ClosestDealerTable.Table.Rotation.Z);
					if (Backface_DealerCard1 != null)
					{
						Backface_DealerCard1.Opacity = 0;
					}
				}
			}
			if (ClosestDealerTable.Card2 != null)
			{
				ClosestDealerTable.Card2.Position = ClosestDealerTable.Table.GetOffsetPosition(GetDealerCardPositionOffset(2)) + new Vector3(0f, 0f, 0.01f);
				ClosestDealerTable.Card2.Rotation = ClosestDealerTable.Table.Rotation;
				if (ClosestDealerTable.Card2.Opacity != 0 && Debug)
				{
					World.DrawMarker(MarkerType.Cylinder, ClosestDealerTable.Card2.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.005f, 0.005f, 0.5f), Color.BlueViolet);
				}
				if (DealerCards < 2)
				{
					ClosestDealerTable.Card2.Opacity = 0;
				}
				else
				{
					ClosestDealerTable.Card2.Opacity = 255;
				}
			}
			if (ClosestDealerTable.Card3 != null)
			{
				ClosestDealerTable.Card3.Position = ClosestDealerTable.Table.GetOffsetPosition(GetDealerCardPositionOffset(1)) + new Vector3(0f, 0f, 0.01f);
				ClosestDealerTable.Card3.Rotation = ClosestDealerTable.Table.Rotation;
				if (ClosestDealerTable.Card3.Opacity != 0 && Debug)
				{
					World.DrawMarker(MarkerType.Cylinder, ClosestDealerTable.Card3.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.005f, 0.005f, 0.5f), Color.DarkGray);
				}
				if (DealerCards < 3)
				{
					ClosestDealerTable.Card3.Opacity = 0;
				}
				else
				{
					ClosestDealerTable.Card3.Opacity = 255;
				}
			}
			if (ClosestDealerTable.Card4 != null)
			{
				ClosestDealerTable.Card4.Position = ClosestDealerTable.Table.GetOffsetPosition(GetDealerCardPositionOffset(3)) + new Vector3(0f, 0f, 0.01f);
				ClosestDealerTable.Card4.Rotation = ClosestDealerTable.Table.Rotation;
				if (ClosestDealerTable.Card4.Opacity != 0 && Debug)
				{
					World.DrawMarker(MarkerType.Cylinder, ClosestDealerTable.Card4.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.005f, 0.005f, 0.5f), Color.CadetBlue);
				}
				if (DealerCards < 4)
				{
					ClosestDealerTable.Card4.Opacity = 0;
				}
				else
				{
					ClosestDealerTable.Card4.Opacity = 255;
				}
			}
			if (ClosestDealerTable.Card5 != null)
			{
				ClosestDealerTable.Card5.Position = ClosestDealerTable.Table.GetOffsetPosition(GetDealerCardPositionOffset(4)) + new Vector3(0f, 0f, 0.01f);
				ClosestDealerTable.Card5.Rotation = ClosestDealerTable.Table.Rotation;
				if (ClosestDealerTable.Card5.Opacity != 0 && Debug)
				{
					World.DrawMarker(MarkerType.Cylinder, ClosestDealerTable.Card5.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.005f, 0.005f, 0.5f), Color.Gold);
				}
				if (DealerCards < 5)
				{
					ClosestDealerTable.Card5.Opacity = 0;
				}
				else
				{
					ClosestDealerTable.Card5.Opacity = 255;
				}
			}
			if (ClosestDealerTable.Card6 != null)
			{
				ClosestDealerTable.Card6.Position = ClosestDealerTable.Table.GetOffsetPosition(GetDealerCardPositionOffset(5)) + new Vector3(0f, 0f, 0.01f);
				ClosestDealerTable.Card6.Rotation = ClosestDealerTable.Table.Rotation;
				if (ClosestDealerTable.Card6.Opacity != 0 && Debug)
				{
					World.DrawMarker(MarkerType.Cylinder, ClosestDealerTable.Card6.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.005f, 0.005f, 0.5f), Color.DeepPink);
				}
				if (DealerCards < 6)
				{
					ClosestDealerTable.Card6.Opacity = 0;
				}
				else
				{
					ClosestDealerTable.Card6.Opacity = 255;
				}
			}
		}

		public static void OverrideDealer1stCard()
		{
			Vector3 offsetPosition = GetClosesBlackjackTable().GetOffsetPosition(new Vector3(0.195f, -0.19f, 0.986f));
			if (!(Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 100f) <= 49.1f))
			{
				return;
			}
			if (Playercards == 3)
			{
				int num = BlackjackBackupDealerCard1_ThreeCards;
				if (num > 2 && num < 11)
				{
					float num2 = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 100f);
					if (num2 <= 30f)
					{
						num--;
					}
					if (num2 > 70f)
					{
						num++;
					}
				}
				if (num > 3 && num < 10)
				{
					float num3 = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 100f);
					if (num3 <= 10f)
					{
						num -= 2;
					}
					if (num3 > 10f && num3 <= 30f)
					{
						num--;
					}
					if (num3 >= 70f && num3 < 90f)
					{
						num++;
					}
					if (num3 >= 90f)
					{
						num += 2;
					}
				}
				DealerPresetCars[PresetChoosen].Card1 = GetCardOfType(CardVarient.Any, CardNumberToID(num));
				Prop card = ClosestDealerTable.Card1;
				string card2 = DealerPresetCars[PresetChoosen].Card1;
				ClosestDealerTable.Card1num = Set_DealerCard_Blackjack(card2, 1);
				Script.Wait(1);
				Prop prop = GlobalHelper.CreateProp(card2, offsetPosition, true, false);
				prop.IsPositionFrozen = true;
				ClosestDealerTable.Card1 = prop;
				ClosestDealerTable.Card1Id = card2;
				ClosestDealerTable.Card1.Position = card.Position;
				ClosestDealerTable.Card1.Rotation = card.Rotation;
				card.Delete();
				Backface_DealerCard1.AttachTo(ClosestDealerTable.Card1, new Vector3(0f, 0f, 0.001f), new Vector3(0f, 180f, 0f));
			}
			if (Playercards == 4)
			{
				int num4 = BlackjackBackupDealerCard1_FourCards;
				if (num4 > 2 && num4 < 11)
				{
					float num5 = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 100f);
					if (num5 <= 30f)
					{
						num4--;
					}
					if (num5 > 70f)
					{
						num4++;
					}
				}
				if (num4 > 3 && num4 < 10)
				{
					float num6 = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 100f);
					if (num6 <= 10f)
					{
						num4 -= 2;
					}
					if (num6 > 10f && num6 <= 30f)
					{
						num4--;
					}
					if (num6 >= 70f && num6 < 90f)
					{
						num4++;
					}
					if (num6 >= 90f)
					{
						num4 += 2;
					}
				}
				DealerPresetCars[PresetChoosen].Card1 = GetCardOfType(CardVarient.Any, CardNumberToID(num4));
				Prop card3 = ClosestDealerTable.Card1;
				string card4 = DealerPresetCars[PresetChoosen].Card1;
				ClosestDealerTable.Card1num = Set_DealerCard_Blackjack(card4, 1);
				Script.Wait(1);
				Prop prop2 = GlobalHelper.CreateProp(card4, offsetPosition, true, false);
				prop2.IsPositionFrozen = true;
				ClosestDealerTable.Card1 = prop2;
				ClosestDealerTable.Card1Id = card4;
				ClosestDealerTable.Card1.Position = card3.Position;
				ClosestDealerTable.Card1.Rotation = card3.Rotation;
				card3.Delete();
				Backface_DealerCard1.AttachTo(ClosestDealerTable.Card1, new Vector3(0f, 0f, 0.001f), new Vector3(0f, 180f, 0f));
			}
			if (Playercards == 5)
			{
				int num7 = BlackjackBackupDealerCard1_FiveCards;
				if (num7 > 2 && num7 < 11)
				{
					float num8 = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 100f);
					if (num8 <= 30f)
					{
						num7--;
					}
					if (num8 > 70f)
					{
						num7++;
					}
				}
				if (num7 > 3 && num7 < 10)
				{
					float num9 = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 100f);
					if (num9 <= 10f)
					{
						num7 -= 2;
					}
					if (num9 > 10f && num9 <= 30f)
					{
						num7--;
					}
					if (num9 >= 70f && num9 < 90f)
					{
						num7++;
					}
					if (num9 >= 90f)
					{
						num7 += 2;
					}
				}
				DealerPresetCars[PresetChoosen].Card1 = GetCardOfType(CardVarient.Any, CardNumberToID(num7));
				Prop card5 = ClosestDealerTable.Card1;
				string card6 = DealerPresetCars[PresetChoosen].Card1;
				ClosestDealerTable.Card1num = Set_DealerCard_Blackjack(card6, 1);
				Script.Wait(1);
				Prop prop3 = GlobalHelper.CreateProp(card6, offsetPosition, true, false);
				prop3.IsPositionFrozen = true;
				ClosestDealerTable.Card1 = prop3;
				ClosestDealerTable.Card1Id = card6;
				ClosestDealerTable.Card1.Position = card5.Position;
				ClosestDealerTable.Card1.Rotation = card5.Rotation;
				card5.Delete();
				Backface_DealerCard1.AttachTo(ClosestDealerTable.Card1, new Vector3(0f, 0f, 0.001f), new Vector3(0f, 180f, 0f));
			}
			DealerCard_1 = ClosestDealerTable.Card1num;
			DealerCard_2 = ClosestDealerTable.Card2num;
			if (Playercards < 2)
			{
				DealerCard_2 = 0;
			}
			DealerCard_3 = ClosestDealerTable.Card3num;
			if (Playercards < 3)
			{
				DealerCard_3 = 0;
			}
			DealerCard_4 = ClosestDealerTable.Card4num;
			if (Playercards < 4)
			{
				DealerCard_4 = 0;
			}
			DealerCard_5 = ClosestDealerTable.Card5num;
			if (Playercards < 5)
			{
				DealerCard_5 = 0;
			}
			DealerCard_6 = ClosestDealerTable.Card6num;
			if (Playercards < 6)
			{
				DealerCard_6 = 0;
			}
		}

		private static ModelNames IntToEnum(Type enumType, uint hash)
		{
			return ModelNames.VW_PROP_CHIP_10DOLLAR_X1;
		}

		public static void DetatchAllCards()
		{
			if (ClosestDealerTable.Card1 != null)
			{
				ClosestDealerTable.Card1.Detach();
			}
			if (ClosestDealerTable.Card2 != null)
			{
				ClosestDealerTable.Card2.Detach();
			}
			if (ClosestDealerTable.Card3 != null)
			{
				ClosestDealerTable.Card3.Detach();
			}
			if (ClosestDealerTable.Card4 != null)
			{
				ClosestDealerTable.Card4.Detach();
			}
			if (ClosestDealerTable.Card5 != null)
			{
				ClosestDealerTable.Card5.Detach();
			}
			if (ClosestDealerTable.Card6 != null)
			{
				ClosestDealerTable.Card6.Detach();
			}
			if (PlayerCardA != null)
			{
				PlayerCardA.Detach();
			}
			if (PlayerCardB != null)
			{
				PlayerCardB.Detach();
			}
			if (PlayerCardC != null)
			{
				PlayerCardC.Detach();
			}
			if (PlayerCardD != null)
			{
				PlayerCardD.Detach();
			}
			if (PlayerCardE != null)
			{
				PlayerCardE.Detach();
			}
			if (PlayerCardF != null)
			{
				PlayerCardF.Detach();
			}
		}

		public static Vector3 GetDealerAnimOrigin()
		{
			return ClosestDealerTable.Table.Position + new Vector3(0f, 0f, 0f);
		}

		public static Vector3 GetDealerAnimOrigin_Roulette()
		{
			return ClosestDealerTable.Table.GetOffsetPosition(new Vector3(0f, 0f, 0.02f));
		}

		public static Vector3 GetDealerAnimOrigin_Roulette_Wheel()
		{
			return ClosestDealerTable.Table.GetOffsetPosition(new Vector3(-0.738f, -0.171f, 1.075f));
		}

		public static Vector3 GetDealerAnimOriginRot_Roulette()
		{
			return new Vector3(0f, 0f, ClosestDealerTable.Table.Heading + 180f);
		}

		public static ValueTuple<Vector3, float> GetPlayerAnimOriginH(int seat, TableType tableType)
		{
			Vector3 vector = GET_PLAYER_CHAIR_POSITION(seat, tableType);
			return new ValueTuple<Vector3, float>(item2: GET_PLAYER_CHAIR_ROTATION(seat, tableType).Z, item1: new Vector3(vector.X, vector.Y, vector.Z + 1f));
		}

		public static Vector3 GET_PLAYER_CHAIR_POSITION(int iSeatID, TableType tableType)
		{
			Prop prop = null;
			if (tableType == TableType.Blackjack)
			{
				prop = GetClosesBlackjackTable();
			}
			if (tableType == TableType.Poker)
			{
				prop = GetClosesPokerTable();
			}
			if (tableType == TableType.Roulette)
			{
				prop = GetClosesRouletteTable();
			}
			if (prop.Exists())
			{
				switch (iSeatID)
				{
				case 4:
					return GET_ENTITY_BONE_POSTION(prop, GET_ENTITY_BONE_INDEX_BY_NAME(prop, "Chair_Base_01"));
				case 3:
					return GET_ENTITY_BONE_POSTION(prop, GET_ENTITY_BONE_INDEX_BY_NAME(prop, "Chair_Base_02"));
				case 2:
					return GET_ENTITY_BONE_POSTION(prop, GET_ENTITY_BONE_INDEX_BY_NAME(prop, "Chair_Base_03"));
				case 1:
					return GET_ENTITY_BONE_POSTION(prop, GET_ENTITY_BONE_INDEX_BY_NAME(prop, "Chair_Base_04"));
				}
			}
			return new Vector3(0f, 0f, 0f);
		}

		public static Vector3 GET_PLAYER_CHAIR_POSITION(int iSeatID, Prop objTable)
		{
			if (objTable.Exists())
			{
				switch (iSeatID)
				{
				case 4:
					return GET_ENTITY_BONE_POSTION(objTable, GET_ENTITY_BONE_INDEX_BY_NAME(objTable, "Chair_Base_01"));
				case 3:
					return GET_ENTITY_BONE_POSTION(objTable, GET_ENTITY_BONE_INDEX_BY_NAME(objTable, "Chair_Base_02"));
				case 2:
					return GET_ENTITY_BONE_POSTION(objTable, GET_ENTITY_BONE_INDEX_BY_NAME(objTable, "Chair_Base_03"));
				case 1:
					return GET_ENTITY_BONE_POSTION(objTable, GET_ENTITY_BONE_INDEX_BY_NAME(objTable, "Chair_Base_04"));
				}
			}
			return new Vector3(0f, 0f, 0f);
		}

		public static Vector3 GET_PLAYER_CHAIR_ROTATION(int iSeatID, Prop objTable)
		{
			if (objTable.Exists())
			{
				switch (iSeatID)
				{
				case 4:
					return GET_ENTITY_BONE_ROTATION(objTable, GET_ENTITY_BONE_INDEX_BY_NAME(objTable, "Chair_Base_01"));
				case 3:
					return GET_ENTITY_BONE_ROTATION(objTable, GET_ENTITY_BONE_INDEX_BY_NAME(objTable, "Chair_Base_02"));
				case 2:
					return GET_ENTITY_BONE_ROTATION(objTable, GET_ENTITY_BONE_INDEX_BY_NAME(objTable, "Chair_Base_03"));
				case 1:
					return GET_ENTITY_BONE_ROTATION(objTable, GET_ENTITY_BONE_INDEX_BY_NAME(objTable, "Chair_Base_04"));
				}
			}
			return new Vector3(0f, 0f, 0f);
		}

		public static Vector3 GET_PLAYER_CHAIR_ROTATION(int iSeatID, TableType tableType)
		{
			Prop prop = null;
			if (tableType == TableType.Blackjack)
			{
				prop = GetClosesBlackjackTable();
			}
			if (tableType == TableType.Poker)
			{
				prop = GetClosesPokerTable();
			}
			if (tableType == TableType.Roulette)
			{
				prop = GetClosesRouletteTable();
			}
			if (prop.Exists())
			{
				switch (iSeatID)
				{
				case 4:
					return GET_ENTITY_BONE_ROTATION(prop, GET_ENTITY_BONE_INDEX_BY_NAME(prop, "Chair_Base_01"));
				case 3:
					return GET_ENTITY_BONE_ROTATION(prop, GET_ENTITY_BONE_INDEX_BY_NAME(prop, "Chair_Base_02"));
				case 2:
					return GET_ENTITY_BONE_ROTATION(prop, GET_ENTITY_BONE_INDEX_BY_NAME(prop, "Chair_Base_03"));
				case 1:
					return GET_ENTITY_BONE_ROTATION(prop, GET_ENTITY_BONE_INDEX_BY_NAME(prop, "Chair_Base_04"));
				}
			}
			return new Vector3(0f, 0f, 0f);
		}

		public static Vector3 GET_ENTITY_BONE_POSTION(Prop Obj, int Bone)
		{
			return Function.Call<Vector3>(Hash.GET_ENTITY_BONE_POSTION, Obj, Bone);
		}

		public static Vector3 GET_ENTITY_BONE_ROTATION(Prop Obj, int Bone)
		{
			return Function.Call<Vector3>(Hash.GET_ENTITY_BONE_ROTATION, Obj, Bone);
		}

		public static int GET_ENTITY_BONE_INDEX_BY_NAME(Prop Obj, string Bone)
		{
			return Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, Obj, Bone);
		}

		public static Vector3 GET_ANIM_INITIAL_OFFSET_ROTATION(string animDict, string animName, Vector3 Pos, Vector3 Rot, float p8)
		{
			return Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, animDict, animName, Pos.X, Pos.Y, Pos.Z, Rot.X, Rot.Y, Rot.Z, p8);
		}

		public static Vector3 GET_ANIM_INITIAL_OFFSET_POSITION(string animDict, string animName, Vector3 Pos, Vector3 Rot, float p8)
		{
			return Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, animDict, animName, Pos.X, Pos.Y, Pos.Z, Rot.X, Rot.Y, Rot.Z, p8);
		}

		public static float GET_PLAYER_ENTER_HEADING(string dict, string enter, int iSeatID, TableType tableType)
		{
			return GET_ANIM_INITIAL_OFFSET_ROTATION(dict, enter, GET_PLAYER_CHAIR_POSITION(iSeatID, tableType), GET_PLAYER_CHAIR_ROTATION(iSeatID, tableType), 0.01f).Z;
		}

		public static Vector3 GET_PLAYER_ENTER_POSITION(string dict, string enter, int iSeatID, TableType tableType)
		{
			return GET_ANIM_INITIAL_OFFSET_POSITION(dict, enter, GET_PLAYER_CHAIR_POSITION(iSeatID, tableType), GET_PLAYER_CHAIR_ROTATION(iSeatID, tableType), 0.01f);
		}

		public static int ReturnClosestBetAmtFromCustomBet_Poker(int PrevBet)
		{
			int result = 5000;
			if (PrevBet >= 5000)
			{
				result = 5000;
			}
			if (PrevBet >= 10000)
			{
				result = 10000;
			}
			if (PrevBet >= 15000)
			{
				result = 15000;
			}
			if (PrevBet >= 20000)
			{
				result = 20000;
			}
			if (PrevBet >= 25000)
			{
				result = 25000;
			}
			if (PrevBet >= 30000)
			{
				result = 30000;
			}
			if (PrevBet >= 35000)
			{
				result = 35000;
			}
			if (PrevBet >= 40000)
			{
				result = 40000;
			}
			if (PrevBet >= 45000)
			{
				result = 45000;
			}
			if (PrevBet >= 50000)
			{
				result = 50000;
			}
			return result;
		}

		public static int ReturnClosestBetAmtFromCustomBet(int PrevBet)
		{
			int result = 10;
			if (PrevBet >= 10)
			{
				result = 10;
			}
			if (PrevBet >= 20)
			{
				result = 20;
			}
			if (PrevBet >= 30)
			{
				result = 30;
			}
			if (PrevBet >= 40)
			{
				result = 40;
			}
			if (PrevBet >= 50)
			{
				result = 50;
			}
			if (PrevBet >= 60)
			{
				result = 60;
			}
			if (PrevBet >= 70)
			{
				result = 70;
			}
			if (PrevBet >= 80)
			{
				result = 80;
			}
			if (PrevBet >= 90)
			{
				result = 90;
			}
			if (PrevBet >= 100)
			{
				result = 100;
			}
			if (PrevBet >= 150)
			{
				result = 150;
			}
			if (PrevBet >= 200)
			{
				result = 200;
			}
			if (PrevBet >= 250)
			{
				result = 250;
			}
			if (PrevBet >= 300)
			{
				result = 300;
			}
			if (PrevBet >= 350)
			{
				result = 350;
			}
			if (PrevBet >= 400)
			{
				result = 400;
			}
			if (PrevBet >= 450)
			{
				result = 450;
			}
			if (PrevBet >= 500)
			{
				result = 500;
			}
			if (PrevBet >= 1000)
			{
				result = 1000;
			}
			if (PrevBet >= 1500)
			{
				result = 1500;
			}
			if (PrevBet >= 2000)
			{
				result = 2000;
			}
			if (PrevBet >= 2500)
			{
				result = 2500;
			}
			if (PrevBet >= 3000)
			{
				result = 3000;
			}
			if (PrevBet >= 3500)
			{
				result = 3500;
			}
			if (PrevBet >= 4000)
			{
				result = 4000;
			}
			if (PrevBet >= 4500)
			{
				result = 4500;
			}
			if (PrevBet >= 5000)
			{
				result = 5000;
			}
			if (PrevBet >= 6000)
			{
				result = 6000;
			}
			if (PrevBet >= 7000)
			{
				result = 7000;
			}
			if (PrevBet >= 8000)
			{
				result = 8000;
			}
			if (PrevBet >= 9000)
			{
				result = 9000;
			}
			if (PrevBet >= 10000)
			{
				result = 10000;
			}
			if (PrevBet >= 15000)
			{
				result = 15000;
			}
			if (PrevBet >= 20000)
			{
				result = 20000;
			}
			if (PrevBet >= 25000)
			{
				result = 25000;
			}
			if (PrevBet >= 30000)
			{
				result = 30000;
			}
			if (PrevBet >= 35000)
			{
				result = 35000;
			}
			if (PrevBet >= 40000)
			{
				result = 40000;
			}
			if (PrevBet >= 45000)
			{
				result = 45000;
			}
			if (PrevBet >= 50000)
			{
				result = 50000;
			}
			return result;
		}

		public static Vector3 GetPlayerBetRotationOffset(int iBetAmount, int iBetID, int iPlayerID, bool bHighLimitTable)
		{
			if (!bHighLimitTable)
			{
				switch (iPlayerID)
				{
				case 0:
					switch (iBetID)
					{
					case 0:
						return new Vector3(0f, 0f, 72f);
					case 1:
						return new Vector3(0f, 0f, 64.8f);
					case 2:
						return new Vector3(0f, 0f, 74.52f);
					case 3:
						return new Vector3(0f, 0f, 72f);
					}
					break;
				case 1:
					switch (iBetID)
					{
					case 0:
						return new Vector3(0f, 0f, 12.96f);
					case 1:
						return new Vector3(0f, 0f, 29.16f);
					case 2:
						return new Vector3(0f, 0f, 32.04f);
					case 3:
						return new Vector3(0f, 0f, 32.04f);
					}
					break;
				case 2:
					switch (iBetID)
					{
					case 0:
						return new Vector3(0f, 0f, -18.36f);
					case 1:
						return new Vector3(0f, 0f, -18.72f);
					case 2:
						return new Vector3(0f, 0f, -15.48f);
					case 3:
						return new Vector3(0f, 0f, -18f);
					}
					break;
				case 3:
					switch (iBetID)
					{
					case 0:
						return new Vector3(0f, 0f, -79.2f);
					case 1:
						return new Vector3(0f, 0f, -68.76f);
					case 2:
						return new Vector3(0f, 0f, -57.6f);
					case 3:
						return new Vector3(0f, 0f, -64.8f);
					}
					break;
				}
			}
			else
			{
				switch (iBetAmount)
				{
				case 5000:
				case 10000:
				case 15000:
				case 20000:
				case 25000:
				case 30000:
				case 35000:
				case 40000:
				case 45000:
				case 50000:
					switch (iPlayerID)
					{
					case 0:
						switch (iBetID)
						{
						case 0:
							return new Vector3(0f, 0f, -16.56f);
						case 1:
							return new Vector3(0f, 0f, -22.32f);
						case 2:
							return new Vector3(0f, 0f, -10.8f);
						case 3:
							return new Vector3(0f, 0f, -9.72f);
						}
						break;
					case 1:
						switch (iBetID)
						{
						case 0:
							return new Vector3(0f, 0f, -69.12f);
						case 1:
							return new Vector3(0f, 0f, -64.8f);
						case 2:
							return new Vector3(0f, 0f, -58.68f);
						case 3:
							return new Vector3(0f, 0f, -51.12f);
						}
						break;
					case 2:
						switch (iBetID)
						{
						case 0:
							return new Vector3(0f, 0f, -112.32f);
						case 1:
							return new Vector3(0f, 0f, -108.36f);
						case 2:
							return new Vector3(0f, 0f, -99.72f);
						case 3:
							return new Vector3(0f, 0f, -102.6f);
						}
						break;
					case 3:
						switch (iBetID)
						{
						case 0:
							return new Vector3(0f, 0f, -155.88f);
						case 1:
							return new Vector3(0f, 0f, -151.92f);
						case 2:
							return new Vector3(0f, 0f, -147.24f);
						case 3:
							return new Vector3(0f, 0f, -146.52f);
						}
						break;
					}
					break;
				default:
					switch (iPlayerID)
					{
					case 0:
						switch (iBetID)
						{
						case 0:
							return new Vector3(0f, 0f, 72f);
						case 1:
							return new Vector3(0f, 0f, 64.8f);
						case 2:
							return new Vector3(0f, 0f, 74.52f);
						case 3:
							return new Vector3(0f, 0f, 72f);
						}
						break;
					case 1:
						switch (iBetID)
						{
						case 0:
							return new Vector3(0f, 0f, 12.96f);
						case 1:
							return new Vector3(0f, 0f, 29.16f);
						case 2:
							return new Vector3(0f, 0f, 32.04f);
						case 3:
							return new Vector3(0f, 0f, 32.04f);
						}
						break;
					case 2:
						switch (iBetID)
						{
						case 0:
							return new Vector3(0f, 0f, -18.36f);
						case 1:
							return new Vector3(0f, 0f, -18.72f);
						case 2:
							return new Vector3(0f, 0f, -15.48f);
						case 3:
							return new Vector3(0f, 0f, -18f);
						}
						break;
					case 3:
						switch (iBetID)
						{
						case 0:
							return new Vector3(0f, 0f, -79.2f);
						case 1:
							return new Vector3(0f, 0f, -68.76f);
						case 2:
							return new Vector3(0f, 0f, -57.6f);
						case 3:
							return new Vector3(0f, 0f, -64.8f);
						}
						break;
					}
					break;
				}
			}
			return new Vector3(0f, 0f, 0f);
		}

		public static Vector3 GetPlayerBetPositionZOffset(int iBetAmount, int iBetID, int iPlayerID, bool bHighLimitTable)
		{
			float z = 0f;
			Vector3 result = Vector3.Zero;
			if (!bHighLimitTable)
			{
				switch (iBetAmount)
				{
				case 10:
					z = 0.95f;
					break;
				case 20:
					z = 0.896f;
					break;
				case 30:
					z = 0.901f;
					break;
				case 40:
					z = 0.907f;
					break;
				case 50:
					z = 0.95f;
					break;
				case 60:
					z = 0.917f;
					break;
				case 70:
					z = 0.922f;
					break;
				case 80:
					z = 0.927f;
					break;
				case 90:
					z = 0.932f;
					break;
				case 100:
					z = 0.95f;
					break;
				case 150:
					z = 0.904f;
					break;
				case 200:
					z = 0.899f;
					break;
				case 250:
					z = 0.914f;
					break;
				case 300:
					z = 0.904f;
					break;
				case 350:
					z = 0.924f;
					break;
				case 400:
					z = 0.91f;
					break;
				case 450:
					z = 0.935f;
					break;
				case 500:
					z = 0.95f;
					break;
				case 1000:
					z = 0.95f;
					break;
				case 1500:
					z = 0.904f;
					break;
				case 2000:
					z = 0.899f;
					break;
				case 2500:
					z = 0.915f;
					break;
				case 3000:
					z = 0.904f;
					break;
				case 3500:
					z = 0.925f;
					break;
				case 4000:
					z = 0.91f;
					break;
				case 4500:
					z = 0.935f;
					break;
				case 5000:
					z = 0.95f;
					break;
				case 6000:
					z = 0.919f;
					break;
				case 7000:
					z = 0.924f;
					break;
				case 8000:
					z = 0.93f;
					break;
				case 9000:
					z = 0.935f;
					break;
				case 10000:
					z = 0.95f;
					break;
				case 15000:
					z = 0.902f;
					break;
				case 20000:
					z = 0.897f;
					break;
				case 25000:
					z = 0.912f;
					break;
				case 30000:
					z = 0.902f;
					break;
				case 35000:
					z = 0.922f;
					break;
				case 40000:
					z = 0.907f;
					break;
				case 45000:
					z = 0.932f;
					break;
				case 50000:
					z = 0.912f;
					break;
				}
				switch (iPlayerID)
				{
				case 0:
					switch (iBetID)
					{
					case 0:
						result = new Vector3(0.712625f, 0.170625f, 0f);
						break;
					case 1:
						result = new Vector3(0.6658f, 0.218375f, 0f);
						break;
					case 2:
						result = new Vector3(0.756775f, 0.292775f, 0f);
						break;
					case 3:
						result = new Vector3(0.701875f, 0.3439f, 0f);
						break;
					}
					break;
				case 1:
					switch (iBetID)
					{
					case 0:
						result = new Vector3(0.278125f, -0.2571f, 0f);
						break;
					case 1:
						result = new Vector3(0.280375f, -0.190375f, 0f);
						break;
					case 2:
						result = new Vector3(0.397775f, -0.208525f, 0f);
						break;
					case 3:
						result = new Vector3(0.39715f, -0.1354f, 0f);
						break;
					}
					break;
				case 2:
					switch (iBetID)
					{
					case 0:
						result = new Vector3(-0.30305f, -0.2464f, 0f);
						break;
					case 1:
						result = new Vector3(-0.257975f, -0.19715f, 0f);
						break;
					case 2:
						result = new Vector3(-0.186575f, -0.2861f, 0f);
						break;
					case 3:
						result = new Vector3(-0.141675f, -0.237925f, 0f);
						break;
					}
					break;
				case 3:
					switch (iBetID)
					{
					case 0:
						result = new Vector3(-0.72855f, 0.17345f, 0f);
						break;
					case 1:
						result = new Vector3(-0.652825f, 0.177525f, 0f);
						break;
					case 2:
						result = new Vector3(-0.6783f, 0.0744f, 0f);
						break;
					case 3:
						result = new Vector3(-0.604425f, 0.082575f, 0f);
						break;
					}
					break;
				}
			}
			else
			{
				switch (iBetAmount)
				{
				case 10:
					z = 0.95f;
					break;
				case 20:
					z = 0.896f;
					break;
				case 30:
					z = 0.901f;
					break;
				case 40:
					z = 0.907f;
					break;
				case 50:
					z = 0.95f;
					break;
				case 60:
					z = 0.917f;
					break;
				case 70:
					z = 0.922f;
					break;
				case 80:
					z = 0.927f;
					break;
				case 90:
					z = 0.932f;
					break;
				case 100:
					z = 0.95f;
					break;
				case 150:
					z = 0.904f;
					break;
				case 200:
					z = 0.899f;
					break;
				case 250:
					z = 0.914f;
					break;
				case 300:
					z = 0.904f;
					break;
				case 350:
					z = 0.924f;
					break;
				case 400:
					z = 0.91f;
					break;
				case 450:
					z = 0.935f;
					break;
				case 500:
					z = 0.95f;
					break;
				case 1000:
					z = 0.95f;
					break;
				case 1500:
					z = 0.904f;
					break;
				case 2000:
					z = 0.899f;
					break;
				case 2500:
					z = 0.915f;
					break;
				case 3000:
					z = 0.904f;
					break;
				case 3500:
					z = 0.925f;
					break;
				case 4000:
					z = 0.91f;
					break;
				case 4500:
					z = 0.935f;
					break;
				case 5000:
					z = 0.953f;
					break;
				case 6000:
					z = 0.919f;
					break;
				case 7000:
					z = 0.924f;
					break;
				case 8000:
					z = 0.93f;
					break;
				case 9000:
					z = 0.935f;
					break;
				case 10000:
					z = 0.95f;
					break;
				case 15000:
					z = 0.902f;
					break;
				case 20000:
					z = 0.897f;
					break;
				case 25000:
					z = 0.912f;
					break;
				case 30000:
					z = 0.902f;
					break;
				case 35000:
					z = 0.922f;
					break;
				case 40000:
					z = 0.907f;
					break;
				case 45000:
					z = 0.932f;
					break;
				case 50000:
					z = 0.912f;
					break;
				}
				switch (iBetAmount)
				{
				case 5000:
				case 10000:
				case 15000:
				case 20000:
				case 25000:
				case 30000:
				case 35000:
				case 40000:
				case 45000:
				case 50000:
					switch (iPlayerID)
					{
					case 0:
						switch (iBetID)
						{
						case 0:
							result = new Vector3(0.6931f, 0.1952f, 0f);
							break;
						case 1:
							result = new Vector3(0.724925f, 0.26955f, 0f);
							break;
						case 2:
							result = new Vector3(0.7374f, 0.349625f, 0f);
							break;
						case 3:
							result = new Vector3(0.76415f, 0.419225f, 0f);
							break;
						}
						break;
					case 1:
						switch (iBetID)
						{
						case 0:
							result = new Vector3(0.2827f, -0.227825f, 0f);
							break;
						case 1:
							result = new Vector3(0.3605f, -0.1898f, 0f);
							break;
						case 2:
							result = new Vector3(0.4309f, -0.16365f, 0f);
							break;
						case 3:
							result = new Vector3(0.49275f, -0.111575f, 0f);
							break;
						}
						break;
					case 2:
						switch (iBetID)
						{
						case 0:
							result = new Vector3(-0.279425f, -0.2238f, 0f);
							break;
						case 1:
							result = new Vector3(-0.200775f, -0.25855f, 0f);
							break;
						case 2:
							result = new Vector3(-0.125775f, -0.26815f, 0f);
							break;
						case 3:
							result = new Vector3(-0.05615f, -0.29435f, 0f);
							break;
						}
						break;
					case 3:
						switch (iBetID)
						{
						case 0:
							result = new Vector3(-0.685925f, 0.173275f, 0f);
							break;
						case 1:
							result = new Vector3(-0.6568f, 0.092525f, 0f);
							break;
						case 2:
							result = new Vector3(-0.612875f, 0.033025f, 0f);
							break;
						case 3:
							result = new Vector3(-0.58465f, -0.0374f, 0f);
							break;
						}
						break;
					}
					break;
				default:
					switch (iPlayerID)
					{
					case 0:
						switch (iBetID)
						{
						case 0:
							result = new Vector3(0.712625f, 0.170625f, 0f);
							break;
						case 1:
							result = new Vector3(0.6658f, 0.218375f, 0f);
							break;
						case 2:
							result = new Vector3(0.756775f, 0.292775f, 0f);
							break;
						case 3:
							result = new Vector3(0.701875f, 0.3439f, 0f);
							break;
						}
						break;
					case 1:
						switch (iBetID)
						{
						case 0:
							result = new Vector3(0.278125f, -0.2571f, 0f);
							break;
						case 1:
							result = new Vector3(0.280375f, -0.190375f, 0f);
							break;
						case 2:
							result = new Vector3(0.397775f, -0.208525f, 0f);
							break;
						case 3:
							result = new Vector3(0.39715f, -0.1354f, 0f);
							break;
						}
						break;
					case 2:
						switch (iBetID)
						{
						case 0:
							result = new Vector3(-0.30305f, -0.2464f, 0f);
							break;
						case 1:
							result = new Vector3(-0.257975f, -0.19715f, 0f);
							break;
						case 2:
							result = new Vector3(-0.186575f, -0.2861f, 0f);
							break;
						case 3:
							result = new Vector3(-0.141675f, -0.237925f, 0f);
							break;
						}
						break;
					case 3:
						switch (iBetID)
						{
						case 0:
							result = new Vector3(-0.72855f, 0.17345f, 0f);
							break;
						case 1:
							result = new Vector3(-0.652825f, 0.177525f, 0f);
							break;
						case 2:
							result = new Vector3(-0.6783f, 0.0744f, 0f);
							break;
						case 3:
							result = new Vector3(-0.604425f, 0.082575f, 0f);
							break;
						}
						break;
					}
					break;
				}
			}
			result.Z = z;
			return result;
		}

		public static float GetPlayerBetPositionZHeightOffset(int iBetAmount, int iBetID, int iPlayerID, bool bHighLimitTable)
		{
			float num = 0f;
			Vector3 vector = Vector3.Zero;
			if (!bHighLimitTable)
			{
				switch (iBetAmount)
				{
				case 10:
					num = 0.95f;
					break;
				case 20:
					num = 0.896f;
					break;
				case 30:
					num = 0.901f;
					break;
				case 40:
					num = 0.907f;
					break;
				case 50:
					num = 0.95f;
					break;
				case 60:
					num = 0.917f;
					break;
				case 70:
					num = 0.922f;
					break;
				case 80:
					num = 0.927f;
					break;
				case 90:
					num = 0.932f;
					break;
				case 100:
					num = 0.95f;
					break;
				case 150:
					num = 0.904f;
					break;
				case 200:
					num = 0.899f;
					break;
				case 250:
					num = 0.914f;
					break;
				case 300:
					num = 0.904f;
					break;
				case 350:
					num = 0.924f;
					break;
				case 400:
					num = 0.91f;
					break;
				case 450:
					num = 0.935f;
					break;
				case 500:
					num = 0.95f;
					break;
				case 1000:
					num = 0.95f;
					break;
				case 1500:
					num = 0.904f;
					break;
				case 2000:
					num = 0.899f;
					break;
				case 2500:
					num = 0.915f;
					break;
				case 3000:
					num = 0.904f;
					break;
				case 3500:
					num = 0.925f;
					break;
				case 4000:
					num = 0.91f;
					break;
				case 4500:
					num = 0.935f;
					break;
				case 5000:
					num = 0.95f;
					break;
				case 6000:
					num = 0.919f;
					break;
				case 7000:
					num = 0.924f;
					break;
				case 8000:
					num = 0.93f;
					break;
				case 9000:
					num = 0.935f;
					break;
				case 10000:
					num = 0.95f;
					break;
				case 15000:
					num = 0.902f;
					break;
				case 20000:
					num = 0.897f;
					break;
				case 25000:
					num = 0.912f;
					break;
				case 30000:
					num = 0.902f;
					break;
				case 35000:
					num = 0.922f;
					break;
				case 40000:
					num = 0.907f;
					break;
				case 45000:
					num = 0.932f;
					break;
				case 50000:
					num = 0.912f;
					break;
				}
				switch (iPlayerID)
				{
				case 0:
					switch (iBetID)
					{
					case 0:
						vector = new Vector3(0.712625f, 0.170625f, 0f);
						break;
					case 1:
						vector = new Vector3(0.6658f, 0.218375f, 0f);
						break;
					case 2:
						vector = new Vector3(0.756775f, 0.292775f, 0f);
						break;
					case 3:
						vector = new Vector3(0.701875f, 0.3439f, 0f);
						break;
					}
					break;
				case 1:
					switch (iBetID)
					{
					case 0:
						vector = new Vector3(0.278125f, -0.2571f, 0f);
						break;
					case 1:
						vector = new Vector3(0.280375f, -0.190375f, 0f);
						break;
					case 2:
						vector = new Vector3(0.397775f, -0.208525f, 0f);
						break;
					case 3:
						vector = new Vector3(0.39715f, -0.1354f, 0f);
						break;
					}
					break;
				case 2:
					switch (iBetID)
					{
					case 0:
						vector = new Vector3(-0.30305f, -0.2464f, 0f);
						break;
					case 1:
						vector = new Vector3(-0.257975f, -0.19715f, 0f);
						break;
					case 2:
						vector = new Vector3(-0.186575f, -0.2861f, 0f);
						break;
					case 3:
						vector = new Vector3(-0.141675f, -0.237925f, 0f);
						break;
					}
					break;
				case 3:
					switch (iBetID)
					{
					case 0:
						vector = new Vector3(-0.72855f, 0.17345f, 0f);
						break;
					case 1:
						vector = new Vector3(-0.652825f, 0.177525f, 0f);
						break;
					case 2:
						vector = new Vector3(-0.6783f, 0.0744f, 0f);
						break;
					case 3:
						vector = new Vector3(-0.604425f, 0.082575f, 0f);
						break;
					}
					break;
				}
			}
			else
			{
				switch (iBetAmount)
				{
				case 10:
					num = 0.95f;
					break;
				case 20:
					num = 0.896f;
					break;
				case 30:
					num = 0.901f;
					break;
				case 40:
					num = 0.907f;
					break;
				case 50:
					num = 0.95f;
					break;
				case 60:
					num = 0.917f;
					break;
				case 70:
					num = 0.922f;
					break;
				case 80:
					num = 0.927f;
					break;
				case 90:
					num = 0.932f;
					break;
				case 100:
					num = 0.95f;
					break;
				case 150:
					num = 0.904f;
					break;
				case 200:
					num = 0.899f;
					break;
				case 250:
					num = 0.914f;
					break;
				case 300:
					num = 0.904f;
					break;
				case 350:
					num = 0.924f;
					break;
				case 400:
					num = 0.91f;
					break;
				case 450:
					num = 0.935f;
					break;
				case 500:
					num = 0.95f;
					break;
				case 1000:
					num = 0.95f;
					break;
				case 1500:
					num = 0.904f;
					break;
				case 2000:
					num = 0.899f;
					break;
				case 2500:
					num = 0.915f;
					break;
				case 3000:
					num = 0.904f;
					break;
				case 3500:
					num = 0.925f;
					break;
				case 4000:
					num = 0.91f;
					break;
				case 4500:
					num = 0.935f;
					break;
				case 5000:
					num = 0.953f;
					break;
				case 6000:
					num = 0.919f;
					break;
				case 7000:
					num = 0.924f;
					break;
				case 8000:
					num = 0.93f;
					break;
				case 9000:
					num = 0.935f;
					break;
				case 10000:
					num = 0.95f;
					break;
				case 15000:
					num = 0.902f;
					break;
				case 20000:
					num = 0.897f;
					break;
				case 25000:
					num = 0.912f;
					break;
				case 30000:
					num = 0.902f;
					break;
				case 35000:
					num = 0.922f;
					break;
				case 40000:
					num = 0.907f;
					break;
				case 45000:
					num = 0.932f;
					break;
				case 50000:
					num = 0.912f;
					break;
				}
				switch (iBetAmount)
				{
				case 5000:
				case 10000:
				case 15000:
				case 20000:
				case 25000:
				case 30000:
				case 35000:
				case 40000:
				case 45000:
				case 50000:
					switch (iPlayerID)
					{
					case 0:
						switch (iBetID)
						{
						case 0:
							vector = new Vector3(0.6931f, 0.1952f, 0f);
							break;
						case 1:
							vector = new Vector3(0.724925f, 0.26955f, 0f);
							break;
						case 2:
							vector = new Vector3(0.7374f, 0.349625f, 0f);
							break;
						case 3:
							vector = new Vector3(0.76415f, 0.419225f, 0f);
							break;
						}
						break;
					case 1:
						switch (iBetID)
						{
						case 0:
							vector = new Vector3(0.2827f, -0.227825f, 0f);
							break;
						case 1:
							vector = new Vector3(0.3605f, -0.1898f, 0f);
							break;
						case 2:
							vector = new Vector3(0.4309f, -0.16365f, 0f);
							break;
						case 3:
							vector = new Vector3(0.49275f, -0.111575f, 0f);
							break;
						}
						break;
					case 2:
						switch (iBetID)
						{
						case 0:
							vector = new Vector3(-0.279425f, -0.2238f, 0f);
							break;
						case 1:
							vector = new Vector3(-0.200775f, -0.25855f, 0f);
							break;
						case 2:
							vector = new Vector3(-0.125775f, -0.26815f, 0f);
							break;
						case 3:
							vector = new Vector3(-0.05615f, -0.29435f, 0f);
							break;
						}
						break;
					case 3:
						switch (iBetID)
						{
						case 0:
							vector = new Vector3(-0.685925f, 0.173275f, 0f);
							break;
						case 1:
							vector = new Vector3(-0.6568f, 0.092525f, 0f);
							break;
						case 2:
							vector = new Vector3(-0.612875f, 0.033025f, 0f);
							break;
						case 3:
							vector = new Vector3(-0.58465f, -0.0374f, 0f);
							break;
						}
						break;
					}
					break;
				default:
					switch (iPlayerID)
					{
					case 0:
						switch (iBetID)
						{
						case 0:
							vector = new Vector3(0.712625f, 0.170625f, 0f);
							break;
						case 1:
							vector = new Vector3(0.6658f, 0.218375f, 0f);
							break;
						case 2:
							vector = new Vector3(0.756775f, 0.292775f, 0f);
							break;
						case 3:
							vector = new Vector3(0.701875f, 0.3439f, 0f);
							break;
						}
						break;
					case 1:
						switch (iBetID)
						{
						case 0:
							vector = new Vector3(0.278125f, -0.2571f, 0f);
							break;
						case 1:
							vector = new Vector3(0.280375f, -0.190375f, 0f);
							break;
						case 2:
							vector = new Vector3(0.397775f, -0.208525f, 0f);
							break;
						case 3:
							vector = new Vector3(0.39715f, -0.1354f, 0f);
							break;
						}
						break;
					case 2:
						switch (iBetID)
						{
						case 0:
							vector = new Vector3(-0.30305f, -0.2464f, 0f);
							break;
						case 1:
							vector = new Vector3(-0.257975f, -0.19715f, 0f);
							break;
						case 2:
							vector = new Vector3(-0.186575f, -0.2861f, 0f);
							break;
						case 3:
							vector = new Vector3(-0.141675f, -0.237925f, 0f);
							break;
						}
						break;
					case 3:
						switch (iBetID)
						{
						case 0:
							vector = new Vector3(-0.72855f, 0.17345f, 0f);
							break;
						case 1:
							vector = new Vector3(-0.652825f, 0.177525f, 0f);
							break;
						case 2:
							vector = new Vector3(-0.6783f, 0.0744f, 0f);
							break;
						case 3:
							vector = new Vector3(-0.604425f, 0.082575f, 0f);
							break;
						}
						break;
					}
					break;
				}
			}
			vector.Z = num;
			return num;
		}

		public static Vector3 GetDealerCardPositionOffset(int iCardID)
		{
			switch (iCardID)
			{
			case 0:
				return new Vector3(0.0356f, 0.2105f, 0.94885f);
			case 1:
				return new Vector3(-0.0436f, 0.21205f, 0.948875f);
			case 2:
				return new Vector3(-0.0636f, 0.213825f, 0.9496f);
			case 3:
				return new Vector3(-0.0806f, 0.2137f, 0.950225f);
			case 4:
				return new Vector3(-0.1006f, 0.21125f, 0.950875f);
			case 5:
				return new Vector3(-0.1256f, 0.21505f, 0.951875f);
			case 6:
				return new Vector3(-0.1416f, 0.21305f, 0.953f);
			case 7:
				return new Vector3(-0.1656f, 0.21205f, 0.954025f);
			case 8:
				return new Vector3(-0.1836f, 0.21255f, 0.95495f);
			case 9:
				return new Vector3(-0.2076f, 0.21105f, 0.956025f);
			case 10:
				return new Vector3(-0.2246f, 0.21305f, 0.957f);
			default:
				return new Vector3(0f, 0f, 0f);
			}
		}

		public static Vector3 GetPlayerCardRotationOffset(int iCardID, int iPlayerID, bool bSecondHand = false, bool bLastCard = false)
		{
			Vector3 result = new Vector3(0f, 0f, 0f);
			if (!bSecondHand)
			{
				switch (iPlayerID)
				{
				case 0:
					switch (iCardID)
					{
					case 0:
						result = new Vector3(0f, 0f, 69.12f);
						break;
					case 1:
						result = new Vector3(0f, 0f, 67.8f);
						break;
					case 2:
						result = new Vector3(0f, 0f, 66.6f);
						break;
					case 3:
						result = new Vector3(0f, 0f, 70.44f);
						break;
					case 4:
						result = new Vector3(0f, 0f, 70.84f);
						break;
					case 5:
						result = new Vector3(0f, 0f, 67.88f);
						break;
					case 6:
						result = new Vector3(0f, 0f, 69.56f);
						break;
					}
					break;
				case 1:
					switch (iCardID)
					{
					case 0:
						result = new Vector3(0f, 0f, 22.11f);
						break;
					case 1:
						result = new Vector3(0f, 0f, 22.32f);
						break;
					case 2:
						result = new Vector3(0f, 0f, 20.8f);
						break;
					case 3:
						result = new Vector3(0f, 0f, 19.8f);
						break;
					case 4:
						result = new Vector3(0f, 0f, 19.44f);
						break;
					case 5:
						result = new Vector3(0f, 0f, 26.28f);
						break;
					case 6:
						result = new Vector3(0f, 0f, 22.68f);
						break;
					}
					break;
				case 2:
					switch (iCardID)
					{
					case 0:
						result = new Vector3(0f, 0f, -21.43f);
						break;
					case 1:
						result = new Vector3(0f, 0f, -20.16f);
						break;
					case 2:
						result = new Vector3(0f, 0f, -16.92f);
						break;
					case 3:
						result = new Vector3(0f, 0f, -23.4f);
						break;
					case 4:
						result = new Vector3(0f, 0f, -21.24f);
						break;
					case 5:
						result = new Vector3(0f, 0f, -23.76f);
						break;
					case 6:
						result = new Vector3(0f, 0f, -19.44f);
						break;
					}
					break;
				case 3:
					switch (iCardID)
					{
					case 0:
						result = new Vector3(0f, 0f, -67.03f);
						break;
					case 1:
						result = new Vector3(0f, 0f, -69.12f);
						break;
					case 2:
						result = new Vector3(0f, 0f, -64.44f);
						break;
					case 3:
						result = new Vector3(0f, 0f, -67.68f);
						break;
					case 4:
						result = new Vector3(0f, 0f, -63.72f);
						break;
					case 5:
						result = new Vector3(0f, 0f, -68.4f);
						break;
					case 6:
						result = new Vector3(0f, 0f, -64.44f);
						break;
					}
					break;
				}
			}
			else
			{
				switch (iPlayerID)
				{
				case 0:
					switch (iCardID)
					{
					case 0:
						result = new Vector3(0f, 0f, 68.57f);
						break;
					case 1:
						result = new Vector3(0f, 0f, 67.52f);
						break;
					case 2:
						result = new Vector3(0f, 0f, 67.76f);
						break;
					case 3:
						result = new Vector3(0f, 0f, 67.04f);
						break;
					case 4:
						result = new Vector3(0f, 0f, 68.84f);
						break;
					case 5:
						result = new Vector3(0f, 0f, 65.96f);
						break;
					case 6:
						result = new Vector3(0f, 0f, 67.76f);
						break;
					}
					break;
				case 1:
					switch (iCardID)
					{
					case 0:
						result = new Vector3(0f, 0f, 22.11f);
						break;
					case 1:
						result = new Vector3(0f, 0f, 22f);
						break;
					case 2:
						result = new Vector3(0f, 0f, 24.44f);
						break;
					case 3:
						result = new Vector3(0f, 0f, 21.08f);
						break;
					case 4:
						result = new Vector3(0f, 0f, 25.96f);
						break;
					case 5:
						result = new Vector3(0f, 0f, 26.16f);
						break;
					case 6:
						result = new Vector3(0f, 0f, 28.76f);
						break;
					}
					break;
				case 2:
					switch (iCardID)
					{
					case 0:
						result = new Vector3(0f, 0f, -14.04f);
						break;
					case 1:
						result = new Vector3(0f, 0f, -15.48f);
						break;
					case 2:
						result = new Vector3(0f, 0f, -16.56f);
						break;
					case 3:
						result = new Vector3(0f, 0f, -15.84f);
						break;
					case 4:
						result = new Vector3(0f, 0f, -16.92f);
						break;
					case 5:
						result = new Vector3(0f, 0f, -14.4f);
						break;
					case 6:
						result = new Vector3(0f, 0f, -14.28f);
						break;
					}
					break;
				case 3:
					switch (iCardID)
					{
					case 0:
						result = new Vector3(0f, 0f, -67.03f);
						break;
					case 1:
						result = new Vector3(0f, 0f, -67.6f);
						break;
					case 2:
						result = new Vector3(0f, 0f, -69.4f);
						break;
					case 3:
						result = new Vector3(0f, 0f, -69.04f);
						break;
					case 4:
						result = new Vector3(0f, 0f, -68.68f);
						break;
					case 5:
						result = new Vector3(0f, 0f, -66.16f);
						break;
					case 6:
						result = new Vector3(0f, 0f, -63.28f);
						break;
					}
					break;
				}
			}
			if (bLastCard)
			{
				result.Z += 90f;
			}
			return result;
		}

		public static Vector3 GetPlayerCardPositionOffset(int iCardID, int iPlayerID, bool bSecondHand = false)
		{
			if (!bSecondHand)
			{
				switch (iPlayerID)
				{
				case 0:
					switch (iCardID)
					{
					case 0:
						return new Vector3(0.5737f, 0.2376f, 0.948025f);
					case 1:
						return new Vector3(0.562975f, 0.2523f, 0.94875f);
					case 2:
						return new Vector3(0.553875f, 0.266325f, 0.94955f);
					case 3:
						return new Vector3(0.5459f, 0.282075f, 0.9501f);
					case 4:
						return new Vector3(0.536125f, 0.29645f, 0.95085f);
					case 5:
						return new Vector3(0.524975f, 0.30975f, 0.9516f);
					case 6:
						return new Vector3(0.515775f, 0.325325f, 0.95235f);
					}
					break;
				case 1:
					switch (iCardID)
					{
					case 0:
						return new Vector3(0.2325f, -0.1082f, 0.94805f);
					case 1:
						return new Vector3(0.23645f, -0.0918f, 0.949f);
					case 2:
						return new Vector3(0.2401f, -0.074475f, 0.950225f);
					case 3:
						return new Vector3(0.244625f, -0.057675f, 0.951125f);
					case 4:
						return new Vector3(0.249675f, -0.041475f, 0.95205f);
					case 5:
						return new Vector3(0.257575f, -0.0256f, 0.9532f);
					case 6:
						return new Vector3(0.2601f, -0.008175f, 0.954375f);
					}
					break;
				case 2:
					switch (iCardID)
					{
					case 0:
						return new Vector3(-0.2359f, -0.1091f, 0.9483f);
					case 1:
						return new Vector3(-0.221025f, -0.100675f, 0.949f);
					case 2:
						return new Vector3(-33f / 160f, -0.092875f, 0.949725f);
					case 3:
						return new Vector3(-0.193225f, -0.07985f, 0.950325f);
					case 4:
						return new Vector3(-0.1776f, -0.072f, 0.951025f);
					case 5:
						return new Vector3(-0.165f, -0.060025f, 0.951825f);
					case 6:
						return new Vector3(-0.14895f, -0.05155f, 0.95255f);
					}
					break;
				case 3:
					switch (iCardID)
					{
					case 0:
						return new Vector3(-0.5765f, 0.2229f, 0.9482f);
					case 1:
						return new Vector3(-0.558925f, 0.2197f, 0.949175f);
					case 2:
						return new Vector3(-0.5425f, 0.213025f, 0.9499f);
					case 3:
						return new Vector3(-0.525925f, 0.21105f, 0.95095f);
					case 4:
						return new Vector3(-0.509475f, 0.20535f, 0.9519f);
					case 5:
						return new Vector3(-0.491775f, 0.204075f, 0.952825f);
					case 6:
						return new Vector3(-0.4752f, 0.197525f, 0.9543f);
					}
					break;
				}
			}
			else
			{
				switch (iPlayerID)
				{
				case 0:
					switch (iCardID)
					{
					case 0:
						return new Vector3(0.6083f, 0.3523f, 0.94795f);
					case 1:
						return new Vector3(0.598475f, 0.366475f, 0.948925f);
					case 2:
						return new Vector3(0.589525f, 0.3807f, 0.94975f);
					case 3:
						return new Vector3(0.58045f, 0.39435f, 0.950375f);
					case 4:
						return new Vector3(0.571975f, 0.4092f, 0.951075f);
					case 5:
						return new Vector3(0.5614f, 0.4237f, 0.951775f);
					case 6:
						return new Vector3(0.554325f, 0.4402f, 0.952525f);
					}
					break;
				case 1:
					switch (iCardID)
					{
					case 0:
						return new Vector3(0.3431f, -0.0527f, 0.94855f);
					case 1:
						return new Vector3(0.348575f, -0.0348f, 0.949425f);
					case 2:
						return new Vector3(0.35465f, -0.018825f, 0.9502f);
					case 3:
						return new Vector3(0.3581f, -0.001625f, 0.95115f);
					case 4:
						return new Vector3(0.36515f, 0.015275f, 0.952075f);
					case 5:
						return new Vector3(0.368525f, 0.032475f, 0.95335f);
					case 6:
						return new Vector3(0.373275f, 0.0506f, 0.9543f);
					}
					break;
				case 2:
					switch (iCardID)
					{
					case 0:
						return new Vector3(-0.116f, -0.1501f, 0.947875f);
					case 1:
						return new Vector3(-0.102725f, -0.13795f, 0.948525f);
					case 2:
						return new Vector3(-0.08975f, -0.12665f, 0.949175f);
					case 3:
						return new Vector3(-0.075025f, -0.1159f, 0.949875f);
					case 4:
						return new Vector3(-0.0614f, -0.104775f, 0.9507f);
					case 5:
						return new Vector3(-0.046275f, -0.095025f, 0.9516f);
					case 6:
						return new Vector3(-0.031425f, -0.0846f, 0.952675f);
					}
					break;
				case 3:
					switch (iCardID)
					{
					case 0:
						return new Vector3(-0.5205f, 0.1122f, 0.9478f);
					case 1:
						return new Vector3(-0.503175f, 0.108525f, 0.94865f);
					case 2:
						return new Vector3(-0.485125f, 0.10475f, 0.949175f);
					case 3:
						return new Vector3(-0.468275f, 0.099175f, 0.94995f);
					case 4:
						return new Vector3(-0.45155f, 0.09435f, 0.95085f);
					case 5:
						return new Vector3(-0.434475f, 0.089725f, 0.95145f);
					case 6:
						return new Vector3(-0.415875f, 0.0846f, 0.9523f);
					}
					break;
				}
			}
			return new Vector3(0f, 0f, 0f);
		}

		public static string GetBetChipModel(int iBetAmount, bool bHighLimitTable)
		{
			if (!bHighLimitTable)
			{
				switch (iBetAmount)
				{
				case 10:
					return "vw_prop_chip_10dollar_x1";
				case 20:
					return "vw_prop_chip_10dollar_st";
				case 30:
					return "vw_prop_chip_10dollar_st";
				case 40:
					return "vw_prop_chip_10dollar_st";
				case 50:
					return "vw_prop_chip_50dollar_x1";
				case 60:
					return "vw_prop_chip_10dollar_st";
				case 70:
					return "vw_prop_chip_10dollar_st";
				case 80:
					return "vw_prop_chip_10dollar_st";
				case 90:
					return "vw_prop_chip_10dollar_st";
				case 100:
					return "vw_prop_chip_100dollar_x1";
				case 150:
					return "vw_prop_chip_50dollar_st";
				case 200:
					return "vw_prop_chip_100dollar_st";
				case 250:
					return "vw_prop_chip_50dollar_st";
				case 300:
					return "vw_prop_chip_100dollar_st";
				case 350:
					return "vw_prop_chip_50dollar_st";
				case 400:
					return "vw_prop_chip_100dollar_st";
				case 450:
					return "vw_prop_chip_50dollar_st";
				case 500:
					return "vw_prop_chip_500dollar_x1";
				case 1000:
					return "vw_prop_chip_1kdollar_x1";
				case 1500:
					return "vw_prop_chip_500dollar_st";
				case 2000:
					return "vw_prop_chip_1kdollar_st";
				case 2500:
					return "vw_prop_chip_500dollar_st";
				case 3000:
					return "vw_prop_chip_1kdollar_st";
				case 3500:
					return "vw_prop_chip_500dollar_st";
				case 4000:
					return "vw_prop_chip_1kdollar_st";
				case 4500:
					return "vw_prop_chip_500dollar_st";
				case 5000:
					return "vw_prop_chip_5kdollar_x1";
				case 6000:
					return "vw_prop_chip_1kdollar_st";
				case 7000:
					return "vw_prop_chip_1kdollar_st";
				case 8000:
					return "vw_prop_chip_1kdollar_st";
				case 9000:
					return "vw_prop_chip_1kdollar_st";
				case 10000:
					return "vw_prop_chip_10kdollar_x1";
				case 15000:
					return "vw_prop_chip_5kdollar_st";
				case 20000:
					return "vw_prop_chip_10kdollar_st";
				case 25000:
					return "vw_prop_chip_5kdollar_st";
				case 30000:
					return "vw_prop_chip_10kdollar_st";
				case 35000:
					return "vw_prop_chip_5kdollar_st";
				case 40000:
					return "vw_prop_chip_10kdollar_st";
				case 45000:
					return "vw_prop_chip_5kdollar_st";
				case 50000:
					return "vw_prop_chip_10kdollar_st";
				}
			}
			else
			{
				switch (iBetAmount)
				{
				case 10:
					return "vw_prop_chip_10dollar_x1";
				case 20:
					return "vw_prop_chip_10dollar_st";
				case 30:
					return "vw_prop_chip_10dollar_st";
				case 40:
					return "vw_prop_chip_10dollar_st";
				case 50:
					return "vw_prop_chip_50dollar_x1";
				case 60:
					return "vw_prop_chip_10dollar_st";
				case 70:
					return "vw_prop_chip_10dollar_st";
				case 80:
					return "vw_prop_chip_10dollar_st";
				case 90:
					return "vw_prop_chip_10dollar_st";
				case 100:
					return "vw_prop_chip_100dollar_x1";
				case 150:
					return "vw_prop_chip_50dollar_st";
				case 200:
					return "vw_prop_chip_100dollar_st";
				case 250:
					return "vw_prop_chip_50dollar_st";
				case 300:
					return "vw_prop_chip_100dollar_st";
				case 350:
					return "vw_prop_chip_50dollar_st";
				case 400:
					return "vw_prop_chip_100dollar_st";
				case 450:
					return "vw_prop_chip_50dollar_st";
				case 500:
					return "vw_prop_chip_500dollar_x1";
				case 1000:
					return "vw_prop_chip_1kdollar_x1";
				case 1500:
					return "vw_prop_chip_500dollar_st";
				case 2000:
					return "vw_prop_chip_1kdollar_st";
				case 2500:
					return "vw_prop_chip_500dollar_st";
				case 3000:
					return "vw_prop_chip_1kdollar_st";
				case 3500:
					return "vw_prop_chip_500dollar_st";
				case 4000:
					return "vw_prop_chip_1kdollar_st";
				case 4500:
					return "vw_prop_chip_500dollar_st";
				case 5000:
					return "vw_prop_plaq_5kdollar_x1";
				case 6000:
					return "vw_prop_chip_1kdollar_st";
				case 7000:
					return "vw_prop_chip_1kdollar_st";
				case 8000:
					return "vw_prop_chip_1kdollar_st";
				case 9000:
					return "vw_prop_chip_1kdollar_st";
				case 10000:
					return "vw_prop_plaq_10kdollar_x1";
				case 15000:
					return "vw_prop_plaq_5kdollar_st";
				case 20000:
					return "vw_prop_plaq_10kdollar_st";
				case 25000:
					return "vw_prop_plaq_5kdollar_st";
				case 30000:
					return "vw_prop_plaq_10kdollar_st";
				case 35000:
					return "vw_prop_plaq_5kdollar_st";
				case 40000:
					return "vw_prop_plaq_10kdollar_st";
				case 45000:
					return "vw_prop_plaq_5kdollar_st";
				case 50000:
					return "vw_prop_plaq_10kdollar_st";
				}
			}
			if (iBetAmount >= 50000)
			{
				return "vw_prop_plaq_10kdollar_st";
			}
			return "vw_prop_chip_10dollar_x1";
		}

		public static int GetBetFromIndex(int Index)
		{
			switch (Index)
			{
			case 1:
				return 10;
			case 2:
				return 20;
			case 3:
				return 30;
			case 4:
				return 40;
			case 5:
				return 50;
			case 6:
				return 60;
			case 7:
				return 70;
			case 8:
				return 80;
			case 9:
				return 90;
			case 10:
				return 100;
			case 11:
				return 150;
			case 12:
				return 200;
			case 13:
				return 250;
			case 14:
				return 300;
			case 15:
				return 350;
			case 16:
				return 400;
			case 17:
				return 450;
			case 18:
				return 500;
			case 19:
				return 1000;
			case 20:
				return 1500;
			case 21:
				return 2000;
			case 22:
				return 2500;
			case 23:
				return 3000;
			case 24:
				return 3500;
			case 25:
				return 4000;
			case 26:
				return 4500;
			case 27:
				return 5000;
			case 28:
				return 6000;
			case 29:
				return 7000;
			case 30:
				return 8000;
			case 31:
				return 9000;
			case 32:
				return 10000;
			case 33:
				return 15000;
			case 34:
				return 20000;
			case 35:
				return 25000;
			case 36:
				return 30000;
			case 37:
				return 35000;
			case 38:
				return 40000;
			case 39:
				return 45000;
			case 40:
				return 50000;
			case 41:
				return 65000;
			case 42:
				return 70000;
			case 43:
				return 75000;
			case 44:
				return 80000;
			case 45:
				return 85000;
			case 46:
				return 90000;
			case 47:
				return 95000;
			case 48:
				return 100000;
			case 49:
				return 105000;
			case 50:
				return 110000;
			case 51:
				return 115000;
			case 52:
				return 120000;
			case 53:
				return 125000;
			case 54:
				return 130000;
			case 55:
				return 135000;
			case 56:
				return 145000;
			case 57:
				return 150000;
			case 58:
				return 160000;
			case 59:
				return 170000;
			case 60:
				return 180000;
			case 61:
				return 190000;
			case 62:
				return 200000;
			case 63:
				return 210000;
			case 64:
				return 220000;
			case 65:
				return 230000;
			case 66:
				return 240000;
			case 67:
				return 250000;
			default:
				return 10;
			}
		}

		public static int HASH(string input)
		{
			return Function.Call<int>(Hash.GET_HASH_KEY, input);
		}

		public static void drawSprite2(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha = 255)
		{
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, screenX, screenY, width, height, 0, r, g, b, alpha, 0);
			}
		}

		public static void drawSprite2(string textureDict, string textureName, float screenX, float screenY, float width, float height)
		{
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, screenX, screenY, width, height, 0, 255, 255, 255, 200, 0);
			}
		}

		public static void drawNumber(int moneh, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "ESDOLLA");
			Function.Call(Hash.ADD_TEXT_COMPONENT_INTEGER, moneh);
			Function.Call(Hash.ADD_TEXT_COMPONENT_FORMATTED_INTEGER, moneh, 1);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		public static void drawNumberS(int moneh, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_INTEGER, moneh);
			Function.Call(Hash.ADD_TEXT_COMPONENT_FORMATTED_INTEGER, moneh, 1);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		public static void drawNumberStr(string moneh, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, moneh);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		public static ValueTuple<int, int> AddBetToWinnings(string BetPlacement, int Bet, string Prize, int WinningNumber)
		{
			int num = 0;
			for (int i = 1; i < 37; i++)
			{
				if (BetPlacement.Equals(Prize))
				{
					int num2 = i;
					if (WinningNumber == num2)
					{
						num = Bet;
					}
				}
			}
			if (BetPlacement.Equals("0") && WinningNumber == 37)
			{
				num = Bet;
			}
			if (BetPlacement.Equals("00") && WinningNumber == 38)
			{
				num = Bet;
			}
			if (BetPlacement.Equals("1st Column") && (WinningNumber == 1 || WinningNumber == 4 || WinningNumber == 7 || WinningNumber == 10 || WinningNumber == 13 || WinningNumber == 16 || WinningNumber == 19 || WinningNumber == 22 || WinningNumber == 25 || WinningNumber == 28 || WinningNumber == 31 || WinningNumber == 34))
			{
				num = Bet;
			}
			if (BetPlacement.Equals("2nd Column") && (WinningNumber == 2 || WinningNumber == 5 || WinningNumber == 9 || WinningNumber == 11 || WinningNumber == 14 || WinningNumber == 17 || WinningNumber == 20 || WinningNumber == 23 || WinningNumber == 26 || WinningNumber == 29 || WinningNumber == 32 || WinningNumber == 35))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("3rd Column") && (WinningNumber == 3 || WinningNumber == 6 || WinningNumber == 10 || WinningNumber == 12 || WinningNumber == 15 || WinningNumber == 18 || WinningNumber == 21 || WinningNumber == 24 || WinningNumber == 27 || WinningNumber == 30 || WinningNumber == 33 || WinningNumber == 36))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("3rd 12") && (WinningNumber == 25 || WinningNumber == 26 || WinningNumber == 27 || WinningNumber == 28 || WinningNumber == 29 || WinningNumber == 30 || WinningNumber == 31 || WinningNumber == 32 || WinningNumber == 33 || WinningNumber == 34 || WinningNumber == 35 || WinningNumber == 36))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("2nd 12") && (WinningNumber == 13 || WinningNumber == 14 || WinningNumber == 15 || WinningNumber == 16 || WinningNumber == 17 || WinningNumber == 18 || WinningNumber == 19 || WinningNumber == 20 || WinningNumber == 21 || WinningNumber == 22 || WinningNumber == 23 || WinningNumber == 24))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("1st 12") && (WinningNumber == 1 || WinningNumber == 2 || WinningNumber == 3 || WinningNumber == 4 || WinningNumber == 5 || WinningNumber == 6 || WinningNumber == 7 || WinningNumber == 8 || WinningNumber == 9 || WinningNumber == 10 || WinningNumber == 11 || WinningNumber == 12))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("1 to 18") && (WinningNumber == 1 || WinningNumber == 2 || WinningNumber == 3 || WinningNumber == 4 || WinningNumber == 5 || WinningNumber == 6 || WinningNumber == 7 || WinningNumber == 8 || WinningNumber == 9 || WinningNumber == 10 || WinningNumber == 11 || WinningNumber == 12 || WinningNumber == 13 || WinningNumber == 14 || WinningNumber == 15 || WinningNumber == 16 || WinningNumber == 17 || WinningNumber == 18))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Even") && (WinningNumber == 2 || WinningNumber == 4 || WinningNumber == 6 || WinningNumber == 8 || WinningNumber == 10 || WinningNumber == 12 || WinningNumber == 14 || WinningNumber == 16 || WinningNumber == 18 || WinningNumber == 20 || WinningNumber == 22 || WinningNumber == 24 || WinningNumber == 26 || WinningNumber == 28 || WinningNumber == 30 || WinningNumber == 32 || WinningNumber == 34 || WinningNumber == 36))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Red") && (WinningNumber == 1 || WinningNumber == 3 || WinningNumber == 5 || WinningNumber == 7 || WinningNumber == 9 || WinningNumber == 12 || WinningNumber == 14 || WinningNumber == 16 || WinningNumber == 18 || WinningNumber == 19 || WinningNumber == 21 || WinningNumber == 23 || WinningNumber == 25 || WinningNumber == 27 || WinningNumber == 30 || WinningNumber == 32 || WinningNumber == 34 || WinningNumber == 36))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Black") && (WinningNumber == 2 || WinningNumber == 4 || WinningNumber == 6 || WinningNumber == 8 || WinningNumber == 10 || WinningNumber == 11 || WinningNumber == 13 || WinningNumber == 15 || WinningNumber == 17 || WinningNumber == 20 || WinningNumber == 22 || WinningNumber == 24 || WinningNumber == 26 || WinningNumber == 28 || WinningNumber == 29 || WinningNumber == 33 || WinningNumber == 35))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Odd") && (WinningNumber == 1 || WinningNumber == 3 || WinningNumber == 5 || WinningNumber == 7 || WinningNumber == 8 || WinningNumber == 11 || WinningNumber == 13 || WinningNumber == 15 || WinningNumber == 17 || WinningNumber == 19 || WinningNumber == 21 || WinningNumber == 23 || WinningNumber == 25 || WinningNumber == 27 || WinningNumber == 29 || WinningNumber == 31 || WinningNumber == 33 || WinningNumber == 35))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("19 to 36") && (WinningNumber == 19 || WinningNumber == 20 || WinningNumber == 21 || WinningNumber == 22 || WinningNumber == 23 || WinningNumber == 24 || WinningNumber == 25 || WinningNumber == 26 || WinningNumber == 27 || WinningNumber == 28 || WinningNumber == 29 || WinningNumber == 30 || WinningNumber == 31 || WinningNumber == 32 || WinningNumber == 33 || WinningNumber == 34 || WinningNumber == 35 || WinningNumber == 36))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 1, 2, 4, 5") && (WinningNumber == 1 || WinningNumber == 2 || WinningNumber == 4 || WinningNumber == 5))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 2, 3, 5, 6") && (WinningNumber == 2 || WinningNumber == 3 || WinningNumber == 5 || WinningNumber == 6))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 4, 5, 7, 8") && (WinningNumber == 4 || WinningNumber == 5 || WinningNumber == 7 || WinningNumber == 8))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 5, 6, 8, 9") && (WinningNumber == 5 || WinningNumber == 6 || WinningNumber == 8 || WinningNumber == 9))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 7, 8, 10, 11") && (WinningNumber == 7 || WinningNumber == 8 || WinningNumber == 10 || WinningNumber == 11))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 8, 9, 11, 12") && (WinningNumber == 8 || WinningNumber == 9 || WinningNumber == 11 || WinningNumber == 12))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 10, 11, 13, 14") && (WinningNumber == 10 || WinningNumber == 11 || WinningNumber == 13 || WinningNumber == 14))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 11, 12, 14, 15") && (WinningNumber == 11 || WinningNumber == 12 || WinningNumber == 14 || WinningNumber == 15))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 13, 14, 16, 17") && (WinningNumber == 13 || WinningNumber == 14 || WinningNumber == 16 || WinningNumber == 17))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 14, 15, 17, 18") && (WinningNumber == 14 || WinningNumber == 15 || WinningNumber == 17 || WinningNumber == 18))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 16, 17, 19, 20") && (WinningNumber == 16 || WinningNumber == 17 || WinningNumber == 19 || WinningNumber == 20))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 17, 18, 20, 21") && (WinningNumber == 17 || WinningNumber == 18 || WinningNumber == 20 || WinningNumber == 21))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 19, 20, 22, 23") && (WinningNumber == 19 || WinningNumber == 20 || WinningNumber == 22 || WinningNumber == 23))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 20, 21, 23, 24") && (WinningNumber == 20 || WinningNumber == 21 || WinningNumber == 23 || WinningNumber == 24))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 22, 23, 25, 26") && (WinningNumber == 22 || WinningNumber == 23 || WinningNumber == 25 || WinningNumber == 26))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 23, 24, 26, 27") && (WinningNumber == 23 || WinningNumber == 24 || WinningNumber == 26 || WinningNumber == 27))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 25, 26, 28, 29") && (WinningNumber == 25 || WinningNumber == 26 || WinningNumber == 28 || WinningNumber == 29))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 26, 27, 29, 30") && (WinningNumber == 26 || WinningNumber == 27 || WinningNumber == 29 || WinningNumber == 30))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 28, 29, 31, 32") && (WinningNumber == 28 || WinningNumber == 29 || WinningNumber == 31 || WinningNumber == 32))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 29, 30, 32, 33") && (WinningNumber == 29 || WinningNumber == 30 || WinningNumber == 32 || WinningNumber == 33))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 31, 32, 34, 35") && (WinningNumber == 31 || WinningNumber == 32 || WinningNumber == 34 || WinningNumber == 35))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 32, 33, 35, 36") && (WinningNumber == 32 || WinningNumber == 33 || WinningNumber == 35 || WinningNumber == 36))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Row#1 1, 2, 3") && (WinningNumber == 1 || WinningNumber == 2 || WinningNumber == 3))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Segment#1 1, 2, 3, 4, 5, 6") && (WinningNumber == 1 || WinningNumber == 2 || WinningNumber == 3 || WinningNumber == 4 || WinningNumber == 5 || WinningNumber == 6))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Row#2 4, 5, 6") && (WinningNumber == 4 || WinningNumber == 5 || WinningNumber == 6))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Segment#2 4, 5, 6, 7, 8, 9") && (WinningNumber == 4 || WinningNumber == 5 || WinningNumber == 6 || WinningNumber == 7 || WinningNumber == 8 || WinningNumber == 9))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Row#3 7, 8, 9") && (WinningNumber == 7 || WinningNumber == 8 || WinningNumber == 9))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Segment#3 7, 8, 9, 10, 11, 12") && (WinningNumber == 7 || WinningNumber == 8 || WinningNumber == 9 || WinningNumber == 10 || WinningNumber == 11 || WinningNumber == 12))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Row#4 10, 11, 12") && (WinningNumber == 10 || WinningNumber == 11 || WinningNumber == 12))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Segment#4 10, 11, 12, 13, 14, 15") && (WinningNumber == 10 || WinningNumber == 11 || WinningNumber == 12 || WinningNumber == 13 || WinningNumber == 14 || WinningNumber == 15))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Row#5 13, 14, 15") && (WinningNumber == 13 || WinningNumber == 14 || WinningNumber == 15))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Segment#5 13, 14, 15, 16, 17, 18") && (WinningNumber == 13 || WinningNumber == 14 || WinningNumber == 15 || WinningNumber == 16 || WinningNumber == 17 || WinningNumber == 18))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Row#6 16, 17, 18") && (WinningNumber == 16 || WinningNumber == 17 || WinningNumber == 18))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Segment#6 16, 17, 18, 19, 20, 21") && (WinningNumber == 16 || WinningNumber == 17 || WinningNumber == 18 || WinningNumber == 19 || WinningNumber == 20 || WinningNumber == 21))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Row#7 19, 20, 21") && (WinningNumber == 19 || WinningNumber == 20 || WinningNumber == 21))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Segment#7 19, 20, 21, 22, 23, 24") && (WinningNumber == 19 || WinningNumber == 20 || WinningNumber == 21 || WinningNumber == 22 || WinningNumber == 23 || WinningNumber == 24))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Row#8 22, 23, 24") && (WinningNumber == 22 || WinningNumber == 23 || WinningNumber == 24))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Segment#8 22, 23, 24, 25, 26, 27") && (WinningNumber == 22 || WinningNumber == 23 || WinningNumber == 24 || WinningNumber == 25 || WinningNumber == 26 || WinningNumber == 27))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Row#9 25, 26, 27") && (WinningNumber == 25 || WinningNumber == 26 || WinningNumber == 27))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Segment#9 25, 26, 27, 28, 29, 30") && (WinningNumber == 25 || WinningNumber == 26 || WinningNumber == 27 || WinningNumber == 28 || WinningNumber == 29 || WinningNumber == 30))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Row#10 28, 29, 30") && (WinningNumber == 28 || WinningNumber == 29 || WinningNumber == 30))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Segment#10 28, 29, 30, 31, 32, 33") && (WinningNumber == 28 || WinningNumber == 29 || WinningNumber == 30 || WinningNumber == 31 || WinningNumber == 32 || WinningNumber == 33))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Row#11 31, 32, 33") && (WinningNumber == 31 || WinningNumber == 32 || WinningNumber == 33))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Segment#11 31, 32, 33, 34, 35, 36") && (WinningNumber == 31 || WinningNumber == 32 || WinningNumber == 33 || WinningNumber == 34 || WinningNumber == 35 || WinningNumber == 36))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Row#12 34, 35, 36") && (WinningNumber == 34 || WinningNumber == 35 || WinningNumber == 36))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 1, 2, 3, 0, 00") && (WinningNumber == 1 || WinningNumber == 2 || WinningNumber == 3 || WinningNumber == 37 || WinningNumber == 38))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 1 & 2") && (WinningNumber == 1 || WinningNumber == 2))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 2 & 3") && (WinningNumber == 2 || WinningNumber == 3))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 4 & 5") && (WinningNumber == 4 || WinningNumber == 5))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 5 & 6") && (WinningNumber == 5 || WinningNumber == 6))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 7 & 8") && (WinningNumber == 7 || WinningNumber == 8))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 8 & 9") && (WinningNumber == 8 || WinningNumber == 9))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 10 & 11") && (WinningNumber == 10 || WinningNumber == 11))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 11 & 12") && (WinningNumber == 11 || WinningNumber == 12))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 13 & 14") && (WinningNumber == 13 || WinningNumber == 14))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 14 & 15") && (WinningNumber == 14 || WinningNumber == 15))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 16 & 17") && (WinningNumber == 16 || WinningNumber == 17))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 17 & 18") && (WinningNumber == 17 || WinningNumber == 18))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 19 & 20") && (WinningNumber == 19 || WinningNumber == 20))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 20 & 21") && (WinningNumber == 20 || WinningNumber == 21))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 22 & 23") && (WinningNumber == 22 || WinningNumber == 23))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 23 & 24") && (WinningNumber == 23 || WinningNumber == 24))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 25 & 26") && (WinningNumber == 25 || WinningNumber == 26))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 26 & 27") && (WinningNumber == 26 || WinningNumber == 27))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 28 & 29") && (WinningNumber == 28 || WinningNumber == 29))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 29 & 30") && (WinningNumber == 29 || WinningNumber == 30))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 31 & 32") && (WinningNumber == 31 || WinningNumber == 32))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 32 & 33") && (WinningNumber == 32 || WinningNumber == 33))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 34 & 35") && (WinningNumber == 34 || WinningNumber == 35))
			{
				num += Bet;
			}
			if (BetPlacement.Equals("Numbers 35 & 36") && (WinningNumber == 35 || WinningNumber == 36))
			{
				num += Bet;
			}
			return new ValueTuple<int, int>(num, Bet);
		}

		public static bool GetDidNumberWin(string BetPlacement, int Bet, string Prize, int WinningNumber)
		{
			bool result = false;
			for (int i = 1; i < 37; i++)
			{
				if (BetPlacement.Equals(Prize))
				{
					int num = i;
					if (WinningNumber == num)
					{
						result = true;
					}
				}
			}
			if (BetPlacement.Equals("0") && WinningNumber == 37)
			{
				result = true;
			}
			if (BetPlacement.Equals("00") && WinningNumber == 38)
			{
				result = true;
			}
			if (BetPlacement.Equals("1st Column") && (WinningNumber == 1 || WinningNumber == 4 || WinningNumber == 7 || WinningNumber == 10 || WinningNumber == 13 || WinningNumber == 16 || WinningNumber == 19 || WinningNumber == 22 || WinningNumber == 25 || WinningNumber == 28 || WinningNumber == 31 || WinningNumber == 34))
			{
				result = true;
			}
			if (BetPlacement.Equals("2nd Column") && (WinningNumber == 2 || WinningNumber == 5 || WinningNumber == 9 || WinningNumber == 11 || WinningNumber == 14 || WinningNumber == 17 || WinningNumber == 20 || WinningNumber == 23 || WinningNumber == 26 || WinningNumber == 29 || WinningNumber == 32 || WinningNumber == 35))
			{
				result = true;
			}
			if (BetPlacement.Equals("3rd Column") && (WinningNumber == 3 || WinningNumber == 6 || WinningNumber == 10 || WinningNumber == 12 || WinningNumber == 15 || WinningNumber == 18 || WinningNumber == 21 || WinningNumber == 24 || WinningNumber == 27 || WinningNumber == 30 || WinningNumber == 33 || WinningNumber == 36))
			{
				result = true;
			}
			if (BetPlacement.Equals("3rd 12") && (WinningNumber == 25 || WinningNumber == 26 || WinningNumber == 27 || WinningNumber == 28 || WinningNumber == 29 || WinningNumber == 30 || WinningNumber == 31 || WinningNumber == 32 || WinningNumber == 33 || WinningNumber == 34 || WinningNumber == 35 || WinningNumber == 36))
			{
				result = true;
			}
			if (BetPlacement.Equals("2nd 12") && (WinningNumber == 13 || WinningNumber == 14 || WinningNumber == 15 || WinningNumber == 16 || WinningNumber == 17 || WinningNumber == 18 || WinningNumber == 19 || WinningNumber == 20 || WinningNumber == 21 || WinningNumber == 22 || WinningNumber == 23 || WinningNumber == 24))
			{
				result = true;
			}
			if (BetPlacement.Equals("1st 12") && (WinningNumber == 1 || WinningNumber == 2 || WinningNumber == 3 || WinningNumber == 4 || WinningNumber == 5 || WinningNumber == 6 || WinningNumber == 7 || WinningNumber == 8 || WinningNumber == 9 || WinningNumber == 10 || WinningNumber == 11 || WinningNumber == 12))
			{
				result = true;
			}
			if (BetPlacement.Equals("1 to 18") && (WinningNumber == 1 || WinningNumber == 2 || WinningNumber == 3 || WinningNumber == 4 || WinningNumber == 5 || WinningNumber == 6 || WinningNumber == 7 || WinningNumber == 8 || WinningNumber == 9 || WinningNumber == 10 || WinningNumber == 11 || WinningNumber == 12 || WinningNumber == 13 || WinningNumber == 14 || WinningNumber == 15 || WinningNumber == 16 || WinningNumber == 17 || WinningNumber == 18))
			{
				result = true;
			}
			if (BetPlacement.Equals("Even") && (WinningNumber == 2 || WinningNumber == 4 || WinningNumber == 6 || WinningNumber == 8 || WinningNumber == 10 || WinningNumber == 12 || WinningNumber == 14 || WinningNumber == 16 || WinningNumber == 18 || WinningNumber == 20 || WinningNumber == 22 || WinningNumber == 24 || WinningNumber == 26 || WinningNumber == 28 || WinningNumber == 30 || WinningNumber == 32 || WinningNumber == 34 || WinningNumber == 36))
			{
				result = true;
			}
			if (BetPlacement.Equals("Red") && (WinningNumber == 1 || WinningNumber == 3 || WinningNumber == 5 || WinningNumber == 7 || WinningNumber == 9 || WinningNumber == 12 || WinningNumber == 14 || WinningNumber == 16 || WinningNumber == 18 || WinningNumber == 19 || WinningNumber == 21 || WinningNumber == 23 || WinningNumber == 25 || WinningNumber == 27 || WinningNumber == 30 || WinningNumber == 32 || WinningNumber == 34 || WinningNumber == 36))
			{
				result = true;
			}
			if (BetPlacement.Equals("Black") && (WinningNumber == 2 || WinningNumber == 4 || WinningNumber == 6 || WinningNumber == 8 || WinningNumber == 10 || WinningNumber == 11 || WinningNumber == 13 || WinningNumber == 15 || WinningNumber == 17 || WinningNumber == 20 || WinningNumber == 22 || WinningNumber == 24 || WinningNumber == 26 || WinningNumber == 28 || WinningNumber == 29 || WinningNumber == 31 || WinningNumber == 33 || WinningNumber == 35))
			{
				result = true;
			}
			if (BetPlacement.Equals("Odd") && (WinningNumber == 1 || WinningNumber == 3 || WinningNumber == 5 || WinningNumber == 7 || WinningNumber == 8 || WinningNumber == 11 || WinningNumber == 13 || WinningNumber == 15 || WinningNumber == 17 || WinningNumber == 19 || WinningNumber == 21 || WinningNumber == 23 || WinningNumber == 25 || WinningNumber == 27 || WinningNumber == 29 || WinningNumber == 31 || WinningNumber == 33 || WinningNumber == 35))
			{
				result = true;
			}
			if (BetPlacement.Equals("19 to 36") && (WinningNumber == 19 || WinningNumber == 20 || WinningNumber == 21 || WinningNumber == 22 || WinningNumber == 23 || WinningNumber == 24 || WinningNumber == 25 || WinningNumber == 26 || WinningNumber == 27 || WinningNumber == 28 || WinningNumber == 29 || WinningNumber == 30 || WinningNumber == 31 || WinningNumber == 32 || WinningNumber == 33 || WinningNumber == 34 || WinningNumber == 35 || WinningNumber == 36))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 1, 2, 4, 5") && (WinningNumber == 1 || WinningNumber == 2 || WinningNumber == 4 || WinningNumber == 5))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 2, 3, 5, 6") && (WinningNumber == 2 || WinningNumber == 3 || WinningNumber == 5 || WinningNumber == 6))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 4, 5, 7, 8") && (WinningNumber == 4 || WinningNumber == 5 || WinningNumber == 7 || WinningNumber == 8))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 5, 6, 8, 9") && (WinningNumber == 5 || WinningNumber == 6 || WinningNumber == 8 || WinningNumber == 9))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 7, 8, 10, 11") && (WinningNumber == 7 || WinningNumber == 8 || WinningNumber == 10 || WinningNumber == 11))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 8, 9, 11, 12") && (WinningNumber == 8 || WinningNumber == 9 || WinningNumber == 11 || WinningNumber == 12))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 10, 11, 13, 14") && (WinningNumber == 10 || WinningNumber == 11 || WinningNumber == 13 || WinningNumber == 14))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 11, 12, 14, 15") && (WinningNumber == 11 || WinningNumber == 12 || WinningNumber == 14 || WinningNumber == 15))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 13, 14, 16, 17") && (WinningNumber == 13 || WinningNumber == 14 || WinningNumber == 16 || WinningNumber == 17))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 14, 15, 17, 18") && (WinningNumber == 14 || WinningNumber == 15 || WinningNumber == 17 || WinningNumber == 18))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 16, 17, 19, 20") && (WinningNumber == 16 || WinningNumber == 17 || WinningNumber == 19 || WinningNumber == 20))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 17, 18, 20, 21") && (WinningNumber == 17 || WinningNumber == 18 || WinningNumber == 20 || WinningNumber == 21))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 19, 20, 22, 23") && (WinningNumber == 19 || WinningNumber == 20 || WinningNumber == 22 || WinningNumber == 23))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 20, 21, 23, 24") && (WinningNumber == 20 || WinningNumber == 21 || WinningNumber == 23 || WinningNumber == 24))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 22, 23, 25, 26") && (WinningNumber == 22 || WinningNumber == 23 || WinningNumber == 25 || WinningNumber == 26))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 23, 24, 26, 27") && (WinningNumber == 23 || WinningNumber == 24 || WinningNumber == 26 || WinningNumber == 27))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 25, 26, 28, 29") && (WinningNumber == 25 || WinningNumber == 26 || WinningNumber == 28 || WinningNumber == 29))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 26, 27, 29, 30") && (WinningNumber == 26 || WinningNumber == 27 || WinningNumber == 29 || WinningNumber == 30))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 28, 29, 31, 32") && (WinningNumber == 28 || WinningNumber == 29 || WinningNumber == 31 || WinningNumber == 32))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 29, 30, 32, 33") && (WinningNumber == 29 || WinningNumber == 30 || WinningNumber == 32 || WinningNumber == 33))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 31, 32, 34, 35") && (WinningNumber == 31 || WinningNumber == 32 || WinningNumber == 34 || WinningNumber == 35))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 32, 33, 35, 36") && (WinningNumber == 32 || WinningNumber == 33 || WinningNumber == 35 || WinningNumber == 36))
			{
				result = true;
			}
			if (BetPlacement.Equals("Row#1 1, 2, 3") && (WinningNumber == 1 || WinningNumber == 2 || WinningNumber == 3))
			{
				result = true;
			}
			if (BetPlacement.Equals("Segment#1 1, 2, 3, 4, 5, 6") && (WinningNumber == 1 || WinningNumber == 2 || WinningNumber == 3 || WinningNumber == 4 || WinningNumber == 5 || WinningNumber == 6))
			{
				result = true;
			}
			if (BetPlacement.Equals("Row#2 4, 5, 6") && (WinningNumber == 4 || WinningNumber == 5 || WinningNumber == 6))
			{
				result = true;
			}
			if (BetPlacement.Equals("Segment#2 4, 5, 6, 7, 8, 9") && (WinningNumber == 4 || WinningNumber == 5 || WinningNumber == 6 || WinningNumber == 7 || WinningNumber == 8 || WinningNumber == 9))
			{
				result = true;
			}
			if (BetPlacement.Equals("Row#3 7, 8, 9") && (WinningNumber == 7 || WinningNumber == 8 || WinningNumber == 9))
			{
				result = true;
			}
			if (BetPlacement.Equals("Segment#3 7, 8, 9, 10, 11, 12") && (WinningNumber == 7 || WinningNumber == 8 || WinningNumber == 9 || WinningNumber == 10 || WinningNumber == 11 || WinningNumber == 12))
			{
				result = true;
			}
			if (BetPlacement.Equals("Row#4 10, 11, 12") && (WinningNumber == 10 || WinningNumber == 11 || WinningNumber == 12))
			{
				result = true;
			}
			if (BetPlacement.Equals("Segment#4 10, 11, 12, 13, 14, 15") && (WinningNumber == 10 || WinningNumber == 11 || WinningNumber == 12 || WinningNumber == 13 || WinningNumber == 14 || WinningNumber == 15))
			{
				result = true;
			}
			if (BetPlacement.Equals("Row#5 13, 14, 15") && (WinningNumber == 13 || WinningNumber == 14 || WinningNumber == 15))
			{
				result = true;
			}
			if (BetPlacement.Equals("Segment#5 13, 14, 15, 16, 17, 18") && (WinningNumber == 13 || WinningNumber == 14 || WinningNumber == 15 || WinningNumber == 16 || WinningNumber == 17 || WinningNumber == 18))
			{
				result = true;
			}
			if (BetPlacement.Equals("Row#6 16, 17, 18") && (WinningNumber == 16 || WinningNumber == 17 || WinningNumber == 18))
			{
				result = true;
			}
			if (BetPlacement.Equals("Segment#6 16, 17, 18, 19, 20, 21") && (WinningNumber == 16 || WinningNumber == 17 || WinningNumber == 18 || WinningNumber == 19 || WinningNumber == 20 || WinningNumber == 21))
			{
				result = true;
			}
			if (BetPlacement.Equals("Row#7 19, 20, 21") && (WinningNumber == 19 || WinningNumber == 20 || WinningNumber == 21))
			{
				result = true;
			}
			if (BetPlacement.Equals("Segment#7 19, 20, 21, 22, 23, 24") && (WinningNumber == 19 || WinningNumber == 20 || WinningNumber == 21 || WinningNumber == 22 || WinningNumber == 23 || WinningNumber == 24))
			{
				result = true;
			}
			if (BetPlacement.Equals("Row#8 22, 23, 24") && (WinningNumber == 22 || WinningNumber == 23 || WinningNumber == 24))
			{
				result = true;
			}
			if (BetPlacement.Equals("Segment#8 22, 23, 24, 25, 26, 27") && (WinningNumber == 22 || WinningNumber == 23 || WinningNumber == 24 || WinningNumber == 25 || WinningNumber == 26 || WinningNumber == 27))
			{
				result = true;
			}
			if (BetPlacement.Equals("Row#9 25, 26, 27") && (WinningNumber == 25 || WinningNumber == 26 || WinningNumber == 27))
			{
				result = true;
			}
			if (BetPlacement.Equals("Segment#9 25, 26, 27, 28, 29, 30") && (WinningNumber == 25 || WinningNumber == 26 || WinningNumber == 27 || WinningNumber == 28 || WinningNumber == 29 || WinningNumber == 30))
			{
				result = true;
			}
			if (BetPlacement.Equals("Row#10 28, 29, 30") && (WinningNumber == 28 || WinningNumber == 29 || WinningNumber == 30))
			{
				result = true;
			}
			if (BetPlacement.Equals("Segment#10 28, 29, 30, 31, 32, 33") && (WinningNumber == 28 || WinningNumber == 29 || WinningNumber == 30 || WinningNumber == 31 || WinningNumber == 32 || WinningNumber == 33))
			{
				result = true;
			}
			if (BetPlacement.Equals("Row#11 31, 32, 33") && (WinningNumber == 31 || WinningNumber == 32 || WinningNumber == 33))
			{
				result = true;
			}
			if (BetPlacement.Equals("Segment#11 31, 32, 33, 34, 35, 36") && (WinningNumber == 31 || WinningNumber == 32 || WinningNumber == 33 || WinningNumber == 34 || WinningNumber == 35 || WinningNumber == 36))
			{
				result = true;
			}
			if (BetPlacement.Equals("Row#12 34, 35, 36") && (WinningNumber == 34 || WinningNumber == 35 || WinningNumber == 36))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 1, 2, 3, 0, 00") && (WinningNumber == 1 || WinningNumber == 2 || WinningNumber == 3 || WinningNumber == 37 || WinningNumber == 38))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 1 & 2") && (WinningNumber == 1 || WinningNumber == 2))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 2 & 3") && (WinningNumber == 2 || WinningNumber == 3))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 4 & 5") && (WinningNumber == 4 || WinningNumber == 5))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 5 & 6") && (WinningNumber == 5 || WinningNumber == 6))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 7 & 8") && (WinningNumber == 7 || WinningNumber == 8))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 8 & 9") && (WinningNumber == 8 || WinningNumber == 9))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 10 & 11") && (WinningNumber == 10 || WinningNumber == 11))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 11 & 12") && (WinningNumber == 11 || WinningNumber == 12))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 13 & 14") && (WinningNumber == 13 || WinningNumber == 14))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 14 & 15") && (WinningNumber == 14 || WinningNumber == 15))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 16 & 17") && (WinningNumber == 16 || WinningNumber == 17))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 17 & 18") && (WinningNumber == 17 || WinningNumber == 18))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 19 & 20") && (WinningNumber == 19 || WinningNumber == 20))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 20 & 21") && (WinningNumber == 20 || WinningNumber == 21))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 22 & 23") && (WinningNumber == 22 || WinningNumber == 23))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 23 & 24") && (WinningNumber == 23 || WinningNumber == 24))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 25 & 26") && (WinningNumber == 25 || WinningNumber == 26))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 26 & 27") && (WinningNumber == 26 || WinningNumber == 27))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 28 & 29") && (WinningNumber == 28 || WinningNumber == 29))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 29 & 30") && (WinningNumber == 29 || WinningNumber == 30))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 31 & 32") && (WinningNumber == 31 || WinningNumber == 32))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 32 & 33") && (WinningNumber == 32 || WinningNumber == 33))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 34 & 35") && (WinningNumber == 34 || WinningNumber == 35))
			{
				result = true;
			}
			if (BetPlacement.Equals("Numbers 35 & 36") && (WinningNumber == 35 || WinningNumber == 36))
			{
				result = true;
			}
			return result;
		}

		public static void CleanUp_RouletteBetsAndMarkers()
		{
			RouletteBets.Clear();
		}

		public CasinoGames()
		{
			if (_Debug.CasinoGames_Main != -1)
			{
				base.Tick += OnTick;
			}
		}

		public static Vector3 GetClosestGame(Vector3 XE, string model)
		{
			Vector3 result = default(Vector3);
			Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, XE.X, XE.Y, XE.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, model), 0, 0, 0);
			if (prop != null && prop.Exists())
			{
				return prop.Position;
			}
			return result;
		}

		public static bool IsAllowedToPlayThisGame(Prop Table, TableType tableType)
		{
			bool flag = true;
			if (!isPlaceableCasinoGames)
			{
				flag = true;
			}
			else
			{
				flag = false;
				if (tableType == TableType.Blackjack && Class1.Tables_Blackjack.Contains(Table))
				{
					flag = true;
				}
				if (tableType == TableType.Poker && Class1.Tables_Poker.Contains(Table))
				{
					flag = true;
				}
				if (tableType == TableType.Roulette && Class1.Tables_Roulette.Contains(Table))
				{
					flag = true;
				}
			}
			return flag;
		}

		public unsafe void OnTick(object sender, EventArgs e)
		{
			CanRunModRightNow = true;
			if (CurrentHelpMessage != null && CurrentHelpMessage != "")
			{
				if (Game.GameTime < HelpMessageShowTime)
				{
					DisplayHelpTextThisFrame(CurrentHelpMessage);
				}
				else
				{
					CurrentHelpMessage = "";
				}
			}
			if (isPlaceableCasinoGames)
			{
				Vector3 position = Game.Player.Character.Position;
				int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position.X, position.Y, position.Z);
				if (num == 275201 || num == 274689)
				{
					CanRunModRightNow = false;
					return;
				}
			}
			else
			{
				Vector3 position2 = Game.Player.Character.Position;
				int num2 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position2.X, position2.Y, position2.Z);
				if (num2 != 275201 && num2 != 274689)
				{
					CanRunModRightNow = false;
					return;
				}
			}
			if (!Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_VINEWOOD/CASINO_GENERAL", false, -1))
			{
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_VINEWOOD/CASINO_GENERAL", false, -1);
			}
			if (!Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_VINEWOOD/CASINO_SLOT_MACHINES_01", false, -1))
			{
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_VINEWOOD/CASINO_SLOT_MACHINES_01", false, -1);
			}
			if (!Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_VINEWOOD/CASINO_SLOT_MACHINES_02", false, -1))
			{
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_VINEWOOD/CASINO_SLOT_MACHINES_02", false, -1);
			}
			if (!Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_VINEWOOD/CASINO_SLOT_MACHINES_03", false, -1))
			{
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_VINEWOOD/CASINO_SLOT_MACHINES_03", false, -1);
			}
			if (global::Gambling.Diamond.Enabled) return;   // RealismSuite plays this game here
			if (!Class1.Initialized)
			{
				return;
			}
			if (Playing_Poker || Playing_BlackJack || Playing_Roulette)
			{
				Game.DisableControlThisFrame(Control.Phone);
			}
			Vector3 position3 = Game.Player.Character.Position;
			Vector3 closestGame = GetClosestGame(position3, "vw_prop_casino_blckjack_01b");
			Vector3 closestGame2 = GetClosestGame(position3, "vw_prop_casino_blckjack_01");
			Vector3 closestGame3 = GetClosestGame(position3, "vw_prop_casino_blckjack_01b");
			Vector3 closestGame4 = GetClosestGame(position3, "vw_prop_casino_blckjack_01b");
			Vector3 closestGame5 = GetClosestGame(position3, "vw_prop_casino_roulette_01b");
			if (GetDistance_VDIST2(Game.Player.Character.Position, closestGame, GetDistSign.Greater, 10f) && GetDistance_VDIST2(Game.Player.Character.Position, closestGame2, GetDistSign.Greater, 10f) && GetDistance_VDIST2(Game.Player.Character.Position, closestGame3, GetDistSign.Greater, 10f) && GetDistance_VDIST2(Game.Player.Character.Position, closestGame4, GetDistSign.Greater, 10f) && GetDistance_VDIST2(Game.Player.Character.Position, closestGame5, GetDistSign.Greater, 10f))
			{
				if (isPlaceableCasinoGames)
				{
					HideChips();
				}
				_Debug.CasinoGames_Main = 0;
				return;
			}
			_Debug.CasinoGames_Main = 1;
			if (Playing_Poker || Playing_BlackJack)
			{
				CardMoveStatus = CardMove.Active;
			}
			else
			{
				CardMoveStatus = CardMove.Disabled;
			}
			CurrentGame = CasinoGamePlaying.None;
			if (CardMoveStatus == CardMove.Active && (Playing_BlackJack || Playing_Poker))
			{
				for (int i = 0; i < Tables.Count; i++)
				{
					if (!(Tables[i] != null) || !GetDistance_VDIST2(Game.Player.Character.Position, Tables[i].Position, GetDistSign.Less, 3f))
					{
						continue;
					}
					Vector3 position4 = default(Vector3);
					Vector3 position5 = default(Vector3);
					Vector3 position6 = default(Vector3);
					Vector3 position7 = default(Vector3);
					Vector3 position8 = default(Vector3);
					Vector3 position9 = default(Vector3);
					Vector3 position10 = default(Vector3);
					Vector3 position11 = default(Vector3);
					if (Playing_Poker)
					{
						position4 = Tables[i].GetOffsetPosition(new Vector3(0.39f, -0.18f, 0.95f));
						position5 = Tables[i].GetOffsetPosition(new Vector3(0.29f, -0.22f, 0.95f));
						position6 = Tables[i].GetOffsetPosition(new Vector3(0.19f, -0.25f, 0.95f));
						position7 = Tables[i].GetOffsetPosition(new Vector3(0.16f, -0.15f, 0.95f));
						Tables[i].GetOffsetPosition(new Vector3(0.24f, -0.11f, 0.95f));
						position8 = Tables[i].GetOffsetPosition(new Vector3(0.35f, -0.08f, 0.95f));
						position9 = Tables[i].GetOffsetPosition(new Vector3(0.15f, 0.13f, 0.95f));
						position10 = Tables[i].GetOffsetPosition(new Vector3(0f, 0.1f, 0.95f));
						position11 = Tables[i].GetOffsetPosition(new Vector3(-0.15f, 0.13f, 0.95f));
					}
					if (Playing_BlackJack)
					{
						Tables[i].GetOffsetPosition(new Vector3(0.234f, -0.1f, 0.95f));
						Tables[i].GetOffsetPosition(new Vector3(0.2436f, -0.0736f, 0.953f));
						Tables[i].GetOffsetPosition(new Vector3(0.2532f, -0.0589f, 0.956f));
						Tables[i].GetOffsetPosition(new Vector3(0.2628f, -0.0384f, 0.959f));
						Tables[i].GetOffsetPosition(new Vector3(0.2724f, -0.0179f, 0.962f));
						position7 = Tables[i].GetOffsetPosition(new Vector3(0.16f, -0.15f, 0.95f));
						Tables[i].GetOffsetPosition(new Vector3(0.24f, -0.11f, 0.95f));
						position8 = Tables[i].GetOffsetPosition(new Vector3(0.35f, -0.08f, 0.95f));
						position9 = Tables[i].GetOffsetPosition(new Vector3(0.15f, 0.13f, 0.95f));
						position10 = Tables[i].GetOffsetPosition(new Vector3(0f, 0.1f, 0.95f));
						position11 = Tables[i].GetOffsetPosition(new Vector3(-0.15f, 0.13f, 0.95f));
					}
					if (!Playing_BlackJack)
					{
						if (PlayerCardA != null)
						{
							PlayerCardA.Position = position6;
						}
						if (PlayerCardB != null)
						{
							PlayerCardB.Position = position5;
						}
						if (PlayerCardC != null)
						{
							PlayerCardC.Position = position4;
						}
						if (PlayerCardF != null)
						{
							PlayerCardF.Position = position8;
						}
					}
					if (PlayerCardD != null)
					{
						PlayerCardD.Position = position7;
					}
					if (CardA != null)
					{
						CardA.Position = position9;
						CardA.Rotation = new Vector3(CardA.Rotation.X, CardA.Rotation.Y, Tables[i].Rotation.Z + 20f);
					}
					if (CardB != null)
					{
						CardB.Position = position10;
						CardB.Rotation = new Vector3(CardB.Rotation.X, CardB.Rotation.Y, Tables[i].Rotation.Z);
					}
					if (CardC != null)
					{
						CardC.Position = position11;
						CardC.Rotation = new Vector3(CardC.Rotation.X, CardC.Rotation.Y, Tables[i].Rotation.Z - 20f);
					}
				}
			}
			switch (CurrentGame)
			{
			case CasinoGamePlaying.None:
			{
				if (Playing_Poker)
				{
					CurrentGame = CasinoGamePlaying.Poker;
				}
				if (Playing_Roulette)
				{
					CurrentGame = CasinoGamePlaying.Roulette;
				}
				if (Playing_BlackJack)
				{
					CurrentGame = CasinoGamePlaying.Blackjack;
				}
				if (GetClosesRouletteTable() != null && IsAllowedToPlayThisGame(GetClosesRouletteTable(), TableType.Roulette))
				{
					if (isPlaceableCasinoGames)
					{
						DisplayChips(Chips_Amount);
					}
					Vector3 item = GetClosestSeat(GetClosesRouletteTable()).Item1;
					GetClosestSeat(GetClosesRouletteTable());
					int item2 = GetClosestSeat(GetClosesRouletteTable()).Item3;
					if ((GetDistance_VDIST2(Game.Player.Character.Position, GetClosesRouletteTable().Position, GetDistSign.Less, 2f) || GetDistance_VDIST2(Game.Player.Character.Position, item, GetDistSign.Less, 2f)) && !Playing_Roulette)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to play Roulette");
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, GetClosesRouletteTable().Position, GetDistSign.Less, 2f) || GetDistance_VDIST2(Game.Player.Character.Position, item, GetDistSign.Less, 2f))
					{
						if (!Playing_Roulette && !Placebet)
						{
							if (Game.IsControlJustPressed(Control.Context))
							{
								if (RouletteMarker1 != null)
								{
									RouletteMarker1.Delete();
								}
								RouletteBetIndex = 1;
								SeatSittingIn = item2;
								LookingAtWheel = false;
								if (RoulleteCam != null)
								{
									Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 1, 3000, 1, 0);
									RoulleteCam.IsActive = false;
									RoulleteCam.Delete();
								}
								foreach (Prop betPosMarker in BetPosMarkers)
								{
									if (betPosMarker != null)
									{
										betPosMarker.Delete();
									}
								}
								BetPosMarkers.Clear();
								PlayerOriginPosition = GetPlayerAnimOriginH(SeatSittingIn, TableType.Roulette).Item1;
								PlayerOriginHeading = GetPlayerAnimOriginH(SeatSittingIn, TableType.Roulette).Item2;
								ResetCardDeck();
								int currentTable = 0;
								for (int j = 0; j < RouletteTables.Count; j++)
								{
									if (GetDistance_VDIST2(RouletteTables[j], GetClosesRouletteTable().Position, GetDistSign.Less, 3f))
									{
										currentTable = j;
									}
								}
								CreatedRouletteCam = false;
								if (RoulleteCam != null)
								{
									Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
									RoulleteCam.IsActive = false;
									RoulleteCam.Delete();
								}
								Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, GetClosestDealer(GetClosesRouletteTable().Position, RouletteDealerTables), "MINIGAME_DEALER_GREET", "SPEECH_PARAMS_STANDARD", false);
								foreach (DealerTable rouletteDealerTable in RouletteDealerTables)
								{
									ResetDealerPos(rouletteDealerTable);
								}
								Vector3 vector = GET_PLAYER_CHAIR_POSITION(SeatSittingIn, TableType.Roulette);
								float z = GET_PLAYER_CHAIR_ROTATION(SeatSittingIn, TableType.Roulette).Z;
								int taskID = 0;
								OpenSequenceTask(&taskID);
								TaskPedSlideToCoord(0, vector, z, 8f);
								CloseSequenceTask(taskID);
								TaskPerformSequence(Game.Player.Character, taskID);
								ClearSequenceTask(&taskID);
								Script.Wait(100);
								while (Function.Call<int>(Hash.GET_SEQUENCE_PROGRESS, Game.Player.Character) != -1 && GetDistance_VDIST2(Game.Player.Character.Position, vector, GetDistSign.Greater, 1.25f))
								{
									Script.Wait(0);
								}
								ClosestDealer = GetClosestDealer(Game.Player.Character.Position, RouletteDealerTables);
								ClosestTable = GetClosestTable(Game.Player.Character.Position, RouletteDealerTables);
								ClosestDealerTable = GetClosestDealerTable(Game.Player.Character.Position, RouletteDealerTables);
								GetEnterAnim();
								string dict = "anim_casino_b@amb@casino@games@shared@player@";
								LoadDict("anim_casino_b@amb@casino@games@shared@player@");
								LoadDict(dict);
								Vector3 item3 = GetPlayerAnimOriginH(SeatSittingIn, TableType.Roulette).Item1;
								float item4 = GetPlayerAnimOriginH(SeatSittingIn, TableType.Roulette).Item2;
								RouletteStartScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, item3.X, item3.Y, item3.Z - 1f, 0.0, 0.0, item4, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, RouletteStartScene, LoadDict(dict), "sit_enter_left", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, RouletteStartScene, "sit_enter_left", LoadDict(dict), 16f, -16f, 0, 1148846080);
								Placebet2 = false;
								Editbet = false;
								Placebet = false;
								Drawcards = false;
								TableT = ClosestTable;
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
								CurrentTable = currentTable;
								BetPos = "None";
								BetPosNum = 0;
								MarkerNum = 0;
								foreach (RouletteBet rouletteBet in RouletteBets)
								{
									if (rouletteBet.BetProp != null)
									{
										rouletteBet.BetProp.Delete();
									}
								}
								RouletteBets.Clear();
								MarkerNum = 0;
								foreach (Prop chip in Chips)
								{
									if (chip != null)
									{
										chip.Delete();
									}
								}
								Playing_Roulette = true;
							}
						}
						else if (Playing_Roulette && !Placebet && Game.IsControlJustPressed(Control.Context))
						{
							CurrentHelpMessage = "";
							if (RouletteMarker1 != null)
							{
								RouletteMarker1.Delete();
							}
							RouletteBetIndex = 1;
							LookingAtWheel = false;
							if (RoulleteCam != null)
							{
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 1, 3000, 1, 0);
								RoulleteCam.IsActive = false;
								RoulleteCam.Delete();
							}
							foreach (Prop betPosMarker2 in BetPosMarkers)
							{
								if (betPosMarker2 != null)
								{
									betPosMarker2.Delete();
								}
							}
							BetPosMarkers.Clear();
							Playing_Roulette = false;
							ResetCardDeck();
							int num3 = 0;
							for (int k = 0; k < RouletteTables.Count; k++)
							{
								if (GetDistance_VDIST2(RouletteTables[k], GetClosesRouletteTable().Position, GetDistSign.Less, 3f))
								{
									num3 = k;
								}
							}
							CreatedRouletteCam = false;
							if (RoulleteCam != null)
							{
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
								RoulleteCam.IsActive = false;
								RoulleteCam.Delete();
							}
							Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, GetClosestDealer(Game.Player.Character.Position, RouletteDealerTables), "MINIGAME_DEALER_LEAVE_NEUTRAL_GAME", "SPEECH_PARAMS_STANDARD", false);
							foreach (DealerTable rouletteDealerTable2 in RouletteDealerTables)
							{
								ResetDealerPos(rouletteDealerTable2);
							}
							ClosestDealer = GetClosestDealer(Game.Player.Character.Position, RouletteDealerTables);
							ClosestTable = GetClosestTable(Game.Player.Character.Position, RouletteDealerTables);
							ClosestDealerTable = GetClosestDealerTable(Game.Player.Character.Position, RouletteDealerTables);
							TableT = null;
							CurrentTable = num3;
							string dict2 = "anim_casino_b@amb@casino@games@shared@player@";
							LoadDict(dict2);
							Vector3 item5 = GetPlayerAnimOriginH(SeatSittingIn, TableType.Roulette).Item1;
							float item6 = GetPlayerAnimOriginH(SeatSittingIn, TableType.Roulette).Item2;
							Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, item5.X, item5.Y, item5.Z - 1f, 0.0, 0.0, item6, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict2), "sit_exit_left", 16f, -16f, 0, 1148846080);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "sit_exit_left", LoadDict(dict2), 16f, -16f, 0, 1148846080);
							Script.Wait(4000);
							Game.Player.Character.Task.ClearAll();
							Game.Player.Character.IsPositionFrozen = false;
							CreatedMarker = false;
							Game.Player.Character.Heading = RouletteTablesRot[num3];
							PlaceBet_R = false;
							foreach (Prop chip2 in Chips)
							{
								if (chip2 != null)
								{
									chip2.Delete();
								}
							}
							CleanUp_RouletteBetsAndMarkers();
							BetPos = "None";
							MarkerNum = 0;
							BetPosNum = 0;
							foreach (RouletteBet rouletteBet2 in RouletteBets)
							{
								if (rouletteBet2.BetProp != null)
								{
									rouletteBet2.BetProp.Delete();
								}
							}
							RouletteBets.Clear();
							foreach (Prop chip3 in Chips)
							{
								if (chip3 != null)
								{
									chip3.Delete();
								}
							}
							Editbet = false;
							Placebet = false;
							Placebet2 = false;
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
				}
				if (GetClosesPokerTable() != null && IsAllowedToPlayThisGame(GetClosesPokerTable(), TableType.Poker))
				{
					if (isPlaceableCasinoGames)
					{
						DisplayChips(Chips_Amount);
					}
					Vector3 item7 = GetClosestSeat(GetClosesPokerTable()).Item1;
					GetClosestSeat(GetClosesPokerTable());
					int item8 = GetClosestSeat(GetClosesPokerTable()).Item3;
					if (GetDistance_VDIST2(Game.Player.Character.Position, GetClosesPokerTable().Position, GetDistSign.Less, 2f) || GetDistance_VDIST2(Game.Player.Character.Position, item7, GetDistSign.Less, 2f))
					{
						Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
						Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
						Function.Call(Hash.DISABLE_FRONTEND_THIS_FRAME);
						Function.Call(Hash.SUPPRESS_FRONTEND_RENDERING_THIS_FRAME);
						Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
						if (!Playing_Poker)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to play Poker ");
						}
						if (Playing_Poker && !Placebet && !Endgame)
						{
							ShowHelpMessage("Bets can be placed on the Ante, Pair Plus, or both. Play and Ante bets pay out 1 to 1 if your hand beats the Dealer's hand. A Straight or higher is needed to receive the Ante Bonus.");
							if (BetType == 2)
							{
								BetType = 0;
							}
							if (BetType == 0)
							{
								drawSprite2("timerbars", "all_black_bg", 0.89f, 0.85f, 0.21f, 0.04f);
								drawText("PAIR PLUS BET :", 0.88f, 0.841f, 0.3f, 255, 255, 255);
								drawNumber(Bet_PairPlus, 0.950000048f, 0.832f, 0.48f, 0, 255, 0);
								drawSprite2("timerbars", "all_black_bg", 0.89f, 0.890000045f, 0.21f, 0.04f);
								drawText("ANTE BET :", 0.88f, 0.881000042f, 0.3f, 255, 255, 255);
								drawNumber(Bet_Ante, 0.950000048f, 0.872000039f, 0.48f, 255, 255, 255);
							}
							if (BetType == 1)
							{
								drawSprite2("timerbars", "all_black_bg", 0.89f, 0.85f, 0.21f, 0.04f);
								drawText("PAIR PLUS BET :", 0.88f, 0.841f, 0.3f, 255, 255, 255);
								drawNumber(Bet_PairPlus, 0.950000048f, 0.832f, 0.48f, 255, 255, 255);
								drawSprite2("timerbars", "all_black_bg", 0.89f, 0.890000045f, 0.21f, 0.04f);
								drawText("ANTE BET :", 0.88f, 0.881000042f, 0.3f, 255, 255, 255);
								drawNumber(Bet_Ante, 0.950000048f, 0.872000039f, 0.48f, 0, 255, 0);
							}
							Scaleform scaleform = new Scaleform("instructional_buttons");
							scaleform.CallFunction("CLEAR_ALL");
							scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
							scaleform.CallFunction("CREATE_CONTAINER");
							scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Exit");
							scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 44, 0), "Confirm Ante & Pair Plus Bets");
							scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 172, 0), "Change Bet Type");
							scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 173, 0), "");
							string text = "Pair Plus";
							if (BetType == 0)
							{
								text = "Pair Plus";
							}
							if (BetType == 1)
							{
								text = "Ante";
							}
							scaleform.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 175, 0), "-/+ '" + text + "' Bet");
							scaleform.CallFunction("SET_DATA_SLOT", 5, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 174, 0), "");
							scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
							scaleform.Render2D();
							if (Game.IsControlJustPressed(Control.PhoneUp))
							{
								if (BetType == 0)
								{
									BetType = 1;
								}
								else if (BetType == 1)
								{
									BetType = 0;
								}
							}
							if (Game.IsControlJustPressed(Control.PhoneDown))
							{
								if (BetType == 1)
								{
									BetType = 0;
								}
								else if (BetType == 0)
								{
									BetType = 1;
								}
							}
							if (Game.GameTime >= TimeBetweenBets)
							{
								if (Game.IsControlPressed(Control.PhoneLeft))
								{
									if (BetType == 0)
									{
										Bet_PairPlus_Index--;
										if (Bet_PairPlus_Index <= -1)
										{
											Bet_PairPlus_Index = -1;
											Bet_PairPlus = 0;
										}
										else
										{
											Bet_PairPlus = GetBetFromIndex(Bet_PairPlus_Index);
											TimeBetweenBets = Game.GameTime + Set_TimeBetweenBets;
										}
									}
									if (BetType == 1)
									{
										if (Bet_Ante_Index > 0)
										{
											Bet_Ante_Index--;
										}
										Bet_Ante = GetBetFromIndex(Bet_Ante_Index);
										TimeBetweenBets = Game.GameTime + Set_TimeBetweenBets;
									}
								}
								if (Game.IsControlPressed(Control.PhoneRight))
								{
									if (BetType == 0)
									{
										Bet_PairPlus_Index++;
										Bet_PairPlus = GetBetFromIndex(Bet_PairPlus_Index);
										TimeBetweenBets = Game.GameTime + Set_TimeBetweenBets;
									}
									if (BetType == 1)
									{
										Bet_Ante_Index++;
										Bet_Ante = GetBetFromIndex(Bet_Ante_Index);
										TimeBetweenBets = Game.GameTime + Set_TimeBetweenBets;
									}
								}
							}
						}
						if (Placebet && !Endgame && Playercards == 5)
						{
							Show_G("Accept Hand");
						}
						if (Drawcards && !Endgame)
						{
							if (BetType != 2)
							{
								BetType = 2;
							}
							Scaleform scaleform2 = new Scaleform("instructional_buttons");
							scaleform2.CallFunction("CLEAR_ALL");
							scaleform2.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
							scaleform2.CallFunction("CREATE_CONTAINER");
							scaleform2.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Fold");
							scaleform2.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 44, 0), "Play Hand");
							scaleform2.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 58, 0), "Look At Hand");
							scaleform2.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 175, 0), "-/+  'Play' Bet");
							scaleform2.CallFunction("SET_DATA_SLOT", 5, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 174, 0), "");
							scaleform2.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
							scaleform2.Render2D();
							drawSprite2("timerbars", "all_black_bg", 0.89f, 0.890000045f, 0.21f, 0.04f);
							drawText("ANTE BET :", 0.88f, 0.881000042f, 0.3f, 255, 255, 255);
							drawNumber(Bet_Ante, 0.950000048f, 0.872000039f, 0.48f, 255, 255, 255);
							if (Bet_Play > 0)
							{
								drawSprite2("timerbars", "all_black_bg", 0.89f, 0.85f, 0.21f, 0.04f);
								drawText("PLAY BET :", 0.88f, 0.841f, 0.3f, 255, 255, 255);
								drawNumber(Bet_Play, 0.950000048f, 0.832f, 0.48f, 0, 255, 0);
								if (Bet_PairPlus > 0)
								{
									drawSprite2("timerbars", "all_black_bg", 0.89f, 0.810000062f, 0.21f, 0.04f);
									drawText("PAIR PLUS BET :", 0.88f, 0.801f, 0.3f, 255, 255, 255);
									drawNumber(Bet_PairPlus, 0.950000048f, 0.792000055f, 0.48f, 255, 255, 255);
								}
							}
							else if (Bet_PairPlus > 0)
							{
								drawSprite2("timerbars", "all_black_bg", 0.89f, 0.85f, 0.21f, 0.04f);
								drawText("PAIR PLUS BET :", 0.88f, 0.841f, 0.3f, 255, 255, 255);
								drawNumber(Bet_PairPlus, 0.950000048f, 0.832f, 0.48f, 255, 255, 255);
							}
							if (Bet_Play_Index > 0 && Bet_Play < Bet_Ante)
							{
								Bet_Play_Index++;
								Bet_Play = GetBetFromIndex(Bet_Play_Index);
							}
							if (TimeBetweenBets < Game.GameTime)
							{
								if (Game.IsControlPressed(Control.PhoneLeft) && BetType == 2)
								{
									int num5 = (int)((float)Bet_Play / 10000f);
									int num6 = 0;
									if (Bet_Play_Index != 0 && Bet_Play_Index < Bet_Ante_Index)
									{
										Bet_Play_Index = 0;
									}
									if (Bet_Play > 0)
									{
										Bet_Play_Index--;
										if (Bet_Play_Index < Bet_Ante_Index)
										{
											Bet_Play_Index = 0;
										}
										if (Bet_Play_Index <= -1)
										{
											Bet_Play_Index = -1;
										}
										if (Bet_Play_Index == -1)
										{
											Bet_Play = 0;
										}
										else
										{
											Bet_Play = GetBetFromIndex(Bet_Play_Index);
											if (Bet_Play != 0)
											{
												if (PokerPlayerPlayBet != null)
												{
													Prop v = GlobalHelper.CreateProp(PokerPlayerPlayBet.Model, PokerPlayerPlayBet.Position, PokerPlayerPlayBet.Rotation, false, false);
													EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(v));
													PokerPlayerPlayBet.Delete();
												}
												Prop prop = GlobalHelper.CreateProp(RequestModel(GetBetChipModel(Bet_Play, true)), ReturnChipPosition(PokerDealerTables, BetTypeId.PlayBet), true, false);
												prop.IsPositionFrozen = true;
												PokerPlayerPlayBet = prop;
												prop.Position = GetClosestTable(Game.Player.Character.Position, PokerDealerTables).GetOffsetPosition(THREE_CARD_POKER_GET_PLAYER_BET_POSITION_OFFSET(ReturnClosestBetAmtFromCustomBet(Bet_Play), BetTypeID.Play, SeatSittingIn - 1, false, true));
												prop.Rotation = GetClosestTable(Game.Player.Character.Position, PokerDealerTables).Rotation + THREE_CARD_POKER_GET_PLAYER_BET_ROTATION_OFFSET(ReturnClosestBetAmtFromCustomBet(Bet_Play), BetTypeID.Play, SeatSittingIn - 1, false);
												Chips.Add(prop);
												EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(prop, true, 255));
											}
										}
										TimeBetweenBets = Game.GameTime + Set_TimeBetweenBets;
									}
									if (Bet_Play != 0 && Bet_Play <= Bet_Ante)
									{
										Bet_Play = Bet_Ante;
									}
								}
								if (Game.IsControlPressed(Control.PhoneRight) && BetType == 2)
								{
									Bet_Play_Index++;
									if (Bet_Play_Index != 0)
									{
										while (Bet_Play_Index < Bet_Ante_Index)
										{
											Script.Wait(1);
											Bet_Play_Index = Bet_Ante_Index;
										}
									}
									Bet_Play = GetBetFromIndex(Bet_Play_Index);
									if (Bet_Play_Index == 1)
									{
										string dict3 = "anim_casino_b@amb@casino@games@blackjack@dealer";
										string text2 = "place_bet_request";
										Vector3 dealerAnimOrigin = GetDealerAnimOrigin();
										Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin.X, dealerAnimOrigin.Y, dealerAnimOrigin.Z, 0.0, 0.0, ClosestDealer.Rotation.Z - 180f, 2);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, ClosestDealer, Scene1, LoadDict(dict3), text2, 16f, -16f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ClosestDealer, Scene1, text2, LoadDict(dict3), 16f, -16f, 0, 1148846080);
										dict3 = "anim_casino_b@amb@casino@games@threecardpoker@player";
										text2 = "bet_ante";
										LoadDict(dict3);
										GetPlayerAnimOriginH(3, TableType.Blackjack);
										GetPlayerAnimOriginH(3, TableType.Blackjack);
										Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, PlayerOriginPosition.X, PlayerOriginPosition.Y, PlayerOriginPosition.Z - 1f, 0.0, 0.0, PlayerOriginHeading, 2);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene2, LoadDict(dict3), text2, 16f, -16f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene2, text2, LoadDict(dict3), 16f, -16f, 0, 1148846080);
										Stage = 20f;
									}
									if (Bet_Play != 0)
									{
										if (PokerPlayerPlayBet != null)
										{
											Prop v2 = GlobalHelper.CreateProp(PokerPlayerPlayBet.Model, PokerPlayerPlayBet.Position, PokerPlayerPlayBet.Rotation, false, false);
											EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(v2));
											PokerPlayerPlayBet.Delete();
										}
										Prop prop2 = GlobalHelper.CreateProp(RequestModel(GetBetChipModel(Bet_Play, true)), ReturnChipPosition(PokerDealerTables, BetTypeId.PlayBet), true, false);
										prop2.IsPositionFrozen = true;
										PokerPlayerPlayBet = prop2;
										prop2.Position = GetClosestTable(Game.Player.Character.Position, PokerDealerTables).GetOffsetPosition(THREE_CARD_POKER_GET_PLAYER_BET_POSITION_OFFSET(ReturnClosestBetAmtFromCustomBet(Bet_Play), BetTypeID.Play, SeatSittingIn - 1, false, true));
										prop2.Rotation = GetClosestTable(Game.Player.Character.Position, PokerDealerTables).Rotation + THREE_CARD_POKER_GET_PLAYER_BET_ROTATION_OFFSET(ReturnClosestBetAmtFromCustomBet(Bet_Play), BetTypeID.Play, SeatSittingIn - 1, false);
										Chips.Add(prop2);
										EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(prop2, true, 255));
									}
									TimeBetweenBets = Game.GameTime + Set_TimeBetweenBets;
									if (Bet_Play != 0 && Bet_Play <= Bet_Ante)
									{
										Bet_Play = Bet_Ante;
									}
								}
							}
						}
						if (Endgame)
						{
							Scaleform scaleform3 = new Scaleform("instructional_buttons");
							scaleform3.CallFunction("CLEAR_ALL");
							scaleform3.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
							scaleform3.CallFunction("CREATE_CONTAINER");
							scaleform3.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Quit Poker");
							scaleform3.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 44, 0), "Play Again");
							if (!LookingAtDealerCards)
							{
								scaleform3.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 75, 0), "View Dealer Cards");
							}
							else if (LookingAtDealerCards)
							{
								scaleform3.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 75, 0), "Stop Looking at Dealer Cards");
							}
							scaleform3.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
							scaleform3.Render2D();
						}
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, GetClosesPokerTable().Position, GetDistSign.Less, 2f) || GetDistance_VDIST2(Game.Player.Character.Position, item7, GetDistSign.Less, 2f))
					{
						if (!Playing_Poker && !Placebet)
						{
							if (Game.IsControlJustPressed(Control.Context))
							{
								LookingAtDealerCards = false;
								if (RoulleteCam != null)
								{
									Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 1, 3000, 1, 0);
									RoulleteCam.IsActive = false;
									RoulleteCam.Delete();
								}
								SeatSittingIn = item8;
								DeleteCardDeck();
								Bet_Play_Index = 0;
								ClosestDealer = GetClosestDealer(Game.Player.Character.Position, PokerDealerTables);
								ClosestTable = GetClosestTable(Game.Player.Character.Position, PokerDealerTables);
								ClosestDealerTable = GetClosestDealerTable(Game.Player.Character.Position, PokerDealerTables);
								PlayerOriginPosition = GetPlayerAnimOriginH(SeatSittingIn, TableType.Poker).Item1;
								PlayerOriginHeading = GetPlayerAnimOriginH(SeatSittingIn, TableType.Poker).Item2;
								BlackJackSplit = false;
								ResetCardDeck();
								int currentTable2 = 0;
								for (int l = 0; l < PokerTables.Count; l++)
								{
									if (GetDistance_VDIST2(PokerTables[l], GetClosesPokerTable().Position, GetDistSign.Less, 3f))
									{
										currentTable2 = l;
									}
								}
								Bet_PairPlus = 0;
								Bet_Ante = 0;
								Bet_Play = 0;
								BetType = 0;
								Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, GetClosestDealer(GetClosesPokerTable().Position, PokerDealerTables), "MINIGAME_DEALER_GREET", "SPEECH_PARAMS_STANDARD", false);
								Stage = -2f;
								Playercards = 0;
								foreach (DealerTable pokerDealerTable in PokerDealerTables)
								{
									if (pokerDealerTable.Dealer != null)
									{
										ResetDealerPos(pokerDealerTable);
									}
									if (pokerDealerTable.Card1 != null)
									{
										pokerDealerTable.Card1.Delete();
									}
									if (pokerDealerTable.Card2 != null)
									{
										pokerDealerTable.Card2.Delete();
									}
									if (pokerDealerTable.Card3 != null)
									{
										pokerDealerTable.Card3.Delete();
									}
									if (pokerDealerTable.Card4 != null)
									{
										pokerDealerTable.Card4.Delete();
									}
									if (pokerDealerTable.Card5 != null)
									{
										pokerDealerTable.Card5.Delete();
									}
									if (pokerDealerTable.Card6 != null)
									{
										pokerDealerTable.Card6.Delete();
									}
								}
								foreach (DealerTable blackjackDealerTable in BlackjackDealerTables)
								{
									if (blackjackDealerTable.Dealer != null)
									{
										ResetDealerPos(blackjackDealerTable);
									}
									if (blackjackDealerTable.Card1 != null)
									{
										blackjackDealerTable.Card1.Delete();
									}
									if (blackjackDealerTable.Card2 != null)
									{
										blackjackDealerTable.Card2.Delete();
									}
									if (blackjackDealerTable.Card3 != null)
									{
										blackjackDealerTable.Card3.Delete();
									}
									if (blackjackDealerTable.Card4 != null)
									{
										blackjackDealerTable.Card4.Delete();
									}
									if (blackjackDealerTable.Card5 != null)
									{
										blackjackDealerTable.Card5.Delete();
									}
									if (blackjackDealerTable.Card6 != null)
									{
										blackjackDealerTable.Card6.Delete();
									}
								}
								foreach (DealerTable rouletteDealerTable3 in RouletteDealerTables)
								{
									if (rouletteDealerTable3.Dealer != null)
									{
										ResetDealerPos(rouletteDealerTable3);
									}
									if (rouletteDealerTable3.Card1 != null)
									{
										rouletteDealerTable3.Card1.Delete();
									}
									if (rouletteDealerTable3.Card2 != null)
									{
										rouletteDealerTable3.Card2.Delete();
									}
									if (rouletteDealerTable3.Card3 != null)
									{
										rouletteDealerTable3.Card3.Delete();
									}
									if (rouletteDealerTable3.Card4 != null)
									{
										rouletteDealerTable3.Card4.Delete();
									}
									if (rouletteDealerTable3.Card5 != null)
									{
										rouletteDealerTable3.Card5.Delete();
									}
									if (rouletteDealerTable3.Card6 != null)
									{
										rouletteDealerTable3.Card6.Delete();
									}
								}
								ClosestDealerTable = GetClosestDealerTable(Game.Player.Character.Position, PokerDealerTables);
								GetEnterAnim();
								string dict4 = "anim_casino_b@amb@casino@games@shared@player@";
								LoadDict("anim_casino_b@amb@casino@games@shared@player@");
								LoadDict(dict4);
								Vector3 vector2 = GET_PLAYER_CHAIR_POSITION(SeatSittingIn, TableType.Poker);
								float z2 = GET_PLAYER_CHAIR_ROTATION(SeatSittingIn, TableType.Poker).Z;
								int taskID2 = 0;
								OpenSequenceTask(&taskID2);
								TaskPedSlideToCoord(0, vector2, z2, 8f);
								CloseSequenceTask(taskID2);
								TaskPerformSequence(Game.Player.Character, taskID2);
								ClearSequenceTask(&taskID2);
								Script.Wait(100);
								while (Function.Call<int>(Hash.GET_SEQUENCE_PROGRESS, Game.Player.Character) != -1 && GetDistance_VDIST2(Game.Player.Character.Position, vector2, GetDistSign.Greater, 1.25f))
								{
									Script.Wait(0);
								}
								ClosestTable = GetClosestTable(Game.Player.Character.Position, PokerDealerTables);
								Vector3 item9 = GetPlayerAnimOriginH(SeatSittingIn, TableType.Poker).Item1;
								float item10 = GetPlayerAnimOriginH(SeatSittingIn, TableType.Poker).Item2;
								Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, item9.X, item9.Y, item9.Z - 1f, 0.0, 0.0, item10, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict4), "sit_enter_left", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "sit_enter_left", LoadDict(dict4), 16f, -16f, 0, 1148846080);
								Placebet2 = false;
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
								CurrentTable = currentTable2;
								foreach (Prop chip4 in Chips)
								{
									if (chip4 != null)
									{
										chip4.Delete();
									}
								}
								ShowHelpMessage("The aim of Three Card Poker is to make the best poker hand possible with three cards, comparing your hand against the Dealer's hand.");
								Playing_Poker = true;
							}
						}
						else if (Playing_Poker && (!Placebet || Stage == 50f || Stage == 51f || Stage == 52f) && Game.IsControlJustPressed(Control.Context))
						{
							CurrentHelpMessage = "";
							LookingAtDealerCards = false;
							if (RoulleteCam != null)
							{
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 1, 3000, 1, 0);
								RoulleteCam.IsActive = false;
								RoulleteCam.Delete();
							}
							DeleteCardDeck();
							Bet_Play_Index = 0;
							BlackJackSplit = false;
							ResetCardDeck();
							int currentTable3 = 0;
							for (int m = 0; m < PokerTables.Count; m++)
							{
								if (GetDistance_VDIST2(PokerTables[m], GetClosesPokerTable().Position, GetDistSign.Less, 3f))
								{
									currentTable3 = m;
								}
							}
							Bet_PairPlus = 0;
							Bet_Ante = 0;
							Bet_Play = 0;
							BetType = 0;
							Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, GetClosestDealer(Game.Player.Character.Position, PokerDealerTables), "MINIGAME_DEALER_LEAVE_NEUTRAL_GAME", "SPEECH_PARAMS_STANDARD", false);
							Stage = 0f;
							Playercards = 0;
							foreach (DealerTable pokerDealerTable2 in PokerDealerTables)
							{
								if (pokerDealerTable2.Dealer != null)
								{
									ResetDealerPos(pokerDealerTable2);
								}
								if (pokerDealerTable2.Card1 != null)
								{
									pokerDealerTable2.Card1.Delete();
								}
								if (pokerDealerTable2.Card2 != null)
								{
									pokerDealerTable2.Card2.Delete();
								}
								if (pokerDealerTable2.Card3 != null)
								{
									pokerDealerTable2.Card3.Delete();
								}
								if (pokerDealerTable2.Card4 != null)
								{
									pokerDealerTable2.Card4.Delete();
								}
								if (pokerDealerTable2.Card5 != null)
								{
									pokerDealerTable2.Card5.Delete();
								}
								if (pokerDealerTable2.Card6 != null)
								{
									pokerDealerTable2.Card6.Delete();
								}
							}
							foreach (DealerTable blackjackDealerTable2 in BlackjackDealerTables)
							{
								if (blackjackDealerTable2.Dealer != null)
								{
									ResetDealerPos(blackjackDealerTable2);
								}
								if (blackjackDealerTable2.Card1 != null)
								{
									blackjackDealerTable2.Card1.Delete();
								}
								if (blackjackDealerTable2.Card2 != null)
								{
									blackjackDealerTable2.Card2.Delete();
								}
								if (blackjackDealerTable2.Card3 != null)
								{
									blackjackDealerTable2.Card3.Delete();
								}
								if (blackjackDealerTable2.Card4 != null)
								{
									blackjackDealerTable2.Card4.Delete();
								}
								if (blackjackDealerTable2.Card5 != null)
								{
									blackjackDealerTable2.Card5.Delete();
								}
								if (blackjackDealerTable2.Card6 != null)
								{
									blackjackDealerTable2.Card6.Delete();
								}
							}
							foreach (DealerTable rouletteDealerTable4 in RouletteDealerTables)
							{
								if (rouletteDealerTable4.Dealer != null)
								{
									ResetDealerPos(rouletteDealerTable4);
								}
								if (rouletteDealerTable4.Card1 != null)
								{
									rouletteDealerTable4.Card1.Delete();
								}
								if (rouletteDealerTable4.Card2 != null)
								{
									rouletteDealerTable4.Card2.Delete();
								}
								if (rouletteDealerTable4.Card3 != null)
								{
									rouletteDealerTable4.Card3.Delete();
								}
								if (rouletteDealerTable4.Card4 != null)
								{
									rouletteDealerTable4.Card4.Delete();
								}
								if (rouletteDealerTable4.Card5 != null)
								{
									rouletteDealerTable4.Card5.Delete();
								}
								if (rouletteDealerTable4.Card6 != null)
								{
									rouletteDealerTable4.Card6.Delete();
								}
							}
							CurrentTable = currentTable3;
							GetEnterAnim();
							string dict5 = "anim_casino_b@amb@casino@games@shared@player@";
							LoadDict(dict5);
							ClosestTable = GetClosestTable(Game.Player.Character.Position, PokerDealerTables);
							Function.Call<Vector3>(Hash.GET_ENTITY_BONE_POSTION, ClosestDealerTable.Table, Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, ClosestDealerTable.Table, "Chair_Base_03"));
							Function.Call<Vector3>(Hash.GET_ENTITY_BONE_ROTATION, ClosestDealerTable.Table, Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, ClosestDealerTable.Table, "Chair_Base_03"));
							Vector3 item11 = GetPlayerAnimOriginH(SeatSittingIn, TableType.Poker).Item1;
							float item12 = GetPlayerAnimOriginH(SeatSittingIn, TableType.Poker).Item2;
							Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, item11.X, item11.Y, item11.Z - 1f, 0.0, 0.0, item12, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict5), "sit_exit_left", 16f, -16f, 0, 1148846080);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "sit_exit_left", LoadDict(dict5), 16f, -16f, 0, 1148846080);
							Script.Wait(4000);
							Game.Player.Character.Task.ClearAll();
							Game.Player.Character.IsPositionFrozen = false;
							foreach (Prop chip5 in Chips)
							{
								if (chip5 != null)
								{
									chip5.Delete();
								}
							}
							Playing_Poker = false;
							foreach (Prop chip6 in Chips)
							{
								if (chip6 != null)
								{
									chip6.Delete();
								}
							}
							Placebet2 = false;
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
				}
				if (!(GetClosesBlackjackTable() != null) || !IsAllowedToPlayThisGame(GetClosesBlackjackTable(), TableType.Blackjack))
				{
					break;
				}
				if (isPlaceableCasinoGames)
				{
					DisplayChips(Chips_Amount);
				}
				Vector3 item13 = GetClosestSeat(GetClosesBlackjackTable()).Item1;
				GetClosestSeat(GetClosesBlackjackTable());
				int item14 = GetClosestSeat(GetClosesBlackjackTable()).Item3;
				if (GetDistance_VDIST2(Game.Player.Character.Position, GetClosesBlackjackTable().Position, GetDistSign.Less, 2f) || GetDistance_VDIST2(Game.Player.Character.Position, item13, GetDistSign.Less, 2f))
				{
					if (!Playing_BlackJack)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to play BlackJack ");
					}
					if (Playing_BlackJack && !Placebet && !Endgame)
					{
						Scaleform scaleform4 = new Scaleform("instructional_buttons");
						scaleform4.CallFunction("CLEAR_ALL");
						scaleform4.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
						scaleform4.CallFunction("CREATE_CONTAINER");
						scaleform4.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Quit Blackjack");
						scaleform4.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 44, 0), "Confirm Bet");
						scaleform4.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 172, 0), "");
						scaleform4.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 173, 0), "-/+ Bet");
						scaleform4.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
						scaleform4.Render2D();
						drawSprite2("timerbars", "all_black_bg", 0.89f, 0.890000045f, 0.21f, 0.04f);
						drawText("Current Bet :", 0.88f, 0.881000042f, 0.3f, 255, 255, 255);
						drawNumber((int)Bet, 0.950000048f, 0.872000039f, 0.48f, 0, 255, 0);
						if (Game.GameTime >= TimeBetweenBets)
						{
							if (Game.IsControlPressed(Control.PhoneUp))
							{
								BetAmtIndex++;
								int num7 = (int)(Bet / 10000f);
								int num8 = 0;
								Bet = GetBetFromIndex(BetAmtIndex);
								TimeBetweenBets = Game.GameTime + Set_TimeBetweenBets;
								if (BlackjackPlayerInitialBet != null)
								{
									Prop v3 = GlobalHelper.CreateProp(BlackjackPlayerInitialBet.Model, BlackjackPlayerInitialBet.Position, BlackjackPlayerInitialBet.Rotation, false, false);
									EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(v3));
									BlackjackPlayerInitialBet.Delete();
								}
								Prop prop3 = GlobalHelper.CreateProp(RequestModel(GetBetChipModel((int)Bet, true)), ReturnChipPosition(BlackjackDealerTables, BetTypeId.BlackJack), true, false);
								prop3.IsPositionFrozen = true;
								prop3.Heading = ReturnChipAngle(PokerDealerTables);
								Chips.Add(prop3);
								BlackjackPlayerInitialBet = prop3;
								EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(prop3, true, 255));
							}
							if (Game.IsControlPressed(Control.PhoneDown) && Bet >= 10f)
							{
								if (BetAmtIndex > 0)
								{
									BetAmtIndex--;
								}
								int num9 = (int)(Bet / 10000f);
								int num10 = 0;
								Bet = GetBetFromIndex(BetAmtIndex);
								TimeBetweenBets = Game.GameTime + Set_TimeBetweenBets;
								if (BlackjackPlayerInitialBet != null)
								{
									Prop v4 = GlobalHelper.CreateProp(BlackjackPlayerInitialBet.Model, BlackjackPlayerInitialBet.Position, BlackjackPlayerInitialBet.Rotation, false, false);
									EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(v4));
									BlackjackPlayerInitialBet.Delete();
								}
								Prop prop4 = GlobalHelper.CreateProp(RequestModel(GetBetChipModel((int)Bet, true)), ReturnChipPosition(BlackjackDealerTables, BetTypeId.BlackJack), true, false);
								prop4.IsPositionFrozen = true;
								prop4.Heading = ReturnChipAngle(PokerDealerTables);
								Chips.Add(prop4);
								BlackjackPlayerInitialBet = prop4;
								EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(prop4, true, 255));
							}
						}
						if (Bet < 0f)
						{
							Bet = 0f;
						}
					}
					if (Placebet && !Endgame)
					{
						if (BlackJackSplit)
						{
							int right_B_HandTotal = CasinoGames_Blackjack.Get_Right_B_HandTotal();
							int left_AHandTotal = CasinoGames_Blackjack.Get_Left_AHandTotal();
							drawSprite2("timerbars", "all_black_bg", 0.89f, 0.810000062f, 0.21f, 0.04f);
							drawText("RIGHT HAND :", 0.88f, 0.801f, 0.3f, 255, 255, 255);
							drawNumberS(right_B_HandTotal, 0.950000048f, 0.792000055f, 0.48f, 255, 255, 255);
							drawSprite2("timerbars", "all_black_bg", 0.89f, 0.85f, 0.21f, 0.04f);
							drawText("LEFT HAND :", 0.88f, 0.841f, 0.3f, 255, 255, 255);
							drawNumberS(left_AHandTotal, 0.950000048f, 0.832f, 0.48f, 255, 255, 255);
						}
						else
						{
							drawSprite2("timerbars", "all_black_bg", 0.89f, 0.85f, 0.21f, 0.04f);
							drawText("HAND :", 0.88f, 0.841f, 0.3f, 255, 255, 255);
							drawNumberS(PlayerCard_1 + PlayerCard_2 + PlayerCard_3 + PlayerCard_4 + PlayerCard_5, 0.950000048f, 0.832f, 0.48f, 255, 255, 255);
						}
						drawSprite2("timerbars", "all_black_bg", 0.89f, 0.890000045f, 0.21f, 0.04f);
						drawText("BET :", 0.88f, 0.881000042f, 0.3f, 255, 255, 255);
						drawNumber((int)Bet, 0.950000048f, 0.872000039f, 0.48f, 255, 255, 255);
						Scaleform scaleform5 = new Scaleform("instructional_buttons");
						scaleform5.CallFunction("CLEAR_ALL");
						scaleform5.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
						scaleform5.CallFunction("CREATE_CONTAINER");
						scaleform5.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 58, 0), "Accept Hand");
						if (Playercards < 6)
						{
							if (Playercards == 1)
							{
								DisplayHelpTextThisFrame("Take another card to stand with your current hand.");
							}
							if (Playercards == 2)
							{
								if (!BlackJackDoubleDown && !BlackJackSplit)
								{
									DisplayHelpTextThisFrame("Take another card or stand with your current hand. You can Split you hand into two or Double down, double your bet and take just one extra card.");
								}
								if (!BlackJackDoubleDown && BlackJackSplit)
								{
									DisplayHelpTextThisFrame("Take another card or stand with your current hand. Double Down will double your bet and take just one extra card.");
								}
								if (BlackJackDoubleDown && !BlackJackSplit)
								{
									DisplayHelpTextThisFrame("Take another card or stand with your current hand. You can also split your cards into two separate hands by doubling your bet.");
								}
							}
							if (Playercards == 3)
							{
								if (BlackJackDoubleDown)
								{
									DisplayHelpTextThisFrame("Take another card to stand with your current hand.");
								}
								else
								{
									DisplayHelpTextThisFrame("Take another card or stand with your current hand. Double Down will double your bet and take just one extra card.");
								}
							}
							if (Playercards == 4)
							{
								if (BlackJackDoubleDown)
								{
									DisplayHelpTextThisFrame("Take another card to stand with your current hand.");
								}
								else
								{
									DisplayHelpTextThisFrame("Take another card or stand with your current hand. Double Down will double your bet and take just one extra card.");
								}
							}
							if (Playercards == 5)
							{
								if (BlackJackDoubleDown)
								{
									DisplayHelpTextThisFrame("Take another card to stand with your current hand.");
								}
								else
								{
									DisplayHelpTextThisFrame("Take another card or stand with your current hand. Double Down will double your bet and take just one extra card.");
								}
							}
							if (Playercards == 6)
							{
								if (BlackJackDoubleDown)
								{
									DisplayHelpTextThisFrame("Take another card to stand with your current hand.");
								}
								else
								{
									DisplayHelpTextThisFrame("Take another card or stand with your current hand. Double Down will double your bet and take just one extra card.");
								}
							}
							scaleform5.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 44, 0), "Draw Card");
						}
						if (Playercards == 2 && !BlackJackSplit)
						{
							scaleform5.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 76, 0), "Split");
						}
						if (Playercards >= 2 && !BlackJackDoubleDown && Playercards >= 2 && Playercards <= 4)
						{
							scaleform5.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Double Down");
						}
						if (DealerCards >= 2 && !LookingAtDealerCards)
						{
							scaleform5.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 75, 0), "View Dealers Cards");
						}
						else if (LookingAtDealerCards)
						{
							scaleform5.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 75, 0), "Stop Viewing Dealers Cards");
						}
						scaleform5.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
						scaleform5.Render2D();
					}
					if (Stage == 50f)
					{
						Scaleform scaleform6 = new Scaleform("instructional_buttons");
						scaleform6.CallFunction("CLEAR_ALL");
						scaleform6.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
						scaleform6.CallFunction("CREATE_CONTAINER");
						scaleform6.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Exit");
						scaleform6.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 44, 0), "Play Again");
						if (DealerCards >= 2 && !LookingAtDealerCards)
						{
							scaleform6.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 75, 0), "View Dealers Cards");
						}
						else if (LookingAtDealerCards)
						{
							scaleform6.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 75, 0), "Stop Viewing Dealers Cards");
						}
						scaleform6.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
						scaleform6.Render2D();
					}
					if (Endgame)
					{
						Scaleform scaleform7 = new Scaleform("instructional_buttons");
						scaleform7.CallFunction("CLEAR_ALL");
						scaleform7.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
						scaleform7.CallFunction("CREATE_CONTAINER");
						scaleform7.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Exit");
						scaleform7.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 44, 0), "Play Again");
						if (DealerCards >= 2 && !LookingAtDealerCards)
						{
							scaleform7.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 75, 0), "View Dealers Cards");
						}
						else if (LookingAtDealerCards)
						{
							scaleform7.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 75, 0), "Stop Viewing Dealers Cards");
						}
						scaleform7.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
						scaleform7.Render2D();
					}
				}
				if (!GetDistance_VDIST2(Game.Player.Character.Position, GetClosesBlackjackTable().Position, GetDistSign.Less, 2f) && !GetDistance_VDIST2(Game.Player.Character.Position, item13, GetDistSign.Less, 2f))
				{
					break;
				}
				if (!Playing_BlackJack && !Placebet)
				{
					if (!Game.IsControlJustPressed(Control.Context))
					{
						break;
					}
					ShowHelpMessage("The aim of Blackjack is to beat the Dealer's hand without going over 21. This game uses four standard 52-card decks, which are shuffled at the start of every hand. Insurance is not offered if the Dealer's face up card is an ace.");
					SeatSittingIn = item14;
					Stage = 0f;
					LookingAtDealerCards = false;
					if (RoulleteCam != null)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 1, 3000, 1, 0);
						RoulleteCam.IsActive = false;
						RoulleteCam.Delete();
					}
					ClosestDealer = GetClosestDealer(Game.Player.Character.Position, BlackjackDealerTables);
					ClosestTable = GetClosestTable(Game.Player.Character.Position, BlackjackDealerTables);
					ClosestDealerTable = GetClosestDealerTable(Game.Player.Character.Position, BlackjackDealerTables);
					PlayerOriginPosition = GetPlayerAnimOriginH(SeatSittingIn, TableType.Blackjack).Item1;
					PlayerOriginHeading = GetPlayerAnimOriginH(SeatSittingIn, TableType.Blackjack).Item2;
					BlackJackSplit = false;
					BlackJackDoubleDown = false;
					if (Backface_DealerCard1 != null)
					{
						Backface_DealerCard1.Delete();
					}
					ResetCardDeck();
					int currentTable4 = 0;
					for (int n = 0; n < BlackJackTables.Count; n++)
					{
						if (GetDistance_VDIST2(BlackJackTables[n], GetClosesBlackjackTable().Position, GetDistSign.Greater, 3f))
						{
							currentTable4 = n;
						}
					}
					BlackJackCard1 = "";
					BlackJackCard2 = "";
					BlackJackCard3 = "";
					DealerCardsDrawn = 0;
					Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, GetClosestDealer(GetClosesBlackjackTable().Position, BlackjackDealerTables), "MINIGAME_DEALER_GREET", "SPEECH_PARAMS_STANDARD", false);
					if (PlayerCardD != null)
					{
						PlayerCardD.Delete();
					}
					Stage = 0f;
					foreach (DealerTable blackjackDealerTable3 in BlackjackDealerTables)
					{
						if (blackjackDealerTable3.Dealer != null)
						{
							ResetDealerPos(blackjackDealerTable3);
						}
						if (blackjackDealerTable3.Card1 != null)
						{
							blackjackDealerTable3.Card1.Delete();
						}
						if (blackjackDealerTable3.Card2 != null)
						{
							blackjackDealerTable3.Card2.Delete();
						}
						if (blackjackDealerTable3.Card3 != null)
						{
							blackjackDealerTable3.Card3.Delete();
						}
						if (blackjackDealerTable3.Card4 != null)
						{
							blackjackDealerTable3.Card4.Delete();
						}
						if (blackjackDealerTable3.Card5 != null)
						{
							blackjackDealerTable3.Card5.Delete();
						}
						if (blackjackDealerTable3.Card6 != null)
						{
							blackjackDealerTable3.Card6.Delete();
						}
					}
					Stage = 0f;
					ClosestDealer = GetClosestDealer(Game.Player.Character.Position, BlackjackDealerTables);
					bool flag = ClosestDealer == null;
					bool flag2 = ClosestDealer != null;
					ClosestTable = GetClosestTable(Game.Player.Character.Position, BlackjackDealerTables);
					ClosestDealerTable = GetClosestDealerTable(Game.Player.Character.Position, BlackjackDealerTables);
					CurrentTable = currentTable4;
					Function.Call<Vector3>(Hash.GET_ENTITY_BONE_POSTION, ClosestDealerTable.Table, Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, ClosestDealerTable.Table, "Chair_Base_03"));
					Function.Call<Vector3>(Hash.GET_ENTITY_BONE_ROTATION, ClosestDealerTable.Table, Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, ClosestDealerTable.Table, "Chair_Base_03"));
					Vector3 vector3 = GET_PLAYER_CHAIR_POSITION(SeatSittingIn, TableType.Blackjack);
					float z3 = GET_PLAYER_CHAIR_ROTATION(SeatSittingIn, TableType.Blackjack).Z;
					int taskID3 = 0;
					OpenSequenceTask(&taskID3);
					TaskPedSlideToCoord(0, vector3, z3, 8f);
					CloseSequenceTask(taskID3);
					TaskPerformSequence(Game.Player.Character, taskID3);
					ClearSequenceTask(&taskID3);
					Script.Wait(100);
					while (Function.Call<int>(Hash.GET_SEQUENCE_PROGRESS, Game.Player.Character) != -1 && GetDistance_VDIST2(Game.Player.Character.Position, vector3, GetDistSign.Greater, 1.25f))
					{
						Script.Wait(0);
					}
					GetEnterAnim();
					string dict6 = "anim_casino_b@amb@casino@games@shared@player@";
					LoadDict("anim_casino_b@amb@casino@games@shared@player@");
					LoadDict(dict6);
					Vector3 item15 = GetPlayerAnimOriginH(SeatSittingIn, TableType.Blackjack).Item1;
					float item16 = GetPlayerAnimOriginH(SeatSittingIn, TableType.Blackjack).Item2;
					Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, item15.X, item15.Y, item15.Z - 1f, 0.0, 0.0, item16, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict6), "sit_enter_left", 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "sit_enter_left", LoadDict(dict6), 16f, -16f, 0, 1148846080);
					Placebet2 = false;
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
					Bet = 10f;
					EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(BlackjackPlayerInitialBet, true, 255));
					Prop prop5 = GlobalHelper.CreateProp(RequestModel(GetBetChipModel((int)Bet, true)), ReturnChipPosition(BlackjackDealerTables, BetTypeId.BlackJack), true, false);
					prop5.IsPositionFrozen = true;
					prop5.Heading = ReturnChipAngle(PokerDealerTables);
					Chips.Add(prop5);
					BlackjackPlayerInitialBet = prop5;
					DealerCard_1 = 0;
					DealerCard_2 = 0;
					DealerCard_3 = 0;
					DealerCard_4 = 0;
					DealerCard_5 = 0;
					DealerCard_6 = 0;
					foreach (Prop chip7 in Chips)
					{
						if (chip7 != null)
						{
							chip7.Delete();
						}
					}
					Playing_BlackJack = true;
				}
				else
				{
					if (!Playing_BlackJack || (Placebet && Stage != 50f) || (Stage != 22f && Stage != 21f && Stage != 0f && Stage != 50f) || !Game.IsControlJustPressed(Control.Context))
					{
						break;
					}
					CurrentHelpMessage = "";
					LookingAtDealerCards = false;
					if (RoulleteCam != null)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 1, 3000, 1, 0);
						RoulleteCam.IsActive = false;
						RoulleteCam.Delete();
					}
					BlackJackSplit = false;
					BlackJackDoubleDown = false;
					if (Backface_DealerCard1 != null)
					{
						Backface_DealerCard1.Delete();
					}
					ResetCardDeck();
					int currentTable5 = 0;
					for (int num4 = 0; num4 < BlackJackTables.Count; num4++)
					{
						if (GetDistance_VDIST2(BlackJackTables[num4], GetClosesBlackjackTable().Position, GetDistSign.Less, 3f))
						{
							currentTable5 = num4;
						}
					}
					Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, GetClosestDealer(Game.Player.Character.Position, BlackjackDealerTables), "MINIGAME_DEALER_LEAVE_NEUTRAL_GAME", "SPEECH_PARAMS_STANDARD", false);
					if (PlayerCardD != null)
					{
						PlayerCardD.Delete();
					}
					foreach (DealerTable blackjackDealerTable4 in BlackjackDealerTables)
					{
						if (blackjackDealerTable4.Dealer != null)
						{
							ResetDealerPos(blackjackDealerTable4);
						}
						if (blackjackDealerTable4.Card1 != null)
						{
							blackjackDealerTable4.Card1.Delete();
						}
						if (blackjackDealerTable4.Card2 != null)
						{
							blackjackDealerTable4.Card2.Delete();
						}
						if (blackjackDealerTable4.Card3 != null)
						{
							blackjackDealerTable4.Card3.Delete();
						}
						if (blackjackDealerTable4.Card4 != null)
						{
							blackjackDealerTable4.Card4.Delete();
						}
						if (blackjackDealerTable4.Card5 != null)
						{
							blackjackDealerTable4.Card5.Delete();
						}
						if (blackjackDealerTable4.Card6 != null)
						{
							blackjackDealerTable4.Card6.Delete();
						}
					}
					DealerCardsDrawn = 0;
					Stage = 0f;
					ClosestDealer = GetClosestDealer(Game.Player.Character.Position, BlackjackDealerTables);
					ClosestTable = GetClosestTable(Game.Player.Character.Position, BlackjackDealerTables);
					ClosestDealerTable = GetClosestDealerTable(Game.Player.Character.Position, BlackjackDealerTables);
					World.DrawMarker(MarkerType.Cylinder, ClosestDealer.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.5f, 0.05f, 0.5f), Color.Green);
					World.DrawMarker(MarkerType.Cylinder, ClosestTable.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.5f, 0.5f, 0.5f), Color.Blue);
					CurrentTable = currentTable5;
					Function.Call<Vector3>(Hash.GET_ENTITY_BONE_POSTION, ClosestDealerTable.Table, Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, ClosestDealerTable.Table, "Chair_Base_03"));
					Function.Call<Vector3>(Hash.GET_ENTITY_BONE_ROTATION, ClosestDealerTable.Table, Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, ClosestDealerTable.Table, "Chair_Base_03"));
					GetEnterAnim();
					string dict7 = "anim_casino_b@amb@casino@games@shared@player@";
					LoadDict(dict7);
					Vector3 item17 = GetPlayerAnimOriginH(SeatSittingIn, TableType.Blackjack).Item1;
					float item18 = GetPlayerAnimOriginH(SeatSittingIn, TableType.Blackjack).Item2;
					Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, item17.X, item17.Y, item17.Z - 1f, 0.0, 0.0, item18, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict7), "sit_exit_left", 16f, -16f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "sit_exit_left", LoadDict(dict7), 16f, -16f, 0, 1148846080);
					Script.Wait(4000);
					Game.Player.Character.Task.ClearAll();
					Game.Player.Character.IsPositionFrozen = false;
					foreach (Prop chip8 in Chips)
					{
						if (chip8 != null)
						{
							chip8.Delete();
						}
					}
					Playing_BlackJack = false;
					foreach (Prop chip9 in Chips)
					{
						if (chip9 != null)
						{
							chip9.Delete();
						}
					}
					Placebet2 = false;
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
				break;
			}
			case CasinoGamePlaying.Blackjack:
			{
				if (!Playing_BlackJack)
				{
					CurrentGame = CasinoGamePlaying.None;
				}
				bool playing_BlackJack = Playing_BlackJack;
				break;
			}
			case CasinoGamePlaying.Roulette:
				if (!Playing_Roulette)
				{
					CurrentGame = CasinoGamePlaying.None;
				}
				break;
			}
		}
	}
}
