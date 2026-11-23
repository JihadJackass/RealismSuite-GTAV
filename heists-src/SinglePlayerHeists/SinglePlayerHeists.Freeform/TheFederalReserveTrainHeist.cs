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
	internal class TheFederalReserveTrainHeist : Script
	{
		public enum ActiveGroup
		{
			None,
			TheFederalReserveTrainHeist_LostMCGuards,
			TheFederalReserveTrainHeist_OnielFarmGuards,
			TheFederalReserveTrainHeist_DepartmentOfRail
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

		public class C4TrainNode
		{
			public Blip C4Blip { get; set; }

			public Vector3 Location { get; set; }

			public bool C4Placed { get; set; }

			public C4TrainNode()
			{
			}

			public C4TrainNode(Vector3 Loc, bool Active)
			{
				Location = Loc;
				C4Placed = Active;
			}
		}

		public class KeypadClass
		{
			public Vector3 KeypadPos { get; set; }

			public float KeypadRot { get; set; }

			public bool AwaitingHackGUI { get; set; }

			public Prop Keypad { get; set; }

			public bool SpawnedKeypad { get; set; }

			public int Type { get; set; }

			public KeypadClass()
			{
			}

			public KeypadClass(Prop K, int T)
			{
				Keypad = K;
				Keypad.IsPositionFrozen = true;
				Keypad.AddBlip();
				Keypad.AttachedBlip.Sprite = BlipSprite.Keypad2;
				Keypad.AttachedBlip.Scale = 0.75f;
				Keypad.AttachedBlip.IsShortRange = true;
				SpawnedKeypad = true;
				KeypadRot = K.Rotation.Z;
				KeypadPos = K.Position;
				Type = T;
			}

			public KeypadClass(Vector3 L, float R, int T)
			{
				SpawnedKeypad = false;
				KeypadRot = R;
				KeypadPos = L;
				Type = T;
			}
		}

		public enum HeistStage
		{
			Scout,
			C4,
			TrainEngineerStationKeycard,
			Thermite,
			Grinder,
			TrainStoppingAtQuarry,
			CannotOpenTrainEngineerStation,
			SwitchedPower,
			SwitchedTrack,
			CannotOpenContainers,
			PlaceC4onTrack,
			C4PlaceWaitforTrain,
			LootingTrain,
			HackKeypadTrainEngineersStation
		}

		public enum Nodetype
		{
			AnyRoad,
			Road,
			Offroad,
			Water
		}

		public class BusinessCrateVehicleData
		{
			public List<ObjectPlacementMap> VehicleData { get; set; }

			public BusinessCrateVehicleData()
			{
			}

			public BusinessCrateVehicleData(List<ObjectPlacementMap> VD)
			{
				VehicleData = VD;
			}
		}

		public class RobbableContainer
		{
			public List<BusinessCrateVehicleData> Train_Loot_ContainerChoices = new List<BusinessCrateVehicleData>
			{
				new BusinessCrateVehicleData(new List<ObjectPlacementMap>
				{
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 441968, 3595011691u, new Vector3(-1043.70276f, -3210.77881f, 12.9458771f), new Vector3(0f, -0f, -119.917778f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_cash_low_trolly_01c"), 92933, 3781553190u, new Vector3(-1042.27673f, -3210.90723f, 13.5458775f), new Vector3(0f, -0f, -299.9178f), 441968, 0, new Vector3(-0.5999999f, 1.30000007f, 0.5999999f), new Vector3(0f, 0f, 180f)),
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_gold_trolly_01c"), 360810, 3559648747u, new Vector3(-1042.87524f, -3211.94727f, 13.5458775f), new Vector3(0f, -0f, -299.9178f), 441968, 0, new Vector3(0.5999999f, 1.30000007f, 0.599999964f), new Vector3(0f, 0f, 180f))
				}),
				new BusinessCrateVehicleData(new List<ObjectPlacementMap>
				{
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 370499, 3595011691u, new Vector3(-1038.30408f, -3210.77881f, 12.9458771f), new Vector3(0f, -0f, -119.917778f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_gold_trolly_01c"), 575824, 3559648747u, new Vector3(-1036.928f, -3210.994f, 13.5458775f), new Vector3(0f, -0f, -299.9178f), 370499, 0, new Vector3(-0.5f, 1.30000007f, 0.599999964f), new Vector3(0f, 0f, 180f)),
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_gold_trolly_01c"), 527470, 3559648747u, new Vector3(-1037.47656f, -3211.94727f, 13.5458775f), new Vector3(0f, -0f, -299.9178f), 370499, 0, new Vector3(0.5999999f, 1.30000007f, 0.599999964f), new Vector3(0f, 0f, 180f))
				}),
				new BusinessCrateVehicleData(new List<ObjectPlacementMap>
				{
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 770569, 3595011691u, new Vector3(-1054.30017f, -3210.77881f, 12.9458771f), new Vector3(0f, -0f, -119.917778f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_cash_low_trolly_01c"), 592174, 3781553190u, new Vector3(-1052.87415f, -3210.90723f, 13.5458775f), new Vector3(0f, -0f, -299.9178f), 770569, 0, new Vector3(-0.5999999f, 1.30000007f, 0.5999999f), new Vector3(0f, 0f, 180f)),
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_cash_low_trolly_01c"), 211467, 3781553190u, new Vector3(-1053.47266f, -3211.94727f, 13.5458775f), new Vector3(0f, -0f, -299.9178f), 770569, 0, new Vector3(0.6f, 1.30000007f, 0.5999999f), new Vector3(0f, 0f, 180f))
				}),
				new BusinessCrateVehicleData(new List<ObjectPlacementMap>
				{
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 769544, 3595011691u, new Vector3(-1033.00537f, -3210.77881f, 12.9458771f), new Vector3(0f, -0f, -119.917778f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_gold_trolly_01c"), 423278, 3559648747u, new Vector3(-1032.17786f, -3211.94727f, 13.5458775f), new Vector3(0f, -0f, -299.9178f), 769544, 0, new Vector3(0.5999999f, 1.30000007f, 0.599999964f), new Vector3(0f, 0f, 180f))
				}),
				new BusinessCrateVehicleData(new List<ObjectPlacementMap>
				{
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 606840, 3595011691u, new Vector3(-1054.30017f, -3214.78271f, 12.9458771f), new Vector3(0f, -0f, -119.917778f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_cash_low_trolly_01c"), 381301, 3781553190u, new Vector3(-1052.87415f, -3214.91113f, 13.5458775f), new Vector3(0f, -0f, -299.9178f), 606840, 0, new Vector3(-0.5999999f, 1.30000007f, 0.5999999f), new Vector3(0f, 0f, 180f))
				}),
				new BusinessCrateVehicleData(new List<ObjectPlacementMap>
				{
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 210303, 3595011691u, new Vector3(-1054.30017f, -3219.5874f, 12.9458771f), new Vector3(0f, -0f, -119.917778f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_cash_low_trolly_01c"), 744558, 3781553190u, new Vector3(-1053.47266f, -3220.75586f, 13.5458775f), new Vector3(0f, -0f, -299.9178f), 210303, 0, new Vector3(0.6f, 1.30000007f, 0.5999999f), new Vector3(0f, 0f, 180f))
				}),
				new BusinessCrateVehicleData(new List<ObjectPlacementMap>
				{
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 134676, 3595011691u, new Vector3(-1040.00366f, -3206.47461f, 12.9458771f), new Vector3(0f, -0f, -119.917778f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_gold_trolly_01c"), 304996, 3559648747u, new Vector3(-1038.62756f, -3206.6897f, 13.5458775f), new Vector3(0f, -0f, -299.9178f), 134676, 0, new Vector3(-0.5f, 1.30000007f, 0.599999964f), new Vector3(0f, 0f, 180f))
				}),
				new BusinessCrateVehicleData(new List<ObjectPlacementMap>
				{
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 406037, 3595011691u, new Vector3(-1042.40308f, -3202.27051f, 13.0458775f), new Vector3(0f, -0f, -119.917778f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_diamond_trolly_01c"), 297060, 3489641539u, new Vector3(-1041.06372f, -3202.34888f, 13.6458778f), new Vector3(0f, -0f, -299.9178f), 406037, 0, new Vector3(-0.599999964f, 1.2f, 0.599999964f), new Vector3(0f, 0f, 180f)),
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_diamond_trolly_01c"), 300113, 3489641539u, new Vector3(-1041.66223f, -3203.38916f, 13.6458778f), new Vector3(0f, -0f, -299.9178f), 406037, 0, new Vector3(0.599999964f, 1.2f, 0.599999964f), new Vector3(0f, 0f, 180f))
				}),
				new BusinessCrateVehicleData(new List<ObjectPlacementMap>
				{
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 742425, 3595011691u, new Vector3(-1046.702f, -3206.57471f, 13.0458775f), new Vector3(0f, -0f, -119.917778f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_diamond_trolly_01c"), 62056, 3489641539u, new Vector3(-1045.662f, -8718f / MathF.E, 13.6458778f), new Vector3(0f, -0f, -299.9178f), 742425, 0, new Vector3(0f, 1.2f, 0.599999964f), new Vector3(0f, 0f, 180f))
				}),
				new BusinessCrateVehicleData(new List<ObjectPlacementMap>
				{
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 16456, 3595011691u, new Vector3(-1048.60156f, -3202.37061f, 12.9458771f), new Vector3(0f, -0f, -119.917778f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sf_prop_sf_table_office_01a"), 526667, 3057578855u, new Vector3(-1047.64819f, -3202.91919f, 13.1458769f), new Vector3(0f, -0f, -119.917778f), 16456, 0, new Vector3(0f, 1.1f, 0.199999988f), new Vector3(0f, 0f, 0f)),
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 440591, 1228147776u, new Vector3(-1047.64819f, -3202.91919f, 13.9458771f), new Vector3(0f, -0f, -119.917778f), 16456, 0, new Vector3(0f, 1.1f, 1f), new Vector3(0f, 0f, 0f))
				}),
				new BusinessCrateVehicleData(new List<ObjectPlacementMap>
				{
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 287595, 3595011691u, new Vector3(-1051.70081f, -3206.07422f, 12.9458771f), new Vector3(0f, -0f, -119.917778f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sf_prop_sf_table_office_01a"), 443447, 3057578855u, new Vector3(-1050.74744f, -3206.6228f, 13.1458769f), new Vector3(0f, -0f, -119.917778f), 287595, 0, new Vector3(0f, 1.1f, 0.199999988f), new Vector3(0f, 0f, 0f)),
					new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_gold_stack_01a"), 118553, 4114893066u, new Vector3(-1050.83411f, -3206.573f, 13.9458771f), new Vector3(0f, -0f, -119.917778f), 287595, 0, new Vector3(0f, 1f, 1f), new Vector3(0f, 0f, 0f))
				})
			};

			public int ParticleTimer { get; set; }

			public int containerswitch { get; set; }

			public bool ContainsAnECU { get; set; }

			public List<ObjectPlacementMap> InteriorObjects { get; set; }

			public int PTFX1 { get; set; }

			public int PTFX2 { get; set; }

			public int BombExplodeTimer { get; set; }

			public Prop Bomb { get; set; }

			public int plantBombStage { get; set; }

			public int plantThermalChargeStage { get; set; }

			public bool OpeningContainer { get; set; }

			public bool OpenedContainer { get; set; }

			public Prop Saw { get; set; }

			public Prop Bag { get; set; }

			public int SyncScene { get; set; }

			public int SyncScene2 { get; set; }

			public int SawTimer { get; set; }

			public Prop Table { get; set; }

			public Prop FakeContainer { get; set; }

			public Prop Lock { get; set; }

			public Prop Loot { get; set; }

			public Vehicle TrainCar { get; set; }

			public int Type { get; set; }

			public bool PlayingPTFX { get; set; }

			public float SceneRate { get; set; }

			public int Sound { get; set; }

			public int ParticleFx { get; set; }

			public int RobbableType { get; set; }

			public bool RobbedLoot { get; set; }

			public Ped Driver { get; set; }

			public Ped Passenger { get; set; }

			public Model RequestModel(string Name)
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

			public Model RequestModel(int Name)
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

			public Model RequestModel(PedHash Name)
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

			public Model RequestModel(VehicleHash Name)
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
		}

		public class ArmouredTrainCarrage
		{
			public bool CreatedInteriorLoot { get; set; }

			public bool CreatedLoot { get; set; }

			public Vehicle Carrage { get; set; }

			public Vehicle Turret1 { get; set; }

			public Vehicle Turret2 { get; set; }

			public Ped Turret1Pilot { get; set; }

			public int Turret1Cycles { get; set; }

			public int Turret1ReloadWait { get; set; }

			public int Turret1CyclesToReload { get; set; }

			public Ped Turret2Pilot { get; set; }

			public int Turret2Cycles { get; set; }

			public int Turret2CyclesToReload { get; set; }

			public int Turret2ReloadWait { get; set; }

			public Prop Container1 { get; set; }

			public Prop Container2 { get; set; }

			public RobbableContainer RobbableContainer1 { get; set; }

			public RobbableContainer RobbableContainer2 { get; set; }

			public ArmouredTrainCarrage()
			{
			}

			public ArmouredTrainCarrage(Vehicle C, Vehicle T1, Vehicle T2, Ped T1P, Ped T2P, Prop Cn1)
			{
				Carrage = C;
				Turret1 = T1;
				Turret2 = T2;
				Turret1Pilot = T1P;
				Turret2Pilot = T2P;
				Container1 = Cn1;
			}

			public ArmouredTrainCarrage(Vehicle C, Prop Cn1, Prop Cn2)
			{
				Carrage = C;
				Container1 = Cn1;
				Container2 = Cn2;
			}
		}

		public class AATurret
		{
			public Vehicle Turret1 { get; set; }

			public Ped Turret1Pilot { get; set; }

			public int Turret1Cycles { get; set; }

			public int Turret1ReloadWait { get; set; }

			public int Turret1CyclesToReload { get; set; }

			public AATurret()
			{
			}

			public AATurret(Vehicle T1, Ped T1P)
			{
				Turret1 = T1;
				Turret1Pilot = T1P;
			}
		}

		public class SmartAI
		{
			public Vehicle LastVehicle { get; set; }

			public int ShootAtTimer { get; set; }

			public int HeliAttackTimer { get; set; }

			public int NextHeliAttackTimer { get; set; }

			public bool MarkAsInvestigated { get; set; }

			public bool InvestigatingDeadBody { get; set; }

			public PedTaskLoop WanderLoop { get; set; }

			public Vector3 InvestigatePoint { get; set; }

			public Vector3 OriginalPosition { get; set; }

			public float OriginalHeading { get; set; }

			public Ped Ped { get; set; }

			public bool HasSeenPlayer { get; set; }

			public bool InvestigatingGunshot { get; set; }

			public int InvestigationTimeout { get; set; }

			public bool MovingBacktoOriginalPoint { get; set; }

			public int Timer { get; set; }

			public Vehicle OwnedVehicle { get; set; }

			public SmartAI()
			{
			}

			public SmartAI(Ped P)
			{
				OriginalPosition = P.Position;
				OriginalHeading = P.Heading;
				Ped = P;
				InvestigatingGunshot = false;
				HasSeenPlayer = false;
			}
		}

		public class AircraftWithWeapons
		{
			public Ped TargetPed { get; set; }

			public Ped Pilot { get; set; }

			public Vehicle Heli { get; set; }

			public int RoundsToFire { get; set; }

			public int RoundsFired { get; set; }

			public int NextFireTime { get; set; }

			public AircraftWithWeapons()
			{
			}

			public AircraftWithWeapons(Ped T, Ped P, Vehicle H, int R)
			{
				TargetPed = T;
				Pilot = P;
				Heli = H;
				RoundsToFire = R;
			}
		}

		public class AircraftWeaponController : Script
		{
			public static List<AircraftWithWeapons> AircraftWithWeapons = new List<AircraftWithWeapons>();

			public AircraftWeaponController()
			{
				base.Tick += update;
			}

			private static void drawText(string text, float x, float y, float scale, int r, int g, int b)
			{
				Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
				Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
				Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
				Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
			}

			public void update(object sender, EventArgs e)
			{
				for (int i = 0; i < AircraftWithWeapons.Count; i++)
				{
					AircraftWithWeapons aircraftWithWeapons = AircraftWithWeapons[i];
					if (aircraftWithWeapons.Heli != null && aircraftWithWeapons.Heli.IsAlive && aircraftWithWeapons.Pilot != null && aircraftWithWeapons.Pilot.IsAlive && aircraftWithWeapons.RoundsFired < aircraftWithWeapons.RoundsToFire && Game.GameTime >= aircraftWithWeapons.NextFireTime)
					{
						aircraftWithWeapons.NextFireTime = Game.GameTime + 2500;
						Vector3 position = Game.Player.Character.Position;
						aircraftWithWeapons.RoundsFired++;
						Function.Call(Hash.SET_VEHICLE_SHOOT_AT_TARGET, aircraftWithWeapons.Heli, Game.Player.Character, position.X, position.Y, position.Z);
						Function.Call(Hash.SET_VEHICLE_SHOOT_AT_TARGET, aircraftWithWeapons.Heli, Game.Player.Character, position.X, position.Y, position.Z);
						Function.Call(Hash.SET_VEHICLE_SHOOT_AT_TARGET, aircraftWithWeapons.Heli, Game.Player.Character, position.X, position.Y, position.Z);
					}
					if (aircraftWithWeapons.RoundsFired >= aircraftWithWeapons.RoundsToFire)
					{
						AircraftWithWeapons.RemoveAt(i);
					}
				}
			}
		}

		public class SmartAI_Settings
		{
			public Entity AI_Target = Game.Player.Character;

			public List<int> AircraftWithMissiles = new List<int>
			{
				Function.Call<int>(Hash.GET_HASH_KEY, "lazer"),
				Function.Call<int>(Hash.GET_HASH_KEY, "buzzard"),
				Function.Call<int>(Hash.GET_HASH_KEY, "Conada2"),
				Function.Call<int>(Hash.GET_HASH_KEY, "hydra")
			};

			public float TooFarAwayDist = 1000f;

			public bool DisableAiBlip { get; set; }

			public int SmartAI_SpawnedHelis { get; set; }

			public int SmartAI_SpawnedCars { get; set; }

			public int SmartAI_SpawnedPlanes { get; set; }

			public int SmartAI_SpawnedBoats { get; set; }

			public bool CanAttackPlayerInBoat { get; set; }

			public bool CanAttackPlayerInPlane { get; set; }

			public bool CanAttackPlayerInCar { get; set; }

			public bool CanAttackPlayerInHeli { get; set; }

			public bool AllowDespawnIfTooFarAwayThisFrame { get; set; }

			public bool LockMissileUseThisFrame { get; set; }

			public int MerryweatherCheck { get; set; }

			public Vector3 Player_LastWeaponImpactPosition { get; set; }

			public float Radius { get; set; }

			public float HitRadius { get; set; }

			public bool ShowCone { get; set; }

			public bool AllowInvestigate { get; set; }

			public float InvestigateDist { get; set; }

			public float SeeingAngle { get; set; }

			public bool ArrestInsteadOfShoot { get; set; }

			public bool RunPatrolRoute { get; set; }
		}

		public class PedTaskLoop : Script
		{
			public List<Vector3> Loop { get; set; }

			public int AtPoint { get; set; }

			public bool AllowDelay { get; set; }

			public bool OverrideDelayOff { get; set; }

			public int Timerdelay { get; set; }

			public int Timer { get; set; }

			public PedTaskLoop()
			{
			}

			public PedTaskLoop(List<Vector3> L, int Point)
			{
				Loop = L;
			}

			public PedTaskLoop(List<Vector3> L, int Point, bool AD)
			{
				OverrideDelayOff = AD;
				Loop = L;
			}
		}

		public class ArmouredTrain
		{
			public List<SmartAI> MerryweatherPeds = new List<SmartAI>();

			public List<Vehicle> TrainCars = new List<Vehicle>();

			public List<ArmouredTrainCarrage> TrainArmouredCarrages = new List<ArmouredTrainCarrage>();

			public List<Vehicle> Turrets = new List<Vehicle>();

			public List<Ped> AttackingPeds = new List<Ped>();

			public List<Prop> SecondaryProps = new List<Prop>();

			public List<Prop> BrakeCylinders = new List<Prop>();

			public bool CreatedBlip;

			public Vector3 Spawn { get; set; }

			public int UpdateAiTaskCall { get; set; }

			public float TrainSpeed { get; set; }

			public Ped TrainPilot { get; set; }

			public Vehicle Train { get; set; }

			public bool TrainStopped { get; set; }

			public bool TrainFullyStopped { get; set; }

			public bool TrainActive { get; set; }

			public bool TrainDerailed { get; set; }

			public bool AwaingReadySignal { get; set; }

			public Blip Radius { get; set; }

			public Blip DeliveryPoint { get; set; }

			public Model RequestModel(string Name)
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

			public Model RequestModel(int Name)
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

			public Model RequestModel(PedHash Name)
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

			public Model RequestModel(VehicleHash Name)
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

			public ArmouredTrain()
			{
			}

			public void ATTACH_ENTITY_TO_ENTITY(Entity entity1, Entity entity2, int boneIndex, float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, bool p9, bool useSoftPinning, bool collision, bool isPed, int vertexIndex, bool fixedRot)
			{
				Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, entity1, entity2, boneIndex, xPos, yPos, zPos, xRot, yRot, zRot, p9, useSoftPinning, collision, isPed, vertexIndex, fixedRot);
			}

			public void RemoveTrain()
			{
				if (!TrainActive)
				{
					return;
				}
				TrainActive = false;
				if (Train != null)
				{
					Train.Delete();
				}
				if (TrainPilot != null)
				{
					TrainPilot.Delete();
				}
				if (TrainCars.Count <= 0)
				{
					return;
				}
				foreach (Vehicle trainCar in TrainCars)
				{
					if (trainCar != null)
					{
						trainCar.Delete();
					}
				}
				TrainCars.Clear();
			}

			public void CreateTrain()
			{
				AwaingReadySignal = true;
				if (Train != null)
				{
					Train.Delete();
				}
				if (TrainPilot != null)
				{
					TrainPilot.Delete();
				}
				if (TrainCars.Count > 0)
				{
					foreach (Vehicle trainCar in TrainCars)
					{
						if (trainCar != null)
						{
							trainCar.Delete();
						}
					}
					TrainCars.Clear();
				}
				RequestModel(VehicleHash.Freight);
				RequestModel(VehicleHash.FreightCar);
				RequestModel(VehicleHash.FreightGrain);
				RequestModel(VehicleHash.FreightCont1);
				RequestModel(VehicleHash.FreightCont2);
				RequestModel(VehicleHash.FreightTrailer);
				Vector3 spawn = Spawn;
				Train = Function.Call<Vehicle>(Hash.CREATE_MISSION_TRAIN, 7, spawn.X, spawn.Y, spawn.Z, false, 0, 0);
				Function.Call(Hash.SET_VEHICLE_MOD_KIT, Train.Handle, 0);
				Train.Mods.PrimaryColor = VehicleColor.MatteBlack;
				Train.Mods.SecondaryColor = VehicleColor.MetallicRed;
				TrainSpeed = 15f;
				TrainPilot = GlobalHelper.CreatePed(RequestModel("ig_pilot"), Train.Position, 0f);
				TrainPilot.SetIntoVehicle(Train, VehicleSeat.Driver);
				Train.AddBlip();
				Train.AttachedBlip.Sprite = BlipSprite.Train;
				Train.AttachedBlip.Name = "Armoured Train";
				Train.AttachedBlip.IsShortRange = true;
				BrakeCylinders.Clear();
				Prop prop = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_break_dev_01a"), Train.Position, true, false);
				ATTACH_ENTITY_TO_ENTITY(prop, Train, 0, 1.15f, -5.15f, -0.7f, 0f, 0f, 90f, false, false, false, false, 0, true);
				BrakeCylinders.Add(prop);
				prop.IsInvincible = false;
				Function.Call(Hash.SET_ENTITY_CAN_BE_DAMAGED, prop, true);
				prop.AddBlip();
				prop.AttachedBlip.Sprite = BlipSprite.Crosshair2;
				prop.AttachedBlip.Scale = 0.5f;
				prop.AttachedBlip.Name = "Brake Cylinders";
				prop.AttachedBlip.Color = BlipColor.Red;
				prop.AttachedBlip.IsShortRange = true;
				prop = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_break_dev_01a"), Train.Position, true, false);
				ATTACH_ENTITY_TO_ENTITY(prop, Train, 0, -1.15f, -5.15f, -0.7f, 0f, 0f, -90f, false, false, false, false, 0, true);
				BrakeCylinders.Add(prop);
				prop.IsInvincible = false;
				Function.Call(Hash.SET_ENTITY_CAN_BE_DAMAGED, prop, true);
				prop.AddBlip();
				prop.AttachedBlip.Sprite = BlipSprite.Crosshair2;
				prop.AttachedBlip.Scale = 0.5f;
				prop.AttachedBlip.Name = "Brake Cylinders";
				prop.AttachedBlip.Color = BlipColor.Red;
				prop.AttachedBlip.IsShortRange = true;
				prop = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_break_dev_01a"), Train.Position, true, false);
				ATTACH_ENTITY_TO_ENTITY(prop, Train, 0, 1.15f, 5.15f, -0.7f, 0f, 0f, 90f, false, false, false, false, 0, true);
				BrakeCylinders.Add(prop);
				prop.IsInvincible = false;
				Function.Call(Hash.SET_ENTITY_CAN_BE_DAMAGED, prop, true);
				prop.AddBlip();
				prop.AttachedBlip.Sprite = BlipSprite.Crosshair2;
				prop.AttachedBlip.Scale = 0.5f;
				prop.AttachedBlip.Name = "Brake Cylinders";
				prop.AttachedBlip.Color = BlipColor.Red;
				prop.AttachedBlip.IsShortRange = true;
				prop = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_break_dev_01a"), Train.Position, true, false);
				ATTACH_ENTITY_TO_ENTITY(prop, Train, 0, -1.15f, 5.15f, -0.7f, 0f, 0f, -90f, false, false, false, false, 0, true);
				BrakeCylinders.Add(prop);
				prop.IsInvincible = false;
				Function.Call(Hash.SET_ENTITY_CAN_BE_DAMAGED, prop, true);
				prop.AddBlip();
				prop.AttachedBlip.Sprite = BlipSprite.Crosshair2;
				prop.AttachedBlip.Scale = 0.5f;
				prop.AttachedBlip.Name = "Brake Cylinders";
				prop.AttachedBlip.Color = BlipColor.Red;
				prop.AttachedBlip.IsShortRange = true;
				for (int i = 0; i < 100; i++)
				{
					Vehicle vehicle = Function.Call<Vehicle>(Hash.GET_TRAIN_CARRIAGE, Train, i);
					if (vehicle != null)
					{
						TrainCars.Add(vehicle);
						if (vehicle.Model == RequestModel("FreightCar"))
						{
							Prop prop2 = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_container_01a"), vehicle.GetOffsetPosition(new Vector3(1.6f, 0f, -0.33f)), false, false);
							Function.Call(Hash.SET_OBJECT_TINT_INDEX, prop2, 1);
							ATTACH_ENTITY_TO_ENTITY(prop2, vehicle, 0, 0f, 4.2f, -0.33f, 0f, 0f, 0f, false, false, true, false, 2, true);
							SecondaryProps.Add(prop2);
							Prop prop3 = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_container_01a"), vehicle.GetOffsetPosition(new Vector3(1.6f, 0f, -0.33f)), false, false);
							Function.Call(Hash.SET_OBJECT_TINT_INDEX, prop3, 1);
							ATTACH_ENTITY_TO_ENTITY(prop3, vehicle, 0, 0f, -4.2f, -0.33f, 0f, 0f, -180f, false, false, true, false, 2, true);
							SecondaryProps.Add(prop3);
							ArmouredTrainCarrage item = new ArmouredTrainCarrage(vehicle, prop2, prop3);
							TrainArmouredCarrages.Add(item);
						}
					}
				}
				TrainActive = true;
			}

			public ArmouredTrain(Vector3 S)
			{
				Spawn = S;
			}

			public void DeleteBlip()
			{
				if (Radius != null)
				{
					Radius.Delete();
					Radius = null;
				}
				if (DeliveryPoint != null)
				{
					DeliveryPoint.Delete();
					DeliveryPoint = null;
				}
			}

			public void CreateBlip()
			{
				Radius = GlobalHelper.CreateBlip(Spawn.Around(20f), 250f);
				Radius.Color = BlipColor.Yellow;
				Radius.IsShortRange = true;
				Radius.Alpha = 144;
				DeliveryPoint = GlobalHelper.CreateBlip(Radius.Position);
				DeliveryPoint.Color = BlipColor.Yellow;
				DeliveryPoint.IsShortRange = true;
				DeliveryPoint.Name = "Source Business Crates : Armoured Train";
				DeliveryPoint.Alpha = 0;
				Radius.Alpha = 0;
			}

			public void SetRelationShip(Ped P)
			{
				RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
				Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, P, relationshipGroup);
				Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, P, 0, 0);
				Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, P, 1);
			}
		}

		public static ActiveGroup CurrentActiveGroup = ActiveGroup.None;

		public static bool GotGrinder;

		public static bool GotC4 = false;

		public static int C4ChargesCollected = 20;

		public static bool GotThermite = true;

		public static int ThermiteChargesCollected = 20;

		public static bool GotKeycard;

		public static int HeistMinPay = 850000;

		public static int HeistMaxPay = 3250000;

		public static int CurrentPay = 0;

		public static int LocalTake = 0;

		public static bool HackingKeypad = false;

		public static Prop LastKeypad;

		public static Prop USB;

		public static Prop Phone;

		public int Scene1;

		public static List<SmartGuards.SmartAiV2> LostMCGuards = new List<SmartGuards.SmartAiV2>();

		public static List<SmartGuards.SmartAiV2> OnielFarmGuards = new List<SmartGuards.SmartAiV2>();

		public static List<SmartGuards.SmartAiV2> DepartmentofRailGuards = new List<SmartGuards.SmartAiV2>();

		public static MissionOjects LostMCObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cs_server_drive"), 210181, 977288393u, new Vector3(2158.90356f, 3810.25635f, 31.4118423f), new Vector3(0f, -0f, 171.5889f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_mil_crate_02b"), 630790, 3626201158u, new Vector3(2152.604f, 3809.13428f, 31.700386f), new Vector3(0f, -0f, -144.147125f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mb_crate_01a"), 169225, 481432069u, new Vector3(2150.70239f, 3807.348f, 31.3898468f), new Vector3(0f, -0f, -144.555954f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 133135, 304964818u, new Vector3(1295.61914f, 4383.595f, 40.308548f), new Vector3(0f, -0f, -105.179192f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 251407, 304964818u, new Vector3(1337.11035f, 4361.094f, 43.36432f), new Vector3(0f, -0f, 134.135788f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 514575, 304964818u, new Vector3(1322.306f, 4308.98242f, 37.07568f), new Vector3(0f, 0f, -8.40444f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 353808, 304964818u, new Vector3(1301.62024f, 4339.27f, 37.83504f), new Vector3(0f, 0f, -15.1602459f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_coke_block_01a"), 35090, 3896957328u, new Vector3(1337.42261f, 4360.90137f, 44.2048454f), new Vector3(0f, -0f, 135.1505f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_boxcoke_01"), 215826, 3348973628u, new Vector3(1321.7655f, 4309.074f, 37.92012f), new Vector3(0f, 0f, -9.53076649f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_boxcoke_01"), 452370, 3348973628u, new Vector3(1301.98315f, 4339.269f, 38.67948f), new Vector3(0f, 0f, -12.4480705f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_coke_block_01a"), 287506, 3896957328u, new Vector3(1295.46582f, 4383.305f, 41.1490746f), new Vector3(0f, -0f, -102.679832f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects LostMCC4Objects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 286479, 277707989u, new Vector3(1300.936f, 4339.49658f, 38.79896f), new Vector3(0f, 0f, -1.69960546f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 444432, 277707989u, new Vector3(1322.95789f, 4308.711f, 38.0396f), new Vector3(0f, 0f, -31.0258942f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 707856, 277707989u, new Vector3(1295.86584f, 4384.283f, 41.27247f), new Vector3(0f, -0f, -179.041809f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 484625, 277707989u, new Vector3(1336.71f, 4361.5957f, 44.32824f), new Vector3(0f, -0f, 120.615509f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 609803, 277707989u, new Vector3(1348.07874f, 4390.533f, 43.4682922f), new Vector3(0f, 0f, 71.2422943f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 566795, 277707989u, new Vector3(1320.96765f, 4312.557f, 38.2143478f), new Vector3(0f, -0f, -124.517159f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static List<MissionOjects> LostMCNavs = new List<MissionOjects>
		{
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 661779, 1803116220u, new Vector3(1395.64014f, 4384.19f, 42.5443f), new Vector3(0f, 0f, 9.049474f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 590097, 1803116220u, new Vector3(1379.62439f, 4375.94434f, 43.255024f), new Vector3(0f, 0f, 4.903784f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 173076, 1803116220u, new Vector3(1354.87439f, 4374.762f, 43.7250748f), new Vector3(0f, 0f, 6.285645f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 716309, 1803116220u, new Vector3(1330.58752f, 4379.46f, 43.75755f), new Vector3(0f, 0f, -4.0529027f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 238102, 1803116220u, new Vector3(1354.73328f, 4374.74072f, 43.7244339f), new Vector3(0f, 0f, -4.922974f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 536597, 1803116220u, new Vector3(1379.822f, 4376.056f, 43.2344627f), new Vector3(0f, 0f, 7.20688248f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 287766, 1803116220u, new Vector3(1395.85913f, 4384.27246f, 42.5346947f), new Vector3(0f, 0f, 29.5728359f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "The Lost MC Male"), 404502, 1330042375u, new Vector3(1396.48535f, 4384.80664f, 43.1465378f), new Vector3(0f, -0f, 139.81665f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 663575, 1803116220u, new Vector3(1316.44006f, 4379.202f, 40.7667236f), new Vector3(0f, -0f, 132.2398f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 292119, 1803116220u, new Vector3(1312.98755f, 4364.94f, 40.382618f), new Vector3(0f, -0f, 159.314011f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 251671, 1803116220u, new Vector3(1307.8429f, 4338.977f, 38.22436f), new Vector3(0f, -0f, 164.943573f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 45848, 1803116220u, new Vector3(1305.98438f, 4316.383f, 37.14092f), new Vector3(0f, -0f, 97.94638f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 514838, 1803116220u, new Vector3(1307.80127f, 4339.309f, 38.2630577f), new Vector3(0f, 0f, 77.62705f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 52504, 1803116220u, new Vector3(1313.01855f, 4364.69336f, 40.3666267f), new Vector3(0f, 0f, 76.2959442f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 167441, 1803116220u, new Vector3(1316.50024f, 4378.99268f, 40.78285f), new Vector3(0f, 0f, 70.9729462f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "The Lost MC Male"), 331800, 1330042375u, new Vector3(1316.64063f, 4380.167f, 41.43069f), new Vector3(0f, -0f, 168.676941f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 541209, 1803116220u, new Vector3(1309.36133f, 4277.836f, 33.2431641f), new Vector3(0f, -0f, 170.774887f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 263447, 1803116220u, new Vector3(1302.51245f, 4235.56836f, 33.2431641f), new Vector3(0f, -0f, 168.419952f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 623896, 1803116220u, new Vector3(1297.83313f, 4216.95f, 33.2431641f), new Vector3(0f, -0f, 166.781708f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 67098, 1803116220u, new Vector3(1302.47534f, 4235.8457f, 33.2431641f), new Vector3(0f, -0f, 167.855972f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 700691, 1803116220u, new Vector3(1309.36731f, 4277.70166f, 33.2431679f), new Vector3(0f, 0f, -8.565462f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "The Lost MC Male"), 172570, 1330042375u, new Vector3(1309.77356f, 4280.12549f, 33.90356f), new Vector3(0f, -0f, 167.394424f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 277274, 1803116220u, new Vector3(1329.49377f, 4328.781f, 37.3514328f), new Vector3(0f, -0f, -102.730927f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 458008, 1803116220u, new Vector3(1351.35352f, 4328.77148f, 37.5715866f), new Vector3(0f, -0f, -92.6991348f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 781338, 1803116220u, new Vector3(1379.86633f, 4331.579f, 40.7838249f), new Vector3(0f, 0f, -88.45093f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 523546, 1803116220u, new Vector3(1392.54138f, 4343.7085f, 42.4554863f), new Vector3(0f, 0f, -58.1511841f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 110868, 1803116220u, new Vector3(1399.31042f, 4354.764f, 41.8477821f), new Vector3(0f, 0f, -51.59958f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 2321, 1803116220u, new Vector3(1392.57361f, 4343.383f, 42.44486f), new Vector3(0f, -0f, -125.504745f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 337179, 1803116220u, new Vector3(1379.81519f, 4331.734f, 40.800312f), new Vector3(0f, -0f, -150.327271f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 544793, 1803116220u, new Vector3(1351.149f, 4328.851f, 37.7954674f), new Vector3(0f, -0f, -160.051315f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 617499, 1803116220u, new Vector3(1329.2616f, 4328.7627f, 37.360878f), new Vector3(0f, -0f, -175.917282f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 430107, 1803116220u, new Vector3(1289.45264f, 4329.629f, 37.80445f), new Vector3(0f, -0f, 177.685989f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "The Lost MC Male"), 153884, 1330042375u, new Vector3(1288.65173f, 4329.546f, 38.4911957f), new Vector3(0f, -0f, -95.97082f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 106013, 1803116220u, new Vector3(1344.31689f, 4327.652f, 37.4699554f), new Vector3(0f, -0f, 173.642639f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 620571, 1803116220u, new Vector3(1343.04541f, 4312.314f, 37.31054f), new Vector3(0f, -0f, 174.512436f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 687644, 1803116220u, new Vector3(1341.79065f, 4296.37744f, 36.4549179f), new Vector3(0f, -0f, 174.870529f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 453660, 1803116220u, new Vector3(1343.067f, 4313.058f, 37.311615f), new Vector3(0f, -0f, 174.409683f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 259353, 1803116220u, new Vector3(1344.31738f, 4328.077f, 37.4764175f), new Vector3(0f, -0f, 173.436981f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 409885, 1803116220u, new Vector3(1344.92993f, 4336.53f, 37.4519f), new Vector3(0f, -0f, 168.727921f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "The Lost MC Male"), 395037, 1330042375u, new Vector3(1345.10193f, 4337.30957f, 38.1463165f), new Vector3(0f, -0f, 168.983658f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 17438, 1803116220u, new Vector3(1366.17053f, 4298.84131f, 36.5889244f), new Vector3(0f, 0f, -39.1186676f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 772377, 1803116220u, new Vector3(1372.17786f, 4306.958f, 37.3339272f), new Vector3(0f, 0f, -38.8625259f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 239133, 1803116220u, new Vector3(1374.32324f, 4315.248f, 37.26851f), new Vector3(0f, 0f, 41.85002f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 75549, 1803116220u, new Vector3(1366.57f, 4322.2583f, 37.4469528f), new Vector3(0f, 0f, 44.9719658f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 375070, 1803116220u, new Vector3(1353.55469f, 4325.978f, 37.3662529f), new Vector3(0f, 0f, 56.59f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 304670, 1803116220u, new Vector3(1366.64941f, 4322.179f, 37.4460945f), new Vector3(0f, 0f, 44.971756f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 560669, 1803116220u, new Vector3(1374.26062f, 4315.07129f, 37.2606621f), new Vector3(0f, 0f, 42.8220444f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 204823, 1803116220u, new Vector3(1372.20862f, 4307.308f, 37.3595963f), new Vector3(0f, 0f, 81.61706f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 160023, 1803116220u, new Vector3(1366.01648f, 4299.2666f, 36.6574f), new Vector3(0f, -0f, 116.317406f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 456478, 1803116220u, new Vector3(1357.50989f, 4298.603f, 36.6238747f), new Vector3(0f, -0f, 108.588852f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "The Lost MC Male"), 255770, 1330042375u, new Vector3(1356.73572f, 4298.31641f, 37.2347069f), new Vector3(0f, 0f, -70.34027f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 590878, 1803116220u, new Vector3(1348.32471f, 4352.50928f, 42.9149437f), new Vector3(0f, 0f, -39.0692177f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 772895, 1803116220u, new Vector3(1356.416f, 4362.388f, 43.5950356f), new Vector3(0f, 0f, -39.22267f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 194336, 1803116220u, new Vector3(1367.66736f, 4383.10059f, 43.7001724f), new Vector3(0f, 0f, -33.59269f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 438559, 1803116220u, new Vector3(1356.52771f, 4362.46f, 43.6443443f), new Vector3(0f, -0f, 142.060364f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 184608, 1803116220u, new Vector3(1348.189f, 4352.67725f, 42.9112778f), new Vector3(0f, -0f, 140.933563f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 158241, 1803116220u, new Vector3(1342.311f, 4349.54834f, 42.539f), new Vector3(0f, 0f, -1.35022044f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "The Lost MC Male"), 547103, 1330042375u, new Vector3(1341.91406f, 4349.161f, 43.1082039f), new Vector3(0f, 0f, -39.5823f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 294171, 1803116220u, new Vector3(1424.62f, 4376.515f, 43.54617f), new Vector3(0f, 0f, -37.8933334f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 369184, 1803116220u, new Vector3(1433.64575f, 4388.037f, 43.9885025f), new Vector3(0f, 0f, -37.99558f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 477216, 1803116220u, new Vector3(1424.41431f, 4376.46f, 43.71726f), new Vector3(0f, 0f, -37.17656f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 366113, 1803116220u, new Vector3(1420.37683f, 4371.46875f, 43.4735374f), new Vector3(0f, 0f, -34.3103142f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "The Lost MC Male"), 618274, 1330042375u, new Vector3(1420.04163f, 4370.74756f, 44.1187057f), new Vector3(0f, 0f, -36.6133728f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 125984, 1803116220u, new Vector3(1338.79858f, 4225.596f, 33.2500572f), new Vector3(0f, -0f, 99.52714f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 482339, 1803116220u, new Vector3(1318.72632f, 4229.69238f, 33.25006f), new Vector3(0f, 0f, 83.30234f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 571680, 1803116220u, new Vector3(1302.32471f, 4232.441f, 33.2431641f), new Vector3(0f, 0f, 82.1763f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 341282, 1803116220u, new Vector3(1318.83765f, 4229.535f, 33.2500572f), new Vector3(0f, 0f, 83.558136f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 244249, 1803116220u, new Vector3(1338.99316f, 4225.677f, 33.5362549f), new Vector3(0f, -0f, 99.0657654f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "The Lost MC Male"), 342818, 1330042375u, new Vector3(1340.96582f, 4225.421f, 33.91001f), new Vector3(0f, 0f, 87.9591751f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 609057, 1803116220u, new Vector3(1382.16614f, 4344.56641f, 42.4105f), new Vector3(0f, -0f, 115.0836f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 748068, 1803116220u, new Vector3(1361.59949f, 4336.09326f, 38.5080757f), new Vector3(0f, 0f, 18.2487183f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 192548, 1803116220u, new Vector3(1338.651f, 4335.40869f, 37.24814f), new Vector3(0f, 0f, 65.7958f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 685348, 1803116220u, new Vector3(1310.62842f, 4334.73828f, 37.8640251f), new Vector3(0f, 0f, 78.1814041f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 247843, 1803116220u, new Vector3(1346.33069f, 4336.31641f, 37.4376526f), new Vector3(0f, 0f, 56.07099f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 466724, 1803116220u, new Vector3(1364.0437f, 4336.319f, 38.8368454f), new Vector3(0f, 0f, 7.85833645f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 289059, 1803116220u, new Vector3(1381.93713f, 4344.184f, 42.3752136f), new Vector3(0f, 0f, -37.48809f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "The Lost MC Male"), 726052, 1330042375u, new Vector3(1385.63574f, 4348.898f, 43.0842628f), new Vector3(0f, -0f, 138.267334f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			})
		};

		public static MissionOjects OneilsFarmObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 118823, 865563579u, new Vector3(2445.00317f, 4971.79053f, 46.8396568f), new Vector3(0f, 0f, 21.65749f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 99363, 865563579u, new Vector3(2444.82349f, 4971.834f, 46.8396568f), new Vector3(0f, 0f, -26.55507f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 310311, 865563579u, new Vector3(2443.60913f, 4977.681f, 46.8438339f), new Vector3(0f, 0f, -44.41716f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 555559, 865563579u, new Vector3(2443.52832f, 4977.78955f, 46.8438339f), new Vector3(0f, 0f, -36.9958458f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 338983, 865563579u, new Vector3(2440.32837f, 4983.48828f, 46.84574f), new Vector3(0f, -0f, 135.4332f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 611623, 865563579u, new Vector3(2441.87378f, 4986.886f, 46.8462563f), new Vector3(0f, 0f, -13.7084846f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 8476, 865563579u, new Vector3(2442.03247f, 4986.871f, 46.8462563f), new Vector3(0f, 0f, -21.64153f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 735526, 865563579u, new Vector3(2442.08984f, 4986.66f, 46.8462563f), new Vector3(0f, 0f, -29.36985f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 474920, 865563579u, new Vector3(2439.53979f, 4968.311f, 51.2456055f), new Vector3(0f, -0f, 169.869522f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 249383, 865563579u, new Vector3(2439.388f, 4968.416f, 51.2456055f), new Vector3(0f, -0f, 158.660431f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 304169, 865563579u, new Vector3(2439.206f, 4968.40137f, 51.2456055f), new Vector3(0f, -0f, 148.731f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 358690, 865563579u, new Vector3(2433.4292f, 4970.009f, 46.7437439f), new Vector3(0f, 0f, -64.83826f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 19498, 865563579u, new Vector3(2433.27881f, 4970.17f, 46.7437439f), new Vector3(0f, 0f, -58.4917564f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 15400, 865563579u, new Vector3(2433.47461f, 4969.841f, 46.7437439f), new Vector3(0f, 0f, -70.26329f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 439337, 865563579u, new Vector3(2433.502f, 4969.734f, 46.7437439f), new Vector3(0f, 0f, -73.743515f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 695596, 865563579u, new Vector3(2445.38232f, 4984.47852f, 46.84399f), new Vector3(0f, -0f, 93.36478f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 199213, 865563579u, new Vector3(2445.48486f, 4984.62646f, 46.84399f), new Vector3(0f, 0f, 88.8606f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 297768, 865563579u, new Vector3(2445.455f, 4984.796f, 46.84399f), new Vector3(0f, 0f, 83.38411f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static List<MissionOjects> OneilsFarmNavs = new List<MissionOjects>
		{
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 506672, 1803116220u, new Vector3(2460.019f, 4983.44238f, 45.3499565f), new Vector3(0f, -0f, 130.3882f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 320816, 1803116220u, new Vector3(2467.61523f, 4973.86768f, 44.8283424f), new Vector3(0f, -0f, 130.3882f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 683570, 1803116220u, new Vector3(2453.50317f, 4960.5835f, 44.65796f), new Vector3(0f, 0f, 40.872097f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 561458, 1803116220u, new Vector3(2444.95044f, 4952.052f, 44.3644333f), new Vector3(0f, 0f, 42.35633f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 227891, 1803116220u, new Vector3(2453.81616f, 4960.3916f, 44.6235428f), new Vector3(0f, 0f, 42.6634026f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 615982, 1803116220u, new Vector3(2467.80249f, 4974.24951f, 44.8238449f), new Vector3(0f, 0f, 52.43883f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 566323, 1803116220u, new Vector3(2468.24316f, 4977.68457f, 45.7841949f), new Vector3(0f, 0f, 54.8953133f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Hillbilly Male 2"), 557616, 2064532783u, new Vector3(2458.718f, 4985.344f, 46.3652954f), new Vector3(0f, -0f, -137.188675f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 741172, 1803116220u, new Vector3(2448.49829f, 4998.49f, 45.3467255f), new Vector3(0f, 0f, -78.22678f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 313397, 1803116220u, new Vector3(2440.415f, 4990.851f, 45.6241722f), new Vector3(0f, -0f, -143.584579f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 404277, 1803116220u, new Vector3(2431.59326f, 4981.317f, 45.22217f), new Vector3(0f, -0f, 161.242035f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 309549, 1803116220u, new Vector3(2422.17725f, 4970.18066f, 45.44273f), new Vector3(0f, -0f, 150.800751f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 322097, 1803116220u, new Vector3(2417.32178f, 4963.225f, 45.3664322f), new Vector3(0f, -0f, 149.521027f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 601909, 1803116220u, new Vector3(2431.72217f, 4981.52734f, 45.23629f), new Vector3(0f, -0f, 161.343231f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 580406, 1803116220u, new Vector3(2440.85254f, 4990.86963f, 45.62587f), new Vector3(0f, -0f, -140.41272f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 204083, 1803116220u, new Vector3(2448.17163f, 4998.454f, 45.3514f), new Vector3(0f, 0f, -78.0227051f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Hillbilly Male 2"), 415285, 2064532783u, new Vector3(2448.75439f, 4999.2334f, 46.01085f), new Vector3(0f, -0f, 150.032547f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 718172, 1803116220u, new Vector3(2428.267f, 4986.843f, 45.40967f), new Vector3(0f, 0f, -45.1728554f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 586053, 1803116220u, new Vector3(2435.45117f, 4993.77441f, 45.5282822f), new Vector3(0f, 0f, -45.68455f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 308832, 1803116220u, new Vector3(2446.2146f, 5004.165f, 45.22818f), new Vector3(0f, 0f, -45.838028f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 147552, 1803116220u, new Vector3(2435.26f, 4993.589f, 45.5234032f), new Vector3(0f, 0f, -45.68437f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Hillbilly Male 2"), 785498, 2064532783u, new Vector3(2427.634f, 4986.214f, 46.05185f), new Vector3(0f, 0f, -45.2235832f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Hillbilly Male 2"), 152155, 2064532783u, new Vector3(2441.4043f, 4966.05957f, 46.80517f), new Vector3(0f, 0f, 37.62982f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Hillbilly Male 2"), 102239, 2064532783u, new Vector3(2428.27148f, 4966.04834f, 46.93113f), new Vector3(0f, 0f, -44.2005653f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 212318, 1803116220u, new Vector3(2426.54224f, 4955.46631f, 45.19233f), new Vector3(0f, -0f, -133.35611f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 416351, 1803116220u, new Vector3(2434.222f, 4948.174f, 44.9667664f), new Vector3(0f, -0f, -133.407059f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 122716, 1803116220u, new Vector3(2426.43457f, 4955.333f, 45.1807632f), new Vector3(0f, -0f, -133.816269f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 627542, 1803116220u, new Vector3(2417.75635f, 4963.58936f, 45.3978424f), new Vector3(0f, -0f, -134.123154f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Hillbilly Male 2"), 634977, 2064532783u, new Vector3(2416.975f, 4963.61768f, 46.04521f), new Vector3(0f, -0f, -133.047958f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Hillbilly Male 2"), 194147, 2064532783u, new Vector3(2457.846f, 4971.37158f, 46.567215f), new Vector3(0f, -0f, -133.046234f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Hillbilly Male 2"), 550755, 2064532783u, new Vector3(2456.414f, 4981.69043f, 46.8150063f), new Vector3(0f, -0f, 172.885773f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Hillbilly Male 2"), 668515, 2064532783u, new Vector3(2439.65381f, 4979.31836f, 51.57276f), new Vector3(0f, 0f, 62.99172f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Hillbilly Male 2"), 200544, 2064532783u, new Vector3(2444.66748f, 4965.42627f, 51.57283f), new Vector3(0f, -0f, -138.653946f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Hillbilly Male 2"), 231782, 2064532783u, new Vector3(2461.43945f, 4976.941f, 51.57232f), new Vector3(-2.12964665E-07f, 1.86563923E-07f, -56.76253f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Hillbilly Male 2"), 317282, 2064532783u, new Vector3(2485.62549f, 4953.66162f, 44.82021f), new Vector3(0f, -0f, 135.5271f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Hillbilly Male 2"), 504417, 2064532783u, new Vector3(2453.01685f, 4956.829f, 44.92002f), new Vector3(0f, -0f, -149.644119f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 508258, 1803116220u, new Vector3(2475.49365f, 4964.44f, 44.474926f), new Vector3(0f, 0f, -50.3029f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 729701, 1803116220u, new Vector3(2475.84f, 4956.039f, 44.4091034f), new Vector3(0f, -0f, -94.9838257f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 8806, 1803116220u, new Vector3(2482.46582f, 4947.277f, 44.08661f), new Vector3(0f, -0f, -120.778923f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 516706, 1803116220u, new Vector3(2488.43213f, 4941.54639f, 43.51312f), new Vector3(0f, -0f, -124.770927f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 730471, 1803116220u, new Vector3(2481.76758f, 4947.46729f, 44.1427345f), new Vector3(0f, -0f, -121.239296f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 357222, 1803116220u, new Vector3(2475.6543f, 4955.8877f, 44.4067764f), new Vector3(0f, -0f, -95.90436f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 642146, 1803116220u, new Vector3(2475.18262f, 4964.169f, 44.5116577f), new Vector3(0f, 0f, -50.3530045f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Hillbilly Male 2"), 590697, 2064532783u, new Vector3(2475.36084f, 4965.07471f, 45.2209663f), new Vector3(0f, -0f, 178.265259f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			})
		};

		public static MissionOjects TrainOfficeObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_trainkey_01a"), 495628, 1977600814u, new Vector3(-1026.22375f, -436.231659f, 67.4661255f), new Vector3(-3.05333233E-13f, 1.40285863E-06f, -22.637825f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_mp_h_acc_rugwooll_03"), 5391, 2826327287u, new Vector3(-1023.80566f, -430.964935f, 63.5615959f), new Vector3(90f, -63.58291f, 0f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_lobby_pillar_03a"), 337684, 2689055672u, new Vector3(-1024.65356f, -429.283356f, 62.8660622f), new Vector3(0f, 0f, -62.0460625f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_lobby_pillar_03a"), 780565, 2689055672u, new Vector3(-1022.99884f, -432.4015f, 62.8660622f), new Vector3(-0f, -0f, -62.0460625f), 337684, 0, new Vector3(3.529997f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Dock Worker"), 450598, 2255894993u, new Vector3(-1024.40894f, -439.731f, 67.66371f), new Vector3(0f, 0f, -44.9356842f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Dock Worker"), 85030, 2255894993u, new Vector3(-1009.41467f, -426.472321f, 63.8567238f), new Vector3(0f, -0f, 118.791344f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Dock Worker"), 526631, 2255894993u, new Vector3(-1017.152f, -420.5453f, 72.465065f), new Vector3(0f, -0f, 148.373169f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Dock Worker"), 242213, 2255894993u, new Vector3(-1015.85242f, -436.465851f, 72.46485f), new Vector3(0f, 0f, 34.86196f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Dock Worker"), 527649, 2255894993u, new Vector3(-1008.55872f, -426.062256f, 72.46487f), new Vector3(0f, 0f, 83.1761f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Dock Worker"), 482343, 2255894993u, new Vector3(-1016.23761f, -423.1172f, 67.6640244f), new Vector3(0f, -0f, 133.89505f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Dock Worker"), 492841, 2255894993u, new Vector3(-1030.77051f, -429.7627f, 72.46478f), new Vector3(0f, 0f, -71.7416f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects KeycardObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_trainkey_01a"), 408087, 1977600814u, new Vector3(-1018.81195f, -434.566345f, 72.417305f), new Vector3(0f, 0f, -58.3024445f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_trainkey_01a"), 59160, 1977600814u, new Vector3(-1021.543f, -425.511963f, 72.4112549f), new Vector3(0f, 0f, -23.499157f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_trainkey_01a"), 292888, 1977600814u, new Vector3(-1025.58411f, -421.555f, 72.26605f), new Vector3(0f, 0f, -48.7824326f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_trainkey_01a"), 521496, 1977600814u, new Vector3(-1019.54846f, -420.633484f, 72.26605f), new Vector3(0f, -0f, 97.90255f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_trainkey_01a"), 234519, 1977600814u, new Vector3(-1016.90906f, -429.887177f, 67.42095f), new Vector3(0f, 0f, 30.6576061f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_trainkey_01a"), 579098, 1977600814u, new Vector3(-1020.5788f, -420.547058f, 67.4803f), new Vector3(0f, 0f, -28.5082226f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_trainkey_01a"), 347931, 1977600814u, new Vector3(-1011.089f, -420.552032f, 67.4661255f), new Vector3(0f, 0f, 26.7671547f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_trainkey_01a"), 6171, 1977600814u, new Vector3(-1032.19153f, -422.8869f, 72.26612f), new Vector3(0f, 0f, 82.19541f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_trainkey_01a"), 651804, 1977600814u, new Vector3(-1013.20367f, -414.653168f, 72.41605f), new Vector3(0f, 0f, -80.35539f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects LockedDoorsObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_keypad_02"), 655896, 1407828617u, new Vector3(2635.86255f, 2934.72168f, 45.21076f), new Vector3(0f, -0f, 154.677979f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_keypad_02"), 772628, 1407828617u, new Vector3(2622.53076f, 1694.157f, 32.23562f), new Vector3(0f, -0f, -179.401855f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_iaa_base_door_01"), 240697, 4147641866u, new Vector3(2635.858f, 2934.55933f, 44.9129753f), new Vector3(0f, 0f, -30.3814354f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_iaa_base_door_01"), 281139, 4147641866u, new Vector3(2622.59229f, 1694.01123f, 32.02213f), new Vector3(0f, 0f, 1.08992338f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects GrinderObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 550178, 2271741578u, new Vector3(-175.192535f, -1003.00061f, 114.13636f), new Vector3(0f, 0f, -84.8793945f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 624227, 2271741578u, new Vector3(-177.225037f, -994.8583f, 114.146828f), new Vector3(0f, 0f, 11.750598f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 758020, 2271741578u, new Vector3(-167.555634f, -993.4776f, 114.303993f), new Vector3(0f, 0f, 11.289979f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 241160, 2271741578u, new Vector3(-164.4164f, -990.414063f, 114.162071f), new Vector3(0f, 0f, 1.05374718f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 710719, 2271741578u, new Vector3(-156.061447f, -997.8252f, 114.239738f), new Vector3(0f, 0f, 71.2228f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 201788, 2271741578u, new Vector3(-162.430984f, -1004.8006f, 114.162071f), new Vector3(0f, -0f, 130.899567f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 140116, 2271741578u, new Vector3(-158.0955f, -1017.08191f, 114.162071f), new Vector3(0f, -0f, 149.887024f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 488467, 2271741578u, new Vector3(-152.085983f, -1020.317f, 113.872383f), new Vector3(0f, -0f, 103.617943f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 639752, 2271741578u, new Vector3(-151.984528f, -984.5647f, 114.162071f), new Vector3(0f, 0f, 37.1846924f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 668963, 2271741578u, new Vector3(-148.5782f, -977.0642f, 114.318771f), new Vector3(0f, 0f, 33.7043266f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 616288, 2271741578u, new Vector3(-143.182724f, -969.7111f, 114.318771f), new Vector3(0f, 0f, 47.420723f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 90192, 2271741578u, new Vector3(-136.1424f, -966.863464f, 114.141716f), new Vector3(0f, 0f, 2.99552584f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 213863, 2271741578u, new Vector3(-138.085f, -958.525f, 114.164215f), new Vector3(0f, 0f, 16.6097469f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 525126, 2271741578u, new Vector3(-153.040924f, -953.227539f, 114.146828f), new Vector3(0f, 0f, 67.48365f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 219912, 2271741578u, new Vector3(-159.51326f, -982.8059f, 114.301f), new Vector3(0f, -0f, -135.74852f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 337705, 2271741578u, new Vector3(-163.880173f, -989.7795f, 114.13636f), new Vector3(0f, -0f, -175.208939f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 123909, 2271741578u, new Vector3(-175.452454f, -990.383545f, 114.301f), new Vector3(0f, -0f, -167.172989f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static Vector3 LostMC_CompountLoc = new Vector3(1350.91f, 4357.684f, 44.3089f);

		public static Vector3 OneilsFarmLoc = new Vector3(2444.424f, 4974.523f, 51.13515f);

		public static Vector3 SwapTrainRouteQuarry = new Vector3(2632.059f, 2934.051f, 43.70516f);

		public static Vector3 SwapTrainRoutePowerPlant = new Vector3(2618.774f, 1691.839f, 30.86389f);

		public static Vector3 TrainEngineerOfficeEnter = new Vector3(-1001.92f, -414.39f, 38.6f);

		public static Vector3 TrainEngineerOfficeExit = new Vector3(-1033.195f, -434.58f, 62.8f);

		public static Vector3 GrinderIntelLoc = new Vector3(-160.06f, -990.12f, 113f);

		public static Vector3 LeverMainLoc = new Vector3(2632f, 2933f, 0f);

		public static Vector3 LeverSecondaryLoc = new Vector3(2618.774f, 1691.839f, 0f);

		public static MissionOjects TrainCrashObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Freight Train"), 753170, 184361638u, new Vector3(-510.320465f, 4375.81445f, 90.4118042f), new Vector3(8.681129f, -2.51410818f, 87.0611343f), 41, 0, 41, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Freight Train"), 46098, 184361638u, new Vector3(-511.173431f, 4409.55566f, 89.3712845f), new Vector3(0.8915045f, 8.595975f, 130.557617f), 18, 0, 18, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Freight Train"), 339475, 184361638u, new Vector3(-517.2736f, 4418.50439f, 90.50519f), new Vector3(13.0052395f, 0.06855911f, 68.05005f), 47, 0, 47, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_02a"), 615958, 2104596125u, new Vector3(-521.222351f, 4420.095f, 91.18056f), new Vector3(13.0052395f, 0.06855911f, 68.05005f), 339475, 0, new Vector3(0f, 4.29999971f, -0.299999982f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_02a"), 331803, 2104596125u, new Vector3(-513.4504f, 4416.96338f, 89.24521f), new Vector3(-13.00524f, -0.0685543f, -111.949921f), 339475, 0, new Vector3(0f, -4.29999828f, -0.299999982f), new Vector3(0f, 0f, -179.999985f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_02a"), 270874, 2104596125u, new Vector3(-514.3384f, 4406.788f, 89.14004f), new Vector3(0.8915045f, 8.595975f, 130.557617f), 46098, 0, new Vector3(0f, 4.19999838f, -0.299999982f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_02a"), 484891, 2104596125u, new Vector3(-507.9572f, 4412.249f, 89.009346f), new Vector3(-0.891508341f, -8.595975f, -49.4423561f), 46098, 0, new Vector3(0f, -4.19999838f, -0.299999982f), new Vector3(0f, 0f, -179.999985f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_02a"), 310299, 2104596125u, new Vector3(-514.5114f, 4376.043f, 90.74945f), new Vector3(8.681129f, -2.51410818f, 87.0611343f), 753170, 0, new Vector3(0f, 4.19999838f, -0.299999982f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_02a"), 294166, 2104596125u, new Vector3(-506.218536f, 4375.617f, 89.4816f), new Vector3(-8.6811285f, 2.51411128f, -92.9388351f), 753170, -1, new Vector3(0f, -4.19999838f, -0.299999982f), new Vector3(0f, 0f, -179.999985f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_02a"), 525858, 2104596125u, new Vector3(-509.410034f, 4429.704f, 88.756134f), new Vector3(-4.318869f, -2.51410723f, -105.721245f), 75555, 0, new Vector3(0f, 4.19999838f, -0.299999982f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_02a"), 673314, 2104596125u, new Vector3(-517.472839f, 4431.97363f, 89.38872f), new Vector3(4.31887054f, 2.514105f, 74.27878f), 75555, -1, new Vector3(0f, -4.19999838f, -0.299999982f), new Vector3(0f, 0f, -179.999985f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Freight Train"), 75555, 184361638u, new Vector3(-513.416138f, 4430.845f, 89.3712845f), new Vector3(-4.318869f, -2.51410723f, -105.721245f), 41, 0, 41, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_02a"), 455974, 2104596125u, new Vector3(-493.339722f, 4397.994f, 31.61985f), new Vector3(-4.31888056f, -2.514103f, -108.83799f), 387365, 0, new Vector3(0f, 4.19999838f, -0.299999982f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_02a"), 483109, 2104596125u, new Vector3(-501.2672f, 4400.69873f, 32.25243f), new Vector3(4.318881f, 2.514101f, 71.16203f), 387365, -1, new Vector3(0f, -4.19999838f, -0.299999982f), new Vector3(0f, 0f, -179.999985f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Freight Train"), 387365, 184361638u, new Vector3(-497.277863f, 4399.35156f, 32.235f), new Vector3(-4.31888056f, -2.514103f, -108.83799f), 41, 0, 41, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static bool MainTrackSwitch;

		public static bool SecondaryTrackSwitch;

		public Prop iLocal_910;

		public Prop iLocal_911;

		public static bool ResetTrainAfterEnteringQuarry;

		public static bool SwappedTrainTrack;

		public static bool HasSwappedTrainRouteToTrainYard;

		public static bool HasSwappedTrainRouteToEnterQuarry;

		public static bool TrainIsStoppingAtYard;

		public static List<Vector3> TrainSpawnCoords = new List<Vector3>
		{
			new Vector3(2724.25f, 3127.04f, 42.6809f),
			new Vector3(2611.01f, 1661.71f, 26.4093f),
			new Vector3(1952.24f, 1897.95f, 66.7899f),
			new Vector3(516.535f, 3190.45f, 41.9281f),
			new Vector3(-396.292f, 3896.78f, 76.4003f),
			new Vector3(2390.53f, 3838.05f, 37.474f),
			new Vector3(2888.44f, 3729.71f, 43.1929f),
			new Vector3(2899.25f, 4839.8f, 62.0683f),
			new Vector3(2446.18f, 5813.55f, 59.58f),
			new Vector3(2802.08f, 3295.9f, 44.9836f),
			new Vector3(2455.91f, 2452.9f, 40.346f)
		};

		public static int PlaceBombScene;

		public static int PlacedBombStage;

		public static bool PlacedC4AtLocation1;

		public static bool PlacedC4AtLocation2;

		public static bool PlacedC4AtLocation3;

		public static Prop MainC4Prop;

		public static bool StoppedTrainUsingC4;

		public static List<Prop> TrainNodeBombs = new List<Prop>();

		public static int CheckTrainNodeBombs;

		public static List<Vector3> TrainNodes = new List<Vector3>
		{
			new Vector3(2409.355f, 3847.355f, 39.34928f),
			new Vector3(680.5977f, -1256.063f, 25.0268f),
			new Vector3(690.569f, -1392.745f, 24.12667f),
			new Vector3(836.1763f, -1664.047f, 20.91438f),
			new Vector3(783.5282f, -2013.508f, 21.2321f),
			new Vector3(760.2407f, -2269.03f, 21.13881f),
			new Vector3(488.7349f, -2617.626f, 13.14309f),
			new Vector3(217.5038f, -2519.408f, 7.035395f),
			new Vector3(164.6168f, -2110.95f, 16.32013f),
			new Vector3(213.5001f, -1929.604f, 23.52823f),
			new Vector3(413.4453f, -1691.279f, 29.7945f),
			new Vector3(655.2806f, -1271.481f, 23.35835f),
			new Vector3(669.273f, -1096.79f, 24.113f),
			new Vector3(669.2761f, -952.9529f, 22.66184f),
			new Vector3(664.616f, -884.7941f, 23.13215f),
			new Vector3(670.3511f, -720.8022f, 25.01263f),
			new Vector3(669.5583f, -668.7302f, 26.03423f),
			new Vector3(723.3917f, -540.8462f, 27.33015f),
			new Vector3(811.2823f, -471.8155f, 30.24261f),
			new Vector3(1001.712f, -497.9883f, 33.35184f),
			new Vector3(1101.008f, -505.3965f, 34.57522f),
			new Vector3(1504.755f, -967.4773f, 66.52593f),
			new Vector3(1612.329f, -917.3607f, 74.99297f),
			new Vector3(1738.869f, -826.8101f, 87.25734f),
			new Vector3(1830.732f, -771.6052f, 95.09778f),
			new Vector3(1971.281f, -748.3033f, 97.75032f),
			new Vector3(2049.069f, -717.3406f, 97.71058f),
			new Vector3(2124.759f, -658.2141f, 97.42986f),
			new Vector3(2199.417f, -597.7468f, 96.97811f),
			new Vector3(2449.39f, -306.4421f, 94.31933f),
			new Vector3(2481.341f, -270.3248f, 94.06274f),
			new Vector3(2573.772f, -88.35106f, 94.13375f),
			new Vector3(2575.907f, -69.88692f, 94.18323f),
			new Vector3(2615.729f, 43.21269f, 94.43325f),
			new Vector3(2658.12f, 198.774f, 94.91172f),
			new Vector3(2668.698f, 384.5321f, 95.37401f),
			new Vector3(2677.544f, 486.2038f, 95.17033f),
			new Vector3(2654.616f, 664.9323f, 93.2771f),
			new Vector3(2664.888f, 758.4712f, 85.11991f),
			new Vector3(2623.263f, 1027.303f, 63.93083f),
			new Vector3(2612.012f, 1278.03f, 42.22602f),
			new Vector3(2610.73f, 1499.75f, 31.55189f),
			new Vector3(2610.918f, 1613.72f, 28.91346f),
			new Vector3(2611.05f, 1715.701f, 27.18185f),
			new Vector3(2611.255f, 1841.7f, 27.61004f),
			new Vector3(2611.458f, 1994.64f, 31.68243f),
			new Vector3(2603.425f, 2113.915f, 32.91561f),
			new Vector3(2502.795f, 2304.523f, 34.60692f),
			new Vector3(2469.264f, 2400.643f, 39.63413f),
			new Vector3(2446.945f, 2506.116f, 42.85609f),
			new Vector3(2441.451f, 2610.826f, 41.82346f),
			new Vector3(2469.187f, 2723.384f, 39.00424f),
			new Vector3(2545.521f, 2838.194f, 39.09233f),
			new Vector3(2674.507f, 3017.362f, 42.15122f),
			new Vector3(2735.671f, 3109.88f, 45.89397f),
			new Vector3(2799.118f, 3229.095f, 50.59462f),
			new Vector3(2842.766f, 3327.841f, 53.3313f),
			new Vector3(2887.574f, 3439.12f, 55.76265f),
			new Vector3(2939.623f, 3620.026f, 55.55696f),
			new Vector3(2959.891f, 3719.983f, 55.54211f),
			new Vector3(2975.011f, 3811.746f, 55.64935f),
			new Vector3(2986.097f, 3898.033f, 55.77171f),
			new Vector3(2995.494f, 3999.589f, 56.47933f),
			new Vector3(3011.028f, 4139.701f, 58.94398f),
			new Vector3(3023.045f, 4256.069f, 60.83938f),
			new Vector3(3037.478f, 4387.261f, 63.1802f),
			new Vector3(3042.777f, 4518.961f, 63.39468f),
			new Vector3(3015.721f, 4647.897f, 63.34291f),
			new Vector3(2996.855f, 4688.644f, 63.33012f),
			new Vector3(2933.364f, 4786.896f, 63.53801f),
			new Vector3(2889.753f, 4855.155f, 63.77546f),
			new Vector3(2685.655f, 5295.063f, 62.8586f),
			new Vector3(2632.653f, 5444.964f, 62.75574f),
			new Vector3(2589.14f, 5556.747f, 62.36561f),
			new Vector3(2551.211f, 5628.304f, 62.0235f),
			new Vector3(2491.886f, 5736.052f, 61.51904f),
			new Vector3(2462.048f, 5788.101f, 61.31355f),
			new Vector3(2404.245f, 5875.723f, 61.21251f),
			new Vector3(2360.294f, 5928.837f, 61.40683f),
			new Vector3(2268.555f, 6015.159f, 61.77194f),
			new Vector3(2208.105f, 6064.417f, 61.78407f),
			new Vector3(2121.165f, 6123.226f, 61.20998f),
			new Vector3(1709.094f, 6329.724f, 45.83703f),
			new Vector3(1464.805f, 6401.33f, 35.47356f),
			new Vector3(1240.799f, 6431.959f, 32.43484f),
			new Vector3(1093.84f, 6435.195f, 32.64763f),
			new Vector3(940.8427f, 6436.64f, 32.762f),
			new Vector3(766.8447f, 6437.48f, 32.81081f),
			new Vector3(511.8458f, 6437.949f, 32.61544f),
			new Vector3(365.3483f, 6430.506f, 32.10448f),
			new Vector3(232.5724f, 6384.43f, 32.2908f),
			new Vector3(137.9979f, 6332.311f, 32.36706f),
			new Vector3(46.52836f, 6280.754f, 32.26023f),
			new Vector3(-35.92408f, 6231.614f, 32.26982f),
			new Vector3(-146.6872f, 6135.589f, 32.22514f),
			new Vector3(-229.0064f, 6052.468f, 33.69404f),
			new Vector3(-289.0288f, 5989.662f, 38.1697f),
			new Vector3(-339.7485f, 5934.686f, 43.69813f),
			new Vector3(-414.8801f, 5835.202f, 54.22075f),
			new Vector3(-435.5077f, 5714.681f, 62.22489f),
			new Vector3(-436.4413f, 5613.022f, 68.34377f),
			new Vector3(-431.8012f, 5523.291f, 72.75411f),
			new Vector3(-434.3235f, 5460.453f, 76.25135f),
			new Vector3(-444.6221f, 5374.281f, 81.58381f),
			new Vector3(-457.3299f, 5318.847f, 84.92753f),
			new Vector3(-478.8329f, 5253.382f, 87.92522f),
			new Vector3(-497.4566f, 5205.922f, 89.35883f),
			new Vector3(-444.6252f, 4073.121f, 85.07041f),
			new Vector3(-488.6866f, 4256.854f, 89.39474f),
			new Vector3(-394.5783f, 3891.029f, 77.80544f),
			new Vector3(-353.3977f, 3788.209f, 72.80466f),
			new Vector3(-304.4715f, 3702.461f, 67.94201f),
			new Vector3(-257.4121f, 3648.117f, 64.95654f),
			new Vector3(-150.2382f, 3547.901f, 57.54739f),
			new Vector3(-89.05327f, 3499.675f, 54.03105f),
			new Vector3(-4.616978f, 3442.587f, 51.27259f),
			new Vector3(80.45702f, 3391.975f, 50.04194f),
			new Vector3(361.0497f, 3234.598f, 45.30698f),
			new Vector3(504.7567f, 3192.735f, 43.67189f),
			new Vector3(644.6615f, 3176.618f, 42.84573f),
			new Vector3(800.2275f, 3185.429f, 42.48761f),
			new Vector3(1019.539f, 3219.676f, 41.21471f),
			new Vector3(1137.526f, 3241.577f, 40.6087f),
			new Vector3(1257.411f, 3268.979f, 40.17419f),
			new Vector3(1414.847f, 3318.144f, 39.58871f),
			new Vector3(1540.654f, 3367.045f, 39.44363f),
			new Vector3(1662.169f, 3425.755f, 39.62032f),
			new Vector3(1901.878f, 3562.542f, 39.29086f),
			new Vector3(2021.553f, 3631.254f, 39.21865f),
			new Vector3(2112.709f, 3683.37f, 39.1796f),
			new Vector3(2269.397f, 3771.966f, 39.05614f),
			new Vector3(2431.026f, 3857.658f, 39.63326f),
			new Vector3(2483.392f, 3880.132f, 40.40668f),
			new Vector3(2679.193f, 3910.438f, 43.72042f),
			new Vector3(2766.424f, 3879.655f, 44.82823f),
			new Vector3(2843.49f, 3814.106f, 44.78345f),
			new Vector3(2879.424f, 3751.944f, 44.85474f),
			new Vector3(2906.126f, 3623.108f, 44.92574f),
			new Vector3(2884.242f, 3511.616f, 46.59934f),
			new Vector3(2854.36f, 3426.842f, 47.3999f),
			new Vector3(2808.946f, 3312.536f, 46.75779f),
			new Vector3(2757.307f, 3194.341f, 45.73274f),
			new Vector3(2701.257f, 3081.527f, 43.42963f),
			new Vector3(2677.953f, 3022.275f, 42.30566f),
			new Vector3(2618.546f, 2949.494f, 40.81794f),
			new Vector3(2551.748f, 2857.13f, 39.66814f),
			new Vector3(2483.769f, 2773.361f, 38.68112f),
			new Vector3(2395.084f, 2703.088f, 42.06644f),
			new Vector3(2296.373f, 2659.737f, 47.11277f),
			new Vector3(2178.959f, 2614.398f, 52.73953f),
			new Vector3(2066.565f, 2546.394f, 57.55473f),
			new Vector3(1977.397f, 2442.025f, 60.64592f),
			new Vector3(1929.606f, 2319.583f, 61.31555f),
			new Vector3(1913.996f, 2170.832f, 62.479f),
			new Vector3(1938.961f, 1962.586f, 67.25843f),
			new Vector3(1981.335f, 1794.014f, 70.68142f),
			new Vector3(2025.448f, 1676.078f, 74.55499f),
			new Vector3(2076.301f, 1560.871f, 78.20548f),
			new Vector3(2129.393f, 1460.087f, 80.44589f),
			new Vector3(2184.985f, 1381.863f, 80.27392f),
			new Vector3(2450.337f, 1060.228f, 80.35757f),
			new Vector3(2535.084f, 951.4211f, 83.02557f),
			new Vector3(2586.891f, 856.8615f, 86.8588f),
			new Vector3(2637.094f, 728.5216f, 91.47308f),
			new Vector3(2666.999f, 600.1471f, 94.62489f),
			new Vector3(2672.687f, 492.453f, 95.29128f),
			new Vector3(2668.998f, 390.5248f, 95.37142f),
			new Vector3(2663.939f, 300.7089f, 95.29269f),
			new Vector3(2615.729f, 43.21269f, 94.43325f)
		};

		public static List<C4TrainNode> C4Placements_TrainNodes = new List<C4TrainNode>();

		public static C4TrainNode ClosestTrainNode;

		public static int CheckC4NodesTime;

		public static List<Blip> IntelMarkers = new List<Blip>();

		public static List<Blip> IntelMarkersRadiusArea = new List<Blip>();

		public static Blip TargetBlip;

		public static Blip TargetBlipArea;

		public static bool TheFederalReserveTrainHeistOn;

		public static Prop Grinder;

		public static Prop Keycard;

		public static KeypadClass MainVaultKeypad;

		public static bool HackedMainKeypad;

		public static KeypadClass SecondaryVaultKeypad;

		public static bool HackedSecondaryKeypad;

		public static bool CreatedOneilsAssets;

		public static bool CreatedLostMCAssets;

		public static bool InTrainOffice;

		public static HeistStage CurrentHeistStage;

		public Prop Lift1;

		public Prop Lift2;

		public bool LiftOn1;

		public bool LiftOn2;

		public int LiftDic1;

		public int LiftDic2;

		public static int TrainStage = 0;

		public List<SmartAI> MerryWeatherGuards = new List<SmartAI>();

		public SmartAI_Settings SmartAISettings = new SmartAI_Settings();

		public bool BrakeCyldr1Dead;

		public bool BrakeCyldr2Dead;

		public bool BrakeCyldr3Dead;

		public bool BrakeCyldr4Dead;

		public static List<string> C4TrackPositions = new List<string>();

		public static ArmouredTrain ArmouredTrainTest;

		public float GetDistance(Vector3 _1, Vector3 _2)
		{
			return _1.DistanceTo(_2);
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

		public Model RequestModel(string Name)
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

		public Model RequestModel(int Name)
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

		public Model RequestModel(PedHash Name)
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

		public Model RequestModel(VehicleHash Name)
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

		public static void ATTACH_ENTITY_TO_ENTITY(Entity entity1, Entity entity2, int boneIndex, float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, bool p9, bool useSoftPinning, bool collision, bool isPed, int vertexIndex, bool fixedRot)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, entity1, entity2, boneIndex, xPos, yPos, zPos, xRot, yRot, zRot, p9, useSoftPinning, collision, isPed, vertexIndex, fixedRot);
		}

		public void ConvertNavToSmartAIV2(List<MissionOjects> Navs, List<SmartGuards.SmartAiV2> AIGroup, SmartGuards.EnemyGroup Group)
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
						SmartGuards.SmartAiV2 smartAiV = new SmartGuards.SmartAiV2(0, objectPlacementMap.Inputtedhash, objectPlacementMap.Position, objectPlacementMap.Rotation.Z, true, 200, 100, 1, wL, 40f, 50f, WeaponHash.MilitaryRifle, Group);
						smartAiV.GroupActive = false;
						smartAiV.DistToSpawn = 200f;
						smartAiV.SpawnGroupArea = objectPlacementMap.Position;
						AIGroup.Add(smartAiV);
					}
				}
			}
		}

		public TheFederalReserveTrainHeist()
		{
			ConvertNavToSmartAIV2(LostMCNavs, LostMCGuards, SmartGuards.EnemyGroup.Lost_MC);
			ConvertNavToSmartAIV2(OneilsFarmNavs, OnielFarmGuards, SmartGuards.EnemyGroup.Rednecks);
			base.Tick += OnTick;
		}

		public static void CreateTrainAtCoords(Vector3 X)
		{
			TrainStage = 0;
			if (ArmouredTrainTest != null)
			{
				if (ArmouredTrainTest.TrainCars.Count > 0)
				{
					foreach (Vehicle trainCar in ArmouredTrainTest.TrainCars)
					{
						if (trainCar != null)
						{
							trainCar.Delete();
						}
					}
				}
				if (ArmouredTrainTest.AttackingPeds.Count > 0)
				{
					foreach (Ped attackingPed in ArmouredTrainTest.AttackingPeds)
					{
						if (attackingPed != null)
						{
							attackingPed.Delete();
						}
					}
				}
				if (ArmouredTrainTest.SecondaryProps.Count > 0)
				{
					foreach (Prop secondaryProp in ArmouredTrainTest.SecondaryProps)
					{
						if (secondaryProp != null)
						{
							secondaryProp.Delete();
						}
					}
				}
				if (ArmouredTrainTest.Turrets.Count > 0)
				{
					foreach (Vehicle turret in ArmouredTrainTest.Turrets)
					{
						if (turret != null)
						{
							turret.Delete();
						}
					}
				}
				if (ArmouredTrainTest.BrakeCylinders.Count > 0)
				{
					foreach (Prop brakeCylinder in ArmouredTrainTest.BrakeCylinders)
					{
						if (brakeCylinder != null)
						{
							brakeCylinder.Delete();
						}
					}
				}
				if (ArmouredTrainTest.Train != null)
				{
					ArmouredTrainTest.Train.Delete();
				}
				if (ArmouredTrainTest.TrainPilot != null)
				{
					ArmouredTrainTest.TrainPilot.Delete();
				}
				foreach (ArmouredTrainCarrage trainArmouredCarrage in ArmouredTrainTest.TrainArmouredCarrages)
				{
					if (trainArmouredCarrage.RobbableContainer1 != null)
					{
						foreach (ObjectPlacementMap interiorObject in trainArmouredCarrage.RobbableContainer1.InteriorObjects)
						{
							if (interiorObject.Entity != null)
							{
								interiorObject.Entity.Delete();
							}
						}
						if (trainArmouredCarrage.RobbableContainer1.FakeContainer != null)
						{
							trainArmouredCarrage.RobbableContainer1.FakeContainer.Delete();
						}
						if (trainArmouredCarrage.RobbableContainer1.TrainCar != null)
						{
							trainArmouredCarrage.RobbableContainer1.TrainCar.Delete();
						}
						if (trainArmouredCarrage.RobbableContainer1.Lock != null)
						{
							trainArmouredCarrage.RobbableContainer1.Lock.Delete();
						}
						if (trainArmouredCarrage.RobbableContainer1.Table != null)
						{
							trainArmouredCarrage.RobbableContainer1.Table.Delete();
						}
						if (trainArmouredCarrage.RobbableContainer1.Passenger != null)
						{
							trainArmouredCarrage.RobbableContainer1.Passenger.Delete();
						}
						if (trainArmouredCarrage.RobbableContainer1.Driver != null)
						{
							trainArmouredCarrage.RobbableContainer1.Driver.Delete();
						}
						if (trainArmouredCarrage.RobbableContainer1.Saw != null)
						{
							trainArmouredCarrage.RobbableContainer1.Saw.Delete();
						}
						if (trainArmouredCarrage.RobbableContainer1.Bag != null)
						{
							trainArmouredCarrage.RobbableContainer1.Bag.Delete();
						}
					}
					if (trainArmouredCarrage.RobbableContainer2 == null)
					{
						continue;
					}
					foreach (ObjectPlacementMap interiorObject2 in trainArmouredCarrage.RobbableContainer2.InteriorObjects)
					{
						if (interiorObject2.Entity != null)
						{
							interiorObject2.Entity.Delete();
						}
					}
					if (trainArmouredCarrage.RobbableContainer2.FakeContainer != null)
					{
						trainArmouredCarrage.RobbableContainer2.FakeContainer.Delete();
					}
					if (trainArmouredCarrage.RobbableContainer2.TrainCar != null)
					{
						trainArmouredCarrage.RobbableContainer2.TrainCar.Delete();
					}
					if (trainArmouredCarrage.RobbableContainer2.Lock != null)
					{
						trainArmouredCarrage.RobbableContainer2.Lock.Delete();
					}
					if (trainArmouredCarrage.RobbableContainer2.Table != null)
					{
						trainArmouredCarrage.RobbableContainer2.Table.Delete();
					}
					if (trainArmouredCarrage.RobbableContainer2.Passenger != null)
					{
						trainArmouredCarrage.RobbableContainer2.Passenger.Delete();
					}
					if (trainArmouredCarrage.RobbableContainer2.Driver != null)
					{
						trainArmouredCarrage.RobbableContainer2.Driver.Delete();
					}
					if (trainArmouredCarrage.RobbableContainer2.Saw != null)
					{
						trainArmouredCarrage.RobbableContainer2.Saw.Delete();
					}
					if (trainArmouredCarrage.RobbableContainer2.Bag != null)
					{
						trainArmouredCarrage.RobbableContainer2.Bag.Delete();
					}
				}
			}
			ArmouredTrainTest = new ArmouredTrain(X);
		}

		public static void EndHeist()
		{
			if (MainVaultKeypad != null && MainVaultKeypad.Keypad != null && MainVaultKeypad.Keypad.AttachedBlip != null)
			{
				MainVaultKeypad.Keypad.AttachedBlip.Delete();
			}
			if (SecondaryVaultKeypad != null && SecondaryVaultKeypad.Keypad != null && SecondaryVaultKeypad.Keypad.AttachedBlip != null)
			{
				SecondaryVaultKeypad.Keypad.AttachedBlip.Delete();
			}
			ManageRobberyAnims.DeleteLoot();
			Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
			TheFederalReserveTrainHeistOn = false;
			foreach (SmartGuards.SmartAiV2 onielFarmGuard in OnielFarmGuards)
			{
				if (onielFarmGuard != null && onielFarmGuard.AIPed != null)
				{
					onielFarmGuard.AIPed.Delete();
				}
			}
			foreach (SmartGuards.SmartAiV2 departmentofRailGuard in DepartmentofRailGuards)
			{
				if (departmentofRailGuard != null && departmentofRailGuard.AIPed != null)
				{
					departmentofRailGuard.AIPed.Delete();
				}
			}
			foreach (SmartGuards.SmartAiV2 lostMCGuard in LostMCGuards)
			{
				if (lostMCGuard != null && lostMCGuard.AIPed != null)
				{
					lostMCGuard.AIPed.Delete();
				}
			}
			Function.Call(Hash.SET_DISABLE_RANDOM_TRAINS_THIS_FRAME, false);
			CurrentHeistStage = HeistStage.Scout;
			foreach (Prop trainNodeBomb in TrainNodeBombs)
			{
				if (trainNodeBomb != null)
				{
					trainNodeBomb.Delete();
				}
			}
			TrainNodeBombs.Clear();
			foreach (C4TrainNode c4Placements_TrainNode in C4Placements_TrainNodes)
			{
				if (c4Placements_TrainNode.C4Blip != null)
				{
					c4Placements_TrainNode.C4Blip.Delete();
				}
			}
			C4Placements_TrainNodes.Clear();
			PlacedBombStage = 0;
			PlaceBombScene = -1;
			StoppedTrainUsingC4 = false;
			if (MainC4Prop != null)
			{
				MainC4Prop.Delete();
			}
			PlacedC4AtLocation1 = false;
			PlacedC4AtLocation2 = false;
			PlacedC4AtLocation3 = false;
			CreatedOneilsAssets = false;
			CreatedLostMCAssets = false;
			GotGrinder = false;
			GotC4 = false;
			C4ChargesCollected = 0;
			GotThermite = false;
			ThermiteChargesCollected = 0;
			GotKeycard = false;
			CurrentPay = 0;
			LocalTake = 0;
			HasSwappedTrainRouteToTrainYard = false;
			HasSwappedTrainRouteToEnterQuarry = false;
			TrainIsStoppingAtYard = false;
			SwappedTrainTrack = false;
			MainTrackSwitch = false;
			SecondaryTrackSwitch = false;
			if (USB != null)
			{
				USB.Delete();
			}
			if (Phone != null)
			{
				Phone.Delete();
			}
			if (LastKeypad != null)
			{
				LastKeypad.Delete();
			}
			HackedMainKeypad = false;
			HackedSecondaryKeypad = false;
			GrinderObjects.DeleteEntites();
			KeycardObjects.DeleteEntites();
			LockedDoorsObjects.DeleteEntites();
			LostMCObjects.DeleteEntites();
			LostMCC4Objects.DeleteEntites();
			OneilsFarmObjects.DeleteEntites();
			TrainCrashObjects.DeleteEntites();
			TrainOfficeObjects.DeleteEntites();
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
			foreach (Blip item in IntelMarkersRadiusArea)
			{
				if (item != null)
				{
					item.Delete();
				}
			}
			if (ArmouredTrainTest == null)
			{
				return;
			}
			if (ArmouredTrainTest.TrainCars.Count > 0)
			{
				foreach (Vehicle trainCar in ArmouredTrainTest.TrainCars)
				{
					if (trainCar != null)
					{
						trainCar.Delete();
					}
				}
			}
			if (ArmouredTrainTest.AttackingPeds.Count > 0)
			{
				foreach (Ped attackingPed in ArmouredTrainTest.AttackingPeds)
				{
					if (attackingPed != null)
					{
						attackingPed.Delete();
					}
				}
			}
			if (ArmouredTrainTest.SecondaryProps.Count > 0)
			{
				foreach (Prop secondaryProp in ArmouredTrainTest.SecondaryProps)
				{
					if (secondaryProp != null)
					{
						secondaryProp.Delete();
					}
				}
			}
			if (ArmouredTrainTest.Turrets.Count > 0)
			{
				foreach (Vehicle turret in ArmouredTrainTest.Turrets)
				{
					if (turret != null)
					{
						turret.Delete();
					}
				}
			}
			if (ArmouredTrainTest.BrakeCylinders.Count > 0)
			{
				foreach (Prop brakeCylinder in ArmouredTrainTest.BrakeCylinders)
				{
					if (brakeCylinder != null)
					{
						brakeCylinder.Delete();
					}
				}
			}
			if (ArmouredTrainTest.Train != null)
			{
				ArmouredTrainTest.Train.Delete();
			}
			if (ArmouredTrainTest.TrainPilot != null)
			{
				ArmouredTrainTest.TrainPilot.Delete();
			}
			foreach (ArmouredTrainCarrage trainArmouredCarrage in ArmouredTrainTest.TrainArmouredCarrages)
			{
				if (trainArmouredCarrage.RobbableContainer1 != null)
				{
					foreach (ObjectPlacementMap interiorObject in trainArmouredCarrage.RobbableContainer1.InteriorObjects)
					{
						if (interiorObject.Entity != null)
						{
							interiorObject.Entity.Delete();
						}
					}
					if (trainArmouredCarrage.RobbableContainer1.FakeContainer != null)
					{
						trainArmouredCarrage.RobbableContainer1.FakeContainer.Delete();
					}
					if (trainArmouredCarrage.RobbableContainer1.TrainCar != null)
					{
						trainArmouredCarrage.RobbableContainer1.TrainCar.Delete();
					}
					if (trainArmouredCarrage.RobbableContainer1.Lock != null)
					{
						trainArmouredCarrage.RobbableContainer1.Lock.Delete();
					}
					if (trainArmouredCarrage.RobbableContainer1.Table != null)
					{
						trainArmouredCarrage.RobbableContainer1.Table.Delete();
					}
					if (trainArmouredCarrage.RobbableContainer1.Passenger != null)
					{
						trainArmouredCarrage.RobbableContainer1.Passenger.Delete();
					}
					if (trainArmouredCarrage.RobbableContainer1.Driver != null)
					{
						trainArmouredCarrage.RobbableContainer1.Driver.Delete();
					}
					if (trainArmouredCarrage.RobbableContainer1.Saw != null)
					{
						trainArmouredCarrage.RobbableContainer1.Saw.Delete();
					}
					if (trainArmouredCarrage.RobbableContainer1.Bag != null)
					{
						trainArmouredCarrage.RobbableContainer1.Bag.Delete();
					}
				}
				if (trainArmouredCarrage.RobbableContainer2 == null)
				{
					continue;
				}
				foreach (ObjectPlacementMap interiorObject2 in trainArmouredCarrage.RobbableContainer2.InteriorObjects)
				{
					if (interiorObject2.Entity != null)
					{
						interiorObject2.Entity.Delete();
					}
				}
				if (trainArmouredCarrage.RobbableContainer2.FakeContainer != null)
				{
					trainArmouredCarrage.RobbableContainer2.FakeContainer.Delete();
				}
				if (trainArmouredCarrage.RobbableContainer2.TrainCar != null)
				{
					trainArmouredCarrage.RobbableContainer2.TrainCar.Delete();
				}
				if (trainArmouredCarrage.RobbableContainer2.Lock != null)
				{
					trainArmouredCarrage.RobbableContainer2.Lock.Delete();
				}
				if (trainArmouredCarrage.RobbableContainer2.Table != null)
				{
					trainArmouredCarrage.RobbableContainer2.Table.Delete();
				}
				if (trainArmouredCarrage.RobbableContainer2.Passenger != null)
				{
					trainArmouredCarrage.RobbableContainer2.Passenger.Delete();
				}
				if (trainArmouredCarrage.RobbableContainer2.Driver != null)
				{
					trainArmouredCarrage.RobbableContainer2.Driver.Delete();
				}
				if (trainArmouredCarrage.RobbableContainer2.Saw != null)
				{
					trainArmouredCarrage.RobbableContainer2.Saw.Delete();
				}
				if (trainArmouredCarrage.RobbableContainer2.Bag != null)
				{
					trainArmouredCarrage.RobbableContainer2.Bag.Delete();
				}
			}
		}

		public static void StartHeist()
		{
			ManageRobberyAnims.Heist_Average_Pay = (float)(HeistMinPay + HeistMaxPay) / 2f;
			Function.Call(Hash.BEGIN_TEXT_COMMAND_BUSYSPINNER_ON, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "Loading The Federal Reserve Train Heist, Please Wait");
			Function.Call(Hash.END_TEXT_COMMAND_BUSYSPINNER_ON, 5);
			EndHeist();
			LockedDoorsObjects.CreateEntites();
			foreach (Vector3 trainNode in TrainNodes)
			{
				C4Placements_TrainNodes.Add(new C4TrainNode(trainNode, false));
			}
			int index = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, GrinderObjects.Entities.Count);
			GrinderObjects.Entities[index].CreateObjatCoords();
			if (GrinderObjects.Entities[index].Entity != null)
			{
				Grinder = (Prop)GrinderObjects.Entities[index].Entity;
			}
			int index2 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, KeycardObjects.Entities.Count);
			KeycardObjects.Entities[index2].CreateObjatCoords();
			if (KeycardObjects.Entities[index2].Entity != null)
			{
				Keycard = (Prop)KeycardObjects.Entities[index2].Entity;
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
			Blip blip = GlobalHelper.CreateBlip(LostMC_CompountLoc);
			blip.Sprite = BlipSprite.HeistSetup;
			blip.Color = BlipColor.Green;
			blip.IsShortRange = true;
			blip.Scale = 0.5f;
			blip.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip);
			blip = GlobalHelper.CreateBlip(LostMC_CompountLoc, 100f);
			blip.Name = "Possible Intel/Asset";
			blip.Color = BlipColor.Green;
			blip.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip);
			blip = GlobalHelper.CreateBlip(OneilsFarmLoc);
			blip.Sprite = BlipSprite.HeistSetup;
			blip.Color = BlipColor.Green;
			blip.IsShortRange = true;
			blip.Scale = 0.5f;
			blip.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip);
			blip = GlobalHelper.CreateBlip(OneilsFarmLoc, 100f);
			blip.Name = "Possible Intel/Asset";
			blip.Color = BlipColor.Green;
			blip.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip);
			blip = GlobalHelper.CreateBlip(SwapTrainRouteQuarry);
			blip.Sprite = BlipSprite.HeistSetup;
			blip.Color = BlipColor.Green;
			blip.IsShortRange = true;
			blip.Scale = 0.5f;
			blip.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip);
			blip = GlobalHelper.CreateBlip(SwapTrainRouteQuarry, 100f);
			blip.Name = "Possible Intel/Asset";
			blip.Color = BlipColor.Green;
			blip.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip);
			blip = GlobalHelper.CreateBlip(SwapTrainRoutePowerPlant);
			blip.Sprite = BlipSprite.HeistSetup;
			blip.Color = BlipColor.Green;
			blip.IsShortRange = true;
			blip.Scale = 0.5f;
			blip.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip);
			blip = GlobalHelper.CreateBlip(SwapTrainRoutePowerPlant, 100f);
			blip.Name = "Possible Intel/Asset";
			blip.Color = BlipColor.Green;
			blip.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip);
			blip = GlobalHelper.CreateBlip(GrinderIntelLoc);
			blip.Sprite = BlipSprite.HeistSetup;
			blip.Color = BlipColor.Green;
			blip.IsShortRange = true;
			blip.Scale = 0.5f;
			blip.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip);
			blip = GlobalHelper.CreateBlip(GrinderIntelLoc, 100f);
			blip.Name = "Possible Intel/Asset";
			blip.Color = BlipColor.Green;
			blip.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip);
			blip = GlobalHelper.CreateBlip(TrainEngineerOfficeEnter);
			blip.Sprite = BlipSprite.HeistSetup;
			blip.Color = BlipColor.Green;
			blip.IsShortRange = true;
			blip.Scale = 0.5f;
			blip.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip);
			blip = GlobalHelper.CreateBlip(TrainEngineerOfficeEnter, 100f);
			blip.Name = "Possible Intel/Asset";
			blip.Color = BlipColor.Green;
			blip.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip);
			SecondaryVaultKeypad = new KeypadClass((Prop)LockedDoorsObjects.Entities[0].Entity, 2);
			MainVaultKeypad = new KeypadClass((Prop)LockedDoorsObjects.Entities[1].Entity, 2);
			Vector3 x = TrainSpawnCoords[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, TrainSpawnCoords.Count)];
			CreateTrainAtCoords(x);
			Function.Call(Hash.BUSYSPINNER_OFF);
			CurrentHeistStage = HeistStage.Scout;
			TheFederalReserveTrainHeistOn = true;
		}

		public void CheckKeypad(KeypadClass KeyPad)
		{
			if (!KeyPad.SpawnedKeypad && !KeyPad.SpawnedKeypad)
			{
				KeyPad.SpawnedKeypad = true;
				KeyPad.Keypad = GlobalHelper.CreateProp(RequestModel("ch_prop_fingerprint_scanner_01c"), KeyPad.KeypadPos, new Vector3(0f, 0f, KeyPad.KeypadRot), false, false);
				KeyPad.Keypad.IsPositionFrozen = true;
				KeyPad.Keypad.AddBlip();
				KeyPad.Keypad.AttachedBlip.Sprite = BlipSprite.Keypad2;
				KeyPad.Keypad.AttachedBlip.Scale = 0.75f;
				KeyPad.Keypad.AttachedBlip.IsShortRange = true;
				Script.Wait(1);
				if (GetDistance_VDIST2(KeyPad.Keypad.Position, new Vector3(2519.19f, -281.8895f, -64.6f), "<", 5f))
				{
					KeyPad.Keypad.Position = KeyPad.Keypad.GetOffsetPosition(new Vector3(0f, -0.007f, 0f));
					Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, KeyPad.Keypad, 277761, 554788825);
				}
				if (GetDistance_VDIST2(KeyPad.Keypad.Position, new Vector3(2516.78f, -259.7819f, -38.9f), "<", 5f))
				{
					KeyPad.Keypad.Position = KeyPad.Keypad.GetOffsetPosition(new Vector3(0f, 0.002f, 0f));
				}
			}
			if (!KeyPad.SpawnedKeypad)
			{
				return;
			}
			float num = KeyPad.KeypadPos.Z - 3f;
			float num2 = KeyPad.KeypadPos.Z + 3f;
			if (GetDistance_VDIST2(Game.Player.Character.Position, KeyPad.KeypadPos, ">", 30f) && KeyPad.Keypad.AttachedBlip != null)
			{
				KeyPad.Keypad.AttachedBlip.Alpha = 0;
			}
			if (GetDistance_VDIST2(Game.Player.Character.Position, KeyPad.KeypadPos, "<", 30f))
			{
				if (Game.Player.Character.Position.Z > num && Game.Player.Character.Position.Z < num2)
				{
					if (KeyPad.Keypad.AttachedBlip != null)
					{
						KeyPad.Keypad.AttachedBlip.Alpha = 255;
					}
				}
				else if (KeyPad.Keypad.AttachedBlip != null)
				{
					KeyPad.Keypad.AttachedBlip.Alpha = 0;
				}
			}
			if (!HackingKeypad)
			{
				if (!GetDistance_VDIST2(Game.Player.Character.Position, KeyPad.KeypadPos, "<", 10f))
				{
					return;
				}
				Vector3 offsetPosition = KeyPad.Keypad.GetOffsetPosition(new Vector3(0f, -1f, 0f));
				if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition, "<", 2f))
				{
					DisplayHelpTextThisFrame("~INPUT_CONTEXT~ Hack Keypad", 1);
					if (Game.IsControlJustPressed(Control.Context) && !HackingKeypad)
					{
						LastKeypad = KeyPad.Keypad;
						if (Phone != null)
						{
							Phone.Delete();
						}
						if (USB != null)
						{
							USB.Delete();
						}
						Phone = GlobalHelper.CreateProp(RequestModel("prop_phone_ing"), KeyPad.KeypadPos, false, false);
						USB = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_usb_drive01x"), KeyPad.KeypadPos, false, false);
						USB.IsVisible = false;
						Phone.IsVisible = false;
						USB.IsVisible = true;
						Phone.IsVisible = true;
						int index = Game.Player.Character.Bones[Bone.PHLeftHand].Index;
						LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
						Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, KeyPad.Keypad.Position.X, KeyPad.Keypad.Position.Y, KeyPad.Keypad.Position.Z, KeyPad.Keypad.Rotation.X, KeyPad.Keypad.Rotation.Y, KeyPad.Keypad.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "action_var_01", 0.0, 0.0, 0, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "action_var_01_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "action_var_01_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
						if (!Main.DisableHacks)
						{
							KeyPad.AwaitingHackGUI = true;
						}
						else
						{
							Script.Wait(8000);
							if (KeyPad == MainVaultKeypad)
							{
								HackedMainKeypad = true;
							}
							if (KeyPad == SecondaryVaultKeypad)
							{
								HackedSecondaryKeypad = true;
							}
							HackingKeypad = false;
							KeyPad.AwaitingHackGUI = false;
							LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
							Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, LastKeypad.Position.X, LastKeypad.Position.Y, LastKeypad.Position.Z, LastKeypad.Rotation.X, LastKeypad.Rotation.Y, LastKeypad.Rotation.Z, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit", 0.0, 0.0, 0, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "exit_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "exit_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
							Script.Wait(4000);
							Game.Player.Character.Task.ClearAllImmediately();
						}
					}
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353200 && KeyPad.AwaitingHackGUI)
				{
					Scene1 = -1;
					if (KeyPad.Type == 1)
					{
						FingerprintHack.RunKeypadHack();
					}
					if (KeyPad.Type == 2)
					{
						FingerprintHack.RunFingerPrintHack();
					}
					KeyPad.AwaitingHackGUI = false;
					HackingKeypad = true;
				}
			}
			else
			{
				if (!HackingKeypad)
				{
					return;
				}
				if (Game.IsControlJustPressed(Control.Context) && HackingKeypad)
				{
					HackingKeypad = false;
					KeyPad.AwaitingHackGUI = false;
					LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
					Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, LastKeypad.Position.X, LastKeypad.Position.Y, LastKeypad.Position.Z, LastKeypad.Rotation.X, LastKeypad.Rotation.Y, LastKeypad.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit", 0.0, 0.0, 0, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "exit_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "exit_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
				}
				if (!GetDistance_VDIST2(Game.Player.Character.Position, KeyPad.KeypadPos, "<", 5f))
				{
					return;
				}
				if (FingerprintHack.HackFinishedID == 2)
				{
					KeyPad.AwaitingHackGUI = false;
					FingerprintHack.HackFinishedID = 0;
					LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
					Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, LastKeypad.Position.X, LastKeypad.Position.Y, LastKeypad.Position.Z, LastKeypad.Rotation.X, LastKeypad.Rotation.Y, LastKeypad.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit", 0.0, 0.0, 0, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "success_exit", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "success_react_exit_var_01_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "success_react_exit_var_01_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
				}
				if (FingerprintHack.HackFinishedID != 1)
				{
					return;
				}
				KeyPad.Keypad.AttachedBlip.Alpha = 0;
				KeyPad.AwaitingHackGUI = false;
				FingerprintHack.HackFinishedID = 0;
				LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, LastKeypad.Position.X, LastKeypad.Position.Y, LastKeypad.Position.Z, LastKeypad.Rotation.X, LastKeypad.Rotation.Y, LastKeypad.Rotation.Z, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit", 0.0, 0.0, 0, 0, 1148846080, 0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "success_exit", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "fail_react_quick_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "fail_react_quick_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
				if (KeyPad == MainVaultKeypad)
				{
					HackedMainKeypad = true;
				}
				if (KeyPad == SecondaryVaultKeypad)
				{
					HackedSecondaryKeypad = true;
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353200)
				{
					HackingKeypad = false;
					KeyPad.AwaitingHackGUI = false;
					if (Phone != null)
					{
						Phone.Delete();
					}
					if (USB != null)
					{
						USB.Delete();
					}
					Scene1 = -1;
					HackingKeypad = false;
					Game.Player.Character.Task.ClearAll();
				}
			}
		}

		public static void CompleteHeist()
		{
			Main.TotalCrewEarned += 0f;
			Main.TotalPlayerEarned += CurrentPay;
			XP.CompleteHeist_FederalReserveTrain++;
			Main.SaveCuts();
			Class1.SetNoLongerInHeist();
			MissionScreen.SetPass("The Federal Reserve Train Heist", CurrentPay, 0, "The Player has died", 2, 0f, 5);
			int num = Game.GameTime + 30000;
			while (HKHModHelperNew.Class1.HeistPassedScreen != -1 && Game.GameTime <= num)
			{
				Script.Wait(0);
			}
			Script.Wait(2000);
			XP.AddXP_Percent(XP.CompleteHeist_XPPercent_FederalReserveTrain);
			Game.Player.Money += CurrentPay;
			EndHeist();
			CurrentPay = 0;
			LocalTake = 0;
			EndHeist();
			TheFederalReserveTrainHeistOn = false;
		}

		public static void FailHeist(string reason)
		{
			Class1.SetNoLongerInHeist();
			HKHModHelperNew.Class1.scaleformDescription = "The Federal Reserve Train Heist was failed " + reason;
			HKHModHelperNew.Class1.scaleformTitle = "The Federal Reserve Train Heist was Failed";
			HKHModHelperNew.Class1.ScaleformColor = HKHModHelperNew.Class1.ScaleformColour.Red;
			HKHModHelperNew.Class1.ShowScaleform = true;
			HKHModHelperNew.Class1.Passed_Index = 0;
			EndHeist();
			TheFederalReserveTrainHeistOn = false;
		}

		private void OnTick(object sender, EventArgs e)
		{
			if (!TheFederalReserveTrainHeistOn)
			{
				return;
			}
			if (TheFederalReserveTrainHeistOn)
			{
				switch (CurrentHeistStage)
				{
				}
				Function.Call(Hash.SET_RANDOM_TRAINS, false);
				Function.Call(Hash.SET_DISABLE_RANDOM_TRAINS_THIS_FRAME, true);
				if (CurrentHeistStage == HeistStage.Scout)
				{
					if (CurrentPay == 0)
					{
						Screen.ShowSubtitle("Approach the ~y~Federal Reserve Train~w~ or Scout for more ~g~Assets/Intel~w~");
					}
					if (CurrentPay != 0 && CurrentPay < 500000)
					{
						Screen.ShowSubtitle("Keep looting the ~y~Federal Reserve Train~w~ for more ~g~Valuables~w~");
					}
					if (CurrentPay >= 500000)
					{
						Screen.ShowSubtitle("Lose the ~r~Cops");
					}
				}
				if (CurrentHeistStage == HeistStage.C4)
				{
					Screen.ShowSubtitle("Find and steal ~g~C4~w~ to open the ~y~Federal Reserve Train's Containers~w~");
				}
				if (CurrentHeistStage == HeistStage.C4PlaceWaitforTrain)
				{
					Screen.ShowSubtitle("Wait for the ~y~Federal Reserve Train~w~ to pass over the placed ~r~C4");
				}
				if (CurrentHeistStage == HeistStage.CannotOpenContainers)
				{
					Screen.ShowSubtitle("Cannot Open the ~y~Federal Reserve Train's Containers~w~ find more ~g~Assets/Intel~w~ to figure out a way to open them");
				}
				if (CurrentHeistStage == HeistStage.HackKeypadTrainEngineersStation)
				{
					Screen.ShowSubtitle("Hack the ~y~Keypad~w~ to gain access to the ~y~Train Engineer's Station");
				}
				if (CurrentHeistStage == HeistStage.CannotOpenTrainEngineerStation)
				{
					Screen.ShowSubtitle("A ~g~Train Engineer's Keycard~w~ is required to access the ~y~Train Engineer's Station");
				}
				if (CurrentHeistStage == HeistStage.Grinder)
				{
					Screen.ShowSubtitle("Find and steal a ~g~Grinder~w~ to open the ~y~Federal Reserve Train's Containers~w~");
				}
				if (CurrentHeistStage == HeistStage.LootingTrain)
				{
					Screen.ShowSubtitle("Loot the ~y~Federal Reserve Train~w~ or escape the ~r~Cops");
				}
				if (CurrentHeistStage == HeistStage.PlaceC4onTrack)
				{
					Screen.ShowSubtitle("place a ~r~C4~w~ on the tracks to stop the  ~y~Federal Reserve Train~w~ at this designated spot");
				}
				if (CurrentHeistStage == HeistStage.Thermite)
				{
					Screen.ShowSubtitle("Find and steal ~g~Thermite~w~ to open the ~y~Federal Reserve Train's Containers~w~");
				}
				if (CurrentHeistStage == HeistStage.TrainEngineerStationKeycard)
				{
					Screen.ShowSubtitle("Find and steal a ~g~Train Engineer's Keycard~w~ to open the ~y~Train Engineer's Station~w~ at ~d~Power Plant & Davis Quarry");
				}
				if (CurrentHeistStage == HeistStage.TrainStoppingAtQuarry)
				{
					Screen.ShowSubtitle("Wait for the ~y~Federal Reserve Train~w~ to stop");
				}
				if (CurrentHeistStage == HeistStage.SwitchedTrack)
				{
					Screen.ShowSubtitle("Pull the ~y~Lever~w~ to Switch the tracks leading into ~d~Davis Quarry~w~");
				}
				if (CurrentHeistStage == HeistStage.SwitchedPower)
				{
					Screen.ShowSubtitle("Pull the ~y~Lever~w~ to Cut power to the main track leading away from ~d~Davis Quarry~w~ to force the train into ~d~Davis Quarry~w~");
				}
				if (ArmouredTrainTest != null && ArmouredTrainTest.Train != null)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, ArmouredTrainTest.Train.Position, "<", 200f))
					{
						if (ArmouredTrainTest.TrainStopped)
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
							CurrentHeistStage = HeistStage.TrainStoppingAtQuarry;
						}
						if (ArmouredTrainTest.TrainFullyStopped)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, ArmouredTrainTest.Train.Position, ">", 30f) && GetDistance_VDIST2(Game.Player.Character.Position, ArmouredTrainTest.Train.Position, "<", 200f))
							{
								Game.Player.WantedLevel = 3;
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
							}
							else
							{
								Game.Player.WantedLevel = 0;
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 3);
							}
							CurrentHeistStage = HeistStage.LootingTrain;
						}
					}
					else
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, ArmouredTrainTest.Train.Position, ">", 200f) && Game.Player.WantedLevel == 0 && CurrentPay >= 500000)
						{
							CompleteHeist();
						}
						if (CurrentHeistStage == HeistStage.LootingTrain || CurrentHeistStage == HeistStage.TrainStoppingAtQuarry)
						{
							CurrentHeistStage = HeistStage.Scout;
						}
					}
				}
				Function.Call(Hash.SWITCH_TRAIN_TRACK, 0, true);
				Function.Call(Hash.SWITCH_TRAIN_TRACK, 1, false);
				if (GotC4)
				{
					if (Game.GameTime >= CheckC4NodesTime)
					{
						CheckC4NodesTime = Game.GameTime + 2500;
						float radius = 500f;
						foreach (C4TrainNode c4Placements_TrainNode in C4Placements_TrainNodes)
						{
							if (c4Placements_TrainNode.C4Blip != null)
							{
								c4Placements_TrainNode.C4Blip.Delete();
								c4Placements_TrainNode.C4Blip = null;
							}
							if (GetDistance_VDIST2(c4Placements_TrainNode.Location, Game.Player.Character.Position, "<", radius) && !c4Placements_TrainNode.C4Placed)
							{
								radius = GetDistance(c4Placements_TrainNode.Location, Game.Player.Character.Position);
								ClosestTrainNode = c4Placements_TrainNode;
							}
						}
						if (ClosestTrainNode != null)
						{
							ClosestTrainNode.C4Blip = GlobalHelper.CreateBlip(ClosestTrainNode.Location);
							ClosestTrainNode.C4Blip.Sprite = BlipSprite.Bomb;
							ClosestTrainNode.C4Blip.Color = BlipColor.Yellow2;
							ClosestTrainNode.C4Blip.IsFlashing = true;
							ClosestTrainNode.C4Blip.Name = "Place Bomb to Stop train";
						}
					}
					if (ClosestTrainNode != null)
					{
						if (CurrentHeistStage == HeistStage.Scout && !StoppedTrainUsingC4 && !ArmouredTrainTest.TrainFullyStopped && !ArmouredTrainTest.TrainStopped)
						{
							CurrentHeistStage = HeistStage.PlaceC4onTrack;
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, ClosestTrainNode.Location, "<", 100f))
						{
							if (!ClosestTrainNode.C4Placed)
							{
								Vector3 vector = new Vector3(ClosestTrainNode.Location.X, ClosestTrainNode.Location.Y, ClosestTrainNode.Location.Z);
								vector = new Vector3(vector.X, vector.Y, World.GetGroundHeight(new Vector3(vector.X, vector.Y, vector.Z + 1f)));
								World.DrawMarker(MarkerType.Cylinder, vector, Vector3.Zero, Vector3.Zero, new Vector3(1f, 1f, 0.45f), Color.Yellow);
								if (GetDistance_VDIST2(Game.Player.Character.Position, ClosestTrainNode.Location, "<", 2f))
								{
									DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To place bomb on Train Track");
									if (Game.IsControlJustPressed(Control.Context))
									{
										PlacedBombStage = 0;
										if (MainC4Prop != null)
										{
											MainC4Prop.Delete();
										}
										ClosestTrainNode.C4Placed = true;
										LoadDict("weapons@projectile@sticky_bomb");
										LoadDict("weapons@projectile@sticky_bomb");
										PlaceBombScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z, 0, 0, Game.Player.Character.Rotation.Z, 2);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, PlaceBombScene, "weapons@projectile@sticky_bomb", "plant_floor", 0.0, 0.0, 0, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, PlaceBombScene, "plant_floor", LoadDict("weapons@projectile@sticky_bomb"), 1000f, 0f, 0, 1000f);
									}
								}
							}
						}
						else if (CurrentHeistStage == HeistStage.PlaceC4onTrack)
						{
							CurrentHeistStage = HeistStage.Scout;
						}
					}
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, PlaceBombScene) >= 1050737829 && PlacedBombStage != -1)
					{
						Vector3 position = Game.Player.Character.Bones[Bone.IKRightHand].Position;
						Prop prop = GlobalHelper.CreateProp(position: new Vector3(position.X, position.Y, World.GetGroundHeight(new Vector3(position.X, position.Y, position.Z)) + 0.005f), model: RequestModel("h4_prop_h4_ld_bomb_01a"), rotation: new Vector3(-90f, 0f, Game.Player.Character.Heading), dynamic: false, placeOnGround: true);
						prop.Position = prop.GetOffsetPosition(new Vector3(0f, 0f, 0.15f));
						TrainNodeBombs.Add(prop);
						PlacedBombStage = -1;
						prop.IsPositionFrozen = true;
					}
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, PlaceBombScene) >= 1065353200 && PlacedBombStage == -1)
					{
						Game.Player.Character.Task.ClearAllImmediately();
						PlacedBombStage = -2;
					}
					if (TrainNodeBombs.Count > 0)
					{
						if (ArmouredTrainTest.Train != null)
						{
							World.DrawMarker(MarkerType.Cylinder, ArmouredTrainTest.Train.Position, Vector3.Zero, Vector3.Zero, new Vector3(1f, 1f, 30f), Color.Yellow);
						}
						foreach (Prop trainNodeBomb in TrainNodeBombs)
						{
							if (ArmouredTrainTest == null || !(ArmouredTrainTest.Train != null) || !(trainNodeBomb != null))
							{
								continue;
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, trainNodeBomb.Position, "<", 30f))
							{
								if (CurrentHeistStage == HeistStage.Scout || CurrentHeistStage == HeistStage.PlaceC4onTrack)
								{
									CurrentHeistStage = HeistStage.C4PlaceWaitforTrain;
								}
							}
							else if (CurrentHeistStage == HeistStage.C4PlaceWaitforTrain)
							{
								CurrentHeistStage = HeistStage.Scout;
							}
							if (GetDistance_VDIST2(trainNodeBomb.Position, ArmouredTrainTest.Train.Position, "<", 100f))
							{
								Vector3 _ = new Vector3(trainNodeBomb.Position.X, trainNodeBomb.Position.Y, trainNodeBomb.Position.Z + 1f);
								drawText("Dist " + GetDistance(_, ArmouredTrainTest.Train.Position), 0.1f, 0.15f, 0.3f, 255, 255, 255);
								if (GetDistance_VDIST2(_, ArmouredTrainTest.Train.Position, "<", 5f) && !StoppedTrainUsingC4)
								{
									ArmouredTrainTest.BrakeCylinders[0].Health = -1;
									ArmouredTrainTest.BrakeCylinders[1].Health = -1;
									ArmouredTrainTest.BrakeCylinders[2].Health = -1;
									ArmouredTrainTest.BrakeCylinders[3].Health = -1;
									World.AddExplosion(trainNodeBomb.Position, ExplosionType.TankShell, 3f, 3f, Game.Player.Character);
									World.AddExplosion(trainNodeBomb.Position.Around(1f), ExplosionType.TankShell, 3f, 3f, Game.Player.Character);
									World.AddExplosion(trainNodeBomb.Position.Around(1f), ExplosionType.TankShell, 3f, 3f, Game.Player.Character);
									World.AddExplosion(trainNodeBomb.Position.Around(1f), ExplosionType.TankShell, 3f, 3f, Game.Player.Character);
									World.AddExplosion(trainNodeBomb.Position.Around(1f), ExplosionType.TankShell, 3f, 3f, Game.Player.Character);
									StoppedTrainUsingC4 = true;
								}
							}
						}
					}
				}
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
					ShowTextInfoBar("CurrentPay ", LocalTake, 1, new Vector2(-0.05f, 0f), new Vector2(-0.05f, 0f), true);
					ShowTextInfoBar("Thermal Charges ", ThermiteChargesCollected, 2, new Vector2(-0.05f, 0f), new Vector2(-0.05f, 0f), false);
					ShowTextInfoBar("C4 Charges ", C4ChargesCollected, 3, new Vector2(-0.05f, 0f), new Vector2(-0.05f, 0f), false);
				}
				if (DepartmentofRailGuards.Count > 1 && CurrentActiveGroup == ActiveGroup.TheFederalReserveTrainHeist_DepartmentOfRail)
				{
					CurrentHeistStage = HeistStage.TrainEngineerStationKeycard;
					if (Keycard != null && !GotKeycard)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, Keycard.Position, "<", 30f))
						{
							World.DrawMarker(MarkerType.Chevron1, Keycard.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), Vector3.Zero, new Vector3(0f, 180f, 0f), new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 255, 0), true, true);
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
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Pickup Train Engineers Keycard");
							if (Game.IsControlJustPressed(Control.Context))
							{
								IntelMarkers[5].Alpha = 0;
								IntelMarkersRadiusArea[5].Alpha = 0;
								if (Keycard.AttachedBlip != null)
								{
									Keycard.AttachedBlip.Alpha = 0;
								}
								EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(Keycard, 0));
								TextNotification("CHAR_HUMANDEFAULT", "~y~The Billionaires Party~w~", "Intel Found", "You have picked up a Keycard to get access to Engineers controls at Davis Quarry and Power Plant");
								GotKeycard = true;
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
				}
				if (!InTrainOffice)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, TrainEngineerOfficeEnter, "<", 100f))
					{
						World.DrawMarker(MarkerType.Cylinder, TrainEngineerOfficeEnter, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
						if (GetDistance_VDIST2(Game.Player.Character.Position, TrainEngineerOfficeEnter, "<", 2f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Los Santos Department of Rail");
							if (Game.IsControlJustPressed(Control.Context))
							{
								Function.Call(Hash.BEGIN_TEXT_COMMAND_BUSYSPINNER_ON, "STRING");
								Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "Loading The Federal Reserve Assets");
								Function.Call(Hash.END_TEXT_COMMAND_BUSYSPINNER_ON, 5);
								TrainOfficeObjects.CreateEntites();
								DepartmentofRailGuards.Clear();
								foreach (ObjectPlacementMap entity in TrainOfficeObjects.Entities)
								{
									if (entity.Entity != null && entity.Entity.Model.IsPed)
									{
										DepartmentofRailGuards.Add(new SmartGuards.SmartAiV2((Ped)entity.Entity, SmartGuards.EnemyGroup.Enemy));
									}
								}
								Function.Call(Hash.BUSYSPINNER_OFF);
								Screen.FadeOut(1500);
								Script.Wait(1500);
								InTrainOffice = true;
								Game.Player.Character.Position = TrainEngineerOfficeExit;
								CurrentHeistStage = HeistStage.TrainEngineerStationKeycard;
								Script.Wait(1500);
								Screen.FadeIn(1500);
							}
						}
					}
				}
				else if (InTrainOffice)
				{
					if (CurrentActiveGroup != ActiveGroup.TheFederalReserveTrainHeist_DepartmentOfRail)
					{
						SmartGuards.GuardGroup[0].GroupList.Clear();
						SmartGuards.GuardGroup[0].GroupList.AddRange(DepartmentofRailGuards);
						SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Enemy;
						CurrentActiveGroup = ActiveGroup.TheFederalReserveTrainHeist_DepartmentOfRail;
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, TrainEngineerOfficeExit, "<", 100f))
					{
						World.DrawMarker(MarkerType.Cylinder, TrainEngineerOfficeExit, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
						if (GetDistance_VDIST2(Game.Player.Character.Position, TrainEngineerOfficeExit, "<", 2f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit Los Santos Department of Rail");
							if (Game.IsControlJustPressed(Control.Context))
							{
								Screen.FadeOut(1500);
								Script.Wait(1500);
								InTrainOffice = false;
								Game.Player.Character.Position = TrainEngineerOfficeEnter;
								TrainOfficeObjects.DeleteEntites();
								CurrentActiveGroup = ActiveGroup.None;
								SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.None;
								CurrentHeistStage = HeistStage.Scout;
								Script.Wait(1500);
								Screen.FadeIn(1500);
							}
						}
					}
				}
				if (!HackedMainKeypad && GotKeycard)
				{
					if (MainVaultKeypad != null && MainVaultKeypad.Keypad != null)
					{
						CheckKeypad(MainVaultKeypad);
						if (GetDistance_VDIST2(Game.Player.Character.Position, MainVaultKeypad.Keypad.Position, "<", 10f))
						{
							CurrentHeistStage = HeistStage.HackKeypadTrainEngineersStation;
						}
					}
				}
				else if (!GotKeycard)
				{
					if (MainVaultKeypad != null && MainVaultKeypad.Keypad != null)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, MainVaultKeypad.Keypad.Position, "<", 10f))
						{
							CurrentHeistStage = HeistStage.CannotOpenTrainEngineerStation;
						}
						else if (CurrentHeistStage == HeistStage.CannotOpenTrainEngineerStation)
						{
							CurrentHeistStage = HeistStage.Scout;
						}
					}
				}
				else if (!MainTrackSwitch)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, MainVaultKeypad.Keypad.Position, "<", 30f))
					{
						CurrentHeistStage = HeistStage.SwitchedTrack;
					}
					else if (CurrentHeistStage == HeistStage.SwitchedTrack)
					{
						CurrentHeistStage = HeistStage.Scout;
					}
				}
				else if (CurrentHeistStage == HeistStage.SwitchedTrack)
				{
					CurrentHeistStage = HeistStage.Scout;
				}
				if (!HackedSecondaryKeypad && GotKeycard)
				{
					if (SecondaryVaultKeypad != null && SecondaryVaultKeypad.Keypad != null)
					{
						CheckKeypad(SecondaryVaultKeypad);
						if (GetDistance_VDIST2(Game.Player.Character.Position, SecondaryVaultKeypad.Keypad.Position, "<", 10f))
						{
							CurrentHeistStage = HeistStage.HackKeypadTrainEngineersStation;
						}
					}
				}
				else if (!GotKeycard)
				{
					if (SecondaryVaultKeypad != null && SecondaryVaultKeypad.Keypad != null)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, SecondaryVaultKeypad.Keypad.Position, "<", 10f))
						{
							CurrentHeistStage = HeistStage.CannotOpenTrainEngineerStation;
						}
						else if (CurrentHeistStage == HeistStage.CannotOpenTrainEngineerStation)
						{
							CurrentHeistStage = HeistStage.Scout;
						}
					}
				}
				else if (!SecondaryTrackSwitch)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, SecondaryVaultKeypad.Keypad.Position, "<", 30f))
					{
						CurrentHeistStage = HeistStage.SwitchedPower;
					}
					else if (CurrentHeistStage == HeistStage.SwitchedPower)
					{
						CurrentHeistStage = HeistStage.Scout;
					}
				}
				else if (CurrentHeistStage == HeistStage.SwitchedPower)
				{
					CurrentHeistStage = HeistStage.Scout;
				}
				if (HackedSecondaryKeypad)
				{
					Prop prop2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2635.85f, 2934.55f, 44.91f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "xm_Prop_iaa_base_door_01"), 0, 0, 0);
					if (prop2 != null)
					{
						if (prop2.IsAttached())
						{
							prop2.Detach();
						}
						if (prop2.Rotation.Z >= -126f)
						{
							Vector3 vector3 = (prop2.Rotation = new Vector3(0f, 0f, prop2.Rotation.Z - 0.25f));
						}
					}
				}
				if (HackedMainKeypad)
				{
					Prop prop3 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2622.59f, 1694.011f, 32f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "xm_Prop_iaa_base_door_01"), 0, 0, 0);
					if (prop3 != null)
					{
						if (prop3.IsAttached())
						{
							prop3.Detach();
						}
						if (prop3.Rotation.Z >= -90f)
						{
							Vector3 vector5 = (prop3.Rotation = new Vector3(0f, 0f, prop3.Rotation.Z - 0.25f));
						}
					}
				}
				if (ArmouredTrainTest != null && ArmouredTrainTest.Train != null)
				{
					if (TargetBlipArea == null)
					{
						TargetBlipArea = GlobalHelper.CreateBlip(ArmouredTrainTest.Train.Position, 100f);
						TargetBlipArea.Color = BlipColor.Yellow;
						TargetBlipArea.Name = "Federal Reserve Train";
						TargetBlipArea.IsShortRange = true;
						TargetBlipArea.Alpha = 144;
					}
					if (TargetBlipArea != null)
					{
						TargetBlipArea.Position = ArmouredTrainTest.Train.Position;
					}
				}
				if (SecondaryTrackSwitch && MainTrackSwitch)
				{
					SwappedTrainTrack = true;
				}
				else
				{
					SwappedTrainTrack = false;
				}
				if (Grinder != null && !GotGrinder)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, Grinder.Position, "<", 10f))
					{
						CurrentHeistStage = HeistStage.Grinder;
					}
					else if (CurrentHeistStage == HeistStage.Grinder)
					{
						CurrentHeistStage = HeistStage.Scout;
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, Grinder.Position, "<", 30f))
					{
						World.DrawMarker(MarkerType.Chevron1, Grinder.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), Vector3.Zero, new Vector3(0f, 180f, 0f), new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 255, 0), true, true);
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, Grinder.Position, "<", 10f) && Grinder.AttachedBlip == null)
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
							IntelMarkers[4].Alpha = 0;
							IntelMarkersRadiusArea[4].Alpha = 0;
							if (Grinder.AttachedBlip != null)
							{
								Grinder.AttachedBlip.Alpha = 0;
							}
							EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(Grinder, 0));
							TextNotification("CHAR_HUMANDEFAULT", "~y~Federal Reserve Train Heist~w~", "Intel Found", "You have found the Grinder to saw open Federal Reserve Train's Containers");
							GotGrinder = true;
						}
					}
				}
				else if (CurrentHeistStage == HeistStage.Grinder)
				{
					CurrentHeistStage = HeistStage.Scout;
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, LostMC_CompountLoc, "<", 300f))
				{
					if (CurrentActiveGroup != ActiveGroup.TheFederalReserveTrainHeist_LostMCGuards)
					{
						SmartGuards.GuardGroup[14].GroupList.Clear();
						SmartGuards.GuardGroup[14].GroupList.AddRange(LostMCGuards);
						SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Lost_MC;
						CurrentActiveGroup = ActiveGroup.TheFederalReserveTrainHeist_LostMCGuards;
					}
					if (!CreatedLostMCAssets)
					{
						CreatedLostMCAssets = true;
						LostMCC4Objects.CreateEntites();
						LostMCObjects.CreateEntites();
					}
					else
					{
						Vector3 position3 = Game.Player.Character.Position;
						Prop prop4 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position3.X, position3.Y, position3.Z, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 0, 0, 0);
						if (prop4.Exists() && prop4 != null)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, prop4.Position, "<", 30f))
							{
								World.DrawMarker(MarkerType.Chevron1, prop4.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), Vector3.Zero, new Vector3(0f, 180f, 0f), new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 255, 0), true, true);
							}
							if (prop4.AttachedBlip == null)
							{
								prop4.AddBlip();
								prop4.AttachedBlip.Sprite = BlipSprite.Bomb;
								prop4.AttachedBlip.Name = "C4 Crate";
								prop4.AttachedBlip.Color = BlipColor.Green;
								prop4.AttachedBlip.IsShortRange = true;
								prop4.AttachedBlip.IsFlashing = true;
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, prop4.Position, "<", 2.5f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Pickup C4");
								if (Game.IsControlJustPressed(Control.Context))
								{
									IntelMarkers[0].Alpha = 0;
									IntelMarkersRadiusArea[0].Alpha = 0;
									C4ChargesCollected += 3;
									if (prop4.AttachedBlip != null)
									{
										prop4.AttachedBlip.Alpha = 0;
									}
									EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(prop4, 0));
									TextNotification("CHAR_HUMANDEFAULT", "~y~Federal Reserve Train Heist~w~", "Intel Found", "You have found the C4 to blow open Federal Reserve Train's Containers");
									GotC4 = true;
								}
							}
						}
					}
				}
				else
				{
					if (CurrentActiveGroup == ActiveGroup.TheFederalReserveTrainHeist_LostMCGuards)
					{
						SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.None;
						CurrentActiveGroup = ActiveGroup.None;
					}
					if (CurrentHeistStage == HeistStage.C4)
					{
						CurrentHeistStage = HeistStage.Scout;
					}
				}
				if (LostMCGuards.Count > 1 && CurrentActiveGroup == ActiveGroup.TheFederalReserveTrainHeist_LostMCGuards)
				{
					CurrentHeistStage = HeistStage.C4;
					SmartGuards.GetCurrentGroupSettings().ShowCone = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipRadius = 100f;
					SmartGuards.GetCurrentGroupSettings().Radius = 80f;
					SmartGuards.GetCurrentGroupSettings().SeeingAngle = 35f;
					SmartGuards.GetCurrentGroupSettings().AllowInvestigate = true;
					SmartGuards.GetCurrentGroupSettings().InvestigateDist = 40f;
					SmartGuards.GetCurrentGroupSettings().HitRadius = 30f;
					SmartGuards.GetCurrentGroupSettings().ArrestInsteadOfShoot = false;
					SmartGuards.GetCurrentGroupSettings().UseShowBlipWhenSameHeightWithPlayer = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightAdd = 30f;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightMinus = 30f;
					SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, OneilsFarmLoc, "<", 200f))
				{
					if (CurrentActiveGroup != ActiveGroup.TheFederalReserveTrainHeist_OnielFarmGuards)
					{
						SmartGuards.GuardGroup[20].GroupList.Clear();
						SmartGuards.GuardGroup[20].GroupList.AddRange(OnielFarmGuards);
						SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Rednecks;
						CurrentActiveGroup = ActiveGroup.TheFederalReserveTrainHeist_OnielFarmGuards;
					}
					if (!CreatedOneilsAssets)
					{
						CreatedOneilsAssets = true;
						OneilsFarmObjects.CreateEntites();
					}
					else
					{
						Vector3 position4 = Game.Player.Character.Position;
						Prop prop5 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position4.X, position4.Y, position4.Z, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 0, 0, 0);
						if (prop5 != null)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, prop5.Position, "<", 30f))
							{
								World.DrawMarker(MarkerType.Chevron1, prop5.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), Vector3.Zero, new Vector3(0f, 180f, 0f), new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 255, 0), true, true);
							}
							if (prop5.Exists())
							{
								if (prop5.AttachedBlip == null)
								{
									prop5.AddBlip();
									prop5.AttachedBlip.Sprite = BlipSprite.Bomb;
									prop5.AttachedBlip.Name = "Thermal Charges";
									prop5.AttachedBlip.Color = BlipColor.Green;
									prop5.AttachedBlip.IsShortRange = true;
									prop5.AttachedBlip.IsFlashing = true;
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, prop5.Position, "<", 2.5f))
								{
									DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Pickup Thermal Charges");
									if (Game.IsControlJustPressed(Control.Context))
									{
										IntelMarkers[1].Alpha = 0;
										IntelMarkersRadiusArea[1].Alpha = 0;
										ThermiteChargesCollected++;
										if (prop5.AttachedBlip != null)
										{
											prop5.AttachedBlip.Alpha = 0;
										}
										EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(prop5, 0));
										TextNotification("CHAR_HUMANDEFAULT", "~y~Federal Reserve Train Heist~w~", "Intel Found", "You have found the Themite to open The Federal Reserve Train's Containers");
										GotThermite = true;
									}
								}
							}
						}
					}
				}
				else
				{
					if (CurrentActiveGroup == ActiveGroup.TheFederalReserveTrainHeist_OnielFarmGuards)
					{
						SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.None;
						CurrentActiveGroup = ActiveGroup.None;
					}
					if (CurrentHeistStage == HeistStage.Thermite)
					{
						CurrentHeistStage = HeistStage.Scout;
					}
				}
				if (OnielFarmGuards.Count > 1 && CurrentActiveGroup == ActiveGroup.TheFederalReserveTrainHeist_OnielFarmGuards)
				{
					CurrentHeistStage = HeistStage.Thermite;
					SmartGuards.GetCurrentGroupSettings().ShowCone = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipRadius = 100f;
					SmartGuards.GetCurrentGroupSettings().Radius = 80f;
					SmartGuards.GetCurrentGroupSettings().SeeingAngle = 35f;
					SmartGuards.GetCurrentGroupSettings().AllowInvestigate = true;
					SmartGuards.GetCurrentGroupSettings().InvestigateDist = 40f;
					SmartGuards.GetCurrentGroupSettings().HitRadius = 30f;
					SmartGuards.GetCurrentGroupSettings().ArrestInsteadOfShoot = false;
					SmartGuards.GetCurrentGroupSettings().UseShowBlipWhenSameHeightWithPlayer = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightAdd = 30f;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightMinus = 30f;
					SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
				}
				Vector3 position5 = Game.Player.Character.Position;
				if (!MainTrackSwitch)
				{
					Prop prop6 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2632.059f, 2934.051f, 43.70516f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "p_rail_controller_s"), 0, 0, 0);
					if (prop6 != null && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2632.059f, 2934.051f, 43.70516f), "<", 2.5f))
					{
						prop6.Position = new Vector3(2632.059f, 2934.051f, 43.70516f);
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Switch Primary Track", 1);
						if (Game.IsControlJustPressed(Control.Context))
						{
							IntelMarkers[2].Alpha = 0;
							IntelMarkersRadiusArea[2].Alpha = 0;
							prop6.Position = new Vector3(2632.059f, 2934.051f, 43.70516f);
							LoadDict("missbigscoreprepd");
							Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, prop6.Position.X, prop6.Position.Y, prop6.Position.Z, 0.0, 0.0, prop6.Rotation.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict("missbigscoreprepd"), "pull_level_michael", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "pull_level_michael", LoadDict("missbigscoreprepd"), 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop6, Scene1, "pull_level_panel", LoadDict("missbigscoreprepd"), 16f, -16f, 0, 1148846080);
							MainTrackSwitch = true;
						}
					}
				}
				else if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2632.059f, 2934.051f, 43.70516f), "<", 2.5f) && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
				{
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
					Game.Player.Character.Task.ClearAll();
				}
				if (!SecondaryTrackSwitch)
				{
					Prop prop7 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2618.774f, 1691.839f, 30.86389f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "prop_rail_controller"), 0, 0, 0);
					if (prop7 != null && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2618.774f, 1691.839f, 30.86389f), "<", 2.5f))
					{
						prop7.Position = new Vector3(2618.774f, 1691.839f, 30.86389f);
						World.DrawMarker(MarkerType.Cylinder, prop7.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.05f, 0.05f, 3f), Color.Goldenrod);
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Cut Power to Primary Track at Davis Quarry", 1);
						if (Game.IsControlJustPressed(Control.Context))
						{
							IntelMarkers[3].Alpha = 0;
							IntelMarkersRadiusArea[3].Alpha = 0;
							prop7.Position = new Vector3(2618.774f, 1691.839f, 30.86389f);
							LoadDict("missbigscoreprepd");
							Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, prop7.Position.X, prop7.Position.Y, prop7.Position.Z, 0.0, 0.0, prop7.Rotation.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict("missbigscoreprepd"), "pull_level_michael", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "pull_level_michael", LoadDict("missbigscoreprepd"), 16f, -16f, 0, 1148846080);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, prop7, Scene1, "pull_level_panel", LoadDict("missbigscoreprepd"), 16f, -16f, 0, 1148846080);
							SecondaryTrackSwitch = true;
						}
					}
				}
				else if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2618.774f, 1691.839f, 30.86389f), "<", 2.5f) && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
				{
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
					Game.Player.Character.Task.ClearAll();
				}
				if (ArmouredTrainTest != null)
				{
					if (!ArmouredTrainTest.TrainActive)
					{
						ArmouredTrainTest.TrainActive = true;
						ArmouredTrainTest.CreateTrain();
					}
					bool flag = false;
					if (ArmouredTrainTest.TrainActive)
					{
						if (!ArmouredTrainTest.CreatedBlip)
						{
							ArmouredTrainTest.CreateBlip();
							ArmouredTrainTest.CreatedBlip = true;
						}
						if (!ArmouredTrainTest.TrainActive)
						{
							ArmouredTrainTest.DeliveryPoint.Alpha = 255;
							ArmouredTrainTest.Radius.Alpha = 144;
						}
						if (!SwappedTrainTrack && !ResetTrainAfterEnteringQuarry && GetDistance_VDIST2(ArmouredTrainTest.Train.Position, new Vector3(2655.13f, 3000.66f, 39.783f), "<", 2.5f))
						{
							CreateTrainAtCoords(new Vector3(2659.64f, 3008.6f, 39.9753f));
						}
						if (ResetTrainAfterEnteringQuarry && GetDistance_VDIST2(ArmouredTrainTest.Train.Position, new Vector3(2655.13f, 3000.66f, 39.783f), ">", 5f))
						{
							ResetTrainAfterEnteringQuarry = false;
						}
						if (!ArmouredTrainTest.TrainStopped)
						{
							if (ArmouredTrainTest.Train != null)
							{
								if (!HasSwappedTrainRouteToEnterQuarry && GetDistance_VDIST2(ArmouredTrainTest.Train.Position, new Vector3(2659.64f, 3008.6f, 39.9753f), "<", 5f) && SwappedTrainTrack)
								{
									HasSwappedTrainRouteToEnterQuarry = true;
									CreateTrainAtCoords(new Vector3(2655.13f, 3000.66f, 39.783f));
									Notification.Show("Train Entering Quarry");
								}
								if (ArmouredTrainTest.Train != null)
								{
									if (!HasSwappedTrainRouteToTrainYard)
									{
										if (GetDistance_VDIST2(ArmouredTrainTest.Train.Position, new Vector3(499.604f, -1571.5f, 28.3367f), "<", 5f))
										{
											HasSwappedTrainRouteToTrainYard = true;
											CreateTrainAtCoords(new Vector3(499.604f, -1571.59f, -28.3367f));
										}
									}
									else if (HasSwappedTrainRouteToTrainYard && GetDistance_VDIST2(ArmouredTrainTest.Train.Position, new Vector3(532.343f, -965.388f, 26.3755f), "<", 5f))
									{
										TrainIsStoppingAtYard = true;
									}
								}
								if (ArmouredTrainTest.Train != null)
								{
									ArmouredTrainTest.TrainStopped = true;
									ArmouredTrainTest.Train.ForwardSpeed = 15f;
									Function.Call(Hash.SET_TRAIN_SPEED, ArmouredTrainTest.Train, 15f);
									Function.Call(Hash.SET_TRAIN_CRUISE_SPEED, ArmouredTrainTest.Train, 15f);
								}
							}
						}
						else if (ArmouredTrainTest.Train != null)
						{
							if (ArmouredTrainTest.TrainSpeed > 0f)
							{
								ArmouredTrainTest.TrainSpeed -= 0.025f;
							}
							if (ArmouredTrainTest.TrainSpeed <= 0f)
							{
								ArmouredTrainTest.TrainSpeed = 0f;
								if (!ArmouredTrainTest.TrainFullyStopped)
								{
									TrainHandler.Trains.Add(ArmouredTrainTest.Train);
									ArmouredTrainTest.TrainFullyStopped = true;
								}
							}
							ArmouredTrainTest.Train.ForwardSpeed = ArmouredTrainTest.TrainSpeed;
							Function.Call(Hash.SET_TRAIN_SPEED, ArmouredTrainTest.Train, ArmouredTrainTest.TrainSpeed);
							Function.Call(Hash.SET_TRAIN_CRUISE_SPEED, ArmouredTrainTest.Train, ArmouredTrainTest.TrainSpeed);
						}
						bool flag2 = true;
						foreach (Prop brakeCylinder in ArmouredTrainTest.BrakeCylinders)
						{
							if (brakeCylinder.Health < 100)
							{
								brakeCylinder.Opacity = 0;
								if (brakeCylinder.AttachedBlip != null)
								{
									brakeCylinder.AttachedBlip.Alpha = 0;
								}
							}
							if (GetDistance_VDIST2(brakeCylinder.Position, Game.Player.Character.Position, "<", 15f))
							{
								Vector2 vector6 = World3DToScreen2d(brakeCylinder.Position);
								if (brakeCylinder.AttachedBlip != null)
								{
									if (brakeCylinder.Health > 100)
									{
										brakeCylinder.AttachedBlip.Alpha = 255;
									}
									else
									{
										brakeCylinder.AttachedBlip.Alpha = 0;
									}
								}
							}
							else if (brakeCylinder.AttachedBlip != null)
							{
								brakeCylinder.AttachedBlip.Alpha = 0;
							}
							if (brakeCylinder.Health >= 100)
							{
								flag2 = false;
							}
						}
						if (flag2 || HasSwappedTrainRouteToEnterQuarry)
						{
							ArmouredTrainTest.TrainStopped = true;
						}
						else
						{
							ArmouredTrainTest.TrainStopped = false;
						}
						if (ArmouredTrainTest.Train != null)
						{
							if (GetDistance_VDIST2(ArmouredTrainTest.Spawn, Game.Player.Character.Position, "<", 800f))
							{
								flag = true;
							}
							if (GetDistance_VDIST2(ArmouredTrainTest.Train.Position, Game.Player.Character.Position, "<", 800f))
							{
								flag = true;
							}
						}
						if ((GetDistance_VDIST2(ArmouredTrainTest.Spawn, Game.Player.Character.Position, "<", 800f) || flag) && ArmouredTrainTest != null && ArmouredTrainTest != null)
						{
							if (ArmouredTrainTest.Radius != null && ArmouredTrainTest.DeliveryPoint != null)
							{
								if (ArmouredTrainTest.TrainActive)
								{
									bool flag3 = ArmouredTrainTest.Train != null;
								}
								if (!ArmouredTrainTest.TrainActive && !ArmouredTrainTest.TrainActive)
								{
									ArmouredTrainTest.TrainActive = true;
									ArmouredTrainTest.CreateTrain();
								}
							}
							if (ArmouredTrainTest.TrainArmouredCarrages.Count > 0)
							{
								for (int i = 0; i < ArmouredTrainTest.TrainArmouredCarrages.Count; i++)
								{
									ArmouredTrainCarrage armouredTrainCarrage = ArmouredTrainTest.TrainArmouredCarrages[i];
									if (armouredTrainCarrage == null || !(armouredTrainCarrage.Carrage != null) || !(armouredTrainCarrage.Container1 != null) || !(armouredTrainCarrage.Container2 != null) || !ArmouredTrainTest.TrainFullyStopped)
									{
										continue;
									}
									if (ArmouredTrainTest.Train != null)
									{
										ArmouredTrainTest.Train.IsPositionFrozen = true;
									}
									if (armouredTrainCarrage.RobbableContainer1 != null && armouredTrainCarrage.RobbableContainer1.Lock != null)
									{
										armouredTrainCarrage.RobbableContainer1.Lock.Position = armouredTrainCarrage.Container1.GetOffsetPosition(new Vector3(0f, -1.8f, 1.2f));
									}
									if (armouredTrainCarrage.RobbableContainer2 != null && armouredTrainCarrage.RobbableContainer2.Lock != null)
									{
										armouredTrainCarrage.RobbableContainer2.Lock.Position = armouredTrainCarrage.Container2.GetOffsetPosition(new Vector3(0f, -1.8f, 1.2f));
									}
									if (!armouredTrainCarrage.CreatedLoot)
									{
										if (ArmouredTrainTest.Train != null)
										{
											ArmouredTrainTest.Train.IsPositionFrozen = true;
										}
										armouredTrainCarrage.CreatedLoot = true;
										Prop @lock = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_lock_01a"), armouredTrainCarrage.Container1.GetOffsetPosition(new Vector3(0f, -1.8f, 1.2f)), new Vector3(0f, 0f, armouredTrainCarrage.Container1.Rotation.Z), false, false);
										armouredTrainCarrage.RobbableContainer1 = new RobbableContainer();
										armouredTrainCarrage.RobbableContainer1.Lock = @lock;
										armouredTrainCarrage.Container1.IsPositionFrozen = true;
										armouredTrainCarrage.Container1.Detach();
										armouredTrainCarrage.Container1.AddBlip();
										armouredTrainCarrage.Container1.AttachedBlip.Sprite = BlipSprite.ShippingContainer;
										armouredTrainCarrage.Container1.AttachedBlip.Color = BlipColor.White;
										armouredTrainCarrage.Container1.AttachedBlip.Alpha = 255;
										armouredTrainCarrage.Container1.AttachedBlip.Name = "Robbable Container";
										@lock = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_lock_01a"), armouredTrainCarrage.Container2.GetOffsetPosition(new Vector3(0f, -1.8f, 1.2f)), new Vector3(0f, 0f, armouredTrainCarrage.Container2.Rotation.Z), false, false);
										armouredTrainCarrage.RobbableContainer2 = new RobbableContainer();
										armouredTrainCarrage.RobbableContainer2.Lock = @lock;
										armouredTrainCarrage.Container2.IsPositionFrozen = true;
										armouredTrainCarrage.Container2.Detach();
										armouredTrainCarrage.Container2.AddBlip();
										armouredTrainCarrage.Container2.AttachedBlip.Sprite = BlipSprite.ShippingContainer;
										armouredTrainCarrage.Container2.AttachedBlip.Color = BlipColor.White;
										armouredTrainCarrage.Container2.AttachedBlip.Alpha = 255;
										armouredTrainCarrage.Container2.AttachedBlip.Name = "Robbable Container";
										if (!armouredTrainCarrage.CreatedInteriorLoot)
										{
											if (armouredTrainCarrage.RobbableContainer1 != null)
											{
												BusinessCrateVehicleData businessCrateVehicleData = armouredTrainCarrage.RobbableContainer1.Train_Loot_ContainerChoices[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, armouredTrainCarrage.RobbableContainer1.Train_Loot_ContainerChoices.Count)];
												armouredTrainCarrage.RobbableContainer1.InteriorObjects = businessCrateVehicleData.VehicleData;
												foreach (ObjectPlacementMap interiorObject in armouredTrainCarrage.RobbableContainer1.InteriorObjects)
												{
													interiorObject.CreateObjatCoords();
													if (interiorObject.Entity != null)
													{
														interiorObject.Entity.IsCollisionEnabled = false;
														interiorObject.Entity.Position = interiorObject.Position;
														interiorObject.Entity.Rotation = interiorObject.Rotation;
														if (interiorObject.Entity != null)
														{
															ManageRobberyAnims.AddPropAsLoot((Prop)interiorObject.Entity);
														}
													}
												}
												foreach (ObjectPlacementMap interiorObject2 in armouredTrainCarrage.RobbableContainer1.InteriorObjects)
												{
													if (!(interiorObject2.Entity != null) || interiorObject2.EntityToAttachToHandle == -1)
													{
														continue;
													}
													foreach (ObjectPlacementMap interiorObject3 in armouredTrainCarrage.RobbableContainer1.InteriorObjects)
													{
														if (interiorObject3.Entity != null && interiorObject2.EntityToAttachToHandle == interiorObject3.EntityHandle)
														{
															if (interiorObject2.EntityToAttachBone == -1)
															{
																interiorObject2.EntityToAttachBone = 0;
															}
															interiorObject2.Entity.Position = interiorObject2.Position;
															interiorObject2.Entity.Rotation = interiorObject2.Rotation;
															Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, interiorObject2.Entity, interiorObject3.Entity, interiorObject2.EntityToAttachBone, interiorObject2.EntityToAttachOffset.X, interiorObject2.EntityToAttachOffset.Y, interiorObject2.EntityToAttachOffset.Z, interiorObject2.EntityToAttachRotation.X, interiorObject2.EntityToAttachRotation.Y, interiorObject2.EntityToAttachRotation.Z, false, false, true, false, 2, true);
															break;
														}
													}
												}
												if (armouredTrainCarrage.RobbableContainer1.InteriorObjects[0].Entity != null)
												{
													armouredTrainCarrage.RobbableContainer1.InteriorObjects[0].Entity.IsCollisionEnabled = false;
													armouredTrainCarrage.RobbableContainer1.InteriorObjects[0].Entity.Opacity = 0;
													Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, armouredTrainCarrage.RobbableContainer1.InteriorObjects[0].Entity, armouredTrainCarrage.Container1, 0, 0, 0, 0, 0, 0, 0, false, false, false, false, 2, true);
												}
											}
											if (armouredTrainCarrage.RobbableContainer2 != null)
											{
												BusinessCrateVehicleData businessCrateVehicleData2 = armouredTrainCarrage.RobbableContainer2.Train_Loot_ContainerChoices[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, armouredTrainCarrage.RobbableContainer2.Train_Loot_ContainerChoices.Count)];
												armouredTrainCarrage.RobbableContainer2.InteriorObjects = businessCrateVehicleData2.VehicleData;
												foreach (ObjectPlacementMap interiorObject4 in armouredTrainCarrage.RobbableContainer2.InteriorObjects)
												{
													interiorObject4.CreateObjatCoords();
													if (interiorObject4.Entity != null)
													{
														interiorObject4.Entity.IsCollisionEnabled = false;
														interiorObject4.Entity.Position = interiorObject4.Position;
														interiorObject4.Entity.Rotation = interiorObject4.Rotation;
														if (interiorObject4.Entity != null)
														{
															ManageRobberyAnims.AddPropAsLoot((Prop)interiorObject4.Entity);
														}
													}
												}
												foreach (ObjectPlacementMap interiorObject5 in armouredTrainCarrage.RobbableContainer2.InteriorObjects)
												{
													if (!(interiorObject5.Entity != null) || interiorObject5.EntityToAttachToHandle == -1)
													{
														continue;
													}
													foreach (ObjectPlacementMap interiorObject6 in armouredTrainCarrage.RobbableContainer2.InteriorObjects)
													{
														if (interiorObject6.Entity != null && interiorObject5.EntityToAttachToHandle == interiorObject6.EntityHandle)
														{
															if (interiorObject5.EntityToAttachBone == -1)
															{
																interiorObject5.EntityToAttachBone = 0;
															}
															interiorObject5.Entity.Position = interiorObject5.Position;
															interiorObject5.Entity.Rotation = interiorObject5.Rotation;
															Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, interiorObject5.Entity, interiorObject6.Entity, interiorObject5.EntityToAttachBone, interiorObject5.EntityToAttachOffset.X, interiorObject5.EntityToAttachOffset.Y, interiorObject5.EntityToAttachOffset.Z, interiorObject5.EntityToAttachRotation.X, interiorObject5.EntityToAttachRotation.Y, interiorObject5.EntityToAttachRotation.Z, false, false, true, false, 2, true);
															break;
														}
													}
												}
												if (armouredTrainCarrage.RobbableContainer2.InteriorObjects[0].Entity != null)
												{
													armouredTrainCarrage.RobbableContainer2.InteriorObjects[0].Entity.IsCollisionEnabled = false;
													armouredTrainCarrage.RobbableContainer2.InteriorObjects[0].Entity.Opacity = 0;
													Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, armouredTrainCarrage.RobbableContainer2.InteriorObjects[0].Entity, armouredTrainCarrage.Container2, 0, 0, 0, 0, 0, 0, 0, false, false, false, false, 2, true);
												}
											}
											armouredTrainCarrage.CreatedInteriorLoot = true;
										}
									}
									if (!armouredTrainCarrage.CreatedLoot)
									{
										continue;
									}
									if (armouredTrainCarrage.RobbableContainer1 != null)
									{
										if (GotGrinder)
										{
											switch (armouredTrainCarrage.RobbableContainer1.containerswitch)
											{
											case 0:
											{
												if (!(Game.Player.Character.Position.DistanceTo(new Vector3(armouredTrainCarrage.RobbableContainer1.Lock.Position.X, armouredTrainCarrage.RobbableContainer1.Lock.Position.Y, armouredTrainCarrage.RobbableContainer1.Lock.Position.Z)) < 2f))
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
												Vector3 vector7 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", "action", armouredTrainCarrage.Container1.Position.X, armouredTrainCarrage.Container1.Position.Y, armouredTrainCarrage.Container1.Position.Z, 0f, 0f, armouredTrainCarrage.Container1.Rotation.Z, 0f, 2);
												Vector3 vector8 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", "action", armouredTrainCarrage.Container1.Position.X, armouredTrainCarrage.Container1.Position.Y, armouredTrainCarrage.Container1.Position.Z, 0f, 0f, armouredTrainCarrage.Container1.Rotation.Z, 0f, 2);
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
													armouredTrainCarrage.RobbableContainer1.Bag = GlobalHelper.CreateProp(model, Game.Player.Character.Position, false, false);
													armouredTrainCarrage.RobbableContainer1.Bag.IsCollisionEnabled = false;
													armouredTrainCarrage.RobbableContainer1.containerswitch = 10;
												}
												Model model2 = new Model("tr_prop_tr_grinder_01a");
												model2.Request(100);
												if (model2.IsValid && model2.IsInCdImage)
												{
													while (!model2.IsLoaded)
													{
														Script.Wait(50);
													}
													armouredTrainCarrage.RobbableContainer1.Saw = GlobalHelper.CreateProp(model2, Game.Player.Character.Position, false, false);
													armouredTrainCarrage.RobbableContainer1.Saw.IsCollisionEnabled = false;
													armouredTrainCarrage.RobbableContainer1.containerswitch = 10;
												}
												Model model3 = new Model("tr_prop_tr_cont_coll_01a");
												model3.Request(100);
												if (model3.IsValid && model3.IsInCdImage)
												{
													while (!model3.IsLoaded)
													{
														Script.Wait(50);
													}
													armouredTrainCarrage.RobbableContainer1.FakeContainer = GlobalHelper.CreateProp(model3, armouredTrainCarrage.Container1.Position, false, false);
													armouredTrainCarrage.RobbableContainer1.FakeContainer.Rotation = new Vector3(0f, 0f, armouredTrainCarrage.Container1.Rotation.X);
													armouredTrainCarrage.RobbableContainer1.FakeContainer.IsCollisionEnabled = false;
													armouredTrainCarrage.RobbableContainer1.containerswitch = 10;
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
												armouredTrainCarrage.RobbableContainer1.SyncScene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, armouredTrainCarrage.Container1.Position.X, armouredTrainCarrage.Container1.Position.Y, armouredTrainCarrage.Container1.Position.Z, 0f, 0f, armouredTrainCarrage.Container1.Rotation.Z, 2);
												armouredTrainCarrage.RobbableContainer1.SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, armouredTrainCarrage.Container1.Position.X, armouredTrainCarrage.Container1.Position.Y, armouredTrainCarrage.Container1.Position.Z, 0f, 0f, armouredTrainCarrage.Container1.Rotation.Z, 2);
												Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, armouredTrainCarrage.RobbableContainer1.SyncScene2, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", "action", 4f, -15f, 3341, 16, 1148846080, 0);
												Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
												Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, armouredTrainCarrage.RobbableContainer1.Bag, armouredTrainCarrage.RobbableContainer1.SyncScene2, "action_bag", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
												Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, armouredTrainCarrage.RobbableContainer1.Lock, armouredTrainCarrage.RobbableContainer1.SyncScene, "action_lock", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
												Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, armouredTrainCarrage.RobbableContainer1.Saw, armouredTrainCarrage.RobbableContainer1.SyncScene2, "action_angle_grinder", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
												Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, armouredTrainCarrage.RobbableContainer1.FakeContainer, armouredTrainCarrage.RobbableContainer1.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
												Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, armouredTrainCarrage.Container1, armouredTrainCarrage.RobbableContainer1.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
												armouredTrainCarrage.RobbableContainer1.ParticleTimer = Game.GameTime + 4000;
												Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_tn_tr");
												armouredTrainCarrage.RobbableContainer1.containerswitch = 20;
												break;
											case 20:
												if (Game.GameTime > armouredTrainCarrage.RobbableContainer1.ParticleTimer)
												{
													Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_tn_tr");
													Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_tn_tr");
													armouredTrainCarrage.RobbableContainer1.ParticleFx = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_tn_tr_angle_grinder_sparks", armouredTrainCarrage.RobbableContainer1.Saw, 0f, 0.25f, 0f, 0f, 0f, 0f, 1f, false, false, false);
													armouredTrainCarrage.RobbableContainer1.ParticleTimer = Game.GameTime + 1000;
													armouredTrainCarrage.RobbableContainer1.containerswitch = 21;
												}
												break;
											case 21:
												if (Game.GameTime > armouredTrainCarrage.RobbableContainer1.ParticleTimer)
												{
													Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, armouredTrainCarrage.RobbableContainer1.ParticleFx);
													armouredTrainCarrage.RobbableContainer1.containerswitch = 30;
												}
												break;
											case 30:
												if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, armouredTrainCarrage.RobbableContainer1.SyncScene2) > 0.99f)
												{
													armouredTrainCarrage.RobbableContainer1.containerswitch = -100;
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
													Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, armouredTrainCarrage.RobbableContainer1.ParticleFx);
													Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, armouredTrainCarrage.RobbableContainer1.SyncScene, 0f);
													armouredTrainCarrage.RobbableContainer1.Bag.Delete();
													armouredTrainCarrage.RobbableContainer1.Saw.Delete();
													armouredTrainCarrage.Container1.IsCollisionEnabled = false;
													armouredTrainCarrage.RobbableContainer1.FakeContainer.IsCollisionEnabled = true;
													Game.Player.Character.Task.ClearAll();
												}
												break;
											case -100:
												if (armouredTrainCarrage.Container1.AttachedBlip != null)
												{
													armouredTrainCarrage.Container1.AttachedBlip.Alpha = 0;
												}
												armouredTrainCarrage.RobbableContainer1.containerswitch = -1;
												break;
											}
										}
										if (!GotGrinder && GotC4)
										{
											Vector3 offsetPosition = armouredTrainCarrage.Container1.GetOffsetPosition(new Vector3(0f, -1.8f, 1.5f));
											Vector3 rotation = armouredTrainCarrage.Container1.Rotation;
											switch (armouredTrainCarrage.RobbableContainer1.plantBombStage)
											{
											case 0:
											{
												if (!(Game.Player.Character.Position.DistanceTo(new Vector3(offsetPosition.X, offsetPosition.Y, offsetPosition.Z)) < 1.5f) || C4ChargesCollected < 1)
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
													armouredTrainCarrage.RobbableContainer1.Bomb = GlobalHelper.CreateProp(model4, Game.Player.Character.Position, false, false);
													armouredTrainCarrage.RobbableContainer1.Bomb.IsCollisionEnabled = false;
													armouredTrainCarrage.RobbableContainer1.plantBombStage = 10;
												}
												Model model5 = new Model("hei_p_m_bag_var22_arm_s");
												model5.Request(250);
												if (model5.IsValid && model5.IsInCdImage)
												{
													while (!model5.IsLoaded)
													{
														Script.Wait(50);
													}
													armouredTrainCarrage.RobbableContainer1.Bag = GlobalHelper.CreateProp(model5, Game.Player.Character.Position, false, false);
													armouredTrainCarrage.RobbableContainer1.Bag.IsCollisionEnabled = false;
													armouredTrainCarrage.RobbableContainer1.plantBombStage = 10;
												}
												break;
											}
											case 10:
												LoadDict("anim@scripted@heist@ig11_bomb_plant@male@");
												LoadDict("anim@scripted@player@mission@tun_bomb_plant@heeled@");
												armouredTrainCarrage.RobbableContainer1.SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z + 0.15f, 0f, 0f, rotation.Z, 2);
												Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, armouredTrainCarrage.RobbableContainer1.SyncScene, "anim@scripted@player@mission@tun_bomb_plant@heeled@", "enter", 4f, -15f, 3341, 16, 1148846080, 0);
												Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, armouredTrainCarrage.RobbableContainer1.Bomb, armouredTrainCarrage.RobbableContainer1.SyncScene, "enter_bomb", "anim@scripted@heist@ig11_bomb_plant@male@", 1f, -1f, 0, 1148846080);
												Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, armouredTrainCarrage.RobbableContainer1.Bag, armouredTrainCarrage.RobbableContainer1.SyncScene, "enter_bag", "anim@scripted@heist@ig11_bomb_plant@male@", 1f, -1f, 0, 1148846080);
												Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
												Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
												armouredTrainCarrage.RobbableContainer1.plantBombStage = 20;
												break;
											case 20:
												if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, armouredTrainCarrage.RobbableContainer1.SyncScene) > 0.99f)
												{
													armouredTrainCarrage.RobbableContainer1.Bag.Delete();
													Game.Player.Character.Task.ClearAll();
													armouredTrainCarrage.RobbableContainer1.BombExplodeTimer = Game.GameTime + 5000;
													armouredTrainCarrage.RobbableContainer1.plantBombStage = -1;
												}
												break;
											case -1:
												if (Game.GameTime >= armouredTrainCarrage.RobbableContainer1.BombExplodeTimer)
												{
													World.AddExplosion(armouredTrainCarrage.RobbableContainer1.Bomb.Position, ExplosionType.PipeBomb, 1f, 1f, Game.Player.Character);
													if (armouredTrainCarrage.RobbableContainer1.Bomb != null)
													{
														armouredTrainCarrage.RobbableContainer1.Bomb.Delete();
													}
													armouredTrainCarrage.RobbableContainer1.FakeContainer = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_cont_coll_01a"), armouredTrainCarrage.Container1.Position, false, false);
													armouredTrainCarrage.RobbableContainer1.FakeContainer.Rotation = new Vector3(0f, 0f, armouredTrainCarrage.Container1.Rotation.X);
													armouredTrainCarrage.RobbableContainer1.FakeContainer.IsCollisionEnabled = false;
													LoadDict("anim@scripted@player@mission@tunf_train_ig1_container_p1@male@");
													Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@");
													armouredTrainCarrage.RobbableContainer1.SyncScene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, armouredTrainCarrage.Container1.Position.X, armouredTrainCarrage.Container1.Position.Y, armouredTrainCarrage.Container1.Position.Z, 0f, 0f, armouredTrainCarrage.Container1.Rotation.Z, 2);
													armouredTrainCarrage.RobbableContainer1.SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, armouredTrainCarrage.Container1.Position.X, armouredTrainCarrage.Container1.Position.Y, armouredTrainCarrage.Container1.Position.Z, 0f, 0f, armouredTrainCarrage.Container1.Rotation.Z, 2);
													Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, armouredTrainCarrage.RobbableContainer1.FakeContainer, armouredTrainCarrage.RobbableContainer1.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
													Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, armouredTrainCarrage.Container1, armouredTrainCarrage.RobbableContainer1.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
													if (armouredTrainCarrage.RobbableContainer1.Lock != null)
													{
														armouredTrainCarrage.RobbableContainer1.Lock.Delete();
													}
													if (armouredTrainCarrage.RobbableContainer1.Bomb != null)
													{
														armouredTrainCarrage.RobbableContainer1.Bomb.Delete();
													}
													armouredTrainCarrage.RobbableContainer1.plantBombStage = -2;
												}
												break;
											case -2:
												armouredTrainCarrage.Container1.IsCollisionEnabled = false;
												armouredTrainCarrage.RobbableContainer1.FakeContainer.IsCollisionEnabled = true;
												Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, armouredTrainCarrage.RobbableContainer1.SyncScene, 1f);
												Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, armouredTrainCarrage.RobbableContainer1.SyncScene2, 1f);
												if (C4ChargesCollected <= 0)
												{
													GotC4 = false;
												}
												break;
											}
										}
										if (!GotGrinder && !GotC4 && GotThermite)
										{
											Vector3 offsetPosition2 = armouredTrainCarrage.Container1.GetOffsetPosition(new Vector3(0f, -1.85f, 1.2f));
											Vector3 rotation2 = armouredTrainCarrage.Container1.Rotation;
											switch (armouredTrainCarrage.RobbableContainer1.plantBombStage)
											{
											case 0:
												if (!(Game.Player.Character.Position.DistanceTo(new Vector3(offsetPosition2.X, offsetPosition2.Y, offsetPosition2.Z)) < 1.5f) || ThermiteChargesCollected < 1)
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
													if (armouredTrainCarrage.RobbableContainer1.Bomb != null)
													{
														armouredTrainCarrage.RobbableContainer1.Bomb.Delete();
													}
													if (armouredTrainCarrage.RobbableContainer1.Bag != null)
													{
														armouredTrainCarrage.RobbableContainer1.Bag.Delete();
													}
													armouredTrainCarrage.RobbableContainer1.Bag = GlobalHelper.CreateProp(RequestModel("ch_p_m_bag_var02_arm_s"), offsetPosition2, new Vector3(1f, 0f, 0f), false, false);
													armouredTrainCarrage.RobbableContainer1.SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition2.X, offsetPosition2.Y, offsetPosition2.Z, 0.0, 0.0, rotation2.Z, 2);
													Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, armouredTrainCarrage.RobbableContainer1.SyncScene, LoadDict("anim@heists@ornate_bank@thermal_charge"), "thermal_charge", 1f, -1f, 3341, 16, 1148846080, 0);
													Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, armouredTrainCarrage.RobbableContainer1.Bag, armouredTrainCarrage.RobbableContainer1.SyncScene, "bag_thermal_charge", LoadDict("anim@heists@ornate_bank@thermal_charge"), 1f, -1f, 0, 1148846080);
													Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, armouredTrainCarrage.RobbableContainer1.Bag);
													armouredTrainCarrage.RobbableContainer1.Bomb = GlobalHelper.CreateProp(RequestModel("hei_prop_heist_thermite"), Game.Player.Character.Position, false, false);
													Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, armouredTrainCarrage.RobbableContainer1.Bomb, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, -90f, 0f, 0f, 0, 0, 0, 0, 2, 1);
													Function.Call(Hash.PLAY_ENTITY_ANIM, armouredTrainCarrage.RobbableContainer1.Bomb, "bomb_thermal_charge", "anim@heists@ornate_bank@thermal_charge", 1000.0, 1, 1, 1, 0, 0);
													armouredTrainCarrage.RobbableContainer1.plantBombStage = 10;
												}
												break;
											case 10:
												if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, armouredTrainCarrage.RobbableContainer1.SyncScene) > 0.5f)
												{
													armouredTrainCarrage.RobbableContainer1.Bomb.Detach();
													armouredTrainCarrage.RobbableContainer1.Bomb.IsPositionFrozen = true;
													Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ornate_heist");
													Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ornate_heist");
													armouredTrainCarrage.RobbableContainer1.PTFX1 = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_heist_ornate_thermal_burn", armouredTrainCarrage.RobbableContainer1.Bomb, 0f, 0f, 0f, 1f, 0, 0, 0);
													Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
													Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
													armouredTrainCarrage.RobbableContainer1.PTFX2 = GlobalHelper.CreatePTFX_LoopedOnEntity("proj_flare_fuse_fp", armouredTrainCarrage.RobbableContainer1.Bomb, 0f, 0f, 0f, 10f, 0, 0, 0);
													Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
													armouredTrainCarrage.RobbableContainer1.plantBombStage = 20;
												}
												break;
											case 20:
												if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, armouredTrainCarrage.RobbableContainer1.SyncScene) > 0.99f)
												{
													armouredTrainCarrage.RobbableContainer1.Bag.Delete();
													armouredTrainCarrage.RobbableContainer1.Bomb.IsPositionFrozen = true;
													Game.Player.Character.Task.ClearAll();
													armouredTrainCarrage.RobbableContainer1.BombExplodeTimer = Game.GameTime + 7500;
													armouredTrainCarrage.RobbableContainer1.plantBombStage = -1;
												}
												break;
											case -1:
												if (Game.GameTime >= armouredTrainCarrage.RobbableContainer1.BombExplodeTimer)
												{
													Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, armouredTrainCarrage.RobbableContainer1.PTFX2, false);
													Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, armouredTrainCarrage.RobbableContainer1.PTFX1, false);
													Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "Gate_Lock_Break", armouredTrainCarrage.RobbableContainer1.Bomb, "DLC_HEISTS_ORNATE_BANK_FINALE_SOUNDS", 0, 0);
													if (armouredTrainCarrage.RobbableContainer1.Bomb != null)
													{
														armouredTrainCarrage.RobbableContainer1.Bomb.Delete();
													}
													armouredTrainCarrage.RobbableContainer1.FakeContainer = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_cont_coll_01a"), armouredTrainCarrage.Container1.Position, false, false);
													armouredTrainCarrage.RobbableContainer1.FakeContainer.Rotation = new Vector3(0f, 0f, armouredTrainCarrage.Container1.Rotation.X);
													armouredTrainCarrage.RobbableContainer1.FakeContainer.IsCollisionEnabled = false;
													LoadDict("anim@scripted@player@mission@tunf_train_ig1_container_p1@male@");
													Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@");
													armouredTrainCarrage.RobbableContainer1.SyncScene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, armouredTrainCarrage.Container1.Position.X, armouredTrainCarrage.Container1.Position.Y, armouredTrainCarrage.Container1.Position.Z, 0f, 0f, armouredTrainCarrage.Container1.Rotation.Z, 2);
													armouredTrainCarrage.RobbableContainer1.SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, armouredTrainCarrage.Container1.Position.X, armouredTrainCarrage.Container1.Position.Y, armouredTrainCarrage.Container1.Position.Z, 0f, 0f, armouredTrainCarrage.Container1.Rotation.Z, 2);
													Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, armouredTrainCarrage.RobbableContainer1.FakeContainer, armouredTrainCarrage.RobbableContainer1.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
													Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, armouredTrainCarrage.Container1, armouredTrainCarrage.RobbableContainer1.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
													if (armouredTrainCarrage.RobbableContainer1.Lock != null)
													{
														armouredTrainCarrage.RobbableContainer1.Lock.Delete();
													}
													if (armouredTrainCarrage.RobbableContainer1.Bomb != null)
													{
														armouredTrainCarrage.RobbableContainer1.Bomb.Delete();
													}
													Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, armouredTrainCarrage.RobbableContainer1.SyncScene, 0.65f);
													Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, armouredTrainCarrage.RobbableContainer1.SyncScene2, 0.65f);
													armouredTrainCarrage.RobbableContainer1.plantBombStage = -2;
												}
												break;
											case -2:
												armouredTrainCarrage.Container1.IsCollisionEnabled = false;
												armouredTrainCarrage.RobbableContainer1.FakeContainer.IsCollisionEnabled = true;
												Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, armouredTrainCarrage.RobbableContainer1.SyncScene, 0.65f);
												Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, armouredTrainCarrage.RobbableContainer1.SyncScene2, 0.65f);
												if (ThermiteChargesCollected <= 0)
												{
													GotThermite = false;
												}
												break;
											}
										}
									}
									if (armouredTrainCarrage.RobbableContainer2 == null)
									{
										continue;
									}
									if (GotGrinder)
									{
										switch (armouredTrainCarrage.RobbableContainer2.containerswitch)
										{
										case 0:
										{
											if (!(Game.Player.Character.Position.DistanceTo(new Vector3(armouredTrainCarrage.RobbableContainer2.Lock.Position.X, armouredTrainCarrage.RobbableContainer2.Lock.Position.Y, armouredTrainCarrage.RobbableContainer2.Lock.Position.Z)) < 2f))
											{
												break;
											}
											DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to break lock", 1);
											if (!Game.IsControlJustPressed(Control.Context))
											{
												break;
											}
											Ped character3 = Game.Player.Character;
											if (Game.Player.Character.Model == PedHash.Trevor)
											{
												Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character3, 9, 0, 0, 1);
											}
											if (Game.Player.Character.Model == PedHash.Michael)
											{
												Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character3, 9, 0, 0, 1);
											}
											if (Game.Player.Character.Model == PedHash.Franklin)
											{
												Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character3, 9, 0, 0, 1);
											}
											Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_CM2022/CM2022_GENERIC_01");
											Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_TUNER/DLC_Tuner_Generic");
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_CM2022/CM2022_GENERIC_01", false);
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_TUNER/DLC_Tuner_Generic", false);
											Vector3 vector9 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", "action", armouredTrainCarrage.Container2.Position.X, armouredTrainCarrage.Container2.Position.Y, armouredTrainCarrage.Container2.Position.Z, 0f, 0f, armouredTrainCarrage.Container2.Rotation.Z, 0f, 2);
											Vector3 vector10 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", "action", armouredTrainCarrage.Container2.Position.X, armouredTrainCarrage.Container2.Position.Y, armouredTrainCarrage.Container2.Position.Z, 0f, 0f, armouredTrainCarrage.Container2.Rotation.Z, 0f, 2);
											Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@");
											Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_CM2022/CM2022_GENERIC_01");
											Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_TUNER/DLC_Tuner_Generic");
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_CM2022/CM2022_GENERIC_01", false);
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_TUNER/DLC_Tuner_Generic", false);
											Model model6 = new Model("ch_p_m_bag_var04_arm_s");
											model6.Request(10000);
											if (model6.IsValid && model6.IsInCdImage)
											{
												while (!model6.IsLoaded)
												{
													Script.Wait(50);
												}
												armouredTrainCarrage.RobbableContainer2.Bag = GlobalHelper.CreateProp(model6, Game.Player.Character.Position, false, false);
												armouredTrainCarrage.RobbableContainer2.Bag.IsCollisionEnabled = false;
												armouredTrainCarrage.RobbableContainer2.containerswitch = 10;
											}
											Model model7 = new Model("tr_prop_tr_grinder_01a");
											model7.Request(100);
											if (model7.IsValid && model7.IsInCdImage)
											{
												while (!model7.IsLoaded)
												{
													Script.Wait(50);
												}
												armouredTrainCarrage.RobbableContainer2.Saw = GlobalHelper.CreateProp(model7, Game.Player.Character.Position, false, false);
												armouredTrainCarrage.RobbableContainer2.Saw.IsCollisionEnabled = false;
												armouredTrainCarrage.RobbableContainer2.containerswitch = 10;
											}
											Model model8 = new Model("tr_prop_tr_cont_coll_01a");
											model8.Request(100);
											if (model8.IsValid && model8.IsInCdImage)
											{
												while (!model8.IsLoaded)
												{
													Script.Wait(50);
												}
												armouredTrainCarrage.RobbableContainer2.FakeContainer = GlobalHelper.CreateProp(model8, armouredTrainCarrage.Container2.Position, false, false);
												armouredTrainCarrage.RobbableContainer2.FakeContainer.Rotation = new Vector3(0f, 0f, armouredTrainCarrage.Container2.Rotation.X);
												armouredTrainCarrage.RobbableContainer2.FakeContainer.IsCollisionEnabled = false;
												armouredTrainCarrage.RobbableContainer2.containerswitch = 10;
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
											armouredTrainCarrage.RobbableContainer2.SyncScene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, armouredTrainCarrage.Container2.Position.X, armouredTrainCarrage.Container2.Position.Y, armouredTrainCarrage.Container2.Position.Z, 0f, 0f, armouredTrainCarrage.Container2.Rotation.Z, 2);
											armouredTrainCarrage.RobbableContainer2.SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, armouredTrainCarrage.Container2.Position.X, armouredTrainCarrage.Container2.Position.Y, armouredTrainCarrage.Container2.Position.Z, 0f, 0f, armouredTrainCarrage.Container2.Rotation.Z, 2);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, armouredTrainCarrage.RobbableContainer2.SyncScene2, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", "action", 4f, -15f, 3341, 16, 1148846080, 0);
											Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, armouredTrainCarrage.RobbableContainer2.Bag, armouredTrainCarrage.RobbableContainer2.SyncScene2, "action_bag", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, armouredTrainCarrage.RobbableContainer2.Lock, armouredTrainCarrage.RobbableContainer2.SyncScene, "action_lock", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, armouredTrainCarrage.RobbableContainer2.Saw, armouredTrainCarrage.RobbableContainer2.SyncScene2, "action_angle_grinder", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, armouredTrainCarrage.RobbableContainer2.FakeContainer, armouredTrainCarrage.RobbableContainer2.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, armouredTrainCarrage.Container2, armouredTrainCarrage.RobbableContainer2.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
											armouredTrainCarrage.RobbableContainer2.ParticleTimer = Game.GameTime + 4000;
											Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_tn_tr");
											armouredTrainCarrage.RobbableContainer2.containerswitch = 20;
											break;
										case 20:
											if (Game.GameTime > armouredTrainCarrage.RobbableContainer2.ParticleTimer)
											{
												Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_tn_tr");
												Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_tn_tr");
												armouredTrainCarrage.RobbableContainer2.ParticleFx = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_tn_tr_angle_grinder_sparks", armouredTrainCarrage.RobbableContainer2.Saw, 0f, 0.25f, 0f, 0f, 0f, 0f, 1f, false, false, false);
												armouredTrainCarrage.RobbableContainer2.ParticleTimer = Game.GameTime + 1000;
												armouredTrainCarrage.RobbableContainer2.containerswitch = 21;
											}
											break;
										case 21:
											if (Game.GameTime > armouredTrainCarrage.RobbableContainer2.ParticleTimer)
											{
												Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, armouredTrainCarrage.RobbableContainer2.ParticleFx);
												armouredTrainCarrage.RobbableContainer2.containerswitch = 30;
											}
											break;
										case 30:
											if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, armouredTrainCarrage.RobbableContainer2.SyncScene2) > 0.99f)
											{
												armouredTrainCarrage.RobbableContainer2.containerswitch = -100;
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
												Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, armouredTrainCarrage.RobbableContainer2.ParticleFx);
												Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, armouredTrainCarrage.RobbableContainer2.SyncScene, 0f);
												armouredTrainCarrage.RobbableContainer2.Bag.Delete();
												armouredTrainCarrage.RobbableContainer2.Saw.Delete();
												armouredTrainCarrage.Container2.IsCollisionEnabled = false;
												armouredTrainCarrage.RobbableContainer2.FakeContainer.IsCollisionEnabled = true;
												Game.Player.Character.Task.ClearAll();
											}
											break;
										case -100:
											if (armouredTrainCarrage.Container2.AttachedBlip != null)
											{
												armouredTrainCarrage.Container2.AttachedBlip.Alpha = 0;
											}
											armouredTrainCarrage.RobbableContainer2.containerswitch = -1;
											break;
										}
									}
									if (!GotGrinder && GotC4)
									{
										Vector3 offsetPosition3 = armouredTrainCarrage.Container2.GetOffsetPosition(new Vector3(0f, -1.8f, 1.5f));
										Vector3 rotation3 = armouredTrainCarrage.Container2.Rotation;
										switch (armouredTrainCarrage.RobbableContainer2.plantBombStage)
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
											Model model9 = new Model("ch_prop_ch_ld_bomb_01a");
											model9.Request(250);
											if (model9.IsValid && model9.IsInCdImage)
											{
												while (!model9.IsLoaded)
												{
													Script.Wait(50);
												}
												armouredTrainCarrage.RobbableContainer2.Bomb = GlobalHelper.CreateProp(model9, Game.Player.Character.Position, false, false);
												armouredTrainCarrage.RobbableContainer2.Bomb.IsCollisionEnabled = false;
												armouredTrainCarrage.RobbableContainer2.plantBombStage = 10;
											}
											Model model10 = new Model("hei_p_m_bag_var22_arm_s");
											model10.Request(250);
											if (model10.IsValid && model10.IsInCdImage)
											{
												while (!model10.IsLoaded)
												{
													Script.Wait(50);
												}
												armouredTrainCarrage.RobbableContainer2.Bag = GlobalHelper.CreateProp(model10, Game.Player.Character.Position, false, false);
												armouredTrainCarrage.RobbableContainer2.Bag.IsCollisionEnabled = false;
												armouredTrainCarrage.RobbableContainer2.plantBombStage = 10;
											}
											break;
										}
										case 10:
											LoadDict("anim@scripted@heist@ig11_bomb_plant@male@");
											LoadDict("anim@scripted@player@mission@tun_bomb_plant@heeled@");
											armouredTrainCarrage.RobbableContainer2.SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition3.X, offsetPosition3.Y, offsetPosition3.Z + 0.15f, 0f, 0f, rotation3.Z, 2);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, armouredTrainCarrage.RobbableContainer2.SyncScene, "anim@scripted@player@mission@tun_bomb_plant@heeled@", "enter", 4f, -15f, 3341, 16, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, armouredTrainCarrage.RobbableContainer2.Bomb, armouredTrainCarrage.RobbableContainer2.SyncScene, "enter_bomb", "anim@scripted@heist@ig11_bomb_plant@male@", 1f, -1f, 0, 1148846080);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, armouredTrainCarrage.RobbableContainer2.Bag, armouredTrainCarrage.RobbableContainer2.SyncScene, "enter_bag", "anim@scripted@heist@ig11_bomb_plant@male@", 1f, -1f, 0, 1148846080);
											Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
											Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
											armouredTrainCarrage.RobbableContainer2.plantBombStage = 20;
											break;
										case 20:
											if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, armouredTrainCarrage.RobbableContainer2.SyncScene) > 0.99f)
											{
												armouredTrainCarrage.RobbableContainer2.Bag.Delete();
												Game.Player.Character.Task.ClearAll();
												armouredTrainCarrage.RobbableContainer2.BombExplodeTimer = Game.GameTime + 5000;
												armouredTrainCarrage.RobbableContainer2.plantBombStage = -1;
											}
											break;
										case -1:
											if (Game.GameTime >= armouredTrainCarrage.RobbableContainer2.BombExplodeTimer)
											{
												World.AddExplosion(armouredTrainCarrage.RobbableContainer2.Bomb.Position, ExplosionType.PipeBomb, 1f, 1f, Game.Player.Character);
												if (armouredTrainCarrage.RobbableContainer2.Bomb != null)
												{
													armouredTrainCarrage.RobbableContainer2.Bomb.Delete();
												}
												armouredTrainCarrage.RobbableContainer2.FakeContainer = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_cont_coll_01a"), armouredTrainCarrage.Container2.Position, false, false);
												armouredTrainCarrage.RobbableContainer2.FakeContainer.Rotation = new Vector3(0f, 0f, armouredTrainCarrage.Container2.Rotation.X);
												armouredTrainCarrage.RobbableContainer2.FakeContainer.IsCollisionEnabled = false;
												LoadDict("anim@scripted@player@mission@tunf_train_ig1_container_p1@male@");
												Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@");
												armouredTrainCarrage.RobbableContainer2.SyncScene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, armouredTrainCarrage.Container2.Position.X, armouredTrainCarrage.Container2.Position.Y, armouredTrainCarrage.Container2.Position.Z, 0f, 0f, armouredTrainCarrage.Container2.Rotation.Z, 2);
												armouredTrainCarrage.RobbableContainer2.SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, armouredTrainCarrage.Container2.Position.X, armouredTrainCarrage.Container2.Position.Y, armouredTrainCarrage.Container2.Position.Z, 0f, 0f, armouredTrainCarrage.Container2.Rotation.Z, 2);
												Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, armouredTrainCarrage.RobbableContainer2.FakeContainer, armouredTrainCarrage.RobbableContainer2.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
												Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, armouredTrainCarrage.Container2, armouredTrainCarrage.RobbableContainer2.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
												if (armouredTrainCarrage.RobbableContainer2.Lock != null)
												{
													armouredTrainCarrage.RobbableContainer2.Lock.Delete();
												}
												if (armouredTrainCarrage.RobbableContainer2.Bomb != null)
												{
													armouredTrainCarrage.RobbableContainer2.Bomb.Delete();
												}
												armouredTrainCarrage.RobbableContainer2.plantBombStage = -2;
											}
											break;
										case -2:
											armouredTrainCarrage.Container2.IsCollisionEnabled = false;
											armouredTrainCarrage.RobbableContainer2.FakeContainer.IsCollisionEnabled = true;
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, armouredTrainCarrage.RobbableContainer2.SyncScene, 1f);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, armouredTrainCarrage.RobbableContainer2.SyncScene2, 1f);
											if (C4ChargesCollected <= 0)
											{
												GotC4 = false;
											}
											break;
										}
									}
									if (GotGrinder || GotC4 || !GotThermite)
									{
										continue;
									}
									Vector3 offsetPosition4 = armouredTrainCarrage.Container2.GetOffsetPosition(new Vector3(0f, -1.85f, 1.2f));
									Vector3 rotation4 = armouredTrainCarrage.Container2.Rotation;
									switch (armouredTrainCarrage.RobbableContainer2.plantBombStage)
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
											Ped character4 = Game.Player.Character;
											if (Game.Player.Character.Model == PedHash.Trevor)
											{
												Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 9, 0, 0, 1);
											}
											if (Game.Player.Character.Model == PedHash.Michael)
											{
												Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 9, 0, 0, 1);
											}
											if (Game.Player.Character.Model == PedHash.Franklin)
											{
												Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 9, 0, 0, 1);
											}
											string dict3 = "anim_heist@hs3f@ig13_thermal_charge@thermal_charge@male@";
											string dict4 = "anim@heists@ornate_bank@thermal_charge";
											LoadDict(dict3);
											LoadDict(dict3);
											LoadDict(dict4);
											LoadDict(dict4);
											if (armouredTrainCarrage.RobbableContainer2.Bomb != null)
											{
												armouredTrainCarrage.RobbableContainer2.Bomb.Delete();
											}
											if (armouredTrainCarrage.RobbableContainer2.Bag != null)
											{
												armouredTrainCarrage.RobbableContainer2.Bag.Delete();
											}
											armouredTrainCarrage.RobbableContainer2.Bag = GlobalHelper.CreateProp(RequestModel("ch_p_m_bag_var02_arm_s"), offsetPosition4, new Vector3(1f, 0f, 0f), false, false);
											armouredTrainCarrage.RobbableContainer2.SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition4.X, offsetPosition4.Y, offsetPosition4.Z, 0.0, 0.0, rotation4.Z, 2);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, armouredTrainCarrage.RobbableContainer2.SyncScene, LoadDict("anim@heists@ornate_bank@thermal_charge"), "thermal_charge", 1f, -1f, 3341, 16, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, armouredTrainCarrage.RobbableContainer2.Bag, armouredTrainCarrage.RobbableContainer2.SyncScene, "bag_thermal_charge", LoadDict("anim@heists@ornate_bank@thermal_charge"), 1f, -1f, 0, 1148846080);
											Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, armouredTrainCarrage.RobbableContainer2.Bag);
											armouredTrainCarrage.RobbableContainer2.Bomb = GlobalHelper.CreateProp(RequestModel("hei_prop_heist_thermite"), Game.Player.Character.Position, false, false);
											Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, armouredTrainCarrage.RobbableContainer2.Bomb, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, -90f, 0f, 0f, 0, 0, 0, 0, 2, 1);
											Function.Call(Hash.PLAY_ENTITY_ANIM, armouredTrainCarrage.RobbableContainer2.Bomb, "bomb_thermal_charge", "anim@heists@ornate_bank@thermal_charge", 1000.0, 1, 1, 1, 0, 0);
											armouredTrainCarrage.RobbableContainer2.plantBombStage = 10;
										}
										break;
									case 10:
										if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, armouredTrainCarrage.RobbableContainer2.SyncScene) > 0.5f)
										{
											armouredTrainCarrage.RobbableContainer2.Bomb.Detach();
											armouredTrainCarrage.RobbableContainer2.Bomb.IsPositionFrozen = true;
											Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ornate_heist");
											Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ornate_heist");
											armouredTrainCarrage.RobbableContainer2.PTFX1 = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_heist_ornate_thermal_burn", armouredTrainCarrage.RobbableContainer2.Bomb, 0f, 0f, 0f, 1f, 0, 0, 0);
											Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
											Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
											armouredTrainCarrage.RobbableContainer2.PTFX2 = GlobalHelper.CreatePTFX_LoopedOnEntity("proj_flare_fuse_fp", armouredTrainCarrage.RobbableContainer2.Bomb, 0f, 0f, 0f, 10f, 0, 0, 0);
											Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
											armouredTrainCarrage.RobbableContainer2.plantBombStage = 20;
										}
										break;
									case 20:
										if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, armouredTrainCarrage.RobbableContainer2.SyncScene) > 0.99f)
										{
											armouredTrainCarrage.RobbableContainer2.Bag.Delete();
											armouredTrainCarrage.RobbableContainer2.Bomb.IsPositionFrozen = true;
											Game.Player.Character.Task.ClearAll();
											armouredTrainCarrage.RobbableContainer2.BombExplodeTimer = Game.GameTime + 7500;
											armouredTrainCarrage.RobbableContainer2.plantBombStage = -1;
										}
										break;
									case -1:
										if (Game.GameTime >= armouredTrainCarrage.RobbableContainer2.BombExplodeTimer)
										{
											Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, armouredTrainCarrage.RobbableContainer2.PTFX2, false);
											Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, armouredTrainCarrage.RobbableContainer2.PTFX1, false);
											Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "Gate_Lock_Break", armouredTrainCarrage.RobbableContainer2.Bomb, "DLC_HEISTS_ORNATE_BANK_FINALE_SOUNDS", 0, 0);
											if (armouredTrainCarrage.RobbableContainer2.Bomb != null)
											{
												armouredTrainCarrage.RobbableContainer2.Bomb.Delete();
											}
											armouredTrainCarrage.RobbableContainer2.FakeContainer = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_cont_coll_01a"), armouredTrainCarrage.Container2.Position, false, false);
											armouredTrainCarrage.RobbableContainer2.FakeContainer.Rotation = new Vector3(0f, 0f, armouredTrainCarrage.Container2.Rotation.X);
											armouredTrainCarrage.RobbableContainer2.FakeContainer.IsCollisionEnabled = false;
											LoadDict("anim@scripted@player@mission@tunf_train_ig1_container_p1@male@");
											Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@");
											armouredTrainCarrage.RobbableContainer2.SyncScene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, armouredTrainCarrage.Container2.Position.X, armouredTrainCarrage.Container2.Position.Y, armouredTrainCarrage.Container2.Position.Z, 0f, 0f, armouredTrainCarrage.Container2.Rotation.Z, 2);
											armouredTrainCarrage.RobbableContainer2.SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, armouredTrainCarrage.Container2.Position.X, armouredTrainCarrage.Container2.Position.Y, armouredTrainCarrage.Container2.Position.Z, 0f, 0f, armouredTrainCarrage.Container2.Rotation.Z, 2);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, armouredTrainCarrage.RobbableContainer2.FakeContainer, armouredTrainCarrage.RobbableContainer2.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, armouredTrainCarrage.Container2, armouredTrainCarrage.RobbableContainer2.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
											if (armouredTrainCarrage.RobbableContainer2.Lock != null)
											{
												armouredTrainCarrage.RobbableContainer2.Lock.Delete();
											}
											if (armouredTrainCarrage.RobbableContainer2.Bomb != null)
											{
												armouredTrainCarrage.RobbableContainer2.Bomb.Delete();
											}
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, armouredTrainCarrage.RobbableContainer2.SyncScene, 0.65f);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, armouredTrainCarrage.RobbableContainer2.SyncScene2, 0.65f);
											armouredTrainCarrage.RobbableContainer2.plantBombStage = -2;
										}
										break;
									case -2:
										armouredTrainCarrage.Container2.IsCollisionEnabled = false;
										armouredTrainCarrage.RobbableContainer2.FakeContainer.IsCollisionEnabled = true;
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, armouredTrainCarrage.RobbableContainer2.SyncScene, 0.65f);
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, armouredTrainCarrage.RobbableContainer2.SyncScene2, 0.65f);
										if (ThermiteChargesCollected <= 0)
										{
											GotThermite = false;
										}
										break;
									}
								}
							}
						}
						if (ArmouredTrainTest.Train != null)
						{
							if (TrainStage == 0)
							{
								if (ArmouredTrainTest.Train != null && ArmouredTrainTest.Train.AttachedBlip != null)
								{
									ArmouredTrainTest.Train.AttachedBlip.Sprite = BlipSprite.Train;
									ArmouredTrainTest.Train.AttachedBlip.Name = "Federal Reserve Train";
									ArmouredTrainTest.Train.AttachedBlip.Color = BlipColor.Yellow;
									ArmouredTrainTest.Train.AttachedBlip.Alpha = 255;
									ArmouredTrainTest.Train.AttachedBlip.Position = ArmouredTrainTest.Train.GetOffsetPosition(new Vector3(0f, -20f, 0f));
								}
								if (ArmouredTrainTest.DeliveryPoint != null)
								{
									ArmouredTrainTest.DeliveryPoint.Alpha = 0;
								}
								if (ArmouredTrainTest.Radius != null)
								{
									ArmouredTrainTest.Radius.Alpha = 0;
								}
								if (ArmouredTrainTest.Train != null)
								{
									if (GetDistance_VDIST2(Game.Player.Character.Position, ArmouredTrainTest.Train.Position, "<", 150f))
									{
										if (ArmouredTrainTest.BrakeCylinders[0].Health > 0)
										{
											DrawProgressBar_Advanced("BRAKE CYLINDER", 5, ArmouredTrainTest.BrakeCylinders[0].Health, 1000f, Color.Green, false);
										}
										else
										{
											DrawProgressBar_Advanced("BRAKE CYLINDER", 5, 0f, 900f, Color.Green, false);
											if (!BrakeCyldr1Dead)
											{
												BrakeCyldr1Dead = true;
												Vector3 vector11 = Game.Player.Character.Position.Around(Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 75, 200));
											}
										}
										if (ArmouredTrainTest.BrakeCylinders[1].Health > 0)
										{
											DrawProgressBar_Advanced("BRAKE CYLINDER", 6, ArmouredTrainTest.BrakeCylinders[1].Health, 1000f, Color.Green, false);
										}
										else
										{
											DrawProgressBar_Advanced("BRAKE CYLINDER", 6, 0f, 900f, Color.Green, false);
											if (!BrakeCyldr2Dead)
											{
												BrakeCyldr2Dead = true;
												Vector3 vector12 = Game.Player.Character.Position.Around(Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 75, 200));
											}
										}
										if (ArmouredTrainTest.BrakeCylinders[2].Health > 0)
										{
											DrawProgressBar_Advanced("BRAKE CYLINDER", 7, ArmouredTrainTest.BrakeCylinders[2].Health, 1000f, Color.Green, false);
										}
										else
										{
											DrawProgressBar_Advanced("BRAKE CYLINDER", 7, 0f, 900f, Color.Green, false);
											if (!BrakeCyldr3Dead)
											{
												BrakeCyldr3Dead = true;
												Vector3 vector13 = Game.Player.Character.Position.Around(Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 75, 200));
											}
										}
										if (ArmouredTrainTest.BrakeCylinders[3].Health > 0)
										{
											DrawProgressBar_Advanced("BRAKE CYLINDER", 8, ArmouredTrainTest.BrakeCylinders[3].Health, 1000f, Color.Green, false);
										}
										else
										{
											DrawProgressBar_Advanced("BRAKE CYLINDER", 8, 0f, 900f, Color.Green, false);
											if (!BrakeCyldr4Dead)
											{
												BrakeCyldr4Dead = true;
												Vector3 vector14 = Game.Player.Character.Position.Around(Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 75, 200));
											}
										}
									}
									if ((ArmouredTrainTest.BrakeCylinders[0].Health <= 0 && ArmouredTrainTest.BrakeCylinders[1].Health <= 0 && ArmouredTrainTest.BrakeCylinders[2].Health <= 0 && ArmouredTrainTest.BrakeCylinders[3].Health <= 0) || HasSwappedTrainRouteToEnterQuarry || TrainIsStoppingAtYard || StoppedTrainUsingC4)
									{
										TrainStage = 1;
									}
								}
							}
							if (TrainStage == 1 && ArmouredTrainTest.Train != null)
							{
								if (ArmouredTrainTest.TrainSpeed > 0f)
								{
									ArmouredTrainTest.TrainSpeed -= 0.025f;
								}
								if (ArmouredTrainTest.TrainSpeed <= 0f)
								{
									ArmouredTrainTest.TrainSpeed = 0f;
									if (!ArmouredTrainTest.TrainFullyStopped)
									{
										TrainHandler.Trains.Add(ArmouredTrainTest.Train);
										ArmouredTrainTest.TrainFullyStopped = true;
									}
								}
								ArmouredTrainTest.Train.ForwardSpeed = ArmouredTrainTest.TrainSpeed;
								Function.Call(Hash.SET_TRAIN_SPEED, ArmouredTrainTest.Train, ArmouredTrainTest.TrainSpeed);
								Function.Call(Hash.SET_TRAIN_CRUISE_SPEED, ArmouredTrainTest.Train, ArmouredTrainTest.TrainSpeed);
							}
						}
					}
				}
				if (Lift1 == null)
				{
					Lift1 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, -182.2164f, -1016.441f, 31.36157f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "prop_conslift_lift"), 0, 0, 0);
				}
				if (Lift2 == null)
				{
					Lift2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, -182.2239f, -1016.448f, 115.5188f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "prop_conslift_lift"), 0, 0, 0);
				}
				if (Lift1 != null)
				{
					Ped character5 = Game.Player.Character;
					if (character5.Position.DistanceTo(Lift1.GetOffsetPosition(new Vector3(0f, -2f, -2.2f))) < 300f)
					{
						World.DrawMarker(MarkerType.Cylinder, Lift1.GetOffsetPosition(new Vector3(0f, -2f, -2.2f)), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
					}
					if (character5.Position.DistanceTo(Lift1.GetOffsetPosition(new Vector3(0f, -2f, -2.2f))) < 5f)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Start/Stop Elevator");
						if (Game.IsControlJustPressed(Control.Context))
						{
							LiftOn1 = !LiftOn1;
							if (Lift1.Position.Z < 35f)
							{
								LiftDic1 = 1;
							}
							if (Lift1.Position.Z > 114f)
							{
								LiftDic1 = -1;
							}
							Screen.ShowSubtitle("Lift On " + LiftOn1 + "__" + LiftDic1);
						}
					}
				}
				if (Lift2 != null)
				{
					Ped character6 = Game.Player.Character;
					if (character6.Position.DistanceTo(Lift2.GetOffsetPosition(new Vector3(0f, -2f, -2.2f))) < 300f)
					{
						World.DrawMarker(MarkerType.Cylinder, Lift2.GetOffsetPosition(new Vector3(0f, -2f, -2.2f)), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
					}
					if (character6.Position.DistanceTo(Lift2.GetOffsetPosition(new Vector3(0f, -2f, -2.2f))) < 5f)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Start/Stop Elevator");
						if (Game.IsControlJustPressed(Control.Context))
						{
							LiftOn2 = !LiftOn2;
							if (Lift2.Position.Z < 35f)
							{
								LiftDic2 = 1;
							}
							if (Lift2.Position.Z > 114f)
							{
								LiftDic2 = -1;
							}
							Screen.ShowSubtitle("Lift On " + LiftOn2 + "__" + LiftDic2);
						}
					}
				}
				if (LiftOn2 && Lift2 != null)
				{
					Prop lift = Lift2;
					if (LiftDic2 == 1 && lift.Position.Z > 30f && lift.Position.Z < 115.4f)
					{
						lift.Position = new Vector3(lift.Position.X, lift.Position.Y, lift.Position.Z + 0.1f);
					}
					if (LiftDic2 == -1 && lift.Position.Z > 31.3f && lift.Position.Z < 116f)
					{
						lift.Position = new Vector3(lift.Position.X, lift.Position.Y, lift.Position.Z - 0.1f);
					}
				}
				if (LiftOn1 && Lift1 != null)
				{
					Prop lift2 = Lift1;
					if (LiftDic1 == 1 && lift2.Position.Z > 30f && lift2.Position.Z < 115.4f)
					{
						lift2.Position = new Vector3(lift2.Position.X, lift2.Position.Y, lift2.Position.Z + 0.1f);
					}
					if (LiftDic1 == -1 && lift2.Position.Z > 31.3f && lift2.Position.Z < 116f)
					{
						lift2.Position = new Vector3(lift2.Position.X, lift2.Position.Y, lift2.Position.Z - 0.1f);
					}
				}
				if (!Game.Player.IsAlive)
				{
					EndHeist();
					FailHeist("cause the Player Died");
				}
			}
			if (Game.IsControlPressed(Control.Sprint) && Game.IsControlJustPressed(Control.Context))
			{
				TrainIsStoppingAtYard = false;
				HasSwappedTrainRouteToTrainYard = false;
				HasSwappedTrainRouteToEnterQuarry = false;
				CreateTrainAtCoords(Game.Player.Character.Position);
			}
		}

		public static Vector3 GenerateSpawnPos(Vector3 desiredPos, Nodetype roadtype, bool sidewalk)
		{
			Vector3 zero = Vector3.Zero;
			bool flag = false;
			OutputArgument outputArgument = new OutputArgument();
			int num = 1;
			int num2 = 0;
			if (roadtype == Nodetype.AnyRoad)
			{
				num2 = 1;
			}
			if (roadtype == Nodetype.Road)
			{
				num2 = 0;
			}
			if (roadtype == Nodetype.Offroad)
			{
				num2 = 1;
				flag = true;
			}
			if (roadtype == Nodetype.Water)
			{
				num2 = 3;
			}
			int num3 = Function.Call<int>(Hash.GET_NTH_CLOSEST_VEHICLE_NODE_ID, desiredPos.X, desiredPos.Y, desiredPos.Z, num, num2, 300f, 300f);
			if (flag)
			{
				while (!Function.Call<bool>(Hash.GET_VEHICLE_NODE_IS_SWITCHED_OFF, num3) && num < 500)
				{
					num++;
					num3 = Function.Call<int>(Hash.GET_NTH_CLOSEST_VEHICLE_NODE_ID, desiredPos.X, desiredPos.Y, desiredPos.Z, num, num2, 300f, 300f);
				}
			}
			Function.Call(Hash.GET_VEHICLE_NODE_POSITION, num3, outputArgument);
			zero = outputArgument.GetResult<Vector3>();
			if (sidewalk)
			{
				zero = World.GetNextPositionOnSidewalk(zero);
			}
			return zero;
		}

		public static string LoadStreamedTextureDict(string dict)
		{
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, dict))
			{
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, dict);
				Script.Yield();
			}
			return dict;
		}

		private float progressxcoord(float percent)
		{
			float num = 0.04f * percent;
			return 0.9f + num;
		}

		private float progressxcoord(float X, float percent)
		{
			float num = 0.04f * percent;
			return X + num;
		}

		private float progresswidth(float percent)
		{
			return 0.08f * percent;
		}

		private static void drawSprite2(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha)
		{
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, screenX, screenY, width, height, 0, r, g, b, alpha, 0);
			}
		}

		private static void drawText(string text, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		public void DrawProgressBar_Advanced(string Name, int Slot, float CrntValue, float MaxValue, Color MarkerColor, bool ShowBarLines)
		{
			float num = 0f;
			float num2 = 0f;
			if (Slot == 1)
			{
				num = 0.89f;
				num2 = 0.97f;
			}
			if (Slot == 2)
			{
				num = 0.89f;
				num2 = 0.94f;
			}
			if (Slot == 3)
			{
				num = 0.89f;
				num2 = 0.91f;
			}
			if (Slot == 4)
			{
				num = 0.89f;
				num2 = 0.88f;
			}
			if (Slot == 5)
			{
				num = 0.89f;
				num2 = 0.85f;
			}
			if (Slot == 6)
			{
				num = 0.89f;
				num2 = 0.82f;
			}
			if (Slot == 7)
			{
				num = 0.89f;
				num2 = 0.79f;
			}
			if (Slot == 8)
			{
				num = 0.89f;
				num2 = 0.76f;
			}
			drawSprite2("timerbars", "all_black_bg", num, num2, 0.21f, 0.04f, 255, 255, 255, 255);
			drawSprite2("timerbars", "all_black_bg", num - 0.1f, num2, 0.21f, 0.04f, 255, 255, 255, 255);
			drawSprite2("timerbars", "damagebarfill_128", num + 0.05f, num2, 0.08f, 0.03f, MarkerColor.R, MarkerColor.G, MarkerColor.B, 130);
			drawSprite2("timerbars", "damagebarfill_128", progressxcoord(CrntValue / MaxValue), num2, progresswidth(CrntValue / MaxValue), 0.03f, MarkerColor.R, MarkerColor.G, MarkerColor.B, 255);
			if (ShowBarLines)
			{
				drawSprite2("timerbar_lines", "LineMarker20_128", num + 0.05f, num2, 0.08f, 0.03f, 0, 0, 0, 255);
				drawSprite2("timerbar_lines", "LineMarker40_128", num + 0.05f, num2, 0.08f, 0.03f, 0, 0, 0, 255);
				drawSprite2("timerbar_lines", "LineMarker60_128", num + 0.05f, num2, 0.08f, 0.03f, 0, 0, 0, 255);
				drawSprite2("timerbar_lines", "LineMarker80_128", num + 0.05f, num2, 0.08f, 0.03f, 0, 0, 0, 255);
			}
			drawText(Name, num - 0.1f, num2 - 0.016f, 0.3f, 255, 255, 255);
		}

		private Vector2 World3DToScreen2d(Vector3 pos)
		{
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, pos.X, pos.Y, pos.Z, outputArgument, outputArgument2);
			return new Vector2(outputArgument.GetResult<float>(), outputArgument2.GetResult<float>());
		}
	}
}
