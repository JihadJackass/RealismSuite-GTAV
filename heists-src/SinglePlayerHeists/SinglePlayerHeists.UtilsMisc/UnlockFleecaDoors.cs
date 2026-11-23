using System;
using GTA;
using GTA.Native;

namespace SinglePlayerHeists.UtilsMisc
{
	internal class UnlockFleecaDoors : Script
	{
		public int Timer;

		public UnlockFleecaDoors()
		{
			base.Tick += Ontick;
		}

		public void Ontick(object sender, EventArgs e)
		{
			if (Game.GameTime >= Timer)
			{
				Timer = Game.GameTime + 1000;
				Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, -348.8109f, -47.26213f, 49.38f, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_genbankdoor2"), 0, 0, 0);
				if (prop != null)
				{
					prop.IsPositionFrozen = false;
				}
				prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, -348.8109f, -47.26213f, 49.38f, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_genbankdoor1"), 0, 0, 0);
				if (prop != null)
				{
					prop.IsPositionFrozen = false;
				}
				prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, -1213.074f, -327.3524f, 38.13f, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_genbankdoor2"), 0, 0, 0);
				if (prop != null)
				{
					prop.IsPositionFrozen = false;
				}
				prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, -1215.386f, -328.5237f, 38.13f, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_genbankdoor1"), 0, 0, 0);
				if (prop != null)
				{
					prop.IsPositionFrozen = false;
				}
				prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 152.0632f, -1038.124f, 29.71f, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_genbankdoor2"), 0, 0, 0);
				if (prop != null)
				{
					prop.IsPositionFrozen = false;
				}
				prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 149.6298f, -1037.231f, 29.71f, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_genbankdoor1"), 0, 0, 0);
				if (prop != null)
				{
					prop.IsPositionFrozen = false;
				}
				prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 316.3925f, -276.4888f, 54.51f, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_genbankdoor2"), 0, 0, 0);
				if (prop != null)
				{
					prop.IsPositionFrozen = false;
				}
				prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 313.9587f, -275.5965f, 54.51f, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_genbankdoor1"), 0, 0, 0);
				if (prop != null)
				{
					prop.IsPositionFrozen = false;
				}
				prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, -2965.71f, 484.2195f, 16f, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_genbankdoor2"), 0, 0, 0);
				if (prop != null)
				{
					prop.IsPositionFrozen = false;
				}
				prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, -2965.821f, 481.6297f, 16f, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_genbankdoor1"), 0, 0, 0);
				if (prop != null)
				{
					prop.IsPositionFrozen = false;
				}
				prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1173.903f, 2703.613f, 38.43f, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_genbankdoor2"), 0, 0, 0);
				if (prop != null)
				{
					prop.IsPositionFrozen = false;
				}
				prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1173.903f, 2703.613f, 38.43f, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_genbankdoor1"), 0, 0, 0);
				if (prop != null)
				{
					prop.IsPositionFrozen = false;
				}
			}
		}
	}
}
