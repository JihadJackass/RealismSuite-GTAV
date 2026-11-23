using System.Drawing;
using GTA.UI;

namespace INMNativeUI
{
	public abstract class TimerBarBase
	{
		public string Label { get; set; }

		public TimerBarBase(string label)
		{
			Label = label;
		}

		public virtual void Draw(int interval)
		{
			SizeF screenResolutionMaintainRatio = UIMenu.GetScreenResolutionMaintainRatio();
			Point safezoneBounds = UIMenu.GetSafezoneBounds();
			UIResText.Draw(Label, (int)screenResolutionMaintainRatio.Width - safezoneBounds.X - 180, (int)screenResolutionMaintainRatio.Height - safezoneBounds.Y - (30 + 4 * interval), GTA.UI.Font.ChaletLondon, 0.3f, Color.White, UIResText.Alignment.Right, false, false, 0);
			Sprite.Draw("timerbars", "all_black_bg", (int)screenResolutionMaintainRatio.Width - safezoneBounds.X - 298, (int)screenResolutionMaintainRatio.Height - safezoneBounds.Y - (40 + 4 * interval), 300, 37, 0f, Color.FromArgb(180, 255, 255, 255));
			Hud.HideComponentThisFrame(HudComponent.AreaName);
			Hud.HideComponentThisFrame(HudComponent.StreetName);
			Hud.HideComponentThisFrame(HudComponent.VehicleName);
		}
	}
}
