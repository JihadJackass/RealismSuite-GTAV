using System;
using System.Collections.Generic;
using System.IO;
using AddonWeapons2.Common;
using AddonWeapons2.Weapons;
using GTA;
using GTA.Math;
using GTA.Native;
using LemonUI.Menus;

namespace AddonWeapons2.UI
{
	public static class Items
	{
		private static readonly IWeaponTintService _tintService = new WeaponTintService();

		private static readonly IWeaponManager _weaponManager = new WeaponManager();

		private static readonly IBadgeProvider _badgeProvider = new BadgeProvider();

		private const float PreviewHoverHeight = 0.9f;

		private const float PreviewForwardOffset = 0.5f;

		private const float PreviewSpinSpeed = 45f;

		private static Vector3 _previewBasePos;

		private static float _previewYaw;

		private static readonly Dictionary<uint, WeaponCategory> _groupToCategory = new Dictionary<uint, WeaponCategory>
		{
			{
				2725924767u,
				WeaponCategory.Heavy
			},
			{
				3566412244u,
				WeaponCategory.Melee
			},
			{
				1159398588u,
				WeaponCategory.MG
			},
			{
				416676503u,
				WeaponCategory.Pistol
			},
			{
				970310034u,
				WeaponCategory.Rifle
			},
			{
				860033945u,
				WeaponCategory.Shotgun
			},
			{
				3337201093u,
				WeaponCategory.SMG
			},
			{
				3082541095u,
				WeaponCategory.Sniper
			},
			{
				690389602u,
				WeaponCategory.StunGun
			},
			{
				1548507267u,
				WeaponCategory.Thrown
			},
			{
				88899580u,
				WeaponCategory.RubberGun
			}
		};

		private static readonly HashSet<uint> _noAmmoGroups = new HashSet<uint> { 3566412244u, 690389602u };

		public static Prop WeaponObject { get; set; }

		public static void Initialize()
		{
			foreach (KeyValuePair<uint, List<WeaponInfo.DlcWeaponDataWithComponents>> weaponCategory in WeaponInfo.WeaponCategories)
			{
				foreach (WeaponInfo.DlcWeaponDataWithComponents item in weaponCategory.Value)
				{
					CreateAndAddWeaponItem(item);
				}
			}
		}

		private static void CreateAndAddWeaponItem(WeaponInfo.DlcWeaponDataWithComponents weapon)
		{
			string weaponName = GetWeaponName(weapon);
			string localizedString = Game.GetLocalizedString(weapon.WeaponData.GetDescLabel());
			int weaponCost = GetWeaponCost(weapon);
			uint weaponHash = weapon.WeaponData.WeaponHash;
			AddToAppropriateMenu(CreateWeaponItem(weapon, weaponName, localizedString, weaponCost, weaponHash), weaponHash);
		}

		public static NativeItem CreateWeaponItem(WeaponInfo.DlcWeaponDataWithComponents weapon, string weaponName, string weaponDesc, int weaponCost, uint weaponHash)
		{
			NativeItem item = new NativeItem(weaponName, weaponDesc, $"${weaponCost}");
			BadgeSet badgeSet = _badgeProvider.GetGunBadge();
			item.Selected += delegate
			{
				OnWeaponSelected(weapon, weaponName, weaponHash, weaponCost, badgeSet, item);
			};
			item.Activated += delegate
			{
				OnWeaponActivated(weapon, weaponName, weaponHash, weaponCost, badgeSet, item);
			};
			if (Game.Player.Character.Weapons.HasWeapon((WeaponHash)weaponHash) && _weaponManager.HasComponentsAvailable(weaponHash))
			{
				item.AltTitle = "";
				item.RightBadgeSet = badgeSet;
			}
			return item;
		}

		private static void OnWeaponActivated(WeaponInfo.DlcWeaponDataWithComponents weapon, string weaponName, uint weaponHash, int cost, BadgeSet badgeSet, NativeItem item)
		{
			WeaponMenu.ComponentMenu.Name = weaponName;
			if (PlayerUtils.CanAfford(cost))
			{
				if (Game.Player.Character.Weapons.HasWeapon((WeaponHash)weaponHash))
				{
					HandleExistingWeapon(weapon, weaponHash, badgeSet, item);
				}
				else
				{
					PurchaseNewWeapon(weaponHash, weapon.Components, cost, badgeSet, item);
				}
			}
		}

