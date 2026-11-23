using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GTA.UI;

namespace INMNativeUI.PauseMenu
{
	public class TabItemSimpleList : TabItem
	{
		public Dictionary<string, string> Dictionary { get; set; }

		public TabItemSimpleList(string title, Dictionary<string, string> dict)
			: base(title)
		{
			Dictionary = dict;
			DrawBg = false;
		}

		public override void Draw()
		{
			base.Draw();
			UIMenu.GetScreenResolutionMaintainRatio();
			int alpha = ((Focused || !base.CanBeFocused) ? 180 : 60);
			int alpha2 = ((Focused || !base.CanBeFocused) ? 200 : 90);
			int alpha3 = ((Focused || !base.CanBeFocused) ? 255 : 150);
			int width = base.BottomRight.X - base.TopLeft.X;
			for (int i = 0; i < Dictionary.Count; i++)
			{
				new UIResRectangle(new Point(base.TopLeft.X, base.TopLeft.Y + 40 * i), new Size(width, 40), (i % 2 == 0) ? Color.FromArgb(alpha, 0, 0, 0) : Color.FromArgb(alpha2, 0, 0, 0)).Draw();
				KeyValuePair<string, string> keyValuePair = Dictionary.ElementAt(i);
				new UIResText(keyValuePair.Key, new Point(base.TopLeft.X + 6, base.TopLeft.Y + 5 + 40 * i), 0.35f, Color.FromArgb(alpha3, Color.White)).Draw();
				new UIResText(keyValuePair.Value, new Point(base.BottomRight.X - 6, base.TopLeft.Y + 5 + 40 * i), 0.35f, Color.FromArgb(alpha3, Color.White), GTA.UI.Font.ChaletLondon, UIResText.Alignment.Right).Draw();
			}
		}
	}
}
