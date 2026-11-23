using System.Collections.Generic;
using System.Drawing;

namespace INMNativeUI
{
	public class UIContainer : IUIElement
	{
		public bool Enabled { get; set; } = true;


		public Point Position { get; set; }

		public Size Size { get; set; }

		public Color Color { get; set; }

		public List<IUIElement> Items { get; } = new List<IUIElement>();


		public UIContainer(Point position, Size size, Color color)
		{
			Position = position;
			Size = size;
			Color = color;
		}

		public void Draw()
		{
			Draw(new Size(0, 0));
		}

		public void Draw(Size offset)
		{
			if (!Enabled)
			{
				return;
			}
			if (Color.A > 0)
			{
				UIResRectangle.Draw(Position.X + offset.Width, Position.Y + offset.Height, Size.Width, Size.Height, Color);
			}
			foreach (IUIElement item in Items)
			{
				item.Draw(new Size(Position.X + offset.Width, Position.Y + offset.Height));
			}
		}
	}
}
