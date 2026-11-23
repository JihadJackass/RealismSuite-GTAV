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
using GTA.Native;
using GTA.UI;
using INMNativeUI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class MansionSecurity
	{
		internal sealed class NearbyPedBuffer : IDisposable
		{
			public const int SlotBytes = 8;

			public readonly int Capacity;

			private IntPtr memory;

			public IntPtr Pointer
			{
				get
				{
					if (memory == IntPtr.Zero)
					{
						throw new ObjectDisposedException("NearbyPedBuffer");
					}
					return memory;
				}
			}

			public NearbyPedBuffer(int capacity)
			{
				if (capacity < 1 || capacity > 48)
				{
					throw new ArgumentOutOfRangeException("capacity");
				}
				Capacity = capacity;
				checked
				{
					memory = Marshal.AllocHGlobal((capacity + 1) * 8);
					for (int i = 0; i <= capacity; i++)
					{
						Marshal.WriteInt64(memory, i * 8, 0L);
					}
					Marshal.WriteInt32(memory, capacity);
				}
			}

			public int[] ReadHandles(int count, int excludedHandle)
			{
				IntPtr pointer = Pointer;
				if (count < 0 || count > Capacity)
				{
					throw new InvalidDataException("Invalid nearby-ped count: " + count);
				}
				List<int> list = new List<int>(count);
				HashSet<int> hashSet = new HashSet<int>();
				checked
				{
					int num = count - 1;
					for (int i = 0; i <= num; i++)
					{
						int num2 = Marshal.ReadInt32(pointer, (i + 1) * 8);
						if (num2 > 0 && num2 != excludedHandle && hashSet.Add(num2))
						{
							list.Add(num2);
						}
					}
					return list.ToArray();
				}
			}

			public void Dispose()
			{
				if (!(memory == IntPtr.Zero))
				{
					Marshal.FreeHGlobal(memory);
					memory = IntPtr.Zero;
				}
			}

			void IDisposable.Dispose()
			{
				//ILSpy generated this explicit interface implementation from .override directive in Dispose
				this.Dispose();
			}
		}

		internal sealed class DefenseRetryState
		{
			public long NextAttempt;

			public long NextLog;

			public bool TryRun(bool stable, long now, Action work, Action<Exception> logFailure)
			{
				if (!stable || now < NextAttempt)
				{
					return false;
				}
				checked
				{
					NextAttempt = now + 750;
					try
					{
						work();
						return true;
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception obj = ex;
						NextAttempt = now + 3000;
						if (now >= NextLog)
						{
							NextLog = now + 10000;
							try
							{
								logFailure(obj);
							}
							catch (Exception projectError)
							{
								ProjectData.SetProjectError(projectError);
								ProjectData.ClearProjectError();
							}
						}
						bool result = false;
						ProjectData.ClearProjectError();
						return result;
					}
				}
			}
		}

		internal class SlotLifecycle
		{
			public int State;

			public int Candidate;

			public int StableFrames;

			public long RetryAt;

			public string Failure;

			public bool DeletePending;

			public SlotLifecycle()
			{
				Failure = "not started";
			}

			public bool StartSearch(long now)
			{
				if (DeletePending || (State != 0 && State != 4) || (State == 4 && now < RetryAt))
				{
					return false;
				}
				State = 1;
				Candidate = 0;
				StableFrames = 0;
				Failure = "searching";
				return true;
			}

			public bool RejectCandidate(string reason, int bound)
			{
				if (State != 1 || DeletePending || bound <= 0 || Candidate >= bound)
				{
					return false;
				}
				checked
				{
					Candidate++;
					Failure = reason;
					return Candidate < bound;
				}
			}

			public bool Confirm(bool clear)
			{
				if (State != 2 || DeletePending)
				{
					return false;
				}
				StableFrames = (clear ? checked(StableFrames + 1) : 0);
				if (StableFrames < 2)
				{
					return false;
				}
				State = 3;
				Failure = "ready";
				return true;
			}

			public void RetainCasualty()
			{
				if (State == 3 && !DeletePending)
				{
					State = 5;
					Failure = "fallen; body retained until security leaves";
				}
			}

			public void BeginRetirement(string reason, long retry)
			{
				State = -1;
				DeletePending = true;
				StableFrames = 0;
				Failure = reason;
				RetryAt = retry;
			}

			public bool CompleteRetirement(bool identityStillExists)
			{
				if (!DeletePending || identityStillExists)
				{
					return false;
				}
				DeletePending = false;
				State = 4;
				return true;
			}
		}

		internal class RecoveryState
		{
			public bool Pending;

			public long StableSince;

			public long NextReturnAt;

			public RecoveryState()
			{
				StableSince = -1L;
			}

			public bool Observe(bool paused, long now)
			{
				if (paused)
				{
					Pending = true;
					StableSince = -1L;
					return false;
				}
				if (Pending)
				{
					if (StableSince < 0)
					{
						StableSince = now;
					}
					if (checked(now - StableSince) < 1000)
					{
						return false;
					}
				}
				return true;
			}

			public bool ReturnDue(long now)
			{
				return now >= NextReturnAt;
			}

			public void TaskIssued()
			{
				Pending = false;
				StableSince = -1L;
			}

			public void ReturnTimedOut(long now)
			{
				Pending = true;
				StableSince = -1L;
				NextReturnAt = checked(now + 5000);
			}
		}

		internal class GuardSlot
		{
			public int Index;

			public string Role;

			public SlotLifecycle Life;

			public RecoveryState Recovery;

			public Vector3 Anchor;

			public Vector3 Floor;

			public Quaternion[] Candidates;

			public Ped Guard;

			public int GuardHandle;

			public int GuardModel;

			public IntPtr GuardAddress;

			public int RequestedFrame;

			public float Heading;

			public Stopwatch PhaseClock;

			public long NextVerify;

			public long LastDeleteLog;

			public bool Defending;

			public bool Returning;

			public Ped Target;

			public int TargetHandle;

			public int TargetModel;

			public IntPtr TargetAddress;

			public Vector3 DefenseOrigin;

			public long NextCombatTask;

			public long ReturnDeadline;

			public Quaternion[] PatrolRoute;

			public MansionPatrols.RouteState Patrol;

			public int PatrolCandidate;

			public int PatrolRequestedFrame;

			public Vector3 PatrolGoal;

			public float PatrolClosestDistance;

			public bool PatrolRejoining;

			public long NextPatrolLog;

			public GuardSlot()
			{
				Life = new SlotLifecycle();
				Recovery = new RecoveryState();
				RequestedFrame = -1;
				PhaseClock = Stopwatch.StartNew();
				PatrolRequestedFrame = -1;
			}
		}

		internal class Deployment
		{
			public Mansion Home;

			public eOwner Owner;

			public Ped Player;

			public int PlayerHandle;

			public int PlayerModel;

			public IntPtr PlayerAddress;

			public List<GuardSlot> Slots;

			public Model Model;

			public bool ModelRequested;

			public bool ModelReleased;

			public bool ModelReady;

			public bool Retiring;

			public bool WeaponRequested;

			public bool WeaponReleased;

			public int WeaponHash;

			public DefenseRetryState Defense;

			public Vector3 GateCenter;

			public int Cursor;

			public string LastStatus;

			public Stopwatch PhaseClock;

			public Deployment()
			{
				Slots = new List<GuardSlot>();
				Defense = new DefenseRetryState();
				PhaseClock = Stopwatch.StartNew();
			}
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<string, IEnumerable<string>> _0024I36_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal IEnumerable<string> _Lambda_0024__36_002D0(string key)
			{
				_Closure_0024__36_002D0 @object = new _Closure_0024__36_002D0
				{
					_0024VB_0024Local_key = key
				};
				return new eOwner[3]
				{
					eOwner.Michael,
					eOwner.Franklin,
					eOwner.Trevor
				}.Select(@object._Lambda_0024__1);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__30_002D0
		{
			public int _0024VB_0024Local_playerHandle;

			[SpecialName]
			internal int _Lambda_0024__0(IntPtr address)
			{
				return Function.Call<int>(Hash.GET_PED_NEARBY_PEDS, _0024VB_0024Local_playerHandle, address, -1);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__36_002D0
		{
			public string _0024VB_0024Local_key;

			[SpecialName]
			internal string _Lambda_0024__1(eOwner owner)
			{
				return PropertyProfiles.Section(_0024VB_0024Local_key, owner);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__39_002D0
		{
			public StringBuilder _0024VB_0024Local_text;

			public Dictionary<string, bool> _0024VB_0024Local_expected;

			public _Closure_0024__39_002D0(_Closure_0024__39_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_text = arg0._0024VB_0024Local_text;
					_0024VB_0024Local_expected = arg0._0024VB_0024Local_expected;
				}
			}

			[SpecialName]
			internal void _Lambda_0024__0(string candidate)
			{
				File.WriteAllText(candidate, _0024VB_0024Local_text.ToString(), new UTF8Encoding(false));
			}

			[SpecialName]
			internal void _Lambda_0024__1(string candidate)
			{
				Dictionary<string, bool> dictionary = ReadPreferences(candidate);
				foreach (string item in ProfileKeys())
				{
					if (dictionary[item] != _0024VB_0024Local_expected[item])
					{
						throw new IOException("Security preference verification failed: " + item);
					}
				}
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__91_002D0
		{
			internal Deployment _0024VB_0024Local_s;

			[SpecialName]
			internal void _Lambda_0024__0()
			{
				DefenseTick(_0024VB_0024Local_s, true);
			}

			[SpecialName]
			internal void _Lambda_0024__1(Exception ex)
			{
				Logger.Log("Mansion security defense deferred; deployment retained: " + _0024VB_0024Local_s.Home.Name + "; " + ex.ToString());
			}
		}

		public const int MaximumGuards = 13;

		public const int GateGuardCount = 2;

		private const int DrivewayGateIndex = 0;

		public const int GroundsGuardCount = 11;

		public const int CandidateCount = 12;

		public const float MaximumPlayerDistance = 125f;

		public const int ModelTimeoutMilliseconds = 5000;

		public const int PlacementTimeoutMilliseconds = 4000;

		public const long RecoverySettleMilliseconds = 1000L;

		public const long ReturnRetryMilliseconds = 5000L;

		public const string RuntimeConfigPath = "scripts\\SPA II\\MansionSecurity.ini";

		private static readonly string[] HomeKeys = new string[3] { "MANSION_VINEWOOD", "MANSION_RICHMAN", "MANSION_TONGVA" };

		private static readonly Dictionary<string, bool> preferences = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

		private static readonly Dictionary<string, long> retryAfter = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);

		private static readonly Stopwatch clock = Stopwatch.StartNew();

		private static bool initialized;

		private static long nextCheck;

		private static long nextTickErrorLog;

		private const string RifleName = "WEAPON_CARBINERIFLE";

		private const long ThreatScanMilliseconds = 750L;

		private const long DefenseRetryMilliseconds = 3000L;

		private const int MaximumThreatCandidates = 48;

		private static UIMenu securityMenu;

		private static Mansion menuHome;

		private static UIMenuCheckboxItem attendantItem;

		private static UIMenuItem statusItem;

		private static Deployment active;

		internal static int[] QueryNearbyPedHandles(Func<IntPtr, int> query, int excludedHandle)
		{
			using (NearbyPedBuffer nearbyPedBuffer = new NearbyPedBuffer(48))
			{
				return nearbyPedBuffer.ReadHandles(query(nearbyPedBuffer.Pointer), excludedHandle);
			}
		}

		private static int[] NearbyPedHandles(int playerHandle)
		{
			_Closure_0024__30_002D0 closure_0024__30_002D = new _Closure_0024__30_002D0();
			closure_0024__30_002D._0024VB_0024Local_playerHandle = playerHandle;
			return QueryNearbyPedHandles(closure_0024__30_002D._Lambda_0024__0, closure_0024__30_002D._0024VB_0024Local_playerHandle);
		}

		private static IEnumerable<string> ProfileKeys()
		{
			return HomeKeys.Concat(HomeKeys.SelectMany((_Closure_0024__._0024I36_002D0 != null) ? _Closure_0024__._0024I36_002D0 : (_Closure_0024__._0024I36_002D0 = _Closure_0024__._0024I._Lambda_0024__36_002D0)));
		}

		private static bool KnownHome(string key)
		{
			if (key == null)
			{
				return false;
			}
			foreach (string item in ProfileKeys())
			{
				if (string.Equals(item, key, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		internal static Dictionary<string, bool> ReadPreferences(string path)
		{
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
			foreach (string item in ProfileKeys())
			{
				dictionary[item] = true;
			}
			if (!File.Exists(path))
			{
				return dictionary;
			}
			if (new FileInfo(path).Length > 65536)
			{
				throw new IOException("Security preferences exceed the supported size.");
			}
			string key = null;
			string[] array = File.ReadAllLines(path);
			checked
			{
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i].Trim();
					if (text.Length == 0 || text.StartsWith(";", StringComparison.Ordinal) || text.StartsWith("#", StringComparison.Ordinal))
					{
						continue;
					}
					if (text.StartsWith("[", StringComparison.Ordinal) && text.EndsWith("]", StringComparison.Ordinal))
					{
						key = text.Substring(1, text.Length - 2).Trim();
						continue;
					}
					int num = text.IndexOf('=');
					if (KnownHome(key) && num >= 0 && string.Equals(text.Substring(0, num).Trim(), "Enabled", StringComparison.OrdinalIgnoreCase))
					{
						bool result;
						dictionary[key] = bool.TryParse(text.Substring(num + 1).Trim(), out result) && result;
					}
				}
				return dictionary;
			}
		}

		internal static void WritePreferences(string path, IDictionary<string, bool> values)
		{
			_Closure_0024__39_002D0 arg = new _Closure_0024__39_002D0(null);
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			arg._0024VB_0024Local_expected = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
			arg._0024VB_0024Local_text = new StringBuilder("; SPA II armed defensive mansion security. Runtime preferences; not a release default." + Environment.NewLine);
			foreach (string item in ProfileKeys())
			{
				bool value;
				arg._0024VB_0024Local_expected[item] = !values.TryGetValue(item, out value) || value;
				arg._0024VB_0024Local_text.Append("[").Append(item).Append("]")
					.AppendLine();
				arg._0024VB_0024Local_text.Append("Enabled=").Append(arg._0024VB_0024Local_expected[item] ? "true" : "false").AppendLine()
					.AppendLine();
			}
			using (AtomicFileTransaction atomicFileTransaction = AtomicFileTransaction.BeginWrite(path, arg._Lambda_0024__0, arg._Lambda_0024__1))
			{
				atomicFileTransaction.Complete();
			}
		}

		public static void Initialize()
		{
			Cleanup();
			preferences.Clear();
			retryAfter.Clear();
			try
			{
				foreach (KeyValuePair<string, bool> item in ReadPreferences("scripts\\SPA II\\MansionSecurity.ini"))
				{
					preferences[item.Key] = item.Value;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Mansion security preferences: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			initialized = true;
			nextCheck = 0L;
		}

		public static bool IsEnabled(Mansion home)
		{
			bool value;
			if (home != null && preferences.TryGetValue(home.ProfileSection(PropertyProfiles.ActiveCharacter), out value))
			{
				return value;
			}
			return false;
		}

		public static bool SetEnabled(Mansion home, bool enabled)
		{
			if (!initialized || home == null || !KnownHome(home.Key) || !home.Enabled || !home.IsOwnedByPlayer())
			{
				return false;
			}
			bool value = IsEnabled(home);
			string key = home.ProfileSection(PropertyProfiles.ActiveCharacter);
			preferences[key] = enabled;
			try
			{
				WritePreferences("scripts\\SPA II\\MansionSecurity.ini", preferences);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				preferences[key] = value;
				Logger.Log("Mansion security save: " + ex2.Message);
				Notification.Show("The security setting could not be saved. The previous setting has been kept.");
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
			if (!enabled)
			{
				Release(home);
			}
			retryAfter.Remove(home.Key);
			nextCheck = 0L;
			return true;
		}

		public static string Status(Mansion home)
		{
			if (home == null || !IsEnabled(home))
			{
				return "Off";
			}
			Deployment deployment = active;
			if (deployment == null || deployment.Home != home)
			{
				return "Standby: four grounds patrols and the existing fixed posts when nearby.";
			}
			if (deployment.Retiring)
			{
				return "Removing security personnel.";
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			checked
			{
				foreach (GuardSlot slot in deployment.Slots)
				{
					if (slot.Life.State == 3)
					{
						num++;
					}
					if (slot.Life.State == 5)
					{
						num3++;
					}
					if (slot.Life.State == 4 || slot.Life.DeletePending)
					{
						num2++;
					}
				}
				return num + "/" + deployment.Slots.Count + " ready; " + num3 + " fallen; " + num2 + " unavailable" + ((num + num3 + num2 < deployment.Slots.Count) ? "; checking positions." : ".");
			}
		}

		public static string StatusDetails(Mansion home)
		{
			Deployment deployment = active;
			if (deployment == null || deployment.Home != home)
			{
				return Status(home);
			}
			StringBuilder stringBuilder = new StringBuilder("Armed defense; four grounds patrols, two gate posts and the remaining fixed property posts. ");
			foreach (GuardSlot slot in deployment.Slots)
			{
				stringBuilder.Append(slot.Role).Append(": ").Append(slot.Life.Failure)
					.Append("; ");
			}
			return stringBuilder.ToString();
		}

		public static void Open(Mansion home, UIMenu parent)
		{
			if (initialized && home != null && home.Enabled && home.IplsLoaded && home.IsOwnedByPlayer() && MansionFeatures.AtMansion(home))
			{
				if (securityMenu == null)
				{
					securityMenu = new UIMenu("", "MANSION SECURITY", new Point(0, -107));
					securityMenu.SetBannerType(Helper.MenuBanner);
					securityMenu.MouseEdgeEnabled = false;
					Helper.MenuPool.Add(securityMenu);
					securityMenu.OnCheckboxChange += AttendantChecked;
				}
				menuHome = home;
				securityMenu.Clear();
				securityMenu.Subtitle.Caption = "SECURITY - " + home.Name.ToUpperInvariant();
				attendantItem = new UIMenuCheckboxItem("Armed property security", IsEnabled(home), "Four grounds guards patrol between property checkpoints with short watch pauses; other guards hold their existing posts. They defend you against active attackers. Saved on/off choices are retained. New homes default to on.");
				securityMenu.AddItem(attendantItem);
				statusItem = new UIMenuItem("Deployment: " + Status(home), StatusDetails(home));
				securityMenu.AddItem(statusItem);
				securityMenu.ParentMenu = parent;
				if (parent != null)
				{
					parent.Visible = false;
				}
				securityMenu.RefreshIndex();
				securityMenu.Visible = true;
			}
		}

		private static void AttendantChecked(UIMenu sender, UIMenuCheckboxItem item, bool enabled)
		{
			if (item == attendantItem)
			{
				if (!SetEnabled(menuHome, enabled))
				{
					item.Checked = IsEnabled(menuHome);
				}
				if (statusItem != null)
				{
					statusItem.Text = "Deployment: " + Status(menuHome);
					statusItem.Description = StatusDetails(menuHome);
				}
			}
		}

		public static bool IdentityMatches(int handle, int modelHash, IntPtr address, int observedHandle, int observedModel, IntPtr observedAddress)
		{
			if (handle != 0 && modelHash != 0 && address != IntPtr.Zero && handle == observedHandle && modelHash == observedModel)
			{
				return address == observedAddress;
			}
			return false;
		}

		private static Ped ResolveGuard(GuardSlot guardSlot)
		{
			if (guardSlot == null || guardSlot.GuardHandle == 0 || guardSlot.GuardModel == 0 || guardSlot.GuardAddress == IntPtr.Zero)
			{
				return null;
			}
			Ped ped = Entity.FromHandle(guardSlot.GuardHandle) as Ped;
			if ((object)ped == null || !ped.Exists() || !IdentityMatches(guardSlot.GuardHandle, guardSlot.GuardModel, guardSlot.GuardAddress, ped.Handle, ped.Model.Hash, ped.MemoryAddress))
			{
				return null;
			}
			guardSlot.Guard = ped;
			return ped;
		}

		private static bool OwnGuard(GuardSlot guardSlot)
		{
			return (object)ResolveGuard(guardSlot) != null;
		}

		private static bool ReceiptStillExists(GuardSlot guardSlot)
		{
			if (guardSlot.GuardHandle == 0)
			{
				return false;
			}
			if (guardSlot.GuardAddress == IntPtr.Zero)
			{
				return Function.Call<bool>(Hash.DOES_ENTITY_EXIST, guardSlot.GuardHandle);
			}
			return OwnGuard(guardSlot);
		}

		private static bool SamePlayer(Deployment s)
		{
			if (s != null && (object)s.Player != null && s.Player.Exists() && (object)Helper.PP != null && IdentityMatches(s.PlayerHandle, s.PlayerModel, s.PlayerAddress, Helper.PP.Handle, Helper.PP.Model.Hash, Helper.PP.MemoryAddress))
			{
				return IdentityMatches(s.PlayerHandle, s.PlayerModel, s.PlayerAddress, s.Player.Handle, s.Player.Model.Hash, s.Player.MemoryAddress);
			}
			return false;
		}

		public static bool DeploymentAllowed(bool preference, bool owned, bool enabled, bool loaded, bool nearby, bool alive, bool stable, float distance)
		{
			if (preference && owned && enabled && loaded && nearby && alive && stable && !float.IsNaN(distance) && !float.IsInfinity(distance) && distance >= 0f)
			{
				return distance <= 125f;
			}
			return false;
		}

		public static bool PropertyInteriorAllowed(int observed, int anchorInterior, int gateInterior)
		{
			if (observed >= 0)
			{
				if (observed != 0 && (anchorInterior <= 0 || observed != anchorInterior))
				{
					if (gateInterior > 0)
					{
						return observed == gateInterior;
					}
					return false;
				}
				return true;
			}
			return false;
		}

		private static bool GateBasis(Mansion home, ref Vector3 center, ref Vector3 across, ref Vector3 inward)
		{
			if (home == null || !KnownHome(home.Key) || home.DefaultGates == null || home.DefaultGates.Length != 2 || home.DefaultGates[0] == null || home.DefaultGates[1] == null || !PartyPlacement.FinitePoint(home.Reference))
			{
				return false;
			}
			Vector3 position = home.DefaultGates[0].Position;
			Vector3 position2 = home.DefaultGates[1].Position;
			if (!PartyPlacement.FinitePoint(position) || !PartyPlacement.FinitePoint(position2) || Math.Abs(position.Z - position2.Z) > 0.25f)
			{
				return false;
			}
			center = (position + position2) * 0.5f;
			across = position2 - position;
			across.Z = 0f;
			float num = across.Length();
			if (num < 1f || num > 12f)
			{
				return false;
			}
			across.Normalize();
			inward = new Vector3(0f - across.Y, across.X, 0f);
			if (Vector3.Dot(inward, home.Reference - center) < 0f)
			{
				inward = -inward;
			}
			return true;
		}

		public static Quaternion[] GateCandidates(Mansion home, int side = -1)
		{
			List<Quaternion> list = new List<Quaternion>();
			Vector3 center = default(Vector3);
			Vector3 across = default(Vector3);
			Vector3 inward = default(Vector3);
			if ((side != -1 && side != 1) || !GateBasis(home, ref center, ref across, ref inward))
			{
				return list.ToArray();
			}
			float num = home.DefaultGates[0].Position.DistanceTo(home.DefaultGates[1].Position);
			float num2 = (float)(Math.Atan2(inward.X, 0f - inward.Y) * 180.0 / Math.PI);
			if (num2 < 0f)
			{
				num2 += 360f;
			}
			if (side == -1)
			{
				Vector3 vector = new Vector3(0f - inward.Y, inward.X, 0f);
				float[] array = new float[4] { 4.25f, 4.75f, 5.25f, 5.75f };
				foreach (float num3 in array)
				{
					float[] array2 = new float[3] { 0.32f, 0.36f, 0.28f };
					foreach (float num4 in array2)
					{
						Vector3 vector2 = center + inward * num3 + vector * (num * num4);
						list.Add(new Quaternion(vector2.X, vector2.Y, center.Z, num2));
					}
				}
				return list.ToArray();
			}
			float[] array3 = new float[4] { 3f, 3.75f, 4.5f, 5.25f };
			foreach (float num5 in array3)
			{
				float[] array4 = new float[3] { 0.85f, 1.4f, 2f };
				foreach (float num6 in array4)
				{
					Vector3 vector3 = center + inward * num5 + across * ((float)side * (num * 0.5f + num6));
					list.Add(new Quaternion(vector3.X, vector3.Y, center.Z, num2));
				}
			}
			return list.ToArray();
		}

		public static Quaternion[] GroundsAnchors(Mansion home)
		{
			if (home == null || !KnownHome(home.Key))
			{
				return new Quaternion[0];
			}
			return OriginalGroundsPosts(home);
		}

		private static Quaternion[] OriginalGroundsPosts(Mansion home)
		{
			Vector3[] array;
			switch (home.Key)
			{
			case "MANSION_VINEWOOD":
				array = new Vector3[10]
				{
					new Vector3(552.7f, 698.2f, 202.3f),
					new Vector3(565.2f, 737.7f, 198.7f),
					new Vector3(567f, 747.2f, 202.2f),
					new Vector3(544f, 775.7f, 202.3f),
					new Vector3(530.4f, 761.1f, 202.3f),
					new Vector3(511.8f, 754f, 202.3f),
					new Vector3(506.3f, 713.1f, 195.2f),
					new Vector3(515.9f, 697.6f, 195.2f),
					new Vector3(536.5f, 713.2f, 198.4f),
					new Vector3(540.5f, 701.4f, 202.3f)
				};
				break;
			case "MANSION_RICHMAN":
				array = new Vector3[10]
				{
					new Vector3(-1694f, 488.3f, 129.1f),
					new Vector3(-1680.2f, 454.1f, 129.2f),
					new Vector3(-1641.9f, 464.8f, 129.2f),
					new Vector3(-1622.4f, 463.9f, 129.1f),
					new Vector3(-1614.2f, 472.6f, 129.1f),
					new Vector3(-1648.5f, 500.1f, 125.6f),
					new Vector3(-1659.9f, 484.8f, 129.1f),
					new Vector3(-1629.3f, 449.4f, 125.3f),
					new Vector3(-1611.9f, 428.7f, 122f),
					new Vector3(-1621.7f, 427.1f, 121.9f)
				};
				break;
			case "MANSION_TONGVA":
				array = new Vector3[11]
				{
					new Vector3(-2599.3f, 1895.7f, 171.5f),
					new Vector3(-2571.9f, 1884.6f, 171.5f),
					new Vector3(-2569.3f, 1888.1f, 163.7f),
					new Vector3(-2604f, 1860.6f, 167.3f),
					new Vector3(-2607.1f, 1928.8f, 167.3f),
					new Vector3(-2624f, 1902f, 163.4f),
					new Vector3(-2624.2f, 1879.9f, 163.4f),
					new Vector3(-2614.9f, 1879.1f, 163.4f),
					new Vector3(-2634.8f, 1885.8f, 160.1f),
					new Vector3(-2651.8f, 1872.5f, 160.2f),
					new Vector3(-2640.4f, 1865.8f, 160.2f)
				};
				break;
			default:
				return new Quaternion[0];
			}
			List<Quaternion> list = new List<Quaternion>();
			Vector3[] array2 = array;
			for (int i = 0; i < array2.Length; i = checked(i + 1))
			{
				Vector3 vector = array2[i];
				Vector3 vector2 = home.Reference - vector;
				float num = (float)(Math.Atan2(0f - vector2.X, vector2.Y) * 180.0 / Math.PI);
				if (num < 0f)
				{
					num += 360f;
				}
				list.Add(new Quaternion(vector.X, vector.Y, vector.Z, num));
			}
			return list.ToArray();
		}

		public static Quaternion[] GroundsCandidates(Quaternion anchor)
		{
			List<Quaternion> list = new List<Quaternion>();
			if (!PartyPlacement.FinitePoint(Helper.ToVector3(anchor)))
			{
				return list.ToArray();
			}
			list.Add(anchor);
			int num = 0;
			do
			{
				double num2 = (double)num * 2.0 * Math.PI / 11.0;
				float num3 = ((num % 2 == 0) ? 0.35f : 0.7f);
				list.Add(new Quaternion(anchor.X + num3 * (float)Math.Cos(num2), anchor.Y + num3 * (float)Math.Sin(num2), anchor.Z, anchor.W));
				num = checked(num + 1);
			}
			while (num <= 10);
			return list.ToArray();
		}

		public static bool CandidateWithinPost(Vector3 candidate, Vector3 anchor, bool gatePost)
		{
			if (PartyPlacement.FinitePoint(candidate) && PartyPlacement.FinitePoint(anchor) && Mansion.Dist2D(candidate, anchor) <= (gatePost ? 12f : 0.71f))
			{
				return Math.Abs(candidate.Z - anchor.Z) <= 0.01f;
			}
			return false;
		}

		public static bool FloorHeightAllowed(float anchorZ, float floorZ, bool gatePost)
		{
			if (!float.IsNaN(anchorZ) && !float.IsInfinity(anchorZ) && !float.IsNaN(floorZ) && !float.IsInfinity(floorZ) && floorZ >= anchorZ - (gatePost ? 3f : 1.5f))
			{
				return floorZ <= anchorZ + 0.25f;
			}
			return false;
		}

		public static bool SupportHeightsMatch(float surfaceZ, float mapFloorZ)
		{
			if (!float.IsNaN(surfaceZ) && !float.IsInfinity(surfaceZ) && !float.IsNaN(mapFloorZ) && !float.IsInfinity(mapFloorZ))
			{
				return Math.Abs(surfaceZ - mapFloorZ) <= 0.1f;
			}
			return false;
		}

		internal static bool GateDrivewayHeightAllowed(float postZ, float drivewayZ)
		{
			if (!float.IsNaN(postZ) && !float.IsInfinity(postZ) && !float.IsNaN(drivewayZ) && !float.IsInfinity(drivewayZ))
			{
				return Math.Abs(postZ - drivewayZ) <= 0.4f;
			}
			return false;
		}

		public static bool PostClearOfActor(Vector3 post, Vector3 actorPosition)
		{
			if (!PartyPlacement.FinitePoint(post) || !PartyPlacement.FinitePoint(actorPosition))
			{
				return false;
			}
			return Math.Abs(post.Z - actorPosition.Z) > 2f || Mansion.Dist2D(post, actorPosition) >= 2f;
		}

		private static bool GroundsMapSupport(GuardSlot guardSlot, Vector3 floor)
		{
			float z = guardSlot.Anchor.Z - ((guardSlot.Index < 2) ? 3.1f : 1.6f);
			Vector3[] array = new Vector3[5]
			{
				Vector3.Zero,
				new Vector3(0.27f, 0f, 0f),
				new Vector3(-0.27f, 0f, 0f),
				new Vector3(0f, 0.27f, 0f),
				new Vector3(0f, -0.27f, 0f)
			};
			bool hit = default(bool);
			Vector3 point = default(Vector3);
			Vector3 normal = default(Vector3);
			foreach (Vector3 vector in array)
			{
				Vector3 vector2 = floor + vector;
				if (!PartyPlacement.IsUsableFloorHit(PartyPlacement.PhysicalRay(new Vector3(vector2.X, vector2.Y, guardSlot.Anchor.Z + 0.35f), new Vector3(vector2.X, vector2.Y, z), 1, ref hit, ref point, ref normal, guardSlot.Guard), hit, point, normal.Z) || !SupportHeightsMatch(floor.Z, point.Z))
				{
					return false;
				}
			}
			return true;
		}

		private static bool DrivewayMapSupport(Mansion home, GuardSlot guardSlot, Vector3 floor)
		{
			Vector3 center = default(Vector3);
			Vector3 across = default(Vector3);
			Vector3 inward = default(Vector3);
			if (!GateBasis(home, ref center, ref across, ref inward))
			{
				return false;
			}
			Vector3 vector = center + inward * Vector3.Dot(floor - center, inward);
			bool hit = default(bool);
			Vector3 point = default(Vector3);
			Vector3 normal = default(Vector3);
			return PartyPlacement.IsUsableFloorHit(PartyPlacement.PhysicalRay(vector + new Vector3(0f, 0f, 0.35f), vector - new Vector3(0f, 0f, 3.1f), 1, ref hit, ref point, ref normal, guardSlot.Guard), hit, point, normal.Z) && GateDrivewayHeightAllowed(floor.Z, point.Z) && GroundsMapSupport(guardSlot, floor);
		}

		private static bool StableWorld()
		{
			if ((object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsDead || Helper.PP.IsFalling || Helper.PP.IsRagdoll || !PartyPlacement.FinitePoint(Helper.PP.Position) || DestinationStreaming.IsPlayerSafetyHeld(Helper.PP))
			{
				return false;
			}
			if (Function.Call<bool>(Hash.IS_PLAYER_SWITCH_IN_PROGRESS) || Function.Call<bool>(Hash.IS_NEW_LOAD_SCENE_ACTIVE) || Function.Call<bool>(Hash.IS_CUTSCENE_ACTIVE))
			{
				return false;
			}
			Camera renderingCamera = World.RenderingCamera;
			if ((object)renderingCamera != null && renderingCamera.Exists() && !Stability.IsSpaCamera(renderingCamera))
			{
				return false;
			}
			return Function.Call<bool>(Hash.HAS_COLLISION_LOADED_AROUND_ENTITY, Helper.PP.Handle);
		}

		private static bool Eligible(Mansion home, bool stable)
		{
			if (home == null || (object)Helper.PP == null || !Helper.PP.Exists())
			{
				return false;
			}
			Vector3 center = default(Vector3);
			Vector3 across = default(Vector3);
			Vector3 inward = default(Vector3);
			if (!GateBasis(home, ref center, ref across, ref inward))
			{
				return false;
			}
			return DeploymentAllowed(IsEnabled(home), home.IsOwnedByPlayer(), home.Enabled, home.IplsLoaded && home.SetsApplied, home.NearBy, !Helper.PP.IsDead, stable, Helper.PP.Position.DistanceTo(center));
		}

		private static void Queue(Mansion home)
		{
			Deployment deployment = new Deployment
			{
				Home = home,
				Owner = home.Owner,
				Player = Helper.PP,
				PlayerHandle = Helper.PP.Handle,
				PlayerModel = Helper.PP.Model.Hash,
				PlayerAddress = Helper.PP.MemoryAddress,
				Model = new Model("s_m_m_highsec_01"),
				GateCenter = (home.DefaultGates[0].Position + home.DefaultGates[1].Position) * 0.5f
			};
			int num = 0;
			checked
			{
				do
				{
					deployment.Slots.Add(new GuardSlot
					{
						Index = num,
						Role = "gate " + (num + 1),
						Anchor = deployment.GateCenter,
						Candidates = GateCandidates(home, (num != 0) ? 1 : (-1))
					});
					num++;
				}
				while (num <= 1);
				Quaternion[] array = GroundsAnchors(home);
				int num2 = array.Length - 1;
				for (int i = 0; i <= num2; i++)
				{
					Quaternion quaternion = array[i];
					GuardSlot guardSlot = new GuardSlot
					{
						Index = i + 2,
						Role = "grounds " + (i + 1),
						Anchor = Helper.ToVector3(quaternion),
						Candidates = GroundsCandidates(quaternion)
					};
					int[] array2 = MansionPatrols.RouteIndices(home.Key, i);
					if (array2.Length >= 2)
					{
						List<Quaternion> list = new List<Quaternion>();
						int[] array3 = array2;
						foreach (int num3 in array3)
						{
							list.Add(array[num3]);
						}
						guardSlot.PatrolRoute = list.ToArray();
						guardSlot.Patrol = new MansionPatrols.RouteState();
						guardSlot.Role = "patrol " + (i + 1);
					}
					deployment.Slots.Add(guardSlot);
				}
				active = deployment;
				if (!deployment.Model.IsValid || !deployment.Model.IsInCdImage || !deployment.Model.IsPed)
				{
					Fail(deployment, "security model is unavailable");
					return;
				}
				deployment.ModelRequested = true;
				deployment.Model.Request();
				deployment.WeaponHash = Game.GenerateHash("WEAPON_CARBINERIFLE");
				deployment.WeaponRequested = true;
				Function.Call(Hash.REQUEST_WEAPON_ASSET, deployment.WeaponHash, 31, 0);
				Logger.Log("Mansion security queued: " + home.Name + "; two gate posts, " + array.Length + " property guards including four patrols; armed defense.");
			}
		}

		private static void RequestCandidate(Deployment s, GuardSlot guardSlot)
		{
			if (guardSlot.Life.Candidate >= guardSlot.Candidates.Length)
			{
				FailSlot(s, guardSlot, "no verified clear post: " + guardSlot.Life.Failure);
				return;
			}
			Quaternion quaternion = guardSlot.Candidates[guardSlot.Life.Candidate];
			Function.Call(Hash.REQUEST_COLLISION_AT_COORD, quaternion.X, quaternion.Y, quaternion.Z);
			guardSlot.RequestedFrame = Game.FrameCount;
		}

		private static bool ClearAfterSpawn(Deployment s, GuardSlot guardSlot)
		{
			if (!OwnGuard(guardSlot) || !DestinationStreaming.PedAtDestination(guardSlot.Guard.Position, guardSlot.Floor) || !Function.Call<bool>(Hash.HAS_COLLISION_LOADED_AROUND_ENTITY, guardSlot.Guard.Handle))
			{
				return false;
			}
			Vector3[] array = new Vector3[5]
			{
				Vector3.Zero,
				new Vector3(0.27f, 0f, 0f),
				new Vector3(-0.27f, 0f, 0f),
				new Vector3(0f, 0.27f, 0f),
				new Vector3(0f, -0.27f, 0f)
			};
			bool hit = default(bool);
			Vector3 point = default(Vector3);
			Vector3 normal = default(Vector3);
			foreach (Vector3 vector in array)
			{
				Vector3 vector2 = guardSlot.Floor + vector;
				if (!PartyPlacement.IsUsableFloorHit(PartyPlacement.PhysicalRay(vector2 + new Vector3(0f, 0f, 0.28f), vector2 - new Vector3(0f, 0f, 0.28f), 81, ref hit, ref point, ref normal, guardSlot.Guard), hit, point, normal.Z) || Math.Abs(point.Z - guardSlot.Floor.Z) > 0.22f)
				{
					return false;
				}
			}
			Vector3[] array2 = PartyPlacement.BodyProbeSegments(guardSlot.Floor);
			checked
			{
				int num = array2.Length - 1;
				bool hit2 = default(bool);
				Vector3 point2 = default(Vector3);
				Vector3 normal2 = default(Vector3);
				for (int j = 0; j <= num; j += 2)
				{
					if (!PartyPlacement.BodyProbeClear(PartyPlacement.PhysicalRay(array2[j], array2[j + 1], 87, ref hit2, ref point2, ref normal2, guardSlot.Guard), hit2))
					{
						return false;
					}
				}
				if (guardSlot.Index == 0)
				{
					return DrivewayMapSupport(s.Home, guardSlot, guardSlot.Floor);
				}
				return guardSlot.Index < 2 || GroundsMapSupport(guardSlot, guardSlot.Floor);
			}
		}

		private static void CreateAttendant(GuardSlot guardSlot, Model model, Vector3 floor, float heading)
		{
			guardSlot.Floor = floor;
			guardSlot.Heading = heading;
			guardSlot.Recovery = new RecoveryState();
			guardSlot.Guard = World.CreatePed(model, floor + new Vector3(0f, 0f, 1f), heading);
			if ((object)guardSlot.Guard == null)
			{
				throw new InvalidOperationException("creation failed");
			}
			guardSlot.GuardHandle = guardSlot.Guard.Handle;
			guardSlot.GuardModel = model.Hash;
			guardSlot.Guard.IsVisible = false;
			guardSlot.GuardAddress = guardSlot.Guard.MemoryAddress;
			if (!OwnGuard(guardSlot))
			{
				throw new InvalidOperationException("entity identity unavailable");
			}
			guardSlot.Guard.IsPersistent = true;
			guardSlot.Guard.BlockPermanentEvents = true;
			guardSlot.Guard.CanRagdoll = true;
			guardSlot.Guard.CanBeTargetted = true;
			guardSlot.Guard.IsInvincible = false;
			guardSlot.Guard.Health = 250;
			guardSlot.Guard.Armor = 100;
			guardSlot.Guard.CanSwitchWeapons = true;
			guardSlot.Guard.SetCombatAttribute(CombatAttributes.AlwaysFight, false);
			guardSlot.Guard.SetCombatAttribute(CombatAttributes.AlwaysFlee, false);
			guardSlot.Guard.SetCombatAttribute(CombatAttributes.CanFightArmedPedsWhenNotArmed, false);
			guardSlot.Guard.SetCombatAttribute(CombatAttributes.CanInvestigate, false);
			guardSlot.Guard.SetCombatAttribute(CombatAttributes.HasRadio, false);
			guardSlot.Guard.SetConfigFlag(PedConfigFlagToggles.DisableShockingEvents, true);
			guardSlot.Guard.SetResetFlag(PedResetFlagToggles.SuspendInitiatedMeleeActions, true);
			Function.Call(Hash.GIVE_WEAPON_TO_PED, guardSlot.GuardHandle, Game.GenerateHash("WEAPON_CARBINERIFLE"), 600, false, true);
			Function.Call(Hash.SET_CURRENT_PED_WEAPON, guardSlot.GuardHandle, Game.GenerateHash("WEAPON_CARBINERIFLE"), true);
			Function.Call(Hash.SET_PED_ACCURACY, guardSlot.GuardHandle, 65);
			Function.Call(Hash.FREEZE_ENTITY_POSITION, guardSlot.GuardHandle, true);
			Function.Call(Hash.SET_ENTITY_COORDS, guardSlot.GuardHandle, floor.X, floor.Y, floor.Z, false, false, false, false);
			Function.Call(Hash.SET_ENTITY_HEADING, guardSlot.GuardHandle, heading);
			Function.Call(Hash.SET_ENTITY_VELOCITY, guardSlot.GuardHandle, 0f, 0f, 0f);
			Function.Call(Hash.TASK_STAND_STILL, guardSlot.GuardHandle, -1);
			guardSlot.Life.State = 2;
			guardSlot.RequestedFrame = Game.FrameCount;
			guardSlot.PhaseClock.Restart();
		}

		private static bool SeparatedFromSlots(Deployment s, GuardSlot guardSlot, Vector3 floor)
		{
			if (!PartyPlacement.FinitePoint(floor))
			{
				return false;
			}
			foreach (GuardSlot slot in s.Slots)
			{
				if (slot != guardSlot && (slot.Life.State == 2 || slot.Life.State == 3 || slot.Life.State == 5 || slot.Life.DeletePending) && slot.GuardHandle != 0 && !PostClearOfActor(floor, slot.Floor))
				{
					return false;
				}
			}
			return true;
		}

		private static void AdvanceSlot(Deployment s, GuardSlot guardSlot, bool stable)
		{
			checked
			{
				if (guardSlot.Life.DeletePending)
				{
					RetireSlot(guardSlot);
				}
				else
				{
					if (guardSlot.Life.State == 5 || (guardSlot.Life.State == 4 && clock.ElapsedMilliseconds < guardSlot.Life.RetryAt))
					{
						return;
					}
					if (guardSlot.Life.State == 3)
					{
						if (!DeployedReadyForTasks(guardSlot) || guardSlot.Defending || guardSlot.Returning)
						{
							return;
						}
						if (guardSlot.Patrol != null)
						{
							if (stable)
							{
								TryPatrolTick(s, guardSlot);
							}
						}
						else if (clock.ElapsedMilliseconds >= guardSlot.NextVerify)
						{
							guardSlot.NextVerify = clock.ElapsedMilliseconds + 1000;
							if (guardSlot.Recovery.Pending || !DestinationStreaming.PedAtDestination(guardSlot.Guard.Position, guardSlot.Floor))
							{
								ReturnToPost(s, guardSlot);
							}
						}
						return;
					}
					if (!stable)
					{
						guardSlot.PhaseClock.Stop();
						return;
					}
					guardSlot.PhaseClock.Start();
					if (guardSlot.Life.State == 0 || guardSlot.Life.State == 4)
					{
						if (guardSlot.Life.StartSearch(clock.ElapsedMilliseconds))
						{
							guardSlot.PhaseClock.Restart();
							RequestCandidate(s, guardSlot);
						}
					}
					else if (guardSlot.Life.State == 1)
					{
						if (guardSlot.PhaseClock.ElapsedMilliseconds >= 4000)
						{
							FailSlot(s, guardSlot, "post collision timed out: " + guardSlot.Life.Failure);
						}
						else if (Game.FrameCount != guardSlot.RequestedFrame)
						{
							Quaternion q = guardSlot.Candidates[guardSlot.Life.Candidate];
							string text = "outside authored post footprint";
							bool flag = CandidateWithinPost(Helper.ToVector3(q), guardSlot.Anchor, guardSlot.Index < 2);
							if (flag)
							{
								flag = PostClearOfActor(Helper.ToVector3(q), Helper.PP.Position);
								text = "player is occupying post";
							}
							Vector3 floor = default(Vector3);
							if (flag)
							{
								flag = PartyPlacement.TryStandingSpot(Helper.ToVector3(q), guardSlot.Anchor.Z - ((guardSlot.Index < 2) ? 3f : 1.5f), guardSlot.Anchor.Z + 0.25f, ref floor) && FloorHeightAllowed(guardSlot.Anchor.Z, floor.Z, guardSlot.Index < 2);
								text = PartyPlacement.LastFailureReason;
							}
							if (flag && guardSlot.Index == 0)
							{
								flag = DrivewayMapSupport(s.Home, guardSlot, floor);
								text = "not supported by the driveway floor";
							}
							else if (flag && guardSlot.Index >= 2)
							{
								flag = GroundsMapSupport(guardSlot, floor);
								text = "no matching map floor below patio furniture";
							}
							if (flag)
							{
								int observed = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, floor.X, floor.Y, floor.Z + 0.9f);
								int anchorInterior = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, guardSlot.Anchor.X, guardSlot.Anchor.Y, guardSlot.Anchor.Z);
								int gateInterior = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, s.GateCenter.X, s.GateCenter.Y, s.GateCenter.Z);
								flag = PropertyInteriorAllowed(observed, anchorInterior, gateInterior);
								text = "foreign room " + observed + " (post=" + anchorInterior + ", gate=" + gateInterior + ")";
							}
							if (flag)
							{
								flag = SeparatedFromSlots(s, guardSlot, floor);
								text = "another security post is occupying floor";
							}
							if (flag)
							{
								CreateAttendant(guardSlot, s.Model, floor, q.W);
							}
							else if (guardSlot.Life.RejectCandidate(text, guardSlot.Candidates.Length))
							{
								RequestCandidate(s, guardSlot);
							}
							else
							{
								FailSlot(s, guardSlot, "no verified clear post; " + text);
							}
						}
					}
					else
					{
						if (guardSlot.Life.State != 2 || Game.FrameCount == guardSlot.RequestedFrame)
						{
							return;
						}
						if (guardSlot.PhaseClock.ElapsedMilliseconds >= 4000)
						{
							FailSlot(s, guardSlot, "placement confirmation timed out");
							return;
						}
						guardSlot.RequestedFrame = Game.FrameCount;
						if (guardSlot.Life.Confirm(ClearAfterSpawn(s, guardSlot)))
						{
							guardSlot.Guard.IsVisible = true;
							StartIdle(guardSlot);
							guardSlot.NextVerify = clock.ElapsedMilliseconds + 1000;
							Logger.Log("Mansion security ready: " + s.Home.Name + "; " + guardSlot.Role + "; floor=" + guardSlot.Floor.ToString());
						}
					}
				}
			}
		}

		private static Vector3 PatrolCandidatePosition(Quaternion anchor, int index)
		{
			if (index == 0)
			{
				return Helper.ToVector3(anchor);
			}
			double num = (double)checked(index - 1) * Math.PI / 4.0;
			return Helper.ToVector3(anchor) + new Vector3(2.75f * (float)Math.Cos(num), 2.75f * (float)Math.Sin(num), 0f);
		}

		private static bool PatrolFloorClear(Deployment s, GuardSlot guardSlot, Quaternion anchor, Vector3 wanted, ref Vector3 floor)
		{
			Vector3 result;
			using (OutputArgument outputArgument = new OutputArgument())
			{
				if (!Function.Call<bool>(Hash.GET_SAFE_COORD_FOR_PED, wanted.X, wanted.Y, wanted.Z, false, outputArgument, 8))
				{
					return false;
				}
				result = outputArgument.GetResult<Vector3>();
			}
			if (!PartyPlacement.FinitePoint(result) || Mansion.Dist2D(result, Helper.ToVector3(anchor)) > 3.5f || !MansionPatrols.WaypointHeightAllowed(anchor.Z, result.Z))
			{
				return false;
			}
			if (!PartyPlacement.TryStandingSpot(result, anchor.Z - 1.5f, anchor.Z + 0.25f, ref floor) || !MansionPatrols.WaypointHeightAllowed(anchor.Z, floor.Z) || !PostClearOfActor(floor, Helper.PP.Position))
			{
				return false;
			}
			foreach (GuardSlot slot in s.Slots)
			{
				if (slot != guardSlot && (slot.Life.State == 2 || slot.Life.State == 3 || slot.Life.State == 5))
				{
					Vector3 actorPosition = slot.Floor;
					if (OwnGuard(slot))
					{
						actorPosition = slot.Guard.Position;
					}
					if (!PostClearOfActor(floor, actorPosition))
					{
						return false;
					}
				}
			}
			Vector3[] array = new Vector3[5]
			{
				Vector3.Zero,
				new Vector3(0.27f, 0f, 0f),
				new Vector3(-0.27f, 0f, 0f),
				new Vector3(0f, 0.27f, 0f),
				new Vector3(0f, -0.27f, 0f)
			};
			bool hit = default(bool);
			Vector3 point = default(Vector3);
			Vector3 normal = default(Vector3);
			foreach (Vector3 vector in array)
			{
				Vector3 vector2 = floor + vector;
				if (!PartyPlacement.IsUsableFloorHit(PartyPlacement.PhysicalRay(new Vector3(vector2.X, vector2.Y, anchor.Z + 0.35f), new Vector3(vector2.X, vector2.Y, anchor.Z - 1.6f), 1, ref hit, ref point, ref normal, guardSlot.Guard), hit, point, normal.Z) || !SupportHeightsMatch(floor.Z, point.Z))
				{
					return false;
				}
			}
			int observed = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, floor.X, floor.Y, floor.Z + 0.9f);
			int anchorInterior = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, anchor.X, anchor.Y, anchor.Z);
			int gateInterior = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, s.GateCenter.X, s.GateCenter.Y, s.GateCenter.Z);
			return PropertyInteriorAllowed(observed, anchorInterior, gateInterior);
		}

		private static bool WithinPatrolBounds(GuardSlot guardSlot)
		{
			Vector3 position = guardSlot.Guard.Position;
			if (!PartyPlacement.FinitePoint(position))
			{
				return false;
			}
			Vector3 vector = Helper.ToVector3(guardSlot.PatrolRoute[0]);
			Vector3 vector2 = vector;
			Quaternion[] patrolRoute = guardSlot.PatrolRoute;
			for (int i = 0; i < patrolRoute.Length; i = checked(i + 1))
			{
				Quaternion quaternion = patrolRoute[i];
				vector.X = Math.Min(vector.X, quaternion.X);
				vector2.X = Math.Max(vector2.X, quaternion.X);
				vector.Y = Math.Min(vector.Y, quaternion.Y);
				vector2.Y = Math.Max(vector2.Y, quaternion.Y);
				vector.Z = Math.Min(vector.Z, quaternion.Z);
				vector2.Z = Math.Max(vector2.Z, quaternion.Z);
			}
			return position.X >= vector.X - 12f && position.X <= vector2.X + 12f && position.Y >= vector.Y - 12f && position.Y <= vector2.Y + 12f && position.Z >= vector.Z - 3f && position.Z <= vector2.Z + 2f;
		}

		private static void PatrolPause(GuardSlot guardSlot, bool blocked)
		{
			if (DeployedReadyForTasks(guardSlot))
			{
				guardSlot.Patrol.Complete(clock.ElapsedMilliseconds, guardSlot.PatrolRoute.Length, guardSlot.Index, blocked);
				guardSlot.PatrolRequestedFrame = -1;
				guardSlot.PatrolCandidate = 0;
				Function.Call(Hash.CLEAR_PED_TASKS, guardSlot.GuardHandle);
				Function.Call(Hash.TASK_STAND_STILL, guardSlot.GuardHandle, -1);
				guardSlot.Life.Failure = (blocked ? "route blocked; checkpoint deferred" : "watching; patrol resumes shortly");
				guardSlot.Recovery.TaskIssued();
			}
		}

		private static void PatrolTick(Deployment s, GuardSlot guardSlot)
		{
			if (!DeployedReadyForTasks(guardSlot))
			{
				return;
			}
			if (guardSlot.Recovery.Pending)
			{
				StartIdle(guardSlot);
				return;
			}
			long elapsedMilliseconds = clock.ElapsedMilliseconds;
			MansionPatrols.RouteState patrol = guardSlot.Patrol;
			checked
			{
				if (patrol.Walking)
				{
					Vector3 position = guardSlot.Guard.Position;
					bool progressed = MansionPatrols.AdvancesTowardCheckpoint(position.DistanceTo(guardSlot.PatrolGoal), ref guardSlot.PatrolClosestDistance);
					bool arrived = Mansion.Dist2D(position, guardSlot.PatrolGoal) <= 1f && Math.Abs(position.Z - guardSlot.PatrolGoal.Z) <= 1.2f;
					int num = patrol.Observe(elapsedMilliseconds, arrived, progressed);
					if (WithinPatrolBounds(guardSlot))
					{
						guardSlot.PatrolRejoining = false;
					}
					else if (!guardSlot.PatrolRejoining)
					{
						num = 2;
					}
					if (num > 0)
					{
						PatrolPause(guardSlot, num == 2);
					}
				}
				else
				{
					if (!patrol.CanStart(elapsedMilliseconds))
					{
						return;
					}
					Quaternion anchor = guardSlot.PatrolRoute[patrol.TargetIndex];
					Vector3 wanted = PatrolCandidatePosition(anchor, guardSlot.PatrolCandidate);
					if (guardSlot.PatrolRequestedFrame < 0)
					{
						Function.Call(Hash.REQUEST_COLLISION_AT_COORD, wanted.X, wanted.Y, wanted.Z);
						guardSlot.PatrolRequestedFrame = Game.FrameCount;
					}
					else
					{
						if (Game.FrameCount == guardSlot.PatrolRequestedFrame)
						{
							return;
						}
						guardSlot.PatrolRequestedFrame = -1;
						Vector3 floor = default(Vector3);
						if (!PatrolFloorClear(s, guardSlot, anchor, wanted, ref floor))
						{
							guardSlot.PatrolCandidate++;
							if (guardSlot.PatrolCandidate >= 9)
							{
								PatrolPause(guardSlot, true);
							}
							return;
						}
						Function.Call(Hash.CLEAR_PED_TASKS, guardSlot.GuardHandle);
						Function.Call(Hash.FREEZE_ENTITY_POSITION, guardSlot.GuardHandle, false);
						Function.Call(Hash.SET_PED_PATH_CAN_USE_CLIMBOVERS, guardSlot.GuardHandle, false);
						Function.Call(Hash.SET_PED_PATH_CAN_USE_LADDERS, guardSlot.GuardHandle, false);
						Function.Call(Hash.SET_PED_PATH_CAN_DROP_FROM_HEIGHT, guardSlot.GuardHandle, false);
						Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, guardSlot.GuardHandle, floor.X, floor.Y, floor.Z, 1f, 90000, 0.65f, 32800, anchor.W);
						guardSlot.PatrolGoal = floor;
						guardSlot.PatrolClosestDistance = guardSlot.Guard.Position.DistanceTo(floor);
						guardSlot.PatrolRejoining = !WithinPatrolBounds(guardSlot);
						patrol.BeginWalk(elapsedMilliseconds);
						guardSlot.Recovery.TaskIssued();
						guardSlot.Life.Failure = "patrolling checkpoint " + (patrol.TargetIndex + 1);
					}
				}
			}
		}

		private static void TryPatrolTick(Deployment s, GuardSlot guardSlot)
		{
			try
			{
				PatrolTick(s, guardSlot);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				try
				{
					if (DeployedReadyForTasks(guardSlot))
					{
						Function.Call(Hash.CLEAR_PED_TASKS, guardSlot.GuardHandle);
						Function.Call(Hash.TASK_STAND_STILL, guardSlot.GuardHandle, -1);
					}
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					ProjectData.ClearProjectError();
				}
				guardSlot.Patrol.Pause(clock.ElapsedMilliseconds, 12000L);
				guardSlot.PatrolRequestedFrame = -1;
				guardSlot.PatrolCandidate = 0;
				if (clock.ElapsedMilliseconds >= guardSlot.NextPatrolLog)
				{
					guardSlot.NextPatrolLog = checked(clock.ElapsedMilliseconds + 30000);
					try
					{
						Logger.Log("Mansion patrol deferred; guard retained: " + s.Home.Name + "; " + guardSlot.Role + "; " + ex2.Message);
					}
					catch (Exception projectError2)
					{
						ProjectData.SetProjectError(projectError2);
						ProjectData.ClearProjectError();
					}
				}
				ProjectData.ClearProjectError();
			}
		}

		public static bool ThreatAllowed(bool candidateAlive, bool playerOrGuard, bool attackingProtectedActor, float distance)
		{
			if (candidateAlive && !playerOrGuard && attackingProtectedActor && !float.IsNaN(distance) && !float.IsInfinity(distance) && distance >= 0f)
			{
				return distance <= 85f;
			}
			return false;
		}

		public static string IdleScenario(int index)
		{
			switch (index % 4)
			{
			case 1:
				return "WORLD_HUMAN_STAND_MOBILE";
			case 2:
				return "WORLD_HUMAN_DRINKING";
			case 3:
				return "WORLD_HUMAN_SMOKING";
			default:
				return null;
			}
		}

		public static bool DeployedMotionPaused(bool alive, bool falling, bool ragdoll, bool gettingUp, bool melee, bool collisionLoaded)
		{
			if (alive && !falling && !ragdoll && !gettingUp && !melee)
			{
				return !collisionLoaded;
			}
			return true;
		}

		private static bool DeployedReadyForTasks(GuardSlot guardSlot)
		{
			if (guardSlot.Life.State != 3 || !OwnGuard(guardSlot))
			{
				return false;
			}
			Ped guard = guardSlot.Guard;
			bool paused = DeployedMotionPaused(!guard.IsDead, guard.IsFalling, guard.IsRagdoll, Function.Call<bool>(Hash.IS_PED_GETTING_UP, guardSlot.GuardHandle), Function.Call<bool>(Hash.IS_PED_IN_MELEE_COMBAT, guardSlot.GuardHandle), Function.Call<bool>(Hash.HAS_COLLISION_LOADED_AROUND_ENTITY, guardSlot.GuardHandle));
			if (!guardSlot.Recovery.Observe(paused, clock.ElapsedMilliseconds))
			{
				guardSlot.Defending = false;
				guardSlot.Returning = false;
				guardSlot.Target = null;
				guardSlot.TargetHandle = 0;
				guardSlot.TargetModel = 0;
				guardSlot.TargetAddress = IntPtr.Zero;
				if (guardSlot.Patrol != null)
				{
					guardSlot.Patrol.Pause(clock.ElapsedMilliseconds, MansionPatrols.WatchMilliseconds(guardSlot.Index));
					guardSlot.PatrolRequestedFrame = -1;
					guardSlot.PatrolCandidate = 0;
				}
				return false;
			}
			return true;
		}

		private static void StartIdle(GuardSlot guardSlot)
		{
			if (DeployedReadyForTasks(guardSlot))
			{
				guardSlot.Defending = false;
				guardSlot.Returning = false;
				guardSlot.Target = null;
				guardSlot.TargetHandle = 0;
				guardSlot.TargetModel = 0;
				guardSlot.TargetAddress = IntPtr.Zero;
				Function.Call(Hash.CLEAR_PED_TASKS, guardSlot.GuardHandle);
				Function.Call(Hash.FREEZE_ENTITY_POSITION, guardSlot.GuardHandle, false);
				Function.Call(Hash.SET_CURRENT_PED_WEAPON, guardSlot.GuardHandle, Game.GenerateHash("WEAPON_CARBINERIFLE"), true);
				string text = ((guardSlot.Patrol != null) ? null : IdleScenario(guardSlot.Index));
				if (guardSlot.Patrol != null)
				{
					guardSlot.Patrol.Pause(clock.ElapsedMilliseconds, MansionPatrols.WatchMilliseconds(guardSlot.Index));
					guardSlot.PatrolRequestedFrame = -1;
					guardSlot.PatrolCandidate = 0;
					guardSlot.Life.Failure = "watching; patrol resumes shortly";
				}
				if (text == null)
				{
					Function.Call(Hash.TASK_STAND_STILL, guardSlot.GuardHandle, -1);
				}
				else
				{
					Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, guardSlot.GuardHandle, text, 0, false);
				}
				guardSlot.Recovery.TaskIssued();
			}
		}

		private static bool ProtectedHandle(Deployment s, int handle)
		{
			if (handle == s.PlayerHandle)
			{
				return true;
			}
			foreach (GuardSlot slot in s.Slots)
			{
				if (slot.GuardHandle == handle && OwnGuard(slot))
				{
					return true;
				}
			}
			return false;
		}

		private static bool AttacksProtectedActor(Deployment s, Ped candidate)
		{
			if ((object)candidate == null || !candidate.Exists())
			{
				return false;
			}
			bool candidateAlive = !candidate.IsDead;
			bool playerOrGuard = ProtectedHandle(s, candidate.Handle);
			float distance = candidate.Position.DistanceTo(s.Player.Position);
			if (!ThreatAllowed(candidateAlive, playerOrGuard, true, distance))
			{
				return false;
			}
			if (!Function.Call<bool>(Hash.IS_PED_IN_COMBAT, candidate.Handle, 0))
			{
				return false;
			}
			bool flag = Function.Call<bool>(Hash.IS_PED_IN_COMBAT, candidate.Handle, s.PlayerHandle);
			if (!flag)
			{
				foreach (GuardSlot slot in s.Slots)
				{
					if (slot.Life.State == 3 && OwnGuard(slot) && Function.Call<bool>(Hash.IS_PED_IN_COMBAT, candidate.Handle, slot.GuardHandle))
					{
						flag = true;
						break;
					}
				}
			}
			return ThreatAllowed(candidateAlive, playerOrGuard, flag, distance);
		}

		private static bool SameTarget(GuardSlot guardSlot)
		{
			Ped target = guardSlot.Target;
			if ((object)target != null && target.Exists() && !target.IsDead)
			{
				return IdentityMatches(guardSlot.TargetHandle, guardSlot.TargetModel, guardSlot.TargetAddress, target.Handle, target.Model.Hash, target.MemoryAddress);
			}
			return false;
		}

		private static void ReturnToPost(Deployment s, GuardSlot guardSlot)
		{
			if (DeployedReadyForTasks(guardSlot))
			{
				guardSlot.Defending = false;
				if (DestinationStreaming.PedAtDestination(guardSlot.Guard.Position, guardSlot.Floor))
				{
					StartIdle(guardSlot);
				}
				else if (guardSlot.Recovery.ReturnDue(clock.ElapsedMilliseconds))
				{
					int num = ((guardSlot.Patrol != null) ? 90000 : 12000);
					guardSlot.Returning = true;
					guardSlot.ReturnDeadline = checked(clock.ElapsedMilliseconds + num);
					Function.Call(Hash.CLEAR_PED_TASKS, guardSlot.GuardHandle);
					Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, guardSlot.GuardHandle, guardSlot.Floor.X, guardSlot.Floor.Y, guardSlot.Floor.Z, 1f, num, 0.5f, 32800, guardSlot.Heading);
					guardSlot.Recovery.TaskIssued();
				}
			}
		}

		public static bool ReturnMayBeInterrupted(bool attackerVerified, float distance)
		{
			if (attackerVerified && !float.IsNaN(distance) && !float.IsInfinity(distance) && distance >= 0f)
			{
				return distance <= 65f;
			}
			return false;
		}

		private static void DefenseTick(Deployment s, bool stable)
		{
			if (!stable)
			{
				return;
			}
			Ped ped = null;
			float num = 86f;
			int[] array = NearbyPedHandles(s.PlayerHandle);
			checked
			{
				for (int i = 0; i < array.Length; i++)
				{
					Ped ped2 = Entity.FromHandle(array[i]) as Ped;
					if (AttacksProtectedActor(s, ped2))
					{
						float num2 = ped2.Position.DistanceTo(s.Player.Position);
						if (num2 < num)
						{
							ped = ped2;
							num = num2;
						}
					}
				}
				foreach (GuardSlot slot in s.Slots)
				{
					if (!DeployedReadyForTasks(slot))
					{
						continue;
					}
					if (slot.Returning)
					{
						if ((object)ped == null || !ReturnMayBeInterrupted(AttacksProtectedActor(s, ped), ped.Position.DistanceTo(slot.Guard.Position)))
						{
							if (DestinationStreaming.PedAtDestination(slot.Guard.Position, slot.Floor))
							{
								StartIdle(slot);
							}
							else if (clock.ElapsedMilliseconds >= slot.ReturnDeadline)
							{
								slot.Returning = false;
								slot.Recovery.ReturnTimedOut(clock.ElapsedMilliseconds);
							}
							continue;
						}
						slot.Returning = false;
					}
					if (slot.Defending && (!SameTarget(slot) || !AttacksProtectedActor(s, slot.Target) || slot.Guard.Position.DistanceTo((slot.Patrol != null) ? slot.DefenseOrigin : slot.Floor) > 20f))
					{
						ReturnToPost(s, slot);
					}
					if ((object)ped == null || ped.Position.DistanceTo(slot.Guard.Position) > 65f || (slot.Defending && SameTarget(slot) && slot.TargetHandle == ped.Handle && clock.ElapsedMilliseconds < slot.NextCombatTask) || !AttacksProtectedActor(s, ped))
					{
						continue;
					}
					slot.Target = ped;
					slot.TargetHandle = ped.Handle;
					slot.TargetModel = ped.Model.Hash;
					slot.TargetAddress = ped.MemoryAddress;
					if (SameTarget(slot))
					{
						slot.Returning = false;
						if (!slot.Defending)
						{
							slot.DefenseOrigin = slot.Guard.Position;
						}
						slot.Defending = true;
						slot.NextCombatTask = clock.ElapsedMilliseconds + 5000;
						if (slot.Patrol != null)
						{
							slot.Patrol.Pause(clock.ElapsedMilliseconds, MansionPatrols.WatchMilliseconds(slot.Index));
						}
						Function.Call(Hash.CLEAR_PED_TASKS, slot.GuardHandle);
						Function.Call(Hash.FREEZE_ENTITY_POSITION, slot.GuardHandle, false);
						Function.Call(Hash.SET_CURRENT_PED_WEAPON, slot.GuardHandle, s.WeaponHash, true);
						Function.Call(Hash.SET_PED_COMBAT_MOVEMENT, slot.GuardHandle, 0);
						Function.Call(Hash.SET_PED_COMBAT_RANGE, slot.GuardHandle, 2);
						Function.Call(Hash.TASK_COMBAT_PED, slot.GuardHandle, slot.TargetHandle, 0, 16);
						slot.Recovery.TaskIssued();
					}
				}
			}
		}

		private static void TryDefenseTick(Deployment s, bool stable)
		{
			_Closure_0024__91_002D0 closure_0024__91_002D = new _Closure_0024__91_002D0();
			closure_0024__91_002D._0024VB_0024Local_s = s;
			closure_0024__91_002D._0024VB_0024Local_s.Defense.TryRun(stable, clock.ElapsedMilliseconds, closure_0024__91_002D._Lambda_0024__0, closure_0024__91_002D._Lambda_0024__1);
		}

		public static void Tick()
		{
			try
			{
				if (statusItem != null && securityMenu != null && securityMenu.Visible)
				{
					statusItem.Text = "Deployment: " + Status(menuHome);
					statusItem.Description = StatusDetails(menuHome);
				}
				if (active != null && active.Retiring)
				{
					Retire(active);
					if (active != null)
					{
						return;
					}
				}
				if (!initialized)
				{
					return;
				}
				bool flag = StableWorld();
				Deployment deployment = active;
				if (deployment != null)
				{
					if (!SamePlayer(deployment) || deployment.Home.Owner != deployment.Owner || !Eligible(deployment.Home, true))
					{
						Release(deployment.Home);
						return;
					}
					if (!deployment.ModelReady)
					{
						if (!flag)
						{
							deployment.PhaseClock.Stop();
							return;
						}
						deployment.PhaseClock.Start();
						if (deployment.PhaseClock.ElapsedMilliseconds >= 5000)
						{
							Fail(deployment, "guard model or weapon streaming timed out");
						}
						else if (deployment.Model.IsLoaded && Function.Call<bool>(Hash.HAS_WEAPON_ASSET_LOADED, deployment.WeaponHash))
						{
							deployment.ModelReady = true;
						}
						return;
					}
					foreach (GuardSlot slot in deployment.Slots)
					{
						if (slot.Life.State != 2 && slot.Life.State != 3)
						{
							continue;
						}
						if (!OwnGuard(slot))
						{
							FailSlot(deployment, slot, "owned guard no longer exists");
						}
						else if (slot.Guard.IsDead)
						{
							if (slot.Life.State == 3)
							{
								slot.Life.RetainCasualty();
								slot.Defending = false;
								slot.Returning = false;
								Logger.Log("Mansion security fallen: " + deployment.Home.Name + "; " + slot.Role + "; body retained until deployment cleanup.");
							}
							else
							{
								FailSlot(deployment, slot, "guard died before placement confirmation");
							}
						}
						else if (slot.Life.State == 2 && (slot.Guard.IsFalling || slot.Guard.IsRagdoll || !DestinationStreaming.PedAtDestination(slot.Guard.Position, slot.Floor)))
						{
							FailSlot(deployment, slot, "guard no longer safely standing");
						}
					}
					TryDefenseTick(deployment, flag);
					GuardSlot guardSlot = deployment.Slots[deployment.Cursor];
					deployment.Cursor = checked(deployment.Cursor + 1) % deployment.Slots.Count;
					try
					{
						AdvanceSlot(deployment, guardSlot, flag);
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						FailSlot(deployment, guardSlot, "native or streaming error: " + ex2.Message);
						ProjectData.ClearProjectError();
					}
					ReportStatus(deployment);
				}
				else
				{
					if (!flag || clock.ElapsedMilliseconds < nextCheck)
					{
						return;
					}
					nextCheck = checked(clock.ElapsedMilliseconds + 1000);
					Mansion mansion = null;
					float num = 126f;
					foreach (Mansion mansion2 in Mansions.MansionList)
					{
						long value;
						if ((!retryAfter.TryGetValue(mansion2.Key, out value) || clock.ElapsedMilliseconds >= value) && Eligible(mansion2, flag))
						{
							Vector3 position = (mansion2.DefaultGates[0].Position + mansion2.DefaultGates[1].Position) * 0.5f;
							float num2 = Helper.PP.Position.DistanceTo(position);
							if (num2 < num)
							{
								mansion = mansion2;
								num = num2;
							}
						}
					}
					if (mansion != null)
					{
						Queue(mansion);
					}
				}
			}
			catch (Exception ex3)
			{
				ProjectData.SetProjectError(ex3);
				Exception ex4 = ex3;
				if (clock.ElapsedMilliseconds >= nextTickErrorLog)
				{
					Logger.Log("Mansion security tick: " + ex4.ToString());
					nextTickErrorLog = checked(clock.ElapsedMilliseconds + 10000);
				}
				if (active != null && !active.Retiring)
				{
					bool flag2 = false;
					foreach (GuardSlot slot2 in active.Slots)
					{
						if (slot2.Life.State == 3)
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						Fail(active, "native or streaming error");
					}
				}
				ProjectData.ClearProjectError();
			}
		}

		private static void ReportStatus(Deployment s)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			checked
			{
				foreach (GuardSlot slot in s.Slots)
				{
					if (slot.Life.State == 3)
					{
						num++;
					}
					if (slot.Life.State == 5)
					{
						num3++;
					}
					if (slot.Life.State == 4 || slot.Life.DeletePending)
					{
						num2++;
					}
				}
				if (num + num3 + num2 == s.Slots.Count)
				{
					string text = Status(s.Home);
					if (!string.Equals(text, s.LastStatus, StringComparison.Ordinal))
					{
						s.LastStatus = text;
						Logger.Log("Mansion security status: " + s.Home.Name + "; " + text);
						Notification.Show("Mansion security: " + text + ((num2 > 0) ? " Unavailable posts retry in 30 seconds; details in the SPA II log." : ""));
					}
				}
			}
		}

		private static void FailSlot(Deployment s, GuardSlot guardSlot, string reason)
		{
			if (!guardSlot.Life.DeletePending)
			{
				Logger.Log("Mansion security post unavailable: " + s.Home.Name + "; " + guardSlot.Role + "; candidate=" + guardSlot.Life.Candidate + "/" + guardSlot.Candidates.Length + "; " + reason);
				guardSlot.Life.BeginRetirement(reason, checked(clock.ElapsedMilliseconds + 30000));
				RetireSlot(guardSlot);
			}
		}

		private static void Fail(Deployment s, string reason)
		{
			if (s != null)
			{
				if (s.Home != null)
				{
					retryAfter[s.Home.Key] = checked(clock.ElapsedMilliseconds + 30000);
				}
				Logger.Log("Mansion security cancelled: " + ((s.Home == null) ? "unknown home" : s.Home.Name) + "; " + reason);
				Notification.Show("Mansion security unavailable: " + reason + ". Retrying when nearby.");
				Retire(s);
			}
		}

		private static void ReleaseModel(Deployment s)
		{
			if (s.ModelRequested && !s.ModelReleased)
			{
				s.ModelReleased = true;
				s.Model.MarkAsNoLongerNeeded();
				if (s.WeaponRequested && !s.WeaponReleased)
				{
					s.WeaponReleased = true;
					Function.Call(Hash.REMOVE_WEAPON_ASSET, s.WeaponHash);
				}
			}
		}

		private static void RetireSlot(GuardSlot guardSlot)
		{
			if (guardSlot == null)
			{
				return;
			}
			checked
			{
				if (!guardSlot.Life.DeletePending)
				{
					guardSlot.Life.BeginRetirement("released", clock.ElapsedMilliseconds + 30000);
				}
				try
				{
					if (OwnGuard(guardSlot))
					{
						guardSlot.Guard.IsVisible = false;
						Function.Call(Hash.SET_ENTITY_COLLISION, guardSlot.GuardHandle, false, false);
						guardSlot.Guard.Delete();
					}
					if (guardSlot.Life.CompleteRetirement(ReceiptStillExists(guardSlot)))
					{
						guardSlot.Guard = null;
						guardSlot.GuardHandle = 0;
						guardSlot.GuardModel = 0;
						guardSlot.GuardAddress = IntPtr.Zero;
						guardSlot.RequestedFrame = -1;
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					if (clock.ElapsedMilliseconds >= guardSlot.LastDeleteLog)
					{
						guardSlot.LastDeleteLog = clock.ElapsedMilliseconds + 5000;
						Logger.Log("Mansion security scoped delete retry: " + guardSlot.Role + "; " + ex2.Message);
					}
					ProjectData.ClearProjectError();
				}
			}
		}

		private static void Retire(Deployment s)
		{
			if (s == null)
			{
				return;
			}
			s.Retiring = true;
			bool flag = default(bool);
			foreach (GuardSlot slot in s.Slots)
			{
				RetireSlot(slot);
				flag = flag || slot.Life.DeletePending;
			}
			try
			{
				ReleaseModel(s);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Mansion security model release: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			if (!flag && active == s)
			{
				active = null;
			}
		}

		public static void Release(Mansion home)
		{
			if (active != null && active.Home == home)
			{
				Retire(active);
			}
			if (menuHome == home && securityMenu != null)
			{
				securityMenu.Visible = false;
			}
		}

		public static void Cleanup()
		{
			initialized = false;
			if (active != null)
			{
				Retire(active);
			}
			if (securityMenu != null)
			{
				securityMenu.Visible = false;
			}
			menuHome = null;
		}
	}
}
