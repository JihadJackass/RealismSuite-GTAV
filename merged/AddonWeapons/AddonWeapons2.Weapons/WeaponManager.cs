using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using AddonWeapons2.UI;
using GTA;
using GTA.Native;

namespace AddonWeapons2.Weapons
{
	public class WeaponManager : IWeaponManager
	{
		public List<uint> GetComponentsList(WeaponInfo.DlcWeaponDataWithComponents weapon)
		{
			List<uint> list = new List<uint>();
			if (weapon?.Components != null)
			{
				foreach (WeaponInfo.DlcComponentData component in weapon.Components)
				{
					if (!ComponentMenuHandler.DisableComponentsList.Contains(component.ComponentHash))
					{
						list.Add(component.ComponentHash);
					}
				}
				return list;
			}
			return list;
		}

		public List<int> GetComponentsCost(WeaponInfo.DlcWeaponDataWithComponents weapon)
		{
			List<int> list = new List<int>();
			if (weapon?.Components != null)
			{
				foreach (WeaponInfo.DlcComponentData component in weapon.Components)
				{
					if (!ComponentMenuHandler.DisableComponentsList.Contains(component.ComponentHash))
					{
						list.Add(component.ComponentCost);
					}
				}
				return list;
			}
			return list;
		}

		public unsafe bool IsMaxAmmo(uint weaponHash)
		{
			int num = 0;
			Function.Call(Hash.GET_MAX_AMMO, Game.Player.Character, weaponHash, &num);
			return Function.Call<int>(Hash.GET_AMMO_IN_PED_WEAPON, Game.Player.Character, weaponHash) >= num;
		}

		public string[] ExtractParameters(string command, string commandName, int paramCount)
		{
			if (string.IsNullOrWhiteSpace(command) || !command.StartsWith(commandName))
			{
				return null;
			}
			return command.Substring(commandName.Length).Trim().Split(new char[1] { ' ' }, paramCount, StringSplitOptions.RemoveEmptyEntries);
		}

		public bool HasComponentsAvailable(uint weaponHash)
		{
			return WeaponInfo.AreComponentsAvailable(weaponHash);
		}

		public uint GetWeaponTypeGroup(uint weaponHash)
		{
			return Function.Call<uint>(Hash.GET_WEAPONTYPE_GROUP, weaponHash);
		}

		public bool UsesAmmo(uint weaponHash)
		{
			return !WeaponInfo.NoAmmoWeapons.Contains(weaponHash);
		}

		public bool IsMeleeOrThrown(uint weaponHash)
		{
			uint weaponTypeGroup = GetWeaponTypeGroup(weaponHash);
			if (weaponTypeGroup != 3566412244u)
			{
				return weaponTypeGroup == 1548507267;
			}
			return true;
		}

		public static void ApplyWeaponShopComponents(Ped ped, uint playerModelHash, uint weaponHash, Prop WeaponObject)
		{
			if (WeaponObject == null || !WeaponObject.Exists())
			{
				WeaponInfo.PreviewComponent = 0u;
				return;
			}
			if (WeaponInfo.PreviewComponent != 0)
			{
				Function.Call(Hash.REMOVE_WEAPON_COMPONENT_FROM_WEAPON_OBJECT, WeaponObject, WeaponInfo.PreviewComponent);
				WeaponInfo.PreviewComponent = 0u;
			}
			Dictionary<uint, List<uint>> value;
			List<uint> value2;
			if (!WeaponInfo.InstalledComponents.TryGetValue(playerModelHash, out value) || !value.TryGetValue(weaponHash, out value2))
			{
				return;
			}
			foreach (uint item in value2)
			{
				LoadComponentModel(item);
				Function.Call(Hash.GIVE_WEAPON_COMPONENT_TO_WEAPON_OBJECT, WeaponObject, item);
			}
		}

		public static void ApplyWeaponShopTints(Ped ped, uint playerModelHash, uint weaponHash, Prop WeaponObject)
		{
			Dictionary<uint, List<int>> value;
			List<int> value2;
			if (WeaponObject == null || !WeaponObject.Exists() || !WeaponInfo.PurchasedTints.TryGetValue(playerModelHash, out value) || !value.TryGetValue(weaponHash, out value2))
			{
				return;
			}
			foreach (int item in value2)
			{
				if (WeaponDataUtils.ContainsWeaponData(5, playerModelHash, weaponHash, 0u, item))
				{
					Function.Call(Hash.SET_WEAPON_OBJECT_TINT_INDEX, WeaponObject, item);
				}
			}
		}

		public static void SetWeaponShopTintPreview(Prop WeaponObject, int tintIndex)
		{
			if (!(WeaponObject == null) && WeaponObject.Exists())
			{
				Function.Call(Hash.SET_WEAPON_OBJECT_TINT_INDEX, WeaponObject, tintIndex);
			}
		}

		public static void SetWeaponShopComponentPreview(Prop WeaponObject, uint componentHash)
		{
			if (!(WeaponObject == null) && WeaponObject.Exists())
			{
				LoadComponentModel(componentHash);
				Function.Call(Hash.GIVE_WEAPON_COMPONENT_TO_WEAPON_OBJECT, WeaponObject, componentHash);
				WeaponInfo.PreviewComponent = componentHash;
			}
		}

		public void SerializeDictionary<T1, T2, T3>(string filePath, Dictionary<T1, Dictionary<T2, List<T3>>> dictionary)
		{
			using (FileStream serializationStream = new FileStream(filePath, FileMode.Create))
			{
				new BinaryFormatter().Serialize(serializationStream, dictionary);
			}
		}

		public Dictionary<T1, Dictionary<T2, List<T3>>> DeserializeDictionary<T1, T2, T3>(string filePath)
		{
			using (FileStream serializationStream = new FileStream(filePath, FileMode.Open))
			{
				return (Dictionary<T1, Dictionary<T2, List<T3>>>)new BinaryFormatter().Deserialize(serializationStream);
			}
		}

		public static int LoadComponentModel(uint component)
		{
			int num = Function.Call<int>(Hash.GET_WEAPON_COMPONENT_TYPE_MODEL, component);
			if (num != 0)
			{
				Function.Call(Hash.REQUEST_MODEL, num);
				int num2 = Game.GameTime + 1000;
				while (!Function.Call<bool>(Hash.HAS_MODEL_LOADED, num) && Game.GameTime < num2)
				{
					WeaponMenu.Pool.Process();
					Script.Wait(0);
				}
				return num;
			}
			return 0;
		}
	}
}
