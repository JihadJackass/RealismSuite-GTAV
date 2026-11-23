using System;
using System.IO;
using System.Windows.Forms;
using GTA;

namespace VehicleMechanics
{
	public static class Config
	{
		public static bool RolloverEnabled = true;

		public static int RolloverChance = 50;

		public static float RolloverMinSpeed = 8f;

		public static bool TireBurstEnabled = true;

		public static int TireBurstChance = 35;

		public static float TireBurstMinSpeed = 12f;

		public static bool EngineFireEnabled = true;

		public static int EngineFireChance = 25;

		public static float EngineFireHealthThreshold = 250f;

		public static bool FuelEnabled = true;

		public static float FuelConsumption = 0.1f;

		public static bool DeformationEnabled = true;

		public static bool BrakeFadeEnabled = true;

		public static float BrakeFadeRate = 0.015f;

		public static float BrakeRecoveryRate = 0.008f;

		public static bool OverheatEnabled = true;

		public static float OverheatRate = 0.04f;

		public static float OverheatCoolRate = 0.03f;

		public static float OverheatWarningTemp = 95f;

		public static float OverheatCriticalTemp = 115f;

		public static bool HeadlightEnabled = true;

		public static int HeadlightChance = 60;

		public static bool WindshieldEnabled = true;

		public static int WindshieldChance = 45;

		public static bool RadiatorEnabled = true;

		public static int RadiatorLeakChance = 40;

		public static bool BatteryEnabled = true;

		public static int BatteryFailureChance = 30;

		public static bool TransmissionEnabled = true;

		public static int TransmissionDamageChance = 35;

		public static bool OilLeakEnabled = true;

		public static int OilLeakChance = 30;

		public static Keys MenuKey = Keys.F3;

		public static bool TurnSignalsEnabled = true;

		public static Keys TurnSignalLeftKey = Keys.NumPad4;

		public static Keys TurnSignalRightKey = Keys.NumPad6;

		public static Keys HazardKey = Keys.NumPad5;

		public static int MechanicCost = 250;

		public static int PriorityCost = 500;

		public static bool InstantRepairEnabled = true;

		public static bool RefuelEnabled = true;

		public static bool FuelHudEnabled = true;

		public static bool FuelHudHorizontal = false;

		public static float FuelHudX = 0.2235f;

		public static float FuelHudY = 0.89f;

		public static float FuelHudHeight = 0.1875f;

		public static float FuelHudWidth = 0.006f;

		public static float FuelHudRotation = 0f;

		public static float FuelHudScale = 1f;

		private static readonly string FileName = "VehicleMechanics.ini";

