using System;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class SouthLSHousingTravel
	{
		[CompilerGenerated]
		internal sealed class _Closure_0024__8_002D0
		{
			public Ped _0024VB_0024Local_actor;

			public IntPtr _0024VB_0024Local_address;

			public int _0024VB_0024Local_model;

			public eOwner _0024VB_0024Local_owner;

			public Vector3 _0024VB_0024Local_origin;

			public int _0024VB_0024Local_sourceInterior;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				return SourceReady(_0024VB_0024Local_actor, _0024VB_0024Local_address, _0024VB_0024Local_model, _0024VB_0024Local_owner, _0024VB_0024Local_origin, _0024VB_0024Local_sourceInterior);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__9_002D0
		{
			public Ped _0024VB_0024Local_actor;

			public IntPtr _0024VB_0024Local_address;

			public int _0024VB_0024Local_model;

			public eOwner _0024VB_0024Local_owner;

			public ApartmentClass _0024VB_0024Local_apartment;

			public int _0024VB_0024Local_interior;

			public Vector3 _0024VB_0024Local_interiorAnchor;

			public bool _0024VB_0024Local_newlyPinned;

			[SpecialName]
			internal bool _Lambda_0024__0()
			{
				if (!SameActor(_0024VB_0024Local_actor, _0024VB_0024Local_address, _0024VB_0024Local_model, _0024VB_0024Local_owner) || _0024VB_0024Local_apartment.Owner() != _0024VB_0024Local_owner)
				{
					return false;
				}
				_0024VB_0024Local_interior = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, _0024VB_0024Local_interiorAnchor.X, _0024VB_0024Local_interiorAnchor.Y, _0024VB_0024Local_interiorAnchor.Z);
				if (_0024VB_0024Local_interior == 0 || !Function.Call<bool>(Hash.IS_VALID_INTERIOR, _0024VB_0024Local_interior))
				{
					return false;
				}
				Function.Call(Hash.DISABLE_INTERIOR, _0024VB_0024Local_interior, false);
				Function.Call(Hash.SET_INTERIOR_ACTIVE, _0024VB_0024Local_interior, true);
				if (!Stability.IsPinnedInterior(_0024VB_0024Local_interior))
				{
					Stability.PinInterior(_0024VB_0024Local_interior, _0024VB_0024Local_interiorAnchor);
					_0024VB_0024Local_newlyPinned = true;
				}
				return Function.Call<bool>(Hash.IS_INTERIOR_READY, _0024VB_0024Local_interior);
			}
		}

		private static bool travelling;

		public static bool IsBusy => travelling;

		private static void Note(string message)
		{
			try
			{
				Logger.Log(message);
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
			}
		}

		public static Vector3 StandingPoint(Vector3 originalDoorPoint)
		{
			if (!PartyPlacement.FinitePoint(originalDoorPoint))
			{
				return new Vector3(float.NaN, float.NaN, float.NaN);
			}
			return originalDoorPoint + new Vector3(0f, 0f, 1f);
		}

		private static bool CanTravel(ApartmentClass apartment)
		{
			if (travelling || apartment == null || !SouthLSHousing.IsPropertyID(apartment.ID) || apartment.Owner() != Helper.GetPlayer() || (object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsDead || Helper.PP.IsInVehicle() || Helper.PP.IsFalling || Helper.PP.IsRagdoll || PartyService.IsPlayerActionActive || MansionFeatures.IsWorkoutActive || PartyPlayerActions.IsKissing || DestinationStreaming.IsPlayerSafetyHeld(Helper.PP))
			{
				return false;
			}
			Camera renderingCamera = World.RenderingCamera;
			return (object)renderingCamera == null || !renderingCamera.Exists() || Stability.IsSpaCamera(renderingCamera);
		}

		private static bool SameActor(Ped actor, IntPtr address, int model, eOwner owner)
		{
			if ((object)actor != null && actor.Exists() && actor.MemoryAddress == address && actor.Model.Hash == model && !actor.IsDead && (object)Helper.PP != null && Helper.PP.Handle == actor.Handle)
			{
				return Helper.GetPlayer() == owner;
			}
			return false;
		}

		private static bool SourceReady(Ped actor, IntPtr address, int model, eOwner owner, Vector3 origin, int sourceInterior)
		{
			if (!SameActor(actor, address, model, owner))
			{
				return false;
			}
			Function.Call(Hash.REQUEST_COLLISION_AT_COORD, origin.X, origin.Y, origin.Z);
			if (sourceInterior != 0 && (!Function.Call<bool>(Hash.IS_VALID_INTERIOR, sourceInterior) || !Function.Call<bool>(Hash.IS_INTERIOR_READY, sourceInterior)))
			{
				return false;
			}
			bool hit = default(bool);
			Vector3 point = default(Vector3);
			Vector3 normal = default(Vector3);
			return PartyPlacement.PhysicalRay(origin + new Vector3(0f, 0f, 0.75f), origin - new Vector3(0f, 0f, 1.8f), 81, ref hit, ref point, ref normal, actor) == 2 && hit && MansionFeatures.TeleportFloorAcceptable(origin.Z, point.Z, normal.Z) && DestinationStreaming.PedAtDestination(actor.Position, point) && Function.Call<bool>(Hash.HAS_COLLISION_LOADED_AROUND_ENTITY, actor.Handle);
		}

		private static void RestoreTravelFreeze(Ped actor, IntPtr address, int model, eOwner owner, Vector3 origin, int sourceInterior, bool wasFrozen, bool arrived, bool destinationPrewarmed)
		{
			_Closure_0024__8_002D0 closure_0024__8_002D = new _Closure_0024__8_002D0();
			closure_0024__8_002D._0024VB_0024Local_actor = actor;
			closure_0024__8_002D._0024VB_0024Local_address = address;
			closure_0024__8_002D._0024VB_0024Local_model = model;
			closure_0024__8_002D._0024VB_0024Local_owner = owner;
			closure_0024__8_002D._0024VB_0024Local_origin = origin;
			closure_0024__8_002D._0024VB_0024Local_sourceInterior = sourceInterior;
			if (closure_0024__8_002D._0024VB_0024Local_actor.Exists() && !(closure_0024__8_002D._0024VB_0024Local_actor.MemoryAddress != closure_0024__8_002D._0024VB_0024Local_address) && closure_0024__8_002D._0024VB_0024Local_actor.Model.Hash == closure_0024__8_002D._0024VB_0024Local_model)
			{
				if (!arrived && destinationPrewarmed && SameActor(closure_0024__8_002D._0024VB_0024Local_actor, closure_0024__8_002D._0024VB_0024Local_address, closure_0024__8_002D._0024VB_0024Local_model, closure_0024__8_002D._0024VB_0024Local_owner) && !DestinationStreaming.IsPlayerSafetyHeld(closure_0024__8_002D._0024VB_0024Local_actor))
				{
					DestinationStreaming.HoldPlayerUntilReady(closure_0024__8_002D._0024VB_0024Local_actor, wasFrozen, closure_0024__8_002D._0024VB_0024Local_origin, closure_0024__8_002D._Lambda_0024__0, Screen.IsFadedOut || Screen.IsFadingOut);
				}
				DestinationStreaming.SetSafetyHoldOriginalFreeze(closure_0024__8_002D._0024VB_0024Local_actor, wasFrozen);
				if (!DestinationStreaming.IsPlayerSafetyHeld(closure_0024__8_002D._0024VB_0024Local_actor))
				{
					Function.Call(Hash.FREEZE_ENTITY_POSITION, closure_0024__8_002D._0024VB_0024Local_actor.Handle, wasFrozen);
				}
			}
		}

		private static bool MoveToInterior(ApartmentClass apartment, Vector3 point, float heading, Vector3 interiorAnchor)
		{
			_Closure_0024__9_002D0 closure_0024__9_002D = new _Closure_0024__9_002D0();
			closure_0024__9_002D._0024VB_0024Local_apartment = apartment;
			closure_0024__9_002D._0024VB_0024Local_interiorAnchor = interiorAnchor;
			closure_0024__9_002D._0024VB_0024Local_actor = Helper.PP;
			closure_0024__9_002D._0024VB_0024Local_address = closure_0024__9_002D._0024VB_0024Local_actor.MemoryAddress;
			closure_0024__9_002D._0024VB_0024Local_model = closure_0024__9_002D._0024VB_0024Local_actor.Model.Hash;
			closure_0024__9_002D._0024VB_0024Local_owner = Helper.GetPlayer();
			Vector3 position = closure_0024__9_002D._0024VB_0024Local_actor.Position;
			bool wasFrozen = DestinationStreaming.OriginalPlayerFreeze(closure_0024__9_002D._0024VB_0024Local_actor, closure_0024__9_002D._0024VB_0024Local_actor.IsPositionFrozen);
			int sourceInterior = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, closure_0024__9_002D._0024VB_0024Local_actor.Handle);
			bool destinationPrewarmed = default(bool);
			bool flag = default(bool);
			try
			{
				Function.Call(Hash.FREEZE_ENTITY_POSITION, closure_0024__9_002D._0024VB_0024Local_actor.Handle, true);
				using (DestinationStreaming.SceneLease sceneLease = DestinationStreaming.Acquire(point))
				{
					if (sceneLease == null)
					{
						return false;
					}
					destinationPrewarmed = true;
					if (!sceneLease.WaitUntilReady(closure_0024__9_002D._Lambda_0024__0) || !sceneLease.CanContinue || !SameActor(closure_0024__9_002D._0024VB_0024Local_actor, closure_0024__9_002D._0024VB_0024Local_address, closure_0024__9_002D._0024VB_0024Local_model, closure_0024__9_002D._0024VB_0024Local_owner) || closure_0024__9_002D._0024VB_0024Local_apartment.Owner() != closure_0024__9_002D._0024VB_0024Local_owner)
					{
						return false;
					}
					flag = MansionFeatures.TeleportPlayer(point, heading) && SameActor(closure_0024__9_002D._0024VB_0024Local_actor, closure_0024__9_002D._0024VB_0024Local_address, closure_0024__9_002D._0024VB_0024Local_model, closure_0024__9_002D._0024VB_0024Local_owner);
					return flag;
				}
			}
			finally
			{
				try
				{
					if (!flag && closure_0024__9_002D._0024VB_0024Local_newlyPinned)
					{
						Stability.UnpinInterior(closure_0024__9_002D._0024VB_0024Local_interior);
					}
				}
				finally
				{
					RestoreTravelFreeze(closure_0024__9_002D._0024VB_0024Local_actor, closure_0024__9_002D._0024VB_0024Local_address, closure_0024__9_002D._0024VB_0024Local_model, closure_0024__9_002D._0024VB_0024Local_owner, position, sourceInterior, wasFrozen, flag, destinationPrewarmed);
				}
			}
		}

		private static void CloseOwnMenus(ApartmentClass apartment)
		{
			if (apartment.AptMenu != null)
			{
				apartment.AptMenu.Visible = false;
			}
			BuildingClass buildingClass = apartment.Building();
			if (buildingClass != null)
			{
				if (buildingClass.AptMenu != null)
				{
					buildingClass.AptMenu.Visible = false;
				}
				if (buildingClass.GrgMenu != null)
				{
					buildingClass.GrgMenu.Visible = false;
				}
			}
			Stability.DestroySpaCameras();
			Helper.HideHud = false;
		}

		private static void ReleaseHome(ApartmentClass apartment)
		{
			if (LowEndApartment.Apartment == apartment)
			{
				LowEndApartment.Apartment = null;
				try
				{
					LowEndApartment.Clear();
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Note("South LS source door cleanup: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
			if (MediumEndApartment.Apartment == apartment)
			{
				MediumEndApartment.Apartment = null;
				try
				{
					MediumEndApartment.Clear();
				}
				catch (Exception ex3)
				{
					ProjectData.SetProjectError(ex3);
					Exception ex4 = ex3;
					Note("South LS source door cleanup: " + ex4.Message);
					ProjectData.ClearProjectError();
				}
			}
		}

		public static bool EnterApartment(BuildingClass building, ApartmentClass apartment)
		{
			if (building == null || apartment == null || building.Apartments == null || !building.Apartments.Contains(apartment) || (apartment.ApartmentType != eApartmentType.LowEnd && apartment.ApartmentType != eApartmentType.MediumEnd) || !CanTravel(apartment))
			{
				return false;
			}
			bool flag = TwoCarGarage.Apartment == apartment && Helper.PP.Position.DistanceTo(TwoCarGarage.Interior) < 45f;
			if (!flag && Helper.PP.Position.DistanceTo(Helper.ToVector3(building.BuildingInPos)) > 5f)
			{
				return false;
			}
			travelling = true;
			bool flag2 = default(bool);
			try
			{
				CloseOwnMenus(apartment);
				if (!flag && !SouthLSHousingExterior.RememberEntryPosition(building, Helper.PP.Position))
				{
					Notification.Show("Stand nearer the front door before entering this home.");
					return false;
				}
				Vector3 point = StandingPoint(apartment.ApartmentInPos);
				if (!MoveToInterior(apartment, point, apartment.ApartmentDoorPos.W, apartment.InteriorPos()))
				{
					return false;
				}
				flag2 = true;
				if (apartment.ApartmentType == eApartmentType.LowEnd)
				{
					LowEndApartment.Apartment = apartment;
				}
				else if (apartment.ApartmentType == eApartmentType.MediumEnd)
				{
					MediumEndApartment.Apartment = apartment;
				}
				try
				{
					if (apartment.ApartmentType == eApartmentType.LowEnd)
					{
						LowEndApartment.SpawnDoor();
					}
					else
					{
						MediumEndApartment.SpawnDoor();
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Note("South LS arrival door decoration: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
				ApartmentResidence.Bind(apartment);
				Note("South LS apartment arrival: " + apartment.Name);
				return true;
			}
			catch (Exception ex3)
			{
				ProjectData.SetProjectError(ex3);
				Exception ex4 = ex3;
				Note("South LS apartment entry: " + ex4.Message);
				bool result = flag2;
				ProjectData.ClearProjectError();
				return result;
			}
			finally
			{
				travelling = false;
				Helper.HideHud = false;
			}
		}

		public static bool ExitApartment(ApartmentClass apartment)
		{
			if (!CanTravel(apartment) || !ApartmentHomeServices.InHome(apartment))
			{
				return false;
			}
			BuildingClass buildingClass = apartment.Building();
			if (buildingClass == null)
			{
				return false;
			}
			Ped pP = Helper.PP;
			IntPtr memoryAddress = pP.MemoryAddress;
			int hash = pP.Model.Hash;
			eOwner player = Helper.GetPlayer();
			Vector3 position = pP.Position;
			bool wasFrozen = DestinationStreaming.OriginalPlayerFreeze(pP, pP.IsPositionFrozen);
			int sourceInterior = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, pP.Handle);
			travelling = true;
			bool destinationPrewarmed = default(bool);
			bool flag = default(bool);
			try
			{
				CloseOwnMenus(apartment);
				int id = apartment.InteriorID();
				Function.Call(Hash.FREEZE_ENTITY_POSITION, pP.Handle, true);
				destinationPrewarmed = true;
				if (!SouthLSHousingExterior.PrepareFootExit(buildingClass))
				{
					return false;
				}
				if (!SameActor(pP, memoryAddress, hash, player) || apartment.Owner() != player)
				{
					return false;
				}
				if (!MansionFeatures.TeleportPlayer(Helper.ToVector3(buildingClass.GarageFootOutPos), buildingClass.GarageFootOutPos.W))
				{
					return false;
				}
				flag = true;
				ReleaseHome(apartment);
				ApartmentResidence.Leave();
				Stability.UnpinInterior(id);
				Note("South LS apartment exit: " + apartment.Name);
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Note("South LS apartment exit: " + ex2.Message);
				bool result = flag;
				ProjectData.ClearProjectError();
				return result;
			}
			finally
			{
				try
				{
					RestoreTravelFreeze(pP, memoryAddress, hash, player, position, sourceInterior, wasFrozen, flag, destinationPrewarmed);
				}
				finally
				{
					travelling = false;
					Helper.HideHud = false;
				}
			}
		}

		public static bool EnterGarage(ApartmentClass apartment)
		{
			if (apartment == null || !SouthLSHousing.HasGarage(apartment.ID))
			{
				return false;
			}
			if (!CanTravel(apartment) || !ApartmentHomeServices.InHome(apartment))
			{
				return false;
			}
			travelling = true;
			bool flag = default(bool);
			try
			{
				CloseOwnMenus(apartment);
				Vector3 point = StandingPoint(Helper.ToVector3(TwoCarGarage.SpawnInPos));
				int id = apartment.InteriorID();
				if (!MoveToInterior(apartment, point, TwoCarGarage.SpawnInPos.W, TwoCarGarage.Interior))
				{
					return false;
				}
				TwoCarGarage.Apartment = apartment;
				flag = true;
				ReleaseHome(apartment);
				ApartmentResidence.Leave();
				Stability.UnpinInterior(id);
				TwoCarGarage.LoadVehicles();
				Note("South LS private garage arrival: " + apartment.Name);
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Note("South LS private garage entry: " + ex2.Message);
				bool result = flag;
				ProjectData.ClearProjectError();
				return result;
			}
			finally
			{
				travelling = false;
				Helper.HideHud = false;
			}
		}
	}
}
