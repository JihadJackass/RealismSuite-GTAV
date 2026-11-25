using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace GTAO_DiamondCasino
{
	public class InitializeCasino : Script
	{
		public enum TableColor
		{
			Green,
			Red,
			Blue,
			Purple
		}

		public static Utils.MissionOjects TableObjects = new Utils.MissionOjects(new List<Utils.ObjectPlacementMap>
		{
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_3cardpoker_01b"), 310076, 618005505u, new Vector3(1133.74f, 266.694733f, -52.04094f), new Vector3(-0f, -0f, -44.9999352f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), 3),
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_roulette_01b"), 535847, 3021682919u, new Vector3(1129.883f, 267.433f, -52f), new Vector3(0f, -0f, -158.999985f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), 3),
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_blckjack_01b"), 294165, 2168288314u, new Vector3(1129.40649f, 262.357758f, -52.0410042f), new Vector3(0f, -0f, 135.309433f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), 3),
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_roulette_01b"), 27668, 3021682919u, new Vector3(1148.889f, 248.0665f, -52f), new Vector3(0f, 0f, 42.9999962f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), 3),
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_blckjack_01b"), 133979, 2168288314u, new Vector3(1144.42908f, 247.3352f, -52.0410042f), new Vector3(0f, -0f, 135.309433f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), 3),
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_3cardpoker_01b"), 596822, 618005505u, new Vector3(1148.74f, 251.694717f, -52.04094f), new Vector3(-0f, -0f, -44.9999352f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), 3),
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_roulette_01b"), 96788, 3021682919u, new Vector3(1147.584f, 265.6707f, -52.8f), new Vector3(0f, -0f, 132.999985f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), 0),
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_blckjack_01"), 479820, 112404821u, new Vector3(1148.83679f, 269.74704f, -52.8409462f), new Vector3(0f, -0f, -134.690659f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), 0),
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_blckjack_01"), 29764, 112404821u, new Vector3(1151.84f, 266.747f, -52.8409462f), new Vector3(0f, 0f, 45.30934f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), 0),
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_3cardpoker_01"), 12605, 2566890193u, new Vector3(1146.329f, 261.2543f, -52.8409424f), new Vector3(0f, 0f, 44.9999352f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), 0),
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_3cardpoker_01"), 112713, 2566890193u, new Vector3(1143.33789f, 264.2453f, -52.8409424f), new Vector3(0f, -0f, -134.999954f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), 0),
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_3cardpoker_01"), 689168, 2566890193u, new Vector3(921.0039f, 35.57f, 110.66f), new Vector3(0f, -0f, -122f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), 2),
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_blckjack_01"), 685584, 112404821u, new Vector3(919.0012f, 32.7f, 110.65f), new Vector3(0f, -0f, -120.999992f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), 2),
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_roulette_01b"), 678409, 3021682919u, new Vector3(912.65f, 57.7f, 110.65f), new Vector3(0f, -0f, -121.3f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), 2),
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_blckjack_01"), 35352, 112404821u, new Vector3(931.42f, 47.18f, 112.54f), new Vector3(0f, 0f, 58.4799957f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), 2),
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_3cardpoker_01"), 684560, 2566890193u, new Vector3(945.48f, 69.4f, 112.5f), new Vector3(0f, 0f, 57.6999931f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), 2),
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_blckjack_01"), 356126, 112404821u, new Vector3(968.682251f, 33.43925f, 115.181152f), new Vector3(0f, -0f, -122.167191f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), 2),
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_3cardpoker_01"), 673047, 2566890193u, new Vector3(966.750061f, 30.3664932f, 115.181152f), new Vector3(0f, -0f, -122.167191f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), 2)
		});

		public static bool RunMethodNow()
		{
			bool result = true;
			if (Class1.Initialized)
			{
				result = false;
			}
			return result;
		}

		public InitializeCasino()
		{
			if (_Debug.Initialize != -1)
			{
				base.Tick += OnTick;
			}
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

		public static void Setup_Roulette_DealerTable(DealerTable T)
		{
			if (!(T.Table != null) || !T.Table.Exists() || T.CreatedDealer)
			{
				return;
			}
			try
			{
				if (T.Dealer != null)
				{
					T.Dealer.Delete();
				}
				if (T.Table == null)
				{
					Notification.Show("TABLE IS NULL");
				}
				List<string> list = new List<string>();
				list.Add("s_m_y_casino_01");
				list.Add("s_f_y_casino_01");
				list.Add("s_m_y_casino_01");
				list.Add("s_f_y_casino_01");
				list.Add("s_m_y_casino_01");
				list.Add("s_f_y_casino_01");
				list.Add("s_m_y_casino_01");
				list.Add("s_f_y_casino_01");
				string text = list[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list.Count)];
				Ped ped = GlobalHelper.CreatePed(RequestModel(text), new Vector3(0f, 0f, 0f), 0f);
				if (T.Table == null)
				{
					Game.Player.Character.Position = T.Table.Position;
					Notification.Show("TABLE IS NULL (POKER) {" + text + "}");
				}
				ped.Position = T.Table.GetOffsetPosition(new Vector3(-1.5f, 0.4f, 0f));
				ped.Heading = T.Table.Heading - 90f;
				T.Dealer = ped;
				if (T.Dealer == null)
				{
					Game.Player.Character.Position = T.Table.Position;
					Notification.Show("DEALER IS NULL (ROULETTE) {" + text + "}");
				}
				T.Dealer.Position = CasinoGames.SetDealerPositionAtTable(T.Table, T.TableType).Item1;
				T.Dealer.Rotation = CasinoGames.SetDealerPositionAtTable(T.Table, T.TableType).Item2;
				T.Dealer.IsPositionFrozen = true;
				Class1.SetPedOutfit(T.Dealer);
				Class1.MainPed.Add(T.Dealer);
				CasinoGames.PlayDealerAnim(T.Dealer);
				T.Dealer.RelationshipGroup = 2;
				T.Dealer.AlwaysKeepTask = true;
				T.Dealer.BlockPermanentEvents = true;
				T.DealerOriginHeading = T.Dealer.Heading;
				T.DealerOriginPosition = new Vector3(T.Dealer.Position.X, T.Dealer.Position.Y, T.Dealer.Position.Z - 1f);
				T.CreatedDealer = true;
			}
			catch
			{
			}
		}

		public static void Setup_Blackjack_DealerTable(DealerTable T)
		{
			if (!(T.Table != null))
			{
				return;
			}
			if (T.Table.Position.DistanceTo(new Vector3(1148.837f, 269.747f, -52.84095f)) < 5f)
			{
				Function.Call(Hash.SET_OBJECT_TINT_INDEX, T.Table, CasinoGames.NormalTableColor);
			}
			if (T.Table.Position.DistanceTo(new Vector3(1151.84f, 266.747f, -52.84095f)) < 5f)
			{
				Function.Call(Hash.SET_OBJECT_TINT_INDEX, T.Table, CasinoGames.NormalTableColor);
			}
			if (!T.Table.Exists() || T.CreatedDealer)
			{
				return;
			}
			try
			{
				if (T.Dealer != null)
				{
					T.Dealer.Delete();
				}
				if (T.Table == null)
				{
					Notification.Show("TABLE IS NULL");
				}
				List<string> list = new List<string>();
				list.Add("s_m_y_casino_01");
				list.Add("s_f_y_casino_01");
				list.Add("s_m_y_casino_01");
				list.Add("s_f_y_casino_01");
				list.Add("s_m_y_casino_01");
				list.Add("s_f_y_casino_01");
				list.Add("s_m_y_casino_01");
				list.Add("s_f_y_casino_01");
				string text = list[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list.Count)];
				Ped ped = GlobalHelper.CreatePed(RequestModel(text), new Vector3(0f, 0f, 0f), 0f);
				if (T.Table == null)
				{
					Game.Player.Character.Position = T.Table.Position;
					Notification.Show("TABLE IS NULL (POKER) {" + text + "}");
				}
				ped.Position = T.Table.GetOffsetPosition(new Vector3(0f, 0.8f, 0.02f));
				ped.Heading = T.Table.Heading - 180f;
				T.Dealer = ped;
				if (T.Dealer == null)
				{
					Game.Player.Character.Position = T.Table.Position;
					Notification.Show("DEALER IS NULL (BLACKJACK){" + text + "}");
				}
				T.Dealer.Position = CasinoGames.SetDealerPositionAtTable(T.Table, T.TableType).Item1;
				T.Dealer.Rotation = CasinoGames.SetDealerPositionAtTable(T.Table, T.TableType).Item2;
				T.Dealer.IsPositionFrozen = true;
				Class1.SetPedOutfit(T.Dealer);
				Class1.MainPed.Add(T.Dealer);
				CasinoGames.PlayDealerAnim(T.Dealer);
				T.Dealer.RelationshipGroup = 2;
				T.Dealer.AlwaysKeepTask = true;
				T.Dealer.BlockPermanentEvents = true;
				T.DealerOriginHeading = T.Dealer.Heading;
				T.DealerOriginPosition = new Vector3(T.Dealer.Position.X, T.Dealer.Position.Y, T.Dealer.Position.Z - 1f);
				T.CreatedDealer = true;
			}
			catch
			{
			}
		}

		public static void Setup_Poker_DealerTable(DealerTable T)
		{
			if (!(T.Table != null) || !T.Table.Exists() || T.CreatedDealer)
			{
				return;
			}
			try
			{
				if (T.Dealer != null)
				{
					T.Dealer.Delete();
				}
				if (T.Table == null)
				{
					Notification.Show("TABLE IS NULL");
				}
				List<string> list = new List<string>();
				list.Add("s_m_y_casino_01");
				list.Add("s_f_y_casino_01");
				list.Add("s_m_y_casino_01");
				list.Add("s_f_y_casino_01");
				list.Add("s_m_y_casino_01");
				list.Add("s_f_y_casino_01");
				list.Add("s_m_y_casino_01");
				list.Add("s_f_y_casino_01");
				string text = list[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list.Count)];
				Ped ped = GlobalHelper.CreatePed(RequestModel(text), new Vector3(0f, 0f, 0f), 0f);
				if (T.Table == null)
				{
					Game.Player.Character.Position = T.Table.Position;
					Notification.Show("TABLE IS NULL (POKER) {" + text + "}");
				}
				ped.Position = T.Table.GetOffsetPosition(new Vector3(0f, 0.8f, 0.02f));
				ped.Heading = T.Table.Heading - 180f;
				T.Dealer = ped;
				if (T.Dealer == null)
				{
					Game.Player.Character.Position = T.Table.Position;
					Notification.Show("DEALER IS NULL (POKER) {" + text + "}");
				}
				T.Dealer.Position = CasinoGames.SetDealerPositionAtTable(T.Table, T.TableType).Item1;
				T.Dealer.Rotation = CasinoGames.SetDealerPositionAtTable(T.Table, T.TableType).Item2;
				T.Dealer.IsPositionFrozen = true;
				Class1.SetPedOutfit(T.Dealer);
				Class1.MainPed.Add(T.Dealer);
				CasinoGames.PlayDealerAnim(T.Dealer);
				T.Dealer.RelationshipGroup = 2;
				T.Dealer.AlwaysKeepTask = true;
				T.Dealer.BlockPermanentEvents = true;
				T.DealerOriginHeading = T.Dealer.Heading;
				T.DealerOriginPosition = new Vector3(T.Dealer.Position.X, T.Dealer.Position.Y, T.Dealer.Position.Z - 1f);
				T.CreatedDealer = true;
			}
			catch
			{
			}
		}

		public static void OnTick(object sender, EventArgs e)
		{
			if (Class1.Initialized || Class1.Initialized)
			{
				return;
			}
			foreach (string ambientPedModel in CasinoPeds.ambientPedModelList)
			{
				RequestModel(ambientPedModel);
			}
			CasinoPeds.CreateSecondaryPeds();
			CasinoGames.HighLimitBlackjackTable1 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1129.4f, 262.39f, -52f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_blckjack_01b"), 0, 0, 0);
			if (CasinoGames.HighLimitBlackjackTable1 != null)
			{
				CasinoGames.HighLimitBlackjackTable1.Delete();
			}
			CasinoGames.HighLimitBlackjackTable2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1144.5f, 247.24f, -52f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_blckjack_01b"), 0, 0, 0);
			if (CasinoGames.HighLimitBlackjackTable2 != null)
			{
				CasinoGames.HighLimitBlackjackTable2.Delete();
			}
			CasinoGames.NormalBlackjackTable1 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1151.84f, 266.747f, -52.84095f, 3f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_blckjack_01"), 0, 0, 0);
			if (CasinoGames.NormalBlackjackTable1 != null)
			{
				CasinoGames.NormalBlackjackTable1.Delete();
			}
			CasinoGames.NormalBlackjackTable2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1148.837f, 269.747f, -52.84095f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_blckjack_01"), 0, 0, 0);
			if (CasinoGames.NormalBlackjackTable2 != null)
			{
				CasinoGames.NormalBlackjackTable2.Delete();
			}
			CasinoGames.HighLimitPokerTable1 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1134.04f, 266.7f, -52f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_3cardpoker_01b"), 0, 0, 0);
			if (CasinoGames.HighLimitPokerTable1 != null)
			{
				CasinoGames.HighLimitPokerTable1.Delete();
			}
			CasinoGames.HighLimitPokerTable2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1148.63f, 251.38f, -52f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_3cardpoker_01b"), 0, 0, 0);
			if (CasinoGames.HighLimitPokerTable2 != null)
			{
				CasinoGames.HighLimitPokerTable2.Delete();
			}
			CasinoGames.NormalPokerTable1 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1146.226f, 260.99f, -52f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_3cardpoker_01"), 0, 0, 0);
			if (CasinoGames.NormalPokerTable1 != null)
			{
				CasinoGames.NormalPokerTable1.Delete();
			}
			CasinoGames.NormalPokerTable2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1143.513f, 264.31f, -52f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_3cardpoker_01"), 0, 0, 0);
			if (CasinoGames.NormalPokerTable2 != null)
			{
				CasinoGames.NormalPokerTable2.Delete();
			}
			TableObjects.CreateEntites();
			foreach (Utils.ObjectPlacementMap entity in TableObjects.Entities)
			{
				if (entity.Entity != null)
				{
					if (entity.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_3cardpoker_01b"))
					{
						CasinoGames.PokerTables.Add(entity.Position);
						CasinoGames.PokerTablesRot.Add(entity.Rotation.Z);
						DealerTable dealerTable = new DealerTable((Prop)entity.Entity, TableType.Poker);
						CasinoGames.PokerDealerTables.Add(dealerTable);
						ManageDuplicateTables.CasinoTables.Add((Prop)entity.Entity);
						Setup_Poker_DealerTable(dealerTable);
						Function.Call(Hash.SET_OBJECT_TINT_INDEX, (Prop)entity.Entity, entity.ObjectColor);
					}
					if (entity.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_blckjack_01b"))
					{
						CasinoGames.BlackJackTables.Add(entity.Position);
						CasinoGames.BlackJackRot.Add(entity.Rotation.Z);
						DealerTable dealerTable2 = new DealerTable((Prop)entity.Entity, TableType.Blackjack);
						CasinoGames.BlackjackDealerTables.Add(dealerTable2);
						ManageDuplicateTables.CasinoTables.Add((Prop)entity.Entity);
						Setup_Blackjack_DealerTable(dealerTable2);
						Function.Call(Hash.SET_OBJECT_TINT_INDEX, (Prop)entity.Entity, entity.ObjectColor);
					}
					if (entity.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_blckjack_01"))
					{
						CasinoGames.BlackJackTables.Add(entity.Position);
						CasinoGames.BlackJackRot.Add(entity.Rotation.Z);
						DealerTable dealerTable3 = new DealerTable((Prop)entity.Entity, TableType.Blackjack);
						CasinoGames.BlackjackDealerTables.Add(dealerTable3);
						ManageDuplicateTables.CasinoTables.Add((Prop)entity.Entity);
						Setup_Blackjack_DealerTable(dealerTable3);
						Function.Call(Hash.SET_OBJECT_TINT_INDEX, (Prop)entity.Entity, entity.ObjectColor);
					}
					if (entity.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_3cardpoker_01"))
					{
						CasinoGames.PokerTables.Add(entity.Position);
						CasinoGames.PokerTablesRot.Add(entity.Rotation.Z);
						DealerTable dealerTable4 = new DealerTable((Prop)entity.Entity, TableType.Poker);
						CasinoGames.PokerDealerTables.Add(dealerTable4);
						ManageDuplicateTables.CasinoTables.Add((Prop)entity.Entity);
						Setup_Poker_DealerTable(dealerTable4);
						Function.Call(Hash.SET_OBJECT_TINT_INDEX, (Prop)entity.Entity, entity.ObjectColor);
					}
					if (entity.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_roulette_01b"))
					{
						CasinoGames.RouletteTables.Add(entity.Position);
						CasinoGames.RouletteTablesRot.Add(entity.Rotation.Z);
						DealerTable dealerTable5 = new DealerTable((Prop)entity.Entity, TableType.Roulette);
						CasinoGames.RouletteDealerTables.Add(dealerTable5);
						ManageDuplicateTables.CasinoTables.Add((Prop)entity.Entity);
						Setup_Roulette_DealerTable(dealerTable5);
						Function.Call(Hash.SET_OBJECT_TINT_INDEX, (Prop)entity.Entity, entity.ObjectColor);
					}
				}
				else
				{
					Notification.Show("Dealertable " + entity.Inputtedhash + " Is Null");
				}
			}
			DrinksMenu.Bartenders.CreateEntites();
			foreach (Utils.ObjectPlacementMap entity2 in DrinksMenu.Bartenders.Entities)
			{
				if (entity2.Entity != null)
				{
					entity2.Entity.IsPositionFrozen = true;
					entity2.Entity.Heading = entity2.Rotation.Z;
					entity2.Entity.Position = new Vector3(entity2.Position.X, entity2.Position.Y, entity2.Position.Z - 1f);
					Class1.SetPedOutfit((Ped)entity2.Entity);
					CasinoPeds.assignPedProperties((Ped)entity2.Entity, false);
				}
			}
			CasinoGames.SetupGames();
			CasinoGames.CreateBlackJackPresets(CasinoGames.DealerPresetCars);
			Function.Call(Hash.REQUEST_IPL, "hei_dlc_casino_aircon");
			Function.Call(Hash.REQUEST_IPL, "hei_dlc_casino_aircon_lod");
			Function.Call(Hash.REQUEST_IPL, "hei_dlc_casino_door");
			Function.Call(Hash.REQUEST_IPL, "hei_dlc_casino_door_lod");
			Function.Call(Hash.REQUEST_IPL, "hei_dlc_vw_roofdoors_locked");
			Function.Call(Hash.REQUEST_IPL, "hei_dlc_windows_casino");
			Function.Call(Hash.REQUEST_IPL, "hei_dlc_windows_casino_lod");
			Function.Call(Hash.REQUEST_IPL, "vw_ch3_additions");
			Function.Call(Hash.REQUEST_IPL, "vw_ch3_additions_long_0");
			Function.Call(Hash.REQUEST_IPL, "vw_ch3_additions_strm_0");
			Function.Call(Hash.REQUEST_IPL, "vw_dlc_casino_door");
			Function.Call(Hash.REQUEST_IPL, "vw_dlc_casino_door_lod");
			Function.Call(Hash.REQUEST_IPL, "vw_int_placement_vw_interior_0_dlc_casino_main_milo_");
			Function.Call(Hash.REQUEST_IPL, "vw_int_placement_vw_interior_2_dlc_casino_garage_milo_");
			Function.Call(Hash.REQUEST_IPL, "vw_int_placement_vw_interior_4_dlc_casino_carpark_milo_");
			Function.Call(Hash.REQUEST_IPL, "vw_casino_penthouse");
			Function.Call(Hash.REQUEST_IPL, "vw_casino_carpark");
			Function.Call(Hash.REQUEST_IPL, "vw_casino_main");
			Function.Call(Hash.REQUEST_IPL, "vw_casino_garage");
			if (CasinoGames._8PlayerPokerGameTable1 != null)
			{
				CasinoGames._8PlayerPokerGameTable1.Delete();
			}
			if (CasinoGames._8PlayerPokerGameTable2 != null)
			{
				CasinoGames._8PlayerPokerGameTable2.Delete();
			}
			if (CasinoGames._8PlayerPokerGameTable3 != null)
			{
				CasinoGames._8PlayerPokerGameTable3.Delete();
			}
			CasinoGames._8PlayerPokerTables.Clear();
			CasinoGames._8PlayerPokerGameTable1 = GlobalHelper.CreateProp(Class1.RequestModel("vw_prop_casino_3cardpoker_01"), new Vector3(1136.664f, 237.4058f, -51.5f), new Vector3(0f, 0f, -138f), false, false);
			CasinoGames._8PlayerPokerGameTable2 = GlobalHelper.CreateProp(Class1.RequestModel("vw_prop_casino_3cardpoker_01"), CasinoGames._8PlayerPokerGameTable1.GetOffsetPosition(new Vector3(0f, 1.36f, 0f)), new Vector3(0f, 0f, CasinoGames._8PlayerPokerGameTable1.Rotation.Z - 180f), false, false);
			CasinoGames._8PlayerPokerGameTable3 = GlobalHelper.CreateProp(Class1.RequestModel("prop_arm_wrestle_01"), CasinoGames._8PlayerPokerGameTable1.GetOffsetPosition(new Vector3(0f, 0.7f, -0.08f)), new Vector3(0f, 0f, CasinoGames._8PlayerPokerGameTable1.Rotation.Z), false, false);
			if (CasinoGames._8PlayerGameDealer != null)
			{
				CasinoGames._8PlayerGameDealer.Delete();
			}
			CasinoGames._8PlayerGameDealer = GlobalHelper.CreatePed(Class1.RequestModel(CasinoGames.GetRandomDealerModel()), new Vector3(1136.207f, 236.0575f, -50.4f), -45f);
			if (CasinoGames._8PlayerGameDealer != null)
			{
				Class1.SetPedOutfit(CasinoGames._8PlayerGameDealer);
				Script.Wait(1);
				CasinoGames.PlayDealerAnim(CasinoGames._8PlayerGameDealer);
				Class1.MainPed.Add(CasinoGames._8PlayerGameDealer);
				Script.Wait(1);
				Vector3 position = CasinoGames._8PlayerGameDealer.Position;
				CasinoGames._8PlayerGameDealer.Position = Class1.MarkerEnter;
				Script.Wait(1);
				CasinoGames._8PlayerGameDealer.Position = position;
			}
			try
			{
				if (Class1.WheelProp2 != null)
				{
					Class1.WheelProp2.Delete();
				}
				if (Class1.WheelProp != null)
				{
					Class1.WheelProp.Delete();
				}
				float num = 90f;
				Vector3 vector = new Vector3(CasinoGames.LuckyWheel.X, CasinoGames.LuckyWheel.Y + 1.2f, CasinoGames.LuckyWheel.Z + 1.2f);
				Class1.WheelProp = GlobalHelper.CreateProp(Class1.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(vector.X, vector.Y, vector.Z + 0.2f), new Vector3(0f, 90f, num - 90f), false, false);
				CasinoGames.ReelABase.IsPositionFrozen = true;
				Vector3 thraxPos = Class1.ThraxPos;
				thraxPos = new Vector3(thraxPos.X, thraxPos.Y, thraxPos.Z - 5.1f);
				if (Class1.BasePlate != null)
				{
					Class1.BasePlate.Delete();
				}
				Class1.BasePlate = GlobalHelper.CreateProp(Class1.RequestModel("xs_prop_arena_flipper_small_01a_sf"), thraxPos, true, false);
				Class1.BasePlate.Rotation = new Vector3(0f, 0f, 0f);
				Class1.BasePlate.IsPositionFrozen = true;
				Class1.BasePlate.IsVisible = false;
				Class1.BasePlate.IsCollisionEnabled = true;
			}
			catch (Exception)
			{
			}
			try
			{
				if (Class1.WheelProp2 != null)
				{
					Class1.WheelProp2.Delete();
				}
				Class1.WheelProp2 = GlobalHelper.CreateProp(Class1.RequestModel("vw_prop_vw_luckywheel_02a"), Class1.WheelProp.Position, new Vector3(0f, 0f, Class1.WheelProp.Heading), false, false);
				Class1.WheelProp2.IsPositionFrozen = true;
				Class1.WheelProp2.AttachTo(Class1.WheelProp, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f));
				Class1.WheelProp2.IsVisible = true;
			}
			catch (Exception)
			{
				Notification.Show("~r~Prize Wheel Markers failed to spawn~");
			}
			Class1.SetupCasinoPeds = true;
			Prop[] nearbyProps = World.GetNearbyProps(Class1.MarkerExit, 3000f, Class1.RequestModel("vw_prop_casino_3cardpoker_01"));
			foreach (Prop prop in nearbyProps)
			{
				if (prop != null && prop != CasinoGames._8PlayerPokerGameTable4 && prop != CasinoGames._8PlayerPokerGameTable5 && prop != CasinoGames._8PlayerPokerGameTable6 && prop != CasinoGames._8PlayerPokerGameTable7)
				{
					CasinoGames.Tables.Add(prop);
				}
			}
			nearbyProps = World.GetNearbyProps(Class1.MarkerExit, 3000f, Class1.RequestModel("vw_prop_casino_3cardpoker_01b"));
			foreach (Prop prop2 in nearbyProps)
			{
				if (prop2 != null)
				{
					CasinoGames.Tables.Add(prop2);
				}
			}
			nearbyProps = World.GetNearbyProps(Class1.MarkerExit, 3000f, Class1.RequestModel("vw_prop_casino_blckjack_01"));
			foreach (Prop prop3 in nearbyProps)
			{
				if (prop3 != null)
				{
					CasinoGames.Tables.Add(prop3);
				}
			}
			nearbyProps = World.GetNearbyProps(Class1.MarkerExit, 3000f, Class1.RequestModel("vw_prop_casino_blckjack_01b"));
			foreach (Prop prop4 in nearbyProps)
			{
				if (prop4 != null)
				{
					CasinoGames.Tables.Add(prop4);
				}
			}
			CasinoGames.RouletteTableA = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1147.584f, 265.6707f, -52.8f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_roulette_01b"), 0, 0, 0);
			CasinoGames.Tables2.Add(CasinoGames.RouletteTableA);
			CasinoGames.NormalRouleteTable1 = CasinoGames.RouletteTableA;
			CasinoGames.RouletteTableB = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1148.889f, 248.0665f, -52f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_roulette_01b"), 0, 0, 0);
			CasinoGames.Tables2.Add(CasinoGames.RouletteTableB);
			CasinoGames.HighLimitRouleteTable1 = CasinoGames.RouletteTableB;
			CasinoGames.RouletteTableC = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1129.883f, 267.433f, -52, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_roulette_01b"), 0, 0, 0);
			CasinoGames.Tables2.Add(CasinoGames.RouletteTableC);
			CasinoGames.HighLimitRouleteTable2 = CasinoGames.RouletteTableC;
			Vector3 position2 = Game.Player.Character.Position;
			if (Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position2.X, position2.Y, position2.Z) == 275201)
			{
				if (Zones.CurrentZone == Zones.ZoneIn.None)
				{
					CasinoPeds.DeleteGamingAreaPeds();
					CasinoPeds.SpawnHallAPeds();
					CasinoPeds.SpawnHallBPeds();
					CasinoPeds.SpawnMainHallPeds();
					Zones.CurrentZone = Zones.ZoneIn.MainHall;
					Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
				}
				Class1.IsinInterior = true;
			}
			Class1.Initialized = true;
		}
	}
}
