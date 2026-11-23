using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using iFruitAddon2;
using INMNativeUI;
using Microsoft.VisualBasic.CompilerServices;
using SPAII.INM;

namespace SPAII
{
	[StandardModule]
	public static class MansionFeatures
	{
		internal class PodiumModelRequest
		{
			public int UID;

			public int ModelHash;

			public int StartedAt;

			public int NextRequestAt;

			public bool Requested;
		}

		private class PodiumDisplayState
		{
			public Quaternion Point;

			public bool OriginalPlatform;

			public Vector3 Center;

			public Vector3 Normal;

			public Vector3 Minimum;

			public Vector3 Maximum;

			public float Heading;

			public bool Ready;

			public int StartedAt;

			public int Deadline;

			public int NextProbeAt;
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<Mansion, bool> _0024I15_002D0;

			public static Func<Mansion, float> _0024I15_002D1;

			public static Func<Mansion, bool> _0024I91_002D0;

			public static Func<ApartmentClass, bool> _0024I91_002D1;

			public static Func<Mansion, bool> _0024I155_002D0;

			public static Func<Mansion, float> _0024I155_002D1;

			public static Func<Mansion, bool> _0024I159_002D0;

			public static Func<Mansion, float> _0024I159_002D1;

			public static Func<Mansion, bool> _0024I160_002D0;

			public static Func<VehicleClass, bool> _0024I185_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__15_002D0(Mansion x)
			{
				if (x.Enabled)
				{
					return x.IsOwnedByPlayer();
				}
				return false;
			}

			[SpecialName]
			internal float _Lambda_0024__15_002D1(Mansion x)
			{
				return x.Reference.DistanceTo(Helper.PP.Position);
			}

			[SpecialName]
			internal bool _Lambda_0024__91_002D0(Mansion x)
			{
				if (x.Enabled)
				{
					return x.IsOwnedByPlayer();
				}
				return false;
			}

			[SpecialName]
			internal bool _Lambda_0024__91_002D1(ApartmentClass a)
			{
				return a.Owner() == Helper.GetPlayer();
			}

			[SpecialName]
			internal bool _Lambda_0024__155_002D0(Mansion x)
			{
				return x.Enabled;
			}

			[SpecialName]
			internal float _Lambda_0024__155_002D1(Mansion x)
			{
				return Mansion.Dist2D(Helper.PP.Position, x.Reference);
			}

			[SpecialName]
			internal bool _Lambda_0024__159_002D0(Mansion x)
			{
				return x.Enabled;
			}

			[SpecialName]
			internal float _Lambda_0024__159_002D1(Mansion x)
			{
				return Mansion.Dist2D(Helper.PP.Position, x.Reference);
			}

			[SpecialName]
			internal bool _Lambda_0024__160_002D0(Mansion x)
			{
				return x.Enabled;
			}

