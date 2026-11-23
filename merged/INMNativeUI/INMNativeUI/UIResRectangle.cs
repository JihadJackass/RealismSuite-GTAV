using System.Drawing;
using GTA.Native;
using GTA.UI;

namespace INMNativeUI
{
	public class UIResRectangle : IUIElement
	{
		public bool Enabled { get; set; } = true;


		public Color Color { get; set; } = Color.Transparent;


		public Point Position { get; set; } = Point.Empty;


		public Size Size { get; set; } = new Size(1280, 720);


		public UIResRectangle()
		{
		}

		public UIResRectangle(Point pos, Size size)
		{
			Position = pos;
			Size = size;
		}

		public UIResRectangle(Point pos, Size size, Color color)
		{
			Position = pos;
			Size = size;
			Color = color;
		}

		public void Draw()
		{
			Draw(new Size(0, 0));
		}

		public void Draw(Size offset)
		{
			if (Enabled)
			{
				int width = Screen.Resolution.Width;
				int height = Screen.Resolution.Height;
				float num = (float)width / (float)height;
				float num2 = 1080f * num;
				float num3 = (float)Size.Width / num2;
				float num4 = (float)Size.Height / 1080f;
				float num5 = (float)(Position.X + offset.Width) / num2 + num3 * 0.5f;
				float num6 = (float)(Position.Y + offset.Height) / 1080f + num4 * 0.5f;
				Function.Call(Hash.DRAW_RECT, num5, num6, num3, num4, Color.R, Color.G, Color.B, Color.A);
			}
		}

		public static void Draw(int xPos, int yPos, int boxWidth, int boxHeight, Color color)
		{
			int width = Screen.Resolution.Width;
			int height = Screen.Resolution.Height;
			float num = (float)width / (float)height;
			float num2 = 1080f * num;
			float num3 = (float)boxWidth / num2;
			float num4 = (float)boxHeight / 1080f;
			float num5 = (float)xPos / num2 + num3 * 0.5f;
			float num6 = (float)yPos / 1080f + num4 * 0.5f;
			Function.Call(Hash.DRAW_RECT, num5, num6, num3, num4, color.R, color.G, color.B, color.A);
		}
	}
}
