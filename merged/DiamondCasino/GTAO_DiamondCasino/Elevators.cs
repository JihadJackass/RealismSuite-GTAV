using System;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using GTAO_DiamondCasino.Penthouse;

namespace GTAO_DiamondCasino
{
	internal class Elevators : Script
	{
		public enum DoorType
		{
			Casino_ElvDoorL,
			Casino_ElvDoorR,
			Garage_ElvDoorL,
			Garage_ElvDoorR,
			Penthouse_ElvDoorL,
			Penthouse_ElvDoorR
		}

		public static Tuple<Vector3, Vector3> Casino_ElevatorDoorL_Data = new Tuple<Vector3, Vector3>(new Vector3(1085.46f, 213.38f, -50.2f), new Vector3(0f, 0f, 134.99f));

		public static Tuple<Vector3, Vector3> Casino_ElevatorDoorR_Data = new Tuple<Vector3, Vector3>(new Vector3(1084.4f, 214.44f, -50.2f), new Vector3(0f, 0f, -45f));

		public static Quaternion Casino_ElevatorOut = new Quaternion(1085.786f, 214.84f, -50.2f, -43f);

		public static Quaternion Casino_ElevatorIn = new Quaternion(1083.931f, 212.88f, -50.2f, -50f);

		public static Prop Casino_ElavatorDoorL;

		public static Prop Casino_ElavatorDoorR;

		public static bool Casino_IsElevatorDoorLOpening;

		public static bool Casino_IsElevatorDoorROpening;

		public static bool Casino_IsElevatorDoorLClosing;

		public static bool Casino_IsElevatorDoorRClosing;

		public static Tuple<Vector3, Vector3> Penthouse_ElevatorDoorL_Data = new Tuple<Vector3, Vector3>(new Vector3(1085.46f, 213.38f, -50.2f), new Vector3(0f, 0f, 134.99f));

		public static Tuple<Vector3, Vector3> Penthouse_ElevatorDoorR_Data = new Tuple<Vector3, Vector3>(new Vector3(1084.4f, 214.44f, -50.2f), new Vector3(0f, 0f, -45f));

		public static Quaternion Penthouse_ElevatorOut = new Quaternion(1085.786f, 214.84f, -50.2f, -43f);

		public static Quaternion Penthouse_ElevatorIn = new Quaternion(1083.931f, 212.88f, -50.2f, -50f);

		public static Prop Penthouse_ElavatorDoorL;

		public static Prop Penthouse_ElavatorDoorR;

		public static bool Penthouse_IsElevatorDoorLOpening;

		public static bool Penthouse_IsElevatorDoorROpening;

		public static bool Penthouse_IsElevatorDoorLClosing;

		public static bool Penthouse_IsElevatorDoorRClosing;

		public static Tuple<Vector3, Vector3> Garage_ElevatorDoorL_Data = new Tuple<Vector3, Vector3>(new Vector3(1296.08f, 219.03f, -50.05f), new Vector3(0f, 0f, 180f));

		public static Tuple<Vector3, Vector3> Garage_ElevatorDoorR_Data = new Tuple<Vector3, Vector3>(new Vector3(1294.58f, 219.03f, -50.05f), new Vector3(0f, 0f, 0f));

		public static Quaternion Garage_ElevatorOut = new Quaternion(1295.26f, 220.38f, -50.05f, 0f);

		public static Quaternion Garage_ElevatorIn = new Quaternion(1295.32f, 217.8f, -50.05f, 0f);

		public static Prop Garage_ElavatorDoorL;

		public static Prop Garage_ElavatorDoorR;

		public static bool Garage_IsElevatorDoorLOpening;

		public static bool Garage_IsElevatorDoorROpening;

		public static bool Garage_IsElevatorDoorLClosing;

		public static bool Garage_IsElevatorDoorRClosing;

		public static int PlayingScene = -1;

		public static int PlayingElevatorCutscene = -1;

		public static bool RunMethodNow()
		{
			return false;
		}

