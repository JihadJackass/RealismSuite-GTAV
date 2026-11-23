using System;
using System.Collections.Generic;

public static class WebSP
{
	private static readonly HashSet<ItemName> _unlocked = new HashSet<ItemName>();

	private static bool _dirty;

	private static readonly Queue<SpecialPurchase> _purchases = new Queue<SpecialPurchase>();

	public static SpecialPurchase LastPurchase { get; private set; }

	public static event Action<SpecialPurchase> PurchaseCompleted;

	public static void UnlockItem(ItemName item)
	{
		if (_unlocked.Add(item))
		{
			_dirty = true;
		}
	}

	public static void LockItem(ItemName item)
	{
		if (_unlocked.Remove(item))
		{
			_dirty = true;
		}
	}

	public static bool IsUnlocked(ItemName item)
	{
		return _unlocked.Contains(item);
	}

	public static void UnlockAll()
	{
		foreach (ItemName value in Enum.GetValues(typeof(ItemName)))
		{
			if (value != ItemName.ArenaWorkshop)
			{
				UnlockItem(value);
			}
		}
	}

	public static bool TryDequeuePurchase(out SpecialPurchase purchase)
	{
		if (_purchases.Count > 0)
		{
			purchase = _purchases.Dequeue();
			return true;
		}
		purchase = null;
		return false;
	}

	internal static void ReportPurchase(SpecialPurchase p)
	{
		LastPurchase = p;
		_purchases.Enqueue(p);
		if (_purchases.Count > 64)
		{
			_purchases.Dequeue();
		}
		Action<SpecialPurchase> purchaseCompleted = WebSP.PurchaseCompleted;
		if (purchaseCompleted != null)
		{
			try
			{
				purchaseCompleted(p);
			}
			catch
			{
			}
		}
	}

	internal static bool ConsumeDirty()
	{
		bool dirty = _dirty;
		_dirty = false;
		return dirty;
	}
}
