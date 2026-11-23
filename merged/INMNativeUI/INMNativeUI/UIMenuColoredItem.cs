using System.Drawing;
using GTA.UI;

namespace INMNativeUI
{
	public class UIMenuColoredItem : UIMenuItem
	{
		public Color MainColor { get; set; }

		public Color HighlightColor { get; set; }

		public Color TextColor { get; set; }

		public Color HighlightedTextColor { get; set; }

		public UIMenuColoredItem(string label, Color color, Color highlightColor)
			: base(label)
		{
			MainColor = color;
			HighlightColor = highlightColor;
			TextColor = Color.White;
			HighlightedTextColor = Color.Black;
			Init();
		}

		public UIMenuColoredItem(string label, string description, Color color, Color highlightColor)
			: base(label, description)
		{
			MainColor = color;
			HighlightColor = highlightColor;
			TextColor = Color.White;
			HighlightedTextColor = Color.Black;
			Init();
		}

		protected void Init()
		{
			_selectedSprite = new Sprite("commonmenu", "gradient_nav", new Point(0, 0), new Size(431, 38), 0f, HighlightColor);
			_rectangle = new UIResRectangle(new Point(0, 0), new Size(431, 38), Color.FromArgb(150, 0, 0, 0));
			_text = new UIResText(base.Text, new Point(8, 0), 0.33f, Color.WhiteSmoke, GTA.UI.Font.ChaletLondon, UIResText.Alignment.Left);
			Description = Description;
			_badgeLeft = new Sprite("commonmenu", "", new Point(0, 0), new Size(40, 40));
			_badgeRight = new Sprite("commonmenu", "", new Point(0, 0), new Size(40, 40));
			_labelText = new UIResText("", new Point(0, 0), 0.35f)
			{
				TextAlignment = UIResText.Alignment.Right
			};
		}

		public override void Draw()
		{
			_rectangle.Size = new Size(431 + base.Parent.WidthOffset, 38);
			_selectedSprite.Size = new Size(431 + base.Parent.WidthOffset, 38);
			if (Hovered && !Selected)
			{
				_rectangle.Color = Color.FromArgb(20, 255, 255, 255);
				_rectangle.Draw();
			}
			if (Selected)
			{
				_selectedSprite.Color = HighlightColor;
				_selectedSprite.Draw();
			}
			else
			{
				_selectedSprite.Color = MainColor;
				_selectedSprite.Draw();
			}
			_text.Color = ((!Enabled) ? Color.FromArgb(163, 159, 148) : (Selected ? HighlightedTextColor : TextColor));
			if (LeftBadge != 0)
			{
				_text.Position = new Point(35 + base.Offset.X, _text.Position.Y);
				_badgeLeft.TextureDict = UIMenuItem.BadgeToSpriteLib(LeftBadge);
				_badgeLeft.TextureName = UIMenuItem.BadgeToSpriteName(LeftBadge, Selected);
				_badgeLeft.Color = UIMenuItem.BadgeToColor(LeftBadge, Selected);
				_badgeLeft.Draw();
			}
			else
			{
				_text.Position = new Point(8 + base.Offset.X, _text.Position.Y);
			}
			if (RightBadge != 0)
			{
				_badgeRight.Position = new Point(385 + base.Offset.X + base.Parent.WidthOffset, _badgeRight.Position.Y);
				_badgeRight.TextureDict = UIMenuItem.BadgeToSpriteLib(RightBadge);
				_badgeRight.TextureName = UIMenuItem.BadgeToSpriteName(RightBadge, Selected);
				_badgeRight.Color = UIMenuItem.BadgeToColor(RightBadge, Selected);
				_badgeRight.Draw();
			}
			if (!string.IsNullOrWhiteSpace(RightLabel))
			{
				_labelText.Position = new Point(420 + base.Offset.X + base.Parent.WidthOffset, _labelText.Position.Y);
				_labelText.Caption = RightLabel;
				Color color3 = (_labelText.Color = (_text.Color = ((!Enabled) ? Color.FromArgb(163, 159, 148) : (Selected ? HighlightedTextColor : TextColor))));
				_labelText.Draw();
			}
			_text.Draw();
		}
	}
}
