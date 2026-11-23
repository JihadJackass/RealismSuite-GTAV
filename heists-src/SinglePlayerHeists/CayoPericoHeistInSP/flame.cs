using GTA;

namespace CayoPericoHeistInSP
{
	public class flame : Script
	{
		public int ID { get; set; }

		public int LifeTime { get; set; }

		public int Fireid { get; set; }

		public int X1 { get; set; }

		public int X2 { get; set; }

		public int X3 { get; set; }

		public int X4 { get; set; }

		public int X5 { get; set; }

		public int X6 { get; set; }

		public int X7 { get; set; }

		public int Type { get; set; }

		public flame()
		{
		}

		public flame(int x1, int timer)
		{
			Type = 2;
			LifeTime = timer;
			X1 = x1;
		}

		public flame(int type, int id, int timer)
		{
			Type = type;
			ID = id;
			LifeTime = timer;
		}
	}
}
