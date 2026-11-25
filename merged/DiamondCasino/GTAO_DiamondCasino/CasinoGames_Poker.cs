using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace GTAO_DiamondCasino
{
	internal class CasinoGames_Poker : Script
	{
		public static bool RunMethodNow()
		{
			bool result = true;
			if (CasinoGames.CurrentGame != CasinoGames.CasinoGamePlaying.Poker)
			{
				_Debug.CasinoGames_Poker = 0;
				result = false;
			}
			return result;
		}

		public CasinoGames_Poker()
		{
			if (_Debug.CasinoGames_Poker != -1)
			{
				base.Tick += OnTick;
			}
		}

		public static void OnTick(object sender, EventArgs e)
		{
			_Debug.CasinoGames_Poker = 0;
			if (CasinoGames.CurrentGame != CasinoGames.CasinoGamePlaying.Poker || CasinoGames.CurrentGame != CasinoGames.CasinoGamePlaying.Poker)
			{
				return;
			}
			_Debug.CasinoGames_Poker = 1;
			if (CasinoGames.CurrentGame != CasinoGames.CasinoGamePlaying.Poker)
			{
				return;
			}
			if (!CasinoGames.Playing_Poker)
			{
				CasinoGames.CurrentGame = CasinoGames.CasinoGamePlaying.None;
			}
			if (!CasinoGames.Playing_Poker)
			{
				return;
			}
			CasinoGames.Poker_ManageDealerCardDeck();
			if (!CasinoGames.Playing_Poker)
			{
				return;
			}
			Vector3 offsetPosition = Game.Player.Character.GetOffsetPosition(new Vector3(-0.05f, 0.9f, -0.45f));
			Vector3 offsetPosition2 = Game.Player.Character.GetOffsetPosition(new Vector3(0.05f, 0.9f, -0.45f));
			Vector3 offsetPosition3 = Game.Player.Character.GetOffsetPosition(new Vector3(-0.15f, 0.9f, -0.45f));
			Game.Player.Character.GetOffsetPosition(new Vector3(0.15f, 0.9f, -0.45f));
			Game.Player.Character.GetOffsetPosition(new Vector3(-0.25f, 0.9f, -0.45f));
			Game.Player.Character.GetOffsetPosition(new Vector3(0.25f, 0.9f, -0.45f));
			Vector3 offsetPosition4 = Game.Player.Character.GetOffsetPosition(new Vector3(-0.05f, 0.55f, -0.45f));
			Vector3 offsetPosition5 = Game.Player.Character.GetOffsetPosition(new Vector3(0.05f, 0.55f, -0.45f));
			Vector3 offsetPosition6 = Game.Player.Character.GetOffsetPosition(new Vector3(-0.15f, 0.55f, -0.45f));
			Game.Player.Character.GetOffsetPosition(new Vector3(0.15f, 0.55f, -0.45f));
			Game.Player.Character.GetOffsetPosition(new Vector3(-0.25f, 0.55f, -0.45f));
			Game.Player.Character.GetOffsetPosition(new Vector3(0.25f, 0.55f, -0.45f));
			Game.Player.Character.GetOffsetPosition(new Vector3(0.4f, 0.6f, 0f));
			Game.Player.Character.GetOffsetPosition(new Vector3(0.4f, 0.7f, 0f));
			if (CasinoGames.Endgame)
			{
				Prop prop = (CasinoGames.ClosestTable = CasinoGames.GetClosestTable(Game.Player.Character.Position, CasinoGames.PokerDealerTables));
				if (prop != null)
				{
					if (CasinoGames.Stage == 14f || CasinoGames.Stage == 50f || CasinoGames.Stage == 51f || CasinoGames.Stage == 52f)
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
								CasinoGames.RoulleteCam = GlobalHelper.CreateCamera(prop.GetOffsetPosition(new Vector3(0f, 0.1f, 1.35f)), new Vector3(-90f, 0f, prop.Heading), 60f);
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
				if (Game.IsControlJustPressed(Control.Cover))
				{
					CasinoGames.Playercards = 0;
					foreach (DealerTable pokerDealerTable in CasinoGames.PokerDealerTables)
					{
						if (pokerDealerTable.Dealer != null)
						{
							CasinoGames.ResetDealerPos(pokerDealerTable);
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
					}
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
					}
					foreach (DealerTable rouletteDealerTable in CasinoGames.RouletteDealerTables)
					{
						if (rouletteDealerTable.Dealer != null)
						{
							CasinoGames.ResetDealerPos(rouletteDealerTable);
						}
						if (rouletteDealerTable.Card1 != null)
						{
							rouletteDealerTable.Card1.Delete();
						}
						if (rouletteDealerTable.Card2 != null)
						{
							rouletteDealerTable.Card2.Delete();
						}
						if (rouletteDealerTable.Card3 != null)
						{
							rouletteDealerTable.Card3.Delete();
						}
					}
					foreach (Prop chip in CasinoGames.Chips)
					{
						if (chip != null)
						{
							EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(chip, false, 0));
						}
					}
					CasinoGames.Placebet2 = false;
					CasinoGames.Editbet = false;
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
					CasinoGames.Bet = 0f;
					CasinoGames.DealerCard_1 = 0;
					CasinoGames.DealerCard_2 = 0;
					CasinoGames.DealerCard_3 = 0;
					CasinoGames.DealerCard_4 = 0;
					CasinoGames.DealerCard_5 = 0;
					CasinoGames.DealerCard_6 = 0;
					CasinoGames.Stage = 52f;
					CasinoGames.ResetCardDeck();
					CasinoGames.LookingAtDealerCards = false;
					if (CasinoGames.RoulleteCam != null)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 1, 3000, 1, 0);
						CasinoGames.RoulleteCam.IsActive = false;
						CasinoGames.RoulleteCam.Delete();
					}
					CasinoGames.Stage = 53f;
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
			if (CasinoGames.Placebet)
			{
				if (CasinoGames.Stage == 1f)
				{
					if (CasinoGames.Bet_PairPlus <= 0)
					{
						CasinoGames.Stage = 2f;
					}
					else if (CasinoGames.Bet_PairPlus > 0)
					{
						string dict = "anim_casino_b@amb@casino@games@threecardpoker@player";
						string text = "bet_plus";
						CasinoGames.LoadDict(dict);
						CasinoGames.GetPlayerAnimOriginH(CasinoGames.SeatSittingIn, CasinoGames.TableType.Blackjack);
						CasinoGames.GetPlayerAnimOriginH(CasinoGames.SeatSittingIn, CasinoGames.TableType.Blackjack);
						CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict(dict), text, 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, text, CasinoGames.LoadDict(dict), 16f, -16f, 0, 1148846080);
						CasinoGames.Stage = 3f;
					}
				}
				if ((CasinoGames.Stage == 2f || CasinoGames.Stage == 3f) && (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216 || CasinoGames.Stage == 2f) && CasinoGames.Playercards < 3)
				{
					CasinoGames.Playercards = 3;
					CasinoGames.Stage = 0f;
					foreach (DealerTable pokerDealerTable2 in CasinoGames.PokerDealerTables)
					{
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
					}
					foreach (DealerTable blackjackDealerTable2 in CasinoGames.BlackjackDealerTables)
					{
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
					}
					foreach (DealerTable rouletteDealerTable2 in CasinoGames.RouletteDealerTables)
					{
						if (rouletteDealerTable2.Card1 != null)
						{
							rouletteDealerTable2.Card1.Delete();
						}
						if (rouletteDealerTable2.Card2 != null)
						{
							rouletteDealerTable2.Card2.Delete();
						}
						if (rouletteDealerTable2.Card3 != null)
						{
							rouletteDealerTable2.Card3.Delete();
						}
					}
					CasinoGames.ClosestDealer = CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.PokerDealerTables);
					CasinoGames.ClosestTable = CasinoGames.GetClosestTable(Game.Player.Character.Position, CasinoGames.PokerDealerTables);
					CasinoGames.ClosestDealerTable = CasinoGames.GetClosestDealerTable(Game.Player.Character.Position, CasinoGames.PokerDealerTables);
					if (Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 100f) >= 25.28f)
					{
						CasinoGames.CardVarient cardRandomVarient = CasinoGames.GetCardRandomVarient();
						CasinoGames.GetCardRandomVarient();
						CasinoGames.GetCardRandomVarient();
						int randomCardPlayer_ReturnID_ForceEqualChance = CasinoGames.GetRandomCardPlayer_ReturnID_ForceEqualChance(CasinoGames.CardID.Two, CasinoGames.CardID.Ace);
						int randomCardPlayer_ReturnID_ForceEqualChance2 = CasinoGames.GetRandomCardPlayer_ReturnID_ForceEqualChance(CasinoGames.CardID.Two, CasinoGames.CardID.Ace);
						int randomCardPlayer_ReturnID_ForceEqualChance3 = CasinoGames.GetRandomCardPlayer_ReturnID_ForceEqualChance(CasinoGames.CardID.Two, CasinoGames.CardID.Ace);
						Script.Wait(1);
						string cardOfType = CasinoGames.GetCardOfType(cardRandomVarient, (CasinoGames.CardID)randomCardPlayer_ReturnID_ForceEqualChance);
						CasinoGames.DealerCard_1 = CasinoGames.Set_DealerCardPoker(cardOfType, 1);
						Prop prop2 = GlobalHelper.CreateProp(CasinoGames.RequestModel(cardOfType), offsetPosition3, true, false);
						prop2.IsPositionFrozen = true;
						CasinoGames.ClosestDealerTable.Card1 = prop2;
						Vector3 position = prop2.Position;
						CasinoGames.CardC = prop2;
						prop2.Position = new Vector3(position.X, position.Y, position.Z - CasinoGames.GetTableHeight_Cards(CasinoGames.CurrentTable));
						prop2.Heading = Game.Player.Character.Heading;
						CasinoGames.Chips.Add(prop2);
						CasinoGames.CardC.Rotation = new Vector3(CasinoGames.CardC.Rotation.X, 180f, CasinoGames.CardC.Rotation.Z);
						Script.Wait(1);
						string cardOfType2 = CasinoGames.GetCardOfType(cardRandomVarient, (CasinoGames.CardID)randomCardPlayer_ReturnID_ForceEqualChance2);
						CasinoGames.DealerCard_2 = CasinoGames.Set_DealerCardPoker(cardOfType2, 2);
						prop2 = GlobalHelper.CreateProp(CasinoGames.RequestModel(cardOfType2), offsetPosition2, true, false);
						prop2.IsPositionFrozen = true;
						position = prop2.Position;
						CasinoGames.CardB = prop2;
						CasinoGames.ClosestDealerTable.Card2 = prop2;
						prop2.Position = new Vector3(position.X, position.Y, position.Z - CasinoGames.GetTableHeight_Cards(CasinoGames.CurrentTable));
						prop2.Heading = Game.Player.Character.Heading;
						CasinoGames.Chips.Add(prop2);
						CasinoGames.CardB.Rotation = new Vector3(CasinoGames.CardB.Rotation.X, 180f, CasinoGames.CardB.Rotation.Z);
						Script.Wait(1);
						string cardOfType3 = CasinoGames.GetCardOfType(cardRandomVarient, (CasinoGames.CardID)randomCardPlayer_ReturnID_ForceEqualChance3);
						CasinoGames.DealerCard_3 = CasinoGames.Set_DealerCardPoker(cardOfType3, 3);
						prop2 = GlobalHelper.CreateProp(CasinoGames.RequestModel(cardOfType3), offsetPosition, true, false);
						prop2.IsPositionFrozen = true;
						position = prop2.Position;
						CasinoGames.CardA = prop2;
						CasinoGames.ClosestDealerTable.Card3 = prop2;
						prop2.Position = new Vector3(position.X, position.Y, position.Z - CasinoGames.GetTableHeight_Cards(CasinoGames.CurrentTable));
						prop2.Heading = Game.Player.Character.Heading;
						CasinoGames.Chips.Add(prop2);
						CasinoGames.CardA.Rotation = new Vector3(CasinoGames.CardA.Rotation.X, 180f, CasinoGames.CardA.Rotation.Z);
					}
					else
					{
						CasinoGames.CardVarient cardRandomVarient2 = CasinoGames.GetCardRandomVarient();
						CasinoGames.GetCardRandomVarient();
						CasinoGames.GetCardRandomVarient();
						int num = CasinoGames.GetRandomCardPlayer_ReturnID(CasinoGames.CardID.Four);
						int num2 = 4;
						if (num == 4)
						{
							num2 = 6;
						}
						int num3 = CasinoGames.GetRandomCardPlayer_ReturnID(CasinoGames.CardID.Six);
						int randomCardPlayer_ReturnID_ForceEqualChance4 = CasinoGames.GetRandomCardPlayer_ReturnID_ForceEqualChance(CasinoGames.CardID.Queen, CasinoGames.CardID.Ace);
						float num4 = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 100f);
						if (num4 <= 33f)
						{
							num = CasinoGames.GetRandomCardPlayer_ReturnID(CasinoGames.CardID.Four);
							num2 = 4;
							if (num == 4)
							{
								num2 = 6;
							}
							num3 = CasinoGames.GetRandomCardPlayer_ReturnID_ForceEqualChance((CasinoGames.CardID)num2, CasinoGames.CardID.Ten);
							randomCardPlayer_ReturnID_ForceEqualChance4 = CasinoGames.GetRandomCardPlayer_ReturnID_ForceEqualChance(CasinoGames.CardID.Queen, CasinoGames.CardID.Ace);
						}
						if (num4 >= 34f && num4 <= 66f)
						{
							num = CasinoGames.GetRandomCardPlayer_ReturnID(CasinoGames.CardID.Four);
							num2 = 4;
							if (num == 4)
							{
								num2 = 6;
							}
							num3 = CasinoGames.GetRandomCardPlayer_ReturnID_ForceEqualChance(CasinoGames.CardID.Queen, CasinoGames.CardID.Ace);
							randomCardPlayer_ReturnID_ForceEqualChance4 = CasinoGames.GetRandomCardPlayer_ReturnID_ForceEqualChance((CasinoGames.CardID)num2, CasinoGames.CardID.Ten);
						}
						if (num4 >= 67f && num4 <= 100f)
						{
							num = CasinoGames.GetRandomCardPlayer_ReturnID_ForceEqualChance(CasinoGames.CardID.Queen, CasinoGames.CardID.Ace);
							num3 = CasinoGames.GetRandomCardPlayer_ReturnID(CasinoGames.CardID.Four);
							num2 = 4;
							if (num3 == 4)
							{
								num2 = 6;
							}
							randomCardPlayer_ReturnID_ForceEqualChance4 = CasinoGames.GetRandomCardPlayer_ReturnID_ForceEqualChance((CasinoGames.CardID)num2, CasinoGames.CardID.Ten);
						}
						Script.Wait(1);
						string cardOfType4 = CasinoGames.GetCardOfType(cardRandomVarient2, (CasinoGames.CardID)num);
						CasinoGames.DealerCard_1 = CasinoGames.Set_DealerCardPoker(cardOfType4, 1);
						Prop prop3 = GlobalHelper.CreateProp(CasinoGames.RequestModel(cardOfType4), offsetPosition3, true, false);
						prop3.IsPositionFrozen = true;
						CasinoGames.ClosestDealerTable.Card1 = prop3;
						Vector3 position2 = prop3.Position;
						CasinoGames.CardC = prop3;
						prop3.Position = new Vector3(position2.X, position2.Y, position2.Z - CasinoGames.GetTableHeight_Cards(CasinoGames.CurrentTable));
						prop3.Heading = Game.Player.Character.Heading;
						CasinoGames.Chips.Add(prop3);
						CasinoGames.CardC.Rotation = new Vector3(CasinoGames.CardC.Rotation.X, 180f, CasinoGames.CardC.Rotation.Z);
						Script.Wait(1);
						string cardOfType5 = CasinoGames.GetCardOfType(cardRandomVarient2, (CasinoGames.CardID)num3);
						CasinoGames.DealerCard_2 = CasinoGames.Set_DealerCardPoker(cardOfType5, 2);
						prop3 = GlobalHelper.CreateProp(CasinoGames.RequestModel(cardOfType5), offsetPosition2, true, false);
						prop3.IsPositionFrozen = true;
						position2 = prop3.Position;
						CasinoGames.CardB = prop3;
						CasinoGames.ClosestDealerTable.Card2 = prop3;
						prop3.Position = new Vector3(position2.X, position2.Y, position2.Z - CasinoGames.GetTableHeight_Cards(CasinoGames.CurrentTable));
						prop3.Heading = Game.Player.Character.Heading;
						CasinoGames.Chips.Add(prop3);
						CasinoGames.CardB.Rotation = new Vector3(CasinoGames.CardB.Rotation.X, 180f, CasinoGames.CardB.Rotation.Z);
						Script.Wait(1);
						string cardOfType6 = CasinoGames.GetCardOfType(cardRandomVarient2, (CasinoGames.CardID)randomCardPlayer_ReturnID_ForceEqualChance4);
						CasinoGames.DealerCard_3 = CasinoGames.Set_DealerCardPoker(cardOfType6, 3);
						prop3 = GlobalHelper.CreateProp(CasinoGames.RequestModel(cardOfType6), offsetPosition, true, false);
						prop3.IsPositionFrozen = true;
						position2 = prop3.Position;
						CasinoGames.CardA = prop3;
						CasinoGames.ClosestDealerTable.Card3 = prop3;
						prop3.Position = new Vector3(position2.X, position2.Y, position2.Z - CasinoGames.GetTableHeight_Cards(CasinoGames.CurrentTable));
						prop3.Heading = Game.Player.Character.Heading;
						CasinoGames.Chips.Add(prop3);
						CasinoGames.CardA.Rotation = new Vector3(CasinoGames.CardA.Rotation.X, 180f, CasinoGames.CardA.Rotation.Z);
					}
					float num5 = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 100f);
					float num6 = 20f;
					float num7 = 15f;
					float num8 = 12.5f;
					float num9 = 7.5f;
					float num10 = 2.5f;
					if (num5 <= num6)
					{
						if (num5 <= num6 && num5 >= num7)
						{
							CasinoGames.CardVarient cardRandomVarient3 = CasinoGames.GetCardRandomVarient();
							int randomCardPlayer_ReturnID = CasinoGames.GetRandomCardPlayer_ReturnID();
							int randomCardPlayer_ReturnID2 = CasinoGames.GetRandomCardPlayer_ReturnID();
							float num11 = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 100f);
							List<string> list = new List<string>();
							string cardOfType7 = CasinoGames.GetCardOfType(cardRandomVarient3, (CasinoGames.CardID)randomCardPlayer_ReturnID);
							list.Add(cardOfType7);
							while (list.Count < 3)
							{
								Script.Wait(1);
								if (list.Count == 1)
								{
									cardOfType7 = CasinoGames.GetCardOfType(CasinoGames.GetCardRandomVarient(), (CasinoGames.CardID)randomCardPlayer_ReturnID2);
									if (!list.Contains(cardOfType7))
									{
										list.Add(cardOfType7);
									}
								}
								if (list.Count == 2)
								{
									cardOfType7 = CasinoGames.GetCardOfType(CasinoGames.GetCardRandomVarient(), (CasinoGames.CardID)randomCardPlayer_ReturnID);
									if (!list.Contains(cardOfType7))
									{
										list.Add(cardOfType7);
									}
								}
							}
							if (num11 <= 33f)
							{
								CasinoGames.PlayerCard_1 = CasinoGames.Set_PlayerCardPoker(list[0], 1);
								CasinoGames.PlayerCard_1_ID = list[0];
								CasinoGames.PlayerCard_2 = CasinoGames.Set_PlayerCardPoker(list[1], 2);
								CasinoGames.PlayerCard_2_ID = list[1];
								CasinoGames.PlayerCard_3 = CasinoGames.Set_PlayerCardPoker(list[2], 3);
								CasinoGames.PlayerCard_3_ID = list[2];
							}
							if (num11 >= 34f && num11 <= 67f)
							{
								CasinoGames.PlayerCard_1 = CasinoGames.Set_PlayerCardPoker(list[1], 1);
								CasinoGames.PlayerCard_1_ID = list[1];
								CasinoGames.PlayerCard_2 = CasinoGames.Set_PlayerCardPoker(list[0], 2);
								CasinoGames.PlayerCard_2_ID = list[0];
								CasinoGames.PlayerCard_3 = CasinoGames.Set_PlayerCardPoker(list[2], 3);
								CasinoGames.PlayerCard_3_ID = list[2];
							}
							if (num11 >= 67f)
							{
								CasinoGames.PlayerCard_1 = CasinoGames.Set_PlayerCardPoker(list[2], 1);
								CasinoGames.PlayerCard_1_ID = list[2];
								CasinoGames.PlayerCard_2 = CasinoGames.Set_PlayerCardPoker(list[1], 2);
								CasinoGames.PlayerCard_2_ID = list[1];
								CasinoGames.PlayerCard_3 = CasinoGames.Set_PlayerCardPoker(list[0], 3);
								CasinoGames.PlayerCard_3_ID = list[0];
							}
						}
						if (num5 <= num7 && num5 >= num8)
						{
							CasinoGames.CardVarient cardRandomVarient4 = CasinoGames.GetCardRandomVarient();
							int randomCardPlayer_ReturnID3 = CasinoGames.GetRandomCardPlayer_ReturnID();
							int randomCardPlayer_ReturnID4 = CasinoGames.GetRandomCardPlayer_ReturnID();
							int randomCardPlayer_ReturnID5 = CasinoGames.GetRandomCardPlayer_ReturnID();
							List<string> list2 = new List<string>();
							string cardOfType8 = CasinoGames.GetCardOfType(cardRandomVarient4, (CasinoGames.CardID)randomCardPlayer_ReturnID3);
							list2.Add(cardOfType8);
							while (list2.Count < 3)
							{
								CasinoGames.drawText("Getting 'Flush' Cards " + list2.Count, 0.1f, 0.15f, 0.3f, 255, 255, 255);
								Script.Wait(1);
								if (list2.Count == 1)
								{
									if (randomCardPlayer_ReturnID4 == randomCardPlayer_ReturnID3)
									{
										randomCardPlayer_ReturnID4 = CasinoGames.GetRandomCardPlayer_ReturnID();
									}
									cardOfType8 = CasinoGames.GetCardOfType(cardRandomVarient4, (CasinoGames.CardID)randomCardPlayer_ReturnID4);
									if (!list2.Contains(cardOfType8))
									{
										list2.Add(cardOfType8);
									}
								}
								if (list2.Count == 2)
								{
									if (randomCardPlayer_ReturnID5 == randomCardPlayer_ReturnID3)
									{
										randomCardPlayer_ReturnID5 = CasinoGames.GetRandomCardPlayer_ReturnID();
									}
									if (randomCardPlayer_ReturnID5 == randomCardPlayer_ReturnID4)
									{
										randomCardPlayer_ReturnID5 = CasinoGames.GetRandomCardPlayer_ReturnID();
									}
									cardOfType8 = CasinoGames.GetCardOfType(cardRandomVarient4, (CasinoGames.CardID)randomCardPlayer_ReturnID5);
									if (!list2.Contains(cardOfType8))
									{
										list2.Add(cardOfType8);
									}
								}
							}
							CasinoGames.PlayerCard_1 = CasinoGames.Set_PlayerCardPoker(list2[0], 1);
							CasinoGames.PlayerCard_1_ID = list2[0];
							CasinoGames.PlayerCard_2 = CasinoGames.Set_PlayerCardPoker(list2[1], 2);
							CasinoGames.PlayerCard_2_ID = list2[1];
							CasinoGames.PlayerCard_3 = CasinoGames.Set_PlayerCardPoker(list2[2], 3);
							CasinoGames.PlayerCard_3_ID = list2[2];
						}
						if (num5 <= num8 && num5 >= num9)
						{
							CasinoGames.CardVarient cardRandomVarient5 = CasinoGames.GetCardRandomVarient();
							int randomCardPlayer_ReturnID_Max_Queen;
							int num12 = (randomCardPlayer_ReturnID_Max_Queen = CasinoGames.GetRandomCardPlayer_ReturnID_Max_Queen());
							int iD = num12 + 1;
							int iD2 = num12 + 2;
							List<string> list3 = new List<string>();
							string cardOfType9 = CasinoGames.GetCardOfType(cardRandomVarient5, (CasinoGames.CardID)randomCardPlayer_ReturnID_Max_Queen);
							list3.Add(cardOfType9);
							while (list3.Count < 3)
							{
								Script.Wait(1);
								if (list3.Count == 1)
								{
									cardOfType9 = CasinoGames.GetCardOfType(CasinoGames.GetCardRandomVarient(), (CasinoGames.CardID)iD);
									if (!list3.Contains(cardOfType9))
									{
										list3.Add(cardOfType9);
									}
								}
								if (list3.Count == 2)
								{
									cardOfType9 = CasinoGames.GetCardOfType(CasinoGames.GetCardRandomVarient(), (CasinoGames.CardID)iD2);
									if (!list3.Contains(cardOfType9))
									{
										list3.Add(cardOfType9);
									}
								}
							}
							CasinoGames.PlayerCard_1 = CasinoGames.Set_PlayerCardPoker(list3[0], 1);
							CasinoGames.PlayerCard_1_ID = list3[0];
							CasinoGames.PlayerCard_2 = CasinoGames.Set_PlayerCardPoker(list3[1], 2);
							CasinoGames.PlayerCard_2_ID = list3[1];
							CasinoGames.PlayerCard_3 = CasinoGames.Set_PlayerCardPoker(list3[2], 3);
							CasinoGames.PlayerCard_3_ID = list3[2];
						}
						if (num5 <= num9 && num5 >= num10)
						{
							int randomCardPlayer_ReturnID6 = CasinoGames.GetRandomCardPlayer_ReturnID();
							CasinoGames.CardVarient cardRandomVarient6 = CasinoGames.GetCardRandomVarient();
							List<string> list4 = new List<string>();
							string cardOfType10 = CasinoGames.GetCardOfType(cardRandomVarient6, (CasinoGames.CardID)randomCardPlayer_ReturnID6);
							list4.Add(cardOfType10);
							while (list4.Count < 3)
							{
								Script.Wait(1);
								if (list4.Count == 1)
								{
									cardOfType10 = CasinoGames.GetCardOfType(CasinoGames.GetCardRandomVarient(), (CasinoGames.CardID)randomCardPlayer_ReturnID6);
									if (!list4.Contains(cardOfType10))
									{
										list4.Add(cardOfType10);
									}
								}
								if (list4.Count == 2)
								{
									cardOfType10 = CasinoGames.GetCardOfType(CasinoGames.GetCardRandomVarient(), (CasinoGames.CardID)randomCardPlayer_ReturnID6);
									if (!list4.Contains(cardOfType10))
									{
										list4.Add(cardOfType10);
									}
								}
							}
							CasinoGames.PlayerCard_1 = CasinoGames.Set_PlayerCardPoker(list4[0], 1);
							CasinoGames.PlayerCard_1_ID = list4[0];
							CasinoGames.PlayerCard_2 = CasinoGames.Set_PlayerCardPoker(list4[1], 2);
							CasinoGames.PlayerCard_2_ID = list4[1];
							CasinoGames.PlayerCard_3 = CasinoGames.Set_PlayerCardPoker(list4[2], 3);
							CasinoGames.PlayerCard_3_ID = list4[2];
						}
						if (num5 <= num10)
						{
							CasinoGames.CardVarient cardRandomVarient7 = CasinoGames.GetCardRandomVarient();
							int randomCardPlayer_ReturnID_Max_Queen2;
							int num13 = (randomCardPlayer_ReturnID_Max_Queen2 = CasinoGames.GetRandomCardPlayer_ReturnID_Max_Queen());
							int iD3 = num13 + 1;
							int iD4 = num13 + 2;
							List<string> list5 = new List<string>();
							string cardOfType11 = CasinoGames.GetCardOfType(cardRandomVarient7, (CasinoGames.CardID)randomCardPlayer_ReturnID_Max_Queen2);
							list5.Add(cardOfType11);
							while (list5.Count < 3)
							{
								Script.Wait(1);
								if (list5.Count == 1)
								{
									cardOfType11 = CasinoGames.GetCardOfType(cardRandomVarient7, (CasinoGames.CardID)iD3);
									if (!list5.Contains(cardOfType11))
									{
										list5.Add(cardOfType11);
									}
								}
								if (list5.Count == 2)
								{
									cardOfType11 = CasinoGames.GetCardOfType(cardRandomVarient7, (CasinoGames.CardID)iD4);
									if (!list5.Contains(cardOfType11))
									{
										list5.Add(cardOfType11);
									}
								}
							}
							CasinoGames.PlayerCard_1 = CasinoGames.Set_PlayerCardPoker(list5[0], 1);
							CasinoGames.PlayerCard_1_ID = list5[0];
							CasinoGames.PlayerCard_2 = CasinoGames.Set_PlayerCardPoker(list5[1], 2);
							CasinoGames.PlayerCard_2_ID = list5[1];
							CasinoGames.PlayerCard_3 = CasinoGames.Set_PlayerCardPoker(list5[2], 3);
							CasinoGames.PlayerCard_3_ID = list5[2];
						}
						Prop prop4 = GlobalHelper.CreateProp(CasinoGames.RequestModel(CasinoGames.PlayerCard_1_ID), offsetPosition5, true, false);
						prop4.IsPositionFrozen = true;
						CasinoGames.PlayerCardB = prop4;
						CasinoGames.PlayerCardB.IsVisible = false;
						Vector3 position3 = prop4.Position;
						prop4.Position = new Vector3(position3.X, position3.Y, position3.Z - CasinoGames.GetTableHeight_Cards(CasinoGames.CurrentTable));
						prop4.Heading = Game.Player.Character.Heading;
						CasinoGames.Chips.Add(prop4);
						Script.Wait(1);
						prop4 = GlobalHelper.CreateProp(CasinoGames.RequestModel(CasinoGames.PlayerCard_2_ID), offsetPosition6, true, false);
						prop4.IsPositionFrozen = true;
						CasinoGames.PlayerCardC = prop4;
						CasinoGames.PlayerCardC.IsVisible = false;
						position3 = prop4.Position;
						prop4.Position = new Vector3(position3.X, position3.Y, position3.Z - CasinoGames.GetTableHeight_Cards(CasinoGames.CurrentTable));
						prop4.Heading = Game.Player.Character.Heading;
						CasinoGames.Chips.Add(prop4);
						Script.Wait(1);
						prop4 = GlobalHelper.CreateProp(CasinoGames.RequestModel(CasinoGames.PlayerCard_3_ID), offsetPosition4, true, false);
						prop4.IsPositionFrozen = true;
						position3 = prop4.Position;
						CasinoGames.PlayerCardA = prop4;
						CasinoGames.PlayerCardA.IsVisible = false;
						prop4.Position = new Vector3(position3.X, position3.Y, position3.Z - CasinoGames.GetTableHeight_Cards(CasinoGames.CurrentTable));
						prop4.Heading = Game.Player.Character.Heading;
						CasinoGames.Chips.Add(prop4);
					}
					else
					{
						string randomCardPlayer = CasinoGames.GetRandomCardPlayer();
						CasinoGames.PlayerCard_2 = CasinoGames.Set_PlayerCardPoker(randomCardPlayer, 2);
						CasinoGames.PlayerCard_2_ID = randomCardPlayer;
						Prop prop5 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer), offsetPosition5, true, false);
						prop5.IsPositionFrozen = true;
						CasinoGames.PlayerCardB = prop5;
						CasinoGames.PlayerCardB.IsVisible = false;
						Vector3 position4 = prop5.Position;
						prop5.Position = new Vector3(position4.X, position4.Y, position4.Z - CasinoGames.GetTableHeight_Cards(CasinoGames.CurrentTable));
						prop5.Heading = Game.Player.Character.Heading;
						CasinoGames.Chips.Add(prop5);
						Script.Wait(1);
						string randomCardPlayer2 = CasinoGames.GetRandomCardPlayer();
						CasinoGames.PlayerCard_3 = CasinoGames.Set_PlayerCardPoker(randomCardPlayer2, 3);
						CasinoGames.PlayerCard_3_ID = randomCardPlayer2;
						prop5 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer2), offsetPosition6, true, false);
						prop5.IsPositionFrozen = true;
						CasinoGames.PlayerCardC = prop5;
						CasinoGames.PlayerCardC.IsVisible = false;
						position4 = prop5.Position;
						prop5.Position = new Vector3(position4.X, position4.Y, position4.Z - CasinoGames.GetTableHeight_Cards(CasinoGames.CurrentTable));
						prop5.Heading = Game.Player.Character.Heading;
						CasinoGames.Chips.Add(prop5);
						Script.Wait(1);
						string randomCardPlayer3 = CasinoGames.GetRandomCardPlayer();
						CasinoGames.PlayerCard_1 = CasinoGames.Set_PlayerCardPoker(randomCardPlayer3, 1);
						CasinoGames.PlayerCard_1_ID = randomCardPlayer3;
						prop5 = GlobalHelper.CreateProp(CasinoGames.RequestModel(randomCardPlayer3), offsetPosition4, true, false);
						prop5.IsPositionFrozen = true;
						position4 = prop5.Position;
						CasinoGames.PlayerCardA = prop5;
						CasinoGames.PlayerCardA.IsVisible = false;
						prop5.Position = new Vector3(position4.X, position4.Y, position4.Z - CasinoGames.GetTableHeight_Cards(CasinoGames.CurrentTable));
						prop5.Heading = Game.Player.Character.Heading;
						CasinoGames.Chips.Add(prop5);
					}
					CasinoGames.Stage = 4f;
				}
				if (CasinoGames.Stage == 4f)
				{
					string dict2 = "anim_casino_b@amb@casino@games@threecardpoker@dealer";
					string text2 = CasinoGames.IsDealerFemale() + "deck_deal_self";
					CasinoGames.LoadDict(dict2);
					Vector3 dealerAnimOrigin = CasinoGames.GetDealerAnimOrigin();
					CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin.X, dealerAnimOrigin.Y, dealerAnimOrigin.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict2), text2, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text2, CasinoGames.LoadDict(dict2), 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealerTable.Card1, CasinoGames.Scene2, "deck_deal_self_card_a", CasinoGames.LoadDict(dict2), 16f, -16f, 0, 1148846080);
					CasinoGames._8PlayerGamePokerPot += 1000f;
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealerTable.Card2, CasinoGames.Scene2, "deck_deal_self_card_b", CasinoGames.LoadDict(dict2), 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealerTable.Card3, CasinoGames.Scene2, "deck_deal_self_card_c", CasinoGames.LoadDict(dict2), 16f, -16f, 0, 1148846080);
					CasinoGames.Stage = 5f;
				}
				if (CasinoGames.Stage == 5f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216)
				{
					CasinoGames.PlayerCardA.IsVisible = true;
					CasinoGames.PlayerCardB.IsVisible = true;
					CasinoGames.PlayerCardC.IsVisible = true;
					string dict3 = "anim_casino_b@amb@casino@games@threecardpoker@dealer";
					string text3 = CasinoGames.IsDealerFemale() + "deck_deal_p0" + CasinoGames.SeatSittingIn;
					CasinoGames.LoadDict(dict3);
					Vector3 dealerAnimOrigin2 = CasinoGames.GetDealerAnimOrigin();
					CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin2.X, dealerAnimOrigin2.Y, dealerAnimOrigin2.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict3), text3, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text3, CasinoGames.LoadDict(dict3), 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCardA, CasinoGames.Scene2, "deck_deal_p0" + CasinoGames.SeatSittingIn + "_card_a", CasinoGames.LoadDict(dict3), 16f, -16f, 0, 1148846080);
					CasinoGames._8PlayerGamePokerPot += 1000f;
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCardB, CasinoGames.Scene2, "deck_deal_p0" + CasinoGames.SeatSittingIn + "_card_b", CasinoGames.LoadDict(dict3), 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCardC, CasinoGames.Scene2, "deck_deal_p0" + CasinoGames.SeatSittingIn + "_card_c", CasinoGames.LoadDict(dict3), 16f, -16f, 0, 1148846080);
					CasinoGames.Stage = 6f;
				}
				if (CasinoGames.Stage == 6f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216)
				{
					string dict4 = "anim_casino_b@amb@casino@games@threecardpoker@player";
					string text4 = "cards_pickup";
					CasinoGames.LoadDict(dict4);
					CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict(dict4), text4, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, text4, CasinoGames.LoadDict(dict4), 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCardA, CasinoGames.Scene2, "cards_pickup_card_a", CasinoGames.LoadDict(dict4), 16f, -16f, 0, 1148846080);
					CasinoGames._8PlayerGamePokerPot += 1000f;
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCardB, CasinoGames.Scene2, "cards_pickup_card_b", CasinoGames.LoadDict(dict4), 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCardC, CasinoGames.Scene2, "cards_pickup_card_c", CasinoGames.LoadDict(dict4), 16f, -16f, 0, 1148846080);
					CasinoGames.Stage = 7.5f;
					bool hasFlush = CasinoGames.GetHasFlush();
					bool hasThreeOfAkind = CasinoGames.GetHasThreeOfAkind();
					bool hasPair = CasinoGames.GetHasPair(hasThreeOfAkind);
					bool hasStraightFlush = CasinoGames.GetHasStraightFlush();
					bool hasStraight = CasinoGames.GetHasStraight(hasStraightFlush);
					if (hasFlush)
					{
						CasinoGames.ShowHelpMessage("You have a Flush.");
					}
					if (hasPair)
					{
						CasinoGames.ShowHelpMessage("You have a Pair.");
					}
					if (hasThreeOfAkind)
					{
						CasinoGames.ShowHelpMessage("You have a Three of a Kind.");
					}
					if (hasStraightFlush)
					{
						CasinoGames.ShowHelpMessage("You have a Straight Flush.");
					}
					if (hasStraight)
					{
						CasinoGames.ShowHelpMessage("You have a Straight.");
					}
				}
				if (CasinoGames.Stage == 7f)
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216)
					{
						CasinoGames.Drawcards = true;
						CasinoGames.Endgame = false;
						CasinoGames.Editbet = true;
						CasinoGames.Playercards = 5;
					}
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216)
					{
						string dict5 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@idles";
						Random random = new Random();
						List<string> list6 = new List<string> { "idle_a", "idle_b", "idle_c", "idle_d" };
						string text5 = list6[random.Next(0, list6.Count)];
						CasinoGames.LoadDict(dict5);
						CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading - 90f, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict(dict5), text5, 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, text5, CasinoGames.LoadDict(dict5), 16f, -16f, 0, 1148846080);
						string dict6 = "anim_casino_b@amb@casino@games@blackjack@dealer";
						string text6 = "idle";
						Vector3 dealerAnimOrigin3 = CasinoGames.GetDealerAnimOrigin();
						CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin3.X, dealerAnimOrigin3.Y, dealerAnimOrigin3.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene1, CasinoGames.LoadDict(dict6), text6, 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene1, text6, CasinoGames.LoadDict(dict6), 16f, -16f, 0, 1148846080);
					}
				}
				if (CasinoGames.Stage == 7.5f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216)
				{
					string dict7 = "anim_casino_b@amb@casino@games@threecardpoker@player";
					string text7 = "cards_play";
					CasinoGames.LoadDict(dict7);
					CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, CasinoGames.Scene2, 2f);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict(dict7), text7, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, text7, CasinoGames.LoadDict(dict7), 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCardA, CasinoGames.Scene2, "cards_play_card_a", CasinoGames.LoadDict(dict7), 16f, -16f, 0, 1148846080);
					CasinoGames._8PlayerGamePokerPot += 1000f;
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCardB, CasinoGames.Scene2, "cards_play_card_b", CasinoGames.LoadDict(dict7), 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCardC, CasinoGames.Scene2, "cards_play_card_c", CasinoGames.LoadDict(dict7), 16f, -16f, 0, 1148846080);
					dict7 = "anim_casino_b@amb@casino@games@threecardpoker@dealer";
					text7 = CasinoGames.IsDealerFemale() + "deck_put_down";
					Vector3 dealerAnimOrigin4 = CasinoGames.GetDealerAnimOrigin();
					CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin4.X, dealerAnimOrigin4.Y, dealerAnimOrigin4.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene1, CasinoGames.LoadDict(dict7), text7, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene1, text7, CasinoGames.LoadDict(dict7), 16f, -16f, 0, 1148846080);
					Script.Wait(1);
					CasinoGames.Stage = 7f;
				}
				if (CasinoGames.Stage == 8f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216)
				{
					string dict8 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@idles";
					Random random2 = new Random();
					List<string> list7 = new List<string> { "idle_a", "idle_b", "idle_c", "idle_d" };
					string text8 = list7[random2.Next(0, list7.Count)];
					CasinoGames.LoadDict(dict8);
					CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict(dict8), text8, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, text8, CasinoGames.LoadDict(dict8), 16f, -16f, 0, 1148846080);
					string dict9 = "anim_casino_b@amb@casino@games@blackjack@dealer";
					string text9 = "idle";
					Vector3 dealerAnimOrigin5 = CasinoGames.GetDealerAnimOrigin();
					CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin5.X, dealerAnimOrigin5.Y, dealerAnimOrigin5.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene1, CasinoGames.LoadDict(dict9), text9, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene1, text9, CasinoGames.LoadDict(dict9), 16f, -16f, 0, 1148846080);
					Script.Wait(1);
					CasinoGames.Stage = 7f;
				}
				if (CasinoGames.Stage == 11f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene1) >= 1065353216)
				{
					CasinoGames.Endgame = true;
					CasinoGames.CardA.Rotation = new Vector3(CasinoGames.CardA.Rotation.X, 180f, CasinoGames.CardA.Rotation.Z);
					CasinoGames.CardB.Rotation = new Vector3(CasinoGames.CardB.Rotation.X, 180f, CasinoGames.CardB.Rotation.Z);
					CasinoGames.CardC.Rotation = new Vector3(CasinoGames.CardC.Rotation.X, 180f, CasinoGames.CardC.Rotation.Z);
					string dict10 = "anim_casino_b@amb@casino@games@blackjack@ped_male@engaged@01a@play@v02";
					string text10 = "place_bet";
					CasinoGames.LoadDict(dict10);
					CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading - 90f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene1, CasinoGames.LoadDict(dict10), text10, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene1, text10, CasinoGames.LoadDict(dict10), 16f, -16f, 0, 1148846080);
					string dict11 = "anim_casino_b@amb@casino@games@threecardpoker@dealer";
					string text11 = CasinoGames.IsDealerFemale() + "reveal_self";
					Vector3 dealerAnimOrigin6 = CasinoGames.GetDealerAnimOrigin();
					CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin6.X, dealerAnimOrigin6.Y, dealerAnimOrigin6.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict11), text11, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text11, CasinoGames.LoadDict(dict11), 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealerTable.Card1, CasinoGames.Scene2, "reveal_self_card_a", CasinoGames.LoadDict(dict11), 16f, -16f, 0, 1148846080);
					CasinoGames._8PlayerGamePokerPot += 1000f;
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealerTable.Card2, CasinoGames.Scene2, "reveal_self_card_b", CasinoGames.LoadDict(dict11), 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealerTable.Card3, CasinoGames.Scene2, "reveal_self_card_c", CasinoGames.LoadDict(dict11), 16f, -16f, 0, 1148846080);
					Script.Wait(1);
					CasinoGames.Stage = 14f;
				}
				if (CasinoGames.Stage == 12f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216)
				{
					CasinoGames.Stage = 0f;
					string dict12 = "anim_casino_b@amb@casino@games@threecardpoker@dealer";
					string text12 = CasinoGames.IsDealerFemale() + "reveal_self";
					Vector3 dealerAnimOrigin7 = CasinoGames.GetDealerAnimOrigin();
					CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin7.X, dealerAnimOrigin7.Y, dealerAnimOrigin7.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict12), text12, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text12, CasinoGames.LoadDict(dict12), 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealerTable.Card1, CasinoGames.Scene2, "reveal_self_card_a", CasinoGames.LoadDict(dict12), 16f, -16f, 0, 1148846080);
					CasinoGames._8PlayerGamePokerPot += 1000f;
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealerTable.Card2, CasinoGames.Scene2, "reveal_self_card_b", CasinoGames.LoadDict(dict12), 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealerTable.Card3, CasinoGames.Scene2, "reveal_self_card_c", CasinoGames.LoadDict(dict12), 16f, -16f, 0, 1148846080);
					CasinoGames.PlayerCardA.Rotation = new Vector3(CasinoGames.PlayerCardA.Rotation.X, 180f, CasinoGames.PlayerCardA.Rotation.Z);
					Script.Wait(200);
					CasinoGames.PlayerCardB.Rotation = new Vector3(CasinoGames.PlayerCardB.Rotation.X, 180f, CasinoGames.PlayerCardB.Rotation.Z);
					Script.Wait(200);
					CasinoGames.PlayerCardC.Rotation = new Vector3(CasinoGames.PlayerCardC.Rotation.X, 180f, CasinoGames.PlayerCardC.Rotation.Z);
					Script.Wait(1);
					CasinoGames.Stage = 14f;
				}
				if (CasinoGames.Stage == 13f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216)
				{
					string dict13 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@engaged@01b@reacts@v01";
					string text13 = "collect_chips_card_c";
					CasinoGames.LoadDict(dict13);
					CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z, 0.0, 0.0, CasinoGames.PlayerOriginHeading, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict(dict13), text13, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, text13, CasinoGames.LoadDict(dict13), 16f, -16f, 0, 1148846080);
					CasinoGames.CardB.Rotation = new Vector3(CasinoGames.CardB.Rotation.X, 180f, CasinoGames.CardB.Rotation.Z);
					Script.Wait(200);
					CasinoGames.CardC.Rotation = new Vector3(CasinoGames.CardC.Rotation.X, 180f, CasinoGames.CardC.Rotation.Z);
					Script.Wait(200);
					CasinoGames.CardA.Rotation = new Vector3(CasinoGames.CardA.Rotation.X, 180f, CasinoGames.CardA.Rotation.Z);
					string dict14 = "anim_casino_b@amb@casino@games@blackjack@dealer";
					string text14 = "idle";
					Vector3 dealerAnimOrigin8 = CasinoGames.GetDealerAnimOrigin();
					CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin8.X, dealerAnimOrigin8.Y, dealerAnimOrigin8.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene1, CasinoGames.LoadDict(dict14), text14, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene1, text14, CasinoGames.LoadDict(dict14), 16f, -16f, 0, 1148846080);
					Script.Wait(1);
					CasinoGames.Stage = 14f;
				}
				if (CasinoGames.Stage == 14f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216)
				{
					CasinoGames.CalculatePoker();
					Script.Wait(1);
					CasinoGames.Stage = 50f;
				}
				if (CasinoGames.Stage == 15f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216)
				{
					string dict15 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@idles";
					Random random3 = new Random();
					List<string> list8 = new List<string> { "idle_a", "idle_b", "idle_c", "idle_d" };
					string text15 = list8[random3.Next(0, list8.Count)];
					CasinoGames.LoadDict(dict15);
					CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict(dict15), text15, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, text15, CasinoGames.LoadDict(dict15), 16f, -16f, 0, 1148846080);
					string dict16 = "anim_casino_b@amb@casino@games@blackjack@dealer";
					string text16 = "idle";
					Vector3 dealerAnimOrigin9 = CasinoGames.GetDealerAnimOrigin();
					CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin9.X, dealerAnimOrigin9.Y, dealerAnimOrigin9.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene1, CasinoGames.LoadDict(dict16), text16, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene1, text16, CasinoGames.LoadDict(dict16), 16f, -16f, 0, 1148846080);
				}
				if (CasinoGames.Stage == 20f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216)
				{
					string dict17 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@idles";
					Random random4 = new Random();
					List<string> list9 = new List<string> { "idle_a", "idle_b", "idle_c", "idle_d" };
					string text17 = list9[random4.Next(0, list9.Count)];
					CasinoGames.LoadDict(dict17);
					CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict(dict17), text17, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, text17, CasinoGames.LoadDict(dict17), 16f, -16f, 0, 1148846080);
					string dict18 = "anim_casino_b@amb@casino@games@blackjack@dealer";
					string text18 = "idle";
					Vector3 dealerAnimOrigin10 = CasinoGames.GetDealerAnimOrigin();
					CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin10.X, dealerAnimOrigin10.Y, dealerAnimOrigin10.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene1, CasinoGames.LoadDict(dict18), text18, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene1, text18, CasinoGames.LoadDict(dict18), 16f, -16f, 0, 1148846080);
					CasinoGames.Stage = 7f;
				}
				if (CasinoGames.Stage == 30f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216)
				{
					string dict19 = "anim_casino_b@amb@casino@games@threecardpoker@player";
					string text19 = "cards_play";
					CasinoGames.LoadDict(dict19);
					CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, CasinoGames.Scene1, 2f);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene1, CasinoGames.LoadDict(dict19), text19, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene1, text19, CasinoGames.LoadDict(dict19), 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCardA, CasinoGames.Scene1, "cards_play_card_a", CasinoGames.LoadDict(dict19), 16f, -16f, 0, 1148846080);
					CasinoGames._8PlayerGamePokerPot += 1000f;
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCardB, CasinoGames.Scene1, "cards_play_card_b", CasinoGames.LoadDict(dict19), 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCardC, CasinoGames.Scene1, "cards_play_card_c", CasinoGames.LoadDict(dict19), 16f, -16f, 0, 1148846080);
					Script.Wait(1);
					CasinoGames.Stage = 11f;
				}
				if (CasinoGames.Stage == 40f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216)
				{
					string dict20 = "anim_casino_b@amb@casino@games@threecardpoker@player";
					string text20 = "cards_fold";
					CasinoGames.LoadDict(dict20);
					CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict(dict20), text20, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, text20, CasinoGames.LoadDict(dict20), 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCardA, CasinoGames.Scene2, "cards_fold_card_a", CasinoGames.LoadDict(dict20), 16f, -16f, 0, 1148846080);
					CasinoGames._8PlayerGamePokerPot += 1000f;
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCardB, CasinoGames.Scene2, "cards_fold_card_a", CasinoGames.LoadDict(dict20), 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCardC, CasinoGames.Scene2, "cards_fold_card_a", CasinoGames.LoadDict(dict20), 16f, -16f, 0, 1148846080);
					Script.Wait(1);
					CasinoGames.Stage = 12f;
					Notification.Show("Fold ");
				}
				if (CasinoGames.Stage == 50f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216)
				{
					string dict21 = "anim_casino_b@amb@casino@games@blackjack@dealer";
					string text21 = "retrieve_cards_player_0" + CasinoGames.SeatSittingIn;
					Vector3 dealerAnimOrigin11 = CasinoGames.GetDealerAnimOrigin();
					CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin11.X, dealerAnimOrigin11.Y, dealerAnimOrigin11.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict21), text21, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text21, CasinoGames.LoadDict(dict21), 16f, -16f, 0, 1148846080);
					Script.Wait(1);
					CasinoGames.Stage = 51f;
				}
				if (CasinoGames.Stage == 51f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216)
				{
					CasinoGames.Stage = 52f;
				}
				if ((CasinoGames.Stage == 51f || CasinoGames.Stage == 52f) && (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216 || CasinoGames.Stage == 52f))
				{
					CasinoGames.Stage = 51f;
					string dict22 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@idles";
					Random random5 = new Random();
					List<string> list10 = new List<string> { "idle_a", "idle_b", "idle_c", "idle_d" };
					string text22 = list10[random5.Next(0, list10.Count)];
					CasinoGames.LoadDict(dict22);
					CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading - 90f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict(dict22), text22, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, text22, CasinoGames.LoadDict(dict22), 16f, -16f, 0, 1148846080);
					string dict23 = "anim_casino_b@amb@casino@games@blackjack@dealer";
					string text23 = "idle";
					Vector3 dealerAnimOrigin12 = CasinoGames.GetDealerAnimOrigin();
					CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin12.X, dealerAnimOrigin12.Y, dealerAnimOrigin12.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene1, CasinoGames.LoadDict(dict23), text23, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene1, text23, CasinoGames.LoadDict(dict23), 16f, -16f, 0, 1148846080);
				}
				if (CasinoGames.Stage == 53f)
				{
					string dict24 = "anim_casino_b@amb@casino@games@threecardpoker@dealer";
					string text24 = CasinoGames.IsDealerFemale() + "deck_pick_up";
					Vector3 dealerAnimOrigin13 = CasinoGames.GetDealerAnimOrigin();
					CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin13.X, dealerAnimOrigin13.Y, dealerAnimOrigin13.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict24), text24, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text24, CasinoGames.LoadDict(dict24), 16f, -16f, 0, 1148846080);
					Script.Wait(1);
					CasinoGames.Stage = 54f;
				}
				if (CasinoGames.Stage == 54f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216)
				{
					string dict25 = "anim_casino_b@amb@casino@games@threecardpoker@dealer";
					string text25 = CasinoGames.IsDealerFemale() + "deck_idle";
					Vector3 dealerAnimOrigin14 = CasinoGames.GetDealerAnimOrigin();
					CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin14.X, dealerAnimOrigin14.Y, dealerAnimOrigin14.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict25), text25, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text25, CasinoGames.LoadDict(dict25), 16f, -16f, 0, 1148846080);
					Script.Wait(1);
					CasinoGames.Stage = 55f;
				}
				if (CasinoGames.Stage == 55f)
				{
					CasinoGames.Editbet = false;
					CasinoGames.Endgame = true;
					CasinoGames.Placebet = false;
					CasinoGames.Placebet2 = false;
					CasinoGames.BetType = 0;
					CasinoGames.Stage = 0f;
					CasinoGames.Playercards = 0;
					foreach (DealerTable pokerDealerTable3 in CasinoGames.PokerDealerTables)
					{
						if (pokerDealerTable3.Dealer != null)
						{
							CasinoGames.ResetDealerPos(pokerDealerTable3);
						}
						if (pokerDealerTable3.Card1 != null)
						{
							pokerDealerTable3.Card1.Delete();
						}
						if (pokerDealerTable3.Card2 != null)
						{
							pokerDealerTable3.Card2.Delete();
						}
						if (pokerDealerTable3.Card3 != null)
						{
							pokerDealerTable3.Card3.Delete();
						}
					}
					foreach (DealerTable blackjackDealerTable3 in CasinoGames.BlackjackDealerTables)
					{
						if (blackjackDealerTable3.Dealer != null)
						{
							CasinoGames.ResetDealerPos(blackjackDealerTable3);
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
					}
					foreach (DealerTable rouletteDealerTable3 in CasinoGames.RouletteDealerTables)
					{
						if (rouletteDealerTable3.Dealer != null)
						{
							CasinoGames.ResetDealerPos(rouletteDealerTable3);
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
					}
					foreach (DealerTable pokerDealerTable4 in CasinoGames.PokerDealerTables)
					{
						if (pokerDealerTable4.Dealer != null)
						{
							CasinoGames.ResetDealerPos(pokerDealerTable4);
						}
						if (pokerDealerTable4.Card1 != null)
						{
							pokerDealerTable4.Card1.Delete();
						}
						if (pokerDealerTable4.Card2 != null)
						{
							pokerDealerTable4.Card2.Delete();
						}
						if (pokerDealerTable4.Card3 != null)
						{
							pokerDealerTable4.Card3.Delete();
						}
					}
					foreach (DealerTable blackjackDealerTable4 in CasinoGames.BlackjackDealerTables)
					{
						if (blackjackDealerTable4.Dealer != null)
						{
							CasinoGames.ResetDealerPos(blackjackDealerTable4);
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
					}
					foreach (DealerTable rouletteDealerTable4 in CasinoGames.RouletteDealerTables)
					{
						if (rouletteDealerTable4.Dealer != null)
						{
							CasinoGames.ResetDealerPos(rouletteDealerTable4);
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
					}
					foreach (Prop chip2 in CasinoGames.Chips)
					{
						if (chip2 != null)
						{
							chip2.Delete();
						}
					}
					CasinoGames.Placebet2 = false;
					CasinoGames.Editbet = false;
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
					CasinoGames.Bet = 0f;
					CasinoGames.DealerCard_1 = 0;
					CasinoGames.DealerCard_2 = 0;
					CasinoGames.DealerCard_3 = 0;
					CasinoGames.DealerCard_4 = 0;
					CasinoGames.DealerCard_5 = 0;
					CasinoGames.DealerCard_6 = 0;
					CasinoGames.ResetCardDeck();
				}
				if (CasinoGames.Editbet && CasinoGames.Stage == 7f)
				{
					if (Game.IsControlJustPressed(Control.Context))
					{
						CasinoGames.DisplayHelpTextThisFrame("You folded the hand.");
						string dict26 = "anim_casino_b@amb@casino@games@threecardpoker@player";
						string text26 = "cards_pickup";
						CasinoGames.LoadDict(dict26);
						CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict(dict26), text26, 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, text26, CasinoGames.LoadDict(dict26), 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCardA, CasinoGames.Scene2, "cards_pickup_card_a", CasinoGames.LoadDict(dict26), 16f, -16f, 0, 1148846080);
						CasinoGames._8PlayerGamePokerPot += 1000f;
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCardB, CasinoGames.Scene2, "cards_pickup_card_b", CasinoGames.LoadDict(dict26), 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCardC, CasinoGames.Scene2, "cards_pickup_card_c", CasinoGames.LoadDict(dict26), 16f, -16f, 0, 1148846080);
						Script.Wait(1);
						CasinoGames.Stage = 40f;
					}
					if (Game.IsControlJustPressed(Control.ThrowGrenade))
					{
						string dict27 = "anim_casino_b@amb@casino@games@threecardpoker@player";
						string text27 = "cards_pickup";
						CasinoGames.LoadDict(dict27);
						CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict(dict27), text27, 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, text27, CasinoGames.LoadDict(dict27), 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCardA, CasinoGames.Scene2, "cards_pickup_card_a", CasinoGames.LoadDict(dict27), 16f, -16f, 0, 1148846080);
						CasinoGames._8PlayerGamePokerPot += 1000f;
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCardB, CasinoGames.Scene2, "cards_pickup_card_b", CasinoGames.LoadDict(dict27), 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCardC, CasinoGames.Scene2, "cards_pickup_card_c", CasinoGames.LoadDict(dict27), 16f, -16f, 0, 1148846080);
						CasinoGames.Stage = 7.5f;
					}
					if (Game.IsControlJustPressed(Control.Cover))
					{
						if (CasinoGames.Playercards == 5)
						{
							string dict28 = "anim_casino_b@amb@casino@games@threecardpoker@player";
							string text28 = "cards_pickup";
							CasinoGames.LoadDict(dict28);
							CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict(dict28), text28, 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, text28, CasinoGames.LoadDict(dict28), 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCardA, CasinoGames.Scene2, "cards_pickup_card_a", CasinoGames.LoadDict(dict28), 16f, -16f, 0, 1148846080);
							CasinoGames._8PlayerGamePokerPot += 1000f;
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCardB, CasinoGames.Scene2, "cards_pickup_card_b", CasinoGames.LoadDict(dict28), 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.PlayerCardC, CasinoGames.Scene2, "cards_pickup_card_c", CasinoGames.LoadDict(dict28), 16f, -16f, 0, 1148846080);
							CasinoGames.Stage = 30f;
							bool hasFlush2 = CasinoGames.GetHasFlush();
							bool hasThreeOfAkind2 = CasinoGames.GetHasThreeOfAkind();
							bool hasPair2 = CasinoGames.GetHasPair(hasThreeOfAkind2);
							bool hasStraightFlush2 = CasinoGames.GetHasStraightFlush();
							bool hasStraight2 = CasinoGames.GetHasStraight(hasStraightFlush2);
							if (hasFlush2)
							{
								CasinoGames.ShowHelpMessage("You have a Flush. If you win against the Dealer, a Flush pays out 4 to 1 on Pair-Plus bets.");
							}
							if (hasPair2)
							{
								CasinoGames.ShowHelpMessage("You have a Pair. If you win against the Dealer, a Pair pays out 1 to 1 on Pair-Plus bets.");
							}
							if (hasThreeOfAkind2)
							{
								CasinoGames.ShowHelpMessage("You have a Three of a Kind. If you win against the Dealer, a Three of a Kind pays out 30 to 1 on Pair-Plus bets and 4 to 1 on Ante bets.");
							}
							if (hasStraightFlush2)
							{
								CasinoGames.ShowHelpMessage("You have a Straight Flush. If you win against the Dealer, a Straight Flush pays out 40 to 1 on Pair-Plus bets and 5 to 1 on Ante bets.");
							}
							if (hasStraight2)
							{
								CasinoGames.ShowHelpMessage("You have a Straight. If you win against the Dealer, a Straight pays out 6 to 1 on Pair-Plus bets.");
							}
						}
						else
						{
							Notification.Show("You cannot Accept untill you have a hand of at least 3");
						}
					}
				}
			}
			if (CasinoGames.Placebet)
			{
				return;
			}
			if (CasinoGames.Stage == -2f)
			{
				string dict29 = "anim_casino_b@amb@casino@games@threecardpoker@dealer";
				string text29 = CasinoGames.IsDealerFemale() + "deck_pick_up";
				Vector3 dealerAnimOrigin15 = CasinoGames.GetDealerAnimOrigin();
				CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin15.X, dealerAnimOrigin15.Y, dealerAnimOrigin15.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict29), text29, 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text29, CasinoGames.LoadDict(dict29), 16f, -16f, 0, 1148846080);
				Script.Wait(1);
				CasinoGames.Stage = -1f;
			}
			if (CasinoGames.Stage == -1f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216)
			{
				string dict30 = "anim_casino_b@amb@casino@games@threecardpoker@dealer";
				string text30 = CasinoGames.IsDealerFemale() + "deck_idle";
				Vector3 dealerAnimOrigin16 = CasinoGames.GetDealerAnimOrigin();
				CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin16.X, dealerAnimOrigin16.Y, dealerAnimOrigin16.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict30), text30, 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text30, CasinoGames.LoadDict(dict30), 16f, -16f, 0, 1148846080);
			}
			if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene1) >= 1065353216)
			{
				string dict31 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@idles";
				Random random6 = new Random();
				List<string> list11 = new List<string> { "idle_a", "idle_b", "idle_c", "idle_d" };
				string text31 = list11[random6.Next(0, list11.Count)];
				CasinoGames.LoadDict(dict31);
				CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading - 90f, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene1, CasinoGames.LoadDict(dict31), text31, 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene1, text31, CasinoGames.LoadDict(dict31), 16f, -16f, 0, 1148846080);
			}
			if (!Game.IsControlJustPressed(Control.Cover))
			{
				return;
			}
			if (CasinoGames.Bet_Ante >= 0)
			{
				try
				{
					if (CasinoGames.Bet_Ante != 0)
					{
						Prop prop6 = GlobalHelper.CreateProp(CasinoGames.RequestModel(CasinoGames.GetBetChipModel(CasinoGames.Bet_Ante, true)), CasinoGames.ReturnChipPosition(CasinoGames.PokerDealerTables, CasinoGames.BetTypeId.AnteBet), true, false);
						prop6.IsPositionFrozen = true;
						CasinoGames.Chips.Add(prop6);
						prop6.Position = CasinoGames.GetClosestTable(Game.Player.Character.Position, CasinoGames.PokerDealerTables).GetOffsetPosition(CasinoGames.THREE_CARD_POKER_GET_PLAYER_BET_POSITION_OFFSET(CasinoGames.ReturnClosestBetAmtFromCustomBet(CasinoGames.Bet_Ante), CasinoGames.BetTypeID.Ante, CasinoGames.SeatSittingIn - 1, false));
						prop6.Rotation = CasinoGames.GetClosestTable(Game.Player.Character.Position, CasinoGames.PokerDealerTables).Rotation + CasinoGames.THREE_CARD_POKER_GET_PLAYER_BET_ROTATION_OFFSET(CasinoGames.ReturnClosestBetAmtFromCustomBet(CasinoGames.Bet_Ante), CasinoGames.BetTypeID.Ante, CasinoGames.SeatSittingIn - 1, false);
						EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(prop6, true, 255));
					}
					if (CasinoGames.Bet_Play != 0)
					{
						Prop prop7 = GlobalHelper.CreateProp(CasinoGames.RequestModel(CasinoGames.GetBetChipModel(CasinoGames.Bet_Play, true)), CasinoGames.ReturnChipPosition(CasinoGames.PokerDealerTables, CasinoGames.BetTypeId.PlayBet), true, false);
						prop7.IsPositionFrozen = true;
						prop7.Position = CasinoGames.GetClosestTable(Game.Player.Character.Position, CasinoGames.PokerDealerTables).GetOffsetPosition(CasinoGames.THREE_CARD_POKER_GET_PLAYER_BET_POSITION_OFFSET(CasinoGames.ReturnClosestBetAmtFromCustomBet(CasinoGames.Bet_Play), CasinoGames.BetTypeID.Play, CasinoGames.SeatSittingIn - 1, false, true));
						prop7.Rotation = CasinoGames.GetClosestTable(Game.Player.Character.Position, CasinoGames.PokerDealerTables).Rotation + CasinoGames.THREE_CARD_POKER_GET_PLAYER_BET_ROTATION_OFFSET(CasinoGames.ReturnClosestBetAmtFromCustomBet(CasinoGames.Bet_Play), CasinoGames.BetTypeID.Play, CasinoGames.SeatSittingIn - 1, false);
						CasinoGames.Chips.Add(prop7);
						EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(prop7, true, 255));
					}
					if (CasinoGames.Bet_PairPlus != 0)
					{
						Prop prop8 = GlobalHelper.CreateProp(CasinoGames.RequestModel(CasinoGames.GetBetChipModel(CasinoGames.Bet_PairPlus, true)), CasinoGames.ReturnChipPosition(CasinoGames.PokerDealerTables, CasinoGames.BetTypeId.PairPlusBet), true, false);
						prop8.IsPositionFrozen = true;
						prop8.Position = CasinoGames.GetClosestTable(Game.Player.Character.Position, CasinoGames.PokerDealerTables).GetOffsetPosition(CasinoGames.THREE_CARD_POKER_GET_PLAYER_BET_POSITION_OFFSET(CasinoGames.ReturnClosestBetAmtFromCustomBet(CasinoGames.Bet_PairPlus), CasinoGames.BetTypeID.PairPlus, CasinoGames.SeatSittingIn - 1, false));
						prop8.Rotation = CasinoGames.GetClosestTable(Game.Player.Character.Position, CasinoGames.PokerDealerTables).Rotation + CasinoGames.THREE_CARD_POKER_GET_PLAYER_BET_ROTATION_OFFSET(CasinoGames.ReturnClosestBetAmtFromCustomBet(CasinoGames.Bet_PairPlus), CasinoGames.BetTypeID.PairPlus, CasinoGames.SeatSittingIn - 1, false);
						CasinoGames.Chips.Add(prop8);
						EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(prop8, true, 255));
					}
					CasinoGames.ClosestDealer = CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.PokerDealerTables);
					CasinoGames.ClosestTable = CasinoGames.GetClosestTable(Game.Player.Character.Position, CasinoGames.PokerDealerTables);
					CasinoGames.ClosestDealerTable = CasinoGames.GetClosestDealerTable(Game.Player.Character.Position, CasinoGames.PokerDealerTables);
					CasinoGames.PlayerOriginPosition = CasinoGames.GetPlayerAnimOriginH(CasinoGames.SeatSittingIn, CasinoGames.TableType.Poker).Item1;
					CasinoGames.PlayerOriginHeading = CasinoGames.GetPlayerAnimOriginH(CasinoGames.SeatSittingIn, CasinoGames.TableType.Poker).Item2;
					string dict32 = "anim_casino_b@amb@casino@games@blackjack@dealer";
					string text32 = "place_bet_request";
					Vector3 dealerAnimOrigin17 = CasinoGames.GetDealerAnimOrigin();
					CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin17.X, dealerAnimOrigin17.Y, dealerAnimOrigin17.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene1, CasinoGames.LoadDict(dict32), text32, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene1, text32, CasinoGames.LoadDict(dict32), 16f, -16f, 0, 1148846080);
					dict32 = "anim_casino_b@amb@casino@games@threecardpoker@player";
					text32 = "bet_ante";
					CasinoGames.LoadDict(dict32);
					CasinoGames.GetPlayerAnimOriginH(CasinoGames.SeatSittingIn, CasinoGames.TableType.Blackjack);
					CasinoGames.GetPlayerAnimOriginH(CasinoGames.SeatSittingIn, CasinoGames.TableType.Blackjack);
					CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict(dict32), text32, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, text32, CasinoGames.LoadDict(dict32), 16f, -16f, 0, 1148846080);
					CasinoGames.Stage = 1f;
					CasinoGames.Placebet = true;
				}
				catch (Exception)
				{
					CasinoGames.ClosestDealer = CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.PokerDealerTables);
					CasinoGames.ClosestTable = CasinoGames.GetClosestTable(Game.Player.Character.Position, CasinoGames.PokerDealerTables);
					CasinoGames.ClosestDealerTable = CasinoGames.GetClosestDealerTable(Game.Player.Character.Position, CasinoGames.PokerDealerTables);
					Notification.Show("Player did not enter a valid bet");
				}
				Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.PokerDealerTables), "MINIGAME_DEALER_PLACE_CHIPS", "SPEECH_PARAMS_STANDARD", false);
			}
			else
			{
				Notification.Show("Player needs to place a bet on ANTE, PLAY or PAIR PLUS");
			}
		}
	}
}
