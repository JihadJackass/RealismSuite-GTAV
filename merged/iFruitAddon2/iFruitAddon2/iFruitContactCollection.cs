using System.Collections.Generic;
using GTA;
using GTA.Native;

namespace iFruitAddon2
{
	public class iFruitContactCollection : List<iFruitContact>
	{
		private bool _shouldDraw = true;

		private readonly int _appContactScriptHash;

		public iFruitContactCollection()
		{
			Logger.Debug("Initializing new iFruitContactCollection...");
			_appContactScriptHash = Game.GenerateHash("appcontacts");
			Logger.Debug("iFruitContactCollection initialized!");
		}

		internal void Update(int handle)
		{
			Update(handle, true);
		}

		internal void Update(int handle, bool phoneOnScreen)
		{
			if (!phoneOnScreen)
			{
				using (Enumerator enumerator = GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.Update();
					}
					return;
				}
			}
			int num = 0;
			if (Function.Call<int>(Hash.GET_NUMBER_OF_THREADS_RUNNING_THE_SCRIPT_WITH_THIS_HASH, _appContactScriptHash) > 0)
			{
				_shouldDraw = true;
				Logger.Debug("Current contact indexes: [" + string.Join(", ", ConvertAll((iFruitContact c) => c.Index)) + "]");
				if (Game.IsControlPressed(Control.PhoneSelect))
				{
					Logger.Debug("Reading the index of the selected contact...");
					num = GetSelectedIndex(handle);
					Logger.Debug("Selected contact: " + num);
				}
			}
			else
			{
				num = -1;
			}
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					iFruitContact current = enumerator.Current;
					current.Update();
					if (_shouldDraw)
					{
						current.Draw(handle);
					}
					if (num != -1 && num == current.Index)
					{
						Logger.Debug("Contact has been selected for calling");
						Tools.Scripts.TerminateScript("appcontacts");
						Logger.Debug("Script killed!");
						Logger.Debug("Calling contact...");
						current.Call();
						DisplayCallUI(handle, current.Name, "CELL_211", current.Icon.Name.SetBold(current.Bold));
						Logger.Debug("Contact called!");
						Script.Wait(10);
						Logger.Debug("Removing notification...");
						Tools.Ui.RemoveActiveNotification();
						Logger.Debug("Notification removed!");
					}
				}
			}
			_shouldDraw = false;
		}

		public static void DisplayCallUI(int handle, string contactName, string statusText = "CELL_211", string picName = "CELL_300")
		{
			string localizedString = Game.GetLocalizedString(statusText);
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, handle, "SET_DATA_SLOT");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 4);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 0);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 3);
			Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PHONE_NUMBER, contactName, -1);
			Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
			Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "CELL_2000");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, picName);
			Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
			Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PHONE_NUMBER, localizedString, -1);
			Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, handle, "DISPLAY_VIEW");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 4);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		internal int GetSelectedIndex(int handle)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, handle, "GET_CURRENT_SELECTION");
			int num = Function.Call<int>(Hash.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE);
			int num2 = Game.GameTime + 1000;
			while (!Function.Call<bool>(Hash.IS_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_READY, num))
			{
				if (Game.GameTime > num2)
				{
					return -1;
				}
				Script.Wait(0);
			}
			return Function.Call<int>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_INT, num);
		}
	}
}
