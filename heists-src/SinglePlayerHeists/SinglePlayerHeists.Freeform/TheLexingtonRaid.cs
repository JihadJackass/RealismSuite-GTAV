using System;
using System.Collections.Generic;
using System.Drawing;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using HKHModHelperNew;
using SinglePlayerHeists.UtilsMisc;
using SmartAIGuards;

namespace SinglePlayerHeists.Freeform
{
	internal class TheLexingtonRaid : Script
	{
		public enum ActiveGroup
		{
			None,
			LexigntonRaid_Carrier,
			LexigntonRaid_TunerGarage,
			LexigntonRaid_MerryweatherFacility,
			LexigntonRaid_MerryweatherGarage,
			LexigntonRaid_MerryweatherCargoship
		}

		public class ObjectPlacementMap
		{
			public float SpawnDist;

			public bool CannotBeDeleted { get; set; }

			public bool CannotBeSpawned { get; set; }

			public Ped GunnerPed { get; set; }

			public Prop MainProp { get; set; }

			public Entity Entity { get; set; }

			public Vehicle Veh { get; set; }

			public Prop Prop { get; set; }

			public Ped Ped { get; set; }

			public bool PlacedObj { get; set; }

			public Blip Blip { get; set; }

			public int Hash { get; set; }

			public Vector3 Position { get; set; }

			public Vector3 Rotation { get; set; }

			public uint Inputtedhash { get; set; }

			public int Health { get; set; }

			public int Armour { get; set; }

			public string Scenario { get; set; }

			public string AnimDict { get; set; }

			public string AnimName { get; set; }

			public VehicleColor Primary { get; set; }

			public VehicleColor Secondary { get; set; }

			public VehicleColor Pearlescent { get; set; }

			public int Livery { get; set; }

			public uint WeaponHash { get; set; }

			public int EntityHandle { get; set; }

			public int EntityToAttachToHandle { get; set; }

			public int EntityToAttachBone { get; set; }

			public Vector3 EntityToAttachOffset { get; set; }

			public Vector3 EntityToAttachRotation { get; set; }

			public ObjectType ObjType { get; set; }

			public VehicleHash VehicleHash { get; set; }

			public Model RequestModel(int Name)
			{
				Model result = new Model(Name);
				result.Request(50);
				if (result.IsInCdImage && result.IsValid)
				{
					while (!result.IsLoaded)
					{
						Script.Yield();
					}
					return result;
				}
				result.MarkAsNoLongerNeeded();
				return result;
			}

			public Model RequestModel(VehicleHash Name)
			{
				Model result = new Model(Name);
				result.Request(250);
				if (result.IsInCdImage && result.IsValid)
				{
					while (!result.IsLoaded)
					{
						Script.Yield();
					}
					return result;
				}
				result.MarkAsNoLongerNeeded();
				return result;
			}

			public Model RequestModel(uint Name, ObjectType Type)
			{
				Model result = new Model((int)Name);
				if (Type == ObjectType.Ped)
				{
					result = new Model((PedHash)Name);
					result.Request(50);
					if (result.IsInCdImage && result.IsValid)
					{
						while (!result.IsLoaded)
						{
							Script.Yield();
						}
						return result;
					}
					result.MarkAsNoLongerNeeded();
				}
				if (Type == ObjectType.Vehicle)
				{
					result = new Model((VehicleHash)Name);
					result.Request(50);
					if (result.IsInCdImage && result.IsValid)
					{
						while (!result.IsLoaded)
						{
							Script.Yield();
						}
						return result;
					}
					result.MarkAsNoLongerNeeded();
				}
				if (Type == ObjectType.Prop)
				{
					result = new Model((int)Name);
					result.Request(50);
					if (result.IsInCdImage && result.IsValid)
					{
						while (!result.IsLoaded)
						{
							Script.Yield();
						}
						return result;
					}
					result.MarkAsNoLongerNeeded();
				}
				return result;
			}

