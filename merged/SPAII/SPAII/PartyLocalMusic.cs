using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.UI;
using INMNativeUI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class PartyLocalMusic
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Predicate<string> _0024I19_002D0;

			public static Predicate<string> _0024I22_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__19_002D0(string candidate)
			{
				return string.Equals(candidate, track, StringComparison.OrdinalIgnoreCase);
			}

			[SpecialName]
			internal bool _Lambda_0024__22_002D0(string candidate)
			{
				return string.Equals(candidate, track, StringComparison.OrdinalIgnoreCase);
			}
		}

		internal const string MusicFolder = "scripts\\SPA II\\NightclubMusic";

		private const int DecodeTimeoutMs = 10000;

		private static UIMenu menu;

		private static ILocalMusicPlayer player;

		private static readonly Stopwatch opening = new Stopwatch();

		private static string track;

		private static bool ready;

		private static int nextUpdate;

		private static bool lastPaused;

		internal static bool HasSelection => player != null;

		internal static bool IsReady
		{
			get
			{
				if (player != null)
				{
					return ready;
				}
				return false;
			}
		}

		internal static string[] AvailableTracks(string folder)
		{
			return NightclubTrackRequests.AvailableTracks(folder, null);
		}

		internal static int NextTrackIndex(int count, int currentIndex)
		{
			if (count <= 0)
			{
				return -1;
			}
			if (currentIndex < 0 || currentIndex >= count)
			{
				return 0;
			}
			return checked(currentIndex + 1) % count;
		}

		internal static int VolumeAt(Vector3 position, Vector3 center, float radius)
		{
			if (!PartyService.IsPartyMusicInRange(position, center, radius))
			{
				return 0;
			}
			double num = Math.Max(10.0, Math.Min(120.0, radius));
			double num2 = Math.Sqrt(position.DistanceToSquared(center));
			return checked((int)Math.Round(Math.Max(0.0, 700.0 * (1.0 - num2 / num))));
		}

		internal static bool ShouldPause(bool active, bool enabled, bool paused, bool clubOwnsAudio, bool inRange)
		{
			if (active && enabled && !paused && !clubOwnsAudio)
			{
				return !inRange;
			}
			return true;
		}

		internal static void OpenMenu(UIMenu parent)
		{
			if (!PartyService.IsPartyActive)
			{
				return;
			}
			try
			{
				string[] array = AvailableTracks("scripts\\SPA II\\NightclubMusic");
				if (menu == null)
				{
					menu = new UIMenu("", "PARTY / OWN MP3S", new Point(0, -107))
					{
						MouseEdgeEnabled = false
					};
					menu.SetBannerType(Helper.MenuBanner);
					Helper.MenuPool.Add(menu);
					menu.OnItemSelect += SelectItem;
				}
				menu.Clear();
				menu.AddItem(new UIMenuItem("Return to DJ / radio", "Stop your MP3 and use the normal party music.")
				{
					Tag = "@radio"
				});
				menu.AddItem(new UIMenuItem("Next track", "Play the next MP3 in title order, wrapping after the last track.")
				{
					Tag = "@next",
					Enabled = (array.Length > 0)
				});
				string[] array2 = array;
				foreach (string text in array2)
				{
					UIMenuItem uIMenuItem = new UIMenuItem(Path.GetFileNameWithoutExtension(text), "Play this MP3 at the party. Nightclub music takes priority inside a club.")
					{
						Tag = text
					};
					if (string.Equals(text, track, StringComparison.OrdinalIgnoreCase))
					{
						uIMenuItem.SetRightLabel("Playing");
					}
					menu.AddItem(uIMenuItem);
				}
				if (array.Length == 0)
				{
					menu.AddItem(new UIMenuItem("No MP3s found", "Put MP3 files directly in scripts\\SPA II\\NightclubMusic.")
					{
						Enabled = false
					});
				}
				menu.ParentMenu = parent;
				menu.RefreshIndex();
				if (parent != null)
				{
					parent.Visible = false;
				}
				menu.Visible = true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Party MP3 list: " + ex2.Message);
				Notification.Show("The party could not read the MP3 folder.");
				ProjectData.ClearProjectError();
			}
		}

		private static void SelectItem(UIMenu sender, UIMenuItem item, int index)
		{
			if (sender != menu || !sender.Visible || !PartyService.IsPartyActive || Game.IsPaused || StoryLoadGuard.WorldLoading() || Game.IsMissionActive)
			{
				return;
			}
			string text = item.Tag as string;
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			if (Operators.CompareString(text, "@radio", false) == 0)
			{
				PartyService.ResumePartyDjMusic();
				return;
			}
			try
			{
				string[] array = AvailableTracks("scripts\\SPA II\\NightclubMusic");
				if (Operators.CompareString(text, "@next", false) == 0)
				{
					int num = NextTrackIndex(array.Length, Array.FindIndex(array, (_Closure_0024__._0024I19_002D0 != null) ? _Closure_0024__._0024I19_002D0 : (_Closure_0024__._0024I19_002D0 = _Closure_0024__._0024I._Lambda_0024__19_002D0)));
					if (num < 0)
					{
						return;
					}
					text = array[num];
				}
				else if (!array.Contains(text, StringComparer.OrdinalIgnoreCase))
				{
					throw new IOException("The selected MP3 is no longer in the music folder.");
				}
				PartyService.EnableLocalPartyMusic();
				StartTrack(text);
				if (menu != null)
				{
					menu.Visible = false;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Fail(ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		private static void StartTrack(string path)
		{
			StopPlayer();
			if (!File.Exists(path) || !string.Equals(Path.GetExtension(path), ".mp3", StringComparison.OrdinalIgnoreCase))
			{
				throw new IOException("The selected MP3 is missing or invalid.");
			}
			track = Path.GetFullPath(path);
			player = CreatePlayer(track);
			opening.Restart();
			nextUpdate = 0;
			Logger.Log("Party MP3 opening: " + Path.GetFileName(track));
		}

		private static ILocalMusicPlayer CreatePlayer(string path)
		{
			return new NightclubWindowsMedia(path, false);
		}

		internal static void Tick(bool active, bool enabled, Vector3 center, float radius, Ped actor, bool clubOwnsAudio)
		{
			if (!active || !enabled || (object)actor == null || !actor.Exists() || actor.IsDead || StoryLoadGuard.WorldLoading() || Game.IsMissionActive)
			{
				Reset();
			}
			else
			{
				if (player == null)
				{
					return;
				}
				try
				{
					string failure = null;
					switch (player.ReadState(ref failure))
					{
					case 0:
						player.SetPaused(true);
						if (opening.ElapsedMilliseconds >= 10000)
						{
							Fail("Windows media decoder opening timed out.");
						}
						return;
					case 3:
						Fail("Windows media decoder: " + failure);
						return;
					case 2:
					{
						string[] array = AvailableTracks("scripts\\SPA II\\NightclubMusic");
						int num = NextTrackIndex(array.Length, Array.FindIndex(array, (_Closure_0024__._0024I22_002D0 != null) ? _Closure_0024__._0024I22_002D0 : (_Closure_0024__._0024I22_002D0 = _Closure_0024__._0024I._Lambda_0024__22_002D0)));
						if (num < 0)
						{
							StopPlayer();
						}
						else
						{
							StartTrack(array[num]);
						}
						return;
					}
					}
					if (!ready)
					{
						ready = true;
						opening.Reset();
						Logger.Log("Party MP3 decoder opened: " + Path.GetFileName(track) + "; audibility still requires a Windows/GTA test.");
					}
					int num2 = VolumeAt(actor.Position, center, radius);
					bool flag = ShouldPause(active, enabled, Game.IsPaused, clubOwnsAudio, num2 > 0);
					if (Game.GameTime >= nextUpdate || flag != lastPaused)
					{
						nextUpdate = checked(Game.GameTime + 200);
						player.SetLevels((!flag) ? num2 : 0, NightclubTrackRequests.PanAt(actor.Position, GameplayCamera.Direction, center));
						player.SetPaused(flag);
						lastPaused = flag;
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Fail(ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
		}

		private static void Fail(string reason)
		{
			Logger.Log("Party MP3 failed: " + (track ?? "(no track)") + "; " + reason);
			StopPlayer();
			Notification.Show("This MP3 could not be played. The normal party music is active.");
		}

		private static void StopPlayer()
		{
			ILocalMusicPlayer localMusicPlayer = player;
			player = null;
			track = null;
			ready = false;
			nextUpdate = 0;
			lastPaused = false;
			opening.Reset();
			if (localMusicPlayer != null)
			{
				try
				{
					localMusicPlayer.Dispose();
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Party MP3 cleanup: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
		}

		internal static void Reset()
		{
			if (menu != null)
			{
				menu.Visible = false;
			}
			StopPlayer();
		}
	}
}
