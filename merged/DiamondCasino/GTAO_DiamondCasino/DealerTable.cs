using GTA;
using GTA.Math;

namespace GTAO_DiamondCasino
{
	public class DealerTable : Script
	{
		public Vector3 TableOriginPosition { get; set; }

		public float TableOriginHeading { get; set; }

		public bool CreatedDealer { get; set; }

		public Vector3 DealerOriginPosition { get; set; }

		public float DealerOriginHeading { get; set; }

		public Prop Card1 { get; set; }

		public Prop Card2 { get; set; }

		public Prop Card3 { get; set; }

		public Prop Card4 { get; set; }

		public Prop Card5 { get; set; }

		public Prop Card6 { get; set; }

		public string Card1Id { get; set; }

		public string Card2Id { get; set; }

		public string Card3Id { get; set; }

		public string Card4Id { get; set; }

		public string Card5Id { get; set; }

		public string Card6Id { get; set; }

		public int Card1num { get; set; }

		public int Card2num { get; set; }

		public int Card3num { get; set; }

		public int Card4num { get; set; }

		public int Card5num { get; set; }

		public int Card6num { get; set; }

		public Prop Table { get; set; }

		public Ped Dealer { get; set; }

		public TableType TableType { get; set; }

		public DealerTable()
		{
		}

		public DealerTable(Prop T, Ped D, TableType tabletype)
		{
			TableType = tabletype;
			Table = T;
			Dealer = D;
			TableOriginPosition = T.Position;
			TableOriginHeading = T.Heading;
		}

		public DealerTable(Prop T, TableType tabletype)
		{
			TableType = tabletype;
			Table = T;
			TableOriginPosition = T.Position;
			TableOriginHeading = T.Heading;
		}
	}
}
