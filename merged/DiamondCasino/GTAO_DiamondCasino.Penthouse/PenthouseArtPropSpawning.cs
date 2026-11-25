using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;

namespace GTAO_DiamondCasino.Penthouse
{
	internal class PenthouseArtPropSpawning : Script
	{
		public static bool CreatedProps;

		public static List<Prop> ArtProps = new List<Prop>();

		public static Vector3 Art_POSITION1 = new Vector3(983.9946f, 67.53082f, 116.2f);

		public static Vector3 Art_POSITION2 = new Vector3(971.1457f, 47.36984f, 116.2f);

		public static Vector3 Art_POSITION3 = new Vector3(961.6328f, 37.60376f, 116.2f);

		public static Vector3 Art_POSITION4 = new Vector3(956.2783f, 40.9492f, 116.2f);

		public static Vector3 Art_POSITION5 = new Vector3(965.6121f, 27.96647f, 116.2f);

		public static Vector3 Art_POSITION6 = new Vector3(957.3006f, 28.48209f, 116.2f);

		public static Vector3 Art_POSITION7 = new Vector3(958.4638f, 30.34361f, 116.2f);

		public static Vector3 Art_POSITION8 = new Vector3(970.4116f, 51.76957f, 116.2f);

		public static Vector3 Art_POSITION9 = new Vector3(964.9167f, 55.20304f, 116.2f);

		public static Vector3 Art_POSITION10 = new Vector3(972.3202f, 52.8088f, 116.2f);

		public static Vector3 Art_POSITION11 = new Vector3(974.3784f, 56.95401f, 116.2f);

		public static Vector3 Art_POSITION12 = new Vector3(972.0966f, 53.30207f, 116.2f);

		public static Vector3 Art_POSITION13 = new Vector3(973.5657f, 59.20665f, 116.2f);

		public static Vector3 Art_POSITION14 = new Vector3(970.0708f, 61.39066f, 116.2f);

		public static string ArtPropName_1;

		public static string ArtPropName_2;

		public static string ArtPropName_3;

		public static string ArtPropName_4;

		public static string ArtPropName_5;

		public static string ArtPropName_6;

		public static string ArtPropName_7;

		public static string ArtPropName_8;

		public static string ArtPropName_9;

		public static string ArtPropName_10;

		public static string ArtPropName_11;

		public static string ArtPropName_12;

		public static string ArtPropName_13;

		public static string ArtPropName_14;

		public PenthouseArtPropSpawning()
		{
			base.Tick += OnTick;
		}

		public static void OnTick(object sender, EventArgs e)
		{
			if (Class1.Initialized && PenthouseStuff.IsInPenhouse && PenthouseStuff.IsInPenhouse && !CreatedProps && !CreatedProps)
			{
				CreatedProps = true;
				SpawnSculptures();
				SpawnArt();
			}
		}

		public static Model RequestModel(string Prop)
		{
			Model result = new Model(Prop);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(1);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public static Model RequestModel(VehicleHash Prop)
		{
			Model result = new Model(Prop);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(1);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public static Model RequestModel(PedHash Prop)
		{
			Model result = new Model(Prop);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(1);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public static void RefreshArtAndSculptures()
		{
			foreach (Prop artProp in ArtProps)
			{
				if (artProp != null)
				{
					artProp.Delete();
				}
			}
			ArtProps.Clear();
			SpawnArt();
			SpawnSculptures();
		}

		public static void SpawnArt()
		{
			for (int i = 0; i < PenthouseArtPlacement.PaintingLocations.Count; i++)
			{
				string defaultvalue = "null";
				defaultvalue = Class1.Config.GetValue("Art", "ArtPropName_" + (i + 1), defaultvalue);
				if (!defaultvalue.Equals("null") && !defaultvalue.Equals("None"))
				{
					Vector3 vector = new Vector3(PenthouseArtPlacement.PaintingLocations[i].X, PenthouseArtPlacement.PaintingLocations[i].Y, PenthouseArtPlacement.PaintingLocations[i].Z);
					Prop prop = GlobalHelper.CreateProp(RequestModel(defaultvalue), Art_POSITION1, true, false);
					prop.Position = new Vector3(vector.X, vector.Y, vector.Z - PenthouseArtPlacement.GetDimensions(prop).Z / 5.5f + 0.25f);
					prop.Rotation = new Vector3(0, 0, PenthouseArtPlacement.PaintingLocations[i].W);
					prop.IsPositionFrozen = true;
					ArtProps.Add(prop);
				}
			}
		}

		public static void SpawnSculptures()
		{
			for (int i = 0; i < PenthouseArtPlacement.SculptLoc.Count; i++)
			{
				Class1.Config.GetValue("Art", "SculptPropName_" + i, "null");
				Class1.Config.GetValue("Art", "SculptPropRotationZ_" + i, 0);
				try
				{
					int value = Class1.Config.GetValue("Art", "SculptPropRotationZ_" + i, 0);
					string value2 = Class1.Config.GetValue("Art", "SculptPropName_" + i, "null");
					if (!value2.Equals("null") && !value2.Equals("None"))
					{
						Vector3 vector = PenthouseArtPlacement.SculptLoc[i];
						Prop prop = GlobalHelper.CreateProp(position: new Vector3(vector.X, vector.Y, vector.Z - 0.025f), model: RequestModel(value2), dynamic: true, placeOnGround: false);
						prop.Rotation = new Vector3(0f, 0f, value);
						prop.IsPositionFrozen = true;
						ArtProps.Add(prop);
					}
				}
				catch
				{
					Class1.Config.SetValue("Art", "SculptPropName_" + i, "null");
					Class1.Config.SetValue("Art", "SculptPropRotationZ_" + i, 0);
					Class1.Config.Save();
				}
			}
		}
	}
}
