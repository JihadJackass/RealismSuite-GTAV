using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using INMNativeUI;
using Metadata;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using SPAII.INM;

namespace SPAII
{
	[StandardModule]
	internal static class Helper
	{
		[CompilerGenerated]
		internal sealed class _Closure_0024__114_002D0
		{
			public Vector3 _0024VB_0024Local_pos;

			[SpecialName]
			internal float _Lambda_0024__0(Vector5 x)
			{
				return Math.Abs(x.Vector3.DistanceToSquared(_0024VB_0024Local_pos));
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__145_002D0
		{
			public eOwner _0024VB_0024Local_ow;

			[SpecialName]
			internal bool _Lambda_0024__0(ApartmentClass o)
			{
				return o.Owner() == _0024VB_0024Local_ow;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__88_002D0
		{
			public int _0024VB_0024Local_aid;

			public int _0024VB_0024Local_uid;

			[SpecialName]
			internal bool _Lambda_0024__0(Vehicle x)
			{
				if ((object)x != null && x.Exists() && x.GetInt(vehIdDecor) == _0024VB_0024Local_aid)
				{
					return x.GetInt(vehUidDecor) == _0024VB_0024Local_uid;
				}
				return false;
			}
		}

		public static bool HideHud = false;

		private static int persistentHelpFrame = int.MinValue;

		private static string persistentHelpText;

		public static string grgXmlPath = ".\\scripts\\SPA II\\Garages\\";

		public static string soundPath = ".\\scripts\\SPA II\\Sounds\\";

		public static string nitroModDecor = "inm_nitro_active";

		public static string vehIdDecor = "inm_spa2_id";

		public static string vehUidDecor = "inm_spa2_uid";

		public static List<BuildingClass> buildings = new List<BuildingClass>();

		public static List<ApartmentClass> apartments = new List<ApartmentClass>();

		public static bool buildingsLoaded = false;

		public static bool forSaleSignSpawned = false;

		public static List<Vehicle> outVehicleList = new List<Vehicle>();

		private static readonly Dictionary<string, eOwner> outsideVehicleOwners = new Dictionary<string, eOwner>(StringComparer.Ordinal);

		public static string debug3rdLine = "Nothing to show now";

		public static Ped PP;

		public static Vehicle LV;

		public static int PM;

		public static Player Player;

		public static int PI;

		public static MenuPool MenuPool = new MenuPool();

		public static UIResRectangle MenuBanner = new UIResRectangle(Point.Empty, new Size(0, 0), Color.FromArgb(0, 0, 0, 0));

		public static INMNativeUI.Sprite MichaelBanner = new INMNativeUI.Sprite("shopui_title_graphics_michael", "shopui_title_graphics_michael", default(Point), default(Size));

		public static INMNativeUI.Sprite FranklinBanner = new INMNativeUI.Sprite("shopui_title_graphics_franklin", "shopui_title_graphics_franklin", default(Point), default(Size));

		public static INMNativeUI.Sprite TrevorBanner = new INMNativeUI.Sprite("shopui_title_graphics_trevor", "shopui_title_graphics_trevor", default(Point), default(Size));

		public static Vector3 TwentyCarGarage = new Vector3(0f, 0f, 0f);

		public static Vector3 GarageMarkerIndicator = new Vector3(0f, 0f, 0f);

		public static string ForSaleSign = "prop_forsale_dyn_01";

		public static Speech OnTheClock = new Speech("mechanic_on_the_clock_some_wheels.wav");

		public static Speech YouNeedSomething = new Speech("mechanic_u_need_something_huh.wav");

		public static Speech ICantGetYourRide = new Speech("mechanic_cant_get_your_ride.wav");

		public static Speech IllGetBackToWork = new Speech("mechanic_get_back_to_work_then.wav");

		public static Speech IllBeThere = new Speech("mechanic_get_there_as_soon_as_i_can.wav");

		private static readonly Dictionary<string, long> additionalTextAttempts = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);

		public static bool mpMapActive = false;

		private static int mpMapLastCheck = 0;

		private static int mpMapFarSince = 0;

		private static int mpMapOnAt = 0;

		private static bool mpLoadedAdditive;

		public static int ShopControllerGlobal = 0;

		public static Scaleform StatScaleform0 => Stability.StatSf(0);

		public static Scaleform StatScaleform1 => Stability.StatSf(1);

		public static Scaleform StatScaleform2 => Stability.StatSf(2);

		public static Scaleform StatScaleform3 => Stability.StatSf(3);

		public static Scaleform StatScaleform4 => Stability.StatSf(4);

		public static Scaleform StatScaleform5 => Stability.StatSf(5);

		public static Scaleform StatScaleform6 => Stability.StatSf(6);

		public static Scaleform StatScaleform7 => Stability.StatSf(7);

		public static Scaleform StatScaleform8 => Stability.StatSf(8);

		public static Scaleform StatScaleform9 => Stability.StatSf(9);

		public static bool MayPresentPersistentHelp(bool anyHelpVisible, bool exactCurrentVisible, bool exactPreviousVisible, bool renderedThisFrame)
		{
			if (!renderedThisFrame)
			{
				if (anyHelpVisible && !exactCurrentVisible)
				{
					return exactPreviousVisible;
				}
				return true;
			}
			return false;
		}

		public static string[] PersistentHelpComponents(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return new string[0];
			}
			List<string> list = new List<string>();
			int num = 0;
			int i = 0;
			int num2 = 0;
			checked
			{
				int num3;
				for (; i < text.Length; i += num3)
				{
					num3 = ((!char.IsSurrogatePair(text, i)) ? 1 : 2);
					int byteCount = Encoding.UTF8.GetByteCount(text.Substring(i, num3));
					if (num2 + byteCount > 99)
					{
						list.Add(text.Substring(num, i - num));
						num = i;
						num2 = 0;
					}
					num2 += byteCount;
				}
				list.Add(text.Substring(num));
				return list.ToArray();
			}
		}

		private static void AppendPersistentHelp(string text)
		{
			string[] array = PersistentHelpComponents(text);
			foreach (string text2 in array)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text2);
			}
		}

		private static bool ExactPersistentHelpVisible(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			Function.Call(Hash.BEGIN_TEXT_COMMAND_IS_THIS_HELP_MESSAGE_BEING_DISPLAYED, "CELL_EMAIL_BCON");
			AppendPersistentHelp(text);
			return Function.Call<int>(Hash.END_TEXT_COMMAND_IS_THIS_HELP_MESSAGE_BEING_DISPLAYED, 0) != 0;
		}

