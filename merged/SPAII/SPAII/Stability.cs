using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using iFruitAddon2;
using INMNativeUI;
using Metadata;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class Stability
	{
		private class PinInfo
		{
			public Vector3 Pos;

			public int PinnedAt;

			public bool Managed;

			public bool DeferredUnpin;
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Predicate<Camera> _0024I17_002D0;

			public static Predicate<Mansion> _0024I28_002D0;

			public static Predicate<Mansion> _0024I28_002D1;

			public static Func<AssemblyName, bool> _0024I29_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__17_002D0(Camera x)
			{
				if ((object)x != null)
				{
					return !x.Exists();
				}
				return true;
			}

			[SpecialName]
			internal bool _Lambda_0024__28_002D0(Mansion m)
			{
				return m.NearBy;
			}

			[SpecialName]
			internal bool _Lambda_0024__28_002D1(Mansion m)
			{
				return m.IplsLoaded;
			}

			[SpecialName]
			internal bool _Lambda_0024__29_002D0(AssemblyName r)
			{
				if (!r.Name.Equals("ScriptHookVDotNet2", StringComparison.OrdinalIgnoreCase))
				{
					return r.Name.Equals("ScriptHookVDotNet", StringComparison.OrdinalIgnoreCase);
				}
				return true;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__18_002D0
		{
			public Camera _0024VB_0024Local_camera;

			[SpecialName]
			internal bool _Lambda_0024__0(Camera owned)
			{
				if ((object)owned != null && owned.Exists())
				{
					return owned.Handle == _0024VB_0024Local_camera.Handle;
				}
				return false;
			}
		}

		private static readonly Dictionary<int, PinInfo> pinnedInteriors = new Dictionary<int, PinInfo>();

		private static int nextInteriorCheck = 0;

		public const float InteriorReleaseDistance = 200f;

		private const int PinGraceMs = 20000;

		private static readonly List<Camera> spaCameras = new List<Camera>();

		private static readonly Scaleform[] statScaleforms = new Scaleform[10];

		private static int nextHeartbeat = 0;

		public static int PinnedInteriorCount => pinnedInteriors.Count;

		public static int SpaCameraCount => spaCameras.Count;

		public static void ForgetPinsForStoryLoad()
		{
			pinnedInteriors.Clear();
		}

		public static void PinInterior(int id, Vector3 pos, bool managed = true)
		{
			if (id != 0)
			{
				Function.Call(Hash.PIN_INTERIOR_IN_MEMORY, id);
				PinInfo value = null;
				if (pinnedInteriors.TryGetValue(id, out value))
				{
					value.Pos = pos;
					value.PinnedAt = Game.GameTime;
					value.Managed = managed;
					value.DeferredUnpin = false;
				}
				else
				{
					pinnedInteriors.Add(id, new PinInfo
					{
						Pos = pos,
						PinnedAt = Game.GameTime,
						Managed = managed
					});
					Logger.Log($"interior {id} pinned (pinned: {pinnedInteriors.Count})");
				}
			}
		}

		public static void UnpinInterior(int id)
		{
			PinInfo value = null;
			if (id == 0 || !pinnedInteriors.TryGetValue(id, out value))
			{
				return;
			}
			if (CharacterReturnResidency.HoldsInterior(id))
			{
				value.DeferredUnpin = true;
				value.PinnedAt = Game.GameTime;
				return;
			}
			try
			{
				Function.Call(Hash.UNPIN_INTERIOR, id);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"UnpinInterior {id}: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
			pinnedInteriors.Remove(id);
			Logger.Log($"interior {id} unpinned (pinned: {pinnedInteriors.Count})");
		}

		public static bool IsPinnedInterior(int id)
		{
			return pinnedInteriors.ContainsKey(id);
		}

		public static void InteriorGuardTick()
		{
			if (Game.GameTime < nextInteriorCheck)
			{
				return;
			}
			checked
			{
				nextInteriorCheck = Game.GameTime + 2000;
				if (pinnedInteriors.Count == 0)
				{
					return;
				}
				Ped character = Game.Player.Character;
				if ((object)character == null || !character.Exists())
				{
					return;
				}
				Vector3 position = character.Position;
				int interior = Helper.GetInterior(position);
				List<int> list = null;
				foreach (KeyValuePair<int, PinInfo> pinnedInterior in pinnedInteriors)
				{
					PinInfo value = pinnedInterior.Value;
					if (!value.Managed && !value.DeferredUnpin)
					{
						continue;
					}
					if (pinnedInterior.Key == interior || CharacterReturnResidency.HoldsInterior(pinnedInterior.Key))
					{
						value.PinnedAt = Game.GameTime;
					}
					else if (Game.GameTime - value.PinnedAt >= 20000 && !(position.DistanceTo(value.Pos) < 200f))
					{
						if (list == null)
						{
							list = new List<int>();
						}
						list.Add(pinnedInterior.Key);
					}
				}
				if (list == null)
				{
					return;
				}
				foreach (int item in list)
				{
					if (pinnedInteriors[item].DeferredUnpin)
					{
						UnpinInterior(item);
					}
					else
					{
						ReleaseInterior(item);
					}
				}
			}
		}

		private static void ReleaseInterior(int id)
		{
			try
			{
				Function.Call(Hash.UNPIN_INTERIOR, id);
				Function.Call(Hash.SET_INTERIOR_ACTIVE, id, false);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"ReleaseInterior {id}: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
			pinnedInteriors.Remove(id);
			Logger.Log($"interior {id} released (pinned: {pinnedInteriors.Count})");
		}

		public static void ReleaseAllInteriors()
		{
			foreach (KeyValuePair<int, PinInfo> item in new List<KeyValuePair<int, PinInfo>>(pinnedInteriors))
			{
				if (item.Value.Managed && !item.Value.DeferredUnpin)
				{
					ReleaseInterior(item.Key);
				}
				else
				{
					UnpinInterior(item.Key);
				}
			}
		}

		public static void ForgetPresentationForStoryLoad()
		{
			spaCameras.Clear();
			Array.Clear(statScaleforms, 0, statScaleforms.Length);
			nextInteriorCheck = 0;
		}

		public static Camera CreateCam(Vector3 pos, Vector3 rot, float fov)
		{
			Camera camera = World.CreateCamera(pos, rot, fov);
			if ((object)camera != null)
			{
				spaCameras.RemoveAll((_Closure_0024__._0024I17_002D0 != null) ? _Closure_0024__._0024I17_002D0 : (_Closure_0024__._0024I17_002D0 = _Closure_0024__._0024I._Lambda_0024__17_002D0));
				spaCameras.Add(camera);
			}
			return camera;
		}

		public static bool IsSpaCamera(Camera camera)
		{
			_Closure_0024__18_002D0 closure_0024__18_002D = new _Closure_0024__18_002D0();
			closure_0024__18_002D._0024VB_0024Local_camera = camera;
			if ((object)closure_0024__18_002D._0024VB_0024Local_camera == null || !closure_0024__18_002D._0024VB_0024Local_camera.Exists())
			{
				return false;
			}
			return spaCameras.Any(closure_0024__18_002D._Lambda_0024__0);
		}

		public static void DestroySpaCameras()
		{
			try
			{
				if (IsSpaCamera(World.RenderingCamera))
				{
					World.RenderingCamera = null;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("SPA camera detach: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			foreach (Camera spaCamera in spaCameras)
			{
				try
				{
					if ((object)spaCamera != null && spaCamera.Exists())
					{
						spaCamera.Delete();
					}
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					ProjectData.ClearProjectError();
				}
			}
			spaCameras.Clear();
		}

		public static Scaleform StatSf(int i)
		{
			if (statScaleforms[i] == null)
			{
				statScaleforms[i] = new Scaleform($"MP_CAR_STATS_{checked(i + 1):00}");
			}
			return statScaleforms[i];
		}

		public static void ReleaseStatScaleforms()
		{
			checked
			{
				int num = statScaleforms.Length - 1;
				for (int i = 0; i <= num; i++)
				{
					if (statScaleforms[i] != null)
					{
						ReleaseScaleform(statScaleforms[i].Handle);
						statScaleforms[i] = null;
					}
				}
			}
		}

		public static void ReleaseScaleform(int handle)
		{
			if (handle == 0)
			{
				return;
			}
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				intPtr = Marshal.AllocHGlobal(8);
				Marshal.WriteInt64(intPtr, 0L);
				Marshal.WriteInt32(intPtr, handle);
				Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, intPtr);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"ReleaseScaleform {handle}: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
		}

		public static void Trace(string msg)
		{
			Logger.Log("> " + msg);
		}

		public static void HeartbeatTick(Vector3 ppos, int currentInterior)
		{
			if (Game.GameTime >= nextHeartbeat)
			{
				nextHeartbeat = checked(Game.GameTime + 60000);
				try
				{
					int count = Mansions.MansionList.FindAll((_Closure_0024__._0024I28_002D0 != null) ? _Closure_0024__._0024I28_002D0 : (_Closure_0024__._0024I28_002D0 = _Closure_0024__._0024I._Lambda_0024__28_002D0)).Count;
					int count2 = Mansions.MansionList.FindAll((_Closure_0024__._0024I28_002D1 != null) ? _Closure_0024__._0024I28_002D1 : (_Closure_0024__._0024I28_002D1 = _Closure_0024__._0024I._Lambda_0024__28_002D1)).Count;
					Logger.Log($"hb: pos={ppos.X:0},{ppos.Y:0},{ppos.Z:0} int={currentInterior} mpMap={Helper.mpMapActive} mansions near/loaded={count}/{count2} pinned={pinnedInteriors.Count} cams={spaCameras.Count} outVeh={Helper.outVehicleList.Count} party={PartyService.IsPartyActive} fps={Game.FPS:0}");
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log($"hb: {ex2.Message}");
					ProjectData.ClearProjectError();
				}
			}
		}

		public static void SelfCheck()
		{
			try
			{
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				foreach (Assembly assembly in assemblies)
				{
					string name = assembly.GetName().Name;
					switch (name)
					{
					case "INMNativeUI":
					case "Metadata":
					case "iFruitAddon2":
					case "ScriptHookVDotNet3":
					case "ScriptHookVDotNet2":
					case "SPAII":
					case "NativeUI":
					case "LemonUI.SHVDN3":
					{
						string text = "";
						try
						{
							text = assembly.Location;
						}
						catch (Exception projectError)
						{
							ProjectData.SetProjectError(projectError);
							ProjectData.ClearProjectError();
						}
						string text2 = "?";
						object[] customAttributes = assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
						if (customAttributes.Length > 0)
						{
							text2 = ((AssemblyFileVersionAttribute)customAttributes[0]).Version;
						}
						Logger.Log($"lib: {name} {assembly.GetName().Version} (file {text2}) {text}");
						if (Operators.CompareString(name, "INMNativeUI", false) == 0)
						{
							Version result = null;
							if (!Version.TryParse(text2, out result) || result.CompareTo(new Version(1, 8, 608, 9)) < 0)
							{
								Logger.Log("WARNING: this SPA II update requires INMNativeUI file version 1.8.608.9 or newer; replace both DLLs from the update.");
								Notification.Show("~y~SPA II~w~: update INMNativeUI.dll from this package too. The older menu library can prevent the assistant from opening.");
							}
						}
						break;
					}
					}
				}
				List<string> list = new List<string>();
				Type[] array = new Type[3]
				{
					typeof(Decor),
					typeof(UIMenu),
					typeof(CustomiFruit)
				};
				for (int j = 0; j < array.Length; j = checked(j + 1))
				{
					Assembly assembly2 = array[j].Assembly;
					if (assembly2.GetReferencedAssemblies().Any((_Closure_0024__._0024I29_002D0 != null) ? _Closure_0024__._0024I29_002D0 : (_Closure_0024__._0024I29_002D0 = _Closure_0024__._0024I._Lambda_0024__29_002D0)))
					{
						list.Add(assembly2.GetName().Name + ".dll");
					}
				}
				if (list.Count > 0)
				{
					Logger.Log(string.Format("WARNING: SHVDN2 version of {0} is loaded - replace with the files from this SPA II Legacy API3 package!", string.Join(", ", list)));
					Notification.Show(string.Format("~r~SPA II Legacy~w~: old {0} detected. Use the API3 files from this Legacy package.", string.Join(", ", list)));
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"SelfCheck: {ex2.Message}");
				ProjectData.ClearProjectError();
			}
		}
	}
}
