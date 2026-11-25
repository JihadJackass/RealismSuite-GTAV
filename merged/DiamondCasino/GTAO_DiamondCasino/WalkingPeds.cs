using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace GTAO_DiamondCasino
{
	internal class WalkingPeds : Script
	{
		public static Utils.MissionOjects ExtraPeds = new Utils.MissionOjects(new List<Utils.ObjectPlacementMap>
		{
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_offchair_exec_03"), 679200, 1580642483u, new Vector3(1089.25208f, 221.414841f, -49.5982361f), new Vector3(0f, 0f, 1.93618417f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Tom Connors"), 421409, 55858852u, new Vector3(1089.25549f, 221.3149f, -49.1982346f), new Vector3(0f, -0f, -178.063858f), 2725352035u, 200, 0, "false", "anim@amb@office@pa@male@", "pa_idle_a", 679200, 0, new Vector3(0f, -0.100000016f, 0.400000066f), new Vector3(0f, 0f, 179.999985f)),
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Agatha Bake"), 613929, 756308504u, new Vector3(1110.96265f, 242.779236f, -45.741f), new Vector3(-0f, -0f, -4.9999814f), 2725352035u, 200, 0, "false", "anim@amb@office@pa@female@", "pa_idle_d", 202500, 0, new Vector3(0f, 0f, 1.00000012f), new Vector3(0f, 0f, 179.999985f)),
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_offchair_02"), 202500, 1127420746u, new Vector3(1110.96265f, 242.779236f, -46.741f), new Vector3(0f, -0f, 175.000031f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Casino Staff"), 572212, 3163733717u, new Vector3(1117.42615f, 219.988464f, -49.88104f), new Vector3(-5.3360867E-07f, -1.59027725E-15f, 92.52163f), 2725352035u, 200, 0, "false", "mp_safehouseseated@male@generic@idle_a", "idle_a", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static List<Utils.MissionOjects> WalkingPedsObjects = new List<Utils.MissionOjects>
		{
			new Utils.MissionOjects(new List<Utils.ObjectPlacementMap>
			{
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Casino Staff"), 545796, 3163733717u, new Vector3(1105.72f, 204.357361f, -49.43972f), new Vector3(0f, 0f, 1.8289367f), 2725352035u, 200, 0, "false", "anim@move_f@waitress", "idle_var_05", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_champ_tray_01c"), 598548, 62973108u, new Vector3(1106.13318f, 204.463837f, -48.9332352f), new Vector3(0.966539145f, 0.725490332f, -144.046936f), 545796, 60, new Vector3(0.139999986f, 0.07999999f, 0.009999992f), new Vector3(13.999999f, 168.000015f, -20.9999962f))
			}),
			new Utils.MissionOjects(new List<Utils.ObjectPlacementMap>
			{
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Casino Staff"), 687633, 337826907u, new Vector3(1113.4906f, 260.20224f, -50.4389877f), new Vector3(0f, -0f, 134.7595f), 2725352035u, 200, 0, "false", "anim@move_f@waitress", "idle_var_01", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_champ_tray_01b"), 92177, 3590752576u, new Vector3(1113.08887f, 260.322784f, -49.91889f), new Vector3(-2.15571642f, 5.17496729f, -29.7272358f), 687633, 71, new Vector3(0.109999992f, 0.01f, -0.03f), new Vector3(12.999999f, 168.000015f, 1.90734863E-06f))
			}),
			new Utils.MissionOjects(new List<Utils.ObjectPlacementMap>
			{
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Casino Staff"), 343332, 3163733717u, new Vector3(1151.9668f, 254.2185f, -51.34137f), new Vector3(0f, 0f, 41.986412f), 2725352035u, 200, 0, "false", "anim@move_f@waitress", "idle_var_01", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_champ_tray_01a"), 4134, 3891277075u, new Vector3(1152.14771f, 254.579681f, -50.83386f), new Vector3(-3.13187146f, 4.02241945f, -127.1782f), 343332, 60, new Vector3(0.110000014f, 0.0200000033f, -0.0100000016f), new Vector3(13.000001f, 168.000015f, 0f))
			}),
			new Utils.MissionOjects(new List<Utils.ObjectPlacementMap>
			{
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Casino Staff"), 357942, 337826907u, new Vector3(1123.0116f, 264.816956f, -51.0414925f), new Vector3(0f, 0f, -85.57497f), 2725352035u, 200, 0, "false", "anim@move_f@waitress", "idle_var_04", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_champ_tray_01b"), 58936, 3590752576u, new Vector3(1123.25159f, 264.3931f, -50.55407f), new Vector3(-2.61049247f, 5.97916746f, 107.56411f), 357942, 71, new Vector3(0.140000045f, 0f, -0.0100000016f), new Vector3(13.000001f, 168.000015f, 0f))
			}),
			new Utils.MissionOjects(new List<Utils.ObjectPlacementMap>
			{
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Casino Staff"), 185411, 3163733717u, new Vector3(1139.05688f, 270.485962f, -51.4425774f), new Vector3(0f, -0f, -178.211639f), 2725352035u, 200, 0, "false", "anim@move_f@waitress", "idle_var_03", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_champ_tray_01a"), 168770, 3891277075u, new Vector3(1138.70825f, 270.2217f, -50.98249f), new Vector3(-3.3332603f, 8.23739052f, 23.8099842f), 185411, 60, new Vector3(0.120000035f, 0f, 0f), new Vector3(13.000001f, 168.000015f, 0f))
			})
		};

		public static List<Utils.MissionOjects> WalkingPedsRoutesObjects = new List<Utils.MissionOjects>
		{
			new Utils.MissionOjects(new List<Utils.ObjectPlacementMap>
			{
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 550708, 1468594282u, new Vector3(1106.43921f, 209.46965f, -50.4401054f), new Vector3(0f, 0f, -26.119751f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 719156, 1468594282u, new Vector3(1108.831f, 214.3814f, -50.4401054f), new Vector3(0f, 0f, -26.0173016f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 408360, 1468594282u, new Vector3(1110.62537f, 219.554382f, -50.4401054f), new Vector3(0f, -0f, -92.39861f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 518953, 1468594282u, new Vector3(1113.14417f, 223.689346f, -50.8407745f), new Vector3(0f, 0f, -51.5047035f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 382250, 1468594282u, new Vector3(1114.642f, 230.2955f, -50.84077f), new Vector3(0f, -0f, 179.366516f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 472887, 1468594282u, new Vector3(1116.502f, 233.120682f, -50.84077f), new Vector3(0f, 0f, -20.3857956f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 306231, 1468594282u, new Vector3(1117.52722f, 236.679657f, -50.840786f), new Vector3(0f, 0f, -18.03135f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 564012, 1468594282u, new Vector3(1121.2251f, 239.227585f, -51.44074f), new Vector3(0f, 0f, -47.2556038f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 430392, 1468594282u, new Vector3(1127.087f, 237.693924f, -51.44084f), new Vector3(0f, -0f, -107.444221f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 488760, 1468594282u, new Vector3(1132.24158f, 236.597137f, -51.4408226f), new Vector3(0f, 0f, 66.98105f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 115513, 1468594282u, new Vector3(1135.673f, 238.990158f, -51.44084f), new Vector3(0f, 0f, -35.483532f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 563501, 1468594282u, new Vector3(1137.72314f, 242.251984f, -51.4408226f), new Vector3(0f, 0f, 38.9848137f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 244269, 1468594282u, new Vector3(1135.65063f, 248.131256f, -52.03075f), new Vector3(0f, 0f, 29.10673f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 708152, 1468594282u, new Vector3(1133.76526f, 251.979813f, -52.03075f), new Vector3(0f, -0f, 114.834328f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 618297, 1468594282u, new Vector3(1128.00378f, 254.725937f, -51.4408226f), new Vector3(0f, 0f, 84.99515f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 410413, 1468594282u, new Vector3(1123.66516f, 256.344482f, -51.44074f), new Vector3(0f, -0f, -158.260071f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 533038, 1468594282u, new Vector3(1119.96411f, 254.18718f, -51.44074f), new Vector3(0f, -0f, 152.759323f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 390714, 1468594282u, new Vector3(1118.26587f, 249.332642f, -51.44074f), new Vector3(0f, -0f, -175.763611f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 270650, 1468594282u, new Vector3(1119.69714f, 243.990448f, -51.44074f), new Vector3(0f, -0f, -170.030533f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 721466, 1468594282u, new Vector3(1121.21216f, 239.267792f, -51.4209328f), new Vector3(0f, -0f, 164.070786f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 492079, 1468594282u, new Vector3(1117.64246f, 236.659882f, -50.840786f), new Vector3(0f, -0f, 136.07428f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 398907, 1468594282u, new Vector3(1116.42566f, 233.15683f, -50.8407745f), new Vector3(0f, -0f, 157.773788f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 645680, 1468594282u, new Vector3(1114.57068f, 230.155f, -50.8407745f), new Vector3(0f, -0f, 153.1161f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 468027, 1468594282u, new Vector3(1113.0946f, 223.785873f, -50.8407745f), new Vector3(0f, -0f, 164.784637f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 415791, 1468594282u, new Vector3(1110.634f, 219.602631f, -50.4294f), new Vector3(0f, -0f, 156.492676f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 229424, 1468594282u, new Vector3(1108.841f, 214.557648f, -50.4401054f), new Vector3(0f, -0f, 166.523575f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 374321, 1468594282u, new Vector3(1106.42712f, 209.537811f, -50.4401054f), new Vector3(0f, -0f, 165.448f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new Utils.MissionOjects(new List<Utils.ObjectPlacementMap>
			{
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 183580, 1468594282u, new Vector3(1111.77148f, 258.330536f, -51.44074f), new Vector3(0f, -0f, -163.048416f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 122652, 1468594282u, new Vector3(1109.64f, 255.231964f, -51.4407768f), new Vector3(0f, -0f, 156.619827f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 374556, 1468594282u, new Vector3(1105.6272f, 250.796631f, -51.44074f), new Vector3(0f, -0f, 144.745514f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 156189, 1468594282u, new Vector3(1102.74731f, 248.484726f, -51.44074f), new Vector3(0f, -0f, 144.283691f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 23836, 1468594282u, new Vector3(1098.26379f, 244.10083f, -51.44074f), new Vector3(0f, -0f, -102.861732f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 172828, 1468594282u, new Vector3(1101.14819f, 239.142639f, -51.44074f), new Vector3(0f, -0f, -139.864624f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 633629, 1468594282u, new Vector3(1104.50879f, 235.188644f, -50.8407745f), new Vector3(0f, 0f, -7.50942469f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 68382, 1468594282u, new Vector3(1107.90588f, 231.066666f, -50.8407745f), new Vector3(0f, -0f, 113.836319f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 285982, 1468594282u, new Vector3(1108.06091f, 226.543808f, -50.8407745f), new Vector3(0f, -0f, 159.387024f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 387358, 1468594282u, new Vector3(1110.44263f, 225.722229f, -50.84077f), new Vector3(0f, 0f, 71.0989151f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 185118, 1468594282u, new Vector3(1113.70557f, 226.438568f, -50.8407745f), new Vector3(0f, 0f, 26.16179f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 713758, 1468594282u, new Vector3(1114.455f, 229.777359f, -50.8407745f), new Vector3(0f, 0f, 3.69327378f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 762651, 1468594282u, new Vector3(1111.08887f, 232.994049f, -50.8407745f), new Vector3(0f, 0f, -16.5229549f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 595997, 1468594282u, new Vector3(1111.53967f, 235.466125f, -50.8407745f), new Vector3(0f, 0f, -59.7703819f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 626207, 1468594282u, new Vector3(1115.117f, 237.5309f, -50.840786f), new Vector3(0f, 0f, -59.9237747f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 674590, 1468594282u, new Vector3(1118.04041f, 236.14418f, -50.8407745f), new Vector3(0f, 0f, -77.99055f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 390432, 1468594282u, new Vector3(1121.29309f, 239.6485f, -51.44074f), new Vector3(0f, 0f, -48.1005135f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 549660, 1468594282u, new Vector3(1118.97693f, 246.5761f, -51.44074f), new Vector3(0f, 0f, -67.80493f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 662816, 1468594282u, new Vector3(1117.386f, 251.524048f, -51.44074f), new Vector3(0f, -0f, -108.391289f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 209438, 1468594282u, new Vector3(1117.62646f, 255.6954f, -51.44074f), new Vector3(0f, -0f, -150.358963f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 347427, 1468594282u, new Vector3(1116.03906f, 257.981262f, -51.44074f), new Vector3(0f, 0f, 46.8494072f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 65060, 1468594282u, new Vector3(1113.7356f, 260.2476f, -51.44074f), new Vector3(0f, 0f, 46.2351379f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new Utils.MissionOjects(new List<Utils.ObjectPlacementMap>
			{
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 35116, 1468594282u, new Vector3(1150.83765f, 256.3316f, -52.4416428f), new Vector3(0f, -0f, -157.9144f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 341037, 1468594282u, new Vector3(1146.50989f, 256.546661f, -52.44084f), new Vector3(0f, -0f, -178.949875f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 178221, 1468594282u, new Vector3(1143.18872f, 257.282166f, -52.44084f), new Vector3(0f, -0f, 139.1319f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 629805, 1468594282u, new Vector3(1138.93921f, 261.515747f, -52.4414063f), new Vector3(0f, -0f, 112.977783f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 417066, 1468594282u, new Vector3(1138.82007f, 266.850159f, -52.44361f), new Vector3(0f, -0f, 111.697823f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 88878, 1468594282u, new Vector3(1138.38232f, 268.7174f, -52.44084f), new Vector3(0f, 0f, 57.6502762f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 319789, 1468594282u, new Vector3(1134.162f, 271.168884f, -52.0408363f), new Vector3(0f, 0f, 59.13437f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 96301, 1468594282u, new Vector3(1128.95935f, 271.092377f, -52.0408363f), new Vector3(0f, -0f, 91.8385239f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 208173, 1468594282u, new Vector3(1124.644f, 265.385925f, -52.0408363f), new Vector3(0f, -0f, 155.711716f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 599084, 1468594282u, new Vector3(1130.302f, 257.8275f, -52.0408363f), new Vector3(0f, 0f, -86.72444f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 314414, 1468594282u, new Vector3(1132.62964f, 253.159409f, -52.03075f), new Vector3(0f, 0f, 59.5427666f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 92718, 1468594282u, new Vector3(1136.24768f, 247.502365f, -52.03075f), new Vector3(0f, -0f, 153.458939f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 628527, 1468594282u, new Vector3(1138.172f, 242.514191f, -51.4408226f), new Vector3(0f, -0f, -162.729843f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 62256, 1468594282u, new Vector3(1133.7583f, 237.4155f, -51.44084f), new Vector3(0f, 0f, 28.6259766f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 227886, 1468594282u, new Vector3(1127.80029f, 237.622879f, -51.4408226f), new Vector3(0f, 0f, -31.2557678f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 162098, 1468594282u, new Vector3(1121.5708f, 240.237289f, -51.44074f), new Vector3(0f, -0f, -113.676407f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 696882, 1468594282u, new Vector3(1119.00574f, 246.4292f, -51.44074f), new Vector3(0f, 0f, -71.1954041f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 332082, 1468594282u, new Vector3(1117.32056f, 251.4434f, -51.44074f), new Vector3(0f, 0f, -71.75805f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 299058, 1468594282u, new Vector3(1117.60559f, 255.628586f, -51.44074f), new Vector3(0f, -0f, -97.911026f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 661299, 1468594282u, new Vector3(1122.03284f, 256.8773f, -51.44074f), new Vector3(0f, -0f, -170.280518f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 649779, 1468594282u, new Vector3(1127.47913f, 255.039673f, -51.4408226f), new Vector3(0f, -0f, 110.082031f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 217652, 1468594282u, new Vector3(1132.56616f, 253.203857f, -52.03075f), new Vector3(0f, -0f, -121.811394f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 423218, 1468594282u, new Vector3(1137.62622f, 249.938354f, -52.03075f), new Vector3(0f, -0f, -127.594063f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 726065, 1468594282u, new Vector3(1139.7594f, 248.307724f, -52.0408363f), new Vector3(0f, -0f, -148.885254f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 589872, 1468594282u, new Vector3(1141.98523f, 244.677673f, -52.0408363f), new Vector3(0f, -0f, -148.731216f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 231732, 1468594282u, new Vector3(1145.31409f, 242.978577f, -52.0408363f), new Vector3(0f, 0f, 9.265024f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 773426, 1468594282u, new Vector3(1151.09753f, 244.320969f, -52.0408363f), new Vector3(0f, 0f, 36.6979179f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 657461, 1468594282u, new Vector3(1153.9115f, 249.69931f, -52.0408363f), new Vector3(0f, 0f, -14.5855083f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 780082, 1468594282u, new Vector3(1152.38062f, 253.3691f, -52.0408363f), new Vector3(0f, 0f, 21.7017269f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new Utils.MissionOjects(new List<Utils.ObjectPlacementMap>
			{
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 378172, 1468594282u, new Vector3(1124.62292f, 264.6083f, -52.0408363f), new Vector3(0f, -0f, 164.1861f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 452920, 1468594282u, new Vector3(1129.07886f, 264.860718f, -52.03075f), new Vector3(0f, 0f, -88.3331757f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 53820, 1468594282u, new Vector3(1136.813f, 265.233f, -52.0408363f), new Vector3(0f, -0f, 164.186752f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 195133, 1468594282u, new Vector3(1138.98279f, 261.576233f, -52.4414139f), new Vector3(0f, -0f, 117.918472f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 367421, 1468594282u, new Vector3(1143.15491f, 257.316284f, -52.4210358f), new Vector3(0f, -0f, 136.957291f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 296765, 1468594282u, new Vector3(1146.448f, 256.576172f, -52.44084f), new Vector3(0f, -0f, 172.425323f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 553274, 1468594282u, new Vector3(1146.51074f, 250.264786f, -52.03075f), new Vector3(0f, -0f, 178.105621f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 597051, 1468594282u, new Vector3(1146.4585f, 243.376068f, -52.0408363f), new Vector3(0f, -0f, 178.9239f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 27711, 1468594282u, new Vector3(1141.54712f, 244.793274f, -52.0408363f), new Vector3(0f, 0f, 37.0985031f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 65600, 1468594282u, new Vector3(1137.7074f, 249.906265f, -52.03075f), new Vector3(0f, 0f, 42.47233f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 33600, 1468594282u, new Vector3(1132.68127f, 253.144653f, -52.03075f), new Vector3(0f, 0f, 55.77898f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 725821, 1468594282u, new Vector3(1130.46436f, 257.618622f, -52.0408363f), new Vector3(0f, 0f, 35.8181953f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 334145, 1468594282u, new Vector3(1126.1864f, 260.445984f, -52.0408363f), new Vector3(0f, 0f, 32.33755f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new Utils.MissionOjects(new List<Utils.ObjectPlacementMap>
			{
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 615238, 1468594282u, new Vector3(1138.83826f, 266.833649f, -52.4238052f), new Vector3(0f, -0f, 115.6607f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 779821, 1468594282u, new Vector3(1139.05176f, 261.563629f, -52.44132f), new Vector3(0f, -0f, 107.471344f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 724805, 1468594282u, new Vector3(1136.99536f, 255.4947f, -52.0307541f), new Vector3(0f, -0f, 137.462692f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 213065, 1468594282u, new Vector3(1132.53943f, 253.210663f, -52.0060654f), new Vector3(0f, 0f, 86.3833542f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 745029, 1468594282u, new Vector3(1127.54358f, 255.052353f, -51.3616447f), new Vector3(0f, 0f, 74.8672256f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 644681, 1468594282u, new Vector3(1117.60339f, 255.693436f, -51.42093f), new Vector3(0f, -0f, 90.835144f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 202058, 1468594282u, new Vector3(1117.25464f, 251.4103f, -51.44074f), new Vector3(0f, -0f, -166.80162f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 643402, 1468594282u, new Vector3(1118.9043f, 246.559341f, -51.44074f), new Vector3(0f, -0f, -158.3044f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 317770, 1468594282u, new Vector3(1121.3811f, 239.668427f, -51.44074f), new Vector3(0f, -0f, 136.0298f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 277577, 1468594282u, new Vector3(1117.9093f, 236.0571f, -50.8407745f), new Vector3(0f, 0f, 24.3514233f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 310091, 1468594282u, new Vector3(1115.14148f, 237.5035f, -50.7575264f), new Vector3(0f, 0f, 55.82759f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 204361, 1468594282u, new Vector3(1111.64124f, 235.415268f, -50.8407745f), new Vector3(0f, -0f, 149.0273f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 749895, 1468594282u, new Vector3(1111.08679f, 233.043015f, -50.8290749f), new Vector3(0f, -0f, 157.881241f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 590410, 1468594282u, new Vector3(1108.00745f, 231.1452f, -50.8407745f), new Vector3(0f, -0f, 115.860619f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 629579, 1468594282u, new Vector3(1107.97864f, 226.519669f, -50.8407745f), new Vector3(0f, -0f, 176.3554f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 403788, 1468594282u, new Vector3(1109.04626f, 223.515839f, -50.84077f), new Vector3(0f, -0f, -169.774231f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 338764, 1468594282u, new Vector3(1110.94531f, 217.0884f, -50.4401054f), new Vector3(0f, -0f, 119.544563f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 394572, 1468594282u, new Vector3(1114.69641f, 214.829147f, -50.4401054f), new Vector3(0f, -0f, -140.34491f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 42317, 1468594282u, new Vector3(1116.82153f, 210.916779f, -50.4401054f), new Vector3(0f, -0f, -145.76976f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 360012, 1468594282u, new Vector3(1116.24377f, 206.000168f, -50.4401054f), new Vector3(0f, 0f, 41.3911476f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 40781, 1468594282u, new Vector3(1113.19836f, 203.594467f, -50.4401054f), new Vector3(0f, 0f, 10.4263754f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 220236, 1468594282u, new Vector3(1109.4325f, 203.805511f, -50.4401054f), new Vector3(0f, 0f, 68.10729f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 407115, 1468594282u, new Vector3(1104.30884f, 206.7566f, -50.4401054f), new Vector3(0f, 0f, -28.0618134f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 100686, 1468594282u, new Vector3(1100.90063f, 208.694962f, -50.4401054f), new Vector3(0f, -0f, -113.379654f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 652622, 1468594282u, new Vector3(1096.54822f, 208.1716f, -49.99987f), new Vector3(0f, -0f, -167.580017f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 41295, 1468594282u, new Vector3(1095.03967f, 213.1951f, -49.9997559f), new Vector3(0f, -0f, -95.51616f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 349519, 1468594282u, new Vector3(1095.62122f, 216.828918f, -49.9899673f), new Vector3(0f, 0f, 21.7909069f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 271439, 1468594282u, new Vector3(1094.63965f, 221.043732f, -49.9899673f), new Vector3(0f, -0f, 164.892868f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 4688, 1468594282u, new Vector3(1096.52979f, 224.509109f, -49.9899673f), new Vector3(0f, 0f, 18.4633026f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 710991, 1468594282u, new Vector3(1102.375f, 225.255447f, -49.9899673f), new Vector3(0f, 0f, 53.57332f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 174416, 1468594282u, new Vector3(1106.86243f, 227.950058f, -50.8407745f), new Vector3(0f, 0f, -20.1786346f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 389967, 1468594282u, new Vector3(1108.01819f, 231.049759f, -50.84077f), new Vector3(0f, 0f, 34.07275f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 304723, 1468594282u, new Vector3(1111.02942f, 232.930573f, -50.8407745f), new Vector3(0f, 0f, -20.93141f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 294229, 1468594282u, new Vector3(1111.55383f, 235.408157f, -50.8407745f), new Vector3(0f, 0f, -14.8408f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 483669, 1468594282u, new Vector3(1115.12964f, 237.490051f, -50.8209763f), new Vector3(0f, 0f, -46.98216f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 279382, 1468594282u, new Vector3(1117.98645f, 236.19606f, -50.84077f), new Vector3(0f, 0f, -83.9858551f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 165718, 1468594282u, new Vector3(1121.51721f, 239.610138f, -51.44074f), new Vector3(0f, 0f, -29.8870888f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 290134, 1468594282u, new Vector3(1127.69824f, 237.5416f, -51.4408226f), new Vector3(0f, -0f, -95.49925f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 114519, 1468594282u, new Vector3(1132.26318f, 235.966614f, -51.44084f), new Vector3(0f, 0f, 53.1308556f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 301911, 1468594282u, new Vector3(1137.25037f, 234.089661f, -51.44084f), new Vector3(0f, -0f, -103.073563f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 705111, 1468594282u, new Vector3(1140.18347f, 236.5653f, -51.4408226f), new Vector3(0f, -0f, -136.033722f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 5976, 1468594282u, new Vector3(1138.18884f, 242.404861f, -51.4408226f), new Vector3(0f, 0f, -21.3366165f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 340799, 1468594282u, new Vector3(1136.25037f, 247.4552f, -52.01094f), new Vector3(0f, 0f, 38.340416f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 700247, 1468594282u, new Vector3(1133.39709f, 250.837387f, -52.0307541f), new Vector3(0f, -0f, -98.7222748f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_artifact_01a"), 334681, 1468594282u, new Vector3(1137.067f, 255.612946f, -52.0307541f), new Vector3(0f, -0f, 108.91143f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			})
		};

		public static bool CreatedWalkingPeds;

		public static Ped Waitress_1;

		public static bool OverrideBlock;

		public static bool RunMethodNow()
		{
			bool result = true;
			if (!Class1.IsinInterior)
			{
				_Debug.WalkingPeds = 0;
				result = false;
			}
			_Debug.WalkingPeds = 1;
			return result;
		}

		public WalkingPeds()
		{
			if (_Debug.WalkingPeds != -1)
			{
				base.Tick += OnTick;
			}
		}

		public unsafe static void OnTick(object sender, EventArgs e)
		{
			if (!Class1.IsinInterior)
			{
				_Debug.WalkingPeds = 0;
				OverrideBlock = false;
				if (!CreatedWalkingPeds)
				{
					return;
				}
			}
			if (CreatedWalkingPeds && !OverrideBlock)
			{
				_Debug.WalkingPeds = 0;
				return;
			}
			_Debug.WalkingPeds = 1;
			if (Class1.IsinInterior)
			{
				OverrideBlock = false;
				if (CreatedWalkingPeds)
				{
					return;
				}
				CreatedWalkingPeds = true;
				ExtraPeds.CreateEntites();
				foreach (Utils.MissionOjects walkingPedsObject in WalkingPedsObjects)
				{
					walkingPedsObject.CreateEntites();
				}
				for (int i = 0; i < 5; i++)
				{
					int index = i;
					if (WalkingPedsObjects[index].Entities[0] == null)
					{
						continue;
					}
					Ped ped = (Ped)WalkingPedsObjects[index].Entities[0].Entity;
					if (!(ped != null))
					{
						continue;
					}
					Class1.SetPedOutfit(ped);
					CasinoPeds.assignPedProperties(ped, false);
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@move_f@waitress"))
					{
						Class1.LoadDict("anim@move_f@waitress");
						Screen.ShowSubtitle("Loading Anim");
						Script.Wait(0);
					}
					Function.Call(Hash.SET_PED_ALTERNATE_MOVEMENT_ANIM, ped, 0, "anim@move_f@waitress", "idle", 1090519040, true);
					Function.Call(Hash.SET_PED_ALTERNATE_MOVEMENT_ANIM, ped, 1, "anim@move_f@waitress", "walk", 1090519040, true);
					Function.Call(Hash.REMOVE_ANIM_SET, "anim@move_f@waitress");
					Script.Wait(100);
					int taskID = 0;
					Utils.OpenSequenceTask(&taskID);
					foreach (Utils.ObjectPlacementMap entity in WalkingPedsRoutesObjects[index].Entities)
					{
						Utils.TaskFollowNavMeshToCoord(0, entity.Position.X, entity.Position.Y, entity.Position.Z, 1f, -1, 1f, 512, -1f);
						Function.Call(Hash.SET_PED_ALTERNATE_WALK_ANIM, 0, "anim@move_f@waitress", "walk", 8f, true);
						Function.Call(Hash.SET_PED_ALTERNATE_MOVEMENT_ANIM, 0, 0, "anim@move_f@waitress", "idle", 1090519040, true);
						Function.Call(Hash.SET_PED_ALTERNATE_MOVEMENT_ANIM, 0, 1, "anim@move_f@waitress", "walk", 1090519040, true);
					}
					Utils.RepeatTaskSequence(taskID, true);
					Utils.CloseSequenceTask(taskID);
					Utils.TaskPerformSequence(ped, taskID);
					Utils.ClearSequenceTask(&taskID);
				}
			}
			else
			{
				if (!CreatedWalkingPeds)
				{
					return;
				}
				CreatedWalkingPeds = false;
				ExtraPeds.DeleteEntites();
				foreach (Utils.MissionOjects walkingPedsObject2 in WalkingPedsObjects)
				{
					foreach (Utils.ObjectPlacementMap entity2 in walkingPedsObject2.Entities)
					{
						if (entity2.Entity != null)
						{
							entity2.Entity.Delete();
						}
					}
				}
			}
		}
	}
}
