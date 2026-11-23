using System.Drawing;

namespace INMNativeUI.PauseMenu
{
	public class TabTextItem : TabItem
	{
		public string TextTitle { get; set; }

		public string Text { get; set; }

		public int WordWrap { get; set; }

		public TabTextItem(string name, string title)
			: base(name)
		{
			TextTitle = title;
		}

		public TabTextItem(string name, string title, string text)
			: base(name)
		{
			TextTitle = title;
			Text = text;
		}

		public override void Draw()
		{
			base.Draw();
			UIMenu.GetScreenResolutionMaintainRatio();
			int alpha = ((Focused || !base.CanBeFocused) ? 255 : 200);
			if (!string.IsNullOrEmpty(TextTitle))
			{
				new UIResText(TextTitle, base.SafeSize.AddPoints(new Point(40, 20)), 1.5f, Color.FromArgb(alpha, Color.White)).Draw();
			}
			if (!string.IsNullOrEmpty(Text))
			{
				int width = ((WordWrap == 0) ? (base.BottomRight.X - base.TopLeft.X - 40) : WordWrap);
				UIResText uIResText = new UIResText(Text, base.SafeSize.AddPoints(new Point(40, 150)), 0.4f, Color.FromArgb(alpha, Color.White));
				uIResText.WordWrap = new Size(width, 0);
				uIResText.Draw();
			}
		}
	}
}
