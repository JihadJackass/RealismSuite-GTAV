using System;
using System.Collections.Generic;
using System.Drawing;
using GTA;
using GTA.Native;
using GTA.UI;

namespace INMNativeUI.PauseMenu
{
	public class TabMissionSelectItem : TabItem
	{
		protected const int MaxItemsPerView = 15;

		protected int _minItem;

		protected int _maxItem;

		public List<MissionInformation> Heists { get; set; }

		public int Index { get; set; }

		protected Sprite _noLogo { get; set; }

		public event OnItemSelect OnItemSelect;

		public TabMissionSelectItem(string name, IEnumerable<MissionInformation> list)
			: base(name)
		{
			base.FadeInWhenFocused = true;
			DrawBg = false;
			_noLogo = new Sprite("gtav_online", "rockstarlogo256", default(Point), new Size(512, 256));
			_maxItem = 15;
			_minItem = 0;
			base.CanBeFocused = true;
			Heists = new List<MissionInformation>(list);
		}

		public override void ProcessControls()
		{
			if (!Focused || Heists.Count == 0)
			{
				return;
			}
			if (base.JustOpened)
			{
				base.JustOpened = false;
				return;
			}
			if (Game.IsControlJustPressed(Control.PhoneSelect))
			{
				Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
				this.OnItemSelect?.Invoke(Heists[Index]);
			}
			if (Game.IsControlJustPressed(Control.FrontendUp) || Game.IsControlJustPressed(Control.MoveUpOnly))
			{
				Index = (1000 - 1000 % Heists.Count + Index - 1) % Heists.Count;
				Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
				if (Heists.Count > 15)
				{
					if (Index < _minItem)
					{
						_minItem--;
						_maxItem--;
					}
					if (Index == Heists.Count - 1)
					{
						_minItem = Heists.Count - 15;
						_maxItem = Heists.Count;
					}
				}
			}
			else
			{
				if (!Game.IsControlJustPressed(Control.FrontendDown) && !Game.IsControlJustPressed(Control.MoveDownOnly))
				{
					return;
				}
				Index = (1000 - 1000 % Heists.Count + Index + 1) % Heists.Count;
				Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
				if (Heists.Count > 15)
				{
					if (Index >= _maxItem)
					{
						_maxItem++;
						_minItem++;
					}
					if (Index == 0)
					{
						_minItem = 0;
						_maxItem = 15;
					}
				}
			}
		}

