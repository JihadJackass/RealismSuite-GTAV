using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using GTAO_DiamondCasino.Penthouse;
using HKHModHelperNew;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Menus;

namespace GTAO_DiamondCasino.Menus
{
	public class PurchasePenthouse : Script
	{
		public static Quaternion StyleCamPosH = new Quaternion(984.47f, 63.94f, 116.7f, 59.14f);

		public static Quaternion BarCamPosH = new Quaternion(947.11f, 27.16f, 117.44f, -177f);

		public static Quaternion SpaCamPosH = new Quaternion(951.28f, 4.34f, 117.93f, 9.88f);

		public static Quaternion MediaRoomCamPosH = new Quaternion(955.53f, 40.47f, 117.72f, -164f);

		public static Quaternion OfficePosH = new Quaternion(974.74f, 41.1f, 117.2f, 2.2f);

		public static Quaternion ExtraBedroomCamPosH = new Quaternion(989.09f, 64.4f, 117.33f, -4f);

		public static Quaternion BarLightCamPosH = new Quaternion(947.11f, 27.16f, 117.44f, -177f);

		public static Quaternion DealersCamPosH = new Quaternion(962.77f, 34.82f, 116.96f, -125f);

		public static Quaternion ArcadeCamPosH = new Quaternion(940.14f, 4f, 116.4f, -30f);

		public static Quaternion ColorCamPosH = new Quaternion(967.65f, 52.22f, 117f, 138f);

		public static bool IsBuying;

		public static NativeItem StartP;

		public static NativeItem StopP;

		public static NativeMenu PenthouseArt;

		public static NativeMenu submenu14s;

		public static NativeMenu submenu13s;

		public static NativeMenu submenu12s;