			public void CreateObjatCoords()
			{
				if (RequestModel(Inputtedhash, ObjectType.Ped).IsPed)
				{
					if (RequestModel(Hash).IsValid)
					{
						Entity = GlobalHelper.CreatePed(RequestModel(Hash), Position, Rotation.Z);
					}
					else
					{
						Entity = GlobalHelper.CreatePed(RequestModel(Inputtedhash, ObjectType.Ped), Position, Rotation.Z);
					}
					if (Entity != null)
					{
						Function.Call(GTA.Native.Hash.SET_ENTITY_INVINCIBLE, Entity, true);
						Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, Entity, true);
						Ped = (Ped)Entity;
						if (WeaponHash != -1)
						{
							Ped.Weapons.Give((WeaponHash)WeaponHash, 9999, true, true);
						}
						Ped.Health = Health;
						Ped.Armor = Armour;
						if (!Scenario.Equals("false"))
						{
							Function.Call(GTA.Native.Hash.TASK_START_SCENARIO_AT_POSITION, Ped, Scenario, Position.X, Position.Y, Position.Z, Rotation.Z, 0, 0, 1);
						}
						if (!AnimDict.Equals("None") && !AnimName.Equals("None"))
						{
							Ped.Task.PlayAnimation(AnimDict, AnimName, 1f, -1, AnimationFlags.Loop);
						}
					}
				}
				else if (RequestModel(Inputtedhash, ObjectType.Vehicle).IsVehicle)
				{
					if (RequestModel(Hash).IsValid)
					{
						Entity = GlobalHelper.CreateVehicle(RequestModel(Hash), Position, Rotation.Z);
					}
					else
					{
						Entity = GlobalHelper.CreateVehicle(RequestModel(Inputtedhash, ObjectType.Vehicle), Position, Rotation.Z);
					}
					if (Entity != null)
					{
						Function.Call(GTA.Native.Hash.SET_ENTITY_INVINCIBLE, Entity, true);
						Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, Entity, true);
						Vehicle vehicle = (Vehicle)Entity;
						Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, vehicle.Handle, 0);
						Function.Call(GTA.Native.Hash.SET_VEHICLE_COLOURS, vehicle, (int)Primary, (int)Secondary);
						Function.Call(GTA.Native.Hash.SET_VEHICLE_EXTRA_COLOURS, vehicle, (int)Pearlescent, 0);
						Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, vehicle, 48, Livery, true);
					}
				}
				else
				{
					if (RequestModel(Hash).IsValid)
					{
						Entity = GlobalHelper.CreateProp(RequestModel(Hash), Position, Rotation, true, false);
					}
					else
					{
						Entity = GlobalHelper.CreateProp(RequestModel(Inputtedhash, ObjectType.Prop), Position, Rotation, true, false);
					}
					try
					{
						Function.Call(GTA.Native.Hash.SET_ENTITY_INVINCIBLE, Entity, true);
						Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, Entity, true);
						Entity.Position = Position;
						Entity.Rotation = Rotation;
						Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, Entity, true);
					}
					catch
					{
					}
				}
				if (Entity != null)
				{
					Function.Call(GTA.Native.Hash.SET_ENTITY_COLLISION, Entity, true, true);
					if (!Entity.Model.IsPed && !Entity.Model.IsVehicle)
					{
						Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, Entity, true);
					}
					Entity.LodDistance = 3000;
					SpawnDist = 100f;
					PlacedObj = true;
				}
			}

			public void CreateObjatCoords_Offset(Prop Base, Vector3 OffsetP, Vector3 OffsetR)
			{
				if (RequestModel(Inputtedhash, ObjectType.Ped).IsPed)
				{
					if (RequestModel(Hash).IsValid)
					{
						Entity = GlobalHelper.CreatePed(RequestModel(Hash), Position, Rotation.Z);
					}
					else
					{
						Entity = GlobalHelper.CreatePed(RequestModel(Inputtedhash, ObjectType.Ped), Position, Rotation.Z);
					}
					if (Entity != null)
					{
						Entity.Position = Base.GetOffsetPosition(OffsetP);
						Entity.Rotation = OffsetR;
						Function.Call(GTA.Native.Hash.SET_ENTITY_INVINCIBLE, Entity, true);
						Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, Entity, true);
						Ped = (Ped)Entity;
						if (WeaponHash != -1)
						{
							Ped.Weapons.Give((WeaponHash)WeaponHash, 9999, true, true);
						}
						Ped.Health = Health;
						Ped.Armor = Armour;
						if (!Scenario.Equals("false"))
						{
							Function.Call(GTA.Native.Hash.TASK_START_SCENARIO_AT_POSITION, Ped, Scenario, Position.X, Position.Y, Position.Z, Rotation.Z, 0, 0, 1);
						}
						if (!AnimDict.Equals("None") && !AnimName.Equals("None"))
						{
							Ped.Task.PlayAnimation(AnimDict, AnimName, 1f, -1, AnimationFlags.Loop);
						}
					}
				}
				else if (RequestModel(Inputtedhash, ObjectType.Vehicle).IsVehicle)
				{
					if (RequestModel(Hash).IsValid)
					{
						Entity = GlobalHelper.CreateVehicle(RequestModel(Hash), Position, Rotation.Z);
					}
					else
					{
						Entity = GlobalHelper.CreateVehicle(RequestModel(Inputtedhash, ObjectType.Vehicle), Position, Rotation.Z);
					}
					if (Entity != null)
					{
						Entity.Position = Base.GetOffsetPosition(OffsetP);
						Entity.Rotation = OffsetR;
						Function.Call(GTA.Native.Hash.SET_ENTITY_INVINCIBLE, Entity, true);
						Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, Entity, true);
						Vehicle vehicle = (Vehicle)Entity;
						Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, vehicle.Handle, 0);
						Function.Call(GTA.Native.Hash.SET_VEHICLE_COLOURS, vehicle, (int)Primary, (int)Secondary);
						Function.Call(GTA.Native.Hash.SET_VEHICLE_EXTRA_COLOURS, vehicle, (int)Pearlescent, 0);
						Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, vehicle, 48, Livery, true);
					}
				}
				else
				{
					if (RequestModel(Hash).IsValid)
					{
						Entity = GlobalHelper.CreateProp(RequestModel(Hash), Position, Rotation, true, false);
					}
					else
					{
						Entity = GlobalHelper.CreateProp(RequestModel(Inputtedhash, ObjectType.Prop), Position, Rotation, true, false);
					}
					try
					{
						Entity.Position = Base.GetOffsetPosition(OffsetP);
						Entity.Rotation = OffsetR;
						Function.Call(GTA.Native.Hash.SET_ENTITY_INVINCIBLE, Entity, true);
						Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, Entity, true);
						Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, Entity, true);
					}
					catch
					{
					}
				}
				if (Entity != null)
				{
					Function.Call(GTA.Native.Hash.SET_ENTITY_COLLISION, Entity, true, true);
					if (!Entity.Model.IsPed && !Entity.Model.IsVehicle)
					{
						Function.Call(GTA.Native.Hash.FREEZE_ENTITY_POSITION, Entity, true);
					}
					Entity.LodDistance = 3000;
					SpawnDist = 100f;
					PlacedObj = true;
				}
			}

			public ObjectPlacementMap(int M, int AttH, uint H, Vector3 P, Vector3 R, int PC, int SC, int pearl, int L, int HTAT, int Bone, Vector3 Offset, Vector3 Rot)
			{
				EntityHandle = AttH;
				EntityToAttachToHandle = HTAT;
				EntityToAttachBone = Bone;
				EntityToAttachOffset = Offset;
				EntityToAttachRotation = Rot;
				Rotation = Rot;
				Inputtedhash = H;
				Primary = (VehicleColor)PC;
				Secondary = (VehicleColor)SC;
				Pearlescent = (VehicleColor)pearl;
				Livery = L;
				Hash = M;
				Position = P;
				Rotation = R;
			}

			public ObjectPlacementMap(int M, int AttH, uint H, Vector3 P, Vector3 R, uint W, int HP, int Am, string Scen, string AnimD, string AnimN, int HTAT, int Bone, Vector3 Offset, Vector3 Rot)
			{
				EntityHandle = AttH;
				EntityToAttachToHandle = HTAT;
				EntityToAttachBone = Bone;
				EntityToAttachOffset = Offset;
				EntityToAttachRotation = Rot;
				Scenario = Scen;
				AnimDict = AnimD;
				AnimName = AnimN;
				Armour = Am;
				Health = HP;
				Inputtedhash = H;
				WeaponHash = W;
				Hash = M;
				Position = P;
				Rotation = R;
			}

			public ObjectPlacementMap(int M, int AttH, uint H, Vector3 P, Vector3 R, int HTAT, int Bone, Vector3 Offset, Vector3 Rot)
			{
				EntityHandle = AttH;
				EntityToAttachToHandle = HTAT;
				EntityToAttachBone = Bone;
				EntityToAttachOffset = Offset;
				EntityToAttachRotation = Rot;
				Rotation = Rot;
				Hash = M;
				Position = P;
				Rotation = R;
			}

			public ObjectPlacementMap()
			{
			}

			public ObjectPlacementMap(int M, Vector3 P, Vector3 R)
			{
				Hash = M;
				Position = P;
				Rotation = R;
			}

			public ObjectPlacementMap(int M, Vector3 P, float R)
			{
				Hash = M;
				Position = P;
				Rotation = new Vector3(0f, 0f, R);
			}

			public ObjectPlacementMap(Prop O, int M, Vector3 P, float R)
			{
				MainProp = O;
				Hash = M;
				Position = P;
				Rotation = new Vector3(0f, 0f, R);
			}

			public ObjectPlacementMap(Prop O, int M, Vector3 P, float R, ObjectType Ob)
			{
				ObjType = Ob;
				MainProp = O;
				Hash = M;
				Position = P;
				Rotation = new Vector3(0f, 0f, R);
				SpawnDist = 3500f;
			}

			public ObjectPlacementMap(Prop O, int M, Vector3 P, Vector3 R, ObjectType Ob)
			{
				ObjType = Ob;
				MainProp = O;
				Hash = M;
				Position = P;
				Rotation = R;
				SpawnDist = 3500f;
			}

			public ObjectPlacementMap(int M, Vector3 P, Vector3 R, ObjectType Ob)
			{
				ObjType = Ob;
				Hash = M;
				Position = P;
				Rotation = R;
				SpawnDist = 3500f;
			}

			public ObjectPlacementMap(Prop O, VehicleHash M, Vector3 P, float R, ObjectType Ob)
			{
				ObjType = Ob;
				MainProp = O;
				VehicleHash = M;
				Position = P;
				Rotation = new Vector3(0f, 0f, R);
				SpawnDist = 3500f;
			}

			public ObjectPlacementMap(Prop O, VehicleHash M, Vector3 P, Vector3 R, ObjectType Ob)
			{
				ObjType = Ob;
				MainProp = O;
				VehicleHash = M;
				Position = P;
				Rotation = R;
				SpawnDist = 3500f;
			}
		}

		public enum ObjectType
		{
			Ped,
			Prop,
			Vehicle
		}

		public class MissionOjects
		{
			public List<ObjectPlacementMap> Entities { get; set; }

			public void ATTACH_ENTITY_TO_ENTITY(Entity entity1, Entity entity2, int boneIndex, float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, bool p9, bool useSoftPinning, bool collision, bool isPed, int vertexIndex, bool fixedRot)
			{
				Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, entity1, entity2, boneIndex, xPos, yPos, zPos, xRot, yRot, zRot, p9, useSoftPinning, collision, isPed, vertexIndex, fixedRot);
			}

			public void DeleteEntites()
			{
				if (Entities.Count <= 0)
				{
					return;
				}
				foreach (ObjectPlacementMap entity in Entities)
				{
					if (entity.Entity != null)
					{
						entity.Entity.Delete();
					}
				}
			}

			public void CreateEntites()
			{
				DeleteEntites();
				foreach (ObjectPlacementMap entity in Entities)
				{
					entity.CreateObjatCoords();
				}
				foreach (ObjectPlacementMap entity2 in Entities)
				{
					if (entity2.Entity != null)
					{
						entity2.Entity.Position = entity2.Position;
						entity2.Entity.Rotation = entity2.Rotation;
					}
				}
				foreach (ObjectPlacementMap entity3 in Entities)
				{
					if (!(entity3.Entity != null) || entity3.EntityToAttachToHandle == -1)
					{
						continue;
					}
					foreach (ObjectPlacementMap entity4 in Entities)
					{
						if (entity4.Entity != null && entity3.EntityToAttachToHandle == entity4.EntityHandle)
						{
							if (entity3.EntityToAttachBone == -1)
							{
								entity3.EntityToAttachBone = 0;
							}
							entity3.Entity.Position = entity3.Position;
							entity3.Entity.Rotation = entity3.Rotation;
							ATTACH_ENTITY_TO_ENTITY(entity3.Entity, entity4.Entity, entity3.EntityToAttachBone, entity3.EntityToAttachOffset.X, entity3.EntityToAttachOffset.Y, entity3.EntityToAttachOffset.Z, entity3.EntityToAttachRotation.X, entity3.EntityToAttachRotation.Y, entity3.EntityToAttachRotation.Z, false, false, true, false, 2, true);
							if (entity3.Entity.Model.IsVehicle)
							{
								Vehicle vehicle = (Vehicle)entity3.Entity;
								Function.Call(Hash.SET_VEHICLE_FIXED, vehicle);
								Function.Call(Hash.SET_VEHICLE_DEFORMATION_FIXED, vehicle);
								Function.Call(Hash.SET_VEHICLE_ENGINE_HEALTH, vehicle, 1000f);
								Function.Call(Hash.SET_VEHICLE_BODY_HEALTH, vehicle, 1000f);
							}
							Function.Call(Hash.SET_ENTITY_NO_COLLISION_ENTITY, entity3.Entity, entity4.Entity, false);
							Function.Call(Hash.SET_ENTITY_NO_COLLISION_ENTITY, entity4.Entity, entity3.Entity, false);
							break;
						}
					}
				}
				foreach (ObjectPlacementMap entity5 in Entities)
				{
					if (entity5.Entity != null)
					{
						if (entity5.Entity.Model.IsVehicle || entity5.Entity.Model.IsPed)
						{
							Function.Call(Hash.FREEZE_ENTITY_POSITION, entity5.Entity, false);
						}
						Function.Call(Hash.SET_ENTITY_INVINCIBLE, entity5.Entity, false);
						if (entity5.Entity.Model.IsVehicle)
						{
							Vehicle vehicle2 = (Vehicle)entity5.Entity;
							Function.Call(Hash.SET_VEHICLE_FIXED, vehicle2);
							Function.Call(Hash.SET_VEHICLE_DEFORMATION_FIXED, vehicle2);
							Function.Call(Hash.SET_VEHICLE_ENGINE_HEALTH, vehicle2, 1000f);
							Function.Call(Hash.SET_VEHICLE_BODY_HEALTH, vehicle2, 1000f);
						}
					}
				}
			}

			public MissionOjects()
			{
			}

			public MissionOjects(List<ObjectPlacementMap> ListOfEntities)
			{
				Entities = ListOfEntities;
			}
		}

		public class SamSite
		{
			public bool IsDead { get; set; }

			public Prop Sam { get; set; }

			public int roundsFired { get; set; }

			public int fireDelay { get; set; }

			public SamSite()
			{
			}

			public SamSite(Prop S)
			{
				roundsFired = 0;
				Sam = S;
				fireDelay = 0;
			}
		}

		public enum HeistStage
		{
			Scout,
			Keycard,
			Thermite,
			InRaiju,
			InPTBoat,
			InValkyrie,
			C4,
			Grinder,
			SamJammer,
			CargoshipTrojanVehicle,
			ShootPtBoatCable,
			LootingCarrier,
			CannotOpenContainer
		}

		public class Container
		{
			public int PTFX1 { get; set; }

			public int PTFX2 { get; set; }

			public Prop Bomb { get; set; }

			public int plantBombStage { get; set; }

			public int plantThermalChargeStage { get; set; }

			public Prop Bag { get; set; }

			public int containerswitch { get; set; }

			public Prop Container1 { get; set; }

			public Prop Swipecard { get; set; }

			public int LargeContainerAnim { get; set; }

			public int LargeContainerSyncScene { get; set; }

			public int unstucktimer { get; set; }

			public int BombExplodeTimer { get; set; }

			public Prop Lock { get; set; }

			public Prop Saw { get; set; }

			public Prop FakeContainer { get; set; }

			public int ParticleFx { get; set; }

			public int SyncScene { get; set; }

			public int SyncScene2 { get; set; }

			public int ParticleTimer { get; set; }

			public Container()
			{
			}

			public Container(Prop C)
			{
				if (Lock != null)
				{
					Lock.Delete();
				}
				Container1 = C;
			}
		}

		public static ActiveGroup CurrentActiveGroup = ActiveGroup.None;

		public static bool GotGrinder;

		public static bool GotC4 = false;

		public static int C4ChargesCollected = 20;

		public static bool GotThermite = true;

		public static int ThermiteChargesCollected = 20;

		public static bool GotKeycard;

		public static bool GotSamJammer;

		public static int HeistMinPay = 750000;

		public static int HeistMaxPay = 3500000;

		public static int CurrentPay = 0;

		public static int LocalTake = 0;

		public static bool HackingKeypad = false;

		public Prop LastKeypad;

		public Prop USB;

		public Prop Phone;

		public int Scene1;

		public static List<SmartGuards.SmartAiV2> CarrierGuards = new List<SmartGuards.SmartAiV2>();

		public static List<SmartGuards.SmartAiV2> CargoshipGuards = new List<SmartGuards.SmartAiV2>();

		public static List<SmartGuards.SmartAiV2> MerryweatherGarageGuards = new List<SmartGuards.SmartAiV2>();

		public static List<SmartGuards.SmartAiV2> MerryweatherFacilityGuards = new List<SmartGuards.SmartAiV2>();

		public static List<SmartGuards.SmartAiV2> TunerGarageGuards = new List<SmartGuards.SmartAiV2>();

		public static MissionOjects AircraftCarrierObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "RM-10 Bombushka"), 437536, 4262088844u, new Vector3(3074.95483f, -4771.765f, 14.0977936f), new Vector3(-0.0512587763f, 0.000578278f, 173.341614f), 154, 25, 3, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_01a"), 80474, 2202902697u, new Vector3(3074.41235f, -4778.02246f, 5.077253f), new Vector3(0f, -0f, 102.235321f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 704059, 3595011691u, new Vector3(3072.7085f, -4775.049f, 5.07869864f), new Vector3(0f, -0f, 102.491043f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 139296, 3595011691u, new Vector3(3082.09619f, -4742.98438f, 5.07869864f), new Vector3(0f, 0f, -71.93395f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_01a"), 446012, 2202902697u, new Vector3(3053.31665f, -4706.779f, 5.077292f), new Vector3(0f, -0f, 105.20269f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 456737, 3595011691u, new Vector3(3048.1272f, -4700.8623f, 5.07873774f), new Vector3(0f, -0f, -165.077118f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_01a"), 273184, 2202902697u, new Vector3(3080.38135f, -4654.36f, 5.07718849f), new Vector3(0f, -0f, 104.945633f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 603171, 3595011691u, new Vector3(3071.21143f, -4698.987f, 5.07873774f), new Vector3(0f, -0f, -164.821045f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 315168, 3595011691u, new Vector3(3079.15137f, -4728.94336f, 5.07873774f), new Vector3(0f, 0f, -74.17849f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 484132, 3595011691u, new Vector3(3080.412f, -4733.385f, 5.07873774f), new Vector3(0f, 0f, -74.17849f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_01a"), 443419, 2202902697u, new Vector3(3059.22827f, -4788.179f, 5.077038f), new Vector3(0f, 0f, -74.38208f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 117538, 3595011691u, new Vector3(3061.49023f, -4790.667f, 5.078484f), new Vector3(0f, 0f, -74.27948f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 581158, 3595011691u, new Vector3(3063.44238f, -4744.64648f, 5.07869864f), new Vector3(0f, -0f, 105.212013f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 91430, 3595011691u, new Vector3(3067.97949f, -4681.57227f, 5.07873726f), new Vector3(0f, 0f, 15.1397467f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_01a"), 309801, 2202902697u, new Vector3(3054.17261f, -4630.88867f, 5.077309f), new Vector3(0f, 0f, -75.1956558f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 73589, 3595011691u, new Vector3(3056.27783f, -4633.60254f, 5.07875538f), new Vector3(0f, 0f, -74.99048f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 174379, 3595011691u, new Vector3(3078.1333f, -4651.69434f, 5.078634f), new Vector3(0f, -0f, 106.1386f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_01a"), 259882, 2202902697u, new Vector3(3081.80127f, -4635.819f, 5.077188f), new Vector3(0f, 0f, 16.3154945f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_01a"), 215083, 2202902697u, new Vector3(3068.49072f, -4658.37451f, 5.07730961f), new Vector3(0f, 0f, -76.1613f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 127276, 3595011691u, new Vector3(3062.45654f, -4674.12646f, 5.078755f), new Vector3(0f, -0f, -163.474289f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 504877, 3595011691u, new Vector3(3101.80322f, -4717.269f, 14.2637634f), new Vector3(0f, -0f, 105.023491f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 639276, 3595011691u, new Vector3(3100.69263f, -4713.12354f, 14.2637634f), new Vector3(0f, -0f, 105.023491f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_01a"), 235310, 2202902697u, new Vector3(3094.246f, -4694.00342f, 14.26232f), new Vector3(0f, -0f, -164.230148f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 701744, 3595011691u, new Vector3(3068.48145f, -4595.2915f, 14.2628736f), new Vector3(0f, 0f, 85.21597f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 687692, 3595011691u, new Vector3(3056.38452f, -4779.073f, 14.2627554f), new Vector3(0f, -0f, 109.269379f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 627253, 3595011691u, new Vector3(3078.82715f, -4689.201f, 14.2628727f), new Vector3(0f, 0f, -74.4111938f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 707379, 3595011691u, new Vector3(3079.62476f, -4692.062f, 14.2628727f), new Vector3(0f, 0f, -74.4111938f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 725554, 3595011691u, new Vector3(3081.751f, -4699.69043f, 14.2637644f), new Vector3(0f, 0f, -74.4111938f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 369200, 3595011691u, new Vector3(3080.95361f, -4696.82959f, 14.2637644f), new Vector3(0f, 0f, -74.4111938f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_03mb"), 678969, 3293497890u, new Vector3(3094.16479f, -4755.925f, 5.078728f), new Vector3(0f, 0f, -74.20056f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_03mb"), 79418, 3293497890u, new Vector3(3094.95752f, -4758.786f, 5.078728f), new Vector3(0f, 0f, -74.2005539f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 325945, 3595011691u, new Vector3(3097.14551f, -4761.0415f, 5.07869864f), new Vector3(0f, 0f, -73.99539f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_03mb"), 445244, 3293497890u, new Vector3(3065.068f, -4804.311f, 5.078513f), new Vector3(0f, 0f, -76.09551f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_03mb"), 182587, 3293497890u, new Vector3(3064.354f, -4801.428f, 5.07851267f), new Vector3(0f, 0f, -76.09551f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_03mb"), 92280, 3293497890u, new Vector3(3049.003f, -4774.153f, 14.262785f), new Vector3(0f, 0f, -60.4324455f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_03mb"), 252220, 3293497890u, new Vector3(3050.63135f, -4777.024f, 14.2627831f), new Vector3(0f, 0f, -60.4324455f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_03mb"), 266558, 3293497890u, new Vector3(3078.762f, -4636.50439f, 5.0786643f), new Vector3(0f, 0f, 16.13642f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_03mb"), 711232, 3293497890u, new Vector3(3075.897f, -4637.30859f, 5.0786643f), new Vector3(0f, 0f, 16.1876011f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_03mb"), 601665, 3293497890u, new Vector3(3073.04443f, -4638.13428f, 5.078664f), new Vector3(0f, 0f, 16.1876011f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_03mb"), 614977, 3293497890u, new Vector3(3045.74316f, -4619.80762f, 5.078785f), new Vector3(0f, 0f, -75.0181656f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_03mb"), 452416, 3293497890u, new Vector3(3031.805f, -4637.328f, 5.07878447f), new Vector3(0f, -0f, 104.268021f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_03mb"), 778050, 3293497890u, new Vector3(3074.17969f, -4795.69971f, 5.07872868f), new Vector3(0f, 0f, 71.76828f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_03mb"), 562463, 3293497890u, new Vector3(3067.78613f, -14625f / MathF.PI, 5.07878447f), new Vector3(0f, 0f, -75.8388748f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 624223, 3595011691u, new Vector3(3082.77f, -4704.061f, 26.2633171f), new Vector3(0f, -0f, 103.786293f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 285536, 3595011691u, new Vector3(3084.02783f, -4709.18652f, 26.2633171f), new Vector3(0f, -0f, 103.786293f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_staff_desk_02"), 741426, 2084153992u, new Vector3(3072.18481f, -4772.984f, 5.07725334f), new Vector3(0f, -0f, -167.851273f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_staff_desk_01"), 762419, 1845693979u, new Vector3(3068.04565f, -4752.706f, 5.077253f), new Vector3(0f, -0f, 100.326782f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_staff_desk_01"), 110124, 1845693979u, new Vector3(3078.5625f, -4726.909f, 5.077292f), new Vector3(0f, -0f, -164.943375f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_staff_desk_01"), 769075, 1845693979u, new Vector3(3079.78833f, -4723.88525f, 5.07729244f), new Vector3(0f, 0f, -76.29731f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_staff_desk_02"), 440118, 2084153992u, new Vector3(3055.10278f, -4708.876f, 5.07729244f), new Vector3(0f, 0f, 14.23325f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_staff_desk_02"), 496438, 2084153992u, new Vector3(3052.22388f, -4709.60547f, 5.07729244f), new Vector3(0f, 0f, 14.23325f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_staff_desk_02"), 411943, 2084153992u, new Vector3(3051.08643f, -4698.845f, 5.07729149f), new Vector3(0f, -0f, -164.072647f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_staff_desk_02"), 42041, 2084153992u, new Vector3(3069.215f, -4699.533f, 5.077292f), new Vector3(0f, 0f, -75.4241257f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_staff_desk_02"), 594233, 2084153992u, new Vector3(3090.22437f, -4757.53857f, 5.07725239f), new Vector3(0f, 0f, -74.9551239f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_staff_desk_02"), 784954, 2084153992u, new Vector3(3097.766f, -4763.043f, 5.077253f), new Vector3(0f, 0f, 15.9418983f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_staff_desk_02"), 329790, 2084153992u, new Vector3(3071.01123f, -4754.78125f, 5.07725334f), new Vector3(0f, -0f, -169.48381f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_staff_desk_02"), 17470, 2084153992u, new Vector3(3076.79956f, -4722.054f, 5.077292f), new Vector3(0f, 0f, 13.7361383f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_staff_desk_02"), 109121, 2084153992u, new Vector3(3041.5144f, -4669.90674f, 5.07730961f), new Vector3(0f, -0f, 104.783257f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_staff_desk_02"), 488002, 2084153992u, new Vector3(3043.21655f, -4667.26563f, 5.07730865f), new Vector3(0f, 0f, 13.4243832f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_staff_desk_02"), 307525, 2084153992u, new Vector3(3061.777f, -4667.069f, 5.077309f), new Vector3(0f, 0f, -75.16866f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_offchair_exec_03"), 569158, 1580642483u, new Vector3(3050.788f, -4697.6333f, 5.67948675f), new Vector3(0f, 0f, 33.1727371f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_offchair_exec_03"), 444959, 1580642483u, new Vector3(3052.38013f, -4710.746f, 5.67948675f), new Vector3(0f, -0f, -168.019592f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_offchair_exec_03"), 361798, 1580642483u, new Vector3(3055.406f, -4709.8f, 5.679487f), new Vector3(0f, -0f, -138.589783f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_offchair_exec_03"), 613957, 1580642483u, new Vector3(3077.97314f, -4726.091f, 5.67948675f), new Vector3(0f, 0f, 57.4263573f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_offchair_exec_03"), 70212, 1580642483u, new Vector3(3078.92456f, -4724.2915f, 5.67948675f), new Vector3(0f, -0f, 101.54261f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_offchair_exec_03"), 460869, 1580642483u, new Vector3(3076.76782f, -4723.069f, 5.67948675f), new Vector3(0f, -0f, 173.347275f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_offchair_exec_03"), 633161, 1580642483u, new Vector3(3068.254f, -4699.99658f, 5.67948675f), new Vector3(0f, -0f, 133.16f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_offchair_exec_03"), 638024, 1580642483u, new Vector3(3069.05835f, -4752.379f, 5.67944765f), new Vector3(0f, 0f, -36.1969833f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_offchair_exec_03"), 42315, 1580642483u, new Vector3(3070.918f, -4753.77344f, 5.67944765f), new Vector3(0f, 0f, 52.6016045f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_offchair_exec_03"), 189248, 1580642483u, new Vector3(3071.98218f, -4771.99756f, 5.67944765f), new Vector3(0f, 0f, 15.6468239f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_offchair_exec_03"), 28746, 1580642483u, new Vector3(3089.20386f, -4757.95f, 5.67944765f), new Vector3(0f, -0f, 132.9529f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_offchair_exec_03"), 436556, 1580642483u, new Vector3(3043.45972f, -4668.17773f, 5.679504f), new Vector3(0f, -0f, -125.153267f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_offchair_exec_03"), 556623, 1580642483u, new Vector3(3042.549f, -4669.655f, 5.679504f), new Vector3(0f, 0f, -50.3773842f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_offchair_exec_03"), 677966, 1580642483u, new Vector3(3060.67383f, -4667.23f, 5.679504f), new Vector3(0f, -0f, 110.01638f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects AircraftCarrierLoot = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_ser_storage_01a"), 526612, 1413173041u, new Vector3(3073.365f, -4772.371f, 5.87925339f), new Vector3(0f, -0f, -165.371063f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_ser_storage_01a"), 43798, 1413173041u, new Vector3(3068.74219f, -4753.943f, 5.87925243f), new Vector3(0f, -0f, 113.863533f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_ser_storage_01a"), 36118, 1413173041u, new Vector3(3072.026f, -4754.225f, 5.87925339f), new Vector3(0f, -0f, -158.9237f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_ser_storage_01a"), 366873, 1413173041u, new Vector3(3052.04541f, -4698.23047f, 5.87929153f), new Vector3(0f, -0f, -163.119614f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_ser_storage_01a"), 12566, 1413173041u, new Vector3(3069.09863f, -4700.718f, 5.87929153f), new Vector3(0f, 0f, -74.9337158f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_ser_storage_01a"), 353817, 1413173041u, new Vector3(3051.3645f, -4710.21533f, 5.87929249f), new Vector3(0f, 0f, 13.96722f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_ser_storage_01a"), 295449, 1413173041u, new Vector3(3054.37231f, -4709.474f, 5.87929249f), new Vector3(0f, 0f, 17.7033157f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_ser_storage_01a"), 397593, 1413173041u, new Vector3(3078.96655f, -4726.27539f, 5.87929153f), new Vector3(0f, -0f, -150.161514f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_ser_storage_01a"), 326425, 1413173041u, new Vector3(3078.96777f, -4722.68555f, 5.87929249f), new Vector3(0f, 0f, -65.40504f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_ser_storage_01a"), 84266, 1413173041u, new Vector3(3075.76636f, -4722.71729f, 5.879292f), new Vector3(0f, 0f, 13.2603168f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_ser_storage_01a"), 359961, 1413173041u, new Vector3(3096.79517f, -4763.64063f, 5.879253f), new Vector3(0f, 0f, 16.43304f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_ser_storage_01a"), 481836, 1413173041u, new Vector3(3090.18579f, -4758.78662f, 5.87925243f), new Vector3(0f, 0f, -73.645134f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_ser_storage_01a"), 695851, 1413173041u, new Vector3(3042.14282f, -4670.944f, 5.87930965f), new Vector3(0f, -0f, 106.409081f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_ser_storage_01a"), 136492, 1413173041u, new Vector3(3044.47119f, -4667.344f, 5.8793087f), new Vector3(0f, 0f, 11.3656626f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_ser_storage_01a"), 540715, 1413173041u, new Vector3(3061.13721f, -4666.01074f, 5.8793087f), new Vector3(0f, 0f, -74.51467f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_adv_case_01a"), 655364, 3614165362u, new Vector3(3066.759f, -4773.60449f, 5.37164259f), new Vector3(0f, -0f, -75.63476f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_adv_case_01a"), 300299, 3614165362u, new Vector3(3070.43213f, -4701.84229f, 5.37168074f), new Vector3(0f, 0f, -254.870621f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_adv_case_01a"), 333835, 3614165362u, new Vector3(3074.18848f, -4694.42871f, 5.371681f), new Vector3(0f, -0f, -345.8177f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_adv_case_01a"), 411261, 3614165362u, new Vector3(3058.069f, -4635.38428f, 5.37169838f), new Vector3(0f, 0f, -165.9675f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_adv_case_01a"), 293892, 3614165362u, new Vector3(3037.9126f, -4660.69971f, 5.371698f), new Vector3(0f, -0f, -75.74399f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_adv_case_01a"), 717618, 3614165362u, new Vector3(3096.783f, -4710.57959f, 14.5567083f), new Vector3(0f, 0f, -249.8645f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_adv_case_01a"), 407819, 3614165362u, new Vector3(3101.58f, -4719.8335f, 14.5567093f), new Vector3(0f, -0f, -74.05751f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_adv_case_01a"), 445801, 3614165362u, new Vector3(3071.0332f, -4599.85449f, 14.5558167f), new Vector3(0f, 0f, -90.44546f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_adv_case_01a"), 92169, 3614165362u, new Vector3(3066.61719f, -4755.57227f, 5.37164259f), new Vector3(2.03555488E-13f, -1.00179113E-05f, -342.705383f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_adv_case_01a"), 83980, 3614165362u, new Vector3(3064.59082f, -4640.08252f, 5.37169838f), new Vector3(0f, 0f, -163.265121f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_adv_case_01a"), 124693, 3614165362u, new Vector3(3064.32983f, -4638.674f, 10.8839874f), new Vector3(0f, 0f, -253.138367f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_adv_case_01a"), 269166, 3614165362u, new Vector3(3050.73633f, -4713.08447f, 5.371681f), new Vector3(0f, 0f, 16.2235928f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_adv_case_01a"), 43285, 3614165362u, new Vector3(3062.21753f, -4742.84033f, 5.371681f), new Vector3(0f, -0f, -72.41989f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_adv_case_01a"), 548887, 3614165362u, new Vector3(3087.78467f, -4745.15234f, 5.371642f), new Vector3(2.03555488E-13f, -1.00179095E-05f, -164.3841f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_adv_case_01a"), 763929, 3614165362u, new Vector3(3081.95654f, -4730.639f, 5.371681f), new Vector3(0f, 0f, -256.20105f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_adv_case_01a"), 286746, 3614165362u, new Vector3(3079.33887f, -4721.26367f, 5.371681f), new Vector3(0f, -0f, -345.253967f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_01a"), 451848, 2082893870u, new Vector3(3081.088f, -4746.30566f, 5.07725239f), new Vector3(0f, 0f, -70.5746f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_01a"), 406074, 2082893870u, new Vector3(3057.51147f, -4715.82227f, 5.077291f), new Vector3(0f, 0f, -69.03232f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_01a"), 532853, 2082893870u, new Vector3(3084.03784f, -4739.547f, 5.077252f), new Vector3(0f, 0f, -74.91548f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_01a"), 155492, 2082893870u, new Vector3(3062.98364f, -4664.46826f, 5.077308f), new Vector3(0f, -0f, -165.503784f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_01a"), 137582, 2082893870u, new Vector3(3037.48047f, -4659.813f, 5.07730865f), new Vector3(0f, -0f, 111.88797f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_01a"), 3887, 2082893870u, new Vector3(3075.84424f, -4710.52637f, 5.07729149f), new Vector3(0f, 0f, 14.4494438f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_01a"), 274542, 2082893870u, new Vector3(3074.28418f, -4800.303f, 5.07725239f), new Vector3(0f, 0f, -18.75733f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_01a"), 484219, 2082893870u, new Vector3(3094.61157f, -4714.732f, 14.2626228f), new Vector3(0f, 0f, -80.25563f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_01a"), 301316, 2082893870u, new Vector3(3097.45654f, -4699.59473f, 14.26232f), new Vector3(0f, -0f, 104.200111f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_01a"), 23041, 2082893870u, new Vector3(3080.05151f, -4688.665f, 14.2612534f), new Vector3(0f, 0f, -74.8744f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_01a"), 54644, 2082893870u, new Vector3(3113.82983f, -4775.23975f, 14.2616158f), new Vector3(0f, 0f, -37.6629639f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_01a"), 589926, 2082893870u, new Vector3(3126.04419f, -4798.72559f, 14.2616158f), new Vector3(0f, -0f, 105.798141f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_01a"), 69750, 2082893870u, new Vector3(3071.88428f, -4824.34668f, 14.2616158f), new Vector3(0f, 0f, -81.7812347f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_01a"), 598841, 2082893870u, new Vector3(3046.252f, -4773.5293f, 14.26131f), new Vector3(0f, -0f, -149.951447f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_01a"), 516213, 2082893870u, new Vector3(3026.87744f, -4713.976f, 13.261549f), new Vector3(0f, 0f, -74.355896f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_01a"), 599396, 2082893870u, new Vector3(3064.794f, -4593.412f, 14.2614269f), new Vector3(0f, -0f, 172.072937f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static List<MissionOjects> AircraftCarrierSpawnLoot = new List<MissionOjects>();

		public static List<MissionOjects> AircraftCarrierNavs = new List<MissionOjects>
		{
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 437762, 1803116220u, new Vector3(3093.3147f, -4678.07f, 14.5918179f), new Vector3(0f, -0f, 110.417686f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 438018, 1803116220u, new Vector3(3073.10474f, -4684.08154f, 14.5909262f), new Vector3(0f, -0f, -165.338058f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 438274, 1803116220u, new Vector3(3086.00146f, -4729.438f, 14.5918169f), new Vector3(0f, 0f, -77.45942f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 438530, 1803116220u, new Vector3(3102.24438f, -4725.51025f, 14.5918169f), new Vector3(0f, 0f, -68.65597f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 438786, 1803116220u, new Vector3(3085.80957f, -4729.542f, 14.7916231f), new Vector3(0f, 0f, -53.9667168f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 439042, 1803116220u, new Vector3(3072.84961f, -4684.24072f, 14.5909262f), new Vector3(0f, 0f, -75.92305f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 439298, 1803116220u, new Vector3(3093.14746f, -4678.21143f, 14.80106f), new Vector3(0f, 0f, -74.33602f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier"), 439554, 3019107892u, new Vector3(3094.7124f, -4677.61426f, 15.261878f), new Vector3(0f, -0f, 106.327713f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 439810, 1803116220u, new Vector3(3073.202f, -4684.33252f, 14.5909281f), new Vector3(0f, 0f, -72.78805f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 440066, 1803116220u, new Vector3(3085.84521f, -4729.132f, 14.5918188f), new Vector3(0f, 0f, -74.42574f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier 2"), 440322, 2047212121u, new Vector3(3086.17969f, -4730.47266f, 15.2586689f), new Vector3(0f, 0f, 15.8108664f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 440578, 1803116220u, new Vector3(3046.05664f, -4779.98047f, 13.5910921f), new Vector3(0f, 0f, 2.60543537f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 440834, 1803116220u, new Vector3(3042.18555f, -4775.75537f, 13.5910921f), new Vector3(0f, 0f, 32.6998367f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 441090, 1803116220u, new Vector3(3032.78516f, -4736.884f, 13.5910749f), new Vector3(-1.01777744E-13f, 1.04024025E-06f, 39.1994934f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 441346, 1803116220u, new Vector3(3033.45337f, -4733.43457f, 13.5908937f), new Vector3(0f, 0f, -19.55611f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 441602, 1803116220u, new Vector3(3028.69116f, -4720.693f, 13.5910473f), new Vector3(0f, 0f, 18.6761532f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 441858, 1803116220u, new Vector3(3024.61133f, -4706.427f, 13.5910759f), new Vector3(0f, -0f, -93.35892f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 442114, 1803116220u, new Vector3(3025.93555f, -4702.22754f, 13.5908f), new Vector3(0f, -0f, -96.12246f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 442370, 1803116220u, new Vector3(3020.157f, -4684.96436f, 13.5909281f), new Vector3(0f, 0f, -81.02379f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 442626, 1803116220u, new Vector3(3017.01025f, -4683.144f, 13.5910921f), new Vector3(0f, 0f, -81.17706f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 442882, 1803116220u, new Vector3(3014.93359f, -4671.5957f, 13.591094f), new Vector3(0f, 0f, -82.6100159f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 443138, 1803116220u, new Vector3(3016.99341f, -4682.7627f, 13.5910921f), new Vector3(0f, 0f, -78.00347f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 443394, 1803116220u, new Vector3(3020.18579f, -4685.262f, 13.5909357f), new Vector3(0f, 0f, -81.1253738f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 443650, 1803116220u, new Vector3(3025.70142f, -4702.393f, 13.5908318f), new Vector3(0f, 0f, -68.22758f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 443906, 1803116220u, new Vector3(3028.66f, -4720.381f, 13.5910406f), new Vector3(0f, 0f, -72.5261841f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 444162, 1803116220u, new Vector3(3033.237f, -4733.59961f, 13.5909243f), new Vector3(0f, 0f, -70.2741f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 444418, 1803116220u, new Vector3(3032.51831f, -4740.098f, 13.5910921f), new Vector3(0f, -0f, -120.737823f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 444674, 1803116220u, new Vector3(3042.14673f, -4775.94238f, 13.5910921f), new Vector3(0f, 0f, -73.49804f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 444930, 1803116220u, new Vector3(3045.892f, -4779.86f, 13.7344379f), new Vector3(0f, -0f, -91.25764f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier 2"), 445186, 2047212121u, new Vector3(3047.12964f, -4784.15625f, 14.258153f), new Vector3(0f, 0f, 15.4584227f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 445442, 1803116220u, new Vector3(3056.19775f, -4794.92f, 5.401683f), new Vector3(0f, 0f, -75.696846f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 445698, 1803116220u, new Vector3(3068.99878f, -4791.90771f, 5.40675068f), new Vector3(0f, 0f, -77.33436f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 445954, 1803116220u, new Vector3(3078.45532f, -4789.662f, 5.40694427f), new Vector3(0f, 0f, -77.33421f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 446210, 1803116220u, new Vector3(3068.752f, -4792.024f, 5.40675163f), new Vector3(0f, 0f, -67.35355f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 446466, 1803116220u, new Vector3(3055.893f, -4794.927f, 5.401681f), new Vector3(0f, 0f, -67.40448f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 446722, 1803116220u, new Vector3(3054.21753f, -4788.1543f, 5.401681f), new Vector3(0f, 0f, -72.7783356f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier"), 446978, 3019107892u, new Vector3(3054.18774f, -4787.23438f, 6.068671f), new Vector3(0f, -0f, -164.591965f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 447234, 1803116220u, new Vector3(3080.22681f, -4751.74658f, 5.40675259f), new Vector3(0f, 0f, 15.7660322f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 447490, 1803116220u, new Vector3(3074.794f, -4729.361f, 5.40679f), new Vector3(0f, 0f, 15.817174f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 447746, 1803116220u, new Vector3(3080.38818f, -4751.971f, 5.40675163f), new Vector3(0f, 0f, -60.9027252f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier"), 448002, 3019107892u, new Vector3(3080.45337f, -4752.82861f, 6.080569f), new Vector3(0f, 0f, 15.5635605f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 448258, 1803116220u, new Vector3(3083.29639f, -4767.20264f, 5.40675163f), new Vector3(0f, -0f, 106.5186f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 448514, 1803116220u, new Vector3(3076.3335f, -4769.55371f, 5.40675163f), new Vector3(0f, -0f, -162.890625f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 448770, 1803116220u, new Vector3(3080.05078f, -4783.94434f, 5.40681553f), new Vector3(0f, -0f, 106.26236f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 449026, 1803116220u, new Vector3(3068.46362f, -4787.002f, 5.40675163f), new Vector3(0f, -0f, 105.238548f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 449282, 1803116220u, new Vector3(3069.85034f, -4792.095f, 5.40675068f), new Vector3(0f, -0f, 105.238548f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 449538, 1803116220u, new Vector3(3068.5498f, -4787.31934f, 5.40675163f), new Vector3(0f, -0f, 105.238548f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 449794, 1803116220u, new Vector3(3080.29785f, -4783.929f, 5.40681648f), new Vector3(0f, -0f, 101.246132f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 450050, 1803116220u, new Vector3(3076.63281f, -4769.56445f, 5.40675163f), new Vector3(0f, -0f, 96.5883942f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 450306, 1803116220u, new Vector3(3096.87085f, -4763.638f, 5.40675163f), new Vector3(0f, 0f, 16.02908f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier"), 450562, 3019107892u, new Vector3(3098.0415f, -4763.164f, 6.074175f), new Vector3(0f, -0f, 101.913513f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 450818, 1803116220u, new Vector3(3060.76831f, -4643.9917f, 5.406807f), new Vector3(0f, 0f, -75.73801f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 451074, 1803116220u, new Vector3(3063.76f, -4653.665f, 5.406808f), new Vector3(0f, 0f, -73.8953857f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 451330, 1803116220u, new Vector3(3071.344f, -4651.4126f, 5.41661644f), new Vector3(0f, -0f, -162.8468f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 451586, 1803116220u, new Vector3(3083.00024f, -4647.928f, 5.406687f), new Vector3(0f, -0f, -163.409637f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 451842, 1803116220u, new Vector3(3085.00757f, -4654.88867f, 5.406687f), new Vector3(0f, -0f, -163.716324f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 452098, 1803116220u, new Vector3(3082.772f, -4648.219f, 5.406687f), new Vector3(0f, -0f, -166.940338f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 452354, 1803116220u, new Vector3(3071.516f, -4650.82471f, 5.41661644f), new Vector3(0f, -0f, -166.940338f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 452610, 1803116220u, new Vector3(3063.42773f, -4653.28955f, 5.406808f), new Vector3(0f, -0f, -165.404709f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 452866, 1803116220u, new Vector3(3060.60986f, -4643.79736f, 5.406808f), new Vector3(0f, -0f, -90.78213f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 453122, 1803116220u, new Vector3(3058.42456f, -4636.373f, 5.406808f), new Vector3(0f, -0f, -106.44323f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier"), 453378, 3019107892u, new Vector3(3057.97314f, -4635.84375f, 6.075256f), new Vector3(0f, -0f, -151.838684f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 453634, 1803116220u, new Vector3(3037.64526f, -4672.397f, 10.0716677f), new Vector3(0f, -0f, 105.030128f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 453890, 1803116220u, new Vector3(3035.16f, -4663.14941f, 10.0716677f), new Vector3(0f, -0f, 105.030128f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 454146, 1803116220u, new Vector3(3032.67163f, -4652.53369f, 5.406808f), new Vector3(0f, -0f, 104.8761f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 454402, 1803116220u, new Vector3(3035.167f, -4662.90674f, 10.0716677f), new Vector3(0f, -0f, 106.256035f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 454658, 1803116220u, new Vector3(3037.63037f, -4672.12646f, 10.0716677f), new Vector3(0f, -0f, 105.385857f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 454914, 1803116220u, new Vector3(3039.72974f, -4679.7793f, 10.0716677f), new Vector3(0f, -0f, 105.692917f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier"), 455170, 3019107892u, new Vector3(3040.30151f, -4681.18945f, 10.7306709f), new Vector3(0f, 0f, 19.4028244f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 314883, 1803116220u, new Vector3(3013.37329f, -4651.52148f, 14.5909271f), new Vector3(0f, -0f, -142.208908f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 493059, 1803116220u, new Vector3(3033.86816f, -4695.68555f, 14.59081f), new Vector3(0f, -0f, -151.113617f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 596227, 1803116220u, new Vector3(3054.72388f, -4739.956f, 14.59081f), new Vector3(0f, -0f, -153.672272f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 724227, 1803116220u, new Vector3(3075.19849f, -4784.20947f, 14.5908089f), new Vector3(0f, -0f, -139.0847f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 22532, 1803116220u, new Vector3(3096.03931f, -4828.60352f, 14.591115f), new Vector3(0f, -0f, -152.851532f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 230660, 1803116220u, new Vector3(3085.58887f, -4806.541f, 14.591116f), new Vector3(0f, 0f, -65.89432f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 367108, 1803116220u, new Vector3(3064.8833f, -4762.19727f, 14.5908079f), new Vector3(0f, 0f, -68.2993851f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 575492, 1803116220u, new Vector3(3044.4585f, -4717.81543f, 14.59081f), new Vector3(0f, 0f, -63.5393829f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 713732, 1803116220u, new Vector3(3023.77832f, -4673.758f, 14.5908079f), new Vector3(0f, 0f, -62.92511f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 114693, 1803116220u, new Vector3(3002.86938f, -4629.175f, 14.5909262f), new Vector3(0f, 0f, -63.9485321f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 302341, 1803116220u, new Vector3(2992.83423f, -4607.48145f, 14.5909271f), new Vector3(0f, 0f, -63.2831f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier 2"), 785669, 2047212121u, new Vector3(2992.65625f, -4606.588f, 15.2579546f), new Vector3(0f, -0f, -151.26088f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 427013, 1803116220u, new Vector3(3105.71948f, -4797.06738f, 14.591115f), new Vector3(0f, 0f, -65.37981f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 697094, 1803116220u, new Vector3(3084.80762f, -4752.70459f, 14.5909262f), new Vector3(0f, 0f, -54.4778671f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 23815, 1803116220u, new Vector3(3064.32837f, -4708.36035f, 14.5909243f), new Vector3(0f, 0f, -62.2058563f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 207623, 1803116220u, new Vector3(3043.72925f, -4664.40625f, 14.5909262f), new Vector3(0f, 0f, -61.28446f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 480516, 1803116220u, new Vector3(3023.04956f, -4619.961f, 14.5909262f), new Vector3(0f, 0f, -62.7172127f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 369159, 1803116220u, new Vector3(3012.713f, -4597.75928f, 14.5909281f), new Vector3(0f, 0f, -62.6148071f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 601863, 1803116220u, new Vector3(3033.24536f, -4642.214f, 14.5909262f), new Vector3(0f, 0f, -64.61068f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 13320, 1803116220u, new Vector3(3054.04248f, -4686.40625f, 14.5909262f), new Vector3(0f, 0f, -64.61067f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 118280, 1803116220u, new Vector3(3074.51733f, -4730.552f, 14.5909262f), new Vector3(0f, 0f, -64.1500244f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 326152, 1803116220u, new Vector3(3095.29443f, -4774.76758f, 14.5909262f), new Vector3(0f, 0f, -64.20119f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 517640, 1803116220u, new Vector3(3115.899f, -4818.87842f, 14.5911169f), new Vector3(0f, 0f, -61.02769f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier 2"), 751112, 2047212121u, new Vector3(3116.31641f, -4819.643f, 15.2568417f), new Vector3(0f, 0f, 19.5854969f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 657161, 1803116220u, new Vector3(3074.45557f, -4814.2207f, 14.591115f), new Vector3(0f, 0f, 15.8462706f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 712457, 1803116220u, new Vector3(3069.44531f, -4796.921f, 14.5911131f), new Vector3(0f, 0f, 16.0509529f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 767753, 1803116220u, new Vector3(3060.45117f, -4768.249f, 14.59081f), new Vector3(0f, 0f, 16.7674637f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 9994, 1803116220u, new Vector3(3064.33716f, -4766.45654f, 14.5908079f), new Vector3(0f, 0f, 12.5705843f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 154634, 1803116220u, new Vector3(3073.79468f, -4785.87842f, 14.59081f), new Vector3(0f, 0f, 4.842213f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 37898, 1803116220u, new Vector3(3083.91284f, -4807.27051f, 14.5908079f), new Vector3(0f, 0f, -24.02387f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 317450, 1803116220u, new Vector3(3078.72144f, -4810.596f, 14.591115f), new Vector3(0f, 0f, -8.260052f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier 2"), 677642, 2047212121u, new Vector3(3075.18945f, -4819.97949f, 15.2571859f), new Vector3(0f, 0f, 11.44728f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 602125, 1803116220u, new Vector3(3046.24219f, -4703.29443f, 10.0715742f), new Vector3(0f, -0f, 105.00708f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 774157, 1803116220u, new Vector3(3051.758f, -4723.685f, 10.0715742f), new Vector3(0f, -0f, 105.006622f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 98574, 1803116220u, new Vector3(3056.714f, -4742.17871f, 10.0715742f), new Vector3(0f, -0f, 105.006622f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 433422, 1803116220u, new Vector3(3054.32153f, -4733.25049f, 10.0715742f), new Vector3(0f, -0f, 105.006622f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 383758, 1803116220u, new Vector3(3049.02368f, -4713.482f, 10.0715742f), new Vector3(0f, -0f, 105.006622f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 523022, 1803116220u, new Vector3(3043.55518f, -4693.076f, 10.0715742f), new Vector3(0f, -0f, 105.006622f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 615694, 1803116220u, new Vector3(3041.1626f, -4684.14844f, 10.0715742f), new Vector3(0f, -0f, 105.006622f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier"), 158223, 3019107892u, new Vector3(3040.56079f, -4683.8916f, 10.73728f), new Vector3(0f, -0f, -162.198944f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 473616, 1803116220u, new Vector3(3071.21655f, -4685.97363f, 10.0715742f), new Vector3(0f, 0f, -75.49884f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 664848, 1803116220u, new Vector3(3076.593f, -4706.534f, 10.0715742f), new Vector3(0f, 0f, -75.2428f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 785424, 1803116220u, new Vector3(3081.85938f, -4726.019f, 10.0715742f), new Vector3(0f, 0f, -76.31757f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 97041, 1803116220u, new Vector3(3084.14819f, -4733.93f, 10.0715742f), new Vector3(0f, 0f, -75.70332f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 351761, 1803116220u, new Vector3(3079.3645f, -4716.032f, 10.0715742f), new Vector3(0f, 0f, -74.73083f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 512017, 1803116220u, new Vector3(3073.88916f, -4695.97559f, 10.0715742f), new Vector3(0f, 0f, -74.73083f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 652305, 1803116220u, new Vector3(3068.67456f, -4676.875f, 10.0715742f), new Vector3(0f, 0f, -74.73083f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier 2"), 361490, 2047212121u, new Vector3(3084.74927f, -4734.499f, 10.7380285f), new Vector3(0f, 0f, 20.6729736f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 414483, 1803116220u, new Vector3(3040.07861f, -4688.093f, 5.40679073f), new Vector3(0f, 0f, 10.5876579f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 617231, 1803116220u, new Vector3(3037.63013f, -4685.096f, 5.40679073f), new Vector3(0f, 0f, 25.68603f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 688915, 1803116220u, new Vector3(3036.10034f, -4680.08154f, 5.40679073f), new Vector3(0f, 0f, -74.6797f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 729107, 1803116220u, new Vector3(3039.02051f, -4676.933f, 5.40679073f), new Vector3(0f, 0f, -53.49055f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 31508, 1803116220u, new Vector3(3043.10571f, -4672.191f, 5.406807f), new Vector3(0f, 0f, -39.4667931f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 242196, 1803116220u, new Vector3(3040.681f, -4663.417f, 5.406808f), new Vector3(0f, 0f, 13.4031115f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 299796, 1803116220u, new Vector3(3038.2312f, -4654.33057f, 5.406808f), new Vector3(0f, 0f, 14.2731628f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 381204, 1803116220u, new Vector3(3040.75562f, -4663.72f, 5.406807f), new Vector3(0f, -0f, 101.075829f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 527124, 1803116220u, new Vector3(3042.74438f, -4672.134f, 5.406808f), new Vector3(0f, -0f, 102.457596f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 589332, 1803116220u, new Vector3(3039.33618f, -4676.49463f, 5.40679f), new Vector3(0f, -0f, 115.252647f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 696084, 1803116220u, new Vector3(3036.3186f, -4679.99072f, 5.40679073f), new Vector3(0f, -0f, 155.838425f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 34837, 1803116220u, new Vector3(3037.19653f, -4684.80273f, 5.40679073f), new Vector3(0f, -0f, -151.5985f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 132117, 1803116220u, new Vector3(3039.72974f, -4687.89f, 5.40679073f), new Vector3(0f, -0f, -112.239861f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier"), 568341, 3019107892u, new Vector3(3040.594f, -4688.45654f, 6.07354927f), new Vector3(0f, 0f, 45.28999f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 384023, 1803116220u, new Vector3(3049.55127f, -4697.11572f, 5.40679073f), new Vector3(0f, -0f, -115.3658f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 336663, 1803116220u, new Vector3(3055.53149f, -4698.792f, 5.40679073f), new Vector3(0f, -0f, -109.223572f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 269335, 1803116220u, new Vector3(3060.24951f, -4699.05225f, 5.40679073f), new Vector3(0f, -0f, -104.0029f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 499991, 1803116220u, new Vector3(3062.09741f, -4694.493f, 5.40679073f), new Vector3(0f, 0f, -86.0892944f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 590103, 1803116220u, new Vector3(3069.882f, -4693.62451f, 5.40679073f), new Vector3(0f, 0f, -85.27029f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 637975, 1803116220u, new Vector3(3061.92261f, -4694.35156f, 5.40679073f), new Vector3(0f, 0f, -85.52608f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 691223, 1803116220u, new Vector3(3060.0896f, -4698.83f, 5.40679073f), new Vector3(0f, -0f, -103.285645f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 736535, 1803116220u, new Vector3(3055.24487f, -4698.3457f, 5.40679073f), new Vector3(0f, -0f, -107.226379f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 431637, 1803116220u, new Vector3(3049.54028f, -4696.86475f, 5.40679f), new Vector3(0f, -0f, -111.729942f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 670742, 1803116220u, new Vector3(3046.903f, -4697.559f, 5.40679073f), new Vector3(0f, -0f, -163.064346f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier 3"), 415000, 1349953339u, new Vector3(3046.24316f, -4697.62061f, 6.073043f), new Vector3(-0f, -0f, -78.45871f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 569113, 1803116220u, new Vector3(3095.1626f, -4781.401f, 5.40695f), new Vector3(0f, -0f, -164.135315f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 598297, 1803116220u, new Vector3(3093.02124f, -4773.45361f, 5.40675163f), new Vector3(0f, -0f, -162.190216f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 654105, 1803116220u, new Vector3(3101.09229f, -4771.03955f, 5.40675068f), new Vector3(0f, -0f, -94.68197f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 712217, 1803116220u, new Vector3(3093.35938f, -4773.42871f, 5.40675259f), new Vector3(0f, -0f, -156.712936f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 761625, 1803116220u, new Vector3(3094.989f, -4781.423f, 5.406948f), new Vector3(0f, -0f, -164.901337f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 58138, 1803116220u, new Vector3(3097.77979f, -4789.925f, 5.40775967f), new Vector3(0f, -0f, -163.621277f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier 2"), 485145, 2047212121u, new Vector3(3098.12f, -4790.926f, 6.07428074f), new Vector3(0f, 0f, 11.8311424f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 256553, 1803116220u, new Vector3(3052.80884f, -4588.36426f, 14.5909262f), new Vector3(0f, 0f, 84.15226f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 355625, 1803116220u, new Vector3(3062.56738f, -4607.174f, 14.5909262f), new Vector3(0f, -0f, 114.502251f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 59691, 1803116220u, new Vector3(3049.75537f, -4611.55225f, 14.5909262f), new Vector3(0f, 0f, 17.3601227f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 33066, 1803116220u, new Vector3(3060.29028f, -4638.27f, 14.5909243f), new Vector3(0f, 0f, -72.87196f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 516137, 1803116220u, new Vector3(3077.08179f, -4634.83057f, 14.5909262f), new Vector3(0f, 0f, -78.85983f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 350250, 1803116220u, new Vector3(3081.00439f, -4630.56f, 14.5909262f), new Vector3(0f, 0f, -66.21799f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 559658, 1803116220u, new Vector3(3080.567f, -4625.946f, 14.5909262f), new Vector3(0f, 0f, -51.8359146f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 257578, 1803116220u, new Vector3(3080.918f, -4630.65869f, 14.5909262f), new Vector3(0f, 0f, -67.1898956f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 683305, 1803116220u, new Vector3(3076.93262f, -4634.6f, 14.5909262f), new Vector3(0f, 0f, -74.25263f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 94507, 1803116220u, new Vector3(3060.62085f, -4638.33545f, 14.5909262f), new Vector3(0f, 0f, -17.85095f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 538666, 1803116220u, new Vector3(3049.81641f, -4611.792f, 14.5909262f), new Vector3(0f, 0f, -70.82298f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 161549, 1803116220u, new Vector3(3062.87915f, -4607.2915f, 14.5909262f), new Vector3(0f, 0f, -68.6730042f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 221228, 1803116220u, new Vector3(3052.78613f, -4588.515f, 14.5909262f), new Vector3(0f, 0f, -68.41701f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier"), 37163, 3019107892u, new Vector3(3052.25415f, -4587.59f, 15.2577505f), new Vector3(0f, -0f, -164.788467f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 406317, 1803116220u, new Vector3(3094.52881f, -4678.376f, 14.5918179f), new Vector3(0f, -0f, 105.823479f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 672810, 1803116220u, new Vector3(3098.67261f, -4692.97461f, 14.5918188f), new Vector3(0f, -0f, 105.823479f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 21536, 1803116220u, new Vector3(3103.82715f, -4712.48047f, 14.5918188f), new Vector3(0f, -0f, 106.284019f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 446993, 1803116220u, new Vector3(3106.78784f, -4722.62256f, 14.5918179f), new Vector3(0f, -0f, 106.284019f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 569645, 1803116220u, new Vector3(3112.445f, -4743.88037f, 14.5921211f), new Vector3(0f, -0f, 105.209076f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 33325, 1803116220u, new Vector3(3108.46118f, -4748.26367f, 14.5921221f), new Vector3(0f, 0f, 75.42132f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 132132, 1803116220u, new Vector3(3100.98315f, -4750.474f, 14.5921221f), new Vector3(0f, 0f, 21.2203541f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 583724, 1803116220u, new Vector3(3094.03882f, -4752.888f, 14.5918188f), new Vector3(0f, 0f, 24.9052563f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 457746, 1803116220u, new Vector3(3086.64673f, -4730.423f, 14.5918179f), new Vector3(0f, 0f, -66.2472458f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 516398, 1803116220u, new Vector3(3072.253f, -4684.054f, 14.5909262f), new Vector3(0f, 0f, -77.3531952f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier 2"), 399148, 2047212121u, new Vector3(3094.39038f, -4677.85547f, 15.2584009f), new Vector3(0f, -0f, -164.2544f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 744751, 1803116220u, new Vector3(3019.155f, -4671.82959f, 14.5908089f), new Vector3(0f, -0f, 105.414856f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 76336, 1803116220u, new Vector3(3023.80029f, -4688.702f, 14.5908079f), new Vector3(0f, -0f, 105.414856f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 689454, 1803116220u, new Vector3(3027.92139f, -4704.42627f, 14.5908079f), new Vector3(0f, -0f, 103.060326f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 439317, 1803116220u, new Vector3(3032.437f, -4721.15137f, 14.5908089f), new Vector3(0f, -0f, 104.749138f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 507441, 1803116220u, new Vector3(3035.46777f, -4731.51855f, 14.59081f), new Vector3(0f, 0f, 89.13869f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 640303, 1803116220u, new Vector3(3033.92188f, -4738.91f, 14.59081f), new Vector3(0f, -0f, 106.795517f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 775470, 1803116220u, new Vector3(3037.88574f, -4753.42334f, 14.59081f), new Vector3(0f, -0f, 108.27948f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 246066, 1803116220u, new Vector3(3033.94287f, -4738.72266f, 14.5908089f), new Vector3(0f, -0f, 100.141342f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 35633, 1803116220u, new Vector3(3035.543f, -4731.89f, 14.5908079f), new Vector3(0f, 0f, 81.15282f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 432174, 1803116220u, new Vector3(3032.49316f, -4720.955f, 14.59081f), new Vector3(0f, -0f, 111.246635f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 551985, 1803116220u, new Vector3(3028.06519f, -4704.453f, 14.5908089f), new Vector3(0f, -0f, 101.521744f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 588337, 1803116220u, new Vector3(3023.87427f, -4688.416f, 14.59081f), new Vector3(0f, -0f, 104.131561f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 25394, 1803116220u, new Vector3(3019.33862f, -4671.52637f, 14.59081f), new Vector3(0f, -0f, 107.765213f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 361511, 1803116220u, new Vector3(3016.66333f, -4662.387f, 14.5908089f), new Vector3(0f, -0f, 123.528229f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier"), 96051, 3019107892u, new Vector3(3016.234f, -4661.13428f, 15.25714f), new Vector3(0f, -0f, -166.146164f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 43573, 1803116220u, new Vector3(3102.20166f, -4804.87549f, 6.35882568f), new Vector3(0f, 0f, 17.7363415f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 415285, 1803116220u, new Vector3(3106.32642f, -4818.644f, 6.35977554f), new Vector3(0f, 0f, 53.7676239f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 597556, 1803116220u, new Vector3(3082.93213f, -4825.119f, 6.35668945f), new Vector3(0f, -0f, 103.668732f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 219417, 1803116220u, new Vector3(3079.79541f, -4811.994f, 6.35668945f), new Vector3(0f, -0f, 108.530327f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier"), 89143, 3019107892u, new Vector3(3101.92236f, -4804.55273f, 7.024403f), new Vector3(-4.26947651E-07f, 8.007682E-08f, -154.169556f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 390458, 1803116220u, new Vector3(3064.28882f, -4702.027f, 5.40679073f), new Vector3(0f, 0f, -81.23705f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 637495, 1803116220u, new Vector3(3069.42f, -4722.604f, 5.40679073f), new Vector3(0f, 0f, -76.98885f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 434733, 1803116220u, new Vector3(3076.842f, -4754.733f, 5.40675163f), new Vector3(0f, 0f, -76.98885f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 761402, 1803116220u, new Vector3(3082.081f, -4778.47754f, 5.40675163f), new Vector3(0f, 0f, -75.70914f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 624440, 1803116220u, new Vector3(3076.674f, -4754.599f, 5.58656025f), new Vector3(0f, 0f, -79.1892242f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 103227, 1803116220u, new Vector3(3069.50513f, -4722.88428f, 5.40679073f), new Vector3(0f, 0f, -69.51573f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier 3"), 462903, 1349953339u, new Vector3(3064.16187f, -4701.374f, 6.08196735f), new Vector3(0f, -0f, -161.537216f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			})
		};

		public static MissionOjects CargoshipObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Kurtz 31 Patrol Boat"), 70260, 4018222598u, new Vector3(-472.454346f, -2649.24365f, 9.270111f), new Vector3(0.6368112f, 0.175310969f, 142.84201f), 152, 46, 52, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "F-160 Raiju"), 13666, 239897677u, new Vector3(-432.072876f, -2634.20459f, 11.913805f), new Vector3(-0.688008f, -0.002919182f, -145.052f), 121, 121, 8, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_bison_winch"), 132381, 1641541792u, new Vector3(-473.217834f, -2649.1272f, 26.2753754f), new Vector3(2.134434E-07f, 179.000977f, -137.369461f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_cs_15m_rope_s"), 564258, 4146966289u, new Vector3(-473.358978f, -2649.06055f, 11.2396564f), new Vector3(0f, -0f, -142.851166f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Valkyrie"), 497511, 2694714877u, new Vector3(-403.2874f, -2607.26563f, 16.11949f), new Vector3(0.286559731f, 0.118209891f, 97.9540939f), 14, 12, 18, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_helipad_01"), 716128, 3789865418u, new Vector3(-432.014648f, -2634.034f, 8.024234f), new Vector3(0f, -0f, -135.219971f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_01a"), 432499, 2202902697u, new Vector3(-423.527435f, -2626.43481f, 9.829632f), new Vector3(0f, -0f, -136.193939f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_01d"), 243313, 2140719283u, new Vector3(-402.319519f, -2604.53931f, 9.831107f), new Vector3(0f, -0f, -134.66481f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_01e"), 20993, 2437639192u, new Vector3(-404.8711f, -2607.12085f, 9.831107f), new Vector3(0f, -0f, -134.66481f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_01f"), 149780, 1525186387u, new Vector3(-407.423065f, -2609.70142f, 9.831108f), new Vector3(0f, -0f, -134.66481f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_01d"), 62847, 2140719283u, new Vector3(-412.293945f, -2614.62817f, 9.831106f), new Vector3(0f, -0f, -134.66481f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_01a"), 496507, 3665231470u, new Vector3(-414.845978f, -2617.209f, 9.831106f), new Vector3(0f, -0f, -134.66481f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_01a"), 45137, 3665231470u, new Vector3(-409.7427f, -2612.04761f, 9.831106f), new Vector3(0f, -0f, -134.66481f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_01c"), 261896, 772023703u, new Vector3(-478.695648f, -2680.85571f, 9.831106f), new Vector3(0f, 0f, 45.7525024f), 91413, 0, new Vector3(-3.399999f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_01f"), 91413, 1525186387u, new Vector3(-476.323273f, -2678.42017f, 9.831106f), new Vector3(0f, 0f, 45.7525024f), 192526, 0, new Vector3(-3.399999f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_01h"), 192526, 511018606u, new Vector3(-473.9509f, -2675.98462f, 9.831106f), new Vector3(0f, 0f, 45.7525024f), 661363, 0, new Vector3(-3.49999881f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_02a"), 661363, 1670285818u, new Vector3(-471.508728f, -2673.47754f, 9.831106f), new Vector3(0f, 0f, 45.7525024f), 407676, 0, new Vector3(-3.69999862f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_01g"), 407676, 3914341412u, new Vector3(-468.927032f, -2670.82715f, 9.831106f), new Vector3(0f, 0f, 45.7525024f), 267870, 0, new Vector3(-3.49999881f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_01c"), 267870, 772023703u, new Vector3(-466.484863f, -2668.32f, 9.831106f), new Vector3(0f, 0f, 45.7525024f), 318291, 0, new Vector3(-12.4000111f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_01c"), 318291, 772023703u, new Vector3(-457.832642f, -2659.4375f, 9.831106f), new Vector3(0f, 0f, 45.7525024f), 292367, 0, new Vector3(-3.39999866f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_01b"), 292367, 466911544u, new Vector3(-455.460266f, -2657.002f, 9.831106f), new Vector3(0f, 0f, 45.7525024f), 179226, 0, new Vector3(-3.399999f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_01a"), 179226, 3665231470u, new Vector3(-453.0879f, -2654.56641f, 9.831106f), new Vector3(0f, 0f, 45.7525024f), 461850, 0, new Vector3(-3.59999871f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_01e"), 461850, 2437639192u, new Vector3(-450.575958f, -2651.98755f, 9.831106f), new Vector3(0f, 0f, 45.7525024f), 45931, 0, new Vector3(-3.399999f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_01h"), 45931, 511018606u, new Vector3(-448.203583f, -2649.552f, 9.831106f), new Vector3(0f, 0f, 45.7525024f), 557858, 0, new Vector3(-3.79999852f, -5.96046448E-08f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_01mb"), 557858, 1600026313u, new Vector3(-445.5521f, -2646.82983f, 9.831106f), new Vector3(0f, 0f, 45.7525024f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_01h"), 445981, 511018606u, new Vector3(-498.0488f, -2700.51587f, 13.4952946f), new Vector3(0f, -0f, -133.5762f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_helipad_01"), 469561, 3789865418u, new Vector3(-404.3868f, -2607.31177f, 12.8478727f), new Vector3(0f, 0f, 43.23846f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static List<MissionOjects> CargoshipNavs = new List<MissionOjects>
		{
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 600888, 1803116220u, new Vector3(-493.738861f, -2711.09448f, 8.087783f), new Vector3(0f, 0f, -51.4987755f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 782368, 1803116220u, new Vector3(-481.4771f, -2699.61328f, 8.089618f), new Vector3(0f, 0f, -48.2230949f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 678457, 1803116220u, new Vector3(-465.172424f, -2684.23438f, 8.090636f), new Vector3(0f, 0f, -46.7898331f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 131706, 1803116220u, new Vector3(-444.680267f, -2663.18115f, 8.090636f), new Vector3(0f, 0f, -37.06531f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 476197, 1803116220u, new Vector3(-419.36264f, -2636.411f, 8.090636f), new Vector3(0f, 0f, -46.2777f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 744751, 1803116220u, new Vector3(-397.870636f, -2611.81445f, 8.135916f), new Vector3(0f, 0f, -34.25f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 139587, 1803116220u, new Vector3(-419.940674f, -2636.94385f, 8.090636f), new Vector3(0f, 0f, 43.0333862f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 651064, 1803116220u, new Vector3(-445.3033f, -2663.95679f, 8.090638f), new Vector3(0f, 0f, 45.541172f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 164972, 1803116220u, new Vector3(-465.972382f, -2685.021f, 8.090636f), new Vector3(0f, 0f, 45.541172f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 784446, 1803116220u, new Vector3(-500.972717f, -2716.48145f, 8.08795452f), new Vector3(0f, 0f, 40.7812424f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier 2"), 522277, 2047212121u, new Vector3(-501.3389f, -2717.34326f, 8.762135f), new Vector3(-0f, -0f, -35.2219467f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 232544, 1803116220u, new Vector3(-442.48526f, -2642.026f, 8.090636f), new Vector3(0f, -0f, -133.59201f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 313903, 1803116220u, new Vector3(-432.924561f, -2651.6167f, 8.090636f), new Vector3(0f, -0f, -134.257339f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 458511, 1803116220u, new Vector3(-442.732025f, -2641.771f, 8.090636f), new Vector3(0f, -0f, -133.540634f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 11317, 1803116220u, new Vector3(-450.218048f, -2634.50928f, 8.090637f), new Vector3(0f, -0f, -132.516815f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier 2"), 549158, 2047212121u, new Vector3(-450.637f, -2634.03076f, 8.757365f), new Vector3(0f, -0f, -132.668167f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 142718, 1803116220u, new Vector3(-421.176666f, -2608.293f, 8.089759f), new Vector3(0f, -0f, -144.799118f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 625220, 1803116220u, new Vector3(-449.664978f, -2636.27319f, 8.090638f), new Vector3(0f, -0f, -128.727783f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 521540, 1803116220u, new Vector3(-488.345917f, -2673.53247f, 8.090636f), new Vector3(0f, -0f, -136.50705f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 232228, 1803116220u, new Vector3(-508.718628f, -2696.50562f, 8.087717f), new Vector3(0f, -0f, -129.904526f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 164152, 1803116220u, new Vector3(-513.50354f, -2702.00879f, 8.08783f), new Vector3(0f, -0f, -130.211548f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 589879, 1803116220u, new Vector3(-487.6515f, -2673.723f, 8.090636f), new Vector3(0f, -0f, -135.7389f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 157300, 1803116220u, new Vector3(-449.979828f, -2637.01074f, 8.090635f), new Vector3(0f, -0f, -135.7389f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 671260, 1803116220u, new Vector3(-421.189178f, -2608.41187f, 8.6441f), new Vector3(0f, -0f, -135.7389f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 730945, 1803116220u, new Vector3(-407.412262f, -2597.86646f, 8.14026f), new Vector3(0f, -0f, -136.506561f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier"), 560249, 3019107892u, new Vector3(-406.901672f, -2597.16772f, 8.80429f), new Vector3(0f, -0f, 140.377579f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 188183, 1803116220u, new Vector3(-481.878235f, -2684.2998f, 8.090637f), new Vector3(0f, 0f, 45.9461975f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 209730, 1803116220u, new Vector3(-490.822754f, -2675.64746f, 8.090637f), new Vector3(0f, 0f, 45.9460754f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 163886, 1803116220u, new Vector3(-481.7864f, -2684.54688f, 8.090636f), new Vector3(0f, 0f, 46.2530823f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 759352, 1803116220u, new Vector3(-473.227142f, -2692.9812f, 8.090636f), new Vector3(0f, 0f, 47.37898f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier 2"), 77640, 2047212121u, new Vector3(-472.818939f, -2693.34644f, 8.756103f), new Vector3(0f, 0f, 46.9216843f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 358938, 1803116220u, new Vector3(-403.409119f, -2591.67969f, 12.9935131f), new Vector3(0f, 0f, 47.7364922f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 576553, 1803116220u, new Vector3(-389.76944f, -2604.92383f, 12.9935131f), new Vector3(0f, 0f, 54.79933f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier 2"), 341061, 2047212121u, new Vector3(-389.20343f, -2605.241f, 13.6584425f), new Vector3(0f, 0f, 53.9319153f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			})
		};

		public static MissionOjects MerryweatherGarageObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Merryweather Mesa"), 350237, 2230595153u, new Vector3(1222.99866f, -2284.926f, -49.22612f), new Vector3(0.184692666f, 0.000165594553f, -179.219513f), 152, 153, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Merryweather Mesa"), 652319, 2230595153u, new Vector3(1219.49524f, -2284.7688f, -49.22636f), new Vector3(0.180955485f, 0.000506616663f, 179.296188f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Merryweather Mesa"), 287525, 2230595153u, new Vector3(1233.38867f, -2284.86255f, -49.2256241f), new Vector3(0.184066489f, 0.000362704915f, 179.551926f), 154, 153, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "gr_prop_gr_crates_weapon_mix_01a"), 341291, 3804568937u, new Vector3(1230.72559f, -2287.14478f, -49.997818f), new Vector3(0f, -0f, -177.0708f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "gr_prop_gr_crates_weapon_mix_01b"), 626189, 4103028989u, new Vector3(1230.97949f, -2285.08984f, -49.99806f), new Vector3(0f, 0f, -87.96348f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "gr_prop_gr_crates_rifles_04a"), 134444, 443999472u, new Vector3(1231.03357f, -2283.33228f, -49.9996567f), new Vector3(0f, -0f, -90.82945f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier"), 590383, 3019107892u, new Vector3(1230.81311f, -2282.028f, -48.9954224f), new Vector3(0f, 0f, 9.076957f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier"), 201764, 3019107892u, new Vector3(1214.411f, -2290.65137f, -47.90624f), new Vector3(-0f, -0f, -12.4193726f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 42254, 3595011691u, new Vector3(1225.69165f, -2286.71338f, -49.9978523f), new Vector3(0f, -0f, -179.065048f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 126257, 3595011691u, new Vector3(1225.62073f, -2282.7146f, -49.99827f), new Vector3(0f, -0f, -179.013382f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sf_prop_sf_table_office_01a"), 174648, 3057578855u, new Vector3(1221.26331f, -2289.48047f, -49.9944267f), new Vector3(0f, -0f, 179.044037f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sf_prop_sf_table_office_01a"), 255765, 3057578855u, new Vector3(1235.413f, -2289.37671f, -49.99444f), new Vector3(0f, -0f, -179.882263f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_crate_set_01a"), 727349, 3641664093u, new Vector3(1216.22937f, -2287.4873f, -49.41009f), new Vector3(0f, -0f, 179.45192f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "gr_prop_gr_crates_sam_01a"), 745843, 3454541985u, new Vector3(1210.37634f, -2288.003f, -49.9986076f), new Vector3(0f, -0f, -179.370422f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "gr_prop_gr_crates_weapon_mix_01b"), 8476, 4103028989u, new Vector3(1208.39648f, -2288.025f, -49.99763f), new Vector3(0f, -0f, -179.370422f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "gr_prop_gr_cratespile_01a"), 40471, 3308813655u, new Vector3(1220.85217f, -2293.30127f, -48.9098434f), new Vector3(0f, -0f, 179.2468f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "gr_prop_gr_rsply_crate04b"), 681245, 1433923164u, new Vector3(1218.53589f, -2294.23242f, -48.9098969f), new Vector3(0f, -0f, 179.450546f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "gr_prop_gr_rsply_crate04b"), 768536, 1433923164u, new Vector3(1222.82532f, -2294.27368f, -48.9098434f), new Vector3(0f, -0f, 179.450546f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "gr_prop_gr_rsply_crate04a"), 395036, 1133202051u, new Vector3(1224.805f, -2294.29272f, -48.9098434f), new Vector3(0f, -0f, 179.450546f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_crate_03a"), 473888, 3235515268u, new Vector3(1231.16406f, -2294.30151f, -48.90992f), new Vector3(0f, -0f, -178.911423f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_crate_ch_04b"), 143416, 1697755746u, new Vector3(1225.60315f, -2280.27783f, -49.9998627f), new Vector3(0f, -0f, -179.6803f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_crate_hazard"), 248613, 2085196638u, new Vector3(1219.317f, -2289.183f, -49.9927635f), new Vector3(0f, -0f, 179.3459f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_crate_hazard"), 267808, 2085196638u, new Vector3(1218.46692f, -2289.15332f, -49.9927673f), new Vector3(0f, -0f, -177.123f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_crate_hazard"), 646176, 2085196638u, new Vector3(1217.61987f, -2289.08936f, -49.992775f), new Vector3(0f, -0f, 176.171982f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_crate_ch_04b"), 36142, 1697755746u, new Vector3(1205.288f, -2289.385f, -49.9989f), new Vector3(0f, -0f, 179.44606f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_01b"), 664125, 2568473661u, new Vector3(1207.42139f, -2274.68042f, -49.99984f), new Vector3(0f, 0f, 89.6309f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_02a"), 477758, 2104596125u, new Vector3(1205.766f, -2277.78223f, -49.9983635f), new Vector3(0f, 0f, 88.09465f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sf_prop_sf_table_office_01a"), 296010, 3057578855u, new Vector3(1204.79285f, -2274.08f, -49.82902f), new Vector3(0f, 0f, 89.01554f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "gr_prop_gr_rsply_crate04a"), 546362, 1133202051u, new Vector3(1204.90564f, -2275.60815f, -49.8335533f), new Vector3(0f, -0f, 93.928f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "gr_prop_gr_rsply_crate04a"), 227651, 1133202051u, new Vector3(1208.30286f, -2277.03f, -49.99984f), new Vector3(0f, 0f, 0.06094607f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Merryweather Mesa"), 282943, 2230595153u, new Vector3(1215.44f, -2274.10181f, -49.22699f), new Vector3(0.14774318f, 0.002158105f, 95.91407f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 444994, 3595011691u, new Vector3(1220.01111f, -2273.90137f, -49.998394f), new Vector3(0f, -0f, 90.38719f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_trolley_01a"), 437563, 546634076u, new Vector3(1222.7113f, -2273.193f, -49.61175f), new Vector3(0f, 0f, 89.97727f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_crate_set_01a"), 191594, 3641664093u, new Vector3(1205.00037f, -2280.6582f, -49.4108124f), new Vector3(0f, 0f, 0.665776f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_crate_closed_mw"), 27486, 3300657431u, new Vector3(1216.2063f, -2285.28442f, -49.8716354f), new Vector3(0f, -0f, -90.07786f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_crate_closed_mw"), 119143, 3300657431u, new Vector3(1216.11108f, -2285.289f, -49.0287628f), new Vector3(0f, -0f, -90.18016f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_crate_closed_mw"), 497530, 3300657431u, new Vector3(1216.33118f, -2283.38843f, -49.8718529f), new Vector3(0f, -0f, -91.7154846f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_crate_closed_mw"), 549950, 3300657431u, new Vector3(1236.3053f, -2279.84644f, -49.8721275f), new Vector3(0f, -0f, -90.33215f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_crate_closed_mw"), 259133, 3300657431u, new Vector3(1236.13245f, -2279.81665f, -49.0292549f), new Vector3(0f, 0f, -88.43795f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_crate_closed_mw"), 490300, 3300657431u, new Vector3(1236.2323f, -2279.83f, -48.1863823f), new Vector3(0f, -0f, -90.79085f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_box_wood04a"), 582980, 2972783418u, new Vector3(1236.84045f, -2277.78076f, -49.9957733f), new Vector3(0f, 0f, -89.91981f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_auto_salvage_elegy"), 365356, 3760858856u, new Vector3(1235.83484f, -2274.51074f, -49.97569f), new Vector3(0f, 0f, 12.8007259f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_pallet_ch_01a"), 49995, 414057361u, new Vector3(1236.33081f, -2287.99854f, -49.87501f), new Vector3(0f, -0f, -179.127121f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_pallet_ch_01a"), 510788, 414057361u, new Vector3(1236.28479f, -2285.571f, -49.8753052f), new Vector3(0f, -0f, -179.229111f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_pallet_ch_01a"), 697643, 414057361u, new Vector3(1236.24243f, -2283.3894f, -49.87552f), new Vector3(0f, -0f, -179.48465f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_gc_weapons"), 570694, 424713737u, new Vector3(1221.15686f, -2294.74121f, -46.94272f), new Vector3(0f, -0f, 179.701019f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_shelves_01a"), 297289, 1536058492u, new Vector3(1203.8822f, -2286.44531f, -49.996933f), new Vector3(0f, 0f, 89.26609f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_shelves_01a"), 755774, 1536058492u, new Vector3(1227.49011f, -2282.76758f, -49.9974022f), new Vector3(0f, -0f, 90.17853f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_miniature_yacht_01a"), 483101, 2597105484u, new Vector3(1227.90222f, -2281.82764f, -49.0433273f), new Vector3(0f, -0f, 110.289429f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 483657, 865563579u, new Vector3(1227.78076f, -2283.83032f, -49.01017f), new Vector3(0f, -0f, 111.207748f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 333372, 865563579u, new Vector3(1227.7522f, -2283.65332f, -49.01017f), new Vector3(0f, -0f, 107.727188f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 605773, 865563579u, new Vector3(1226.30225f, -2280.45239f, -48.8410378f), new Vector3(0f, -0f, 175.899673f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 704591, 865563579u, new Vector3(1226.19373f, -2280.44824f, -48.8410378f), new Vector3(0f, -0f, 169.348343f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 101199, 865563579u, new Vector3(1204.93347f, -2274.262f, -48.9957962f), new Vector3(0f, -0f, 109.6182f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 707391, 865563579u, new Vector3(1204.80762f, -2274.133f, -48.9957962f), new Vector3(0f, -0f, 101.480171f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 704366, 865563579u, new Vector3(1204.95593f, -2273.96851f, -48.9957962f), new Vector3(0f, -0f, 93.80281f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 585552, 865563579u, new Vector3(1203.85718f, -2287.05273f, -49.0097f), new Vector3(0f, -0f, 105.880173f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 596553, 865563579u, new Vector3(1203.76331f, -2286.88672f, -49.0097f), new Vector3(0f, -0f, 101.4272f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 314700, 865563579u, new Vector3(1203.88989f, -2286.723f, -49.0097f), new Vector3(0f, -0f, 97.89552f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 345392, 865563579u, new Vector3(1221.51282f, -2289.30566f, -49.1612f), new Vector3(0f, -0f, -147.355087f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 361260, 865563579u, new Vector3(1221.4491f, -2289.41724f, -49.1612f), new Vector3(0f, -0f, -151.039856f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 362289, 865563579u, new Vector3(1221.26868f, -2289.363f, -49.1612f), new Vector3(0f, -0f, -155.54361f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 681026, 865563579u, new Vector3(1235.452f, -2289.215f, -49.1612129f), new Vector3(0f, -0f, -153.289536f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 386642, 865563579u, new Vector3(1235.33862f, -2289.30273f, -49.1612129f), new Vector3(0f, -0f, -158.100235f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 202835, 865563579u, new Vector3(1235.16772f, -2289.251f, -49.1612129f), new Vector3(0f, -0f, -163.166931f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 613192, 865563579u, new Vector3(1208.37537f, -2277.09888f, -48.8216972f), new Vector3(0f, 0f, 40.1664124f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 578108, 865563579u, new Vector3(1205.129f, -2281.579f, -48.7886276f), new Vector3(0f, -0f, 97.07911f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 276533, 865563579u, new Vector3(1205.07837f, -2281.42725f, -48.7886276f), new Vector3(0f, -0f, 91.7049255f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 476214, 865563579u, new Vector3(1206.00659f, -2289.20972f, -48.8400764f), new Vector3(0f, -0f, 171.546112f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 141136, 865563579u, new Vector3(1205.83374f, -2289.277f, -48.8400764f), new Vector3(0f, -0f, 166.223114f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 649556, 865563579u, new Vector3(1221.00781f, -2292.378f, -48.0205421f), new Vector3(0f, -0f, -161.0196f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 400978, 865563579u, new Vector3(1220.94092f, -2292.50659f, -48.0205421f), new Vector3(0f, -0f, -167.979935f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 2127, 865563579u, new Vector3(1231.95081f, -2294.02979f, -47.71661f), new Vector3(0f, -0f, -151.703445f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 778607, 865563579u, new Vector3(1231.86963f, -2294.21313f, -47.71661f), new Vector3(0f, -0f, -155.900009f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 334162, 865563579u, new Vector3(1231.66528f, -2294.06763f, -47.71661f), new Vector3(0f, -0f, -159.636032f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects MerryweatherFacilityObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 57615, 277707989u, new Vector3(-1904.92114f, 3756.06079f, -99.72176f), new Vector3(0f, 0f, 20.9529953f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 361218, 277707989u, new Vector3(-1899.44421f, 3758.252f, -99.72176f), new Vector3(0f, 0f, -62.5743866f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 144133, 277707989u, new Vector3(-1899.01038f, 3744.74756f, -99.36183f), new Vector3(0f, -0f, 114.248611f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 20252, 277707989u, new Vector3(-1891.91162f, 3739.57617f, -99.36183f), new Vector3(0f, -0f, -140.624786f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 79389, 277707989u, new Vector3(-1882.05664f, 3759.15063f, -99.72178f), new Vector3(0f, -0f, -163.040848f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 782874, 277707989u, new Vector3(-1891.59741f, 3756.613f, -99.72176f), new Vector3(0f, -0f, 110.25753f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 475400, 277707989u, new Vector3(-1874.53f, 3747.088f, -99.81946f), new Vector3(0f, 0f, 58.4090233f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_corpse_01"), 558871, 497099277u, new Vector3(-1870.19934f, 3753.84326f, -100.182495f), new Vector3(0f, 0f, 21.4044914f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_corpse_02"), 681225, 794936718u, new Vector3(-1872.76672f, 3752.34717f, -100.150642f), new Vector3(0f, 0f, 55.54209f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_corpse_03"), 150817, 1116990450u, new Vector3(-1872.61816f, 3747.12915f, -100.186012f), new Vector3(0f, -0f, 131.698273f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_ped_hillbilly_01a"), 313446, 462487855u, new Vector3(-1869.84436f, 3745.5686f, -100.18f), new Vector3(0f, -0f, -100.963028f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier"), 203811, 3019107892u, new Vector3(-1873.86f, 3751.2876f, -99.8496f), new Vector3(-0f, -0f, -67.53751f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier"), 146207, 3019107892u, new Vector3(-1883.047f, 3759.87549f, -99.65082f), new Vector3(0f, -0f, 166.563263f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier"), 51808, 3019107892u, new Vector3(-1894.89368f, 3738.81079f, -99.65126f), new Vector3(-0f, -0f, -21.4153824f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier"), 693087, 3019107892u, new Vector3(-1899.90137f, 3759.112f, -99.65068f), new Vector3(0f, -0f, 133.552109f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier"), 471582, 3019107892u, new Vector3(-1910.29663f, 3748.45117f, -99.6492844f), new Vector3(-0f, -0f, -50.6928864f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects TunerGarageObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "300R"), 94723, 1076201208u, new Vector3(-1349.70129f, 158.706192f, -99.82316f), new Vector3(0.0638842657f, -0.000683145947f, -121.367439f), 112, 0, 111, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Comet Retro Custom"), 618365, 2272483501u, new Vector3(-1350.863f, 149.964417f, -99.9076843f), new Vector3(0.01086709f, 8.52950252E-05f, -72.2843246f), 33, 111, 38, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Coquette D10"), 119316, 2566281822u, new Vector3(-1342.67334f, 157.361526f, -99.91984f), new Vector3(-0.023469504f, 4.020156E-05f, -152.53566f), 73, 111, 74, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Elegy Retro Custom"), 557636, 196747873u, new Vector3(-1338.877f, 154.849518f, -99.86554f), new Vector3(0.00753655564f, 0.000361568527f, 148.964142f), 79, 5, 61, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Jester Classic"), 126725, 4080061290u, new Vector3(-1343.33435f, 148.964264f, -99.89847f), new Vector3(-0.0221549366f, 0.000210816186f, 27.56182f), 38, 8, 5, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Sultan Classic"), 286830, 872704284u, new Vector3(-1334.97961f, 148.949753f, -99.8328f), new Vector3(0f, -0f, 166.568741f), 9, 9, 2, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_crates_sam_01a"), 409106, 3410831756u, new Vector3(-1344.44958f, 145.930984f, -100.193832f), new Vector3(0f, 0f, -2.36193275f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_crates_rifles_04a"), 623369, 4085396070u, new Vector3(-1344.70776f, 142.757553f, -100.19429f), new Vector3(0f, 0f, 0.5553951f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_tarpcrate_01a"), 414980, 4137360559u, new Vector3(-1342.50793f, 138.490021f, -100.186089f), new Vector3(0f, 0f, 89.0472f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_cgo_01a"), 672375, 702180737u, new Vector3(-1336.97668f, 142.6076f, -100.192131f), new Vector3(0f, -0f, 178.510834f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_cgo_01a"), 168564, 702180737u, new Vector3(-1335.13379f, 143.084366f, -100.192131f), new Vector3(0f, 0f, -88.69703f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_01c"), 694537, 595803909u, new Vector3(-1333.30737f, 143.2923f, -100.19429f), new Vector3(0f, -0f, 97.54283f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_carrier_cargo_02a"), 56835, 3946537745u, new Vector3(-1329.68542f, 137.9122f, -100.193916f), new Vector3(0f, -0f, 178.66452f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_carrier_cargo_03a"), 614922, 102012783u, new Vector3(-1322.01721f, 137.5117f, -100.193916f), new Vector3(0f, -0f, -176.984558f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_carrier_cargo_03a"), 39437, 102012783u, new Vector3(-1321.84912f, 140.550629f, -100.193916f), new Vector3(0f, -0f, -92.22811f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_carrier_cargo_04b"), 151416, 2940106248u, new Vector3(-1321.48877f, 143.156387f, -100.191483f), new Vector3(0f, -0f, -92.38159f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_carrier_cargo_05b"), 514058, 811366734u, new Vector3(-1328.84f, 145.943832f, -100.19429f), new Vector3(0f, -0f, -92.84186f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_carrier_cargo_05b"), 392969, 811366734u, new Vector3(-1322.36719f, 150.231857f, -100.19429f), new Vector3(0f, 0f, -89.51458f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_cargo_01a"), 680972, 3395017374u, new Vector3(-1324.694f, 151.164429f, -100.19429f), new Vector3(0f, 0f, -87.21136f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_cargo_01a"), 57614, 3395017374u, new Vector3(-1340.14221f, 157.924454f, -100.19429f), new Vector3(0f, 0f, 3.83985925f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_air_cargo_04c"), 620409, 3488393308u, new Vector3(-1328.16089f, 158.927765f, -100.175659f), new Vector3(0f, 0f, 0.308434159f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mb_cargo_04a"), 177014, 897366637u, new Vector3(-1330.39575f, 159.354675f, -100.19429f), new Vector3(0f, 0f, -0.203377172f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_tarpcargo_01b"), 643338, 815254291u, new Vector3(-1334.13281f, 156.9713f, -100.120369f), new Vector3(0f, 0f, 0.7690557f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ZR350"), 683281, 1923534526u, new Vector3(-1328.74512f, 154.458527f, -99.5792847f), new Vector3(0.0382192768f, -0.00125479233f, 27.3831482f), 2, 6, 6, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Jester RR"), 488456, 2531693357u, new Vector3(-1335.38416f, 139.088226f, -99.8273849f), new Vector3(-0.06912194f, 0.0237327311f, 111.422142f), 112, 0, 111, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Futo GTX"), 342033, 2787736776u, new Vector3(-1325.03381f, 144.922363f, -99.70404f), new Vector3(0.450618118f, -0.000885509f, -178.561356f), 0, 3, 6, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Euros"), 13431, 821121576u, new Vector3(-1327.84741f, 140.662628f, -99.85129f), new Vector3(0.051068075f, -0.0009511158f, -128.7117f), 2, 6, 6, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Autoshop Worker"), 79895, 68070371u, new Vector3(-1348.30359f, 153.110291f, -99.19947f), new Vector3(0f, -0f, 157.12999f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Autoshop Worker"), 346892, 68070371u, new Vector3(-1348.81506f, 145.883636f, -95.12547f), new Vector3(0f, 0f, -5.933523f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Autoshop Worker 2"), 763193, 4033578141u, new Vector3(-1337.79114f, 146.8606f, -99.197876f), new Vector3(0f, -0f, 111.270294f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Autoshop Worker 2"), 95824, 4033578141u, new Vector3(-1330.4552f, 139.9075f, -99.19839f), new Vector3(0f, 0f, 7.935065f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Autoshop Worker"), 32284, 68070371u, new Vector3(-1324.85217f, 149.584076f, -99.19822f), new Vector3(0f, -0f, 109.222076f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Autoshop Worker"), 125187, 68070371u, new Vector3(-1330.457f, 157.924225f, -99.19882f), new Vector3(0f, -0f, 177.905991f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Autoshop Worker"), 17944, 68070371u, new Vector3(-1341.79578f, 159.867691f, -99.19919f), new Vector3(0f, 0f, -83.2120056f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Autoshop Worker"), 523534, 68070371u, new Vector3(-1338.69531f, 151.38649f, -98.74305f), new Vector3(0f, 0f, 16.27591f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Autoshop Worker"), 483177, 68070371u, new Vector3(-1366.892f, 164.2193f, -98.99952f), new Vector3(0f, -0f, -103.129272f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects GrinderObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 330002, 2271741578u, new Vector3(-1333.26221f, 142.688538f, -98.9584351f), new Vector3(0f, 0f, 70.63848f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 657520, 2271741578u, new Vector3(-1332.635f, 152.902588f, -99.2608f), new Vector3(0f, -0f, -173.853119f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 335368, 2271741578u, new Vector3(-1340.476f, 150.662033f, -99.26079f), new Vector3(0f, -0f, 161.783768f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 737345, 2271741578u, new Vector3(-1349.98279f, 142.1007f, -95.2447f), new Vector3(0f, -0f, 170.279236f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 69384, 2271741578u, new Vector3(-1358.12415f, 138.368027f, -95.2806244f), new Vector3(0f, -0f, -163.414063f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 184686, 2271741578u, new Vector3(-1363.13269f, 165.190842f, -98.80955f), new Vector3(0f, 0f, 33.8807068f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 507658, 2271741578u, new Vector3(-1346.55859f, 160.154785f, -99.01033f), new Vector3(0f, -0f, -140.492661f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 604991, 2271741578u, new Vector3(-1351.07947f, 155.366959f, -98.9030457f), new Vector3(0f, 0f, -86.80319f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects KeycardObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_securitycard_01a"), 608367, 3575961148u, new Vector3(461.134064f, -988.7814f, 24.8233871f), new Vector3(-3.201651E-07f, 8.773804E-05f, 100.500481f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects SamJammerObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_jammer_01a"), 280151, 3188649882u, new Vector3(-2359.02539f, 3241.3772f, 92.9044647f), new Vector3(0f, -0f, -163.366989f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_jammer_01a"), 32574, 3188649882u, new Vector3(-2364.8938f, 3246.16382f, 92.9044647f), new Vector3(0f, 0f, 77.2272339f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_jammer_01a"), 594010, 3188649882u, new Vector3(-2361.80469f, 3251.13525f, 101.2788f), new Vector3(0f, 0f, 69.1307144f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_jammer_01a"), 234820, 3188649882u, new Vector3(-2360.08862f, 3248.93433f, 101.256676f), new Vector3(0f, -0f, 146.515381f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_jammer_01a"), 240467, 3188649882u, new Vector3(-2358.25464f, 3251.056f, 101.451515f), new Vector3(0f, 0f, -42.7950859f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_jammer_01a"), 535058, 3188649882u, new Vector3(-2353.924f, 3253.88623f, 92.90113f), new Vector3(0f, -0f, 143.545853f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_jammer_01a"), 595790, 3188649882u, new Vector3(-2357.07739f, 3255.68628f, 92.90113f), new Vector3(0f, -0f, -161.383667f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_jammer_01a"), 675417, 3188649882u, new Vector3(-2346.99487f, 3268.70239f, 32.7970428f), new Vector3(0f, -0f, -96.2798f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static Vector3 SamIntelLoc = new Vector3(-2360.08862f, 3248.93433f, 101.256676f);

		public static Vector3 KeycardIntelLoc = new Vector3(461.134064f, -988.7814f, 24.8233871f);

		public static Vector3 TunerGarageEnter = new Vector3(331.65f, -1119.551f, 28.4f);

		public static Vector3 TunerGarageExit = new Vector3(-1358.15f, 168.37f, -99.77f);

		public static Vector3 MerryweatherGarageEnter = new Vector3(1130.47f, -1304.74f, 33.74f);

		public static Vector3 MerryweatherGarageExit = new Vector3(1203.53f, -2268.3f, -48.11f);

		public static Vector3 MerryweatherFacilityEnter = new Vector3(550.94f, -3052.301f, 12.28f);

		public static Vector3 MerryweatherFacilityExit = new Vector3(-1922.09f, 3749.717f, -100.64f);

		public static Vector3 CargoshipIntelLoc = new Vector3(-469f, -2672f, 0f);

		public static Vector3 AircraftCarrierLoc = new Vector3(3050f, -4650f, 20f);

		public static bool CreatedCargoshipObjects;

		public static bool CreatedMerryweatherFacilityObjects;

		public static bool CreatedMerryweatherGarageObjects;

		public static bool CreatedTunerGarageObjects;

		public static bool CreatedKeycardIntelObjects;

		public static bool CreatedSamIntelObjects;

		public Vehicle Raiju;

		public static bool UsingRaiju;

		public Vehicle Valkyrie;

		public static bool UsingValkyrie;

		public Vehicle PTBoat;

		public static bool UsingPTBoat;

		public int StyleId = 4;

		public int BedroomId;

		public int ColorId = 1;

		public int LiftId = 1;

		public int ClutterId;

		public bool LoadTunerGarage = true;

		public static List<SmartGuards.SmartAiV2> SpawnGroupsUse = new List<SmartGuards.SmartAiV2>();

		public static bool CreatedAircraftCarrier;

		public static bool CreatedCargoship;

		public static List<Prop> SamSiteProps = new List<Prop>();

		public static List<SamSite> SamSites = new List<SamSite>();

		public static bool FreedPTBoat;

		public static Prop Winch;

		public static Prop Rope;

		public static List<Blip> IntelMarkers = new List<Blip>();

		public static List<Blip> IntelMarkersRadiusArea = new List<Blip>();

		public static Blip TargetBlip;

		public static Blip TargetBlipArea;

		public static bool TheLuxingtonRaidHeistOn;

		public static Prop SamJammer;

		public static Prop Grinder;

		public static List<Prop> ContainersProps = new List<Prop>();

		public static int PercentNeedTake = (int)((float)(HeistMinPay + HeistMaxPay) / 1.5f / 100f * 35f);

		public static Prop Keycard;

		public static bool InTunerGarage;

		public static bool InMerryweatherFacility;

		public static bool InMerryweatherGarage;

		public static HeistStage CurrentHeistStage;

		public static Vector3 CurrentContainer;

		public static List<Container> Containers = new List<Container>();

		private static void drawSprite2(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha)
		{
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, screenX, screenY, width, height, 0, r, g, b, alpha, 0);
			}
		}

		public static void drawText(string text, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		public static void ShowTextInfoBar(string Action, int Amt, int Slot, Vector2 TextOffset1, Vector2 TextOffset2, bool Money)
		{
			if (Slot == 1)
			{
				drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f, 0.21f, 0.04f, 255, 255, 255, 130);
				drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f, 0.21f, 0.04f, 255, 255, 255, 130);
				drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f, 0.21f, 0.04f, 255, 255, 255, 130);
				drawText(Action + " ", 0.895f + TextOffset1.X, 0.961f + TextOffset1.Y, 0.35f, 255, 255, 255);
				if (!Money)
				{
					drawText(Amt.ToString(), 0.965f + TextOffset2.X, 0.961f + TextOffset2.Y, 0.35f, 255, 255, 255);
				}
				else
				{
					drawText("~g~$" + Amt.ToString("N"), 0.965f + TextOffset2.X, 0.961f + TextOffset2.Y, 0.35f, 255, 255, 255);
				}
			}
			if (Slot == 2)
			{
				drawSprite2("timerbars", "all_black_bg", 0.89f, 0.93f, 0.21f, 0.04f, 255, 255, 255, 130);
				drawSprite2("timerbars", "all_black_bg", 0.89f, 0.93f, 0.21f, 0.04f, 255, 255, 255, 130);
				drawSprite2("timerbars", "all_black_bg", 0.89f, 0.93f, 0.21f, 0.04f, 255, 255, 255, 130);
				drawText(Action + "  ", 0.895f + TextOffset1.X, 0.921f + TextOffset1.Y, 0.35f, 255, 255, 255);
				if (!Money)
				{
					drawText(Amt.ToString(), 0.965f + TextOffset2.X, 0.921f + TextOffset2.Y, 0.35f, 255, 255, 255);
				}
				else
				{
					drawText("~g~$" + Amt.ToString("N"), 0.965f + TextOffset2.X, 0.921f + TextOffset2.Y, 0.35f, 255, 255, 255);
				}
			}
			if (Slot == 3)
			{
				drawSprite2("timerbars", "all_black_bg", 0.89f, 0.89f, 0.21f, 0.04f, 255, 255, 255, 130);
				drawSprite2("timerbars", "all_black_bg", 0.89f, 0.89f, 0.21f, 0.04f, 255, 255, 255, 130);
				drawSprite2("timerbars", "all_black_bg", 0.89f, 0.89f, 0.21f, 0.04f, 255, 255, 255, 130);
				drawText(Action + " ", 0.895f + TextOffset1.X, 0.881f + TextOffset1.Y, 0.35f, 255, 255, 255);
				if (!Money)
				{
					drawText(Amt.ToString(), 0.965f + TextOffset2.X, 0.881f + TextOffset2.Y, 0.35f, 255, 255, 255);
				}
				else
				{
					drawText("~g~$" + Amt.ToString("N"), 0.965f + TextOffset2.X, 0.881f + TextOffset2.Y, 0.35f, 255, 255, 255);
				}
			}
		}

		public void ConvertNavToSmartAIV2(List<MissionOjects> Navs, List<SmartGuards.SmartAiV2> AIGroup)
		{
			foreach (MissionOjects Nav in Navs)
			{
				List<Vector3> list = new List<Vector3>();
				for (int i = 0; i < Nav.Entities.Count; i++)
				{
					ObjectPlacementMap objectPlacementMap = Nav.Entities[i];
					if (objectPlacementMap.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"))
					{
						list.Add(objectPlacementMap.Position);
					}
					if (objectPlacementMap.Hash != Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"))
					{
						SmartGuards.PedTaskLoop wL = new SmartGuards.PedTaskLoop(list, 0, true);
						SmartGuards.SmartAiV2 smartAiV = new SmartGuards.SmartAiV2(0, objectPlacementMap.Inputtedhash, objectPlacementMap.Position, objectPlacementMap.Rotation.Z, true, 200, 100, 1, wL, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Merryweather);
						smartAiV.GroupActive = false;
						smartAiV.DistToSpawn = 200f;
						smartAiV.SpawnGroupArea = objectPlacementMap.Position;
						AIGroup.Add(smartAiV);
					}
				}
			}
		}

		public void DisplayHelpTextThisFrame(string helpText, int Shape)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, 1);
		}

		public static void TextNotification(string avatar, string author, string title, string message)
		{
			Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "CONFIRM_BEEP", "HUD_MINI_GAME_SOUNDSET");
			Function.Call(Hash.BEGIN_TEXT_COMMAND_THEFEED_POST, new InputArgument[1] { "STRING" });
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { message });
			int num = Function.Call<int>(Hash.END_TEXT_COMMAND_THEFEED_POST_MESSAGETEXT, new InputArgument[6] { avatar, avatar, true, 0, title, author });
		}

		public static string LoadDict(string dict)
		{
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, dict))
			{
				Function.Call(Hash.REQUEST_ANIM_DICT, dict);
				Script.Yield();
			}
			return dict;
		}

		public void DisplayHelpTextThisFrame(string helpText)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, 1);
		}

		public static Model RequestModel(string Name)
		{
			Model result = new Model(Name);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(50);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public static Model RequestModel(VehicleHash Name)
		{
			Model result = new Model(Name);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(50);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public static Model RequestModel(PedHash Name)
		{
			Model result = new Model(Name);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(50);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public static Model RequestModel(int Name)
		{
			Model result = new Model(Name);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(50);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public void LOADIPL(string IPL)
		{
			try
			{
				Function.Call(Hash.REQUEST_IPL, IPL);
			}
			catch
			{
				Screen.ShowSubtitle("~r~IPL CAll OUSIDE MAIN LOOP~w~{" + IPL + "}");
			}
		}

		public void RefreshTunerGarage()
		{
			if (LoadTunerGarage)
			{
				Function.Call(Hash.REMOVE_IPL, "tr_int_placement_tr_interior_0_tuner_mod_garage_milo_");
				LOADIPL("tr_int_placement_tr_interior_0_tuner_mod_garage_milo_");
				int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -1350f, 160f, -100f);
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_bedroom");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_bedroom_empty");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_bombs");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_box_clutter");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_car_lift_cutscene");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_car_lift_default");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_car_lift_purchase");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_chalkboard");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_container");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_cut_seats");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_def_table");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_drive");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_ecu");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_IAA");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_jammers");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_laptop");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_lightbox");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_methLab");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_plate");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_scope");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_table");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_virus");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_thermal");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_train");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_tints");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_style_2");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_bedroom");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_bedroom_empty");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_bombs");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_box_clutter");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_car_lift_cutscene");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_chalkboard");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_container");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_cut_seats");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_def_table");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_drive");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_ecu");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_IAA");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_jammers");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_laptop");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_lightbox");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_methLab");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_plate");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_scope");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_table");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_virus");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_thermal");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_train");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_tints");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_style_1");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_style_2");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_style_3");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_style_4");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_style_5");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_style_6");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_style_7");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_style_8");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_style_9");
				Function.Call(Hash.REFRESH_INTERIOR, num);
				if (BedroomId == 1)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_bedroom");
				}
				if (BedroomId == 0)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_bedroom_empty");
				}
				if (ClutterId == 0)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_box_clutter");
				}
				if (ClutterId == 1)
				{
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_box_clutter");
				}
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_bombs");
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_car_lift_cutscene");
				if (LiftId == 0)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, "entity_set_car_lift_default");
				}
				if (LiftId == 1)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, "entity_set_car_lift_default");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, "entity_set_car_lift_purchase");
				}
				Function.Call((Hash)3590001984630001091uL, num, "entity_set_tints", ColorId);
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, ("entity_set_style_" + StyleId).ToString());
				Function.Call(Hash.REFRESH_INTERIOR, num);
			}
			else
			{
				Function.Call(Hash.REMOVE_IPL, "tr_int_placement_tr_interior_0_tuner_mod_garage_milo_");
			}
		}

		public static void ATTACH_ENTITY_TO_ENTITY(Entity entity1, Entity entity2, int boneIndex, float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, bool p9, bool useSoftPinning, bool collision, bool isPed, int vertexIndex, bool fixedRot)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, entity1, entity2, boneIndex, xPos, yPos, zPos, xRot, yRot, zRot, p9, useSoftPinning, collision, isPed, vertexIndex, fixedRot);
		}

		public TheLexingtonRaid()
		{
			List<SmartGuards.SmartAiV2> list = new List<SmartGuards.SmartAiV2>();
			foreach (MissionOjects aircraftCarrierNav in AircraftCarrierNavs)
			{
				List<Vector3> list2 = new List<Vector3>();
				for (int i = 0; i < aircraftCarrierNav.Entities.Count; i++)
				{
					ObjectPlacementMap objectPlacementMap = aircraftCarrierNav.Entities[i];
					if (objectPlacementMap.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"))
					{
						list2.Add(objectPlacementMap.Position);
					}
					if (objectPlacementMap.Hash != Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"))
					{
						SmartGuards.PedTaskLoop wL = new SmartGuards.PedTaskLoop(list2, 0, true);
						list.Add(new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", objectPlacementMap.Position, objectPlacementMap.Rotation.Z, true, 200, 100, 1, wL, 40, 50, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Merryweather));
					}
				}
			}
			CarrierGuards.AddRange(list);
			base.Tick += OnTick;
		}

		public static bool GetDistance_VDIST2(Vector3 _1, Vector3 _2, string Sign, float Radius)
		{
			bool result = false;
			float num = Radius * Radius;
			float num2 = Function.Call<float>(Hash.VDIST2, _1.X, _1.Y, _1.Z, _2.X, _2.Y, _2.Z);
			if (Sign.Equals("<=") && num2 <= num)
			{
				result = true;
			}
			if (Sign.Equals("<") && num2 < num)
			{
				result = true;
			}
			if (Sign.Equals(">=") && num2 >= num)
			{
				result = true;
			}
			if (Sign.Equals(">") && num2 > num)
			{
				result = true;
			}
			return result;
		}

		public static void EndHeist()
		{
			ManageRobberyAnims.DeleteLoot();
			Function.Call(Hash.SET_RADAR_ZOOM, 850);
			TheLuxingtonRaidHeistOn = false;
			SamSiteProps.Clear();
			SamSites.Clear();
			foreach (Container container in Containers)
			{
				Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, container.PTFX2, false);
				Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, container.PTFX1, false);
				container.containerswitch = 0;
				if (container.Bomb != null)
				{
					container.Bomb.Delete();
				}
				container.BombExplodeTimer = 0;
				container.plantBombStage = 0;
				container.unstucktimer = 0;
				container.LargeContainerAnim = 0;
				if (container.Bag != null)
				{
					container.Bag.Delete();
				}
				if (container.Bomb != null)
				{
					container.Bomb.Delete();
				}
				if (container.Swipecard != null)
				{
					container.Swipecard.Delete();
				}
				if (container.Saw != null)
				{
					container.Saw.Delete();
				}
				if (container.Container1 != null)
				{
					container.Container1.Delete();
				}
				if (container.FakeContainer != null)
				{
					container.FakeContainer.Delete();
				}
				if (container.Lock != null)
				{
					container.Lock.Delete();
				}
				Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, container.ParticleFx);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, container.SyncScene, 0f);
			}
			Containers.Clear();
			foreach (SmartGuards.SmartAiV2 cargoshipGuard in CargoshipGuards)
			{
				if (cargoshipGuard != null && cargoshipGuard.AIPed != null)
				{
					cargoshipGuard.AIPed.Delete();
				}
			}
			foreach (SmartGuards.SmartAiV2 merryweatherFacilityGuard in MerryweatherFacilityGuards)
			{
				if (merryweatherFacilityGuard != null && merryweatherFacilityGuard.AIPed != null)
				{
					merryweatherFacilityGuard.AIPed.Delete();
				}
			}
			foreach (SmartGuards.SmartAiV2 merryweatherGarageGuard in MerryweatherGarageGuards)
			{
				if (merryweatherGarageGuard != null && merryweatherGarageGuard.AIPed != null)
				{
					merryweatherGarageGuard.AIPed.Delete();
				}
			}
			foreach (SmartGuards.SmartAiV2 tunerGarageGuard in TunerGarageGuards)
			{
				if (tunerGarageGuard != null && tunerGarageGuard.AIPed != null)
				{
					tunerGarageGuard.AIPed.Delete();
				}
			}
			Containers.Clear();
			FreedPTBoat = false;
			CurrentHeistStage = HeistStage.Scout;
			UsingRaiju = false;
			UsingValkyrie = false;
			UsingPTBoat = false;
			GotSamJammer = false;
			GotGrinder = false;
			GotC4 = false;
			C4ChargesCollected = 0;
			GotThermite = false;
			ThermiteChargesCollected = 0;
			GotKeycard = false;
			CurrentPay = 0;
			LocalTake = 0;
			MerryweatherGarageObjects.DeleteEntites();
			MerryweatherFacilityObjects.DeleteEntites();
			TunerGarageObjects.DeleteEntites();
			GrinderObjects.DeleteEntites();
			KeycardObjects.DeleteEntites();
			CargoshipObjects.DeleteEntites();
			SamJammerObjects.DeleteEntites();
			CreatedCargoshipObjects = false;
			CreatedMerryweatherFacilityObjects = false;
			CreatedMerryweatherGarageObjects = false;
			CreatedTunerGarageObjects = false;
			CreatedKeycardIntelObjects = false;
			CreatedSamIntelObjects = false;
			foreach (MissionOjects item in AircraftCarrierSpawnLoot)
			{
				item.DeleteEntites();
			}
			AircraftCarrierLoot.DeleteEntites();
			AircraftCarrierObjects.DeleteEntites();
			if (TargetBlip != null)
			{
				TargetBlip.Delete();
			}
			if (TargetBlipArea != null)
			{
				TargetBlipArea.Delete();
			}
			foreach (Blip intelMarker in IntelMarkers)
			{
				if (intelMarker != null)
				{
					intelMarker.Delete();
				}
			}
			foreach (Blip item2 in IntelMarkersRadiusArea)
			{
				if (item2 != null)
				{
					item2.Delete();
				}
			}
			foreach (SmartGuards.SmartAiV2 cargoshipGuard2 in CargoshipGuards)
			{
				if (cargoshipGuard2.AIPed != null)
				{
					cargoshipGuard2.AIPed.Delete();
				}
			}
		}

		public static void StartHeist()
		{
			ManageRobberyAnims.Heist_Average_Pay = (float)(HeistMinPay + HeistMaxPay) / 2f;
			Function.Call(Hash.REQUEST_IPL, "hei_carrier");
			Function.Call(Hash.REQUEST_IPL, "hei_carrier_DistantLights");
			Function.Call(Hash.REQUEST_IPL, "hei_Carrier_int1");
			Function.Call(Hash.REQUEST_IPL, "hei_Carrier_int2");
			Function.Call(Hash.REQUEST_IPL, "hei_Carrier_int3");
			Function.Call(Hash.REQUEST_IPL, "hei_Carrier_int4");
			Function.Call(Hash.REQUEST_IPL, "hei_Carrier_int5");
			Function.Call(Hash.REQUEST_IPL, "hei_Carrier_int6");
			Function.Call(Hash.REQUEST_IPL, "hei_carrier_LODLights");
			Function.Call(Hash.BEGIN_TEXT_COMMAND_BUSYSPINNER_ON, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "Loading The Luxignton Raid Heist, Please Wait");
			Function.Call(Hash.END_TEXT_COMMAND_BUSYSPINNER_ON, 5);
			EndHeist();
			Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 3033.639f, -4772.087f, 11.61987f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_carrier_defense_01"), 0, 0, 0);
			if (!SamSiteProps.Contains(prop))
			{
				SamSites.Add(new SamSite(prop));
				SamSiteProps.Add(prop);
			}
			prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 3057.013f, -4578.828f, 11.32295f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_carrier_defense_01"), 0, 0, 0);
			if (!SamSiteProps.Contains(prop))
			{
				SamSites.Add(new SamSite(prop));
				SamSiteProps.Add(prop);
			}
			prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 3120.947f, -4811.005f, 10.86835f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_carrier_defense_01"), 0, 0, 0);
			if (!SamSiteProps.Contains(prop))
			{
				SamSites.Add(new SamSite(prop));
				SamSiteProps.Add(prop);
			}
			CreatedAircraftCarrier = true;
			AircraftCarrierObjects.CreateEntites();
			foreach (ObjectPlacementMap entity in AircraftCarrierObjects.Entities)
			{
				if (entity.Entity != null)
				{
					entity.Entity.IsPositionFrozen = true;
				}
				int num = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100);
				if (num < 55 && entity.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"))
				{
					MissionOjects missionOjects = new MissionOjects();
					missionOjects.Entities = new List<ObjectPlacementMap>();
					missionOjects.Entities.Add(new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sf_prop_sf_table_office_01a"), 587522, 3057578855u, new Vector3(3078.21875f, -4759.44238f, 5.078699f), new Vector3(0f, -0f, -163.626114f), 312191, 0, new Vector3(0f, 1.1f, 0f), new Vector3(0f, 0f, 180f)));
					missionOjects.Entities.Add(new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_vancrate_01a"), 216438, 4126380171u, new Vector3(3078.21875f, -4759.44238f, 6.1186986f), new Vector3(0f, -0f, -343.6261f), 587522, 0, new Vector3(0f, 0f, 1.03999937f), new Vector3(0f, 0f, 0f)));
					missionOjects.CreateEntites();
					ATTACH_ENTITY_TO_ENTITY(missionOjects.Entities[0].Entity, entity.Entity, -1, missionOjects.Entities[0].EntityToAttachOffset.X, missionOjects.Entities[0].EntityToAttachOffset.Y, missionOjects.Entities[0].EntityToAttachOffset.Z, missionOjects.Entities[0].EntityToAttachRotation.X, missionOjects.Entities[0].EntityToAttachRotation.Y, missionOjects.Entities[0].EntityToAttachRotation.Z, false, false, true, false, 2, true);
					ATTACH_ENTITY_TO_ENTITY(missionOjects.Entities[1].Entity, missionOjects.Entities[0].Entity, -1, missionOjects.Entities[1].EntityToAttachOffset.X, missionOjects.Entities[1].EntityToAttachOffset.Y, missionOjects.Entities[1].EntityToAttachOffset.Z, missionOjects.Entities[1].EntityToAttachRotation.X, missionOjects.Entities[1].EntityToAttachRotation.Y, missionOjects.Entities[1].EntityToAttachRotation.Z, false, false, true, false, 2, true);
					AircraftCarrierSpawnLoot.Add(missionOjects);
					missionOjects.Entities[0].Entity.Detach();
					missionOjects.Entities[1].Entity.Detach();
					ManageRobberyAnims.AddPropAsLoot((Prop)missionOjects.Entities[1].Entity);
					Prop prop2 = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_container_01a"), entity.Entity.Position, entity.Entity.Rotation, false, false);
					Containers.Add(new Container(prop2));
					ContainersProps.Add(prop2);
					prop2.AddBlip();
					prop2.AttachedBlip.Sprite = BlipSprite.ShippingContainer;
					prop2.AttachedBlip.Color = BlipColor.White;
					prop2.AttachedBlip.Alpha = 255;
					prop2.AttachedBlip.Name = "Robbable Container";
					prop2.AttachedBlip.Alpha = 0;
					entity.Entity.Delete();
				}
				num = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100);
				if (num >= 75 || entity.Hash != Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_01a"))
				{
					continue;
				}
				MissionOjects missionOjects2 = new MissionOjects();
				missionOjects2.Entities = new List<ObjectPlacementMap>();
				missionOjects2.Entities.Add(new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sf_prop_sf_table_office_01a"), 299270, 3057578855u, new Vector3(3071.925f, -4779.33936f, 5.247253f), new Vector3(0f, -0f, 102.235321f), 95324, 0, new Vector3(-0.759999633f, 2.709998f, 0.170000046f), new Vector3(0f, 0f, 0f)));
				missionOjects2.Entities.Add(new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sf_prop_sf_table_office_01a"), 690703, 3057578855u, new Vector3(3071.60718f, -4777.87354f, 5.247253f), new Vector3(0f, -0f, 102.235321f), 95324, 0, new Vector3(0.739999652f, 2.709998f, 0.170000046f), new Vector3(0f, 0f, 0f)));
				missionOjects2.Entities.Add(new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_gold_stack_01a"), 140059, 4114893066u, new Vector3(3071.96826f, -4779.35059f, 6.047252f), new Vector3(0f, -0f, 102.235321f), 95324, 0, new Vector3(-0.7799996f, 2.669998f, 0.969999433f), new Vector3(0f, 0f, 0f)));
				missionOjects2.Entities.Add(new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_gold_stack_01a"), 729863, 4114893066u, new Vector3(3071.6377f, -4777.826f, 6.05725241f), new Vector3(0f, -0f, 102.235321f), 95324, 0, new Vector3(0.7799996f, 2.669998f, 0.9799994f), new Vector3(0f, 0f, 0f)));
				missionOjects2.CreateEntites();
				foreach (ObjectPlacementMap entity2 in missionOjects2.Entities)
				{
					ATTACH_ENTITY_TO_ENTITY(entity2.Entity, entity.Entity, -1, entity2.EntityToAttachOffset.X, entity2.EntityToAttachOffset.Y, entity2.EntityToAttachOffset.Z, entity2.EntityToAttachRotation.X, entity2.EntityToAttachRotation.Y, entity2.EntityToAttachRotation.Z, false, false, true, false, 2, true);
				}
				missionOjects2.Entities[0].Entity.Detach();
				missionOjects2.Entities[1].Entity.Detach();
				missionOjects2.Entities[2].Entity.Detach();
				missionOjects2.Entities[3].Entity.Detach();
				AircraftCarrierSpawnLoot.Add(missionOjects2);
				AircraftCarrierSpawnLoot.Add(missionOjects2);
				ManageRobberyAnims.AddPropAsLoot((Prop)missionOjects2.Entities[2].Entity);
				ManageRobberyAnims.AddPropAsLoot((Prop)missionOjects2.Entities[3].Entity);
				Containers.Add(new Container((Prop)entity.Entity));
				entity.Entity.AddBlip();
				entity.Entity.AttachedBlip.Sprite = BlipSprite.ShippingContainer;
				entity.Entity.AttachedBlip.Color = BlipColor.White;
				entity.Entity.AttachedBlip.Alpha = 255;
				entity.Entity.AttachedBlip.Name = "Robbable Container";
				entity.Entity.AttachedBlip.Alpha = 0;
			}
			foreach (ObjectPlacementMap entity3 in AircraftCarrierLoot.Entities)
			{
				Script.Wait(1);
				int num2 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100);
				if (num2 < 75)
				{
					entity3.CreateObjatCoords();
					if (entity3.Entity != null)
					{
						entity3.Entity.IsPositionFrozen = true;
						ManageRobberyAnims.AddPropAsLoot((Prop)entity3.Entity);
					}
				}
			}
			if (TargetBlip == null)
			{
				TargetBlip = GlobalHelper.CreateBlip(AircraftCarrierLoc);
				TargetBlip.Sprite = BlipSprite.Yacht;
				TargetBlip.Color = BlipColor.Yellow;
				TargetBlip.Name = "Luxington Aircraft Carrier";
				TargetBlip.IsShortRange = true;
			}
			if (TargetBlipArea == null)
			{
				TargetBlipArea = GlobalHelper.CreateBlip(AircraftCarrierLoc, 100f);
				TargetBlipArea.Color = BlipColor.Yellow;
				TargetBlipArea.Name = "Luxington Aircraft Carrier";
				TargetBlipArea.IsShortRange = true;
				TargetBlipArea.Alpha = 144;
			}
			foreach (Blip intelMarker in IntelMarkers)
			{
				if (intelMarker != null)
				{
					intelMarker.Delete();
				}
			}
			IntelMarkers.Clear();
			foreach (Blip item in IntelMarkersRadiusArea)
			{
				if (item != null)
				{
					item.Delete();
				}
			}
			IntelMarkersRadiusArea.Clear();
			Blip blip = GlobalHelper.CreateBlip(SamIntelLoc);
			blip.Sprite = BlipSprite.HeistSetup;
			blip.Color = BlipColor.Green;
			blip.IsShortRange = true;
			blip.Scale = 0.5f;
			blip.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip);
			blip = GlobalHelper.CreateBlip(SamIntelLoc, 100f);
			blip.Name = "Possible Intel/Asset";
			blip.Color = BlipColor.Green;
			blip.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip);
			blip = GlobalHelper.CreateBlip(CargoshipIntelLoc);
			blip.Sprite = BlipSprite.HeistSetup;
			blip.Color = BlipColor.Green;
			blip.IsShortRange = true;
			blip.Scale = 0.5f;
			blip.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip);
			blip = GlobalHelper.CreateBlip(CargoshipIntelLoc, 100f);
			blip.Name = "Possible Intel/Asset";
			blip.Color = BlipColor.Green;
			blip.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip);
			blip = GlobalHelper.CreateBlip(KeycardIntelLoc);
			blip.Sprite = BlipSprite.HeistSetup;
			blip.Color = BlipColor.Green;
			blip.IsShortRange = true;
			blip.Scale = 0.5f;
			blip.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip);
			blip = GlobalHelper.CreateBlip(KeycardIntelLoc, 100f);
			blip.Name = "Possible Intel/Asset";
			blip.Color = BlipColor.Green;
			blip.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip);
			blip = GlobalHelper.CreateBlip(MerryweatherFacilityEnter);
			blip.Sprite = BlipSprite.HeistSetup;
			blip.Color = BlipColor.Green;
			blip.IsShortRange = true;
			blip.Scale = 0.5f;
			blip.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip);
			blip = GlobalHelper.CreateBlip(MerryweatherFacilityEnter, 100f);
			blip.Name = "Possible Intel/Asset";
			blip.Color = BlipColor.Green;
			blip.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip);
			blip = GlobalHelper.CreateBlip(MerryweatherGarageEnter);
			blip.Sprite = BlipSprite.HeistSetup;
			blip.Color = BlipColor.Green;
			blip.IsShortRange = true;
			blip.Scale = 0.5f;
			blip.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip);
			blip = GlobalHelper.CreateBlip(MerryweatherGarageEnter, 100f);
			blip.Name = "Possible Intel/Asset";
			blip.Color = BlipColor.Green;
			blip.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip);
			blip = GlobalHelper.CreateBlip(TunerGarageEnter);
			blip.Sprite = BlipSprite.HeistSetup;
			blip.Color = BlipColor.Green;
			blip.IsShortRange = true;
			blip.Scale = 0.5f;
			blip.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip);
			blip = GlobalHelper.CreateBlip(TunerGarageEnter, 100f);
			blip.Name = "Possible Intel/Asset";
			blip.Color = BlipColor.Green;
			blip.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip);
			KeycardObjects.CreateEntites();
			int index = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, SamJammerObjects.Entities.Count);
			SamJammerObjects.Entities[index].CreateObjatCoords();
			if (SamJammerObjects.Entities[index].Entity != null)
			{
				SamJammer = (Prop)SamJammerObjects.Entities[index].Entity;
			}
			index = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, GrinderObjects.Entities.Count);
			GrinderObjects.Entities[index].CreateObjatCoords();
			if (GrinderObjects.Entities[index].Entity != null)
			{
				Grinder = (Prop)SamJammerObjects.Entities[index].Entity;
			}
			KeycardObjects.CreateEntites();
			Keycard = (Prop)KeycardObjects.Entities[0].Entity;
			Function.Call(Hash.BUSYSPINNER_OFF);
			CurrentHeistStage = HeistStage.Scout;
			TheLuxingtonRaidHeistOn = true;
		}

		public Prop propcreate(Vector3 pos, Vector3 rot, string name, bool isfrozen, bool hascollision)
		{
			Model model = new Model(name);
			model.Request(10000);
			Prop prop = null;
			if (model.IsValid && model.IsInCdImage)
			{
				while (!model.IsLoaded)
				{
					Script.Wait(50);
				}
				prop = GlobalHelper.CreateProp(model, pos, rot, false, false);
				prop.Position = pos;
				prop.Rotation = rot;
				prop.IsPositionFrozen = isfrozen;
				prop.IsCollisionEnabled = hascollision;
			}
			return prop;
		}

		private void AddDuffel()
		{
			if (Game.Player.Character.Model == PedHash.Franklin)
			{
				Ped character = Game.Player.Character;
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[5] { character, 9, 1, 0, 2 });
			}
			if (Game.Player.Character.Model == PedHash.Michael)
			{
				Ped character2 = Game.Player.Character;
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[5] { character2, 9, 1, 0, 2 });
			}
			if (Game.Player.Character.Model == PedHash.Trevor)
			{
				Ped character3 = Game.Player.Character;
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[5] { character3, 9, 1, 0, 2 });
			}
			if (Game.Player.Character.Model == PedHash.FreemodeMale01)
			{
				Ped character4 = Game.Player.Character;
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[5] { character4, 5, 45, 0, 2 });
			}
			if (Game.Player.Character.Model == PedHash.FreemodeFemale01)
			{
				Ped character5 = Game.Player.Character;
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[5] { character5, 5, 45, 0, 2 });
			}
		}

		private void disablewalkingvoid()
		{
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 32, true);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 33, true);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 34, true);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 35, true);
			Game.DisableControlThisFrame(Control.MoveUpOnly);
			Game.DisableControlThisFrame(Control.MoveDownOnly);
			Game.DisableControlThisFrame(Control.MoveLeftOnly);
			Game.DisableControlThisFrame(Control.MoveRightOnly);
		}

		private void DelDuffel()
		{
			if (Game.Player.Character.Model == PedHash.Franklin)
			{
				Ped character = Game.Player.Character;
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[5] { character, 9, 0, 0, 2 });
			}
			if (Game.Player.Character.Model == PedHash.Michael)
			{
				Ped character2 = Game.Player.Character;
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[5] { character2, 9, 0, 0, 2 });
			}
			if (Game.Player.Character.Model == PedHash.Trevor)
			{
				Ped character3 = Game.Player.Character;
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[5] { character3, 9, 0, 0, 2 });
			}
			if (Game.Player.Character.Model == PedHash.FreemodeMale01)
			{
				Ped character4 = Game.Player.Character;
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[5] { character4, 5, 0, 0, 2 });
			}
			if (Game.Player.Character.Model == PedHash.FreemodeFemale01)
			{
				Ped character5 = Game.Player.Character;
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[5] { character5, 5, 0, 0, 2 });
			}
		}

		public static void CompleteHeist()
		{
			Main.TotalCrewEarned += 0f;
			Main.TotalPlayerEarned += CurrentPay;
			XP.CompleteHeist_TheLexingtonRaid++;
			Main.SaveCuts();
			Class1.SetNoLongerInHeist();
			MissionScreen.SetPass("The Luxignton Raid Heist", CurrentPay, 0, "The Player has died", 2, 0f, 5);
			int num = Game.GameTime + 30000;
			while (HKHModHelperNew.Class1.HeistPassedScreen != -1 && Game.GameTime <= num)
			{
				Script.Wait(0);
			}
			Script.Wait(2000);
			XP.AddXP_Percent(XP.CompleteHeist_XPPercent_TheLexingtonRaid);
			Game.Player.Money += CurrentPay;
			EndHeist();
			CurrentPay = 0;
			LocalTake = 0;
			EndHeist();
			TheLuxingtonRaidHeistOn = false;
		}

		public static void FailHeist(string reason)
		{
			Class1.SetNoLongerInHeist();
			HKHModHelperNew.Class1.scaleformDescription = "The Luxignton Raid Heist was failed " + reason;
			HKHModHelperNew.Class1.scaleformTitle = "The Luxignton Raid Heist was Failed";
			HKHModHelperNew.Class1.ScaleformColor = HKHModHelperNew.Class1.ScaleformColour.Red;
			HKHModHelperNew.Class1.ShowScaleform = true;
			HKHModHelperNew.Class1.Passed_Index = 0;
			EndHeist();
			TheLuxingtonRaidHeistOn = false;
		}

		private void OnTick(object sender, EventArgs e)
		{
			if (!TheLuxingtonRaidHeistOn)
			{
				return;
			}
			if (TheLuxingtonRaidHeistOn)
			{
				switch (CurrentHeistStage)
				{
				case HeistStage.C4:
				{
					if (MerryweatherFacilityGuards.Count <= 1 || CurrentActiveGroup != ActiveGroup.LexigntonRaid_MerryweatherFacility)
					{
						break;
					}
					Vector3 position2 = Game.Player.Character.Position;
					CurrentHeistStage = HeistStage.C4;
					Prop prop2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position2.X, position2.Y, position2.Z, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 0, 0, 0);
					if (prop2 != null)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, prop2.Position, "<", 30f))
						{
							World.DrawMarker(MarkerType.Chevron1, prop2.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), Vector3.Zero, new Vector3(0f, 180f, 0f), new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 255, 0), true, true);
						}
						if (prop2.Exists())
						{
							if (prop2.AttachedBlip == null)
							{
								prop2.AddBlip();
								prop2.AttachedBlip.Sprite = BlipSprite.Bomb;
								prop2.AttachedBlip.Name = "C4 Crate";
								prop2.AttachedBlip.Color = BlipColor.Green;
								prop2.AttachedBlip.IsShortRange = true;
								prop2.AttachedBlip.IsFlashing = true;
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, prop2.Position, "<", 2.5f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Pickup C4");
								if (Game.IsControlJustPressed(Control.Context))
								{
									IntelMarkers[2].Alpha = 0;
									IntelMarkersRadiusArea[2].Alpha = 0;
									C4ChargesCollected += 3;
									if (prop2.AttachedBlip != null)
									{
										prop2.AttachedBlip.Alpha = 0;
									}
									EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(prop2, 0));
									TextNotification("CHAR_HUMANDEFAULT", "~y~The Luxington Raid ~w~", "Intel Found", "You have found the C4 to blow open Containers on the Luxignton Aircraft Carrier");
									GotC4 = true;
								}
							}
						}
					}
					SmartGuards.GetCurrentGroupSettings().ShowCone = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipRadius = 100f;
					SmartGuards.GetCurrentGroupSettings().Radius = 80f;
					SmartGuards.GetCurrentGroupSettings().SeeingAngle = 35f;
					SmartGuards.GetCurrentGroupSettings().AllowInvestigate = true;
					SmartGuards.GetCurrentGroupSettings().InvestigateDist = 40f;
					SmartGuards.GetCurrentGroupSettings().HitRadius = 30f;
					SmartGuards.GetCurrentGroupSettings().ArrestInsteadOfShoot = false;
					SmartGuards.GetCurrentGroupSettings().UseShowBlipWhenSameHeightWithPlayer = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightAdd = 6f;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightMinus = 1.5f;
					SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
					break;
				}
				case HeistStage.CargoshipTrojanVehicle:
					if (CargoshipGuards.Count > 1 && CurrentActiveGroup == ActiveGroup.LexigntonRaid_MerryweatherCargoship)
					{
						SmartGuards.GetCurrentGroupSettings().ShowCone = true;
						SmartGuards.GetCurrentGroupSettings().ShowBlipRadius = 100f;
						SmartGuards.GetCurrentGroupSettings().Radius = 100f;
						SmartGuards.GetCurrentGroupSettings().SeeingAngle = 25f;
						SmartGuards.GetCurrentGroupSettings().AllowInvestigate = true;
						SmartGuards.GetCurrentGroupSettings().InvestigateDist = 40f;
						SmartGuards.GetCurrentGroupSettings().HitRadius = 30f;
						SmartGuards.GetCurrentGroupSettings().ArrestInsteadOfShoot = false;
						SmartGuards.GetCurrentGroupSettings().UseShowBlipWhenSameHeightWithPlayer = true;
						SmartGuards.GetCurrentGroupSettings().ShowBlipHeightAdd = 10f;
						SmartGuards.GetCurrentGroupSettings().ShowBlipHeightMinus = 10f;
						SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
					}
					break;
				case HeistStage.Grinder:
					if (TunerGarageGuards.Count <= 1 || CurrentActiveGroup != ActiveGroup.LexigntonRaid_TunerGarage)
					{
						break;
					}
					CurrentHeistStage = HeistStage.Grinder;
					if (Grinder != null && !GotGrinder)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, Grinder.Position, "<", 30f))
						{
							World.DrawMarker(MarkerType.Chevron1, Grinder.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), Vector3.Zero, new Vector3(0f, 180f, 0f), new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 255, 0), true, true);
						}
						if (Grinder.AttachedBlip == null)
						{
							Grinder.AddBlip();
							Grinder.AttachedBlip.Sprite = BlipSprite.BoltCutters;
							Grinder.AttachedBlip.Name = "Grinder";
							Grinder.AttachedBlip.Color = BlipColor.Green;
							Grinder.AttachedBlip.IsShortRange = true;
							Grinder.AttachedBlip.IsFlashing = true;
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, Grinder.Position, "<", 2.5f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Pickup Grinder");
							if (Game.IsControlJustPressed(Control.Context))
							{
								GotGrinder = true;
								IntelMarkers[5].Alpha = 0;
								IntelMarkersRadiusArea[5].Alpha = 0;
								if (Grinder.AttachedBlip != null)
								{
									Grinder.AttachedBlip.Alpha = 0;
								}
								EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(Grinder, 0));
								TextNotification("CHAR_HUMANDEFAULT", "~y~The Luxington Raid~w~", "Intel Found", "You have found the Grinder to saw open Containers on the Luxington Aircraft Carrier");
							}
						}
					}
					SmartGuards.GetCurrentGroupSettings().ShowCone = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipRadius = 100f;
					SmartGuards.GetCurrentGroupSettings().Radius = 50f;
					SmartGuards.GetCurrentGroupSettings().SeeingAngle = 30f;
					SmartGuards.GetCurrentGroupSettings().AllowInvestigate = true;
					SmartGuards.GetCurrentGroupSettings().InvestigateDist = 40f;
					SmartGuards.GetCurrentGroupSettings().HitRadius = 30f;
					SmartGuards.GetCurrentGroupSettings().ArrestInsteadOfShoot = false;
					SmartGuards.GetCurrentGroupSettings().UseShowBlipWhenSameHeightWithPlayer = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightAdd = 6f;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightMinus = 1.5f;
					SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
					break;
				case HeistStage.Thermite:
				{
					if (MerryweatherGarageGuards.Count <= 1 || CurrentActiveGroup != ActiveGroup.LexigntonRaid_MerryweatherGarage)
					{
						break;
					}
					Vector3 position = Game.Player.Character.Position;
					CurrentHeistStage = HeistStage.Thermite;
					Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 0, 0, 0);
					if (prop != null)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, prop.Position, "<", 30f))
						{
							World.DrawMarker(MarkerType.Chevron1, prop.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), Vector3.Zero, new Vector3(0f, 180f, 0f), new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 255, 0), true, true);
						}
						if (prop.Exists())
						{
							if (prop.AttachedBlip == null)
							{
								prop.AddBlip();
								prop.AttachedBlip.Sprite = BlipSprite.Bomb;
								prop.AttachedBlip.Name = "Thermal Charges";
								prop.AttachedBlip.Color = BlipColor.Green;
								prop.AttachedBlip.IsShortRange = true;
								prop.AttachedBlip.IsFlashing = true;
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, prop.Position, "<", 2.5f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Pickup Thermal Charges");
								if (Game.IsControlJustPressed(Control.Context))
								{
									IntelMarkers[3].Alpha = 0;
									IntelMarkersRadiusArea[3].Alpha = 0;
									ThermiteChargesCollected++;
									if (prop.AttachedBlip != null)
									{
										prop.AttachedBlip.Alpha = 0;
									}
									EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(prop, 0));
									TextNotification("CHAR_HUMANDEFAULT", "~y~The Luxington Raid ~w~", "Intel Found", "You have found the Thermite Charges to open Containers on the Luxignton Aircraft Carrier");
									GotThermite = true;
								}
							}
						}
					}
					SmartGuards.GetCurrentGroupSettings().ShowCone = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipRadius = 100f;
					SmartGuards.GetCurrentGroupSettings().Radius = 80f;
					SmartGuards.GetCurrentGroupSettings().SeeingAngle = 35f;
					SmartGuards.GetCurrentGroupSettings().AllowInvestigate = true;
					SmartGuards.GetCurrentGroupSettings().InvestigateDist = 40f;
					SmartGuards.GetCurrentGroupSettings().HitRadius = 30f;
					SmartGuards.GetCurrentGroupSettings().ArrestInsteadOfShoot = false;
					SmartGuards.GetCurrentGroupSettings().UseShowBlipWhenSameHeightWithPlayer = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightAdd = 6f;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightMinus = 1.5f;
					SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
					break;
				}
				}
				if (CurrentPay >= PercentNeedTake && GetDistance_VDIST2(Game.Player.Character.Position, AircraftCarrierLoc, ">", 500f))
				{
					CompleteHeist();
				}
				if (CurrentHeistStage == HeistStage.Scout)
				{
					Screen.ShowSubtitle("Approach the ~y~USS Luxignton~w~ or Scout for more ~g~Assets/Intel~w~");
				}
				if (CurrentHeistStage == HeistStage.C4)
				{
					Screen.ShowSubtitle("Find and steal a ~g~C4~w~ to open the ~y~USS Luxignton's Containers~w~");
				}
				if (CurrentHeistStage == HeistStage.CargoshipTrojanVehicle)
				{
					Screen.ShowSubtitle("Steal the ~b~Raiju~w~, ~b~Patrol Boat~w~ or ~b~Valkyrie~w~ to Approach the ~y~USS Luxignton~w~ without alearting the ~r~SAMs");
				}
				if (CurrentHeistStage == HeistStage.Grinder)
				{
					Screen.ShowSubtitle("Find and steal a ~g~Grinder~w~ to open the ~y~USS Luxignton's Small Containers~w~");
				}
				if (CurrentHeistStage == HeistStage.Keycard)
				{
					Screen.ShowSubtitle("Find and steal a ~g~Keycard~w~ to open the ~y~USS Luxignton's Large Containers~w~");
				}
				if (CurrentHeistStage == HeistStage.LootingCarrier)
				{
					if (CurrentPay >= PercentNeedTake)
					{
						Screen.ShowSubtitle("Leave the ~y~USS Luxignton~w~ or keep stealing ~d~HDDs/Silver/Gold~w~ from the ~y~USS Luxignton~w~");
					}
					else
					{
						Screen.ShowSubtitle("Find and steal a ~y~$" + PercentNeedTake.ToString("N") + "~w~ of ~d~HDDs/Silver/Gold~w~ from the ~y~USS Luxignton~w~");
					}
				}
				if (CurrentHeistStage == HeistStage.SamJammer)
				{
					Screen.ShowSubtitle("Find and steal a ~g~Sam Jammer~w~ to Approach the ~y~USS Luxignton~w~ by Air, without alearting the ~r~SAMs");
				}
				if (CurrentHeistStage == HeistStage.Thermite)
				{
					Screen.ShowSubtitle("Find and steal a ~g~Thermite~w~ to open the ~y~USS Luxignton's Small Containers~w~");
				}
				if (CurrentHeistStage == HeistStage.InRaiju)
				{
					Screen.ShowSubtitle("Fly the ~b~Raiju~w~ to the ~y~USS Luxignton~w~");
				}
				if (CurrentHeistStage == HeistStage.InPTBoat)
				{
					Screen.ShowSubtitle("Drive the ~b~Patrol Boat~w~ to the ~y~USS Luxignton~w~");
				}
				if (CurrentHeistStage == HeistStage.InValkyrie)
				{
					Screen.ShowSubtitle("Fly the ~b~Valkyrie~w~ to the ~y~USS Luxignton~w~");
				}
				if (CurrentHeistStage == HeistStage.ShootPtBoatCable)
				{
					Screen.ShowSubtitle("Shoot the ~r~Cable Winch~w~ to free the ~b~Patrol Boat~w~");
				}
				if (!GotSamJammer && !UsingRaiju)
				{
					foreach (SamSite samSite in SamSites)
					{
						if (samSite.Sam != null)
						{
							if (samSite.Sam.AttachedBlip == null)
							{
								samSite.Sam.AddBlip();
								if (samSite.Sam.AttachedBlip != null)
								{
									samSite.Sam.AttachedBlip.Sprite = BlipSprite.SAMTurret;
									samSite.Sam.AttachedBlip.Color = BlipColor.Red;
									samSite.Sam.AttachedBlip.Name = "Surface to Air Missile Site";
									samSite.Sam.AttachedBlip.IsShortRange = true;
								}
							}
							if (samSite.Sam.AttachedBlip != null)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, samSite.Sam.Position, "<", 300f))
								{
									if (!samSite.IsDead)
									{
										samSite.Sam.AttachedBlip.Alpha = 255;
									}
									if (samSite.IsDead)
									{
										samSite.Sam.AttachedBlip.Alpha = 0;
									}
								}
								else
								{
									samSite.Sam.AttachedBlip.Alpha = 0;
								}
							}
						}
						if (!GetDistance_VDIST2(Game.Player.Character.Position, samSite.Sam.Position, "<", 350f))
						{
							continue;
						}
						if (samSite.Sam.Health <= 350 && samSite.Sam.AttachedBlip != null)
						{
							samSite.IsDead = true;
							samSite.Sam.AttachedBlip.Alpha = 0;
						}
						if (!(Game.Player.Character.CurrentVehicle != null))
						{
							continue;
						}
						if (!samSite.IsDead)
						{
							if (!(Game.Player.Character.CurrentVehicle != null) || !(Game.Player.Character.CurrentVehicle != Raiju) || !(Game.Player.Character.CurrentVehicle != null))
							{
								continue;
							}
							if ((Game.Player.Character.CurrentVehicle.Position.Z > samSite.Sam.Position.Z - 5f || Function.Call<bool>((Hash)16881535976618417939uL, Game.Player.Character.CurrentVehicle)) && (Game.Player.Character.CurrentVehicle.Model.IsPlane || Game.Player.Character.CurrentVehicle.Model.IsHelicopter) && samSite.fireDelay < Game.GameTime)
							{
								samSite.roundsFired++;
								if (samSite.roundsFired < 6)
								{
									samSite.fireDelay = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 2500, 7500);
								}
								if (samSite.roundsFired >= 6)
								{
									samSite.roundsFired = 0;
									samSite.fireDelay = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 10000, 30000);
								}
								Vector3 offsetPosition = samSite.Sam.GetOffsetPosition(new Vector3(0f, 0f, 10f));
								Vector3 position3 = Game.Player.Character.CurrentVehicle.Position;
								Function.Call(Hash.SHOOT_SINGLE_BULLET_BETWEEN_COORDS_IGNORE_ENTITY_NEW, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, position3.X, position3.Y, position3.Z, 1000, false, Function.Call<int>(Hash.GET_HASH_KEY, "VEHICLE_WEAPON_PLANE_ROCKET"), 0, true, true, -1f, 0, true, true, Game.Player.Character.CurrentVehicle, false, 0, 0, 0);
							}
							Function.Call(Hash.SET_VEHICLE_WEAPON_CAN_TARGET_OBJECTS, Game.Player.Character.CurrentVehicle, true);
						}
						else
						{
							Function.Call(Hash.SET_OBJECT_TARGETTABLE, samSite.Sam, false);
							Function.Call(Hash.SET_ENTITY_IS_TARGET_PRIORITY, samSite.Sam, false, 0f);
						}
					}
				}
				Vector3 position4 = Game.Player.Character.Position;
				if (ManageRobberyAnims.CurrentType != ManageRobberyAnims.LootType.GlassCut_Cabinet)
				{
					CurrentPay = ManageRobberyAnims.CurrentPay;
					if (LocalTake != ManageRobberyAnims.CurrentPay)
					{
						if (LocalTake < ManageRobberyAnims.CurrentPay)
						{
							LocalTake += ManageRobberyAnims.CurrentPay / 200;
						}
						if (LocalTake > ManageRobberyAnims.CurrentPay)
						{
							LocalTake = ManageRobberyAnims.CurrentPay;
						}
					}
					ShowTextInfoBar("CurrentPay ", CurrentPay, 1, new Vector2(-0.05f, 0f), new Vector2(-0.05f, 0f), true);
					if (ThermiteChargesCollected >= 1 && C4ChargesCollected == 0)
					{
						ShowTextInfoBar("Thermal Charges ", ThermiteChargesCollected, 2, new Vector2(-0.05f, 0f), new Vector2(-0.05f, 0f), false);
					}
					if (ThermiteChargesCollected == 0 && C4ChargesCollected >= 1)
					{
						ShowTextInfoBar("C4 Charges ", C4ChargesCollected, 2, new Vector2(-0.05f, 0f), new Vector2(-0.05f, 0f), false);
					}
					if (ThermiteChargesCollected >= 1 && C4ChargesCollected >= 1)
					{
						ShowTextInfoBar("Thermal Charges ", ThermiteChargesCollected, 2, new Vector2(-0.05f, 0f), new Vector2(-0.05f, 0f), false);
						ShowTextInfoBar("C4 Charges ", C4ChargesCollected, 3, new Vector2(-0.05f, 0f), new Vector2(-0.05f, 0f), false);
					}
				}
				if (SpawnGroupsUse.Count > 1 && CurrentActiveGroup == ActiveGroup.LexigntonRaid_Carrier)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, AircraftCarrierLoc, "<", 100f))
					{
						CurrentHeistStage = HeistStage.LootingCarrier;
					}
					SmartGuards.GetCurrentGroupSettings().ShowCone = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipRadius = 100f;
					SmartGuards.GetCurrentGroupSettings().Radius = 80f;
					SmartGuards.GetCurrentGroupSettings().SeeingAngle = 35f;
					SmartGuards.GetCurrentGroupSettings().AllowInvestigate = true;
					SmartGuards.GetCurrentGroupSettings().InvestigateDist = 40f;
					SmartGuards.GetCurrentGroupSettings().HitRadius = 30f;
					SmartGuards.GetCurrentGroupSettings().ArrestInsteadOfShoot = false;
					SmartGuards.GetCurrentGroupSettings().UseShowBlipWhenSameHeightWithPlayer = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightAdd = 6f;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightMinus = 1.5f;
					SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, AircraftCarrierLoc, "<", 300f))
				{
					CurrentHeistStage = HeistStage.LootingCarrier;
					foreach (Container container2 in Containers)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, CurrentContainer, ">", 3f) && CurrentHeistStage == HeistStage.CannotOpenContainer)
						{
							CurrentHeistStage = HeistStage.LootingCarrier;
						}
						if (container2.containerswitch >= 0 && container2.plantBombStage >= 0)
						{
							if (container2.Container1 != null)
							{
								if (GetDistance_VDIST2(container2.Container1.Position, Game.Player.Character.Position, "<", 30f))
								{
									if (container2.Container1.AttachedBlip != null)
									{
										container2.Container1.AttachedBlip.Alpha = 255;
									}
								}
								else if (container2.Container1.AttachedBlip != null)
								{
									container2.Container1.AttachedBlip.Alpha = 0;
								}
							}
						}
						else if (container2.Container1.AttachedBlip != null)
						{
							container2.Container1.AttachedBlip.Alpha = 0;
						}
						if (container2.Container1.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_container_01a"))
						{
							if (container2.Lock == null)
							{
								container2.Lock = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_lock_01a"), container2.Container1.GetOffsetPosition(new Vector3(0f, -1.8f, 1.2f)), new Vector3(0f, 0f, container2.Container1.Rotation.Z), false, false);
							}
							container2.Container1.IsPositionFrozen = true;
							Vector3 offsetPosition2 = container2.Container1.GetOffsetPosition(new Vector3(0f, -1.8f, 1.2f));
							if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition2, "<", 3f))
							{
								if (GetDistance_VDIST2(CurrentContainer, offsetPosition2, ">", 3f))
								{
									CurrentContainer = offsetPosition2;
								}
								else if (C4ChargesCollected == 0 && !GotGrinder && ThermiteChargesCollected == 0)
								{
									CurrentHeistStage = HeistStage.CannotOpenContainer;
								}
							}
							if (GotGrinder)
							{
								switch (container2.containerswitch)
								{
								case 0:
								{
									if (!(Game.Player.Character.Position.DistanceTo(new Vector3(container2.Lock.Position.X, container2.Lock.Position.Y, container2.Lock.Position.Z)) < 2f))
									{
										break;
									}
									DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to break lock", 1);
									if (!Game.IsControlJustPressed(Control.Context))
									{
										break;
									}
									Ped character = Game.Player.Character;
									if (Game.Player.Character.Model == PedHash.Trevor)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
									}
									if (Game.Player.Character.Model == PedHash.Michael)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
									}
									if (Game.Player.Character.Model == PedHash.Franklin)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
									}
									Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_CM2022/CM2022_GENERIC_01");
									Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_TUNER/DLC_Tuner_Generic");
									Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_CM2022/CM2022_GENERIC_01", false);
									Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_TUNER/DLC_Tuner_Generic", false);
									Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", "action", container2.Container1.Position.X, container2.Container1.Position.Y, container2.Container1.Position.Z, 0f, 0f, container2.Container1.Rotation.Z, 0f, 2);
									Vector3 vector2 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", "action", container2.Container1.Position.X, container2.Container1.Position.Y, container2.Container1.Position.Z, 0f, 0f, container2.Container1.Rotation.Z, 0f, 2);
									Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@");
									Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_CM2022/CM2022_GENERIC_01");
									Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_TUNER/DLC_Tuner_Generic");
									Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_CM2022/CM2022_GENERIC_01", false);
									Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_TUNER/DLC_Tuner_Generic", false);
									Model model = new Model("ch_p_m_bag_var04_arm_s");
									model.Request(10000);
									if (model.IsValid && model.IsInCdImage)
									{
										while (!model.IsLoaded)
										{
											Script.Wait(50);
										}
										container2.Bag = GlobalHelper.CreateProp(model, Game.Player.Character.Position, false, false);
										container2.Bag.IsCollisionEnabled = false;
										container2.containerswitch = 10;
									}
									Model model2 = new Model("tr_prop_tr_grinder_01a");
									model2.Request(100);
									if (model2.IsValid && model2.IsInCdImage)
									{
										while (!model2.IsLoaded)
										{
											Script.Wait(50);
										}
										container2.Saw = GlobalHelper.CreateProp(model2, Game.Player.Character.Position, false, false);
										container2.Saw.IsCollisionEnabled = false;
										container2.containerswitch = 10;
									}
									Model model3 = new Model("tr_prop_tr_cont_coll_01a");
									model3.Request(100);
									if (model3.IsValid && model3.IsInCdImage)
									{
										while (!model3.IsLoaded)
										{
											Script.Wait(50);
										}
										container2.FakeContainer = GlobalHelper.CreateProp(model3, container2.Container1.Position, false, false);
										container2.FakeContainer.Rotation = new Vector3(0f, 0f, container2.Container1.Rotation.X);
										container2.FakeContainer.IsCollisionEnabled = false;
										container2.containerswitch = 10;
									}
									break;
								}
								case 10:
									Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@");
									while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@"))
									{
										Script.Wait(0);
									}
									Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_CM2022/CM2022_GENERIC_01");
									Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_TUNER/DLC_Tuner_Generic");
									Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_CM2022/CM2022_GENERIC_01", false);
									Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_TUNER/DLC_Tuner_Generic", false);
									Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@");
									container2.SyncScene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, container2.Container1.Position.X, container2.Container1.Position.Y, container2.Container1.Position.Z, 0f, 0f, container2.Container1.Rotation.Z, 2);
									container2.SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, container2.Container1.Position.X, container2.Container1.Position.Y, container2.Container1.Position.Z, 0f, 0f, container2.Container1.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, container2.SyncScene2, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", "action", 4f, -15f, 3341, 16, 1148846080, 0);
									Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container2.Bag, container2.SyncScene2, "action_bag", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container2.Lock, container2.SyncScene, "action_lock", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container2.Saw, container2.SyncScene2, "action_angle_grinder", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container2.FakeContainer, container2.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container2.Container1, container2.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
									container2.ParticleTimer = Game.GameTime + 4000;
									Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_tn_tr");
									container2.containerswitch = 20;
									break;
								case 20:
									if (Game.GameTime > container2.ParticleTimer)
									{
										Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_tn_tr");
										Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_tn_tr");
										container2.ParticleFx = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_tn_tr_angle_grinder_sparks", container2.Saw, 0f, 0.25f, 0f, 0f, 0f, 0f, 1f, false, false, false);
										container2.ParticleTimer = Game.GameTime + 1000;
										container2.containerswitch = 21;
									}
									break;
								case 21:
									if (Game.GameTime > container2.ParticleTimer)
									{
										Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, container2.ParticleFx);
										container2.containerswitch = 30;
									}
									break;
								case 30:
									if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, container2.SyncScene2) > 0.99f)
									{
										container2.containerswitch = -100;
										if (Game.Player.Character.Model == PedHash.Trevor)
										{
											Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 1, 0, 1);
										}
										if (Game.Player.Character.Model == PedHash.Michael)
										{
											Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 2, 0, 1);
										}
										if (Game.Player.Character.Model == PedHash.Franklin)
										{
											Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 6, 0, 1);
										}
										Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, container2.ParticleFx);
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, container2.SyncScene, 0f);
										container2.Bag.Delete();
										container2.Saw.Delete();
										container2.Container1.IsCollisionEnabled = false;
										container2.FakeContainer.IsCollisionEnabled = true;
										Game.Player.Character.Task.ClearAll();
									}
									break;
								case -100:
									if (container2.Container1.AttachedBlip != null)
									{
										container2.Container1.AttachedBlip.Alpha = 0;
									}
									container2.containerswitch = -1;
									break;
								}
							}
							else if (!GotGrinder && GotC4)
							{
								Vector3 offsetPosition3 = container2.Container1.GetOffsetPosition(new Vector3(0f, -1.8f, 1.5f));
								Vector3 rotation = container2.Container1.Rotation;
								switch (container2.plantBombStage)
								{
								case 0:
								{
									if (!(Game.Player.Character.Position.DistanceTo(new Vector3(offsetPosition3.X, offsetPosition3.Y, offsetPosition3.Z)) < 1.5f) || C4ChargesCollected < 1)
									{
										break;
									}
									DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to plant a ~r~bomb.", 1);
									if (!Game.IsControlJustPressed(Control.Context))
									{
										break;
									}
									C4ChargesCollected--;
									LoadDict("anim@scripted@heist@ig11_bomb_plant@male@");
									LoadDict("anim@scripted@player@mission@tun_bomb_plant@heeled@");
									Model model4 = new Model("ch_prop_ch_ld_bomb_01a");
									model4.Request(250);
									if (model4.IsValid && model4.IsInCdImage)
									{
										while (!model4.IsLoaded)
										{
											Script.Wait(50);
										}
										container2.Bomb = GlobalHelper.CreateProp(model4, Game.Player.Character.Position, false, false);
										container2.Bomb.IsCollisionEnabled = false;
										container2.plantBombStage = 10;
									}
									Model model5 = new Model("hei_p_m_bag_var22_arm_s");
									model5.Request(250);
									if (model5.IsValid && model5.IsInCdImage)
									{
										while (!model5.IsLoaded)
										{
											Script.Wait(50);
										}
										container2.Bag = GlobalHelper.CreateProp(model5, Game.Player.Character.Position, false, false);
										container2.Bag.IsCollisionEnabled = false;
										container2.plantBombStage = 10;
									}
									break;
								}
								case 10:
									LoadDict("anim@scripted@heist@ig11_bomb_plant@male@");
									LoadDict("anim@scripted@player@mission@tun_bomb_plant@heeled@");
									container2.SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition3.X, offsetPosition3.Y, offsetPosition3.Z + 0.15f, 0f, 0f, rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, container2.SyncScene, "anim@scripted@player@mission@tun_bomb_plant@heeled@", "enter", 4f, -15f, 3341, 16, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container2.Bomb, container2.SyncScene, "enter_bomb", "anim@scripted@heist@ig11_bomb_plant@male@", 1f, -1f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container2.Bag, container2.SyncScene, "enter_bag", "anim@scripted@heist@ig11_bomb_plant@male@", 1f, -1f, 0, 1148846080);
									Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
									Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
									container2.plantBombStage = 20;
									break;
								case 20:
									if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, container2.SyncScene) > 0.99f)
									{
										container2.Bag.Delete();
										Game.Player.Character.Task.ClearAll();
										container2.BombExplodeTimer = Game.GameTime + 5000;
										container2.plantBombStage = -1;
									}
									break;
								case -1:
									if (Game.GameTime >= container2.BombExplodeTimer)
									{
										World.AddExplosion(container2.Bomb.Position, ExplosionType.PipeBomb, 1f, 1f, Game.Player.Character);
										if (container2.Bomb != null)
										{
											container2.Bomb.Delete();
										}
										container2.FakeContainer = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_cont_coll_01a"), container2.Container1.Position, false, false);
										container2.FakeContainer.Rotation = new Vector3(0f, 0f, container2.Container1.Rotation.X);
										container2.FakeContainer.IsCollisionEnabled = false;
										LoadDict("anim@scripted@player@mission@tunf_train_ig1_container_p1@male@");
										Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@");
										container2.SyncScene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, container2.Container1.Position.X, container2.Container1.Position.Y, container2.Container1.Position.Z, 0f, 0f, container2.Container1.Rotation.Z, 2);
										container2.SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, container2.Container1.Position.X, container2.Container1.Position.Y, container2.Container1.Position.Z, 0f, 0f, container2.Container1.Rotation.Z, 2);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container2.FakeContainer, container2.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container2.Container1, container2.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
										if (container2.Lock != null)
										{
											container2.Lock.Delete();
										}
										if (container2.Bomb != null)
										{
											container2.Bomb.Delete();
										}
										container2.plantBombStage = -2;
									}
									break;
								case -2:
									container2.Container1.IsCollisionEnabled = false;
									container2.FakeContainer.IsCollisionEnabled = true;
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, container2.SyncScene, 1f);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, container2.SyncScene2, 1f);
									break;
								}
							}
							else if (!GotGrinder && !GotC4 && GotThermite)
							{
								Vector3 offsetPosition4 = container2.Container1.GetOffsetPosition(new Vector3(0f, -1.85f, 1.2f));
								Vector3 rotation2 = container2.Container1.Rotation;
								switch (container2.plantBombStage)
								{
								case 0:
									if (!(Game.Player.Character.Position.DistanceTo(new Vector3(offsetPosition4.X, offsetPosition4.Y, offsetPosition4.Z)) < 1.5f) || ThermiteChargesCollected < 1)
									{
										break;
									}
									DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to plant a ~r~Thermal Charge.", 1);
									if (Game.IsControlJustPressed(Control.Context))
									{
										ThermiteChargesCollected--;
										Ped character2 = Game.Player.Character;
										if (Game.Player.Character.Model == PedHash.Trevor)
										{
											Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 9, 0, 0, 1);
										}
										if (Game.Player.Character.Model == PedHash.Michael)
										{
											Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 9, 0, 0, 1);
										}
										if (Game.Player.Character.Model == PedHash.Franklin)
										{
											Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 9, 0, 0, 1);
										}
										string dict = "anim_heist@hs3f@ig13_thermal_charge@thermal_charge@male@";
										string dict2 = "anim@heists@ornate_bank@thermal_charge";
										LoadDict(dict);
										LoadDict(dict);
										LoadDict(dict2);
										LoadDict(dict2);
										if (container2.Bomb != null)
										{
											container2.Bomb.Delete();
										}
										if (container2.Bag != null)
										{
											container2.Bag.Delete();
										}
										container2.Bag = GlobalHelper.CreateProp(RequestModel("ch_p_m_bag_var02_arm_s"), offsetPosition4, new Vector3(1f, 0f, 0f), false, false);
										container2.SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition4.X, offsetPosition4.Y, offsetPosition4.Z, 0.0, 0.0, rotation2.Z, 2);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, container2.SyncScene, LoadDict("anim@heists@ornate_bank@thermal_charge"), "thermal_charge", 1f, -1f, 3341, 16, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container2.Bag, container2.SyncScene, "bag_thermal_charge", LoadDict("anim@heists@ornate_bank@thermal_charge"), 1f, -1f, 0, 1148846080);
										Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, container2.Bag);
										container2.Bomb = GlobalHelper.CreateProp(RequestModel("hei_prop_heist_thermite"), Game.Player.Character.Position, false, false);
										Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, container2.Bomb, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, -90f, 0f, 0f, 0, 0, 0, 0, 2, 1);
										Function.Call(Hash.PLAY_ENTITY_ANIM, container2.Bomb, "bomb_thermal_charge", "anim@heists@ornate_bank@thermal_charge", 1000.0, 1, 1, 1, 0, 0);
										container2.plantBombStage = 10;
									}
									break;
								case 10:
									if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, container2.SyncScene) > 0.5f)
									{
										container2.Bomb.Detach();
										container2.Bomb.IsPositionFrozen = true;
										Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ornate_heist");
										Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ornate_heist");
										container2.PTFX1 = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_heist_ornate_thermal_burn", container2.Bomb, 0f, 0f, 0f, 1f, 0, 0, 0);
										Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
										Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
										container2.PTFX2 = GlobalHelper.CreatePTFX_LoopedOnEntity("proj_flare_fuse_fp", container2.Bomb, 0f, 0f, 0f, 10f, 0, 0, 0);
										Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
										container2.plantBombStage = 20;
									}
									break;
								case 20:
									if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, container2.SyncScene) > 0.99f)
									{
										container2.Bag.Delete();
										container2.Bomb.IsPositionFrozen = true;
										Game.Player.Character.Task.ClearAll();
										container2.BombExplodeTimer = Game.GameTime + 7500;
										container2.plantBombStage = -1;
									}
									break;
								case -1:
									if (Game.GameTime >= container2.BombExplodeTimer)
									{
										Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, container2.PTFX2, false);
										Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, container2.PTFX1, false);
										Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "Gate_Lock_Break", container2.Bomb, "DLC_HEISTS_ORNATE_BANK_FINALE_SOUNDS", 0, 0);
										if (container2.Bomb != null)
										{
											container2.Bomb.Delete();
										}
										container2.FakeContainer = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_cont_coll_01a"), container2.Container1.Position, false, false);
										container2.FakeContainer.Rotation = new Vector3(0f, 0f, container2.Container1.Rotation.X);
										container2.FakeContainer.IsCollisionEnabled = false;
										LoadDict("anim@scripted@player@mission@tunf_train_ig1_container_p1@male@");
										Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@");
										container2.SyncScene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, container2.Container1.Position.X, container2.Container1.Position.Y, container2.Container1.Position.Z, 0f, 0f, container2.Container1.Rotation.Z, 2);
										container2.SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, container2.Container1.Position.X, container2.Container1.Position.Y, container2.Container1.Position.Z, 0f, 0f, container2.Container1.Rotation.Z, 2);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container2.FakeContainer, container2.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container2.Container1, container2.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
										if (container2.Lock != null)
										{
											container2.Lock.Delete();
										}
										if (container2.Bomb != null)
										{
											container2.Bomb.Delete();
										}
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, container2.SyncScene, 0.65f);
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, container2.SyncScene2, 0.65f);
										container2.plantBombStage = -2;
									}
									break;
								case -2:
									container2.Container1.IsCollisionEnabled = false;
									container2.FakeContainer.IsCollisionEnabled = true;
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, container2.SyncScene, 0.65f);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, container2.SyncScene2, 0.65f);
									break;
								}
							}
						}
						if (container2.Container1.Model.Hash != Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_01a"))
						{
							continue;
						}
						Vector3 offsetPosition5 = container2.Container1.GetOffsetPosition(new Vector3(0f, -3.5f, 1.2f));
						if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition5, "<", 3f))
						{
							if (GetDistance_VDIST2(CurrentContainer, offsetPosition5, ">", 3f))
							{
								CurrentContainer = offsetPosition5;
							}
							else if (C4ChargesCollected == 0 && !GotGrinder && ThermiteChargesCollected == 0)
							{
								CurrentHeistStage = HeistStage.CannotOpenContainer;
							}
						}
						if (GotKeycard)
						{
							switch (container2.LargeContainerAnim)
							{
							case 0:
							{
								Prop container = container2.Container1;
								Vector3 offsetPosition6 = container.GetOffsetPosition(new Vector3(0f, -3.5f, 0f));
								if (Game.Player.Character.Position.DistanceTo(offsetPosition6) < 1.5f)
								{
									Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@freemode@ig1_open_container@male@");
									DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to swipe the card");
									if (Game.IsControlJustPressed(Control.Context))
									{
										LoadDict("anim@scripted@freemode@ig1_open_container@male@");
										container2.Container1.Detach();
										Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
										Vector3 vector3 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "anim@scripted@freemode@ig1_open_container@male@", "action", container.Position.X, container.Position.Y, container.Position.Z, 0f, 0f, container.Rotation.Z, 0f, 2);
										Function.Call(argument6: Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "anim@scripted@freemode@ig1_open_container@male@", "action", container.Position.X, container.Position.Y, container.Position.Z, 0f, 0f, container.Rotation.Z, 0f, 2).Z, hash: Hash.TASK_GO_STRAIGHT_TO_COORD, argument0: Game.Player.Character, argument1: vector3.X, argument2: vector3.Y, argument3: vector3.Z, argument4: 1f, argument5: 3000, argument7: 0f);
										Function.Call(Hash.SET_PED_MOVE_RATE_OVERRIDE, Game.Player.Character.Handle, 0f);
										container2.unstucktimer = Game.GameTime + 2000;
										container2.LargeContainerAnim = 1;
									}
								}
								break;
							}
							case 1:
							{
								disablewalkingvoid();
								if (Function.Call<bool>(Hash.IS_PED_WALKING, Game.Player.Character) || Game.GameTime <= container2.unstucktimer)
								{
									break;
								}
								Model model6 = new Model("m23_1_prop_m31_swipe_card_01a");
								model6.Request(10000);
								if (model6.IsValid && model6.IsInCdImage)
								{
									while (!model6.IsLoaded)
									{
										Script.Wait(50);
									}
									container2.Swipecard = GlobalHelper.CreateProp(model6, Game.Player.Character.Position, false, false);
									container2.Swipecard.IsCollisionEnabled = false;
									container2.LargeContainerAnim = 10;
								}
								break;
							}
							case 10:
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, container2.Swipecard, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 90f, 0f, 0f, false, false, false, false, 0, true);
								Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@freemode@ig1_open_container@male@");
								container2.LargeContainerSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, container2.Container1.Position.X, container2.Container1.Position.Y, container2.Container1.Position.Z, 0f, 0f, container2.Container1.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, container2.LargeContainerSyncScene, "anim@scripted@freemode@ig1_open_container@male@", "action", 4f, -15f, 3341, 16, 1148846080, 0);
								Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container2.Container1, container2.LargeContainerSyncScene, "action_container", "anim@scripted@freemode@ig1_open_container@male@", 1f, -1f, 0, 1148846080);
								container2.LargeContainerAnim = 20;
								break;
							case 20:
								if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, container2.LargeContainerSyncScene) > 0.99f)
								{
									Function.Call(Hash.SET_PED_MOVE_RATE_OVERRIDE, Game.Player.Character.Handle, -1f);
									container2.Swipecard.Delete();
									AddDuffel();
									Game.Player.Character.Task.ClearAll();
									Prop fakeContainer = propcreate(container2.Container1.Position, container2.Container1.Rotation, "m23_1_prop_m31_container_01b", true, true);
									container2.Container1.Delete();
									container2.FakeContainer = fakeContainer;
									container2.LargeContainerAnim = 0;
								}
								break;
							}
						}
						else
						{
							if (GotKeycard || !GotC4)
							{
								continue;
							}
							Vector3 offsetPosition7 = container2.Container1.GetOffsetPosition(new Vector3(0f, -3.15f, 1.5f));
							Vector3 rotation3 = container2.Container1.Rotation;
							switch (container2.plantBombStage)
							{
							case 0:
							{
								if (!(Game.Player.Character.Position.DistanceTo(new Vector3(offsetPosition7.X, offsetPosition7.Y, offsetPosition7.Z)) < 1.5f) || C4ChargesCollected < 1)
								{
									break;
								}
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to plant a ~r~bomb.", 1);
								if (!Game.IsControlJustPressed(Control.Context))
								{
									break;
								}
								C4ChargesCollected--;
								LoadDict("anim@scripted@heist@ig11_bomb_plant@male@");
								LoadDict("anim@scripted@player@mission@tun_bomb_plant@heeled@");
								Model model7 = new Model("ch_prop_ch_ld_bomb_01a");
								model7.Request(250);
								if (model7.IsValid && model7.IsInCdImage)
								{
									while (!model7.IsLoaded)
									{
										Script.Wait(50);
									}
									container2.Bomb = GlobalHelper.CreateProp(model7, Game.Player.Character.Position, false, false);
									container2.Bomb.IsCollisionEnabled = false;
									container2.plantBombStage = 10;
								}
								Model model8 = new Model("hei_p_m_bag_var22_arm_s");
								model8.Request(250);
								if (model8.IsValid && model8.IsInCdImage)
								{
									while (!model8.IsLoaded)
									{
										Script.Wait(50);
									}
									container2.Bag = GlobalHelper.CreateProp(model8, Game.Player.Character.Position, false, false);
									container2.Bag.IsCollisionEnabled = false;
									container2.plantBombStage = 10;
								}
								break;
							}
							case 10:
								LoadDict("anim@scripted@heist@ig11_bomb_plant@male@");
								LoadDict("anim@scripted@player@mission@tun_bomb_plant@heeled@");
								container2.SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition7.X, offsetPosition7.Y, offsetPosition7.Z + 0.15f, 0f, 0f, rotation3.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, container2.SyncScene, "anim@scripted@player@mission@tun_bomb_plant@heeled@", "enter", 4f, -15f, 3341, 16, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container2.Bomb, container2.SyncScene, "enter_bomb", "anim@scripted@heist@ig11_bomb_plant@male@", 1f, -1f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container2.Bag, container2.SyncScene, "enter_bag", "anim@scripted@heist@ig11_bomb_plant@male@", 1f, -1f, 0, 1148846080);
								Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
								Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
								container2.plantBombStage = 20;
								break;
							case 20:
								if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, container2.SyncScene) > 0.99f)
								{
									container2.Bag.Delete();
									Game.Player.Character.Task.ClearAll();
									container2.BombExplodeTimer = Game.GameTime + 5000;
									container2.plantBombStage = -1;
								}
								break;
							case -1:
								if (Game.GameTime >= container2.BombExplodeTimer)
								{
									World.AddExplosion(container2.Bomb.Position, ExplosionType.PipeBomb, 1f, 1f, Game.Player.Character);
									if (container2.Bomb != null)
									{
										container2.Bomb.Delete();
									}
									container2.FakeContainer = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_cont_coll_01a"), container2.Container1.Position, false, false);
									container2.FakeContainer.Rotation = new Vector3(0f, 0f, container2.Container1.Rotation.X);
									container2.FakeContainer.IsCollisionEnabled = false;
									LoadDict("anim@scripted@player@mission@tunf_train_ig1_container_p1@male@");
									Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@");
									container2.SyncScene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, container2.Container1.Position.X, container2.Container1.Position.Y, container2.Container1.Position.Z, 0f, 0f, container2.Container1.Rotation.Z, 2);
									container2.SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, container2.Container1.Position.X, container2.Container1.Position.Y, container2.Container1.Position.Z, 0f, 0f, container2.Container1.Rotation.Z, 2);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container2.FakeContainer, container2.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container2.Container1, container2.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
									if (container2.Lock != null)
									{
										container2.Lock.Delete();
									}
									if (container2.Bomb != null)
									{
										container2.Bomb.Delete();
									}
									container2.plantBombStage = -2;
								}
								break;
							case -2:
								container2.Container1.IsCollisionEnabled = false;
								container2.FakeContainer.IsCollisionEnabled = true;
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, container2.SyncScene, 1f);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, container2.SyncScene2, 1f);
								break;
							}
						}
					}
					if (CurrentActiveGroup != ActiveGroup.LexigntonRaid_Carrier)
					{
						SmartGuards.GuardGroup[2].GroupList.Clear();
						SmartGuards.GuardGroup[2].GroupList.AddRange(CarrierGuards);
						SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Merryweather;
						CurrentActiveGroup = ActiveGroup.LexigntonRaid_Carrier;
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, AircraftCarrierLoc, ">", 300f))
					{
						if (CurrentHeistStage != HeistStage.InPTBoat || CurrentHeistStage != HeistStage.InRaiju || CurrentHeistStage != HeistStage.InValkyrie)
						{
							CurrentHeistStage = HeistStage.Scout;
						}
					}
					else
					{
						CurrentHeistStage = HeistStage.LootingCarrier;
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(3050f, -4650f, 5f), "<", 300f))
					{
						if (position4.Z >= 13.5f)
						{
							Function.Call(Hash.SET_RADAR_AS_INTERIOR_THIS_FRAME, Function.Call<int>(Hash.GET_HASH_KEY, "V_FAKEHEISTAC"), 3050f, -4650f, 0, 2);
							Function.Call(Hash.SET_RADAR_ZOOM_PRECISE, 70f);
						}
						if (position4.Z < 13.5f)
						{
							Function.Call(Hash.SET_RADAR_AS_INTERIOR_THIS_FRAME, Function.Call<int>(Hash.GET_HASH_KEY, "V_FAKEHEISTAC"), 3050f, -4650f, 0, 0);
							Function.Call(Hash.SET_RADAR_ZOOM_PRECISE, 55f);
						}
					}
				}
				else if (CurrentActiveGroup == ActiveGroup.LexigntonRaid_Carrier)
				{
					CurrentHeistStage = HeistStage.Scout;
					SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.None;
					CurrentActiveGroup = ActiveGroup.None;
				}
				if (Keycard != null && !GotKeycard)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, Keycard.Position, "<", 100f))
					{
						CurrentHeistStage = HeistStage.Keycard;
					}
					else if (CurrentHeistStage == HeistStage.Keycard)
					{
						CurrentHeistStage = HeistStage.Scout;
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, Keycard.Position, "<", 10f) && Keycard.AttachedBlip == null)
					{
						Keycard.AddBlip();
						Keycard.AttachedBlip.Sprite = BlipSprite.Document;
						Keycard.AttachedBlip.Name = "Keycard";
						Keycard.AttachedBlip.Color = BlipColor.Green;
						Keycard.AttachedBlip.IsShortRange = true;
						Keycard.AttachedBlip.IsFlashing = true;
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, Keycard.Position, "<", 2.5f))
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Merryweather Keycard");
						if (Game.IsControlJustPressed(Control.Context))
						{
							IntelMarkers[2].Alpha = 0;
							IntelMarkersRadiusArea[2].Alpha = 0;
							if (Keycard.AttachedBlip != null)
							{
								Keycard.AttachedBlip.Alpha = 0;
							}
							EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(Keycard, 0));
							TextNotification("CHAR_HUMANDEFAULT", "~y~The Luxignton Raid~w~", "Intel Found", "You have picked up a Keycard to get access to Large Containers on the Luxignton Aircraft Carrier");
							GotKeycard = true;
						}
					}
				}
				if (SamJammer != null && !GotSamJammer)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, SamJammer.Position, "<", 100f))
					{
						if (CurrentHeistStage == HeistStage.Scout)
						{
							CurrentHeistStage = HeistStage.SamJammer;
						}
					}
					else if (CurrentHeistStage == HeistStage.SamJammer)
					{
						CurrentHeistStage = HeistStage.Scout;
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, SamJammer.Position, "<", 10f) && SamJammer.AttachedBlip == null)
					{
						SamJammer.AddBlip();
						SamJammer.AttachedBlip.Sprite = BlipSprite.SubMissile;
						SamJammer.AttachedBlip.Name = "SAM Jammer";
						SamJammer.AttachedBlip.Color = BlipColor.Green;
						SamJammer.AttachedBlip.IsShortRange = true;
						SamJammer.AttachedBlip.IsFlashing = true;
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, SamJammer.Position, "<", 2.5f))
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Merryweather SAM Jammer");
						if (Game.IsControlJustPressed(Control.Context))
						{
							IntelMarkers[0].Alpha = 0;
							IntelMarkersRadiusArea[0].Alpha = 0;
							if (SamJammer.AttachedBlip != null)
							{
								SamJammer.AttachedBlip.Alpha = 0;
							}
							EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(SamJammer, 0));
							TextNotification("CHAR_HUMANDEFAULT", "~y~The Luxignton Raid~w~", "Intel Found", "You have picked up a SAM Jammer, The Luxignton Aircraft Carrier SAMs will not target you in an Aircraft now.");
							GotKeycard = true;
						}
					}
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, CargoshipIntelLoc, "<", 200f))
				{
					if (Raiju != null)
					{
						if (Game.Player.Character.CurrentVehicle == Raiju)
						{
							if (GetDistance_VDIST2(Raiju.Position, AircraftCarrierLoc, ">", 100f))
							{
								IntelMarkers[1].Alpha = 0;
								IntelMarkersRadiusArea[1].Alpha = 0;
								CurrentHeistStage = HeistStage.InRaiju;
							}
							else
							{
								CurrentHeistStage = HeistStage.Scout;
							}
							UsingRaiju = true;
						}
						else if (GetDistance_VDIST2(Game.Player.Character.Position, CargoshipIntelLoc, "<", 200f))
						{
							CurrentHeistStage = HeistStage.CargoshipTrojanVehicle;
						}
						else if (CurrentHeistStage == HeistStage.CargoshipTrojanVehicle)
						{
							CurrentHeistStage = HeistStage.Scout;
						}
					}
					if (Valkyrie != null)
					{
						if (Game.Player.Character.CurrentVehicle == Valkyrie)
						{
							if (GetDistance_VDIST2(Valkyrie.Position, AircraftCarrierLoc, ">", 100f))
							{
								IntelMarkers[1].Alpha = 0;
								IntelMarkersRadiusArea[1].Alpha = 0;
								CurrentHeistStage = HeistStage.InValkyrie;
							}
							else
							{
								CurrentHeistStage = HeistStage.Scout;
							}
							UsingValkyrie = true;
						}
						else if (GetDistance_VDIST2(Game.Player.Character.Position, CargoshipIntelLoc, "<", 200f))
						{
							CurrentHeistStage = HeistStage.CargoshipTrojanVehicle;
						}
						else if (CurrentHeistStage == HeistStage.CargoshipTrojanVehicle)
						{
							CurrentHeistStage = HeistStage.Scout;
						}
					}
					if (PTBoat != null)
					{
						if (Game.Player.Character.CurrentVehicle == PTBoat)
						{
							if (GetDistance_VDIST2(PTBoat.Position, AircraftCarrierLoc, ">", 100f))
							{
								IntelMarkers[1].Alpha = 0;
								IntelMarkersRadiusArea[1].Alpha = 0;
								CurrentHeistStage = HeistStage.InPTBoat;
							}
							else
							{
								CurrentHeistStage = HeistStage.Scout;
							}
							UsingPTBoat = true;
						}
						else if (GetDistance_VDIST2(Game.Player.Character.Position, CargoshipIntelLoc, "<", 200f))
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, PTBoat.Position, "<", 10f))
							{
								if (!FreedPTBoat)
								{
									CurrentHeistStage = HeistStage.ShootPtBoatCable;
								}
								else
								{
									CurrentHeistStage = HeistStage.CargoshipTrojanVehicle;
								}
							}
							else
							{
								CurrentHeistStage = HeistStage.CargoshipTrojanVehicle;
							}
						}
						else if (CurrentHeistStage == HeistStage.CargoshipTrojanVehicle)
						{
							CurrentHeistStage = HeistStage.Scout;
						}
						if (!FreedPTBoat && Winch != null)
						{
							if (GetDistance_VDIST2(Game.Player.Character.LastWeaponImpactPosition, Winch.Position, "<", 4f))
							{
								if (Winch.AttachedBlip != null)
								{
									Winch.AttachedBlip.Alpha = 0;
								}
								if (Rope != null)
								{
									EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(Rope, 0));
								}
								FreedPTBoat = true;
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, PTBoat.Position, "<", 10f))
							{
								if (Winch.AttachedBlip == null)
								{
									Winch.AddBlip();
								}
								if (Winch.AttachedBlip != null)
								{
									Winch.AttachedBlip.Sprite = BlipSprite.Junkyard;
									Winch.AttachedBlip.Color = BlipColor.Red;
									Winch.AttachedBlip.Name = "Shoot Winch";
									Winch.AttachedBlip.IsShortRange = true;
								}
								CurrentHeistStage = HeistStage.ShootPtBoatCable;
							}
							else if (GetDistance_VDIST2(Game.Player.Character.Position, CargoshipIntelLoc, "<", 200f))
							{
								CurrentHeistStage = HeistStage.CargoshipTrojanVehicle;
							}
						}
						if (FreedPTBoat && PTBoat.IsPositionFrozen)
						{
							PTBoat.Detach();
							PTBoat.IsPositionFrozen = false;
							PTBoat.HasGravity = true;
							PTBoat.ApplyForce(Vector3.WorldDown);
						}
					}
					if (!CreatedCargoshipObjects)
					{
						CargoshipObjects.CreateEntites();
						CargoshipGuards.Clear();
						ConvertNavToSmartAIV2(CargoshipNavs, CargoshipGuards);
						CreatedCargoshipObjects = true;
						Raiju = (Vehicle)CargoshipObjects.Entities[1].Entity;
						Valkyrie = (Vehicle)CargoshipObjects.Entities[4].Entity;
						PTBoat = (Vehicle)CargoshipObjects.Entities[0].Entity;
						PTBoat.Position = CargoshipObjects.Entities[0].Position;
						PTBoat.Rotation = CargoshipObjects.Entities[0].Rotation;
						PTBoat.IsPositionFrozen = true;
						Winch = (Prop)CargoshipObjects.Entities[2].Entity;
						Rope = (Prop)CargoshipObjects.Entities[3].Entity;
					}
					if (CurrentActiveGroup != ActiveGroup.LexigntonRaid_MerryweatherCargoship)
					{
						SmartGuards.GuardGroup[2].GroupList.Clear();
						SmartGuards.GuardGroup[2].GroupList.AddRange(CargoshipGuards);
						SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Merryweather;
						CurrentActiveGroup = ActiveGroup.LexigntonRaid_MerryweatherCargoship;
					}
				}
				else if (CreatedCargoshipObjects)
				{
					foreach (ObjectPlacementMap entity in CargoshipObjects.Entities)
					{
						if (!(entity.Entity != null))
						{
							continue;
						}
						if (entity.Entity == Valkyrie && !UsingValkyrie && entity.Entity != null)
						{
							entity.Entity.Delete();
						}
						if (entity.Entity == Raiju && !UsingRaiju && entity.Entity != null)
						{
							entity.Entity.Delete();
						}
						if (entity.Entity == PTBoat)
						{
							if (!UsingPTBoat && entity.Entity != null)
							{
								entity.Entity.Delete();
							}
						}
						else if (entity.Entity != Valkyrie && entity.Entity != Raiju && entity.Entity != PTBoat && entity.Entity != null)
						{
							entity.Entity.Delete();
						}
					}
					if (CurrentActiveGroup == ActiveGroup.LexigntonRaid_MerryweatherCargoship)
					{
						CurrentHeistStage = HeistStage.Scout;
						CurrentActiveGroup = ActiveGroup.None;
						SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.None;
					}
					CreatedCargoshipObjects = false;
				}
				if (!InTunerGarage)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, TunerGarageEnter, "<", 100f))
					{
						World.DrawMarker(MarkerType.Cylinder, TunerGarageEnter, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
						if (GetDistance_VDIST2(Game.Player.Character.Position, TunerGarageEnter, "<", 2f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Tuner Garage");
							if (Game.IsControlJustPressed(Control.Context))
							{
								RefreshTunerGarage();
								Function.Call(Hash.BEGIN_TEXT_COMMAND_BUSYSPINNER_ON, "STRING");
								Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "Loading Luxignton Raid Assets");
								Function.Call(Hash.END_TEXT_COMMAND_BUSYSPINNER_ON, 5);
								TunerGarageObjects.CreateEntites();
								foreach (ObjectPlacementMap entity2 in TunerGarageObjects.Entities)
								{
									if (entity2.Entity != null && entity2.Entity.Model.IsPed)
									{
										TunerGarageGuards.Add(new SmartGuards.SmartAiV2((Ped)entity2.Entity, SmartGuards.EnemyGroup.Cartel));
									}
								}
								int index = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, GrinderObjects.Entities.Count);
								GrinderObjects.Entities[index].CreateObjatCoords();
								Grinder = (Prop)GrinderObjects.Entities[index].Entity;
								Function.Call(Hash.BUSYSPINNER_OFF);
								Screen.FadeOut(1500);
								Script.Wait(1500);
								InTunerGarage = true;
								Game.Player.Character.Position = TunerGarageExit;
								CurrentHeistStage = HeistStage.Grinder;
								Script.Wait(1500);
								Screen.FadeIn(1500);
							}
						}
					}
				}
				else if (InTunerGarage)
				{
					if (CurrentActiveGroup != ActiveGroup.LexigntonRaid_TunerGarage)
					{
						SmartGuards.GuardGroup[5].GroupList.Clear();
						SmartGuards.GuardGroup[5].GroupList.AddRange(TunerGarageGuards);
						SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Cartel;
						CurrentActiveGroup = ActiveGroup.LexigntonRaid_TunerGarage;
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, TunerGarageExit, "<", 100f))
					{
						World.DrawMarker(MarkerType.Cylinder, TunerGarageExit, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
						if (GetDistance_VDIST2(Game.Player.Character.Position, TunerGarageExit, "<", 2f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit Tuner Garage");
							if (Game.IsControlJustPressed(Control.Context))
							{
								Screen.FadeOut(1500);
								Script.Wait(1500);
								InTunerGarage = false;
								Game.Player.Character.Position = TunerGarageEnter;
								TunerGarageObjects.DeleteEntites();
								SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.None;
								CurrentHeistStage = HeistStage.Scout;
								Script.Wait(1500);
								Screen.FadeIn(1500);
							}
						}
					}
				}
				if (!InMerryweatherGarage)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, MerryweatherGarageEnter, "<", 100f) && ThermiteChargesCollected <= 6)
					{
						World.DrawMarker(MarkerType.Cylinder, MerryweatherGarageEnter, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
						if (GetDistance_VDIST2(Game.Player.Character.Position, MerryweatherGarageEnter, "<", 2f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Merryweather Garage");
							if (Game.IsControlJustPressed(Control.Context))
							{
								Function.Call(Hash.BEGIN_TEXT_COMMAND_BUSYSPINNER_ON, "STRING");
								Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "Loading Luxignton Raid Assets");
								Function.Call(Hash.END_TEXT_COMMAND_BUSYSPINNER_ON, 5);
								MerryweatherGarageObjects.CreateEntites();
								MerryweatherGarageGuards.Clear();
								foreach (ObjectPlacementMap entity3 in MerryweatherGarageObjects.Entities)
								{
									if (entity3.Entity != null && entity3.Entity.Model.IsPed)
									{
										MerryweatherGarageGuards.Add(new SmartGuards.SmartAiV2((Ped)entity3.Entity, SmartGuards.EnemyGroup.Merryweather));
									}
								}
								Function.Call(Hash.BUSYSPINNER_OFF);
								Screen.FadeOut(1500);
								Script.Wait(1500);
								InMerryweatherGarage = true;
								Game.Player.Character.Position = MerryweatherGarageExit;
								CurrentHeistStage = HeistStage.Thermite;
								Script.Wait(1500);
								Screen.FadeIn(1500);
							}
						}
					}
				}
				else if (InMerryweatherGarage)
				{
					if (CurrentActiveGroup != ActiveGroup.LexigntonRaid_MerryweatherGarage)
					{
						SmartGuards.GuardGroup[2].GroupList.Clear();
						SmartGuards.GuardGroup[2].GroupList.AddRange(MerryweatherGarageGuards);
						SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Merryweather;
						CurrentActiveGroup = ActiveGroup.LexigntonRaid_MerryweatherGarage;
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, MerryweatherGarageExit, "<", 100f))
					{
						World.DrawMarker(MarkerType.Cylinder, MerryweatherGarageExit, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
						if (GetDistance_VDIST2(Game.Player.Character.Position, MerryweatherGarageExit, "<", 2f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit Merryweather Garage");
							if (Game.IsControlJustPressed(Control.Context))
							{
								Screen.FadeOut(1500);
								Script.Wait(1500);
								InMerryweatherGarage = false;
								Game.Player.Character.Position = MerryweatherGarageEnter;
								MerryweatherGarageObjects.DeleteEntites();
								SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.None;
								CurrentHeistStage = HeistStage.Scout;
								Script.Wait(1500);
								Screen.FadeIn(1500);
							}
						}
					}
				}
				if (!InMerryweatherFacility)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, MerryweatherFacilityEnter, "<", 100f) && C4ChargesCollected <= 6)
					{
						World.DrawMarker(MarkerType.Cylinder, MerryweatherFacilityEnter, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
						if (GetDistance_VDIST2(Game.Player.Character.Position, MerryweatherFacilityEnter, "<", 2f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Merryweather Facility");
							if (Game.IsControlJustPressed(Control.Context))
							{
								Function.Call(Hash.BEGIN_TEXT_COMMAND_BUSYSPINNER_ON, "STRING");
								Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "Loading Luxignton Raid Assets");
								Function.Call(Hash.END_TEXT_COMMAND_BUSYSPINNER_ON, 5);
								MerryweatherFacilityObjects.CreateEntites();
								MerryweatherFacilityGuards.Clear();
								foreach (ObjectPlacementMap entity4 in MerryweatherFacilityObjects.Entities)
								{
									if (entity4.Entity != null && entity4.Entity.Model.IsPed)
									{
										MerryweatherFacilityGuards.Add(new SmartGuards.SmartAiV2((Ped)entity4.Entity, SmartGuards.EnemyGroup.Merryweather));
									}
								}
								Function.Call(Hash.BUSYSPINNER_OFF);
								Screen.FadeOut(1500);
								Script.Wait(1500);
								InMerryweatherFacility = true;
								Game.Player.Character.Position = MerryweatherFacilityExit;
								CurrentHeistStage = HeistStage.C4;
								Script.Wait(1500);
								Screen.FadeIn(1500);
							}
						}
					}
				}
				else if (InMerryweatherFacility)
				{
					if (CurrentActiveGroup != ActiveGroup.LexigntonRaid_MerryweatherFacility)
					{
						SmartGuards.GuardGroup[2].GroupList.Clear();
						SmartGuards.GuardGroup[2].GroupList.AddRange(MerryweatherFacilityGuards);
						SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Merryweather;
						CurrentActiveGroup = ActiveGroup.LexigntonRaid_MerryweatherFacility;
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, MerryweatherFacilityExit, "<", 100f))
					{
						World.DrawMarker(MarkerType.Cylinder, MerryweatherFacilityExit, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
						if (GetDistance_VDIST2(Game.Player.Character.Position, MerryweatherFacilityExit, "<", 2f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit Merryweather Facility");
							if (Game.IsControlJustPressed(Control.Context))
							{
								Screen.FadeOut(1500);
								Script.Wait(1500);
								InMerryweatherFacility = false;
								Game.Player.Character.Position = MerryweatherFacilityEnter;
								MerryweatherFacilityObjects.DeleteEntites();
								SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.None;
								CurrentHeistStage = HeistStage.Scout;
								Script.Wait(1500);
								Screen.FadeIn(1500);
							}
						}
					}
				}
				if (!Game.Player.IsAlive)
				{
					EndHeist();
					FailHeist("cause the Player Died");
				}
			}
			Vector3 position5 = Game.Player.Character.Position;
		}
	}
}
