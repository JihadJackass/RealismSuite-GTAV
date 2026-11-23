using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GTA;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class ConfigHelper
	{
		public static ScriptSettings config = ScriptSettings.Load("scripts\\SPA II\\modconfig.ini");

		public static bool DebugMode = false;

		public static bool OnlineMap = true;

		public static int SoundVolume = 100;

		public static int OnlineMapDelay = 0;

		public static string OnlineMapMode = "Always";

		public static int OnlineMapRadius = 250;

		public static bool HideBlipsOnMission = true;

		public static bool OnlineMapAdditive = false;

		public static void LoadStartupConfig()
		{
			config = ScriptSettings.Load("scripts\\SPA II\\modconfig.ini");
			OnlineMap = config.GetValue("SETTING", "OnlineMap", true);
			OnlineMapDelay = Math.Max(0, config.GetValue("SETTING", "OnlineMapDelay", 0));
			OnlineMapMode = config.GetValue("SETTING", "OnlineMapMode", "Always").Trim();
			OnlineMapAdditive = config.GetValue("SETTING", "OnlineMapAdditive", false);
			if (!OnlineMap)
			{
				OnlineMapMode = "Off";
			}
		}

		public static void LoadModConfig()
		{
			LoadStartupConfig();
			SoundVolume = config.GetValue("SOUND", "Volume", 100);
			DebugMode = config.GetValue("SETTING", "DebugMode", false);
			Helper.ShopControllerGlobal = config.GetValue("SETTING", "ShopControllerGlobal", 0);
			OnlineMapRadius = config.GetValue("SETTING", "OnlineMapRadius", 250);
			HideBlipsOnMission = config.GetValue("SETTING", "HideBlipsOnMission", true);
			SpaBlips.ToggleKey = config.GetValue("SETTING", "ToggleBlipsKey", Keys.F10);
		}

		public static void GenerateModConfig()
		{
			if (File.Exists("scripts\\SPA II\\modconfig.ini"))
			{
				return;
			}
			config = ScriptSettings.Load("scripts\\SPA II\\modconfig.ini");
			config.SetValue("SOUND", "Volume", 100);
			config.SetValue("SETTING", "OnlineMap", true);
			config.SetValue("SETTING", "HideBlipsOnMission", true);
			config.SetValue("SETTING", "ToggleBlipsKey", Keys.F10);
			config.SetValue("SETTING", "DebugMode", false);
			config.SetValue("SETTING", "ShopControllerGlobal", 0);
			config.SetValue("SETTING", "OnlineMapDelay", 0);
			config.SetValue("SETTING", "OnlineMapMode", "Always");
			config.SetValue("SETTING", "OnlineMapRadius", 250);
			config.SetValue("SETTING", "OnlineMapAdditive", false);
			int[] source = new int[4] { 82, 88, 91, 93 };
			int num = 1;
			do
			{
				if (!source.Contains(num))
				{
					config.SetValue("BUILDING", $"MP_PROP_{num}", eOwner.Nobody);
				}
				num = checked(num + 1);
			}
			while (num <= 95);
			config.SetValue("IPL", "MP_PROP_79", "apa_v_mp_h_01_a");
			config.SetValue("IPL", "MP_PROP_80", "apa_v_mp_h_01_b");
			config.SetValue("IPL", "MP_PROP_81", "apa_v_mp_h_01_c");
			config.SetValue("APARTMENT", "3AST", true);
			config.SetValue("APARTMENT", "4IWY", true);
			config.SetValue("APARTMENT", "DPHS", true);
			config.SetValue("APARTMENT", "ESTS", true);
			config.SetValue("APARTMENT", "RCMC", true);
			config.SetValue("APARTMENT", "TSTS", true);
			config.SetValue("APARTMENT", "WZPZ", true);
			config.SetValue("APARTMENT", "2862HCAV", true);
			config.SetValue("APARTMENT", "2866HCAV", true);
			config.SetValue("APARTMENT", "2868HCAV", true);
			config.SetValue("APARTMENT", "2874HCAV", true);
			config.SetValue("APARTMENT", "2113MWTD", true);
			config.SetValue("APARTMENT", "2117MTRD", true);
			config.SetValue("APARTMENT", "2044NCAV", true);
			config.SetValue("APARTMENT", "2045NCAV", true);
			config.SetValue("APARTMENT", "3677WMDR", true);
			config.SetValue("APARTMENT", "3655WODR", true);
			config.SetValue("APARTMENT", "0115BCAV", true);
			config.SetValue("APARTMENT", "DMTR", true);
			config.SetValue("APARTMENT", "4HMDR", true);
			config.SetValue("APARTMENT", "0604LLBL", true);
			config.SetValue("APARTMENT", "0184MTRD", true);
			config.SetValue("APARTMENT", "1162PWST", true);
			config.SetValue("APARTMENT", "4401PPDR", true);
			config.SetValue("APARTMENT", "4584PPDR", true);
			config.SetValue("APARTMENT", "0504SMMD", true);
			config.SetValue("APARTMENT", "0325SRFD", true);
			config.SetValue("APARTMENT", "0605SNAV", true);
			config.SetValue("APARTMENT", "12STCR", true);
			config.SetValue("APARTMENT", "TRYL", true);
			config.SetValue("APARTMENT", "1115BDPR", true);
			config.SetValue("APARTMENT", "1561SVTS", true);
			config.SetValue("APARTMENT", "1237PPRS", true);
			config.SetValue("APARTMENT", "0069CGAV", true);
			config.SetValue("APARTMENT", "2143LLBL", true);
			config.SetValue("APARTMENT", "1893GSAV", true);
			config.SetValue("APARTMENT", "0232PLTB", true);
			config.SetValue("APARTMENT", "0112SRFD", true);
			config.SetValue("APARTMENT", "2057VPCB", true);
			config.SetValue("APARTMENT", "140ZCDA", true);
			config.SetValue("APARTMENT", "MRTH", true);
			config.SetValue("APARTMENT", "U2PS", true);
			config.SetValue("APARTMENT", "331SPST", true);
			config.SetValue("APARTMENT", "U76GP", true);
			config.SetValue("APARTMENT", "133XCTW", true);
			config.SetValue("APARTMENT", "1623SSBS", true);
			config.SetValue("APARTMENT", "0552RLSB", true);
			config.SetValue("APARTMENT", "U14PS", true);
			config.SetValue("APARTMENT", "1905DVAV", true);
			config.SetValue("APARTMENT", "0432DVAV", true);
			config.SetValue("APARTMENT", "U1OF", true);
			config.SetValue("APARTMENT", "4531DST", true);
			config.SetValue("APARTMENT", "8754RT68", true);
			config.SetValue("APARTMENT", "870R68A", true);
			config.SetValue("APARTMENT", "0897MPBL", true);
			config.SetValue("APARTMENT", "GICB", true);
			config.SetValue("APARTMENT", "634BDPR", true);
			config.SetValue("APARTMENT", "1920SNRW", true);
			config.SetValue("APARTMENT", "12LBHA", true);
			config.SetValue("APARTMENT", "2000GOHW", true);
			config.SetValue("APARTMENT", "0754RLSB", true);
			config.SetValue("APARTMENT", "197RT68", true);
			config.SetValue("APARTMENT", "1200RT68", true);
			config.SetValue("APARTMENT", "1932GSAV", true);
			config.SetValue("APARTMENT", "142PLTB", true);
			config.SetValue("APARTMENT", "1SBAV", true);
			config.SetValue("APARTMENT", "U124PS", true);
			config.Save();
		}
	}
}
