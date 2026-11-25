using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.UI;
using HKHModHelperNew;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Menus;

namespace GTAO_DiamondCasino
{
	internal class PurchaseChips : Script
	{
		public static ScaledRectangle RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 0));

		public static List<NativeMenu> GLemonUIMenusNativeMenus = new List<NativeMenu>();

		public static ObjectPool BusinessMain_ObjectPool;

		public static NativeMenu BusinessMain_Menu;

		public static float Mp = 1f;

		public static Vector3 PurchaseMarker = new Vector3(1115.76f, 220f, -50.4f);

		public static bool RunMethodNow()
		{
			bool result = true;
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, PurchaseMarker, Class1.GetDistSign.Greater, 5f))
			{
				_Debug.PurchaseChips = 0;
				result = false;
			}
			return result;
		}

		public static void SetBanner(NativeMenu menu)
		{
			menu.Banner = new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), "shopui_title_casino", "shopui_title_casino");
		}

		public static void CreateBanner()
		{
			RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 0));
			RectangleGUI.Color = Color.FromArgb(255, 255, 255, 255);
		}

		public PurchaseChips()
		{
			BusinessMain_ObjectPool = new ObjectPool();
			BusinessMain_Menu = new NativeMenu("", "Purchase Chips", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), "shopui_title_casino", "shopui_title_casino"));
			BusinessMain_ObjectPool.Add(BusinessMain_Menu);
			GLemonUIMenusNativeMenus.Add(BusinessMain_Menu);
			CHipsM();
			for (int i = 0; i < GLemonUIMenusNativeMenus.Count(); i++)
			{
				SetBanner(GLemonUIMenusNativeMenus[i]);
			}
			base.Tick += OnTick;
		}

		public void CHipsM()
		{
			float Chips = 10f;
			float Money = 10f;
			List<dynamic> Cash = new List<object>();
			List<dynamic> Multi = new List<object>();
			Multi.Add(1.0);
			Multi.Add(1.05);
			Multi.Add(1.1);
			Multi.Add(1.15);
			Multi.Add(1.2);
			Multi.Add(1.25);
			Multi.Add(1.3);
			Multi.Add(1.35);
			Multi.Add(1.4);
			Multi.Add(1.45);
			Multi.Add(1.5);
			Multi.Add(1.55);
			Multi.Add(1.6);
			Multi.Add(1.65);
			Multi.Add(1.7);
			Multi.Add(1.75);
			Multi.Add(1.8);
			Multi.Add(1.95);
			Multi.Add(2.0);
			Cash.Add(10);
			Cash.Add(50);
			Cash.Add(100);
			Cash.Add(500);
			Cash.Add(1000);
			Cash.Add(5000);
			Cash.Add(10000);
			Cash.Add(100000);
			Cash.Add(1000000);
			NativeMenu nativeMenu = new NativeMenu("", "Purchase Chips", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), "shopui_title_casino", "shopui_title_casino"));
			BusinessMain_ObjectPool.Add(nativeMenu);
			BusinessMain_Menu.AddSubMenu(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			NativeItem Buy = new NativeItem("Buy " + Chips.ToString("N") + "c", "", "$" + Money.ToString("N"));
			nativeMenu.Add(Buy);
			NativeListItem<dynamic> list = new NativeListItem<object>("Chips Amount : ", Cash.ToArray());
			nativeMenu.Add(list);
			list.ItemChanged += delegate
			{
				Chips = Cash[list.SelectedIndex];
				Money = Chips * Mp;
				Buy.Title = "Buy  " + Chips.ToString("N") + "c";
				Buy.AltTitle = "$" + Money.ToString("N");
			};
			NativeItem item2 = new NativeItem("-----------------------------------------------------------");
			nativeMenu.Add(item2);
			NativeListItem<dynamic> C_Mp = new NativeListItem<object>("Multiplier : ", Multi.ToArray());
			nativeMenu.Add(C_Mp);
			NativeItem nativeItem = new NativeItem("Change Chips Multiplier");
			nativeMenu.Add(nativeItem);
			NativeItem item3 = new NativeItem("-----------------------------------------------------------");
			nativeMenu.Add(item3);
			List<dynamic> Chip = new List<object>();
			Chip.Add(10);
			Chip.Add(50);
			Chip.Add(100);
			Chip.Add(500);
			Chip.Add(1000);
			Chip.Add(5000);
			Chip.Add(10000);
			Chip.Add(25000);
			Chip.Add(50000);
			Chip.Add(75000);
			Chip.Add(100000);
			Chip.Add(125000);
			Chip.Add(150000);
			Chip.Add(300000);
			NativeListItem<dynamic> C4M = new NativeListItem<object>("Amount : ", Chip.ToArray());
			nativeMenu.Add(C4M);
			NativeItem nativeItem2 = new NativeItem("Exchange " + Chip[C4M.SelectedIndex] + "c Chips for $ " + Chip[C4M.SelectedIndex] * Mp);
			nativeMenu.Add(nativeItem2);
			NativeItem C_C4M2 = new NativeItem("Exchange " + CasinoGames.Chips_Amount + "c Chips for $ " + (float)CasinoGames.Chips_Amount * Mp);
			nativeMenu.Add(C_C4M2);
			nativeItem2.Title = "Exchange " + Chip[C4M.SelectedIndex] + "c Chips for $ " + Chip[C4M.SelectedIndex] * Mp;
			C_C4M2.Activated += delegate
			{
				if (CasinoGames.Chips_Amount >= 0)
				{
					ManagePlayerFunds.AddFunds((int)((float)CasinoGames.Chips_Amount * Mp));
					CasinoGames.Chips_Amount = 0;
					Class1.Config.SetValue("Setup", "Chips_Amount", CasinoGames.Chips_Amount);
					Class1.Config.Save();
					C_C4M2.Title = "Exchange " + CasinoGames.Chips_Amount + "c Chips for Money";
					Notification.Show(Class1.GetHostName() + " : Sold all Chips");
				}
				else
				{
					Notification.Show(Class1.GetHostName() + " : You do not have that amount of Chips");
				}
			};
			nativeItem2.Activated += delegate
			{
				if (CasinoGames.Chips_Amount - Chip[C4M.SelectedIndex] >= 0)
				{
					ManagePlayerFunds.AddFunds((int)(Chip[C4M.SelectedIndex] * Mp));
					CasinoGames.Chips_Amount = (int)(CasinoGames.Chips_Amount - Chip[C4M.SelectedIndex]);
					Class1.Config.SetValue("Setup", "Chips_Amount", CasinoGames.Chips_Amount);
					Class1.Config.Save();
					C_C4M2.Title = "Exchange " + CasinoGames.Chips_Amount + "c Chips for Money";
					Notification.Show(Class1.GetHostName() + " : Sold " + Chip[C4M.SelectedIndex] + " worth of Chips");
				}
				else
				{
					Notification.Show(Class1.GetHostName() + " : You do not have that amount of Chips");
				}
			};
			nativeItem.Activated += delegate
			{
				Mp = (float)Multi[C_Mp.SelectedIndex];
				Class1.Config.SetValue("Setup", "Chips_Multiplier", Mp);
				Class1.Config.Save();
				Notification.Show(Class1.GetHostName() + " : Cjips Multiplier changed to : " + Multi[C_Mp.SelectedIndex]);
			};
			Chips = Cash[list.SelectedIndex];
			Money = Chips * Mp;
			Buy.Title = "Buy  " + Chips.ToString("N") + "c";
			Buy.AltTitle = "$" + Money.ToString("N");
			Buy.Activated += delegate
			{
				if (ManagePlayerFunds.AreFundsHigher(Money))
				{
					ManagePlayerFunds.RemoveFunds((int)Money);
					CasinoGames.Chips_Amount = (int)(CasinoGames.Chips_Amount + Cash[list.SelectedIndex]);
					Class1.Config.SetValue("Setup", "Chips_Amount", CasinoGames.Chips_Amount);
					Class1.Config.Save();
					Notification.Show(Class1.GetHostName() + " : Successfuly purchased " + Cash[list.SelectedIndex] + " Chips");
				}
				else
				{
					Notification.Show(Class1.GetHostName() + " : You do not have enough money for this amount of chips");
				}
			};
			Chips = Cash[list.SelectedIndex];
			Money = Chips * Mp;
			Buy.Title = "Buy  " + Chips.ToString("N") + "c";
			Buy.AltTitle = "$" + Money.ToString("N");
		}

		public static void OnTick(object sender, EventArgs e)
		{
			_Debug.PurchaseChips = 0;
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, PurchaseMarker, Class1.GetDistSign.Greater, 5f))
			{
				_Debug.PurchaseChips = 0;
				return;
			}
			_Debug.PurchaseChips = 1;
			if (BusinessMain_ObjectPool != null)
			{
				BusinessMain_ObjectPool.Process();
			}
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, PurchaseMarker, Class1.GetDistSign.Less, 1.5f))
			{
				Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Purchase a Chips");
				if (Game.IsControlJustPressed(Control.Context) && BusinessMain_Menu != null && !BusinessMain_Menu.Visible)
				{
					BusinessMain_Menu.Visible = !BusinessMain_Menu.Visible;
				}
			}
		}
	}
}
