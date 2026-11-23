using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using INMNativeUI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class MansionTrophyCases
	{
		private class Display
		{
			public readonly string Choice;

			public readonly Model Asset;

			public Prop Prop;

			public IntPtr Address;

			public bool Requested;

			public long NextAction;

			public Display(string choice)
			{
				Choice = choice;
				Asset = new Model("m25_2_prop_m52_trpy_" + choice);
			}

			public bool OwnsProp()
			{
				if ((object)Prop != null && Address != IntPtr.Zero && Prop.Exists() && Prop.MemoryAddress == Address)
				{
					return Prop.Model.Hash == Asset.Hash;
				}
				return false;
			}

			public void ReleaseModel()
			{
				if (Requested)
				{
					Asset.MarkAsNoLongerNeeded();
					Requested = false;
				}
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__20_002D0
		{
			public Vector3 _0024VB_0024Local_position;

			public _Closure_0024__20_002D0(_Closure_0024__20_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_position = arg0._0024VB_0024Local_position;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(Mansion m)
			{
				if (m.Enabled && m.IplsLoaded && m.NearBy && m.InteriorId != 0 && m.IsOwnedByPlayer())
				{
					return _0024VB_0024Local_position.DistanceToSquared(Helper.ToVector3(CabinetPose(m.Key, 2))) < 1600f;
				}
				return false;
			}
		}

		internal const int SlotCount = 5;

		internal const string EmptyChoice = "none";

		private const int TrophyRoom = 35493493;

		private const string ModelPrefix = "m25_2_prop_m52_trpy_";

		private static readonly string[] keys = new string[31]
		{
			"none", "completionist", "gtav", "careercriminal", "rank100", "rank500", "rank1000", "ninefig", "500mil", "3commaclub",
			"platinum", "heistpro", "mastermind", "vault", "doomsday", "diamondcasino", "cayoperico", "spotlight", "wellliked", "entrepreneur",
			"carcollector", "arenawarlegend", "arcademaster", "collector", "flightschool", "adversary", "100races", "worldrecord", "lscm", "story",
			"10yo"
		};

		private static readonly string[] titles = new string[31]
		{
			"Empty", "Completionist", "GTA V", "Career Criminal", "Rank 100", "Rank 500", "Rank 1000", "Nine Figures", "$500 Million", "Three Comma Club",
			"Platinum", "Heist Pro", "Mastermind", "Vault", "Doomsday", "Diamond Casino", "Cayo Perico", "Spotlight", "Well Liked", "Entrepreneur",
			"Car Collector", "Arena War Legend", "Arcade Master", "Collector", "Flying Ace", "Adversary", "100 Races", "World Record", "LS Car Meet", "Story",
			"10 Years"
		};

		private static readonly Dictionary<string, Quaternion[]> cases = new Dictionary<string, Quaternion[]>
		{
			{
				"MANSION_VINEWOOD",
				new Quaternion[5]
				{
					new Quaternion(537.828f, 735.003f, 198.728f, 128.588f),
					new Quaternion(537.205f, 735.784f, 198.728f, 128.588f),
					new Quaternion(536.576f, 736.573f, 198.728f, 128.588f),
					new Quaternion(535.954f, 737.354f, 198.728f, 128.588f),
					new Quaternion(535.325f, 738.143f, 198.728f, 128.588f)
				}
			},
			{
				"MANSION_RICHMAN",
				new Quaternion[5]
				{
					new Quaternion(-1653.84f, 472.225f, 125.598f, 198.336f),
					new Quaternion(-1654.79f, 471.914f, 125.598f, 198.336f),
					new Quaternion(-1655.74f, 471.6f, 125.588f, 198.336f),
					new Quaternion(-1656.69f, 471.285f, 125.588f, 198.336f),
					new Quaternion(-1657.65f, 470.99f, 125.588f, 198.336f)
				}
			},
			{
				"MANSION_TONGVA",
				new Quaternion[5]
				{
					new Quaternion(-2595.19f, 1899.1f, 163.732f, 96.124f),
					new Quaternion(-2595.29f, 1900.1f, 163.732f, 96.124f),
					new Quaternion(-2595.39f, 1901.09f, 163.732f, 96.124f),
					new Quaternion(-2595.49f, 1902.09f, 163.732f, 96.124f),
					new Quaternion(-2595.59f, 1903.1f, 163.732f, 96.124f)
				}
			}
		};

		private static readonly Display[] displays = new Display[5];

		private static readonly Stopwatch clock = Stopwatch.StartNew();

		private static Mansion active;

		private static int activeInterior;

		private static long nextTick;

		private static UIMenu menu;

		private static Mansion menuHome;

		internal static string NormalizeChoice(string value)
		{
			string text = (value ?? "").Trim().ToLowerInvariant();
			if (Array.IndexOf(keys, text) < 0)
			{
				return "none";
			}
			return text;
		}

		internal static Quaternion CabinetPose(string key, int slot)
		{
			Quaternion[] value = null;
			if (slot < 0 || slot >= 5 || key == null || !cases.TryGetValue(key, out value))
			{
				return default(Quaternion);
			}
			return value[slot];
		}

		internal static void OpenMenu(Mansion home, UIMenu parent)
		{
			checked
			{
				if (home != null && home.Enabled && home.IsOwnedByPlayer())
				{
					if (menu == null)
					{
						menu = new UIMenu("", "TROPHY CABINETS", new Point(0, -107));
						menu.SetBannerType(Helper.MenuBanner);
						menu.MouseEdgeEnabled = false;
						menu.OnItemSelect += SelectTrophy;
						Helper.MenuPool.Add(menu);
					}
					menuHome = home;
					menu.MenuItems.Clear();
					List<object> list = new List<object> { titles[0] };
					int num = keys.Length - 1;
					for (int i = 1; i <= num; i++)
					{
						string localizedString = Game.GetLocalizedString("MDM_MT_O" + Conversions.ToString(i));
						list.Add((string.IsNullOrWhiteSpace(localizedString) || Operators.CompareString(localizedString, "NULL", false) == 0) ? titles[i] : localizedString);
					}
					int num2 = 0;
					do
					{
						menu.AddItem(new UIMenuListItem($"Cabinet {num2 + 1}", list, Array.IndexOf(keys, NormalizeChoice(home.TrophyChoices[num2])), "Use the arrows to choose a trophy. Press Select to place it, or choose Empty to clear the cabinet.")
						{
							Tag = num2
						});
						num2++;
					}
					while (num2 <= 4);
					menu.ParentMenu = parent;
					menu.RefreshIndex();
					parent.Visible = false;
					menu.Visible = true;
				}
			}
		}

		private static void SelectTrophy(UIMenu sender, UIMenuItem selected, int index)
		{
			UIMenuListItem uIMenuListItem = selected as UIMenuListItem;
			Mansion mansion = menuHome;
			if (uIMenuListItem == null || mansion == null || !sender.Visible || StoryLoadGuard.WorldLoading() || !mansion.Enabled || !mansion.IsOwnedByPlayer() || !(uIMenuListItem.Tag is int))
			{
				return;
			}
			int num = Conversions.ToInteger(uIMenuListItem.Tag);
			if (num < 0 || num >= 5 || uIMenuListItem.Index < 0 || uIMenuListItem.Index >= keys.Length)
			{
				return;
			}
			string text = NormalizeChoice(mansion.TrophyChoices[num]);
			string text2 = keys[uIMenuListItem.Index];
			if (Operators.CompareString(text, text2, false) == 0)
			{
				return;
			}
			if (Operators.CompareString(text2, "none", false) != 0)
			{
				Model model = new Model("m25_2_prop_m52_trpy_" + text2);
				if (!model.IsValid || !model.IsInCdImage)
				{
					uIMenuListItem.Index = Array.IndexOf(keys, text);
					Notification.Show("This trophy is unavailable in the installed game.");
					return;
				}
			}
			mansion.TrophyChoices[num] = text2;
			if (!Mansions.SaveMansions())
			{
				mansion.TrophyChoices[num] = text;
				uIMenuListItem.Index = Array.IndexOf(keys, text);
			}
			else
			{
				nextTick = 0L;
			}
		}

		internal static void Tick()
		{
			long elapsedMilliseconds = clock.ElapsedMilliseconds;
			if (elapsedMilliseconds < nextTick)
			{
				return;
			}
			checked
			{
				nextTick = elapsedMilliseconds + 250;
				try
				{
					_Closure_0024__20_002D0 arg = new _Closure_0024__20_002D0(null);
					if ((object)Helper.PP == null || !Helper.PP.Exists())
					{
						Cleanup();
						return;
					}
					arg._0024VB_0024Local_position = Helper.PP.Position;
					Mansion mansion = Mansions.MansionList.Find(arg._Lambda_0024__0);
					if (mansion != null && Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, Helper.PP.Handle) != mansion.InteriorId)
					{
						mansion = null;
					}
					if (mansion != active || (mansion != null && mansion.InteriorId != activeInterior))
					{
						Cleanup();
						active = mansion;
						activeInterior = mansion?.InteriorId ?? 0;
					}
					if (mansion != null)
					{
						int num = 0;
						do
						{
							MaintainSlot(mansion, num, elapsedMilliseconds);
							num++;
						}
						while (num <= 4);
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Mansion trophy cabinets: " + ex2.Message);
					nextTick = elapsedMilliseconds + 2000;
					ProjectData.ClearProjectError();
				}
			}
		}

		private static void MaintainSlot(Mansion home, int slot, long now)
		{
			string text = NormalizeChoice(home.TrophyChoices[slot]);
			Display display = displays[slot];
			if (display != null && Operators.CompareString(display.Choice, text, false) != 0)
			{
				ReleaseSlot(slot);
				display = null;
			}
			if (Operators.CompareString(text, "none", false) == 0 || !home.SetsApplied)
			{
				return;
			}
			if (display == null)
			{
				display = new Display(text);
				displays[slot] = display;
			}
			if (display.OwnsProp() || !display.Asset.IsValid || !display.Asset.IsInCdImage)
			{
				return;
			}
			checked
			{
				if (!display.Asset.IsLoaded)
				{
					if (now >= display.NextAction)
					{
						if (display.Requested)
						{
							display.ReleaseModel();
							display.NextAction = now + 3000;
						}
						else
						{
							display.Asset.Request();
							display.Requested = true;
							display.NextAction = now + 8000;
						}
					}
					return;
				}
				Quaternion q = CabinetPose(home.Key, slot);
				Vector3 position = Helper.ToVector3(q);
				if (!display.Requested && now < display.NextAction)
				{
					return;
				}
				display.NextAction = now + 2000;
				try
				{
					Prop prop = World.CreateProp(display.Asset, position, false, false);
					if ((object)prop != null && prop.Exists())
					{
						display.Prop = prop;
						display.Address = prop.MemoryAddress;
						if (display.Address == IntPtr.Zero)
						{
							prop.Delete();
							return;
						}
						prop.IsVisible = false;
						prop.IsPersistent = true;
						prop.IsPositionFrozen = true;
						Function.Call(Hash.SET_ENTITY_COLLISION, prop.Handle, false, false);
						Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, prop.Handle, position.X, position.Y, position.Z, false, false, false);
						prop.Heading = q.W;
						Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, prop.Handle, home.InteriorId, 35493493);
						prop.IsVisible = true;
					}
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					ReleaseSlot(slot);
					throw;
				}
				finally
				{
					display.ReleaseModel();
				}
			}
		}

		private static void ReleaseSlot(int slot)
		{
			Display display = displays[slot];
			if (display == null)
			{
				return;
			}
			displays[slot] = null;
			try
			{
				try
				{
					if (display.OwnsProp())
					{
						display.Prop.Delete();
					}
				}
				finally
				{
					display.ReleaseModel();
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Mansion trophy cabinet {checked(slot + 1)} cleanup: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
		}

		internal static void Cleanup(Mansion home = null)
		{
			if (home == null || active == home)
			{
				int num = 0;
				do
				{
					ReleaseSlot(num);
					num = checked(num + 1);
				}
				while (num <= 4);
				active = null;
				activeInterior = 0;
			}
		}

		internal static void ForgetForStoryLoad()
		{
			Array.Clear(displays, 0, displays.Length);
			active = null;
			activeInterior = 0;
			nextTick = 0L;
			menuHome = null;
		}
	}
}
