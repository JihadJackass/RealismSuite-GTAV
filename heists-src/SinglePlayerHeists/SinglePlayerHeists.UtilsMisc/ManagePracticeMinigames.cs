using System;
using System.Collections.Generic;
using System.Drawing;
using GTA;
using GTA.UI;
using HKHModHelperNew;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Menus;

namespace SinglePlayerHeists.UtilsMisc
{
	internal class ManagePracticeMinigames : Script
	{
		public static ObjectPool DrillingObjectPool;

		public static NativeMenu DrillingmainMenu;

		public static ObjectPool HackingObjectPool;

		public static NativeMenu HackingmainMenu;

		public static ScaledRectangle RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 108));

		public static List<NativeMenu> GLemonUIMenusNativeMenus = new List<NativeMenu>();

		public static Model RequestModel(string Name)
		{
			Model result = new Model(Name);
			result.Request(10000);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(50);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public static Model RequestModel(int Name)
		{
			Model result = new Model(Name);
			result.Request(2000);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(50);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public static Model RequestModel(VehicleHash Name)
		{
			Model result = new Model(Name);
			result.Request(10000);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(50);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public static Model RequestModel(PedHash Name)
		{
			Model result = new Model(Name);
			result.Request(10000);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(50);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
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

		public ManagePracticeMinigames()
		{
			CreateBanner();
			DrillingObjectPool = new ObjectPool();
			DrillingmainMenu = new NativeMenu("Drilling Minigames", "Select an Option");
			DrillingmainMenu.BannerText.Font = GTA.UI.Font.RockstarTag;
			DrillingObjectPool.Add(DrillingmainMenu);
			GLemonUIMenusNativeMenus.Add(DrillingmainMenu);
			NativeItem FleecaDrill = new NativeItem("Launch Fleeca Drill");
			DrillingmainMenu.Add(FleecaDrill);
			FleecaDrill.Activated += delegate
			{
				HackingObjectPool.HideAll();
				if (DrillingMinigames.FleecaDrillActive)
				{
					FleecaDrill.Title = "Launch Fleeca Drill";
					DrillingMinigames.EndFleecaDrill();
				}
				else if (!DrillingMinigames.FleecaDrillActive)
				{
					FleecaDrill.Title = "Quit Fleeca Drill";
					Game.Player.Character.Position = Main.PracticeDrill;
					DrillingMinigames.StartFleecaDrill();
				}
			};
			NativeItem CasinoDrill = new NativeItem("Launch Casino Drill");
			DrillingmainMenu.Add(CasinoDrill);
			CasinoDrill.Activated += delegate
			{
				HackingObjectPool.HideAll();
				if (DrillingMinigames.CurrentDrill != 0)
				{
					CasinoDrill.Title = "Launch Casino Drill";
					DrillingMinigames.EndCasinoDrillorLaser();
				}
				else
				{
					CasinoDrill.Title = "Quit Casino Drill";
					Game.Player.Character.Position = Main.PracticeDrill;
					DrillingMinigames.StartCasinoDrill();
				}
			};
			NativeItem CasinoLaser = new NativeItem("Launch Casino Laser");
			DrillingmainMenu.Add(CasinoLaser);
			CasinoLaser.Activated += delegate
			{
				HackingObjectPool.HideAll();
				if (DrillingMinigames.CurrentDrill != 0)
				{
					CasinoLaser.Title = "Launch Casino Laser";
					DrillingMinigames.EndCasinoDrillorLaser();
				}
				else
				{
					CasinoLaser.Title = "Quit Casino Laser";
					Game.Player.Character.Position = Main.PracticeDrill;
					DrillingMinigames.StartCasinoLaser();
				}
			};
			HackingObjectPool = new ObjectPool();
			HackingmainMenu = new NativeMenu("Hacking Minigames", "Select an Option");
			HackingmainMenu.BannerText.Font = GTA.UI.Font.RockstarTag;
			HackingObjectPool.Add(HackingmainMenu);
			GLemonUIMenusNativeMenus.Add(HackingmainMenu);
			NativeCheckboxItem DisableHacks = new NativeCheckboxItem("Disable Hacks in Heists");
			HackingmainMenu.Add(DisableHacks);
			DisableHacks.Checked = Main.DisableHacks;
			DisableHacks.CheckboxChanged += delegate
			{
				Main.DisableHacks = DisableHacks.Checked;
				Main.Config.SetValue("Misc", "DisableHacks", Main.DisableHacks);
				Main.Config.Save();
				Notification.Show("Disable Hacks in Heist set to " + Main.DisableHacks);
			};
			NativeItem nativeItem = new NativeItem("Fleeca Circuit Hack");
			HackingmainMenu.Add(nativeItem);
			nativeItem.Activated += delegate
			{
				HackingObjectPool.HideAll();
				FingerprintHack.RunCircuitHack();
			};
			NativeItem nativeItem2 = new NativeItem("Bruteforce Minigame (Both)");
			HackingmainMenu.Add(nativeItem2);
			nativeItem2.Activated += delegate
			{
				HackingObjectPool.HideAll();
				FingerprintHack.RunBruteforceHack_Both();
			};
			NativeItem nativeItem3 = new NativeItem("Bruteforce Minigame (Numbers)");
			HackingmainMenu.Add(nativeItem3);
			nativeItem3.Activated += delegate
			{
				HackingObjectPool.HideAll();
				FingerprintHack.RunBruteforceHack_Numbers();
			};
			NativeItem nativeItem4 = new NativeItem("Bruteforce Minigame (Passcode)");
			HackingmainMenu.Add(nativeItem4);
			nativeItem4.Activated += delegate
			{
				HackingObjectPool.HideAll();
				FingerprintHack.RunBruteforceHack_Password();
			};
			NativeItem nativeItem5 = new NativeItem("Casino Hack (Fingerprint)");
			HackingmainMenu.Add(nativeItem5);
			nativeItem5.Activated += delegate
			{
				HackingObjectPool.HideAll();
				FingerprintHack.RunFingerPrintHack();
			};
			NativeItem nativeItem6 = new NativeItem("Casino Hack (Dots)");
			HackingmainMenu.Add(nativeItem6);
			nativeItem6.Activated += delegate
			{
				HackingObjectPool.HideAll();
				FingerprintHack.RunKeypadHack();
			};
			NativeItem nativeItem7 = new NativeItem("Fingerprint Cloner (Cayo Perico)");
			HackingmainMenu.Add(nativeItem7);
			nativeItem7.Activated += delegate
			{
				HackingObjectPool.HideAll();
				FingerprintHack.RunCopyPrintHack();
			};
			NativeItem nativeItem8 = new NativeItem("Voltage Minigame (Cayo Perico)");
			HackingmainMenu.Add(nativeItem8);
			nativeItem8.Activated += delegate
			{
				HackingObjectPool.HideAll();
				FingerprintHack.RunVoltageHack();
			};
			for (int i = 0; i < GLemonUIMenusNativeMenus.Count; i++)
			{
				NativeMenu banner = GLemonUIMenusNativeMenus[i];
				SetBanner(banner);
			}
			base.Tick += Ontick;
		}

		public void Ontick(object sender, EventArgs e)
		{
			if (HackingObjectPool != null)
			{
				HackingObjectPool.Process();
			}
			if (DrillingObjectPool != null)
			{
				DrillingObjectPool.Process();
			}
		}
	}
}
