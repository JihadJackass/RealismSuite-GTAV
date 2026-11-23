using GTA;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII.INM
{
	public class Clothing
	{
		public string Name;

		public string LocalizedName;

		public eClothingType Type;

		public int ComponentID;

		public int DrawableID;

		public int TextureID;

		public int PaletteID;

		public int ComponentID2;

		public int DrawableID2;

		public int TextureID2;

		public int PaletteID2;

		public int ComponentID3;

		public int DrawableID3;

		public int TextureID3;

		public int PaletteID3;

		public Clothing(string name, string local, eClothingType type, int com, int draw, int txd, int pal = 2)
		{
			Name = name;
			LocalizedName = local;
			Type = type;
			ComponentID = com;
			DrawableID = draw;
			TextureID = txd;
			PaletteID = pal;
			ComponentID2 = -1;
			DrawableID2 = -1;
			TextureID2 = -1;
			PaletteID2 = -1;
			ComponentID3 = -1;
			DrawableID3 = -1;
			TextureID3 = -1;
			PaletteID3 = -1;
		}

		public Clothing(string name, string local, eClothingType type, int com1, int draw1, int txd1, int com2, int draw2, int txd2, int pal1 = 2, int pal2 = 2)
		{
			Name = name;
			LocalizedName = local;
			Type = type;
			ComponentID = com1;
			DrawableID = draw1;
			TextureID = txd1;
			PaletteID = pal1;
			ComponentID2 = com2;
			DrawableID2 = draw2;
			TextureID2 = txd2;
			PaletteID2 = pal2;
			ComponentID3 = -1;
			DrawableID3 = -1;
			TextureID3 = -1;
			PaletteID3 = -1;
		}

		public Clothing(string name, string local, eClothingType type, int com1, int draw1, int txd1, int com2, int draw2, int txd2, int com3, int draw3, int txd3, int pal1 = 2, int pal2 = 2, int pal3 = 2)
		{
			Name = name;
			LocalizedName = local;
			Type = type;
			ComponentID = com1;
			DrawableID = draw1;
			TextureID = txd1;
			PaletteID = pal1;
			ComponentID2 = com2;
			DrawableID2 = draw2;
			TextureID2 = txd2;
			PaletteID2 = pal2;
			ComponentID3 = com3;
			DrawableID3 = draw3;
			TextureID3 = txd3;
			PaletteID3 = pal3;
		}

		public string GetLocalizedName()
		{
			string text = null;
			text = ((Operators.CompareString(Name, "", false) != 0) ? Game.GetLocalizedString(Name) : LocalizedName);
			if (string.IsNullOrWhiteSpace(text) || Operators.CompareString(text, "NULL", false) == 0)
			{
				return LocalizedName;
			}
			return text;
		}

		public ClothingSet.CS CS1()
		{
			return new ClothingSet.CS(ComponentID, DrawableID, TextureID, PaletteID);
		}

		public ClothingSet.CS CS2()
		{
			return new ClothingSet.CS(ComponentID2, DrawableID2, TextureID2, PaletteID2);
		}

		public ClothingSet.CS CS3()
		{
			return new ClothingSet.CS(ComponentID3, DrawableID3, TextureID3, PaletteID3);
		}
	}
}
