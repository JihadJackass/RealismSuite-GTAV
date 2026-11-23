using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class NightclubExpansion
	{
		private sealed class Resident
		{
			public readonly string ModelName;

			public readonly Vector3 Position;

			public readonly float Heading;

			public readonly string Dictionary;

			public readonly string Clip;

			public readonly string Scenario;

			public readonly bool SeatOrigin;

			public readonly string RoomName;

			public Resident(string modelName, Vector3 position, float heading, string dictionary = null, string clip = null, string scenario = null, bool seatOrigin = false, string roomName = null)
			{
				ModelName = modelName;
				Position = position;
				Heading = heading;
				Dictionary = dictionary;
				Clip = clip;
				Scenario = scenario;
				SeatOrigin = seatOrigin || NightclubCharacterScenes.IsSeated(modelName);
				RoomName = roomName;
			}
		}

		internal struct ResidentReceipt
		{
			public Ped Actor;

			public IntPtr Address;

			public int ModelHash;

			public bool IsDj;

			public int ResidentIndex;

			public Vector3 Floor;
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			internal static Func<ResidentReceipt, Vector3> _0024I62_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal Vector3 _Lambda_0024__62_002D0(ResidentReceipt receipt)
			{
				return receipt.Floor;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__49_002D0
		{
			public string _0024VB_0024Local_key;

			[SpecialName]
			internal bool _Lambda_0024__0(int interior)
			{
				return NightclubExpansionMenu.Prepare(interior, _0024VB_0024Local_key);
			}
		}

		private const string ClubIpl = "ba_int_placement_ba_interior_0_dlc_int_01_ba_milo_";

		private const string ClubArchetype = "ba_dlc_int_01_ba";

		private const string ClubAudioScene = "DLC_Ba_NightClub_Scene";

		private const string OfficeEmitter = "SE_ba_dlc_int_01_office";

		private const string DanceDictionary = "anim@amb@nightclub@dancers@crowddance_facedj@hi_intensity";

		private const string GroupDanceDictionary = "anim@amb@nightclub@dancers@crowddance_groups@hi_intensity";

		private const string DjModel = "ig_sol";

		private const string DjDictionary = "mini@strip_club@idles@dj@base";

		private const string BartenderDictionary = "anim@amb@nightclub@mini@drinking@champagne_drinking@base@";

		private const string EntryHallRoom = "Int_01_Entry_hall";

		private const int AssetTimeoutMs = 3000;

		private const int PlacementGraceMs = 500;

		private const int SpawnIntervalMs = 90;

		private const int PopulationPasses = 3;

		private const int PopulationRetryMs = 1800;

		private const int DjSwapAttempts = 3;

		private static readonly Vector3 Anchor = new Vector3(-1604.664f, -3012.583f, -79.9999f);

		private static readonly Vector3 DjPlatform = new Vector3(-1602.862f, -3012.739f, -78.606f);

		private static readonly Quaternion Arrival = new Quaternion(-1591.485f, -3013.607f, -80.006f, 74.0804f);

		private static readonly Quaternion HallArrival = new Quaternion(-1569.257f, -3008.166f, -75.4061f, 0f);

		private static readonly Vector3 ExitPoint = new Vector3(-1569.44775f, -3017.4646f, -75.40613f);

		private static readonly string[] MixEmitters = new string[7] { "SE_ba_dlc_int_01_main_area", "SE_ba_dlc_int_01_main_area_2", "SE_ba_dlc_int_01_office", "SE_ba_dlc_int_01_Bogs", "SE_ba_dlc_int_01_Entry_Hall", "SE_ba_dlc_int_01_Entry_Stairs", "SE_ba_dlc_int_01_rear_L_corridor" };

		private static readonly Resident[] Residents = BuildResidents();

		private static readonly List<ResidentReceipt> spawned = new List<ResidentReceipt>();

		private static readonly HashSet<int> completed = new HashSet<int>();

		private static readonly HashSet<string> ownedDictionaries = new HashSet<string>(StringComparer.Ordinal);

		private static readonly HashSet<string> unavailableDictionaries = new HashSet<string>(StringComparer.Ordinal);

		private static readonly List<string> ownedEmitters = new List<string>();

		private static readonly Stopwatch requestClock = new Stopwatch();

		private static readonly Stopwatch visitClock = new Stopwatch();

		private static ExpansionSession liveSession;

		private static int residentIndex;

		private static int populationPass;

		private static long nextResidentAt;

		private static Model pendingModel;

		private static bool ownsModel;

		private static bool ownsAudioScene;

		private static NightclubDj currentDj;

		private static NightclubDj requestedDj;

		private static int djSwapAttemptsLeft;

		private static bool mixEmittersEnabled = true;

		private static int populationChoice = 2;

		private static readonly Vector3[] DanceFloorOffsets = new Vector3[3]
		{
			Vector3.Zero,
			new Vector3(0f, 0.32f, 0f),
			new Vector3(0f, -0.32f, 0f)
		};

		private static Resident[] BuildResidents()
		{
			List<Resident> list = new List<Resident>
			{
				new Resident("ig_sol", DjPlatform, 278.3279f, "mini@strip_club@idles@dj@base", "base"),
				new Resident("s_f_y_clubbar_01", new Vector3(-1576.561f, -3016.594f, -80.0058f), 356.1967f, "anim@amb@nightclub@mini@drinking@champagne_drinking@base@", "bartender_idle"),
				new Resident("s_f_y_clubbar_01", new Vector3(-1585f, -3012.574f, -77.005f), 85f, "anim@amb@nightclub@mini@drinking@champagne_drinking@base@", "bartender_idle"),
				new Resident("s_m_m_bouncer_01", new Vector3(-1590.468f, -3010.858f, -80.00606f), 94.99987f, null, null, "WORLD_HUMAN_GUARD_STAND"),
				new Resident("s_m_m_bouncer_01", new Vector3(-1594.557f, -3004.772f, -77.005f), 139.9995f, null, null, "WORLD_HUMAN_GUARD_STAND"),
				new Resident("s_m_m_bouncer_01", new Vector3(-1605.868f, -3007.602f, -77.005f), 319.9996f, null, null, "WORLD_HUMAN_GUARD_STAND")
			};
			string[] models = new string[6] { "a_f_y_clubcust_01", "a_m_y_clubcust_01", "a_f_y_clubcust_02", "a_m_y_clubcust_02", "a_f_y_clubcust_03", "a_m_y_clubcust_03" };
			list.Add(new Resident("s_f_y_clubbar_01", new Vector3(-1572.273f, -3013.267f, -75.5995f), 285f, "anim@amb@nightclub@peds@", "amb_prop_human_seat_bar_female_elbows_on_bar_idle_a", null, true, "Int_01_Entry_hall"));
			AddDanceFloorGuests(list, models);
			list.Add(new Resident("ig_tonyprince", NightclubCharacterScenes.TonyPose, 34.5436f, "amb@prop_human_seat_computer@male@base", "base"));
			list.Add(new Resident("ig_lazlow_2", NightclubCharacterScenes.LazlowPose, 94.99975f, "anim@amb@nightclub@lazlow@hi_dancefloor@", "crowddance_mi_11_spankthat_laz"));
			list.Add(new Resident("s_m_y_clubbar_01", NightclubCharacterScenes.AttendantPose, 240.814f, "amb@prop_human_seat_chair@male@generic@base", "base"));
			AddReferenceGuests(list, models);
			return list.ToArray();
		}

		private static void AddDanceFloorGuests(List<Resident> residents, string[] models)
		{
			Quaternion[] array = new Quaternion[16]
			{
				new Quaternion(-1597.81f, -3013.999f, -80.0061f, 64.9992f),
				new Quaternion(-1598.791f, -3013.354f, -80.0061f, 89.999f),
				new Quaternion(-1598.803f, -3011.52f, -80.0061f, 89.999f),
				new Quaternion(-1598.276f, -3010.419f, -80.0061f, 89.9994f),
				new Quaternion(-1596.136f, -3014.376f, -80.0061f, 64.9992f),
				new Quaternion(-1596.532f, -3012.257f, -80.0061f, 84.9991f),
				new Quaternion(-1597.35f, -3011.214f, -80.0061f, 89.999f),
				new Quaternion(-1597.269f, -3009.916f, -80.0061f, 139.9992f),
				new Quaternion(-1594.935f, -3014.76f, -80.0061f, 79.998f),
				new Quaternion(-1595.858f, -3012.948f, -80.0061f, 89.9984f),
				new Quaternion(-1594.98f, -3011.497f, -80.0061f, 74.999f),
				new Quaternion(-1595.68f, -3009.549f, -80.0061f, 119.999f),
				new Quaternion(-1593.243f, -3014.435f, -80.0061f, 69.9988f),
				new Quaternion(-1593.164f, -3013.143f, -80.0061f, 69.9988f),
				new Quaternion(-1593.241f, -3011.182f, -80.0061f, 94.9989f),
				new Quaternion(-1593.338f, -3009.497f, -80.0061f, 99.9987f)
			};
			string[] array2 = new string[4] { "09", "11", "13", "15" };
			int num = checked(array.Length - 1);
			for (int i = 0; i <= num; i = checked(i + 1))
			{
				Quaternion q = array[i];
				int num2 = i % models.Length;
				string text = ((num2 % 2 == 0) ? "female" : "male");
				string clip = "hi_dance_facedj_" + array2[checked(i + unchecked(i / 4)) % array2.Length] + "_v1_" + text + "^1";
				residents.Add(new Resident(models[num2], Helper.ToVector3(q), q.W, "anim@amb@nightclub@dancers@crowddance_facedj@hi_intensity", clip));
			}
		}

		private static void AddReferenceGuests(List<Resident> residents, string[] models)
		{
			Quaternion[] array = new Quaternion[9]
			{
				new Quaternion(-1605.931f, -3014.545f, -78.7911f, 309.9981f),
				new Quaternion(-1606.21f, -3011.844f, -78.7911f, 254.9979f),
				new Quaternion(-1605.556f, -3012.632f, -78.7911f, 279.9979f),
				new Quaternion(-1586.844f, -3007.437f, -77.005f, 119.9996f),
				new Quaternion(-1587.767f, -3008.194f, -77.005f, 69.9992f),
				new Quaternion(-1591.617f, -3005.606f, -77.005f, 184.9989f),
				new Quaternion(-1590.841f, -9459f / MathF.PI, -77.005f, 99.9987f),
				new Quaternion(-1590.943f, -3013.203f, -77.005f, 69.9985f),
				new Quaternion(-1589.096f, -3015.45f, -77.005f, 84.9984f)
			};
			Quaternion[] array2;
			string[] array3;
			int num2;
			checked
			{
				int num = array.Length - 1;
				for (int i = 0; i <= num; i++)
				{
					Quaternion quaternion = array[i];
					string text = ((unchecked(i % 2) == 0) ? "female" : "male");
					string clip = "hi_dance_crowd_15_v2_" + text + "^" + Conversions.ToString(unchecked(i % 6) + 1);
					residents.Add(new Resident(models[unchecked(i % models.Length)], new Vector3(quaternion.X, quaternion.Y, quaternion.Z), quaternion.W, "anim@amb@nightclub@dancers@crowddance_groups@hi_intensity", clip));
				}
				array2 = new Quaternion[10]
				{
					new Quaternion(-1601.101f, -3007.519f, -77.005f, 204.9998f),
					new Quaternion(-1598.456f, -3007.338f, -77.005f, 189.9997f),
					new Quaternion(-1593.753f, -3007.614f, -77.005f, 169.9995f),
					new Quaternion(-1600.778f, -3002.237f, -76.8052f, 279.9997f),
					new Quaternion(-1597.622f, -8155f / MathF.E, -76.8052f, 184.9996f),
					new Quaternion(-1593.891f, -3001.232f, -76.8052f, 109.9995f),
					new Quaternion(-1580.195f, -3014.741f, -80.0059f, 204.9996f),
					new Quaternion(-1581.842f, -9471f / MathF.PI, -80.006f, 94.9995f),
					new Quaternion(-1576.229f, -3011.572f, -80.0059f, 134.9993f),
					new Quaternion(-1584.957f, -3012.322f, -80.0061f, 359.9953f)
				};
				array3 = new string[4] { "WORLD_HUMAN_DRINKING", "WORLD_HUMAN_PARTYING", "WORLD_HUMAN_HANG_OUT_STREET", "WORLD_HUMAN_STAND_MOBILE" };
				num2 = array2.Length - 1;
			}
			for (int j = 0; j <= num2; j = checked(j + 1))
			{
				Quaternion quaternion2 = array2[j];
				residents.Add(new Resident(models[checked(j + 1) % models.Length], new Vector3(quaternion2.X, quaternion2.Y, quaternion2.Z), quaternion2.W, null, null, array3[j % array3.Length]));
			}
		}

		public static void Register(List<ExpansionProperty> properties)
		{
			AddClub(properties, "la_mesa", "La Mesa Nightclub", 1500000, new Quaternion(757.009f, -1332.32f, 26.1802f, 354.7854f));
			AddClub(properties, "mission_row", "Mission Row Nightclub", 1440000, new Quaternion(345.7519f, -978.8848f, 28.2681f, 90f));
			AddClub(properties, "strawberry", "Strawberry Nightclub", 1525000, new Quaternion(-120.906f, -1260.49f, 28.2088f, 90f));
			AddClub(properties, "west_vinewood", "West Vinewood Nightclub", 1700000, new Quaternion(5.53709f, 221.35f, 106.6566f, 70f));
			AddClub(properties, "cypress_flats", "Cypress Flats Nightclub", 1370000, new Quaternion(871.47f, -2099.57f, 29.3768f, 271.9949f));
			AddClub(properties, "lsia", "LSIA Nightclub", 1135000, new Quaternion(-675.225f, -2459.15f, 12.8444f, 332f));
			AddClub(properties, "elysian_island", "Elysian Island Nightclub", 1080000, new Quaternion(195.534f, -3168.88f, 4.7903f, 270f));
			AddClub(properties, "downtown_vinewood", "Downtown Vinewood Nightclub", 1670000, new Quaternion(373.05f, 252.13f, 101.9097f, 160f));
			AddClub(properties, "del_perro", "Del Perro Nightclub", 1645000, new Quaternion(-1283.38f, -649.916f, 25.5198f, 212.1381f));
			AddClub(properties, "vespucci_canals", "Vespucci Canals Nightclub", 1320000, new Quaternion(-1174.85f, -1152.3f, 4.56128f, 105.5f));
		}

		private static void AddClub(List<ExpansionProperty> properties, string district, string name, int price, Quaternion entrance)
		{
			_Closure_0024__49_002D0 closure_0024__49_002D = new _Closure_0024__49_002D0();
			closure_0024__49_002D._0024VB_0024Local_key = "after_hours_" + district;
			ExpansionProperty expansionProperty = new ExpansionProperty
			{
				Key = closure_0024__49_002D._0024VB_0024Local_key,
				Name = name,
				Price = price,
				Entrance = entrance,
				BlipSprite = 614,
				Arrival = new ExpansionDestination
				{
					Key = closure_0024__49_002D._0024VB_0024Local_key + "_floor",
					Pose = Arrival,
					Anchor = Anchor,
					Ipl = "ba_int_placement_ba_interior_0_dlc_int_01_ba_milo_",
					InteriorType = "ba_dlc_int_01_ba",
					FallbackPose = HallArrival,
					ExitPoint = ExitPoint,
					Radius = 65f,
					EnableInterior = true,
					RetryResidency = true,
					Prepare = closure_0024__49_002D._Lambda_0024__0
				},
				OnTick = Tick,
				OnLeave = Leave,
				OnReset = Reset
			};
			NightclubGarage.Bind(expansionProperty);
			properties.Add(expansionProperty);
		}

		private static void Tick(ExpansionSession session)
		{
			if (liveSession != session)
			{
				Cleanup();
				liveSession = session;
				populationChoice = NightclubExpansionMenu.SelectedPopulation(session.Definition.Key);
				currentDj = NightclubExpansionMenu.SelectedDj(session.Definition.Key);
				visitClock.Restart();
				NightclubExpansionMenu.ApplyVisitDj(session, currentDj);
				StartLocalMusic();
			}
			if (requestedDj != null && visitClock.ElapsedMilliseconds >= nextResidentAt)
			{
				TickResident(session, true);
			}
			else if (populationPass < 3 && visitClock.ElapsedMilliseconds >= nextResidentAt)
			{
				TickPopulation(session);
			}
			NightclubProjector.Tick(session);
			NightclubStageFog.Tick(session);
			NightclubPodiumDancers.Tick(session);
			bool flag = NightclubTrackRequests.Tick(session);
			if (Game.IsPaused)
			{
				NightclubExpansionMenu.Tick(session);
				return;
			}
			bool flag2 = NightclubActivities.Tick(session);
			bool flag3 = NightclubCharacterScenes.Tick(session, !flag && !flag2);
			if (!flag && !flag2 && !flag3)
			{
				NightclubExpansionMenu.Tick(session);
			}
		}

		private static void TickPopulation(ExpansionSession session)
		{
			checked
			{
				while (residentIndex < Residents.Length && (completed.Contains(residentIndex) || !ResidentEnabled(residentIndex, populationChoice)))
				{
					residentIndex++;
				}
				if (residentIndex < Residents.Length)
				{
					TickResident(session, false);
					return;
				}
				populationPass++;
				if (completed.Count == Residents.Length || populationPass >= 3)
				{
					populationPass = 3;
					Logger.Log($"After Hours: placed {spawned.Count} of {Residents.Length} club residents.");
				}
				else
				{
					residentIndex = 0;
					nextResidentAt = visitClock.ElapsedMilliseconds + 1800;
				}
			}
		}

		private static bool ResidentEnabled(int index, int choice)
		{
			if (index < 0 || index >= Residents.Length)
			{
				return false;
			}
			Resident resident = Residents[index];
			if (!resident.ModelName.Contains("clubcust"))
			{
				return true;
			}
			if (choice < 0 || choice > 2)
			{
				choice = 2;
			}
			int num = ((Operators.CompareString(resident.Dictionary, "anim@amb@nightclub@dancers@crowddance_facedj@hi_intensity", false) != 0) ? ((Operators.CompareString(resident.Dictionary, "anim@amb@nightclub@dancers@crowddance_groups@hi_intensity", false) == 0) ? 1 : 2) : 0);
			int[][] array = new int[3][]
			{
				new int[3] { 6, 12, 16 },
				new int[3] { 3, 6, 9 },
				new int[3] { 3, 7, 10 }
			};
			int num2 = 0;
			checked
			{
				int num3 = index - 1;
				for (int i = 0; i <= num3; i++)
				{
					Resident resident2 = Residents[i];
					if (resident2.ModelName.Contains("clubcust"))
					{
						int num4 = ((Operators.CompareString(resident2.Dictionary, "anim@amb@nightclub@dancers@crowddance_facedj@hi_intensity", false) != 0) ? ((Operators.CompareString(resident2.Dictionary, "anim@amb@nightclub@dancers@crowddance_groups@hi_intensity", false) == 0) ? 1 : 2) : 0);
						if (num == num4)
						{
							num2++;
						}
					}
				}
				return num2 < array[num][choice];
			}
		}

		internal static void RefreshPopulation(ExpansionSession session)
		{
			if (session == null || session != liveSession)
			{
				return;
			}
			populationChoice = NightclubExpansionMenu.SelectedPopulation(session.Definition.Key);
			ReleasePendingModel();
			checked
			{
				for (int i = spawned.Count - 1; i >= 0; i += -1)
				{
					ResidentReceipt receipt = spawned[i];
					if (!ResidentEnabled(receipt.ResidentIndex, populationChoice) && DeleteResident(receipt))
					{
						spawned.RemoveAt(i);
					}
				}
				completed.Clear();
				foreach (ResidentReceipt item in spawned)
				{
					completed.Add(item.ResidentIndex);
				}
				residentIndex = 0;
				populationPass = 0;
				nextResidentAt = visitClock.ElapsedMilliseconds + 90;
			}
		}

		internal static NightclubDj CurrentVisitDj(ExpansionSession session)
		{
			if (session == liveSession)
			{
				return currentDj;
			}
			return null;
		}

		internal static void RefreshDj(ExpansionSession session)
		{
			if (session == null || session != liveSession)
			{
				return;
			}
			NightclubDj nightclubDj = NightclubExpansionMenu.SelectedDj(session.Definition.Key);
			if (requestedDj != null && Operators.CompareString(nightclubDj.ModelName, requestedDj.ModelName, false) == 0)
			{
				return;
			}
			if (Operators.CompareString(nightclubDj.ModelName, currentDj.ModelName, false) == 0)
			{
				if (requestedDj != null)
				{
					ReleasePendingModel();
					requestedDj = null;
					djSwapAttemptsLeft = 0;
					nextResidentAt = visitClock.ElapsedMilliseconds;
				}
			}
			else
			{
				ReleasePendingModel();
				requestedDj = nightclubDj;
				djSwapAttemptsLeft = 3;
				nextResidentAt = checked(visitClock.ElapsedMilliseconds + 500);
			}
		}

		private static void StartLocalMusic()
		{
			try
			{
				if (Function.Call<int>(Hash.IS_AUDIO_SCENE_ACTIVE, "DLC_Ba_NightClub_Scene") == 0)
				{
					ownsAudioScene = Function.Call<int>(Hash.START_AUDIO_SCENE, "DLC_Ba_NightClub_Scene") != 0;
					ApplyMixStation();
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("After Hours audio: " + ex2.Message);
				StopLocalMusic();
				ProjectData.ClearProjectError();
			}
		}

		private static void ApplyMixStation()
		{
			if (!ownsAudioScene || currentDj == null)
			{
				return;
			}
			string[] mixEmitters = MixEmitters;
			foreach (string text in mixEmitters)
			{
				try
				{
					Function.Call(Hash.SET_EMITTER_RADIO_STATION, text, currentDj.Station);
					if (!ownedEmitters.Contains(text))
					{
						Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, text, mixEmittersEnabled);
						ownedEmitters.Add(text);
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("After Hours radio emitter " + text + ": " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
		}

		internal static void SetMixEmittersEnabled(bool enabled)
		{
			if (!ownsAudioScene || mixEmittersEnabled == enabled)
			{
				return;
			}
			mixEmittersEnabled = enabled;
			foreach (string ownedEmitter in ownedEmitters)
			{
				try
				{
					Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, ownedEmitter, enabled);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("After Hours radio emitter " + ownedEmitter + ": " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
		}

		private static void StopLocalMusic()
		{
			foreach (string ownedEmitter in ownedEmitters)
			{
				try
				{
					Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, ownedEmitter, false);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("After Hours emitter cleanup: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
			ownedEmitters.Clear();
			if (ownsAudioScene)
			{
				ownsAudioScene = false;
				try
				{
					Function.Call(Hash.STOP_AUDIO_SCENE, "DLC_Ba_NightClub_Scene");
				}
				catch (Exception ex3)
				{
					ProjectData.SetProjectError(ex3);
					Exception ex4 = ex3;
					Logger.Log("After Hours audio cleanup: " + ex4.Message);
					ProjectData.ClearProjectError();
				}
			}
		}

		private static void TickResident(ExpansionSession session, bool replacingDj)
		{
			Resident resident = Residents[(!replacingDj) ? residentIndex : 0];
			NightclubDj nightclubDj = (replacingDj ? requestedDj : currentDj);
			string text = ((Operators.CompareString(resident.ModelName, "ig_sol", false) == 0) ? nightclubDj.ModelName : resident.ModelName);
			bool placed = default(bool);
			try
			{
				if (!requestClock.IsRunning)
				{
					pendingModel = new Model(text);
					if (!pendingModel.IsValid || !pendingModel.IsInCdImage)
					{
						Logger.Log("After Hours resident model unavailable: " + text);
						FinishResidentRequest(session, replacingDj, !replacingDj);
						return;
					}
					requestClock.Restart();
					if (!pendingModel.IsLoaded)
					{
						pendingModel.Request();
						ownsModel = true;
					}
					if (resident.Dictionary != null && !DictionaryLoaded(resident.Dictionary) && ownedDictionaries.Add(resident.Dictionary))
					{
						Function.Call(Hash.REQUEST_ANIM_DICT, resident.Dictionary);
					}
					return;
				}
				long elapsedMilliseconds = requestClock.ElapsedMilliseconds;
				bool num = elapsedMilliseconds >= 3000;
				bool flag = resident.Dictionary == null || DictionaryLoaded(resident.Dictionary);
				if (!num && (!pendingModel.IsLoaded || (!flag && !unavailableDictionaries.Contains(resident.Dictionary))))
				{
					return;
				}
				if (!flag)
				{
					unavailableDictionaries.Add(resident.Dictionary);
				}
				if (pendingModel.IsLoaded)
				{
					Vector3 floor = default(Vector3);
					if (TryResidentFloor(session, resident, ref floor))
					{
						placed = SpawnResident(session, resident, floor, flag);
					}
					else
					{
						if (elapsedMilliseconds < 500)
						{
							nextResidentAt = checked(visitClock.ElapsedMilliseconds + 70);
							return;
						}
						if (Operators.CompareString(resident.ModelName, "ig_sol", false) == 0)
						{
							Logger.Log("After Hours DJ: the raised platform did not provide a clear standing spot.");
						}
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("After Hours resident " + text + ": " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			FinishResidentRequest(session, replacingDj, placed);
		}

		private static bool DictionaryLoaded(string name)
		{
			return Function.Call<int>(Hash.HAS_ANIM_DICT_LOADED, name) != 0;
		}

		private static bool TryResidentFloor(ExpansionSession session, Resident resident, ref Vector3 floor)
		{
			Vector3 position = resident.Position;
			if (resident.SeatOrigin)
			{
				return NightclubCharacterScenes.TrySeatedPose(session, position, ref floor);
			}
			if (Operators.CompareString(resident.Dictionary, "anim@amb@nightclub@dancers@crowddance_facedj@hi_intensity", false) != 0)
			{
				return TryStandingFloor(session, position, position.Z - 0.45f, position.Z + 0.45f, ref floor);
			}
			IEnumerable<Vector3> occupied = spawned.Select((_Closure_0024__._0024I62_002D0 != null) ? _Closure_0024__._0024I62_002D0 : (_Closure_0024__._0024I62_002D0 = _Closure_0024__._0024I._Lambda_0024__62_002D0));
			Vector3[] danceFloorOffsets = DanceFloorOffsets;
			foreach (Vector3 vector in danceFloorOffsets)
			{
				Vector3 vector2 = position + vector;
				if (PartyPlacement.IsSeparated(vector2, occupied, 0.68f) && TryStandingFloor(session, vector2, position.Z - 0.45f, position.Z + 0.45f, ref floor))
				{
					return true;
				}
			}
			return false;
		}

		internal static bool TryStandingFloor(ExpansionSession session, Vector3 position, float minimumZ, float maximumZ, ref Vector3 floor)
		{
			Function.Call(Hash.REQUEST_COLLISION_AT_COORD, position.X, position.Y, position.Z);
			Vector3 start = new Vector3(position.X, position.Y, maximumZ);
			Vector3 finish = new Vector3(position.X, position.Y, minimumZ);
			bool hit = default(bool);
			Vector3 point = default(Vector3);
			Vector3 normal = default(Vector3);
			if (!PartyPlacement.IsUsableFloorHit(PartyPlacement.PhysicalRay(start, finish, 81, ref hit, ref point, ref normal, session.Actor), hit, point, normal.Z) || point.Z < minimumZ || point.Z > maximumZ || session.Actor.Position.DistanceToSquared(point + new Vector3(0f, 0f, 1f)) < 0.5625f)
			{
				return false;
			}
			int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS_WITH_TYPE, point.X, point.Y, point.Z + 0.75f, "ba_dlc_int_01_ba");
			if (num != session.Interior)
			{
				if (num != 0)
				{
					return false;
				}
				int num2 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, point.X, point.Y, point.Z + 0.75f);
				if (num2 != session.Interior && (num2 != 0 || Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, session.Actor.Handle) != session.Interior))
				{
					return false;
				}
			}
			Vector3 point2 = default(Vector3);
			Vector3 normal2 = default(Vector3);
			if (!PartyPlacement.BodyProbeClear(PartyPlacement.PhysicalRay(point + new Vector3(0f, 0f, 0.12f), point + new Vector3(0f, 0f, 1.75f), 81, ref hit, ref point2, ref normal2, session.Actor), hit))
			{
				return false;
			}
			floor = point;
			return true;
		}

		private static bool SpawnResident(ExpansionSession session, Resident resident, Vector3 floor, bool dictionaryReady)
		{
			Ped ped = World.CreatePed(pendingModel, floor, resident.Heading);
			if ((object)ped == null || !ped.Exists())
			{
				return false;
			}
			IntPtr memoryAddress = ped.MemoryAddress;
			if (memoryAddress == IntPtr.Zero)
			{
				ped.MarkAsNoLongerNeeded();
				return false;
			}
			bool flag = Operators.CompareString(resident.ModelName, "ig_sol", false) == 0;
			ResidentReceipt residentReceipt = default(ResidentReceipt);
			residentReceipt.Actor = ped;
			residentReceipt.Address = memoryAddress;
			residentReceipt.ModelHash = pendingModel.Hash;
			residentReceipt.IsDj = flag;
			residentReceipt.Floor = floor;
			residentReceipt.ResidentIndex = ((!flag) ? residentIndex : 0);
			try
			{
				ped.IsPersistent = true;
				if (resident.SeatOrigin)
				{
					Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, ped.Handle, floor.X, floor.Y, floor.Z, false, false, false);
				}
				else
				{
					Function.Call(Hash.SET_ENTITY_COORDS, ped.Handle, floor.X, floor.Y, floor.Z, false, false, false, false);
				}
				ped.IsPositionFrozen = true;
				ped.BlockPermanentEvents = true;
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				DeleteResident(residentReceipt);
				throw;
			}
			checked
			{
				if (flag)
				{
					for (int i = spawned.Count - 1; i >= 0; i += -1)
					{
						if (!spawned[i].IsDj)
						{
							continue;
						}
						if (!DeleteResident(spawned[i]))
						{
							if (!DeleteResident(residentReceipt))
							{
								residentReceipt.IsDj = false;
								spawned.Add(residentReceipt);
								try
								{
									ped.IsVisible = false;
									ped.IsCollisionEnabled = false;
								}
								catch (Exception ex)
								{
									ProjectData.SetProjectError(ex);
									Exception ex2 = ex;
									Logger.Log("After Hours DJ candidate cleanup: " + ex2.Message);
									ProjectData.ClearProjectError();
								}
							}
							return false;
						}
						spawned.RemoveAt(i);
					}
				}
				spawned.Add(residentReceipt);
				try
				{
					ApplyResidentActivity(session, resident, ped, dictionaryReady, floor);
				}
				catch (Exception ex3)
				{
					ProjectData.SetProjectError(ex3);
					Exception ex4 = ex3;
					Logger.Log($"After Hours resident activity {resident.ModelName}: {ex4.Message}");
					ProjectData.ClearProjectError();
				}
				return true;
			}
		}

		private static void ApplyResidentActivity(ExpansionSession session, Resident resident, Ped actor, bool dictionaryReady, Vector3 floor)
		{
			int num;
			if (resident.RoomName != null)
			{
				num = Game.GenerateHash(resident.RoomName);
			}
			else
			{
				num = Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, actor.Handle);
				if (Operators.CompareString(resident.Dictionary, "anim@amb@nightclub@dancers@crowddance_facedj@hi_intensity", false) == 0 || (num == 0 && !resident.SeatOrigin && !NightclubCharacterScenes.IsCameo(resident.ModelName)))
				{
					num = Game.GenerateHash("Int_01_main_area");
				}
			}
			if (num != 0)
			{
				Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, actor.Handle, session.Interior, num);
			}
			if (Operators.CompareString(resident.RoomName, "Int_01_Entry_hall", false) == 0)
			{
				Logger.Log(string.Format("After Hours cloakroom: assigned {0} at {1}.", "Int_01_Entry_hall", floor));
			}
			if (resident.Scenario != null)
			{
				Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, actor.Handle, resident.Scenario, -1, false);
			}
			else if (dictionaryReady && resident.Dictionary != null && Function.Call<float>(Hash.GET_ANIM_DURATION, resident.Dictionary, resident.Clip) > 0f)
			{
				AnimationFlags animationFlags = AnimationFlags.Loop;
				if (Operators.CompareString(resident.Dictionary, "anim@amb@nightclub@dancers@crowddance_facedj@hi_intensity", false) != 0)
				{
					animationFlags |= AnimationFlags.NotInterruptable;
				}
				Function.Call(Hash.TASK_PLAY_ANIM, actor.Handle, resident.Dictionary, resident.Clip, 4f, -4f, -1, (int)animationFlags, 0f, false, false, false);
				if (Operators.CompareString(resident.ModelName, "ig_sol", false) == 0)
				{
					Logger.Log($"After Hours DJ: turntable loop requested at {floor}.");
				}
			}
			else if (Operators.CompareString(resident.Dictionary, "anim@amb@nightclub@dancers@crowddance_facedj@hi_intensity", false) == 0 || Operators.CompareString(resident.Dictionary, "anim@amb@nightclub@dancers@crowddance_groups@hi_intensity", false) == 0)
			{
				Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, actor.Handle, "WORLD_HUMAN_PARTYING", -1, false);
			}
			else
			{
				Logger.Log($"After Hours animation unavailable: {resident.ModelName}; {resident.Dictionary}/{resident.Clip}.");
			}
		}

		internal static Ped ResidentByModel(string modelName)
		{
			int num = Game.GenerateHash(modelName);
			foreach (ResidentReceipt item in spawned)
			{
				if (item.ModelHash == num && ResidentMatches(item) && !item.Actor.IsDead)
				{
					return item.Actor;
				}
			}
			return null;
		}

		private static void FinishResidentRequest(ExpansionSession session, bool replacingDj, bool placed)
		{
			ReleasePendingModel();
			checked
			{
				nextResidentAt = visitClock.ElapsedMilliseconds + 90;
				if (replacingDj)
				{
					if (placed)
					{
						currentDj = requestedDj;
						requestedDj = null;
						djSwapAttemptsLeft = 0;
						completed.Add(0);
						NightclubExpansionMenu.ApplyVisitDj(session, currentDj);
						ApplyMixStation();
						return;
					}
					djSwapAttemptsLeft--;
					if (djSwapAttemptsLeft > 0)
					{
						nextResidentAt = visitClock.ElapsedMilliseconds + 1800;
						return;
					}
					requestedDj = null;
					Logger.Log("After Hours DJ: replacement could not be placed. The previous DJ is still active.");
					if (OnlineExpansion.IsCurrent(session))
					{
						Notification.Show("The booth is unavailable. The current DJ will keep playing.");
					}
				}
				else
				{
					if (placed)
					{
						completed.Add(residentIndex);
					}
					residentIndex++;
				}
			}
		}

		private static void ReleasePendingModel()
		{
			requestClock.Reset();
			if (ownsModel)
			{
				ownsModel = false;
				try
				{
					pendingModel.MarkAsNoLongerNeeded();
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("After Hours model cleanup: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
		}

		private static void Cleanup()
		{
			ReleasePendingModel();
			StopLocalMusic();
			foreach (ResidentReceipt item in spawned)
			{
				DeleteResident(item);
			}
			foreach (string ownedDictionary in ownedDictionaries)
			{
				try
				{
					Function.Call(Hash.REMOVE_ANIM_DICT, ownedDictionary);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("After Hours animation cleanup: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
			Forget();
		}

		private static bool ResidentMatches(ResidentReceipt receipt)
		{
			if (receipt.Actor.Exists() && receipt.Actor.MemoryAddress == receipt.Address)
			{
				return receipt.Actor.Model.Hash == receipt.ModelHash;
			}
			return false;
		}

		private static bool DeleteResident(ResidentReceipt receipt)
		{
			try
			{
				if (!ResidentMatches(receipt))
				{
					return true;
				}
				receipt.Actor.Delete();
				return !ResidentMatches(receipt);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("After Hours resident cleanup: " + ex2.Message);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		private static void Leave(ExpansionSession session)
		{
			try
			{
				NightclubProjector.Close(session);
				NightclubCharacterScenes.Close(session);
				NightclubTrackRequests.Close(session);
				NightclubActivities.Close(session);
				NightclubStageFog.Close(session);
				NightclubPodiumDancers.Close(session);
				NightclubExpansionMenu.Close(session);
			}
			finally
			{
				Cleanup();
			}
		}

		private static void Reset(bool storyLoad)
		{
			try
			{
				NightclubProjector.Reset(storyLoad);
				NightclubCharacterScenes.Reset(storyLoad);
				NightclubTrackRequests.Reset(storyLoad);
				NightclubActivities.Reset(storyLoad);
				NightclubStageFog.Reset(storyLoad);
				NightclubPodiumDancers.Reset(storyLoad);
				NightclubExpansionMenu.Reset(storyLoad);
			}
			finally
			{
				if (storyLoad)
				{
					Forget();
				}
				else
				{
					Cleanup();
				}
			}
		}

		private static void Forget()
		{
			liveSession = null;
			residentIndex = 0;
			populationPass = 0;
			nextResidentAt = 0L;
			requestClock.Reset();
			visitClock.Reset();
			pendingModel = default(Model);
			ownsModel = false;
			ownsAudioScene = false;
			currentDj = null;
			requestedDj = null;
			djSwapAttemptsLeft = 0;
			mixEmittersEnabled = true;
			populationChoice = 2;
			spawned.Clear();
			completed.Clear();
			ownedEmitters.Clear();
			ownedDictionaries.Clear();
			unavailableDictionaries.Clear();
		}
	}
}
