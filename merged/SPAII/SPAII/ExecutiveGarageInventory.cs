using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	internal sealed class ExecutiveGarageInventory
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<VehicleClass, int> _0024I13_002D1;

			public static Func<string, string> _0024I15_002D0;

			public static Func<VehicleData, int> _0024I15_002D1;

			public static Func<IGrouping<int, VehicleData>, bool> _0024I15_002D2;

			public static Func<IGrouping<int, VehicleData>, VehicleData> _0024I15_002D3;

			public static Func<VehicleData, int> _0024I15_002D4;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal int _Lambda_0024__13_002D1(VehicleClass vehicle)
			{
				return vehicle.Index;
			}

			[SpecialName]
			internal string _Lambda_0024__15_002D0(string path)
			{
				return path;
			}

			[SpecialName]
			internal int _Lambda_0024__15_002D1(VehicleData record)
			{
				return record.Index;
			}

			[SpecialName]
			internal bool _Lambda_0024__15_002D2(IGrouping<int, VehicleData> group)
			{
				return group.Count() == 1;
			}

			[SpecialName]
			internal VehicleData _Lambda_0024__15_002D3(IGrouping<int, VehicleData> group)
			{
				return group.First();
			}

			[SpecialName]
			internal int _Lambda_0024__15_002D4(VehicleData record)
			{
				return record.Index;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__13_002D0
		{
			public int _0024VB_0024Local_sourceUid;

			public int _0024VB_0024Local_modelHash;

			[SpecialName]
			internal bool _Lambda_0024__0(VehicleClass vehicle)
			{
				if (vehicle.UniqueID == _0024VB_0024Local_sourceUid)
				{
					return vehicle.Hash == _0024VB_0024Local_modelHash;
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__16_002D0
		{
			public eOwner _0024VB_0024Local_owner;

			[SpecialName]
			internal VehicleClass _Lambda_0024__0(VehicleData record)
			{
				return new VehicleClass(record, _0024VB_0024Local_owner);
			}
		}

		internal const int PropertyId = 9310;

		internal const string OwnerKey = "SPA_EXECUTIVE_60";

		internal const string SettingKey = "EXECUTIVE_GARAGE";

		internal const string FolderName = "executive60";

		internal const string DisplayName = "Executive Garage";

		internal const string Description = "An executive garage with three floors and 60 vehicle spaces.";

		internal const int FloorCount = 3;

		internal const int SlotsPerFloor = 20;

		internal const int Capacity = 60;

		private ExecutiveGarageInventory()
		{
		}

		internal static int FloorOf(int index)
		{
			if (index < 0 || index >= 60)
			{
				return -1;
			}
			return index / 20;
		}

		internal static int BayOf(int index)
		{
			if (FloorOf(index) < 0)
			{
				return -1;
			}
			return index % 20;
		}

		internal static int FindSlot(IEnumerable<int> occupied, int existing, int preferredFloor)
		{
			if (preferredFloor < -1 || preferredFloor >= 3)
			{
				return -1;
			}
			if (FloorOf(existing) >= 0)
			{
				return existing;
			}
			HashSet<int> hashSet = new HashSet<int>(occupied);
			checked
			{
				int num = ((preferredFloor >= 0) ? (preferredFloor * 20) : 0);
				int num2 = ((preferredFloor < 0) ? 60 : 20);
				int num3 = num;
				int num4 = num + num2 - 1;
				for (int i = num3; i <= num4; i++)
				{
					if (!hashSet.Contains(i))
					{
						return i;
					}
				}
				return -1;
			}
		}

		internal static int SelectVehicleSlot(List<VehicleClass> vehicles, int sourceId, int sourceUid, int modelHash, int preferredFloor)
		{
			_Closure_0024__13_002D0 closure_0024__13_002D = new _Closure_0024__13_002D0();
			closure_0024__13_002D._0024VB_0024Local_sourceUid = sourceUid;
			closure_0024__13_002D._0024VB_0024Local_modelHash = modelHash;
			int existing = -1;
			if (sourceId == 9310)
			{
				VehicleClass vehicleClass = vehicles.FirstOrDefault(closure_0024__13_002D._Lambda_0024__0);
				if (vehicleClass == null)
				{
					return -1;
				}
				existing = vehicleClass.Index;
			}
			return FindSlot(vehicles.Select((_Closure_0024__._0024I13_002D1 != null) ? _Closure_0024__._0024I13_002D1 : (_Closure_0024__._0024I13_002D1 = _Closure_0024__._0024I._Lambda_0024__13_002D1)), existing, preferredFloor);
		}

		internal static bool IsProperty(ApartmentClass apartment)
		{
			if (apartment != null && apartment.ID == 9310 && string.Equals(apartment.Name, "SPA_EXECUTIVE_60", StringComparison.Ordinal))
			{
				return string.Equals(apartment.GarageFilePath, "executive60", StringComparison.Ordinal);
			}
			return false;
		}

		internal static List<VehicleData> ReadInventory(string directoryPath, eOwner owner, ref bool complete)
		{
			List<VehicleData> list = new List<VehicleData>();
			complete = false;
			if (string.IsNullOrWhiteSpace(directoryPath) || owner < eOwner.Michael || owner > eOwner.Trevor)
			{
				return list;
			}
			if (File.Exists(directoryPath))
			{
				return list;
			}
			try
			{
				complete = true;
				foreach (string item in Directory.GetFiles(directoryPath, "*.xml").OrderBy((_Closure_0024__._0024I15_002D0 != null) ? _Closure_0024__._0024I15_002D0 : (_Closure_0024__._0024I15_002D0 = _Closure_0024__._0024I._Lambda_0024__15_002D0), StringComparer.Ordinal))
				{
					VehicleData value = default(VehicleData);
					if (!new VehicleData(item).TryRead(ref value) || value.ApartmentID != 9310 || value.Owner != owner || value.UniqueID == 0 || FloorOf(value.Index) < 0 || !string.Equals(Path.GetFileNameWithoutExtension(item), value.UniqueID.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal))
					{
						complete = false;
					}
					else
					{
						list.Add(value);
					}
				}
			}
			catch (DirectoryNotFoundException ex)
			{
				ProjectData.SetProjectError(ex);
				DirectoryNotFoundException ex2 = ex;
				complete = list.Count == 0;
				ProjectData.ClearProjectError();
			}
			catch (Exception ex3)
			{
				ProjectData.SetProjectError(ex3);
				Exception ex4 = ex3;
				complete = false;
				Logger.Log("Executive garage inventory could not be read: " + ex4.Message);
				ProjectData.ClearProjectError();
			}
			List<VehicleData> list2 = list.GroupBy((_Closure_0024__._0024I15_002D1 != null) ? _Closure_0024__._0024I15_002D1 : (_Closure_0024__._0024I15_002D1 = _Closure_0024__._0024I._Lambda_0024__15_002D1)).Where((_Closure_0024__._0024I15_002D2 != null) ? _Closure_0024__._0024I15_002D2 : (_Closure_0024__._0024I15_002D2 = _Closure_0024__._0024I._Lambda_0024__15_002D2)).Select((_Closure_0024__._0024I15_002D3 != null) ? _Closure_0024__._0024I15_002D3 : (_Closure_0024__._0024I15_002D3 = _Closure_0024__._0024I._Lambda_0024__15_002D3))
				.OrderBy((_Closure_0024__._0024I15_002D4 != null) ? _Closure_0024__._0024I15_002D4 : (_Closure_0024__._0024I15_002D4 = _Closure_0024__._0024I._Lambda_0024__15_002D4))
				.ToList();
			if (list2.Count != list.Count)
			{
				complete = false;
			}
			return list2;
		}

		internal static List<VehicleClass> ReadVehicles(ApartmentClass apartment, ref bool complete)
		{
			_Closure_0024__16_002D0 closure_0024__16_002D = new _Closure_0024__16_002D0();
			complete = false;
			if (!IsProperty(apartment))
			{
				return new List<VehicleClass>();
			}
			closure_0024__16_002D._0024VB_0024Local_owner = apartment.Owner();
			return ReadInventory(InventoryDirectory(apartment, closure_0024__16_002D._0024VB_0024Local_owner), closure_0024__16_002D._0024VB_0024Local_owner, ref complete).Select(closure_0024__16_002D._Lambda_0024__0).ToList();
		}

		internal static string InventoryDirectory(ApartmentClass apartment, eOwner owner)
		{
			if (!IsProperty(apartment) || !PropertyProfiles.IsCharacter(owner) || apartment.OwnerFor(owner) != owner)
			{
				return null;
			}
			string text = apartment.StorageFolder(owner);
			if (string.IsNullOrWhiteSpace(text))
			{
				return null;
			}
			return Path.Combine(Helper.grgXmlPath, text);
		}
	}
}
