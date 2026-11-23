using System;
using GTA;
using GTA.Native;

namespace BusinessV.World
{
	internal sealed class BusinessInstructionalButtons : IDisposable
	{
		private Scaleform scaleform;

		private bool configured;

		private bool configuredForKeyboard;

		private bool configuredForHorizontalNavigation;

		public void Draw(bool horizontalNavigation)
		{
			if (scaleform == null)
			{
				scaleform = Scaleform.RequestMovie("instructional_buttons");
			}
			if (scaleform.IsLoaded)
			{
				bool flag = Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2);
				if (!configured || configuredForKeyboard != flag || configuredForHorizontalNavigation != horizontalNavigation)
				{
					Configure(horizontalNavigation, flag);
				}
				scaleform.Render2D();
			}
		}

		public void Dispose()
		{
			scaleform?.Dispose();
			scaleform = null;
			configured = false;
		}

		private void Configure(bool horizontalNavigation, bool keyboard)
		{
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", false);
			scaleform.CallFunction("CREATE_CONTAINER");
			SetSlot(0, "Back", Control.FrontendCancel);
			SetSlot(1, "Select", Control.FrontendAccept);
			SetSlot(2, "Navigate", horizontalNavigation ? Control.FrontendLeft : Control.FrontendUp, horizontalNavigation ? Control.FrontendRight : Control.FrontendDown);
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.CallFunction("SET_BACKGROUND_COLOUR", 0, 0, 0, 80);
			configured = true;
			configuredForKeyboard = keyboard;
			configuredForHorizontalNavigation = horizontalNavigation;
		}

		private void SetSlot(int slot, string label, params Control[] controls)
		{
			object[] array = new object[controls.Length + 2];
			array[0] = slot;
			for (int i = 0; i < controls.Length; i++)
			{
				array[i + 1] = Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, (int)controls[i], true);
			}
			array[array.Length - 1] = label;
			scaleform.CallFunction("SET_DATA_SLOT", array);
		}
	}
}
