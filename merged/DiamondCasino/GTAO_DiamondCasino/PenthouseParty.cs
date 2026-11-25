using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using GTAO_DiamondCasino.Menus;

namespace GTAO_DiamondCasino
{
	public class PenthouseParty : Script
	{
		public class SpawnS : Script
		{
			public Vector3 Vec { get; set; }

			public float Flo { get; set; }

			public string Pedhash { get; set; }

			public int Type { get; set; }

			public SpawnS()
			{
			}

			public SpawnS(Vector3 V, float F)
			{
				Vec = V;
				Flo = F;
			}

			public SpawnS(string S, int T, Vector3 V, float F)
			{
				Pedhash = S;
				Type = T;
				Vec = V;
				Flo = F;
			}

			public SpawnS(int T, Vector3 V, float F)
			{
				Type = T;
				Vec = V;
				Flo = F;
			}
		}

		public static List<string> DancesList = new List<string>
		{
			"anim@amb@nightclub_island@dancers@beachdance@,mi_loop_m03", "anim@amb@nightclub_island@dancers@beachdance@,mi_loop_m04", "anim@amb@nightclub_island@dancers@beachdance@,mi_loop_m05", "anim@amb@nightclub_island@dancers@beachdance@,mi_loop_m04", "anim@amb@nightclub_island@dancers@beachdance@,ti_loop_m01", "anim@amb@nightclub_island@dancers@beachdance@,ti_loop_m02", "anim@amb@nightclub_island@dancers@beachdance@,ti_loop_m03", "anim@amb@nightclub_island@dancers@beachdance@,ti_loop_m04", "anim@amb@nightclub_island@dancers@beachdance@,hi_loop_f01", "anim@amb@nightclub_island@dancers@beachdance@,hi_loop_f02",
			"anim@amb@nightclub_island@dancers@beachdance@,li_loop_m01", "anim@amb@nightclub_island@dancers@beachdance@,li_loop_m02", "anim@amb@nightclub_island@dancers@beachdance@,li_loop_m03", "anim@amb@nightclub_island@dancers@beachdance@,li_loop_m04", "anim@amb@nightclub_island@dancers@beachdance@,li_loop_f01", "anim@amb@nightclub_island@dancers@beachdance@,li_loop_f02"
		};

		public static bool SpawnedDancers;

		public static List<Ped> Dancers = new List<Ped>();