		public override void Draw()
		{
			base.Draw();
			if (Heists.Count != 0)
			{
				SizeF screenResolutionMaintainRatio = UIMenu.GetScreenResolutionMaintainRatio();
				float num = screenResolutionMaintainRatio.Width - (float)(base.SafeSize.X * 2);
				Size size = new Size((int)num - 515, 40);
				int alpha = (Focused ? 120 : 30);
				int alpha2 = (Focused ? 200 : 100);
				int alpha3 = (Focused ? 255 : 150);
				int num2 = 0;
				for (int i = _minItem; i < Math.Min(Heists.Count, _maxItem); i++)
				{
					new UIResRectangle(base.SafeSize.AddPoints(new Point(0, (size.Height + 3) * num2)), size, (Index == i && Focused) ? Color.FromArgb(alpha3, Color.White) : Color.FromArgb(alpha2, Color.Black)).Draw();
					new UIResText(Heists[i].Name, base.SafeSize.AddPoints(new Point(6, 5 + (size.Height + 3) * num2)), 0.35f, Color.FromArgb(alpha3, (Index == i && Focused) ? Color.Black : Color.White)).Draw();
					num2++;
				}
				if (Heists[Index].Logo == null || string.IsNullOrEmpty(Heists[Index].Logo.FileName))
				{
					_noLogo.Position = new Point((int)screenResolutionMaintainRatio.Width - base.SafeSize.X - 512, base.SafeSize.Y);
					_noLogo.Color = Color.FromArgb(alpha2, 0, 0, 0);
					_noLogo.Draw();
				}
				else if (Heists[Index].Logo != null && Heists[Index].Logo.FileName != null && !Heists[Index].Logo.IsGameTexture)
				{
					Sprite.DrawTexture(Heists[Index].Logo.FileName, new Point((int)screenResolutionMaintainRatio.Width - base.SafeSize.X - 512, base.SafeSize.Y), new Size(512, 256));
				}
				else if (Heists[Index].Logo != null && Heists[Index].Logo.FileName != null && Heists[Index].Logo.IsGameTexture)
				{
					Sprite sprite = new Sprite(Heists[Index].Logo.DictionaryName, Heists[Index].Logo.FileName, default(Point), new Size(512, 256));
					sprite.Position = new Point((int)screenResolutionMaintainRatio.Width - base.SafeSize.X - 512, base.SafeSize.Y);
					sprite.Color = Color.FromArgb(alpha2, 0, 0, 0);
					sprite.Draw();
				}
				new UIResRectangle(new Point((int)screenResolutionMaintainRatio.Width - base.SafeSize.X - 512, base.SafeSize.Y + 256), new Size(512, 40), Color.FromArgb(alpha3, Color.Black)).Draw();
				new UIResText(Heists[Index].Name, new Point((int)screenResolutionMaintainRatio.Width - base.SafeSize.X - 4, base.SafeSize.Y + 260), 0.5f, Color.FromArgb(alpha3, Color.White), GTA.UI.Font.HouseScript, UIResText.Alignment.Right).Draw();
				for (int j = 0; j < Heists[Index].ValueList.Count; j++)
				{
					new UIResRectangle(new Point((int)screenResolutionMaintainRatio.Width - base.SafeSize.X - 512, base.SafeSize.Y + 256 + 40 + 40 * j), new Size(512, 40), (j % 2 == 0) ? Color.FromArgb(alpha, 0, 0, 0) : Color.FromArgb(alpha2, 0, 0, 0)).Draw();
					string item = Heists[Index].ValueList[j].Item1;
					string item2 = Heists[Index].ValueList[j].Item2;
					new UIResText(item, new Point((int)screenResolutionMaintainRatio.Width - base.SafeSize.X - 506, base.SafeSize.Y + 260 + 42 + 40 * j), 0.35f, Color.FromArgb(alpha3, Color.White)).Draw();
					new UIResText(item2, new Point((int)screenResolutionMaintainRatio.Width - base.SafeSize.X - 6, base.SafeSize.Y + 260 + 42 + 40 * j), 0.35f, Color.FromArgb(alpha3, Color.White), GTA.UI.Font.ChaletLondon, UIResText.Alignment.Right).Draw();
				}
				if (!string.IsNullOrEmpty(Heists[Index].Description))
				{
					int count = Heists[Index].ValueList.Count;
					new UIResRectangle(new Point((int)screenResolutionMaintainRatio.Width - base.SafeSize.X - 512, base.SafeSize.Y + 256 + 42 + 40 * count), new Size(512, 2), Color.FromArgb(alpha3, Color.White)).Draw();
					UIResText uIResText = new UIResText(Heists[Index].Description, new Point((int)screenResolutionMaintainRatio.Width - base.SafeSize.X - 508, base.SafeSize.Y + 256 + 45 + 40 * count + 4), 0.35f, Color.FromArgb(alpha3, Color.White));
					uIResText.WordWrap = new Size(508, 0);
					uIResText.Draw();
					new UIResRectangle(new Point((int)screenResolutionMaintainRatio.Width - base.SafeSize.X - 512, base.SafeSize.Y + 256 + 44 + 40 * count), new Size(512, 45 * (int)(StringMeasurer.MeasureString(Heists[Index].Description, GTA.UI.Font.ChaletLondon, 0.35f) / 500f)), Color.FromArgb(alpha2, 0, 0, 0)).Draw();
				}
			}
		}
	}
}
