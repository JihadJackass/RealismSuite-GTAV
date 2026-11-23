using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class ApartmentDrinks
	{
		public static readonly List<Model> Models = new List<Model>
		{
			"apa_mp_h_acc_drink_tray_02", "prop_wine_bot_01", "prop_wine_bot_02", "prop_whiskey_01", "prop_whiskey_bottle", "prop_whiskey_glasses", "prop_cs_whiskey_bottle", "p_whiskey_bottle_s", "prop_vodka_bottle", "prop_beer_bottle",
			"prop_amb_beer_bottle", "prop_drink_redwine", "prop_drink_whtwine", "prop_drink_whisky", "p_champ_flute_s", "apa_p_apa_champ_flute_s", "hei_p_hei_champ_flute_s", "lux_p_champ_flute_s", "prop_food_bs_juice01", "prop_ld_flow_bottle",
			"p_w_grass_gls_s", "prop_wheat_grass_glass", "prop_drink_champ"
		};

		public static bool IsDrinkModel(Model model)
		{
			return Models.Contains(model);
		}

		public static string DrinkKind(Model model)
		{
			if (model == new Model("prop_food_bs_juice01") || model == new Model("p_w_grass_gls_s") || model == new Model("prop_wheat_grass_glass"))
			{
				return "Smoothie";
			}
			if (model == new Model("prop_ld_flow_bottle"))
			{
				return "Water";
			}
			string[] array = new string[2] { "prop_beer_bottle", "prop_amb_beer_bottle" };
			foreach (string name in array)
			{
				if (model == new Model(name))
				{
					return "Beer";
				}
			}
			string[] array2 = new string[7] { "prop_whiskey_01", "prop_whiskey_bottle", "prop_whiskey_glasses", "prop_cs_whiskey_bottle", "p_whiskey_bottle_s", "prop_drink_whisky", "prop_vodka_bottle" };
			foreach (string name2 in array2)
			{
				if (model == new Model(name2))
				{
					return "Whiskey";
				}
			}
			string[] array3 = new string[4] { "prop_wine_bot_01", "prop_wine_bot_02", "prop_drink_redwine", "prop_drink_whtwine" };
			foreach (string name3 in array3)
			{
				if (model == new Model(name3))
				{
					return "Wine";
				}
			}
			return "Champagne";
		}

		public static bool WorldDrinkSourceAllowed(bool modelSupported, bool attached, float distance)
		{
			if (modelSupported && !attached && !float.IsNaN(distance) && !float.IsInfinity(distance) && distance >= 0f)
			{
				return distance <= 1.8f;
			}
			return false;
		}

		public static bool Tick(Prop item)
		{
			if ((object)item == null || !item.Exists() || !IsDrinkModel(item.Model) || (object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsDead || Helper.PP.IsInVehicle() || Helper.MenuPool.IsAnyMenuOpen() || PartyService.IsPlayerActionActive || MansionFeatures.IsWorkoutActive)
			{
				return false;
			}
			if (!WorldDrinkSourceAllowed(true, Function.Call<bool>(Hash.IS_ENTITY_ATTACHED, item.Handle), Helper.PP.Position.DistanceTo(item.Position)))
			{
				return false;
			}
			bool hit = default(bool);
			Vector3 point = default(Vector3);
			Vector3 normal = default(Vector3);
			if (PartyPlacement.PhysicalRay(Helper.PP.Position + new Vector3(0f, 0f, 0.1f), item.Position, 81, ref hit, ref point, ref normal, Helper.PP) != 2)
			{
				return false;
			}
			if (hit && point.DistanceTo(item.Position) > 0.65f)
			{
				return false;
			}
			if (!Helper.ShowPersistentHelp("Press ~INPUT_CONTEXT~ to have a drink. Back cancels the animation."))
			{
				return false;
			}
			if (Game.IsControlJustReleased(Control.Context))
			{
				MansionKitchen.Drink(DrinkKind(item.Model));
			}
			return true;
		}
	}
}
