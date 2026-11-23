using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using GTA.UI;

namespace INMNativeUI
{
	public class UIMenu
	{
		public enum MenuControls
		{
			Up,
			Down,
			Left,
			Right,
			Select,
			Back
		}

		private readonly UIContainer _mainMenu;

		private Sprite _logo;

		private readonly Sprite _background;

		private readonly UIResRectangle _descriptionBar;

		private readonly Sprite _descriptionRectangle;

		private readonly UIResText _descriptionText;

		private readonly UIResText _counterText;

		private readonly Sprite _statRectangle;

		private readonly UIResText _stat1Text;

		private readonly UIResRectangle _stat1BGBar;

		private readonly UIResRectangle _stat1WhiteBar;

		private readonly UIResRectangle _stat11SepBar;

		private readonly UIResRectangle _stat12SepBar;

		private readonly UIResRectangle _stat13SepBar;

		private readonly UIResRectangle _stat14SepBar;

		public int Stat1 = 50;

		public bool _stat1StrCustom;

		private readonly UIResText _stat2Text;

		private readonly UIResRectangle _stat2BGBar;

		private readonly UIResRectangle _stat2WhiteBar;

		private readonly UIResRectangle _stat21SepBar;

		private readonly UIResRectangle _stat22SepBar;

		private readonly UIResRectangle _stat23SepBar;

		private readonly UIResRectangle _stat24SepBar;

		public int Stat2 = 100;

		public bool _stat2StrCustom;

		private readonly UIResText _stat3Text;

		private readonly UIResRectangle _stat3BGBar;

		private readonly UIResRectangle _stat3WhiteBar;

		private readonly UIResRectangle _stat31SepBar;

		private readonly UIResRectangle _stat32SepBar;

		private readonly UIResRectangle _stat33SepBar;

		private readonly UIResRectangle _stat34SepBar;

		public int Stat3 = 150;

		public bool _stat3StrCustom;

		private readonly UIResText _stat4Text;

		private readonly UIResRectangle _stat4BGBar;

		private readonly UIResRectangle _stat4WhiteBar;

		private readonly UIResRectangle _stat41SepBar;

		private readonly UIResRectangle _stat42SepBar;

		private readonly UIResRectangle _stat43SepBar;

		private readonly UIResRectangle _stat44SepBar;

		public int Stat4 = 200;

		public bool _stat4StrCustom;

		public string _stat1Str;

		public string _stat2Str;

		public string _stat3Str;

		public string _stat4Str;

		private UIResRectangle _tmpRectangle;

		private string _customBanner;

		private int _activeItem = 1000;

		private bool _visible;

		private bool _buttonsEnabled = true;

		private bool _justOpened = true;

		private bool _itemsDirty;

		private const int MaxItemsOnScreen = 9;

		private int _minItem;

		private int _maxItem = 9;

		private readonly Dictionary<MenuControls, Tuple<List<Keys>, List<Tuple<GTA.Control, int>>>> _keyDictionary = new Dictionary<MenuControls, Tuple<List<Keys>, List<Tuple<GTA.Control, int>>>>();

		private readonly List<InstructionalButton> _instructionalButtons = new List<InstructionalButton>();

		private readonly Sprite _upAndDownSprite;

		private readonly UIResRectangle _extraRectangleUp;

		private readonly UIResRectangle _extraRectangleDown;

		private static Scaleform _sharedButtons;

		private static UIMenu _sharedButtonsOwner;

		private static bool _sharedButtonsPushedLoaded;

		private Point _offset;

		private readonly int _extraYOffset;

		private static readonly MenuControls[] _menuControls = Enum.GetValues(typeof(MenuControls)).Cast<MenuControls>().ToArray();

		private static readonly List<GTA.Control> _necessaryControlsForKeyboard = new List<GTA.Control>
		{
			GTA.Control.FrontendAccept,
			GTA.Control.FrontendAxisX,
			GTA.Control.FrontendAxisY,
			GTA.Control.FrontendDown,
			GTA.Control.FrontendUp,
			GTA.Control.FrontendLeft,
			GTA.Control.FrontendRight,
			GTA.Control.FrontendCancel,
			GTA.Control.FrontendSelect,
			GTA.Control.CursorScrollDown,
			GTA.Control.CursorScrollUp,
			GTA.Control.CursorX,
			GTA.Control.CursorY,
			GTA.Control.MoveUpDown,
			GTA.Control.MoveLeftRight,
			GTA.Control.Sprint,
			GTA.Control.Jump,
			GTA.Control.Enter,
			GTA.Control.VehicleExit,
			GTA.Control.VehicleAccelerate,
			GTA.Control.VehicleBrake,
			GTA.Control.VehicleMoveLeftRight,
			GTA.Control.VehicleFlyYawLeft,
			GTA.Control.FlyLeftRight,
			GTA.Control.FlyUpDown,
			GTA.Control.VehicleFlyYawRight,
			GTA.Control.VehicleHandbrake
		};

		private static readonly List<GTA.Control> _necessaryControlsForController = _necessaryControlsForKeyboard.Concat(new GTA.Control[4]
		{
			GTA.Control.LookUpDown,
			GTA.Control.LookLeftRight,
			GTA.Control.Aim,
			GTA.Control.Attack
		}).ToList();

		private bool reDraw = true;

		private static string _selectText;

		private static string _backText;

		public string AUDIO_LIBRARY = "HUD_FRONTEND_DEFAULT_SOUNDSET";

		public string AUDIO_UPDOWN = "NAV_UP_DOWN";

		public string AUDIO_LEFTRIGHT = "NAV_LEFT_RIGHT";

		public string AUDIO_SELECT = "SELECT";

		public string AUDIO_BACK = "BACK";

		public string AUDIO_ERROR = "ERROR";

		public List<UIMenuItem> MenuItems = new List<UIMenuItem>();

		public bool MouseEdgeEnabled = true;

		public bool ControlDisablingEnabled = true;

		public bool ResetCursorOnOpen = true;

		public bool FormatDescriptions = true;

		public bool FormatStats = true;

		public bool MouseControlsEnabled = true;

		public bool ScaleWithSafezone = true;

		private MenuInputRepeat[] _inputStates;

		private int[] _inputTransitions;

		private int _inputSnapshotFrame = -1;

		private int _processedControlFrame = -1;

		private static Scaleform SharedButtons
		{
			get
			{
				if (_sharedButtons == null)
				{
					_sharedButtons = new Scaleform("instructional_buttons");
				}
				return _sharedButtons;
			}
		}

		private Point safe { get; set; }

		private Size backgroundSize { get; set; }

		private Size drawWidth { get; set; }

		internal static string _selectTextLocalized => _selectText ?? (_selectText = Game.GetLocalizedString("HUD_INPUT2"));

		internal static string _backTextLocalized => _backText ?? (_backText = Game.GetLocalizedString("HUD_INPUT3"));

		public bool SoundEnabled { get; set; } = true;


		public bool Visible
		{
			get
			{
				return _visible;
			}
			set
			{
				_visible = value;
				_inputStates = null;
				_inputTransitions = null;
				_inputSnapshotFrame = -1;
				_processedControlFrame = -1;
				_justOpened = value;
				_itemsDirty = value;
				UpdateScaleform();
				if (ParentMenu == null && value && ResetCursorOnOpen)
				{
					Cursor.Position = new Point(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 2);
					Function.Call(Hash.SET_MOUSE_CURSOR_STYLE, 1);
				}
			}
		}

		public int CurrentSelection
		{
			get
			{
				if (MenuItems.Count == 0)
				{
					return 0;
				}
				int num = _activeItem % MenuItems.Count;
				if (num >= 0)
				{
					return num;
				}
				return num + MenuItems.Count;
			}
			set
			{
				if (MenuItems.Count == 0)
				{
					RefreshIndex();
					return;
				}
				MenuItems[CurrentSelection].Selected = false;
				int num = value % MenuItems.Count;
				if (num < 0)
				{
					num += MenuItems.Count;
				}
				_activeItem = 1000000 - 1000000 % MenuItems.Count + num;
				if (CurrentSelection > _maxItem)
				{
					_maxItem = CurrentSelection;
					_minItem = CurrentSelection - 9;
				}
				else if (CurrentSelection < _minItem)
				{
					_maxItem = 9 + CurrentSelection;
					_minItem = CurrentSelection;
				}
			}
		}

