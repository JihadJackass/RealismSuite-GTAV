using System.Collections.Generic;
using System.Linq;
using GTA;
using GTA.Native;

namespace SinglePlayerHeists
{
	public class InstructionalButtons : BaseScaleform
	{
		public List<Dictionary<Control, string>> Pool { get; set; }

		public bool MouseEnabled { get; set; } = true;


		private bool ShouldUpdate { get; set; } = true;


		public InstructionalButtons()
			: base("INSTRUCTIONAL_BUTTONS")
		{
			Pool = new List<Dictionary<Control, string>>();
		}

		public void SetPool(List<Dictionary<Control, string>> pool)
		{
			Pool = pool;
		}

		public bool AddButton(Dictionary<Control, string> button)
		{
			if (Pool != null && !Pool.Contains(button))
			{
				Pool.Add(button);
				ShouldUpdate = true;
				return true;
			}
			return false;
		}

		public bool RemoveButton(Dictionary<Control, string> button)
		{
			if (Pool != null && Pool.Contains(button))
			{
				Pool.Remove(button);
				ShouldUpdate = true;
				return true;
			}
			return false;
		}

		public void Draw()
		{
			if (ShouldUpdate)
			{
				CallFunction("CLEAR_ALL");
				CallFunction("TOGGLE_MOUSE_BUTTONS", MouseEnabled ? 1 : 0);
				if (Pool != null && Pool.Count > 0)
				{
					for (int i = 0; i < Pool.Count; i++)
					{
						KeyValuePair<Control, string> keyValuePair = Pool[i].First();
						if (MouseEnabled)
						{
							CallFunction("SET_DATA_SLOT", i, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 0, (int)keyValuePair.Key, 0), keyValuePair.Value, true, (int)keyValuePair.Key);
						}
						else
						{
							CallFunction("SET_DATA_SLOT", i, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 0, (int)keyValuePair.Key, 0), keyValuePair.Value);
						}
					}
				}
				CallFunction("DRAW_INSTRUCTIONAL_BUTTONS");
				ShouldUpdate = false;
			}
			DrawFullscreen();
		}
	}
}
