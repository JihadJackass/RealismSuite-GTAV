using System;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace GTAO_DiamondCasino
{
	public class Valet : Script
	{
		public static bool inExit_Cutscene;

		public static bool inEnter_Cutscene;

		public static bool valetChecker;

		public static bool UseValetInDoorAnim = true;

		public static Prop CasinoDoor_Enter;

		public static Prop CasinoDoor_Exit;

		public static Prop CasinoDoor_Exit2;

		public static Camera enterCamera;

		public static int EnterSyncScene;

		public static Ped ValetPed;

		public static Prop ValetStand;

		public static bool CreatedValet;

		public static int ExitAnimStage = 1;

		public static bool RunMethodNow()
		{
			bool result = true;
			if (Class1.IsinInterior)
			{
				_Debug.Valet = 0;
				result = false;
			}
			return result;
		}

		public static string LoadDict(string dict)
		{
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, dict))
			{
				Function.Call(Hash.REQUEST_ANIM_DICT, dict);
				Script.Yield();
			}
			return dict;
		}

		public static void EnterCasino()
		{
			Function.Call(Hash.REMOVE_IPL, "vw_dlc_casino_door");
			Script.Wait(100);
			Function.Call(Hash.REQUEST_IPL, "vw_dlc_casino_door");
			Class1.LoadDict("anim@amb@casino@valet@intro@");
			Class1.LoadDict("anim@apt_trans@hinge_l");
			Class1.LoadDict("anim@amb@casino@valet@intro@");
			Class1.LoadDict("anim@apt_trans@hinge_l");
			Script.Wait(100);
			while (CasinoDoor_Enter == null)
			{
				CasinoDoor_Enter = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 927.73f, 49.6f, 81.54f, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_casino_door_02a"), 0, 0, 0);
				Script.Wait(1);
			}
			if (!(CasinoDoor_Enter != null) || !(ValetStand != null))
			{
				return;
			}
			if (ValetPed != null)
			{
				if (UseValetInDoorAnim)
				{
					LoadDict("anim@amb@casino@valet@intro@");
					inEnter_Cutscene = true;
					valetChecker = true;
					Vector3 vector = new Vector3(926.3f, 47.21f, 81.6f);
					Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, ValetPed);
					int num = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, vector.X, vector.Y, vector.Z, 0f, 0f, -120f);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, num, "anim@amb@casino@valet@intro@", "intro_mp_m_freemode_01", 1000f, -8f, 0, 0, 1148846080, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, ValetPed, num, "anim@amb@casino@valet@intro@", "intro_s_m_y_valet_01", 1000f, -8f, 0, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoDoor_Enter, num, "intro_vw_prop_vw_casino_door_02a", "anim@amb@casino@valet@intro@", 1000f, 1000f, 0, 1148846080);
					enterCamera = Function.Call<Camera>(Hash.CREATE_CAM, "DEFAULT_ANIMATED_CAMERA", true);
					Function.Call(Hash.PLAY_CAM_ANIM, enterCamera, "intro_cam", "anim@amb@casino@valet@intro@", vector.X, vector.Y, vector.Z, 0f, 0f, -121.32f, 0, 2);
					Function.Call(Hash.PLAY_FACIAL_ANIM, Game.Player.Character, "intro_mp_m_freemode_facial", "anim@amb@casino@valet@intro@");
					Function.Call(Hash.PLAY_FACIAL_ANIM, ValetPed, "intro_s_m_y_valet_facial", "anim@amb@casino@valet@intro@");
					Function.Call(Hash.TASK_PLAY_ANIM, Game.Player.Character, "anim@amb@casino@valet@intro@", "intro_mp_m_freemode_01", 8f, 8f, -1, 50, -1f, true, true, true);
					enterCamera.IsActive = true;
					World.RenderingCamera = enterCamera;
					EnterSyncScene = num;
				}
				else
				{
					LoadDict("anim@apt_trans@hinge_l");
					inEnter_Cutscene = true;
					valetChecker = true;
					Vector3 vector2 = new Vector3(925.809f, 46.759f, 80.0989f);
					int num2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, vector2.X, vector2.Y, vector2.Z, 0f, 0f, -121.32f);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, num2, "anim@apt_trans@hinge_l", "ext_player", 1000f, -8f, 0, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoDoor_Enter, num2, "ext_door", "anim@apt_trans@hinge_l", 1000f, 1000f, 0, 1148846080);
					Function.Call(Hash.TASK_PLAY_ANIM, Game.Player.Character, "anim@apt_trans@hinge_l", "ext_player", 8f, 8f, -1, 50, -1f, true, true, true);
					enterCamera = GlobalHelper.CreateCamera(new Vector3(918.7504f, 60.33242f, 80.89931f), new Vector3(0f, 0f, 204.5259f), 45f);
					enterCamera.IsActive = true;
					World.RenderingCamera = enterCamera;
					Function.Call(Hash.SHAKE_CAM, enterCamera, "HAND_SHAKE", 0.25f);
					EnterSyncScene = num2;
				}
			}
			else
			{
				LoadDict("anim@apt_trans@hinge_l");
				inEnter_Cutscene = true;
				valetChecker = true;
				Vector3 vector3 = new Vector3(925.809f, 46.759f, 80.0989f);
				int num3 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, vector3.X, vector3.Y, vector3.Z, 0f, 0f, -121.32f);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, num3, "anim@apt_trans@hinge_l", "ext_player", 1000f, -8f, 0, 0, 1148846080, 0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoDoor_Enter, num3, "ext_door", "anim@apt_trans@hinge_l", 1000f, 1000f, 0, 1148846080);
				Function.Call(Hash.TASK_PLAY_ANIM, Game.Player.Character, "anim@apt_trans@hinge_l", "ext_player", 8f, 8f, -1, 50, -1f, true, true, true);
				enterCamera = GlobalHelper.CreateCamera(new Vector3(918.7504f, 60.33242f, 80.89931f), new Vector3(0f, 0f, 204.5259f), 45f);
				enterCamera.IsActive = true;
				World.RenderingCamera = enterCamera;
				Function.Call(Hash.SHAKE_CAM, enterCamera, "HAND_SHAKE", 0.25f);
			}
		}

		public static void ExitCasino()
		{
			CasinoDoor_Exit = null;
			CasinoDoor_Exit2 = null;
			Function.Call(Hash.REMOVE_IPL, "vw_dlc_casino_door");
			Script.Wait(100);
			Function.Call(Hash.REQUEST_IPL, "vw_dlc_casino_door");
			Class1.LoadDict("anim@amb@casino@valet@intro@");
			Class1.LoadDict("anim@apt_trans@hinge_l");
			Class1.LoadDict("anim@amb@casino@valet@intro@");
			Class1.LoadDict("anim@apt_trans@hinge_l");
			Class1.LoadDict("anim@apt_trans@hinge_r");
			Class1.LoadDict("anim@apt_trans@hinge_r");
			if (ExitAnimStage == 0)
			{
				Game.Player.Character.Position = new Vector3(1090.39f, 207.69f, -50f);
				Function.Call(Hash.REFRESH_INTERIOR, 275201);
				Script.Wait(100);
				int num = Game.GameTime + 5000;
				while (CasinoDoor_Exit == null && Game.GameTime < num)
				{
					CasinoDoor_Exit = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1090.93f, 205.36f, -48.49f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_casino_door_02a"), 0, 0, 0);
					Script.Wait(1);
				}
				if (CasinoDoor_Exit != null)
				{
					if (enterCamera != null)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						enterCamera.IsActive = false;
						enterCamera.Delete();
					}
					valetChecker = true;
					Vector3 offsetPosition = CasinoDoor_Exit.GetOffsetPosition(new Vector3(0.6f, -0.1f, -1.45f));
					int num2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0f, 0f, CasinoDoor_Exit.Heading);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, num2, "anim@apt_trans@hinge_l", "ext_player", 1000f, -8f, 0, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoDoor_Exit, num2, "ext_door", "anim@apt_trans@hinge_l", 1000f, 1000f, 0, 1148846080);
					Function.Call(Hash.TASK_PLAY_ANIM, Game.Player.Character, "anim@apt_trans@hinge_l", "ext_player", 8f, 8f, -1, 50, -1f, true, true, true);
					enterCamera = GlobalHelper.CreateCamera(new Vector3(1092.617f, 206.18f, -48.2f), new Vector3(0f, 0f, 73f), 45f);
					enterCamera.IsActive = true;
					World.RenderingCamera = enterCamera;
					Function.Call(Hash.SHAKE_CAM, enterCamera, "HAND_SHAKE", 0.25f);
					EnterSyncScene = num2;
					inExit_Cutscene = true;
				}
			}
			if (ExitAnimStage != 1)
			{
				return;
			}
			CasinoPeds.DeleteGamingAreaPeds();
			CasinoPeds.DeleteHallAPeds();
			CasinoPeds.DeleteHallBPeds();
			CasinoPeds.DeleteMainHallPeds();
			int num3 = 274689;
			Function.Call(Hash.REFRESH_INTERIOR, num3);
			Script.Wait(100);
			int num4 = Game.GameTime + 5000;
			while (CasinoDoor_Exit2 == null && Game.GameTime < num4)
			{
				CasinoDoor_Exit2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 926.23f, 47.21f, 81.54f, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_casino_door_02a"), 0, 0, 0);
				Script.Wait(1);
			}
			if (CasinoDoor_Exit2 != null)
			{
				if (enterCamera != null)
				{
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
					enterCamera.IsActive = false;
					enterCamera.Delete();
				}
				valetChecker = true;
				Vector3 offsetPosition2 = CasinoDoor_Exit2.GetOffsetPosition(new Vector3(0.6f, -0.1f, -1.45f));
				Vector3 offsetPosition3 = CasinoDoor_Exit2.GetOffsetPosition(new Vector3(-0.6f, -0.1f, -1.45f));
				float heading = CasinoDoor_Exit2.Heading;
				int num5 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition2.X, offsetPosition2.Y, offsetPosition2.Z, 0f, 0f, heading);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, num5, "anim@apt_trans@hinge_r", "ext_player", 1000f, -8f, 0, 0, 1148846080, 0);
				int num6 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition3.X, offsetPosition3.Y, offsetPosition3.Z, 0f, 0f, heading - 180f);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CasinoDoor_Exit2, num6, "ext_door", "anim@apt_trans@hinge_r", 1000f, 1000f, 0, 1148846080);
				Function.Call(Hash.TASK_PLAY_ANIM, Game.Player.Character, "anim@apt_trans@hinge_r", "ext_player", 8f, 8f, -1, 50, -1f, true, true, true);
				enterCamera = GlobalHelper.CreateCamera(new Vector3(928.26f, 51.3f, 81.85f), new Vector3(0f, 0f, 120f), 45f);
				enterCamera.IsActive = true;
				World.RenderingCamera = enterCamera;
				Function.Call(Hash.SHAKE_CAM, enterCamera, "HAND_SHAKE", 0.25f);
				EnterSyncScene = num5;
				inExit_Cutscene = true;
			}
		}

		public Valet()
		{
			if (_Debug.Valet != -1)
			{
				base.Tick += OnTick;
			}
		}

		public static void OnTick(object sender, EventArgs e)
		{
			if (!inExit_Cutscene && !inEnter_Cutscene && Class1.GetDistance_VDIST2(Game.Player.Character.Position, Class1.MarkerEnter, Class1.GetDistSign.Greater, 100f) && Class1.GetDistance_VDIST2(Game.Player.Character.Position, Class1.MarkerExit, Class1.GetDistSign.Greater, 20f))
			{
				_Debug.Valet = 0;
				return;
			}
			_Debug.Valet = 1;
			if (inExit_Cutscene && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, EnterSyncScene) >= 1058353216)
			{
				if (ExitAnimStage == 0)
				{
					Screen.FadeOut(500);
					Script.Wait(500);
					int num = 275201;
					Function.Call(Hash.REFRESH_INTERIOR, num);
					Game.Player.Character.Task.ClearAllImmediately();
					Game.Player.Character.Position = new Vector3(926.84f, 45.45f, 80.1f);
					Script.Wait(500);
					Screen.FadeIn(500);
					ExitAnimStage = 1;
					ExitCasino();
				}
				else if (ExitAnimStage == 1)
				{
					ExitAnimStage = 0;
					inExit_Cutscene = false;
					if (enterCamera != null)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						enterCamera.IsActive = false;
						enterCamera.Delete();
					}
					CasinoPeds.DeleteGamingAreaPeds();
					CasinoPeds.DeleteHallAPeds();
					CasinoPeds.DeleteHallBPeds();
					CasinoPeds.DeleteMainHallPeds();
					Zones.CurrentZone = Zones.ZoneIn.None;
					Screen.FadeOut(500);
					Script.Wait(500);
					Game.Player.Character.Position = Class1.MarkerEnter;
					Game.Player.Character.Heading = 54f;
					Class1.IsinInterior = false;
					Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_HEADING, 0f);
					Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_PITCH, 0f);
					Game.Player.Character.IsPositionFrozen = false;
					Function.Call(Hash.REMOVE_IPL, "vw_dlc_casino_door");
					Script.Wait(100);
					Function.Call(Hash.REQUEST_IPL, "vw_dlc_casino_door");
					Script.Wait(500);
					Screen.FadeIn(500);
					Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
				}
			}
			if (inEnter_Cutscene && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, EnterSyncScene) >= 1064353216)
			{
				CasinoPeds.DeleteGamingAreaPeds();
				CasinoPeds.SpawnHallAPeds();
				CasinoPeds.SpawnHallBPeds();
				CasinoPeds.SpawnMainHallPeds();
				Zones.CurrentZone = Zones.ZoneIn.MainHall;
				Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
				Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
				Screen.FadeOut(500);
				Script.Wait(500);
				Game.Player.Character.Position = Class1.MarkerExit;
				Game.Player.Character.Heading = 0f;
				Class1.IsinInterior = true;
				Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_HEADING, 0f);
				Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_PITCH, 0f);
				Game.Player.Character.IsPositionFrozen = false;
				Script.Wait(500);
				Screen.FadeIn(500);
				inEnter_Cutscene = false;
				if (enterCamera != null)
				{
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
					enterCamera.IsActive = false;
					enterCamera.Delete();
				}
				if (ValetPed != null)
				{
					ValetPed.Delete();
				}
				if (ValetPed != null)
				{
					ValetPed.Delete();
				}
				ValetPed = GlobalHelper.CreatePed(Class1.RequestModel("s_m_y_casino_01"), new Vector3(925.894f, 51.552f, 80.2f));
				ValetPed.IsPositionFrozen = true;
				ValetPed.Rotation = new Vector3(0f, 0f, 60f);
				ValetPed.IsPositionFrozen = true;
				ValetPed.BlockPermanentEvents = true;
				ValetPed.Task.PlayAnimation("anim@amb@casino@valet_scenario@pose_a@", "look_around_a_m_y_vinewood_01", 8f, 1f, -1, AnimationFlags.Loop, -1f);
				Class1.SetPedOutfit(ValetPed);
				ValetStand = GlobalHelper.CreateProp(Class1.RequestModel("vw_prop_vw_valet_01a"), new Vector3(925.29f, 51.83f, 80.2f), true, false);
				if (ValetStand != null)
				{
					ValetStand.IsPositionFrozen = true;
					ValetStand.Rotation = new Vector3(0f, 0f, 60f);
				}
			}
			if (!Class1.Initialized)
			{
				_Debug.Valet = 0;
				return;
			}
			if (Class1.IsinInterior)
			{
				_Debug.Valet = 0;
				CasinoDoor_Exit = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1090.93f, 205.36f, -48.49f, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_casino_door_02a"), 0, 0, 0);
				CasinoDoor_Exit2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 926.23f, 47.21f, 81.54f, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_casino_door_02a"), 0, 0, 0);
				return;
			}
			_Debug.Valet = 1;
			if (!CreatedValet)
			{
				if (!inEnter_Cutscene && !inExit_Cutscene)
				{
					if (ValetPed != null)
					{
						ValetPed.Delete();
					}
					if (ValetStand != null)
					{
						ValetStand.Delete();
					}
					ValetPed = GlobalHelper.CreatePed(Class1.RequestModel("s_m_y_casino_01"), new Vector3(925.894f, 51.552f, 80.2f));
					ValetPed.IsPositionFrozen = true;
					ValetPed.Rotation = new Vector3(0f, 0f, 60f);
					ValetPed.IsPositionFrozen = true;
					ValetPed.BlockPermanentEvents = true;
					ValetPed.Task.PlayAnimation("anim@amb@casino@valet_scenario@pose_a@", "look_around_a_m_y_vinewood_01", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					Class1.SetPedOutfit(ValetPed);
					ValetStand = GlobalHelper.CreateProp(Class1.RequestModel("vw_prop_vw_valet_01a"), new Vector3(925.29f, 51.83f, 80.2f), true, false);
					ValetStand.IsPositionFrozen = true;
					ValetStand.Rotation = new Vector3(0f, 0f, 60f);
					CreatedValet = true;
				}
			}
			else
			{
				CasinoDoor_Enter = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 927.73f, 49.6f, 81.54f, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_casino_door_02a"), 0, 0, 0);
			}
		}
	}
}
