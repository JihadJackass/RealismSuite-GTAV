using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class SpaBlips
	{
		internal class OwnedBlip
		{
			public Blip Marker;

			public Vector3 Position;

			public BlipSprite Sprite;

			public BlipColor Color;

			public int NativeType;

			public Vehicle Vehicle;

			public int VehicleModel;

			public IntPtr VehicleAddress;

			public SpaBlipVisibility Visibility;

			public bool Matches(int handle)
			{
				if (Marker.Handle != handle || !Marker.Exists() || Function.Call<int>(Hash.GET_BLIP_INFO_ID_TYPE, handle) != NativeType)
				{
					return false;
				}
				int num = Function.Call<int>(Hash.GET_BLIP_INFO_ID_ENTITY_INDEX, handle);
				if ((object)Vehicle == null)
				{
					return num == 0 && Marker.Sprite == Sprite && Marker.Color == Color && Marker.Position.DistanceToSquared(Position) <= 0.01f;
				}
				return VehicleAddress != IntPtr.Zero && num == Vehicle.Handle && Vehicle.Exists() && Vehicle.MemoryAddress == VehicleAddress && Vehicle.Model.Hash == VehicleModel;
			}

			public bool SetHidden(bool hide)
			{
				return Visibility.Apply(hide, _Lambda_0024__11_002D0, _Lambda_0024__11_002D1, _Lambda_0024__11_002D2);
			}

			[SpecialName]
			[CompilerGenerated]
			private int _Lambda_0024__11_002D0()
			{
				return Marker.Alpha;
			}

			[SpecialName]
			[CompilerGenerated]
			private void _Lambda_0024__11_002D1(int alpha)
			{
				Marker.Alpha = alpha;
			}

			[SpecialName]
			[CompilerGenerated]
			private void _Lambda_0024__11_002D2(bool value)
			{
				Function.Call(Hash.SET_BLIP_HIDDEN_ON_LEGEND, Marker.Handle, value);
			}
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<KeyValuePair<int, OwnedBlip>, bool> _0024I15_002D0;

			public static Func<KeyValuePair<int, OwnedBlip>, int> _0024I15_002D1;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__15_002D0(KeyValuePair<int, OwnedBlip> entry)
			{
				return (object)entry.Value.Vehicle != null;
			}

			[SpecialName]
			internal int _Lambda_0024__15_002D1(KeyValuePair<int, OwnedBlip> entry)
			{
				return entry.Key;
			}
		}

		internal const Keys DefaultKey = Keys.F10;

		internal static Keys ToggleKey = Keys.F10;

		private static readonly Dictionary<int, OwnedBlip> markers = new Dictionary<int, OwnedBlip>();

		private static bool hidden;

		private static bool keyDown;

		private static int additions;

		internal static bool HandleKey(Keys key)
		{
			if (ToggleKey == Keys.None || key != ToggleKey || key == MansionFeatures.AssistantKey || key == MansionFeatures.MansionMenuKey)
			{
				return false;
			}
			if (keyDown)
			{
				return true;
			}
			keyDown = true;
			hidden = !hidden;
			int num = RefreshMarkers(true);
			Logger.Log($"SPA II map blip toggle: hidden={hidden}; changed owned blips={num}; tracked={markers.Count}.");
			Notification.Show(hidden ? "SPA II map blips hidden." : "SPA II map blips shown.");
			return true;
		}

		internal static void ReleaseKey(Keys key)
		{
			if (key == ToggleKey)
			{
				keyDown = false;
			}
		}

		private static int RefreshMarkers(bool applyVisibility)
		{
			int[] array = markers.Keys.ToArray();
			int num2 = default(int);
			foreach (int num in array)
			{
				OwnedBlip ownedBlip = markers[num];
				try
				{
					if (!ownedBlip.Matches(num))
					{
						markers.Remove(num);
					}
					else if (applyVisibility && ownedBlip.SetHidden(hidden))
					{
						num2 = checked(num2 + 1);
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("SPA II map blip update: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
			return num2;
		}

		private static bool Register(OwnedBlip item)
		{
			int handle = item.Marker.Handle;
			item.NativeType = Function.Call<int>(Hash.GET_BLIP_INFO_ID_TYPE, handle);
			if (!item.Matches(handle))
			{
				return false;
			}
			item.Visibility = new SpaBlipVisibility(item.Marker.Alpha);
			markers[handle] = item;
			if (hidden)
			{
				item.SetHidden(true);
			}
			checked
			{
				additions++;
				if (additions >= 64)
				{
					additions = 0;
					RefreshMarkers(false);
				}
				return true;
			}
		}

		internal static void RegisterCoordinate(Blip marker)
		{
			try
			{
				if ((object)marker != null && marker.Exists())
				{
					Register(new OwnedBlip
					{
						Marker = marker,
						Position = marker.Position,
						Sprite = marker.Sprite,
						Color = marker.Color
					});
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("SPA II map blip registration: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		internal static void SetMissionHidden(Blip marker, bool missionHidden)
		{
			if ((object)marker == null)
			{
				return;
			}
			OwnedBlip value = null;
			if (markers.TryGetValue(marker.Handle, out value))
			{
				if (!value.Matches(marker.Handle))
				{
					markers.Remove(marker.Handle);
					return;
				}
				value.Visibility.MissionHidden = missionHidden;
				value.SetHidden(hidden);
			}
		}

		internal static void RecordCoordinateMove(Blip marker)
		{
			OwnedBlip value = null;
			if ((object)marker != null && markers.TryGetValue(marker.Handle, out value) && (object)value.Marker == marker && (object)value.Vehicle == null)
			{
				value.Position = marker.Position;
			}
		}

		internal static void ForgetForStoryLoad()
		{
			keyDown = false;
			int[] array = markers.Where((_Closure_0024__._0024I15_002D0 != null) ? _Closure_0024__._0024I15_002D0 : (_Closure_0024__._0024I15_002D0 = _Closure_0024__._0024I._Lambda_0024__15_002D0)).Select((_Closure_0024__._0024I15_002D1 != null) ? _Closure_0024__._0024I15_002D1 : (_Closure_0024__._0024I15_002D1 = _Closure_0024__._0024I._Lambda_0024__15_002D1)).ToArray();
			foreach (int key in array)
			{
				markers.Remove(key);
			}
		}

		internal static Blip AddSpaBlip(this Vehicle vehicle)
		{
			Blip blip = V2Compat.CurrentBlip(vehicle);
			if ((object)blip != null && blip.Exists())
			{
				OwnedBlip value = null;
				if (markers.TryGetValue(blip.Handle, out value) && (object)value.Vehicle != null && value.Vehicle.Handle == vehicle.Handle && value.Matches(blip.Handle))
				{
					return value.Marker;
				}
				return null;
			}
			Blip blip2 = vehicle.AddBlip();
			try
			{
				if ((object)blip2 != null && blip2.Exists() && Register(new OwnedBlip
				{
					Marker = blip2,
					Vehicle = vehicle,
					VehicleModel = vehicle.Model.Hash,
					VehicleAddress = vehicle.MemoryAddress
				}))
				{
					return blip2;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("SPA II vehicle blip registration: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			return null;
		}
	}
}
