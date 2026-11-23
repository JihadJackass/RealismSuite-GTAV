using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using iFruitAddon2;
using INMNativeUI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class PartyService
	{
		internal class PartyPed
		{
			public Ped Ped;

			public string Role;

			public string ModelName;

			public string Dict;

			public string[] Dicts;

			public int DictIndex;

			public string[] Anims;

			public int AnimIndex;

			public string Fallback;

			public int CheckAt;

			public bool Settled;

			public bool AnimPlayingRequested;

			public int DictDeadline;

			public Vector3 HomePos;

			public float HomeHeading;

			public Vector3 FloorPos;

			public string Behavior;

			public Prop HandProp;

			public int NextBehaviorAt;

			public Vector3 WalkTarget;

			public int WalkingUntil;

			public int WalkStartAt;

			public PartyPed()
			{
				Dicts = new string[0];
				DictIndex = 0;
				Anims = new string[0];
				AnimIndex = -1;
				Fallback = "WORLD_HUMAN_PARTYING";
				CheckAt = 0;
				Settled = false;
				Behavior = "dance";
			}
		}

		internal struct BedInfo
		{
			public bool Found;

			public Vector3 Center;

			public Vector3 HeadDir;

			public Vector3 Side;

			public float TopZ;

			public float FloorZ;

			public int Cells;

			public bool OriginalScene;

			public Vector3 SceneOrigin;

			public float SceneHeading;

			public string AnimationDict;

			public string AnimationClip;

			public Mansion Home;
		}

		internal class RestingBedScene
		{
			public Ped Actor;

			public FwSyncedScene Scene;

			public bool WasFrozen;

			public string Dict;

			public string Clip;

			public int Handle;

			public int Model;

			public IntPtr Address;

			public PartyPed Source;

			public Mansion Home;

			public eOwner Owner;

			internal BedInfo Bed;
		}

		private class GroupBedroomState
		{
			public RestingBedScene FirstRest;

			public PartyPed Second;

			public Ped Player;

			public int PlayerHandle;

			public int PlayerModel;

			public IntPtr PlayerAddress;

			public int SecondHandle;

			public int SecondModel;

			public IntPtr SecondAddress;

			public Vector3 JoinFloor;

			public int Deadline;

			public int DanceStarted;

			public string PlayerClip;

			public bool DancesStarted;

			public bool FirstResumed;

			public int ProbeIndex;

			public int NextRestVerify;

			public bool SlotMeasured;

			public BedInfo SecondBed;

			public Vector3 BodyDirection;

			public Vector3 LateralDirection;

			public Vector3 FirstPelvis;

			public Vector3 FirstHead;

			public Vector3 SecondPelvisTarget;

			public Vector3 SecondHeadTarget;

			public float SurfaceZ;
		}

		private class MorningPairState
		{
			public RestingBedScene First;

			public RestingBedScene Second;

			public eOwner Owner;

			public PartyCompanionPolicy.MorningGate Greeting;

			public MorningPairState()
			{
				Greeting = new PartyCompanionPolicy.MorningGate();
			}
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<string, string> _0024I100_002D0;

			public static Func<string, bool> _0024I100_002D1;

			public static Func<string, string> _0024I101_002D0;

			public static Func<int, object> _0024I105_002D0;

			public static Func<int, object> _0024I105_002D1;

			public static Func<string, object> _0024I105_002D2;

			public static Func<string, object> _0024I105_002D3;

			public static Func<string, int> _0024I123_002D0;

			public static Func<string, bool> _0024I130_002D0;

			internal static Func<PartyPed, bool> _0024I140_002D0;

			internal static Func<PartyPed, bool> _0024I140_002D1;

			internal static Func<PartyPed, bool> _0024I141_002D0;

			internal static Func<PartyPed, bool> _0024I141_002D1;

			internal static Func<PartyPed, bool> _0024I142_002D0;

			internal static Func<RestingBedScene, bool> _0024I192_002D0;

			internal static Func<PartyPed, float> _0024I195_002D1;

			public static Func<Mansion, bool> _0024I210_002D0;

			public static Func<Mansion, float> _0024I210_002D1;

			internal static Func<RestingBedScene, bool> _0024I210_002D2;

			internal static Func<RestingBedScene, bool> _0024I247_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal string _Lambda_0024__100_002D0(string x)
			{
				return x.Trim();
			}

			[SpecialName]
			internal bool _Lambda_0024__100_002D1(string x)
			{
				return x.Length > 0;
			}

			[SpecialName]
			internal string _Lambda_0024__101_002D0(string x)
			{
				if (!x.Equals("ig_blackmadonna", StringComparison.OrdinalIgnoreCase))
				{
					return x;
				}
				return "ig_djblamadon";
			}

			[SpecialName]
			internal object _Lambda_0024__105_002D0(int x)
			{
				return x.ToString();
			}

			[SpecialName]
			internal object _Lambda_0024__105_002D1(int x)
			{
				return x.ToString();
			}

			[SpecialName]
			internal object _Lambda_0024__105_002D2(string x)
			{
				return x;
			}

			[SpecialName]
			internal object _Lambda_0024__105_002D3(string x)
			{
				return x;
			}

			[SpecialName]
			internal int _Lambda_0024__123_002D0(string x)
			{
				return rnd.Next();
			}

			[SpecialName]
			internal bool _Lambda_0024__130_002D0(string x)
			{
				return !string.IsNullOrWhiteSpace(x);
			}

			[SpecialName]
			internal bool _Lambda_0024__140_002D0(PartyPed actor)
			{
				if (CanRefreshPartyActor("dancer", actor.Role, actor == companion, actor == privateDancer) && (object)actor.Ped != null)
				{
					return actor.Ped.Exists();
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__140_002D1(PartyPed actor)
			{
				if (CanRefreshPartyActor("dancer", actor.Role, actor == companion, actor == privateDancer) && (object)actor.Ped != null)
				{
					return actor.Ped.Exists();
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__141_002D0(PartyPed x)
			{
				if (CanRefreshPartyActor("guest", x.Role, x == companion, x == privateDancer) && (object)x.Ped != null)
				{
					return x.Ped.Exists();
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__141_002D1(PartyPed x)
			{
				if (CanRefreshPartyActor("guest", x.Role, x == companion, x == privateDancer) && (object)x.Ped != null)
				{
					return x.Ped.Exists();
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__142_002D0(PartyPed x)
			{
				if (Operators.CompareString(x.Role, "dj", false) == 0 && (object)x.Ped != null)
				{
					return x.Ped.Exists();
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__192_002D0(RestingBedScene x)
			{
				return (object)x.Actor == privateDancer.Ped;
			}

			[SpecialName]
			internal float _Lambda_0024__195_002D1(PartyPed x)
			{
				return x.Ped.Position.DistanceTo(Helper.PP.Position);
			}

			[SpecialName]
			internal bool _Lambda_0024__210_002D0(Mansion x)
			{
				return PartyBedroomPlacement.CanUseOriginalBed(MansionAutoLayout.MainAnchor(x) != Vector3.Zero, x.Enabled, x.IsOwnedByPlayer(), x.IplsLoaded, x.InteriorProbe.DistanceTo(Helper.PP.Position));
			}

			[SpecialName]
			internal float _Lambda_0024__210_002D1(Mansion x)
			{
				return x.InteriorProbe.DistanceTo(Helper.PP.Position);
			}

			[SpecialName]
			internal bool _Lambda_0024__210_002D2(RestingBedScene x)
			{
				if (OwnRestingActor(x))
				{
					return x.Actor.IsDead;
				}
				return true;
			}

			[SpecialName]
			internal bool _Lambda_0024__247_002D0(RestingBedScene r)
			{
				if (OwnRestingActor(r) && !r.Actor.IsDead && r.Home != null && r.Home.Enabled && r.Home.IplsLoaded && r.Home.IsOwnedByPlayer() && r.Owner == Helper.GetPlayer() && r.Source != null && partyPeds.Contains(r.Source) && PartyCompanionPolicy.KnownAdultModel(r.Source.ModelName) && r.Bed.OriginalScene)
				{
					return r.Actor.Position.DistanceTo(Helper.PP.Position) <= 8f;
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__195_002D0
		{
			public HashSet<string> _0024VB_0024Local_requestedModels;

			[SpecialName]
			internal bool _Lambda_0024__0(PartyPed x)
			{
				if (Operators.CompareString(x.Role, "dancer", false) == 0 && (object)x.Ped != null && _0024VB_0024Local_requestedModels.Contains(x.ModelName) && x.Ped.Exists() && !x.Ped.IsDead && Math.Abs(x.Ped.Position.Z - Helper.PP.Position.Z) < 1.5f)
				{
					return x.Ped.Position.DistanceTo(Helper.PP.Position) <= 20f;
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__210_002D0
		{
			public Quaternion _0024VB_0024Local_scene;

			public _Closure_0024__210_002D0(_Closure_0024__210_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_scene = arg0._0024VB_0024Local_scene;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__3(RestingBedScene x)
			{
				return PartyBedroomPlacement.BedIsOccupied(OwnRestingActor(x) && !x.Actor.IsDead, OwnRestingActor(x) ? x.Actor.Position.DistanceTo(Helper.ToVector3(_0024VB_0024Local_scene)) : float.MaxValue);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__247_002D0
		{
			internal RestingBedScene _0024VB_0024Local_first;

			[SpecialName]
			internal bool _Lambda_0024__1(RestingBedScene r)
			{
				if (OwnRestingActor(r))
				{
					return r.Actor.Position.DistanceTo(_0024VB_0024Local_first.Bed.SceneOrigin) <= 3f;
				}
				return false;
			}
		}

		private static readonly string iniPath = "scripts\\SPA II\\Party.ini";

		private static ScriptSettings cfg;

		private static List<PartyPed> partyPeds = new List<PartyPed>();

		private static List<Prop> partyProps = new List<Prop>();

		private static bool partyActive = false;

		private static int partyEndAt = 0;

		private static Vector3 partyCenter;

		private static int lastMaintain = 0;

		private static string djModelName = "";

		private static Vector3 djFloorPos;

		private static float djHeading;

		private static int nextDjRetryAt;

		private static bool partyStarting = false;

		private static bool pendingGuestRefresh;

		private static bool pendingDancerRefresh;

		private static bool pendingDancerAdjustment;

		private static readonly HashSet<string> ownedAnimationDictionaries = new HashSet<string>(StringComparer.Ordinal);

		public const int MaximumPartyGuests = 27;

		private static int Cost = 5000;

		private static int DurationMinutes = 0;

		private static int GuestCount = 6;

		private static int DancerCount = 3;

		private static string GuestTheme = "Mix";

		private static string DancerLook = "Mix";

		public static string PrivateAppearance = "";

		public static string PrivateAppearanceCategory = "Mix";

		private static string RadioStation = "RADIO_22_DLC_BATTLE_MIX1_RADIO";

		private static bool UseDjSet = true;

		private static bool MusicEnabled = true;

		private static float MusicRadius = 50f;

		private static bool musicOwned = false;

		private static bool musicRouteActive = false;

		private static bool localRadioSuspended;

		private static string previousMusicStation = "OFF";

		private static bool previousMobileRadio = false;

		private static bool previousWasApartmentRadio;

		private static Vector3 previousApartmentRadioPosition;

		private static string playingMusicStation = "";

		private static string forcedTrackStation = "";

		private static int musicVerifyAt = 0;

		private static int musicRecoveryCount = 0;

		private static bool musicBaseStationFallback = false;

		private static string[] RadioStations = new string[24]
		{
			"RADIO_22_DLC_BATTLE_MIX1_RADIO", "RADIO_01_CLASS_ROCK", "RADIO_02_POP", "RADIO_03_HIPHOP_NEW", "RADIO_04_PUNK", "RADIO_06_COUNTRY", "RADIO_07_DANCE_01", "RADIO_08_MEXICAN", "RADIO_09_HIPHOP_OLD", "RADIO_12_REGGAE",
			"RADIO_13_JAZZ", "RADIO_14_DANCE_02", "RADIO_15_MOTOWN", "RADIO_16_SILVERLAKE", "RADIO_17_FUNK", "RADIO_18_90S_ROCK", "RADIO_20_THELAB", "RADIO_21_DLC_XM17", "RADIO_23_DLC_XM19_RADIO", "RADIO_27_DLC_PRHEI4",
			"RADIO_34_DLC_HEI4_KULT", "RADIO_35_DLC_HEI4_MLR", "RADIO_36_AUDIOPLAYER", "RADIO_37_MOTOMAMI"
		};

		private static string[] DjModels = new string[4] { "ig_sol", "ig_talcc", "ig_dix", "ig_djblamadon" };

		private static string[] DancerModels = new string[8] { "s_f_y_stripper_01", "s_f_y_stripper_02", "a_f_y_topless_01", "s_f_y_clubbar_01", "s_f_y_stripperlite", "a_f_y_beach_01", "a_f_y_hipster_02", "a_f_y_clubcust_03" };

		private static string[] GuestModels = new string[11]
		{
			"a_f_y_clubcust_01", "a_f_y_clubcust_02", "a_f_y_clubcust_03", "a_m_y_clubcust_01", "a_m_y_clubcust_02", "a_m_y_clubcust_03", "a_f_y_beach_01", "a_f_y_hipster_02", "a_f_y_vinewood_01", "a_f_y_vinewood_02",
			"a_m_y_vinewood_01"
		};

		private static string[] DjDicts = new string[4] { "anim@amb@nightclub@djs@solomun@", "anim@amb@nightclub@djs@tale_of_us@", "anim@amb@nightclub@djs@dixon@", "anim@amb@nightclub@djs@black_madonna@" };

		private static string[] DjAnims = new string[12]
		{
			"sol_idle_a_sol", "sol_idle_b_sol", "sol_dance_cntr_open_sol", "tale_idle_a_tale", "tale_dance_cntr_open_tale", "dixn_idle_a_dix", "dixn_dance_cntr_open_dix", "bm_idle_a_bm", "bm_dance_cntr_open_bm", "temp_dj_idle_01",
			"idle_a", "idle_b"
		};

		private static string[] DancerDicts = new string[3] { "anim@amb@nightclub@dancers@podium_dancers@", "anim@amb@nightclub@dancers@crowddance_facedj@hi_intensity", "anim@amb@nightclub@dancers@crowddance_facedj@med_intensity" };

		private static string[] DancerAnims = new string[9] { "hi_idle_a_f01", "hi_idle_b_f01", "hi_idle_c_f01", "hi_dance_facedj_09_v1_female^1", "hi_dance_facedj_11_v1_female^1", "hi_dance_facedj_13_v1_female^1", "hi_dance_facedj_15_v1_female^1", "hi_dance_facedj_17_v1_female^1", "med_dance_facedj_09_v1_female^1" };

		private static string[] GuestDicts = new string[3] { "anim@amb@nightclub@dancers@crowddance_facedj@hi_intensity", "anim@amb@nightclub@dancers@crowddance_facedj@med_intensity", "anim@amb@nightclub@dancers@crowddance_single_props@hi_intensity" };

		private static string[] GuestAnimsF = new string[6] { "hi_dance_facedj_09_v1_female^1", "hi_dance_facedj_11_v1_female^1", "hi_dance_facedj_13_v1_female^1", "hi_dance_facedj_15_v1_female^1", "med_dance_facedj_09_v1_female^1", "hi_dance_prop_01_v1_female^1" };

		private static string[] GuestAnimsM = new string[6] { "hi_dance_facedj_09_v1_male^1", "hi_dance_facedj_11_v1_male^1", "hi_dance_facedj_13_v1_male^1", "hi_dance_facedj_15_v1_male^1", "med_dance_facedj_09_v1_male^1", "hi_dance_prop_01_v1_male^1" };

		private static string[] GuestScenarios = new string[6] { "WORLD_HUMAN_DRINKING", "WORLD_HUMAN_DRINKING", "WORLD_HUMAN_PARTYING", "WORLD_HUMAN_SMOKING", "WORLD_HUMAN_SMOKING_POT", "WORLD_HUMAN_STAND_MOBILE" };

		private static int BedroomCost = 500;

		private static string BedroomDict = "mini@strip_club@private_dance@part1";

		private static string[] BedroomAnims = new string[1] { "priv_dance_p1" };

		private static int BedroomDurationSec = 25;

		private static bool bedroomBusy = false;

		private static PartyPed companion = null;

		private static int companionState = 0;

		private static bool privateDancerActive = false;

		private static PartyPed privateDancer;

		private static Vector3 privateDancerCenter;

		private static int privateDancerExpiresAt;

		private static bool privateRequestPending;

		private static Ped privateRequestPlayer;

		private static eOwner privateRequestOwner;

		private static Vector3 privateRequestOrigin;

		private static string[] privateRequestCandidates = new string[0];

		private static int privateRequestIndex;

		private static Model? privateRequestModel;

		private static int privateRequestModelDeadline;

		private static PartyPed privateRequestExisting;

		private static readonly Random rnd = new Random();

		private static UIMenuListItem stationItem;

		private static UIMenuListItem guestsItem;

		private static UIMenuListItem dancersItem;

		private static UIMenuListItem guestThemeItem;

		private static UIMenuListItem dancerLookItem;

		private static UIMenuCheckboxItem djSetItem;

		private static UIMenuCheckboxItem musicItem;

		private static int DrinkEffectSeconds = 60;

		private static int SmokeEffectSeconds = 45;

		private static int drunkUntil = 0;

		private static int highUntil = 0;

		private static int drunkLevel = 0;

		private static Ped consumePed;

		private static string consumeKind;

		private static string consumeScenario;

		private static Vector3 consumeOrigin;

		private static int consumeStartedAt;

		private static int consumeDuration;

		private static bool consumeRequiresParty;

		private static Ped buzzPed;

		private static string pendingDrunkClip;

		private static int pendingDrunkClipDeadline;

		private static string KissDict = "hs3_ext-20";

		private static string KissAnimM = "cs_lestercrest_3_dual-20";

		private static string KissAnimF = "csb_georginacheng_dual-20";

		private static string BedDict = "anim@mp_bedmid@right_var_01";

		private static string BedAnimF = "f_sleep_r_loop_bighouse";

		private static int bedroomPhase;

		private static int bedroomPhaseAt;

		private static Ped bedroomActor;

		private static Ped bedroomPlayer;

		private static int bedroomActorHandle;

		private static int bedroomActorModel;

		private static int bedroomPlayerHandle;

		private static int bedroomPlayerModel;

		private static IntPtr bedroomActorAddress;

		private static IntPtr bedroomPlayerAddress;

		private static PartyPed bedroomSource;

		private static Vector3 bedroomStartPosition;

		private static int bedroomStartedAt;

		private static string bedroomPrivateClip;

		private static BedInfo bedroomBed;

		private static int bedroomAlignPass;

		private static bool bedroomWasFrozen;

		private static bool bedroomFrozeActor;

		private static bool bedroomFaded;

		private static eOwner bedroomOwner;

		private static Vector3 bedroomActorReturnPosition;

		private static float bedroomActorReturnHeading;

		private static bool bedroomActorMoved;

		private static FwSyncedScene bedroomRestScene;

		private static readonly List<RestingBedScene> restingBedScenes = new List<RestingBedScene>();

		private static GroupBedroomState bedroomGroup;

		private static MorningPairState morningPair;

		private static int nextMorningClockAt;

		[System.Runtime.CompilerServices.AccessedThroughProperty("PartyContact")]
		private static iFruitContact _PartyContact;

		public static iFruitContact PartyContact
		{
			[CompilerGenerated]
			get
			{
				return _PartyContact;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ContactAnsweredEvent value2 = PartyContact_Answered;
				iFruitContact partyContact = _PartyContact;
				if (partyContact != null)
				{
					partyContact.Answered -= value2;
				}
				_PartyContact = value;
				partyContact = _PartyContact;
				if (partyContact != null)
				{
					partyContact.Answered += value2;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("PartyMenu")]
		private static UIMenu _PartyMenu;

		public static UIMenu PartyMenu
		{
			[CompilerGenerated]
			get
			{
				return _PartyMenu;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ListChangedEvent value2 = PartyMenu_OnListChange;
				CheckboxChangeEvent value3 = PartyMenu_OnCheckboxChange;
				ItemSelectEvent value4 = PartyMenu_OnItemSelect;
				UIMenu partyMenu = _PartyMenu;
				if (partyMenu != null)
				{
					partyMenu.OnListChange -= value2;
					partyMenu.OnCheckboxChange -= value3;
					partyMenu.OnItemSelect -= value4;
				}
				_PartyMenu = value;
				partyMenu = _PartyMenu;
				if (partyMenu != null)
				{
					partyMenu.OnListChange += value2;
					partyMenu.OnCheckboxChange += value3;
					partyMenu.OnItemSelect += value4;
				}
			}
		}

		public static bool IsPartyActive => partyActive;

		public static bool CompanionSessionActive
		{
			get
			{
				if (!partyActive)
				{
					return privateDancerActive;
				}
				return true;
			}
		}

		public static bool IsPlayerActionActive
		{
			get
			{
				if ((object)consumePed == null && !bedroomBusy && !privateRequestPending && !MansionPetCare.IsBusy && !MansionKitchen.IsBusy && !MansionCigarLounge.IsBusy && !MansionArcade.IsPlaying && !ApartmentHomeServices.IsBusy)
				{
					return SouthLSHousingTravel.IsBusy;
				}
				return true;
			}
		}

		private static void RequestPartyDictionary(string dict)
		{
			if (!string.IsNullOrWhiteSpace(dict) && ownedAnimationDictionaries.Add(dict))
			{
				Function.Call(Hash.REQUEST_ANIM_DICT, dict);
			}
		}

		private static void ReleasePartyDictionaries()
		{
			foreach (string ownedAnimationDictionary in ownedAnimationDictionaries)
			{
				try
				{
					Function.Call(Hash.REMOVE_ANIM_DICT, ownedAnimationDictionary);
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					ProjectData.ClearProjectError();
				}
			}
			ownedAnimationDictionaries.Clear();
			PartyPlayerActions.Cleanup();
		}

		public static int ClampPartyGuestCount(int value)
		{
			return Math.Max(2, Math.Min(27, value));
		}

		public static string GuestBehaviorForIndex(int index)
		{
			return PartyGuestProfiles.GuestBehaviorForIndex(index);
		}

		private static string[] ReadList(string section, string key, string[] def)
		{
			string[] array = cfg.GetValue(section, key, string.Join(",", def)).Split(new char[1] { ',' }).Select((_Closure_0024__._0024I100_002D0 != null) ? _Closure_0024__._0024I100_002D0 : (_Closure_0024__._0024I100_002D0 = _Closure_0024__._0024I._Lambda_0024__100_002D0))
				.Where((_Closure_0024__._0024I100_002D1 != null) ? _Closure_0024__._0024I100_002D1 : (_Closure_0024__._0024I100_002D1 = _Closure_0024__._0024I._Lambda_0024__100_002D1))
				.ToArray();
			if (array.Length != 0)
			{
				return array;
			}
			return def;
		}

		public static void LoadPartyConfig()
		{
			try
			{
				File.Exists(iniPath);
				cfg = ScriptSettings.Load(iniPath);
				Cost = Math.Max(0, cfg.GetValue("PARTY", "Cost", Cost));
				DurationMinutes = Math.Max(0, Math.Min(1440, cfg.GetValue("PARTY", "DurationMinutes", DurationMinutes)));
				GuestCount = ClampPartyGuestCount(cfg.GetValue("PARTY", "Guests", GuestCount));
				DancerCount = PartyGuestProfiles.ClampDancerCount(cfg.GetValue("PARTY", "Dancers", DancerCount));
				GuestTheme = PartyGuestProfiles.NormalizeGuestTheme(cfg.GetValue("PARTY", "GuestTheme", "Mix"));
				DancerLook = PartyGuestProfiles.NormalizeDancerLook(cfg.GetValue("PARTY", "DancerLook", "Mix"));
				RadioStation = cfg.GetValue("PARTY", "RadioStation", RadioStation);
				UseDjSet = cfg.GetValue("PARTY", "UseDJSet", UseDjSet);
				MusicEnabled = cfg.GetValue("PARTY", "MusicEnabled", true);
				MusicRadius = Math.Max(10, Math.Min(120, cfg.GetValue("PARTY", "MusicRadius", 50)));
				BedroomCost = Math.Max(0, cfg.GetValue("BEDROOM", "Cost", BedroomCost));
				BedroomDurationSec = Math.Max(5, Math.Min(120, cfg.GetValue("BEDROOM", "DurationSeconds", BedroomDurationSec)));
				BedroomDict = cfg.GetValue("BEDROOM", "AnimDict", BedroomDict);
				BedroomAnims = ReadList("BEDROOM", "Anims", BedroomAnims);
				KissDict = cfg.GetValue("BEDROOM", "KissDict", KissDict);
				KissAnimM = cfg.GetValue("BEDROOM", "KissAnimMale", KissAnimM);
				KissAnimF = cfg.GetValue("BEDROOM", "KissAnimFemale", KissAnimF);
				if (Operators.CompareString(KissDict, "mp_ped_interaction", false) == 0 && Operators.CompareString(KissAnimM, "kisses_guy_a", false) == 0 && Operators.CompareString(KissAnimF, "kisses_girl_a", false) == 0)
				{
					KissDict = "hs3_ext-20";
					KissAnimM = "cs_lestercrest_3_dual-20";
					KissAnimF = "csb_georginacheng_dual-20";
				}
				BedDict = cfg.GetValue("BEDROOM", "BedDict", BedDict);
				BedAnimF = cfg.GetValue("BEDROOM", "BedAnimFemale", BedAnimF);
				DjModels = ReadList("MODELS", "DJ", DjModels).Select((_Closure_0024__._0024I101_002D0 != null) ? _Closure_0024__._0024I101_002D0 : (_Closure_0024__._0024I101_002D0 = _Closure_0024__._0024I._Lambda_0024__101_002D0)).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
				DancerModels = ReadList("MODELS", "Dancers", DancerModels);
				PrivateAppearance = cfg.GetValue("BEDROOM", "PrivateDancerModel", "").Trim();
				if (!PrivateAppearanceModels().Contains(PrivateAppearance, StringComparer.OrdinalIgnoreCase))
				{
					PrivateAppearance = "";
				}
				PrivateAppearanceCategory = PrivateDancerPreview.NormalizeCategory(cfg.GetValue("BEDROOM", "PrivateDancerCategory", "")) ?? PrivateDancerPreview.CategoryForModel(PrivateAppearance);
				if (!PrivateDancerPreview.ModelsForCategory(PrivateAppearanceCategory, PrivateAppearanceModels()).Contains(PrivateAppearance, StringComparer.OrdinalIgnoreCase))
				{
					PrivateAppearanceCategory = PrivateDancerPreview.CategoryForModel(PrivateAppearance);
				}
				GuestModels = ReadList("MODELS", "Guests", GuestModels);
				DrinkEffectSeconds = cfg.GetValue("PARTY", "DrinkEffectSeconds", DrinkEffectSeconds);
				SmokeEffectSeconds = cfg.GetValue("PARTY", "SmokeEffectSeconds", SmokeEffectSeconds);
				DjDicts = ReadList("ANIMS", "DJDicts", DjDicts);
				DjAnims = ReadList("ANIMS", "DJ", DjAnims);
				DancerDicts = ReadList("ANIMS", "DancerDicts", DancerDicts);
				DancerAnims = ReadList("ANIMS", "Dancers", DancerAnims);
				GuestDicts = ReadList("ANIMS", "GuestDicts", GuestDicts);
				GuestAnimsF = ReadList("ANIMS", "GuestsFemale", GuestAnimsF);
				GuestAnimsM = ReadList("ANIMS", "GuestsMale", GuestAnimsM);
				GuestScenarios = ReadList("ANIMS", "GuestScenarios2", GuestScenarios);
				SavePartyConfig();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Party config: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
		}

		private static void SavePartyConfig()
		{
			cfg.SetValue("PARTY", "Cost", Cost);
			cfg.SetValue("PARTY", "DurationMinutes", DurationMinutes);
			cfg.SetValue("PARTY", "Guests", GuestCount);
			cfg.SetValue("PARTY", "Dancers", DancerCount);
			cfg.SetValue("PARTY", "GuestTheme", GuestTheme);
			cfg.SetValue("PARTY", "DancerLook", DancerLook);
			cfg.SetValue("BEDROOM", "PrivateDancerModel", PrivateAppearance);
			cfg.SetValue("BEDROOM", "PrivateDancerCategory", PrivateAppearanceCategory);
			cfg.SetValue("PARTY", "RadioStation", RadioStation);
			cfg.SetValue("PARTY", "UseDJSet", UseDjSet);
			cfg.SetValue("PARTY", "MusicEnabled", MusicEnabled);
			cfg.SetValue("PARTY", "MusicRadius", checked((int)Math.Round(MusicRadius)));
			cfg.SetValue("BEDROOM", "Cost", BedroomCost);
			cfg.SetValue("BEDROOM", "DurationSeconds", BedroomDurationSec);
			cfg.SetValue("BEDROOM", "AnimDict", BedroomDict);
			cfg.SetValue("BEDROOM", "Anims", string.Join(",", BedroomAnims));
			cfg.SetValue("BEDROOM", "KissDict", KissDict);
			cfg.SetValue("BEDROOM", "KissAnimMale", KissAnimM);
			cfg.SetValue("BEDROOM", "KissAnimFemale", KissAnimF);
			cfg.SetValue("BEDROOM", "BedDict", BedDict);
			cfg.SetValue("BEDROOM", "BedAnimFemale", BedAnimF);
			cfg.SetValue("MODELS", "DJ", string.Join(",", DjModels));
			cfg.SetValue("MODELS", "Dancers", string.Join(",", DancerModels));
			cfg.SetValue("MODELS", "Guests", string.Join(",", GuestModels));
			cfg.SetValue("PARTY", "DrinkEffectSeconds", DrinkEffectSeconds);
			cfg.SetValue("PARTY", "SmokeEffectSeconds", SmokeEffectSeconds);
			cfg.SetValue("ANIMS", "DJDicts", string.Join(",", DjDicts));
			cfg.SetValue("ANIMS", "DJ", string.Join(",", DjAnims));
			cfg.SetValue("ANIMS", "DancerDicts", string.Join(",", DancerDicts));
			cfg.SetValue("ANIMS", "Dancers", string.Join(",", DancerAnims));
			cfg.SetValue("ANIMS", "GuestDicts", string.Join(",", GuestDicts));
			cfg.SetValue("ANIMS", "GuestsFemale", string.Join(",", GuestAnimsF));
			cfg.SetValue("ANIMS", "GuestsMale", string.Join(",", GuestAnimsM));
			cfg.SetValue("ANIMS", "GuestScenarios2", string.Join(",", GuestScenarios));
			cfg.Save();
		}

		public static void LoadPartyContact()
		{
			PartyContact = new iFruitContact("Party Service");
			iFruitContact partyContact = PartyContact;
			partyContact.DialTimeout = 3000;
			partyContact.Active = true;
			partyContact.Icon = ContactIcon.SocialClub;
			Mechanic.iFruit.Contacts.Add(PartyContact);
			PartyMenu = new UIMenu("", "PARTY SERVICE", new Point(0, -107));
			PartyMenu.SetBannerType(Helper.MenuBanner);
			PartyMenu.MouseEdgeEnabled = false;
			Helper.MenuPool.Add(PartyMenu);
			RebuildMenu();
		}

		private static string StationDisplay(string name)
		{
			string text = Game.GetLocalizedString(name);
			if (string.IsNullOrEmpty(text) || Operators.CompareString(text, "NULL", false) == 0)
			{
				text = name.Replace("RADIO_", "").Replace("DLC_", "");
			}
			return text;
		}

		private static void RebuildMenu()
		{
			PartyMenu.MenuItems.Clear();
			UIMenuItem uIMenuItem = new UIMenuItem("Throw a party here", "Starts a party at your current position (DJ, dancers, guests).");
			uIMenuItem.SetRightLabel($"${Cost:0,0}");
			uIMenuItem.Tag = "Start";
			UIMenuItem item = new UIMenuItem("End the party", "Sends everyone home.")
			{
				Tag = "Stop",
				Enabled = partyActive
			};
			UIMenuItem item2 = new UIMenuItem("Have a drink", "Grab a drink at the party. Stacks - three and you can barely walk.")
			{
				Tag = "Drink",
				Enabled = partyActive
			};
			UIMenuItem item3 = new UIMenuItem("Smoke a joint", "Light one up.")
			{
				Tag = "Smoke",
				Enabled = partyActive
			};
			UIMenuItem item4 = new UIMenuItem(PartyPlayerActions.IsDancing ? "Stop dancing" : "Dance with the guests", "Dance at your current spot. Move or press Back to stop.")
			{
				Tag = "Dance",
				Enabled = partyActive
			};
			UIMenuItem uIMenuItem2 = new UIMenuItem("Private dancer / 3D preview", "Choose an appearance in the preview, then call the selected dancer. A party is not required.")
			{
				Tag = "Private",
				Enabled = (!IsPlayerActionActive && companionState == 0)
			};
			uIMenuItem2.SetRightLabel($"${BedroomCost:N0}");
			musicItem = new UIMenuCheckboxItem("Party music", MusicEnabled, "Play music in the party area, including inside the mansion.");
			UIMenuItem item5 = new UIMenuItem("Own MP3s", "Choose tracks from scripts\\SPA II\\NightclubMusic, skip a track or return to the DJ/radio.")
			{
				Tag = "OwnMP3",
				Enabled = partyActive
			};
			UIMenuItem item6 = new UIMenuItem("Restart party music", "Retune the party music if another script has stopped the radio.")
			{
				Tag = "RestartMusic",
				Enabled = partyActive
			};
			djSetItem = new UIMenuCheckboxItem("Play the DJ's own set", UseDjSet, "Solomun, Tale Of Us, Dixon or The Black Madonna play their Los Santos Underground Radio set.");
			List<object> list = new List<object>();
			string[] radioStations = RadioStations;
			foreach (string name in radioStations)
			{
				list.Add(StationDisplay(name));
			}
			int index = Math.Max(0, Array.IndexOf(RadioStations, RadioStation));
			stationItem = new UIMenuListItem("Station", list, index);
			guestsItem = new UIMenuListItem("Guests", Enumerable.Range(2, 26).Select((_Closure_0024__._0024I105_002D0 != null) ? _Closure_0024__._0024I105_002D0 : (_Closure_0024__._0024I105_002D0 = _Closure_0024__._0024I._Lambda_0024__105_002D0)).ToList(), checked(ClampPartyGuestCount(GuestCount) - 2));
			dancersItem = new UIMenuListItem("Dancers", Enumerable.Range(0, 8).Select((_Closure_0024__._0024I105_002D1 != null) ? _Closure_0024__._0024I105_002D1 : (_Closure_0024__._0024I105_002D1 = _Closure_0024__._0024I._Lambda_0024__105_002D1)).ToList(), PartyGuestProfiles.ClampDancerCount(DancerCount));
			string[] array = PartyGuestProfiles.GuestThemeChoices();
			guestThemeItem = new UIMenuListItem("Guest style", array.Select((_Closure_0024__._0024I105_002D2 != null) ? _Closure_0024__._0024I105_002D2 : (_Closure_0024__._0024I105_002D2 = _Closure_0024__._0024I._Lambda_0024__105_002D2)).ToList(), Array.IndexOf(array, PartyGuestProfiles.NormalizeGuestTheme(GuestTheme)));
			guestThemeItem.Description = "Mix, Hippie, Hipster, Rich, Sexy or Club guests. Applies after closing this menu.";
			string[] array2 = PartyGuestProfiles.DancerLookChoices();
			dancerLookItem = new UIMenuListItem("Dancer look", array2.Select((_Closure_0024__._0024I105_002D3 != null) ? _Closure_0024__._0024I105_002D3 : (_Closure_0024__._0024I105_002D3 = _Closure_0024__._0024I._Lambda_0024__105_002D3)).ToList(), Array.IndexOf(array2, PartyGuestProfiles.NormalizeDancerLook(DancerLook)));
			dancerLookItem.Description = "Mix or seven party dancer looks. Choose a private visitor separately in Private dancer / 3D preview.";
			UIMenu partyMenu = PartyMenu;
			partyMenu.AddItem(uIMenuItem);
			partyMenu.AddItem(item);
			partyMenu.AddItem(item4);
			partyMenu.AddItem(uIMenuItem2);
			partyMenu.AddItem(item2);
			partyMenu.AddItem(item3);
			partyMenu.AddItem(musicItem);
			partyMenu.AddItem(item5);
			partyMenu.AddItem(item6);
			partyMenu.AddItem(djSetItem);
			partyMenu.AddItem(stationItem);
			partyMenu.AddItem(guestsItem);
			partyMenu.AddItem(guestThemeItem);
			partyMenu.AddItem(dancersItem);
			partyMenu.AddItem(dancerLookItem);
			partyMenu.RefreshIndex();
		}

		private static void PartyContact_Answered(iFruitContact contact)
		{
			try
			{
				Mechanic.iFruit.Close(1500);
				OpenPartyMenuHere();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Party contact: {ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		public static void OpenPartyMenuHere(UIMenu parent = null)
		{
			RebuildMenu();
			PartyMenu.ParentMenu = parent;
			if (parent != null)
			{
				parent.Visible = false;
			}
			PartyMenu.Visible = true;
		}

		private static void PartyMenu_OnListChange(UIMenu sender, UIMenuListItem listItem, int newIndex)
		{
			try
			{
				if (listItem == stationItem)
				{
					PartyLocalMusic.Reset();
					RadioStation = RadioStations[newIndex];
					UseDjSet = false;
					if (djSetItem != null)
					{
						djSetItem.Checked = false;
					}
					musicBaseStationFallback = false;
					if (partyActive)
					{
						SetPartyRadio(true);
					}
				}
				else if (listItem == guestsItem)
				{
					GuestCount = ClampPartyGuestCount(checked(newIndex + 2));
					if (partyActive)
					{
						AdjustGuestCount();
					}
				}
				else if (listItem == dancersItem)
				{
					DancerCount = PartyGuestProfiles.ClampDancerCount(newIndex);
					if (partyActive)
					{
						pendingDancerAdjustment = true;
					}
				}
				else if (listItem == guestThemeItem)
				{
					string[] array = PartyGuestProfiles.GuestThemeChoices();
					if (newIndex < 0 || newIndex >= array.Length)
					{
						return;
					}
					GuestTheme = array[newIndex];
					if (partyActive)
					{
						pendingGuestRefresh = true;
					}
				}
				else if (listItem == dancerLookItem)
				{
					string[] array2 = PartyGuestProfiles.DancerLookChoices();
					if (newIndex < 0 || newIndex >= array2.Length)
					{
						return;
					}
					DancerLook = array2[newIndex];
					if (partyActive)
					{
						pendingDancerRefresh = true;
					}
				}
				SavePartyConfig();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Party list: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
		}

		private static void PartyMenu_OnCheckboxChange(UIMenu sender, UIMenuCheckboxItem checkboxItem, bool @checked)
		{
			try
			{
				if (checkboxItem == djSetItem)
				{
					PartyLocalMusic.Reset();
					UseDjSet = @checked;
					musicBaseStationFallback = false;
					if (partyActive)
					{
						SetPartyRadio(true);
					}
					SavePartyConfig();
				}
				else if (checkboxItem == musicItem)
				{
					MusicEnabled = @checked;
					if (!@checked)
					{
						PartyLocalMusic.Reset();
					}
					if (partyActive)
					{
						SetPartyRadio(@checked);
					}
					SavePartyConfig();
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Party checkbox: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
		}

		private static void PartyMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			try
			{
				switch (Conversions.ToString(selectedItem.Tag))
				{
				case "Start":
					if (partyActive)
					{
						Screen.ShowSubtitle("The party is already running.", 3000);
						break;
					}
					PartyMenu.Visible = false;
					StartPaidParty();
					break;
				case "Stop":
					PartyMenu.Visible = false;
					EndParty();
					break;
				case "Drink":
					PartyMenu.Visible = false;
					PlayerConsume("drink");
					break;
				case "Smoke":
					PartyMenu.Visible = false;
					PlayerConsume("smoke");
					break;
				case "Dance":
					PartyMenu.Visible = false;
					PartyPlayerActions.StartOrStopPlayerDance();
					break;
				case "Private":
					PartyMenu.Visible = false;
					PrivateDancerPreview.Open(PartyMenu);
					break;
				case "OwnMP3":
					if (partyActive)
					{
						PartyLocalMusic.OpenMenu(PartyMenu);
					}
					break;
				case "RestartMusic":
					PartyLocalMusic.Reset();
					MusicEnabled = true;
					if (musicItem != null)
					{
						musicItem.Checked = true;
					}
					musicBaseStationFallback = false;
					SetPartyRadio(true);
					SavePartyConfig();
					break;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Party select: {ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		private static bool TryPay(int amount)
		{
			eOwner player = Helper.GetPlayer();
			if ((uint)player <= 2u)
			{
				if (Game.Player.Money < amount)
				{
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "ERROR", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
					Screen.ShowSubtitle(Game.GetLocalizedString("MP_REP_PROP_4"), 4000);
					return false;
				}
				Game.Player.Money = checked(Game.Player.Money - amount);
			}
			return true;
		}

		private static Model? LoadModel(string name)
		{
			Model value = new Model(name);
			Model? result;
			if (!value.IsValid || !value.IsInCdImage)
			{
				Logger.Log($"Party: model '{name}' invalid");
				result = null;
			}
			else
			{
				value.Request(2000);
				if (!value.IsLoaded)
				{
					Logger.Log($"Party: model '{name}' did not load");
					result = null;
				}
				else
				{
					result = value;
				}
			}
			return result;
		}

		private static Model? FirstValidModel(string[] candidates)
		{
			for (int i = 0; i < candidates.Length; i = checked(i + 1))
			{
				Model? result = LoadModel(candidates[i]);
				if (result.HasValue)
				{
					return result;
				}
			}
			return null;
		}

		private static float ModelHeight(Model m)
		{
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			Function.Call(Hash.GET_MODEL_DIMENSIONS, m.Hash, outputArgument, outputArgument2);
			Vector3 result = outputArgument.GetResult<Vector3>();
			return outputArgument2.GetResult<Vector3>().Z - result.Z;
		}

		public static bool IsPlayableAnimationDuration(float duration)
		{
			if (!float.IsNaN(duration) && !float.IsInfinity(duration))
			{
				return duration >= 0.1f;
			}
			return false;
		}

		private static void AnimationFallback(PartyPed pd)
		{
			RemoveHandProp(pd);
			Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, pd.Ped.Handle);
			Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, pd.Ped.Handle, pd.Fallback, 0, true);
			pd.Settled = true;
			pd.AnimPlayingRequested = false;
			Logger.Log($"Party: {pd.Role} -> scenario {pd.Fallback} (no valid dictionary/clip pair played)");
		}

		private static bool TryNextAnim(PartyPed pd, bool advanceClip = true)
		{
			if ((object)pd.Ped == null || !pd.Ped.Exists())
			{
				return false;
			}
			pd.Settled = false;
			pd.AnimPlayingRequested = false;
			checked
			{
				if (advanceClip)
				{
					pd.AnimIndex++;
				}
				if (pd.Dict == null)
				{
					if (pd.DictIndex >= pd.Dicts.Length || pd.Anims.Length == 0)
					{
						AnimationFallback(pd);
						return false;
					}
					pd.Dict = pd.Dicts[pd.DictIndex];
					pd.AnimIndex = 0;
					pd.DictDeadline = Game.GameTime + 1500;
					RequestPartyDictionary(pd.Dict);
				}
				if (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, pd.Dict))
				{
					if (Game.GameTime >= pd.DictDeadline)
					{
						pd.DictIndex++;
						pd.Dict = null;
					}
					pd.CheckAt = Game.GameTime + 50;
					return true;
				}
				int num = 0;
				while (pd.AnimIndex < pd.Anims.Length && num < 4)
				{
					string text = pd.Anims[pd.AnimIndex];
					if (!string.IsNullOrWhiteSpace(text) && IsPlayableAnimationDuration(Function.Call<float>(Hash.GET_ANIM_DURATION, pd.Dict, text)))
					{
						Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, pd.Ped.Handle);
						Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, pd.Ped.Handle, pd.HomePos.X, pd.HomePos.Y, pd.HomePos.Z, false, false, false);
						Function.Call(Hash.SET_ENTITY_HEADING, pd.Ped.Handle, pd.HomeHeading);
						Function.Call(Hash.TASK_PLAY_ANIM, pd.Ped.Handle, pd.Dict, text, 4f, -4f, -1, 1, 0f, false, false, false);
						pd.AnimPlayingRequested = true;
						pd.CheckAt = Game.GameTime + 1200;
						return true;
					}
					num++;
					pd.AnimIndex++;
				}
				if (pd.AnimIndex >= pd.Anims.Length)
				{
					pd.DictIndex++;
					pd.Dict = null;
				}
				pd.CheckAt = Game.GameTime + 25;
				return true;
			}
		}

		private static void StartAnimChain(PartyPed pd)
		{
			pd.Settled = false;
			pd.Dict = null;
			pd.DictIndex = 0;
			pd.AnimIndex = -1;
			pd.AnimPlayingRequested = false;
			TryNextAnim(pd, false);
		}

		private static void CheckAnims()
		{
			foreach (PartyPed partyPed in partyPeds)
			{
				if (partyPed == companion || Operators.CompareString(partyPed.Role, "resting", false) == 0 || Operators.CompareString(partyPed.Role, "bedroom_group", false) == 0)
				{
					continue;
				}
				if ((object)partyPed.Ped == null || !partyPed.Ped.Exists())
				{
					partyPed.Settled = true;
				}
				else
				{
					if (Game.GameTime < partyPed.CheckAt || (partyPed.Settled && !partyPed.AnimPlayingRequested))
					{
						continue;
					}
					if (partyPed.AnimPlayingRequested && partyPed.Dict != null && partyPed.AnimIndex >= 0 && partyPed.AnimIndex < partyPed.Anims.Length && Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, partyPed.Ped.Handle, partyPed.Dict, partyPed.Anims[partyPed.AnimIndex], 3))
					{
						if (!partyPed.Settled)
						{
							Logger.Log($"Party: {partyPed.Role} plays {partyPed.Dict}/{partyPed.Anims[partyPed.AnimIndex]}");
						}
						partyPed.Settled = true;
						partyPed.CheckAt = checked(Game.GameTime + 5000);
					}
					else
					{
						bool advanceClip = partyPed.AnimPlayingRequested && !partyPed.Settled;
						TryNextAnim(partyPed, advanceClip);
					}
				}
			}
		}

		private static PartyPed SpawnPed(string modelName, string role, Vector3 pos, float heading)
		{
			Model? model = LoadModel(modelName);
			if (!model.HasValue)
			{
				return null;
			}
			return SpawnLoadedPed(model.Value, modelName, role, pos, heading);
		}

		private static PartyPed SpawnLoadedPed(Model model, string modelName, string role, Vector3 pos, float heading)
		{
			Ped ped = null;
			try
			{
				if (!model.IsLoaded || !model.IsPed)
				{
					return null;
				}
				ped = World.CreatePed(model, pos + new Vector3(0f, 0f, 1f), heading);
				if ((object)ped == null || !ped.Exists())
				{
					return null;
				}
				ped.IsPersistent = true;
				ped.BlockPermanentEvents = true;
				ped.CanRagdoll = false;
				ped.IsInvincible = true;
				ped.CanBeTargetted = false;
				ped.RelationshipGroup = Helper.PP.RelationshipGroup;
				Function.Call(Hash.SET_PED_RANDOM_COMPONENT_VARIATION, ped.Handle, 0);
				if (!PartyPlacement.TryPlacePedOnFloor(ped, pos, heading))
				{
					ped.Delete();
					return null;
				}
				PartyPed partyPed = new PartyPed
				{
					Ped = ped,
					Role = role,
					ModelName = modelName,
					FloorPos = pos,
					HomePos = ped.Position,
					HomeHeading = heading
				};
				partyPeds.Add(partyPed);
				return partyPed;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				try
				{
					if ((object)ped != null && ped.Exists())
					{
						ped.Delete();
					}
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					ProjectData.ClearProjectError();
				}
				Logger.Log($"Party spawn {role}/{modelName}: {ex2.Message}");
				PartyPed result = null;
				ProjectData.ClearProjectError();
				return result;
			}
			finally
			{
				model.MarkAsNoLongerNeeded();
			}
		}

		private static bool IsFemaleModel(string name)
		{
			if (!name.StartsWith("a_f_", StringComparison.OrdinalIgnoreCase) && !name.StartsWith("s_f_", StringComparison.OrdinalIgnoreCase))
			{
				return name.StartsWith("g_f_", StringComparison.OrdinalIgnoreCase);
			}
			return true;
		}

		private static string[] Shuffle(string[] arr)
		{
			return arr.OrderBy((_Closure_0024__._0024I123_002D0 != null) ? _Closure_0024__._0024I123_002D0 : (_Closure_0024__._0024I123_002D0 = _Closure_0024__._0024I._Lambda_0024__123_002D0)).ToArray();
		}

		public static string[] PrivateAppearanceModels()
		{
			return PrivateDancerPreview.AllAppearanceModels(DancerModels);
		}

		public static bool SetPrivateAppearance(string name)
		{
			if (!PrivateAppearanceModels().Contains(name, StringComparer.OrdinalIgnoreCase))
			{
				return false;
			}
			return SetPrivateAppearanceChoice(PrivateDancerPreview.CategoryForModel(name), name);
		}

		public static bool SetPrivateAppearanceChoice(string category, string name)
		{
			string text = PrivateDancerPreview.NormalizeCategory(category);
			if (text == null || !PrivateAppearanceModels().Contains(name, StringComparer.OrdinalIgnoreCase) || !PrivateDancerPreview.ModelsForCategory(text, PrivateAppearanceModels()).Contains(name, StringComparer.OrdinalIgnoreCase))
			{
				return false;
			}
			PrivateAppearance = name;
			PrivateAppearanceCategory = text;
			if (cfg != null)
			{
				SavePartyConfig();
			}
			return true;
		}

		private static string[] PrivateDancerCandidates()
		{
			if (!string.IsNullOrWhiteSpace(PrivateAppearance))
			{
				return new string[1] { PrivateAppearance };
			}
			return Shuffle(PartyGuestProfiles.DancerModelsForLook(DancerLook, DancerModels));
		}

		private static string DjTrackList(string model)
		{
			switch (model.ToLowerInvariant())
			{
			case "ig_sol":
				return "BATTLE_MIX1_RADIO";
			case "ig_talcc":
				return "BATTLE_MIX2_RADIO";
			case "ig_dix":
				return "BATTLE_MIX3_RADIO";
			case "ig_blackmadonna":
			case "ig_djblamadon":
				return "BATTLE_MIX4_RADIO";
			default:
				return "BATTLE_MIX1_RADIO";
			}
		}

		private static List<Vector3> OccupiedPartyPositions(PartyPed exclude = null)
		{
			List<Vector3> list = new List<Vector3> { Helper.PP.Position };
			foreach (PartyPed partyPed in partyPeds)
			{
				if (partyPed != exclude && (object)partyPed.Ped != null && partyPed.Ped.Exists())
				{
					list.Add(partyPed.Ped.Position);
					if (partyPed.WalkingUntil > 0)
					{
						list.Add(partyPed.WalkTarget);
					}
				}
			}
			list.AddRange(PartyStage.ReservedPositions());
			return list;
		}

		private static PartyPed SpawnWithFallback(IEnumerable<string> names, string role, Vector3 floorPoint, float heading)
		{
			foreach (string item in names.Where((_Closure_0024__._0024I130_002D0 != null) ? _Closure_0024__._0024I130_002D0 : (_Closure_0024__._0024I130_002D0 = _Closure_0024__._0024I._Lambda_0024__130_002D0)).Distinct(StringComparer.OrdinalIgnoreCase))
			{
				PartyPed partyPed = SpawnPed(item, role, floorPoint, heading);
				if (partyPed != null)
				{
					return partyPed;
				}
			}
			return null;
		}

		private static PartyPed CreateDj()
		{
			Vector3 spot = default(Vector3);
			if (!PartyPlacement.TryFindSpot(partyCenter, djFloorPos, OccupiedPartyPositions(), ref spot, 1f) && !PartyPlacement.TryFindPartySpot(partyCenter, 0, OccupiedPartyPositions(), ref spot))
			{
				Logger.Log("Party DJ: no clear same-floor position for recovery.");
				return null;
			}
			djFloorPos = spot;
			djHeading = (float)(Math.Atan2(partyCenter.Y - djFloorPos.Y, partyCenter.X - djFloorPos.X) * 180.0 / Math.PI) - 90f;
			PartyPed partyPed = SpawnWithFallback(Shuffle(DjModels).Concat(new string[6] { "ig_sol", "ig_talcc", "ig_dix", "ig_djblamadon", "a_m_y_business_01", "s_m_m_highsec_01" }), "dj", djFloorPos, djHeading);
			if (partyPed == null)
			{
				return null;
			}
			djModelName = partyPed.ModelName;
			partyPed.Dicts = DjDicts.Concat(GuestDicts).Distinct().ToArray();
			partyPed.Anims = DjAnims.Concat(GuestAnimsM).ToArray();
			partyPed.Fallback = "WORLD_HUMAN_PARTYING";
			if (!PartyStage.Start(partyCenter, djFloorPos, ref djHeading, partyPed.Ped))
			{
				partyPed.Ped.Delete();
				partyPeds.Remove(partyPed);
				Notification.Show("~y~Party Service~w~: the DJ table cannot fit safely here. Start the party in a clearer area.");
				return null;
			}
			partyPed.HomeHeading = djHeading;
			partyPed.Ped.Heading = djHeading;
			StartAnimChain(partyPed);
			Logger.Log($"Party DJ created: {djModelName}, handle={partyPed.Ped.Handle}, origin={partyPed.HomePos}, floor={partyPed.FloorPos}");
			return partyPed;
		}

		private static void RemoveHandProp(PartyPed pd)
		{
			if ((object)pd.HandProp == null)
			{
				return;
			}
			try
			{
				if (pd.HandProp.Exists())
				{
					pd.HandProp.Delete();
				}
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
			}
			partyProps.Remove(pd.HandProp);
			pd.HandProp = null;
		}

		private static bool AttachDrink(PartyPed pd, bool whiskey)
		{
			Model? model = LoadModel(whiskey ? "prop_drink_whisky" : "prop_amb_beer_bottle");
			if (!model.HasValue)
			{
				return false;
			}
			Prop prop = null;
			try
			{
				prop = World.CreateProp(model.Value, pd.Ped.Position, false, false);
				if ((object)prop == null || !prop.Exists())
				{
					return false;
				}
				prop.IsPersistent = true;
				Function.Call(Hash.SET_ENTITY_COLLISION, prop.Handle, false, false);
				int num = Function.Call<int>(Hash.GET_PED_BONE_INDEX, pd.Ped.Handle, 28422);
				Vector3 vector = (whiskey ? new Vector3(0.01f, -0.01f, -0.06f) : Vector3.Zero);
				Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop.Handle, pd.Ped.Handle, num, vector.X, vector.Y, vector.Z, 0f, 0f, 0f, false, false, false, true, 2, true);
				pd.HandProp = prop;
				partyProps.Add(prop);
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				if ((object)prop != null && prop.Exists())
				{
					prop.Delete();
				}
				Logger.Log("Party drink prop: " + ex2.Message);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
			finally
			{
				model.Value.MarkAsNoLongerNeeded();
			}
		}

		private static void StartGuestBehavior(PartyPed pd)
		{
			if ((object)pd.Ped == null || !pd.Ped.Exists())
			{
				return;
			}
			RemoveHandProp(pd);
			Function.Call(Hash.CLEAR_PED_TASKS, pd.Ped.Handle);
			pd.WalkingUntil = 0;
			pd.Settled = true;
			pd.AnimPlayingRequested = false;
			pd.Dict = null;
			checked
			{
				pd.NextBehaviorAt = Game.GameTime + rnd.Next(22000, 45001);
				switch (pd.Behavior)
				{
				case "walk":
				{
					Vector3 spot = default(Vector3);
					if (PartyPlacement.TryFindWalkDestination(partyCenter, pd.Ped.Position, OccupiedPartyPositions(pd), ref spot))
					{
						pd.WalkTarget = spot;
						pd.WalkStartAt = Game.GameTime;
						pd.WalkingUntil = Game.GameTime + 15000;
						Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, pd.Ped.Handle, spot.X, spot.Y, spot.Z, 0.8f, 14000, pd.HomeHeading, 0.4f);
					}
					else
					{
						Function.Call(Hash.TASK_STAND_STILL, pd.Ped.Handle, 5000);
						pd.NextBehaviorAt = Game.GameTime + 5000;
					}
					break;
				}
				case "beer":
				case "whiskey":
				{
					string text = "amb@world_human_drinking@coffee@male@idle_a";
					if (AttachDrink(pd, Operators.CompareString(pd.Behavior, "whiskey", false) == 0))
					{
						pd.Dicts = new string[1] { text };
						pd.Anims = new string[1] { "idle_c" };
						pd.Fallback = "WORLD_HUMAN_DRINKING";
						StartAnimChain(pd);
					}
					else
					{
						Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, pd.Ped.Handle, "WORLD_HUMAN_DRINKING", 0, true);
					}
					break;
				}
				case "joint":
					Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, pd.Ped.Handle, "WORLD_HUMAN_SMOKING_POT", 0, true);
					break;
				default:
					pd.Dicts = GuestDicts;
					pd.Anims = Shuffle(IsFemaleModel(pd.ModelName) ? GuestAnimsF : GuestAnimsM);
					pd.Fallback = "WORLD_HUMAN_PARTYING";
					StartAnimChain(pd);
					break;
				}
			}
		}

		private static bool AddGuest(int ordinal)
		{
			Vector3 spot = default(Vector3);
			if (!PartyPlacement.TryFindPartySpot(partyCenter, ordinal, OccupiedPartyPositions(), ref spot))
			{
				return false;
			}
			float heading = (float)(Math.Atan2(djFloorPos.Y - spot.Y, djFloorPos.X - spot.X) * 180.0 / Math.PI) - 90f;
			PartyPed partyPed = SpawnWithFallback(Shuffle(PartyGuestProfiles.GuestModelsForTheme(GuestTheme, GuestModels)).Take(3), "guest", spot, heading);
			if (partyPed == null)
			{
				return false;
			}
			partyPed.Behavior = GuestBehaviorForIndex(ordinal);
			StartGuestBehavior(partyPed);
			return true;
		}

		public static bool CanRefreshPartyActor(string requestedRole, string actorRole, bool isCompanion, bool isPrivateVisitor)
		{
			if ((Operators.CompareString(requestedRole, "guest", false) == 0 || Operators.CompareString(requestedRole, "dancer", false) == 0) && Operators.CompareString(actorRole, requestedRole, false) == 0 && !isCompanion)
			{
				return !isPrivateVisitor;
			}
			return false;
		}

		private static void RefreshPartyModels(string role)
		{
			PartyPed[] array = partyPeds.ToArray();
			foreach (PartyPed partyPed in array)
			{
				if (CanRefreshPartyActor(role, partyPed.Role, partyPed == companion, partyPed == privateDancer))
				{
					RemoveHandProp(partyPed);
					if ((object)partyPed.Ped != null && partyPed.Ped.Exists())
					{
						partyPed.Ped.Delete();
					}
					partyPeds.Remove(partyPed);
				}
			}
			if (Operators.CompareString(role, "guest", false) == 0)
			{
				AdjustGuestCount();
			}
			else
			{
				AdjustDancerCount();
			}
		}

		private static void ApplyPendingCrowdOptions()
		{
			if (partyActive && (PartyMenu == null || !PartyMenu.Visible))
			{
				bool flag = pendingGuestRefresh;
				bool num = pendingDancerRefresh;
				bool flag2 = pendingDancerAdjustment;
				pendingGuestRefresh = false;
				pendingDancerRefresh = false;
				pendingDancerAdjustment = false;
				if (flag)
				{
					RefreshPartyModels("guest");
				}
				if (num)
				{
					RefreshPartyModels("dancer");
				}
				else if (flag2)
				{
					AdjustDancerCount();
				}
			}
		}

		private static bool AddDancer(int ordinal)
		{
			double num = (double)djHeading * Math.PI / 180.0;
			Vector3 vector = new Vector3((float)Math.Cos(num), (float)Math.Sin(num), 0f);
			Vector3 vector2 = new Vector3(0f - vector.Y, vector.X, 0f);
			float[] array = new float[7] { -1.8f, 1.8f, -3.2f, 3.2f, -4.6f, 4.6f, 0f };
			int num2 = PartyGuestProfiles.ClampDancerCount(ordinal);
			if (num2 >= array.Length)
			{
				return false;
			}
			Vector3 desired = djFloorPos + vector * array[num2];
			checked
			{
				if (num2 == array.Length - 1)
				{
					desired += vector2 * 3.2f;
				}
				Vector3 spot = default(Vector3);
				if (!PartyPlacement.TryFindSpot(partyCenter, desired, OccupiedPartyPositions(), ref spot, 0.95f) && !PartyPlacement.TryFindPartySpot(partyCenter, ordinal + 27, OccupiedPartyPositions(), ref spot))
				{
					return false;
				}
				PartyPed partyPed = SpawnWithFallback(Shuffle(PartyGuestProfiles.DancerModelsForLook(DancerLook, DancerModels)).Take(3), "dancer", spot, djHeading);
				if (partyPed == null)
				{
					return false;
				}
				partyPed.Dicts = DancerDicts;
				partyPed.Anims = Shuffle(DancerAnims);
				partyPed.Fallback = "WORLD_HUMAN_PARTYING";
				StartAnimChain(partyPed);
				return true;
			}
		}

		private static void AdjustDancerCount()
		{
			DancerCount = PartyGuestProfiles.ClampDancerCount(DancerCount);
			List<PartyPed> list = partyPeds.Where((_Closure_0024__._0024I140_002D0 != null) ? _Closure_0024__._0024I140_002D0 : (_Closure_0024__._0024I140_002D0 = _Closure_0024__._0024I._Lambda_0024__140_002D0)).ToList();
			checked
			{
				while (list.Count > DancerCount)
				{
					PartyPed partyPed = list[list.Count - 1];
					RemoveHandProp(partyPed);
					partyPed.Ped.Delete();
					partyPeds.Remove(partyPed);
					list.RemoveAt(list.Count - 1);
				}
				int count = list.Count;
				int num = DancerCount - 1;
				for (int i = count; i <= num; i++)
				{
					AddDancer(i);
				}
				int num2 = partyPeds.Where((_Closure_0024__._0024I140_002D1 != null) ? _Closure_0024__._0024I140_002D1 : (_Closure_0024__._0024I140_002D1 = _Closure_0024__._0024I._Lambda_0024__140_002D1)).Count();
				Logger.Log($"Party dancers requested={DancerCount}, created={num2}, look={DancerLook}; safe placement retained.");
				if (num2 < DancerCount)
				{
					Notification.Show($"~y~Party Service~w~: safe space for {num2}/{DancerCount} dancers here.");
				}
			}
		}

		private static void AdjustGuestCount()
		{
			GuestCount = ClampPartyGuestCount(GuestCount);
			List<PartyPed> list = partyPeds.Where((_Closure_0024__._0024I141_002D0 != null) ? _Closure_0024__._0024I141_002D0 : (_Closure_0024__._0024I141_002D0 = _Closure_0024__._0024I._Lambda_0024__141_002D0)).ToList();
			checked
			{
				while (list.Count > GuestCount)
				{
					PartyPed partyPed = list[list.Count - 1];
					RemoveHandProp(partyPed);
					partyPed.Ped.Delete();
					partyPeds.Remove(partyPed);
					list.RemoveAt(list.Count - 1);
				}
				int num = 0;
				int count = list.Count;
				int num2 = GuestCount - 1;
				for (int i = count; i <= num2; i++)
				{
					if (!AddGuest(i))
					{
						num++;
					}
				}
				int num3 = partyPeds.Where((_Closure_0024__._0024I141_002D1 != null) ? _Closure_0024__._0024I141_002D1 : (_Closure_0024__._0024I141_002D1 = _Closure_0024__._0024I._Lambda_0024__141_002D1)).Count();
				Logger.Log($"Party guests requested={GuestCount}, created={num3}, style={GuestTheme}; unsafe/full spots skipped={num}");
				if (num3 < GuestCount)
				{
					Notification.Show($"~y~Party Service~w~: safe space for {num3}/{GuestCount} guests on this floor. Move to a larger area for more.");
				}
			}
		}

		private static void MaintainGuests()
		{
			PartyPed[] array = partyPeds.ToArray();
			checked
			{
				Vector3 floorPoint = default(Vector3);
				foreach (PartyPed partyPed in array)
				{
					if ((object)partyPed.Ped == null || !partyPed.Ped.Exists())
					{
						RemoveHandProp(partyPed);
						partyPeds.Remove(partyPed);
					}
					else
					{
						if (partyPed == companion || Operators.CompareString(partyPed.Role, "resting", false) == 0 || Operators.CompareString(partyPed.Role, "bedroom_group", false) == 0)
						{
							continue;
						}
						Vector3 position = partyPed.Ped.Position;
						if (position.Z < partyPed.FloorPos.Z - 0.15f || Math.Abs(position.Z - partyPed.HomePos.Z) > 2f || (Operators.CompareString(partyPed.Role, "guest", false) == 0 && Mansion.Dist2D(position, partyCenter) > 11f))
						{
							RemoveHandProp(partyPed);
							if (PartyPlacement.TryPlacePedOnFloor(partyPed.Ped, partyPed.FloorPos, partyPed.HomeHeading))
							{
								partyPed.HomePos = partyPed.Ped.Position;
								if (Operators.CompareString(partyPed.Role, "guest", false) == 0)
								{
									StartGuestBehavior(partyPed);
								}
								else
								{
									StartAnimChain(partyPed);
								}
							}
							else
							{
								partyPed.Ped.Delete();
								partyPeds.Remove(partyPed);
							}
						}
						else
						{
							if (Operators.CompareString(partyPed.Role, "guest", false) != 0)
							{
								continue;
							}
							if (Operators.CompareString(partyPed.Behavior, "walk", false) == 0 && partyPed.WalkingUntil > 0)
							{
								if ((Mansion.Dist2D(position, partyPed.WalkTarget) < 0.6f && Game.GameTime - partyPed.WalkStartAt > 1000) || Game.GameTime >= partyPed.WalkingUntil)
								{
									partyPed.WalkingUntil = 0;
									partyPed.HomePos = position;
									if (PartyPlacement.TryGetFloor(partyCenter, position, ref floorPoint))
									{
										partyPed.FloorPos = floorPoint;
									}
									Function.Call(Hash.CLEAR_PED_TASKS, partyPed.Ped.Handle);
									Function.Call(Hash.TASK_STAND_STILL, partyPed.Ped.Handle, 4000);
									partyPed.NextBehaviorAt = Game.GameTime + rnd.Next(2500, 6001);
								}
							}
							else if (Game.GameTime >= partyPed.NextBehaviorAt)
							{
								StartGuestBehavior(partyPed);
							}
						}
					}
				}
				if (!partyPeds.Any((_Closure_0024__._0024I142_002D0 != null) ? _Closure_0024__._0024I142_002D0 : (_Closure_0024__._0024I142_002D0 = _Closure_0024__._0024I._Lambda_0024__142_002D0)) && Game.GameTime >= nextDjRetryAt)
				{
					nextDjRetryAt = Game.GameTime + 15000;
					PartyStage.Stop();
					if (CreateDj() != null)
					{
						SetPartyRadio(true);
					}
				}
			}
		}

		private static void RefundPartyCost(eOwner owner, int amount, string serviceName = "party")
		{
			if (amount <= 0)
			{
				return;
			}
			string text = null;
			switch (owner)
			{
			case eOwner.Michael:
				text = "SP0_TOTAL_CASH";
				break;
			case eOwner.Franklin:
				text = "SP1_TOTAL_CASH";
				break;
			case eOwner.Trevor:
				text = "SP2_TOTAL_CASH";
				break;
			}
			if (text == null)
			{
				return;
			}
			bool flag = false;
			checked
			{
				if (owner == Helper.GetPlayer())
				{
					Game.Player.Money = (int)Math.Min(2147483647L, unchecked((long)Game.Player.Money) + unchecked((long)amount));
					flag = true;
				}
				else
				{
					OutputArgument outputArgument = new OutputArgument();
					int num = Game.GenerateHash(text);
					if (Function.Call<bool>(Hash.STAT_GET_INT, num, outputArgument, -1))
					{
						flag = Function.Call<bool>(Hash.STAT_SET_INT, num, (int)Math.Min(2147483647L, unchecked((long)outputArgument.GetResult<int>()) + unchecked((long)amount)), true);
					}
				}
				if (flag)
				{
					Notification.Show($"~y~Party Service~w~: {serviceName} could not start; ${amount:N0} refunded.");
					return;
				}
				Logger.Log($"Party refund could not be committed: owner={owner}, amount={amount}, service={serviceName}");
				Notification.Show("~r~Party Service~w~: refund failed; details are in SPA II.log.");
			}
		}

		private static void StartPaidParty()
		{
			if (partyActive || partyStarting)
			{
				return;
			}
			if (IsPlayerActionActive || companionState != 0)
			{
				Notification.Show("Finish the current action before starting a party.");
				return;
			}
			if (Helper.PP.IsInVehicle())
			{
				Notification.Show("Get out of the vehicle before starting a party.");
				return;
			}
			eOwner player = Helper.GetPlayer();
			int cost = Cost;
			if (TryPay(cost))
			{
				if (privateDancerActive)
				{
					EndPrivateDancerSession();
				}
				if (!StartParty())
				{
					RefundPartyCost(player, cost);
				}
			}
		}

		public static bool StartParty()
		{
			if (partyActive || partyStarting)
			{
				return false;
			}
			partyStarting = true;
			checked
			{
				try
				{
					partyCenter = Helper.PP.Position;
					Vector3 forwardVector = Helper.PP.ForwardVector;
					Vector3 rightVector = Helper.PP.RightVector;
					float heading = Helper.PP.Heading;
					Vector3 desired = partyCenter + forwardVector * 2.5f;
					if (!PartyPlacement.TryFindSpot(partyCenter, desired, OccupiedPartyPositions(), ref djFloorPos, 1.1f) && !PartyPlacement.TryFindPartySpot(partyCenter, 0, OccupiedPartyPositions(), ref djFloorPos))
					{
						Notification.Show("~y~Party Service~w~: no safe DJ position on this floor. Try a clearer area.");
						return false;
					}
					djHeading = heading + 180f;
					if (CreateDj() == null)
					{
						Notification.Show("~r~Party Service~w~: the DJ setup could not be placed safely; the party was not started.");
						EndParty();
						return false;
					}
					AdjustDancerCount();
					partyActive = true;
					partyEndAt = ((DurationMinutes > 0) ? (Game.GameTime + DurationMinutes * 60000) : 0);
					AdjustGuestCount();
					SetPartyRadio(true);
					lastMaintain = Game.GameTime;
					nextDjRetryAt = Game.GameTime + 15000;
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "PROPERTY_PURCHASE", "HUD_AWARDS", false);
					Notification.Show("~g~Party Service~w~: DJ ready! Call Party Service to dance, drink, change guests or end the party.");
					Logger.Log($"Party started at {partyCenter}: {partyPeds.Count} peds, actual DJ {djModelName}");
					return true;
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log($"StartParty: {ex2.Message} {ex2.StackTrace}");
					EndParty();
					bool result = false;
					ProjectData.ClearProjectError();
					return result;
				}
				finally
				{
					partyStarting = false;
				}
			}
		}

		public static bool IsPartyMusicInRange(Vector3 playerPosition, Vector3 center, float radius)
		{
			if (!PartyPlacement.FinitePoint(playerPosition) || !PartyPlacement.FinitePoint(center) || float.IsNaN(radius) || float.IsInfinity(radius))
			{
				return false;
			}
			float num = Math.Max(10f, Math.Min(120f, radius));
			return Mansion.Dist2D(playerPosition, center) <= num && Math.Abs(playerPosition.Z - center.Z) <= 12f;
		}

		public static bool ShouldRestorePreviousRadio(bool previousEnabled, bool previousWasApartment, bool apartmentSessionValid)
		{
			if (previousEnabled)
			{
				if (previousWasApartment)
				{
					return apartmentSessionValid;
				}
				return true;
			}
			return false;
		}

		private static void CapturePreviousRadio()
		{
			previousMobileRadio = Function.Call<bool>(Hash.IS_MOBILE_PHONE_RADIO_ACTIVE);
			previousMusicStation = Function.Call<string>(Hash.GET_PLAYER_RADIO_STATION_NAME);
			if (string.IsNullOrWhiteSpace(previousMusicStation))
			{
				previousMusicStation = "OFF";
			}
			previousWasApartmentRadio = ApartmentProps.RadioOn && ApartmentProps.RadioPos != Vector3.Zero;
			previousApartmentRadioPosition = (previousWasApartmentRadio ? ApartmentProps.RadioPos : Vector3.Zero);
		}

		private static void SnapshotPartyRadio()
		{
			if (!musicOwned)
			{
				CapturePreviousRadio();
				musicOwned = true;
			}
		}

		private static void ActivatePartyMusicRoute()
		{
			if (!musicRouteActive && !localRadioSuspended)
			{
				CapturePreviousRadio();
			}
			Function.Call(Hash.SET_AUDIO_FLAG, "LoadMPData", true);
			Function.Call(Hash.SET_AUDIO_FLAG, "MobileRadioInGame", true);
			Function.Call(Hash.SET_AUDIO_FLAG, "FrontendRadioDisabled", false);
			Function.Call(Hash.SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY, true);
			Function.Call(Hash.SET_MOBILE_PHONE_RADIO_STATE, true);
			Function.Call(Hash.SET_FRONTEND_RADIO_ACTIVE, true);
			musicRouteActive = true;
			localRadioSuspended = false;
		}

		private static void RestorePartyMusicRoute()
		{
			if (musicOwned && (musicRouteActive || localRadioSuspended))
			{
				try
				{
					bool apartmentSessionValid = ApartmentProps.RadioOn && ApartmentProps.RadioPos != Vector3.Zero && ApartmentProps.RadioPos.DistanceTo(previousApartmentRadioPosition) < 0.1f && (object)Helper.PP != null && Helper.PP.Exists() && !Helper.PP.IsInVehicle() && Helper.PP.Position.DistanceTo(ApartmentProps.RadioPos) <= 30f;
					bool flag = ShouldRestorePreviousRadio(previousMobileRadio, previousWasApartmentRadio, apartmentSessionValid);
					Function.Call(Hash.SET_FRONTEND_RADIO_ACTIVE, flag);
					Function.Call(Hash.SET_MOBILE_PHONE_RADIO_STATE, flag);
					Function.Call(Hash.SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY, flag);
					Function.Call(Hash.SET_AUDIO_FLAG, "MobileRadioInGame", flag);
					Function.Call(Hash.SET_RADIO_TO_STATION_NAME, flag ? previousMusicStation : "OFF");
					Function.Call(Hash.SET_RADIO_AUTO_UNFREEZE, true);
					Function.Call(Hash.SET_USER_RADIO_CONTROL_ENABLED, true);
				}
				finally
				{
					musicRouteActive = false;
					localRadioSuspended = false;
				}
			}
		}

		internal static bool OwnsRadioRoute()
		{
			if (musicOwned)
			{
				if (!musicRouteActive)
				{
					return localRadioSuspended;
				}
				return true;
			}
			return false;
		}

		private static void SuspendPartyRadioForLocalTrack()
		{
			if (!localRadioSuspended)
			{
				SnapshotPartyRadio();
				if (!musicRouteActive)
				{
					CapturePreviousRadio();
				}
				Function.Call(Hash.SET_FRONTEND_RADIO_ACTIVE, false);
				Function.Call(Hash.SET_MOBILE_PHONE_RADIO_STATE, false);
				Function.Call(Hash.SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY, false);
				Function.Call(Hash.SET_AUDIO_FLAG, "MobileRadioInGame", false);
				localRadioSuspended = true;
				musicRouteActive = false;
			}
		}

		internal static void ForgetForStoryLoad()
		{
			ForgetMusicForStoryLoad();
			PartyStage.ForgetForStoryLoad();
			PartyPlayerActions.ForgetForStoryLoad();
			PrivateDancerPreview.ForgetForStoryLoad();
			if (PartyMenu != null)
			{
				PartyMenu.Visible = false;
			}
			partyActive = false;
			partyStarting = false;
			partyEndAt = 0;
			partyPeds.Clear();
			partyProps.Clear();
			ownedAnimationDictionaries.Clear();
			partyCenter = Vector3.Zero;
			djFloorPos = Vector3.Zero;
			djModelName = "";
			djHeading = 0f;
			nextDjRetryAt = 0;
			lastMaintain = 0;
			pendingGuestRefresh = false;
			pendingDancerRefresh = false;
			pendingDancerAdjustment = false;
			companion = null;
			companionState = 0;
			privateDancer = null;
			privateDancerActive = false;
			privateDancerExpiresAt = 0;
			privateDancerCenter = Vector3.Zero;
			privateRequestPending = false;
			privateRequestPlayer = null;
			privateRequestExisting = null;
			privateRequestCandidates = new string[0];
			privateRequestIndex = 0;
			privateRequestModel = null;
			privateRequestModelDeadline = 0;
			privateRequestOwner = eOwner.Nobody;
			privateRequestOrigin = Vector3.Zero;
			consumePed = null;
			consumeKind = null;
			consumeScenario = null;
			consumeOrigin = Vector3.Zero;
			consumeStartedAt = 0;
			consumeDuration = 0;
			consumeRequiresParty = false;
			buzzPed = null;
			pendingDrunkClip = null;
			pendingDrunkClipDeadline = 0;
			drunkUntil = 0;
			highUntil = 0;
			drunkLevel = 0;
			ResetBedroomTracking();
			bedroomPhaseAt = 0;
			bedroomStartedAt = 0;
			bedroomStartPosition = Vector3.Zero;
			bedroomWasFrozen = false;
			bedroomOwner = eOwner.Nobody;
			bedroomActorReturnPosition = Vector3.Zero;
			bedroomActorReturnHeading = 0f;
			restingBedScenes.Clear();
			bedroomGroup = null;
			morningPair = null;
			nextMorningClockAt = 0;
		}

		internal static void ForgetMusicForStoryLoad()
		{
			PartyLocalMusic.Reset();
			MansionPartyAudio.ForgetForStoryLoad();
			ForgetRadioOwnership();
		}

		private static void YieldPartyRadioToClub()
		{
			if (!musicOwned)
			{
				return;
			}
			try
			{
				if (musicRouteActive && string.Equals(Function.Call<string>(Hash.GET_PLAYER_RADIO_STATION_NAME), playingMusicStation, StringComparison.Ordinal))
				{
					Function.Call(Hash.SET_FRONTEND_RADIO_ACTIVE, false);
					Function.Call(Hash.SET_MOBILE_PHONE_RADIO_STATE, false);
					Function.Call(Hash.SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY, false);
					Function.Call(Hash.SET_AUDIO_FLAG, "MobileRadioInGame", false);
				}
				if (string.Equals(forcedTrackStation, "RADIO_22_DLC_BATTLE_MIX1_RADIO", StringComparison.Ordinal))
				{
					Function.Call(Hash.CLEAR_CUSTOM_RADIO_TRACK_LIST, forcedTrackStation);
				}
			}
			finally
			{
				ForgetRadioOwnership();
			}
		}

		private static void ForgetRadioOwnership()
		{
			musicOwned = false;
			musicRouteActive = false;
			localRadioSuspended = false;
			previousMusicStation = "OFF";
			previousMobileRadio = false;
			previousWasApartmentRadio = false;
			previousApartmentRadioPosition = Vector3.Zero;
			forcedTrackStation = "";
			playingMusicStation = "";
			musicVerifyAt = 0;
			musicRecoveryCount = 0;
			musicBaseStationFallback = false;
		}

		internal static void EnableLocalPartyMusic()
		{
			MusicEnabled = true;
			if (musicItem != null)
			{
				musicItem.Checked = true;
			}
			SavePartyConfig();
		}

		internal static void ResumePartyDjMusic()
		{
			PartyLocalMusic.Reset();
			if (partyActive && MusicEnabled && !OnlineExpansion.IsNightclubSession())
			{
				SetPartyRadio(true);
			}
		}

		private static void SetPartyRadio(bool enable)
		{
			try
			{
				if (OnlineExpansion.IsNightclubSession())
				{
					YieldPartyRadioToClub();
				}
				else if (enable && MusicEnabled)
				{
					Function.Call(Hash.SET_AUDIO_FLAG, "LoadMPData", true);
					SnapshotPartyRadio();
					string text = (musicBaseStationFallback ? "RADIO_07_DANCE_01" : (UseDjSet ? "RADIO_22_DLC_BATTLE_MIX1_RADIO" : RadioStation));
					if (!string.IsNullOrEmpty(forcedTrackStation) && Operators.CompareString(forcedTrackStation, text, false) != 0)
					{
						Function.Call(Hash.CLEAR_CUSTOM_RADIO_TRACK_LIST, forcedTrackStation);
						forcedTrackStation = "";
					}
					Function.Call(Hash.LOCK_RADIO_STATION, text, false);
					Function.Call(Hash.UNFREEZE_RADIO_STATION, text);
					Function.Call(Hash.SET_RADIO_AUTO_UNFREEZE, true);
					Function.Call(Hash.SET_RADIO_FRONTEND_FADE_TIME, 0.5f);
					if (UseDjSet && !musicBaseStationFallback)
					{
						string text2 = DjTrackList(djModelName);
						Function.Call(Hash.UNLOCK_RADIO_STATION_TRACK_LIST, text, text2);
						Function.Call(Hash.SET_CUSTOM_RADIO_TRACK_LIST, text, text2, true);
						Function.Call(Hash.FORCE_MUSIC_TRACK_LIST, text, text2, 0);
						forcedTrackStation = text;
					}
					else if (Operators.CompareString(forcedTrackStation, text, false) == 0)
					{
						Function.Call(Hash.CLEAR_CUSTOM_RADIO_TRACK_LIST, forcedTrackStation);
						forcedTrackStation = "";
					}
					playingMusicStation = text;
					if (!OnlineExpansion.IsNightclubSession() && IsPartyMusicInRange(Helper.PP.Position, partyCenter, MusicRadius))
					{
						ActivatePartyMusicRoute();
						Function.Call(Hash.SET_RADIO_TO_STATION_NAME, text);
					}
					musicVerifyAt = checked(Game.GameTime + 3000);
					musicRecoveryCount = 0;
					Logger.Log($"Party music: gameplay frontend route enabled; station={text}, DJSet={UseDjSet}, fallback={musicBaseStationFallback}, range={MusicRadius}m");
				}
				else
				{
					RestorePartyMusicRoute();
					if (!string.IsNullOrEmpty(forcedTrackStation))
					{
						Function.Call(Hash.CLEAR_CUSTOM_RADIO_TRACK_LIST, forcedTrackStation);
						forcedTrackStation = "";
					}
					musicOwned = false;
					playingMusicStation = "";
					musicVerifyAt = 0;
					musicRecoveryCount = 0;
					musicBaseStationFallback = false;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Party music: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
			finally
			{
				MansionPartyAudio.Update(OwnsRadioRoute(), partyCenter);
			}
		}

		private static void PartyMusicTick()
		{
			checked
			{
				try
				{
					bool flag = OnlineExpansion.IsNightclubSession();
					PartyLocalMusic.Tick(partyActive, MusicEnabled, partyCenter, MusicRadius, Helper.PP, flag);
					if (!MusicEnabled)
					{
						return;
					}
					if (flag)
					{
						YieldPartyRadioToClub();
						return;
					}
					bool flag2 = IsPartyMusicInRange(Helper.PP.Position, partyCenter, MusicRadius);
					if (PartyLocalMusic.IsReady)
					{
						if (flag2)
						{
							SuspendPartyRadioForLocalTrack();
						}
						else
						{
							RestorePartyMusicRoute();
						}
						return;
					}
					if (PartyLocalMusic.HasSelection && localRadioSuspended)
					{
						if (!flag2)
						{
							RestorePartyMusicRoute();
						}
						return;
					}
					if (!musicOwned)
					{
						SetPartyRadio(true);
					}
					if (!musicOwned)
					{
						return;
					}
					if (!flag2)
					{
						if (OwnsRadioRoute())
						{
							RestorePartyMusicRoute();
						}
						return;
					}
					if (!musicRouteActive)
					{
						ActivatePartyMusicRoute();
						Function.Call(Hash.SET_RADIO_TO_STATION_NAME, playingMusicStation);
						musicVerifyAt = Game.GameTime + 3000;
					}
					else
					{
						Function.Call(Hash.SET_FRONTEND_RADIO_ACTIVE, true);
					}
					if (musicVerifyAt != 0 && Game.GameTime >= musicVerifyAt)
					{
						bool flag3 = Function.Call<bool>(Hash.IS_RADIO_FADED_OUT);
						bool flag4 = Function.Call<bool>(Hash.IS_MOBILE_PHONE_RADIO_ACTIVE);
						string text = Function.Call<string>(Hash.GET_PLAYER_RADIO_STATION_NAME);
						int num = Function.Call<int>(Hash.GET_AUDIBLE_MUSIC_TRACK_TEXT_ID);
						Logger.Log($"Party music state: station={text}, mobile={flag4}, fadedOut={flag3}, trackTextId={num}; this is engine state, not an audibility test.");
						if ((flag3 || !flag4) && musicRecoveryCount < 2)
						{
							musicRecoveryCount++;
							ActivatePartyMusicRoute();
							Function.Call(Hash.UNFREEZE_RADIO_STATION, playingMusicStation);
							Function.Call(Hash.SET_RADIO_TO_STATION_NAME, playingMusicStation);
							musicVerifyAt = Game.GameTime + 3000;
						}
						else if (flag3 && !musicBaseStationFallback && UseDjSet)
						{
							musicBaseStationFallback = true;
							Logger.Log("Party music: DJ radio still faded out after retries; using Soulwax FM.");
							SetPartyRadio(true);
						}
						else
						{
							musicVerifyAt = Game.GameTime + 15000;
						}
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					musicVerifyAt = Game.GameTime + 15000;
					Logger.Log($"Party music tick: {ex2.Message}");
					ProjectData.ClearProjectError();
				}
				finally
				{
					MansionPartyAudio.Update(OwnsRadioRoute(), partyCenter);
				}
			}
		}

		public static void EndParty()
		{
			PartyLocalMusic.Reset();
			PrivateDancerPreview.Close();
			pendingGuestRefresh = false;
			pendingDancerRefresh = false;
			pendingDancerAdjustment = false;
			try
			{
				CancelPrivateDancerRequest();
				CancelConsume();
				CancelBedroomScene();
				CleanupBedroomRestingScenes();
				PartyPlayerActions.CancelPairedKiss();
				PartyPlayerActions.StopPlayerDance();
				if (companionState == 2)
				{
					Screen.FadeIn(500);
					Helper.HideHud = false;
				}
				companion = null;
				companionState = 0;
				bedroomBusy = false;
				SetPartyRadio(false);
				PartyStage.Stop();
				foreach (PartyPed partyPed in partyPeds)
				{
					try
					{
						RemoveHandProp(partyPed);
						if ((object)partyPed.Ped != null && partyPed.Ped.Exists())
						{
							partyPed.Ped.Delete();
						}
					}
					catch (Exception projectError)
					{
						ProjectData.SetProjectError(projectError);
						ProjectData.ClearProjectError();
					}
				}
				partyPeds.Clear();
				foreach (Prop partyProp in partyProps)
				{
					try
					{
						if ((object)partyProp != null && partyProp.Exists())
						{
							partyProp.Delete();
						}
					}
					catch (Exception projectError2)
					{
						ProjectData.SetProjectError(projectError2);
						ProjectData.ClearProjectError();
					}
				}
				partyProps.Clear();
				ReleasePartyDictionaries();
				ClearBuzz();
				if (partyActive)
				{
					Notification.Show("~y~Party Service~w~: Party's over.");
				}
				partyActive = false;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"EndParty: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
			finally
			{
				partyActive = false;
				privateDancerActive = false;
				privateDancer = null;
				companion = null;
				companionState = 0;
				bedroomBusy = false;
				morningPair = null;
				MansionPartyAudio.Update(false, partyCenter);
			}
		}

		public static void PartyTick()
		{
			PartyStage.Tick();
			if (!partyActive)
			{
				MansionPartyAudio.Update(false, partyCenter);
			}
			PrivateDancerPreview.Tick();
			if ((object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsDead)
			{
				if (partyActive || privateDancerActive || IsPlayerActionActive || PartyPlayerActions.IsKissing || PartyPlayerActions.IsDancing || (object)buzzPed != null)
				{
					EndParty();
				}
				return;
			}
			MorningCompanionTick();
			BuzzTick();
			ConsumeTick();
			PrivateDancerRequestTick();
			PartyPlayerActions.Tick(partyActive, partyCenter);
			if (!partyActive)
			{
				if (privateDancerActive)
				{
					if (privateDancer == null || (object)privateDancer.Ped == null || !privateDancer.Ped.Exists() || Helper.PP.Position.DistanceTo(privateDancerCenter) > 80f || Game.GameTime > privateDancerExpiresAt)
					{
						EndPrivateDancerSession();
					}
					else if (companionState != 0)
					{
						CompanionTick();
					}
				}
				return;
			}
			if (partyEndAt > 0 && Game.GameTime > partyEndAt)
			{
				EndParty();
				return;
			}
			if (Helper.PP.Position.DistanceTo(partyCenter) > 120f)
			{
				EndParty();
				return;
			}
			ApplyPendingCrowdOptions();
			PartyStage.Tick();
			PartyMusicTick();
			CheckAnims();
			if (!PartyPlayerActions.IsDancing)
			{
				CompanionTick();
			}
			if (Game.GameTime > checked(lastMaintain + 1200))
			{
				lastMaintain = Game.GameTime;
				MaintainGuests();
			}
		}

		public static void PartyAborted()
		{
			EndParty();
		}

		public static void DrinkChampagne()
		{
			MansionKitchen.Drink("Champagne");
		}

		internal static void ApplyKitchenDrinkEffect(Ped actor)
		{
			ApplyConsumeEffect(actor, "drink");
		}

		internal static void ApplyClubJointEffect(Ped actor)
		{
			ApplyConsumeEffect(actor, "smoke");
		}

		public static void HostPartyHere()
		{
			if (partyActive)
			{
				Screen.ShowSubtitle("The party is already running.", 3000);
			}
			else
			{
				StartPaidParty();
			}
		}

		private static void PlayerConsume(string kind, bool requireParty = true)
		{
			if (Operators.CompareString(kind, "drink", false) == 0)
			{
				MansionKitchen.Drink("Beer", requireParty);
			}
			else if (!MansionKitchen.IsBusy && !MansionCigarLounge.IsBusy && !MansionPetCare.IsBusy && (object)consumePed == null && !bedroomBusy && !privateRequestPending && !MansionFeatures.IsWorkoutActive && !PartyPlayerActions.IsKissing && (object)Helper.PP != null && Helper.PP.Exists() && !Helper.PP.IsDead && !Helper.PP.IsInVehicle() && !Helper.PP.IsFalling && !Helper.PP.IsRagdoll && (!requireParty || partyActive) && (Operators.CompareString(kind, "drink", false) == 0 || Operators.CompareString(kind, "smoke", false) == 0))
			{
				try
				{
					PartyPlayerActions.StopPlayerDance();
					consumePed = Helper.PP;
					consumeKind = kind;
					consumeScenario = "WORLD_HUMAN_SMOKING_POT";
					consumeOrigin = Helper.PP.Position;
					consumeStartedAt = Game.GameTime;
					consumeDuration = ((Operators.CompareString(kind, "drink", false) == 0) ? 8000 : 10000);
					consumeRequiresParty = requireParty;
					Function.Call(Hash.CLEAR_PED_TASKS, consumePed.Handle);
					Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, consumePed.Handle, consumeScenario, 0, true);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Party consume start: " + ex2.Message);
					CancelConsume();
					ProjectData.ClearProjectError();
				}
			}
		}

		private static void CancelConsume()
		{
			Ped ped = consumePed;
			string text = consumeScenario;
			consumePed = null;
			consumeKind = null;
			consumeScenario = null;
			try
			{
				if ((object)ped != null && ped.Exists() && !string.IsNullOrEmpty(text) && Function.Call<bool>(Hash.IS_PED_USING_SCENARIO, ped.Handle, text))
				{
					Function.Call(Hash.CLEAR_PED_TASKS, ped.Handle);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Party consume cleanup: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		private static void ConsumeTick()
		{
			Ped ped = consumePed;
			if ((object)ped == null)
			{
				return;
			}
			checked
			{
				try
				{
					bool flag = (object)Helper.PP == null || !Helper.PP.Exists() || !ped.Exists() || Helper.PP.Handle != ped.Handle || ped.IsDead || ped.IsInVehicle() || ped.IsRagdoll || ped.IsFalling || ped.Position.DistanceTo(consumeOrigin) > 2.5f || (consumeRequiresParty && !partyActive);
					if (!flag && Game.GameTime - consumeStartedAt > 1500)
					{
						flag = !Function.Call<bool>(Hash.IS_PED_USING_SCENARIO, ped.Handle, consumeScenario);
					}
					if (!flag && !Helper.MenuPool.IsAnyMenuOpen())
					{
						flag = Game.IsControlJustPressed(Control.FrontendCancel) || Game.IsControlJustPressed(Control.PhoneCancel);
					}
					string left = PartyPlayerActions.TimedActionOutcome(unchecked((long)Game.GameTime) - unchecked((long)consumeStartedAt), consumeDuration, flag);
					if (Operators.CompareString(left, "Cancelled", false) == 0)
					{
						CancelConsume();
						return;
					}
					if (Operators.CompareString(left, "Completed", false) == 0)
					{
						string kind = consumeKind;
						CancelConsume();
						ApplyConsumeEffect(ped, kind);
						return;
					}
					Control[] array = new Control[7]
					{
						Control.MoveLeftRight,
						Control.MoveUpDown,
						Control.Jump,
						Control.Sprint,
						Control.Attack,
						Control.Aim,
						Control.Enter
					};
					for (int i = 0; i < array.Length; i++)
					{
						Game.DisableControlThisFrame(array[i]);
					}
					if (!Helper.MenuPool.IsAnyMenuOpen())
					{
						Helper.ShowPersistentHelp("Press ~INPUT_FRONTEND_CANCEL~ to stop.");
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Party consume tick: " + ex2.Message);
					CancelConsume();
					ProjectData.ClearProjectError();
				}
			}
		}

		private static void ApplyConsumeEffect(Ped actor, string kind)
		{
			if ((object)actor == null || !actor.Exists() || (object)Helper.PP == null || Helper.PP.Handle != actor.Handle || actor.IsDead)
			{
				return;
			}
			if ((object)buzzPed != null && buzzPed.Handle != actor.Handle)
			{
				ClearBuzz();
			}
			buzzPed = actor;
			checked
			{
				if (Operators.CompareString(kind, "drink", false) == 0)
				{
					drunkLevel = Math.Min(3, drunkLevel + 1);
					drunkUntil = Game.GameTime + Math.Max(0, Math.Min(3600, DrinkEffectSeconds)) * 1000;
					pendingDrunkClip = ((drunkLevel >= 3) ? "move_m@drunk@verydrunk" : ((drunkLevel == 2) ? "move_m@drunk@moderatedrunk" : "move_m@drunk@slightlydrunk"));
					pendingDrunkClipDeadline = Game.GameTime + 1000;
					Function.Call(Hash.REQUEST_CLIP_SET, pendingDrunkClip);
					Function.Call(Hash.SET_PED_IS_DRUNK, actor.Handle, true);
					Function.Call(Hash.SHAKE_GAMEPLAY_CAM, "DRUNK_SHAKE", 0.4f * (float)drunkLevel);
					if (highUntil <= Game.GameTime)
					{
						Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "spectator5");
					}
					Notification.Show((drunkLevel >= 3) ? "~y~Party Service~w~: Whoa... easy there." : "~y~Party Service~w~: Cheers!");
				}
				else
				{
					highUntil = Game.GameTime + Math.Max(0, Math.Min(3600, SmokeEffectSeconds)) * 1000;
					Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "drug_wobbly");
					Function.Call(Hash.SET_TIMECYCLE_MODIFIER_STRENGTH, 0.8f);
					Function.Call(Hash.SHAKE_GAMEPLAY_CAM, "HAND_SHAKE", 0.5f);
					Notification.Show("~g~Party Service~w~: Puff puff pass.");
				}
			}
		}

		private static void BuzzTick()
		{
			if ((object)buzzPed == null)
			{
				return;
			}
			if ((object)Helper.PP == null || !Helper.PP.Exists() || !buzzPed.Exists() || Helper.PP.Handle != buzzPed.Handle || buzzPed.IsDead)
			{
				ClearBuzz();
				return;
			}
			if (pendingDrunkClip != null)
			{
				if (Function.Call<bool>(Hash.HAS_CLIP_SET_LOADED, pendingDrunkClip))
				{
					Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, buzzPed.Handle, pendingDrunkClip, 1f);
					pendingDrunkClip = null;
				}
				else if (Game.GameTime >= pendingDrunkClipDeadline)
				{
					pendingDrunkClip = null;
				}
			}
			if (drunkUntil > 0 && Game.GameTime >= drunkUntil)
			{
				drunkUntil = 0;
				drunkLevel = 0;
				pendingDrunkClip = null;
				Function.Call(Hash.RESET_PED_MOVEMENT_CLIPSET, buzzPed.Handle, 0.5f);
				Function.Call(Hash.SET_PED_IS_DRUNK, buzzPed.Handle, false);
				if (highUntil == 0)
				{
					Function.Call(Hash.STOP_GAMEPLAY_CAM_SHAKING, true);
					Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
				}
			}
			if (highUntil > 0 && Game.GameTime >= highUntil)
			{
				highUntil = 0;
				if (drunkUntil == 0)
				{
					Function.Call(Hash.STOP_GAMEPLAY_CAM_SHAKING, true);
					Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
				}
				else
				{
					Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "spectator5");
					Function.Call(Hash.SHAKE_GAMEPLAY_CAM, "DRUNK_SHAKE", 0.4f * (float)drunkLevel);
				}
			}
			if (drunkUntil == 0 && highUntil == 0)
			{
				buzzPed = null;
			}
		}

		private static void ClearBuzz()
		{
			Ped ped = buzzPed;
			bool num = drunkUntil > 0 || highUntil > 0 || pendingDrunkClip != null;
			buzzPed = null;
			pendingDrunkClip = null;
			drunkUntil = 0;
			highUntil = 0;
			drunkLevel = 0;
			if (num)
			{
				if ((object)ped != null && ped.Exists())
				{
					Function.Call(Hash.RESET_PED_MOVEMENT_CLIPSET, ped.Handle, 0.5f);
					Function.Call(Hash.SET_PED_IS_DRUNK, ped.Handle, false);
				}
				Function.Call(Hash.STOP_GAMEPLAY_CAM_SHAKING, true);
				Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
			}
		}

		private static void EndPrivateDancerSession()
		{
			if (companion == privateDancer)
			{
				CancelBedroomScene();
			}
			if (privateDancer != null)
			{
				RestingBedScene[] array = restingBedScenes.Where((_Closure_0024__._0024I192_002D0 != null) ? _Closure_0024__._0024I192_002D0 : (_Closure_0024__._0024I192_002D0 = _Closure_0024__._0024I._Lambda_0024__192_002D0)).ToArray();
				for (int i = 0; i < array.Length; i = checked(i + 1))
				{
					ReleaseRestingBedScene(array[i]);
				}
				RemoveHandProp(privateDancer);
				try
				{
					if ((object)privateDancer.Ped != null && privateDancer.Ped.Exists())
					{
						privateDancer.Ped.Delete();
					}
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					ProjectData.ClearProjectError();
				}
				partyPeds.Remove(privateDancer);
			}
			if (companion == privateDancer)
			{
				companion = null;
				companionState = 0;
				bedroomBusy = false;
			}
			privateDancer = null;
			privateDancerActive = false;
			if (!partyActive)
			{
				ReleasePartyDictionaries();
			}
		}

		public static bool PrivateRequestCanContinue(bool playerMatches, bool ownerMatches, bool alive, bool onFoot, bool stable, float displacement)
		{
			if (playerMatches && ownerMatches && alive && onFoot && stable && !float.IsNaN(displacement) && !float.IsInfinity(displacement) && displacement >= 0f)
			{
				return displacement <= 6f;
			}
			return false;
		}

		private static void CancelPrivateDancerRequest(string reason = null)
		{
			privateRequestPending = false;
			privateRequestPlayer = null;
			privateRequestExisting = null;
			privateRequestCandidates = new string[0];
			privateRequestIndex = 0;
			if (privateRequestModel.HasValue)
			{
				try
				{
					privateRequestModel.Value.MarkAsNoLongerNeeded();
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					ProjectData.ClearProjectError();
				}
			}
			privateRequestModel = null;
			if (!string.IsNullOrWhiteSpace(reason))
			{
				Logger.Log("Private dancer request cancelled: " + reason);
				Notification.Show("~y~Party Service~w~: " + reason + " No payment was taken.");
			}
		}

		public static void RequestPrivateDancerHere()
		{
			if (IsPlayerActionActive || MansionFeatures.IsWorkoutActive || companionState != 0 || PartyPlayerActions.IsKissing)
			{
				Notification.Show("Your private dancer visit is already in progress.");
			}
			else
			{
				if ((object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsDead || Helper.PP.IsFalling || Helper.PP.IsRagdoll)
				{
					return;
				}
				if (Helper.PP.IsInVehicle())
				{
					Notification.Show("Get out of the vehicle to request a private dancer.");
					return;
				}
				eOwner player = Helper.GetPlayer();
				int balance = default(int);
				if ((uint)player > 2u || (Helper.TryGetOwnerMoney(player, ref balance) && balance >= BedroomCost))
				{
					try
					{
						_Closure_0024__195_002D0 closure_0024__195_002D = new _Closure_0024__195_002D0();
						if (privateDancerActive)
						{
							EndPrivateDancerSession();
						}
						privateRequestPlayer = Helper.PP;
						privateRequestOwner = player;
						privateRequestOrigin = Helper.PP.Position;
						privateRequestCandidates = PrivateDancerCandidates();
						closure_0024__195_002D._0024VB_0024Local_requestedModels = new HashSet<string>(privateRequestCandidates, StringComparer.OrdinalIgnoreCase);
						privateRequestExisting = partyPeds.Where(closure_0024__195_002D._Lambda_0024__0).OrderBy((_Closure_0024__._0024I195_002D1 != null) ? _Closure_0024__._0024I195_002D1 : (_Closure_0024__._0024I195_002D1 = _Closure_0024__._0024I._Lambda_0024__195_002D1)).FirstOrDefault();
						if (!string.IsNullOrWhiteSpace(PrivateAppearance))
						{
							privateRequestExisting = null;
						}
						privateRequestIndex = 0;
						privateRequestPending = true;
						PartyPlayerActions.StopPlayerDance();
						Logger.Log($"Private dancer requested: owner={player}, position={privateRequestOrigin}; models stream on normal ticks.");
						Notification.Show("~p~Party Service~w~: arranging your private dancer. Payment is taken when she arrives.");
						return;
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						Logger.Log("Private dancer request: " + ex2.Message);
						CancelPrivateDancerRequest("The visit could not be arranged.");
						ProjectData.ClearProjectError();
						return;
					}
				}
				Notification.Show("You do not have enough story-mode cash for a private dancer.");
			}
		}

		private static bool PrivateDancerRequestContextValid()
		{
			if (!privateRequestPending || (object)Helper.PP == null || !Helper.PP.Exists() || (object)privateRequestPlayer == null || !privateRequestPlayer.Exists())
			{
				return false;
			}
			return PrivateRequestCanContinue(Helper.PP.Handle == privateRequestPlayer.Handle, Helper.GetPlayer() == privateRequestOwner, !Helper.PP.IsDead, !Helper.PP.IsInVehicle(), !Helper.PP.IsFalling && !Helper.PP.IsRagdoll, Helper.PP.Position.DistanceTo(privateRequestOrigin));
		}

		private static void PrivateDancerRequestTick()
		{
			if (!privateRequestPending)
			{
				return;
			}
			checked
			{
				try
				{
					if (!PrivateDancerRequestContextValid() || MansionFeatures.IsWorkoutActive || PartyPlayerActions.IsKissing)
					{
						CancelPrivateDancerRequest("The player moved away or changed before arrival.");
						return;
					}
					if (!Helper.MenuPool.IsAnyMenuOpen() && (Game.IsControlJustReleased(Control.FrontendCancel) || Game.IsControlJustReleased(Control.PhoneCancel)))
					{
						CancelPrivateDancerRequest("The visit was cancelled.");
						return;
					}
					if (privateRequestExisting != null)
					{
						PartyPed partyPed = privateRequestExisting;
						if ((object)partyPed.Ped != null && partyPed.Ped.Exists() && !partyPed.Ped.IsDead)
						{
							AcceptPrivateDancerArrival(partyPed, false);
							return;
						}
						privateRequestExisting = null;
					}
					if (!privateRequestModel.HasValue)
					{
						if (privateRequestIndex >= privateRequestCandidates.Length)
						{
							CancelPrivateDancerRequest("No dancer model could be loaded.");
							return;
						}
						string text = privateRequestCandidates[privateRequestIndex];
						privateRequestIndex++;
						Model value = new Model(text);
						if (!value.IsValid || !value.IsInCdImage || !value.IsPed)
						{
							Logger.Log("Private dancer: unavailable model " + text);
							return;
						}
						privateRequestModel = value;
						privateRequestModelDeadline = Game.GameTime + 2000;
						Function.Call(Hash.REQUEST_MODEL, value.Hash);
					}
					if (!privateRequestModel.Value.IsLoaded)
					{
						if (Game.GameTime >= privateRequestModelDeadline)
						{
							Logger.Log("Private dancer: model streaming timed out: " + privateRequestCandidates[privateRequestIndex - 1]);
							privateRequestModel.Value.MarkAsNoLongerNeeded();
							privateRequestModel = null;
						}
						return;
					}
					Vector3 desired = Helper.PP.Position + Helper.PP.ForwardVector * 1.8f;
					Vector3 spot = default(Vector3);
					if (!PartyPlacement.TryFindCompanionSpot(Helper.PP.Position, desired, OccupiedPartyPositions(), ref spot, 1f))
					{
						Logger.Log($"Private dancer: no reachable clear same-floor point near {Helper.PP.Position}; reason={PartyPlacement.LastFailureReason}.");
						CancelPrivateDancerRequest("There is no free standing space nearby.");
						return;
					}
					PartyPed partyPed2 = SpawnLoadedPed(privateRequestModel.Value, privateRequestCandidates[privateRequestIndex - 1], partyActive ? "dancer" : "private", spot, Helper.PP.Heading + 180f);
					if (partyPed2 == null)
					{
						Logger.Log("Private dancer: spawn or verified floor placement failed for " + privateRequestCandidates[privateRequestIndex - 1]);
						privateRequestModel.Value.MarkAsNoLongerNeeded();
						privateRequestModel = null;
						return;
					}
					if (!string.IsNullOrWhiteSpace(PrivateAppearance))
					{
						Function.Call(Hash.SET_PED_DEFAULT_COMPONENT_VARIATION, partyPed2.Ped.Handle);
						Function.Call(Hash.CLEAR_ALL_PED_PROPS, partyPed2.Ped.Handle);
					}
					partyPed2.Dicts = DancerDicts;
					partyPed2.Anims = Shuffle(DancerAnims);
					AcceptPrivateDancerArrival(partyPed2, true);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Private dancer arrival: " + ex2.Message);
					CancelPrivateDancerRequest("The visit could not be started.");
					ProjectData.ClearProjectError();
				}
			}
		}

		private static void RemoveFailedPrivateArrival(PartyPed selected)
		{
			if (selected == null)
			{
				return;
			}
			try
			{
				RemoveHandProp(selected);
				if ((object)selected.Ped != null && selected.Ped.Exists())
				{
					selected.Ped.Delete();
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Private dancer failed-arrival cleanup: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			finally
			{
				partyPeds.Remove(selected);
			}
		}

		private static void AcceptPrivateDancerArrival(PartyPed selected, bool created)
		{
			eOwner eOwner2 = privateRequestOwner;
			bool flag = false;
			try
			{
				if (!PrivateDancerRequestContextValid())
				{
					if (created)
					{
						RemoveFailedPrivateArrival(selected);
					}
					CancelPrivateDancerRequest("The player moved away or changed before arrival.");
					return;
				}
				Function.Call(Hash.FREEZE_ENTITY_POSITION, selected.Ped.Handle, false);
				Function.Call(Hash.CLEAR_PED_TASKS, selected.Ped.Handle);
				Function.Call(Hash.TASK_FOLLOW_TO_OFFSET_OF_ENTITY, selected.Ped.Handle, Helper.PP.Handle, 0f, -1f, 0f, 1.2f, -1, 0.8f, true);
				if ((uint)eOwner2 <= 2u)
				{
					if (!Helper.TryChangeOwnerMoney(eOwner2, checked(-BedroomCost)))
					{
						if (created)
						{
							RemoveFailedPrivateArrival(selected);
						}
						CancelPrivateDancerRequest("Payment could not be completed.");
						return;
					}
					flag = true;
				}
				if (created && !partyActive)
				{
					privateDancer = selected;
					privateDancerActive = true;
					privateDancerCenter = Helper.PP.Position;
					privateDancerExpiresAt = checked(Game.GameTime + 900000);
				}
				companion = selected;
				companionState = 1;
				CancelPrivateDancerRequest();
				Logger.Log($"Private dancer arrived: model={selected.ModelName}, handle={selected.Ped.Handle}, charged={flag}, owner={eOwner2}.");
				Notification.Show("~p~Party Service~w~: your private dancer is here. Press ~INPUT_CONTEXT~ in a clear space to start, or lead her to the bedroom.");
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Private dancer acceptance: " + ex2.Message);
				if (companion == selected)
				{
					companion = null;
					companionState = 0;
				}
				if (created)
				{
					if (privateDancer == selected)
					{
						EndPrivateDancerSession();
					}
					else
					{
						RemoveFailedPrivateArrival(selected);
					}
				}
				else if (selected != null && (object)selected.Ped != null && selected.Ped.Exists())
				{
					StartAnimChain(selected);
				}
				CancelPrivateDancerRequest();
				if (flag)
				{
					RefundPartyCost(eOwner2, BedroomCost, "private dancer visit");
				}
				ProjectData.ClearProjectError();
			}
		}

		private static void CompanionTick()
		{
			switch (companionState)
			{
			case 0:
				if (bedroomBusy || IsPlayerActionActive || MansionFeatures.IsWorkoutActive || Helper.MenuPool.IsAnyMenuOpen() || Helper.PP.IsInVehicle())
				{
					break;
				}
				{
					foreach (PartyPed partyPed in partyPeds)
					{
						if (Operators.CompareString(partyPed.Role, "dancer", false) == 0 && (object)partyPed.Ped != null && partyPed.Ped.Exists() && Helper.PP.Position.DistanceTo(partyPed.Ped.Position) <= 1.6f)
						{
							if (Helper.ShowPersistentHelp($"Press ~INPUT_CONTEXT~ to invite her upstairs (~g~${BedroomCost:0,0}~w~).") && Game.IsControlJustReleased(Control.Context) && TryPay(BedroomCost))
							{
								companion = partyPed;
								companionState = 1;
								Function.Call(Hash.FREEZE_ENTITY_POSITION, partyPed.Ped.Handle, false);
								Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, partyPed.Ped.Handle);
								Function.Call(Hash.TASK_FOLLOW_TO_OFFSET_OF_ENTITY, partyPed.Ped.Handle, Helper.PP.Handle, 0f, -1f, 0f, 1.5f, -1, 0.8f, true);
								Screen.ShowSubtitle("She follows you. Lead her to the bedroom and press ~INPUT_CONTEXT~ again.", 5000);
							}
							break;
						}
					}
					break;
				}
			case 1:
				if (IsPlayerActionActive || MansionFeatures.IsWorkoutActive)
				{
					break;
				}
				if (companion == null || (object)companion.Ped == null || !companion.Ped.Exists())
				{
					companionState = 0;
					companion = null;
				}
				else if (!Helper.MenuPool.IsAnyMenuOpen())
				{
					bool flag = Helper.ShowPersistentHelp("Press ~INPUT_CONTEXT~ to start. Press ~INPUT_FRONTEND_CANCEL~ to send her back.");
					if (Game.IsControlJustReleased(Control.FrontendCancel) || Game.IsControlJustReleased(Control.PhoneCancel))
					{
						SendCompanionBack();
					}
					else if (flag && Game.IsControlJustReleased(Control.Context) && Helper.PP.Position.DistanceTo(companion.Ped.Position) <= 3f)
					{
						BedroomScene();
					}
				}
				break;
			case 2:
				BedroomSceneTick();
				break;
			}
		}

		private static void SendCompanionBack()
		{
			bool flag = bedroomPhase == 0 || OwnBedroomActor();
			CancelBedroomScene();
			if (privateDancerActive && companion == privateDancer)
			{
				if (flag)
				{
					EndPrivateDancerSession();
					return;
				}
				partyPeds.Remove(privateDancer);
				privateDancer = null;
				privateDancerActive = false;
				companion = null;
				companionState = 0;
			}
			else
			{
				if (flag && companion != null && (object)companion.Ped != null && companion.Ped.Exists())
				{
					Function.Call(Hash.FREEZE_ENTITY_POSITION, companion.Ped.Handle, false);
					StartAnimChain(companion);
				}
				companion = null;
				companionState = 0;
			}
		}

		private static Vector3 NearestBed()
		{
			Vector3 vector = Vector3.Zero;
			float num = 30f;
			try
			{
				List<Vector3> list = new List<Vector3>();
				foreach (ApartmentClass apartment in Helper.apartments)
				{
					list.Add(apartment.SavePos);
				}
				list.Add(LowEndApartment.SavePos);
				list.Add(MediumEndApartment.SavePos);
				foreach (Vector3 item in list)
				{
					if (!(item == Vector3.Zero) && !(Math.Abs(item.Z - Helper.PP.Position.Z) > 4f))
					{
						float num2 = Helper.PP.Position.DistanceTo(item);
						if (num2 < num)
						{
							num = num2;
							vector = item;
						}
					}
				}
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
			}
			Logger.Log($"Party: bed {vector} (dist {num:0.0})");
			return vector;
		}

		private static BedInfo ScanBed(Vector3 origin)
		{
			BedInfo result = default(BedInfo);
			checked
			{
				try
				{
					IntersectFlags options = IntersectFlags.LosToEntity;
					RaycastResult raycastResult = World.Raycast(origin + new Vector3(0f, 0f, 0.5f), origin - new Vector3(0f, 0f, 3f), options, Helper.PP);
					if (!raycastResult.DidHit)
					{
						return result;
					}
					float z = raycastResult.HitPosition.Z;
					int num = 18;
					int num2 = num * 2 + 1;
					float[,] array = new float[num2 - 1 + 1, num2 - 1 + 1];
					bool[,] array2 = new bool[num2 - 1 + 1, num2 - 1 + 1];
					int num3 = num2 - 1;
					for (int i = 0; i <= num3; i++)
					{
						int num4 = num2 - 1;
						for (int j = 0; j <= num4; j++)
						{
							float num5 = (float)(i - num) * 0.25f;
							float num6 = (float)(j - num) * 0.25f;
							if (num5 * num5 + num6 * num6 > 20.25f)
							{
								continue;
							}
							float x = origin.X + num5;
							float y = origin.Y + num6;
							RaycastResult raycastResult2 = World.Raycast(new Vector3(x, y, z + 1.5f), new Vector3(x, y, z + 0.1f), options, Helper.PP);
							if (raycastResult2.DidHit)
							{
								float num7 = raycastResult2.HitPosition.Z - z;
								if (num7 >= 0.3f && num7 <= 0.85f)
								{
									array2[i, j] = true;
									array[i, j] = raycastResult2.HitPosition.Z;
								}
							}
						}
					}
					bool[,] array3 = new bool[num2 - 1 + 1, num2 - 1 + 1];
					List<Point> list = new List<Point>();
					int num8 = num2 - 1;
					for (int k = 0; k <= num8; k++)
					{
						int num9 = num2 - 1;
						for (int l = 0; l <= num9; l++)
						{
							if (!array2[k, l] || array3[k, l])
							{
								continue;
							}
							List<Point> list2 = new List<Point>();
							Queue<Point> queue = new Queue<Point>();
							queue.Enqueue(new Point(k, l));
							array3[k, l] = true;
							while (queue.Count > 0)
							{
								Point item = queue.Dequeue();
								list2.Add(item);
								Point[] array4 = new Point[4]
								{
									new Point(1, 0),
									new Point(-1, 0),
									new Point(0, 1),
									new Point(0, -1)
								};
								for (int m = 0; m < array4.Length; m++)
								{
									Point point = array4[m];
									int num10 = item.X + point.X;
									int num11 = item.Y + point.Y;
									if (num10 >= 0 && num11 >= 0 && num10 < num2 && num11 < num2 && array2[num10, num11] && !array3[num10, num11] && !(Math.Abs(array[num10, num11] - array[item.X, item.Y]) > 0.2f))
									{
										array3[num10, num11] = true;
										queue.Enqueue(new Point(num10, num11));
									}
								}
							}
							if (list2.Count > list.Count)
							{
								list = list2;
							}
						}
					}
					if (list.Count < 16)
					{
						Logger.Log($"Party: bed scan at {origin}: no bed (largest area {list.Count} cells)");
						return result;
					}
					double num12 = 0.0;
					double num13 = 0.0;
					double num14 = 0.0;
					foreach (Point item2 in list)
					{
						num12 += (double)item2.X;
						num13 += (double)item2.Y;
						num14 += (double)array[item2.X, item2.Y];
					}
					num12 /= (double)list.Count;
					num13 /= (double)list.Count;
					num14 /= (double)list.Count;
					double num15 = 0.0;
					double num16 = 0.0;
					double num17 = 0.0;
					foreach (Point item3 in list)
					{
						double num18 = (double)item3.X - num12;
						double num19 = (double)item3.Y - num13;
						num15 += num18 * num18;
						num16 += num19 * num19;
						num17 += num18 * num19;
					}
					double num20 = 0.5 * Math.Atan2(2.0 * num17, num15 - num16);
					Vector3 vector = new Vector3((float)Math.Cos(num20), (float)Math.Sin(num20), 0f);
					Vector3 vector2 = new Vector3(origin.X + (float)((num12 - (double)num) * 0.25), origin.Y + (float)((num13 - (double)num) * 0.25), (float)num14);
					Vector3 vector3 = new Vector3(0f, 0f, 0.35f);
					float num21 = 9f;
					float num22 = 9f;
					RaycastResult raycastResult3 = World.Raycast(vector2 + vector3, vector2 + vector3 + vector * 3f, options, Helper.PP);
					if (raycastResult3.DidHit)
					{
						num21 = raycastResult3.HitPosition.DistanceTo(vector2 + vector3);
					}
					RaycastResult raycastResult4 = World.Raycast(vector2 + vector3, vector2 + vector3 - vector * 3f, options, Helper.PP);
					if (raycastResult4.DidHit)
					{
						num22 = raycastResult4.HitPosition.DistanceTo(vector2 + vector3);
					}
					Vector3 vector4 = ((num21 <= num22) ? vector : (-vector));
					result.Found = true;
					result.Center = vector2;
					result.HeadDir = vector4;
					result.Side = new Vector3(0f - vector4.Y, vector4.X, 0f);
					result.TopZ = (float)num14;
					result.FloorZ = z;
					result.Cells = list.Count;
					Logger.Log($"Party: bed found at {vector2} area {(float)list.Count * 0.25f * 0.25f:0.0} m2 head {vector4} (wall +{num21:0.0}/-{num22:0.0})");
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log($"Party: bed scan: {ex2.Message}");
					ProjectData.ClearProjectError();
				}
				return result;
			}
		}

		private static BedInfo FindBed()
		{
			BedInfo result = default(BedInfo);
			Mansion mansion = Mansions.MansionList.Where((_Closure_0024__._0024I210_002D0 != null) ? _Closure_0024__._0024I210_002D0 : (_Closure_0024__._0024I210_002D0 = _Closure_0024__._0024I._Lambda_0024__210_002D0)).OrderBy((_Closure_0024__._0024I210_002D1 != null) ? _Closure_0024__._0024I210_002D1 : (_Closure_0024__._0024I210_002D1 = _Closure_0024__._0024I._Lambda_0024__210_002D1)).FirstOrDefault();
			if (mansion != null)
			{
				_Closure_0024__210_002D0 arg = new _Closure_0024__210_002D0(null);
				arg._0024VB_0024Local_scene = MansionAutoLayout.OriginalMasterBedPoint(mansion);
				if (!PartyPlacement.FinitePoint(Helper.ToVector3(arg._0024VB_0024Local_scene)) || Helper.ToVector3(arg._0024VB_0024Local_scene) == Vector3.Zero)
				{
					return result;
				}
				RestingBedScene[] array = restingBedScenes.Where((_Closure_0024__._0024I210_002D2 != null) ? _Closure_0024__._0024I210_002D2 : (_Closure_0024__._0024I210_002D2 = _Closure_0024__._0024I._Lambda_0024__210_002D2)).ToArray();
				for (int i = 0; i < array.Length; i = checked(i + 1))
				{
					ReleaseRestingBedScene(array[i]);
				}
				if (restingBedScenes.Any(arg._Lambda_0024__3))
				{
					Logger.Log("Party: original master bed is already occupied by another resting companion.");
					return result;
				}
				BedInfo result2 = default(BedInfo);
				result2.Found = true;
				result2.OriginalScene = true;
				result2.SceneOrigin = Helper.ToVector3(arg._0024VB_0024Local_scene);
				result2.SceneHeading = arg._0024VB_0024Local_scene.W;
				result2.Center = Helper.ToVector3(arg._0024VB_0024Local_scene);
				result2.AnimationDict = "anim@mp_bedmid@right_var_02";
				result2.AnimationClip = "f_sleep_r_loop_bighouse";
				result2.Home = mansion;
				return result2;
			}
			Vector3 vector = NearestBed();
			if (vector != Vector3.Zero)
			{
				result = ScanBed(vector);
			}
			if (result.Found)
			{
				result.AnimationDict = BedDict;
				result.AnimationClip = BedAnimF;
			}
			return result;
		}

		private static Vector3 BoneOf(Ped p, int boneId)
		{
			return Function.Call<Vector3>(Hash.GET_PED_BONE_COORDS, p.Handle, boneId, 0f, 0f, 0f);
		}

		public static void CleanupBedroomRestingScenes()
		{
			morningPair = null;
			nextMorningClockAt = 0;
			RestingBedScene[] array = restingBedScenes.ToArray();
			for (int i = 0; i < array.Length; i = checked(i + 1))
			{
				ReleaseRestingBedScene(array[i]);
			}
		}

		private static void ReleaseRestingBedScene(RestingBedScene resting)
		{
			restingBedScenes.Remove(resting);
			try
			{
				if (OwnRestingActor(resting))
				{
					if (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, resting.Actor.Handle, resting.Dict, resting.Clip, 3))
					{
						Function.Call(Hash.CLEAR_PED_TASKS, resting.Actor.Handle);
					}
					Function.Call(Hash.FREEZE_ENTITY_POSITION, resting.Actor.Handle, resting.WasFrozen);
				}
				if ((object)resting.Scene != null && resting.Scene.Exists())
				{
					resting.Scene.IsLooped = false;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Party bed scene cleanup: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		private static bool OwnRestingActor(RestingBedScene resting)
		{
			if (resting != null && (object)resting.Actor != null && resting.Actor.Exists())
			{
				return PartyCompanionPolicy.IdentityMatches(resting.Handle, resting.Model, resting.Address, resting.Actor.Handle, resting.Actor.Model.Hash, resting.Actor.MemoryAddress);
			}
			return false;
		}

		private static bool GroupActorsValid(GroupBedroomState g)
		{
			if (g != null && OwnRestingActor(g.FirstRest) && !g.FirstRest.Actor.IsDead && g.FirstRest.Source != null && partyPeds.Contains(g.FirstRest.Source) && g.Second == bedroomSource && partyPeds.Contains(g.Second) && (object)g.Second.Ped != null && g.Second.Ped.Exists() && PartyCompanionPolicy.IdentityMatches(g.SecondHandle, g.SecondModel, g.SecondAddress, g.Second.Ped.Handle, g.Second.Ped.Model.Hash, g.Second.Ped.MemoryAddress) && !g.Second.Ped.IsDead && (object)Helper.PP != null && Helper.PP.Exists() && (object)g.Player != null && g.Player.Exists() && PartyCompanionPolicy.IdentityMatches(g.PlayerHandle, g.PlayerModel, g.PlayerAddress, Helper.PP.Handle, Helper.PP.Model.Hash, Helper.PP.MemoryAddress) && !Helper.PP.IsDead && !Helper.PP.IsInVehicle() && !g.FirstRest.Actor.IsInVehicle() && !g.Second.Ped.IsInVehicle() && Helper.GetPlayer() == g.FirstRest.Owner && g.FirstRest.Home != null && g.FirstRest.Home.Enabled && g.FirstRest.Home.IplsLoaded && g.FirstRest.Home.IsOwnedByPlayer())
			{
				return Helper.PP.Position.DistanceTo(g.FirstRest.Bed.SceneOrigin) <= 12f;
			}
			return false;
		}

		private static bool TryBeginGroupContinuation()
		{
			_Closure_0024__247_002D0 closure_0024__247_002D = new _Closure_0024__247_002D0();
			if (bedroomGroup != null || !partyActive || bedroomSource == null || !PartyCompanionPolicy.KnownAdultModel(bedroomSource.ModelName))
			{
				return false;
			}
			RestingBedScene[] array = restingBedScenes.Where((_Closure_0024__._0024I247_002D0 != null) ? _Closure_0024__._0024I247_002D0 : (_Closure_0024__._0024I247_002D0 = _Closure_0024__._0024I._Lambda_0024__247_002D0)).ToArray();
			if (array.Length != 1)
			{
				return false;
			}
			closure_0024__247_002D._0024VB_0024Local_first = array[0];
			GroupBedroomState groupBedroomState = new GroupBedroomState
			{
				FirstRest = closure_0024__247_002D._0024VB_0024Local_first,
				Second = bedroomSource,
				Player = Helper.PP,
				PlayerHandle = Helper.PP.Handle,
				PlayerModel = Helper.PP.Model.Hash,
				PlayerAddress = Helper.PP.MemoryAddress,
				SecondHandle = bedroomActor.Handle,
				SecondModel = bedroomActor.Model.Hash,
				SecondAddress = bedroomActor.MemoryAddress
			};
			int sleepingCount = restingBedScenes.Where(closure_0024__247_002D._Lambda_0024__1).Count();
			if (!PartyCompanionPolicy.CanJoin(partyActive, closure_0024__247_002D._0024VB_0024Local_first.Home.IsOwnedByPlayer(), partyPeds.Contains(closure_0024__247_002D._0024VB_0024Local_first.Source) && partyPeds.Contains(bedroomSource), closure_0024__247_002D._0024VB_0024Local_first.Actor.Handle != bedroomActor.Handle, sleepingCount, Helper.PP.Position.DistanceTo(closure_0024__247_002D._0024VB_0024Local_first.Bed.SceneOrigin)))
			{
				return false;
			}
			Vector3 desired = Helper.PP.Position + Helper.PP.RightVector * 1.6f;
			if (!PartyPlacement.TryFindCompanionSpot(Helper.PP.Position, desired, OccupiedPartyPositions(closure_0024__247_002D._0024VB_0024Local_first.Source), ref groupBedroomState.JoinFloor, 0.95f, closure_0024__247_002D._0024VB_0024Local_first.Actor))
			{
				return false;
			}
			bedroomGroup = groupBedroomState;
			RequestPartyDictionary("anim@amb@nightclub@dancers@crowddance_facedj@hi_intensity");
			groupBedroomState.Deadline = checked(Game.GameTime + 3500);
			bedroomPhase = 12;
			Logger.Log("Party: existing resting companion invited to join the non-explicit bedroom dance.");
			return true;
		}

		private static bool ReadMattressSurface(Vector3 position, Ped ignore, ref float surfaceZ)
		{
			bool hit = default(bool);
			Vector3 point = default(Vector3);
			Vector3 normal = default(Vector3);
			if (!PartyPlacement.IsUsableFloorHit(PartyPlacement.PhysicalRay(position + new Vector3(0f, 0f, 0.4f), position - new Vector3(0f, 0f, 0.65f), 81, ref hit, ref point, ref normal, ignore), hit, point, normal.Z) || normal.Z < 0.85f)
			{
				return false;
			}
			surfaceZ = point.Z;
			return true;
		}

		private static bool MattressFootprintSupported(Vector3 pelvis, Vector3 bodyDirection, float surfaceZ, Ped ignore)
		{
			Vector3 vector = new Vector3(0f - bodyDirection.Y, bodyDirection.X, 0f);
			float[] array = new float[5] { -0.85f, -0.425f, 0f, 0.425f, 0.85f };
			float surfaceZ2 = default(float);
			foreach (float num in array)
			{
				float[] array2 = new float[3] { -0.2f, 0f, 0.2f };
				foreach (float num2 in array2)
				{
					if (!ReadMattressSurface(pelvis + bodyDirection * num + vector * num2, ignore, ref surfaceZ2) || !PartyCompanionPolicy.MattressSurfaceAcceptable(surfaceZ, surfaceZ2))
					{
						return false;
					}
				}
			}
			return true;
		}

		private static bool ProbeAdjacentBedSlot(GroupBedroomState g)
		{
			if (!GroupActorsValid(g) || g.ProbeIndex >= 12)
			{
				return false;
			}
			if (g.ProbeIndex == 0)
			{
				if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, g.FirstRest.Handle, g.FirstRest.Dict, g.FirstRest.Clip, 3))
				{
					return false;
				}
				g.FirstPelvis = BoneOf(g.FirstRest.Actor, 11816);
				g.FirstHead = BoneOf(g.FirstRest.Actor, 31086);
				if (!PartyCompanionPolicy.SleepingBodyAxisValid(g.FirstHead, g.FirstPelvis))
				{
					return false;
				}
				g.BodyDirection = g.FirstHead - g.FirstPelvis;
				g.BodyDirection.Z = 0f;
				g.BodyDirection.Normalize();
				g.LateralDirection = new Vector3(0f - g.BodyDirection.Y, g.BodyDirection.X, 0f);
				if (!ReadMattressSurface(g.FirstPelvis, g.FirstRest.Actor, ref g.SurfaceZ))
				{
					return false;
				}
			}
			float num = PartyCompanionPolicy.BedCandidateOffset(g.ProbeIndex);
			checked
			{
				g.ProbeIndex++;
				Vector3 vector = g.LateralDirection * num;
				Vector3 vector2 = g.FirstPelvis + vector;
				float[] array = new float[4] { 0.25f, 0.5f, 0.75f, 1f };
				float surfaceZ = default(float);
				foreach (float num2 in array)
				{
					if (!ReadMattressSurface(g.FirstPelvis + vector * num2, g.FirstRest.Actor, ref surfaceZ) || !PartyCompanionPolicy.MattressSurfaceAcceptable(g.SurfaceZ, surfaceZ))
					{
						return false;
					}
				}
				if (!MattressFootprintSupported(vector2, g.BodyDirection, g.SurfaceZ, g.FirstRest.Actor))
				{
					return false;
				}
				g.SecondBed = g.FirstRest.Bed;
				g.SecondBed.SceneOrigin += vector;
				g.SecondBed.Center += vector;
				g.SecondBed.AnimationDict = g.FirstRest.Dict;
				g.SecondBed.AnimationClip = g.FirstRest.Clip;
				g.SecondPelvisTarget = vector2;
				g.SecondHeadTarget = g.FirstHead + vector;
				g.SlotMeasured = true;
				Logger.Log("Party: second mattress position measured; lateral=" + num + "; surface=" + g.SurfaceZ);
				return true;
			}
		}

		private static bool GroupBedRootsReady(GroupBedroomState g)
		{
			if (!GroupActorsValid(g) || !g.SlotMeasured || !Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, g.FirstRest.Dict))
			{
				return false;
			}
			BedInfo bed = g.FirstRest.Bed;
			BedInfo secondBed = g.SecondBed;
			if (!PartyBedroomPlacement.CanStartRest(bed.Found, true, Function.Call<float>(Hash.GET_ANIM_DURATION, g.FirstRest.Dict, g.FirstRest.Clip)))
			{
				return false;
			}
			Vector3 first = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, g.FirstRest.Dict, g.FirstRest.Clip, bed.SceneOrigin.X, bed.SceneOrigin.Y, bed.SceneOrigin.Z, 0f, 0f, bed.SceneHeading, 0f, 2);
			Vector3 second = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, secondBed.AnimationDict, secondBed.AnimationClip, secondBed.SceneOrigin.X, secondBed.SceneOrigin.Y, secondBed.SceneOrigin.Z, 0f, 0f, secondBed.SceneHeading, 0f, 2);
			return PartyCompanionPolicy.SeparateSleepRoots(first, second, bed.SceneOrigin);
		}

		private static bool StartGroupDances(GroupBedroomState g)
		{
			string text = "anim@amb@nightclub@dancers@crowddance_facedj@hi_intensity";
			if (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, text))
			{
				return false;
			}
			g.PlayerClip = (Function.Call<bool>(Hash.IS_PED_MALE, g.PlayerHandle) ? "hi_dance_facedj_09_v1_male^1" : "hi_dance_facedj_09_v1_female^1");
			if (!IsPlayableAnimationDuration(Function.Call<float>(Hash.GET_ANIM_DURATION, text, g.PlayerClip)) || !IsPlayableAnimationDuration(Function.Call<float>(Hash.GET_ANIM_DURATION, text, "hi_dance_facedj_09_v1_female^1")))
			{
				return false;
			}
			g.DancesStarted = true;
			Ped[] array = new Ped[3]
			{
				g.Player,
				g.FirstRest.Actor,
				g.Second.Ped
			};
			foreach (Ped ped in array)
			{
				string text2 = ((ped.Handle == g.PlayerHandle) ? g.PlayerClip : "hi_dance_facedj_09_v1_female^1");
				Function.Call(Hash.TASK_PLAY_ANIM, ped.Handle, text, text2, 4f, -4f, -1, 1, 0f, false, false, false);
			}
			g.DanceStarted = Game.GameTime;
			Screen.ShowSubtitle((Game.Language == Language.German) ? "Zu dritt ist die Party doch erst komplett!" : "Now this party is complete with the three of us!", 5000);
			return true;
		}

		private static void StopGroupDances(GroupBedroomState g)
		{
			if (g == null || !g.DancesStarted)
			{
				return;
			}
			Ped[] array = new Ped[3]
			{
				g.Player,
				g.FirstRest.Actor,
				g.Second.Ped
			};
			foreach (Ped ped in array)
			{
				try
				{
					bool num;
					if ((object)ped != g.Player)
					{
						if ((object)ped == g.FirstRest.Actor)
						{
							num = OwnRestingActor(g.FirstRest);
							goto IL_00ee;
						}
						if ((object)ped != null && ped.Exists())
						{
							num = PartyCompanionPolicy.IdentityMatches(g.SecondHandle, g.SecondModel, g.SecondAddress, ped.Handle, ped.Model.Hash, ped.MemoryAddress);
							goto IL_00ee;
						}
					}
					else if ((object)ped != null && ped.Exists())
					{
						num = PartyCompanionPolicy.IdentityMatches(g.PlayerHandle, g.PlayerModel, g.PlayerAddress, ped.Handle, ped.Model.Hash, ped.MemoryAddress);
						goto IL_00ee;
					}
					goto end_IL_0048;
					IL_00ee:
					if (num)
					{
						string text = (((object)ped == g.Player) ? g.PlayerClip : "hi_dance_facedj_09_v1_female^1");
						if (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, ped.Handle, "anim@amb@nightclub@dancers@crowddance_facedj@hi_intensity", text, 3))
						{
							Function.Call(Hash.STOP_ANIM_TASK, ped.Handle, "anim@amb@nightclub@dancers@crowddance_facedj@hi_intensity", text, -4f);
						}
					}
					end_IL_0048:;
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Party group dance cleanup: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
			g.DancesStarted = false;
		}

		private static void BeginBedroomFade(int now)
		{
			if (bedroomGroup != null)
			{
				StopGroupDances(bedroomGroup);
			}
			Screen.FadeOut(1500);
			bedroomFaded = true;
			bedroomPhase = 4;
			bedroomPhaseAt = checked(now + 1600);
		}

		private static void GroupContinuationTick(int now, bool menuOpen)
		{
			GroupBedroomState groupBedroomState = bedroomGroup;
			if (!GroupActorsValid(groupBedroomState) || PartyPlayerActions.MovementRequested() || Helper.PP.Position.DistanceTo(bedroomStartPosition) > 3f)
			{
				SendCompanionBack();
				return;
			}
			checked
			{
				switch (bedroomPhase)
				{
				case 12:
					if (ProbeAdjacentBedSlot(groupBedroomState))
					{
						if (!GroupBedRootsReady(groupBedroomState))
						{
							Notification.Show("The second bed position could not be confirmed. She can keep resting.");
							SendCompanionBack();
							break;
						}
						ReleaseRestingBedScene(groupBedroomState.FirstRest);
						groupBedroomState.FirstRest.Source.Role = "bedroom_group";
						groupBedroomState.FirstRest.Source.Settled = true;
						groupBedroomState.FirstRest.Source.AnimPlayingRequested = false;
						Function.Call(Hash.FREEZE_ENTITY_POSITION, groupBedroomState.FirstRest.Handle, false);
						Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, groupBedroomState.FirstRest.Handle, groupBedroomState.JoinFloor.X, groupBedroomState.JoinFloor.Y, groupBedroomState.JoinFloor.Z, 1f, 8000, Helper.PP.Heading + 180f, 0.25f);
						groupBedroomState.Deadline = now + 8000;
						bedroomPhase = 9;
					}
					else if (now >= groupBedroomState.Deadline || groupBedroomState.ProbeIndex >= 12)
					{
						Notification.Show("There is not enough connected mattress space for two. She can keep resting.");
						SendCompanionBack();
					}
					break;
				case 9:
					if (DestinationStreaming.PedAtDestination(groupBedroomState.FirstRest.Actor.Position, groupBedroomState.JoinFloor) && groupBedroomState.FirstRest.Actor.Position.DistanceTo(Helper.PP.Position) < 3f && StartGroupDances(groupBedroomState))
					{
						bedroomPhase = 10;
					}
					else if (now >= groupBedroomState.Deadline)
					{
						Notification.Show("She could not reach a clear place to join. She is returning to her bed.");
						SendCompanionBack();
					}
					break;
				case 10:
				{
					if (now - groupBedroomState.DanceStarted > 1200 && (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, groupBedroomState.FirstRest.Handle, "anim@amb@nightclub@dancers@crowddance_facedj@hi_intensity", "hi_dance_facedj_09_v1_female^1", 3) || !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, groupBedroomState.SecondHandle, "anim@amb@nightclub@dancers@crowddance_facedj@hi_intensity", "hi_dance_facedj_09_v1_female^1", 3)))
					{
						SendCompanionBack();
						break;
					}
					if (!menuOpen)
					{
						Helper.ShowPersistentHelp((now - groupBedroomState.DanceStarted < 8000) ? "Dancing together: ~INPUT_FRONTEND_CANCEL~ stop." : "~INPUT_CONTEXT~ kiss her  ~INPUT_FRONTEND_CANCEL~ stop; wait to settle down together.");
					}
					if (now - groupBedroomState.DanceStarted >= 8000)
					{
						if (!GroupBedRootsReady(groupBedroomState))
						{
							Notification.Show("This bed does not provide two separate sleeping positions. She is returning to her bed.");
							SendCompanionBack();
							break;
						}
						if (!menuOpen && Game.IsControlJustReleased(Control.Context))
						{
							StopGroupDances(groupBedroomState);
							if (PartyPlayerActions.StartPairedKiss(groupBedroomState.FirstRest.Actor, KissDict, KissAnimM, KissAnimF))
							{
								bedroomPhase = 11;
							}
							else
							{
								SendCompanionBack();
							}
						}
						else if (now - groupBedroomState.DanceStarted >= 20000)
						{
							BeginBedroomFade(now);
						}
					}
					Control[] array = new Control[7]
					{
						Control.MoveLeftRight,
						Control.MoveUpDown,
						Control.Attack,
						Control.Aim,
						Control.Jump,
						Control.Sprint,
						Control.Enter
					};
					for (int i = 0; i < array.Length; i++)
					{
						Game.DisableControlThisFrame(array[i]);
					}
					break;
				}
				case 11:
					switch (PartyPlayerActions.LastPairedKissOutcome)
					{
					case PartyPlayerActions.PairedKissOutcome.Succeeded:
						BeginBedroomFade(now);
						break;
					default:
						SendCompanionBack();
						break;
					case PartyPlayerActions.PairedKissOutcome.Running:
						break;
					}
					break;
				}
			}
		}

		private static bool ResumeGroupSleeper(GroupBedroomState g)
		{
			if (g == null || !OwnRestingActor(g.FirstRest))
			{
				return false;
			}
			if (restingBedScenes.Contains(g.FirstRest))
			{
				g.FirstResumed = true;
				return true;
			}
			RestingBedScene firstRest = g.FirstRest;
			BedInfo bed = firstRest.Bed;
			if (firstRest.Home == null || !firstRest.Home.Enabled || !firstRest.Home.IplsLoaded || firstRest.Home.Owner != firstRest.Owner || !Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, firstRest.Dict))
			{
				return false;
			}
			int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, bed.SceneOrigin.X, bed.SceneOrigin.Y, bed.SceneOrigin.Z);
			if (num == 0 || !Function.Call<bool>(Hash.IS_INTERIOR_READY, num))
			{
				return false;
			}
			FwSyncedScene fwSyncedScene = FwSyncedScene.Create(bed.SceneOrigin, new Vector3(0f, 0f, bed.SceneHeading));
			if ((object)fwSyncedScene == null)
			{
				return false;
			}
			try
			{
				Function.Call(Hash.CLEAR_PED_TASKS, firstRest.Handle);
				Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, firstRest.Handle, bed.SceneOrigin.X, bed.SceneOrigin.Y, bed.SceneOrigin.Z, false, false, false);
				Function.Call(Hash.SET_ENTITY_HEADING, firstRest.Handle, bed.SceneHeading);
				Function.Call(Hash.FREEZE_ENTITY_POSITION, firstRest.Handle, true);
				fwSyncedScene.IsLooped = true;
				fwSyncedScene.HoldsLastFrame = false;
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, firstRest.Handle, fwSyncedScene.Handle, firstRest.Dict, firstRest.Clip, 1000f, -1000f, 261, 0, 1000f, 0);
				firstRest.Scene = fwSyncedScene;
				firstRest.Source.Role = "resting";
				firstRest.Source.Settled = true;
				firstRest.Source.AnimPlayingRequested = false;
				restingBedScenes.Add(firstRest);
				g.FirstResumed = true;
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				try
				{
					if (fwSyncedScene.Exists())
					{
						fwSyncedScene.IsLooped = false;
					}
					if (OwnRestingActor(firstRest))
					{
						if (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, firstRest.Handle, firstRest.Dict, firstRest.Clip, 3))
						{
							Function.Call(Hash.STOP_ANIM_TASK, firstRest.Handle, firstRest.Dict, firstRest.Clip, -4f);
						}
						Function.Call(Hash.FREEZE_ENTITY_POSITION, firstRest.Handle, firstRest.WasFrozen);
					}
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					ProjectData.ClearProjectError();
				}
				Logger.Log("Party group sleeper resume: " + ex2.Message);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		private static bool GroupRestConfirmed()
		{
			GroupBedroomState groupBedroomState = bedroomGroup;
			if (!GroupActorsValid(groupBedroomState) || !OwnRestingActor(groupBedroomState.FirstRest) || (object)groupBedroomState.FirstRest.Scene == null || !groupBedroomState.FirstRest.Scene.Exists())
			{
				return false;
			}
			if (!PartyBedroomPlacement.RestPoseConfirmed(Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, groupBedroomState.FirstRest.Handle, groupBedroomState.FirstRest.Dict, groupBedroomState.FirstRest.Clip, 3), Function.Call<bool>(Hash.IS_SYNCHRONIZED_SCENE_RUNNING, groupBedroomState.FirstRest.Scene.Handle), groupBedroomState.FirstRest.Actor.Position.DistanceTo(groupBedroomState.FirstRest.Bed.SceneOrigin)))
			{
				return false;
			}
			if (Game.GameTime < groupBedroomState.NextRestVerify)
			{
				return false;
			}
			groupBedroomState.NextRestVerify = checked(Game.GameTime + 250);
			Vector3 vector = BoneOf(bedroomActor, 11816);
			Vector3 secondHead = BoneOf(bedroomActor, 31086);
			if (!PartyCompanionPolicy.MeasuredSleepPoseConfirmed(BoneOf(groupBedroomState.FirstRest.Actor, 11816), vector, secondHead, groupBedroomState.SecondPelvisTarget, groupBedroomState.SecondHeadTarget))
			{
				return false;
			}
			return MattressFootprintSupported(vector, groupBedroomState.BodyDirection, groupBedroomState.SurfaceZ, bedroomActor);
		}

		private static void CancelGroupContinuation()
		{
			GroupBedroomState groupBedroomState = bedroomGroup;
			if (groupBedroomState == null)
			{
				return;
			}
			bedroomGroup = null;
			StopGroupDances(groupBedroomState);
			try
			{
				if (!ResumeGroupSleeper(groupBedroomState) && OwnRestingActor(groupBedroomState.FirstRest) && groupBedroomState.FirstRest.Source != null)
				{
					Function.Call(Hash.FREEZE_ENTITY_POSITION, groupBedroomState.FirstRest.Handle, groupBedroomState.FirstRest.WasFrozen);
					groupBedroomState.FirstRest.Source.Role = "dancer";
					groupBedroomState.FirstRest.Source.HomePos = groupBedroomState.FirstRest.Source.FloorPos;
					StartAnimChain(groupBedroomState.FirstRest.Source);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Party: restore original sleeping companion: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		private static int ReadPartyClock()
		{
			return checked(Function.Call<int>(Hash.GET_CLOCK_HOURS) * 3600 + Function.Call<int>(Hash.GET_CLOCK_MINUTES) * 60 + Function.Call<int>(Hash.GET_CLOCK_SECONDS));
		}

		private static void MorningCompanionTick()
		{
			MorningPairState morningPairState = morningPair;
			if (morningPairState != null && Game.GameTime >= nextMorningClockAt)
			{
				nextMorningClockAt = checked(Game.GameTime + 250);
				morningPairState.Greeting.Observe(ReadPartyClock());
				bool flag = OwnRestingActor(morningPairState.First) && OwnRestingActor(morningPairState.Second) && !morningPairState.First.Actor.IsDead && !morningPairState.Second.Actor.IsDead && restingBedScenes.Contains(morningPairState.First) && restingBedScenes.Contains(morningPairState.Second);
				Mansion home = morningPairState.First.Home;
				bool sameHome = home != null && home.Enabled && home.IplsLoaded && home.IsOwnedByPlayer() && (object)Helper.PP != null && Helper.PP.Exists() && Helper.PP.Position.DistanceTo(morningPairState.First.Bed.SceneOrigin) <= 12f;
				if (morningPairState.Greeting.TryGreet(sameHome, Helper.GetPlayer() == morningPairState.Owner, flag, !IsPlayerActionActive && !Helper.MenuPool.IsAnyMenuOpen() && !PartyPlayerActions.IsKissing))
				{
					Screen.ShowSubtitle((Game.Language == Language.German) ? "Na, noch Energie, du Stier? Erst mal Kaffee!" : "Still got energy, tiger? Coffee first!", 6000);
					Logger.Log("Party: one morning greeting after both companions stayed and at least six game hours elapsed.");
				}
				if (!flag)
				{
					morningPair = null;
				}
			}
		}

		private static void AlignBedHeading(Ped girl, BedInfo bed)
		{
			Vector3 vector = BoneOf(girl, 31086) - BoneOf(girl, 11816);
			vector.Z = 0f;
			if (!(vector.Length() <= 0.2f))
			{
				double num = Math.Atan2(bed.HeadDir.Y, bed.HeadDir.X);
				double num2 = Math.Atan2(vector.Y, vector.X);
				girl.Heading += (float)((num - num2) * 180.0 / Math.PI);
			}
		}

		private static void AlignBedPosition(Ped girl, BedInfo bed)
		{
			Vector3 vector = BoneOf(girl, 31086);
			Vector3 vector2 = BoneOf(girl, 11816);
			Vector3 vector3 = (vector + vector2) / 2f;
			Vector3 vector4 = bed.Center + bed.HeadDir * 0.3f;
			Vector3 point = girl.Position + new Vector3(vector4.X - vector3.X, vector4.Y - vector3.Y, bed.TopZ + 0.14f - vector2.Z);
			if (PartyPlacement.FinitePoint(point))
			{
				Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, girl.Handle, point.X, point.Y, point.Z, false, false, false);
			}
		}

		private static void ResetBedroomTracking()
		{
			bedroomPhase = 0;
			bedroomBusy = false;
			bedroomActor = null;
			bedroomPlayer = null;
			bedroomSource = null;
			bedroomActorHandle = 0;
			bedroomActorModel = 0;
			bedroomActorAddress = IntPtr.Zero;
			bedroomPlayerHandle = 0;
			bedroomPlayerModel = 0;
			bedroomPlayerAddress = IntPtr.Zero;
			bedroomPrivateClip = null;
			bedroomBed = default(BedInfo);
			bedroomFaded = false;
			bedroomFrozeActor = false;
			bedroomAlignPass = 0;
			bedroomActorMoved = false;
			bedroomRestScene = null;
		}

		private static void CancelBedroomScene()
		{
			if (bedroomPhase == 0 && !bedroomBusy)
			{
				return;
			}
			Ped ped = bedroomActor;
			Ped ped2 = bedroomPlayer;
			int handle = bedroomActorHandle;
			int model = bedroomActorModel;
			IntPtr address = bedroomActorAddress;
			int handle2 = bedroomPlayerHandle;
			int model2 = bedroomPlayerModel;
			IntPtr address2 = bedroomPlayerAddress;
			bool flag = bedroomFrozeActor;
			bool flag2 = bedroomWasFrozen;
			bool flag3 = bedroomFaded;
			string text = bedroomPrivateClip;
			string animationDict = bedroomBed.AnimationDict;
			string animationClip = bedroomBed.AnimationClip;
			FwSyncedScene fwSyncedScene = bedroomRestScene;
			bool flag4 = bedroomActorMoved;
			Vector3 vector = bedroomActorReturnPosition;
			float num = bedroomActorReturnHeading;
			CancelGroupContinuation();
			ResetBedroomTracking();
			PartyPlayerActions.CancelPairedKiss();
			try
			{
				if ((object)ped != null && ped.Exists() && PartyCompanionPolicy.IdentityMatches(handle, model, address, ped.Handle, ped.Model.Hash, ped.MemoryAddress))
				{
					if (flag)
					{
						Function.Call(Hash.FREEZE_ENTITY_POSITION, ped.Handle, flag2);
					}
					if (text != null && Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, ped.Handle, BedroomDict, text, 3))
					{
						Function.Call(Hash.STOP_ANIM_TASK, ped.Handle, BedroomDict, text, -4f);
					}
					if (!string.IsNullOrEmpty(animationDict) && !string.IsNullOrEmpty(animationClip) && Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, ped.Handle, animationDict, animationClip, 3))
					{
						Function.Call(Hash.CLEAR_PED_TASKS, ped.Handle);
					}
					if (flag4)
					{
						Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, ped.Handle, vector.X, vector.Y, vector.Z, false, false, false);
						Function.Call(Hash.SET_ENTITY_HEADING, ped.Handle, num);
						Function.Call(Hash.SET_ENTITY_VELOCITY, ped.Handle, 0f, 0f, 0f);
					}
				}
				if ((object)fwSyncedScene != null && fwSyncedScene.Exists())
				{
					fwSyncedScene.IsLooped = false;
				}
				if ((object)ped2 != null && ped2.Exists() && PartyCompanionPolicy.IdentityMatches(handle2, model2, address2, ped2.Handle, ped2.Model.Hash, ped2.MemoryAddress))
				{
					Function.Call(Hash.TASK_CLEAR_LOOK_AT, ped2.Handle);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Party bedroom cleanup: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (flag3)
				{
					Screen.FadeIn(500);
				}
			}
		}

		private static void BedroomScene()
		{
			if (bedroomBusy || (object)consumePed != null || MansionFeatures.IsWorkoutActive || companion == null || (object)companion.Ped == null || !companion.Ped.Exists() || (object)Helper.PP == null || !Helper.PP.Exists())
			{
				return;
			}
			try
			{
				PartyPlayerActions.StopPlayerDance();
				Ped ped = companion.Ped;
				Ped pP = Helper.PP;
				PartyPed partyPed = companion;
				Vector3 position = ped.Position;
				float heading = ped.Heading;
				Vector3 spot = default(Vector3);
				if (!PartyPlacement.TryFindCompanionSpot(pP.Position, pP.Position + pP.ForwardVector * 1.4f, OccupiedPartyPositions(partyPed), ref spot, 0.8f, ped))
				{
					Logger.Log($"Private dance: no clear same-floor performance position near {pP.Position}; reason={PartyPlacement.LastFailureReason}.");
					Notification.Show("There is not enough free standing space to dance here. Move into a clearer area and try again.");
				}
				else if ((object)Helper.PP != null && Helper.PP.Exists() && Helper.PP.Handle == pP.Handle && !Helper.PP.IsDead && companion == partyPed && ped.Exists())
				{
					if (!PartyPlacement.TryPlacePedOnFloor(ped, spot, pP.Heading + 180f))
					{
						Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, ped.Handle, position.X, position.Y, position.Z, false, false, false);
						Function.Call(Hash.SET_ENTITY_HEADING, ped.Handle, heading);
						Notification.Show("The dance position became blocked. Move into a clearer area and try again.");
					}
					else if ((object)Helper.PP != null && Helper.PP.Exists() && Helper.PP.Handle == pP.Handle && !Helper.PP.IsDead && companion == partyPed)
					{
						bedroomActor = ped;
						bedroomPlayer = pP;
						bedroomSource = partyPed;
						bedroomActorHandle = ped.Handle;
						bedroomActorModel = ped.Model.Hash;
						bedroomActorAddress = ped.MemoryAddress;
						bedroomPlayerHandle = pP.Handle;
						bedroomPlayerModel = pP.Model.Hash;
						bedroomPlayerAddress = pP.MemoryAddress;
						bedroomOwner = Helper.GetPlayer();
						bedroomActorReturnPosition = position;
						bedroomActorReturnHeading = heading;
						bedroomWasFrozen = ped.IsPositionFrozen;
						bedroomStartPosition = Helper.PP.Position;
						bedroomStartedAt = Game.GameTime;
						bedroomPhase = 1;
						bedroomPhaseAt = checked(Game.GameTime + 2000);
						bedroomBusy = true;
						companionState = 2;
						Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, ped.Handle);
						RequestPartyDictionary(BedroomDict);
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Party bedroom start: " + ex2.Message);
				SendCompanionBack();
				ProjectData.ClearProjectError();
			}
		}

		private static bool OwnBedroomActor()
		{
			if ((object)bedroomActor != null && bedroomActor.Exists())
			{
				return PartyCompanionPolicy.IdentityMatches(bedroomActorHandle, bedroomActorModel, bedroomActorAddress, bedroomActor.Handle, bedroomActor.Model.Hash, bedroomActor.MemoryAddress);
			}
			return false;
		}

		private static bool BedroomActorsValid()
		{
			if (OwnBedroomActor() && (object)bedroomPlayer != null && bedroomPlayer.Exists() && PartyCompanionPolicy.IdentityMatches(bedroomPlayerHandle, bedroomPlayerModel, bedroomPlayerAddress, bedroomPlayer.Handle, bedroomPlayer.Model.Hash, bedroomPlayer.MemoryAddress) && CompanionSessionActive && companion == bedroomSource && (object)Helper.PP != null && Helper.PP.Exists() && (object)bedroomPlayer != null && bedroomPlayer.Exists() && (object)bedroomActor != null && bedroomActor.Exists() && Helper.PP.Handle == bedroomPlayer.Handle && !bedroomPlayer.IsDead && !bedroomActor.IsDead && Helper.GetPlayer() == bedroomOwner && !bedroomPlayer.IsInVehicle() && !bedroomActor.IsInVehicle() && !bedroomPlayer.IsFalling)
			{
				return !bedroomPlayer.IsRagdoll;
			}
			return false;
		}

		private static void BeginBedroomKiss()
		{
			if (!PartyPlayerActions.StartPairedKiss(bedroomActor, KissDict, KissAnimM, KissAnimF))
			{
				SendCompanionBack();
			}
			else
			{
				bedroomPhase = 3;
			}
		}

		private static void BedroomSceneTick()
		{
			if (bedroomPhase == 0)
			{
				return;
			}
			checked
			{
				try
				{
					if (!BedroomActorsValid())
					{
						SendCompanionBack();
						return;
					}
					int gameTime = Game.GameTime;
					bool flag = Helper.MenuPool.IsAnyMenuOpen();
					if (!flag && (Game.IsControlJustPressed(Control.FrontendCancel) || Game.IsControlJustPressed(Control.PhoneCancel)))
					{
						SendCompanionBack();
						return;
					}
					if (bedroomPhase <= 3 && (bedroomPlayer.Position.DistanceTo(bedroomStartPosition) > 3f || bedroomActor.Position.DistanceTo(bedroomPlayer.Position) > 4f))
					{
						SendCompanionBack();
						return;
					}
					if (bedroomGroup != null && !GroupActorsValid(bedroomGroup))
					{
						SendCompanionBack();
						return;
					}
					switch (bedroomPhase)
					{
					case 1:
						if (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, BedroomDict))
						{
							foreach (string item in Shuffle(BedroomAnims).Take(32))
							{
								if (IsPlayableAnimationDuration(Function.Call<float>(Hash.GET_ANIM_DURATION, BedroomDict, item)))
								{
									bedroomPrivateClip = item;
									break;
								}
							}
							if (bedroomPrivateClip == null)
							{
								Notification.Show("The configured private dance is unavailable in this game build.");
								SendCompanionBack();
								break;
							}
							Function.Call(Hash.TASK_PLAY_ANIM, bedroomActor.Handle, BedroomDict, bedroomPrivateClip, 4f, -4f, -1, 1, 0f, false, false, false);
							Function.Call(Hash.TASK_LOOK_AT_ENTITY, bedroomPlayer.Handle, bedroomActor.Handle, -1, 2048, 3);
							Logger.Log($"Private dance playback requested: {BedroomDict}/{bedroomPrivateClip}; actor={bedroomActor.Handle}.");
							bedroomStartedAt = gameTime;
							bedroomPhase = 2;
						}
						else if (gameTime >= bedroomPhaseAt)
						{
							Notification.Show("The private dance could not be loaded.");
							SendCompanionBack();
						}
						break;
					case 2:
					{
						if (gameTime - bedroomStartedAt > 1200 && !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, bedroomActor.Handle, BedroomDict, bedroomPrivateClip, 3))
						{
							Logger.Log($"Private dance playback stopped or failed: {BedroomDict}/{bedroomPrivateClip}; actor={bedroomActor.Handle}.");
							Notification.Show("The dancer could not keep the animation playing here.");
							SendCompanionBack();
							break;
						}
						if (!flag)
						{
							Helper.ShowPersistentHelp("~INPUT_CONTEXT~ skip the dance  ~INPUT_FRONTEND_CANCEL~ stop");
							if (gameTime - bedroomStartedAt > 500 && PartyPlayerActions.MovementRequested())
							{
								SendCompanionBack();
								break;
							}
						}
						Control[] array = new Control[7]
						{
							Control.MoveLeftRight,
							Control.MoveUpDown,
							Control.Jump,
							Control.Sprint,
							Control.Attack,
							Control.Aim,
							Control.Enter
						};
						for (int i = 0; i < array.Length; i++)
						{
							Game.DisableControlThisFrame(array[i]);
						}
						if (Operators.CompareString(PartyPlayerActions.TimedActionOutcome(unchecked((long)gameTime) - unchecked((long)bedroomStartedAt), BedroomDurationSec * 1000, false), "Completed", false) == 0 || (!flag && gameTime - bedroomStartedAt > 500 && Game.IsControlJustReleased(Control.Context)))
						{
							BeginBedroomKiss();
						}
						break;
					}
					case 3:
						switch (PartyPlayerActions.LastPairedKissOutcome)
						{
						case PartyPlayerActions.PairedKissOutcome.Succeeded:
							if (!TryBeginGroupContinuation())
							{
								BeginBedroomFade(gameTime);
							}
							break;
						case PartyPlayerActions.PairedKissOutcome.Running:
							break;
						default:
							SendCompanionBack();
							break;
						}
						break;
					case 4:
						if (gameTime >= bedroomPhaseAt)
						{
							PrepareBedroomRest();
						}
						break;
					case 9:
					case 10:
					case 11:
					case 12:
						GroupContinuationTick(gameTime, flag);
						break;
					case 5:
					{
						bool flag2 = Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, bedroomBed.AnimationDict);
						if (!flag2 && gameTime < bedroomPhaseAt)
						{
							break;
						}
						float duration = (flag2 ? Function.Call<float>(Hash.GET_ANIM_DURATION, bedroomBed.AnimationDict, bedroomBed.AnimationClip) : 0f);
						if (!PartyBedroomPlacement.CanStartRest(bedroomBed.Found, flag2, duration))
						{
							Logger.Log("Party: bed rest cancelled; authored resting animation unavailable.");
							Notification.Show("The bed animation could not be loaded. She has returned to the party.");
							SendCompanionBack();
							break;
						}
						if (bedroomBed.OriginalScene)
						{
							BedInfo bedInfo = bedroomBed;
							int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, bedInfo.SceneOrigin.X, bedInfo.SceneOrigin.Y, bedInfo.SceneOrigin.Z);
							Function.Call(Hash.REQUEST_COLLISION_AT_COORD, bedInfo.SceneOrigin.X, bedInfo.SceneOrigin.Y, bedInfo.SceneOrigin.Z);
							if (num == 0 || !Function.Call<bool>(Hash.IS_INTERIOR_READY, num))
							{
								if (gameTime >= bedroomPhaseAt)
								{
									Notification.Show("The bedroom is not ready. She has returned to the party.");
									SendCompanionBack();
								}
								break;
							}
							bedroomRestScene = FwSyncedScene.Create(bedInfo.SceneOrigin, new Vector3(0f, 0f, bedInfo.SceneHeading));
							if ((object)bedroomRestScene == null)
							{
								SendCompanionBack();
								break;
							}
							bedroomActorMoved = true;
							Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, bedroomActor.Handle);
							Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, bedroomActor.Handle, bedInfo.SceneOrigin.X, bedInfo.SceneOrigin.Y, bedInfo.SceneOrigin.Z, false, false, false);
							Function.Call(Hash.SET_ENTITY_HEADING, bedroomActor.Handle, bedInfo.SceneHeading);
							Function.Call(Hash.SET_ENTITY_VELOCITY, bedroomActor.Handle, 0f, 0f, 0f);
							Function.Call(Hash.FREEZE_ENTITY_POSITION, bedroomActor.Handle, true);
							bedroomFrozeActor = true;
							bedroomRestScene.IsLooped = true;
							bedroomRestScene.HoldsLastFrame = false;
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, bedroomActor.Handle, bedroomRestScene.Handle, bedInfo.AnimationDict, bedInfo.AnimationClip, 1000f, -1000f, 261, 0, 1000f, 0);
							Logger.Log($"Party: original master bed scene requested at {bedInfo.SceneOrigin}, heading={bedInfo.SceneHeading:0.000}; {bedInfo.AnimationDict}/{bedInfo.AnimationClip}.");
							bedroomPhase = 8;
							bedroomPhaseAt = gameTime + 1000;
						}
						else
						{
							Function.Call(Hash.TASK_PLAY_ANIM, bedroomActor.Handle, bedroomBed.AnimationDict, bedroomBed.AnimationClip, 8f, -8f, -1, 1, 0f, false, false, false);
							bedroomAlignPass = 0;
							bedroomPhase = 6;
							bedroomPhaseAt = gameTime + 150;
						}
						bedroomStartedAt = gameTime;
						break;
					}
					case 6:
						if (gameTime >= bedroomPhaseAt)
						{
							AlignBedHeading(bedroomActor, bedroomBed);
							bedroomPhase = 7;
							bedroomPhaseAt = gameTime + 100;
						}
						break;
					case 7:
						if (gameTime >= bedroomPhaseAt)
						{
							AlignBedPosition(bedroomActor, bedroomBed);
							bedroomAlignPass++;
							bedroomPhase = ((bedroomAlignPass < 3) ? 6 : 8);
							bedroomPhaseAt = gameTime + ((bedroomAlignPass < 3) ? 150 : 250);
						}
						break;
					case 8:
					{
						if (gameTime < bedroomPhaseAt)
						{
							break;
						}
						bool animationPlaying = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, bedroomActor.Handle, bedroomBed.AnimationDict, bedroomBed.AnimationClip, 3);
						bool sceneRunning = !bedroomBed.OriginalScene || ((object)bedroomRestScene != null && bedroomRestScene.Exists() && Function.Call<bool>(Hash.IS_SYNCHRONIZED_SCENE_RUNNING, bedroomRestScene.Handle));
						if (!PartyBedroomPlacement.RestPoseConfirmed(animationPlaying, sceneRunning, bedroomActor.Position.DistanceTo(bedroomBed.Center)))
						{
							if (gameTime - bedroomStartedAt >= 3000)
							{
								Logger.Log("Party: bed rest did not start at the actual bed; restoring the companion.");
								Notification.Show("She could not settle into the bed. She has returned to the party.");
								SendCompanionBack();
							}
							break;
						}
						if (bedroomGroup != null && !GroupRestConfirmed())
						{
							if (gameTime - bedroomStartedAt >= 3000)
							{
								Logger.Log("Party: second bed side was not independently confirmed; restoring the first companion.");
								SendCompanionBack();
							}
							break;
						}
						bedroomSource.Settled = true;
						bedroomSource.AnimPlayingRequested = false;
						bedroomSource.Role = "resting";
						bedroomSource.HomePos = bedroomActor.Position;
						RestingBedScene restingBedScene = new RestingBedScene
						{
							Actor = bedroomActor,
							Scene = bedroomRestScene,
							WasFrozen = bedroomWasFrozen,
							Dict = bedroomBed.AnimationDict,
							Clip = bedroomBed.AnimationClip,
							Handle = bedroomActor.Handle,
							Model = bedroomActor.Model.Hash,
							Address = bedroomActor.MemoryAddress,
							Source = bedroomSource,
							Home = bedroomBed.Home,
							Owner = bedroomOwner,
							Bed = bedroomBed
						};
						restingBedScenes.Add(restingBedScene);
						Function.Call(Hash.TASK_CLEAR_LOOK_AT, bedroomPlayer.Handle);
						Function.Call(Hash.ADD_TO_CLOCK_TIME, 1, 30, 0);
						Screen.FadeIn(2000);
						if (bedroomGroup != null)
						{
							StopGroupDances(bedroomGroup);
							morningPair = new MorningPairState
							{
								First = bedroomGroup.FirstRest,
								Second = restingBedScene,
								Owner = bedroomOwner
							};
							morningPair.Greeting.Arm(ReadPartyClock());
							bedroomGroup = null;
							Notification.Show("~p~Party Service~w~: Both companions are staying the night.");
						}
						else
						{
							Notification.Show("~p~Party Service~w~: She's staying the night.");
						}
						companion = null;
						companionState = 0;
						ResetBedroomTracking();
						break;
					}
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Party bedroom tick: " + ex2.Message);
					SendCompanionBack();
					ProjectData.ClearProjectError();
				}
			}
		}

		private static void PrepareBedroomRest()
		{
			if (bedroomGroup != null && !ResumeGroupSleeper(bedroomGroup))
			{
				SendCompanionBack();
				return;
			}
			bedroomBed = ((bedroomGroup == null) ? FindBed() : bedroomGroup.SecondBed);
			if (!bedroomBed.Found)
			{
				Logger.Log("Party: no actual bed is available; floor resting is disabled.");
				Notification.Show("No bed is available here. She has returned to the party.");
				SendCompanionBack();
				return;
			}
			Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, bedroomActor.Handle);
			if (!bedroomBed.OriginalScene)
			{
				BedInfo bedInfo = bedroomBed;
				Vector3 vector = bedInfo.Center + new Vector3(0f, 0f, 0.6f);
				bedroomActorMoved = true;
				Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, bedroomActor.Handle, vector.X, vector.Y, vector.Z, false, false, false);
				bedroomActor.Heading = (float)(Math.Atan2(bedInfo.HeadDir.Y, bedInfo.HeadDir.X) * 180.0 / Math.PI) - 90f;
				Function.Call(Hash.FREEZE_ENTITY_POSITION, bedroomActor.Handle, true);
				bedroomFrozeActor = true;
			}
			RequestPartyDictionary(bedroomBed.AnimationDict);
			bedroomPhase = 5;
			bedroomPhaseAt = checked(Game.GameTime + 2000);
		}
	}
}
