using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;

namespace SinglePlayerHeists.UtilsMisc
{
	internal class ManageCashProps : Script
	{
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

		public List<ObjectPlacementMap> GoldBag = new List<ObjectPlacementMap>();

		public List<ObjectPlacementMap> CashBag = new List<ObjectPlacementMap>();

		public List<ObjectPlacementMap> MixedBag = new List<ObjectPlacementMap>();

		public CargoShipHeist.MissionOjects CashProps = new CargoShipHeist.MissionOjects(new List<CargoShipHeist.ObjectPlacementMap>
		{
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_moneypack_03a"), 140885, 1554100735u, new Vector3(2681.33765f, -388.02536f, -56.3794365f), new Vector3(0f, 0f, -87.15613f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_moneypack_03a"), 517209, 1554100735u, new Vector3(2681.30249f, -387.40686f, -56.3794365f), new Vector3(0f, -0f, -90.58476f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_moneypack_03a"), 116310, 1554100735u, new Vector3(2681.35352f, -388.615723f, -56.3794365f), new Vector3(0f, 0f, -86.84834f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_moneypack_03a"), 124248, 1554100735u, new Vector3(2681.935f, -387.9894f, -56.3794365f), new Vector3(0f, 0f, -83.1115646f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_prtmachine_moneyream"), 103768, 3279188378u, new Vector3(2681.301f, -388.015961f, -56.23104f), new Vector3(0f, 0f, -86.5914841f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_moneypack_02a"), 33621, 238227343u, new Vector3(2681.27539f, -388.028656f, -56.0480728f), new Vector3(0f, 0f, -86.13014f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_moneypack_02a"), 628308, 3022709653u, new Vector3(2681.90479f, -387.221161f, -56.3804321f), new Vector3(0f, -0f, -173.649292f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_moneypack_02a"), 353627, 3022709653u, new Vector3(2682.16284f, -387.962677f, -56.34408f), new Vector3(0f, -0f, 98.06236f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_moneypack_02a"), 512340, 3022709653u, new Vector3(2681.90649f, -388.72226f, -56.3004456f), new Vector3(0f, 0f, 87.8770752f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_exec_cashpile"), 315221, 955232737u, new Vector3(2682.01074f, -388.1567f, -56.1144829f), new Vector3(0f, -0f, 91.81661f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_exec_cashpile"), 104796, 955232737u, new Vector3(2681.92163f, -387.627655f, -56.1522942f), new Vector3(0f, -0f, 101.335747f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_exec_cashpile"), 460890, 955232737u, new Vector3(2681.87061f, -388.55307f, -56.1086578f), new Vector3(0f, 0f, 6.90642643f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cash_crate_01"), 175453, 3830275308u, new Vector3(2681.25952f, -388.8278f, -56.2306252f), new Vector3(0f, 0f, 4.500905f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cash_crate_01"), 746571, 3830275308u, new Vector3(2681.28857f, -387.225739f, -56.2306252f), new Vector3(0f, -0f, 95.3462143f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cash_crate_01"), 354908, 3830275308u, new Vector3(2681.1936f, -388.453247f, -56.0478172f), new Vector3(0f, -0f, 96.57372f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cash_crate_01"), 9051, 3830275308u, new Vector3(2681.15332f, -387.780365f, -55.9724274f), new Vector3(0f, -0f, 96.16401f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cash_crate_01"), 42079, 3830275308u, new Vector3(2681.887f, -388.953f, -56.2248f), new Vector3(0f, -0f, 94.6278839f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cash_crate_01"), 622941, 3830275308u, new Vector3(2681.818f, -387.264435f, -56.3047867f), new Vector3(0f, -0f, 92.88724f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cash_crate_01"), 224856, 3830275308u, new Vector3(2681.822f, -388.4863f, -55.9909363f), new Vector3(0f, -0f, 95.95743f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cash_crate_01"), 532054, 3830275308u, new Vector3(2682.35425f, -388.457336f, -56.3801765f), new Vector3(0f, -0f, 97.7992f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cash_crate_01"), 777561, 3830275308u, new Vector3(2682.36523f, -387.788483f, -56.2684364f), new Vector3(0f, -0f, 96.62145f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_moneypack_03a"), 478550, 1554100735u, new Vector3(2681.89673f, -387.853851f, -56.0338326f), new Vector3(0f, -0f, 102.711235f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_moneypack_03a"), 571477, 1554100735u, new Vector3(2682.34814f, -387.209839f, -56.3794365f), new Vector3(0f, -0f, 95.28885f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_moneypack_03a"), 24663, 1554100735u, new Vector3(2682.381f, -387.210968f, -56.2298851f), new Vector3(0f, -0f, 136.437943f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_moneypack_03a"), 2144, 1554100735u, new Vector3(2682.36768f, -387.2437f, -56.0803337f), new Vector3(0f, -0f, 103.886444f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_moneypack_03a"), 397408, 1554100735u, new Vector3(2682.47729f, -389.01828f, -56.3794365f), new Vector3(0f, 0f, 75.5829544f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_moneypack_03a"), 487768, 1554100735u, new Vector3(2682.49463f, -389.013763f, -56.2298851f), new Vector3(0f, 0f, 84.94883f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_moneypack_03a"), 481879, 1554100735u, new Vector3(2682.411f, -388.8584f, -56.0803337f), new Vector3(0f, 0f, 78.8069458f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_moneypack_03a"), 312922, 1554100735u, new Vector3(2682.36182f, -388.3458f, -56.08121f), new Vector3(0f, -0f, 91.652565f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_exec_cashpile"), 385882, 955232737u, new Vector3(2681.24976f, -388.806824f, -55.8162575f), new Vector3(0f, 0f, 1.82956791f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_exec_cashpile"), 679772, 955232737u, new Vector3(2681.89722f, -388.885254f, -55.8104324f), new Vector3(0f, 0f, 7.45946932f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_exec_cashpile"), 200801, 955232737u, new Vector3(2681.92261f, -389.0696f, -55.8104324f), new Vector3(0f, 0f, 7.40828f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_exec_cashpile"), 400465, 955232737u, new Vector3(2681.28345f, -389.003418f, -55.8162575f), new Vector3(0f, 0f, 5.92400646f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_exec_cashpile"), 598874, 955232737u, new Vector3(2681.19165f, -388.180878f, -55.63345f), new Vector3(0f, 0f, 7.101156f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_exec_cashpile"), 222044, 955232737u, new Vector3(2681.77222f, -387.774384f, -55.76888f), new Vector3(0f, 0f, 9.301928f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_exec_cashpile"), 112738, 955232737u, new Vector3(2681.797f, -387.956879f, -55.76888f), new Vector3(0f, 0f, 9.711355f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_exec_cashpile"), 88416, 955232737u, new Vector3(2681.15332f, -388.012726f, -55.55806f), new Vector3(0f, -0f, -171.206039f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_cash_pile_8"), 100196, 3785873857u, new Vector3(2682.877f, -387.733215f, -56.3759079f), new Vector3(0f, -0f, 96.36052f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_cash_pile_005"), 360546, 2877991226u, new Vector3(2682.98755f, -388.5466f, -56.3741951f), new Vector3(0f, -0f, 92.98198f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_bkr_cashpile_05"), 767841, 3275899984u, new Vector3(2682.83057f, -389.102234f, -56.378994f), new Vector3(0f, 0f, 65.03695f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_prtmachine_moneyream"), 105567, 3279188378u, new Vector3(2681.589f, -389.592834f, -56.3805923f), new Vector3(0f, -0f, -91.93561f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_moneypack_03a"), 461922, 1554100735u, new Vector3(2681.29053f, -389.384583f, -56.19663f), new Vector3(0f, 0f, -83.95091f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_moneypack_03a"), 222305, 1554100735u, new Vector3(2681.78979f, -389.519165f, -56.19663f), new Vector3(0f, 0f, -3.95467281f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_cutter_moneystack_01a"), 662625, 3919595750u, new Vector3(2682.45654f, -389.7029f, -56.3807831f), new Vector3(0f, 0f, 3.21035981f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_moneypack_03a"), 288094, 1554100735u, new Vector3(2682.32642f, -389.509155f, -56.3004951f), new Vector3(0f, 0f, 7.86781645f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_bkr_cashpile_01"), 335178, 2596205386u, new Vector3(2681.79175f, -389.470032f, -56.0456238f), new Vector3(0f, 0f, -0.6794418f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_bkr_cashpile_01"), 424027, 2596205386u, new Vector3(2681.30176f, -389.4503f, -56.0456238f), new Vector3(0f, 0f, 0.08826834f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_bkr_cashpile_01"), 144487, 2596205386u, new Vector3(2682.36133f, -388.418884f, -55.9302063f), new Vector3(0f, -0f, 96.7688751f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_bkr_cashpile_01"), 472156, 2596205386u, new Vector3(2682.358f, -389.03067f, -55.92933f), new Vector3(0f, 0f, 79.87882f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_bkr_cashpile_01"), 352102, 2596205386u, new Vector3(2682.31323f, -387.8367f, -55.9680176f), new Vector3(0f, -0f, 98.86665f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_bkr_cashpile_01"), 430183, 2596205386u, new Vector3(2681.78247f, -387.481384f, -56.0043678f), new Vector3(0f, -0f, -171.156586f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_bkr_cashpile_07"), 723815, 816815913u, new Vector3(2683.358f, -387.738464f, -56.3759079f), new Vector3(0f, -0f, 93.5437f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_bkr_cashpile_07"), 714085, 816815913u, new Vector3(2682.167f, -386.633484f, -56.3759079f), new Vector3(0f, -0f, -175.660919f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_bkr_cashpile_07"), 220511, 816815913u, new Vector3(2683.066f, -386.73172f, -56.3759079f), new Vector3(0f, -0f, 178.401779f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_bkr_cashpile_07"), 769383, 816815913u, new Vector3(2680.73364f, -389.408936f, -56.3759079f), new Vector3(0f, 0f, -80.3100739f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_bkr_cashpile_04"), 505697, 4097844811u, new Vector3(2680.66846f, -388.6389f, -56.3741951f), new Vector3(0f, 0f, -88.6524658f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_bkr_cashpile_04"), 18791, 4097844811u, new Vector3(2681.93726f, -386.178131f, -56.3741951f), new Vector3(0f, -0f, -175.506f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_bkr_cashpile_04"), 63589, 4097844811u, new Vector3(2683.89282f, -387.599182f, -56.3741951f), new Vector3(0f, -0f, 91.39515f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_bkr_cashpile_01"), 617833, 2596205386u, new Vector3(2681.87183f, -389.415039f, -55.8902664f), new Vector3(0f, 0f, 81.67013f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_bkr_cashpile_07"), 681831, 816815913u, new Vector3(2681.15942f, -388.697571f, -55.7453232f), new Vector3(0f, 0f, -83.18361f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_bkr_cashpile_07"), 670048, 816815913u, new Vector3(2681.919f, -388.656158f, -55.6884422f), new Vector3(0f, -0f, 99.22505f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cash_crate_01"), 351841, 3830275308u, new Vector3(2682.325f, -388.391541f, -55.7770424f), new Vector3(0f, -0f, 95.64159f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cash_crate_01"), 392296, 3830275308u, new Vector3(2681.79053f, -387.832184f, -55.6511574f), new Vector3(0f, 0f, -78.98828f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cash_crate_01"), 581733, 3830275308u, new Vector3(2681.651f, -387.260162f, -55.8512421f), new Vector3(0f, -0f, -174.2857f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cash_crate_01"), 102760, 3830275308u, new Vector3(2682.26318f, -387.740967f, -55.8148537f), new Vector3(0f, -0f, 115.135048f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cash_crate_01"), 518506, 3830275308u, new Vector3(2682.03955f, -388.0774f, -55.5321045f), new Vector3(0f, -0f, 98.9100647f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cash_crate_01"), 623723, 3830275308u, new Vector3(2682.044f, -388.7043f, -55.438282f), new Vector3(0f, -0f, 95.83876f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cash_crate_01"), 182893, 3830275308u, new Vector3(2681.547f, -388.241638f, -55.44083f), new Vector3(0f, -0f, -165.687729f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cash_case_01"), 518755, 1603932804u, new Vector3(2682.19751f, -387.205231f, -55.88968f), new Vector3(0f, -0f, -161.643982f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_cash_crate_half_full"), 274274, 3546768279u, new Vector3(2683.42944f, -388.5397f, -56.3801765f), new Vector3(0f, 0f, 78.54108f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 644972, 1228147776u, new Vector3(2681.689f, -387.2869f, -55.55053f), new Vector3(0f, -0f, -173.465942f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 654702, 1228147776u, new Vector3(2681.07056f, -387.360535f, -55.9299126f), new Vector3(0f, 0f, -88.14435f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_exec_cashpile"), 775528, 955232737u, new Vector3(2681.31836f, -387.682f, -55.55806f), new Vector3(0f, 0f, -88.80765f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_exec_cashpile"), 176746, 955232737u, new Vector3(2682.36865f, -388.6294f, -55.3626747f), new Vector3(0f, -0f, 90.77696f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_exec_cashpile"), 3693, 955232737u, new Vector3(2681.6333f, -388.814758f, -55.1445351f), new Vector3(0f, 0f, -85.6948547f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_exec_cashpile"), 61040, 955232737u, new Vector3(2681.44727f, -388.805237f, -55.12867f), new Vector3(0f, 0f, -83.49391f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_exec_cashpile"), 643691, 955232737u, new Vector3(2681.32422f, -389.317139f, -55.72962f), new Vector3(0f, 0f, 5.049458f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_exec_cashpile"), 593262, 955232737u, new Vector3(2682.388f, -389.432068f, -56.03554f), new Vector3(0f, 0f, 1.10850286f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_exec_cashpile"), 785775, 955232737u, new Vector3(2681.33032f, -389.683075f, -56.2640343f), new Vector3(0f, 0f, -1.194932f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_exec_cashpile"), 310128, 955232737u, new Vector3(2680.91846f, -387.372162f, -56.2640343f), new Vector3(0f, 0f, -88.9184341f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_exec_cashpile"), 265837, 955232737u, new Vector3(2681.34448f, -386.8243f, -56.2640343f), new Vector3(0f, -0f, -173.877609f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cash_crate_01"), 769649, 3830275308u, new Vector3(2681.296f, -386.512024f, -56.3801765f), new Vector3(0f, -0f, -174.6448f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cash_crate_01"), 29298, 3830275308u, new Vector3(2681.36938f, -386.7226f, -56.08195f), new Vector3(0f, -0f, -172.3412f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 549490, 1228147776u, new Vector3(2682.00757f, -388.619324f, -55.13757f), new Vector3(0f, -0f, 97.01456f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_moneypack_03a"), 413553, 1554100735u, new Vector3(2682.02686f, -388.140869f, -55.23314f), new Vector3(0f, -0f, 102.388557f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_moneypack_03a"), 292202, 1554100735u, new Vector3(2681.908f, -387.647949f, -55.3521919f), new Vector3(0f, -0f, 118.970894f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_moneypack_03a"), 554356, 1554100735u, new Vector3(2682.238f, -387.5387f, -55.51589f), new Vector3(0f, -0f, 96.24602f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_moneypack_03a"), 387957, 1554100735u, new Vector3(2681.25513f, -387.650543f, -55.4395981f), new Vector3(0f, -0f, -172.651077f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_moneypack_03a"), 442483, 1554100735u, new Vector3(2681.41187f, -387.730316f, -55.2900467f), new Vector3(0f, -0f, -164.00116f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_moneypack_03a"), 103030, 1554100735u, new Vector3(2681.77539f, -387.920929f, -55.1418648f), new Vector3(0f, 0f, -59.69338f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_moneypack_03a"), 3183, 1554100735u, new Vector3(2681.92725f, -388.503235f, -55.1393166f), new Vector3(0f, -0f, -90.9134445f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_moneypack_03a"), 786038, 1554100735u, new Vector3(2681.45215f, -388.280823f, -55.1418648f), new Vector3(0f, 0f, -72.3345f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_moneypack_03a"), 685684, 1554100735u, new Vector3(2681.62549f, -388.6405f, -55.0260735f), new Vector3(0f, 0f, -77.4525f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_bkr_cashpile_01"), 467576, 2596205386u, new Vector3(2681.78882f, -388.259033f, -55.0340271f), new Vector3(0f, -0f, -167.018463f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_bkr_cashpile_01"), 288884, 2596205386u, new Vector3(2682.11865f, -388.7228f, -54.98831f), new Vector3(0f, -0f, 95.53204f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_bkr_cashpile_01"), 548472, 2596205386u, new Vector3(2681.40234f, -386.792267f, -55.7815323f), new Vector3(0f, -0f, -163.281876f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_cash_pile_005"), 427634, 2877991226u, new Vector3(2681.30127f, -385.952179f, -56.3741951f), new Vector3(0f, -0f, -172.3905f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_cash_pile_005"), 65132, 2877991226u, new Vector3(2683.84644f, -388.1812f, -56.3741951f), new Vector3(0f, 0f, 87.1401f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_cash_pile_01"), 473967, 1898540942u, new Vector3(2682.46362f, -389.7705f, -56.29904f), new Vector3(0f, 0f, 7.706777f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_cash_pile_01"), 399477, 1898540942u, new Vector3(2681.81616f, -389.775177f, -56.1951752f), new Vector3(0f, 0f, 6.734322f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public bool CreatedCashProps;

		public int LocalPlayerCash;

		public static void ATTACH_ENTITY_TO_ENTITY(Entity entity1, Entity entity2, int boneIndex, float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, bool p9, bool useSoftPinning, bool collision, bool isPed, int vertexIndex, bool fixedRot)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, entity1, entity2, boneIndex, xPos, yPos, zPos, xRot, yRot, zRot, p9, useSoftPinning, collision, isPed, vertexIndex, fixedRot);
		}

		public ManageCashProps()
		{
			base.Tick += Ontick;
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

		public void Ontick(object sender, EventArgs e)
		{
			if (!Main.IsInArcade)
			{
				return;
			}
			if (SPH_Debug.ShowRunningClassDebug)
			{
				Class1.drawText("Manage Cash Props", 0.8f, 0.22f, 0.5f, 255, 255, 255);
			}
			if (!CreatedCashProps)
			{
				CreatedCashProps = true;
				if (!((float)LocalPlayerCash <= Main.TotalPlayerEarned))
				{
					return;
				}
				LocalPlayerCash = (int)Main.TotalPlayerEarned;
				for (int i = 0; i < CashProps.Entities.Count; i++)
				{
					int num = 75000;
					int num2 = 0;
					if (i >= 10)
					{
						num2 = 175000;
					}
					if (i >= 20)
					{
						num2 = 225000;
					}
					if (i >= 30)
					{
						num2 = 350000;
					}
					if (i >= 40)
					{
						num2 = 650000;
					}
					if (i >= 50)
					{
						num2 = 850000;
					}
					if (i >= 60)
					{
						num2 = 1000000;
					}
					if (i >= 70)
					{
						num2 = 1250000;
					}
					if (i >= 80)
					{
						num2 = 1500000;
					}
					if (i >= 90)
					{
						num2 = 1750000;
					}
					if (CashProps.Entities[i].Entity == null && LocalPlayerCash >= (num + num2) * i)
					{
						CashProps.Entities[i].CreateObjatCoords();
					}
				}
			}
			else
			{
				if (!Main.IsInArcade || !((float)LocalPlayerCash <= Main.TotalPlayerEarned))
				{
					return;
				}
				LocalPlayerCash = (int)Main.TotalPlayerEarned;
				for (int j = 0; j < CashProps.Entities.Count; j++)
				{
					int num3 = 75000;
					int num4 = 0;
					if (j >= 10)
					{
						num4 = 175000;
					}
					if (j >= 20)
					{
						num4 = 225000;
					}
					if (j >= 30)
					{
						num4 = 350000;
					}
					if (j >= 40)
					{
						num4 = 650000;
					}
					if (j >= 50)
					{
						num4 = 850000;
					}
					if (j >= 60)
					{
						num4 = 1000000;
					}
					if (j >= 70)
					{
						num4 = 1250000;
					}
					if (j >= 80)
					{
						num4 = 1500000;
					}
					if (j >= 90)
					{
						num4 = 1750000;
					}
					if (CashProps.Entities[j].Entity == null && LocalPlayerCash >= (num3 + num4) * j)
					{
						CashProps.Entities[j].CreateObjatCoords();
					}
				}
			}
		}
	}
}
