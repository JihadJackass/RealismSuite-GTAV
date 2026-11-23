using System;
using GTA.Math;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class PartyCompanionPolicy
	{
		internal class MorningGate
		{
			public long ElapsedSeconds;

			public int LastClock;

			public bool Delivered;

			public MorningGate()
			{
				LastClock = -1;
			}

			public void Arm(int clockSeconds)
			{
				LastClock = ((clockSeconds >= 0 && clockSeconds < 86400) ? clockSeconds : (-1));
				ElapsedSeconds = 0L;
				Delivered = false;
			}

			public void Observe(int clockSeconds)
			{
				if (LastClock >= 0 && clockSeconds >= 0 && clockSeconds < 86400)
				{
					int num = checked(clockSeconds - LastClock + 86400) % 86400;
					LastClock = clockSeconds;
					checked
					{
						if (num <= 43200)
						{
							ElapsedSeconds += num;
						}
					}
				}
			}

			public bool TryGreet(bool sameHome, bool sameOwner, bool bothPresent, bool quiet)
			{
				if (Delivered || LastClock < 21600 || LastClock >= 43200 || ElapsedSeconds < 21600 || !sameHome || !sameOwner || !bothPresent || !quiet)
				{
					return false;
				}
				Delivered = true;
				return true;
			}
		}

		public const string DanceDictionary = "anim@amb@nightclub@dancers@crowddance_facedj@hi_intensity";

		public const string FemaleDanceClip = "hi_dance_facedj_09_v1_female^1";

		public const string MaleDanceClip = "hi_dance_facedj_09_v1_male^1";

		public const int BedCandidateCount = 12;

		public static bool KnownAdultModel(string name)
		{
			switch ((name ?? "").ToLowerInvariant())
			{
			case "s_f_y_stripper_01":
			case "s_f_y_stripper_02":
			case "s_f_y_stripperlite":
			case "s_f_y_clubbar_01":
			case "a_f_y_topless_01":
			case "a_f_y_beach_01":
			case "a_f_y_hipster_02":
			case "a_f_y_clubcust_03":
				return true;
			default:
				return false;
			}
		}

		public static bool IdentityMatches(int handle, int model, IntPtr address, int observedHandle, int observedModel, IntPtr observedAddress)
		{
			if (handle != 0 && address != IntPtr.Zero && handle == observedHandle && model == observedModel)
			{
				return address == observedAddress;
			}
			return false;
		}

		public static bool CanJoin(bool sessionActive, bool sameHome, bool ownedAdults, bool distinctActors, int sleepingCount, float distance)
		{
			if (sessionActive && sameHome && ownedAdults && distinctActors && sleepingCount == 1 && !float.IsNaN(distance) && !float.IsInfinity(distance) && distance >= 0f)
			{
				return distance <= 8f;
			}
			return false;
		}

		public static float BedCandidateOffset(int index)
		{
			if (index < 0 || index >= 12)
			{
				return float.NaN;
			}
			return (new float[6] { 0.65f, 0.75f, 0.85f, 0.95f, 1.05f, 1.1f })[index / 2] * ((index % 2 == 0) ? 1f : (-1f));
		}

		public static bool SleepingBodyAxisValid(Vector3 head, Vector3 pelvis)
		{
			if (!PartyPlacement.FinitePoint(head) || !PartyPlacement.FinitePoint(pelvis))
			{
				return false;
			}
			Vector3 vector = head - pelvis;
			return vector.X * vector.X + vector.Y * vector.Y >= 0.09f && vector.X * vector.X + vector.Y * vector.Y <= 1.5625f && Math.Abs(vector.Z) <= 0.5f;
		}

		public static bool MattressSurfaceAcceptable(float referenceZ, float surfaceZ)
		{
			if (!float.IsNaN(referenceZ) && !float.IsInfinity(referenceZ) && !float.IsNaN(surfaceZ) && !float.IsInfinity(surfaceZ))
			{
				return Math.Abs(referenceZ - surfaceZ) <= 0.1f;
			}
			return false;
		}

		public static bool MeasuredSleepPoseConfirmed(Vector3 firstPelvis, Vector3 secondPelvis, Vector3 secondHead, Vector3 targetPelvis, Vector3 targetHead)
		{
			if (!PartyPlacement.FinitePoint(firstPelvis) || !PartyPlacement.FinitePoint(secondPelvis) || !PartyPlacement.FinitePoint(secondHead) || !PartyPlacement.FinitePoint(targetPelvis) || !PartyPlacement.FinitePoint(targetHead))
			{
				return false;
			}
			Vector3 vector = firstPelvis - secondPelvis;
			float num = vector.X * vector.X + vector.Y * vector.Y;
			return num >= 0.3025f && num <= 1.69f && Math.Abs(vector.Z) <= 0.25f && secondPelvis.DistanceTo(targetPelvis) <= 0.3f && secondHead.DistanceTo(targetHead) <= 0.35f;
		}

		public static bool SeparateSleepRoots(Vector3 first, Vector3 second, Vector3 scene)
		{
			if (!PartyPlacement.FinitePoint(first) || !PartyPlacement.FinitePoint(second) || !PartyPlacement.FinitePoint(scene))
			{
				return false;
			}
			Vector3 vector = first - second;
			float num = vector.X * vector.X + vector.Y * vector.Y;
			return num >= 0.2025f && num <= 5.0625f && Math.Abs(vector.Z) <= 0.35f && first.DistanceTo(scene) <= 3f && second.DistanceTo(scene) <= 3f;
		}
	}
}
