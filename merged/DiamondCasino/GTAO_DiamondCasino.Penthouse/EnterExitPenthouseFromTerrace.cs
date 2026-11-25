using System;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace GTAO_DiamondCasino.Penthouse
{
	internal class EnterExitPenthouseFromTerrace : Script
	{
		public static Vector3 PenthouseMainEnter = new Vector3(980.87f, 56.4f, 115.16f);

		public static Vector3 PenthouseEnter = new Vector3(963.8229f, 59.32881f, 111.5f);

		public static Vector3 PenthouseEnter2 = new Vector3(966.767f, 64.20187f, 111.5f);

		public static Vector3 PenthouseExit2 = new Vector3(969.6f, 63.19f, 111.5f);

		public static Vector3 PenthouseEnterTerrace = new Vector3(967.7f, 63.76f, 111.5f);

		public static Quaternion ExitPenthouse_CameraInterior = new Quaternion(969.97f, 64.81f, 112.8f, 154.52f);

		public static Quaternion ExitPenthouse_CameraExterior = new Quaternion(969.1f, 64.96f, 113.17f, 144.3f);

		public static Camera ExitEnterPenthouseCam;

		public EnterExitPenthouseFromTerrace()
		{
			if (_Debug.EnterExitPenthouseFromTerrace != -1)
			{
				base.Tick += OnTick;
			}
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

		public static void OnTick(object sender, EventArgs e)
		{
			if (Class1.Purchased_Penthouse == 0)
			{
				return;
			}
			_Debug.EnterExitPenthouseFromTerrace = 0;
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, PenthouseMainEnter, Class1.GetDistSign.Greater, 10f) && Class1.GetDistance_VDIST2(Game.Player.Character.Position, PenthouseEnter2, Class1.GetDistSign.Greater, 10f) && Class1.GetDistance_VDIST2(Game.Player.Character.Position, PenthouseExit2, Class1.GetDistSign.Greater, 10f))
			{
				_Debug.EnterExitPenthouseFromTerrace = 0;
				return;
			}
			_Debug.EnterExitPenthouseFromTerrace = 1;
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, PenthouseMainEnter, Class1.GetDistSign.Less, 10f) && Class1.GetDistance_VDIST2(Game.Player.Character.Position, PenthouseMainEnter, Class1.GetDistSign.Less, 2f))
			{
				Game.DisableControlThisFrame(Control.Phone);
				Class1.DisplayHelpTextThisFrame("Press ~INPUT_CELLPHONE_DOWN~ to take elevator down to casino.");
				if (Game.IsControlJustPressed(Control.PhoneDown))
				{
					Screen.FadeOut(500);
					CasinoPeds.DeleteGamingAreaPeds();
					CasinoPeds.DeleteHallAPeds();
					CasinoPeds.DeleteHallAPeds();
					CasinoPeds.DeleteMainHallPeds();
					Elevators.Casino_IsElevatorDoorLClosing = false;
					Elevators.Casino_IsElevatorDoorRClosing = false;
					Elevators.Garage_IsElevatorDoorLClosing = true;
					Elevators.Garage_IsElevatorDoorRClosing = true;
					Zones.CurrentZone = Zones.ZoneIn.MainHall;
					Script.Wait(2000);
					CasinoPeds.SpawnMainHallPeds();
					CasinoPeds.SpawnHallAPeds();
					CasinoPeds.SpawnHallBPeds();
					Elevators.Casino_ElavatorDoorL = null;
					Elevators.Casino_ElavatorDoorR = null;
					Screen.FadeIn(500);
					PenthouseInterior.RefreshPenthouse();
					Elevators.PlayExitElevator(new Tuple<Elevators.DoorType, Elevators.DoorType>(Elevators.DoorType.Casino_ElvDoorL, Elevators.DoorType.Casino_ElvDoorR));
					Class1.IsinInterior = true;
					PenthouseGarage.DeleteVehicles();
					PenthouseGarage.InGarage = false;
					PenthouseStuff.IsInPenhouse = false;
				}
			}
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, PenthouseExit2, Class1.GetDistSign.Less, 10f) && Class1.GetDistance_VDIST2(Game.Player.Character.Position, PenthouseExit2, Class1.GetDistSign.Less, 2f))
			{
				Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to exit penthouse onto terrace.");
				if (Game.IsControlJustPressed(Control.Context))
				{
					LoadDict("anim@apt_trans@hinge_l");
					if (ExitEnterPenthouseCam != null)
					{
						Game.Player.Character.IsInvincible = false;
						Game.Player.Character.IsPositionFrozen = false;
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						ExitEnterPenthouseCam.IsActive = false;
						ExitEnterPenthouseCam.Delete();
					}
					Game.Player.Character.Position = PenthouseExit2;
					Game.Player.Character.Heading = 58f;
					float heading = Game.Player.Character.Heading;
					Vector3 offsetPosition = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0.25f, 0f));
					int num = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z - 0.65f, 0.0, 0.0, heading, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, num, LoadDict("anim@apt_trans@hinge_l"), "ext_player", 16f, -16f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, num, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, num));
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, num, "ext_player", LoadDict("anim@apt_trans@hinge_l"), 16f, -16f, 0, 1148846080);
					ExitEnterPenthouseCam = GlobalHelper.CreateCamera(new Vector3(ExitPenthouse_CameraInterior.X, ExitPenthouse_CameraInterior.Y, ExitPenthouse_CameraInterior.Z + 0.3f), new Vector3(0f, 0f, ExitPenthouse_CameraInterior.W), GameplayCamera.FieldOfView);
					World.RenderingCamera = ExitEnterPenthouseCam;
					ExitEnterPenthouseCam.IsActive = true;
					ExitEnterPenthouseCam.FarClip = 2000f;
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
					Script.Wait(4000);
					if (ExitEnterPenthouseCam != null)
					{
						Game.Player.Character.IsInvincible = false;
						Game.Player.Character.IsPositionFrozen = false;
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						ExitEnterPenthouseCam.IsActive = false;
						ExitEnterPenthouseCam.Delete();
					}
					Game.Player.Character.Position = PenthouseEnterTerrace;
					Game.Player.Character.Heading = 54f;
				}
			}
			if (!Class1.GetDistance_VDIST2(Game.Player.Character.Position, PenthouseEnterTerrace, Class1.GetDistSign.Less, 10f) || !Class1.GetDistance_VDIST2(Game.Player.Character.Position, PenthouseEnterTerrace, Class1.GetDistSign.Less, 2f))
			{
				return;
			}
			Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to enter penthouse.");
			if (Game.IsControlJustPressed(Control.Context))
			{
				LoadDict("anim@apt_trans@hinge_l");
				if (ExitEnterPenthouseCam != null)
				{
					Game.Player.Character.IsInvincible = false;
					Game.Player.Character.IsPositionFrozen = false;
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
					ExitEnterPenthouseCam.IsActive = false;
					ExitEnterPenthouseCam.Delete();
				}
				Game.Player.Character.Position = PenthouseEnterTerrace;
				Game.Player.Character.Heading = -128f;
				float heading2 = Game.Player.Character.Heading;
				Vector3 offsetPosition2 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0.45f, 0f));
				int num2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition2.X, offsetPosition2.Y, offsetPosition2.Z - 0.75f, 0.0, 0.0, heading2, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, num2, LoadDict("anim@apt_trans@hinge_l"), "ext_player", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, num2, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, num2));
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, num2, "ext_player", LoadDict("anim@apt_trans@hinge_l"), 16f, -16f, 0, 1148846080);
				ExitEnterPenthouseCam = GlobalHelper.CreateCamera(new Vector3(ExitPenthouse_CameraExterior.X, ExitPenthouse_CameraExterior.Y, ExitPenthouse_CameraExterior.Z + 0.3f), new Vector3(0f, 0f, ExitPenthouse_CameraExterior.W), GameplayCamera.FieldOfView);
				ExitEnterPenthouseCam.Position = ExitEnterPenthouseCam.GetOffsetPosition(new Vector3(0.1f, 0f, 0f));
				World.RenderingCamera = ExitEnterPenthouseCam;
				ExitEnterPenthouseCam.IsActive = true;
				ExitEnterPenthouseCam.FarClip = 2000f;
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
				Script.Wait(4000);
				if (ExitEnterPenthouseCam != null)
				{
					Game.Player.Character.IsInvincible = false;
					Game.Player.Character.IsPositionFrozen = false;
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
					ExitEnterPenthouseCam.IsActive = false;
					ExitEnterPenthouseCam.Delete();
				}
				Game.Player.Character.Position = PenthouseExit2;
				Game.Player.Character.Heading = -128f;
			}
		}
	}
}
