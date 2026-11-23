using GTA;
using GTA.Math;

namespace SPAII
{
	public class GateDef
	{
		public int ModelHash;

		public Vector3 Position;

		public float Heading;

		public Prop Replacement;

		public float Angle;

		public int OpenSign;

		public bool Hidden;

		public GateDef()
		{
			Angle = 0f;
			OpenSign = 1;
			Hidden = false;
		}

		public GateDef(int model, float x, float y, float z, float h)
		{
			Angle = 0f;
			OpenSign = 1;
			Hidden = false;
			ModelHash = model;
			Position = new Vector3(x, y, z);
			Heading = h;
		}
	}
}
