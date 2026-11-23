using GTA.Math;

namespace SPAII.INM
{
	public class Vector5
	{
		public Vector3 Vector3;

		public Vector2 Vector2;

		public Vector5(Vector3 v3, Vector2 v2)
		{
			Vector3 = v3;
			Vector2 = v2;
		}

		public static Vector5 Zero()
		{
			return new Vector5(Vector3.Zero, Vector2.Zero);
		}
	}
}