		public static void Load()
		{
			string text = Path.Combine("scripts", FileName);
			if (!File.Exists(text))
			{
				return;
			}
			try
			{
				ScriptSettings scriptSettings = ScriptSettings.Load(text);
				RolloverEnabled = scriptSettings.GetValue("Rollover", "Enabled", RolloverEnabled);
				RolloverChance = scriptSettings.GetValue("Rollover", "Chance", RolloverChance);
				RolloverMinSpeed = scriptSettings.GetValue("Rollover", "MinSpeed", RolloverMinSpeed);
				TireBurstEnabled = scriptSettings.GetValue("TireBurst", "Enabled", TireBurstEnabled);
				TireBurstChance = scriptSettings.GetValue("TireBurst", "Chance", TireBurstChance);
				TireBurstMinSpeed = scriptSettings.GetValue("TireBurst", "MinSpeed", TireBurstMinSpeed);
				EngineFireEnabled = scriptSettings.GetValue("EngineFire", "Enabled", EngineFireEnabled);
				EngineFireChance = scriptSettings.GetValue("EngineFire", "Chance", EngineFireChance);
				EngineFireHealthThreshold = scriptSettings.GetValue("EngineFire", "HealthThreshold", EngineFireHealthThreshold);
				FuelEnabled = scriptSettings.GetValue("Fuel", "Enabled", FuelEnabled);
				FuelConsumption = scriptSettings.GetValue("Fuel", "Consumption", FuelConsumption);
				DeformationEnabled = scriptSettings.GetValue("Deformation", "Enabled", DeformationEnabled);
				BrakeFadeEnabled = scriptSettings.GetValue("BrakeFade", "Enabled", BrakeFadeEnabled);
				OverheatEnabled = scriptSettings.GetValue("Overheat", "Enabled", OverheatEnabled);
				HeadlightEnabled = scriptSettings.GetValue("Headlights", "Enabled", HeadlightEnabled);
				HeadlightChance = scriptSettings.GetValue("Headlights", "Chance", HeadlightChance);
				WindshieldEnabled = scriptSettings.GetValue("Windshield", "Enabled", WindshieldEnabled);
				WindshieldChance = scriptSettings.GetValue("Windshield", "Chance", WindshieldChance);
				RadiatorEnabled = scriptSettings.GetValue("Radiator", "Enabled", RadiatorEnabled);
				RadiatorLeakChance = scriptSettings.GetValue("Radiator", "LeakChance", RadiatorLeakChance);
				BatteryEnabled = scriptSettings.GetValue("Battery", "Enabled", BatteryEnabled);
				BatteryFailureChance = scriptSettings.GetValue("Battery", "FailureChance", BatteryFailureChance);
				TransmissionEnabled = scriptSettings.GetValue("Transmission", "Enabled", TransmissionEnabled);
				TransmissionDamageChance = scriptSettings.GetValue("Transmission", "DamageChance", TransmissionDamageChance);
				OilLeakEnabled = scriptSettings.GetValue("OilLeak", "Enabled", OilLeakEnabled);
				OilLeakChance = scriptSettings.GetValue("OilLeak", "Chance", OilLeakChance);
				MechanicCost = scriptSettings.GetValue("Mechanic", "Cost", MechanicCost);
				PriorityCost = scriptSettings.GetValue("Mechanic", "PriorityCost", PriorityCost);
				InstantRepairEnabled = scriptSettings.GetValue("Mechanic", "InstantRepairEnabled", InstantRepairEnabled);
				RefuelEnabled = scriptSettings.GetValue("Mechanic", "RefuelEnabled", RefuelEnabled);
				FuelHudEnabled = scriptSettings.GetValue("FuelHud", "Enabled", FuelHudEnabled);
				FuelHudHorizontal = scriptSettings.GetValue("FuelHud", "Horizontal", FuelHudHorizontal);
				FuelHudX = scriptSettings.GetValue("FuelHud", "X", FuelHudX);
				FuelHudY = scriptSettings.GetValue("FuelHud", "Y", FuelHudY);
				FuelHudHeight = scriptSettings.GetValue("FuelHud", "Height", FuelHudHeight);
				FuelHudWidth = scriptSettings.GetValue("FuelHud", "Width", FuelHudWidth);
				FuelHudRotation = scriptSettings.GetValue("FuelHud", "Rotation", FuelHudRotation);
				FuelHudScale = scriptSettings.GetValue("FuelHud", "Scale", FuelHudScale);
				MenuKey = ParseKey(scriptSettings.GetValue("Mechanic", "MenuKey", "F3"), MenuKey);
				TurnSignalsEnabled = scriptSettings.GetValue("TurnSignals", "Enabled", TurnSignalsEnabled);
				TurnSignalLeftKey = ParseKey(scriptSettings.GetValue("TurnSignals", "LeftKey", "NumPad4"), TurnSignalLeftKey);
				TurnSignalRightKey = ParseKey(scriptSettings.GetValue("TurnSignals", "RightKey", "NumPad6"), TurnSignalRightKey);
				HazardKey = ParseKey(scriptSettings.GetValue("TurnSignals", "HazardKey", "NumPad5"), HazardKey);
			}
			catch
			{
			}
		}

