using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;

namespace GTAO_DiamondCasino
{
	public class PenthouseExteriorPeds : Script
	{
		public static List<Ped> TerracePeds = new List<Ped>();

		public static List<string> swimwearPedModelList = new List<string> { "A_M_Y_Beach_04", "A_F_Y_Beach_02", "A_M_Y_Beach_04", "A_F_Y_Beach_02", "A_M_Y_Beach_04", "A_F_Y_Beach_02", "A_M_Y_Beach_04", "A_F_Y_Beach_02", "A_M_Y_Beach_04", "A_F_Y_Beach_02" };

		public static List<string> ambientPedModelList = new List<string>
		{
			"A_M_Y_SmartCasPat_01", "A_M_Y_GenCasPat_01", "A_M_Y_SmartCasPat_01", "A_M_Y_GenCasPat_01", "A_F_Y_GenCasPat_01", "A_F_Y_SmartCasPat_01", "g_m_m_casrn_01", "A_M_Y_SmartCasPat_01", "A_M_Y_GenCasPat_01", "A_M_Y_SmartCasPat_01",
			"A_F_Y_GenCasPat_01", "A_F_Y_SmartCasPat_01", "g_m_m_casrn_01", "A_M_Y_GenCasPat_01", "A_M_Y_SmartCasPat_01", "A_M_Y_GenCasPat_01", "A_F_Y_SmartCasPat_01", "g_m_m_casrn_01", "A_M_Y_SmartCasPat_01", "A_M_Y_GenCasPat_01",
			"A_M_Y_SmartCasPat_01", "A_M_Y_GenCasPat_01", "A_M_Y_SmartCasPat_01", "A_M_Y_GenCasPat_01", "A_F_Y_GenCasPat_01"
		};

		public static Random rnd = new Random();

		public static List<Ped> TerracePed = new List<Ped>();

		public static bool CreatedPenthousePeds = false;

		public static bool RunMethodNow()
		{
			bool result = true;
			if (!CreatedPenthousePeds && Class1.IsinInterior)
			{
				_Debug.PenthouseExteriroPeds = 0;
				result = false;
			}
			return result;
		}

		public static void SetPedAnim(Ped ped, string type)
		{
			CasinoPeds.SetPedAnim(ped, type);
		}

