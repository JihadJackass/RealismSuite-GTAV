using System.Collections.Generic;
using System.Drawing;
using GTA;
using GTA.Native;

namespace INMNativeUI.PauseMenu
{
	public class TabSubmenuItem : TabItem
	{
		private bool _focused;

		public List<TabItem> Items { get; set; }

		public int Index { get; set; }

		public bool IsInList { get; set; }

		public override bool Focused
		{
			get
			{
				return _focused;
			}
			set
			{
				_focused = value;
				if (!value)
				{
					Items[Index].Focused = false;
				}
			}
		}

		public TabSubmenuItem(string name, IEnumerable<TabItem> items)
			: base(name)
		{
			DrawBg = false;
			base.CanBeFocused = true;
			Items = new List<TabItem>(items);
			IsInList = true;
		}

		public void RefreshIndex()
		{
			foreach (TabItem item in Items)
			{
				item.Focused = false;
				item.Active = false;
				item.Visible = false;
			}
			Index = (1000 - 1000 % Items.Count) % Items.Count;
		}

		public override void ProcessControls()
		{
			if (base.JustOpened)
			{
				base.JustOpened = false;
			}
			else
			{
				if (!Focused)
				{
					return;
				}
				if (Game.IsControlJustPressed(Control.PhoneSelect) && Focused && base.Parent.FocusLevel == 1)
				{
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
					if (Items[Index].CanBeFocused && !Items[Index].Focused)
					{
						base.Parent.FocusLevel++;
						Items[Index].JustOpened = true;
						Items[Index].Focused = true;
					}
					else
					{
						Items[Index].OnActivated();
					}
				}
				if (Game.IsControlJustPressed(Control.PhoneCancel) && Focused && base.Parent.FocusLevel > 1)
				{
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "CANCEL", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
					if (Items[Index].CanBeFocused && Items[Index].Focused)
					{
						base.Parent.FocusLevel--;
						Items[Index].Focused = false;
					}
				}
				if ((Game.IsControlJustPressed(Control.FrontendUp) || Game.IsControlJustPressed(Control.MoveUpOnly) || Game.IsControlJustPressed(Control.CursorScrollUp)) && base.Parent.FocusLevel == 1)
				{
					Index = (1000 - 1000 % Items.Count + Index - 1) % Items.Count;
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
				}
				else if ((Game.IsControlJustPressed(Control.FrontendDown) || Game.IsControlJustPressed(Control.MoveDownOnly) || Game.IsControlJustPressed(Control.CursorScrollDown)) && base.Parent.FocusLevel == 1)
				{
					Index = (1000 - 1000 % Items.Count + Index + 1) % Items.Count;
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
				}
				if (Items.Count > 0)
				{
					Items[Index].ProcessControls();
				}
			}
		}

		public override void Draw()
		{
			if (!Visible)
			{
				return;
			}
			base.Draw();
			SizeF screenResolutionMaintainRatio = UIMenu.GetScreenResolutionMaintainRatio();
			bool focused = Focused;
			int alpha = (Focused ? 200 : 100);
			int alpha2 = (Focused ? 255 : 150);
			float num = screenResolutionMaintainRatio.Width - (float)(base.SafeSize.X * 2);
			int num2 = (int)(num * 0.6818f);
			Size size = new Size((int)num - (num2 + 3), 40);
			for (int i = 0; i < Items.Count; i++)
			{
				bool flag = UIMenu.IsMouseInBounds(base.SafeSize.AddPoints(new Point(0, (size.Height + 3) * i)), size);
				new UIResRectangle(base.SafeSize.AddPoints(new Point(0, (size.Height + 3) * i)), size, (Index == i && Focused) ? Color.FromArgb(alpha2, Color.White) : ((flag && Focused) ? Color.FromArgb(100, 50, 50, 50) : Color.FromArgb(alpha, Color.Black))).Draw();
				new UIResText(Items[i].Title, base.SafeSize.AddPoints(new Point(6, 5 + (size.Height + 3) * i)), 0.35f, Color.FromArgb(alpha2, (Index == i && Focused) ? Color.Black : Color.White)).Draw();
				if (!(Focused && flag) || !Game.IsControlJustPressed(Control.CursorAccept))
				{
					continue;
				}
				Items[Index].Focused = false;
				Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
				bool num3 = Index == i;
				Index = (1000 - 1000 % Items.Count + i) % Items.Count;
				if (num3)
				{
					if (Items[Index].CanBeFocused && !Items[Index].Focused)
					{
						base.Parent.FocusLevel = 2;
						Items[Index].JustOpened = true;
						Items[Index].Focused = true;
					}
					else
					{
						Items[Index].OnActivated();
					}
				}
				else
				{
					base.Parent.FocusLevel = 1;
				}
			}
			Items[Index].Visible = true;
			Items[Index].FadeInWhenFocused = true;
			if (!Items[Index].CanBeFocused)
			{
				Items[Index].Focused = Focused;
			}
			Items[Index].UseDynamicPositionment = false;
			Items[Index].SafeSize = base.SafeSize.AddPoints(new Point((int)num - num2, 0));
			Items[Index].TopLeft = base.SafeSize.AddPoints(new Point((int)num - num2, 0));
			Items[Index].BottomRight = new Point((int)screenResolutionMaintainRatio.Width - base.SafeSize.X, (int)screenResolutionMaintainRatio.Height - base.SafeSize.Y);
			Items[Index].Draw();
		}
	}
}
