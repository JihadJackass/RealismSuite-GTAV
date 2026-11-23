using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Native;
using GTA.UI;

namespace iFruitAddon2
{
	internal static class Tools
	{
		internal static class Game
		{
			internal static uint GetPlayerPedHandle()
			{
				return Function.Call<uint>(Hash.PLAYER_PED_ID);
			}

			internal static PedHash GetPlayerPedModelHash()
			{
				return Function.Call<PedHash>(Hash.GET_ENTITY_MODEL, GetPlayerPedHandle());
			}

			internal static void TaskPutAwayMobilePhone()
			{
				Function.Call(Hash.TASK_USE_MOBILE_PHONE, GetPlayerPedHandle(), false);
			}

			internal static void TaskUseMobilePhone()
			{
				Function.Call(Hash.TASK_USE_MOBILE_PHONE, GetPlayerPedHandle(), true);
			}
		}

		internal static class Scripts
		{
			internal static void DestroyPhone(int handle)
			{
				Function.Call(Hash.DESTROY_MOBILE_PHONE, handle);
			}

			internal static void StartScript(string scriptName, int buffer)
			{
				Function.Call(Hash.REQUEST_SCRIPT, scriptName);
				while (!Function.Call<bool>(Hash.HAS_SCRIPT_LOADED, scriptName))
				{
					Function.Call(Hash.REQUEST_SCRIPT, scriptName);
					Script.Yield();
				}
				Function.Call(Hash.START_NEW_SCRIPT, scriptName, buffer);
				Function.Call(Hash.SET_SCRIPT_AS_NO_LONGER_NEEDED, scriptName);
			}

			internal static void TerminateScript(string scriptName)
			{
				Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, scriptName);
			}
		}

		internal static class Ui
		{
			internal static void RemoveActiveNotification()
			{
				int num = Notification.Show("");
				Notification.Hide(num);
				Notification.Hide(num - 1);
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static string GetCurrentMethod(int offset = 0)
		{
			MethodBase method = new StackTrace().GetFrame(1 + offset).GetMethod();
			return method.ReflectedType.Name + "." + method.Name;
		}

		internal static string GetiFruitAddonDirectory()
		{
			return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "iFruitAddon2");
		}
	}
}
