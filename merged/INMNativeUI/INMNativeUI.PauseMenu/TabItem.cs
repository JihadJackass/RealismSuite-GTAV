using System;
using System.Drawing;

namespace INMNativeUI.PauseMenu
{
	public class TabItem
	{
		public bool DrawBg;

		protected Sprite RockstarTile;

		public virtual bool Visible { get; set; }

		public virtual bool Focused { get; set; }

		public string Title { get; set; }

		public bool Active { get; set; }

		public bool JustOpened { get; set; }

		public bool CanBeFocused { get; set; }

		public Point TopLeft { get; set; }

		public Point BottomRight { get; set; }

		public Point SafeSize { get; set; }

		public bool UseDynamicPositionment { get; set; }

		public TabView Parent { get; set; }

		public bool FadeInWhenFocused { get; set; }

		public event EventHandler Activated;

		public event EventHandler DrawInstructionalButtons;

		public TabItem(string name)
		{
			RockstarTile = new Sprite("pause_menu_sp_content", "rockstartilebmp", default(Point), new Size(64, 64), 0f, Color.FromArgb(40, 255, 255, 255));
			Title = name;
			DrawBg = true;
			UseDynamicPositionment = true;
		}

		public void OnActivated()
		{
			this.Activated?.Invoke(this, EventArgs.Empty);
		}

		public virtual void ProcessControls()
		{
		}

		public virtual void Draw()
		{
			if (!Visible)
			{
				return;
			}
			SizeF screenResolutionMaintainRatio = UIMenu.GetScreenResolutionMaintainRatio();
			if (UseDynamicPositionment)
			{
				SafeSize = new Point(300, 240);
				TopLeft = new Point(SafeSize.X, SafeSize.Y);
				BottomRight = new Point((int)screenResolutionMaintainRatio.Width - SafeSize.X, (int)screenResolutionMaintainRatio.Height - SafeSize.Y);
			}
			Size size = new Size(BottomRight.SubtractPoints(TopLeft));
			this.DrawInstructionalButtons?.Invoke(this, EventArgs.Empty);
			if (DrawBg)
			{
				new UIResRectangle(TopLeft, size, Color.FromArgb((Focused || !FadeInWhenFocused) ? 200 : 120, 0, 0, 0)).Draw();
				int num = 100;
				RockstarTile.Size = new Size(num, num);
				int num2 = size.Width / num;
				int num3 = 4;
				for (int i = 0; i < num2 * num3; i++)
				{
					RockstarTile.Position = TopLeft.AddPoints(new Point(num * (i % num2), num * (i / num2)));
					RockstarTile.Color = Color.FromArgb((int)MiscExtensions.LinearFloatLerp(40f, 0f, i / num2, num3), 255, 255, 255);
					RockstarTile.Draw();
				}
			}
		}
	}
}
