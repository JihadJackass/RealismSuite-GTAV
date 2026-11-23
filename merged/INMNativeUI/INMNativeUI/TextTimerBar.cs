using System.Drawing;
using GTA.UI;

namespace INMNativeUI
{
	public class TextTimerBar : TimerBarBase
	{
		public string Text { get; set; }

		public TextTimerBar(string label, string text)
			: base(label)
		{
			Text = text;
		}

		public override void Draw(int interval)
		{
			SizeF screenResolutionMaintainRatio = UIMenu.GetScreenResolutionMaintainRatio();
			Point safezoneBounds = UIMenu.GetSafezoneBounds();
			base.Draw(interval);
			UIResText.Draw(Text, (int)screenResolutionMaintainRatio.Width - safezoneBounds.X - 10, (int)screenResolutionMaintainRatio.Height - safezoneBounds.Y - (42 + 4 * interval), GTA.UI.Font.ChaletLondon, 0.5f, Color.White, UIResText.Alignment.Right, false, false, 0);
		}
	}
}
