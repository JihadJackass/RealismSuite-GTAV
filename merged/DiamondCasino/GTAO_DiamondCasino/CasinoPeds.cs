using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace GTAO_DiamondCasino
{
	internal class CasinoPeds : Script
	{
		public int NextPedCheck;

		public int NextGroupCheck;

		public static List<string> ambientPedModelList = new List<string>
		{
			"A_M_Y_SmartCasPat_01", "A_M_Y_GenCasPat_01", "A_M_Y_SmartCasPat_01", "A_M_Y_GenCasPat_01", "A_F_Y_GenCasPat_01", "A_F_Y_SmartCasPat_01", "g_m_m_casrn_01", "A_M_Y_SmartCasPat_01", "A_M_Y_GenCasPat_01", "A_M_Y_SmartCasPat_01",
			"A_F_Y_GenCasPat_01", "A_F_Y_SmartCasPat_01", "g_m_m_casrn_01", "A_M_Y_GenCasPat_01", "A_M_Y_SmartCasPat_01", "A_M_Y_GenCasPat_01", "A_F_Y_SmartCasPat_01", "g_m_m_casrn_01", "A_M_Y_SmartCasPat_01", "A_M_Y_GenCasPat_01",
			"A_M_Y_SmartCasPat_01", "A_M_Y_GenCasPat_01", "A_M_Y_SmartCasPat_01", "A_M_Y_GenCasPat_01", "A_F_Y_GenCasPat_01"
		};

		public static List<string> staffPedModelList = new List<string> { "s_f_y_casino_01", "s_m_y_casino_01" };

		public static List<string> ped_FemaleConvoIdleDict = new List<string> { "anim@amb@casino@hangout@ped_female@stand@01a@idles_convo", "anim@amb@casino@hangout@ped_female@stand@01b@idles_convo", "anim@amb@casino@hangout@ped_female@stand@02a@idles_convo", "anim@amb@casino@hangout@ped_female@stand@02b@idles_convo", "anim@amb@casino@hangout@ped_female@stand@03a@idles_convo", "anim@amb@casino@hangout@ped_female@stand@03b@idles_convo" };

		public static List<string> ped_MaleConvoIdleDict = new List<string> { "anim@amb@casino@hangout@ped_male@stand@01a@idles_convo", "anim@amb@casino@hangout@ped_male@stand@01b@idles_convo", "anim@amb@casino@hangout@ped_male@stand@02a@idles_convo", "anim@amb@casino@hangout@ped_male@stand@02b@idles_convo", "anim@amb@casino@hangout@ped_male@stand@03a@idles_convo", "anim@amb@casino@hangout@ped_male@stand@03b@idles_convo" };

		public static List<string> ped_ConvoIdlesClip = new List<string> { "idle_a", "idle_b", "idle_c", "idle_d" };

		private static List<string> ped_FemaleIdleBaseDict = new List<string> { "anim@amb@casino@hangout@ped_female@stand@01a@base", "anim@amb@casino@hangout@ped_female@stand@01b@base", "anim@amb@casino@hangout@ped_female@stand@02a@base", "anim@amb@casino@hangout@ped_female@stand@02b@base", "anim@amb@casino@hangout@ped_female@stand@03a@base", "anim@amb@casino@hangout@ped_female@stand@03b@base" };

		private static List<string> ped_MaleIdleBaseDict = new List<string> { "anim@amb@casino@hangout@ped_male@stand@01a@base", "anim@amb@casino@hangout@ped_male@stand@01b@base", "anim@amb@casino@hangout@ped_male@stand@02a@base", "anim@amb@casino@hangout@ped_male@stand@02b@base", "anim@amb@casino@hangout@ped_male@stand@03a@base", "anim@amb@casino@hangout@ped_male@stand@03b@base" };

		private static List<string> ped_FemaleIdleIdlesDict = new List<string> { "anim@amb@casino@hangout@ped_female@stand@01a@idles", "anim@amb@casino@hangout@ped_female@stand@01b@idles", "anim@amb@casino@hangout@ped_female@stand@02a@idles", "anim@amb@casino@hangout@ped_female@stand@02b@idles", "anim@amb@casino@hangout@ped_female@stand@03a@idles", "anim@amb@casino@hangout@ped_female@stand@03b@idles" };

		private static List<string> ped_MaleIdleIdlesDict = new List<string> { "anim@amb@casino@hangout@ped_male@stand@01a@idles", "anim@amb@casino@hangout@ped_male@stand@01b@idles", "anim@amb@casino@hangout@ped_male@stand@02a@idles", "anim@amb@casino@hangout@ped_male@stand@02b@idles", "anim@amb@casino@hangout@ped_male@stand@03a@idles", "anim@amb@casino@hangout@ped_male@stand@03b@idles" };

		public static Random rnd = new Random();

		public int getNEarbyGroup;

		public static Vector3 EnterGamingZoneA = new Vector3(1102.99f, 236.9f, -50f);

		public static Vector3 MainZone = new Vector3(1119.21f, 237.07f, -50f);

		public static Vector3 EnterMainHallA = new Vector3(1137.016f, 236.92f, -50f);

		public static Vector3 EnterMainHallB = new Vector3(1117.52f, 250.98f, -50f);

		public static Vector3 EnterMainHallC = new Vector3(1112.32f, 257.97f, -50f);

		public static Vector3 EnterMainHallD = new Vector3(1101.48f, 246.71f, -50f);

		public static Vector3 LastSpawnPos;

		public static string CurrentMarker;

		public static bool UseLessPeds = false;

		public new static void Wait(int time)
		{
			Script.Wait(time);
		}

		public CasinoPeds()
		{
			if (_Debug.CasinoPeds != -1)
			{
				base.Tick += OnTick;
			}
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

		public static void DeleteMainHallPeds()
		{
			foreach (Ped mainEnterancePed in Class1.MainEnterancePeds)
			{
				if (mainEnterancePed != null)
				{
					mainEnterancePed.Delete();
				}
			}
		}

		public static void SpawnMainHallPeds()
		{
			spawnPed("Ambient", 1111.095f, 236.2102f, -49.84077f, 5.663957f, Class1.MainEnterancePeds);
			spawnPed("Ambient", 1110.046f, 236.2936f, -49.84077f, 9.66931f, Class1.MainEnterancePeds);
			if (!UseLessPeds)
			{
				spawnPed("Ambient", 1116.052f, 214.8701f, -49.43397f, 215.5901f, Class1.MainEnterancePeds);
				spawnPed("Ambient", 1117.318f, 213.7941f, -49.44009f, 44.83752f, Class1.MainEnterancePeds);
				spawnPed("Ambient", 1117.52f, 214.9503f, -49.44009f, 141.8353f, Class1.MainEnterancePeds);
			}
			spawnPed("Ambient_Convo", 1115.622f, 203.4886f, -49.44013f, 14.0468f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1115.786f, 204.5832f, -49.44011f, 170.9756f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1114.602f, 203.8935f, -49.44011f, 260.443f, Class1.MainEnterancePeds);
			if (!UseLessPeds)
			{
				spawnPed("Ambient", 1102.066f, 206.0208f, -49.44009f, 89.80536f, Class1.MainEnterancePeds);
				spawnPed("Ambient", 1100.216f, 206.6753f, -49.44009f, 262.116f, Class1.MainEnterancePeds);
			}
			spawnPed("Ambient", 1100.191f, 211.3238f, -49.44009f, 310.7232f, Class1.MainEnterancePeds);
			spawnPed("Ambient", 1101.487f, 211.2394f, -49.44009f, 86.45404f, Class1.MainEnterancePeds);
			spawnPed("Ambient", 1101.133f, 210.3366f, -49.44009f, 24.05198f, Class1.MainEnterancePeds);
			spawnPed("Ambient", 1094.042f, 217.9336f, -48.99499f, 9.927972f, Class1.MainEnterancePeds);
			spawnPed("Ambient", 1098.412f, 215.0746f, -48.99499f, 356.0311f, Class1.MainEnterancePeds);
			if (!UseLessPeds)
			{
				spawnPed("Ambient", 1113.874f, 212.9357f, -49.44012f, 42.64484f, Class1.MainEnterancePeds);
				spawnPed("Ambient", 1113.819f, 214.1065f, -49.44012f, 156.4539f, Class1.MainEnterancePeds);
			}
			if (!UseLessPeds)
			{
				spawnPed("Ambient", 1108.797f, 217.9115f, -49.44012f, 9.28023f, Class1.MainEnterancePeds);
				spawnPed("Ambient", 1108.319f, 219.3674f, -49.44011f, 194.5887f, Class1.MainEnterancePeds);
			}
			if (!UseLessPeds)
			{
				spawnPed("Ambient", 1107.536f, 224.4355f, -49.74295f, 39.71287f, Class1.MainEnterancePeds);
				spawnPed("Ambient", 1106.733f, 225.6312f, -49.73832f, 204.5223f, Class1.MainEnterancePeds);
			}
			spawnPed("Ambient", 1119.74f, 225.4799f, -49.84076f, 307.2037f, Class1.MainEnterancePeds);
			spawnPed("Ambient", 1120.963f, 226.3334f, -49.84076f, 205.7111f, Class1.MainEnterancePeds);
			spawnPed("Ambient", 1120.983f, 225.2209f, -49.84076f, 72.12221f, Class1.MainEnterancePeds);
			spawnPed("Ambient", 1117.99f, 233.707f, -49.84078f, 22.66266f, Class1.MainEnterancePeds);
			spawnPed("Ambient", 1117.709f, 234.7181f, -49.84079f, 188.2404f, Class1.MainEnterancePeds);
			spawnPed("Ambient", 1115.332f, 238.5402f, -49.84079f, 267.2131f, Class1.MainEnterancePeds);
			spawnPed("Ambient", 1116.535f, 237.857f, -49.84079f, 19.71138f, Class1.MainEnterancePeds);
			spawnPed("Ambient", 1100.501f, 214.6202f, -48.99495f, 14.99359f, Class1.MainEnterancePeds);
			spawnPed("Ambient", 1103.254f, 216.0214f, -48.99495f, 39.36172f, Class1.MainEnterancePeds);
			spawnPed("Ambient", 1095.918f, 224.819f, -48.99497f, 216.0234f, Class1.MainEnterancePeds);
			spawnPed("Ambient", 1099.95f, 225.1008f, -48.99497f, 168.4407f, Class1.MainEnterancePeds);
			spawnPed("Ambient", 1102.207f, 215.0328f, -48.99497f, 30.86236f, Class1.MainEnterancePeds);
			spawnPed("Ambient", 1095.263f, 220.8741f, -48.99496f, 266.7065f, Class1.MainEnterancePeds);
			if (!UseLessPeds)
			{
				spawnPed("Ambient_Convo", 1095.813f, 217.6257f, -48.99496f, 127.801f, Class1.MainEnterancePeds);
				spawnPed("Ambient_Convo", 1095.126f, 217.2566f, -48.99496f, 291.97f, Class1.MainEnterancePeds);
			}
			if (!UseLessPeds)
			{
				spawnPed("Ambient_Convo", 1104.907f, 211.8016f, -49.44011f, 28.96025f, Class1.MainEnterancePeds);
				spawnPed("Ambient_Convo", 1105.644f, 212.8635f, -49.44011f, 99.6747f, Class1.MainEnterancePeds);
				spawnPed("Ambient_Convo", 1104.324f, 212.9702f, -49.44011f, 234.1667f, Class1.MainEnterancePeds);
			}
			spawnPed("Ambient_Convo", 1095.335f, 208.0307f, -48.9998f, 337.937f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1096.115f, 208.8232f, -48.99982f, 139.0096f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1095.064f, 209.019f, -48.9998f, 210.3411f, Class1.MainEnterancePeds);
			if (!UseLessPeds)
			{
				spawnPed("Ambient_Convo", 1088.945f, 212.4277f, -49.00023f, 301.7932f, Class1.MainEnterancePeds);
				spawnPed("Ambient_Convo", 1090.333f, 213.2783f, -49.00019f, 120.3152f, Class1.MainEnterancePeds);
			}
			spawnPed("Ambient_Convo", 1103.238f, 204.2908f, -49.44009f, 293.7145f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1104.429f, 204.0358f, -49.44009f, 78.21771f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1103.989f, 205.1927f, -49.44009f, 162.4751f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1106.94f, 200.0394f, -49.44011f, 2.084367f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1108.042f, 200.8284f, -49.44011f, 124.106f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1106.571f, 201.1328f, -49.44011f, 205.9628f, Class1.MainEnterancePeds);
			if (!UseLessPeds)
			{
				spawnPed("Ambient_Convo", 1112.048f, 216.1039f, -49.44009f, 148.3837f, Class1.MainEnterancePeds);
				spawnPed("Ambient_Convo", 1111.001f, 215.4492f, -49.44009f, 299.8455f, Class1.MainEnterancePeds);
				spawnPed("Ambient_Convo", 1112.045f, 214.8782f, -49.44009f, 12.76126f, Class1.MainEnterancePeds);
			}
			spawnPed("Ambient_Convo", 1107.406f, 217.0322f, -49.44009f, 244.8367f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1108.55f, 216.6108f, -49.44012f, 71.04099f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1108.263f, 211.3615f, -49.44012f, 4.078389f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1107.831f, 212.4179f, -49.44012f, 214.1381f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1108.736f, 212.105f, -49.44012f, 119.9867f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1112.655f, 219.7584f, -49.44012f, 105.2008f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1111.404f, 219.4106f, -49.44012f, 286.4084f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1112.162f, 218.8443f, -49.44012f, 2.204866f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1106.144f, 207.383f, -49.44012f, 354.351f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1106.975f, 208.2808f, -49.44012f, 145.6713f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1105.673f, 208.2765f, -49.44012f, 234.3328f, Class1.MainEnterancePeds);
			if (!UseLessPeds)
			{
				spawnPed("Ambient_Convo", 1096.135f, 212.9631f, -48.99973f, 95.62075f, Class1.MainEnterancePeds);
			}
			if (!UseLessPeds)
			{
				spawnPed("Ambient_Convo", 1095.109f, 213.3836f, -48.99977f, 255.7172f, Class1.MainEnterancePeds);
				spawnPed("Ambient_Convo", 1095.407f, 212.297f, -48.99977f, 317.0717f, Class1.MainEnterancePeds);
			}
			if (!UseLessPeds)
			{
				spawnPed("Ambient_Convo", 1099.824f, 207.7433f, -49.44012f, 339.0691f, Class1.MainEnterancePeds);
				spawnPed("Ambient_Convo", 1100.191f, 208.8896f, -49.44012f, 161.4986f, Class1.MainEnterancePeds);
			}
			if (!UseLessPeds)
			{
				spawnPed("Ambient_Convo", 1102.72f, 210.7225f, -49.44012f, 207.2259f, Class1.MainEnterancePeds);
				spawnPed("Ambient_Convo", 1102.49f, 209.7168f, -49.44012f, 341.6592f, Class1.MainEnterancePeds);
				spawnPed("Ambient_Convo", 1103.64f, 210.1313f, -49.44012f, 80.35323f, Class1.MainEnterancePeds);
			}
			if (!UseLessPeds)
			{
				spawnPed("Ambient_Convo", 1108.212f, 204.635f, -49.44012f, 121.2613f, Class1.MainEnterancePeds);
				spawnPed("Ambient_Convo", 1107.625f, 203.7958f, -49.44012f, 336.5826f, Class1.MainEnterancePeds);
				spawnPed("Ambient_Convo", 1107.094f, 204.7056f, -49.44011f, 265.7986f, Class1.MainEnterancePeds);
			}
			spawnPed("Ambient_Shop", 1101.427f, 202.0399f, -49.44011f, 48.71939f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Shop", 1105.222f, 197.0543f, -49.44011f, 222.6552f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Shop", 1103.766f, 197.6607f, -49.44011f, 316.5819f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1091.882f, 218.1018f, -49.20037f, 111.9105f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1091.314f, 217.0556f, -49.20034f, 339.046f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1090.695f, 218.087f, -49.20035f, 262.855f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1116.541f, 212.1049f, -49.44013f, 47.32497f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1115.688f, 212.9339f, -49.44012f, 234.494f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1116.492f, 213.197f, -49.4401f, 155.9921f, Class1.MainEnterancePeds);
			if (!UseLessPeds)
			{
				spawnPed("Ambient_Convo", 1116.562f, 222.9531f, -49.43516f, 52.19352f, Class1.MainEnterancePeds);
				spawnPed("Ambient_Convo", 1115.781f, 223.7066f, -49.43583f, 227.1467f, Class1.MainEnterancePeds);
				spawnPed("Ambient_Convo", 1116.573f, 224.0073f, -49.43552f, 149.4794f, Class1.MainEnterancePeds);
			}
			if (!UseLessPeds)
			{
				spawnPed("Ambient_Convo", 1113.8f, 227.3636f, -49.84076f, 197.5729f, Class1.MainEnterancePeds);
			}
			spawnPed("Ambient_Convo", 1114.615f, 226.7325f, -49.84076f, 71.0835f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1113.522f, 226.3381f, -49.84076f, 332.6985f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1108.176f, 224.5086f, -49.84077f, 268.8908f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1109.362f, 224.4065f, -49.84077f, 86.14603f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1108.772f, 223.7844f, -49.84077f, 8.9926f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1103.165f, 223.9412f, -48.99495f, 225.3213f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1103.856f, 223.2046f, -48.99496f, 54.08739f, Class1.MainEnterancePeds);
			spawnPed("Ambient_Convo", 1104.116f, 224.0626f, -48.99496f, 132.3816f, Class1.MainEnterancePeds);
			spawnPed("Security", 1093.525f, 218.952f, -48.99496f, 277.5889f, Class1.MainEnterancePeds);
			spawnPed("Security", 1097.69f, 226.0964f, -48.99496f, 196.7982f, Class1.MainEnterancePeds);
		}

		public static void DeleteHallAPeds()
		{
			foreach (Ped hallwayAPed in Class1.HallwayAPeds)
			{
				if (hallwayAPed != null)
				{
					hallwayAPed.Delete();
				}
			}
		}

		public static void SpawnHallAPeds()
		{
			spawnPed("Ambient", 1098.436f, 238.5493f, -50.44072f, 46.30819f, Class1.HallwayAPeds);
			spawnPed("Ambient", 1097.692f, 239.7135f, -50.44072f, 226.3934f, Class1.HallwayAPeds);
			spawnPed("Ambient", 1099.054f, 239.7451f, -50.44072f, 187.1908f, Class1.HallwayAPeds);
			if (!UseLessPeds)
			{
				spawnPed("Ambient", 1102.649f, 240.459f, -50.44072f, 35.87964f, Class1.HallwayAPeds);
				spawnPed("Ambient", 1101.379f, 241.1677f, -50.44072f, 280.2795f, Class1.HallwayAPeds);
			}
			spawnPed("Ambient", 1095.204f, 242.8067f, -50.44072f, 35.63808f, Class1.HallwayAPeds);
			spawnPed("Ambient", 1095.268f, 244.7111f, -50.44072f, 164.2379f, Class1.HallwayAPeds);
			spawnPed("Ambient", 1096.292f, 244.1019f, -50.44072f, 121.6322f, Class1.HallwayAPeds);
			if (!UseLessPeds)
			{
				spawnPed("Ambient", 1097.243f, 247.1659f, -50.44072f, 309.9936f, Class1.HallwayAPeds);
				spawnPed("Ambient", 1098.647f, 247.0262f, -50.44072f, 57.19551f, Class1.HallwayAPeds);
			}
			if (!UseLessPeds)
			{
				spawnPed("Ambient", 1102.076f, 244.8788f, -50.44072f, 64.07639f, Class1.HallwayAPeds);
			}
			spawnPed("Ambient", 1104.129f, 246.8703f, -50.44072f, 307.0704f, Class1.HallwayAPeds);
			spawnPed("Ambient", 1105.803f, 247.5711f, -50.44072f, 52.5594f, Class1.HallwayAPeds);
			spawnPed("Ambient", 1104.366f, 248.3489f, -50.44072f, 208.8281f, Class1.HallwayAPeds);
			spawnPed("Ambient", 1099.599f, 254.5231f, -51.24227f, 46.72178f, Class1.HallwayAPeds);
			spawnPed("Ambient", 1102.507f, 258.1137f, -51.24092f, 73.02457f, Class1.HallwayAPeds);
			spawnPed("Ambient", 1103.421f, 255.4797f, -50.83745f, 204.9727f, Class1.HallwayAPeds);
			spawnPed("Ambient", 1104.435f, 253.7624f, -50.84049f, 16.67011f, Class1.HallwayAPeds);
			spawnPed("Ambient", 1104.956f, 255.3856f, -50.84088f, 113.3036f, Class1.HallwayAPeds);
			if (!UseLessPeds)
			{
				spawnPed("Ambient", 1109.138f, 252.5364f, -50.44083f, 268.6497f, Class1.HallwayAPeds);
				spawnPed("Ambient", 1110.242f, 253.005f, -50.44086f, 138.6669f, Class1.HallwayAPeds);
				spawnPed("Ambient", 1109.975f, 251.5302f, -50.44085f, 15.99325f, Class1.HallwayAPeds);
			}
			spawnPed("Ambient", 1109.234f, 257.1004f, -50.44072f, 2.858796f, Class1.HallwayAPeds);
			spawnPed("Ambient", 1109.675f, 258.6625f, -50.44072f, 157.6555f, Class1.HallwayAPeds);
			spawnPed("Ambient", 1111.402f, 261.2273f, -50.44072f, 331.7974f, Class1.HallwayAPeds);
			spawnPed("Ambient", 1112.661f, 262.0168f, -50.44072f, 168.4493f, Class1.HallwayAPeds);
			spawnPed("Ambient", 1112.654f, 261.0183f, -50.44072f, 70.63845f, Class1.HallwayAPeds);
			if (!UseLessPeds)
			{
				spawnPed("Ambient", 1112.537f, 255.4369f, -50.44078f, 29.29731f, Class1.HallwayAPeds);
				spawnPed("Ambient", 1112.727f, 256.6924f, -50.44075f, 176.4469f, Class1.HallwayAPeds);
				spawnPed("Ambient", 1111.493f, 256.0128f, -50.44075f, 283.5635f, Class1.HallwayAPeds);
			}
		}

		public static void DeleteHallBPeds()
		{
			foreach (Ped hallwayBPed in Class1.HallwayBPeds)
			{
				if (hallwayBPed != null)
				{
					hallwayBPed.Delete();
				}
			}
		}

		public static void SpawnHallBPeds()
		{
			spawnPed("Security", 1138.174f, 233.7164f, -50.4408f, 41.29535f, Class1.HallwayBPeds);
			if (!UseLessPeds)
			{
				spawnPed("Ambient", 1118.935f, 240.4305f, -50.43644f, 14.79257f, Class1.HallwayBPeds);
				spawnPed("Ambient", 1118.207f, 241.6676f, -50.44072f, 176.7056f, Class1.HallwayBPeds);
			}
			spawnPed("Ambient", 1121.646f, 242.6565f, -50.44072f, 286.0923f, Class1.HallwayBPeds);
			spawnPed("Ambient", 1123.034f, 242.9245f, -50.4408f, 198.34f, Class1.HallwayBPeds);
			spawnPed("Ambient", 1122.974f, 241.8389f, -50.44077f, 61.57843f, Class1.HallwayBPeds);
			if (!UseLessPeds)
			{
				spawnPed("Ambient", 1124.419f, 240.7996f, -50.4408f, 254.0152f, Class1.HallwayBPeds);
				spawnPed("Ambient", 1125.703f, 240.2029f, -50.4408f, 28.58896f, Class1.HallwayBPeds);
			}
			if (!UseLessPeds)
			{
				spawnPed("Ambient", 1125.019f, 235.8706f, -50.44076f, 178.7099f, Class1.HallwayBPeds);
				spawnPed("Ambient", 1124.325f, 235.1729f, -50.44076f, 103.3183f, Class1.HallwayBPeds);
				spawnPed("Ambient", 1123.133f, 236.4226f, -50.44075f, 205.5879f, Class1.HallwayBPeds);
				spawnPed("Ambient", 1124.066f, 236.8061f, -50.44075f, 194.9484f, Class1.HallwayBPeds);
			}
			spawnPed("Ambient", 1128.379f, 236.0778f, -50.44081f, 231.7241f, Class1.HallwayBPeds);
			spawnPed("Ambient", 1129.828f, 235.0164f, -50.44083f, 44.45817f, Class1.HallwayBPeds);
			spawnPed("Ambient", 1129.808f, 236.1413f, -50.44083f, 162.5104f, Class1.HallwayBPeds);
			if (!UseLessPeds)
			{
				spawnPed("Ambient", 1132.716f, 234.2531f, -50.44083f, 208.2981f, Class1.HallwayBPeds);
				spawnPed("Ambient", 1133.811f, 233.7485f, -50.44083f, 65.19306f, Class1.HallwayBPeds);
			}
			spawnPed("Ambient", 1127.968f, 256.2524f, -50.45139f, 62.81363f, Class1.HallwayBPeds);
			spawnPed("Ambient", 1126.193f, 256.8166f, -50.44081f, 260.2012f, Class1.HallwayBPeds);
			if (!UseLessPeds)
			{
				spawnPed("Ambient", 1124.5f, 258.2428f, -50.44076f, 62.7263f, Class1.HallwayBPeds);
				spawnPed("Ambient", 1123.447f, 259.5547f, -50.44076f, 139.6605f, Class1.HallwayBPeds);
				spawnPed("Ambient", 1122.611f, 258.2857f, -50.44076f, 279.2654f, Class1.HallwayBPeds);
			}
			if (!UseLessPeds)
			{
				spawnPed("Ambient", 1116.1f, 253.782f, -50.44075f, 118.4373f, Class1.HallwayBPeds);
				spawnPed("Ambient", 1115.047f, 252.0814f, -50.44075f, 0.61928f, Class1.HallwayBPeds);
			}
			spawnPed("Ambient", 1123.927f, 252.0391f, -50.04012f, 263.6153f, Class1.HallwayBPeds);
			spawnPed("Ambient", 1125.315f, 251.6236f, -50.0408f, 59.87027f, Class1.HallwayBPeds);
			spawnPed("Ambient", 1124.225f, 250.6693f, -50.04081f, 353.9799f, Class1.HallwayBPeds);
			spawnPed("Ambient", 1119.54f, 250.8569f, -50.44069f, 141.6542f, Class1.HallwayBPeds);
			spawnPed("Ambient", 1119.197f, 249.3849f, -50.44073f, 342.2236f, Class1.HallwayBPeds);
			spawnPed("Ambient", 1118.603f, 252.5937f, -50.44075f, 296.5069f, Class1.HallwayBPeds);
			spawnPed("Ambient", 1119.089f, 253.6974f, -50.44075f, 102.2817f, Class1.HallwayBPeds);
			spawnPed("Ambient", 1118.214f, 253.9731f, -50.44075f, 210.3038f, Class1.HallwayBPeds);
		}

		public static void DeleteGamingAreaPeds()
		{
			foreach (Ped gamingAreaPed in Class1.GamingAreaPeds)
			{
				if (gamingAreaPed != null)
				{
					gamingAreaPed.Delete();
				}
			}
		}

		public static void SpawnGamingAreaPeds()
		{
			spawnPed("Ambient_Sit_Out_Of_Money", 1155.592f, 248.2551f, -51.04082f, 94.77995f, Class1.GamingAreaPeds);
			spawnPed("Ambient_Sit_Out_Of_Money", 1130.188f, 273.55f, -51.04081f, 186.3109f, Class1.GamingAreaPeds);
			spawnPed("Bathroom_Urinal", 1130.064f, 282.8449f, -51.04082f, 89.83972f, Class1.GamingAreaPeds);
			spawnPed("Bathroom_Wash", 1133.091f, 276.0128f, -51.04082f, 270f, Class1.GamingAreaPeds);
			spawnPed("Bathroom_WashFace", 1157.104f, 251.1387f, -51.04082f, 0f, Class1.GamingAreaPeds);
			spawnPed("Security", 1145.026f, 240.6712f, -51.04081f, 359.3594f, Class1.GamingAreaPeds);
			spawnPed("Security", 1100.777f, 253.1441f, -50.84097f, 219.6611f, Class1.GamingAreaPeds);
			spawnPed("Security", 1134.203f, 240.781f, -50.04079f, 196.4238f, Class1.GamingAreaPeds);
			spawnPed("Security", 1122.674f, 252.0884f, -50.0408f, 76.07817f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1133.55f, 244.1342f, -50.04084f, 110.0729f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1132.542f, 243.5671f, -50.04084f, 278.2481f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1133.28f, 242.7522f, -50.04084f, 4.048811f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1140.325f, 241.4782f, -50.44083f, 13.21978f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1139.431f, 242.8266f, -50.44083f, 270.3011f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1140.482f, 242.6203f, -50.44083f, 117.5287f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1137.119f, 248.1009f, -51.02732f, 330.5509f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1137.217f, 249.6406f, -51.03574f, 162.3815f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1136.121f, 249.2994f, -51.03574f, 237.8266f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1129.406f, 252.7254f, -51.03574f, 73.00798f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1128.097f, 253.2899f, -50.56031f, 217.2823f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1128.963f, 253.8419f, -50.94689f, 150.1592f, Class1.GamingAreaPeds);
			if (!UseLessPeds)
			{
				spawnPed("Ambient", 1131.697f, 253.8458f, -51.03035f, 259.3825f, Class1.GamingAreaPeds);
				spawnPed("Ambient", 1132.742f, 253.1228f, -51.03574f, 318.444f, Class1.GamingAreaPeds);
				spawnPed("Ambient", 1132.737f, 254.1611f, -51.03574f, 129.2212f, Class1.GamingAreaPeds);
			}
			if (!UseLessPeds)
			{
				spawnPed("Ambient", 1134.319f, 251.0581f, -51.03574f, 191.6853f, Class1.GamingAreaPeds);
				spawnPed("Ambient", 1134.854f, 250.1977f, -51.03574f, 119.9588f, Class1.GamingAreaPeds);
			}
			spawnPed("Ambient", 1137.579f, 255.2417f, -51.03574f, 305.2039f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1139.152f, 256.0306f, -51.03574f, 37.08674f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1138.175f, 256.8043f, -51.03574f, 172.1775f, Class1.GamingAreaPeds);
			if (!UseLessPeds)
			{
				spawnPed("Ambient", 1137.167f, 260.4806f, -51.44135f, 304.1956f, Class1.GamingAreaPeds);
				spawnPed("Ambient", 1138.751f, 261.2563f, -51.44138f, 137.3517f, Class1.GamingAreaPeds);
				spawnPed("Ambient", 1138.225f, 259.5233f, -51.44085f, 33.42665f, Class1.GamingAreaPeds);
			}
			spawnPed("Ambient", 1137.765f, 265.8468f, -51.39021f, 329.5708f, Class1.GamingAreaPeds);
			if (!UseLessPeds)
			{
				spawnPed("Ambient", 1138.495f, 267.7196f, -51.44209f, 292.0003f, Class1.GamingAreaPeds);
			}
			spawnPed("Ambient", 1135.291f, 271.3358f, -51.04082f, 45.25751f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1133.935f, 271.8986f, -51.04082f, 279.1727f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1128.204f, 271.1629f, -51.04082f, 104.7433f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1126.975f, 270.5958f, -51.04082f, 273.1444f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1127.822f, 270.3361f, -51.04082f, 337.4778f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1126.82f, 263.4178f, -51.03801f, 330.2697f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1127.367f, 265.0264f, -51.03577f, 181.5444f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1128.137f, 264.2766f, -51.03577f, 75.77078f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1130.203f, 264.313f, -51.03573f, 159.9315f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1131.39f, 264.0587f, -51.03574f, 152.624f, Class1.GamingAreaPeds);
			if (!UseLessPeds)
			{
				spawnPed("Ambient", 1134.923f, 262.0023f, -51.03574f, 102.0767f, Class1.GamingAreaPeds);
				spawnPed("Ambient", 1133.688f, 260.8722f, -51.03574f, 303.568f, Class1.GamingAreaPeds);
				spawnPed("Ambient", 1133.737f, 261.9765f, -51.03574f, 230.5615f, Class1.GamingAreaPeds);
			}
			spawnPed("Ambient", 1144.257f, 256.539f, -51.4449f, 249.307f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1145.354f, 257.3246f, -51.44085f, 110.9575f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1144.189f, 253.4209f, -51.034f, 118.1228f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1142.842f, 252.3358f, -51.03574f, 318.9642f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1144.899f, 251.3764f, -51.03574f, 239.8168f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1146.114f, 251.3639f, -51.03574f, 157.1451f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1148.701f, 241.1792f, -51.04082f, 130.2524f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1147.304f, 241.1551f, -51.04082f, 262.6589f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1147.801f, 241.9699f, -51.04082f, 205.8803f, Class1.GamingAreaPeds);
			if (!UseLessPeds)
			{
				spawnPed("Ambient", 1149.323f, 260.804f, -51.84083f, 254.0134f, Class1.GamingAreaPeds);
				spawnPed("Ambient", 1151.279f, 261.266f, -51.84083f, 46.6459f, Class1.GamingAreaPeds);
				spawnPed("Ambient", 1149.845f, 262.1045f, -51.84083f, 190.0181f, Class1.GamingAreaPeds);
			}
			spawnPed("Ambient", 1153.759f, 264.2555f, -51.84083f, 119.4967f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1152.716f, 263.4677f, -51.84083f, 17.02253f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1152.883f, 264.7525f, -51.84083f, 199.0661f, Class1.GamingAreaPeds);
			if (!UseLessPeds)
			{
				spawnPed("Ambient", 1150.303f, 264.3685f, -51.84084f, 75.40145f, Class1.GamingAreaPeds);
				spawnPed("Ambient", 1149.831f, 265.7737f, -51.84084f, 302.2845f, Class1.GamingAreaPeds);
			}
			if (!UseLessPeds)
			{
				spawnPed("Ambient", 1151.276f, 265.3188f, -51.84084f, 77.28851f, Class1.GamingAreaPeds);
				spawnPed("Ambient", 1152.451f, 269.7397f, -51.84084f, 56.79968f, Class1.GamingAreaPeds);
				spawnPed("Ambient", 1151.26f, 270.751f, -51.84084f, 244.0864f, Class1.GamingAreaPeds);
			}
			spawnPed("Ambient", 1098.956f, 250.0602f, -50.84063f, 107.5749f, Class1.GamingAreaPeds);
			spawnPed("Ambient", 1097.173f, 249.787f, -50.84085f, 259.2625f, Class1.GamingAreaPeds);
			if (!UseLessPeds)
			{
				spawnPed("Ambient", 1098.818f, 250.6817f, -50.84088f, 231.1119f, Class1.GamingAreaPeds);
			}
		}

		public static void CreateSecondaryPeds()
		{
			Notification.Show("Created Peds");
		}

		public static void applyGuardVariation(Ped ped)
		{
			if (ped == null)
			{
				return;
			}
			Random random = new Random();
			Function.Call(Hash.SET_PED_RANDOM_PROPS, ped);
			switch (random.Next(1, 5))
			{
			case 1:
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 0, 0, 1);
				break;
			case 2:
			{
				switch (random.Next(1, 3))
				{
				case 0:
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 1, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 2, 1);
					break;
				case 1:
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 1, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 5, 1);
					break;
				case 2:
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 1, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 1, 1);
					break;
				}
				int num4 = random.Next(0, 2);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, num4, 1);
				break;
			}
			case 3:
			{
				int num3 = random.Next(0, 2);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 2, num3, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 3, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 2, 0, 1);
				break;
			}
			case 4:
			{
				switch (random.Next(1, 3))
				{
				case 0:
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 3, 1);
					break;
				case 1:
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 3, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 2, 1);
					break;
				case 2:
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 3, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 0, 1);
					break;
				}
				int num2 = random.Next(0, 2);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 3, num2, 1);
				break;
			}
			case 5:
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 4, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 0, 1);
				int num = random.Next(0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 4, num, 1);
				break;
			}
			}
			switch (random.Next(1, 2))
			{
			case 1:
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				break;
			case 2:
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 1, 0, 1);
				break;
			}
			int num5 = random.Next(1, 100);
			if (num5 < 80)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
			}
			else if (num5 >= 80)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 2, 0, 1);
			}
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
		}

		public static void applyStaffVariation(Ped ped)
		{
			Function.Call(Hash.SET_PED_RANDOM_PROPS, ped);
			if (Function.Call<bool>(Hash.IS_PED_MALE, ped))
			{
				switch (rnd.Next(1, 5))
				{
				case 1:
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 0, 0, 1);
					break;
				case 2:
				{
					switch (rnd.Next(1, 3))
					{
					case 0:
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 1, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 2, 1);
						break;
					case 1:
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 1, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 5, 1);
						break;
					case 2:
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 1, 2, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 1, 1);
						break;
					}
					int num4 = rnd.Next(0, 2);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, num4, 1);
					break;
				}
				case 3:
				{
					int num3 = rnd.Next(0, 2);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 2, num3, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 3, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 2, 0, 1);
					break;
				}
				case 4:
				{
					switch (rnd.Next(1, 3))
					{
					case 0:
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 3, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 3, 1);
						break;
					case 1:
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 3, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 2, 1);
						break;
					case 2:
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 3, 2, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 0, 1);
						break;
					}
					int num2 = rnd.Next(0, 2);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 3, num2, 1);
					break;
				}
				case 5:
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 4, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 0, 1);
					int num = rnd.Next(0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 4, num, 1);
					break;
				}
				}
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
			}
			else
			{
				int num5 = rnd.Next(0, 3);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, num5, 0, 1);
				if (num5 == 0 || num5 == 2)
				{
					int num6 = rnd.Next(1, 2);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, num6, 0, 1);
				}
				else
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 2, 0, 1);
				}
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				int num7 = rnd.Next(0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, num7, 0, 1);
				int num8 = rnd.Next(0, 5);
				int num9 = rnd.Next(0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, num8, num9, 1);
				switch (num8)
				{
				case 0:
				{
					int num15 = rnd.Next(0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 0, num15, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 1, 0, 1);
					break;
				}
				case 1:
				{
					int num14 = rnd.Next(0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, num14, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 1, 3, 1);
					break;
				}
				case 2:
				{
					int num13 = rnd.Next(0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 2, num13, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 1, 3, 1);
					break;
				}
				case 3:
				{
					int num12 = rnd.Next(0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 3, num12, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 1, 1, 1);
					break;
				}
				case 4:
				{
					int num11 = rnd.Next(0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 4, num11, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 1, 1, 1);
					break;
				}
				case 5:
				{
					int num10 = rnd.Next(0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 5, num10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 1, 2, 1);
					break;
				}
				}
			}
		}

		public static void spawnPed(string type, float x1, float y1, float z1, float rot, List<Ped> Peds)
		{
			try
			{
				string text = ambientPedModelList[rnd.Next(ambientPedModelList.Count)];
				if (type == "Ambient")
				{
					Ped ped = GlobalHelper.CreatePed(RequestModel(text), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped, true);
					assignStandAnimation(ped, "Stand_Base");
					Peds.Add(ped);
				}
				if (type == "sunbather")
				{
					text = PenthouseExteriorPeds.swimwearPedModelList[rnd.Next(PenthouseExteriorPeds.swimwearPedModelList.Count)];
					Ped ped2 = GlobalHelper.CreatePed(RequestModel(text), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped2, true);
					assignStandAnimation(ped2, "sunbather");
					Peds.Add(ped2);
				}
				switch (type)
				{
				case "jacuzzi":
				{
					text = PenthouseExteriorPeds.swimwearPedModelList[rnd.Next(PenthouseExteriorPeds.swimwearPedModelList.Count)];
					Ped ped6 = GlobalHelper.CreatePed(RequestModel(text), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped6, true);
					assignStandAnimation(ped6, "jacuzzi");
					Peds.Add(ped6);
					break;
				}
				case "Ambient_Convo":
				{
					Ped ped5 = GlobalHelper.CreatePed(RequestModel(text), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped5, true);
					assignStandAnimation(ped5, "Stand_Convo");
					Peds.Add(ped5);
					break;
				}
				case "Ambient_Sitting":
				{
					text = ambientPedModelList[rnd.Next(ambientPedModelList.Count)];
					Ped ped4 = GlobalHelper.CreatePed(RequestModel(text), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped4, true);
					assignStandAnimation(ped4, "Sit_Generic");
					int handle = ped4.Handle;
					Peds.Add(ped4);
					break;
				}
				case "Ambient_Slots":
				{
					text = ambientPedModelList[rnd.Next(ambientPedModelList.Count)];
					Ped ped3 = GlobalHelper.CreatePed(RequestModel(text), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped3, true);
					assignStandAnimation(ped3, "Sit_Slots");
					int handle2 = ped3.Handle;
					Peds.Add(ped3);
					break;
				}
				}
				if (type == "Ambient_Shop")
				{
					text = ambientPedModelList[rnd.Next(ambientPedModelList.Count)];
					Ped ped7 = GlobalHelper.CreatePed(RequestModel(text), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped7, true);
					assignStandAnimation(ped7, "Shop");
					Peds.Add(ped7);
				}
				if (type == "Ambient_Phone")
				{
					text = ambientPedModelList[rnd.Next(ambientPedModelList.Count)];
					Ped ped8 = GlobalHelper.CreatePed(RequestModel(text), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped8, true);
					assignStandAnimation(ped8, "Mobile");
					Peds.Add(ped8);
				}
				if (type == "Ambient_Sit_Out_Of_Money")
				{
					text = ambientPedModelList[rnd.Next(ambientPedModelList.Count)];
					Ped ped9 = GlobalHelper.CreatePed(RequestModel(text), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped9, true);
					assignStandAnimation(ped9, "Out_Of_Money_Sit");
					Peds.Add(ped9);
				}
				if (type == "Bathroom_Urinal")
				{
					text = ambientPedModelList[rnd.Next(ambientPedModelList.Count)];
					if (text == "A_F_Y_GenCasPat_01" || text == "A_F_Y_SmartCasPat_01")
					{
						text = "A_M_Y_SmartCasPat_01";
					}
					Ped ped10 = GlobalHelper.CreatePed(RequestModel(text), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped10, true);
					assignStandAnimation(ped10, "Peeing");
					Peds.Add(ped10);
				}
				if (type == "Bathroom_Wash")
				{
					if (text == "A_F_Y_GenCasPat_01" || text == "A_F_Y_SmartCasPat_01")
					{
						text = "A_M_Y_SmartCasPat_01";
					}
					text = ambientPedModelList[rnd.Next(ambientPedModelList.Count)];
					Ped ped11 = GlobalHelper.CreatePed(RequestModel(text), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped11, true);
					assignStandAnimation(ped11, "Washing_Hands");
					Peds.Add(ped11);
				}
				switch (type)
				{
				case "Bathroom_WashFace":
				{
					if (text == "A_F_Y_GenCasPat_01" || text == "A_F_Y_SmartCasPat_01")
					{
						text = "A_M_Y_SmartCasPat_01";
					}
					text = ambientPedModelList[rnd.Next(ambientPedModelList.Count)];
					Ped ped28 = GlobalHelper.CreatePed(RequestModel(text), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped28, true);
					assignStandAnimation(ped28, "Washing_Face");
					int handle3 = ped28.Handle;
					Peds.Add(ped28);
					break;
				}
				case "Security":
				{
					Ped ped27 = GlobalHelper.CreatePed(RequestModel("s_m_y_casino_01"), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped27, true);
					assignStandAnimation(ped27, "Guard");
					int handle4 = ped27.Handle;
					applyGuardVariation(ped27);
					Peds.Add(ped27);
					break;
				}
				case "Security_Convo":
				{
					Ped ped26 = GlobalHelper.CreatePed(RequestModel("s_m_y_casino_01"), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped26, true);
					assignStandAnimation(ped26, "Stand_Convo");
					Peds.Add(ped26);
					applyGuardVariation(ped26);
					break;
				}
				case "Security_Amb":
				{
					Ped ped25 = GlobalHelper.CreatePed(RequestModel("s_m_y_casino_01"), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped25, true);
					assignStandAnimation(ped25, "Stand_Base");
					Peds.Add(ped25);
					applyGuardVariation(ped25);
					break;
				}
				case "Security_Mobile":
				{
					Ped ped24 = GlobalHelper.CreatePed(RequestModel("s_m_y_casino_01"), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped24, true);
					assignStandAnimation(ped24, "Mobile");
					Peds.Add(ped24);
					applyGuardVariation(ped24);
					break;
				}
				case "Security_Booth":
				{
					Ped ped23 = GlobalHelper.CreatePed(RequestModel("s_m_y_casino_01"), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped23, true);
					assignStandAnimation(ped23, "Guard");
					Peds.Add(ped23);
					applyGuardVariation(ped23);
					break;
				}
				case "Security_Shop":
				{
					Ped ped22 = GlobalHelper.CreatePed(RequestModel("s_m_y_casino_01"), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped22, true);
					assignStandAnimation(ped22, "Shop");
					Peds.Add(ped22);
					applyGuardVariation(ped22);
					break;
				}
				case "Security_Drink":
				{
					Ped ped21 = GlobalHelper.CreatePed(RequestModel("s_m_y_casino_01"), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped21, true);
					assignStandAnimation(ped21, "Drink");
					Peds.Add(ped21);
					applyGuardVariation(ped21);
					break;
				}
				case "Staff_Female_Computer":
				{
					Ped ped20 = GlobalHelper.CreatePed(RequestModel("s_f_y_casino_01"), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped20, true);
					assignStandAnimation(ped20, "Computer");
					Peds.Add(ped20);
					applyStaffVariation(ped20);
					break;
				}
				case "Staff_Convo":
				{
					text = staffPedModelList[rnd.Next(staffPedModelList.Count)];
					Ped ped19 = GlobalHelper.CreatePed(RequestModel(text), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped19, true);
					assignStandAnimation(ped19, "Stand_Convo");
					Peds.Add(ped19);
					applyStaffVariation(ped19);
					break;
				}
				case "Staff_Phone":
				{
					text = staffPedModelList[rnd.Next(staffPedModelList.Count)];
					Ped ped18 = GlobalHelper.CreatePed(RequestModel(text), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped18, true);
					assignStandAnimation(ped18, "Mobile");
					Peds.Add(ped18);
					applyStaffVariation(ped18);
					break;
				}
				case "Staff_Dealer":
				{
					text = staffPedModelList[rnd.Next(staffPedModelList.Count)];
					Ped ped17 = GlobalHelper.CreatePed(RequestModel(text), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped17, true);
					assignStandAnimation(ped17, "Dealer");
					Peds.Add(ped17);
					applyStaffVariation(ped17);
					break;
				}
				case "Repairmen":
				{
					Ped ped16 = GlobalHelper.CreatePed(RequestModel("s_m_y_winclean_01"), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped16, true);
					assignStandAnimation(ped16, "Impatient");
					Peds.Add(ped16);
					break;
				}
				case "Repairmen2":
				{
					Ped ped15 = GlobalHelper.CreatePed(RequestModel("s_m_y_construct_02"), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped15, true);
					assignStandAnimation(ped15, "Stand_Convo");
					Peds.Add(ped15);
					break;
				}
				case "Valet2":
				{
					Ped ped14 = GlobalHelper.CreatePed(RequestModel("s_m_y_valet_01"), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped14, true);
					assignStandAnimation(ped14, "Hangout");
					Peds.Add(ped14);
					break;
				}
				case "Hooker":
				{
					Ped ped13 = GlobalHelper.CreatePed(RequestModel("s_f_y_hooker_01"), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped13, true);
					assignStandAnimation(ped13, "Hooker");
					Peds.Add(ped13);
					break;
				}
				case "Hooker_Male":
				{
					Ped ped12 = GlobalHelper.CreatePed(RequestModel("g_m_m_casrn_01"), new Vector3(x1, y1, z1 - 1f), rot);
					assignPedProperties(ped12, true);
					assignStandAnimation(ped12, "Stand_Convo");
					Peds.Add(ped12);
					break;
				}
				}
			}
			catch
			{
			}
		}

		public static void SetPedAnim(Ped ped, string type)
		{
			switch (type)
			{
			case "Ambient":
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Stand_Base");
				break;
			case "Ambient_Convo":
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Stand_Convo");
				break;
			case "Ambient_Sitting":
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Sit_Generic");
				break;
			case "Ambient_Slots":
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Sit_Slots");
				break;
			}
			if (type == "Ambient_Shop")
			{
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Shop");
			}
			if (type == "Ambient_Phone")
			{
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Mobile");
			}
			if (type == "Ambient_Sit_Out_Of_Money")
			{
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Out_Of_Money_Sit");
			}
			if (type == "Bathroom_Urinal")
			{
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Peeing");
			}
			if (type == "Bathroom_Wash")
			{
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Washing_Hands");
			}
			switch (type)
			{
			case "Bathroom_WashFace":
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Washing_Face");
				break;
			case "Security":
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Guard");
				applyGuardVariation(ped);
				break;
			case "Security_Convo":
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Stand_Convo");
				applyGuardVariation(ped);
				break;
			case "Security_Amb":
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Stand_Base");
				applyGuardVariation(ped);
				break;
			case "Security_Mobile":
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Mobile");
				applyGuardVariation(ped);
				break;
			case "Security_Booth":
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Guard");
				applyGuardVariation(ped);
				break;
			case "Security_Shop":
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Shop");
				applyGuardVariation(ped);
				break;
			case "Security_Drink":
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Drink");
				applyGuardVariation(ped);
				break;
			case "Staff_Female_Computer":
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Computer");
				applyStaffVariation(ped);
				break;
			case "Staff_Convo":
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Stand_Convo");
				applyStaffVariation(ped);
				break;
			case "Staff_Phone":
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Mobile");
				applyStaffVariation(ped);
				break;
			case "Staff_Dealer":
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Dealer");
				applyStaffVariation(ped);
				break;
			case "Repairmen":
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Impatient");
				break;
			case "Repairmen2":
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Stand_Convo");
				break;
			case "Valet2":
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Hangout");
				break;
			case "Hooker":
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Hooker");
				break;
			default:
				assignPedProperties(ped, true);
				assignStandAnimation(ped, "Stand_Convo");
				break;
			}
		}

		public static void assignPedProperties(Ped pedo, bool freeze)
		{
			if (!(pedo == null))
			{
				Function.Call(Hash.SET_PED_CAN_EVASIVE_DIVE, pedo, 0);
				Function.Call(Hash.SET_PED_CAN_RAGDOLL_FROM_PLAYER_IMPACT, pedo, 0);
				Function.Call(Hash.SET_PED_RESET_FLAG, pedo, 249, true);
				Function.Call(Hash.SET_PED_CONFIG_FLAG, pedo, 185, true);
				Function.Call(Hash.SET_PED_CONFIG_FLAG, pedo, 108, true);
				Function.Call(Hash.SET_PED_CONFIG_FLAG, pedo, 106, true);
				Function.Call(Hash.SET_PED_CONFIG_FLAG, pedo, 208, true);
				Function.Call(Hash.SET_TREAT_AS_AMBIENT_PED_FOR_DRIVER_LOCKON, pedo, 1);
				Function.Call(Hash.SET_PED_CONFIG_FLAG, pedo, 208, true);
				Function.Call(Hash.SET_PED_CAN_BE_TARGETTED, pedo, false);
				Function.Call(Hash.SET_PED_CAN_RAGDOLL, pedo, false);
				Function.Call(Hash.SET_ENTITY_IS_TARGET_PRIORITY, pedo, false, false);
				Function.Call(Hash.SET_PED_CAN_BE_TARGETED_WITHOUT_LOS, pedo, false);
				Function.Call(Hash.SET_ENTITY_CAN_BE_DAMAGED, pedo, false);
				Function.Call(Hash.SET_PED_AS_ENEMY, pedo, false);
				pedo.AlwaysKeepTask = true;
				pedo.BlockPermanentEvents = true;
				if (freeze)
				{
					Function.Call(Hash.FREEZE_ENTITY_POSITION, pedo, true);
				}
			}
		}

		public unsafe static void assignStandAnimation(Ped ped, string type)
		{
			if (ped == null || !Function.Call<bool>(Hash.DOES_ENTITY_EXIST, ped))
			{
				return;
			}
			switch (type)
			{
			case "Stand_Base":
				if (Function.Call<bool>(Hash.IS_PED_MALE, ped))
				{
					string text5 = ped_MaleIdleIdlesDict[rnd.Next(ped_MaleIdleIdlesDict.Count)];
					string text6 = ped_ConvoIdlesClip[rnd.Next(ped_ConvoIdlesClip.Count)];
					Class1.LoadDict(text5);
					Function.Call(Hash.TASK_PLAY_ANIM, ped, text5, text6, 1000f, -4f, -1, 1, 0, 0, 0, 0);
				}
				else if (!Function.Call<bool>(Hash.IS_PED_MALE, ped))
				{
					string text7 = ped_FemaleIdleIdlesDict[rnd.Next(ped_MaleIdleIdlesDict.Count)];
					string text8 = ped_ConvoIdlesClip[rnd.Next(ped_ConvoIdlesClip.Count)];
					Class1.LoadDict(text7);
					Function.Call(Hash.TASK_PLAY_ANIM, ped, text7, text8, 1000f, -4f, -1, 1, 0, 0, 0, 0);
				}
				break;
			case "Stand_Idles":
				if (!Function.Call<bool>(Hash.IS_PED_MALE, ped))
				{
					Function.Call<bool>(Hash.IS_PED_MALE, ped);
				}
				break;
			case "Stand_WithDrink":
				if (!Function.Call<bool>(Hash.IS_PED_MALE, ped))
				{
					Function.Call<bool>(Hash.IS_PED_MALE, ped);
				}
				break;
			case "Stand_Convo":
				if (Function.Call<bool>(Hash.IS_PED_MALE, ped))
				{
					string text = ped_MaleConvoIdleDict[rnd.Next(ped_MaleConvoIdleDict.Count)];
					string text2 = ped_ConvoIdlesClip[rnd.Next(ped_ConvoIdlesClip.Count)];
					Class1.LoadDict(text);
					Function.Call(Hash.TASK_PLAY_ANIM, ped, text, text2, 1000f, -4f, -1, 1, 0, 0, 0, 0);
				}
				else if (!Function.Call<bool>(Hash.IS_PED_MALE, ped))
				{
					string text3 = ped_FemaleConvoIdleDict[rnd.Next(ped_FemaleConvoIdleDict.Count)];
					string text4 = ped_ConvoIdlesClip[rnd.Next(ped_ConvoIdlesClip.Count)];
					Class1.LoadDict(text3);
					Function.Call(Hash.TASK_PLAY_ANIM, ped, text3, text4, 1000f, -4f, -1, 1, 0, 0, 0, 0);
				}
				break;
			case "Shop":
				Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, ped, "WORLD_HUMAN_WINDOW_SHOP_BROWSE", -1, false);
				break;
			case "Mobile":
				Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, ped, "WORLD_HUMAN_STAND_MOBILE", -1, false);
				break;
			case "Guard":
			{
				Class1.LoadDict("anim@move_m@security_guard");
				Class1.LoadDict("amb@world_human_stand_guard@male@enter");
				int num4 = 0;
				Function.Call(Hash.OPEN_SEQUENCE_TASK, &num4);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@world_human_stand_guard@male@enter", "enter", 2f, 2f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@world_human_stand_guard@male@base", "base", 2f, 2f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@world_human_stand_guard@male@idle_a", "idle_c", 2f, 2f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@world_human_stand_guard@male@base", "base", 2f, 2f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@world_human_stand_guard@male@idle_a", "idle_b", 2f, 2f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@world_human_stand_guard@male@base", "base", 2f, 2f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@world_human_stand_guard@male@idle_a", "idle_a", 2f, 2f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@world_human_stand_guard@male@base", "base", 2f, 2f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@world_human_stand_guard@male@exit", "exit", 2f, 2f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@move_m@security_guard", "idle", 6f, 1f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@move_m@security_guard", "idle_var_01", 6f, 1f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@move_m@security_guard", "idle", 6f, 1f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@move_m@security_guard", "idle_var_05", 6f, 1f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@move_m@security_guard", "idle", 6f, 1f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@move_m@security_guard", "idle_var_04", 6f, 1f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@move_m@security_guard", "idle", 6f, 1f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@move_m@security_guard", "idle_var_02", 6f, 1f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@move_m@security_guard", "idle", 6f, 1f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@move_m@security_guard", "idle_var_03", 6f, 1f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.SET_SEQUENCE_TO_REPEAT, num4, true);
				Function.Call(Hash.CLOSE_SEQUENCE_TASK, num4);
				Function.Call(Hash.TASK_PERFORM_SEQUENCE, ped, num4);
				Function.Call(Hash.CLEAR_SEQUENCE_TASK, &num4);
				break;
			}
			case "Guard_Patrol":
				Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, ped, "WORLD_HUMAN_GUARD_PATROL", -1, false);
				break;
			case "Impatient":
				Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, ped, "WORLD_HUMAN_STAND_IMPATIENT", -1, false);
				break;
			case "Hangout":
				Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, ped, "WORLD_HUMAN_HANG_OUT_STREET", -1, false);
				break;
			case "Drink":
				Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, ped, "WORLD_HUMAN_DRINKING", -1, false);
				break;
			case "Out_Of_Money_Sit":
			{
				Class1.LoadDict("anim@amb@casino@out_of_money@ped_male@02b@base");
				Class1.LoadDict("anim@amb@casino@out_of_money@ped_male@02b@idles");
				Class1.LoadDict("anim@amb@casino@out_of_money@ped_female@02b@base");
				Class1.LoadDict("anim@amb@casino@out_of_money@ped_female@02b@idles");
				int num7 = 0;
				Function.Call(Hash.OPEN_SEQUENCE_TASK, &num7);
				if (Function.Call<bool>(Hash.IS_PED_MALE, ped))
				{
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@casino@out_of_money@ped_male@02b@base", "base", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@casino@out_of_money@ped_male@02b@idles", "idle_b", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@casino@out_of_money@ped_male@02b@base", "base", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@casino@out_of_money@ped_male@02b@base", "base", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@casino@out_of_money@ped_male@02b@idles", "idle_a", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@casino@out_of_money@ped_male@02b@idles", "idle_c", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@casino@out_of_money@ped_male@02b@base", "base", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@casino@out_of_money@ped_male@02b@base", "base", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@casino@out_of_money@ped_male@02b@idles", "idle_d", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@casino@out_of_money@ped_male@02b@base", "base", 2f, 2f, -1, 0, -1f, true, true, true);
				}
				else
				{
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@casino@out_of_money@ped_female@02b@base", "base", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@casino@out_of_money@ped_female@02b@idles", "idle_b", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@casino@out_of_money@ped_female@02b@base", "base", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@casino@out_of_money@ped_female@02b@base", "base", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@casino@out_of_money@ped_female@02b@idles", "idle_a", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@casino@out_of_money@ped_female@02b@idles", "idle_c", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@casino@out_of_money@ped_female@02b@base", "base", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@casino@out_of_money@ped_female@02b@base", "base", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@casino@out_of_money@ped_female@02b@idles", "idle_d", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@casino@out_of_money@ped_female@02b@base", "base", 2f, 2f, -1, 0, -1f, true, true, true);
				}
				Function.Call(Hash.SET_SEQUENCE_TO_REPEAT, num7, true);
				Function.Call(Hash.CLOSE_SEQUENCE_TASK, num7);
				Function.Call(Hash.TASK_PERFORM_SEQUENCE, ped, num7);
				Function.Call(Hash.CLEAR_SEQUENCE_TASK, &num7);
				break;
			}
			case "Peeing":
			{
				Class1.LoadDict("missheist_agency3aig_23");
				int num6 = 0;
				Function.Call(Hash.OPEN_SEQUENCE_TASK, &num6);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "missheist_agency3aig_23", "urinal_base", 2f, 2f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "missheist_agency3aig_23", "urinal_asifyouneededproof", 2f, 2f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "missheist_agency3aig_23", "urinal_base", 2f, 2f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "missheist_agency3aig_23", "urinal_base", 2f, 2f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "missheist_agency3aig_23", "urinal_base", 2f, 2f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "missheist_agency3aig_23", "urinal_dontstartmopping", 2f, 2f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "missheist_agency3aig_23", "urinal_base", 2f, 2f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "missheist_agency3aig_23", "urinal_base", 2f, 2f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "missheist_agency3aig_23", "urinal_youknowyoustayed", 2f, 2f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.TASK_PLAY_ANIM, 0, "missheist_agency3aig_23", "urinal_base", 2f, 2f, -1, 0, -1f, true, true, true);
				Function.Call(Hash.SET_SEQUENCE_TO_REPEAT, num6, true);
				Function.Call(Hash.CLOSE_SEQUENCE_TASK, num6);
				Function.Call(Hash.TASK_PERFORM_SEQUENCE, ped, num6);
				Function.Call(Hash.CLEAR_SEQUENCE_TASK, &num6);
				break;
			}
			case "Washing_Hands":
				Function.Call(Hash.TASK_PLAY_ANIM, ped, "missheist_agency3aig_23", "urinal_sink_loop", 2f, 2f, -1, 1, -1f, true, true, true);
				break;
			case "Washing_Face":
				Function.Call(Hash.TASK_PLAY_ANIM, ped, "missmic2_washing_face", "michael_washing_face", 2f, 2f, -1, 1, -1f, true, true, true);
				break;
			case "Hooker":
				Function.Call(Hash.TASK_PLAY_ANIM, ped, "switch@michael@prostitute", "base_hooker", 1000f, -4f, -1, 1, 0, 0, 0, 0);
				break;
			case "Computer":
				if (Function.Call<bool>(Hash.IS_PED_MALE, ped))
				{
					Function.Call(Hash.TASK_PLAY_ANIM, ped, "amb@prop_human_seat_computer@male@idle_a", "idle_a", 1000f, -4f, -1, 1, 0, 0, 0, 0);
				}
				else if (!Function.Call<bool>(Hash.IS_PED_MALE, ped))
				{
					Function.Call(Hash.TASK_PLAY_ANIM, ped, "amb@prop_human_seat_computer@female@idle_a", "idle_a", 1000f, -4f, -1, 1, 0, 0, 0, 0);
				}
				break;
			case "Dealer":
			{
				int num5 = 0;
				Function.Call(Hash.OPEN_SEQUENCE_TASK, &num5);
				if (Function.Call<bool>(Hash.IS_PED_MALE, ped))
				{
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "idle", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "idle_var_02", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "idle", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "idle", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "idle_var_04", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "idle_var_01", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "idle", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "idle_var_05", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "acknowledge_p01", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "idle", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "idle_var_03", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "idle", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "acknowledge_p03", 2f, 2f, -1, 0, -1f, true, true, true);
				}
				else
				{
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "female_idle", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "female_idle_var_05", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "female_idle", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "female_idle", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "female_idle_var_02", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "female_idle_var_04", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "female_idle", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "female_idle_var_01", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "female_acknowledge_p04", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "female_idle", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "female_idle_var_05", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "female_idle_var_03", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim_casino_b@amb@casino@games@shared@dealer@", "female_acknowledge_p02", 2f, 2f, -1, 0, -1f, true, true, true);
				}
				Function.Call(Hash.SET_SEQUENCE_TO_REPEAT, num5, true);
				Function.Call(Hash.CLOSE_SEQUENCE_TASK, num5);
				Function.Call(Hash.TASK_PERFORM_SEQUENCE, ped, num5);
				Function.Call(Hash.CLEAR_SEQUENCE_TASK, &num5);
				break;
			}
			case "sunbather":
			{
				Class1.LoadDict("amb@prop_human_seat_sunlounger@male@base");
				Class1.LoadDict("amb@prop_human_seat_sunlounger@male@idle_a");
				Class1.LoadDict("amb@prop_human_seat_sunlounger@female@base");
				Class1.LoadDict("amb@prop_human_seat_sunlounger@female@idle_a");
				int num8 = 0;
				Function.Call(Hash.OPEN_SEQUENCE_TASK, &num8);
				if (Function.Call<bool>(Hash.IS_PED_MALE, ped))
				{
					List<string> list2 = new List<string>();
					list2.Add("a");
					list2.Add("b");
					list2.Add("c");
					list2.Add("d");
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@male@base", "base", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@male@idle_a", "idle_" + list2[rnd.Next(0, list2.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@male@idle_a", "idle_" + list2[rnd.Next(0, list2.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@male@idle_a", "idle_" + list2[rnd.Next(0, list2.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@male@idle_a", "idle_" + list2[rnd.Next(0, list2.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@male@base", "base", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@male@idle_a", "idle_" + list2[rnd.Next(0, list2.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@male@idle_a", "idle_" + list2[rnd.Next(0, list2.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@male@idle_a", "idle_" + list2[rnd.Next(0, list2.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@male@idle_a", "idle_" + list2[rnd.Next(0, list2.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@male@base", "base", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@male@idle_a", "idle_" + list2[rnd.Next(0, list2.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@male@idle_a", "idle_" + list2[rnd.Next(0, list2.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@male@idle_a", "idle_" + list2[rnd.Next(0, list2.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@male@idle_a", "idle_" + list2[rnd.Next(0, list2.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
				}
				else
				{
					List<string> list3 = new List<string>();
					list3.Add("a");
					list3.Add("b");
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@female@base", "base", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@female@idle_a", "idle_" + list3[rnd.Next(0, list3.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@female@idle_a", "idle_" + list3[rnd.Next(0, list3.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@female@idle_a", "idle_" + list3[rnd.Next(0, list3.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@female@idle_a", "idle_" + list3[rnd.Next(0, list3.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@female@idle_a", "idle_" + list3[rnd.Next(0, list3.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@female@base", "base", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@female@idle_a", "idle_" + list3[rnd.Next(0, list3.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@female@idle_a", "idle_" + list3[rnd.Next(0, list3.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@female@idle_a", "idle_" + list3[rnd.Next(0, list3.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@female@idle_a", "idle_" + list3[rnd.Next(0, list3.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@female@idle_a", "idle_" + list3[rnd.Next(0, list3.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@female@base", "base", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@female@idle_a", "idle_" + list3[rnd.Next(0, list3.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@female@idle_a", "idle_" + list3[rnd.Next(0, list3.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@female@idle_a", "idle_" + list3[rnd.Next(0, list3.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@female@idle_a", "idle_" + list3[rnd.Next(0, list3.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "amb@prop_human_seat_sunlounger@female@idle_a", "idle_" + list3[rnd.Next(0, list3.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
				}
				Function.Call(Hash.SET_SEQUENCE_TO_REPEAT, num8, true);
				Function.Call(Hash.CLOSE_SEQUENCE_TASK, num8);
				Function.Call(Hash.TASK_PERFORM_SEQUENCE, ped, num8);
				Function.Call(Hash.CLEAR_SEQUENCE_TASK, &num8);
				break;
			}
			case "jacuzzi":
			{
				Class1.LoadDict("anim@amb@yacht@jacuzzi@seated@female@variation_01@");
				Class1.LoadDict("anim@amb@yacht@jacuzzi@seated@female@variation_02@");
				Class1.LoadDict("anim@amb@yacht@jacuzzi@seated@female@variation_03@");
				Class1.LoadDict("anim@amb@yacht@jacuzzi@seated@female@variation_04@");
				Class1.LoadDict("anim@amb@yacht@jacuzzi@seated@female@variation_05@");
				Class1.LoadDict("anim@amb@yacht@jacuzzi@seated@male@variation_01@");
				Class1.LoadDict("anim@amb@yacht@jacuzzi@seated@male@variation_02@");
				Class1.LoadDict("anim@amb@yacht@jacuzzi@seated@male@variation_03@");
				Class1.LoadDict("anim@amb@yacht@jacuzzi@seated@male@variation_04@");
				Class1.LoadDict("anim@amb@yacht@jacuzzi@seated@male@variation_05@");
				int num = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 1, 100);
				int num2 = 1;
				if (num < 20)
				{
					num2 = 1;
				}
				if (num >= 20 && num < 40)
				{
					num2 = 2;
				}
				if (num >= 40 && num < 60)
				{
					num2 = 3;
				}
				if (num >= 60 && num < 80)
				{
					num2 = 4;
				}
				if (num >= 80 && num <= 100)
				{
					num2 = 5;
				}
				int num3 = 0;
				List<string> list = new List<string>();
				list.Add("a");
				list.Add("b");
				list.Add("c");
				list.Add("d");
				Function.Call(Hash.OPEN_SEQUENCE_TASK, &num3);
				if (Function.Call<bool>(Hash.IS_PED_MALE, ped))
				{
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@yacht@jacuzzi@seated@male@variation_0" + num2 + "@", "base", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@yacht@jacuzzi@seated@male@variation_0" + num2 + "@", "idle_" + list[rnd.Next(0, list.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@yacht@jacuzzi@seated@male@variation_0" + num2 + "@", "idle_" + list[rnd.Next(0, list.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@yacht@jacuzzi@seated@male@variation_0" + num2 + "@", "idle_" + list[rnd.Next(0, list.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@yacht@jacuzzi@seated@male@variation_0" + num2 + "@", "idle_" + list[rnd.Next(0, list.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@yacht@jacuzzi@seated@male@variation_0" + num2 + "@", "base", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@yacht@jacuzzi@seated@male@variation_0" + num2 + "@", "idle_" + list[rnd.Next(0, list.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@yacht@jacuzzi@seated@male@variation_0" + num2 + "@", "idle_" + list[rnd.Next(0, list.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@yacht@jacuzzi@seated@male@variation_0" + num2 + "@", "idle_" + list[rnd.Next(0, list.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@yacht@jacuzzi@seated@male@variation_0" + num2 + "@", "idle_" + list[rnd.Next(0, list.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
				}
				else
				{
					list.Add("e");
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@yacht@jacuzzi@seated@female@variation_0" + num2 + "@", "base", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@yacht@jacuzzi@seated@female@variation_0" + num2 + "@", "idle_" + list[rnd.Next(0, list.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@yacht@jacuzzi@seated@female@variation_0" + num2 + "@", "idle_" + list[rnd.Next(0, list.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@yacht@jacuzzi@seated@female@variation_0" + num2 + "@", "idle_" + list[rnd.Next(0, list.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@yacht@jacuzzi@seated@female@variation_0" + num2 + "@", "idle_" + list[rnd.Next(0, list.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@yacht@jacuzzi@seated@female@variation_0" + num2 + "@", "base", 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@yacht@jacuzzi@seated@female@variation_0" + num2 + "@", "idle_" + list[rnd.Next(0, list.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@yacht@jacuzzi@seated@female@variation_0" + num2 + "@", "idle_" + list[rnd.Next(0, list.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@yacht@jacuzzi@seated@female@variation_0" + num2 + "@", "idle_" + list[rnd.Next(0, list.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
					Function.Call(Hash.TASK_PLAY_ANIM, 0, "anim@amb@yacht@jacuzzi@seated@female@variation_0" + num2 + "@", "idle_" + list[rnd.Next(0, list.Count)], 2f, 2f, -1, 0, -1f, true, true, true);
				}
				Function.Call(Hash.SET_SEQUENCE_TO_REPEAT, num3, true);
				Function.Call(Hash.CLOSE_SEQUENCE_TASK, num3);
				Function.Call(Hash.TASK_PERFORM_SEQUENCE, ped, num3);
				Function.Call(Hash.CLEAR_SEQUENCE_TASK, &num3);
				break;
			}
			}
		}

		public static void OnTick(object sender, EventArgs e)
		{
			if (!Class1.Initialized)
			{
				return;
			}
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, EnterMainHallA, Class1.GetDistSign.GreaterEqual, 5f) && Class1.GetDistance_VDIST2(Game.Player.Character.Position, EnterMainHallB, Class1.GetDistSign.GreaterEqual, 5f) && Class1.GetDistance_VDIST2(Game.Player.Character.Position, EnterMainHallC, Class1.GetDistSign.GreaterEqual, 5f) && Class1.GetDistance_VDIST2(Game.Player.Character.Position, EnterMainHallD, Class1.GetDistSign.GreaterEqual, 5f) && Class1.GetDistance_VDIST2(Game.Player.Character.Position, MainZone, Class1.GetDistSign.GreaterEqual, 7f) && Class1.GetDistance_VDIST2(Game.Player.Character.Position, EnterGamingZoneA, Class1.GetDistSign.GreaterEqual, 5f))
			{
				_Debug.CasinoPeds = 0;
				return;
			}
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, LastSpawnPos, Class1.GetDistSign.Less, 7f))
			{
				_Debug.CasinoPeds = 0;
				return;
			}
			_Debug.CasinoPeds = 1;
			Vector3 rotation = Game.Player.Character.Rotation;
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, EnterMainHallA, Class1.GetDistSign.Less, 7f))
			{
				LastSpawnPos = EnterMainHallA;
				CurrentMarker = "BLUE";
				if (Zones.CurrentZone != Zones.ZoneIn.GamingArea)
				{
					DeleteMainHallPeds();
					DeleteGamingAreaPeds();
					SpawnGamingAreaPeds();
					Zones.CurrentZone = Zones.ZoneIn.GamingArea;
				}
			}
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, EnterMainHallB, Class1.GetDistSign.Less, 5f))
			{
				LastSpawnPos = EnterMainHallB;
				CurrentMarker = "GREEN";
				if (Zones.CurrentZone != Zones.ZoneIn.GamingArea)
				{
					DeleteMainHallPeds();
					DeleteGamingAreaPeds();
					SpawnGamingAreaPeds();
					Zones.CurrentZone = Zones.ZoneIn.GamingArea;
				}
			}
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, EnterMainHallC, Class1.GetDistSign.Less, 5f))
			{
				LastSpawnPos = EnterMainHallC;
				CurrentMarker = "YELLOW";
				if (Zones.CurrentZone != Zones.ZoneIn.GamingArea)
				{
					DeleteMainHallPeds();
					DeleteGamingAreaPeds();
					SpawnGamingAreaPeds();
					Zones.CurrentZone = Zones.ZoneIn.GamingArea;
				}
			}
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, EnterMainHallD, Class1.GetDistSign.Less, 5f))
			{
				LastSpawnPos = EnterMainHallD;
				CurrentMarker = "VIOLET";
				if (Zones.CurrentZone != Zones.ZoneIn.MainHall)
				{
					DeleteMainHallPeds();
					DeleteGamingAreaPeds();
					SpawnMainHallPeds();
					Zones.CurrentZone = Zones.ZoneIn.MainHall;
				}
			}
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, MainZone, Class1.GetDistSign.Less, 7f))
			{
				LastSpawnPos = MainZone;
				CurrentMarker = "RED";
				if (Zones.CurrentZone != Zones.ZoneIn.MainHall)
				{
					DeleteMainHallPeds();
					DeleteGamingAreaPeds();
					SpawnMainHallPeds();
					Zones.CurrentZone = Zones.ZoneIn.MainHall;
				}
			}
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, EnterGamingZoneA, Class1.GetDistSign.Less, 5f))
			{
				LastSpawnPos = EnterGamingZoneA;
				CurrentMarker = "PINK";
				if (Zones.CurrentZone != Zones.ZoneIn.MainHall)
				{
					DeleteMainHallPeds();
					DeleteGamingAreaPeds();
					SpawnMainHallPeds();
					Zones.CurrentZone = Zones.ZoneIn.MainHall;
				}
			}
		}
	}
}