		private static Keys ParseKey(string raw, Keys fallback)
		{
			if (string.IsNullOrWhiteSpace(raw))
			{
				return fallback;
			}
			string text = raw.Trim();
			string[] obj = new string[4] { "shift+", "ctrl+", "control+", "alt+" };
			string text2 = text.ToLowerInvariant();
			string[] array = obj;
			foreach (string text3 in array)
			{
				if (text2.StartsWith(text3))
				{
					text = text.Substring(text3.Length);
					text2 = text.ToLowerInvariant();
				}
			}
			int result;
			if (int.TryParse(text, out result))
			{
				try
				{
					return (Keys)result;
				}
				catch
				{
					return fallback;
				}
			}
			if (text.Length == 1)
			{
				char c = char.ToUpperInvariant(text[0]);
				if ((c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9'))
				{
					Keys result2;
					if (Enum.TryParse<Keys>(c.ToString(), out result2))
					{
						return result2;
					}
					if (Enum.TryParse<Keys>("D" + c, out result2))
					{
						return result2;
					}
				}
			}
			Keys result3;
			if (Enum.TryParse<Keys>(text, true, out result3))
			{
				return result3;
			}
			return fallback;
		}

		public static void Save()
		{
			string filename = Path.Combine("scripts", FileName);
			try
			{
				Directory.CreateDirectory("scripts");
				ScriptSettings scriptSettings = ScriptSettings.Load(filename);
				scriptSettings.SetValue("FuelHud", "Enabled", FuelHudEnabled);
				scriptSettings.SetValue("FuelHud", "Horizontal", FuelHudHorizontal);
				scriptSettings.SetValue("FuelHud", "X", FuelHudX);
				scriptSettings.SetValue("FuelHud", "Y", FuelHudY);
				scriptSettings.SetValue("FuelHud", "Height", FuelHudHeight);
				scriptSettings.SetValue("FuelHud", "Width", FuelHudWidth);
				scriptSettings.SetValue("FuelHud", "Rotation", FuelHudRotation);
				scriptSettings.SetValue("FuelHud", "Scale", FuelHudScale);
				scriptSettings.Save();
			}
			catch
			{
			}
		}

		public static void SaveDefaults()
		{
			string path = Path.Combine("scripts", FileName);
			if (File.Exists(path))
			{
				return;
			}
			try
			{
				Directory.CreateDirectory("scripts");
				File.WriteAllText(path, "[Rollover]\nEnabled=true\nChance=50\nMinSpeed=8\n\n[TireBurst]\nEnabled=true\nChance=35\nMinSpeed=12\n\n[EngineFire]\nEnabled=true\nChance=25\nHealthThreshold=250\n\n[Fuel]\nEnabled=false\nConsumption=0.1\n\n[Deformation]\nEnabled=true\n\n[BrakeFade]\nEnabled=true\n\n[Overheat]\nEnabled=true\n\n[Headlights]\nEnabled=true\nChance=60\n\n[Windshield]\nEnabled=true\nChance=45\n\n[Radiator]\nEnabled=true\nLeakChance=40\n\n[Battery]\nEnabled=true\nFailureChance=30\n\n[Transmission]\nEnabled=true\nDamageChance=35\n\n[OilLeak]\nEnabled=true\nChance=30\n\n[Mechanic]\nMenuKey=F3\nCost=250\nPriorityCost=500\nInstantRepairEnabled=true\nRefuelEnabled=true\n\n[TurnSignals]\nEnabled=true\nLeftKey=NumPad4\nRightKey=NumPad6\nHazardKey=NumPad5\n\n[FuelHud]\nEnabled=true\nX=0.2235\nY=0.8900\nHeight=0.1875\nWidth=0.006\nRotation=0\nScale=1.0\n\n");
			}
			catch
			{
			}
		}
	}
}
