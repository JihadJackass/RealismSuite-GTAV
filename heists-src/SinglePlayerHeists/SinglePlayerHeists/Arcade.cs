using GTA;
using GTA.Math;

namespace SinglePlayerHeists
{
	public class Arcade : Script
	{
		public new string Name { get; set; }

		public float Price { get; set; }

		public Vector3 Location { get; set; }

		public Arcade()
		{
		}

		public Arcade(string n, float p, Vector3 l)
		{
			Name = n;
			Price = p;
			Location = l;
		}
	}
}
