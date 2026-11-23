using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA.Math;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	internal sealed class ExpandedGarageInventory
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<char, bool> _0024I3_002D0;

			public static Func<Quaternion[], bool> _0024I3_002D1;

			public static Func<VehicleData, int> _0024I8_002D0;

			public static Func<string, string> _0024I11_002D0;

			public static Func<VehicleData, int> _0024I11_002D1;

			public static Func<IGrouping<int, VehicleData>, bool> _0024I11_002D2;

			public static Func<IGrouping<int, VehicleData>, VehicleData> _0024I11_002D3;

			public static Func<VehicleData, int> _0024I11_002D4;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__3_002D0(char ch)
			{
				if ((ch < 'a' || ch > 'z') && (ch < '0' || ch > '9') && ch != '-')
				{
					return ch != '_';
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__3_002D1(Quaternion[] floor)
			{
				if (floor != null)
				{
					return floor.Length == 0;
				}
				return true;
			}

			[SpecialName]
			internal int _Lambda_0024__8_002D0(VehicleData record)
			{
				return record.Index;
			}

			[SpecialName]
			internal string _Lambda_0024__11_002D0(string filePath)
			{
				return filePath;
			}

			[SpecialName]
			internal int _Lambda_0024__11_002D1(VehicleData record)
			{
				return record.Index;
			}

			[SpecialName]
			internal bool _Lambda_0024__11_002D2(IGrouping<int, VehicleData> group)
			{
				return group.Count() == 1;
			}

			[SpecialName]
			internal VehicleData _Lambda_0024__11_002D3(IGrouping<int, VehicleData> group)
			{
				return group.First();
			}

			[SpecialName]
			internal int _Lambda_0024__11_002D4(VehicleData record)
			{
				return record.Index;
			}
		}

		private readonly int id;

		private readonly string key;

		private readonly int[] starts;

		internal ExpandedGarageInventory(int storageId, string propertyKey, Quaternion[][] slots)
		{
			if (storageId <= 0)
			{
				throw new ArgumentOutOfRangeException("storageId");
			}
			if (string.IsNullOrEmpty(propertyKey) || propertyKey.Any((_Closure_0024__._0024I3_002D0 != null) ? _Closure_0024__._0024I3_002D0 : (_Closure_0024__._0024I3_002D0 = _Closure_0024__._0024I._Lambda_0024__3_002D0)))
			{
				throw new ArgumentException("Invalid garage key.", "propertyKey");
			}
			if (slots == null || slots.Length == 0 || slots.Any((_Closure_0024__._0024I3_002D1 != null) ? _Closure_0024__._0024I3_002D1 : (_Closure_0024__._0024I3_002D1 = _Closure_0024__._0024I._Lambda_0024__3_002D1)))
			{
				throw new ArgumentException("A garage needs at least one valid floor.", "slots");
			}
			id = storageId;
			key = propertyKey;
			checked
			{
				starts = new int[slots.Length + 1];
				int num = slots.Length - 1;
				for (int i = 0; i <= num; i++)
				{
					starts[i + 1] = starts[i] + slots[i].Length;
				}
			}
		}

		internal int FloorOf(int slot)
		{
			checked
			{
				if (slot < 0 || slot >= starts[starts.Length - 1])
				{
					return -1;
				}
				int num = starts.Length - 2;
				for (int i = 0; i <= num; i++)
				{
					if (slot < starts[i + 1])
					{
						return i;
					}
				}
				return -1;
			}
		}

		internal int BayOf(int slot)
		{
			int num = FloorOf(slot);
			if (num < 0)
			{
				return -1;
			}
			return checked(slot - starts[num]);
		}

		internal int SelectSlot(IEnumerable<VehicleData> records, int existingUid, int modelHash)
		{
			if (existingUid == 0)
			{
				return FreeSlot(records, 0, starts[checked(starts.Length - 1)]);
			}
			foreach (VehicleData record in records)
			{
				if (record.UniqueID == existingUid && record.Hash == modelHash)
				{
					return record.Index;
				}
			}
			return -1;
		}

		internal int SelectSlotOnFloor(IEnumerable<VehicleData> records, int existingUid, int modelHash, int floor)
		{
			checked
			{
				if (floor < 0 || floor >= starts.Length - 1)
				{
					return -1;
				}
				if (existingUid != 0)
				{
					int num = SelectSlot(records, existingUid, modelHash);
					if (num < 0)
					{
						return -1;
					}
					if (FloorOf(num) == floor)
					{
						return num;
					}
				}
				return FreeSlot(records, starts[floor], starts[floor + 1]);
			}
		}

		private static int FreeSlot(IEnumerable<VehicleData> records, int first, int limit)
		{
			HashSet<int> hashSet = new HashSet<int>(records.Select((_Closure_0024__._0024I8_002D0 != null) ? _Closure_0024__._0024I8_002D0 : (_Closure_0024__._0024I8_002D0 = _Closure_0024__._0024I._Lambda_0024__8_002D0)));
			checked
			{
				int num = limit - 1;
				for (int i = first; i <= num; i++)
				{
					if (!hashSet.Contains(i))
					{
						return i;
					}
				}
				return -1;
			}
		}

		internal string RecordPath(string root, eOwner owner, int uid)
		{
			return Path.Combine(OwnerPath(root, owner), uid.ToString(CultureInfo.InvariantCulture) + ".xml");
		}

		internal string OwnerPath(string root, eOwner owner)
		{
			if (owner < eOwner.Michael || owner > eOwner.Trevor)
			{
				throw new ArgumentOutOfRangeException("owner");
			}
			string path = key;
			int num = (int)owner;
			return Path.Combine(root, "expansion", path, num.ToString(CultureInfo.InvariantCulture));
		}

		internal List<VehicleData> ReadRecords(string root, eOwner owner, ref bool complete)
		{
			List<VehicleData> list = new List<VehicleData>();
			complete = false;
			if (owner < eOwner.Michael || owner > eOwner.Trevor || string.IsNullOrWhiteSpace(root))
			{
				return list;
			}
			string path = OwnerPath(root, owner);
			if (File.Exists(path))
			{
				return list;
			}
			complete = true;
			try
			{
				foreach (string item in Directory.GetFiles(path, "*.xml").OrderBy((_Closure_0024__._0024I11_002D0 != null) ? _Closure_0024__._0024I11_002D0 : (_Closure_0024__._0024I11_002D0 = _Closure_0024__._0024I._Lambda_0024__11_002D0), StringComparer.Ordinal))
				{
					VehicleData value = default(VehicleData);
					if (!new VehicleData(item).TryRead(ref value) || value.ApartmentID != id || value.Owner != owner || value.UniqueID == 0 || FloorOf(value.Index) < 0 || !string.Equals(Path.GetFileNameWithoutExtension(item), value.UniqueID.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal))
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
				Logger.Log("Expansion garage inventory: " + ex4.Message);
				ProjectData.ClearProjectError();
			}
			List<VehicleData> list2 = list.GroupBy((_Closure_0024__._0024I11_002D1 != null) ? _Closure_0024__._0024I11_002D1 : (_Closure_0024__._0024I11_002D1 = _Closure_0024__._0024I._Lambda_0024__11_002D1)).Where((_Closure_0024__._0024I11_002D2 != null) ? _Closure_0024__._0024I11_002D2 : (_Closure_0024__._0024I11_002D2 = _Closure_0024__._0024I._Lambda_0024__11_002D2)).Select((_Closure_0024__._0024I11_002D3 != null) ? _Closure_0024__._0024I11_002D3 : (_Closure_0024__._0024I11_002D3 = _Closure_0024__._0024I._Lambda_0024__11_002D3))
				.OrderBy((_Closure_0024__._0024I11_002D4 != null) ? _Closure_0024__._0024I11_002D4 : (_Closure_0024__._0024I11_002D4 = _Closure_0024__._0024I._Lambda_0024__11_002D4))
				.ToList();
			if (list2.Count != list.Count)
			{
				complete = false;
			}
			return list2;
		}
	}
}
