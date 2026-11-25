using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTAO_DiamondCasino.Menus;
using GTAO_DiamondCasino.Penthouse;

namespace GTAO_DiamondCasino
{
	internal class ManageBlips : Script
	{
		public static Blip MainBlip;

		public static List<Blip> InteriorBlip = new List<Blip>();

		public static Blip PenthouseOptions;

		public static Blip ExitPenthouse;

		public static Blip EnterPenthouse;

		public static Blip ExitGarage;

		public static Blip Elevator;

		public static bool SetupInteriorBlips;

		public static bool CreateSecondaryBlips;

		public static bool RunMethodNow()
		{
			return true;
		}

		public ManageBlips()
		{
			base.Tick += OnTick;
			MainBlip = GlobalHelper.CreateBlip(Class1.MarkerEnter);
			MainBlip.Sprite = BlipSprite.Casino;
			MainBlip.Color = BlipColor.White;
			MainBlip.IsShortRange = true;
		}

		public static void OnTick(object sender, EventArgs e)
		{
			if (!CreateSecondaryBlips)
			{
				if (Class1.Purchased_Penthouse == 1)
				{
					CreatePenthouseAndGarageBlips();
				}
				CreateSecondaryBlips = true;
			}
			else
			{
				if (!PenthouseStuff.IsInPenhouse)
				{
					if (PenthouseOptions != null)
					{
						PenthouseOptions.Alpha = 0;
					}
					if (ExitPenthouse != null)
					{
						ExitPenthouse.Alpha = 0;
					}
				}
				else
				{
					if (PenthouseOptions != null)
					{
						PenthouseOptions.Alpha = 255;
					}
					if (ExitPenthouse != null)
					{
						ExitPenthouse.Alpha = 255;
					}
				}
				if (!PenthouseGarage.InGarage)
				{
					if (ExitGarage != null)
					{
						ExitGarage.Alpha = 0;
					}
				}
				else if (ExitGarage != null)
				{
					ExitGarage.Alpha = 255;
				}
				if (!PenthouseStuff.IsOnPenthouseTerrace)
				{
					if (EnterPenthouse != null)
					{
						EnterPenthouse.Alpha = 0;
					}
				}
				else if (EnterPenthouse != null)
				{
					EnterPenthouse.Alpha = 255;
				}
				if (!Class1.IsinInterior)
				{
					if (Elevator != null)
					{
						Elevator.Alpha = 0;
					}
				}
				else if (Elevator != null)
				{
					Elevator.Alpha = 255;
				}
			}
			if (!Class1.IsinInterior)
			{
				if (!SetupInteriorBlips)
				{
					return;
				}
				foreach (Blip item in InteriorBlip)
				{
					if (item != null)
					{
						item.Delete();
					}
				}
				InteriorBlip.Clear();
				SetupInteriorBlips = false;
			}
			else
			{
				if (SetupInteriorBlips || SetupInteriorBlips)
				{
					return;
				}
				SetupInteriorBlips = true;
				foreach (Blip item2 in InteriorBlip)
				{
					if (item2 != null)
					{
						item2.Delete();
					}
				}
				Blip blip = GlobalHelper.CreateBlip(new Vector3(1115.6f, 219.17f, -49f));
				blip.Sprite = BlipSprite.Chips;
				blip.Name = "Purchase Chips Here";
				InteriorBlip.Add(blip);
				blip = GlobalHelper.CreateBlip(new Vector3(1096.2f, 260.8f, -50f));
				blip.Sprite = BlipSprite.HorseRacing;
				blip.Name = "Inside Track";
				InteriorBlip.Add(blip);
				blip = GlobalHelper.CreateBlip(new Vector3(1143.3f, 264.2f, -52f));
				blip.Sprite = BlipSprite.TableGames;
				blip.Name = "Poker";
				InteriorBlip.Add(blip);
				blip = GlobalHelper.CreateBlip(new Vector3(1146.09f, 261.4f, -52f));
				blip.Sprite = BlipSprite.TableGames;
				blip.Name = "Poker";
				InteriorBlip.Add(blip);
				blip = GlobalHelper.CreateBlip(new Vector3(1147.6f, 265.4f, -52f));
				blip.Sprite = BlipSprite.LuckyWheel;
				blip.Name = "Roulette";
				InteriorBlip.Add(blip);
				blip = GlobalHelper.CreateBlip(new Vector3(1111.1f, 228.8f, -49f));
				blip.Sprite = BlipSprite.LuckyWheel;
				blip.Name = "Lucky Wheel";
				InteriorBlip.Add(blip);
				blip = GlobalHelper.CreateBlip(new Vector3(1148.8f, 269.5f, -52f));
				blip.Sprite = BlipSprite.TableGames;
				blip.Name = "Blackjack";
				InteriorBlip.Add(blip);
				blip = GlobalHelper.CreateBlip(new Vector3(1151.6f, 266.8f, -52f));
				blip.Sprite = BlipSprite.TableGames;
				blip.Name = "Blackjack";
				InteriorBlip.Add(blip);
				blip = GlobalHelper.CreateBlip(new Vector3(1144.18f, 246.9f, -51f));
				blip.Sprite = BlipSprite.TableGames;
				blip.Name = "Blackjack";
				InteriorBlip.Add(blip);
				blip = GlobalHelper.CreateBlip(new Vector3(1129.2f, 262.25f, -51f));
				blip.Sprite = BlipSprite.TableGames;
				blip.Name = "Blackjack";
				InteriorBlip.Add(blip);
				blip = GlobalHelper.CreateBlip(new Vector3(1146.09f, 261.4f, -52f));
				blip.Sprite = BlipSprite.TableGames;
				blip.Name = "Poker";
				InteriorBlip.Add(blip);
				blip = GlobalHelper.CreateBlip(new Vector3(1149.03f, 247.88f, -51f));
				blip.Sprite = BlipSprite.LuckyWheel;
				blip.Name = "Roulette";
				InteriorBlip.Add(blip);
				blip = GlobalHelper.CreateBlip(new Vector3(1129.8f, 267.6f, -51f));
				blip.Sprite = BlipSprite.LuckyWheel;
				blip.Name = "Roulette";
				InteriorBlip.Add(blip);
				blip = GlobalHelper.CreateBlip(new Vector3(1133.5f, 266.5f, -51f));
				blip.Sprite = BlipSprite.TableGames;
				blip.Name = "Poker";
				InteriorBlip.Add(blip);
				blip = GlobalHelper.CreateBlip(new Vector3(1149f, 251.9f, -50f));
				blip.Sprite = BlipSprite.TableGames;
				blip.Name = "Poker";
				InteriorBlip.Add(blip);
				blip = GlobalHelper.CreateBlip(PurchasePenthouse.PurchaseMarker);
				blip.Sprite = BlipSprite.Concierge;
				blip.Name = "Purchase/Manage Penthouse";
				InteriorBlip.Add(blip);
				if (CasinoGames._8PlayerPokerGameTable3 != null)
				{
					blip = GlobalHelper.CreateBlip(CasinoGames._8PlayerPokerGameTable3.Position);
					blip.Sprite = BlipSprite.TableGames;
					blip.Name = "8 Player Poker";
					InteriorBlip.Add(blip);
				}
			}
		}

