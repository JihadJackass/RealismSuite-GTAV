using System;
using System.Collections.Generic;
using GTA;
using GTA.Native;

namespace GTAO_DiamondCasino
{
	internal class ManageDuplicateTables : Script
	{
		public static List<Prop> CasinoTables = new List<Prop>();

		public static bool RunMethodNow()
		{
			bool result = true;
			if (!Class1.Initialized)
			{
				result = false;
			}
			return result;
		}

		public ManageDuplicateTables()
		{
			if (_Debug.ManageDuplicateTables != -1)
			{
				base.Tick += OnTick;
			}
		}

		public static void OnTick(object sender, EventArgs e)
		{
			if (Class1.Initialized)
			{
				Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1129.4f, 262.39f, -52f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_blckjack_01b"), 0, 0, 0);
				if (prop != null && !CasinoTables.Contains(prop))
				{
					prop.Delete();
				}
				prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1144.5f, 247.24f, -52f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_blckjack_01b"), 0, 0, 0);
				if (prop != null && !CasinoTables.Contains(prop))
				{
					prop.Delete();
				}
				prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1151.84f, 266.747f, -52.84095f, 3f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_blckjack_01"), 0, 0, 0);
				if (prop != null && !CasinoTables.Contains(prop))
				{
					prop.Delete();
				}
				prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1148.837f, 269.747f, -52.84095f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_blckjack_01"), 0, 0, 0);
				if (prop != null && !CasinoTables.Contains(prop))
				{
					prop.Delete();
				}
				prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1134.04f, 266.7f, -52f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_3cardpoker_01b"), 0, 0, 0);
				if (prop != null && !CasinoTables.Contains(prop))
				{
					prop.Delete();
				}
				prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1148.63f, 251.38f, -52f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_3cardpoker_01b"), 0, 0, 0);
				if (prop != null && !CasinoTables.Contains(prop))
				{
					prop.Delete();
				}
				prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1146.226f, 260.99f, -52f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_3cardpoker_01"), 0, 0, 0);
				if (prop != null && !CasinoTables.Contains(prop))
				{
					prop.Delete();
				}
				prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1143.513f, 264.31f, -52f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_3cardpoker_01"), 0, 0, 0);
				if (prop != null && !CasinoTables.Contains(prop))
				{
					prop.Delete();
				}
			}
		}
	}
}