		public static void CreatePenthouseExteriorPeds()
		{
			CasinoPeds.spawnPed("sunbather", 0f, 0f, 0f, 0f, TerracePeds);
			CasinoPeds.spawnPed("sunbather", 931.3459f, 27.1438885f, 111.873581f, 63.06773f, TerracePeds);
			CasinoPeds.spawnPed("sunbather", 936.6642f, 35.35233f, 111.914841f, 59.55853f, TerracePeds);
			CasinoPeds.spawnPed("sunbather", 937.3723f, 36.6131554f, 111.9268f, 60.9069252f, TerracePeds);
			CasinoPeds.spawnPed("sunbather", 943.8869f, 46.9052048f, 111.95694f, 57.7841225f, TerracePeds);
			CasinoPeds.spawnPed("sunbather", 950.8467f, 47.66906f, 111.951218f, 58.24423f, TerracePeds);
			CasinoPeds.spawnPed("sunbather", 951.5862f, 49.025177f, 111.940414f, 58.090107f, TerracePeds);
			CasinoPeds.spawnPed("sunbather", 950.0508f, 56.94133f, 111.943184f, 59.5742264f, TerracePeds);
			CasinoPeds.spawnPed("sunbather", 939.469055f, 29.4598064f, 111.944954f, 58.1838379f, TerracePeds);
			CasinoPeds.spawnPed("sunbather", 938.659546f, 28.1404572f, 112.006714f, 58.3367729f, TerracePeds);
			CasinoPeds.spawnPed("sunbather", 936.393555f, 25.3546448f, 111.309792f, 143.818619f, TerracePeds);
			CasinoPeds.spawnPed("sunbather", 936.9951f, 23.7751427f, 111.309616f, 50.8510971f, TerracePeds);
			CasinoPeds.spawnPed("jacuzzi", 934.9429f, 23.0531445f, 111.309792f, -30.20134f, TerracePeds);
			CasinoPeds.spawnPed("jacuzzi", 934.528931f, 24.9485149f, 111.309425f, -124.551292f, TerracePeds);
			CasinoPeds.spawnPed("jacuzzi", 941.1501f, 32.8142776f, 111.300011f, -32.5606956f, TerracePeds);
			CasinoPeds.spawnPed("jacuzzi", 940.630859f, 34.7683754f, 111.299423f, -128.56366f, TerracePeds);
			CasinoPeds.spawnPed("jacuzzi", 942.531555f, 35.1328964f, 111.309784f, 148.351791f, TerracePeds);
			CasinoPeds.spawnPed("jacuzzi", 942.9805f, 33.35457f, 111.319611f, 60.7799873f, TerracePeds);
			CasinoPeds.spawnPed("jacuzzi", 949.084534f, 43.13834f, 111.319618f, 60.21666f, TerracePeds);
			CasinoPeds.spawnPed("jacuzzi", 947.2589f, 42.63078f, 111.3196f, -30.783247f, TerracePeds);
			CasinoPeds.spawnPed("jacuzzi", 946.5944f, 44.2957954f, 111.30896f, -120.603966f, TerracePeds);
			CasinoPeds.spawnPed("jacuzzi", 948.5988f, 44.9898949f, 111.3096f, 147.114273f, TerracePeds);
			CasinoPeds.spawnPed("jacuzzi", 953.4675f, 52.2868843f, 111.289604f, -34.16084f, TerracePeds);
			CasinoPeds.spawnPed("jacuzzi", 952.792969f, 54.17612f, 111.299591f, -121.065468f, TerracePeds);
			CasinoPeds.spawnPed("jacuzzi", 954.6641f, 54.6746979f, 111.289589f, 150.054474f, TerracePeds);
			CasinoPeds.spawnPed("jacuzzi", 955.194763f, 52.9267921f, 111.309731f, 59.1022644f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 923.001831f, 21.1664371f, 112.546478f, -68.72737f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 923.6296f, 22.1009159f, 112.546761f, 170.5874f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 924.358154f, 21.1318722f, 112.546623f, 70.06741f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 926.766357f, 24.6024837f, 112.547234f, -36.7474251f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 927.7771f, 25.675066f, 112.556686f, 169.606216f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 928.1557f, 24.6127338f, 112.547745f, 66.21944f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 933.873f, 19.3070354f, 112.547127f, 86.58887f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 932.454041f, 20.0413017f, 112.547722f, -143.30719f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 932.7986f, 18.6481743f, 112.557922f, -26.10272f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 934.487366f, 29.3186779f, 112.547493f, -105.022858f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 935.962f, 29.598175f, 112.547493f, 129.838181f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 935.4526f, 28.17353f, 112.547707f, 14.6804085f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 929.7395f, 32.4749222f, 112.549263f, -172.019882f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 930.808167f, 31.5068626f, 112.547691f, 101.1252f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 929.438049f, 30.69465f, 112.547913f, -25.4967976f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 942.7265f, 38.97264f, 112.55661f, -73.54753f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 944.197632f, 38.4813538f, 112.548325f, 50.91661f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 943.805359f, 39.798687f, 112.547363f, 171.39537f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 947.4063f, 48.89651f, 112.557976f, -47.5039558f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 948.8223f, 50.00177f, 112.547523f, 132.542526f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 940.4581f, 43.9088745f, 112.547211f, 107.462776f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 939.122f, 43.42005f, 112.557114f, -64.1474152f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 951.0313f, 59.25463f, 112.547905f, -98.13037f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 952.355164f, 58.8112221f, 112.547188f, 78.79463f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 949.5729f, 69.2188644f, 113.54879f, 14.7668581f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 950.290466f, 70.53189f, 113.548981f, 104.683479f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 948.668f, 70.52789f, 113.555443f, -115.540581f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 921.9978f, 27.9514122f, 113.55381f, -121.37336f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 923.1184f, 27.0813f, 113.542519f, 55.1499557f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 927.2961f, 34.0162621f, 113.541641f, 10.7758656f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 927.713745f, 35.18829f, 113.552437f, 123.519295f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 926.4302f, 35.3117447f, 113.5424f, -122.244247f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 930.299255f, 42.896244f, 113.544121f, -47.5186157f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 931.4414f, 43.979126f, 113.554947f, 139.181931f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 937.9392f, 58.5555649f, 113.544456f, -80.93923f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 938.9944f, 57.91294f, 113.544853f, 37.99742f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 938.903259f, 59.2703972f, 113.544228f, 160.882645f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 945.4461f, 33.81906f, 112.557869f, 127.76841f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 944.112244f, 33.04755f, 112.548256f, -62.66815f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 914.4013f, 37.3339043f, 111.696f, -111.288612f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 915.9726f, 37.8690224f, 111.696426f, 130.431519f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 915.6935f, 36.2039452f, 111.696144f, 19.572979f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 920.574036f, 49.77318f, 111.695877f, -71.3755f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 921.961548f, 50.07414f, 111.696136f, 97.56526f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 925.2181f, 51.5856934f, 111.696716f, -73.57445f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 925.7651f, 52.8068848f, 111.696785f, 172.700668f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 926.89f, 51.815815f, 111.65638f, 80.4717255f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 955.994f, 57.96307f, 112.557419f, -63.18446f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 957.5026f, 57.5308151f, 112.557243f, 23.09883f, TerracePeds);
			CasinoPeds.spawnPed("Ambient", 956.9027f, 59.1101875f, 112.557739f, 179.302887f, TerracePeds);
		}

		public static void DeletePenthouseExteriorPeds()
		{
			foreach (Ped terracePed in TerracePeds)
			{
				if (terracePed != null)
				{
					terracePed.Delete();
				}
			}
		}

		public PenthouseExteriorPeds()
		{
			if (_Debug.PenthouseExteriroPeds != -1)
			{
				base.Tick += OnTick;
			}
		}

		public static void OnTick(object sender, EventArgs e)
		{
			if (!CreatedPenthousePeds && Class1.IsinInterior)
			{
				_Debug.PenthouseExteriroPeds = 0;
				return;
			}
			_Debug.PenthouseExteriroPeds = 0;
			if (Class1.IsinInterior)
			{
				if (CreatedPenthousePeds)
				{
					CreatedPenthousePeds = false;
					DeletePenthouseExteriorPeds();
				}
			}
			else if (!CreatedPenthousePeds)
			{
				if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(944.79f, 45f, 111.55f), Class1.GetDistSign.Less, 100f) && Game.Player.Character.Position.Z > 95f)
				{
					CreatedPenthousePeds = true;
					CreatePenthouseExteriorPeds();
				}
			}
			else if (Game.Player.Character.Position.Z < 95f)
			{
				CreatedPenthousePeds = false;
				DeletePenthouseExteriorPeds();
			}
		}
	}
}
