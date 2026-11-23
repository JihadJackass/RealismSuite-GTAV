using GTA;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII.INM
{
	public class ClothingSet
	{
		public class CS
		{
			public int ComponentID;

			public int DrawableID;

			public int TextureID;

			public int PaletteID;

			public CS(int cid, int did, int tid, int pid = 2)
			{
				ComponentID = cid;
				DrawableID = did;
				TextureID = tid;
				PaletteID = pid;
			}

			public CS()
			{
				ComponentID = -1;
				DrawableID = -1;
				TextureID = -1;
				PaletteID = -1;
			}
		}

		public string Name;

		public string LocalizedName;

		public eClothingType Type;

		public CS Set1;

		public CS Set2;

		public CS Set3;

		public CS Set4;

		public CS Set5;

		public CS Set6;

		public CS Set7;

		public CS Set8;

		public CS Set9;

		public CS Set10;

		public CS Set11;

		public ClothingSet(string name, string local, CS s1, CS s2, CS s3, CS s4, CS s5, CS s6, CS s7)
		{
			Name = name;
			LocalizedName = local;
			Type = eClothingType.FullSuit;
			Set1 = s1;
			Set2 = s2;
			Set3 = s3;
			Set4 = s4;
			Set5 = s5;
			Set6 = s6;
			Set7 = s7;
			Set8 = null;
			Set9 = null;
			Set10 = null;
		}

		public ClothingSet(string name, string local, CS s1, CS s2, CS s3, CS s4, CS s5, CS s6, CS s7, CS s8, CS s9, CS s10)
		{
			Name = name;
			LocalizedName = local;
			Type = eClothingType.Outfit;
			Set1 = s1;
			Set2 = s2;
			Set3 = s3;
			Set4 = s4;
			Set5 = s5;
			Set6 = s6;
			Set7 = s7;
			Set8 = s8;
			Set9 = s9;
			Set10 = s10;
			Set11 = null;
		}

		public ClothingSet(string name, string local, CS s1, CS s2, CS s3, CS s4, CS s5, CS s6, CS s7, CS s8, CS s9, CS s10, CS s11)
		{
			Name = name;
			LocalizedName = local;
			Type = eClothingType.Outfit;
			Set1 = s1;
			Set2 = s2;
			Set3 = s3;
			Set4 = s4;
			Set5 = s5;
			Set6 = s6;
			Set7 = s7;
			Set8 = s8;
			Set9 = s9;
			Set10 = s10;
			Set11 = s11;
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
	}
}
