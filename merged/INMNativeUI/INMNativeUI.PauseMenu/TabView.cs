using System;
using System.Collections.Generic;
using System.Drawing;
using GTA;
using GTA.Native;
using GTA.UI;

namespace INMNativeUI.PauseMenu
{
	public class TabView
	{
		internal static readonly string _browseTextLocalized = Game.GetLocalizedString("HUD_INPUT1C");

		public int Index;

		private bool _visible;

		private Scaleform _sc;

		public string Title { get; set; }

		public Sprite Photo { get; set; }

		public string Name { get; set; }

		public string Money { get; set; }

		public string MoneySubtitle { get; set; }

		public List<TabItem> Tabs { get; set; }

		public int FocusLevel { get; set; }

		public bool TemporarilyHidden { get; set; }

		public bool CanLeave { get; set; }

		public bool HideTabs { get; set; }

		public bool Visible
		{
			get
			{
				return _visible;
			}
			set
			{
				_visible = value;
				if (value)
				{
					Function.Call(Hash.ANIMPOSTFX_PLAY, "MinigameTransitionIn", 0, true);
				}
				else
				{
					Function.Call(Hash.ANIMPOSTFX_STOP, "MinigameTransitionIn");
				}
			}
		}

		public event EventHandler OnMenuClose;

		public TabView(string title)
		{
			Title = title;
			Tabs = new List<TabItem>();
			Index = 0;
			Name = Game.Player.Name;
			TemporarilyHidden = false;
			CanLeave = true;
		}

		public void AddTab(TabItem item)
		{
			Tabs.Add(item);
			item.Parent = this;
		}

