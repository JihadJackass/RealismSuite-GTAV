using System;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class SouthLSHousingPurchase
	{
		[CompilerGenerated]
		internal sealed class _Closure_0024__15_002D0
		{
			public BuildingClass _0024VB_0024Local_building;

			public ApartmentClass _0024VB_0024Local_apartment;

			public Ped _0024VB_0024Local_actor;

			public IntPtr _0024VB_0024Local_address;

			public int _0024VB_0024Local_model;

			public eOwner _0024VB_0024Local_owner;

			public int _0024VB_0024Local_expectedId;

			public string _0024VB_0024Local_expectedKey;

			public int _0024VB_0024Local_expectedPrice;

			public bool _0024VB_0024Local_fromPhone;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				return ContextValid(_0024VB_0024Local_building, _0024VB_0024Local_apartment, _0024VB_0024Local_actor, _0024VB_0024Local_address, _0024VB_0024Local_model, _0024VB_0024Local_owner, _0024VB_0024Local_expectedId, _0024VB_0024Local_expectedKey, _0024VB_0024Local_expectedPrice, _0024VB_0024Local_fromPhone);
			}

			[SpecialName]
			internal int? _Lambda_0024__1()
			{
				return ReadCapturedBalance(_0024VB_0024Local_owner);
			}

			[SpecialName]
			internal bool _Lambda_0024__2(int amount)
			{
				return SetCapturedBalance(_0024VB_0024Local_owner, amount);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__5_002D0
		{
			public string _0024VB_0024Local_ownerKey;

			public int _0024VB_0024Local_cost;

			[SpecialName]
			internal bool _Lambda_0024__0(SouthLSHousing.Definition item)
			{
				if (Operators.CompareString(item.LabelKey, _0024VB_0024Local_ownerKey, false) == 0)
				{
					return item.Price == _0024VB_0024Local_cost;
				}
				return false;
			}
		}

		public const string ConfigPath = "scripts\\SPA II\\modconfig.ini";

		private static void Note(string message)
		{
			try
			{
				Logger.Log(message);
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
			}
		}

		public static bool CanAfford(eOwner currentOwner, eOwner purchaser, int balance, int cost)
		{
			if (currentOwner == eOwner.Nobody && purchaser >= eOwner.Michael && purchaser <= eOwner.Trevor && balance >= 0 && cost > 0)
			{
				return balance >= cost;
			}
			return false;
		}

		public static bool ReadOwnerValue(string raw, ref eOwner owner)
		{
			owner = eOwner.Nobody;
			if (raw == null)
			{
				return true;
			}
			return !string.IsNullOrWhiteSpace(raw) && raw.IndexOf(',') < 0 && Enum.TryParse<eOwner>(raw, true, out owner) && owner >= eOwner.Nobody && owner <= eOwner.Others && Enum.IsDefined(typeof(eOwner), owner);
		}

		private static bool ReadOwner(ScriptSettings settings, string key, ref eOwner owner)
		{
			return ReadOwnerValue(settings.GetValue<string>("BUILDING", key, null), ref owner);
		}

		public static bool TryCommitPurchase(string path, string ownerKey, eOwner purchaser, int cost, Func<bool> identityValid, Func<int?> readBalance, Func<int, bool> setBalance, ref string failure)
		{
			_Closure_0024__5_002D0 closure_0024__5_002D = new _Closure_0024__5_002D0();
			closure_0024__5_002D._0024VB_0024Local_ownerKey = ownerKey;
			closure_0024__5_002D._0024VB_0024Local_cost = cost;
			failure = null;
			if (SouthLSHousing.Definitions().FirstOrDefault(closure_0024__5_002D._Lambda_0024__0) == null)
			{
				failure = "This South LS property or price is not supported.";
				return false;
			}
			return TryCommitRegisteredPurchase(path, closure_0024__5_002D._0024VB_0024Local_ownerKey, purchaser, closure_0024__5_002D._0024VB_0024Local_cost, identityValid, readBalance, setBalance, ref failure);
		}

		public static bool TryCommitRegisteredPurchase(string path, string ownerKey, eOwner purchaser, int cost, Func<bool> registeredIdentityValid, Func<int?> readBalance, Func<int, bool> setBalance, ref string failure)
		{
			failure = null;
			if (cost <= 0 || string.IsNullOrWhiteSpace(ownerKey) || registeredIdentityValid == null)
			{
				failure = "The registered property identity or price is invalid.";
				return false;
			}
			return PropertyTransactions.TryCommitOwnerChange(path, ownerKey, eOwner.Nobody, purchaser, purchaser, checked(-cost), registeredIdentityValid, readBalance, setBalance, ref failure);
		}

		private static string CashStat(eOwner owner)
		{
			switch (owner)
			{
			case eOwner.Michael:
				return "SP0_TOTAL_CASH";
			case eOwner.Franklin:
				return "SP1_TOTAL_CASH";
			case eOwner.Trevor:
				return "SP2_TOTAL_CASH";
			default:
				return null;
			}
		}

		private static int? ReadCapturedBalance(eOwner owner)
		{
			int balance = default(int);
			return (!Helper.TryGetOwnerMoney(owner, ref balance)) ? null : new int?(balance);
		}

		private static bool SetCapturedBalance(eOwner owner, int balance)
		{
			string text = CashStat(owner);
			if (text == null || balance < 0)
			{
				return false;
			}
			return Function.Call<bool>(Hash.STAT_SET_INT, Game.GenerateHash(text), balance, true);
		}

		public static bool AtPurchaseDoor(Vector3 player, Vector3 door)
		{
			if (!PartyPlacement.FinitePoint(player) || !PartyPlacement.FinitePoint(door))
			{
				return false;
			}
			Vector3 vector = player - door;
			return vector.X * vector.X + vector.Y * vector.Y <= 25f && Math.Abs(vector.Z) <= 2.5f;
		}

		public static bool RegisteredPurchaseMatches(BuildingClass building, ApartmentClass apartment, int expectedId, string expectedKey, int expectedPrice)
		{
			if (building == null || apartment == null || Helper.buildings == null || !Helper.buildings.Contains(building) || building.Apartments == null || !building.Apartments.Contains(apartment) || Helper.apartments == null || !Helper.apartments.Contains(apartment) || expectedId <= 0 || string.IsNullOrWhiteSpace(expectedKey) || expectedPrice <= 0 || apartment.ID != expectedId || !string.Equals(apartment.Name, expectedKey, StringComparison.Ordinal) || apartment.Price != expectedPrice)
			{
				return false;
			}
			SouthLSHousing.Definition definition = SouthLSHousing.DefinitionForID(expectedId);
			if (definition != null)
			{
				return string.Equals(expectedKey, definition.LabelKey, StringComparison.Ordinal) && expectedPrice == definition.Price;
			}
			return expectedId < 9301 || expectedId > 9304;
		}

		private static bool ContextValid(BuildingClass building, ApartmentClass apartment, Ped actor, IntPtr address, int model, eOwner owner, int expectedId, string expectedKey, int expectedPrice, bool fromPhone)
		{
			if (!RegisteredPurchaseMatches(building, apartment, expectedId, expectedKey, expectedPrice) || (object)actor == null || !actor.Exists() || actor.MemoryAddress != address || actor.Model.Hash != model || actor.IsDead || actor.IsFalling || actor.IsRagdoll || (object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.Handle != actor.Handle || Helper.GetPlayer() != owner || owner < eOwner.Michael || owner > eOwner.Trevor || DestinationStreaming.IsPlayerSafetyHeld(actor) || PartyService.IsPlayerActionActive || MansionFeatures.IsWorkoutActive || PartyPlayerActions.IsKissing)
			{
				return false;
			}
			if (fromPhone)
			{
				return true;
			}
			if (building.AtGaragePurchaseEntrance(actor))
			{
				return true;
			}
			if (actor.IsInVehicle())
			{
				return false;
			}
			if (SouthLSHousing.IsPropertyID(expectedId))
			{
				return PartyPlacement.FinitePoint(SouthLSHousingExterior.DisplayEntrancePosition(building)) && AtPurchaseDoor(actor.Position, SouthLSHousingExterior.DisplayEntrancePosition(building));
			}
			return PartyPlacement.FinitePoint(actor.Position) && building.SaleSignDistanceFrom(actor.Position, actor.Position.DistanceToSquared(Helper.ToVector3(building.BuildingInPos))) <= 25f;
		}

		public static bool Buy(BuildingClass building, ApartmentClass apartment)
		{
			return ExecutePurchase(building, apartment, false);
		}

		public static bool BuyFromPhone(BuildingClass building, ApartmentClass apartment)
		{
			return ExecutePurchase(building, apartment, true);
		}

		private static bool ExecutePurchase(BuildingClass building, ApartmentClass apartment, bool fromPhone)
		{
			_Closure_0024__15_002D0 closure_0024__15_002D = new _Closure_0024__15_002D0();
			closure_0024__15_002D._0024VB_0024Local_building = building;
			closure_0024__15_002D._0024VB_0024Local_apartment = apartment;
			closure_0024__15_002D._0024VB_0024Local_fromPhone = fromPhone;
			if (closure_0024__15_002D._0024VB_0024Local_building == null || closure_0024__15_002D._0024VB_0024Local_apartment == null || (object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsDead)
			{
				return false;
			}
			closure_0024__15_002D._0024VB_0024Local_actor = Helper.PP;
			closure_0024__15_002D._0024VB_0024Local_owner = Helper.GetPlayer();
			closure_0024__15_002D._0024VB_0024Local_address = closure_0024__15_002D._0024VB_0024Local_actor.MemoryAddress;
			closure_0024__15_002D._0024VB_0024Local_model = closure_0024__15_002D._0024VB_0024Local_actor.Model.Hash;
			closure_0024__15_002D._0024VB_0024Local_expectedId = closure_0024__15_002D._0024VB_0024Local_apartment.ID;
			closure_0024__15_002D._0024VB_0024Local_expectedKey = closure_0024__15_002D._0024VB_0024Local_apartment.Name;
			closure_0024__15_002D._0024VB_0024Local_expectedPrice = closure_0024__15_002D._0024VB_0024Local_apartment.Price;
			if (closure_0024__15_002D._0024VB_0024Local_apartment.Owner() != eOwner.Nobody)
			{
				Notification.Show("This property already has an owner.");
				return false;
			}
			if (!ContextValid(closure_0024__15_002D._0024VB_0024Local_building, closure_0024__15_002D._0024VB_0024Local_apartment, closure_0024__15_002D._0024VB_0024Local_actor, closure_0024__15_002D._0024VB_0024Local_address, closure_0024__15_002D._0024VB_0024Local_model, closure_0024__15_002D._0024VB_0024Local_owner, closure_0024__15_002D._0024VB_0024Local_expectedId, closure_0024__15_002D._0024VB_0024Local_expectedKey, closure_0024__15_002D._0024VB_0024Local_expectedPrice, closure_0024__15_002D._0024VB_0024Local_fromPhone))
			{
				string text = (closure_0024__15_002D._0024VB_0024Local_building.BuyAtGarageEntrance ? "Return to this garage's purchase marker before buying." : "Stand at this home's front door before buying.");
				Notification.Show(closure_0024__15_002D._0024VB_0024Local_fromPhone ? "This phone purchase is no longer available for the current character." : text);
				return false;
			}
			string failure = null;
			VB_0024AnonymousDelegate_0<bool> vB_0024AnonymousDelegate_ = closure_0024__15_002D._Lambda_0024__0;
			VB_0024AnonymousDelegate_0<int?> vB_0024AnonymousDelegate_2 = closure_0024__15_002D._Lambda_0024__1;
			VB_0024AnonymousDelegate_1<int, bool> vB_0024AnonymousDelegate_3 = closure_0024__15_002D._Lambda_0024__2;
			bool num;
			if (!closure_0024__15_002D._0024VB_0024Local_apartment.HasCharacterProfile())
			{
				string _0024VB_0024Local_expectedKey = closure_0024__15_002D._0024VB_0024Local_expectedKey;
				eOwner _0024VB_0024Local_owner = closure_0024__15_002D._0024VB_0024Local_owner;
				int _0024VB_0024Local_expectedPrice = closure_0024__15_002D._0024VB_0024Local_expectedPrice;
				VB_0024AnonymousDelegate_0<bool> vB_0024AnonymousDelegate_4 = vB_0024AnonymousDelegate_;
				Func<bool> registeredIdentityValid = ((vB_0024AnonymousDelegate_4 == null) ? null : new Func<bool>(vB_0024AnonymousDelegate_4.Invoke));
				VB_0024AnonymousDelegate_0<int?> vB_0024AnonymousDelegate_5 = vB_0024AnonymousDelegate_2;
				Func<int?> readBalance = ((vB_0024AnonymousDelegate_5 == null) ? null : new Func<int?>(vB_0024AnonymousDelegate_5.Invoke));
				VB_0024AnonymousDelegate_1<int, bool> vB_0024AnonymousDelegate_6 = vB_0024AnonymousDelegate_3;
				num = TryCommitRegisteredPurchase("scripts\\SPA II\\modconfig.ini", _0024VB_0024Local_expectedKey, _0024VB_0024Local_owner, _0024VB_0024Local_expectedPrice, registeredIdentityValid, readBalance, (vB_0024AnonymousDelegate_6 == null) ? null : new Func<int, bool>(vB_0024AnonymousDelegate_6.Invoke), ref failure);
			}
			else
			{
				string _0024VB_0024Local_expectedKey2 = closure_0024__15_002D._0024VB_0024Local_expectedKey;
				eOwner _0024VB_0024Local_owner2 = closure_0024__15_002D._0024VB_0024Local_owner;
				eOwner _0024VB_0024Local_owner3 = closure_0024__15_002D._0024VB_0024Local_owner;
				int amountDelta = checked(-closure_0024__15_002D._0024VB_0024Local_expectedPrice);
				VB_0024AnonymousDelegate_0<bool> vB_0024AnonymousDelegate_4 = vB_0024AnonymousDelegate_;
				Func<bool> identityValid = ((vB_0024AnonymousDelegate_4 == null) ? null : new Func<bool>(vB_0024AnonymousDelegate_4.Invoke));
				VB_0024AnonymousDelegate_0<int?> vB_0024AnonymousDelegate_5 = vB_0024AnonymousDelegate_2;
				Func<int?> readBalance2 = ((vB_0024AnonymousDelegate_5 == null) ? null : new Func<int?>(vB_0024AnonymousDelegate_5.Invoke));
				VB_0024AnonymousDelegate_1<int, bool> vB_0024AnonymousDelegate_6 = vB_0024AnonymousDelegate_3;
				num = PropertyTransactions.TryCommitCharacterOwnerChange("scripts\\SPA II\\modconfig.ini", _0024VB_0024Local_expectedKey2, eOwner.Nobody, _0024VB_0024Local_owner2, _0024VB_0024Local_owner3, amountDelta, identityValid, readBalance2, (vB_0024AnonymousDelegate_6 == null) ? null : new Func<int, bool>(vB_0024AnonymousDelegate_6.Invoke), ref failure);
			}
			if (!num)
			{
				Notification.Show(failure ?? "The purchase could not be completed.");
				return false;
			}
			try
			{
				ConfigHelper.config = ScriptSettings.Load("scripts\\SPA II\\modconfig.ini");
				closure_0024__15_002D._0024VB_0024Local_building.RefreshSaleSign();
				if (closure_0024__15_002D._0024VB_0024Local_building.BuyMenu != null)
				{
					closure_0024__15_002D._0024VB_0024Local_building.BuyMenu.Visible = false;
				}
				Stability.DestroySpaCameras();
				Helper.HideHud = false;
				closure_0024__15_002D._0024VB_0024Local_building.RefreshBuyMenu();
				closure_0024__15_002D._0024VB_0024Local_building.RefreshAptMenu();
				closure_0024__15_002D._0024VB_0024Local_building.RefreshGrgMenu();
				closure_0024__15_002D._0024VB_0024Local_building.RefreshBlips();
				Helper.PlayPropertyPurchase(closure_0024__15_002D._0024VB_0024Local_apartment.Name);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Note("South LS purchase display: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			Note("South LS home purchased: " + closure_0024__15_002D._0024VB_0024Local_apartment.Name + "; owner=" + closure_0024__15_002D._0024VB_0024Local_owner.ToString() + "; paid=" + Conversions.ToString(closure_0024__15_002D._0024VB_0024Local_apartment.Price));
			return true;
		}
	}
}
