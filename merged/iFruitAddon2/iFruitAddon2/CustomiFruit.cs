using System;
using System.Drawing;
using GTA;
using GTA.Native;
using iFruitSlotFix;

namespace iFruitAddon2
{
	public class CustomiFruit
	{
		internal static CustomiFruit Instance;

		private bool _shouldDraw = true;

		private PhoneImage _wallpaper;

		private iFruitContactCollection _contacts;

		private int _timerClose = -1;

		public Color LeftButtonColor { get; set; } = Color.Empty;


		public Color CenterButtonColor { get; set; } = Color.Empty;


		public Color RightButtonColor { get; set; } = Color.Empty;


		public SoftKeyIcon LeftButtonIcon { get; set; } = SoftKeyIcon.Blank;


		public SoftKeyIcon CenterButtonIcon { get; set; } = SoftKeyIcon.Blank;


		public SoftKeyIcon RightButtonIcon { get; set; } = SoftKeyIcon.Blank;


		public iFruitContactCollection Contacts
		{
			get
			{
				return _contacts;
			}
			set
			{
				_contacts = value;
			}
		}

		public int Handle => PhoneScript.GetCurrentScaleform().Handle;

		public CustomiFruit()
			: this(new iFruitContactCollection())
		{
		}

		public CustomiFruit(iFruitContactCollection contacts)
		{
			Instance = this;
			_contacts = contacts;
			Logger.Debug("CustomiFruit initialized with a contact collection containing " + contacts.Count + " contacts.");
		}

		public void SetTextHeader(string text)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Handle, "SET_HEADER");
			Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PHONE_NUMBER, text, -1);
			Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		public void SetSoftKeyIcon(int buttonID, SoftKeyIcon icon)
		{
			PhoneScript.GetCurrentScaleform().CallFunction("SET_SOFT_KEYS", buttonID, true, (int)icon);
		}

		public void SetSoftKeyColor(int buttonID, Color color)
		{
			PhoneScript.GetCurrentScaleform().CallFunction("SET_SOFT_KEYS_COLOUR", buttonID, (int)color.R, (int)color.G, (int)color.B);
		}

		internal void SetWallpaperTXD(string textureDict)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Handle, "SET_BACKGROUND_CREW_IMAGE");
			Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "CELL_2000");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, textureDict);
			Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		[Obsolete("Use SetWallpaper(PhoneImage) instead.")]
		public void SetWallpaper(Wallpaper wallpaper)
		{
			_wallpaper = wallpaper;
		}

		[Obsolete("Use SetWallpaper(PhoneImage) instead.")]
		public void SetWallpaper(ContactIcon icon)
		{
			_wallpaper = icon;
		}

		public void SetWallpaper(PhoneImage phoneImage)
		{
			_wallpaper = phoneImage;
		}

		public void SetWallpaper(string textureDict)
		{
			_wallpaper = new Wallpaper(textureDict);
		}

		public void Update()
		{
			bool flag = Function.Call<int>(Hash.GET_NUMBER_OF_THREADS_RUNNING_THE_SCRIPT_WITH_THIS_HASH, PhoneScript.CellphoneHandHash) > 0;
			if (flag)
			{
				if (LeftButtonIcon != SoftKeyIcon.Blank)
				{
					SetSoftKeyIcon(1, LeftButtonIcon);
				}
				if (CenterButtonIcon != SoftKeyIcon.Blank)
				{
					SetSoftKeyIcon(2, CenterButtonIcon);
				}
				if (RightButtonIcon != SoftKeyIcon.Blank)
				{
					SetSoftKeyIcon(3, RightButtonIcon);
				}
				if (_shouldDraw)
				{
					Script.Wait(0);
					if (_wallpaper != null)
					{
						SetWallpaperTXD(_wallpaper.Name);
					}
					if (LeftButtonColor != Color.Empty)
					{
						SetSoftKeyColor(1, LeftButtonColor);
					}
					if (CenterButtonColor != Color.Empty)
					{
						SetSoftKeyColor(2, CenterButtonColor);
					}
					if (RightButtonColor != Color.Empty)
					{
						SetSoftKeyColor(3, RightButtonColor);
					}
					_shouldDraw = !_shouldDraw;
				}
			}
			else
			{
				_shouldDraw = true;
			}
			if (_timerClose != -1 && _timerClose <= Game.GameTime)
			{
				Close();
				_timerClose = -1;
			}
			_contacts.Update(flag ? Handle : 0, flag);
		}

		public void Close(int timer = 0)
		{
			if (timer == 0)
			{
				Close();
			}
			else
			{
				_timerClose = Game.GameTime + timer;
			}
		}

		private void Close()
		{
			Slots.ClosePhone();
		}
	}
}
