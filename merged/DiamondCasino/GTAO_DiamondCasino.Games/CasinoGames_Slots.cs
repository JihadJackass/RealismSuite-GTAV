using System;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace GTAO_DiamondCasino.Games
{
	internal class CasinoGames_Slots : Script
	{
		public static int ClosestSlotMachine = -1;

		public static int NextSlotMachineGet;

		public CasinoGames_Slots()
		{
			if (_Debug.CasinoGames_Slots != -1)
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

		public static void OnTick(object sender, EventArgs e)
		{
			if (global::Gambling.Diamond.Enabled) return;   // RealismSuite plays this game here
			if (!CasinoGames.CanRunModRightNow)
			{
				return;
			}
			if (!Class1.Initialized)
			{
				_Debug.CasinoGames_Slots = 0;
				return;
			}
			_Debug.CasinoGames_Slots = 0;
			Vector3 position = Game.Player.Character.Position;
			Vector3 closestGame = GetClosestGame(position, "vw_prop_casino_slot_04a");
			Vector3 closestGame2 = GetClosestGame(position, "vw_prop_casino_slot_05a");
			Vector3 closestGame3 = GetClosestGame(position, "vw_prop_casino_slot_06a");
			Vector3 closestGame4 = GetClosestGame(position, "vw_prop_casino_slot_01a");
			Vector3 closestGame5 = GetClosestGame(position, "vw_prop_casino_slot_08a");
			Vector3 closestGame6 = GetClosestGame(position, "vw_prop_casino_slot_07a");
			Vector3 closestGame7 = GetClosestGame(position, "vw_prop_casino_slot_02a");
			Vector3 closestGame8 = GetClosestGame(position, "vw_prop_casino_slot_03a");
			if (CasinoGames.GetDistance_VDIST2(Game.Player.Character.Position, closestGame, CasinoGames.GetDistSign.GreaterEqual, 5f) && CasinoGames.GetDistance_VDIST2(Game.Player.Character.Position, closestGame2, CasinoGames.GetDistSign.GreaterEqual, 5f) && CasinoGames.GetDistance_VDIST2(Game.Player.Character.Position, closestGame3, CasinoGames.GetDistSign.GreaterEqual, 5f) && CasinoGames.GetDistance_VDIST2(Game.Player.Character.Position, closestGame4, CasinoGames.GetDistSign.GreaterEqual, 5f) && CasinoGames.GetDistance_VDIST2(Game.Player.Character.Position, closestGame5, CasinoGames.GetDistSign.GreaterEqual, 5f) && CasinoGames.GetDistance_VDIST2(Game.Player.Character.Position, closestGame6, CasinoGames.GetDistSign.GreaterEqual, 5f) && CasinoGames.GetDistance_VDIST2(Game.Player.Character.Position, closestGame7, CasinoGames.GetDistSign.GreaterEqual, 5f) && CasinoGames.GetDistance_VDIST2(Game.Player.Character.Position, closestGame8, CasinoGames.GetDistSign.GreaterEqual, 5f))
			{
				return;
			}
			_Debug.CasinoGames_Slots = 1;
			if (Game.GameTime >= NextSlotMachineGet)
			{
				ClosestSlotMachine = -1;
				float radius = 30f;
				for (int i = 0; i < CasinoGames.SlotLocH.Count; i++)
				{
					if (CasinoGames.GetDistance_VDIST2(Game.Player.Character.Position, CasinoGames.SlotLocH[i].Loc, CasinoGames.GetDistSign.LessEqual, radius))
					{
						radius = CasinoGames.GetDistance(Game.Player.Character.Position, CasinoGames.SlotLocH[i].Loc);
						ClosestSlotMachine = i;
					}
				}
				NextSlotMachineGet = Game.GameTime + 2500;
				try
				{
					if (CasinoGames.SlotMachineSounds.Count > 0)
					{
						for (int j = 0; j < CasinoGames.SlotMachineSounds.Count; j++)
						{
							int num = CasinoGames.SlotMachineSounds[j];
							if (Function.Call<bool>(Hash.HAS_SOUND_FINISHED, num))
							{
								Function.Call(Hash.RELEASE_SOUND_ID, num);
								CasinoGames.SlotMachineSounds.Remove(j);
							}
						}
					}
				}
				catch
				{
					Notification.Show("Error Pass 11");
				}
			}
			if (ClosestSlotMachine == -1)
			{
				return;
			}
			int closestSlotMachine = ClosestSlotMachine;
			if (CasinoGames.SlotLocH[closestSlotMachine].In_use)
			{
				return;
			}
			SlotsClass slotsClass = CasinoGames.SlotLocH[closestSlotMachine];
			Prop machineProp = CasinoGames.SlotLocH[closestSlotMachine].MachineProp;
			Vector3 offsetPosition = machineProp.GetOffsetPosition(new Vector3(0f, -0.8f, 0.8f));
			Vector3 offsetPosition2 = machineProp.GetOffsetPosition(new Vector3(0f, -1.3f, 0.4f));
			if (CasinoGames.GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition2, CasinoGames.GetDistSign.Less, 1f))
			{
				if (!CasinoGames.SlotInUse && CasinoGames.SlotMachineStage == 0)
				{
					if (slotsClass.Machine.Equals("vw_prop_casino_slot_01a"))
					{
						CasinoGames.MinStartBet = 500f;
					}
					if (slotsClass.Machine.Equals("vw_prop_casino_slot_02a"))
					{
						CasinoGames.MinStartBet = 125f;
					}
					if (slotsClass.Machine.Equals("vw_prop_casino_slot_03a"))
					{
						CasinoGames.MinStartBet = 125f;
					}
					if (slotsClass.Machine.Equals("vw_prop_casino_slot_04a"))
					{
						CasinoGames.MinStartBet = 25f;
					}
					if (slotsClass.Machine.Equals("vw_prop_casino_slot_05a"))
					{
						CasinoGames.MinStartBet = 2500f;
					}
					if (slotsClass.Machine.Equals("vw_prop_casino_slot_06a"))
					{
						CasinoGames.MinStartBet = 500f;
					}
					if (slotsClass.Machine.Equals("vw_prop_casino_slot_07a"))
					{
						CasinoGames.MinStartBet = 2500f;
					}
					if (slotsClass.Machine.Equals("vw_prop_casino_slot_08a"))
					{
						CasinoGames.MinStartBet = 25f;
					}
					if ((float)CasinoGames.Chips_Amount >= CasinoGames.MinStartBet)
					{
						CasinoGames.DisplayHelpTextThisFrameNoSound("Press ~INPUT_CONTEXT~ to play " + CasinoGames.ConvertSlotHashNametoActualName(slotsClass.Machine));
					}
					if ((float)CasinoGames.Chips_Amount <= CasinoGames.MinStartBet)
					{
						CasinoGames.DisplayHelpTextThisFrameNoSound("You must have atleast " + CasinoGames.MinStartBet + "c to play " + CasinoGames.ConvertSlotHashNametoActualName(slotsClass.Machine));
					}
				}
				if (CasinoGames.SlotInUse)
				{
					if (CasinoGames.SlotMachineStage == 1)
					{
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_01a"))
						{
							CasinoGames.DisplayHelpTextThisFrameNoSound("Bet One is 100 chips                             Bet Max is 500 chips");
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_02a"))
						{
							CasinoGames.DisplayHelpTextThisFrameNoSound("Bet One is 25 chips                              Bet Max is 125 chips");
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_03a"))
						{
							CasinoGames.DisplayHelpTextThisFrameNoSound("Bet One is 25 chips                              Bet Max is 125 chips");
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_04a"))
						{
							CasinoGames.DisplayHelpTextThisFrameNoSound("Bet One is 5 chips                                  Bet Max is 25 chips");
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_05a"))
						{
							CasinoGames.DisplayHelpTextThisFrameNoSound("Bet One is 500 chips                             Bet Max is 2500 chips");
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_06a"))
						{
							CasinoGames.DisplayHelpTextThisFrameNoSound("Bet One is 100 chips                             Bet Max is 500 chips");
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_07a"))
						{
							CasinoGames.DisplayHelpTextThisFrameNoSound("Bet One is 500 chips                             Bet Max is 2500 chips");
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_08a"))
						{
							CasinoGames.DisplayHelpTextThisFrameNoSound("Class1Bet One is 5 chips                                 Bet Max is 25 chips");
						}
					}
					if (CasinoGames.SlotMachineStage == 2)
					{
						CasinoGames.SetBet_SlotMachine();
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_01a"))
						{
							if (Game.IsControlPressed(Control.VehicleHandbrake))
							{
								CasinoGames.BetSlot = 100f;
								if ((float)CasinoGames.Chips_Amount - CasinoGames.BetSlot >= CasinoGames.BetSlot)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "place_bet", CasinoGames.GetSlotMachineAudioDict(slotsClass.Machine), true);
									CasinoGames.BetOne = true;
									CasinoGames.BetMax = false;
									string dict = "anim_casino_a@amb@casino@games@slots@male";
									CasinoGames.LoadDict(dict);
									CasinoGames.LoadDict(dict);
									int num2 = new Random().Next(0, 300);
									string text = "";
									if (num2 >= 0 && num2 < 100)
									{
										text = "betidle_press_betone_a";
									}
									if (num2 >= 100 && num2 < 200)
									{
										text = "betidle_press_betone_b";
									}
									if (num2 >= 200 && num2 < 300)
									{
										text = "betidle_press_betone_c";
									}
									Vector3 position2 = CasinoGames.CurrentSlotMachine.Position;
									CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position2.X, position2.Y, position2.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict), text, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text, CasinoGames.LoadDict(dict), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
									CasinoGames.SlotMachineStage = 3;
								}
								else
								{
									Notification.Show(CasinoGames.GetHostName() + " you dont have enough chips to play another round of this game!");
								}
							}
							if (Game.IsControlPressed(Control.PhoneSelect))
							{
								CasinoGames.BetSlot = 500f;
								if ((float)CasinoGames.Chips_Amount - CasinoGames.BetSlot >= CasinoGames.BetSlot)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "place_max_bet", CasinoGames.GetSlotMachineAudioDict(slotsClass.Machine), true);
									CasinoGames.BetOne = false;
									CasinoGames.BetMax = true;
									string dict2 = "anim_casino_a@amb@casino@games@slots@male";
									CasinoGames.LoadDict(dict2);
									CasinoGames.LoadDict(dict2);
									int num3 = new Random().Next(0, 300);
									string text2 = "";
									if (num3 >= 0 && num3 < 100)
									{
										text2 = "betidle_press_betmax_a";
									}
									if (num3 >= 100 && num3 < 200)
									{
										text2 = "betidle_press_betmax_b";
									}
									if (num3 >= 200 && num3 < 300)
									{
										text2 = "betidle_press_betmax_c";
									}
									Vector3 position3 = CasinoGames.CurrentSlotMachine.Position;
									CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position3.X, position3.Y, position3.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict2), text2, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text2, CasinoGames.LoadDict(dict2), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
									CasinoGames.SlotMachineStage = 3;
								}
								else
								{
									Notification.Show(CasinoGames.GetHostName() + " you dont have enough chips to play another round of this game!");
								}
							}
							if (Game.IsControlPressed(Control.Sprint))
							{
								CasinoGames.BetSlot = 500f;
								if ((float)CasinoGames.Chips_Amount - CasinoGames.BetSlot >= CasinoGames.BetSlot)
								{
									CasinoGames.BetOne = false;
									CasinoGames.BetMax = true;
									string dict3 = "anim_casino_a@amb@casino@games@slots@male";
									CasinoGames.LoadDict(dict3);
									CasinoGames.LoadDict(dict3);
									int num4 = new Random().Next(0, 200);
									string text3 = "";
									string text4 = "";
									if (num4 >= 0 && num4 < 100)
									{
										text4 = "betidle_pull_spin_a_slotmachine";
										text3 = "betidle_pull_spin_a";
									}
									if (num4 >= 100 && num4 < 200)
									{
										text4 = "betidle_pull_spin_b_slotmachine";
										text3 = "betidle_pull_spin_b";
									}
									Vector3 position4 = CasinoGames.CurrentSlotMachine.Position;
									CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position4.X, position4.Y, position4.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict3), text3, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text3, CasinoGames.LoadDict(dict3), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict3), text3, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.CurrentSlotMachine, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict3), text4, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.CurrentSlotMachine, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text4, CasinoGames.LoadDict(dict3), 3000.0, -2f, -2f, CasinoGames.CurrentSlotMachine.Rotation.Z, 1148846080, 0);
									CasinoGames.SlotMachineStage = 3;
								}
								else
								{
									Notification.Show(CasinoGames.GetHostName() + " you dont have enough chips to play another round of this game!");
								}
							}
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_02a"))
						{
							if (Game.IsControlPressed(Control.VehicleHandbrake))
							{
								CasinoGames.BetSlot = 25f;
								if ((float)CasinoGames.Chips_Amount - CasinoGames.BetSlot >= CasinoGames.BetSlot)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "place_bet", CasinoGames.GetSlotMachineAudioDict(slotsClass.Machine), true);
									CasinoGames.BetOne = true;
									CasinoGames.BetMax = false;
									string dict4 = "anim_casino_a@amb@casino@games@slots@male";
									CasinoGames.LoadDict(dict4);
									CasinoGames.LoadDict(dict4);
									int num5 = new Random().Next(0, 300);
									string text5 = "";
									if (num5 >= 0 && num5 < 100)
									{
										text5 = "betidle_press_betone_a";
									}
									if (num5 >= 100 && num5 < 200)
									{
										text5 = "betidle_press_betone_b";
									}
									if (num5 >= 200 && num5 < 300)
									{
										text5 = "betidle_press_betone_c";
									}
									Vector3 position5 = CasinoGames.CurrentSlotMachine.Position;
									CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position5.X, position5.Y, position5.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict4), text5, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text5, CasinoGames.LoadDict(dict4), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
									CasinoGames.SlotMachineStage = 3;
								}
								else
								{
									Notification.Show(CasinoGames.GetHostName() + " you dont have enough chips to play another round of this game!");
								}
							}
							if (Game.IsControlPressed(Control.PhoneSelect))
							{
								CasinoGames.BetSlot = 125f;
								if ((float)CasinoGames.Chips_Amount - CasinoGames.BetSlot >= CasinoGames.BetSlot)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "place_max_bet", CasinoGames.GetSlotMachineAudioDict(slotsClass.Machine), true);
									CasinoGames.BetOne = false;
									CasinoGames.BetMax = true;
									string dict5 = "anim_casino_a@amb@casino@games@slots@male";
									CasinoGames.LoadDict(dict5);
									CasinoGames.LoadDict(dict5);
									int num6 = new Random().Next(0, 300);
									string text6 = "";
									if (num6 >= 0 && num6 < 100)
									{
										text6 = "betidle_press_betmax_a";
									}
									if (num6 >= 100 && num6 < 200)
									{
										text6 = "betidle_press_betmax_b";
									}
									if (num6 >= 200 && num6 < 300)
									{
										text6 = "betidle_press_betmax_c";
									}
									Vector3 position6 = CasinoGames.CurrentSlotMachine.Position;
									CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position6.X, position6.Y, position6.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict5), text6, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text6, CasinoGames.LoadDict(dict5), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
									CasinoGames.SlotMachineStage = 3;
								}
								else
								{
									Notification.Show(CasinoGames.GetHostName() + " you dont have enough chips to play another round of this game!");
								}
							}
							if (Game.IsControlPressed(Control.Sprint))
							{
								CasinoGames.BetSlot = 125f;
								if ((float)CasinoGames.Chips_Amount - CasinoGames.BetSlot >= CasinoGames.BetSlot)
								{
									CasinoGames.BetOne = false;
									CasinoGames.BetMax = true;
									string dict6 = "anim_casino_a@amb@casino@games@slots@male";
									CasinoGames.LoadDict(dict6);
									CasinoGames.LoadDict(dict6);
									int num7 = new Random().Next(0, 200);
									string text7 = "";
									string text8 = "";
									if (num7 >= 0 && num7 < 100)
									{
										text8 = "betidle_pull_spin_a_slotmachine";
										text7 = "betidle_pull_spin_a";
									}
									if (num7 >= 100 && num7 < 200)
									{
										text8 = "betidle_pull_spin_b_slotmachine";
										text7 = "betidle_pull_spin_b";
									}
									Vector3 position7 = CasinoGames.CurrentSlotMachine.Position;
									CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position7.X, position7.Y, position7.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict6), text7, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text7, CasinoGames.LoadDict(dict6), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict6), text7, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.CurrentSlotMachine, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict6), text8, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.CurrentSlotMachine, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text8, CasinoGames.LoadDict(dict6), 3000.0, -2f, -2f, CasinoGames.CurrentSlotMachine.Rotation.Z, 1148846080, 0);
									CasinoGames.SlotMachineStage = 3;
								}
								else
								{
									Notification.Show(CasinoGames.GetHostName() + " you dont have enough chips to play another round of this game!");
								}
							}
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_03a"))
						{
							if (Game.IsControlPressed(Control.VehicleHandbrake))
							{
								CasinoGames.BetSlot = 25f;
								if ((float)CasinoGames.Chips_Amount - CasinoGames.BetSlot >= CasinoGames.BetSlot)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "place_bet", CasinoGames.GetSlotMachineAudioDict(slotsClass.Machine), true);
									CasinoGames.BetOne = true;
									CasinoGames.BetMax = false;
									string dict7 = "anim_casino_a@amb@casino@games@slots@male";
									CasinoGames.LoadDict(dict7);
									CasinoGames.LoadDict(dict7);
									int num8 = new Random().Next(0, 300);
									string text9 = "";
									if (num8 >= 0 && num8 < 100)
									{
										text9 = "betidle_press_betone_a";
									}
									if (num8 >= 100 && num8 < 200)
									{
										text9 = "betidle_press_betone_b";
									}
									if (num8 >= 200 && num8 < 300)
									{
										text9 = "betidle_press_betone_c";
									}
									Vector3 position8 = CasinoGames.CurrentSlotMachine.Position;
									CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position8.X, position8.Y, position8.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict7), text9, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text9, CasinoGames.LoadDict(dict7), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
									CasinoGames.SlotMachineStage = 3;
								}
								else
								{
									Notification.Show(CasinoGames.GetHostName() + " you dont have enough chips to play another round of this game!");
								}
							}
							if (Game.IsControlPressed(Control.PhoneSelect))
							{
								CasinoGames.BetSlot = 125f;
								if ((float)CasinoGames.Chips_Amount - CasinoGames.BetSlot >= CasinoGames.BetSlot)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "place_max_bet", CasinoGames.GetSlotMachineAudioDict(slotsClass.Machine), true);
									CasinoGames.BetOne = false;
									CasinoGames.BetMax = true;
									string dict8 = "anim_casino_a@amb@casino@games@slots@male";
									CasinoGames.LoadDict(dict8);
									CasinoGames.LoadDict(dict8);
									int num9 = new Random().Next(0, 300);
									string text10 = "";
									if (num9 >= 0 && num9 < 100)
									{
										text10 = "betidle_press_betmax_a";
									}
									if (num9 >= 100 && num9 < 200)
									{
										text10 = "betidle_press_betmax_b";
									}
									if (num9 >= 200 && num9 < 300)
									{
										text10 = "betidle_press_betmax_c";
									}
									Vector3 position9 = CasinoGames.CurrentSlotMachine.Position;
									CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position9.X, position9.Y, position9.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict8), text10, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text10, CasinoGames.LoadDict(dict8), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
									CasinoGames.SlotMachineStage = 3;
								}
								else
								{
									Notification.Show(CasinoGames.GetHostName() + " you dont have enough chips to play another round of this game!");
								}
							}
							if (Game.IsControlPressed(Control.Sprint))
							{
								CasinoGames.BetSlot = 125f;
								if ((float)CasinoGames.Chips_Amount - CasinoGames.BetSlot >= CasinoGames.BetSlot)
								{
									CasinoGames.BetOne = false;
									CasinoGames.BetMax = true;
									string dict9 = "anim_casino_a@amb@casino@games@slots@male";
									CasinoGames.LoadDict(dict9);
									CasinoGames.LoadDict(dict9);
									int num10 = new Random().Next(0, 200);
									string text11 = "";
									string text12 = "";
									if (num10 >= 0 && num10 < 100)
									{
										text12 = "betidle_pull_spin_a_slotmachine";
										text11 = "betidle_pull_spin_a";
									}
									if (num10 >= 100 && num10 < 200)
									{
										text12 = "betidle_pull_spin_b_slotmachine";
										text11 = "betidle_pull_spin_b";
									}
									Vector3 position10 = CasinoGames.CurrentSlotMachine.Position;
									CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position10.X, position10.Y, position10.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict9), text11, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text11, CasinoGames.LoadDict(dict9), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict9), text11, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.CurrentSlotMachine, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict9), text12, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.CurrentSlotMachine, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text12, CasinoGames.LoadDict(dict9), 3000.0, -2f, -2f, CasinoGames.CurrentSlotMachine.Rotation.Z, 1148846080, 0);
									CasinoGames.SlotMachineStage = 3;
								}
								else
								{
									Notification.Show(CasinoGames.GetHostName() + " you dont have enough chips to play another round of this game!");
								}
							}
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_04a"))
						{
							if (Game.IsControlPressed(Control.VehicleHandbrake))
							{
								CasinoGames.BetSlot = 5f;
								if ((float)CasinoGames.Chips_Amount - CasinoGames.BetSlot >= CasinoGames.BetSlot)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "place_bet", CasinoGames.GetSlotMachineAudioDict(slotsClass.Machine), true);
									CasinoGames.BetOne = true;
									CasinoGames.BetMax = false;
									string dict10 = "anim_casino_a@amb@casino@games@slots@male";
									CasinoGames.LoadDict(dict10);
									CasinoGames.LoadDict(dict10);
									int num11 = new Random().Next(0, 300);
									string text13 = "";
									if (num11 >= 0 && num11 < 100)
									{
										text13 = "betidle_press_betone_a";
									}
									if (num11 >= 100 && num11 < 200)
									{
										text13 = "betidle_press_betone_b";
									}
									if (num11 >= 200 && num11 < 300)
									{
										text13 = "betidle_press_betone_c";
									}
									Vector3 position11 = CasinoGames.CurrentSlotMachine.Position;
									CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position11.X, position11.Y, position11.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict10), text13, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text13, CasinoGames.LoadDict(dict10), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
									CasinoGames.SlotMachineStage = 3;
								}
								else
								{
									Notification.Show(CasinoGames.GetHostName() + " you dont have enough chips to play another round of this game!");
								}
							}
							if (Game.IsControlPressed(Control.PhoneSelect))
							{
								CasinoGames.BetSlot = 25f;
								if ((float)CasinoGames.Chips_Amount - CasinoGames.BetSlot >= CasinoGames.BetSlot)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "place_max_bet", CasinoGames.GetSlotMachineAudioDict(slotsClass.Machine), true);
									CasinoGames.BetOne = false;
									CasinoGames.BetMax = true;
									string dict11 = "anim_casino_a@amb@casino@games@slots@male";
									CasinoGames.LoadDict(dict11);
									CasinoGames.LoadDict(dict11);
									int num12 = new Random().Next(0, 300);
									string text14 = "";
									if (num12 >= 0 && num12 < 100)
									{
										text14 = "betidle_press_betmax_a";
									}
									if (num12 >= 100 && num12 < 200)
									{
										text14 = "betidle_press_betmax_b";
									}
									if (num12 >= 200 && num12 < 300)
									{
										text14 = "betidle_press_betmax_c";
									}
									Vector3 position12 = CasinoGames.CurrentSlotMachine.Position;
									CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position12.X, position12.Y, position12.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict11), text14, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text14, CasinoGames.LoadDict(dict11), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
									CasinoGames.SlotMachineStage = 3;
								}
								else
								{
									Notification.Show(CasinoGames.GetHostName() + " you dont have enough chips to play another round of this game!");
								}
							}
							if (Game.IsControlPressed(Control.Sprint))
							{
								CasinoGames.BetSlot = 25f;
								if ((float)CasinoGames.Chips_Amount - CasinoGames.BetSlot >= CasinoGames.BetSlot)
								{
									CasinoGames.BetOne = false;
									CasinoGames.BetMax = true;
									string dict12 = "anim_casino_a@amb@casino@games@slots@male";
									CasinoGames.LoadDict(dict12);
									CasinoGames.LoadDict(dict12);
									int num13 = new Random().Next(0, 200);
									string text15 = "";
									string text16 = "";
									if (num13 >= 0 && num13 < 100)
									{
										text16 = "betidle_pull_spin_a_slotmachine";
										text15 = "betidle_pull_spin_a";
									}
									if (num13 >= 100 && num13 < 200)
									{
										text16 = "betidle_pull_spin_b_slotmachine";
										text15 = "betidle_pull_spin_b";
									}
									Vector3 position13 = CasinoGames.CurrentSlotMachine.Position;
									CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position13.X, position13.Y, position13.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict12), text15, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text15, CasinoGames.LoadDict(dict12), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict12), text15, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.CurrentSlotMachine, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict12), text16, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.CurrentSlotMachine, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text16, CasinoGames.LoadDict(dict12), 3000.0, -2f, -2f, CasinoGames.CurrentSlotMachine.Rotation.Z, 1148846080, 0);
									CasinoGames.SlotMachineStage = 3;
								}
								else
								{
									Notification.Show(CasinoGames.GetHostName() + " you dont have enough chips to play another round of this game!");
								}
							}
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_05a"))
						{
							if (Game.IsControlPressed(Control.VehicleHandbrake))
							{
								CasinoGames.BetSlot = 500f;
								if ((float)CasinoGames.Chips_Amount - CasinoGames.BetSlot >= CasinoGames.BetSlot)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "place_bet", CasinoGames.GetSlotMachineAudioDict(slotsClass.Machine), true);
									CasinoGames.BetOne = true;
									CasinoGames.BetMax = false;
									string dict13 = "anim_casino_a@amb@casino@games@slots@male";
									CasinoGames.LoadDict(dict13);
									CasinoGames.LoadDict(dict13);
									int num14 = new Random().Next(0, 300);
									string text17 = "";
									if (num14 >= 0 && num14 < 100)
									{
										text17 = "betidle_press_betone_a";
									}
									if (num14 >= 100 && num14 < 200)
									{
										text17 = "betidle_press_betone_b";
									}
									if (num14 >= 200 && num14 < 300)
									{
										text17 = "betidle_press_betone_c";
									}
									Vector3 position14 = CasinoGames.CurrentSlotMachine.Position;
									CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position14.X, position14.Y, position14.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict13), text17, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text17, CasinoGames.LoadDict(dict13), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
									CasinoGames.SlotMachineStage = 3;
								}
								else
								{
									Notification.Show(CasinoGames.GetHostName() + " you dont have enough chips to play another round of this game!");
								}
							}
							if (Game.IsControlPressed(Control.PhoneSelect))
							{
								CasinoGames.BetSlot = 2500f;
								if ((float)CasinoGames.Chips_Amount - CasinoGames.BetSlot >= CasinoGames.BetSlot)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "place_max_bet", CasinoGames.GetSlotMachineAudioDict(slotsClass.Machine), true);
									CasinoGames.BetOne = false;
									CasinoGames.BetMax = true;
									string dict14 = "anim_casino_a@amb@casino@games@slots@male";
									CasinoGames.LoadDict(dict14);
									CasinoGames.LoadDict(dict14);
									int num15 = new Random().Next(0, 300);
									string text18 = "";
									if (num15 >= 0 && num15 < 100)
									{
										text18 = "betidle_press_betmax_a";
									}
									if (num15 >= 100 && num15 < 200)
									{
										text18 = "betidle_press_betmax_b";
									}
									if (num15 >= 200 && num15 < 300)
									{
										text18 = "betidle_press_betmax_c";
									}
									Vector3 position15 = CasinoGames.CurrentSlotMachine.Position;
									CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position15.X, position15.Y, position15.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict14), text18, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text18, CasinoGames.LoadDict(dict14), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
									CasinoGames.SlotMachineStage = 3;
								}
								else
								{
									Notification.Show(CasinoGames.GetHostName() + " you dont have enough chips to play another round of this game!");
								}
							}
							if (Game.IsControlPressed(Control.Sprint))
							{
								CasinoGames.BetSlot = 2500f;
								if ((float)CasinoGames.Chips_Amount - CasinoGames.BetSlot >= CasinoGames.BetSlot)
								{
									CasinoGames.BetOne = false;
									CasinoGames.BetMax = true;
									string dict15 = "anim_casino_a@amb@casino@games@slots@male";
									CasinoGames.LoadDict(dict15);
									CasinoGames.LoadDict(dict15);
									int num16 = new Random().Next(0, 200);
									string text19 = "";
									string text20 = "";
									if (num16 >= 0 && num16 < 100)
									{
										text20 = "betidle_pull_spin_a_slotmachine";
										text19 = "betidle_pull_spin_a";
									}
									if (num16 >= 100 && num16 < 200)
									{
										text20 = "betidle_pull_spin_b_slotmachine";
										text19 = "betidle_pull_spin_b";
									}
									Vector3 position16 = CasinoGames.CurrentSlotMachine.Position;
									CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position16.X, position16.Y, position16.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict15), text19, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text19, CasinoGames.LoadDict(dict15), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict15), text19, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.CurrentSlotMachine, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict15), text20, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.CurrentSlotMachine, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text20, CasinoGames.LoadDict(dict15), 3000.0, -2f, -2f, CasinoGames.CurrentSlotMachine.Rotation.Z, 1148846080, 0);
									CasinoGames.SlotMachineStage = 3;
								}
								else
								{
									Notification.Show(CasinoGames.GetHostName() + " you dont have enough chips to play another round of this game!");
								}
							}
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_06a"))
						{
							if (Game.IsControlPressed(Control.VehicleHandbrake))
							{
								CasinoGames.BetSlot = 100f;
								if ((float)CasinoGames.Chips_Amount - CasinoGames.BetSlot >= CasinoGames.BetSlot)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "place_bet", CasinoGames.GetSlotMachineAudioDict(slotsClass.Machine), true);
									CasinoGames.BetOne = true;
									CasinoGames.BetMax = false;
									string dict16 = "anim_casino_a@amb@casino@games@slots@male";
									CasinoGames.LoadDict(dict16);
									CasinoGames.LoadDict(dict16);
									int num17 = new Random().Next(0, 300);
									string text21 = "";
									if (num17 >= 0 && num17 < 100)
									{
										text21 = "betidle_press_betone_a";
									}
									if (num17 >= 100 && num17 < 200)
									{
										text21 = "betidle_press_betone_b";
									}
									if (num17 >= 200 && num17 < 300)
									{
										text21 = "betidle_press_betone_c";
									}
									Vector3 position17 = CasinoGames.CurrentSlotMachine.Position;
									CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position17.X, position17.Y, position17.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict16), text21, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text21, CasinoGames.LoadDict(dict16), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
									CasinoGames.SlotMachineStage = 3;
								}
								else
								{
									Notification.Show(CasinoGames.GetHostName() + " you dont have enough chips to play another round of this game!");
								}
							}
							if (Game.IsControlPressed(Control.PhoneSelect))
							{
								CasinoGames.BetSlot = 500f;
								if ((float)CasinoGames.Chips_Amount - CasinoGames.BetSlot >= CasinoGames.BetSlot)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "place_max_bet", CasinoGames.GetSlotMachineAudioDict(slotsClass.Machine), true);
									CasinoGames.BetOne = false;
									CasinoGames.BetMax = true;
									string dict17 = "anim_casino_a@amb@casino@games@slots@male";
									CasinoGames.LoadDict(dict17);
									CasinoGames.LoadDict(dict17);
									int num18 = new Random().Next(0, 300);
									string text22 = "";
									if (num18 >= 0 && num18 < 100)
									{
										text22 = "betidle_press_betmax_a";
									}
									if (num18 >= 100 && num18 < 200)
									{
										text22 = "betidle_press_betmax_b";
									}
									if (num18 >= 200 && num18 < 300)
									{
										text22 = "betidle_press_betmax_c";
									}
									Vector3 position18 = CasinoGames.CurrentSlotMachine.Position;
									CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position18.X, position18.Y, position18.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict17), text22, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text22, CasinoGames.LoadDict(dict17), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
									CasinoGames.SlotMachineStage = 3;
								}
								else
								{
									Notification.Show(CasinoGames.GetHostName() + " you dont have enough chips to play another round of this game!");
								}
							}
							if (Game.IsControlPressed(Control.Sprint))
							{
								CasinoGames.BetSlot = 500f;
								if ((float)CasinoGames.Chips_Amount - CasinoGames.BetSlot >= CasinoGames.BetSlot)
								{
									CasinoGames.BetOne = false;
									CasinoGames.BetMax = true;
									string dict18 = "anim_casino_a@amb@casino@games@slots@male";
									CasinoGames.LoadDict(dict18);
									CasinoGames.LoadDict(dict18);
									int num19 = new Random().Next(0, 200);
									string text23 = "";
									string text24 = "";
									if (num19 >= 0 && num19 < 100)
									{
										text24 = "betidle_pull_spin_a_slotmachine";
										text23 = "betidle_pull_spin_a";
									}
									if (num19 >= 100 && num19 < 200)
									{
										text24 = "betidle_pull_spin_b_slotmachine";
										text23 = "betidle_pull_spin_b";
									}
									Vector3 position19 = CasinoGames.CurrentSlotMachine.Position;
									CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position19.X, position19.Y, position19.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict18), text23, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text23, CasinoGames.LoadDict(dict18), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict18), text23, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.CurrentSlotMachine, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict18), text24, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.CurrentSlotMachine, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text24, CasinoGames.LoadDict(dict18), 3000.0, -2f, -2f, CasinoGames.CurrentSlotMachine.Rotation.Z, 1148846080, 0);
									CasinoGames.SlotMachineStage = 3;
								}
								else
								{
									Notification.Show(CasinoGames.GetHostName() + " you dont have enough chips to play another round of this game!");
								}
							}
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_07a"))
						{
							if (Game.IsControlPressed(Control.VehicleHandbrake))
							{
								CasinoGames.BetSlot = 500f;
								if ((float)CasinoGames.Chips_Amount - CasinoGames.BetSlot >= CasinoGames.BetSlot)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "place_bet", CasinoGames.GetSlotMachineAudioDict(slotsClass.Machine), true);
									CasinoGames.BetOne = true;
									CasinoGames.BetMax = false;
									string dict19 = "anim_casino_a@amb@casino@games@slots@male";
									CasinoGames.LoadDict(dict19);
									CasinoGames.LoadDict(dict19);
									int num20 = new Random().Next(0, 300);
									string text25 = "";
									if (num20 >= 0 && num20 < 100)
									{
										text25 = "betidle_press_betone_a";
									}
									if (num20 >= 100 && num20 < 200)
									{
										text25 = "betidle_press_betone_b";
									}
									if (num20 >= 200 && num20 < 300)
									{
										text25 = "betidle_press_betone_c";
									}
									Vector3 position20 = CasinoGames.CurrentSlotMachine.Position;
									CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position20.X, position20.Y, position20.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict19), text25, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text25, CasinoGames.LoadDict(dict19), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
									CasinoGames.SlotMachineStage = 3;
								}
								else
								{
									Notification.Show(CasinoGames.GetHostName() + " you dont have enough chips to play another round of this game!");
								}
							}
							if (Game.IsControlPressed(Control.PhoneSelect))
							{
								CasinoGames.BetSlot = 2500f;
								if ((float)CasinoGames.Chips_Amount - CasinoGames.BetSlot >= CasinoGames.BetSlot)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "place_max_bet", CasinoGames.GetSlotMachineAudioDict(slotsClass.Machine), true);
									CasinoGames.BetOne = false;
									CasinoGames.BetMax = true;
									string dict20 = "anim_casino_a@amb@casino@games@slots@male";
									CasinoGames.LoadDict(dict20);
									CasinoGames.LoadDict(dict20);
									int num21 = new Random().Next(0, 300);
									string text26 = "";
									if (num21 >= 0 && num21 < 100)
									{
										text26 = "betidle_press_betmax_a";
									}
									if (num21 >= 100 && num21 < 200)
									{
										text26 = "betidle_press_betmax_b";
									}
									if (num21 >= 200 && num21 < 300)
									{
										text26 = "betidle_press_betmax_c";
									}
									Vector3 position21 = CasinoGames.CurrentSlotMachine.Position;
									CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position21.X, position21.Y, position21.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict20), text26, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text26, CasinoGames.LoadDict(dict20), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
									CasinoGames.SlotMachineStage = 3;
								}
								else
								{
									Notification.Show(CasinoGames.GetHostName() + " you dont have enough chips to play another round of this game!");
								}
							}
							if (Game.IsControlPressed(Control.Sprint))
							{
								CasinoGames.BetSlot = 2500f;
								if ((float)CasinoGames.Chips_Amount - CasinoGames.BetSlot >= CasinoGames.BetSlot)
								{
									CasinoGames.BetOne = false;
									CasinoGames.BetMax = true;
									string dict21 = "anim_casino_a@amb@casino@games@slots@male";
									CasinoGames.LoadDict(dict21);
									CasinoGames.LoadDict(dict21);
									int num22 = new Random().Next(0, 200);
									string text27 = "";
									string text28 = "";
									if (num22 >= 0 && num22 < 100)
									{
										text28 = "betidle_pull_spin_a_slotmachine";
										text27 = "betidle_pull_spin_a";
									}
									if (num22 >= 100 && num22 < 200)
									{
										text28 = "betidle_pull_spin_b_slotmachine";
										text27 = "betidle_pull_spin_b";
									}
									Vector3 position22 = CasinoGames.CurrentSlotMachine.Position;
									CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position22.X, position22.Y, position22.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict21), text27, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text27, CasinoGames.LoadDict(dict21), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict21), text27, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.CurrentSlotMachine, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict21), text28, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.CurrentSlotMachine, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text28, CasinoGames.LoadDict(dict21), 3000.0, -2f, -2f, CasinoGames.CurrentSlotMachine.Rotation.Z, 1148846080, 0);
									CasinoGames.SlotMachineStage = 3;
								}
								else
								{
									Notification.Show(CasinoGames.GetHostName() + " you dont have enough chips to play another round of this game!");
								}
							}
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_08a"))
						{
							if (Game.IsControlPressed(Control.VehicleHandbrake))
							{
								CasinoGames.BetSlot = 5f;
								if ((float)CasinoGames.Chips_Amount - CasinoGames.BetSlot >= CasinoGames.BetSlot)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "place_bet", CasinoGames.GetSlotMachineAudioDict(slotsClass.Machine), true);
									CasinoGames.BetOne = true;
									CasinoGames.BetMax = false;
									string dict22 = "anim_casino_a@amb@casino@games@slots@male";
									CasinoGames.LoadDict(dict22);
									CasinoGames.LoadDict(dict22);
									int num23 = new Random().Next(0, 300);
									string text29 = "";
									if (num23 >= 0 && num23 < 100)
									{
										text29 = "betidle_press_betone_a";
									}
									if (num23 >= 100 && num23 < 200)
									{
										text29 = "betidle_press_betone_b";
									}
									if (num23 >= 200 && num23 < 300)
									{
										text29 = "betidle_press_betone_c";
									}
									Vector3 position23 = CasinoGames.CurrentSlotMachine.Position;
									CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position23.X, position23.Y, position23.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict22), text29, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text29, CasinoGames.LoadDict(dict22), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
									CasinoGames.SlotMachineStage = 3;
								}
								else
								{
									Notification.Show(CasinoGames.GetHostName() + " you dont have enough chips to play another round of this game!");
								}
							}
							if (Game.IsControlPressed(Control.PhoneSelect))
							{
								CasinoGames.BetSlot = 25f;
								if ((float)CasinoGames.Chips_Amount - CasinoGames.BetSlot >= CasinoGames.BetSlot)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "place_max_bet", CasinoGames.GetSlotMachineAudioDict(slotsClass.Machine), true);
									CasinoGames.BetOne = false;
									CasinoGames.BetMax = true;
									string dict23 = "anim_casino_a@amb@casino@games@slots@male";
									CasinoGames.LoadDict(dict23);
									CasinoGames.LoadDict(dict23);
									int num24 = new Random().Next(0, 300);
									string text30 = "";
									if (num24 >= 0 && num24 < 100)
									{
										text30 = "betidle_press_betmax_a";
									}
									if (num24 >= 100 && num24 < 200)
									{
										text30 = "betidle_press_betmax_b";
									}
									if (num24 >= 200 && num24 < 300)
									{
										text30 = "betidle_press_betmax_c";
									}
									Vector3 position24 = CasinoGames.CurrentSlotMachine.Position;
									CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position24.X, position24.Y, position24.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict23), text30, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text30, CasinoGames.LoadDict(dict23), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
									CasinoGames.SlotMachineStage = 3;
								}
								else
								{
									Notification.Show(CasinoGames.GetHostName() + " you dont have enough chips to play another round of this game!");
								}
							}
							if (Game.IsControlPressed(Control.Sprint))
							{
								CasinoGames.BetSlot = 25f;
								if ((float)CasinoGames.Chips_Amount - CasinoGames.BetSlot >= CasinoGames.BetSlot)
								{
									CasinoGames.BetOne = false;
									CasinoGames.BetMax = true;
									string dict24 = "anim_casino_a@amb@casino@games@slots@male";
									CasinoGames.LoadDict(dict24);
									CasinoGames.LoadDict(dict24);
									int num25 = new Random().Next(0, 200);
									string text31 = "";
									string text32 = "";
									if (num25 >= 0 && num25 < 100)
									{
										text32 = "betidle_pull_spin_a_slotmachine";
										text31 = "betidle_pull_spin_a";
									}
									if (num25 >= 100 && num25 < 200)
									{
										text32 = "betidle_pull_spin_b_slotmachine";
										text31 = "betidle_pull_spin_b";
									}
									Vector3 position25 = CasinoGames.CurrentSlotMachine.Position;
									CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position25.X, position25.Y, position25.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict24), text31, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text31, CasinoGames.LoadDict(dict24), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict24), text31, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, CasinoGames.CurrentSlotMachine, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict24), text32, 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoGames.CurrentSlotMachine, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text32, CasinoGames.LoadDict(dict24), 3000.0, -2f, -2f, CasinoGames.CurrentSlotMachine.Rotation.Z, 1148846080, 0);
									CasinoGames.SlotMachineStage = 3;
								}
								else
								{
									Notification.Show(CasinoGames.GetHostName() + " you dont have enough chips to play another round of this game!");
								}
							}
						}
					}
					if (CasinoGames.SlotMachineStage == 3)
					{
						if (!CasinoGames.SpinnedReels)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "start_spin", CasinoGames.GetSlotMachineAudioDict(slotsClass.Machine), true);
							CasinoGames.Chips_Amount -= (int)CasinoGames.BetSlot;
							Class1.Config.SetValue("Setup", "Chips_Amount", CasinoGames.Chips_Amount);
							Class1.Config.Save();
							if (CasinoGames.BetOne)
							{
								CasinoGames.SlotMultiplier = 1f;
							}
							if (CasinoGames.BetMax)
							{
								CasinoGames.SlotMultiplier = 5f;
							}
							Notification.Show("~y~ " + CasinoGames.BetSlot.ToString("N") + "c~w~ deducted from bank");
							Script.Wait(1);
							CasinoGames.ReelA_SpinAmt = CasinoGames.GetRandomfloat(6500, 15500);
							Script.Wait(1);
							float num26 = CasinoGames.GetRandomfloat(6500, 15500);
							if (num26 < CasinoGames.ReelA_SpinAmt)
							{
								num26 += CasinoGames.GetRandomfloat(6500, 15500);
							}
							CasinoGames.ReelB_SpinAmt = num26;
							Script.Wait(1);
							num26 = CasinoGames.GetRandomfloat(6500, 15500);
							if (num26 < CasinoGames.ReelB_SpinAmt)
							{
								num26 += CasinoGames.GetRandomfloat(6500, 15500);
							}
							CasinoGames.ReelC_SpinAmt = num26;
							Script.Wait(500);
							CasinoGames.SpinnedReels = true;
							CasinoGames.SlotMachineSpinningSoundFx = Function.Call<int>(Hash.GET_SOUND_ID);
							Function.Call(Hash.PLAY_SOUND_FRONTEND, CasinoGames.SlotMachineSpinningSoundFx, "spinning", CasinoGames.GetSlotMachineAudioDict(slotsClass.Machine), true);
						}
						if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene) >= 1065353216)
						{
							string dict25 = "anim_casino_a@amb@casino@games@slots@male";
							CasinoGames.LoadDict(dict25);
							CasinoGames.LoadDict(dict25);
							int num27 = new Random().Next(0, 300);
							string text33 = "";
							if (num27 >= 0 && num27 < 100)
							{
								text33 = "spinning_a";
							}
							if (num27 >= 100 && num27 < 200)
							{
								text33 = "spinning_b";
							}
							if (num27 >= 200 && num27 < 300)
							{
								text33 = "spinning_c";
							}
							Vector3 position26 = CasinoGames.CurrentSlotMachine.Position;
							CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position26.X, position26.Y, position26.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict25), text33, 3000.0, -2.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text33, CasinoGames.LoadDict(dict25), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
						}
						if (CasinoGames.SpinnedReels)
						{
							if (Function.Call<bool>(Hash.HAS_SOUND_FINISHED, CasinoGames.SlotMachineSpinningSoundFx))
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, CasinoGames.SlotMachineSpinningSoundFx, "spinning", CasinoGames.GetSlotMachineAudioDict(slotsClass.Machine), true);
							}
							if (CasinoGames.ReelABase != null && !CasinoGames.FinishedReelA)
							{
								Vector3 rotation = CasinoGames.ReelABase.Rotation;
								CasinoGames.ReelABase.Rotation = new Vector3(rotation.X, rotation.Y += 10f, rotation.Z);
								CasinoGames.ReelA_SpinAmt -= 0.05f;
								if (CasinoGames.ReelA_SpinAmt < 0.05f)
								{
									CasinoGames.ReelA_SpinAmt = 0f;
									CasinoGames.FinishedReelA = true;
									CasinoGames.ReelABase.Rotation = new Vector3(rotation.X, CasinoGames.GetClosest(rotation.Y, CasinoGames.CurrentSlotClass.Machine), rotation.Z);
								}
							}
							if (CasinoGames.ReelBBase != null)
							{
								if (!CasinoGames.FinishedReelB && CasinoGames.ReelB_SpinAmt >= 0f)
								{
									Vector3 rotation2 = CasinoGames.ReelBBase.Rotation;
									CasinoGames.ReelBBase.Rotation = new Vector3(rotation2.X, rotation2.Y += CasinoGames.ReelB_SpinAmt, rotation2.Z);
									CasinoGames.ReelB_SpinAmt -= 0.05f;
									if (CasinoGames.ReelB_SpinAmt < 0.05f)
									{
										CasinoGames.ReelA_SpinAmt = 0f;
										CasinoGames.ReelB_SpinAmt = 0f;
										CasinoGames.FinishedReelB = true;
										CasinoGames.ReelBBase.Rotation = new Vector3(rotation2.X, CasinoGames.GetClosest(rotation2.Y, CasinoGames.CurrentSlotClass.Machine), rotation2.Z);
									}
								}
								if (CasinoGames.ReelA_SpinAmt >= 0f)
								{
									Vector3 rotation3 = CasinoGames.ReelBBase.Rotation;
									CasinoGames.ReelBBase.Rotation = new Vector3(rotation3.X, rotation3.Y += CasinoGames.ReelB_SpinAmt, rotation3.Z);
								}
							}
							if (CasinoGames.ReelCBase != null)
							{
								if (CasinoGames.ReelA_SpinAmt <= 0f && CasinoGames.ReelB_SpinAmt <= 0f)
								{
									if (!CasinoGames.FinishedReelC)
									{
										Vector3 rotation4 = CasinoGames.ReelCBase.Rotation;
										CasinoGames.ReelCBase.Rotation = new Vector3(rotation4.X, rotation4.Y += CasinoGames.ReelC_SpinAmt, rotation4.Z);
										CasinoGames.ReelC_SpinAmt -= 0.05f;
										if (CasinoGames.ReelC_SpinAmt < 0.05f)
										{
											CasinoGames.ReelA_SpinAmt = 0f;
											CasinoGames.ReelB_SpinAmt = 0f;
											CasinoGames.ReelC_SpinAmt = 0f;
											CasinoGames.FinishedReelC = true;
											CasinoGames.ReelCBase.Rotation = new Vector3(rotation4.X, CasinoGames.GetClosest(rotation4.Y, CasinoGames.CurrentSlotClass.Machine), rotation4.Z);
										}
									}
								}
								else
								{
									Vector3 rotation5 = CasinoGames.ReelCBase.Rotation;
									CasinoGames.ReelCBase.Rotation = new Vector3(rotation5.X, rotation5.Y += CasinoGames.ReelC_SpinAmt, rotation5.Z);
								}
							}
							if (CasinoGames.FinishedReelA && CasinoGames.FinishedReelB && CasinoGames.FinishedReelC)
							{
								Function.Call(Hash.STOP_SOUND, CasinoGames.SlotMachineSpinningSoundFx);
								Function.Call(Hash.RELEASE_SOUND_ID, CasinoGames.SlotMachineSpinningSoundFx);
								Vector3 rotation6 = CasinoGames.ReelABase.Rotation;
								Vector3 rotation7 = CasinoGames.ReelBBase.Rotation;
								Vector3 rotation8 = CasinoGames.ReelCBase.Rotation;
								CasinoGames.ALandedOn = CasinoGames.GetLandedOn(rotation6.Y, CasinoGames.CurrentSlotClass.Machine);
								CasinoGames.BLandedOn = CasinoGames.GetLandedOn(rotation7.Y, CasinoGames.CurrentSlotClass.Machine);
								CasinoGames.CLandedOn = CasinoGames.GetLandedOn(rotation8.Y, CasinoGames.CurrentSlotClass.Machine);
								CasinoGames.SlotMachineStage = 4;
							}
						}
					}
					if (CasinoGames.SlotMachineStage == 4)
					{
						float num28 = CasinoGames.CalcualteSlotWin(CasinoGames.CurrentSlotClass.Machine);
						if (num28 >= 1f)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "wheel_stop_on_prize", CasinoGames.GetSlotMachineAudioDict(slotsClass.Machine), true);
							Notification.Show(CasinoGames.GetHostName() + " You won ~y~" + num28.ToString("N") + "~c!");
							CasinoGames.Chips_Amount += (int)num28;
							Class1.Config.SetValue("Setup", "CasinoGames.Chips_Amount", CasinoGames.Chips_Amount);
							Class1.Config.Save();
							CasinoGames.SlotMachineStage = 2;
							CasinoGames.SpinnedReels = false;
							CasinoGames.ReelA_SpinAmt = CasinoGames.GetRandomfloat(6500, 15500);
							Script.Wait(1);
							float num29 = CasinoGames.GetRandomfloat(6500, 15500);
							if (num29 < CasinoGames.ReelA_SpinAmt)
							{
								num29 += CasinoGames.GetRandomfloat(6500, 15500);
							}
							CasinoGames.ReelB_SpinAmt = num29;
							Script.Wait(1);
							num29 = CasinoGames.GetRandomfloat(6500, 15500);
							if (num29 < CasinoGames.ReelB_SpinAmt)
							{
								num29 += CasinoGames.GetRandomfloat(6500, 15500);
							}
							CasinoGames.ReelC_SpinAmt = num29;
							CasinoGames.FinishedReelA = false;
							CasinoGames.FinishedReelB = false;
							CasinoGames.FinishedReelC = false;
						}
						if (num28 <= 0f)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "wheel_stop_clunk", CasinoGames.GetSlotMachineAudioDict(slotsClass.Machine), true);
							Notification.Show(CasinoGames.GetHostName() + " You won nothing");
							CasinoGames.SlotMachineStage = 2;
							CasinoGames.SpinnedReels = false;
							CasinoGames.ReelA_SpinAmt = CasinoGames.GetRandomfloat(6500, 15500);
							Script.Wait(1);
							float num30 = CasinoGames.GetRandomfloat(6500, 15500);
							if (num30 < CasinoGames.ReelA_SpinAmt)
							{
								num30 += CasinoGames.GetRandomfloat(6500, 15500);
							}
							CasinoGames.ReelB_SpinAmt = num30;
							Script.Wait(1);
							num30 = CasinoGames.GetRandomfloat(6500, 15500);
							if (num30 < CasinoGames.ReelB_SpinAmt)
							{
								num30 += CasinoGames.GetRandomfloat(6500, 15500);
							}
							CasinoGames.ReelC_SpinAmt = num30;
							CasinoGames.FinishedReelA = false;
							CasinoGames.FinishedReelB = false;
							CasinoGames.FinishedReelC = false;
							CasinoGames.FinishedReelA = false;
						}
					}
				}
				if (Game.IsControlPressed(Control.Context))
				{
					if (CasinoGames.SlotInUse && CasinoGames.SlotMachineStage == 2)
					{
						CasinoGames.CurrentSlotClass = null;
						string dict26 = "anim_casino_a@amb@casino@games@slots@male";
						CasinoGames.LoadDict(dict26);
						CasinoGames.LoadDict(dict26);
						Vector3 position27 = CasinoGames.CurrentSlotMachine.Position;
						CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position27.X, position27.Y, position27.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict26), "exit_left", 3000.0, -2.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, "exit_left", CasinoGames.LoadDict(dict26), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z, 1148846080, 0);
						CasinoGames.SlotMachineStage = -1;
						CasinoGames.SpinnedReels = false;
						if (CasinoGames.ReelABase != null)
						{
							CasinoGames.ReelABase.Delete();
						}
						if (CasinoGames.ReelBBase != null)
						{
							CasinoGames.ReelBBase.Delete();
						}
						if (CasinoGames.ReelCBase != null)
						{
							CasinoGames.ReelCBase.Delete();
						}
						if (CasinoGames.SlotProp1 != null)
						{
							CasinoGames.SlotProp1.Delete();
						}
						if (CasinoGames.SlotProp2 != null)
						{
							CasinoGames.SlotProp2.Delete();
						}
						if (CasinoGames.SlotProp3 != null)
						{
							CasinoGames.SlotProp3.Delete();
						}
						CasinoGames.SlotProp1 = null;
						CasinoGames.SlotProp2 = null;
						CasinoGames.SlotProp3 = null;
						CasinoGames.FinishedReelA = false;
						CasinoGames.FinishedReelB = false;
						CasinoGames.FinishedReelC = false;
					}
					if (!CasinoGames.SlotInUse && CasinoGames.SlotMachineStage == 0 && (float)CasinoGames.Chips_Amount >= CasinoGames.MinStartBet)
					{
						if (CasinoGames.ReelABase != null)
						{
							CasinoGames.ReelABase.Delete();
						}
						if (CasinoGames.ReelBBase != null)
						{
							CasinoGames.ReelBBase.Delete();
						}
						if (CasinoGames.ReelCBase != null)
						{
							CasinoGames.ReelCBase.Delete();
						}
						if (CasinoGames.SlotProp1 != null)
						{
							CasinoGames.SlotProp1.Delete();
						}
						if (CasinoGames.SlotProp2 != null)
						{
							CasinoGames.SlotProp2.Delete();
						}
						if (CasinoGames.SlotProp3 != null)
						{
							CasinoGames.SlotProp3.Delete();
						}
						CasinoGames.SlotProp1 = null;
						CasinoGames.SlotProp2 = null;
						CasinoGames.SlotProp3 = null;
						Vector3 position28 = Function.Call<Vector3>(Hash.GET_OFFSET_FROM_COORD_AND_HEADING_IN_WORLD_COORDS, slotsClass.Loc.X, slotsClass.Loc.Y, slotsClass.Loc.Z, slotsClass.Heading, -0.115f, 0.047f, 0.906f);
						Vector3 position29 = Function.Call<Vector3>(Hash.GET_OFFSET_FROM_COORD_AND_HEADING_IN_WORLD_COORDS, slotsClass.Loc.X, slotsClass.Loc.Y, slotsClass.Loc.Z, slotsClass.Heading, 0.005f, 0.047f, 0.906f);
						Vector3 position30 = Function.Call<Vector3>(Hash.GET_OFFSET_FROM_COORD_AND_HEADING_IN_WORLD_COORDS, slotsClass.Loc.X, slotsClass.Loc.Y, slotsClass.Loc.Z, slotsClass.Heading, 0.125f, 0.047f, 0.906f);
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_01a"))
						{
							CasinoGames.ReelABase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position28.X, position28.Y, position28.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							CasinoGames.ReelABase.IsPositionFrozen = true;
							CasinoGames.SlotProp1 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_01a_reels"), position28, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							CasinoGames.SlotProp1.IsPositionFrozen = true;
							CasinoGames.SlotProp1.AttachTo(CasinoGames.ReelABase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							CasinoGames.ReelBBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position29.X, position29.Y, position29.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							CasinoGames.ReelABase.IsPositionFrozen = true;
							CasinoGames.SlotProp2 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_01a_reels"), position29, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							CasinoGames.SlotProp2.IsPositionFrozen = true;
							CasinoGames.SlotProp2.AttachTo(CasinoGames.ReelBBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							CasinoGames.ReelCBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position30.X, position30.Y, position30.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							CasinoGames.ReelABase.IsPositionFrozen = true;
							CasinoGames.SlotProp3 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_01a_reels"), position30, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							CasinoGames.SlotProp3.IsPositionFrozen = true;
							CasinoGames.SlotProp3.AttachTo(CasinoGames.ReelCBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_02a"))
						{
							CasinoGames.ReelABase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position28.X, position28.Y, position28.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							CasinoGames.ReelABase.IsPositionFrozen = true;
							CasinoGames.SlotProp1 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_02a_reels"), position28, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							CasinoGames.SlotProp1.IsPositionFrozen = true;
							CasinoGames.SlotProp1.AttachTo(CasinoGames.ReelABase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							CasinoGames.ReelBBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position29.X, position29.Y, position29.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							CasinoGames.ReelABase.IsPositionFrozen = true;
							CasinoGames.SlotProp2 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_02a_reels"), position29, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							CasinoGames.SlotProp2.IsPositionFrozen = true;
							CasinoGames.SlotProp2.AttachTo(CasinoGames.ReelBBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							CasinoGames.ReelCBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position30.X, position30.Y, position30.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							CasinoGames.ReelABase.IsPositionFrozen = true;
							CasinoGames.SlotProp3 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_02a_reels"), position30, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							CasinoGames.SlotProp3.IsPositionFrozen = true;
							CasinoGames.SlotProp3.AttachTo(CasinoGames.ReelCBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_03a"))
						{
							CasinoGames.ReelABase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position28.X, position28.Y, position28.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							CasinoGames.ReelABase.IsPositionFrozen = true;
							CasinoGames.SlotProp1 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_03a_reels"), position28, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							CasinoGames.SlotProp1.IsPositionFrozen = true;
							CasinoGames.SlotProp1.AttachTo(CasinoGames.ReelABase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							CasinoGames.ReelBBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position29.X, position29.Y, position29.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							CasinoGames.ReelABase.IsPositionFrozen = true;
							CasinoGames.SlotProp2 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_03a_reels"), position29, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							CasinoGames.SlotProp2.IsPositionFrozen = true;
							CasinoGames.SlotProp2.AttachTo(CasinoGames.ReelBBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							CasinoGames.ReelCBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position30.X, position30.Y, position30.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							CasinoGames.ReelABase.IsPositionFrozen = true;
							CasinoGames.SlotProp3 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_03a_reels"), position30, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							CasinoGames.SlotProp3.IsPositionFrozen = true;
							CasinoGames.SlotProp3.AttachTo(CasinoGames.ReelCBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_04a"))
						{
							CasinoGames.ReelABase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position28.X, position28.Y, position28.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							CasinoGames.ReelABase.IsPositionFrozen = true;
							CasinoGames.SlotProp1 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_04a_reels"), position28, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							CasinoGames.SlotProp1.IsPositionFrozen = true;
							CasinoGames.SlotProp1.AttachTo(CasinoGames.ReelABase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							CasinoGames.ReelBBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position29.X, position29.Y, position29.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							CasinoGames.ReelABase.IsPositionFrozen = true;
							CasinoGames.SlotProp2 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_04a_reels"), position29, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							CasinoGames.SlotProp2.IsPositionFrozen = true;
							CasinoGames.SlotProp2.AttachTo(CasinoGames.ReelBBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							CasinoGames.ReelCBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position30.X, position30.Y, position30.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							CasinoGames.ReelABase.IsPositionFrozen = true;
							CasinoGames.SlotProp3 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_04a_reels"), position30, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							CasinoGames.SlotProp3.IsPositionFrozen = true;
							CasinoGames.SlotProp3.AttachTo(CasinoGames.ReelCBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_05a"))
						{
							CasinoGames.ReelABase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position28.X, position28.Y, position28.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							CasinoGames.ReelABase.IsPositionFrozen = true;
							CasinoGames.SlotProp1 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_05a_reels"), position28, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							CasinoGames.SlotProp1.IsPositionFrozen = true;
							CasinoGames.SlotProp1.AttachTo(CasinoGames.ReelABase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							CasinoGames.ReelBBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position29.X, position29.Y, position29.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							CasinoGames.ReelABase.IsPositionFrozen = true;
							CasinoGames.SlotProp2 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_05a_reels"), position29, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							CasinoGames.SlotProp2.IsPositionFrozen = true;
							CasinoGames.SlotProp2.AttachTo(CasinoGames.ReelBBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							CasinoGames.ReelCBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position30.X, position30.Y, position30.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							CasinoGames.ReelABase.IsPositionFrozen = true;
							CasinoGames.SlotProp3 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_05a_reels"), position30, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							CasinoGames.SlotProp3.IsPositionFrozen = true;
							CasinoGames.SlotProp3.AttachTo(CasinoGames.ReelCBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_06a"))
						{
							CasinoGames.ReelABase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position28.X, position28.Y, position28.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							CasinoGames.ReelABase.IsPositionFrozen = true;
							CasinoGames.SlotProp1 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_06a_reels"), position28, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							CasinoGames.SlotProp1.IsPositionFrozen = true;
							CasinoGames.SlotProp1.AttachTo(CasinoGames.ReelABase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							CasinoGames.ReelBBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position29.X, position29.Y, position29.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							CasinoGames.ReelABase.IsPositionFrozen = true;
							CasinoGames.SlotProp2 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_06a_reels"), position29, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							CasinoGames.SlotProp2.IsPositionFrozen = true;
							CasinoGames.SlotProp2.AttachTo(CasinoGames.ReelBBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							CasinoGames.ReelCBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position30.X, position30.Y, position30.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							CasinoGames.ReelABase.IsPositionFrozen = true;
							CasinoGames.SlotProp3 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_06a_reels"), position30, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							CasinoGames.SlotProp3.IsPositionFrozen = true;
							CasinoGames.SlotProp3.AttachTo(CasinoGames.ReelCBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_07a"))
						{
							CasinoGames.ReelABase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position28.X, position28.Y, position28.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							CasinoGames.ReelABase.IsPositionFrozen = true;
							CasinoGames.SlotProp1 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_07a_reels"), position28, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							CasinoGames.SlotProp1.IsPositionFrozen = true;
							CasinoGames.SlotProp1.AttachTo(CasinoGames.ReelABase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							CasinoGames.ReelBBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position29.X, position29.Y, position29.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							CasinoGames.ReelABase.IsPositionFrozen = true;
							CasinoGames.SlotProp2 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_07a_reels"), position29, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							CasinoGames.SlotProp2.IsPositionFrozen = true;
							CasinoGames.SlotProp2.AttachTo(CasinoGames.ReelBBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							CasinoGames.ReelCBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position30.X, position30.Y, position30.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							CasinoGames.ReelABase.IsPositionFrozen = true;
							CasinoGames.SlotProp3 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_07a_reels"), position30, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							CasinoGames.SlotProp3.IsPositionFrozen = true;
							CasinoGames.SlotProp3.AttachTo(CasinoGames.ReelCBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
						}
						if (slotsClass.Machine.Equals("vw_prop_casino_slot_08a"))
						{
							CasinoGames.ReelABase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position28.X, position28.Y, position28.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							CasinoGames.ReelABase.IsPositionFrozen = true;
							CasinoGames.SlotProp1 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_08a_reels"), position28, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							CasinoGames.SlotProp1.IsPositionFrozen = true;
							CasinoGames.SlotProp1.AttachTo(CasinoGames.ReelABase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							CasinoGames.ReelBBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position29.X, position29.Y, position29.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							CasinoGames.ReelABase.IsPositionFrozen = true;
							CasinoGames.SlotProp2 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_08a_reels"), position29, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							CasinoGames.SlotProp2.IsPositionFrozen = true;
							CasinoGames.SlotProp2.AttachTo(CasinoGames.ReelBBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
							CasinoGames.ReelCBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position30.X, position30.Y, position30.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
							CasinoGames.ReelABase.IsPositionFrozen = true;
							CasinoGames.SlotProp3 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_08a_reels"), position30, new Vector3(0f, 0f, slotsClass.Heading), false, false);
							CasinoGames.SlotProp3.IsPositionFrozen = true;
							CasinoGames.SlotProp3.AttachTo(CasinoGames.ReelCBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
						}
						Script.Wait(500);
						CasinoGames.CurrentSlotMachine = machineProp;
						CasinoGames.CurrentSlotClass = slotsClass;
						string dict27 = "anim_casino_a@amb@casino@games@slots@male";
						CasinoGames.LoadDict(dict27);
						CasinoGames.LoadDict(dict27);
						Vector3 position31 = CasinoGames.CurrentSlotMachine.Position;
						CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position31.X, position31.Y, position31.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict27), "enter_left", 3000.0, -2.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, "enter_left", CasinoGames.LoadDict(dict27), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z - 180f, 1148846080, 0);
						CasinoGames.SlotMachineStage = 1;
						CasinoGames.SlotInUse = true;
						CasinoGames.BetOne = false;
						CasinoGames.BetMax = false;
						CasinoGames.BetSlot = 0f;
						CasinoGames.SpinnedReels = false;
						CasinoGames.ReelA_SpinAmt = 0f;
						CasinoGames.ReelB_SpinAmt = 0f;
						CasinoGames.ReelC_SpinAmt = 0f;
						CasinoGames.FinishedReelA = false;
						CasinoGames.FinishedReelB = false;
						CasinoGames.FinishedReelC = false;
					}
				}
				if (CasinoGames.SlotInUse)
				{
					if (CasinoGames.SlotMachineStage == -1 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene) >= 1065353216)
					{
						CasinoGames.CurrentSlotMachine = null;
						CasinoGames.SlotInUse = false;
						Game.Player.Character.Task.ClearAll();
						CasinoGames.SlotMachineStage = 0;
					}
					if (CasinoGames.SlotMachineStage == 1)
					{
						if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene) >= 1065353216)
						{
							CasinoGames.SlotMachineStage = 2;
						}
						if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, "anim_casino_a@amb@casino@games@slots@male", "enter_left", 3))
						{
							int num31 = new Random().Next(0, 500);
							if (num31 >= 0)
							{
								int num33 = 100;
							}
							if (num31 >= 100)
							{
								int num34 = 200;
							}
							if (num31 >= 200)
							{
								int num35 = 300;
							}
							if (num31 >= 300)
							{
								int num36 = 400;
							}
							if (num31 >= 400)
							{
								int num37 = 500;
							}
							if (num31 >= 500)
							{
								int num38 = 600;
							}
							CasinoGames.SlotMachineStage = 2;
							string dict28 = "anim_casino_a@amb@casino@games@slots@male";
							CasinoGames.LoadDict(dict28);
							CasinoGames.LoadDict(dict28);
							Vector3 position32 = CasinoGames.CurrentSlotMachine.Position;
							CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position32.X, position32.Y, position32.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict28), "enter_left", 3000.0, -2.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, "enter_left", CasinoGames.LoadDict(dict28), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z - 180f, 1148846080, 0);
						}
					}
					if (CasinoGames.SlotMachineStage == 2 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene) >= 1065353216)
					{
						string text34 = "";
						int num32 = new Random().Next(0, 500);
						if (num32 >= 0 && num32 < 100)
						{
							text34 = "base_idle_a";
						}
						if (num32 >= 100 && num32 < 200)
						{
							text34 = "base_idle_b";
						}
						if (num32 >= 200 && num32 < 300)
						{
							text34 = "base_idle_c";
						}
						if (num32 >= 300 && num32 < 400)
						{
							text34 = "base_idle_d";
						}
						if (num32 >= 400 && num32 < 500)
						{
							text34 = "base_idle_e";
						}
						if (num32 >= 500 && num32 < 600)
						{
							text34 = "base_idle_f";
						}
						CasinoGames.SlotMachineStage = 2;
						string dict29 = "anim_casino_a@amb@casino@games@slots@male";
						CasinoGames.LoadDict(dict29);
						CasinoGames.LoadDict(dict29);
						Vector3 position33 = CasinoGames.CurrentSlotMachine.Position;
						CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position33.X, position33.Y, position33.Z, 0.0, 0.0, machineProp.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, CasinoGames.LoadDict(dict29), text34, 3000.0, -2.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene));
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, 0.0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CasinoGames.SlotLocH[closestSlotMachine].SlotSyncedScene, text34, CasinoGames.LoadDict(dict29), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z - 180f, 1148846080, 0);
					}
				}
			}
			if (!CasinoGames.GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition, CasinoGames.GetDistSign.Less, 2f))
			{
				return;
			}
			Vector3 position34 = Function.Call<Vector3>(Hash.GET_OFFSET_FROM_COORD_AND_HEADING_IN_WORLD_COORDS, slotsClass.Loc.X, slotsClass.Loc.Y, slotsClass.Loc.Z, slotsClass.Heading, -0.115f, 0.047f, 0.906f);
			Vector3 position35 = Function.Call<Vector3>(Hash.GET_OFFSET_FROM_COORD_AND_HEADING_IN_WORLD_COORDS, slotsClass.Loc.X, slotsClass.Loc.Y, slotsClass.Loc.Z, slotsClass.Heading, 0.005f, 0.047f, 0.906f);
			Vector3 position36 = Function.Call<Vector3>(Hash.GET_OFFSET_FROM_COORD_AND_HEADING_IN_WORLD_COORDS, slotsClass.Loc.X, slotsClass.Loc.Y, slotsClass.Loc.Z, slotsClass.Heading, 0.125f, 0.047f, 0.906f);
			if (Game.IsControlPressed(Control.VehicleDuck))
			{
				if (CasinoGames.ReelABase != null)
				{
					CasinoGames.ReelABase.Delete();
				}
				if (CasinoGames.ReelBBase != null)
				{
					CasinoGames.ReelBBase.Delete();
				}
				if (CasinoGames.ReelCBase != null)
				{
					CasinoGames.ReelCBase.Delete();
				}
				World.GetNearbyProps(slotsClass.Loc, 1.35f);
				CasinoGames.CurrentSlotMachine = World.GetClosest(slotsClass.Loc, Array.Empty<Prop>());
				if (CasinoGames.SlotProp1 != null)
				{
					CasinoGames.SlotProp1.Delete();
				}
				if (CasinoGames.SlotProp2 != null)
				{
					CasinoGames.SlotProp2.Delete();
				}
				if (CasinoGames.SlotProp3 != null)
				{
					CasinoGames.SlotProp3.Delete();
				}
				CasinoGames.SlotProp1 = null;
				CasinoGames.SlotProp2 = null;
				CasinoGames.SlotProp3 = null;
				if (slotsClass.Machine.Equals("vw_prop_casino_slot_01a"))
				{
					CasinoGames.ReelABase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position34.X, position34.Y, position34.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
					CasinoGames.ReelABase.IsPositionFrozen = true;
					CasinoGames.SlotProp1 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_01a_reels"), position34, new Vector3(0f, 0f, slotsClass.Heading), false, false);
					CasinoGames.SlotProp1.IsPositionFrozen = true;
					CasinoGames.SlotProp1.AttachTo(CasinoGames.ReelABase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
					CasinoGames.ReelBBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position35.X, position35.Y, position35.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
					CasinoGames.ReelABase.IsPositionFrozen = true;
					CasinoGames.SlotProp2 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_01a_reels"), position35, new Vector3(0f, 0f, slotsClass.Heading), false, false);
					CasinoGames.SlotProp2.IsPositionFrozen = true;
					CasinoGames.SlotProp2.AttachTo(CasinoGames.ReelBBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
					CasinoGames.ReelCBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position36.X, position36.Y, position36.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
					CasinoGames.ReelABase.IsPositionFrozen = true;
					CasinoGames.SlotProp3 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_01a_reels"), position36, new Vector3(0f, 0f, slotsClass.Heading), false, false);
					CasinoGames.SlotProp3.IsPositionFrozen = true;
					CasinoGames.SlotProp3.AttachTo(CasinoGames.ReelCBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
				}
				if (slotsClass.Machine.Equals("vw_prop_casino_slot_02a"))
				{
					CasinoGames.ReelABase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position34.X, position34.Y, position34.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
					CasinoGames.ReelABase.IsPositionFrozen = true;
					CasinoGames.SlotProp1 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_02a_reels"), position34, new Vector3(0f, 0f, slotsClass.Heading), false, false);
					CasinoGames.SlotProp1.IsPositionFrozen = true;
					CasinoGames.SlotProp1.AttachTo(CasinoGames.ReelABase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
					CasinoGames.ReelBBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position35.X, position35.Y, position35.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
					CasinoGames.ReelABase.IsPositionFrozen = true;
					CasinoGames.SlotProp2 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_02a_reels"), position35, new Vector3(0f, 0f, slotsClass.Heading), false, false);
					CasinoGames.SlotProp2.IsPositionFrozen = true;
					CasinoGames.SlotProp2.AttachTo(CasinoGames.ReelBBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
					CasinoGames.ReelCBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position36.X, position36.Y, position36.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
					CasinoGames.ReelABase.IsPositionFrozen = true;
					CasinoGames.SlotProp3 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_02a_reels"), position36, new Vector3(0f, 0f, slotsClass.Heading), false, false);
					CasinoGames.SlotProp3.IsPositionFrozen = true;
					CasinoGames.SlotProp3.AttachTo(CasinoGames.ReelCBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
				}
				if (slotsClass.Machine.Equals("vw_prop_casino_slot_03a"))
				{
					CasinoGames.ReelABase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position34.X, position34.Y, position34.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
					CasinoGames.ReelABase.IsPositionFrozen = true;
					CasinoGames.SlotProp1 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_03a_reels"), position34, new Vector3(0f, 0f, slotsClass.Heading), false, false);
					CasinoGames.SlotProp1.IsPositionFrozen = true;
					CasinoGames.SlotProp1.AttachTo(CasinoGames.ReelABase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
					CasinoGames.ReelBBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position35.X, position35.Y, position35.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
					CasinoGames.ReelABase.IsPositionFrozen = true;
					CasinoGames.SlotProp2 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_03a_reels"), position35, new Vector3(0f, 0f, slotsClass.Heading), false, false);
					CasinoGames.SlotProp2.IsPositionFrozen = true;
					CasinoGames.SlotProp2.AttachTo(CasinoGames.ReelBBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
					CasinoGames.ReelCBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position36.X, position36.Y, position36.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
					CasinoGames.ReelABase.IsPositionFrozen = true;
					CasinoGames.SlotProp3 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_03a_reels"), position36, new Vector3(0f, 0f, slotsClass.Heading), false, false);
					CasinoGames.SlotProp3.IsPositionFrozen = true;
					CasinoGames.SlotProp3.AttachTo(CasinoGames.ReelCBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
				}
				if (slotsClass.Machine.Equals("vw_prop_casino_slot_04a"))
				{
					CasinoGames.ReelABase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position34.X, position34.Y, position34.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
					CasinoGames.ReelABase.IsPositionFrozen = true;
					CasinoGames.SlotProp1 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_04a_reels"), position34, new Vector3(0f, 0f, slotsClass.Heading), false, false);
					CasinoGames.SlotProp1.IsPositionFrozen = true;
					CasinoGames.SlotProp1.AttachTo(CasinoGames.ReelABase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
					CasinoGames.ReelBBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position35.X, position35.Y, position35.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
					CasinoGames.ReelABase.IsPositionFrozen = true;
					CasinoGames.SlotProp2 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_04a_reels"), position35, new Vector3(0f, 0f, slotsClass.Heading), false, false);
					CasinoGames.SlotProp2.IsPositionFrozen = true;
					CasinoGames.SlotProp2.AttachTo(CasinoGames.ReelBBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
					CasinoGames.ReelCBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position36.X, position36.Y, position36.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
					CasinoGames.ReelABase.IsPositionFrozen = true;
					CasinoGames.SlotProp3 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_04a_reels"), position36, new Vector3(0f, 0f, slotsClass.Heading), false, false);
					CasinoGames.SlotProp3.IsPositionFrozen = true;
					CasinoGames.SlotProp3.AttachTo(CasinoGames.ReelCBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
				}
				if (slotsClass.Machine.Equals("vw_prop_casino_slot_05a"))
				{
					CasinoGames.ReelABase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position34.X, position34.Y, position34.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
					CasinoGames.ReelABase.IsPositionFrozen = true;
					CasinoGames.SlotProp1 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_05a_reels"), position34, new Vector3(0f, 0f, slotsClass.Heading), false, false);
					CasinoGames.SlotProp1.IsPositionFrozen = true;
					CasinoGames.SlotProp1.AttachTo(CasinoGames.ReelABase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
					CasinoGames.ReelBBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position35.X, position35.Y, position35.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
					CasinoGames.ReelABase.IsPositionFrozen = true;
					CasinoGames.SlotProp2 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_05a_reels"), position35, new Vector3(0f, 0f, slotsClass.Heading), false, false);
					CasinoGames.SlotProp2.IsPositionFrozen = true;
					CasinoGames.SlotProp2.AttachTo(CasinoGames.ReelBBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
					CasinoGames.ReelCBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position36.X, position36.Y, position36.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
					CasinoGames.ReelABase.IsPositionFrozen = true;
					CasinoGames.SlotProp3 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_05a_reels"), position36, new Vector3(0f, 0f, slotsClass.Heading), false, false);
					CasinoGames.SlotProp3.IsPositionFrozen = true;
					CasinoGames.SlotProp3.AttachTo(CasinoGames.ReelCBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
				}
				if (slotsClass.Machine.Equals("vw_prop_casino_slot_06a"))
				{
					CasinoGames.ReelABase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position34.X, position34.Y, position34.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
					CasinoGames.ReelABase.IsPositionFrozen = true;
					CasinoGames.SlotProp1 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_06a_reels"), position34, new Vector3(0f, 0f, slotsClass.Heading), false, false);
					CasinoGames.SlotProp1.IsPositionFrozen = true;
					CasinoGames.SlotProp1.AttachTo(CasinoGames.ReelABase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
					CasinoGames.ReelBBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position35.X, position35.Y, position35.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
					CasinoGames.ReelABase.IsPositionFrozen = true;
					CasinoGames.SlotProp2 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_06a_reels"), position35, new Vector3(0f, 0f, slotsClass.Heading), false, false);
					CasinoGames.SlotProp2.IsPositionFrozen = true;
					CasinoGames.SlotProp2.AttachTo(CasinoGames.ReelBBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
					CasinoGames.ReelCBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position36.X, position36.Y, position36.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
					CasinoGames.ReelABase.IsPositionFrozen = true;
					CasinoGames.SlotProp3 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_06a_reels"), position36, new Vector3(0f, 0f, slotsClass.Heading), false, false);
					CasinoGames.SlotProp3.IsPositionFrozen = true;
					CasinoGames.SlotProp3.AttachTo(CasinoGames.ReelCBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
				}
				if (slotsClass.Machine.Equals("vw_prop_casino_slot_07a"))
				{
					CasinoGames.ReelABase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position34.X, position34.Y, position34.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
					CasinoGames.ReelABase.IsPositionFrozen = true;
					CasinoGames.SlotProp1 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_07a_reels"), position34, new Vector3(0f, 0f, slotsClass.Heading), false, false);
					CasinoGames.SlotProp1.IsPositionFrozen = true;
					CasinoGames.SlotProp1.AttachTo(CasinoGames.ReelABase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
					CasinoGames.ReelBBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position35.X, position35.Y, position35.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
					CasinoGames.ReelABase.IsPositionFrozen = true;
					CasinoGames.SlotProp2 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_07a_reels"), position35, new Vector3(0f, 0f, slotsClass.Heading), false, false);
					CasinoGames.SlotProp2.IsPositionFrozen = true;
					CasinoGames.SlotProp2.AttachTo(CasinoGames.ReelBBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
					CasinoGames.ReelCBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position36.X, position36.Y, position36.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
					CasinoGames.ReelABase.IsPositionFrozen = true;
					CasinoGames.SlotProp3 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_07a_reels"), position36, new Vector3(0f, 0f, slotsClass.Heading), false, false);
					CasinoGames.SlotProp3.IsPositionFrozen = true;
					CasinoGames.SlotProp3.AttachTo(CasinoGames.ReelCBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
				}
				if (slotsClass.Machine.Equals("vw_prop_casino_slot_08a"))
				{
					CasinoGames.ReelABase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position34.X, position34.Y, position34.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
					CasinoGames.ReelABase.IsPositionFrozen = true;
					CasinoGames.SlotProp1 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_08a_reels"), position34, new Vector3(0f, 0f, slotsClass.Heading), false, false);
					CasinoGames.SlotProp1.IsPositionFrozen = true;
					CasinoGames.SlotProp1.AttachTo(CasinoGames.ReelABase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
					CasinoGames.ReelBBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position35.X, position35.Y, position35.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
					CasinoGames.ReelABase.IsPositionFrozen = true;
					CasinoGames.SlotProp2 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_08a_reels"), position35, new Vector3(0f, 0f, slotsClass.Heading), false, false);
					CasinoGames.SlotProp2.IsPositionFrozen = true;
					CasinoGames.SlotProp2.AttachTo(CasinoGames.ReelBBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
					CasinoGames.ReelCBase = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_plaq_10kdollar_x1"), new Vector3(position36.X, position36.Y, position36.Z + 0.2f), new Vector3(0f, 90f, slotsClass.Heading - 90f), false, false);
					CasinoGames.ReelABase.IsPositionFrozen = true;
					CasinoGames.SlotProp3 = GlobalHelper.CreateProp(CasinoGames.RequestModel("vw_prop_casino_slot_08a_reels"), position36, new Vector3(0f, 0f, slotsClass.Heading), false, false);
					CasinoGames.SlotProp3.IsPositionFrozen = true;
					CasinoGames.SlotProp3.AttachTo(CasinoGames.ReelCBase.Bones[1], new Vector3(0f, 0f, 0f), new Vector3(90f, 90f, 0f));
				}
			}
		}
	}
}
