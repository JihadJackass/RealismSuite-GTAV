using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	internal sealed class NightclubDj
	{
		public readonly string DisplayName;

		public readonly string ModelName;

		public readonly string EntitySet;

		public readonly string Station;

		public readonly int LightIndex;

		public NightclubDj(string displayName, string modelName, int referenceIndex)
		{
			DisplayName = displayName;
			ModelName = modelName;
			EntitySet = "Int01_ba_dj0" + Conversions.ToString(referenceIndex);
			Station = "DLC_BATTLE_MIX" + Conversions.ToString(referenceIndex) + "_CLUB_PRIV";
			LightIndex = checked(referenceIndex - 1);
		}
	}
}
