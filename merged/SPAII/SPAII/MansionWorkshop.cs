using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using GTA;
using GTA.Native;
using GTA.UI;
using INMNativeUI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class MansionWorkshop
	{
		private class ModInfo
		{
			public string Tag;

			public int[] Values;

			public ModInfo(string t, int[] v)
			{
				Tag = t;
				Values = v;
			}
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<int, string> _0024I44_002D0;

			public static Func<VehicleWheelType, int> _0024I45_002D0;

			public static Func<int, int> _0024I45_002D1;

			public static Func<int, string> _0024I45_002D2;

			public static Func<int, string> _0024I45_002D3;

			public static Func<int, bool> _0024I47_002D0;

			public static Func<int, bool> _0024I47_002D1;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal string _Lambda_0024__44_002D0(int i)
			{
				return $"Livery {checked(i + 1)}";
			}

			[SpecialName]
			internal int _Lambda_0024__45_002D0(VehicleWheelType w)
			{
				return (int)w;
			}

			[SpecialName]
			internal int _Lambda_0024__45_002D1(int i)
			{
				return i;
			}

			[SpecialName]
			internal string _Lambda_0024__45_002D2(int i)
			{
				return WheelTypeName(i);
			}

			[SpecialName]
			internal string _Lambda_0024__45_002D3(int i)
			{
				return ModName(23, i);
			}

			[SpecialName]
			internal bool _Lambda_0024__47_002D0(int i)
			{
				return i >= 0;
			}

			[SpecialName]
			internal bool _Lambda_0024__47_002D1(int i)
			{
				return i >= 0;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__39_002D0
		{
			public int _0024VB_0024Local_t;

			[SpecialName]
			internal bool _Lambda_0024__0(int i)
			{
				return !IsHswPerformancePart(modVeh.Model.Hash, _0024VB_0024Local_t, i);
			}

			[SpecialName]
			internal string _Lambda_0024__1(int i)
			{
				return ModName(_0024VB_0024Local_t, i);
			}
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__41<_0024CLS0>
		{
			public static readonly _Closure_0024__41<_0024CLS0> _0024I;

			public static Func<object, int> _0024I41_002D0;

			public static Func<int, int> _0024I41_002D1;

			static _Closure_0024__41()
			{
				_0024I = new _Closure_0024__41<_0024CLS0>();
			}

			[SpecialName]
			internal int _Lambda_0024__41_002D0(object o)
			{
				return Convert.ToInt32(RuntimeHelpers.GetObjectValue(o));
			}

			[SpecialName]
			internal int _Lambda_0024__41_002D1(int i)
			{
				return i;
			}
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__42<_0024CLS0>
		{
			public static readonly _Closure_0024__42<_0024CLS0> _0024I;

			public static Func<int, string> _0024I42_002D0;

			static _Closure_0024__42()
			{
				_0024I = new _Closure_0024__42<_0024CLS0>();
			}

			[SpecialName]
			internal string _Lambda_0024__42_002D0(int i)
			{
				return Pretty(Enum.GetName(typeof(_0024CLS0), i));
			}
		}

		internal static Mansion modM;

		internal static Vehicle modVeh;

		private static readonly Dictionary<string, int> modOrig = new Dictionary<string, int>();

		private static readonly Dictionary<string, int> modCost = new Dictionary<string, int>();

		private static bool modRepair = false;

		private static string modPlateOrig = "";

		private static bool origPrimCustom = false;

		private static bool origSecCustom = false;

		private static Color origPrimRgb = Color.Black;

		private static Color origSecRgb = Color.Black;

		private static readonly int[] PerformanceTypes = new int[5] { 11, 12, 13, 15, 16 };

		private static readonly int[] BodyTypes = new int[35]
		{
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
			10, 14, 25, 26, 27, 28, 29, 30, 31, 32,
			33, 34, 35, 36, 37, 38, 39, 40, 41, 42,
			43, 44, 45, 46, 48
		};

		private static readonly string[] XenonNames = new string[14]
		{
			"Default", "White", "Blue", "Electric Blue", "Mint Green", "Lime Green", "Yellow", "Golden Shower", "Orange", "Red",
			"Pony Pink", "Hot Pink", "Purple", "Blacklight"
		};

		private static readonly string[] NeonNames = new string[13]
		{
			"White", "Blue", "Electric Blue", "Mint Green", "Lime Green", "Yellow", "Golden Shower", "Orange", "Red", "Pony Pink",
			"Hot Pink", "Purple", "Blacklight"
		};

		private static readonly Color[] NeonColors = new Color[13]
		{
			Color.FromArgb(222, 222, 255),
			Color.FromArgb(2, 21, 255),
			Color.FromArgb(3, 83, 255),
			Color.FromArgb(0, 255, 140),
			Color.FromArgb(94, 255, 1),
			Color.FromArgb(255, 255, 0),
			Color.FromArgb(255, 150, 0),
			Color.FromArgb(255, 62, 0),
			Color.FromArgb(255, 1, 1),
			Color.FromArgb(255, 50, 100),
			Color.FromArgb(255, 5, 190),
			Color.FromArgb(35, 1, 255),
			Color.FromArgb(15, 3, 255)
		};

		private static readonly string[] SmokeNames = new string[11]
		{
			"White", "Black", "Blue", "Yellow", "Orange", "Red", "Green", "Purple", "Pink", "Brown",
			"Patriot"
		};

		private static readonly Color[] SmokeColors = new Color[11]
		{
			Color.FromArgb(255, 255, 255),
			Color.FromArgb(20, 20, 20),
			Color.FromArgb(0, 174, 239),
			Color.FromArgb(252, 238, 0),
			Color.FromArgb(255, 153, 51),
			Color.FromArgb(226, 6, 6),
			Color.FromArgb(114, 204, 114),
			Color.FromArgb(132, 102, 226),
			Color.FromArgb(255, 120, 200),
			Color.FromArgb(180, 130, 97),
			Color.FromArgb(0, 0, 0)
		};

		private static string Pretty(string s)
		{
			return Regex.Replace(s, "(?<=[a-z0-9])(?=[A-Z])", " ");
		}

		private static string Localized(string label)
		{
			if (string.IsNullOrEmpty(label))
			{
				return null;
			}
			string localizedString = Game.GetLocalizedString(label);
			if (string.IsNullOrEmpty(localizedString) || Operators.CompareString(localizedString, "NULL", false) == 0)
			{
				return null;
			}
			return localizedString;
		}

		private static string SlotName(int t)
		{
			if (IsHswVehicle())
			{
				switch (t)
				{
				case 36:
					return "HSW conversion";
				case 34:
					return "HSW turbo";
				}
			}
			switch (t)
			{
			case 11:
				return "Engine";
			case 12:
				return "Brakes";
			case 13:
				return "Transmission";
			case 14:
				return "Horn";
			case 15:
				return "Suspension";
			case 16:
				return "Armour";
			case 23:
				return "Rims";
			case 48:
				return "Livery";
			default:
			{
				string text = Localized(Function.Call<string>(Hash.GET_MOD_SLOT_NAME, modVeh.Handle, t));
				if (text != null)
				{
					return text;
				}
				if (Enum.IsDefined(typeof(V2Compat.VehicleMod), t))
				{
					V2Compat.VehicleMod vehicleMod = (V2Compat.VehicleMod)t;
					return Pretty(vehicleMod.ToString());
				}
				return $"Part {t}";
			}
			}
		}

		private static string ModName(int t, int i)
		{
			if (i < 0)
			{
				return "Stock";
			}
			checked
			{
				if (IsHswVehicle())
				{
					if (t == 36 && i == 0)
					{
						return "HSW conversion";
					}
					if (t == 34)
					{
						return Localized(Function.Call<string>(Hash.GET_MOD_TEXT_LABEL, modVeh.Handle, t, i)) ?? $"HSW turbo option {i + 1}";
					}
					if (IsHswPerformancePart(modVeh.Model.Hash, t, i))
					{
						return "HSW " + SlotName(t).ToLowerInvariant();
					}
				}
				return Localized(Function.Call<string>(Hash.GET_MOD_TEXT_LABEL, modVeh.Handle, t, i)) ?? $"{SlotName(t)} {i + 1}";
			}
		}

		private static int Pick(int[] arr, int value)
		{
			if (value < 0)
			{
				return 0;
			}
			return arr[Math.Min(value, checked(arr.Length - 1))];
		}

		private static int BasePrice(string tag, int value)
		{
			checked
			{
				if (tag.StartsWith("mod:"))
				{
					switch (Conversions.ToInteger(tag.Substring(4)))
					{
					case 36:
						if (IsHswVehicle())
						{
							return (value >= 0) ? 500000 : 0;
						}
						return (value < 0) ? 250 : Math.Min(6000, 750 + 250 * value);
					case 34:
						if (IsHswVehicle())
						{
							return (value >= 0) ? (25000 * (value + 1)) : 0;
						}
						return (value < 0) ? 250 : Math.Min(6000, 750 + 250 * value);
					case 11:
						return Pick(new int[4] { 9000, 12500, 18000, 33500 }, value);
					case 12:
						return Pick(new int[4] { 20000, 26500, 35000, 40000 }, value);
					case 13:
						return Pick(new int[4] { 29500, 32500, 40000, 45000 }, value);
					case 15:
						return Pick(new int[5] { 1000, 2000, 3400, 4400, 5400 }, value);
					case 16:
						return Pick(new int[5] { 2500, 5000, 7500, 10000, 12500 }, value);
					case 14:
						return (value >= 0) ? 1500 : 0;
					default:
						return (value < 0) ? 250 : Math.Min(6000, 750 + 250 * value);
					}
				}
				switch (tag)
				{
				case "turbo":
					return (value == 1) ? 50000 : 0;
				case "wtype":
					return 0;
				case "rims":
					return (value < 0) ? 250 : Math.Min(8000, 2500 + 100 * value);
				case "ctires":
					return 1000;
				case "bptires":
					return 2500;
				case "smoke":
					return 1500;
				case "smokecol":
				case "xcol":
					return 500;
				case "prim":
				case "sec":
					return 1500;
				case "pearl":
					return 1000;
				case "rimcol":
				case "neoncol":
					return 750;
				case "tint":
				case "xenon":
					return 1500;
				case "plate":
					return 200;
				case "platetext":
					return 300;
				case "neonF":
				case "neonB":
				case "neonL":
				case "neonR":
					return 1000;
				case "livery":
					return 2000;
				default:
					return 1000;
				}
			}
		}

		private static int Price(string tag, int value)
		{
			checked
			{
				return (int)unchecked(checked(unchecked((long)BasePrice(tag, value)) * unchecked((long)Math.Max(0, MansionFeatures.WorkshopPricePercent))) / 100);
			}
		}

		private static int TotalCost()
		{
			int num = modCost.Values.Sum();
			checked
			{
				if (modRepair)
				{
					num += (int)unchecked(checked(500L * unchecked((long)Math.Max(0, MansionFeatures.WorkshopPricePercent))) / 100);
				}
				return num;
			}
		}

		private static void UpdateSaveLabel()
		{
			if (MansionFeatures.miSave != null)
			{
				MansionFeatures.miSave.SetRightLabel($"${TotalCost():N0}");
			}
		}

		private static int ColorIndex(Color[] arr, Color c)
		{
			checked
			{
				int num = arr.Length - 1;
				for (int i = 0; i <= num; i++)
				{
					if (arr[i].R == c.R && arr[i].G == c.G && arr[i].B == c.B)
					{
						return i;
					}
				}
				return -1;
			}
		}

		private static bool IsBike()
		{
			return modVeh.Model.IsBike;
		}

		private static bool IsHswVehicle()
		{
			if ((object)modVeh != null && modVeh.Exists())
			{
				return MansionVehicleShop.IsHswModel(modVeh.Model.Hash);
			}
			return false;
		}

		private static bool HswConverted()
		{
			if (IsHswVehicle())
			{
				return Function.Call<int>(Hash.GET_VEHICLE_MOD, modVeh.Handle, 36) >= 0;
			}
			return false;
		}

		public static bool IsHswPerformancePart(int modelHash, int modType, int modIndex)
		{
			if (modIndex < 0 || !MansionVehicleShop.IsHswModel(modelHash))
			{
				return false;
			}
			switch (modType)
			{
			case 11:
				return modIndex == 4;
			case 12:
			case 13:
				return modIndex == 3;
			case 15:
				return modIndex == MansionVehicleShop.HswSuspensionIndex(modelHash);
			case 34:
				return modIndex >= 0;
			default:
				return false;
			}
		}

		private static bool HasHswPartsInstalled()
		{
			if (!IsHswVehicle())
			{
				return false;
			}
			int[] array = new int[5] { 11, 12, 13, 15, 34 };
			foreach (int num in array)
			{
				if (IsHswPerformancePart(modVeh.Model.Hash, num, Function.Call<int>(Hash.GET_VEHICLE_MOD, modVeh.Handle, num)))
				{
					return true;
				}
			}
			return false;
		}

		private static int GetVal(string tag)
		{
			Vehicle vehicle = modVeh;
			if (tag.StartsWith("mod:"))
			{
				return Function.Call<int>(Hash.GET_VEHICLE_MOD, vehicle.Handle, Conversions.ToInteger(tag.Substring(4)));
			}
			switch (tag)
			{
			case "turbo":
				return V2Compat.IsToggleModOn(vehicle, V2Compat.VehicleToggleMod.Turbo) ? 1 : 0;
			case "smoke":
				return V2Compat.IsToggleModOn(vehicle, V2Compat.VehicleToggleMod.TireSmoke) ? 1 : 0;
			case "xenon":
				return V2Compat.IsToggleModOn(vehicle, V2Compat.VehicleToggleMod.XenonHeadlights) ? 1 : 0;
			case "wtype":
				return (int)vehicle.Mods.WheelType;
			case "rims":
				return Function.Call<int>(Hash.GET_VEHICLE_MOD, vehicle.Handle, 23);
			case "ctires":
				return Function.Call<bool>(Hash.GET_VEHICLE_MOD_VARIATION, vehicle.Handle, 23) ? 1 : 0;
			case "bptires":
				return (!vehicle.CanTiresBurst) ? 1 : 0;
			case "smokecol":
				return ColorIndex(SmokeColors, vehicle.Mods.TireSmokeColor);
			case "prim":
				return (int)vehicle.Mods.PrimaryColor;
			case "sec":
				return (int)vehicle.Mods.SecondaryColor;
			case "pearl":
				return (int)vehicle.Mods.PearlescentColor;
			case "rimcol":
				return (int)vehicle.Mods.RimColor;
			case "tint":
			{
				int windowTint = (int)vehicle.Mods.WindowTint;
				return (windowTint >= 0) ? windowTint : 0;
			}
			case "plate":
				return (int)vehicle.Mods.LicensePlateStyle;
			case "xcol":
			{
				int num = Helper.XenonHeadlightsColor(vehicle);
				return (num < 0 || num > 12) ? (-1) : num;
			}
			case "neonF":
				return vehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Front) ? 1 : 0;
			case "neonB":
				return vehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Back) ? 1 : 0;
			case "neonL":
				return vehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Left) ? 1 : 0;
			case "neonR":
				return vehicle.Mods.IsNeonLightsOn(VehicleNeonLight.Right) ? 1 : 0;
			case "neoncol":
				return ColorIndex(NeonColors, vehicle.Mods.NeonLightsColor);
			case "livery":
				return vehicle.Mods.Livery;
			default:
				return 0;
			}
		}

		private static void SetVal(string tag, int value)
		{
			Vehicle vehicle = modVeh;
			if (tag.StartsWith("mod:"))
			{
				Function.Call(Hash.SET_VEHICLE_MOD, vehicle.Handle, Conversions.ToInteger(tag.Substring(4)), value, false);
				return;
			}
			switch (tag)
			{
			case "turbo":
				V2Compat.ToggleMod(vehicle, V2Compat.VehicleToggleMod.Turbo, value == 1);
				break;
			case "smoke":
				V2Compat.ToggleMod(vehicle, V2Compat.VehicleToggleMod.TireSmoke, value == 1);
				break;
			case "xenon":
				V2Compat.ToggleMod(vehicle, V2Compat.VehicleToggleMod.XenonHeadlights, value == 1);
				break;
			case "wtype":
				vehicle.Mods.WheelType = (VehicleWheelType)value;
				Function.Call(Hash.SET_VEHICLE_MOD, vehicle.Handle, 23, -1, false);
				if (IsBike())
				{
					Function.Call(Hash.SET_VEHICLE_MOD, vehicle.Handle, 24, -1, false);
				}
				break;
			case "rims":
			case "ctires":
			{
				int num = ((Operators.CompareString(tag, "rims", false) == 0) ? value : GetVal("rims"));
				bool flag = ((Operators.CompareString(tag, "ctires", false) == 0) ? (value == 1) : (GetVal("ctires") == 1));
				Function.Call(Hash.SET_VEHICLE_MOD, vehicle.Handle, 23, num, flag);
				if (IsBike())
				{
					Function.Call(Hash.SET_VEHICLE_MOD, vehicle.Handle, 24, num, flag);
				}
				break;
			}
			case "bptires":
				vehicle.CanTiresBurst = value == 0;
				break;
			case "smokecol":
				if (value >= 0)
				{
					vehicle.Mods.TireSmokeColor = SmokeColors[value];
				}
				break;
			case "prim":
				Function.Call(Hash.CLEAR_VEHICLE_CUSTOM_PRIMARY_COLOUR, vehicle.Handle);
				vehicle.Mods.PrimaryColor = (VehicleColor)value;
				if (origPrimCustom && modOrig.ContainsKey("prim") && value == modOrig["prim"])
				{
					Function.Call(Hash.SET_VEHICLE_CUSTOM_PRIMARY_COLOUR, vehicle.Handle, (int)origPrimRgb.R, (int)origPrimRgb.G, (int)origPrimRgb.B);
				}
				break;
			case "sec":
				Function.Call(Hash.CLEAR_VEHICLE_CUSTOM_SECONDARY_COLOUR, vehicle.Handle);
				vehicle.Mods.SecondaryColor = (VehicleColor)value;
				if (origSecCustom && modOrig.ContainsKey("sec") && value == modOrig["sec"])
				{
					Function.Call(Hash.SET_VEHICLE_CUSTOM_SECONDARY_COLOUR, vehicle.Handle, (int)origSecRgb.R, (int)origSecRgb.G, (int)origSecRgb.B);
				}
				break;
			case "pearl":
				vehicle.Mods.PearlescentColor = (VehicleColor)value;
				break;
			case "rimcol":
				vehicle.Mods.RimColor = (VehicleColor)value;
				break;
			case "tint":
				vehicle.Mods.WindowTint = (VehicleWindowTint)value;
				break;
			case "plate":
				vehicle.Mods.LicensePlateStyle = (LicensePlateStyle)value;
				break;
			case "xcol":
				Helper.XenonHeadlightsColor(vehicle, (value < 0) ? 255 : value);
				break;
			case "neonF":
				vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Front, value == 1);
				break;
			case "neonB":
				vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Back, value == 1);
				break;
			case "neonL":
				vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Left, value == 1);
				break;
			case "neonR":
				vehicle.Mods.SetNeonLightsOn(VehicleNeonLight.Right, value == 1);
				break;
			case "neoncol":
				if (value >= 0)
				{
					vehicle.Mods.NeonLightsColor = NeonColors[value];
				}
				break;
			case "livery":
				vehicle.Mods.Livery = value;
				break;
			}
		}

		private static void Remember(string tag)
		{
			if (!modOrig.ContainsKey(tag))
			{
				modOrig[tag] = GetVal(tag);
			}
		}

		private static bool ApplyChange(string tag, int value)
		{
			Remember(tag);
			int val = GetVal(tag);
			SetVal(tag, value);
			if (IsHswVehicle() && tag.StartsWith("mod:") && GetVal(tag) != value)
			{
				int val2 = GetVal(tag);
				SetVal(tag, val);
				int val3 = GetVal(tag);
				Logger.Log($"Workshop HSW mod rejected: {modVeh.Model.Hash}/{tag}, requested={value}, actual={val2}, restored={val3}");
				Notification.Show((val3 == val) ? "The installed game rejected that HSW part. It has not been added to your bill; the previous part has been restored." : "The installed game rejected that HSW part. No new charge was added. Use Discard changes to reload the saved vehicle.");
				return false;
			}
			if (value == modOrig[tag])
			{
				modCost.Remove(tag);
			}
			else
			{
				modCost[tag] = Price(tag, value);
			}
			UpdateSaveLabel();
			return true;
		}

		private static void AddList(UIMenu menu, string text, string tag, int[] values, IEnumerable<string> names, string desc = null)
		{
			if (values.Length != 0)
			{
				Remember(tag);
				int val = GetVal(tag);
				int num = Array.IndexOf(values, val);
				if (num < 0)
				{
					num = 0;
				}
				UIMenuListItem item = new UIMenuListItem(text, new List<object>(names.Cast<object>()), num, desc ?? "Left / right to try it on. Pay for everything with \"Pay & save\".")
				{
					Tag = new ModInfo(tag, values)
				};
				menu.AddItem(item);
			}
		}

		private static void AddModList(UIMenu menu, int t)
		{
			_Closure_0024__39_002D0 closure_0024__39_002D = new _Closure_0024__39_002D0();
			closure_0024__39_002D._0024VB_0024Local_t = t;
			int num = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, modVeh.Handle, closure_0024__39_002D._0024VB_0024Local_t);
			if (num > 0)
			{
				int[] array = Enumerable.Range(-1, checked(num + 1)).ToArray();
				if (IsHswVehicle() && !HswConverted())
				{
					array = array.Where(closure_0024__39_002D._Lambda_0024__0).ToArray();
				}
				List<string> names = array.Select(closure_0024__39_002D._Lambda_0024__1).ToList();
				AddList(menu, SlotName(closure_0024__39_002D._0024VB_0024Local_t), "mod:" + Conversions.ToString(closure_0024__39_002D._0024VB_0024Local_t), array, names);
			}
		}

		private static void AddToggle(UIMenu menu, string text, string tag, string desc = "")
		{
			Remember(tag);
			UIMenuCheckboxItem item = new UIMenuCheckboxItem(text, GetVal(tag) == 1, desc)
			{
				Tag = new ModInfo(tag, null)
			};
			menu.AddItem(item);
		}

		private static int[] EnumList<T>()
		{
			return Enum.GetValues(typeof(T)).Cast<object>().Select((_Closure_0024__41<T>._0024I41_002D0 != null) ? _Closure_0024__41<T>._0024I41_002D0 : (_Closure_0024__41<T>._0024I41_002D0 = _Closure_0024__41<T>._0024I._Lambda_0024__41_002D0))
				.Distinct()
				.OrderBy((_Closure_0024__41<T>._0024I41_002D1 != null) ? _Closure_0024__41<T>._0024I41_002D1 : (_Closure_0024__41<T>._0024I41_002D1 = _Closure_0024__41<T>._0024I._Lambda_0024__41_002D1))
				.ToArray();
		}

		private static List<string> EnumNames<T>(int[] values)
		{
			return values.Select((_Closure_0024__42<T>._0024I42_002D0 != null) ? _Closure_0024__42<T>._0024I42_002D0 : (_Closure_0024__42<T>._0024I42_002D0 = _Closure_0024__42<T>._0024I._Lambda_0024__42_002D0)).ToList();
		}

		private static void BuildPerfMenu()
		{
			MansionFeatures.ModPerfMenu.Clear();
			if (IsHswVehicle())
			{
				if (Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, modVeh.Handle, 36) > 0)
				{
					AddList(MansionFeatures.ModPerfMenu, "HSW conversion", "mod:36", new int[2] { -1, 0 }, new string[2] { "Stock", "HSW conversion" }, "SPA II conversion fee: $500,000 before the workshop price multiplier. Unlocks the model's original HSW parts. Remove HSW parts before returning the conversion to Stock.");
				}
				else
				{
					MansionFeatures.ModPerfMenu.AddItem(new UIMenuItem("HSW conversion unavailable", "This installed model does not expose the original HSW conversion mod.")
					{
						Enabled = false
					});
				}
			}
			int[] performanceTypes = PerformanceTypes;
			foreach (int t in performanceTypes)
			{
				AddModList(MansionFeatures.ModPerfMenu, t);
			}
			if (HswConverted())
			{
				AddModList(MansionFeatures.ModPerfMenu, 34);
			}
			AddToggle(MansionFeatures.ModPerfMenu, "Turbo", "turbo", "Turbo tuning.");
			if (MansionFeatures.ModPerfMenu.MenuItems.Count == 0)
			{
				MansionFeatures.ModPerfMenu.AddItem(new UIMenuItem("(no performance parts for this vehicle)")
				{
					Enabled = false
				});
			}
			MansionFeatures.ModPerfMenu.RefreshIndex();
		}

		private static void BuildBodyMenu()
		{
			MansionFeatures.ModBodyMenu.Clear();
			int[] bodyTypes = BodyTypes;
			foreach (int num in bodyTypes)
			{
				if (!IsHswVehicle() || (num != 34 && num != 36))
				{
					AddModList(MansionFeatures.ModBodyMenu, num);
				}
			}
			int num2 = ((Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, modVeh.Handle, 48) <= 0) ? modVeh.Mods.LiveryCount : 0);
			if (num2 > 0)
			{
				int[] array = Enumerable.Range(0, num2).ToArray();
				AddList(MansionFeatures.ModBodyMenu, "Livery (paint job)", "livery", array, array.Select((_Closure_0024__._0024I44_002D0 != null) ? _Closure_0024__._0024I44_002D0 : (_Closure_0024__._0024I44_002D0 = _Closure_0024__._0024I._Lambda_0024__44_002D0)));
			}
			if (MansionFeatures.ModBodyMenu.MenuItems.Count == 0)
			{
				MansionFeatures.ModBodyMenu.AddItem(new UIMenuItem("(no body parts for this vehicle)")
				{
					Enabled = false
				});
			}
			MansionFeatures.ModBodyMenu.RefreshIndex();
		}

		private static void BuildWheelMenu(int keepSelection = -1)
		{
			MansionFeatures.ModWheelMenu.Clear();
			int[] array = null;
			try
			{
				array = modVeh.Mods.AllowedWheelTypes.Select((_Closure_0024__._0024I45_002D0 != null) ? _Closure_0024__._0024I45_002D0 : (_Closure_0024__._0024I45_002D0 = _Closure_0024__._0024I._Lambda_0024__45_002D0)).ToArray();
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
			}
			if (array == null)
			{
				array = new int[0];
			}
			int wheelType = (int)modVeh.Mods.WheelType;
			if (!array.Contains(wheelType))
			{
				array = array.Concat(new int[1] { wheelType }).OrderBy((_Closure_0024__._0024I45_002D1 != null) ? _Closure_0024__._0024I45_002D1 : (_Closure_0024__._0024I45_002D1 = _Closure_0024__._0024I._Lambda_0024__45_002D1)).ToArray();
			}
			AddList(MansionFeatures.ModWheelMenu, "Wheel type", "wtype", array, array.Select((_Closure_0024__._0024I45_002D2 != null) ? _Closure_0024__._0024I45_002D2 : (_Closure_0024__._0024I45_002D2 = _Closure_0024__._0024I._Lambda_0024__45_002D2)), "Changing the type resets the rims to stock.");
			int num = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, modVeh.Handle, 23);
			if (num > 0)
			{
				int[] array2 = Enumerable.Range(-1, checked(num + 1)).ToArray();
				AddList(MansionFeatures.ModWheelMenu, "Rims", "rims", array2, array2.Select((_Closure_0024__._0024I45_002D3 != null) ? _Closure_0024__._0024I45_002D3 : (_Closure_0024__._0024I45_002D3 = _Closure_0024__._0024I._Lambda_0024__45_002D3)));
			}
			AddToggle(MansionFeatures.ModWheelMenu, "Custom tyres", "ctires");
			AddToggle(MansionFeatures.ModWheelMenu, "Bulletproof tyres", "bptires");
			AddToggle(MansionFeatures.ModWheelMenu, "Tyre smoke", "smoke");
			AddList(MansionFeatures.ModWheelMenu, "Tyre smoke colour", "smokecol", Enumerable.Range(0, SmokeColors.Length).ToArray(), SmokeNames);
			MansionFeatures.ModWheelMenu.RefreshIndex();
			if (keepSelection >= 0 && keepSelection < MansionFeatures.ModWheelMenu.MenuItems.Count)
			{
				MansionFeatures.ModWheelMenu.CurrentSelection = keepSelection;
			}
		}

		private static string WheelTypeName(int i)
		{
			string name = Enum.GetName(typeof(VehicleWheelType), i);
			if (name != null)
			{
				return Pretty(name);
			}
			return $"Type {i}";
		}

		private static void BuildPaintMenu()
		{
			MansionFeatures.ModPaintMenu.Clear();
			int[] values = EnumList<VehicleColor>();
			List<string> names = EnumNames<VehicleColor>(values);
			AddList(MansionFeatures.ModPaintMenu, "Primary colour", "prim", values, names);
			AddList(MansionFeatures.ModPaintMenu, "Secondary colour", "sec", values, names);
			AddList(MansionFeatures.ModPaintMenu, "Pearlescent", "pearl", values, names);
			AddList(MansionFeatures.ModPaintMenu, "Wheel colour", "rimcol", values, names);
			int[] values2 = EnumList<VehicleWindowTint>().Where((_Closure_0024__._0024I47_002D0 != null) ? _Closure_0024__._0024I47_002D0 : (_Closure_0024__._0024I47_002D0 = _Closure_0024__._0024I._Lambda_0024__47_002D0)).ToArray();
			AddList(MansionFeatures.ModPaintMenu, "Window tint", "tint", values2, EnumNames<VehicleWindowTint>(values2));
			int[] values3 = EnumList<LicensePlateStyle>().Where((_Closure_0024__._0024I47_002D1 != null) ? _Closure_0024__._0024I47_002D1 : (_Closure_0024__._0024I47_002D1 = _Closure_0024__._0024I._Lambda_0024__47_002D1)).ToArray();
			AddList(MansionFeatures.ModPaintMenu, "Plate style", "plate", values3, EnumNames<LicensePlateStyle>(values3));
			MansionFeatures.ModPaintMenu.RefreshIndex();
		}

		private static void BuildLightMenu()
		{
			MansionFeatures.ModLightMenu.Clear();
			AddToggle(MansionFeatures.ModLightMenu, "Xenon headlights", "xenon");
			AddList(MansionFeatures.ModLightMenu, "Xenon colour", "xcol", Enumerable.Range(-1, XenonNames.Length).ToArray(), XenonNames);
			bool flag = true;
			try
			{
				flag = modVeh.Mods.HasNeonLights;
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
			}
			if (flag)
			{
				AddToggle(MansionFeatures.ModLightMenu, "Neon front", "neonF");
				AddToggle(MansionFeatures.ModLightMenu, "Neon back", "neonB");
				AddToggle(MansionFeatures.ModLightMenu, "Neon left", "neonL");
				AddToggle(MansionFeatures.ModLightMenu, "Neon right", "neonR");
				AddList(MansionFeatures.ModLightMenu, "Neon colour", "neoncol", Enumerable.Range(0, NeonColors.Length).ToArray(), NeonNames);
			}
			MansionFeatures.ModLightMenu.RefreshIndex();
		}

		private static void InitSession(Mansion m)
		{
			modM = m;
			modVeh = m.WorkshopVeh;
			modOrig.Clear();
			modCost.Clear();
			modRepair = false;
			modPlateOrig = modVeh.Mods.LicensePlate;
			ReadCustomColour(true, ref origPrimCustom, ref origPrimRgb);
			ReadCustomColour(false, ref origSecCustom, ref origSecRgb);
			UpdateSaveLabel();
		}

		private static void ReadCustomColour(bool primary, ref bool isCustom, ref Color rgb)
		{
			isCustom = false;
			rgb = Color.Black;
			try
			{
				int handle = modVeh.Handle;
				isCustom = Function.Call<bool>(primary ? Hash.GET_IS_VEHICLE_PRIMARY_COLOUR_CUSTOM : Hash.GET_IS_VEHICLE_SECONDARY_COLOUR_CUSTOM, handle);
				if (isCustom)
				{
					OutputArgument outputArgument = new OutputArgument();
					OutputArgument outputArgument2 = new OutputArgument();
					OutputArgument outputArgument3 = new OutputArgument();
					Function.Call(primary ? Hash.GET_VEHICLE_CUSTOM_PRIMARY_COLOUR : Hash.GET_VEHICLE_CUSTOM_SECONDARY_COLOUR, handle, outputArgument, outputArgument2, outputArgument3);
					rgb = Color.FromArgb(outputArgument.GetResult<int>(), outputArgument2.GetResult<int>(), outputArgument3.GetResult<int>());
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Workshop custom colour: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
		}

		public static void OpenModMenu(Mansion m)
		{
			try
			{
				if ((object)m.WorkshopVeh != null && m.WorkshopVeh.Exists())
				{
					if (modM != m || (object)modVeh == null || modVeh.Handle != m.WorkshopVeh.Handle)
					{
						InitSession(m);
					}
					modVeh.Mods.InstallModKit();
					MansionFeatures.ModMenu.Subtitle.Caption = $"WORKSHOP - {m.WorkshopVc.Make} {m.WorkshopVc.Name}".ToUpper();
					BuildPerfMenu();
					BuildBodyMenu();
					BuildWheelMenu();
					BuildPaintMenu();
					BuildLightMenu();
					UpdateSaveLabel();
					MansionFeatures.ModMenu.ParentMenu = null;
					MansionFeatures.ModMenu.RefreshIndex();
					MansionFeatures.ModMenu.Visible = true;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Workshop menu: {ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		internal static void ModList_Changed(UIMenu sender, UIMenuListItem listItem, int newIndex)
		{
			try
			{
				ModInfo modInfo = listItem.Tag as ModInfo;
				if (modInfo == null || (object)modVeh == null || !modVeh.Exists() || newIndex < 0 || newIndex >= modInfo.Values.Length)
				{
					return;
				}
				int num = modInfo.Values[newIndex];
				if (IsHswVehicle() && Operators.CompareString(modInfo.Tag, "mod:36", false) == 0 && num < 0 && HasHswPartsInstalled())
				{
					listItem.Index = Math.Max(0, Array.IndexOf(modInfo.Values, GetVal(modInfo.Tag)));
					Notification.Show("Select Stock for the HSW engine, brakes, transmission, suspension and HSW turbo before removing the HSW conversion.");
					return;
				}
				if (IsHswVehicle() && modInfo.Tag.StartsWith("mod:") && Operators.CompareString(modInfo.Tag, "mod:36", false) != 0 && IsHswPerformancePart(modVeh.Model.Hash, Conversions.ToInteger(modInfo.Tag.Substring(4)), num) && !HswConverted())
				{
					listItem.Index = Math.Max(0, Array.IndexOf(modInfo.Values, GetVal(modInfo.Tag)));
					Notification.Show("Install the HSW conversion first.");
					return;
				}
				if (!ApplyChange(modInfo.Tag, num))
				{
					listItem.Index = Math.Max(0, Array.IndexOf(modInfo.Values, GetVal(modInfo.Tag)));
					return;
				}
				if (Operators.CompareString(modInfo.Tag, "mod:36", false) == 0 && IsHswVehicle())
				{
					int currentSelection = sender.CurrentSelection;
					BuildPerfMenu();
					sender.CurrentSelection = Math.Min(currentSelection, Math.Max(0, checked(sender.MenuItems.Count - 1)));
				}
				if (Operators.CompareString(modInfo.Tag, "wtype", false) == 0)
				{
					modCost.Remove("rims");
					modOrig.Remove("rims");
					BuildWheelMenu(sender.CurrentSelection);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Workshop list: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
		}

		internal static void ModCheck_Changed(UIMenu sender, UIMenuCheckboxItem checkboxItem, bool @checked)
		{
			try
			{
				ModInfo modInfo = checkboxItem.Tag as ModInfo;
				if (modInfo != null && (object)modVeh != null && modVeh.Exists())
				{
					ApplyChange(modInfo.Tag, @checked ? 1 : 0);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Workshop toggle: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
		}

		internal static void ModMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			Mansion mansion = modM;
			if (mansion == null || (object)modVeh == null || !modVeh.Exists())
			{
				return;
			}
			try
			{
				switch (selectedItem.Tag as string)
				{
				case "PlateText":
				{
					string userInput = Game.GetUserInput(WindowTitle.EnterMessage20, modVeh.Mods.LicensePlate, 8);
					if (!string.IsNullOrWhiteSpace(userInput))
					{
						modVeh.Mods.LicensePlate = userInput.ToUpper().Trim();
						if (Operators.CompareString(modVeh.Mods.LicensePlate.Trim(), modPlateOrig.Trim(), false) == 0)
						{
							modCost.Remove("platetext");
						}
						else
						{
							modCost["platetext"] = Price("platetext", 1);
						}
						UpdateSaveLabel();
					}
					break;
				}
				case "Repair":
					modVeh.Repair();
					modVeh.DirtLevel = 0f;
					modRepair = true;
					UpdateSaveLabel();
					break;
				case "Save":
				{
					int num2 = TotalCost();
					if (num2 == 0 && modCost.Count == 0 && !modRepair)
					{
						Notification.Show("Nothing to save.");
						break;
					}
					eOwner player = Helper.GetPlayer();
					int balance = default(int);
					if (!mansion.IsOwnedByPlayer() || !Helper.TryGetOwnerMoney(player, ref balance) || balance < num2)
					{
						Notification.Show("The workshop bill cannot be paid by this character. Your saved vehicle has been kept.");
						break;
					}
					VehicleClass workshopVc2 = mansion.WorkshopVc;
					if (workshopVc2 == null)
					{
						break;
					}
					VehicleClass vehicleClass = new VehicleClass(modVeh, player, mansion.GarageId, workshopVc2.UniqueID, workshopVc2.Index);
					using (AtomicFileTransaction atomicFileTransaction = new VehicleData($"{Helper.grgXmlPath}{mansion.GarageFolder()}\\{workshopVc2.UniqueID}.xml", vehicleClass).BeginSaveTransaction())
					{
						if (!Helper.TryChangeOwnerMoney(player, checked(-num2)))
						{
							Notification.Show("The workshop payment failed. Your saved vehicle and unpaid tuning have been kept.");
							break;
						}
						atomicFileTransaction.Complete();
					}
					mansion.WorkshopVc = vehicleClass;
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "PROPERTY_PURCHASE", "HUD_AWARDS", false);
					Notification.Show($"~g~Workshop~w~: changes saved ({workshopVc2.Make} {workshopVc2.Name}), paid ${num2:N0}.");
					Logger.Log($"> Workshop {mansion.Name}: saved {workshopVc2.Make} {workshopVc2.Name} for {num2}");
					InitSession(mansion);
					break;
				}
				case "Discard":
				{
					VehicleClass workshopVc = mansion.WorkshopVc;
					MansionFeatures.ModMenu.Visible = false;
					Helper.FadeScreen(1, 300);
					MansionFeatures.SpawnWorkshopVehicle(mansion, workshopVc);
					Helper.FadeScreen(0, 300);
					Notification.Show("Workshop: changes discarded.");
					break;
				}
				case "Return":
				{
					bool num = modCost.Count > 0 || modRepair;
					MansionFeatures.ModMenu.Visible = false;
					MansionFeatures.RemoveWorkshopVehicle(mansion);
					Notification.Show(num ? "Workshop: vehicle back in the garage, unsaved changes discarded." : "Workshop: vehicle back in the garage.");
					break;
				}
				default:
					if (selectedItem == MansionFeatures.miPerf)
					{
						BuildPerfMenu();
					}
					if (selectedItem == MansionFeatures.miBody)
					{
						BuildBodyMenu();
					}
					if (selectedItem == MansionFeatures.miWheels)
					{
						BuildWheelMenu();
					}
					if (selectedItem == MansionFeatures.miPaint)
					{
						BuildPaintMenu();
					}
					if (selectedItem == MansionFeatures.miLights)
					{
						BuildLightMenu();
					}
					break;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Workshop select: {ex2.Message} {ex2.StackTrace}");
				Notification.Show("The workshop action could not finish. A failed save keeps the previous garage file; unpaid tuning remains available.");
				ProjectData.ClearProjectError();
			}
		}
	}
}
