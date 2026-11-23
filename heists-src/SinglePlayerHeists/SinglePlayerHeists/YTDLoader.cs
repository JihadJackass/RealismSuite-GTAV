using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using GTA;
using GTA.Native;
using GTA.UI;

namespace SinglePlayerHeists
{
	public class YTDLoader : Script
	{
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate IntPtr _getTlsContextDelegate();

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void _setTlsContextDelegate(IntPtr ptr);

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private unsafe delegate uint* RegisterIndividualFileDelegate(uint* _index, [MarshalAs(UnmanagedType.LPStr)] string file, bool quitOnBadVersion, [MarshalAs(UnmanagedType.LPStr)] string relativePath, bool quitIfMissing, bool overlayIfExists);

		private bool _shouldDraw;

		private Sprite _sprite;

		public static string YTDtoLoad = "SPHTextures";

		private static _getTlsContextDelegate _getTlsContext = null;

		private static _setTlsContextDelegate _setTlsContext = null;

		private static IntPtr _tlsContextOfMainThread;

		private static RegisterIndividualFileDelegate s_RegisterIndividualFile = null;

		public static bool IsGen9()
		{
			return Process.GetCurrentProcess().ProcessName == "GTA5_Enhanced";
		}

		public YTDLoader()
		{
			base.Tick += OnTick;
		}

		public static void drawText(string text, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		private void OnTick(object sender, EventArgs e)
		{
			if (Main.CanInitializeYTD && !Main.HasInitializedYTD)
			{
				RegisterYtd();
				Main.HasInitializedYTD = true;
			}
		}

		public unsafe static uint RegisterIndividualFile(string file, bool quitOnBadVersion = true, string relativePath = null, bool quitIfMissing = false, bool overlayIfExists = false)
		{
			IntPtr ptr = IntPtr.Zero;
			if (!IsGen9())
			{
				ptr = _getTlsContext();
				_setTlsContext(_tlsContextOfMainThread);
			}
			uint result = uint.MaxValue;
			s_RegisterIndividualFile(&result, file, quitOnBadVersion, relativePath, quitIfMissing, overlayIfExists);
			if (!IsGen9())
			{
				_setTlsContext(ptr);
			}
			return result;
		}

		private static Type GetType(string typeName)
		{
			Type type = Type.GetType(typeName);
			if (type != null)
			{
				return type;
			}
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				type = assembly.GetType(typeName);
				if (type != null)
				{
					return type;
				}
			}
			return null;
		}

		public unsafe static void RegisterYtd()
		{
			IntPtr intPtr = ((!IsGen9()) ? Game.FindPattern("E8 ?? ?? ?? ?? 8B 08 89 4F ?? 89 0D", (IntPtr)0) : Game.FindPattern("E8 ?? ?? ?? ?? 8B 44 24 ?? 89 46 ?? 89 05", (IntPtr)0));
			if (intPtr != IntPtr.Zero)
			{
				IntPtr pointer = IntPtr.Add(intPtr, *(int*)(void*)IntPtr.Add(intPtr, 1));
				pointer = IntPtr.Add(pointer, 5);
				s_RegisterIndividualFile = Marshal.GetDelegateForFunctionPointer<RegisterIndividualFileDelegate>(pointer);
				if (!IsGen9())
				{
					Type type = GetType("SHVDN.ScriptDomain");
					PropertyInfo property = type.GetProperty("CurrentDomain", BindingFlags.Static | BindingFlags.Public);
					FieldInfo field = type.GetField("_getTlsContext", BindingFlags.Instance | BindingFlags.NonPublic);
					FieldInfo field2 = type.GetField("_setTlsContext", BindingFlags.Instance | BindingFlags.NonPublic);
					FieldInfo field3 = type.GetField("_tlsContextOfMainThread", BindingFlags.Instance | BindingFlags.NonPublic);
					object value = property.GetValue(null);
					_getTlsContext = Marshal.GetDelegateForFunctionPointer<_getTlsContextDelegate>((IntPtr)field.GetValue(value));
					_setTlsContext = Marshal.GetDelegateForFunctionPointer<_setTlsContextDelegate>((IntPtr)field2.GetValue(value));
					_tlsContextOfMainThread = (IntPtr)field3.GetValue(value);
				}
				string text = (IsGen9() ? "Enhanced" : "Legacy");
				if (IsGen9())
				{
					uint num = RegisterIndividualFile(AppDomain.CurrentDomain.BaseDirectory + "/HKH_SinglePlayerHeists/YTD/Enhanced/SPHTextures.ytd", true, "SPHTextures.ytd");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "SPHTextures", 0);
				}
				else
				{
					uint num2 = RegisterIndividualFile("scripts/HKH_SinglePlayerHeists/YTD/Legacy/SPHTextures.ytd", true, "SPHTextures.ytd");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "SPHTextures", 0);
				}
			}
			else
			{
				Notification.Show("Single Player Heists Error: YTD Import Mem Address is Unkown.");
			}
		}
	}
}
