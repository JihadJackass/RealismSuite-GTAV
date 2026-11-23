using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using GTA;
using GTA.Math;
using GTA.UI;
using INMNativeUI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class NightclubTrackRequests
	{
		private sealed class SpatialLevels
		{
			private readonly string deviceAlias;

			private bool useStereo;

			private int lastLeft;

			private int lastRight;

			public SpatialLevels(string deviceAlias)
			{
				useStereo = true;
				lastLeft = -1000;
				lastRight = -1000;
				this.deviceAlias = deviceAlias;
			}

			public bool Apply(int volume, float pan)
			{
				checked
				{
					if (useStereo)
					{
						int num = (int)Math.Round((float)volume * (1f - Math.Max(0f, pan)));
						int num2 = (int)Math.Round((float)volume * (1f - Math.Max(0f, 0f - pan)));
						if (Math.Abs(num - lastLeft) < 12 && Math.Abs(num2 - lastRight) < 12)
						{
							return true;
						}
						if (MediaCommand($"setaudio {deviceAlias} left volume to {num}") && MediaCommand($"setaudio {deviceAlias} right volume to {num2}"))
						{
							lastLeft = num;
							lastRight = num2;
							return true;
						}
						useStereo = false;
						lastLeft = -1000;
					}
					if (Math.Abs(volume - lastLeft) < 12)
					{
						return true;
					}
					if (!MediaCommand($"setaudio {deviceAlias} volume to {volume}"))
					{
						return false;
					}
					lastLeft = volume;
					return true;
				}
			}
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<string, bool> _0024I29_002D0;

			public static Func<string, IEnumerable<string>> _0024I29_002D1;

			public static Func<string, bool> _0024I29_002D2;

			public static Func<string, string> _0024I29_002D3;

			public static Func<string, string> _0024I29_002D4;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__29_002D0(string folderPath)
			{
				return Directory.Exists(folderPath);
			}

			[SpecialName]
			internal IEnumerable<string> _Lambda_0024__29_002D1(string folderPath)
			{
				return Directory.EnumerateFiles(folderPath, "*", SearchOption.TopDirectoryOnly);
			}

			[SpecialName]
			internal bool _Lambda_0024__29_002D2(string candidate)
			{
				return string.Equals(Path.GetExtension(candidate), ".mp3", StringComparison.OrdinalIgnoreCase);
			}

			[SpecialName]
			internal string _Lambda_0024__29_002D3(string candidate)
			{
				return Path.GetFullPath(candidate);
			}

			[SpecialName]
			internal string _Lambda_0024__29_002D4(string candidate)
			{
				return Path.GetFileName(candidate);
			}
		}

		private const string MusicFolder = "scripts\\SPA II\\NightclubMusic";

		private const string ReferenceFolder = "scripts\\AfterHoursSP\\Audio";

		private const string ReferenceCrowd = "scripts\\AfterHoursSP\\Crowd.mp3";

		private const int TrackLimit = 80;

		private const int DecodeTimeoutMs = 10000;

		private static readonly Vector3 Booth = new Vector3(-1603.96f, -3012.73f, -78.79f);

		private static readonly Vector3 DanceSpeaker = new Vector3(-1596.004f, -3012.315f, -74.006f);

		private static readonly Vector3 OfficeSpeaker = new Vector3(-1618.019f, -3013.427f, -76.205f);

		private static readonly string PlayerAlias = "spaiiclub" + Guid.NewGuid().ToString("N");

		private static readonly string CrowdAlias = PlayerAlias + "crowd";

		private static UIMenu menu;

		private static ExpansionSession menuSession;

		private static ExpansionSession playbackSession;

		private static bool mediaOpen;

		private static bool crowdOpen;

		private static bool paused;

		private static SpatialLevels trackLevels;

		private static SpatialLevels crowdLevels;

		private static NightclubWindowsMedia decodedTrack;

		private static NightclubWindowsMedia decodedCrowd;

		private static readonly Stopwatch decodeClock = new Stopwatch();

		private static readonly Stopwatch crowdDecodeClock = new Stopwatch();

		private static string currentTrack;

		private static bool mixMuted;

		private static int nextUpdate;

		[DllImport("winmm.dll", CharSet = CharSet.Unicode, EntryPoint = "mciSendStringW")]
		private static extern uint SendMediaCommand(string command, StringBuilder output, int outputLength, IntPtr window);

		[DllImport("winmm.dll", CharSet = CharSet.Unicode, EntryPoint = "mciGetErrorStringW")]
		private static extern bool GetMediaError(uint code, StringBuilder description, int length);

		internal static string[] AvailableTracks(string folder, string fallback)
		{
			return new string[2] { folder, fallback }.Where((_Closure_0024__._0024I29_002D0 != null) ? _Closure_0024__._0024I29_002D0 : (_Closure_0024__._0024I29_002D0 = _Closure_0024__._0024I._Lambda_0024__29_002D0)).SelectMany((_Closure_0024__._0024I29_002D1 != null) ? _Closure_0024__._0024I29_002D1 : (_Closure_0024__._0024I29_002D1 = _Closure_0024__._0024I._Lambda_0024__29_002D1)).Where((_Closure_0024__._0024I29_002D2 != null) ? _Closure_0024__._0024I29_002D2 : (_Closure_0024__._0024I29_002D2 = _Closure_0024__._0024I._Lambda_0024__29_002D2))
				.Select((_Closure_0024__._0024I29_002D3 != null) ? _Closure_0024__._0024I29_002D3 : (_Closure_0024__._0024I29_002D3 = _Closure_0024__._0024I._Lambda_0024__29_002D3))
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.OrderBy((_Closure_0024__._0024I29_002D4 != null) ? _Closure_0024__._0024I29_002D4 : (_Closure_0024__._0024I29_002D4 = _Closure_0024__._0024I._Lambda_0024__29_002D4), StringComparer.OrdinalIgnoreCase)
				.Take(80)
				.ToArray();
		}

		internal static int VolumeAt(Vector3 position)
		{
			double num = Math.Sqrt(position.DistanceToSquared(NearestSpeaker(position)));
			return checked((int)Math.Round(Math.Max(170.0, 680.0 - num * 17.0)));
		}

		private static Vector3 NearestSpeaker(Vector3 position)
		{
			if (!(position.DistanceToSquared(DanceSpeaker) <= position.DistanceToSquared(OfficeSpeaker)))
			{
				return OfficeSpeaker;
			}
			return DanceSpeaker;
		}

		internal static float PanAt(Vector3 position, Vector3 facing, Vector3 source)
		{
			float num = source.X - position.X;
			float num2 = source.Y - position.Y;
			double val = Math.Sqrt(num * num + num2 * num2);
			double num3 = Math.Sqrt(facing.X * facing.X + facing.Y * facing.Y);
			if (num3 < 0.01)
			{
				return 0f;
			}
			double num4 = (double)(num * facing.Y - num2 * facing.X) / (Math.Max(1.0, val) * num3);
			return (float)Math.Max(-0.78, Math.Min(0.78, num4 * 0.78));
		}

		private static bool ApplyTrackLevels(Vector3 position, Vector3 facing)
		{
			int volume = VolumeAt(position);
			float pan = PanAt(position, facing, NearestSpeaker(position));
			if (decodedTrack == null)
			{
				return trackLevels.Apply(volume, pan);
			}
			decodedTrack.SetLevels(volume, pan);
			return true;
		}

		public static bool Tick(ExpansionSession session)
		{
			UpdatePlayback(session);
			if (Game.IsPaused)
			{
				CloseMenu();
				return false;
			}
			if (menu != null && menu.Visible)
			{
				if (!CanUseBooth(session))
				{
					CloseMenu();
				}
				return true;
			}
			if (menuSession != null)
			{
				CloseMenu();
			}
			if (!session.Actor.IsInVehicle() && OnlineExpansion.TryInteract(session, Booth, 1.85f, "Press ~INPUT_CONTEXT~ to request a song from the DJ."))
			{
				OpenMenu(session);
				return true;
			}
			return false;
		}

		private static bool CanUseBooth(ExpansionSession session)
		{
			if (!Game.IsPaused && session != null && session == menuSession && OnlineExpansion.IsCurrent(session) && OnlineExpansion.IsOwned(session.Definition, session.Owner) && !Game.IsMissionActive && !StoryLoadGuard.WorldLoading() && !session.Actor.IsInVehicle())
			{
				return session.Actor.Position.DistanceToSquared(Booth) <= 4.62250042f;
			}
			return false;
		}

		private static void OpenMenu(ExpansionSession session)
		{
			try
			{
				string[] array = AvailableTracks("scripts\\SPA II\\NightclubMusic", "scripts\\AfterHoursSP\\Audio");
				if (array.Length == 0 && !mediaOpen)
				{
					Notification.Show("Put MP3 files in scripts\\SPA II\\NightclubMusic to request a song.");
					return;
				}
				if (menu == null)
				{
					menu = new UIMenu("", "DJ REQUESTS", new Point(0, -107))
					{
						MouseEdgeEnabled = false
					};
					menu.SetBannerType(Helper.MenuBanner);
					Helper.MenuPool.Add(menu);
					menu.OnItemSelect += SelectTrack;
				}
				menu.Clear();
				if (mediaOpen)
				{
					menu.AddItem(new UIMenuItem("Return to the DJ mix", "Stop the current request."));
				}
				string[] array2 = array;
				foreach (string text in array2)
				{
					menu.AddItem(new UIMenuItem(Path.GetFileNameWithoutExtension(text), "Play this local track in the nightclub.")
					{
						Tag = text
					});
				}
				menu.RefreshIndex();
				menuSession = session;
				menu.Visible = true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				CloseMenu();
				Logger.Log("After Hours track list: " + ex2.Message);
				Notification.Show("The nightclub could not read the music folder.");
				ProjectData.ClearProjectError();
			}
		}

		private static void SelectTrack(UIMenu sender, UIMenuItem item, int index)
		{
			ExpansionSession expansionSession = menuSession;
			if (sender != menu || !sender.Visible || !CanUseBooth(expansionSession))
			{
				return;
			}
			CloseMenu();
			string text = item.Tag as string;
			StopPlayback(true);
			if (text == null)
			{
				return;
			}
			try
			{
				if (!File.Exists(text) || text.IndexOf('"') >= 0)
				{
					throw new IOException("Invalid MP3 path.");
				}
				currentTrack = Path.GetFileName(text);
				mediaOpen = true;
				playbackSession = expansionSession;
				string failure = null;
				if (TryOpenMci(text, PlayerAlias, ref failure))
				{
					trackLevels = new SpatialLevels(PlayerAlias);
					if (ApplyTrackLevels(expansionSession.Actor.Position, GameplayCamera.Direction) && MediaCommand("play " + PlayerAlias))
					{
						StartRequestedMix();
						nextUpdate = checked(Game.GameTime + 750);
						return;
					}
					MediaCommand("close " + PlayerAlias);
					trackLevels = null;
					failure = "MCI opened the file but rejected the playback or volume command.";
				}
				Logger.Log($"After Hours track request: using the Windows media decoder for {currentTrack} ({failure}).");
				decodedTrack = new NightclubWindowsMedia(text, false);
				decodeClock.Restart();
				nextUpdate = 0;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				FailTrack(ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		private static void StartRequestedMix()
		{
			if (!mixMuted)
			{
				mixMuted = true;
				NightclubExpansion.SetMixEmittersEnabled(false);
				StartCrowd();
			}
		}

		private static void FailTrack(string reason)
		{
			Logger.Log($"After Hours track request failed: {currentTrack}, {reason}");
			StopPlayback(true);
			Notification.Show("The DJ could not play this MP3. The club mix is active.");
		}

		private static void UpdatePlayback(ExpansionSession session)
		{
			if (!mediaOpen)
			{
				return;
			}
			if (session != playbackSession || Game.IsMissionActive)
			{
				StopPlayback(session == playbackSession);
				return;
			}
			try
			{
				if (crowdDecodeClock.IsRunning)
				{
					CheckCrowdDecoder();
				}
				if (Game.GameTime < nextUpdate && Game.IsPaused == paused)
				{
					return;
				}
				nextUpdate = checked(Game.GameTime + 250);
				if (decodedTrack != null)
				{
					string failure = null;
					switch (decodedTrack.GetPlayback(ref failure))
					{
					case NightclubWindowsMedia.Playback.Opening:
						decodedTrack.SetPaused(Game.IsPaused);
						if (decodeClock.ElapsedMilliseconds >= 10000)
						{
							FailTrack("The Windows media decoder timed out.");
						}
						return;
					case NightclubWindowsMedia.Playback.Failed:
						FailTrack("Windows media decoder: " + failure);
						return;
					case NightclubWindowsMedia.Playback.Ended:
						StopPlayback(true);
						return;
					}
					if (!mixMuted)
					{
						Logger.Log($"After Hours track request: Windows media decoder opened {currentTrack}.");
						StartRequestedMix();
					}
				}
				if (Game.IsPaused)
				{
					if (!paused)
					{
						paused = SetPaused(true);
					}
					return;
				}
				if (paused)
				{
					paused = false;
					if (!SetPaused(false))
					{
						StopPlayback(true);
						return;
					}
				}
				if (decodedTrack == null)
				{
					StringBuilder stringBuilder = new StringBuilder(32);
					if (!MediaCommand($"status {PlayerAlias} mode", stringBuilder) || string.Equals(stringBuilder.ToString().Trim(), "stopped", StringComparison.OrdinalIgnoreCase))
					{
						StopPlayback(true);
						return;
					}
				}
				Vector3 position = session.Actor.Position;
				Vector3 direction = GameplayCamera.Direction;
				if (!ApplyTrackLevels(position, direction))
				{
					StopPlayback(true);
				}
				else
				{
					UpdateCrowd(position, direction);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("After Hours track playback: " + ex2.Message);
				StopPlayback(true);
				ProjectData.ClearProjectError();
			}
		}

		private static bool SetPaused(bool value)
		{
			string text = (value ? "pause " : "resume ");
			if (decodedTrack != null)
			{
				decodedTrack.SetPaused(value);
			}
			else if (!MediaCommand(text + PlayerAlias))
			{
				return false;
			}
			if (decodedCrowd != null)
			{
				decodedCrowd.SetPaused(value);
			}
			else if (crowdOpen)
			{
				MediaCommand(text + CrowdAlias);
			}
			return true;
		}

		private static bool MediaCommand(string command, StringBuilder output = null)
		{
			return SendMediaCommand(command, output, output?.Capacity ?? 0, IntPtr.Zero) == 0;
		}

		internal static string[] MciOpenCommands(string track, string deviceAlias)
		{
			string text = "open \"" + track + "\"";
			return new string[2]
			{
				text + " type mpegvideo alias " + deviceAlias,
				text + " alias " + deviceAlias
			};
		}

		private static bool TryOpenMci(string track, string deviceAlias, ref string failure)
		{
			List<string> list = new List<string>();
			string[] array = MciOpenCommands(track, deviceAlias);
			for (int i = 0; i < array.Length; i = checked(i + 1))
			{
				uint num = SendMediaCommand(array[i], null, 0, IntPtr.Zero);
				if (num == 0)
				{
					return true;
				}
				StringBuilder stringBuilder = new StringBuilder(256);
				GetMediaError(num, stringBuilder, stringBuilder.Capacity);
				list.Add($"MCI {num}: {stringBuilder}");
			}
			failure = string.Join(" / ", list);
			return false;
		}

		private static void StartCrowd()
		{
			if (!File.Exists("scripts\\AfterHoursSP\\Crowd.mp3"))
			{
				return;
			}
			try
			{
				string fullPath = Path.GetFullPath("scripts\\AfterHoursSP\\Crowd.mp3");
				string failure = null;
				crowdOpen = true;
				if (TryOpenMci(fullPath, CrowdAlias, ref failure))
				{
					crowdLevels = new SpatialLevels(CrowdAlias);
					Vector3 position = playbackSession.Actor.Position;
					if (crowdLevels.Apply(CrowdVolume(position), PanAt(position, GameplayCamera.Direction, DanceSpeaker)) && MediaCommand("play " + CrowdAlias + " repeat"))
					{
						return;
					}
					MediaCommand("close " + CrowdAlias);
					crowdLevels = null;
				}
				decodedCrowd = new NightclubWindowsMedia(fullPath, true);
				crowdDecodeClock.Restart();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				StopCrowd();
				Logger.Log("After Hours crowd audio: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		private static void UpdateCrowd(Vector3 position, Vector3 facing)
		{
			if (!crowdOpen)
			{
				return;
			}
			int volume = CrowdVolume(position);
			float pan = PanAt(position, facing, DanceSpeaker);
			if (decodedCrowd == null)
			{
				if (!crowdLevels.Apply(volume, pan))
				{
					StopCrowd();
				}
			}
			else if (CheckCrowdDecoder())
			{
				decodedCrowd.SetLevels(volume, pan);
			}
		}

		private static bool CheckCrowdDecoder()
		{
			string failure = null;
			switch (decodedCrowd.GetPlayback(ref failure))
			{
			case NightclubWindowsMedia.Playback.Playing:
				crowdDecodeClock.Reset();
				return true;
			case NightclubWindowsMedia.Playback.Opening:
				if (crowdDecodeClock.ElapsedMilliseconds < 10000)
				{
					return false;
				}
				Logger.Log("After Hours crowd audio: the Windows media decoder timed out.");
				break;
			case NightclubWindowsMedia.Playback.Failed:
				Logger.Log("After Hours crowd audio: " + failure);
				break;
			}
			StopCrowd();
			return false;
		}

		private static int CrowdVolume(Vector3 position)
		{
			float num = position.X - DanceSpeaker.X;
			float num2 = position.Y - DanceSpeaker.Y;
			double num3 = Math.Sqrt(num * num + num2 * num2);
			if (!(position.Z < -78.9f))
			{
				return 0;
			}
			return checked((int)Math.Round(Math.Max(0.0, 240.0 - num3 * 35.0)));
		}

		private static void StopCrowd()
		{
			if (!crowdOpen)
			{
				return;
			}
			crowdOpen = false;
			try
			{
				if (decodedCrowd != null)
				{
					decodedCrowd.Dispose();
				}
				MediaCommand("close " + CrowdAlias);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("After Hours crowd audio cleanup: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			finally
			{
				decodedCrowd = null;
				crowdLevels = null;
				crowdDecodeClock.Reset();
			}
		}

		private static void StopPlayback(bool restoreMix)
		{
			if (!mediaOpen)
			{
				return;
			}
			mediaOpen = false;
			paused = false;
			playbackSession = null;
			try
			{
				StopCrowd();
				if (decodedTrack != null)
				{
					decodedTrack.Dispose();
				}
				MediaCommand("close " + PlayerAlias);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("After Hours track cleanup: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			finally
			{
				decodedTrack = null;
				trackLevels = null;
				decodeClock.Reset();
				currentTrack = null;
				bool num = restoreMix && mixMuted;
				mixMuted = false;
				if (num)
				{
					NightclubExpansion.SetMixEmittersEnabled(true);
				}
			}
		}

		private static void CloseMenu()
		{
			if (menu != null)
			{
				menu.Visible = false;
			}
			menuSession = null;
		}

		public static void Close(ExpansionSession session)
		{
			if (menuSession == session)
			{
				CloseMenu();
			}
			if (playbackSession == session)
			{
				StopPlayback(false);
			}
		}

		public static void Reset(bool storyLoad)
		{
			CloseMenu();
			StopPlayback(false);
		}
	}
}
