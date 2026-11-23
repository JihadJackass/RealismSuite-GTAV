using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Serialization;
using GTA;
using GTA.Native;
using GTA.UI;
using Metadata;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class PropertySale
	{
		[CompilerGenerated]
		internal sealed class _Closure_0024__22_002D0
		{
			public ApartmentClass _0024VB_0024Local_apartment;
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__22_002D1
		{
			public Ped _0024VB_0024Local_actor;

			public IntPtr _0024VB_0024Local_address;

			public int _0024VB_0024Local_model;

			public eOwner _0024VB_0024Local_owner;

			public string _0024VB_0024Local_key;

			public string _0024VB_0024Local_folder;

			public int _0024VB_0024Local_price;

			public _Closure_0024__22_002D0 _0024VB_0024NonLocal__0024VB_0024Closure_2;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				return ContextValid(_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_apartment, _0024VB_0024Local_actor, _0024VB_0024Local_address, _0024VB_0024Local_model, _0024VB_0024Local_owner, _0024VB_0024Local_key, _0024VB_0024Local_folder, _0024VB_0024Local_price);
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

			[SpecialName]
			internal bool _Lambda_0024__3()
			{
				return ContextValid(_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_apartment, _0024VB_0024Local_actor, _0024VB_0024Local_address, _0024VB_0024Local_model, _0024VB_0024Local_owner, _0024VB_0024Local_key, _0024VB_0024Local_folder, _0024VB_0024Local_price);
			}

			[SpecialName]
			internal int? _Lambda_0024__4()
			{
				return ReadCapturedBalance(_0024VB_0024Local_owner);
			}

			[SpecialName]
			internal bool _Lambda_0024__5(int amount)
			{
				return SetCapturedBalance(_0024VB_0024Local_owner, amount);
			}
		}

		public const string MansionConfigPath = "scripts\\SPA II\\Mansions.ini";

		private const int MaxInventoryFiles = 8192;

		private const long MaxVehicleXmlBytes = 4194304L;

		public static int FairSalePrice(int price)
		{
			if (price <= 0)
			{
				return 0;
			}
			checked
			{
				return (int)unchecked(checked(unchecked((long)price) * 995L) / 1000);
			}
		}

		public static bool CanSellInventory(int savedFileCount, int outsideCount, bool readable)
		{
			if (readable && savedFileCount == 0)
			{
				return outsideCount == 0;
			}
			return false;
		}

		public static bool IsVehicleSaveEvidence(string fileName)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				return false;
			}
			string fileName2 = Path.GetFileName(fileName);
			return fileName2.EndsWith(".xml", StringComparison.OrdinalIgnoreCase) || fileName2.IndexOf(".xml.", StringComparison.OrdinalIgnoreCase) >= 0;
		}

		public static bool TryCountSavedFiles(string directoryPath, ref int count)
		{
			count = 0;
			if (string.IsNullOrWhiteSpace(directoryPath))
			{
				return false;
			}
			if (File.Exists(directoryPath))
			{
				return false;
			}
			checked
			{
				try
				{
					int num = default(int);
					foreach (string item in Directory.EnumerateFiles(directoryPath))
					{
						num++;
						if (num > 8192)
						{
							return false;
						}
						if (IsVehicleSaveEvidence(item))
						{
							count++;
						}
					}
					return true;
				}
				catch (DirectoryNotFoundException ex)
				{
					ProjectData.SetProjectError(ex);
					DirectoryNotFoundException ex2 = ex;
					bool result = true;
					ProjectData.ClearProjectError();
					return result;
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					bool result = false;
					ProjectData.ClearProjectError();
					return result;
				}
			}
		}

		private static bool TryReadVehicleIdentity(string path, eOwner owner, int apartmentID, ref string identity)
		{
			identity = null;
			try
			{
				FileInfo fileInfo = new FileInfo(path);
				if (fileInfo.Length <= 0 || fileInfo.Length > 4194304)
				{
					return false;
				}
				XmlReaderSettings settings = new XmlReaderSettings
				{
					DtdProcessing = DtdProcessing.Prohibit,
					XmlResolver = null,
					MaxCharactersInDocument = 4194304L
				};
				VehicleData vehicleData;
				using (FileStream input = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					using (XmlReader xmlReader = XmlReader.Create(input, settings))
					{
						object obj = new XmlSerializer(typeof(VehicleData)).Deserialize(xmlReader);
						vehicleData = ((obj != null) ? ((VehicleData)obj) : default(VehicleData));
					}
				}
				if (vehicleData.Hash == 0 || vehicleData.UniqueID == 0 || vehicleData.Index < 0 || vehicleData.Owner != owner || vehicleData.ApartmentID != apartmentID)
				{
					return false;
				}
				string[] obj2 = new string[5]
				{
					vehicleData.UniqueID.ToString(CultureInfo.InvariantCulture),
					":",
					vehicleData.Hash.ToString(CultureInfo.InvariantCulture),
					":",
					null
				};
				int num = (int)owner;
				obj2[4] = num.ToString();
				identity = string.Concat(obj2);
				return true;
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public static bool VerifiedTransferIdentity(string path, eOwner owner, int apartmentID, int uid, int modelHash)
		{
			if (uid == 0 || modelHash == 0)
			{
				return false;
			}
			string identity = null;
			int result;
			if (TryReadVehicleIdentity(path, owner, apartmentID, ref identity))
			{
				string a = identity;
				string[] obj = new string[5]
				{
					uid.ToString(CultureInfo.InvariantCulture),
					":",
					modelHash.ToString(CultureInfo.InvariantCulture),
					":",
					null
				};
				int num = (int)owner;
				obj[4] = num.ToString();
				result = (string.Equals(a, string.Concat(obj), StringComparison.Ordinal) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}

		private static bool SavedEnabled(ScriptSettings settings, string section, string key)
		{
			string value = settings.GetValue<string>(section, key, null);
			if (value == null)
			{
				return true;
			}
			bool result;
			return bool.TryParse(value, out result) && result;
		}

		public static bool OwnedMansionGarage(Mansion home, eOwner owner, ScriptSettings settings)
		{
			if (home == null || settings == null || Mansions.MansionList == null || !Mansions.MansionList.Contains(home) || !home.Enabled || home.Owner != owner || owner < eOwner.Michael || owner > eOwner.Trevor)
			{
				return false;
			}
			string text;
			switch (home.GarageId)
			{
			case 9001:
				text = "MANSION_VINEWOOD";
				break;
			case 9002:
				text = "MANSION_RICHMAN";
				break;
			case 9003:
				text = "MANSION_TONGVA";
				break;
			default:
				return false;
			}
			return string.Equals(home.Key, text, StringComparison.Ordinal) && SavedEnabled(settings, "SETTING", "EnableMansions") && SavedEnabled(settings, text, "Enabled") && Mansions.ReadMansionProfileOwner(settings, text, owner) == owner;
		}

		public static bool AddOwnedMansionDestinations(IDictionary<string, int> destinationFolders, string garageRoot, string settingsPath, eOwner owner)
		{
			if (destinationFolders == null || string.IsNullOrWhiteSpace(garageRoot) || string.IsNullOrWhiteSpace(settingsPath) || owner < eOwner.Michael || owner > eOwner.Trevor)
			{
				return false;
			}
			try
			{
				ScriptSettings settings = ScriptSettings.Load(settingsPath);
				Mansion[] array = Mansions.MansionList.ToArray();
				foreach (Mansion mansion in array)
				{
					if (OwnedMansionGarage(mansion, owner, settings))
					{
						string fullPath = Path.GetFullPath(Path.Combine(garageRoot, mansion.GarageFolder()));
						int value;
						if (destinationFolders.TryGetValue(fullPath, out value) && value != mansion.GarageId)
						{
							return false;
						}
						destinationFolders[fullPath] = mansion.GarageId;
					}
				}
				return true;
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public static bool TryCountUnmovedSaveEvidence(string directoryPath, int sourceApartmentID, eOwner owner, IDictionary<string, int> destinationFolders, ref int count)
		{
			count = 0;
			HashSet<string> unmoved = null;
			if (!TryGetUnmovedSaveEvidence(directoryPath, sourceApartmentID, owner, destinationFolders, ref unmoved))
			{
				return false;
			}
			count = unmoved.Count;
			return true;
		}

		internal static bool TryGetUnmovedSaveEvidence(string directoryPath, int sourceApartmentID, eOwner owner, IDictionary<string, int> destinationFolders, ref HashSet<string> unmoved)
		{
			unmoved = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			if (string.IsNullOrWhiteSpace(directoryPath) || File.Exists(directoryPath) || owner < eOwner.Michael || owner > eOwner.Trevor || destinationFolders == null)
			{
				return false;
			}
			checked
			{
				try
				{
					string fullPath = Path.GetFullPath(directoryPath);
					List<string> list = new List<string>();
					int num = default(int);
					foreach (string item in Directory.EnumerateFiles(fullPath))
					{
						num++;
						if (num > 8192)
						{
							return false;
						}
						if (IsVehicleSaveEvidence(item))
						{
							list.Add(item);
						}
					}
					if (list.Count == 0)
					{
						return true;
					}
					HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
					foreach (KeyValuePair<string, int> destinationFolder in destinationFolders)
					{
						if (destinationFolder.Value == sourceApartmentID || string.IsNullOrWhiteSpace(destinationFolder.Key))
						{
							continue;
						}
						string fullPath2 = Path.GetFullPath(destinationFolder.Key);
						if (string.Equals(fullPath2, fullPath, StringComparison.OrdinalIgnoreCase) || !Directory.Exists(fullPath2))
						{
							continue;
						}
						foreach (string item2 in Directory.EnumerateFiles(fullPath2))
						{
							num++;
							if (num > 8192)
							{
								return false;
							}
							if (item2.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
							{
								string identity = null;
								if (TryReadVehicleIdentity(item2, owner, destinationFolder.Value, ref identity))
								{
									hashSet.Add(identity);
								}
							}
						}
					}
					foreach (string item3 in list)
					{
						string identity2 = null;
						if (item3.EndsWith(".xml", StringComparison.OrdinalIgnoreCase) || !TryReadVehicleIdentity(item3, owner, sourceApartmentID, ref identity2) || !hashSet.Contains(identity2))
						{
							unmoved.Add(item3);
						}
					}
					return true;
				}
				catch (DirectoryNotFoundException ex)
				{
					ProjectData.SetProjectError(ex);
					DirectoryNotFoundException ex2 = ex;
					bool result = !Directory.Exists(directoryPath) && !File.Exists(directoryPath);
					ProjectData.ClearProjectError();
					return result;
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					bool result = false;
					ProjectData.ClearProjectError();
					return result;
				}
			}
		}

		internal static bool TryGetUnmovedGarageEvidence(ApartmentClass apartment, eOwner owner, string directoryPath, ref HashSet<string> unmoved)
		{
			try
			{
				return TryGetUnmovedSaveEvidence(directoryPath, apartment.ID, owner, OwnedDestinationFolders(apartment, owner), ref unmoved);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Garage backup evidence: " + ex2.Message);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		private static Dictionary<string, int> OwnedDestinationFolders(ApartmentClass source, eOwner owner)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			ScriptSettings scriptSettings = ScriptSettings.Load("scripts\\SPA II\\modconfig.ini");
			ApartmentClass[] array = Helper.apartments.ToArray();
			eOwner owner2 = default(eOwner);
			foreach (ApartmentClass apartmentClass in array)
			{
				if (apartmentClass != null && apartmentClass != source && apartmentClass.ID != source.ID && RegisteredApartment(apartmentClass) && !string.IsNullOrWhiteSpace(apartmentClass.StorageFolder(owner)) && apartmentClass.Building().GarageType != 0 && (apartmentClass.HasCharacterProfile() ? PropertyProfiles.ReadOwner(scriptSettings, apartmentClass.Name, owner, ref owner2) : PropertyTransactions.ReadOwnerValue(scriptSettings.GetValue<string>("BUILDING", apartmentClass.Name, null), ref owner2)) && owner2 == owner)
				{
					string fullPath = Path.GetFullPath(Path.Combine(Helper.grgXmlPath, apartmentClass.StorageFolder(owner)));
					if (!dictionary.ContainsKey(fullPath))
					{
						dictionary.Add(fullPath, apartmentClass.ID);
					}
				}
			}
			if (!AddOwnedMansionDestinations(dictionary, Helper.grgXmlPath, "scripts\\SPA II\\Mansions.ini", owner))
			{
				throw new IOException("The owned mansion garage destinations could not be verified.");
			}
			ExpandedGarage.AddOwnedGarageDestinations(dictionary, Helper.grgXmlPath, owner);
			return dictionary;
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

		public static bool RegisteredApartment(ApartmentClass apartment)
		{
			if (apartment == null || !Helper.apartments.Contains(apartment))
			{
				return false;
			}
			BuildingClass buildingClass = apartment.Building();
			return buildingClass != null && Helper.buildings.Contains(buildingClass) && buildingClass.Apartments != null && buildingClass.Apartments.Contains(apartment);
		}

		private static bool InventoryEmpty(ApartmentClass apartment, eOwner owner, ref string failure)
		{
			try
			{
				if (apartment == null || string.IsNullOrWhiteSpace(apartment.StorageFolder(owner)))
				{
					failure = "The property's saved-vehicle location could not be verified.";
					return false;
				}
				int count = default(int);
				bool flag = TryCountUnmovedSaveEvidence(Path.Combine(Helper.grgXmlPath, apartment.StorageFolder(owner)), apartment.ID, owner, OwnedDestinationFolders(apartment, owner), ref count);
				if (!flag)
				{
					failure = "The property's vehicle records could not be read. They have been kept.";
					return false;
				}
				Vehicle[] array = Helper.outVehicleList.ToArray();
				int num = default(int);
				foreach (Vehicle vehicle in array)
				{
					if ((object)vehicle != null && vehicle.Exists() && vehicle.GetInt(Helper.vehIdDecor) == apartment.ID && (!apartment.HasCharacterProfile() || Helper.Owner(vehicle) == owner))
					{
						num = checked(num + 1);
					}
				}
				if (!CanSellInventory(count, num, flag))
				{
					failure = ((count > 0) ? "Vehicle save/recovery files remain. Restore/move the vehicles or review their backups before selling; nothing was deleted." : "Move this property's delivered vehicles to another owned garage before selling; nothing was deleted.");
					return false;
				}
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				failure = "The vehicle inventory could not be verified: " + ex2.Message;
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		private static bool ContextValid(ApartmentClass apartment, Ped actor, IntPtr address, int model, eOwner owner, string key, string folder, int price)
		{
			if (!RegisteredApartment(apartment) || owner < eOwner.Michael || owner > eOwner.Trevor || (object)actor == null || !actor.Exists() || actor.MemoryAddress != address || actor.Model.Hash != model || actor.IsDead || actor.IsFalling || actor.IsRagdoll || (object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.Handle != actor.Handle || Helper.GetPlayer() != owner || apartment.Owner() != owner || Operators.CompareString(apartment.Name, key, false) != 0 || Operators.CompareString(apartment.StorageFolder(owner), folder, false) != 0 || apartment.Price != price || price <= 0 || DestinationStreaming.IsPlayerSafetyHeld(actor) || PartyService.IsPlayerActionActive || MansionFeatures.IsWorkoutActive || PartyPlayerActions.IsKissing)
			{
				return false;
			}
			string failure = null;
			return InventoryEmpty(apartment, owner, ref failure);
		}

		private static int? ReadCapturedBalance(eOwner owner)
		{
			int balance = default(int);
			return (!Helper.TryGetOwnerMoney(owner, ref balance)) ? null : new int?(balance);
		}

		private static bool SetCapturedBalance(eOwner owner, int balance)
		{
			if (balance < 0)
			{
				return false;
			}
			string input;
			switch (owner)
			{
			case eOwner.Michael:
				input = "SP0_TOTAL_CASH";
				break;
			case eOwner.Franklin:
				input = "SP1_TOTAL_CASH";
				break;
			case eOwner.Trevor:
				input = "SP2_TOTAL_CASH";
				break;
			default:
				return false;
			}
			return Function.Call<bool>(Hash.STAT_SET_INT, Game.GenerateHash(input), balance, true);
		}

		public static bool Sell(ApartmentClass apartment)
		{
			_Closure_0024__22_002D0 closure_0024__22_002D = new _Closure_0024__22_002D0();
			closure_0024__22_002D._0024VB_0024Local_apartment = apartment;
			bool flag = default(bool);
			try
			{
				_Closure_0024__22_002D1 closure_0024__22_002D2 = new _Closure_0024__22_002D1();
				closure_0024__22_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2 = closure_0024__22_002D;
				if (!RegisteredApartment(closure_0024__22_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_apartment) || (object)Helper.PP == null || !Helper.PP.Exists())
				{
					return false;
				}
				closure_0024__22_002D2._0024VB_0024Local_actor = Helper.PP;
				closure_0024__22_002D2._0024VB_0024Local_address = closure_0024__22_002D2._0024VB_0024Local_actor.MemoryAddress;
				closure_0024__22_002D2._0024VB_0024Local_model = closure_0024__22_002D2._0024VB_0024Local_actor.Model.Hash;
				closure_0024__22_002D2._0024VB_0024Local_owner = Helper.GetPlayer();
				closure_0024__22_002D2._0024VB_0024Local_key = closure_0024__22_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_apartment.Name;
				closure_0024__22_002D2._0024VB_0024Local_folder = closure_0024__22_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_apartment.StorageFolder(closure_0024__22_002D2._0024VB_0024Local_owner);
				closure_0024__22_002D2._0024VB_0024Local_price = closure_0024__22_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_apartment.Price;
				string failure = null;
				if (!InventoryEmpty(closure_0024__22_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_apartment, closure_0024__22_002D2._0024VB_0024Local_owner, ref failure))
				{
					Notification.Show(failure);
					return false;
				}
				if (!ContextValid(closure_0024__22_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_apartment, closure_0024__22_002D2._0024VB_0024Local_actor, closure_0024__22_002D2._0024VB_0024Local_address, closure_0024__22_002D2._0024VB_0024Local_model, closure_0024__22_002D2._0024VB_0024Local_owner, closure_0024__22_002D2._0024VB_0024Local_key, closure_0024__22_002D2._0024VB_0024Local_folder, closure_0024__22_002D2._0024VB_0024Local_price))
				{
					Notification.Show("This property sale is no longer available for the current story character.");
					return false;
				}
				int amountDelta = FairSalePrice(closure_0024__22_002D2._0024VB_0024Local_price);
				flag = ((!closure_0024__22_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_apartment.HasCharacterProfile()) ? PropertyTransactions.TryCommitOwnerChange("scripts\\SPA II\\modconfig.ini", closure_0024__22_002D2._0024VB_0024Local_key, closure_0024__22_002D2._0024VB_0024Local_owner, eOwner.Nobody, closure_0024__22_002D2._0024VB_0024Local_owner, amountDelta, closure_0024__22_002D2._Lambda_0024__3, closure_0024__22_002D2._Lambda_0024__4, closure_0024__22_002D2._Lambda_0024__5, ref failure) : PropertyTransactions.TryCommitCharacterOwnerChange("scripts\\SPA II\\modconfig.ini", closure_0024__22_002D2._0024VB_0024Local_key, closure_0024__22_002D2._0024VB_0024Local_owner, eOwner.Nobody, closure_0024__22_002D2._0024VB_0024Local_owner, amountDelta, closure_0024__22_002D2._Lambda_0024__0, closure_0024__22_002D2._Lambda_0024__1, closure_0024__22_002D2._Lambda_0024__2, ref failure));
				if (!flag)
				{
					Notification.Show(failure ?? "The property sale could not be completed.");
					return false;
				}
				try
				{
					ConfigHelper.config = ScriptSettings.Load("scripts\\SPA II\\modconfig.ini");
					BuildingClass buildingClass = closure_0024__22_002D2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_apartment.Building();
					buildingClass.RefreshSaleSign();
					if (buildingClass.BuyMenu != null)
					{
						buildingClass.BuyMenu.Visible = false;
					}
					if (buildingClass.AptMenu != null)
					{
						buildingClass.AptMenu.Visible = false;
					}
					if (buildingClass.GrgMenu != null)
					{
						buildingClass.GrgMenu.Visible = false;
					}
					buildingClass.RefreshBuyMenu();
					buildingClass.RefreshAptMenu();
					buildingClass.RefreshGrgMenu();
					buildingClass.RefreshBlips();
					Notification.Show("Property sold for $" + amountDelta.ToString("N0") + ".");
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Note("Property sale display: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
				Note("Property sold: " + closure_0024__22_002D2._0024VB_0024Local_key + "; owner=" + closure_0024__22_002D2._0024VB_0024Local_owner.ToString() + "; credited=" + amountDelta);
				return true;
			}
			catch (Exception ex3)
			{
				ProjectData.SetProjectError(ex3);
				Exception ex4 = ex3;
				Note("Property sale: " + ex4.Message);
				if (!flag)
				{
					Notification.Show("The property sale failed. Check SPA II.log before retrying.");
				}
				bool result = flag;
				ProjectData.ClearProjectError();
				return result;
			}
		}
	}
}
