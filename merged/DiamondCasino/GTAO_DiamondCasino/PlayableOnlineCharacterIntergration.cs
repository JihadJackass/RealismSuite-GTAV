using System;
using System.IO;
using System.Reflection;
using GTA;
using GTA.Math;

namespace GTAO_DiamondCasino
{
	internal class PlayableOnlineCharacterIntergration : Script
	{
		public static bool AddedClothesMenu;

		public static Quaternion CasinoChangeClothesLoc = new Quaternion(1096.57f, 200.64f, -50.4f, -129f);

		public PlayableOnlineCharacterIntergration()
		{
			if (_Debug.PlayableOnlineCharacterIntergration != -1)
			{
				base.Tick += OnTick;
			}
		}

		public static void AddClothesMarker(Quaternion Q)
		{
			if (File.Exists("scripts//PlayableOnlineCharacter.dll"))
			{
				AddedClothesMenu = true;
				MethodInfo method = Assembly.LoadFrom("scripts/PlayableOnlineCharacter.dll").GetType("PlayableOnlineCharacter.Class1").GetMethod("AddChangeableClothLocation");
				object[] parameters = new object[1] { Q };
				method.Invoke(method, parameters);
			}
		}

		public static void RemoveClothesMarker(Quaternion Q)
		{
			if (File.Exists("scripts//PlayableOnlineCharacter.dll"))
			{
				MethodInfo method = Assembly.LoadFrom("scripts/PlayableOnlineCharacter.dll").GetType("PlayableOnlineCharacter.Class1").GetMethod("RemoveChangeableClothLocation");
				object[] parameters = new object[1] { Q };
				method.Invoke(method, parameters);
				AddedClothesMenu = false;
			}
		}

		public static void OnTick(object sender, EventArgs e)
		{
			if (!Class1.IsinInterior)
			{
				if (AddedClothesMenu)
				{
					AddedClothesMenu = false;
					RemoveClothesMarker(CasinoChangeClothesLoc);
				}
			}
			else if (!AddedClothesMenu)
			{
				AddedClothesMenu = true;
				AddClothesMarker(CasinoChangeClothesLoc);
			}
		}
	}
}