		public static bool IsUsingController => !Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 2);

		public int Size => MenuItems.Count;

		public UIResText Title { get; }

		public UIResText Subtitle { get; }

		public string CounterPretext { get; set; }

		public UIMenu ParentMenu { get; set; }

		public UIMenuItem ParentItem { get; set; }

		public bool EnableStats { get; set; }

		public Dictionary<UIMenuItem, UIMenu> Children { get; }

		public int WidthOffset { get; private set; }

		public event IndexChangedEvent OnIndexChange;

		public event ListChangedEvent OnListChange;

		public event CheckboxChangeEvent OnCheckboxChange;

		public event ItemSelectEvent OnItemSelect;

		public event MenuCloseEvent OnMenuClose;

		public event MenuChangeEvent OnMenuChange;

		private void PlayMenuSound(string name)
		{
			if (Visible && SoundEnabled)
			{
				Audio.PlaySoundFrontendAndForget(name, AUDIO_LIBRARY, false);
			}
		}

		public UIMenu(string title, string subtitle)
			: this(title, subtitle, new Point(0, 0), "commonmenu", "interaction_bgd")
		{
		}

		public UIMenu(string title, string subtitle, bool showStats = false)
			: this(title, subtitle, new Point(0, 0), "commonmenu", "interaction_bgd", showStats)
		{
		}

		public UIMenu(string title, string subtitle, Point offset)
			: this(title, subtitle, offset, "commonmenu", "interaction_bgd")
		{
		}

		public UIMenu(string title, string subtitle, Point offset, string customBanner)
			: this(title, subtitle, offset, "commonmenu", "interaction_bgd")
		{
			_customBanner = customBanner;
		}

		public UIMenu(string title, string subtitle, Point offset, string spriteLibrary, string spriteName, bool showStats = false)
		{
			_offset = offset;
			Children = new Dictionary<UIMenuItem, UIMenu>();
			WidthOffset = 0;
			EnableStats = showStats;
			_mainMenu = new UIContainer(new Point(0, 0), new Size(700, 500), Color.FromArgb(0, 0, 0, 0));
			_logo = new Sprite(spriteLibrary, spriteName, new Point(_offset.X, _offset.Y), new Size(431, 107));
			_mainMenu.Items.Add(Title = new UIResText(title, new Point(215 + _offset.X, 20 + _offset.Y), 1.15f, Color.White, GTA.UI.Font.HouseScript, UIResText.Alignment.Centered));
			if (!string.IsNullOrWhiteSpace(subtitle))
			{
				_mainMenu.Items.Add(new UIResRectangle(new Point(offset.X, 107 + _offset.Y), new Size(431, 37), Color.Black));
				_mainMenu.Items.Add(Subtitle = new UIResText(subtitle, new Point(8 + _offset.X, 110 + _offset.Y), 0.35f, Color.WhiteSmoke, GTA.UI.Font.ChaletLondon, UIResText.Alignment.Left));
				if (subtitle.StartsWith("~"))
				{
					CounterPretext = subtitle.Substring(0, 3);
				}
				_counterText = new UIResText("", new Point(425 + _offset.X, 110 + _offset.Y), 0.35f, Color.WhiteSmoke, GTA.UI.Font.ChaletLondon, UIResText.Alignment.Right);
				_extraYOffset = 37;
			}
			_upAndDownSprite = new Sprite("commonmenu", "shop_arrows_upanddown", new Point(190 + _offset.X, 517 + _offset.Y - 37 + _extraYOffset), new Size(50, 50));
			_extraRectangleUp = new UIResRectangle(new Point(_offset.X, 524 + _offset.Y - 37 + _extraYOffset), new Size(431, 18), Color.FromArgb(200, 0, 0, 0));
			_extraRectangleDown = new UIResRectangle(new Point(_offset.X, 542 + _offset.Y - 37 + _extraYOffset), new Size(431, 18), Color.FromArgb(200, 0, 0, 0));
			_descriptionBar = new UIResRectangle(new Point(_offset.X, 123), new Size(431, 4), Color.Black);
			_descriptionRectangle = new Sprite("commonmenu", "gradient_bgd", new Point(_offset.X, 127), new Size(431, 30));
			_descriptionText = new UIResText("Description", new Point(_offset.X + 5, 125), 0.35f, Color.FromArgb(255, 255, 255, 255), GTA.UI.Font.ChaletLondon, UIResText.Alignment.Left);
			_statRectangle = new Sprite("commonmenu", "gradient_bgd", new Point(_offset.X, _descriptionRectangle.Position.Y + 2), new Size(431, 30), 0f, Color.Black);
			if (_stat1StrCustom)
			{
				_stat1Text = new UIResText(_stat1Str, new Point(_offset.X + 5, 125), 0.35f, Color.FromArgb(255, 255, 255, 255), GTA.UI.Font.ChaletLondon, UIResText.Alignment.Left);
			}
			else
			{
				_stat1Text = new UIResText(Game.GetLocalizedString("CMOD_STAT_0"), new Point(_offset.X + 5, 125), 0.35f, Color.FromArgb(255, 255, 255, 255), GTA.UI.Font.ChaletLondon, UIResText.Alignment.Left);
			}
			_stat1BGBar = new UIResRectangle(new Point(0, 0), new Size(200, 10), Color.DarkGray);
			_stat1WhiteBar = new UIResRectangle(new Point(0, 0), new Size(200, 10), Color.White);
			if (_stat2StrCustom)
			{
				_stat2Text = new UIResText(_stat2Str, new Point(_offset.X + 5, 135), 0.35f, Color.FromArgb(255, 255, 255, 255), GTA.UI.Font.ChaletLondon, UIResText.Alignment.Left);
			}
			else
			{
				_stat2Text = new UIResText(Game.GetLocalizedString("CMOD_STAT_1"), new Point(_offset.X + 5, 135), 0.35f, Color.FromArgb(255, 255, 255, 255), GTA.UI.Font.ChaletLondon, UIResText.Alignment.Left);
			}
			_stat2BGBar = new UIResRectangle(new Point(0, 0), new Size(200, 10), Color.DarkGray);
			_stat2WhiteBar = new UIResRectangle(new Point(0, 0), new Size(200, 10), Color.White);
			if (_stat3StrCustom)
			{
				_stat3Text = new UIResText(_stat3Str, new Point(_offset.X + 5, 145), 0.35f, Color.FromArgb(255, 255, 255, 255), GTA.UI.Font.ChaletLondon, UIResText.Alignment.Left);
			}
			else
			{
				_stat3Text = new UIResText(Game.GetLocalizedString("CMOD_STAT_2"), new Point(_offset.X + 5, 145), 0.35f, Color.FromArgb(255, 255, 255, 255), GTA.UI.Font.ChaletLondon, UIResText.Alignment.Left);
			}
			_stat3BGBar = new UIResRectangle(new Point(0, 0), new Size(200, 10), Color.DarkGray);
			_stat3WhiteBar = new UIResRectangle(new Point(0, 0), new Size(200, 10), Color.White);
			if (_stat4StrCustom)
			{
				_stat4Text = new UIResText(_stat4Str, new Point(_offset.X + 5, 155), 0.35f, Color.FromArgb(255, 255, 255, 255), GTA.UI.Font.ChaletLondon, UIResText.Alignment.Left);
			}
			else
			{
				_stat4Text = new UIResText(Game.GetLocalizedString("CMOD_STAT_3"), new Point(_offset.X + 5, 155), 0.35f, Color.FromArgb(255, 255, 255, 255), GTA.UI.Font.ChaletLondon, UIResText.Alignment.Left);
			}
			_stat4BGBar = new UIResRectangle(new Point(0, 0), new Size(200, 10), Color.DarkGray);
			_stat4WhiteBar = new UIResRectangle(new Point(0, 0), new Size(200, 10), Color.White);
			_stat11SepBar = new UIResRectangle(new Point(0, 0), new Size(2, 10), Color.Black);
			_stat12SepBar = new UIResRectangle(new Point(0, 0), new Size(2, 10), Color.Black);
			_stat13SepBar = new UIResRectangle(new Point(0, 0), new Size(2, 10), Color.Black);
			_stat14SepBar = new UIResRectangle(new Point(0, 0), new Size(2, 10), Color.Black);
			_stat21SepBar = new UIResRectangle(new Point(0, 0), new Size(2, 10), Color.Black);
			_stat22SepBar = new UIResRectangle(new Point(0, 0), new Size(2, 10), Color.Black);
			_stat23SepBar = new UIResRectangle(new Point(0, 0), new Size(2, 10), Color.Black);
			_stat24SepBar = new UIResRectangle(new Point(0, 0), new Size(2, 10), Color.Black);
			_stat31SepBar = new UIResRectangle(new Point(0, 0), new Size(2, 10), Color.Black);
			_stat32SepBar = new UIResRectangle(new Point(0, 0), new Size(2, 10), Color.Black);
			_stat33SepBar = new UIResRectangle(new Point(0, 0), new Size(2, 10), Color.Black);
			_stat34SepBar = new UIResRectangle(new Point(0, 0), new Size(2, 10), Color.Black);
			_stat41SepBar = new UIResRectangle(new Point(0, 0), new Size(2, 10), Color.Black);
			_stat42SepBar = new UIResRectangle(new Point(0, 0), new Size(2, 10), Color.Black);
			_stat43SepBar = new UIResRectangle(new Point(0, 0), new Size(2, 10), Color.Black);
			_stat44SepBar = new UIResRectangle(new Point(0, 0), new Size(2, 10), Color.Black);
			_background = new Sprite("commonmenu", "gradient_bgd", new Point(_offset.X, 144 + _offset.Y - 37 + _extraYOffset), new Size(290, 25));
			SetKey(MenuControls.Up, GTA.Control.PhoneUp);
			SetKey(MenuControls.Up, GTA.Control.CursorScrollUp);
			SetKey(MenuControls.Down, GTA.Control.PhoneDown);
			SetKey(MenuControls.Down, GTA.Control.CursorScrollDown);
			SetKey(MenuControls.Left, GTA.Control.PhoneLeft);
			SetKey(MenuControls.Right, GTA.Control.PhoneRight);
			SetKey(MenuControls.Select, GTA.Control.FrontendAccept);
			SetKey(MenuControls.Back, GTA.Control.PhoneCancel);
			SetKey(MenuControls.Back, GTA.Control.FrontendPause);
		}