		private static void OnWeaponSelected(WeaponInfo.DlcWeaponDataWithComponents weapon, string weaponName, uint weaponHash, int cost, BadgeSet badgeSet, NativeItem item)
		{
			PreviewWeapon(weaponHash);
		}

		public static void PreviewWeapon(uint weaponHash)
		{
			ValueTuple<Vector3, float>? nearestAmmoBox = AmmoShopManager.GetNearestAmmoBox();
			if (nearestAmmoBox.HasValue)
			{
				Ped character = Game.Player.Character;
				uint hash = (uint)character.Model.Hash;
				WeaponInfo.CurrentWeaponHash = weaponHash;
				DeleteWeaponObject();
				WeaponObject = CreateWeaponObject(weaponHash, nearestAmmoBox.Value.Item1);
				if (!(WeaponObject == null) && WeaponObject.Exists())
				{
					SetupPreviewObject(nearestAmmoBox.Value.Item1);
					WeaponManager.ApplyWeaponShopComponents(character, hash, weaponHash, WeaponObject);
					WeaponManager.ApplyWeaponShopTints(character, hash, weaponHash, WeaponObject);
					WeaponPreviewCamera.EnsureActive(_previewBasePos);
				}
			}
		}

		public static Prop CreateWeaponObject(uint WeaponModel, Vector3 position)
		{
			return Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, WeaponModel, 1, position.X, position.Y, position.Z + 0.9f, true, 1f, 0);
		}

		private static void SetupPreviewObject(Vector3 boxPosition)
		{
			_previewBasePos = ComputePreviewBasePos(boxPosition);
			_previewYaw = 0f;
			FreezePreviewObject();
		}

		private static Vector3 ComputePreviewBasePos(Vector3 boxPosition)
		{
			Vector3 vector = Game.Player.Character.Position - boxPosition;
			vector.Z = 0f;
			if (vector.Length() < 0.01f)
			{
				vector = new Vector3(0f, 1f, 0f);
			}
			vector.Normalize();
			return new Vector3(boxPosition.X, boxPosition.Y, boxPosition.Z + 0.9f) + vector * 0.5f;
		}

