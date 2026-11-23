using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class MansionOrientation
	{
		private class Station
		{
			public readonly string Label;

			public readonly Vector3 Position;

			public readonly BlipSprite Sprite;

			public Blip Blip;

			public Station(string label, Vector3 position, BlipSprite sprite)
			{
				Label = label;
				Position = position;
				Sprite = sprite;
			}

			public bool HasBlip()
			{
				if ((object)Blip != null && Blip.Exists() && Blip.Sprite == Sprite)
				{
					return Function.Call<Vector3>(Hash.GET_BLIP_COORDS, Blip.Handle).DistanceToSquared(Position) < 0.01f;
				}
				return false;
			}

			public void Hide()
			{
				if (HasBlip())
				{
					V2Compat.Remove(Blip);
				}
				Blip = null;
			}

			public void Show()
			{
				if (HasBlip())
				{
					return;
				}
				Blip blip = World.CreateBlip(Position);
				if ((object)blip != null && blip.Exists())
				{
					try
					{
						Function.Call(Hash.SET_BLIP_DISPLAY, blip.Handle, 5);
						blip.Sprite = Sprite;
						blip.IsShortRange = true;
						blip.Scale = 0.65f;
						blip.Name = Label;
						SpaBlips.RegisterCoordinate(blip);
						Blip = blip;
					}
					catch (Exception projectError)
					{
						ProjectData.SetProjectError(projectError);
						V2Compat.Remove(blip);
						throw;
					}
				}
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__18_002D0
		{
			public Vector3 _0024VB_0024Local_position;

			public _Closure_0024__18_002D0(_Closure_0024__18_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_position = arg0._0024VB_0024Local_position;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(Mansion candidate)
			{
				if (candidate.Enabled && candidate.IplsLoaded && candidate.SetsApplied && _0024VB_0024Local_position.DistanceToSquared(candidate.Reference) < 22500f)
				{
					return candidate.IsOwnedByPlayer();
				}
				return false;
			}
		}

		private const int MinimapOnly = 5;

		private const int UpdateInterval = 250;

		private const float MaxFloorDifference = 2f;

		private const float MaxDistanceSquared = 2025f;

		private const string TabletLabel = "Assistant tablet";

		private static readonly List<Station> stations = new List<Station>();

		private static Mansion active;

		private static int activeInterior;

		private static int nextUpdate;

		internal static bool Enabled { get; set; } = true;


		internal static bool SameFloorNearby(Vector3 player, Vector3 point)
		{
			if (point != Vector3.Zero && PartyPlacement.FinitePoint(point) && PartyPlacement.FinitePoint(player) && Math.Abs(player.Z - point.Z) <= 2f)
			{
				return player.DistanceToSquared(point) <= 2025f;
			}
			return false;
		}

		private static void AddStation(string label, Vector3 point, int sprite)
		{
			if (!(point == Vector3.Zero) && PartyPlacement.FinitePoint(point) && Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, point.X, point.Y, point.Z + 0.65f) == activeInterior)
			{
				stations.Add(new Station(label, point, (BlipSprite)sprite));
			}
		}

		private static void LoadStations(Mansion mansion)
		{
			string[] elevatorFloorKeys = MansionFeatures.ElevatorFloorKeys;
			foreach (string key in elevatorFloorKeys)
			{
				AddStation("Elevator", Helper.ToVector3(MansionFeatures.GetStop(mansion, key)), 63);
			}
			AddStation("Office computer", MansionOffice.OfficePoint(mansion), 521);
			AddStation("Wardrobe", Helper.ToVector3(mansion.WardrobePos), 73);
			AddStation("Armoury", Helper.ToVector3(mansion.ArmoryPos), 110);
			AddStation("Vault", Helper.ToVector3(mansion.VaultPos), 500);
			AddStation("Gym", Helper.ToVector3(mansion.GymPos), 311);
			AddStation("Drinks", Helper.ToVector3(mansion.BarPos), 93);
			AddStation("Barber", MansionBarber.ChairPoint(mansion), 71);
			AddStation("Arcade", Helper.ToVector3(MansionArcade.CabinetPoint(mansion, 0)), 740);
			foreach (Quaternion assistPanel in mansion.AssistPanels)
			{
				AddStation("Assistant tablet", Helper.ToVector3(assistPanel), 521);
			}
		}

		internal static void Tick()
		{
			checked
			{
				try
				{
					_Closure_0024__18_002D0 arg = new _Closure_0024__18_002D0(null);
					if (!Enabled || Game.IsPaused || Game.IsMissionActive || (object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsDead || Helper.PP.IsInVehicle())
					{
						Cleanup();
						return;
					}
					int gameTime = Game.GameTime;
					if (gameTime < nextUpdate)
					{
						return;
					}
					arg._0024VB_0024Local_position = Helper.PP.Position;
					Mansion mansion = Mansions.MansionList.Find(arg._Lambda_0024__0);
					int num = default(int);
					if (mansion != null)
					{
						num = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, Helper.PP.Handle);
						if (num == 0 || (num != mansion.InteriorId && num != mansion.BasementId))
						{
							mansion = null;
						}
					}
					if (mansion != active || (mansion != null && num != activeInterior))
					{
						Cleanup();
						if (mansion != null)
						{
							active = mansion;
							activeInterior = num;
							LoadStations(mansion);
						}
					}
					nextUpdate = gameTime + 250;
					if (active == null)
					{
						return;
					}
					Station station = null;
					foreach (Station station2 in stations)
					{
						if (Operators.CompareString(station2.Label, "Assistant tablet", false) == 0 && SameFloorNearby(arg._0024VB_0024Local_position, station2.Position) && (station == null || arg._0024VB_0024Local_position.DistanceToSquared(station2.Position) < arg._0024VB_0024Local_position.DistanceToSquared(station.Position)))
						{
							station = station2;
						}
					}
					foreach (Station station3 in stations)
					{
						if (SameFloorNearby(arg._0024VB_0024Local_position, station3.Position) && (Operators.CompareString(station3.Label, "Assistant tablet", false) != 0 || station3 == station))
						{
							station3.Show();
						}
						else
						{
							station3.Hide();
						}
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Cleanup();
					nextUpdate = Game.GameTime + 2000;
					Logger.Log($"Mansion indoor map markers: {ex2.Message}");
					ProjectData.ClearProjectError();
				}
			}
		}

		internal static void ForgetForStoryLoad()
		{
			stations.Clear();
			active = null;
			activeInterior = 0;
			nextUpdate = 0;
		}

		internal static void Cleanup()
		{
			foreach (Station station in stations)
			{
				try
				{
					station.Hide();
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log($"Mansion indoor map marker cleanup: {ex2.Message}");
					ProjectData.ClearProjectError();
				}
			}
			ForgetForStoryLoad();
		}
	}
}
