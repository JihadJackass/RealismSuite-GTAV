using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using GTA;
using GTA.Native;

namespace HKHModHelperNew
{
	public class WeaponsHelper : Script
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct DlcWeaponData
		{
			public int emptyCheck;

			public int padding1;

			public uint weaponHash;

			public int padding2;

			public int unk;

			public int padding3;

			public int weaponCost;

			public int padding4;

			public int ammoCost;

			public int padding5;

			public int ammoType;

			public int padding6;

			public int defaultClipSize;

			public int padding7;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public byte[] nameLabel;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public byte[] descLabel;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public byte[] desc2Label;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public byte[] upperCaseNameLabel;

			public string GetNameLabel()
			{
				return GetString(nameLabel);
			}

			public string GetDescLabel()
			{
				return GetString(descLabel);
			}

			public string GetDesc2Label()
			{
				return GetString(desc2Label);
			}

			public string GetUpperCaseNameLabel()
			{
				return GetString(upperCaseNameLabel);
			}

			private string GetString(byte[] byteArray)
			{
				int num = Array.IndexOf(byteArray, (byte)0);
				if (num == -1)
				{
					num = byteArray.Length;
				}
				return Encoding.ASCII.GetString(byteArray, 0, num);
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct DlcComponentData
		{
			public int attachBone;

			public int padding1;

			public int bActiveByDefault;

			public int padding2;

			public int unk;

			public int padding3;

			public int componentHash;

			public int padding4;

			public int unk2;

			public int padding5;

			public int componentCost;

			public int padding6;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public byte[] nameLabel;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public byte[] descLabel;

			public string GetNameLabel()
			{
				return GetString(nameLabel);
			}

			public string GetDescLabel()
			{
				return GetString(descLabel);
			}

			private string GetString(byte[] byteArray)
			{
				int num = Array.IndexOf(byteArray, (byte)0);
				if (num == -1)
				{
					num = byteArray.Length;
				}
				return Encoding.ASCII.GetString(byteArray, 0, num);
			}
		}

		public class DlcWeaponDataWithComponents
		{
			public DlcWeaponData WeaponData { get; }

			public List<DlcComponentData> Components { get; }

			public DlcWeaponDataWithComponents(DlcWeaponData weaponData, List<DlcComponentData> components)
			{
				WeaponData = weaponData;
				Components = components;
			}
		}

		public static List<uint> KnownWeapons = new List<uint>();

		public static List<DlcWeaponDataWithComponents> AddedWeapons = new List<DlcWeaponDataWithComponents>();

		private Dictionary<uint, List<DlcWeaponDataWithComponents>> weaponCategories = new Dictionary<uint, List<DlcWeaponDataWithComponents>>();

		private Dictionary<uint, string> groupNames = new Dictionary<uint, string>();

		private const uint GROUP_DIGISCANNER = 3539449195u;

		private const uint GROUP_FIREEXTINGUISHER = 4257178988u;

		private const uint GROUP_HACKINGDEVICE = 1175761940u;

		private const uint GROUP_HEAVY = 2725924767u;

		private const uint GROUP_MELEE = 3566412244u;

		private const uint GROUP_METALDETECTOR = 3759491383u;

		private const uint GROUP_MG = 1159398588u;

		private const uint GROUP_NIGHTVISION = 3493187224u;

		private const uint GROUP_PARACHUTE = 431593103u;

		private const uint GROUP_PETROLCAN = 1595662460u;

		private const uint GROUP_PISTOL = 416676503u;

		private const uint GROUP_RIFLE = 970310034u;

		private const uint GROUP_SHOTGUN = 860033945u;

		private const uint GROUP_SMG = 3337201093u;

		private const uint GROUP_SNIPER = 3082541095u;

		private const uint GROUP_STUNGUN = 690389602u;

		private const uint GROUP_THROWN = 1548507267u;

		private const uint GROUP_TRANQILIZER = 75159441u;

		private const uint GROUP_UNARMED = 2685387236u;

		public void InitializeGroupNames()
		{
			groupNames.Add(3539449195u, "GROUP_DIGISCANNER");
			groupNames.Add(4257178988u, "GROUP_FIREEXTINGUISHER");
			groupNames.Add(1175761940u, "GROUP_HACKINGDEVICE");
			groupNames.Add(2725924767u, "GROUP_HEAVY");
			groupNames.Add(3566412244u, "GROUP_MELEE");
			groupNames.Add(3759491383u, "GROUP_METALDETECTOR");
			groupNames.Add(1159398588u, "GROUP_MG");
			groupNames.Add(3493187224u, "GROUP_NIGHTVISION");
			groupNames.Add(431593103u, "GROUP_PARACHUTE");
			groupNames.Add(1595662460u, "GROUP_PETROLCAN");
			groupNames.Add(416676503u, "GROUP_PISTOL");
			groupNames.Add(970310034u, "GROUP_RIFLE");
			groupNames.Add(860033945u, "GROUP_SHOTGUN");
			groupNames.Add(3337201093u, "GROUP_SMG");
			groupNames.Add(3082541095u, "GROUP_SNIPER");
			groupNames.Add(690389602u, "GROUP_STUNGUN");
			groupNames.Add(1548507267u, "GROUP_THROWN");
			groupNames.Add(75159441u, "GROUP_TRANQILIZER");
			groupNames.Add(2685387236u, "GROUP_UNARMED");
		}

		public void InitializeCategories()
		{
			weaponCategories.Add(3539449195u, new List<DlcWeaponDataWithComponents>());
			weaponCategories.Add(4257178988u, new List<DlcWeaponDataWithComponents>());
			weaponCategories.Add(1175761940u, new List<DlcWeaponDataWithComponents>());
			weaponCategories.Add(2725924767u, new List<DlcWeaponDataWithComponents>());
			weaponCategories.Add(3566412244u, new List<DlcWeaponDataWithComponents>());
			weaponCategories.Add(3759491383u, new List<DlcWeaponDataWithComponents>());
			weaponCategories.Add(1159398588u, new List<DlcWeaponDataWithComponents>());
			weaponCategories.Add(3493187224u, new List<DlcWeaponDataWithComponents>());
			weaponCategories.Add(431593103u, new List<DlcWeaponDataWithComponents>());
			weaponCategories.Add(1595662460u, new List<DlcWeaponDataWithComponents>());
			weaponCategories.Add(416676503u, new List<DlcWeaponDataWithComponents>());
			weaponCategories.Add(970310034u, new List<DlcWeaponDataWithComponents>());
			weaponCategories.Add(860033945u, new List<DlcWeaponDataWithComponents>());
			weaponCategories.Add(3337201093u, new List<DlcWeaponDataWithComponents>());
			weaponCategories.Add(3082541095u, new List<DlcWeaponDataWithComponents>());
			weaponCategories.Add(690389602u, new List<DlcWeaponDataWithComponents>());
			weaponCategories.Add(1548507267u, new List<DlcWeaponDataWithComponents>());
			weaponCategories.Add(75159441u, new List<DlcWeaponDataWithComponents>());
			weaponCategories.Add(2685387236u, new List<DlcWeaponDataWithComponents>());
		}

		public void GetDlcWeaponModels()
		{
			int num = Function.Call<int>(Hash.GET_NUM_DLC_WEAPONS);
			for (int i = 0; i < num; i++)
			{
				IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(DlcWeaponData)));
				try
				{
					if (Function.Call<bool>(Hash.GET_DLC_WEAPON_DATA, i, intPtr))
					{
						DlcWeaponData weaponData = Marshal.PtrToStructure<DlcWeaponData>(intPtr);
						List<DlcComponentData> dlcWeaponComponents = GetDlcWeaponComponents(i);
						DlcWeaponDataWithComponents dlcWeaponDataWithComponents = new DlcWeaponDataWithComponents(weaponData, dlcWeaponComponents);
						uint weaponTypeGroup = Function.Call<uint>(Hash.GET_WEAPONTYPE_GROUP, weaponData.weaponHash);
						CategorizeWeapon(dlcWeaponDataWithComponents, weaponTypeGroup);
						AddedWeapons.Add(dlcWeaponDataWithComponents);
					}
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
		}

		public List<DlcComponentData> GetDlcWeaponComponents(int dlcWeaponIndex)
		{
			List<DlcComponentData> list = new List<DlcComponentData>();
			int num = Function.Call<int>(Hash.GET_NUM_DLC_WEAPON_COMPONENTS, dlcWeaponIndex);
			for (int i = 0; i < num; i++)
			{
				IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(DlcComponentData)));
				try
				{
					if (Function.Call<bool>(Hash.GET_DLC_WEAPON_COMPONENT_DATA, dlcWeaponIndex, i, intPtr))
					{
						DlcComponentData item = Marshal.PtrToStructure<DlcComponentData>(intPtr);
						list.Add(item);
					}
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
			return list;
		}

		public void CategorizeWeapon(DlcWeaponDataWithComponents weaponDataWithComponents, uint weaponTypeGroup)
		{
			if (!weaponCategories.ContainsKey(weaponTypeGroup))
			{
				weaponCategories.Add(weaponTypeGroup, new List<DlcWeaponDataWithComponents>());
			}
			weaponCategories[weaponTypeGroup].Add(weaponDataWithComponents);
		}
	}
}
