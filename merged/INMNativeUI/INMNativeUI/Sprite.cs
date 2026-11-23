using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using GTA.Native;
using GTA.UI;

namespace INMNativeUI
{
	public class Sprite
	{
		public Point Position;

		public Size Size;

		public Color Color;

		public bool Visible;

		public float Heading;

		public string TextureName;

		private string _textureDict;

		public string TextureDict
		{
			get
			{
				return _textureDict;
			}
			set
			{
				_textureDict = value;
			}
		}

		public Sprite(string textureDict, string textureName, Point position, Size size, float heading, Color color)
		{
			TextureDict = textureDict;
			TextureName = textureName;
			Position = position;
			Size = size;
			Heading = heading;
			Color = color;
			Visible = true;
		}

		public Sprite(string textureDict, string textureName, Point position, Size size)
			: this(textureDict, textureName, position, size, 0f, Color.FromArgb(255, 255, 255, 255))
		{
		}

		public void Draw()
		{
			if (Visible)
			{
				if (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, TextureDict))
				{
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, TextureDict, true);
				}
				int width = Screen.Resolution.Width;
				int height = Screen.Resolution.Height;
				float num = (float)width / (float)height;
				float num2 = 1080f * num;
				float num3 = (float)Size.Width / num2;
				float num4 = (float)Size.Height / 1080f;
				float num5 = (float)Position.X / num2 + num3 * 0.5f;
				float num6 = (float)Position.Y / 1080f + num4 * 0.5f;
				Function.Call(Hash.DRAW_SPRITE, TextureDict, TextureName, num5, num6, num3, num4, Heading, Color.R, Color.G, Color.B, Color.A);
			}
		}

		public static void Draw(string dict, string name, int xpos, int ypos, int boxWidth, int boxHeight, float rotation, Color color)
		{
			if (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, dict))
			{
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, dict, true);
			}
			int width = Screen.Resolution.Width;
			int height = Screen.Resolution.Height;
			float num = (float)width / (float)height;
			float num2 = 1080f * num;
			float num3 = (float)boxWidth / num2;
			float num4 = (float)boxHeight / 1080f;
			float num5 = (float)xpos / num2 + num3 * 0.5f;
			float num6 = (float)ypos / 1080f + num4 * 0.5f;
			Function.Call(Hash.DRAW_SPRITE, dict, name, num5, num6, num3, num4, rotation, color.R, color.G, color.B, color.A);
		}

		public static void DrawTexture(string path, Point position, Size size, float rotation, Color color)
		{
			int width = Screen.Resolution.Width;
			int height = Screen.Resolution.Height;
			float num = (float)width / (float)height;
			float num2 = 1080f * num;
			float num3 = 1280f / num2;
			float num4 = 2f / 3f;
			new CustomSprite(path, new SizeF((float)size.Width * num3, (float)size.Height * num4), new PointF((float)position.X * num3, (float)position.Y * num4), color, rotation).Draw();
		}

		public static void DrawTexture(string path, Point position, Size size)
		{
			DrawTexture(path, position, size, 0f, Color.White);
		}

		public static string WriteFileFromResources(Assembly yourAssembly, string fullResourceName)
		{
			string tempFileName = Path.GetTempFileName();
			return WriteFileFromResources(yourAssembly, fullResourceName, tempFileName);
		}

		public static string WriteFileFromResources(Assembly yourAssembly, string fullResourceName, string savePath)
		{
			using (Stream stream = yourAssembly.GetManifestResourceStream(fullResourceName))
			{
				if (stream != null)
				{
					byte[] buffer = new byte[stream.Length];
					stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
					using (FileStream fileStream = File.Create(savePath))
					{
						fileStream.Write(buffer, 0, Convert.ToInt32(stream.Length));
						fileStream.Close();
					}
				}
			}
			return Path.GetFullPath(savePath);
		}
	}
}
