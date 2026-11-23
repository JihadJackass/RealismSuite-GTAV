using GTA.Math;

namespace SPAII
{
	public class CashPoint
	{
		public Vector3 Pos;

		public int ModelHash;

		public string Taken;

		public bool Hidden;

		public CashPoint()
		{
			ModelHash = 0;
			Taken = "";
			Hidden = false;
		}
	}
}
