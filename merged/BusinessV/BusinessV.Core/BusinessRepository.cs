using System;
using System.Collections.Generic;
using System.IO;
using BusinessV.Infrastructure;
using BusinessV.Models;

namespace BusinessV.Core
{
	internal sealed class BusinessRepository
	{
		private const string DefaultResourceName = "BusinessV.Defaults.businesses.json";

		private readonly Logger logger;

		public BusinessRepository(Logger logger)
		{
			this.logger = logger;
		}

		public IReadOnlyList<BusinessDefinition> Load(string path)
		{
			string fullPath = Path.GetFullPath(path);
			if (File.Exists(fullPath))
			{
				try
				{
					logger.Info("Loading business definitions: " + fullPath);
					return ValidateLoaded(JsonFile.Read<List<BusinessDefinition>>(fullPath), fullPath);
				}
				catch (Exception exception)
				{
					logger.Exception("Business definitions at " + fullPath + " could not be loaded; embedded defaults are used without changing the file", exception);
				}
			}
			string json = ReadEmbeddedDefaults();
			if (!File.Exists(fullPath))
			{
				CreateDefaultFile(fullPath, json);
			}
			logger.Info("Loading embedded business definitions: BusinessV.Defaults.businesses.json (install path: " + fullPath + ")");
			return ValidateLoaded(JsonFile.Deserialize<List<BusinessDefinition>>(json), "embedded resource BusinessV.Defaults.businesses.json");
		}

		private IReadOnlyList<BusinessDefinition> ValidateLoaded(List<BusinessDefinition> definitions, string source)
		{
			definitions = definitions ?? new List<BusinessDefinition>();
			List<string> list = Validate(definitions);
			foreach (string item in list)
			{
				logger.Error("Business definition validation: " + item);
			}
			if (list.Count != 0)
			{
				throw new InvalidDataException("Business definitions from " + source + " are invalid. See BusinessV.log for details.");
			}
			logger.Info("Loaded 50 businesses from " + source);
			logger.Info("Validated purchase interaction points: 50/50");
			int num = 0;
			for (int i = 0; i < definitions.Count; i++)
			{
				if (definitions[i].InteriorManagementPoint != null)
				{
					num++;
				}
			}
			logger.Info("Validated accessible interior management points: " + num + ".");
			return definitions;
		}

		private static string ReadEmbeddedDefaults()
		{
			using (Stream stream = typeof(BusinessRepository).Assembly.GetManifestResourceStream("BusinessV.Defaults.businesses.json"))
			{
				if (stream == null)
				{
					throw new InvalidDataException("Embedded business defaults are unavailable: BusinessV.Defaults.businesses.json");
				}
				using (StreamReader streamReader = new StreamReader(stream))
				{
					return streamReader.ReadToEnd();
				}
			}
		}

		private void CreateDefaultFile(string path, string json)
		{
			try
			{
				JsonFile.WriteNew(path, json);
				logger.Info("Created default business definitions: " + path);
			}
			catch (Exception ex)
			{
				if (File.Exists(path))
				{
					logger.Info("Business definitions were created concurrently: " + path);
				}
				else
				{
					logger.Warn("Could not create business definitions at " + path + "; embedded defaults remain active. " + ex.Message);
				}
			}
		}

