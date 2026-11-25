using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using GTAO_DiamondCasino.Menus;
using GTAO_DiamondCasino.Penthouse;
using LemonUI;
using LemonUI.Menus;

namespace GTAO_DiamondCasino
{
	public class PenthouseArtPlacement : Script
	{
		public static Vector3 PenthouseUpgradeMarker = new Vector3(983.468f, 66.4218f, 115f);

		public static ObjectPool BusinessMain_ObjectPool;

		public static bool CamActive;

		public static bool InArtDecorator;

		public static Prop CurrentArtProp;

		public static Camera ArtPropCam;

		public static bool RemoveArt;

		public static List<dynamic> PaintProp = new List<object>();

		public static List<dynamic> SculptProp = new List<object>();

		public static List<dynamic> SculptLoc = new List<object>();

		public static List<float> SculptPropRot = new List<float>();

		public static List<dynamic> PaintingLocations = new List<object>
		{
			new Quaternion(983.9474f, 67.5602951f, 116.894218f, -33.650013f),
			new Quaternion(979.395142f, 65.20925f, 116.663887f, 57.6055f),
			new Quaternion(983.975159f, 62.29535f, 116.550209f, -122.082375f),
			new Quaternion(970.8146f, 47.57675f, 116.911957f, 147.9405f),
			new Quaternion(970.420532f, 51.7640038f, 116.906082f, -31.2364311f),
			new Quaternion(965.115234f, 55.0789833f, 116.854538f, -33.48818f),
			new Quaternion(961.608948f, 37.6186676f, 116.87149f, 146.199478f),
			new Quaternion(956.2463f, 40.9692f, 116.931862f, 147.476974f),
			new Quaternion(965.1635f, 28.2467518f, 116.882675f, 148.038284f),
			new Quaternion(971.459839f, 41.7700539f, 117.400879f, -121.984756f),
			new Quaternion(969.7111f, 38.9714546f, 117.401016f, -121.984756f),
			new Quaternion(971.063843f, 43.52286f, 117.421356f, -31.6998615f),
			new Quaternion(956.8553f, 27.7693672f, 117.05732f, 58.0635452f),
			new Quaternion(958.2585f, 30.0149651f, 117.067307f, 57.90997f),
			new Quaternion(956.48114f, 17.1010227f, 117.879417f, -32.0071259f),
			new Quaternion(978.673462f, 70.4864655f, 116.889259f, -31.49545f),
			new Quaternion(974.9607f, 68.6329346f, 116.847649f, 147.995117f),
			new Quaternion(978.620544f, 66.346f, 116.747353f, 147.5853f),
			new Quaternion(975.58606f, 73.04904f, 116.912689f, -31.8556f),
			new Quaternion(976.4873f, 71.3422241f, 116.923454f, -32.2649269f),
			new Quaternion(975.917358f, 74.30386f, 116.800468f, -121.983528f)
		};

		public static NativeMenu ArtPlacementMenu;

		public static List<Prop> ArtProps = new List<Prop>();

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

		public static bool RunMethodNow()
		{
			bool result = true;
			if (!PenthouseStuff.IsInPenhouse)
			{
				_Debug.PenthouseArtPlacement = 0;
				result = false;
			}
			return result;
		}

