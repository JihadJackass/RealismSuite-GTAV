using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using INMNativeUI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class Mansions
	{
		internal class SettingsExpectation
		{
			public string Section;

			public string Key;

			public string Value;
		}

		internal class OwnedSaleSign
		{
			public int Handle;

			public int ModelHash;

			public IntPtr Address;
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<Mansion, string> _0024I80_002D0;

			public static Predicate<Mansion> _0024I81_002D0;

			public static Func<GateDef, int> _0024I123_002D0;

			public static Func<Prop, bool> _0024I129_002D1;

			public static Func<VehicleClass, int> _0024I142_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal string _Lambda_0024__80_002D0(Mansion m)
			{
				return m.Key;
			}

			[SpecialName]
			internal bool _Lambda_0024__81_002D0(Mansion x)
			{
				return x.Enabled;
			}

			[SpecialName]
			internal int _Lambda_0024__123_002D0(GateDef x)
			{
				return x.OpenSign;
			}

			[SpecialName]
			internal bool _Lambda_0024__129_002D1(Prop p)
			{
				return IsGateLike(p);
			}

			[SpecialName]
			internal int _Lambda_0024__142_002D0(VehicleClass x)
			{
				return x.Index;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__101_002D0
		{
			public Mansion _0024VB_0024Local_m;

			public _Closure_0024__101_002D0(_Closure_0024__101_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_m = arg0._0024VB_0024Local_m;
				}
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__101_002D1
		{
			public string _0024VB_0024Local_ipl;

			public _Closure_0024__101_002D0 _0024VB_0024NonLocal__0024VB_0024Closure_2;

			public _Closure_0024__101_002D1(_Closure_0024__101_002D1 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_ipl = arg0._0024VB_0024Local_ipl;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(Mansion other)
			{
				if (!object.ReferenceEquals(other, _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_m) && other.Enabled && (other.IplsLoaded || other.IplRequestClock != null) && other.Ipls != null)
				{
					return other.Ipls.Contains(_0024VB_0024Local_ipl);
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__115_002D0
		{
			internal OwnedSaleSign _0024VB_0024Local_receipt;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				return (object)ResolveOwnedSaleSign(_0024VB_0024Local_receipt) != null;
			}

			[SpecialName]
			internal void _Lambda_0024__1()
			{
				ResolveOwnedSaleSign(_0024VB_0024Local_receipt)?.Delete();
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__129_002D0
		{
			public int _0024VB_0024Local_signHandle;

			public Vector3 _0024VB_0024Local_ppos;

			public Prop _0024VB_0024Local_first;

			public _Closure_0024__129_002D0(_Closure_0024__129_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_signHandle = arg0._0024VB_0024Local_signHandle;
					_0024VB_0024Local_ppos = arg0._0024VB_0024Local_ppos;
					_0024VB_0024Local_first = arg0._0024VB_0024Local_first;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(Prop p)
			{
				if ((object)p != null && p.Exists() && p.Model.Hash != 0)
				{
					return p.Handle != _0024VB_0024Local_signHandle;
				}
				return false;
			}

			[SpecialName]
			internal float _Lambda_0024__2(Prop p)
			{
				return p.Position.DistanceTo(_0024VB_0024Local_ppos);
			}

			[SpecialName]
			internal bool _Lambda_0024__3(Prop p)
			{
				return p.Position.DistanceTo(_0024VB_0024Local_ppos) <= 6f;
			}

			[SpecialName]
			internal float _Lambda_0024__4(Prop p)
			{
				return p.Position.DistanceTo(_0024VB_0024Local_ppos);
			}

			[SpecialName]
			internal bool _Lambda_0024__5(Prop p)
			{
				return p.Position.DistanceTo(_0024VB_0024Local_first.Position) < 9f;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__143_002D0
		{
			public int _0024VB_0024Local_id;

			[SpecialName]
			internal bool _Lambda_0024__0(Mansion home)
			{
				return home.GarageId == _0024VB_0024Local_id;
			}

			[SpecialName]
			internal bool _Lambda_0024__1(ApartmentClass apartment)
			{
				if (apartment.ID == _0024VB_0024Local_id)
				{
					return apartment.HasCharacterProfile();
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__144_002D0
		{
			public int _0024VB_0024Local_id;

			[SpecialName]
			internal bool _Lambda_0024__0(ApartmentClass x)
			{
				return x.ID == _0024VB_0024Local_id;
			}

			[SpecialName]
			internal bool _Lambda_0024__1(Mansion x)
			{
				return x.GarageId == _0024VB_0024Local_id;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__84_002D0
		{
			public string _0024VB_0024Local_phase;

			public _Closure_0024__84_002D0(_Closure_0024__84_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_phase = arg0._0024VB_0024Local_phase;
				}
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__84_002D1
		{
			public List<SettingsExpectation> _0024VB_0024Local_expected;

			public _Closure_0024__84_002D0 _0024VB_0024NonLocal__0024VB_0024Closure_2;

			public _Closure_0024__84_002D1(_Closure_0024__84_002D1 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_expected = arg0._0024VB_0024Local_expected;
				}
			}

			[SpecialName]
			internal void _Lambda_0024__0(string candidatePath)
			{
				_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_phase = "copy original settings";
				if (File.Exists(iniPath))
				{
					File.Copy(iniPath, candidatePath, true);
				}
				_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_phase = "read staged settings";
				ScriptSettings scriptSettings = ScriptSettings.Load(candidatePath);
				_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_phase = "populate mansion settings";
				PopulateMansionSettings(scriptSettings, _0024VB_0024Local_expected);
				_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_phase = "write staged settings";
				if (!scriptSettings.Save())
				{
					throw new IOException("The staged mansion settings could not be written.");
				}
			}

			[SpecialName]
			internal void _Lambda_0024__1(string candidatePath)
			{
				_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_phase = ((Operators.CompareString(candidatePath, Path.GetFullPath(iniPath), false) == 0) ? "verify published settings" : "verify staged settings");
				if (!File.Exists(candidatePath) || new FileInfo(candidatePath).Length == 0L)
				{
					throw new IOException("The staged mansion settings are empty.");
				}
				ScriptSettings scriptSettings = ScriptSettings.Load(candidatePath);
				foreach (SettingsExpectation item in _0024VB_0024Local_expected)
				{
					if (Operators.CompareString(scriptSettings.GetValue(item.Section, item.Key, "__SPA_MISSING_SETTING__"), item.Value, false) != 0)
					{
						throw new IOException("Mansion settings verification failed for " + item.Section + "/" + item.Key);
					}
				}
				_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_phase = "flush or publish verified settings";
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__97_002D0
		{
			public Mansion _0024VB_0024Local_m;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				if (!_0024VB_0024Local_m.IplsLoaded)
				{
					LoadIpls(_0024VB_0024Local_m);
				}
				return _0024VB_0024Local_m.IplsLoaded;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__98_002D0
		{
			public Mansion _0024VB_0024Local_m;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				if (!_0024VB_0024Local_m.SetsApplied)
				{
					ApplySets(_0024VB_0024Local_m, true);
					return false;
				}
				return MansionInteriorPairIsReady(_0024VB_0024Local_m);
			}
		}

		public const float NearDistance = 250f;

		public const float FarDistance = 350f;

		private static float BuyRadius = 6f;

		public static List<Mansion> MansionList = new List<Mansion>();

		private static ScriptSettings mansionConfig;

		private static readonly string iniPath = "scripts\\SPA II\\Mansions.ini";

		internal static Mansion currentMansion;

		private static int lastScan = 0;

		private static UIMenuListItem styleItem;

		private static UIMenuListItem wallpaperItem;

		private static UIMenuListItem tintItem;

		private static UIMenuListItem assistantItem;

		private static UIMenuListItem decoItem;

		private static UIMenuListItem artCollectionItem;

		private static UIMenuCheckboxItem windowShuttersItem;

		private static UIMenuCheckboxItem catItem;

		private static UIMenuCheckboxItem dogItem;

		private static UIMenuCheckboxItem posterItem;

		private static UIMenuCheckboxItem orientationItem;

		private static readonly string[] StyleKeys = new string[3] { "cali", "loft", "holly" };

		private static readonly string[] StyleNames = new string[3] { "San Andreas Coastal", "Los Santos Loft", "Vinewood Regency" };

		private static readonly string[] WallpaperKeys = new string[6] { "rustic", "deco", "coastal", "subtle", "safari", "popart" };

		private static readonly string[] TintNames = new string[4] { "Cream", "Mint", "Lavender", "Salmon" };

		private static readonly string[] AssistantKeys = new string[3] { "og", "haviland", "angel" };

		private static readonly string[] AssistantNames = new string[3] { "OG", "Haviland", "Angel" };

		private static readonly string[] DecoKeys = new string[5] { "none", "xmas", "halloween", "lunar", "birthday" };

		private static readonly string[] DecoNames = new string[5] { "None", "Christmas", "Halloween", "Lunar New Year", "Birthday" };

		private static readonly string[] ArtCollectionKeys = new string[4] { "style", "coastal", "loft", "regency" };

		private static readonly string[] ArtCollectionNames = new string[4] { "Match the mansion style", "Coastal", "Loft", "Regency" };

		private static readonly string[] AllStyleSets = new string[15]
		{
			"set_style_cali", "set_elev_cali", "set_art_coastal", "set_trophy_planter", "set_shelving_planter", "set_style_loft", "set_elev_loft", "set_art_loft", "set_loft_trophy_planter", "set_loft_shelving_planter",
			"set_style_holly", "set_elev_holly", "set_art_regency", "set_reg_shelving_planter", "set_reg_trophy_planter"
		};

		private static readonly string[] AllWallpaperSets = new string[6] { "set_wallpaper_rustic", "set_wallpaper_deco", "set_wallpaper_coastal", "set_wallpaper_subtle", "set_wallpaper_safari", "set_wallpaper_popart" };

		private static readonly string[] AllDecoSets = new string[4] { "set_xmas", "set_halloween", "set_lunar", "set_birthday" };

		private static readonly string[] AllAssistantSets = new string[3] { "set_ai_tablets_01", "set_ai_tablets_02", "set_ai_tablets_03" };

		private static readonly string[] AllBaseAssistantSets = new string[3] { "set_base_ai_tablets_01", "set_base_ai_tablets_02", "set_base_ai_tablets_03" };

		private static readonly string[] AllBaseDecoSets = new string[3] { "set_base_xmas", "set_base_halloween", "set_base_new_year" };

		private static readonly Hash H_ACTIVATE_INTERIOR_ENTITY_SET = Hash.ACTIVATE_INTERIOR_ENTITY_SET;

		private static readonly Hash H_DEACTIVATE_INTERIOR_ENTITY_SET = Hash.DEACTIVATE_INTERIOR_ENTITY_SET;

		private static readonly Hash H_IS_INTERIOR_ENTITY_SET_ACTIVE = Hash.IS_INTERIOR_ENTITY_SET_ACTIVE;

		private static readonly Hash H_SET_INTERIOR_ENTITY_SET_TINT = Hash.SET_INTERIOR_ENTITY_SET_TINT_INDEX;

		private static readonly Hash H_REFRESH_INTERIOR = Hash.REFRESH_INTERIOR;

		private static readonly Hash H_GET_INTERIOR_AT_COORDS = Hash.GET_INTERIOR_AT_COORDS;

		private static readonly Hash H_REQUEST_IPL = Hash.REQUEST_IPL;

		private static readonly Hash H_REMOVE_IPL = Hash.REMOVE_IPL;

		private static readonly Hash H_IS_IPL_ACTIVE = Hash.IS_IPL_ACTIVE;

		private static readonly Hash H_IS_VALID_INTERIOR = Hash.IS_VALID_INTERIOR;

		private static readonly Hash H_PIN_INTERIOR_IN_MEMORY = Hash.PIN_INTERIOR_IN_MEMORY;

		private static readonly Dictionary<Mansion, int> nextSignAttempt = new Dictionary<Mansion, int>();

		private static readonly Dictionary<Mansion, OwnedSaleSign> ownedSaleSigns = new Dictionary<Mansion, OwnedSaleSign>();

		private static readonly HashSet<Mansion> pendingSignRemoval = new HashSet<Mansion>();

		public const float GateOpenDistance = 20f;

		public const float GateCloseDistance = 28f;

		private const float GateOpenAngle = 90f;

		private const float GateSpeed = 40f;

		public static string GateMode = "Swing";

		private static Dictionary<string, ApartmentClass> mansionApts = new Dictionary<string, ApartmentClass>();

		[System.Runtime.CompilerServices.AccessedThroughProperty("MansionBuyMenu")]
		private static UIMenu _MansionBuyMenu;

		public static UIMenu MansionBuyMenu
		{
			[CompilerGenerated]
			get
			{
				return _MansionBuyMenu;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = MansionBuyMenu_OnItemSelect;
				UIMenu mansionBuyMenu = _MansionBuyMenu;
				if (mansionBuyMenu != null)
				{
					mansionBuyMenu.OnItemSelect -= value2;
				}
				_MansionBuyMenu = value;
				mansionBuyMenu = _MansionBuyMenu;
				if (mansionBuyMenu != null)
				{
					mansionBuyMenu.OnItemSelect += value2;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("MansionMenu")]
		private static UIMenu _MansionMenu;

		public static UIMenu MansionMenu
		{
			[CompilerGenerated]
			get
			{
				return _MansionMenu;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ListChangedEvent value2 = MansionMenu_OnListChange;
				CheckboxChangeEvent value3 = MansionMenu_OnCheckboxChange;
				ItemSelectEvent value4 = MansionMenu_OnItemSelect;
				UIMenu mansionMenu = _MansionMenu;
				if (mansionMenu != null)
				{
					mansionMenu.OnListChange -= value2;
					mansionMenu.OnCheckboxChange -= value3;
					mansionMenu.OnItemSelect -= value4;
				}
				_MansionMenu = value;
				mansionMenu = _MansionMenu;
				if (mansionMenu != null)
				{
					mansionMenu.OnListChange += value2;
					mansionMenu.OnCheckboxChange += value3;
					mansionMenu.OnItemSelect += value4;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("MansionGarageMenu")]
		private static UIMenu _MansionGarageMenu;

		public static UIMenu MansionGarageMenu
		{
			[CompilerGenerated]
			get
			{
				return _MansionGarageMenu;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = MansionGarageMenu_OnItemSelect;
				UIMenu mansionGarageMenu = _MansionGarageMenu;
				if (mansionGarageMenu != null)
				{
					mansionGarageMenu.OnItemSelect -= value2;
				}
				_MansionGarageMenu = value;
				mansionGarageMenu = _MansionGarageMenu;
				if (mansionGarageMenu != null)
				{
					mansionGarageMenu.OnItemSelect += value2;
				}
			}
		}

		private static string ArtCollectionSet(string style, string collection)
		{
			switch (collection)
			{
			case "coastal":
				return "set_art_coastal";
			case "loft":
				return "set_art_loft";
			case "regency":
				return "set_art_regency";
			default:
				if (Operators.CompareString(style, "loft", false) != 0)
				{
					if (Operators.CompareString(style, "holly", false) == 0)
					{
						return "set_art_regency";
					}
					return "set_art_coastal";
				}
				return "set_art_loft";
			}
		}

		private static string[] StyleSets(string style, string collection)
		{
			string text = ArtCollectionSet(style, collection);
			if (Operators.CompareString(style, "loft", false) != 0)
			{
				if (Operators.CompareString(style, "holly", false) == 0)
				{
					return new string[5] { "set_style_holly", "set_elev_holly", text, "set_reg_shelving_planter", "set_reg_trophy_planter" };
				}
				return new string[5] { "set_style_cali", "set_elev_cali", text, "set_trophy_planter", "set_shelving_planter" };
			}
			return new string[5] { "set_style_loft", "set_elev_loft", text, "set_loft_trophy_planter", "set_loft_shelving_planter" };
		}

		public static bool EntitySetChangeRequired(bool active, bool desired)
		{
			return active != desired;
		}

		private static bool SetEntitySet(int interiorId, string setName, bool enable)
		{
			if (interiorId == 0)
			{
				return false;
			}
			if (!EntitySetChangeRequired(Function.Call<bool>(H_IS_INTERIOR_ENTITY_SET_ACTIVE, interiorId, setName), enable))
			{
				return false;
			}
			if (enable)
			{
				Function.Call(H_ACTIVATE_INTERIOR_ENTITY_SET, interiorId, setName);
			}
			else
			{
				Function.Call(H_DEACTIVATE_INTERIOR_ENTITY_SET, interiorId, setName);
			}
			return true;
		}

		private static bool UpdateEntitySetGroup(int interiorId, string[] setNames, params string[] selected)
		{
			bool result = default(bool);
			foreach (string text in setNames)
			{
				if (!selected.Contains(text) && SetEntitySet(interiorId, text, false))
				{
					result = true;
				}
			}
			foreach (string setName in selected)
			{
				if (SetEntitySet(interiorId, setName, true))
				{
					result = true;
				}
			}
			return result;
		}

		public static void OpenMansionUpgradeRooms(int interiorId, bool refresh = false)
		{
			bool flag = UpdateMansionUpgradeRooms(interiorId);
			if (refresh && flag)
			{
				Function.Call(H_REFRESH_INTERIOR, interiorId);
			}
		}

		private static bool UpdateMansionUpgradeRooms(int interiorId)
		{
			if (interiorId == 0)
			{
				return false;
			}
			string[] array = new string[6] { "SET_PODIUM_BLOCKER", "SET_MOD_BLOCKER", "SET_ARMORY_BLOCKER", "SET_GAR_MOD_BLOCKER", "SET_GAR_PODIUM_BLOCKER", "SET_ARCADE_BLOCKER" };
			bool result = default(bool);
			foreach (string setName in array)
			{
				if (SetEntitySet(interiorId, setName, false))
				{
					result = true;
				}
			}
			return result;
		}

		private static void RequestIpl(string name)
		{
			if (!Function.Call<bool>(H_IS_IPL_ACTIVE, name))
			{
				Function.Call(H_REQUEST_IPL, name);
			}
		}

		private static void RemoveIpl(string name)
		{
			if (Function.Call<bool>(H_IS_IPL_ACTIVE, name))
			{
				Function.Call(H_REMOVE_IPL, name);
			}
		}

		private static int InteriorAt(Vector3 pos)
		{
			if (pos == Vector3.Zero)
			{
				return 0;
			}
			return Function.Call<int>(H_GET_INTERIOR_AT_COORDS, pos.X, pos.Y, pos.Z);
		}

		private static List<Mansion> DefaultMansions()
		{
			List<Mansion> list = new List<Mansion>();
			Mansion mansion = new Mansion();
			mansion.KnownInteriorId = 304385;
			mansion.KnownBasementId = 304641;
			mansion.GarageId = 9001;
			mansion.Key = "MANSION_VINEWOOD";
			mansion.Name = "The Vinewood Residence";
			mansion.Description = "Luxury mansion in the Vinewood Hills with a 3-storey interior, gym, pool and helipad.";
			mansion.Price = 12200000;
			mansion.Reference = new Vector3(543.852f, 712.754f, 201f);
			mansion.InteriorProbe = new Vector3(539.7012f, 749.0894f, 201.3617f);
			mansion.BasementProbe = Vector3.Zero;
			mansion.SignPos = new Quaternion(555.7825f, 800.2668f, 201.0036f, 121.4658f);
			mansion.WardrobePos = new Quaternion(563.6439f, 728.189f, 198.7078f, 161.9352f);
			mansion.DefaultGaragePos = new Quaternion(521.6802f, 761.3641f, 202.0141f, 168.9743f);
			mansion.DefaultBedPos = new Quaternion(560.4256f, 719.0443f, 198.7076f, 128.0032f);
			mansion.DefaultGates = new GateDef[2]
			{
				new GateDef(-1249591818, 537.0177f, 777.0928f, 202.3633f, 128.1952f),
				new GateDef(546378757, 540.4052f, 772.7877f, 202.3633f, 128.1952f)
			};
			mansion.GenericIpl = "apa_ch2_04_mansion_generic";
			mansion.Ipls = new string[14]
			{
				"m25_2_ch2_04_mansion_interior_a", "m25_2_ch2_04_mansion_interior_b", "m25_2_ch2_04_mansion_interior_c", "apa_ch2_04_mansion_shared", "apa_ch2_04_mansion_private", "apa_ch2_04_mansion_furniture", "apa_ch2_04_mansion_firepit", "apa_ch2_04_mansion_railings_p", "m25_2_east_mansion_gym", "m25_2_east_dog_house",
				"m25_2_mansion_props", "hei_ch1_roads_mansion", "apa_ch2_04_mansion_shared_distantlights", "apa_ch2_04_mansion_shared_lodlights"
			};
			Mansion item = mansion;
			list.Add(item);
			mansion = new Mansion();
			mansion.KnownInteriorId = 302593;
			mansion.KnownBasementId = 303105;
			mansion.GarageId = 9002;
			mansion.Key = "MANSION_RICHMAN";
			mansion.Name = "Richman Villa";
			mansion.Description = "The most exclusive address in Richman: mansion with gym, pool, helipad and a private basement.";
			mansion.Price = 12800000;
			mansion.Reference = new Vector3(-1630.434f, 470.852f, 128f);
			mansion.InteriorProbe = new Vector3(-1666.368f, 478.9271f, 128.2216f);
			mansion.BasementProbe = new Vector3(-1679.877f, 493.596f, 56.45711f);
			mansion.SignPos = new Quaternion(-1694.186f, 491.8099f, 128.9693f, 155.585f);
			mansion.WardrobePos = new Quaternion(-1638.324f, 494.5627f, 125.5676f, 219.5001f);
			mansion.DefaultGates = new GateDef[2]
			{
				new GateDef(-1249591818, -1693.499f, 486.2354f, 129.2232f, 196.0495f),
				new GateDef(546378757, -1688.234f, 487.7502f, 129.2232f, 195.7743f)
			};
			mansion.GenericIpl = "hei_ch1_06e_mansion_generic";
			mansion.Ipls = new string[15]
			{
				"hei_ch1_06e_mansion_shared", "hei_ch1_06f_mansion_shared", "m25_2_ch1_06e_mansion_interior_a", "m25_2_ch1_06e_mansion_interior_b", "m25_2_ch1_06e_mansion_interior_c", "hei_ch1_06e_mansion_private", "hei_ch1_06e_mansion_furniture", "hei_ch1_06e_mansion_firepit", "hei_ch1_06e_mansion_railings_p", "m25_2_mansion_gym",
				"m25_2_dog_house", "m25_2_mansion_props", "hei_ch1_roads_mansion", "hei_ch1_06e_mansion_shared_distantlights", "hei_ch1_06e_mansion_shared_lodlights"
			};
			Mansion mansion2 = mansion;
			mansion2.DefaultGaragePos = OriginalGarageDriveway(mansion2);
			list.Add(mansion2);
			mansion = new Mansion();
			mansion.KnownInteriorId = 303617;
			mansion.KnownBasementId = 303873;
			mansion.GarageId = 9003;
			mansion.Key = "MANSION_TONGVA";
			mansion.Name = "The Tongva Estate";
			mansion.Description = "Secluded estate in the Tongva Hills with ocean views, gym, pool and helipad.";
			mansion.Price = 11500000;
			mansion.Reference = new Vector3(-2601.712f, 1874.826f, 166f);
			mansion.InteriorProbe = new Vector3(-2586.065f, 1909.995f, 166.3754f);
			mansion.BasementProbe = Vector3.Zero;
			mansion.SignPos = new Quaternion(-2550.365f, 1912.886f, 169.2083f, 157.7942f);
			mansion.WardrobePos = new Quaternion(-2576.583f, 1879.015f, 163.7215f, 43.32806f);
			mansion.DefaultGates = new GateDef[2]
			{
				new GateDef(-1249591818, -2556.632f, 1915.706f, 169.0264f, 62.9334f),
				new GateDef(546378757, -2559.125f, 1910.828f, 169.0264f, 62.9334f)
			};
			mansion.GenericIpl = "hei_ch1_09_mansion_generic";
			mansion.Ipls = new string[14]
			{
				"hei_ch1_09_mansion_shared", "m25_2_ch1_09_mansion_interior_a", "m25_2_ch1_09_mansion_interior_b", "m25_2_ch1_09_mansion_interior_c", "hei_ch1_09_mansion_private", "hei_ch1_09_mansion_furniture", "hei_ch1_09_mansion_firepit", "hei_ch1_09_mansion_railings_p", "m25_2_tongva_mansion_gym", "m25_2_tongva_dog_house",
				"m25_2_mansion_props", "hei_ch1_roads_mansion", "hei_ch1_09_mansion_shared_distantlights", "hei_ch1_09_mansion_shared_lodlights"
			};
			Mansion mansion3 = mansion;
			mansion3.DefaultGaragePos = OriginalGarageDriveway(mansion3);
			list.Add(mansion3);
			foreach (Mansion item2 in list)
			{
				Mansion mansion4 = item2;
			}
			return list;
		}

		public static Quaternion OriginalGarageDriveway(Mansion m)
		{
			if (m == null || MansionAutoLayout.GarageScriptAnchor(m) == Vector3.Zero)
			{
				return default(Quaternion);
			}
			return MansionAutoLayout.GaragePoint(m, -12.8054f, -18.0868f, 11.1556f, 178.805f);
		}

		public static bool GarageDrivewayValid(Quaternion point)
		{
			if (!float.IsNaN(point.X) && !float.IsInfinity(point.X) && !float.IsNaN(point.Y) && !float.IsInfinity(point.Y) && !float.IsNaN(point.Z) && !float.IsInfinity(point.Z) && !float.IsNaN(point.W) && !float.IsInfinity(point.W))
			{
				return MansionFeatures.IsSet(point);
			}
			return false;
		}

		public static Quaternion ResolveGarageDriveway(Quaternion configured, Quaternion fallback)
		{
			if (GarageDrivewayValid(configured))
			{
				return configured;
			}
			return GarageDrivewayValid(fallback) ? fallback : default(Quaternion);
		}

		public static bool IsInOriginalGarageDriveway(Mansion m, Vector3 position)
		{
			if (m == null || !PartyPlacement.FinitePoint(position))
			{
				return false;
			}
			Vector3 vector = MansionAutoLayout.GarageScriptAnchor(m);
			if (vector == Vector3.Zero)
			{
				return false;
			}
			double num = (double)MansionAutoLayout.MansionHeading(m) * Math.PI / 180.0;
			double num2 = Math.Cos(num);
			double num3 = Math.Sin(num);
			double num4 = (double)position.X - (double)vector.X;
			double num5 = (double)position.Y - (double)vector.Y;
			double num6 = num2 * num4 + num3 * num5;
			double num7 = (0.0 - num3) * num4 + num2 * num5;
			double num8 = (double)position.Z - (double)vector.Z;
			return Math.Abs(num6 + 12.669) <= 3.0 && num7 >= -25.215 && num7 <= -13.055 && num8 >= 10.781 && num8 <= 13.326;
		}

		public static bool IsAtGarageDriveway(Mansion m, Vector3 position)
		{
			if (m == null || !GarageDrivewayValid(m.GaragePos) || !PartyPlacement.FinitePoint(position))
			{
				return false;
			}
			if (Mansion.Dist2D(position, Helper.ToVector3(m.GaragePos)) < 4f && Math.Abs(position.Z - m.GaragePos.Z) < 4f)
			{
				return true;
			}
			return GarageDrivewayValid(m.DefaultGaragePos) && Helper.ToVector3(m.GaragePos).DistanceTo(Helper.ToVector3(m.DefaultGaragePos)) < 0.1f && MansionFeatures.HeadingDiff(m.GaragePos.W, m.DefaultGaragePos.W) < 0.1f && IsInOriginalGarageDriveway(m, position);
		}

		private static Quaternion ReadQ(string section, string prefix, Quaternion def)
		{
			float x = ReadCoordinate(section, prefix + "X", def.X);
			float y = ReadCoordinate(section, prefix + "Y", def.Y);
			float z = ReadCoordinate(section, prefix + "Z", def.Z);
			float w = ReadCoordinate(section, prefix + "Heading", def.W);
			return new Quaternion(x, y, z, w);
		}

		private static float ReadCoordinate(string section, string key, float fallback)
		{
			string text = mansionConfig.GetValue(section, key, "").Trim();
			if (text.Length == 0)
			{
				return fallback;
			}
			if (text.Contains(",") && !text.Contains("."))
			{
				text = text.Replace(",", ".");
			}
			float result;
			if (float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out result) && !float.IsNaN(result) && !float.IsInfinity(result))
			{
				return result;
			}
			Logger.Log($"Mansion coordinate {section}/{key}: invalid value, using default");
			return fallback;
		}

		private static void RecordMansionSetting(ScriptSettings settings, string section, string key, List<SettingsExpectation> expected)
		{
			expected?.Add(new SettingsExpectation
			{
				Section = section,
				Key = key,
				Value = settings.GetValue(section, key, "__SPA_MISSING_SETTING__")
			});
		}

		private static void SetMansionSetting<T>(ScriptSettings settings, string section, string key, T value, List<SettingsExpectation> expected)
		{
			settings.SetValue(section, key, value);
			RecordMansionSetting(settings, section, key, expected);
		}

		private static void WriteQ(string section, string prefix, Quaternion q, ScriptSettings settings = null, List<SettingsExpectation> expected = null)
		{
			ScriptSettings settings2 = settings ?? mansionConfig;
			SetMansionSetting(settings2, section, prefix + "X", q.X, expected);
			SetMansionSetting(settings2, section, prefix + "Y", q.Y, expected);
			SetMansionSetting(settings2, section, prefix + "Z", q.Z, expected);
			SetMansionSetting(settings2, section, prefix + "Heading", q.W, expected);
		}

		private static eOwner MansionLegacyOwner(ScriptSettings settings, string key)
		{
			string value = settings.GetValue<string>(key, "LegacyOwner", null);
			if (value == null)
			{
				value = settings.GetValue<string>(key, "Owner", null);
			}
			eOwner owner = default(eOwner);
			if (!PropertyTransactions.ReadOwnerValue(value, ref owner))
			{
				return eOwner.Others;
			}
			return owner;
		}

		internal static eOwner ReadMansionProfileOwner(ScriptSettings settings, string key, eOwner character)
		{
			eOwner eOwner2 = MansionLegacyOwner(settings, key);
			if (!PropertyProfiles.IsCharacter(character))
			{
				return eOwner2;
			}
			string value = settings.GetValue<string>(PropertyProfiles.Section(key, character), "Owner", null);
			if (value == null)
			{
				if (eOwner2 == eOwner.Others)
				{
					return eOwner.Others;
				}
				return (eOwner2 == character) ? character : eOwner.Nobody;
			}
			eOwner owner = default(eOwner);
			if (!PropertyTransactions.ReadOwnerValue(value, ref owner) || (owner != character && owner != eOwner.Nobody))
			{
				return eOwner.Others;
			}
			return owner;
		}

		private static void LoadMansionProfile(Mansion m, Mansion defaults, eOwner character)
		{
			string section = m.ProfileSection(character);
			m.Owner = ReadMansionProfileOwner(mansionConfig, m.Key, character);
			m.Style = mansionConfig.GetValue(section, "Style", defaults.Style);
			m.Wallpaper = mansionConfig.GetValue(section, "Wallpaper", defaults.Wallpaper);
			m.Tint = mansionConfig.GetValue(section, "Tint", defaults.Tint);
			m.Assistant = mansionConfig.GetValue(section, "Assistant", defaults.Assistant);
			m.Decoration = mansionConfig.GetValue(section, "Decoration", defaults.Decoration);
			m.PetCat = mansionConfig.GetValue(section, "PetCat", defaults.PetCat);
			m.PetDog = mansionConfig.GetValue(section, "PetDog", defaults.PetDog);
			m.MichaelPoster = mansionConfig.GetValue(section, "MichaelPoster", defaults.MichaelPoster);
			string value = mansionConfig.GetValue(section, "ArtCollection", defaults.ArtCollection);
			m.ArtCollection = ArtCollectionKeys[IndexOfKey(ArtCollectionKeys, value)];
			m.WindowShutters = mansionConfig.GetValue(section, "WindowShutters", defaults.WindowShutters);
			int num = 0;
			checked
			{
				do
				{
					string value2 = mansionConfig.GetValue(section, $"TrophyCase{num + 1}", "none");
					m.TrophyChoices[num] = MansionTrophyCases.NormalizeChoice(value2);
					num++;
				}
				while (num <= 4);
				int num2 = m.CashPoints.Count - 1;
				for (int i = 0; i <= num2; i++)
				{
					m.CashPoints[i].Taken = mansionConfig.GetValue(section, $"Cash{i}Taken", "");
				}
				m.PodiumUid = mansionConfig.GetValue(section, "PodiumUid", 0);
				m.PodiumRotate = mansionConfig.GetValue(section, "PodiumRotate", false);
				MansionLeisure.LoadConfig(mansionConfig, m);
			}
		}

		internal static void ApplyMansionCharacterProfiles()
		{
			if (mansionConfig == null)
			{
				return;
			}
			MansionFeatures.MansionFeaturesCleanup();
			MansionSecurity.Initialize();
			mansionApts.Clear();
			Dictionary<string, Mansion> dictionary = DefaultMansions().ToDictionary((_Closure_0024__._0024I80_002D0 != null) ? _Closure_0024__._0024I80_002D0 : (_Closure_0024__._0024I80_002D0 = _Closure_0024__._0024I._Lambda_0024__80_002D0), StringComparer.Ordinal);
			foreach (Mansion mansion in MansionList)
			{
				Mansion value = null;
				if (!dictionary.TryGetValue(mansion.Key, out value))
				{
					continue;
				}
				LoadMansionProfile(mansion, value, PropertyProfiles.ActiveCharacter);
				mansion.SetsApplied = false;
				mansion.SetsPending = false;
				mansion.AppliedSetSignature = string.Empty;
				mansion.AppliedTint = null;
				if (!mansion.Enabled)
				{
					continue;
				}
				RefreshBlip(mansion);
				if (mansion.NearBy)
				{
					if (mansion.Owner == eOwner.Nobody)
					{
						SpawnSaleSign(mansion);
					}
					else
					{
						RemoveSaleSign(mansion);
					}
				}
			}
		}

		public static void LoadMansions()
		{
			checked
			{
				try
				{
					MansionList.Clear();
					bool flag = File.Exists(iniPath);
					mansionConfig = ScriptSettings.Load(iniPath);
					bool value = mansionConfig.GetValue("SETTING", "EnableMansions", true);
					BuyRadius = Math.Max(2f, mansionConfig.GetValue("SETTING", "BuyRadius", 6f));
					GateMode = mansionConfig.GetValue("SETTING", "GateMode", "Swing");
					MansionFeatures.WorkshopPricePercent = Math.Max(0, mansionConfig.GetValue("SETTING", "WorkshopPricePercent", 100));
					MansionFeatures.BuffMinutes = Math.Max(1, mansionConfig.GetValue("SETTING", "GymBuffMinutes", 30));
					MansionFeatures.ArmoryCost = Math.Max(0, mansionConfig.GetValue("SETTING", "ArmoryCost", 2500));
					string value2 = mansionConfig.GetValue("SETTING", "AssistantKey", "");
					MansionFeatures.AssistantKey = MansionFeatures.ParseMansionShortcut(value2, Keys.PageDown);
					string value3 = mansionConfig.GetValue("SETTING", "MansionMenuKey", "");
					MansionFeatures.MansionMenuKey = MansionFeatures.ParseMansionShortcut(value3, Keys.I);
					string value4 = mansionConfig.GetValue("SETTING", "AssistantVoiceMode", "");
					MansionOriginalVoice.VoiceMode = MansionOriginalVoice.NormalizeMode(value4);
					MansionVault.VaultCashAmount = Math.Max(0, mansionConfig.GetValue("SETTING", "VaultCashAmount", 10000));
					MansionVault.VaultRefillHours = Math.Max(1, mansionConfig.GetValue("SETTING", "VaultRefillHours", 24));
					MansionVault.VaultHideTaken = mansionConfig.GetValue("SETTING", "VaultHideTaken", true);
					MansionOrientation.Enabled = mansionConfig.GetValue("SETTING", "InteriorBlips", true);
					bool flag2 = default(bool);
					foreach (Mansion item in DefaultMansions())
					{
						item.Enabled = mansionConfig.GetValue(item.Key, "Enabled", true) && value;
						item.LegacyOwner = MansionLegacyOwner(mansionConfig, item.Key);
						item.Price = Math.Max(0, mansionConfig.GetValue(item.Key, "Price", item.Price));
						Quaternion signPos = item.SignPos;
						Quaternion wardrobePos = item.WardrobePos;
						item.SignPos = ReadQ(item.Key, "Sign", item.SignPos);
						item.SignSet = mansionConfig.GetValue(item.Key, "SignSet", false);
						if (!item.SignSet && Mansion.Dist2D(Helper.ToVector3(item.SignPos), item.Reference) < 1f)
						{
							item.SignPos = signPos;
						}
						item.SignSet = true;
						item.WardrobePos = ReadQ(item.Key, "Wardrobe", item.WardrobePos);
						if (Helper.ToVector3(item.WardrobePos) == Vector3.Zero)
						{
							item.WardrobePos = wardrobePos;
						}
						Quaternion quaternion = ReadQ(item.Key, "Garage", default(Quaternion));
						item.GaragePos = ResolveGarageDriveway(quaternion, item.DefaultGaragePos);
						if (!GarageDrivewayValid(quaternion) && GarageDrivewayValid(item.GaragePos))
						{
							flag2 = true;
						}
						item.BedPos = ReadQ(item.Key, "Bed", item.DefaultBedPos);
						if (!MansionFeatures.IsSet(item.BedPos))
						{
							item.BedPos = item.DefaultBedPos;
						}
						item.SignPropPos = ReadQ(item.Key, "SignProp", new Quaternion(0f, 0f, 0f, 0f));
						item.ElevatorStops.Clear();
						string[] elevatorFloorKeys = MansionFeatures.ElevatorFloorKeys;
						foreach (string text in elevatorFloorKeys)
						{
							Quaternion quaternion2 = ReadQ(item.Key, "Elev" + text, new Quaternion(0f, 0f, 0f, 0f));
							if (MansionFeatures.IsSet(quaternion2))
							{
								item.ElevatorStops[text] = quaternion2;
							}
						}
						item.PodiumPos = ReadQ(item.Key, "Podium", new Quaternion(0f, 0f, 0f, 0f));
						item.WorkshopPos = ReadQ(item.Key, "Workshop", new Quaternion(0f, 0f, 0f, 0f));
						item.HelipadPos = ReadQ(item.Key, "Helipad", new Quaternion(0f, 0f, 0f, 0f));
						item.GymPos = ReadQ(item.Key, "Gym", new Quaternion(0f, 0f, 0f, 0f));
						item.ArmoryPos = ReadQ(item.Key, "Armory", new Quaternion(0f, 0f, 0f, 0f));
						item.VaultPos = ReadQ(item.Key, "Vault", new Quaternion(0f, 0f, 0f, 0f));
						item.BarPos = ReadQ(item.Key, "Bar", new Quaternion(0f, 0f, 0f, 0f));
						item.AssistPanels.Clear();
						int num = 0;
						do
						{
							Quaternion quaternion3 = ReadQ(item.Key, $"Panel{num}", new Quaternion(0f, 0f, 0f, 0f));
							if (MansionFeatures.IsSet(quaternion3))
							{
								item.AssistPanels.Add(quaternion3);
							}
							num++;
						}
						while (num <= 20);
						item.CashPoints.Clear();
						int num2 = 0;
						do
						{
							Quaternion q = ReadQ(item.Key, $"Cash{num2}", new Quaternion(0f, 0f, 0f, 0f));
							if (MansionFeatures.IsSet(q))
							{
								item.CashPoints.Add(new CashPoint
								{
									Pos = Helper.ToVector3(q),
									ModelHash = mansionConfig.GetValue(item.Key, $"Cash{num2}Model", 0),
									Taken = ""
								});
							}
							num2++;
						}
						while (num2 <= 63);
						item.Gates.Clear();
						int num3 = 0;
						do
						{
							int value5 = mansionConfig.GetValue(item.Key, $"Gate{num3}Model", 0);
							if (value5 != 0)
							{
								Quaternion q2 = ReadQ(item.Key, $"Gate{num3}", new Quaternion(0f, 0f, 0f, 0f));
								item.Gates.Add(new GateDef
								{
									ModelHash = value5,
									Position = Helper.ToVector3(q2),
									Heading = q2.W
								});
							}
							num3++;
						}
						while (num3 <= 1);
						if (item.Gates.Count == 0)
						{
							GateDef[] defaultGates = item.DefaultGates;
							foreach (GateDef gateDef in defaultGates)
							{
								item.Gates.Add(new GateDef(gateDef.ModelHash, gateDef.Position.X, gateDef.Position.Y, gateDef.Position.Z, gateDef.Heading));
							}
						}
						MansionAutoLayout.ApplyDefaults(item);
						LoadMansionProfile(item, item, PropertyProfiles.ActiveCharacter);
						MansionList.Add(item);
					}
					if (!flag || flag2 || Operators.CompareString(value2, MansionFeatures.AssistantKey.ToString(), false) != 0 || Operators.CompareString(value3, MansionFeatures.MansionMenuKey.ToString(), false) != 0 || Operators.CompareString(value4, MansionOriginalVoice.VoiceMode, false) != 0)
					{
						SaveMansions();
					}
					Logger.Log($"Mansions: {MansionList.Count} loaded (enabled: {MansionList.FindAll((_Closure_0024__._0024I81_002D0 != null) ? _Closure_0024__._0024I81_002D0 : (_Closure_0024__._0024I81_002D0 = _Closure_0024__._0024I._Lambda_0024__81_002D0)).Count})");
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log($"LoadMansions: {ex2.Message} {ex2.StackTrace}");
					ProjectData.ClearProjectError();
				}
			}
		}

		public static bool SaveMansions()
		{
			try
			{
				using (AtomicFileTransaction atomicFileTransaction = BeginMansionSaveTransaction())
				{
					atomicFileTransaction.Complete();
				}
				RefreshMansionSettingsCache();
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("SaveMansions: " + ex2.ToString());
				Notification.Show("Mansion settings could not be saved. The previous settings file has been kept.");
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		private static void RefreshMansionSettingsCache()
		{
			try
			{
				mansionConfig = ScriptSettings.Load(iniPath);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Committed mansion settings cache refresh: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		internal static AtomicFileTransaction BeginMansionSaveTransaction()
		{
			_Closure_0024__84_002D0 arg = new _Closure_0024__84_002D0(null);
			arg._0024VB_0024Local_phase = "read original settings";
			try
			{
				_Closure_0024__84_002D1 arg2 = new _Closure_0024__84_002D1(null);
				arg2._0024VB_0024NonLocal__0024VB_0024Closure_2 = arg;
				arg2._0024VB_0024Local_expected = new List<SettingsExpectation>();
				return AtomicFileTransaction.BeginWrite(iniPath, arg2._Lambda_0024__0, arg2._Lambda_0024__1);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Mansion settings save failed; phase=" + arg._0024VB_0024Local_phase + "; path=" + Path.GetFullPath(iniPath) + "; settingsAPI=" + typeof(ScriptSettings).Assembly.FullName + "; " + ex2.ToString());
				throw;
			}
		}

		private static void PopulateMansionSettings(ScriptSettings settings, List<SettingsExpectation> expected)
		{
			SetMansionSetting(settings, "SETTING", "EnableMansions", settings.GetValue("SETTING", "EnableMansions", true), expected);
			SetMansionSetting(settings, "SETTING", "BuyRadius", BuyRadius, expected);
			SetMansionSetting(settings, "SETTING", "GateMode", GateMode, expected);
			SetMansionSetting(settings, "SETTING", "WorkshopPricePercent", MansionFeatures.WorkshopPricePercent, expected);
			SetMansionSetting(settings, "SETTING", "GymBuffMinutes", MansionFeatures.BuffMinutes, expected);
			SetMansionSetting(settings, "SETTING", "ArmoryCost", MansionFeatures.ArmoryCost, expected);
			SetMansionSetting(settings, "SETTING", "AssistantKey", MansionFeatures.AssistantKey.ToString(), expected);
			SetMansionSetting(settings, "SETTING", "MansionMenuKey", MansionFeatures.MansionMenuKey.ToString(), expected);
			SetMansionSetting(settings, "SETTING", "AssistantVoiceMode", MansionOriginalVoice.NormalizeMode(MansionOriginalVoice.VoiceMode), expected);
			SetMansionSetting(settings, "SETTING", "VaultCashAmount", MansionVault.VaultCashAmount, expected);
			SetMansionSetting(settings, "SETTING", "VaultRefillHours", MansionVault.VaultRefillHours, expected);
			SetMansionSetting(settings, "SETTING", "VaultHideTaken", MansionVault.VaultHideTaken, expected);
			SetMansionSetting(settings, "SETTING", "InteriorBlips", MansionOrientation.Enabled, expected);
			checked
			{
				foreach (Mansion mansion in MansionList)
				{
					SetMansionSetting(settings, mansion.Key, "Enabled", mansion.Enabled, expected);
					eOwner activeCharacter = PropertyProfiles.ActiveCharacter;
					string section = mansion.ProfileSection(activeCharacter);
					if (PropertyProfiles.IsCharacter(activeCharacter))
					{
						SetMansionSetting(settings, mansion.Key, "LegacyOwner", mansion.LegacyOwner, expected);
						if (settings.GetValue<string>(mansion.Key, "Owner", null) == null)
						{
							SetMansionSetting(settings, mansion.Key, "Owner", mansion.LegacyOwner, expected);
						}
						if (mansion.Owner == activeCharacter || mansion.Owner == eOwner.Nobody)
						{
							SetMansionSetting(settings, PropertyProfiles.Section(mansion.Key, activeCharacter), "Owner", mansion.Owner, expected);
						}
					}
					SetMansionSetting(settings, mansion.Key, "Price", mansion.Price, expected);
					SetMansionSetting(settings, section, "Style", mansion.Style, expected);
					SetMansionSetting(settings, section, "Wallpaper", mansion.Wallpaper, expected);
					SetMansionSetting(settings, section, "Tint", mansion.Tint, expected);
					SetMansionSetting(settings, section, "Assistant", mansion.Assistant, expected);
					SetMansionSetting(settings, section, "Decoration", mansion.Decoration, expected);
					SetMansionSetting(settings, section, "PetCat", mansion.PetCat, expected);
					SetMansionSetting(settings, section, "PetDog", mansion.PetDog, expected);
					SetMansionSetting(settings, section, "MichaelPoster", mansion.MichaelPoster, expected);
					SetMansionSetting(settings, section, "ArtCollection", mansion.ArtCollection, expected);
					SetMansionSetting(settings, section, "WindowShutters", mansion.WindowShutters, expected);
					int num = 0;
					do
					{
						SetMansionSetting(settings, section, $"TrophyCase{num + 1}", MansionTrophyCases.NormalizeChoice(mansion.TrophyChoices[num]), expected);
						num++;
					}
					while (num <= 4);
					MansionLeisure.SaveConfig(settings, mansion);
					string[] array = new string[4] { "CatName", "DogName", "DogBreed", "PetsFollow" };
					foreach (string key in array)
					{
						RecordMansionSetting(settings, section, key, expected);
					}
					WriteQ(mansion.Key, "Sign", mansion.SignPos, settings, expected);
					SetMansionSetting(settings, mansion.Key, "SignSet", mansion.SignSet, expected);
					WriteQ(mansion.Key, "Wardrobe", mansion.WardrobePos, settings, expected);
					WriteQ(mansion.Key, "Garage", mansion.GaragePos, settings, expected);
					WriteQ(mansion.Key, "Bed", mansion.BedPos, settings, expected);
					WriteQ(mansion.Key, "SignProp", mansion.SignPropPos, settings, expected);
					string[] elevatorFloorKeys = MansionFeatures.ElevatorFloorKeys;
					foreach (string text in elevatorFloorKeys)
					{
						WriteQ(mansion.Key, "Elev" + text, MansionFeatures.GetStop(mansion, text), settings, expected);
					}
					WriteQ(mansion.Key, "Podium", mansion.PodiumPos, settings, expected);
					WriteQ(mansion.Key, "Workshop", mansion.WorkshopPos, settings, expected);
					WriteQ(mansion.Key, "Helipad", mansion.HelipadPos, settings, expected);
					WriteQ(mansion.Key, "Gym", mansion.GymPos, settings, expected);
					WriteQ(mansion.Key, "Armory", mansion.ArmoryPos, settings, expected);
					WriteQ(mansion.Key, "Vault", mansion.VaultPos, settings, expected);
					WriteQ(mansion.Key, "Bar", mansion.BarPos, settings, expected);
					int num2 = 0;
					do
					{
						WriteQ(mansion.Key, $"Panel{num2}", (num2 < mansion.AssistPanels.Count) ? mansion.AssistPanels[num2] : new Quaternion(0f, 0f, 0f, 0f), settings, expected);
						num2++;
					}
					while (num2 <= 20);
					int num3 = 0;
					do
					{
						CashPoint cashPoint = ((num3 < mansion.CashPoints.Count) ? mansion.CashPoints[num3] : null);
						WriteQ(mansion.Key, $"Cash{num3}", (cashPoint == null) ? new Quaternion(0f, 0f, 0f, 0f) : new Quaternion(cashPoint.Pos.X, cashPoint.Pos.Y, cashPoint.Pos.Z, 0f), settings, expected);
						SetMansionSetting(settings, mansion.Key, $"Cash{num3}Model", cashPoint?.ModelHash ?? 0, expected);
						SetMansionSetting(settings, section, $"Cash{num3}Taken", (cashPoint == null) ? "" : cashPoint.Taken, expected);
						num3++;
					}
					while (num3 <= 63);
					SetMansionSetting(settings, section, "PodiumUid", mansion.PodiumUid, expected);
					SetMansionSetting(settings, section, "PodiumRotate", mansion.PodiumRotate, expected);
					int num4 = 0;
					do
					{
						if (num4 < mansion.Gates.Count)
						{
							SetMansionSetting(settings, mansion.Key, $"Gate{num4}Model", mansion.Gates[num4].ModelHash, expected);
							WriteQ(mansion.Key, $"Gate{num4}", new Quaternion(mansion.Gates[num4].Position.X, mansion.Gates[num4].Position.Y, mansion.Gates[num4].Position.Z, mansion.Gates[num4].Heading), settings, expected);
						}
						else
						{
							SetMansionSetting(settings, mansion.Key, $"Gate{num4}Model", 0, expected);
						}
						num4++;
					}
					while (num4 <= 1);
				}
			}
		}

		public static void LoadMansionBlips()
		{
			foreach (Mansion mansion in MansionList)
			{
				if (mansion.Enabled)
				{
					RefreshBlip(mansion);
				}
			}
		}

		private static void RefreshBlip(Mansion m)
		{
			try
			{
				if ((object)m.Blip != null && m.Blip.Exists())
				{
					m.Blip.Delete();
				}
				m.Blip = World.CreateBlip(Helper.ToVector3(m.SignPos));
				Blip blip = m.Blip;
				blip.IsShortRange = true;
				if (m.Owner == eOwner.Nobody)
				{
					blip.Sprite = BlipSprite.SafehouseForSale;
					blip.Name = $"{m.Name} - {PropertyOwnershipText.ForSaleLabel(Game.Language == Language.German)} (${m.Price:0,0})";
					blip.Color = BlipColor.White;
				}
				else
				{
					blip.Sprite = BlipSprite.Safehouse;
					blip.Name = m.Name + " - " + PropertyOwnershipText.OwnerLabel(m.Owner, Game.Language == Language.German);
					switch (m.Owner)
					{
					case eOwner.Michael:
						blip.Color = BlipColor.Michael;
						break;
					case eOwner.Franklin:
						blip.Color = BlipColor.Franklin;
						break;
					case eOwner.Trevor:
						blip.Color = BlipColor.Trevor;
						break;
					default:
						blip.Color = BlipColor.Yellow;
						break;
					}
				}
				blip = null;
				SpaBlips.RegisterCoordinate(m.Blip);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Mansion blip: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
		}

		public static bool RequestedMansionIplsReady(IEnumerable<bool> states)
		{
			if (states == null)
			{
				return false;
			}
			bool result = default(bool);
			foreach (bool state in states)
			{
				result = true;
				if (!state)
				{
					return false;
				}
			}
			return result;
		}

		public static bool MansionInteriorPairReady(int mainId, int basementId, bool mainReady, bool basementReady)
		{
			if (mainId != 0 && basementId != 0 && mainId != basementId && mainReady)
			{
				return basementReady;
			}
			return false;
		}

		public static int ResolveTypedMansionInterior(Mansion m, bool basement)
		{
			if (m == null)
			{
				return 0;
			}
			Vector3 vector = (basement ? MansionAutoLayout.BasementAnchor(m) : MansionAutoLayout.MainAnchor(m));
			if (vector == Vector3.Zero || !PartyPlacement.FinitePoint(vector))
			{
				return 0;
			}
			int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS_WITH_TYPE, vector.X, vector.Y, vector.Z, basement ? "m25_2_int_mansion_2" : "m25_2_int_mansion");
			if (num == 0 || !Function.Call<bool>(Hash.IS_VALID_INTERIOR, num))
			{
				return 0;
			}
			return num;
		}

		public static void EnableKnownMansionInterior(int id, Vector3 anchor)
		{
			if (id != 0 && Function.Call<bool>(Hash.IS_VALID_INTERIOR, id))
			{
				if (Function.Call<bool>(Hash.IS_INTERIOR_DISABLED, id))
				{
					Function.Call(Hash.DISABLE_INTERIOR, id, false);
				}
				if (Function.Call<bool>(Hash.IS_INTERIOR_CAPPED, id))
				{
					Function.Call(Hash.CAP_INTERIOR, id, false);
				}
				if (!Stability.IsPinnedInterior(id))
				{
					Stability.PinInterior(id, anchor, false);
				}
			}
		}

		public static bool MansionInteriorPairIsReady(Mansion m)
		{
			if (m == null || m.InteriorId == 0 || m.BasementId == 0)
			{
				return false;
			}
			if (ResolveTypedMansionInterior(m, false) != m.InteriorId || ResolveTypedMansionInterior(m, true) != m.BasementId)
			{
				return false;
			}
			return MansionInteriorPairReady(m.InteriorId, m.BasementId, Function.Call<bool>(Hash.IS_INTERIOR_READY, m.InteriorId), Function.Call<bool>(Hash.IS_INTERIOR_READY, m.BasementId));
		}

		public static Mansion StartupNearbyMansion(Vector3 position)
		{
			if (position == Vector3.Zero || !PartyPlacement.FinitePoint(position))
			{
				return null;
			}
			Mansion result = null;
			float num = 250f;
			foreach (Mansion mansion in MansionList)
			{
				if (mansion != null && mansion.Enabled && !mansion.IplLoadFailed)
				{
					float num2 = position.DistanceTo(mansion.Reference);
					if (num2 < num)
					{
						num = num2;
						result = mansion;
					}
				}
			}
			return result;
		}

		public static bool PrepareStartupNearbyTick(Vector3 position)
		{
			if (!Helper.mpMapActive)
			{
				return false;
			}
			Mansion mansion = StartupNearbyMansion(position);
			if (mansion == null)
			{
				return false;
			}
			MansionTerrain.Tick();
			mansion.NearBy = true;
			if (!mansion.IplsLoaded)
			{
				LoadIpls(mansion);
			}
			if (mansion.IplLoadFailed)
			{
				return false;
			}
			if (!mansion.IplsLoaded)
			{
				return true;
			}
			ApplySets(mansion, true);
			return !mansion.SetsApplied || !MansionInteriorPairIsReady(mansion);
		}

		private static void LoadIpls(Mansion m)
		{
			if (m == null || m.Ipls == null || m.Ipls.Length == 0)
			{
				return;
			}
			if (m.IplRequestClock == null)
			{
				m.IplRequestClock = Stopwatch.StartNew();
			}
			List<bool> list = new List<bool>();
			string[] ipls = m.Ipls;
			foreach (string text in ipls)
			{
				bool flag = Function.Call<bool>(H_IS_IPL_ACTIVE, text);
				list.Add(flag);
				if (!flag && !m.IplLoadFailed && !m.OurIpls.Contains(text))
				{
					Function.Call(H_REQUEST_IPL, text);
					m.OurIpls.Add(text);
				}
			}
			if (!RequestedMansionIplsReady(list))
			{
				if (!m.IplLoadFailed && m.IplRequestClock.ElapsedMilliseconds >= 15000)
				{
					m.IplLoadFailed = true;
					m.IplRequestClock.Stop();
					Logger.Log($"Mansion {m.Name}: required IPLs did not activate; original exterior retained, no repeated IPL requests.");
				}
			}
			else if (MansionTerrain.PrepareReplacement(m))
			{
				m.RemovedGeneric = true;
				m.IplsLoaded = true;
				m.IplLoadFailed = false;
				m.IplRequestClock.Stop();
				Stability.Trace($"mansion {m.Name}: all {list.Count} required IPLs active; generic removed={m.RemovedGeneric}");
			}
		}

		public static bool EnsureMansionMapReady(int timeoutMs = 15000)
		{
			if (Helper.mpMapActive)
			{
				return true;
			}
			if (timeoutMs <= 0 || !ConfigHelper.OnlineMap || Operators.CompareString(ConfigHelper.OnlineMapMode.ToLower(), "off", false) == 0)
			{
				return false;
			}
			if (Helper.SwitchOnlineMap(true))
			{
				return false;
			}
			return Helper.mpMapActive;
		}

		public static bool PrepareMansionContentNow(Mansion m, int timeoutMs = 15000)
		{
			_Closure_0024__97_002D0 closure_0024__97_002D = new _Closure_0024__97_002D0();
			closure_0024__97_002D._0024VB_0024Local_m = m;
			try
			{
				if (closure_0024__97_002D._0024VB_0024Local_m == null || timeoutMs <= 0 || !EnsureMansionMapReady(timeoutMs))
				{
					return false;
				}
				using (DestinationStreaming.SceneLease sceneLease = DestinationStreaming.Acquire(MansionAutoLayout.MainAnchor(closure_0024__97_002D._0024VB_0024Local_m), 120f, timeoutMs))
				{
					if (sceneLease == null)
					{
						return false;
					}
					closure_0024__97_002D._0024VB_0024Local_m.NearBy = true;
					return sceneLease.WaitUntilReady(closure_0024__97_002D._Lambda_0024__0);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"PrepareMansionContentNow {closure_0024__97_002D._0024VB_0024Local_m.Name}: {ex2.Message}");
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public static bool PrepareMansionNow(Mansion m, int timeoutMs = 15000)
		{
			_Closure_0024__98_002D0 closure_0024__98_002D = new _Closure_0024__98_002D0();
			closure_0024__98_002D._0024VB_0024Local_m = m;
			try
			{
				if (closure_0024__98_002D._0024VB_0024Local_m == null || timeoutMs <= 0 || !EnsureMansionMapReady(timeoutMs))
				{
					return false;
				}
				using (DestinationStreaming.SceneLease sceneLease = DestinationStreaming.Acquire(MansionAutoLayout.MainAnchor(closure_0024__98_002D._0024VB_0024Local_m), 120f, timeoutMs))
				{
					if (sceneLease == null || !PrepareMansionContentNow(closure_0024__98_002D._0024VB_0024Local_m, timeoutMs))
					{
						return false;
					}
					bool num = sceneLease.WaitUntilReady(closure_0024__98_002D._Lambda_0024__0);
					if (!num)
					{
						Logger.Log($"PrepareMansionNow {closure_0024__98_002D._0024VB_0024Local_m.Name}: destination scene/IPLs/interiors not ready; player retained at origin.");
					}
					return num;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"PrepareMansionNow {closure_0024__98_002D._0024VB_0024Local_m.Name}: {ex2.Message}");
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public static void ForgetForStoryLoad()
		{
			MansionOrientation.ForgetForStoryLoad();
			MansionCigarLounge.ForgetForStoryLoad();
			MansionTrophyCases.ForgetForStoryLoad();
			MansionFeatures.ForgetPresentationForStoryLoad();
			lastScan = -1000;
			foreach (Mansion mansion in MansionList)
			{
				if (mansion != null)
				{
					mansion.InteriorId = 0;
					mansion.BasementId = 0;
					mansion.IplsLoaded = false;
					mansion.IplLoadFailed = false;
					mansion.IplRequestClock = null;
					mansion.OurIpls.Clear();
					mansion.RemovedGeneric = false;
					mansion.NearBy = false;
					mansion.SetsApplied = false;
					mansion.SetsPending = false;
					mansion.SetsAppliedFrame = -1;
					mansion.AppliedSetSignature = string.Empty;
					mansion.AppliedTint = null;
					mansion.PodiumVeh = null;
					mansion.PodiumVehUid = 0;
					mansion.WorkshopVeh = null;
					mansion.WorkshopVc = null;
					mansion.WorkshopUid = 0;
				}
			}
		}

		public static void UnloadAllMansions(bool forMapSwitch = false)
		{
			MansionOrientation.Cleanup();
			foreach (Mansion mansion in MansionList)
			{
				if (mansion.IplsLoaded || mansion.OurIpls.Count > 0 || mansion.IplRequestClock != null)
				{
					UnloadIpls(mansion, forMapSwitch);
				}
			}
		}

		private static void UnloadIpls(Mansion m, bool forMapSwitch = false)
		{
			_Closure_0024__101_002D0 arg = new _Closure_0024__101_002D0(null);
			arg._0024VB_0024Local_m = m;
			if (!forMapSwitch && !MansionTerrain.PrepareOriginal(arg._0024VB_0024Local_m))
			{
				return;
			}
			MansionTrophyCases.Cleanup(arg._0024VB_0024Local_m);
			MansionSecurity.Release(arg._0024VB_0024Local_m);
			if (MansionNativeGarage.ActiveMansion == arg._0024VB_0024Local_m)
			{
				MansionNativeGarage.Clear();
			}
			Stability.UnpinInterior(arg._0024VB_0024Local_m.InteriorId);
			Stability.UnpinInterior(arg._0024VB_0024Local_m.BasementId);
			using (List<string>.Enumerator enumerator = arg._0024VB_0024Local_m.OurIpls.GetEnumerator())
			{
				_Closure_0024__101_002D1 closure_0024__101_002D = default(_Closure_0024__101_002D1);
				while (enumerator.MoveNext())
				{
					closure_0024__101_002D = new _Closure_0024__101_002D1(closure_0024__101_002D);
					closure_0024__101_002D._0024VB_0024NonLocal__0024VB_0024Closure_2 = arg;
					closure_0024__101_002D._0024VB_0024Local_ipl = enumerator.Current;
					Mansion mansion = MansionList.FirstOrDefault(closure_0024__101_002D._Lambda_0024__0);
					if (mansion != null)
					{
						if (!mansion.OurIpls.Contains(closure_0024__101_002D._0024VB_0024Local_ipl))
						{
							mansion.OurIpls.Add(closure_0024__101_002D._0024VB_0024Local_ipl);
						}
					}
					else
					{
						Function.Call(H_REMOVE_IPL, closure_0024__101_002D._0024VB_0024Local_ipl);
					}
				}
			}
			arg._0024VB_0024Local_m.OurIpls.Clear();
			arg._0024VB_0024Local_m.RemovedGeneric = false;
			arg._0024VB_0024Local_m.IplsLoaded = false;
			arg._0024VB_0024Local_m.IplLoadFailed = false;
			arg._0024VB_0024Local_m.IplRequestClock = null;
			arg._0024VB_0024Local_m.SetsApplied = false;
			arg._0024VB_0024Local_m.SetsPending = false;
			arg._0024VB_0024Local_m.AppliedSetSignature = string.Empty;
			arg._0024VB_0024Local_m.AppliedTint = null;
			arg._0024VB_0024Local_m.InteriorId = 0;
			arg._0024VB_0024Local_m.BasementId = 0;
			Stability.Trace($"mansion {arg._0024VB_0024Local_m.Name}: IPLs unloaded");
		}

		private static bool ValidInteriorNear(int id, Vector3 near)
		{
			try
			{
				if (id == 0 || !Function.Call<bool>(Hash.IS_VALID_INTERIOR, id))
				{
					return false;
				}
				OutputArgument outputArgument = new OutputArgument();
				OutputArgument outputArgument2 = new OutputArgument();
				Function.Call(Hash.GET_INTERIOR_LOCATION_AND_NAMEHASH, id, outputArgument, outputArgument2);
				return outputArgument.GetResult<Vector3>().DistanceTo(near) < 200f;
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		private static string WindowShutterIpl(string key)
		{
			switch (key)
			{
			case "MANSION_VINEWOOD":
				return "apa_ch2_04_mansion_shutters";
			case "MANSION_RICHMAN":
				return "hei_ch1_06e_mansion_shutters";
			case "MANSION_TONGVA":
				return "hei_ch1_09_mansion_shutters";
			default:
				return null;
			}
		}

		private static void ApplyWindowShutters(Mansion m)
		{
			if (m == null || !m.IplsLoaded)
			{
				return;
			}
			string text = WindowShutterIpl(m.Key);
			if (text == null)
			{
				return;
			}
			bool flag = m.OurIpls.Contains(text);
			if (m.WindowShutters)
			{
				if (!flag && !Function.Call<bool>(H_IS_IPL_ACTIVE, text))
				{
					Function.Call(H_REQUEST_IPL, text);
					m.OurIpls.Add(text);
				}
			}
			else if (flag)
			{
				Function.Call(H_REMOVE_IPL, text);
				m.OurIpls.Remove(text);
			}
		}

		public static void ApplySets(Mansion m, bool refresh)
		{
			try
			{
				if (m == null || !m.IplsLoaded)
				{
					return;
				}
				ApplyWindowShutters(m);
				int interiorId = m.InteriorId;
				int basementId = m.BasementId;
				m.InteriorId = ResolveTypedMansionInterior(m, false);
				m.BasementId = ResolveTypedMansionInterior(m, true);
				if (m.InteriorId != 0)
				{
					EnableKnownMansionInterior(m.InteriorId, MansionAutoLayout.MainAnchor(m));
				}
				if (m.BasementId != 0)
				{
					EnableKnownMansionInterior(m.BasementId, MansionAutoLayout.BasementAnchor(m));
				}
				if (m.InteriorId == 0 || m.BasementId == 0 || m.InteriorId == m.BasementId)
				{
					m.SetsApplied = false;
					return;
				}
				string text = string.Join("|", m.Style.ToString(), m.Wallpaper.ToString(), m.Tint.ToString(), m.Assistant, m.Decoration, m.PetCat.ToString(), m.PetDog.ToString(), m.MichaelPoster.ToString(), m.ArtCollection);
				if (m.SetsApplied && !m.SetsPending && interiorId == m.InteriorId && basementId == m.BasementId && Operators.CompareString(m.AppliedSetSignature, text, false) == 0 && MansionInteriorPairIsReady(m))
				{
					return;
				}
				if (m.SetsPending && interiorId == m.InteriorId && basementId == m.BasementId && Operators.CompareString(m.AppliedSetSignature, text, false) == 0)
				{
					m.SetsApplied = Game.FrameCount > m.SetsAppliedFrame && MansionInteriorPairIsReady(m);
					if (m.SetsApplied)
					{
						m.SetsPending = false;
						Logger.Log($"Mansion {m.Name}: both refreshed typed interiors ready ({m.InteriorId}/{m.BasementId}).");
					}
					return;
				}
				int interiorId2 = m.InteriorId;
				int basementId2 = m.BasementId;
				bool flag = string.IsNullOrEmpty(m.AppliedSetSignature) || (m.SetsApplied && Operators.CompareString(m.AppliedSetSignature, text, false) == 0);
				bool flag2 = flag || interiorId != interiorId2;
				bool flag3 = flag || basementId != basementId2;
				string text2 = ((Operators.CompareString(m.Assistant, "og", false) == 0) ? "01" : ((Operators.CompareString(m.Assistant, "haviland", false) == 0) ? "02" : "03"));
				flag2 = UpdateMansionUpgradeRooms(interiorId2) || flag2;
				flag2 = UpdateEntitySetGroup(interiorId2, AllStyleSets, StyleSets(m.Style, m.ArtCollection)) || flag2;
				flag2 = UpdateEntitySetGroup(interiorId2, AllWallpaperSets, "set_wallpaper_" + m.Wallpaper) || flag2;
				bool flag4 = SetEntitySet(interiorId2, "set_style_cali_tint", true);
				if (flag || interiorId != interiorId2 || flag4 || !m.AppliedTint.HasValue || m.AppliedTint.Value != m.Tint)
				{
					Function.Call(H_SET_INTERIOR_ENTITY_SET_TINT, interiorId2, "set_style_cali_tint", m.Tint);
					m.AppliedTint = m.Tint;
					flag2 = true;
				}
				flag2 = UpdateEntitySetGroup(interiorId2, AllAssistantSets, "set_ai_tablets_" + text2) || flag2;
				string[] selected = ((Operators.CompareString(m.Decoration, "none", false) == 0) ? Array.Empty<string>() : new string[1] { "set_" + m.Decoration });
				flag2 = UpdateEntitySetGroup(interiorId2, AllDecoSets, selected) || flag2;
				flag2 = SetEntitySet(interiorId2, "set_pet_cat", m.PetCat) || flag2;
				flag2 = SetEntitySet(interiorId2, "set_pet_dog", m.PetDog) || flag2;
				flag2 = SetEntitySet(interiorId2, "set_michael_poster", m.MichaelPoster) || flag2;
				flag3 = UpdateMansionUpgradeRooms(basementId2) || flag3;
				flag3 = SetEntitySet(basementId2, "set_vault_door_closed", false) || flag3;
				flag3 = SetEntitySet(basementId2, "set_vault_door_open", true) || flag3;
				flag3 = SetEntitySet(basementId2, "set_elev_std", true) || flag3;
				flag3 = UpdateEntitySetGroup(basementId2, AllBaseAssistantSets, "set_base_ai_tablets_" + text2) || flag3;
				string[] selected2 = Array.Empty<string>();
				switch (m.Decoration)
				{
				case "xmas":
					selected2 = new string[1] { "set_base_xmas" };
					break;
				case "halloween":
					selected2 = new string[1] { "set_base_halloween" };
					break;
				case "lunar":
					selected2 = new string[1] { "set_base_new_year" };
					break;
				}
				flag3 = UpdateEntitySetGroup(basementId2, AllBaseDecoSets, selected2) || flag3;
				int num = 0;
				do
				{
					flag3 = SetEntitySet(basementId2, $"set_base_vault_0{num}", true) || flag3;
					num = checked(num + 1);
				}
				while (num <= 4);
				if (refresh && flag2)
				{
					Function.Call(H_REFRESH_INTERIOR, interiorId2);
				}
				if (refresh && flag3)
				{
					Function.Call(H_REFRESH_INTERIOR, basementId2);
				}
				m.AppliedSetSignature = text;
				if (flag2 || flag3)
				{
					m.SetsApplied = false;
					m.SetsPending = true;
					m.SetsAppliedFrame = Game.FrameCount;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				if (m != null)
				{
					m.SetsApplied = false;
					m.SetsPending = false;
					m.AppliedSetSignature = string.Empty;
					m.AppliedTint = null;
				}
				Logger.Log($"ApplySets {m.Name}: {ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		private static float GroundBelow(Vector3 p, float refZ)
		{
			RaycastResult raycastResult = World.Raycast(new Vector3(p.X, p.Y, refZ + 1f), new Vector3(p.X, p.Y, refZ - 4f), IntersectFlags.LosToEntity, Helper.PP);
			if (raycastResult.DidHit)
			{
				return raycastResult.HitPosition.Z;
			}
			return float.NaN;
		}

		private static void SpawnSaleSign(Mansion m)
		{
			if (pendingSignRemoval.Contains(m))
			{
				RemoveSaleSign(m);
				if (pendingSignRemoval.Contains(m))
				{
					return;
				}
			}
			int value;
			if (((object)m.SaleSignProp != null && m.SaleSignProp.Exists()) || (nextSignAttempt.TryGetValue(m, out value) && Game.GameTime < value))
			{
				return;
			}
			nextSignAttempt[m] = checked(Game.GameTime + 3000);
			Model model = new Model(Helper.ForSaleSign);
			try
			{
				model.Request(500);
				if (!model.IsLoaded)
				{
					return;
				}
				OutputArgument outputArgument = new OutputArgument();
				OutputArgument outputArgument2 = new OutputArgument();
				Function.Call(Hash.GET_MODEL_DIMENSIONS, model.Hash, outputArgument, outputArgument2);
				Vector3 result = outputArgument.GetResult<Vector3>();
				Vector3 result2 = outputArgument2.GetResult<Vector3>();
				List<Quaternion> list = new List<Quaternion>();
				if (Helper.ToVector3(m.SignPropPos) != Vector3.Zero)
				{
					list.Add(m.SignPropPos);
				}
				list.AddRange(MansionSignPlacement.Candidates(m, result, result2));
				bool flag = false;
				Quaternion pose = default(Quaternion);
				foreach (Quaternion item in list)
				{
					if (MansionSignPlacement.TryPose(m, item, result, result2, ref pose))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					m.SaleSignProp = World.CreateProp(model, Helper.ToVector3(pose), new Vector3(0f, 0f, pose.W), false, false);
					if ((object)m.SaleSignProp != null && m.SaleSignProp.Exists())
					{
						ownedSaleSigns[m] = new OwnedSaleSign
						{
							Handle = m.SaleSignProp.Handle,
							ModelHash = m.SaleSignProp.Model.Hash,
							Address = m.SaleSignProp.MemoryAddress
						};
						m.SaleSignProp.IsPersistent = true;
						Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, m.SaleSignProp.Handle, pose.X, pose.Y, pose.Z, false, false, false);
						Function.Call(Hash.FREEZE_ENTITY_POSITION, m.SaleSignProp.Handle, true);
						m.BoardPos = Helper.ToVector3(pose);
						nextSignAttempt.Remove(m);
						Logger.Log("Mansion sale sign: " + m.Name + "; beside driveway at " + pose.ToString());
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Mansion sign: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
			finally
			{
				model.MarkAsNoLongerNeeded();
			}
		}

		public static bool ConfirmOwnedSaleSignRemoval(Func<bool> entityExists, Action removeEntity)
		{
			if (entityExists == null || removeEntity == null)
			{
				return false;
			}
			if (!entityExists())
			{
				return true;
			}
			removeEntity();
			return !entityExists();
		}

		public static bool OwnedSaleSignIdentityMatches(int expectedHandle, int expectedModel, IntPtr expectedAddress, int observedHandle, int observedModel, IntPtr observedAddress)
		{
			if (expectedHandle != 0 && expectedModel != 0 && expectedAddress != IntPtr.Zero && expectedHandle == observedHandle && expectedModel == observedModel)
			{
				return expectedAddress == observedAddress;
			}
			return false;
		}

		private static Prop ResolveOwnedSaleSign(OwnedSaleSign receipt)
		{
			if (receipt == null || !Function.Call<bool>(Hash.DOES_ENTITY_EXIST, receipt.Handle))
			{
				return null;
			}
			Entity entity = Entity.FromHandle(receipt.Handle);
			if ((object)entity == null)
			{
				throw new InvalidOperationException("The tracked sale-sign handle could not be resolved.");
			}
			if (!entity.Exists())
			{
				return null;
			}
			Prop prop = entity as Prop;
			if ((object)prop == null)
			{
				return null;
			}
			if (receipt.Address == IntPtr.Zero)
			{
				throw new InvalidOperationException("The created sale sign has no verified entity address.");
			}
			if (!OwnedSaleSignIdentityMatches(receipt.Handle, receipt.ModelHash, receipt.Address, prop.Handle, prop.Model.Hash, prop.MemoryAddress))
			{
				return null;
			}
			return prop;
		}

		private static void RemoveSaleSign(Mansion m)
		{
			_Closure_0024__115_002D0 closure_0024__115_002D = new _Closure_0024__115_002D0();
			if (m == null)
			{
				return;
			}
			closure_0024__115_002D._0024VB_0024Local_receipt = null;
			if (ownedSaleSigns.TryGetValue(m, out closure_0024__115_002D._0024VB_0024Local_receipt))
			{
				try
				{
					if (!ConfirmOwnedSaleSignRemoval(closure_0024__115_002D._Lambda_0024__0, closure_0024__115_002D._Lambda_0024__1))
					{
						if (pendingSignRemoval.Add(m))
						{
							Logger.Log("Mansion sale sign removal pending: " + m.Name + "; retaining the owned entity for retry.");
						}
						return;
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					if (pendingSignRemoval.Add(m))
					{
						Logger.Log("Mansion sale sign removal will retry: " + m.Name + "; " + ex2.Message);
					}
					ProjectData.ClearProjectError();
					return;
				}
			}
			else if ((object)m.SaleSignProp != null && m.SaleSignProp.Exists())
			{
				if (pendingSignRemoval.Add(m))
				{
					Logger.Log("Mansion sale sign not removed: " + m.Name + "; no SPA creation receipt.");
				}
				return;
			}
			ownedSaleSigns.Remove(m);
			m.SaleSignProp = null;
			m.BoardPos = Vector3.Zero;
			nextSignAttempt.Remove(m);
			pendingSignRemoval.Remove(m);
		}

		public static void MansionsSlowTick()
		{
			if (Game.GameTime < checked(lastScan + 1000))
			{
				return;
			}
			lastScan = Game.GameTime;
			MansionTerrain.Tick();
			Vector3 position = Helper.PP.Position;
			foreach (Mansion mansion in MansionList)
			{
				if (!mansion.Enabled)
				{
					RemoveSaleSign(mansion);
					if (mansion.IplsLoaded || mansion.OurIpls.Count > 0 || mansion.IplRequestClock != null)
					{
						UnloadIpls(mansion);
					}
					continue;
				}
				float num = position.DistanceTo(mansion.Reference);
				bool flag = ((mansion.IplsLoaded || mansion.NearBy) ? (num < 350f) : (num < 250f));
				if (TenCarGarage.Apartment != null && TenCarGarage.Apartment.ID == mansion.GarageId)
				{
					flag = true;
				}
				if (MansionNativeGarage.ActiveMansion == mansion)
				{
					flag = true;
				}
				mansion.NearBy = flag;
				if (flag && !mansion.IplsLoaded && Helper.mpMapActive)
				{
					LoadIpls(mansion);
				}
				if ((mansion.IplsLoaded || mansion.OurIpls.Count > 0 || mansion.IplRequestClock != null) && ((!flag && !CharacterReturnResidency.HoldsMansion(mansion)) || !Helper.mpMapActive))
				{
					UnloadIpls(mansion);
				}
				if (flag && mansion.IplsLoaded)
				{
					MansionTerrain.PrepareReplacement(mansion);
				}
				if (flag && mansion.IplsLoaded && !mansion.SetsApplied)
				{
					ApplySets(mansion, true);
				}
				if (flag && mansion.IplsLoaded && mansion.SetsApplied)
				{
					MansionAutoLayout.AutoLayoutTick(mansion);
				}
				if (flag)
				{
					if (mansion.Owner == eOwner.Nobody)
					{
						SpawnSaleSign(mansion);
					}
					else
					{
						RemoveSaleSign(mansion);
					}
				}
				else
				{
					RemoveSaleSign(mansion);
					if (mansion.GatesSpawned)
					{
						DespawnGates(mansion);
					}
				}
				MansionFeatures.FeaturesSlowTick(mansion);
			}
		}

		private static void PrepareGateGeometry(Mansion m)
		{
			Vector3 vector = m.GateCenter();
			checked
			{
				int num = m.Gates.Count - 1;
				for (int i = 0; i <= num; i++)
				{
					GateDef gateDef = m.Gates[i];
					Vector3 vector2;
					if (m.Gates.Count >= 2)
					{
						vector2 = m.Gates[(i == 0) ? 1 : 0].Position - gateDef.Position;
					}
					else
					{
						double num2 = (double)gateDef.Heading * Math.PI / 180.0;
						vector2 = new Vector3((float)Math.Cos(num2), (float)Math.Sin(num2), 0f);
					}
					vector2.Z = 0f;
					if (vector2.Length() < 0.01f)
					{
						vector2 = new Vector3(1f, 0f, 0f);
					}
					vector2.Normalize();
					Vector3 vector3 = m.InteriorProbe - vector;
					vector3.Z = 0f;
					vector3 -= vector2 * Vector3.Dot(vector3, vector2);
					Vector3 left = new Vector3(0f - vector2.Y, vector2.X, 0f);
					gateDef.OpenSign = ((Vector3.Dot(left, vector3) >= 0f) ? 1 : (-1));
				}
			}
		}

		private static void SpawnGates(Mansion m)
		{
			if (m.GatesSpawned)
			{
				return;
			}
			if (GateMode.Equals("Hide", StringComparison.OrdinalIgnoreCase))
			{
				m.GatesSpawned = true;
				return;
			}
			try
			{
				PrepareGateGeometry(m);
				foreach (GateDef gate in m.Gates)
				{
					Function.Call(Hash.CREATE_MODEL_HIDE, gate.Position.X, gate.Position.Y, gate.Position.Z, 0.6f, gate.ModelHash, true);
					gate.Hidden = true;
					Model model = new Model(gate.ModelHash);
					model.Request(1000);
					if (!model.IsLoaded)
					{
						Logger.Log($"Mansion {m.Name}: gate model {gate.ModelHash} not loaded");
						continue;
					}
					int num = Function.Call<int>(Hash.CREATE_OBJECT_NO_OFFSET, gate.ModelHash, gate.Position.X, gate.Position.Y, gate.Position.Z, false, false, false);
					model.MarkAsNoLongerNeeded();
					if (num != 0)
					{
						gate.Replacement = Entity.FromHandle(num) as Prop;
						if ((object)gate.Replacement != null)
						{
							Function.Call(Hash.SET_ENTITY_ROTATION, num, 0f, 0f, gate.Heading, 2, true);
							Function.Call(Hash.FREEZE_ENTITY_POSITION, num, true);
							gate.Replacement.IsPersistent = true;
							gate.Angle = 0f;
						}
					}
				}
				m.GatesSpawned = true;
				Logger.Log(string.Format("> Mansion {0}: {1} gate wing(s) replaced (open dir {2})", m.Name, m.Gates.Count, string.Join("/", m.Gates.Select((_Closure_0024__._0024I123_002D0 != null) ? _Closure_0024__._0024I123_002D0 : (_Closure_0024__._0024I123_002D0 = _Closure_0024__._0024I._Lambda_0024__123_002D0)))));
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Mansion gates spawn: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
		}

		private static void DespawnGates(Mansion m)
		{
			foreach (GateDef gate in m.Gates)
			{
				try
				{
					if ((object)gate.Replacement != null && gate.Replacement.Exists())
					{
						gate.Replacement.Delete();
					}
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					ProjectData.ClearProjectError();
				}
				gate.Replacement = null;
				if (gate.Hidden)
				{
					Function.Call(Hash.REMOVE_MODEL_HIDE, gate.Position.X, gate.Position.Y, gate.Position.Z, 0.6f, gate.ModelHash, false);
					gate.Hidden = false;
				}
				gate.Angle = 0f;
			}
			m.GatesSpawned = false;
			m.GatesOpen = false;
		}

		public static void RestoreAllGates()
		{
			foreach (Mansion mansion in MansionList)
			{
				DespawnGates(mansion);
			}
		}

		private static void SetGates(Mansion m, bool open)
		{
			if (m.GatesOpen != open)
			{
				m.GatesOpen = open;
				Logger.Log(string.Format("> Mansion {0}: gate {1}", m.Name, open ? "open" : "close"));
			}
		}

		private static void GateTick(Mansion m)
		{
			if (m.Gates.Count == 0)
			{
				return;
			}
			if (!m.IplsLoaded)
			{
				if (m.GatesSpawned)
				{
					DespawnGates(m);
				}
				return;
			}
			if (!m.GatesSpawned)
			{
				SpawnGates(m);
			}
			float num = Mansion.Dist2D(Helper.PP.Position, m.GateCenter());
			bool open = m.GatesOpen;
			if (m.IsOwnedByPlayer() || Game.GameTime < m.GateTestUntil)
			{
				if (num < 20f)
				{
					open = true;
				}
				if (num > 28f)
				{
					open = false;
				}
				if (Game.GameTime < m.GateTestUntil)
				{
					open = true;
				}
			}
			else
			{
				open = false;
			}
			SetGates(m, open);
			if (!GateMode.Equals("Hide", StringComparison.OrdinalIgnoreCase))
			{
				float num2 = (m.GatesOpen ? 90f : 0f);
				float num3 = 40f * Game.LastFrameTime;
				{
					foreach (GateDef gate in m.Gates)
					{
						if ((object)gate.Replacement != null && gate.Replacement.Exists() && !(Math.Abs(gate.Angle - num2) < 0.01f))
						{
							if (gate.Angle < num2)
							{
								gate.Angle = Math.Min(num2, gate.Angle + num3);
							}
							else
							{
								gate.Angle = Math.Max(num2, gate.Angle - num3);
							}
							Function.Call(Hash.SET_ENTITY_ROTATION, gate.Replacement.Handle, 0f, 0f, gate.Heading + (float)gate.OpenSign * gate.Angle, 2, true);
						}
					}
					return;
				}
			}
			foreach (GateDef gate2 in m.Gates)
			{
				if (m.GatesOpen && !gate2.Hidden)
				{
					Function.Call(Hash.CREATE_MODEL_HIDE, gate2.Position.X, gate2.Position.Y, gate2.Position.Z, 0.6f, gate2.ModelHash, true);
					gate2.Hidden = true;
				}
				else if (!m.GatesOpen && gate2.Hidden)
				{
					Function.Call(Hash.REMOVE_MODEL_HIDE, gate2.Position.X, gate2.Position.Y, gate2.Position.Z, 0.6f, gate2.ModelHash, false);
					gate2.Hidden = false;
				}
			}
		}

		private static bool IsGateLike(Prop p)
		{
			try
			{
				ValueTuple<Vector3, Vector3> dimensions = p.Model.Dimensions;
				Vector3 vector = dimensions.Item2 - dimensions.Item1;
				float num = Math.Max(vector.X, vector.Y);
				float num2 = Math.Min(vector.X, vector.Y);
				return num >= 1.8f && num <= 14f && vector.Z >= 1.2f && num2 <= 1.5f;
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		private static int CaptureGates(Mansion m)
		{
			_Closure_0024__129_002D0 arg = new _Closure_0024__129_002D0(null);
			DespawnGates(m);
			Script.Wait(300);
			arg._0024VB_0024Local_ppos = Helper.PP.Position;
			arg._0024VB_0024Local_signHandle = (((object)m.SaleSignProp != null) ? m.SaleSignProp.Handle : 0);
			List<Prop> source = World.GetNearbyProps(arg._0024VB_0024Local_ppos, 10f).Where(arg._Lambda_0024__0).ToList();
			List<Prop> list = source.Where((_Closure_0024__._0024I129_002D1 != null) ? _Closure_0024__._0024I129_002D1 : (_Closure_0024__._0024I129_002D1 = _Closure_0024__._0024I._Lambda_0024__129_002D1)).OrderBy(arg._Lambda_0024__2).ToList();
			if (list.Count == 0)
			{
				list = source.Where(arg._Lambda_0024__3).OrderBy(arg._Lambda_0024__4).ToList();
			}
			if (list.Count == 0)
			{
				return 0;
			}
			arg._0024VB_0024Local_first = list[0];
			List<Prop> list2 = new List<Prop> { arg._0024VB_0024Local_first };
			Prop prop = list.Skip(1).FirstOrDefault(arg._Lambda_0024__5);
			if ((object)prop != null)
			{
				list2.Add(prop);
			}
			m.Gates.Clear();
			foreach (Prop item in list2)
			{
				m.Gates.Add(new GateDef
				{
					ModelHash = item.Model.Hash,
					Position = item.Position,
					Heading = item.Heading
				});
				Logger.Log($"Mansion {m.Name}: gate prop {item.Model.Hash} at {item.Position} heading {item.Heading:0.0}");
			}
			return list2.Count;
		}

		public static void LoadMansionMenus()
		{
			MansionBuyMenu = new UIMenu("", Game.GetLocalizedString("MP_PROP_GEN0"), new Point(0, -107));
			MansionBuyMenu.SetBannerType(Helper.MenuBanner);
			MansionBuyMenu.MouseEdgeEnabled = false;
			Helper.MenuPool.Add(MansionBuyMenu);
			MansionMenu = new UIMenu("", "MANSION", new Point(0, -107));
			MansionMenu.SetBannerType(Helper.MenuBanner);
			MansionMenu.MouseEdgeEnabled = false;
			Helper.MenuPool.Add(MansionMenu);
			MansionGarageMenu = new UIMenu("", "GARAGE", new Point(0, -107));
			MansionGarageMenu.SetBannerType(Helper.MenuBanner);
			MansionGarageMenu.MouseEdgeEnabled = false;
			Helper.MenuPool.Add(MansionGarageMenu);
		}

		private static void BuildBuyMenu(Mansion m)
		{
			MansionBuyMenu.MenuItems.Clear();
			UIMenuItem uIMenuItem = new UIMenuItem(m.Name, Helper.Truncate(m.Description));
			uIMenuItem.SetRightLabel(string.Format("${0}", m.Price.ToString("0,0")));
			uIMenuItem.Tag = m;
			MansionBuyMenu.AddItem(uIMenuItem);
			MansionBuyMenu.RefreshIndex();
		}

		private static int IndexOfKey(string[] keys, string key)
		{
			int num = Array.IndexOf(keys, key);
			if (num >= 0)
			{
				return num;
			}
			return 0;
		}

		internal static void BuildMansionMenu(Mansion m)
		{
			currentMansion = m;
			MansionMenu.MenuItems.Clear();
			MansionMenu.Subtitle.Caption = m.Name.ToUpper();
			MansionMenu.ParentMenu = null;
			styleItem = new UIMenuListItem(Game.GetLocalizedString("PM_APT_TVARIANT"), new List<object>(StyleNames), IndexOfKey(StyleKeys, m.Style));
			wallpaperItem = new UIMenuListItem("Wallpaper", new List<object>(new object[6] { "Rustic", "Deco", "Coastal", "Subtle", "Safari", "Pop Art" }), IndexOfKey(WallpaperKeys, m.Wallpaper));
			tintItem = new UIMenuListItem("Accent Colour", new List<object>(TintNames), Math.Max(0, Math.Min(3, m.Tint)));
			assistantItem = new UIMenuListItem("Assistant", new List<object>(AssistantNames), IndexOfKey(AssistantKeys, m.Assistant));
			decoItem = new UIMenuListItem("Decorations", new List<object>(DecoNames), IndexOfKey(DecoKeys, m.Decoration));
			artCollectionItem = new UIMenuListItem("Art Collection", new List<object>(ArtCollectionNames), IndexOfKey(ArtCollectionKeys, m.ArtCollection));
			windowShuttersItem = new UIMenuCheckboxItem("Close Window Shutters", m.WindowShutters);
			catItem = new UIMenuCheckboxItem("Pet Cat", m.PetCat);
			dogItem = new UIMenuCheckboxItem("Dog Bowl", m.PetDog);
			posterItem = new UIMenuCheckboxItem("Michael Poster", m.MichaelPoster);
			UIMenu mansionMenu = MansionMenu;
			int count = MansionGarageVehicles(m).Count;
			if (m.IsOwnedByPlayer())
			{
				mansionMenu.AddItem(new UIMenuItem("Mansion Assistant (services)", "Fast travel, cars, chauffeur, helicopter, champagne, party, gym, armory, vault ...")
				{
					Tag = "Assistant"
				});
				mansionMenu.AddItem(new UIMenuItem("Enter original mansion garage", "Enter the original 20-vehicle mansion garage. Existing vehicles are kept.")
				{
					Tag = "NativeGarage"
				});
				mansionMenu.AddItem(new UIMenuItem("Buy vehicles / 3D preview", "Preview installed GTA Online cars and HSW-capable models, then buy into this mansion garage.")
				{
					Tag = "BuyVehicles"
				});
				mansionMenu.AddItem(new UIMenuItem("Vehicle workshop", "Travel to the mansion tuning garage and choose a vehicle.")
				{
					Tag = "Workshop"
				});
			}
			if (Helper.PP.IsInVehicle())
			{
				mansionMenu.AddItem(new UIMenuItem("Park this vehicle here", $"Stores the vehicle in the mansion garage ({count}/{20}).")
				{
					Tag = "Park"
				});
			}
			UIMenuItem uIMenuItem = new UIMenuItem("Stored vehicles / driveway delivery", "Take out one of your stored vehicles at the driveway.");
			uIMenuItem.SetRightLabel($"{count}/{20}");
			mansionMenu.AddItem(uIMenuItem);
			BuildGarageMenu(m);
			mansionMenu.BindMenuToItem(MansionGarageMenu, uIMenuItem);
			mansionMenu.AddItem(styleItem);
			mansionMenu.AddItem(wallpaperItem);
			mansionMenu.AddItem(tintItem);
			mansionMenu.AddItem(assistantItem);
			mansionMenu.AddItem(decoItem);
			mansionMenu.AddItem(artCollectionItem);
			if (m.IsOwnedByPlayer())
			{
				mansionMenu.AddItem(new UIMenuItem("Trophy Cabinets", "Choose a trophy for each of the five glass cabinets.")
				{
					Tag = "TrophyCabinets"
				});
			}
			mansionMenu.AddItem(windowShuttersItem);
			mansionMenu.AddItem(catItem);
			mansionMenu.AddItem(dogItem);
			mansionMenu.AddItem(posterItem);
			if (Helper.ToVector3(m.WardrobePos) != Vector3.Zero)
			{
				mansionMenu.AddItem(new UIMenuItem(Game.GetLocalizedString("WARD_TRIG").Replace("~a~", "").Trim(), "Open the wardrobe.")
				{
					Tag = "Wardrobe"
				});
			}
			UIMenuItem uIMenuItem2 = new UIMenuItem("Podium (show car)", "Choose the car that is displayed on the podium.")
			{
				Tag = "Podium"
			};
			if (m.PodiumUid != 0)
			{
				uIMenuItem2.SetRightLabel(MansionFeatures.PodiumDisplayStatus(m));
			}
			string[][] array = new string[3][]
			{
				new string[2] { "Armory", "Armoury shop / weapon upgrades" },
				new string[2] { "Office", "Office computer" },
				new string[2] { "Kitchen", "Kitchen / snacks / drinks" }
			};
			foreach (string[] array2 in array)
			{
				mansionMenu.AddItem(new UIMenuItem(array2[1])
				{
					Tag = array2[0]
				});
			}
			mansionMenu.AddItem(uIMenuItem2);
			orientationItem = new UIMenuCheckboxItem("Indoor map markers", MansionOrientation.Enabled, "Show nearby rooms and services on the minimap inside your mansions.");
			mansionMenu.AddItem(orientationItem);
			mansionMenu.RefreshIndex();
			mansionMenu = null;
		}

		public static void MansionsTick()
		{
			MansionsSlowTick();
			MansionOrientation.Tick();
			MansionTrophyCases.Tick();
			MansionFeatures.BuffTick();
			MansionFeatures.ChauffeurTick();
			if (MansionFeatures.AssistantRequestPending() || MansionNativeGarage.Tick())
			{
				return;
			}
			foreach (Mansion mansion in MansionList)
			{
				if (!mansion.Enabled || !mansion.NearBy)
				{
					continue;
				}
				GateTick(mansion);
				if (ApartmentHomeServices.MansionBedTick(mansion) || MansionFeatures.FeaturesTick(mansion) || Helper.MenuPool.IsAnyMenuOpen())
				{
					continue;
				}
				if (mansion.Owner != eOwner.Nobody && !mansion.IsOwnedByPlayer())
				{
					if ((mansion.Gates.Count > 0 && Helper.PP.Position.DistanceToSquared(mansion.GateCenter()) <= 36f) || mansion.SignDistance2D() <= BuyRadius || IsAtGarageDriveway(mansion, Helper.PP.Position))
					{
						Helper.ShowPersistentHelp(PropertyOwnershipText.OwnerLabel(mansion.Owner, Game.Language == Language.German));
					}
				}
				else if (mansion.IsOwnedByPlayer() && IsAtGarageDriveway(mansion, Helper.PP.Position) && (!Helper.PP.IsInVehicle() || (Helper.PP.CurrentVehicle.Driver == Helper.PP && MansionFeatures.HeadingDiff(Helper.PP.CurrentVehicle.Heading, mansion.GaragePos.W) < 90f)))
				{
					if (Helper.PP.IsInVehicle())
					{
						if (!Helper.ShowPersistentHelp("Press ~INPUT_CONTEXT~ to drive into the garage."))
						{
							continue;
						}
					}
					else if (!Helper.ShowPersistentHelp("Press ~INPUT_CONTEXT~ to enter the garage."))
					{
						continue;
					}
					if (Game.IsControlJustReleased(GTA.Control.Context))
					{
						currentMansion = mansion;
						EnterMansionGarage(mansion);
					}
				}
				else
				{
					if (mansion.SignDistance2D() > BuyRadius)
					{
						continue;
					}
					if (mansion.Owner == eOwner.Nobody)
					{
						if (Helper.ShowPersistentHelp(Game.GetLocalizedString("MP_PROP_PUR0")) && Game.IsControlJustReleased(GTA.Control.Context))
						{
							currentMansion = mansion;
							BuildBuyMenu(mansion);
							MansionBuyMenu.Visible = true;
						}
					}
					else
					{
						if (!mansion.IsOwnedByPlayer())
						{
							continue;
						}
						if (Helper.PP.IsInVehicle())
						{
							if (!Helper.ShowPersistentHelp("Press ~INPUT_CONTEXT~ to open the mansion menu (park this vehicle in the mansion garage)."))
							{
								continue;
							}
						}
						else if (!Helper.ShowPersistentHelp("Press ~INPUT_CONTEXT~ to open the mansion menu (garage and style)."))
						{
							continue;
						}
						if (Game.IsControlJustReleased(GTA.Control.Context))
						{
							currentMansion = mansion;
							BuildMansionMenu(mansion);
							MansionMenu.Visible = true;
						}
					}
				}
			}
		}

		public static ApartmentClass MansionApartment(Mansion m)
		{
			ApartmentClass value = null;
			if (!mansionApts.TryGetValue(m.Key, out value))
			{
				value = new ApartmentClass
				{
					ID = m.GarageId,
					Name = m.Name,
					Description = m.Description,
					Price = m.Price,
					GarageFilePath = m.GarageFolder(),
					ApartmentType = eApartmentType.Other
				};
				value.BuildingOverride = new BuildingClass
				{
					Name = m.Name,
					GarageType = eGarageType.TenCarGarage,
					GarageDoor = eFrontDoor.NoDoor,
					Apartments = new List<ApartmentClass> { value }
				};
				mansionApts[m.Key] = value;
			}
			value.GarageFilePath = m.GarageFolder();
			value.MansionGarageOwner = m.Owner;
			Quaternion quaternion = (MansionFeatures.IsSet(m.GaragePos) ? new Quaternion(m.GaragePos.X, m.GaragePos.Y, m.GaragePos.Z, m.GaragePos.W + 180f) : m.SignPos);
			BuildingClass buildingOverride = value.BuildingOverride;
			buildingOverride.GarageOutPos = quaternion;
			buildingOverride.GarageWaypoint = quaternion;
			buildingOverride.GarageInPos = Helper.ToVector3(quaternion);
			buildingOverride.GarageFootOutPos = quaternion;
			return value;
		}

		internal static void EnterMansionGarage(Mansion m, bool viaElevator = false)
		{
			MansionNativeGarage.Enter(m, viaElevator);
		}

		public static List<VehicleClass> MansionGarageVehicles(Mansion m)
		{
			List<VehicleClass> list = new List<VehicleClass>();
			try
			{
				string path = $"{Helper.grgXmlPath}{m.GarageFolder()}";
				if (!Directory.Exists(path))
				{
					return list;
				}
				string[] files = Directory.GetFiles(path, "*.xml");
				foreach (string filename in files)
				{
					VehicleData value = default(VehicleData);
					if (new VehicleData(filename).TryRead(ref value) && value.Hash != 0 && value.Index >= 0 && value.Index < 20 && value.Owner == m.Owner && value.ApartmentID == m.GarageId)
					{
						list.Add(new VehicleClass(value, m.Owner));
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Mansion garage list: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
			return list.OrderBy((_Closure_0024__._0024I142_002D0 != null) ? _Closure_0024__._0024I142_002D0 : (_Closure_0024__._0024I142_002D0 = _Closure_0024__._0024I._Lambda_0024__142_002D0)).ToList();
		}

		internal static bool IsCharacterPropertyGarage(int id)
		{
			_Closure_0024__143_002D0 closure_0024__143_002D = new _Closure_0024__143_002D0();
			closure_0024__143_002D._0024VB_0024Local_id = id;
			if (!MansionList.Any(closure_0024__143_002D._Lambda_0024__0))
			{
				return Helper.apartments.Any(closure_0024__143_002D._Lambda_0024__1);
			}
			return true;
		}

		public static string GarageFolderForId(int id)
		{
			_Closure_0024__144_002D0 closure_0024__144_002D = new _Closure_0024__144_002D0();
			closure_0024__144_002D._0024VB_0024Local_id = id;
			ApartmentClass apartmentClass = Helper.apartments.Find(closure_0024__144_002D._Lambda_0024__0);
			if (apartmentClass != null)
			{
				return apartmentClass.StorageFolder(PropertyProfiles.ActiveCharacter);
			}
			Mansion mansion = MansionList.Find(closure_0024__144_002D._Lambda_0024__1);
			if (mansion != null)
			{
				return PropertyProfiles.GarageFolder("Mansion_" + mansion.Key, PropertyProfiles.ActiveCharacter, mansion.LegacyOwner);
			}
			return SouthLSHousing.ArchiveGarageFolder(closure_0024__144_002D._0024VB_0024Local_id);
		}

		private static void ParkVehicle(Mansion m)
		{
			Vehicle currentVehicle = Helper.PP.CurrentVehicle;
			int committedUid = 0;
			if (MansionNativeGarage.StoreVehicle(m, currentVehicle, ref committedUid))
			{
				MansionMenu.Visible = false;
				Notification.Show($"Vehicle stored in the {m.Name} garage ({MansionGarageVehicles(m).Count}/{20}).");
			}
		}

		public static Vehicle SpawnOutVehicle(Mansion m, VehicleClass vc, Quaternion q)
		{
			Vehicle vehicle = Helper.CreateGarageVehicle(vc, q, m.GarageId);
			if ((object)vehicle == null || !vehicle.Exists())
			{
				return null;
			}
			MansionNativeGarage.ReleaseStoredVehicle(m, vc.UniqueID);
			if (!Helper.RegisterOutsideVehicle(vehicle, vc.Owner))
			{
				if (vehicle.Exists())
				{
					vehicle.Delete();
				}
				return null;
			}
			vehicle.PlaceOnGround();
			return vehicle;
		}

		private static void BuildGarageMenu(Mansion m)
		{
			MansionGarageMenu.MenuItems.Clear();
			MansionGarageMenu.Subtitle.Caption = $"{m.Name.ToUpper()} GARAGE";
			List<VehicleClass> list = MansionGarageVehicles(m);
			if (list.Count == 0)
			{
				MansionGarageMenu.AddItem(new UIMenuItem("(empty)", "Drive a vehicle to the mansion buy point and choose 'Park this vehicle here'."));
			}
			foreach (VehicleClass item in list)
			{
				bool flag = Helper.IsGarageVehicleAlreadyExistInWorldMap(m.GarageId, item.UniqueID);
				bool flag2 = MansionFeatures.IsVehicleBusy(m, item.UniqueID);
				UIMenuItem uIMenuItem = new UIMenuItem($"{item.Make} {item.Name}", flag ? "This vehicle is already out." : (flag2 ? "On the podium / workshop lift." : "Take this vehicle out."));
				uIMenuItem.SetRightLabel(flag ? "OUT" : (flag2 ? ((m.PodiumUid == item.UniqueID) ? "PODIUM" : "LIFT") : item.PlateNumber));
				uIMenuItem.Tag = item;
				MansionGarageMenu.AddItem(uIMenuItem);
			}
			MansionGarageMenu.RefreshIndex();
		}

		private static void MansionGarageMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			try
			{
				Mansion mansion = currentMansion;
				VehicleClass vehicleClass = selectedItem.Tag as VehicleClass;
				if (mansion == null || vehicleClass == null)
				{
					return;
				}
				if (Helper.IsGarageVehicleAlreadyExistInWorldMap(mansion.GarageId, vehicleClass.UniqueID))
				{
					Notification.Show("This vehicle is already out.");
					return;
				}
				if (MansionFeatures.IsVehicleBusy(mansion, vehicleClass.UniqueID))
				{
					Notification.Show("This vehicle is on the podium / workshop lift - choose another show car or send it back first.");
					return;
				}
				if (Helper.PP.IsInVehicle())
				{
					Notification.Show("Get out of your current vehicle first (or park it).");
					return;
				}
				MansionGarageMenu.Visible = false;
				Helper.FadeScreen(1);
				Quaternion q;
				if (Helper.ToVector3(mansion.GaragePos) != Vector3.Zero)
				{
					q = new Quaternion(mansion.GaragePos.X, mansion.GaragePos.Y, mansion.GaragePos.Z, mansion.GaragePos.W + 180f);
				}
				else
				{
					Vector3 vector = Helper.PP.Position + Helper.PP.ForwardVector * 3f;
					q = new Quaternion(vector.X, vector.Y, vector.Z, Helper.PP.Heading);
				}
				Vehicle vehicle = SpawnOutVehicle(mansion, vehicleClass, q);
				if ((object)vehicle == null)
				{
					Helper.FadeScreen(0);
					Notification.Show("Vehicle model could not be loaded (addon car missing?).");
					return;
				}
				Helper.SetPlayerIntoVehicle(vehicle);
				vehicle.IsEngineRunning = true;
				Helper.FadeScreen(0);
				Logger.Log($"> Mansion {mansion.Name}: took out {vehicleClass.Make} {vehicleClass.Name} (uid {vehicleClass.UniqueID})");
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Helper.FadeScreen(0);
				Logger.Log($"Mansion garage take out: {ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		private static void ShowMansionPurchaseResult(string message)
		{
			try
			{
				Notification.Show(message);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Mansion purchase notification: " + ex2.Message);
				try
				{
					GTA.UI.Screen.ShowSubtitle(message, 6000);
				}
				catch (Exception ex3)
				{
					ProjectData.SetProjectError(ex3);
					Exception ex4 = ex3;
					Logger.Log("Mansion purchase subtitle: " + ex4.Message);
					ProjectData.ClearProjectError();
				}
				ProjectData.ClearProjectError();
			}
		}

		public static bool BuyMansion(Mansion m, bool fade)
		{
			if (m == null)
			{
				ShowMansionPurchaseResult("The mansion selection is no longer available. Open the purchase menu again.");
				return false;
			}
			if (m.Owner != eOwner.Nobody)
			{
				ShowMansionPurchaseResult(m.Name + ": this mansion has already been purchased.");
				return false;
			}
			eOwner player = Helper.GetPlayer();
			if (player != PropertyProfiles.ActiveCharacter || !PropertyProfiles.IsCharacter(player) || ReadMansionProfileOwner(ScriptSettings.Load(iniPath), m.Key, player) != eOwner.Nobody)
			{
				return false;
			}
			int num = Math.Max(0, m.Price);
			int balance = default(int);
			if (!Helper.TryGetOwnerMoney(player, ref balance))
			{
				ShowMansionPurchaseResult("Mansion purchases require a story character's bank account.");
				return false;
			}
			if (balance < num)
			{
				ShowMansionPurchaseResult($"Not enough money for {m.Name}. Price: ${num:0,0}; available: ${balance:0,0}.");
				return false;
			}
			eOwner owner = m.Owner;
			bool flag = false;
			try
			{
				m.Owner = player;
				using (AtomicFileTransaction atomicFileTransaction = BeginMansionSaveTransaction())
				{
					if (!Helper.TryChangeOwnerMoney(player, checked(-num)))
					{
						m.Owner = owner;
						ShowMansionPurchaseResult("The mansion purchase could not be paid. No ownership change was kept.");
						return false;
					}
					flag = true;
					atomicFileTransaction.Complete();
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				m.Owner = owner;
				if (flag && !Helper.TryChangeOwnerMoney(player, num))
				{
					Logger.Log("Mansion purchase refund failed for " + player);
				}
				Logger.Log("Mansion purchase was not committed: " + ex2.Message);
				ShowMansionPurchaseResult("The mansion purchase could not be saved. Your previous ownership has been kept.");
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
			RefreshMansionSettingsCache();
			ShowMansionPurchaseResult("~y~Property purchased~w~: " + m.Name + ".");
			try
			{
				RemoveSaleSign(m);
				RefreshBlip(m);
				if (fade)
				{
					Helper.FadeScreen(1);
				}
				if (fade)
				{
					Helper.FadeScreen(0);
				}
				Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "PROPERTY_PURCHASE", "HUD_AWARDS", false);
				BigMessageThread.MessageInstance.ShowWeaponPurchasedMessage("~y~" + Game.GetLocalizedString("PROPR_PURCHASED"), "~w~" + m.Name, (WeaponHash)0u);
			}
			catch (Exception ex3)
			{
				ProjectData.SetProjectError(ex3);
				Exception ex4 = ex3;
				Logger.Log("Mansion purchase presentation: " + ex4.Message);
				if (fade)
				{
					try
					{
						Helper.FadeScreen(0);
					}
					catch (Exception ex5)
					{
						ProjectData.SetProjectError(ex5);
						Exception ex6 = ex5;
						Logger.Log("Mansion purchase fade recovery: " + ex6.Message);
						ProjectData.ClearProjectError();
					}
				}
				ProjectData.ClearProjectError();
			}
			Logger.Log($"> Mansion {m.Name}: bought by {player}");
			return true;
		}

		public static bool SellMansion(Mansion m, int payout)
		{
			if (m == null || !m.IsOwnedByPlayer())
			{
				return false;
			}
			eOwner owner = m.Owner;
			if (owner != PropertyProfiles.ActiveCharacter || ReadMansionProfileOwner(ScriptSettings.Load(iniPath), m.Key, owner) != owner)
			{
				return false;
			}
			int num = Math.Max(0, payout);
			checked
			{
				int balance = default(int);
				if (!Helper.TryGetOwnerMoney(owner, ref balance) || unchecked((long)balance) + unchecked((long)num) > int.MaxValue)
				{
					Notification.Show("The mansion sale cannot be credited to this character's account.");
					return false;
				}
				bool flag = false;
				try
				{
					m.Owner = eOwner.Nobody;
					using (AtomicFileTransaction atomicFileTransaction = BeginMansionSaveTransaction())
					{
						if (!Helper.TryChangeOwnerMoney(owner, num))
						{
							m.Owner = owner;
							Notification.Show("The sale could not be paid. Your mansion has been kept.");
							return false;
						}
						flag = true;
						atomicFileTransaction.Complete();
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					m.Owner = owner;
					if (flag && !Helper.TryChangeOwnerMoney(owner, -num))
					{
						Logger.Log("Mansion sale credit rollback failed for " + owner);
					}
					Logger.Log("Mansion sale was not committed: " + ex2.Message);
					Notification.Show("The mansion sale could not be saved. Your mansion has been kept.");
					bool result = false;
					ProjectData.ClearProjectError();
					return result;
				}
				RefreshMansionSettingsCache();
				try
				{
					RefreshBlip(m);
					if (m.NearBy)
					{
						SpawnSaleSign(m);
					}
				}
				catch (Exception ex3)
				{
					ProjectData.SetProjectError(ex3);
					Exception ex4 = ex3;
					Logger.Log("Mansion sale presentation: " + ex4.Message);
					ProjectData.ClearProjectError();
				}
				Logger.Log($"> Mansion {m.Name}: sold for {num}");
				return true;
			}
		}

		public static void SetMansionWaypoint(Mansion m)
		{
			Function.Call(Hash.SET_NEW_WAYPOINT, m.SignPos.X, m.SignPos.Y);
		}

		private static void MansionBuyMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			try
			{
				Mansion mansion = selectedItem.Tag as Mansion;
				Logger.Log(string.Format("MansionBuyMenu select: {0} owner={1} money={2}", (mansion == null) ? "?" : mansion.Name, (mansion == null) ? "?" : mansion.Owner.ToString(), Helper.PM));
				if (BuyMansion(mansion, true))
				{
					MansionBuyMenu.Visible = false;
				}
				else
				{
					MansionBuyMenu.Visible = true;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				MansionBuyMenu.Visible = true;
				ShowMansionPurchaseResult("The mansion purchase could not be completed. Please try again.");
				Logger.Log($"Mansion buy: {ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		private static void MansionMenu_OnListChange(UIMenu sender, UIMenuListItem listItem, int newIndex)
		{
			try
			{
				Mansion mansion = currentMansion;
				if (mansion == null)
				{
					return;
				}
				if (listItem == artCollectionItem && (!mansion.IsOwnedByPlayer() || newIndex < 0 || newIndex >= ArtCollectionKeys.Length))
				{
					listItem.Index = IndexOfKey(ArtCollectionKeys, mansion.ArtCollection);
					return;
				}
				string style = mansion.Style;
				string wallpaper = mansion.Wallpaper;
				int tint = mansion.Tint;
				string assistant = mansion.Assistant;
				string decoration = mansion.Decoration;
				string artCollection = mansion.ArtCollection;
				if (listItem == styleItem && newIndex >= 0 && newIndex < StyleKeys.Length)
				{
					mansion.Style = StyleKeys[newIndex];
				}
				if (listItem == wallpaperItem && newIndex >= 0 && newIndex < WallpaperKeys.Length)
				{
					mansion.Wallpaper = WallpaperKeys[newIndex];
				}
				if (listItem == tintItem && newIndex >= 0 && newIndex <= 3)
				{
					mansion.Tint = newIndex;
				}
				if (listItem == assistantItem && newIndex >= 0 && newIndex < AssistantKeys.Length)
				{
					mansion.Assistant = AssistantKeys[newIndex];
				}
				if (listItem == decoItem && newIndex >= 0 && newIndex < DecoKeys.Length)
				{
					mansion.Decoration = DecoKeys[newIndex];
				}
				if (listItem == artCollectionItem)
				{
					mansion.ArtCollection = ArtCollectionKeys[newIndex];
				}
				if (!SaveMansions())
				{
					mansion.Style = style;
					mansion.Wallpaper = wallpaper;
					mansion.Tint = tint;
					mansion.Assistant = assistant;
					mansion.Decoration = decoration;
					mansion.ArtCollection = artCollection;
					if (listItem == styleItem)
					{
						listItem.Index = IndexOfKey(StyleKeys, mansion.Style);
					}
					if (listItem == wallpaperItem)
					{
						listItem.Index = IndexOfKey(WallpaperKeys, mansion.Wallpaper);
					}
					if (listItem == tintItem)
					{
						listItem.Index = mansion.Tint;
					}
					if (listItem == assistantItem)
					{
						listItem.Index = IndexOfKey(AssistantKeys, mansion.Assistant);
					}
					if (listItem == decoItem)
					{
						listItem.Index = IndexOfKey(DecoKeys, mansion.Decoration);
					}
					if (listItem == artCollectionItem)
					{
						listItem.Index = IndexOfKey(ArtCollectionKeys, mansion.ArtCollection);
					}
				}
				else
				{
					ApplySets(mansion, true);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Mansion list: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
		}

		private static void MansionMenu_OnCheckboxChange(UIMenu sender, UIMenuCheckboxItem checkboxItem, bool @checked)
		{
			try
			{
				Mansion mansion = currentMansion;
				if (mansion == null)
				{
					return;
				}
				if (checkboxItem == orientationItem)
				{
					bool enabled = MansionOrientation.Enabled;
					if (!sender.Visible || !mansion.Enabled || !mansion.IsOwnedByPlayer() || StoryLoadGuard.WorldLoading())
					{
						checkboxItem.Checked = enabled;
						return;
					}
					MansionOrientation.Enabled = @checked;
					if (!SaveMansions())
					{
						MansionOrientation.Enabled = enabled;
						checkboxItem.Checked = enabled;
					}
					if (!MansionOrientation.Enabled)
					{
						MansionOrientation.Cleanup();
					}
					return;
				}
				if (checkboxItem == windowShuttersItem)
				{
					bool windowShutters = mansion.WindowShutters;
					if (!mansion.IsOwnedByPlayer())
					{
						checkboxItem.Checked = windowShutters;
						return;
					}
					mansion.WindowShutters = @checked;
					if (!SaveMansions())
					{
						mansion.WindowShutters = windowShutters;
						checkboxItem.Checked = windowShutters;
					}
					else
					{
						ApplyWindowShutters(mansion);
					}
					return;
				}
				bool petCat = mansion.PetCat;
				bool petDog = mansion.PetDog;
				bool michaelPoster = mansion.MichaelPoster;
				if (checkboxItem == catItem)
				{
					mansion.PetCat = @checked;
				}
				if (checkboxItem == dogItem)
				{
					mansion.PetDog = @checked;
				}
				if (checkboxItem == posterItem)
				{
					mansion.MichaelPoster = @checked;
				}
				if (!SaveMansions())
				{
					mansion.PetCat = petCat;
					mansion.PetDog = petDog;
					mansion.MichaelPoster = michaelPoster;
					if (checkboxItem == catItem)
					{
						checkboxItem.Checked = petCat;
					}
					if (checkboxItem == dogItem)
					{
						checkboxItem.Checked = petDog;
					}
					if (checkboxItem == posterItem)
					{
						checkboxItem.Checked = michaelPoster;
					}
				}
				else
				{
					ApplySets(mansion, true);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Mansion checkbox: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
		}

		private static void MansionMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			try
			{
				Mansion mansion = currentMansion;
				if (mansion != null && (!(selectedItem.Tag is string) || !MansionAmenities.OpenService(Conversions.ToString(selectedItem.Tag), mansion, sender)))
				{
					if (selectedItem.Tag is string && Operators.CompareString(Conversions.ToString(selectedItem.Tag), "Park", false) == 0)
					{
						ParkVehicle(mansion);
					}
					else if (selectedItem.Tag is string && Operators.CompareString(Conversions.ToString(selectedItem.Tag), "Assistant", false) == 0)
					{
						MansionMenu.Visible = false;
						MansionFeatures.OpenAssistant(mansion);
					}
					else if (selectedItem.Tag is string && Operators.CompareString(Conversions.ToString(selectedItem.Tag), "NativeGarage", false) == 0)
					{
						MansionMenu.Visible = false;
						EnterMansionGarage(mansion);
					}
					else if (selectedItem.Tag is string && Operators.CompareString(Conversions.ToString(selectedItem.Tag), "BuyVehicles", false) == 0)
					{
						MansionVehicleShop.Open(mansion, MansionMenu);
					}
					else if (selectedItem.Tag is string && Operators.CompareString(Conversions.ToString(selectedItem.Tag), "Workshop", false) == 0)
					{
						MansionWorkshopTravel.OpenWorkshopHere(mansion, MansionMenu);
					}
					else if (selectedItem.Tag is string && Operators.CompareString(Conversions.ToString(selectedItem.Tag), "Podium", false) == 0)
					{
						MansionFeatures.OpenPodiumMenu(mansion, MansionMenu);
					}
					else if (selectedItem.Tag is string && Operators.CompareString(Conversions.ToString(selectedItem.Tag), "TrophyCabinets", false) == 0)
					{
						MansionTrophyCases.OpenMenu(mansion, sender);
					}
					else if (selectedItem.Tag is string && Operators.CompareString(Conversions.ToString(selectedItem.Tag), "Wardrobe", false) == 0)
					{
						MansionWardrobe.Open(mansion, MansionMenu);
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Mansion select: {ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}
	}
}
