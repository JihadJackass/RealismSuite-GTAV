using GTA.Math;

namespace SPAII.INM
{
	public class CameraPRH
	{
		public Vector3 Position;

		public Vector3 Rotation;

		public float FOV;

		public CameraPRH(Vector3 pos, Vector3 rot, float fov)
		{
			Position = pos;
			Rotation = rot;
			FOV = fov;
		}

		public CameraPRH Zero()
		{
			return new CameraPRH(Vector3.Zero, Vector3.Zero, 0f);
		}
	}
}
