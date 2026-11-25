using System;
using System.Collections.Generic;
using GTA;
using GTA.Native;
using GTA.UI;
using LemonUI;

namespace GTAO_DiamondCasino
{
	internal class InsideTrack : Script
	{
		public static bool RunMethodNow()
		{
			bool result = true;
			if (!CasinoGames.RunningInsideTrackScaleform)
			{
				result = false;
				_Debug.InsideTrack = 0;
			}
			return result;
		}

		public InsideTrack()
		{
			if (_Debug.InsideTrack != -1)
			{
				base.Tick += OnTick;
			}
		}

		public static void OnTick(object sender, EventArgs e)
		{
			if (!CasinoGames.RunningInsideTrackScaleform)
			{
				_Debug.InsideTrack = 0;
				return;
			}
			_Debug.InsideTrack = 1;
			if (CasinoGames.RunningInsideTrackScaleform)
			{
				if (CasinoGames.GUIBIX == 15)
				{
					CasinoGames.Screen = 9f;
					CasinoGames.frame = 1;
					CasinoGames.GUIBIX = -1;
				}
				if (CasinoGames.GUIBIX == 1)
				{
					CasinoGames.Screen = 1f;
					CasinoGames.frame = 1;
					CasinoGames.GUIBIX = -1;
				}
				if (CasinoGames.GUIBIX == 2)
				{
					CasinoGames.SelectedHorse = 1;
					CasinoGames.Screen = 3f;
					CasinoGames.frame = 1;
					CasinoGames.GUIBIX = -1;
				}
				if (CasinoGames.GUIBIX == 3)
				{
					CasinoGames.SelectedHorse = 2;
					CasinoGames.Screen = 3f;
					CasinoGames.frame = 1;
					CasinoGames.GUIBIX = -1;
				}
				if (CasinoGames.GUIBIX == 4)
				{
					CasinoGames.SelectedHorse = 3;
					CasinoGames.Screen = 3f;
					CasinoGames.frame = 1;
					CasinoGames.GUIBIX = -1;
				}
				if (CasinoGames.GUIBIX == 5)
				{
					CasinoGames.SelectedHorse = 4;
					CasinoGames.Screen = 3f;
					CasinoGames.frame = 1;
					CasinoGames.GUIBIX = -1;
				}
				if (CasinoGames.GUIBIX == 6)
				{
					CasinoGames.SelectedHorse = 5;
					CasinoGames.Screen = 3f;
					CasinoGames.frame = 1;
					CasinoGames.GUIBIX = -1;
				}
				if (CasinoGames.GUIBIX == 7)
				{
					CasinoGames.SelectedHorse = 6;
					CasinoGames.Screen = 3f;
					CasinoGames.frame = 1;
					CasinoGames.GUIBIX = -1;
				}
				if (CasinoGames.GUIBIX == 12)
				{
					CasinoGames.Screen = 0f;
					CasinoGames.frame = 1;
					CasinoGames.GUIBIX = -1;
				}
				if (CasinoGames.GUIBIX == 9)
				{
					if (CasinoGames.RacingBet > 100)
					{
						CasinoGames.RacingBet -= 100;
					}
					CasinoGames.frame = 1;
					CasinoGames.GUIBIX = -1;
				}
				if (CasinoGames.GUIBIX == 8)
				{
					if (CasinoGames.RacingBet < 10000)
					{
						CasinoGames.RacingBet += 100;
					}
					CasinoGames.frame = 1;
					CasinoGames.GUIBIX = -1;
				}
				if (CasinoGames.GUIBIX == 13)
				{
					if (CasinoGames.Screen == 7f)
					{
						Random random = new Random();
						List<int> list = new List<int>();
						List<int> list2 = new List<int>();
						List<int> list3 = new List<int>();
						string[] separator = new string[1] { "," };
						string[] separator2 = new string[1] { "/" };
						int index = random.Next(0, CasinoGames.HorseNamesAndOdds.Count);
						int iParam = random.Next(0, 100);
						string[] array = CasinoGames.HorseNamesAndOdds[index].Split(separator, StringSplitOptions.None);
						CasinoGames.func_6397(iParam);
						CasinoGames.GUIscaleform.CallFunction("SET_HORSE", 1, array[0], array[1], CasinoGames.Horse_Color_secondaryColour, CasinoGames.Horse_Color_horseColour, CasinoGames.Horse_Color_maneColour);
						CasinoGames.GUIscaleform.CallFunction("SET_DETAIL_HORSE", 1);
						string[] array2 = array[1].Split(separator2, StringSplitOptions.None);
						CasinoGames.Horse1Entries = 1000f / float.Parse(array2[0]);
						list3.Add(int.Parse(array2[0]));
						CasinoGames.Horse1Odds = int.Parse(array2[0]);
						index = random.Next(0, CasinoGames.HorseNamesAndOdds.Count);
						iParam = random.Next(0, 100);
						while (list.Contains(index))
						{
							index = random.Next(0, CasinoGames.HorseNamesAndOdds.Count);
						}
						while (list2.Contains(iParam))
						{
							iParam = random.Next(0, 100);
						}
						array = CasinoGames.HorseNamesAndOdds[index].Split(separator, StringSplitOptions.None);
						array2 = array[1].Split(separator2, StringSplitOptions.None);
						while (list3.Contains(int.Parse(array2[0])))
						{
							index = random.Next(0, CasinoGames.HorseNamesAndOdds.Count);
							iParam = random.Next(0, 100);
							while (list.Contains(index))
							{
								index = random.Next(0, CasinoGames.HorseNamesAndOdds.Count);
							}
							while (list2.Contains(iParam))
							{
								iParam = random.Next(0, 100);
							}
							array = CasinoGames.HorseNamesAndOdds[index].Split(separator, StringSplitOptions.None);
							array2 = array[1].Split(separator2, StringSplitOptions.None);
						}
						CasinoGames.func_6397(iParam);
						CasinoGames.GUIscaleform.CallFunction("SET_HORSE", 2, array[0], array[1], CasinoGames.Horse_Color_secondaryColour, CasinoGames.Horse_Color_horseColour, CasinoGames.Horse_Color_maneColour);
						CasinoGames.GUIscaleform.CallFunction("SET_DETAIL_HORSE", 2);
						CasinoGames.Horse2Entries = 1000f / float.Parse(array2[0]);
						CasinoGames.Horse2Odds = int.Parse(array2[0]);
						index = random.Next(0, CasinoGames.HorseNamesAndOdds.Count);
						iParam = random.Next(0, 100);
						while (list.Contains(index))
						{
							index = random.Next(0, CasinoGames.HorseNamesAndOdds.Count);
						}
						while (list2.Contains(iParam))
						{
							iParam = random.Next(0, 100);
						}
						array = CasinoGames.HorseNamesAndOdds[index].Split(separator, StringSplitOptions.None);
						array2 = array[1].Split(separator2, StringSplitOptions.None);
						while (list3.Contains(int.Parse(array2[0])))
						{
							index = random.Next(0, CasinoGames.HorseNamesAndOdds.Count);
							iParam = random.Next(0, 100);
							while (list.Contains(index))
							{
								index = random.Next(0, CasinoGames.HorseNamesAndOdds.Count);
							}
							while (list2.Contains(iParam))
							{
								iParam = random.Next(0, 100);
							}
							array = CasinoGames.HorseNamesAndOdds[index].Split(separator, StringSplitOptions.None);
							array2 = array[1].Split(separator2, StringSplitOptions.None);
						}
						CasinoGames.func_6397(iParam);
						CasinoGames.GUIscaleform.CallFunction("SET_HORSE", 3, array[0], array[1], CasinoGames.Horse_Color_secondaryColour, CasinoGames.Horse_Color_horseColour, CasinoGames.Horse_Color_maneColour);
						CasinoGames.GUIscaleform.CallFunction("SET_DETAIL_HORSE", 3);
						CasinoGames.Horse3Entries = 1000f / float.Parse(array2[0]);
						CasinoGames.Horse3Odds = int.Parse(array2[0]);
						index = random.Next(0, CasinoGames.HorseNamesAndOdds.Count);
						iParam = random.Next(0, 100);
						while (list.Contains(index))
						{
							index = random.Next(0, CasinoGames.HorseNamesAndOdds.Count);
						}
						while (list2.Contains(iParam))
						{
							iParam = random.Next(0, 100);
						}
						array = CasinoGames.HorseNamesAndOdds[index].Split(separator, StringSplitOptions.None);
						array2 = array[1].Split(separator2, StringSplitOptions.None);
						while (list3.Contains(int.Parse(array2[0])))
						{
							index = random.Next(0, CasinoGames.HorseNamesAndOdds.Count);
							iParam = random.Next(0, 100);
							while (list.Contains(index))
							{
								index = random.Next(0, CasinoGames.HorseNamesAndOdds.Count);
							}
							while (list2.Contains(iParam))
							{
								iParam = random.Next(0, 100);
							}
							array = CasinoGames.HorseNamesAndOdds[index].Split(separator, StringSplitOptions.None);
							array2 = array[1].Split(separator2, StringSplitOptions.None);
						}
						CasinoGames.func_6397(iParam);
						CasinoGames.GUIscaleform.CallFunction("SET_HORSE", 4, array[0], array[1], CasinoGames.Horse_Color_secondaryColour, CasinoGames.Horse_Color_horseColour, CasinoGames.Horse_Color_maneColour);
						CasinoGames.GUIscaleform.CallFunction("SET_DETAIL_HORSE", 4);
						CasinoGames.Horse4Entries = 1000f / float.Parse(array2[0]);
						CasinoGames.Horse4Odds = int.Parse(array2[0]);
						index = random.Next(0, CasinoGames.HorseNamesAndOdds.Count);
						iParam = random.Next(0, 100);
						while (list.Contains(index))
						{
							index = random.Next(0, CasinoGames.HorseNamesAndOdds.Count);
						}
						while (list2.Contains(iParam))
						{
							iParam = random.Next(0, 100);
						}
						array = CasinoGames.HorseNamesAndOdds[index].Split(separator, StringSplitOptions.None);
						array2 = array[1].Split(separator2, StringSplitOptions.None);
						while (list3.Contains(int.Parse(array2[0])))
						{
							index = random.Next(0, CasinoGames.HorseNamesAndOdds.Count);
							iParam = random.Next(0, 100);
							while (list.Contains(index))
							{
								index = random.Next(0, CasinoGames.HorseNamesAndOdds.Count);
							}
							while (list2.Contains(iParam))
							{
								iParam = random.Next(0, 100);
							}
							array = CasinoGames.HorseNamesAndOdds[index].Split(separator, StringSplitOptions.None);
							array2 = array[1].Split(separator2, StringSplitOptions.None);
						}
						CasinoGames.func_6397(iParam);
						CasinoGames.GUIscaleform.CallFunction("SET_HORSE", 5, array[0], array[1], CasinoGames.Horse_Color_secondaryColour, CasinoGames.Horse_Color_horseColour, CasinoGames.Horse_Color_maneColour);
						CasinoGames.GUIscaleform.CallFunction("SET_DETAIL_HORSE", 5);
						CasinoGames.Horse5Entries = 1000f / float.Parse(array2[0]);
						CasinoGames.Horse5Odds = int.Parse(array2[0]);
						index = random.Next(0, CasinoGames.HorseNamesAndOdds.Count);
						iParam = random.Next(0, 100);
						while (list.Contains(index))
						{
							index = random.Next(0, CasinoGames.HorseNamesAndOdds.Count);
						}
						while (list2.Contains(iParam))
						{
							iParam = random.Next(0, 100);
						}
						array = CasinoGames.HorseNamesAndOdds[index].Split(separator, StringSplitOptions.None);
						array2 = array[1].Split(separator2, StringSplitOptions.None);
						while (list3.Contains(int.Parse(array2[0])))
						{
							index = random.Next(0, CasinoGames.HorseNamesAndOdds.Count);
							iParam = random.Next(0, 100);
							while (list.Contains(index))
							{
								index = random.Next(0, CasinoGames.HorseNamesAndOdds.Count);
							}
							while (list2.Contains(iParam))
							{
								iParam = random.Next(0, 100);
							}
							array = CasinoGames.HorseNamesAndOdds[index].Split(separator, StringSplitOptions.None);
							array2 = array[1].Split(separator2, StringSplitOptions.None);
						}
						CasinoGames.func_6397(iParam);
						CasinoGames.GUIscaleform.CallFunction("SET_HORSE", 6, array[0], array[1], CasinoGames.Horse_Color_secondaryColour, CasinoGames.Horse_Color_horseColour, CasinoGames.Horse_Color_maneColour);
						CasinoGames.GUIscaleform.CallFunction("SET_DETAIL_HORSE", 6);
						CasinoGames.Horse6Entries = 1000f / float.Parse(array2[0]);
						CasinoGames.Horse6Odds = int.Parse(array2[0]);
						CasinoGames.GUIscaleform.CallFunction("SET_COUNTDOWN", 0);
						CasinoGames.HorseRacingStage = 1;
					}
					CasinoGames.Screen = 1f;
					CasinoGames.RacingBet = 100;
					CasinoGames.frame = 1;
					CasinoGames.GUIBIX = -1;
				}
				if (CasinoGames.frame == 0)
				{
					CasinoGames.RacingBet = 100;
					string scaleformID = "HORSE_RACING_CONSOLE";
					if (Function.Call<bool>(Hash.GET_IS_WIDESCREEN))
					{
						CasinoGames.GUIscaleform = new Scaleform(scaleformID);
					}
					else
					{
						CasinoGames.GUIscaleform = new Scaleform(scaleformID);
					}
				}
				while (!CasinoGames.GUIscaleform.IsLoaded)
				{
					Script.Wait(0);
				}
				if (CasinoGames.GUIscaleform.IsLoaded)
				{
					if (CasinoGames.frame == 0)
					{
						CasinoGames.frame = 1;
					}
					if (CasinoGames.frame == 1)
					{
						CasinoGames.frame = 2;
					}
					if (CasinoGames.frame == 2)
					{
						Function.Call(Hash.ENABLE_CONTROL_ACTION, new InputArgument[3] { 2, 197, 1 });
						Function.Call(Hash.ENABLE_CONTROL_ACTION, new InputArgument[3] { 2, 198, 1 });
						int controlValue = Game.GetControlValue(Control.FrontendRightAxisX);
						int controlValue2 = Game.GetControlValue(Control.FrontendRightAxisY);
						bool flag = Game.IsControlPressed(Control.CursorScrollDown) || Game.IsControlPressed(Control.CursorScrollDown);
						if (!flag)
						{
							flag = Game.IsControlPressed(Control.CursorScrollUp) || Game.IsControlPressed(Control.CursorScrollUp);
						}
						CasinoGames.GUIscaleform.CallFunction("SET_ANALOG_STICK_INPUT", false, controlValue, controlValue2, flag);
						CasinoGames.GUIscaleform.CallFunction("SHOW_SCREEN", (int)CasinoGames.Screen);
						if (CasinoGames.HorseRacingStage == 0)
						{
							Random random2 = new Random();
							List<int> list4 = new List<int>();
							List<int> list5 = new List<int>();
							List<int> list6 = new List<int>();
							string[] separator3 = new string[1] { "," };
							string[] separator4 = new string[1] { "/" };
							int index2 = random2.Next(0, CasinoGames.HorseNamesAndOdds.Count);
							int iParam2 = random2.Next(0, 100);
							string[] array3 = CasinoGames.HorseNamesAndOdds[index2].Split(separator3, StringSplitOptions.None);
							CasinoGames.func_6397(iParam2);
							CasinoGames.GUIscaleform.CallFunction("SET_HORSE", 1, array3[0], array3[1], CasinoGames.Horse_Color_secondaryColour, CasinoGames.Horse_Color_horseColour, CasinoGames.Horse_Color_maneColour);
							CasinoGames.GUIscaleform.CallFunction("SET_DETAIL_HORSE", 1);
							string[] array4 = array3[1].Split(separator4, StringSplitOptions.None);
							CasinoGames.Horse1Entries = 1000f / float.Parse(array4[0]);
							list6.Add(int.Parse(array4[0]));
							CasinoGames.Horse1Odds = int.Parse(array4[0]);
							index2 = random2.Next(0, CasinoGames.HorseNamesAndOdds.Count);
							iParam2 = random2.Next(0, 100);
							while (list4.Contains(index2))
							{
								index2 = random2.Next(0, CasinoGames.HorseNamesAndOdds.Count);
							}
							while (list5.Contains(iParam2))
							{
								iParam2 = random2.Next(0, 100);
							}
							array3 = CasinoGames.HorseNamesAndOdds[index2].Split(separator3, StringSplitOptions.None);
							array4 = array3[1].Split(separator4, StringSplitOptions.None);
							while (list6.Contains(int.Parse(array4[0])))
							{
								index2 = random2.Next(0, CasinoGames.HorseNamesAndOdds.Count);
								iParam2 = random2.Next(0, 100);
								while (list4.Contains(index2))
								{
									index2 = random2.Next(0, CasinoGames.HorseNamesAndOdds.Count);
								}
								while (list5.Contains(iParam2))
								{
									iParam2 = random2.Next(0, 100);
								}
								array3 = CasinoGames.HorseNamesAndOdds[index2].Split(separator3, StringSplitOptions.None);
								array4 = array3[1].Split(separator4, StringSplitOptions.None);
							}
							CasinoGames.func_6397(iParam2);
							CasinoGames.GUIscaleform.CallFunction("SET_HORSE", 2, array3[0], array3[1], CasinoGames.Horse_Color_secondaryColour, CasinoGames.Horse_Color_horseColour, CasinoGames.Horse_Color_maneColour);
							CasinoGames.GUIscaleform.CallFunction("SET_DETAIL_HORSE", 2);
							CasinoGames.Horse2Entries = 1000f / float.Parse(array4[0]);
							CasinoGames.Horse2Odds = int.Parse(array4[0]);
							index2 = random2.Next(0, CasinoGames.HorseNamesAndOdds.Count);
							iParam2 = random2.Next(0, 100);
							while (list4.Contains(index2))
							{
								index2 = random2.Next(0, CasinoGames.HorseNamesAndOdds.Count);
							}
							while (list5.Contains(iParam2))
							{
								iParam2 = random2.Next(0, 100);
							}
							array3 = CasinoGames.HorseNamesAndOdds[index2].Split(separator3, StringSplitOptions.None);
							array4 = array3[1].Split(separator4, StringSplitOptions.None);
							while (list6.Contains(int.Parse(array4[0])))
							{
								index2 = random2.Next(0, CasinoGames.HorseNamesAndOdds.Count);
								iParam2 = random2.Next(0, 100);
								while (list4.Contains(index2))
								{
									index2 = random2.Next(0, CasinoGames.HorseNamesAndOdds.Count);
								}
								while (list5.Contains(iParam2))
								{
									iParam2 = random2.Next(0, 100);
								}
								array3 = CasinoGames.HorseNamesAndOdds[index2].Split(separator3, StringSplitOptions.None);
								array4 = array3[1].Split(separator4, StringSplitOptions.None);
							}
							CasinoGames.func_6397(iParam2);
							CasinoGames.GUIscaleform.CallFunction("SET_HORSE", 3, array3[0], array3[1], CasinoGames.Horse_Color_secondaryColour, CasinoGames.Horse_Color_horseColour, CasinoGames.Horse_Color_maneColour);
							CasinoGames.GUIscaleform.CallFunction("SET_DETAIL_HORSE", 3);
							CasinoGames.Horse3Entries = 1000f / float.Parse(array4[0]);
							CasinoGames.Horse3Odds = int.Parse(array4[0]);
							index2 = random2.Next(0, CasinoGames.HorseNamesAndOdds.Count);
							iParam2 = random2.Next(0, 100);
							while (list4.Contains(index2))
							{
								index2 = random2.Next(0, CasinoGames.HorseNamesAndOdds.Count);
							}
							iParam2 = random2.Next(0, 100);
							array3 = CasinoGames.HorseNamesAndOdds[index2].Split(separator3, StringSplitOptions.None);
							array4 = array3[1].Split(separator4, StringSplitOptions.None);
							while (list6.Contains(int.Parse(array4[0])))
							{
								index2 = random2.Next(0, CasinoGames.HorseNamesAndOdds.Count);
								iParam2 = random2.Next(0, 100);
								while (list4.Contains(index2))
								{
									index2 = random2.Next(0, CasinoGames.HorseNamesAndOdds.Count);
								}
								while (list5.Contains(iParam2))
								{
									iParam2 = random2.Next(0, 100);
								}
								array3 = CasinoGames.HorseNamesAndOdds[index2].Split(separator3, StringSplitOptions.None);
								array4 = array3[1].Split(separator4, StringSplitOptions.None);
							}
							CasinoGames.func_6397(iParam2);
							CasinoGames.GUIscaleform.CallFunction("SET_HORSE", 4, array3[0], array3[1], CasinoGames.Horse_Color_secondaryColour, CasinoGames.Horse_Color_horseColour, CasinoGames.Horse_Color_maneColour);
							CasinoGames.GUIscaleform.CallFunction("SET_DETAIL_HORSE", 4);
							CasinoGames.Horse4Entries = 1000f / float.Parse(array4[0]);
							CasinoGames.Horse4Odds = int.Parse(array4[0]);
							index2 = random2.Next(0, CasinoGames.HorseNamesAndOdds.Count);
							iParam2 = random2.Next(0, 100);
							while (list4.Contains(index2))
							{
								index2 = random2.Next(0, CasinoGames.HorseNamesAndOdds.Count);
							}
							while (list5.Contains(iParam2))
							{
								iParam2 = random2.Next(0, 100);
							}
							array3 = CasinoGames.HorseNamesAndOdds[index2].Split(separator3, StringSplitOptions.None);
							array4 = array3[1].Split(separator4, StringSplitOptions.None);
							while (list6.Contains(int.Parse(array4[0])))
							{
								index2 = random2.Next(0, CasinoGames.HorseNamesAndOdds.Count);
								iParam2 = random2.Next(0, 100);
								while (list4.Contains(index2))
								{
									index2 = random2.Next(0, CasinoGames.HorseNamesAndOdds.Count);
								}
								while (list5.Contains(iParam2))
								{
									iParam2 = random2.Next(0, 100);
								}
								array3 = CasinoGames.HorseNamesAndOdds[index2].Split(separator3, StringSplitOptions.None);
								array4 = array3[1].Split(separator4, StringSplitOptions.None);
							}
							CasinoGames.func_6397(iParam2);
							CasinoGames.GUIscaleform.CallFunction("SET_HORSE", 5, array3[0], array3[1], CasinoGames.Horse_Color_secondaryColour, CasinoGames.Horse_Color_horseColour, CasinoGames.Horse_Color_maneColour);
							CasinoGames.GUIscaleform.CallFunction("SET_DETAIL_HORSE", 5);
							CasinoGames.Horse5Entries = 1000f / float.Parse(array4[0]);
							CasinoGames.Horse5Odds = int.Parse(array4[0]);
							index2 = random2.Next(0, CasinoGames.HorseNamesAndOdds.Count);
							iParam2 = random2.Next(0, 100);
							while (list4.Contains(index2))
							{
								index2 = random2.Next(0, CasinoGames.HorseNamesAndOdds.Count);
							}
							while (list5.Contains(iParam2))
							{
								iParam2 = random2.Next(0, 100);
							}
							array3 = CasinoGames.HorseNamesAndOdds[index2].Split(separator3, StringSplitOptions.None);
							array4 = array3[1].Split(separator4, StringSplitOptions.None);
							while (list6.Contains(int.Parse(array4[0])))
							{
								index2 = random2.Next(0, CasinoGames.HorseNamesAndOdds.Count);
								iParam2 = random2.Next(0, 100);
								while (list4.Contains(index2))
								{
									index2 = random2.Next(0, CasinoGames.HorseNamesAndOdds.Count);
								}
								while (list5.Contains(iParam2))
								{
									iParam2 = random2.Next(0, 100);
								}
								array3 = CasinoGames.HorseNamesAndOdds[index2].Split(separator3, StringSplitOptions.None);
								array4 = array3[1].Split(separator4, StringSplitOptions.None);
							}
							CasinoGames.func_6397(iParam2);
							CasinoGames.GUIscaleform.CallFunction("SET_HORSE", 6, array3[0], array3[1], CasinoGames.Horse_Color_secondaryColour, CasinoGames.Horse_Color_horseColour, CasinoGames.Horse_Color_maneColour);
							CasinoGames.GUIscaleform.CallFunction("SET_DETAIL_HORSE", 6);
							CasinoGames.Horse6Entries = 1000f / float.Parse(array4[0]);
							CasinoGames.Horse6Odds = int.Parse(array4[0]);
							CasinoGames.GUIscaleform.CallFunction("SET_COUNTDOWN", 0);
							CasinoGames.HorseRacingStage = 1;
						}
						if (!CasinoGames.HorsesAreRacing)
						{
							if (CasinoGames.SelectedHorse == 1)
							{
								CasinoGames.HorseRacingPayout = CasinoGames.RacingBet * CasinoGames.Horse1Odds;
							}
							if (CasinoGames.SelectedHorse == 2)
							{
								CasinoGames.HorseRacingPayout = CasinoGames.RacingBet * CasinoGames.Horse2Odds;
							}
							if (CasinoGames.SelectedHorse == 3)
							{
								CasinoGames.HorseRacingPayout = CasinoGames.RacingBet * CasinoGames.Horse3Odds;
							}
							if (CasinoGames.SelectedHorse == 4)
							{
								CasinoGames.HorseRacingPayout = CasinoGames.RacingBet * CasinoGames.Horse4Odds;
							}
							if (CasinoGames.SelectedHorse == 5)
							{
								CasinoGames.HorseRacingPayout = CasinoGames.RacingBet * CasinoGames.Horse5Odds;
							}
							if (CasinoGames.SelectedHorse == 6)
							{
								CasinoGames.HorseRacingPayout = CasinoGames.RacingBet * CasinoGames.Horse6Odds;
							}
							if (CasinoGames.Screen == 3f)
							{
								if (Game.IsControlPressed(Control.PhoneRight) && CasinoGames.RacingBet < 10000)
								{
									CasinoGames.RacingBet += 100;
									CasinoGames.frame = 1;
								}
								if (Game.IsControlPressed(Control.PhoneLeft) && CasinoGames.RacingBet > 100)
								{
									CasinoGames.RacingBet -= 100;
									CasinoGames.frame = 1;
								}
							}
							CasinoGames.GUIscaleform.CallFunction("SET_BETTING_VALUES", CasinoGames.SelectedHorse, CasinoGames.RacingBet, CasinoGames.Chips_Amount, CasinoGames.HorseRacingPayout);
							CasinoGames.GUIscaleform.CallFunction("SET_BETTING_ENABLED", true);
						}
						if (CasinoGames.GUIBIX == 10 && CasinoGames.Chips_Amount >= CasinoGames.RacingBet)
						{
							CasinoGames.Chips_Amount -= CasinoGames.RacingBet;
							Class1.Config.SetValue("Setup", "Chips_Amount", CasinoGames.Chips_Amount);
							Class1.Config.Save();
							CasinoGames.frame = 1;
							CasinoGames.GUIBIX = -1;
							Random random3 = new Random();
							CasinoGames.GUIscaleform.CallFunction("SET_RACE_TYPE", true);
							CasinoGames.GUIscaleform.CallFunction("SET_BETTING_ENABLED", false);
							CasinoGames.GUIscaleform.CallFunction("SET_MAIN_EVENT_IN_PROGRESS", true);
							List<int> list7 = new List<int>();
							list7.Add(718551);
							list7.Add(775927);
							list7.Add(816525);
							list7.Add(832727);
							list7.Add(929760);
							list7.Add(1037876);
							list7.Add(1089104);
							list7.Add(1153632);
							list7.Add(1246272);
							list7.Add(1306892);
							list7.Add(1415152);
							list7.Add(1443682);
							list7.Add(1495368);
							list7.Add(1579808);
							list7.Add(1615874);
							list7.Add(1696559);
							list7.Add(1734873);
							List<int> list8 = new List<int>();
							for (int i = 0; i < (int)CasinoGames.Horse1Entries; i++)
							{
								list8.Add(1);
							}
							for (int j = 0; j < (int)CasinoGames.Horse2Entries; j++)
							{
								list8.Add(2);
							}
							for (int k = 0; k < (int)CasinoGames.Horse3Entries; k++)
							{
								list8.Add(3);
							}
							for (int l = 0; l < (int)CasinoGames.Horse4Entries; l++)
							{
								list8.Add(4);
							}
							for (int m = 0; m < (int)CasinoGames.Horse5Entries; m++)
							{
								list8.Add(5);
							}
							for (int n = 0; n < (int)CasinoGames.Horse6Entries; n++)
							{
								list8.Add(6);
							}
							CasinoGames.Horse1Place = list8[random3.Next(0, list8.Count)];
							list8.RemoveAll((int item) => item == CasinoGames.Horse1Place);
							CasinoGames.Horse2Place = list8[random3.Next(0, list8.Count)];
							list8.RemoveAll((int item) => item == CasinoGames.Horse2Place);
							CasinoGames.Horse3Place = list8[random3.Next(0, list8.Count)];
							list8.RemoveAll((int item) => item == CasinoGames.Horse3Place);
							CasinoGames.Horse4Place = list8[random3.Next(0, list8.Count)];
							list8.RemoveAll((int item) => item == CasinoGames.Horse4Place);
							CasinoGames.Horse5Place = list8[random3.Next(0, list8.Count)];
							list8.RemoveAll((int item) => item == CasinoGames.Horse5Place);
							CasinoGames.Horse6Place = list8[random3.Next(0, list8.Count)];
							CasinoGames.RaceSeed = list7[random3.Next(0, list7.Count)];
							if (CasinoGames.SelectedHorse == 1)
							{
								CasinoGames.HorseRacingPayout = CasinoGames.RacingBet * CasinoGames.Horse1Odds;
							}
							if (CasinoGames.SelectedHorse == 2)
							{
								CasinoGames.HorseRacingPayout = CasinoGames.RacingBet * CasinoGames.Horse2Odds;
							}
							if (CasinoGames.SelectedHorse == 3)
							{
								CasinoGames.HorseRacingPayout = CasinoGames.RacingBet * CasinoGames.Horse3Odds;
							}
							if (CasinoGames.SelectedHorse == 4)
							{
								CasinoGames.HorseRacingPayout = CasinoGames.RacingBet * CasinoGames.Horse4Odds;
							}
							if (CasinoGames.SelectedHorse == 5)
							{
								CasinoGames.HorseRacingPayout = CasinoGames.RacingBet * CasinoGames.Horse5Odds;
							}
							if (CasinoGames.SelectedHorse == 6)
							{
								CasinoGames.HorseRacingPayout = CasinoGames.RacingBet * CasinoGames.Horse6Odds;
							}
							CasinoGames.GUIscaleform.CallFunction("START_RACE", 10000f, CasinoGames.RaceSeed, CasinoGames.Horse1Place, CasinoGames.Horse2Place, CasinoGames.Horse3Place, CasinoGames.Horse4Place, CasinoGames.Horse5Place, CasinoGames.Horse6Place, 1, true);
							CasinoGames.Screen = 5f;
							CasinoGames.frame = 1;
							CasinoGames.HorsesAreRacing = true;
							CasinoGames.EndOfRaceDelay = (int)((float)Game.GameTime + 10000f);
							CasinoGames.RacingSoundLoop = Function.Call<int>(Hash.GET_SOUND_ID);
							Function.Call(Hash.PLAY_SOUND_FRONTEND, CasinoGames.RacingSoundLoop, "race_loop", "dlc_vw_casino_inside_track_betting_single_event_sounds", true);
						}
						if (CasinoGames.HorsesAreRacing && Game.GameTime >= CasinoGames.EndOfRaceDelay)
						{
							CasinoGames.GUIscaleform.CallFunction("START_RACE", 10000f, CasinoGames.RaceSeed, CasinoGames.Horse1Place, CasinoGames.Horse2Place, CasinoGames.Horse3Place, CasinoGames.Horse4Place, CasinoGames.Horse5Place, CasinoGames.Horse6Place, 100000, true);
							CasinoGames.Screen = 6f;
							CasinoGames.frame = 1;
							CasinoGames.HorsesAreRacing = false;
							CasinoGames.ShowingEndScrene = true;
							CasinoGames.EndScreenDelay = Game.GameTime + 5000;
						}
						if (CasinoGames.ShowingEndScrene && Game.GameTime >= CasinoGames.EndScreenDelay)
						{
							if (CasinoGames.SelectedHorse == CasinoGames.Horse1Place)
							{
								Function.Call(Hash.STOP_SOUND, CasinoGames.RacingSoundLoop);
								Function.Call(Hash.RELEASE_SOUND_ID, CasinoGames.RacingSoundLoop);
								if (CasinoGames.SelectedHorse == 1)
								{
									CasinoGames.HorseRacingPayout = CasinoGames.RacingBet * CasinoGames.Horse1Odds;
								}
								if (CasinoGames.SelectedHorse == 2)
								{
									CasinoGames.HorseRacingPayout = CasinoGames.RacingBet * CasinoGames.Horse2Odds;
								}
								if (CasinoGames.SelectedHorse == 3)
								{
									CasinoGames.HorseRacingPayout = CasinoGames.RacingBet * CasinoGames.Horse3Odds;
								}
								if (CasinoGames.SelectedHorse == 4)
								{
									CasinoGames.HorseRacingPayout = CasinoGames.RacingBet * CasinoGames.Horse4Odds;
								}
								if (CasinoGames.SelectedHorse == 5)
								{
									CasinoGames.HorseRacingPayout = CasinoGames.RacingBet * CasinoGames.Horse5Odds;
								}
								if (CasinoGames.SelectedHorse == 6)
								{
									CasinoGames.HorseRacingPayout = CasinoGames.RacingBet * CasinoGames.Horse6Odds;
								}
								CasinoGames.LastWin = "You Won {" + CasinoGames.SelectedHorse + "}";
								CasinoGames.Chips_Amount += CasinoGames.HorseRacingPayout;
								Class1.Config.SetValue("Setup", "Chips_Amount", CasinoGames.Chips_Amount);
								Class1.Config.Save();
								Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
								Notification.Show("You won " + CasinoGames.HorseRacingPayout + " Chips");
								Function.Call(Hash.SET_RANDOM_SEED, 4);
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "race_winner", "dlc_vw_casino_inside_track_betting_single_event_sounds", true);
							}
							else
							{
								Function.Call(Hash.STOP_SOUND, CasinoGames.RacingSoundLoop);
								Function.Call(Hash.RELEASE_SOUND_ID, CasinoGames.RacingSoundLoop);
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "race_finish", "dlc_vw_casino_inside_track_betting_single_event_sounds", true);
								Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
								Notification.Show("No Win, " + CasinoGames.RacingBet + " Chips Lost");
								CasinoGames.LastWin = "You Lost {" + CasinoGames.SelectedHorse + "}";
								Function.Call(Hash.SET_RANDOM_SEED, 4);
							}
							CasinoGames.Screen = 7f;
							CasinoGames.frame = 1;
							CasinoGames.HorsesAreRacing = false;
							CasinoGames.ShowingEndScrene = false;
						}
						CasinoGames.GUIscaleform.CallFunction("SET_MAIN_EVENT_IN_PROGRESS", true);
						Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
						CasinoGames.GUIscaleform.Render2D();
						Function.Call(Hash.SET_RANDOM_SEED, 4);
						Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, true);
						if (CasinoGames.Screen == 0f)
						{
							CasinoGames.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~/~INPUT_COVER~ to Quit.\nSelect \"Place Bet\" to start Betting", 1);
						}
						if (CasinoGames.Screen == 1f)
						{
							CasinoGames.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~/~INPUT_COVER~ to Quit.\nChoose a horse, a horse with lower odds has a better chance of winning, but has less payout.", 1);
						}
						if (CasinoGames.Screen == 3f)
						{
							CasinoGames.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~/~INPUT_COVER~ to Quit.\nUse ~INPUT_CELLPHONE_LEFT~/~INPUT_CELLPHONE_RIGHT~ to raise/lower the bet.\n Press ~INPUT_VEH_HANDBRAKE~ to Set Max Bet", 1);
							if (Game.IsControlJustPressed(Control.VehicleHandbrake))
							{
								CasinoGames.Bet = 10000f;
							}
						}
						if (CasinoGames.Screen == 7f)
						{
							CasinoGames.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~/~INPUT_COVER~ to Quit.\nDid u win?", 1);
						}
						Function.Call(Hash.SET_RANDOM_SEED, 7);
						CasinoGames.DisplayChips(CasinoGames.Chips_Amount);
						Function.Call(Hash.SHOW_SCRIPTED_HUD_COMPONENT_THIS_FRAME, 21);
						if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
						{
							CasinoGames.GUIscaleform.CallFunction("SET_MOUSE_INPUT", Function.Call<float>(Hash.GET_CONTROL_NORMAL, new InputArgument[2] { 0, 239 }), Function.Call<float>(Hash.GET_CONTROL_NORMAL, new InputArgument[2] { 0, 240 }));
						}
						if (Game.LastInputMethod == InputMethod.GamePad && CasinoGames.GUIMT < Game.GameTime)
						{
							if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, new InputArgument[2] { 2, 32 }) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, new InputArgument[2] { 2, 172 }))
							{
								CasinoGames.GUIMT = Game.GameTime + 2;
								Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, CasinoGames.GUIscaleform.Handle, "SET_INPUT_EVENT");
								Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 8);
								Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
								Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, CasinoGames.GUIscaleform.Handle, "SET_MOUSE_INPUT");
								Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, CasinoGames.GUIPosX);
								if (CasinoGames.GUIPosY > -10.5f)
								{
									CasinoGames.GUIPosY -= 0.01f;
								}
								Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, new InputArgument[1] { CasinoGames.GUIPosY });
								Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
							}
							if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, new InputArgument[2] { 2, 33 }) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, new InputArgument[2] { 2, 173 }))
							{
								CasinoGames.GUIMT = Game.GameTime + 2;
								Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2]
								{
									CasinoGames.GUIscaleform.Handle,
									"SET_INPUT_EVENT"
								});
								Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, new InputArgument[1] { 9 });
								Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
								Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2]
								{
									CasinoGames.GUIscaleform.Handle,
									"SET_MOUSE_INPUT"
								});
								Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, new InputArgument[1] { CasinoGames.GUIPosX });
								if (CasinoGames.GUIPosY < 10.5f)
								{
									CasinoGames.GUIPosY += 0.01f;
								}
								Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, new InputArgument[1] { CasinoGames.GUIPosY });
								Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
							}
							if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, new InputArgument[2] { 2, 34 }) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, new InputArgument[2] { 2, 174 }))
							{
								CasinoGames.GUIMT = Game.GameTime + 2;
								Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2]
								{
									CasinoGames.GUIscaleform.Handle,
									"SET_INPUT_EVENT"
								});
								Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, new InputArgument[1] { 10 });
								Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
								Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2]
								{
									CasinoGames.GUIscaleform.Handle,
									"SET_MOUSE_INPUT"
								});
								if (CasinoGames.GUIPosX > -10.5f)
								{
									CasinoGames.GUIPosX -= 0.01f;
								}
								Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, new InputArgument[1] { CasinoGames.GUIPosX });
								Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, new InputArgument[1] { CasinoGames.GUIPosY });
								Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
							}
							if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, new InputArgument[2] { 2, 35 }) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, new InputArgument[2] { 2, 175 }))
							{
								CasinoGames.GUIMT = Game.GameTime + 2;
								Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2]
								{
									CasinoGames.GUIscaleform.Handle,
									"SET_INPUT_EVENT"
								});
								Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, new InputArgument[1] { 11 });
								Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
								if (CasinoGames.GUIPosX < 10.5f)
								{
									CasinoGames.GUIPosX += 0.01f;
								}
								Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2]
								{
									CasinoGames.GUIscaleform.Handle,
									"SET_MOUSE_INPUT"
								});
								Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, new InputArgument[1] { CasinoGames.GUIPosX });
								Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, new InputArgument[1] { CasinoGames.GUIPosY });
								Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
							}
						}
						Function.Call(Hash.ENABLE_CONTROL_ACTION, new InputArgument[3] { 2, 201, 1 });
						Function.Call(Hash.ENABLE_CONTROL_ACTION, new InputArgument[3] { 2, 237, 1 });
					}
				}
			}
			if (!CasinoGames.GUIOn)
			{
				return;
			}
			Function.Call(Hash.DISPLAY_RADAR, false);
			if (CasinoGames.Screen == 5f && Function.Call<bool>(Hash.HAS_SOUND_FINISHED, CasinoGames.RacingSoundLoop))
			{
				Function.Call(Hash.PLAY_SOUND_FRONTEND, CasinoGames.RacingSoundLoop, "race_loop", "dlc_vw_casino_inside_track_betting_single_event_sounds", true);
			}
			if (Game.IsControlJustPressed(Control.FrontendCancel))
			{
				Function.Call(Hash.STOP_SOUND, CasinoGames.RacingSoundLoop);
				Function.Call(Hash.RELEASE_SOUND_ID, CasinoGames.RacingSoundLoop);
				Game.Player.CanControlCharacter = true;
				CasinoGames.Screen = 0f;
				CasinoGames.frame = 0;
				CasinoGames.GUIOn = false;
				CasinoGames.GUIscaleform.Dispose();
				CasinoGames.GUIAPP = -1;
				CasinoGames.GUIBIX = -1;
				CasinoGames.RunningInsideTrackScaleform = false;
			}
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			Convert.ToInt32(absoluteX / 2f);
			Convert.ToInt32(absoluteX / 2f);
			Convert.ToInt32(absoluteY / 2f);
			Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
			Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
			Game.Player.Character.IsPositionFrozen = true;
			if (Game.Player.Character.Weapons.Current.GetHashCode() != 2725352035u)
			{
				Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
			}
			if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, new InputArgument[2] { 2, 201 }) || Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, new InputArgument[2] { 2, 237 }))
			{
				CasinoGames.GUICheckBool = false;
				CasinoGames.GUIscaleform.CallFunction("SET_INPUT_EVENT", 201f, -1082130432, -1082130432, -1082130432, -1082130432);
				if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 188))
				{
					CasinoGames.GUIscaleform.CallFunction("SET_INPUT_EVENT", 188f, -1f, -1f, -1f, -1f);
				}
				if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 187))
				{
					CasinoGames.GUIscaleform.CallFunction("SET_INPUT_EVENT", 187f, -1f, -1f, -1f, -1f);
				}
				if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 189))
				{
					CasinoGames.GUIscaleform.CallFunction("SET_INPUT_EVENT", 189f, -1f, -1f, -1f, -1f);
				}
				if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 190))
				{
					CasinoGames.GUIscaleform.CallFunction("SET_INPUT_EVENT", 190f, -1f, -1f, -1f, -1f);
				}
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2]
				{
					CasinoGames.GUIscaleform.Handle,
					"GET_CURRENT_SELECTION"
				});
				CasinoGames.GUIAPP = Function.Call<int>(Hash.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE);
				Function.Call<int>(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, CasinoGames.GUIscaleform.Handle, "GET_SELECTED_BUSINESS_ID");
				CasinoGames.SelectedBus = Function.Call<int>(Hash.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE);
				Function.Call<int>(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, CasinoGames.GUIscaleform.Handle, "GET_CURRENT_SCREEN_ID");
				CasinoGames.SelectedScr = Function.Call<int>(Hash.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE);
				Function.Call<int>(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, CasinoGames.GUIscaleform.Handle, "GET_CURRENT_ROLLOVER");
				CasinoGames.SelectedRoll = Function.Call<int>(Hash.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE);
				CasinoGames.BEGIN_SCALEFORM_MOVIE_METHOD(CasinoGames.GUIscaleform.Handle, "GET_HORSE_POSITIONS");
				CasinoGames.HorsePosReturn = CasinoGames.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE();
				CasinoGames.BEGIN_SCALEFORM_MOVIE_METHOD(CasinoGames.GUIscaleform.Handle, "GET_RACE_IS_COMPLETE");
				CasinoGames.RaceCompletedReturn = CasinoGames.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE();
			}
			if (CasinoGames.GUICheckBool)
			{
				return;
			}
			if (Function.Call<bool>(Hash.IS_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_READY, new InputArgument[1] { CasinoGames.HorsePosReturn }))
			{
				CasinoGames.HorsePositions = Function.Call<int>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_INT, new InputArgument[1] { CasinoGames.HorsePosReturn });
			}
			if (Function.Call<bool>(Hash.IS_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_READY, new InputArgument[1] { CasinoGames.RaceCompletedReturn }))
			{
				CasinoGames.RaceComplete = Function.Call<bool>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_BOOL, new InputArgument[1] { CasinoGames.RaceCompletedReturn });
			}
			if (Function.Call<bool>(Hash.IS_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_READY, new InputArgument[1] { CasinoGames.GUIAPP }))
			{
				CasinoGames.GUIBIX = Function.Call<int>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_INT, new InputArgument[1] { CasinoGames.GUIAPP });
				if (CasinoGames.GUIBIX != -1)
				{
					CasinoGames.LastGUIButton = CasinoGames.GUIBIX;
				}
				CasinoGames.GUICheckBool = true;
			}
			if (Function.Call<bool>(Hash.IS_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_READY, new InputArgument[1] { CasinoGames.SelectedBus }))
			{
				CasinoGames.SelectedMCBusiness = Function.Call<int>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_INT, new InputArgument[1] { CasinoGames.SelectedBus });
			}
			if (Function.Call<bool>(Hash.IS_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_READY, new InputArgument[1] { CasinoGames.SelectedScr }))
			{
				CasinoGames.SelectedScreenID = Function.Call<int>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_INT, new InputArgument[1] { CasinoGames.SelectedScr });
			}
			if (Function.Call<bool>(Hash.IS_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_READY, new InputArgument[1] { CasinoGames.SelectedRoll }))
			{
				CasinoGames.SelectedRollover = Function.Call<int>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_INT, new InputArgument[1] { CasinoGames.SelectedRoll });
			}
		}
	}
}
