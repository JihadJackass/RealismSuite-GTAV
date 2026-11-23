using GTA;

public class WebVehicleShopsScript : Script
{
	public WebVehicleShopsScript()
	{
		base.Interval = 0;
		WebPrices.Load();
		base.Tick += delegate
		{
			WebVehicleShops.Update();
		};
		base.Aborted += delegate
		{
			WebVehicleShops.FinalizeOnAbort();
		};
	}
}