		private static void RenderPersistentHelp(string text)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			AppendPersistentHelp(text);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, false, false, 1);
		}

		public static bool ShowPersistentHelp(string text, bool compatibilityBeep = false)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				return false;
			}
			int num = Function.Call<int>(Hash.GET_FRAME_COUNT);
			if (num == persistentHelpFrame)
			{
				return false;
			}
			int num2;
			int num3;
			if (Function.Call<int>(Hash.IS_HELP_MESSAGE_BEING_DISPLAYED) == 0)
			{
				num2 = ((Function.Call<int>(Hash.IS_HELP_MESSAGE_ON_SCREEN) != 0) ? 1 : 0);
				if (num2 == 0)
				{
					num3 = 0;
					goto IL_005a;
				}
			}
			else
			{
				num2 = 1;
			}
			num3 = (ExactPersistentHelpVisible(text) ? 1 : 0);
			goto IL_005a;
			IL_005a:
			bool exactCurrentVisible = (byte)num3 != 0;
			bool exactPreviousVisible = num2 != 0 && !string.Equals(text, persistentHelpText, StringComparison.Ordinal) && ExactPersistentHelpVisible(persistentHelpText);
			if (!MayPresentPersistentHelp((byte)num2 != 0, exactCurrentVisible, exactPreviousVisible, false))
			{
				return false;
			}
			RenderPersistentHelp(text);
			persistentHelpFrame = num;
			persistentHelpText = text;
			return true;
		}

		public static bool MayClaimApartmentBedHelp(bool ownedActualBed, bool exactNativeBedHelp, bool renderedThisFrame)
		{
			if (ownedActualBed && exactNativeBedHelp)
			{
				return !renderedThisFrame;
			}
			return false;
		}

		public static bool ShowApartmentBedHelp(string text, bool ownedActualBed)
		{
			return ShowOwnedHomeHelp(text, "SA_BED_IN", ownedActualBed);
		}

		public static bool ShowOwnedHomeHelp(string text, string nativeLabel, bool ownedActualStation)
		{
			if (ownedActualStation && !string.IsNullOrWhiteSpace(text))
			{
				switch (nativeLabel)
				{
				case "SA_BED_IN":
				case "WARD_TRIG":
				case "HOTB_SIT":
				case "HOTB_LEAN":
				case "MPTV_GRGE":
				case "MPTV_WALK":
				case "MPTV_WALKOFF":
				case "MPTV_SEAT":
				case "MPTV_SEAT_KM":
				{
					if (ShowPersistentHelp(text))
					{
						return true;
					}
					int num = Function.Call<int>(Hash.GET_FRAME_COUNT);
					Function.Call(Hash.BEGIN_TEXT_COMMAND_IS_THIS_HELP_MESSAGE_BEING_DISPLAYED, nativeLabel);
					bool exactNativeBedHelp = Function.Call<int>(Hash.END_TEXT_COMMAND_IS_THIS_HELP_MESSAGE_BEING_DISPLAYED, 0) != 0;
					if (!MayClaimApartmentBedHelp(ownedActualStation, exactNativeBedHelp, num == persistentHelpFrame))
					{
						return false;
					}
					RenderPersistentHelp(text);
					persistentHelpFrame = num;
					persistentHelpText = text;
					return true;
				}
				}
			}
			return false;
		}

		public static void ResetPersistentHelp()
		{
			persistentHelpFrame = int.MinValue;
			persistentHelpText = null;
		}

		public static string Make(this Vehicle vehicle)
		{
			return Game.GetLocalizedString(Function.Call<string>(INMNative._GET_MAKE_NAME_FROM_VEHICLE_MODEL, vehicle.Model.Hash));
		}

		public static bool WheelsVariation(this Vehicle vehicle)
		{
			return Function.Call<bool>(Hash.GET_VEHICLE_MOD_VARIATION, vehicle.Handle, 23);
		}

		public static Color ToColor(this VsColor vs)
		{
			return Color.FromArgb(vs.Red, vs.Green, vs.Blue);
		}

		public static VsColor ToVsColor(this Color col)
		{
			return new VsColor(col.R, col.G, col.B);
		}

		public static int Livery2(this Vehicle veh)
		{
			return Function.Call<int>(INMNative._GET_VEHICLE_ROOF_LIVERY, veh.Handle);
		}

		public static void Livery2(this Vehicle veh, int liv)
		{
			Function.Call(INMNative._SET_VEHICLE_ROOF_LIVERY, veh.Handle, liv);
		}

		public static int XenonHeadlightsColor(this Vehicle veh)
		{
			return Function.Call<int>(INMNative._GET_VEHICLE_XENON_LIGHTS_COLOR, veh.Handle);
		}

		public static void XenonHeadlightsColor(this Vehicle veh, int colorID)
		{
			Function.Call(INMNative._SET_VEHICLE_XENON_LIGHTS_COLOR, veh.Handle, colorID);
		}

		public static int IsNitroModInstalled()
		{
			return 0 - (Decor.Registered(nitroModDecor, Decor.eDecorType.Int) ? 1 : 0);
		}

		public static eOwner GetPlayer()
		{
			switch (Game.Player.Character.Model.GetHashCode())
			{
			case 225514697:
				return eOwner.Michael;
			case -1692214353:
				return eOwner.Franklin;
			case -1686040670:
				return eOwner.Trevor;
			default:
				return eOwner.Others;
			}
		}

		public static void UpdateApartmentOwner(this ApartmentClass apt, bool tradeIn = false)
		{
			ConfigHelper.config.SetValue("BUILDING", apt.Name, tradeIn ? eOwner.Nobody : GetPlayer());
			ConfigHelper.config.Save();
			ConfigHelper.config = ScriptSettings.Load("scripts\\SPA II\\modconfig.ini");
		}

		public static void FadeScreen(int inOut, int duration = 1000, bool _hidehud = false)
		{
			if (_hidehud)
			{
				HideHud = _hidehud;
			}
			if (inOut == 1)
			{
				Screen.FadeOut(duration);
			}
			else
			{
				Screen.FadeIn(duration);
			}
			Script.Wait(duration);
			if (_hidehud)
			{
				HideHud = !_hidehud;
			}
		}

		public static void PlayPropertyPurchase(string aptName)
		{
			Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "PROPERTY_PURCHASE", "HUD_AWARDS", false);
			BigMessageThread.MessageInstance.ShowWeaponPurchasedMessage("~y~" + Game.GetLocalizedString("PROPR_PURCHASED"), "~w~" + SouthLSHousing.PropertyText(aptName), (WeaponHash)0u);
		}

		public static void SetInteriorActive(this Vector3 coord)
		{
			try
			{
				int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, coord.X, coord.Y, coord.Z);
				if (num != 0)
				{
					Stability.PinInterior(num, coord);
					Function.Call(Hash.SET_INTERIOR_ACTIVE, num, true);
					Function.Call(Hash.DISABLE_INTERIOR, num, false);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"{ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		public static Vector3 ToVector3(this Quaternion q, bool placeOnGround = false)
		{
			return new Vector3(q.X, q.Y, placeOnGround ? World.GetGroundHeight(new Vector2(q.X, q.Y)) : q.Z);
		}

		public static int GetHashKey(this string str)
		{
			return Function.Call<int>(Hash.GET_HASH_KEY, str);
		}

		public static Vector3 ToRotation(this float heading)
		{
			return new Vector3(0f, 0f, heading);
		}

		public static Vehicle CreateGarageVehicle(this VehicleClass vehClass, Quaternion pos, int aptID, Func<bool> canContinue = null, Action<Vehicle> onCreated = null)
		{
			Vehicle vehicle = null;
			Model model = new Model(vehClass.Hash);
			if (canContinue != null && !canContinue())
			{
				return null;
			}
			if (model.IsInCdImage && model.IsValid)
			{
				if (canContinue != null)
				{
					if (!model.IsLoaded || !canContinue())
					{
						return null;
					}
					vehicle = Entity.FromHandle(Function.Call<int>(Hash.CREATE_VEHICLE, model.Hash, pos.X, pos.Y, pos.Z, pos.W, false, false)) as Vehicle;
				}
				else
				{
					if (!model.Request(5000))
					{
						Logger.Log($"CreateGarageVehicle: model {vehClass.Hash} ({vehClass.Name}) did not load");
						model.MarkAsNoLongerNeeded();
						return null;
					}
					vehicle = CreateVehicle(vehClass.Hash, ToVector3(pos), pos.W);
				}
				if ((object)vehicle == null || !vehicle.Exists())
				{
					Logger.Log($"CreateGarageVehicle: vehicle {vehClass.Name} could not be created");
					if (canContinue == null)
					{
						model.MarkAsNoLongerNeeded();
					}
					return null;
				}
				onCreated?.Invoke(vehicle);
				Vehicle vehicle2 = vehicle;
				V2Compat.InstallModKit(vehicle2);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Speakers, vehClass.Speakers, true);
				vehicle2.Mods.WheelType = (VehicleWheelType)vehClass.WheelType;
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Aerials, vehClass.Aerials, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Suspension, vehClass.Suspension, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Brakes, vehClass.Brakes, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Engine, vehClass.Engine, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Transmission, vehClass.Transmission, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Armor, vehClass.Armor, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.FrontBumper, vehClass.FrontBumper, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.RearBumper, vehClass.RearBumper, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.SideSkirt, vehClass.SideSkirt, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Trim, vehClass.Trim, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.EngineBlock, vehClass.EngineBlock, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.AirFilter, vehClass.AirFilter, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Struts, vehClass.Struts, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.ColumnShifterLevers, vehClass.ColumnShifterLevers, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Dashboard, vehClass.Dashboard, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.DialDesign, vehClass.DialDesign, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Ornaments, vehClass.Ornaments, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Seats, vehClass.Seats, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.SteeringWheels, vehClass.SteeringWheel, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.TrimDesign, vehClass.TrimDesign, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.PlateHolder, vehClass.PlateHolder, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.VanityPlates, vehClass.VanityPlates, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.FrontWheels, vehClass.Frontwheels, vehClass.WheelsVariation);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.BackWheels, vehClass.BackWheels, vehClass.WheelsVariation);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.ArchCover, vehClass.ArchCover, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Exhaust, vehClass.Exhaust, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Fender, vehClass.Fender, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.RightFender, vehClass.RightFender, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.DoorSpeakers, vehClass.DoorSpeakers, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Frame, vehClass.Frame, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Grille, vehClass.Grille, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Hood, vehClass.Hood, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Horns, vehClass.Horn, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Hydraulics, vehClass.Hydraulics, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Livery, vehClass.Livery, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Plaques, vehClass.Plaques, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Roof, vehClass.Roof, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Spoilers, vehClass.Spoiler, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Tank, vehClass.Tank, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Trunk, vehClass.Trunk, true);
				V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Windows, vehClass.Windows, true);
				V2Compat.ToggleMod(vehicle2, V2Compat.VehicleToggleMod.XenonHeadlights, vehClass.Headlights);
				V2Compat.ToggleMod(vehicle2, V2Compat.VehicleToggleMod.Turbo, vehClass.Turbo);
				V2Compat.ToggleMod(vehicle2, V2Compat.VehicleToggleMod.TireSmoke, vehClass.Tiresmoke);
				vehicle2.Mods.TrimColor = (VehicleColor)vehClass.TrimColor;
				vehicle2.Mods.LicensePlateStyle = (LicensePlateStyle)vehClass.NumberPlate;
				vehicle2.Mods.LicensePlate = vehClass.PlateNumber;
				V2Compat.SetNeonLightsOn(vehicle2, VehicleNeonLight.Front, vehClass.FrontNeon);
				V2Compat.SetNeonLightsOn(vehicle2, VehicleNeonLight.Back, vehClass.BackNeon);
				V2Compat.SetNeonLightsOn(vehicle2, VehicleNeonLight.Left, vehClass.LeftNeon);
				V2Compat.SetNeonLightsOn(vehicle2, VehicleNeonLight.Right, vehClass.RightNeon);
				vehicle2.Mods.WindowTint = (VehicleWindowTint)vehClass.Tint;
				vehicle2.Mods.PrimaryColor = (VehicleColor)vehClass.PrimaryColor;
				vehicle2.Mods.SecondaryColor = (VehicleColor)vehClass.SecondaryColor;
				vehicle2.Mods.PearlescentColor = (VehicleColor)vehClass.PearlescentColor;
				vehicle2.Mods.RimColor = (VehicleColor)vehClass.RimColor;
				vehicle2.Mods.DashboardColor = (VehicleColor)vehClass.LightsColor;
				vehicle2.Mods.NeonLightsColor = ToColor(vehClass.NeonLightsColor);
				vehicle2.Mods.TireSmokeColor = ToColor(vehClass.TireSmokeColor);
				Livery2(vehicle2, vehClass.Livery2);
				vehicle2.Mods.Livery = vehClass.Livery1;
				XenonHeadlightsColor(vehicle2, vehClass.HeadlightsColor);
				vehicle2.CanTiresBurst = vehClass.BulletProofTires;
				if (vehClass.HasCustomPrimaryColor)
				{
					vehicle2.Mods.CustomPrimaryColor = ToColor(vehClass.CustomPrimaryColor);
				}
				if (vehClass.HasCustomSecondaryColor)
				{
					vehicle2.Mods.CustomSecondaryColor = ToColor(vehClass.CustomSecondaryColor);
				}
				vehicle2.ToggleExtra(0, vehClass.Extra0);
				vehicle2.ToggleExtra(1, vehClass.Extra1);
				vehicle2.ToggleExtra(2, vehClass.Extra2);
				vehicle2.ToggleExtra(3, vehClass.Extra3);
				vehicle2.ToggleExtra(4, vehClass.Extra4);
				vehicle2.ToggleExtra(5, vehClass.Extra5);
				vehicle2.ToggleExtra(6, vehClass.Extra6);
				vehicle2.ToggleExtra(7, vehClass.Extra7);
				vehicle2.ToggleExtra(8, vehClass.Extra8);
				vehicle2.ToggleExtra(9, vehClass.Extra9);
				vehicle2.ToggleExtra(10, vehClass.Extra10);
				vehicle2.ToggleExtra(11, vehClass.Extra11);
				vehicle2.ToggleExtra(12, vehClass.Extra12);
				vehicle2.ToggleExtra(13, vehClass.Extra13);
				vehicle2.ToggleExtra(14, vehClass.Extra14);
				vehicle2.ToggleExtra(15, vehClass.Extra15);
				vehicle2.RoofState = (VehicleRoofState)vehClass.RoofState;
				vehicle2.DirtLevel = 0f;
				if (IsNitroModInstalled() != 0)
				{
					vehicle2.SetInt(nitroModDecor, vehClass.HasNitro);
				}
				vehicle2.IsPersistent = true;
				vehicle2.SetInt(vehIdDecor, aptID);
				vehicle2.SetInt(vehUidDecor, vehClass.UniqueID);
				vehicle2 = null;
				RememberVehicleOwner(vehicle, vehClass.Owner);
			}
			if (canContinue == null)
			{
				model.MarkAsNoLongerNeeded();
			}
			return vehicle;
		}

		public static void RegisterDecor(string d, Decor.eDecorType t)
		{
			if (!Decor.Registered(d, t))
			{
				Decor.Unlock();
				Decor.Register(d, t);
				Decor.Lock();
			}
		}

		public static void Debug()
		{
			UIResText uIResText = new UIResText(string.Format("Player Position: {0}  Rotation: {1}  Heading: {2}  |  Vehicle Position: {3}  Rotation: {4}  Heading: {5}", PP.Position, PP.Rotation, PP.Heading.ToString("N"), ((object)PP.LastVehicle == null) ? "-" : PP.LastVehicle.Position.ToString(), ((object)PP.LastVehicle == null) ? "-" : PP.LastVehicle.Rotation.ToString(), ((object)PP.LastVehicle == null) ? "-" : PP.LastVehicle.Heading.ToString("N")), Point.Empty, 0.3f, Color.White, GTA.UI.Font.ChaletLondon, UIResText.Alignment.Left);
			checked
			{
				UIResText uIResText2 = ((!DebugCamera.IsEnabled()) ? new UIResText($"Camera Position: {GameplayCamera.Position}  Rotation: {GameplayCamera.Rotation}  FOV: {GameplayCamera.FieldOfView}", new Point(0, uIResText.Position.Y + 20), 0.3f, Color.White, GTA.UI.Font.ChaletLondon, UIResText.Alignment.Left) : new UIResText($"Camera Position: {DebugCamera.Camera.Position}  Rotation: {DebugCamera.Camera.Rotation}  FOV: {DebugCamera.Camera.FieldOfView}", new Point(0, uIResText.Position.Y + 20), 0.3f, Color.White, GTA.UI.Font.ChaletLondon, UIResText.Alignment.Left));
				UIResText uIResText3 = new UIResText($"↑: Vector  ↓: Quaternion  ←: CameraPRH  →: Door  Del: Debug Camera  End: Debug Menu  |  2 Car Garage: {TwoCarGarage.Apartment != null}     6 Car Garage: {SixCarGarage.Apartment != null}     10 Car Garage: {TenCarGarage.Apartment != null}     Low Apt: {LowEndApartment.Apartment != null}     Medium Apt: {MediumEndApartment.Apartment != null}     High Apt: {HighEndApartment.Building != null}", new Point(0, uIResText2.Position.Y + 20), 0.3f, Color.White, GTA.UI.Font.ChaletLondon, UIResText.Alignment.Left);
				UIResText uIResText4 = new UIResText(debug3rdLine, new Point(0, uIResText3.Position.Y + 20), 0.3f, Color.White, GTA.UI.Font.ChaletLondon, UIResText.Alignment.Left);
				uIResText.Draw();
				uIResText2.Draw();
				uIResText3.Draw();
				uIResText4.Draw();
			}
		}

		public static void ChangeIPL(string old, string @new)
		{
			if (Function.Call<bool>(Hash.IS_IPL_ACTIVE, old))
			{
				Function.Call(Hash.REMOVE_IPL, old);
			}
			Function.Call(Hash.REQUEST_IPL, @new);
			int num = checked(Game.GameTime + 5000);
			while (!Function.Call<bool>(Hash.IS_IPL_ACTIVE, @new) && Game.GameTime < num)
			{
				Script.Yield();
			}
			if (!Function.Call<bool>(Hash.IS_IPL_ACTIVE, @new))
			{
				Logger.Log($"ChangeIPL: '{@new}' did not load");
			}
		}

		public static object NewUiMenuItem2(string title, string desc = null, UIMenuItem.BadgeStyle rightbadge = UIMenuItem.BadgeStyle.None)
		{
			UIMenuItem uIMenuItem = new UIMenuItem(title, desc);
			uIMenuItem.SetRightBadge(rightbadge);
			return uIMenuItem;
		}

		public static Vehicle WorldCreateVehicle(Model model, Vector3 position, float heading = 0f)
		{
			if (!model.IsVehicle || !model.Request(1000))
			{
				return null;
			}
			return Entity.FromHandle(Function.Call<int>(Hash.CREATE_VEHICLE, model.Hash, position.X, position.Y, position.Z, heading, false, false)) as Vehicle;
		}

		public static Vehicle CreateVehicle(int VehicleHash, Vector3 Position, float Heading = 0f)
		{
			Model model = new Model(VehicleHash);
			if (model.IsInCdImage && model.IsValid)
			{
				if (model.Request(5000))
				{
					return WorldCreateVehicle(model, Position, Heading);
				}
				Logger.Log($"CreateVehicle: model {VehicleHash} did not load");
			}
			model.MarkAsNoLongerNeeded();
			return null;
		}

		public static bool IsGarageVehicleAlreadyExistInWorldMap(int aid, int uid)
		{
			_Closure_0024__88_002D0 closure_0024__88_002D = new _Closure_0024__88_002D0();
			closure_0024__88_002D._0024VB_0024Local_aid = aid;
			closure_0024__88_002D._0024VB_0024Local_uid = uid;
			return outVehicleList.Any(closure_0024__88_002D._Lambda_0024__0);
		}

		public static bool IsCurrentVehicleExistInList(this Vehicle vehicle)
		{
			return outVehicleList.Contains(vehicle);
		}

		public static Vehicle CloneVehicle(this Vehicle source, Vector2 pos, float head, bool cloneDamage = false)
		{
			Vehicle vehicle = WorldCreateVehicle(source.Model, pos, head);
			Vehicle vehicle2 = vehicle;
			V2Compat.InstallModKit(vehicle2);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Speakers, V2Compat.GetMod(source, V2Compat.VehicleMod.Speakers), true);
			vehicle2.Mods.WheelType = source.Mods.WheelType;
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Aerials, V2Compat.GetMod(source, V2Compat.VehicleMod.Aerials), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Suspension, V2Compat.GetMod(source, V2Compat.VehicleMod.Suspension), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Brakes, V2Compat.GetMod(source, V2Compat.VehicleMod.Brakes), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Engine, V2Compat.GetMod(source, V2Compat.VehicleMod.Engine), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Transmission, V2Compat.GetMod(source, V2Compat.VehicleMod.Transmission), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Armor, V2Compat.GetMod(source, V2Compat.VehicleMod.Armor), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.FrontBumper, V2Compat.GetMod(source, V2Compat.VehicleMod.FrontBumper), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.RearBumper, V2Compat.GetMod(source, V2Compat.VehicleMod.RearBumper), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.SideSkirt, V2Compat.GetMod(source, V2Compat.VehicleMod.SideSkirt), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Trim, V2Compat.GetMod(source, V2Compat.VehicleMod.Trim), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.EngineBlock, V2Compat.GetMod(source, V2Compat.VehicleMod.EngineBlock), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.AirFilter, V2Compat.GetMod(source, V2Compat.VehicleMod.AirFilter), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Struts, V2Compat.GetMod(source, V2Compat.VehicleMod.Struts), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.ColumnShifterLevers, V2Compat.GetMod(source, V2Compat.VehicleMod.ColumnShifterLevers), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Dashboard, V2Compat.GetMod(source, V2Compat.VehicleMod.Dashboard), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.DialDesign, V2Compat.GetMod(source, V2Compat.VehicleMod.DialDesign), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Ornaments, V2Compat.GetMod(source, V2Compat.VehicleMod.Ornaments), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Seats, V2Compat.GetMod(source, V2Compat.VehicleMod.Seats), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.SteeringWheels, V2Compat.GetMod(source, V2Compat.VehicleMod.SteeringWheels), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.TrimDesign, V2Compat.GetMod(source, V2Compat.VehicleMod.TrimDesign), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.PlateHolder, V2Compat.GetMod(source, V2Compat.VehicleMod.PlateHolder), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.VanityPlates, V2Compat.GetMod(source, V2Compat.VehicleMod.VanityPlates), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.FrontWheels, V2Compat.GetMod(source, V2Compat.VehicleMod.FrontWheels), WheelsVariation(source));
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.BackWheels, V2Compat.GetMod(source, V2Compat.VehicleMod.BackWheels), WheelsVariation(source));
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.ArchCover, V2Compat.GetMod(source, V2Compat.VehicleMod.ArchCover), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Exhaust, V2Compat.GetMod(source, V2Compat.VehicleMod.Exhaust), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Fender, V2Compat.GetMod(source, V2Compat.VehicleMod.Fender), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.RightFender, V2Compat.GetMod(source, V2Compat.VehicleMod.RightFender), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.DoorSpeakers, V2Compat.GetMod(source, V2Compat.VehicleMod.DoorSpeakers), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Frame, V2Compat.GetMod(source, V2Compat.VehicleMod.Frame), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Grille, V2Compat.GetMod(source, V2Compat.VehicleMod.Grille), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Hood, V2Compat.GetMod(source, V2Compat.VehicleMod.Hood), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Horns, V2Compat.GetMod(source, V2Compat.VehicleMod.Horns), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Hydraulics, V2Compat.GetMod(source, V2Compat.VehicleMod.Hydraulics), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Livery, V2Compat.GetMod(source, V2Compat.VehicleMod.Livery), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Plaques, V2Compat.GetMod(source, V2Compat.VehicleMod.Plaques), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Roof, V2Compat.GetMod(source, V2Compat.VehicleMod.Roof), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Spoilers, V2Compat.GetMod(source, V2Compat.VehicleMod.Spoilers), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Tank, V2Compat.GetMod(source, V2Compat.VehicleMod.Tank), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Trunk, V2Compat.GetMod(source, V2Compat.VehicleMod.Trunk), true);
			V2Compat.SetMod(vehicle2, V2Compat.VehicleMod.Windows, V2Compat.GetMod(source, V2Compat.VehicleMod.Windows), true);
			V2Compat.ToggleMod(vehicle2, V2Compat.VehicleToggleMod.XenonHeadlights, V2Compat.IsToggleModOn(source, V2Compat.VehicleToggleMod.XenonHeadlights));
			V2Compat.ToggleMod(vehicle2, V2Compat.VehicleToggleMod.Turbo, V2Compat.IsToggleModOn(source, V2Compat.VehicleToggleMod.Turbo));
			V2Compat.ToggleMod(vehicle2, V2Compat.VehicleToggleMod.TireSmoke, V2Compat.IsToggleModOn(source, V2Compat.VehicleToggleMod.TireSmoke));
			vehicle2.Mods.TrimColor = source.Mods.TrimColor;
			vehicle2.Mods.LicensePlateStyle = source.Mods.LicensePlateStyle;
			vehicle2.Mods.LicensePlate = source.Mods.LicensePlate;
			V2Compat.SetNeonLightsOn(vehicle2, VehicleNeonLight.Front, V2Compat.IsNeonLightsOn(source, VehicleNeonLight.Front));
			V2Compat.SetNeonLightsOn(vehicle2, VehicleNeonLight.Back, V2Compat.IsNeonLightsOn(source, VehicleNeonLight.Back));
			V2Compat.SetNeonLightsOn(vehicle2, VehicleNeonLight.Left, V2Compat.IsNeonLightsOn(source, VehicleNeonLight.Left));
			V2Compat.SetNeonLightsOn(vehicle2, VehicleNeonLight.Right, V2Compat.IsNeonLightsOn(source, VehicleNeonLight.Right));
			vehicle2.Mods.WindowTint = source.Mods.WindowTint;
			vehicle2.Mods.PrimaryColor = source.Mods.PrimaryColor;
			vehicle2.Mods.SecondaryColor = source.Mods.SecondaryColor;
			vehicle2.Mods.PearlescentColor = source.Mods.PearlescentColor;
			vehicle2.Mods.RimColor = source.Mods.RimColor;
			vehicle2.Mods.DashboardColor = source.Mods.DashboardColor;
			vehicle2.Mods.NeonLightsColor = source.Mods.NeonLightsColor;
			vehicle2.Mods.TireSmokeColor = source.Mods.TireSmokeColor;
			Livery2(vehicle2, Livery2(source));
			vehicle2.Mods.Livery = source.Mods.Livery;
			XenonHeadlightsColor(vehicle2, XenonHeadlightsColor(source));
			vehicle2.CanTiresBurst = source.CanTiresBurst;
			if (source.Mods.IsPrimaryColorCustom)
			{
				vehicle2.Mods.CustomPrimaryColor = source.Mods.CustomPrimaryColor;
			}
			if (source.Mods.IsSecondaryColorCustom)
			{
				vehicle2.Mods.CustomSecondaryColor = source.Mods.CustomSecondaryColor;
			}
			vehicle2.ToggleExtra(0, source.IsExtraOn(0));
			vehicle2.ToggleExtra(1, source.IsExtraOn(1));
			vehicle2.ToggleExtra(2, source.IsExtraOn(2));
			vehicle2.ToggleExtra(3, source.IsExtraOn(3));
			vehicle2.ToggleExtra(4, source.IsExtraOn(4));
			vehicle2.ToggleExtra(5, source.IsExtraOn(5));
			vehicle2.ToggleExtra(6, source.IsExtraOn(6));
			vehicle2.ToggleExtra(7, source.IsExtraOn(7));
			vehicle2.ToggleExtra(8, source.IsExtraOn(8));
			vehicle2.ToggleExtra(9, source.IsExtraOn(9));
			vehicle2.ToggleExtra(10, source.IsExtraOn(10));
			vehicle2.ToggleExtra(11, source.IsExtraOn(11));
			vehicle2.ToggleExtra(12, source.IsExtraOn(12));
			vehicle2.ToggleExtra(13, source.IsExtraOn(13));
			vehicle2.ToggleExtra(14, source.IsExtraOn(14));
			vehicle2.ToggleExtra(15, source.IsExtraOn(15));
			vehicle2.RoofState = source.RoofState;
			vehicle2.DirtLevel = source.DirtLevel;
			if (IsNitroModInstalled() != 0)
			{
				vehicle2.SetInt(nitroModDecor, source.GetInt(nitroModDecor));
			}
			vehicle2.IsPersistent = true;
			vehicle2.SetInt(vehIdDecor, source.GetInt(vehIdDecor));
			vehicle2.SetInt(vehUidDecor, source.GetInt(vehUidDecor));
			vehicle2 = null;
			if (cloneDamage)
			{
				Function.Call(INMNative.COPY_VEHICLE_DAMAGES, source, vehicle);
			}
			return vehicle;
		}

		public static void SetPlayerIntoVehicle(this Vehicle vehicle, VehicleSeat seat = VehicleSeat.Driver)
		{
			Function.Call(Hash.SET_PED_INTO_VEHICLE, Game.Player.Character, vehicle.Handle, (int)seat);
		}

		public static bool IsPersonalVehicle(this Vehicle veh)
		{
			return veh.GetInt(vehUidDecor) != 0;
		}

		public static void RequestAdditionalText(string gxt2Lib, string gxt)
		{
			if (!string.IsNullOrWhiteSpace(Game.GetLocalizedString(gxt)) && Operators.CompareString(Game.GetLocalizedString(gxt), "NULL", false) != 0)
			{
				return;
			}
			checked
			{
				long num = (long)Math.Round((double)Stopwatch.GetTimestamp() * 1000.0 / (double)Stopwatch.Frequency);
				long value;
				if (!additionalTextAttempts.TryGetValue(gxt2Lib, out value) || num - value >= 30000)
				{
					additionalTextAttempts[gxt2Lib] = num;
					if (!Function.Call<bool>(Hash.HAS_THIS_ADDITIONAL_TEXT_LOADED, gxt2Lib, 10))
					{
						Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 10, true);
						Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, gxt2Lib, 10);
					}
					Stopwatch stopwatch = Stopwatch.StartNew();
					while ((string.IsNullOrWhiteSpace(Game.GetLocalizedString(gxt)) || Operators.CompareString(Game.GetLocalizedString(gxt), "NULL", false) == 0) && stopwatch.ElapsedMilliseconds < 3000)
					{
						Script.Yield();
					}
				}
			}
		}

		public static int GetAvailableIndex(List<VehicleClass> vcList, eGarageType garageType)
		{
			int num = 10;
			switch (garageType)
			{
			case eGarageType.TwoCarGarage:
				num = 2;
				break;
			case eGarageType.SixCarGarage:
				num = 6;
				break;
			case eGarageType.TenCarGarage:
				num = 10;
				break;
			case eGarageType.TwentyCarGarage:
				num = 20;
				break;
			}
			List<int> list = new List<int>();
			foreach (VehicleClass vc in vcList)
			{
				list.Add(vc.Index);
			}
			return Enumerable.Range(0, checked(num - 0 + 1)).Except(list).FirstOrDefault();
		}

		public static Door GetEclipseTowersPenthouseDoor(this string ipl)
		{
			Door result = new Door(0, Vector3.Zero);
			switch (ipl)
			{
			case "apa_v_mp_h_01_a":
				result = new Door(-658026477, new Vector3(-782.4497f, 317.5156f, 217.7876f));
				break;
			case "apa_v_mp_h_01_c":
				result = new Door(-658026477, new Vector3(-782.4497f, 317.5156f, 188.0627f));
				break;
			case "apa_v_mp_h_01_b":
				result = new Door(-658026477, new Vector3(-778.5566f, 340.2328f, 196.8354f));
				break;
			case "apa_v_mp_h_02_a":
				result = new Door(103339342, new Vector3(-782.4496f, 317.5228f, 217.7876f));
				break;
			case "apa_v_mp_h_02_c":
				result = new Door(103339342, new Vector3(-782.4496f, 317.5228f, 188.0627f));
				break;
			case "apa_v_mp_h_02_b":
				result = new Door(103339342, new Vector3(-778.5568f, 340.2256f, 196.8354f));
				break;
			case "apa_v_mp_h_03_a":
				result = new Door(1398355146, new Vector3(-782.4496f, 317.5228f, 217.7876f));
				break;
			case "apa_v_mp_h_03_c":
				result = new Door(1398355146, new Vector3(-782.4496f, 317.5228f, 188.0627f));
				break;
			case "apa_v_mp_h_03_b":
				result = new Door(1398355146, new Vector3(-778.5568f, 340.2256f, 196.8354f));
				break;
			case "apa_v_mp_h_04_a":
				result = new Door(103339342, new Vector3(-782.4495f, 317.5152f, 217.7884f));
				break;
			case "apa_v_mp_h_04_c":
				result = new Door(103339342, new Vector3(-782.4495f, 317.5152f, 188.0634f));
				break;
			case "apa_v_mp_h_04_b":
				result = new Door(103339342, new Vector3(-778.5568f, 340.2332f, 196.8362f));
				break;
			case "apa_v_mp_h_05_a":
				result = new Door(-711771128, new Vector3(-782.4497f, 317.5156f, 217.7876f));
				break;
			case "apa_v_mp_h_05_c":
				result = new Door(-711771128, new Vector3(-782.4497f, 317.5156f, 188.0627f));
				break;
			case "apa_v_mp_h_05_b":
				result = new Door(-711771128, new Vector3(-778.5566f, 340.2328f, 196.8354f));
				break;
			case "apa_v_mp_h_06_a":
				result = new Door(103339342, new Vector3(-782.4496f, 317.5228f, 217.7876f));
				break;
			case "apa_v_mp_h_06_c":
				result = new Door(103339342, new Vector3(-782.4496f, 317.5228f, 188.0627f));
				break;
			case "apa_v_mp_h_06_b":
				result = new Door(103339342, new Vector3(-778.5568f, 340.2256f, 196.8354f));
				break;
			case "apa_v_mp_h_07_a":
				result = new Door(1398355146, new Vector3(-782.4496f, 317.5228f, 217.7876f));
				break;
			case "apa_v_mp_h_07_c":
				result = new Door(1398355146, new Vector3(-782.4496f, 317.5228f, 188.0627f));
				break;
			case "apa_v_mp_h_07_b":
				result = new Door(1398355146, new Vector3(-778.5568f, 340.2256f, 196.8354f));
				break;
			case "apa_v_mp_h_08_a":
				result = new Door(103339342, new Vector3(-782.4495f, 317.5152f, 217.7884f));
				break;
			case "apa_v_mp_h_08_c":
				result = new Door(103339342, new Vector3(-782.4495f, 317.5152f, 188.0634f));
				break;
			case "apa_v_mp_h_08_b":
				result = new Door(103339342, new Vector3(-778.5568f, 340.2332f, 196.8362f));
				break;
			}
			return result;
		}

		public static string Truncate(this string value, int length = 380)
		{
			if (IsAsian())
			{
				length = checked((int)Math.Round((double)length / 3.0));
			}
			if (length > Length2(value))
			{
				if (IsAsian())
				{
					value = SplitChineseString(value);
				}
				return value;
			}
			if (IsAsian())
			{
				value = SplitChineseString(value);
			}
			try
			{
				return $"{value.Substring(0, length)}...";
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				string result = value;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public static bool IsAsian()
		{
			Language language = Game.Language;
			if ((uint)(language - 8) <= 2u || language == Language.ChineseSimplified)
			{
				return true;
			}
			return false;
		}

		public static string SplitChineseString(this string value)
		{
			int num = Strings.Len(value);
			checked
			{
				for (int i = 0; i <= num; i += 21)
				{
					try
					{
						value = value.Insert(i + 20, "\r\n");
					}
					catch (Exception projectError)
					{
						ProjectData.SetProjectError(projectError);
						ProjectData.ClearProjectError();
					}
				}
				return value.Replace("\r\n", "~n~");
			}
		}

		public static int Length2(this string value)
		{
			if (IsAsian())
			{
				return checked(value.Length * 2);
			}
			return value.Length;
		}

		public static Quaternion QuaternionZero()
		{
			return new Quaternion(0f, 0f, 0f, 0f);
		}

		public static void DrawMarker(this Vector3 pos, Color col = default(Color), Vector3 size = default(Vector3), string text = null)
		{
			if (col == default(Color))
			{
				col = Color.DeepSkyBlue;
			}
			if (size == default(Vector3))
			{
				size = new Vector3(0.8f, 0.8f, 0.4f);
			}
			World.DrawMarker(MarkerType.Cylinder, pos, Vector3.Zero, Vector3.Zero, size, Color.FromArgb(150, col));
			if (Operators.CompareString(text, null, false) != 0)
			{
				Draw3DText(pos, text);
			}
		}

		public static void DrawMgmtMarker(this Vector3 pos, Color col = default(Color), Vector3 size = default(Vector3))
		{
			if (col == default(Color))
			{
				col = Color.DeepSkyBlue;
			}
			if (size == default(Vector3))
			{
				size = new Vector3(0.4f, 0.4f, 0.4f);
			}
			World.DrawMarker(MarkerType.Chevron1, new Vector3(pos.X, pos.Y, pos.Z + 2f), Vector3.Zero, new Vector3(0f, 180f, 0f), size, Color.FromArgb(150, col));
		}

		public static void Draw3DText(this Vector3 pos, string text)
		{
			Vector2 screenCoordFromWorldCoord = GetScreenCoordFromWorldCoord(pos);
			float distanceBetweenCoords = GetDistanceBetweenCoords(GameplayCamera.Position, pos);
			float num = 1f / distanceBetweenCoords * 2f;
			float num2 = 1f / GameplayCamera.FieldOfView * 100f;
			num *= num2;
			DrawText(text, num, num, GTA.UI.Font.ChaletLondon, screenCoordFromWorldCoord);
		}

		public static Point ToPoint(this Vector2 v2)
		{
			return checked(new Point((int)Math.Round(v2.X), (int)Math.Round(v2.Y)));
		}

		public static Vector2 GetScreenCoordFromWorldCoord(this Vector3 pos)
		{
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			Function.Call<bool>(INMNative.GET_SCREEN_COORD_FROM_WORLD_COORD, pos.X, pos.Y, pos.Z, outputArgument, outputArgument2);
			return new Vector2(outputArgument.GetResult<float>(), outputArgument2.GetResult<float>());
		}

		public static float GetDistanceBetweenCoords(this Vector3 pos1, Vector3 pos2)
		{
			return Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, pos1.X, pos1.Y, pos1.Z, pos2.X, pos2.Y, pos2.Z, true);
		}

		public static void DrawText(string text, float scale, float size, GTA.UI.Font font, Vector2 pos)
		{
			Function.Call(Hash.SET_TEXT_SCALE, scale, size);
			Function.Call(Hash.SET_TEXT_FONT, (int)font);
			Function.Call(Hash.SET_TEXT_PROPORTIONAL, true);
			Color white = Color.White;
			Function.Call(Hash.SET_TEXT_COLOUR, white.R, white.B, white.G, 150);
			Color black = Color.Black;
			Function.Call(Hash.SET_TEXT_DROPSHADOW, 0, black.R, black.G, black.B, 255);
			Function.Call(Hash.SET_TEXT_DROP_SHADOW);
			Function.Call(Hash.SET_TEXT_OUTLINE);
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.SET_TEXT_CENTRE, true);
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, pos.X, pos.Y);
		}

		public static void SetAsGarage(this ApartmentClass apt)
		{
			apt.SavePos = Vector3.Zero;
			apt.ApartmentDoorPos = QuaternionZero();
			apt.ApartmentInPos = Vector3.Zero;
			apt.ApartmentOutPos = Vector3.Zero;
			apt.WardrobePos = QuaternionZero();
			apt.IPL = null;
			apt.AptStyleCam = null;
			apt.EnterCam = null;
			apt.ExitCam = null;
			apt.GarageElevatorPos = Vector3.Zero;
			apt.GarageMenuPos = Vector3.Zero;
			apt.ApartmentType = eApartmentType.Other;
			apt.Door = null;
		}

		public static void SetAsGarage(this BuildingClass bld)
		{
			BuildingClass buildingClass = bld;
			buildingClass.BuildingInPos = new Quaternion(buildingClass.GarageInPos.X, buildingClass.GarageInPos.Y, buildingClass.GarageInPos.Z, 0f);
			buildingClass.BuildingOutPos = QuaternionZero();
			buildingClass.BuildingLobby = QuaternionZero();
			buildingClass.EnterCamera1 = null;
			buildingClass.EnterCamera2 = null;
			buildingClass.BuildingType = eBuildingType.Garage;
			buildingClass.FrontDoor = eFrontDoor.NoDoor;
			buildingClass.Door1 = null;
			buildingClass.Door2 = null;
			buildingClass.GarageDoor = eFrontDoor.NoDoor;
			buildingClass.Door3 = null;
			buildingClass.GarageWaypoint = QuaternionZero();
			buildingClass = null;
		}

		public static void SetAsLowEndApartment(this ApartmentClass apt)
		{
			apt.SavePos = LowEndApartment.SavePos;
			apt.ApartmentDoorPos = LowEndApartment.DoorPos;
			apt.ApartmentInPos = LowEndApartment.InPos;
			apt.ApartmentOutPos = LowEndApartment.OutPos;
			apt.WardrobePos = LowEndApartment.WardrobePos;
			apt.IPL = null;
			apt.AptStyleCam = null;
			apt.EnterCam = LowEndApartment.EnterCam;
			apt.ExitCam = LowEndApartment.ExitCam;
			apt.GarageElevatorPos = Vector3.Zero;
			apt.GarageMenuPos = Vector3.Zero;
			apt.ApartmentType = eApartmentType.LowEnd;
			apt.Door = LowEndApartment.Door;
		}

		public static void SetAsMediumApartment(this ApartmentClass apt)
		{
			apt.SavePos = MediumEndApartment.SavePos;
			apt.ApartmentDoorPos = MediumEndApartment.DoorPos;
			apt.ApartmentInPos = MediumEndApartment.InPos;
			apt.ApartmentOutPos = MediumEndApartment.OutPos;
			apt.WardrobePos = MediumEndApartment.WardrobePos;
			apt.IPL = null;
			apt.AptStyleCam = null;
			apt.EnterCam = MediumEndApartment.EnterCam;
			apt.ExitCam = MediumEndApartment.ExitCam;
			apt.GarageElevatorPos = Vector3.Zero;
			apt.GarageMenuPos = Vector3.Zero;
			apt.ApartmentType = eApartmentType.MediumEnd;
			apt.Door = MediumEndApartment.Door;
		}

		public static List<Vector5> GetParkingSpotByZone(this Vector3 pp)
		{
			switch (Function.Call<string>(Hash.GET_NAME_OF_ZONE, pp.X, pp.Y, pp.Z))
			{
			case "DOWNT":
			case "VINE":
				return CarGens.DOWNT;
			case "GALLI":
			case "CHIL":
				return CarGens.CHIL;
			case "DESRT":
			case "GREATC":
				return CarGens.DESRT;
			case "CMSW":
			case "PALCOV":
				return CarGens.CMSW;
			case "PBOX":
				return CarGens.PBOX;
			case "SKID":
				return CarGens.SKID;
			case "TEXTI":
				return CarGens.TEXTI;
			case "LEGSQU":
				return CarGens.LEGSQU;
			case "CANNY":
				return CarGens.CANNY;
			case "DTVINE":
				return CarGens.DTVINE;
			case "EAST_V":
				return CarGens.EAST_V;
			case "MIRR":
				return CarGens.MIRR;
			case "WVINE":
				return CarGens.WVINE;
			case "ALTA":
				return CarGens.ALTA;
			case "HAWICK":
				return CarGens.HAWICK;
			case "RICHM":
				return CarGens.RICHM;
			case "golf":
				return CarGens.golf;
			case "ROCKF":
				return CarGens.ROCKF;
			case "CCREAK":
				return CarGens.CCREAK;
			case "RGLEN":
				return CarGens.RGLEN;
			case "OBSERV":
				return CarGens.OBSERV;
			case "DAVIS":
				return CarGens.DAVIS;
			case "STRAW":
				return CarGens.STRAW;
			case "CHAMH":
				return CarGens.CHAMH;
			case "RANCHO":
				return CarGens.RANCHO;
			case "BANNING":
				return CarGens.BANNING;
			case "ELYSIAN":
				return CarGens.ELYSIAN;
			case "TERMINA":
				return CarGens.TERMINA;
			case "ZP_ORT":
				return CarGens.ZP_ORT;
			case "LMESA":
				return CarGens.LMESA;
			case "CYPRE":
				return CarGens.CYPRE;
			case "EBURO":
				return CarGens.EBURO;
			case "MURRI":
				return CarGens.MURRI;
			case "VESP":
				return CarGens.VESP;
			case "BEACH":
				return CarGens.BEACH;
			case "VCANA":
				return CarGens.VCANA;
			case "DELSOL":
				return CarGens.DELSOL;
			case "DELBE":
				return CarGens.DELBE;
			case "DELPE":
				return CarGens.DELPE;
			case "LOSPUER":
				return CarGens.LOSPUER;
			case "STAD":
				return CarGens.STAD;
			case "KOREAT":
				return CarGens.KOREAT;
			case "AIRP":
				return CarGens.AIRP;
			case "MORN":
				return CarGens.MORN;
			case "PBLUFF":
				return CarGens.PBLUFF;
			case "BHAMCA":
				return CarGens.BHAMCA;
			case "CHU":
				return CarGens.CHU;
			case "TONGVAH":
				return CarGens.TONGVAH;
			case "TONGVAV":
				return CarGens.TONGVAV;
			case "TATAMO":
				return CarGens.TATAMO;
			case "PALHIGH":
				return CarGens.PALHIGH;
			case "NOOSE":
				return CarGens.NOOSE;
			case "MOVIE":
				return CarGens.MOVIE;
			case "SanAnd":
				return CarGens.SanAnd;
			case "ALAMO":
				return CarGens.ALAMO;
			case "JAIL":
				return CarGens.JAIL;
			case "RTRAK":
				return CarGens.RTRAK;
			case "SANCHIA":
				return CarGens.SANCHIA;
			case "WINDF":
				return CarGens.WINDF;
			case "PALMPOW":
				return CarGens.PALMPOW;
			case "HUMLAB":
				return CarGens.HUMLAB;
			case "ZQ_UAR":
				return CarGens.ZQ_UAR;
			case "PALETO":
				return CarGens.PALETO;
			case "PALFOR":
				return CarGens.PALFOR;
			case "PROCOB":
				return CarGens.PROCOB;
			case "HARMO":
				return CarGens.HARMO;
			case "SANDY":
				return CarGens.SANDY;
			case "ZANCUDO":
				return CarGens.ZANCUDO;
			case "SLAB":
				return CarGens.SLAB;
			case "NCHU":
				return CarGens.NCHU;
			case "GRAPES":
				return CarGens.GRAPES;
			case "MTGORDO":
				return CarGens.MTGORDO;
			case "MTCHIL":
				return CarGens.MTCHIL;
			case "GALFISH":
				return CarGens.GALFISH;
			case "LAGO":
				return CarGens.LAGO;
			case "ARMYB":
				return CarGens.ARMYB;
			case "BURTON":
				return CarGens.BURTON;
			default:
				return new List<Vector5>();
			}
		}

		public static Vector5 GetNearestParkingSpot(this Vector3 pos)
		{
			_Closure_0024__114_002D0 closure_0024__114_002D = new _Closure_0024__114_002D0();
			closure_0024__114_002D._0024VB_0024Local_pos = pos;
			IOrderedEnumerable<Vector5> source = GetParkingSpotByZone(closure_0024__114_002D._0024VB_0024Local_pos).OrderBy(closure_0024__114_002D._Lambda_0024__0);
			if (source.Count() != 0)
			{
				return source.FirstOrDefault();
			}
			return Vector5.Zero();
		}

		public static Quaternion ToQuaternion(this Vector5 v5)
		{
			return new Quaternion(v5.Vector3.X, v5.Vector3.Y, v5.Vector3.Z, v5.Vector2.ToHeading());
		}

		public static bool IsPositionOccupied(this Vector3 pos, float range)
		{
			return Function.Call<bool>(Hash.IS_POSITION_OCCUPIED, pos.X, pos.Y, pos.Z, range, false, true, false, false, false, 0, false);
		}

		public static void Play(this Speech speech)
		{
			using (WaveStream waveStream = new WaveStream(File.OpenRead($"{soundPath}{speech.Wav}")))
			{
				waveStream.Volume = ConfigHelper.SoundVolume;
				using (SoundPlayer soundPlayer = new SoundPlayer(waveStream))
				{
					soundPlayer.Play();
				}
			}
		}

		public static void Sleep(ApartmentClass apt)
		{
			ApartmentHomeServices.OpenBed(apt);
		}

		public static ClothingSet.CS GetClothes(this Ped ped, ePedVariation com)
		{
			return new ClothingSet.CS((int)com, Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, ped.Handle, (int)com), Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, ped.Handle, (int)com), Function.Call<int>(Hash.GET_PED_PALETTE_VARIATION, ped.Handle, (int)com));
		}

		public static ClothingSet.CS GetProps(this Ped ped, ePropVariation com)
		{
			return new ClothingSet.CS((int)com, Function.Call<int>(Hash.GET_PED_PROP_INDEX, ped.Handle, (int)com), Function.Call<int>(Hash.GET_PED_PROP_TEXTURE_INDEX, ped.Handle, (int)com), 0);
		}

		public static void SetProps(this Ped ped, int com, int draw, int txd)
		{
			if (draw == -1)
			{
				Function.Call(Hash.CLEAR_PED_PROP, ped, com);
			}
			Function.Call(Hash.SET_PED_PROP_INDEX, ped, com, draw, txd, 0);
		}

		public static void SetProps(this Ped ped, ClothingSet.CS cs)
		{
			if (cs.DrawableID == -1)
			{
				Function.Call(Hash.CLEAR_PED_PROP, ped, cs.ComponentID);
			}
			Function.Call(Hash.SET_PED_PROP_INDEX, ped, cs.ComponentID, cs.DrawableID, cs.TextureID, cs.PaletteID);
		}

		public static void SetClothes(this Ped ped, int com, int draw, int txd)
		{
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, com, draw, txd, 2);
		}

		public static void SetClothes(this Ped ped, ClothingSet.CS cs)
		{
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, cs.ComponentID, cs.DrawableID, cs.TextureID, cs.PaletteID);
		}

		public static string GetControlInstructionalButton(Control control)
		{
			return Function.Call<string>(INMNative.GET_CONTROL_INSTRUCTIONAL_BUTTON, 2, (int)control, true);
		}

		public static int GetInterior(this Vector3 pos)
		{
			return Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, pos.X, pos.Y, pos.Z);
		}

		public static bool IsInInterior()
		{
			return Function.Call<bool>(Hash.IS_INTERIOR_SCENE);
		}

		public static void ForgetOnlineMapForStoryLoad()
		{
			mpMapActive = false;
			mpMapLastCheck = -1000;
			mpMapFarSince = 0;
			mpMapOnAt = 0;
			mpLoadedAdditive = false;
			MansionTerrain.ForgetForStoryLoad();
		}

		public static bool EnableOnlineMap()
		{
			bool result = false;
			if (ConfigHelper.OnlineMap && Operators.CompareString(ConfigHelper.OnlineMapMode.ToLower(), "always", false) == 0)
			{
				result = SwitchOnlineMap(true);
			}
			else if (Operators.CompareString(ConfigHelper.OnlineMapMode.ToLower(), "near", false) == 0)
			{
				Logger.Log($"online map: mode Near (radius {ConfigHelper.OnlineMapRadius} m) - switching on demand");
			}
			return result;
		}

		public static bool SwitchOnlineMap(bool enable)
		{
			if (enable == mpMapActive)
			{
				return false;
			}
			try
			{
				if (enable)
				{
					if (!MansionTerrain.CanBeginMapActivation())
					{
						return true;
					}
					MansionTerrain.BeginMapActivation();
					global::RealismSuite.FogKeeper.BeforeMapSwitch("Online map");   // the switch clears explored areas
					if (ConfigHelper.OnlineMapAdditive)
					{
						Logger.Log("online map: EXECUTE GROUP_MAP (additive)");
						Function.CallLongBlockingFunc(Hash.EXECUTE_CONTENT_CHANGESET_GROUP_FOR_ALL, Game.GenerateHash("GROUP_MAP"));
						Logger.Log("online map: EXECUTE GROUP_MAP done");
					}
					else
					{
						Logger.Log("online map: ON_ENTER_MP");
						Function.CallLongBlockingFunc(INMNative.ON_ENTER_MP);
						Logger.Log("online map: ON_ENTER_MP done");
					}
					global::RealismSuite.FogKeeper.AfterMapSwitch("Online map");
					MansionTerrain.CompleteMapActivation();
					mpLoadedAdditive = ConfigHelper.OnlineMapAdditive;
					mpMapActive = true;
					Logger.Log("online map: entity sets");
					LoadMPDLCMapMissingObjects();
					Logger.Log("online map: entity sets done");
				}
				else
				{
					Mansions.UnloadAllMansions(true);
					MansionTerrain.PrepareMapDeactivation();
					global::RealismSuite.FogKeeper.BeforeMapSwitch("story map");
					if (mpLoadedAdditive)
					{
						Logger.Log("online map: REVERT GROUP_MAP (additive)");
						Function.CallLongBlockingFunc(Hash.REVERT_CONTENT_CHANGESET_GROUP_FOR_ALL, Game.GenerateHash("GROUP_MAP"));
						Logger.Log("online map: REVERT GROUP_MAP done");
					}
					else
					{
						Logger.Log("online map: ON_ENTER_SP");
						Function.CallLongBlockingFunc(Hash.ON_ENTER_SP);
						Logger.Log("online map: ON_ENTER_SP done");
					}
					global::RealismSuite.FogKeeper.AfterMapSwitch("story map");
					MansionTerrain.MapDeactivationNativeReturned();
					MansionTerrain.CompleteMapDeactivation();
					mpMapActive = false;
				}
				return false;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"online map: {ex2.Message}");
				try
				{
					MansionTerrain.RestoreAfterAbort();
				}
				catch (Exception ex3)
				{
					ProjectData.SetProjectError(ex3);
					Exception ex4 = ex3;
					Logger.Log("online map terrain recovery: " + ex4.Message);
					ProjectData.ClearProjectError();
				}
				bool result = true;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public static void OnlineMapNearTick()
		{
			checked
			{
				if (Operators.CompareString(ConfigHelper.OnlineMapMode.ToLower(), "near", false) != 0 || Game.GameTime < mpMapLastCheck + 1000)
				{
					return;
				}
				mpMapLastCheck = Game.GameTime;
				try
				{
					Vector3 position = PP.Position;
					float num = ConfigHelper.OnlineMapRadius;
					float num2 = (float)ConfigHelper.OnlineMapRadius + 100f;
					bool flag = false;
					bool flag2 = true;
					foreach (BuildingClass building in buildings)
					{
						float val = position.DistanceTo2D(ToVector3(building.BuildingInPos));
						float val2 = ((building.GarageInPos != Vector3.Zero) ? position.DistanceTo2D(building.GarageInPos) : 9999f);
						float num3 = Math.Min(val, val2);
						if (num3 < num)
						{
							flag = true;
						}
						if (num3 < num2)
						{
							flag2 = false;
						}
					}
					foreach (Mansion mansion in Mansions.MansionList)
					{
						if (mansion.Enabled)
						{
							float num4 = position.DistanceTo2D(mansion.Reference);
							if (num4 < num)
							{
								flag = true;
							}
							if (num4 < num2)
							{
								flag2 = false;
							}
						}
					}
					if (PI != 0 && Stability.IsPinnedInterior(PI))
					{
						flag = true;
						flag2 = false;
					}
					if (ExecutiveGarage.HoldsOnlineMap() || OnlineExpansion.HoldsOnlineMap() || CharacterReturnResidency.HoldsOnlineMap())
					{
						flag = true;
						flag2 = false;
					}
					if (flag && !mpMapActive)
					{
						SwitchOnlineMap(true);
						mpMapOnAt = Game.GameTime;
						mpMapFarSince = 0;
					}
					else if (flag2 && mpMapActive)
					{
						if (mpMapFarSince == 0)
						{
							mpMapFarSince = Game.GameTime;
						}
						if (Game.GameTime > mpMapFarSince + 10000 && Game.GameTime > mpMapOnAt + 60000)
						{
							SwitchOnlineMap(false);
							mpMapFarSince = 0;
						}
					}
					else
					{
						mpMapFarSince = 0;
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log($"OnlineMapNearTick: {ex2.Message}");
					ProjectData.ClearProjectError();
				}
			}
		}

		public static void LoadMPDLCMapMissingObjects()
		{
			ActivateInteriorEntitySet(new Vector3(-1155.31f, -1518.57f, 10.63f), "swap_clean_apt", "layer_whiskey", "layer_sextoys_a", "swap_mrJam_A", "swap_sofa_A");
			ActivateInteriorEntitySet(new Vector3(-802.311f, 175.055984f, 72.84459f), "V_Michael_bed_tidy", "V_Michael_L_Items", "V_Michael_S_Items", "V_Michael_D_Items", "V_Michael_M_Items", "Michael_premier", "V_Michael_plane_ticket");
			ActivateInteriorEntitySet(new Vector3(-9.96562f, -1438.54f, 31.1015f), "V_57_FranklinStuff");
			ActivateInteriorEntitySet(new Vector3(0.91675f, 528.485f, 174.628f), "franklin_settled", "franklin_unpacking", "bong_and_wine", "progress_flyer", "progress_tshirt", "progress_tux", "unlocked");
			ActivateInteriorEntitySet(new Vector3(-172.983f, 494.033f, 137.654f), "Stilts_Kitchen_Window");
			ActivateInteriorEntitySet(new Vector3(340.941f, 437.18f, 149.39f), "Stilts_Kitchen_Window");
			ActivateInteriorEntitySet(new Vector3(373.023f, 416.105f, 145.701f), "Stilts_Kitchen_Window");
			ActivateInteriorEntitySet(new Vector3(-676.127f, 588.612f, 145.17f), "Stilts_Kitchen_Window");
			ActivateInteriorEntitySet(new Vector3(-763.107f, 615.906f, 144.14f), "Stilts_Kitchen_Window");
			ActivateInteriorEntitySet(new Vector3(-857.798f, 682.563f, 152.653f), "Stilts_Kitchen_Window");
			ActivateInteriorEntitySet(new Vector3(-572.61f, 653.13f, 145.63f), "Stilts_Kitchen_Window");
			ActivateInteriorEntitySet(new Vector3(120.5f, 549.952f, 184.097f), "Stilts_Kitchen_Window");
			ActivateInteriorEntitySet(new Vector3(-1288f, 440.748f, 97.6946f), "Stilts_Kitchen_Window");
		}

		public static void ActivateInteriorEntitySet(Vector3 pos, params string[] entities)
		{
			int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, pos.X, pos.Y, pos.Z);
			Logger.Log(string.Format("entity set: interior {0} at {1}: {2}", num, pos, string.Join(",", entities)));
			if (num == 0)
			{
				Logger.Log($"ActivateInteriorEntitySet: no interior at {pos}");
				return;
			}
			foreach (string text in entities)
			{
				Function.Call(INMNative.ACTIVATE_INTERIOR_ENTITY_SET, num, text);
			}
			Function.Call(Hash.REFRESH_INTERIOR, num);
		}

		public static Prop GetClosestProp(this Vector3 pos, float radius, string model, bool persistence)
		{
			return Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, pos.X, pos.Y, pos.Z, radius, GetHashKey(model), persistence, false, true);
		}

		public static void RequestIPL(string ipl)
		{
			Function.Call(Hash.REQUEST_IPL, ipl);
		}

		public static float TopSpeed(this Vehicle veh)
		{
			return Function.Call<float>(INMNative.GET_VEHICLE_ESTIMATED_MAX_SPEED, veh.Model.GetHashCode());
		}

		public static float Acceleration(Vehicle veh)
		{
			return Function.Call<float>(Hash.GET_VEHICLE_ACCELERATION, veh);
		}

		public static void DrawMPCarStats(this Scaleform sf, Vehicle veh, Vector3 scale = default(Vector3))
		{
			if (scale == default(Vector3))
			{
				scale = new Vector3(6f, 3.5f, 1f);
			}
			if (veh.IsOnScreen && veh.Position.DistanceToSquared(PP.Position) <= 100f)
			{
				sf.CallFunction("SET_VEHICLE_INFOR_AND_STATS", V2Compat.FriendlyName(veh), Game.GetLocalizedString("MP_PROP_CAR0"), "MPCarHUD", Make(veh), Game.GetLocalizedString("FMMC_VEHST_0"), Game.GetLocalizedString("FMMC_VEHST_1"), Game.GetLocalizedString("FMMC_VEHST_2"), Game.GetLocalizedString("FMMC_VEHST_3"), TopSpeed(veh) * 100f, veh.MaxBraking * 100f, veh.Acceleration * 100f, veh.MaxTraction * 100f);
				sf.Render3D(new Vector3(veh.Position.X, veh.Position.Y, veh.Position.Z + 3f), GameplayCamera.Rotation, scale);
			}
		}

		public static BlipColor GetBlipColor(this BuildingClass bd)
		{
			if (bd.Apartments.Count == 1)
			{
				switch (bd.Apartments.FirstOrDefault().Owner())
				{
				case eOwner.Michael:
					return BlipColor.Michael;
				case eOwner.Franklin:
					return BlipColor.Franklin;
				case eOwner.Trevor:
					return BlipColor.Trevor;
				case eOwner.Others:
					return BlipColor.Yellow;
				}
			}
			else
			{
				int ownerCount = GetOwnerCount(bd, eOwner.Michael);
				int ownerCount2 = GetOwnerCount(bd, eOwner.Franklin);
				int ownerCount3 = GetOwnerCount(bd, eOwner.Trevor);
				int ownerCount4 = GetOwnerCount(bd, eOwner.Others);
				int num = new int[4] { ownerCount, ownerCount2, ownerCount3, ownerCount4 }.Max();
				if (num == ownerCount)
				{
					return BlipColor.Michael;
				}
				if (num == ownerCount2)
				{
					return BlipColor.Franklin;
				}
				if (num == ownerCount3)
				{
					return BlipColor.Trevor;
				}
				if (num == ownerCount4)
				{
					return BlipColor.Yellow;
				}
			}
			return BlipColor.White;
		}

		public static int GetOwnerCount(this BuildingClass bd, eOwner ow)
		{
			_Closure_0024__145_002D0 closure_0024__145_002D = new _Closure_0024__145_002D0();
			closure_0024__145_002D._0024VB_0024Local_ow = ow;
			return bd.Apartments.Where(closure_0024__145_002D._Lambda_0024__0).Count();
		}

		public static string TruncateString(this string str, int len)
		{
			if (len > str.Length)
			{
				return str;
			}
			return str.Substring(0, len);
		}

		public static string OutsideVehicleIdentity(int garageId, int uid, int modelHash)
		{
			if (garageId == 0 || uid == 0 || modelHash == 0)
			{
				return null;
			}
			return garageId.ToString(CultureInfo.InvariantCulture) + ":" + uid.ToString(CultureInfo.InvariantCulture) + ":" + modelHash.ToString(CultureInfo.InvariantCulture);
		}

		private static string VehicleIdentity(Vehicle veh)
		{
			if ((object)veh == null || !veh.Exists())
			{
				return null;
			}
			return OutsideVehicleIdentity(veh.GetInt(vehIdDecor), veh.GetInt(vehUidDecor), veh.Model.Hash);
		}

		internal static void RememberVehicleOwner(Vehicle veh, eOwner owner)
		{
			string text = VehicleIdentity(veh);
			if (text != null && owner >= eOwner.Michael && owner <= eOwner.Others)
			{
				outsideVehicleOwners[text] = owner;
			}
		}

		public static bool RegisterOutsideVehicle(Vehicle veh, eOwner owner)
		{
			string text = VehicleIdentity(veh);
			if (text == null)
			{
				return false;
			}
			eOwner value;
			if (outsideVehicleOwners.TryGetValue(text, out value))
			{
				owner = value;
			}
			if (owner < eOwner.Michael || owner > eOwner.Others)
			{
				owner = Owner(veh);
			}
			if (owner < eOwner.Michael || owner > eOwner.Others)
			{
				return false;
			}
			RememberVehicleOwner(veh, owner);
			if (!outVehicleList.Contains(veh))
			{
				outVehicleList.Add(veh);
			}
			try
			{
				Blip blip = SpaBlips.AddSpaBlip(veh);
				if ((object)blip != null && blip.Exists())
				{
					blip.Sprite = GetProperBlipSprite(veh.Model);
					switch (owner)
					{
					case eOwner.Michael:
						blip.Color = BlipColor.Michael;
						break;
					case eOwner.Franklin:
						blip.Color = BlipColor.Franklin;
						break;
					case eOwner.Trevor:
						blip.Color = BlipColor.Trevor;
						break;
					default:
						blip.Color = BlipColor.Yellow;
						break;
					}
					blip.IsShortRange = true;
					blip.Name = $"{Make(veh)} {V2Compat.FriendlyName(veh)}";
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Outside vehicle blip: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			return true;
		}

		public static void ClearOutsideVehicleOwners()
		{
			outsideVehicleOwners.Clear();
		}

		public static eOwner Owner(this Vehicle veh)
		{
			if ((object)veh == null || !veh.Exists())
			{
				return eOwner.Nobody;
			}
			try
			{
				string text = VehicleIdentity(veh);
				if (text == null)
				{
					return eOwner.Nobody;
				}
				eOwner value;
				if (outsideVehicleOwners.TryGetValue(text, out value))
				{
					return value;
				}
				int @int = veh.GetInt(vehIdDecor);
				int int2 = veh.GetInt(vehUidDecor);
				string text2 = Mansions.GarageFolderForId(@int);
				VehicleData value2 = default(VehicleData);
				if (text2 != null && new VehicleData(Path.Combine(grgXmlPath + text2, int2.ToString(CultureInfo.InvariantCulture) + ".xml")).TryRead(ref value2) && value2.UniqueID == int2 && value2.ApartmentID == @int && value2.Hash == veh.Model.Hash && value2.Owner >= eOwner.Michael && value2.Owner <= eOwner.Others)
				{
					outsideVehicleOwners[text] = value2.Owner;
					return value2.Owner;
				}
				Blip blip = V2Compat.CurrentBlip(veh);
				if ((object)blip != null && blip.Exists())
				{
					switch (blip.Color)
					{
					case BlipColor.Franklin:
						value = eOwner.Franklin;
						break;
					case BlipColor.Michael:
						value = eOwner.Michael;
						break;
					case BlipColor.Trevor:
						value = eOwner.Trevor;
						break;
					case BlipColor.Yellow:
						value = eOwner.Others;
						break;
					default:
						return eOwner.Nobody;
					}
					outsideVehicleOwners[text] = value;
					return value;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Outside vehicle owner: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			return eOwner.Nobody;
		}

		public static bool TryCashAfterChange(int balance, int delta, ref int updated)
		{
			updated = balance;
			checked
			{
				long num = unchecked((long)balance) + unchecked((long)delta);
				if (balance < 0 || num < 0 || num > int.MaxValue)
				{
					return false;
				}
				updated = (int)num;
				return true;
			}
		}

		private static string MoneyStatName(eOwner owner)
		{
			switch (owner)
			{
			case eOwner.Michael:
				return "SP0_TOTAL_CASH";
			case eOwner.Franklin:
				return "SP1_TOTAL_CASH";
			case eOwner.Trevor:
				return "SP2_TOTAL_CASH";
			default:
				return null;
			}
		}

		public static bool TryGetOwnerMoney(eOwner owner, ref int balance)
		{
			balance = 0;
			string text = MoneyStatName(owner);
			if (text == null)
			{
				return false;
			}
			try
			{
				OutputArgument outputArgument = new OutputArgument();
				if (!Function.Call<bool>(Hash.STAT_GET_INT, Game.GenerateHash(text), outputArgument, -1))
				{
					return false;
				}
				balance = outputArgument.GetResult<int>();
				return balance >= 0;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Character balance read: " + ex2.Message);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public static bool TryChangeOwnerMoney(eOwner owner, int delta)
		{
			int balance = default(int);
			int updated = default(int);
			if (!TryGetOwnerMoney(owner, ref balance) || !TryCashAfterChange(balance, delta, ref updated))
			{
				return false;
			}
			if (delta == 0)
			{
				return true;
			}
			try
			{
				return Function.Call<bool>(Hash.STAT_SET_INT, Game.GenerateHash(MoneyStatName(owner)), updated, true);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Character balance write: " + ex2.Message);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public static void SetShopControllerGlobal()
		{
			try
			{
				if (ShopControllerGlobal > 0)
				{
					GlobalVariable.Get(ShopControllerGlobal).Write(1);
					Logger.Log($"shop_controller global {ShopControllerGlobal} set to 1");
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"SetShopControllerGlobal: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
		}

		public static BlipSprite GetProperBlipSprite(this Model model)
		{
			bool flag = true;
			if (flag == (model == "akula"))
			{
				return BlipSprite.Akula;
			}
			if (flag == (model == "alphaz1"))
			{
				return BlipSprite.AlphaZ1;
			}
			if (flag == (model == "apc"))
			{
				return BlipSprite.APC;
			}
			if (flag == (model == "boxville5"))
			{
				return BlipSprite.ArmoredBoxville;
			}
			if (flag == (model == "stockade3") || flag == (model == "stockade"))
			{
				return BlipSprite.ArmoredTruck;
			}
			if (flag == (model == "avenger"))
			{
				return BlipSprite.Avenger;
			}
			if (flag == (model == "strikeforce"))
			{
				return BlipSprite.B11StrikeForce;
			}
			if (flag == (model == "barrage"))
			{
				return BlipSprite.Barrage;
			}
			if (flag == (model == "blimp"))
			{
				return BlipSprite.Blimp;
			}
			if (flag == (model == "blimp2") || flag == (model == "blimp3"))
			{
				return BlipSprite.Blimp2;
			}
			if (flag == (model == "bombushka"))
			{
				return BlipSprite.Bombushka;
			}
			if (flag == (model == "bruiser") || flag == (model == "bruiser2") || flag == (model == "bruiser3"))
			{
				return BlipSprite.Bruiser;
			}
			if (flag == (model == "brutus") || flag == (model == "brutus2") || flag == (model == "brutus3"))
			{
				return BlipSprite.Brutus;
			}
			if (flag == (model == "bus") || flag == (model == "airbus"))
			{
				return BlipSprite.Bus;
			}
			if (flag == (model == "taxi"))
			{
				return BlipSprite.Cab;
			}
			if (flag == (model == "caracara"))
			{
				return BlipSprite.Caracara;
			}
			if (flag == (model == "cargobob") || flag == (model == "cargobob2") || flag == (model == "cargobob3") || flag == (model == "cargobob4"))
			{
				return BlipSprite.Cargobob;
			}
			if (flag == (model == "cerberus") || flag == (model == "cerberus2") || flag == (model == "cerberus3"))
			{
				return BlipSprite.Cerberus;
			}
			if (flag == (model == "chernobog"))
			{
				return BlipSprite.Chernobog;
			}
			if (flag == (model == "deathbike") || flag == (model == "deathbike2") || flag == (model == "deathbike3"))
			{
				return BlipSprite.Deathbike;
			}
			if (flag == (model == "deluxo"))
			{
				return BlipSprite.Deluxo;
			}
			if (flag == (model == "dinghy") || flag == (model == "dinghy2") || flag == (model == "dinghy3") || flag == (model == "dinghy4"))
			{
				return BlipSprite.Dinghy;
			}
			if (flag == (model == "dominator3"))
			{
				return BlipSprite.Dominator;
			}
			if (flag == (model == "dune3"))
			{
				return BlipSprite.DuneFAV;
			}
			if (flag == (model == "pbus2"))
			{
				return BlipSprite.FestivalBus;
			}
			if (flag == (model == "insurgent") || flag == (model == "insurgent2") || flag == (model == "insurgent3"))
			{
				return BlipSprite.GunCar;
			}
			if (flag == (model == "halftrack"))
			{
				return BlipSprite.HalfTrack;
			}
			if (flag == (model == "havok"))
			{
				return BlipSprite.Havok;
			}
			if (flag == (model == "howard"))
			{
				return BlipSprite.HowardNX25;
			}
			if (flag == (model == "hunter"))
			{
				return BlipSprite.Hunter;
			}
			if (flag == (model == "impaler") || flag == (model == "impaler2") || flag == (model == "impaler3") || flag == (model == "impaler4"))
			{
				return BlipSprite.Impaler;
			}
			if (flag == (model == "imperator") || flag == (model == "imperator2") || flag == (model == "imperator3"))
			{
				return BlipSprite.Imperator;
			}
			if (flag == (model == "issi3") || flag == (model == "issi4") || flag == (model == "issi5") || flag == (model == "issi6"))
			{
				return BlipSprite.Issi;
			}
			if (flag == (model == "khanjali"))
			{
				return BlipSprite.Khanjali;
			}
			if (flag == (model == "limo"))
			{
				return BlipSprite.Limo;
			}
			if (flag == (model == "menacer"))
			{
				return BlipSprite.Menacer;
			}
			if (flag == (model == "mogul"))
			{
				return BlipSprite.Mogul;
			}
			if (flag == (model == "mule4"))
			{
				return BlipSprite.MuleCustom;
			}
			if (flag == (model == "oppressor"))
			{
				return BlipSprite.Oppressor;
			}
			if (flag == (model == "oppressor2"))
			{
				return BlipSprite.OppressorMkII;
			}
			if (flag == (model == "nokota"))
			{
				return BlipSprite.P45Nokota;
			}
			if (flag == (model == "phantom2"))
			{
				return BlipSprite.PhantomWedge;
			}
			if (flag == (model == "pounder2"))
			{
				return BlipSprite.PounderCustom;
			}
			if (flag == (model == "pyro"))
			{
				return BlipSprite.Pyro;
			}
			if (flag == (model == "dune4") || flag == (model == "dune5"))
			{
				return BlipSprite.RampBuggy;
			}
			if (flag == (model == "riot2"))
			{
				return BlipSprite.RCV;
			}
			if (flag == (model == "rcbandito"))
			{
				return BlipSprite.RCVehicle;
			}
			if (flag == (model == "voltic2"))
			{
				return BlipSprite.RocketVoltic;
			}
			if (flag == (model == "rogue"))
			{
				return BlipSprite.Rogue;
			}
			if (flag == (model == "ruiner2"))
			{
				return BlipSprite.Ruiner2000;
			}
			if (flag == (model == "monster") || flag == (model == "monster2") || flag == (model == "monster3"))
			{
				return BlipSprite.Sasquatch;
			}
			if (flag == (model == "scarab") || flag == (model == "scarab2") || flag == (model == "scarab3"))
			{
				return BlipSprite.Scarab;
			}
			if (flag == (model == "scramjet"))
			{
				return BlipSprite.Scramjet;
			}
			if (flag == (model == "seabreeze"))
			{
				return BlipSprite.Seabreeze;
			}
			if (flag == (model == "seashark") || flag == (model == "seashark2") || flag == (model == "seashark3"))
			{
				return BlipSprite.Seashark;
			}
			if (flag == (model == "seasparrow"))
			{
				return BlipSprite.SeaSparrow;
			}
			if (flag == (model == "slamvan4") || flag == (model == "slamvan5") || flag == (model == "slamvan6"))
			{
				return BlipSprite.Slamvam;
			}
			if (flag == (model == "speedo4"))
			{
				return BlipSprite.SpeedoCustom;
			}
			if (flag == (model == "starling"))
			{
				return BlipSprite.Starling;
			}
			if (flag == (model == "stromberg"))
			{
				return BlipSprite.Stromberg;
			}
			if (flag == (model == "submersible"))
			{
				return BlipSprite.Sub;
			}
			if (flag == (model == "rhino"))
			{
				return BlipSprite.Tank;
			}
			if (flag == (model == "technical2"))
			{
				return BlipSprite.TechnicalAqua;
			}
			if (flag == (model == "terbyte"))
			{
				return BlipSprite.Terrorbyte;
			}
			if (flag == (model == "thruster"))
			{
				return BlipSprite.Thruster;
			}
			if (flag == (model == "towtruck"))
			{
				return BlipSprite.TowTruck;
			}
			if (flag == (model == "towtruck2"))
			{
				return BlipSprite.TowTruck2;
			}
			if (flag == (model == "tula"))
			{
				return BlipSprite.Tula;
			}
			if (flag == (model == "limo2"))
			{
				return BlipSprite.TurretedLimo;
			}
			if (flag == (model == "microlight"))
			{
				return BlipSprite.Ultralight;
			}
			if (flag == (model == "molotok"))
			{
				return BlipSprite.V65Molotok;
			}
			if (flag == (model == "volatol"))
			{
				return BlipSprite.Volatol;
			}
			if (flag == (model == "wastelander"))
			{
				return BlipSprite.Wastelander;
			}
			if (flag == (model == "tampa3"))
			{
				return BlipSprite.WeaponizedTampa;
			}
			if (flag == (model == "trailersmall2"))
			{
				return BlipSprite.WeaponizedTrailer;
			}
			if (flag == (model == "zr380") || flag == (model == "zr3802") || flag == (model == "zr3803"))
			{
				return BlipSprite.ZR380;
			}
			if (flag == model.IsBike)
			{
				return BlipSprite.PersonalVehicleBike;
			}
			if (flag == model.IsBoat)
			{
				return BlipSprite.Boat;
			}
			if (flag == model.IsHelicopter)
			{
				return BlipSprite.Helicopter;
			}
			if (flag == model.IsPlane)
			{
				return BlipSprite.Plane;
			}
			if (flag == model.IsQuadBike)
			{
				return BlipSprite.QuadBike;
			}
			if (flag == model.IsCargobob)
			{
				return BlipSprite.Cargobob;
			}
			return BlipSprite.PersonalVehicleCar;
		}

		public static void StartScreenFx(string fxName, int duration, bool looped, bool stopAfterDuration)
		{
			Function.Call(INMNative.ANIMPOSTFX_PLAY, fxName, duration, looped);
			Script.Wait(duration);
			if (stopAfterDuration)
			{
				StopScreenFx(fxName);
			}
		}

		public static void StopScreenFx(string fxName)
		{
			Function.Call(INMNative.ANIMPOSTFX_STOP, fxName);
		}
	}
}
