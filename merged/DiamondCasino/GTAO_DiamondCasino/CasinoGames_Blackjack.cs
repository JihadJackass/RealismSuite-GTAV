using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace GTAO_DiamondCasino
{
	internal class CasinoGames_Blackjack : Script
	{
		public static bool Debug;

		public static int Get_Left_AHandTotal()
		{
			return CasinoGames.PlayerCard_1 + CasinoGames.PlayerCard_3 + CasinoGames.PlayerCard_5;
		}

		public static int Get_Right_B_HandTotal()
		{
			return CasinoGames.PlayerCard_2 + CasinoGames.PlayerCard_4 + CasinoGames.PlayerCard_6;
		}

		public CasinoGames_Blackjack()
		{
			if (_Debug.CasinoGames_Blackjack != -1)
			{
				base.Tick += OnTick;
			}
		}

		public void OnTick(object sender, EventArgs e)
		{
			_Debug.CasinoGames_Blackjack = 0;
			if (CasinoGames.CurrentGame != CasinoGames.CasinoGamePlaying.Blackjack || CasinoGames.CurrentGame != CasinoGames.CasinoGamePlaying.Blackjack)
			{
				return;
			}
			_Debug.CasinoGames_Blackjack = 1;
			if (CasinoGames.CurrentGame != CasinoGames.CasinoGamePlaying.Blackjack)
			{
				return;
			}
			if (!CasinoGames.Playing_BlackJack)
			{
				CasinoGames.CurrentGame = CasinoGames.CasinoGamePlaying.None;
			}
			if (!CasinoGames.Playing_BlackJack)
			{
				return;
			}
			CasinoGames.Blackjack_ManageDealerCards();
			if (Game.IsControlJustPressed(Control.VehicleDuck) && Game.Player.Name.ToUpper().Equals("CINNBARGNU"))
			{
				Debug = !Debug;
			}
			if (Debug)
			{
				CasinoGames.drawText("Stage " + CasinoGames.Stage + "\n" + CasinoGames.ReturnClosestBetAmtFromCustomBet((int)CasinoGames.Bet) + "\n" + Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, CasinoGames.ClosestDealer, "anim_casino_b@amb@casino@games@blackjack@dealer", "deal_card_player_0" + CasinoGames.SeatSittingIn, 3) + "\nCards " + CasinoGames.Playercards + "/" + CasinoGames.DealerCards + "\nSum: " + (CasinoGames.ClosestDealerTable.Card1num + CasinoGames.ClosestDealerTable.Card2num + CasinoGames.ClosestDealerTable.Card3num + CasinoGames.ClosestDealerTable.Card4num + CasinoGames.ClosestDealerTable.Card5num + CasinoGames.ClosestDealerTable.Card6num) + "\nSum/W C1: " + (CasinoGames.ClosestDealerTable.Card2num + CasinoGames.ClosestDealerTable.Card3num + CasinoGames.ClosestDealerTable.Card4num + CasinoGames.ClosestDealerTable.Card5num + CasinoGames.ClosestDealerTable.Card6num) + "\nSum/W C1/C2 + C3: " + (CasinoGames.ClosestDealerTable.Card2num + CasinoGames.ClosestDealerTable.Card3num), 0.1f, 0.1f, 0.3f, 255, 255, 255);
				CasinoGames.drawText("Dealer Cards \n" + CasinoGames.DealerPresetCars[CasinoGames.PresetChoosen].Card1 + "/" + CasinoGames.DealerCard_1 + "\n" + CasinoGames.DealerPresetCars[CasinoGames.PresetChoosen].Card2 + "/" + CasinoGames.DealerCard_2 + "\n" + CasinoGames.DealerPresetCars[CasinoGames.PresetChoosen].Card3 + "/" + CasinoGames.DealerCard_3, 0.1f, 0.3f, 0.3f, 255, 255, 255);
				CasinoGames.drawText(CasinoGames.DealerPresetCars[CasinoGames.PresetChoosen].Card4 + "/" + CasinoGames.DealerCard_4 + "\n" + CasinoGames.DealerPresetCars[CasinoGames.PresetChoosen].Card5 + "/" + CasinoGames.DealerCard_5 + "\n" + CasinoGames.DealerPresetCars[CasinoGames.PresetChoosen].Card6 + "/" + CasinoGames.DealerCard_6, 0.1f, 0.4f, 0.3f, 255, 255, 255);
			}
			CasinoGames.ShowCards(false);
			CasinoGames.GetClosesBlackjackTable().GetOffsetPosition(new Vector3(0.17f, -0.23f, 0.98f));
			CasinoGames.GetClosesBlackjackTable().GetOffsetPosition(new Vector3(0.185f, -0.214f, 0.983f));
			Vector3 offsetPosition = CasinoGames.GetClosesBlackjackTable().GetOffsetPosition(new Vector3(0.195f, -0.19f, 0.986f));
			if (CasinoGames.GetClosesBlackjackTable() != null)
			{
				Prop closesBlackjackTable = CasinoGames.GetClosesBlackjackTable();
				if (CasinoGames.DealerCards >= 2)
				{
					if (Game.IsControlJustPressed(Control.VehicleExit))
					{
						if (!CasinoGames.LookingAtDealerCards)
						{
							if (CasinoGames.RoulleteCam != null)
							{
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 1, 3000, 1, 0);
								CasinoGames.RoulleteCam.IsActive = false;
								CasinoGames.RoulleteCam.Delete();
							}
							CasinoGames.RoulleteCam = GlobalHelper.CreateCamera(closesBlackjackTable.GetOffsetPosition(new Vector3(0f, 0.1f, 1.35f)), new Vector3(-90f, 0f, closesBlackjackTable.Heading), 60f);
							CasinoGames.RoulleteCam.IsActive = true;
							CasinoGames.RoulleteCam.FarClip = 2000f;
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 1, 3000, 1, 0);
							CasinoGames.LookingAtDealerCards = true;
						}
						else
						{
							CasinoGames.LookingAtDealerCards = false;
							if (CasinoGames.RoulleteCam != null)
							{
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 1, 3000, 1, 0);
								CasinoGames.RoulleteCam.IsActive = false;
								CasinoGames.RoulleteCam.Delete();
							}
						}
					}
				}
				else
				{
					CasinoGames.LookingAtDealerCards = false;
					if (CasinoGames.RoulleteCam != null)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 1, 3000, 1, 0);
						CasinoGames.RoulleteCam.IsActive = false;
						CasinoGames.RoulleteCam.Delete();
					}
				}
			}
			Game.Player.Character.GetOffsetPosition(new Vector3(0.4f, 0.6f, 0f));
			Game.Player.Character.GetOffsetPosition(new Vector3(0.4f, 0.6f, -0.45f));
			Game.Player.Character.GetOffsetPosition(new Vector3(0.4f, 0.7f, -0.45f));
			if (CasinoGames.BlackjackPlayerInitialBet != null)
			{
				CasinoGames.BlackjackPlayerInitialBet.Position = CasinoGames.ClosestDealerTable.Table.GetOffsetPosition(CasinoGames.GetPlayerBetPositionZOffset(CasinoGames.ReturnClosestBetAmtFromCustomBet((int)CasinoGames.Bet), 0, CasinoGames.SeatSittingIn - 1, true));
				CasinoGames.BlackjackPlayerInitialBet.Rotation = CasinoGames.ClosestDealerTable.Table.Rotation + CasinoGames.GetPlayerBetRotationOffset(CasinoGames.ReturnClosestBetAmtFromCustomBet((int)CasinoGames.Bet), 0, CasinoGames.SeatSittingIn - 1, true);
			}
			if (CasinoGames.Endgame)
			{
				if (CasinoGames.Stage == 50f)
				{
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 0.99f)
					{
						string dict = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@idles";
						Random random = new Random();
						List<string> list = new List<string> { "idle_a", "idle_b", "idle_c", "idle_d" };
						string text = list[random.Next(0, list.Count)];
						CasinoGames.LoadDict(dict);
						CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading - 90f, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict(dict), text, 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, text, CasinoGames.LoadDict(dict), 16f, -16f, 0, 1148846080);
						string dict2 = "anim_casino_b@amb@casino@games@blackjack@dealer";
						string text2 = "idle";
						Vector3 dealerAnimOrigin = CasinoGames.GetDealerAnimOrigin();
						CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin.X, dealerAnimOrigin.Y, dealerAnimOrigin.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene1, CasinoGames.LoadDict(dict2), text2, 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene1, text2, CasinoGames.LoadDict(dict2), 16f, -16f, 0, 1148846080);
					}
					if (Game.IsControlJustPressed(Control.Cover))
					{
						string dict3 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@engaged@01b@play@v02";
						string text3 = "collect_chips";
						CasinoGames.LoadDict(dict3);
						CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z, 0.0, 0.0, CasinoGames.PlayerOriginHeading - 90f, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict(dict3), text3, 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, text3, CasinoGames.LoadDict(dict3), 16f, -16f, 0, 1148846080);
						string dict4 = "anim_casino_b@amb@casino@games@shared@dealer@";
						string text4 = "acknowledge_p01";
						Vector3 dealerAnimOrigin2 = CasinoGames.GetDealerAnimOrigin();
						CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin2.X, dealerAnimOrigin2.Y, dealerAnimOrigin2.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene1, CasinoGames.LoadDict(dict4), text4, 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene1, text4, CasinoGames.LoadDict(dict4), 16f, -16f, 0, 1148846080);
						CasinoGames.Stage = 100f;
						Script.Wait(1);
					}
				}
				if (CasinoGames.Stage == 100f && Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene1) >= 0.99f)
				{
					CasinoGames.BlackJackSplit = false;
					CasinoGames.BlackJackDoubleDown = false;
					CasinoGames.ResetCardDeck();
					CasinoGames.DealerCardsDrawn = 0;
					CasinoGames.Stage = 0f;
					CasinoGames.TimeSinceLastCard = Game.GameTime + 5000;
					Vector3 position = CasinoGames.Backface_DealerCard1.Position;
					Vector3 rotation = CasinoGames.Backface_DealerCard1.Rotation;
					if (CasinoGames.Backface_DealerCard1 != null)
					{
						CasinoGames.Backface_DealerCard1.Delete();
					}
					if (CasinoGames.ClosestDealerTable.Card1 != null)
					{
						CasinoGames.ClosestDealerTable.Card1.Delete();
					}
					CasinoGames.ClosestDealerTable.Card1 = GlobalHelper.CreateProp(CasinoGames.RequestModel(CasinoGames.FrontFace_DealerCard1), position, rotation, false, false);
					foreach (DealerTable blackjackDealerTable in CasinoGames.BlackjackDealerTables)
					{
						if (blackjackDealerTable.Dealer != null)
						{
							CasinoGames.ResetDealerPos(blackjackDealerTable);
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
					foreach (Prop chip in CasinoGames.Chips)
					{
						if (chip != null)
						{
							chip.Delete();
						}
					}
					CasinoGames.Placebet2 = false;
					CasinoGames.Placebet = false;
					CasinoGames.Drawcards = false;
					CasinoGames.Endgame = false;
					CasinoGames.Playercards = 0;
					CasinoGames.DealerCards = 0;
					CasinoGames.PlayerCard_1 = 0;
					CasinoGames.PlayerCard_2 = 0;
					CasinoGames.PlayerCard_3 = 0;
					CasinoGames.PlayerCard_4 = 0;
					CasinoGames.PlayerCard_5 = 0;
					CasinoGames.PlayerCard_6 = 0;
					CasinoGames.Bet = CasinoGames.InitalBet;
					EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(CasinoGames.BlackjackPlayerInitialBet, false, 0));
					Prop prop = GlobalHelper.CreateProp(CasinoGames.RequestModel(CasinoGames.GetBetChipModel((int)CasinoGames.Bet, true)), CasinoGames.ReturnChipPosition(CasinoGames.BlackjackDealerTables, CasinoGames.BetTypeId.BlackJack), true, false);
					prop.IsPositionFrozen = true;
					prop.Position = CasinoGames.GetPlayerBetPositionZOffset((int)CasinoGames.Bet, 0, CasinoGames.SeatSittingIn - 1, true);
					prop.Rotation = CasinoGames.GetPlayerBetRotationOffset((int)CasinoGames.Bet, 0, CasinoGames.SeatSittingIn - 1, true);
					CasinoGames.Chips.Add(prop);
					CasinoGames.BlackjackPlayerInitialBet = prop;
					CasinoGames.DealerCard_1 = 0;
					CasinoGames.DealerCard_2 = 0;
					CasinoGames.DealerCard_3 = 0;
					CasinoGames.DealerCard_4 = 0;
					CasinoGames.DealerCard_5 = 0;
					CasinoGames.DealerCard_6 = 0;
					CasinoGames.Stage = 0f;
					CasinoGames.TimeSinceLastCard = Game.GameTime + 5000;
					return;
				}
			}
			if (CasinoGames.Placebet)
			{
				if (CasinoGames.ClosestDealerTable.Card2num + CasinoGames.ClosestDealerTable.Card3num + CasinoGames.BlackjackBackupDealerCard1_ThreeCards < 21 && CasinoGames.BlackjackBackupDealerCard1_ThreeCards < 11)
				{
					CasinoGames.BlackjackBackupDealerCard1_ThreeCards++;
				}
				if (CasinoGames.ClosestDealerTable.Card2num + CasinoGames.ClosestDealerTable.Card3num + CasinoGames.ClosestDealerTable.Card4num + CasinoGames.BlackjackBackupDealerCard1_FourCards < 21 && CasinoGames.BlackjackBackupDealerCard1_FourCards < 11)
				{
					CasinoGames.BlackjackBackupDealerCard1_FourCards++;
				}
				if (CasinoGames.ClosestDealerTable.Card2num + CasinoGames.ClosestDealerTable.Card3num + CasinoGames.ClosestDealerTable.Card4num + CasinoGames.ClosestDealerTable.Card5num + CasinoGames.BlackjackBackupDealerCard1_FiveCards < 21 && CasinoGames.BlackjackBackupDealerCard1_FiveCards < 11)
				{
					CasinoGames.BlackjackBackupDealerCard1_FiveCards++;
				}
				int num = CasinoGames.ClosestDealerTable.Card1num + CasinoGames.ClosestDealerTable.Card2num;
				int num2 = CasinoGames.ClosestDealerTable.Card1num + CasinoGames.ClosestDealerTable.Card2num;
				if (CasinoGames.Playercards == 1)
				{
					num = CasinoGames.ClosestDealerTable.Card1num;
					num2 = CasinoGames.ClosestDealerTable.Card1num;
				}
				if (CasinoGames.Playercards == 2)
				{
					num = CasinoGames.ClosestDealerTable.Card1num + CasinoGames.ClosestDealerTable.Card2num;
					num2 = CasinoGames.ClosestDealerTable.Card1num + CasinoGames.ClosestDealerTable.Card2num;
				}
				if (CasinoGames.Playercards == 3)
				{
					num = CasinoGames.ClosestDealerTable.Card1num + CasinoGames.ClosestDealerTable.Card2num + CasinoGames.ClosestDealerTable.Card3num;
					num2 = CasinoGames.BlackjackBackupDealerCard1_ThreeCards + CasinoGames.ClosestDealerTable.Card2num + CasinoGames.ClosestDealerTable.Card3num;
				}
				if (CasinoGames.Playercards == 4)
				{
					num = CasinoGames.ClosestDealerTable.Card1num + CasinoGames.ClosestDealerTable.Card2num + CasinoGames.ClosestDealerTable.Card3num + CasinoGames.ClosestDealerTable.Card4num;
					num2 = CasinoGames.BlackjackBackupDealerCard1_FourCards + CasinoGames.ClosestDealerTable.Card2num + CasinoGames.ClosestDealerTable.Card3num + CasinoGames.ClosestDealerTable.Card4num;
				}
				if (CasinoGames.Playercards == 5)
				{
					num = CasinoGames.ClosestDealerTable.Card1num + CasinoGames.ClosestDealerTable.Card2num + CasinoGames.ClosestDealerTable.Card3num + CasinoGames.ClosestDealerTable.Card4num + CasinoGames.ClosestDealerTable.Card5num;
					num2 = CasinoGames.BlackjackBackupDealerCard1_FiveCards + CasinoGames.ClosestDealerTable.Card2num + CasinoGames.ClosestDealerTable.Card3num + CasinoGames.ClosestDealerTable.Card4num + CasinoGames.ClosestDealerTable.Card5num;
				}
				if (CasinoGames.Playercards == 6)
				{
					num = CasinoGames.ClosestDealerTable.Card1num + CasinoGames.ClosestDealerTable.Card2num + CasinoGames.ClosestDealerTable.Card3num + CasinoGames.ClosestDealerTable.Card4num + CasinoGames.ClosestDealerTable.Card5num + CasinoGames.ClosestDealerTable.Card6num;
					num2 = CasinoGames.ClosestDealerTable.Card1num + CasinoGames.ClosestDealerTable.Card2num + CasinoGames.ClosestDealerTable.Card3num + CasinoGames.ClosestDealerTable.Card4num + CasinoGames.ClosestDealerTable.Card5num + CasinoGames.ClosestDealerTable.Card6num;
				}
				CasinoGames.DealerCard_1 = CasinoGames.ClosestDealerTable.Card1num;
				CasinoGames.DealerCard_2 = CasinoGames.ClosestDealerTable.Card2num;
				if (CasinoGames.Playercards < 2)
				{
					CasinoGames.DealerCard_2 = 0;
				}
				CasinoGames.DealerCard_3 = CasinoGames.ClosestDealerTable.Card3num;
				if (CasinoGames.Playercards < 3)
				{
					CasinoGames.DealerCard_3 = 0;
				}
				CasinoGames.DealerCard_4 = CasinoGames.ClosestDealerTable.Card4num;
				if (CasinoGames.Playercards < 4)
				{
					CasinoGames.DealerCard_4 = 0;
				}
				CasinoGames.DealerCard_5 = CasinoGames.ClosestDealerTable.Card5num;
				if (CasinoGames.Playercards < 5)
				{
					CasinoGames.DealerCard_5 = 0;
				}
				CasinoGames.DealerCard_6 = CasinoGames.ClosestDealerTable.Card6num;
				if (CasinoGames.Playercards < 6)
				{
					CasinoGames.DealerCard_6 = 0;
				}
				int num3 = CasinoGames.DealerCard_1 + CasinoGames.DealerCard_2 + CasinoGames.DealerCard_3 + CasinoGames.DealerCard_4 + CasinoGames.DealerCard_5 + CasinoGames.DealerCard_6;
				if (Debug)
				{
					CasinoGames.drawText("Dealer Sum " + num + "/" + num2 + "/" + num3, 0.1f, 0.55f, 0.3f, 255, 255, 255);
					CasinoGames.drawText("Backup Override \n" + (CasinoGames.ClosestDealerTable.Card2num + CasinoGames.ClosestDealerTable.Card3num + CasinoGames.ClosestDealerTable.Card4num + CasinoGames.ClosestDealerTable.Card5num) + "/" + CasinoGames.BlackjackBackupDealerCard1_FiveCards + "\n" + (CasinoGames.ClosestDealerTable.Card2num + CasinoGames.ClosestDealerTable.Card3num + CasinoGames.ClosestDealerTable.Card4num) + "/" + CasinoGames.BlackjackBackupDealerCard1_FourCards + "\n" + (CasinoGames.ClosestDealerTable.Card2num + CasinoGames.ClosestDealerTable.Card3num) + "/" + CasinoGames.BlackjackBackupDealerCard1_ThreeCards, 0.1f, 0.46f, 0.3f, 255, 255, 255);
				}
				if (!CasinoGames.Endgame)
				{
					if (Game.IsControlJustPressed(Control.VehicleHandbrake) && !CasinoGames.BlackJackSplit && CasinoGames.Playercards == 2)
					{
						CasinoGames.DetatchAllCards();
						string dict5 = "anim_casino_b@amb@casino@games@blackjack@player";
						string text5 = "place_bet_large_split";
						CasinoGames.LoadDict(dict5);
						CasinoGames.GetPlayerAnimOriginH(CasinoGames.SeatSittingIn, CasinoGames.TableType.Blackjack);
						CasinoGames.GetPlayerAnimOriginH(CasinoGames.SeatSittingIn, CasinoGames.TableType.Blackjack);
						CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict(dict5), text5, 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, text5, CasinoGames.LoadDict(dict5), 16f, -16f, 0, 1148846080);
						CasinoGames.Stage = 15f;
					}
					if (Game.IsControlJustPressed(Control.Context) && !CasinoGames.BlackJackDoubleDown && CasinoGames.Playercards >= 2 && CasinoGames.Playercards <= 4)
					{
						CasinoGames.DetatchAllCards();
						string dict6 = "anim_casino_b@amb@casino@games@blackjack@player";
						string text6 = "place_bet_double_down";
						CasinoGames.LoadDict(dict6);
						CasinoGames.GetPlayerAnimOriginH(CasinoGames.SeatSittingIn, CasinoGames.TableType.Blackjack);
						CasinoGames.GetPlayerAnimOriginH(CasinoGames.SeatSittingIn, CasinoGames.TableType.Blackjack);
						CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict(dict6), text6, 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, text6, CasinoGames.LoadDict(dict6), 16f, -16f, 0, 1148846080);
						CasinoGames.BlackJackDoubleDown = true;
						CasinoGames.Stage = 35f;
						CasinoGames.Bet *= 2f;
					}
					if (Game.IsControlJustPressed(Control.ThrowGrenade))
					{
						if (CasinoGames.Playercards >= 2)
						{
							CasinoGames.DetatchAllCards();
							CasinoGames.OverrideDealer1stCard();
							CasinoGames.BlackJackCard1 = "";
							CasinoGames.BlackJackCard2 = "";
							CasinoGames.BlackJackCard3 = "";
							CasinoGames.BlackJackCard4 = "";
							CasinoGames.BlackJackCard5 = "";
							CasinoGames.Placebet2 = false;
							CasinoGames.Drawcards = true;
							new Random().Next(0, 2000);
							if (CasinoGames.Playercards >= 4)
							{
								CasinoGames.DealerCard_4 = CasinoGames.ClosestDealerTable.Card4num;
							}
							CasinoGames.DealerCard_5 = 0;
							if (CasinoGames.Playercards >= 5)
							{
								CasinoGames.DealerCard_5 = CasinoGames.ClosestDealerTable.Card5num;
							}
							CasinoGames.DealerCard_5 = 0;
							if (CasinoGames.Playercards >= 6)
							{
								CasinoGames.DealerCard_6 = CasinoGames.ClosestDealerTable.Card6num;
							}
							CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading - 90f, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base"), "base", 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, "base", CasinoGames.LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base"), 16f, -16f, 0, 1148846080);
							Vector3 position2 = CasinoGames.Backface_DealerCard1.Position;
							Vector3 rotation2 = CasinoGames.Backface_DealerCard1.Rotation;
							if (CasinoGames.Backface_DealerCard1 != null)
							{
								CasinoGames.Backface_DealerCard1.Delete();
							}
							if (CasinoGames.ClosestDealerTable.Card1 != null)
							{
								CasinoGames.ClosestDealerTable.Card1.Delete();
							}
							CasinoGames.ClosestDealerTable.Card1 = GlobalHelper.CreateProp(CasinoGames.RequestModel(CasinoGames.FrontFace_DealerCard1), position2, rotation2, false, false);
							string dict7 = "anim_casino_b@amb@casino@games@blackjack@dealer";
							string text7 = "check_and_turn_card";
							Vector3 dealerAnimOrigin3 = CasinoGames.GetDealerAnimOrigin();
							CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin3.X, dealerAnimOrigin3.Y, dealerAnimOrigin3.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene1, CasinoGames.LoadDict(dict7), text7, 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene1, text7, CasinoGames.LoadDict(dict7), 16f, -16f, 0, 1148846080);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.ClosestDealerTable.Card1, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							Script.Wait(1);
							if (CasinoGames.DealerCards < CasinoGames.Playercards)
							{
								CasinoGames.Stage = 150f;
							}
							else
							{
								CasinoGames.Stage = 300f;
							}
						}
						else
						{
							string dict8 = "anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base";
							string text8 = "base";
							CasinoGames.LoadDict(dict8);
							Vector3 position11 = Game.Player.Character.Position;
							CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading - 90f, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict(dict8), text8, 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, text8, CasinoGames.LoadDict(dict8), 16f, -16f, 0, 1148846080);
							Notification.Show("You cannot Accept untill you have a hand of at least 2");
						}
						CasinoGames.ResetCardDeck();
					}
					if (Game.IsControlJustPressed(Control.Cover) || CasinoGames.Stage == 11f)
					{
						if (CasinoGames.Stage == 11f)
						{
							CasinoGames.Stage = 0f;
							CasinoGames.TimeSinceLastCard = Game.GameTime + 5000;
						}
						if (CasinoGames.Playercards >= 6)
						{
							Notification.Show("~p~Dealer~w~: You cannot get anymore cards, please accept your hand");
						}
						if (CasinoGames.Stage == 2f && !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, CasinoGames.ClosestDealer, "anim_casino_b@amb@casino@games@blackjack@dealer", "deal_card_player_0" + CasinoGames.SeatSittingIn, 3))
						{
							CasinoGames.Stage = 0f;
							CasinoGames.TimeSinceLastCard = Game.GameTime + 5000;
						}
						if (CasinoGames.Stage == 0f)
						{
							if (Game.GameTime >= CasinoGames.TimeSinceLastCard)
							{
								CasinoGames.TimeSinceLastCard = Game.GameTime + 5000;
								Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.BlackjackDealerTables), "MINIGAME_BJACK_DEALER_ANOTHER_CARD", "SPEECH_PARAMS_STANDARD", false);
							}
							string dict9 = "anim_casino_b@amb@casino@games@blackjack@player";
							string text9 = "request_card";
							float num4 = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 100f);
							if (num4 >= (float)(20 + 10 * CasinoGames.Playercards))
							{
								text9 = "request_card_alt1";
							}
							if (num4 >= (float)(40 + 10 * CasinoGames.Playercards))
							{
								text9 = "request_card_alt2";
							}
							CasinoGames.LoadDict(dict9);
							CasinoGames.GetPlayerAnimOriginH(CasinoGames.SeatSittingIn, CasinoGames.TableType.Blackjack);
							CasinoGames.GetPlayerAnimOriginH(CasinoGames.SeatSittingIn, CasinoGames.TableType.Blackjack);
							CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene1, CasinoGames.LoadDict(dict9), text9, 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene1, text9, CasinoGames.LoadDict(dict9), 16f, -16f, 0, 1148846080);
							CasinoGames.DetatchAllCards();
							CasinoGames.Playercards++;
							dict9 = "anim_casino_b@amb@casino@games@blackjack@dealer";
							text9 = "deal_card_player_0" + CasinoGames.SeatSittingIn;
							if (CasinoGames.BlackJackSplit)
							{
								text9 = "hit_second_card_player_0" + CasinoGames.SeatSittingIn;
								if (CasinoGames.Playercards == 1)
								{
									text9 = "deal_card_player_0" + CasinoGames.SeatSittingIn;
								}
								if (CasinoGames.Playercards == 2)
								{
									text9 = "hit_second_card_player_0" + CasinoGames.SeatSittingIn;
								}
								if (CasinoGames.Playercards == 3)
								{
									text9 = "deal_card_player_0" + CasinoGames.SeatSittingIn;
								}
								if (CasinoGames.Playercards == 4)
								{
									text9 = "hit_second_card_player_0" + CasinoGames.SeatSittingIn;
								}
								if (CasinoGames.Playercards == 5)
								{
									text9 = "deal_card_player_0" + CasinoGames.SeatSittingIn;
								}
								if (CasinoGames.Playercards == 6)
								{
									text9 = "hit_second_card_player_0" + CasinoGames.SeatSittingIn;
								}
							}
							CasinoGames.LoadDict(dict9);
							Vector3 dealerAnimOrigin4 = CasinoGames.GetDealerAnimOrigin();
							CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin4.X, dealerAnimOrigin4.Y, dealerAnimOrigin4.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict9), text9, 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text9, CasinoGames.LoadDict(dict9), 16f, -16f, 0, 1148846080);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, CasinoGames.Scene2, 1.5f);
							string text10 = (CasinoGames.CardId = CasinoGames.GetRandomCard_Blackjack());
							if (CasinoGames.Playercards == 1)
							{
								CasinoGames.PlayerCard_1 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 1);
								CasinoGames.PlayerCardA = GlobalHelper.CreateProp(text10, offsetPosition, true, false);
								CasinoGames.PlayerCardA.IsPositionFrozen = true;
								CasinoGames.PlayerCardA.IsPositionFrozen = true;
								CasinoGames.Chips.Add(CasinoGames.PlayerCardA);
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardA, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							}
							if (CasinoGames.Playercards == 2)
							{
								CasinoGames.PlayerCard_2 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 2);
								CasinoGames.PlayerCardB = GlobalHelper.CreateProp(text10, offsetPosition, true, false);
								CasinoGames.PlayerCardB.IsPositionFrozen = true;
								CasinoGames.PlayerCardB.IsPositionFrozen = true;
								CasinoGames.Chips.Add(CasinoGames.PlayerCardB);
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardB, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							}
							if (CasinoGames.Playercards == 3)
							{
								CasinoGames.PlayerCard_3 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 3);
								CasinoGames.PlayerCardC = GlobalHelper.CreateProp(text10, offsetPosition, true, false);
								CasinoGames.PlayerCardC.IsPositionFrozen = true;
								CasinoGames.PlayerCardC.IsPositionFrozen = true;
								CasinoGames.Chips.Add(CasinoGames.PlayerCardC);
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardC, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							}
							if (CasinoGames.Playercards == 4)
							{
								CasinoGames.PlayerCard_4 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 4);
								CasinoGames.PlayerCardD = GlobalHelper.CreateProp(text10, offsetPosition, true, false);
								CasinoGames.PlayerCardD.IsPositionFrozen = true;
								CasinoGames.PlayerCardD.IsPositionFrozen = true;
								CasinoGames.Chips.Add(CasinoGames.PlayerCardD);
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardD, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							}
							if (CasinoGames.Playercards == 5)
							{
								CasinoGames.PlayerCard_5 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 5);
								CasinoGames.PlayerCardE = GlobalHelper.CreateProp(text10, offsetPosition, true, false);
								CasinoGames.PlayerCardE.IsPositionFrozen = true;
								CasinoGames.PlayerCardE.IsPositionFrozen = true;
								CasinoGames.Chips.Add(CasinoGames.PlayerCardE);
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardE, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							}
							if (CasinoGames.Playercards == 6)
							{
								CasinoGames.PlayerCard_6 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 6);
								CasinoGames.PlayerCardF = GlobalHelper.CreateProp(text10, offsetPosition, true, false);
								CasinoGames.PlayerCardF.IsPositionFrozen = true;
								CasinoGames.PlayerCardF.IsPositionFrozen = true;
								CasinoGames.Chips.Add(CasinoGames.PlayerCardF);
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardF, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							}
							CasinoGames.Stage = 1f;
						}
						if (CasinoGames.Stage == -1f)
						{
							string dict10 = "anim_casino_b@amb@casino@games@blackjack@dealer";
							string text11 = "deal_card_player_0" + CasinoGames.SeatSittingIn;
							CasinoGames.LoadDict(dict10);
							Vector3 dealerAnimOrigin5 = CasinoGames.GetDealerAnimOrigin();
							CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin5.X, dealerAnimOrigin5.Y, dealerAnimOrigin5.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict10), text11, 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text11, CasinoGames.LoadDict(dict10), 16f, -16f, 0, 1148846080);
							string text12 = (CasinoGames.CardId = CasinoGames.GetRandomCard_Blackjack());
							if (CasinoGames.Playercards == 1)
							{
								CasinoGames.PlayerCard_1 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 1);
								CasinoGames.PlayerCardA = GlobalHelper.CreateProp(text12, offsetPosition, true, false);
								CasinoGames.PlayerCardA.IsPositionFrozen = true;
								CasinoGames.PlayerCardA.IsPositionFrozen = true;
								CasinoGames.Chips.Add(CasinoGames.PlayerCardA);
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardA, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							}
							if (CasinoGames.Playercards == 2)
							{
								CasinoGames.PlayerCard_2 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 2);
								CasinoGames.PlayerCardB = GlobalHelper.CreateProp(text12, offsetPosition, true, false);
								CasinoGames.PlayerCardB.IsPositionFrozen = true;
								CasinoGames.PlayerCardB.IsPositionFrozen = true;
								CasinoGames.Chips.Add(CasinoGames.PlayerCardB);
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardB, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							}
							if (CasinoGames.Playercards == 3)
							{
								CasinoGames.PlayerCard_3 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 3);
								CasinoGames.PlayerCardC = GlobalHelper.CreateProp(text12, offsetPosition, true, false);
								CasinoGames.PlayerCardC.IsPositionFrozen = true;
								CasinoGames.PlayerCardC.IsPositionFrozen = true;
								CasinoGames.Chips.Add(CasinoGames.PlayerCardC);
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardC, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							}
							if (CasinoGames.Playercards == 4)
							{
								CasinoGames.PlayerCard_4 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 4);
								CasinoGames.PlayerCardD = GlobalHelper.CreateProp(text12, offsetPosition, true, false);
								CasinoGames.PlayerCardD.IsPositionFrozen = true;
								CasinoGames.PlayerCardD.IsPositionFrozen = true;
								CasinoGames.Chips.Add(CasinoGames.PlayerCardD);
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardD, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							}
							if (CasinoGames.Playercards == 5)
							{
								CasinoGames.PlayerCard_5 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 5);
								CasinoGames.PlayerCardE = GlobalHelper.CreateProp(text12, offsetPosition, true, false);
								CasinoGames.PlayerCardE.IsPositionFrozen = true;
								CasinoGames.PlayerCardE.IsPositionFrozen = true;
								CasinoGames.Chips.Add(CasinoGames.PlayerCardE);
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardE, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							}
							if (CasinoGames.Playercards == 6)
							{
								CasinoGames.PlayerCard_6 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 6);
								CasinoGames.PlayerCardF = GlobalHelper.CreateProp(text12, offsetPosition, true, false);
								CasinoGames.PlayerCardF.IsPositionFrozen = true;
								CasinoGames.PlayerCardF.IsPositionFrozen = true;
								CasinoGames.Chips.Add(CasinoGames.PlayerCardF);
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardF, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							}
							CasinoGames.Stage = 1f;
						}
					}
				}
				if (CasinoGames.Stage == 10f)
				{
					if (Function.Call<bool>(Hash.HAS_ANIM_EVENT_FIRED, CasinoGames.ClosestDealer, Function.Call<int>(Hash.GET_HASH_KEY, "HideCard")))
					{
						Notification.Show("Hide Card");
						CasinoGames.DetatchAllCards();
					}
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 0.99f)
					{
						if (CasinoGames.BlackjackPlayerInitialBet != null)
						{
							EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(CasinoGames.BlackjackPlayerInitialBet, true, 255));
						}
						CasinoGames.Stage = 11f;
					}
				}
				if (CasinoGames.Stage == 35f && Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 0.99f)
				{
					Script.Wait(1);
					CasinoGames.DetatchAllCards();
					if (CasinoGames.BlackJackSplit)
					{
						string dict11 = "anim_casino_b@amb@casino@games@blackjack@player";
						string text13 = "place_bet_double_down";
						CasinoGames.LoadDict(dict11);
						CasinoGames.GetPlayerAnimOriginH(CasinoGames.SeatSittingIn, CasinoGames.TableType.Blackjack);
						CasinoGames.GetPlayerAnimOriginH(CasinoGames.SeatSittingIn, CasinoGames.TableType.Blackjack);
						CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict(dict11), text13, 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, text13, CasinoGames.LoadDict(dict11), 16f, -16f, 0, 1148846080);
						CasinoGames.Stage = 40f;
					}
					else
					{
						CasinoGames.Stage = 36f;
					}
					Prop prop2 = GlobalHelper.CreateProp(CasinoGames.BlackjackPlayerInitialBet.Model, CasinoGames.ReturnChipPosition(CasinoGames.BlackjackDealerTables, CasinoGames.BetTypeId.BlackJack), true, false);
					prop2.IsPositionFrozen = true;
					prop2.Heading = CasinoGames.ReturnChipAngle(CasinoGames.PokerDealerTables);
					CasinoGames.Chips.Add(prop2);
					prop2.Position = CasinoGames.ClosestDealerTable.Table.GetOffsetPosition(CasinoGames.GetPlayerBetPositionZOffset(CasinoGames.ReturnClosestBetAmtFromCustomBet((int)CasinoGames.Bet), 1, CasinoGames.SeatSittingIn - 1, true));
					prop2.Rotation = CasinoGames.ClosestDealerTable.Table.Rotation + CasinoGames.GetPlayerBetRotationOffset(CasinoGames.ReturnClosestBetAmtFromCustomBet((int)CasinoGames.Bet), 1, CasinoGames.SeatSittingIn - 1, true);
					EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(prop2, true, 255));
				}
				if (CasinoGames.Stage == 36f && Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 0.99f)
				{
					if (CasinoGames.ClosestDealer != null && Function.Call<bool>(Hash.HAS_ANIM_EVENT_FIRED, CasinoGames.ClosestDealer, Function.Call<int>(Hash.GET_HASH_KEY, "HideCard")))
					{
						CasinoGames.DetatchAllCards();
					}
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 0.99f)
					{
						CasinoGames.DetatchAllCards();
						CasinoGames.Playercards++;
						string dict12 = "anim_casino_b@amb@casino@games@blackjack@dealer";
						string text14 = "deal_card_player_0" + CasinoGames.SeatSittingIn;
						CasinoGames.LoadDict(dict12);
						Vector3 dealerAnimOrigin6 = CasinoGames.GetDealerAnimOrigin();
						CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin6.X, dealerAnimOrigin6.Y, dealerAnimOrigin6.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict12), text14, 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text14, CasinoGames.LoadDict(dict12), 16f, -16f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, CasinoGames.Scene2, 1.5f);
						string text15 = (CasinoGames.CardId = CasinoGames.GetRandomCard_Blackjack());
						if (CasinoGames.Playercards == 1)
						{
							CasinoGames.PlayerCard_1 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 1);
							CasinoGames.PlayerCardA = GlobalHelper.CreateProp(text15, offsetPosition, true, false);
							CasinoGames.PlayerCardA.IsPositionFrozen = true;
							CasinoGames.PlayerCardA.IsPositionFrozen = true;
							CasinoGames.Chips.Add(CasinoGames.PlayerCardA);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardA, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
						}
						if (CasinoGames.Playercards == 2)
						{
							CasinoGames.PlayerCard_2 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 2);
							CasinoGames.PlayerCardB = GlobalHelper.CreateProp(text15, offsetPosition, true, false);
							CasinoGames.PlayerCardB.IsPositionFrozen = true;
							CasinoGames.PlayerCardB.IsPositionFrozen = true;
							CasinoGames.Chips.Add(CasinoGames.PlayerCardB);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardB, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
						}
						if (CasinoGames.Playercards == 3)
						{
							CasinoGames.PlayerCard_3 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 3);
							CasinoGames.PlayerCardC = GlobalHelper.CreateProp(text15, offsetPosition, true, false);
							CasinoGames.PlayerCardC.IsPositionFrozen = true;
							CasinoGames.PlayerCardC.IsPositionFrozen = true;
							CasinoGames.Chips.Add(CasinoGames.PlayerCardC);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardC, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
						}
						if (CasinoGames.Playercards == 4)
						{
							CasinoGames.PlayerCard_4 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 4);
							CasinoGames.PlayerCardD = GlobalHelper.CreateProp(text15, offsetPosition, true, false);
							CasinoGames.PlayerCardD.IsPositionFrozen = true;
							CasinoGames.PlayerCardD.IsPositionFrozen = true;
							CasinoGames.Chips.Add(CasinoGames.PlayerCardD);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardD, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
						}
						if (CasinoGames.Playercards == 5)
						{
							CasinoGames.PlayerCard_5 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 5);
							CasinoGames.PlayerCardE = GlobalHelper.CreateProp(text15, offsetPosition, true, false);
							CasinoGames.PlayerCardE.IsPositionFrozen = true;
							CasinoGames.PlayerCardE.IsPositionFrozen = true;
							CasinoGames.Chips.Add(CasinoGames.PlayerCardE);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardE, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
						}
						if (CasinoGames.Playercards == 6)
						{
							CasinoGames.PlayerCard_6 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 6);
							CasinoGames.PlayerCardF = GlobalHelper.CreateProp(text15, offsetPosition, true, false);
							CasinoGames.PlayerCardF.IsPositionFrozen = true;
							CasinoGames.PlayerCardF.IsPositionFrozen = true;
							CasinoGames.Chips.Add(CasinoGames.PlayerCardF);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardF, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
						}
						Function.Call(argument1: "MINIGAME_BJACK_DEALER_" + Get_Left_AHandTotal(), hash: Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, argument0: CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.BlackjackDealerTables), argument2: "SPEECH_PARAMS_STANDARD", argument3: false);
						CasinoGames.Stage = 37f;
					}
				}
				if (CasinoGames.Stage == 37f && Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 0.99f)
				{
					Vector3 position3 = CasinoGames.Backface_DealerCard1.Position;
					Vector3 rotation3 = CasinoGames.Backface_DealerCard1.Rotation;
					if (CasinoGames.Backface_DealerCard1 != null)
					{
						CasinoGames.Backface_DealerCard1.Delete();
					}
					if (CasinoGames.ClosestDealerTable.Card1 != null)
					{
						CasinoGames.ClosestDealerTable.Card1.Delete();
					}
					CasinoGames.ClosestDealerTable.Card1 = GlobalHelper.CreateProp(CasinoGames.RequestModel(CasinoGames.FrontFace_DealerCard1), position3, rotation3, false, false);
					string dict13 = "anim_casino_b@amb@casino@games@blackjack@dealer";
					string text16 = "check_and_turn_card";
					Vector3 dealerAnimOrigin7 = CasinoGames.GetDealerAnimOrigin();
					CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin7.X, dealerAnimOrigin7.Y, dealerAnimOrigin7.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene1, CasinoGames.LoadDict(dict13), text16, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene1, text16, CasinoGames.LoadDict(dict13), 16f, -16f, 0, 1148846080);
					Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.ClosestDealerTable.Card1, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					Script.Wait(1);
					if (CasinoGames.DealerCards < CasinoGames.Playercards)
					{
						CasinoGames.Stage = 150f;
					}
					else
					{
						CasinoGames.Stage = 300f;
					}
				}
				if (CasinoGames.Stage == 40f && Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 0.99f)
				{
					Prop prop3 = GlobalHelper.CreateProp(CasinoGames.BlackjackPlayerInitialBet.Model, CasinoGames.ReturnChipPosition(CasinoGames.BlackjackDealerTables, CasinoGames.BetTypeId.BlackJack), true, false);
					prop3.IsPositionFrozen = true;
					prop3.Heading = CasinoGames.ReturnChipAngle(CasinoGames.PokerDealerTables);
					CasinoGames.Chips.Add(prop3);
					prop3.Position = CasinoGames.ClosestDealerTable.Table.GetOffsetPosition(CasinoGames.GetPlayerBetPositionZOffset(CasinoGames.ReturnClosestBetAmtFromCustomBet((int)CasinoGames.Bet), 3, CasinoGames.SeatSittingIn - 1, true));
					prop3.Rotation = CasinoGames.ClosestDealerTable.Table.Rotation + CasinoGames.GetPlayerBetRotationOffset(CasinoGames.ReturnClosestBetAmtFromCustomBet((int)CasinoGames.Bet), 3, CasinoGames.SeatSittingIn - 1, true);
					EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(prop3, true, 255));
					CasinoGames.DetatchAllCards();
					CasinoGames.Playercards++;
					string dict14 = "anim_casino_b@amb@casino@games@blackjack@dealer";
					string text17 = "deal_card_player_0" + CasinoGames.SeatSittingIn;
					CasinoGames.LoadDict(dict14);
					Vector3 dealerAnimOrigin8 = CasinoGames.GetDealerAnimOrigin();
					CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin8.X, dealerAnimOrigin8.Y, dealerAnimOrigin8.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict14), text17, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text17, CasinoGames.LoadDict(dict14), 16f, -16f, 0, 1148846080);
					string text18 = (CasinoGames.CardId = CasinoGames.GetRandomCard_Blackjack());
					if (CasinoGames.Playercards == 1)
					{
						CasinoGames.PlayerCard_1 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 1);
						CasinoGames.PlayerCardA = GlobalHelper.CreateProp(text18, offsetPosition, true, false);
						CasinoGames.PlayerCardA.IsPositionFrozen = true;
						CasinoGames.PlayerCardA.IsPositionFrozen = true;
						CasinoGames.Chips.Add(CasinoGames.PlayerCardA);
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardA, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					}
					if (CasinoGames.Playercards == 2)
					{
						CasinoGames.PlayerCard_2 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 2);
						CasinoGames.PlayerCardB = GlobalHelper.CreateProp(text18, offsetPosition, true, false);
						CasinoGames.PlayerCardB.IsPositionFrozen = true;
						CasinoGames.PlayerCardB.IsPositionFrozen = true;
						CasinoGames.Chips.Add(CasinoGames.PlayerCardB);
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardB, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					}
					if (CasinoGames.Playercards == 3)
					{
						CasinoGames.PlayerCard_3 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 3);
						CasinoGames.PlayerCardC = GlobalHelper.CreateProp(text18, offsetPosition, true, false);
						CasinoGames.PlayerCardC.IsPositionFrozen = true;
						CasinoGames.PlayerCardC.IsPositionFrozen = true;
						CasinoGames.Chips.Add(CasinoGames.PlayerCardC);
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardC, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					}
					if (CasinoGames.Playercards == 4)
					{
						CasinoGames.PlayerCard_4 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 4);
						CasinoGames.PlayerCardD = GlobalHelper.CreateProp(text18, offsetPosition, true, false);
						CasinoGames.PlayerCardD.IsPositionFrozen = true;
						CasinoGames.PlayerCardD.IsPositionFrozen = true;
						CasinoGames.Chips.Add(CasinoGames.PlayerCardD);
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardD, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					}
					if (CasinoGames.Playercards == 5)
					{
						CasinoGames.PlayerCard_5 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 5);
						CasinoGames.PlayerCardE = GlobalHelper.CreateProp(text18, offsetPosition, true, false);
						CasinoGames.PlayerCardE.IsPositionFrozen = true;
						CasinoGames.PlayerCardE.IsPositionFrozen = true;
						CasinoGames.Chips.Add(CasinoGames.PlayerCardE);
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardE, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					}
					if (CasinoGames.Playercards == 6)
					{
						CasinoGames.PlayerCard_6 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 6);
						CasinoGames.PlayerCardF = GlobalHelper.CreateProp(text18, offsetPosition, true, false);
						CasinoGames.PlayerCardF.IsPositionFrozen = true;
						CasinoGames.PlayerCardF.IsPositionFrozen = true;
						CasinoGames.Chips.Add(CasinoGames.PlayerCardF);
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardF, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					}
					Function.Call(argument1: "MINIGAME_BJACK_DEALER_" + Get_Right_B_HandTotal(), hash: Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, argument0: CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.BlackjackDealerTables), argument2: "SPEECH_PARAMS_STANDARD", argument3: false);
					CasinoGames.Stage = 41f;
				}
				if (CasinoGames.Stage == 41f && Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 0.99f)
				{
					Prop prop4 = GlobalHelper.CreateProp(CasinoGames.BlackjackPlayerInitialBet.Model, CasinoGames.ReturnChipPosition(CasinoGames.BlackjackDealerTables, CasinoGames.BetTypeId.BlackJack), true, false);
					prop4.IsPositionFrozen = true;
					prop4.Heading = CasinoGames.ReturnChipAngle(CasinoGames.PokerDealerTables);
					CasinoGames.Chips.Add(prop4);
					prop4.Position = CasinoGames.ClosestDealerTable.Table.GetOffsetPosition(CasinoGames.GetPlayerBetPositionZOffset(CasinoGames.ReturnClosestBetAmtFromCustomBet((int)CasinoGames.Bet), 3, CasinoGames.SeatSittingIn - 1, true));
					prop4.Rotation = CasinoGames.ClosestDealerTable.Table.Rotation + CasinoGames.GetPlayerBetRotationOffset(CasinoGames.ReturnClosestBetAmtFromCustomBet((int)CasinoGames.Bet), 3, CasinoGames.SeatSittingIn - 1, true);
					EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(prop4, true, 255));
					CasinoGames.DetatchAllCards();
					CasinoGames.Playercards++;
					string dict15 = "anim_casino_b@amb@casino@games@blackjack@dealer";
					string text19 = "hit_second_card_player_0" + CasinoGames.SeatSittingIn;
					CasinoGames.LoadDict(dict15);
					Vector3 dealerAnimOrigin9 = CasinoGames.GetDealerAnimOrigin();
					CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin9.X, dealerAnimOrigin9.Y, dealerAnimOrigin9.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict15), text19, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text19, CasinoGames.LoadDict(dict15), 16f, -16f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, CasinoGames.Scene2, 1.5f);
					string text20 = (CasinoGames.CardId = CasinoGames.GetRandomCard_Blackjack());
					if (CasinoGames.Playercards == 1)
					{
						CasinoGames.PlayerCard_1 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 1);
						CasinoGames.PlayerCardA = GlobalHelper.CreateProp(text20, offsetPosition, true, false);
						CasinoGames.PlayerCardA.IsPositionFrozen = true;
						CasinoGames.PlayerCardA.IsPositionFrozen = true;
						CasinoGames.Chips.Add(CasinoGames.PlayerCardA);
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardA, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					}
					if (CasinoGames.Playercards == 2)
					{
						CasinoGames.PlayerCard_2 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 2);
						CasinoGames.PlayerCardB = GlobalHelper.CreateProp(text20, offsetPosition, true, false);
						CasinoGames.PlayerCardB.IsPositionFrozen = true;
						CasinoGames.PlayerCardB.IsPositionFrozen = true;
						CasinoGames.Chips.Add(CasinoGames.PlayerCardB);
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardB, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					}
					if (CasinoGames.Playercards == 3)
					{
						CasinoGames.PlayerCard_3 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 3);
						CasinoGames.PlayerCardC = GlobalHelper.CreateProp(text20, offsetPosition, true, false);
						CasinoGames.PlayerCardC.IsPositionFrozen = true;
						CasinoGames.PlayerCardC.IsPositionFrozen = true;
						CasinoGames.Chips.Add(CasinoGames.PlayerCardC);
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardC, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					}
					if (CasinoGames.Playercards == 4)
					{
						CasinoGames.PlayerCard_4 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 4);
						CasinoGames.PlayerCardD = GlobalHelper.CreateProp(text20, offsetPosition, true, false);
						CasinoGames.PlayerCardD.IsPositionFrozen = true;
						CasinoGames.PlayerCardD.IsPositionFrozen = true;
						CasinoGames.Chips.Add(CasinoGames.PlayerCardD);
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardD, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					}
					if (CasinoGames.Playercards == 5)
					{
						CasinoGames.PlayerCard_5 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 5);
						CasinoGames.PlayerCardE = GlobalHelper.CreateProp(text20, offsetPosition, true, false);
						CasinoGames.PlayerCardE.IsPositionFrozen = true;
						CasinoGames.PlayerCardE.IsPositionFrozen = true;
						CasinoGames.Chips.Add(CasinoGames.PlayerCardE);
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardE, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					}
					if (CasinoGames.Playercards == 6)
					{
						CasinoGames.PlayerCard_6 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 6);
						CasinoGames.PlayerCardF = GlobalHelper.CreateProp(text20, offsetPosition, true, false);
						CasinoGames.PlayerCardF.IsPositionFrozen = true;
						CasinoGames.PlayerCardF.IsPositionFrozen = true;
						CasinoGames.Chips.Add(CasinoGames.PlayerCardF);
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardF, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					}
					Function.Call(argument1: "MINIGAME_BJACK_DEALER_" + Get_Right_B_HandTotal(), hash: Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, argument0: CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.BlackjackDealerTables), argument2: "SPEECH_PARAMS_STANDARD", argument3: false);
					CasinoGames.Stage = 42f;
				}
				if (CasinoGames.Stage == 42f && Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 0.99f)
				{
					Vector3 position4 = CasinoGames.Backface_DealerCard1.Position;
					Vector3 rotation4 = CasinoGames.Backface_DealerCard1.Rotation;
					if (CasinoGames.Backface_DealerCard1 != null)
					{
						CasinoGames.Backface_DealerCard1.Delete();
					}
					if (CasinoGames.ClosestDealerTable.Card1 != null)
					{
						CasinoGames.ClosestDealerTable.Card1.Delete();
					}
					CasinoGames.ClosestDealerTable.Card1 = GlobalHelper.CreateProp(CasinoGames.RequestModel(CasinoGames.FrontFace_DealerCard1), position4, rotation4, false, false);
					string dict16 = "anim_casino_b@amb@casino@games@blackjack@dealer";
					string text21 = "check_and_turn_card";
					Vector3 dealerAnimOrigin10 = CasinoGames.GetDealerAnimOrigin();
					CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin10.X, dealerAnimOrigin10.Y, dealerAnimOrigin10.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene1, CasinoGames.LoadDict(dict16), text21, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene1, text21, CasinoGames.LoadDict(dict16), 16f, -16f, 0, 1148846080);
					Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.ClosestDealerTable.Card1, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					Script.Wait(1);
					if (CasinoGames.DealerCards < CasinoGames.Playercards)
					{
						CasinoGames.Stage = 150f;
					}
					else
					{
						CasinoGames.Stage = 300f;
					}
				}
				if (CasinoGames.Stage == 45f)
				{
					if (CasinoGames.ClosestDealer != null && Function.Call<bool>(Hash.HAS_ANIM_EVENT_FIRED, CasinoGames.ClosestDealer, Function.Call<int>(Hash.GET_HASH_KEY, "HideCard")))
					{
						Notification.Show("Hide Card");
						CasinoGames.DetatchAllCards();
					}
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 0.99f)
					{
						Script.Wait(1);
						CasinoGames.DetatchAllCards();
						Vector3 position12 = Game.Player.Character.Position;
						CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading - 90f, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base"), "base", 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, "base", CasinoGames.LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base"), 16f, -16f, 0, 1148846080);
						Prop prop5 = GlobalHelper.CreateProp(CasinoGames.BlackjackPlayerInitialBet.Model, CasinoGames.ReturnChipPosition(CasinoGames.BlackjackDealerTables, CasinoGames.BetTypeId.BlackJack), true, false);
						prop5.IsPositionFrozen = true;
						prop5.Heading = CasinoGames.ReturnChipAngle(CasinoGames.PokerDealerTables);
						CasinoGames.Chips.Add(prop5);
						prop5.Position = CasinoGames.ClosestDealerTable.Table.GetOffsetPosition(CasinoGames.GetPlayerBetPositionZOffset(CasinoGames.ReturnClosestBetAmtFromCustomBet((int)CasinoGames.Bet), 3, CasinoGames.SeatSittingIn - 1, true));
						prop5.Rotation = CasinoGames.ClosestDealerTable.Table.Rotation + CasinoGames.GetPlayerBetRotationOffset(CasinoGames.ReturnClosestBetAmtFromCustomBet((int)CasinoGames.Bet), 3, CasinoGames.SeatSittingIn - 1, true);
						EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(prop5, true, 255));
						CasinoGames.Stage = 41f;
					}
				}
				if (CasinoGames.Stage == 15f && Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 0.99f)
				{
					CasinoGames.BlackJackSplit = true;
					Script.Wait(1);
					CasinoGames.DetatchAllCards();
					Vector3 position13 = Game.Player.Character.Position;
					CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading - 90f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base"), "base", 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, "base", CasinoGames.LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base"), 16f, -16f, 0, 1148846080);
					string dict17 = "anim_casino_b@amb@casino@games@blackjack@dealer";
					string text22 = "split_card_player_0" + CasinoGames.SeatSittingIn;
					Vector3 dealerAnimOrigin11 = CasinoGames.GetDealerAnimOrigin();
					CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin11.X, dealerAnimOrigin11.Y, dealerAnimOrigin11.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene1, CasinoGames.LoadDict(dict17), text22, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene1, text22, CasinoGames.LoadDict(dict17), 16f, -16f, 0, 1148846080);
					Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardB, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					CasinoGames.BlackJackSplit = true;
					CasinoGames.Stage = 20f;
					Prop prop6 = GlobalHelper.CreateProp(CasinoGames.BlackjackPlayerInitialBet.Model, CasinoGames.ReturnChipPosition(CasinoGames.BlackjackDealerTables, CasinoGames.BetTypeId.BlackJack), true, false);
					prop6.IsPositionFrozen = true;
					prop6.Heading = CasinoGames.ReturnChipAngle(CasinoGames.PokerDealerTables);
					CasinoGames.Chips.Add(prop6);
					prop6.Position = CasinoGames.ClosestDealerTable.Table.GetOffsetPosition(CasinoGames.GetPlayerBetPositionZOffset(CasinoGames.ReturnClosestBetAmtFromCustomBet((int)CasinoGames.Bet), 2, CasinoGames.SeatSittingIn - 1, true));
					prop6.Rotation = CasinoGames.ClosestDealerTable.Table.Rotation + CasinoGames.GetPlayerBetRotationOffset(CasinoGames.ReturnClosestBetAmtFromCustomBet((int)CasinoGames.Bet), 2, CasinoGames.SeatSittingIn - 1, true);
					EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(prop6, true, 255));
				}
				if (CasinoGames.Stage == 20f)
				{
					if (CasinoGames.ClosestDealer != null && Function.Call<bool>(Hash.HAS_ANIM_EVENT_FIRED, CasinoGames.ClosestDealer, Function.Call<int>(Hash.GET_HASH_KEY, "HideCard")))
					{
						Notification.Show("Hide Card");
						CasinoGames.DetatchAllCards();
					}
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 0.99f)
					{
						CasinoGames.DetatchAllCards();
						CasinoGames.Playercards++;
						string dict18 = "anim_casino_b@amb@casino@games@blackjack@dealer";
						string text23 = "hit_card_player_0" + CasinoGames.SeatSittingIn;
						CasinoGames.LoadDict(dict18);
						Vector3 dealerAnimOrigin12 = CasinoGames.GetDealerAnimOrigin();
						CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin12.X, dealerAnimOrigin12.Y, dealerAnimOrigin12.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict18), text23, 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text23, CasinoGames.LoadDict(dict18), 16f, -16f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, CasinoGames.Scene2, 1.5f);
						string text24 = (CasinoGames.CardId = CasinoGames.GetRandomCard_Blackjack());
						if (CasinoGames.Playercards == 1)
						{
							CasinoGames.PlayerCard_1 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 1);
							CasinoGames.PlayerCardA = GlobalHelper.CreateProp(text24, offsetPosition, true, false);
							CasinoGames.PlayerCardA.IsPositionFrozen = true;
							CasinoGames.PlayerCardA.IsPositionFrozen = true;
							CasinoGames.Chips.Add(CasinoGames.PlayerCardA);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardA, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
						}
						if (CasinoGames.Playercards == 2)
						{
							CasinoGames.PlayerCard_2 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 2);
							CasinoGames.PlayerCardB = GlobalHelper.CreateProp(text24, offsetPosition, true, false);
							CasinoGames.PlayerCardB.IsPositionFrozen = true;
							CasinoGames.PlayerCardB.IsPositionFrozen = true;
							CasinoGames.Chips.Add(CasinoGames.PlayerCardB);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardB, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
						}
						if (CasinoGames.Playercards == 3)
						{
							CasinoGames.PlayerCard_3 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 3);
							CasinoGames.PlayerCardC = GlobalHelper.CreateProp(text24, offsetPosition, true, false);
							CasinoGames.PlayerCardC.IsPositionFrozen = true;
							CasinoGames.PlayerCardC.IsPositionFrozen = true;
							CasinoGames.Chips.Add(CasinoGames.PlayerCardC);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardC, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
						}
						if (CasinoGames.Playercards == 4)
						{
							CasinoGames.PlayerCard_4 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 4);
							CasinoGames.PlayerCardD = GlobalHelper.CreateProp(text24, offsetPosition, true, false);
							CasinoGames.PlayerCardD.IsPositionFrozen = true;
							CasinoGames.PlayerCardD.IsPositionFrozen = true;
							CasinoGames.Chips.Add(CasinoGames.PlayerCardD);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardD, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
						}
						if (CasinoGames.Playercards == 5)
						{
							CasinoGames.PlayerCard_5 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 5);
							CasinoGames.PlayerCardE = GlobalHelper.CreateProp(text24, offsetPosition, true, false);
							CasinoGames.PlayerCardE.IsPositionFrozen = true;
							CasinoGames.PlayerCardE.IsPositionFrozen = true;
							CasinoGames.Chips.Add(CasinoGames.PlayerCardE);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardE, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
						}
						if (CasinoGames.Playercards == 6)
						{
							CasinoGames.PlayerCard_6 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 6);
							CasinoGames.PlayerCardF = GlobalHelper.CreateProp(text24, offsetPosition, true, false);
							CasinoGames.PlayerCardF.IsPositionFrozen = true;
							CasinoGames.PlayerCardF.IsPositionFrozen = true;
							CasinoGames.Chips.Add(CasinoGames.PlayerCardF);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardF, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
						}
						Function.Call(argument1: "MINIGAME_BJACK_DEALER_" + Get_Left_AHandTotal(), hash: Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, argument0: CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.BlackjackDealerTables), argument2: "SPEECH_PARAMS_STANDARD", argument3: false);
						Script.Wait(1);
						CasinoGames.Stage = 25f;
					}
				}
				if (CasinoGames.Stage == 25f)
				{
					if (CasinoGames.ClosestDealer != null && Function.Call<bool>(Hash.HAS_ANIM_EVENT_FIRED, CasinoGames.ClosestDealer, Function.Call<int>(Hash.GET_HASH_KEY, "HideCard")))
					{
						Notification.Show("Hide Card");
						CasinoGames.DetatchAllCards();
					}
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 0.99f)
					{
						CasinoGames.DetatchAllCards();
						CasinoGames.Playercards++;
						string dict19 = "anim_casino_b@amb@casino@games@blackjack@dealer";
						string text25 = "hit_second_card_player_0" + CasinoGames.SeatSittingIn;
						CasinoGames.LoadDict(dict19);
						Vector3 dealerAnimOrigin13 = CasinoGames.GetDealerAnimOrigin();
						CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin13.X, dealerAnimOrigin13.Y, dealerAnimOrigin13.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict19), text25, 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text25, CasinoGames.LoadDict(dict19), 16f, -16f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, CasinoGames.Scene2, 1.5f);
						string text26 = (CasinoGames.CardId = CasinoGames.GetRandomCard_Blackjack());
						if (CasinoGames.Playercards == 1)
						{
							CasinoGames.PlayerCard_1 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 1);
							CasinoGames.PlayerCardA = GlobalHelper.CreateProp(text26, offsetPosition, true, false);
							CasinoGames.PlayerCardA.IsPositionFrozen = true;
							CasinoGames.PlayerCardA.IsPositionFrozen = true;
							CasinoGames.Chips.Add(CasinoGames.PlayerCardA);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardA, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
						}
						if (CasinoGames.Playercards == 2)
						{
							CasinoGames.PlayerCard_2 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 2);
							CasinoGames.PlayerCardB = GlobalHelper.CreateProp(text26, offsetPosition, true, false);
							CasinoGames.PlayerCardB.IsPositionFrozen = true;
							CasinoGames.PlayerCardB.IsPositionFrozen = true;
							CasinoGames.Chips.Add(CasinoGames.PlayerCardB);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardB, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
						}
						if (CasinoGames.Playercards == 3)
						{
							CasinoGames.PlayerCard_3 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 3);
							CasinoGames.PlayerCardC = GlobalHelper.CreateProp(text26, offsetPosition, true, false);
							CasinoGames.PlayerCardC.IsPositionFrozen = true;
							CasinoGames.PlayerCardC.IsPositionFrozen = true;
							CasinoGames.Chips.Add(CasinoGames.PlayerCardC);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardC, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
						}
						if (CasinoGames.Playercards == 4)
						{
							CasinoGames.PlayerCard_4 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 4);
							CasinoGames.PlayerCardD = GlobalHelper.CreateProp(text26, offsetPosition, true, false);
							CasinoGames.PlayerCardD.IsPositionFrozen = true;
							CasinoGames.PlayerCardD.IsPositionFrozen = true;
							CasinoGames.Chips.Add(CasinoGames.PlayerCardD);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardD, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
						}
						if (CasinoGames.Playercards == 5)
						{
							CasinoGames.PlayerCard_5 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 5);
							CasinoGames.PlayerCardE = GlobalHelper.CreateProp(text26, offsetPosition, true, false);
							CasinoGames.PlayerCardE.IsPositionFrozen = true;
							CasinoGames.PlayerCardE.IsPositionFrozen = true;
							CasinoGames.Chips.Add(CasinoGames.PlayerCardE);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardE, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
						}
						if (CasinoGames.Playercards == 6)
						{
							CasinoGames.PlayerCard_6 = CasinoGames.Set_PlayerCard_Blackjack(CasinoGames.CardId, 6);
							CasinoGames.PlayerCardF = GlobalHelper.CreateProp(text26, offsetPosition, true, false);
							CasinoGames.PlayerCardF.IsPositionFrozen = true;
							CasinoGames.PlayerCardF.IsPositionFrozen = true;
							CasinoGames.Chips.Add(CasinoGames.PlayerCardF);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.PlayerCardF, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
						}
						Function.Call(argument1: "MINIGAME_BJACK_DEALER_" + Get_Right_B_HandTotal(), hash: Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, argument0: CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.BlackjackDealerTables), argument2: "SPEECH_PARAMS_STANDARD", argument3: false);
						if (CasinoGames.BlackJackDoubleDown)
						{
							dict19 = "anim_casino_b@amb@casino@games@blackjack@player";
							text25 = "place_bet_double_down";
							CasinoGames.LoadDict(dict19);
							CasinoGames.GetPlayerAnimOriginH(CasinoGames.SeatSittingIn, CasinoGames.TableType.Blackjack);
							CasinoGames.GetPlayerAnimOriginH(CasinoGames.SeatSittingIn, CasinoGames.TableType.Blackjack);
							CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene1, CasinoGames.LoadDict(dict19), text25, 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene1, text25, CasinoGames.LoadDict(dict19), 16f, -16f, 0, 1148846080);
							CasinoGames.Stage = 45f;
						}
						else
						{
							CasinoGames.Stage = 2f;
						}
					}
				}
				if (CasinoGames.Stage == 2f || CasinoGames.Stage == -1f || CasinoGames.Stage == 0f)
				{
					if (CasinoGames.DealerCards < 1)
					{
						if (CasinoGames.ClosestDealerTable.Card1 != null)
						{
							CasinoGames.ClosestDealerTable.Card1.Opacity = 0;
						}
					}
					else if (CasinoGames.Backface_DealerCard1 != null)
					{
						CasinoGames.Backface_DealerCard1.Opacity = 255;
					}
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 0.99f)
					{
						CasinoGames.DetatchAllCards();
						CasinoGames.Stage = 0f;
						CasinoGames.TimeSinceLastCard = Game.GameTime + 5000;
						string dict20 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@idles";
						Random random2 = new Random();
						List<string> list2 = new List<string> { "idle_a", "idle_b", "idle_c", "idle_d" };
						string text27 = list2[random2.Next(0, list2.Count)];
						CasinoGames.LoadDict(dict20);
						CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading - 90f, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict(dict20), text27, 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, text27, CasinoGames.LoadDict(dict20), 16f, -16f, 0, 1148846080);
						string dict21 = "anim_casino_b@amb@casino@games@blackjack@dealer";
						string text28 = "idle";
						Vector3 dealerAnimOrigin14 = CasinoGames.GetDealerAnimOrigin();
						CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin14.X, dealerAnimOrigin14.Y, dealerAnimOrigin14.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene1, CasinoGames.LoadDict(dict21), text28, 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene1, text28, CasinoGames.LoadDict(dict21), 16f, -16f, 0, 1148846080);
					}
				}
				if (CasinoGames.Stage == 5f)
				{
					if (CasinoGames.DealerCards < 1)
					{
						if (CasinoGames.ClosestDealerTable.Card1 != null)
						{
							CasinoGames.ClosestDealerTable.Card1.Opacity = 0;
						}
					}
					else if (CasinoGames.Backface_DealerCard1 != null)
					{
						CasinoGames.Backface_DealerCard1.Opacity = 255;
					}
					int num5 = CasinoGames.PlayerCard_1 + CasinoGames.PlayerCard_2 + CasinoGames.PlayerCard_3 + CasinoGames.PlayerCard_4;
					if (CasinoGames.Playercards >= 2)
					{
						num5 = CasinoGames.PlayerCard_1 + CasinoGames.PlayerCard_2;
					}
					if (CasinoGames.Playercards >= 3)
					{
						num5 = CasinoGames.PlayerCard_1 + CasinoGames.PlayerCard_2 + CasinoGames.PlayerCard_3;
					}
					if (CasinoGames.Playercards >= 4)
					{
						num5 = CasinoGames.PlayerCard_1 + CasinoGames.PlayerCard_2 + CasinoGames.PlayerCard_3 + CasinoGames.PlayerCard_4;
					}
					if (CasinoGames.Playercards >= 5)
					{
						num5 = CasinoGames.PlayerCard_1 + CasinoGames.PlayerCard_2 + CasinoGames.PlayerCard_3 + CasinoGames.PlayerCard_4 + CasinoGames.PlayerCard_5;
					}
					if (CasinoGames.BlackJackSplit)
					{
						int right_B_HandTotal = Get_Right_B_HandTotal();
						num5 = Get_Left_AHandTotal();
						if (right_B_HandTotal == 21)
						{
							CasinoGames.OverrideDealer1stCard();
							CasinoGames.DrawCardTime = -1;
							CasinoGames.BlackJackCard1 = "";
							CasinoGames.BlackJackCard2 = "";
							CasinoGames.BlackJackCard3 = "";
							CasinoGames.BlackJackCard4 = "";
							CasinoGames.BlackJackCard5 = "";
							CasinoGames.Placebet2 = false;
							CasinoGames.Drawcards = true;
							new Random().Next(0, 2000);
							CasinoGames.OverrideDealer1stCard();
							Vector3 position14 = Game.Player.Character.Position;
							CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading - 90f, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base"), "base", 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, "base", CasinoGames.LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base"), 16f, -16f, 0, 1148846080);
							Vector3 position5 = CasinoGames.Backface_DealerCard1.Position;
							Vector3 rotation5 = CasinoGames.Backface_DealerCard1.Rotation;
							if (CasinoGames.Backface_DealerCard1 != null)
							{
								CasinoGames.Backface_DealerCard1.Delete();
							}
							if (CasinoGames.ClosestDealerTable.Card1 != null)
							{
								CasinoGames.ClosestDealerTable.Card1.Delete();
							}
							CasinoGames.ClosestDealerTable.Card1 = GlobalHelper.CreateProp(CasinoGames.RequestModel(CasinoGames.FrontFace_DealerCard1), position5, rotation5, false, false);
							string dict22 = "anim_casino_b@amb@casino@games@blackjack@dealer";
							string text29 = "check_and_turn_card";
							Vector3 dealerAnimOrigin15 = CasinoGames.GetDealerAnimOrigin();
							CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin15.X, dealerAnimOrigin15.Y, dealerAnimOrigin15.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene1, CasinoGames.LoadDict(dict22), text29, 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene1, text29, CasinoGames.LoadDict(dict22), 16f, -16f, 0, 1148846080);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.ClosestDealerTable.Card1, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							Script.Wait(1);
							if (CasinoGames.DealerCards < CasinoGames.Playercards)
							{
								CasinoGames.Stage = 150f;
							}
							else
							{
								CasinoGames.Stage = 300f;
							}
						}
						if (right_B_HandTotal >= 22)
						{
							CasinoGames.OverrideDealer1stCard();
							CasinoGames.DrawCardTime = -1;
							CasinoGames.BlackJackCard1 = "";
							CasinoGames.BlackJackCard2 = "";
							CasinoGames.BlackJackCard3 = "";
							CasinoGames.Placebet2 = false;
							CasinoGames.Drawcards = true;
							new Random().Next(0, 2000);
							CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading - 90f, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base"), "base", 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, "base", CasinoGames.LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base"), 16f, -16f, 0, 1148846080);
							Vector3 position6 = CasinoGames.Backface_DealerCard1.Position;
							Vector3 rotation6 = CasinoGames.Backface_DealerCard1.Rotation;
							if (CasinoGames.Backface_DealerCard1 != null)
							{
								CasinoGames.Backface_DealerCard1.Delete();
							}
							if (CasinoGames.ClosestDealerTable.Card1 != null)
							{
								CasinoGames.ClosestDealerTable.Card1.Delete();
							}
							CasinoGames.ClosestDealerTable.Card1 = GlobalHelper.CreateProp(CasinoGames.RequestModel(CasinoGames.FrontFace_DealerCard1), position6, rotation6, false, false);
							string dict23 = "anim_casino_b@amb@casino@games@blackjack@dealer";
							string text30 = "check_and_turn_card";
							Vector3 dealerAnimOrigin16 = CasinoGames.GetDealerAnimOrigin();
							CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin16.X, dealerAnimOrigin16.Y, dealerAnimOrigin16.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene1, CasinoGames.LoadDict(dict23), text30, 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene1, text30, CasinoGames.LoadDict(dict23), 16f, -16f, 0, 1148846080);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.ClosestDealerTable.Card1, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							Script.Wait(1);
							if (CasinoGames.DealerCards < CasinoGames.Playercards)
							{
								CasinoGames.Stage = 150f;
							}
							else
							{
								CasinoGames.Stage = 300f;
							}
						}
						if (num5 == 21)
						{
							CasinoGames.OverrideDealer1stCard();
							CasinoGames.DrawCardTime = -1;
							CasinoGames.BlackJackCard1 = "";
							CasinoGames.BlackJackCard2 = "";
							CasinoGames.BlackJackCard3 = "";
							CasinoGames.BlackJackCard4 = "";
							CasinoGames.BlackJackCard5 = "";
							CasinoGames.Placebet2 = false;
							CasinoGames.Drawcards = true;
							new Random().Next(0, 2000);
							Vector3 position15 = Game.Player.Character.Position;
							CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading - 90f, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base"), "base", 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, "base", CasinoGames.LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base"), 16f, -16f, 0, 1148846080);
							Vector3 position7 = CasinoGames.Backface_DealerCard1.Position;
							Vector3 rotation7 = CasinoGames.Backface_DealerCard1.Rotation;
							if (CasinoGames.Backface_DealerCard1 != null)
							{
								CasinoGames.Backface_DealerCard1.Delete();
							}
							if (CasinoGames.ClosestDealerTable.Card1 != null)
							{
								CasinoGames.ClosestDealerTable.Card1.Delete();
							}
							CasinoGames.ClosestDealerTable.Card1 = GlobalHelper.CreateProp(CasinoGames.RequestModel(CasinoGames.FrontFace_DealerCard1), position7, rotation7, false, false);
							string dict24 = "anim_casino_b@amb@casino@games@blackjack@dealer";
							string text31 = "check_and_turn_card";
							Vector3 dealerAnimOrigin17 = CasinoGames.GetDealerAnimOrigin();
							CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin17.X, dealerAnimOrigin17.Y, dealerAnimOrigin17.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene1, CasinoGames.LoadDict(dict24), text31, 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene1, text31, CasinoGames.LoadDict(dict24), 16f, -16f, 0, 1148846080);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.ClosestDealerTable.Card1, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							Script.Wait(1);
							if (CasinoGames.DealerCards < CasinoGames.Playercards)
							{
								CasinoGames.Stage = 150f;
							}
							else
							{
								CasinoGames.Stage = 300f;
							}
						}
						if (num5 >= 22)
						{
							CasinoGames.OverrideDealer1stCard();
							CasinoGames.DrawCardTime = -1;
							CasinoGames.BlackJackCard1 = "";
							CasinoGames.BlackJackCard2 = "";
							CasinoGames.BlackJackCard3 = "";
							CasinoGames.Placebet2 = false;
							CasinoGames.Drawcards = true;
							new Random().Next(0, 2000);
							CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading - 90f, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base"), "base", 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, "base", CasinoGames.LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base"), 16f, -16f, 0, 1148846080);
							Vector3 position8 = CasinoGames.Backface_DealerCard1.Position;
							Vector3 rotation8 = CasinoGames.Backface_DealerCard1.Rotation;
							if (CasinoGames.Backface_DealerCard1 != null)
							{
								CasinoGames.Backface_DealerCard1.Delete();
							}
							if (CasinoGames.ClosestDealerTable.Card1 != null)
							{
								CasinoGames.ClosestDealerTable.Card1.Delete();
							}
							CasinoGames.ClosestDealerTable.Card1 = GlobalHelper.CreateProp(CasinoGames.RequestModel(CasinoGames.FrontFace_DealerCard1), position8, rotation8, false, false);
							string dict25 = "anim_casino_b@amb@casino@games@blackjack@dealer";
							string text32 = "check_and_turn_card";
							Vector3 dealerAnimOrigin18 = CasinoGames.GetDealerAnimOrigin();
							CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin18.X, dealerAnimOrigin18.Y, dealerAnimOrigin18.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene1, CasinoGames.LoadDict(dict25), text32, 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene1, text32, CasinoGames.LoadDict(dict25), 16f, -16f, 0, 1148846080);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.ClosestDealerTable.Card1, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							Script.Wait(1);
							if (CasinoGames.DealerCards < CasinoGames.Playercards)
							{
								CasinoGames.Stage = 150f;
							}
							else
							{
								CasinoGames.Stage = 300f;
							}
						}
					}
					else
					{
						if (num5 == 21)
						{
							CasinoGames.OverrideDealer1stCard();
							CasinoGames.DrawCardTime = -1;
							CasinoGames.BlackJackCard1 = "";
							CasinoGames.BlackJackCard2 = "";
							CasinoGames.BlackJackCard3 = "";
							CasinoGames.BlackJackCard4 = "";
							CasinoGames.BlackJackCard5 = "";
							CasinoGames.Placebet2 = false;
							CasinoGames.Drawcards = true;
							new Random().Next(0, 2000);
							Vector3 position16 = Game.Player.Character.Position;
							CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading - 90f, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base"), "base", 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, "base", CasinoGames.LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base"), 16f, -16f, 0, 1148846080);
							Vector3 position9 = CasinoGames.Backface_DealerCard1.Position;
							Vector3 rotation9 = CasinoGames.Backface_DealerCard1.Rotation;
							if (CasinoGames.Backface_DealerCard1 != null)
							{
								CasinoGames.Backface_DealerCard1.Delete();
							}
							if (CasinoGames.ClosestDealerTable.Card1 != null)
							{
								CasinoGames.ClosestDealerTable.Card1.Delete();
							}
							CasinoGames.ClosestDealerTable.Card1 = GlobalHelper.CreateProp(CasinoGames.RequestModel(CasinoGames.FrontFace_DealerCard1), position9, rotation9, false, false);
							string dict26 = "anim_casino_b@amb@casino@games@blackjack@dealer";
							string text33 = "check_and_turn_card";
							Vector3 dealerAnimOrigin19 = CasinoGames.GetDealerAnimOrigin();
							CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin19.X, dealerAnimOrigin19.Y, dealerAnimOrigin19.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene1, CasinoGames.LoadDict(dict26), text33, 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene1, text33, CasinoGames.LoadDict(dict26), 16f, -16f, 0, 1148846080);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.ClosestDealerTable.Card1, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							Script.Wait(1);
							if (CasinoGames.DealerCards < CasinoGames.Playercards)
							{
								CasinoGames.Stage = 150f;
							}
							else
							{
								CasinoGames.Stage = 300f;
							}
						}
						if (num5 >= 22)
						{
							CasinoGames.OverrideDealer1stCard();
							Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.BlackjackDealerTables), "MINIGAME_BJACK_DEALER_PLAYER_BUST", "SPEECH_PARAMS_STANDARD", false);
							CasinoGames.DrawCardTime = -1;
							CasinoGames.BlackJackCard1 = "";
							CasinoGames.BlackJackCard2 = "";
							CasinoGames.BlackJackCard3 = "";
							CasinoGames.Placebet2 = false;
							CasinoGames.Drawcards = true;
							new Random().Next(0, 2000);
							CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading - 90f, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base"), "base", 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, "base", CasinoGames.LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base"), 16f, -16f, 0, 1148846080);
							Vector3 position10 = CasinoGames.Backface_DealerCard1.Position;
							Vector3 rotation10 = CasinoGames.Backface_DealerCard1.Rotation;
							if (CasinoGames.Backface_DealerCard1 != null)
							{
								CasinoGames.Backface_DealerCard1.Delete();
							}
							if (CasinoGames.ClosestDealerTable.Card1 != null)
							{
								CasinoGames.ClosestDealerTable.Card1.Delete();
							}
							CasinoGames.ClosestDealerTable.Card1 = GlobalHelper.CreateProp(CasinoGames.RequestModel(CasinoGames.FrontFace_DealerCard1), position10, rotation10, false, false);
							string dict27 = "anim_casino_b@amb@casino@games@blackjack@dealer";
							string text34 = "check_and_turn_card";
							Vector3 dealerAnimOrigin20 = CasinoGames.GetDealerAnimOrigin();
							CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin20.X, dealerAnimOrigin20.Y, dealerAnimOrigin20.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene1, CasinoGames.LoadDict(dict27), text34, 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene1, text34, CasinoGames.LoadDict(dict27), 16f, -16f, 0, 1148846080);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.ClosestDealerTable.Card1, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							Script.Wait(1);
							if (CasinoGames.DealerCards < CasinoGames.Playercards)
							{
								CasinoGames.Stage = 150f;
							}
							else
							{
								CasinoGames.Stage = 300f;
							}
						}
					}
					if (CasinoGames.DrawCardTime != -1)
					{
						if (Game.GameTime <= CasinoGames.DrawCardTime)
						{
							string dict28 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@idles";
							Random random3 = new Random();
							List<string> list3 = new List<string> { "idle_a", "idle_b", "idle_c", "idle_d" };
							string text35 = list3[random3.Next(0, list3.Count)];
							CasinoGames.LoadDict(dict28);
							CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading - 90f, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict(dict28), text35, 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, text35, CasinoGames.LoadDict(dict28), 16f, -16f, 0, 1148846080);
							string dict29 = "anim_casino_b@amb@casino@games@blackjack@dealer";
							string text36 = "idle";
							Vector3 dealerAnimOrigin21 = CasinoGames.GetDealerAnimOrigin();
							CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin21.X, dealerAnimOrigin21.Y, dealerAnimOrigin21.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene1, CasinoGames.LoadDict(dict29), text36, 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene1, text36, CasinoGames.LoadDict(dict29), 16f, -16f, 0, 1148846080);
						}
						if (Game.GameTime >= CasinoGames.DrawCardTime && CasinoGames.DrawCardTime != -1)
						{
							CasinoGames.DetatchAllCards();
							Vector3 position17 = Game.Player.Character.Position;
							CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading - 90f, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base"), "base", 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, "base", CasinoGames.LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base"), 16f, -16f, 0, 1148846080);
							string dict30 = "anim_casino_b@amb@casino@games@blackjack@dealer";
							string text37 = "deal_card_self_second_card";
							if (CasinoGames.DealerCards > 2)
							{
								text37 = "deal_card_self_second_card";
							}
							if (CasinoGames.DealerCards == 3)
							{
								text37 = "deal_card_self_card_06";
							}
							if (CasinoGames.DealerCards == 4)
							{
								text37 = "female_deal_card_self_card_10";
							}
							Vector3 dealerAnimOrigin22 = CasinoGames.GetDealerAnimOrigin();
							CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin22.X, dealerAnimOrigin22.Y, dealerAnimOrigin22.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene1, CasinoGames.LoadDict(dict30), text37, 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene1, text37, CasinoGames.LoadDict(dict30), 16f, -16f, 0, 1148846080);
							if (CasinoGames.DealerCards == 0)
							{
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.ClosestDealerTable.Card1, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							}
							if (CasinoGames.DealerCards == 1)
							{
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.ClosestDealerTable.Card2, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							}
							if (CasinoGames.DealerCards == 2)
							{
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.ClosestDealerTable.Card3, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							}
							if (CasinoGames.DealerCards == 3)
							{
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.ClosestDealerTable.Card4, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							}
							if (CasinoGames.DealerCards == 4)
							{
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.ClosestDealerTable.Card5, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							}
							if (CasinoGames.DealerCards == 5)
							{
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.ClosestDealerTable.Card6, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							}
							CasinoGames.DealerCards++;
							CasinoGames.Stage = 2f;
						}
					}
				}
				if (CasinoGames.Stage == 1f)
				{
					if (CasinoGames.DealerCards < 1 && CasinoGames.ClosestDealerTable.Card1 != null)
					{
						CasinoGames.ClosestDealerTable.Card1.Opacity = 0;
					}
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 0.99f)
					{
						if (CasinoGames.Playercards == 0)
						{
							CasinoGames.Stage = 5f;
							int playerCard_ = CasinoGames.PlayerCard_1;
							Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.BlackjackDealerTables), "MINIGAME_BJACK_DEALER_" + playerCard_, "SPEECH_PARAMS_STANDARD", false);
							CasinoGames.DrawCardTime = -1;
						}
						else if (CasinoGames.Playercards == 1)
						{
							CasinoGames.Stage = 5f;
							Function.Call(argument1: "MINIGAME_BJACK_DEALER_" + (CasinoGames.PlayerCard_1 + CasinoGames.PlayerCard_2), hash: Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, argument0: CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.BlackjackDealerTables), argument2: "SPEECH_PARAMS_STANDARD", argument3: false);
							CasinoGames.DrawCardTime = Game.GameTime + 1000;
						}
						else if (CasinoGames.Playercards == 2)
						{
							CasinoGames.Stage = 5f;
							Function.Call(argument1: "MINIGAME_BJACK_DEALER_" + (CasinoGames.PlayerCard_1 + CasinoGames.PlayerCard_2 + CasinoGames.PlayerCard_3), hash: Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, argument0: CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.BlackjackDealerTables), argument2: "SPEECH_PARAMS_STANDARD", argument3: false);
							CasinoGames.DrawCardTime = Game.GameTime + 1000;
						}
						else if (CasinoGames.Playercards == 3)
						{
							CasinoGames.Stage = 5f;
							Function.Call(argument1: "MINIGAME_BJACK_DEALER_" + (CasinoGames.PlayerCard_1 + CasinoGames.PlayerCard_2 + CasinoGames.PlayerCard_3 + CasinoGames.PlayerCard_4), hash: Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, argument0: CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.BlackjackDealerTables), argument2: "SPEECH_PARAMS_STANDARD", argument3: false);
							CasinoGames.DrawCardTime = Game.GameTime + 1000;
						}
						else if (CasinoGames.Playercards == 4)
						{
							if (CasinoGames.BlackJackSplit)
							{
								CasinoGames.Playercards++;
							}
							CasinoGames.Stage = 5f;
							Function.Call(argument1: "MINIGAME_BJACK_DEALER_" + (CasinoGames.PlayerCard_1 + CasinoGames.PlayerCard_2 + CasinoGames.PlayerCard_3 + CasinoGames.PlayerCard_4 + CasinoGames.PlayerCard_5), hash: Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, argument0: CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.BlackjackDealerTables), argument2: "SPEECH_PARAMS_STANDARD", argument3: false);
							CasinoGames.DrawCardTime = Game.GameTime + 1000;
						}
						else if (CasinoGames.Playercards == 5)
						{
							CasinoGames.Stage = 5f;
							Function.Call(argument1: "MINIGAME_BJACK_DEALER_" + (CasinoGames.PlayerCard_1 + CasinoGames.PlayerCard_2 + CasinoGames.PlayerCard_3 + CasinoGames.PlayerCard_4 + CasinoGames.PlayerCard_5), hash: Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, argument0: CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.BlackjackDealerTables), argument2: "SPEECH_PARAMS_STANDARD", argument3: false);
							CasinoGames.DrawCardTime = Game.GameTime + 1000;
						}
					}
				}
				if (CasinoGames.Stage == 150f && Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene1) >= 0.99f)
				{
					int num6 = 0;
					if (CasinoGames.DealerCards == 1)
					{
						num6 = CasinoGames.DealerCard_1;
					}
					if (CasinoGames.DealerCards == 2)
					{
						num6 = CasinoGames.DealerCard_1 + CasinoGames.DealerCard_2;
					}
					if (CasinoGames.DealerCards == 3)
					{
						num6 = CasinoGames.DealerCard_1 + CasinoGames.DealerCard_2 + CasinoGames.DealerCard_3;
					}
					if (CasinoGames.DealerCards == 4)
					{
						num6 = CasinoGames.DealerCard_1 + CasinoGames.DealerCard_2 + CasinoGames.DealerCard_3 + CasinoGames.DealerCard_4;
					}
					if (CasinoGames.DealerCards == 5)
					{
						num6 = CasinoGames.DealerCard_1 + CasinoGames.DealerCard_2 + CasinoGames.DealerCard_3 + CasinoGames.DealerCard_4 + CasinoGames.DealerCard_5;
					}
					if (CasinoGames.DealerCards == 6)
					{
						num6 = CasinoGames.DealerCard_1 + CasinoGames.DealerCard_2 + CasinoGames.DealerCard_3 + CasinoGames.DealerCard_4 + CasinoGames.DealerCard_5 + CasinoGames.DealerCard_6;
					}
					if (num6 <= 20)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.BlackjackDealerTables), "MINIGAME_BJACK_DEALER_" + num6, "SPEECH_PARAMS_STANDARD", false);
					}
					if (num6 == 21)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.BlackjackDealerTables), "MINIGAME_BJACK_DEALER_BLACKJACK", "SPEECH_PARAMS_STANDARD", false);
					}
					if (num6 >= 22)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.BlackjackDealerTables), "MINIGAME_DEALER_BUSTS", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.DealerCards >= CasinoGames.Playercards)
					{
						CasinoGames.Stage = 350f;
						return;
					}
					if (num6 >= 18 && num6 <= 21)
					{
						CasinoGames.Stage = 350f;
						return;
					}
					if (num6 >= 22)
					{
						CasinoGames.Stage = 350f;
						return;
					}
					CasinoGames.DetatchAllCards();
					Vector3 position18 = Game.Player.Character.Position;
					CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading - 90f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base"), "base", 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, "base", CasinoGames.LoadDict("anim_casino_b@amb@casino@games@blackjack@ped_male@regular@02b@base"), 16f, -16f, 0, 1148846080);
					string dict31 = "anim_casino_b@amb@casino@games@blackjack@dealer";
					string text38 = "deal_card_self_second_card";
					if (CasinoGames.DealerCards > 2)
					{
						text38 = "deal_card_self_second_card";
					}
					if (CasinoGames.DealerCards == 3)
					{
						text38 = "deal_card_self_card_06";
					}
					if (CasinoGames.DealerCards == 4)
					{
						text38 = "female_deal_card_self_card_10";
					}
					Vector3 dealerAnimOrigin23 = CasinoGames.GetDealerAnimOrigin();
					CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin23.X, dealerAnimOrigin23.Y, dealerAnimOrigin23.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene1, CasinoGames.LoadDict(dict31), text38, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene1, text38, CasinoGames.LoadDict(dict31), 16f, -16f, 0, 1148846080);
					if (CasinoGames.DealerCards == 0)
					{
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.ClosestDealerTable.Card1, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					}
					if (CasinoGames.DealerCards == 1)
					{
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.ClosestDealerTable.Card2, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					}
					if (CasinoGames.DealerCards == 2)
					{
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.ClosestDealerTable.Card3, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					}
					if (CasinoGames.DealerCards == 3)
					{
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.ClosestDealerTable.Card4, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					}
					if (CasinoGames.DealerCards == 4)
					{
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.ClosestDealerTable.Card5, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					}
					if (CasinoGames.DealerCards == 5)
					{
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, CasinoGames.ClosestDealerTable.Card6, CasinoGames.ClosestDealer, CasinoGames.ClosestDealer.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					}
					Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.BlackjackDealerTables), "MINIGAME_BJACK_DEALER_" + num6, "SPEECH_PARAMS_STANDARD", false);
					Script.Wait(1);
					CasinoGames.DealerCards++;
				}
				if ((CasinoGames.Stage == 300f || CasinoGames.Stage == 350f) && (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene1) >= 0.99f || CasinoGames.Stage == 350f))
				{
					CasinoGames.DealerCard_1 = CasinoGames.ClosestDealerTable.Card1num;
					CasinoGames.DealerCard_2 = CasinoGames.ClosestDealerTable.Card2num;
					if (CasinoGames.Playercards < 2)
					{
						CasinoGames.DealerCard_2 = 0;
					}
					CasinoGames.DealerCard_3 = CasinoGames.ClosestDealerTable.Card3num;
					if (CasinoGames.Playercards < 3)
					{
						CasinoGames.DealerCard_3 = 0;
					}
					CasinoGames.DealerCard_4 = CasinoGames.ClosestDealerTable.Card4num;
					if (CasinoGames.Playercards < 4)
					{
						CasinoGames.DealerCard_4 = 0;
					}
					CasinoGames.DealerCard_5 = CasinoGames.ClosestDealerTable.Card5num;
					if (CasinoGames.Playercards < 5)
					{
						CasinoGames.DealerCard_5 = 0;
					}
					CasinoGames.DealerCard_6 = CasinoGames.ClosestDealerTable.Card6num;
					if (CasinoGames.Playercards < 6)
					{
						CasinoGames.DealerCard_6 = 0;
					}
					CasinoGames.CalculateBlackJack(CasinoGames.ClosestDealerTable);
					CasinoGames.Drawcards = false;
					CasinoGames.Endgame = true;
					CasinoGames.ResetCardDeck();
				}
			}
			if (CasinoGames.Placebet || CasinoGames.Endgame)
			{
				return;
			}
			CasinoGames.DealerCardsDrawn = 0;
			if (!Game.IsControlJustPressed(Control.Cover))
			{
				return;
			}
			CasinoGames.BlackJackSplit = false;
			CasinoGames.BlackJackDoubleDown = false;
			CasinoGames.DealerCards = 0;
			try
			{
				CasinoGames.BlackjackBackupDealerCard1_FiveCards = 0;
				CasinoGames.BlackjackBackupDealerCard1_FourCards = 0;
				CasinoGames.BlackjackBackupDealerCard1_ThreeCards = 0;
				if (CasinoGames.Bet < (float)CasinoGames.Chips_Amount)
				{
					CasinoGames.ClosestDealer = CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.BlackjackDealerTables);
					CasinoGames.ClosestTable = CasinoGames.GetClosestTable(Game.Player.Character.Position, CasinoGames.BlackjackDealerTables);
					CasinoGames.ClosestDealerTable = CasinoGames.GetClosestDealerTable(Game.Player.Character.Position, CasinoGames.BlackjackDealerTables);
					CasinoGames.PlayerOriginPosition = CasinoGames.GetPlayerAnimOriginH(CasinoGames.SeatSittingIn, CasinoGames.TableType.Blackjack).Item1;
					CasinoGames.PlayerOriginHeading = CasinoGames.GetPlayerAnimOriginH(CasinoGames.SeatSittingIn, CasinoGames.TableType.Blackjack).Item2;
					CasinoGames.Stage = -1f;
					Notification.Show("~t~Blackjack~w~ Player bet:" + CasinoGames.Bet);
					CasinoGames.Placebet = true;
					CasinoGames.PresetChoosen = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, CasinoGames.DealerPresetCars.Count);
					int presetChoosen = CasinoGames.PresetChoosen;
					string text39 = "vw_prop_casino_cards_single";
					text39 = ((CasinoGames.PlayerDeck != 0 && CasinoGames.PlayerDeck != 1) ? "vw_prop_vw_jo_char_01a" : "vw_prop_casino_cards_single");
					string card = CasinoGames.DealerPresetCars[presetChoosen].Card1;
					CasinoGames.ClosestDealerTable.Card1num = CasinoGames.Set_PlayerCard_Blackjack(card, 1);
					Script.Wait(1);
					Prop prop7 = GlobalHelper.CreateProp(text39, offsetPosition, true, false);
					prop7.IsPositionFrozen = true;
					CasinoGames.ClosestDealerTable.Card1 = prop7;
					CasinoGames.ClosestDealerTable.Card1Id = card;
					CasinoGames.FrontFace_DealerCard1 = CasinoGames.GetCardOfType(CasinoGames.CardVarient.Any, CasinoGames.CardNumberToID(CasinoGames.Set_PlayerCard_Blackjack(card, 1)));
					CasinoGames.Backface_DealerCard1 = GlobalHelper.CreateProp(text39, offsetPosition, true, false);
					prop7.IsPositionFrozen = true;
					CasinoGames.Backface_DealerCard1.AttachTo(CasinoGames.ClosestDealerTable.Card1, new Vector3(0f, 0f, 0.001f), new Vector3(0f, 180f, 0f));
					CasinoGames.Backface_DealerCard1.Opacity = 0;
					CasinoGames.DealerPresetCars[presetChoosen].Card2 = CasinoGames.GetRandomCard_Blackjack_Dealer(CasinoGames.ClosestDealerTable.Card1num);
					card = CasinoGames.DealerPresetCars[presetChoosen].Card2;
					CasinoGames.ClosestDealerTable.Card2num = CasinoGames.Set_PlayerCard_Blackjack(card, 2);
					Script.Wait(1);
					prop7 = GlobalHelper.CreateProp(card, offsetPosition, true, false);
					prop7.IsPositionFrozen = true;
					CasinoGames.ClosestDealerTable.Card2 = prop7;
					CasinoGames.ClosestDealerTable.Card2Id = card;
					CasinoGames.DealerPresetCars[presetChoosen].Card3 = CasinoGames.GetRandomCard_Blackjack_Dealer(CasinoGames.ClosestDealerTable.Card1num + CasinoGames.ClosestDealerTable.Card2num);
					card = CasinoGames.DealerPresetCars[presetChoosen].Card3;
					CasinoGames.ClosestDealerTable.Card3num = CasinoGames.Set_PlayerCard_Blackjack(card, 3);
					Script.Wait(1);
					prop7 = GlobalHelper.CreateProp(card, offsetPosition, true, false);
					prop7.IsPositionFrozen = true;
					CasinoGames.ClosestDealerTable.Card3 = prop7;
					CasinoGames.ClosestDealerTable.Card3Id = card;
					CasinoGames.DealerPresetCars[presetChoosen].Card4 = CasinoGames.GetRandomCard_Blackjack_Dealer(CasinoGames.ClosestDealerTable.Card1num + CasinoGames.ClosestDealerTable.Card2num + CasinoGames.ClosestDealerTable.Card3num);
					card = CasinoGames.DealerPresetCars[presetChoosen].Card4;
					CasinoGames.ClosestDealerTable.Card4num = CasinoGames.Set_PlayerCard_Blackjack(card, 4);
					Script.Wait(1);
					prop7 = GlobalHelper.CreateProp(card, offsetPosition, true, false);
					prop7.IsPositionFrozen = true;
					CasinoGames.ClosestDealerTable.Card4 = prop7;
					CasinoGames.ClosestDealerTable.Card4Id = card;
					CasinoGames.DealerPresetCars[presetChoosen].Card5 = CasinoGames.GetRandomCard_Blackjack_Dealer(CasinoGames.ClosestDealerTable.Card1num + CasinoGames.ClosestDealerTable.Card2num + CasinoGames.ClosestDealerTable.Card3num + CasinoGames.ClosestDealerTable.Card4num);
					card = CasinoGames.DealerPresetCars[presetChoosen].Card5;
					CasinoGames.ClosestDealerTable.Card5num = CasinoGames.Set_PlayerCard_Blackjack(card, 5);
					Script.Wait(1);
					prop7 = GlobalHelper.CreateProp(card, offsetPosition, true, false);
					prop7.IsPositionFrozen = true;
					CasinoGames.ClosestDealerTable.Card5 = prop7;
					CasinoGames.ClosestDealerTable.Card5Id = card;
					CasinoGames.DealerPresetCars[presetChoosen].Card6 = CasinoGames.GetRandomCard_Blackjack_Dealer(CasinoGames.ClosestDealerTable.Card1num + CasinoGames.ClosestDealerTable.Card2num + CasinoGames.ClosestDealerTable.Card3num + CasinoGames.ClosestDealerTable.Card4num + CasinoGames.ClosestDealerTable.Card5num);
					card = CasinoGames.DealerPresetCars[presetChoosen].Card6;
					CasinoGames.ClosestDealerTable.Card6num = CasinoGames.Set_PlayerCard_Blackjack(card, 6);
					Script.Wait(1);
					prop7 = GlobalHelper.CreateProp(card, offsetPosition, true, false);
					prop7.IsPositionFrozen = true;
					CasinoGames.ClosestDealerTable.Card6 = prop7;
					CasinoGames.ClosestDealerTable.Card6Id = card;
					float bet = CasinoGames.Bet;
					if (CasinoGames.BlackjackPlayerInitialBet != null)
					{
						CasinoGames.BlackjackPlayerInitialBet.Delete();
					}
					Prop prop8 = GlobalHelper.CreateProp(CasinoGames.RequestModel(CasinoGames.GetBetChipModel((int)CasinoGames.Bet, true)), CasinoGames.ReturnChipPosition(CasinoGames.BlackjackDealerTables, CasinoGames.BetTypeId.BlackJack), true, false);
					prop8.IsPositionFrozen = true;
					prop8.Heading = CasinoGames.ReturnChipAngle(CasinoGames.PokerDealerTables);
					CasinoGames.Chips.Add(prop8);
					CasinoGames.BlackjackPlayerInitialBet = prop8;
					CasinoGames.BlackjackPlayerInitialBet.Opacity = 0;
				}
				if (CasinoGames.Bet > (float)CasinoGames.Chips_Amount)
				{
					Notification.Show("you do not have enough chips for this bet");
					CasinoGames.Placebet2 = false;
					CasinoGames.Placebet = false;
					return;
				}
				CasinoGames.DealerCardsDrawn = 1;
				CasinoGames.InitalBet = CasinoGames.Bet;
				CasinoGames.DealerCardsDrawn = 1;
				CasinoGames.DetatchAllCards();
				CasinoGames.DealerCards = 0;
				string dict32 = "anim_casino_b@amb@casino@games@blackjack@dealer";
				string text40 = "place_bet_request";
				Vector3 dealerAnimOrigin24 = CasinoGames.GetDealerAnimOrigin();
				CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin24.X, dealerAnimOrigin24.Y, dealerAnimOrigin24.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene1, CasinoGames.LoadDict(dict32), text40, 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene1, text40, CasinoGames.LoadDict(dict32), 16f, -16f, 0, 1148846080);
				dict32 = "anim_casino_b@amb@casino@games@blackjack@player";
				text40 = "place_bet_large";
				CasinoGames.LoadDict(dict32);
				CasinoGames.GetPlayerAnimOriginH(CasinoGames.SeatSittingIn, CasinoGames.TableType.Blackjack);
				CasinoGames.GetPlayerAnimOriginH(CasinoGames.SeatSittingIn, CasinoGames.TableType.Blackjack);
				CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict(dict32), text40, 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, text40, CasinoGames.LoadDict(dict32), 16f, -16f, 0, 1148846080);
				if (CasinoGames.ClosestDealer.Gender == Gender.Female)
				{
					string text41 = CasinoGames.func_440(23);
					string text42 = CasinoGames.func_435(6);
					Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_WITH_VOICE_NATIVE, CasinoGames.ClosestDealer, text41, "S_F_Y_Casino_01_ASIAN_01", text42, 0);
				}
				if (CasinoGames.ClosestDealer.Gender == Gender.Male)
				{
					string text43 = CasinoGames.func_440(23);
					string text44 = CasinoGames.func_435(6);
					Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_WITH_VOICE_NATIVE, CasinoGames.ClosestDealer, text43, "S_M_Y_Casino_01_WHITE_01", text44, 0);
				}
				CasinoGames.Stage = 10f;
			}
			catch (Exception)
			{
				Notification.Show("Player did not enter a valid bet");
			}
			Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.BlackjackDealerTables), "MINIGAME_DEALER_PLACE_CHIPS", "SPEECH_PARAMS_STANDARD", false);
		}
	}
}
