using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace GTAO_DiamondCasino.Games
{
	internal class EightPlayerPoker : Script
	{
		public static bool RunMethodNow()
		{
			bool result = true;
			if (CasinoGames._8PlayerPokerGameTable1 != null)
			{
				if (CasinoGames.GetDistance_VDIST2(Game.Player.Character.Position, CasinoGames._8PlayerPokerGameTable1.Position, CasinoGames.GetDistSign.GreaterEqual, 10f))
				{
					_Debug.EightPlayerPoker = 0;
					result = false;
				}
			}
			else
			{
				_Debug.EightPlayerPoker = 0;
				result = false;
			}
			return result;
		}

		public EightPlayerPoker()
		{
			if (_Debug.EightPlayerPoker != -1)
			{
				base.Tick += OnTick;
			}
		}

		public static void OnTick(object sender, EventArgs e)
		{
			if (global::Gambling.Diamond.Enabled) return;   // RealismSuite plays this game here
			if (!CasinoGames.CanRunModRightNow || !Class1.Initialized)
			{
				return;
			}
			if (CasinoGames._8PlayerPokerGameTable1 != null)
			{
				if (CasinoGames.GetDistance_VDIST2(Game.Player.Character.Position, CasinoGames._8PlayerPokerGameTable1.Position, CasinoGames.GetDistSign.GreaterEqual, 10f))
				{
					_Debug.EightPlayerPoker = 0;
					return;
				}
				_Debug.EightPlayerPoker = 1;
				if (CasinoGames._8PlayerPokerGameTable1 != null && CasinoGames.GetDistance_VDIST2(Game.Player.Character.Position, CasinoGames._8PlayerPokerGameTable1.Position, CasinoGames.GetDistSign.Less, 5f))
				{
					if (CasinoGames.Playing8PlayerPoker)
					{
						CasinoGames.ShowPlayerTotals();
						CasinoGames.ShowPlayerBanks();
						Screen.ShowSubtitle("~o~ Pot : $" + CasinoGames._8PlayerGamePokerPot.ToString("N"));
						if (CasinoGames._8PlayerGameStage == 1)
						{
							CasinoGames.Show_Q("Quit");
						}
						if (CasinoGames._8PlayerGameStage == 2)
						{
							CasinoGames.Show_EQ("Continue", "Quit");
						}
						if (CasinoGames._8PlayerGameStage == 3)
						{
							CasinoGames.Show_EGQRX("Look at Cards", "Place Bet", "Quit", "Fold", "Check");
						}
						if (CasinoGames.Player_Bust)
						{
							CasinoGames.Show_Q("Quit");
						}
					}
					if (!CasinoGames.Playing8PlayerPoker)
					{
						CasinoGames.Show_EQ("   Current Chips :   " + Game.Player.Money.ToString("N") + "   ", "    Current Chips :" + CasinoGames.Chips_Amount.ToString("N") + "    ");
						CasinoGames.DisplayHelpTextThisFrameNoSound("Press ~INPUT_COVER~ to Play 8 Player Poker");
					}
				}
				if (CasinoGames.Playing8PlayerPoker)
				{
					if (CasinoGames.AreBetsBeingPlaced)
					{
						string text = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
						string text2 = "bet_plus";
						Random random = new Random();
						int num = 0;
						CasinoGames.LastRaiseAmmount = CasinoGames.BettingRoundStartingBet;
						if (CasinoGames.BettingRoundStage == 0)
						{
							if (!CasinoGames.AI1_Bust && !CasinoGames.AI1_SkipTurn && !CasinoGames.AI1_HasPlayedBettingRoundMove)
							{
								bool foldCauseHighBet = CasinoGames.GetFoldCauseHighBet(CasinoGames.BettingRoundStartingBet, CasinoGames._8PlayerGamePoker_AI1Bank);
								bool foldCauseLowScore = CasinoGames.GetFoldCauseLowScore(CasinoGames.AI1Total);
								if (CasinoGames._8PlayerGamePoker_AI1Bank >= CasinoGames.BettingRoundStartingBet && !foldCauseHighBet && !foldCauseLowScore)
								{
									text = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
									text2 = "bet_plus";
									CasinoGames.PlaySyncAnim(CasinoGames.AI_1, CasinoGames.AI_1, CasinoGames.AI1_SyncScene, text, text2);
									int num2 = random.Next(0, 100);
									int num3 = 0;
									float aI1Total = CasinoGames.AI1Total;
									if (aI1Total >= 0f && aI1Total < 50f)
									{
										num3 = 10;
									}
									if (aI1Total >= 50f && aI1Total < 200f)
									{
										num3 = 14;
									}
									if (aI1Total >= 100f && aI1Total < 200f)
									{
										num3 = 22;
									}
									if (aI1Total >= 200f && aI1Total < 300f)
									{
										num3 = 35;
									}
									if (aI1Total >= 300f && aI1Total < 400f)
									{
										num3 = 45;
									}
									if (aI1Total >= 400f && aI1Total < 700f)
									{
										num3 = 65;
									}
									if (aI1Total >= 700f && aI1Total < 1000f)
									{
										num3 = 60;
									}
									if (aI1Total >= 1000f && aI1Total < 2500f)
									{
										num3 = 70;
									}
									if (aI1Total >= 2500f)
									{
										num3 = 85;
									}
									if (num3 > num2)
									{
										float num4 = random.Next(100, 2500);
										if (CasinoGames._8PlayerGamePoker_AI1Bank - CasinoGames.BettingRoundStartingBet > 0f)
										{
											CasinoGames.BettingRoundStartingBet += num4;
											Notification.Show("AI 1 ~y~Raised~w~ the Bet");
										}
										else
										{
											Notification.Show("AI 1 ~y~Cannot Raise~w~ Bet, ~y~Called~ Bet!");
										}
									}
									else
									{
										Notification.Show("AI 1 ~y~Called~w~ the Bet");
									}
									CasinoGames._8PlayerGamePokerPot += CasinoGames.BettingRoundStartingBet;
									CasinoGames.RunBet(CasinoGames._8PlayerGamePokerPot);
									CasinoGames._8PlayerGamePoker_AI1Bank -= CasinoGames.BettingRoundStartingBet;
									if (CasinoGames.AI_1_VisualBank != null)
									{
										CasinoGames.AI_1_VisualBank.SpawnChips(CasinoGames._8PlayerGamePoker_AI1Bank);
									}
									CasinoGames.AI1_HasPlayedBettingRoundMove = true;
								}
								else
								{
									Ped aI_ = CasinoGames.AI_1;
									CasinoGames.AI1_SkipTurn = true;
									text = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@play@v01";
									text2 = "check_cards_fold";
									CasinoGames.LoadDict(text);
									CasinoGames.PlaySyncAnim(CasinoGames.AI_1, CasinoGames.AI_1, CasinoGames.AI1_SyncScene, text, text2);
									try
									{
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[num].Card1, CasinoGames.AI1_SyncScene, "check_cards_fold_card_a", CasinoGames.LoadDict(text), 16f, -16f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[num].Card2, CasinoGames.AI1_SyncScene, "check_cards_fold_card_b", CasinoGames.LoadDict(text), 16f, -16f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[num].Card3, CasinoGames.AI1_SyncScene, "check_cards_fold_card_c", CasinoGames.LoadDict(text), 16f, -16f, 0, 1148846080);
									}
									catch
									{
									}
									int num5 = new Random().Next(0, 8);
									if (num5 == 0)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_, "GENERIC_INSULT_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num5 == 1)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_, "GENERIC_FUCK_YOU", "Speech_Params_Force_Shouted_Clear");
									}
									if (num5 == 2)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_, "GENERIC_INSULT_HIGH", "Speech_Params_Force_Shouted_Clear");
									}
									if (num5 == 3)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_, "GENERIC_SHOCKED_HIGH", "Speech_Params_Force_Shouted_Clear");
									}
									if (num5 == 4)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_, "GENERIC_SHOCKED_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num5 == 5)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_, "GENERIC_FRIGHTENED_HIGH", "Speech_Params_Force_Shouted_Clear");
									}
									if (num5 == 6)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_, "GENERIC_FRIGHTENED_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num5 == 7)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num5 == 8)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (CasinoGames._8PlayerGamePoker_AI1Bank >= CasinoGames.BettingRoundStartingBet)
									{
										Notification.Show("AI 1 has ~y~Folded!");
									}
									else if (CasinoGames._8PlayerGamePoker_AI1Bank >= 1000f)
									{
										CasinoGames.AI1_SkipTurn = true;
										CasinoGames.AI1_Bust = true;
										Notification.Show("AI 1 has gone ~y~Bust!");
									}
									else
									{
										Notification.Show("AI 1 has ~y~Folded!");
									}
									CasinoGames.AI2_HasPlayedBettingRoundMove = true;
								}
							}
							if (CasinoGames.AI1_Bust || CasinoGames.AI1_SkipTurn)
							{
								CasinoGames.BettingRoundStage = 1;
							}
						}
						if (CasinoGames.AI1_HasPlayedBettingRoundMove && (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI1_SyncScene) >= 1065353216 || Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI1_SyncScene) == 0))
						{
							CasinoGames.AI1_HasPlayedBettingRoundMove = false;
							CasinoGames.BettingRoundStage = 1;
						}
						num++;
						if (CasinoGames.BettingRoundStage == 1)
						{
							if (!CasinoGames.AI2_Bust && !CasinoGames.AI2_SkipTurn && !CasinoGames.AI2_HasPlayedBettingRoundMove)
							{
								bool foldCauseHighBet2 = CasinoGames.GetFoldCauseHighBet(CasinoGames.BettingRoundStartingBet, CasinoGames._8PlayerGamePoker_AI2Bank);
								bool foldCauseLowScore2 = CasinoGames.GetFoldCauseLowScore(CasinoGames.AI2Total);
								if (CasinoGames._8PlayerGamePoker_AI2Bank >= CasinoGames.BettingRoundStartingBet && !foldCauseHighBet2 && !foldCauseLowScore2)
								{
									text = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
									text2 = "bet_plus";
									CasinoGames.PlaySyncAnim(CasinoGames.AI_2, CasinoGames.AI_2, CasinoGames.AI2_SyncScene, text, text2);
									int num6 = random.Next(0, 100);
									int num7 = 0;
									float aI2Total = CasinoGames.AI2Total;
									if (aI2Total >= 0f && aI2Total < 50f)
									{
										num7 = 10;
									}
									if (aI2Total >= 50f && aI2Total < 200f)
									{
										num7 = 14;
									}
									if (aI2Total >= 100f && aI2Total < 200f)
									{
										num7 = 22;
									}
									if (aI2Total >= 200f && aI2Total < 300f)
									{
										num7 = 35;
									}
									if (aI2Total >= 300f && aI2Total < 400f)
									{
										num7 = 45;
									}
									if (aI2Total >= 400f && aI2Total < 700f)
									{
										num7 = 65;
									}
									if (aI2Total >= 700f && aI2Total < 1000f)
									{
										num7 = 60;
									}
									if (aI2Total >= 1000f && aI2Total < 2500f)
									{
										num7 = 70;
									}
									if (aI2Total >= 2500f)
									{
										num7 = 85;
									}
									if (num7 > num6)
									{
										float num8 = random.Next(100, 2500);
										if (CasinoGames._8PlayerGamePoker_AI2Bank - CasinoGames.BettingRoundStartingBet > 0f)
										{
											CasinoGames.BettingRoundStartingBet += num8;
											Notification.Show("AI 2 ~y~Raised~w~ the Bet");
										}
										else
										{
											Notification.Show("Parter ~y~Cannot Raise~w~ Bet, ~y~Called~ Bet!");
										}
									}
									else
									{
										Notification.Show("AI 2 ~y~Called~w~ the Bet");
									}
									CasinoGames._8PlayerGamePokerPot += CasinoGames.BettingRoundStartingBet;
									CasinoGames.RunBet(CasinoGames._8PlayerGamePokerPot);
									CasinoGames._8PlayerGamePoker_AI2Bank -= CasinoGames.BettingRoundStartingBet;
									if (CasinoGames.AI_2_VisualBank != null)
									{
										CasinoGames.AI_2_VisualBank.SpawnChips(CasinoGames._8PlayerGamePoker_AI2Bank);
									}
									CasinoGames.AI2_HasPlayedBettingRoundMove = true;
								}
								else
								{
									Ped aI_2 = CasinoGames.AI_2;
									CasinoGames.AI2_SkipTurn = true;
									text = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@play@v01";
									text2 = "check_cards_fold";
									CasinoGames.LoadDict(text);
									CasinoGames.PlaySyncAnim(CasinoGames.AI_2, CasinoGames.AI_2, CasinoGames.AI2_SyncScene, text, text2);
									try
									{
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[num].Card1, CasinoGames.AI2_SyncScene, "check_cards_fold_card_a", CasinoGames.LoadDict(text), 16f, -16f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[num].Card2, CasinoGames.AI2_SyncScene, "check_cards_fold_card_b", CasinoGames.LoadDict(text), 16f, -16f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[num].Card3, CasinoGames.AI2_SyncScene, "check_cards_fold_card_c", CasinoGames.LoadDict(text), 16f, -16f, 0, 1148846080);
									}
									catch
									{
									}
									int num9 = new Random().Next(0, 8);
									if (num9 == 0)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_2, "GENERIC_INSULT_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num9 == 1)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_2, "GENERIC_FUCK_YOU", "Speech_Params_Force_Shouted_Clear");
									}
									if (num9 == 2)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_2, "GENERIC_INSULT_HIGH", "Speech_Params_Force_Shouted_Clear");
									}
									if (num9 == 3)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_2, "GENERIC_SHOCKED_HIGH", "Speech_Params_Force_Shouted_Clear");
									}
									if (num9 == 4)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_2, "GENERIC_SHOCKED_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num9 == 5)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_2, "GENERIC_FRIGHTENED_HIGH", "Speech_Params_Force_Shouted_Clear");
									}
									if (num9 == 6)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_2, "GENERIC_FRIGHTENED_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num9 == 7)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_2, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num9 == 8)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_2, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (CasinoGames._8PlayerGamePoker_AI2Bank >= CasinoGames.BettingRoundStartingBet)
									{
										Notification.Show("AI 2 has ~y~Folded!");
									}
									else if (CasinoGames._8PlayerGamePoker_AI2Bank >= 1000f)
									{
										CasinoGames.AI2_SkipTurn = true;
										CasinoGames.AI2_Bust = true;
										Notification.Show("AI 2 has gone ~y~Bust!");
									}
									else
									{
										Notification.Show("AI 2 has ~y~Folded!");
									}
									CasinoGames.AI2_HasPlayedBettingRoundMove = true;
								}
							}
							else if (CasinoGames.AI2_Bust || CasinoGames.AI2_SkipTurn)
							{
								CasinoGames.BettingRoundStage = 2;
							}
						}
						if (CasinoGames.AI2_HasPlayedBettingRoundMove && (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI2_SyncScene) >= 1065353216 || Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI2_SyncScene) == 0))
						{
							CasinoGames.AI2_HasPlayedBettingRoundMove = false;
							CasinoGames.BettingRoundStage = 2;
						}
						num++;
						if (CasinoGames.BettingRoundStage == 2)
						{
							if (!CasinoGames.AI3_Bust && !CasinoGames.AI3_SkipTurn && !CasinoGames.AI3_HasPlayedBettingRoundMove)
							{
								bool foldCauseHighBet3 = CasinoGames.GetFoldCauseHighBet(CasinoGames.BettingRoundStartingBet, CasinoGames._8PlayerGamePoker_AI3Bank);
								bool foldCauseLowScore3 = CasinoGames.GetFoldCauseLowScore(CasinoGames.AI3Total);
								if (CasinoGames._8PlayerGamePoker_AI3Bank >= CasinoGames.BettingRoundStartingBet && !foldCauseHighBet3 && !foldCauseLowScore3)
								{
									text = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
									text2 = "bet_plus";
									CasinoGames.PlaySyncAnim(CasinoGames.AI_3, CasinoGames.AI_3, CasinoGames.AI3_SyncScene, text, text2);
									int num10 = random.Next(0, 100);
									int num11 = 0;
									float aI3Total = CasinoGames.AI3Total;
									if (aI3Total >= 0f && aI3Total < 50f)
									{
										num11 = 10;
									}
									if (aI3Total >= 50f && aI3Total < 200f)
									{
										num11 = 14;
									}
									if (aI3Total >= 100f && aI3Total < 200f)
									{
										num11 = 22;
									}
									if (aI3Total >= 200f && aI3Total < 300f)
									{
										num11 = 35;
									}
									if (aI3Total >= 300f && aI3Total < 400f)
									{
										num11 = 45;
									}
									if (aI3Total >= 400f && aI3Total < 700f)
									{
										num11 = 65;
									}
									if (aI3Total >= 700f && aI3Total < 1000f)
									{
										num11 = 60;
									}
									if (aI3Total >= 1000f && aI3Total < 2500f)
									{
										num11 = 70;
									}
									if (aI3Total >= 2500f)
									{
										num11 = 85;
									}
									if (num11 > num10)
									{
										float num12 = random.Next(100, 2500);
										if (CasinoGames._8PlayerGamePoker_AI3Bank - CasinoGames.BettingRoundStartingBet > 0f)
										{
											CasinoGames.BettingRoundStartingBet += num12;
											Notification.Show("AI 3 ~y~Raised~w~ the Bet");
										}
										else
										{
											Notification.Show("AI 3 ~y~Cannot Raise~w~ Bet, ~y~Called~ Bet!");
										}
									}
									else
									{
										Notification.Show("AI 3 ~y~Called~w~ the Bet");
									}
									CasinoGames._8PlayerGamePokerPot += CasinoGames.BettingRoundStartingBet;
									CasinoGames.RunBet(CasinoGames._8PlayerGamePokerPot);
									CasinoGames._8PlayerGamePoker_AI3Bank -= CasinoGames.BettingRoundStartingBet;
									if (CasinoGames.AI_3_VisualBank != null)
									{
										CasinoGames.AI_3_VisualBank.SpawnChips(CasinoGames._8PlayerGamePoker_AI3Bank);
									}
									CasinoGames.AI3_HasPlayedBettingRoundMove = true;
								}
								else
								{
									Ped aI_3 = CasinoGames.AI_3;
									CasinoGames.AI3_SkipTurn = true;
									text = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@play@v01";
									text2 = "check_cards_fold";
									CasinoGames.LoadDict(text);
									CasinoGames.PlaySyncAnim(CasinoGames.AI_3, CasinoGames.AI_3, CasinoGames.AI3_SyncScene, text, text2);
									try
									{
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[num].Card1, CasinoGames.AI3_SyncScene, "check_cards_fold_card_a", CasinoGames.LoadDict(text), 16f, -16f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[num].Card2, CasinoGames.AI3_SyncScene, "check_cards_fold_card_b", CasinoGames.LoadDict(text), 16f, -16f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[num].Card3, CasinoGames.AI3_SyncScene, "check_cards_fold_card_c", CasinoGames.LoadDict(text), 16f, -16f, 0, 1148846080);
									}
									catch
									{
									}
									int num13 = new Random().Next(0, 8);
									if (num13 == 0)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_3, "GENERIC_INSULT_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num13 == 1)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_3, "GENERIC_FUCK_YOU", "Speech_Params_Force_Shouted_Clear");
									}
									if (num13 == 2)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_3, "GENERIC_INSULT_HIGH", "Speech_Params_Force_Shouted_Clear");
									}
									if (num13 == 3)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_3, "GENERIC_SHOCKED_HIGH", "Speech_Params_Force_Shouted_Clear");
									}
									if (num13 == 4)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_3, "GENERIC_SHOCKED_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num13 == 5)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_3, "GENERIC_FRIGHTENED_HIGH", "Speech_Params_Force_Shouted_Clear");
									}
									if (num13 == 6)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_3, "GENERIC_FRIGHTENED_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num13 == 7)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_3, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num13 == 8)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_3, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (CasinoGames._8PlayerGamePoker_AI3Bank >= CasinoGames.BettingRoundStartingBet)
									{
										Notification.Show("AI 3 has ~y~Folded!");
									}
									else if (CasinoGames._8PlayerGamePoker_AI3Bank >= 1000f)
									{
										CasinoGames.AI3_SkipTurn = true;
										CasinoGames.AI3_Bust = true;
										Notification.Show("AI 3 has gone ~y~Bust!");
									}
									else
									{
										Notification.Show("AI 3 has ~y~Folded!");
									}
									CasinoGames.AI3_HasPlayedBettingRoundMove = true;
								}
							}
							if (CasinoGames.AI3_Bust || CasinoGames.AI3_SkipTurn)
							{
								CasinoGames.BettingRoundStage = 3;
							}
						}
						if (CasinoGames.AI3_HasPlayedBettingRoundMove && (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI3_SyncScene) >= 1065353216 || Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI3_SyncScene) == 0))
						{
							CasinoGames.AI3_HasPlayedBettingRoundMove = false;
							CasinoGames.BettingRoundStage = 3;
						}
						num++;
						if (CasinoGames.BettingRoundStage == 3)
						{
							if (!CasinoGames.AI4_Bust && !CasinoGames.AI4_SkipTurn && !CasinoGames.AI4_HasPlayedBettingRoundMove)
							{
								bool foldCauseHighBet4 = CasinoGames.GetFoldCauseHighBet(CasinoGames.BettingRoundStartingBet, CasinoGames._8PlayerGamePoker_AI4Bank);
								bool foldCauseLowScore4 = CasinoGames.GetFoldCauseLowScore(CasinoGames.AI4Total);
								if (CasinoGames._8PlayerGamePoker_AI4Bank >= CasinoGames.BettingRoundStartingBet && !foldCauseHighBet4 && !foldCauseLowScore4)
								{
									text = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
									text2 = "bet_plus";
									CasinoGames.PlaySyncAnim(CasinoGames.AI_4, CasinoGames.AI_4, CasinoGames.AI4_SyncScene, text, text2);
									int num14 = random.Next(0, 100);
									int num15 = 0;
									float aI4Total = CasinoGames.AI4Total;
									if (aI4Total >= 0f && aI4Total < 50f)
									{
										num15 = 10;
									}
									if (aI4Total >= 50f && aI4Total < 200f)
									{
										num15 = 14;
									}
									if (aI4Total >= 100f && aI4Total < 200f)
									{
										num15 = 22;
									}
									if (aI4Total >= 200f && aI4Total < 300f)
									{
										num15 = 35;
									}
									if (aI4Total >= 300f && aI4Total < 400f)
									{
										num15 = 45;
									}
									if (aI4Total >= 400f && aI4Total < 700f)
									{
										num15 = 65;
									}
									if (aI4Total >= 700f && aI4Total < 1000f)
									{
										num15 = 60;
									}
									if (aI4Total >= 1000f && aI4Total < 2500f)
									{
										num15 = 70;
									}
									if (aI4Total >= 2500f)
									{
										num15 = 85;
									}
									if (num15 > num14)
									{
										float num16 = random.Next(100, 2500);
										if (CasinoGames._8PlayerGamePoker_AI4Bank - CasinoGames.BettingRoundStartingBet > 0f)
										{
											CasinoGames.BettingRoundStartingBet += num16;
											Notification.Show("AI 4 ~y~Raised~w~ the Bet");
										}
										else
										{
											Notification.Show("AI 4 ~y~Cannot Raise~w~ Bet, ~y~Called~ Bet!");
										}
									}
									else
									{
										Notification.Show("AI 4 ~y~Called~w~ the Bet");
									}
									CasinoGames._8PlayerGamePokerPot += CasinoGames.BettingRoundStartingBet;
									CasinoGames.RunBet(CasinoGames._8PlayerGamePokerPot);
									CasinoGames._8PlayerGamePoker_AI4Bank -= CasinoGames.BettingRoundStartingBet;
									if (CasinoGames.AI_4_VisualBank != null)
									{
										CasinoGames.AI_4_VisualBank.SpawnChips(CasinoGames._8PlayerGamePoker_AI4Bank);
									}
									CasinoGames.AI4_HasPlayedBettingRoundMove = true;
								}
								else
								{
									Ped aI_4 = CasinoGames.AI_4;
									CasinoGames.AI4_SkipTurn = true;
									text = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@play@v01";
									text2 = "check_cards_fold";
									CasinoGames.LoadDict(text);
									CasinoGames.PlaySyncAnim(CasinoGames.AI_4, CasinoGames.AI_4, CasinoGames.AI4_SyncScene, text, text2);
									try
									{
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[num].Card1, CasinoGames.AI4_SyncScene, "check_cards_fold_card_a", CasinoGames.LoadDict(text), 16f, -16f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[num].Card2, CasinoGames.AI4_SyncScene, "check_cards_fold_card_b", CasinoGames.LoadDict(text), 16f, -16f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[num].Card3, CasinoGames.AI4_SyncScene, "check_cards_fold_card_c", CasinoGames.LoadDict(text), 16f, -16f, 0, 1148846080);
									}
									catch
									{
									}
									int num17 = new Random().Next(0, 8);
									if (num17 == 0)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_4, "GENERIC_INSULT_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num17 == 1)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_4, "GENERIC_FUCK_YOU", "Speech_Params_Force_Shouted_Clear");
									}
									if (num17 == 2)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_4, "GENERIC_INSULT_HIGH", "Speech_Params_Force_Shouted_Clear");
									}
									if (num17 == 3)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_4, "GENERIC_SHOCKED_HIGH", "Speech_Params_Force_Shouted_Clear");
									}
									if (num17 == 4)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_4, "GENERIC_SHOCKED_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num17 == 5)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_4, "GENERIC_FRIGHTENED_HIGH", "Speech_Params_Force_Shouted_Clear");
									}
									if (num17 == 6)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_4, "GENERIC_FRIGHTENED_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num17 == 7)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_4, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num17 == 8)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_4, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (CasinoGames._8PlayerGamePoker_AI4Bank >= CasinoGames.BettingRoundStartingBet)
									{
										Notification.Show("AI 4 has ~y~Folded!");
									}
									else if (CasinoGames._8PlayerGamePoker_AI4Bank >= 1000f)
									{
										CasinoGames.AI4_SkipTurn = true;
										CasinoGames.AI4_Bust = true;
										Notification.Show("AI 4 has gone ~y~Bust!");
									}
									else
									{
										Notification.Show("AI 4 has ~y~Folded!");
									}
									CasinoGames.AI4_HasPlayedBettingRoundMove = true;
								}
							}
							if (CasinoGames.AI4_Bust || CasinoGames.AI4_SkipTurn)
							{
								CasinoGames.BettingRoundStage = 4;
							}
						}
						if (CasinoGames.AI4_HasPlayedBettingRoundMove && (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI4_SyncScene) >= 1065353216 || Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI4_SyncScene) == 0))
						{
							CasinoGames.AI4_HasPlayedBettingRoundMove = false;
							CasinoGames.BettingRoundStage = 4;
						}
						num++;
						if (CasinoGames.BettingRoundStage == 4)
						{
							if (!CasinoGames.AI5_Bust && !CasinoGames.AI5_SkipTurn)
							{
								bool foldCauseHighBet5 = CasinoGames.GetFoldCauseHighBet(CasinoGames.BettingRoundStartingBet, CasinoGames._8PlayerGamePoker_AI5Bank);
								bool foldCauseLowScore5 = CasinoGames.GetFoldCauseLowScore(CasinoGames.AI5Total);
								if (CasinoGames._8PlayerGamePoker_AI5Bank >= CasinoGames.BettingRoundStartingBet && !foldCauseHighBet5 && !foldCauseLowScore5)
								{
									text = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
									text2 = "bet_plus";
									CasinoGames.PlaySyncAnim(CasinoGames.AI_5, CasinoGames.AI_5, CasinoGames.AI5_SyncScene, text, text2);
									int num18 = random.Next(0, 100);
									int num19 = 0;
									float aI5Total = CasinoGames.AI5Total;
									if (aI5Total >= 0f && aI5Total < 50f)
									{
										num19 = 10;
									}
									if (aI5Total >= 50f && aI5Total < 200f)
									{
										num19 = 14;
									}
									if (aI5Total >= 100f && aI5Total < 200f)
									{
										num19 = 22;
									}
									if (aI5Total >= 200f && aI5Total < 300f)
									{
										num19 = 35;
									}
									if (aI5Total >= 300f && aI5Total < 400f)
									{
										num19 = 45;
									}
									if (aI5Total >= 400f && aI5Total < 700f)
									{
										num19 = 65;
									}
									if (aI5Total >= 700f && aI5Total < 1000f)
									{
										num19 = 60;
									}
									if (aI5Total >= 1000f && aI5Total < 2500f)
									{
										num19 = 70;
									}
									if (aI5Total >= 2500f)
									{
										num19 = 85;
									}
									if (num19 > num18)
									{
										float num20 = random.Next(100, 2500);
										if (CasinoGames._8PlayerGamePoker_AI5Bank - CasinoGames.BettingRoundStartingBet > 0f)
										{
											CasinoGames.BettingRoundStartingBet += num20;
											Notification.Show("AI 5 ~y~Raised~w~ the Bet");
										}
										else
										{
											Notification.Show("AI 5 ~y~Cannot Raise~w~ Bet, ~y~Called~ Bet!");
										}
									}
									else
									{
										Notification.Show("AI 5 ~y~Called~w~ the Bet");
									}
									CasinoGames._8PlayerGamePokerPot += CasinoGames.BettingRoundStartingBet;
									CasinoGames.RunBet(CasinoGames._8PlayerGamePokerPot);
									CasinoGames._8PlayerGamePoker_AI5Bank -= CasinoGames.BettingRoundStartingBet;
									if (CasinoGames.AI_5_VisualBank != null)
									{
										CasinoGames.AI_5_VisualBank.SpawnChips(CasinoGames._8PlayerGamePoker_AI5Bank);
									}
									CasinoGames.AI5_HasPlayedBettingRoundMove = true;
								}
								else
								{
									Ped aI_5 = CasinoGames.AI_5;
									CasinoGames.AI5_SkipTurn = true;
									text = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@play@v01";
									text2 = "check_cards_fold";
									CasinoGames.LoadDict(text);
									CasinoGames.PlaySyncAnim(CasinoGames.AI_5, CasinoGames.AI_5, CasinoGames.AI5_SyncScene, text, text2);
									try
									{
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[num].Card1, CasinoGames.AI5_SyncScene, "check_cards_fold_card_a", CasinoGames.LoadDict(text), 16f, -16f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[num].Card2, CasinoGames.AI5_SyncScene, "check_cards_fold_card_b", CasinoGames.LoadDict(text), 16f, -16f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[num].Card3, CasinoGames.AI5_SyncScene, "check_cards_fold_card_c", CasinoGames.LoadDict(text), 16f, -16f, 0, 1148846080);
									}
									catch
									{
									}
									int num21 = new Random().Next(0, 8);
									if (num21 == 0)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_5, "GENERIC_INSULT_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num21 == 1)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_5, "GENERIC_FUCK_YOU", "Speech_Params_Force_Shouted_Clear");
									}
									if (num21 == 2)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_5, "GENERIC_INSULT_HIGH", "Speech_Params_Force_Shouted_Clear");
									}
									if (num21 == 3)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_5, "GENERIC_SHOCKED_HIGH", "Speech_Params_Force_Shouted_Clear");
									}
									if (num21 == 4)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_5, "GENERIC_SHOCKED_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num21 == 5)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_5, "GENERIC_FRIGHTENED_HIGH", "Speech_Params_Force_Shouted_Clear");
									}
									if (num21 == 6)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_5, "GENERIC_FRIGHTENED_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num21 == 7)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_5, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num21 == 8)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_5, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (CasinoGames._8PlayerGamePoker_AI5Bank >= CasinoGames.BettingRoundStartingBet)
									{
										Notification.Show("AI 5 has ~y~Folded!");
									}
									else if (CasinoGames._8PlayerGamePoker_AI5Bank >= 1000f)
									{
										CasinoGames.AI5_SkipTurn = true;
										CasinoGames.AI5_Bust = true;
										Notification.Show("AI 5 has gone ~y~Bust!");
									}
									else
									{
										Notification.Show("AI 5 has ~y~Folded!");
									}
									CasinoGames.AI5_HasPlayedBettingRoundMove = true;
								}
							}
							if (CasinoGames.AI5_Bust || CasinoGames.AI5_SkipTurn)
							{
								CasinoGames.BettingRoundStage = 5;
							}
						}
						if (CasinoGames.AI5_HasPlayedBettingRoundMove && (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI5_SyncScene) >= 1065353216 || Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI5_SyncScene) == 0))
						{
							CasinoGames.AI5_HasPlayedBettingRoundMove = false;
							CasinoGames.BettingRoundStage = 5;
						}
						num++;
						if (CasinoGames.BettingRoundStage == 5)
						{
							if (!CasinoGames.AI6_Bust && !CasinoGames.AI6_SkipTurn && !CasinoGames.AI6_HasPlayedBettingRoundMove)
							{
								bool foldCauseHighBet6 = CasinoGames.GetFoldCauseHighBet(CasinoGames.BettingRoundStartingBet, CasinoGames._8PlayerGamePoker_AI6Bank);
								bool foldCauseLowScore6 = CasinoGames.GetFoldCauseLowScore(CasinoGames.AI6Total);
								if (CasinoGames._8PlayerGamePoker_AI6Bank >= CasinoGames.BettingRoundStartingBet && !foldCauseHighBet6 && !foldCauseLowScore6)
								{
									text = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
									text2 = "bet_plus";
									CasinoGames.PlaySyncAnim(CasinoGames.AI_6, CasinoGames.AI_6, CasinoGames.AI6_SyncScene, text, text2);
									int num22 = random.Next(0, 100);
									int num23 = 0;
									float aI6Total = CasinoGames.AI6Total;
									if (aI6Total >= 0f && aI6Total < 50f)
									{
										num23 = 10;
									}
									if (aI6Total >= 50f && aI6Total < 200f)
									{
										num23 = 14;
									}
									if (aI6Total >= 100f && aI6Total < 200f)
									{
										num23 = 22;
									}
									if (aI6Total >= 200f && aI6Total < 300f)
									{
										num23 = 35;
									}
									if (aI6Total >= 300f && aI6Total < 400f)
									{
										num23 = 45;
									}
									if (aI6Total >= 400f && aI6Total < 700f)
									{
										num23 = 65;
									}
									if (aI6Total >= 700f && aI6Total < 1000f)
									{
										num23 = 60;
									}
									if (aI6Total >= 1000f && aI6Total < 2500f)
									{
										num23 = 70;
									}
									if (aI6Total >= 2500f)
									{
										num23 = 85;
									}
									if (num23 > num22)
									{
										float num24 = random.Next(100, 2500);
										if (CasinoGames._8PlayerGamePoker_AI6Bank - CasinoGames.BettingRoundStartingBet > 0f)
										{
											CasinoGames.BettingRoundStartingBet += num24;
											Notification.Show("AI 6 ~y~Raised~w~ the Bet");
										}
										else
										{
											Notification.Show("AI 6 ~y~Cannot Raise~w~ Bet, ~y~Called~ Bet!");
										}
									}
									else
									{
										Notification.Show("AI 6 ~y~Called~w~ the Bet");
									}
									CasinoGames._8PlayerGamePokerPot += CasinoGames.BettingRoundStartingBet;
									CasinoGames.RunBet(CasinoGames._8PlayerGamePokerPot);
									CasinoGames._8PlayerGamePoker_AI6Bank -= CasinoGames.BettingRoundStartingBet;
									if (CasinoGames.AI_6_VisualBank != null)
									{
										CasinoGames.AI_6_VisualBank.SpawnChips(CasinoGames._8PlayerGamePoker_AI6Bank);
									}
									CasinoGames.AI6_HasPlayedBettingRoundMove = true;
								}
								else
								{
									Ped aI_6 = CasinoGames.AI_6;
									CasinoGames.AI6_SkipTurn = true;
									text = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@play@v01";
									text2 = "check_cards_fold";
									CasinoGames.LoadDict(text);
									CasinoGames.PlaySyncAnim(CasinoGames.AI_6, CasinoGames.AI_6, CasinoGames.AI6_SyncScene, text, text2);
									try
									{
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[num].Card1, CasinoGames.AI6_SyncScene, "check_cards_fold_card_a", CasinoGames.LoadDict(text), 16f, -16f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[num].Card2, CasinoGames.AI6_SyncScene, "check_cards_fold_card_b", CasinoGames.LoadDict(text), 16f, -16f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[num].Card3, CasinoGames.AI6_SyncScene, "check_cards_fold_card_c", CasinoGames.LoadDict(text), 16f, -16f, 0, 1148846080);
									}
									catch
									{
									}
									int num25 = new Random().Next(0, 8);
									if (num25 == 0)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_6, "GENERIC_INSULT_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num25 == 1)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_6, "GENERIC_FUCK_YOU", "Speech_Params_Force_Shouted_Clear");
									}
									if (num25 == 2)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_6, "GENERIC_INSULT_HIGH", "Speech_Params_Force_Shouted_Clear");
									}
									if (num25 == 3)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_6, "GENERIC_SHOCKED_HIGH", "Speech_Params_Force_Shouted_Clear");
									}
									if (num25 == 4)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_6, "GENERIC_SHOCKED_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num25 == 5)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_6, "GENERIC_FRIGHTENED_HIGH", "Speech_Params_Force_Shouted_Clear");
									}
									if (num25 == 6)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_6, "GENERIC_FRIGHTENED_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num25 == 7)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_6, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num25 == 8)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_6, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (CasinoGames._8PlayerGamePoker_AI6Bank >= CasinoGames.BettingRoundStartingBet)
									{
										Notification.Show("AI 6 has ~y~Folded!");
									}
									else if (CasinoGames._8PlayerGamePoker_AI6Bank >= 1000f)
									{
										CasinoGames.AI6_SkipTurn = true;
										CasinoGames.AI6_Bust = true;
										Notification.Show("AI 6 has gone ~y~Bust!");
									}
									else
									{
										Notification.Show("AI 6 has ~y~Folded!");
									}
									CasinoGames.AI6_HasPlayedBettingRoundMove = true;
								}
							}
							if (CasinoGames.AI6_Bust || CasinoGames.AI6_SkipTurn)
							{
								CasinoGames.BettingRoundStage = 6;
							}
						}
						if (CasinoGames.AI6_HasPlayedBettingRoundMove && (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI6_SyncScene) >= 1065353216 || Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI6_SyncScene) == 0))
						{
							CasinoGames.AI6_HasPlayedBettingRoundMove = false;
							CasinoGames.BettingRoundStage = 6;
						}
						num++;
						if (CasinoGames.BettingRoundStage == 6)
						{
							if (!CasinoGames.AI7_Bust && !CasinoGames.AI7_SkipTurn && !CasinoGames.AI7_HasPlayedBettingRoundMove)
							{
								bool foldCauseHighBet7 = CasinoGames.GetFoldCauseHighBet(CasinoGames.BettingRoundStartingBet, CasinoGames._8PlayerGamePoker_AI7Bank);
								bool foldCauseLowScore7 = CasinoGames.GetFoldCauseLowScore(CasinoGames.AI7Total);
								if (CasinoGames._8PlayerGamePoker_AI7Bank >= CasinoGames.BettingRoundStartingBet && !foldCauseHighBet7 && !foldCauseLowScore7)
								{
									text = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
									text2 = "bet_plus";
									CasinoGames.PlaySyncAnim(CasinoGames.AI_7, CasinoGames.AI_7, CasinoGames.AI7_SyncScene, text, text2);
									int num26 = random.Next(0, 100);
									int num27 = 0;
									float aI7Total = CasinoGames.AI7Total;
									if (aI7Total >= 0f && aI7Total < 50f)
									{
										num27 = 10;
									}
									if (aI7Total >= 50f && aI7Total < 200f)
									{
										num27 = 14;
									}
									if (aI7Total >= 100f && aI7Total < 200f)
									{
										num27 = 22;
									}
									if (aI7Total >= 200f && aI7Total < 300f)
									{
										num27 = 35;
									}
									if (aI7Total >= 300f && aI7Total < 400f)
									{
										num27 = 45;
									}
									if (aI7Total >= 400f && aI7Total < 700f)
									{
										num27 = 65;
									}
									if (aI7Total >= 700f && aI7Total < 1000f)
									{
										num27 = 60;
									}
									if (aI7Total >= 1000f && aI7Total < 2500f)
									{
										num27 = 70;
									}
									if (aI7Total >= 2500f)
									{
										num27 = 85;
									}
									if (num27 > num26)
									{
										float num28 = random.Next(100, 2500);
										if (CasinoGames._8PlayerGamePoker_AI7Bank - CasinoGames.BettingRoundStartingBet > 0f)
										{
											CasinoGames.BettingRoundStartingBet += num28;
											Notification.Show("AI 7 ~y~Raised~w~ the Bet");
										}
										else
										{
											Notification.Show("AI 7 ~y~Cannot Raise~w~ Bet, ~y~Called~ Bet!");
										}
									}
									else
									{
										Notification.Show("AI 7 ~y~Called~w~ the Bet");
									}
									CasinoGames._8PlayerGamePokerPot += CasinoGames.BettingRoundStartingBet;
									CasinoGames.RunBet(CasinoGames._8PlayerGamePokerPot);
									CasinoGames._8PlayerGamePoker_AI7Bank -= CasinoGames.BettingRoundStartingBet;
									if (CasinoGames.AI_7_VisualBank != null)
									{
										CasinoGames.AI_7_VisualBank.SpawnChips(CasinoGames._8PlayerGamePoker_AI7Bank);
									}
									CasinoGames.AI7_HasPlayedBettingRoundMove = true;
								}
								else
								{
									Ped aI_7 = CasinoGames.AI_7;
									CasinoGames.AI7_SkipTurn = true;
									text = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@play@v01";
									text2 = "check_cards_fold";
									CasinoGames.LoadDict(text);
									CasinoGames.PlaySyncAnim(CasinoGames.AI_7, CasinoGames.AI_7, CasinoGames.AI7_SyncScene, text, text2);
									try
									{
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[num].Card1, CasinoGames.AI7_SyncScene, "check_cards_fold_card_a", CasinoGames.LoadDict(text), 16f, -16f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[num].Card2, CasinoGames.AI7_SyncScene, "check_cards_fold_card_b", CasinoGames.LoadDict(text), 16f, -16f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[num].Card3, CasinoGames.AI7_SyncScene, "check_cards_fold_card_c", CasinoGames.LoadDict(text), 16f, -16f, 0, 1148846080);
									}
									catch
									{
									}
									int num29 = new Random().Next(0, 8);
									if (num29 == 0)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_7, "GENERIC_INSULT_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num29 == 1)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_7, "GENERIC_FUCK_YOU", "Speech_Params_Force_Shouted_Clear");
									}
									if (num29 == 2)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_7, "GENERIC_INSULT_HIGH", "Speech_Params_Force_Shouted_Clear");
									}
									if (num29 == 3)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_7, "GENERIC_SHOCKED_HIGH", "Speech_Params_Force_Shouted_Clear");
									}
									if (num29 == 4)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_7, "GENERIC_SHOCKED_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num29 == 5)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_7, "GENERIC_FRIGHTENED_HIGH", "Speech_Params_Force_Shouted_Clear");
									}
									if (num29 == 6)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_7, "GENERIC_FRIGHTENED_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num29 == 7)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_7, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (num29 == 8)
									{
										Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, aI_7, "GENERIC_CURSE_MED", "Speech_Params_Force_Shouted_Clear");
									}
									if (CasinoGames._8PlayerGamePoker_AI7Bank >= CasinoGames.BettingRoundStartingBet)
									{
										Notification.Show("AI 7 has ~y~Folded!");
									}
									else if (CasinoGames._8PlayerGamePoker_AI7Bank >= 1000f)
									{
										CasinoGames.AI7_SkipTurn = true;
										CasinoGames.AI7_Bust = true;
										Notification.Show("AI 7 has gone ~y~Bust!");
									}
									else
									{
										Notification.Show("AI 7 has ~y~Folded!");
									}
									CasinoGames.AI7_HasPlayedBettingRoundMove = true;
								}
							}
							if (CasinoGames.AI7_Bust || CasinoGames.AI7_SkipTurn)
							{
								CasinoGames.BettingRoundStage = 7;
							}
						}
						if (CasinoGames.AI7_HasPlayedBettingRoundMove && (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI7_SyncScene) >= 1065353216 || Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI7_SyncScene) == 0))
						{
							CasinoGames.AI7_HasPlayedBettingRoundMove = false;
							CasinoGames.BettingRoundStage = 7;
						}
						num++;
						if (CasinoGames.BettingRoundStage == 7 && !CasinoGames.AI1_HasPlayedBettingRoundMove && !CasinoGames.AI2_HasPlayedBettingRoundMove && !CasinoGames.AI3_HasPlayedBettingRoundMove && !CasinoGames.AI4_HasPlayedBettingRoundMove && !CasinoGames.AI5_HasPlayedBettingRoundMove && !CasinoGames.AI6_HasPlayedBettingRoundMove && !CasinoGames.AI7_HasPlayedBettingRoundMove)
						{
							CasinoGames.LastRaiseAmmount = CasinoGames.BettingRoundStartingBet;
							CasinoGames.AreBetsBeingPlaced = false;
							CasinoGames.BettingRoundStage = 0;
							CasinoGames.AI1_HasPlayedBettingRoundMove = false;
							CasinoGames.AI2_HasPlayedBettingRoundMove = false;
							CasinoGames.AI3_HasPlayedBettingRoundMove = false;
							CasinoGames.AI4_HasPlayedBettingRoundMove = false;
							CasinoGames.AI5_HasPlayedBettingRoundMove = false;
							CasinoGames.AI6_HasPlayedBettingRoundMove = false;
							CasinoGames.AI7_HasPlayedBettingRoundMove = false;
							CasinoGames._8PlayerGameStage = 2;
						}
					}
					if (CasinoGames._8PlayerGameStage == 3 || CasinoGames._8PlayerGameStage == 2)
					{
						if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Player_SyncScene) >= 1065353216 || Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Player_SyncScene) == 0)
						{
							if (CasinoGames.Player_IdleScenesPlayed == 0)
							{
								if (Game.Player.Character.Gender == Gender.Male)
								{
									new Random();
									List<string> list = new List<string>();
									list.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@engaged@01a@idles");
									list.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@engaged@01b@idles");
									list.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01a@idles");
									list.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@idles");
									list.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@idles");
									list.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02b@idles");
									List<string> list2 = new List<string>();
									list2.Add("idle_a");
									list2.Add("idle_b");
									list2.Add("idle_c");
									list2.Add("idle_d");
									Prop _8PlayerPokerGameTable = CasinoGames._8PlayerPokerGameTable1;
									string dict = list[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list.Count)];
									string text3 = list2[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list2.Count)];
									CasinoGames.Player_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, _8PlayerPokerGameTable.Position.Z, 0.0, 0.0, Game.Player.Character.Heading, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Player_SyncScene, CasinoGames.LoadDict(dict), text3, 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Player_SyncScene, text3, CasinoGames.LoadDict(dict), 16f, -16f, 0, 1148846080);
								}
								if (Game.Player.Character.Gender == Gender.Female)
								{
									new Random();
									List<string> list3 = new List<string>();
									list3.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@idles");
									list3.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01b@idles");
									list3.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@01a@idles");
									list3.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@01b@idles");
									list3.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@02a@idles");
									list3.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@02b@idles");
									List<string> list4 = new List<string>();
									list4.Add("idle_a");
									list4.Add("idle_b");
									list4.Add("idle_c");
									list4.Add("idle_d");
									Prop _8PlayerPokerGameTable2 = CasinoGames._8PlayerPokerGameTable1;
									string dict2 = list3[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list3.Count)];
									string text4 = list4[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list4.Count)];
									CasinoGames.Player_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, _8PlayerPokerGameTable2.Position.Z, 0.0, 0.0, Game.Player.Character.Heading, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Player_SyncScene, CasinoGames.LoadDict(dict2), text4, 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Player_SyncScene, text4, CasinoGames.LoadDict(dict2), 16f, -16f, 0, 1148846080);
								}
							}
							else
							{
								new Random();
								if (Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100) <= 30)
								{
									Ped character = Game.Player.Character;
									string text5 = "anim_casino_b@amb@casino@games@threecardpoker@dealer";
									text5 = ((character.Gender != Gender.Female) ? "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v01" : "anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@01b@play@v01");
									string anim = "check_cards_fold";
									CasinoGames.LoadDict(text5);
									CasinoGames.Player_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, character.Position.X, character.Position.Y, character.Position.Z, 0.0, 0.0, character.Rotation.Z, 2);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Player_SyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Player_SyncScene));
									CasinoGames.PlaySyncAnim(character, character, CasinoGames.Player_SyncScene, text5, anim);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCard1, CasinoGames.Player_SyncScene, "check_cards_fold_card_a", CasinoGames.LoadDict(text5), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCard2, CasinoGames.Player_SyncScene, "check_cards_fold_card_b", CasinoGames.LoadDict(text5), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCard3, CasinoGames.Player_SyncScene, "check_cards_fold_card_c", CasinoGames.LoadDict(text5), 16f, -16f, 0, 1148846080);
								}
								else
								{
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Player_SyncScene, 1);
									CasinoGames.Player_IdleScenesPlayed++;
									if (CasinoGames.Player_IdleScenesPlayed >= CasinoGames.Player_IdleScenesToPlayBeforeChange)
									{
										CasinoGames.Player_IdleScenesPlayed = 0;
									}
								}
							}
						}
						if (!CasinoGames.AI1_HasPlayedBettingRoundMove && (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI1_SyncScene) >= 1065353216 || Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI1_SyncScene) == 0))
						{
							if (CasinoGames.AI1_IdleScenesPlayed == 0)
							{
								if (CasinoGames.AI_1.Gender == Gender.Male)
								{
									new Random();
									List<string> list5 = new List<string>();
									list5.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@engaged@01a@idles");
									list5.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@engaged@01b@idles");
									list5.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01a@idles");
									list5.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@idles");
									list5.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@idles");
									list5.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02b@idles");
									List<string> list6 = new List<string>();
									list6.Add("idle_a");
									list6.Add("idle_b");
									list6.Add("idle_c");
									list6.Add("idle_d");
									Prop _8PlayerPokerGameTable3 = CasinoGames._8PlayerPokerGameTable1;
									string dict3 = list5[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list5.Count)];
									string text6 = list6[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list6.Count)];
									CasinoGames.AI1_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.AI_1.Position.X, CasinoGames.AI_1.Position.Y, _8PlayerPokerGameTable3.Position.Z, 0.0, 0.0, CasinoGames.AI_1.Heading, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.AI_1, CasinoGames.AI1_SyncScene, CasinoGames.LoadDict(dict3), text6, 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.AI_1, CasinoGames.AI1_SyncScene, text6, CasinoGames.LoadDict(dict3), 16f, -16f, 0, 1148846080);
								}
								if (CasinoGames.AI_1.Gender == Gender.Female)
								{
									new Random();
									List<string> list7 = new List<string>();
									list7.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@idles");
									list7.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01b@idles");
									list7.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@01a@idles");
									list7.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@01b@idles");
									list7.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@02a@idles");
									list7.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@02b@idles");
									List<string> list8 = new List<string>();
									list8.Add("idle_a");
									list8.Add("idle_b");
									list8.Add("idle_c");
									list8.Add("idle_d");
									Prop _8PlayerPokerGameTable4 = CasinoGames._8PlayerPokerGameTable1;
									string dict4 = list7[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list7.Count)];
									string text7 = list8[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list8.Count)];
									CasinoGames.AI1_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.AI_1.Position.X, CasinoGames.AI_1.Position.Y, _8PlayerPokerGameTable4.Position.Z, 0.0, 0.0, CasinoGames.AI_1.Heading, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.AI_1, CasinoGames.AI1_SyncScene, CasinoGames.LoadDict(dict4), text7, 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.AI_1, CasinoGames.AI1_SyncScene, text7, CasinoGames.LoadDict(dict4), 16f, -16f, 0, 1148846080);
								}
							}
							else
							{
								new Random();
								if (Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100) <= 30)
								{
									Ped aI_8 = CasinoGames.AI_1;
									string text8 = "";
									text8 = ((aI_8.Gender != Gender.Female) ? "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v01" : "anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@01b@play@v01");
									string anim2 = "check_cards_fold";
									CasinoGames.LoadDict(text8);
									CasinoGames.AI1_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, aI_8.Position.X, aI_8.Position.Y, aI_8.Position.Z, 0.0, 0.0, aI_8.Rotation.Z, 2);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI1_SyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI1_SyncScene));
									CasinoGames.PlaySyncAnim(aI_8, aI_8, CasinoGames.AI1_SyncScene, text8, anim2);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[0].Card1, CasinoGames.AI1_SyncScene, "check_cards_fold_card_a", CasinoGames.LoadDict(text8), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[0].Card2, CasinoGames.AI1_SyncScene, "check_cards_fold_card_b", CasinoGames.LoadDict(text8), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[0].Card3, CasinoGames.AI1_SyncScene, "check_cards_fold_card_c", CasinoGames.LoadDict(text8), 16f, -16f, 0, 1148846080);
								}
								else
								{
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI1_SyncScene, 1);
									CasinoGames.AI1_IdleScenesPlayed++;
									if (CasinoGames.AI1_IdleScenesPlayed >= CasinoGames.AI1_IdleScenesToPlayBeforeChange)
									{
										CasinoGames.AI1_IdleScenesPlayed = 0;
									}
								}
							}
						}
						if (!CasinoGames.AI2_HasPlayedBettingRoundMove && (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI2_SyncScene) >= 1065353216 || Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI2_SyncScene) == 0))
						{
							if (CasinoGames.AI2_IdleScenesPlayed == 0)
							{
								if (CasinoGames.AI_2.Gender == Gender.Male)
								{
									new Random();
									List<string> list9 = new List<string>();
									list9.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@engaged@01a@idles");
									list9.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@engaged@01b@idles");
									list9.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01a@idles");
									list9.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@idles");
									list9.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@idles");
									list9.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02b@idles");
									List<string> list10 = new List<string>();
									list10.Add("idle_a");
									list10.Add("idle_b");
									list10.Add("idle_c");
									list10.Add("idle_d");
									Prop _8PlayerPokerGameTable5 = CasinoGames._8PlayerPokerGameTable1;
									string dict5 = list9[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list9.Count)];
									string text9 = list10[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list10.Count)];
									CasinoGames.AI2_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.AI_2.Position.X, CasinoGames.AI_2.Position.Y, _8PlayerPokerGameTable5.Position.Z, 0.0, 0.0, CasinoGames.AI_2.Heading, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.AI_2, CasinoGames.AI2_SyncScene, CasinoGames.LoadDict(dict5), text9, 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.AI_2, CasinoGames.AI2_SyncScene, text9, CasinoGames.LoadDict(dict5), 16f, -16f, 0, 1148846080);
								}
								if (CasinoGames.AI_2.Gender == Gender.Female)
								{
									new Random();
									List<string> list11 = new List<string>();
									list11.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@idles");
									list11.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01b@idles");
									list11.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@01a@idles");
									list11.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@01b@idles");
									list11.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@02a@idles");
									list11.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@02b@idles");
									List<string> list12 = new List<string>();
									list12.Add("idle_a");
									list12.Add("idle_b");
									list12.Add("idle_c");
									list12.Add("idle_d");
									Prop _8PlayerPokerGameTable6 = CasinoGames._8PlayerPokerGameTable1;
									string dict6 = list11[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list11.Count)];
									string text10 = list12[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list12.Count)];
									CasinoGames.AI2_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.AI_2.Position.X, CasinoGames.AI_2.Position.Y, _8PlayerPokerGameTable6.Position.Z, 0.0, 0.0, CasinoGames.AI_2.Heading, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.AI_2, CasinoGames.AI2_SyncScene, CasinoGames.LoadDict(dict6), text10, 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.AI_2, CasinoGames.AI2_SyncScene, text10, CasinoGames.LoadDict(dict6), 16f, -16f, 0, 1148846080);
								}
							}
							else
							{
								new Random();
								if (Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100) <= 30)
								{
									Ped aI_9 = CasinoGames.AI_2;
									string text11 = "";
									text11 = ((aI_9.Gender != Gender.Female) ? "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v01" : "anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@01b@play@v01");
									string anim3 = "check_cards_fold";
									CasinoGames.LoadDict(text11);
									CasinoGames.AI2_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, aI_9.Position.X, aI_9.Position.Y, aI_9.Position.Z, 0.0, 0.0, aI_9.Rotation.Z, 2);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI2_SyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI2_SyncScene));
									CasinoGames.PlaySyncAnim(aI_9, aI_9, CasinoGames.AI2_SyncScene, text11, anim3);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[1].Card1, CasinoGames.AI2_SyncScene, "check_cards_fold_card_a", CasinoGames.LoadDict(text11), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[1].Card2, CasinoGames.AI2_SyncScene, "check_cards_fold_card_b", CasinoGames.LoadDict(text11), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[1].Card3, CasinoGames.AI2_SyncScene, "check_cards_fold_card_c", CasinoGames.LoadDict(text11), 16f, -16f, 0, 1148846080);
								}
								else
								{
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI2_SyncScene, 1);
									CasinoGames.AI2_IdleScenesPlayed++;
									if (CasinoGames.AI2_IdleScenesPlayed >= CasinoGames.AI2_IdleScenesToPlayBeforeChange)
									{
										CasinoGames.AI2_IdleScenesPlayed = 0;
									}
								}
							}
						}
						if (!CasinoGames.AI3_HasPlayedBettingRoundMove && (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI3_SyncScene) >= 1065353216 || Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI3_SyncScene) == 0))
						{
							if (CasinoGames.AI3_IdleScenesPlayed == 0)
							{
								if (CasinoGames.AI_3.Gender == Gender.Male)
								{
									new Random();
									List<string> list13 = new List<string>();
									list13.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@engaged@01a@idles");
									list13.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@engaged@01b@idles");
									list13.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01a@idles");
									list13.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@idles");
									list13.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@idles");
									list13.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02b@idles");
									List<string> list14 = new List<string>();
									list14.Add("idle_a");
									list14.Add("idle_b");
									list14.Add("idle_c");
									list14.Add("idle_d");
									Prop _8PlayerPokerGameTable7 = CasinoGames._8PlayerPokerGameTable1;
									string dict7 = list13[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list13.Count)];
									string text12 = list14[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list14.Count)];
									CasinoGames.AI3_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.AI_3.Position.X, CasinoGames.AI_3.Position.Y, _8PlayerPokerGameTable7.Position.Z, 0.0, 0.0, CasinoGames.AI_3.Heading, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.AI_3, CasinoGames.AI3_SyncScene, CasinoGames.LoadDict(dict7), text12, 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.AI_3, CasinoGames.AI3_SyncScene, text12, CasinoGames.LoadDict(dict7), 16f, -16f, 0, 1148846080);
								}
								if (CasinoGames.AI_3.Gender == Gender.Female)
								{
									new Random();
									List<string> list15 = new List<string>();
									list15.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@idles");
									list15.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01b@idles");
									list15.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@01a@idles");
									list15.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@01b@idles");
									list15.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@02a@idles");
									list15.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@02b@idles");
									List<string> list16 = new List<string>();
									list16.Add("idle_a");
									list16.Add("idle_b");
									list16.Add("idle_c");
									list16.Add("idle_d");
									Prop _8PlayerPokerGameTable8 = CasinoGames._8PlayerPokerGameTable1;
									string dict8 = list15[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list15.Count)];
									string text13 = list16[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list16.Count)];
									CasinoGames.AI3_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.AI_3.Position.X, CasinoGames.AI_3.Position.Y, _8PlayerPokerGameTable8.Position.Z, 0.0, 0.0, CasinoGames.AI_3.Heading, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.AI_3, CasinoGames.AI3_SyncScene, CasinoGames.LoadDict(dict8), text13, 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.AI_3, CasinoGames.AI3_SyncScene, text13, CasinoGames.LoadDict(dict8), 16f, -16f, 0, 1148846080);
								}
							}
							else
							{
								new Random();
								if (Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100) <= 30)
								{
									Ped aI_10 = CasinoGames.AI_3;
									string text14 = "";
									text14 = ((aI_10.Gender != Gender.Female) ? "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v01" : "anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@01b@play@v01");
									string anim4 = "check_cards_fold";
									CasinoGames.LoadDict(text14);
									CasinoGames.AI3_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, aI_10.Position.X, aI_10.Position.Y, aI_10.Position.Z, 0.0, 0.0, aI_10.Rotation.Z, 2);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI3_SyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI3_SyncScene));
									CasinoGames.PlaySyncAnim(aI_10, aI_10, CasinoGames.AI3_SyncScene, text14, anim4);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[2].Card1, CasinoGames.AI3_SyncScene, "check_cards_fold_card_a", CasinoGames.LoadDict(text14), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[2].Card2, CasinoGames.AI3_SyncScene, "check_cards_fold_card_b", CasinoGames.LoadDict(text14), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[2].Card3, CasinoGames.AI3_SyncScene, "check_cards_fold_card_c", CasinoGames.LoadDict(text14), 16f, -16f, 0, 1148846080);
								}
								else
								{
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI3_SyncScene, 1);
									CasinoGames.AI3_IdleScenesPlayed++;
									if (CasinoGames.AI3_IdleScenesPlayed >= CasinoGames.AI3_IdleScenesToPlayBeforeChange)
									{
										CasinoGames.AI3_IdleScenesPlayed = 0;
									}
								}
							}
						}
						if (!CasinoGames.AI4_HasPlayedBettingRoundMove && (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI4_SyncScene) >= 1065353216 || Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI4_SyncScene) == 0))
						{
							if (CasinoGames.AI4_IdleScenesPlayed == 0)
							{
								if (CasinoGames.AI_4.Gender == Gender.Male)
								{
									new Random();
									List<string> list17 = new List<string>();
									list17.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@engaged@01a@idles");
									list17.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@engaged@01b@idles");
									list17.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01a@idles");
									list17.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@idles");
									list17.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@idles");
									list17.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02b@idles");
									List<string> list18 = new List<string>();
									list18.Add("idle_a");
									list18.Add("idle_b");
									list18.Add("idle_c");
									list18.Add("idle_d");
									Prop _8PlayerPokerGameTable9 = CasinoGames._8PlayerPokerGameTable1;
									string dict9 = list17[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list17.Count)];
									string text15 = list18[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list18.Count)];
									CasinoGames.AI4_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.AI_4.Position.X, CasinoGames.AI_4.Position.Y, _8PlayerPokerGameTable9.Position.Z, 0.0, 0.0, CasinoGames.AI_4.Heading, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.AI_4, CasinoGames.AI4_SyncScene, CasinoGames.LoadDict(dict9), text15, 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.AI_4, CasinoGames.AI4_SyncScene, text15, CasinoGames.LoadDict(dict9), 16f, -16f, 0, 1148846080);
								}
								if (CasinoGames.AI_4.Gender == Gender.Female)
								{
									new Random();
									List<string> list19 = new List<string>();
									list19.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@idles");
									list19.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01b@idles");
									list19.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@01a@idles");
									list19.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@01b@idles");
									list19.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@02a@idles");
									list19.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@02b@idles");
									List<string> list20 = new List<string>();
									list20.Add("idle_a");
									list20.Add("idle_b");
									list20.Add("idle_c");
									list20.Add("idle_d");
									Prop _8PlayerPokerGameTable10 = CasinoGames._8PlayerPokerGameTable1;
									string dict10 = list19[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list19.Count)];
									string text16 = list20[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list20.Count)];
									CasinoGames.AI4_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.AI_4.Position.X, CasinoGames.AI_4.Position.Y, _8PlayerPokerGameTable10.Position.Z, 0.0, 0.0, CasinoGames.AI_4.Heading, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.AI_4, CasinoGames.AI4_SyncScene, CasinoGames.LoadDict(dict10), text16, 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.AI_4, CasinoGames.AI4_SyncScene, text16, CasinoGames.LoadDict(dict10), 16f, -16f, 0, 1148846080);
								}
							}
							else
							{
								new Random();
								if (Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100) <= 30)
								{
									Ped aI_11 = CasinoGames.AI_4;
									string text17 = "";
									text17 = ((aI_11.Gender != Gender.Female) ? "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v01" : "anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@01b@play@v01");
									string anim5 = "check_cards_fold";
									CasinoGames.LoadDict(text17);
									CasinoGames.AI4_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, aI_11.Position.X, aI_11.Position.Y, aI_11.Position.Z, 0.0, 0.0, aI_11.Rotation.Z, 2);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI4_SyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI4_SyncScene));
									CasinoGames.PlaySyncAnim(aI_11, aI_11, CasinoGames.AI4_SyncScene, text17, anim5);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[3].Card1, CasinoGames.AI4_SyncScene, "check_cards_fold_card_a", CasinoGames.LoadDict(text17), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[3].Card2, CasinoGames.AI4_SyncScene, "check_cards_fold_card_b", CasinoGames.LoadDict(text17), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[3].Card3, CasinoGames.AI4_SyncScene, "check_cards_fold_card_c", CasinoGames.LoadDict(text17), 16f, -16f, 0, 1148846080);
								}
								else
								{
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI4_SyncScene, 1);
									CasinoGames.AI4_IdleScenesPlayed++;
									if (CasinoGames.AI4_IdleScenesPlayed >= CasinoGames.AI4_IdleScenesToPlayBeforeChange)
									{
										CasinoGames.AI4_IdleScenesPlayed = 0;
									}
								}
							}
						}
						if (!CasinoGames.AI5_HasPlayedBettingRoundMove && (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI5_SyncScene) >= 1065353216 || Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI5_SyncScene) == 0))
						{
							if (CasinoGames.AI5_IdleScenesPlayed == 0)
							{
								if (CasinoGames.AI_5.Gender == Gender.Male)
								{
									new Random();
									List<string> list21 = new List<string>();
									list21.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@engaged@01a@idles");
									list21.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@engaged@01b@idles");
									list21.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01a@idles");
									list21.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@idles");
									list21.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@idles");
									list21.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02b@idles");
									List<string> list22 = new List<string>();
									list22.Add("idle_a");
									list22.Add("idle_b");
									list22.Add("idle_c");
									list22.Add("idle_d");
									Prop _8PlayerPokerGameTable11 = CasinoGames._8PlayerPokerGameTable1;
									string dict11 = list21[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list21.Count)];
									string text18 = list22[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list22.Count)];
									CasinoGames.AI5_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.AI_5.Position.X, CasinoGames.AI_5.Position.Y, _8PlayerPokerGameTable11.Position.Z, 0.0, 0.0, CasinoGames.AI_5.Heading, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.AI_5, CasinoGames.AI5_SyncScene, CasinoGames.LoadDict(dict11), text18, 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.AI_5, CasinoGames.AI5_SyncScene, text18, CasinoGames.LoadDict(dict11), 16f, -16f, 0, 1148846080);
								}
								if (CasinoGames.AI_5.Gender == Gender.Female)
								{
									new Random();
									List<string> list23 = new List<string>();
									list23.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@idles");
									list23.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01b@idles");
									list23.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@01a@idles");
									list23.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@01b@idles");
									list23.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@02a@idles");
									list23.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@02b@idles");
									List<string> list24 = new List<string>();
									list24.Add("idle_a");
									list24.Add("idle_b");
									list24.Add("idle_c");
									list24.Add("idle_d");
									Prop _8PlayerPokerGameTable12 = CasinoGames._8PlayerPokerGameTable1;
									string dict12 = list23[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list23.Count)];
									string text19 = list24[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list24.Count)];
									CasinoGames.AI5_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.AI_5.Position.X, CasinoGames.AI_5.Position.Y, _8PlayerPokerGameTable12.Position.Z, 0.0, 0.0, CasinoGames.AI_5.Heading, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.AI_5, CasinoGames.AI5_SyncScene, CasinoGames.LoadDict(dict12), text19, 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.AI_5, CasinoGames.AI5_SyncScene, text19, CasinoGames.LoadDict(dict12), 16f, -16f, 0, 1148846080);
								}
							}
							else
							{
								new Random();
								if (Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100) <= 30)
								{
									Ped aI_12 = CasinoGames.AI_5;
									string text20 = "";
									text20 = ((aI_12.Gender != Gender.Female) ? "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v01" : "anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@01b@play@v01");
									string anim6 = "check_cards_fold";
									CasinoGames.LoadDict(text20);
									CasinoGames.AI5_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, aI_12.Position.X, aI_12.Position.Y, aI_12.Position.Z, 0.0, 0.0, aI_12.Rotation.Z, 2);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI5_SyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI5_SyncScene));
									CasinoGames.PlaySyncAnim(aI_12, aI_12, CasinoGames.AI5_SyncScene, text20, anim6);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[4].Card1, CasinoGames.AI5_SyncScene, "check_cards_fold_card_a", CasinoGames.LoadDict(text20), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[4].Card2, CasinoGames.AI5_SyncScene, "check_cards_fold_card_b", CasinoGames.LoadDict(text20), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[4].Card3, CasinoGames.AI5_SyncScene, "check_cards_fold_card_c", CasinoGames.LoadDict(text20), 16f, -16f, 0, 1148846080);
								}
								else
								{
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI5_SyncScene, 1);
									CasinoGames.AI5_IdleScenesPlayed++;
									if (CasinoGames.AI5_IdleScenesPlayed >= CasinoGames.AI5_IdleScenesToPlayBeforeChange)
									{
										CasinoGames.AI5_IdleScenesPlayed = 0;
									}
								}
							}
						}
						if (!CasinoGames.AI6_HasPlayedBettingRoundMove && (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI6_SyncScene) >= 1065353216 || Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI6_SyncScene) == 0))
						{
							if (CasinoGames.AI6_IdleScenesPlayed == 0)
							{
								if (CasinoGames.AI_6.Gender == Gender.Male)
								{
									new Random();
									List<string> list25 = new List<string>();
									list25.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@engaged@01a@idles");
									list25.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@engaged@01b@idles");
									list25.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01a@idles");
									list25.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@idles");
									list25.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@idles");
									list25.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02b@idles");
									List<string> list26 = new List<string>();
									list26.Add("idle_a");
									list26.Add("idle_b");
									list26.Add("idle_c");
									list26.Add("idle_d");
									Prop _8PlayerPokerGameTable13 = CasinoGames._8PlayerPokerGameTable1;
									string dict13 = list25[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list25.Count)];
									string text21 = list26[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list26.Count)];
									CasinoGames.AI6_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.AI_6.Position.X, CasinoGames.AI_6.Position.Y, _8PlayerPokerGameTable13.Position.Z, 0.0, 0.0, CasinoGames.AI_6.Heading, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.AI_6, CasinoGames.AI6_SyncScene, CasinoGames.LoadDict(dict13), text21, 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.AI_6, CasinoGames.AI6_SyncScene, text21, CasinoGames.LoadDict(dict13), 16f, -16f, 0, 1148846080);
								}
								if (CasinoGames.AI_6.Gender == Gender.Female)
								{
									new Random();
									List<string> list27 = new List<string>();
									list27.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@idles");
									list27.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01b@idles");
									list27.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@01a@idles");
									list27.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@01b@idles");
									list27.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@02a@idles");
									list27.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@02b@idles");
									List<string> list28 = new List<string>();
									list28.Add("idle_a");
									list28.Add("idle_b");
									list28.Add("idle_c");
									list28.Add("idle_d");
									Prop _8PlayerPokerGameTable14 = CasinoGames._8PlayerPokerGameTable1;
									string dict14 = list27[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list27.Count)];
									string text22 = list28[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list28.Count)];
									CasinoGames.AI6_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.AI_6.Position.X, CasinoGames.AI_6.Position.Y, _8PlayerPokerGameTable14.Position.Z, 0.0, 0.0, CasinoGames.AI_6.Heading, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.AI_6, CasinoGames.AI6_SyncScene, CasinoGames.LoadDict(dict14), text22, 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.AI_6, CasinoGames.AI6_SyncScene, text22, CasinoGames.LoadDict(dict14), 16f, -16f, 0, 1148846080);
								}
							}
							else
							{
								new Random();
								if (Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100) <= 30)
								{
									Ped aI_13 = CasinoGames.AI_6;
									string text23 = "";
									text23 = ((aI_13.Gender != Gender.Female) ? "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v01" : "anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@01b@play@v01");
									string anim7 = "check_cards_fold";
									CasinoGames.LoadDict(text23);
									CasinoGames.AI6_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, aI_13.Position.X, aI_13.Position.Y, aI_13.Position.Z, 0.0, 0.0, aI_13.Rotation.Z, 2);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI6_SyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI6_SyncScene));
									CasinoGames.PlaySyncAnim(aI_13, aI_13, CasinoGames.AI6_SyncScene, text23, anim7);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[5].Card1, CasinoGames.AI6_SyncScene, "check_cards_fold_card_a", CasinoGames.LoadDict(text23), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[5].Card2, CasinoGames.AI6_SyncScene, "check_cards_fold_card_b", CasinoGames.LoadDict(text23), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[5].Card3, CasinoGames.AI6_SyncScene, "check_cards_fold_card_c", CasinoGames.LoadDict(text23), 16f, -16f, 0, 1148846080);
								}
								else
								{
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI6_SyncScene, 1);
									CasinoGames.AI6_IdleScenesPlayed++;
									if (CasinoGames.AI6_IdleScenesPlayed >= CasinoGames.AI6_IdleScenesToPlayBeforeChange)
									{
										CasinoGames.AI6_IdleScenesPlayed = 0;
									}
								}
							}
						}
						if (!CasinoGames.AI7_HasPlayedBettingRoundMove && (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI7_SyncScene) >= 1065353216 || Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI7_SyncScene) == 0))
						{
							if (CasinoGames.AI7_IdleScenesPlayed == 0)
							{
								if (CasinoGames.AI_7.Gender == Gender.Male)
								{
									new Random();
									List<string> list29 = new List<string>();
									list29.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@engaged@01a@idles");
									list29.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@engaged@01b@idles");
									list29.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01a@idles");
									list29.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@idles");
									list29.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@idles");
									list29.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02b@idles");
									List<string> list30 = new List<string>();
									list30.Add("idle_a");
									list30.Add("idle_b");
									list30.Add("idle_c");
									list30.Add("idle_d");
									Prop _8PlayerPokerGameTable15 = CasinoGames._8PlayerPokerGameTable1;
									string dict15 = list29[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list29.Count)];
									string text24 = list30[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list30.Count)];
									CasinoGames.AI7_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.AI_7.Position.X, CasinoGames.AI_7.Position.Y, _8PlayerPokerGameTable15.Position.Z, 0.0, 0.0, CasinoGames.AI_7.Heading, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.AI_7, CasinoGames.AI7_SyncScene, CasinoGames.LoadDict(dict15), text24, 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.AI_7, CasinoGames.AI7_SyncScene, text24, CasinoGames.LoadDict(dict15), 16f, -16f, 0, 1148846080);
								}
								if (CasinoGames.AI_7.Gender == Gender.Female)
								{
									new Random();
									List<string> list31 = new List<string>();
									list31.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@idles");
									list31.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01b@idles");
									list31.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@01a@idles");
									list31.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@01b@idles");
									list31.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@02a@idles");
									list31.Add("anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@02b@idles");
									List<string> list32 = new List<string>();
									list32.Add("idle_a");
									list32.Add("idle_b");
									list32.Add("idle_c");
									list32.Add("idle_d");
									Prop _8PlayerPokerGameTable16 = CasinoGames._8PlayerPokerGameTable1;
									string dict16 = list31[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list31.Count)];
									string text25 = list32[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list32.Count)];
									CasinoGames.AI7_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.AI_7.Position.X, CasinoGames.AI_7.Position.Y, _8PlayerPokerGameTable16.Position.Z, 0.0, 0.0, CasinoGames.AI_7.Heading, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.AI_7, CasinoGames.AI7_SyncScene, CasinoGames.LoadDict(dict16), text25, 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.AI_7, CasinoGames.AI7_SyncScene, text25, CasinoGames.LoadDict(dict16), 16f, -16f, 0, 1148846080);
								}
							}
							else
							{
								new Random();
								if (Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100) <= 30)
								{
									Ped aI_14 = CasinoGames.AI_7;
									string text26 = "";
									text26 = ((CasinoGames.AI_7.Gender != Gender.Female) ? "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v01" : "anim_casino_b@amb@casino@games@threecardpoker@ped_female@regular@01b@play@v01");
									string anim8 = "check_cards_fold";
									CasinoGames.LoadDict(text26);
									CasinoGames.AI7_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, aI_14.Position.X, aI_14.Position.Y, aI_14.Position.Z, 0.0, 0.0, aI_14.Rotation.Z, 2);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI7_SyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI7_SyncScene));
									CasinoGames.PlaySyncAnim(aI_14, aI_14, CasinoGames.AI7_SyncScene, text26, anim8);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[6].Card1, CasinoGames.AI7_SyncScene, "check_cards_fold_card_a", CasinoGames.LoadDict(text26), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[6].Card2, CasinoGames.AI7_SyncScene, "check_cards_fold_card_b", CasinoGames.LoadDict(text26), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[6].Card3, CasinoGames.AI7_SyncScene, "check_cards_fold_card_c", CasinoGames.LoadDict(text26), 16f, -16f, 0, 1148846080);
									CasinoGames.AI7_IdleScenesPlayed = 0;
								}
								else
								{
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.AI7_SyncScene, 1);
									CasinoGames.AI7_IdleScenesPlayed++;
									if (CasinoGames.AI7_IdleScenesPlayed >= CasinoGames.AI7_IdleScenesToPlayBeforeChange)
									{
										CasinoGames.AI7_IdleScenesPlayed = 0;
									}
								}
							}
						}
					}
					if (CasinoGames._8PlayerGameStage > 1)
					{
						if (CasinoGames._8PlayerGamePoker_PlayerBank <= 0f)
						{
							CasinoGames.Player_Bust = true;
						}
						if (CasinoGames._8PlayerGamePoker_AI1Bank <= 0f)
						{
							CasinoGames.AI1_Bust = true;
						}
						if (CasinoGames._8PlayerGamePoker_AI2Bank <= 0f)
						{
							CasinoGames.AI2_Bust = true;
						}
						if (CasinoGames._8PlayerGamePoker_AI3Bank <= 0f)
						{
							CasinoGames.AI3_Bust = true;
						}
						if (CasinoGames._8PlayerGamePoker_AI4Bank <= 0f)
						{
							CasinoGames.AI4_Bust = true;
						}
						if (CasinoGames._8PlayerGamePoker_AI5Bank <= 0f)
						{
							CasinoGames.AI5_Bust = true;
						}
						if (CasinoGames._8PlayerGamePoker_AI6Bank <= 0f)
						{
							CasinoGames.AI6_Bust = true;
						}
						if (CasinoGames._8PlayerGamePoker_AI7Bank <= 0f)
						{
							CasinoGames.AI7_Bust = true;
						}
					}
					if (CasinoGames._8PlayerGameStage == 3 && CasinoGames.BettingRoundStage == 0 && !CasinoGames.Player_Bust && !CasinoGames.Player_SkipTurn)
					{
						if (Game.IsControlJustPressed(Control.VehicleDuck))
						{
							List<Ped> list33 = new List<Ped>();
							CasinoGames.CommunityCard1.Heading = CasinoGames._8PlayerPokerGameTable3.Heading + 90f;
							CasinoGames.CommunityCard2.Heading = CasinoGames._8PlayerPokerGameTable3.Heading + 90f;
							CasinoGames.CommunityCard3.Heading = CasinoGames._8PlayerPokerGameTable3.Heading + 90f;
							list33.Add(CasinoGames.AI_1);
							list33.Add(CasinoGames.AI_2);
							list33.Add(CasinoGames.AI_3);
							list33.Add(CasinoGames.AI_4);
							list33.Add(CasinoGames.AI_5);
							list33.Add(CasinoGames.AI_6);
							list33.Add(CasinoGames.AI_7);
							list33.Add(Game.Player.Character);
							int index = 0;
							foreach (Ped item in list33)
							{
								string dict17 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01a@play@v02";
								string anim9 = "bet_ante";
								CasinoGames.LoadDict(dict17);
								if (item != Game.Player.Character)
								{
									CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, item.Position.X, item.Position.Y, item.Position.Z, 0.0, 0.0, item.Rotation.Z, 2);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2));
									CasinoGames.PlaySyncAnim(item, item, CasinoGames.Scene2, dict17, anim9);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[index].Card1, CasinoGames.Scene2, "bet_ante_card_a", CasinoGames.LoadDict(dict17), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[index].Card1, CasinoGames.Scene2, "bet_ante_card_b", CasinoGames.LoadDict(dict17), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCards[index].Card1, CasinoGames.Scene2, "bet_ante_card_c", CasinoGames.LoadDict(dict17), 16f, -16f, 0, 1148846080);
								}
								if (item == Game.Player.Character)
								{
									CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, item.Position.X, item.Position.Y, item.Position.Z, 0.0, 0.0, item.Rotation.Z, 2);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2));
									CasinoGames.PlaySyncAnim(item, item, CasinoGames.Scene2, dict17, anim9);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCard1, CasinoGames.Scene2, "bet_ante_card_a", CasinoGames.LoadDict(dict17), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCard2, CasinoGames.Scene2, "bet_ante_card_b", CasinoGames.LoadDict(dict17), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCard3, CasinoGames.Scene2, "bet_ante_card_c", CasinoGames.LoadDict(dict17), 16f, -16f, 0, 1148846080);
								}
							}
							Script.Wait(2000);
							CasinoGames.CheckPokerWin();
							if (CasinoGames.Player_VisualBank != null)
							{
								CasinoGames.Player_VisualBank.SpawnChips(CasinoGames._8PlayerGamePoker_PlayerBank);
							}
							if (CasinoGames.AI_1_VisualBank != null)
							{
								CasinoGames.AI_1_VisualBank.SpawnChips(CasinoGames._8PlayerGamePoker_AI1Bank);
							}
							if (CasinoGames.AI_2_VisualBank != null)
							{
								CasinoGames.AI_2_VisualBank.SpawnChips(CasinoGames._8PlayerGamePoker_AI2Bank);
							}
							if (CasinoGames.AI_3_VisualBank != null)
							{
								CasinoGames.AI_3_VisualBank.SpawnChips(CasinoGames._8PlayerGamePoker_AI3Bank);
							}
							if (CasinoGames.AI_4_VisualBank != null)
							{
								CasinoGames.AI_4_VisualBank.SpawnChips(CasinoGames._8PlayerGamePoker_AI4Bank);
							}
							if (CasinoGames.AI_5_VisualBank != null)
							{
								CasinoGames.AI_5_VisualBank.SpawnChips(CasinoGames._8PlayerGamePoker_AI5Bank);
							}
							if (CasinoGames.AI_6_VisualBank != null)
							{
								CasinoGames.AI_6_VisualBank.SpawnChips(CasinoGames._8PlayerGamePoker_AI6Bank);
							}
							if (CasinoGames.AI_7_VisualBank != null)
							{
								CasinoGames.AI_7_VisualBank.SpawnChips(CasinoGames._8PlayerGamePoker_AI7Bank);
							}
						}
						if (Game.IsControlJustPressed(Control.Reload))
						{
							Ped character2 = Game.Player.Character;
							string dict18 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@play@v01";
							string anim10 = "check_cards_fold";
							CasinoGames.LoadDict(dict18);
							CasinoGames.Player_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, character2.Position.X, character2.Position.Y, character2.Position.Z, 0.0, 0.0, character2.Rotation.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Player_SyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Player_SyncScene));
							CasinoGames.PlaySyncAnim(character2, character2, CasinoGames.Player_SyncScene, dict18, anim10);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCard1, CasinoGames.Player_SyncScene, "check_cards_fold_card_a", CasinoGames.LoadDict(dict18), 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCard2, CasinoGames.Player_SyncScene, "check_cards_fold_card_b", CasinoGames.LoadDict(dict18), 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCard3, CasinoGames.Player_SyncScene, "check_cards_fold_card_c", CasinoGames.LoadDict(dict18), 16f, -16f, 0, 1148846080);
							Script.Wait(8000);
							CasinoGames.CommunityCard1.Heading = CasinoGames._8PlayerPokerGameTable3.Heading + 90f;
							CasinoGames.CommunityCard2.Heading = CasinoGames._8PlayerPokerGameTable3.Heading + 90f;
							CasinoGames.CommunityCard3.Heading = CasinoGames._8PlayerPokerGameTable3.Heading + 90f;
							Script.Wait(2000);
							CasinoGames.CheckPokerWin();
							if (CasinoGames.Player_VisualBank != null)
							{
								CasinoGames.Player_VisualBank.SpawnChips(CasinoGames._8PlayerGamePoker_PlayerBank);
							}
							if (CasinoGames.AI_1_VisualBank != null)
							{
								CasinoGames.AI_1_VisualBank.SpawnChips(CasinoGames._8PlayerGamePoker_AI2Bank);
							}
							if (CasinoGames.AI_2_VisualBank != null)
							{
								CasinoGames.AI_2_VisualBank.SpawnChips(CasinoGames._8PlayerGamePoker_AI3Bank);
							}
							if (CasinoGames.AI_3_VisualBank != null)
							{
								CasinoGames.AI_3_VisualBank.SpawnChips(CasinoGames._8PlayerGamePoker_AI4Bank);
							}
							if (CasinoGames.AI_4_VisualBank != null)
							{
								CasinoGames.AI_4_VisualBank.SpawnChips(CasinoGames._8PlayerGamePoker_AI5Bank);
							}
							if (CasinoGames.AI_5_VisualBank != null)
							{
								CasinoGames.AI_5_VisualBank.SpawnChips(CasinoGames._8PlayerGamePoker_AI5Bank);
							}
							if (CasinoGames.AI_6_VisualBank != null)
							{
								CasinoGames.AI_6_VisualBank.SpawnChips(CasinoGames._8PlayerGamePoker_AI6Bank);
							}
							if (CasinoGames.AI_7_VisualBank != null)
							{
								CasinoGames.AI_7_VisualBank.SpawnChips(CasinoGames._8PlayerGamePoker_AI7Bank);
							}
							CasinoGames._8PlayerGameStage = 1;
							Notification.Show("~g~ HKH191~w~ : Player Has folded!, Starting new Match");
						}
						if (Game.IsControlJustPressed(Control.Context))
						{
							Ped character3 = Game.Player.Character;
							string dict19 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v01";
							string anim11 = "check_cards_fold";
							CasinoGames.LoadDict(dict19);
							CasinoGames.Player_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, character3.Position.X, character3.Position.Y, character3.Position.Z, 0.0, 0.0, character3.Rotation.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Player_SyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Player_SyncScene));
							CasinoGames.PlaySyncAnim(character3, character3, CasinoGames.Player_SyncScene, dict19, anim11);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCard1, CasinoGames.Player_SyncScene, "check_cards_fold_card_a", CasinoGames.LoadDict(dict19), 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCard2, CasinoGames.Player_SyncScene, "check_cards_fold_card_b", CasinoGames.LoadDict(dict19), 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCard3, CasinoGames.Player_SyncScene, "check_cards_fold_card_c", CasinoGames.LoadDict(dict19), 16f, -16f, 0, 1148846080);
						}
					}
					if (CasinoGames._8PlayerGameStage == 3 && CasinoGames.BettingRoundStage == 0 && !CasinoGames.IsinBettingRound && Game.IsControlJustPressed(Control.ThrowGrenade))
					{
						try
						{
							string text27 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
							string text28 = "bet_plus";
							string userInput = Game.GetUserInput(WindowTitle.EnterEyefindMessage, "", 12);
							CasinoGames.IsinBettingRound = true;
							new Random();
							float num30 = float.Parse(userInput);
							if (CasinoGames.LastRaiseAmmount <= num30)
							{
								text27 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
								text28 = "bet_plus";
								CasinoGames.LastRaiseAmmount = num30;
								if (CasinoGames._8PlayerGamePoker_PlayerBank - num30 > 0f)
								{
									text27 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
									text28 = "bet_plus";
									CasinoGames._8PlayerGamePoker_PlayerBank -= num30;
									CasinoGames._8PlayerGamePokerPot += CasinoGames.LastRaiseAmmount;
									CasinoGames.PlaySyncAnim(Game.Player.Character, Game.Player.Character, CasinoGames.Player_SyncScene, text27, text28);
									if (CasinoGames.Player_VisualBank != null)
									{
										CasinoGames.Player_VisualBank.SpawnChips(CasinoGames._8PlayerGamePoker_PlayerBank);
									}
								}
								else
								{
									Ped character4 = Game.Player.Character;
									text27 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@play@v01";
									text28 = "check_cards_fold";
									CasinoGames.LoadDict(text27);
									CasinoGames.Player_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, character4.Position.X, character4.Position.Y, character4.Position.Z, 0.0, 0.0, character4.Rotation.Z, 2);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Player_SyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Player_SyncScene));
									CasinoGames.PlaySyncAnim(character4, character4, CasinoGames.Player_SyncScene, text27, text28);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCard1, CasinoGames.Player_SyncScene, "check_cards_fold_card_a", CasinoGames.LoadDict(text27), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCard2, CasinoGames.Player_SyncScene, "check_cards_fold_card_b", CasinoGames.LoadDict(text27), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCard3, CasinoGames.Player_SyncScene, "check_cards_fold_card_c", CasinoGames.LoadDict(text27), 16f, -16f, 0, 1148846080);
									Notification.Show("You Folded!, Due to not Being able to Call/Raise Bet");
								}
							}
							else
							{
								Notification.Show("~g~ HKH191~w~ Cannot Enter a bet lower than the Last Bet!, Skippinng Players Turn");
							}
							List<Ped> list34 = new List<Ped>();
							Script.Wait(700);
							CasinoGames.RunBet(num30);
							list34.Add(CasinoGames.AI_1);
							list34.Add(CasinoGames.AI_2);
							list34.Add(CasinoGames.AI_3);
							list34.Add(CasinoGames.AI_4);
							list34.Add(CasinoGames.AI_5);
							list34.Add(CasinoGames.AI_6);
							list34.Add(CasinoGames.AI_7);
							CasinoGames.AreBetsBeingPlaced = true;
							CasinoGames.BettingRoundStage = 0;
						}
						catch
						{
							Notification.Show("~g~ HKH191~w~ Player did not enter a valid Bet!");
							CasinoGames.IsinBettingRound = false;
						}
					}
					if (CasinoGames._8PlayerGameStage == 2)
					{
						if (CasinoGames.IsinBettingRound)
						{
							CasinoGames._8PlayerGameStage = 3;
							CasinoGames.CheckPokerBuyOut();
							Notification.Show("Its your turn, either Bet, Fold or Check");
							CasinoGames.IsinBettingRound = false;
						}
						if (Game.IsControlJustPressed(Control.Context))
						{
							CasinoGames.IsinBettingRound = false;
							Notification.Show("Its your turn, either Bet, Fold or Check");
							CasinoGames._8PlayerGameStage = 3;
						}
					}
					if (CasinoGames._8PlayerGameStage == 1)
					{
						CasinoGames.ResetCardDeck();
						string text29 = "anim_casino_b@amb@casino@games@threecardpoker@dealer";
						string text30 = "deck_shuffle";
						if (CasinoGames.AI1_Bust && CasinoGames.AI2_Bust && CasinoGames.AI3_Bust && CasinoGames.AI4_Bust && CasinoGames.AI5_Bust && CasinoGames.AI6_Bust && CasinoGames.AI7_Bust)
						{
							Notification.Show("Exiting Game, All AI went Bust!");
							CasinoGames.StackChipHeight = 0.6f;
							CasinoGames.StackChip = 0;
							text29 = "anim_casino_b@amb@casino@games@shared@player@";
							CasinoGames.Playing8PlayerPoker = false;
							Prop _8PlayerPokerGameTable17 = CasinoGames._8PlayerPokerGameTable1;
							CasinoGames.Player_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, _8PlayerPokerGameTable17.Position.Z, 0.0, 0.0, _8PlayerPokerGameTable17.Rotation.Z, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Player_SyncScene, CasinoGames.LoadDict(text29), "sit_exit_left", 16f, -16f, 0, 1148846080);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Player_SyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Player_SyncScene));
							text30 = CasinoGames.GetExitAnim();
							CasinoGames.PlaySyncExitAnim(CasinoGames.AI_1, _8PlayerPokerGameTable17, CasinoGames.AI1_SyncScene, text29, text30);
							CasinoGames.PlaySyncExitAnim(CasinoGames.AI_2, _8PlayerPokerGameTable17, CasinoGames.AI2_SyncScene, text29, text30);
							CasinoGames.PlaySyncExitAnim(CasinoGames.AI_3, _8PlayerPokerGameTable17, CasinoGames.AI3_SyncScene, text29, text30);
							CasinoGames.PlaySyncExitAnim(CasinoGames.AI_4, _8PlayerPokerGameTable17, CasinoGames.AI4_SyncScene, text29, text30);
							CasinoGames.PlaySyncExitAnim(CasinoGames.AI_5, _8PlayerPokerGameTable17, CasinoGames.AI5_SyncScene, text29, text30);
							CasinoGames.PlaySyncExitAnim(CasinoGames.AI_6, _8PlayerPokerGameTable17, CasinoGames.AI6_SyncScene, text29, text30);
							CasinoGames.PlaySyncExitAnim(CasinoGames.AI_7, _8PlayerPokerGameTable17, CasinoGames.AI7_SyncScene, text29, text30);
							Script.Wait(4000);
							Game.Player.Character.Task.ClearAll();
							if (CasinoGames.AI_1 != null)
							{
								CasinoGames.AI_1.Task.WanderAround();
							}
							if (CasinoGames.AI_2 != null)
							{
								CasinoGames.AI_2.Task.WanderAround();
							}
							if (CasinoGames.AI_3 != null)
							{
								CasinoGames.AI_3.Task.WanderAround();
							}
							if (CasinoGames.AI_4 != null)
							{
								CasinoGames.AI_4.Task.WanderAround();
							}
							if (CasinoGames.AI_5 != null)
							{
								CasinoGames.AI_5.Task.WanderAround();
							}
							if (CasinoGames.AI_6 != null)
							{
								CasinoGames.AI_6.Task.WanderAround();
							}
							if (CasinoGames.AI_7 != null)
							{
								CasinoGames.AI_7.Task.WanderAround();
							}
							foreach (Ped _8PlayerGamePokerPed in CasinoGames._8PlayerGamePokerPeds)
							{
								if (_8PlayerGamePokerPed != null)
								{
									_8PlayerGamePokerPed.Task.WanderAround();
								}
							}
						}
						CasinoGames.StackChipHeight = 0.6f;
						CasinoGames.StackChip = 0;
						CasinoGames.Player_SkipTurn = false;
						CasinoGames.AI1_SkipTurn = false;
						CasinoGames.AI2_SkipTurn = false;
						CasinoGames.AI3_SkipTurn = false;
						CasinoGames.AI4_SkipTurn = false;
						CasinoGames.AI5_SkipTurn = false;
						CasinoGames.AI6_SkipTurn = false;
						CasinoGames.AI7_SkipTurn = false;
						foreach (Prop _8PlayerGamePokerProp in CasinoGames._8PlayerGamePokerProps)
						{
							if (_8PlayerGamePokerProp != null)
							{
								_8PlayerGamePokerProp.Delete();
							}
						}
						foreach (Prop _8PlayerGamePokerPotChip in CasinoGames._8PlayerGamePokerPotChips)
						{
							if (_8PlayerGamePokerPotChip != null)
							{
								_8PlayerGamePokerPotChip.Delete();
							}
						}
						CasinoGames._8PlayerGamePokerPot = 0f;
						CasinoGames._8PlayerGamePokerProps.Clear();
						Random random2 = new Random();
						List<Ped> list35 = new List<Ped>();
						list35.Add(CasinoGames.AI_1);
						list35.Add(CasinoGames.AI_2);
						list35.Add(CasinoGames.AI_3);
						list35.Add(CasinoGames.AI_4);
						list35.Add(CasinoGames.AI_5);
						list35.Add(CasinoGames.AI_6);
						list35.Add(CasinoGames.AI_7);
						int num31 = 0;
						list35.Add(Game.Player.Character);
						Script.Wait(1);
						string randomCardPlayer = CasinoGames.GetRandomCardPlayer();
						Script.Wait(1);
						string randomCardPlayer2 = CasinoGames.GetRandomCardPlayer();
						Script.Wait(1);
						string randomCardPlayer3 = CasinoGames.GetRandomCardPlayer();
						CasinoGames.CommunityCard1ID = CasinoGames.GetCardName(randomCardPlayer);
						CasinoGames.CommunityCard2ID = CasinoGames.GetCardName(randomCardPlayer2);
						CasinoGames.CommunityCard3ID = CasinoGames.GetCardName(randomCardPlayer3);
						CasinoGames.CommunityCard1 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer), CasinoGames._8PlayerPokerGameTable3.GetOffsetPosition(new Vector3(-0.35f, 0f, -0.4f)), true, false);
						CasinoGames.CommunityCard1Value = CasinoGames.GetCardTotal(randomCardPlayer);
						CasinoGames.CommunityCard1.Heading = CasinoGames._8PlayerPokerGameTable3.Heading - 90f;
						CasinoGames._8PlayerGamePokerProps.Add(CasinoGames.CommunityCard1);
						CasinoGames.CommunityCard2 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer2), CasinoGames._8PlayerPokerGameTable3.GetOffsetPosition(new Vector3(-0.35f, -0.1f, -0.4f)), true, false);
						CasinoGames.CommunityCard2Value = CasinoGames.GetCardTotal(randomCardPlayer2);
						CasinoGames.CommunityCard2.Heading = CasinoGames._8PlayerPokerGameTable3.Heading - 90f;
						CasinoGames._8PlayerGamePokerProps.Add(CasinoGames.CommunityCard2);
						CasinoGames.CommunityCard3 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer3), CasinoGames._8PlayerPokerGameTable3.GetOffsetPosition(new Vector3(-0.35f, 0.1f, -0.4f)), true, false);
						CasinoGames.CommunityCard3Value = CasinoGames.GetCardTotal(randomCardPlayer3);
						CasinoGames.CommunityCard3.Heading = CasinoGames._8PlayerPokerGameTable3.Heading - 90f;
						CasinoGames._8PlayerGamePokerProps.Add(CasinoGames.CommunityCard3);
						CasinoGames.CommunityCard1.Rotation = new Vector3(CasinoGames.CommunityCard1.Rotation.X, 180f, CasinoGames.CommunityCard1.Rotation.Z);
						CasinoGames.CommunityCard2.Rotation = new Vector3(CasinoGames.CommunityCard2.Rotation.X, 180f, CasinoGames.CommunityCard2.Rotation.Z);
						CasinoGames.CommunityCard3.Rotation = new Vector3(CasinoGames.CommunityCard3.Rotation.X, 180f, CasinoGames.CommunityCard3.Rotation.Z);
						CasinoGames.PlayerCards.Clear();
						foreach (Ped item2 in list35)
						{
							if (item2 == CasinoGames.AI_1 && !CasinoGames.AI1_Bust)
							{
								Script.Wait(1);
								string randomCardPlayer4 = CasinoGames.GetRandomCardPlayer();
								Prop prop = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer4), item2.GetOffsetPosition(new Vector3(0.08f, 0.5f, -1.05f)), true, false);
								prop.Heading = item2.Heading;
								CasinoGames._8PlayerGamePokerProps.Add(prop);
								Script.Wait(1);
								string randomCardPlayer5 = CasinoGames.GetRandomCardPlayer();
								Prop prop2 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer5), item2.GetOffsetPosition(new Vector3(0f, 0.5f, -1.05f)), true, false);
								prop2.Heading = item2.Heading;
								CasinoGames._8PlayerGamePokerProps.Add(prop2);
								Script.Wait(1);
								string randomCardPlayer6 = CasinoGames.GetRandomCardPlayer();
								Prop prop3 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer6), item2.GetOffsetPosition(new Vector3(-0.08f, 0.5f, -1.05f)), true, false);
								prop3.Heading = item2.Heading;
								CasinoGames._8PlayerGamePokerProps.Add(prop3);
								Script.Wait(1);
								random2.Next(1, 2);
								Script.Wait(1);
								text29 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v02";
								text30 = "check_cards_bet";
								CasinoGames.LoadDict(text29);
								CasinoGames.AI1_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, item2.Position.X, item2.Position.Y, item2.Position.Z, 0.0, 0.0, item2.Rotation.Z, 2);
								CasinoGames.PlaySyncAnim(item2, item2, CasinoGames.AI1_SyncScene, text29, text30);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop, CasinoGames.AI1_SyncScene, "check_cards_bet_card_a", CasinoGames.LoadDict(text29), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop2, CasinoGames.AI1_SyncScene, "check_cards_bet_card_b", CasinoGames.LoadDict(text29), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop3, CasinoGames.AI1_SyncScene, "check_cards_bet_card_c", CasinoGames.LoadDict(text29), 16f, -16f, 0, 1148846080);
								CasinoGames.PlayerCards.Add(new PlayerCards(prop, prop2, prop3, randomCardPlayer4, randomCardPlayer5, randomCardPlayer6));
								if (item2 == Game.Player.Character)
								{
									CasinoGames.PlayerCard1 = prop;
									CasinoGames.PlayerCard2 = prop2;
									CasinoGames.PlayerCard3 = prop3;
								}
								CasinoGames.AI1Total = CasinoGames.GetPlayerTotal(randomCardPlayer4, randomCardPlayer5, randomCardPlayer6);
							}
							if (item2 == CasinoGames.AI_2 && !CasinoGames.AI2_Bust)
							{
								Script.Wait(1);
								string randomCardPlayer7 = CasinoGames.GetRandomCardPlayer();
								Prop prop4 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer7), item2.GetOffsetPosition(new Vector3(0.08f, 0.5f, -1.05f)), true, false);
								prop4.Heading = item2.Heading;
								CasinoGames._8PlayerGamePokerProps.Add(prop4);
								Script.Wait(1);
								string randomCardPlayer8 = CasinoGames.GetRandomCardPlayer();
								Prop prop5 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer8), item2.GetOffsetPosition(new Vector3(0f, 0.5f, -1.05f)), true, false);
								prop5.Heading = item2.Heading;
								CasinoGames._8PlayerGamePokerProps.Add(prop5);
								Script.Wait(1);
								string randomCardPlayer9 = CasinoGames.GetRandomCardPlayer();
								Prop prop6 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer9), item2.GetOffsetPosition(new Vector3(-0.08f, 0.5f, -1.05f)), true, false);
								prop6.Heading = item2.Heading;
								CasinoGames._8PlayerGamePokerProps.Add(prop6);
								Script.Wait(1);
								random2.Next(1, 2);
								Script.Wait(1);
								text29 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v02";
								text30 = "check_cards_bet";
								CasinoGames.LoadDict(text29);
								CasinoGames.AI2_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, item2.Position.X, item2.Position.Y, item2.Position.Z, 0.0, 0.0, item2.Rotation.Z, 2);
								CasinoGames.PlaySyncAnim(item2, item2, CasinoGames.AI2_SyncScene, text29, text30);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop4, CasinoGames.AI2_SyncScene, "check_cards_bet_card_a", CasinoGames.LoadDict(text29), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop5, CasinoGames.AI2_SyncScene, "check_cards_bet_card_b", CasinoGames.LoadDict(text29), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop6, CasinoGames.AI2_SyncScene, "check_cards_bet_card_c", CasinoGames.LoadDict(text29), 16f, -16f, 0, 1148846080);
								CasinoGames.PlayerCards.Add(new PlayerCards(prop4, prop5, prop6, randomCardPlayer7, randomCardPlayer8, randomCardPlayer9));
								if (item2 == Game.Player.Character)
								{
									CasinoGames.PlayerCard1 = prop4;
									CasinoGames.PlayerCard2 = prop5;
									CasinoGames.PlayerCard3 = prop6;
								}
								CasinoGames.AI2Total = CasinoGames.GetPlayerTotal(randomCardPlayer7, randomCardPlayer8, randomCardPlayer9);
							}
							if (item2 == CasinoGames.AI_3 && !CasinoGames.AI3_Bust)
							{
								Script.Wait(1);
								string randomCardPlayer10 = CasinoGames.GetRandomCardPlayer();
								Prop prop7 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer10), item2.GetOffsetPosition(new Vector3(0.08f, 0.5f, -1.05f)), true, false);
								prop7.Heading = item2.Heading;
								CasinoGames._8PlayerGamePokerProps.Add(prop7);
								Script.Wait(1);
								string randomCardPlayer11 = CasinoGames.GetRandomCardPlayer();
								Prop prop8 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer11), item2.GetOffsetPosition(new Vector3(0f, 0.5f, -1.05f)), true, false);
								prop8.Heading = item2.Heading;
								CasinoGames._8PlayerGamePokerProps.Add(prop8);
								Script.Wait(1);
								string randomCardPlayer12 = CasinoGames.GetRandomCardPlayer();
								Prop prop9 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer12), item2.GetOffsetPosition(new Vector3(-0.08f, 0.5f, -1.05f)), true, false);
								prop9.Heading = item2.Heading;
								CasinoGames._8PlayerGamePokerProps.Add(prop9);
								Script.Wait(1);
								random2.Next(1, 2);
								Script.Wait(1);
								text29 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v02";
								text30 = "check_cards_bet";
								CasinoGames.LoadDict(text29);
								CasinoGames.AI3_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, item2.Position.X, item2.Position.Y, item2.Position.Z, 0.0, 0.0, item2.Rotation.Z, 2);
								CasinoGames.PlaySyncAnim(item2, item2, CasinoGames.AI3_SyncScene, text29, text30);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop7, CasinoGames.AI3_SyncScene, "check_cards_bet_card_a", CasinoGames.LoadDict(text29), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop8, CasinoGames.AI3_SyncScene, "check_cards_bet_card_b", CasinoGames.LoadDict(text29), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop9, CasinoGames.AI3_SyncScene, "check_cards_bet_card_c", CasinoGames.LoadDict(text29), 16f, -16f, 0, 1148846080);
								CasinoGames.PlayerCards.Add(new PlayerCards(prop7, prop8, prop9, randomCardPlayer10, randomCardPlayer11, randomCardPlayer12));
								if (item2 == Game.Player.Character)
								{
									CasinoGames.PlayerCard1 = prop7;
									CasinoGames.PlayerCard2 = prop8;
									CasinoGames.PlayerCard3 = prop9;
								}
								CasinoGames.AI3Total = CasinoGames.GetPlayerTotal(randomCardPlayer10, randomCardPlayer11, randomCardPlayer12);
							}
							if (item2 == CasinoGames.AI_4 && !CasinoGames.AI4_Bust)
							{
								Script.Wait(1);
								string randomCardPlayer13 = CasinoGames.GetRandomCardPlayer();
								Prop prop10 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer13), item2.GetOffsetPosition(new Vector3(0.08f, 0.5f, -1.05f)), true, false);
								prop10.Heading = item2.Heading;
								CasinoGames._8PlayerGamePokerProps.Add(prop10);
								Script.Wait(1);
								string randomCardPlayer14 = CasinoGames.GetRandomCardPlayer();
								Prop prop11 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer14), item2.GetOffsetPosition(new Vector3(0f, 0.5f, -1.05f)), true, false);
								prop11.Heading = item2.Heading;
								CasinoGames._8PlayerGamePokerProps.Add(prop11);
								Script.Wait(1);
								string randomCardPlayer15 = CasinoGames.GetRandomCardPlayer();
								Prop prop12 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer15), item2.GetOffsetPosition(new Vector3(-0.08f, 0.5f, -1.05f)), true, false);
								prop12.Heading = item2.Heading;
								CasinoGames._8PlayerGamePokerProps.Add(prop12);
								Script.Wait(1);
								random2.Next(1, 2);
								Script.Wait(1);
								text29 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v02";
								text30 = "check_cards_bet";
								CasinoGames.LoadDict(text29);
								CasinoGames.AI4_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, item2.Position.X, item2.Position.Y, item2.Position.Z, 0.0, 0.0, item2.Rotation.Z, 2);
								CasinoGames.PlaySyncAnim(item2, item2, CasinoGames.AI4_SyncScene, text29, text30);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop10, CasinoGames.AI4_SyncScene, "check_cards_bet_card_a", CasinoGames.LoadDict(text29), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop11, CasinoGames.AI4_SyncScene, "check_cards_bet_card_b", CasinoGames.LoadDict(text29), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop12, CasinoGames.AI4_SyncScene, "check_cards_bet_card_c", CasinoGames.LoadDict(text29), 16f, -16f, 0, 1148846080);
								CasinoGames.PlayerCards.Add(new PlayerCards(prop10, prop11, prop12, randomCardPlayer13, randomCardPlayer14, randomCardPlayer15));
								if (item2 == Game.Player.Character)
								{
									CasinoGames.PlayerCard1 = prop10;
									CasinoGames.PlayerCard2 = prop11;
									CasinoGames.PlayerCard3 = prop12;
								}
								CasinoGames.AI4Total = CasinoGames.GetPlayerTotal(randomCardPlayer13, randomCardPlayer14, randomCardPlayer15);
							}
							if (item2 == CasinoGames.AI_5 && !CasinoGames.AI5_Bust)
							{
								Script.Wait(1);
								string randomCardPlayer16 = CasinoGames.GetRandomCardPlayer();
								Prop prop13 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer16), item2.GetOffsetPosition(new Vector3(0.08f, 0.5f, -1.05f)), true, false);
								prop13.Heading = item2.Heading;
								CasinoGames._8PlayerGamePokerProps.Add(prop13);
								Script.Wait(1);
								string randomCardPlayer17 = CasinoGames.GetRandomCardPlayer();
								Prop prop14 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer17), item2.GetOffsetPosition(new Vector3(0f, 0.5f, -1.05f)), true, false);
								prop14.Heading = item2.Heading;
								CasinoGames._8PlayerGamePokerProps.Add(prop14);
								Script.Wait(1);
								string randomCardPlayer18 = CasinoGames.GetRandomCardPlayer();
								Prop prop15 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer18), item2.GetOffsetPosition(new Vector3(-0.08f, 0.5f, -1.05f)), true, false);
								prop15.Heading = item2.Heading;
								CasinoGames._8PlayerGamePokerProps.Add(prop15);
								Script.Wait(1);
								random2.Next(1, 2);
								Script.Wait(1);
								text29 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v02";
								text30 = "check_cards_bet";
								CasinoGames.LoadDict(text29);
								CasinoGames.AI5_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, item2.Position.X, item2.Position.Y, item2.Position.Z, 0.0, 0.0, item2.Rotation.Z, 2);
								CasinoGames.PlaySyncAnim(item2, item2, CasinoGames.AI5_SyncScene, text29, text30);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop13, CasinoGames.AI5_SyncScene, "check_cards_bet_card_a", CasinoGames.LoadDict(text29), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop14, CasinoGames.AI5_SyncScene, "check_cards_bet_card_b", CasinoGames.LoadDict(text29), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop15, CasinoGames.AI5_SyncScene, "check_cards_bet_card_c", CasinoGames.LoadDict(text29), 16f, -16f, 0, 1148846080);
								CasinoGames.PlayerCards.Add(new PlayerCards(prop13, prop14, prop15, randomCardPlayer16, randomCardPlayer17, randomCardPlayer18));
								if (item2 == Game.Player.Character)
								{
									CasinoGames.PlayerCard1 = prop13;
									CasinoGames.PlayerCard2 = prop14;
									CasinoGames.PlayerCard3 = prop15;
								}
								CasinoGames.AI5Total = CasinoGames.GetPlayerTotal(randomCardPlayer16, randomCardPlayer17, randomCardPlayer18);
							}
							if (item2 == CasinoGames.AI_6 && !CasinoGames.AI6_Bust)
							{
								Script.Wait(1);
								string randomCardPlayer19 = CasinoGames.GetRandomCardPlayer();
								Prop prop16 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer19), item2.GetOffsetPosition(new Vector3(0.08f, 0.5f, -1.05f)), true, false);
								prop16.Heading = item2.Heading;
								CasinoGames._8PlayerGamePokerProps.Add(prop16);
								Script.Wait(1);
								string randomCardPlayer20 = CasinoGames.GetRandomCardPlayer();
								Prop prop17 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer20), item2.GetOffsetPosition(new Vector3(0f, 0.5f, -1.05f)), true, false);
								prop17.Heading = item2.Heading;
								CasinoGames._8PlayerGamePokerProps.Add(prop17);
								Script.Wait(1);
								string randomCardPlayer21 = CasinoGames.GetRandomCardPlayer();
								Prop prop18 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer21), item2.GetOffsetPosition(new Vector3(-0.08f, 0.5f, -1.05f)), true, false);
								prop18.Heading = item2.Heading;
								CasinoGames._8PlayerGamePokerProps.Add(prop18);
								Script.Wait(1);
								random2.Next(1, 2);
								Script.Wait(1);
								text29 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v02";
								text30 = "check_cards_bet";
								CasinoGames.LoadDict(text29);
								CasinoGames.AI6_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, item2.Position.X, item2.Position.Y, item2.Position.Z, 0.0, 0.0, item2.Rotation.Z, 2);
								CasinoGames.PlaySyncAnim(item2, item2, CasinoGames.AI6_SyncScene, text29, text30);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop16, CasinoGames.AI6_SyncScene, "check_cards_bet_card_a", CasinoGames.LoadDict(text29), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop17, CasinoGames.AI6_SyncScene, "check_cards_bet_card_b", CasinoGames.LoadDict(text29), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop18, CasinoGames.AI6_SyncScene, "check_cards_bet_card_c", CasinoGames.LoadDict(text29), 16f, -16f, 0, 1148846080);
								CasinoGames.PlayerCards.Add(new PlayerCards(prop16, prop17, prop18, randomCardPlayer19, randomCardPlayer20, randomCardPlayer21));
								if (item2 == Game.Player.Character)
								{
									CasinoGames.PlayerCard1 = prop16;
									CasinoGames.PlayerCard2 = prop17;
									CasinoGames.PlayerCard3 = prop18;
								}
								CasinoGames.AI6Total = CasinoGames.GetPlayerTotal(randomCardPlayer19, randomCardPlayer20, randomCardPlayer21);
							}
							if (item2 == CasinoGames.AI_7 && !CasinoGames.AI7_Bust)
							{
								Script.Wait(1);
								string randomCardPlayer22 = CasinoGames.GetRandomCardPlayer();
								Prop prop19 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer22), item2.GetOffsetPosition(new Vector3(0.08f, 0.5f, -1.05f)), true, false);
								prop19.Heading = item2.Heading;
								CasinoGames._8PlayerGamePokerProps.Add(prop19);
								Script.Wait(1);
								string randomCardPlayer23 = CasinoGames.GetRandomCardPlayer();
								Prop prop20 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer23), item2.GetOffsetPosition(new Vector3(0f, 0.5f, -1.05f)), true, false);
								prop20.Heading = item2.Heading;
								CasinoGames._8PlayerGamePokerProps.Add(prop20);
								Script.Wait(1);
								string randomCardPlayer24 = CasinoGames.GetRandomCardPlayer();
								Prop prop21 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer24), item2.GetOffsetPosition(new Vector3(-0.08f, 0.5f, -1.05f)), true, false);
								prop21.Heading = item2.Heading;
								CasinoGames._8PlayerGamePokerProps.Add(prop21);
								Script.Wait(1);
								random2.Next(1, 2);
								Script.Wait(1);
								text29 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v02";
								text30 = "check_cards_bet";
								CasinoGames.LoadDict(text29);
								CasinoGames.AI7_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, item2.Position.X, item2.Position.Y, item2.Position.Z, 0.0, 0.0, item2.Rotation.Z, 2);
								CasinoGames.PlaySyncAnim(item2, item2, CasinoGames.AI7_SyncScene, text29, text30);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop19, CasinoGames.AI7_SyncScene, "check_cards_bet_card_a", CasinoGames.LoadDict(text29), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop20, CasinoGames.AI7_SyncScene, "check_cards_bet_card_b", CasinoGames.LoadDict(text29), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop21, CasinoGames.AI7_SyncScene, "check_cards_bet_card_c", CasinoGames.LoadDict(text29), 16f, -16f, 0, 1148846080);
								CasinoGames.PlayerCards.Add(new PlayerCards(prop19, prop20, prop21, randomCardPlayer22, randomCardPlayer23, randomCardPlayer24));
								if (item2 == Game.Player.Character)
								{
									CasinoGames.PlayerCard1 = prop19;
									CasinoGames.PlayerCard2 = prop20;
									CasinoGames.PlayerCard3 = prop21;
								}
								CasinoGames.AI7Total = CasinoGames.GetPlayerTotal(randomCardPlayer22, randomCardPlayer23, randomCardPlayer24);
							}
							if (item2 == Game.Player.Character)
							{
								Script.Wait(1);
								string randomCardPlayer25 = CasinoGames.GetRandomCardPlayer();
								Prop prop22 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer25), item2.GetOffsetPosition(new Vector3(0.08f, 0.5f, -1.05f)), true, false);
								prop22.Heading = item2.Heading;
								CasinoGames._8PlayerGamePokerProps.Add(prop22);
								Script.Wait(1);
								string randomCardPlayer26 = CasinoGames.GetRandomCardPlayer();
								Prop prop23 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer26), item2.GetOffsetPosition(new Vector3(0f, 0.5f, -1.05f)), true, false);
								prop23.Heading = item2.Heading;
								CasinoGames._8PlayerGamePokerProps.Add(prop23);
								Script.Wait(1);
								string randomCardPlayer27 = CasinoGames.GetRandomCardPlayer();
								Prop prop24 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer27), item2.GetOffsetPosition(new Vector3(-0.08f, 0.5f, -1.05f)), true, false);
								prop24.Heading = item2.Heading;
								CasinoGames._8PlayerGamePokerProps.Add(prop24);
								Script.Wait(1);
								random2.Next(1, 2);
								Script.Wait(1);
								text29 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@01b@play@v02";
								text30 = "check_cards_bet";
								CasinoGames.LoadDict(text29);
								CasinoGames.Player_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, item2.Position.X, item2.Position.Y, item2.Position.Z, 0.0, 0.0, item2.Rotation.Z, 2);
								CasinoGames.PlaySyncAnim(item2, item2, CasinoGames.Player_SyncScene, text29, text30);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop22, CasinoGames.Player_SyncScene, "check_cards_bet_card_a", CasinoGames.LoadDict(text29), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop23, CasinoGames.Player_SyncScene, "check_cards_bet_card_b", CasinoGames.LoadDict(text29), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop24, CasinoGames.Player_SyncScene, "check_cards_bet_card_c", CasinoGames.LoadDict(text29), 16f, -16f, 0, 1148846080);
								CasinoGames.PlayerCards.Add(new PlayerCards(prop22, prop23, prop24, randomCardPlayer25, randomCardPlayer26, randomCardPlayer27));
								if (item2 == Game.Player.Character)
								{
									CasinoGames.PlayerCard1 = prop22;
									CasinoGames.PlayerCard2 = prop23;
									CasinoGames.PlayerCard3 = prop24;
								}
								CasinoGames.PlayerTotal = CasinoGames.GetPlayerTotal(randomCardPlayer25, randomCardPlayer26, randomCardPlayer27);
							}
							num31++;
							Script.Wait(100);
						}
						if (CasinoGames.Deck != null)
						{
							CasinoGames.Deck.Delete();
						}
						CasinoGames.Deck = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_cards_01"), CasinoGames._8PlayerPokerGameTable1.Position, false, false);
						CasinoGames._8PlayerGamePokerProps.Add(CasinoGames.Deck);
						Script.Wait(10000);
						CasinoGames.Deck.Detach();
						if (CasinoGames.ChipsSpawn != null)
						{
							CasinoGames.ChipsSpawn.Delete();
						}
						CasinoGames.ChipsSpawn = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_chip_1kdollar_x1"), CasinoGames._8PlayerPokerGameTable3.GetOffsetPosition(new Vector3(0f, 0f, 1f)), false, false);
						CasinoGames.ChipsSpawn.IsCollisionEnabled = true;
						CasinoGames.ChipsSpawn.IsPositionFrozen = true;
						CasinoGames.ChipsSpawn.IsVisible = false;
						foreach (Ped item3 in list35)
						{
							if (item3 == CasinoGames.AI_1 && !CasinoGames.AI1_Bust)
							{
								text29 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
								text30 = "bet_plus";
								CasinoGames.PlaySyncAnim(item3, item3, CasinoGames.AI1_SyncScene, text29, text30);
								Script.Wait(700);
								CasinoGames._8PlayerGamePokerPot += CasinoGames._8PlayerGamePoker_AI1StartingBank / 100f;
								CasinoGames.RunBet(CasinoGames._8PlayerGamePokerPot);
								CasinoGames._8PlayerGamePoker_AI1Bank -= CasinoGames._8PlayerGamePoker_AI1StartingBank / 100f;
							}
							if (item3 == CasinoGames.AI_2 && !CasinoGames.AI2_Bust)
							{
								text29 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
								text30 = "bet_plus";
								CasinoGames.PlaySyncAnim(item3, item3, CasinoGames.AI2_SyncScene, text29, text30);
								Script.Wait(700);
								CasinoGames._8PlayerGamePokerPot += CasinoGames._8PlayerGamePoker_AI1StartingBank / 100f;
								CasinoGames.RunBet(CasinoGames._8PlayerGamePokerPot);
								CasinoGames._8PlayerGamePoker_AI2Bank -= CasinoGames._8PlayerGamePoker_AI2StartingBank / 100f;
							}
							if (item3 == CasinoGames.AI_3 && !CasinoGames.AI3_Bust)
							{
								text29 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
								text30 = "bet_plus";
								CasinoGames.PlaySyncAnim(item3, item3, CasinoGames.AI3_SyncScene, text29, text30);
								Script.Wait(700);
								CasinoGames._8PlayerGamePokerPot += CasinoGames._8PlayerGamePoker_AI1StartingBank / 100f;
								CasinoGames.RunBet(CasinoGames._8PlayerGamePokerPot);
								CasinoGames._8PlayerGamePoker_AI3Bank -= CasinoGames._8PlayerGamePoker_AI3StartingBank / 100f;
							}
							if (item3 == CasinoGames.AI_4 && !CasinoGames.AI4_Bust)
							{
								text29 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
								text30 = "bet_plus";
								CasinoGames.PlaySyncAnim(item3, item3, CasinoGames.AI4_SyncScene, text29, text30);
								Script.Wait(700);
								CasinoGames._8PlayerGamePokerPot += CasinoGames._8PlayerGamePoker_AI1StartingBank / 100f;
								CasinoGames.RunBet(CasinoGames._8PlayerGamePokerPot);
								CasinoGames._8PlayerGamePoker_AI4Bank -= CasinoGames._8PlayerGamePoker_AI4StartingBank / 100f;
							}
							if (item3 == CasinoGames.AI_5 && !CasinoGames.AI5_Bust)
							{
								text29 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
								text30 = "bet_plus";
								CasinoGames.PlaySyncAnim(item3, item3, CasinoGames.AI5_SyncScene, text29, text30);
								Script.Wait(700);
								CasinoGames._8PlayerGamePokerPot += CasinoGames._8PlayerGamePoker_AI1StartingBank / 100f;
								CasinoGames.RunBet(CasinoGames._8PlayerGamePokerPot);
								CasinoGames._8PlayerGamePoker_AI5Bank -= CasinoGames._8PlayerGamePoker_AI5StartingBank / 100f;
							}
							if (item3 == CasinoGames.AI_6 && !CasinoGames.AI6_Bust)
							{
								text29 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
								text30 = "bet_plus";
								CasinoGames.PlaySyncAnim(item3, item3, CasinoGames.AI6_SyncScene, text29, text30);
								Script.Wait(700);
								CasinoGames._8PlayerGamePokerPot += CasinoGames._8PlayerGamePoker_AI1StartingBank / 100f;
								CasinoGames.RunBet(CasinoGames._8PlayerGamePokerPot);
								CasinoGames._8PlayerGamePoker_AI6Bank -= CasinoGames._8PlayerGamePoker_AI6StartingBank / 100f;
							}
							if (item3 == CasinoGames.AI_7 && !CasinoGames.AI7_Bust)
							{
								text29 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
								text30 = "bet_plus";
								CasinoGames.PlaySyncAnim(item3, item3, CasinoGames.AI7_SyncScene, text29, text30);
								Script.Wait(700);
								CasinoGames._8PlayerGamePokerPot += CasinoGames._8PlayerGamePoker_AI1StartingBank / 100f;
								CasinoGames.RunBet(CasinoGames._8PlayerGamePokerPot);
								CasinoGames._8PlayerGamePoker_AI7Bank -= CasinoGames._8PlayerGamePoker_AI7StartingBank / 100f;
							}
							if (item3 == Game.Player.Character)
							{
								text29 = "anim_casino_b@amb@casino@games@threecardpoker@ped_female@engaged@01a@play@v01";
								text30 = "bet_plus";
								CasinoGames.PlaySyncAnim(item3, item3, CasinoGames.Player_SyncScene, text29, text30);
								Script.Wait(700);
								CasinoGames._8PlayerGamePokerPot += CasinoGames._8PlayerGamePoker_AI1StartingBank / 100f;
								CasinoGames.RunBet(CasinoGames._8PlayerGamePokerPot);
								CasinoGames._8PlayerGamePoker_PlayerBank -= CasinoGames._8PlayerGamePoker_PlayerStartingBank / 100f;
							}
						}
						CasinoGames.LastRaiseAmmount = CasinoGames._8PlayerGamePoker_AI1StartingBank / 100f;
						CasinoGames._8PlayerGameStage = 2;
					}
				}
				if (!Game.IsControlJustPressed(Control.Cover) || !(CasinoGames._8PlayerPokerGameTable1 != null) || !CasinoGames.GetDistance_VDIST2(Game.Player.Character.Position, CasinoGames._8PlayerPokerGameTable1.Position, CasinoGames.GetDistSign.Less, 5f))
				{
					return;
				}
				try
				{
					if (!CasinoGames.Playing8PlayerPoker)
					{
						if (CasinoGames._8PlayerGamePoker_PlayerStartingBank <= (float)CasinoGames.Chips_Amount)
						{
							CasinoGames.ResetCardDeck();
							CasinoGames.AI1_HasPlayedBettingRoundMove = false;
							CasinoGames.AI2_HasPlayedBettingRoundMove = false;
							CasinoGames.AI3_HasPlayedBettingRoundMove = false;
							CasinoGames.AI4_HasPlayedBettingRoundMove = false;
							CasinoGames.AI5_HasPlayedBettingRoundMove = false;
							CasinoGames.AI6_HasPlayedBettingRoundMove = false;
							CasinoGames.AI7_HasPlayedBettingRoundMove = false;
							CasinoGames.Player_IdleScenesPlayed = 0;
							CasinoGames.AI1_IdleScenesPlayed = 0;
							CasinoGames.AI2_IdleScenesPlayed = 0;
							CasinoGames.AI3_IdleScenesPlayed = 0;
							CasinoGames.AI4_IdleScenesPlayed = 0;
							CasinoGames.AI5_IdleScenesPlayed = 0;
							CasinoGames.AI6_IdleScenesPlayed = 0;
							CasinoGames.AI7_IdleScenesPlayed = 0;
							Random random3 = new Random();
							CasinoGames.Player_IdleScenesToPlayBeforeChange = random3.Next(3, 12);
							Script.Wait(1);
							CasinoGames.AI1_IdleScenesToPlayBeforeChange = random3.Next(3, 12);
							Script.Wait(1);
							CasinoGames.AI2_IdleScenesToPlayBeforeChange = random3.Next(3, 12);
							Script.Wait(1);
							CasinoGames.AI3_IdleScenesToPlayBeforeChange = random3.Next(3, 12);
							Script.Wait(1);
							CasinoGames.AI4_IdleScenesToPlayBeforeChange = random3.Next(3, 12);
							Script.Wait(1);
							CasinoGames.AI5_IdleScenesToPlayBeforeChange = random3.Next(3, 12);
							Script.Wait(1);
							CasinoGames.AI6_IdleScenesToPlayBeforeChange = random3.Next(3, 12);
							Script.Wait(1);
							CasinoGames.AI7_IdleScenesToPlayBeforeChange = random3.Next(3, 12);
							Script.Wait(1);
							Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
							CasinoGames.Chips_Amount -= (int)CasinoGames._8PlayerGamePoker_PlayerStartingBank;
							Class1.Config.SetValue("Setup", "Chips_Amount", CasinoGames.Chips_Amount);
							Class1.Config.Save();
							CasinoGames.StackChipHeight = 0.6f;
							CasinoGames.StackChip = 0;
							CasinoGames.Playing8PlayerPoker = true;
							CasinoGames._8PlayerGameDealer.Position = new Vector3(1136.207f, 236.0575f, -50.4f);
							CasinoGames._8PlayerGameDealer.Heading = -45f;
							if (CasinoGames.ChipsSpawn != null)
							{
								CasinoGames.ChipsSpawn.Delete();
							}
							if (CasinoGames.AI_1 != null)
							{
								CasinoGames.AI_1.Delete();
							}
							if (CasinoGames.AI_2 != null)
							{
								CasinoGames.AI_2.Delete();
							}
							if (CasinoGames.AI_3 != null)
							{
								CasinoGames.AI_3.Delete();
							}
							if (CasinoGames.AI_4 != null)
							{
								CasinoGames.AI_4.Delete();
							}
							if (CasinoGames.AI_5 != null)
							{
								CasinoGames.AI_5.Delete();
							}
							if (CasinoGames.AI_6 != null)
							{
								CasinoGames.AI_6.Delete();
							}
							if (CasinoGames.AI_7 != null)
							{
								CasinoGames.AI_7.Delete();
							}
							foreach (Ped _8PlayerGamePokerPed2 in CasinoGames._8PlayerGamePokerPeds)
							{
								if (_8PlayerGamePokerPed2 != null)
								{
									_8PlayerGamePokerPed2.Delete();
								}
							}
							if (CasinoGames._8PlayerGamePokerPeds.Count > 0)
							{
								CasinoGames._8PlayerGamePokerPeds.Clear();
							}
							foreach (Prop _8PlayerGamePokerProp2 in CasinoGames._8PlayerGamePokerProps)
							{
								if (_8PlayerGamePokerProp2 != null)
								{
									_8PlayerGamePokerProp2.Delete();
								}
							}
							foreach (Prop _8PlayerGamePokerPotChip2 in CasinoGames._8PlayerGamePokerPotChips)
							{
								if (_8PlayerGamePokerPotChip2 != null)
								{
									_8PlayerGamePokerPotChip2.Delete();
								}
							}
							if (CasinoGames.PlayerCards.Count > 0)
							{
								CasinoGames.PlayerCards.Clear();
							}
							if (CasinoGames.PlayerChips.Count > 0)
							{
								CasinoGames.PlayerChips.Clear();
							}
							CasinoGames._8PlayerGamePokerPot = 0f;
							if (CasinoGames._8PlayerGamePokerPotChips.Count > 0)
							{
								CasinoGames._8PlayerGamePokerPotChips.Clear();
							}
							if (CasinoGames._8PlayerGamePokerProps.Count > 0)
							{
								CasinoGames._8PlayerGamePokerProps.Clear();
							}
							CasinoGames._8PlayerGamePoker_AI1Bank = CasinoGames._8PlayerGamePoker_AI1StartingBank;
							CasinoGames._8PlayerGamePoker_AI2Bank = CasinoGames._8PlayerGamePoker_AI2StartingBank;
							CasinoGames._8PlayerGamePoker_AI3Bank = CasinoGames._8PlayerGamePoker_AI3StartingBank;
							CasinoGames._8PlayerGamePoker_AI4Bank = CasinoGames._8PlayerGamePoker_AI4StartingBank;
							CasinoGames._8PlayerGamePoker_AI5Bank = CasinoGames._8PlayerGamePoker_AI5StartingBank;
							CasinoGames._8PlayerGamePoker_AI6Bank = CasinoGames._8PlayerGamePoker_AI6StartingBank;
							CasinoGames._8PlayerGamePoker_AI7Bank = CasinoGames._8PlayerGamePoker_AI7StartingBank;
							CasinoGames._8PlayerGamePoker_PlayerBank = CasinoGames._8PlayerGamePoker_PlayerStartingBank;
							Script.Wait(100);
							string text31 = "anim_casino_b@amb@casino@games@shared@player@";
							CasinoGames.LoadDict("anim_casino_b@amb@casino@games@shared@player@");
							PedHash[] obj9 = (PedHash[])Enum.GetValues(typeof(PedHash));
							List<object> list36 = new List<object>();
							PedHash[] array = obj9;
							foreach (PedHash pedHash in array)
							{
								list36.Add(pedHash);
							}
							list36.Remove(PedHash.ChickenHawk);
							list36.Remove(PedHash.Cow);
							list36.Remove(PedHash.Seagull);
							list36.Remove(PedHash.Boar);
							list36.Remove(PedHash.Chimp);
							list36.Remove(PedHash.Rottweiler);
							list36.Remove(PedHash.Poodle);
							list36.Remove(PedHash.KillerWhale);
							list36.Remove(PedHash.HammerShark);
							list36.Remove(PedHash.TigerShark);
							list36.Remove(PedHash.Dolphin);
							list36.Remove(PedHash.Chop);
							list36.Remove(PedHash.Husky);
							list36.Remove(PedHash.Humpback);
							list36.Remove(PedHash.MountainLion);
							list36.Remove(PedHash.Pig);
							list36.Remove(PedHash.Pigeon);
							list36.Remove(PedHash.Poodle);
							list36.Remove(PedHash.Pug);
							list36.Remove(PedHash.Rabbit);
							list36.Remove(PedHash.Rat);
							list36.Remove(PedHash.Retriever);
							list36.Remove(PedHash.Cat);
							list36.Remove(PedHash.Cow);
							list36.Remove(PedHash.Coyote);
							list36.Remove(PedHash.CrisFormage);
							list36.Remove(PedHash.CrisFormageCutscene);
							list36.Remove(PedHash.Deer);
							list36.Remove(PedHash.Dolphin);
							list36.Remove(PedHash.Fish);
							list36.Remove(PedHash.Humpback);
							list36.Remove(PedHash.Retriever);
							list36.Remove(PedHash.Shepherd);
							list36.Remove(PedHash.Stingray);
							list36.Remove(PedHash.Chimp);
							list36.Remove(PedHash.Chop);
							list36.Remove(PedHash.Crow);
							list36.Remove(PedHash.Stingray);
							list36.Remove(PedHash.Stingray);
							list36.Remove(PedHash.Stingray);
							CasinoGames.RequestModel((dynamic)list36[random3.Next(0, list36.Count)]);
							CasinoGames.Player_SkipTurn = false;
							CasinoGames.AI1_SkipTurn = false;
							CasinoGames.AI2_SkipTurn = false;
							CasinoGames.AI3_SkipTurn = false;
							CasinoGames.AI4_SkipTurn = false;
							CasinoGames.AI5_SkipTurn = false;
							CasinoGames.AI6_SkipTurn = false;
							CasinoGames.AI7_SkipTurn = false;
							CasinoGames.Player_Bust = false;
							CasinoGames.AI1_Bust = false;
							CasinoGames.AI2_Bust = false;
							CasinoGames.AI3_Bust = false;
							CasinoGames.AI4_Bust = false;
							CasinoGames.AI5_Bust = false;
							CasinoGames.AI6_Bust = false;
							CasinoGames.AI7_Bust = false;
							CasinoGames.AI_1 = GlobalHelper.CreatePed(CasinoGames.RequestModel((dynamic)list36[random3.Next(0, list36.Count)]), new Vector3(1136.644f, 238.0772f, -50.5f), -156);
							CasinoGames.AI_1.IsVisible = false;
							CasinoGames.AI_2 = GlobalHelper.CreatePed(CasinoGames.RequestModel((dynamic)list36[random3.Next(0, list36.Count)]), new Vector3(1135.934f, 237.5755f, -50.5f), -117);
							CasinoGames.AI_2.IsVisible = false;
							CasinoGames.AI_3 = GlobalHelper.CreatePed(CasinoGames.RequestModel((dynamic)list36[random3.Next(0, list36.Count)]), new Vector3(1135.852f, 236.6536f, -50.5f), -69);
							CasinoGames.AI_3.IsVisible = false;
							CasinoGames.AI_4 = GlobalHelper.CreatePed(CasinoGames.RequestModel((dynamic)list36[random3.Next(0, list36.Count)]), new Vector3(1136.768f, 235.8449f, -50.5f), -27);
							CasinoGames.AI_4.IsVisible = false;
							CasinoGames.AI_5 = GlobalHelper.CreatePed(CasinoGames.RequestModel((dynamic)list36[random3.Next(0, list36.Count)]), new Vector3(1137.563f, 235.7726f, -50.5f), -25.5f);
							CasinoGames.AI_5.IsVisible = false;
							CasinoGames.AI_6 = GlobalHelper.CreatePed(CasinoGames.RequestModel((dynamic)list36[random3.Next(0, list36.Count)]), new Vector3(1138.273f, 236.2365f, -50.5f), 64);
							CasinoGames.AI_6.IsVisible = false;
							CasinoGames.AI_7 = GlobalHelper.CreatePed(CasinoGames.RequestModel((dynamic)list36[random3.Next(0, list36.Count)]), new Vector3(1138.334f, 237.1217f, -50.5f), 113);
							CasinoGames.AI_7.IsVisible = false;
							CasinoGames.AI_1.IsVisible = true;
							CasinoGames.AI_2.IsVisible = true;
							CasinoGames.AI_3.IsVisible = true;
							CasinoGames.AI_4.IsVisible = true;
							CasinoGames.AI_5.IsVisible = true;
							CasinoGames.AI_6.IsVisible = true;
							CasinoGames.AI_7.IsVisible = true;
							CasinoGames._8PlayerGamePokerPeds.Add(CasinoGames.AI_2);
							CasinoGames._8PlayerGamePokerPeds.Add(CasinoGames.AI_1);
							CasinoGames._8PlayerGamePokerPeds.Add(CasinoGames.AI_3);
							CasinoGames._8PlayerGamePokerPeds.Add(CasinoGames.AI_4);
							CasinoGames._8PlayerGamePokerPeds.Add(CasinoGames.AI_5);
							CasinoGames._8PlayerGamePokerPeds.Add(CasinoGames.AI_6);
							CasinoGames._8PlayerGamePokerPeds.Add(CasinoGames.AI_7);
							foreach (Ped _8PlayerGamePokerPed3 in CasinoGames._8PlayerGamePokerPeds)
							{
								if (_8PlayerGamePokerPed3 != null)
								{
									_8PlayerGamePokerPed3.RelationshipGroup = 2;
									_8PlayerGamePokerPed3.AlwaysKeepTask = true;
									_8PlayerGamePokerPed3.BlockPermanentEvents = true;
								}
							}
							if (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, text31))
							{
								Game.Player.Character.Position = new Vector3(1137.47f, 237.9809f, -50.5f);
								Prop _8PlayerPokerGameTable18 = CasinoGames._8PlayerPokerGameTable1;
								CasinoGames.Player_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, _8PlayerPokerGameTable18.Position.Z, 0.0, 0.0, _8PlayerPokerGameTable18.Rotation.Z + 10f, 2);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Player_SyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Player_SyncScene));
								CasinoGames.GetEnterAnim();
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Player_SyncScene, CasinoGames.LoadDict(text31), "sit_enter_left", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Player_SyncScene, "sit_enter_left", CasinoGames.LoadDict(text31), 16f, -16f, 0, 1148846080);
								CasinoGames.GetEnterAnim();
								CasinoGames.AI_1.Heading = -156f;
								CasinoGames.AI1_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.AI_1.Position.X, CasinoGames.AI_1.Position.Y, _8PlayerPokerGameTable18.Position.Z, 0.0, 0.0, CasinoGames.AI_1.Heading + 90f, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.AI_1, CasinoGames.AI1_SyncScene, CasinoGames.LoadDict(text31), "sit_enter_left_side", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.AI_1, CasinoGames.AI1_SyncScene, "sit_enter_left_side", CasinoGames.LoadDict(text31), 16f, -16f, 0, 1148846080);
								CasinoGames.AI_2.Heading = -118f;
								CasinoGames.AI2_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.AI_2.Position.X, CasinoGames.AI_2.Position.Y, _8PlayerPokerGameTable18.Position.Z, 0.0, 0.0, CasinoGames.AI_2.Heading + 90f, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.AI_2, CasinoGames.AI2_SyncScene, CasinoGames.LoadDict(text31), "sit_enter_left_side", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.AI_2, CasinoGames.AI2_SyncScene, "sit_enter_left_side", CasinoGames.LoadDict(text31), 16f, -16f, 0, 1148846080);
								CasinoGames.AI_3.Heading = -69f;
								CasinoGames.AI3_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.AI_3.Position.X, CasinoGames.AI_3.Position.Y, _8PlayerPokerGameTable18.Position.Z, 0.0, 0.0, CasinoGames.AI_3.Heading + 90f, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.AI_3, CasinoGames.AI3_SyncScene, CasinoGames.LoadDict(text31), "sit_enter_left_side", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.AI_3, CasinoGames.AI3_SyncScene, "sit_enter_left_side", CasinoGames.LoadDict(text31), 16f, -16f, 0, 1148846080);
								CasinoGames.AI_4.Heading = -28f;
								CasinoGames.AI4_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.AI_4.Position.X, CasinoGames.AI_4.Position.Y, _8PlayerPokerGameTable18.Position.Z, 0.0, 0.0, CasinoGames.AI_4.Heading + 90f, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.AI_4, CasinoGames.AI4_SyncScene, CasinoGames.LoadDict(text31), "sit_enter_left_side", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.AI_4, CasinoGames.AI4_SyncScene, "sit_enter_left_side", CasinoGames.LoadDict(text31), 16f, -16f, 0, 1148846080);
								CasinoGames.AI_5.Heading = 25.5f;
								CasinoGames.AI5_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.AI_5.Position.X, CasinoGames.AI_5.Position.Y, _8PlayerPokerGameTable18.Position.Z, 0.0, 0.0, CasinoGames.AI_5.Heading + 90f, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.AI_5, CasinoGames.AI5_SyncScene, CasinoGames.LoadDict(text31), "sit_enter_left_side", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.AI_5, CasinoGames.AI5_SyncScene, "sit_enter_left_side", CasinoGames.LoadDict(text31), 16f, -16f, 0, 1148846080);
								CasinoGames.AI6_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.AI_6.Position.X, CasinoGames.AI_6.Position.Y, _8PlayerPokerGameTable18.Position.Z, 0.0, 0.0, CasinoGames.AI_6.Heading + 90f, 2);
								CasinoGames.AI_6.Heading = 64f;
								CasinoGames.AI6_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.AI_6.Position.X, CasinoGames.AI_6.Position.Y, _8PlayerPokerGameTable18.Position.Z, 0.0, 0.0, CasinoGames.AI_6.Heading + 90f, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.AI_6, CasinoGames.AI6_SyncScene, CasinoGames.LoadDict(text31), "sit_enter_left_side", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.AI_6, CasinoGames.AI6_SyncScene, "sit_enter_left_side", CasinoGames.LoadDict(text31), 16f, -16f, 0, 1148846080);
								CasinoGames.AI_7.Heading = 113f;
								CasinoGames.AI7_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.AI_7.Position.X, CasinoGames.AI_7.Position.Y, _8PlayerPokerGameTable18.Position.Z, 0.0, 0.0, CasinoGames.AI_7.Heading + 90f, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.AI_7, CasinoGames.AI7_SyncScene, CasinoGames.LoadDict(text31), "sit_enter_left_side", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.AI_7, CasinoGames.AI7_SyncScene, "sit_enter_left_side", CasinoGames.LoadDict(text31), 16f, -16f, 0, 1148846080);
								Script.Wait(1000);
								CasinoGames.Player_SkipTurn = false;
								CasinoGames.Player_Bust = false;
								CasinoGames.AI1_SkipTurn = false;
								CasinoGames.AI1_Bust = false;
								CasinoGames.AI2_SkipTurn = false;
								CasinoGames.AI2_Bust = false;
								CasinoGames.AI3_SkipTurn = false;
								CasinoGames.AI3_Bust = false;
								CasinoGames.AI4_SkipTurn = false;
								CasinoGames.AI4_Bust = false;
								CasinoGames.AI5_SkipTurn = false;
								CasinoGames.AI5_Bust = false;
								CasinoGames.AI6_SkipTurn = false;
								CasinoGames.AI6_Bust = false;
								CasinoGames.AI7_SkipTurn = false;
								CasinoGames.AI7_Bust = false;
								if (CasinoGames.Player_VisualBank == null)
								{
									CasinoGames.Player_VisualBank = new CasinoGames._8PlayerPokerChipsBank(true, CasinoGames._8PlayerPokerGameTable1, CasinoGames._8PlayerPokerChair1_ChipOffset, CasinoGames._8PlayerGamePoker_PlayerStartingBank, 10, 2, 4, 4);
								}
								if (CasinoGames.AI_1_VisualBank == null)
								{
									CasinoGames.AI_1_VisualBank = new CasinoGames._8PlayerPokerChipsBank(true, CasinoGames._8PlayerPokerGameTable1, CasinoGames._8PlayerPokerChair2_ChipOffset, CasinoGames._8PlayerGamePoker_AI1StartingBank, 10, 2, 4, 4);
								}
								if (CasinoGames.AI_2_VisualBank == null)
								{
									CasinoGames.AI_2_VisualBank = new CasinoGames._8PlayerPokerChipsBank(true, CasinoGames._8PlayerPokerGameTable1, CasinoGames._8PlayerPokerChair3_ChipOffset, CasinoGames._8PlayerGamePoker_AI2StartingBank, 10, 2, 4, 4);
								}
								if (CasinoGames.AI_3_VisualBank == null)
								{
									CasinoGames.AI_3_VisualBank = new CasinoGames._8PlayerPokerChipsBank(true, CasinoGames._8PlayerPokerGameTable1, CasinoGames._8PlayerPokerChair4_ChipOffset, CasinoGames._8PlayerGamePoker_AI3StartingBank, 10, 2, 4, 4);
								}
								if (CasinoGames.AI_4_VisualBank == null)
								{
									CasinoGames.AI_4_VisualBank = new CasinoGames._8PlayerPokerChipsBank(true, CasinoGames._8PlayerPokerGameTable2, CasinoGames._8PlayerPokerChair1_ChipOffset, CasinoGames._8PlayerGamePoker_AI4StartingBank, 10, 2, 4, 4);
								}
								if (CasinoGames.AI_5_VisualBank == null)
								{
									CasinoGames.AI_5_VisualBank = new CasinoGames._8PlayerPokerChipsBank(true, CasinoGames._8PlayerPokerGameTable2, CasinoGames._8PlayerPokerChair2_ChipOffset, CasinoGames._8PlayerGamePoker_AI5StartingBank, 10, 2, 4, 4);
								}
								if (CasinoGames.AI_6_VisualBank == null)
								{
									CasinoGames.AI_6_VisualBank = new CasinoGames._8PlayerPokerChipsBank(true, CasinoGames._8PlayerPokerGameTable2, CasinoGames._8PlayerPokerChair3_ChipOffset, CasinoGames._8PlayerGamePoker_AI6StartingBank, 10, 2, 4, 4);
								}
								if (CasinoGames.AI_7_VisualBank == null)
								{
									CasinoGames.AI_7_VisualBank = new CasinoGames._8PlayerPokerChipsBank(true, CasinoGames._8PlayerPokerGameTable2, CasinoGames._8PlayerPokerChair4_ChipOffset, CasinoGames._8PlayerGamePoker_AI7StartingBank, 10, 2, 4, 4);
								}
								if (CasinoGames._8PlayerPokerPot == null)
								{
									int num32 = Function.Call<int>(Hash.GET_HASH_KEY, "prop_arm_wrestle_01");
									Vector3 position = Game.Player.Character.Position;
									Prop attachProp = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 10f, num32, 0, 0, 0);
									CasinoGames._8PlayerPokerPot = new CasinoGames._8PlayerPokerChipsBank(false, attachProp, new Vector3(0f, 0f, 0.55f), 0f, 10, 2, 6, 6);
								}
								CasinoGames._8PlayerGameStage = 1;
							}
						}
						else
						{
							Notification.Show(CasinoGames.GetHostName() + " You need atleast ~g~" + CasinoGames._8PlayerGamePoker_PlayerStartingBank.ToString("N") + "c~w~ (Chips), to play 8 Player Poker!");
						}
					}
					else
					{
						if (!CasinoGames.Playing8PlayerPoker)
						{
							return;
						}
						CasinoGames.ResetCardDeck();
						CasinoGames.AI1_HasPlayedBettingRoundMove = false;
						CasinoGames.AI2_HasPlayedBettingRoundMove = false;
						CasinoGames.AI3_HasPlayedBettingRoundMove = false;
						CasinoGames.AI4_HasPlayedBettingRoundMove = false;
						CasinoGames.AI5_HasPlayedBettingRoundMove = false;
						CasinoGames.AI6_HasPlayedBettingRoundMove = false;
						CasinoGames.AI7_HasPlayedBettingRoundMove = false;
						CasinoGames.Player_IdleScenesPlayed = 0;
						CasinoGames.AI1_IdleScenesPlayed = 0;
						CasinoGames.AI2_IdleScenesPlayed = 0;
						CasinoGames.AI3_IdleScenesPlayed = 0;
						CasinoGames.AI4_IdleScenesPlayed = 0;
						CasinoGames.AI5_IdleScenesPlayed = 0;
						CasinoGames.AI6_IdleScenesPlayed = 0;
						CasinoGames.AI7_IdleScenesPlayed = 0;
						if (CasinoGames._8PlayerPokerPot != null)
						{
							CasinoGames._8PlayerPokerPot.DeleteAllChips();
						}
						if (CasinoGames.Player_VisualBank != null)
						{
							CasinoGames.Player_VisualBank.DeleteAllChips();
						}
						if (CasinoGames.AI_1_VisualBank != null)
						{
							CasinoGames.AI_1_VisualBank.DeleteAllChips();
						}
						if (CasinoGames.AI_2_VisualBank != null)
						{
							CasinoGames.AI_2_VisualBank.DeleteAllChips();
						}
						if (CasinoGames.AI_3_VisualBank != null)
						{
							CasinoGames.AI_3_VisualBank.DeleteAllChips();
						}
						if (CasinoGames.AI_4_VisualBank != null)
						{
							CasinoGames.AI_4_VisualBank.DeleteAllChips();
						}
						if (CasinoGames.AI_5_VisualBank != null)
						{
							CasinoGames.AI_5_VisualBank.DeleteAllChips();
						}
						if (CasinoGames.AI_6_VisualBank != null)
						{
							CasinoGames.AI_6_VisualBank.DeleteAllChips();
						}
						if (CasinoGames.AI_7_VisualBank != null)
						{
							CasinoGames.AI_7_VisualBank.DeleteAllChips();
						}
						CasinoGames.Chips_Amount += (int)CasinoGames._8PlayerGamePoker_PlayerBank;
						Class1.Config.SetValue("Setup", "Chips_Amount", CasinoGames.Chips_Amount);
						Class1.Config.Save();
						Notification.Show(CasinoGames.GetHostName() + " You have walked away with ~g~" + CasinoGames._8PlayerGamePoker_PlayerBank.ToString("N") + "c~w~ (Chips), from your recent 8 Player Poker Match!");
						CasinoGames.StackChipHeight = 0.6f;
						CasinoGames.StackChip = 0;
						string dict20 = "anim_casino_b@amb@casino@games@shared@player@";
						CasinoGames.Playing8PlayerPoker = false;
						Prop _8PlayerPokerGameTable19 = CasinoGames._8PlayerPokerGameTable1;
						CasinoGames.Player_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, _8PlayerPokerGameTable19.Position.Z, 0.0, 0.0, _8PlayerPokerGameTable19.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Player_SyncScene, CasinoGames.LoadDict(dict20), "sit_exit_left", 16f, -16f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Player_SyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Player_SyncScene));
						string exitAnim = CasinoGames.GetExitAnim();
						exitAnim = CasinoGames.GetExitAnim();
						CasinoGames.PlaySyncExitAnim(CasinoGames.AI_1, _8PlayerPokerGameTable19, CasinoGames.AI1_SyncScene, dict20, exitAnim);
						CasinoGames.PlaySyncExitAnim(CasinoGames.AI_2, _8PlayerPokerGameTable19, CasinoGames.AI2_SyncScene, dict20, exitAnim);
						CasinoGames.PlaySyncExitAnim(CasinoGames.AI_3, _8PlayerPokerGameTable19, CasinoGames.AI3_SyncScene, dict20, exitAnim);
						CasinoGames.PlaySyncExitAnim(CasinoGames.AI_4, _8PlayerPokerGameTable19, CasinoGames.AI4_SyncScene, dict20, exitAnim);
						CasinoGames.PlaySyncExitAnim(CasinoGames.AI_5, _8PlayerPokerGameTable19, CasinoGames.AI5_SyncScene, dict20, exitAnim);
						CasinoGames.PlaySyncExitAnim(CasinoGames.AI_6, _8PlayerPokerGameTable19, CasinoGames.AI6_SyncScene, dict20, exitAnim);
						CasinoGames.PlaySyncExitAnim(CasinoGames.AI_7, _8PlayerPokerGameTable19, CasinoGames.AI7_SyncScene, dict20, exitAnim);
						Script.Wait(4000);
						Game.Player.Character.Task.ClearAll();
						if (CasinoGames.AI_1 != null)
						{
							CasinoGames.AI_1.Task.WanderAround();
						}
						if (CasinoGames.AI_2 != null)
						{
							CasinoGames.AI_2.Task.WanderAround();
						}
						if (CasinoGames.AI_3 != null)
						{
							CasinoGames.AI_3.Task.WanderAround();
						}
						if (CasinoGames.AI_4 != null)
						{
							CasinoGames.AI_4.Task.WanderAround();
						}
						if (CasinoGames.AI_5 != null)
						{
							CasinoGames.AI_5.Task.WanderAround();
						}
						if (CasinoGames.AI_6 != null)
						{
							CasinoGames.AI_6.Task.WanderAround();
						}
						if (CasinoGames.AI_7 != null)
						{
							CasinoGames.AI_7.Task.WanderAround();
						}
						{
							foreach (Ped _8PlayerGamePokerPed4 in CasinoGames._8PlayerGamePokerPeds)
							{
								if (_8PlayerGamePokerPed4 != null)
								{
									_8PlayerGamePokerPed4.Task.WanderAround();
								}
							}
							return;
						}
					}
					return;
				}
				catch
				{
					CasinoGames.Playing8PlayerPoker = false;
					Notification.Show("~r~ Error ~w~ An Error occurred when starting 8 Player Poker, please try again");
					return;
				}
			}
			_Debug.EightPlayerPoker = 0;
		}
	}
}
