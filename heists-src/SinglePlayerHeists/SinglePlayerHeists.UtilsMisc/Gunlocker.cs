using System;
using System.Collections.Generic;
using System.Drawing;
using GTA;
using HKHModHelperNew;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Menus;

namespace SinglePlayerHeists.UtilsMisc
{
	internal class Gunlocker : Script
	{
		public static ScaledRectangle RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 108));

		public static List<NativeMenu> GLemonUIMenusNativeMenus = new List<NativeMenu>();

		public static bool SetupGunLockerMenu;

		public static NativeMenu weaponsMenu;

		public static NativeMenu GunmainMenu;

		public static ObjectPool GunmodLemonUIObjectPool;

		public static void SetBanner(NativeMenu menu)
		{
			menu.Banner = RectangleGUI;
		}

		public static void CreateBanner()
		{
			RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 108));
			RectangleGUI.Color = Color.FromArgb(255, 0, 0, 0);
		}

		public Gunlocker()
		{
			base.Tick += Ontick;
		}

		public void Ontick(object sender, EventArgs e)
		{
			if (SetupGunLockerMenu)
			{
				GunmodLemonUIObjectPool.Process();
			}
			else if (!SetupGunLockerMenu)
			{
				GunmodLemonUIObjectPool = new ObjectPool();
				GunmainMenu = new NativeMenu("Gunlocker", "Select an Option");
				GLemonUIMenusNativeMenus.Add(GunmainMenu);
				GunmodLemonUIObjectPool.Add(GunmainMenu);
				weaponsMenu = new NativeMenu("Gunlocker", "Weapons");
				GunmainMenu.AddSubMenu(weaponsMenu);
				GunmodLemonUIObjectPool.Add(weaponsMenu);
				GLemonUIMenusNativeMenus.Add(weaponsMenu);
				HKHModHelperNew.Class1 @class = new HKHModHelperNew.Class1();
				@class.SetupWeapons(GunmodLemonUIObjectPool, weaponsMenu, GLemonUIMenusNativeMenus);
				for (int i = 0; i < GLemonUIMenusNativeMenus.Count; i++)
				{
					NativeMenu banner = GLemonUIMenusNativeMenus[i];
					SetBanner(banner);
				}
				SetupGunLockerMenu = true;
			}
		}
	}
}
