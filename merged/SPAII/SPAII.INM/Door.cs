using GTA.Math;
using GTA.Native;

namespace SPAII.INM
{
	public class Door
	{
		public int ModelHash;

		public Vector3 Position;

		public Door(int hash, Vector3 position)
		{
			ModelHash = hash;
			Position = position;
		}

		public void LockDoor()
		{
			Function.Call(V2Compat.DOOR_CONTROL, ModelHash, Position.X, Position.Y, Position.Z, true, 0.0, 50.0, 0);
		}

		public void UnlockDoor()
		{
			Function.Call(V2Compat.DOOR_CONTROL, ModelHash, Position.X, Position.Y, Position.Z, false, 0.0, 50.0, 0);
		}
	}
}
