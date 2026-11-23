using System;
using System.Diagnostics;
using System.Globalization;
using GTA;
using GTA.Math;
using GTA.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	internal sealed class TvRenderSurface : IDisposable
	{
		private readonly Prop source;

		private readonly IntPtr sourceAddress;

		private readonly int sourceHash;

		private readonly bool wasVisible;

		private readonly Model model;

		private readonly Quaternion? authoredPose;

		private readonly bool useExisting;

		private readonly int authoredInterior;

		private readonly int authoredRoom;

		private Vector3 authoredInsertPosition;

		private readonly Stopwatch started;

		private Prop copy;

		private IntPtr copyAddress;

		private bool ownsTarget;

		private int ownedTargetId;

		private readonly bool queuedRegistration;

		private readonly int queuedTargetId;

		private bool sourceHidden;

		private int stage;

		private int lastFrame;

		private int previousId;

		private int id;

		private bool disposed;

		public readonly string Target;

		private bool failure;

		private string failureDetail;

		private bool bindingRegistered;

		private bool bindingLinked;

		private bool bindingVisible;

		private bool bindingObservedOnce;

		private int bindingId;

		private int bindingDefaultId;

		private long bindingReportedAt;

		private long bindingLinkedAt;

		private int relinkAttempts;

		public bool Failed
		{
			get
			{
				return failure;
			}
			private set
			{
				failure = value;
			}
		}

		public string FailureReason => failureDetail;

		public Prop Surface => copy;

		public int RenderId => id;

		internal int OwnedRegistrationId
		{
			get
			{
				if (!ownsTarget)
				{
					return 0;
				}
				return ownedTargetId;
			}
		}

		public bool Ready
		{
			get
			{
				if (stage == 5 && !Failed)
				{
					return !disposed;
				}
				return false;
			}
		}

		public TvRenderSurface(Prop original, string surfaceModel, string targetName, Quaternion? authored = null, int interior = 0, int room = 0, bool useExisting = false)
		{
			started = Stopwatch.StartNew();
			lastFrame = -1;
			bindingReportedAt = -1000L;
			if ((object)original != null && original.Exists())
			{
				source = original;
				sourceAddress = original.MemoryAddress;
				sourceHash = original.Model.Hash;
				wasVisible = original.IsVisible;
			}
			else if (!authored.HasValue)
			{
				throw new ArgumentException("Missing TV furniture");
			}
			if (authored.HasValue)
			{
				Vector3 vector = Helper.ToVector3(authored.Value);
				if (interior == 0 || room == 0 || !ApartmentVideoPlacement.PlacementAllowed(vector, vector, new Vector3(0f, 0f, authored.Value.W)) || ((object)source != null && source.Position.DistanceToSquared(vector) > 0.25f))
				{
					throw new ArgumentException("Invalid authored television pose");
				}
				authoredPose = authored;
				authoredInterior = interior;
				authoredRoom = room;
			}
			model = new Model(surfaceModel);
			Target = targetName;
			if (useExisting && ((object)source == null || sourceHash != model.Hash || authored.HasValue))
			{
				throw new ArgumentException("Existing video fixture does not match the requested drawable");
			}
			this.useExisting = useExisting;
			queuedRegistration = Function.Call<int>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, Target) != 0;
			if (queuedRegistration)
			{
				queuedTargetId = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, Target);
			}
		}

		public static bool LoadingExpired(int currentStage, long elapsedMilliseconds)
		{
			if (currentStage >= 0 && currentStage <= 5 && elapsedMilliseconds >= 0)
			{
				if (currentStage < 5)
				{
					return elapsedMilliseconds >= 10000;
				}
				return false;
			}
			return true;
		}

		public static bool MayResetStaleTarget(bool wasRegistered, bool sameRegistration, bool linked, int decoderChannel)
		{
			if (wasRegistered && sameRegistration && !linked)
			{
				return decoderChannel == -1;
			}
			return false;
		}

		public static bool MayReleaseTarget(bool owned, bool sameRegistration, int decoderChannel)
		{
			if (owned && sameRegistration)
			{
				return decoderChannel == -1;
			}
			return false;
		}

		public static bool BindingMayComplete(bool registered, bool linked, bool visible, int observed, int defaultId)
		{
			if (registered && linked && visible)
			{
				return UsableId(observed, defaultId);
			}
			return false;
		}

		public static bool MayRetryBinding(bool registered, bool linked, bool owned, bool sameRegistration, int decoderChannel, int attempts, long sinceLinkMilliseconds)
		{
			if (registered && !linked && owned && sameRegistration && decoderChannel == -1 && attempts >= 0 && attempts < 2)
			{
				return sinceLinkMilliseconds >= 500;
			}
			return false;
		}

		public static string BindingDiagnostic(bool registered, bool linked, bool visible, int observed, int defaultId, int previous, int attempts)
		{
			return "registered=" + registered + "; linked=" + linked + "; visible=" + visible + "; observed=" + observed.ToString(CultureInfo.InvariantCulture) + "(0x" + observed.ToString("X8") + "); default=" + defaultId.ToString(CultureInfo.InvariantCulture) + "(0x" + defaultId.ToString("X8") + "); previous=" + previous.ToString(CultureInfo.InvariantCulture) + "; relinks=" + attempts;
		}

		private void ReadBindingState()
		{
			bindingRegistered = Function.Call<int>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, Target) != 0;
			bindingLinked = Function.Call<int>(Hash.IS_NAMED_RENDERTARGET_LINKED, model.Hash) != 0;
			bindingId = (bindingRegistered ? Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, Target) : 0);
			bindingDefaultId = Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID);
			bindingVisible = CopyMatches() && copy.IsVisible;
			bindingObservedOnce = true;
		}

		private string BindingStateText()
		{
			if (!bindingObservedOnce)
			{
				return "binding=not-observed";
			}
			return BindingDiagnostic(bindingRegistered, bindingLinked, bindingVisible, bindingId, bindingDefaultId, previousId, relinkAttempts);
		}

		private void ReportBinding(string reason, bool force = false)
		{
			if (force || checked(started.ElapsedMilliseconds - bindingReportedAt) >= 1000)
			{
				bindingReportedAt = started.ElapsedMilliseconds;
				try
				{
					Logger.Log("TV binding " + reason + ": stage=" + stage + "; target=" + Target + "; elapsed=" + started.ElapsedMilliseconds + "; " + BindingStateText());
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					ProjectData.ClearProjectError();
				}
			}
		}

		private void Fail(string reason)
		{
			if (Failed)
			{
				return;
			}
			failureDetail = reason;
			Failed = true;
			try
			{
				if (stage >= 2 && CopyMatches())
				{
					ReadBindingState();
				}
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
			}
			try
			{
				Logger.Log("TV surface failed: stage=" + stage + "; furniture=" + sourceHash + "; drawable=" + model.Hash + "; target=" + Target + "; elapsed=" + started.ElapsedMilliseconds + "; reason=" + reason + "; " + BindingStateText());
			}
			catch (Exception projectError2)
			{
				ProjectData.SetProjectError(projectError2);
				ProjectData.ClearProjectError();
			}
			try
			{
				RestoreSourceVisibility();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("TV source visibility recovery: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		private void RestoreSourceVisibility()
		{
			if (sourceHidden && SourceMatches())
			{
				source.IsVisible = wasVisible;
				sourceHidden = false;
			}
		}

		internal static bool AuthoredInsertPoseAllowed(Vector3 position, Vector3 rotation, Vector3 actual, Vector3 actualRotation, float maximumModelOffset)
		{
			float num = actual.Z - position.Z;
			if (maximumModelOffset >= 0f && !float.IsInfinity(maximumModelOffset) && num >= -0.005f && num <= maximumModelOffset + 0.005f)
			{
				return ApartmentVideoPlacement.ExactPoseMatches(new Vector3(position.X, position.Y, actual.Z), rotation, actual, actualRotation);
			}
			return false;
		}

		private bool ExactReplacementAligned()
		{
			if (useExisting)
			{
				return SourceMatches();
			}
			if (authoredPose.HasValue)
			{
				return CopyMatches() && ApartmentVideoPlacement.ExactPoseMatches(authoredInsertPosition, new Vector3(0f, 0f, authoredPose.Value.W), Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, copy.Handle, false), Function.Call<Vector3>(Hash.GET_ENTITY_ROTATION, copy.Handle, 2));
			}
			if (sourceHash != model.Hash)
			{
				return true;
			}
			if (!SourceMatches() || !CopyMatches())
			{
				return false;
			}
			return ApartmentVideoPlacement.ExactPoseMatches(Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, source.Handle, false), Function.Call<Vector3>(Hash.GET_ENTITY_ROTATION, source.Handle, 2), Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, copy.Handle, false), Function.Call<Vector3>(Hash.GET_ENTITY_ROTATION, copy.Handle, 2));
		}

		private void HideReplacedSource()
		{
			if (!useExisting && wasVisible && sourceHash == model.Hash)
			{
				sourceHidden = true;
				source.IsVisible = false;
			}
		}

		private void RetryOriginalLink()
		{
			int num = (bindingRegistered ? Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, Target) : 0);
			checked
			{
				if (MayRetryBinding(bindingRegistered, bindingLinked, ownsTarget, num == ownedTargetId, Function.Call<int>(Hash.GET_TV_CHANNEL), relinkAttempts, started.ElapsedMilliseconds - bindingLinkedAt))
				{
					Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, Target);
					ownsTarget = false;
					ownedTargetId = 0;
					previousId = 0;
					Function.Call(Hash.LINK_NAMED_RENDERTARGET, model.Hash);
					relinkAttempts++;
					bindingLinkedAt = started.ElapsedMilliseconds;
					ReadBindingState();
					if (bindingRegistered)
					{
						ownsTarget = true;
						ownedTargetId = bindingId;
					}
					ReportBinding("original relink", true);
					if (!bindingRegistered)
					{
						stage = 2;
					}
				}
			}
		}

		internal bool ViewerMayRemain(Ped actor, int expectedInterior)
		{
			if ((object)actor == null || !actor.Exists() || !SourceMatches())
			{
				return false;
			}
			return ApartmentProps.PlaybackMayRemain(true, true, expectedInterior, Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, actor.Handle), actor.Position.DistanceToSquared(source.Position));
		}

		public static bool UsableId(int value, int defaultId)
		{
			if (value != 0)
			{
				return value != defaultId;
			}
			return false;
		}

		private bool SourceMatches()
		{
			if ((object)source != null && source.Exists() && source.Model.Hash == sourceHash)
			{
				return source.MemoryAddress == sourceAddress;
			}
			return false;
		}

		private bool CopyMatches()
		{
			if ((object)copy != null && copy.Exists() && copy.Model.Hash == model.Hash)
			{
				return copy.MemoryAddress == copyAddress;
			}
			return false;
		}

		internal bool OwnsSurface(int handle)
		{
			if (!useExisting && !disposed && handle > 0 && CopyMatches())
			{
				return copy.Handle == handle;
			}
			return false;
		}

		internal bool OwnsSource(Prop original)
		{
			if (!disposed && (object)original != null && SourceMatches() && original.Handle == source.Handle)
			{
				return original.MemoryAddress == sourceAddress;
			}
			return false;
		}

		private bool PrepareNamedTarget()
		{
			if (Function.Call<int>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, Target) == 0 || Function.Call<int>(Hash.IS_NAMED_RENDERTARGET_LINKED, model.Hash) != 0)
			{
				return true;
			}
			int num = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, Target);
			if (!MayResetStaleTarget(queuedRegistration, num == queuedTargetId, false, Function.Call<int>(Hash.GET_TV_CHANNEL)))
			{
				Fail("named target changed or belongs to an active television");
				return false;
			}
			Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, Target);
			return true;
		}

		public void Advance()
		{
			if (disposed || Failed)
			{
				return;
			}
			if (LoadingExpired(stage, started.ElapsedMilliseconds))
			{
				Fail("loading deadline expired");
			}
			else
			{
				if (lastFrame == Game.FrameCount)
				{
					return;
				}
				lastFrame = Game.FrameCount;
				if (((object)source != null || !authoredPose.HasValue) && !SourceMatches())
				{
					Fail("source furniture identity changed");
					return;
				}
				if (stage >= 2 && !CopyMatches())
				{
					Fail("owned video drawable identity changed");
					return;
				}
				if (stage < 5 && Function.Call<int>(Hash.GET_TV_CHANNEL) >= 0)
				{
					Fail("another decoder became active before binding");
					return;
				}
				switch (stage)
				{
				case 0:
					if (useExisting)
					{
						if (PrepareNamedTarget())
						{
							copy = source;
							copyAddress = sourceAddress;
							stage = 2;
						}
					}
					else if (!model.IsValid || !model.IsInCdImage)
					{
						Fail("video drawable is unavailable in this game");
					}
					else
					{
						model.Request();
						stage = 1;
					}
					break;
				case 1:
				{
					if (!model.IsLoaded)
					{
						model.Request();
						break;
					}
					Vector3 point = default(Vector3);
					Vector3 rotation = default(Vector3);
					if (authoredPose.HasValue)
					{
						point = Helper.ToVector3(authoredPose.Value);
						rotation = new Vector3(0f, 0f, authoredPose.Value.W);
					}
					else
					{
						if (!ApartmentVideoPlacement.SurfacePairSupported(sourceHash, model.Hash))
						{
							Fail("source and video drawable have no verified placement pairing");
							break;
						}
						if (sourceHash == model.Hash)
						{
							if (!ApartmentVideoPlacement.TrySurfacePose(source, model.Hash, ref point, ref rotation))
							{
								break;
							}
						}
						else if (!ApartmentVideoPlacement.TryPose(source, ref point, ref rotation))
						{
							break;
						}
					}
					if (!PrepareNamedTarget())
					{
						break;
					}
					bool flag = sourceHash == model.Hash && !authoredPose.HasValue;
					int handle = Function.Call<int>(flag ? Hash.CREATE_OBJECT_NO_OFFSET : Hash.CREATE_OBJECT, model.Hash, point.X, point.Y, point.Z, false, false, false);
					copy = Entity.FromHandle(handle) as Prop;
					if ((object)copy == null || !copy.Exists())
					{
						Fail("video drawable creation failed");
						break;
					}
					copyAddress = copy.MemoryAddress;
					copy.IsPersistent = true;
					Function.Call(Hash.SET_ENTITY_ROTATION, copy.Handle, rotation.X, rotation.Y, rotation.Z, 2, true);
					if (!flag)
					{
						Function.Call(Hash.SET_ENTITY_COORDS, copy.Handle, point.X, point.Y, point.Z, authoredPose.HasValue, false, false, false);
					}
					if (authoredPose.HasValue)
					{
						Vector3 min;
						Vector3 max;
						model.GetDimensions(out min, out max);
						Vector3 vector = new Vector3(Math.Max(Math.Abs(min.X), Math.Abs(max.X)), Math.Max(Math.Abs(min.Y), Math.Abs(max.Y)), Math.Max(Math.Abs(min.Z), Math.Abs(max.Z)));
						Vector3 actual = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, copy.Handle, false);
						if (!AuthoredInsertPoseAllowed(point, rotation, actual, Function.Call<Vector3>(Hash.GET_ENTITY_ROTATION, copy.Handle, 2), vector.Length()))
						{
							Fail("authored insert did not retain the stock placement");
							break;
						}
						authoredInsertPosition = actual;
					}
					if (!ExactReplacementAligned())
					{
						Fail("replacement did not retain its source pose");
						break;
					}
					if (authoredPose.HasValue && (Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, copy.Handle) != authoredInterior || Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, copy.Handle) != authoredRoom))
					{
						Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, copy.Handle, authoredInterior, authoredRoom);
					}
					copy.IsPositionFrozen = true;
					copy.IsCollisionEnabled = false;
					copy.IsVisible = true;
					Function.Call(Hash.SET_ENTITY_LOD_DIST, copy.Handle, 200);
					Logger.Log("TV surface pose: furniture=" + sourceHash + "; requested=" + point.ToString() + "; rotation=" + rotation.ToString() + "; actual=" + Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, copy.Handle, false).ToString());
					stage = 2;
					break;
				}
				case 2:
					if (Function.Call<int>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, Target) == 0)
					{
						Function.Call(Hash.REGISTER_NAMED_RENDERTARGET, Target, false);
						ownsTarget = true;
					}
					stage = 3;
					break;
				case 3:
					if (Function.Call<int>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, Target) == 0)
					{
						stage = 2;
						break;
					}
					if (ownsTarget)
					{
						ownedTargetId = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, Target);
					}
					Function.Call(Hash.LINK_NAMED_RENDERTARGET, model.Hash);
					bindingLinkedAt = started.ElapsedMilliseconds;
					stage = 4;
					break;
				case 4:
					ReadBindingState();
					ReportBinding("waiting");
					if (!bindingRegistered)
					{
						stage = 2;
						previousId = 0;
						break;
					}
					if (!bindingLinked)
					{
						previousId = 0;
						RetryOriginalLink();
						break;
					}
					if (!BindingMayComplete(bindingRegistered, bindingLinked, bindingVisible, bindingId, bindingDefaultId))
					{
						previousId = (UsableId(bindingId, bindingDefaultId) ? bindingId : 0);
						break;
					}
					if (!ExactReplacementAligned())
					{
						Fail("replacement no longer matches the physical screen");
						break;
					}
					id = bindingId;
					if (ownsTarget)
					{
						if (ownedTargetId == 0)
						{
							ownedTargetId = bindingId;
						}
						else if (ownedTargetId != bindingId)
						{
							ownsTarget = false;
							ownedTargetId = 0;
						}
					}
					HideReplacedSource();
					if (!useExisting)
					{
						copy.IsVisible = true;
					}
					stage = 5;
					ReportBinding("ready", true);
					Logger.Log("TV surface bound: furniture=" + sourceHash + "; drawable=" + model.Hash + "; target=" + Target + "; opaqueRenderId=" + id);
					break;
				case 5:
					ReadBindingState();
					if (!BindingMayComplete(bindingRegistered, bindingLinked, bindingVisible, bindingId, bindingDefaultId))
					{
						Fail("video target registration, link or usable handle was lost");
					}
					else if (!ExactReplacementAligned())
					{
						Fail("physical screen pose changed during playback");
					}
					else if (id != bindingId)
					{
						previousId = id;
						id = bindingId;
						ownsTarget = false;
						ownedTargetId = 0;
						ReportBinding("handle changed", true);
					}
					break;
				}
			}
		}

		public void Dispose()
		{
			if (disposed)
			{
				return;
			}
			disposed = true;
			try
			{
				if (sourceHidden && SourceMatches())
				{
					RestoreSourceVisibility();
				}
			}
			finally
			{
				try
				{
					if (!useExisting && CopyMatches())
					{
						copy.Delete();
					}
				}
				finally
				{
					try
					{
						if (ownsTarget && ownedTargetId != 0 && Function.Call<int>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, Target) != 0 && MayReleaseTarget(true, Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, Target) == ownedTargetId, Function.Call<int>(Hash.GET_TV_CHANNEL)))
						{
							Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, Target);
						}
					}
					finally
					{
						if (!useExisting)
						{
							model.MarkAsNoLongerNeeded();
						}
						copy = null;
						id = 0;
						stage = 0;
					}
				}
			}
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Dispose
			this.Dispose();
		}
	}
}
