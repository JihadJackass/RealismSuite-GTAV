using GTA.Math;

namespace SPAII.INM
{
	public class EntityVector
	{
		public string Model;

		public Quaternion Position;

		public EntityVector(string model, Quaternion position)
		{
			Model = model;
			Position = position;
		}
	}
}