		public static List<SpawnS> CivillianSpawns = new List<SpawnS>
		{
			new SpawnS(1, new Vector3(940.3389f, 13.86792f, 116.1642f), 228.6752f),
			new SpawnS(1, new Vector3(941.5753f, 14.70272f, 116.1642f), 89.62915f),
			new SpawnS(1, new Vector3(941.4255f, 13.57982f, 116.1642f), 38.23604f),
			new SpawnS(1, new Vector3(935.844f, 7.046496f, 116.1642f), 332.9319f),
			new SpawnS(1, new Vector3(937.1695f, 8.634978f, 116.1642f), 113.1428f),
			new SpawnS(1, new Vector3(938.6244f, 7.334195f, 116.1642f), 224.2561f),
			new SpawnS(1, new Vector3(939.9634f, 5.947296f, 116.1642f), 38.73786f),
			new SpawnS(1, new Vector3(942.0176f, 5.581164f, 116.1642f), 4.225938f),
			new SpawnS(1, new Vector3(943.2343f, 5.575437f, 116.1642f), 269.261f),
			new SpawnS(1, new Vector3(944.0798f, 6.644825f, 116.1642f), 160.258f),
			new SpawnS(1, new Vector3(944.8551f, 5.288838f, 116.1642f), 51.35738f),
			new SpawnS(1, new Vector3(945.2695f, 3.143576f, 116.1642f), 284.1477f),
			new SpawnS(1, new Vector3(946.7433f, 3.693798f, 116.1642f), 100.9248f),
			new SpawnS(1, new Vector3(940.4261f, 3.537667f, 116.1642f), 147.2634f),
			new SpawnS(1, new Vector3(939.0691f, 3.774562f, 116.1642f), 170.9926f),
			new SpawnS(1, new Vector3(937.4559f, 3.006f, 116.1642f), 253.2326f),
			new SpawnS(1, new Vector3(943.8021f, 11.55267f, 116.1642f), 134.2634f),
			new SpawnS(1, new Vector3(942.8433f, 11.65282f, 116.1642f), 277.1606f),
			new SpawnS(1, new Vector3(945.8914f, 17.10113f, 116.1642f), 78.21075f),
			new SpawnS(1, new Vector3(944.2933f, 16.83902f, 116.1642f), 295.8203f),
			new SpawnS(1, new Vector3(943.8658f, 19.63585f, 116.1642f), 340.3393f),
			new SpawnS(1, new Vector3(944.8027f, 20.67276f, 116.1642f), 57.57127f),
			new SpawnS(1, new Vector3(948.5615f, 18.84385f, 116.1642f), 142.5691f),
			new SpawnS(1, new Vector3(952.2647f, 21.91467f, 116.1642f), 244.8902f),
			new SpawnS(1, new Vector3(953.821f, 21.60463f, 116.1642f), 66.87621f),
			new SpawnS(1, new Vector3(953.443f, 26.45722f, 116.1642f), 167.6311f),
			new SpawnS(1, new Vector3(954.2025f, 25.03222f, 116.1642f), 347.7527f),
			new SpawnS(1, new Vector3(951.2604f, 24.45657f, 116.1642f), 153.1017f),
			new SpawnS(1, new Vector3(952.4682f, 23.67835f, 116.1642f), 47.29372f),
			new SpawnS(1, new Vector3(950.8041f, 23.02069f, 116.1642f), 303.5323f),
			new SpawnS(1, new Vector3(958.1925f, 22.93664f, 116.1642f), 109.3908f),
			new SpawnS(1, new Vector3(957.4963f, 21.5837f, 116.1642f), 323.1607f),
			new SpawnS(1, new Vector3(964.197f, 31.16289f, 116.1642f), 229.7441f),
			new SpawnS(1, new Vector3(965.7258f, 31.85718f, 116.1761f), 206.5975f),
			new SpawnS(1, new Vector3(964.3633f, 32.58329f, 116.1642f), 178.6785f),
			new SpawnS(1, new Vector3(965.4962f, 34.1111f, 116.1642f), 265.0572f),
			new SpawnS(1, new Vector3(966.2554f, 35.5251f, 116.1642f), 124.6912f),
			new SpawnS(1, new Vector3(967.8754f, 31.92117f, 116.1761f), 329.0187f),
			new SpawnS(1, new Vector3(963.045f, 37.69917f, 116.1642f), 57.25354f),
			new SpawnS(1, new Vector3(961.3177f, 38.33633f, 116.1642f), 262.9016f),
			new SpawnS(1, new Vector3(962.6148f, 39.1163f, 116.1642f), 147.0888f),
			new SpawnS(1, new Vector3(957.5583f, 41.58075f, 116.1642f), 334.0545f),
			new SpawnS(1, new Vector3(959.1136f, 41.39601f, 116.1642f), 107.2954f),
			new SpawnS(1, new Vector3(963.4489f, 44.11718f, 116.1759f), 308.3614f),
			new SpawnS(1, new Vector3(964.9203f, 45.09602f, 116.1761f), 71.03542f),
			new SpawnS(1, new Vector3(964.7509f, 42.89546f, 116.1642f), 11.23004f),
			new SpawnS(1, new Vector3(967.9941f, 44.22229f, 116.1642f), 223.1564f),
			new SpawnS(1, new Vector3(969.8339f, 43.05304f, 116.1642f), 140.3064f),
			new SpawnS(1, new Vector3(968.1816f, 42.37641f, 116.1642f), 350.4776f),
			new SpawnS(1, new Vector3(963.1379f, 47.53055f, 116.1761f), 23.33661f),
			new SpawnS(1, new Vector3(963.6896f, 48.83577f, 116.1761f), 120.9801f),
			new SpawnS(1, new Vector3(966.6624f, 49.87446f, 116.176f), 292.8786f),
			new SpawnS(1, new Vector3(967.7124f, 51.30257f, 116.1642f), 160.6275f),
			new SpawnS(1, new Vector3(968.4327f, 49.45813f, 116.1759f), 55.67472f),
			new SpawnS(1, new Vector3(972.5819f, 48.44128f, 116.1642f), 53.20551f),
			new SpawnS(1, new Vector3(971.0115f, 48.63354f, 116.1642f), 284.5505f),
			new SpawnS(1, new Vector3(975.9191f, 57.17556f, 116.1637f), 296.2444f),
			new SpawnS(1, new Vector3(975.5403f, 55.87547f, 116.1641f), 278.5245f),
			new SpawnS(1, new Vector3(977.2548f, 56.41534f, 116.1691f), 87.98623f),
			new SpawnS(1, new Vector3(958.053f, 28.12675f, 116.1761f), 313.5931f),
			new SpawnS(1, new Vector3(958.8018f, 29.51286f, 116.1761f), 110.5298f),
			new SpawnS(1, new Vector3(960.5164f, 32.79151f, 116.1646f), 318.1877f),
			new SpawnS(1, new Vector3(961.0229f, 33.87328f, 116.1642f), 206.6653f),
			new SpawnS(1, new Vector3(947.3071f, 20.20756f, 116.1642f), 137.0335f),
			new SpawnS(1, new Vector3(946.2736f, 19.78407f, 116.1642f), 209.6335f),
			new SpawnS(1, new Vector3(945.2597f, 8.354125f, 116.1642f), 193.494f),
			new SpawnS(1, new Vector3(945.7256f, 7.392662f, 116.1642f), 15.32119f)
		};

