using System;
using GTA;
using GTA.Math;
using SinglePlayerHeists.UtilsMisc;

namespace SinglePlayerHeists
{
	internal class StartHeistMarkers : Script
	{
		public StartHeistMarkers()
		{
			base.Tick += OnTick;
		}

		private void OnTick(object sender, EventArgs e)
		{
			if (Main.ArcadeOwned != 1 || Class1.Activities.Count <= 0 || !Class1.nearActivity || Class1.PlayingFleeca || Class1.PlayingPaleto || Class1.PlayingPacificStandard || Class1.JewelStoreRobberyOn || Class1.PlayingUnion || Class1.InLaFuetaBlancaHeist || Class1.PlayingBikerClubhouseHeist || Main.ArcadeOwned != 1)
			{
				return;
			}
			foreach (Class1.Activity activity in Class1.Activities)
			{
				Vector3 location21 = activity.Location;
				if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, ">", 10f) && Class1.GetDistance_VDIST2(Class1.CurrentActivity, Game.Player.Character.Position, ">", 10f))
				{
					Class1.nearActivity = false;
				}
				if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 4f))
				{
					Class1.CurrentActivity = activity.Location;
					Class1.nearActivity = true;
				}
				if (activity.Id == Main.HeistID.BikerClubhouse)
				{
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 60f))
					{
						World.DrawMarker(MarkerType.Cylinder, activity.Location, Vector3.Zero, Vector3.Zero, new Vector3(2.65f, 2.65f, 0.4f), Class1.ActivityMarkerColor);
						Vector3 location = activity.Location;
						float z = (location - Game.Player.Character.Position).ToHeading();
						Scaleform scaleform = new Scaleform("PLAYER_NAME_09");
						scaleform.CallFunction("SET_PLAYER_NAME", "~b~Single Player Heists~w~\n" + activity.Name);
						scaleform.Render3D(new Vector3(location.X, location.Y, location.Z + 2f), new Vector3(0f, 0f, z), new Vector3(6f, 3f, 2f));
					}
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 2f))
					{
						Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Start The Biker Clubhouse Heist");
						if (Game.IsControlJustPressed(Control.Context))
						{
							StartHeistScreen.ToggleFinaleCam(true, Main.HeistID.BikerClubhouse);
						}
					}
				}
				if (activity.Id == Main.HeistID.TheLuxigntonRaid)
				{
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 60f))
					{
						World.DrawMarker(MarkerType.Cylinder, activity.Location, Vector3.Zero, Vector3.Zero, new Vector3(2.65f, 2.65f, 0.4f), Class1.ActivityMarkerColor);
						Vector3 location2 = activity.Location;
						float z2 = (location2 - Game.Player.Character.Position).ToHeading();
						Scaleform scaleform2 = new Scaleform("PLAYER_NAME_09");
						scaleform2.CallFunction("SET_PLAYER_NAME", "~b~Single Player Heists~w~\n" + activity.Name);
						scaleform2.Render3D(new Vector3(location2.X, location2.Y, location2.Z + 2f), new Vector3(0f, 0f, z2), new Vector3(6f, 3f, 2f));
					}
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 2f))
					{
						Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Start The Luxignton Raid Heist");
						if (Game.IsControlJustPressed(Control.Context))
						{
							StartHeistScreen.ToggleFinaleCam(true, Main.HeistID.TheLuxigntonRaid);
						}
					}
				}
				if (activity.Id == Main.HeistID.TheBillionairesParty)
				{
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 60f))
					{
						World.DrawMarker(MarkerType.Cylinder, activity.Location, Vector3.Zero, Vector3.Zero, new Vector3(2.65f, 2.65f, 0.4f), Class1.ActivityMarkerColor);
						Vector3 location3 = activity.Location;
						float z3 = (location3 - Game.Player.Character.Position).ToHeading();
						Scaleform scaleform3 = new Scaleform("PLAYER_NAME_09");
						scaleform3.CallFunction("SET_PLAYER_NAME", "~b~Single Player Heists~w~\n" + activity.Name);
						scaleform3.Render3D(new Vector3(location3.X, location3.Y, location3.Z + 2f), new Vector3(0f, 0f, z3), new Vector3(6f, 3f, 2f));
					}
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 2f))
					{
						Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Start The Billionaires Party Heist");
						if (Game.IsControlJustPressed(Control.Context))
						{
							StartHeistScreen.ToggleFinaleCam(true, Main.HeistID.TheBillionairesParty);
						}
					}
				}
				if (activity.Id == Main.HeistID.TheCartelYachtHeist)
				{
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 60f))
					{
						World.DrawMarker(MarkerType.Cylinder, activity.Location, Vector3.Zero, Vector3.Zero, new Vector3(2.65f, 2.65f, 0.4f), Class1.ActivityMarkerColor);
						Vector3 location4 = activity.Location;
						float z4 = (location4 - Game.Player.Character.Position).ToHeading();
						Scaleform scaleform4 = new Scaleform("PLAYER_NAME_09");
						scaleform4.CallFunction("SET_PLAYER_NAME", "~b~Single Player Heists~w~\n" + activity.Name);
						scaleform4.Render3D(new Vector3(location4.X, location4.Y, location4.Z + 2f), new Vector3(0f, 0f, z4), new Vector3(6f, 3f, 2f));
					}
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 2f))
					{
						Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Start The Cartel Yacht Heist");
						if (Game.IsControlJustPressed(Control.Context))
						{
							StartHeistScreen.ToggleFinaleCam(true, Main.HeistID.TheCartelYachtHeist);
						}
					}
				}
				if (activity.Id == Main.HeistID.TheFederalReserveTrainHeist)
				{
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 60f))
					{
						World.DrawMarker(MarkerType.Cylinder, activity.Location, Vector3.Zero, Vector3.Zero, new Vector3(2.65f, 2.65f, 0.4f), Class1.ActivityMarkerColor);
						Vector3 location5 = activity.Location;
						float z5 = (location5 - Game.Player.Character.Position).ToHeading();
						Scaleform scaleform5 = new Scaleform("PLAYER_NAME_09");
						scaleform5.CallFunction("SET_PLAYER_NAME", "~b~Single Player Heists~w~\n" + activity.Name);
						scaleform5.Render3D(new Vector3(location5.X, location5.Y, location5.Z + 2f), new Vector3(0f, 0f, z5), new Vector3(6f, 3f, 2f));
					}
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 2f))
					{
						Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Start The Federal Reserve Train Heist");
						if (Game.IsControlJustPressed(Control.Context))
						{
							StartHeistScreen.ToggleFinaleCam(true, Main.HeistID.TheFederalReserveTrainHeist);
						}
					}
				}
				if (activity.Id == Main.HeistID.TheMazeBankSavingsBankHeist)
				{
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 60f))
					{
						World.DrawMarker(MarkerType.Cylinder, activity.Location, Vector3.Zero, Vector3.Zero, new Vector3(2.65f, 2.65f, 0.4f), Class1.ActivityMarkerColor);
						Vector3 location6 = activity.Location;
						float z6 = (location6 - Game.Player.Character.Position).ToHeading();
						Scaleform scaleform6 = new Scaleform("PLAYER_NAME_09");
						scaleform6.CallFunction("SET_PLAYER_NAME", "~b~Single Player Heists~w~\n" + activity.Name);
						scaleform6.Render3D(new Vector3(location6.X, location6.Y, location6.Z + 2f), new Vector3(0f, 0f, z6), new Vector3(6f, 3f, 2f));
					}
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 2f))
					{
						Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Start The Maze Bank Saving Bank Heist ");
						if (Game.IsControlJustPressed(Control.Context))
						{
							StartHeistScreen.ToggleFinaleCam(true, Main.HeistID.TheMazeBankSavingsBankHeist);
						}
					}
				}
				if (activity.Id == Main.HeistID.ThePrinthouseHeist)
				{
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 60f))
					{
						World.DrawMarker(MarkerType.Cylinder, activity.Location, Vector3.Zero, Vector3.Zero, new Vector3(2.65f, 2.65f, 0.4f), Class1.ActivityMarkerColor);
						Vector3 location7 = activity.Location;
						float z7 = (location7 - Game.Player.Character.Position).ToHeading();
						Scaleform scaleform7 = new Scaleform("PLAYER_NAME_09");
						scaleform7.CallFunction("SET_PLAYER_NAME", "~b~Single Player Heists~w~\n" + activity.Name);
						scaleform7.Render3D(new Vector3(location7.X, location7.Y, location7.Z + 2f), new Vector3(0f, 0f, z7), new Vector3(6f, 3f, 2f));
					}
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 2f))
					{
						Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Start The Printhouse Heist");
						if (Game.IsControlJustPressed(Control.Context))
						{
							StartHeistScreen.ToggleFinaleCam(true, Main.HeistID.ThePrinthouseHeist);
						}
					}
				}
				if (activity.Id == Main.HeistID.TheDiamondJewelStoreHeist)
				{
					if (!Game.Player.Name.ToUpper().Equals("CINNBARGNU"))
					{
						break;
					}
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 60f))
					{
						World.DrawMarker(MarkerType.Cylinder, activity.Location, Vector3.Zero, Vector3.Zero, new Vector3(2.65f, 2.65f, 0.4f), Class1.ActivityMarkerColor);
						Vector3 location8 = activity.Location;
						float z8 = (location8 - Game.Player.Character.Position).ToHeading();
						Scaleform scaleform8 = new Scaleform("PLAYER_NAME_09");
						scaleform8.CallFunction("SET_PLAYER_NAME", "~b~Single Player Heists~w~\n" + activity.Name);
						scaleform8.Render3D(new Vector3(location8.X, location8.Y, location8.Z + 2f), new Vector3(0f, 0f, z8), new Vector3(6f, 3f, 2f));
					}
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 2f))
					{
						Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Start The Diamond Jewel Store Heist");
						if (Game.IsControlJustPressed(Control.Context))
						{
							StartHeistScreen.ToggleFinaleCam(true, Main.HeistID.TheDiamondJewelStoreHeist);
						}
					}
				}
				if (activity.Id == Main.HeistID.TheBettaBankHeist)
				{
					if (!Game.Player.Name.ToUpper().Equals("CINNBARGNU"))
					{
						break;
					}
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 60f))
					{
						World.DrawMarker(MarkerType.Cylinder, activity.Location, Vector3.Zero, Vector3.Zero, new Vector3(2.65f, 2.65f, 0.4f), Class1.ActivityMarkerColor);
						Vector3 location9 = activity.Location;
						float z9 = (location9 - Game.Player.Character.Position).ToHeading();
						Scaleform scaleform9 = new Scaleform("PLAYER_NAME_09");
						scaleform9.CallFunction("SET_PLAYER_NAME", "~b~Single Player Heists~w~\n" + activity.Name);
						scaleform9.Render3D(new Vector3(location9.X, location9.Y, location9.Z + 2f), new Vector3(0f, 0f, z9), new Vector3(6f, 3f, 2f));
					}
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 2f))
					{
						Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Start The Betta Bank Heist");
						if (Game.IsControlJustPressed(Control.Context))
						{
							StartHeistScreen.ToggleFinaleCam(true, Main.HeistID.TheBettaBankHeist);
						}
					}
				}
				if (activity.Id == Main.HeistID.TheGefangnisArtHeist)
				{
					if (!Game.Player.Name.ToUpper().Equals("CINNBARGNU"))
					{
						break;
					}
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 60f))
					{
						World.DrawMarker(MarkerType.Cylinder, activity.Location, Vector3.Zero, Vector3.Zero, new Vector3(2.65f, 2.65f, 0.4f), Class1.ActivityMarkerColor);
						Vector3 location10 = activity.Location;
						float z10 = (location10 - Game.Player.Character.Position).ToHeading();
						Scaleform scaleform10 = new Scaleform("PLAYER_NAME_09");
						scaleform10.CallFunction("SET_PLAYER_NAME", "~b~Single Player Heists~w~\n" + activity.Name);
						scaleform10.Render3D(new Vector3(location10.X, location10.Y, location10.Z + 2f), new Vector3(0f, 0f, z10), new Vector3(6f, 3f, 2f));
					}
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 2f))
					{
						Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Start The Gefangnis Art Heist");
						if (Game.IsControlJustPressed(Control.Context))
						{
							StartHeistScreen.ToggleFinaleCam(true, Main.HeistID.TheGefangnisArtHeist);
						}
					}
				}
				if (activity.Id == Main.HeistID.UnionDepository)
				{
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 60f))
					{
						World.DrawMarker(MarkerType.Cylinder, activity.Location, Vector3.Zero, Vector3.Zero, new Vector3(2.65f, 2.65f, 0.4f), Class1.ActivityMarkerColor);
						Vector3 location11 = activity.Location;
						float z11 = (location11 - Game.Player.Character.Position).ToHeading();
						Scaleform scaleform11 = new Scaleform("PLAYER_NAME_05");
						scaleform11.CallFunction("SET_PLAYER_NAME", "~b~Single Player Heists~w~\n" + activity.Name);
						scaleform11.Render3D(new Vector3(location11.X, location11.Y, location11.Z + 2f), new Vector3(0f, 0f, z11), new Vector3(6f, 3f, 2f));
					}
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 2f))
					{
						Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Start Union Depository Heist");
						if (Game.IsControlJustPressed(Control.Context))
						{
							StartHeistScreen.ToggleFinaleCam(true, Main.HeistID.UnionDepository);
						}
					}
				}
				if (activity.Id == Main.HeistID.LaFuenteBlanca)
				{
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 60f))
					{
						World.DrawMarker(MarkerType.Cylinder, activity.Location, Vector3.Zero, Vector3.Zero, new Vector3(2.65f, 2.65f, 0.4f), Class1.ActivityMarkerColor);
						Vector3 location12 = activity.Location;
						float z12 = (location12 - Game.Player.Character.Position).ToHeading();
						Scaleform scaleform12 = new Scaleform("PLAYER_NAME_09");
						scaleform12.CallFunction("SET_PLAYER_NAME", "~b~Single Player Heists~w~\n" + activity.Name);
						scaleform12.Render3D(new Vector3(location12.X, location12.Y, location12.Z + 2f), new Vector3(0f, 0f, z12), new Vector3(6f, 3f, 2f));
					}
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 2f))
					{
						Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Start La Fuente Blanca Heist");
						if (Game.IsControlJustPressed(Control.Context))
						{
							StartHeistScreen.ToggleFinaleCam(true, Main.HeistID.LaFuenteBlanca);
						}
					}
				}
				if (activity.Id == Main.HeistID.Fleeca)
				{
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 60f))
					{
						World.DrawMarker(MarkerType.Cylinder, activity.Location, Vector3.Zero, Vector3.Zero, new Vector3(2.65f, 2.65f, 0.4f), Class1.ActivityMarkerColor);
						Vector3 location13 = activity.Location;
						float z13 = (location13 - Game.Player.Character.Position).ToHeading();
						Scaleform scaleform13 = new Scaleform("PLAYER_NAME_09");
						scaleform13.CallFunction("SET_PLAYER_NAME", "~b~Single Player Heists~w~\n" + activity.Name);
						scaleform13.Render3D(new Vector3(location13.X, location13.Y, location13.Z + 2f), new Vector3(0f, 0f, z13), new Vector3(6f, 3f, 2f));
					}
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 2f))
					{
						Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Start Fleeca Heist");
						if (Game.IsControlJustPressed(Control.Context))
						{
							StartHeistScreen.ToggleFinaleCam(true, Main.HeistID.Fleeca);
						}
					}
				}
				if (activity.Id == Main.HeistID.PacificStandard)
				{
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 60f))
					{
						World.DrawMarker(MarkerType.Cylinder, activity.Location, Vector3.Zero, Vector3.Zero, new Vector3(2.65f, 2.65f, 0.4f), Class1.ActivityMarkerColor);
						Vector3 location14 = activity.Location;
						float z14 = (location14 - Game.Player.Character.Position).ToHeading();
						Scaleform scaleform14 = new Scaleform("PLAYER_NAME_09");
						scaleform14.CallFunction("SET_PLAYER_NAME", "~b~Single Player Heists~w~\n" + activity.Name);
						scaleform14.Render3D(new Vector3(location14.X, location14.Y, location14.Z + 2f), new Vector3(0f, 0f, z14), new Vector3(6f, 3f, 2f));
					}
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 2f))
					{
						Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Start Pacific Standard Bank Heist");
						if (Game.IsControlJustPressed(Control.Context))
						{
							StartHeistScreen.ToggleFinaleCam(true, Main.HeistID.PacificStandard);
						}
					}
				}
				if (activity.Id == Main.HeistID.Paleto)
				{
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 60f))
					{
						World.DrawMarker(MarkerType.Cylinder, activity.Location, Vector3.Zero, Vector3.Zero, new Vector3(2.65f, 2.65f, 0.4f), Class1.ActivityMarkerColor);
						Vector3 location15 = activity.Location;
						float z15 = (location15 - Game.Player.Character.Position).ToHeading();
						Scaleform scaleform15 = new Scaleform("PLAYER_NAME_09");
						scaleform15.CallFunction("SET_PLAYER_NAME", "~b~Single Player Heists~w~\n" + activity.Name);
						scaleform15.Render3D(new Vector3(location15.X, location15.Y, location15.Z + 2f), new Vector3(0f, 0f, z15), new Vector3(6f, 3f, 2f));
					}
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 2f))
					{
						Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Start Paleto Bank Heist");
						if (Game.IsControlJustPressed(Control.Context))
						{
							StartHeistScreen.ToggleFinaleCam(true, Main.HeistID.Paleto);
						}
					}
				}
				if (activity.Id == Main.HeistID.JewelStore)
				{
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 60f))
					{
						World.DrawMarker(MarkerType.Cylinder, activity.Location, Vector3.Zero, Vector3.Zero, new Vector3(2.65f, 2.65f, 0.4f), Class1.ActivityMarkerColor);
						Vector3 location16 = activity.Location;
						float z16 = (location16 - Game.Player.Character.Position).ToHeading();
						Scaleform scaleform16 = new Scaleform("PLAYER_NAME_09");
						scaleform16.CallFunction("SET_PLAYER_NAME", "~b~Single Player Heists~w~\n" + activity.Name);
						scaleform16.Render3D(new Vector3(location16.X, location16.Y, location16.Z + 2f), new Vector3(0f, 0f, z16), new Vector3(6f, 3f, 2f));
					}
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 2f))
					{
						Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Start Jewel Store Heist");
						if (Game.IsControlJustPressed(Control.Context))
						{
							StartHeistScreen.ToggleFinaleCam(true, Main.HeistID.JewelStore);
						}
					}
				}
				if (activity.Id == Main.HeistID.LaoubPrincessCargoshipHeist)
				{
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 60f))
					{
						World.DrawMarker(MarkerType.Cylinder, activity.Location, Vector3.Zero, Vector3.Zero, new Vector3(2.65f, 2.65f, 0.4f), Class1.ActivityMarkerColor);
						Vector3 location17 = activity.Location;
						float z17 = (location17 - Game.Player.Character.Position).ToHeading();
						Scaleform scaleform17 = new Scaleform("PLAYER_NAME_09");
						scaleform17.CallFunction("SET_PLAYER_NAME", "~b~Single Player Heists~w~\n" + activity.Name);
						scaleform17.Render3D(new Vector3(location17.X, location17.Y, location17.Z + 2f), new Vector3(0f, 0f, z17), new Vector3(6f, 3f, 2f));
					}
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 2f))
					{
						Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Start La'oub Princess Cargoship Heist");
						if (Game.IsControlJustPressed(Control.Context))
						{
							StartHeistScreen.ToggleFinaleCam(true, Main.HeistID.LaoubPrincessCargoshipHeist);
						}
					}
				}
				if (activity.Id == Main.HeistID.ThePrisonBreak)
				{
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 60f))
					{
						World.DrawMarker(MarkerType.Cylinder, activity.Location, Vector3.Zero, Vector3.Zero, new Vector3(2.65f, 2.65f, 0.4f), Class1.ActivityMarkerColor);
						Vector3 location18 = activity.Location;
						float z18 = (location18 - Game.Player.Character.Position).ToHeading();
						Scaleform scaleform18 = new Scaleform("PLAYER_NAME_09");
						scaleform18.CallFunction("SET_PLAYER_NAME", "~b~Single Player Heists~w~\n" + activity.Name);
						scaleform18.Render3D(new Vector3(location18.X, location18.Y, location18.Z + 2f), new Vector3(0f, 0f, z18), new Vector3(6f, 3f, 2f));
					}
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 2f))
					{
						Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Start/Continue The Prison Break");
						if (Game.IsControlJustPressed(Control.Context))
						{
							if (ThePrisonBreak.HeistStage < 4)
							{
								ThePrisonBreak.StartHeist = 1;
							}
							else
							{
								StartHeistScreen.ToggleFinaleCam(true, Main.HeistID.ThePrisonBreak);
							}
						}
					}
				}
				if (activity.Id == Main.HeistID.TheHumaineLabs)
				{
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 60f))
					{
						World.DrawMarker(MarkerType.Cylinder, activity.Location, Vector3.Zero, Vector3.Zero, new Vector3(2.65f, 2.65f, 0.4f), Class1.ActivityMarkerColor);
						Vector3 location19 = activity.Location;
						float z19 = (location19 - Game.Player.Character.Position).ToHeading();
						Scaleform scaleform19 = new Scaleform("PLAYER_NAME_09");
						scaleform19.CallFunction("SET_PLAYER_NAME", "~b~Single Player Heists~w~\n" + activity.Name);
						scaleform19.Render3D(new Vector3(location19.X, location19.Y, location19.Z + 2f), new Vector3(0f, 0f, z19), new Vector3(6f, 3f, 2f));
					}
					if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 2f))
					{
						Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Start/Continue Humaine Labs Raid");
						if (Game.IsControlJustPressed(Control.Context))
						{
							if (HumainLabsOnline.HeistStage < 4)
							{
								HumainLabsOnline.StartHeist = 1;
							}
							else
							{
								StartHeistScreen.ToggleFinaleCam(true, Main.HeistID.TheHumaineLabs);
							}
						}
					}
				}
				if (activity.Id != Main.HeistID.TheDiamondCasino)
				{
					continue;
				}
				if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 60f))
				{
					World.DrawMarker(MarkerType.Cylinder, activity.Location, Vector3.Zero, Vector3.Zero, new Vector3(2.65f, 2.65f, 0.4f), Class1.ActivityMarkerColor);
					Vector3 location20 = activity.Location;
					float z20 = (location20 - Game.Player.Character.Position).ToHeading();
					Scaleform scaleform20 = new Scaleform("PLAYER_NAME_09");
					scaleform20.CallFunction("SET_PLAYER_NAME", "~b~Single Player Heists~w~\n" + activity.Name);
					scaleform20.Render3D(new Vector3(location20.X, location20.Y, location20.Z + 2f), new Vector3(0f, 0f, z20), new Vector3(6f, 3f, 2f));
				}
				if (!Class1.GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 2f))
				{
					continue;
				}
				Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Start/Continue The Diamond Casino Heist");
				if (Game.IsControlJustPressed(Control.Context))
				{
					if (TheDiamondHeist.HeistStage < 4)
					{
						TheDiamondHeist.RunHeist = 1;
					}
					else
					{
						StartHeistScreen.ToggleFinaleCam(true, Main.HeistID.TheDiamondCasino);
					}
				}
			}
		}
	}
}
