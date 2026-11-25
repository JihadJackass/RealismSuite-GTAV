using GTA;

namespace GTAO_DiamondCasino
{
	public class RaceArcade : Script
	{
		public Prop Base { get; set; }

		public Prop Seat1 { get; set; }

		public Prop Seat2 { get; set; }

		public string Decleration { get; set; }

		public RaceArcade()
		{
		}

		public RaceArcade(Prop Abase, Prop Aseat1, Prop Aseat2, string decleration)
		{
			Base = Abase;
			Seat1 = Aseat1;
			Seat2 = Aseat2;
			Decleration = decleration;
		}
	}
}