		public void ShowInstructionalButtons()
		{
			if (_sc == null)
			{
				_sc = new Scaleform("instructional_buttons");
			}
			_sc.CallFunction("CLEAR_ALL");
			_sc.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			_sc.CallFunction("CREATE_CONTAINER");
			_sc.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 176, 0), UIMenu._selectTextLocalized);
			_sc.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 177, 0), UIMenu._backTextLocalized);
			_sc.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 206, 0), "");
			_sc.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 205, 0), _browseTextLocalized);
		}

		public void DrawInstructionalButton(int slot, Control control, string text)
		{
			_sc.CallFunction("SET_DATA_SLOT", slot, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, (int)control, 0), text);
		}

		public void ProcessControls()
		{
			if (!Visible || TemporarilyHidden)
			{
				return;
			}
			Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, 0);
			if (Game.IsControlJustPressed(Control.PhoneLeft) && FocusLevel == 0)
			{
				Tabs[Index].Active = false;
				Tabs[Index].Focused = false;
				Tabs[Index].Visible = false;
				Index = (1000 - 1000 % Tabs.Count + Index - 1) % Tabs.Count;
				Tabs[Index].Active = true;
				Tabs[Index].Focused = false;
				Tabs[Index].Visible = true;
				Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
			}
			else if (Game.IsControlJustPressed(Control.PhoneRight) && FocusLevel == 0)
			{
				Tabs[Index].Active = false;
				Tabs[Index].Focused = false;
				Tabs[Index].Visible = false;
				Index = (1000 - 1000 % Tabs.Count + Index + 1) % Tabs.Count;
				Tabs[Index].Active = true;
				Tabs[Index].Focused = false;
				Tabs[Index].Visible = true;
				Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
			}
			else if (Game.IsControlJustPressed(Control.FrontendAccept) && FocusLevel == 0)
			{
				if (Tabs[Index].CanBeFocused)
				{
					Tabs[Index].Focused = true;
					Tabs[Index].JustOpened = true;
					FocusLevel = 1;
				}
				else
				{
					Tabs[Index].JustOpened = true;
					Tabs[Index].OnActivated();
				}
				Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
			}
			else if (Game.IsControlJustPressed(Control.PhoneCancel) && FocusLevel == 1)
			{
				Tabs[Index].Focused = false;
				FocusLevel = 0;
				Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "BACK", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
			}
			else if (Game.IsControlJustPressed(Control.PhoneCancel) && FocusLevel == 0 && CanLeave)
			{
				Visible = false;
				Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "BACK", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
				this.OnMenuClose?.Invoke(this, EventArgs.Empty);
			}
			if (!HideTabs)
			{
				if (Game.IsControlJustPressed(Control.FrontendLb))
				{
					Tabs[Index].Active = false;
					Tabs[Index].Focused = false;
					Tabs[Index].Visible = false;
					Index = (1000 - 1000 % Tabs.Count + Index - 1) % Tabs.Count;
					Tabs[Index].Active = true;
					Tabs[Index].Focused = false;
					Tabs[Index].Visible = true;
					FocusLevel = 0;
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
				}
				else if (Game.IsControlJustPressed(Control.FrontendRb))
				{
					Tabs[Index].Active = false;
					Tabs[Index].Focused = false;
					Tabs[Index].Visible = false;
					Index = (1000 - 1000 % Tabs.Count + Index + 1) % Tabs.Count;
					Tabs[Index].Active = true;
					Tabs[Index].Focused = false;
					Tabs[Index].Visible = true;
					FocusLevel = 0;
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
				}
			}
			if (Tabs.Count > 0)
			{
				Tabs[Index].ProcessControls();
			}
		}

		public void RefreshIndex()
		{
			foreach (TabItem tab in Tabs)
			{
				tab.Focused = false;
				tab.Active = false;
				tab.Visible = false;
			}
			Index = (1000 - 1000 % Tabs.Count) % Tabs.Count;
			Tabs[Index].Active = true;
			Tabs[Index].Focused = false;
			Tabs[Index].Visible = true;
			FocusLevel = 0;
		}

		public void Update()
		{
			if (!Visible || TemporarilyHidden)
			{
				return;
			}
			ShowInstructionalButtons();
			Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
			Function.Call(Hash.SET_MOUSE_CURSOR_THIS_FRAME);
			SizeF screenResolutionMaintainRatio = UIMenu.GetScreenResolutionMaintainRatio();
			Point left = new Point(300, 180);
			if (!HideTabs)
			{
				UIResText uIResText = new UIResText(Title, new Point(left.X, left.Y - 80), 1f, Color.White, GTA.UI.Font.ChaletComprimeCologne, UIResText.Alignment.Left);
				uIResText.DropShadow = true;
				uIResText.Draw();
				if (Photo == null)
				{
					new Sprite("char_multiplayer", "char_multiplayer", new Point((int)screenResolutionMaintainRatio.Width - left.X - 64, left.Y - 80), new Size(64, 64)).Draw();
				}
				else
				{
					Photo.Position = new Point((int)screenResolutionMaintainRatio.Width - left.X - 100, left.Y - 80);
					Photo.Size = new Size(64, 64);
					Photo.Draw();
				}
				UIResText uIResText2 = new UIResText(Name, new Point((int)screenResolutionMaintainRatio.Width - left.X - 70, left.Y - 95), 0.7f, Color.White, GTA.UI.Font.ChaletComprimeCologne, UIResText.Alignment.Right);
				uIResText2.DropShadow = true;
				uIResText2.Draw();
				string caption = Money;
				if (string.IsNullOrEmpty(Money))
				{
					caption = DateTime.Now.ToString();
				}
				UIResText uIResText3 = new UIResText(caption, new Point((int)screenResolutionMaintainRatio.Width - left.X - 70, left.Y - 60), 0.4f, Color.White, GTA.UI.Font.ChaletComprimeCologne, UIResText.Alignment.Right);
				uIResText3.DropShadow = true;
				uIResText3.Draw();
				string caption2 = MoneySubtitle;
				if (string.IsNullOrEmpty(MoneySubtitle))
				{
					caption2 = "";
				}
				UIResText uIResText4 = new UIResText(caption2, new Point((int)screenResolutionMaintainRatio.Width - left.X - 70, left.Y - 40), 0.4f, Color.White, GTA.UI.Font.ChaletComprimeCologne, UIResText.Alignment.Right);
				uIResText4.DropShadow = true;
				uIResText4.Draw();
				for (int i = 0; i < Tabs.Count; i++)
				{
					int num = (int)(screenResolutionMaintainRatio.Width - (float)(2 * left.X) - 20f) / Tabs.Count;
					Game.EnableControlThisFrame(Control.CursorX);
					Game.EnableControlThisFrame(Control.CursorY);
					bool flag = UIMenu.IsMouseInBounds(left.AddPoints(new Point((num + 5) * i, 0)), new Size(num, 40));
					Color baseColor = (Tabs[i].Active ? Color.White : (flag ? Color.FromArgb(100, 50, 50, 50) : Color.Black));
					new UIResRectangle(left.AddPoints(new Point((num + 5) * i, 0)), new Size(num, 40), Color.FromArgb(Tabs[i].Active ? 255 : 200, baseColor)).Draw();
					new UIResText(Tabs[i].Title.ToUpper(), left.AddPoints(new Point(num / 2 + (num + 5) * i, 5)), 0.35f, Tabs[i].Active ? Color.Black : Color.White, GTA.UI.Font.ChaletLondon, UIResText.Alignment.Centered).Draw();
					if (Tabs[i].Active)
					{
						new UIResRectangle(left.SubtractPoints(new Point(-((num + 5) * i), 10)), new Size(num, 10), Color.DodgerBlue).Draw();
					}
					if (flag && Game.IsControlJustPressed(Control.CursorAccept) && !Tabs[i].Active)
					{
						Tabs[Index].Active = false;
						Tabs[Index].Focused = false;
						Tabs[Index].Visible = false;
						Index = (1000 - 1000 % Tabs.Count + i) % Tabs.Count;
						Tabs[Index].Active = true;
						Tabs[Index].Focused = true;
						Tabs[Index].Visible = true;
						Tabs[Index].JustOpened = true;
						if (Tabs[Index].CanBeFocused)
						{
							FocusLevel = 1;
						}
						else
						{
							FocusLevel = 0;
						}
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
					}
				}
			}
			Tabs[Index].Draw();
			_sc.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			_sc.Render2D();
		}
	}
}
