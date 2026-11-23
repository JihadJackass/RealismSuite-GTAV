using System;
using System.Drawing;

namespace INMNativeUI
{
	public class UIMenuCheckboxItem : UIMenuItem
	{
		protected Sprite _checkedSprite;

		public bool Checked { get; set; }

		public event ItemCheckboxEvent CheckboxEvent;

		public UIMenuCheckboxItem(string text, bool check)
			: this(text, check, "")
		{
		}

		public UIMenuCheckboxItem(string text, bool check, string description)
			: base(text, description)
		{
			_checkedSprite = new Sprite("commonmenu", "shop_box_blank", new Point(410, 95), new Size(50, 50));
			Checked = check;
		}

		public override void Position(int y)
		{
			base.Position(y);
			_checkedSprite.Position = new Point(380 + base.Offset.X + base.Parent.WidthOffset, y + 138 + base.Offset.Y);
		}

		public override void Draw()
		{
			base.Draw();
			_checkedSprite.Position = new Point(380 + base.Offset.X + base.Parent.WidthOffset, _checkedSprite.Position.Y);
			if (Selected)
			{
				_checkedSprite.TextureName = (Checked ? "shop_box_tickb" : "shop_box_blankb");
			}
			else
			{
				_checkedSprite.TextureName = (Checked ? "shop_box_tick" : "shop_box_blank");
			}
			_checkedSprite.Draw();
		}

		public void CheckboxEventTrigger()
		{
			this.CheckboxEvent?.Invoke(this, Checked);
		}

		public override void SetRightBadge(BadgeStyle badge)
		{
			throw new Exception("UIMenuCheckboxItem cannot have a right badge.");
		}

		public override void SetRightLabel(string text)
		{
			throw new Exception("UIMenuListItem cannot have a right label.");
		}
	}
}