		public static List<string> Validate(IReadOnlyList<BusinessDefinition> definitions)
		{
			List<string> list = new List<string>();
			if (definitions == null)
			{
				list.Add("Definition list is null.");
				return list;
			}
			if (definitions.Count != 50)
			{
				list.Add("Expected exactly 50 businesses, found " + definitions.Count + ".");
			}
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			HashSet<string> hashSet2 = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			HashSet<string> hashSet3 = new HashSet<string>(new string[7] { "Automotive", "Entertainment", "Food & Hospitality", "Industrial", "Premium", "Retail", "Services" }, StringComparer.OrdinalIgnoreCase);
			for (int i = 0; i < definitions.Count; i++)
			{
				BusinessDefinition businessDefinition = definitions[i];
				string text = "entry " + (i + 1);
				if (businessDefinition == null)
				{
					list.Add(text + " is null.");
					continue;
				}
				if (string.IsNullOrWhiteSpace(businessDefinition.Id))
				{
					list.Add(text + " has an empty ID.");
				}
				else if (!hashSet.Add(businessDefinition.Id.Trim()))
				{
					list.Add(text + " has duplicate ID '" + businessDefinition.Id + "'.");
				}
				if (string.IsNullOrWhiteSpace(businessDefinition.Name))
				{
					list.Add(text + " has an empty name.");
				}
				else if (!hashSet2.Add(businessDefinition.Name.Trim()))
				{
					list.Add(text + " has duplicate name '" + businessDefinition.Name + "'.");
				}
				if (string.IsNullOrWhiteSpace(businessDefinition.Category) || !hashSet3.Contains(businessDefinition.Category.Trim()))
				{
					list.Add(text + " has an invalid category.");
				}
				if (!Plausible(businessDefinition.Position))
				{
					list.Add(text + " has an invalid or implausible position.");
				}
				if (!Plausible(businessDefinition.PurchaseInteractionPoint))
				{
					list.Add(text + " has an invalid or implausible purchase interaction point.");
				}
				else if (businessDefinition.Position != null && DistanceSquared(businessDefinition.Position, businessDefinition.PurchaseInteractionPoint) > 1225f)
				{
					list.Add(text + " has a purchase interaction point more than 35 metres from its map position.");
				}
				else if (businessDefinition.Position != null && !NativeBusinessBlipPolicy.IsVanillaProperty(businessDefinition.Id) && DistanceSquared(businessDefinition.Position, businessDefinition.PurchaseInteractionPoint) < 6.25f)
				{
					list.Add(text + " has a BusinessV interaction point too close to its vanilla location.");
				}
				if (businessDefinition.InteriorManagementPoint != null)
				{
					if (!Plausible(businessDefinition.InteriorManagementPoint))
					{
						list.Add(text + " has an invalid or implausible interior management point.");
					}
					else if (businessDefinition.Position != null && DistanceSquared(businessDefinition.Position, businessDefinition.InteriorManagementPoint) > 625f)
					{
						list.Add(text + " has an interior management point more than 25 metres from its map position.");
					}
					else if (businessDefinition.Position != null && DistanceSquared(businessDefinition.Position, businessDefinition.InteriorManagementPoint) < 6.25f)
					{
						list.Add(text + " has an interior management point too close to its vanilla interaction.");
					}
				}
				if (businessDefinition.PurchasePrice <= 0)
				{
					list.Add(text + " has a non-positive purchase price.");
				}
				if (businessDefinition.BaseDailyIncome <= 0)
				{
					list.Add(text + " has a non-positive daily income.");
				}
				else if (!BusinessEconomyPolicy.IsBaseEconomyBalanced(businessDefinition.PurchasePrice, businessDefinition.BaseDailyIncome))
				{
					list.Add(text + " has an unbalanced purchase-price to income ratio.");
				}
				if (businessDefinition.BlipSprite <= 0)
				{
					list.Add(text + " has an invalid blip sprite.");
				}
				else if (!string.IsNullOrWhiteSpace(businessDefinition.Id) && !string.IsNullOrWhiteSpace(businessDefinition.Category) && !NativeBusinessBlipPolicy.HasReviewedSourceProfile(businessDefinition.Id, businessDefinition.Category, businessDefinition.BlipSprite))
				{
					list.Add(text + " has an invalid vanilla blip conflict profile.");
				}
				for (int j = 0; j < i; j++)
				{
					BusinessDefinition businessDefinition2 = definitions[j];
					if (businessDefinition2?.Position == null || businessDefinition.Position == null)
					{
						continue;
					}
					float num = businessDefinition.Position.X - businessDefinition2.Position.X;
					float num2 = businessDefinition.Position.Y - businessDefinition2.Position.Y;
					float num3 = businessDefinition.Position.Z - businessDefinition2.Position.Z;
					if (num * num + num2 * num2 + num3 * num3 < 1f)
					{
						list.Add(text + " is practically identical to entry " + (j + 1) + ".");
					}
					if (businessDefinition2?.PurchaseInteractionPoint != null && businessDefinition.PurchaseInteractionPoint != null)
					{
						if (DistanceSquared(businessDefinition.PurchaseInteractionPoint, businessDefinition2.PurchaseInteractionPoint) < 1f)
						{
							list.Add(text + " has a purchase interaction point practically identical to entry " + (j + 1) + ".");
						}
						if (businessDefinition2.InteriorManagementPoint != null && businessDefinition.InteriorManagementPoint != null && DistanceSquared(businessDefinition.InteriorManagementPoint, businessDefinition2.InteriorManagementPoint) < 1f)
						{
							list.Add(text + " has an interior management point practically identical to entry " + (j + 1) + ".");
						}
					}
				}
			}
			return list;
		}

		public static bool Plausible(WorldPosition value)
		{
			if (value != null && Finite(value.X) && Finite(value.Y) && Finite(value.Z) && value.X >= -5000f && value.X <= 5000f && value.Y >= -5000f && value.Y <= 8500f && value.Z >= -100f && value.Z <= 1000f)
			{
				if (!(Math.Abs(value.X) > 1f))
				{
					return Math.Abs(value.Y) > 1f;
				}
				return true;
			}
			return false;
		}

		private static bool Finite(float value)
		{
			if (!float.IsNaN(value))
			{
				return !float.IsInfinity(value);
			}
			return false;
		}

		private static float DistanceSquared(WorldPosition first, WorldPosition second)
		{
			float num = first.X - second.X;
			float num2 = first.Y - second.Y;
			float num3 = first.Z - second.Z;
			return num * num + num2 * num2 + num3 * num3;
		}
	}
}
