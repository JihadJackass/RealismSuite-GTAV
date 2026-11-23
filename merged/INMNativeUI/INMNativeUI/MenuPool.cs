using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GTA;

namespace INMNativeUI
{
	public class MenuPool
	{
		private readonly List<UIMenu> _menuList = new List<UIMenu>();

		public bool MouseEdgeEnabled
		{
			set
			{
				_menuList.ForEach(delegate(UIMenu m)
				{
					m.MouseEdgeEnabled = value;
				});
			}
		}

		public bool ControlDisablingEnabled
		{
			set
			{
				_menuList.ForEach(delegate(UIMenu m)
				{
					m.ControlDisablingEnabled = value;
				});
			}
		}

		public bool ResetCursorOnOpen
		{
			set
			{
				_menuList.ForEach(delegate(UIMenu m)
				{
					m.ResetCursorOnOpen = value;
				});
			}
		}

		public bool FormatDescriptions
		{
			set
			{
				_menuList.ForEach(delegate(UIMenu m)
				{
					m.FormatDescriptions = value;
				});
			}
		}

		public string AUDIO_LIBRARY
		{
			set
			{
				_menuList.ForEach(delegate(UIMenu m)
				{
					m.AUDIO_LIBRARY = value;
				});
			}
		}

		public string AUDIO_UPDOWN
		{
			set
			{
				_menuList.ForEach(delegate(UIMenu m)
				{
					m.AUDIO_UPDOWN = value;
				});
			}
		}

		public string AUDIO_SELECT
		{
			set
			{
				_menuList.ForEach(delegate(UIMenu m)
				{
					m.AUDIO_SELECT = value;
				});
			}
		}

		public string AUDIO_BACK
		{
			set
			{
				_menuList.ForEach(delegate(UIMenu m)
				{
					m.AUDIO_BACK = value;
				});
			}
		}

		public string AUDIO_ERROR
		{
			set
			{
				_menuList.ForEach(delegate(UIMenu m)
				{
					m.AUDIO_ERROR = value;
				});
			}
		}

		public int WidthOffset
		{
			set
			{
				_menuList.ForEach(delegate(UIMenu m)
				{
					m.SetMenuWidthOffset(value);
				});
			}
		}

		public string CounterPretext
		{
			set
			{
				_menuList.ForEach(delegate(UIMenu m)
				{
					m.CounterPretext = value;
				});
			}
		}

		public bool DisableInstructionalButtons
		{
			set
			{
				_menuList.ForEach(delegate(UIMenu m)
				{
					m.DisableInstructionalButtons(value);
				});
			}
		}

		public void Add(UIMenu menu)
		{
			_menuList.Add(menu);
		}

		public UIMenu AddSubMenu(UIMenu menu, string text)
		{
			UIMenuItem uIMenuItem = new UIMenuItem(text);
			menu.AddItem(uIMenuItem);
			UIMenu uIMenu = new UIMenu(menu.Title.Caption, text);
			Add(uIMenu);
			menu.BindMenuToItem(uIMenu, uIMenuItem);
			return uIMenu;
		}

		public UIMenu AddSubMenu(UIMenu menu, string text, Point offset)
		{
			UIMenuItem uIMenuItem = new UIMenuItem(text);
			menu.AddItem(uIMenuItem);
			UIMenu uIMenu = new UIMenu(menu.Title.Caption, text, offset);
			Add(uIMenu);
			menu.BindMenuToItem(uIMenu, uIMenuItem);
			return uIMenu;
		}

		public UIMenu AddSubMenu(UIMenu menu, string text, string description)
		{
			UIMenuItem uIMenuItem = new UIMenuItem(text, description);
			menu.AddItem(uIMenuItem);
			UIMenu uIMenu = new UIMenu(menu.Title.Caption, text);
			Add(uIMenu);
			menu.BindMenuToItem(uIMenu, uIMenuItem);
			return uIMenu;
		}

		public void RefreshIndex()
		{
			foreach (UIMenu menu in _menuList)
			{
				menu.RefreshIndex();
			}
		}

		public List<UIMenu> ToList()
		{
			return _menuList;
		}

		public void ProcessControl()
		{
			UIMenu[] array = _menuList.Where((UIMenu menu) => menu.Visible).ToArray();
			foreach (UIMenu uIMenu in array)
			{
				if (uIMenu.Visible)
				{
					uIMenu.ProcessControl();
				}
			}
		}

		public void ProcessKey(Keys key)
		{
			int count = _menuList.Count;
			for (int i = 0; i < count; i++)
			{
				if (_menuList[i].Visible)
				{
					_menuList[i].ProcessKey(key);
				}
			}
		}

		public void ProcessMouse()
		{
			int count = _menuList.Count;
			for (int i = 0; i < count; i++)
			{
				if (_menuList[i].Visible)
				{
					_menuList[i].ProcessMouse();
				}
			}
		}

		public void Draw()
		{
			int count = _menuList.Count;
			for (int i = 0; i < count; i++)
			{
				if (_menuList[i].Visible)
				{
					_menuList[i].Draw();
				}
			}
		}

		public bool IsAnyMenuOpen()
		{
			return _menuList.Any((UIMenu menu) => menu.Visible);
		}

		public void ProcessMenus()
		{
			ProcessControl();
			ProcessMouse();
			Draw();
		}

		public void UpdateStats(int Stat1, int Stat2, int Stat3, int Stat4)
		{
			foreach (UIMenu item in _menuList.Where((UIMenu menu) => menu.EnableStats))
			{
				item.Stat1 = Stat1;
				item.Stat2 = Stat2;
				item.Stat3 = Stat3;
				item.Stat4 = Stat4;
			}
		}

		public void CloseAllMenus()
		{
			foreach (UIMenu item in _menuList.Where((UIMenu menu) => menu.Visible))
			{
				item.Visible = false;
			}
		}

		public void SetBannerType(Sprite bannerType)
		{
			_menuList.ForEach(delegate(UIMenu m)
			{
				m.SetBannerType(bannerType);
			});
		}

		public void SetBannerType(UIResRectangle bannerType)
		{
			_menuList.ForEach(delegate(UIMenu m)
			{
				m.SetBannerType(bannerType);
			});
		}

		public void SetBannerType(string bannerPath)
		{
			_menuList.ForEach(delegate(UIMenu m)
			{
				m.SetBannerType(bannerPath);
			});
		}

		public void SetKey(UIMenu.MenuControls menuControl, GTA.Control control)
		{
			_menuList.ForEach(delegate(UIMenu m)
			{
				m.SetKey(menuControl, control);
			});
		}

		public void SetKey(UIMenu.MenuControls menuControl, GTA.Control control, int controllerIndex)
		{
			_menuList.ForEach(delegate(UIMenu m)
			{
				m.SetKey(menuControl, control, controllerIndex);
			});
		}

		public void SetKey(UIMenu.MenuControls menuControl, Keys control)
		{
			_menuList.ForEach(delegate(UIMenu m)
			{
				m.SetKey(menuControl, control);
			});
		}

		public void ResetKey(UIMenu.MenuControls menuControl)
		{
			_menuList.ForEach(delegate(UIMenu m)
			{
				m.ResetKey(menuControl);
			});
		}
	}
}
