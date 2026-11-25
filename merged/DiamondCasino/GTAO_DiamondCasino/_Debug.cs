using System;
using GTA;

namespace GTAO_DiamondCasino
{
	internal class _Debug : Script
	{
		public static int ZonesO;

		public static int CasinoGames_Main;

		public static int CasinoGames_Blackjack;

		public static int CasinoGames_Poker;

		public static int CasinoGames_HorseBettingArea;

		public static int CasinoGames_Roulette;

		public static int CasinoGames_Slots;

		public static int InsideTrack;

		public static int ManageCasinoDealerTables;

		public static int ManageDuplicateTables;

		public static int ManageSlotMachines;

		public static int DrinksMenu;

		public static int PurchaseChips;

		public static int PurchasePenthouse;

		public static int CasinoPeds;

		public static int Valet;

		public static int WalkingPeds;

		public static int PenthouseArtPlacement;

		public static int PenthouseExteriroPeds;

		public static int PenthouseGarage;

		public static int PenthouseParty;

		public static int PenthouseStuffO;

		public static int CasinoScreens;

		public static int HorseBettingScreens;

		public static int PenthouseScreens;

		public static int PrizeWheel;

		public static int Main;

		public static int EnterExit;

		public static int Initialize;

		public static int EnterExitPenthouseFromTerrace;

		public static int PenthouseEnterCutscene;

		public static int PenthouseInteiror;

		public static int PenthouseScreen;

		public static int EightPlayerPoker;

		public static int Elevators;

		public static int ElevatorDoors;

		public static int PlayableOnlineCharacterIntergration;

		public static bool Enable_Debugging;

		public static bool RunMethodNow()
		{
			return true;
		}

		public _Debug()
		{
			base.Tick += OnTick;
		}

		public static void OnTick(object sender, EventArgs e)
		{
			if (Game.IsControlPressed(Control.Sprint) && Game.IsControlJustPressed(Control.VehicleFlySelectTargetRight) && Game.Player.Name.Equals("CINNBARGNU"))
			{
				Enable_Debugging = !Enable_Debugging;
			}
			if (Enable_Debugging && Enable_Debugging)
			{
				float num = 0.06f;
				if (CasinoGames_Main == 1)
				{
					Class1.drawText("Running::CasinoGames_Main", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (ZonesO == 1)
				{
					Class1.drawText("GTAO Casino In Sp Entities: " + GlobalHelper.Mod_Props.Count + " | " + GlobalHelper.Mod_Peds.Count + " | " + GlobalHelper.Mod_Vehicles.Count + " | " + GlobalHelper.Mod_Blips.Count + " | " + GlobalHelper.Mod_PTFX.Count, 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
					Class1.drawText("Running::Zones", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (PrizeWheel == 1)
				{
					Class1.drawText("Running::PrizeWheel", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (PlayableOnlineCharacterIntergration == 1)
				{
					Class1.drawText("Running::PlayableOnlineCharacterIntergration", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (Elevators == 1)
				{
					Class1.drawText("Running::Elevators", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (ElevatorDoors == 1)
				{
					Class1.drawText("Running::ElevatorDoors", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (PenthouseScreen == 1)
				{
					Class1.drawText("Running::PenthouseScreen", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (PenthouseInteiror == 1)
				{
					Class1.drawText("Running::PenthouseInteiror", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (EnterExitPenthouseFromTerrace == 1)
				{
					Class1.drawText("Running::EnterExitPenthouseFromTerrace", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (PenthouseEnterCutscene == 1)
				{
					Class1.drawText("Running::PenthouseEnterCutscene", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (CasinoGames_HorseBettingArea == 1)
				{
					Class1.drawText("Running::CasinoGames_HorseBettingArea", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (CasinoGames_Blackjack == 1)
				{
					Class1.drawText("Running::CasinoGames_Blackjack", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (CasinoGames_Poker == 1)
				{
					Class1.drawText("Running::CasinoGames_Poker", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (CasinoGames_Roulette == 1)
				{
					Class1.drawText("Running::CasinoGames_Roulette", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (PenthouseArtPlacement == 1)
				{
					Class1.drawText("Running::PenthouseArtPlacement", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (CasinoGames_Slots == 1)
				{
					Class1.drawText("Running::CasinoGames_Slots", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (EightPlayerPoker == 1)
				{
					Class1.drawText("Running::CasinoGames_EightPlayerPoker", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (InsideTrack == 1)
				{
					Class1.drawText("Running::InsideTrack", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (ManageCasinoDealerTables == 1)
				{
					Class1.drawText("Running::ManageCasinoDealerTables", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (ManageDuplicateTables == 1)
				{
					Class1.drawText("Running::ManageDuplicateTables", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (ManageSlotMachines == 1)
				{
					Class1.drawText("Running::ManageSlotMachines", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (DrinksMenu == 1)
				{
					Class1.drawText("Running::DrinksMenu", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (PurchaseChips == 1)
				{
					Class1.drawText("Running::PurchaseChips", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (PurchasePenthouse == 1)
				{
					Class1.drawText("Running::PurchasePenthouse", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (CasinoPeds == 1)
				{
					Class1.drawText("Running::CasinoPeds", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (Valet == 1)
				{
					Class1.drawText("Running::Valet", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (WalkingPeds == 1)
				{
					Class1.drawText("Running::WalkingPeds", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (PenthouseExteriroPeds == 1)
				{
					Class1.drawText("Running::PenthouseExteriroPeds", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (PenthouseGarage == 1)
				{
					Class1.drawText("Running::PenthouseGarage", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (PenthouseParty == 1)
				{
					Class1.drawText("Running::PenthouseParty", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (PenthouseStuffO == 1)
				{
					Class1.drawText("Running::PenthouseStuff", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (CasinoScreens == 1)
				{
					Class1.drawText("Running::CasinoScreens", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (HorseBettingScreens == 1)
				{
					Class1.drawText("Running::HorseBettingScreens", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (PenthouseScreens == 1)
				{
					Class1.drawText("Running::PenthouseScreens", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (Main == 1)
				{
					Class1.drawText("Running::Main", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (EnterExit == 1)
				{
					Class1.drawText("Running::EnterExit", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (Initialize == 1)
				{
					Class1.drawText("Running::Initializex1", 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
				if (Class1.IsinInterior || Class1.GetDistance_VDIST2(Game.Player.Character.Position, Class1.MarkerEnter, Class1.GetDistSign.Less, 200f))
				{
					Class1.drawText("Running::On Penthouse Terrace " + Zones.PenthouseTerraceZone.Zone.WithBoundary(Zones.PlayerPoint).ToString() + "/" + PenthouseStuff.IsOnPenthouseTerrace, 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
					Class1.drawText("Running::In Penthouse " + Zones.PenthouseZone.Zone.WithBoundary(Zones.PlayerPoint).ToString() + "/" + PenthouseStuff.IsInPenhouse, 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
					Class1.drawText("Running::Zone " + Zones.CurrentZone, 0.8f, num, 0.3f, 255, 255, 255);
					num += 0.03f;
				}
			}
		}
	}
}
