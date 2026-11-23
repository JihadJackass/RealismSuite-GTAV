using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class NightclubProjector
	{
		private const string Target = "Club_Projector";

		private const int Channel = 0;

		private const int VideoStartTimeout = 4000;

		private const int VideoRetryDelay = 10000;

		private const int VideoStartAttempts = 2;

		private const int ClassicScreen = -13322879;

		private const int UpgradedScreen = 1206830836;

		private const int BasicScreen = -1833573429;

		private static readonly Vector3 Stage = new Vector3(-1597.229f, -3012.469f, -74.32224f);

		private static readonly Vector3 ScreenOrigin = new Vector3(-1604.664f, -3012.583f, -80f);

		private static readonly string[] DjCodes = new string[4] { "DIX", "SOL", "TOU", "TBM" };

		private static readonly string[] LightCodes = new string[5] { "NL", "LED", "GEO", "RIB", "LSER" };

		private static readonly string[] NameCodes = new string[9] { "GALAXY", "LOS", "OMEGA", "TECH", "GEFANGNIS", "MAIS", "FUNHOUSE", "PALACE", "PARADISE" };

		private static readonly Color[] LightColors = new Color[5]
		{
			Color.SteelBlue,
			Color.MediumPurple,
			Color.Turquoise,
			Color.DeepPink,
			Color.LawnGreen
		};

		private static readonly Stopwatch clock = Stopwatch.StartNew();

		private static ExpansionSession active;

		private static bool videos;

		private static long nextSetup;

		private static bool targetOwned;

		private static int targetId;

		private static int linkedModel;

		private static bool decoderStarted;

		private static float previousVolume;

		private static string playlist;

		private static string requestedPlaylist;

		private static int startsWithoutClip;

		private static long nextVideoAttempt;

		private static long videoDeadline;

		internal static string PlaylistFor(int dj, int name, int lighting)
		{
			if (dj < 0 || dj >= DjCodes.Length)
			{
				dj = 1;
			}
			if (name < 0 || name >= NameCodes.Length)
			{
				name = 0;
			}
			return "PL_" + DjCodes[dj] + "_" + LightCodes[LightingIndex(lighting)] + "_" + NameCodes[name];
		}

		private static int LightingIndex(int lighting)
		{
			if (lighting < 0 || lighting >= LightCodes.Length)
			{
				return 1;
			}
			return lighting;
		}

		internal static int ScreenModelFor(int lighting)
		{
			if (LightingIndex(lighting) != 0)
			{
				return 1206830836;
			}
			return -13322879;
		}

		internal static bool IsEnhancedProcessName(string processName)
		{
			string text = (processName ?? "").Trim();
			if (text.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
			{
				text = text.Substring(0, checked(text.Length - 4));
			}
			if (!string.Equals(text, "GTA5_Enhanced", StringComparison.OrdinalIgnoreCase))
			{
				return string.Equals(text, "GTA5_Enhanced_BE", StringComparison.OrdinalIgnoreCase);
			}
			return true;
		}

		internal static bool VideosEnabled(ScriptSettings settings, bool enhanced)
		{
			return settings?.GetValue("SETTING", "NightclubVideos", enhanced) ?? enhanced;
		}

		internal static bool UsableProjectorId(int renderId, int defaultId)
		{
			if (renderId != -1)
			{
				return TvRenderSurface.UsableId(renderId, defaultId);
			}
			return false;
		}

		internal static void Tick(ExpansionSession session)
		{
			if (active != session)
			{
				Reset(false);
				active = session;
				if (session != null)
				{
					using (Process process = Process.GetCurrentProcess())
					{
						videos = VideosEnabled(ConfigHelper.config, IsEnhancedProcessName(process.ProcessName));
					}
					if (!videos)
					{
						Logger.Log($"After Hours projector: using animated lights for {session.Definition.Key} (NightclubVideos off).");
					}
				}
			}
			if (session == null)
			{
				return;
			}
			checked
			{
				try
				{
					if (session.Actor.Position.DistanceToSquared(Stage) > 1156f)
					{
						ReleasePlayback(false);
						return;
					}
					NightclubDj nightclubDj = NightclubExpansion.CurrentVisitDj(session);
					if (nightclubDj == null)
					{
						return;
					}
					int lighting = NightclubExpansionMenu.SelectedLighting(session.Definition.Key);
					long elapsedMilliseconds = clock.ElapsedMilliseconds;
					DrawStageLight(lighting, elapsedMilliseconds);
					if (elapsedMilliseconds >= nextSetup)
					{
						nextSetup = elapsedMilliseconds + 250;
						if (!RefreshTarget(session.Interior, lighting))
						{
							return;
						}
						SyncDecoder(PlaylistFor(nightclubDj.LightIndex, NightclubExpansionMenu.SelectedClubName(session.Definition.Key), lighting), elapsedMilliseconds);
					}
					if (targetId == 0)
					{
						return;
					}
					if (!MatchesTarget())
					{
						ReleasePlayback(false);
						return;
					}
					bool flag = VideoPlaying();
					if (decoderStarted)
					{
						videoDeadline = ClipDeadline(videoDeadline, elapsedMilliseconds, flag);
						if (flag)
						{
							startsWithoutClip = 0;
						}
						else if (elapsedMilliseconds >= videoDeadline)
						{
							Logger.Log($"After Hours projector: no active clip for {playlist}. Using animated lights.");
							StopDecoder();
							nextVideoAttempt = elapsedMilliseconds + 10000;
						}
					}
					if (flag)
					{
						ApartmentProps.RenderTV(targetId);
					}
					else
					{
						DrawAnimatedScreen(lighting, elapsedMilliseconds);
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Nightclub screens: " + ex2.Message);
					ReleasePlayback(false);
					nextSetup = clock.ElapsedMilliseconds + 2000;
					ProjectData.ClearProjectError();
				}
			}
		}

		private static bool RefreshTarget(int interior, int lighting)
		{
			List<int> list = FindScreenModels(interior, lighting);
			int num = list.FirstOrDefault();
			if (num == 0 || (linkedModel != 0 && linkedModel != num))
			{
				ReleasePlayback(false);
			}
			if (num == 0)
			{
				return false;
			}
			bool flag = Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "Club_Projector");
			if (targetOwned && (!flag || (targetId != 0 && Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "Club_Projector") != targetId)))
			{
				targetOwned = false;
				ReleasePlayback(false);
				return false;
			}
			if (!targetOwned)
			{
				if (flag)
				{
					return false;
				}
				Function.Call(Hash.REGISTER_NAMED_RENDERTARGET, "Club_Projector", false);
				targetOwned = Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "Club_Projector");
				if (!targetOwned)
				{
					return false;
				}
			}
			foreach (int item in list)
			{
				if (!Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_LINKED, item))
				{
					Function.Call(Hash.LINK_NAMED_RENDERTARGET, item);
					return false;
				}
			}
			int renderId = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "Club_Projector");
			if (!UsableProjectorId(renderId, Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID)))
			{
				return false;
			}
			linkedModel = num;
			targetId = renderId;
			return true;
		}

		private static List<int> FindScreenModels(int interior, int lighting)
		{
			int num = ScreenModelFor(lighting);
			int num2 = ((num == -13322879) ? 1206830836 : (-13322879));
			List<int> list = new List<int>();
			int[] array = new int[3] { num, num2, -1833573429 };
			foreach (int num3 in array)
			{
				if (!new Model(num3).IsLoaded)
				{
					continue;
				}
				int num4 = Function.Call<int>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, ScreenOrigin.X, ScreenOrigin.Y, ScreenOrigin.Z, 22f, num3, false, false, false);
				if (num4 != 0 && Function.Call<bool>(Hash.DOES_ENTITY_EXIST, num4))
				{
					int num5 = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, num4);
					if (num5 == 0 || num5 == interior)
					{
						list.Add(num3);
					}
				}
			}
			if (list.Count == 0 && new Model(num).IsLoaded)
			{
				list.Add(num);
			}
			return list;
		}

		private static bool MatchesTarget()
		{
			if (targetOwned && targetId != 0 && linkedModel != 0 && Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "Club_Projector") && Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "Club_Projector") == targetId)
			{
				return Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_LINKED, linkedModel);
			}
			return false;
		}

		private static void SyncDecoder(string selected, long now)
		{
			if (!string.Equals(requestedPlaylist, selected, StringComparison.Ordinal))
			{
				requestedPlaylist = selected;
				startsWithoutClip = 0;
				nextVideoAttempt = 0L;
			}
			if (!videos || OtherTvActive())
			{
				StopDecoder();
				return;
			}
			int num = Function.Call<int>(Hash.GET_TV_CHANNEL);
			if (decoderStarted)
			{
				if (num != 0)
				{
					decoderStarted = false;
					playlist = null;
					videoDeadline = 0L;
					return;
				}
				if (string.Equals(playlist, selected, StringComparison.Ordinal))
				{
					return;
				}
				StopDecoder();
			}
			else if (num >= 0)
			{
				return;
			}
			checked
			{
				if (CanStartVideo(startsWithoutClip, nextVideoAttempt, now))
				{
					startsWithoutClip++;
					nextVideoAttempt = now + 10000;
					previousVolume = Function.Call<float>(Hash.GET_TV_VOLUME);
					Function.Call(Hash.SET_TV_CHANNEL_PLAYLIST, 0, selected, true);
					Function.Call(Hash.SET_TV_AUDIO_FRONTEND, true);
					Function.Call(Hash.SET_TV_CHANNEL, 0);
					decoderStarted = Function.Call<int>(Hash.GET_TV_CHANNEL) == 0;
					if (!decoderStarted)
					{
						Function.Call(Hash.SET_TV_AUDIO_FRONTEND, false);
						return;
					}
					Function.Call(Hash.SET_TV_VOLUME, -60f);
					playlist = selected;
					videoDeadline = 0L;
					Logger.Log($"After Hours projector: queued {selected} with target {targetId}.");
				}
			}
		}

		internal static bool CanStartVideo(int attempts, long nextAttempt, long now)
		{
			if (attempts < 2)
			{
				return now >= nextAttempt;
			}
			return false;
		}

		internal static long ClipDeadline(long deadline, long now, bool hasClip)
		{
			if (hasClip)
			{
				return 0L;
			}
			return (deadline == 0L) ? checked(now + 4000) : deadline;
		}

		private static bool VideoPlaying()
		{
			if (decoderStarted && !OtherTvActive() && Function.Call<int>(Hash.GET_TV_CHANNEL) == 0)
			{
				return Function.Call<int>(Hash.GET_CURRENT_TV_CLIP_NAMEHASH) != 0;
			}
			return false;
		}

		private static void DrawStageLight(int lighting, long now)
		{
			float num = (float)Math.Sin((double)(now % 3600) / 3600.0 * Math.PI * 2.0);
			World.DrawSpotLight(Stage, new Vector3(0.101f, num * 0.2f, -0.975f), LightColors[LightingIndex(lighting)], 15f, 1.25f, 10f, 50f, 200f);
		}

		internal static float AnimatedBarHeight(int index, long elapsed)
		{
			return 0.18f + 0.64f * (float)((1.0 + Math.Sin((double)(elapsed % 2400) / 2400.0 * Math.PI * 2.0 - (double)index * 0.76)) * 0.5);
		}

		private static void DrawAnimatedScreen(int lighting, long now)
		{
			Color color = LightColors[LightingIndex(lighting)];
			try
			{
				Function.Call(Hash.SET_TEXT_RENDER_ID, targetId);
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, true);
				Function.Call(Hash.DRAW_RECT, 0.5f, 0.5f, 1f, 1f, 6, 4, 16, 255, false);
				int num = 0;
				do
				{
					float num2 = AnimatedBarHeight(num, now);
					Function.Call(Hash.DRAW_RECT, 0.1f + (float)num * 0.1f, 0.9f - num2 * 0.5f, 0.065f, num2, (int)color.R, (int)color.G, (int)color.B, 255, false);
					num = checked(num + 1);
				}
				while (num <= 8);
			}
			finally
			{
				Function.Call(Hash.SET_TEXT_RENDER_ID, Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 0);
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, false);
			}
		}

		private static bool OtherTvActive()
		{
			if (!ApartmentProps.TVOn)
			{
				return MansionMedia.OwnsDecoder;
			}
			return true;
		}

		internal static void Close(ExpansionSession session)
		{
			if (active == session)
			{
				Reset(false);
			}
		}

		internal static void Reset(bool storyLoad)
		{
			active = null;
			videos = false;
			nextSetup = 0L;
			requestedPlaylist = null;
			startsWithoutClip = 0;
			nextVideoAttempt = 0L;
			ReleasePlayback(storyLoad);
		}

		private static void StopDecoder()
		{
			bool num = decoderStarted;
			decoderStarted = false;
			playlist = null;
			videoDeadline = 0L;
			if (!num)
			{
				return;
			}
			try
			{
				if (!OtherTvActive() && Function.Call<int>(Hash.GET_TV_CHANNEL) == 0)
				{
					Function.Call(Hash.SET_TV_CHANNEL, -1);
					Function.Call(Hash.SET_TV_AUDIO_FRONTEND, false);
					Function.Call(Hash.SET_TV_VOLUME, previousVolume);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Nightclub screen decoder cleanup: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		private static void ReleasePlayback(bool storyLoad)
		{
			bool flag = targetOwned;
			int num = targetId;
			targetOwned = false;
			targetId = 0;
			linkedModel = 0;
			if (storyLoad)
			{
				decoderStarted = false;
				playlist = null;
				videoDeadline = 0L;
				return;
			}
			StopDecoder();
			try
			{
				if (flag && Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "Club_Projector") && (num == 0 || Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "Club_Projector") == num))
				{
					Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, "Club_Projector");
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Nightclub screen render target cleanup: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}
	}
}
