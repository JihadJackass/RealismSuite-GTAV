using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AddonWeapons2.Weapons;
using GTA;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Menus;

namespace AddonWeapons2.UI
{
	public static class WeaponMenu
	{
		private static readonly IWeaponManager _weaponManager = new WeaponManager();

		private static readonly Dictionary<WeaponCategory, NativeMenu> _categoryMenus = new Dictionary<WeaponCategory, NativeMenu>();

		private static readonly List<NativeMenu> _customMenus = new List<NativeMenu>();

		private static int _menuOpenedFlag = 0;

		private static ScriptSettings _configSettings;

		public static ObjectPool Pool { get; private set; }

		public static NativeMenu MainMenu { get; private set; }

		public static NativeMenu ComponentMenu { get; private set; }

		public static NativeMenu CurrentOpenedMenu { get; private set; }

		public static List<NativeMenu> CustomMenusList { get; } = new List<NativeMenu>();


		public static string TitleMain { get; private set; }

		public static string TitleHeavy { get; private set; }

		public static string TitleMelee { get; private set; }

		public static string TitleMG { get; private set; }

		public static string TitlePistols { get; private set; }

		public static string TitleRifles { get; private set; }

		public static string TitleShotguns { get; private set; }

		public static string TitleSMG { get; private set; }

		public static string TitleSR { get; private set; }

		public static string TitleSG { get; private set; }

		public static string TitleThrown { get; private set; }

		public static string RoundsText { get; private set; }

		public static string MaxRoundsText { get; private set; }

		public static string HelpMessage { get; private set; }

		public static string NoMoneyText { get; private set; }

		public static event Action<WeaponCategory> OnCategoryMenuClosedByPlayer;

		public static void Initialize()
		{
			SetLanguage();
			InitializeMenus();
		}

		public static void SetLanguage()
		{
			_configSettings = ScriptSettings.Load("Scripts\\AddonWeapons\\settings.ini");
			TitleMain = GetLocalizedString("GS_TITLE_0", "WEAPONS");
			TitleHeavy = GetLocalizedString("VAULT_WMENUI_6", "Heavy Weapons");
			TitleMelee = GetLocalizedString("VAULT_WMENUI_8", "Melee Weapons");
			TitleMG = GetLocalizedString("VAULT_WMENUI_3", "Machine Guns");
			TitlePistols = GetLocalizedString("VAULT_WMENUI_9", "Pistols");
			TitleRifles = GetLocalizedString("VAULT_WMENUI_4", "Rifles");
			TitleShotguns = GetLocalizedString("VAULT_WMENUI_2", "Shotguns");
			TitleSMG = GetLocalizedString("HUD_MG_SMG", "Submachine Guns");
			TitleSR = GetLocalizedString("VAULT_WMENUI_5", "Sniper Rifles");
			TitleSG = GetLocalizedString("VRT_B_SGUN1", "Stun Guns");
			TitleThrown = GetLocalizedString("VAULT_WMENUI_7", "Explosives");
			RoundsText = GetLocalizedString("GSA_TYPE_R", "Rounds");
			MaxRoundsText = GetLocalizedString("GS_FULL", "FULL");
			HelpMessage = "GS_BROWSE_W";
			NoMoneyText = GetLocalizedString("MPCT_SMON_04", "~z~You'll need more cash to afford that.");
		}

		private static string GetLocalizedString(string key, string defaultValue = "")
		{
			string localizedString = Game.GetLocalizedString(key);
			if (!string.IsNullOrEmpty(localizedString) && localizedString.Length >= 3)
			{
				return localizedString;
			}
			return defaultValue;
		}

		private static void InitializeMenus()
		{
			Pool = new ObjectPool();
			MainMenu = CreateMenu("", TitleMain, true, false, false);
			ComponentMenu = CreateMenu("", "", false, false, true);
			InitializeCategoryMenus();
			LoadCustomMenus();
			AddMenusToPool();
		}

		private static NativeMenu CreateMenu(string subtitle, string title, bool IsMainMenu, bool IsWeaponMenu, bool IsComponentMenu)
		{
			NativeMenu nativeMenu = new NativeMenu(subtitle, title, " ", new ScaledTexture(PointF.Empty, new SizeF(0f, 108f), "shopui_title_gunclub", "shopui_title_gunclub"));
			nativeMenu.Closed += delegate
			{
				Items.DeleteWeaponObject();
			};
			if (IsComponentMenu)
			{
				nativeMenu.Opening += delegate
				{
					if (WeaponInfo.CurrentWeaponHash != 0)
					{
						Items.PreviewWeapon(WeaponInfo.CurrentWeaponHash);
					}
				};
			}
			return nativeMenu;
		}