			[SpecialName]
			internal bool _Lambda_0024__185_002D0(VehicleClass vc)
			{
				return InstalledShowroomVehicle(vc);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__101_002D0
		{
			public Random _0024VB_0024Local_rnd;

			public _Closure_0024__101_002D0(_Closure_0024__101_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_rnd = arg0._0024VB_0024Local_rnd;
				}
			}

			[SpecialName]
			internal int _Lambda_0024__0(string x)
			{
				return _0024VB_0024Local_rnd.Next();
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__160_002D0
		{
			public Vector3 _0024VB_0024Local_p;

			[SpecialName]
			internal float _Lambda_0024__1(Mansion x)
			{
				return Mansion.Dist2D(_0024VB_0024Local_p, x.Reference);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__17_002D0
		{
			public int _0024VB_0024Local_id;

			[SpecialName]
			internal bool _Lambda_0024__0(Mansion x)
			{
				return x.GarageId == _0024VB_0024Local_id;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__180_002D0
		{
			public Mansion _0024VB_0024Local_m;

			internal PodiumModelRequest _0024VB_0024Local_pending;

			[SpecialName]
			internal bool _Lambda_0024__0(VehicleClass x)
			{
				return x.UniqueID == _0024VB_0024Local_m.PodiumUid;
			}

			[SpecialName]
			internal bool _Lambda_0024__1(VehicleClass item)
			{
				return item.UniqueID == _0024VB_0024Local_pending.UID;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__182_002D0
		{
			public int _0024VB_0024Local_uid;

			[SpecialName]
			internal bool _Lambda_0024__0(VehicleClass x)
			{
				return x.UniqueID == _0024VB_0024Local_uid;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__185_002D0
		{
			public Mansion _0024VB_0024Local_m;

			public _Closure_0024__185_002D0(_Closure_0024__185_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_m = arg0._0024VB_0024Local_m;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__1(VehicleClass vc)
			{
				return vc.UniqueID == _0024VB_0024Local_m.PodiumUid;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__30_002D0
		{
			public Ped _0024VB_0024Local_ped;

			public Vector3 _0024VB_0024Local_pos;

			public string _0024VB_0024Local_expectedInteriorType;

			public HashSet<int> _0024VB_0024Local_extraPins;

			public Vehicle _0024VB_0024Local_originVehicle;

			public int _0024VB_0024Local_originSeat;

			public int _0024VB_0024Local_originVehicleHash;

			public IntPtr _0024VB_0024Local_originVehicleAddress;

			public Vector3 _0024VB_0024Local_origin;

			public bool _0024VB_0024Local_originWasInterior;

			public _Closure_0024__30_002D0(_Closure_0024__30_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_ped = arg0._0024VB_0024Local_ped;
					_0024VB_0024Local_pos = arg0._0024VB_0024Local_pos;
					_0024VB_0024Local_expectedInteriorType = arg0._0024VB_0024Local_expectedInteriorType;
					_0024VB_0024Local_extraPins = arg0._0024VB_0024Local_extraPins;
					_0024VB_0024Local_originVehicle = arg0._0024VB_0024Local_originVehicle;
					_0024VB_0024Local_originSeat = arg0._0024VB_0024Local_originSeat;
					_0024VB_0024Local_originVehicleHash = arg0._0024VB_0024Local_originVehicleHash;
					_0024VB_0024Local_originVehicleAddress = arg0._0024VB_0024Local_originVehicleAddress;
					_0024VB_0024Local_origin = arg0._0024VB_0024Local_origin;
					_0024VB_0024Local_originWasInterior = arg0._0024VB_0024Local_originWasInterior;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__1()
			{
				if (VehicleSeatAvailable(_0024VB_0024Local_ped, _0024VB_0024Local_originVehicle, _0024VB_0024Local_originSeat, _0024VB_0024Local_originVehicleHash, _0024VB_0024Local_originVehicleAddress))
				{
					return true;
				}
				int num = ArrivalInteriorAt(_0024VB_0024Local_origin, null);
				if (_0024VB_0024Local_originWasInterior && num == 0)
				{
					return false;
				}
				if (num != 0)
				{
					if (!Function.Call<bool>(Hash.IS_VALID_INTERIOR, num))
					{
						return false;
					}
					if (!Stability.IsPinnedInterior(num))
					{
						Stability.PinInterior(num, _0024VB_0024Local_origin);
					}
					if (!Function.Call<bool>(Hash.IS_INTERIOR_READY, num))
					{
						return false;
					}
				}
				Vector3 floor = Vector3.Zero;
				return TeleportFloorAt(_0024VB_0024Local_ped, _0024VB_0024Local_origin, ref floor);
			}

			[SpecialName]
			internal bool _Lambda_0024__2()
			{
				return ReturnPointReady(_0024VB_0024Local_ped, _0024VB_0024Local_origin, _0024VB_0024Local_originWasInterior, _0024VB_0024Local_originVehicle, _0024VB_0024Local_originSeat, _0024VB_0024Local_originVehicleHash, _0024VB_0024Local_originVehicleAddress);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__30_002D1
		{
			public int _0024VB_0024Local_interiorId;

			public bool _0024VB_0024Local_required;

			public Vector3 _0024VB_0024Local_floor;

			public _Closure_0024__30_002D0 _0024VB_0024NonLocal__0024VB_0024Closure_2;

			public _Closure_0024__30_002D1(_Closure_0024__30_002D1 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_interiorId = arg0._0024VB_0024Local_interiorId;
					_0024VB_0024Local_required = arg0._0024VB_0024Local_required;
					_0024VB_0024Local_floor = arg0._0024VB_0024Local_floor;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				if (!_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_ped.Exists() || _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_ped.IsDead || Game.Player.Character.Handle != _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_ped.Handle)
				{
					return false;
				}
				_0024VB_0024Local_interiorId = ArrivalInteriorAt(_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_pos, _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_expectedInteriorType);
				if (_0024VB_0024Local_required && _0024VB_0024Local_interiorId == 0)
				{
					return false;
				}
				if (_0024VB_0024Local_interiorId != 0)
				{
					if (!Function.Call<bool>(Hash.IS_VALID_INTERIOR, _0024VB_0024Local_interiorId))
					{
						return false;
					}
					if (!Stability.IsPinnedInterior(_0024VB_0024Local_interiorId))
					{
						Stability.PinInterior(_0024VB_0024Local_interiorId, _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_pos);
						_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_extraPins.Add(_0024VB_0024Local_interiorId);
					}
					if (!Function.Call<bool>(Hash.IS_INTERIOR_READY, _0024VB_0024Local_interiorId))
					{
						return false;
					}
				}
				return TeleportFloorAt(_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_ped, _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_pos, ref _0024VB_0024Local_floor);
			}
		}

		public static readonly string[] ElevatorFloorKeys = new string[6] { "Roof", "Upper", "Main", "Lower", "B1", "B2" };

		public static readonly string[] ElevatorFloorNames = new string[6] { "Roof (custom stop)", "Upper floor (custom stop)", "Ground floor (entrance)", "Lower floor (showroom)", "Basement / workshop", "Mansion garage" };

		public static int WorkshopPricePercent = 100;

		public static int BuffMinutes = 30;

		public static int ArmoryCost = 2500;

		private static readonly string[] LuxuryModels = new string[7] { "cognoscenti", "superd", "windsor", "baller4", "schafter3", "xls", "stretch" };

		private static readonly string HeliModel = "volatus";

		private static readonly Random voiceRnd = new Random();

		private static int lastVoiceAt = 0;

		public static UIMenu AssistantMenu;

		public static UIMenu TravelMenu;

		public static UIMenu ExitVehMenu;

		public static UIMenu GymMenu;

		public static UIMenu PodiumMenu;

		public static UIMenu ElevatorMenu;

		public static UIMenu WorkshopPickMenu;

		public static UIMenu ModMenu;

		public static UIMenu ModPerfMenu;

		public static UIMenu ModBodyMenu;

		public static UIMenu ModWheelMenu;

		public static UIMenu ModPaintMenu;

		public static UIMenu ModLightMenu;

		private static UIMenu AiOptionsMenu;

		private static UIMenu GarageServicesMenu;

		private static UIMenu HomeServicesMenu;

		private static readonly string[] AiPersonalityKeys = new string[3] { "angel", "haviland", "og" };

		private static readonly string[] AiVoiceModeKeys = new string[3] { "Original", "Wav", "Off" };

		public static iFruitContact AssistantContact;

		private static Mansion assistMansion;

		private static Mansion pendingAssistantMansion;

		private static int assistantOpenAt;

		private static int assistantInputReleasedAt = -1;

		private static bool assistantShortcutWasDown;

		private static int lastAssistantShortcutAt = -1000;

		private static Mansion elevMansion;

		private static string elevFrom = "";

		private static Mansion podiumMansion;

		private static Mansion workshopMansion;

		internal static UIMenuItem miPerf;

		internal static UIMenuItem miBody;

		internal static UIMenuItem miWheels;

		internal static UIMenuItem miPaint;

		internal static UIMenuItem miLights;

		internal static UIMenuItem miPlate;

		internal static UIMenuItem miRepair;

		internal static UIMenuItem miSave;

		internal static UIMenuItem miDiscard;

		internal static UIMenuItem miReturn;

		private static readonly string[] ChauffeurCars = new string[6] { "stretch", "cognoscenti", "superd", "schafter3", "baller4", "xls" };

		private static readonly string[] ChauffeurPeds = new string[3] { "s_m_m_highsec_01", "s_m_m_highsec_02", "a_m_y_business_01" };

		private static Vehicle chauffeurVeh;

		private static Ped chauffeurPed;

		private static Vector3 chauffeurDest = Vector3.Zero;

		private static bool chauffeurArrived = false;

		private static bool chauffeurWasIn = false;

		private static int chauffeurLeftAt = 0;

		private static int chauffeurNextCheck = 0;

		private static readonly string[] SmallTalkLines = new string[7] { "You look fantastic today.", "The neighbours keep asking who the new owner is.", "Your car collection is the talk of the hills.", "I took the liberty of polishing the trophies.", "Dynasty 8 says this is the best address in Los Santos.", "The pool is heated and the champagne is cold.", "Shall I cancel all your appointments? Consider it done." };

		private static readonly Random talkRnd = new Random();

		private static int strengthBuffUntil = 0;

		private static int yogaBuffUntil = 0;

		private static Ped workoutPed;

		private static eOwner workoutOwner;

		private static Vector3 workoutOrigin;

		private static string workoutScenario;

		private static string workoutKind;

		private static int workoutStartedAt;

		private static Mansion houseInside = null;

		private static int houseHintAt = -600000;

		private static int contextDownAt = 0;

		public static Keys MansionMenuKey = Keys.I;

		private static bool mansionMenuShortcutWasDown;

		public static Keys AssistantKey = Keys.PageDown;

		public static bool AssistantKeyRequested = false;

		private static readonly Dictionary<Mansion, int> podiumRetryAt = new Dictionary<Mansion, int>();

		private static readonly Dictionary<Mansion, PodiumModelRequest> podiumModelRequests = new Dictionary<Mansion, PodiumModelRequest>();

		private static readonly Dictionary<Mansion, string> podiumFailure = new Dictionary<Mansion, string>();

		private static readonly Dictionary<Mansion, PodiumDisplayState> podiumDisplays = new Dictionary<Mansion, PodiumDisplayState>();

		public static bool IsWorkoutActive => (object)workoutPed != null;

		public static string FloorName(string key)
		{
			int num = Array.IndexOf(ElevatorFloorKeys, key);
			if (num < 0)
			{
				return key;
			}
			return ElevatorFloorNames[num];
		}

		public static bool IsSet(Quaternion q)
		{
			if (q.X == 0f)
			{
				return q.Y != 0f;
			}
			return true;
		}

		public static float HeadingDiff(float a, float b)
		{
			float num = (a - b) % 360f;
			if (num < 0f)
			{
				num += 360f;
			}
			if (!(num > 180f))
			{
				return num;
			}
			return 360f - num;
		}

		private static bool Help(string text)
		{
			return Helper.ShowPersistentHelp(text);
		}

		private static UIMenu NewMenu(string subtitle)
		{
			UIMenu uIMenu = new UIMenu("", subtitle, new Point(0, -107));
			uIMenu.SetBannerType(Helper.MenuBanner);
			uIMenu.MouseEdgeEnabled = false;
			Helper.MenuPool.Add(uIMenu);
			return uIMenu;
		}

		private static void OpenChild(UIMenu parent, UIMenu child)
		{
			child.ParentMenu = parent;
			if (parent != null)
			{
				parent.Visible = false;
			}
			child.RefreshIndex();
			child.Visible = true;
		}

		private static bool TryPayMansion(int amount)
		{
			if (amount <= 0)
			{
				return true;
			}
			eOwner player = Helper.GetPlayer();
			if ((uint)player <= 2u)
			{
				if (Game.Player.Money < amount)
				{
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "ERROR", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
					GTA.UI.Screen.ShowSubtitle(Game.GetLocalizedString("MP_REP_PROP_4"), 4000);
					return false;
				}
				Game.Player.Money = checked(Game.Player.Money - amount);
			}
			return true;
		}

		public static Mansion NearestOwnedMansion()
		{
			return Mansions.MansionList.Where((_Closure_0024__._0024I15_002D0 != null) ? _Closure_0024__._0024I15_002D0 : (_Closure_0024__._0024I15_002D0 = _Closure_0024__._0024I._Lambda_0024__15_002D0)).OrderBy((_Closure_0024__._0024I15_002D1 != null) ? _Closure_0024__._0024I15_002D1 : (_Closure_0024__._0024I15_002D1 = _Closure_0024__._0024I._Lambda_0024__15_002D1)).FirstOrDefault();
		}

		public static bool AtMansion(Mansion m)
		{
			if (m != null)
			{
				if (MansionNativeGarage.ActiveMansion != m)
				{
					return Helper.PP.Position.DistanceTo(m.Reference) < 150f;
				}
				return true;
			}
			return false;
		}

		public static Mansion MansionForGarageId(int id)
		{
			_Closure_0024__17_002D0 closure_0024__17_002D = new _Closure_0024__17_002D0();
			closure_0024__17_002D._0024VB_0024Local_id = id;
			return Mansions.MansionList.FirstOrDefault(closure_0024__17_002D._Lambda_0024__0);
		}

		public static bool IsMansionGarageId(int id)
		{
			return MansionForGarageId(id) != null;
		}

		public static int PodiumUidFor(int aptId)
		{
			return MansionForGarageId(aptId)?.PodiumUid ?? 0;
		}

		public static bool IsVehicleBusy(Mansion m, int uid)
		{
			if (m == null || uid == 0)
			{
				return false;
			}
			if (m.PodiumUid == uid)
			{
				return true;
			}
			if ((object)m.WorkshopVeh != null && m.WorkshopVeh.Exists() && m.WorkshopUid == uid)
			{
				return true;
			}
			return false;
		}

		private static string AssistantName(Mansion m)
		{
			string assistant = m.Assistant;
			if (Operators.CompareString(assistant, "og", false) != 0)
			{
				if (Operators.CompareString(assistant, "haviland", false) == 0)
				{
					return "Haviland";
				}
				return "Angel";
			}
			return "OG";
		}

		private static void Say(Mansion m, string text, string voiceEvent = null)
		{
			Notification.Show(string.Format("~b~{0}~w~: {1}", (m == null) ? "Assistant" : AssistantName(m), text));
			if (voiceEvent != null)
			{
				Voice(m, voiceEvent);
			}
		}

		public static void Voice(Mansion m, string voiceEvent)
		{
			try
			{
				if (m == null)
				{
					return;
				}
				string left = MansionOriginalVoice.NormalizeMode(MansionOriginalVoice.VoiceMode);
				if (Operators.CompareString(left, "Off", false) == 0)
				{
					return;
				}
				if (Operators.CompareString(left, "Original", false) == 0)
				{
					MansionOriginalVoice.TryPlay(m, voiceEvent);
				}
				else
				{
					if (checked(Game.GameTime - lastVoiceAt) < 1500)
					{
						return;
					}
					string path = Path.Combine(Helper.soundPath, "Assistant", m.Assistant);
					if (Directory.Exists(path))
					{
						string[] files = Directory.GetFiles(path, voiceEvent + "*.wav");
						if (files.Length != 0)
						{
							string obj = files[voiceRnd.Next(files.Length)];
							lastVoiceAt = Game.GameTime;
							Helper.Play(new Speech(obj.Substring(Helper.soundPath.Length)));
						}
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Assistant voice {voiceEvent}: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
		}

		private static void LeaveGarageState()
		{
			try
			{
				MansionNativeGarage.Clear();
				if (TenCarGarage.Apartment != null)
				{
					TenCarGarage.Clear();
					TenCarGarage.Apartment = null;
				}
				if (SixCarGarage.Apartment != null)
				{
					SixCarGarage.Clear();
					SixCarGarage.Apartment = null;
				}
				if (TwoCarGarage.Apartment != null)
				{
					TwoCarGarage.Clear();
					TwoCarGarage.Apartment = null;
				}
				ApartmentResidence.Leave();
				LowEndApartment.Apartment = null;
				MediumEndApartment.Apartment = null;
				HighEndApartment.Building = null;
				Helper.HideHud = false;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"LeaveGarageState: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
		}

		public static bool TeleportFloorAcceptable(float requestedZ, float floorZ, float normalZ)
		{
			if (float.IsNaN(requestedZ) || float.IsInfinity(requestedZ) || float.IsNaN(floorZ) || float.IsInfinity(floorZ) || float.IsNaN(normalZ) || float.IsInfinity(normalZ))
			{
				return false;
			}
			float num = floorZ - requestedZ;
			return normalZ >= 0.8f && num >= -1.8f && num <= 0.75f;
		}

		private static bool TeleportFloorAt(Ped ped, Vector3 requested, ref Vector3 floor)
		{
			int num = Function.Call<int>(Hash.START_EXPENSIVE_SYNCHRONOUS_SHAPE_TEST_LOS_PROBE, requested.X, requested.Y, requested.Z + 0.75f, requested.X, requested.Y, requested.Z - 1.8f, 81, ped?.Handle ?? 0, 4);
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			OutputArgument outputArgument3 = new OutputArgument();
			OutputArgument outputArgument4 = new OutputArgument();
			if (Function.Call<int>(Hash.GET_SHAPE_TEST_RESULT, num, outputArgument, outputArgument2, outputArgument3, outputArgument4) != 2 || outputArgument.GetResult<int>() == 0)
			{
				return false;
			}
			Vector3 result = outputArgument2.GetResult<Vector3>();
			Vector3 result2 = outputArgument3.GetResult<Vector3>();
			if (!PartyPlacement.FinitePoint(result) || !TeleportFloorAcceptable(requested.Z, result.Z, result2.Z))
			{
				return false;
			}
			floor = result;
			return true;
		}

		private static int ArrivalInteriorAt(Vector3 position, string expectedType)
		{
			if (!string.IsNullOrEmpty(expectedType))
			{
				return Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS_WITH_TYPE, position.X, position.Y, position.Z + 0.65f, expectedType);
			}
			return Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position.X, position.Y, position.Z + 0.65f);
		}

		public static bool TeleportPlayer(Vector3 pos, float heading, bool fade = true, string expectedInteriorType = null, DestinationStreaming.SceneLease preparedScene = null)
		{
			_Closure_0024__30_002D0 arg = new _Closure_0024__30_002D0(null);
			arg._0024VB_0024Local_pos = pos;
			arg._0024VB_0024Local_expectedInteriorType = expectedInteriorType;
			arg._0024VB_0024Local_ped = Helper.PP;
			if ((object)arg._0024VB_0024Local_ped == null || !arg._0024VB_0024Local_ped.Exists() || !PartyPlacement.FinitePoint(arg._0024VB_0024Local_pos) || float.IsNaN(heading) || float.IsInfinity(heading))
			{
				return false;
			}
			arg._0024VB_0024Local_origin = arg._0024VB_0024Local_ped.Position;
			float heading2 = arg._0024VB_0024Local_ped.Heading;
			bool flag = DestinationStreaming.OriginalPlayerFreeze(arg._0024VB_0024Local_ped, arg._0024VB_0024Local_ped.IsPositionFrozen);
			arg._0024VB_0024Local_originVehicle = (arg._0024VB_0024Local_ped.IsInVehicle() ? arg._0024VB_0024Local_ped.CurrentVehicle : null);
			bool flag2 = (object)arg._0024VB_0024Local_originVehicle != null && arg._0024VB_0024Local_originVehicle.Exists() && DestinationStreaming.OriginalEntityFreeze(arg._0024VB_0024Local_originVehicle, arg._0024VB_0024Local_originVehicle.IsPositionFrozen);
			arg._0024VB_0024Local_originVehicleHash = (((object)arg._0024VB_0024Local_originVehicle != null) ? arg._0024VB_0024Local_originVehicle.Model.Hash : 0);
			arg._0024VB_0024Local_originVehicleAddress = (((object)arg._0024VB_0024Local_originVehicle == null) ? IntPtr.Zero : arg._0024VB_0024Local_originVehicle.MemoryAddress);
			arg._0024VB_0024Local_originSeat = -2;
			checked
			{
				if ((object)arg._0024VB_0024Local_originVehicle != null && arg._0024VB_0024Local_originVehicle.Exists())
				{
					int num = Math.Min(Function.Call<int>(Hash.GET_VEHICLE_MAX_NUMBER_OF_PASSENGERS, arg._0024VB_0024Local_originVehicle.Handle) - 1, 15);
					for (int i = -1; i <= num; i++)
					{
						if (Function.Call<int>(Hash.GET_PED_IN_VEHICLE_SEAT, arg._0024VB_0024Local_originVehicle.Handle, i, false) == arg._0024VB_0024Local_ped.Handle)
						{
							arg._0024VB_0024Local_originSeat = i;
							break;
						}
					}
				}
				arg._0024VB_0024Local_originWasInterior = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, arg._0024VB_0024Local_ped.Handle) != 0;
				bool flag3 = false;
				bool flag4 = false;
				bool flag5 = false;
				DestinationStreaming.SceneLease sceneLease = null;
				arg._0024VB_0024Local_extraPins = new HashSet<int>();
				Stopwatch stopwatch = Stopwatch.StartNew();
				bool flag6 = default(bool);
				try
				{
					_Closure_0024__30_002D1 arg2 = new _Closure_0024__30_002D1(null);
					arg2._0024VB_0024NonLocal__0024VB_0024Closure_2 = arg;
					CancelWorkout();
					if ((object)arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_originVehicle != null && arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_originVehicle.Exists())
					{
						Function.Call(Hash.FREEZE_ENTITY_POSITION, arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_originVehicle.Handle, true);
					}
					else
					{
						Function.Call(Hash.FREEZE_ENTITY_POSITION, arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_ped.Handle, true);
					}
					sceneLease = DestinationStreaming.Acquire(arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_pos);
					if (preparedScene != null)
					{
						preparedScene.Dispose();
						preparedScene = null;
					}
					if (sceneLease == null)
					{
						Notification.Show("A scene or camera is already loading. Please try travelling again in a moment.");
						return false;
					}
					flag5 = true;
					if (fade)
					{
						flag6 = true;
						Helper.FadeScreen(1, 500);
					}
					arg2._0024VB_0024Local_floor = Vector3.Zero;
					arg2._0024VB_0024Local_interiorId = 0;
					arg2._0024VB_0024Local_required = !string.IsNullOrEmpty(arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_expectedInteriorType);
					Func<bool> targetReady = arg2._Lambda_0024__0;
					if (!sceneLease.WaitUntilReady(targetReady) || !sceneLease.CanContinue)
					{
						Notification.Show("The destination did not finish loading safely. You have been kept at your previous position.");
						return false;
					}
					MansionNativeGarage.ReleasePlayerRoomForTravel();
					if ((object)arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_originVehicle != null)
					{
						arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_ped.Task.WarpOutOfVehicle(arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_originVehicle);
						Stopwatch stopwatch2 = Stopwatch.StartNew();
						while (arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_ped.IsInVehicle() && stopwatch2.ElapsedMilliseconds < 1000 && sceneLease.CanContinue)
						{
							Script.Wait(0);
						}
						if (arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_ped.IsInVehicle() || !sceneLease.CanContinue)
						{
							return false;
						}
					}
					Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_ped.Handle);
					Function.Call(Hash.FREEZE_ENTITY_POSITION, arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_ped.Handle, true);
					Function.Call(Hash.SET_ENTITY_COORDS, arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_ped.Handle, arg2._0024VB_0024Local_floor.X, arg2._0024VB_0024Local_floor.Y, arg2._0024VB_0024Local_floor.Z, false, false, false, false);
					Function.Call(Hash.SET_ENTITY_HEADING, arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_ped.Handle, heading);
					Function.Call(Hash.SET_ENTITY_VELOCITY, arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_ped.Handle, 0f, 0f, 0f);
					flag3 = true;
					int num2 = 0;
					int num3 = -1;
					while (arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_ped.Exists() && !arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_ped.IsDead && Game.Player.Character.Handle == arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_ped.Handle && sceneLease.CanContinue)
					{
						Function.Call(Hash.REQUEST_COLLISION_AT_COORD, arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_pos.X, arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_pos.Y, arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_pos.Z);
						arg2._0024VB_0024Local_interiorId = ArrivalInteriorAt(arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_pos, arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_expectedInteriorType);
						bool flag7 = arg2._0024VB_0024Local_interiorId != 0 && Function.Call<bool>(Hash.IS_INTERIOR_READY, arg2._0024VB_0024Local_interiorId);
						bool flag8 = Function.Call<bool>(Hash.HAS_COLLISION_LOADED_AROUND_ENTITY, arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_ped.Handle);
						Vector3 floor = Vector3.Zero;
						bool flag9 = TeleportFloorAt(arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_ped, arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_pos, ref floor) && floor.DistanceToSquared(arg2._0024VB_0024Local_floor) <= 0.01f && DestinationStreaming.PedAtDestination(arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_ped.Position, floor);
						bool sceneLoaded = sceneLease.SceneLoaded;
						int frameCount = Game.FrameCount;
						if (frameCount != num3)
						{
							num3 = frameCount;
							num2 = ((sceneLoaded && flag8 && flag9 && (!arg2._0024VB_0024Local_required || arg2._0024VB_0024Local_interiorId != 0) && (arg2._0024VB_0024Local_interiorId == 0 || flag7)) ? (num2 + 1) : 0);
						}
						if (DestinationStreaming.ReadinessAcceptable(sceneLoaded, flag8, arg2._0024VB_0024Local_required, arg2._0024VB_0024Local_interiorId, flag7, flag9, num2))
						{
							flag4 = true;
							MansionNativeGarage.RefreshPlayerRoomAfterTravel();
							DestinationStreaming.CompletePlayerRecovery(arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_ped);
							break;
						}
						Script.Wait(0);
					}
					if (!flag4)
					{
						Logger.Log($"TeleportPlayer: destination did not become stable at {arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_pos}; restoring previous position.");
						Notification.Show("The destination did not finish loading safely. You have been kept at your previous position.");
					}
					else
					{
						Logger.Log($"TeleportPlayer: scene, interior {arg2._0024VB_0024Local_interiorId}, collision and floor ready at {arg2._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_pos}; elapsed={sceneLease.ElapsedMilliseconds} ms");
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					flag4 = false;
					Logger.Log($"TeleportPlayer: {ex2.Message}");
					ProjectData.ClearProjectError();
				}
				finally
				{
					if (preparedScene != null)
					{
						preparedScene.Dispose();
						preparedScene = null;
					}
					if (!flag4 && sceneLease != null)
					{
						DestinationStreaming.CancelOwned();
					}
					sceneLease?.Dispose();
					if ((object)arg._0024VB_0024Local_ped != null && arg._0024VB_0024Local_ped.Exists())
					{
						if (!flag4 && flag5 && !arg._0024VB_0024Local_ped.IsDead && Game.Player.Character.Handle == arg._0024VB_0024Local_ped.Handle)
						{
							bool flag10 = false;
							try
							{
								using (DestinationStreaming.SceneLease sceneLease2 = DestinationStreaming.Acquire(arg._0024VB_0024Local_origin, 120f, 7000))
								{
									Func<bool> targetReady2 = arg._Lambda_0024__1;
									bool num4 = sceneLease2?.WaitUntilReady(targetReady2) ?? false;
									Function.Call(Hash.FREEZE_ENTITY_POSITION, arg._0024VB_0024Local_ped.Handle, true);
									if (flag3)
									{
										Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, arg._0024VB_0024Local_ped.Handle, arg._0024VB_0024Local_origin.X, arg._0024VB_0024Local_origin.Y, arg._0024VB_0024Local_origin.Z, false, false, false);
										Function.Call(Hash.SET_ENTITY_HEADING, arg._0024VB_0024Local_ped.Handle, heading2);
										Function.Call(Hash.SET_ENTITY_VELOCITY, arg._0024VB_0024Local_ped.Handle, 0f, 0f, 0f);
									}
									if (arg._0024VB_0024Local_originSeat >= -1 && (object)arg._0024VB_0024Local_originVehicle != null && arg._0024VB_0024Local_originVehicle.Exists())
									{
										int num5 = Function.Call<int>(Hash.GET_PED_IN_VEHICLE_SEAT, arg._0024VB_0024Local_originVehicle.Handle, arg._0024VB_0024Local_originSeat, false);
										if (num5 == 0 || num5 == arg._0024VB_0024Local_ped.Handle)
										{
											Function.Call(Hash.SET_PED_INTO_VEHICLE, arg._0024VB_0024Local_ped.Handle, arg._0024VB_0024Local_originVehicle.Handle, arg._0024VB_0024Local_originSeat);
										}
									}
									if (num4)
									{
										int num6 = 0;
										int num7 = -1;
										while (sceneLease2.CanContinue)
										{
											bool flag11 = sceneLease2.SceneLoaded && ReturnPointReady(arg._0024VB_0024Local_ped, arg._0024VB_0024Local_origin, arg._0024VB_0024Local_originWasInterior, arg._0024VB_0024Local_originVehicle, arg._0024VB_0024Local_originSeat, arg._0024VB_0024Local_originVehicleHash, arg._0024VB_0024Local_originVehicleAddress);
											int frameCount2 = Game.FrameCount;
											if (frameCount2 != num7)
											{
												num7 = frameCount2;
												num6 = (flag11 ? (num6 + 1) : 0);
											}
											if (num6 >= 2)
											{
												flag10 = true;
												break;
											}
											Script.Wait(0);
										}
									}
								}
							}
							catch (Exception ex3)
							{
								ProjectData.SetProjectError(ex3);
								Exception ex4 = ex3;
								Logger.Log("Teleport origin recovery: " + ex4.Message);
								ProjectData.ClearProjectError();
							}
							if (flag10)
							{
								MansionNativeGarage.RefreshPlayerRoomAfterTravel();
							}
							else
							{
								DestinationStreaming.HoldPlayerUntilReady(arg._0024VB_0024Local_ped, flag, arg._0024VB_0024Local_origin, arg._Lambda_0024__2, fade || GTA.UI.Screen.IsFadedOut || GTA.UI.Screen.IsFadingOut, arg._0024VB_0024Local_originVehicle, flag2);
							}
						}
						if (!DestinationStreaming.IsPlayerSafetyHeld(arg._0024VB_0024Local_ped))
						{
							Function.Call(Hash.FREEZE_ENTITY_POSITION, arg._0024VB_0024Local_ped.Handle, flag);
						}
					}
					if ((object)arg._0024VB_0024Local_originVehicle != null && arg._0024VB_0024Local_originVehicle.Exists() && !DestinationStreaming.IsPlayerSafetyHeld(arg._0024VB_0024Local_originVehicle))
					{
						Function.Call(Hash.FREEZE_ENTITY_POSITION, arg._0024VB_0024Local_originVehicle.Handle, flag2);
					}
					if (!flag4)
					{
						foreach (int _0024VB_0024Local_extraPin in arg._0024VB_0024Local_extraPins)
						{
							Stability.UnpinInterior(_0024VB_0024Local_extraPin);
						}
					}
					if (flag6)
					{
						FadeInAfterTravel();
					}
					Logger.Log("Travel readiness: arrived=" + flag4 + "; elapsedMs=" + stopwatch.ElapsedMilliseconds);
				}
				return flag4;
			}
		}

		private static bool VehicleSeatAvailable(Ped ped, Vehicle vehicle, int seat, int modelHash, IntPtr address)
		{
			if ((object)ped == null || (object)vehicle == null || !vehicle.Exists() || seat < -1 || vehicle.Model.Hash != modelHash || address == IntPtr.Zero || vehicle.MemoryAddress != address)
			{
				return false;
			}
			int num = Function.Call<int>(Hash.GET_PED_IN_VEHICLE_SEAT, vehicle.Handle, seat, false);
			return num == 0 || num == ped.Handle;
		}

		private static bool ReturnPointReady(Ped ped, Vector3 origin, bool interiorRequired, Vehicle vehicle, int seat, int vehicleHash, IntPtr vehicleAddress)
		{
			if ((object)ped == null || !ped.Exists() || ped.IsDead || Game.Player.Character.Handle != ped.Handle)
			{
				return false;
			}
			Function.Call(Hash.REQUEST_COLLISION_AT_COORD, origin.X, origin.Y, origin.Z);
			if (VehicleSeatAvailable(ped, vehicle, seat, vehicleHash, vehicleAddress) && ped.IsInVehicle(vehicle) && Function.Call<int>(Hash.GET_PED_IN_VEHICLE_SEAT, vehicle.Handle, seat, false) == ped.Handle)
			{
				return Function.Call<bool>(Hash.HAS_COLLISION_LOADED_AROUND_ENTITY, vehicle.Handle) && Function.Call<bool>(Hash.HAS_COLLISION_LOADED_AROUND_ENTITY, ped.Handle);
			}
			int num = ArrivalInteriorAt(origin, null);
			if (interiorRequired && num == 0)
			{
				return false;
			}
			if (num != 0)
			{
				if (!Function.Call<bool>(Hash.IS_VALID_INTERIOR, num))
				{
					return false;
				}
				if (!Stability.IsPinnedInterior(num))
				{
					Stability.PinInterior(num, origin);
				}
				if (!Function.Call<bool>(Hash.IS_INTERIOR_READY, num))
				{
					return false;
				}
			}
			Vector3 floor = Vector3.Zero;
			return Function.Call<bool>(Hash.HAS_COLLISION_LOADED_AROUND_ENTITY, ped.Handle) && TeleportFloorAt(ped, origin, ref floor) && DestinationStreaming.PedAtDestination(ped.Position, floor);
		}

		private static void FadeInAfterTravel(int milliseconds = 500)
		{
			if (!DestinationStreaming.IsPlayerSafetyHeld(Helper.PP))
			{
				Helper.FadeScreen(0, milliseconds);
			}
		}

		public static void LoadMansionFeatures()
		{
			AssistantMenu = NewMenu("MANSION ASSISTANT");
			AiOptionsMenu = NewMenu("AI OPTIONS");
			GarageServicesMenu = NewMenu("GARAGE & HELIPAD");
			HomeServicesMenu = NewMenu("MANSION SERVICES");
			TravelMenu = NewMenu("FAST TRAVEL");
			ExitVehMenu = NewMenu("PERSONAL VEHICLE");
			GymMenu = NewMenu("GYM");
			PodiumMenu = NewMenu("PODIUM");
			ElevatorMenu = NewMenu("ELEVATOR");
			WorkshopPickMenu = NewMenu("WORKSHOP");
			ModMenu = NewMenu("VEHICLE WORKSHOP");
			ModPerfMenu = NewMenu("PERFORMANCE");
			ModBodyMenu = NewMenu("BODY");
			ModWheelMenu = NewMenu("WHEELS");
			ModPaintMenu = NewMenu("RESPRAY");
			ModLightMenu = NewMenu("LIGHTS");
			AssistantMenu.OnItemSelect += AssistantMenu_OnItemSelect;
			GarageServicesMenu.OnItemSelect += AssistantMenu_OnItemSelect;
			HomeServicesMenu.OnItemSelect += AssistantMenu_OnItemSelect;
			AiOptionsMenu.OnItemSelect += AssistantMenu_OnItemSelect;
			AiOptionsMenu.OnListChange += AiOptionsMenu_OnListChange;
			TravelMenu.OnItemSelect += TravelMenu_OnItemSelect;
			ExitVehMenu.OnItemSelect += ExitVehMenu_OnItemSelect;
			GymMenu.OnItemSelect += GymMenu_OnItemSelect;
			PodiumMenu.OnItemSelect += PodiumMenu_OnItemSelect;
			PodiumMenu.OnCheckboxChange += PodiumMenu_OnCheckboxChange;
			ElevatorMenu.OnItemSelect += ElevatorMenu_OnItemSelect;
			WorkshopPickMenu.OnItemSelect += WorkshopPickMenu_OnItemSelect;
			miPerf = new UIMenuItem("Performance", "Engine, brakes, transmission, suspension, armour, turbo.");
			miBody = new UIMenuItem("Body parts", "Spoilers, bumpers, skirts, exhaust, hood, roof, interior, livery ...");
			miWheels = new UIMenuItem("Wheels", "Wheel type, rims, custom tyres, bulletproof tyres, tyre smoke.");
			miPaint = new UIMenuItem("Respray", "Primary, secondary, pearlescent and wheel colour, window tint, plate style.");
			miLights = new UIMenuItem("Lights", "Xenon headlights and neon kits.");
			miPlate = new UIMenuItem("Change plate text", "Enter a new number plate.")
			{
				Tag = "PlateText"
			};
			miRepair = new UIMenuItem("Repair & clean", "Fixes all damage and washes the vehicle.")
			{
				Tag = "Repair"
			};
			miSave = new UIMenuItem("Pay & save", "Pay for the changes and save them to the vehicle in the garage.")
			{
				Tag = "Save"
			};
			miDiscard = new UIMenuItem("Discard changes", "Reverts the vehicle to its saved state.")
			{
				Tag = "Discard"
			};
			miReturn = new UIMenuItem("Send the vehicle back to the garage", "Unsaved changes are lost.")
			{
				Tag = "Return"
			};
			UIMenuItem[] array = new UIMenuItem[9] { miPerf, miBody, miWheels, miPaint, miLights, miPlate, miSave, miDiscard, miReturn };
			foreach (UIMenuItem item in array)
			{
				ModMenu.AddItem(item);
			}
			ModMenu.BindMenuToItem(ModPerfMenu, miPerf);
			ModMenu.BindMenuToItem(ModBodyMenu, miBody);
			ModMenu.BindMenuToItem(ModWheelMenu, miWheels);
			ModMenu.BindMenuToItem(ModPaintMenu, miPaint);
			ModMenu.BindMenuToItem(ModLightMenu, miLights);
			ModMenu.RefreshIndex();
			ModMenu.OnItemSelect += MansionWorkshop.ModMenu_OnItemSelect;
			UIMenu[] array2 = new UIMenu[5] { ModPerfMenu, ModBodyMenu, ModWheelMenu, ModPaintMenu, ModLightMenu };
			foreach (UIMenu obj in array2)
			{
				obj.OnListChange += MansionWorkshop.ModList_Changed;
				obj.OnCheckboxChange += MansionWorkshop.ModCheck_Changed;
			}
			try
			{
				AssistantContact = new iFruitContact("Mansion Assistant");
				iFruitContact assistantContact = AssistantContact;
				assistantContact.DialTimeout = 2500;
				assistantContact.Active = true;
				assistantContact.Icon = ContactIcon.Human;
				Mechanic.iFruit.Contacts.Add(AssistantContact);
				AssistantContact.Answered += AssistantContact_Answered;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Mansion assistant contact: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
		}

		public static void ForgetPresentationForStoryLoad()
		{
			podiumDisplays.Clear();
			podiumModelRequests.Clear();
			podiumRetryAt.Clear();
			podiumFailure.Clear();
			podiumMansion = null;
			workshopMansion = null;
			MansionWorkshop.modM = null;
			MansionWorkshop.modVeh = null;
			MansionShowroomPlacement.ForgetForStoryLoad();
			MansionWorkshopTravel.ForgetForStoryLoad();
		}

		public static void MansionFeaturesCleanup()
		{
			foreach (Mansion mansion in Mansions.MansionList)
			{
				DespawnPodium(mansion);
				RemoveWorkshopVehicle(mansion);
			}
			pendingAssistantMansion = null;
			AssistantKeyRequested = false;
			mansionMenuShortcutWasDown = false;
			CancelWorkout();
			MansionNativeGarage.Clear();
			MansionVehicleShop.Cleanup();
			MansionWorkshopTravel.Cleanup();
			MansionOriginalVoice.Cleanup();
			MansionAmenities.Cleanup();
			MansionLeisure.Cleanup();
			MansionSecurity.Cleanup();
			MansionTrophyCases.Cleanup();
			PrivateDancerPreview.Close();
			DismissChauffeur(true);
			MansionVault.VaultCleanup();
			ResetBuffs();
		}

		public static bool FeaturesTick(Mansion m)
		{
			TickPodium(m);
			UpdatePodiumMenuLabels(m);
			if (IsWorkoutActive || PartyService.IsPlayerActionActive || PartyPlayerActions.IsKissing || ApartmentHomeServices.IsBusy || MansionBarber.IsBusy)
			{
				return true;
			}
			if (pendingAssistantMansion != null)
			{
				return true;
			}
			if (Helper.MenuPool.IsAnyMenuOpen())
			{
				return false;
			}
			HouseKeyTick(m);
			if (pendingAssistantMansion != null)
			{
				return true;
			}
			if (Helper.MenuPool.IsAnyMenuOpen() || !m.IsOwnedByPlayer() || !m.IplsLoaded)
			{
				return false;
			}
			if (MansionLeisure.PetInteractionTick(m))
			{
				return true;
			}
			if (ElevatorTick(m))
			{
				return true;
			}
			if (WorkshopTick(m))
			{
				return true;
			}
			if (MansionVault.CashTick(m))
			{
				return true;
			}
			if (MansionAmenities.Tick(m))
			{
				return true;
			}
			if (StationTick(m))
			{
				return true;
			}
			if (PanelTick(m))
			{
				return true;
			}
			return false;
		}

		public static bool PodiumReferenceNeedsRetirement(bool referencePresent, bool live, int liveUid, int selectedUid)
		{
			if (referencePresent)
			{
				if (live)
				{
					return liveUid != selectedUid;
				}
				return true;
			}
			return false;
		}

		public static void FeaturesSlowTick(Mansion m)
		{
			try
			{
				if (!m.NearBy || !m.IplsLoaded || !m.SetsApplied || !m.IsOwnedByPlayer() || m.PodiumUid == 0 || !IsSet(m.PodiumPos) || !(Helper.PP.Position.DistanceTo(Helper.ToVector3(m.PodiumPos)) < 90f))
				{
					DespawnPodium(m);
				}
				else
				{
					if (PodiumReferenceNeedsRetirement((object)m.PodiumVeh != null, (object)m.PodiumVeh != null && m.PodiumVeh.Exists(), m.PodiumVehUid, m.PodiumUid))
					{
						DespawnPodium(m);
					}
					if ((object)m.PodiumVeh == null || !m.PodiumVeh.Exists() || m.PodiumVehUid != m.PodiumUid)
					{
						SpawnPodium(m);
					}
				}
				MansionVault.CashSlowTick(m);
				if ((object)m.WorkshopVeh != null && (!m.WorkshopVeh.Exists() || !m.IplsLoaded || Helper.PP.Position.DistanceTo(Helper.ToVector3(m.WorkshopPos)) > 60f))
				{
					RemoveWorkshopVehicle(m);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"FeaturesSlowTick {m.Name}: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
		}

		private static void AssistantContact_Answered(iFruitContact contact)
		{
			try
			{
				Mechanic.iFruit.Close(1500);
				Mansion mansion = NearestOwnedMansion();
				if (mansion == null)
				{
					Notification.Show("~b~Mansion Assistant~w~: You don't own a mansion yet. Dynasty 8 has three of them for sale.");
				}
				else
				{
					OpenAssistant(mansion);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Assistant: {ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		public static void OpenAssistant(Mansion m)
		{
			if (m != null && m.Enabled && m.IsOwnedByPlayer() && pendingAssistantMansion != m && (AssistantMenu == null || !AssistantMenu.Visible || assistMansion != m))
			{
				pendingAssistantMansion = m;
				assistantOpenAt = checked(Game.GameTime + 150);
				assistantInputReleasedAt = -1;
				contextDownAt = 0;
				Logger.Log($"Assistant queued: {m.Name}");
			}
		}

		public static bool AssistantRequestPending()
		{
			return pendingAssistantMansion != null;
		}

		private static bool AssistantOpeningInputDown()
		{
			if (Game.IsKeyPressed(AssistantKey))
			{
				return true;
			}
			GTA.Control[] array = new GTA.Control[5]
			{
				GTA.Control.Context,
				GTA.Control.FrontendAccept,
				GTA.Control.PhoneSelect,
				GTA.Control.PhoneCancel,
				GTA.Control.FrontendPause
			};
			foreach (GTA.Control control in array)
			{
				if (Game.IsControlPressed(control) || Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, 0, (int)control))
				{
					return true;
				}
			}
			return false;
		}

		public static void AssistantMenuTick()
		{
			if (pendingAssistantMansion == null)
			{
				return;
			}
			Mansion mansion = pendingAssistantMansion;
			if (!mansion.Enabled || !mansion.IsOwnedByPlayer())
			{
				pendingAssistantMansion = null;
				return;
			}
			if (Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE) || Game.IsLoading || AssistantOpeningInputDown())
			{
				assistantInputReleasedAt = -1;
				return;
			}
			if (assistantInputReleasedAt < 0)
			{
				assistantInputReleasedAt = Game.GameTime;
			}
			if (Game.GameTime < assistantOpenAt || checked(Game.GameTime - assistantInputReleasedAt) < 100)
			{
				return;
			}
			pendingAssistantMansion = null;
			try
			{
				if (AssistantMenu == null)
				{
					throw new InvalidOperationException("Assistant menu was not initialized.");
				}
				assistMansion = mansion;
				BuildAssistantMenu(mansion);
				Helper.MenuPool.CloseAllMenus();
				AssistantMenu.ParentMenu = null;
				AssistantMenu.RefreshIndex();
				AssistantMenu.Visible = true;
				Logger.Log($"Assistant opened: {mansion.Name}, items={AssistantMenu.MenuItems.Count}, visible={AssistantMenu.Visible}");
				Voice(mansion, "hello");
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Assistant open: {ex2.Message} {ex2.StackTrace}");
				Notification.Show("~r~Mansion Assistant~w~: menu could not open. Details are in SPA II.log.");
				ProjectData.ClearProjectError();
			}
		}

		private static UIMenuItem Item(string text, string desc, string tag, bool enabled = true, string right = null)
		{
			UIMenuItem uIMenuItem = new UIMenuItem(text, desc)
			{
				Tag = tag,
				Enabled = enabled
			};
			if (right != null)
			{
				uIMenuItem.SetRightLabel(right);
			}
			return uIMenuItem;
		}

		private static void BuildAssistantMenu(Mansion m)
		{
			MansionAutoLayout.ApplyDefaults(m);
			AssistantMenu.Clear();
			AssistantMenu.Subtitle.Caption = $"{AssistantName(m).ToUpper()} - {m.Name.ToUpper()}";
			bool flag = AtMansion(m);
			UIMenu assistantMenu = AssistantMenu;
			assistantMenu.AddItem(Item("Fast Travel", "Travel to one of your owned SPA properties.", "Travel"));
			assistantMenu.AddItem(Item("AI Options", "Choose Angel, Haviland or OG, original game voices, custom WAVs or mute.", "AI"));
			assistantMenu.AddItem(Item("Customize Mansion Look", "Style, wallpaper, colours and decorations.", "Manage", flag));
			assistantMenu.AddItem(Item("Garage & Helipad Management", "Original 20-car garage, showroom, workshop, elevator and vehicle services.", "Vehicles", flag));
			assistantMenu.AddItem(Item("Buy vehicles / 3D preview", "Browse installed GTA Online vehicles, HSW-capable models and add-ons; preview before buying for this mansion garage.", "BuyVehicles", flag));
			assistantMenu.AddItem(Item("Office computer", "Property services and management from your mansion computer.", "Office", flag));
			assistantMenu.AddItem(Item("Kitchen & snacks", "Eat, refill snacks or choose a cold drink.", "Kitchen", flag));
			assistantMenu.AddItem(Item("Manage Pets", "Cat, dog, names, breeds, call, follow and dismiss.", "Pets", flag));
			assistantMenu.AddItem(Item("Request A Private Dancer", "Invite an adult dancer without starting a full party.", "PrivateDancer", flag));
			assistantMenu.AddItem(Item("Champagne Service", "Request a glass of champagne.", "Champagne", flag));
			assistantMenu.AddItem(Item("VIP Heli Service", "A Volatus waits on your original rooftop helipad.", "ExitHeli", flag && IsSet(m.HelipadPos)));
			assistantMenu.AddItem(Item("Host Party", "Party controls: up to 27 guests, DJ, dancing, drinks and smoking.", "Party", flag, PartyService.IsPartyActive ? "ACTIVE" : "PRIVATE"));
			assistantMenu.AddItem(Item("Gym & Yoga", "Training and temporary fitness bonuses.", "Gym", flag));
			assistantMenu.AddItem(Item("Armory, Vault & Small Talk", "Ammunition, armour, vault balance and your assistant.", "Home", flag));
			assistantMenu.AddItem(Item("Mansion Management", "Property appearance, saved vehicles and automatic mansion layout.", "Manage", flag));
			assistantMenu.RefreshIndex();
		}

		private static void BuildAiOptionsMenu(Mansion m)
		{
			AiOptionsMenu.Clear();
			int index = Math.Max(0, Array.IndexOf(AiPersonalityKeys, m.Assistant));
			AiOptionsMenu.AddItem(new UIMenuListItem("Personality", new List<object>(new object[3] { "Angel", "Haviland", "OG" }), index)
			{
				Tag = "Personality"
			});
			string value = MansionOriginalVoice.NormalizeMode(MansionOriginalVoice.VoiceMode);
			int index2 = Math.Max(0, Array.IndexOf(AiVoiceModeKeys, value));
			AiOptionsMenu.AddItem(new UIMenuListItem("Voice", new List<object>(new object[3] { "Original GTA Online", "Custom WAV files", "Off" }), index2)
			{
				Tag = "VoiceMode"
			});
			AiOptionsMenu.AddItem(Item("Play greeting", "Hear the currently selected assistant.", "Greeting"));
			AiOptionsMenu.AddItem(Item("Small talk", "A few words from your assistant.", "Talk"));
			AiOptionsMenu.RefreshIndex();
		}

		private static void AiOptionsMenu_OnListChange(UIMenu sender, UIMenuListItem selectedItem, int newIndex)
		{
			Mansion mansion = assistMansion;
			if (mansion == null || !mansion.IsOwnedByPlayer())
			{
				return;
			}
			string assistant = mansion.Assistant;
			string voiceMode = MansionOriginalVoice.VoiceMode;
			if (Operators.CompareString(Conversions.ToString(selectedItem.Tag), "Personality", false) == 0 && newIndex >= 0 && newIndex < AiPersonalityKeys.Length)
			{
				mansion.Assistant = AiPersonalityKeys[newIndex];
			}
			else
			{
				if (Operators.CompareString(Conversions.ToString(selectedItem.Tag), "VoiceMode", false) != 0 || newIndex < 0 || newIndex >= AiVoiceModeKeys.Length)
				{
					return;
				}
				MansionOriginalVoice.VoiceMode = AiVoiceModeKeys[newIndex];
			}
			if (!Mansions.SaveMansions())
			{
				mansion.Assistant = assistant;
				MansionOriginalVoice.VoiceMode = voiceMode;
				string[] array = ((Operators.CompareString(Conversions.ToString(selectedItem.Tag), "Personality", false) == 0) ? AiPersonalityKeys : AiVoiceModeKeys);
				selectedItem.Index = Math.Max(0, Array.IndexOf(array, (Operators.CompareString(Conversions.ToString(selectedItem.Tag), "Personality", false) == 0) ? assistant : voiceMode));
			}
			else if (Operators.CompareString(mansion.Assistant, assistant, false) != 0)
			{
				Mansions.ApplySets(mansion, true);
			}
		}

		private static void BuildGarageServicesMenu(Mansion m)
		{
			GarageServicesMenu.Clear();
			GarageServicesMenu.AddItem(Item("Enter original mansion garage", "20 saved spaces across both original levels.", "Garage"));
			GarageServicesMenu.AddItem(Item("Showroom (show car)", "Display a saved vehicle or the car you are driving.", "Podium"));
			GarageServicesMenu.AddItem(Item("Buy vehicles / 3D preview", "Installed GTA Online vehicles, HSW-capable models and add-ons. Buy into this mansion's saved garage.", "BuyVehicles"));
			GarageServicesMenu.AddItem(Item("Vehicle workshop", "Travel directly to the mansion workshop, choose a saved car or bring your current vehicle, then tune it.", "Workshop", IsSet(m.WorkshopPos)));
			GarageServicesMenu.AddItem(Item("Elevator / floors", "Original mansion levels and garage.", "Elevator"));
			GarageServicesMenu.AddItem(Item("Leave in a personal vehicle", "Retrieve a stored car at the driveway.", "ExitVeh"));
			GarageServicesMenu.AddItem(Item("Leave in a luxury car", "A luxury car is brought to the driveway.", "ExitLux"));
			GarageServicesMenu.AddItem(Item("Chauffeur", "Your driver takes you to the map waypoint.", "Chauffeur"));
			GarageServicesMenu.AddItem(Item("VIP Heli Service", "Prepare a Volatus on the helipad.", "ExitHeli", IsSet(m.HelipadPos)));
			GarageServicesMenu.RefreshIndex();
		}

		private static void BuildHomeServicesMenu(Mansion m)
		{
			HomeServicesMenu.Clear();
			HomeServicesMenu.AddItem(Item("Armory", "Buy weapons, compatible upgrades, ammunition and body armour.", "Armory"));
			HomeServicesMenu.AddItem(Item("Vault", "Check the story characters' cash balances.", "Vault"));
			HomeServicesMenu.AddItem(Item("Weather & small talk", "Chat with your assistant.", "Talk"));
			HomeServicesMenu.AddItem(Item("Gate & grounds security", "Armed security with four patrols and fixed posts. Enabled by default when no preference is saved.", "Security"));
			HomeServicesMenu.RefreshIndex();
		}

		private static void AssistantMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			Mansion mansion = assistMansion;
			if (mansion == null)
			{
				return;
			}
			try
			{
				switch (Conversions.ToString(selectedItem.Tag))
				{
				case "AI":
					BuildAiOptionsMenu(mansion);
					OpenChild(sender, AiOptionsMenu);
					break;
				case "Greeting":
					Voice(mansion, "hello");
					break;
				case "Vehicles":
					BuildGarageServicesMenu(mansion);
					OpenChild(sender, GarageServicesMenu);
					break;
				case "BuyVehicles":
					MansionVehicleShop.Open(mansion, sender);
					break;
				case "Home":
					BuildHomeServicesMenu(mansion);
					OpenChild(sender, HomeServicesMenu);
					break;
				case "Pets":
					MansionLeisure.OpenPets(mansion, sender);
					break;
				case "PrivateDancer":
					sender.Visible = false;
					PrivateDancerPreview.Open(sender);
					break;
				case "Garage":
					sender.Visible = false;
					Mansions.currentMansion = mansion;
					Mansions.EnterMansionGarage(mansion, true);
					break;
				case "Elevator":
					elevMansion = mansion;
					elevFrom = "";
					BuildElevatorMenu(mansion, "");
					OpenChild(sender, ElevatorMenu);
					break;
				case "Workshop":
					MansionWorkshopTravel.OpenWorkshopHere(mansion, sender);
					break;
				case "Travel":
					Voice(mansion, "travel");
					BuildTravelMenu(mansion);
					OpenChild(sender, TravelMenu);
					break;
				case "ExitVeh":
					BuildExitVehMenu(mansion);
					OpenChild(sender, ExitVehMenu);
					break;
				case "ExitLux":
					sender.Visible = false;
					LeaveInLuxuryCar(mansion);
					break;
				case "ExitHeli":
					sender.Visible = false;
					LeaveByHelicopter(mansion);
					break;
				case "Chauffeur":
					sender.Visible = false;
					CallChauffeur(mansion);
					break;
				case "Security":
					MansionSecurity.Open(mansion, sender);
					break;
				case "Talk":
					SmallTalk(mansion);
					break;
				case "Champagne":
					sender.Visible = false;
					Say(mansion, "Champagne is on its way.", "champagne");
					PartyService.DrinkChampagne();
					break;
				case "Party":
					sender.Visible = false;
					Voice(mansion, "party");
					PartyService.OpenPartyMenuHere(sender);
					break;
				case "Gym":
					Voice(mansion, "gym");
					BuildGymMenu();
					OpenChild(sender, GymMenu);
					break;
				case "Office":
				case "Kitchen":
					MansionAmenities.OpenService(Conversions.ToString(selectedItem.Tag), mansion, sender);
					break;
				case "Armory":
					sender.Visible = false;
					MansionArmory.Open(mansion, sender);
					break;
				case "Podium":
					podiumMansion = mansion;
					BuildPodiumMenu(mansion);
					OpenChild(sender, PodiumMenu);
					break;
				case "Vault":
					ShowVault(mansion);
					break;
				case "Manage":
					Mansions.currentMansion = mansion;
					Mansions.BuildMansionMenu(mansion);
					OpenChild(sender, Mansions.MansionMenu);
					break;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Assistant select: {ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		private static void BuildTravelMenu(Mansion m)
		{
			TravelMenu.Clear();
			foreach (Mansion item in Mansions.MansionList.Where((_Closure_0024__._0024I91_002D0 != null) ? _Closure_0024__._0024I91_002D0 : (_Closure_0024__._0024I91_002D0 = _Closure_0024__._0024I._Lambda_0024__91_002D0)))
			{
				TravelMenu.AddItem(new UIMenuItem(item.Name, (item == m) ? "Back home." : "Your mansion.")
				{
					Tag = item
				});
			}
			try
			{
				foreach (ApartmentClass item2 in Helper.apartments.Where((_Closure_0024__._0024I91_002D1 != null) ? _Closure_0024__._0024I91_002D1 : (_Closure_0024__._0024I91_002D1 = _Closure_0024__._0024I._Lambda_0024__91_002D1)))
				{
					BuildingClass buildingClass = item2.Building();
					if (buildingClass != null && IsSet(TravelTarget(buildingClass)))
					{
						TravelMenu.AddItem(new UIMenuItem(item2.FriendlyName(), "Your apartment / garage.")
						{
							Tag = item2
						});
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Travel menu: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
			if (TravelMenu.MenuItems.Count == 0)
			{
				TravelMenu.AddItem(new UIMenuItem("(no other properties)")
				{
					Enabled = false
				});
			}
		}

		private static Quaternion TravelTarget(BuildingClass b)
		{
			Quaternion result;
			if (b == null)
			{
				result = new Quaternion(0f, 0f, 0f, 0f);
			}
			else
			{
				if (IsSet(b.BuildingOutPos))
				{
					return b.BuildingOutPos;
				}
				if (IsSet(b.GarageFootOutPos))
				{
					return b.GarageFootOutPos;
				}
				if (IsSet(b.GarageOutPos))
				{
					return b.GarageOutPos;
				}
				result = new Quaternion(0f, 0f, 0f, 0f);
			}
			return result;
		}

		private static void TravelMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			if ((object)Helper.PP == null || !Helper.PP.Exists())
			{
				return;
			}
			Ped pP = Helper.PP;
			bool flag = DestinationStreaming.OriginalEntityFreeze(pP, pP.IsPositionFrozen);
			Vehicle vehicle = (pP.IsInVehicle() ? pP.CurrentVehicle : null);
			bool flag2 = (object)vehicle != null && vehicle.Exists() && DestinationStreaming.OriginalEntityFreeze(vehicle, vehicle.IsPositionFrozen);
			int num = -2;
			bool flag3 = false;
			checked
			{
				if ((object)vehicle != null && vehicle.Exists())
				{
					int num2 = Math.Min(Function.Call<int>(Hash.GET_VEHICLE_MAX_NUMBER_OF_PASSENGERS, vehicle.Handle) - 1, 15);
					for (int i = -1; i <= num2; i++)
					{
						if (Function.Call<int>(Hash.GET_PED_IN_VEHICLE_SEAT, vehicle.Handle, i, false) == pP.Handle)
						{
							num = i;
							break;
						}
					}
				}
				try
				{
					Function.Call(Hash.FREEZE_ENTITY_POSITION, pP.Handle, true);
					if ((object)vehicle != null && vehicle.Exists())
					{
						Function.Call(Hash.FREEZE_ENTITY_POSITION, vehicle.Handle, true);
					}
					TravelMenu.Visible = false;
					if (selectedItem.Tag is Mansion)
					{
						Mansion mansion = (Mansion)selectedItem.Tag;
						Quaternion stop = GetStop(mansion, "Main");
						if (!IsSet(stop))
						{
							return;
						}
						using (DestinationStreaming.SceneLease sceneLease = DestinationStreaming.Acquire(MansionAutoLayout.MainAnchor(mansion)))
						{
							if (sceneLease == null)
							{
								return;
							}
							Helper.FadeScreen(1, 500);
							if (!Mansions.PrepareMansionNow(mansion) || !TeleportPlayer(Helper.ToVector3(stop), stop.W, false, "m25_2_int_mansion", sceneLease))
							{
								return;
							}
						}
						LeaveGarageState();
						flag3 = true;
						Logger.Log($"> fast travel to {mansion.Name}");
					}
					else if (selectedItem.Tag is ApartmentClass)
					{
						ApartmentClass apartmentClass = (ApartmentClass)selectedItem.Tag;
						Quaternion q = TravelTarget(apartmentClass.Building());
						if (IsSet(q) && TeleportPlayer(Helper.ToVector3(q), q.W))
						{
							LeaveGarageState();
							flag3 = true;
							Logger.Log($"> fast travel to {apartmentClass.Name}");
						}
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log($"Fast travel: {ex2.Message}");
					ProjectData.ClearProjectError();
				}
				finally
				{
					if (!flag3 && num >= -1 && (object)vehicle != null && vehicle.Exists() && pP.Exists() && !pP.IsDead && !DestinationStreaming.IsPlayerSafetyHeld(pP) && pP.Position.DistanceTo(vehicle.Position) < 20f)
					{
						int num3 = Function.Call<int>(Hash.GET_PED_IN_VEHICLE_SEAT, vehicle.Handle, num, false);
						if (num3 == 0 || num3 == pP.Handle)
						{
							Function.Call(Hash.SET_PED_INTO_VEHICLE, pP.Handle, vehicle.Handle, num);
						}
					}
					DestinationStreaming.SetSafetyHoldOriginalFreeze(pP, flag);
					DestinationStreaming.SetSafetyHoldOriginalFreeze(vehicle, flag2);
					if (pP.Exists() && !DestinationStreaming.IsPlayerSafetyHeld(pP))
					{
						Function.Call(Hash.FREEZE_ENTITY_POSITION, pP.Handle, flag);
					}
					if ((object)vehicle != null && vehicle.Exists() && !DestinationStreaming.IsPlayerSafetyHeld(vehicle))
					{
						Function.Call(Hash.FREEZE_ENTITY_POSITION, vehicle.Handle, flag2);
					}
					FadeInAfterTravel();
				}
			}
		}

		private static Quaternion DrivewaySpawn(Mansion m)
		{
			if (IsSet(m.GaragePos))
			{
				return new Quaternion(m.GaragePos.X, m.GaragePos.Y, m.GaragePos.Z, m.GaragePos.W + 180f);
			}
			return m.SignPos;
		}

		private static void BuildExitVehMenu(Mansion m)
		{
			ExitVehMenu.Clear();
			foreach (VehicleClass item in Mansions.MansionGarageVehicles(m))
			{
				bool flag = IsVehicleBusy(m, item.UniqueID);
				bool flag2 = Helper.IsGarageVehicleAlreadyExistInWorldMap(m.GarageId, item.UniqueID);
				UIMenuItem uIMenuItem = new UIMenuItem($"{item.Make} {item.Name}", flag2 ? "This vehicle is already out." : (flag ? "On the podium / lift." : "Waiting at the driveway."))
				{
					Tag = item,
					Enabled = (!flag2 && !flag)
				};
				uIMenuItem.SetRightLabel(item.PlateNumber);
				ExitVehMenu.AddItem(uIMenuItem);
			}
			if (ExitVehMenu.MenuItems.Count == 0)
			{
				ExitVehMenu.AddItem(new UIMenuItem("(the mansion garage is empty)")
				{
					Enabled = false
				});
			}
		}

		private static void ExitVehMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			Mansion mansion = assistMansion;
			VehicleClass vehicleClass = selectedItem.Tag as VehicleClass;
			if (mansion == null || vehicleClass == null)
			{
				return;
			}
			ExitVehMenu.Visible = false;
			try
			{
				RemoveWorkshopVehicle(mansion);
				Helper.FadeScreen(1, 500);
				Vehicle vehicle = Mansions.SpawnOutVehicle(mansion, vehicleClass, FreeSpawnNear(DrivewaySpawn(mansion)));
				if ((object)vehicle != null)
				{
					Function.Call(Hash.REQUEST_COLLISION_AT_COORD, vehicle.Position.X, vehicle.Position.Y, vehicle.Position.Z);
					Script.Wait(300);
					Helper.SetPlayerIntoVehicle(vehicle);
					vehicle.IsEngineRunning = true;
					Say(mansion, $"Your {vehicleClass.Make} {vehicleClass.Name} is ready. Drive safe.", "car");
				}
				else
				{
					Notification.Show("Vehicle model could not be loaded (addon car missing?).");
				}
				FadeInAfterTravel();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				FadeInAfterTravel(300);
				Logger.Log($"Exit personal vehicle: {ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		private static void ClearSpot(Vector3 pos, float radius)
		{
			try
			{
				Vehicle[] nearbyVehicles = World.GetNearbyVehicles(pos, radius);
				foreach (Vehicle vehicle in nearbyVehicles)
				{
					if ((object)vehicle != null && vehicle.Exists() && !vehicle.IsPersistent && vehicle.Occupants.Length <= 0)
					{
						vehicle.Delete();
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"ClearSpot: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
		}

		private static bool SpotFree(Vector3 c)
		{
			Vehicle[] nearbyVehicles = World.GetNearbyVehicles(c, 4f);
			foreach (Vehicle vehicle in nearbyVehicles)
			{
				if ((object)vehicle != null && vehicle.Exists())
				{
					return false;
				}
			}
			return true;
		}

		private static Quaternion FreeSpawnNear(Quaternion q)
		{
			try
			{
				double num = (double)q.W * Math.PI / 180.0;
				Vector3 vector = new Vector3((float)(0.0 - Math.Sin(num)), (float)Math.Cos(num), 0f);
				Vector3 vector2 = new Vector3((float)Math.Cos(num), (float)Math.Sin(num), 0f);
				Vector3 vector3 = Helper.ToVector3(q);
				Vector3 vector4 = new Vector3(0f, 0f, 1f);
				Vector3[] array = new Vector3[5]
				{
					Vector3.Zero,
					vector * 6f,
					vector2 * 3.5f,
					-vector2 * 3.5f,
					vector * 12f
				};
				foreach (Vector3 vector5 in array)
				{
					Vector3 vector6 = vector3 + vector5;
					if (vector5 != Vector3.Zero)
					{
						if (World.Raycast(vector3 + vector4, vector6 + vector4, IntersectFlags.Map, Helper.PP).DidHit)
						{
							continue;
						}
						RaycastResult raycastResult = World.Raycast(vector6 + vector4, vector6 - vector4 * 2f, IntersectFlags.Map, Helper.PP);
						if (!raycastResult.DidHit)
						{
							continue;
						}
						vector6 = new Vector3(vector6.X, vector6.Y, raycastResult.HitPosition.Z + 0.3f);
					}
					if (SpotFree(vector6))
					{
						return new Quaternion(vector6.X, vector6.Y, vector6.Z, q.W);
					}
				}
				ClearSpot(vector3, 3f);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"FreeSpawnNear: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
			return q;
		}

		private static Vehicle SpawnTempVehicle(string modelName, Quaternion q)
		{
			Model model = new Model(modelName);
			if (!model.IsInCdImage || !model.IsValid)
			{
				return null;
			}
			model.Request(3000);
			if (!model.IsLoaded)
			{
				return null;
			}
			Vehicle vehicle = World.CreateVehicle(model, Helper.ToVector3(q) + new Vector3(0f, 0f, 0.5f), q.W);
			model.MarkAsNoLongerNeeded();
			if ((object)vehicle == null)
			{
				return null;
			}
			Function.Call(Hash.SET_VEHICLE_ON_GROUND_PROPERLY, vehicle.Handle, 5f);
			return vehicle;
		}

		private static void LeaveInLuxuryCar(Mansion m)
		{
			try
			{
				_Closure_0024__101_002D0 arg = new _Closure_0024__101_002D0(null);
				arg._0024VB_0024Local_rnd = new Random();
				List<string> list = LuxuryModels.OrderBy(arg._Lambda_0024__0).ToList();
				Helper.FadeScreen(1, 500);
				Quaternion q = FreeSpawnNear(DrivewaySpawn(m));
				Vehicle vehicle = null;
				foreach (string item in list)
				{
					vehicle = SpawnTempVehicle(item, q);
					if ((object)vehicle != null)
					{
						break;
					}
				}
				if ((object)vehicle != null)
				{
					Helper.SetPlayerIntoVehicle(vehicle);
					vehicle.IsEngineRunning = true;
					vehicle.MarkAsNoLongerNeeded();
					Say(m, "Your car is at the driveway.", "car");
				}
				FadeInAfterTravel();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				FadeInAfterTravel(300);
				Logger.Log($"Luxury car: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
		}

		private static void LeaveByHelicopter(Mansion m)
		{
			if (!IsSet(m.HelipadPos))
			{
				return;
			}
			try
			{
				Helper.FadeScreen(1, 500);
				if (!TeleportPlayer(Helper.ToVector3(m.HelipadPos), m.HelipadPos.W, false))
				{
					FadeInAfterTravel();
					return;
				}
				ClearSpot(Helper.ToVector3(m.HelipadPos), 7f);
				Vehicle vehicle = SpawnTempVehicle(HeliModel, m.HelipadPos);
				if ((object)vehicle != null)
				{
					Helper.SetPlayerIntoVehicle(vehicle);
					vehicle.IsEngineRunning = true;
					Function.Call(Hash.SET_HELI_BLADES_FULL_SPEED, vehicle.Handle);
					vehicle.MarkAsNoLongerNeeded();
					Say(m, "The Volatus is ready for take-off.", "heli");
				}
				else
				{
					Notification.Show("The helicopter model could not be loaded.");
				}
				FadeInAfterTravel();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				FadeInAfterTravel(300);
				Logger.Log($"Helicopter: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
		}

		private static Quaternion RoadSpawnNearPlayer()
		{
			Vector3 position = Helper.PP.Position;
			try
			{
				OutputArgument outputArgument = new OutputArgument();
				OutputArgument outputArgument2 = new OutputArgument();
				if (Function.Call<bool>(Hash.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING, position.X, position.Y, position.Z, outputArgument, outputArgument2, 1, 3f, 0))
				{
					Vector3 result = outputArgument.GetResult<Vector3>();
					if (result != Vector3.Zero && result.DistanceTo(position) < 300f)
					{
						return new Quaternion(result.X, result.Y, result.Z, outputArgument2.GetResult<float>());
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Chauffeur road node: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
			Vector3 vector = position + Helper.PP.ForwardVector * 4f;
			return new Quaternion(vector.X, vector.Y, vector.Z, Helper.PP.Heading);
		}

		private static void CallChauffeur(Mansion m)
		{
			if (Helper.PP.IsInVehicle())
			{
				Say(m, "Please get out of your vehicle first - the chauffeur will pick you up.");
				return;
			}
			try
			{
				DismissChauffeur(true);
				Helper.FadeScreen(1, 500);
				Quaternion q = (AtMansion(m) ? FreeSpawnNear(DrivewaySpawn(m)) : FreeSpawnNear(RoadSpawnNearPlayer()));
				Vehicle vehicle = null;
				string[] chauffeurCars = ChauffeurCars;
				for (int i = 0; i < chauffeurCars.Length; i = checked(i + 1))
				{
					vehicle = SpawnTempVehicle(chauffeurCars[i], q);
					if ((object)vehicle != null)
					{
						break;
					}
				}
				if ((object)vehicle == null)
				{
					FadeInAfterTravel(300);
					Notification.Show("No limousine model could be loaded.");
					return;
				}
				Ped ped = null;
				string[] chauffeurPeds = ChauffeurPeds;
				foreach (string name in chauffeurPeds)
				{
					Model model = new Model(name);
					try
					{
						if (!model.IsInCdImage || !model.IsValid || !model.IsPed)
						{
							continue;
						}
						model.Request(3000);
						if (model.IsLoaded)
						{
							ped = vehicle.CreatePedOnSeat(VehicleSeat.Driver, model);
							if ((object)ped != null)
							{
								break;
							}
						}
					}
					finally
					{
						model.MarkAsNoLongerNeeded();
					}
				}
				if ((object)ped == null)
				{
					vehicle.Delete();
					FadeInAfterTravel(300);
					Notification.Show("The chauffeur could not be created.");
					return;
				}
				Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, ped.Handle, true);
				Function.Call(Hash.SET_PED_KEEP_TASK, ped.Handle, true);
				Function.Call(Hash.SET_PED_CAN_BE_DRAGGED_OUT, ped.Handle, false);
				Function.Call(Hash.SET_DRIVER_ABILITY, ped.Handle, 1f);
				Function.Call(Hash.SET_DRIVER_AGGRESSIVENESS, ped.Handle, 0f);
				vehicle.IsEngineRunning = true;
				LeaveGarageState();
				Helper.PP.SetIntoVehicle(vehicle, VehicleSeat.RightRear);
				chauffeurVeh = vehicle;
				chauffeurPed = ped;
				chauffeurDest = Vector3.Zero;
				chauffeurArrived = false;
				chauffeurWasIn = true;
				chauffeurNextCheck = 0;
				Script.Wait(300);
				FadeInAfterTravel();
				if (Game.IsWaypointActive)
				{
					Say(m, "Your chauffeur is taking you to your waypoint.", "chauffeur");
				}
				else
				{
					Say(m, "Your chauffeur is waiting. Set a waypoint on the map and he drives you there.", "chauffeur");
				}
				GTA.UI.Screen.ShowHelpText("Set a waypoint on the map - the chauffeur drives you there. Get out to send him away.", 8000, false);
				Logger.Log("> chauffeur");
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				FadeInAfterTravel(300);
				Logger.Log($"Chauffeur: {ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		public static void ChauffeurTick()
		{
			if ((object)chauffeurVeh == null && (object)chauffeurPed == null)
			{
				return;
			}
			checked
			{
				try
				{
					if ((object)chauffeurVeh == null || !chauffeurVeh.Exists() || (object)chauffeurPed == null || !chauffeurPed.Exists() || chauffeurPed.IsDead || !chauffeurVeh.IsDriveable || !chauffeurPed.IsInVehicle(chauffeurVeh))
					{
						DismissChauffeur(false);
						return;
					}
					if (!Helper.PP.IsInVehicle(chauffeurVeh))
					{
						if (chauffeurWasIn)
						{
							chauffeurWasIn = false;
							chauffeurLeftAt = Game.GameTime;
						}
						if (Game.GameTime - chauffeurLeftAt > 4000)
						{
							DismissChauffeur(false);
						}
						return;
					}
					chauffeurWasIn = true;
					if (Game.GameTime < chauffeurNextCheck)
					{
						return;
					}
					chauffeurNextCheck = Game.GameTime + 1000;
					if (Game.IsWaypointActive)
					{
						Vector3 waypointPosition = World.WaypointPosition;
						if (waypointPosition != Vector3.Zero && (chauffeurDest == Vector3.Zero || Mansion.Dist2D(waypointPosition, chauffeurDest) > 5f))
						{
							chauffeurDest = waypointPosition;
							chauffeurArrived = false;
							Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, chauffeurPed.Handle, chauffeurVeh.Handle, waypointPosition.X, waypointPosition.Y, waypointPosition.Z, 20f, 786603, 20f);
							Logger.Log($"> chauffeur to {waypointPosition.X:F0} / {waypointPosition.Y:F0}");
						}
					}
					if (chauffeurDest != Vector3.Zero && !chauffeurArrived && Mansion.Dist2D(chauffeurVeh.Position, chauffeurDest) < 35f && chauffeurVeh.Speed < 1f)
					{
						chauffeurArrived = true;
						Notification.Show("~b~Chauffeur~w~: We have arrived. Have a nice day.");
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log($"ChauffeurTick: {ex2.Message}");
					DismissChauffeur(false);
					ProjectData.ClearProjectError();
				}
			}
		}

		private static void DismissChauffeur(bool deleteNow)
		{
			Vehicle vehicle = chauffeurVeh;
			Ped ped = chauffeurPed;
			chauffeurVeh = null;
			chauffeurPed = null;
			chauffeurDest = Vector3.Zero;
			chauffeurArrived = false;
			try
			{
				bool flag = (object)vehicle != null && vehicle.Exists() && Helper.PP.IsInVehicle(vehicle);
				if ((object)ped != null && ped.Exists())
				{
					if (deleteNow && !flag)
					{
						ped.Delete();
					}
					else
					{
						if (!flag && (object)vehicle != null && vehicle.Exists() && ped.IsInVehicle(vehicle) && !ped.IsDead)
						{
							Function.Call(Hash.TASK_VEHICLE_DRIVE_WANDER, ped.Handle, vehicle.Handle, 15f, 786603);
						}
						Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, ped.Handle, false);
						ped.MarkAsNoLongerNeeded();
					}
				}
				if ((object)vehicle != null && vehicle.Exists())
				{
					if (deleteNow && !flag)
					{
						vehicle.Delete();
					}
					else
					{
						vehicle.MarkAsNoLongerNeeded();
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"DismissChauffeur: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
		}

		private static void SmallTalk(Mansion m)
		{
			try
			{
				int num = Function.Call<int>(Hash.GET_CLOCK_HOURS);
				int num2 = Function.Call<int>(Hash.GET_CLOCK_MINUTES);
				string text = World.Weather.ToString();
				text = ((Operators.CompareString(text, "Unknown", false) == 0) ? "changing" : Regex.Replace(text, "(?<=[a-z])(?=[A-Z])", " ").ToLowerInvariant());
				Say(m, $"It is {num:00}:{num2:00} and the weather is {text}. {SmallTalkLines[talkRnd.Next(SmallTalkLines.Length)]}", "talk");
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Small talk: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
		}

		private static void BuildGymMenu()
		{
			GymMenu.Clear();
			GymMenu.AddItem(Item("Lift weights", "Strength +2, stamina +1, then more melee damage and less damage taken for a while.", "weights"));
			GymMenu.AddItem(Item("Push-ups", "Strength +2, stamina +1, same bonus as the weights.", "pushups"));
			GymMenu.AddItem(Item("Sit-ups", "Strength +1, stamina +2, same bonus as the weights.", "situps"));
			GymMenu.AddItem(Item("Yoga", "Lung capacity +1, stamina +1, then faster sprinting and swimming for a while.", "yoga"));
		}

		private static void GymMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			GymMenu.Visible = false;
			Workout(Conversions.ToString(selectedItem.Tag));
		}

		private static string StatPrefix()
		{
			switch (Helper.GetPlayer())
			{
			case eOwner.Michael:
				return "SP0";
			case eOwner.Franklin:
				return "SP1";
			case eOwner.Trevor:
				return "SP2";
			default:
				return null;
			}
		}

		private static int GetStatInt(string name)
		{
			OutputArgument outputArgument = new OutputArgument();
			if (Function.Call<bool>(Hash.STAT_GET_INT, Game.GenerateHash(name), outputArgument, -1))
			{
				return outputArgument.GetResult<int>();
			}
			return 0;
		}

		private static void AddStat(string stat, int delta)
		{
			string text = StatPrefix();
			if (text != null && delta != 0)
			{
				string text2 = $"{text}_{stat}";
				int statInt = GetStatInt(text2);
				Function.Call(Hash.STAT_SET_INT, Game.GenerateHash(text2), Math.Min(100, checked(statInt + delta)), true);
			}
		}

		public static void Workout(string kind)
		{
			if ((object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsDead || Helper.PP.IsInVehicle() || PartyService.IsPlayerActionActive || PartyPlayerActions.IsKissing)
			{
				return;
			}
			if (IsWorkoutActive)
			{
				Notification.Show("Finish the current workout first.");
				return;
			}
			switch (kind)
			{
			case "pushups":
				workoutScenario = "WORLD_HUMAN_PUSH_UPS";
				break;
			case "situps":
				workoutScenario = "WORLD_HUMAN_SIT_UPS";
				break;
			case "yoga":
				workoutScenario = "WORLD_HUMAN_YOGA";
				break;
			default:
				kind = "weights";
				workoutScenario = "WORLD_HUMAN_MUSCLE_FREE_WEIGHTS";
				break;
			}
			try
			{
				PartyPlayerActions.StopPlayerDance();
				workoutPed = Helper.PP;
				workoutOwner = Helper.GetPlayer();
				workoutOrigin = Helper.PP.Position;
				workoutKind = kind;
				workoutStartedAt = Game.GameTime;
				Function.Call(Hash.CLEAR_PED_TASKS, workoutPed.Handle);
				Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, workoutPed.Handle, workoutScenario, 0, true);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Workout start: " + ex2.Message);
				CancelWorkout();
				ProjectData.ClearProjectError();
			}
		}

		public static void CancelWorkout()
		{
			Ped ped = workoutPed;
			string text = workoutScenario;
			workoutPed = null;
			workoutScenario = null;
			workoutKind = null;
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
				Logger.Log("Workout cleanup: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		public static void MansionActionsTick()
		{
			Ped ped = workoutPed;
			if ((object)ped == null)
			{
				return;
			}
			checked
			{
				try
				{
					long num = unchecked((long)Game.GameTime) - unchecked((long)workoutStartedAt);
					bool flag = (object)Helper.PP == null || !Helper.PP.Exists() || !ped.Exists() || Helper.PP.Handle != ped.Handle || Helper.GetPlayer() != workoutOwner || ped.IsDead || ped.IsInVehicle() || ped.IsFalling || ped.IsRagdoll || ped.Position.DistanceTo(workoutOrigin) > 1.5f || PartyService.IsPlayerActionActive || PartyPlayerActions.IsKissing;
					if (!flag && num > 1500)
					{
						flag = !Function.Call<bool>(Hash.IS_PED_USING_SCENARIO, ped.Handle, workoutScenario);
					}
					bool flag2 = false;
					if (!flag && !Helper.MenuPool.IsAnyMenuOpen() && num >= 250)
					{
						flag = Game.IsControlJustPressed(GTA.Control.FrontendCancel) || Game.IsControlJustPressed(GTA.Control.PhoneCancel) || Math.Abs(Function.Call<float>(Hash.GET_DISABLED_CONTROL_NORMAL, 0, 30)) > 0.2f || Math.Abs(Function.Call<float>(Hash.GET_DISABLED_CONTROL_NORMAL, 0, 31)) > 0.2f;
						flag2 = Game.IsControlJustReleased(GTA.Control.Context);
					}
					if (flag2)
					{
						Game.DisableControlThisFrame(GTA.Control.Context);
					}
					string left = PartyPlayerActions.TimedActionOutcome(num, 20000, flag);
					if (Operators.CompareString(left, "Cancelled", false) == 0)
					{
						CancelWorkout();
					}
					else if (Operators.CompareString(left, "Completed", false) == 0 || flag2)
					{
						string text = workoutKind;
						CancelWorkout();
						if (Operators.CompareString(PartyPlayerActions.TimedActionOutcome(num, 8000, false), "Completed", false) != 0)
						{
							Notification.Show("Workout stopped too early - no bonus.");
							return;
						}
						int handle = Game.Player.Handle;
						if (Operators.CompareString(text, "yoga", false) == 0)
						{
							AddStat("LUNG_CAPACITY", 1);
							AddStat("STAMINA", 1);
							yogaBuffUntil = Game.GameTime + BuffMinutes * 60000;
							Function.Call(Hash.SET_RUN_SPRINT_MULTIPLIER_FOR_PLAYER, handle, 1.15f);
							Function.Call(Hash.SET_SWIM_MULTIPLIER_FOR_PLAYER, handle, 1.15f);
							Notification.Show($"~g~Yoga~w~: you sprint and swim faster for {BuffMinutes} minutes.");
						}
						else
						{
							if (Operators.CompareString(text, "situps", false) == 0)
							{
								AddStat("STRENGTH", 1);
								AddStat("STAMINA", 2);
							}
							else
							{
								AddStat("STRENGTH", 2);
								AddStat("STAMINA", 1);
							}
							strengthBuffUntil = Game.GameTime + BuffMinutes * 60000;
							Function.Call(Hash.SET_PLAYER_MELEE_WEAPON_DAMAGE_MODIFIER, handle, 1.25f, 1);
							Function.Call(Hash.SET_PLAYER_WEAPON_DEFENSE_MODIFIER, handle, 0.85f);
							Notification.Show($"~g~Workout~w~: more melee damage and less damage taken for {BuffMinutes} minutes.");
						}
						Logger.Log($"> workout {text}");
					}
					else
					{
						GTA.Control[] array = new GTA.Control[6]
						{
							GTA.Control.MoveLeftRight,
							GTA.Control.MoveUpDown,
							GTA.Control.Jump,
							GTA.Control.Sprint,
							GTA.Control.Attack,
							GTA.Control.Aim
						};
						for (int i = 0; i < array.Length; i++)
						{
							Game.DisableControlThisFrame(array[i]);
						}
						if (!Helper.MenuPool.IsAnyMenuOpen())
						{
							Helper.ShowPersistentHelp("~INPUT_CONTEXT~ finish the workout  ~INPUT_FRONTEND_CANCEL~ cancel");
						}
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					CancelWorkout();
					Logger.Log("Workout tick: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
		}

		public static void BuffTick()
		{
			if (strengthBuffUntil > 0 && Game.GameTime > strengthBuffUntil)
			{
				strengthBuffUntil = 0;
				Function.Call(Hash.SET_PLAYER_MELEE_WEAPON_DAMAGE_MODIFIER, Game.Player.Handle, 1f, 1);
				Function.Call(Hash.SET_PLAYER_WEAPON_DEFENSE_MODIFIER, Game.Player.Handle, 1f);
			}
			if (yogaBuffUntil > 0 && Game.GameTime > yogaBuffUntil)
			{
				yogaBuffUntil = 0;
				Function.Call(Hash.SET_RUN_SPRINT_MULTIPLIER_FOR_PLAYER, Game.Player.Handle, 1f);
				Function.Call(Hash.SET_SWIM_MULTIPLIER_FOR_PLAYER, Game.Player.Handle, 1f);
			}
		}

		private static void ResetBuffs()
		{
			checked
			{
				try
				{
					if (strengthBuffUntil > 0 || yogaBuffUntil > 0)
					{
						strengthBuffUntil = Game.GameTime - 1;
						yogaBuffUntil = Game.GameTime - 1;
						BuffTick();
					}
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					ProjectData.ClearProjectError();
				}
			}
		}

		private static void UseArmory(Mansion m)
		{
			if (!TryPayMansion(ArmoryCost))
			{
				return;
			}
			int num = 0;
			try
			{
				IEnumerator enumerator = default(IEnumerator);
				try
				{
					enumerator = Enum.GetValues(typeof(WeaponHash)).GetEnumerator();
					while (enumerator.MoveNext())
					{
						WeaponHash weaponHash = (WeaponHash)Conversions.ToUInteger(enumerator.Current);
						if (weaponHash != WeaponHash.Unarmed && Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, Helper.PP.Handle, (uint)weaponHash, false))
						{
							OutputArgument outputArgument = new OutputArgument();
							if (Function.Call<bool>(Hash.GET_MAX_AMMO, Helper.PP.Handle, (uint)weaponHash, outputArgument))
							{
								Function.Call(Hash.SET_PED_AMMO, Helper.PP.Handle, (uint)weaponHash, outputArgument.GetResult<int>(), false);
								num = checked(num + 1);
							}
						}
					}
				}
				finally
				{
					if (enumerator is IDisposable)
					{
						(enumerator as IDisposable).Dispose();
					}
				}
				Helper.PP.Armor = 100;
				Say(m, $"Armory: {num} weapon(s) refilled, armour on.", "armory");
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Armory: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
		}

		private static void ShowVault(Mansion m)
		{
			int statInt = GetStatInt("SP0_TOTAL_CASH");
			int statInt2 = GetStatInt("SP1_TOTAL_CASH");
			int statInt3 = GetStatInt("SP2_TOTAL_CASH");
			Say(m, $"Vault - Michael ~g~${statInt:N0}~w~, Franklin ~g~${statInt2:N0}~w~, Trevor ~g~${statInt3:N0}~w~", "vault");
		}

		private static string ElevatorInteriorType(string key)
		{
			switch ((key ?? string.Empty).ToUpperInvariant())
			{
			case "MAIN":
			case "LOWER":
				return "m25_2_int_mansion";
			case "B1":
			case "B2":
				return "m25_2_int_mansion_2";
			case "GARAGE":
				return "m25_2_int_mansion_garage";
			default:
				return null;
			}
		}

		public static Quaternion GetStop(Mansion m, string key)
		{
			Quaternion value;
			if (m.ElevatorStops.TryGetValue(key, out value))
			{
				return value;
			}
			return new Quaternion(0f, 0f, 0f, 0f);
		}

		private static bool ElevatorTick(Mansion m)
		{
			if (Helper.PP.IsInVehicle())
			{
				return false;
			}
			Vector3 position = Helper.PP.Position;
			string[] elevatorFloorKeys = ElevatorFloorKeys;
			foreach (string text in elevatorFloorKeys)
			{
				Quaternion stop = GetStop(m, text);
				if (!IsSet(stop))
				{
					continue;
				}
				Vector3 b = Helper.ToVector3(stop);
				if (Mansion.Dist2D(position, b) < 1.6f && Math.Abs(position.Z - b.Z) < 1.5f)
				{
					if (!Help("Press ~INPUT_CONTEXT~ to use the elevator."))
					{
						return false;
					}
					if (Game.IsControlJustReleased(GTA.Control.Context))
					{
						elevMansion = m;
						elevFrom = text;
						BuildElevatorMenu(m, text);
						ElevatorMenu.ParentMenu = null;
						ElevatorMenu.RefreshIndex();
						ElevatorMenu.Visible = true;
					}
					return true;
				}
			}
			return false;
		}

		private static void BuildElevatorMenu(Mansion m, string fromKey)
		{
			ElevatorMenu.Clear();
			ElevatorMenu.Subtitle.Caption = $"ELEVATOR - {FloorName(fromKey).ToUpper()}";
			string[] elevatorFloorKeys = ElevatorFloorKeys;
			foreach (string text in elevatorFloorKeys)
			{
				if (Operators.CompareString(text, fromKey, false) != 0 && IsSet(GetStop(m, text)))
				{
					ElevatorMenu.AddItem(new UIMenuItem(FloorName(text))
					{
						Tag = text
					});
				}
			}
			ElevatorMenu.AddItem(new UIMenuItem("Garage", $"Your mansion garage ({Mansions.MansionGarageVehicles(m).Count}/{20} vehicles).")
			{
				Tag = "GARAGE"
			});
		}

		private static void ElevatorMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			Mansion mansion = elevMansion;
			if (mansion == null)
			{
				return;
			}
			ElevatorMenu.Visible = false;
			try
			{
				string text = Conversions.ToString(selectedItem.Tag);
				Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "FAKE_ARRIVE", "MP_PROPERTIES_ELEVATOR_DOORS", true);
				if (Operators.CompareString(text, "GARAGE", false) == 0)
				{
					Mansions.currentMansion = mansion;
					Mansions.EnterMansionGarage(mansion, true);
					return;
				}
				Quaternion stop = GetStop(mansion, text);
				if (TeleportPlayer(Helper.ToVector3(stop), stop.W, true, ElevatorInteriorType(text)))
				{
					LeaveGarageState();
					Logger.Log($"> {mansion.Name}: elevator {elevFrom} -> {text}");
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				FadeInAfterTravel(300);
				Logger.Log($"Elevator: {ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		public static void GarageElevatorToMansion(ApartmentClass apt)
		{
			Mansion mansion = MansionForGarageId(apt.ID);
			if (mansion == null)
			{
				return;
			}
			try
			{
				Quaternion q = new Quaternion(0f, 0f, 0f, 0f);
				string expectedInteriorType = null;
				string[] array = new string[6] { "B1", "Lower", "Main", "B2", "Upper", "Roof" };
				foreach (string key in array)
				{
					if (IsSet(GetStop(mansion, key)))
					{
						q = GetStop(mansion, key);
						expectedInteriorType = ElevatorInteriorType(key);
						break;
					}
				}
				Helper.FadeScreen(1, 500);
				if (IsSet(q) && !Mansions.PrepareMansionNow(mansion))
				{
					q = new Quaternion(0f, 0f, 0f, 0f);
				}
				if (!IsSet(q))
				{
					q = DrivewaySpawn(mansion);
					expectedInteriorType = null;
				}
				Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "FAKE_ARRIVE", "MP_PROPERTIES_ELEVATOR_DOORS", true);
				if (!TeleportPlayer(Helper.ToVector3(q), q.W, false, expectedInteriorType))
				{
					FadeInAfterTravel();
					return;
				}
				TenCarGarage.Clear();
				TenCarGarage.Apartment = null;
				Helper.HideHud = false;
				FadeInAfterTravel();
				Logger.Log($"> {mansion.Name}: garage elevator -> mansion");
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				FadeInAfterTravel(300);
				Logger.Log($"Garage elevator: {ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		private static bool NearPoint(Quaternion q, Vector3 p)
		{
			if (IsSet(q) && Mansion.Dist2D(p, Helper.ToVector3(q)) < 1.5f)
			{
				return Math.Abs(p.Z - q.Z) < 1.5f;
			}
			return false;
		}

		private static bool PanelTick(Mansion m)
		{
			if (Helper.PP.IsInVehicle() || m.AssistPanels.Count == 0)
			{
				return false;
			}
			Vector3 position = Helper.PP.Position;
			foreach (Quaternion assistPanel in m.AssistPanels)
			{
				if (NearPoint(assistPanel, position))
				{
					if (!Help($"Press ~INPUT_CONTEXT~ to talk to {AssistantName(m)}."))
					{
						return false;
					}
					if (Game.IsControlJustReleased(GTA.Control.Context))
					{
						OpenAssistant(m);
					}
					return true;
				}
			}
			return false;
		}

		public static Keys ParseMansionShortcut(string value, Keys fallback)
		{
			string text = (value ?? "").Trim();
			if (text.Length == 0)
			{
				return fallback;
			}
			switch (text.ToUpperInvariant())
			{
			case "BILD RUNTER":
			case "BILDRUNTER":
			case "BILD AB":
			case "BILDAB":
			case "PAGEDOWN":
			case "PAGE DOWN":
			case "PGDN":
				text = "Next";
				break;
			case "Ä":
				text = "Oem7";
				break;
			case "Ü":
				text = "Oem1";
				break;
			}
			Keys result;
			if (Enum.TryParse<Keys>(text, true, out result) && Enum.IsDefined(typeof(Keys), result))
			{
				return result;
			}
			Logger.Log("Invalid mansion shortcut '" + value + "'; using " + fallback);
			return fallback;
		}

		private static void MansionMenuKeyTick()
		{
			int num;
			int num2;
			if (MansionMenuKey != 0)
			{
				num = (Game.IsKeyPressed(MansionMenuKey) ? 1 : 0);
				if (num != 0)
				{
					num2 = ((!mansionMenuShortcutWasDown) ? 1 : 0);
					goto IL_0022;
				}
			}
			else
			{
				num = 0;
			}
			num2 = 0;
			goto IL_0022;
			IL_0022:
			bool flag = (byte)num2 != 0;
			mansionMenuShortcutWasDown = (byte)num != 0;
			if (!flag || MansionMenuKey == AssistantKey || pendingAssistantMansion != null || Helper.MenuPool.IsAnyMenuOpen() || Game.IsLoading || Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE))
			{
				return;
			}
			Mansion mansion = Mansions.MansionList.Where((_Closure_0024__._0024I155_002D0 != null) ? _Closure_0024__._0024I155_002D0 : (_Closure_0024__._0024I155_002D0 = _Closure_0024__._0024I._Lambda_0024__155_002D0)).OrderBy((_Closure_0024__._0024I155_002D1 != null) ? _Closure_0024__._0024I155_002D1 : (_Closure_0024__._0024I155_002D1 = _Closure_0024__._0024I._Lambda_0024__155_002D1)).FirstOrDefault();
			if (mansion != null && !(Mansion.Dist2D(Helper.PP.Position, mansion.Reference) > 150f))
			{
				if (!mansion.IsOwnedByPlayer())
				{
					Notification.Show(mansion.Name + ": buy this mansion before opening its management menu.");
					return;
				}
				Mansions.currentMansion = mansion;
				Mansions.BuildMansionMenu(mansion);
				Mansions.MansionMenu.Visible = true;
				Logger.Log("Mansion management shortcut: " + MansionMenuKey.ToString() + "; " + mansion.Name);
			}
		}

		public static string AssistantKeyName()
		{
			if (AssistantKey == Keys.PageDown)
			{
				return "Bild runter (Page Down)";
			}
			if (AssistantKey == Keys.OemQuotes)
			{
				return "Ä";
			}
			return AssistantKey.ToString();
		}

		public static void AssistantInputTick()
		{
			MansionMenuKeyTick();
			AssistantKeyTick();
			if (pendingAssistantMansion != null || Helper.MenuPool.IsAnyMenuOpen() || Game.IsLoading || Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE))
			{
				contextDownAt = 0;
				return;
			}
			Mansion mansion = Mansions.MansionList.Where((_Closure_0024__._0024I159_002D0 != null) ? _Closure_0024__._0024I159_002D0 : (_Closure_0024__._0024I159_002D0 = _Closure_0024__._0024I._Lambda_0024__159_002D0)).OrderBy((_Closure_0024__._0024I159_002D1 != null) ? _Closure_0024__._0024I159_002D1 : (_Closure_0024__._0024I159_002D1 = _Closure_0024__._0024I._Lambda_0024__159_002D1)).FirstOrDefault();
			if (mansion != null)
			{
				HouseKeyTick(mansion);
			}
		}

		public static void AssistantKeyTick()
		{
			bool flag = Game.IsKeyPressed(AssistantKey);
			bool num = AssistantKeyRequested || (flag && !assistantShortcutWasDown);
			AssistantKeyRequested = false;
			assistantShortcutWasDown = flag;
			if (!num || pendingAssistantMansion != null || checked(Game.GameTime - lastAssistantShortcutAt) < 500)
			{
				return;
			}
			lastAssistantShortcutAt = Game.GameTime;
			try
			{
				_Closure_0024__160_002D0 closure_0024__160_002D = new _Closure_0024__160_002D0();
				if (Game.IsLoading || Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE))
				{
					return;
				}
				closure_0024__160_002D._0024VB_0024Local_p = Helper.PP.Position;
				Mansion mansion = Mansions.MansionList.Where((_Closure_0024__._0024I160_002D0 != null) ? _Closure_0024__._0024I160_002D0 : (_Closure_0024__._0024I160_002D0 = _Closure_0024__._0024I._Lambda_0024__160_002D0)).OrderBy(closure_0024__160_002D._Lambda_0024__1).FirstOrDefault();
				float num2 = ((mansion == null) ? (-1f) : Mansion.Dist2D(closure_0024__160_002D._0024VB_0024Local_p, mansion.Reference));
				Logger.Log(string.Format("> assistant key: nearest {0} {1:0} m, owner {2}, player {3}, menu open {4}", (mansion == null) ? "-" : mansion.Name, num2, (mansion == null) ? "-" : mansion.Owner.ToString(), Helper.GetPlayer(), Helper.MenuPool.IsAnyMenuOpen()));
				if (mansion != null && !(num2 > 150f) && !Helper.MenuPool.IsAnyMenuOpen())
				{
					if (!mansion.IsOwnedByPlayer())
					{
						Notification.Show(string.Format("~b~{0}~w~: this mansion belongs to {1}. You play {2}.", mansion.Name, (mansion.Owner == eOwner.Nobody) ? "nobody yet - buy it at the for-sale sign" : mansion.Owner.ToString(), Helper.GetPlayer()));
					}
					else
					{
						OpenAssistant(mansion);
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"AssistantKeyTick: {ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		private static void HouseKeyTick(Mansion m)
		{
			Vector3 position = Helper.PP.Position;
			if (!(Mansion.Dist2D(position, m.Reference) < 120f) || !(Math.Abs(position.Z - m.Reference.Z) < 80f))
			{
				if (houseInside == m)
				{
					houseInside = null;
				}
				contextDownAt = 0;
				return;
			}
			checked
			{
				if (houseInside != m)
				{
					houseInside = m;
					Logger.Log($"> {m.Name}: on site (owner={m.IsOwnedByPlayer()}, ipls={m.IplsLoaded}, sets={m.SetsApplied}, stops={m.ElevatorStops.Count})");
					if (m.IsOwnedByPlayer() && Game.GameTime - houseHintAt > 300000)
					{
						houseHintAt = Game.GameTime;
						GTA.UI.Screen.ShowHelpText($"{m.Name}: press {MansionMenuKey} for mansion management, {AssistantKeyName()} for the assistant, or hold then release ~INPUT_CONTEXT~ (controller) to call {AssistantName(m)}.", 10000, false);
					}
				}
				bool flag = false;
				if (!flag)
				{
					if (Game.IsControlPressed(GTA.Control.Context))
					{
						if (contextDownAt == 0)
						{
							contextDownAt = Game.GameTime;
						}
						if (Game.GameTime - contextDownAt > 900)
						{
							contextDownAt = 0;
							flag = true;
						}
					}
					else
					{
						contextDownAt = 0;
					}
				}
				if (flag)
				{
					Logger.Log($"> {m.Name}: assistant key");
					if (!m.IsOwnedByPlayer())
					{
						Notification.Show($"~b~{m.Name}~w~: you don't own this mansion yet - buy it at the for-sale sign at the gate or via Dynasty 8.");
					}
					else
					{
						OpenAssistant(m);
					}
				}
			}
		}

		private static bool StationTick(Mansion m)
		{
			if (Helper.PP.IsInVehicle())
			{
				return false;
			}
			Vector3 position = Helper.PP.Position;
			if (NearPoint(m.GymPos, position))
			{
				if (!Help("Press ~INPUT_CONTEXT~ to work out."))
				{
					return false;
				}
				if (Game.IsControlJustReleased(GTA.Control.Context))
				{
					BuildGymMenu();
					GymMenu.ParentMenu = null;
					GymMenu.RefreshIndex();
					GymMenu.Visible = true;
				}
				return true;
			}
			if (NearPoint(m.ArmoryPos, position))
			{
				if (!Help("Press ~INPUT_CONTEXT~ to open the armoury shop and workbench."))
				{
					return false;
				}
				if (Game.IsControlJustReleased(GTA.Control.Context))
				{
					MansionArmory.Open(m);
				}
				return true;
			}
			if (NearPoint(m.VaultPos, position))
			{
				if (!Help("Press ~INPUT_CONTEXT~ to check the vault."))
				{
					return false;
				}
				if (Game.IsControlJustReleased(GTA.Control.Context))
				{
					ShowVault(m);
				}
				return true;
			}
			return false;
		}

		public static bool ShowroomSelectionMatches(int expectedUid, int expectedHash, int actualUid, int actualHash, bool homeReady)
		{
			if (homeReady && expectedUid != 0 && expectedHash != 0 && expectedUid == actualUid)
			{
				return expectedHash == actualHash;
			}
			return false;
		}

		public static bool ShowroomModelAvailable(int modelHash, bool inCdImage, bool valid, bool vehicle)
		{
			if (modelHash != 0 && inCdImage && valid)
			{
				return vehicle;
			}
			return false;
		}

		private static bool InstalledShowroomVehicle(VehicleClass vc)
		{
			if (vc == null || vc.Hash == 0)
			{
				return false;
			}
			Model model = new Model(vc.Hash);
			return ShowroomModelAvailable(vc.Hash, model.IsInCdImage, model.IsValid, model.IsVehicle);
		}

		private static bool ClearPodiumSelection(Mansion m, bool notify)
		{
			if (m == null || !m.IsOwnedByPlayer())
			{
				return false;
			}
			int podiumUid = m.PodiumUid;
			if (podiumUid != 0)
			{
				m.PodiumUid = 0;
				if (!Mansions.SaveMansions())
				{
					m.PodiumUid = podiumUid;
					return false;
				}
			}
			podiumRetryAt.Remove(m);
			podiumFailure.Remove(m);
			DespawnPodium(m);
			if (notify)
			{
				Notification.Show($"{m.Name}: the showroom is empty. Saved vehicles and tuning remain in the garage records.");
			}
			return true;
		}

		private static void ReleasePodiumModelRequest(Mansion m)
		{
			PodiumModelRequest value = null;
			if (podiumModelRequests.TryGetValue(m, out value))
			{
				podiumModelRequests.Remove(m);
				if (value.Requested && value.ModelHash != 0)
				{
					new Model(value.ModelHash).MarkAsNoLongerNeeded();
				}
			}
		}

		private static void PodiumSpawnFailed(Mansion m, VehicleClass vc, string state, Model model)
		{
			ReleasePodiumModelRequest(m);
			podiumFailure[m] = state;
			checked
			{
				podiumRetryAt[m] = Game.GameTime + 30000;
				string text = $"hash=0x{(uint)(vc.Hash & 0xFFFFFFFFu):X8}; uid={vc.UniqueID}; state={state}; cdImage={model.IsInCdImage}; valid={model.IsValid}; loaded={model.IsLoaded}; vehicle={model.IsVehicle}";
				Logger.Log($"Podium {m.Name}: {vc.Make} {vc.Name}; {text}; selected saved record retained; retry in30s");
				if (PodiumMenu != null && PodiumMenu.Visible && podiumMansion == m)
				{
					string text2 = ((Operators.CompareString(state, "MODEL MISSING", false) == 0) ? "is not registered in this game session. Check that this saved car's add-on is enabled." : ((Operators.CompareString(state, "LOAD TIMEOUT", false) == 0) ? "did not finish loading. Try the saved car again in a moment." : "could not be created as a valid vehicle in this session."));
					Notification.Show($"{vc.Make} {vc.Name}: model0x{(uint)(vc.Hash & 0xFFFFFFFFu):X8} {text2} The saved car and tuning have been kept.");
				}
			}
		}

		public static string ShowCarStatus(bool hasSelection, bool liveDisplay, bool displayReady, bool displayVisible, bool failed)
		{
			if (!hasSelection)
			{
				return "EMPTY";
			}
			if (liveDisplay && displayReady && displayVisible)
			{
				return "SHOWN";
			}
			if (failed)
			{
				return "UNAVAILABLE";
			}
			if (liveDisplay)
			{
				return "LOADING";
			}
			return "SELECTED";
		}

		public static string PodiumDisplayStatus(Mansion m)
		{
			if (m == null || m.PodiumUid == 0)
			{
				return "EMPTY";
			}
			PodiumModelRequest value = null;
			if (podiumModelRequests.TryGetValue(m, out value) && value.UID == m.PodiumUid)
			{
				return "LOADING";
			}
			string value2 = null;
			int value3;
			if (podiumFailure.TryGetValue(m, out value2) && podiumRetryAt.TryGetValue(m, out value3) && value3 > Game.GameTime)
			{
				return value2;
			}
			PodiumDisplayState value4 = null;
			bool flag = podiumDisplays.TryGetValue(m, out value4);
			bool flag2 = (object)m.PodiumVeh != null && m.PodiumVeh.Exists() && m.PodiumVehUid == m.PodiumUid;
			int value5;
			bool failed = podiumRetryAt.TryGetValue(m, out value5) && value5 > Game.GameTime;
			return ShowCarStatus(true, flag2, flag && value4.Ready, flag2 && m.PodiumVeh.IsVisible, failed);
		}

		private static void UpdatePodiumMenuLabels(Mansion m)
		{
			if (PodiumMenu != null && PodiumMenu.Visible && podiumMansion == m)
			{
				string rightLabel = PodiumDisplayStatus(m);
				foreach (UIMenuItem menuItem in PodiumMenu.MenuItems)
				{
					if (menuItem.Tag is int && Conversions.ToInteger(menuItem.Tag) == m.PodiumUid)
					{
						menuItem.SetRightLabel(rightLabel);
					}
				}
			}
			if (Mansions.MansionMenu == null || !Mansions.MansionMenu.Visible || Mansions.currentMansion != m)
			{
				return;
			}
			foreach (UIMenuItem menuItem2 in Mansions.MansionMenu.MenuItems)
			{
				if (menuItem2.Tag is string && Operators.CompareString(Conversions.ToString(menuItem2.Tag), "Podium", false) == 0)
				{
					menuItem2.SetRightLabel(PodiumDisplayStatus(m));
				}
			}
		}

		private static bool ApplyPodiumPose(Mansion m, PodiumDisplayState display)
		{
			if ((object)m.PodiumVeh == null || !m.PodiumVeh.Exists())
			{
				return false;
			}
			Vector3 origin = default(Vector3);
			Quaternion rotation = default(Quaternion);
			if (!MansionShowroomPlacement.TryDisplayPose(display.Center, display.Normal, display.Minimum, display.Maximum, display.Heading, ref origin, ref rotation))
			{
				return false;
			}
			int handle = m.PodiumVeh.Handle;
			Function.Call(Hash.SET_ENTITY_QUATERNION, handle, rotation.X, rotation.Y, rotation.Z, rotation.W);
			Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, handle, origin.X, origin.Y, origin.Z, false, false, false);
			Function.Call(Hash.SET_ENTITY_VELOCITY, handle, 0f, 0f, 0f);
			return true;
		}

		private static void TickPodium(Mansion m)
		{
			PodiumDisplayState value = null;
			if (!podiumDisplays.TryGetValue(m, out value) || (object)m.PodiumVeh == null || !m.PodiumVeh.Exists())
			{
				return;
			}
			checked
			{
				if (!value.Ready)
				{
					if (Game.GameTime >= value.NextProbeAt)
					{
						value.NextProbeAt = Game.GameTime + 250;
						string source = null;
						Function.Call(Hash.REQUEST_COLLISION_AT_COORD, value.Point.X, value.Point.Y, value.Point.Z);
						Function.Call(Hash.REQUEST_VEHICLE_HIGH_DETAIL_MODEL, m.PodiumVeh.Handle);
						bool flag = MansionShowroomPlacement.TryFindSurface(value.Point, m.PodiumVeh, value.OriginalPlatform, ref value.Center, ref source, m, ref value.Normal);
						m.PodiumVeh.Model.GetDimensions(out value.Minimum, out value.Maximum);
						bool flag2 = MansionShowroomPlacement.BoundsAcceptable(value.Minimum, value.Maximum);
						bool flag3 = Function.Call<bool>(Hash.HAVE_VEHICLE_MODS_STREAMED_IN, m.PodiumVeh.Handle);
						if (MansionShowroomPlacement.CanRevealDisplay(flag2, flag, flag3, Game.GameTime - value.StartedAt) && ApplyPodiumPose(m, value))
						{
							value.Ready = true;
							m.PodiumVeh.IsVisible = true;
							Logger.Log($"> Podium {m.Name}: surface-aligned display, platform={source}, surface={value.Center}, normal={value.Normal}, modelMinZ={value.Minimum.Z:0.000}, modsReady={flag3}, boundedPartsFallback={!flag3}");
						}
						else if (Game.GameTime >= value.Deadline)
						{
							Logger.Log($"Podium {m.Name}: placement unavailable; surfaceReady={flag}, boundsReady={flag2}, modsReady={flag3}, point={value.Point}, min={value.Minimum}, max={value.Maximum}; surface={source}; retry in 30 s (saved car retained)");
							DespawnPodium(m);
							podiumRetryAt[m] = Game.GameTime + 30000;
						}
					}
				}
				else if (m.PodiumRotate)
				{
					float num = Math.Max(0f, Math.Min(0.1f, Game.LastFrameTime));
					value.Heading = (value.Heading + 8f * num) % 360f;
					ApplyPodiumPose(m, value);
				}
			}
		}

		private static void SpawnPodium(Mansion m)
		{
			_Closure_0024__180_002D0 closure_0024__180_002D = new _Closure_0024__180_002D0();
			closure_0024__180_002D._0024VB_0024Local_m = m;
			int value;
			if (podiumRetryAt.TryGetValue(closure_0024__180_002D._0024VB_0024Local_m, out value) && Game.GameTime < value)
			{
				return;
			}
			VehicleClass vehicleClass = Mansions.MansionGarageVehicles(closure_0024__180_002D._0024VB_0024Local_m).FirstOrDefault(closure_0024__180_002D._Lambda_0024__0);
			checked
			{
				if (vehicleClass == null || !InstalledShowroomVehicle(vehicleClass))
				{
					Logger.Log($"Podium {closure_0024__180_002D._0024VB_0024Local_m.Name}: selected vehicle {closure_0024__180_002D._0024VB_0024Local_m.PodiumUid} is no longer installed or saved; clearing the showroom assignment, garage XML retained");
					ClearPodiumSelection(closure_0024__180_002D._0024VB_0024Local_m, false);
				}
				else
				{
					if (Helper.IsGarageVehicleAlreadyExistInWorldMap(closure_0024__180_002D._0024VB_0024Local_m.GarageId, vehicleClass.UniqueID))
					{
						return;
					}
					closure_0024__180_002D._0024VB_0024Local_pending = null;
					if (podiumModelRequests.TryGetValue(closure_0024__180_002D._0024VB_0024Local_m, out closure_0024__180_002D._0024VB_0024Local_pending) && (closure_0024__180_002D._0024VB_0024Local_pending.UID != vehicleClass.UniqueID || closure_0024__180_002D._0024VB_0024Local_pending.ModelHash != vehicleClass.Hash))
					{
						ReleasePodiumModelRequest(closure_0024__180_002D._0024VB_0024Local_m);
						closure_0024__180_002D._0024VB_0024Local_pending = null;
					}
					if (closure_0024__180_002D._0024VB_0024Local_pending == null)
					{
						closure_0024__180_002D._0024VB_0024Local_pending = new PodiumModelRequest
						{
							UID = vehicleClass.UniqueID,
							ModelHash = vehicleClass.Hash,
							StartedAt = Game.GameTime
						};
						podiumModelRequests[closure_0024__180_002D._0024VB_0024Local_m] = closure_0024__180_002D._0024VB_0024Local_pending;
						podiumFailure.Remove(closure_0024__180_002D._0024VB_0024Local_m);
						Logger.Log($"Podium model requested: {closure_0024__180_002D._0024VB_0024Local_m.Key}; uid={vehicleClass.UniqueID}; hash=0x{(uint)(vehicleClass.Hash & 0xFFFFFFFFu):X8}; {vehicleClass.Make} {vehicleClass.Name}");
					}
					Model model = new Model(closure_0024__180_002D._0024VB_0024Local_pending.ModelHash);
					if (closure_0024__180_002D._0024VB_0024Local_pending.ModelHash != 0 && Game.GameTime >= closure_0024__180_002D._0024VB_0024Local_pending.NextRequestAt)
					{
						model.Request();
						closure_0024__180_002D._0024VB_0024Local_pending.Requested = true;
						closure_0024__180_002D._0024VB_0024Local_pending.NextRequestAt = Game.GameTime + 500;
					}
					long elapsed = (unchecked((long)Game.GameTime) - unchecked((long)closure_0024__180_002D._0024VB_0024Local_pending.StartedAt)) & 0xFFFFFFFFu;
					string text = MansionShowroomPlacement.ModelLoadState(closure_0024__180_002D._0024VB_0024Local_pending.ModelHash, model.IsInCdImage, model.IsValid, model.IsLoaded, model.IsVehicle, elapsed);
					if (Operators.CompareString(text, "LOADING", false) == 0)
					{
						return;
					}
					if (Operators.CompareString(text, "READY", false) != 0)
					{
						PodiumSpawnFailed(closure_0024__180_002D._0024VB_0024Local_m, vehicleClass, text, model);
						return;
					}
					VehicleClass vehicleClass2 = Mansions.MansionGarageVehicles(closure_0024__180_002D._0024VB_0024Local_m).FirstOrDefault(closure_0024__180_002D._Lambda_0024__1);
					if (vehicleClass2 == null || !ShowroomSelectionMatches(closure_0024__180_002D._0024VB_0024Local_pending.UID, closure_0024__180_002D._0024VB_0024Local_pending.ModelHash, closure_0024__180_002D._0024VB_0024Local_m.PodiumUid, vehicleClass2?.Hash ?? 0, closure_0024__180_002D._0024VB_0024Local_m.Enabled && closure_0024__180_002D._0024VB_0024Local_m.IplsLoaded && closure_0024__180_002D._0024VB_0024Local_m.IsOwnedByPlayer()))
					{
						ReleasePodiumModelRequest(closure_0024__180_002D._0024VB_0024Local_m);
						return;
					}
					vehicleClass = vehicleClass2;
					Quaternion quaternion = closure_0024__180_002D._0024VB_0024Local_m.PodiumPos;
					Quaternion q = MansionAutoLayout.GaragePoint(closure_0024__180_002D._0024VB_0024Local_m, 14.471f, -13.724f, 7.521f, 96.9188f);
					bool flag = Helper.ToVector3(quaternion).DistanceTo(Helper.ToVector3(q)) < 0.05f && HeadingDiff(quaternion.W, q.W) < 0.1f;
					if (flag)
					{
						quaternion = MansionAutoLayout.OriginalShowroomPoint(closure_0024__180_002D._0024VB_0024Local_m, vehicleClass.Hash);
					}
					int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, quaternion.X, quaternion.Y, quaternion.Z);
					if (num == 0 || Function.Call<bool>(Hash.IS_INTERIOR_READY, num))
					{
						MansionNativeGarage.ReleaseStoredVehicle(closure_0024__180_002D._0024VB_0024Local_m, vehicleClass.UniqueID);
						Vehicle vehicle = Helper.CreateGarageVehicle(vehicleClass, new Quaternion(quaternion.X, quaternion.Y, quaternion.Z + 0.6f, quaternion.W), closure_0024__180_002D._0024VB_0024Local_m.GarageId);
						if ((object)vehicle == null || !vehicle.Exists())
						{
							PodiumSpawnFailed(closure_0024__180_002D._0024VB_0024Local_m, vehicleClass, "CREATE FAILED", model);
							return;
						}
						ReleasePodiumModelRequest(closure_0024__180_002D._0024VB_0024Local_m);
						podiumRetryAt.Remove(closure_0024__180_002D._0024VB_0024Local_m);
						podiumFailure.Remove(closure_0024__180_002D._0024VB_0024Local_m);
						vehicle.IsVisible = false;
						Function.Call(Hash.SET_ENTITY_COLLISION, vehicle.Handle, false, false);
						Function.Call(Hash.FREEZE_ENTITY_POSITION, vehicle.Handle, true);
						Function.Call(Hash.SET_VEHICLE_DOORS_LOCKED, vehicle.Handle, 2);
						vehicle.IsInvincible = true;
						vehicle.IsEngineRunning = false;
						Function.Call(Hash.SET_VEHICLE_LIGHTS, vehicle.Handle, 1);
						closure_0024__180_002D._0024VB_0024Local_m.PodiumVeh = vehicle;
						closure_0024__180_002D._0024VB_0024Local_m.PodiumVehUid = vehicleClass.UniqueID;
						podiumDisplays[closure_0024__180_002D._0024VB_0024Local_m] = new PodiumDisplayState
						{
							Point = quaternion,
							OriginalPlatform = flag,
							Heading = quaternion.W,
							StartedAt = Game.GameTime,
							Deadline = Game.GameTime + 12000
						};
						TickPodium(closure_0024__180_002D._0024VB_0024Local_m);
						Logger.Log($"> Podium {closure_0024__180_002D._0024VB_0024Local_m.Name}: {vehicleClass.Make} {vehicleClass.Name}; uid={vehicleClass.UniqueID}; hash=0x{(uint)(vehicleClass.Hash & 0xFFFFFFFFu):X8}; entity={vehicle.Handle}; requested={Helper.ToVector3(quaternion)}; actual={vehicle.Position}");
					}
				}
			}
		}

		public static void DespawnPodium(Mansion m)
		{
			try
			{
				ReleasePodiumModelRequest(m);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Podium model release: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			try
			{
				if ((object)m.PodiumVeh != null && m.PodiumVeh.Exists())
				{
					m.PodiumVeh.Delete();
				}
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
			}
			m.PodiumVeh = null;
			m.PodiumVehUid = 0;
			if (podiumDisplays.Remove(m))
			{
				MansionShowroomPlacement.ReleaseSurfaceAssets();
			}
		}

		public static void SetPodium(Mansion m, int uid)
		{
			_Closure_0024__182_002D0 closure_0024__182_002D = new _Closure_0024__182_002D0();
			closure_0024__182_002D._0024VB_0024Local_uid = uid;
			if (m == null || !m.IsOwnedByPlayer())
			{
				return;
			}
			if (closure_0024__182_002D._0024VB_0024Local_uid == 0)
			{
				ClearPodiumSelection(m, true);
				return;
			}
			VehicleClass vehicleClass = Mansions.MansionGarageVehicles(m).FirstOrDefault(closure_0024__182_002D._Lambda_0024__0);
			if (vehicleClass == null || !InstalledShowroomVehicle(vehicleClass))
			{
				Notification.Show("This saved vehicle's model is no longer installed. Its garage record and tuning have been kept.");
				return;
			}
			MansionAutoLayout.ApplyDefaults(m);
			if (closure_0024__182_002D._0024VB_0024Local_uid != 0 && Helper.IsGarageVehicleAlreadyExistInWorldMap(m.GarageId, closure_0024__182_002D._0024VB_0024Local_uid))
			{
				Notification.Show("This vehicle is out. Use 'Display the car I am driving' or return it to the mansion garage.");
				return;
			}
			if (closure_0024__182_002D._0024VB_0024Local_uid != 0 && (object)m.WorkshopVeh != null && m.WorkshopVeh.Exists() && m.WorkshopUid == closure_0024__182_002D._0024VB_0024Local_uid)
			{
				Notification.Show("Finish the workshop first: Pay & save, Discard changes, or send the car back to the garage. Your tuning has been kept.");
				return;
			}
			int podiumUid = m.PodiumUid;
			m.PodiumUid = closure_0024__182_002D._0024VB_0024Local_uid;
			if (!Mansions.SaveMansions())
			{
				m.PodiumUid = podiumUid;
				return;
			}
			podiumRetryAt.Remove(m);
			podiumFailure.Remove(m);
			DespawnPodium(m);
			if (closure_0024__182_002D._0024VB_0024Local_uid != 0)
			{
				MansionNativeGarage.ReleaseStoredVehicle(m, closure_0024__182_002D._0024VB_0024Local_uid);
			}
			if (closure_0024__182_002D._0024VB_0024Local_uid != 0 && m.IplsLoaded && IsSet(m.PodiumPos))
			{
				SpawnPodium(m);
			}
			MansionNativeGarage.Refresh(m);
			if (closure_0024__182_002D._0024VB_0024Local_uid == 0)
			{
				Notification.Show($"{m.Name}: the showroom is empty; the previous show car is back in its saved garage slot.");
			}
			else if ((object)m.PodiumVeh != null && m.PodiumVeh.Exists() && m.PodiumVeh.IsVisible)
			{
				Notification.Show($"{m.Name}: the show car is now in the villa showroom.");
			}
			else
			{
				Notification.Show($"{m.Name}: show car selected. Preparing the showroom display.");
			}
		}

		public static void SetPodiumFromGarage(int aptId, int uid)
		{
			Mansion mansion = MansionForGarageId(aptId);
			if (mansion != null)
			{
				SetPodium(mansion, uid);
			}
		}

		private static void DisplayCurrentCar(Mansion m)
		{
			if (!AtMansion(m) || !m.IsOwnedByPlayer() || !Helper.PP.IsInVehicle())
			{
				return;
			}
			Vehicle currentVehicle = Helper.PP.CurrentVehicle;
			if ((object)currentVehicle == null || !currentVehicle.Exists() || (object)currentVehicle.Driver == null || currentVehicle.Driver.Handle != Helper.PP.Handle)
			{
				Notification.Show("Sit in the driver's seat of the car you want to display.");
				return;
			}
			if (currentVehicle.Model.IsHelicopter || currentVehicle.Model.IsPlane || currentVehicle.Model.IsBoat || currentVehicle.Model.IsTrain)
			{
				Notification.Show("Only vehicles accepted by the mansion garage can be displayed in the showroom.");
				return;
			}
			if ((object)m.WorkshopVeh != null && m.WorkshopVeh.Exists() && m.WorkshopVeh.Handle == currentVehicle.Handle)
			{
				Notification.Show("Finish the workshop first. Unsaved tuning will not be discarded by the showroom service.");
				return;
			}
			MansionAutoLayout.ApplyDefaults(m);
			if (!IsSet(m.PodiumPos) || !Mansions.PrepareMansionNow(m))
			{
				Notification.Show("The showroom is still loading. Your current car has been kept.");
				return;
			}
			int committedUid = 0;
			if (MansionNativeGarage.StoreVehicle(m, currentVehicle, ref committedUid))
			{
				SetPodium(m, MansionNativeGarage.LastStoredUid);
			}
		}

		private static void BuildPodiumMenu(Mansion m)
		{
			_Closure_0024__185_002D0 arg = new _Closure_0024__185_002D0(null);
			arg._0024VB_0024Local_m = m;
			MansionAutoLayout.ApplyDefaults(arg._0024VB_0024Local_m);
			PodiumMenu.Clear();
			PodiumMenu.Subtitle.Caption = $"SHOWROOM - {arg._0024VB_0024Local_m.Name.ToUpper()}";
			List<VehicleClass> list = Mansions.MansionGarageVehicles(arg._0024VB_0024Local_m).Where((_Closure_0024__._0024I185_002D0 != null) ? _Closure_0024__._0024I185_002D0 : (_Closure_0024__._0024I185_002D0 = _Closure_0024__._0024I._Lambda_0024__185_002D0)).ToList();
			if (arg._0024VB_0024Local_m.PodiumUid != 0 && !list.Any(arg._Lambda_0024__1))
			{
				ClearPodiumSelection(arg._0024VB_0024Local_m, false);
			}
			bool enabled = AtMansion(arg._0024VB_0024Local_m) && Helper.PP.IsInVehicle() && (object)Helper.PP.CurrentVehicle != null && (object)Helper.PP.CurrentVehicle.Driver != null && Helper.PP.CurrentVehicle.Driver.Handle == Helper.PP.Handle && !Helper.PP.CurrentVehicle.Model.IsHelicopter && !Helper.PP.CurrentVehicle.Model.IsPlane && !Helper.PP.CurrentVehicle.Model.IsBoat && !Helper.PP.CurrentVehicle.Model.IsTrain;
			PodiumMenu.AddItem(new UIMenuItem("Display the car I am driving", "Save the current car in this mansion garage and move it to the villa showroom. Uses one garage slot; saved tuning is preserved.")
			{
				Tag = "CurrentCar",
				Enabled = enabled
			});
			UIMenuItem uIMenuItem = new UIMenuItem("Empty showroom / return show car", "Clear the display even if its add-on model is missing. Saved garage records and tuning are kept.")
			{
				Tag = 0
			};
			if (arg._0024VB_0024Local_m.PodiumUid == 0)
			{
				uIMenuItem.SetRightLabel("EMPTY");
			}
			PodiumMenu.AddItem(uIMenuItem);
			foreach (VehicleClass item in list)
			{
				bool flag = Helper.IsGarageVehicleAlreadyExistInWorldMap(arg._0024VB_0024Local_m.GarageId, item.UniqueID);
				bool flag2 = (object)arg._0024VB_0024Local_m.WorkshopVeh != null && arg._0024VB_0024Local_m.WorkshopVeh.Exists() && arg._0024VB_0024Local_m.WorkshopUid == item.UniqueID;
				string description = (flag ? "This vehicle is out. Drive it here and use 'Display the car I am driving'." : (flag2 ? "On the workshop lift. Finish there first: save, discard changes, or return it to the garage." : "Move this saved car to the villa showroom. Its garage record and tuning are preserved."));
				UIMenuItem uIMenuItem2 = new UIMenuItem($"{item.Make} {item.Name}", description)
				{
					Tag = item.UniqueID,
					Enabled = (!flag && !flag2)
				};
				if (item.UniqueID == arg._0024VB_0024Local_m.PodiumUid)
				{
					uIMenuItem2.SetRightLabel(PodiumDisplayStatus(arg._0024VB_0024Local_m));
				}
				else if (flag2)
				{
					uIMenuItem2.SetRightLabel("LIFT");
				}
				PodiumMenu.AddItem(uIMenuItem2);
			}
			PodiumMenu.AddItem(new UIMenuCheckboxItem("Rotate slowly", arg._0024VB_0024Local_m.PodiumRotate, "The show car turns slowly on the showroom platform."));
		}

		private static void PodiumMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			Mansion mansion = podiumMansion;
			if (mansion == null || selectedItem is UIMenuCheckboxItem)
			{
				return;
			}
			try
			{
				if (selectedItem.Tag is string && Operators.CompareString(Conversions.ToString(selectedItem.Tag), "CurrentCar", false) == 0)
				{
					DisplayCurrentCar(mansion);
				}
				else
				{
					SetPodium(mansion, Conversions.ToInteger(selectedItem.Tag));
				}
				BuildPodiumMenu(mansion);
				if (PodiumMenu.MenuItems.Count > 0)
				{
					PodiumMenu.CurrentSelection = Math.Min(index, checked(PodiumMenu.MenuItems.Count - 1));
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Showroom selection: {ex2.Message} {ex2.StackTrace}");
				Notification.Show("The showroom action could not be completed. Saved cars remain in the garage.");
				ProjectData.ClearProjectError();
			}
		}

		private static void PodiumMenu_OnCheckboxChange(UIMenu sender, UIMenuCheckboxItem checkboxItem, bool @checked)
		{
			Mansion mansion = podiumMansion;
			if (mansion == null)
			{
				return;
			}
			bool podiumRotate = mansion.PodiumRotate;
			mansion.PodiumRotate = @checked;
			if (!Mansions.SaveMansions())
			{
				mansion.PodiumRotate = podiumRotate;
				checkboxItem.Checked = podiumRotate;
			}
			else
			{
				if (@checked)
				{
					return;
				}
				PodiumDisplayState value = null;
				if (podiumDisplays.TryGetValue(mansion, out value))
				{
					value.Heading = value.Point.W;
					if (value.Ready)
					{
						ApplyPodiumPose(mansion, value);
					}
				}
			}
		}

		public static void OpenPodiumMenu(Mansion m, UIMenu parent)
		{
			podiumMansion = m;
			BuildPodiumMenu(m);
			OpenChild(parent, PodiumMenu);
		}

		private static bool WorkshopTick(Mansion m)
		{
			if (!IsSet(m.WorkshopPos) || Helper.PP.IsInVehicle())
			{
				return false;
			}
			if ((object)m.WorkshopVeh != null && m.WorkshopVeh.Exists())
			{
				if (Helper.PP.Position.DistanceTo(m.WorkshopVeh.Position) < 5f)
				{
					if (!Help($"Press ~INPUT_CONTEXT~ to customize the {m.WorkshopVc.Make} {m.WorkshopVc.Name}."))
					{
						return false;
					}
					if (Game.IsControlJustReleased(GTA.Control.Context))
					{
						MansionWorkshop.OpenModMenu(m);
					}
					return true;
				}
				return false;
			}
			Vector3 b = Helper.ToVector3(m.WorkshopPos);
			if (Mansion.Dist2D(Helper.PP.Position, b) < 3.5f && Math.Abs(Helper.PP.Position.Z - b.Z) < 2.5f)
			{
				if (!Help("Press ~INPUT_CONTEXT~ to bring a vehicle from the garage onto the lift."))
				{
					return false;
				}
				if (Game.IsControlJustReleased(GTA.Control.Context))
				{
					MansionWorkshopTravel.OpenWorkshopHere(m);
				}
				return true;
			}
			return false;
		}

		private static void BuildWorkshopPickMenu(Mansion m)
		{
			WorkshopPickMenu.Clear();
			WorkshopPickMenu.Subtitle.Caption = "WORKSHOP - CHOOSE A VEHICLE";
			foreach (VehicleClass item in Mansions.MansionGarageVehicles(m))
			{
				bool flag = Helper.IsGarageVehicleAlreadyExistInWorldMap(m.GarageId, item.UniqueID);
				bool flag2 = IsVehicleBusy(m, item.UniqueID);
				UIMenuItem uIMenuItem = new UIMenuItem($"{item.Make} {item.Name}", flag ? "This vehicle is out - park it in the garage first." : (flag2 ? "On the podium - choose another show car first." : "Bring it onto the lift."))
				{
					Tag = item,
					Enabled = (!flag && !flag2)
				};
				uIMenuItem.SetRightLabel(item.PlateNumber);
				WorkshopPickMenu.AddItem(uIMenuItem);
			}
			if (WorkshopPickMenu.MenuItems.Count == 0)
			{
				WorkshopPickMenu.AddItem(new UIMenuItem("(the mansion garage is empty)", "Drive a car into the garage first.")
				{
					Enabled = false
				});
			}
		}

		private static void WorkshopPickMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			Mansion mansion = workshopMansion;
			VehicleClass vehicleClass = selectedItem.Tag as VehicleClass;
			if (mansion != null && vehicleClass != null)
			{
				WorkshopPickMenu.Visible = false;
				Helper.FadeScreen(1, 400);
				MovePlayerOffLift(mansion);
				SpawnWorkshopVehicle(mansion, vehicleClass);
				Helper.FadeScreen(0, 400);
			}
		}

		private static void MovePlayerOffLift(Mansion m)
		{
			try
			{
				Vector3 b = Helper.ToVector3(m.WorkshopPos);
				Vector3 position = Helper.PP.Position;
				if (!(Mansion.Dist2D(position, b) > 3.2f))
				{
					Vector3 vector = new Vector3(position.X - b.X, position.Y - b.Y, 0f);
					if (vector.Length() < 0.5f)
					{
						double num = (double)m.WorkshopPos.W * Math.PI / 180.0;
						vector = new Vector3((float)(0.0 - Math.Sin(num)), (float)Math.Cos(num), 0f);
					}
					vector.Normalize();
					Vector3 vector2 = new Vector3(b.X + vector.X * 4f, b.Y + vector.Y * 4f, position.Z);
					Vector3 vector3 = new Vector3(0f, 0f, 0.5f);
					if (!World.Raycast(position + vector3, vector2 + vector3, IntersectFlags.Map, Helper.PP).DidHit)
					{
						Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, Helper.PP.Handle, vector2.X, vector2.Y, vector2.Z, false, false, false);
						Helper.PP.Heading = (float)(Math.Atan2(0f - (b.X - vector2.X), b.Y - vector2.Y) * 180.0 / Math.PI);
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"MovePlayerOffLift: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
		}

		internal static void SpawnWorkshopVehicle(Mansion m, VehicleClass vc, Func<bool> canContinue = null)
		{
			try
			{
				if (m == null || vc == null || !m.IsOwnedByPlayer() || (canContinue != null && (!canContinue() || !new Model(vc.Hash).IsLoaded)))
				{
					return;
				}
				if (vc.UniqueID == m.PodiumUid)
				{
					Notification.Show("Return this show car to the garage before moving it onto the workshop lift.");
					return;
				}
				if ((object)m.WorkshopVeh != null && m.WorkshopVeh.Exists() && m.WorkshopUid != vc.UniqueID)
				{
					Notification.Show("Finish the current workshop car first: Pay & save, Discard changes, or return it to the garage. Your tuning has been kept.");
					return;
				}
				MansionAutoLayout.ApplyDefaults(m);
				if (canContinue == null || canContinue())
				{
					RemoveWorkshopVehicle(m);
					MansionNativeGarage.ReleaseStoredVehicle(m, vc.UniqueID);
					Quaternion workshopPos = m.WorkshopPos;
					Vehicle vehicle = Helper.CreateGarageVehicle(vc, new Quaternion(workshopPos.X, workshopPos.Y, workshopPos.Z + 0.6f, workshopPos.W), m.GarageId, canContinue);
					if ((object)vehicle == null || !vehicle.Exists())
					{
						Notification.Show("Vehicle model could not be loaded (addon car missing?).");
						return;
					}
					Function.Call(Hash.SET_VEHICLE_ON_GROUND_PROPERLY, vehicle.Handle, 5f);
					Function.Call(Hash.FREEZE_ENTITY_POSITION, vehicle.Handle, true);
					Function.Call(Hash.SET_VEHICLE_DOORS_LOCKED, vehicle.Handle, 2);
					vehicle.IsEngineRunning = false;
					m.WorkshopVeh = vehicle;
					m.WorkshopVc = vc;
					m.WorkshopUid = vc.UniqueID;
					Logger.Log($"> Workshop {m.Name}: {vc.Make} {vc.Name} on the lift");
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"Workshop spawn: {ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		public static void RemoveWorkshopVehicle(Mansion m)
		{
			try
			{
				if ((object)m.WorkshopVeh != null && m.WorkshopVeh.Exists())
				{
					m.WorkshopVeh.Delete();
				}
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
			}
			m.WorkshopVeh = null;
			m.WorkshopVc = null;
			m.WorkshopUid = 0;
			if (MansionWorkshop.modM != m)
			{
				return;
			}
			UIMenu[] array = new UIMenu[6] { ModMenu, ModPerfMenu, ModBodyMenu, ModWheelMenu, ModPaintMenu, ModLightMenu };
			foreach (UIMenu uIMenu in array)
			{
				if (uIMenu != null)
				{
					uIMenu.Visible = false;
				}
			}
			MansionWorkshop.modM = null;
			MansionWorkshop.modVeh = null;
		}
	}
}