		private static void FreezePreviewObject()
		{
			Function.Call(Hash.SET_ENTITY_COLLISION, WeaponObject, false, false);
			Function.Call(Hash.FREEZE_ENTITY_POSITION, WeaponObject, true);
			Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, WeaponObject, _previewBasePos.X, _previewBasePos.Y, _previewBasePos.Z, false, false, false);
			Function.Call(Hash.SET_ENTITY_ROTATION, WeaponObject, 0f, 0f, _previewYaw, 2, true);
		}

		public static void RebuildPreviewObject(uint weaponHash)
		{
			if (!(_previewBasePos == Vector3.Zero))
			{
				DeleteWeaponObject();
				WeaponObject = Function.Call<Prop>(Hash.CREATE_WEAPON_OBJECT, weaponHash, 1, _previewBasePos.X, _previewBasePos.Y, _previewBasePos.Z, true, 1f, 0);
				if (!(WeaponObject == null) && WeaponObject.Exists())
				{
					FreezePreviewObject();
					WeaponInfo.PreviewComponent = 0u;
				}
			}
		}

		public static void UpdateWeaponPreview()
		{
			if (!(WeaponObject == null) && WeaponObject.Exists())
			{
				_previewYaw += 45f * Game.LastFrameTime;
				if (_previewYaw >= 360f)
				{
					_previewYaw -= 360f;
				}
				Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, WeaponObject, _previewBasePos.X, _previewBasePos.Y, _previewBasePos.Z, false, false, false);
				Function.Call(Hash.SET_ENTITY_ROTATION, WeaponObject, 0f, 0f, _previewYaw, 2, true);
			}
		}

		public static void DeleteWeaponObject()
		{
			if (WeaponObject != null && WeaponObject.Exists())
			{
				WeaponObject.Delete();
				WeaponObject = null;
			}
		}

		private static void HandleExistingWeapon(WeaponInfo.DlcWeaponDataWithComponents weapon, uint weaponHash, BadgeSet badgeSet, NativeItem item)
		{
			if (!_weaponManager.HasComponentsAvailable(weaponHash))
			{
				Game.Player.Money -= weapon.WeaponData.AmmoCost;
				Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, weaponHash, 1);
				return;
			}
			WeaponInfo.CurrentWeaponHash = weaponHash;
			WeaponMenu.CloseAllMenus();
			WeaponMenu.ComponentMenu.Clear();
			SetupWeaponComponents(weapon, weaponHash);
			WeaponMenu.ComponentMenu.Visible = true;
		}

		private static void SetupWeaponComponents(WeaponInfo.DlcWeaponDataWithComponents weapon, uint weaponHash)
		{
			AddAmmoItemIfNeeded(weapon, weaponHash);
			_tintService.AddTintOptions(weapon, weapon.WeaponData.GetNameLabel(), weaponHash, WeaponMenu.ComponentMenu);
			if (weapon.Components.Count == 0)
			{
				if (weaponHash == 1171102963)
				{
					AddStunGunBailComponent(weapon, weaponHash);
				}
			}
			else
			{
				foreach (WeaponInfo.DlcComponentData component in weapon.Components)
				{
					NativeItem item = ComponentMenuHandler.CreateComponentItem(weapon, Game.GetLocalizedString(component.GetNameLabel()), $"${component.ComponentCost}", component.ComponentCost, component.ComponentHash, weaponHash, weapon.WeaponData.DefaultClipSize, $"${weapon.WeaponData.AmmoCost}");
					WeaponMenu.ComponentMenu.Add(item);
				}
			}
			RefreshComponentMenuAfterAction(weapon);
		}

		private static void AddAmmoItemIfNeeded(WeaponInfo.DlcWeaponDataWithComponents weapon, uint weaponHash)
		{
			if (_weaponManager.UsesAmmo(weaponHash))
			{
				NativeItem item = CreateAmmoItem(weapon.WeaponData.DefaultClipSize, $"${weapon.WeaponData.AmmoCost}", weapon.WeaponData.AmmoCost, weaponHash);
				WeaponMenu.ComponentMenu.Add(item);
			}
		}

		private static void AddStunGunBailComponent(WeaponInfo.DlcWeaponDataWithComponents weapon, uint weaponHash)
		{
			string localizedString = Game.GetLocalizedString("WCT_STNGN_BAIL");
			uint componentHash = Function.Call<uint>(Hash.GET_HASH_KEY, "COMPONENT_STUNGUN_VARMOD_BAIL");
			NativeItem item = ComponentMenuHandler.CreateComponentItem(weapon, localizedString, "$1000", 1000, componentHash, weaponHash, weapon.WeaponData.DefaultClipSize, $"${weapon.WeaponData.AmmoCost}");
			WeaponMenu.ComponentMenu.Add(item);
		}

		private static NativeItem CreateAmmoItem(int defaultClipSize, string ammoCost, int cost, uint weaponHash)
		{
			string title = (_weaponManager.IsMaxAmmo(weaponHash) ? "_MAX_ROUNDS" : $"_ROUNDS x {defaultClipSize}");
			NativeItem item = new NativeItem(title, "", ammoCost);
			item.Activated += delegate
			{
				OnAmmoPurchased(weaponHash, cost, defaultClipSize, item);
			};
			return item;
		}

		private static void OnAmmoPurchased(uint weaponHash, int cost, int clipSize, NativeItem item)
		{
			if (PlayerUtils.CanAfford(cost) && !_weaponManager.IsMaxAmmo(weaponHash))
			{
				Game.Player.Money -= cost;
				Function.Call(Hash.ADD_PED_AMMO_BY_TYPE, Game.Player.Character, Function.Call<Hash>(Hash.GET_PED_AMMO_TYPE_FROM_WEAPON, Game.Player.Character, weaponHash), clipSize);
				uint hash = (uint)Game.Player.Character.Model.Hash;
				int tint = Function.Call<int>(Hash.GET_AMMO_IN_PED_WEAPON, Game.Player.Character, weaponHash);
				WeaponDataUtils.AddWeaponData(4, hash, weaponHash, 0u, tint);
				WeaponSave.SaveWeaponInventory(hash);
				if (_weaponManager.IsMaxAmmo(weaponHash))
				{
					item.AltTitle = "_MAX_ROUNDS";
				}
			}
		}

		private static void PurchaseNewWeapon(uint weaponHash, List<WeaponInfo.DlcComponentData> components, int cost, BadgeSet badgeSet, NativeItem item)
		{
			Game.Player.Money -= cost;
			uint hash = (uint)Game.Player.Character.Model.Hash;
			if (_weaponManager.IsMeleeOrThrown(weaponHash))
			{
				Game.Player.Character.Weapons.Give((WeaponHash)weaponHash, 1, true, true);
				return;
			}
			item.AltTitle = "";
			item.RightBadgeSet = badgeSet;
			Game.Player.Character.Weapons.Give((WeaponHash)weaponHash, 1000, true, true);
			WeaponDataUtils.AddWeaponData(0, hash, weaponHash);
			WeaponSave.SaveWeaponInventory(hash);
		}

		private static void RefreshComponentMenuAfterAction(WeaponInfo.DlcWeaponDataWithComponents weapon)
		{
			List<uint> componentsList = _weaponManager.GetComponentsList(weapon);
			List<int> componentsCost = _weaponManager.GetComponentsCost(weapon);
			ComponentMenuHandler.RefreshComponentMenu(WeaponMenu.ComponentMenu, componentsList, componentsCost);
		}

		private static string GetWeaponName(WeaponInfo.DlcWeaponDataWithComponents weapon)
		{
			string text = Game.GetLocalizedString(weapon.WeaponData.GetNameLabel());
			if (string.IsNullOrEmpty(text) || text.Length < 3)
			{
				text = weapon.WeaponData.GetNameLabel();
				if (text == "WT_SNOWLAUNCHER")
				{
					text = Game.GetLocalizedString("WT_SNOWLNCHR");
				}
			}
			return text;
		}

		private static int GetWeaponCost(WeaponInfo.DlcWeaponDataWithComponents weapon)
		{
			return weapon.WeaponData.WeaponCost;
		}

		private static void AddToAppropriateMenu(NativeItem weaponItem, uint weaponHash)
		{
			if (!TryAddToCustomMenu(weaponItem, weaponHash))
			{
				uint num = Function.Call<uint>(Hash.GET_WEAPONTYPE_GROUP, weaponHash);
				WeaponCategory value;
				if (_groupToCategory.TryGetValue(num, out value))
				{
					WeaponMenu.GetMenuForCategory(value)?.Add(weaponItem);
				}
				if (_noAmmoGroups.Contains(num))
				{
					WeaponInfo.NoAmmoWeapons.Add(weaponHash);
				}
			}
		}

		private static bool TryAddToCustomMenu(NativeItem weaponItem, uint weaponHash)
		{
			if (!File.Exists("Scripts\\AddonWeapons\\commandline.txt"))
			{
				return false;
			}
			foreach (string item in File.ReadLines("Scripts\\AddonWeapons\\commandline.txt"))
			{
				string text = item.Trim();
				if (!text.StartsWith("PutWeaponToCategory"))
				{
					continue;
				}
				string[] array = _weaponManager.ExtractParameters(text, "PutWeaponToCategory", 2);
				if (array == null || new Model(array[0]).Hash != (int)weaponHash)
				{
					continue;
				}
				foreach (NativeMenu customMenus in WeaponMenu.CustomMenusList)
				{
					if (customMenus.Name.Equals(array[1]))
					{
						customMenus.Add(weaponItem);
						return true;
					}
				}
			}
			return false;
		}
	}
}
