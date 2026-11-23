using GTA.UI;
using SHVDN;

public static class MpCarsCommands
{
	[ConsoleCommand("MP Cars in SP: emergency cleanup of the character's garage slots.")]
	public static void MpCarsClearGarages()
	{
		WebVehicleShops.PanicClearGarages();
	}

	[ConsoleCommand("MP Cars in SP: reload price overrides from MpCarsPrices.ini.")]
	public static void MpCarsReloadPrices()
	{
		WebPrices.Load();
		Notification.Show("~g~MP Cars:~s~ price overrides reloaded.");
	}
}
