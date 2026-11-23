using GTA.Math;

namespace BusinessV.Models
{
	internal sealed class VanillaPropertyDefinition
	{
		public int VanillaIndex { get; }

		public string BusinessId { get; }

		public string Name { get; }

		public string BoughtStat { get; }

		public Vector3 Position { get; }

		public VanillaPropertyDefinition(int vanillaIndex, string businessId, string name, string boughtStat, Vector3 position)
		{
			VanillaIndex = vanillaIndex;
			BusinessId = businessId;
			Name = name;
			BoughtStat = boughtStat;
			Position = position;
		}
	}
}