		public static int Now;

		public static int Party_NextDay;

		public static int Party_NextMonth;

		public static int Party_NextYear;

		public static int Party_Now;

		public static bool PartyOn;

		public static int DAYSTORESET_UPDATETIME = 12;

		public static int DaysWait = 3;

		public static bool BYPASS_NOSAVE_OR_CRASH;

		public static Vector3 PartyStartMarker = new Vector3(983.54f, 66.71f, 115.16f);

		public static Model RequestModel(string Prop)
		{
			Model result = new Model(Prop);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(50);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public static void SetRandomOutfit(Ped Dancer)
		{
			Random random = new Random();
			if (Dancer.Gender == Gender.Female)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 0, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 0)), 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 2, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 2)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 2)), 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 3)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 3)), 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 8, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 8)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 8)), 0);
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 4)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 4, 4, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 4)), 1);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) != 4)
				{
					int num = random.Next(0, 5);
					if (num == 4)
					{
						int num2 = random.Next(0, 4);
						if (num2 == 0)
						{
							num = 0;
						}
						if (num2 == 1)
						{
							num = 1;
						}
						if (num2 == 2)
						{
							num = 2;
						}
						if (num2 == 3)
						{
							num = 3;
						}
						if (num2 == 4)
						{
							num = 5;
						}
					}
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 4, num, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 4)), 1);
				}
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 0, Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3), 0, 0);
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 4) == 4)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 5, 4, 0, 1);
				}
			}
			if (Dancer.Gender == Gender.Male)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 0, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 0)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 0)), 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 2, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 2)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 2)), 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 3)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 3)), 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 8, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 8)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 8)), 0);
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 0)
				{
					int num3 = random.Next(0, 4);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num3, 0);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 1)
				{
					int num4 = random.Next(0, 3);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num4, 0);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 2)
				{
					int num5 = random.Next(0, 3);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num5, 0);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 3)
				{
					int num6 = random.Next(0, 3);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num6, 0);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 4)
				{
					int num7 = random.Next(0, 2);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num7, 0);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 5)
				{
					int num8 = random.Next(0, 2);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num8, 0);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 6)
				{
					int num9 = random.Next(0, 2);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num9, 0);
				}
			}
		}

		public static void SpawnPartyPeds()
		{
			foreach (Ped dancer in Dancers)
			{
				if (dancer != null)
				{
					dancer.Delete();
				}
			}
			SpawnedDancers = true;
			List<string> list = new List<string>();
			list.Add("a_m_y_clubcust_01");
			list.Add("a_f_y_clubcust_01");
			list.Add("a_m_y_clubcust_01");
			list.Add("a_f_y_clubcust_01");
			list.Add("a_m_y_clubcust_01");
			list.Add("a_f_y_clubcust_01");
			list.Add("a_m_y_clubcust_01");
			list.Add("a_f_y_clubcust_01");
			list.Add("a_m_y_clubcust_01");
			list.Add("a_f_y_clubcust_01");
			list.Add("a_m_y_clubcust_01");
			list.Add("a_f_y_clubcust_01");
			list.Add("a_m_y_clubcust_01");
			list.Add("a_f_y_clubcust_01");
			list.Add("a_m_y_clubcust_01");
			list.Add("a_f_y_clubcust_01");
			list.Add("a_m_y_clubcust_01");
			list.Add("a_f_y_clubcust_01");
			list.Add("a_m_y_clubcust_01");
			list.Add("a_f_y_clubcust_01");
			Random random = new Random();
			foreach (SpawnS civillianSpawn in CivillianSpawns)
			{
				Ped ped = GlobalHelper.CreatePed(RequestModel(list[random.Next(0, list.Count)]), civillianSpawn.Vec, civillianSpawn.Flo);
				Dancers.Add(ped);
				SetRandomOutfit(ped);
				ped.BlockPermanentEvents = true;
				ped.RelationshipGroup = 2;
				Script.Wait(1);
				int index = random.Next(0, DancesList.Count);
				string[] array = DancesList[index].Split(new char[1] { ',' });
				string animDict = array[0];
				string animName = array[1];
				ped.Task.PlayAnimation(animDict, animName, -1f, -1, AnimationFlags.Loop);
				ped.AlwaysKeepTask = true;
			}
			foreach (Ped dancer2 in Dancers)
			{
				if (dancer2 != null)
				{
					Function.Call(Hash.SET_PED_FLEE_ATTRIBUTES, dancer2, 0, 0);
					Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, dancer2, 17, 1);
					Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, dancer2, 1);
					Function.Call(Hash.SET_PED_FLEE_ATTRIBUTES, dancer2, 0, 0);
					Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, dancer2, 17, 1);
					dancer2.BlockPermanentEvents = true;
					dancer2.AlwaysKeepTask = true;
					dancer2.CanRagdoll = false;
					Function.Call(Hash.SET_PED_CAN_BE_TARGETTED, dancer2, false);
					Function.Call(Hash.SET_ENTITY_IS_TARGET_PRIORITY, dancer2, false, false);
					Function.Call(Hash.SET_PED_CAN_BE_TARGETED_WITHOUT_LOS, dancer2, false);
					Function.Call(Hash.SET_ENTITY_CAN_BE_DAMAGED, dancer2, false);
				}
			}
		}

		public static int GetHour()
		{
			return Function.Call<int>(Hash.GET_CLOCK_HOURS);
		}

		public static int GetYear()
		{
			return Function.Call<int>(Hash.GET_CLOCK_YEAR);
		}

		public static int GetMinutes()
		{
			return Function.Call<int>(Hash.GET_CLOCK_MINUTES);
		}

		public static int GetDay()
		{
			return Function.Call<int>(Hash.GET_CLOCK_DAY_OF_MONTH);
		}

		public static int GetMonth()
		{
			return Function.Call<int>(Hash.GET_CLOCK_MONTH);
		}

		public static void Party_GetDateRelative()
		{
			if (!BYPASS_NOSAVE_OR_CRASH || Now == GetDay())
			{
				return;
			}
			Now = GetDay();
			int num = GetDay() + DAYSTORESET_UPDATETIME;
			int num2 = GetMonth();
			int year = GetYear();
			if (num < 0)
			{
				num = -num;
				if (GetMonth() + 1 <= 12)
				{
					num2 = GetMonth() + 1;
				}
				if (GetMonth() + 1 > 12)
				{
					num2 = 1;
				}
			}
			if (num > 30)
			{
				num = 30;
			}
			if (GetMonth() + 1 > 12)
			{
				num2 = 1;
			}
			if ((num > Party_NextDay || num2 > Party_NextMonth) && num2 >= Party_NextMonth && num2 <= Party_NextMonth && year == Party_NextYear)
			{
				return;
			}
			Party_NextMonth = GetMonth();
			Party_NextDay = GetDay() + DaysWait;
			Party_NextYear = GetYear();
			foreach (Ped dancer in Dancers)
			{
				if (dancer != null)
				{
					dancer.Delete();
				}
			}
		}

		public PenthouseParty()
		{
			if (_Debug.PenthouseParty != -1)
			{
				base.Tick += OnTick;
			}
		}

		public static void OnTick(object sender, EventArgs e)
		{
			if (Class1.Purchased_Penthouse == 0)
			{
				return;
			}
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, PartyStartMarker, Class1.GetDistSign.Greater, 1.5f))
			{
				_Debug.PenthouseParty = 0;
				return;
			}
			_Debug.PenthouseParty = 1;
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, PartyStartMarker, Class1.GetDistSign.Less, 1.5f) && !PenthouseArtPlacement.InArtDecorator)
			{
				Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Access Penthouse Options");
				if (Game.IsControlJustPressed(Control.Context) && PurchasePenthouse.PenthouseArt != null && !PurchasePenthouse.PenthouseArt.Visible)
				{
					PurchasePenthouse.PenthouseArt.Visible = !PurchasePenthouse.PenthouseArt.Visible;
				}
			}
		}
	}
}
