using System.Collections.Generic;
using GTA;
using GTA.Native;

namespace SinglePlayerHeists
{
	public class FrontendLobbyMenuSliderItem<T> : FrontendLobbyMenuBaseItem
	{
		public List<T> Items { get; set; }

		public T Value
		{
			get
			{
				if (Items != null && Items.Count > 0 && Index >= 0 && Index < Items.Count)
				{
					return Items[Index];
				}
				return default(T);
			}
		}

		public int Index { get; set; }

		public FrontendMenuSliderItemValueChangedEventHandler<T> ValueChanged { get; set; }

		public FrontendLobbyMenuSliderItem(string title, string description, List<T> list, int startingIndex = 0)
			: base(title, description)
		{
			Items = list;
			Index = startingIndex;
		}

		public override void Add(int index)
		{
			if (Items.Count > 0 && Index < Items.Count)
			{
				base.Add(index);
				BaseScaleform.CallFunctionFrontend("SET_DATA_SLOT", 0, index, 0, index, 0, 0, true, base.Text, string.Empty, 0, Items[Index].ToString(), 0, false);
			}
		}

		public override void Update(int index)
		{
			if (!FrontendLobbyMenu.AwaitingMenuUpdate && Items.Count > 0 && Index < Items.Count)
			{
				base.Update(index);
				BaseScaleform.CallFunctionFrontend("UPDATE_SLOT", 0, index, 0, index, 0, 0, true, base.Text, string.Empty, 0, Items[Index].ToString(), 0, false);
			}
		}

		public override void Process(int index)
		{
			bool flag = false;
			if (Game.IsControlJustPressed(Control.FrontendRight) && Index < Items.Count - 1)
			{
				Index++;
				flag = true;
			}
			if (Game.IsControlJustPressed(Control.FrontendLeft) && Index > 0)
			{
				Index--;
				flag = true;
			}
			if (flag && Items.Count > 0 && Index < Items.Count)
			{
				ValueChanged?.Invoke(this, new FrontendMenuSliderItemValueChangedArgs<T>(Value, Index, this));
				Update(index);
				Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
			}
		}
	}
}
