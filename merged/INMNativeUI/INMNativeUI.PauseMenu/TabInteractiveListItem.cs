using System;
using System.Collections.Generic;
using System.Drawing;
using GTA;
using GTA.Native;
using GTA.UI;

namespace INMNativeUI.PauseMenu
{
	public class TabInteractiveListItem : TabItem
	{
		protected const int MaxItemsPerView = 15;

		protected int _minItem;

		protected int _maxItem;

		private bool _focused;

		public List<UIMenuItem> Items { get; set; }

		public int Index { get; set; }

		public bool IsInList { get; set; }

		public TabInteractiveListItem(string name, IEnumerable<UIMenuItem> items)
			: base(name)
		{
			DrawBg = false;
			base.CanBeFocused = true;
			Items = new List<UIMenuItem>(items);
			IsInList = true;
			_maxItem = 15;
			_minItem = 0;
		}

		public void MoveDown()
		{
			Index = (1000 - 1000 % Items.Count + Index + 1) % Items.Count;
			if (Items.Count > 15)
			{
				if (Index >= _maxItem)
				{
					_maxItem++;
					_minItem++;
				}
				if (Index == 0)
				{
					_minItem = 0;
					_maxItem = 15;
				}
			}
		}

		public void MoveUp()
		{
			Index = (1000 - 1000 % Items.Count + Index - 1) % Items.Count;
			if (Items.Count > 15)
			{
				if (Index < _minItem)
				{
					_minItem--;
					_maxItem--;
				}
				if (Index == Items.Count - 1)
				{
					_minItem = Items.Count - 15;
					_maxItem = Items.Count;
				}
			}
		}

		public void RefreshIndex()
		{
			Index = 0;
			_maxItem = 15;
			_minItem = 0;
		}

