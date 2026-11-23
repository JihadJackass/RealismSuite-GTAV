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
using iFruitAddon2;
using INMNativeUI;
using Metadata;
using Microsoft.VisualBasic.CompilerServices;
using SPAII.INM;

namespace SPAII
{
	[StandardModule]
	internal static class Mechanic
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<Mansion, bool> _0024I41_002D0;

			public static Action<Exception> _0024I45_002D8;

			public static Func<Mansion, bool> _0024I52_002D0;

			public static Func<ApartmentClass, bool> _0024I52_002D1;

			public static Func<Mansion, bool> _0024I53_002D0;

			public static Func<ApartmentClass, bool> _0024I53_002D1;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__41_002D0(Mansion x)
			{
				if (x.Enabled)
				{
					return x.IsOwnedByPlayer();
				}
				return false;
			}

			[SpecialName]
			internal void _Lambda_0024__45_002D8(Exception cleanupError)
			{
				Logger.Log("Mechanic delivery cleanup: " + cleanupError.Message);
			}

			[SpecialName]
			internal bool _Lambda_0024__52_002D0(Mansion x)
			{
				if (x.Enabled)
				{
					return x.Owner == eOwner.Nobody;
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__52_002D1(ApartmentClass a)
			{
				return a.Owner() == eOwner.Nobody;
			}

			[SpecialName]
			internal bool _Lambda_0024__53_002D0(Mansion x)
			{
				if (x.Enabled)
				{
					return x.IsOwnedByPlayer();
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__53_002D1(ApartmentClass a)
			{
				return a.Owner() == Helper.GetPlayer();
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__42_002D0
		{
			public VehicleClass _0024VB_0024Local_veh;

			public _Closure_0024__42_002D0(_Closure_0024__42_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_veh = arg0._0024VB_0024Local_veh;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(Vehicle x)
			{
				if ((object)x != null && x.Exists() && x.GetInt(Helper.vehUidDecor) == _0024VB_0024Local_veh.UniqueID)
				{
					return x.GetInt(Helper.vehIdDecor) == _0024VB_0024Local_veh.ApartmentID;
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__45_002D0
		{
			public VehicleClass _0024VB_0024Local_veh;

			public Mansion _0024VB_0024Local_mansion;

			public eOwner _0024VB_0024Local_owner;

			public ApartmentClass _0024VB_0024Local_apartment;

			public Ped _0024VB_0024Local_actor;

			public Vector3 _0024VB_0024Local_origin;

			public Func<VehicleClass, bool> _0024I2;

			public Func<VehicleClass, bool> _0024I3;

			[SpecialName]
			internal bool _Lambda_0024__0(ApartmentClass a)
			{
				return a.ID == _0024VB_0024Local_veh.ApartmentID;
			}

			[SpecialName]
			internal VehicleClass _Lambda_0024__1()
			{
				if (_0024VB_0024Local_veh.ApartmentID == 9310 && ExecutiveGarage.IsVehicleLoaded(_0024VB_0024Local_veh.UniqueID))
				{
					return null;
				}
				if (_0024VB_0024Local_mansion != null && _0024VB_0024Local_mansion.Owner == _0024VB_0024Local_owner)
				{
					return Mansions.MansionGarageVehicles(_0024VB_0024Local_mansion).FirstOrDefault((_0024I2 == null) ? (_0024I2 = _Lambda_0024__2) : _0024I2);
				}
				if (_0024VB_0024Local_apartment != null && _0024VB_0024Local_apartment.Owner() == _0024VB_0024Local_owner)
				{
					return _0024VB_0024Local_apartment.Vehicles().FirstOrDefault((_0024I3 == null) ? (_0024I3 = _Lambda_0024__3) : _0024I3);
				}
				return null;
			}

			[SpecialName]
			internal bool _Lambda_0024__2(VehicleClass v)
			{
				if (v.UniqueID == _0024VB_0024Local_veh.UniqueID)
				{
					return v.Hash == _0024VB_0024Local_veh.Hash;
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__3(VehicleClass v)
			{
				if (v.UniqueID == _0024VB_0024Local_veh.UniqueID)
				{
					return v.Hash == _0024VB_0024Local_veh.Hash;
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__4(Vehicle v)
			{
				if ((object)v != null && v.Exists() && v.GetInt(Helper.vehIdDecor) == _0024VB_0024Local_veh.ApartmentID)
				{
					return v.GetInt(Helper.vehUidDecor) == _0024VB_0024Local_veh.UniqueID;
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__5()
			{
				return DeliveryContextValid(_0024VB_0024Local_actor, _0024VB_0024Local_owner, _0024VB_0024Local_origin);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__45_002D1
		{
			public bool _0024VB_0024Local_requestedModel;

			public Model _0024VB_0024Local_model;

			public UIMenu _0024VB_0024Local_sender;

			[SpecialName]
			internal void _Lambda_0024__6()
			{
				if (_0024VB_0024Local_requestedModel && !StoryLoadGuard.WorldLoading())
				{
					_0024VB_0024Local_model.MarkAsNoLongerNeeded();
				}
			}

			[SpecialName]
			internal void _Lambda_0024__7()
			{
				_0024VB_0024Local_sender.Visible = false;
			}
		}

		public static CustomiFruit iFruit;

		private static bool deliveryBusy;

		[System.Runtime.CompilerServices.AccessedThroughProperty("MechanicContact")]
		private static iFruitContact _MechanicContact;

		public static iFruitContact MechanicContact
		{
			[CompilerGenerated]
			get
			{
				return _MechanicContact;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ContactAnsweredEvent value2 = MechanicContact_Answered;
				iFruitContact mechanicContact = _MechanicContact;
				if (mechanicContact != null)
				{
					mechanicContact.Answered -= value2;
				}
				_MechanicContact = value;
				mechanicContact = _MechanicContact;
				if (mechanicContact != null)
				{
					mechanicContact.Answered += value2;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("InsuranceContact")]
		private static iFruitContact _InsuranceContact;

		public static iFruitContact InsuranceContact
		{
			[CompilerGenerated]
			get
			{
				return _InsuranceContact;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ContactAnsweredEvent value2 = InsuranceContact_Answered;
				iFruitContact insuranceContact = _InsuranceContact;
				if (insuranceContact != null)
				{
					insuranceContact.Answered -= value2;
				}
				_InsuranceContact = value;
				insuranceContact = _InsuranceContact;
				if (insuranceContact != null)
				{
					insuranceContact.Answered += value2;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("Dynasty8Contact")]
		private static iFruitContact _Dynasty8Contact;

		public static iFruitContact Dynasty8Contact
		{
			[CompilerGenerated]
			get
			{
				return _Dynasty8Contact;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ContactAnsweredEvent value2 = Dynasty8Contact_Answered;
				iFruitContact dynasty8Contact = _Dynasty8Contact;
				if (dynasty8Contact != null)
				{
					dynasty8Contact.Answered -= value2;
				}
				_Dynasty8Contact = value;
				dynasty8Contact = _Dynasty8Contact;
				if (dynasty8Contact != null)
				{
					dynasty8Contact.Answered += value2;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("MechanicMenu")]
		private static UIMenu _MechanicMenu;

		public static UIMenu MechanicMenu
		{
			[CompilerGenerated]
			get
			{
				return _MechanicMenu;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = MechanicMenu_OnItemSelect;
				MenuCloseEvent value3 = MechanicMenu_OnMenuClose;
				UIMenu mechanicMenu = _MechanicMenu;
				if (mechanicMenu != null)
				{
					mechanicMenu.OnItemSelect -= value2;
					mechanicMenu.OnMenuClose -= value3;
				}
				_MechanicMenu = value;
				mechanicMenu = _MechanicMenu;
				if (mechanicMenu != null)
				{
					mechanicMenu.OnItemSelect += value2;
					mechanicMenu.OnMenuClose += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("MechanicAptMenu")]
		private static UIMenu _MechanicAptMenu;

		public static UIMenu MechanicAptMenu
		{
			[CompilerGenerated]
			get
			{
				return _MechanicAptMenu;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = MechanicAptMenu_OnItemSelect;
				UIMenu mechanicAptMenu = _MechanicAptMenu;
				if (mechanicAptMenu != null)
				{
					mechanicAptMenu.OnItemSelect -= value2;
				}
				_MechanicAptMenu = value;
				mechanicAptMenu = _MechanicAptMenu;
				if (mechanicAptMenu != null)
				{
					mechanicAptMenu.OnItemSelect += value2;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("InsuranceMenu")]
		private static UIMenu _InsuranceMenu;

		public static UIMenu InsuranceMenu
		{
			[CompilerGenerated]
			get
			{
				return _InsuranceMenu;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = InsuranceMenu_OnItemSelect;
				UIMenu insuranceMenu = _InsuranceMenu;
				if (insuranceMenu != null)
				{
					insuranceMenu.OnItemSelect -= value2;
				}
				_InsuranceMenu = value;
				insuranceMenu = _InsuranceMenu;
				if (insuranceMenu != null)
				{
					insuranceMenu.OnItemSelect += value2;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("Dynasty8Menu")]
		private static UIMenu _Dynasty8Menu;

		public static UIMenu Dynasty8Menu
		{
			[CompilerGenerated]
			get
			{
				return _Dynasty8Menu;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = Dynasty8Menu_OnItemSelect;
				UIMenu dynasty8Menu = _Dynasty8Menu;
				if (dynasty8Menu != null)
				{
					dynasty8Menu.OnItemSelect -= value2;
				}
				_Dynasty8Menu = value;
				dynasty8Menu = _Dynasty8Menu;
				if (dynasty8Menu != null)
				{
					dynasty8Menu.OnItemSelect += value2;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("Dynasty8BuyMenu")]
		private static UIMenu _Dynasty8BuyMenu;

		public static UIMenu Dynasty8BuyMenu
		{
			[CompilerGenerated]
			get
			{
				return _Dynasty8BuyMenu;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = Dynasty8BuyMenu_OnItemSelect;
				UIMenu dynasty8BuyMenu = _Dynasty8BuyMenu;
				if (dynasty8BuyMenu != null)
				{
					dynasty8BuyMenu.OnItemSelect -= value2;
				}
				_Dynasty8BuyMenu = value;
				dynasty8BuyMenu = _Dynasty8BuyMenu;
				if (dynasty8BuyMenu != null)
				{
					dynasty8BuyMenu.OnItemSelect += value2;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("Dynasty8TradeMenu")]
		private static UIMenu _Dynasty8TradeMenu;

		public static UIMenu Dynasty8TradeMenu
		{
			[CompilerGenerated]
			get
			{
				return _Dynasty8TradeMenu;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = Dynasty8TradeMenu_OnItemSelect;
				UIMenu dynasty8TradeMenu = _Dynasty8TradeMenu;
				if (dynasty8TradeMenu != null)
				{
					dynasty8TradeMenu.OnItemSelect -= value2;
				}
				_Dynasty8TradeMenu = value;
				dynasty8TradeMenu = _Dynasty8TradeMenu;
				if (dynasty8TradeMenu != null)
				{
					dynasty8TradeMenu.OnItemSelect += value2;
				}
			}
		}

		public static void LoadContacts()
		{
			iFruit = new CustomiFruit();
			iFruit.Contacts.Clear();
			MechanicContact = new iFruitContact(Game.GetLocalizedString("CELL_E_MP0"));
			iFruitContact mechanicContact = MechanicContact;
			mechanicContact.DialTimeout = 4000;
			mechanicContact.Active = true;
			mechanicContact.Icon = ContactIcon.LSCustoms;
			iFruit.Contacts.Add(MechanicContact);
			InsuranceContact = new iFruitContact(Game.GetLocalizedString("CELL_E_275"));
			iFruitContact insuranceContact = InsuranceContact;
			insuranceContact.DialTimeout = 4000;
			insuranceContact.Active = true;
			insuranceContact.Icon = ContactIcon.MP_MorsMutual;
			iFruit.Contacts.Add(InsuranceContact);
			Dynasty8Contact = new iFruitContact(Game.GetLocalizedString("BLIP_267"));
			iFruitContact dynasty8Contact = Dynasty8Contact;
			dynasty8Contact.DialTimeout = 4000;
			dynasty8Contact.Active = true;
			dynasty8Contact.Icon = ContactIcon.EstateAgent;
			iFruit.Contacts.Add(Dynasty8Contact);
			MechanicMenu = new UIMenu("", Game.GetLocalizedString("MPCT_MECHH"), new Point(0, -107));
			UIMenu mechanicMenu = MechanicMenu;
			mechanicMenu.SetBannerType(Helper.MenuBanner);
			mechanicMenu.MouseEdgeEnabled = false;
			mechanicMenu.RefreshIndex();
			Helper.MenuPool.Add(MechanicMenu);
			MechanicAptMenu = new UIMenu("", Game.GetLocalizedString("MPCT_PERVEH1"), new Point(0, -107));
			UIMenu mechanicAptMenu = MechanicAptMenu;
			mechanicAptMenu.SetBannerType(Helper.MenuBanner);
			mechanicAptMenu.MouseEdgeEnabled = false;
			mechanicAptMenu.RefreshIndex();
			Helper.MenuPool.Add(MechanicAptMenu);
			InsuranceMenu = new UIMenu("", Game.GetLocalizedString("MPCT_INSH"), new Point(0, -107));
			UIMenu insuranceMenu = InsuranceMenu;
			insuranceMenu.SetBannerType(Helper.MenuBanner);
			insuranceMenu.MouseEdgeEnabled = false;
			insuranceMenu.RefreshIndex();
			Helper.MenuPool.Add(InsuranceMenu);
			Dynasty8Menu = new UIMenu("", Game.GetLocalizedString("BLIP_267").ToUpper(), new Point(0, -107));
			UIMenu dynasty8Menu = Dynasty8Menu;
			dynasty8Menu.SetBannerType(Helper.MenuBanner);
			dynasty8Menu.MouseEdgeEnabled = false;
			dynasty8Menu.AddItem(new UIMenuItem(Game.GetLocalizedString("SC_MSC_PPY"))
			{
				Tag = "Buy"
			});
			dynasty8Menu.AddItem(new UIMenuItem(Game.GetLocalizedString("MP_REP_PROP_0"))
			{
				Tag = "Sell"
			});
			dynasty8Menu.RefreshIndex();
			Helper.MenuPool.Add(Dynasty8Menu);
			Dynasty8BuyMenu = new UIMenu("", Game.GetLocalizedString("DEL_VEH_SEL0").ToUpper(), new Point(0, -107));
			UIMenu dynasty8BuyMenu = Dynasty8BuyMenu;
			dynasty8BuyMenu.SetBannerType(Helper.MenuBanner);
			dynasty8BuyMenu.MouseEdgeEnabled = false;
			dynasty8BuyMenu.RefreshIndex();
			Helper.MenuPool.Add(Dynasty8BuyMenu);
			Dynasty8TradeMenu = new UIMenu("", Game.GetLocalizedString("DEL_VEH_SEL0").ToUpper(), new Point(0, -107));
			UIMenu dynasty8TradeMenu = Dynasty8TradeMenu;
			dynasty8TradeMenu.SetBannerType(Helper.MenuBanner);
			dynasty8TradeMenu.MouseEdgeEnabled = false;
			dynasty8TradeMenu.RefreshIndex();
			Helper.MenuPool.Add(Dynasty8TradeMenu);
		}

		internal static void FinishDelivery(Action releaseModel, Action closeMenu, Action<Exception> report)
		{
			deliveryBusy = false;
			Action[] array = new Action[2] { releaseModel, closeMenu };
			foreach (Action action in array)
			{
				try
				{
					action();
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception obj = ex;
					try
					{
						report(obj);
					}
					catch (Exception projectError)
					{
						ProjectData.SetProjectError(projectError);
						ProjectData.ClearProjectError();
					}
					ProjectData.ClearProjectError();
				}
			}
		}

		private static bool DeliveryContextValid(Ped actor, eOwner owner, Vector3 origin)
		{
			if (!StoryLoadGuard.WorldLoading() && (object)actor != null && actor.Exists() && !actor.IsDead && (object)Helper.PP != null && Helper.PP.Exists() && Helper.PP.Handle == actor.Handle && Helper.GetPlayer() == owner)
			{
				return actor.Position.DistanceToSquared(origin) <= 100f;
			}
			return false;
		}

		public static void RefreshMechanicMenu()
		{
			MechanicMenu.MenuItems.Clear();
			UIMenu mechanicMenu = MechanicMenu;
			foreach (ApartmentClass item2 in Helper.apartments.Concat(SouthLSHousing.RecoveryApartments()))
			{
				if (item2.Owner() == Helper.GetPlayer() && item2.Vehicles().Count != 0)
				{
					string text = item2.FriendlyName();
					if (SouthLSHousing.ArchiveGarageFolder(item2.ID) != null && item2.ID == 9302)
					{
						text += " (saved vehicles)";
					}
					UIMenuItem item = new UIMenuItem(text)
					{
						Tag = item2,
						Enabled = (item2.Vehicles().Count != 0)
					};
					mechanicMenu.AddItem(item);
				}
			}
			foreach (Mansion item3 in Mansions.MansionList.Where((_Closure_0024__._0024I41_002D0 != null) ? _Closure_0024__._0024I41_002D0 : (_Closure_0024__._0024I41_002D0 = _Closure_0024__._0024I._Lambda_0024__41_002D0)))
			{
				ApartmentClass apartmentClass = Mansions.MansionApartment(item3);
				if (apartmentClass.Vehicles().Count != 0)
				{
					mechanicMenu.AddItem(new UIMenuItem($"{item3.Name} (Mansion)")
					{
						Tag = apartmentClass
					});
				}
			}
			mechanicMenu.RefreshIndex();
			mechanicMenu = null;
		}

		public static void RefreshMechanicApartmentMenu(ApartmentClass apt)
		{
			MechanicAptMenu.MenuItems.Clear();
			UIMenu mechanicAptMenu = MechanicAptMenu;
			int num = MansionFeatures.PodiumUidFor(apt.ID);
			Mansion m = MansionFeatures.MansionForGarageId(apt.ID);
			using (List<VehicleClass>.Enumerator enumerator = apt.Vehicles().GetEnumerator())
			{
				_Closure_0024__42_002D0 closure_0024__42_002D = default(_Closure_0024__42_002D0);
				while (enumerator.MoveNext())
				{
					closure_0024__42_002D = new _Closure_0024__42_002D0(closure_0024__42_002D);
					closure_0024__42_002D._0024VB_0024Local_veh = enumerator.Current;
					if ((num == 0 || closure_0024__42_002D._0024VB_0024Local_veh.UniqueID != num) && (apt.ID != 9310 || !ExecutiveGarage.IsVehicleLoaded(closure_0024__42_002D._0024VB_0024Local_veh.UniqueID)) && !MansionFeatures.IsVehicleBusy(m, closure_0024__42_002D._0024VB_0024Local_veh.UniqueID))
					{
						Vehicle vehicle = Helper.outVehicleList.Find(closure_0024__42_002D._Lambda_0024__0);
						VehDeliveryMenuItem tag = new VehDeliveryMenuItem(closure_0024__42_002D._0024VB_0024Local_veh, Helper.IsGarageVehicleAlreadyExistInWorldMap(apt.ID, closure_0024__42_002D._0024VB_0024Local_veh.UniqueID));
						UIMenuItem uIMenuItem = new UIMenuItem($"{closure_0024__42_002D._0024VB_0024Local_veh.Make} {closure_0024__42_002D._0024VB_0024Local_veh.Name} ({closure_0024__42_002D._0024VB_0024Local_veh.PlateNumber})", Game.GetLocalizedString("MPCT_PERVEHC"))
						{
							Tag = tag
						};
						if (vehicle != null && vehicle.IsDead)
						{
							string text = Game.GetLocalizedString("MPCT_PVNAMEDES1").Replace("~a~", $"{closure_0024__42_002D._0024VB_0024Local_veh.Make} {closure_0024__42_002D._0024VB_0024Local_veh.Name} ({closure_0024__42_002D._0024VB_0024Local_veh.PlateNumber})");
							uIMenuItem = new UIMenuItem(text, Game.GetLocalizedString("MPCT_PERVEHC"))
							{
								Tag = tag,
								Enabled = false
							};
						}
						mechanicAptMenu.AddItem(uIMenuItem);
						MechanicMenu.BindMenuToItem(MechanicAptMenu, uIMenuItem);
					}
				}
			}
			mechanicAptMenu.RefreshIndex();
			mechanicAptMenu = null;
		}

		private static void MechanicContact_Answered(iFruitContact contact)
		{
			RefreshMechanicMenu();
			MechanicMenu.Visible = true;
			int num = new Random().Next(1, 3);
			if (num == 1)
			{
				Helper.Play(Helper.YouNeedSomething);
			}
			else
			{
				Helper.Play(Helper.OnTheClock);
			}
			iFruit.Close(5000);
		}

		private static void MechanicMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			RefreshMechanicApartmentMenu((ApartmentClass)selectedItem.Tag);
			MechanicAptMenu.Visible = true;
			sender.Visible = false;
		}

		private static void MechanicAptMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			_Closure_0024__45_002D1 closure_0024__45_002D = new _Closure_0024__45_002D1();
			closure_0024__45_002D._0024VB_0024Local_sender = sender;
			if (deliveryBusy || StoryLoadGuard.WorldLoading() || closure_0024__45_002D._0024VB_0024Local_sender != MechanicAptMenu || !closure_0024__45_002D._0024VB_0024Local_sender.Visible || index < 0 || index >= closure_0024__45_002D._0024VB_0024Local_sender.MenuItems.Count || closure_0024__45_002D._0024VB_0024Local_sender.MenuItems[index] != selectedItem)
			{
				return;
			}
			deliveryBusy = true;
			try
			{
				_Closure_0024__45_002D0 closure_0024__45_002D2 = new _Closure_0024__45_002D0();
				VehDeliveryMenuItem vehDeliveryMenuItem = selectedItem.Tag as VehDeliveryMenuItem;
				if (vehDeliveryMenuItem == null || vehDeliveryMenuItem.VehClass == null)
				{
					return;
				}
				closure_0024__45_002D2._0024VB_0024Local_veh = vehDeliveryMenuItem.VehClass;
				closure_0024__45_002D2._0024VB_0024Local_owner = Helper.GetPlayer();
				closure_0024__45_002D2._0024VB_0024Local_actor = Helper.PP;
				closure_0024__45_002D2._0024VB_0024Local_origin = Helper.PP.Position;
				closure_0024__45_002D2._0024VB_0024Local_mansion = MansionFeatures.MansionForGarageId(closure_0024__45_002D2._0024VB_0024Local_veh.ApartmentID);
				closure_0024__45_002D2._0024VB_0024Local_apartment = Helper.apartments.FirstOrDefault(closure_0024__45_002D2._Lambda_0024__0);
				if (closure_0024__45_002D2._0024VB_0024Local_apartment == null)
				{
					closure_0024__45_002D2._0024VB_0024Local_apartment = SouthLSHousing.RecoveryApartmentForId(closure_0024__45_002D2._0024VB_0024Local_veh.ApartmentID);
				}
				Func<VehicleClass> func = closure_0024__45_002D2._Lambda_0024__1;
				VehicleClass vehicleClass = func();
				if (vehicleClass == null || MansionFeatures.IsVehicleBusy(closure_0024__45_002D2._0024VB_0024Local_mansion, closure_0024__45_002D2._0024VB_0024Local_veh.UniqueID))
				{
					Notification.Show("This saved vehicle is no longer available for delivery.");
					return;
				}
				Vehicle vehicle = Helper.outVehicleList.FirstOrDefault(closure_0024__45_002D2._Lambda_0024__4);
				if ((object)vehicle != null && (vehicle.IsDead || vehicle.Occupants.Length > 0))
				{
					Notification.Show(vehicle.IsDead ? "Recover this vehicle through Insurance first." : "The vehicle is occupied and cannot be moved.");
					return;
				}
				closure_0024__45_002D._0024VB_0024Local_model = new Model(vehicleClass.Hash);
				closure_0024__45_002D._0024VB_0024Local_requestedModel = true;
				if (!closure_0024__45_002D._0024VB_0024Local_model.IsInCdImage || !closure_0024__45_002D._0024VB_0024Local_model.IsValid || !closure_0024__45_002D._0024VB_0024Local_model.Request(1500))
				{
					Notification.Show("This vehicle model is unavailable. Its garage save has been kept.");
				}
				else
				{
					if (!DeliveryContextValid(closure_0024__45_002D2._0024VB_0024Local_actor, closure_0024__45_002D2._0024VB_0024Local_owner, closure_0024__45_002D2._0024VB_0024Local_origin))
					{
						return;
					}
					Vector3 min;
					Vector3 max;
					closure_0024__45_002D._0024VB_0024Local_model.GetDimensions(out min, out max);
					if (!VehiclePlacementProbe.ValidBounds(min, max))
					{
						Notification.Show("This vehicle's dimensions cannot be checked for delivery.");
						return;
					}
					Func<bool> func2 = closure_0024__45_002D2._Lambda_0024__5;
					Quaternion pose = default(Quaternion);
					bool unavailable = default(bool);
					if (!VehiclePlacementProbe.TryFindDelivery(closure_0024__45_002D2._0024VB_0024Local_origin, min, max, vehicle?.Handle ?? 0, func2, ref pose, ref unavailable))
					{
						if (func2())
						{
							Helper.Play(Helper.ICantGetYourRide);
							Logger.Log($"Mechanic delivery search: home={closure_0024__45_002D2._0024VB_0024Local_veh.ApartmentID}; uid={closure_0024__45_002D2._0024VB_0024Local_veh.UniqueID}; origin={closure_0024__45_002D2._0024VB_0024Local_origin}; radius={120f}; queryUnavailable={unavailable}.");
							Notification.Show(unavailable ? "Delivery collision data is not ready. Please try again in a moment." : "No clear delivery space nearby. Move to a clearer street and try again.");
						}
					}
					else
					{
						if (!func2())
						{
							return;
						}
						vehicleClass = func();
						if (vehicleClass == null || MansionFeatures.IsVehicleBusy(closure_0024__45_002D2._0024VB_0024Local_mansion, closure_0024__45_002D2._0024VB_0024Local_veh.UniqueID))
						{
							return;
						}
						if ((object)vehicle != null)
						{
							if (!vehicle.Exists() || vehicle.IsDead || vehicle.Occupants.Length > 0 || vehicle.GetInt(Helper.vehIdDecor) != vehicleClass.ApartmentID || vehicle.GetInt(Helper.vehUidDecor) != vehicleClass.UniqueID || vehicle.Model.Hash != vehicleClass.Hash)
							{
								return;
							}
							Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, vehicle.Handle, pose.X, pose.Y, pose.Z, false, false, false);
							vehicle.Heading = pose.W;
							vehicle.Velocity = Vector3.Zero;
							vehicle.PlaceOnGround();
							Helper.RegisterOutsideVehicle(vehicle, closure_0024__45_002D2._0024VB_0024Local_owner);
						}
						else
						{
							Vehicle vehicle2 = Helper.CreateGarageVehicle(vehicleClass, pose, vehicleClass.ApartmentID);
							if ((object)vehicle2 == null || !vehicle2.Exists())
							{
								Helper.Play(Helper.ICantGetYourRide);
								return;
							}
							if (!func2() || func() == null)
							{
								if (!StoryLoadGuard.WorldLoading() && vehicle2.Exists())
								{
									vehicle2.Delete();
								}
								return;
							}
							MansionNativeGarage.ReleaseStoredVehicle(closure_0024__45_002D2._0024VB_0024Local_mansion, vehicleClass.UniqueID);
							Helper.RegisterOutsideVehicle(vehicle2, closure_0024__45_002D2._0024VB_0024Local_owner);
							vehicle2.PlaceOnGround();
						}
						Logger.Log($"Mechanic delivered: home={vehicleClass.ApartmentID}; uid={vehicleClass.UniqueID}; position={Helper.ToVector3(pose)}.");
						Helper.Play(Helper.IllBeThere);
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Mechanic delivery: " + ex2.Message + " " + ex2.StackTrace);
				Notification.Show("The vehicle could not be delivered. Its garage save has been kept.");
				ProjectData.ClearProjectError();
			}
			finally
			{
				FinishDelivery(closure_0024__45_002D._Lambda_0024__6, closure_0024__45_002D._Lambda_0024__7, (_Closure_0024__._0024I45_002D8 != null) ? _Closure_0024__._0024I45_002D8 : (_Closure_0024__._0024I45_002D8 = _Closure_0024__._0024I._Lambda_0024__45_002D8));
			}
		}

		private static void MechanicMenu_OnMenuClose(UIMenu sender)
		{
			Helper.Play(Helper.IllGetBackToWork);
		}

		private static bool InsuranceSaveExists(Vehicle veh)
		{
			if ((object)veh == null || !veh.Exists())
			{
				return false;
			}
			try
			{
				int @int = veh.GetInt(Helper.vehIdDecor);
				int int2 = veh.GetInt(Helper.vehUidDecor);
				string text = Mansions.GarageFolderForId(@int);
				if (@int == 0 || int2 == 0 || text == null)
				{
					return false;
				}
				VehicleData value = default(VehicleData);
				return new VehicleData(Path.Combine(Helper.grgXmlPath + text, int2.ToString(CultureInfo.InvariantCulture) + ".xml")).TryRead(ref value) && value.ApartmentID == @int && value.UniqueID == int2 && value.Hash == veh.Model.Hash;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Insurance saved vehicle: " + ex2.Message);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public static void RefreshInsuranceMenu()
		{
			InsuranceMenu.MenuItems.Clear();
			UIMenu insuranceMenu = InsuranceMenu;
			Vehicle[] array = Helper.outVehicleList.ToArray();
			foreach (Vehicle vehicle in array)
			{
				if ((object)vehicle == null || !vehicle.Exists())
				{
					Helper.outVehicleList.Remove(vehicle);
				}
				else if (Helper.Owner(vehicle) == Helper.GetPlayer() && vehicle.IsDead)
				{
					UIMenuItem uIMenuItem = new UIMenuItem($"{Helper.Make(vehicle)} {V2Compat.FriendlyName(vehicle)}", Game.GetLocalizedString("MPCT_INSD"));
					uIMenuItem.SetRightLabel("$500");
					uIMenuItem.Tag = vehicle;
					insuranceMenu.AddItem(uIMenuItem);
				}
			}
			insuranceMenu.RefreshIndex();
			insuranceMenu = null;
		}

		private static void InsuranceContact_Answered(iFruitContact contact)
		{
			RefreshInsuranceMenu();
			InsuranceMenu.Visible = true;
			iFruit.Close(5000);
		}

		private static void InsuranceMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			Vehicle vehicle = selectedItem.Tag as Vehicle;
			eOwner player = Helper.GetPlayer();
			bool flag = vehicle?.Exists() ?? false;
			eOwner vehicleOwner = (flag ? Helper.Owner(vehicle) : eOwner.Nobody);
			bool destroyed = flag && vehicle.IsDead;
			bool tracked = flag && Helper.outVehicleList.Contains(vehicle);
			int balance = default(int);
			if (!Helper.TryGetOwnerMoney(player, ref balance) || !CanClaimInsurance(vehicleOwner, player, flag, destroyed, tracked, balance) || !InsuranceSaveExists(vehicle))
			{
				Notification.Show("Insurance requires your destroyed saved vehicle and $500 in the current story character's account.");
				RefreshInsuranceMenu();
				return;
			}
			if (!Helper.TryChangeOwnerMoney(player, -500))
			{
				Notification.Show("Insurance payment could not be completed. No recovery was started.");
				return;
			}
			try
			{
				try
				{
					Blip blip = V2Compat.CurrentBlip(vehicle);
					if ((object)blip != null && blip.Exists())
					{
						V2Compat.Remove(blip);
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Insurance blip cleanup: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
				vehicle.Delete();
				if (vehicle.Exists())
				{
					throw new InvalidOperationException("The destroyed vehicle could not be released.");
				}
				Helper.outVehicleList.Remove(vehicle);
				sender.Visible = false;
				Notification.Show("Vehicle recovered. Call the Mechanic to collect it.");
			}
			catch (Exception ex3)
			{
				ProjectData.SetProjectError(ex3);
				Exception ex4 = ex3;
				bool flag2 = Helper.TryChangeOwnerMoney(player, 500);
				Logger.Log("Insurance recovery failed; refund=" + Conversions.ToString(flag2) + ": " + ex4.Message);
				Notification.Show(flag2 ? "Recovery failed; the $500 fee was refunded." : "Recovery failed and the refund could not be confirmed. See SPA II.log.");
				ProjectData.ClearProjectError();
			}
		}

		public static bool CanClaimInsurance(eOwner vehicleOwner, eOwner currentOwner, bool exists, bool destroyed, bool tracked, int balance)
		{
			if (currentOwner >= eOwner.Michael && currentOwner <= eOwner.Trevor && vehicleOwner == currentOwner && exists && destroyed && tracked)
			{
				return balance >= 500;
			}
			return false;
		}

		public static void RefreshDynasty8BuyMenu()
		{
			Dynasty8BuyMenu.MenuItems.Clear();
			UIMenu dynasty8BuyMenu = Dynasty8BuyMenu;
			foreach (Mansion item in Mansions.MansionList.Where((_Closure_0024__._0024I52_002D0 != null) ? _Closure_0024__._0024I52_002D0 : (_Closure_0024__._0024I52_002D0 = _Closure_0024__._0024I._Lambda_0024__52_002D0)))
			{
				UIMenuItem uIMenuItem = new UIMenuItem($"{item.Name} (Mansion)", Helper.Truncate(item.Description));
				uIMenuItem.Tag = item;
				uIMenuItem.SetRightLabel(string.Format("${0}", item.Price.ToString("0,0")));
				dynasty8BuyMenu.AddItem(uIMenuItem);
			}
			IEnumerable<ApartmentClass> enumerable = Helper.apartments.Where((_Closure_0024__._0024I52_002D1 != null) ? _Closure_0024__._0024I52_002D1 : (_Closure_0024__._0024I52_002D1 = _Closure_0024__._0024I._Lambda_0024__52_002D1));
			foreach (ApartmentClass item2 in enumerable)
			{
				UIMenuItem uIMenuItem2 = new UIMenuItem(item2.FriendlyName(), Helper.Truncate(item2.FriendlyDescription()));
				uIMenuItem2.Tag = item2;
				uIMenuItem2.SetRightLabel(string.Format("${0}", item2.Price.ToString("0,0")));
				dynasty8BuyMenu.AddItem(uIMenuItem2);
				Dynasty8Menu.BindMenuToItem(Dynasty8BuyMenu, uIMenuItem2);
			}
			dynasty8BuyMenu.RefreshIndex();
			dynasty8BuyMenu = null;
		}

		public static void RefreshDynasty8TradeMenu()
		{
			Dynasty8TradeMenu.MenuItems.Clear();
			UIMenu dynasty8TradeMenu = Dynasty8TradeMenu;
			foreach (Mansion item in Mansions.MansionList.Where((_Closure_0024__._0024I53_002D0 != null) ? _Closure_0024__._0024I53_002D0 : (_Closure_0024__._0024I53_002D0 = _Closure_0024__._0024I._Lambda_0024__53_002D0)))
			{
				UIMenuItem uIMenuItem = new UIMenuItem($"{item.Name} (Mansion)", Game.GetLocalizedString("MP_REP_PROP_3"));
				uIMenuItem.Tag = item;
				uIMenuItem.SetRightLabel(string.Format("${0}", checked((int)Math.Round((double)item.Price * 0.5)).ToString("0,0")));
				dynasty8TradeMenu.AddItem(uIMenuItem);
			}
			IEnumerable<ApartmentClass> enumerable = Helper.apartments.Where((_Closure_0024__._0024I53_002D1 != null) ? _Closure_0024__._0024I53_002D1 : (_Closure_0024__._0024I53_002D1 = _Closure_0024__._0024I._Lambda_0024__53_002D1));
			foreach (ApartmentClass item2 in enumerable)
			{
				UIMenuItem uIMenuItem2 = new UIMenuItem(item2.FriendlyName(), Game.GetLocalizedString("MP_REP_PROP_3"));
				uIMenuItem2.Tag = item2;
				uIMenuItem2.SetRightLabel(string.Format("${0}", PropertySale.FairSalePrice(item2.Price).ToString("0,0")));
				dynasty8TradeMenu.AddItem(uIMenuItem2);
				Dynasty8Menu.BindMenuToItem(Dynasty8TradeMenu, uIMenuItem2);
			}
			dynasty8TradeMenu.RefreshIndex();
			dynasty8TradeMenu = null;
		}

		private static void Dynasty8Contact_Answered(iFruitContact contact)
		{
			Dynasty8Menu.Visible = true;
		}

		private static void Dynasty8Menu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			object tag = selectedItem.Tag;
			if (Operators.ConditionalCompareObjectEqual(tag, "Buy", false))
			{
				RefreshDynasty8BuyMenu();
				Dynasty8BuyMenu.Visible = true;
				sender.Visible = false;
			}
			else if (Operators.ConditionalCompareObjectEqual(tag, "Sell", false))
			{
				RefreshDynasty8TradeMenu();
				Dynasty8TradeMenu.Visible = true;
				sender.Visible = false;
			}
		}

		private static void Dynasty8BuyMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			if (selectedItem.Tag is Mansion)
			{
				Mansion mansion = (Mansion)selectedItem.Tag;
				if (Mansions.BuyMansion(mansion, false))
				{
					Dynasty8BuyMenu.Visible = false;
					iFruit.Close();
					Mansions.SetMansionWaypoint(mansion);
					Notification.Show($"~y~Dynasty 8~w~: {mansion.Name} is yours. Waypoint set.");
				}
				return;
			}
			ApartmentClass apartmentClass = selectedItem.Tag as ApartmentClass;
			if (apartmentClass == null)
			{
				return;
			}
			BuildingClass building = apartmentClass.Building();
			if (SouthLSHousingPurchase.BuyFromPhone(building, apartmentClass))
			{
				Dynasty8BuyMenu.Visible = false;
				iFruit.Close();
				Vector3 point = SouthLSHousingExterior.MapEntrancePosition(building);
				if (PartyPlacement.FinitePoint(point))
				{
					Function.Call(Hash.SET_NEW_WAYPOINT, point.X, point.Y);
				}
				Notification.Show("~y~Dynasty 8~w~: " + apartmentClass.FriendlyName() + " is yours. Waypoint set.");
			}
		}

		private static void Dynasty8TradeMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			if (selectedItem.Tag is Mansion)
			{
				Mansion mansion = (Mansion)selectedItem.Tag;
				if (Mansions.SellMansion(mansion, checked((int)Math.Round((double)mansion.Price * 0.5))))
				{
					Dynasty8TradeMenu.Visible = false;
					iFruit.Close();
					Notification.Show($"~y~Dynasty 8~w~: {mansion.Name} sold. Vehicles in its garage stay stored.");
				}
			}
			else
			{
				ApartmentClass apartmentClass = selectedItem.Tag as ApartmentClass;
				if (apartmentClass != null && PropertySale.Sell(apartmentClass))
				{
					Dynasty8TradeMenu.Visible = false;
					iFruit.Close();
				}
			}
		}
	}
}
