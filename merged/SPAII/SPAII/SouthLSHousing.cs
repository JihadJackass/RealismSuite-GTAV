using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class SouthLSHousing
	{
		public class Definition
		{
			public readonly int ID;

			public readonly string Key;

			public readonly string LabelKey;

			public readonly string Title;

			public readonly string DescriptionKey;

			public readonly string Description;

			public readonly string GarageFolder;

			public readonly int Price;

			public readonly eApartmentType InteriorType;

			public Definition(int id, string key, string title, string description, int price, eApartmentType interiorType)
			{
				ID = id;
				Key = key;
				LabelKey = "SPA_" + key;
				Title = title;
				DescriptionKey = LabelKey + "_DESC";
				Description = description;
				Price = price;
				InteriorType = interiorType;
				GarageFolder = "south_ls_" + key.Substring(6).ToLowerInvariant() + "_" + id.ToString(CultureInfo.InvariantCulture);
			}
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<ApartmentClass, bool> _0024I5_002D0;

			public static Func<BuildingClass, bool> _0024I15_002D0;

			public static Func<BuildingClass, IEnumerable<ApartmentClass>> _0024I15_002D1;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__5_002D0(ApartmentClass a)
			{
				if (a != null)
				{
					return HasGarage(a.ID);
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__15_002D0(BuildingClass b)
			{
				return b != null;
			}

			[SpecialName]
			internal IEnumerable<ApartmentClass> _Lambda_0024__15_002D1(BuildingClass b)
			{
				return b.Apartments ?? new List<ApartmentClass>();
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__12_002D0
		{
			public int _0024VB_0024Local_id;

			[SpecialName]
			internal bool _Lambda_0024__0(Definition item)
			{
				return item.ID == _0024VB_0024Local_id;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__15_002D0
		{
			public Definition _0024VB_0024Local_definition;

			public _Closure_0024__15_002D0(_Closure_0024__15_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_definition = arg0._0024VB_0024Local_definition;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__2(ApartmentClass a)
			{
				if (a != null)
				{
					if (a.ID != _0024VB_0024Local_definition.ID && !string.Equals(a.Name, _0024VB_0024Local_definition.LabelKey, StringComparison.Ordinal))
					{
						return string.Equals(a.GarageFilePath, _0024VB_0024Local_definition.GarageFolder, StringComparison.OrdinalIgnoreCase);
					}
					return true;
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__8_002D0
		{
			public int _0024VB_0024Local_id;

			public _Closure_0024__8_002D0(_Closure_0024__8_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_id = arg0._0024VB_0024Local_id;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(ApartmentClass a)
			{
				if (a != null)
				{
					return a.ID == _0024VB_0024Local_id;
				}
				return false;
			}
		}

		private static readonly Definition[] homes = new Definition[3]
		{
			new Definition(9301, "SOUTH_DAVIS", "Ryders Home", "A modest home in Davis with a private two-car garage.", 25000, eApartmentType.LowEnd),
			new Definition(9302, "SOUTH_STRAWBERRY", "Strawberry Apartment", "An affordable Strawberry apartment without a garage.", 35000, eApartmentType.LowEnd),
			new Definition(9303, "SOUTH_RANCHO", "Rancho Small House", "A small Rancho home with a private two-car garage.", 45000, eApartmentType.LowEnd)
		};

		private static readonly Definition retiredChamberlain = new Definition(9304, "SOUTH_CHAMBERLAIN", "Vehicles from removed property", "Recover vehicles saved before their former property was removed.", 60000, eApartmentType.MediumEnd);

		public static bool HasGarage(int id)
		{
			if (id != 9301)
			{
				return id == 9303;
			}
			return true;
		}

		public static bool HasGarage(BuildingClass building)
		{
			if (building != null && building.Apartments != null)
			{
				return building.Apartments.Any((_Closure_0024__._0024I5_002D0 != null) ? _Closure_0024__._0024I5_002D0 : (_Closure_0024__._0024I5_002D0 = _Closure_0024__._0024I._Lambda_0024__5_002D0));
			}
			return false;
		}

		public static string ArchiveGarageFolder(int id)
		{
			switch (id)
			{
			case 9302:
				return "south_ls_strawberry_9302";
			case 9304:
				return retiredChamberlain.GarageFolder;
			default:
				return null;
			}
		}

		public static ApartmentClass RecoveryApartmentForId(int id)
		{
			object obj;
			switch (id)
			{
			default:
				obj = null;
				break;
			case 9302:
				obj = DefinitionForID(id);
				break;
			case 9304:
				obj = retiredChamberlain;
				break;
			}
			Definition definition = (Definition)obj;
			if (definition == null)
			{
				return null;
			}
			return new ApartmentClass
			{
				ID = id,
				Name = definition.LabelKey,
				Description = definition.DescriptionKey,
				GarageFilePath = definition.GarageFolder,
				Price = 0,
				ApartmentType = eApartmentType.None,
				BuildingOverride = new BuildingClass
				{
					Name = definition.Title,
					GarageType = eGarageType.NoGarage
				}
			};
		}

		public static ApartmentClass[] RecoveryApartments()
		{
			List<ApartmentClass> list = new List<ApartmentClass>();
			int[] array = new int[2] { 9302, 9304 };
			_Closure_0024__8_002D0 closure_0024__8_002D = default(_Closure_0024__8_002D0);
			for (int i = 0; i < array.Length; i = checked(i + 1))
			{
				closure_0024__8_002D = new _Closure_0024__8_002D0(closure_0024__8_002D);
				closure_0024__8_002D._0024VB_0024Local_id = array[i];
				if (Helper.apartments != null && Helper.apartments.Any(closure_0024__8_002D._Lambda_0024__0))
				{
					continue;
				}
				ApartmentClass apartmentClass = RecoveryApartmentForId(closure_0024__8_002D._0024VB_0024Local_id);
				string path = Path.Combine(Helper.grgXmlPath, apartmentClass.GarageFilePath);
				try
				{
					if (Directory.Exists(path) && apartmentClass.Vehicles().Count > 0)
					{
						list.Add(apartmentClass);
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Note("South LS archived inventory: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
			return list.ToArray();
		}

		public static Definition[] Definitions()
		{
			return (Definition[])homes.Clone();
		}

		public static bool TryLabel(string key, ref string value)
		{
			value = null;
			if (key == null)
			{
				return false;
			}
			foreach (Definition item in homes.Concat(new Definition[1] { retiredChamberlain }))
			{
				if (string.Equals(key, item.LabelKey, StringComparison.Ordinal))
				{
					value = item.Title;
					return true;
				}
				if (string.Equals(key, item.DescriptionKey, StringComparison.Ordinal))
				{
					value = item.Description;
					return true;
				}
			}
			return false;
		}

		public static string PropertyText(string key)
		{
			string value = null;
			if (TryLabel(key, ref value))
			{
				return value;
			}
			return string.IsNullOrEmpty(key) ? "" : Game.GetLocalizedString(key);
		}

		public static Definition DefinitionForID(int id)
		{
			_Closure_0024__12_002D0 closure_0024__12_002D = new _Closure_0024__12_002D0();
			closure_0024__12_002D._0024VB_0024Local_id = id;
			return homes.FirstOrDefault(closure_0024__12_002D._Lambda_0024__0);
		}

		public static bool IsPropertyID(int id)
		{
			return DefinitionForID(id) != null;
		}

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

		public static int Append(IList<BuildingClass> target, ScriptSettings settings)
		{
			if (target == null || settings == null)
			{
				return 0;
			}
			int num = 0;
			Definition[] array = homes;
			checked
			{
				_Closure_0024__15_002D0 closure_0024__15_002D = default(_Closure_0024__15_002D0);
				for (int i = 0; i < array.Length; i++)
				{
					closure_0024__15_002D = new _Closure_0024__15_002D0(closure_0024__15_002D);
					closure_0024__15_002D._0024VB_0024Local_definition = array[i];
					try
					{
						if (!settings.GetValue("APARTMENT", closure_0024__15_002D._0024VB_0024Local_definition.Key, true))
						{
							continue;
						}
						ApartmentClass[] array2 = target.Where((_Closure_0024__._0024I15_002D0 != null) ? _Closure_0024__._0024I15_002D0 : (_Closure_0024__._0024I15_002D0 = _Closure_0024__._0024I._Lambda_0024__15_002D0)).SelectMany((_Closure_0024__._0024I15_002D1 != null) ? _Closure_0024__._0024I15_002D1 : (_Closure_0024__._0024I15_002D1 = _Closure_0024__._0024I._Lambda_0024__15_002D1)).Where(closure_0024__15_002D._Lambda_0024__2)
							.ToArray();
						if (array2.Length > 0)
						{
							if (array2.Length != 1 || array2[0].ID != closure_0024__15_002D._0024VB_0024Local_definition.ID || Operators.CompareString(array2[0].Name, closure_0024__15_002D._0024VB_0024Local_definition.LabelKey, false) != 0 || !string.Equals(array2[0].GarageFilePath, closure_0024__15_002D._0024VB_0024Local_definition.GarageFolder, StringComparison.Ordinal))
							{
								Note("South LS housing: conflicting property identity; skipped " + closure_0024__15_002D._0024VB_0024Local_definition.Key);
							}
							continue;
						}
						ApartmentClass apartmentClass = new ApartmentClass
						{
							ID = closure_0024__15_002D._0024VB_0024Local_definition.ID,
							Name = closure_0024__15_002D._0024VB_0024Local_definition.LabelKey,
							Description = closure_0024__15_002D._0024VB_0024Local_definition.DescriptionKey,
							Price = closure_0024__15_002D._0024VB_0024Local_definition.Price,
							GarageFilePath = closure_0024__15_002D._0024VB_0024Local_definition.GarageFolder
						};
						if (closure_0024__15_002D._0024VB_0024Local_definition.InteriorType == eApartmentType.LowEnd)
						{
							Helper.SetAsLowEndApartment(apartmentClass);
						}
						else
						{
							Helper.SetAsMediumApartment(apartmentClass);
						}
						BuildingClass buildingClass = new BuildingClass
						{
							Name = closure_0024__15_002D._0024VB_0024Local_definition.Title,
							BuildingType = eBuildingType.Apartment,
							GarageType = (HasGarage(closure_0024__15_002D._0024VB_0024Local_definition.ID) ? eGarageType.TwoCarGarage : eGarageType.NoGarage),
							FrontDoor = eFrontDoor.NoDoor,
							GarageDoor = eFrontDoor.NoDoor,
							GarageWaypoint = default(Quaternion),
							Apartments = new List<ApartmentClass> { apartmentClass }
						};
						if (!SouthLSHousingExterior.Initialize(buildingClass, closure_0024__15_002D._0024VB_0024Local_definition.ID))
						{
							Note("South LS housing: no sourced exterior for " + closure_0024__15_002D._0024VB_0024Local_definition.Key);
							continue;
						}
						target.Add(buildingClass);
						num++;
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						Note("South LS housing: skipped " + closure_0024__15_002D._0024VB_0024Local_definition.Key + "; " + ex2.Message);
						ProjectData.ClearProjectError();
					}
				}
				return num;
			}
		}
	}
}
