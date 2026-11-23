using System;
using System.Drawing;
using GTA.UI;

namespace INMNativeUI
{
	public class UIMenuDynamicListItem : UIMenuItem, IListItem
	{
		public enum ChangeDirection
		{
			Left,
			Right
		}

		public delegate string DynamicListItemChangeCallback(UIMenuDynamicListItem sender, ChangeDirection direction);

		protected UIResText _itemText;

		protected Sprite _arrowLeft;

		protected Sprite _arrowRight;

		public string CurrentListItem { get; internal set; }

		public DynamicListItemChangeCallback Callback { get; set; }

		public UIMenuDynamicListItem(string text, string startingItem, DynamicListItemChangeCallback changeCallback)
			: this(text, null, startingItem, changeCallback)
		{
		}

		public UIMenuDynamicListItem(string text, string description, string startingItem, DynamicListItemChangeCallback changeCallback)
			: base(text, description)
		{
			_arrowLeft = new Sprite("commonmenu", "arrowleft", new Point(110, 105), new Size(30, 30));
			_arrowRight = new Sprite("commonmenu", "arrowright", new Point(280, 105), new Size(30, 30));
			_itemText = new UIResText("", new Point(290, 104), 0.35f, Color.White, GTA.UI.Font.ChaletLondon, UIResText.Alignment.Right);
			CurrentListItem = startingItem;
			Callback = changeCallback;
		}

		public override void Position(int y)
		{
			_arrowLeft.Position = new Point(300 + base.Offset.X + base.Parent.WidthOffset, 147 + y + base.Offset.Y);
			_arrowRight.Position = new Point(400 + base.Offset.X + base.Parent.WidthOffset, 147 + y + base.Offset.Y);
			_itemText.Position = new Point(300 + base.Offset.X + base.Parent.WidthOffset, y + 147 + base.Offset.Y);
			base.Position(y);
		}

		public override void Draw()
		{
			base.Draw();
			string currentListItem = CurrentListItem;
			float num = StringMeasurer.MeasureString(currentListItem, _itemText.Font, _itemText.Scale);
			_itemText.Color = ((!Enabled) ? Color.FromArgb(163, 159, 148) : (Selected ? Color.Black : Color.WhiteSmoke));
			_itemText.Caption = currentListItem;
			_arrowLeft.Color = ((!Enabled) ? Color.FromArgb(163, 159, 148) : (Selected ? Color.Black : Color.WhiteSmoke));
			_arrowRight.Color = ((!Enabled) ? Color.FromArgb(163, 159, 148) : (Selected ? Color.Black : Color.WhiteSmoke));
			_arrowLeft.Position = new Point(375 - (int)num + base.Offset.X + base.Parent.WidthOffset, _arrowLeft.Position.Y);
			if (Selected)
			{
				_arrowLeft.Draw();
				_arrowRight.Draw();
				_itemText.Position = new Point(403 + base.Offset.X + base.Parent.WidthOffset, _itemText.Position.Y);
			}
			else
			{
				_itemText.Position = new Point(418 + base.Offset.X + base.Parent.WidthOffset, _itemText.Position.Y);
			}
			_itemText.Draw();
		}

		public override void SetRightBadge(BadgeStyle badge)
		{
			throw new Exception("UIMenuListItem cannot have a right badge.");
		}

		public override void SetRightLabel(string text)
		{
			throw new Exception("UIMenuListItem cannot have a right label.");
		}

		public string CurrentItem()
		{
			return CurrentListItem;
		}
	}
}
