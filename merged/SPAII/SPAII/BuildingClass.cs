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
using Metadata;
using Microsoft.VisualBasic.CompilerServices;
using SPAII.INM;

namespace SPAII
{
	public class BuildingClass
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<ApartmentClass, bool> _0024I48_002D0;

			public static Func<ApartmentClass, ApartmentClass> _0024I48_002D1;

			public static Func<ApartmentClass, eOwner> _0024I49_002D0;

			public static Func<ApartmentClass, eOwner> _0024I50_002D0;

			public static Func<bool> _0024I85_002D6;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__48_002D0(ApartmentClass a)
			{
				return a.Owner() != eOwner.Nobody;
			}

			[SpecialName]
			internal ApartmentClass _Lambda_0024__48_002D1(ApartmentClass a)
			{
				return a;
			}

			[SpecialName]
			internal eOwner _Lambda_0024__49_002D0(ApartmentClass apartment)
			{
				return apartment.Owner();
			}

			[SpecialName]
			internal eOwner _Lambda_0024__50_002D0(ApartmentClass apartment)
			{
				return apartment.Owner();
			}

			[SpecialName]
			internal bool _Lambda_0024__85_002D6()
			{
				return true;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__85_002D0
		{
			public int _0024VB_0024Local_oldId;

			public int _0024VB_0024Local_oldUid;

			public string _0024VB_0024Local_directoryPath;

			public bool _0024VB_0024Local_executive;

			public Func<int, string> _0024VB_0024Local_destinationPath;

			public bool _0024VB_0024Local_expansionSource;

			public HashSet<int> _0024VB_0024Local_reserved;

			public _Closure_0024__85_002D1 _0024VB_0024NonLocal__0024VB_0024Closure_2;

			public _Closure_0024__85_002D0(_Closure_0024__85_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_oldId = arg0._0024VB_0024Local_oldId;
					_0024VB_0024Local_oldUid = arg0._0024VB_0024Local_oldUid;
					_0024VB_0024Local_directoryPath = arg0._0024VB_0024Local_directoryPath;
					_0024VB_0024Local_executive = arg0._0024VB_0024Local_executive;
					_0024VB_0024Local_destinationPath = arg0._0024VB_0024Local_destinationPath;
					_0024VB_0024Local_expansionSource = arg0._0024VB_0024Local_expansionSource;
					_0024VB_0024Local_reserved = arg0._0024VB_0024Local_reserved;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(VehicleClass v)
			{
				if (_0024VB_0024Local_oldId == _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_selectedApt.ID)
				{
					return v.UniqueID == _0024VB_0024Local_oldUid;
				}
				return false;
			}

			[SpecialName]
			internal string _Lambda_0024__2(int recordUid)
			{
				return Path.Combine(_0024VB_0024Local_directoryPath, (_0024VB_0024Local_executive ? recordUid.ToString(CultureInfo.InvariantCulture) : Conversions.ToString(recordUid)) + ".xml");
			}

			[SpecialName]
			internal bool _Lambda_0024__3(ApartmentClass sourceApartment)
			{
				if (sourceApartment != null && sourceApartment.ID == _0024VB_0024Local_oldId)
				{
					return PropertySale.RegisteredApartment(sourceApartment);
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__4(int candidateUid)
			{
				string text = _0024VB_0024Local_destinationPath(candidateUid);
				if (!File.Exists(text))
				{
					if (_0024VB_0024Local_expansionSource || _0024VB_0024Local_reserved != null)
					{
						return File.Exists(text + ".bak");
					}
					return false;
				}
				return true;
			}

			[SpecialName]
			internal bool _Lambda_0024__5(VehicleClass v)
			{
				return v.UniqueID == _0024VB_0024Local_oldUid;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__85_002D1
		{
			public ApartmentClass _0024VB_0024Local_selectedApt;

			public _Closure_0024__85_002D1(_Closure_0024__85_002D1 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_selectedApt = arg0._0024VB_0024Local_selectedApt;
				}
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__85_002D2
		{
			public int _0024VB_0024Local_candidateIndex;

			public _Closure_0024__85_002D2(_Closure_0024__85_002D2 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_candidateIndex = arg0._0024VB_0024Local_candidateIndex;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__1(VehicleClass v)
			{
				return v.Index == _0024VB_0024Local_candidateIndex;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__88_002D0
		{
			public ApartmentClass _0024VB_0024Local_selectedApt;

			public BuildingClass _0024VB_0024Me;
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__88_002D1
		{
			public Vehicle _0024VB_0024Local_driven;

			public _Closure_0024__88_002D0 _0024VB_0024NonLocal__0024VB_0024Closure_2;

			[SpecialName]
			internal int _Lambda_0024__0()
			{
				BuildingClass _0024VB_0024Me = _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Me;
				ApartmentClass _0024VB_0024Local_selectedApt = _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_selectedApt;
				Vehicle vehicle = _0024VB_0024Local_driven;
				int storedSlot = -1;
				int uid = default(int);
				if (!_0024VB_0024Me.TryStoreApartmentVehicle(_0024VB_0024Local_selectedApt, vehicle, ref uid, ref storedSlot))
				{
					return 0;
				}
				return uid;
			}
		}

		public string Name;

		public Quaternion BuildingInPos;

		public Quaternion BuildingLobby;

		public Quaternion BuildingOutPos;

		public Vector3 GarageInPos;

		public Quaternion GarageFootInPos;

		public Quaternion GarageFootOutPos;

		public Quaternion GarageOutPos;

		public bool BuyAtGarageEntrance;

		public CameraPRH CameraPos;

		public CameraPRH EnterCamera1;

		public CameraPRH EnterCamera2;

		public CameraPRH EnterCamera3;

		public CameraPRH EnterCamera4;

		public eGarageType GarageType;

		public eBuildingType BuildingType;

		public List<ApartmentClass> Apartments;

		public string[] HideObjects;

		public EntityVector SaleSign;

		public eFrontDoor FrontDoor;

		public Door Door1;

		public Door Door2;

		public eFrontDoor GarageDoor;

		public Door Door3;

		public Quaternion GarageWaypoint;

		public Blip BuildingBlip;

		public Blip GarageBlip;

		public Prop SaleSignProp;

		private bool saleSignRemovalPending;

		private int nextSaleSignRemovalAttempt;

		private int nextExecutiveSaleSignCheck;

		private int ownedSaleSignHandle;

		private int ownedSaleSignModel;

		private IntPtr ownedSaleSignAddress;

		public Vector3 SaleSignPoint;

		public static BuildingClass HiddenFor = null;

		public static Vector3 HiddenAt;

		[System.Runtime.CompilerServices.AccessedThroughProperty("BuyMenu")]
		private UIMenu _BuyMenu;

		public virtual UIMenu BuyMenu
		{
			[CompilerGenerated]
			get
			{
				return _BuyMenu;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				MenuCloseEvent value2 = AptMenu_OnMenuClose;
				ItemSelectEvent value3 = BuyMenu_OnItemSelect;
				UIMenu buyMenu = _BuyMenu;
				if (buyMenu != null)
				{
					buyMenu.OnMenuClose -= value2;
					buyMenu.OnItemSelect -= value3;
				}
				_BuyMenu = value;
				buyMenu = _BuyMenu;
				if (buyMenu != null)
				{
					buyMenu.OnMenuClose += value2;
					buyMenu.OnItemSelect += value3;
				}
			}
		}

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
				MenuCloseEvent value3 = AptMenu_OnMenuClose;
				UIMenu aptMenu = _AptMenu;
				if (aptMenu != null)
				{
					aptMenu.OnItemSelect -= value2;
					aptMenu.OnMenuClose -= value3;
				}
				_AptMenu = value;
				aptMenu = _AptMenu;
				if (aptMenu != null)
				{
					aptMenu.OnItemSelect += value2;
					aptMenu.OnMenuClose += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("GrgMenu")]
		private UIMenu _GrgMenu;

		public virtual UIMenu GrgMenu
		{
			[CompilerGenerated]
			get
			{
				return _GrgMenu;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = GrgMenu_OnItemSelect;
				MenuCloseEvent value3 = GrgMenu_OnMenuClose;
				UIMenu grgMenu = _GrgMenu;
				if (grgMenu != null)
				{
					grgMenu.OnItemSelect -= value2;
					grgMenu.OnMenuClose -= value3;
				}
				_GrgMenu = value;
				grgMenu = _GrgMenu;
				if (grgMenu != null)
				{
					grgMenu.OnItemSelect += value2;
					grgMenu.OnMenuClose += value3;
				}
			}
		}

		public BuildingClass()
		{
			SaleSignPoint = Vector3.Zero;
		}

		public bool IsVacant()
		{
			return Apartments.Where((_Closure_0024__._0024I48_002D0 != null) ? _Closure_0024__._0024I48_002D0 : (_Closure_0024__._0024I48_002D0 = _Closure_0024__._0024I._Lambda_0024__48_002D0)).Select((_Closure_0024__._0024I48_002D1 != null) ? _Closure_0024__._0024I48_002D1 : (_Closure_0024__._0024I48_002D1 = _Closure_0024__._0024I._Lambda_0024__48_002D1)).Count() == 0;
		}

		private string OwnedMapLabel(string prefix)
		{
			bool german = Game.Language == Language.German;
			eOwner[] array = Apartments.Select((_Closure_0024__._0024I49_002D0 != null) ? _Closure_0024__._0024I49_002D0 : (_Closure_0024__._0024I49_002D0 = _Closure_0024__._0024I._Lambda_0024__49_002D0)).Distinct().ToArray();
			string text = prefix + " - " + PropertyOwnershipText.OwnerLabel(array, german);
			if (array.Contains(eOwner.Nobody))
			{
				text = text + " / " + PropertyOwnershipText.ForSaleLabel(german);
			}
			return text;
		}

		internal string LockedEntranceLabel(eOwner viewer, bool german)
		{
			eOwner[] array = Apartments.Select((_Closure_0024__._0024I50_002D0 != null) ? _Closure_0024__._0024I50_002D0 : (_Closure_0024__._0024I50_002D0 = _Closure_0024__._0024I._Lambda_0024__50_002D0)).Distinct().ToArray();
			if (array.Length == 0 || array.Contains(viewer) || array.Contains(eOwner.Nobody))
			{
				return null;
			}
			return PropertyOwnershipText.OwnerLabel(array, german);
		}

		public void Load()
		{
			if (!ClearOwnedMapBlips())
			{
				return;
			}
			if (!IsVacant() && BuildingType != eBuildingType.Garage && GarageType != 0)
			{
				GarageBlip = World.CreateBlip(GarageInPos);
				Blip garageBlip = GarageBlip;
				garageBlip.IsShortRange = true;
				garageBlip.Sprite = BlipSprite.Garage;
				garageBlip.Name = OwnedMapLabel(Game.GetLocalizedString("BLIP_357"));
				garageBlip.Color = Helper.GetBlipColor(this);
				SpaBlips.RegisterCoordinate(GarageBlip);
			}
			BuildingBlip = World.CreateBlip((GarageType == eGarageType.SixtyCarGarage && !IsVacant()) ? GarageAccessPoint(true) : SouthLSHousingExterior.MapEntrancePosition(this));
			Blip buildingBlip = BuildingBlip;
			buildingBlip.IsShortRange = true;
			if (IsVacant())
			{
				if (!ConfigHelper.DebugMode)
				{
					switch (BuildingType)
					{
					case eBuildingType.Apartment:
						buildingBlip.Sprite = BlipSprite.SafehouseForSale;
						buildingBlip.Name = (SouthLSHousingExterior.IsCustom(this) ? (Name + " - " + Game.GetLocalizedString("MP_PROP_SALE1")) : Game.GetLocalizedString("MP_PROP_SALE1"));
						break;
					case eBuildingType.Office:
						buildingBlip.Sprite = BlipSprite.OfficeForSale;
						buildingBlip.Name = Game.GetLocalizedString("MP_PROP_SALE2");
						break;
					case eBuildingType.ClubHouse:
					case eBuildingType.NightClub:
					case eBuildingType.Bunker:
						buildingBlip.Sprite = BlipSprite.BusinessForSale;
						buildingBlip.Name = Game.GetLocalizedString("BLIP_373");
						break;
					case eBuildingType.Garage:
						buildingBlip.Sprite = BlipSprite.GarageForSale;
						buildingBlip.Name = Game.GetLocalizedString("MP_PROP_SALE0");
						break;
					case eBuildingType.Hangar:
						buildingBlip.Sprite = BlipSprite.HangarForSale;
						buildingBlip.Name = Game.GetLocalizedString("BLIP_372");
						break;
					case eBuildingType.Warehouse:
						buildingBlip.Sprite = BlipSprite.WarehouseForSale;
						buildingBlip.Name = Game.GetLocalizedString("BLIP_474");
						break;
					}
				}
				else
				{
					switch (BuildingType)
					{
					case eBuildingType.Apartment:
						buildingBlip.Sprite = BlipSprite.Safehouse;
						break;
					case eBuildingType.Office:
						buildingBlip.Sprite = BlipSprite.Office;
						break;
					case eBuildingType.ClubHouse:
						buildingBlip.Sprite = BlipSprite.BikerClubhouse;
						break;
					case eBuildingType.Garage:
						buildingBlip.Sprite = BlipSprite.Garage;
						break;
					case eBuildingType.Hangar:
						buildingBlip.Sprite = BlipSprite.GTAOHangar;
						break;
					case eBuildingType.NightClub:
						buildingBlip.Sprite = BlipSprite.NightclubProperty;
						break;
					case eBuildingType.Warehouse:
						buildingBlip.Sprite = BlipSprite.Warehouse;
						break;
					case eBuildingType.Bunker:
						buildingBlip.Sprite = BlipSprite.Bunker;
						break;
					}
					buildingBlip.Name = Name;
				}
				buildingBlip.Color = BlipColor.White;
			}
			else
			{
				switch (BuildingType)
				{
				case eBuildingType.Apartment:
					buildingBlip.Sprite = BlipSprite.Safehouse;
					buildingBlip.Name = (SouthLSHousingExterior.IsCustom(this) ? Name : Game.GetLocalizedString("CELL_2630"));
					break;
				case eBuildingType.Office:
					buildingBlip.Sprite = BlipSprite.Office;
					buildingBlip.Name = Game.GetLocalizedString("BLIP_475");
					break;
				case eBuildingType.ClubHouse:
					buildingBlip.Sprite = BlipSprite.BikerClubhouse;
					buildingBlip.Name = Game.GetLocalizedString("PM_SPAWN_CLUBH");
					break;
				case eBuildingType.Garage:
					buildingBlip.Sprite = BlipSprite.Garage;
					buildingBlip.Name = Game.GetLocalizedString("BLIP_357");
					break;
				case eBuildingType.Hangar:
					buildingBlip.Sprite = BlipSprite.GTAOHangar;
					buildingBlip.Name = Game.GetLocalizedString("BLIP_359");
					break;
				case eBuildingType.NightClub:
					buildingBlip.Sprite = BlipSprite.NightclubProperty;
					buildingBlip.Name = Game.GetLocalizedString("CELL_CLUB");
					break;
				case eBuildingType.Warehouse:
					buildingBlip.Sprite = BlipSprite.Warehouse;
					buildingBlip.Name = Game.GetLocalizedString("BLIP_473");
					break;
				case eBuildingType.Bunker:
					buildingBlip.Sprite = BlipSprite.Bunker;
					buildingBlip.Name = Game.GetLocalizedString("BLIP_557");
					break;
				}
				buildingBlip.Color = Helper.GetBlipColor(this);
			}
			if (!IsVacant())
			{
				buildingBlip.Name = OwnedMapLabel(Name);
			}
			else if (GarageType == eGarageType.SixtyCarGarage)
			{
				buildingBlip.Name = Name;
			}
			buildingBlip = null;
			SpaBlips.RegisterCoordinate(BuildingBlip);
			BuyMenu = new UIMenu("", Game.GetLocalizedString("MP_PROP_GEN0"), new Point(0, -107));
			BuyMenu.SetBannerType(Helper.MenuBanner);
			BuyMenu.MouseEdgeEnabled = false;
			Helper.MenuPool.Add(BuyMenu);
			UIMenu buyMenu = BuyMenu;
			foreach (ApartmentClass apartment in Apartments)
			{
				UIMenuItem uIMenuItem = new UIMenuItem(apartment.FriendlyName(), Helper.Truncate(apartment.FriendlyDescription()));
				UIMenuItem uIMenuItem2 = uIMenuItem;
				switch (apartment.Owner())
				{
				case eOwner.Nobody:
					uIMenuItem2.SetRightLabel(string.Format("${0}", apartment.Price.ToString("0,0")));
					uIMenuItem2.SetRightBadge(UIMenuItem.BadgeStyle.None);
					break;
				case eOwner.Michael:
					uIMenuItem2.SetRightBadge(UIMenuItem.BadgeStyle.Michael);
					uIMenuItem2.SetRightLabel("Michael");
					break;
				case eOwner.Franklin:
					uIMenuItem2.SetRightBadge(UIMenuItem.BadgeStyle.Franklin);
					uIMenuItem2.SetRightLabel("Franklin");
					break;
				case eOwner.Trevor:
					uIMenuItem2.SetRightBadge(UIMenuItem.BadgeStyle.Trevor);
					uIMenuItem2.SetRightLabel("Trevor");
					break;
				case eOwner.Others:
					uIMenuItem2.SetRightBadge(UIMenuItem.BadgeStyle.Heart);
					uIMenuItem2.SetRightLabel("Owned");
					break;
				}
				uIMenuItem2.Tag = apartment;
				uIMenuItem2 = null;
				buyMenu.AddItem(uIMenuItem);
			}
			buyMenu.RefreshIndex();
			buyMenu = null;
			AptMenu = new UIMenu("", Game.GetLocalizedString("MP_PROP_GEN2A"), new Point(0, -107));
			AptMenu.SetBannerType(Helper.MenuBanner);
			AptMenu.MouseEdgeEnabled = false;
			Helper.MenuPool.Add(AptMenu);
			UIMenu aptMenu = AptMenu;
			foreach (ApartmentClass apartment2 in Apartments)
			{
				UIMenuItem uIMenuItem3 = new UIMenuItem(apartment2.FriendlyName(), Helper.Truncate(apartment2.FriendlyDescription()));
				UIMenuItem uIMenuItem4 = uIMenuItem3;
				switch (apartment2.Owner())
				{
				case eOwner.Nobody:
					uIMenuItem4.SetRightBadge(UIMenuItem.BadgeStyle.None);
					break;
				case eOwner.Michael:
					uIMenuItem4.SetRightBadge(UIMenuItem.BadgeStyle.Michael);
					uIMenuItem4.SetRightLabel("Michael");
					break;
				case eOwner.Franklin:
					uIMenuItem4.SetRightBadge(UIMenuItem.BadgeStyle.Franklin);
					uIMenuItem4.SetRightLabel("Franklin");
					break;
				case eOwner.Trevor:
					uIMenuItem4.SetRightBadge(UIMenuItem.BadgeStyle.Trevor);
					uIMenuItem4.SetRightLabel("Trevor");
					break;
				case eOwner.Others:
					uIMenuItem4.SetRightBadge(UIMenuItem.BadgeStyle.Heart);
					uIMenuItem4.SetRightLabel("Owned");
					break;
				}
				uIMenuItem4.Tag = apartment2;
				uIMenuItem4 = null;
				aptMenu.AddItem(uIMenuItem3);
			}
			aptMenu.RefreshIndex();
			aptMenu = null;
			GrgMenu = new UIMenu("", Game.GetLocalizedString("MP_PROP_GEN2B"), new Point(0, -107));
			GrgMenu.SetBannerType(Helper.MenuBanner);
			GrgMenu.MouseEdgeEnabled = false;
			Helper.MenuPool.Add(GrgMenu);
			UIMenu grgMenu = GrgMenu;
			foreach (ApartmentClass apartment3 in Apartments)
			{
				UIMenuItem uIMenuItem5 = new UIMenuItem(apartment3.FriendlyName(), Helper.Truncate(apartment3.FriendlyDescription()));
				UIMenuItem uIMenuItem6 = uIMenuItem5;
				switch (apartment3.Owner())
				{
				case eOwner.Nobody:
					uIMenuItem6.SetRightBadge(UIMenuItem.BadgeStyle.None);
					break;
				case eOwner.Michael:
					uIMenuItem6.SetRightBadge(UIMenuItem.BadgeStyle.Michael);
					uIMenuItem6.SetRightLabel("Michael");
					break;
				case eOwner.Franklin:
					uIMenuItem6.SetRightBadge(UIMenuItem.BadgeStyle.Franklin);
					uIMenuItem6.SetRightLabel("Franklin");
					break;
				case eOwner.Trevor:
					uIMenuItem6.SetRightBadge(UIMenuItem.BadgeStyle.Trevor);
					uIMenuItem6.SetRightLabel("Trevor");
					break;
				case eOwner.Others:
					uIMenuItem6.SetRightBadge(UIMenuItem.BadgeStyle.Heart);
					uIMenuItem6.SetRightLabel("Owned");
					break;
				}
				uIMenuItem6.Tag = apartment3;
				uIMenuItem6 = null;
				grgMenu.AddItem(uIMenuItem5);
			}
			grgMenu.RefreshIndex();
			grgMenu = null;
			foreach (ApartmentClass apartment4 in Apartments)
			{
				apartment4.Load();
				if (!Helper.apartments.Contains(apartment4))
				{
					Helper.apartments.Add(apartment4);
				}
			}
		}

		private bool HasApartmentForSale()
		{
			if (Apartments == null)
			{
				return false;
			}
			foreach (ApartmentClass apartment in Apartments)
			{
				if (apartment.Owner() == eOwner.Nobody)
				{
					return true;
				}
			}
			return false;
		}

		private Prop ResolveOwnedSaleSign()
		{
			if (ownedSaleSignHandle == 0 || ownedSaleSignModel == 0 || ownedSaleSignAddress == IntPtr.Zero || !Function.Call<bool>(Hash.DOES_ENTITY_EXIST, ownedSaleSignHandle))
			{
				return null;
			}
			Prop prop = Entity.FromHandle(ownedSaleSignHandle) as Prop;
			if ((object)prop == null || !prop.Exists() || prop.Handle != ownedSaleSignHandle || prop.Model.Hash != ownedSaleSignModel || prop.MemoryAddress != ownedSaleSignAddress)
			{
				return null;
			}
			return prop;
		}

		private void RemoveOwnedSaleSign()
		{
			try
			{
				if (ownedSaleSignHandle == 0)
				{
					if ((object)SaleSignProp != null && SaleSignProp.Exists())
					{
						if (!saleSignRemovalPending)
						{
							Logger.Log("Apartment sale sign has no creation receipt: " + Name);
						}
						saleSignRemovalPending = true;
						return;
					}
				}
				else
				{
					Prop prop = ResolveOwnedSaleSign();
					if ((object)prop != null)
					{
						prop.Delete();
						if ((object)ResolveOwnedSaleSign() != null)
						{
							throw new InvalidOperationException("The created sale sign still exists after deletion.");
						}
					}
				}
				ownedSaleSignHandle = 0;
				ownedSaleSignModel = 0;
				ownedSaleSignAddress = IntPtr.Zero;
				SaleSignProp = null;
				SaleSignPoint = Vector3.Zero;
				saleSignRemovalPending = false;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				if (!saleSignRemovalPending)
				{
					Logger.Log("Apartment sale sign removal will retry: " + Name + "; " + ex2.Message);
				}
				saleSignRemovalPending = true;
				ProjectData.ClearProjectError();
			}
		}

		public void RefreshSaleSign()
		{
			if (SouthLSHousingExterior.IsCustom(this))
			{
				SaleSignPoint = Helper.ToVector3(BuildingInPos);
			}
			else if (HasApartmentForSale())
			{
				saleSignRemovalPending = false;
				SpawnForSaleSigns();
			}
			else
			{
				RemoveOwnedSaleSign();
			}
		}

		public void SpawnForSaleSigns()
		{
			if (SouthLSHousingExterior.IsCustom(this))
			{
				SaleSignPoint = Helper.ToVector3(BuildingInPos);
				return;
			}
			if (!HasApartmentForSale())
			{
				RemoveOwnedSaleSign();
				return;
			}
			try
			{
				if ((object)ResolveOwnedSaleSign() != null)
				{
					saleSignRemovalPending = false;
					return;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Apartment sale sign identity check will retry: " + Name + "; " + ex2.Message);
				ProjectData.ClearProjectError();
				return;
			}
			try
			{
				bool flag = GarageType == eGarageType.SixtyCarGarage;
				SaleSignProp = World.CreateProp(SaleSign.Model, Helper.ToVector3(SaleSign.Position), new Vector3(0f, 0f, SaleSign.Position.W), !flag, false);
				if ((object)SaleSignProp != null && SaleSignProp.Exists())
				{
					ownedSaleSignHandle = SaleSignProp.Handle;
					ownedSaleSignModel = SaleSignProp.Model.Hash;
					ownedSaleSignAddress = SaleSignProp.MemoryAddress;
					SaleSignProp.IsPersistent = true;
					SaleSignProp.IsPositionFrozen = true;
					if (flag)
					{
						SaleSignProp.IsCollisionEnabled = false;
						SaleSignPoint = GarageAccessPoint(false);
					}
					else
					{
						SaleSignPoint = SaleSignProp.Position - SaleSignProp.RightVector;
					}
					return;
				}
			}
			catch (Exception ex3)
			{
				ProjectData.SetProjectError(ex3);
				Exception ex4 = ex3;
				Logger.Log($"SpawnForSaleSigns {Name}: {ex4.Message}");
				ProjectData.ClearProjectError();
			}
			if ((object)SaleSignProp == null || !SaleSignProp.Exists())
			{
				SaleSignProp = null;
				Logger.Log($"SpawnForSaleSigns {Name}: sign could not be created (object pool full or model missing)");
			}
		}

		public float SaleSignDistanceFrom(Vector3 p, float entranceSq)
		{
			checked
			{
				if (saleSignRemovalPending && Game.GameTime >= nextSaleSignRemovalAttempt)
				{
					nextSaleSignRemovalAttempt = Game.GameTime + 2000;
					RemoveOwnedSaleSign();
				}
				if (!HasApartmentForSale())
				{
					if (ownedSaleSignHandle != 0 && !saleSignRemovalPending)
					{
						RemoveOwnedSaleSign();
					}
					return float.MaxValue;
				}
				if (GarageType == eGarageType.SixtyCarGarage)
				{
					Vector3 vector = GarageAccessPoint(false);
					if (vector == Vector3.Zero || !PartyPlacement.FinitePoint(vector))
					{
						return float.MaxValue;
					}
					float num = p.DistanceToSquared(vector);
					if (num <= 400f && Game.GameTime >= nextExecutiveSaleSignCheck)
					{
						nextExecutiveSaleSignCheck = Game.GameTime + 2000;
						SpawnForSaleSigns();
					}
					return num;
				}
				if (SouthLSHousingExterior.IsCustom(this))
				{
					return entranceSq;
				}
				if (entranceSq <= 300f || SaleSignPoint == Vector3.Zero)
				{
					return p.DistanceToSquared(Helper.ToVector3(SaleSign.Position));
				}
				return p.DistanceToSquared(SaleSignPoint);
			}
		}

		internal Vector3 GarageAccessPoint(bool inVehicle)
		{
			if (!inVehicle)
			{
				return Helper.ToVector3(GarageFootInPos);
			}
			return GarageInPos;
		}

		public float GarageDistanceFrom(Vector3 footPos, Vector3 vehPos, bool inVehicle)
		{
			if (GarageType == eGarageType.NoGarage)
			{
				return float.MaxValue;
			}
			if (SouthLSHousingExterior.IsCustom(this) && !SouthLSHousingExterior.CanUseGarage(this))
			{
				return float.MaxValue;
			}
			Vector3 vector = GarageAccessPoint(inVehicle);
			if (vector == Vector3.Zero || !PartyPlacement.FinitePoint(vector))
			{
				return float.MaxValue;
			}
			return (inVehicle ? vehPos : footPos).DistanceToSquared(vector);
		}

		internal bool AtGaragePurchaseEntrance(Ped actor)
		{
			if (!BuyAtGarageEntrance || BuildingType != eBuildingType.Garage || (object)actor == null || !actor.Exists())
			{
				return false;
			}
			if (!actor.IsInVehicle())
			{
				return GarageDistanceFrom(actor.Position, Vector3.Zero, false) <= 5f;
			}
			Vehicle currentVehicle = actor.CurrentVehicle;
			if ((object)currentVehicle == null || !currentVehicle.Exists() || currentVehicle.IsDead)
			{
				return false;
			}
			Ped driver = currentVehicle.Driver;
			return (object)driver != null && driver.Handle == actor.Handle && GarageDistanceFrom(Vector3.Zero, currentVehicle.Position, true) <= 5f;
		}

		public void RefreshBuyMenu()
		{
			BuyMenu.MenuItems.Clear();
			foreach (ApartmentClass apartment in Apartments)
			{
				UIMenuItem uIMenuItem = new UIMenuItem(apartment.FriendlyName(), Helper.Truncate(apartment.FriendlyDescription()));
				UIMenuItem uIMenuItem2 = uIMenuItem;
				switch (apartment.Owner())
				{
				case eOwner.Nobody:
					uIMenuItem2.SetRightLabel(string.Format("${0}", apartment.Price.ToString("0,0")));
					uIMenuItem2.SetRightBadge(UIMenuItem.BadgeStyle.None);
					break;
				case eOwner.Michael:
					uIMenuItem2.SetRightBadge(UIMenuItem.BadgeStyle.Michael);
					uIMenuItem2.SetRightLabel("Michael");
					break;
				case eOwner.Franklin:
					uIMenuItem2.SetRightBadge(UIMenuItem.BadgeStyle.Franklin);
					uIMenuItem2.SetRightLabel("Franklin");
					break;
				case eOwner.Trevor:
					uIMenuItem2.SetRightBadge(UIMenuItem.BadgeStyle.Trevor);
					uIMenuItem2.SetRightLabel("Trevor");
					break;
				case eOwner.Others:
					uIMenuItem2.SetRightBadge(UIMenuItem.BadgeStyle.Heart);
					uIMenuItem2.SetRightLabel("Owned");
					break;
				}
				uIMenuItem2.Tag = apartment;
				uIMenuItem2 = null;
				BuyMenu.AddItem(uIMenuItem);
			}
			BuyMenu.RefreshIndex();
		}

		public void RefreshAptMenu()
		{
			AptMenu.MenuItems.Clear();
			foreach (ApartmentClass apartment in Apartments)
			{
				UIMenuItem uIMenuItem = new UIMenuItem(apartment.FriendlyName(), Helper.Truncate(apartment.FriendlyDescription()));
				UIMenuItem uIMenuItem2 = uIMenuItem;
				switch (apartment.Owner())
				{
				case eOwner.Nobody:
					uIMenuItem2.SetRightBadge(UIMenuItem.BadgeStyle.None);
					break;
				case eOwner.Michael:
					uIMenuItem2.SetRightBadge(UIMenuItem.BadgeStyle.Michael);
					uIMenuItem2.SetRightLabel("Michael");
					break;
				case eOwner.Franklin:
					uIMenuItem2.SetRightBadge(UIMenuItem.BadgeStyle.Franklin);
					uIMenuItem2.SetRightLabel("Franklin");
					break;
				case eOwner.Trevor:
					uIMenuItem2.SetRightBadge(UIMenuItem.BadgeStyle.Trevor);
					uIMenuItem2.SetRightLabel("Trevor");
					break;
				case eOwner.Others:
					uIMenuItem2.SetRightBadge(UIMenuItem.BadgeStyle.Heart);
					uIMenuItem2.SetRightLabel("Owned");
					break;
				}
				uIMenuItem2.Tag = apartment;
				uIMenuItem2 = null;
				AptMenu.AddItem(uIMenuItem);
			}
			AptMenu.RefreshIndex();
		}

		public void RefreshGrgMenu()
		{
			GrgMenu.MenuItems.Clear();
			foreach (ApartmentClass apartment in Apartments)
			{
				UIMenuItem uIMenuItem = new UIMenuItem(apartment.FriendlyName(), Helper.Truncate(apartment.FriendlyDescription()));
				UIMenuItem uIMenuItem2 = uIMenuItem;
				switch (apartment.Owner())
				{
				case eOwner.Nobody:
					uIMenuItem2.SetRightBadge(UIMenuItem.BadgeStyle.None);
					break;
				case eOwner.Michael:
					uIMenuItem2.SetRightBadge(UIMenuItem.BadgeStyle.Michael);
					uIMenuItem2.SetRightLabel("Michael");
					break;
				case eOwner.Franklin:
					uIMenuItem2.SetRightBadge(UIMenuItem.BadgeStyle.Franklin);
					uIMenuItem2.SetRightLabel("Franklin");
					break;
				case eOwner.Trevor:
					uIMenuItem2.SetRightBadge(UIMenuItem.BadgeStyle.Trevor);
					uIMenuItem2.SetRightLabel("Trevor");
					break;
				case eOwner.Others:
					uIMenuItem2.SetRightBadge(UIMenuItem.BadgeStyle.Heart);
					uIMenuItem2.SetRightLabel("Owned");
					break;
				}
				uIMenuItem2.Tag = apartment;
				uIMenuItem2 = null;
				GrgMenu.AddItem(uIMenuItem);
			}
			GrgMenu.RefreshIndex();
		}

		public float GarageDistance()
		{
			Ped character = Game.Player.Character;
			if (character.IsInVehicle())
			{
				return GarageDistanceFrom(Vector3.Zero, character.CurrentVehicle.Position, true);
			}
			return GarageDistanceFrom(character.Position, Vector3.Zero, false);
		}

		public float EntranceDistance()
		{
			return Game.Player.Character.Position.DistanceToSquared(Helper.ToVector3(BuildingInPos));
		}

		public float SaleSignDistance()
		{
			if (!HasApartmentForSale())
			{
				return float.MaxValue;
			}
			if (SouthLSHousingExterior.IsCustom(this))
			{
				return EntranceDistance();
			}
			if (EntranceDistance() <= 300f)
			{
				return Game.Player.Character.Position.DistanceToSquared(Helper.ToVector3(SaleSign.Position));
			}
			if (SaleSignProp != null)
			{
				return Game.Player.Character.Position.DistanceToSquared(SaleSignProp.Position - SaleSignProp.RightVector);
			}
			return Game.Player.Character.Position.DistanceToSquared(Helper.ToVector3(SaleSign.Position));
		}

		public bool IsForSale()
		{
			return ((0 - ((BuildingBlip.Sprite == BlipSprite.SafehouseForSale) ? 1 : 0)) | 0x1DC | 0x171) != 0;
		}

		public float GarageElevatorDistance(ApartmentClass apt)
		{
			return Game.Player.Character.Position.DistanceToSquared(apt.GarageElevatorPos);
		}

		public float GarageMenuDistance(ApartmentClass apt)
		{
			return Game.Player.Character.Position.DistanceToSquared(apt.GarageMenuPos);
		}

		public bool IsAtHome()
		{
			return Function.Call<bool>(Hash.IS_INTERIOR_SCENE);
		}

		public void HideExterior()
		{
			if (HideObjects == null || HideObjects.Count() == 0 || !IsAtHome() || HighEndApartment.Building != this)
			{
				return;
			}
			Function.Call(INMNative.DISABLE_OCCLUSION_THIS_FRAME);
			if (HiddenFor != this)
			{
				RestoreHiddenExterior();
				HiddenAt = Helper.PP.Position;
				string[] hideObjects = HideObjects;
				foreach (string input in hideObjects)
				{
					Function.Call(Hash.CREATE_MODEL_HIDE, HiddenAt.X, HiddenAt.Y, HiddenAt.Z, 150f, Game.GenerateHash(input), true);
				}
				HiddenFor = this;
				Logger.Log($"> hide exterior of {Name}: {HideObjects.Count()} models");
			}
		}

		public static void RestoreHiddenExterior()
		{
			if (HiddenFor != null)
			{
				string[] hideObjects = HiddenFor.HideObjects;
				foreach (string input in hideObjects)
				{
					Function.Call(Hash.REMOVE_MODEL_HIDE, HiddenAt.X, HiddenAt.Y, HiddenAt.Z, 150f, Game.GenerateHash(input), false);
				}
				HiddenFor = null;
			}
		}

		public static void ExteriorHideCheck()
		{
			if (HiddenFor != null && (HighEndApartment.Building != HiddenFor || !HiddenFor.IsAtHome()))
			{
				RestoreHiddenExterior();
			}
		}

		public bool ClearOwnedMapBlips()
		{
			bool result = true;
			if ((object)BuildingBlip != null)
			{
				try
				{
					if (BuildingBlip.Exists())
					{
						V2Compat.Remove(BuildingBlip);
					}
					BuildingBlip = null;
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					result = false;
					try
					{
						Logger.Log("Building map blip cleanup: " + ex2.Message);
					}
					catch (Exception projectError)
					{
						ProjectData.SetProjectError(projectError);
						ProjectData.ClearProjectError();
					}
					ProjectData.ClearProjectError();
				}
			}
			if ((object)GarageBlip != null)
			{
				try
				{
					if (GarageBlip.Exists())
					{
						V2Compat.Remove(GarageBlip);
					}
					GarageBlip = null;
					return result;
				}
				catch (Exception ex3)
				{
					ProjectData.SetProjectError(ex3);
					Exception ex4 = ex3;
					result = false;
					try
					{
						Logger.Log("Garage map blip cleanup: " + ex4.Message);
					}
					catch (Exception projectError2)
					{
						ProjectData.SetProjectError(projectError2);
						ProjectData.ClearProjectError();
					}
					ProjectData.ClearProjectError();
					return result;
				}
			}
			return result;
		}

		public void RefreshBlips()
		{
			if (!ClearOwnedMapBlips())
			{
				return;
			}
			if (!IsVacant() && BuildingType != eBuildingType.Garage && GarageType != 0)
			{
				GarageBlip = World.CreateBlip(GarageInPos);
				Blip garageBlip = GarageBlip;
				garageBlip.IsShortRange = true;
				garageBlip.Sprite = BlipSprite.Garage;
				garageBlip.Name = OwnedMapLabel(Game.GetLocalizedString("BLIP_357"));
				garageBlip.Color = Helper.GetBlipColor(this);
				SpaBlips.RegisterCoordinate(GarageBlip);
			}
			BuildingBlip = World.CreateBlip((GarageType == eGarageType.SixtyCarGarage && !IsVacant()) ? GarageAccessPoint(true) : SouthLSHousingExterior.MapEntrancePosition(this));
			Blip buildingBlip = BuildingBlip;
			buildingBlip.IsShortRange = true;
			if (IsVacant())
			{
				if (!ConfigHelper.DebugMode)
				{
					switch (BuildingType)
					{
					case eBuildingType.Apartment:
						buildingBlip.Sprite = BlipSprite.SafehouseForSale;
						buildingBlip.Name = (SouthLSHousingExterior.IsCustom(this) ? (Name + " - " + Game.GetLocalizedString("MP_PROP_SALE1")) : Game.GetLocalizedString("MP_PROP_SALE1"));
						break;
					case eBuildingType.Office:
						buildingBlip.Sprite = BlipSprite.OfficeForSale;
						buildingBlip.Name = Game.GetLocalizedString("MP_PROP_SALE2");
						break;
					case eBuildingType.ClubHouse:
					case eBuildingType.NightClub:
					case eBuildingType.Bunker:
						buildingBlip.Sprite = BlipSprite.BusinessForSale;
						buildingBlip.Name = Game.GetLocalizedString("BLIP_373");
						break;
					case eBuildingType.Garage:
						buildingBlip.Sprite = BlipSprite.GarageForSale;
						buildingBlip.Name = Game.GetLocalizedString("MP_PROP_SALE0");
						break;
					case eBuildingType.Hangar:
						buildingBlip.Sprite = BlipSprite.HangarForSale;
						buildingBlip.Name = Game.GetLocalizedString("BLIP_372");
						break;
					case eBuildingType.Warehouse:
						buildingBlip.Sprite = BlipSprite.WarehouseForSale;
						buildingBlip.Name = Game.GetLocalizedString("BLIP_474");
						break;
					}
				}
				else
				{
					switch (BuildingType)
					{
					case eBuildingType.Apartment:
						buildingBlip.Sprite = BlipSprite.Safehouse;
						break;
					case eBuildingType.Office:
						buildingBlip.Sprite = BlipSprite.Office;
						break;
					case eBuildingType.ClubHouse:
						buildingBlip.Sprite = BlipSprite.BikerClubhouse;
						break;
					case eBuildingType.Garage:
						buildingBlip.Sprite = BlipSprite.Garage;
						break;
					case eBuildingType.Hangar:
						buildingBlip.Sprite = BlipSprite.GTAOHangar;
						break;
					case eBuildingType.NightClub:
						buildingBlip.Sprite = BlipSprite.NightclubProperty;
						break;
					case eBuildingType.Warehouse:
						buildingBlip.Sprite = BlipSprite.Warehouse;
						break;
					case eBuildingType.Bunker:
						buildingBlip.Sprite = BlipSprite.Bunker;
						break;
					}
					buildingBlip.Name = Name;
				}
				buildingBlip.Color = BlipColor.White;
			}
			else
			{
				switch (BuildingType)
				{
				case eBuildingType.Apartment:
					buildingBlip.Sprite = BlipSprite.Safehouse;
					buildingBlip.Name = (SouthLSHousingExterior.IsCustom(this) ? Name : Game.GetLocalizedString("CELL_2630"));
					break;
				case eBuildingType.Office:
					buildingBlip.Sprite = BlipSprite.Office;
					buildingBlip.Name = Game.GetLocalizedString("BLIP_475");
					break;
				case eBuildingType.ClubHouse:
					buildingBlip.Sprite = BlipSprite.BikerClubhouse;
					buildingBlip.Name = Game.GetLocalizedString("PM_SPAWN_CLUBH");
					break;
				case eBuildingType.Garage:
					buildingBlip.Sprite = BlipSprite.Garage;
					buildingBlip.Name = Game.GetLocalizedString("BLIP_357");
					break;
				case eBuildingType.Hangar:
					buildingBlip.Sprite = BlipSprite.GTAOHangar;
					buildingBlip.Name = Game.GetLocalizedString("BLIP_359");
					break;
				case eBuildingType.NightClub:
					buildingBlip.Sprite = BlipSprite.NightclubProperty;
					buildingBlip.Name = Game.GetLocalizedString("CELL_CLUB");
					break;
				case eBuildingType.Warehouse:
					buildingBlip.Sprite = BlipSprite.Warehouse;
					buildingBlip.Name = Game.GetLocalizedString("BLIP_473");
					break;
				case eBuildingType.Bunker:
					buildingBlip.Sprite = BlipSprite.Bunker;
					buildingBlip.Name = Game.GetLocalizedString("BLIP_557");
					break;
				}
				buildingBlip.Color = Helper.GetBlipColor(this);
			}
			if (!IsVacant())
			{
				buildingBlip.Name = OwnedMapLabel(Name);
			}
			else if (GarageType == eGarageType.SixtyCarGarage)
			{
				buildingBlip.Name = Name;
			}
			buildingBlip = null;
			SpaBlips.RegisterCoordinate(BuildingBlip);
		}

		private bool CheckMenuOwner(ApartmentClass apartment)
		{
			if (apartment == null)
			{
				return false;
			}
			eOwner eOwner2 = apartment.Owner();
			if (eOwner2 == Helper.GetPlayer())
			{
				return true;
			}
			bool german = Game.Language == Language.German;
			Notification.Show((eOwner2 == eOwner.Nobody) ? PropertyOwnershipText.ForSaleLabel(german) : PropertyOwnershipText.OwnerLabel(eOwner2, german));
			return false;
		}

		private void AptMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			try
			{
				ApartmentClass apartmentClass = (ApartmentClass)selectedItem.Tag;
				if (apartmentClass == null || CheckMenuOwner(apartmentClass))
				{
					if (apartmentClass != null && SouthLSHousing.IsPropertyID(apartmentClass.ID))
					{
						SouthLSHousingTravel.EnterApartment(this, apartmentClass);
					}
					else
					{
						ApartmentTravel.Enter(this, apartmentClass);
					}
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

		public int GetGarageVehicleCount()
		{
			switch (GarageType)
			{
			case eGarageType.NoGarage:
				return 0;
			case eGarageType.TwoCarGarage:
				return 2;
			case eGarageType.SixCarGarage:
				return 6;
			case eGarageType.TenCarGarage:
				return 10;
			case eGarageType.SixtyCarGarage:
				return 60;
			default:
				return 20;
			}
		}

		private bool VehicleStorageContextValid(ApartmentClass selectedApt, Vehicle vehicle)
		{
			if (selectedApt != null && selectedApt.Owner() == Helper.GetPlayer() && (object)Helper.PP != null && Helper.PP.Exists() && (object)vehicle != null && vehicle.Exists() && Helper.PP.IsInVehicle(vehicle) && (object)vehicle.Driver != null)
			{
				return vehicle.Driver.Handle == Helper.PP.Handle;
			}
			return false;
		}

		private bool ExecutiveRegistrationValid(ApartmentClass selectedApt)
		{
			if (GarageType == eGarageType.SixtyCarGarage && ExecutiveGarageInventory.IsProperty(selectedApt) && PropertySale.RegisteredApartment(selectedApt))
			{
				return selectedApt.Building() == this;
			}
			return false;
		}

		internal int PreviewExecutiveVehicleSlot(ApartmentClass selectedApt, Vehicle vehicle, int preferredFloor = -1)
		{
			if (!ExecutiveRegistrationValid(selectedApt) || !VehicleStorageContextValid(selectedApt, vehicle))
			{
				return -1;
			}
			bool complete = default(bool);
			List<VehicleClass> vehicles = ExecutiveGarageInventory.ReadVehicles(selectedApt, ref complete);
			if (!complete)
			{
				return -1;
			}
			return ExecutiveGarageInventory.SelectVehicleSlot(vehicles, vehicle.GetInt(Helper.vehIdDecor), vehicle.GetInt(Helper.vehUidDecor), vehicle.Model.Hash, preferredFloor);
		}

		internal bool TryStoreApartmentVehicle(ApartmentClass selectedApt, Vehicle vehicle, ref int uid)
		{
			int storedSlot = -1;
			return TryStoreApartmentVehicle(selectedApt, vehicle, ref uid, ref storedSlot);
		}

		internal bool TryStoreApartmentVehicle(ApartmentClass selectedApt, Vehicle vehicle, ref int uid, ref int storedSlot, int preferredFloor = -1, int expectedSlot = -1)
		{
			_Closure_0024__85_002D1 arg = new _Closure_0024__85_002D1(null);
			arg._0024VB_0024Local_selectedApt = selectedApt;
			storedSlot = -1;
			checked
			{
				try
				{
					_Closure_0024__85_002D0 arg2 = new _Closure_0024__85_002D0(null);
					arg2._0024VB_0024NonLocal__0024VB_0024Closure_2 = arg;
					arg2._0024VB_0024Local_executive = GarageType == eGarageType.SixtyCarGarage;
					if (!VehicleStorageContextValid(arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_selectedApt, vehicle) || (arg2._0024VB_0024Local_executive && !ExecutiveRegistrationValid(arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_selectedApt)) || (!arg2._0024VB_0024Local_executive && arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_selectedApt.ID == 9310))
					{
						Notification.Show("The garage transfer is no longer available. Your vehicle has been kept.");
						return false;
					}
					arg2._0024VB_0024Local_oldId = vehicle.GetInt(Helper.vehIdDecor);
					arg2._0024VB_0024Local_oldUid = vehicle.GetInt(Helper.vehUidDecor);
					eOwner player = Helper.GetPlayer();
					arg2._0024VB_0024Local_reserved = null;
					if (!arg2._0024VB_0024Local_executive && arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_selectedApt.ApartmentType == eApartmentType.Other && arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_selectedApt.HasCharacterProfile() && !arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_selectedApt.TryReadStandaloneGarageSlots(player, GetGarageVehicleCount(), ref arg2._0024VB_0024Local_reserved))
					{
						Notification.Show("Garage records need review. All saves and your vehicle have been kept.");
						return false;
					}
					bool complete = true;
					List<VehicleClass> list = (arg2._0024VB_0024Local_executive ? ExecutiveGarageInventory.ReadVehicles(arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_selectedApt, ref complete) : arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_selectedApt.Vehicles());
					if (!complete)
					{
						Notification.Show("Executive garage records need review. All saves and your vehicle have been kept.");
						return false;
					}
					VehicleClass vehicleClass = list.FirstOrDefault(arg2._Lambda_0024__0);
					int num = -1;
					if (arg2._0024VB_0024Local_executive)
					{
						num = ExecutiveGarageInventory.SelectVehicleSlot(list, arg2._0024VB_0024Local_oldId, arg2._0024VB_0024Local_oldUid, vehicle.Model.Hash, preferredFloor);
						if (expectedSlot >= 0 && expectedSlot != num)
						{
							Notification.Show("The selected garage space changed. Your vehicle has been kept.");
							return false;
						}
					}
					else if (vehicleClass != null && vehicleClass.Index >= 0 && vehicleClass.Index < GetGarageVehicleCount())
					{
						num = vehicleClass.Index;
					}
					else
					{
						int num2 = GetGarageVehicleCount() - 1;
						_Closure_0024__85_002D2 closure_0024__85_002D = default(_Closure_0024__85_002D2);
						for (int i = 0; i <= num2; i++)
						{
							closure_0024__85_002D = new _Closure_0024__85_002D2(closure_0024__85_002D);
							closure_0024__85_002D._0024VB_0024Local_candidateIndex = i;
							if (!((arg2._0024VB_0024Local_reserved != null) ? arg2._0024VB_0024Local_reserved.Contains(closure_0024__85_002D._0024VB_0024Local_candidateIndex) : list.Any(closure_0024__85_002D._Lambda_0024__1)))
							{
								num = i;
								break;
							}
						}
					}
					if (num < 0)
					{
						Notification.Show("The garage has no free saved vehicle space. Your vehicle has been kept.");
						return false;
					}
					string text = arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_selectedApt.StorageFolder(PropertyProfiles.ActiveCharacter);
					if (text == null)
					{
						return false;
					}
					arg2._0024VB_0024Local_directoryPath = Helper.grgXmlPath + text;
					Directory.CreateDirectory(arg2._0024VB_0024Local_directoryPath);
					arg2._0024VB_0024Local_destinationPath = arg2._Lambda_0024__2;
					arg2._0024VB_0024Local_expansionSource = ExpandedGarage.IsStorageId(arg2._0024VB_0024Local_oldId);
					string text2 = null;
					if (arg2._0024VB_0024Local_expansionSource)
					{
						text2 = ExpandedGarage.SavedVehiclePath(Helper.grgXmlPath, arg2._0024VB_0024Local_oldId, player, arg2._0024VB_0024Local_oldUid);
					}
					else if (arg2._0024VB_0024Local_oldId != 0 && arg2._0024VB_0024Local_oldUid != 0)
					{
						string text3 = Mansions.GarageFolderForId(arg2._0024VB_0024Local_oldId);
						if (text3 != null)
						{
							string text4 = ((arg2._0024VB_0024Local_oldId == 9310) ? arg2._0024VB_0024Local_oldUid.ToString(CultureInfo.InvariantCulture) : Conversions.ToString(arg2._0024VB_0024Local_oldUid));
							text2 = Path.Combine(Helper.grgXmlPath + text3, text4 + ".xml");
						}
					}
					if ((arg2._0024VB_0024Local_expansionSource || (arg2._0024VB_0024Local_oldUid != 0 && Mansions.IsCharacterPropertyGarage(arg2._0024VB_0024Local_oldId)) || arg2._0024VB_0024Local_oldId == 9310 || vehicleClass != null || (text2 != null && File.Exists(text2))) && (text2 == null || !PropertySale.VerifiedTransferIdentity(text2, player, arg2._0024VB_0024Local_oldId, arg2._0024VB_0024Local_oldUid, vehicle.Model.Hash)))
					{
						Notification.Show("The source garage record could not be verified. Your vehicle has been kept.");
						return false;
					}
					bool flag = arg2._0024VB_0024Local_expansionSource || Apartments.Any(arg2._Lambda_0024__3);
					Func<int, bool> func = arg2._Lambda_0024__4;
					bool flag2 = CanRetainTransferredUid(arg2._0024VB_0024Local_oldUid, arg2._0024VB_0024Local_oldId != arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_selectedApt.ID && flag, text2 != null && File.Exists(text2), list.Any(arg2._Lambda_0024__5) || func(arg2._0024VB_0024Local_oldUid));
					bool flag3 = (vehicleClass != null && arg2._0024VB_0024Local_oldUid != 0) || flag2;
					uid = (flag3 ? arg2._0024VB_0024Local_oldUid : Guid.NewGuid().GetHashCode());
					while (uid == 0 || (!flag3 && func(uid)))
					{
						uid = Guid.NewGuid().GetHashCode();
					}
					VehicleData vehicleData = new VehicleData(arg2._0024VB_0024Local_destinationPath(uid), new VehicleClass(vehicle, player, arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_selectedApt.ID, uid, num));
					if (arg2._0024VB_0024Local_expansionSource)
					{
						if (!vehicleData.TrySaveTransferred(text2, arg2._0024VB_0024Local_oldId, arg2._0024VB_0024Local_oldUid, (_Closure_0024__._0024I85_002D6 != null) ? _Closure_0024__._0024I85_002D6 : (_Closure_0024__._0024I85_002D6 = _Closure_0024__._0024I._Lambda_0024__85_002D6)))
						{
							Notification.Show("The garage transfer did not complete. Your previous save has been kept.");
							return false;
						}
					}
					else
					{
						vehicleData.SaveTransferred(text2, arg2._0024VB_0024Local_oldUid);
					}
					storedSlot = num;
					try
					{
						Logger.Log($"> Garage {arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_selectedApt.Name}: verified transfer to slot {num}, uid {uid}");
					}
					catch (Exception projectError)
					{
						ProjectData.SetProjectError(projectError);
						ProjectData.ClearProjectError();
					}
					return true;
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					try
					{
						Logger.Log("Garage transfer failed; original vehicle and previous save kept: " + ex2.Message);
					}
					catch (Exception projectError2)
					{
						ProjectData.SetProjectError(projectError2);
						ProjectData.ClearProjectError();
					}
					Notification.Show("Vehicle could not be saved. Your vehicle and its previous garage save have been kept.");
					bool result = false;
					ProjectData.ClearProjectError();
					return result;
				}
			}
		}

		public static bool CanRetainTransferredUid(int uid, bool registeredCrossGarageSource, bool sourceExists, bool destinationOccupied)
		{
			if (uid != 0 && registeredCrossGarageSource && sourceExists)
			{
				return !destinationOccupied;
			}
			return false;
		}

		private void RemoveStoredWorldVehicle(Vehicle vehicle)
		{
			if ((object)vehicle == null || !vehicle.Exists())
			{
				return;
			}
			try
			{
				Blip blip = V2Compat.CurrentBlip(vehicle);
				if ((object)blip != null && blip.Exists())
				{
					V2Compat.Remove(blip);
				}
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
			}
			vehicle.Delete();
		}

		private void SaveVehicle(ApartmentClass selectedApt)
		{
			_Closure_0024__88_002D0 closure_0024__88_002D = new _Closure_0024__88_002D0();
			closure_0024__88_002D._0024VB_0024Me = this;
			closure_0024__88_002D._0024VB_0024Local_selectedApt = selectedApt;
			if (GarageType == eGarageType.SixtyCarGarage)
			{
				GrgMenu.Visible = false;
				ExecutiveGarage.Enter(this, closure_0024__88_002D._0024VB_0024Local_selectedApt);
				return;
			}
			if (SouthLSHousingExterior.IsCustom(this))
			{
				if (SouthLSHousingExterior.CanUseGarage(this) && closure_0024__88_002D._0024VB_0024Local_selectedApt != null && closure_0024__88_002D._0024VB_0024Local_selectedApt.Owner() == Helper.GetPlayer())
				{
					GrgMenu.Visible = false;
					Helper.HideHud = false;
					if ((object)Helper.PP != null && Helper.PP.Exists() && Helper.PP.IsInVehicle())
					{
						_Closure_0024__88_002D1 closure_0024__88_002D2 = new _Closure_0024__88_002D1();
						closure_0024__88_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2 = closure_0024__88_002D;
						closure_0024__88_002D2._0024VB_0024Local_driven = Helper.PP.CurrentVehicle;
						TwoCarGarage.EnterHousingVehicle(closure_0024__88_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_selectedApt, closure_0024__88_002D2._Lambda_0024__0);
					}
					else
					{
						TwoCarGarage.EnterHousingGarageOnFoot(closure_0024__88_002D._0024VB_0024Local_selectedApt);
					}
				}
				return;
			}
			try
			{
				GrgMenu.Visible = false;
				Helper.HideHud = false;
				PlayEnterGarageCamera(7000, true, true, CameraShake.Hand, 0.4f);
				Helper.FadeScreen(1);
				Helper.HideHud = false;
				Stability.DestroySpaCameras();
				World.RenderingCamera = null;
				switch (GarageType)
				{
				case eGarageType.TwoCarGarage:
					TwoCarGarage.Apartment = closure_0024__88_002D._0024VB_0024Local_selectedApt;
					Helper.SetInteriorActive(TwoCarGarage.Interior);
					if (Helper.PP.IsInVehicle())
					{
						Vehicle currentVehicle2 = Helper.PP.CurrentVehicle;
						ApartmentClass _0024VB_0024Local_selectedApt2 = closure_0024__88_002D._0024VB_0024Local_selectedApt;
						int storedSlot = -1;
						int uid2 = default(int);
						if (TryStoreApartmentVehicle(_0024VB_0024Local_selectedApt2, currentVehicle2, ref uid2, ref storedSlot))
						{
							Helper.PP.Task.WarpOutOfVehicle(currentVehicle2);
							if (Helper.IsCurrentVehicleExistInList(currentVehicle2))
							{
								Helper.outVehicleList.Remove(currentVehicle2);
							}
							Helper.PP.Position = TwoCarGarage.Elevator;
							TwoCarGarage.LoadVehiclesSetPlayerPos(uid2);
							RemoveStoredWorldVehicle(currentVehicle2);
							Script.Wait(1000);
							Helper.PP.Task.LeaveVehicle();
						}
					}
					else
					{
						Helper.PP.Position = Helper.ToVector3(TwoCarGarage.SpawnInPos);
						Helper.PP.Heading = TwoCarGarage.SpawnInPos.W;
						TwoCarGarage.LoadVehicles();
					}
					break;
				case eGarageType.SixCarGarage:
					SixCarGarage.Apartment = closure_0024__88_002D._0024VB_0024Local_selectedApt;
					Helper.SetInteriorActive(SixCarGarage.Interior);
					if (Helper.PP.IsInVehicle())
					{
						Vehicle currentVehicle3 = Helper.PP.CurrentVehicle;
						ApartmentClass _0024VB_0024Local_selectedApt3 = closure_0024__88_002D._0024VB_0024Local_selectedApt;
						int storedSlot = -1;
						int uid3 = default(int);
						if (TryStoreApartmentVehicle(_0024VB_0024Local_selectedApt3, currentVehicle3, ref uid3, ref storedSlot))
						{
							Helper.PP.Task.WarpOutOfVehicle(currentVehicle3);
							if (Helper.IsCurrentVehicleExistInList(currentVehicle3))
							{
								Helper.outVehicleList.Remove(currentVehicle3);
							}
							Helper.PP.Position = SixCarGarage.Elevator;
							SixCarGarage.LoadVehiclesSetPlayerPos(uid3);
							RemoveStoredWorldVehicle(currentVehicle3);
							Script.Wait(1000);
							Helper.PP.Task.LeaveVehicle();
						}
					}
					else
					{
						Helper.PP.Position = Helper.ToVector3(SixCarGarage.SpawnInPos);
						Helper.PP.Heading = SixCarGarage.SpawnInPos.W;
						SixCarGarage.LoadVehicles();
					}
					break;
				case eGarageType.TenCarGarage:
					TenCarGarage.Apartment = closure_0024__88_002D._0024VB_0024Local_selectedApt;
					Helper.SetInteriorActive(TenCarGarage.Interior);
					if (Helper.PP.IsInVehicle())
					{
						Vehicle currentVehicle = Helper.PP.CurrentVehicle;
						ApartmentClass _0024VB_0024Local_selectedApt = closure_0024__88_002D._0024VB_0024Local_selectedApt;
						int storedSlot = -1;
						int uid = default(int);
						if (TryStoreApartmentVehicle(_0024VB_0024Local_selectedApt, currentVehicle, ref uid, ref storedSlot))
						{
							Helper.PP.Task.WarpOutOfVehicle(currentVehicle);
							if (Helper.IsCurrentVehicleExistInList(currentVehicle))
							{
								Helper.outVehicleList.Remove(currentVehicle);
							}
							Helper.PP.Position = TenCarGarage.Elevator;
							TenCarGarage.LoadVehiclesSetPlayerPos(uid);
							RemoveStoredWorldVehicle(currentVehicle);
							Script.Wait(1000);
							Helper.PP.Task.LeaveVehicle();
						}
					}
					else
					{
						Helper.PP.Position = Helper.ToVector3(TenCarGarage.SpawnInPos);
						Helper.PP.Heading = TenCarGarage.SpawnInPos.W;
						TenCarGarage.LoadVehicles();
					}
					break;
				case eGarageType.TwentyCarGarage:
					Helper.SetInteriorActive(Helper.TwentyCarGarage);
					break;
				}
			}
			finally
			{
				Helper.FadeScreen(0);
			}
		}

		private void GrgMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			if (SouthLSHousingExterior.IsCustom(this) && !SouthLSHousingExterior.CanUseGarage(this))
			{
				Notification.Show("The nearby parking access is still loading. Please try again shortly.");
				return;
			}
			try
			{
				ApartmentClass apartmentClass = (ApartmentClass)selectedItem.Tag;
				if (!CheckMenuOwner(apartmentClass))
				{
					return;
				}
				if (GarageType == eGarageType.SixtyCarGarage)
				{
					World.RenderingCamera = null;
					Stability.DestroySpaCameras();
					Helper.HideHud = false;
					SaveVehicle(apartmentClass);
				}
				else if (Helper.PP.IsInVehicle())
				{
					if (apartmentClass.Vehicles().Count != GetGarageVehicleCount())
					{
						World.RenderingCamera = null;
						Stability.DestroySpaCameras();
						Helper.HideHud = false;
						SaveVehicle(apartmentClass);
					}
					else if (apartmentClass.VehiclesContain(Game.Player.Character.LastVehicle))
					{
						World.RenderingCamera = null;
						Stability.DestroySpaCameras();
						Helper.HideHud = false;
						SaveVehicle(apartmentClass);
					}
					else
					{
						Screen.ShowSubtitle(Game.GetLocalizedString("WEB_VEH_FULL"));
					}
				}
				else
				{
					World.RenderingCamera = null;
					Stability.DestroySpaCameras();
					Helper.HideHud = false;
					SaveVehicle(apartmentClass);
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

		private void AptMenu_OnMenuClose(UIMenu sender)
		{
			if (GarageType != eGarageType.SixtyCarGarage)
			{
				Helper.FadeScreen(1);
				Stability.DestroySpaCameras();
				World.RenderingCamera = null;
				Helper.HideHud = false;
				Helper.FadeScreen(0);
			}
		}

		private void BuyMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			try
			{
				ApartmentClass apartmentClass = ((selectedItem == null) ? null : (selectedItem.Tag as ApartmentClass));
				if (apartmentClass != null)
				{
					SouthLSHousingPurchase.Buy(this, apartmentClass);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Property purchase menu: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		public void PlayEnterApartmentCamera(int duration, bool easePosition, bool easeRotation, CameraShake camShake, float amplitude)
		{
			Helper.HideHud = true;
			switch (FrontDoor)
			{
			case eFrontDoor.DoubleDoors:
				Door1.UnlockDoor();
				Door2.UnlockDoor();
				break;
			case eFrontDoor.StandardDoor:
				Door1.UnlockDoor();
				break;
			}
			V2Compat.GoTo(Helper.PP.Task, Helper.ToVector3(BuildingLobby), true, 7000);
			Camera camera = Stability.CreateCam(EnterCamera1.Position, EnterCamera1.Rotation, EnterCamera1.FOV);
			Camera camera2 = Stability.CreateCam(EnterCamera2.Position, EnterCamera2.Rotation, EnterCamera2.FOV);
			World.RenderingCamera = camera;
			camera.InterpTo(camera2, duration, easePosition ? 1 : 0, easeRotation ? 1 : 0);
			World.RenderingCamera = camera2;
			camera2.Shake(camShake, amplitude);
			Script.Wait(duration);
			switch (FrontDoor)
			{
			case eFrontDoor.DoubleDoors:
				Door1.LockDoor();
				Door2.LockDoor();
				break;
			case eFrontDoor.StandardDoor:
				Door1.LockDoor();
				break;
			}
			Helper.HideHud = false;
		}

		public void PlayExitApartmentCamera(int duration, bool easePosition, bool easeRotation, CameraShake camShake, float amplitude)
		{
			Helper.HideHud = true;
			Helper.PP.Position = Helper.ToVector3(BuildingLobby);
			Helper.PP.Heading = BuildingOutPos.W;
			switch (FrontDoor)
			{
			case eFrontDoor.DoubleDoors:
				Door1.UnlockDoor();
				Door2.UnlockDoor();
				break;
			case eFrontDoor.StandardDoor:
				Door1.UnlockDoor();
				break;
			}
			V2Compat.GoTo(Helper.PP.Task, Helper.ToVector3(BuildingOutPos), false, 7000);
			Camera camera = Stability.CreateCam(EnterCamera2.Position, EnterCamera2.Rotation, EnterCamera2.FOV);
			Camera camera2 = Stability.CreateCam(EnterCamera1.Position, EnterCamera1.Rotation, EnterCamera1.FOV);
			World.RenderingCamera = camera;
			camera.InterpTo(camera2, duration, easePosition ? 1 : 0, easeRotation ? 1 : 0);
			World.RenderingCamera = camera2;
			camera2.Shake(camShake, amplitude);
			Script.Wait(duration);
			Stability.DestroySpaCameras();
			World.RenderingCamera = null;
			switch (FrontDoor)
			{
			case eFrontDoor.DoubleDoors:
				Door1.LockDoor();
				Door2.LockDoor();
				break;
			case eFrontDoor.StandardDoor:
				Door1.LockDoor();
				break;
			}
			Helper.HideHud = false;
		}

		public void PlayEnterGarageCamera(int duration, bool easePosition, bool easeRotation, CameraShake camShake, float amplitude)
		{
			if (SouthLSHousingExterior.IsCustom(this))
			{
				return;
			}
			if (GarageDoor == eFrontDoor.StandardDoor)
			{
				Helper.HideHud = true;
				Door3.UnlockDoor();
				if (Helper.PP.IsInVehicle())
				{
					Helper.PP.CurrentVehicle.Position = Helper.ToVector3(GarageOutPos);
					Helper.PP.CurrentVehicle.Heading = GarageOutPos.W - 180f;
					Helper.PP.CurrentVehicle.PlaceOnGround();
					Camera camera = Stability.CreateCam(EnterCamera3.Position, EnterCamera3.Rotation, EnterCamera3.FOV);
					Camera camera2 = Stability.CreateCam(EnterCamera4.Position, EnterCamera4.Rotation, EnterCamera4.FOV);
					World.RenderingCamera = camera;
					Script.Wait(3000);
					TaskSequence taskSequence = new TaskSequence();
					taskSequence.AddTask.DriveTo(Helper.PP.CurrentVehicle, GarageInPos, 0.1f, 5f, DrivingStyle.Rushed);
					taskSequence.AddTask.DriveTo(Helper.PP.CurrentVehicle, Helper.ToVector3(GarageFootOutPos), 0.1f, 5f, DrivingStyle.Rushed);
					taskSequence.AddTask.DriveTo(Helper.PP.CurrentVehicle, Helper.ToVector3(GarageWaypoint), 0.1f, 5f, DrivingStyle.Rushed);
					Helper.PP.Task.PerformSequence(taskSequence);
					camera.InterpTo(camera2, duration, easePosition ? 1 : 0, easeRotation ? 1 : 0);
					World.RenderingCamera = camera2;
					camera2.Shake(camShake, amplitude);
					Script.Wait(duration);
					taskSequence.Close();
					taskSequence.Dispose();
				}
				else
				{
					Helper.PP.Position = new Vector3(GarageInPos.X, GarageInPos.Y, GarageInPos.Z - 1f);
					Helper.PP.Heading = GarageOutPos.W - 180f;
					TaskSequence taskSequence2 = new TaskSequence();
					V2Compat.GoTo(taskSequence2.AddTask, Helper.ToVector3(GarageFootOutPos), false, duration);
					V2Compat.GoTo(taskSequence2.AddTask, Helper.ToVector3(GarageWaypoint), false, duration);
					Helper.PP.Task.PerformSequence(taskSequence2);
					Camera camera3 = Stability.CreateCam(EnterCamera3.Position, EnterCamera3.Rotation, EnterCamera3.FOV);
					Camera camera4 = Stability.CreateCam(EnterCamera4.Position, EnterCamera4.Rotation, EnterCamera4.FOV);
					World.RenderingCamera = camera3;
					camera3.InterpTo(camera4, duration, easePosition ? 1 : 0, easeRotation ? 1 : 0);
					World.RenderingCamera = camera4;
					camera4.Shake(camShake, amplitude);
					Script.Wait(duration);
					taskSequence2.Close();
					taskSequence2.Dispose();
				}
				Door3.LockDoor();
			}
			else
			{
				Audio.PlaySoundFromEntityAndForget(Helper.PP, "GARAGE_DOOR_SCRIPTED_CLOSE");
			}
		}

		public void PlayExitGarageCamera(int duration, bool easePosition, bool easeRotation, CameraShake camShake, float amplitude)
		{
			if (!SouthLSHousingExterior.IsCustom(this) && GarageDoor == eFrontDoor.StandardDoor)
			{
				Helper.HideHud = true;
				Door3.UnlockDoor();
				if (Helper.PP.IsInVehicle())
				{
					Helper.PP.CurrentVehicle.Position = Helper.ToVector3(GarageWaypoint);
					Helper.PP.CurrentVehicle.Heading = GarageWaypoint.W;
					TaskSequence taskSequence = new TaskSequence();
					taskSequence.AddTask.DriveTo(Helper.PP.CurrentVehicle, Helper.ToVector3(GarageFootOutPos), 0.1f, 5f, DrivingStyle.Rushed);
					taskSequence.AddTask.DriveTo(Helper.PP.CurrentVehicle, GarageInPos, 0.1f, 5f, DrivingStyle.Rushed);
					taskSequence.AddTask.DriveTo(Helper.PP.CurrentVehicle, Helper.ToVector3(GarageOutPos), 0.1f, 5f, DrivingStyle.Rushed);
					Helper.PP.Task.PerformSequence(taskSequence);
					Camera camera = Stability.CreateCam(EnterCamera4.Position, EnterCamera4.Rotation, EnterCamera4.FOV);
					Camera camera2 = Stability.CreateCam(EnterCamera3.Position, EnterCamera3.Rotation, EnterCamera3.FOV);
					World.RenderingCamera = camera;
					camera.InterpTo(camera2, duration, easePosition ? 1 : 0, easeRotation ? 1 : 0);
					World.RenderingCamera = camera2;
					camera2.Shake(camShake, amplitude);
					Helper.FadeScreen(0);
					Script.Wait(duration);
					taskSequence.Close();
					taskSequence.Dispose();
				}
				else
				{
					Helper.PP.Position = GarageInPos;
					Helper.PP.Heading = GarageFootOutPos.W;
					Helper.FadeScreen(0);
				}
				Stability.DestroySpaCameras();
				World.RenderingCamera = null;
				Door3.LockDoor();
				Helper.HideHud = false;
			}
		}

		public void PlayEnterElevatorCutScene(int duration)
		{
			Helper.HideHud = true;
			switch (GarageType)
			{
			case eGarageType.TwoCarGarage:
				World.RenderingCamera = Stability.CreateCam(new Vector3(176.5861f, -1007.405f, -98.99998f), new Vector3(3.541232f, 1.069259E-07f, -44.71983f), 50f);
				Helper.PP.Position = TwoCarGarage.Elevator;
				Helper.PP.Heading = TwoCarGarage.ElevatorInside.W - 160f;
				V2Compat.GoTo(Helper.PP.Task, Helper.ToVector3(TwoCarGarage.ElevatorInside), false, duration);
				break;
			case eGarageType.SixCarGarage:
				World.RenderingCamera = Stability.CreateCam(new Vector3(204.9573f, -1001.148f, -98.99999f), new Vector3(1.120068f, 5.337105E-08f, -57.21745f), 50f);
				Helper.PP.Position = SixCarGarage.Elevator;
				Helper.PP.Heading = SixCarGarage.ElevatorInside.W - 160f;
				V2Compat.GoTo(Helper.PP.Task, Helper.ToVector3(SixCarGarage.ElevatorInside), false, duration);
				break;
			case eGarageType.TenCarGarage:
				World.RenderingCamera = Stability.CreateCam(new Vector3(235.6371f, -1003.728f, -98.9999f), new Vector3(2.576892f, -5.341485E-08f, -110.4919f), 50f);
				Helper.PP.Position = new Vector3(237.2018f, -1004.526f, -99.9999f);
				Helper.PP.Heading = 270f;
				Helper.PP.Task.PlayAnimation("anim@apt_trans@elevator", "elev_1");
				break;
			}
			Script.Wait(duration);
		}

		public void PlayExitElevatorCutscene(int duration)
		{
			Helper.HideHud = true;
			switch (GarageType)
			{
			case eGarageType.TwoCarGarage:
				World.RenderingCamera = Stability.CreateCam(new Vector3(176.5861f, -1007.405f, -98.99998f), new Vector3(3.541232f, 1.069259E-07f, -44.71983f), 50f);
				Helper.PP.Position = Helper.ToVector3(TwoCarGarage.ElevatorInside);
				Helper.PP.Heading = TwoCarGarage.ElevatorInside.W;
				V2Compat.GoTo(Helper.PP.Task, new Vector3(177.9277f, -1006.376f, -99.99992f), false, duration);
				break;
			case eGarageType.SixCarGarage:
				World.RenderingCamera = Stability.CreateCam(new Vector3(204.9573f, -1001.148f, -98.99999f), new Vector3(1.120068f, 5.337105E-08f, -57.21745f), 50f);
				Helper.PP.Position = Helper.ToVector3(SixCarGarage.ElevatorInside);
				Helper.PP.Heading = SixCarGarage.ElevatorInside.W;
				V2Compat.GoTo(Helper.PP.Task, new Vector3(205.923f, -999.2991f, -100f), false, duration);
				break;
			case eGarageType.TenCarGarage:
				World.RenderingCamera = Stability.CreateCam(new Vector3(235.6371f, -1003.728f, -98.9999f), new Vector3(2.576892f, -5.341485E-08f, -110.4919f), 50f);
				Helper.PP.Position = Helper.ToVector3(TenCarGarage.ElevatorInside);
				Helper.PP.Heading = TenCarGarage.ElevatorInside.W;
				V2Compat.GoTo(Helper.PP.Task, new Vector3(237.2018f, -1004.526f, -99.9999f), false, duration);
				break;
			}
			Script.Wait(duration);
		}

		private void GrgMenu_OnMenuClose(UIMenu sender)
		{
			if (GarageType != eGarageType.SixtyCarGarage)
			{
				World.RenderingCamera = null;
				Stability.DestroySpaCameras();
				Helper.HideHud = false;
			}
		}
	}
}