		public static void CreatePenthouseAndGarageBlips()
		{
			Elevator = GlobalHelper.CreateBlip(new Vector3(1085.146f, 214.272f, -50.2f));
			Elevator.Sprite = BlipSprite.Elevator;
			Elevator.Name = "Elevator";
			EnterPenthouse = GlobalHelper.CreateBlip(EnterExitPenthouseFromTerrace.PenthouseEnter2);
			EnterPenthouse.Sprite = BlipSprite.Safehouse;
			EnterPenthouse.Name = "Enter Penthouse";
			ExitPenthouse = GlobalHelper.CreateBlip(EnterExitPenthouseFromTerrace.PenthouseMainEnter);
			ExitPenthouse.Sprite = BlipSprite.Elevator;
			ExitPenthouse.Name = "Exit Penthouse";
			ExitGarage = GlobalHelper.CreateBlip(new Vector3(Elevators.Garage_ElevatorOut.X, Elevators.Garage_ElevatorOut.Y, Elevators.Garage_ElevatorOut.Z));
			ExitGarage.Sprite = BlipSprite.Elevator;
			ExitGarage.Name = "Exit Garage";
			PenthouseOptions = GlobalHelper.CreateBlip(PenthouseParty.PartyStartMarker);
			PenthouseOptions.Sprite = BlipSprite.Concierge;
			PenthouseOptions.Name = "Penthouse Options";
		}
	}
}
