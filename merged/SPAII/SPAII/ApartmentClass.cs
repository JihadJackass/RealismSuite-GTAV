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
using INMNativeUI;
using Metadata;
using Microsoft.VisualBasic.CompilerServices;
using SPAII.INM;

namespace SPAII
{
	public class ApartmentClass
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<BuildingClass, IEnumerable<ApartmentClass>> _0024I35_002D0;

			public static Func<BuildingClass, ApartmentClass, VB_0024AnonymousType_0<BuildingClass, ApartmentClass>> _0024I35_002D1;

			public static Func<VB_0024AnonymousType_0<BuildingClass, ApartmentClass>, BuildingClass> _0024I35_002D3;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal IEnumerable<ApartmentClass> _Lambda_0024__35_002D0(BuildingClass b)
			{
				return b.Apartments;
			}

			[SpecialName]
			internal VB_0024AnonymousType_0<BuildingClass, ApartmentClass> _Lambda_0024__35_002D1(BuildingClass b, ApartmentClass a)
			{
				return new VB_0024AnonymousType_0<BuildingClass, ApartmentClass>(b, a);
			}

			[SpecialName]
			internal BuildingClass _Lambda_0024__35_002D3(VB_0024AnonymousType_0<BuildingClass, ApartmentClass> _0024VB_0024It)
			{
				return _0024VB_0024It.b;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__42_002D0
		{
			public eOwner _0024VB_0024Local_owner;

			public string _0024VB_0024Local_settingsPath;

			public ApartmentClass _0024VB_0024Me;

			public _Closure_0024__42_002D0(_Closure_0024__42_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_owner = arg0._0024VB_0024Local_owner;
					_0024VB_0024Local_settingsPath = arg0._0024VB_0024Local_settingsPath;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				if (PropertyProfiles.ActiveCharacter != _0024VB_0024Local_owner || !File.Exists(_0024VB_0024Local_settingsPath))
				{
					return false;
				}
				ScriptSettings settings = ScriptSettings.Load(_0024VB_0024Local_settingsPath);
				return PropertyProfiles.OwnerOf(settings, _0024VB_0024Me.Name, _0024VB_0024Local_owner) == _0024VB_0024Local_owner && PropertyProfiles.LegacyDataBelongsTo(settings, _0024VB_0024Me.Name, _0024VB_0024Local_owner);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__52_002D0
		{
			public eOwner _0024VB_0024Local_owner;

			public string _0024VB_0024Local_section;

			public string _0024VB_0024Local_selectedIPL;

			public ApartmentClass _0024VB_0024Me;

			public _Closure_0024__52_002D0(_Closure_0024__52_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_owner = arg0._0024VB_0024Local_owner;
					_0024VB_0024Local_section = arg0._0024VB_0024Local_section;
					_0024VB_0024Local_selectedIPL = arg0._0024VB_0024Local_selectedIPL;
				}
			}

			[SpecialName]
			internal void _Lambda_0024__0(string candidate)
			{
				File.Copy("scripts\\SPA II\\modconfig.ini", candidate, true);
				ScriptSettings scriptSettings = ScriptSettings.Load(candidate);
				if (PropertyProfiles.OwnerOf(scriptSettings, _0024VB_0024Me.Name, _0024VB_0024Local_owner) != _0024VB_0024Local_owner)
				{
					throw new IOException("Apartment ownership changed.");
				}
				scriptSettings.SetValue(_0024VB_0024Local_section, _0024VB_0024Me.Name, _0024VB_0024Local_selectedIPL);
				if (!scriptSettings.Save())
				{
					throw new IOException("Apartment style could not be saved.");
				}
			}

			[SpecialName]
			internal void _Lambda_0024__1(string candidate)
			{
				if (Operators.CompareString(ScriptSettings.Load(candidate).GetValue(_0024VB_0024Local_section, _0024VB_0024Me.Name, ""), _0024VB_0024Local_selectedIPL, false) != 0)
				{
					throw new IOException("Apartment style could not be verified.");
				}
			}
		}

		public int ID;

		public string Name;

		public string Description;

		public int Price;

		public Door Door;

		public Vector3 SavePos;

		public Vector3 ApartmentInPos;

		public Quaternion ApartmentDoorPos;

		public Vector3 ApartmentOutPos;

		public Quaternion WardrobePos;

		public string GarageFilePath;

		public string OldIPL;

		public string IPL;

		public CameraPRH EnterCam;

		public CameraPRH ExitCam;

		public CameraPRH AptStyleCam;

		public eApartmentType ApartmentType;

		public Vector3 GarageElevatorPos;

		public Vector3 GarageMenuPos;

		internal eOwner? MansionGarageOwner;

		public BuildingClass BuildingOverride;

		[System.Runtime.CompilerServices.AccessedThroughProperty("AptMenu")]
		private UIMenu _AptMenu;

		public virtual UIMenu AptMenu
		{
			[CompilerGenerated]
			get
			{
				return _AptMenu;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = AptMenu_OnItemSelect;
				UIMenu aptMenu = _AptMenu;
				if (aptMenu != null)
				{
					aptMenu.OnItemSelect -= value2;
				}
				_AptMenu = value;
				aptMenu = _AptMenu;
				if (aptMenu != null)
				{
					aptMenu.OnItemSelect += value2;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("StyleMenu")]
		private UIMenu _StyleMenu;

		public virtual UIMenu StyleMenu
		{
			[CompilerGenerated]
			get
			{
				return _StyleMenu;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = StyleMenu_OnItemSelect;
				IndexChangedEvent value3 = StyleMenu_OnIndexChange;
				MenuCloseEvent value4 = StyleMenu_OnMenuClose;
				UIMenu styleMenu = _StyleMenu;
				if (styleMenu != null)
				{
					styleMenu.OnItemSelect -= value2;
					styleMenu.OnIndexChange -= value3;
					styleMenu.OnMenuClose -= value4;
				}
				_StyleMenu = value;
				styleMenu = _StyleMenu;
				if (styleMenu != null)
				{
					styleMenu.OnItemSelect += value2;
					styleMenu.OnIndexChange += value3;
					styleMenu.OnMenuClose += value4;
				}
			}
		}

		internal bool HasCharacterProfile()
		{
			if (Price > 0)
			{
				return BuildingOverride == null;
			}
			return false;
		}

		public eOwner Owner()
		{
			return OwnerFor(PropertyProfiles.ActiveCharacter);
		}

		internal eOwner OwnerFor(eOwner character)
		{
			if (MansionGarageOwner.HasValue)
			{
				return MansionGarageOwner.Value;
			}
			if (HasCharacterProfile())
			{
				return PropertyProfiles.OwnerOf(ConfigHelper.config, Name, character);
			}
			return ConfigHelper.config.GetValue("BUILDING", Name, eOwner.Nobody);
		}

		internal string StorageFolder(eOwner owner)
		{
			if (!HasCharacterProfile() || !PropertyProfiles.IsCharacter(owner))
			{
				return GarageFilePath;
			}
			eOwner owner2 = default(eOwner);
			if (!PropertyProfiles.TryLegacyOwner(ConfigHelper.config, Name, ref owner2))
			{
				return null;
			}
			return PropertyProfiles.GarageFolder(GarageFilePath, owner, owner2);
		}

		private string ProfileIPL()
		{
			eOwner activeCharacter = PropertyProfiles.ActiveCharacter;
			string defaultvalue;
			switch (ID)
			{
			case 79:
				defaultvalue = "apa_v_mp_h_01_a";
				break;
			case 80:
				defaultvalue = "apa_v_mp_h_01_b";
				break;
			case 81:
				defaultvalue = "apa_v_mp_h_01_c";
				break;
			default:
				return IPL;
			}
			if (!PropertyProfiles.IsCharacter(activeCharacter) || PropertyProfiles.LegacyDataBelongsTo(ConfigHelper.config, Name, activeCharacter))
			{
				defaultvalue = ConfigHelper.config.GetValue("IPL", Name, defaultvalue);
			}
			return ConfigHelper.config.GetValue(PropertyProfiles.Section("IPL", activeCharacter), Name, defaultvalue);
		}

		internal void ApplyCharacterStyle()
		{
			if (ApartmentType != eApartmentType.IPL)
			{
				return;
			}
			string iPL = IPL;
			IPL = ProfileIPL();
			OldIPL = IPL;
			if (!string.IsNullOrEmpty(iPL) && Operators.CompareString(iPL, IPL, false) != 0 && Function.Call<bool>(Hash.IS_IPL_ACTIVE, iPL))
			{
				Function.Call(Hash.REQUEST_IPL, IPL);
				Function.Call(Hash.REMOVE_IPL, iPL);
			}
			checked
			{
				if (StyleMenu != null)
				{
					int num = StyleMenu.MenuItems.Count - 1;
					for (int i = 0; i <= num; i++)
					{
						bool flag = IPL.Contains((i + 1).ToString("00"));
						StyleMenu.MenuItems[i].SetRightBadge(flag ? UIMenuItem.BadgeStyle.Tick : UIMenuItem.BadgeStyle.None);
					}
				}
			}
		}

		public BuildingClass Building()
		{
			if (BuildingOverride != null)
			{
				return BuildingOverride;
			}
			return Helper.buildings.SelectMany((_Closure_0024__._0024I35_002D0 != null) ? _Closure_0024__._0024I35_002D0 : (_Closure_0024__._0024I35_002D0 = _Closure_0024__._0024I._Lambda_0024__35_002D0), (_Closure_0024__._0024I35_002D1 != null) ? _Closure_0024__._0024I35_002D1 : (_Closure_0024__._0024I35_002D1 = _Closure_0024__._0024I._Lambda_0024__35_002D1)).Where(_Lambda_0024__35_002D2).Select((_Closure_0024__._0024I35_002D3 != null) ? _Closure_0024__._0024I35_002D3 : (_Closure_0024__._0024I35_002D3 = _Closure_0024__._0024I._Lambda_0024__35_002D3))
				.FirstOrDefault();
		}

		public string FriendlyName()
		{
			if (ExecutiveGarageInventory.IsProperty(this))
			{
				return "Executive Garage";
			}
			return SouthLSHousing.PropertyText(Name);
		}

		public string FriendlyDescription()
		{
			if (ExecutiveGarageInventory.IsProperty(this))
			{
				return "An executive garage with three floors and 60 vehicle spaces.";
			}
			return SouthLSHousing.PropertyText(Description);
		}

		public Vector3 InteriorPos()
		{
			return Helper.ToVector3(WardrobePos);
		}

		public bool ShareInterior()
		{
			eApartmentType apartmentType = ApartmentType;
			if ((uint)(apartmentType - 1) <= 1u)
			{
				return true;
			}
			return false;
		}

		public int InteriorID()
		{
			return Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, InteriorPos().X, InteriorPos().Y, InteriorPos().Z);
		}

		public List<VehicleClass> Vehicles()
		{
			bool complete = default(bool);
			if (ID == 9310)
			{
				return ExecutiveGarageInventory.ReadVehicles(this, ref complete);
			}
			string arg = null;
			List<VehicleClass> list = new List<VehicleClass>();
			try
			{
				eOwner eOwner2 = Owner();
				if (HasCharacterProfile() && !PropertyProfiles.IsCharacter(eOwner2))
				{
					return list;
				}
				string text = StorageFolder(HasCharacterProfile() ? PropertyProfiles.ActiveCharacter : eOwner2);
				if (text == null || !Directory.Exists(Helper.grgXmlPath + text))
				{
					return list;
				}
				bool flag = HasCharacterProfile() && ApartmentType == eApartmentType.Other && eOwner2 == PropertyProfiles.ActiveCharacter && PropertyProfiles.LegacyDataBelongsTo(ConfigHelper.config, Name, eOwner2);
				string[] files = Directory.GetFiles(Helper.grgXmlPath + text, "*.xml");
				foreach (string text2 in files)
				{
					arg = text2;
					VehicleData vehicleData = new VehicleData(text2);
					VehicleData value = default(VehicleData);
					if (!vehicleData.TryRead(ref value))
					{
						continue;
					}
					if (flag && value.Owner == eOwner.Nobody && !File.Exists(text2 + ".bak"))
					{
						flag = false;
						BuildingClass buildingClass = Building();
						if (buildingClass != null)
						{
							MigrateLegacyStandaloneGarage(eOwner2, buildingClass.GetGarageVehicleCount(), "scripts\\SPA II\\modconfig.ini");
							if (!vehicleData.TryRead(ref value))
							{
								continue;
							}
						}
					}
					if (!HasCharacterProfile() || (value.Owner == eOwner2 && value.ApartmentID == ID))
					{
						VehicleClass item = new VehicleClass(value, eOwner2);
						if (!list.Contains(item))
						{
							list.Add(item);
						}
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"{ex2.Message}{arg}{ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
			return list;
		}

		internal void MigrateLegacyStandaloneGarage(eOwner owner, int capacity, string settingsPath)
		{
			_Closure_0024__42_002D0 arg = new _Closure_0024__42_002D0(null);
			arg._0024VB_0024Me = this;
			arg._0024VB_0024Local_owner = owner;
			arg._0024VB_0024Local_settingsPath = settingsPath;
			if (!HasCharacterProfile() || ApartmentType != eApartmentType.Other || ID == 9310 || !PropertyProfiles.IsCharacter(arg._0024VB_0024Local_owner) || OwnerFor(arg._0024VB_0024Local_owner) != arg._0024VB_0024Local_owner || !PropertyProfiles.LegacyDataBelongsTo(ConfigHelper.config, Name, arg._0024VB_0024Local_owner) || capacity < 1 || capacity > 10 || string.IsNullOrWhiteSpace(GarageFilePath))
			{
				return;
			}
			Func<bool> func = arg._Lambda_0024__0;
			checked
			{
				try
				{
					if (!func())
					{
						return;
					}
					string path = Path.Combine(Helper.grgXmlPath, GarageFilePath);
					List<VehicleData> list = new List<VehicleData>();
					HashSet<int> hashSet = new HashSet<int>();
					HashSet<int> hashSet2 = new HashSet<int>();
					int num = default(int);
					foreach (string item in Directory.EnumerateFiles(path))
					{
						num++;
						if (num > 256)
						{
							return;
						}
						bool flag = item.EndsWith(".xml.bak", StringComparison.OrdinalIgnoreCase);
						if (flag)
						{
							if (File.Exists(item.Substring(0, item.Length - ".bak".Length)))
							{
								continue;
							}
						}
						else if (!item.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
						{
							continue;
						}
						VehicleData value = default(VehicleData);
						if (!VehicleData.TryReadLegacyImportEvidence(item, ref value) || value.Index >= capacity || value.UniqueID == 0 || !hashSet.Add(value.Index) || !hashSet2.Add(value.UniqueID))
						{
							return;
						}
						if (!flag && value.Owner == eOwner.Nobody && value.ApartmentID == ID && !File.Exists(item + ".bak"))
						{
							string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(item);
							if (Operators.CompareString(fileNameWithoutExtension, value.UniqueID.ToString(CultureInfo.InvariantCulture), false) == 0 || Operators.CompareString(fileNameWithoutExtension, Conversions.ToString(value.UniqueID), false) == 0)
							{
								list.Add(value);
							}
						}
					}
					foreach (VehicleData item2 in list)
					{
						VehicleData.AssignLegacyImportOwner(item2, arg._0024VB_0024Local_owner, func);
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Legacy standalone vehicle migration: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
		}

		internal bool TryReadStandaloneGarageSlots(eOwner owner, int capacity, ref HashSet<int> occupied)
		{
			occupied = new HashSet<int>();
			try
			{
				string text = StorageFolder(owner);
				if (string.IsNullOrWhiteSpace(text))
				{
					return false;
				}
				string text2 = Path.Combine(Helper.grgXmlPath, text);
				if (File.Exists(text2))
				{
					return false;
				}
				if (!Directory.Exists(text2))
				{
					return true;
				}
				HashSet<string> unmoved = null;
				bool flag = default(bool);
				foreach (string item in Directory.EnumerateFiles(text2))
				{
					if (item.EndsWith(".xml.bak", StringComparison.OrdinalIgnoreCase))
					{
						if (File.Exists(item.Substring(0, checked(item.Length - ".bak".Length))))
						{
							continue;
						}
						if (!flag)
						{
							flag = true;
							if (!PropertySale.TryGetUnmovedGarageEvidence(this, owner, text2, ref unmoved))
							{
								unmoved = null;
							}
						}
						if (unmoved != null && !unmoved.Contains(Path.GetFullPath(item)))
						{
							continue;
						}
					}
					else if (!item.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
					{
						continue;
					}
					VehicleData value = default(VehicleData);
					if (!new VehicleData(item).TryRead(ref value) || value.Index >= capacity || !occupied.Add(value.Index))
					{
						return false;
					}
				}
				return true;
			}
			catch (IOException ex)
			{
				ProjectData.SetProjectError(ex);
				IOException ex2 = ex;
				Logger.Log("Standalone garage inventory: " + ex2.Message);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
			catch (UnauthorizedAccessException ex3)
			{
				ProjectData.SetProjectError(ex3);
				UnauthorizedAccessException ex4 = ex3;
				Logger.Log("Standalone garage inventory: " + ex4.Message);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public bool VehiclesContain(Vehicle veh)
		{
			bool result = false;
			foreach (VehicleClass item in Vehicles())
			{
				if (veh.GetInt(Helper.vehIdDecor) == item.ApartmentID && veh.GetInt(Helper.vehUidDecor) == item.UniqueID)
				{
					result = true;
				}
			}
			return result;
		}

		public ApartmentClass()
		{
			BuildingOverride = null;
		}

		public float WardrobeDistance()
		{
			return Game.Player.Character.Position.DistanceToSquared(Helper.ToVector3(WardrobePos));
		}

		public float SaveDistance()
		{
			return Game.Player.Character.Position.DistanceToSquared(SavePos);
		}

		public float ExitDistance()
		{
			return Game.Player.Character.Position.DistanceToSquared(ApartmentOutPos);
		}

		public void Load()
		{
			UIMenuItem uIMenuItem = new UIMenuItem(Game.GetLocalizedString("PM_APT_TVARIANT"))
			{
				Tag = "Style"
			};
			AptMenu = new UIMenu("", Game.GetLocalizedString("MP_PROP_GEN2A"), new Point(0, -107));
			AptMenu.SetBannerType(Helper.MenuBanner);
			AptMenu.MouseEdgeEnabled = false;
			Helper.MenuPool.Add(AptMenu);
			UIMenu aptMenu = AptMenu;
			aptMenu.AddItem(new UIMenuItem(Game.GetLocalizedString("MP_PROP_MENU2D"))
			{
				Tag = "Exit"
			});
			if (!SouthLSHousing.IsPropertyID(ID) || SouthLSHousing.HasGarage(ID))
			{
				aptMenu.AddItem(new UIMenuItem(Game.GetLocalizedString("MP_PROP_GOGAR"))
				{
					Tag = "Garage"
				});
			}
			if (ApartmentType == eApartmentType.IPL)
			{
				aptMenu.AddItem(uIMenuItem);
			}
			aptMenu.RefreshIndex();
			aptMenu = null;
			if (!Directory.Exists($"{Helper.grgXmlPath}{GarageFilePath}"))
			{
				Directory.CreateDirectory($"{Helper.grgXmlPath}{GarageFilePath}");
			}
			if (ApartmentType == eApartmentType.IPL)
			{
				IPL = ProfileIPL();
				StyleMenu = new UIMenu("", Game.GetLocalizedString("PM_APT_VARCAPS"), new Point(0, -107));
				StyleMenu.SetBannerType(Helper.MenuBanner);
				StyleMenu.MouseEdgeEnabled = false;
				Helper.MenuPool.Add(StyleMenu);
				UIMenu styleMenu = StyleMenu;
				UIMenuItem uIMenuItem2 = new UIMenuItem(Game.GetLocalizedString("PM_APT_VAR_0"));
				uIMenuItem2.Tag = "Modern";
				uIMenuItem2.SetRightBadge(IPL.Contains("01") ? UIMenuItem.BadgeStyle.Tick : UIMenuItem.BadgeStyle.None);
				styleMenu.AddItem(uIMenuItem2);
				UIMenuItem uIMenuItem3 = new UIMenuItem(Game.GetLocalizedString("PM_APT_VAR_1"));
				uIMenuItem3.Tag = "Moody";
				uIMenuItem3.SetRightBadge(IPL.Contains("02") ? UIMenuItem.BadgeStyle.Tick : UIMenuItem.BadgeStyle.None);
				styleMenu.AddItem(uIMenuItem3);
				UIMenuItem uIMenuItem4 = new UIMenuItem(Game.GetLocalizedString("PM_APT_VAR_2"));
				uIMenuItem4.Tag = "Vibrant";
				uIMenuItem4.SetRightBadge(IPL.Contains("03") ? UIMenuItem.BadgeStyle.Tick : UIMenuItem.BadgeStyle.None);
				styleMenu.AddItem(uIMenuItem4);
				UIMenuItem uIMenuItem5 = new UIMenuItem(Game.GetLocalizedString("PM_APT_VAR_3"));
				uIMenuItem5.Tag = "Sharp";
				uIMenuItem5.SetRightBadge(IPL.Contains("04") ? UIMenuItem.BadgeStyle.Tick : UIMenuItem.BadgeStyle.None);
				styleMenu.AddItem(uIMenuItem5);
				UIMenuItem uIMenuItem6 = new UIMenuItem(Game.GetLocalizedString("PM_APT_VAR_4"));
				uIMenuItem6.Tag = "Monochrome";
				uIMenuItem6.SetRightBadge(IPL.Contains("05") ? UIMenuItem.BadgeStyle.Tick : UIMenuItem.BadgeStyle.None);
				styleMenu.AddItem(uIMenuItem6);
				UIMenuItem uIMenuItem7 = new UIMenuItem(Game.GetLocalizedString("PM_APT_VAR_5"));
				uIMenuItem7.Tag = "Seductive";
				uIMenuItem7.SetRightBadge(IPL.Contains("06") ? UIMenuItem.BadgeStyle.Tick : UIMenuItem.BadgeStyle.None);
				styleMenu.AddItem(uIMenuItem7);
				UIMenuItem uIMenuItem8 = new UIMenuItem(Game.GetLocalizedString("PM_APT_VAR_6"));
				uIMenuItem8.Tag = "Regal";
				uIMenuItem8.SetRightBadge(IPL.Contains("07") ? UIMenuItem.BadgeStyle.Tick : UIMenuItem.BadgeStyle.None);
				styleMenu.AddItem(uIMenuItem8);
				UIMenuItem uIMenuItem9 = new UIMenuItem(Game.GetLocalizedString("PM_APT_VAR_7"));
				uIMenuItem9.Tag = "Aqua";
				uIMenuItem9.SetRightBadge(IPL.Contains("08") ? UIMenuItem.BadgeStyle.Tick : UIMenuItem.BadgeStyle.None);
				styleMenu.AddItem(uIMenuItem9);
				styleMenu.RefreshIndex();
				AptMenu.BindMenuToItem(StyleMenu, uIMenuItem);
			}
		}

		public void SetInteriorActive()
		{
			try
			{
				if ((ApartmentType == eApartmentType.IPL) | (ApartmentType == eApartmentType.IPLwoStyle))
				{
					Function.Call(Hash.REQUEST_IPL, IPL);
				}
				int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, InteriorPos().X, InteriorPos().Y, InteriorPos().Z);
				if (num == 0 && ((ApartmentType == eApartmentType.IPL) | (ApartmentType == eApartmentType.IPLwoStyle)))
				{
					int num2 = checked(Game.GameTime + 1000);
					while (num == 0 && Game.GameTime < num2)
					{
						Script.Yield();
						num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, InteriorPos().X, InteriorPos().Y, InteriorPos().Z);
					}
				}
				if (num == 0)
				{
					Logger.Log($"SetInteriorActive {Name}: no interior at {InteriorPos()}");
					return;
				}
				Stability.PinInterior(num, InteriorPos());
				Function.Call(Hash.SET_INTERIOR_ACTIVE, num, true);
				Function.Call(Hash.DISABLE_INTERIOR, num, false);
				Stability.Trace($"enter interior {num} ({Name})");
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"{ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		private void AptMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			try
			{
				if (SouthLSHousing.IsPropertyID(ID))
				{
					string left = selectedItem.Tag as string;
					if (Operators.CompareString(left, "Exit", false) != 0)
					{
						if (Operators.CompareString(left, "Garage", false) == 0)
						{
							SouthLSHousingTravel.EnterGarage(this);
						}
					}
					else
					{
						SouthLSHousingTravel.ExitApartment(this);
					}
					return;
				}
				object tag = selectedItem.Tag;
				if (Operators.ConditionalCompareObjectEqual(tag, "Exit", false))
				{
					ApartmentTravel.ExitHome(this);
				}
				else if (Operators.ConditionalCompareObjectEqual(tag, "Garage", false))
				{
					AptMenu.Visible = false;
					Helper.HideHud = true;
					PlayApartmerntExitCutscene();
					switch (Building().GarageType)
					{
					case eGarageType.TwoCarGarage:
						TwoCarGarage.Apartment = this;
						Helper.SetInteriorActive(TwoCarGarage.Interior);
						TwoCarGarage.LoadVehicles();
						break;
					case eGarageType.SixCarGarage:
						SixCarGarage.Apartment = this;
						Helper.SetInteriorActive(SixCarGarage.Interior);
						SixCarGarage.LoadVehicles();
						break;
					case eGarageType.TenCarGarage:
						TenCarGarage.Apartment = this;
						Helper.SetInteriorActive(TenCarGarage.Interior);
						TenCarGarage.LoadVehicles();
						break;
					}
					Building().PlayExitElevatorCutscene(5000);
					Helper.HideHud = false;
					World.RenderingCamera = null;
					Stability.DestroySpaCameras();
					switch (ApartmentType)
					{
					case eApartmentType.LowEnd:
						LowEndApartment.Apartment = null;
						break;
					case eApartmentType.MediumEnd:
						MediumEndApartment.Apartment = null;
						break;
					case eApartmentType.None:
					case eApartmentType.IPL:
					case eApartmentType.IPLwoStyle:
						HighEndApartment.Building = null;
						break;
					case eApartmentType.Other:
						break;
					}
				}
				else if (Operators.ConditionalCompareObjectEqual(tag, "Style", false))
				{
					Helper.FadeScreen(1);
					Helper.HideHud = true;
					World.RenderingCamera = Stability.CreateCam(AptStyleCam.Position, AptStyleCam.Rotation, AptStyleCam.FOV);
					Helper.FadeScreen(0);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"{ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		private void StyleMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			try
			{
				_Closure_0024__52_002D0 arg = new _Closure_0024__52_002D0(null);
				arg._0024VB_0024Me = this;
				arg._0024VB_0024Local_owner = PropertyProfiles.ActiveCharacter;
				if (!PropertyProfiles.IsCharacter(arg._0024VB_0024Local_owner) || Owner() != arg._0024VB_0024Local_owner || arg._0024VB_0024Local_owner != Helper.GetPlayer())
				{
					return;
				}
				string text = (arg._0024VB_0024Local_selectedIPL = ProfileIPL());
				object tag = selectedItem.Tag;
				if (Operators.ConditionalCompareObjectEqual(tag, "Modern", false))
				{
					switch (ID)
					{
					case 79:
						arg._0024VB_0024Local_selectedIPL = "apa_v_mp_h_01_a";
						break;
					case 80:
						arg._0024VB_0024Local_selectedIPL = "apa_v_mp_h_01_b";
						break;
					case 81:
						arg._0024VB_0024Local_selectedIPL = "apa_v_mp_h_01_c";
						break;
					}
				}
				else if (Operators.ConditionalCompareObjectEqual(tag, "Moody", false))
				{
					switch (ID)
					{
					case 79:
						arg._0024VB_0024Local_selectedIPL = "apa_v_mp_h_02_a";
						break;
					case 80:
						arg._0024VB_0024Local_selectedIPL = "apa_v_mp_h_02_b";
						break;
					case 81:
						arg._0024VB_0024Local_selectedIPL = "apa_v_mp_h_02_c";
						break;
					}
				}
				else if (Operators.ConditionalCompareObjectEqual(tag, "Vibrant", false))
				{
					switch (ID)
					{
					case 79:
						arg._0024VB_0024Local_selectedIPL = "apa_v_mp_h_03_a";
						break;
					case 80:
						arg._0024VB_0024Local_selectedIPL = "apa_v_mp_h_03_b";
						break;
					case 81:
						arg._0024VB_0024Local_selectedIPL = "apa_v_mp_h_03_c";
						break;
					}
				}
				else if (Operators.ConditionalCompareObjectEqual(tag, "Sharp", false))
				{
					switch (ID)
					{
					case 79:
						arg._0024VB_0024Local_selectedIPL = "apa_v_mp_h_04_a";
						break;
					case 80:
						arg._0024VB_0024Local_selectedIPL = "apa_v_mp_h_04_b";
						break;
					case 81:
						arg._0024VB_0024Local_selectedIPL = "apa_v_mp_h_04_c";
						break;
					}
				}
				else if (Operators.ConditionalCompareObjectEqual(tag, "Monochrome", false))
				{
					switch (ID)
					{
					case 79:
						arg._0024VB_0024Local_selectedIPL = "apa_v_mp_h_05_a";
						break;
					case 80:
						arg._0024VB_0024Local_selectedIPL = "apa_v_mp_h_05_b";
						break;
					case 81:
						arg._0024VB_0024Local_selectedIPL = "apa_v_mp_h_05_c";
						break;
					}
				}
				else if (Operators.ConditionalCompareObjectEqual(tag, "Seductive", false))
				{
					switch (ID)
					{
					case 79:
						arg._0024VB_0024Local_selectedIPL = "apa_v_mp_h_06_a";
						break;
					case 80:
						arg._0024VB_0024Local_selectedIPL = "apa_v_mp_h_06_b";
						break;
					case 81:
						arg._0024VB_0024Local_selectedIPL = "apa_v_mp_h_06_c";
						break;
					}
				}
				else if (Operators.ConditionalCompareObjectEqual(tag, "Regal", false))
				{
					switch (ID)
					{
					case 79:
						arg._0024VB_0024Local_selectedIPL = "apa_v_mp_h_07_a";
						break;
					case 80:
						arg._0024VB_0024Local_selectedIPL = "apa_v_mp_h_07_b";
						break;
					case 81:
						arg._0024VB_0024Local_selectedIPL = "apa_v_mp_h_07_c";
						break;
					}
				}
				else if (Operators.ConditionalCompareObjectEqual(tag, "Aqua", false))
				{
					switch (ID)
					{
					case 79:
						arg._0024VB_0024Local_selectedIPL = "apa_v_mp_h_08_a";
						break;
					case 80:
						arg._0024VB_0024Local_selectedIPL = "apa_v_mp_h_08_b";
						break;
					case 81:
						arg._0024VB_0024Local_selectedIPL = "apa_v_mp_h_08_c";
						break;
					}
				}
				arg._0024VB_0024Local_section = PropertyProfiles.Section("IPL", arg._0024VB_0024Local_owner);
				using (AtomicFileTransaction atomicFileTransaction = AtomicFileTransaction.BeginWrite("scripts\\SPA II\\modconfig.ini", arg._Lambda_0024__0, arg._Lambda_0024__1))
				{
					atomicFileTransaction.Complete();
				}
				ConfigHelper.config = ScriptSettings.Load("scripts\\SPA II\\modconfig.ini");
				Helper.ChangeIPL(IPL, arg._0024VB_0024Local_selectedIPL);
				IPL = arg._0024VB_0024Local_selectedIPL;
				foreach (UIMenuItem menuItem in sender.MenuItems)
				{
					menuItem.SetRightBadge(UIMenuItem.BadgeStyle.None);
				}
				selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Tick);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"{ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		private void StyleMenu_OnIndexChange(UIMenu sender, int newIndex)
		{
			try
			{
				if (Owner() != PropertyProfiles.ActiveCharacter || PropertyProfiles.ActiveCharacter != Helper.GetPlayer())
				{
					return;
				}
				Helper.FadeScreen(1);
				OldIPL = IPL;
				object tag = sender.MenuItems[newIndex].Tag;
				if (Operators.ConditionalCompareObjectEqual(tag, "Modern", false))
				{
					switch (ID)
					{
					case 79:
						IPL = "apa_v_mp_h_01_a";
						break;
					case 80:
						IPL = "apa_v_mp_h_01_b";
						break;
					case 81:
						IPL = "apa_v_mp_h_01_c";
						break;
					}
				}
				else if (Operators.ConditionalCompareObjectEqual(tag, "Moody", false))
				{
					switch (ID)
					{
					case 79:
						IPL = "apa_v_mp_h_02_a";
						break;
					case 80:
						IPL = "apa_v_mp_h_02_b";
						break;
					case 81:
						IPL = "apa_v_mp_h_02_c";
						break;
					}
				}
				else if (Operators.ConditionalCompareObjectEqual(tag, "Vibrant", false))
				{
					switch (ID)
					{
					case 79:
						IPL = "apa_v_mp_h_03_a";
						break;
					case 80:
						IPL = "apa_v_mp_h_03_b";
						break;
					case 81:
						IPL = "apa_v_mp_h_03_c";
						break;
					}
				}
				else if (Operators.ConditionalCompareObjectEqual(tag, "Sharp", false))
				{
					switch (ID)
					{
					case 79:
						IPL = "apa_v_mp_h_04_a";
						break;
					case 80:
						IPL = "apa_v_mp_h_04_b";
						break;
					case 81:
						IPL = "apa_v_mp_h_04_c";
						break;
					}
				}
				else if (Operators.ConditionalCompareObjectEqual(tag, "Monochrome", false))
				{
					switch (ID)
					{
					case 79:
						IPL = "apa_v_mp_h_05_a";
						break;
					case 80:
						IPL = "apa_v_mp_h_05_b";
						break;
					case 81:
						IPL = "apa_v_mp_h_05_c";
						break;
					}
				}
				else if (Operators.ConditionalCompareObjectEqual(tag, "Seductive", false))
				{
					switch (ID)
					{
					case 79:
						IPL = "apa_v_mp_h_06_a";
						break;
					case 80:
						IPL = "apa_v_mp_h_06_b";
						break;
					case 81:
						IPL = "apa_v_mp_h_06_c";
						break;
					}
				}
				else if (Operators.ConditionalCompareObjectEqual(tag, "Regal", false))
				{
					switch (ID)
					{
					case 79:
						IPL = "apa_v_mp_h_07_a";
						break;
					case 80:
						IPL = "apa_v_mp_h_07_b";
						break;
					case 81:
						IPL = "apa_v_mp_h_07_c";
						break;
					}
				}
				else if (Operators.ConditionalCompareObjectEqual(tag, "Aqua", false))
				{
					switch (ID)
					{
					case 79:
						IPL = "apa_v_mp_h_08_a";
						break;
					case 80:
						IPL = "apa_v_mp_h_08_b";
						break;
					case 81:
						IPL = "apa_v_mp_h_08_c";
						break;
					}
				}
				Helper.ChangeIPL(OldIPL, IPL);
				Helper.FadeScreen(0);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"{ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		private void StyleMenu_OnMenuClose(UIMenu sender)
		{
			Helper.FadeScreen(1);
			string text = ProfileIPL();
			if (Operators.CompareString(IPL, text, false) != 0)
			{
				Helper.ChangeIPL(IPL, text);
			}
			IPL = text;
			Stability.DestroySpaCameras();
			World.RenderingCamera = null;
			Helper.HideHud = false;
			Helper.FadeScreen(0);
		}

		public void PlayApartmentEnterCutscene()
		{
			try
			{
				Helper.HideHud = true;
				Door.UnlockDoor();
				World.RenderingCamera = Stability.CreateCam(EnterCam.Position, EnterCam.Rotation, EnterCam.FOV);
				Game.Player.Character.Position = Helper.ToVector3(ApartmentDoorPos);
				Game.Player.Character.Heading = ApartmentDoorPos.W;
				V2Compat.GoTo(Game.Player.Character.Task, ApartmentInPos, true, 6000);
				Script.Wait(6000);
				Door.LockDoor();
				Stability.DestroySpaCameras();
				World.RenderingCamera = null;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"{ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		public void PlayApartmerntExitCutscene()
		{
			try
			{
				Door.UnlockDoor();
				World.RenderingCamera = Stability.CreateCam(ExitCam.Position, ExitCam.Rotation, ExitCam.FOV);
				Game.Player.Character.Position = ApartmentInPos;
				Game.Player.Character.Heading = ApartmentDoorPos.W - 180f;
				V2Compat.GoTo(Game.Player.Character.Task, Helper.ToVector3(ApartmentDoorPos), true, 3500);
				Script.Wait(3500);
				Door.LockDoor();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"{ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		[SpecialName]
		[CompilerGenerated]
		private bool _Lambda_0024__35_002D2(VB_0024AnonymousType_0<BuildingClass, ApartmentClass> _0024VB_0024It)
		{
			return _0024VB_0024It.a.ID == ID;
		}
	}
}
