using System;
using GTA;
using GTA.Math;
using GTA.UI;

namespace GTAO_DiamondCasino
{
	internal class Elevators_Doors : Script
	{
		public Elevators_Doors()
		{
			if (_Debug.ElevatorDoors != -1)
			{
				base.Tick += OnTick;
			}
		}

		public static void OnTick(object sender, EventArgs e)
		{
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(Elevators.Casino_ElevatorOut.X, Elevators.Casino_ElevatorOut.Y, Elevators.Casino_ElevatorOut.Z), Class1.GetDistSign.Greater, 10f) && Class1.GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(Elevators.Garage_ElevatorOut.X, Elevators.Garage_ElevatorOut.Y, Elevators.Garage_ElevatorOut.Z), Class1.GetDistSign.Greater, 10f))
			{
				_Debug.ElevatorDoors = 0;
				return;
			}
			_Debug.ElevatorDoors = 1;
			if (Elevators.Casino_IsElevatorDoorLOpening && Elevators.Casino_ElavatorDoorL != null)
			{
				Vector3 offsetPosition = Elevators.Casino_ElavatorDoorL.GetOffsetPosition(new Vector3(-1f, 0f, 0f));
				if (Class1.GetDistance_VDIST2(Elevators.Casino_ElavatorDoorL.Position, offsetPosition, Class1.GetDistSign.Greater, 0.5f))
				{
					Elevators.Casino_ElavatorDoorL.Position = Elevators.Casino_ElavatorDoorL.GetOffsetPosition(new Vector3(-0.01f, 0f, 0f));
				}
				else
				{
					Notification.Show("Set Dool L Close");
					Elevators.Casino_IsElevatorDoorLOpening = false;
				}
			}
			if (Elevators.Casino_IsElevatorDoorROpening && Elevators.Casino_ElavatorDoorR != null)
			{
				Vector3 offsetPosition2 = Elevators.Casino_ElavatorDoorR.GetOffsetPosition(new Vector3(-1f, 0f, 0f));
				if (Class1.GetDistance_VDIST2(Elevators.Casino_ElavatorDoorR.Position, offsetPosition2, Class1.GetDistSign.Greater, 0.5f))
				{
					Elevators.Casino_ElavatorDoorR.Position = Elevators.Casino_ElavatorDoorR.GetOffsetPosition(new Vector3(-0.01f, 0f, 0f));
				}
				else
				{
					Notification.Show("Set Dool R Close");
					Elevators.Casino_IsElevatorDoorROpening = false;
				}
			}
			if (Elevators.Casino_IsElevatorDoorLClosing && Elevators.Casino_ElavatorDoorL != null)
			{
				Vector3 item = Elevators.Casino_ElevatorDoorL_Data.Item1;
				if (Class1.GetDistance_VDIST2(Elevators.Casino_ElavatorDoorL.Position, item, Class1.GetDistSign.Greater, 0.005f))
				{
					Elevators.Casino_ElavatorDoorL.Position = Elevators.Casino_ElavatorDoorL.GetOffsetPosition(new Vector3(0.01f, 0f, 0f));
				}
				else
				{
					Elevators.Casino_IsElevatorDoorLClosing = false;
				}
			}
			if (Elevators.Casino_IsElevatorDoorRClosing && Elevators.Casino_ElavatorDoorR != null)
			{
				Vector3 item2 = Elevators.Casino_ElevatorDoorR_Data.Item1;
				if (Class1.GetDistance_VDIST2(Elevators.Casino_ElavatorDoorR.Position, item2, Class1.GetDistSign.Greater, 0.005f))
				{
					Elevators.Casino_ElavatorDoorR.Position = Elevators.Casino_ElavatorDoorR.GetOffsetPosition(new Vector3(0.01f, 0f, 0f));
				}
				else
				{
					Elevators.Casino_IsElevatorDoorRClosing = false;
				}
			}
			if (Elevators.Garage_IsElevatorDoorLOpening && Elevators.Garage_ElavatorDoorL != null)
			{
				Vector3 offsetPosition3 = Elevators.Garage_ElavatorDoorL.GetOffsetPosition(new Vector3(-1f, 0f, 0f));
				if (Class1.GetDistance_VDIST2(Elevators.Garage_ElavatorDoorL.Position, offsetPosition3, Class1.GetDistSign.Greater, 0.5f))
				{
					Elevators.Garage_ElavatorDoorL.Position = Elevators.Garage_ElavatorDoorL.GetOffsetPosition(new Vector3(-0.01f, 0f, 0f));
				}
				else
				{
					Elevators.Garage_IsElevatorDoorLOpening = false;
				}
			}
			if (Elevators.Garage_IsElevatorDoorROpening && Elevators.Garage_ElavatorDoorR != null)
			{
				Vector3 offsetPosition4 = Elevators.Garage_ElavatorDoorR.GetOffsetPosition(new Vector3(-1f, 0f, 0f));
				if (Class1.GetDistance_VDIST2(Elevators.Garage_ElavatorDoorR.Position, offsetPosition4, Class1.GetDistSign.Greater, 0.5f))
				{
					Elevators.Garage_ElavatorDoorR.Position = Elevators.Garage_ElavatorDoorR.GetOffsetPosition(new Vector3(-0.01f, 0f, 0f));
				}
				else
				{
					Elevators.Garage_IsElevatorDoorROpening = false;
				}
			}
			if (Elevators.Garage_IsElevatorDoorLClosing && Elevators.Garage_ElavatorDoorL != null)
			{
				Vector3 item3 = Elevators.Garage_ElevatorDoorL_Data.Item1;
				if (Class1.GetDistance_VDIST2(Elevators.Garage_ElavatorDoorL.Position, item3, Class1.GetDistSign.Greater, 0.005f))
				{
					Elevators.Garage_ElavatorDoorL.Position = Elevators.Garage_ElavatorDoorL.GetOffsetPosition(new Vector3(0.01f, 0f, 0f));
				}
				else
				{
					Elevators.Garage_IsElevatorDoorLClosing = false;
				}
			}
			if (Elevators.Garage_IsElevatorDoorRClosing && Elevators.Garage_ElavatorDoorR != null)
			{
				Vector3 item4 = Elevators.Garage_ElevatorDoorR_Data.Item1;
				if (Class1.GetDistance_VDIST2(Elevators.Garage_ElavatorDoorR.Position, item4, Class1.GetDistSign.Greater, 0.005f))
				{
					Elevators.Garage_ElavatorDoorR.Position = Elevators.Garage_ElavatorDoorR.GetOffsetPosition(new Vector3(0.01f, 0f, 0f));
				}
				else
				{
					Elevators.Garage_IsElevatorDoorRClosing = false;
				}
			}
		}
	}
}
