using System.Collections.Generic;

public sealed class SpecialPurchase
{
	public ItemName Item { get; }

	public int Total { get; }

	public int SelectionFlags { get; }

	public IReadOnlyDictionary<string, int> Options { get; }

	internal SpecialPurchase(ItemName item, int total, int flags, Dictionary<string, int> options)
	{
		Item = item;
		Total = total;
		SelectionFlags = flags;
		Options = options;
	}
}
