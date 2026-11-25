using GTA;

namespace GTAO_DiamondCasino
{
	public class PlayerCards : Script
	{
		public Prop Card1 { get; set; }

		public Prop Card2 { get; set; }

		public Prop Card3 { get; set; }

		public string Card1ID { get; set; }

		public string Card2ID { get; set; }

		public string Card3ID { get; set; }

		public PlayerCards()
		{
		}

		public PlayerCards(Prop C1, Prop C2, Prop C3, string _1ID, string _2ID, string _3ID)
		{
			Card1 = C1;
			Card2 = C2;
			Card3 = C3;
			Card1ID = _1ID;
			Card2ID = _2ID;
			Card3ID = _3ID;
		}
	}
}
