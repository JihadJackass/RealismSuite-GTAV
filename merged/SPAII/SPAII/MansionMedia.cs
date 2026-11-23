using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using INMNativeUI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class MansionMedia
	{
		internal class Screen
		{
			public Mansion Home;

			public Prop Prop;

			public IntPtr Address;

			public int ModelHash;

			public bool Cinema;

			public Vector3 Minimum;

			public Vector3 Maximum;

			public bool BoundsLoaded;
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__38_002D0
		{
			public int _0024VB_0024Local_handle;

			public _Closure_0024__38_002D0(_Closure_0024__38_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_handle = arg0._0024VB_0024Local_handle;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(Screen s)
			{
				return s.Prop.Handle == _0024VB_0024Local_handle;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__43_002D0
		{
			public bool _0024VB_0024Local_requireVisible;

			public Vector3 _0024VB_0024Local_position;

			[SpecialName]
			internal bool _Lambda_0024__0(Screen s)
			{
				if (Valid(s))
				{
					if (_0024VB_0024Local_requireVisible)
					{
						return Visible(s, _0024VB_0024Local_position);
					}
					return true;
				}
				return false;
			}

			[SpecialName]
			internal float _Lambda_0024__1(Screen s)
			{
				return s.Prop.Position.DistanceToSquared(_0024VB_0024Local_position);
			}
		}

		private static readonly Stopwatch clock = Stopwatch.StartNew();

		private static readonly Dictionary<Mansion, long> scans = new Dictionary<Mansion, long>();

		private static readonly Dictionary<Mansion, List<Screen>> screens = new Dictionary<Mansion, List<Screen>>();

		private static readonly string[] tvModelNames = new string[2] { "m25_2_prop_m52_mansiontv", "m25_2_prop_m52_mansiontvscreen" };

		private const string CinemaModel = "m25_2_prop_m52_mansioncinescreen";

		private static Screen active;

		private static Screen menuScreen;

		private static Mansion menuHome;

		private static Ped menuActor;

		private static IntPtr menuActorAddress;

		private static int menuActorModel;

		private static UIMenu menu;

		private static UIMenuListItem displayItem;

		private static UIMenuListItem volumeItem;

		private static UIMenuCheckboxItem subtitlesItem;

		private static int targetId;

		private static string targetName;

		private static TvRenderSurface videoSurface;

		private static bool decoderStarted;

		private static int pendingProgram;

		private static bool fullScreen;

		private static int volumeChoice = 2;

		private static bool subtitles = true;

		private static int lastFrame = -1;

		private static readonly HashSet<int> invalidBoundsReported = new HashSet<int>();

		public static bool OwnsDecoder
		{
			get
			{
				if (decoderStarted)
				{
					return active != null;
				}
				return false;
			}
		}

		public static bool IsFullscreenWatching => fullScreen;

		public static string TargetFor(bool cinema)
		{
			if (!cinema)
			{
				return "prop_m52_mansiontv";
			}
			return "prop_m52_cinescreen";
		}

		public static string DrawableFor(int sourceHash, bool cinema)
		{
			if (cinema)
			{
				return (sourceHash == new Model("m25_2_prop_m52_mansioncinescreen").Hash) ? "m25_2_prop_m52_mansioncinescreen" : null;
			}
			string[] array = tvModelNames;
			foreach (string text in array)
			{
				if (sourceHash == new Model(text).Hash)
				{
					return text;
				}
			}
			return null;
		}

		public static string ProgramPlaylist(int index)
		{
			switch (index)
			{
			case 0:
				return "PL_STD_CNT";
			case 1:
				return "PL_STD_WZL";
			case 2:
				return "PL_CINEMA_CARTOON";
			case 3:
				return "PL_CINEMA_ARTHOUSE";
			case 4:
				return "PL_CINEMA_ACTION";
			default:
				return null;
			}
		}

		public static Vector3 ScreenPosition(Mansion m, bool cinema)
		{
			return Helper.ToVector3(cinema ? MansionAutoLayout.MainPoint(m, -14.2997f, 5.959f, -3.3231f) : MansionAutoLayout.MainPoint(m, -8.1482f, -8.9791f, 1.9833f));
		}

		public static bool WithinReach(Vector3 player, Vector3 screen, bool cinema)
		{
			if (!PartyPlacement.FinitePoint(player) || !PartyPlacement.FinitePoint(screen))
			{
				return false;
			}
			Vector3 vector = player - screen;
			float num = 5f;
			return Math.Abs(vector.Z) <= 3.8f && vector.X * vector.X + vector.Y * vector.Y <= num * num;
		}

		private static bool Valid(Screen s)
		{
			if (s != null && (object)s.Prop != null && s.Prop.Exists() && s.Prop.Model.Hash == s.ModelHash)
			{
				return s.Prop.MemoryAddress == s.Address;
			}
			return false;
		}

		private static bool CanUse(Mansion m)
		{
			if (m != null && m.Enabled && m.IplsLoaded && m.IsOwnedByPlayer() && (object)Helper.PP != null && Helper.PP.Exists() && !Helper.PP.IsDead)
			{
				return !Helper.PP.IsInVehicle();
			}
			return false;
		}

		private static void Scan(Mansion m)
		{
			long value;
			if (!CanUse(m) || (scans.TryGetValue(m, out value) && clock.ElapsedMilliseconds < value))
			{
				return;
			}
			checked
			{
				scans[m] = clock.ElapsedMilliseconds + 750;
				List<Screen> list = new List<Screen>();
				if (active != null && active.Home == m && Valid(active))
				{
					list.Add(active);
				}
				_Closure_0024__38_002D0 closure_0024__38_002D = default(_Closure_0024__38_002D0);
				foreach (string item in tvModelNames.Concat(new string[1] { "m25_2_prop_m52_mansioncinescreen" }))
				{
					Model model = new Model(item);
					bool cinema = Operators.CompareString(item, "m25_2_prop_m52_mansioncinescreen", false) == 0;
					Vector3[] array = new Vector3[2]
					{
						Helper.PP.Position,
						ScreenPosition(m, cinema)
					};
					for (int i = 0; i < array.Length; i++)
					{
						Vector3 vector = array[i];
						closure_0024__38_002D = new _Closure_0024__38_002D0(closure_0024__38_002D);
						closure_0024__38_002D._0024VB_0024Local_handle = Function.Call<int>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, vector.X, vector.Y, vector.Z, 18f, model.Hash, false, false, true);
						if (closure_0024__38_002D._0024VB_0024Local_handle != 0 && !list.Any(closure_0024__38_002D._Lambda_0024__0) && (videoSurface == null || (object)videoSurface.Surface == null || videoSurface.Surface.Handle != closure_0024__38_002D._0024VB_0024Local_handle))
						{
							Prop prop = Entity.FromHandle(closure_0024__38_002D._0024VB_0024Local_handle) as Prop;
							if ((object)prop != null && prop.Exists() && prop.Model.Hash == model.Hash && !(prop.Position.DistanceTo(MansionAutoLayout.MainAnchor(m)) > 80f))
							{
								list.Add(new Screen
								{
									Home = m,
									Prop = prop,
									Address = prop.MemoryAddress,
									ModelHash = model.Hash,
									Cinema = cinema
								});
							}
						}
					}
				}
				screens[m] = list;
			}
		}

		private static bool Visible(Screen s, Vector3 position)
		{
			if (!Valid(s) || !WithinReach(position, s.Prop.Position, s.Cinema))
			{
				return false;
			}
			if (!s.BoundsLoaded)
			{
				s.Prop.Model.GetDimensions(out s.Minimum, out s.Maximum);
				s.BoundsLoaded = true;
			}
			if (!ScreenBoundsValid(s.Minimum, s.Maximum))
			{
				if (invalidBoundsReported.Add(s.ModelHash))
				{
					Logger.Log("Mansion media: invalid screen bounds; model=" + s.ModelHash + "; minimum=" + s.Minimum.ToString() + "; maximum=" + s.Maximum.ToString());
				}
				return false;
			}
			Vector3 vector = position + new Vector3(0f, 0f, 0.35f);
			Vector3 point = ScreenFrontPoint(Function.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_GIVEN_WORLD_COORDS, s.Prop.Handle, vector.X, vector.Y, vector.Z), s.Minimum, s.Maximum);
			if (!PartyPlacement.FinitePoint(point))
			{
				return false;
			}
			Vector3 point2 = Function.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_IN_WORLD_COORDS, s.Prop.Handle, point.X, point.Y, point.Z);
			if (!PartyPlacement.FinitePoint(point2))
			{
				return false;
			}
			int num = Function.Call<int>(Hash.START_EXPENSIVE_SYNCHRONOUS_SHAPE_TEST_LOS_PROBE, vector.X, vector.Y, vector.Z, point2.X, point2.Y, point2.Z, 81, ((object)Helper.PP != null) ? Helper.PP.Handle : 0, 4);
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			OutputArgument outputArgument3 = new OutputArgument();
			OutputArgument outputArgument4 = new OutputArgument();
			int num2 = Function.Call<int>(Hash.GET_SHAPE_TEST_RESULT, num, outputArgument, outputArgument2, outputArgument3, outputArgument4);
			if (num2 != 2)
			{
				return false;
			}
			int result = outputArgument4.GetResult<int>();
			int ownedSurface = 0;
			if (active == s && videoSurface != null && videoSurface.OwnsSurface(result))
			{
				ownedSurface = result;
			}
			return ScreenSightClear(num2, outputArgument.GetResult<int>() != 0, result, s.Prop.Handle, ownedSurface);
		}

		public static bool ScreenBoundsValid(Vector3 minimum, Vector3 maximum)
		{
			if (!PartyPlacement.FinitePoint(minimum) || !PartyPlacement.FinitePoint(maximum))
			{
				return false;
			}
			Vector3 vector = maximum - minimum;
			if (vector.X < 0f || vector.Y < 0f || vector.Z < 0f || vector.LengthSquared() > 2500f)
			{
				return false;
			}
			return checked(((vector.X > 0.001f) ? 1 : 0) + ((vector.Y > 0.001f) ? 1 : 0) + ((vector.Z > 0.001f) ? 1 : 0)) >= 2 && Math.Abs(minimum.X) < 1000f && Math.Abs(minimum.Y) < 1000f && Math.Abs(minimum.Z) < 1000f && Math.Abs(maximum.X) < 1000f && Math.Abs(maximum.Y) < 1000f && Math.Abs(maximum.Z) < 1000f;
		}

		public static Vector3 ScreenFrontPoint(Vector3 viewer, Vector3 minimum, Vector3 maximum)
		{
			Vector3 result = new Vector3(float.NaN, float.NaN, float.NaN);
			if (!PartyPlacement.FinitePoint(viewer) || !ScreenBoundsValid(minimum, maximum))
			{
				return result;
			}
			if (viewer.X >= minimum.X && viewer.X <= maximum.X && viewer.Y >= minimum.Y && viewer.Y <= maximum.Y && viewer.Z >= minimum.Z && viewer.Z <= maximum.Z)
			{
				return result;
			}
			Vector3 vector = (minimum + maximum) * 0.5f;
			Vector3 vector2 = (maximum - minimum) * 0.5f;
			Vector3 vector3 = viewer - vector;
			double num = double.PositiveInfinity;
			if (Math.Abs(vector3.X) > 1E-06f)
			{
				num = Math.Min(num, (double)vector2.X / Math.Abs((double)vector3.X));
			}
			if (Math.Abs(vector3.Y) > 1E-06f)
			{
				num = Math.Min(num, (double)vector2.Y / Math.Abs((double)vector3.Y));
			}
			if (Math.Abs(vector3.Z) > 1E-06f)
			{
				num = Math.Min(num, (double)vector2.Z / Math.Abs((double)vector3.Z));
			}
			if (double.IsInfinity(num) || num < 0.0 || num >= 1.0)
			{
				return result;
			}
			Vector3 vector4 = vector + vector3 * (float)num;
			Vector3 vector5 = viewer - vector4;
			double num2 = Math.Sqrt((double)vector5.X * (double)vector5.X + (double)vector5.Y * (double)vector5.Y + (double)vector5.Z * (double)vector5.Z);
			if (num2 <= 1E-06 || double.IsInfinity(num2))
			{
				return result;
			}
			float num3 = (float)(Math.Min(0.08, num2 * 0.5) / num2);
			return vector4 + vector5 * num3;
		}

		public static bool ScreenSightClear(int result, bool didHit, int hitEntity, int screenEntity, int ownedSurface)
		{
			if (result == 2 && screenEntity > 0)
			{
				if (didHit && hitEntity != screenEntity)
				{
					if (ownedSurface > 0)
					{
						return hitEntity == ownedSurface;
					}
					return false;
				}
				return true;
			}
			return false;
		}

		private static Screen Nearest(Mansion m, Vector3 position, bool requireVisible)
		{
			_Closure_0024__43_002D0 closure_0024__43_002D = new _Closure_0024__43_002D0();
			closure_0024__43_002D._0024VB_0024Local_position = position;
			closure_0024__43_002D._0024VB_0024Local_requireVisible = requireVisible;
			Scan(m);
			List<Screen> value = null;
			if (!screens.TryGetValue(m, out value))
			{
				return null;
			}
			return value.Where(closure_0024__43_002D._Lambda_0024__0).OrderBy(closure_0024__43_002D._Lambda_0024__1).FirstOrDefault();
		}

		public static bool IsNear(Mansion m, Vector3 position)
		{
			if (CanUse(m))
			{
				return Nearest(m, position, true) != null;
			}
			return false;
		}

		public static bool Tick(Mansion m)
		{
			if (!CanUse(m) || Helper.MenuPool.IsAnyMenuOpen() || PartyService.IsPlayerActionActive || MansionFeatures.IsWorkoutActive || PartyPlayerActions.IsKissing || fullScreen)
			{
				return false;
			}
			Screen screen = Nearest(m, Helper.PP.Position, true);
			if (screen == null)
			{
				return false;
			}
			if (!ShowQuietHelp(screen.Cinema ? "Press ~INPUT_CONTEXT~ to choose a cinema film." : "Press ~INPUT_CONTEXT~ for television channels and films."))
			{
				return false;
			}
			if (Game.IsControlJustReleased(Control.Context) || Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_RELEASED, 0, 51))
			{
				OpenScreen(m, screen, null);
			}
			return true;
		}

		public static bool MayPresentHelp(bool anyHelpVisible, bool ourHelpVisible)
		{
			if (anyHelpVisible)
			{
				return ourHelpVisible;
			}
			return true;
		}

		private static bool ShowQuietHelp(string text)
		{
			return Helper.ShowPersistentHelp(text);
		}

		public static void Open(Mansion m, UIMenu parent)
		{
			if (CanUse(m))
			{
				OpenScreen(m, Nearest(m, Helper.PP.Position, true), parent);
			}
		}

		private static void EnsureMenu()
		{
			if (menu == null)
			{
				menu = new UIMenu("", "MANSION TELEVISION & CINEMA", new Point(0, -107));
				menu.SoundEnabled = false;
				menu.SetBannerType(Helper.MenuBanner);
				menu.MouseEdgeEnabled = false;
				Helper.MenuPool.Add(menu);
				menu.OnItemSelect += Selected;
				menu.OnListChange += ListChanged;
				menu.OnCheckboxChange += CheckboxChanged;
			}
		}

		private static void OpenScreen(Mansion m, Screen selected, UIMenu parent)
		{
			if (PartyService.IsPlayerActionActive || MansionFeatures.IsWorkoutActive || PartyPlayerActions.IsKissing)
			{
				return;
			}
			if (selected == null || !Visible(selected, Helper.PP.Position))
			{
				Notification.Show("The screen is not streamed in yet. Approach the television or home cinema and try again.");
				return;
			}
			EnsureMenu();
			menuHome = m;
			menuScreen = selected;
			menuActor = Helper.PP;
			menuActorAddress = Helper.PP.MemoryAddress;
			menuActorModel = Helper.PP.Model.Hash;
			menu.ParentMenu = parent;
			if (parent != null)
			{
				parent.Visible = false;
			}
			menu.Clear();
			menu.Subtitle.Caption = (selected.Cinema ? "HOME CINEMA" : "MANSION TELEVISION");
			displayItem = new UIMenuListItem("Screen", new List<object> { "Television", "Home cinema" }, selected.Cinema ? 1 : 0);
			int num = 0;
			do
			{
				string[] array = new string[5] { "CNT", "Weazel", "The Loneliest Robot in Great Britain", "Capolavoro", "Meltdown" };
				menu.AddItem(new UIMenuItem(array[num], (num < 2) ? "Play this GTA television channel." : "Watch the original GTA story-mode cinema film from the beginning.")
				{
					Tag = num
				});
				num = checked(num + 1);
			}
			while (num <= 4);
			volumeItem = new UIMenuListItem("Volume", new List<object> { "Muted", "Quiet", "Normal", "Loud" }, volumeChoice);
			subtitlesItem = new UIMenuCheckboxItem("Movie subtitles", subtitles);
			menu.AddItem(volumeItem);
			menu.AddItem(subtitlesItem);
			menu.AddItem(new UIMenuItem("Watch fullscreen", "Start a channel or film first. Move or press Back to return to the room.")
			{
				Tag = "Fullscreen"
			});
			menu.AddItem(new UIMenuItem("Switch off", "Stop the television or cinema playback.")
			{
				Tag = "Off"
			});
			menu.RefreshIndex();
			menu.Visible = true;
		}

		private static void ListChanged(UIMenu sender, UIMenuListItem item, int index)
		{
			if (!MenuSessionValid())
			{
				CloseMenu();
			}
			else if (item == volumeItem)
			{
				volumeChoice = Math.Max(0, Math.Min(3, index));
				if (active != null)
				{
					Function.Call(Hash.SET_TV_VOLUME, Volume());
				}
			}
		}

		private static void CheckboxChanged(UIMenu sender, UIMenuCheckboxItem item, bool value)
		{
			if (!MenuSessionValid())
			{
				CloseMenu();
			}
			else if (item == subtitlesItem)
			{
				subtitles = value;
				if (active != null)
				{
					Function.Call(Hash.ENABLE_MOVIE_SUBTITLES, subtitles);
				}
			}
		}

		private static float Volume()
		{
			return (new float[4] { -100f, -20f, -10f, 0f })[volumeChoice];
		}

		private static void Selected(UIMenu sender, UIMenuItem item, int index)
		{
			if (!MenuSessionValid())
			{
				CloseMenu();
			}
			else if (item.Tag is int)
			{
				StartPlayback(menuScreen, Conversions.ToInteger(item.Tag));
			}
			else if (Operators.CompareString(item.Tag as string, "Off", false) == 0)
			{
				Cleanup();
			}
			else if (Operators.CompareString(item.Tag as string, "Fullscreen", false) == 0)
			{
				if (active == null)
				{
					Notification.Show("Select a channel or film first.");
					return;
				}
				menu.Visible = false;
				fullScreen = true;
			}
		}

		private static void StartPlayback(Screen selected, int program)
		{
			try
			{
				StartPlaybackCore(selected, program);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Mansion media start: " + ex2.Message);
				StopPlayback(true);
				Notification.Show("This screen could not start playback. Please try again.");
				ProjectData.ClearProjectError();
			}
		}

		private static void StartPlaybackCore(Screen selected, int program)
		{
			string text = ProgramPlaylist(program);
			if (!Valid(selected) || !CanUse(selected.Home) || text == null)
			{
				return;
			}
			string text2 = DrawableFor(selected.ModelHash, selected.Cinema);
			if (text2 == null)
			{
				Notification.Show("This television layout has no matching video surface.");
				return;
			}
			StopPlayback(true);
			if (ApartmentProps.TVOn)
			{
				ApartmentProps.ReleaseTellyNow();
			}
			if (!Visible(selected, Helper.PP.Position))
			{
				Notification.Show("Use the television or cinema screen in front of you.");
				return;
			}
			targetName = TargetFor(selected.Cinema);
			videoSurface = new TvRenderSurface(selected.Prop, text2, targetName);
			active = selected;
			pendingProgram = program;
			decoderStarted = false;
			Logger.Log("Mansion video surface queued: " + targetName + "; playlist=" + text);
		}

		private static void StartDecoder()
		{
			if (Function.Call<int>(Hash.GET_TV_CHANNEL) >= 0)
			{
				throw new InvalidOperationException("Another decoder became active while the video surface was loading.");
			}
			targetId = videoSurface.RenderId;
			Function.Call(Hash.SET_TV_CHANNEL, -1);
			Function.Call(Hash.SET_TV_CHANNEL_PLAYLIST, 0, ProgramPlaylist(pendingProgram), true);
			Function.Call(Hash.SET_TV_CHANNEL, 0);
			decoderStarted = true;
			Function.Call(Hash.SET_TV_VOLUME, Volume());
			Function.Call(Hash.ENABLE_MOVIE_SUBTITLES, subtitles);
			Function.Call(Hash.SET_TV_AUDIO_FRONTEND, false);
			Function.Call(Hash.ATTACH_TV_AUDIO_TO_ENTITY, videoSurface.Surface.Handle);
			Logger.Log("Mansion video surface ready: target=" + targetName + "; opaqueRenderId=" + targetId + "; playlist=" + ProgramPlaylist(pendingProgram));
		}

		public static void FrameTick()
		{
			if (lastFrame == Game.FrameCount)
			{
				return;
			}
			lastFrame = Game.FrameCount;
			ValidateMenu();
			if (active == null)
			{
				return;
			}
			try
			{
				if (!CanUse(active.Home) || !Valid(active) || Helper.PP.Position.DistanceTo(active.Prop.Position) > 40f || ApartmentProps.TVOn || (decoderStarted && Function.Call<int>(Hash.GET_TV_CHANNEL) != 0))
				{
					StopPlayback(!ApartmentProps.TVOn);
					return;
				}
				videoSurface.Advance();
				if (videoSurface.Failed)
				{
					Notification.Show("The video screen could not finish loading. Please try again.");
					StopPlayback(true);
				}
				else
				{
					if (!videoSurface.Ready)
					{
						return;
					}
					if (!decoderStarted)
					{
						StartDecoder();
					}
					targetId = videoSurface.RenderId;
					Render(targetId);
					if (!fullScreen)
					{
						return;
					}
					if (Helper.MenuPool.IsAnyMenuOpen() || PartyService.IsPlayerActionActive || MansionFeatures.IsWorkoutActive || Game.IsControlJustPressed(Control.FrontendCancel) || Game.IsControlJustPressed(Control.PhoneCancel) || PartyPlayerActions.MovementRequested())
					{
						fullScreen = false;
						return;
					}
					Render(Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
					Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
					Control[] array = new Control[4]
					{
						Control.Attack,
						Control.Aim,
						Control.Enter,
						Control.Jump
					};
					for (int i = 0; i < array.Length; i = checked(i + 1))
					{
						Game.DisableControlThisFrame(array[i]);
					}
					ShowQuietHelp("Cinema: move or press ~INPUT_FRONTEND_CANCEL~ to return to the room.");
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Mansion media frame: " + ex2.Message);
				StopPlayback(true);
				ProjectData.ClearProjectError();
			}
		}

		public static bool MenuMayRemain(bool contextValid, bool actorMatches, bool screenValid, bool physicallyVisible)
		{
			if (contextValid && actorMatches && screenValid)
			{
				return physicallyVisible;
			}
			return false;
		}

		private static bool MenuSessionValid()
		{
			if (!CanUse(menuHome) || !Valid(menuScreen) || menuScreen.Home != menuHome)
			{
				return false;
			}
			bool actorMatches = (object)menuActor != null && menuActor.Exists() && menuActor.Handle == Helper.PP.Handle && menuActor.MemoryAddress == menuActorAddress && menuActor.Model.Hash == menuActorModel;
			return MenuMayRemain(true, actorMatches, true, Visible(menuScreen, Helper.PP.Position));
		}

		private static void ValidateMenu()
		{
			if (menu != null && menu.Visible && !MenuSessionValid())
			{
				CloseMenu();
			}
		}

		private static void CloseMenu()
		{
			if (menu != null)
			{
				menu.Visible = false;
			}
			menuScreen = null;
			menuHome = null;
			menuActor = null;
			menuActorAddress = IntPtr.Zero;
			menuActorModel = 0;
		}

		private static void Render(int renderId)
		{
			try
			{
				Function.Call(Hash.SET_TEXT_RENDER_ID, renderId);
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, true);
				Function.Call(Hash.DRAW_TV_CHANNEL, 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255);
			}
			finally
			{
				Function.Call(Hash.SET_TEXT_RENDER_ID, Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 0);
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, false);
			}
		}

		private static void StopPlayback(bool stopDecoder)
		{
			try
			{
				if (decoderStarted && active != null && stopDecoder && !ApartmentProps.TVOn && Function.Call<int>(Hash.GET_TV_CHANNEL) == 0)
				{
					Function.Call(Hash.SET_TV_CHANNEL, -1);
					Function.Call(Hash.ENABLE_MOVIE_SUBTITLES, false);
				}
			}
			finally
			{
				active = null;
				fullScreen = false;
				targetId = 0;
				if (videoSurface != null)
				{
					videoSurface.Dispose();
					videoSurface = null;
				}
				decoderStarted = false;
				targetName = null;
			}
		}

		public static void Cleanup()
		{
			StopPlayback(true);
			CloseMenu();
			scans.Clear();
			screens.Clear();
			invalidBoundsReported.Clear();
		}
	}
}