		public Elevators()
		{
			if (_Debug.Elevators != -1)
			{
				base.Tick += OnTick;
			}
		}

		public static void ResetElevator(Tuple<Prop, Prop> Doors)
		{
			if (Doors.Item1 == Casino_ElavatorDoorL && Doors.Item2 == Casino_ElavatorDoorR)
			{
				Casino_IsElevatorDoorLOpening = false;
				Casino_IsElevatorDoorROpening = false;
				Casino_IsElevatorDoorLClosing = false;
				Casino_IsElevatorDoorRClosing = false;
				if (Casino_ElavatorDoorL != null)
				{
					Casino_ElavatorDoorL.Position = Casino_ElevatorDoorL_Data.Item1;
					Casino_ElavatorDoorL.Rotation = Casino_ElevatorDoorL_Data.Item2;
				}
				if (Casino_ElavatorDoorR != null)
				{
					Casino_ElavatorDoorR.Position = Casino_ElevatorDoorR_Data.Item1;
					Casino_ElavatorDoorR.Rotation = Casino_ElevatorDoorR_Data.Item2;
				}
			}
			if (Doors.Item1 == Garage_ElavatorDoorL && Doors.Item2 == Garage_ElavatorDoorR)
			{
				Garage_IsElevatorDoorLOpening = false;
				Garage_IsElevatorDoorROpening = false;
				Garage_IsElevatorDoorLClosing = false;
				Garage_IsElevatorDoorRClosing = false;
				if (Garage_ElavatorDoorL != null)
				{
					Garage_ElavatorDoorL.Position = Garage_ElevatorDoorL_Data.Item1;
					Garage_ElavatorDoorL.Rotation = Garage_ElevatorDoorL_Data.Item2;
				}
				if (Garage_ElavatorDoorR != null)
				{
					Garage_ElavatorDoorR.Position = Garage_ElevatorDoorR_Data.Item1;
					Garage_ElavatorDoorR.Rotation = Garage_ElevatorDoorR_Data.Item2;
				}
			}
		}

		public static void OpenElevator(Tuple<Prop, Prop> Doors)
		{
			ResetElevator(Doors);
			if (Doors.Item1 == Casino_ElavatorDoorL && Doors.Item2 == Casino_ElavatorDoorR)
			{
				Screen.ShowSubtitle("Exit Elevator 2", 1000);
				Casino_IsElevatorDoorLClosing = false;
				Casino_IsElevatorDoorRClosing = false;
				Casino_IsElevatorDoorLOpening = true;
				Casino_IsElevatorDoorROpening = true;
			}
			if (Doors.Item1 == Garage_ElavatorDoorL && Doors.Item2 == Garage_ElavatorDoorR)
			{
				Garage_IsElevatorDoorLClosing = false;
				Garage_IsElevatorDoorRClosing = false;
				Garage_IsElevatorDoorLOpening = true;
				Garage_IsElevatorDoorROpening = true;
			}
		}

		public static void CloseElevator(Tuple<Prop, Prop> Doors)
		{
			ResetElevator(Doors);
			if (Doors.Item1 == Casino_ElavatorDoorL && Doors.Item2 == Casino_ElavatorDoorR)
			{
				Screen.ShowSubtitle("Set Close", 1500);
				Casino_IsElevatorDoorLOpening = false;
				Casino_IsElevatorDoorROpening = false;
				Casino_IsElevatorDoorLClosing = true;
				Casino_IsElevatorDoorRClosing = true;
				if (Casino_ElavatorDoorL != null)
				{
					Casino_ElavatorDoorL.Position = Casino_ElavatorDoorL.GetOffsetPosition(new Vector3(-1f, 0f, 0f));
				}
				if (Casino_ElavatorDoorR != null)
				{
					Casino_ElavatorDoorR.Position = Casino_ElavatorDoorR.GetOffsetPosition(new Vector3(-1f, 0f, 0f));
				}
			}
			if (Doors.Item1 == Garage_ElavatorDoorL && Doors.Item2 == Garage_ElavatorDoorR)
			{
				Garage_IsElevatorDoorLOpening = false;
				Garage_IsElevatorDoorROpening = false;
				Garage_IsElevatorDoorLClosing = true;
				Garage_IsElevatorDoorRClosing = true;
				if (Garage_ElavatorDoorL != null)
				{
					Garage_ElavatorDoorL.Position = Garage_ElavatorDoorL.GetOffsetPosition(new Vector3(-1f, 0f, 0f));
				}
				if (Garage_ElavatorDoorR != null)
				{
					Garage_ElavatorDoorR.Position = Garage_ElavatorDoorR.GetOffsetPosition(new Vector3(-1f, 0f, 0f));
				}
			}
		}

