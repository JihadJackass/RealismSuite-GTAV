using GTA;
using GTA.Math;

namespace SPAII.INM
{
	public class PropMemory
	{
		public Vector3 Position;

		public Vector3 Rotation;

		public PropMemory(Prop prop)
		{
			Position = prop.Position;
			Rotation = prop.Rotation;
		}

		public PropMemory(Vector3 pos, Vector3 rot)
		{
			Position = pos;
			Rotation = rot;
		}
	}
}
