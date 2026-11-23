using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using GTA;
using GTA.Native;

namespace AddonWeapons2.Weapons
{
	public static class WeaponInfo
	{
		public enum AddonWeaponGroup : uint
		{
			Digiscanner = 3539449195u,
			FireExtinguisher = 4257178988u,
			HackingDevice = 1175761940u,
			Heavy = 2725924767u,
			Melee = 3566412244u,
			MetalDetector = 3759491383u,
			MG = 1159398588u,
			NightVision = 3493187224u,
			Parachute = 431593103u,
			PetrolCan = 1595662460u,
			Pistol = 416676503u,
			Rifle = 970310034u,
			Shotgun = 860033945u,
			SMG = 3337201093u,
			Sniper = 3082541095u,
			StunGun = 690389602u,
			Thrown = 1548507267u,
			Tranquilizer = 75159441u,
			RubberGun = 88899580u
		}

		public enum DictionaryType
		{
			Empty = -1,
			Components,
			InstallComponents,
			Tints,
			Ammo,
			InstallTints
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct DlcWeaponData
		{
			public int EmptyCheck;

			public int Padding1;

			public uint WeaponHash;

			public int Padding2;

			public int Unknown1;

			public int Padding3;

			public int WeaponCost;

			public int Padding4;

			public int AmmoCost;

			public int Padding5;

			public int AmmoType;

			public int Padding6;

			public int DefaultClipSize;

			public int Padding7;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public byte[] NameLabel;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public byte[] DescLabel;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public byte[] Desc2Label;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public byte[] UpperCaseNameLabel;

			public string GetNameLabel()
			{
				return GetString(NameLabel);
			}

			public string GetDescLabel()
			{
				return GetString(DescLabel);
			}

			public string GetDesc2Label()
			{
				return GetString(Desc2Label);
			}

			public string GetUpperCaseNameLabel()
			{
				return GetString(UpperCaseNameLabel);
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
			public int AttachBone;

			public int Padding1;

			public int IsActiveByDefault;

			public int Padding2;

			public int Unknown1;

			public int Padding3;

			public uint ComponentHash;

			public int Padding4;

			public int Unknown2;

			public int Padding5;

			public int ComponentCost;

			public int Padding6;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public byte[] NameLabel;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public byte[] DescLabel;

			public string GetNameLabel()
			{
				return GetString(NameLabel);
			}

			public string GetDescLabel()
			{
				return GetString(DescLabel);
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

		public const int MaxDlcWeapons = 69;

		public static uint PreviewComponent = 0u;

		public static readonly List<int> StandardPrices = new List<int> { 0, 100, 200, 400, 600, 800, 1000, 1500 };

		public static readonly List<int> Mk2Prices = new List<int>
		{
			20000, 20000, 30000, 30000, 30000, 30000, 30000, 35000, 35000, 40000,
			40000, 40000, 40000, 75000, 60000, 60000, 60000, 50000, 50000, 50000,
			50000, 45000, 45000, 100000, 100000, 80000, 80000, 75000, 75000, 75000,
			90000, 90000
		};

		public static List<int> TintPrices = new List<int>();

		public static Dictionary<uint, List<DlcWeaponDataWithComponents>> WeaponCategories { get; } = new Dictionary<uint, List<DlcWeaponDataWithComponents>>();


		public static Dictionary<uint, string> GroupNames { get; } = new Dictionary<uint, string>();


		public static Dictionary<uint, Dictionary<uint, List<uint>>> PurchasedComponents { get; set; } = new Dictionary<uint, Dictionary<uint, List<uint>>>();


		public static Dictionary<uint, Dictionary<uint, List<int>>> PurchasedTints { get; set; } = new Dictionary<uint, Dictionary<uint, List<int>>>();


		public static Dictionary<uint, Dictionary<uint, List<uint>>> InstalledComponents { get; set; } = new Dictionary<uint, Dictionary<uint, List<uint>>>();


		public static Dictionary<uint, Dictionary<uint, List<int>>> InstalledAmmo { get; set; } = new Dictionary<uint, Dictionary<uint, List<int>>>();


		public static Dictionary<uint, Dictionary<uint, List<int>>> InstalledTints { get; set; } = new Dictionary<uint, Dictionary<uint, List<int>>>();


		public static List<uint> NoAmmoWeapons { get; } = new List<uint>();


		public static List<uint> DisabledComponentsWeapons { get; } = new List<uint>();


		public static uint CurrentWeaponHash { get; set; } = 0u;


		public static void LoadDlcWeaponModels()
		{
			int num = Function.Call<int>(Hash.GET_NUM_DLC_WEAPONS);
			HashSet<uint> hashSet = new HashSet<uint>();
			for (int i = 0; i < num; i++)
			{
				IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(DlcWeaponData)));
				try
				{
					if (!Function.Call<bool>(Hash.GET_DLC_WEAPON_DATA, i, intPtr))
					{
						continue;
					}
					DlcWeaponData weaponData = Marshal.PtrToStructure<DlcWeaponData>(intPtr);
					if (!hashSet.Contains(weaponData.WeaponHash))
					{
						int hash = Function.Call<int>(Hash.GET_WEAPONTYPE_MODEL, weaponData.WeaponHash);
						Model model = new Model(hash);
						if (model.IsValid && model.IsInCdImage && !model.IsLoaded)
						{
							model.Request();
						}
						hashSet.Add(weaponData.WeaponHash);
						List<DlcComponentData> dlcWeaponComponents = GetDlcWeaponComponents(i);
						DlcWeaponDataWithComponents weaponDataWithComponents = new DlcWeaponDataWithComponents(weaponData, dlcWeaponComponents);
						uint weaponTypeGroup = Function.Call<uint>(Hash.GET_WEAPONTYPE_GROUP, weaponData.WeaponHash);
						CategoriesManager.CategorizeWeapon(weaponDataWithComponents, weaponTypeGroup);
					}
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
		}

		public static bool AreComponentsAvailable(uint weaponHash)
		{
			uint num = Function.Call<uint>(Hash.GET_WEAPONTYPE_GROUP, weaponHash);
			if (num != 3566412244u && num != 1548507267)
			{
				return !DisabledComponentsWeapons.Contains(weaponHash);
			}
			return false;
		}

		private static List<DlcComponentData> GetDlcWeaponComponents(int dlcWeaponIndex)
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
	}
}
