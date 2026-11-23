using System;
using System.Drawing;
using System.Text;
using GTA.Native;
using GTA.UI;

namespace INMNativeUI
{
	public class UIResText : IUIElement
	{
		public enum Alignment
		{
			Left,
			Centered,
			Right
		}

		public bool Enabled { get; set; } = true;


		public string Caption { get; set; }

		public Point Position { get; set; }

		public float Scale { get; set; }

		public Color Color { get; set; } = Color.WhiteSmoke;


		public GTA.UI.Font Font { get; set; }

		public bool Centered
		{
			get
			{
				return TextAlignment == Alignment.Centered;
			}
			set
			{
				TextAlignment = (value ? Alignment.Centered : Alignment.Left);
			}
		}

		public Alignment TextAlignment { get; set; }

		public bool DropShadow { get; set; }

		public bool Outline { get; set; }

		public Size WordWrap { get; set; }

		public UIResText(string caption, Point position, float scale)
		{
			Caption = caption;
			Position = position;
			Scale = scale;
			TextAlignment = Alignment.Left;
		}

		public UIResText(string caption, Point position, float scale, Color color)
		{
			Caption = caption;
			Position = position;
			Scale = scale;
			Color = color;
			TextAlignment = Alignment.Left;
		}

		public UIResText(string caption, Point position, float scale, Color color, GTA.UI.Font font, Alignment justify)
		{
			Caption = caption;
			Position = position;
			Scale = scale;
			Color = color;
			Font = font;
			TextAlignment = justify;
		}

		public void Draw()
		{
			Draw(new Size(0, 0));
		}

		public static void AddLongString(string str)
		{
			if (Encoding.UTF8.GetByteCount(str) == str.Length)
			{
				AddLongStringForAscii(str);
			}
			else
			{
				AddLongStringForUtf8(str);
			}
		}

		private static void AddLongStringForAscii(string input)
		{
			for (int i = 0; i < input.Length; i += 99)
			{
				string text = input.Substring(i, Math.Min(99, input.Length - i));
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			}
		}

		private static void AddLongStringForUtf8(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return;
			}
			Encoding uTF = Encoding.UTF8;
			if (uTF.GetByteCount(input) < 99)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, input);
				return;
			}
			int num = 0;
			for (int i = 0; i < input.Length; i++)
			{
				int num2 = i - num;
				if (uTF.GetByteCount(input.Substring(num, num2)) > 99)
				{
					string text = input.Substring(num, num2 - 1);
					Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
					i--;
					num = num + num2 - 1;
				}
			}
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, input.Substring(num, input.Length - num));
		}

		public static float MeasureStringWidth(string str, GTA.UI.Font font, float scale)
		{
			int num = 1440;
			float num2 = 2560f / (float)num;
			float num3 = 1080f * num2;
			return MeasureStringWidthNoConvert(str, font, scale) * num3;
		}

		public static float MeasureStringWidthNoConvert(string str, GTA.UI.Font font, float scale)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_GET_SCREEN_WIDTH_OF_DISPLAY_TEXT, "STRING");
			AddLongString(str);
			return Function.Call<float>(Hash.END_TEXT_COMMAND_GET_SCREEN_WIDTH_OF_DISPLAY_TEXT, (int)font) * scale;
		}

		public void Draw(Size offset)
		{
			int width = Screen.Resolution.Width;
			int height = Screen.Resolution.Height;
			float num = (float)width / (float)height;
			float num2 = 1080f * num;
			float num3 = (float)Position.X / num2;
			float num4 = (float)Position.Y / 1080f;
			Function.Call(Hash.SET_TEXT_FONT, (int)Font);
			Function.Call(Hash.SET_TEXT_SCALE, 1f, Scale);
			Function.Call(Hash.SET_TEXT_COLOUR, Color.R, Color.G, Color.B, Color.A);
			if (DropShadow)
			{
				Function.Call(Hash.SET_TEXT_DROP_SHADOW);
			}
			if (Outline)
			{
				Function.Call(Hash.SET_TEXT_OUTLINE);
			}
			switch (TextAlignment)
			{
			case Alignment.Centered:
				Function.Call(Hash.SET_TEXT_CENTRE, true);
				break;
			case Alignment.Right:
				Function.Call(Hash.SET_TEXT_RIGHT_JUSTIFY, true);
				Function.Call(Hash.SET_TEXT_WRAP, 0, num3);
				break;
			}
			if (WordWrap.Width != 0)
			{
				float num5 = (float)(Position.X + WordWrap.Width) / num2;
				Function.Call(Hash.SET_TEXT_WRAP, num3, num5);
			}
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "jamyfafi");
			AddLongString(Caption);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, num3, num4);
		}

		public static void Draw(string caption, int xPos, int yPos, GTA.UI.Font font, float scale, Color color, Alignment alignment, bool dropShadow, bool outline, int wordWrap)
		{
			int width = Screen.Resolution.Width;
			int height = Screen.Resolution.Height;
			float num = (float)width / (float)height;
			float num2 = 1080f * num;
			float num3 = (float)xPos / num2;
			float num4 = (float)yPos / 1080f;
			Function.Call(Hash.SET_TEXT_FONT, (int)font);
			Function.Call(Hash.SET_TEXT_SCALE, 1f, scale);
			Function.Call(Hash.SET_TEXT_COLOUR, color.R, color.G, color.B, color.A);
			if (dropShadow)
			{
				Function.Call(Hash.SET_TEXT_DROP_SHADOW);
			}
			if (outline)
			{
				Function.Call(Hash.SET_TEXT_OUTLINE);
			}
			switch (alignment)
			{
			case Alignment.Centered:
				Function.Call(Hash.SET_TEXT_CENTRE, true);
				break;
			case Alignment.Right:
				Function.Call(Hash.SET_TEXT_RIGHT_JUSTIFY, true);
				Function.Call(Hash.SET_TEXT_WRAP, 0, num3);
				break;
			}
			if (wordWrap != 0)
			{
				float num5 = (float)(xPos + wordWrap) / num2;
				Function.Call(Hash.SET_TEXT_WRAP, num3, num5);
			}
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "jamyfafi");
			AddLongString(caption);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, num3, num4);
		}
	}
}
