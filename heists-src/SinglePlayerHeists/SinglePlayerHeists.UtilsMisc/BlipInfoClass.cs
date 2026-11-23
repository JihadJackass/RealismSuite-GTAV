using System.Collections.Generic;
using GTA;
using GTA.Native;

namespace SinglePlayerHeists.UtilsMisc
{
	internal class BlipInfoClass
	{
		internal class BlipInfo : Script
		{
			public string Title { get; set; } = "";


			public bool RockstarVerified { get; set; }

			public dynamic Info { get; set; } = new { };


			public string Money { get; set; } = "";


			public string RP { get; set; } = "";


			public string Dict { get; set; } = "";


			public string Texture { get; set; } = "";

		}

		public class BlipInfoData
		{
			private static readonly Dictionary<int, BlipInfo> BlipInfoDataDictionary = new Dictionary<int, BlipInfo>();

			public static int Display = 1;

			public static int _labels = 0;

			public static int _entries = 0;

			public static Blip currentBlip;

			public static BlipInfo EnsureBlipInfo(int blip)
			{
				if (blip == 0)
				{
					blip = 0;
				}
				Function.Call(Hash.SET_BLIP_AS_MISSION_CREATOR_BLIP, blip, true);
				if (!BlipInfoDataDictionary.ContainsKey(blip))
				{
					BlipInfoDataDictionary[blip] = new BlipInfo();
				}
				return BlipInfoDataDictionary[blip];
			}

			public static void ResetBlipInfo(int blip)
			{
				EnsureBlipInfo(blip);
				BlipInfoDataDictionary.Remove(blip);
			}

			public static void SetBlipInfoTitle(int blip, string title, bool rockstarVerified)
			{
				BlipInfo blipInfo = EnsureBlipInfo(blip);
				blipInfo.Title = title ?? "";
				blipInfo.RockstarVerified = rockstarVerified;
			}

			public static void SetBlipInfoImage(int blip, string dict, string tex)
			{
				BlipInfo blipInfo = EnsureBlipInfo(blip);
				blipInfo.Dict = dict ?? "";
				blipInfo.Texture = tex ?? "";
			}

			public static void SetBlipInfoEconomy(int blip, int rp, int money)
			{
				BlipInfo blipInfo = EnsureBlipInfo(blip);
				blipInfo.Money = money.ToString() ?? "";
				blipInfo.RP = rp.ToString() ?? "";
			}

			public static void SetBlipInfo(int blip, dynamic info)
			{
				BlipInfo blipInfo = EnsureBlipInfo(blip);
				blipInfo.Info = (object)info;
			}

			public static void AddBlipInfoText(int blip, string leftText, string rightText)
			{
				BlipInfo blipInfo = EnsureBlipInfo(blip);
				if (!string.IsNullOrEmpty(rightText))
				{
					blipInfo.Info.Add(new
					{
						Type = 1,
						LeftText = (leftText ?? ""),
						RightText = (rightText ?? "")
					});
				}
				else
				{
					blipInfo.Info.Add(new
					{
						Type = 5,
						LeftText = (leftText ?? ""),
						RightText = ""
					});
				}
			}

			public static void AddBlipInfoName(int blip, string leftText, string rightText)
			{
				BlipInfo blipInfo = EnsureBlipInfo(blip);
				blipInfo.Info.Add(new
				{
					Type = 3,
					LeftText = (leftText ?? ""),
					RightText = (rightText ?? "")
				});
			}

			public static void AddBlipInfoHeader(int blip, string leftText, string rightText)
			{
				BlipInfo blipInfo = EnsureBlipInfo(blip);
				blipInfo.Info.Add(new
				{
					Type = 4,
					LeftText = (leftText ?? ""),
					RightText = (rightText ?? "")
				});
			}

			public static void AddBlipInfoIcon(int blip, string leftText, string rightText, int iconId, int iconColor, bool isChecked)
			{
				BlipInfo blipInfo = EnsureBlipInfo(blip);
				blipInfo.Info.Add(new
				{
					Type = 2,
					LeftText = (leftText ?? ""),
					RightText = (rightText ?? ""),
					IconId = iconId,
					IconColor = iconColor,
					Checked = isChecked
				});
			}

