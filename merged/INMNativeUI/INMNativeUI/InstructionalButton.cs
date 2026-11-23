using GTA;
using GTA.Native;

namespace INMNativeUI
{
	public class InstructionalButton
	{
		private readonly string _buttonString;

		private readonly Control _buttonControl;

		private readonly bool _usingControls;

		public string Text { get; set; }

		public UIMenuItem ItemBind { get; private set; }

		public InstructionalButton(Control control, string text)
		{
			Text = text;
			_buttonControl = control;
			_usingControls = true;
		}

		public InstructionalButton(string keystring, string text)
		{
			Text = text;
			_buttonString = keystring;
			_usingControls = false;
		}

		public void BindToItem(UIMenuItem item)
		{
			ItemBind = item;
		}

		public string GetButtonId()
		{
			if (!_usingControls)
			{
				return "t_" + _buttonString;
			}
			return Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, (int)_buttonControl, 0);
		}
	}
}