		private static void InitializeCategoryMenus()
		{
			_categoryMenus.Add(WeaponCategory.Heavy, CreateMenu("", TitleHeavy, false, true, false));
			_categoryMenus.Add(WeaponCategory.Melee, CreateMenu("", TitleMelee, false, true, false));
			_categoryMenus.Add(WeaponCategory.MG, CreateMenu("", TitleMG, false, true, false));
			_categoryMenus.Add(WeaponCategory.Pistol, CreateMenu("", TitlePistols, false, true, false));
			_categoryMenus.Add(WeaponCategory.Rifle, CreateMenu("", TitleRifles, false, true, false));
			_categoryMenus.Add(WeaponCategory.Shotgun, CreateMenu("", TitleShotguns, false, true, false));
			_categoryMenus.Add(WeaponCategory.SMG, CreateMenu("", TitleSMG, false, true, false));
			_categoryMenus.Add(WeaponCategory.Sniper, CreateMenu("", TitleSR, false, true, false));
			_categoryMenus.Add(WeaponCategory.StunGun, CreateMenu("", TitleSG, false, true, false));
			_categoryMenus.Add(WeaponCategory.Thrown, CreateMenu("", TitleThrown, false, true, false));
			_categoryMenus.Add(WeaponCategory.RubberGun, CreateMenu("", "Less Lethal", false, true, false));
			_categoryMenus.Add(WeaponCategory.DigiScanner, CreateMenu("", "Digiscanners", false, true, false));
			_categoryMenus.Add(WeaponCategory.FireExtinguisher, CreateMenu("", "Fire Extinguishers", false, true, false));
			_categoryMenus.Add(WeaponCategory.HackingDevice, CreateMenu("", "Hacking Devices", false, true, false));
			_categoryMenus.Add(WeaponCategory.MetalDetector, CreateMenu("", "Metal Detectors", false, true, false));
			_categoryMenus.Add(WeaponCategory.NightVision, CreateMenu("", "Night Visions", false, true, false));
			_categoryMenus.Add(WeaponCategory.Parachute, CreateMenu("", "Parachutes", false, true, false));
			_categoryMenus.Add(WeaponCategory.PetrolCan, CreateMenu("", "Petrol Cans", false, true, false));
			_categoryMenus.Add(WeaponCategory.Tranquilizer, CreateMenu("", "Tranquilizers", false, true, false));
		}

		private static void LoadCustomMenus()
		{
			if (!File.Exists("Scripts\\AddonWeapons\\commandline.txt"))
			{
				return;
			}
			string[] array = File.ReadAllLines("Scripts\\AddonWeapons\\commandline.txt");
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim();
				if (!string.IsNullOrEmpty(text) && !text.StartsWith("//") && text.StartsWith("CreateWeaponCategory"))
				{
					string[] array2 = _weaponManager.ExtractParameters(text, "CreateWeaponCategory", 1);
					NativeMenu nativeMenu = CreateMenu("", array2[0], false, true, false);
					_customMenus.Add(nativeMenu);
					MainMenu.AddSubMenu(nativeMenu);
				}
			}
		}

		private static void AddMenusToPool()
		{
			Pool.Add(MainMenu);
			Pool.Add(ComponentMenu);
			foreach (NativeMenu value in _categoryMenus.Values)
			{
				Pool.Add(value);
			}
			foreach (NativeMenu customMenu in _customMenus)
			{
				Pool.Add(customMenu);
			}
		}

		public static NativeMenu GetMenuForCategory(WeaponCategory category)
		{
			NativeMenu value;
			if (!_categoryMenus.TryGetValue(category, out value))
			{
				return null;
			}
			return value;
		}

		public static void CloseAllMenus()
		{
			if (MainMenu.Visible)
			{
				MainMenu.Visible = false;
				CurrentOpenedMenu = MainMenu;
			}
			if (ComponentMenu.Visible)
			{
				ComponentMenu.Visible = false;
				CurrentOpenedMenu = ComponentMenu;
			}
			foreach (NativeMenu value in _categoryMenus.Values)
			{
				if (value.Visible)
				{
					value.Visible = false;
					CurrentOpenedMenu = value;
				}
			}
			foreach (NativeMenu customMenu in _customMenus)
			{
				if (customMenu.Visible)
				{
					customMenu.Visible = false;
					CurrentOpenedMenu = customMenu;
				}
			}
		}

		public static void BuildMainMenu()
		{
			foreach (NativeMenu value in _categoryMenus.Values)
			{
				if (value.Items.Count > 0)
				{
					MainMenu.AddSubMenu(value);
				}
			}
			foreach (NativeMenu customMenu in _customMenus)
			{
				if (customMenu.Items.Count > 0)
				{
					MainMenu.AddSubMenu(customMenu);
				}
			}
		}

		public static bool IsMenuOpen()
		{
			if (Pool.AreAnyVisible)
			{
				return _menuOpenedFlag == 1;
			}
			return false;
		}

		public static void RefreshAllMenus()
		{
			foreach (NativeMenu value in _categoryMenus.Values)
			{
				value.Clear();
			}
			foreach (NativeMenu customMenu in _customMenus)
			{
				customMenu.Clear();
			}
			Items.Initialize();
		}

		public static void ReOpenLastMenu()
		{
			if (CurrentOpenedMenu != null && !ComponentMenu.Visible)
			{
				CurrentOpenedMenu.Visible = true;
				CurrentOpenedMenu = null;
			}
		}
	}
}
