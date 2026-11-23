using System;
using GTA.Native;

namespace BusinessV.Core
{
	internal static class GameDayClock
	{
		public static int Today()
		{
			int year = Math.Max(1, Math.Min(9999, Function.Call<int>(Hash.GET_CLOCK_YEAR)));
			int month = Math.Max(1, Math.Min(12, Function.Call<int>(Hash.GET_CLOCK_MONTH) + 1));
			int day = Math.Max(1, Math.Min(DateTime.DaysInMonth(year, month), Function.Call<int>(Hash.GET_CLOCK_DAY_OF_MONTH)));
			return (int)(new DateTime(year, month, day).Ticks / 864000000000L);
		}
	}
}
