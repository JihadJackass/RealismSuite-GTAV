using System;
using System.Collections.Generic;
using System.Diagnostics;
using GTA;
using GTA.Math;
using GTA.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class NightclubPodiumDancers
	{
		private sealed class Performer
		{
			public readonly string ModelName;

			public readonly Quaternion Pose;

			public Model Model;

			public bool Requested;

			public bool Finished;

			public Ped Actor;

			public IntPtr Address;

			public int ModelHash;

			public long NextCheck;

			public Performer(string name, Quaternion pose)
			{
				ModelName = name;
				Pose = pose;
			}
		}

		internal const string DanceDictionary = "anim@amb@nightclub@dancers@podium_dancers@";

		internal const string DanceClip = "hi_dance_facedj_17_v2_female^2";

		private const long LoadTimeoutMs = 12000L;

		private static readonly Quaternion[] Posts = new Quaternion[2]
		{
			new Quaternion(-1596.083f, -3007.928f, -78.2081f, 180f),
			new Quaternion(-1598.524f, -3015.737f, -78.2102f, 358.6f)
		};

		private static readonly Vector3[] Footprint = new Vector3[4]
		{
			new Vector3(0.28f, 0f, 0f),
			new Vector3(-0.28f, 0f, 0f),
			new Vector3(0f, 0.28f, 0f),
			new Vector3(0f, -0.28f, 0f)
		};

		private static ExpansionSession current;

		private static int selected = -1;

		private static bool failed;

		private static bool dictionaryRequested;

		private static readonly Stopwatch clock = new Stopwatch();

		private static readonly List<Performer> performers = new List<Performer>();

		private static long nextLoad;

		private static int nextPerformer;

		internal static bool IsPodiumSupport(Quaternion pose, Vector3 point)
		{
			float num = point.X - pose.X;
			float num2 = point.Y - pose.Y;
			if (!float.IsNaN(point.Z) && num * num + num2 * num2 <= 0.0025f)
			{
				return Math.Abs(point.Z - (pose.Z - 1f)) <= 0.25f;
			}
			return false;
		}

		private static bool TryPodiumFloor(ExpansionSession session, Quaternion pose, ref Vector3 floor)
		{
			Vector3 position = Helper.ToVector3(pose) - new Vector3(0f, 0f, 1f);
			if (!NightclubExpansion.TryStandingFloor(session, position, position.Z - 0.25f, position.Z + 0.25f, ref floor) || !IsPodiumSupport(pose, floor))
			{
				return false;
			}
			Vector3[] footprint = Footprint;
			Vector3 floor2 = default(Vector3);
			foreach (Vector3 vector in footprint)
			{
				if (!NightclubExpansion.TryStandingFloor(session, floor + vector, floor.Z - 0.08f, floor.Z + 0.08f, ref floor2))
				{
					return false;
				}
			}
			return true;
		}

		internal static string[] ModelsFor(int choice)
		{
			switch (choice)
			{
			case 1:
				return new string[2] { "u_f_y_dancerave_01", "u_f_y_dancerave_01" };
			case 2:
				return new string[2] { "a_f_y_clubcust_01", "a_f_y_clubcust_02" };
			default:
				return new string[0];
			}
		}

		internal static bool CanContinueLoading(long elapsed, bool finished, bool hasActor)
		{
			if (!finished && !hasActor && elapsed >= 0)
			{
				return elapsed < 12000;
			}
			return false;
		}

		public static void Tick(ExpansionSession session)
		{
			if (StoryLoadGuard.WorldLoading() || Game.IsPaused || !OnlineExpansion.IsCurrent(session) || session.Destination != session.Definition.Arrival)
			{
				return;
			}
			try
			{
				int num2;
				checked
				{
					if (current != session)
					{
						Reset(false);
						current = session;
						selected = NightclubExpansionMenu.SelectedDancers(session.Definition.Key);
						string[] array = ModelsFor(selected);
						int num = array.Length - 1;
						for (int i = 0; i <= num; i++)
						{
							performers.Add(new Performer(array[i], Posts[i]));
						}
						clock.Restart();
						nextLoad = 500L;
					}
					if (failed || performers.Count == 0 || clock.ElapsedMilliseconds < nextLoad)
					{
						return;
					}
					nextLoad = clock.ElapsedMilliseconds + 250;
					foreach (Performer performer3 in performers)
					{
						if (performer3.Finished)
						{
							MaintainAnimation(performer3);
						}
					}
					num2 = performers.Count - 1;
				}
				for (int j = 0; j <= num2; j = checked(j + 1))
				{
					Performer performer2 = performers[nextPerformer];
					nextPerformer = checked(nextPerformer + 1) % performers.Count;
					if (!performer2.Finished)
					{
						LoadPerformer(session, performer2);
						break;
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				failed = true;
				Logger.Log("After Hours podium dancers: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		private static void LoadPerformer(ExpansionSession session, Performer performer)
		{
			if (!CanContinueLoading(clock.ElapsedMilliseconds, performer.Finished, (object)performer.Actor != null))
			{
				performer.Finished = true;
				ReleaseModel(performer);
				Logger.Log("After Hours podium dancer skipped: model=" + performer.ModelName + "; no ready asset or supported podium within deadline.");
				return;
			}
			if (!performer.Requested)
			{
				performer.Model = new Model(performer.ModelName);
				if (!performer.Model.IsValid || !performer.Model.IsInCdImage)
				{
					performer.Finished = true;
					Logger.Log("After Hours podium dancer unavailable: " + performer.ModelName);
					return;
				}
				performer.Model.Request();
				performer.Requested = true;
			}
			if (!performer.Model.IsLoaded)
			{
				return;
			}
			if (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@amb@nightclub@dancers@podium_dancers@"))
			{
				if (!dictionaryRequested)
				{
					Function.Call(Hash.REQUEST_ANIM_DICT, "anim@amb@nightclub@dancers@podium_dancers@");
					dictionaryRequested = true;
				}
			}
			else if (Function.Call<float>(Hash.GET_ANIM_DURATION, "anim@amb@nightclub@dancers@podium_dancers@", "hi_dance_facedj_17_v2_female^2") <= 0f)
			{
				performer.Finished = true;
				ReleaseModel(performer);
				Logger.Log("After Hours podium dance clip unavailable.");
			}
			else
			{
				Vector3 floor = default(Vector3);
				if (!TryPodiumFloor(session, performer.Pose, ref floor))
				{
					return;
				}
				Ped ped = World.CreatePed(performer.Model, floor, performer.Pose.W);
				if ((object)ped != null && ped.Exists())
				{
					if (ped.MemoryAddress == IntPtr.Zero)
					{
						ped.MarkAsNoLongerNeeded();
						return;
					}
					performer.Actor = ped;
					performer.Address = ped.MemoryAddress;
					performer.ModelHash = performer.Model.Hash;
					performer.Finished = true;
					ped.IsPersistent = true;
					Function.Call(Hash.SET_ENTITY_COORDS, ped.Handle, floor.X, floor.Y, floor.Z, false, false, false, false);
					Function.Call(Hash.SET_ENTITY_HEADING, ped.Handle, performer.Pose.W);
					Function.Call(Hash.SET_PED_DEFAULT_COMPONENT_VARIATION, ped.Handle);
					ped.BlockPermanentEvents = true;
					ped.IsPositionFrozen = true;
					ped.CanRagdoll = false;
					Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, ped.Handle, session.Interior, Game.GenerateHash("Int_01_main_area"));
					Function.Call(Hash.SET_PED_KEEP_TASK, ped.Handle, true);
					PlayDance(performer);
					ReleaseModel(performer);
					Logger.Log("After Hours podium dancer placed: " + performer.ModelName + "; floor=" + floor.ToString());
				}
			}
		}

		private static bool Matches(Performer performer)
		{
			if ((object)performer.Actor != null && performer.Address != IntPtr.Zero && performer.Actor.Exists() && performer.Actor.MemoryAddress == performer.Address)
			{
				return performer.Actor.Model.Hash == performer.ModelHash;
			}
			return false;
		}

		private static void PlayDance(Performer performer)
		{
			Function.Call(Hash.TASK_PLAY_ANIM, performer.Actor.Handle, "anim@amb@nightclub@dancers@podium_dancers@", "hi_dance_facedj_17_v2_female^2", 4f, -4f, -1, 1, 0f, false, false, false);
			performer.NextCheck = checked(clock.ElapsedMilliseconds + 2500);
		}

		private static void MaintainAnimation(Performer performer)
		{
			if (Matches(performer) && !performer.Actor.IsDead && clock.ElapsedMilliseconds >= performer.NextCheck)
			{
				performer.NextCheck = checked(clock.ElapsedMilliseconds + 2500);
				if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, performer.Actor.Handle, "anim@amb@nightclub@dancers@podium_dancers@", "hi_dance_facedj_17_v2_female^2", 3) && Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@amb@nightclub@dancers@podium_dancers@"))
				{
					PlayDance(performer);
				}
			}
		}

		private static void ReleaseModel(Performer performer)
		{
			if (performer.Requested)
			{
				performer.Requested = false;
				performer.Model.MarkAsNoLongerNeeded();
			}
		}

		public static void Refresh(ExpansionSession session)
		{
			if (current == session)
			{
				Reset(false);
			}
		}

		public static void Close(ExpansionSession session)
		{
			if (current == session)
			{
				Reset(false);
			}
		}

		public static void Reset(bool storyLoad)
		{
			bool flag = !storyLoad && current != null && !StoryLoadGuard.WorldLoading();
			current = null;
			selected = -1;
			failed = false;
			nextLoad = 0L;
			nextPerformer = 0;
			try
			{
				if (!flag)
				{
					return;
				}
				foreach (Performer performer in performers)
				{
					try
					{
						if (Matches(performer))
						{
							performer.Actor.Delete();
						}
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						Logger.Log("After Hours podium dancer cleanup: " + ex2.Message);
						ProjectData.ClearProjectError();
					}
					finally
					{
						try
						{
							ReleaseModel(performer);
						}
						catch (Exception ex3)
						{
							ProjectData.SetProjectError(ex3);
							Exception ex4 = ex3;
							Logger.Log("After Hours podium dancer model release: " + ex4.Message);
							ProjectData.ClearProjectError();
						}
					}
				}
				if (dictionaryRequested)
				{
					Function.Call(Hash.REMOVE_ANIM_DICT, "anim@amb@nightclub@dancers@podium_dancers@");
				}
			}
			finally
			{
				performers.Clear();
				dictionaryRequested = false;
				clock.Reset();
			}
		}
	}
}
