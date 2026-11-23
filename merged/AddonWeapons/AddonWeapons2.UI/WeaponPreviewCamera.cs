using GTA;
using GTA.Math;

namespace AddonWeapons2.UI
{
	public static class WeaponPreviewCamera
	{
		private const float CameraDistance = 1.7f;

		private const float CameraHeightOffset = 0.15f;

		private const float CameraFov = 40f;

		private static Camera _camera;

		private static bool _playerHidden;

		public static bool IsActive
		{
			get
			{
				if (_camera != null)
				{
					return _camera.Exists();
				}
				return false;
			}
		}

		public static void EnsureActive(Vector3 target)
		{
			Vector3 vector = Game.Player.Character.Position - target;
			vector.Z = 0f;
			if (vector.Length() < 0.01f)
			{
				vector = new Vector3(0f, 1f, 0f);
			}
			vector.Normalize();
			Vector3 position = target + vector * 1.7f + new Vector3(0f, 0f, 0.15f);
			if (!IsActive)
			{
				_camera = World.CreateCamera(position, Vector3.Zero, 40f);
				World.RenderingCamera = _camera;
			}
			else
			{
				_camera.Position = position;
			}
			_camera.PointAt(target);
			if (!_playerHidden)
			{
				Game.Player.Character.IsVisible = false;
				_playerHidden = true;
			}
		}

		public static void Reset()
		{
			if (_playerHidden)
			{
				Game.Player.Character.IsVisible = true;
				_playerHidden = false;
			}
			if (!(_camera == null))
			{
				World.RenderingCamera = null;
				if (_camera.Exists())
				{
					_camera.Delete();
				}
				_camera = null;
			}
		}
	}
}