		public unsafe static void PlayEnterElevator(Tuple<DoorType, DoorType> Doors)
		{
			if (Doors.Item1 == DoorType.Casino_ElvDoorL && Doors.Item2 == DoorType.Casino_ElvDoorR)
			{
				if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(Casino_ElevatorOut.X, Casino_ElevatorOut.Y, Casino_ElevatorOut.Z), Class1.GetDistSign.Greater, 30f))
				{
					Game.Player.Character.Position = new Vector3(Casino_ElevatorOut.X, Casino_ElevatorOut.Y, Casino_ElevatorOut.Z);
					Game.Player.Character.Heading = Casino_ElevatorOut.W - 180f;
				}
				if (Casino_ElavatorDoorL == null)
				{
					Casino_ElavatorDoorL = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1085.45f, 213.38f, -50.2f, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_garageliftdoor"), 0, 0, 0);
				}
				if (Casino_ElavatorDoorR == null)
				{
					Casino_ElavatorDoorR = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1084.4f, 214.44f, -50.2f, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_garageliftdoor"), 0, 0, 0);
				}
				if (Casino_ElavatorDoorR == null || Casino_ElavatorDoorL == null)
				{
					Notification.Show("Doors Are Null");
				}
				Script.Wait(200);
				PlayingElevatorCutscene = 1;
				PlayingScene = Game.GameTime + 3000;
				OpenElevator(new Tuple<Prop, Prop>(Casino_ElavatorDoorL, Casino_ElavatorDoorR));
				int taskID = 0;
				Utils.OpenSequenceTask(&taskID);
				Utils.TaskPedSlideToCoord(0, new Vector3(Casino_ElevatorOut.X, Casino_ElevatorOut.Y, Casino_ElevatorOut.Z), Casino_ElevatorOut.W - 180f, 8f);
				Utils.TaskPedSlideToCoord(0, new Vector3(Casino_ElevatorIn.X, Casino_ElevatorIn.Y, Casino_ElevatorIn.Z), Casino_ElevatorIn.W, 8f);
				Function.Call(Hash.TASK_STAND_STILL, 0, 200);
				Utils.CloseSequenceTask(taskID);
				Utils.TaskPerformSequence(Game.Player.Character, taskID);
				Utils.ClearSequenceTask(&taskID);
			}
			if (Doors.Item1 == DoorType.Garage_ElvDoorL && Doors.Item2 == DoorType.Garage_ElvDoorR)
			{
				if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(Garage_ElevatorOut.X, Garage_ElevatorOut.Y, Garage_ElevatorOut.Z), Class1.GetDistSign.Greater, 10f))
				{
					Game.Player.Character.Position = new Vector3(Garage_ElevatorOut.X, Garage_ElevatorOut.Y, Garage_ElevatorOut.Z);
					Game.Player.Character.Heading = Garage_ElevatorOut.W - 180f;
				}
				if (Garage_ElavatorDoorL == null)
				{
					Garage_ElavatorDoorL = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, Garage_ElevatorDoorL_Data.Item1.X, Garage_ElevatorDoorL_Data.Item1.Y, Garage_ElevatorDoorL_Data.Item1.Z, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_garageliftdoor"), 0, 0, 0);
				}
				if (Garage_ElavatorDoorR == null)
				{
					Garage_ElavatorDoorR = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, Garage_ElevatorDoorR_Data.Item1.X, Garage_ElevatorDoorR_Data.Item1.Y, Garage_ElevatorDoorR_Data.Item1.Z, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_garageliftdoor"), 0, 0, 0);
				}
				Script.Wait(200);
				PlayingElevatorCutscene = 4;
				PlayingScene = Game.GameTime + 3000;
				OpenElevator(new Tuple<Prop, Prop>(Garage_ElavatorDoorL, Garage_ElavatorDoorR));
				int taskID2 = 0;
				Utils.OpenSequenceTask(&taskID2);
				Utils.TaskPedSlideToCoord(0, new Vector3(Garage_ElevatorOut.X, Garage_ElevatorOut.Y, Garage_ElevatorOut.Z), Garage_ElevatorOut.W - 180f, 8f);
				Utils.TaskPedSlideToCoord(0, new Vector3(Garage_ElevatorIn.X, Garage_ElevatorIn.Y, Garage_ElevatorIn.Z), Garage_ElevatorIn.W, 8f);
				Function.Call(Hash.TASK_STAND_STILL, 0, 200);
				Utils.CloseSequenceTask(taskID2);
				Utils.TaskPerformSequence(Game.Player.Character, taskID2);
				Utils.ClearSequenceTask(&taskID2);
			}
		}

		public unsafe static void PlayExitElevator(Tuple<DoorType, DoorType> Doors)
		{
			if (Doors.Item1 == DoorType.Casino_ElvDoorL && Doors.Item2 == DoorType.Casino_ElvDoorR)
			{
				Game.Player.Character.Position = new Vector3(Casino_ElevatorIn.X, Casino_ElevatorIn.Y, Casino_ElevatorIn.Z);
				Game.Player.Character.Heading = Casino_ElevatorIn.W;
				Script.Wait(200);
				Function.Call(Hash.REFRESH_INTERIOR, 275201);
				Script.Wait(200);
				if (Casino_ElavatorDoorL == null)
				{
					Casino_ElavatorDoorL = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1085.45f, 213.38f, -50.2f, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_garageliftdoor"), 0, 0, 0);
				}
				if (Casino_ElavatorDoorR == null)
				{
					Casino_ElavatorDoorR = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 1084.4f, 214.44f, -50.2f, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_garageliftdoor"), 0, 0, 0);
				}
				if (Casino_ElavatorDoorR == null || Casino_ElavatorDoorL == null)
				{
					Notification.Show("Doors Are Null");
				}
				Script.Wait(200);
				PlayingElevatorCutscene = 2;
				PlayingScene = Game.GameTime + 3000;
				OpenElevator(new Tuple<Prop, Prop>(Casino_ElavatorDoorL, Casino_ElavatorDoorR));
				int taskID = 0;
				Utils.OpenSequenceTask(&taskID);
				Function.Call(Hash.TASK_STAND_STILL, 0, 200);
				Utils.TaskPedSlideToCoord(0, new Vector3(Casino_ElevatorOut.X, Casino_ElevatorOut.Y, Casino_ElevatorOut.Z), Casino_ElevatorOut.W, 8f);
				Function.Call(Hash.TASK_STAND_STILL, 0, 200);
				Utils.CloseSequenceTask(taskID);
				Utils.TaskPerformSequence(Game.Player.Character, taskID);
				Utils.ClearSequenceTask(&taskID);
			}
			if (Doors.Item1 == DoorType.Garage_ElvDoorL && Doors.Item2 == DoorType.Garage_ElvDoorR)
			{
				Game.Player.Character.Position = new Vector3(Garage_ElevatorIn.X, Garage_ElevatorIn.Y, Garage_ElevatorIn.Z);
				Game.Player.Character.Heading = Garage_ElevatorIn.W;
				if (Garage_ElavatorDoorL == null)
				{
					Garage_ElavatorDoorL = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, Garage_ElevatorDoorL_Data.Item1.X, Garage_ElevatorDoorL_Data.Item1.Y, Garage_ElevatorDoorL_Data.Item1.Z, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_garageliftdoor"), 0, 0, 0);
				}
				if (Garage_ElavatorDoorR == null)
				{
					Garage_ElavatorDoorR = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, Garage_ElevatorDoorR_Data.Item1.X, Garage_ElevatorDoorR_Data.Item1.Y, Garage_ElevatorDoorR_Data.Item1.Z, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_garageliftdoor"), 0, 0, 0);
				}
				Script.Wait(200);
				PlayingElevatorCutscene = 3;
				PlayingScene = Game.GameTime + 3000;
				OpenElevator(new Tuple<Prop, Prop>(Garage_ElavatorDoorL, Garage_ElavatorDoorR));
				int taskID2 = 0;
				Utils.OpenSequenceTask(&taskID2);
				Function.Call(Hash.TASK_STAND_STILL, 0, 1200);
				Utils.TaskPedSlideToCoord(0, new Vector3(Garage_ElevatorOut.X, Garage_ElevatorOut.Y, Garage_ElevatorOut.Z), Garage_ElevatorOut.W, 8f);
				Function.Call(Hash.TASK_STAND_STILL, 0, 200);
				Utils.CloseSequenceTask(taskID2);
				Utils.TaskPerformSequence(Game.Player.Character, taskID2);
				Utils.ClearSequenceTask(&taskID2);
			}
		}

		public static void OnTick(object sender, EventArgs e)
		{
			_Debug.Elevators = 0;
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(Casino_ElevatorOut.X, Casino_ElevatorOut.Y, Casino_ElevatorOut.Z), Class1.GetDistSign.Greater, 10f) && Class1.GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(Garage_ElevatorOut.X, Garage_ElevatorOut.Y, Garage_ElevatorOut.Z), Class1.GetDistSign.Greater, 10f))
			{
				_Debug.Elevators = 0;
				return;
			}
			_Debug.Elevators = 1;
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(Casino_ElevatorOut.X, Casino_ElevatorOut.Y, Casino_ElevatorOut.Z), Class1.GetDistSign.Less, 2.5f))
			{
				Game.DisableControlThisFrame(Control.Phone);
				if (Class1.Purchased_Penthouse == 1)
				{
					Class1.DisplayHelpTextThisFrame("Press ~INPUT_CELLPHONE_UP~ To take elevator up to penthouse.\nPress ~INPUT_CELLPHONE_DOWN~ To take elevator down to garage.");
					if (Game.IsControlJustPressed(Control.PhoneUp))
					{
						Casino_IsElevatorDoorLClosing = false;
						Casino_IsElevatorDoorRClosing = false;
						Casino_ElavatorDoorL = null;
						Casino_ElavatorDoorR = null;
						Script.Wait(1500);
						PlayEnterElevator(new Tuple<DoorType, DoorType>(DoorType.Casino_ElvDoorL, DoorType.Casino_ElvDoorR));
						Script.Wait(2500);
						Casino_IsElevatorDoorLOpening = false;
						Casino_IsElevatorDoorROpening = false;
						Casino_IsElevatorDoorLClosing = true;
						Casino_IsElevatorDoorRClosing = true;
						Garage_IsElevatorDoorLClosing = false;
						Garage_IsElevatorDoorRClosing = false;
						Script.Wait(2000);
						CasinoPeds.DeleteGamingAreaPeds();
						CasinoPeds.DeleteHallAPeds();
						CasinoPeds.DeleteHallBPeds();
						CasinoPeds.DeleteMainHallPeds();
						Zones.CurrentZone = Zones.ZoneIn.None;
						Screen.FadeOut(500);
						Class1.IsinInterior = false;
						PenthouseInterior.RefreshPenthouse();
						Game.Player.Character.Position = EnterExitPenthouseFromTerrace.PenthouseMainEnter;
						Game.Player.Character.Heading = 66.19f;
						PenthouseStuff.IsInPenhouse = true;
						Screen.FadeIn(500);
					}
					if (Game.IsControlJustPressed(Control.PhoneDown))
					{
						Casino_IsElevatorDoorLClosing = false;
						Casino_IsElevatorDoorRClosing = false;
						Casino_ElavatorDoorL = null;
						Casino_ElavatorDoorR = null;
						Script.Wait(1500);
						PlayEnterElevator(new Tuple<DoorType, DoorType>(DoorType.Casino_ElvDoorL, DoorType.Casino_ElvDoorR));
						Script.Wait(2500);
						Casino_IsElevatorDoorLOpening = false;
						Casino_IsElevatorDoorROpening = false;
						Casino_IsElevatorDoorLClosing = true;
						Casino_IsElevatorDoorRClosing = true;
						Garage_IsElevatorDoorLClosing = false;
						Garage_IsElevatorDoorRClosing = false;
						Script.Wait(2000);
						CasinoPeds.DeleteGamingAreaPeds();
						CasinoPeds.DeleteHallAPeds();
						CasinoPeds.DeleteHallBPeds();
						CasinoPeds.DeleteMainHallPeds();
						Zones.CurrentZone = Zones.ZoneIn.None;
						Screen.FadeOut(500);
						Script.Wait(1000);
						Garage_ElavatorDoorL = null;
						Garage_ElavatorDoorR = null;
						Screen.FadeIn(500);
						PlayExitElevator(new Tuple<DoorType, DoorType>(DoorType.Garage_ElvDoorL, DoorType.Garage_ElvDoorR));
						Class1.IsinInterior = false;
						PenthouseGarage.LoadGarage();
						PenthouseGarage.InGarage = true;
					}
				}
			}
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(Garage_ElevatorOut.X, Garage_ElevatorOut.Y, Garage_ElevatorOut.Z), Class1.GetDistSign.Less, 2.5f))
			{
				Game.DisableControlThisFrame(Control.Phone);
				Class1.DisplayHelpTextThisFrame("Press ~INPUT_CELLPHONE_UP~ To take elevator up to casino.");
				if (Game.IsControlJustPressed(Control.PhoneUp))
				{
					Function.Call(Hash.REFRESH_INTERIOR, 275457);
					Garage_ElavatorDoorL = null;
					Garage_ElavatorDoorR = null;
					PlayEnterElevator(new Tuple<DoorType, DoorType>(DoorType.Garage_ElvDoorL, DoorType.Garage_ElvDoorR));
					Script.Wait(3500);
					Casino_IsElevatorDoorLClosing = false;
					Casino_IsElevatorDoorRClosing = false;
					Garage_IsElevatorDoorLClosing = true;
					Garage_IsElevatorDoorRClosing = true;
					Zones.CurrentZone = Zones.ZoneIn.MainHall;
					Script.Wait(2000);
					Screen.FadeOut(500);
					Script.Wait(1000);
					Casino_ElavatorDoorL = null;
					Casino_ElavatorDoorR = null;
					Screen.FadeIn(500);
					PlayExitElevator(new Tuple<DoorType, DoorType>(DoorType.Casino_ElvDoorL, DoorType.Casino_ElvDoorR));
					Class1.IsinInterior = true;
					PenthouseGarage.DeleteVehicles();
					PenthouseGarage.InGarage = false;
					CasinoPeds.DeleteMainHallPeds();
					CasinoPeds.DeleteGamingAreaPeds();
					CasinoPeds.SpawnMainHallPeds();
				}
			}
			if (PlayingElevatorCutscene != -1 && Game.GameTime > PlayingScene)
			{
				PlayingScene = -1;
				if (PlayingElevatorCutscene == 1 || PlayingElevatorCutscene == 2)
				{
					CloseElevator(new Tuple<Prop, Prop>(Casino_ElavatorDoorL, Casino_ElavatorDoorR));
				}
				if (PlayingElevatorCutscene == 3 || PlayingElevatorCutscene == 4)
				{
					CloseElevator(new Tuple<Prop, Prop>(Garage_ElavatorDoorL, Garage_ElavatorDoorR));
				}
				PlayingElevatorCutscene = -1;
			}
		}
	}
}
