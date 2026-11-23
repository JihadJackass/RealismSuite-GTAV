using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
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
	public static class MansionVehicleShop
	{
		public class CatalogEntry
		{
			public string ModelName;

			public int ModelHash;

			public string Make;

			public string Name;

			public int ClassId;

			public string Dlc;

			public int Price;

			public bool OriginalPrice;

			public bool Hsw;

			public bool AddOn;

			public string Title => (Make + " " + Name).Trim();
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<CatalogEntry, string> _0024I54_002D1;

			public static Func<CatalogEntry, int> _0024I54_002D2;

			public static Func<CatalogEntry, bool> _0024I64_002D0;

			public static Func<CatalogEntry, string> _0024I64_002D1;

			public static Func<CatalogEntry, bool> _0024I64_002D2;

			public static Func<CatalogEntry, bool> _0024I64_002D3;

			public static Func<int, object> _0024I66_002D0;

			public static Predicate<UIMenuItem> _0024I66_002D1;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal int _Lambda_0024__0_002D0(object value)
			{
				return checked((int)((Convert.ToInt64(RuntimeHelpers.GetObjectValue(value)) > int.MaxValue) ? (Convert.ToInt64(RuntimeHelpers.GetObjectValue(value)) - 4294967296L) : Convert.ToInt64(RuntimeHelpers.GetObjectValue(value))));
			}

			[SpecialName]
			internal string _Lambda_0024__54_002D1(CatalogEntry e)
			{
				return e.Title;
			}

			[SpecialName]
			internal int _Lambda_0024__54_002D2(CatalogEntry e)
			{
				return e.ModelHash;
			}

			[SpecialName]
			internal bool _Lambda_0024__64_002D0(CatalogEntry e)
			{
				return IsGarageClass(e.ClassId);
			}

			[SpecialName]
			internal string _Lambda_0024__64_002D1(CatalogEntry e)
			{
				return e.Title;
			}

			[SpecialName]
			internal bool _Lambda_0024__64_002D2(CatalogEntry e)
			{
				return e.Hsw;
			}

			[SpecialName]
			internal bool _Lambda_0024__64_002D3(CatalogEntry e)
			{
				return e.AddOn;
			}

			[SpecialName]
			internal object _Lambda_0024__66_002D0(int p)
			{
				return $"{p} / {PageCount(matching.Count)}";
			}

			[SpecialName]
			internal bool _Lambda_0024__66_002D1(UIMenuItem item)
			{
				return item.Tag is CatalogEntry;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__54_002D0
		{
			public int _0024VB_0024Local_filter;

			public string _0024VB_0024Local_term;

			[SpecialName]
			internal bool _Lambda_0024__0(CatalogEntry e)
			{
				if (e != null && IsGarageClass(e.ClassId) && MatchesFilter(e, _0024VB_0024Local_filter))
				{
					if (_0024VB_0024Local_term.Length != 0)
					{
						return (e.Title + " " + e.ModelName).IndexOf(_0024VB_0024Local_term, StringComparison.OrdinalIgnoreCase) >= 0;
					}
					return true;
				}
				return false;
			}
		}

		public const int VehiclesPerPage = 18;

		public const int HswPackagePrice = 500000;

		private static readonly HashSet<int> hswModels = new HashSet<int>
		{
			-1758379524, -1566741232, -1508420500, -1491268273, -1372798934, -1041692462, -1007324375, -999594302, -982130927, -978456382,
			-973016778, -660007725, -277011826, -255678177, 165968051, 372621319, 386089410, 610429990, 802856453, 956849991,
			1133471123, 1349725314, 1447690049, 1549009676, 1549126457, 1550581940, 1591739866, 1596736441, 1748565021, 1881415402,
			1966698497
		};

		private static readonly string[] classNames = new string[23]
		{
			"Compacts", "Sedans", "SUVs", "Coupes", "Muscle", "Sports classics", "Sports", "Super", "Motorcycles", "Off-road",
			"Industrial", "Utility", "Vans", "Cycles", "Boats", "Helicopters", "Planes", "Service", "Emergency", "Military",
			"Commercial", "Trains", "Open wheel"
		};

		private static readonly string[] filterNames = new string[16]
		{
			"All installed road vehicles", "GTA Online / DLC", "HSW capable", "Installed add-ons", "Super", "Sports", "Sports classics", "Muscle", "Coupes", "Sedans",
			"SUVs", "Compacts", "Motorcycles", "Off-road", "Vans", "Other road vehicles"
		};

		private static readonly int[] filterClasses = new int[16]
		{
			-1, -1, -1, -1, 7, 6, 5, 4, 3, 1,
			2, 0, 8, 9, 12, -1
		};

		private static List<CatalogEntry> roster = new List<CatalogEntry>();

		private static List<CatalogEntry> matching = new List<CatalogEntry>();

		private static UIMenu shopMenu;

		private static UIMenu detailMenu;

		private static UIMenuListItem filterItem;

		private static UIMenuListItem pageItem;

		private static UIMenuCheckboxItem hswItem;

		private static UIMenuItem buyItem;

		private static Mansion shopMansion;

		private static eOwner sessionOwner;

		private static int filterIndex;

		private static int pageIndex;

		private static string searchText = "";

		private static CatalogEntry selected;

		private static CatalogEntry pending;

		private static int pendingAt;

		private static Model? loading;

		private static int loadDeadline;

		private static Vehicle preview;

		private static CatalogEntry previewEntry;

		private static Camera camera;

		private static Camera previousCamera;

		private static Vector3 previewOrigin;

		private static Vector3 previewFloor;

		private static Vector3 previewMinimum;

		private static Vector3 previewMaximum;

		private static float shopFloorOffset;

		private static bool returnToOrigin;

		private static Ped sourcePed;

		private static Quaternion sourcePose;

		private static UIMenu sourceParent;

		private static float initialHeading;

		private static int rotationAt;

		private static bool savedFrozen;

		private static Ped frozenPed;

		private static bool opened;

		private static bool rebuilding;

		private static bool busy;

		private static bool upgradeWanted;

		private static string previewError;

		private static readonly HashSet<int> officialSdkModels = new HashSet<int>(Enum.GetValues(typeof(VehicleHash)).Cast<object>().Select(_Closure_0024__._0024I._Lambda_0024__0_002D0));

		public static void ForgetForStoryLoad()
		{
			opened = false;
			rebuilding = false;
			busy = false;
			shopMansion = null;
			sessionOwner = eOwner.Nobody;
			selected = null;
			pending = null;
			pendingAt = 0;
			loadDeadline = 0;
			loading = null;
			preview = null;
			previewEntry = null;
			previewError = null;
			camera = null;
			previousCamera = null;
			frozenPed = null;
			sourcePed = null;
			sourceParent = null;
			returnToOrigin = false;
			savedFrozen = false;
			upgradeWanted = false;
			sourcePose = default(Quaternion);
			previewOrigin = default(Vector3);
			previewFloor = default(Vector3);
			previewMinimum = default(Vector3);
			previewMaximum = default(Vector3);
			initialHeading = 0f;
			rotationAt = 0;
			shopFloorOffset = 0f;
		}

		public static bool IsHswModel(int modelHash)
		{
			return hswModels.Contains(modelHash);
		}

		public static int HswSuspensionIndex(int modelHash)
		{
			switch (modelHash)
			{
			case 1591739866:
				return 1;
			case -1491268273:
			case 956849991:
			case 1549126457:
				return 3;
			case -982130927:
				return 0;
			default:
				return 4;
			}
		}

		public static bool IsGarageClass(int classId)
		{
			if (classId >= 0 && classId <= 22 && classId != 14 && classId != 15 && classId != 16)
			{
				return classId != 21;
			}
			return false;
		}

		public static int PageCount(int itemCount, int pageSize = 18)
		{
			if (pageSize < 1)
			{
				throw new ArgumentOutOfRangeException("pageSize");
			}
			checked
			{
				return (int)Math.Max(1L, unchecked(checked(Math.Max(0L, itemCount) + pageSize - 1) / pageSize));
			}
		}

		public static CatalogEntry ParseCatalogLine(string line)
		{
			if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#", StringComparison.Ordinal))
			{
				return null;
			}
			string[] array = line.Split(new char[1] { '\t' });
			if (array.Length < 9)
			{
				return null;
			}
			int result;
			if (!int.TryParse(array[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out result) || result == 0)
			{
				return null;
			}
			int result2;
			if (!int.TryParse(array[4], NumberStyles.Integer, CultureInfo.InvariantCulture, out result2) || result2 < 0 || result2 > 22)
			{
				return null;
			}
			int result3;
			if (!int.TryParse(array[6], NumberStyles.Integer, CultureInfo.InvariantCulture, out result3) || result3 < 1)
			{
				return null;
			}
			if (string.IsNullOrWhiteSpace(array[0]) || string.IsNullOrWhiteSpace(array[3]))
			{
				return null;
			}
			return new CatalogEntry
			{
				ModelName = array[0],
				ModelHash = result,
				Make = array[2],
				Name = array[3],
				ClassId = result2,
				Dlc = array[5],
				Price = result3,
				OriginalPrice = (Operators.CompareString(array[7], "original", false) == 0),
				Hsw = IsHswModel(result)
			};
		}

		public static List<CatalogEntry> FilterEntries(IEnumerable<CatalogEntry> entries, int filter, string search)
		{
			_Closure_0024__54_002D0 closure_0024__54_002D = new _Closure_0024__54_002D0();
			closure_0024__54_002D._0024VB_0024Local_filter = filter;
			if (entries == null)
			{
				return new List<CatalogEntry>();
			}
			closure_0024__54_002D._0024VB_0024Local_term = (search ?? "").Trim();
			return entries.Where(closure_0024__54_002D._Lambda_0024__0).OrderBy((_Closure_0024__._0024I54_002D1 != null) ? _Closure_0024__._0024I54_002D1 : (_Closure_0024__._0024I54_002D1 = _Closure_0024__._0024I._Lambda_0024__54_002D1), StringComparer.OrdinalIgnoreCase).ThenBy((_Closure_0024__._0024I54_002D2 != null) ? _Closure_0024__._0024I54_002D2 : (_Closure_0024__._0024I54_002D2 = _Closure_0024__._0024I._Lambda_0024__54_002D2))
				.ToList();
		}

		private static bool MatchesFilter(CatalogEntry e, int filter)
		{
			if (filter < 0 || filter >= filterNames.Length)
			{
				filter = 0;
			}
			switch (filter)
			{
			case 0:
				return true;
			case 1:
				return !e.AddOn && !string.Equals(e.Dlc, "TitleUpdate", StringComparison.OrdinalIgnoreCase);
			case 2:
				return e.Hsw;
			case 3:
				return e.AddOn;
			case 15:
				return e.ClassId == 10 || e.ClassId == 11 || e.ClassId == 13 || e.ClassId >= 17;
			default:
				return e.ClassId == filterClasses[filter];
			}
		}

		public static int EffectivePrice(CatalogEntry entry, bool withHsw = false)
		{
			if (entry == null || entry.Price < 1)
			{
				return 0;
			}
			checked
			{
				long num = unchecked((long)entry.Price) + unchecked((long)((withHsw && entry.Hsw) ? 500000 : 0));
				if (num > int.MaxValue)
				{
					return 0;
				}
				return (int)num;
			}
		}

		public static bool FitsMansionBay(Vector3 minimum, Vector3 maximum)
		{
			Vector3 vector = maximum - minimum;
			if (Finite(vector.X) && Finite(vector.Y) && Finite(vector.Z) && vector.X > 0.1f && vector.Y > 0.1f && vector.Z > 0.1f && vector.X <= 3.3f && vector.Y <= 7.5f)
			{
				return vector.Z <= 3.2f;
			}
			return false;
		}

		private static bool Finite(float value)
		{
			if (!float.IsNaN(value))
			{
				return !float.IsInfinity(value);
			}
			return false;
		}

		private static UIMenu Menu(string title)
		{
			UIMenu uIMenu = new UIMenu("", title, new Point(0, -107));
			uIMenu.SetBannerType(Helper.MenuBanner);
			uIMenu.MouseEdgeEnabled = false;
			Helper.MenuPool.Add(uIMenu);
			return uIMenu;
		}

		private static void EnsureMenus()
		{
			if (shopMenu == null)
			{
				shopMenu = Menu("MANSION MOTOR GALLERY");
				detailMenu = Menu("VEHICLE DETAILS");
				detailMenu.ParentMenu = shopMenu;
				shopMenu.OnListChange += ShopListChange;
				shopMenu.OnIndexChange += ShopIndexChange;
				shopMenu.OnItemSelect += ShopSelect;
				detailMenu.OnItemSelect += DetailSelect;
				detailMenu.OnCheckboxChange += DetailCheckbox;
			}
		}

		private static string LocalText(string label, string fallback)
		{
			string text = (string.IsNullOrWhiteSpace(label) ? "" : Game.GetLocalizedString(label));
			if (!string.IsNullOrWhiteSpace(text) && Operators.CompareString(text, "NULL", false) != 0)
			{
				return text;
			}
			return fallback;
		}

		public static bool IsOfficialModel(int modelHash, IEnumerable<int> catalogHashes)
		{
			if (!officialSdkModels.Contains(modelHash))
			{
				return catalogHashes?.Contains(modelHash) ?? false;
			}
			return true;
		}

		private static void LoadRoster()
		{
			if (roster.Count > 0)
			{
				return;
			}
			Dictionary<int, CatalogEntry> dictionary = new Dictionary<int, CatalogEntry>();
			string path = Path.Combine("scripts", "SPA II", "VehicleCatalog.tsv");
			if (File.Exists(path))
			{
				foreach (string item in File.ReadLines(path))
				{
					CatalogEntry catalogEntry = ParseCatalogLine(item);
					if (catalogEntry != null)
					{
						dictionary[catalogEntry.ModelHash] = catalogEntry;
					}
				}
			}
			else
			{
				Logger.Log("Vehicle shop: VehicleCatalog.tsv missing; installed-model fallback prices apply.");
			}
			HashSet<int> catalogHashes = new HashSet<int>(dictionary.Keys);
			HashSet<int> hashSet = new HashSet<int>(Vehicle.GetAllModelValues());
			int[] array = dictionary.Keys.ToArray();
			foreach (int num in array)
			{
				Model model = new Model(num);
				if (!model.IsInCdImage || !model.IsVehicle || model.IsTrailer)
				{
					dictionary.Remove(num);
				}
			}
			foreach (int item2 in hashSet)
			{
				if (dictionary.ContainsKey(item2))
				{
					continue;
				}
				Model model2 = new Model(item2);
				if (!model2.IsInCdImage || !model2.IsVehicle || model2.IsTrailer)
				{
					continue;
				}
				int classId = Function.Call<int>(Hash.GET_VEHICLE_CLASS_FROM_NAME, item2);
				if (IsGarageClass(classId))
				{
					string text = Function.Call<string>(Hash.GET_DISPLAY_NAME_FROM_VEHICLE_MODEL, item2);
					string make = "";
					try
					{
						make = LocalText(Function.Call<string>(Hash.GET_MAKE_NAME_FROM_VEHICLE_MODEL, item2), "");
					}
					catch (Exception projectError)
					{
						ProjectData.SetProjectError(projectError);
						ProjectData.ClearProjectError();
					}
					dictionary[item2] = new CatalogEntry
					{
						ModelHash = item2,
						ModelName = (text ?? ("0x" + item2.ToString("X8"))),
						Make = make,
						Name = LocalText(text, "Vehicle 0x" + item2.ToString("X8")),
						ClassId = classId,
						Dlc = (IsOfficialModel(item2, catalogHashes) ? "Installed game vehicle" : "Installed add-on"),
						Price = 100000,
						OriginalPrice = false,
						AddOn = !IsOfficialModel(item2, catalogHashes),
						Hsw = IsHswModel(item2)
					};
				}
			}
			roster = dictionary.Values.Where((_Closure_0024__._0024I64_002D0 != null) ? _Closure_0024__._0024I64_002D0 : (_Closure_0024__._0024I64_002D0 = _Closure_0024__._0024I._Lambda_0024__64_002D0)).OrderBy((_Closure_0024__._0024I64_002D1 != null) ? _Closure_0024__._0024I64_002D1 : (_Closure_0024__._0024I64_002D1 = _Closure_0024__._0024I._Lambda_0024__64_002D1), StringComparer.OrdinalIgnoreCase).ToList();
			Logger.Log($"Vehicle shop: {roster.Count} installed road models, {roster.Where((_Closure_0024__._0024I64_002D2 != null) ? _Closure_0024__._0024I64_002D2 : (_Closure_0024__._0024I64_002D2 = _Closure_0024__._0024I._Lambda_0024__64_002D2)).Count()} HSW capable, {roster.Where((_Closure_0024__._0024I64_002D3 != null) ? _Closure_0024__._0024I64_002D3 : (_Closure_0024__._0024I64_002D3 = _Closure_0024__._0024I._Lambda_0024__64_002D3)).Count()} add-ons.");
		}

		public static void Open(Mansion m, UIMenu parent = null)
		{
			if (m == null || !m.IsOwnedByPlayer())
			{
				return;
			}
			if (Helper.GetPlayer() != 0 && Helper.GetPlayer() != eOwner.Franklin && Helper.GetPlayer() != eOwner.Trevor)
			{
				Notification.Show("Vehicle purchases require a story character's bank account.");
				return;
			}
			try
			{
				CloseSession(true);
				if (Helper.PP.IsInVehicle())
				{
					Vehicle currentVehicle = Helper.PP.CurrentVehicle;
					Helper.PP.Task.LeaveVehicle(currentVehicle, true);
					int num = checked(Game.GameTime + 3500);
					while (Helper.PP.IsInVehicle() && Game.GameTime < num)
					{
						Script.Wait(25);
					}
					if (Helper.PP.IsInVehicle())
					{
						Notification.Show("Leave your vehicle to enter the motor gallery. Your car has been kept.");
						return;
					}
				}
				bool flag = MansionNativeGarage.IsActive(m);
				Ped pP = Helper.PP;
				Quaternion quaternion = new Quaternion(Helper.PP.Position.X, Helper.PP.Position.Y, Helper.PP.Position.Z, Helper.PP.Heading);
				if (flag || MansionNativeGarage.Enter(m, true))
				{
					shopMansion = m;
					sessionOwner = Helper.GetPlayer();
					opened = true;
					sourcePed = pP;
					sourcePose = quaternion;
					sourceParent = parent;
					returnToOrigin = !flag;
					shopFloorOffset = MansionShopPreviewGeometry.PreviewFloorOffset(Helper.PP.Position.Z, MansionNativeGarage.Origin(m).Z);
					frozenPed = Helper.PP;
					savedFrozen = frozenPed.IsPositionFrozen;
					frozenPed.IsPositionFrozen = true;
					EnsureMenus();
					LoadRoster();
					shopMenu.ParentMenu = parent;
					if (parent != null)
					{
						parent.Visible = false;
					}
					filterIndex = 0;
					pageIndex = 0;
					searchText = "";
					upgradeWanted = false;
					RebuildShop(true);
					shopMenu.Visible = true;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Vehicle shop open: " + ex2.Message + " " + ex2.StackTrace);
				CloseSession(true);
				Notification.Show("The motor gallery could not open. Details: SPA II.log.");
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (!opened && parent != null)
				{
					parent.Visible = true;
				}
			}
		}

		private static void RebuildShop(bool focusFirstVehicle = false)
		{
			int val = ((shopMenu.MenuItems.Count > 0) ? shopMenu.CurrentSelection : 0);
			CancelPreviewRequest();
			selected = null;
			upgradeWanted = false;
			rebuilding = true;
			checked
			{
				try
				{
					matching = FilterEntries(roster, filterIndex, searchText);
					pageIndex = Math.Max(0, Math.Min(pageIndex, PageCount(matching.Count) - 1));
					shopMenu.Clear();
					filterItem = new UIMenuListItem("Category", new List<object>(filterNames), filterIndex, "Only installed models are listed. HSW uses the actual Enhanced vehicle mods.");
					shopMenu.AddItem(filterItem);
					IEnumerable<object> collection = Enumerable.Range(1, PageCount(matching.Count)).Select((_Closure_0024__._0024I66_002D0 != null) ? _Closure_0024__._0024I66_002D0 : (_Closure_0024__._0024I66_002D0 = _Closure_0024__._0024I._Lambda_0024__66_002D0));
					pageItem = new UIMenuListItem("Page", new List<object>(collection), pageIndex, $"{matching.Count} matching models. {Mansions.MansionGarageVehicles(shopMansion).Count}/{20} garage spaces used.");
					shopMenu.AddItem(pageItem);
					shopMenu.AddItem(new UIMenuItem((searchText.Length == 0) ? "Search vehicle / make" : ("Search: " + searchText), "Search names, makes or model names; erase the text to clear.")
					{
						Tag = "Search"
					});
					foreach (CatalogEntry item in matching.Skip(pageIndex * 18).Take(18))
					{
						UIMenuItem uIMenuItem = new UIMenuItem(item.Title, EntryDescription(item) + " Select to view the car and purchase options.")
						{
							Tag = item
						};
						uIMenuItem.SetRightLabel((item.Hsw ? "HSW  " : "") + "$" + item.Price.ToString("N0"));
						shopMenu.AddItem(uIMenuItem);
					}
					if (matching.Count == 0)
					{
						shopMenu.AddItem(new UIMenuItem("No vehicles match", "Choose another category or clear the search.")
						{
							Enabled = false
						});
					}
					shopMenu.RefreshIndex();
				}
				finally
				{
					rebuilding = false;
				}
				int num = shopMenu.MenuItems.FindIndex((_Closure_0024__._0024I66_002D1 != null) ? _Closure_0024__._0024I66_002D1 : (_Closure_0024__._0024I66_002D1 = _Closure_0024__._0024I._Lambda_0024__66_002D1));
				int num2 = (focusFirstVehicle ? Math.Max(0, num) : Math.Max(0, Math.Min(val, shopMenu.MenuItems.Count - 1)));
				shopMenu.CurrentSelection = num2;
				CatalogEntry catalogEntry = shopMenu.MenuItems[num2].Tag as CatalogEntry;
				if (catalogEntry == null && num >= 0)
				{
					catalogEntry = shopMenu.MenuItems[num].Tag as CatalogEntry;
				}
				QueuePreview(catalogEntry);
			}
		}

		private static string EntryDescription(CatalogEntry e)
		{
			return classNames[e.ClassId] + "; " + e.ModelName + "; " + (e.OriginalPrice ? "original base-price snapshot (no discounts)" : "SPA dealer price") + ".";
		}

		private static void ShopListChange(UIMenu sender, UIMenuListItem item, int newIndex)
		{
			if (rebuilding)
			{
				return;
			}
			if (item == filterItem)
			{
				filterIndex = newIndex;
				pageIndex = 0;
			}
			else
			{
				if (item != pageItem)
				{
					return;
				}
				pageIndex = newIndex;
			}
			RebuildShop();
		}

		private static void ShopIndexChange(UIMenu sender, int newIndex)
		{
			if (!rebuilding && newIndex >= 0 && newIndex < sender.MenuItems.Count)
			{
				QueuePreview(sender.MenuItems[newIndex].Tag as CatalogEntry);
			}
		}

		private static void ShopSelect(UIMenu sender, UIMenuItem item, int index)
		{
			if (busy || rebuilding)
			{
				return;
			}
			CatalogEntry catalogEntry = item.Tag as CatalogEntry;
			if (catalogEntry != null)
			{
				selected = catalogEntry;
				upgradeWanted = false;
				BuildDetail();
				sender.Visible = false;
				detailMenu.Visible = true;
				QueuePreview(catalogEntry, true);
			}
			else if (Operators.CompareString(Conversions.ToString(item.Tag), "Search", false) == 0)
			{
				string userInput = Game.GetUserInput(WindowTitle.EnterMessage60, searchText, 40);
				if (userInput != null)
				{
					searchText = userInput.Trim();
				}
				pageIndex = 0;
				RebuildShop();
			}
		}

		private static void BuildDetail()
		{
			detailMenu.Clear();
			detailMenu.Subtitle.Caption = selected.Title.ToUpperInvariant();
			detailMenu.AddItem(new UIMenuItem(selected.Title, EntryDescription(selected))
			{
				Enabled = false
			});
			if (selected.Hsw)
			{
				hswItem = new UIMenuCheckboxItem("HSW performance package", upgradeWanted, $"Actual Enhanced HSW conversion and available HSW performance parts. SPA package charge: ${500000:N0}; not an Online transaction.");
				detailMenu.AddItem(hswItem);
			}
			else
			{
				hswItem = null;
			}
			int num = EffectivePrice(selected, upgradeWanted);
			buyItem = new UIMenuItem("Buy and deliver to this garage", $"Save to {shopMansion.Name}; money is charged only after the vehicle has been saved. Preview vehicles are temporary.")
			{
				Tag = "Buy"
			};
			buyItem.SetRightLabel("$" + num.ToString("N0"));
			detailMenu.AddItem(buyItem);
			detailMenu.AddItem(new UIMenuItem("3D preview rotates automatically", "Use the catalogue to change the model. Back closes the camera and keeps your existing cars.")
			{
				Enabled = false
			});
			detailMenu.RefreshIndex();
		}

		private static void DetailCheckbox(UIMenu sender, UIMenuCheckboxItem item, bool @checked)
		{
			if (item == hswItem && selected != null)
			{
				upgradeWanted = @checked;
				buyItem.SetRightLabel("$" + EffectivePrice(selected, upgradeWanted).ToString("N0"));
				QueuePreview(selected, true);
			}
		}

		private static void DetailSelect(UIMenu sender, UIMenuItem item, int index)
		{
			if (Operators.CompareString(Conversions.ToString(item.Tag), "Buy", false) == 0 && !busy)
			{
				Purchase();
			}
		}

		private static void QueuePreview(CatalogEntry entry, bool force = false)
		{
			if (entry == null)
			{
				CancelPreviewRequest();
			}
			else if (force || previewEntry != entry || (object)preview == null || !preview.Exists())
			{
				CancelPreviewRequest(true);
				pending = entry;
				pendingAt = checked(Game.GameTime + 220);
			}
		}

		private static void CancelPreviewRequest(bool keepCamera = false)
		{
			pending = null;
			pendingAt = 0;
			loadDeadline = 0;
			previewError = null;
			try
			{
				ReleaseLoading();
			}
			finally
			{
				DeletePreview(keepCamera);
			}
		}

		private static void ReleaseLoading()
		{
			Model? model = loading;
			loading = null;
			if (model.HasValue)
			{
				model.Value.MarkAsNoLongerNeeded();
			}
		}

		private static void BeginPreview()
		{
			if (pending == null)
			{
				return;
			}
			if (!loading.HasValue)
			{
				loading = new Model(pending.ModelHash);
				loadDeadline = checked(Game.GameTime + 5000);
			}
			loading.Value.Request();
			if (!loading.Value.IsLoaded)
			{
				if (Game.GameTime > loadDeadline)
				{
					previewError = "The selected model could not be loaded. No money has been charged.";
					Logger.Log("Vehicle shop preview model timeout: " + pending.ModelName);
					pending = null;
					ReleaseLoading();
				}
				return;
			}
			CatalogEntry catalogEntry = pending;
			pending = null;
			try
			{
				Vector3 min;
				Vector3 max;
				loading.Value.GetDimensions(out min, out max);
				if (!FitsMansionBay(min, max))
				{
					previewError = "This vehicle is too large for the mansion bays; purchase is unavailable.";
					return;
				}
				Quaternion result = default(Quaternion);
				Vector3 cameraPosition = default(Vector3);
				Vector3 focus = default(Vector3);
				float cameraFov = default(float);
				if (!TryPreviewPoint(min, max, ref result, ref cameraPosition, ref focus, ref cameraFov))
				{
					previewError = "No clear preview space and camera view are available on this garage floor. Your cars have been kept.";
					return;
				}
				previewFloor = Helper.ToVector3(result);
				previewMinimum = min;
				previewMaximum = max;
				if (!MansionShopPreviewGeometry.TryVehicleOrigin(previewFloor, min, max, result.W, ref previewOrigin))
				{
					throw new IOException("Invalid preview model origin.");
				}
				preview = World.CreateVehicle(loading.Value, previewOrigin, result.W);
				if ((object)preview == null || !preview.Exists())
				{
					throw new IOException("Preview creation failed.");
				}
				Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, preview.Handle, previewOrigin.X, previewOrigin.Y, previewOrigin.Z, false, false, false);
				preview.IsPersistent = true;
				preview.IsInvincible = true;
				preview.IsPositionFrozen = true;
				Function.Call(Hash.SET_ENTITY_COLLISION, preview.Handle, false, false);
				Function.Call(Hash.SET_VEHICLE_DOORS_LOCKED, preview.Handle, 2);
				Function.Call(Hash.SET_VEHICLE_ENGINE_ON, preview.Handle, false, true, true);
				Function.Call(Hash.SET_VEHICLE_MOD_KIT, preview.Handle, 0);
				if (upgradeWanted && detailMenu.Visible && selected == catalogEntry && !TryApplyHsw(preview))
				{
					previewError = "The installed model does not expose the required HSW parts. Select the stock vehicle or check your Enhanced game files.";
					preview.Delete();
					preview = null;
					return;
				}
				MansionNativeGarage.RegisterShopPreviewRoom(shopMansion, preview);
				previewEntry = catalogEntry;
				initialHeading = result.W;
				rotationAt = Game.GameTime;
				if ((object)camera == null || !camera.Exists())
				{
					previousCamera = World.RenderingCamera;
					camera = World.CreateCamera(cameraPosition, Vector3.Zero, cameraFov);
					if ((object)camera == null || !camera.Exists())
					{
						throw new IOException("Preview camera could not be created.");
					}
				}
				else
				{
					camera.Position = cameraPosition;
					camera.FieldOfView = cameraFov;
				}
				camera.PointAt(focus);
				World.RenderingCamera = camera;
				Logger.Log($"Vehicle shop preview: {catalogEntry.ModelName}; floor={previewFloor}; camera={cameraPosition}; viewport=right; fov={cameraFov:0.0}");
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				previewError = "3D preview failed. No money has been charged.";
				Logger.Log("Vehicle shop preview: " + ex2.Message);
				DeletePreview();
				ProjectData.ClearProjectError();
			}
			finally
			{
				ReleaseLoading();
			}
		}

		private static bool TryPreviewPoint(Vector3 minimum, Vector3 maximum, ref Quaternion result, ref Vector3 cameraPosition, ref Vector3 focus, ref float cameraFov)
		{
			Vector3 vector = maximum - minimum;
			float num = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y) * 0.5f + 0.15f;
			Vector3[] array = new Vector3[6]
			{
				new Vector3(2.3f, -0.5f, 0f),
				new Vector3(2.3f, -6.5f, 0f),
				new Vector3(2.3f, 7.5f, 0f),
				new Vector3(4.5f, -13f, 0f),
				new Vector3(4.5f, 14f, 0f),
				new Vector3(6f, -5f, 0f)
			};
			checked
			{
				Vector3 floorPoint = default(Vector3);
				Vector3 floorPoint2 = default(Vector3);
				for (int i = 0; i < array.Length; i++)
				{
					Vector3 vector2 = array[i];
					Quaternion q = MansionNativeGarage.LocalPoint(shopMansion, new Vector3(vector2.X, vector2.Y, shopFloorOffset + 0.05f), 180f);
					if (!PartyPlacement.TryGetFloor(Helper.PP.Position, Helper.ToVector3(q), ref floorPoint))
					{
						continue;
					}
					bool flag = true;
					Vector3[] array2 = new Vector3[4]
					{
						new Vector3(num, 0f, 0f),
						new Vector3(0f - num, 0f, 0f),
						new Vector3(0f, num, 0f),
						new Vector3(0f, 0f - num, 0f)
					};
					foreach (Vector3 vector3 in array2)
					{
						if (!PartyPlacement.TryGetFloor(Helper.PP.Position, floorPoint + vector3, ref floorPoint2) || Math.Abs(floorPoint2.Z - floorPoint.Z) > 0.18f)
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						continue;
					}
					Vehicle[] nearbyVehicles = World.GetNearbyVehicles(floorPoint, num + 5f);
					foreach (Vehicle vehicle in nearbyVehicles)
					{
						if ((object)vehicle != null && vehicle.Exists() && !(Math.Abs(vehicle.Position.Z - floorPoint.Z) > 3f))
						{
							Vector3 min;
							Vector3 max;
							vehicle.Model.GetDimensions(out min, out max);
							Vector3 vector4 = max - min;
							float num2 = (float)Math.Sqrt(vector4.X * vector4.X + vector4.Y * vector4.Y) * 0.5f;
							if (Mansion.Dist2D(vehicle.Position, floorPoint) < num + num2 + 0.2f)
							{
								flag = false;
								break;
							}
						}
					}
					if (!flag)
					{
						continue;
					}
					Ped[] nearbyPeds = World.GetNearbyPeds(floorPoint, num + 0.7f);
					foreach (Ped ped in nearbyPeds)
					{
						if ((object)ped != null && ped.Exists() && Math.Abs(ped.Position.Z - floorPoint.Z) < 2.5f)
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						continue;
					}
					float[] array3 = new float[3]
					{
						0.35f,
						Math.Min(1.3f, vector.Z),
						vector.Z - 0.1f
					};
					foreach (float z in array3)
					{
						int num3 = 0;
						do
						{
							double num4 = (double)num3 * Math.PI / 8.0;
							Vector3 vector5 = new Vector3(num * (float)Math.Cos(num4), num * (float)Math.Sin(num4), 0f);
							Vector3 vector6 = floorPoint + new Vector3(0f, 0f, z);
							RaycastResult raycastResult = World.Raycast(vector6 - vector5, vector6 + vector5, IntersectFlags.LosToEntity, Helper.PP);
							if (raycastResult.Result != 2 || raycastResult.DidHit)
							{
								flag = false;
								break;
							}
							num3++;
						}
						while (num3 <= 7);
						if (!flag)
						{
							break;
						}
					}
					if (flag && TryPreviewCamera(floorPoint, minimum, maximum, ref cameraPosition, ref focus, ref cameraFov))
					{
						result = new Quaternion(floorPoint.X, floorPoint.Y, floorPoint.Z, q.W);
						return true;
					}
				}
				return false;
			}
		}

		private static bool ClearCameraView(Vector3 floor, Vector3 minimum, Vector3 maximum, Vector3 candidate)
		{
			Vector3 vector = maximum - minimum;
			Vector3 source = new Vector3(floor.X, floor.Y, floor.Z + vector.Z * 0.5f + 0.02f);
			IntersectFlags options = IntersectFlags.LosToEntity | IntersectFlags.Vehicles;
			RaycastResult raycastResult = World.Raycast(source, candidate, options, preview);
			if (raycastResult.Result != 2 || raycastResult.DidHit)
			{
				return false;
			}
			float num = (float)Math.Sqrt((double)vector.X * (double)vector.X + (double)vector.Y * (double)vector.Y) * 0.5f;
			float[] array = new float[2]
			{
				0.12f,
				Math.Max(0.13f, vector.Z - 0.08f)
			};
			foreach (float z in array)
			{
				int num2 = 0;
				do
				{
					double num3 = (double)num2 * Math.PI / 4.0;
					RaycastResult raycastResult2 = World.Raycast(floor + new Vector3(num * (float)Math.Cos(num3), num * (float)Math.Sin(num3), z), candidate, options, preview);
					if (raycastResult2.Result != 2 || raycastResult2.DidHit)
					{
						return false;
					}
					num2 = checked(num2 + 1);
				}
				while (num2 <= 7);
			}
			Vector3[] array2 = new Vector3[3]
			{
				new Vector3(0.35f, 0f, 0f),
				new Vector3(0f, 0.35f, 0f),
				new Vector3(0f, 0f, 0.35f)
			};
			foreach (Vector3 vector2 in array2)
			{
				RaycastResult raycastResult3 = World.Raycast(candidate - vector2, candidate + vector2, options, preview);
				if (raycastResult3.Result != 2 || raycastResult3.DidHit)
				{
					return false;
				}
			}
			return true;
		}

		private static bool TryPreviewCamera(Vector3 floor, Vector3 minimum, Vector3 maximum, ref Vector3 position, ref Vector3 focus, ref float fov)
		{
			int num = 0;
			Vector3 position2 = default(Vector3);
			Vector3 focus2 = default(Vector3);
			float fov2 = default(float);
			do
			{
				float azimuth = MansionNativeGarage.Rotation(shopMansion) + 135f + (float)num * 22.5f;
				if (MansionShopPreviewGeometry.TryCameraPose(floor, minimum, maximum, azimuth, Screen.AspectRatio, ref position2, ref focus2, ref fov2) && ClearCameraView(floor, minimum, maximum, position2))
				{
					position = position2;
					focus = focus2;
					fov = fov2;
					return true;
				}
				num = checked(num + 1);
			}
			while (num <= 15);
			return false;
		}

		public static bool TryApplyHsw(Vehicle vehicle)
		{
			if ((object)vehicle == null || !vehicle.Exists() || !IsHswModel(vehicle.Model.Hash))
			{
				return false;
			}
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			int[] array = new int[6] { 36, 11, 12, 13, 15, 34 };
			int[] array2 = array;
			foreach (int num in array2)
			{
				dictionary[num] = Function.Call<int>(Hash.GET_VEHICLE_MOD, vehicle.Handle, num);
			}
			try
			{
				Function.Call(Hash.SET_VEHICLE_MOD_KIT, vehicle.Handle, 0);
				if (Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, vehicle.Handle, 36) < 1)
				{
					return false;
				}
				Function.Call(Hash.SET_VEHICLE_MOD, vehicle.Handle, 36, 0, false);
				if (Function.Call<int>(Hash.GET_VEHICLE_MOD, vehicle.Handle, 36) != 0)
				{
					throw new IOException("HSW base conversion was rejected.");
				}
				KeyValuePair<int, int>[] array3 = new KeyValuePair<int, int>[5]
				{
					new KeyValuePair<int, int>(11, 4),
					new KeyValuePair<int, int>(12, 3),
					new KeyValuePair<int, int>(13, 3),
					new KeyValuePair<int, int>(15, HswSuspensionIndex(vehicle.Model.Hash)),
					new KeyValuePair<int, int>(34, 2)
				};
				for (int j = 0; j < array3.Length; j = checked(j + 1))
				{
					KeyValuePair<int, int> keyValuePair = array3[j];
					int num2 = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, vehicle.Handle, keyValuePair.Key);
					if (num2 == 0)
					{
						continue;
					}
					if (num2 <= keyValuePair.Value)
					{
						if (keyValuePair.Key != 15 && keyValuePair.Key != 34)
						{
							throw new IOException("Required HSW performance option is unavailable, slot " + Conversions.ToString(keyValuePair.Key));
						}
						continue;
					}
					Function.Call(Hash.SET_VEHICLE_MOD, vehicle.Handle, keyValuePair.Key, keyValuePair.Value, false);
					if (Function.Call<int>(Hash.GET_VEHICLE_MOD, vehicle.Handle, keyValuePair.Key) != keyValuePair.Value)
					{
						throw new IOException("HSW option did not apply, slot " + Conversions.ToString(keyValuePair.Key));
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Vehicle shop HSW: " + ex2.Message);
				int[] array4 = array;
				foreach (int num3 in array4)
				{
					Function.Call(Hash.SET_VEHICLE_MOD, vehicle.Handle, num3, dictionary[num3], false);
				}
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		private static void Purchase()
		{
			if (selected == null || shopMansion == null || sessionOwner != Helper.GetPlayer() || !shopMansion.IsOwnedByPlayer())
			{
				return;
			}
			if ((object)preview == null || !preview.Exists() || previewEntry != selected || previewError != null)
			{
				Notification.Show(previewError ?? "Wait for the 3D preview to load before buying.");
				return;
			}
			if (Mansions.MansionGarageVehicles(shopMansion).Count >= 20)
			{
				Notification.Show("Your mansion garage is full. Free a space before buying.");
				return;
			}
			int num = EffectivePrice(selected, upgradeWanted);
			if (num <= 0 || Game.Player.Money < num)
			{
				Notification.Show("You do not have enough money for this vehicle.");
				return;
			}
			busy = true;
			try
			{
				if (upgradeWanted && !TryApplyHsw(preview))
				{
					Notification.Show("HSW is unavailable for this installed model. No money has been charged.");
					return;
				}
				string title = selected.Title;
				Mansion m = shopMansion;
				Vehicle veh = preview;
				int committedUid = 0;
				if (!MansionNativeGarage.StoreVehicle(m, veh, ref committedUid))
				{
					return;
				}
				preview = null;
				if (!TryChargeOwner(sessionOwner, num))
				{
					string path = Path.Combine(Helper.grgXmlPath + shopMansion.GarageFolder(), Conversions.ToString(MansionNativeGarage.LastStoredUid) + ".xml");
					if (File.Exists(path))
					{
						File.Delete(path);
					}
					MansionNativeGarage.Refresh(shopMansion);
					Notification.Show("The purchase was cancelled because payment could not complete. No money was charged.");
					DeletePreview();
					detailMenu.Visible = false;
					RebuildShop();
					shopMenu.Visible = true;
				}
				else
				{
					Logger.Log($"Vehicle shop purchase: {title}, owner={sessionOwner}, garage={shopMansion.GarageId}, uid={MansionNativeGarage.LastStoredUid}, price={num}, HSW={upgradeWanted}");
					Notification.Show($"~g~{title}~w~ bought for ${num:N0} and delivered to your mansion garage.");
					DeletePreview();
					detailMenu.Visible = false;
					RebuildShop();
					shopMenu.Visible = true;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Vehicle shop purchase: " + ex2.Message + " " + ex2.StackTrace);
				Notification.Show("Vehicle purchase could not complete. Details: SPA II.log.");
				ProjectData.ClearProjectError();
			}
			finally
			{
				busy = false;
			}
		}

		private static bool TryChargeOwner(eOwner owner, int amount)
		{
			string input;
			switch (owner)
			{
			case eOwner.Michael:
				input = "SP0_TOTAL_CASH";
				break;
			case eOwner.Franklin:
				input = "SP1_TOTAL_CASH";
				break;
			case eOwner.Trevor:
				input = "SP2_TOTAL_CASH";
				break;
			default:
				return false;
			}
			int num = Game.GenerateHash(input);
			OutputArgument outputArgument = new OutputArgument();
			if (!Function.Call<bool>(Hash.STAT_GET_INT, num, outputArgument, -1))
			{
				return false;
			}
			int result = outputArgument.GetResult<int>();
			if (amount < 1 || result < amount)
			{
				return false;
			}
			return Function.Call<bool>(Hash.STAT_SET_INT, num, checked(result - amount), true);
		}

		private static void DeletePreview(bool keepCamera = false)
		{
			if (!keepCamera && (object)camera != null)
			{
				try
				{
					Camera renderingCamera = World.RenderingCamera;
					if ((object)renderingCamera != null && renderingCamera.Handle == camera.Handle)
					{
						World.RenderingCamera = (((object)previousCamera != null && previousCamera.Exists()) ? previousCamera : null);
					}
					if (camera.Exists())
					{
						camera.Delete();
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Vehicle shop camera cleanup: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
				camera = null;
				previousCamera = null;
			}
			if ((object)preview != null)
			{
				try
				{
					MansionNativeGarage.ReleaseShopPreviewRoom(preview);
					if (preview.Exists())
					{
						preview.Delete();
					}
				}
				catch (Exception ex3)
				{
					ProjectData.SetProjectError(ex3);
					Exception ex4 = ex3;
					Logger.Log("Vehicle shop vehicle cleanup: " + ex4.Message);
					ProjectData.ClearProjectError();
				}
				finally
				{
					preview = null;
				}
			}
			previewEntry = null;
		}

		public static void Tick()
		{
			if (!opened)
			{
				return;
			}
			checked
			{
				try
				{
					if (shopMansion == null || !shopMansion.IsOwnedByPlayer() || Helper.GetPlayer() != sessionOwner || Helper.PP.IsDead || !MansionNativeGarage.IsActive(shopMansion) || (!shopMenu.Visible && !detailMenu.Visible))
					{
						CloseSession(true);
						return;
					}
					if (pending != null && Game.GameTime >= pendingAt)
					{
						BeginPreview();
					}
					if ((object)preview != null && preview.Exists())
					{
						float num = (initialHeading + (float)(Game.GameTime - rotationAt) * 0.012f) % 360f;
						Vector3 origin = default(Vector3);
						if (MansionShopPreviewGeometry.TryVehicleOrigin(previewFloor, previewMinimum, previewMaximum, num, ref origin))
						{
							previewOrigin = origin;
							Function.Call(Hash.SET_ENTITY_ROTATION, preview.Handle, 0f, 0f, num, 2, true);
							Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, preview.Handle, origin.X, origin.Y, origin.Z, false, false, false);
						}
						Control[] array = new Control[11]
						{
							Control.MoveLeftRight,
							Control.MoveUpDown,
							Control.Jump,
							Control.Attack,
							Control.Aim,
							Control.Enter,
							Control.VehicleExit,
							Control.CharacterWheel,
							Control.SelectCharacterMichael,
							Control.SelectCharacterFranklin,
							Control.SelectCharacterTrevor
						};
						for (int i = 0; i < array.Length; i++)
						{
							Game.DisableControlThisFrame(array[i]);
						}
						Vector3 vector = previewOrigin + new Vector3(1f, -1f, 2.6f);
						Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, vector.X, vector.Y, vector.Z, 230, 235, 255, 8f, 3f);
					}
					if (previewError != null && (shopMenu.Visible || detailMenu.Visible))
					{
						Screen.ShowSubtitle(previewError, 1);
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Vehicle shop tick: " + ex2.Message);
					CloseSession(true);
					ProjectData.ClearProjectError();
				}
			}
		}

		private static void CloseSession(bool restorePosition)
		{
			Mansion mansion = shopMansion;
			bool num = restorePosition && opened && returnToOrigin && mansion != null;
			UIMenu uIMenu = sourceParent;
			Ped ped = sourcePed;
			Quaternion q = sourcePose;
			eOwner eOwner2 = sessionOwner;
			Cleanup();
			if (!num || (object)ped == null || !ped.Exists() || (object)Helper.PP == null || !Helper.PP.Exists() || ped.Handle != Helper.PP.Handle || Helper.PP.IsDead || Helper.PP.IsInVehicle() || Helper.GetPlayer() != eOwner2 || !MansionNativeGarage.IsActive(mansion))
			{
				return;
			}
			if (MansionFeatures.TeleportPlayer(Helper.ToVector3(q), q.W))
			{
				MansionNativeGarage.Clear();
				if (uIMenu != null)
				{
					uIMenu.Visible = true;
				}
				Logger.Log("Vehicle shop: returned to the position where the gallery was opened.");
			}
			else
			{
				Notification.Show("The gallery is closed. Return to the mansion through the garage menu.");
			}
		}

		public static void Cleanup()
		{
			try
			{
				CancelPreviewRequest();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Vehicle shop request cleanup: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			if ((object)frozenPed != null)
			{
				try
				{
					if (frozenPed.Exists())
					{
						frozenPed.IsPositionFrozen = savedFrozen;
					}
				}
				catch (Exception ex3)
				{
					ProjectData.SetProjectError(ex3);
					Exception ex4 = ex3;
					Logger.Log("Vehicle shop player cleanup: " + ex4.Message);
					ProjectData.ClearProjectError();
				}
				finally
				{
					frozenPed = null;
				}
			}
			if (shopMenu != null)
			{
				shopMenu.Visible = false;
			}
			if (detailMenu != null)
			{
				detailMenu.Visible = false;
			}
			opened = false;
			shopMansion = null;
			selected = null;
			upgradeWanted = false;
			previewError = null;
			busy = false;
			returnToOrigin = false;
			sourcePed = null;
			sourceParent = null;
		}
	}
}
