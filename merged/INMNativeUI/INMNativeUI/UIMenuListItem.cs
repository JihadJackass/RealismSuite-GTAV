using System;
using System.Collections.Generic;
using System.Drawing;
using GTA.UI;

namespace INMNativeUI
{
	public class UIMenuListItem : UIMenuItem, IListItem
	{
		protected UIResText _itemText;

		protected Sprite _arrowLeft;

		protected Sprite _arrowRight;

		protected int _index;

		protected List<object> _items;

		public int Index
		{
			get
			{
				if (!HasItems)
				{
					return 0;
				}
				int num = _index % _items.Count;
				if (num >= 0)
				{
					return num;
				}
				return num + _items.Count;
			}
			set
			{
				if (!HasItems)
				{
					_index = 0;
					return;
				}
				int num = value % _items.Count;
				if (num < 0)
				{
					num += _items.Count;
				}
				_index = 100000000 - 100000000 % _items.Count + num;
			}
		}

		internal bool HasItems
		{
			get
			{
				if (_items != null)
				{
					return _items.Count != 0;
				}
				return false;
			}
		}

		public event ItemListEvent OnListChanged;

		public UIMenuListItem(string text, List<object> items, int index)
			: this(text, items, index, "")
		{
		}

		public UIMenuListItem(string text, List<object> items, int index, string description)
			: base(text, description)
		{
			_items = items ?? new List<object>();
			_arrowLeft = new Sprite("commonmenu", "arrowleft", new Point(110, 105), new Size(30, 30));
			_arrowRight = new Sprite("commonmenu", "arrowright", new Point(280, 105), new Size(30, 30));
			_itemText = new UIResText("", new Point(290, 104), 0.35f, Color.White, GTA.UI.Font.ChaletLondon, UIResText.Alignment.Left)
			{
				TextAlignment = UIResText.Alignment.Right
			};
			Index = index;
		}

		public override void Position(int y)
		{
			_arrowLeft.Position = new Point(300 + base.Offset.X + base.Parent.WidthOffset, 147 + y + base.Offset.Y);
			_arrowRight.Position = new Point(400 + base.Offset.X + base.Parent.WidthOffset, 147 + y + base.Offset.Y);
			_itemText.Position = new Point(300 + base.Offset.X + base.Parent.WidthOffset, y + 147 + base.Offset.Y);
			base.Position(y);
		}

		public virtual int ItemToIndex(object item)
		{
			return _items.FindIndex((object p) => p == item);
		}

		public virtual object IndexToItem(int index)
		{
			return _items[index];
		}

		public override void Draw()
		{
			base.Draw();
			if (HasItems)
			{
				string text = _items[Index].ToString();
				float num = StringMeasurer.MeasureString(text, _itemText.Font, _itemText.Scale);
				_itemText.Color = ((!Enabled) ? Color.FromArgb(163, 159, 148) : (Selected ? Color.Black : Color.WhiteSmoke));
				_itemText.Caption = text;
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
		}

		internal virtual void ListChangedTrigger(int newindex)
		{
			this.OnListChanged?.Invoke(this, newindex);
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
			if (!HasItems)
			{
				return string.Empty;
			}
			return _items[Index].ToString();
		}
	}
}
