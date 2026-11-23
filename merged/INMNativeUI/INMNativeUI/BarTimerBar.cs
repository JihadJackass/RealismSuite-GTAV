using System.Drawing;

namespace INMNativeUI
{
	public class BarTimerBar : TimerBarBase
	{
		public float Percentage { get; set; }

		public Color BackgroundColor { get; set; }

		public Color ForegroundColor { get; set; }

		public BarTimerBar(string label)
			: base(label)
		{
			BackgroundColor = Color.DarkRed;
			ForegroundColor = Color.Red;
		}

		public override void Draw(int interval)
		{
			SizeF screenResolutionMaintainRatio = UIMenu.GetScreenResolutionMaintainRatio();
			Point safezoneBounds = UIMenu.GetSafezoneBounds();
			base.Draw(interval);
			Point point = new Point((int)screenResolutionMaintainRatio.Width - safezoneBounds.X - 160, (int)screenResolutionMaintainRatio.Height - safezoneBounds.Y - (28 + 4 * interval));
			UIResRectangle.Draw(point.X, point.Y, 150, 15, BackgroundColor);
			UIResRectangle.Draw(point.X, point.Y, (int)(150f * Percentage), 15, ForegroundColor);
		}
	}
}
