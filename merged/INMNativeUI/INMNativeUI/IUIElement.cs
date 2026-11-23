using System.Drawing;

namespace INMNativeUI
{
	public interface IUIElement
	{
		bool Enabled { get; set; }

		Point Position { get; set; }

		Color Color { get; set; }

		void Draw();

		void Draw(Size offset);
	}
}
