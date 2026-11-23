using System;
using CayoPericoHeistInSP;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using SinglePlayerHeists.Freeform;

namespace SinglePlayerHeists
{
	public class ManageCayoPerico : Script
	{
		public bool CayoPericoActive;

		public ManageCayoPerico()
		{
			base.Tick += onTick;
			base.Aborted += onShutdown;
		}

		public void LOADIPL(string IPL)
		{
			try
			{
				Function.Call(Hash.REQUEST_IPL, IPL);
			}
			catch
			{
				Screen.ShowSubtitle("~r~IPL CAll OUSIDE MAIN LOOP~w~{" + IPL + "}");
			}
		}

		public static bool GetDistance_VDIST2(Vector3 _1, Vector3 _2, string Sign, float Radius)
		{
			bool result = false;
			float num = Radius * Radius;
			float num2 = Function.Call<float>(Hash.VDIST2, _1.X, _1.Y, _1.Z, _2.X, _2.Y, _2.Z);
			if (Sign.Equals("<=") && num2 <= num)
			{
				result = true;
			}
			if (Sign.Equals("<") && num2 < num)
			{
				result = true;
			}
			if (Sign.Equals(">=") && num2 >= num)
			{
				result = true;
			}
			if (Sign.Equals(">") && num2 > num)
			{
				result = true;
			}
			return result;
		}

		public void onTick(object sender, EventArgs e)
		{
			if (CargoShipHeist.CargoshipHeistOn || CayoPericoHeist.MissionOn)
			{
				if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(5103.374f, -5172.456f, 18f), ">", 2500f) && CayoPericoActive)
				{
					CayoPericoActive = false;
					Function.Call(Hash.SET_USE_ISLAND_MAP, false);
					Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "se_dlc_hei4_island_beach_party_music_new_01_left", false);
					Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "se_dlc_hei4_island_beach_party_music_new_02_right", false);
					Function.Call(Hash.SET_ISLAND_ENABLED, "HeistIsland", false);
					Function.Call(Hash.SET_ALLOW_STREAM_HEIST_ISLAND_NODES, false);
					Function.Call((Hash)15942003382278362576uL, false);
					Function.Call(Hash.SET_USE_ISLAND_MAP, false);
					Function.Call(Hash.REMOVE_IPL, "h4_islandairstrip_doorsclosed");
					Function.Call(Hash.REMOVE_IPL, "h4_islandairstrip_doorsclosed");
					LOADIPL("h4_islandairstrip_doorsclosed");
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(5103.374f, -5172.456f, 18f), "<", 2500f))
				{
					if (CayoPericoActive)
					{
						CayoPericoActive = true;
						Function.Call(Hash.SET_ISLAND_ENABLED, "HeistIsland", true);
						Function.Call(Hash.SET_ALLOW_STREAM_HEIST_ISLAND_NODES, true);
						Function.Call((Hash)15942003382278362576uL, true);
						Function.Call(Hash.SET_MAPDATACULLBOX_ENABLED, "HeistIsland", 0);
					}
					Function.Call(Hash.SET_USE_ISLAND_MAP, true);
				}
			}
			if (TheLexingtonRaid.TheLuxingtonRaidHeistOn)
			{
				Function.Call(Hash.SET_USE_ISLAND_MAP, false);
				Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "se_dlc_hei4_island_beach_party_music_new_01_left", false);
				Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, "se_dlc_hei4_island_beach_party_music_new_02_right", false);
				Function.Call(Hash.SET_ISLAND_ENABLED, "HeistIsland", false);
				Function.Call(Hash.SET_ALLOW_STREAM_HEIST_ISLAND_NODES, false);
				Function.Call((Hash)15942003382278362576uL, false);
				Function.Call(Hash.SET_USE_ISLAND_MAP, false);
				Function.Call(Hash.REMOVE_IPL, "h4_islandairstrip_doorsclosed");
				Function.Call(Hash.REMOVE_IPL, "h4_islandairstrip_doorsclosed");
				LOADIPL("h4_islandairstrip_doorsclosed");
			}
		}

		public void onShutdown(object sender, EventArgs e)
		{
		}
	}
}