			public static void UpdateDisplay()
			{
				if (Function.Call<bool>(Hash.BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND, "DISPLAY_DATA_SLOT"))
				{
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, Display);
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
				}
			}

			public static void SetColumnState(int column, bool state)
			{
				if (Function.Call<bool>(Hash.BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND, "SHOW_COLUMN"))
				{
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, column);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, state);
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
				}
			}

			public static void ShowDisplay(bool show)
			{
				SetColumnState(Display, show);
			}

			public static void func_36(string fParam0)
			{
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, fParam0);
				Function.Call(Hash.END_TEXT_COMMAND_UNPARSED_SCALEFORM_STRING);
			}

			public static void SetIcon(int index, string title, string text, int icon, int iconColor, bool completed)
			{
				if (Function.Call<bool>(Hash.BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND, "SET_DATA_SLOT"))
				{
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, Display);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, index);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 65);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 3);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 2);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 0);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 1);
					func_36(title);
					func_36(text);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, icon);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, iconColor);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, completed);
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
				}
			}

			public static void SetText(int index, string title, string text, int textType = 0)
			{
				if (Function.Call<bool>(Hash.BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND, "SET_DATA_SLOT"))
				{
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, Display);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, index);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 65);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 3);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, textType);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 0);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 0);
					func_36(title);
					func_36(text);
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
				}
			}

			public static void ClearDisplay()
			{
				if (Function.Call<bool>(Hash.BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND, "SET_DATA_SLOT_EMPTY"))
				{
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, Display);
				}
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
				_labels = 0;
				_entries = 0;
			}

			public static string _label(string text)
			{
				string result = "LBL" + _labels;
				_labels++;
				return result;
			}

			public static void SetTitle(string title, int rockstarVerified, string rp, string money, string dict, string tex)
			{
				if (Function.Call<bool>(Hash.BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND, "SET_COLUMN_TITLE"))
				{
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, Display);
					func_36("");
					func_36(_label(title));
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, rockstarVerified);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_TEXTURE_NAME_STRING, dict);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_TEXTURE_NAME_STRING, tex);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 0);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 0);
					if (rp == "")
					{
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, 0);
					}
					else
					{
						func_36(_label(rp));
					}
					if (money == "")
					{
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, 0);
					}
					else
					{
						func_36(_label(money));
					}
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
				}
			}

			public static void AddText(string title, string desc, int style)
			{
				SetText(_entries, _label(title), _label(desc), style);
				_entries++;
			}

			public static void AddIcon(string title, string desc, int icon, int color, bool ischecked)
			{
				SetIcon(_entries, _label(title), _label(desc), icon, color, ischecked);
				_entries++;
			}

			public static void MAINFUNCTION()
			{
				if (!Function.Call<bool>(Hash.IS_FRONTEND_READY_FOR_CONTROL))
				{
					return;
				}
				Blip blip = Function.Call<Blip>(Hash.GET_NEW_SELECTED_MISSION_CREATOR_BLIP);
				if (Function.Call<bool>(Hash.IS_HOVERING_OVER_MISSION_CREATOR_BLIP))
				{
					if (!Function.Call<bool>(Hash.DOES_BLIP_EXIST, blip) || !(currentBlip != blip))
					{
						return;
					}
					currentBlip = blip;
					BlipInfo value;
					if (BlipInfoDataDictionary.TryGetValue(blip.Handle, out value))
					{
						Function.Call(Hash.TAKE_CONTROL_OF_FRONTEND);
						ClearDisplay();
						SetTitle(value.Title, value.RockstarVerified ? 1 : 0, value.RP, value.Money, value.Dict, value.Texture);
						foreach (dynamic item in value.Info)
						{
							if (item.Type == 2)
							{
								BlipInfoData.AddIcon(item.LeftText, item.RightText, item.IconId, item.IconColor, item.Checked);
							}
							else
							{
								BlipInfoData.AddText(item.LeftText, item.RightText, item.Type);
							}
						}
						ShowDisplay(true);
						UpdateDisplay();
						Function.Call(Hash.RELEASE_CONTROL_OF_FRONTEND);
					}
					else
					{
						ShowDisplay(false);
					}
				}
				else if (currentBlip != null)
				{
					currentBlip = null;
					ShowDisplay(false);
				}
			}
		}
	}
}
