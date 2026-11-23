using System.Collections.Generic;

public sealed class SiteConfig
{
	public int WebsiteId;

	public string SiteName;

	public Dictionary<int, int> IdToHash;

	public Dictionary<int, int> IdToSbv;

	public string SuccessPage = "PURCHASE_D_SUCCESS";

	public string FailPage = "PURCHASE_D_FAILED";

	public int PurchasePageId = 2;

	public HashSet<int> NavPageIds;
}