		public static Model RequestModel(string Prop)
		{
			Model result = new Model(Prop);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid && !result.IsLoaded)
			{
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public static void GrabArtWork()
		{
			PaintProp.Add("vw_prop_vw_wallart_01a");
			PaintProp.Add("vw_prop_vw_wallart_02a");
			PaintProp.Add("vw_prop_vw_wallart_03a");
			PaintProp.Add("vw_prop_vw_wallart_04a");
			PaintProp.Add("vw_prop_vw_wallart_05a");
			PaintProp.Add("vw_prop_vw_wallart_06a");
			PaintProp.Add("vw_prop_vw_wallart_07a");
			PaintProp.Add("vw_prop_vw_wallart_08a");
			PaintProp.Add("vw_prop_vw_wallart_09a");
			PaintProp.Add("vw_prop_vw_wallart_10a");
			PaintProp.Add("vw_prop_vw_wallart_11a");
			PaintProp.Add("vw_prop_vw_wallart_12a");
			PaintProp.Add("vw_prop_vw_wallart_14a");
			PaintProp.Add("vw_prop_vw_wallart_15a");
			PaintProp.Add("vw_prop_vw_wallart_16a");
			PaintProp.Add("vw_prop_vw_wallart_17a");
			PaintProp.Add("vw_prop_vw_wallart_18a");
			PaintProp.Add("vw_prop_vw_wallart_19a");
			PaintProp.Add("vw_prop_vw_wallart_20a");
			PaintProp.Add("vw_prop_vw_wallart_21a");
			PaintProp.Add("vw_prop_vw_wallart_22a");
			PaintProp.Add("vw_prop_vw_wallart_23a");
			PaintProp.Add("vw_prop_vw_wallart_24a");
			PaintProp.Add("vw_prop_vw_wallart_25a");
			PaintProp.Add("vw_prop_vw_wallart_26a");
			PaintProp.Add("vw_prop_vw_wallart_28a");
			PaintProp.Add("vw_prop_vw_wallart_29a");
			PaintProp.Add("vw_prop_vw_wallart_30a");
			PaintProp.Add("vw_prop_vw_wallart_31a");
			PaintProp.Add("vw_prop_vw_wallart_32a");
			PaintProp.Add("vw_prop_vw_wallart_33a");
			PaintProp.Add("vw_prop_vw_wallart_34a");
			PaintProp.Add("vw_prop_vw_wallart_35a");
			PaintProp.Add("vw_prop_vw_wallart_36a");
			PaintProp.Add("vw_prop_vw_wallart_37a");
			PaintProp.Add("vw_prop_vw_wallart_38a");
			PaintProp.Add("vw_prop_vw_wallart_39a");
			PaintProp.Add("vw_prop_vw_wallart_40a");
			PaintProp.Add("vw_prop_vw_wallart_41a");
			PaintProp.Add("vw_prop_vw_wallart_42a");
			PaintProp.Add("vw_prop_vw_wallart_43a");
			PaintProp.Add("vw_prop_vw_wallart_44a");
			PaintProp.Add("vw_prop_vw_wallart_46a");
			PaintProp.Add("vw_prop_vw_wallart_47a");
			PaintProp.Add("vw_prop_vw_wallart_48a");
			PaintProp.Add("vw_prop_vw_wallart_49a");
			PaintProp.Add("vw_prop_vw_wallart_50a");
			PaintProp.Add("vw_prop_vw_wallart_51a");
			PaintProp.Add("vw_prop_vw_wallart_52a");
			PaintProp.Add("vw_prop_vw_wallart_53a");
			PaintProp.Add("vw_prop_vw_wallart_54a_01a");
			PaintProp.Add("vw_prop_vw_wallart_55a");
			PaintProp.Add("vw_prop_vw_wallart_56a");
			PaintProp.Add("vw_prop_vw_wallart_57a");
			PaintProp.Add("vw_prop_vw_wallart_58a");
			PaintProp.Add("vw_prop_vw_wallart_59a");
			PaintProp.Add("vw_prop_vw_wallart_60a");
			PaintProp.Add("vw_prop_vw_wallart_61a");
			PaintProp.Add("vw_prop_vw_wallart_62a");
			PaintProp.Add("vw_prop_vw_wallart_63a");
			PaintProp.Add("vw_prop_vw_wallart_64a");
			PaintProp.Add("vw_prop_vw_wallart_65a");
			PaintProp.Add("vw_prop_vw_wallart_66a");
			PaintProp.Add("vw_prop_vw_wallart_67a");
			PaintProp.Add("vw_prop_vw_wallart_68a");
			PaintProp.Add("vw_prop_vw_wallart_69a");
			PaintProp.Add("vw_prop_vw_wallart_70a");
			PaintProp.Add("vw_prop_vw_wallart_71a");
			PaintProp.Add("vw_prop_vw_wallart_72a");
			PaintProp.Add("vw_prop_vw_wallart_73a");
			PaintProp.Add("vw_prop_vw_wallart_74a");
			PaintProp.Add("vw_prop_vw_wallart_75a");
			PaintProp.Add("vw_prop_vw_wallart_76a");
			PaintProp.Add("vw_prop_vw_wallart_77a");
			PaintProp.Add("vw_prop_vw_wallart_78a");
			PaintProp.Add("vw_prop_vw_wallart_79a");
			PaintProp.Add("vw_prop_vw_wallart_80a");
			PaintProp.Add("vw_prop_vw_wallart_81a");
			PaintProp.Add("vw_prop_vw_wallart_82a");
			PaintProp.Add("vw_prop_vw_wallart_83a");
			PaintProp.Add("vw_prop_vw_wallart_84a");
			PaintProp.Add("vw_prop_vw_wallart_85a");
			PaintProp.Add("vw_prop_vw_wallart_86a");
			PaintProp.Add("vw_prop_vw_wallart_87a");
			PaintProp.Add("vw_prop_vw_wallart_88a");
			PaintProp.Add("vw_prop_vw_wallart_89a");
			PaintProp.Add("vw_prop_vw_wallart_90a");
			PaintProp.Add("vw_prop_vw_wallart_91a");
			PaintProp.Add("vw_prop_vw_wallart_92a");
			PaintProp.Add("vw_prop_vw_wallart_93a");
			PaintProp.Add("vw_prop_vw_wallart_94a");
			PaintProp.Add("vw_prop_vw_wallart_95a");
			PaintProp.Add("vw_prop_vw_wallart_96a");
			PaintProp.Add("vw_prop_vw_wallart_97a");
			PaintProp.Add("vw_prop_vw_wallart_98a");
			PaintProp.Add("vw_prop_vw_wallart_99a");
			PaintProp.Add("vw_prop_vw_wallart_100a");
			PaintProp.Add("vw_prop_vw_wallart_101a");
			PaintProp.Add("vw_prop_vw_wallart_102a");
			PaintProp.Add("vw_prop_vw_wallart_103a");
			PaintProp.Add("vw_prop_vw_wallart_104a");
			PaintProp.Add("vw_prop_vw_wallart_105a");
			PaintProp.Add("vw_prop_vw_wallart_106a");
			PaintProp.Add("vw_prop_vw_wallart_107a");
			PaintProp.Add("vw_prop_vw_wallart_108a");
			PaintProp.Add("vw_prop_vw_wallart_109a");
			PaintProp.Add("vw_prop_vw_wallart_110a");
			PaintProp.Add("vw_prop_vw_wallart_111a");
			PaintProp.Add("vw_prop_vw_wallart_112a");
			PaintProp.Add("vw_prop_vw_wallart_113a");
			PaintProp.Add("vw_prop_vw_wallart_114a");
			PaintProp.Add("vw_prop_vw_wallart_115a");
			PaintProp.Add("vw_prop_vw_wallart_116a");
			PaintProp.Add("vw_prop_vw_wallart_117a");
			PaintProp.Add("vw_prop_vw_wallart_118a");
			PaintProp.Add("vw_prop_vw_wallart_123a");
			PaintProp.Add("vw_prop_vw_wallart_124a");
			PaintProp.Add("vw_prop_vw_wallart_125a");
			PaintProp.Add("vw_prop_vw_wallart_126a");
			PaintProp.Add("vw_prop_vw_wallart_127a");
			PaintProp.Add("vw_prop_vw_wallart_128a");
			PaintProp.Add("vw_prop_vw_wallart_129a");
			PaintProp.Add("vw_prop_vw_wallart_130a");
			PaintProp.Add("vw_prop_vw_wallart_131a");
			PaintProp.Add("vw_prop_vw_wallart_132a");
			PaintProp.Add("vw_prop_vw_wallart_133a");
			PaintProp.Add("vw_prop_vw_wallart_134a");
			PaintProp.Add("vw_prop_vw_wallart_135a");
			PaintProp.Add("vw_prop_vw_wallart_136a");
			PaintProp.Add("vw_prop_vw_wallart_137a");
			PaintProp.Add("vw_prop_vw_wallart_138a");
			PaintProp.Add("vw_prop_vw_wallart_139a");
			PaintProp.Add("vw_prop_vw_wallart_140a");
			PaintProp.Add("vw_prop_vw_wallart_141a");
			PaintProp.Add("vw_prop_vw_wallart_142a");
			PaintProp.Add("vw_prop_vw_wallart_143a");
			PaintProp.Add("vw_prop_vw_wallart_144a");
			PaintProp.Add("vw_prop_vw_wallart_145a");
			PaintProp.Add("vw_prop_vw_wallart_146a");
			PaintProp.Add("vw_prop_vw_wallart_147a");
			PaintProp.Add("vw_prop_vw_wallart_150a");
			PaintProp.Add("vw_prop_vw_wallart_151a");
			PaintProp.Add("vw_prop_vw_wallart_151b");
			PaintProp.Add("vw_prop_vw_wallart_151c");
			PaintProp.Add("vw_prop_vw_wallart_151d");
			PaintProp.Add("vw_prop_vw_wallart_151e");
			PaintProp.Add("vw_prop_vw_wallart_151f");
			PaintProp.Add("vw_prop_vw_wallart_152a");
			PaintProp.Add("vw_prop_vw_wallart_153a");
			PaintProp.Add("vw_prop_vw_wallart_154a");
			PaintProp.Add("vw_prop_vw_wallart_155a");
			PaintProp.Add("vw_prop_vw_wallart_156a");
			PaintProp.Add("vw_prop_vw_wallart_157a");
			PaintProp.Add("vw_prop_vw_wallart_158a");
			PaintProp.Add("vw_prop_vw_wallart_159a");
			PaintProp.Add("vw_prop_vw_wallart_160a");
			PaintProp.Add("vw_prop_vw_wallart_161a");
			PaintProp.Add("vw_prop_vw_wallart_162a");
			PaintProp.Add("vw_prop_vw_wallart_163a");
			PaintProp.Add("vw_prop_vw_wallart_164a");
			PaintProp.Add("vw_prop_vw_wallart_165a");
			PaintProp.Add("vw_prop_vw_wallart_166a");
			PaintProp.Add("vw_prop_vw_wallart_167a");
			PaintProp.Add("vw_prop_vw_wallart_168a");
			PaintProp.Add("vw_prop_vw_wallart_169a");
			PaintProp.Add("vw_prop_vw_wallart_170a");
			PaintProp.Add("vw_prop_vw_wallart_171a");
			PaintProp.Add("vw_prop_vw_wallart_172a");
			PaintProp.Add("vw_prop_vw_wallart_173a");
			PaintProp.Add("vw_prop_vw_wallart_174a");
		}

		public static void GrabArtWork2()
		{
			SculptProp.Add("vw_prop_casino_art_concrete_01a");
			SculptProp.Add("vw_prop_casino_art_concrete_02a");
			SculptProp.Add("vw_prop_casino_art_console_01a");
			SculptProp.Add("vw_prop_casino_art_console_02a");
			SculptProp.Add("vw_prop_casino_art_miniature_05a");
			SculptProp.Add("vw_prop_casino_art_miniature_05b");
			SculptProp.Add("vw_prop_casino_art_miniature_05c");
			SculptProp.Add("vw_prop_casino_art_miniature_09a");
			SculptProp.Add("vw_prop_casino_art_miniature_09b");
			SculptProp.Add("vw_prop_casino_art_miniature_09c");
			SculptProp.Add("vw_prop_casino_art_sculpture_01a");
			SculptProp.Add("vw_prop_casino_art_sculpture_02a");
			SculptProp.Add("vw_prop_casino_art_sculpture_02b");
			SculptProp.Add("vw_prop_casino_art_skull_01a");
			SculptProp.Add("vw_prop_casino_art_skull_01b");
			SculptProp.Add("vw_prop_casino_art_skull_02a");
			SculptProp.Add("vw_prop_casino_art_skull_02b");
			SculptProp.Add("vw_prop_casino_art_skull_03a");
			SculptProp.Add("vw_prop_casino_art_skull_03b");
			SculptProp.Add("vw_prop_casino_art_statue_01a");
			SculptProp.Add("vw_prop_casino_art_statue_02a");
			SculptProp.Add("vw_prop_casino_art_bird_01a");
			SculptProp.Add("vw_prop_casino_art_car_01a");
			SculptProp.Add("vw_prop_casino_art_car_02a");
			SculptProp.Add("vw_prop_casino_art_car_03a");
			SculptProp.Add("vw_prop_casino_art_car_04a");
			SculptProp.Add("vw_prop_casino_art_car_05a");
			SculptProp.Add("vw_prop_casino_art_car_06a");
			SculptProp.Add("vw_prop_casino_art_car_07a");
			SculptProp.Add("vw_prop_casino_art_car_08a");
			SculptProp.Add("vw_prop_casino_art_car_09a");
			SculptProp.Add("vw_prop_casino_art_car_10a");
			SculptProp.Add("vw_prop_casino_art_car_11a");
			SculptProp.Add("vw_prop_casino_art_car_12a");
			SculptProp.Add("vw_prop_casino_art_cherries_01a");
			SculptProp.Add("vw_prop_casino_art_deer_01a");
			SculptProp.Add("vw_prop_casino_art_dog_01a");
			SculptProp.Add("vw_prop_casino_art_egg_01a");
			SculptProp.Add("vw_prop_casino_art_figurines_01a");
			SculptProp.Add("vw_prop_casino_art_figurines_02a");
			SculptProp.Add("vw_prop_casino_art_grenade_01a");
			SculptProp.Add("vw_prop_casino_art_grenade_01b");
			SculptProp.Add("vw_prop_casino_art_grenade_01c");
			SculptProp.Add("vw_prop_casino_art_grenade_01d");
			SculptProp.Add("vw_prop_casino_art_guitar_01a");
			SculptProp.Add("vw_prop_casino_art_gun_01a");
			SculptProp.Add("vw_prop_casino_art_gun_02a");
			SculptProp.Add("vw_prop_casino_art_head_01a");
			SculptProp.Add("vw_prop_casino_art_head_01b");
			SculptProp.Add("vw_prop_casino_art_head_01c");
			SculptProp.Add("vw_prop_casino_art_head_01d");
			SculptProp.Add("vw_prop_casino_art_lampf_01a");
			SculptProp.Add("vw_prop_casino_art_lampm_01a");
			SculptProp.Add("vw_prop_casino_art_lollipop_01a");
			SculptProp.Add("vw_prop_casino_art_mod_01a");
			SculptProp.Add("vw_prop_casino_art_mod_02a");
			SculptProp.Add("vw_prop_casino_art_mod_03a");
			SculptProp.Add("vw_prop_casino_art_mod_03a_a");
			SculptProp.Add("vw_prop_casino_art_mod_03a_b");
			SculptProp.Add("vw_prop_casino_art_mod_03a_c");
			SculptProp.Add("vw_prop_casino_art_mod_03b");
			SculptProp.Add("vw_prop_casino_art_mod_03b_a");
			SculptProp.Add("vw_prop_casino_art_mod_03b_b");
			SculptProp.Add("vw_prop_casino_art_mod_03b_c");
			SculptProp.Add("vw_prop_casino_art_mod_04a");
			SculptProp.Add("vw_prop_casino_art_mod_04b");
			SculptProp.Add("vw_prop_casino_art_mod_04c");
			SculptProp.Add("vw_prop_casino_art_mod_05a");
			SculptProp.Add("vw_prop_casino_art_mod_06a");
			SculptProp.Add("vw_prop_casino_art_pill_01a");
			SculptProp.Add("vw_prop_casino_art_pill_01b");
			SculptProp.Add("vw_prop_casino_art_pill_01c");
			SculptProp.Add("vw_prop_casino_art_plant_01a");
			SculptProp.Add("vw_prop_casino_art_plant_02a");
			SculptProp.Add("vw_prop_casino_art_plant_03a");
			SculptProp.Add("vw_prop_casino_art_plant_04a");
			SculptProp.Add("vw_prop_casino_art_plant_05a");
			SculptProp.Add("vw_prop_casino_art_plant_06a");
			SculptProp.Add("vw_prop_casino_art_plant_07a");
			SculptProp.Add("vw_prop_casino_art_plant_08a");
			SculptProp.Add("vw_prop_casino_art_plant_09a");
			SculptProp.Add("vw_prop_casino_art_plant_10a");
			SculptProp.Add("vw_prop_casino_art_plant_11a");
			SculptProp.Add("vw_prop_casino_art_plant_12a");
			SculptProp.Add("vw_prop_casino_art_sh_01a");
			SculptProp.Add("vw_prop_casino_art_statue_04a");
			SculptProp.Add("vw_prop_casino_art_vase_01a");
			SculptProp.Add("vw_prop_casino_art_vase_02a");
			SculptProp.Add("vw_prop_casino_art_vase_03a");
			SculptProp.Add("vw_prop_casino_art_vase_04a");
			SculptProp.Add("vw_prop_casino_art_vase_05a");
			SculptProp.Add("vw_prop_casino_art_vase_06a");
			SculptProp.Add("vw_prop_casino_art_vase_07a");
			SculptProp.Add("vw_prop_casino_art_vase_08a");
			SculptProp.Add("vw_prop_casino_art_vase_09a");
			SculptProp.Add("vw_prop_casino_art_vase_10a");
			SculptProp.Add("vw_prop_casino_art_vase_11a");
			SculptProp.Add("vw_prop_casino_art_vase_12a");
			SculptProp.Add("vw_prop_vw_plant_int_03a");
			SculptProp.Add("vw_prop_art_football_01a");
			SculptProp.Add("vw_prop_art_mic_01a");
			SculptProp.Add("vw_prop_art_pug_01a");
			SculptProp.Add("vw_prop_art_pug_01b");
			SculptProp.Add("vw_prop_art_pug_02a");
			SculptProp.Add("vw_prop_art_pug_02b");
			SculptProp.Add("vw_prop_art_pug_03a");
			SculptProp.Add("vw_prop_art_pug_03b");
			SculptProp.Add("vw_prop_art_resin_balls_01a");
			SculptProp.Add("vw_prop_art_resin_guns_01a");
			SculptProp.Add("vw_prop_art_wall_segment_01a");
			SculptProp.Add("vw_prop_art_wall_segment_02a");
			SculptProp.Add("vw_prop_art_wall_segment_02b");
			SculptProp.Add("vw_prop_art_wall_segment_03a");
			SculptProp.Add("vw_prop_art_wings_01a");
			SculptProp.Add("vw_prop_art_wings_01b");
			SculptProp.Add("vw_prop_book_stack_01a");
			SculptProp.Add("vw_prop_book_stack_01b");
			SculptProp.Add("vw_prop_book_stack_01c");
			SculptProp.Add("vw_prop_book_stack_02a");
			SculptProp.Add("vw_prop_book_stack_02b");
			SculptProp.Add("vw_prop_book_stack_02c");
			SculptProp.Add("vw_prop_book_stack_03a");
			SculptProp.Add("vw_prop_book_stack_03b");
			SculptProp.Add("vw_prop_book_stack_03c");
			SculptProp.Add("vw_prop_casino_art_basketball_01a");
			SculptProp.Add("vw_prop_casino_art_basketball_02a");
			SculptProp.Add("vw_prop_flowers_potted_01a");
			SculptProp.Add("vw_prop_flowers_potted_02a");
			SculptProp.Add("vw_prop_flowers_potted_03a");
			SculptProp.Add("vw_prop_flowers_vase_01a");
			SculptProp.Add("vw_prop_flowers_vase_02a");
			SculptProp.Add("vw_prop_flowers_vase_03a");
			SculptProp.Add("vw_prop_miniature_yacht_01a");
			SculptProp.Add("vw_prop_miniature_yacht_01b");
			SculptProp.Add("vw_prop_miniature_yacht_01c");
			SculptProp.Add("vw_prop_plaque_01a");
			SculptProp.Add("vw_prop_plaque_02a");
			SculptProp.Add("vw_prop_plaque_02b");
			SculptProp.Add("vw_prop_toy_sculpture_01a");
			SculptProp.Add("vw_prop_toy_sculpture_02a");
			SculptProp.Add("vw_prop_vw_pogo_gold_01a");
			SculptProp.Add("vw_prop_casino_art_absman_01a");
			SculptProp.Add("vw_prop_casino_art_bottle_01a");
			SculptProp.Add("vw_prop_casino_art_bowling_01a");
			SculptProp.Add("vw_prop_casino_art_bowling_01b");
			SculptProp.Add("vw_prop_casino_art_bowling_02a");
			SculptProp.Add("vw_prop_casino_art_ego_01a");
			SculptProp.Add("vw_prop_casino_art_horse_01a");
			SculptProp.Add("vw_prop_casino_art_horse_01b");
			SculptProp.Add("vw_prop_casino_art_horse_01c");
			SculptProp.Add("vw_prop_casino_art_panther_01a");
			SculptProp.Add("vw_prop_casino_art_panther_01b");
			SculptProp.Add("vw_prop_casino_art_panther_01c");
			SculptProp.Add("vw_prop_casino_art_rocket_01a");
			SculptProp.Add("vw_prop_casino_art_v_01a");
			SculptProp.Add("vw_prop_casino_art_v_01b");
			SculptProp.Add("ba_prop_battle_champ_closed");
			SculptProp.Add("ba_prop_battle_champ_closed_02");
			SculptProp.Add("ba_prop_battle_champ_closed_03");
			SculptProp.Add("ba_prop_battle_champ_open");
			SculptProp.Add("ba_prop_battle_champ_open_02");
			SculptProp.Add("ba_prop_battle_champ_open_03");
			SculptProp.Add("ba_prop_battle_cuffs");
			SculptProp.Add("ba_prop_battle_trophy_battler");
			SculptProp.Add("ba_prop_battle_trophy_dancer");
			SculptProp.Add("ba_prop_battle_trophy_no1");
			SculptProp.Add("xs_prop_arena_champ_closed");
			SculptProp.Add("xs_prop_arena_champ_open");
			SculptProp.Add("xs_prop_arena_trophy_double_01a");
			SculptProp.Add("xs_prop_arena_trophy_double_01b");
			SculptProp.Add("xs_prop_arena_trophy_double_01c");
			SculptProp.Add("xs_prop_arena_trophy_single_01a");
			SculptProp.Add("xs_prop_arena_trophy_single_01b");
			SculptProp.Add("xs_prop_arena_trophy_single_01c");
			SculptProp.Add("xs_prop_trophy_bandito_01a");
			SculptProp.Add("xs_prop_trophy_carfire_01a");
			SculptProp.Add("xs_prop_trophy_carstack_01a");
			SculptProp.Add("xs_prop_trophy_champ_01a");
			SculptProp.Add("xs_prop_trophy_cup_01a");
			SculptProp.Add("xs_prop_trophy_drone_01a");
			SculptProp.Add("xs_prop_trophy_firepit_01a");
			SculptProp.Add("xs_prop_trophy_flags_01a");
			SculptProp.Add("xs_prop_trophy_flipper_01a");
			SculptProp.Add("xs_prop_trophy_goldbag_01a");
			SculptProp.Add("xs_prop_trophy_imperator_01a");
			SculptProp.Add("xs_prop_trophy_mines_01a");
			SculptProp.Add("xs_prop_trophy_pegasus_01a");
			SculptProp.Add("xs_prop_trophy_presents_01a");
			SculptProp.Add("xs_prop_trophy_rc_01a");
			SculptProp.Add("xs_prop_trophy_shunt_01a");
			SculptProp.Add("xs_prop_trophy_spinner_01a");
			SculptProp.Add("xs_prop_trophy_telescope_01a");
			SculptProp.Add("xs_prop_trophy_tower_01a");
			SculptProp.Add("xs_prop_trophy_wrench_01a");
			SculptProp.Add("p_ld_heist_bag_s_1");
			SculptProp.Add("prop_gold_bar");
			SculptProp.Add("prop_jewel_02a");
			SculptProp.Add("prop_jewel_02b");
			SculptProp.Add("prop_jewel_02c");
			SculptProp.Add("prop_jewel_03a");
			SculptProp.Add("prop_jewel_03b");
			SculptProp.Add("prop_jewel_04a");
			SculptProp.Add("prop_jewel_04b");
			SculptProp.Add("p_watch_01");
			SculptProp.Add("p_watch_02");
			SculptProp.Add("p_watch_03");
			SculptProp.Add("p_watch_04");
			SculptProp.Add("p_watch_05");
			SculptProp.Add("p_watch_06");
			SculptProp.Add("prop_idol_01_error");
			SculptProp.Add("ex_prop_exec_award_bronze");
			SculptProp.Add("ex_prop_exec_award_diamond");
			SculptProp.Add("ex_prop_exec_award_gold");
			SculptProp.Add("ex_prop_exec_award_plastic");
			SculptProp.Add("ex_prop_exec_award_silver");
			SculptProp.Add("ex_cash_pile_01");
			SculptProp.Add("vw_prop_casino_cards_01");
			SculptProp.Add("vw_prop_casino_cards_02");
			SculptProp.Add("vw_prop_casino_cards_single");
			SculptProp.Add("vw_prop_cas_card_club_02");
			SculptProp.Add("vw_prop_cas_card_club_03");
			SculptProp.Add("vw_prop_cas_card_club_04");
			SculptProp.Add("vw_prop_cas_card_club_05");
			SculptProp.Add("vw_prop_cas_card_club_06");
			SculptProp.Add("vw_prop_cas_card_club_07");
			SculptProp.Add("vw_prop_cas_card_club_08");
			SculptProp.Add("vw_prop_cas_card_club_ace");
			SculptProp.Add("vw_prop_cas_card_club_jack");
			SculptProp.Add("vw_prop_cas_card_club_king");
			SculptProp.Add("vw_prop_cas_card_club_queen");
			SculptProp.Add("vw_prop_cas_card_dia_02");
			SculptProp.Add("vw_prop_cas_card_dia_03");
			SculptProp.Add("vw_prop_cas_card_dia_04");
			SculptProp.Add("vw_prop_cas_card_dia_05");
			SculptProp.Add("vw_prop_cas_card_dia_06");
			SculptProp.Add("vw_prop_cas_card_dia_07");
			SculptProp.Add("vw_prop_cas_card_dia_08");
			SculptProp.Add("vw_prop_cas_card_dia_09");
			SculptProp.Add("vw_prop_cas_card_dia_10");
			SculptProp.Add("vw_prop_cas_card_dia_ace");
			SculptProp.Add("vw_prop_cas_card_dia_jack");
			SculptProp.Add("vw_prop_cas_card_dia_king");
			SculptProp.Add("vw_prop_cas_card_dia_queen");
			SculptProp.Add("vw_prop_cas_card_hrt_02");
			SculptProp.Add("vw_prop_cas_card_hrt_03");
			SculptProp.Add("vw_prop_cas_card_hrt_04");
			SculptProp.Add("vw_prop_cas_card_hrt_05");
			SculptProp.Add("vw_prop_cas_card_hrt_06");
			SculptProp.Add("vw_prop_cas_card_hrt_07");
			SculptProp.Add("vw_prop_cas_card_hrt_08");
			SculptProp.Add("vw_prop_cas_card_hrt_09");
			SculptProp.Add("vw_prop_cas_card_hrt_10");
			SculptProp.Add("vw_prop_cas_card_hrt_ace");
			SculptProp.Add("vw_prop_cas_card_hrt_jack");
			SculptProp.Add("vw_prop_cas_card_hrt_king");
			SculptProp.Add("vw_prop_cas_card_hrt_queen");
			SculptProp.Add("vw_prop_cas_card_spd_02");
			SculptProp.Add("vw_prop_cas_card_spd_03");
			SculptProp.Add("vw_prop_cas_card_spd_04");
			SculptProp.Add("vw_prop_cas_card_spd_05");
			SculptProp.Add("vw_prop_cas_card_spd_06");
			SculptProp.Add("vw_prop_cas_card_spd_07");
			SculptProp.Add("vw_prop_cas_card_spd_08");
			SculptProp.Add("vw_prop_cas_card_spd_09");
			SculptProp.Add("vw_prop_cas_card_spd_10");
			SculptProp.Add("vw_prop_cas_card_spd_ace");
			SculptProp.Add("vw_prop_cas_card_spd_jack");
			SculptProp.Add("vw_prop_cas_card_spd_king");
			SculptProp.Add("vw_prop_cas_card_spd_queen");
			SculptProp.Add("vw_prop_vw_3card_01a");
			SculptProp.Add("vw_prop_vw_card_case_01a");
			SculptProp.Add("vw_prop_vw_casino_cards_01");
			SculptProp.Add("vw_prop_vw_club_char_02a");
			SculptProp.Add("vw_prop_vw_club_char_03a");
			SculptProp.Add("vw_prop_vw_club_char_04a");
			SculptProp.Add("vw_prop_vw_club_char_05a");
			SculptProp.Add("vw_prop_vw_club_char_06a");
			SculptProp.Add("vw_prop_vw_club_char_07a");
			SculptProp.Add("vw_prop_vw_club_char_08a");
			SculptProp.Add("vw_prop_vw_club_char_09a");
			SculptProp.Add("vw_prop_vw_club_char_10a");
			SculptProp.Add("vw_prop_vw_club_char_a_a");
			SculptProp.Add("vw_prop_vw_club_char_j_a");
			SculptProp.Add("vw_prop_vw_club_char_k_a");
			SculptProp.Add("vw_prop_vw_club_char_q_a");
			SculptProp.Add("vw_prop_vw_dia_char_02a");
			SculptProp.Add("vw_prop_vw_dia_char_03a");
			SculptProp.Add("vw_prop_vw_dia_char_04a");
			SculptProp.Add("vw_prop_vw_dia_char_05a");
			SculptProp.Add("vw_prop_vw_dia_char_06a");
			SculptProp.Add("vw_prop_vw_dia_char_07a");
			SculptProp.Add("vw_prop_vw_dia_char_08a");
			SculptProp.Add("vw_prop_vw_dia_char_09a");
			SculptProp.Add("vw_prop_vw_dia_char_10a");
			SculptProp.Add("vw_prop_vw_dia_char_a_a");
			SculptProp.Add("vw_prop_vw_dia_char_j_a");
			SculptProp.Add("vw_prop_vw_dia_char_k_a");
			SculptProp.Add("vw_prop_vw_dia_char_q_a");
			SculptProp.Add("vw_prop_vw_hrt_char_02a");
			SculptProp.Add("vw_prop_vw_hrt_char_03a");
			SculptProp.Add("vw_prop_vw_hrt_char_04a");
			SculptProp.Add("vw_prop_vw_hrt_char_05a");
			SculptProp.Add("vw_prop_vw_hrt_char_06a");
			SculptProp.Add("vw_prop_vw_hrt_char_07a");
			SculptProp.Add("vw_prop_vw_hrt_char_08a");
			SculptProp.Add("vw_prop_vw_hrt_char_09a");
			SculptProp.Add("vw_prop_vw_hrt_char_10a");
			SculptProp.Add("vw_prop_vw_hrt_char_a_a");
			SculptProp.Add("vw_prop_vw_hrt_char_j_a");
			SculptProp.Add("vw_prop_vw_hrt_char_k_a");
			SculptProp.Add("vw_prop_vw_hrt_char_q_a");
			SculptProp.Add("vw_prop_vw_jo_char_01a");
			SculptProp.Add("vw_prop_vw_jo_char_02a");
			SculptProp.Add("vw_prop_vw_key_card_01a");
			SculptProp.Add("vw_prop_vw_lux_card_01a");
			SculptProp.Add("vw_prop_vw_spd_char_02a");
			SculptProp.Add("vw_prop_vw_spd_char_03a");
			SculptProp.Add("vw_prop_vw_spd_char_04a");
			SculptProp.Add("vw_prop_vw_spd_char_05a");
			SculptProp.Add("vw_prop_vw_spd_char_06a");
			SculptProp.Add("vw_prop_vw_spd_char_07a");
			SculptProp.Add("vw_prop_vw_spd_char_08a");
			SculptProp.Add("vw_prop_vw_spd_char_09a");
			SculptProp.Add("vw_prop_vw_spd_char_10a");
			SculptProp.Add("vw_prop_vw_spd_char_a_a");
			SculptProp.Add("vw_prop_vw_spd_char_j_a");
			SculptProp.Add("vw_prop_vw_spd_char_k_a");
			SculptProp.Add("vw_prop_vw_spd_char_q_a");
			SculptProp.Add("vw_prop_chip_1kdollar_st");
			SculptProp.Add("vw_prop_chip_1kdollar_x1");
			SculptProp.Add("vw_prop_chip_5kdollar_st");
			SculptProp.Add("vw_prop_chip_5kdollar_x1");
			SculptProp.Add("vw_prop_chip_10dollar_st");
			SculptProp.Add("vw_prop_chip_10dollar_x1");
			SculptProp.Add("vw_prop_chip_10kdollar_st");
			SculptProp.Add("vw_prop_chip_10kdollar_x1");
			SculptProp.Add("vw_prop_chip_50dollar_st");
			SculptProp.Add("vw_prop_chip_50dollar_x1");
			SculptProp.Add("vw_prop_chip_100dollar_st");
			SculptProp.Add("vw_prop_chip_100dollar_x1");
			SculptProp.Add("vw_prop_chip_500dollar_st");
			SculptProp.Add("vw_prop_chip_500dollar_x1");
			SculptProp.Add("vw_prop_plaq_1kdollar_x1");
			SculptProp.Add("vw_prop_plaq_5kdollar_st");
			SculptProp.Add("vw_prop_plaq_5kdollar_x1");
			SculptProp.Add("vw_prop_plaq_10kdollar_st");
			SculptProp.Add("vw_prop_plaq_10kdollar_x1");
			SculptProp.Add("vw_prop_vw_chips_pile_01a");
			SculptProp.Add("vw_prop_vw_chips_pile_02a");
			SculptProp.Add("vw_prop_vw_chips_pile_03a");
			SculptProp.Add("vw_prop_vw_coin_01a");
			SculptProp.Add("vw_prop_casino_champset");
			SculptProp.Add("vw_prop_vw_colle_alien");
			SculptProp.Add("vw_prop_vw_colle_beast");
			SculptProp.Add("vw_prop_vw_colle_imporage");
			SculptProp.Add("vw_prop_vw_colle_pogo");
			SculptProp.Add("vw_prop_vw_colle_prbubble");
			SculptProp.Add("vw_prop_vw_colle_rsrcomm");
			SculptProp.Add("vw_prop_vw_colle_rsrgeneric");
			SculptProp.Add("vw_prop_vw_colle_sasquatch");
			SculptProp.Add("p_gcase_s");
			SculptProp.Add("p_laptop_02_s");
			SculptProp.Add("p_mr_raspberry_01_s");
			SculptProp.Add("vw_prop_vw_v_blueprt_01a");
			SculptProp.Add("prop_acc_guitar_01");
			SculptProp.Add("prop_acc_guitar_01_d1");
			SculptProp.Add("vw_prop_vw_chip_carrier_01a");
			SculptProp.Add("apa_mp_h_acc_bottle_01");
			SculptProp.Add("apa_mp_h_acc_bottle_02");
			SculptProp.Add("apa_mp_h_acc_bowl_ceramic_01");
			SculptProp.Add("apa_mp_h_acc_box_trinket_01");
			SculptProp.Add("apa_mp_h_acc_box_trinket_02");
			SculptProp.Add("apa_mp_h_acc_candles_01");
			SculptProp.Add("apa_mp_h_acc_candles_02");
			SculptProp.Add("apa_mp_h_acc_candles_04");
			SculptProp.Add("apa_mp_h_acc_candles_05");
			SculptProp.Add("apa_mp_h_acc_candles_06");
			SculptProp.Add("apa_mp_h_acc_coffeemachine_01");
			SculptProp.Add("apa_mp_h_acc_dec_head_01");
			SculptProp.Add("apa_mp_h_acc_dec_plate_01");
			SculptProp.Add("apa_mp_h_acc_dec_plate_02");
			SculptProp.Add("apa_mp_h_acc_dec_sculpt_01");
			SculptProp.Add("apa_mp_h_acc_dec_sculpt_02");
			SculptProp.Add("apa_mp_h_acc_dec_sculpt_03");
			SculptProp.Add("apa_mp_h_acc_drink_tray_02");
			SculptProp.Add("apa_mp_h_acc_fruitbowl_01");
			SculptProp.Add("apa_mp_h_acc_fruitbowl_02");
			SculptProp.Add("apa_mp_h_acc_jar_02");
			SculptProp.Add("apa_mp_h_acc_jar_03");
			SculptProp.Add("apa_mp_h_acc_jar_04");
			SculptProp.Add("apa_mp_h_acc_phone_01");
			SculptProp.Add("apa_mp_h_acc_plant_palm_01");
			SculptProp.Add("apa_mp_h_acc_plant_tall_01");
			SculptProp.Add("apa_mp_h_acc_pot_pouri_01");
			SculptProp.Add("apa_mp_h_acc_scent_sticks_01");
			SculptProp.Add("apa_mp_h_acc_tray_01");
			SculptProp.Add("apa_mp_h_acc_vase_01");
			SculptProp.Add("apa_mp_h_acc_vase_02");
			SculptProp.Add("apa_mp_h_acc_vase_04");
			SculptProp.Add("apa_mp_h_acc_vase_05");
			SculptProp.Add("apa_mp_h_acc_vase_06");
			SculptProp.Add("apa_mp_h_acc_vase_flowers_01");
			SculptProp.Add("apa_mp_h_acc_vase_flowers_02");
			SculptProp.Add("apa_mp_h_acc_vase_flowers_03");
			SculptProp.Add("apa_mp_h_acc_vase_flowers_04");
		}

		public static void SetupArtMenu()
		{
			SculptLoc.Add(new Vector3(971.4469f, 71.12376f, 116.2449f));
			SculptLoc.Add(new Vector3(973.4052f, 69.97559f, 116.2449f));
			SculptLoc.Add(new Vector3(972.6075f, 70.53499f, 116.2449f));
			SculptLoc.Add(new Vector3(968.3544f, 72.30743f, 115.6902f));
			SculptLoc.Add(new Vector3(976.5177f, 80.23362f, 116.1382f));
			SculptLoc.Add(new Vector3(969.4531f, 77.62489f, 115.6971f));
			SculptLoc.Add(new Vector3(976.4003f, 80.65188f, 116.1382f));
			SculptLoc.Add(new Vector3(977.8342f, 70.76744f, 116.1382f));
			SculptLoc.Add(new Vector3(971.9137f, 81.98214f, 116.1122f));
			SculptLoc.Add(new Vector3(972.9839f, 75.56157f, 115.6972f));
			SculptLoc.Add(new Vector3(983.1158f, 67.49692f, 116.0922f));
			SculptLoc.Add(new Vector3(984.2914f, 66.72982f, 116.0922f));
			SculptLoc.Add(new Vector3(974.4702f, 41.89836f, 116.0019f));
			SculptLoc.Add(new Vector3(966.2922f, 42.10915f, 116.1383f));
			SculptLoc.Add(new Vector3(974.5083f, 63.8875f, 115.6616f));
			SculptLoc.Add(new Vector3(965.8897f, 41.60423f, 116.1383f));
			SculptLoc.Add(new Vector3(964.7979f, 39.74314f, 116.1482f));
			SculptLoc.Add(new Vector3(966.9099f, 43.01525f, 115.8882f));
			SculptLoc.Add(new Vector3(965.3593f, 39.1421f, 116.1482f));
			SculptLoc.Add(new Vector3(974.4998f, 49.53851f, 116.1383f));
			SculptLoc.Add(new Vector3(978.0218f, 46.56517f, 115.9923f));
			SculptLoc.Add(new Vector3(976.4994f, 46.86512f, 115.9922f));
			SculptLoc.Add(new Vector3(976.5798f, 46.46362f, 115.9922f));
			SculptLoc.Add(new Vector3(976.8693f, 46.68201f, 115.9922f));
			SculptLoc.Add(new Vector3(979.0563f, 69.87373f, 116.1382f));
			SculptLoc.Add(new Vector3(945.124f, 14.89447f, 116.2324f));
			SculptLoc.Add(new Vector3(948.8206f, 17.97805f, 116.2324f));
			SculptLoc.Add(new Vector3(940.6143f, 10.60696f, 116.0452f));
			SculptLoc.Add(new Vector3(947.098f, 17.37584f, 116.2324f));
			SculptLoc.Add(new Vector3(946.713f, 17.43841f, 116.2324f));
			SculptLoc.Add(new Vector3(947.0009f, 17.54459f, 116.2324f));
			SculptLoc.Add(new Vector3(944.1304f, 13.74309f, 116.2324f));
			SculptLoc.Add(new Vector3(946.6449f, 17.10868f, 116.2324f));
			SculptLoc.Add(new Vector3(954.0463f, 32.15368f, 115.7886f));
			SculptLoc.Add(new Vector3(956.1825f, 39.25602f, 115.9771f));
			SculptLoc.Add(new Vector3(953.3879f, 31.41343f, 116.3413f));
			SculptLoc.Add(new Vector3(947.5201f, 25.55083f, 115.6968f));
			SculptLoc.Add(new Vector3(966.8933f, 35.24205f, 116.0023f));
			SculptLoc.Add(new Vector3(968.6607f, 36.26171f, 116.1769f));
			SculptLoc.Add(new Vector3(989.285f, 66.46245f, 116.1385f));
			SculptLoc.Add(new Vector3(989.1791f, 65.0499f, 116.5328f));
			SculptLoc.Add(new Vector3(989.5279f, 66.66537f, 116.1352f));
			SculptLoc.Add(new Vector3(989.285f, 66.46245f, 116.1385f));
			SculptLoc.Add(new Vector3(984.8004f, 68.36877f, 116.1383f));
			SculptLoc.Add(new Vector3(961.1185f, 42.7681f, 115.7886f));
			SculptLoc.Add(new Vector3(960.9095f, 47.88779f, 115.7886f));
			SculptLoc.Add(new Vector3(970.9131f, 41.73798f, 116.1388f));
			SculptLoc.Add(new Vector3(989.5279f, 66.66537f, 116.1352f));
			SculptLoc.Add(new Vector3(970.9131f, 41.73798f, 116.1388f));
		}

		public PenthouseArtPlacement()
		{
			base.Tick += OnTick;
		}

		public static Vector3 GetDimensions(Entity E)
		{
			ValueTuple<Vector3, Vector3> dimensions = E.Model.Dimensions;
			Vector3 item = dimensions.Item1;
			Vector3 item2 = dimensions.Item2;
			float x = item2.X - item.X;
			float y = item2.Y - item.Y;
			float z = item2.Z - item.Z;
			return new Vector3(x, y, z);
		}

		public unsafe static Vector3 GetDimensions(string Model)
		{
			Vector3 zero = Vector3.Zero;
			Vector3 zero2 = Vector3.Zero;
			Function.Call(Hash.GET_MODEL_DIMENSIONS, Function.Call<int>(Hash.GET_HASH_KEY, Model), &zero, &zero2);
			float x = zero2.X - zero.X;
			float y = zero2.Y - zero.Y;
			float z = zero2.Z - zero.Z;
			return new Vector3(x, y, z);
		}

		public static void OnTick(object sender, EventArgs e)
		{
			if (!PenthouseStuff.IsInPenhouse)
			{
				_Debug.PenthouseArtPlacement = 0;
				return;
			}
			_Debug.PenthouseArtPlacement = 1;
			if (BusinessMain_ObjectPool != null)
			{
				BusinessMain_ObjectPool.Process();
			}
			if (BusinessMain_ObjectPool != null && BusinessMain_ObjectPool.AreAnyVisible && PurchasePenthouse.submenu0 != null && !PurchasePenthouse.PenthouseArt.Visible && !PurchasePenthouse.submenu16s.Visible && !PurchasePenthouse.submenu0.Visible)
			{
				Function.Call(Hash.SET_FAKE_PAUSEMAP_PLAYER_POSITION_THIS_FRAME, (int)EnterExitPenthouseFromTerrace.PenthouseMainEnter.X, (int)EnterExitPenthouseFromTerrace.PenthouseMainEnter.Y);
				Function.Call(Hash.SET_RADAR_ZOOM, 400);
				if (!InArtDecorator)
				{
					if (CurrentArtProp != null)
					{
						CurrentArtProp.Delete();
					}
					Game.Player.Character.IsVisible = false;
					CamActive = false;
					if (ArtPropCam != null)
					{
						InArtDecorator = false;
						Game.Player.Character.IsVisible = true;
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						ArtPropCam.IsActive = false;
						ArtPropCam.Delete();
					}
					Notification.Show("Create Camera");
					if (CurrentArtProp != null)
					{
						CurrentArtProp.Delete();
					}
					Vector3 vector = new Vector3(PaintingLocations[0].X, PaintingLocations[0].Y, PaintingLocations[0].Z);
					CurrentArtProp = GlobalHelper.CreateProp(PenthouseArtPlacement.RequestModel(PaintProp[0]), vector, new Vector3(0, 0, PaintingLocations[0].W), true, false);
					CurrentArtProp.Position = new Vector3(vector.X, vector.Y, vector.Z - GetDimensions(CurrentArtProp).Z / 3f);
					InArtDecorator = true;
					CamActive = true;
					ArtPropCam = GlobalHelper.CreateCamera(CurrentArtProp.GetOffsetPosition(new Vector3(0f, -1f, 1f)), new Vector3(0f, 0f, 0f), 100f);
					ArtPropCam.IsActive = true;
					ArtPropCam.FarClip = 2000f;
					Vector3 position = ArtPropCam.Position;
					Vector3 position2 = CurrentArtProp.Position;
					Vector3 rotation = position - position2;
					rotation.Normalize();
					ArtPropCam.Rotation = rotation;
					Game.Player.Character.Position = ArtPropCam.Position;
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
					World.RenderingCamera = ArtPropCam;
				}
			}
			if (BusinessMain_ObjectPool != null)
			{
				if (PurchasePenthouse.submenu0 != null && (PurchasePenthouse.PenthouseArt.Visible || PurchasePenthouse.submenu16s.Visible || PurchasePenthouse.submenu0.Visible) && CamActive)
				{
					if (CurrentArtProp != null)
					{
						CurrentArtProp.Delete();
					}
					Function.Call(Hash.SET_RADAR_ZOOM, 5);
					Notification.Show("Removed Camera");
					CamActive = false;
					if (ArtPropCam != null)
					{
						InArtDecorator = false;
						Game.Player.Character.IsVisible = true;
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						ArtPropCam.IsActive = false;
						ArtPropCam.Delete();
						Game.Player.Character.IsVisible = true;
						Game.Player.Character.Position = PenthouseUpgradeMarker;
					}
				}
				if (!BusinessMain_ObjectPool.AreAnyVisible && CamActive)
				{
					if (CurrentArtProp != null)
					{
						CurrentArtProp.Delete();
					}
					Function.Call(Hash.SET_RADAR_ZOOM, 5);
					Notification.Show("Removed Camera");
					CamActive = false;
					if (ArtPropCam != null)
					{
						InArtDecorator = false;
						Game.Player.Character.IsVisible = true;
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						ArtPropCam.IsActive = false;
						ArtPropCam.Delete();
						Game.Player.Character.IsVisible = true;
						Game.Player.Character.Position = PenthouseUpgradeMarker;
					}
				}
			}
			if (!InArtDecorator)
			{
				return;
			}
			if (CurrentArtProp != null)
			{
				if (Game.IsControlPressed(Control.Cover))
				{
					CurrentArtProp.Rotation = new Vector3(0f, 0f, CurrentArtProp.Rotation.Z + 1f);
				}
				if (Game.IsControlPressed(Control.Context))
				{
					CurrentArtProp.Rotation = new Vector3(0f, 0f, CurrentArtProp.Rotation.Z - 1f);
				}
			}
			for (int i = 0; i < SculptLoc.Count; i++)
			{
				if (BusinessMain_ObjectPool.AreAnyVisible && (Class1.GetDistance_VDIST2(Game.Player.Character.Position, SculptLoc[i], Class1.GetDistSign.Less, 0.5f) ? true : false))
				{
					Notification.Show("Current ID: " + SculptLoc[i]);
				}
			}
			if (!BusinessMain_ObjectPool.AreAnyVisible && CurrentArtProp != null)
			{
				CurrentArtProp.Delete();
			}
		}
	}
}
