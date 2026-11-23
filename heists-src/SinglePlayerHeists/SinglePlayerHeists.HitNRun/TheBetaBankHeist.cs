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

namespace SinglePlayerHeists.HitNRun
{
	internal class TheBetaBankHeist : Script
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

		public enum Heiststage
		{
			None,
			HeadingToBank,
			TakeOutCameras,
			OpenSecurityGate,
			LootVault,
			EscapeCops
		}

		public static int HeistMinPay = 450000;

		public static int HeistMaxPay = 2500000;

		public static float PercentNeedTake = (float)(HeistMinPay + HeistMaxPay) / 1.5f / 100f * 80f;

		public static int CurrentPay = 0;

		public static int LocalTake = 0;

		public static bool TheBetaBankHeistOn;

		public static List<ObjectPlacementMap> Interior = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mk_money"), 310859, 1113676861u, new Vector3(-1230.39893f, -336.875824f, 36.8950577f), new Vector3(0f, -0f, -141.54512f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mk_money"), 326777, 1113676861u, new Vector3(-1229.82971f, -338.295532f, 36.8949623f), new Vector3(0f, 0f, -57.19778f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_mp_h_din_table_05"), 327547, 746287557u, new Vector3(-1227.12744f, -338.107269f, 35.8157539f), new Vector3(0f, 1.19209233E-07f, -63.3009f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_mp_h_din_table_05"), 659525, 746287557u, new Vector3(-1228.46753f, -338.781219f, 35.8157539f), new Vector3(0f, 1.19209233E-07f, -63.3009f), 327547, 0, new Vector3(0f, -1.50000024f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_mp_h_din_table_05"), 427903, 746287557u, new Vector3(-1229.80762f, -339.45517f, 35.8157539f), new Vector3(0f, 1.19209233E-07f, -63.3009f), 327547, 0, new Vector3(0f, -2.99999952f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_mp_h_din_table_05"), 114283, 746287557u, new Vector3(-1231.14758f, -340.12915f, 35.8157539f), new Vector3(0f, 1.19209233E-07f, -63.3009f), 327547, 0, new Vector3(0f, -4.499998f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_mp_h_din_table_05"), 42071, 746287557u, new Vector3(-1226.83655f, -342.02417f, 35.8157539f), new Vector3(0f, 1.19209233E-07f, -63.3009f), 680001, 0, new Vector3(0f, -1.50000024f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_mp_h_din_table_05"), 160015, 746287557u, new Vector3(-1228.17664f, -342.69812f, 35.8157539f), new Vector3(0f, 1.19209233E-07f, -63.3009f), 680001, 0, new Vector3(0f, -2.99999952f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_mp_h_din_table_05"), 390402, 746287557u, new Vector3(-1229.5166f, -343.3721f, 35.8157539f), new Vector3(0f, 1.19209233E-07f, -63.3009f), 680001, 0, new Vector3(0f, -4.499998f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_mp_h_din_table_05"), 680001, 746287557u, new Vector3(-1225.49646f, -341.35022f, 35.8157539f), new Vector3(0f, 1.19209233E-07f, -63.3009f), 327547, 0, new Vector3(3.62999868f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_mp_h_din_table_05"), 300041, 746287557u, new Vector3(-1225.18762f, -345.3029f, 35.8157539f), new Vector3(0f, 1.19209233E-07f, -63.3009f), 268312, 0, new Vector3(0f, -1.50000024f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_mp_h_din_table_05"), 326199, 746287557u, new Vector3(-1226.52771f, -345.976837f, 35.8157539f), new Vector3(0f, 1.19209233E-07f, -63.3009f), 268312, 0, new Vector3(0f, -2.99999952f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_mp_h_din_table_05"), 452678, 746287557u, new Vector3(-1227.86768f, -346.650818f, 35.8157539f), new Vector3(0f, 1.19209233E-07f, -63.3009f), 268312, 0, new Vector3(0f, -4.499998f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_mp_h_din_table_05"), 334169, 746287557u, new Vector3(-1222.21655f, -347.8719f, 35.8157539f), new Vector3(0f, 1.19209233E-07f, -63.3009f), 268312, 0, new Vector3(3.62999868f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_mp_h_din_table_05"), 268312, 746287557u, new Vector3(-1223.84753f, -344.628937f, 35.8157539f), new Vector3(0f, 1.19209233E-07f, -63.3009f), 327547, 0, new Vector3(7.29999542f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 610659, 924295337u, new Vector3(-1226.06787f, -339.426758f, 36.28939f), new Vector3(0f, 0f, -61.7650642f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 79672, 924295337u, new Vector3(-1227.53918f, -340.216827f, 36.28939f), new Vector3(-0f, -0f, -61.7650642f), 610659, 0, new Vector3(0f, -1.66999876f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 307058, 924295337u, new Vector3(-1227.82813f, -341.2006f, 36.28939f), new Vector3(-0f, -0f, -151.76506f), 610659, 0, new Vector3(0.7300002f, -2.38999844f, 0f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 549483, 2068850345u, new Vector3(-1222.22144f, -341.360565f, 36.0172f), new Vector3(90f, -64.15205f, 0f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 270374, 2068850345u, new Vector3(-1221.34949f, -343.160461f, 36.0172f), new Vector3(90f, -64.15205f, 0f), 549483, 0, new Vector3(2.00000024f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 474113, 2068850345u, new Vector3(-1220.47754f, -344.9604f, 36.0172f), new Vector3(90f, -64.15205f, 0f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 605727, 2068850345u, new Vector3(-1219.51831f, -346.940277f, 36.0172f), new Vector3(90f, -64.15205f, 0f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_mp_h_din_table_05"), 493936, 746287557u, new Vector3(-1226.41833f, -340.296f, 36.86939f), new Vector3(0f, -0f, -151.76506f), 79672, 0, new Vector3(0.6000001f, 0.950000048f, 0.5799998f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 431413, 924295337u, new Vector3(-1227.79468f, -345.836365f, 36.2586021f), new Vector3(-8.969959E-05f, -179.929886f, 117.496353f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 145259, 924295337u, new Vector3(-1226.29553f, -345.0561f, 36.258606f), new Vector3(-8.969959E-05f, -179.929886f, 117.496353f), 431413, 0, new Vector3(3.7252903E-09f, -1.68999875f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 657961, 924295337u, new Vector3(-1224.77869f, -344.2666f, 36.258606f), new Vector3(-8.969959E-05f, -179.929886f, 117.496353f), 431413, 0, new Vector3(1.86264515E-08f, -3.39999747f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_res_mvasechinese"), 100883, 1855795498u, new Vector3(-1226.46448f, -339.58316f, 37.67357f), new Vector3(0f, -0f, -154.956528f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_mp_h_str_sideboardl_09"), 131091, 1706990326u, new Vector3(-1225.36279f, -344.4222f, 37.6384468f), new Vector3(89.93438f, 27.4963474f, 0f), 131347, 0, new Vector3(2.79999781f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_mp_h_str_sideboardl_09"), 131347, 1706990326u, new Vector3(-1227.84656f, -345.714935f, 37.638443f), new Vector3(89.93438f, 27.4963474f, 0f), 431413, 0, new Vector3(-0.13000004f, -0.0100000016f, -1.38000047f), new Vector3(-90.00001f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 131603, 2068850345u, new Vector3(-1219.1665f, -342.165222f, 36.01616f), new Vector3(90f, -63.24751f, 0f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 131859, 2068850345u, new Vector3(-1218.26624f, -343.951141f, 36.01616f), new Vector3(90f, -63.24751f, 0f), 132371, 0, new Vector3(3.99999857f, 0f, -2.5f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 132115, 2068850345u, new Vector3(-1217.366f, -345.737061f, 36.01616f), new Vector3(90f, -63.24751f, 0f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 132371, 2068850345u, new Vector3(-1222.29919f, -341.504639f, 36.01616f), new Vector3(90f, -63.24751f, 0f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 157971, 2068850345u, new Vector3(-1219.91919f, -340.3786f, 36.0172f), new Vector3(90f, -64.15205f, 0f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstcover_01"), 549714, 1215053148u, new Vector3(-1226.2467f, -345.1499f, 39.7287369f), new Vector3(0.0701000541f, -179.9899f, -152.503632f), 145259, 0, new Vector3(0.110000029f, 0f, -3.46999717f), new Vector3(0f, 0.0100000016f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstcover_01"), 358983, 1215053148u, new Vector3(-1226.24585f, -345.15097f, 38.67874f), new Vector3(0.0701000541f, -179.9899f, -152.503632f), 664858, 0, new Vector3(0f, 0f, -1.06999934f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstcover_01"), 664858, 1215053148u, new Vector3(-1226.24512f, -345.152039f, 37.60874f), new Vector3(0.0701000541f, -179.9899f, -152.503632f), 549714, 0, new Vector3(0f, 0f, 2.11999846f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 628850, 924295337u, new Vector3(-1223.65808f, -344.470276f, 37.8594627f), new Vector3(0.07010006f, -179.9999f, -152.503632f), 657961, 0, new Vector3(0.6999999f, -0.9000001f, -1.60000026f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 182318, 924295337u, new Vector3(-1223.65955f, -344.467346f, 40.55946f), new Vector3(0.07010006f, -179.9999f, -152.503632f), 657961, 0, new Vector3(0.6999999f, -0.9000001f, -4.29999828f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 58955, 924295337u, new Vector3(-1224.44885f, -342.950928f, 40.1773758f), new Vector3(0.07010006f, -179.9999f, -152.503632f), 657961, 0, new Vector3(-1.00999916f, -0.9000001f, -3.92000532f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 319324, 924295337u, new Vector3(-1224.65759f, -342.55f, 40.1768074f), new Vector3(0.07010006f, -179.9999f, -152.503632f), 657961, 0, new Vector3(-1.46199882f, -0.9000001f, -3.91999173f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 708959, 2068850345u, new Vector3(-1221.76819f, -348.030243f, 36.0172f), new Vector3(90f, -64.15205f, 0f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 100661, 2068850345u, new Vector3(-1224.01807f, -349.1202f, 36.0172f), new Vector3(90f, -64.15205f, 0f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 260710, 2068850345u, new Vector3(-1222.64014f, -346.230316f, 36.0172f), new Vector3(90f, -64.15205f, 0f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 517462, 2068850345u, new Vector3(-1224.89f, -347.320282f, 36.0172f), new Vector3(90f, -64.15205f, 0f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 638330, 924295337u, new Vector3(-1225.7782f, -351.3428f, 36.8173332f), new Vector3(0f, 0f, 26.3821545f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 737032, 924295337u, new Vector3(-1226.48914f, -349.909424f, 36.8173332f), new Vector3(0f, 0f, 26.3821545f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 354617, 924295337u, new Vector3(-1227.20007f, -348.476074f, 36.8173332f), new Vector3(0f, 0f, 26.3821545f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 153690, 924295337u, new Vector3(-1223.18335f, -346.8297f, 36.8173332f), new Vector3(0f, -0f, -153.617783f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 563064, 924295337u, new Vector3(-1222.205f, -347.1593f, 36.8173332f), new Vector3(0f, 0f, -63.617794f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 259674, 924295337u, new Vector3(-1226.92883f, -347.537781f, 36.8173332f), new Vector3(0f, 0f, -63.617794f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 622597, 924295337u, new Vector3(-1225.40588f, -346.782379f, 36.8173332f), new Vector3(0f, 0f, -63.617794f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 745814, 924295337u, new Vector3(-1223.88293f, -346.026978f, 36.8173332f), new Vector3(0f, 0f, -63.617794f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 509813, 924295337u, new Vector3(-1222.22668f, -345.5403f, 36.8173332f), new Vector3(0f, 0f, -63.617794f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 187234, 924295337u, new Vector3(-1216.4989f, -347.298279f, 36.8173332f), new Vector3(0f, -0f, -153.617783f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 236405, 924295337u, new Vector3(-1217.25439f, -345.77533f, 36.8173332f), new Vector3(0f, -0f, -153.617783f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 334412, 924295337u, new Vector3(-1218.00977f, -344.25238f, 36.8173332f), new Vector3(0f, -0f, -153.617783f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 361084, 924295337u, new Vector3(-1220.682f, -346.4039f, 36.8173332f), new Vector3(0f, 0f, -63.617794f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 14879, 924295337u, new Vector3(-1220.35254f, -345.425537f, 36.8173332f), new Vector3(0f, -0f, -153.617783f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 400721, 924295337u, new Vector3(-1221.33081f, -345.095947f, 36.8173332f), new Vector3(0f, 0f, -63.617794f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 241678, 924295337u, new Vector3(-1219.96277f, -339.639832f, 36.8173332f), new Vector3(0f, 0f, -63.617794f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 297579, 924295337u, new Vector3(-1220.9856f, -339.1983f, 36.8173332f), new Vector3(0f, 0f, 26.38217f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 121371, 924295337u, new Vector3(-1224.85571f, -341.11792f, 36.8173332f), new Vector3(0f, 0f, 26.38217f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 332355, 924295337u, new Vector3(-1225.7782f, -351.3428f, 36.8173332f), new Vector3(0f, 0f, 26.3821545f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 81213, 924295337u, new Vector3(-1225.7782f, -351.3428f, 39.517292f), new Vector3(0f, 0f, 26.3821545f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 661531, 924295337u, new Vector3(-1224.85571f, -341.11792f, 39.517292f), new Vector3(0f, 0f, 26.38217f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 663131, 924295337u, new Vector3(-1220.9856f, -339.1983f, 39.517292f), new Vector3(0f, 0f, 26.38217f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 733981, 924295337u, new Vector3(-1219.96277f, -339.639832f, 39.517292f), new Vector3(0f, 0f, -63.617794f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 532338, 924295337u, new Vector3(-1218.68811f, -343.7851f, 40.21728f), new Vector3(0f, 0f, -63.617794f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 736883, 924295337u, new Vector3(-1221.33081f, -345.096039f, 39.517292f), new Vector3(0f, 0f, -63.617794f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 744249, 924295337u, new Vector3(-1220.35254f, -345.435638f, 39.52729f), new Vector3(0f, -0f, -153.617783f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 103239, 924295337u, new Vector3(-1220.682f, -346.4039f, 39.517292f), new Vector3(0f, 0f, -63.617794f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 284017, 924295337u, new Vector3(-1218.00977f, -344.25238f, 39.517292f), new Vector3(0f, -0f, -153.617783f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 282652, 924295337u, new Vector3(-1217.25439f, -345.77533f, 39.517292f), new Vector3(0f, -0f, -153.617783f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 484414, 924295337u, new Vector3(-1216.4989f, -347.298279f, 39.517292f), new Vector3(0f, -0f, -153.617783f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 3630, 924295337u, new Vector3(-1222.22668f, -345.5403f, 39.517292f), new Vector3(0f, 0f, -63.617794f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 679789, 924295337u, new Vector3(-1223.88293f, -346.026978f, 39.517292f), new Vector3(0f, 0f, -63.617794f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 302861, 924295337u, new Vector3(-1225.40588f, -346.782379f, 39.517292f), new Vector3(0f, 0f, -63.617794f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 167542, 924295337u, new Vector3(-1226.92883f, -347.537781f, 39.517292f), new Vector3(0f, 0f, -63.617794f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 702018, 924295337u, new Vector3(-1222.205f, -347.1593f, 39.517292f), new Vector3(0f, 0f, -63.617794f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 759394, 924295337u, new Vector3(-1223.18335f, -346.8297f, 39.517292f), new Vector3(0f, -0f, -153.617783f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 611875, 924295337u, new Vector3(-1227.20007f, -348.476074f, 39.517292f), new Vector3(0f, 0f, 26.3821545f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 422956, 924295337u, new Vector3(-1226.48914f, -349.909424f, 39.517292f), new Vector3(0f, 0f, 26.3821545f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 102460, 924295337u, new Vector3(-1225.7782f, -351.3428f, 39.517292f), new Vector3(0f, 0f, 26.3821545f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 2332, 924295337u, new Vector3(-1220.09778f, -344.485138f, 40.2272644f), new Vector3(0f, 0f, -63.617794f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_computer_02"), 633668, 1906160501u, new Vector3(-1226.74036f, -340.647369f, 37.67587f), new Vector3(0f, 0f, 57.165f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_computer_01"), 627718, 1613369486u, new Vector3(-1224.28662f, -344.4682f, 37.6529236f), new Vector3(0f, 0f, 19.1371841f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_computer_01"), 308560, 1613369486u, new Vector3(-1227.941f, -346.39917f, 37.65295f), new Vector3(0f, 0f, 47.5420227f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_ret_gc_cashreg"), 467808, 3997648379u, new Vector3(-1225.49634f, -345.1679f, 37.8517f), new Vector3(0f, 0f, 36.543438f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_ret_gc_cashreg"), 781633, 3997648379u, new Vector3(-1226.67786f, -345.770538f, 37.8516846f), new Vector3(0f, 0f, 14.0236921f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_mp_h_stn_chairstrip_05"), 763994, 3605067516u, new Vector3(-1230.12512f, -341.875946f, 36.61615f), new Vector3(0f, -0f, 116.589958f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_mp_h_stn_chairstrip_05"), 53520, 3605067516u, new Vector3(-1230.71606f, -340.695435f, 36.61615f), new Vector3(0f, -0f, 116.589958f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_mp_h_stn_chairstrip_05"), 714343, 3605067516u, new Vector3(-1231.30688f, -339.515f, 36.61615f), new Vector3(0f, -0f, 116.589958f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_chair_01"), 154996, 538002882u, new Vector3(-1225.89233f, -341.222626f, 37.2183456f), new Vector3(0f, -0f, -134.247787f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_chair_01"), 274475, 538002882u, new Vector3(-1224.5957f, -345.483124f, 37.2183456f), new Vector3(0f, -0f, -141.566f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_chair_01"), 80757, 538002882u, new Vector3(-1227.03625f, -346.7162f, 37.2183456f), new Vector3(0f, -0f, -158.5064f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 704548, 113622690u, new Vector3(-1225.89258f, -344.8151f, 40.0019035f), new Vector3(-90f, 155.241455f, 0f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 495163, 113622690u, new Vector3(-1226.737f, -345.204559f, 40.0019035f), new Vector3(-90f, 155.241455f, 0f), 704548, 0, new Vector3(0.9300002f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 538466, 113622690u, new Vector3(-1227.99341f, -342.480316f, 40.0019035f), new Vector3(-90f, 155.241455f, 0f), 604, 0, new Vector3(0.9300002f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 604, 113622690u, new Vector3(-1227.14893f, -342.090851f, 40.0019035f), new Vector3(-90f, 155.241455f, 0f), 704548, 0, new Vector3(0f, -1.86264515E-09f, -2.99999762f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 136267, 113622690u, new Vector3(-1229.24988f, -339.756073f, 40.0019035f), new Vector3(-90f, 155.241455f, 0f), 349564, 0, new Vector3(0.9300002f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 349564, 113622690u, new Vector3(-1228.4054f, -339.3666f, 40.0019035f), new Vector3(-90f, 155.241455f, 0f), 704548, 0, new Vector3(0f, -1.86264515E-09f, -5.99999475f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 450820, 113622690u, new Vector3(-1223.39709f, -341.169037f, 39.1120148f), new Vector3(-70.00008f, 89.99962f, -152.999924f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 678964, 113622690u, new Vector3(-1221.70422f, -340.306427f, 39.11201f), new Vector3(-70.00008f, 89.99962f, -152.999924f), 450820, 0, new Vector3(0f, 0f, -1.90000045f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 231462, 113622690u, new Vector3(-1219.98035f, -341.03598f, 39.0720062f), new Vector3(-90f, -26.2001476f, 0f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 65050, 113622690u, new Vector3(-1219.15027f, -342.7228f, 39.0720062f), new Vector3(-90f, -26.2001476f, 0f), 231462, 0, new Vector3(0f, 0f, -1.87999856f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_service_door_01b"), 719664, 4027516181u, new Vector3(-1228.6792f, -345.743347f, 37.7663422f), new Vector3(0f, -0f, 116.681442f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_service_door_01b"), 108659, 4027516181u, new Vector3(-1228.127f, -346.82f, 37.7663422f), new Vector3(-0f, -0f, -63.3185768f), 719664, 0, new Vector3(-1.21000028f, -0.0100000026f, 0f), new Vector3(0f, 0f, 179.999985f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_fingerprint_scanner_error_01b"), 506207, 1998651489u, new Vector3(-1229.28992f, -344.484528f, 38.030407f), new Vector3(0f, -0f, 118.226936f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstpillar_01"), 289030, 1405006221u, new Vector3(-1225.09f, -341.64093f, 38.40612f), new Vector3(0f, 0f, -63.030056f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstpillar_01"), 728859, 1405006221u, new Vector3(-1223.94714f, -343.886871f, 38.40612f), new Vector3(-0f, -0f, -63.030056f), 289030, 0, new Vector3(2.51999974f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstpillar_01"), 743494, 1405006221u, new Vector3(-1223.13074f, -345.491119f, 38.40612f), new Vector3(-0f, -0f, -63.030056f), 289030, 0, new Vector3(4.319998f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstpillar_01"), 461618, 1405006221u, new Vector3(-1228.41235f, -346.136658f, 38.40612f), new Vector3(-0f, -0f, -63.030056f), 289030, 0, new Vector3(MathF.PI * 113f / 142f, -4.99999762f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_casino_button_01b"), 123399, 1556427679u, new Vector3(-1226.869f, -341.291748f, 37.4877663f), new Vector3(0f, -0f, 117.170319f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_mp_h_str_sideboardl_09"), 71702, 1706990326u, new Vector3(-1226.949f, -338.004852f, 36.61615f), new Vector3(0f, 0f, -63.5231819f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_int_jewel_plant_01"), 71958, 3120149952u, new Vector3(-1227.78625f, -337.089172f, 37.7055931f), new Vector3(0f, 0f, -84.40329f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 158998, 924295337u, new Vector3(-1228.21838f, -336.636444f, 37.778656f), new Vector3(0f, 0f, -63.0215721f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 672792, 924295337u, new Vector3(-1231.87219f, -338.496429f, 37.778656f), new Vector3(-0f, -0f, -63.0215721f), 158998, 0, new Vector3(0f, -4.09999847f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 71280, 924295337u, new Vector3(-1231.87219f, -338.496429f, 40.4786568f), new Vector3(-0f, -0f, -63.0215721f), 158998, 0, new Vector3(0f, -4.09999847f, 2.69999981f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 583993, 924295337u, new Vector3(-1230.44629f, -337.770569f, 40.4786568f), new Vector3(-0f, -0f, -63.0215721f), 158998, 0, new Vector3(0f, -2.5f, 2.69999981f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 575240, 924295337u, new Vector3(-1228.93127f, -336.999359f, 39.9786568f), new Vector3(-90f, 63.02158f, 0f), 158998, 0, new Vector3(0f, -0.8000002f, 2.20000029f), new Vector3(-90.00001f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_gate_05a"), 578934, 3664155221u, new Vector3(-1224.50916f, -342.5692f, 37.70727f), new Vector3(0f, -0f, 115.4484f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_gate_05a"), 682868, 3664155221u, new Vector3(-1219.33f, -344.413f, 37.74249f), new Vector3(0f, 0f, 25.7779274f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 396820, 924295337u, new Vector3(-1226.17761f, -351.453918f, 36.8173332f), new Vector3(0f, -0f, 116.382172f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 177963, 924295337u, new Vector3(-1224.65466f, -350.698517f, 36.8173332f), new Vector3(0f, -0f, 116.382172f), 396820, 0, new Vector3(0f, -1.70000029f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 12867, 924295337u, new Vector3(-1223.13171f, -349.9431f, 36.8173332f), new Vector3(0f, -0f, 116.382172f), 396820, 0, new Vector3(0f, -3.399999f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 125455, 924295337u, new Vector3(-1221.60876f, -349.1877f, 36.8173332f), new Vector3(0f, -0f, 116.382172f), 396820, 0, new Vector3(0f, -5.09999752f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 286229, 924295337u, new Vector3(-1220.08582f, -348.432281f, 36.8173332f), new Vector3(0f, -0f, 116.382172f), 396820, 0, new Vector3(0f, -6.799996f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 422708, 924295337u, new Vector3(-1218.56287f, -347.67688f, 36.8173332f), new Vector3(0f, -0f, 116.382172f), 396820, 0, new Vector3(0f, -8.499996f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 269888, 924295337u, new Vector3(-1217.03992f, -346.921478f, 36.8173332f), new Vector3(0f, -0f, 116.382172f), 396820, 0, new Vector3(0f, -10.2000027f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 301124, 924295337u, new Vector3(-1224.65466f, -350.698517f, 39.517334f), new Vector3(0f, -0f, 116.382172f), 389909, 0, new Vector3(0f, -1.70000029f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 535851, 924295337u, new Vector3(-1223.13171f, -349.9431f, 39.517334f), new Vector3(0f, -0f, 116.382172f), 389909, 0, new Vector3(0f, -3.399999f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 646766, 924295337u, new Vector3(-1221.60876f, -349.1877f, 39.517334f), new Vector3(0f, -0f, 116.382172f), 389909, 0, new Vector3(0f, -5.09999752f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 663364, 924295337u, new Vector3(-1220.08582f, -348.432281f, 39.517334f), new Vector3(0f, -0f, 116.382172f), 389909, 0, new Vector3(0f, -6.799996f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 78085, 924295337u, new Vector3(-1218.56287f, -347.67688f, 39.517334f), new Vector3(0f, -0f, 116.382172f), 389909, 0, new Vector3(0f, -8.499996f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 507007, 924295337u, new Vector3(-1217.03992f, -346.921478f, 39.517334f), new Vector3(0f, -0f, 116.382172f), 389909, 0, new Vector3(0f, -10.2000027f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 389909, 924295337u, new Vector3(-1226.17761f, -351.453918f, 39.517334f), new Vector3(0f, -0f, 116.382172f), 396820, 0, new Vector3(0f, 0f, 2.69999957f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> SafetyDepositBoxPlacements = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 344097, 2068850345u, new Vector3(-1218.42761f, -347.207031f, 36.65254f), new Vector3(0f, -0f, -153.655762f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 302650, 2068850345u, new Vector3(-1222.01221f, -348.9821f, 36.65254f), new Vector3(0f, -0f, -153.655762f), 344097, 0, new Vector3(3.99999857f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 264979, 2068850345u, new Vector3(-1223.80444f, -349.8696f, 36.65254f), new Vector3(0f, -0f, -153.655762f), 344097, 0, new Vector3(5.99999666f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 398390, 2068850345u, new Vector3(-1220.21985f, -348.094543f, 36.65254f), new Vector3(0f, -0f, -153.655762f), 344097, 0, new Vector3(2.00000024f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 14125, 2068850345u, new Vector3(-1219.23938f, -342.7637f, 36.61732f), new Vector3(-0f, -0f, -63.6177979f), 556549, 0, new Vector3(1.96999848f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 556549, 2068850345u, new Vector3(-1220.11475f, -340.998871f, 36.61732f), new Vector3(0f, 0f, -63.6177979f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 158227, 2068850345u, new Vector3(-1223.42517f, -341.301361f, 36.61732f), new Vector3(0f, 0f, 26.3821716f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 158483, 2068850345u, new Vector3(-1221.6604f, -340.425964f, 36.61732f), new Vector3(0f, 0f, 26.3821716f), 158227, 0, new Vector3(1.96999848f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 13695, 2068850345u, new Vector3(-1224.47144f, -346.8435f, 36.61732f), new Vector3(0f, 0f, 26.3821869f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 218737, 2068850345u, new Vector3(-1226.26318f, -347.7322f, 36.61732f), new Vector3(0f, 0f, 26.3821869f), 13695, 0, new Vector3(-2.00000024f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> Loot = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_cash_low_trolly_01c"), 731453, 3781553190u, new Vector3(-1226.17542f, -349.535553f, 37.12746f), new Vector3(0f, -0f, -65.05926f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_cash_low_trolly_01c"), 66821, 3781553190u, new Vector3(-1223.1123f, -347.9189f, 37.12746f), new Vector3(-1.52666616E-13f, -1.35306345E-06f, -235.738754f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_cash_low_trolly_01c"), 178477, 3781553190u, new Vector3(-1218.044f, -345.5055f, 37.12642f), new Vector3(0f, 0f, -244.133911f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_cash_low_trolly_01c"), 312405, 3781553190u, new Vector3(-1222.26147f, -347.706451f, 37.09003f), new Vector3(0f, 0f, -153.44812f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_cash_low_trolly_01c"), 507731, 3781553190u, new Vector3(-1221.48267f, -344.58493f, 37.12746f), new Vector3(0f, -0f, -333.776978f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_cash_low_trolly_01c"), 351042, 3781553190u, new Vector3(-1223.253f, -343.948181f, 37.12746f), new Vector3(0f, -0f, -63.1394958f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> CCTV = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 149104, 168901740u, new Vector3(-1224.67358f, -343.482574f, 39.9993477f), new Vector3(0f, -0f, -128.499054f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 198987, 168901740u, new Vector3(-1220.90576f, -340.556641f, 40.0357361f), new Vector3(0f, 0f, -27.6208153f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 29264, 168901740u, new Vector3(-1231.77661f, -338.867828f, 39.9993477f), new Vector3(0f, 0f, 62.3041229f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 459364, 168901740u, new Vector3(-1227.46814f, -347.3665f, 39.9993477f), new Vector3(0f, -0f, -157.459915f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> SecurityDoors = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_gate_05a"), 380758, 3664155221u, new Vector3(-1224.43689f, -342.534851f, 37.70727f), new Vector3(-0f, -0f, -64.55157f), 578934, 0, new Vector3(0f, -0.08f, 0f), new Vector3(0f, 0f, -179.999985f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_gate_05a"), 79211, 3664155221u, new Vector3(-1219.298f, -344.485f, 37.74249f), new Vector3(0f, -0f, -154.222046f), 682868, 0, new Vector3(0f, -0.0800000057f, 0f), new Vector3(0f, 0f, -179.999985f))
		};

		public static List<ObjectPlacementMap> Locks = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_chain_lock_01a"), 133157, 320272051u, new Vector3(-1224.01257f, -343.5769f, 37.9058762f), new Vector3(0f, 0f, -63.7862167f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_chain_lock_01a"), 119041, 320272051u, new Vector3(-1220.4679f, -344.670074f, 37.9410934f), new Vector3(0f, -0f, -154.783081f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> TellerGlass = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_lab_barrier01"), 158739, 1714438040u, new Vector3(-1227.57654f, -345.843323f, 38.87848f), new Vector3(-0.07010006f, 179.9899f, 27.4963818f), 358983, 0, new Vector3(-1.50000024f, 1.04308128E-07f, -0.200000048f), new Vector3(0f, 0f, 179.999985f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_lab_barrier01"), 123238, 1714438040u, new Vector3(-1224.91553f, -344.4582f, 38.9090042f), new Vector3(0.0701000541f, -179.9899f, -152.503632f), 664858, 0, new Vector3(1.50000024f, 0f, -1.30000019f), new Vector3(0f, 0f, 0f))
		};

		public static List<Prop> Cameras = new List<Prop>();

		public static Blip MainBlip;

		public static Vector3 Enterance;

		public static Vector3 Exit;

		public static Heiststage CurrentHeistStage;

		public static bool SecurityDoorA_Locked = true;

		public static Prop SecurityDoorA;

		public static Prop SecurityDoorA_Lock;

		public static int SecurityDoorA_Stage = 0;

		public static int SecurityDoorA_SyncScene;

		public static bool SecurityDoorB_Locked = true;

		public static Prop SecurityDoorB;

		public static Prop SecurityDoorB_Lock;

		public static int SecurityDoorB_Stage = 0;

		public static int SecurityDoorB_SyncScene;

		public static Prop CutGateBagProp;

		public static Prop CutGateBoltCutters;

		public static int PTFX1;

		public static int PTFX2;

		public static bool IsInInterior;

		public static Camera DoorCamera;

		public static int DoorScene;

		public static List<Prop> SafetyDepostBoxesFakes = new List<Prop>();

		public static List<Prop> SafetyDepostBoxesHolders = new List<Prop>();

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

		public static void ATTACH_ENTITY_TO_ENTITY(Entity entity1, Entity entity2, int boneIndex, float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, bool p9, bool useSoftPinning, bool collision, bool isPed, int vertexIndex, bool fixedRot)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, entity1, entity2, boneIndex, xPos, yPos, zPos, xRot, yRot, zRot, p9, useSoftPinning, collision, isPed, vertexIndex, fixedRot);
		}

		private void OnTick(object sender, EventArgs e)
		{
			Vector3 position = Game.Player.Character.Position;
			if (!TheBetaBankHeistOn || !TheBetaBankHeistOn)
			{
				return;
			}
			if (IsInInterior)
			{
				Function.Call(Hash.SET_RADAR_ZOOM, 1);
			}
			else
			{
				Function.Call(Hash.SET_RADAR_ZOOM, 850);
			}
			if (SecurityDoorA != null)
			{
				SecurityDoorA.Position = new Vector3(SecurityDoors[0].Position.X, SecurityDoors[0].Position.Y, SecurityDoors[0].Position.Z);
				SecurityDoorA.Rotation = new Vector3(0f, 0f, SecurityDoorA.Rotation.Z);
			}
			if (SecurityDoorB != null)
			{
				SecurityDoorB.Position = new Vector3(SecurityDoors[1].Position.X, SecurityDoors[1].Position.Y, SecurityDoors[1].Position.Z);
				SecurityDoorB.Rotation = new Vector3(0f, 0f, SecurityDoorB.Rotation.Z);
			}
			if (SecurityDoorB != null && SecurityDoorB_Lock != null && SecurityDoorB_Locked)
			{
				if (SecurityDoorB_Stage == 0 && GetDistance_VDIST2(Game.Player.Character.Position, SecurityDoorB.Position, "<", 1.25f))
				{
					DisplayHelpTextThisFrame("~INPUT_CONTEXT~ Use Thermal Torch");
					if (Game.IsControlJustPressed(Control.Context))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
						if (Game.Player.Character.Model == PedHash.Trevor)
						{
							Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
						}
						if (Game.Player.Character.Model == PedHash.Michael)
						{
							Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
						}
						if (Game.Player.Character.Model == PedHash.Franklin)
						{
							Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
						}
						string dict = "anim@scripted@heist@ig5_torch_lock@male@";
						LoadDict(dict);
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ih_fin");
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ih_fin");
						if (CutGateBoltCutters != null)
						{
							CutGateBoltCutters.Delete();
							CutGateBoltCutters.MarkAsNoLongerNeeded();
							CutGateBoltCutters = null;
						}
						if (CutGateBagProp != null)
						{
							CutGateBagProp.Delete();
							CutGateBagProp.MarkAsNoLongerNeeded();
							CutGateBagProp = null;
						}
						CutGateBagProp = GlobalHelper.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
						CutGateBagProp.IsCollisionEnabled = false;
						CutGateBagProp.IsInvincible = true;
						CutGateBoltCutters = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_gascutter_01a"), Game.Player.Character.Position, default(Vector3), false, false);
						CutGateBoltCutters.IsPositionFrozen = true;
						CutGateBoltCutters.IsCollisionEnabled = false;
						CutGateBoltCutters.IsInvincible = true;
						Vector3 rotation = SecurityDoorB_Lock.Rotation;
						Vector3 offsetPosition = SecurityDoorB_Lock.GetOffsetPosition(new Vector3(0f, 0f, 0f));
						SecurityDoorB_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.0, 0.0, rotation.Z, 2);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SecurityDoorB_SyncScene, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.0, 0.0, rotation.Z, true);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, SecurityDoorB_Lock, SecurityDoorB_SyncScene, "action_chainlock", LoadDict(dict), 1000.0, -8f, -8f, rotation.Z + 180f, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CutGateBagProp, SecurityDoorB_SyncScene, "action_bag", LoadDict(dict), 1000.0, -8f, -8f, rotation.Z + 180f, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CutGateBoltCutters, SecurityDoorB_SyncScene, "action_gascutter", LoadDict(dict), 1000.0, -8f, -8f, rotation.Z + 180f, 1148846080, 0);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SecurityDoorB_SyncScene, LoadDict(dict), "action_male", 1000.0, -4.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SecurityDoorB_SyncScene, "action_male", LoadDict(dict), 1000.0, -8f, -8f, rotation.Z + 180f, 1148846080, 0);
						Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
						SecurityDoorB_Stage = 1;
					}
				}
				if (SecurityDoorB_Stage == 1 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SecurityDoorB_SyncScene) >= 1054353216)
				{
					SecurityDoorB_Stage = 4;
					Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ornate_heist");
					Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ornate_heist");
					Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ih_fin");
					Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ih_fin");
					PTFX1 = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_ih_fin_cutter_flame", CutGateBoltCutters, -0.344f, 0f, 0.093f, 1f, 0, 0, 0);
					Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ih_fin");
					Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ih_fin");
					PTFX2 = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_ih_fin_cutter_nozzle", CutGateBoltCutters, -0.344f, 0f, 0.093f, 1f, 0, 0, 0);
				}
				if (SecurityDoorB_Stage == 4 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SecurityDoorB_SyncScene) >= 1059353216)
				{
					Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX2, false);
					Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX1, false);
					SecurityDoorB_Stage = 6;
				}
				if (SecurityDoorB_Stage == 6 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SecurityDoorB_SyncScene) >= 1065353216)
				{
					SecurityDoorB.IsPositionFrozen = false;
					SecurityDoorB_Locked = false;
					Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX2, false);
					Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX1, false);
					SecurityDoorB_Stage = -1;
					if (CutGateBagProp != null)
					{
						CutGateBagProp.Delete();
						CutGateBagProp.MarkAsNoLongerNeeded();
						CutGateBagProp = null;
					}
					if (CutGateBoltCutters != null)
					{
						CutGateBoltCutters.Delete();
						CutGateBoltCutters.MarkAsNoLongerNeeded();
						CutGateBoltCutters = null;
					}
					Game.Player.Character.Task.ClearAllImmediately();
					Ped character = Game.Player.Character;
					if (Game.Player.Character.Model == PedHash.Trevor)
					{
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 1, 0, 1);
					}
					if (Game.Player.Character.Model == PedHash.Michael)
					{
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 2, 0, 1);
					}
					if (Game.Player.Character.Model == PedHash.Franklin)
					{
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 6, 0, 1);
					}
				}
			}
			SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Security;
			Heiststage currentHeistStage = CurrentHeistStage;
			UI.RenderQue = 0f;
			switch (currentHeistStage)
			{
			case Heiststage.HeadingToBank:
				if (GetDistance_VDIST2(Game.Player.Character.Position, Enterance, "<", 30f))
				{
					Screen.ShowSubtitle("Head into the ~y~The Betta Bank Branch");
					if (!GetDistance_VDIST2(Game.Player.Character.Position, Enterance, "<", 100f))
					{
						break;
					}
					World.DrawMarker(MarkerType.Cylinder, Enterance, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
					if (!GetDistance_VDIST2(Game.Player.Character.Position, Enterance, "<", 2f))
					{
						break;
					}
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter The Betta Bank Branch");
					if (!Game.IsControlJustPressed(Control.Context))
					{
						break;
					}
					float num2 = -160f;
					Game.Player.Character.Position = new Vector3(-1230.419f, -336.84f, 36.61f);
					Game.Player.Character.Heading = num2;
					Vector3 offsetPosition2 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1.4f, 0f));
					Game.Player.Character.Position = offsetPosition2;
					if (DoorCamera != null)
					{
						Game.Player.Character.IsInvincible = false;
						Game.Player.Character.IsPositionFrozen = false;
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						DoorCamera.IsActive = false;
						DoorCamera.Delete();
					}
					Vector3 offsetPosition3 = Game.Player.Character.GetOffsetPosition(new Vector3(3f, -0.65f, -0.5f));
					DoorCamera = GlobalHelper.CreateCamera(offsetPosition3, new Vector3(0f, 0f, num2 + 90f), 20f);
					World.RenderingCamera = DoorCamera;
					DoorCamera.IsActive = true;
					DoorCamera.FarClip = 2000f;
					LoadDict("anim@apt_trans@hinge_l");
					float heading = Game.Player.Character.Heading;
					DoorScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition2.X, offsetPosition2.Y, offsetPosition2.Z - 1f, 0.0, 0.0, heading, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DoorScene, LoadDict("anim@apt_trans@hinge_l"), "ext_player", 16f, -16f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, DoorScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DoorScene));
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, DoorScene, "ext_player", LoadDict("anim@apt_trans@hinge_l"), 16f, -16f, 0, 1148846080);
					Script.Wait(3000);
					if (DoorCamera != null)
					{
						Screen.FadeOut(600);
						Script.Wait(600);
						Game.Player.Character.IsInvincible = false;
						Game.Player.Character.IsPositionFrozen = false;
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						DoorCamera.IsActive = false;
						DoorCamera.Delete();
					}
					if (MainBlip != null)
					{
						MainBlip.Alpha = 0;
					}
					Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
					Game.Player.WantedLevel = 0;
					IsInInterior = true;
					CurrentHeistStage = Heiststage.TakeOutCameras;
					Game.Player.Character.Task.ClearAllImmediately();
					Game.Player.Character.Position = Exit;
					Script.Wait(1500);
					Screen.FadeIn(1500);
					foreach (ObjectPlacementMap item in CCTV)
					{
						if (item.Entity != null)
						{
							Prop prop = (Prop)item.Entity;
							Cameras.Add(prop);
							prop.AddBlip();
							prop.AttachedBlip.Sprite = BlipSprite.Standard;
							prop.AttachedBlip.Color = BlipColor.Red2;
							prop.AttachedBlip.Name = "Camera";
							prop.AttachedBlip.Scale = 0.5f;
							prop.IsInvincible = false;
							prop.Health = 1000;
						}
					}
					foreach (ObjectPlacementMap safetyDepositBoxPlacement in SafetyDepositBoxPlacements)
					{
						if (safetyDepositBoxPlacement.Entity != null)
						{
							safetyDepositBoxPlacement.Entity.Opacity = 0;
							Random random = new Random();
							int num3 = random.Next(1, 9);
							if (num3 == 1)
							{
								Prop prop2 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), safetyDepositBoxPlacement.Entity.Position, new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z), false, false);
								Prop prop3 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01a"), safetyDepositBoxPlacement.Entity.Position, new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z), false, false);
								prop3.Position = prop2.GetOffsetPosition(new Vector3(0.4f, 0f, 0f));
								prop2.IsVisible = false;
								SafetyDepostBoxesFakes.Add(prop3);
								SafetyDepostBoxesHolders.Add(prop2);
							}
							if (num3 == 2)
							{
								Prop prop4 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), safetyDepositBoxPlacement.Entity.Position, new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z), false, false);
								Prop prop5 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01b"), safetyDepositBoxPlacement.Entity.Position, new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z), false, false);
								prop5.PositionNoOffset = safetyDepositBoxPlacement.Entity.Position;
								prop5.Rotation = new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z);
								prop5.Position = prop4.Position;
								prop4.IsVisible = false;
								SafetyDepostBoxesFakes.Add(prop5);
								SafetyDepostBoxesHolders.Add(prop4);
							}
							if (num3 == 3)
							{
								Prop prop6 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), safetyDepositBoxPlacement.Entity.Position, new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z), false, false);
								Prop prop7 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01c"), safetyDepositBoxPlacement.Entity.Position, new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z), false, false);
								prop7.PositionNoOffset = safetyDepositBoxPlacement.Entity.Position;
								prop7.Rotation = new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z);
								prop7.Position = prop6.GetOffsetPosition(new Vector3(-0.4f, 0f, 0f));
								prop6.IsVisible = false;
								SafetyDepostBoxesFakes.Add(prop7);
								SafetyDepostBoxesHolders.Add(prop6);
							}
							if (num3 == 4)
							{
								Prop prop8 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), safetyDepositBoxPlacement.Entity.Position, new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z), false, false);
								Prop prop9 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01b"), safetyDepositBoxPlacement.Entity.Position, new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z), false, false);
								prop9.PositionNoOffset = safetyDepositBoxPlacement.Entity.Position;
								prop9.Rotation = new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z);
								prop9.Position = prop8.Position;
								prop8.IsVisible = false;
								SafetyDepostBoxesFakes.Add(prop9);
								SafetyDepostBoxesHolders.Add(prop8);
							}
							if (num3 == 5)
							{
								Prop prop10 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), safetyDepositBoxPlacement.Entity.Position, new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z), false, false);
								Prop prop11 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01e"), safetyDepositBoxPlacement.Entity.Position, new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z), false, false);
								prop11.PositionNoOffset = safetyDepositBoxPlacement.Entity.Position;
								prop11.Rotation = new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z);
								prop11.Position = prop10.Position;
								prop10.IsVisible = false;
								SafetyDepostBoxesFakes.Add(prop11);
								SafetyDepostBoxesHolders.Add(prop10);
							}
							if (num3 == 6)
							{
								Prop prop12 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), safetyDepositBoxPlacement.Entity.Position, new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z), false, false);
								Prop prop13 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01f"), safetyDepositBoxPlacement.Entity.Position, new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z), false, false);
								prop13.PositionNoOffset = safetyDepositBoxPlacement.Entity.Position;
								prop13.Rotation = new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z);
								prop13.Position = prop12.GetOffsetPosition(new Vector3(-0.4f, 0f, 0f));
								prop12.IsVisible = false;
								SafetyDepostBoxesFakes.Add(prop13);
								SafetyDepostBoxesHolders.Add(prop12);
							}
							if (num3 == 7)
							{
								Prop prop14 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), safetyDepositBoxPlacement.Entity.Position, new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z), false, false);
								Prop prop15 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01g"), safetyDepositBoxPlacement.Entity.Position, new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z), false, false);
								prop15.PositionNoOffset = safetyDepositBoxPlacement.Entity.Position;
								prop15.Rotation = new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z);
								prop15.Position = prop14.GetOffsetPosition(new Vector3(0.4f, 0f, 0f));
								prop14.IsVisible = false;
								SafetyDepostBoxesFakes.Add(prop15);
								SafetyDepostBoxesHolders.Add(prop14);
							}
							if (num3 == 8)
							{
								Prop prop16 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), safetyDepositBoxPlacement.Entity.Position, new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z), false, false);
								Prop prop17 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01h"), safetyDepositBoxPlacement.Entity.Position, new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z), false, false);
								prop17.PositionNoOffset = safetyDepositBoxPlacement.Entity.Position;
								prop17.Rotation = new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z);
								prop17.Position = prop16.Position;
								prop16.IsVisible = false;
								SafetyDepostBoxesFakes.Add(prop17);
								SafetyDepostBoxesHolders.Add(prop16);
							}
							if (num3 == 9)
							{
								Prop prop18 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), safetyDepositBoxPlacement.Entity.Position, new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z), false, false);
								Prop prop19 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01i"), safetyDepositBoxPlacement.Entity.Position, new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z), false, false);
								prop19.PositionNoOffset = safetyDepositBoxPlacement.Entity.Position;
								prop19.Rotation = new Vector3(0f, 0f, safetyDepositBoxPlacement.Entity.Rotation.Z);
								prop19.Position = prop18.Position;
								prop18.IsVisible = false;
								SafetyDepostBoxesFakes.Add(prop19);
								SafetyDepostBoxesHolders.Add(prop18);
							}
							Script.Wait(1);
						}
					}
				}
				else
				{
					Screen.ShowSubtitle("Head over to ~y~The Betta Bank Branch");
				}
				break;
			case Heiststage.TakeOutCameras:
			{
				CurrentPay = ManageRobberyAnims.CurrentPay;
				if (LocalTake != CurrentPay)
				{
					if (LocalTake < CurrentPay)
					{
						LocalTake += CurrentPay / 200;
					}
					if (LocalTake > CurrentPay)
					{
						LocalTake = CurrentPay;
					}
				}
				if (RobberyAI.CurrentType < 6)
				{
					UI.DrawPayment("TAKE", LocalTake);
				}
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
				Game.Player.WantedLevel = 0;
				Screen.ShowSubtitle("Take out the ~r~Cameras");
				bool flag = true;
				foreach (Prop camera in Cameras)
				{
					if (camera.Health <= 997 && camera.AttachedBlip != null)
					{
						camera.AttachedBlip.Alpha = 0;
					}
				}
				foreach (Prop camera2 in Cameras)
				{
					if (camera2.Health >= 997)
					{
						flag = false;
					}
				}
				if (flag)
				{
					CurrentHeistStage = Heiststage.OpenSecurityGate;
				}
				break;
			}
			case Heiststage.OpenSecurityGate:
				if (SecurityDoorA != null && SecurityDoorA_Lock != null && SecurityDoorA_Locked)
				{
					if (SecurityDoorA_Stage == 0 && GetDistance_VDIST2(Game.Player.Character.Position, SecurityDoorA.Position, "<", 1.25f))
					{
						DisplayHelpTextThisFrame("~INPUT_CONTEXT~ Use Thermal Torch");
						if (Game.IsControlJustPressed(Control.Context))
						{
							Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
							if (Game.Player.Character.Model == PedHash.Trevor)
							{
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
							}
							if (Game.Player.Character.Model == PedHash.Michael)
							{
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
							}
							if (Game.Player.Character.Model == PedHash.Franklin)
							{
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
							}
							string dict2 = "anim@scripted@heist@ig5_torch_lock@male@";
							LoadDict(dict2);
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ih_fin");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ih_fin");
							if (CutGateBoltCutters != null)
							{
								CutGateBoltCutters.Delete();
								CutGateBoltCutters.MarkAsNoLongerNeeded();
								CutGateBoltCutters = null;
							}
							if (CutGateBagProp != null)
							{
								CutGateBagProp.Delete();
								CutGateBagProp.MarkAsNoLongerNeeded();
								CutGateBagProp = null;
							}
							CutGateBagProp = GlobalHelper.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
							CutGateBagProp.IsCollisionEnabled = false;
							CutGateBagProp.IsInvincible = true;
							CutGateBoltCutters = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_gascutter_01a"), Game.Player.Character.Position, default(Vector3), false, false);
							CutGateBoltCutters.IsPositionFrozen = true;
							CutGateBoltCutters.IsCollisionEnabled = false;
							CutGateBoltCutters.IsInvincible = true;
							Vector3 rotation2 = SecurityDoorA_Lock.Rotation;
							Vector3 offsetPosition4 = SecurityDoorA_Lock.GetOffsetPosition(new Vector3(0f, 0f, 0f));
							SecurityDoorA_SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition4.X, offsetPosition4.Y, offsetPosition4.Z, 0.0, 0.0, rotation2.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SecurityDoorA_SyncScene, offsetPosition4.X, offsetPosition4.Y, offsetPosition4.Z, 0.0, 0.0, rotation2.Z, true);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, SecurityDoorA_Lock, SecurityDoorA_SyncScene, "action_chainlock", LoadDict(dict2), 1000.0, -8f, -8f, rotation2.Z + 180f, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CutGateBagProp, SecurityDoorA_SyncScene, "action_bag", LoadDict(dict2), 1000.0, -8f, -8f, rotation2.Z + 180f, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CutGateBoltCutters, SecurityDoorA_SyncScene, "action_gascutter", LoadDict(dict2), 1000.0, -8f, -8f, rotation2.Z + 180f, 1148846080, 0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SecurityDoorA_SyncScene, LoadDict(dict2), "action_male", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SecurityDoorA_SyncScene, "action_male", LoadDict(dict2), 1000.0, -8f, -8f, rotation2.Z + 180f, 1148846080, 0);
							Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
							SecurityDoorA_Stage = 1;
						}
					}
					if (SecurityDoorA_Stage == 1 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SecurityDoorA_SyncScene) >= 1054353216)
					{
						SecurityDoorA_Stage = 4;
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ornate_heist");
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ornate_heist");
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ih_fin");
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ih_fin");
						PTFX1 = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_ih_fin_cutter_flame", CutGateBoltCutters, -0.344f, 0f, 0.093f, 1f, 0, 0, 0);
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ih_fin");
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ih_fin");
						PTFX2 = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_ih_fin_cutter_nozzle", CutGateBoltCutters, -0.344f, 0f, 0.093f, 1f, 0, 0, 0);
					}
					if (SecurityDoorA_Stage == 4 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SecurityDoorA_SyncScene) >= 1059353216)
					{
						Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX2, false);
						Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX1, false);
						SecurityDoorA_Stage = 6;
					}
					if (SecurityDoorA_Stage == 6 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SecurityDoorA_SyncScene) >= 1065353216)
					{
						SecurityDoorA.IsPositionFrozen = false;
						SecurityDoorA_Locked = false;
						Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX2, false);
						Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX1, false);
						SecurityDoorA_Stage = -1;
						if (CutGateBagProp != null)
						{
							CutGateBagProp.Delete();
							CutGateBagProp.MarkAsNoLongerNeeded();
							CutGateBagProp = null;
						}
						if (CutGateBoltCutters != null)
						{
							CutGateBoltCutters.Delete();
							CutGateBoltCutters.MarkAsNoLongerNeeded();
							CutGateBoltCutters = null;
						}
						Game.Player.Character.Task.ClearAllImmediately();
						Ped character2 = Game.Player.Character;
						if (Game.Player.Character.Model == PedHash.Trevor)
						{
							Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 9, 1, 0, 1);
						}
						if (Game.Player.Character.Model == PedHash.Michael)
						{
							Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 9, 2, 0, 1);
						}
						if (Game.Player.Character.Model == PedHash.Franklin)
						{
							Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 9, 6, 0, 1);
						}
					}
				}
				CurrentPay = ManageRobberyAnims.CurrentPay;
				if (LocalTake != CurrentPay)
				{
					if (LocalTake < CurrentPay)
					{
						LocalTake += CurrentPay / 200;
					}
					if (LocalTake > CurrentPay)
					{
						LocalTake = CurrentPay;
					}
				}
				if (RobberyAI.CurrentType < 6)
				{
					UI.DrawPayment("TAKE", LocalTake);
				}
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
				Game.Player.WantedLevel = 0;
				Screen.ShowSubtitle("Open the ~y~Security Gate~w~ with the ~o~Thermal Torch~w~");
				if (SecurityDoorA_Locked)
				{
					break;
				}
				foreach (ObjectPlacementMap item2 in Loot)
				{
					if (item2.Entity != null)
					{
						ManageRobberyAnims.AddPropAsLoot((Prop)item2.Entity);
					}
				}
				foreach (Prop safetyDepostBoxesFake in SafetyDepostBoxesFakes)
				{
					if (safetyDepostBoxesFake != null)
					{
						ManageRobberyAnims.AddPropAsLoot(safetyDepostBoxesFake);
					}
				}
				CurrentHeistStage = Heiststage.LootVault;
				break;
			case Heiststage.LootVault:
				CurrentPay = ManageRobberyAnims.CurrentPay;
				if (LocalTake != CurrentPay)
				{
					if (LocalTake < CurrentPay)
					{
						LocalTake += CurrentPay / 200;
					}
					if (LocalTake > CurrentPay)
					{
						LocalTake = CurrentPay;
					}
				}
				if (RobberyAI.CurrentType < 6)
				{
					UI.DrawPayment("TAKE", LocalTake);
				}
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
				Game.Player.WantedLevel = 0;
				Screen.ShowSubtitle("Loot atleast ~g~$" + ((int)PercentNeedTake).ToString("N"));
				if ((float)ManageRobberyAnims.CurrentPay >= PercentNeedTake)
				{
					CurrentHeistStage = Heiststage.EscapeCops;
				}
				break;
			case Heiststage.EscapeCops:
				CurrentPay = ManageRobberyAnims.CurrentPay;
				if (LocalTake != CurrentPay)
				{
					if (LocalTake < CurrentPay)
					{
						LocalTake += CurrentPay / 200;
					}
					if (LocalTake > CurrentPay)
					{
						LocalTake = CurrentPay;
					}
				}
				if (RobberyAI.CurrentType < 6)
				{
					UI.DrawPayment("TAKE", LocalTake);
				}
				if (IsInInterior)
				{
					Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
					Game.Player.WantedLevel = 0;
					if (GetDistance_VDIST2(Game.Player.Character.Position, Exit, "<", 30f) && GetDistance_VDIST2(Game.Player.Character.Position, Exit, "<", 100f))
					{
						World.DrawMarker(MarkerType.Cylinder, Exit, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
						if (GetDistance_VDIST2(Game.Player.Character.Position, Exit, "<", 2f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit The Betta Bank Branch");
							if (Game.IsControlJustPressed(Control.Context))
							{
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
								Game.Player.WantedLevel = 5;
								Screen.FadeOut(1500);
								Script.Wait(1500);
								IsInInterior = false;
								Game.Player.Character.Position = Enterance;
								CurrentHeistStage = Heiststage.TakeOutCameras;
								Script.Wait(1500);
								Screen.FadeIn(1500);
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
								Game.Player.WantedLevel = 5;
							}
						}
					}
					if (ManageRobberyAnims.LootLeft >= 0f)
					{
						Screen.ShowSubtitle("Keep looting ~g~vauluables~w~ or leave the ~y~The Betta Bank Branch~w~");
					}
					else
					{
						Screen.ShowSubtitle("Leave the ~y~The Betta Bank Branch~w~");
					}
				}
				else
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, Exit, "<", 30f))
					{
						Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
						Game.Player.WantedLevel = 5;
					}
					Screen.ShowSubtitle("Escape the ~r~Cops");
				}
				if (!IsInInterior && GetDistance_VDIST2(Game.Player.Character.Position, Exit, ">", 60f) && Game.Player.WantedLevel == 0)
				{
					Main.TotalCrewEarned += ManageRobberyAnims.CurrentPay;
					Main.TotalPlayerEarned += 0f;
					XP.CompleteHeist_TheBettaBankHeist++;
					Main.SaveCuts();
					Class1.SetNoLongerInHeist();
					MissionScreen.SetPass("The Betta Bank Heist", ManageRobberyAnims.CurrentPay, 0, "The Player has died", 2, 0f, 5);
					int num = Game.GameTime + 30000;
					while (HKHModHelperNew.Class1.HeistPassedScreen != -1 && Game.GameTime <= num)
					{
						Script.Wait(0);
					}
					Script.Wait(2000);
					ManagePlayerFunds.AddFunds(ManageRobberyAnims.CurrentPay);
					XP.AddXP_Percent(XP.CompleteHeist_XPPercent_TheBettaBankHeist);
					EndHeist();
				}
				break;
			}
			if (!Game.Player.IsAlive)
			{
				MissionScreen.SetFail("The Betta Bank Heist", ManageRobberyAnims.CurrentPay, 0, "The Player has died", 2, 0f, 5);
				EndHeist();
			}
		}

		public static void EndHeist()
		{
			Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
			TheBetaBankHeistOn = false;
			if (DoorCamera != null)
			{
				Game.Player.Character.IsInvincible = false;
				Game.Player.Character.IsPositionFrozen = false;
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
				DoorCamera.IsActive = false;
				DoorCamera.Delete();
			}
			if (MainBlip != null)
			{
				MainBlip.Delete();
			}
			Function.Call(Hash.SET_RADAR_ZOOM, 850);
			CurrentPay = 0;
			LocalTake = 0;
			ManageRobberyAnims.CurrentPay = 0;
			ManageRobberyAnims.DeleteLoot();
			SecurityDoorA_Locked = true;
			SecurityDoorA_Stage = 0;
			SecurityDoorB_Locked = true;
			SecurityDoorB_Stage = 0;
			IsInInterior = false;
			CurrentHeistStage = Heiststage.None;
			SmartGuards.ClearGenericGuards();
			TheBetaBankHeistOn = false;
			if (MainBlip != null)
			{
				MainBlip.Delete();
			}
			foreach (Prop safetyDepostBoxesFake in SafetyDepostBoxesFakes)
			{
				if (safetyDepostBoxesFake != null)
				{
					safetyDepostBoxesFake.Delete();
				}
			}
			SafetyDepostBoxesFakes.Clear();
			foreach (Prop safetyDepostBoxesHolder in SafetyDepostBoxesHolders)
			{
				if (safetyDepostBoxesHolder != null)
				{
					safetyDepostBoxesHolder.Delete();
				}
			}
			SafetyDepostBoxesHolders.Clear();
			foreach (ObjectPlacementMap item in Interior)
			{
				if (item.Entity != null)
				{
					item.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap safetyDepositBoxPlacement in SafetyDepositBoxPlacements)
			{
				if (safetyDepositBoxPlacement.Entity != null)
				{
					safetyDepositBoxPlacement.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item2 in Loot)
			{
				if (item2.Entity != null)
				{
					item2.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item3 in CCTV)
			{
				if (item3.Entity != null)
				{
					item3.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap securityDoor in SecurityDoors)
			{
				if (securityDoor.Entity != null)
				{
					securityDoor.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item4 in TellerGlass)
			{
				if (item4.Entity != null)
				{
					item4.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap @lock in Locks)
			{
				if (@lock.Entity != null)
				{
					@lock.Entity.Delete();
				}
			}
		}

		public static void StartHeist()
		{
			ManageRobberyAnims.CurrentPay = 0;
			ManageRobberyAnims.Heist_Average_Pay = (float)(HeistMinPay + HeistMaxPay) / 2f * 1.5f;
			Function.Call(Hash.BEGIN_TEXT_COMMAND_BUSYSPINNER_ON, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "Loading The Betta Bank Heist, Please Wait");
			Function.Call(Hash.END_TEXT_COMMAND_BUSYSPINNER_ON, 5);
			EndHeist();
			foreach (ObjectPlacementMap item in Interior)
			{
				item.CreateObjatCoords();
			}
			foreach (ObjectPlacementMap safetyDepositBoxPlacement in SafetyDepositBoxPlacements)
			{
				safetyDepositBoxPlacement.CreateObjatCoords();
			}
			foreach (ObjectPlacementMap item2 in Loot)
			{
				item2.CreateObjatCoords();
			}
			foreach (ObjectPlacementMap item3 in CCTV)
			{
				item3.CreateObjatCoords();
			}
			foreach (ObjectPlacementMap securityDoor in SecurityDoors)
			{
				securityDoor.CreateObjatCoords();
			}
			foreach (ObjectPlacementMap @lock in Locks)
			{
				@lock.CreateObjatCoords();
			}
			foreach (ObjectPlacementMap item4 in TellerGlass)
			{
				item4.CreateObjatCoords();
				if (item4.Entity != null)
				{
					item4.Entity.IsInvincible = false;
				}
			}
			Enterance = Interior[0].Entity.Position;
			Interior[0].Entity.Opacity = 0;
			Exit = Interior[1].Entity.Position;
			Interior[1].Entity.Opacity = 0;
			SecurityDoorA = (Prop)SecurityDoors[0].Entity;
			SecurityDoorA_Lock = (Prop)Locks[0].Entity;
			SecurityDoorB = (Prop)SecurityDoors[1].Entity;
			SecurityDoorB_Lock = (Prop)Locks[1].Entity;
			CurrentHeistStage = Heiststage.HeadingToBank;
			TheBetaBankHeistOn = true;
			if (MainBlip != null)
			{
				MainBlip.Delete();
			}
			MainBlip = GlobalHelper.CreateBlip(Enterance);
			MainBlip.Color = BlipColor.Yellow2;
			MainBlip.IsShortRange = true;
			MainBlip.Name = "Enter the Betta Bank Heist";
			Function.Call(Hash.BUSYSPINNER_OFF);
		}

		public TheBetaBankHeist()
		{
			base.Tick += OnTick;
		}
	}
}