		public static int LockFrame = -100;
		public static UIMenu LockedBy;

		public static void DisEnableControls(bool enable)
		{
			if (enable)
			{
				Game.EnableAllControlsThisFrame();
			}
			else
			{
				Game.DisableAllControlsThisFrame();
			}
			if (enable)
			{
				return;
			}
			foreach (GTA.Control item in IsUsingController ? _necessaryControlsForController : _necessaryControlsForKeyboard)
			{
				Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, (int)item);
			}
		}

		public static SizeF GetScreenResolutionMaintainRatio()
		{
			int width = GTA.UI.Screen.Resolution.Width;
			int height = GTA.UI.Screen.Resolution.Height;
			float num = (float)width / (float)height;
			return new SizeF(1080f * num, 1080f);
		}

		public static bool IsMouseInBounds(Point topLeft, Size boxSize)
		{
			SizeF screenResolutionMaintainRatio = GetScreenResolutionMaintainRatio();
			int num = (int)Math.Round(Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, 239) * screenResolutionMaintainRatio.Width);
			int num2 = (int)Math.Round(Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, 240) * screenResolutionMaintainRatio.Height);
			if (num >= topLeft.X && num <= topLeft.X + boxSize.Width)
			{
				if (num2 > topLeft.Y)
				{
					return num2 < topLeft.Y + boxSize.Height;
				}
				return false;
			}
			return false;
		}

		public static Point GetSafezoneBounds()
		{
			double num = Math.Round(Convert.ToDouble(Function.Call<float>(Hash.GET_SAFE_ZONE_SIZE)), 2);
			num = num * 100.0 - 90.0;
			num = 10.0 - num;
			int width = GTA.UI.Screen.Resolution.Width;
			int height = GTA.UI.Screen.Resolution.Height;
			float num2 = (float)width / (float)height * 5.4f;
			return new Point((int)Math.Round(num * (double)num2), (int)Math.Round(num * 5.4000000953674316));
		}

		public void SetMenuWidthOffset(int widthOffset)
		{
			WidthOffset = widthOffset;
			_logo.Size = new Size(431 + WidthOffset, 107);
			_mainMenu.Items[0].Position = new Point((WidthOffset + _offset.X + 431) / 2, 20 + _offset.Y);
			_counterText.Position = new Point(425 + _offset.X + widthOffset, 110 + _offset.Y);
			if (_mainMenu.Items.Count >= 1)
			{
				((UIResRectangle)_mainMenu.Items[1]).Size = new Size(431 + WidthOffset, 37);
			}
			if (_tmpRectangle != null)
			{
				_tmpRectangle.Size = new Size(431 + WidthOffset, 107);
			}
		}

		public void DisableInstructionalButtons(bool disable)
		{
			_buttonsEnabled = !disable;
		}

		public void SetBannerType(Sprite spriteBanner)
		{
			_logo = spriteBanner;
			_logo.Size = new Size(431 + WidthOffset, 107);
			_logo.Position = new Point(_offset.X, _offset.Y);
		}

		public void SetBannerType(UIResRectangle rectangle)
		{
			_logo = null;
			_tmpRectangle = rectangle;
			_tmpRectangle.Position = new Point(_offset.X, _offset.Y);
			_tmpRectangle.Size = new Size(431 + WidthOffset, 107);
		}

		public void SetBannerType(string pathToCustomSprite)
		{
			_customBanner = pathToCustomSprite;
		}

		public void AddItem(UIMenuItem item)
		{
			int currentSelection = CurrentSelection;
			item.Offset = _offset;
			item.Parent = this;
			item.Position(MenuItems.Count * 25 - 37 + _extraYOffset);
			MenuItems.Add(item);
			RecaulculateDescriptionPosition();
			RecaulculateStatPosition();
			CurrentSelection = currentSelection;
		}

		public void RemoveItemAt(int index)
		{
			int currentSelection = CurrentSelection;
			if (Size > 9 && _maxItem == Size - 1)
			{
				_maxItem--;
				_minItem--;
			}
			MenuItems.RemoveAt(index);
			RecaulculateDescriptionPosition();
			RecaulculateStatPosition();
			CurrentSelection = currentSelection;
		}

		public void RefreshIndex()
		{
			reDraw = true;
			if (MenuItems.Count == 0)
			{
				_activeItem = 1000;
				_maxItem = 9;
				_minItem = 0;
			}
			else
			{
				MenuItems[CurrentSelection].Selected = false;
				_activeItem = 1000 - 1000 % MenuItems.Count;
				_maxItem = 9;
				_minItem = 0;
				reDraw = true;
			}
		}

		public void Clear()
		{
			MenuItems.Clear();
			RefreshIndex();
			RecaulculateDescriptionPosition();
			RecaulculateStatPosition();
		}

		private void DrawCalculations()
		{
			drawWidth = new Size(431 + WidthOffset, 107);
			safe = GetSafezoneBounds();
			backgroundSize = ((Size > 10) ? new Size(431 + WidthOffset, 380) : new Size(431 + WidthOffset, 38 * Size));
			_extraRectangleUp.Size = new Size(431 + WidthOffset, 18);
			_extraRectangleDown.Size = new Size(431 + WidthOffset, 18);
			_upAndDownSprite.Position = new Point(190 + _offset.X + ((WidthOffset > 0) ? (WidthOffset / 2) : WidthOffset), 517 + _offset.Y - 37 + _extraYOffset);
			reDraw = false;
			if (MenuItems.Count != 0 && !string.IsNullOrWhiteSpace(MenuItems[CurrentSelection].Description))
			{
				RecaulculateDescriptionPosition();
				string description = MenuItems[CurrentSelection].Description;
				if (FormatDescriptions)
				{
					_descriptionText.Caption = FormatDescription(description);
				}
				else
				{
					_descriptionText.Caption = description;
				}
				int num = _descriptionText.Caption.Split(new char[1] { '\n' }).Length;
				_descriptionRectangle.Size = new Size(431 + WidthOffset, num * 25 + 15);
				RecaulculateStatPosition();
			}
		}

		public void GoUpOverflow()
		{
			if (Size <= 10)
			{
				return;
			}
			if (CurrentSelection <= _minItem)
			{
				if (CurrentSelection == 0)
				{
					_minItem = MenuItems.Count - 9 - 1;
					_maxItem = MenuItems.Count - 1;
					MenuItems[CurrentSelection].Selected = false;
					_activeItem = 1000 - 1000 % MenuItems.Count;
					_activeItem += MenuItems.Count - 1;
					MenuItems[CurrentSelection].Selected = true;
				}
				else
				{
					_minItem--;
					_maxItem--;
					MenuItems[CurrentSelection].Selected = false;
					_activeItem--;
					MenuItems[CurrentSelection].Selected = true;
				}
			}
			else
			{
				MenuItems[CurrentSelection].Selected = false;
				_activeItem--;
				MenuItems[CurrentSelection].Selected = true;
			}
			PlayMenuSound(AUDIO_UPDOWN);
			IndexChange(CurrentSelection);
		}

		public void GoUp()
		{
			if (Size != 0 && Size <= 10)
			{
				MenuItems[CurrentSelection].Selected = false;
				_activeItem--;
				MenuItems[CurrentSelection].Selected = true;
				PlayMenuSound(AUDIO_UPDOWN);
				IndexChange(CurrentSelection);
			}
		}

		public void GoDownOverflow()
		{
			if (Size <= 10)
			{
				return;
			}
			if (CurrentSelection >= _maxItem)
			{
				if (CurrentSelection == MenuItems.Count - 1)
				{
					_minItem = 0;
					_maxItem = 9;
					MenuItems[CurrentSelection].Selected = false;
					_activeItem = 1000 - 1000 % MenuItems.Count;
					MenuItems[CurrentSelection].Selected = true;
				}
				else
				{
					_minItem++;
					_maxItem++;
					MenuItems[CurrentSelection].Selected = false;
					_activeItem++;
					MenuItems[CurrentSelection].Selected = true;
				}
			}
			else
			{
				MenuItems[CurrentSelection].Selected = false;
				_activeItem++;
				MenuItems[CurrentSelection].Selected = true;
			}
			PlayMenuSound(AUDIO_UPDOWN);
			IndexChange(CurrentSelection);
		}

		public void GoDown()
		{
			if (Size != 0 && Size <= 10)
			{
				MenuItems[CurrentSelection].Selected = false;
				_activeItem++;
				MenuItems[CurrentSelection].Selected = true;
				PlayMenuSound(AUDIO_UPDOWN);
				IndexChange(CurrentSelection);
			}
		}

		public void GoLeft()
		{
			if (MenuItems.Count == 0 || !MenuItems[CurrentSelection].Enabled)
			{
				return;
			}
			if (MenuItems[CurrentSelection] is UIMenuListItem)
			{
				UIMenuListItem uIMenuListItem = (UIMenuListItem)MenuItems[CurrentSelection];
				if (uIMenuListItem.HasItems)
				{
					uIMenuListItem.Index--;
					PlayMenuSound(AUDIO_LEFTRIGHT);
					ListChange(uIMenuListItem, uIMenuListItem.Index);
					uIMenuListItem.ListChangedTrigger(uIMenuListItem.Index);
				}
			}
			else if (MenuItems[CurrentSelection] is UIMenuDynamicListItem)
			{
				UIMenuDynamicListItem uIMenuDynamicListItem = (UIMenuDynamicListItem)MenuItems[CurrentSelection];
				string text2 = (uIMenuDynamicListItem.CurrentListItem = uIMenuDynamicListItem.Callback(uIMenuDynamicListItem, UIMenuDynamicListItem.ChangeDirection.Left));
				PlayMenuSound(AUDIO_LEFTRIGHT);
			}
		}

		public void GoRight()
		{
			if (MenuItems.Count == 0 || !MenuItems[CurrentSelection].Enabled)
			{
				return;
			}
			if (MenuItems[CurrentSelection] is UIMenuListItem)
			{
				UIMenuListItem uIMenuListItem = (UIMenuListItem)MenuItems[CurrentSelection];
				if (uIMenuListItem.HasItems)
				{
					uIMenuListItem.Index++;
					PlayMenuSound(AUDIO_LEFTRIGHT);
					ListChange(uIMenuListItem, uIMenuListItem.Index);
					uIMenuListItem.ListChangedTrigger(uIMenuListItem.Index);
				}
			}
			else if (MenuItems[CurrentSelection] is UIMenuDynamicListItem)
			{
				UIMenuDynamicListItem uIMenuDynamicListItem = (UIMenuDynamicListItem)MenuItems[CurrentSelection];
				string text2 = (uIMenuDynamicListItem.CurrentListItem = uIMenuDynamicListItem.Callback(uIMenuDynamicListItem, UIMenuDynamicListItem.ChangeDirection.Right));
				PlayMenuSound(AUDIO_LEFTRIGHT);
			}
		}

		public void SelectItem()
		{
			if (MenuItems.Count == 0)
			{
				return;
			}
			int currentSelection = CurrentSelection;
			UIMenuItem uIMenuItem = MenuItems[currentSelection];
			if (!uIMenuItem.Enabled)
			{
				PlayMenuSound(AUDIO_ERROR);
				return;
			}
			if (uIMenuItem is UIMenuCheckboxItem)
			{
				UIMenuCheckboxItem uIMenuCheckboxItem = (UIMenuCheckboxItem)uIMenuItem;
				uIMenuCheckboxItem.Checked = !uIMenuCheckboxItem.Checked;
				PlayMenuSound(AUDIO_SELECT);
				CheckboxChange(uIMenuCheckboxItem, uIMenuCheckboxItem.Checked);
				uIMenuCheckboxItem.CheckboxEventTrigger();
				return;
			}
			PlayMenuSound(AUDIO_SELECT);
			ItemSelect(uIMenuItem, currentSelection);
			uIMenuItem.ItemActivate(this);
			if (Visible && MenuItems.Contains(uIMenuItem) && Children.ContainsKey(uIMenuItem))
			{
				Visible = false;
				Children[uIMenuItem].Visible = true;
				MenuChangeEv(Children[uIMenuItem], true);
			}
		}

		public void GoBack()
		{
			PlayMenuSound(AUDIO_BACK);
			Visible = false;
			if (ParentMenu != null)
			{
				Point position = Cursor.Position;
				ParentMenu.Visible = true;
				MenuChangeEv(ParentMenu, false);
				if (ResetCursorOnOpen)
				{
					Cursor.Position = position;
				}
			}
			MenuCloseEv();
		}

		public void BindMenuToItem(UIMenu menuToBind, UIMenuItem itemToBindTo)
		{
			menuToBind.ParentMenu = this;
			menuToBind.ParentItem = itemToBindTo;
			if (Children.ContainsKey(itemToBindTo))
			{
				Children[itemToBindTo] = menuToBind;
			}
			else
			{
				Children.Add(itemToBindTo, menuToBind);
			}
		}

		public bool ReleaseMenuFromItem(UIMenuItem releaseFrom)
		{
			if (!Children.ContainsKey(releaseFrom))
			{
				return false;
			}
			Children[releaseFrom].ParentItem = null;
			Children[releaseFrom].ParentMenu = null;
			Children.Remove(releaseFrom);
			return true;
		}

		public void SetKey(MenuControls control, Keys keyToSet)
		{
			if (_keyDictionary.ContainsKey(control))
			{
				_keyDictionary[control].Item1.Add(keyToSet);
				return;
			}
			_keyDictionary.Add(control, new Tuple<List<Keys>, List<Tuple<GTA.Control, int>>>(new List<Keys>(), new List<Tuple<GTA.Control, int>>()));
			_keyDictionary[control].Item1.Add(keyToSet);
		}

		public void SetKey(MenuControls control, GTA.Control gtaControl)
		{
			SetKey(control, gtaControl, 0);
			SetKey(control, gtaControl, 1);
			SetKey(control, gtaControl, 2);
		}

		public void SetKey(MenuControls control, GTA.Control gtaControl, int controlIndex)
		{
			if (_keyDictionary.ContainsKey(control))
			{
				_keyDictionary[control].Item2.Add(new Tuple<GTA.Control, int>(gtaControl, controlIndex));
				return;
			}
			_keyDictionary.Add(control, new Tuple<List<Keys>, List<Tuple<GTA.Control, int>>>(new List<Keys>(), new List<Tuple<GTA.Control, int>>()));
			_keyDictionary[control].Item2.Add(new Tuple<GTA.Control, int>(gtaControl, controlIndex));
		}

		public void ResetKey(MenuControls control)
		{
			_keyDictionary[control].Item1.Clear();
			_keyDictionary[control].Item2.Clear();
		}

		private bool ControlDown(MenuControls control)
		{
			Tuple<List<Keys>, List<Tuple<GTA.Control, int>>> tuple2 = _keyDictionary[control];
			if (tuple2.Item1.Any(Game.IsKeyPressed))
			{
				return true;
			}
			return tuple2.Item2.Any((Tuple<GTA.Control, int> tuple) => Function.Call<bool>(Hash.IS_CONTROL_PRESSED, tuple.Item2, (int)tuple.Item1) || Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, tuple.Item2, (int)tuple.Item1));
		}

		private void PollControls(bool opening = false)
		{
			int frameCount = Game.FrameCount;
			if (!opening && frameCount == _inputSnapshotFrame)
			{
				return;
			}
			_inputSnapshotFrame = frameCount;
			if (_inputStates == null)
			{
				_inputStates = _menuControls.Select((MenuControls c) => new MenuInputRepeat()).ToArray();
				_inputTransitions = new int[_menuControls.Length];
			}
			uint gameTime = (uint)Game.GameTime;
			MenuControls[] menuControls = _menuControls;
			foreach (MenuControls menuControls2 in menuControls)
			{
				int num = (int)menuControls2;
				bool held = ControlDown(menuControls2);
				if (opening)
				{
					_inputStates[num].Suppress(held);
					_inputTransitions[num] = 0;
				}
				else
				{
					_inputTransitions[num] = _inputStates[num].Update(held, gameTime, menuControls2 == MenuControls.Up || menuControls2 == MenuControls.Down || menuControls2 == MenuControls.Left || menuControls2 == MenuControls.Right);
				}
			}
		}

		public bool HasControlJustBeenPressed(MenuControls control, Keys key = Keys.None)
		{
			PollControls();
			return (_inputTransitions[(int)control] & 1) != 0;
		}

		public bool HasControlJustBeenReleaseed(MenuControls control, Keys key = Keys.None)
		{
			PollControls();
			return (_inputTransitions[(int)control] & 4) != 0;
		}

		public bool IsControlBeingPressed(MenuControls control, Keys key = Keys.None)
		{
			PollControls();
			return (_inputTransitions[(int)control] & 3) != 0;
		}

		public void AddInstructionalButton(InstructionalButton button)
		{
			_instructionalButtons.Add(button);
		}

		public void RemoveInstructionalButton(InstructionalButton button)
		{
			_instructionalButtons.Remove(button);
		}

		private void RecaulculateDescriptionPosition()
		{
			_descriptionBar.Position = new Point(_offset.X, 112 + _extraYOffset + _offset.Y);
			_descriptionRectangle.Position = new Point(_offset.X, 112 + _extraYOffset + _offset.Y);
			_descriptionText.Position = new Point(_offset.X + 8, 118 + _extraYOffset + _offset.Y);
			_descriptionBar.Size = new Size(431 + WidthOffset, 4);
			_descriptionRectangle.Size = new Size(431 + WidthOffset, 30);
			int num = Size;
			if (num > 10)
			{
				num = 11;
			}
			_descriptionBar.Position = new Point(_offset.X, 38 * num + _descriptionBar.Position.Y);
			_descriptionRectangle.Position = new Point(_offset.X, 38 * num + _descriptionRectangle.Position.Y);
			_descriptionText.Position = new Point(_offset.X + 8, 38 * num + _descriptionText.Position.Y);
		}

		private void RecaulculateStatPosition()
		{
			_statRectangle.Position = new Point(_offset.X, 112 + _extraYOffset + _offset.Y);
			_stat1Text.Position = new Point(_offset.X + 8, 118 + _extraYOffset + _offset.Y);
			_stat1BGBar.Position = new Point(_offset.X + 200, 118 + _extraYOffset + _offset.Y);
			_stat1WhiteBar.Position = new Point(_offset.X + 200, 118 + _extraYOffset + _offset.Y);
			_stat2Text.Position = new Point(_offset.X + 8, 138 + _extraYOffset + _offset.Y);
			_stat2BGBar.Position = new Point(_offset.X + 200, 138 + _extraYOffset + _offset.Y);
			_stat2WhiteBar.Position = new Point(_offset.X + 200, 138 + _extraYOffset + _offset.Y);
			_stat3Text.Position = new Point(_offset.X + 8, 158 + _extraYOffset + _offset.Y);
			_stat3BGBar.Position = new Point(_offset.X + 200, 158 + _extraYOffset + _offset.Y);
			_stat3WhiteBar.Position = new Point(_offset.X + 200, 158 + _extraYOffset + _offset.Y);
			_stat4Text.Position = new Point(_offset.X + 8, 178 + _extraYOffset + _offset.Y);
			_stat4BGBar.Position = new Point(_offset.X + 200, 178 + _extraYOffset + _offset.Y);
			_stat4WhiteBar.Position = new Point(_offset.X + 200, 178 + _extraYOffset + _offset.Y);
			_stat11SepBar.Position = new Point(_offset.X + 240, 118 + _extraYOffset + _offset.Y);
			_stat12SepBar.Position = new Point(_offset.X + 280, 118 + _extraYOffset + _offset.Y);
			_stat13SepBar.Position = new Point(_offset.X + 320, 118 + _extraYOffset + _offset.Y);
			_stat14SepBar.Position = new Point(_offset.X + 360, 118 + _extraYOffset + _offset.Y);
			_stat21SepBar.Position = new Point(_offset.X + 240, 138 + _extraYOffset + _offset.Y);
			_stat22SepBar.Position = new Point(_offset.X + 280, 138 + _extraYOffset + _offset.Y);
			_stat23SepBar.Position = new Point(_offset.X + 320, 138 + _extraYOffset + _offset.Y);
			_stat24SepBar.Position = new Point(_offset.X + 360, 138 + _extraYOffset + _offset.Y);
			_stat31SepBar.Position = new Point(_offset.X + 240, 158 + _extraYOffset + _offset.Y);
			_stat32SepBar.Position = new Point(_offset.X + 280, 158 + _extraYOffset + _offset.Y);
			_stat33SepBar.Position = new Point(_offset.X + 320, 158 + _extraYOffset + _offset.Y);
			_stat34SepBar.Position = new Point(_offset.X + 360, 158 + _extraYOffset + _offset.Y);
			_stat41SepBar.Position = new Point(_offset.X + 240, 178 + _extraYOffset + _offset.Y);
			_stat42SepBar.Position = new Point(_offset.X + 280, 178 + _extraYOffset + _offset.Y);
			_stat43SepBar.Position = new Point(_offset.X + 320, 178 + _extraYOffset + _offset.Y);
			_stat44SepBar.Position = new Point(_offset.X + 360, 178 + _extraYOffset + _offset.Y);
			_statRectangle.Size = new Size(431 + WidthOffset, 150);
			_stat1WhiteBar.Size = new Size(Stat1, 10);
			_stat2WhiteBar.Size = new Size(Stat2, 10);
			_stat3WhiteBar.Size = new Size(Stat3, 10);
			_stat4WhiteBar.Size = new Size(Stat4, 10);
			int num = Size;
			if (num > 10)
			{
				num = 11;
			}
			if (MenuItems.Count != 0 && !string.IsNullOrWhiteSpace(MenuItems[CurrentSelection].Description))
			{
				_statRectangle.Position = new Point(_offset.X, 38 * num + _statRectangle.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat1Text.Position = new Point(_offset.X + 8, 38 * num + _stat1Text.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat1BGBar.Position = new Point(_offset.X + 200, 38 * num + 11 + _stat1BGBar.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat1WhiteBar.Position = new Point(_offset.X + 200, 38 * num + 11 + _stat1WhiteBar.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat2Text.Position = new Point(_offset.X + 8, 38 * num + 16 + _stat2Text.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat2BGBar.Position = new Point(_offset.X + 200, 38 * num + 25 + _stat2BGBar.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat2WhiteBar.Position = new Point(_offset.X + 200, 38 * num + 25 + _stat2WhiteBar.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat3Text.Position = new Point(_offset.X + 8, 38 * num + 32 + _stat3Text.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat3BGBar.Position = new Point(_offset.X + 200, 38 * num + 39 + _stat3BGBar.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat3WhiteBar.Position = new Point(_offset.X + 200, 38 * num + 39 + _stat3WhiteBar.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat4Text.Position = new Point(_offset.X + 8, 38 * num + 48 + _stat4Text.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat4BGBar.Position = new Point(_offset.X + 200, 38 * num + 53 + _stat4BGBar.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat4WhiteBar.Position = new Point(_offset.X + 200, 38 * num + 53 + _stat4WhiteBar.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat11SepBar.Position = new Point(_offset.X + 240, 38 * num + 11 + _stat11SepBar.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat12SepBar.Position = new Point(_offset.X + 280, 38 * num + 11 + _stat12SepBar.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat13SepBar.Position = new Point(_offset.X + 320, 38 * num + 11 + _stat13SepBar.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat14SepBar.Position = new Point(_offset.X + 360, 38 * num + 11 + _stat14SepBar.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat21SepBar.Position = new Point(_offset.X + 240, 38 * num + 25 + _stat21SepBar.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat22SepBar.Position = new Point(_offset.X + 280, 38 * num + 25 + _stat22SepBar.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat23SepBar.Position = new Point(_offset.X + 320, 38 * num + 25 + _stat23SepBar.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat24SepBar.Position = new Point(_offset.X + 360, 38 * num + 25 + _stat24SepBar.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat31SepBar.Position = new Point(_offset.X + 240, 38 * num + 39 + _stat31SepBar.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat32SepBar.Position = new Point(_offset.X + 280, 38 * num + 39 + _stat32SepBar.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat33SepBar.Position = new Point(_offset.X + 320, 38 * num + 39 + _stat33SepBar.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat34SepBar.Position = new Point(_offset.X + 360, 38 * num + 39 + _stat34SepBar.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat41SepBar.Position = new Point(_offset.X + 240, 38 * num + 53 + _stat41SepBar.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat42SepBar.Position = new Point(_offset.X + 280, 38 * num + 53 + _stat42SepBar.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat43SepBar.Position = new Point(_offset.X + 320, 38 * num + 53 + _stat43SepBar.Position.Y + _descriptionRectangle.Size.Height + 8);
				_stat44SepBar.Position = new Point(_offset.X + 360, 38 * num + 53 + _stat44SepBar.Position.Y + _descriptionRectangle.Size.Height + 8);
			}
			else
			{
				_statRectangle.Position = new Point(_offset.X, 38 * num + _statRectangle.Position.Y + 3);
				_stat1Text.Position = new Point(_offset.X + 8, 38 * num + _stat1Text.Position.Y + 3);
				_stat1BGBar.Position = new Point(_offset.X + 200, 38 * num + 11 + _stat1BGBar.Position.Y + 3);
				_stat1WhiteBar.Position = new Point(_offset.X + 200, 38 * num + 11 + _stat1WhiteBar.Position.Y + 3);
				_stat2Text.Position = new Point(_offset.X + 8, 38 * num + 16 + _stat2Text.Position.Y + 3);
				_stat2BGBar.Position = new Point(_offset.X + 200, 38 * num + 25 + _stat2BGBar.Position.Y + 3);
				_stat2WhiteBar.Position = new Point(_offset.X + 200, 38 * num + 25 + _stat2WhiteBar.Position.Y + 3);
				_stat3Text.Position = new Point(_offset.X + 8, 38 * num + 32 + _stat3Text.Position.Y + 3);
				_stat3BGBar.Position = new Point(_offset.X + 200, 38 * num + 39 + _stat3BGBar.Position.Y + 3);
				_stat3WhiteBar.Position = new Point(_offset.X + 200, 38 * num + 39 + _stat3WhiteBar.Position.Y + 3);
				_stat4Text.Position = new Point(_offset.X + 8, 38 * num + 48 + _stat4Text.Position.Y + 3);
				_stat4BGBar.Position = new Point(_offset.X + 200, 38 * num + 53 + _stat4BGBar.Position.Y + 3);
				_stat4WhiteBar.Position = new Point(_offset.X + 200, 38 * num + 53 + _stat4WhiteBar.Position.Y + 3);
				_stat11SepBar.Position = new Point(_offset.X + 240, 38 * num + 11 + _stat11SepBar.Position.Y + 3);
				_stat12SepBar.Position = new Point(_offset.X + 280, 38 * num + 11 + _stat12SepBar.Position.Y + 3);
				_stat13SepBar.Position = new Point(_offset.X + 320, 38 * num + 11 + _stat13SepBar.Position.Y + 3);
				_stat14SepBar.Position = new Point(_offset.X + 360, 38 * num + 11 + _stat14SepBar.Position.Y + 3);
				_stat21SepBar.Position = new Point(_offset.X + 240, 38 * num + 25 + _stat21SepBar.Position.Y + 3);
				_stat22SepBar.Position = new Point(_offset.X + 280, 38 * num + 25 + _stat22SepBar.Position.Y + 3);
				_stat23SepBar.Position = new Point(_offset.X + 320, 38 * num + 25 + _stat23SepBar.Position.Y + 3);
				_stat24SepBar.Position = new Point(_offset.X + 360, 38 * num + 25 + _stat24SepBar.Position.Y + 3);
				_stat31SepBar.Position = new Point(_offset.X + 240, 38 * num + 39 + _stat31SepBar.Position.Y + 3);
				_stat32SepBar.Position = new Point(_offset.X + 280, 38 * num + 39 + _stat32SepBar.Position.Y + 3);
				_stat33SepBar.Position = new Point(_offset.X + 320, 38 * num + 39 + _stat33SepBar.Position.Y + 3);
				_stat34SepBar.Position = new Point(_offset.X + 360, 38 * num + 39 + _stat34SepBar.Position.Y + 3);
				_stat41SepBar.Position = new Point(_offset.X + 240, 38 * num + 53 + _stat41SepBar.Position.Y + 3);
				_stat42SepBar.Position = new Point(_offset.X + 280, 38 * num + 53 + _stat42SepBar.Position.Y + 3);
				_stat43SepBar.Position = new Point(_offset.X + 320, 38 * num + 53 + _stat43SepBar.Position.Y + 3);
				_stat44SepBar.Position = new Point(_offset.X + 360, 38 * num + 53 + _stat44SepBar.Position.Y + 3);
			}
		}

		private int IsMouseInListItemArrows(UIMenuItem item, Point topLeft, Point safezone)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_GET_SCREEN_WIDTH_OF_DISPLAY_TEXT, "jamyfafi");
			UIResText.AddLongString(item.Text);
			SizeF screenResolutionMaintainRatio = GetScreenResolutionMaintainRatio();
			float width = screenResolutionMaintainRatio.Width;
			float height = screenResolutionMaintainRatio.Height;
			float num = width / height;
			float num2 = 1080f * num;
			int num3 = (int)(Function.Call<float>(Hash.END_TEXT_COMMAND_GET_SCREEN_WIDTH_OF_DISPLAY_TEXT, 0) * num2 * 0.35f);
			int num4 = 5 + num3 + 10;
			int width2 = 431 - num4;
			if (!IsMouseInBounds(topLeft, new Size(num4, 38)))
			{
				if (!IsMouseInBounds(new Point(topLeft.X + num4, topLeft.Y), new Size(width2, 38)))
				{
					return 0;
				}
				return 2;
			}
			return 1;
		}

		private string FormatDescription(string input)
		{
			int num = 425 + WidthOffset;
			int num2 = 0;
			string text = "";
			string[] array = input.Split(new char[1] { ' ' });
			foreach (string text2 in array)
			{
				int num3 = (int)StringMeasurer.MeasureString(text2, GTA.UI.Font.ChaletLondon, 0.35f);
				num2 += num3;
				if (num2 > num)
				{
					text = text + "\n" + text2 + " ";
					num2 = num3 + StringMeasurer.MeasureString(" ");
				}
				else
				{
					text = text + text2 + " ";
					num2 += StringMeasurer.MeasureString(" ");
				}
			}
			return text;
		}

		public void Draw()
		{
			if (!Visible)
			{
				return;
			}
			if (ControlDisablingEnabled)
			{
				LockFrame = Game.FrameCount; LockedBy = this;   // RealismSuite: lets the stuck-menu release see who holds the controls
				DisEnableControls(false);
			}
			if (_buttonsEnabled)
			{
				if (_sharedButtonsOwner != this || !_sharedButtonsPushedLoaded)
				{
					UpdateScaleform();
				}
				Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, SharedButtons.Handle, 255, 255, 255, 255, 0);
			}
			if (ScaleWithSafezone)
			{
				Function.Call(Hash.SET_SCRIPT_GFX_ALIGN, 76, 84);
				Function.Call(Hash.SET_SCRIPT_GFX_ALIGN_PARAMS, 0f, 0f, 0f, 0f);
			}
			if (EnableStats && Visible)
			{
				_stat1WhiteBar.Size = new Size(Stat1, 10);
				_stat2WhiteBar.Size = new Size(Stat2, 10);
				_stat3WhiteBar.Size = new Size(Stat3, 10);
				_stat4WhiteBar.Size = new Size(Stat4, 10);
			}
			if (reDraw)
			{
				DrawCalculations();
			}
			if (string.IsNullOrWhiteSpace(_customBanner))
			{
				if (_logo != null)
				{
					_logo.Draw();
				}
				else
				{
					_tmpRectangle?.Draw();
				}
			}
			else
			{
				Point point = (ScaleWithSafezone ? safe : new Point(0, 0));
				Sprite.DrawTexture(_customBanner, new Point(point.X + _offset.X, point.Y + _offset.Y), drawWidth);
			}
			_mainMenu.Draw();
			if (MenuItems.Count == 0)
			{
				Function.Call(Hash.RESET_SCRIPT_GFX_ALIGN);
				return;
			}
			_background.Size = backgroundSize;
			_background.Draw();
			MenuItems[CurrentSelection].Selected = true;
			if (!string.IsNullOrWhiteSpace(MenuItems[CurrentSelection].Description))
			{
				_descriptionBar.Draw();
				_descriptionRectangle.Draw();
				_descriptionText.Draw();
			}
			if (EnableStats)
			{
				_statRectangle.Draw();
				_stat1Text.Draw();
				_stat1BGBar.Draw();
				_stat1WhiteBar.Draw();
				_stat2Text.Draw();
				_stat2BGBar.Draw();
				_stat2WhiteBar.Draw();
				_stat3Text.Draw();
				_stat3BGBar.Draw();
				_stat3WhiteBar.Draw();
				_stat4Text.Draw();
				_stat4BGBar.Draw();
				_stat4WhiteBar.Draw();
				_stat11SepBar.Draw();
				_stat12SepBar.Draw();
				_stat13SepBar.Draw();
				_stat14SepBar.Draw();
				_stat21SepBar.Draw();
				_stat22SepBar.Draw();
				_stat23SepBar.Draw();
				_stat24SepBar.Draw();
				_stat31SepBar.Draw();
				_stat32SepBar.Draw();
				_stat33SepBar.Draw();
				_stat34SepBar.Draw();
				_stat41SepBar.Draw();
				_stat42SepBar.Draw();
				_stat43SepBar.Draw();
				_stat44SepBar.Draw();
				if (!Function.Call<bool>(Hash.HAS_THIS_ADDITIONAL_TEXT_LOADED, "mod_mnu", 10))
				{
					Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 10, true);
					Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "mod_mnu", 10);
				}
				if (_stat1StrCustom)
				{
					_stat1Text.Caption = _stat1Str;
				}
				else
				{
					_stat1Text.Caption = Game.GetLocalizedString("CMOD_STAT_0");
				}
				if (_stat2StrCustom)
				{
					_stat2Text.Caption = _stat2Str;
				}
				else
				{
					_stat2Text.Caption = Game.GetLocalizedString("CMOD_STAT_1");
				}
				if (_stat3StrCustom)
				{
					_stat3Text.Caption = _stat3Str;
				}
				else
				{
					_stat3Text.Caption = Game.GetLocalizedString("CMOD_STAT_2");
				}
				if (_stat4StrCustom)
				{
					_stat4Text.Caption = _stat4Str;
				}
				else
				{
					_stat4Text.Caption = Game.GetLocalizedString("CMOD_STAT_3");
				}
			}
			if (MenuItems.Count <= 10)
			{
				int num = 0;
				foreach (UIMenuItem menuItem in MenuItems)
				{
					menuItem.Position(num * 38 - 37 + _extraYOffset);
					menuItem.Draw();
					num++;
				}
			}
			else
			{
				int num2 = 0;
				for (int i = _minItem; i <= _maxItem; i++)
				{
					UIMenuItem uIMenuItem = MenuItems[i];
					uIMenuItem.Position(num2 * 38 - 37 + _extraYOffset);
					uIMenuItem.Draw();
					num2++;
				}
				_extraRectangleUp.Draw();
				_extraRectangleDown.Draw();
				_upAndDownSprite.Draw();
				if (_counterText != null)
				{
					string text = CurrentSelection + 1 + " / " + Size;
					_counterText.Caption = CounterPretext + text;
					_counterText.Draw();
				}
			}
			if (ScaleWithSafezone)
			{
				Function.Call(Hash.RESET_SCRIPT_GFX_ALIGN);
			}
		}

		internal bool ProcessMouseRows(Func<int, UIMenuItem, bool> processRow)
		{
			if (processRow == null)
			{
				throw new ArgumentNullException("processRow");
			}
			List<UIMenuItem> menuItems = MenuItems;
			int count = menuItems.Count;
			int num = ((count > 10) ? Math.Min(_maxItem, count - 1) : (count - 1));
			for (int i = Math.Max(0, _minItem); i <= num; i++)
			{
				if (menuItems != MenuItems || MenuItems.Count != count || i >= MenuItems.Count)
				{
					return false;
				}
				UIMenuItem uIMenuItem = MenuItems[i];
				if (!processRow(i, uIMenuItem))
				{
					return false;
				}
				if (menuItems != MenuItems || MenuItems.Count != count || i >= MenuItems.Count || uIMenuItem != MenuItems[i])
				{
					return false;
				}
			}
			return true;
		}

		public void ProcessMouse()
		{
			if (!Visible || _justOpened || MenuItems.Count == 0 || IsUsingController || !MouseControlsEnabled)
			{
				Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, 2, true);
				Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, 1, true);
				Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, 25, true);
				Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, 24, true);
				if (_itemsDirty)
				{
					MenuItems.Where((UIMenuItem i) => i.Hovered).ToList().ForEach(delegate(UIMenuItem i)
					{
						i.Hovered = false;
					});
					_itemsDirty = false;
				}
				return;
			}
			Point safezoneOffset = GetSafezoneBounds();
			Function.Call(Hash.SET_MOUSE_CURSOR_THIS_FRAME);
			int counter = 0;
			if (IsMouseInBounds(new Point(0, 0), new Size(30, 1080)) && MouseEdgeEnabled)
			{
				GameplayCamera.RelativeHeading += 5f;
				Function.Call(Hash.SET_MOUSE_CURSOR_STYLE, 6);
			}
			else if (IsMouseInBounds(new Point(Convert.ToInt32(GetScreenResolutionMaintainRatio().Width - 30f), 0), new Size(30, 1080)) && MouseEdgeEnabled)
			{
				GameplayCamera.RelativeHeading -= 5f;
				Function.Call(Hash.SET_MOUSE_CURSOR_STYLE, 7);
			}
			else if (MouseEdgeEnabled)
			{
				Function.Call(Hash.SET_MOUSE_CURSOR_STYLE, 1);
			}
			if (!ProcessMouseRows(delegate(int i, UIMenuItem uiMenuItem)
			{
				int x2 = _offset.X + safezoneOffset.X;
				int y = _offset.Y + 144 - 37 + _extraYOffset + counter * 38 + safezoneOffset.Y;
				int width = 431 + WidthOffset;
				if (IsMouseInBounds(new Point(x2, y), new Size(width, 38)))
				{
					uiMenuItem.Hovered = true;
					if (Game.IsControlJustPressed(GTA.Control.Attack))
					{
						if (uiMenuItem.Selected && uiMenuItem.Enabled)
						{
							if (uiMenuItem is IListItem && IsMouseInListItemArrows(uiMenuItem, new Point(x2, y), safezoneOffset) > 0)
							{
								switch (IsMouseInListItemArrows(uiMenuItem, new Point(x2, y), safezoneOffset))
								{
								case 1:
									SelectItem();
									break;
								case 2:
									GoRight();
									break;
								}
							}
							else
							{
								SelectItem();
							}
							return false;
						}
						if (!uiMenuItem.Selected)
						{
							CurrentSelection = i;
							PlayMenuSound(AUDIO_UPDOWN);
							IndexChange(CurrentSelection);
							return false;
						}
						if (!uiMenuItem.Enabled && uiMenuItem.Selected)
						{
							PlayMenuSound(AUDIO_ERROR);
							return false;
						}
					}
				}
				else
				{
					uiMenuItem.Hovered = false;
				}
				counter++;
				return true;
			}))
			{
				return;
			}
			int num = 524 + _offset.Y - 37 + _extraYOffset + safezoneOffset.Y;
			int x = safezoneOffset.X + _offset.X;
			if (Size <= 10)
			{
				return;
			}
			if (IsMouseInBounds(new Point(x, num), new Size(431 + WidthOffset, 18)))
			{
				_extraRectangleUp.Color = Color.FromArgb(255, 30, 30, 30);
				if (Game.IsControlJustPressed(GTA.Control.Attack))
				{
					if (Size > 10)
					{
						GoUpOverflow();
					}
					else
					{
						GoUp();
					}
					return;
				}
			}
			else
			{
				_extraRectangleUp.Color = Color.FromArgb(200, 0, 0, 0);
			}
			if (IsMouseInBounds(new Point(x, num + 18), new Size(431 + WidthOffset, 18)))
			{
				_extraRectangleDown.Color = Color.FromArgb(255, 30, 30, 30);
				if (Game.IsControlJustPressed(GTA.Control.Attack))
				{
					if (Size > 10)
					{
						GoDownOverflow();
					}
					else
					{
						GoDown();
					}
				}
			}
			else
			{
				_extraRectangleDown.Color = Color.FromArgb(200, 0, 0, 0);
			}
		}

		public void ProcessControl(Keys key = Keys.None)
		{
			if (!Visible)
			{
				return;
			}
			int frameCount = Game.FrameCount;
			if (_processedControlFrame == frameCount)
			{
				return;
			}
			_processedControlFrame = frameCount;
			if (_justOpened)
			{
				_justOpened = false;
				PollControls(true);
				return;
			}
			PollControls();
			if (HasControlJustBeenReleaseed(MenuControls.Back, key))
			{
				GoBack();
			}
			else
			{
				if (MenuItems.Count == 0)
				{
					return;
				}
				if (IsControlBeingPressed(MenuControls.Up, key))
				{
					if (Size > 10)
					{
						GoUpOverflow();
					}
					else
					{
						GoUp();
					}
					UpdateScaleform();
				}
				else if (IsControlBeingPressed(MenuControls.Down, key))
				{
					if (Size > 10)
					{
						GoDownOverflow();
					}
					else
					{
						GoDown();
					}
					UpdateScaleform();
				}
				else if (IsControlBeingPressed(MenuControls.Left, key))
				{
					GoLeft();
				}
				else if (IsControlBeingPressed(MenuControls.Right, key))
				{
					GoRight();
				}
				else if (HasControlJustBeenPressed(MenuControls.Select, key))
				{
					SelectItem();
				}
			}
		}

		public void ProcessKey(Keys key)
		{
			if ((from MenuControls menuControl in _menuControls
				select new List<Keys>(_keyDictionary[menuControl].Item1)).Any((List<Keys> tmpKeys) => tmpKeys.Any((Keys k) => k == key)))
			{
				ProcessControl(key);
			}
		}

		public void UpdateScaleform()
		{
			if (!Visible || !_buttonsEnabled)
			{
				return;
			}
			Scaleform sharedButtons = SharedButtons;
			_sharedButtonsOwner = this;
			_sharedButtonsPushedLoaded = sharedButtons.IsLoaded;
			sharedButtons.CallFunction("CLEAR_ALL");
			sharedButtons.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			sharedButtons.CallFunction("CREATE_CONTAINER");
			sharedButtons.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 176, 0), _selectTextLocalized);
			sharedButtons.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 177, 0), _backTextLocalized);
			int num = 2;
			foreach (InstructionalButton item in _instructionalButtons.Where((InstructionalButton button) => button.ItemBind == null || (CurrentSelection >= 0 && CurrentSelection < MenuItems.Count && MenuItems[CurrentSelection] == button.ItemBind)))
			{
				sharedButtons.CallFunction("SET_DATA_SLOT", num, item.GetButtonId(), item.Text);
				num++;
			}
			sharedButtons.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
		}

		protected virtual void IndexChange(int newindex)
		{
			reDraw = true;
			this.OnIndexChange?.Invoke(this, newindex);
		}

		protected virtual void ListChange(UIMenuListItem sender, int newindex)
		{
			this.OnListChange?.Invoke(this, sender, newindex);
		}

		protected virtual void ItemSelect(UIMenuItem selecteditem, int index)
		{
			this.OnItemSelect?.Invoke(this, selecteditem, index);
		}

		protected virtual void CheckboxChange(UIMenuCheckboxItem sender, bool Checked)
		{
			this.OnCheckboxChange?.Invoke(this, sender, Checked);
		}

		protected virtual void MenuCloseEv()
		{
			this.OnMenuClose?.Invoke(this);
		}

		protected virtual void MenuChangeEv(UIMenu newmenu, bool forward)
		{
			this.OnMenuChange?.Invoke(this, newmenu, forward);
		}

		public virtual void SetStatText(string text1 = null, string text2 = null, string text3 = null, string text4 = null)
		{
			if (text1 != null)
			{
				_stat1Str = text1;
				_stat1StrCustom = true;
			}
			if (text2 != null)
			{
				_stat2Str = text2;
				_stat2StrCustom = true;
			}
			if (text3 != null)
			{
				_stat3Str = text3;
				_stat3StrCustom = true;
			}
			if (text4 != null)
			{
				_stat4Str = text4;
				_stat4StrCustom = true;
			}
		}
	}
}
