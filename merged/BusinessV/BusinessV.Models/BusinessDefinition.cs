using System.Runtime.CompilerServices;

namespace BusinessV.Models
{
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BusinessDefinition
	{
		public string Id { get; set; } = string.Empty;


		public string Name { get; set; } = string.Empty;


		public string Category { get; set; } = string.Empty;


		public WorldPosition Position { get; set; } = new WorldPosition();


		public WorldPosition PurchaseInteractionPoint { get; set; } = new WorldPosition();


		[Nullable(2)]
		[field: Nullable(2)]
		public WorldPosition InteriorManagementPoint
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		public int PurchasePrice { get; set; }

		public int BaseDailyIncome { get; set; }

		public int BlipSprite { get; set; }

		public int BlipColor { get; set; }
	}
}
