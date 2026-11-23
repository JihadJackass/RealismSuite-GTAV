using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GTA;
using GTA.UI;
using iFruitAddon2;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Menus;

namespace SinglePlayerHeists.UtilsMisc
{
	internal class SPH_SettingsMenu : Script
	{
		private ObjectPool modLemonUIObjectPool;

		private NativeMenu mainMenu;

		private NativeMenu Inventory;

		private NativeMenu Bribes;

		private NativeMenu TaxiService;

		private CustomiFruit ifruit;

		public ScaledRectangle RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 0));

		public List<NativeMenu> GLemonUIMenusNativeMenus = new List<NativeMenu>();

		public static bool ShowDebugTake;

		public void loadMenu(iFruitContact contact)
		{
			ifruit.Close();
			mainMenu.Visible = !mainMenu.Visible;
		}

		public SPH_SettingsMenu()
		{
			ifruit = new CustomiFruit
			{
				CenterButtonColor = Color.Orange,
				LeftButtonColor = Color.LimeGreen,
				RightButtonColor = Color.Purple,
				CenterButtonIcon = SoftKeyIcon.Fire,
				LeftButtonIcon = SoftKeyIcon.Police,
				RightButtonIcon = SoftKeyIcon.Website
			};
			ifruit.SetWallpaper(new Wallpaper("char_facebook"));
			ifruit.SetWallpaper(Wallpaper.BadgerDefault);
			iFruitContact iFruitContact = new iFruitContact("Heist Settings");
			iFruitContact.Answered += loadMenu;
			iFruitContact.DialTimeout = 3000;
			iFruitContact.Active = true;
			iFruitContact.Icon = ContactIcon.Blocked;
			iFruitContact.Name = "SPH Settings";
			ifruit.Contacts.Add(iFruitContact);
			base.Aborted += OnShutdown;
			base.Tick += onTick;
			Setup();
		}

		public void SetBanner(NativeMenu menu)
		{
			menu.Banner = RectangleGUI;
		}

		public void CreateBanner()
		{
			RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 108));
			RectangleGUI.Color = Color.FromArgb(255, 0, 0, 0);
		}

		public void Setup()
		{
			CreateBanner();
			modLemonUIObjectPool = new ObjectPool();
			mainMenu = new NativeMenu("Single Player Heists", "Select an Option");
			GLemonUIMenusNativeMenus.Add(mainMenu);
			modLemonUIObjectPool.Add(mainMenu);
			NativeMenu nativeMenu = new NativeMenu("Single Player Heists", "Settings");
			mainMenu.AddSubMenu(nativeMenu);
			modLemonUIObjectPool.Add(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			List<object> list = new List<object>();
			list.Add("Dollar ($)");
			list.Add("Heist H");
			list.Add("Dollarbill");
			list.Add("Bag with H");
			List<object> list2 = new List<object>();
			list2.Add("Pixel Pete's - Paleto Bay");
			list2.Add("Wonderama - Grapeseed");
			list2.Add("Videogeddon -La Mesa");
			list2.Add("Warehouse - Davis");
			list2.Add("Insert Coin - Rockford Hills");
			list2.Add("Eight Bit - Vinewood");
			NativeItem nativeItem = new NativeItem("Override Lester Text");
			nativeMenu.Add(nativeItem);
			nativeItem.Activated += delegate
			{
				Main.LesterHasToldPlayerOfNewHeists = 1;
				Main.Config.SetValue("Misc", "LesterHasToldPlayerOfNewHeists", Main.LesterHasToldPlayerOfNewHeists);
				Main.Config.Save();
				Main.ToldPlayerHowToStartHeist = 1;
				Main.Config.SetValue("Misc", "ToldPlayerHowToStartHeist", Main.ToldPlayerHowToStartHeist);
				Main.Config.Save();
				Main.RanTutorial = 1;
				Main.ArcadeVersion = 1;
				Main.ArcadeConfig.SetValue("Arcade", "ArcadeVersion", Main.ArcadeVersion);
				Main.ArcadeConfig.Save();
				Main.ChangeArcade();
			};
			NativeListItem<dynamic> Chk11 = new NativeListItem<object>("Arcade Locaiton ", list2.ToArray());
			nativeMenu.Add(Chk11);
			try
			{
				Chk11.SelectedIndex = Main.ArcadeVersion;
			}
			catch
			{
			}
			Chk11.ItemChanged += delegate
			{
				Main.ArcadeVersion = Chk11.SelectedIndex + 1;
				Main.ArcadeConfig.SetValue("Arcade", "ArcadeVersion", Main.ArcadeVersion);
				Main.ArcadeConfig.Save();
				Main.ArcadeVersion = Chk11.SelectedIndex + 1;
				Main.ChangeArcade();
				Main.ArcadeConfig.SetValue("Arcade", "ArcadeVersion", Main.ArcadeVersion);
				Main.ArcadeConfig.Save();
				Notification.Show("Changed Arcade Location");
			};
			NativeListItem<dynamic> Chk17 = new NativeListItem<object>("Heist Icon ", list.ToArray());
			nativeMenu.Add(Chk17);
			try
			{
				Chk17.SelectedIndex = Class1.Heist_Blip_Type;
			}
			catch
			{
			}
			Chk17.ItemChanged += delegate
			{
				Class1.Heist_Blip_Type = Chk17.SelectedIndex;
				Main.Config.SetValue("Misc", "Heist_Blip_Type", Class1.Heist_Blip_Type);
				Main.Config.Save();
				Class1.SetupActivities();
				Notification.Show("Changed Heist Blip Sprite");
			};
			NativeItem nativeItem2 = new NativeItem("Dismiss All Crew");
			nativeMenu.Add(nativeItem2);
			nativeItem2.Activated += delegate
			{
				if (BodyguardMenu.Board1 != null)
				{
					BodyguardMenu.Board1.Delete();
				}
				if (BodyguardMenu.Board2 != null)
				{
					BodyguardMenu.Board2.Delete();
				}
				Game.Player.Character.Opacity = 255;
				BodyguardMenu.PickedAi = false;
				if (BodyguardMenu.RobberA != null)
				{
					BodyguardMenu.RobberA.Delete();
					BodyguardMenu.RobberA = null;
				}
				if (BodyguardMenu.RobberB != null)
				{
					BodyguardMenu.RobberB.Delete();
					BodyguardMenu.RobberB = null;
				}
				if (BodyguardMenu.RobberC != null)
				{
					BodyguardMenu.RobberC.Delete();
					BodyguardMenu.RobberC = null;
				}
			};
			NativeCheckboxItem Chk2 = new NativeCheckboxItem("Disable Hacks in Heists");
			nativeMenu.Add(Chk2);
			Chk2.CheckboxChanged += delegate
			{
				Main.DisableHacks = Chk2.Checked;
				Main.SaveSettingsData();
			};
			Chk2.Checked = Main.DisableHacks;
			NativeCheckboxItem Chk3 = new NativeCheckboxItem("Disable Ai Intergration");
			nativeMenu.Add(Chk3);
			Chk3.CheckboxChanged += delegate
			{
				Class1.Disable_Bodyguard_AiIntergration = Chk3.Checked;
				Main.SaveSettingsData();
			};
			Chk3.Checked = Class1.Disable_Bodyguard_AiIntergration;
			NativeCheckboxItem Chk4 = new NativeCheckboxItem("Disable PlayerCompanion Ai Intergration");
			nativeMenu.Add(Chk4);
			Chk4.CheckboxChanged += delegate
			{
				Class1.Disable_PlayerCompanion_AiIntergration = Chk4.Checked;
				Main.SaveSettingsData();
			};
			Chk4.Checked = Class1.Disable_PlayerCompanion_AiIntergration;
			NativeCheckboxItem Chk5 = new NativeCheckboxItem("Disable Heist Icon Blips");
			nativeMenu.Add(Chk5);
			Chk5.CheckboxChanged += delegate
			{
				Class1.Disable_HeistIcon_Blips = Chk5.Checked;
				Class1.SetupActivities();
				Main.SaveSettingsData();
			};
			Chk5.Checked = Class1.Disable_HeistIcon_Blips;
			NativeCheckboxItem Chk6 = new NativeCheckboxItem("Disable Single Player Pause");
			nativeMenu.Add(Chk6);
			Chk6.CheckboxChanged += delegate
			{
				Class1.Disable_Single_Player_Pause = Chk6.Checked;
				Main.SaveSettingsData();
			};
			Chk6.Checked = Class1.Disable_Single_Player_Pause;
			NativeCheckboxItem Chk7 = new NativeCheckboxItem("Show Chevron Markers On Loot");
			nativeMenu.Add(Chk7);
			Chk7.CheckboxChanged += delegate
			{
				ManageRobberyAnims.ShowChevronMarkersOnLoot = Chk7.Checked;
				Main.SaveSettingsData();
			};
			Chk7.Checked = ManageRobberyAnims.ShowChevronMarkersOnLoot;
			NativeCheckboxItem Chk8 = new NativeCheckboxItem("Block Bodyguard Mantle");
			nativeMenu.Add(Chk8);
			Chk8.CheckboxChanged += delegate
			{
				ManageBodyguards.Block_Bodyguard_Mantle = Chk8.Checked;
				Main.SaveSettingsData();
			};
			Chk8.Checked = ManageBodyguards.Block_Bodyguard_Mantle;
			NativeCheckboxItem Chk9 = new NativeCheckboxItem("Show Secondary Safe UI");
			nativeMenu.Add(Chk9);
			Chk9.CheckboxChanged += delegate
			{
				ManageRobberyAnims.ShowSecondarySafeUI = Chk9.Checked;
				Main.SaveSettingsData();
			};
			Chk9.Checked = ManageRobberyAnims.ShowSecondarySafeUI;
			NativeCheckboxItem Chk10 = new NativeCheckboxItem("PaintingCut Disable Require Input");
			nativeMenu.Add(Chk10);
			Chk10.CheckboxChanged += delegate
			{
				ManageRobberyAnims.PaintingCut_Disable_Require_Input = Chk10.Checked;
				Main.SaveSettingsData();
			};
			Chk10.Checked = ManageRobberyAnims.PaintingCut_Disable_Require_Input;
			NativeCheckboxItem Chk = new NativeCheckboxItem("{DEBUG} Show Robbery Markers ");
			nativeMenu.Add(Chk);
			Chk.CheckboxChanged += delegate
			{
				SPH_Debug.ShowRobberyMarkers = Chk.Checked;
			};
			Chk.Checked = SPH_Debug.ShowRobberyMarkers;
			NativeCheckboxItem Chk18 = new NativeCheckboxItem("{DEBUG} Show Main Debug Debug ");
			nativeMenu.Add(Chk18);
			Chk18.CheckboxChanged += delegate
			{
				SPH_Debug.ShowMainDebug = Chk18.Checked;
			};
			Chk18.Checked = SPH_Debug.ShowMainDebug;
			NativeCheckboxItem Chk19 = new NativeCheckboxItem("{DEBUG} Save Vehicle Location ");
			nativeMenu.Add(Chk19);
			Chk19.CheckboxChanged += delegate
			{
				SPH_Debug.AllowSaveNewLocateAVeh = Chk19.Checked;
			};
			Chk19.Checked = SPH_Debug.AllowSaveNewLocateAVeh;
			NativeCheckboxItem Chk20 = new NativeCheckboxItem("{DEBUG} Show Run Smart Funcs ");
			nativeMenu.Add(Chk20);
			Chk20.CheckboxChanged += delegate
			{
				SPH_Debug.ShowRunSmartFuncs_AiAmt = Chk20.Checked;
			};
			Chk20.Checked = SPH_Debug.ShowRunSmartFuncs_AiAmt;
			NativeCheckboxItem Chk21 = new NativeCheckboxItem("{DEBUG} Show Painting Offsets ");
			nativeMenu.Add(Chk21);
			Chk21.CheckboxChanged += delegate
			{
				SPH_Debug.ShowPaintingOffsets = Chk21.Checked;
			};
			Chk21.Checked = SPH_Debug.ShowPaintingOffsets;
			NativeCheckboxItem Chk22 = new NativeCheckboxItem("{DEBUG} Show Casino Drilling Debug ");
			nativeMenu.Add(Chk22);
			Chk22.CheckboxChanged += delegate
			{
				SPH_Debug.CasinoDrillDebug = Chk22.Checked;
			};
			Chk22.Checked = SPH_Debug.CasinoDrillDebug;
			NativeCheckboxItem Chk23 = new NativeCheckboxItem("{DEBUG} Show Ai Looting Debug ");
			nativeMenu.Add(Chk23);
			Chk23.CheckboxChanged += delegate
			{
				SPH_Debug.ShowAiLootingDebug = Chk23.Checked;
			};
			Chk23.Checked = SPH_Debug.ShowAiLootingDebug;
			NativeCheckboxItem Chk24 = new NativeCheckboxItem("{DEBUG} Get Nearby Loot From Props ");
			nativeMenu.Add(Chk24);
			Chk24.CheckboxChanged += delegate
			{
				SPH_Debug.GetNearbyLootFromProps = Chk24.Checked;
			};
			Chk24.Checked = SPH_Debug.GetNearbyLootFromProps;
			NativeCheckboxItem Chk12 = new NativeCheckboxItem("{DEBUG} Show Ai Chevrons ");
			nativeMenu.Add(Chk12);
			Chk12.CheckboxChanged += delegate
			{
				SPH_Debug.ShowAiChevrons = Chk12.Checked;
			};
			Chk12.Checked = SPH_Debug.ShowAiChevrons;
			NativeCheckboxItem Chk13 = new NativeCheckboxItem("{DEBUG} Show Crack Safe MSGs");
			nativeMenu.Add(Chk13);
			Chk13.CheckboxChanged += delegate
			{
				SPH_Debug.ShowCrackSafeMSGsDebug = Chk13.Checked;
			};
			Chk13.Checked = SPH_Debug.ShowCrackSafeMSGsDebug;
			NativeCheckboxItem Chk14 = new NativeCheckboxItem("{DEBUG} Show Loot Type Text");
			nativeMenu.Add(Chk14);
			Chk14.CheckboxChanged += delegate
			{
				ManageRobberyAnims.ShowLootTypeText = Chk14.Checked;
			};
			Chk14.Checked = ManageRobberyAnims.ShowLootTypeText;
			NativeCheckboxItem Chk15 = new NativeCheckboxItem("{DEBUG} Show Debug Take");
			nativeMenu.Add(Chk15);
			Chk15.CheckboxChanged += delegate
			{
				ShowDebugTake = Chk15.Checked;
			};
			Chk15.Checked = ShowDebugTake;
			NativeCheckboxItem Chk16 = new NativeCheckboxItem("{DEBUG} Show Running Classes");
			nativeMenu.Add(Chk16);
			Chk16.CheckboxChanged += delegate
			{
				SPH_Debug.ShowRunningClassDebug = Chk16.Checked;
			};
			Chk16.Checked = SPH_Debug.ShowRunningClassDebug;
			for (int i = 0; i < GLemonUIMenusNativeMenus.Count(); i++)
			{
				NativeMenu banner = GLemonUIMenusNativeMenus[i];
				SetBanner(banner);
			}
		}

		private void onTick(object sender, EventArgs e)
		{
			if (ShowDebugTake)
			{
				UI.DrawPayment("TAKE", ManageRobberyAnims.CurrentPay);
			}
			if (modLemonUIObjectPool != null)
			{
				modLemonUIObjectPool.Process();
			}
			if (ifruit != null)
			{
				ifruit.Update();
			}
		}

		private void OnShutdown(object sender, EventArgs e)
		{
		}
	}
}
