using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using INMNativeUI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class DebugHelper
	{
		[System.Runtime.CompilerServices.AccessedThroughProperty("DebugMenu")]
		private static UIMenu _DebugMenu;

		public static UIMenu DebugMenu
		{
			[CompilerGenerated]
			get
			{
				return _DebugMenu;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = DebugMenu_OnItemSelect;
				UIMenu debugMenu = _DebugMenu;
				if (debugMenu != null)
				{
					debugMenu.OnItemSelect -= value2;
				}
				_DebugMenu = value;
				debugMenu = _DebugMenu;
				if (debugMenu != null)
				{
					debugMenu.OnItemSelect += value2;
				}
			}
		}

		public static Vector3 GetRaycastCoords(this Camera cam)
		{
			return World.Raycast(cam.Position, cam.Direction, 1000f, IntersectFlags.Everything, Game.Player.Character).HitPosition;
		}

		private static double DegToRad(double deg)
		{
			return deg * Math.PI / 180.0;
		}

		public static Vector3 RotationToDirection(this Vector3 rot)
		{
			double num = DegToRad(rot.Z);
			double num2 = DegToRad(rot.X);
			double num3 = Math.Abs(Math.Cos(num2));
			return new Vector3((float)((0.0 - Math.Sin(num)) * num3), (float)(Math.Cos(num) * num3), (float)Math.Sin(num2));
		}

		public static void LoadDebugMenu()
		{
			DebugMenu = new UIMenu("", "DEBUG MENU", new Point(0, -107));
			UIMenu debugMenu = DebugMenu;
			debugMenu.SetBannerType(Helper.MenuBanner);
			debugMenu.MouseEdgeEnabled = false;
			Helper.MenuPool.Add(DebugMenu);
			foreach (ApartmentClass apartment in Helper.apartments)
			{
				UIMenuItem item = new UIMenuItem(apartment.FriendlyName())
				{
					Tag = apartment
				};
				debugMenu.AddItem(item);
			}
			debugMenu.AddItem(new UIMenuItem("Clear Selection"));
			debugMenu.AddItem(new UIMenuItem("Debug Camera"));
			debugMenu.RefreshIndex();
			debugMenu = null;
		}

		private static void DebugMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			string text = selectedItem.Text;
			if (Operators.CompareString(text, "Clear Selection", false) != 0)
			{
				if (Operators.CompareString(text, "Debug Camera", false) == 0)
				{
					DebugCamera.Toggle();
					return;
				}
				ApartmentClass apartmentClass = (ApartmentClass)selectedItem.Tag;
				apartmentClass.SetInteriorActive();
				switch (apartmentClass.ApartmentType)
				{
				case eApartmentType.LowEnd:
					Game.Player.Character.Position = apartmentClass.InteriorPos();
					LowEndApartment.Apartment = apartmentClass;
					LowEndApartment.SpawnDoor();
					break;
				case eApartmentType.MediumEnd:
					Game.Player.Character.Position = apartmentClass.InteriorPos();
					MediumEndApartment.Apartment = apartmentClass;
					MediumEndApartment.SpawnDoor();
					break;
				case eApartmentType.None:
				case eApartmentType.IPL:
				case eApartmentType.IPLwoStyle:
					Game.Player.Character.Position = apartmentClass.InteriorPos();
					HighEndApartment.Building = apartmentClass.Building();
					break;
				case eApartmentType.Other:
					Game.Player.Character.Position = Helper.ToVector3(apartmentClass.Building().BuildingInPos);
					break;
				}
			}
			else
			{
				LowEndApartment.Apartment = null;
				MediumEndApartment.Apartment = null;
				HighEndApartment.Building = null;
				TwoCarGarage.Apartment = null;
				SixCarGarage.Apartment = null;
				TenCarGarage.Apartment = null;
			}
		}
	}
}
