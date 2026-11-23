using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	public struct VehicleData
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<bool> _0024I110_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__110_002D0()
			{
				return true;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__109_002D0
		{
			public VehicleData _0024VB_0024Local_expected;

			[SpecialName]
			internal void _Lambda_0024__0(string candidate)
			{
				ValidateCandidate(candidate, _0024VB_0024Local_expected);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__119_002D0
		{
			public Func<bool> _0024VB_0024Local_stillOwns;

			public VehicleData _0024VB_0024Local_expected;

			public eOwner _0024VB_0024Local_owner;

			public VehicleData _0024VB_0024Local_assigned;

			[SpecialName]
			internal void _Lambda_0024__0(string candidate)
			{
				VehicleData value = default(VehicleData);
				if (!_0024VB_0024Local_stillOwns() || File.Exists(_0024VB_0024Local_expected.FileName + ".bak") || !TryReadLegacyImportEvidence(_0024VB_0024Local_expected.FileName, ref value) || value.Owner != eOwner.Nobody || value.ApartmentID != _0024VB_0024Local_expected.ApartmentID || value.UniqueID != _0024VB_0024Local_expected.UniqueID || value.Index != _0024VB_0024Local_expected.Index || value.Hash != _0024VB_0024Local_expected.Hash)
				{
					throw new IOException("The original unassigned vehicle or garage owner changed during migration.");
				}
				value.Owner = _0024VB_0024Local_owner;
				_0024VB_0024Local_assigned = value;
				value.WriteCandidate(candidate);
			}

			[SpecialName]
			internal void _Lambda_0024__1(string candidate)
			{
				ValidateCandidate(candidate, _0024VB_0024Local_assigned);
			}
		}

		private const int MaxTransferXmlBytes = 4194304;

		private static readonly HashSet<string> readWarnings = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		public int Index;

		public string Make;

		public string Name;

		public int Hash;

		public eOwner Owner;

		public int Aerials;

		public int Suspension;

		public int Armor;

		public int Brakes;

		public int Engine;

		public int Transmission;

		public int FrontBumper;

		public int RearBumper;

		public int SideSkirt;

		public int Trim;

		public int EngineBlock;

		public int AirFilter;

		public int Struts;

		public int ColumnShifterLevers;

		public int Dashboard;

		public int DialDesign;

		public int Ornaments;

		public int Seats;

		public int SteeringWheel;

		public int TrimDesign;

		public int TrimColor;

		public int PlateHolder;

		public int VanityPlates;

		public int NumberPlate;

		public string PlateNumber;

		public int WheelType;

		public int Frontwheels;

		public int BackWheels;

		public bool WheelsVariation;

		public bool Headlights;

		public bool FrontNeon;

		public bool BackNeon;

		public bool LeftNeon;

		public bool RightNeon;

		public int ArchCover;

		public int Exhaust;

		public int Fender;

		public int RightFender;

		public int DoorSpeakers;

		public int Frame;

		public int Grille;

		public int Hood;

		public int Horn;

		public int Hydraulics;

		public int Livery;

		public int Plaques;

		public int Roof;

		public int Speakers;

		public int Spoiler;

		public int Tank;

		public int Trunk;

		public int Windows;

		public bool Turbo;

		public int Tint;

		public int PrimaryColor;

		public int SecondaryColor;

		public int PearlescentColor;

		public int RimColor;

		public int LightsColor;

		public VsColor NeonLightsColor;

		public VsColor TireSmokeColor;

		public bool Tiresmoke;

		public int Livery2;

		public int HeadlightsColor;

		public bool BulletProofTires;

		public VsColor CustomPrimaryColor;

		public VsColor CustomSecondaryColor;

		public bool HasCustomPrimaryColor;

		public bool HasCustomSecondaryColor;

		public bool Extra0;

		public bool Extra1;

		public bool Extra2;

		public bool Extra3;

		public bool Extra4;

		public bool Extra5;

		public bool Extra6;

		public bool Extra7;

		public bool Extra8;

		public bool Extra9;

		public bool Extra10;

		public bool Extra11;

		public bool Extra12;

		public bool Extra13;

		public bool Extra14;

		public bool Extra15;

		public int RoofState;

		public bool HasNitro;

		public int Livery1;

		public int ApartmentID;

		public int UniqueID;

		public VehicleData Instance => ReadFromFile();

		[XmlIgnore]
		public string FileName { get; set; }

		public VehicleData(string _filename)
		{
			this = default(VehicleData);
			FileName = _filename;
		}

		public VehicleData(string _filename, VehicleClass vehicle)
		{
			this = default(VehicleData);
			FileName = _filename;
			Index = vehicle.Index;
			Make = vehicle.Make;
			Name = vehicle.Name;
			Hash = vehicle.Hash;
			Owner = vehicle.Owner;
			Aerials = vehicle.Aerials;
			Suspension = vehicle.Suspension;
			Armor = vehicle.Armor;
			Brakes = vehicle.Brakes;
			Engine = vehicle.Engine;
			Transmission = vehicle.Transmission;
			FrontBumper = vehicle.FrontBumper;
			RearBumper = vehicle.RearBumper;
			SideSkirt = vehicle.SideSkirt;
			Trim = vehicle.Trim;
			EngineBlock = vehicle.EngineBlock;
			AirFilter = vehicle.AirFilter;
			Struts = vehicle.Struts;
			ColumnShifterLevers = vehicle.ColumnShifterLevers;
			Dashboard = vehicle.Dashboard;
			DialDesign = vehicle.DialDesign;
			Ornaments = vehicle.Ornaments;
			Seats = vehicle.Seats;
			SteeringWheel = vehicle.SteeringWheel;
			TrimDesign = vehicle.TrimDesign;
			TrimColor = vehicle.TrimColor;
			PlateHolder = vehicle.PlateHolder;
			VanityPlates = vehicle.VanityPlates;
			NumberPlate = vehicle.NumberPlate;
			PlateNumber = vehicle.PlateNumber;
			WheelType = vehicle.WheelType;
			Frontwheels = vehicle.Frontwheels;
			BackWheels = vehicle.BackWheels;
			WheelsVariation = vehicle.WheelsVariation;
			Headlights = vehicle.Headlights;
			FrontNeon = vehicle.FrontNeon;
			BackNeon = vehicle.BackNeon;
			LeftNeon = vehicle.LeftNeon;
			RightNeon = vehicle.RightNeon;
			ArchCover = vehicle.ArchCover;
			Exhaust = vehicle.Exhaust;
			Fender = vehicle.Fender;
			RightFender = vehicle.RightFender;
			DoorSpeakers = vehicle.DoorSpeakers;
			Frame = vehicle.Frame;
			Grille = vehicle.Grille;
			Hood = vehicle.Hood;
			Horn = vehicle.Horn;
			Hydraulics = vehicle.Hydraulics;
			Livery = vehicle.Livery;
			Plaques = vehicle.Plaques;
			Roof = vehicle.Roof;
			Speakers = vehicle.Speakers;
			Spoiler = vehicle.Spoiler;
			Tank = vehicle.Tank;
			Trunk = vehicle.Trunk;
			Windows = vehicle.Windows;
			Turbo = vehicle.Turbo;
			Tint = vehicle.Tint;
			PrimaryColor = vehicle.PrimaryColor;
			SecondaryColor = vehicle.SecondaryColor;
			PearlescentColor = vehicle.PearlescentColor;
			RimColor = vehicle.RimColor;
			LightsColor = vehicle.LightsColor;
			NeonLightsColor = vehicle.NeonLightsColor;
			TireSmokeColor = vehicle.TireSmokeColor;
			Tiresmoke = vehicle.Tiresmoke;
			Livery2 = vehicle.Livery2;
			HeadlightsColor = vehicle.HeadlightsColor;
			BulletProofTires = vehicle.BulletProofTires;
			CustomPrimaryColor = vehicle.CustomPrimaryColor;
			CustomSecondaryColor = vehicle.CustomSecondaryColor;
			HasCustomPrimaryColor = vehicle.HasCustomPrimaryColor;
			HasCustomSecondaryColor = vehicle.HasCustomSecondaryColor;
			Extra0 = vehicle.Extra0;
			Extra1 = vehicle.Extra1;
			Extra2 = vehicle.Extra2;
			Extra3 = vehicle.Extra3;
			Extra4 = vehicle.Extra4;
			Extra5 = vehicle.Extra5;
			Extra6 = vehicle.Extra6;
			Extra7 = vehicle.Extra7;
			Extra8 = vehicle.Extra8;
			Extra9 = vehicle.Extra9;
			Extra10 = vehicle.Extra10;
			Extra11 = vehicle.Extra11;
			Extra12 = vehicle.Extra12;
			Extra13 = vehicle.Extra13;
			Extra14 = vehicle.Extra14;
			Extra15 = vehicle.Extra15;
			RoofState = vehicle.RoofState;
			Livery1 = vehicle.Livery1;
			HasNitro = vehicle.HasNitro != 0;
			ApartmentID = vehicle.ApartmentID;
			UniqueID = vehicle.UniqueID;
		}

		public void Save()
		{
			using (AtomicFileTransaction atomicFileTransaction = BeginSaveTransaction())
			{
				atomicFileTransaction.Complete();
			}
		}

		public AtomicFileTransaction BeginSaveTransaction()
		{
			return BeginVehicleSave(false);
		}

		private AtomicFileTransaction BeginVehicleSave(bool createOnly)
		{
			_Closure_0024__109_002D0 closure_0024__109_002D = new _Closure_0024__109_002D0();
			closure_0024__109_002D._0024VB_0024Local_expected = this;
			ValidateRecord(closure_0024__109_002D._0024VB_0024Local_expected);
			Action<string> writeCandidate = closure_0024__109_002D._0024VB_0024Local_expected.WriteCandidate;
			Action<string> validateCandidate = closure_0024__109_002D._Lambda_0024__0;
			if (createOnly)
			{
				return AtomicFileTransaction.BeginCreate(FileName, writeCandidate, validateCandidate);
			}
			return AtomicFileTransaction.BeginWrite(FileName, writeCandidate, validateCandidate);
		}

		public void SaveTransferred(string sourcePath, int expectedSourceUid = 0)
		{
			if (string.IsNullOrWhiteSpace(sourcePath) || !File.Exists(sourcePath))
			{
				Save();
				return;
			}
			VehicleData vehicleData = new VehicleData(sourcePath);
			VehicleData value = default(VehicleData);
			bool flag = string.Equals(Path.GetFullPath(sourcePath), Path.GetFullPath(FileName), StringComparison.OrdinalIgnoreCase);
			if (!vehicleData.TryRead(ref value) || value.Hash != Hash || value.Owner != Owner || (flag && value.ApartmentID != ApartmentID) || (expectedSourceUid != 0 && value.UniqueID != expectedSourceUid))
			{
				throw new InvalidDataException("The original saved vehicle did not match the vehicle being transferred; both have been kept.");
			}
			if (flag)
			{
				Save();
			}
			else
			{
				CommitTransfer(sourcePath, File.ReadAllBytes(sourcePath), false, (_Closure_0024__._0024I110_002D0 != null) ? _Closure_0024__._0024I110_002D0 : (_Closure_0024__._0024I110_002D0 = _Closure_0024__._0024I._Lambda_0024__110_002D0));
			}
		}

		internal bool TrySaveTransferred(string sourcePath, int sourceGarageId, int sourceUid, Func<bool> accept)
		{
			if (accept == null || sourceGarageId <= 0 || sourceUid == 0 || sourceGarageId == ApartmentID || ApartmentID <= 0 || UniqueID == 0 || Hash == 0 || Owner < eOwner.Michael || Owner > eOwner.Trevor || !File.Exists(sourcePath) || string.IsNullOrWhiteSpace(FileName) || string.Equals(Path.GetFullPath(sourcePath), Path.GetFullPath(FileName), StringComparison.OrdinalIgnoreCase) || File.Exists(FileName) || File.Exists(FileName + ".bak"))
			{
				return false;
			}
			byte[] array;
			using (FileStream fileStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				if (fileStream.Length <= 0 || fileStream.Length > 4194304)
				{
					return false;
				}
				using (BinaryReader binaryReader = new BinaryReader(fileStream))
				{
					array = binaryReader.ReadBytes(checked((int)fileStream.Length));
				}
			}
			VehicleData vehicleData;
			try
			{
				XmlReaderSettings settings = new XmlReaderSettings
				{
					DtdProcessing = DtdProcessing.Prohibit,
					XmlResolver = null,
					MaxCharactersInDocument = 4194304L
				};
				using (MemoryStream input = new MemoryStream(array, false))
				{
					using (XmlReader xmlReader = XmlReader.Create(input, settings))
					{
						object obj = new XmlSerializer(typeof(VehicleData)).Deserialize(xmlReader);
						vehicleData = ((obj != null) ? ((VehicleData)obj) : default(VehicleData));
					}
				}
			}
			catch (InvalidOperationException ex)
			{
				ProjectData.SetProjectError(ex);
				InvalidOperationException ex2 = ex;
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
			if (vehicleData.Hash != Hash || vehicleData.Owner != Owner || vehicleData.ApartmentID != sourceGarageId || vehicleData.UniqueID != sourceUid || vehicleData.Index < 0)
			{
				return false;
			}
			return CommitTransfer(sourcePath, array, true, accept);
		}

		private bool CommitTransfer(string sourcePath, byte[] sourceBytes, bool createOnly, Func<bool> accept)
		{
			string text = sourcePath + ".transferred." + Guid.NewGuid().ToString("N");
			using (AtomicFileTransaction atomicFileTransaction = BeginVehicleSave(createOnly))
			{
				bool flag = default(bool);
				bool flag2 = default(bool);
				try
				{
					if (!File.ReadAllBytes(sourcePath).SequenceEqual(sourceBytes))
					{
						throw new IOException("The source vehicle changed while preparing its transfer; it has been kept.");
					}
					File.Move(sourcePath, text);
					flag = true;
					if (!File.ReadAllBytes(text).SequenceEqual(sourceBytes))
					{
						throw new IOException("The source vehicle changed during transfer; it has been kept.");
					}
					if (!accept())
					{
						return false;
					}
					atomicFileTransaction.Complete();
					flag2 = true;
				}
				finally
				{
					if (flag && !flag2)
					{
						if (File.Exists(sourcePath))
						{
							throw new IOException("The transfer failed and the source path was recreated. Its previous file is retained at " + text);
						}
						File.Move(text, sourcePath);
					}
				}
			}
			try
			{
				File.Delete(text);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				ProjectData.ClearProjectError();
			}
			return true;
		}

		public static void SaveTogether(IEnumerable<VehicleData> values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			List<VehicleData> list = new List<VehicleData>(values);
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (VehicleData item3 in list)
			{
				ValidateRecord(item3);
				if (!hashSet.Add(Path.GetFullPath(item3.FileName)))
				{
					throw new ArgumentException("A save batch contains the same target more than once.", "values");
				}
			}
			List<AtomicFileTransaction> list2 = new List<AtomicFileTransaction>();
			checked
			{
				try
				{
					foreach (VehicleData item4 in list)
					{
						list2.Add(item4.BeginSaveTransaction());
					}
					foreach (AtomicFileTransaction item5 in list2)
					{
						item5.Complete();
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception item = ex;
					List<Exception> list3 = new List<Exception> { item };
					for (int i = list2.Count - 1; i >= 0; i += -1)
					{
						try
						{
							list2[i].Rollback();
						}
						catch (Exception ex2)
						{
							ProjectData.SetProjectError(ex2);
							Exception item2 = ex2;
							list3.Add(item2);
							ProjectData.ClearProjectError();
						}
					}
					if (list3.Count > 1)
					{
						throw new AggregateException("The vehicle save failed and a backup could not be restored automatically.", list3);
					}
					throw;
				}
				finally
				{
					foreach (AtomicFileTransaction item6 in list2)
					{
						try
						{
							item6.Dispose();
						}
						catch (Exception ex3)
						{
							ProjectData.SetProjectError(ex3);
							Exception ex4 = ex3;
							ProjectData.ClearProjectError();
						}
					}
				}
			}
		}

		private void WriteCandidate(string candidate)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(VehicleData));
			using (FileStream stream = new FileStream(candidate, FileMode.CreateNew, FileAccess.Write, FileShare.None))
			{
				using (StreamWriter textWriter = new StreamWriter(stream, new UTF8Encoding(false)))
				{
					xmlSerializer.Serialize(textWriter, this);
				}
			}
		}

		private static void ValidateRecord(VehicleData record)
		{
			if (record.Hash == 0)
			{
				throw new InvalidDataException("A vehicle save must contain a valid model hash.");
			}
			if (record.Index < 0)
			{
				throw new InvalidDataException("A vehicle save cannot use a negative garage slot.");
			}
			if (string.IsNullOrWhiteSpace(record.FileName))
			{
				throw new InvalidDataException("A vehicle save path is missing.");
			}
		}

		private static void ValidateCandidate(string candidate, VehicleData expected)
		{
			VehicleData value = default(VehicleData);
			string reason = null;
			if (!TryReadRaw(candidate, ref value, ref reason))
			{
				throw new InvalidDataException("Vehicle XML verification failed: " + reason);
			}
			FieldInfo[] fields = typeof(VehicleData).GetFields(BindingFlags.Instance | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (!object.Equals(RuntimeHelpers.GetObjectValue(fieldInfo.GetValue(expected)), RuntimeHelpers.GetObjectValue(fieldInfo.GetValue(value))))
				{
					throw new InvalidDataException("Vehicle XML verification failed for " + fieldInfo.Name + ".");
				}
			}
		}

		private static bool TryReadRaw(string path, ref VehicleData value, ref string reason)
		{
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(VehicleData));
				using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					using (StreamReader textReader = new StreamReader(stream))
					{
						object obj = xmlSerializer.Deserialize(textReader);
						value = ((obj != null) ? ((VehicleData)obj) : default(VehicleData));
					}
				}
				value.FileName = path;
				ValidateRecord(value);
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				value = default(VehicleData);
				reason = ex2.Message;
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		internal static bool TryReadLegacyImportEvidence(string path, ref VehicleData value)
		{
			FileInfo fileInfo = new FileInfo(path);
			string reason = null;
			if (fileInfo.Exists && fileInfo.Length <= 4194304)
			{
				return TryReadRaw(path, ref value, ref reason);
			}
			return false;
		}

		internal static void AssignLegacyImportOwner(VehicleData expected, eOwner owner, Func<bool> stillOwns)
		{
			_Closure_0024__119_002D0 closure_0024__119_002D = new _Closure_0024__119_002D0();
			closure_0024__119_002D._0024VB_0024Local_expected = expected;
			closure_0024__119_002D._0024VB_0024Local_owner = owner;
			closure_0024__119_002D._0024VB_0024Local_stillOwns = stillOwns;
			closure_0024__119_002D._0024VB_0024Local_assigned = default(VehicleData);
			using (AtomicFileTransaction atomicFileTransaction = AtomicFileTransaction.BeginLegacyVehicleMigration(closure_0024__119_002D._0024VB_0024Local_expected.FileName, closure_0024__119_002D._Lambda_0024__0, closure_0024__119_002D._Lambda_0024__1))
			{
				atomicFileTransaction.Complete();
			}
		}

		public bool TryRead(ref VehicleData value)
		{
			string reason = null;
			if (TryReadRaw(FileName, ref value, ref reason))
			{
				lock (readWarnings)
				{
					readWarnings.Remove(FileName ?? "");
				}
				return true;
			}
			string reason2 = null;
			if (!string.IsNullOrWhiteSpace(FileName) && TryReadRaw(FileName + ".bak", ref value, ref reason2))
			{
				value.FileName = FileName;
				ReportReadWarning("Vehicle XML recovered from prior backup; original kept for inspection: " + FileName + "; " + reason);
				return true;
			}
			value = default(VehicleData);
			ReportReadWarning("Vehicle XML rejected and kept unchanged: " + (FileName ?? "(missing path)") + "; " + reason);
			return false;
		}

		private void ReportReadWarning(string message)
		{
			bool flag;
			lock (readWarnings)
			{
				flag = readWarnings.Add(FileName ?? "");
			}
			if (flag)
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
		}

		public VehicleData ReadFromFile()
		{
			VehicleData value = default(VehicleData);
			if (TryRead(ref value))
			{
				return value;
			}
			throw new InvalidDataException("Vehicle save could not be read; the original file was kept: " + FileName);
		}
	}
}