		public override void ProcessControls()
		{
			if (!Visible)
			{
				return;
			}
			if (base.JustOpened)
			{
				base.JustOpened = false;
			}
			else if (Focused && Items.Count != 0)
			{
				if (Game.IsControlJustPressed(Control.FrontendAccept) && Focused && Items[Index] is UIMenuCheckboxItem)
				{
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
					((UIMenuCheckboxItem)Items[Index]).Checked = !((UIMenuCheckboxItem)Items[Index]).Checked;
					((UIMenuCheckboxItem)Items[Index]).CheckboxEventTrigger();
				}
				else if (Game.IsControlJustPressed(Control.FrontendAccept) && Focused)
				{
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
					Items[Index].ItemActivate(null);
				}
				if (Game.IsControlJustPressed(Control.FrontendLeft) && Focused && Items[Index] is UIMenuListItem)
				{
					UIMenuListItem obj = (UIMenuListItem)Items[Index];
					obj.Index--;
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
					obj.ListChangedTrigger(obj.Index);
				}
				if (Game.IsControlJustPressed(Control.FrontendRight) && Focused && Items[Index] is UIMenuListItem)
				{
					UIMenuListItem obj2 = (UIMenuListItem)Items[Index];
					obj2.Index++;
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
					obj2.ListChangedTrigger(obj2.Index);
				}
				if (Game.IsControlJustPressed(Control.FrontendUp) || Game.IsControlJustPressed(Control.MoveUpOnly) || Game.IsControlJustPressed(Control.CursorScrollUp))
				{
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
					MoveUp();
				}
				else if (Game.IsControlJustPressed(Control.FrontendDown) || Game.IsControlJustPressed(Control.MoveDownOnly) || Game.IsControlJustPressed(Control.CursorScrollDown))
				{
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
					MoveDown();
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
			UIMenu.GetScreenResolutionMaintainRatio();
			bool focused = Focused;
			int alpha = (Focused ? 200 : 100);
			int alpha2 = (Focused ? 255 : 150);
			int width = base.BottomRight.X - base.TopLeft.X;
			Size size = new Size(width, 40);
			int num = 0;
			for (int i = _minItem; i < Math.Min(Items.Count, _maxItem); i++)
			{
				bool flag = UIMenu.IsMouseInBounds(base.SafeSize.AddPoints(new Point(0, (size.Height + 3) * num)), size);
				bool flag2 = Items[i].LeftBadge != UIMenuItem.BadgeStyle.None;
				bool flag3 = Items[i].RightBadge != UIMenuItem.BadgeStyle.None;
				bool flag4 = flag3 && flag2;
				bool flag5 = flag3 || flag2;
				new UIResRectangle(base.SafeSize.AddPoints(new Point(0, (size.Height + 3) * num)), size, (Index == i && Focused) ? Color.FromArgb(alpha2, Color.White) : ((Focused && flag) ? Color.FromArgb(100, 50, 50, 50) : Color.FromArgb(alpha, Color.Black))).Draw();
				new UIResText(Items[i].Text, base.SafeSize.AddPoints(new Point(flag4 ? 60 : (flag5 ? 30 : 6), 5 + (size.Height + 3) * num)), 0.35f, Color.FromArgb(alpha2, (Index == i && Focused) ? Color.Black : Color.White)).Draw();
				if (flag2 && !flag3)
				{
					new Sprite(UIMenuItem.BadgeToSpriteLib(Items[i].LeftBadge), UIMenuItem.BadgeToSpriteName(Items[i].LeftBadge, Index == i && Focused), base.SafeSize.AddPoints(new Point(-2, 1 + (size.Height + 3) * num)), new Size(40, 40), 0f, UIMenuItem.BadgeToColor(Items[i].LeftBadge, Index == i && Focused)).Draw();
				}
				if (!flag2 && flag3)
				{
					new Sprite(UIMenuItem.BadgeToSpriteLib(Items[i].RightBadge), UIMenuItem.BadgeToSpriteName(Items[i].RightBadge, Index == i && Focused), base.SafeSize.AddPoints(new Point(-2, 1 + (size.Height + 3) * num)), new Size(40, 40), 0f, UIMenuItem.BadgeToColor(Items[i].RightBadge, Index == i && Focused)).Draw();
				}
				if (flag2 && flag3)
				{
					new Sprite(UIMenuItem.BadgeToSpriteLib(Items[i].LeftBadge), UIMenuItem.BadgeToSpriteName(Items[i].LeftBadge, Index == i && Focused), base.SafeSize.AddPoints(new Point(-2, 1 + (size.Height + 3) * num)), new Size(40, 40), 0f, UIMenuItem.BadgeToColor(Items[i].LeftBadge, Index == i && Focused)).Draw();
					new Sprite(UIMenuItem.BadgeToSpriteLib(Items[i].RightBadge), UIMenuItem.BadgeToSpriteName(Items[i].RightBadge, Index == i && Focused), base.SafeSize.AddPoints(new Point(25, 1 + (size.Height + 3) * num)), new Size(40, 40), 0f, UIMenuItem.BadgeToColor(Items[i].RightBadge, Index == i && Focused)).Draw();
				}
				if (!string.IsNullOrEmpty(Items[i].RightLabel))
				{
					new UIResText(Items[i].RightLabel, base.SafeSize.AddPoints(new Point(base.BottomRight.X - base.SafeSize.X - 5, 5 + (size.Height + 3) * num)), 0.35f, Color.FromArgb(alpha2, (Index == i && Focused) ? Color.Black : Color.White), GTA.UI.Font.ChaletLondon, UIResText.Alignment.Right).Draw();
				}
				if (Items[i] is UIMenuCheckboxItem)
				{
					string text = "";
					text = ((i != Index || !Focused) ? (((UIMenuCheckboxItem)Items[i]).Checked ? "shop_box_tick" : "shop_box_blank") : (((UIMenuCheckboxItem)Items[i]).Checked ? "shop_box_tickb" : "shop_box_blankb"));
					new Sprite("commonmenu", text, base.SafeSize.AddPoints(new Point(base.BottomRight.X - base.SafeSize.X - 60, -5 + (size.Height + 3) * num)), new Size(50, 50)).Draw();
				}
				else if (Items[i] is UIMenuListItem)
				{
					UIMenuListItem uIMenuListItem = (UIMenuListItem)Items[i];
					int num2 = 5;
					Point position = base.SafeSize.AddPoints(new Point(base.BottomRight.X - base.SafeSize.X - 30, num2 + (size.Height + 3) * num));
					Sprite sprite = new Sprite("commonmenu", "arrowleft", position, new Size(30, 30));
					Sprite sprite2 = new Sprite("commonmenu", "arrowright", position, new Size(30, 30));
					UIResText uIResText = new UIResText("", position, 0.35f, Color.White, GTA.UI.Font.ChaletLondon, UIResText.Alignment.Left)
					{
						TextAlignment = UIResText.Alignment.Right
					};
					string text2 = uIMenuListItem.IndexToItem(uIMenuListItem.Index).ToString();
					float num3 = StringMeasurer.MeasureString(text2, uIResText.Font, uIResText.Scale);
					bool flag6 = i == Index && Focused;
					uIResText.Color = ((!uIMenuListItem.Enabled) ? Color.FromArgb(163, 159, 148) : (flag6 ? Color.Black : Color.WhiteSmoke));
					uIResText.Caption = text2;
					sprite.Color = ((!uIMenuListItem.Enabled) ? Color.FromArgb(163, 159, 148) : (flag6 ? Color.Black : Color.WhiteSmoke));
					sprite2.Color = ((!uIMenuListItem.Enabled) ? Color.FromArgb(163, 159, 148) : (flag6 ? Color.Black : Color.WhiteSmoke));
					sprite.Position = base.SafeSize.AddPoints(new Point(base.BottomRight.X - base.SafeSize.X - 60 - (int)num3, num2 + (size.Height + 3) * num));
					if (flag6)
					{
						sprite.Draw();
						sprite2.Draw();
						uIResText.Position = base.SafeSize.AddPoints(new Point(base.BottomRight.X - base.SafeSize.X - 30, num2 + (size.Height + 3) * num));
					}
					else
					{
						uIResText.Position = base.SafeSize.AddPoints(new Point(base.BottomRight.X - base.SafeSize.X - 5, num2 + (size.Height + 3) * num));
					}
					uIResText.Draw();
				}
				if (Focused && flag && Game.IsControlJustPressed(Control.CursorAccept))
				{
					bool num4 = Index == i;
					Index = (1000 - 1000 % Items.Count + i) % Items.Count;
					if (!num4)
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
					}
					else if (Items[Index] is UIMenuCheckboxItem)
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
						((UIMenuCheckboxItem)Items[Index]).Checked = !((UIMenuCheckboxItem)Items[Index]).Checked;
						((UIMenuCheckboxItem)Items[Index]).CheckboxEventTrigger();
					}
					else
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
						Items[Index].ItemActivate(null);
					}
				}
				num++;
			}
		}
	}
}
