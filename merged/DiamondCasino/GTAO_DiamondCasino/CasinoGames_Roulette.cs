using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace GTAO_DiamondCasino
{
	internal class CasinoGames_Roulette : Script
	{
		public static bool Debug;

		public static int GetClosestRouletteMarker;

		public static int Closest_RouletteMarker = -1;

		public static CasinoGames.RouletteBetPosition Closest_BetPosition;

		public static string Closest_BetName;

		public static string Closest_BetMultiplierNotification;

		public static Vector3 Closest_BetVector3Pos;

		public static int TargetAnimEnd = 1;

		public static bool RunMethodNow()
		{
			bool result = true;
			if (CasinoGames.CurrentGame != CasinoGames.CasinoGamePlaying.Roulette)
			{
				_Debug.CasinoGames_Poker = 0;
				result = false;
			}
			return result;
		}

		public CasinoGames_Roulette()
		{
			if (_Debug.CasinoGames_Roulette != -1)
			{
				base.Tick += OnTick;
			}
		}

		public static void OnTick(object sender, EventArgs e)
		{
			_Debug.CasinoGames_Roulette = 0;
			if (CasinoGames.CurrentGame != CasinoGames.CasinoGamePlaying.Roulette || CasinoGames.CurrentGame != CasinoGames.CasinoGamePlaying.Roulette)
			{
				return;
			}
			if (Game.IsControlJustPressed(Control.VehicleDuck))
			{
				Debug = !Debug;
			}
			if (Debug)
			{
				CasinoGames.drawText("TargetAnimEnd " + TargetAnimEnd + "\nRotation" + CasinoGames.ROULETTE_PROPS_GET_WHEEL_CURRENT_ROTATION(CasinoGames.GetClosesRouletteTable()).ToString() + "\nBetPosMarkers " + CasinoGames.BetPosMarkers.Count + "\nStage " + CasinoGames.Stage + "\nMarkerNum " + CasinoGames.MarkerNum + "/" + CasinoGames.BetPosNum + "\n" + Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.RouletteSpinScene), 0.1f, 0.1f, 0.3f, 255, 255, 255);
				CasinoGames.drawText("Markernum " + CasinoGames.MarkerNum + "\nPhase " + Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.RouletteStartScene) + "\n" + CasinoGames.CreatedRouletteCam + "/" + CasinoGames.ReadyForRouletteEnd, 0.1f, 0.2f, 0.3f, 255, 255, 255);
			}
			_Debug.CasinoGames_Roulette = 1;
			if (CasinoGames.CurrentGame != CasinoGames.CasinoGamePlaying.Roulette)
			{
				return;
			}
			if (!CasinoGames.Playing_Roulette)
			{
				CasinoGames.CurrentGame = CasinoGames.CasinoGamePlaying.None;
			}
			if (!CasinoGames.Playing_Roulette)
			{
				return;
			}
			if (CasinoGames.GetClosesRouletteTable() != null)
			{
				Prop closesRouletteTable = CasinoGames.GetClosesRouletteTable();
				if (CasinoGames.Stage == 12f || CasinoGames.Stage == 13f || CasinoGames.Stage == 14f || CasinoGames.Stage == 15f || CasinoGames.Stage == 16f || CasinoGames.Stage == 17f || CasinoGames.Stage == 18f || CasinoGames.Stage == 19f || CasinoGames.Stage == 20f || CasinoGames.Stage == 21f || CasinoGames.Stage == 22f)
				{
					if (!CasinoGames.LookingAtWheel)
					{
						Scaleform scaleform = new Scaleform("instructional_buttons");
						scaleform.CallFunction("CLEAR_ALL");
						scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
						scaleform.CallFunction("CREATE_CONTAINER");
						scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 75, 0), "Look At Wheel");
						scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
						scaleform.Render2D();
					}
					else
					{
						Scaleform scaleform2 = new Scaleform("instructional_buttons");
						scaleform2.CallFunction("CLEAR_ALL");
						scaleform2.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
						scaleform2.CallFunction("CREATE_CONTAINER");
						scaleform2.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 75, 0), "Stop Looking At Wheel");
						scaleform2.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
						scaleform2.Render2D();
					}
					if (Game.IsControlJustPressed(Control.VehicleExit))
					{
						if (!CasinoGames.LookingAtWheel)
						{
							if (CasinoGames.RoulleteCam != null)
							{
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 1, 3000, 1, 0);
								CasinoGames.RoulleteCam.IsActive = false;
								CasinoGames.RoulleteCam.Delete();
							}
							CasinoGames.RoulleteCam = GlobalHelper.CreateCamera(closesRouletteTable.GetOffsetPosition(new Vector3(-0.4f, -0.6f, 1.6f)), new Vector3(0f, 0f, 0f), 60f);
							CasinoGames.RoulleteCam.IsActive = true;
							CasinoGames.RoulleteCam.FarClip = 2000f;
							CasinoGames.RoulleteCam.PointAt(CasinoGames.ClosestDealerTable.Table.GetOffsetPosition(new Vector3(-0.738f, -0.171f, 1.13f)));
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 1, 3000, 1, 0);
							CasinoGames.LookingAtWheel = true;
						}
						else
						{
							CasinoGames.LookingAtWheel = false;
							if (CasinoGames.RoulleteCam != null)
							{
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 1, 3000, 1, 0);
								CasinoGames.RoulleteCam.IsActive = false;
								CasinoGames.RoulleteCam.Delete();
							}
						}
					}
				}
			}
			if (CasinoGames.RouletteTableA != null && CasinoGames.GetDistance_VDIST2(Game.Player.Character.Position, CasinoGames.RouletteTableA.Position, CasinoGames.GetDistSign.Less, 5f))
			{
				CasinoGames.TableT = CasinoGames.RouletteTableA;
				Prop tableT2 = CasinoGames.TableT;
				CasinoGames.RoulleteTable = "A";
			}
			if (CasinoGames.RouletteTableB != null && CasinoGames.GetDistance_VDIST2(Game.Player.Character.Position, CasinoGames.RouletteTableB.Position, CasinoGames.GetDistSign.Less, 5f))
			{
				CasinoGames.TableT = CasinoGames.RouletteTableB;
				Prop tableT3 = CasinoGames.TableT;
				CasinoGames.RoulleteTable = "B";
			}
			if (CasinoGames.RouletteTableC != null)
			{
				if (CasinoGames.GetDistance_VDIST2(Game.Player.Character.Position, CasinoGames.RouletteTableC.Position, CasinoGames.GetDistSign.Less, 5f))
				{
					CasinoGames.TableT = CasinoGames.RouletteTableC;
					Prop tableT4 = CasinoGames.TableT;
					CasinoGames.RoulleteTable = "C";
				}
			}
			else
			{
				Notification.Show("~DC&R MAJOR Error ~W~ : One of The Roulette Tables has failed to spawn, please restart your game");
			}
			if (!(CasinoGames.TableT != null))
			{
				return;
			}
			Prop tableT = CasinoGames.TableT;
			if (CasinoGames.MarkerNum < 10)
			{
				CasinoGames.ShowRouletteOptions(CasinoGames.TableT);
			}
			if (CasinoGames.MarkerNum == 11 && !CasinoGames.ReadyForRouletteEnd && CasinoGames.Stage == 22f)
			{
				CasinoGames.ShowRouletteOptionsEndGame(CasinoGames.TableT);
				if (Game.IsControlJustPressed(Control.ThrowGrenade))
				{
					CasinoGames.MarkerNum = 0;
					CasinoGames.Stage = 0f;
					CasinoGames.LookingAtWheel = false;
					if (CasinoGames.RoulleteCam != null)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 1, 3000, 1, 0);
						CasinoGames.RoulleteCam.IsActive = false;
						CasinoGames.RoulleteCam.Delete();
					}
					foreach (Prop betPosMarker in CasinoGames.BetPosMarkers)
					{
						if (betPosMarker != null)
						{
							betPosMarker.Delete();
						}
					}
					CasinoGames.BetPosMarkers.Clear();
					foreach (DealerTable rouletteDealerTable in CasinoGames.RouletteDealerTables)
					{
						if (rouletteDealerTable.Dealer != null)
						{
							CasinoGames.ResetDealerPos(rouletteDealerTable);
						}
					}
					CasinoGames.CleanUp_RouletteBetsAndMarkers();
					CasinoGames.CreatedMarker = false;
					foreach (CasinoGames.RouletteBet rouletteBet in CasinoGames.RouletteBets)
					{
						if (rouletteBet.BetProp != null)
						{
							rouletteBet.BetProp.Delete();
						}
					}
					if (CasinoGames.RouletteMarker1 != null)
					{
						CasinoGames.RouletteMarker1.Delete();
					}
					CasinoGames.RouletteBets.Clear();
					CasinoGames.Placebet2 = false;
					CasinoGames.Editbet = false;
					CasinoGames.Placebet = false;
					CasinoGames.Drawcards = false;
					CasinoGames.Endgame = false;
					CasinoGames.CreatedMarker = false;
					CasinoGames.PlaceBet_R = false;
					CasinoGames.Playercards = 0;
					CasinoGames.DealerCards = 0;
					CasinoGames.PlayerCard_1 = 0;
					CasinoGames.PlayerCard_2 = 0;
					CasinoGames.PlayerCard_3 = 0;
					CasinoGames.PlayerCard_4 = 0;
					CasinoGames.PlayerCard_5 = 0;
					CasinoGames.PlayerCard_6 = 0;
					CasinoGames.Bet = 100f;
					CasinoGames.DealerCard_1 = 0;
					CasinoGames.DealerCard_2 = 0;
					CasinoGames.DealerCard_3 = 0;
					CasinoGames.DealerCard_4 = 0;
					CasinoGames.DealerCard_5 = 0;
					CasinoGames.DealerCard_6 = 0;
					CasinoGames.BetPos = "None";
					CasinoGames.BetPosNum = 0;
					CasinoGames.CleanUp_RouletteBetsAndMarkers();
					CasinoGames.MarkerNum = 1;
					foreach (Prop chip in CasinoGames.Chips)
					{
						if (chip != null)
						{
							chip.Delete();
						}
					}
					Prop closestTable = CasinoGames.GetClosestTable(Game.Player.Character.Position, CasinoGames.RouletteDealerTables);
					CasinoGames.RoulleteCam = GlobalHelper.CreateCamera(closestTable.GetOffsetPosition(new Vector3(0.35f, 0f, 2.5f)), new Vector3(-90f, 0f, closestTable.Heading + 90f), 60f);
					CasinoGames.CreatedRouletteCam = true;
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 1, 3000, 1, 0);
					Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_DEALER_PLACE_BET", "SPEECH_PARAMS_STANDARD", false);
					CasinoGames.Playing_Roulette = true;
				}
			}
			bool flag = CasinoGames.Ball != null;
			if (CasinoGames.FakeBall != null)
			{
				CasinoGames.FakeBall.Opacity = 0;
			}
			float z = 0.95f;
			Vector3 offsetPosition = tableT.GetOffsetPosition(new Vector3(-0.14f, 0.11f, z));
			Vector3 offsetPosition2 = tableT.GetOffsetPosition(new Vector3(-0.14f, -0.14f, z));
			Vector3 offsetPosition3 = tableT.GetOffsetPosition(new Vector3(-0.05f, -0.185f, z));
			Vector3 offsetPosition4 = tableT.GetOffsetPosition(new Vector3(-0.05f, -0.025f, z));
			Vector3 offsetPosition5 = tableT.GetOffsetPosition(new Vector3(-0.05f, 0.14f, z));
			Vector3 offsetPosition6 = tableT.GetOffsetPosition(new Vector3(0.03f, -0.185f, z));
			Vector3 offsetPosition7 = tableT.GetOffsetPosition(new Vector3(0.03f, -0.025f, z));
			Vector3 offsetPosition8 = tableT.GetOffsetPosition(new Vector3(0.03f, 0.14f, z));
			Vector3 offsetPosition9 = tableT.GetOffsetPosition(new Vector3(0.1f, -0.185f, z));
			Vector3 offsetPosition10 = tableT.GetOffsetPosition(new Vector3(0.1f, -0.025f, z));
			Vector3 offsetPosition11 = tableT.GetOffsetPosition(new Vector3(0.1f, 0.14f, z));
			Vector3 offsetPosition12 = tableT.GetOffsetPosition(new Vector3(0.19f, -0.185f, z));
			Vector3 offsetPosition13 = tableT.GetOffsetPosition(new Vector3(0.19f, -0.025f, z));
			Vector3 offsetPosition14 = tableT.GetOffsetPosition(new Vector3(0.19f, 0.14f, z));
			Vector3 offsetPosition15 = tableT.GetOffsetPosition(new Vector3(0.27f, -0.185f, z));
			Vector3 offsetPosition16 = tableT.GetOffsetPosition(new Vector3(0.27f, -0.025f, z));
			Vector3 offsetPosition17 = tableT.GetOffsetPosition(new Vector3(0.27f, 0.14f, z));
			Vector3 offsetPosition18 = tableT.GetOffsetPosition(new Vector3(0.35f, -0.185f, z));
			Vector3 offsetPosition19 = tableT.GetOffsetPosition(new Vector3(0.35f, -0.025f, z));
			Vector3 offsetPosition20 = tableT.GetOffsetPosition(new Vector3(0.35f, 0.14f, z));
			Vector3 offsetPosition21 = tableT.GetOffsetPosition(new Vector3(0.43f, -0.185f, z));
			Vector3 offsetPosition22 = tableT.GetOffsetPosition(new Vector3(0.43f, -0.025f, z));
			Vector3 offsetPosition23 = tableT.GetOffsetPosition(new Vector3(0.43f, 0.14f, z));
			Vector3 offsetPosition24 = tableT.GetOffsetPosition(new Vector3(0.51f, -0.185f, z));
			Vector3 offsetPosition25 = tableT.GetOffsetPosition(new Vector3(0.51f, -0.025f, z));
			Vector3 offsetPosition26 = tableT.GetOffsetPosition(new Vector3(0.51f, 0.14f, z));
			Vector3 offsetPosition27 = tableT.GetOffsetPosition(new Vector3(0.59f, -0.185f, z));
			Vector3 offsetPosition28 = tableT.GetOffsetPosition(new Vector3(0.59f, -0.025f, z));
			Vector3 offsetPosition29 = tableT.GetOffsetPosition(new Vector3(0.59f, 0.14f, z));
			Vector3 offsetPosition30 = tableT.GetOffsetPosition(new Vector3(0.67f, -0.185f, z));
			Vector3 offsetPosition31 = tableT.GetOffsetPosition(new Vector3(0.67f, -0.025f, z));
			Vector3 offsetPosition32 = tableT.GetOffsetPosition(new Vector3(0.67f, 0.14f, z));
			Vector3 offsetPosition33 = tableT.GetOffsetPosition(new Vector3(0.75f, -0.185f, z));
			Vector3 offsetPosition34 = tableT.GetOffsetPosition(new Vector3(0.75f, -0.025f, z));
			Vector3 offsetPosition35 = tableT.GetOffsetPosition(new Vector3(0.75f, 0.14f, z));
			Vector3 offsetPosition36 = tableT.GetOffsetPosition(new Vector3(0.83f, -0.185f, z));
			Vector3 offsetPosition37 = tableT.GetOffsetPosition(new Vector3(0.83f, -0.025f, z));
			Vector3 offsetPosition38 = tableT.GetOffsetPosition(new Vector3(0.83f, 0.14f, z));
			Vector3 offsetPosition39 = tableT.GetOffsetPosition(new Vector3(0.91f, -0.185f, z));
			Vector3 offsetPosition40 = tableT.GetOffsetPosition(new Vector3(0.91f, -0.025f, z));
			Vector3 offsetPosition41 = tableT.GetOffsetPosition(new Vector3(0.91f, 0.14f, z));
			Vector3 offsetPosition42 = tableT.GetOffsetPosition(new Vector3(0.794f, -0.374f, z));
			Vector3 offsetPosition43 = tableT.GetOffsetPosition(new Vector3(0.634f, -0.374f, z));
			Vector3 offsetPosition44 = tableT.GetOffsetPosition(new Vector3(-0.02f, -0.374f, z));
			Vector3 offsetPosition45 = tableT.GetOffsetPosition(new Vector3(0.135f, -0.374f, z));
			Vector3 offsetPosition46 = tableT.GetOffsetPosition(new Vector3(0.309f, -0.374f, z));
			Vector3 offsetPosition47 = tableT.GetOffsetPosition(new Vector3(0.474f, -0.374f, z));
			Vector3 offsetPosition48 = tableT.GetOffsetPosition(new Vector3(0.0699f, -0.304f, z));
			Vector3 offsetPosition49 = tableT.GetOffsetPosition(new Vector3(0.389f, -0.304f, z));
			Vector3 offsetPosition50 = tableT.GetOffsetPosition(new Vector3(0.719f, -0.304f, z));
			Vector3 offsetPosition51 = tableT.GetOffsetPosition(new Vector3(-0.017f, -0.11f, z));
			Vector3 offsetPosition52 = tableT.GetOffsetPosition(new Vector3(-0.017f, 0.058f, z));
			Vector3 offsetPosition53 = tableT.GetOffsetPosition(new Vector3(0.066f, -0.11f, z));
			Vector3 offsetPosition54 = tableT.GetOffsetPosition(new Vector3(0.066f, 0.058f, z));
			Vector3 offsetPosition55 = tableT.GetOffsetPosition(new Vector3(0.146f, -0.11f, z));
			Vector3 offsetPosition56 = tableT.GetOffsetPosition(new Vector3(0.146f, 0.058f, z));
			Vector3 offsetPosition57 = tableT.GetOffsetPosition(new Vector3(0.227f, -0.11f, z));
			Vector3 offsetPosition58 = tableT.GetOffsetPosition(new Vector3(0.227f, 0.058f, z));
			Vector3 offsetPosition59 = tableT.GetOffsetPosition(new Vector3(0.309f, -0.11f, z));
			Vector3 offsetPosition60 = tableT.GetOffsetPosition(new Vector3(0.309f, 0.058f, z));
			Vector3 offsetPosition61 = tableT.GetOffsetPosition(new Vector3(0.391f, -0.11f, z));
			Vector3 offsetPosition62 = tableT.GetOffsetPosition(new Vector3(0.391f, 0.058f, z));
			Vector3 offsetPosition63 = tableT.GetOffsetPosition(new Vector3(0.472f, -0.11f, z));
			Vector3 offsetPosition64 = tableT.GetOffsetPosition(new Vector3(0.472f, 0.058f, z));
			Vector3 offsetPosition65 = tableT.GetOffsetPosition(new Vector3(0.553f, -0.11f, z));
			Vector3 offsetPosition66 = tableT.GetOffsetPosition(new Vector3(0.553f, 0.058f, z));
			Vector3 offsetPosition67 = tableT.GetOffsetPosition(new Vector3(0.634f, -0.11f, z));
			Vector3 offsetPosition68 = tableT.GetOffsetPosition(new Vector3(0.634f, 0.058f, z));
			Vector3 offsetPosition69 = tableT.GetOffsetPosition(new Vector3(0.715f, -0.11f, z));
			Vector3 offsetPosition70 = tableT.GetOffsetPosition(new Vector3(0.715f, 0.058f, z));
			Vector3 offsetPosition71 = tableT.GetOffsetPosition(new Vector3(0.793f, -0.11f, z));
			Vector3 offsetPosition72 = tableT.GetOffsetPosition(new Vector3(0.793f, 0.058f, z));
			Vector3 offsetPosition73 = tableT.GetOffsetPosition(new Vector3(-0.057f, -0.275f, z));
			Vector3 offsetPosition74 = tableT.GetOffsetPosition(new Vector3(-0.016f, -0.275f, z));
			Vector3 offsetPosition75 = tableT.GetOffsetPosition(new Vector3(0.025f, -0.275f, z));
			Vector3 offsetPosition76 = tableT.GetOffsetPosition(new Vector3(0.066f, -0.275f, z));
			Vector3 offsetPosition77 = tableT.GetOffsetPosition(new Vector3(0.105f, -0.275f, z));
			Vector3 offsetPosition78 = tableT.GetOffsetPosition(new Vector3(0.145f, -0.275f, z));
			Vector3 offsetPosition79 = tableT.GetOffsetPosition(new Vector3(0.187f, -0.275f, z));
			Vector3 offsetPosition80 = tableT.GetOffsetPosition(new Vector3(0.226f, -0.275f, z));
			Vector3 offsetPosition81 = tableT.GetOffsetPosition(new Vector3(0.27f, -0.275f, z));
			Vector3 offsetPosition82 = tableT.GetOffsetPosition(new Vector3(0.308f, -0.275f, z));
			Vector3 offsetPosition83 = tableT.GetOffsetPosition(new Vector3(0.348f, -0.275f, z));
			Vector3 offsetPosition84 = tableT.GetOffsetPosition(new Vector3(0.39f, -0.275f, z));
			Vector3 offsetPosition85 = tableT.GetOffsetPosition(new Vector3(0.432f, -0.275f, z));
			Vector3 offsetPosition86 = tableT.GetOffsetPosition(new Vector3(0.47f, -0.275f, z));
			Vector3 offsetPosition87 = tableT.GetOffsetPosition(new Vector3(0.512f, -0.275f, z));
			Vector3 offsetPosition88 = tableT.GetOffsetPosition(new Vector3(0.553f, -0.275f, z));
			Vector3 offsetPosition89 = tableT.GetOffsetPosition(new Vector3(0.594f, -0.275f, z));
			Vector3 offsetPosition90 = tableT.GetOffsetPosition(new Vector3(0.634f, -0.275f, z));
			Vector3 offsetPosition91 = tableT.GetOffsetPosition(new Vector3(0.676f, -0.275f, z));
			Vector3 offsetPosition92 = tableT.GetOffsetPosition(new Vector3(0.713f, -0.275f, z));
			Vector3 offsetPosition93 = tableT.GetOffsetPosition(new Vector3(0.755f, -0.275f, z));
			Vector3 offsetPosition94 = tableT.GetOffsetPosition(new Vector3(0.793f, -0.275f, z));
			Vector3 offsetPosition95 = tableT.GetOffsetPosition(new Vector3(0.836f, -0.275f, z));
			Vector3 offsetPosition96 = tableT.GetOffsetPosition(new Vector3(-0.1042f, -0.02f, z));
			Vector3 offsetPosition97 = tableT.GetOffsetPosition(new Vector3(-0.057f, -0.1073f, z));
			Vector3 offsetPosition98 = tableT.GetOffsetPosition(new Vector3(-0.057f, 0.0587f, z));
			Vector3 offsetPosition99 = tableT.GetOffsetPosition(new Vector3(0.025f, -0.1073f, z));
			Vector3 offsetPosition100 = tableT.GetOffsetPosition(new Vector3(0.025f, 0.0587f, z));
			Vector3 offsetPosition101 = tableT.GetOffsetPosition(new Vector3(0.105f, -0.1073f, z));
			Vector3 offsetPosition102 = tableT.GetOffsetPosition(new Vector3(0.105f, 0.0587f, z));
			Vector3 offsetPosition103 = tableT.GetOffsetPosition(new Vector3(0.1855f, -0.1073f, z));
			Vector3 offsetPosition104 = tableT.GetOffsetPosition(new Vector3(0.1855f, 0.0587f, z));
			Vector3 offsetPosition105 = tableT.GetOffsetPosition(new Vector3(0.27f, -0.1073f, z));
			Vector3 offsetPosition106 = tableT.GetOffsetPosition(new Vector3(0.27f, 0.0587f, z));
			Vector3 offsetPosition107 = tableT.GetOffsetPosition(new Vector3(0.348f, -0.1073f, z));
			Vector3 offsetPosition108 = tableT.GetOffsetPosition(new Vector3(0.348f, 0.0587f, z));
			Vector3 offsetPosition109 = tableT.GetOffsetPosition(new Vector3(0.432f, -0.1073f, z));
			Vector3 offsetPosition110 = tableT.GetOffsetPosition(new Vector3(0.432f, 0.0587f, z));
			Vector3 offsetPosition111 = tableT.GetOffsetPosition(new Vector3(0.5155f, -0.1073f, z));
			Vector3 offsetPosition112 = tableT.GetOffsetPosition(new Vector3(0.5155f, 0.0587f, z));
			Vector3 offsetPosition113 = tableT.GetOffsetPosition(new Vector3(0.594f, -0.1073f, z));
			Vector3 offsetPosition114 = tableT.GetOffsetPosition(new Vector3(0.594f, 0.0587f, z));
			Vector3 offsetPosition115 = tableT.GetOffsetPosition(new Vector3(0.676f, -0.1073f, z));
			Vector3 offsetPosition116 = tableT.GetOffsetPosition(new Vector3(0.676f, 0.0587f, z));
			Vector3 offsetPosition117 = tableT.GetOffsetPosition(new Vector3(0.755f, -0.1073f, z));
			Vector3 offsetPosition118 = tableT.GetOffsetPosition(new Vector3(0.755f, 0.0587f, z));
			Vector3 offsetPosition119 = tableT.GetOffsetPosition(new Vector3(0.836f, -0.1073f, z));
			Vector3 offsetPosition120 = tableT.GetOffsetPosition(new Vector3(0.836f, 0.0587f, z));
			Vector3 offsetPosition121 = tableT.GetOffsetPosition(new Vector3(-0.14f, -0.3f, z));
			if (!CasinoGames.CreatedMarker)
			{
				if (CasinoGames.Ball != null)
				{
					CasinoGames.Ball.Delete();
				}
				Vector3 offsetPosition122 = tableT.GetOffsetPosition(new Vector3(-1f, -0.36f, 1.06f));
				if (CasinoGames.RoulleteTable.Equals("A"))
				{
					offsetPosition122 = tableT.GetOffsetPosition(new Vector3(-0.65f, -0.45f, 1.06f));
				}
				if (CasinoGames.RoulleteTable.Equals("B"))
				{
					offsetPosition122 = tableT.GetOffsetPosition(new Vector3(-0.42f, -0.1f, 1.06f));
				}
				if (CasinoGames.RoulleteTable.Equals("C"))
				{
					offsetPosition122 = tableT.GetOffsetPosition(new Vector3(-1f, -0.36f, 1.06f));
				}
				if (CasinoGames.FakeBall != null)
				{
					CasinoGames.FakeBall.Delete();
				}
				if (CasinoGames.Ball != null)
				{
					CasinoGames.Ball.Delete();
				}
				CasinoGames.Ball = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_roulette_ball"), offsetPosition122, new Vector3(0f, 0f, 0f), false, false);
				CasinoGames.Ball.IsPositionFrozen = true;
				CasinoGames.Ball.IsCollisionEnabled = false;
				CasinoGames.SetBallToStartTransform(CasinoGames.Ball, tableT);
				CasinoGames.CreatedMarker = true;
				if (CasinoGames.RouletteMarker1 != null)
				{
					CasinoGames.RouletteMarker1.Delete();
				}
				CasinoGames.RouletteMarker1 = GlobalHelper.CreateProp(CasinoGames.RequestModel(CasinoGames.GetCurrentRouletteBetChipModel()), offsetPosition121, true, false);
				CasinoGames.RouletteMarker1.IsPositionFrozen = false;
			}
			if (CasinoGames.MarkerNum == 11)
			{
				if (CasinoGames.Stage == 9f)
				{
					CasinoGames.InitialWheelRotation = CasinoGames.ROULETTE_PROPS_GET_WHEEL_CURRENT_ROTATION(CasinoGames.GetClosesRouletteTable()).Z;
					string dict = "anim_casino_b@amb@casino@games@roulette@dealer";
					string text = "no_more_bets";
					CasinoGames.LoadDict(dict);
					Vector3 dealerAnimOrigin_Roulette = CasinoGames.GetDealerAnimOrigin_Roulette();
					CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin_Roulette.X, dealerAnimOrigin_Roulette.Y, dealerAnimOrigin_Roulette.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict), text, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text, CasinoGames.LoadDict(dict), 16f, -16f, 0, 1148846080);
					Script.Wait(1);
					CasinoGames.Stage = 10f;
					Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_DEALER_CLOSED_BETS", "SPEECH_PARAMS_STANDARD", false);
				}
				if (CasinoGames.Stage == 10f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216)
				{
					string dict2 = "anim_casino_b@amb@casino@games@roulette@dealer";
					string text2 = "spin_wheel";
					CasinoGames.LoadDict(dict2);
					Vector3 dealerAnimOrigin_Roulette2 = CasinoGames.GetDealerAnimOrigin_Roulette();
					CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin_Roulette2.X, dealerAnimOrigin_Roulette2.Y, dealerAnimOrigin_Roulette2.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict2), text2, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text2, CasinoGames.LoadDict(dict2), 16f, -16f, 0, 1148846080);
					Script.Wait(1);
					Vector3 offsetPosition123 = tableT.GetOffsetPosition(new Vector3(-1f, -0.36f, 1.06f));
					offsetPosition123 = tableT.GetOffsetPosition(new Vector3(-1f, -0.36f, 1.06f));
					dict2 = "anim_casino_b@amb@casino@games@roulette@table";
					text2 = "intro_wheel";
					CasinoGames.LoadDict(dict2);
					dealerAnimOrigin_Roulette2 = CasinoGames.GetDealerAnimOrigin();
					float initialWheelRotation = CasinoGames.InitialWheelRotation;
					CasinoGames.Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin_Roulette2.X, dealerAnimOrigin_Roulette2.Y, dealerAnimOrigin_Roulette2.Z, 0.0, 0.0, tableT.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, tableT, CasinoGames.Scene3, CasinoGames.LoadDict(dict2), text2, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, tableT, CasinoGames.Scene3, text2, CasinoGames.LoadDict(dict2), 16f, -16f, 0, 1148846080);
					if (CasinoGames.Ball != null)
					{
						CasinoGames.Ball.Delete();
					}
					CasinoGames.Ball = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_roulette_ball"), offsetPosition123, new Vector3(0f, 0f, 0f), false, false);
					CasinoGames.Ball.IsPositionFrozen = true;
					CasinoGames.Ball.IsCollisionEnabled = false;
					CasinoGames.SetBallToStartTransform(CasinoGames.Ball, tableT);
					if (CasinoGames.FakeBall != null)
					{
						CasinoGames.FakeBall.Delete();
					}
					CasinoGames.FakeBall = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_roulette_ball"), offsetPosition123, new Vector3(0f, 0f, 0f), false, false);
					CasinoGames.FakeBall.IsPositionFrozen = true;
					CasinoGames.FakeBall.IsCollisionEnabled = false;
					CasinoGames.SetBallToStartTransform(CasinoGames.FakeBall, tableT);
					initialWheelRotation = CasinoGames.InitialWheelRotation;
					dealerAnimOrigin_Roulette2 = CasinoGames.GetDealerAnimOrigin_Roulette_Wheel();
					CasinoGames.Scene4 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin_Roulette2.X, dealerAnimOrigin_Roulette2.Y, dealerAnimOrigin_Roulette2.Z, 0.0, 0.0, initialWheelRotation, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.Ball, CasinoGames.Scene4, CasinoGames.LoadDict(dict2), "intro_ball", 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.Ball, CasinoGames.Scene4, "intro_ball", CasinoGames.LoadDict(dict2), 16f, -16f, 0, 1148846080);
					CasinoGames.RouletteSpinningSound = Function.Call<int>(Hash.GET_SOUND_ID);
					Script.Wait(1);
					CasinoGames.Stage = 12f;
					CasinoGames.BallLoopTime = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 2500, 12500);
				}
				if (CasinoGames.Stage == 12f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene3) >= 1065353216)
				{
					string dict3 = "anim_casino_b@amb@casino@games@roulette@table";
					string text3 = "loop_wheel";
					CasinoGames.LoadDict(dict3);
					Vector3 dealerAnimOrigin = CasinoGames.GetDealerAnimOrigin();
					CasinoGames.Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin.X, dealerAnimOrigin.Y, dealerAnimOrigin.Z, 0.0, 0.0, tableT.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, tableT, CasinoGames.Scene3, CasinoGames.LoadDict(dict3), text3, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, tableT, CasinoGames.Scene3, text3, CasinoGames.LoadDict(dict3), 16f, -16f, 0, 1148846080);
					CasinoGames.SetBallToStartTransform(CasinoGames.Ball, tableT);
					float initialWheelRotation2 = CasinoGames.InitialWheelRotation;
					dealerAnimOrigin = CasinoGames.GetDealerAnimOrigin_Roulette_Wheel();
					CasinoGames.Scene4 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin.X, dealerAnimOrigin.Y, dealerAnimOrigin.Z, 0.0, 0.0, initialWheelRotation2, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.Ball, CasinoGames.Scene4, CasinoGames.LoadDict(dict3), "loop_ball", 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.Ball, CasinoGames.Scene4, "loop_ball", CasinoGames.LoadDict(dict3), 16f, -16f, 0, 1148846080);
					CasinoGames.Ball_Outro_Anim_Index = CasinoGames.ROULETTE_GET_ANIMATION_INDEX_FOR_ROULETTE_NUMBER(CasinoGames.Roulette_Choose_Num(tableT));
					CasinoGames.RouletteSpinningSound = Function.Call<int>(Hash.GET_SOUND_ID);
					Script.Wait(1);
					CasinoGames.Stage = 13f;
					dict3 = "anim_casino_b@amb@casino@games@roulette@table";
					text3 = "exit_" + CasinoGames.Ball_Outro_Anim_Index + "_wheel";
					string text4 = "exit_" + CasinoGames.Ball_Outro_Anim_Index + "_ball";
					CasinoGames.LoadDict(dict3);
					CasinoGames.SetBallToStartTransform(CasinoGames.FakeBall, tableT);
					initialWheelRotation2 = CasinoGames.InitialWheelRotation;
					dealerAnimOrigin = CasinoGames.GetDealerAnimOrigin_Roulette_Wheel();
					int num = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin.X, dealerAnimOrigin.Y, dealerAnimOrigin.Z, 0.0, 0.0, tableT.Rotation.Z - 90f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.FakeBall, num, CasinoGames.LoadDict(dict3), text4, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.FakeBall, num, text4, CasinoGames.LoadDict(dict3), 16f, -16f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, num, 0f);
				}
				if (CasinoGames.Stage == 13f)
				{
					if ((Game.GameTime <= CasinoGames.BallLoopTime || CasinoGames.Ball.Position.DistanceTo(CasinoGames.FakeBall.Position) > 0.1f) && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene3) >= 1065353216)
					{
						string dict4 = "anim_casino_b@amb@casino@games@roulette@table";
						string text5 = "loop_wheel";
						CasinoGames.LoadDict(dict4);
						Vector3 dealerAnimOrigin2 = CasinoGames.GetDealerAnimOrigin();
						CasinoGames.Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin2.X, dealerAnimOrigin2.Y, dealerAnimOrigin2.Z, 0.0, 0.0, tableT.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, tableT, CasinoGames.Scene3, CasinoGames.LoadDict(dict4), text5, 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, tableT, CasinoGames.Scene3, text5, CasinoGames.LoadDict(dict4), 16f, -16f, 0, 1148846080);
						dict4 = "anim_casino_b@amb@casino@games@roulette@table";
						CasinoGames.LoadDict(dict4);
						CasinoGames.SetBallToStartTransform(CasinoGames.Ball, tableT);
						float initialWheelRotation3 = CasinoGames.InitialWheelRotation;
						dealerAnimOrigin2 = CasinoGames.GetDealerAnimOrigin_Roulette_Wheel();
						CasinoGames.Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin2.X, dealerAnimOrigin2.Y, dealerAnimOrigin2.Z, 0.0, 0.0, initialWheelRotation3, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.Ball, CasinoGames.Scene3, CasinoGames.LoadDict(dict4), "loop_ball", 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.Ball, CasinoGames.Scene3, "loop_ball", CasinoGames.LoadDict(dict4), 16f, -16f, 0, 1148846080);
					}
					if (Game.GameTime >= CasinoGames.BallLoopTime && CasinoGames.Ball.Position.DistanceTo(CasinoGames.FakeBall.Position) < 0.1f)
					{
						CasinoGames.RouletteSpinningSound = Function.Call<int>(Hash.GET_SOUND_ID);
						string dict5 = "anim_casino_b@amb@casino@games@roulette@table";
						string text6 = "exit_" + CasinoGames.Ball_Outro_Anim_Index + "_wheel";
						string text7 = "exit_" + CasinoGames.Ball_Outro_Anim_Index + "_ball";
						CasinoGames.LoadDict(dict5);
						Vector3 dealerAnimOrigin3 = CasinoGames.GetDealerAnimOrigin();
						CasinoGames.Scene3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin3.X, dealerAnimOrigin3.Y, dealerAnimOrigin3.Z, 0.0, 0.0, tableT.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, tableT, CasinoGames.Scene3, CasinoGames.LoadDict(dict5), text6, 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, tableT, CasinoGames.Scene3, text6, CasinoGames.LoadDict(dict5), 16f, -16f, 0, 1148846080);
						CasinoGames.SetBallToStartTransform(CasinoGames.Ball, tableT);
						float initialWheelRotation4 = CasinoGames.InitialWheelRotation;
						dealerAnimOrigin3 = CasinoGames.GetDealerAnimOrigin_Roulette_Wheel();
						CasinoGames.Scene4 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin3.X, dealerAnimOrigin3.Y, dealerAnimOrigin3.Z, 0.0, 0.0, tableT.Rotation.Z - 90f, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.Ball, CasinoGames.Scene4, CasinoGames.LoadDict(dict5), text7, 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.Ball, CasinoGames.Scene4, text7, CasinoGames.LoadDict(dict5), 16f, -16f, 0, 1148846080);
						int b_roulette = CasinoGames.ROULETTE_GET_BALL_LAND_INDEX_FROM_ANIM_INDEX(CasinoGames.Ball_Outro_Anim_Index);
						if (Game.Player.Name.ToUpper().Equals("CINNBARGNU"))
						{
							Notification.Show("Exit Anim " + CasinoGames.Ball_Outro_Anim_Index + "/" + b_roulette);
						}
						CasinoGames.B_roulette = b_roulette;
						Script.Wait(1);
						CasinoGames.Stage = 14f;
					}
				}
				if (CasinoGames.Stage == 14f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene3) >= 1065353216)
				{
					CasinoGames.ReadyForRouletteEnd = true;
					CasinoGames.Stage = 15f;
				}
				if (CasinoGames.ReadyForRouletteEnd && CasinoGames.Stage == 15f)
				{
					Function.Call(Hash.STOP_SOUND, CasinoGames.RouletteSpinningSound);
					Function.Call(Hash.RELEASE_SOUND_ID, CasinoGames.RouletteSpinningSound);
					CasinoGames.ReadyForRouletteEnd = false;
					string text8 = "";
					if (CasinoGames.B_roulette == 38)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_0", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 37)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_00", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 1)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_1", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 2)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_2", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 3)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_3", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 4)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_4", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 5)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_5", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 6)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_6", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 7)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_7", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 8)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_8", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 9)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_9", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 10)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_10", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 11)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_11", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 12)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_12", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 13)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_13", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 14)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_14", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 15)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_15", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 16)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_16", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 17)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_17", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 18)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_18", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 19)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_19", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 20)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_20", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 21)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_21", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 22)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_22", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 23)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_23", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 24)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_24", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 25)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_25", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 26)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_26", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 27)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_27", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 28)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_28", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 29)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_29", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 30)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_30", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 31)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_31", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 32)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_32", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 33)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_33", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 34)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_34", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 35)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_35", "SPEECH_PARAMS_STANDARD", false);
					}
					if (CasinoGames.B_roulette == 36)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_ROULETTE_BALL_36", "SPEECH_PARAMS_STANDARD", false);
					}
					text8 = CasinoGames.ReturnRouletteNumString(CasinoGames.B_roulette);
					int num2 = 0;
					int num3 = 0;
					int num4 = 0;
					int num5 = 0;
					int num6 = 0;
					int num7 = 0;
					for (int i = 0; i < CasinoGames.RouletteBets.Count; i++)
					{
						int index = i;
						string betName = CasinoGames.RouletteBets[index].BetName;
						int betAmount = CasinoGames.RouletteBets[index].BetAmount;
						int betPosition = CasinoGames.RouletteBets[index].BetPosition;
						int num8 = 0;
						if (CasinoGames.GetDidNumberWin(betName, betAmount, text8, CasinoGames.B_roulette))
						{
							num8 = betAmount;
							CasinoGames.RouletteBetPosition rouletteBetPosition = CasinoGames.GetRouletteBetPosition(betPosition, tableT);
							num8 *= CasinoGames.GetBetMultiplierAtPosition(rouletteBetPosition);
							num2++;
						}
						else
						{
							num6 += betAmount;
							num3++;
						}
						num5 += num8;
						num7 += CasinoGames.AddBetToWinnings(betName, betAmount, text8, CasinoGames.B_roulette).Item2;
						num4++;
					}
					int num9 = num4 - num3;
					Notification.Show("~t~Ball Landed on ~w~ " + text8);
					Notification.Show("~b~Total Bet~w~ " + num7.ToString("N"));
					int num10 = num7 - num6;
					if (num10 < -1)
					{
						num10 = num7 - num6;
					}
					Notification.Show("~g~Winnings~w~ " + num10.ToString("N"));
					Notification.Show("~r~Loss~w~ " + num6.ToString("N"));
					CasinoGames.Chips_Amount -= num6;
					Class1.Config.SetValue("Setup", "Chips_Amount", CasinoGames.Chips_Amount);
					Class1.Config.Save();
					CasinoGames.Chips_Amount += num10;
					Class1.Config.SetValue("Setup", "Chips_Amount", CasinoGames.Chips_Amount);
					Class1.Config.Save();
					CasinoGames.CreatedRouletteCam = false;
					string text9 = "terrible";
					if (num9 <= 0)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_DEALER_LEAVE_BAD_GAME", "SPEECH_PARAMS_STANDARD", false);
						text9 = "terrible";
					}
					if (num9 == 1)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_DEALER_LEAVE_BAD_GAME", "SPEECH_PARAMS_STANDARD", false);
						text9 = "bad";
					}
					if (num9 == 2)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_DEALER_LEAVE_NEUTRAL_GAME", "SPEECH_PARAMS_STANDARD", false);
						text9 = "impartial";
					}
					if (num9 == 3)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_DEALER_LEAVE_NEUTRAL_GAME", "SPEECH_PARAMS_STANDARD", false);
						text9 = "good";
					}
					if (num9 >= 4)
					{
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_DEALER_LEAVE_GOOD_GAME", "SPEECH_PARAMS_STANDARD", false);
						text9 = "great";
					}
					Random random = new Random();
					List<string> list = new List<string> { "1", "2", "3", "4" };
					string dict6 = "anim_casino_b@amb@casino@games@shared@player@";
					string text10 = "reaction_" + text9 + "_var_0" + list[random.Next(0, list.Count)];
					CasinoGames.LoadDict(dict6);
					CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene2, CasinoGames.LoadDict(dict6), text10, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene2, text10, CasinoGames.LoadDict(dict6), 16f, -16f, 0, 1148846080);
					Script.Wait(1);
					CasinoGames.Stage = 16f;
					if (CasinoGames.RoulleteCam != null)
					{
						Prop closestTable2 = CasinoGames.GetClosestTable(Game.Player.Character.Position, CasinoGames.RouletteDealerTables);
						CasinoGames.RoulleteCam = GlobalHelper.CreateCamera(closestTable2.GetOffsetPosition(new Vector3(0.35f, 0f, 2.5f)), new Vector3(-90f, 0f, closestTable2.Heading + 90f), 60f);
						CasinoGames.CreatedRouletteCam = true;
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 1, 3000, 1, 0);
						Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_DEALER_PLACE_BET", "SPEECH_PARAMS_STANDARD", false);
					}
				}
			}
			if (CasinoGames.Stage == 16f)
			{
				string dict7 = "anim_casino_b@amb@casino@games@roulette@dealer";
				string text11 = "clear_chips_intro";
				Vector3 dealerAnimOrigin_Roulette3 = CasinoGames.GetDealerAnimOrigin_Roulette();
				CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin_Roulette3.X, dealerAnimOrigin_Roulette3.Y, dealerAnimOrigin_Roulette3.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict7), text11, 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text11, CasinoGames.LoadDict(dict7), 16f, -16f, 0, 1148846080);
				Script.Wait(1);
				CasinoGames.Stage = 17f;
				Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_DEALER_CLOSED_BETS", "SPEECH_PARAMS_STANDARD", false);
			}
			if (CasinoGames.Stage == 17f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216)
			{
				string dict8 = "anim_casino_b@amb@casino@games@roulette@dealer";
				string text12 = "clear_chips_zone3";
				Vector3 dealerAnimOrigin_Roulette4 = CasinoGames.GetDealerAnimOrigin_Roulette();
				CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin_Roulette4.X, dealerAnimOrigin_Roulette4.Y, dealerAnimOrigin_Roulette4.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict8), text12, 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text12, CasinoGames.LoadDict(dict8), 16f, -16f, 0, 1148846080);
				Script.Wait(1);
				CasinoGames.Stage = 18f;
			}
			if (CasinoGames.Stage == 18f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216)
			{
				foreach (Prop chip2 in CasinoGames.Chips)
				{
					if (chip2 != null && chip2.Position.DistanceTo(CasinoGames.ClosestDealer.Bones[Bone.PHLeftHand].Position) < 1.25f)
					{
						EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(chip2, false, 0));
					}
				}
				string dict9 = "anim_casino_b@amb@casino@games@roulette@dealer";
				string text13 = "clear_chips_zone2";
				Vector3 dealerAnimOrigin_Roulette5 = CasinoGames.GetDealerAnimOrigin_Roulette();
				CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin_Roulette5.X, dealerAnimOrigin_Roulette5.Y, dealerAnimOrigin_Roulette5.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict9), text13, 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text13, CasinoGames.LoadDict(dict9), 16f, -16f, 0, 1148846080);
				Script.Wait(1);
				CasinoGames.Stage = 19f;
			}
			if (CasinoGames.Stage == 19f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216)
			{
				foreach (Prop chip3 in CasinoGames.Chips)
				{
					if (chip3 != null && chip3.Position.DistanceTo(CasinoGames.ClosestDealer.Bones[Bone.PHLeftHand].Position) < 1.25f)
					{
						EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(chip3, false, 0));
					}
				}
				string dict10 = "anim_casino_b@amb@casino@games@roulette@dealer";
				string text14 = "clear_chips_zone1";
				Vector3 dealerAnimOrigin_Roulette6 = CasinoGames.GetDealerAnimOrigin_Roulette();
				CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin_Roulette6.X, dealerAnimOrigin_Roulette6.Y, dealerAnimOrigin_Roulette6.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict10), text14, 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text14, CasinoGames.LoadDict(dict10), 16f, -16f, 0, 1148846080);
				Script.Wait(1);
				CasinoGames.Stage = 20f;
			}
			if (CasinoGames.Stage == 20f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216)
			{
				foreach (Prop chip4 in CasinoGames.Chips)
				{
					if (chip4 != null && chip4.Position.DistanceTo(CasinoGames.ClosestDealer.Bones[Bone.PHLeftHand].Position) < 1.25f)
					{
						EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(chip4, false, 0));
					}
				}
				string dict11 = "anim_casino_b@amb@casino@games@roulette@dealer";
				string text15 = "clear_chips_outro";
				Vector3 dealerAnimOrigin_Roulette7 = CasinoGames.GetDealerAnimOrigin_Roulette();
				CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin_Roulette7.X, dealerAnimOrigin_Roulette7.Y, dealerAnimOrigin_Roulette7.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict11), text15, 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text15, CasinoGames.LoadDict(dict11), 16f, -16f, 0, 1148846080);
				Script.Wait(1);
				CasinoGames.Stage = 21f;
			}
			if (CasinoGames.Stage == 21f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216)
			{
				foreach (Prop chip5 in CasinoGames.Chips)
				{
					if (chip5 != null)
					{
						EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(chip5, false, 0));
					}
				}
				string dict12 = "anim_casino_b@amb@casino@games@roulette@dealer";
				string text16 = "idle";
				Vector3 dealerAnimOrigin_Roulette8 = CasinoGames.GetDealerAnimOrigin_Roulette();
				CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin_Roulette8.X, dealerAnimOrigin_Roulette8.Y, dealerAnimOrigin_Roulette8.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict12), text16, 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text16, CasinoGames.LoadDict(dict12), 16f, -16f, 0, 1148846080);
				Script.Wait(1);
				CasinoGames.Stage = 22f;
			}
			if (CasinoGames.Stage == 22f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene2) >= 1065353216)
			{
				string dict13 = "anim_casino_b@amb@casino@games@roulette@dealer";
				string text17 = "idle";
				Vector3 dealerAnimOrigin_Roulette9 = CasinoGames.GetDealerAnimOrigin_Roulette();
				CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin_Roulette9.X, dealerAnimOrigin_Roulette9.Y, dealerAnimOrigin_Roulette9.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict13), text17, 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text17, CasinoGames.LoadDict(dict13), 16f, -16f, 0, 1148846080);
			}
			if (CasinoGames.MarkerNum != 0 && (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene4) >= 1065353216 || Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene4) == 0))
			{
				string text18 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@engaged@01b@idles";
				string text19 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@engaged@01a@idles";
				Random random2 = new Random();
				int num11 = random2.Next(100, 400);
				int num12 = random2.Next(100, 400);
				string text20 = "";
				if (num11 < 100)
				{
					text20 = "a";
				}
				if (num11 >= 100 && num11 < 200)
				{
					text20 = "b";
				}
				if (num11 >= 200 && num11 < 300)
				{
					text20 = "c";
				}
				if (num11 >= 300 && num11 < 400)
				{
					text20 = "d";
				}
				string dict14 = "";
				if (num12 >= 0 && num12 < 200)
				{
					dict14 = text18;
				}
				if (num12 >= 200 && num12 < 400)
				{
					dict14 = text19;
				}
				CasinoGames.LoadDict(dict14);
				string text21 = "idle_" + text20.ToString();
				CasinoGames.GetPlayerAnimOriginH(CasinoGames.SeatSittingIn, CasinoGames.TableType.Roulette);
				CasinoGames.GetPlayerAnimOriginH(CasinoGames.SeatSittingIn, CasinoGames.TableType.Roulette);
				CasinoGames.Scene4 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading - 90f, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene4, CasinoGames.LoadDict(dict14), text21, 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene4, text21, CasinoGames.LoadDict(dict14), 16f, -16f, 0, 1148846080);
			}
			if (CasinoGames.CreatedRouletteCam && CasinoGames.MarkerNum == 0)
			{
				CasinoGames.MarkerNum = 1;
			}
			if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.RouletteStartScene) >= 1055353216 && !CasinoGames.CreatedRouletteCam)
			{
				CasinoGames.LookingAtWheel = false;
				if (CasinoGames.RoulleteCam != null)
				{
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 1, 3000, 1, 0);
					CasinoGames.RoulleteCam.IsActive = false;
					CasinoGames.RoulleteCam.Delete();
				}
				foreach (Prop betPosMarker2 in CasinoGames.BetPosMarkers)
				{
					if (betPosMarker2 != null)
					{
						betPosMarker2.Delete();
					}
				}
				CasinoGames.BetPosMarkers.Clear();
				CasinoGames.MarkerNum = 1;
				CasinoGames.ControllerMousePos = new Vector2(0.5f, 0.5f);
				Prop closestTable3 = CasinoGames.GetClosestTable(Game.Player.Character.Position, CasinoGames.RouletteDealerTables);
				CasinoGames.RoulleteCam = GlobalHelper.CreateCamera(closestTable3.GetOffsetPosition(new Vector3(0.35f, 0f, 2.5f)), new Vector3(-90f, 0f, closestTable3.Heading + 90f), 60f);
				CasinoGames.CreatedRouletteCam = true;
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 1, 3000, 1, 0);
				Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_DEALER_PLACE_BET", "SPEECH_PARAMS_STANDARD", false);
			}
			if (!CasinoGames.CreatedRouletteCam)
			{
				return;
			}
			if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.RouletteStartScene) >= 1065353216)
			{
				string dict15 = "anim_casino_b@amb@casino@games@shared@player@";
				CasinoGames.LoadDict("anim_casino_b@amb@casino@games@shared@player@");
				CasinoGames.LoadDict(dict15);
				List<string> list2 = new List<string>();
				string text22 = "";
				Random random3 = new Random();
				list2.Add("idle_var_01");
				list2.Add("idle_var_02");
				list2.Add("idle_var_03");
				list2.Add("idle_var_04");
				list2.Add("idle_var_05");
				list2.Add("idle_var_06");
				list2.Add("idle_var_07");
				list2.Add("idle_var_08");
				list2.Add("idle_var_09");
				list2.Add("idle_var_10");
				list2.Add("idle_var_11");
				list2.Add("idle_var_12");
				list2.Add("idle_var_13");
				text22 = list2[random3.Next(0, list2.Count)];
				Prop closestTable4 = CasinoGames.GetClosestTable(Game.Player.Character.Position, CasinoGames.RouletteDealerTables);
				CasinoGames.RouletteStartScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, closestTable4.Position.Z, 0.0, 0.0, closestTable4.Heading - 90f, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.RouletteStartScene, CasinoGames.LoadDict(dict15), text22, 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.RouletteStartScene, text22, CasinoGames.LoadDict(dict15), 16f, -16f, 0, 1148846080);
			}
			if (CasinoGames.MarkerNum >= 1 && (Game.IsControlJustPressed(Control.VehicleHandbrake) || CasinoGames.MarkerNum == 10))
			{
				if (CasinoGames.RouletteMarker1 != null)
				{
					CasinoGames.RouletteMarker1.Delete();
				}
				CasinoGames.LookingAtWheel = false;
				if (CasinoGames.RoulleteCam != null)
				{
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 1, 3000, 1, 0);
					CasinoGames.RoulleteCam.IsActive = false;
					CasinoGames.RoulleteCam.Delete();
				}
				CasinoGames.MarkerNum = 11;
				if (!CasinoGames.ReadyForRouletteEnd)
				{
					CasinoGames.ShowRouletteOptionsEnd(CasinoGames.TableT);
				}
				foreach (Prop betPosMarker3 in CasinoGames.BetPosMarkers)
				{
					if (betPosMarker3 != null)
					{
						betPosMarker3.Delete();
					}
				}
				CasinoGames.BetPosMarkers.Clear();
				CasinoGames.Stage = 9f;
				string dict16 = "anim_casino_b@amb@casino@games@blackjack@dealer";
				string text23 = "idle";
				Vector3 dealerAnimOrigin_Roulette10 = CasinoGames.GetDealerAnimOrigin_Roulette();
				CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin_Roulette10.X, dealerAnimOrigin_Roulette10.Y, dealerAnimOrigin_Roulette10.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict16), text23, 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text23, CasinoGames.LoadDict(dict16), 16f, -16f, 0, 1148846080);
				CasinoGames.CreatedRouletteCam = false;
				if (CasinoGames.RoulleteCam != null)
				{
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 1, 3000, 1, 0);
					CasinoGames.RoulleteCam.IsActive = false;
					CasinoGames.RoulleteCam.Delete();
				}
				int count = CasinoGames.RouletteBets.Count;
				for (int j = 0; j < count; j++)
				{
					string betName2 = CasinoGames.RouletteBets[j].BetName;
					int betAmount2 = CasinoGames.RouletteBets[j].BetAmount;
					int betPosition2 = CasinoGames.RouletteBets[j].BetPosition;
					Vector3 betVector3Po = CasinoGames.RouletteBets[j].BetVector3Pos;
					int iBetAmount = CasinoGames.ReturnClosestBetAmtFromCustomBet(betAmount2);
					CasinoGames.GetBetChipModel(iBetAmount, false);
					CasinoGames.GetPlayerBetPositionZHeightOffset(iBetAmount, 0, CasinoGames.SeatSittingIn, false);
				}
			}
			if (CasinoGames.MarkerNum >= 10)
			{
				return;
			}
			if (CasinoGames.CreatedRouletteCam)
			{
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.Scene1) >= 1065353216)
				{
					string dict17 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@regular@02a@idles";
					Random random4 = new Random();
					List<string> list3 = new List<string> { "idle_a", "idle_b", "idle_c", "idle_d" };
					string text24 = list3[random4.Next(0, list3.Count)];
					CasinoGames.LoadDict(dict17);
					CasinoGames.Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CasinoGames.PlayerOriginPosition.X, CasinoGames.PlayerOriginPosition.Y, CasinoGames.PlayerOriginPosition.Z - 1f, 0.0, 0.0, CasinoGames.PlayerOriginHeading, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene1, CasinoGames.LoadDict(dict17), text24, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene1, text24, CasinoGames.LoadDict(dict17), 16f, -16f, 0, 1148846080);
					string dict18 = "anim_casino_b@amb@casino@games@blackjack@dealer";
					string text25 = "idle";
					Vector3 dealerAnimOrigin_Roulette11 = CasinoGames.GetDealerAnimOrigin_Roulette();
					CasinoGames.Scene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, dealerAnimOrigin_Roulette11.X, dealerAnimOrigin_Roulette11.Y, dealerAnimOrigin_Roulette11.Z, 0.0, 0.0, CasinoGames.ClosestDealer.Rotation.Z - 180f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.ClosestDealer, CasinoGames.Scene2, CasinoGames.LoadDict(dict18), text25, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.ClosestDealer, CasinoGames.Scene2, text25, CasinoGames.LoadDict(dict18), 16f, -16f, 0, 1148846080);
				}
				Function.Call(Hash.SET_MOUSE_CURSOR_THIS_FRAME);
				Game.EnableControlThisFrame(Control.CursorX);
				Game.EnableControlThisFrame(Control.CursorY);
				while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mp_freemode_mc"))
				{
					Script.Wait(10);
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mp_freemode_mc", 0);
				}
				Game.GetControlValueNormalized(Control.CursorX);
				Game.GetControlValueNormalized(Control.CursorY);
				float y = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
				float x = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
				if (Game.LastInputMethod == InputMethod.GamePad)
				{
					if (Game.IsControlPressed(Control.VehicleMoveUpOnly))
					{
						CasinoGames.ControllerMousePos -= new Vector2(0f, 0.005f);
					}
					if (Game.IsControlPressed(Control.VehicleMoveDownOnly))
					{
						CasinoGames.ControllerMousePos += new Vector2(0f, 0.005f);
					}
					if (Game.IsControlPressed(Control.VehicleMoveLeftOnly))
					{
						CasinoGames.ControllerMousePos -= new Vector2(0.005f, 0f);
					}
					if (Game.IsControlPressed(Control.VehicleMoveRightOnly))
					{
						CasinoGames.ControllerMousePos += new Vector2(0.005f, 0f);
					}
					Function.Call(Hash.SET_CURSOR_POSITION, CasinoGames.ControllerMousePos.X, CasinoGames.ControllerMousePos.Y);
				}
				CasinoGames.World3DToScreen2d(offsetPosition29);
				CasinoGames.MarkerTomove = CasinoGames.RouletteMarker1;
				if (CasinoGames.BetPosNum == 0)
				{
					CasinoGames.BetPos = "None";
					CasinoGames.MarkerTomove.Position = offsetPosition121;
				}
				if (CasinoGames.MarkerTomove == null)
				{
					Notification.Show("MK null");
				}
				if (true)
				{
					if (CasinoGames.World3DToScreen2d(offsetPosition2).DistanceTo(new Vector2(x, y)) < 0.02f)
					{
						CasinoGames.BetPosNum = 1;
						CasinoGames.BetPos = "0";
						CasinoGames.MarkerTomove.Position = offsetPosition2;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition).DistanceTo(new Vector2(x, y)) < 0.02f)
					{
						CasinoGames.BetPosNum = 2;
						CasinoGames.BetPos = "00";
						CasinoGames.MarkerTomove.Position = offsetPosition;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition3).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 3;
						CasinoGames.BetPos = "Red, 1";
						CasinoGames.MarkerTomove.Position = offsetPosition3;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition4).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 4;
						CasinoGames.BetPos = "Black, 2";
						CasinoGames.MarkerTomove.Position = offsetPosition4;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition5).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 5;
						CasinoGames.BetPos = "Red, 3";
						CasinoGames.MarkerTomove.Position = offsetPosition5;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition6).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 6;
						CasinoGames.BetPos = "Black, 4";
						CasinoGames.MarkerTomove.Position = offsetPosition6;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition7).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 7;
						CasinoGames.BetPos = "Red, 5";
						CasinoGames.MarkerTomove.Position = offsetPosition7;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition8).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 8;
						CasinoGames.BetPos = "Black, 6";
						CasinoGames.MarkerTomove.Position = offsetPosition8;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition9).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 9;
						CasinoGames.BetPos = "Red, 7";
						CasinoGames.MarkerTomove.Position = offsetPosition9;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition10).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 10;
						CasinoGames.BetPos = "Black, 8";
						CasinoGames.MarkerTomove.Position = offsetPosition10;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition11).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 11;
						CasinoGames.BetPos = "Red, 9";
						CasinoGames.MarkerTomove.Position = offsetPosition11;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition12).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 12;
						CasinoGames.BetPos = "Black, 10";
						CasinoGames.MarkerTomove.Position = offsetPosition12;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition13).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 13;
						CasinoGames.BetPos = "Black, 11";
						CasinoGames.MarkerTomove.Position = offsetPosition13;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition14).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 14;
						CasinoGames.BetPos = "Red, 12";
						CasinoGames.MarkerTomove.Position = offsetPosition14;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition15).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 15;
						CasinoGames.BetPos = "Black, 13";
						CasinoGames.MarkerTomove.Position = offsetPosition15;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition16).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 16;
						CasinoGames.BetPos = "Red, 14";
						CasinoGames.MarkerTomove.Position = offsetPosition16;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition17).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 17;
						CasinoGames.BetPos = "Black, 15";
						CasinoGames.MarkerTomove.Position = offsetPosition17;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition18).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 18;
						CasinoGames.BetPos = "Red, 16";
						CasinoGames.MarkerTomove.Position = offsetPosition18;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition19).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 19;
						CasinoGames.BetPos = "Black, 17";
						CasinoGames.MarkerTomove.Position = offsetPosition19;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition20).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 20;
						CasinoGames.BetPos = "Red, 18";
						CasinoGames.MarkerTomove.Position = offsetPosition20;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition21).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 21;
						CasinoGames.BetPos = "Red, 19";
						CasinoGames.MarkerTomove.Position = offsetPosition21;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition22).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 22;
						CasinoGames.BetPos = "Black, 20";
						CasinoGames.MarkerTomove.Position = offsetPosition22;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition23).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 23;
						CasinoGames.BetPos = "Red, 21";
						CasinoGames.MarkerTomove.Position = offsetPosition23;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition24).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 24;
						CasinoGames.BetPos = "Black, 22";
						CasinoGames.MarkerTomove.Position = offsetPosition24;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition25).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 25;
						CasinoGames.BetPos = "Red, 23";
						CasinoGames.MarkerTomove.Position = offsetPosition25;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition26).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 26;
						CasinoGames.BetPos = "Black, 24";
						CasinoGames.MarkerTomove.Position = offsetPosition26;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition27).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 27;
						CasinoGames.BetPos = "Red, 25";
						CasinoGames.MarkerTomove.Position = offsetPosition27;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition28).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 28;
						CasinoGames.BetPos = "Black, 26";
						CasinoGames.MarkerTomove.Position = offsetPosition28;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition29).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 29;
						CasinoGames.BetPos = "Red, 27";
						CasinoGames.MarkerTomove.Position = offsetPosition29;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition30).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 30;
						CasinoGames.BetPos = "Black, 28";
						CasinoGames.MarkerTomove.Position = offsetPosition30;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition31).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 31;
						CasinoGames.BetPos = "Black, 29";
						CasinoGames.MarkerTomove.Position = offsetPosition31;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition32).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 32;
						CasinoGames.BetPos = "Red, 30";
						CasinoGames.MarkerTomove.Position = offsetPosition32;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition33).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 33;
						CasinoGames.BetPos = "Black, 31";
						CasinoGames.MarkerTomove.Position = offsetPosition33;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition34).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 34;
						CasinoGames.BetPos = "Red, 32";
						CasinoGames.MarkerTomove.Position = offsetPosition34;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition35).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 35;
						CasinoGames.BetPos = "Black, 33";
						CasinoGames.MarkerTomove.Position = offsetPosition35;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition36).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 36;
						CasinoGames.BetPos = "Red, 34";
						CasinoGames.MarkerTomove.Position = offsetPosition36;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition37).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 37;
						CasinoGames.BetPos = "Black, 35";
						CasinoGames.MarkerTomove.Position = offsetPosition37;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition38).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 38;
						CasinoGames.BetPos = "Red, 36";
						CasinoGames.MarkerTomove.Position = offsetPosition38;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition39).DistanceTo(new Vector2(x, y)) < 0.02f)
					{
						CasinoGames.BetPosNum = 39;
						CasinoGames.BetPos = "1st Column";
						CasinoGames.MarkerTomove.Position = offsetPosition39;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition40).DistanceTo(new Vector2(x, y)) < 0.02f)
					{
						CasinoGames.BetPosNum = 40;
						CasinoGames.BetPos = "2nd Column";
						CasinoGames.MarkerTomove.Position = offsetPosition40;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition41).DistanceTo(new Vector2(x, y)) < 0.02f)
					{
						CasinoGames.BetPosNum = 41;
						CasinoGames.BetPos = "3rd Column";
						CasinoGames.MarkerTomove.Position = offsetPosition41;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition50).DistanceTo(new Vector2(x, y)) < 0.02f)
					{
						CasinoGames.BetPosNum = 42;
						CasinoGames.BetPos = "3rd 12";
						CasinoGames.MarkerTomove.Position = offsetPosition50;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition49).DistanceTo(new Vector2(x, y)) < 0.02f)
					{
						CasinoGames.BetPosNum = 43;
						CasinoGames.BetPos = "2nd 12";
						CasinoGames.MarkerTomove.Position = offsetPosition49;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition48).DistanceTo(new Vector2(x, y)) < 0.02f)
					{
						CasinoGames.BetPosNum = 44;
						CasinoGames.BetPos = "1st 12";
						CasinoGames.MarkerTomove.Position = offsetPosition48;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition44).DistanceTo(new Vector2(x, y)) < 0.02f)
					{
						CasinoGames.BetPosNum = 45;
						CasinoGames.BetPos = "1 to 18";
						CasinoGames.MarkerTomove.Position = offsetPosition44;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition45).DistanceTo(new Vector2(x, y)) < 0.02f)
					{
						CasinoGames.BetPosNum = 46;
						CasinoGames.BetPos = "Even";
						CasinoGames.MarkerTomove.Position = offsetPosition45;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition46).DistanceTo(new Vector2(x, y)) < 0.02f)
					{
						CasinoGames.BetPosNum = 47;
						CasinoGames.BetPos = "Red";
						CasinoGames.MarkerTomove.Position = offsetPosition46;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition47).DistanceTo(new Vector2(x, y)) < 0.02f)
					{
						CasinoGames.BetPosNum = 48;
						CasinoGames.BetPos = "Black";
						CasinoGames.MarkerTomove.Position = offsetPosition47;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition43).DistanceTo(new Vector2(x, y)) < 0.02f)
					{
						CasinoGames.BetPosNum = 49;
						CasinoGames.BetPos = "Odd";
						CasinoGames.MarkerTomove.Position = offsetPosition43;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition42).DistanceTo(new Vector2(x, y)) < 0.02f)
					{
						CasinoGames.BetPosNum = 50;
						CasinoGames.BetPos = "19 to 36";
						CasinoGames.MarkerTomove.Position = offsetPosition42;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition51).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 51;
						CasinoGames.BetPos = "Numbers 1, 2, 4, 5";
						CasinoGames.MarkerTomove.Position = offsetPosition51;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition52).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 52;
						CasinoGames.BetPos = "Numbers 2, 3, 5, 6";
						CasinoGames.MarkerTomove.Position = offsetPosition52;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition53).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 53;
						CasinoGames.BetPos = "Numbers 4, 5, 7, 8";
						CasinoGames.MarkerTomove.Position = offsetPosition53;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition54).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 54;
						CasinoGames.BetPos = "Numbers 5, 6, 8, 9";
						CasinoGames.MarkerTomove.Position = offsetPosition54;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition55).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 55;
						CasinoGames.BetPos = "Numbers 7, 8, 10, 11";
						CasinoGames.MarkerTomove.Position = offsetPosition55;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition56).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 56;
						CasinoGames.BetPos = "Numbers 8, 9, 11, 12";
						CasinoGames.MarkerTomove.Position = offsetPosition56;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition57).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 57;
						CasinoGames.BetPos = "Numbers 10, 11, 13, 14";
						CasinoGames.MarkerTomove.Position = offsetPosition57;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition58).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 58;
						CasinoGames.BetPos = "Numbers 11, 12, 14, 15";
						CasinoGames.MarkerTomove.Position = offsetPosition58;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition59).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 59;
						CasinoGames.BetPos = "Numbers 13, 14, 16, 17";
						CasinoGames.MarkerTomove.Position = offsetPosition59;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition60).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 60;
						CasinoGames.BetPos = "Numbers 14, 15, 17, 18";
						CasinoGames.MarkerTomove.Position = offsetPosition60;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition61).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 61;
						CasinoGames.BetPos = "Numbers 16, 17, 19, 20";
						CasinoGames.MarkerTomove.Position = offsetPosition61;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition62).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 62;
						CasinoGames.BetPos = "Numbers 17, 18, 20, 21";
						CasinoGames.MarkerTomove.Position = offsetPosition62;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition63).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 63;
						CasinoGames.BetPos = "Numbers 19, 20, 22, 23";
						CasinoGames.MarkerTomove.Position = offsetPosition63;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition64).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 64;
						CasinoGames.BetPos = "Numbers 20, 21, 23, 24";
						CasinoGames.MarkerTomove.Position = offsetPosition64;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition65).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 65;
						CasinoGames.BetPos = "Numbers 22, 23, 25, 26";
						CasinoGames.MarkerTomove.Position = offsetPosition65;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition66).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 66;
						CasinoGames.BetPos = "Numbers 23, 24, 26, 27";
						CasinoGames.MarkerTomove.Position = offsetPosition66;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition67).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 67;
						CasinoGames.BetPos = "Numbers 25, 26, 28, 29";
						CasinoGames.MarkerTomove.Position = offsetPosition67;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition68).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 68;
						CasinoGames.BetPos = "Numbers 26, 27, 29, 30";
						CasinoGames.MarkerTomove.Position = offsetPosition68;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition69).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 69;
						CasinoGames.BetPos = "Numbers 28, 29, 31, 32";
						CasinoGames.MarkerTomove.Position = offsetPosition69;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition70).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 70;
						CasinoGames.BetPos = "Numbers 29, 30, 31, 32";
						CasinoGames.MarkerTomove.Position = offsetPosition70;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition71).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 71;
						CasinoGames.BetPos = "Numbers 31, 32, 34, 35";
						CasinoGames.MarkerTomove.Position = offsetPosition71;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition72).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 72;
						CasinoGames.BetPos = "Numbers 32, 33, 35, 36";
						CasinoGames.MarkerTomove.Position = offsetPosition72;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition73).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 73;
						CasinoGames.BetPos = "Row#1 1, 2, 3";
						CasinoGames.MarkerTomove.Position = offsetPosition73;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition74).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 74;
						CasinoGames.BetPos = "Segment#1 1, 2, 3, 4, 5, 6";
						CasinoGames.MarkerTomove.Position = offsetPosition74;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition75).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 75;
						CasinoGames.BetPos = "Row#2 4, 5, 6";
						CasinoGames.MarkerTomove.Position = offsetPosition75;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition76).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 76;
						CasinoGames.BetPos = "Segment#2 4, 5, 6, 7, 8, 9";
						CasinoGames.MarkerTomove.Position = offsetPosition76;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition77).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 77;
						CasinoGames.BetPos = "Row#3 7, 8, 9";
						CasinoGames.MarkerTomove.Position = offsetPosition77;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition78).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 78;
						CasinoGames.BetPos = "Segment#3 7, 8, 9, 10, 11, 12";
						CasinoGames.MarkerTomove.Position = offsetPosition78;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition79).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 79;
						CasinoGames.BetPos = "Row#4 10, 11, 12";
						CasinoGames.MarkerTomove.Position = offsetPosition79;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition80).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 80;
						CasinoGames.BetPos = "Segment#4 10, 11, 12, 13, 14, 15";
						CasinoGames.MarkerTomove.Position = offsetPosition80;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition81).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 81;
						CasinoGames.BetPos = "Row#5 13, 14, 15";
						CasinoGames.MarkerTomove.Position = offsetPosition81;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition82).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 82;
						CasinoGames.BetPos = "Segment#5 13, 14, 15, 16, 17, 18";
						CasinoGames.MarkerTomove.Position = offsetPosition82;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition83).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 83;
						CasinoGames.BetPos = "Row#6 16, 17, 18";
						CasinoGames.MarkerTomove.Position = offsetPosition83;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition84).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 84;
						CasinoGames.BetPos = "Segment#6 16, 17, 18, 19, 20, 21";
						CasinoGames.MarkerTomove.Position = offsetPosition84;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition85).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 85;
						CasinoGames.BetPos = "Row#7 19, 20, 21";
						CasinoGames.MarkerTomove.Position = offsetPosition85;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition86).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 86;
						CasinoGames.BetPos = "Segment#7 19, 20, 21, 22, 23, 24";
						CasinoGames.MarkerTomove.Position = offsetPosition86;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition87).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 87;
						CasinoGames.BetPos = "Row#8 22, 23, 24";
						CasinoGames.MarkerTomove.Position = offsetPosition87;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition88).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 88;
						CasinoGames.BetPos = "Segment#8 22, 23, 24, 25, 26, 27";
						CasinoGames.MarkerTomove.Position = offsetPosition88;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition89).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 89;
						CasinoGames.BetPos = "Row#9 25, 26, 27";
						CasinoGames.MarkerTomove.Position = offsetPosition89;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition90).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 90;
						CasinoGames.BetPos = "Segment#9 25, 26, 27, 28, 29, 30";
						CasinoGames.MarkerTomove.Position = offsetPosition90;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition91).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 91;
						CasinoGames.BetPos = "Row#10 28, 29, 30";
						CasinoGames.MarkerTomove.Position = offsetPosition91;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition92).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 92;
						CasinoGames.BetPos = "Segment#10 28, 29, 30, 31, 32, 33";
						CasinoGames.MarkerTomove.Position = offsetPosition92;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition93).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 93;
						CasinoGames.BetPos = "Row#11 31, 32, 33";
						CasinoGames.MarkerTomove.Position = offsetPosition93;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition94).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 94;
						CasinoGames.BetPos = "Segment#11 31, 32, 33, 34, 35, 36";
						CasinoGames.MarkerTomove.Position = offsetPosition94;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition95).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 95;
						CasinoGames.BetPos = "Row#12 34, 35, 36";
						CasinoGames.MarkerTomove.Position = offsetPosition95;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition96).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 96;
						CasinoGames.BetPos = "Numbers 1, 2, 3, 0, 00";
						CasinoGames.MarkerTomove.Position = offsetPosition96;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition97).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 97;
						CasinoGames.BetPos = "Numbers 1, 2";
						CasinoGames.MarkerTomove.Position = offsetPosition97;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition98).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 98;
						CasinoGames.BetPos = "Numbers 2, 3";
						CasinoGames.MarkerTomove.Position = offsetPosition98;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition99).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 99;
						CasinoGames.BetPos = "Numbers 4, 5";
						CasinoGames.MarkerTomove.Position = offsetPosition99;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition100).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 100;
						CasinoGames.BetPos = "Numbers 5, 6";
						CasinoGames.MarkerTomove.Position = offsetPosition100;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition101).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 101;
						CasinoGames.BetPos = "Numbers 7, 8";
						CasinoGames.MarkerTomove.Position = offsetPosition101;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition102).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 102;
						CasinoGames.BetPos = "Numbers 8, 9";
						CasinoGames.MarkerTomove.Position = offsetPosition102;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition103).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 103;
						CasinoGames.BetPos = "Numbers 10, 11";
						CasinoGames.MarkerTomove.Position = offsetPosition103;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition104).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 104;
						CasinoGames.BetPos = "Numbers 11, 12";
						CasinoGames.MarkerTomove.Position = offsetPosition104;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition105).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 105;
						CasinoGames.BetPos = "Numbers 13, 14";
						CasinoGames.MarkerTomove.Position = offsetPosition105;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition106).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 106;
						CasinoGames.BetPos = "Numbers 14, 15";
						CasinoGames.MarkerTomove.Position = offsetPosition106;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition107).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 107;
						CasinoGames.BetPos = "Numbers 16, 17";
						CasinoGames.MarkerTomove.Position = offsetPosition107;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition108).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 108;
						CasinoGames.BetPos = "Numbers 17, 18";
						CasinoGames.MarkerTomove.Position = offsetPosition108;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition109).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 109;
						CasinoGames.BetPos = "Numbers 19, 20";
						CasinoGames.MarkerTomove.Position = offsetPosition109;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition110).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 110;
						CasinoGames.BetPos = "Numbers 20, 21";
						CasinoGames.MarkerTomove.Position = offsetPosition110;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition111).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 111;
						CasinoGames.BetPos = "Numbers 22, 23";
						CasinoGames.MarkerTomove.Position = offsetPosition111;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition112).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 112;
						CasinoGames.BetPos = "Numbers 23, 24";
						CasinoGames.MarkerTomove.Position = offsetPosition112;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition113).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 113;
						CasinoGames.BetPos = "Numbers 25, 26";
						CasinoGames.MarkerTomove.Position = offsetPosition113;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition114).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 114;
						CasinoGames.BetPos = "Numbers 26, 27";
						CasinoGames.MarkerTomove.Position = offsetPosition114;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition115).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 115;
						CasinoGames.BetPos = "Numbers 28, 29";
						CasinoGames.MarkerTomove.Position = offsetPosition115;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition116).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 116;
						CasinoGames.BetPos = "Numbers 29, 30";
						CasinoGames.MarkerTomove.Position = offsetPosition116;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition117).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 117;
						CasinoGames.BetPos = "Numbers 31, 32";
						CasinoGames.MarkerTomove.Position = offsetPosition117;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition118).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 118;
						CasinoGames.BetPos = "Numbers 32, 33";
						CasinoGames.MarkerTomove.Position = offsetPosition118;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition119).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 119;
						CasinoGames.BetPos = "Numbers 34, 35";
						CasinoGames.MarkerTomove.Position = offsetPosition119;
					}
					if (CasinoGames.World3DToScreen2d(offsetPosition120).DistanceTo(new Vector2(x, y)) < 0.01f)
					{
						CasinoGames.BetPosNum = 120;
						CasinoGames.BetPos = "Numbers 35, 36";
						CasinoGames.MarkerTomove.Position = offsetPosition120;
					}
					if (CasinoGames.BetPosNum != -1)
					{
						CasinoGames.DisplayHelpTextThisFrameNoSound(CasinoGames.GetBetNotificationAtPosition(CasinoGames.GetRouletteBetPosition(CasinoGames.BetPosNum, tableT)));
					}
				}
			}
			if (!Game.IsControlJustPressed(Control.Attack))
			{
				return;
			}
			if (CasinoGames.BetPosNum != 0)
			{
				if (CasinoGames.Bet > 0f)
				{
					Notification.Show("Bet #" + (CasinoGames.RouletteBets.Count + 1) + " (" + CasinoGames.ConvertRouletteIDtoPositionName(CasinoGames.BetPosNum) + ") set to : $" + ((int)CasinoGames.Bet).ToString("N"));
					Prop prop = GlobalHelper.CreateProp(CasinoGames.RequestModel(CasinoGames.GetCurrentRouletteBetChipModel()), CasinoGames.RouletteMarker1.Position, false, false);
					CasinoGames.Chips.Add(prop);
					CasinoGames.RouletteBets.Add(new CasinoGames.RouletteBet(CasinoGames.BetPos, (int)CasinoGames.Bet, CasinoGames.BetPosNum, CasinoGames.RouletteMarker1.Position, prop));
					EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(prop, true, 255));
					string dict19 = "anim_casino_b@amb@casino@games@threecardpoker@ped_male@engaged@01a@play@v02";
					string text26 = "bet_plus";
					Vector3 position = Game.Player.Character.Position;
					CasinoGames.Scene4 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position.X, position.Y, position.Z, 0.0, 0.0, Game.Player.Character.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.Scene4, CasinoGames.LoadDict(dict19), text26, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.Scene4, text26, CasinoGames.LoadDict(dict19), 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, CasinoGames.GetClosestDealer(Game.Player.Character.Position, CasinoGames.RouletteDealerTables), "MINIGAME_DEALER_PLACE_BET", "SPEECH_PARAMS_STANDARD", false);
					CasinoGames.ROULETTE_AUDIO_PLAY_SFX_PLACE_CHIPS(CasinoGames.GetRouletteBetPosition(tableT, CasinoGames.GetRouletteBetPosition(CasinoGames.BetPosNum, tableT)));
					CasinoGames.BetPosNum = 0;
				}
				else
				{
					Notification.Show("~t~Roulette~w~ Cannot place a bet of $0");
				}
			}
			else
			{
				Notification.Show("~t~Roulette~w~ Cannot place a bet, when selected marker has not been moved.");
			}
		}
	}
}
