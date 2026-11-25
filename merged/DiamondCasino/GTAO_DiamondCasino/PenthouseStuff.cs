using System;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAO_DiamondCasino
{
	internal class PenthouseStuff : Script
	{
		public static bool IsInPenhouse;

		public static bool IsOnPenthouseTerrace;

		public static Quaternion ChangeClothesPos = new Quaternion(975.11f, 65.08f, 115.16f, 146f);

		public static bool AddedClothesPos;

		public PenthouseStuff()
		{
			if (_Debug.PenthouseStuffO != -1)
			{
				base.Tick += OnTick;
			}
		}

		public static void OnTick(object sender, EventArgs e)
		{
			if (!Class1.IsinInterior && Class1.GetDistance_VDIST2(Game.Player.Character.Position, Class1.MarkerEnter, Class1.GetDistSign.Greater, 200f))
			{
				_Debug.PenthouseStuffO = 0;
				return;
			}
			if (Class1.IsinInterior)
			{
				_Debug.PenthouseStuffO = 0;
				return;
			}
			_Debug.PenthouseStuffO = 1;
			IsOnPenthouseTerrace = Zones.PenthouseTerraceZone.Zone.IsWithinFence(Zones.PlayerPoint);
			if (IsOnPenthouseTerrace)
			{
				IsOnPenthouseTerrace = !Class1.IsinInterior && Game.Player.Character.Position.Z > 105f && Class1.GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(949f, 49f, 111f), Class1.GetDistSign.Less, 100f);
			}
			Vector3 position = Game.Player.Character.Position;
			if (Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position.X, position.Y, position.Z) == 274689)
			{
				if (Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, Game.Player.Character) != 0)
				{
					IsInPenhouse = true;
					IsOnPenthouseTerrace = false;
				}
				else
				{
					IsInPenhouse = false;
				}
			}
			if (Class1.Purchased_Penthouse == 0)
			{
				return;
			}
			if (IsInPenhouse)
			{
				if (!AddedClothesPos)
				{
					PlayableOnlineCharacterIntergration.AddClothesMarker(ChangeClothesPos);
					AddedClothesPos = true;
				}
			}
			else if (AddedClothesPos)
			{
				PlayableOnlineCharacterIntergration.RemoveClothesMarker(ChangeClothesPos);
				AddedClothesPos = false;
			}
			if (Game.Player.Character.Position.Z < 105f)
			{
				IsOnPenthouseTerrace = false;
			}
			if (!IsOnPenthouseTerrace)
			{
				bool isInPenhouse = IsInPenhouse;
			}
		}
	}
}
