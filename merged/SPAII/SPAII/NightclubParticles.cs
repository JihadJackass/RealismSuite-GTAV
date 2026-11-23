using System;
using GTA.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class NightclubParticles
	{
		internal const string Asset = "scr_ba_club";

		private static int users;

		private static bool ownsRequest;

		internal static bool Ready()
		{
			return Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_ba_club");
		}

		internal static void Acquire()
		{
			checked
			{
				users++;
				if (!Ready())
				{
					Request();
				}
			}
		}

		internal static void Request()
		{
			Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ba_club");
			ownsRequest = true;
		}

		internal static void Release(bool storyLoad)
		{
			if (users <= 0)
			{
				return;
			}
			checked
			{
				users--;
				if (users != 0 || !ownsRequest)
				{
					return;
				}
				ownsRequest = false;
				if (!storyLoad)
				{
					try
					{
						Function.Call(Hash.REMOVE_NAMED_PTFX_ASSET, "scr_ba_club");
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						Logger.Log("After Hours particles release: " + ex2.Message);
						ProjectData.ClearProjectError();
					}
				}
			}
		}
	}
}