		public static ScaledRectangle RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 0));

		public static List<NativeMenu> GLemonUIMenusNativeMenus = new List<NativeMenu>();

		public static ObjectPool BusinessMain_ObjectPool;

		public static NativeMenu BusinessMain_Menu;

		public static Vector3 PurchaseMarker = new Vector3(1088.56f, 219.46f, -50.2f);

		public static NativeMenu submenu0;

		public static NativeMenu submenu16s;

		public static bool CamActive;

		public static Camera ArtPropCam;

		public static bool RunMethodNow()
		{
			bool result = true;
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, PurchaseMarker, Class1.GetDistSign.Greater, 5f))
			{
				_Debug.PurchasePenthouse = 0;
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

		public static void PenthouseOptions()
		{
			PenthouseArtPlacement.GrabArtWork();
			PenthouseArtPlacement.GrabArtWork2();
			PenthouseArtPlacement.SetupArtMenu();
			float InitialCost = 1500000f;
			float Cost = 1500000f;
			float StyleCost = 0f;
			float BarCost = 0f;
			float SpaCost = 0f;
			float MediaCost = 0f;
			float OfficeCost = 0f;
			float ExtraB_Cost = 0f;
			float PDcost = 0f;
			new List<object> { false, true };
			List<object> list = new List<object>();
			list.Add("Blank");
			list.Add("Art Deco");
			list.Add("Art Nouveau");
			list.Add("Classical");
			list.Add("Traditional");
			list.Add("Imperial");
			list.Add("Pop");
			list.Add("Latin");
			list.Add("Natural");
			list.Add("Intricate");
			List<object> list2 = new List<object>();
			list2.Add("No Bar");
			list2.Add("Bar Included");
			List<object> list3 = new List<object>();
			list3.Add("No Spa");
			list3.Add("Spa Included");
			List<object> list4 = new List<object>();
			list4.Add("No Office");
			list4.Add("Office Included");
			List<object> list5 = new List<object>();
			list5.Add("No Media Room");
			list5.Add("Media Room Included");
			List<object> list6 = new List<object>();
			list6.Add("No Extra Room");
			list6.Add("Extra Room Included");
			List<object> list7 = new List<object>();
			list7.Add("Bar Light 1");
			list7.Add("Bar Light 2");
			list7.Add("Bar Light 3");
			List<object> list8 = new List<object>();
			list8.Add("No Poker/Blackjack Dealer");
			list8.Add("Poker/Blackjack Dealer Included");
			List<object> list9 = new List<object>();
			list9.Add("Retro Arcade");
			list9.Add("Modern Arcade");
			List<object> list10 = new List<object>();
			list10.Add("Timeless");
			list10.Add("Vibrent");
			list10.Add("Sharp");
			PenthouseArt = new NativeMenu("", "Penthouse Options", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), "shopui_title_casino", "shopui_title_casino"));
			PenthouseArtPlacement.BusinessMain_ObjectPool.Add(PenthouseArt);
			GLemonUIMenusNativeMenus.Add(PenthouseArt);
			submenu14s = new NativeMenu("", "Art Options", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), "shopui_title_casino", "shopui_title_casino"));
			PenthouseArtPlacement.BusinessMain_ObjectPool.Add(submenu14s);
			PenthouseArt.AddSubMenu(submenu14s);
			GLemonUIMenusNativeMenus.Add(submenu14s);
			submenu13s = new NativeMenu("", "Sculpture Options", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), "shopui_title_casino", "shopui_title_casino"));
			PenthouseArtPlacement.BusinessMain_ObjectPool.Add(submenu13s);
			submenu14s.AddSubMenu(submenu13s);
			GLemonUIMenusNativeMenus.Add(submenu13s);
			GLemonUIMenusNativeMenus.Add(submenu13s);
			submenu16s = new NativeMenu("", "Party Options", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), "shopui_title_casino", "shopui_title_casino"));
			BusinessMain_ObjectPool.Add(submenu16s);
			PenthouseArt.AddSubMenu(submenu16s);
			GLemonUIMenusNativeMenus.Add(submenu16s);
			PenthouseArtPlacement.BusinessMain_ObjectPool.Add(submenu16s);
			StartP = new NativeItem("Start Party : $20,000");
			submenu16s.Add(StartP);
			StopP = new NativeItem("~m~Stop Party~w~");
			submenu16s.Add(StopP);
			StartP.Activated += delegate
			{
				if (!PenthouseParty.PartyOn && ManagePlayerFunds.AreFundsHigher(20000))
				{
					PenthouseParty.Party_GetDateRelative();
					PenthouseParty.SpawnPartyPeds();
					ManagePlayerFunds.RemoveFunds(20000);
					StartP.Title = "~m~Start Party : $20,000";
					StopP.Title = "Stop Party~w~";
					GTA.UI.Screen.FadeOut(2500);
					Script.Wait(3500);
					PenthouseParty.PartyOn = true;
					Script.Wait(3500);
					GTA.UI.Screen.FadeIn(2500);
					Notification.Show(Class1.GetHostName() + " Party Will end in 24 hours!");
				}
			};
			StopP.Activated += delegate
			{
				if (PenthouseParty.PartyOn)
				{
					StartP.Title = "Start Party : $20,000";
					StopP.Title = "~m~Stop Party~w~";
					GTA.UI.Screen.FadeOut(2500);
					Script.Wait(3500);
					PenthouseParty.PartyOn = false;
					Script.Wait(3500);
					GTA.UI.Screen.FadeIn(2500);
				}
			};
			NativeListItem<dynamic> s = new NativeListItem<object>("Sculpture : ", PenthouseArtPlacement.SculptProp.ToArray());
			submenu13s.Add(s);
			NativeListItem<dynamic> loc2 = new NativeListItem<object>("Location : ", PenthouseArtPlacement.SculptLoc.ToArray());
			submenu13s.Add(loc2);
			NativeItem item2 = new NativeItem("Rotate Object press Q/E");
			submenu13s.Add(item2);
			NativeItem nativeItem = new NativeItem("Place Scultpture : $100,000");
			submenu13s.Add(nativeItem);
			NativeItem nativeItem2 = new NativeItem("Remove All Scultpture ");
			submenu13s.Add(nativeItem2);
			nativeItem2.Description = "This May cause the game to freeze for a second, please be patient";
			nativeItem.Description = "Sculpture positions are Marked in Red";
			nativeItem2.Activated += delegate
			{
				foreach (Prop artProp in PenthouseArtPlacement.ArtProps)
				{
					if (artProp != null)
					{
						artProp.Delete();
					}
				}
				for (int i = 0; i < PenthouseArtPlacement.SculptLoc.Count; i++)
				{
					try
					{
						Class1.Config.SetValue("Art", "SculptPropName_" + i, "null");
						Class1.Config.SetValue("Art", "SculptPropRotationZ_" + i, 0);
						Class1.Config.Save();
					}
					catch
					{
						Notification.Show("(~b~1~w~) Error, Found New Item " + i + ",(~b~2~w~)Repairing Ini file, this may take some time");
						Class1.Config.SetValue("Art", "SculptPropName_" + i, "null");
						Class1.Config.SetValue("Art", "SculptPropRotationZ_" + i, 0);
						Class1.Config.Save();
					}
					PenthouseInterior.RefreshPenthouse();
					Notification.Show(Class1.GetHostName() + " : All Sculptures Removed");
				}
			};
			nativeItem.Activated += delegate
			{
				if (ManagePlayerFunds.AreFundsHigher(100000))
				{
					PenthouseArtPlacement.RemoveArt = true;
					ManagePlayerFunds.RemoveFunds(100000);
					Class1.Config.SetValue<string>("Art", "SculptPropName_" + loc2.SelectedIndex, PenthouseArtPlacement.SculptProp[s.SelectedIndex]);
					Class1.Config.SetValue("Art", "SculptPropRotationZ_" + loc2.SelectedIndex, (int)PenthouseArtPlacement.CurrentArtProp.Rotation.Z);
					Class1.Config.Save();
					PenthouseArtPlacement.RemoveArt = true;
					PenthouseInterior.RefreshPenthouse();
				}
				else
				{
					Notification.Show("you do not have enough for this painting");
				}
			};
			submenu12s = new NativeMenu("", "Painting Options", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), "shopui_title_casino", "shopui_title_casino"));
			PenthouseArtPlacement.BusinessMain_ObjectPool.Add(submenu12s);
			submenu14s.AddSubMenu(submenu12s);
			GLemonUIMenusNativeMenus.Add(submenu12s);
			GLemonUIMenusNativeMenus.Add(submenu12s);
			NativeItem item3 = new NativeItem("-----------------------Paintings--------------------------");
			submenu12s.Add(item3);
			NativeListItem<dynamic> p = new NativeListItem<object>("Painting : ", PenthouseArtPlacement.PaintProp.ToArray());
			submenu12s.Add(p);
			NativeListItem<dynamic> loc = new NativeListItem<object>("Location : ", PenthouseArtPlacement.PaintingLocations.ToArray());
			submenu12s.Add(loc);
			NativeItem nativeItem3 = new NativeItem("Put up Artwork : $1,000");
			submenu12s.Add(nativeItem3);
			NativeItem nativeItem4 = new NativeItem("Remove All Paintings ");
			submenu12s.Add(nativeItem4);
			nativeItem4.Description = "This May cause the game to freeze for a second, please be patient";
			nativeItem3.Description = "Painting positions are Marked in Green";
			loc2.ItemChanged += delegate
			{
				if (PenthouseArtPlacement.CurrentArtProp != null)
				{
					PenthouseArtPlacement.CurrentArtProp.Delete();
				}
				PenthouseArtPlacement.CurrentArtProp = GlobalHelper.CreateProp(Class1.RequestModel(PenthouseArtPlacement.SculptProp[s.SelectedIndex]), PenthouseArtPlacement.SculptLoc[loc2.SelectedIndex], true, false);
				PenthouseArtPlacement.CurrentArtProp.AddBlip();
				PenthouseArtPlacement.CurrentArtProp.AttachedBlip.Sprite = BlipSprite.Standard;
				PenthouseArtPlacement.CurrentArtProp.AttachedBlip.Color = BlipColor.White;
				PenthouseArtPlacement.CurrentArtProp.AttachedBlip.Scale = 0.75f;
				PenthouseArtPlacement.ArtPropCam.Position = PenthouseArtPlacement.CurrentArtProp.GetOffsetPosition(new Vector3(0f, -0.8f, 0.8f));
				PenthouseArtPlacement.ArtPropCam.PointAt(PenthouseArtPlacement.CurrentArtProp.Position);
				PenthouseArtPlacement.CurrentArtProp.IsPositionFrozen = true;
				Notification.Show("Painting ID " + (loc.SelectedIndex + 1));
			};
			loc.ItemChanged += delegate
			{
				if (PenthouseArtPlacement.CurrentArtProp != null)
				{
					PenthouseArtPlacement.CurrentArtProp.Delete();
				}
				Vector3 vector2 = new Vector3(PenthouseArtPlacement.PaintingLocations[loc.SelectedIndex].X, PenthouseArtPlacement.PaintingLocations[loc.SelectedIndex].Y, PenthouseArtPlacement.PaintingLocations[loc.SelectedIndex].Z);
				PenthouseArtPlacement.CurrentArtProp = GlobalHelper.CreateProp(Class1.RequestModel(PenthouseArtPlacement.PaintProp[p.SelectedIndex]), vector2, new Vector3(0, 0, PenthouseArtPlacement.PaintingLocations[loc.SelectedIndex].W), true, false);
				PenthouseArtPlacement.CurrentArtProp.Position = new Vector3(vector2.X, vector2.Y, vector2.Z - PenthouseArtPlacement.GetDimensions(PenthouseArtPlacement.CurrentArtProp).Z / 5.5f + 0.25f);
				PenthouseArtPlacement.CurrentArtProp.AddBlip();
				PenthouseArtPlacement.CurrentArtProp.AttachedBlip.Sprite = BlipSprite.Standard;
				PenthouseArtPlacement.CurrentArtProp.AttachedBlip.Color = BlipColor.White;
				PenthouseArtPlacement.CurrentArtProp.AttachedBlip.Scale = 0.75f;
				PenthouseArtPlacement.ArtPropCam.Position = PenthouseArtPlacement.CurrentArtProp.GetOffsetPosition(new Vector3(0f, -1f, 0f));
				PenthouseArtPlacement.ArtPropCam.PointAt(PenthouseArtPlacement.CurrentArtProp.Position);
				PenthouseArtPlacement.CurrentArtProp.IsPositionFrozen = true;
				Notification.Show("Painting ID " + (loc.SelectedIndex + 1));
			};
			s.ItemChanged += delegate
			{
				if (PenthouseArtPlacement.CurrentArtProp != null)
				{
					PenthouseArtPlacement.CurrentArtProp.Delete();
				}
				PenthouseArtPlacement.CurrentArtProp = GlobalHelper.CreateProp(Class1.RequestModel(PenthouseArtPlacement.SculptProp[s.SelectedIndex]), PenthouseArtPlacement.SculptLoc[loc2.SelectedIndex], true, false);
				PenthouseArtPlacement.CurrentArtProp.AddBlip();
				PenthouseArtPlacement.CurrentArtProp.AttachedBlip.Sprite = BlipSprite.Standard;
				PenthouseArtPlacement.CurrentArtProp.AttachedBlip.Color = BlipColor.White;
				PenthouseArtPlacement.CurrentArtProp.AttachedBlip.Scale = 0.75f;
				PenthouseArtPlacement.CurrentArtProp.IsPositionFrozen = true;
				Notification.Show("Painting ID " + (loc.SelectedIndex + 1));
				PenthouseArtPlacement.ArtPropCam.Position = PenthouseArtPlacement.CurrentArtProp.GetOffsetPosition(new Vector3(0f, -0.8f, 0.8f));
				PenthouseArtPlacement.ArtPropCam.PointAt(PenthouseArtPlacement.CurrentArtProp.Position);
			};
			p.ItemChanged += delegate
			{
				if (PenthouseArtPlacement.CurrentArtProp != null)
				{
					PenthouseArtPlacement.CurrentArtProp.Delete();
				}
				Vector3 vector = new Vector3(PenthouseArtPlacement.PaintingLocations[loc.SelectedIndex].X, PenthouseArtPlacement.PaintingLocations[loc.SelectedIndex].Y, PenthouseArtPlacement.PaintingLocations[loc.SelectedIndex].Z);
				PenthouseArtPlacement.CurrentArtProp = GlobalHelper.CreateProp(Class1.RequestModel(PenthouseArtPlacement.PaintProp[p.SelectedIndex]), vector, new Vector3(0, 0, PenthouseArtPlacement.PaintingLocations[loc.SelectedIndex].W), true, false);
				PenthouseArtPlacement.CurrentArtProp.Position = new Vector3(vector.X, vector.Y, vector.Z - PenthouseArtPlacement.GetDimensions(PenthouseArtPlacement.CurrentArtProp).Z / 5.5f + 0.25f);
				PenthouseArtPlacement.CurrentArtProp.AddBlip();
				PenthouseArtPlacement.CurrentArtProp.AttachedBlip.Sprite = BlipSprite.Standard;
				PenthouseArtPlacement.CurrentArtProp.AttachedBlip.Color = BlipColor.White;
				PenthouseArtPlacement.CurrentArtProp.AttachedBlip.Scale = 0.75f;
				PenthouseArtPlacement.ArtPropCam.Position = PenthouseArtPlacement.CurrentArtProp.GetOffsetPosition(new Vector3(0f, -1f, 0f));
				PenthouseArtPlacement.ArtPropCam.PointAt(PenthouseArtPlacement.CurrentArtProp.Position);
				PenthouseArtPlacement.CurrentArtProp.IsPositionFrozen = true;
				Notification.Show("Painting ID " + (loc.SelectedIndex + 1));
			};
			nativeItem4.Activated += delegate
			{
				foreach (Prop artProp2 in PenthouseArtPlacement.ArtProps)
				{
					if (artProp2 != null)
					{
						artProp2.Delete();
					}
				}
				PenthouseArtPlacement.ArtPropName_1 = "null";
				Class1.Config.SetValue("Art", "ArtPropName_1", PenthouseArtPlacement.ArtPropName_1);
				Class1.Config.Save();
				PenthouseArtPlacement.ArtPropName_2 = "null";
				Class1.Config.SetValue("Art", "ArtPropName_2", PenthouseArtPlacement.ArtPropName_2);
				Class1.Config.Save();
				PenthouseArtPlacement.ArtPropName_3 = "null";
				Class1.Config.SetValue("Art", "ArtPropName_3", PenthouseArtPlacement.ArtPropName_3);
				Class1.Config.Save();
				PenthouseArtPlacement.ArtPropName_4 = "null";
				Class1.Config.SetValue("Art", "ArtPropName_4", PenthouseArtPlacement.ArtPropName_4);
				Class1.Config.Save();
				PenthouseArtPlacement.ArtPropName_5 = "null";
				Class1.Config.SetValue("Art", "ArtPropName_5", PenthouseArtPlacement.ArtPropName_5);
				Class1.Config.Save();
				PenthouseArtPlacement.ArtPropName_6 = "null";
				Class1.Config.SetValue("Art", "ArtPropName_6", PenthouseArtPlacement.ArtPropName_6);
				Class1.Config.Save();
				PenthouseArtPlacement.ArtPropName_7 = "null";
				Class1.Config.SetValue("Art", "ArtPropName_7", PenthouseArtPlacement.ArtPropName_7);
				Class1.Config.Save();
				PenthouseArtPlacement.ArtPropName_8 = "null";
				Class1.Config.SetValue("Art", "ArtPropName_8", PenthouseArtPlacement.ArtPropName_8);
				Class1.Config.Save();
				PenthouseArtPlacement.ArtPropName_9 = "null";
				Class1.Config.SetValue("Art", "ArtPropName_9", PenthouseArtPlacement.ArtPropName_9);
				Class1.Config.Save();
				PenthouseArtPlacement.ArtPropName_10 = "null";
				Class1.Config.SetValue("Art", "ArtPropName_10", PenthouseArtPlacement.ArtPropName_10);
				Class1.Config.Save();
				Notification.Show(Class1.GetHostName() + " : removed all Paintings");
				PenthouseInterior.RefreshPenthouse();
			};
			nativeItem3.Activated += delegate
			{
				if (ManagePlayerFunds.AreFundsHigher(1000))
				{
					PenthouseArtPlacement.RemoveArt = true;
					PenthouseArtPlacement.ArtPropName_1 = PenthouseArtPlacement.PaintProp[p.SelectedIndex];
					Class1.Config.SetValue("Art", "ArtPropName_" + (loc.SelectedIndex + 1), PenthouseArtPlacement.ArtPropName_1);
					Class1.Config.Save();
					ManagePlayerFunds.RemoveFunds(1000);
					PenthouseArtPlacement.ArtProps.Add(PenthouseArtPlacement.CurrentArtProp);
					PenthouseArtPlacement.RemoveArt = true;
					PenthouseInterior.RefreshPenthouse();
					if (PenthouseArtPlacement.CurrentArtProp.AttachedBlip != null)
					{
						PenthouseArtPlacement.CurrentArtProp.AttachedBlip.Delete();
					}
					PenthouseArtPlacement.CurrentArtProp = null;
				}
				else
				{
					Notification.Show("you do not have enough for this painting");
				}
			};
			submenu0 = new NativeMenu("", "Purchase Penthouse", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), "shopui_title_casino", "shopui_title_casino"));
			BusinessMain_ObjectPool.Add(submenu0);
			BusinessMain_Menu.AddSubMenu(submenu0);
			GLemonUIMenusNativeMenus.Add(submenu0);
			GLemonUIMenusNativeMenus.Add(submenu0);
			GLemonUIMenusNativeMenus.Add(submenu0);
			List<NativeListItem<object>> list11 = new List<NativeListItem<object>>();
			NativeListItem<dynamic> S = new NativeListItem<object>("Style : ", list.ToArray());
			submenu0.Add(S);
			list11.Add(S);
			NativeListItem<dynamic> B = new NativeListItem<object>("Bar : ", list2.ToArray());
			submenu0.Add(B);
			list11.Add(B);
			NativeListItem<dynamic> Sp = new NativeListItem<object>("Spa : ", list3.ToArray());
			submenu0.Add(Sp);
			list11.Add(Sp);
			NativeListItem<dynamic> O = new NativeListItem<object>("Office : ", list4.ToArray());
			submenu0.Add(O);
			list11.Add(O);
			NativeListItem<dynamic> M = new NativeListItem<object>("Media Room  : ", list5.ToArray());
			submenu0.Add(M);
			list11.Add(M);
			NativeListItem<dynamic> EB = new NativeListItem<object>("Extra Bedroom : ", list6.ToArray());
			submenu0.Add(EB);
			list11.Add(EB);
			NativeListItem<dynamic> BL = new NativeListItem<object>("Bar Light : ", list7.ToArray());
			submenu0.Add(BL);
			list11.Add(BL);
			NativeListItem<dynamic> PD = new NativeListItem<object>("Poker Dealer: ", list8.ToArray());
			submenu0.Add(PD);
			list11.Add(PD);
			NativeListItem<dynamic> A = new NativeListItem<object>("Arcade : ", list9.ToArray());
			submenu0.Add(A);
			list11.Add(A);
			NativeListItem<dynamic> C = new NativeListItem<object>("Colour : ", list10.ToArray());
			submenu0.Add(C);
			list11.Add(C);
			NativeItem Buy = new NativeItem("Buy Penthouse");
			submenu0.Add(Buy);
			foreach (NativeListItem<object> item4 in list11)
			{
				item4.ItemChanged += delegate
				{
					if (B.SelectedIndex == 1)
					{
						BarCost = 700000f;
					}
					else
					{
						BarCost = 0f;
					}
					if (Sp.SelectedIndex == 1)
					{
						SpaCost = 600000f;
					}
					else
					{
						SpaCost = 0f;
					}
					if (EB.SelectedIndex == 1)
					{
						ExtraB_Cost = 200000f;
					}
					else
					{
						ExtraB_Cost = 0f;
					}
					if (PD.SelectedIndex == 1)
					{
						PDcost = 1065000f;
					}
					else
					{
						PDcost = 0f;
					}
					if (M.SelectedIndex == 1)
					{
						MediaCost = 200000f;
					}
					else
					{
						MediaCost = 0f;
					}
					if (O.SelectedIndex == 1)
					{
						OfficeCost = 200000f;
					}
					else
					{
						OfficeCost = 0f;
					}
					if (S.SelectedIndex >= 1)
					{
						StyleCost = 240000 * (int)(1.35 * (double)S.SelectedIndex);
					}
					else
					{
						StyleCost = 0f;
					}
					Cost = InitialCost + BarCost + SpaCost + ExtraB_Cost + PDcost + MediaCost + StyleCost;
					Buy.AltTitle = Cost.ToString("N");
					PenthouseInterior.DeletePentInterior();
					PenthouseInterior.ActiveInterior(S.SelectedIndex, Sp.SelectedIndex, M.SelectedIndex, PD.SelectedIndex, A.SelectedIndex, EB.SelectedIndex, B.SelectedIndex, C.SelectedIndex + 1, O.SelectedIndex, BL.SelectedIndex);
					PenthouseInterior.ActiveInt = true;
				};
			}
			Buy.Activated += delegate
			{
				if ((float)Game.Player.Character.Money <= Cost)
				{
					ManagePlayerFunds.RemoveFunds((int)Cost);
					Class1.Purchased_Penthouse = 1;
					Class1.Config.SetValue("Penthouse", "Purchased_Penthouse", Class1.Purchased_Penthouse);
					PenthouseInterior.Penthouse_Style = S.SelectedIndex;
					Class1.Config.SetValue("Penthouse", "Penthouse_Style", PenthouseInterior.Penthouse_Style);
					PenthouseInterior.Penthouse_Media = M.SelectedIndex;
					Class1.Config.SetValue("Penthouse", "Penthouse_Media", PenthouseInterior.Penthouse_Media);
					PenthouseInterior.Penthouse_Spa = Sp.SelectedIndex;
					Class1.Config.SetValue("Penthouse", "Penthouse_Spa", PenthouseInterior.Penthouse_Spa);
					PenthouseInterior.Penthouse_Bar = B.SelectedIndex;
					Class1.Config.SetValue("Penthouse", "Penthouse_Bar", PenthouseInterior.Penthouse_Bar);
					PenthouseInterior.Penthouse_Colour = C.SelectedIndex + 1;
					Class1.Config.SetValue("Penthouse", "Penthouse_Colour", PenthouseInterior.Penthouse_Colour);
					PenthouseInterior.Penthouse_Arcade = A.SelectedIndex;
					Class1.Config.SetValue("Penthouse", "Penthouse_Arcade", PenthouseInterior.Penthouse_Arcade);
					PenthouseInterior.Penthouse_BarLight = BL.SelectedIndex;
					Class1.Config.SetValue("Penthouse", "Penthouse_BarLight", PenthouseInterior.Penthouse_BarLight);
					PenthouseInterior.Penthouse_PokerDealer = PD.SelectedIndex;
					Class1.Config.SetValue("Penthouse", "Penthouse_PokerDealer", PenthouseInterior.Penthouse_PokerDealer);
					PenthouseInterior.Penthouse_ExtraBedroom = EB.SelectedIndex;
					Class1.Config.SetValue("Penthouse", "Penthouse_ExtraBedroom", PenthouseInterior.Penthouse_ExtraBedroom);
					PenthouseInterior.Penthouse_Office = O.SelectedIndex;
					Class1.Config.SetValue("Penthouse", "Penthouse_Office", PenthouseInterior.Penthouse_Office);
					BusinessMain_ObjectPool.HideAll();
					CamActive = false;
					if (ArtPropCam != null)
					{
						Game.Player.Character.IsVisible = true;
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						ArtPropCam.IsActive = false;
						ArtPropCam.Delete();
					}
					if (Class1.Purchased_Penthouse == 1)
					{
						ManageBlips.CreatePenthouseAndGarageBlips();
					}
					Script.Wait(100);
					Class1.Config.Save();
					PenthouseInterior.RefreshPenthouse();
					Notification.Show("Agatha Baker: Congradulations, you now own a penthouse on the roof of the diamond casino, you can take the elevator up to your penthouse or down to your 10 car garage.");
				}
				else
				{
					Notification.Show(Class1.GetHostName() + " : You do not have enough money for this Penthouse");
				}
			};
		}

		public PurchasePenthouse()
		{
			if (_Debug.PurchasePenthouse != -1)
			{
				BusinessMain_ObjectPool = new ObjectPool();
				BusinessMain_Menu = new NativeMenu("", "Purchase Penthouse", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), "shopui_title_casino", "shopui_title_casino"));
				BusinessMain_ObjectPool.Add(BusinessMain_Menu);
				GLemonUIMenusNativeMenus.Add(BusinessMain_Menu);
				PenthouseArtPlacement.BusinessMain_ObjectPool = new ObjectPool();
				PenthouseOptions();
				for (int i = 0; i < GLemonUIMenusNativeMenus.Count(); i++)
				{
					SetBanner(GLemonUIMenusNativeMenus[i]);
				}
				base.Tick += OnTick;
			}
		}

		public static void OnTick(object sender, EventArgs e)
		{
			_Debug.PurchasePenthouse = 0;
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, PurchaseMarker, Class1.GetDistSign.Greater, 5f))
			{
				return;
			}
			_Debug.PurchasePenthouse = 1;
			if (BusinessMain_ObjectPool != null)
			{
				BusinessMain_ObjectPool.Process();
			}
			if (!BusinessMain_ObjectPool.AreAnyVisible && CamActive)
			{
				CamActive = false;
				if (ArtPropCam != null)
				{
					Game.Player.Character.IsVisible = true;
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
					ArtPropCam.IsActive = false;
					ArtPropCam.Delete();
				}
			}
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, PurchaseMarker, Class1.GetDistSign.Less, 1.5f))
			{
				Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Purchase a Penthouse");
				if (Game.IsControlJustPressed(Control.Context) && BusinessMain_Menu != null && !BusinessMain_Menu.Visible)
				{
					BusinessMain_Menu.Visible = !BusinessMain_Menu.Visible;
					if (CamActive)
					{
						CamActive = false;
						if (ArtPropCam != null)
						{
							Game.Player.Character.IsVisible = true;
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
							ArtPropCam.IsActive = false;
							ArtPropCam.Delete();
						}
					}
					CamActive = true;
					ArtPropCam = GlobalHelper.CreateCamera(new Vector3(StyleCamPosH.X, StyleCamPosH.Y, StyleCamPosH.Z), new Vector3(0f, 0f, StyleCamPosH.W), 100f);
					ArtPropCam.IsActive = true;
					ArtPropCam.FarClip = 2000f;
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
					World.RenderingCamera = ArtPropCam;
				}
			}
			if (!CamActive)
			{
				return;
			}
			Function.Call(Hash.DISABLE_INTERIOR, 274689, false);
			Function.Call(Hash.PIN_INTERIOR_IN_MEMORY, 274689);
			if (submenu0 != null)
			{
				if (submenu0.SelectedIndex == 0 && ArtPropCam != null)
				{
					ArtPropCam.Position = new Vector3(StyleCamPosH.X, StyleCamPosH.Y, StyleCamPosH.Z);
					ArtPropCam.Rotation = new Vector3(0f, 0f, StyleCamPosH.W);
				}
				if (submenu0.SelectedIndex == 1 && ArtPropCam != null)
				{
					ArtPropCam.Position = new Vector3(BarCamPosH.X, BarCamPosH.Y, BarCamPosH.Z);
					ArtPropCam.Rotation = new Vector3(0f, 0f, BarCamPosH.W);
				}
				if (submenu0.SelectedIndex == 2 && ArtPropCam != null)
				{
					ArtPropCam.Position = new Vector3(SpaCamPosH.X, SpaCamPosH.Y, SpaCamPosH.Z);
					ArtPropCam.Rotation = new Vector3(0f, 0f, SpaCamPosH.W);
				}
				if (submenu0.SelectedIndex == 3 && ArtPropCam != null)
				{
					ArtPropCam.Position = new Vector3(OfficePosH.X, OfficePosH.Y, OfficePosH.Z);
					ArtPropCam.Rotation = new Vector3(0f, 0f, OfficePosH.W);
				}
				if (submenu0.SelectedIndex == 4 && ArtPropCam != null)
				{
					ArtPropCam.Position = new Vector3(MediaRoomCamPosH.X, MediaRoomCamPosH.Y, MediaRoomCamPosH.Z);
					ArtPropCam.Rotation = new Vector3(0f, 0f, MediaRoomCamPosH.W);
				}
				if (submenu0.SelectedIndex == 5 && ArtPropCam != null)
				{
					ArtPropCam.Position = new Vector3(ExtraBedroomCamPosH.X, ExtraBedroomCamPosH.Y, ExtraBedroomCamPosH.Z);
					ArtPropCam.Rotation = new Vector3(0f, 0f, ExtraBedroomCamPosH.W);
				}
				if (submenu0.SelectedIndex == 6 && ArtPropCam != null)
				{
					ArtPropCam.Position = new Vector3(BarLightCamPosH.X, BarLightCamPosH.Y, BarLightCamPosH.Z);
					ArtPropCam.Rotation = new Vector3(0f, 0f, BarLightCamPosH.W);
				}
				if (submenu0.SelectedIndex == 7 && ArtPropCam != null)
				{
					ArtPropCam.Position = new Vector3(DealersCamPosH.X, DealersCamPosH.Y, DealersCamPosH.Z);
					ArtPropCam.Rotation = new Vector3(0f, 0f, DealersCamPosH.W);
				}
				if (submenu0.SelectedIndex == 8 && ArtPropCam != null)
				{
					ArtPropCam.Position = new Vector3(ArcadeCamPosH.X, ArcadeCamPosH.Y, ArcadeCamPosH.Z);
					ArtPropCam.Rotation = new Vector3(0f, 0f, ArcadeCamPosH.W);
				}
				if (submenu0.SelectedIndex == 9 && ArtPropCam != null)
				{
					ArtPropCam.Position = new Vector3(ColorCamPosH.X, ColorCamPosH.Y, ColorCamPosH.Z);
					ArtPropCam.Rotation = new Vector3(0f, 0f, ColorCamPosH.W);
				}
			}
		}
	}
}
