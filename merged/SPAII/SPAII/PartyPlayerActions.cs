using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class PartyPlayerActions
	{
		public enum PairedKissOutcome
		{
			None,
			Running,
			Succeeded,
			Failed,
			Cancelled
		}

		private class KissPair
		{
			public string Dict;

			public string PlayerClip;

			public string PartnerClip;

			public float Spacing;

			public KissPair(string dictName, string playerName, string partnerName, float distance)
			{
				Dict = dictName;
				PlayerClip = playerName;
				PartnerClip = partnerName;
				Spacing = distance;
			}
		}

		private class KissSession
		{
			public Ped Player;

			public Ped Partner;

			public int PlayerHandle;

			public int PlayerModel;

			public int PartnerHandle;

			public int PartnerModel;

			public IntPtr PlayerAddress;

			public IntPtr PartnerAddress;

			public List<KissPair> Candidates;

			public int CandidateIndex;

			public int CandidateDeadline;

			public KissPair Pair;

			public FwSyncedScene SceneA;

			public FwSyncedScene SceneB;

			public Vector3 PlayerPosition;

			public Vector3 PartnerPosition;

			public bool ChangedTasks;

			public int StartedAt;

			public int DurationMs;

			public float LastPhase;

			public bool SawPlayback;

			public KissSession()
			{
				Candidates = new List<KissPair>();
			}
		}

		private const string DanceDictionary = "anim@amb@nightclub@dancers@crowddance_facedj@hi_intensity";

		private static readonly string[] DanceNumbers = new string[4] { "09", "11", "13", "15" };

		private static readonly Random random = new Random();

		private static bool dancing;

		private static bool danceRequested;

		private static Ped dancePed;

		private static string danceClip;

		private static int danceAt;

		private static Vector3 danceOrigin;

		private static int danceLoadDeadline;

		private static KissSession kiss;

		private static PairedKissOutcome kissResult = PairedKissOutcome.None;

		private static readonly HashSet<string> ownedDictionaries = new HashSet<string>(StringComparer.Ordinal);

		public static bool IsDancing
		{
			get
			{
				if (!dancing)
				{
					return danceRequested;
				}
				return true;
			}
		}

		public static bool IsKissing => kiss != null;

		public static PairedKissOutcome LastPairedKissOutcome => kissResult;

		private static void RequestDictionary(string dict)
		{
			if (!string.IsNullOrWhiteSpace(dict) && ownedDictionaries.Add(dict))
			{
				Function.Call(Hash.REQUEST_ANIM_DICT, dict);
			}
		}

		internal static void ForgetForStoryLoad()
		{
			dancing = false;
			danceRequested = false;
			dancePed = null;
			danceClip = null;
			danceAt = 0;
			danceOrigin = Vector3.Zero;
			danceLoadDeadline = 0;
			kiss = null;
			kissResult = PairedKissOutcome.None;
			ownedDictionaries.Clear();
		}

		public static void Cleanup()
		{
			CancelPairedKiss();
			StopPlayerDance();
			foreach (string ownedDictionary in ownedDictionaries)
			{
				try
				{
					Function.Call(Hash.REMOVE_ANIM_DICT, ownedDictionary);
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					ProjectData.ClearProjectError();
				}
			}
			ownedDictionaries.Clear();
		}

		public static string TimedActionOutcome(long elapsedMilliseconds, int durationMilliseconds, bool cancelRequested)
		{
			if (cancelRequested)
			{
				return "Cancelled";
			}
			if (elapsedMilliseconds >= Math.Max(0, durationMilliseconds))
			{
				return "Completed";
			}
			return "Running";
		}

		private static float ClipDuration(string dict, string clip)
		{
			if (string.IsNullOrWhiteSpace(clip))
			{
				return 0f;
			}
			float num = Function.Call<float>(Hash.GET_ANIM_DURATION, dict, clip);
			if (float.IsNaN(num) || float.IsInfinity(num) || num < 0.1f)
			{
				return 0f;
			}
			return num;
		}

		public static bool StartOrStopPlayerDance()
		{
			if (dancing || danceRequested)
			{
				StopPlayerDance();
				return false;
			}
			if (IsKissing || MansionFeatures.IsWorkoutActive || PartyService.IsPlayerActionActive || !PartyService.IsPartyActive || (object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsDead || Helper.PP.IsInVehicle() || Helper.PP.IsFalling || Helper.PP.IsRagdoll)
			{
				return false;
			}
			try
			{
				dancePed = Helper.PP;
				danceOrigin = Helper.PP.Position;
				danceRequested = true;
				danceLoadDeadline = checked(Game.GameTime + 2000);
				RequestDictionary("anim@amb@nightclub@dancers@crowddance_facedj@hi_intensity");
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Party player dance: " + ex2.Message);
				StopPlayerDance();
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		public static void StopPlayerDance()
		{
			try
			{
				if (dancing && (object)dancePed != null && dancePed.Exists() && danceClip != null)
				{
					Function.Call(Hash.STOP_ANIM_TASK, dancePed.Handle, "anim@amb@nightclub@dancers@crowddance_facedj@hi_intensity", danceClip, -4f);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Party stop player dance: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			finally
			{
				dancing = false;
				danceRequested = false;
				dancePed = null;
				danceClip = null;
			}
		}

		public static void Tick(bool active, Vector3 center)
		{
			TickPairedKiss();
			if (!dancing && !danceRequested)
			{
				return;
			}
			if (!active || (object)Helper.PP == null || !Helper.PP.Exists() || (object)dancePed == null || !dancePed.Exists() || dancePed.Handle != Helper.PP.Handle || Helper.PP.IsDead || Helper.PP.IsInVehicle() || Helper.PP.IsFalling || Helper.PP.IsRagdoll || Helper.PP.Position.DistanceTo(center) > 120f || Helper.PP.Position.DistanceTo(danceOrigin) > 2.5f)
			{
				StopPlayerDance();
				return;
			}
			if (danceRequested)
			{
				if (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@amb@nightclub@dancers@crowddance_facedj@hi_intensity"))
				{
					if (Game.GameTime >= danceLoadDeadline)
					{
						Notification.Show("The dance animation could not be loaded.");
						StopPlayerDance();
					}
					return;
				}
				string arg = (Function.Call<bool>(Hash.IS_PED_MALE, dancePed.Handle) ? "male" : "female");
				danceClip = $"hi_dance_facedj_{DanceNumbers[random.Next(DanceNumbers.Length)]}_v1_{arg}^1";
				if (ClipDuration("anim@amb@nightclub@dancers@crowddance_facedj@hi_intensity", danceClip) <= 0f)
				{
					StopPlayerDance();
					return;
				}
				Function.Call(Hash.TASK_PLAY_ANIM, dancePed.Handle, "anim@amb@nightclub@dancers@crowddance_facedj@hi_intensity", danceClip, 4f, -4f, -1, 1, 0f, false, false, false);
				danceAt = Game.GameTime;
				danceRequested = false;
				dancing = true;
				Logger.Log("Party: player dance started (" + danceClip + ")");
			}
			if (checked(Game.GameTime - danceAt) > 1000 && !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, dancePed.Handle, "anim@amb@nightclub@dancers@crowddance_facedj@hi_intensity", danceClip, 3))
			{
				StopPlayerDance();
			}
			else if (!Helper.MenuPool.IsAnyMenuOpen())
			{
				if (MovementRequested() || CancelRequested() || Game.IsControlPressed(Control.Attack) || Game.IsControlPressed(Control.Aim) || Game.IsControlPressed(Control.Jump))
				{
					StopPlayerDance();
				}
				else
				{
					Helper.ShowPersistentHelp("Dancing: move or press ~INPUT_FRONTEND_CANCEL~ to stop.");
				}
			}
		}

		public static bool MovementRequested()
		{
			if (!(Math.Abs(Game.GetDisabledControlValueNormalized(Control.MoveLeftRight)) > 0.3f))
			{
				return Math.Abs(Game.GetDisabledControlValueNormalized(Control.MoveUpDown)) > 0.3f;
			}
			return true;
		}

		private static bool CancelRequested()
		{
			if (!Game.IsControlJustPressed(Control.FrontendCancel))
			{
				return Game.IsControlJustPressed(Control.PhoneCancel);
			}
			return true;
		}

		public static bool StartPairedKiss(Ped partner, string dict = "mp_ped_interaction", string maleClip = "kisses_guy_a", string femaleClip = "kisses_girl_a")
		{
			if (MansionPetCare.IsBusy || kiss != null || MansionFeatures.IsWorkoutActive || (object)Helper.PP == null || !Helper.PP.Exists() || (object)partner == null || !partner.Exists())
			{
				return false;
			}
			Ped pP = Helper.PP;
			if (pP.IsDead || partner.IsDead || pP.IsInVehicle() || partner.IsInVehicle() || pP.Position.DistanceTo(partner.Position) > 3f || Math.Abs(pP.Position.Z - partner.Position.Z) > 0.7f)
			{
				return false;
			}
			StopPlayerDance();
			KissSession kissSession = new KissSession
			{
				Player = pP,
				Partner = partner,
				PlayerHandle = pP.Handle,
				PlayerModel = pP.Model.Hash,
				PlayerAddress = pP.MemoryAddress,
				PartnerHandle = partner.Handle,
				PartnerModel = partner.Model.Hash,
				PartnerAddress = partner.MemoryAddress
			};
			if ((!string.Equals(dict, "mp_ped_interaction", StringComparison.OrdinalIgnoreCase) || !string.Equals(maleClip, "kisses_guy_a", StringComparison.OrdinalIgnoreCase) || !string.Equals(femaleClip, "kisses_girl_a", StringComparison.OrdinalIgnoreCase)) && !string.IsNullOrWhiteSpace(dict))
			{
				kissSession.Candidates.Add(new KissPair(dict, maleClip, femaleClip, string.Equals(dict, "mp_ped_interaction", StringComparison.OrdinalIgnoreCase) ? 1.15f : 0.53f));
			}
			kissSession.Candidates.Add(new KissPair("hs3_ext-20", "cs_lestercrest_3_dual-20", "csb_georginacheng_dual-20", 0.53f));
			kissSession.Candidates.Add(new KissPair("mp_ped_interaction", "kisses_guy_a", "kisses_guy_b", 1.15f));
			kissSession.CandidateDeadline = checked(Game.GameTime + 2000);
			kiss = kissSession;
			kissResult = PairedKissOutcome.Running;
			RequestDictionary(kissSession.Candidates[0].Dict);
			return true;
		}

		public static bool TryPlayPairedKiss(Ped partner, string dict = "mp_ped_interaction", string maleClip = "kisses_guy_a", string femaleClip = "kisses_girl_a")
		{
			return StartPairedKiss(partner, dict, maleClip, femaleClip);
		}

		public static void CancelPairedKiss()
		{
			if (kiss != null)
			{
				FinishKiss(PairedKissOutcome.Cancelled);
			}
		}

		private static bool OwnKissActor(Ped actor, int handle, int model, IntPtr address)
		{
			if ((object)actor != null && actor.Exists())
			{
				return PartyCompanionPolicy.IdentityMatches(handle, model, address, actor.Handle, actor.Model.Hash, actor.MemoryAddress);
			}
			return false;
		}

		private static void TickPairedKiss()
		{
			KissSession kissSession = kiss;
			if (kissSession == null)
			{
				return;
			}
			checked
			{
				try
				{
					if (!PartyService.CompanionSessionActive || (object)Helper.PP == null || !Helper.PP.Exists() || !OwnKissActor(kissSession.Player, kissSession.PlayerHandle, kissSession.PlayerModel, kissSession.PlayerAddress) || !OwnKissActor(kissSession.Partner, kissSession.PartnerHandle, kissSession.PartnerModel, kissSession.PartnerAddress) || Helper.PP.Handle != kissSession.PlayerHandle || kissSession.Player.IsDead || kissSession.Partner.IsDead || kissSession.Player.IsInVehicle() || kissSession.Partner.IsInVehicle() || kissSession.Player.IsFalling || kissSession.Player.IsRagdoll || CancelRequested() || MovementRequested())
					{
						FinishKiss(PairedKissOutcome.Cancelled);
						return;
					}
					if (kissSession.Pair == null)
					{
						LoadKissCandidate(kissSession);
						return;
					}
					if (kissSession.Player.Position.DistanceTo(kissSession.PlayerPosition) > 2f || kissSession.Partner.Position.DistanceTo(kissSession.PartnerPosition) > 2f)
					{
						FinishKiss(PairedKissOutcome.Failed);
						return;
					}
					Function.Call(Hash.SET_ENTITY_NO_COLLISION_ENTITY, kissSession.Player.Handle, kissSession.Partner.Handle, true);
					if (kissSession.SceneA.Exists() && kissSession.SceneB.Exists())
					{
						float num = (kissSession.LastPhase = kissSession.SceneA.Phase);
						if (num > 0.01f && Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, kissSession.Player.Handle, kissSession.Pair.Dict, kissSession.Pair.PlayerClip, 3) && Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, kissSession.Partner.Handle, kissSession.Pair.Dict, kissSession.Pair.PartnerClip, 3))
						{
							kissSession.SawPlayback = true;
						}
						if (Math.Abs(kissSession.SceneB.Phase - num) > 0.015f)
						{
							kissSession.SceneB.Phase = num;
						}
						if (num >= 0.98f)
						{
							FinishKiss(kissSession.SawPlayback ? PairedKissOutcome.Succeeded : PairedKissOutcome.Failed);
							return;
						}
					}
					else if (Game.GameTime - kissSession.StartedAt > 500)
					{
						FinishKiss((kissSession.SawPlayback && kissSession.LastPhase >= 0.85f) ? PairedKissOutcome.Succeeded : PairedKissOutcome.Failed);
						return;
					}
					if (Operators.CompareString(TimedActionOutcome(unchecked((long)Game.GameTime) - unchecked((long)kissSession.StartedAt), kissSession.DurationMs, false), "Completed", false) == 0)
					{
						FinishKiss((kissSession.SawPlayback && kissSession.LastPhase >= 0.85f) ? PairedKissOutcome.Succeeded : PairedKissOutcome.Failed);
						return;
					}
					Control[] array = new Control[7]
					{
						Control.MoveLeftRight,
						Control.MoveUpDown,
						Control.Attack,
						Control.Aim,
						Control.Jump,
						Control.Sprint,
						Control.Enter
					};
					for (int i = 0; i < array.Length; i++)
					{
						Game.DisableControlThisFrame(array[i]);
					}
					Helper.ShowPersistentHelp("Press ~INPUT_FRONTEND_CANCEL~ to stop the kiss.");
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Party paired kiss tick: " + ex2.Message);
					FinishKiss(PairedKissOutcome.Failed);
					ProjectData.ClearProjectError();
				}
			}
		}

		private static void LoadKissCandidate(KissSession session)
		{
			if (session.CandidateIndex >= session.Candidates.Count)
			{
				Notification.Show("The paired kiss animation is unavailable in this game build.");
				FinishKiss(PairedKissOutcome.Failed);
				return;
			}
			KissPair kissPair = session.Candidates[session.CandidateIndex];
			checked
			{
				if (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, kissPair.Dict))
				{
					if (Game.GameTime < session.CandidateDeadline)
					{
						return;
					}
					session.CandidateIndex++;
				}
				else
				{
					if (!(ClipDuration(kissPair.Dict, kissPair.PlayerClip) <= 0f) && !(ClipDuration(kissPair.Dict, kissPair.PartnerClip) <= 0f))
					{
						BeginKissScenes(session, kissPair);
						return;
					}
					session.CandidateIndex++;
				}
				session.CandidateDeadline = Game.GameTime + 2000;
				if (session.CandidateIndex < session.Candidates.Count)
				{
					RequestDictionary(session.Candidates[session.CandidateIndex].Dict);
				}
			}
		}

		private static void BeginKissScenes(KissSession session, KissPair pair)
		{
			if (!OwnKissActor(session.Player, session.PlayerHandle, session.PlayerModel, session.PlayerAddress) || !OwnKissActor(session.Partner, session.PartnerHandle, session.PartnerModel, session.PartnerAddress))
			{
				FinishKiss(PairedKissOutcome.Cancelled);
				return;
			}
			Vector3 position = session.Player.Position;
			Vector3 vector = session.Partner.Position - position;
			vector.Z = 0f;
			if (vector.Length() < 0.1f)
			{
				vector = session.Player.ForwardVector;
			}
			else
			{
				vector.Normalize();
			}
			float num = (float)(Math.Atan2(0f - vector.X, vector.Y) * 180.0 / Math.PI);
			Vector3 vector2 = position + vector * pair.Spacing;
			vector2.Z = position.Z;
			if (!SafePairFloor(session.Player, vector2))
			{
				Notification.Show("A little more clear, level floor space is needed for a kiss.");
				FinishKiss(PairedKissOutcome.Failed);
				return;
			}
			Vector3 rotation = default(Vector3);
			Vector3 vector3 = CalibratedOrigin(pair.Dict, pair.PlayerClip, position, num, ref rotation);
			Vector3 rotation2 = default(Vector3);
			Vector3 vector4 = CalibratedOrigin(pair.Dict, pair.PartnerClip, vector2, num + 180f, ref rotation2);
			if (!Finite(vector3) || !Finite(vector4) || !Finite(rotation) || !Finite(rotation2))
			{
				FinishKiss(PairedKissOutcome.Failed);
				return;
			}
			session.Pair = pair;
			session.PlayerPosition = position;
			session.PartnerPosition = vector2;
			Function.Call(Hash.CLEAR_PED_TASKS, session.Player.Handle);
			Function.Call(Hash.CLEAR_PED_TASKS, session.Partner.Handle);
			session.ChangedTasks = true;
			Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, session.Partner.Handle, vector2.X, vector2.Y, vector2.Z, false, false, false);
			session.Partner.Heading = num + 180f;
			session.Player.Heading = num;
			session.SceneA = FwSyncedScene.Create(vector3, rotation);
			session.SceneB = FwSyncedScene.Create(vector4, rotation2);
			if ((object)session.SceneA == null || (object)session.SceneB == null)
			{
				FinishKiss(PairedKissOutcome.Failed);
				return;
			}
			session.SceneA.IsLooped = false;
			session.SceneB.IsLooped = false;
			session.SceneA.HoldsLastFrame = false;
			session.SceneB.HoldsLastFrame = false;
			session.SceneA.Rate = 0f;
			session.SceneB.Rate = 0f;
			Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, session.Player.Handle, session.SceneA.Handle, pair.Dict, pair.PlayerClip, 4f, -4f, 0, 0, 4f, 0);
			Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, session.Partner.Handle, session.SceneB.Handle, pair.Dict, pair.PartnerClip, 4f, -4f, 0, 0, 4f, 0);
			session.SceneA.Phase = 0f;
			session.SceneB.Phase = 0f;
			session.SceneA.Rate = 1f;
			session.SceneB.Rate = 1f;
			session.StartedAt = Game.GameTime;
			session.DurationMs = checked((int)Math.Round(Math.Min(20000f, Math.Max(2500f, Math.Min(ClipDuration(pair.Dict, pair.PlayerClip), ClipDuration(pair.Dict, pair.PartnerClip)) * 1000f))));
			Logger.Log($"Party: paired kiss {pair.Dict}/{pair.PlayerClip}+{pair.PartnerClip}, spacing={pair.Spacing}");
		}

		private static void FinishKiss(PairedKissOutcome result)
		{
			KissSession kissSession = kiss;
			kiss = null;
			kissResult = result;
			if (kissSession == null || !kissSession.ChangedTasks || kissSession.Pair == null)
			{
				return;
			}
			try
			{
				if (OwnKissActor(kissSession.Player, kissSession.PlayerHandle, kissSession.PlayerModel, kissSession.PlayerAddress) && Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, kissSession.Player.Handle, kissSession.Pair.Dict, kissSession.Pair.PlayerClip, 3))
				{
					Function.Call(Hash.CLEAR_PED_TASKS, kissSession.Player.Handle);
				}
				if (OwnKissActor(kissSession.Partner, kissSession.PartnerHandle, kissSession.PartnerModel, kissSession.PartnerAddress) && Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, kissSession.Partner.Handle, kissSession.Pair.Dict, kissSession.Pair.PartnerClip, 3))
				{
					Function.Call(Hash.CLEAR_PED_TASKS, kissSession.Partner.Handle);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Party paired kiss cleanup: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		private static bool SafePairFloor(Ped playerPed, Vector3 target)
		{
			IntersectFlags options = IntersectFlags.LosToEntity;
			Vector3 position = playerPed.Position;
			RaycastResult raycastResult = World.Raycast(position + new Vector3(0f, 0f, 0.4f), position - new Vector3(0f, 0f, 2f), options, playerPed);
			RaycastResult raycastResult2 = World.Raycast(target + new Vector3(0f, 0f, 0.4f), target - new Vector3(0f, 0f, 2f), options, playerPed);
			if (!raycastResult.DidHit || !raycastResult2.DidHit || Math.Abs(raycastResult.HitPosition.Z - raycastResult2.HitPosition.Z) > 0.25f)
			{
				return false;
			}
			Vector3 source = new Vector3(position.X, position.Y, raycastResult.HitPosition.Z + 1f);
			Vector3 target2 = new Vector3(target.X, target.Y, raycastResult2.HitPosition.Z + 1f);
			if (World.Raycast(source, target2, options, playerPed).DidHit)
			{
				return false;
			}
			return !World.Raycast(raycastResult2.HitPosition + new Vector3(0f, 0f, 0.2f), raycastResult2.HitPosition + new Vector3(0f, 0f, 1.9f), options, playerPed).DidHit;
		}

		private static Vector3 CalibratedOrigin(string dict, string clip, Vector3 desired, float heading, ref Vector3 rotation)
		{
			rotation = new Vector3(0f, 0f, heading - Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, dict, clip, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 2).Z);
			Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, dict, clip, desired.X, desired.Y, desired.Z, rotation.X, rotation.Y, rotation.Z, 0f, 2);
			return desired + (desired - vector);
		}

		private static bool Finite(Vector3 p)
		{
			if (!float.IsNaN(p.X) && !float.IsInfinity(p.X) && !float.IsNaN(p.Y) && !float.IsInfinity(p.Y) && !float.IsNaN(p.Z))
			{
				return !float.IsInfinity(p.Z);
			}
			return false;
		}
	}
}
