using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using INMNativeUI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class MansionArmory
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal uint _Lambda_0024__0_002D0(WeaponComponentHash v)
			{
				return (uint)v;
			}

			[SpecialName]
			internal uint _Lambda_0024__0_002D1(WeaponHash v)
			{
				return (uint)v;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__18_002D0
		{
			public uint _0024VB_0024Local_hash;

			[SpecialName]
			internal bool _Lambda_0024__0(uint component)
			{
				return Function.Call<bool>(Hash.DOES_WEAPON_TAKE_WEAPON_COMPONENT, _0024VB_0024Local_hash, component);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__22_002D0
		{
			public Ped _0024VB_0024Local_actor;

			public uint _0024VB_0024Local_hash;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				_0024VB_0024Local_actor.Armor = 100;
				return _0024VB_0024Local_actor.Armor >= 100;
			}

			[SpecialName]
			internal bool _Lambda_0024__2()
			{
				Function.Call(Hash.GIVE_WEAPON_TO_PED, _0024VB_0024Local_actor.Handle, _0024VB_0024Local_hash, 60, false, false);
				return OwnsWeapon(_0024VB_0024Local_hash);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__22_002D1
		{
			public int _0024VB_0024Local_old;

			public _Closure_0024__22_002D0 _0024VB_0024NonLocal__0024VB_0024Closure_2;

			[SpecialName]
			internal void _Lambda_0024__1()
			{
				_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_actor.Armor = _0024VB_0024Local_old;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__22_002D2
		{
			public uint _0024VB_0024Local_ammoType;

			public int _0024VB_0024Local_previousAmmo;

			public _Closure_0024__22_002D0 _0024VB_0024NonLocal__0024VB_0024Closure_3;

			[SpecialName]
			internal void _Lambda_0024__3()
			{
				Function.Call(Hash.REMOVE_WEAPON_FROM_PED, _0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_actor.Handle, _0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_hash);
				Function.Call(Hash.SET_PED_AMMO_BY_TYPE, _0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_actor.Handle, _0024VB_0024Local_ammoType, _0024VB_0024Local_previousAmmo);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__23_002D0
		{
			public Ped _0024VB_0024Local_actor;

			public uint _0024VB_0024Local_hash;

			public _Closure_0024__23_002D0(_Closure_0024__23_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_actor = arg0._0024VB_0024Local_actor;
					_0024VB_0024Local_hash = arg0._0024VB_0024Local_hash;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__4(uint part)
			{
				return HasPart(_0024VB_0024Local_hash, part);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__23_002D1
		{
			public int _0024VB_0024Local_amount;

			public int _0024VB_0024Local_old;

			public _Closure_0024__23_002D0 _0024VB_0024NonLocal__0024VB_0024Closure_2;

			public _Closure_0024__23_002D1(_Closure_0024__23_002D1 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_amount = arg0._0024VB_0024Local_amount;
					_0024VB_0024Local_old = arg0._0024VB_0024Local_old;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				Function.Call(Hash.SET_PED_AMMO, _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_actor.Handle, _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_hash, _0024VB_0024Local_amount, false);
				return Function.Call<int>(Hash.GET_AMMO_IN_PED_WEAPON, _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_actor.Handle, _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_hash) >= _0024VB_0024Local_amount;
			}

			[SpecialName]
			internal void _Lambda_0024__1()
			{
				Function.Call(Hash.SET_PED_AMMO, _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_actor.Handle, _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_hash, _0024VB_0024Local_old, false);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__23_002D2
		{
			public int _0024VB_0024Local_tint;

			public int _0024VB_0024Local_old;

			public _Closure_0024__23_002D0 _0024VB_0024NonLocal__0024VB_0024Closure_3;

			public _Closure_0024__23_002D2(_Closure_0024__23_002D2 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_tint = arg0._0024VB_0024Local_tint;
					_0024VB_0024Local_old = arg0._0024VB_0024Local_old;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__2()
			{
				Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, _0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_actor.Handle, _0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_hash, _0024VB_0024Local_tint);
				return Function.Call<int>(Hash.GET_PED_WEAPON_TINT_INDEX, _0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_actor.Handle, _0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_hash) == _0024VB_0024Local_tint;
			}

			[SpecialName]
			internal void _Lambda_0024__3()
			{
				Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, _0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_actor.Handle, _0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_hash, _0024VB_0024Local_old);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__23_002D3
		{
			public bool _0024VB_0024Local_removing;

			public uint _0024VB_0024Local_component;

			public uint[] _0024VB_0024Local_all;

			public uint[] _0024VB_0024Local_previous;

			public _Closure_0024__23_002D0 _0024VB_0024NonLocal__0024VB_0024Closure_4;

			public _Closure_0024__23_002D3(_Closure_0024__23_002D3 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_removing = arg0._0024VB_0024Local_removing;
					_0024VB_0024Local_component = arg0._0024VB_0024Local_component;
					_0024VB_0024Local_all = arg0._0024VB_0024Local_all;
					_0024VB_0024Local_previous = arg0._0024VB_0024Local_previous;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__5()
			{
				Function.Call(_0024VB_0024Local_removing ? Hash.REMOVE_WEAPON_COMPONENT_FROM_PED : Hash.GIVE_WEAPON_COMPONENT_TO_PED, _0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024Local_actor.Handle, _0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024Local_hash, _0024VB_0024Local_component);
				return HasPart(_0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024Local_hash, _0024VB_0024Local_component) != _0024VB_0024Local_removing;
			}

			[SpecialName]
			internal void _Lambda_0024__6()
			{
				uint[] array = _0024VB_0024Local_all;
				foreach (uint num in array)
				{
					Function.Call(_0024VB_0024Local_previous.Contains(num) ? Hash.GIVE_WEAPON_COMPONENT_TO_PED : Hash.REMOVE_WEAPON_COMPONENT_FROM_PED, _0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024Local_actor.Handle, _0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024Local_hash, num);
				}
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__25_002D0
		{
			public uint _0024VB_0024Local_hash;

			[SpecialName]
			internal bool _Lambda_0024__0(uint part)
			{
				return HasPart(_0024VB_0024Local_hash, part);
			}
		}

		private static Mansion home;

		private static UIMenu menu;

		private static UIMenu weaponMenu;

		private static uint selectedWeapon;

		private static string previewKey;

		private static readonly uint[] componentValues = Enum.GetValues(typeof(WeaponComponentHash)).Cast<WeaponComponentHash>().Select(_Closure_0024__._0024I._Lambda_0024__0_002D0)
			.Distinct()
			.ToArray();

		private static readonly uint[] weaponValues = Enum.GetValues(typeof(WeaponHash)).Cast<WeaponHash>().Select(_Closure_0024__._0024I._Lambda_0024__0_002D1)
			.Distinct()
			.ToArray();

		public static bool IsNear(Mansion m, Vector3 position)
		{
			if (m == null || !MansionFeatures.IsSet(m.ArmoryPos) || !PartyPlacement.FinitePoint(position))
			{
				return false;
			}
			Vector3 vector = position - Helper.ToVector3(m.ArmoryPos);
			return vector.X * vector.X + vector.Y * vector.Y <= 10.24f && Math.Abs(vector.Z) < 2f;
		}

		private static bool CanUse(Mansion m)
		{
			if (m != null && m.Enabled && m.IplsLoaded && m.IsOwnedByPlayer() && MansionFeatures.AtMansion(m) && (object)Helper.PP != null && Helper.PP.Exists() && !Helper.PP.IsDead && !Helper.PP.IsInVehicle() && !PartyService.IsPlayerActionActive && !MansionFeatures.IsWorkoutActive)
			{
				return !PartyPlayerActions.IsKissing;
			}
			return false;
		}

		private static UIMenu MakeMenu(string title)
		{
			UIMenu uIMenu = new UIMenu("", title, new Point(0, -107));
			uIMenu.SetBannerType(Helper.MenuBanner);
			uIMenu.MouseEdgeEnabled = false;
			Helper.MenuPool.Add(uIMenu);
			return uIMenu;
		}

		public static void Open(Mansion m, UIMenu parent = null)
		{
			if (CanUse(m))
			{
				if (menu == null)
				{
					menu = MakeMenu("MANSION ARMORY");
					weaponMenu = MakeMenu("WEAPON WORKBENCH");
					menu.OnItemSelect += SelectWeapon;
					weaponMenu.OnItemSelect += ModifyWeapon;
					menu.OnIndexChange += PreviewIndexChanged;
					weaponMenu.OnIndexChange += PreviewIndexChanged;
				}
				MansionWeaponPreview.BeginSession();
				previewKey = null;
				home = m;
				menu.ParentMenu = parent;
				if (parent != null)
				{
					parent.Visible = false;
				}
				BuildCatalogue();
				menu.Visible = true;
				RefreshPreview();
			}
		}

		public static int PurchasePrice(string group)
		{
			switch (group)
			{
			case "GROUP_MELEE":
				return 1000;
			case "GROUP_PISTOL":
				return 10000;
			case "GROUP_SMG":
				return 20000;
			case "GROUP_SHOTGUN":
				return 25000;
			case "GROUP_RIFLE":
			case "GROUP_MG":
				return 40000;
			case "GROUP_SNIPER":
				return 60000;
			case "GROUP_HEAVY":
				return 90000;
			case "GROUP_THROWN":
				return 5000;
			default:
				return 0;
			}
		}

		public static string ReadableName(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return "Weapon";
			}
			return Regex.Replace(value, "([a-z])([A-Z0-9])", "$1 $2");
		}

		private static string GroupName(uint hash)
		{
			uint num = Function.Call<uint>(Hash.GET_WEAPONTYPE_GROUP, hash);
			string[] array = new string[9] { "GROUP_MELEE", "GROUP_PISTOL", "GROUP_SMG", "GROUP_SHOTGUN", "GROUP_RIFLE", "GROUP_MG", "GROUP_SNIPER", "GROUP_HEAVY", "GROUP_THROWN" };
			foreach (string text in array)
			{
				if (num == checked((uint)(Game.GenerateHash(text) & 0xFFFFFFFFu)))
				{
					return text;
				}
			}
			return null;
		}

		private static bool OwnsWeapon(uint hash)
		{
			return Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, Helper.PP.Handle, hash, false);
		}

		private static string NameOfWeapon(uint hash)
		{
			return ReadableName(Enum.GetName(typeof(WeaponHash), hash) ?? ("Weapon " + hash.ToString("X8")));
		}

		private static void BuildCatalogue()
		{
			menu.Clear();
			UIMenuItem uIMenuItem = new UIMenuItem("Body armour", "Equip full body armour. Charged only after it is applied.")
			{
				Tag = "Armor"
			};
			uIMenuItem.SetRightLabel("$2,500");
			menu.AddItem(uIMenuItem);
			uint[] array = weaponValues;
			foreach (uint num in array)
			{
				if (num != 2725352035u && Function.Call<bool>(Hash.IS_WEAPON_VALID, num))
				{
					int num2 = PurchasePrice(GroupName(num));
					if (num2 > 0)
					{
						bool flag = OwnsWeapon(num);
						UIMenuItem uIMenuItem2 = new UIMenuItem(NameOfWeapon(num), flag ? "3D preview. Open ammunition, compatible parts and finishes." : "3D preview only; select to purchase with starter ammunition. SPA armory prices.")
						{
							Tag = num
						};
						uIMenuItem2.SetRightLabel(flag ? "Owned / modify" : ("$" + num2.ToString("N0")));
						menu.AddItem(uIMenuItem2);
					}
				}
			}
			menu.RefreshIndex();
		}

		private static uint[] Components(uint hash)
		{
			_Closure_0024__18_002D0 closure_0024__18_002D = new _Closure_0024__18_002D0();
			closure_0024__18_002D._0024VB_0024Local_hash = hash;
			return componentValues.Where(closure_0024__18_002D._Lambda_0024__0).ToArray();
		}

		private static bool HasPart(uint hash, uint component)
		{
			return Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON_COMPONENT, Helper.PP.Handle, hash, component);
		}

		private static void BuildWeapon()
		{
			weaponMenu.Clear();
			weaponMenu.Subtitle.Caption = NameOfWeapon(selectedWeapon).ToUpperInvariant();
			UIMenuItem uIMenuItem = new UIMenuItem("Refill ammunition", "Refill to this weapon's supported ammunition limit.")
			{
				Tag = "Ammo"
			};
			uIMenuItem.SetRightLabel("$1,000");
			weaponMenu.AddItem(uIMenuItem);
			uint[] array = Components(selectedWeapon);
			foreach (uint num in array)
			{
				string text = ReadableName(Enum.GetName(typeof(WeaponComponentHash), num));
				bool flag = HasPart(selectedWeapon, num);
				UIMenuItem uIMenuItem2 = new UIMenuItem(text, flag ? "Remove this installed component for free." : "Install this compatible component. Conflicting parts are replaced by the game.")
				{
					Tag = num
				};
				uIMenuItem2.SetRightLabel(flag ? "Installed / remove" : "$2,500");
				weaponMenu.AddItem(uIMenuItem2);
			}
			int num2 = Math.Min(64, Function.Call<int>(Hash.GET_WEAPON_TINT_COUNT, selectedWeapon));
			int num3 = Function.Call<int>(Hash.GET_PED_WEAPON_TINT_INDEX, Helper.PP.Handle, selectedWeapon);
			checked
			{
				int num4 = num2 - 1;
				for (int j = 0; j <= num4; j++)
				{
					UIMenuItem uIMenuItem3 = new UIMenuItem("Finish " + Conversions.ToString(j + 1), "Apply this weapon's supported tint. SPA armory price: $500.")
					{
						Tag = "Tint:" + Conversions.ToString(j)
					};
					uIMenuItem3.SetRightLabel((j == num3) ? "Applied" : "$500");
					weaponMenu.AddItem(uIMenuItem3);
				}
				weaponMenu.ParentMenu = menu;
				weaponMenu.RefreshIndex();
			}
		}

		private static bool Purchase(int cost, Func<bool> apply, Action undo)
		{
			if (!CanUse(home) || cost < 0)
			{
				return false;
			}
			eOwner player = Helper.GetPlayer();
			Ped pP = Helper.PP;
			IntPtr memoryAddress = pP.MemoryAddress;
			int hash = pP.Model.Hash;
			int balance = default(int);
			if (!Helper.TryGetOwnerMoney(player, ref balance) || balance < cost)
			{
				Notification.Show("Not enough money for this armory purchase.");
				return false;
			}
			bool flag = false;
			bool flag2 = false;
			try
			{
				flag = true;
				if (!apply())
				{
					return false;
				}
				if (!pP.Exists() || Helper.PP.Handle != pP.Handle || pP.MemoryAddress != memoryAddress || pP.Model.Hash != hash || Helper.GetPlayer() != player)
				{
					return false;
				}
				if (!Helper.TryChangeOwnerMoney(player, checked(-cost)))
				{
					return false;
				}
				flag2 = true;
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Mansion armory purchase: " + ex2.Message);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
			finally
			{
				if (flag && !flag2 && pP.Exists() && pP.MemoryAddress == memoryAddress && pP.Model.Hash == hash)
				{
					try
					{
						undo();
					}
					catch (Exception ex3)
					{
						ProjectData.SetProjectError(ex3);
						Exception ex4 = ex3;
						Logger.Log("Mansion armory equipment rollback: " + ex4.Message);
						ProjectData.ClearProjectError();
					}
					Notification.Show("The armory purchase could not be completed; no payment was taken.");
				}
			}
		}

		private static void SelectWeapon(UIMenu sender, UIMenuItem item, int index)
		{
			_Closure_0024__22_002D0 closure_0024__22_002D = new _Closure_0024__22_002D0();
			if (!CanUse(home))
			{
				return;
			}
			closure_0024__22_002D._0024VB_0024Local_actor = Helper.PP;
			if (item.Tag is string)
			{
				_Closure_0024__22_002D1 closure_0024__22_002D2 = new _Closure_0024__22_002D1();
				closure_0024__22_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2 = closure_0024__22_002D;
				closure_0024__22_002D2._0024VB_0024Local_old = closure_0024__22_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_actor.Armor;
				if (closure_0024__22_002D2._0024VB_0024Local_old < 100)
				{
					Purchase(2500, closure_0024__22_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2._Lambda_0024__0, closure_0024__22_002D2._Lambda_0024__1);
				}
			}
			else
			{
				if (!(item.Tag is uint))
				{
					return;
				}
				closure_0024__22_002D._0024VB_0024Local_hash = Conversions.ToUInteger(item.Tag);
				if (!OwnsWeapon(closure_0024__22_002D._0024VB_0024Local_hash))
				{
					_Closure_0024__22_002D2 closure_0024__22_002D3 = new _Closure_0024__22_002D2();
					closure_0024__22_002D3._0024VB_0024NonLocal__0024VB_0024Closure_3 = closure_0024__22_002D;
					int num = PurchasePrice(GroupName(closure_0024__22_002D3._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_hash));
					closure_0024__22_002D3._0024VB_0024Local_ammoType = Function.Call<uint>(Hash.GET_PED_AMMO_TYPE_FROM_WEAPON, closure_0024__22_002D3._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_actor.Handle, closure_0024__22_002D3._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_hash);
					closure_0024__22_002D3._0024VB_0024Local_previousAmmo = Function.Call<int>(Hash.GET_PED_AMMO_BY_TYPE, closure_0024__22_002D3._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_actor.Handle, closure_0024__22_002D3._0024VB_0024Local_ammoType);
					if (num <= 0 || !Purchase(num, closure_0024__22_002D3._0024VB_0024NonLocal__0024VB_0024Closure_3._Lambda_0024__2, closure_0024__22_002D3._Lambda_0024__3))
					{
						return;
					}
				}
				selectedWeapon = closure_0024__22_002D._0024VB_0024Local_hash;
				BuildWeapon();
				menu.Visible = false;
				weaponMenu.Visible = true;
				previewKey = null;
				RefreshPreview();
			}
		}

		private static void ModifyWeapon(UIMenu sender, UIMenuItem item, int index)
		{
			_Closure_0024__23_002D0 arg = new _Closure_0024__23_002D0(null);
			if (!CanUse(home) || !OwnsWeapon(selectedWeapon))
			{
				return;
			}
			arg._0024VB_0024Local_actor = Helper.PP;
			arg._0024VB_0024Local_hash = selectedWeapon;
			string text = item.Tag as string;
			if (Operators.CompareString(text, "Ammo", false) == 0)
			{
				_Closure_0024__23_002D1 arg2 = new _Closure_0024__23_002D1(null);
				arg2._0024VB_0024NonLocal__0024VB_0024Closure_2 = arg;
				arg2._0024VB_0024Local_old = Function.Call<int>(Hash.GET_AMMO_IN_PED_WEAPON, arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_actor.Handle, arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_hash);
				OutputArgument outputArgument = new OutputArgument();
				if (!Function.Call<bool>(Hash.GET_MAX_AMMO, arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_actor.Handle, arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_hash, outputArgument))
				{
					return;
				}
				arg2._0024VB_0024Local_amount = outputArgument.GetResult<int>();
				if (arg2._0024VB_0024Local_amount <= arg2._0024VB_0024Local_old || arg2._0024VB_0024Local_amount <= 0)
				{
					return;
				}
				Purchase(1000, arg2._Lambda_0024__0, arg2._Lambda_0024__1);
			}
			else if (text != null && text.StartsWith("Tint:", StringComparison.Ordinal))
			{
				_Closure_0024__23_002D2 arg3 = new _Closure_0024__23_002D2(null);
				arg3._0024VB_0024NonLocal__0024VB_0024Closure_3 = arg;
				if (!int.TryParse(text.Substring(5), out arg3._0024VB_0024Local_tint) || arg3._0024VB_0024Local_tint < 0 || arg3._0024VB_0024Local_tint >= Function.Call<int>(Hash.GET_WEAPON_TINT_COUNT, arg3._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_hash))
				{
					return;
				}
				arg3._0024VB_0024Local_old = Function.Call<int>(Hash.GET_PED_WEAPON_TINT_INDEX, arg3._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_actor.Handle, arg3._0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_hash);
				if (arg3._0024VB_0024Local_old == arg3._0024VB_0024Local_tint)
				{
					return;
				}
				Purchase(500, arg3._Lambda_0024__2, arg3._Lambda_0024__3);
			}
			else if (item.Tag is uint)
			{
				_Closure_0024__23_002D3 arg4 = new _Closure_0024__23_002D3(null);
				arg4._0024VB_0024NonLocal__0024VB_0024Closure_4 = arg;
				arg4._0024VB_0024Local_component = Conversions.ToUInteger(item.Tag);
				if (!Function.Call<bool>(Hash.DOES_WEAPON_TAKE_WEAPON_COMPONENT, arg4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024Local_hash, arg4._0024VB_0024Local_component))
				{
					return;
				}
				arg4._0024VB_0024Local_all = Components(arg4._0024VB_0024NonLocal__0024VB_0024Closure_4._0024VB_0024Local_hash);
				arg4._0024VB_0024Local_previous = arg4._0024VB_0024Local_all.Where(arg4._0024VB_0024NonLocal__0024VB_0024Closure_4._Lambda_0024__4).ToArray();
				arg4._0024VB_0024Local_removing = arg4._0024VB_0024Local_previous.Contains(arg4._0024VB_0024Local_component);
				Purchase((!arg4._0024VB_0024Local_removing) ? 2500 : 0, arg4._Lambda_0024__5, arg4._Lambda_0024__6);
			}
			BuildWeapon();
			previewKey = null;
			RefreshPreview();
		}

		private static void PreviewIndexChanged(UIMenu sender, int index)
		{
			RefreshPreview();
		}

		private static void RefreshPreview()
		{
			_Closure_0024__25_002D0 closure_0024__25_002D = new _Closure_0024__25_002D0();
			if (!CanUse(home))
			{
				MansionWeaponPreview.Cleanup();
				previewKey = null;
			}
			else
			{
				if (MansionWeaponPreview.IsUnavailable)
				{
					return;
				}
				UIMenu uIMenu = ((weaponMenu != null && weaponMenu.Visible) ? weaponMenu : ((menu != null && menu.Visible) ? menu : null));
				if (uIMenu == null || uIMenu.MenuItems.Count == 0)
				{
					MansionWeaponPreview.Cleanup();
					previewKey = null;
					return;
				}
				int currentSelection = uIMenu.CurrentSelection;
				if (currentSelection < 0 || currentSelection >= uIMenu.MenuItems.Count)
				{
					return;
				}
				UIMenuItem uIMenuItem = uIMenu.MenuItems[currentSelection];
				if (uIMenu == weaponMenu)
				{
					closure_0024__25_002D._0024VB_0024Local_hash = selectedWeapon;
				}
				else if (uIMenuItem.Tag is uint)
				{
					closure_0024__25_002D._0024VB_0024Local_hash = Conversions.ToUInteger(uIMenuItem.Tag);
				}
				if (closure_0024__25_002D._0024VB_0024Local_hash == 0)
				{
					if (previewKey != null)
					{
						MansionWeaponPreview.ClearSelection();
					}
					previewKey = null;
					return;
				}
				string b = closure_0024__25_002D._0024VB_0024Local_hash.ToString("X8") + ":" + ((uIMenu == weaponMenu) ? ("detail:" + currentSelection) : "catalogue");
				if (string.Equals(previewKey, b, StringComparison.Ordinal))
				{
					return;
				}
				List<uint> list = new List<uint>();
				bool num = OwnsWeapon(closure_0024__25_002D._0024VB_0024Local_hash);
				if (num)
				{
					list.AddRange(Components(closure_0024__25_002D._0024VB_0024Local_hash).Where(closure_0024__25_002D._Lambda_0024__0));
				}
				int tint = (num ? Function.Call<int>(Hash.GET_PED_WEAPON_TINT_INDEX, Helper.PP.Handle, closure_0024__25_002D._0024VB_0024Local_hash) : 0);
				if (uIMenu == weaponMenu)
				{
					if (uIMenuItem.Tag is uint)
					{
						uint num2 = Conversions.ToUInteger(uIMenuItem.Tag);
						if (Function.Call<bool>(Hash.DOES_WEAPON_TAKE_WEAPON_COMPONENT, closure_0024__25_002D._0024VB_0024Local_hash, num2) && !list.Contains(num2))
						{
							list.Add(num2);
						}
					}
					else
					{
						string text = uIMenuItem.Tag as string;
						int result;
						if (text != null && text.StartsWith("Tint:", StringComparison.Ordinal) && int.TryParse(text.Substring(5), out result) && result >= 0 && result < Function.Call<int>(Hash.GET_WEAPON_TINT_COUNT, closure_0024__25_002D._0024VB_0024Local_hash))
						{
							tint = result;
						}
					}
				}
				previewKey = b;
				MansionWeaponPreview.Queue(home, closure_0024__25_002D._0024VB_0024Local_hash, list.ToArray(), tint);
			}
		}

		public static void FrameTick()
		{
			if (home != null)
			{
				if (!CanUse(home) || ((menu == null || !menu.Visible) && (weaponMenu == null || !weaponMenu.Visible)))
				{
					Cleanup();
					return;
				}
				RefreshPreview();
				MansionWeaponPreview.FrameTick();
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
			if (!Helper.ShowPersistentHelp("Press ~INPUT_CONTEXT~ to buy weapons, ammunition and weapon upgrades."))
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
			if (weaponMenu != null)
			{
				weaponMenu.Visible = false;
			}
			MansionWeaponPreview.Cleanup();
			previewKey = null;
			home = null;
		}
	}
}
