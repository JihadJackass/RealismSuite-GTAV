using GTA;

namespace GTAO_DiamondCasino
{
	public class PlayerChips : Script
	{
		public Prop Chips1 { get; set; }

		public Prop Chips2 { get; set; }

		public Prop Chips3 { get; set; }

		public Prop Chips4 { get; set; }

		public PlayerChips()
		{
		}

		public PlayerChips(Prop C1, Prop C2, Prop C3, Prop C4)
		{
			Chips1 = C1;
			Chips2 = C2;
			Chips3 = C3;
			Chips4 = C4;
		}
	}
}
