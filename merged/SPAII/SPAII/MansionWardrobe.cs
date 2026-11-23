using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using INMNativeUI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class MansionWardrobe
	{
		public static bool TriggerNear(Vector3 position, Vector3 wardrobe)
		{
			if (!PartyPlacement.FinitePoint(position) || !PartyPlacement.FinitePoint(wardrobe) || wardrobe == Vector3.Zero)
			{
				return false;
			}
			Vector3 vector = position - wardrobe;
			return vector.X * vector.X + vector.Y * vector.Y <= 6.25f && Math.Abs(vector.Z) <= 1.5f;
		}

		public static bool SessionInHome(Mansion m)
		{
			if (m != null && m.Enabled && m.IplsLoaded && m.IsOwnedByPlayer() && (object)Helper.PP != null && Helper.PP.Exists() && !Helper.PP.IsDead && !Helper.PP.IsInVehicle() && TriggerNear(HomeActionPose.Position(Helper.PP), Helper.ToVector3(m.WardrobePos)))
			{
				return !DestinationStreaming.IsPlayerSafetyHeld(Helper.PP);
			}
			return false;
		}

		public static bool IsNear(Mansion m, Vector3 position)
		{
			if (m == null || !m.IplsLoaded || !TriggerNear(position, Helper.ToVector3(m.WardrobePos)))
			{
				return false;
			}
			bool hit = default(bool);
			Vector3 point = default(Vector3);
			Vector3 normal = default(Vector3);
			if (PartyPlacement.PhysicalRay(position, Helper.ToVector3(m.WardrobePos), 81, ref hit, ref point, ref normal, Helper.PP) != 2)
			{
				return false;
			}
			return !hit || point.DistanceTo(Helper.ToVector3(m.WardrobePos)) <= 0.6f;
		}

		public static bool Tick(Mansion m)
		{
			if (!SessionInHome(m) || Helper.MenuPool.IsAnyMenuOpen() || MansionBarber.IsBusy || PartyService.IsPlayerActionActive || MansionFeatures.IsWorkoutActive || PartyPlayerActions.IsKissing || !IsNear(m, HomeActionPose.Position(Helper.PP)))
			{
				return false;
			}
			bool contextPressed = Game.IsEnabledControlJustPressed(Control.Context);
			if (!Helper.ShowOwnedHomeHelp("Press ~INPUT_CONTEXT~ to change clothes at the wardrobe.", "WARD_TRIG", true))
			{
				return false;
			}
			Game.DisableControlThisFrame(Control.Context);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 51, true);
			if (ApartmentHomeServices.BedInputRequested(contextPressed, Game.IsControlJustPressed(Control.Context), Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 2, 51)))
			{
				Wardrobe.OpenMansion(m);
			}
			return true;
		}

		public static Vector3[] StandingCandidates(Quaternion point)
		{
			List<Vector3> list = new List<Vector3>();
			if (!PartyPlacement.FinitePoint(Helper.ToVector3(point)) || float.IsNaN(point.W) || float.IsInfinity(point.W))
			{
				return list.ToArray();
			}
			Vector3 vector = Helper.ToVector3(point) - new Vector3(0f, 0f, 1f);
			list.Add(vector);
			float[] array = new float[4] { 0.8f, 1.4f, 2f, 2.6f };
			foreach (float num in array)
			{
				int num2 = 0;
				do
				{
					double num3 = (double)num2 * Math.PI / 4.0;
					list.Add(Helper.ToVector3(MansionAutoLayout.TransformPoint(vector, point.W, (float)Math.Cos(num3) * num, (float)Math.Sin(num3) * num, 0f)));
					num2 = checked(num2 + 1);
				}
				while (num2 <= 7);
			}
			return list.ToArray();
		}

		public static Vector3 CameraPoint(Ped actor)
		{
			Vector3 vector = HomeActionPose.Position(actor);
			float[] array = new float[5] { 0f, 35f, -35f, 70f, -70f };
			bool hit = default(bool);
			Vector3 point = default(Vector3);
			Vector3 normal = default(Vector3);
			foreach (float num in array)
			{
				double num2 = (double)(HomeActionPose.Heading(actor) + num) * Math.PI / 180.0;
				Vector3 vector2 = vector + new Vector3((0f - (float)Math.Sin(num2)) * 2.5f, (float)Math.Cos(num2) * 2.5f, 0.15f);
				if (PartyPlacement.PhysicalRay(vector, vector2, 81, ref hit, ref point, ref normal, actor) == 2 && !hit)
				{
					return vector2;
				}
			}
			throw new InvalidOperationException("No clear local wardrobe camera position is available.");
		}

		private static bool ForeignCameraActive()
		{
			Camera renderingCamera = World.RenderingCamera;
			if ((object)renderingCamera != null && renderingCamera.Handle != 0)
			{
				return renderingCamera.Exists();
			}
			return false;
		}

		public static void Open(Mansion m, UIMenu parent)
		{
			if (m == null || !m.IsOwnedByPlayer() || (object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsInVehicle() || ForeignCameraActive())
			{
				return;
			}
			if (!SessionInHome(m))
			{
				Notification.Show("Go to the mansion wardrobe to change clothes.");
				return;
			}
			if (parent != null)
			{
				parent.Visible = false;
			}
			if (!Wardrobe.OpenMansion(m) && parent != null)
			{
				parent.Visible = true;
			}
		}
	}
}
