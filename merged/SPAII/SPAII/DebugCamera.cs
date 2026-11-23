using System;
using GTA;
using GTA.Math;

namespace SPAII
{
	public class DebugCamera : Script
	{
		private static Camera freeCam = null;

		public static Camera Camera => freeCam;

		public static void ForgetForStoryLoad()
		{
			freeCam = null;
		}

		public DebugCamera()
		{
			base.Tick += DebugCamera_Tick;
		}

		private void DebugCamera_Tick(object sender, EventArgs e)
		{
			if ((object)freeCam != null && !StoryLoadGuard.WorldLoading() && World.RenderingCamera == freeCam)
			{
				Game.EnableControlThisFrame(Control.LookLeftRight);
				Game.EnableControlThisFrame(Control.LookUpDown);
				Game.EnableControlThisFrame(Control.CursorX);
				Game.EnableControlThisFrame(Control.CursorY);
				Game.EnableControlThisFrame(Control.FrontendPauseAlternate);
				Game.EnableControlThisFrame(Control.WeaponWheelPrev);
				Game.EnableControlThisFrame(Control.WeaponWheelNext);
				float controlNormal = V2Compat.GetControlNormal(0, Control.LookLeftRight);
				float controlNormal2 = V2Compat.GetControlNormal(0, Control.LookUpDown);
				freeCam.Rotation += new Vector3(controlNormal2 * -10f, 0f, controlNormal * -10f);
				if (freeCam.Rotation.X < -90f)
				{
					freeCam.Rotation += new Vector3(1f, 0f, 0f);
				}
				if (freeCam.Rotation.X > 90f)
				{
					freeCam.Rotation += new Vector3(-1f, 0f, 0f);
				}
				float num = 0.25f;
				if (Game.IsControlPressed(Control.Sprint))
				{
					num = 1f;
				}
				if (Game.IsControlPressed(Control.Duck))
				{
					num = 0.1f;
				}
				Vector3 position = freeCam.Position;
				Vector3 vector = DebugHelper.RotationToDirection(freeCam.Rotation);
				Vector3 position2 = freeCam.Position;
				Vector3 rot = freeCam.Rotation + new Vector3(0f, 0f, -10f);
				Vector3 vector2 = DebugHelper.RotationToDirection(freeCam.Rotation + new Vector3(0f, 0f, 10f)) - DebugHelper.RotationToDirection(rot);
				if (Game.IsControlPressed(Control.MoveUpOnly))
				{
					position += vector * num;
				}
				if (Game.IsControlPressed(Control.MoveDownOnly))
				{
					position -= vector * num;
				}
				if (Game.IsControlPressed(Control.MoveLeftOnly))
				{
					position += vector2 * (num * 2f);
				}
				if (Game.IsControlPressed(Control.MoveRightOnly))
				{
					position -= vector2 * (num * 2f);
				}
				if (Game.IsControlPressed(Control.PhoneLeft))
				{
					freeCam.Rotation -= new Vector3(0f, 0.5f, 0f);
				}
				if (Game.IsControlPressed(Control.PhoneRight))
				{
					freeCam.Rotation += new Vector3(0f, 0.5f, 0f);
				}
				if (Game.IsControlPressed(Control.PhoneUp))
				{
					freeCam.FieldOfView -= 0.5f;
				}
				if (Game.IsControlPressed(Control.PhoneDown))
				{
					freeCam.FieldOfView += 0.5f;
				}
				if (Game.IsControlPressed(Control.Reload))
				{
					freeCam.FieldOfView = 50f;
					freeCam.Rotation = GameplayCamera.Rotation;
					freeCam.Position = GameplayCamera.Position;
				}
				freeCam.Position = position;
			}
		}

		public static bool IsEnabled()
		{
			if (!(freeCam == null))
			{
				return freeCam.IsActive;
			}
			return false;
		}

		public static void Toggle()
		{
			if (IsEnabled())
			{
				Disable();
			}
			else
			{
				Enable();
			}
		}

		public static void Enable()
		{
			freeCam = Stability.CreateCam(GameplayCamera.Position, GameplayCamera.Rotation, 50f);
			World.RenderingCamera = freeCam;
			Game.Player.Character.Task.StartScenario("WORLD_HUMAN_YOGA", Game.Player.Character.Position, Game.Player.Character.Heading);
			Game.Player.Character.Task.LookAt(freeCam.Position);
			Game.Player.Character.AlwaysKeepTask = true;
		}

		public static void Disable()
		{
			freeCam.Delete();
			World.RenderingCamera = null;
			Game.Player.Character.Task.ClearAllImmediately();
		}
	}
}
