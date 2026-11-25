using GTA;
using GTA.Math;

namespace GTAO_DiamondCasino
{
	public class SlotsClass : Script
	{
		public Vector3 Loc { get; set; }

		public float SeatHeading { get; set; }

		public float Heading { get; set; }

		public string Machine { get; set; }

		public Prop MachineProp { get; set; }

		public Ped SlotPed { get; set; }

		public bool In_use { get; set; }

		public int SlotSyncedScene { get; set; }

		public SlotsClass()
		{
		}

		public SlotsClass(Vector3 L, float SH, float H, string m)
		{
			Loc = L;
			SeatHeading = SH;
			Heading = H;
			Machine = m;
		}

		public SlotsClass(Vector3 L, float H, string m)
		{
			Loc = L;
			Heading = H;
			Machine = m;
		}
	}
}
