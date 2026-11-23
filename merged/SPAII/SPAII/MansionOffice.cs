using System;
using System.Drawing;
using GTA;
using GTA.Math;
using GTA.Native;
using INMNativeUI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class MansionOffice
	{
		private static readonly string[] computerModels = new string[3] { "m25_2_int_01_off_pc_screen_01", "m25_2_prop_m52_monitor_01a", "m25_2_prop_m52_keyboard_01a" };

		private static UIMenu menu;

		private static Mansion home;

		private static Mansion cachedHome;

		private static Vector3 cachedPosition;

		private static bool cachedFound;

		private static int nextScan;

		public static Vector3 OfficePoint(Mansion m)
		{
			return Helper.ToVector3(MansionAutoLayout.MainPoint(m, 35.6069f, -5.86093f, 0.612442f));
		}

		public static bool IsNear(Mansion m, Vector3 position)
		{
			if (m == null || !m.IplsLoaded || !PartyPlacement.FinitePoint(position))
			{
				return false;
			}
			Vector3 vector = OfficePoint(m);
			Vector3 vector2 = position - vector;
			bool hit = default(bool);
			Vector3 point = default(Vector3);
			Vector3 normal = default(Vector3);
			if (vector2.X * vector2.X + vector2.Y * vector2.Y <= 6.25f && Math.Abs(vector2.Z) < 1.8f && PartyPlacement.PhysicalRay(position, vector, 81, ref hit, ref point, ref normal, Helper.PP) == 2 && (!hit || point.DistanceTo(vector) <= 0.9f))
			{
				return true;
			}
			if (cachedHome != m || Game.GameTime >= nextScan)
			{
				cachedHome = m;
				cachedFound = false;
				nextScan = checked(Game.GameTime + 400);
				string[] array = computerModels;
				foreach (string input in array)
				{
					int num = Function.Call<int>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 2.8f, Game.GenerateHash(input), false, false, false);
					if (num == 0)
					{
						continue;
					}
					Prop prop = Entity.FromHandle(num) as Prop;
					if ((object)prop != null && prop.Exists())
					{
						Vector3 position2 = prop.Position;
						if (Math.Abs(position2.Z - position.Z) < 1.8f)
						{
							cachedPosition = position2;
							cachedFound = true;
							break;
						}
					}
				}
			}
			if (!cachedFound || position.DistanceTo(cachedPosition) > 2.8f || Math.Abs(position.Z - cachedPosition.Z) > 1.8f)
			{
				return false;
			}
			bool hit2 = default(bool);
			Vector3 point2 = default(Vector3);
			Vector3 normal2 = default(Vector3);
			if (PartyPlacement.PhysicalRay(position, cachedPosition, 81, ref hit2, ref point2, ref normal2, Helper.PP) != 2)
			{
				return false;
			}
			return !hit2 || point2.DistanceTo(cachedPosition) <= 0.8f;
		}

		private static bool CanUse(Mansion m)
		{
			if (m != null && m.Enabled && m.IplsLoaded && m.IsOwnedByPlayer() && MansionFeatures.AtMansion(m) && (object)Helper.PP != null && Helper.PP.Exists() && !Helper.PP.IsDead && !Helper.PP.IsInVehicle() && !PartyService.IsPlayerActionActive && !MansionFeatures.IsWorkoutActive)
			{
				return !PartyPlayerActions.IsKissing;
			}
			return false;
		}

		public static void Open(Mansion m, UIMenu parent = null)
		{
			if (CanUse(m))
			{
				if (menu == null)
				{
					menu = new UIMenu("", "MANSION OFFICE TERMINAL", new Point(0, -107));
					menu.SetBannerType(Helper.MenuBanner);
					menu.MouseEdgeEnabled = false;
					Helper.MenuPool.Add(menu);
					menu.OnItemSelect += SelectItem;
				}
				home = m;
				menu.Clear();
				menu.ParentMenu = parent;
				if (parent != null)
				{
					parent.Visible = false;
				}
				menu.Subtitle.Caption = "OFFICE - " + m.Name.ToUpperInvariant();
				int balance = default(int);
				UIMenuItem item = new UIMenuItem("Account: " + Helper.GetPlayer(), Helper.TryGetOwnerMoney(Helper.GetPlayer(), ref balance) ? ("$" + balance.ToString("N0")) : "Balance unavailable")
				{
					Enabled = false
				};
				menu.AddItem(item);
				menu.AddItem(new UIMenuItem("Property management", "Change this mansion's style, assistant, decorations and other settings.")
				{
					Tag = "Manage"
				});
				menu.AddItem(new UIMenuItem("Mansion services", "Open your assistant for chauffeur, helicopter, parties and other services.")
				{
					Tag = "Services"
				});
				menu.AddItem(new UIMenuItem("Vehicle catalogue / 3D preview", "Browse and buy vehicles through your mansion motor gallery.")
				{
					Tag = "Cars"
				});
				menu.AddItem(new UIMenuItem("Vehicle workshop", "Travel to your mansion workshop and choose a stored vehicle.")
				{
					Tag = "Workshop"
				});
				menu.AddItem(new UIMenuItem("Pets", "Call and care for your cat or dog.")
				{
					Tag = "Pets"
				});
				menu.RefreshIndex();
				menu.Visible = true;
			}
		}

		private static void SelectItem(UIMenu sender, UIMenuItem item, int index)
		{
			if (CanUse(home))
			{
				menu.Visible = false;
				switch (item.Tag as string)
				{
				case "Manage":
					Mansions.BuildMansionMenu(home);
					Mansions.MansionMenu.ParentMenu = menu;
					Mansions.MansionMenu.Visible = true;
					break;
				case "Services":
					MansionFeatures.OpenAssistant(home);
					break;
				case "Cars":
					MansionVehicleShop.Open(home, menu);
					break;
				case "Workshop":
					MansionWorkshopTravel.OpenWorkshopHere(home, menu);
					break;
				case "Pets":
					MansionLeisure.OpenPets(home, menu);
					break;
				}
			}
		}

		public static bool Tick(Mansion m)
		{
			if (home != null && !CanUse(home))
			{
				Cleanup();
			}
			if (!CanUse(m) || Helper.MenuPool.IsAnyMenuOpen() || !IsNear(m, Helper.PP.Position))
			{
				return false;
			}
			if (!Helper.ShowPersistentHelp("Press ~INPUT_CONTEXT~ to use the mansion office computer."))
			{
				return false;
			}
			if (Game.IsControlJustReleased(Control.Context))
			{
				Open(m);
			}
			return true;
		}

		public static void Cleanup()
		{
			if (menu != null)
			{
				menu.Visible = false;
			}
			home = null;
			cachedHome = null;
			cachedFound = false;
			nextScan = 0;
		}
	}
}
