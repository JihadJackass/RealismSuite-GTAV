using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using HKHModHelperNew;
using SinglePlayerHeists.UtilsMisc;
using SmartAIGuards;

namespace SinglePlayerHeists.HitNRun
{
	internal class TheGefangnisArtHeist : Script
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
			HeadingToStore,
			TakeOutCameras,
			LootStore,
			EscapeCops
		}

		public static int HeistMinPay = 1650000;

		public static int HeistMaxPay = 650000;

		public static float PercentNeedTake = (float)(HeistMinPay + HeistMaxPay) / 1.5f / 100f * 55f;

		public static int CurrentPay = 0;

		public static int LocalTake = 0;

		public static bool TheGefangnisArtHeistOn;

		public static List<ObjectPlacementMap> Interior = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_pillar"), 369926, 223971722u, new Vector3(-666.939148f, -888.745361f, 23.5115833f), new Vector3(0f, -0f, 90.54093f), 394758, 0, new Vector3(6.89999962f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_pillar"), 394758, 223971722u, new Vector3(-666.874f, -895.6451f, 23.5115833f), new Vector3(0f, -0f, 90.54093f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xs_prop_arena_wall_02a_sf"), 446214, 1897672472u, new Vector3(-666.874f, -895.6451f, 23.5115833f), new Vector3(0f, -0f, -179.459076f), 394758, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 89.99999f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xs_prop_arena_wall_02a_sf"), 446470, 1897672472u, new Vector3(-676.8736f, -895.7395f, 23.5115833f), new Vector3(0f, -0f, -179.459076f), 446214, 0, new Vector3(9.999999f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xs_prop_arena_wall_02a_sf"), 446726, 1897672472u, new Vector3(-686.7732f, -895.832947f, 23.5115833f), new Vector3(0f, -0f, -179.459076f), 446214, 0, new Vector3(19.9000359f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xs_prop_arena_wall_02a_sf"), 446982, 1897672472u, new Vector3(-688.9031f, -895.8531f, 23.5115833f), new Vector3(0f, -0f, 90.54093f), 446214, 0, new Vector3(22.0300446f, 0f, 0f), new Vector3(0f, 0f, -89.9999847f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xs_prop_arena_wall_02a_sf"), 447238, 1897672472u, new Vector3(-688.9975f, -885.8535f, 23.5115833f), new Vector3(0f, -0f, 90.54093f), 446214, 0, new Vector3(22.0300446f, -10.0000019f, 0f), new Vector3(0f, 0f, -89.9999847f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xs_prop_arena_wall_02a_sf"), 447494, 1897672472u, new Vector3(-689.0664f, -884.552856f, 23.5115833f), new Vector3(0f, 0f, 0.5409485f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xs_prop_arena_wall_02a_sf"), 447750, 1897672472u, new Vector3(-679.1669f, -884.4594f, 23.5115833f), new Vector3(0f, 0f, 0.5409485f), 447494, 0, new Vector3(9.900002f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xs_prop_arena_wall_02a_sf"), 614659, 1897672472u, new Vector3(-677.2651f, -878.6424f, 23.5115833f), new Vector3(0f, 0f, -89.45902f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xs_prop_arena_wall_02a_sf"), 46598, 1897672472u, new Vector3(-666.9692f, -888.7456f, 23.5115833f), new Vector3(0f, -0f, -179.459f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_pillar"), 448006, 223971722u, new Vector3(-677.0687f, -888.840942f, 23.5115833f), new Vector3(0f, -0f, -179.459f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstpillar_01"), 533766, 1405006221u, new Vector3(-666.897766f, -890.540955f, 25.30195f), new Vector3(0f, -0f, 91.73152f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstpillar_01"), 552198, 1405006221u, new Vector3(-666.8424f, -893.787964f, 25.30204f), new Vector3(0f, -0f, 90.7590256f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 596230, 924295337u, new Vector3(-666.925f, -889.641357f, 23.10195f), new Vector3(0f, 0f, 1.73152375f), 533766, 0, new Vector3(0.900000155f, 0f, -2.2f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 619526, 924295337u, new Vector3(-666.764832f, -894.938965f, 23.10195f), new Vector3(0f, 0f, 1.73152375f), 533766, 0, new Vector3(-4.399998f, 0f, -2.2f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_go_window"), 655622, 2310905709u, new Vector3(-666.6528f, -895.335754f, 25.4019489f), new Vector3(-0f, -0f, -88.26847f), 619526, 0, new Vector3(0.100000016f, -0.400000066f, 2.3f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_go_window"), 674054, 2310905709u, new Vector3(-666.852234f, -888.7388f, 25.4019489f), new Vector3(-0f, -0f, -88.26847f), 596230, 0, new Vector3(0.100000016f, 0.9000001f, 2.3f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstpillar_01"), 697862, 1405006221u, new Vector3(-666.8424f, -893.787964f, 28.8020382f), new Vector3(0f, -0f, 90.7590256f), 552198, 0, new Vector3(0f, 0f, 3.49999881f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstpillar_01"), 716550, 1405006221u, new Vector3(-666.897766f, -890.540955f, 28.8019485f), new Vector3(0f, -0f, 91.73152f), 533766, 0, new Vector3(0f, -2.23517418E-07f, 3.49999881f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 736774, 924295337u, new Vector3(-666.925f, -889.641357f, 28.20195f), new Vector3(0f, 0f, 1.73152375f), 533766, 0, new Vector3(0.900000155f, 0f, 2.89999962f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 752134, 924295337u, new Vector3(-666.764832f, -894.938965f, 28.30195f), new Vector3(0f, 0f, 1.73152375f), 533766, 0, new Vector3(-4.399998f, 0f, 2.99999952f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 772102, 924295337u, new Vector3(-666.8482f, -892.180237f, 26.9119511f), new Vector3(-90f, -1.73152542f, 0f), 752134, 0, new Vector3(0f, 2.75999951f, -1.389999f), new Vector3(-90.0000153f, -1.90734863E-06f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 15879, 924295337u, new Vector3(-666.8482f, -892.180237f, 28.52195f), new Vector3(-90f, -1.73152542f, 0f), 752134, 0, new Vector3(0f, 2.75999951f, 0.220000073f), new Vector3(-90.0000153f, -1.90734863E-06f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 36871, 924295337u, new Vector3(-684.6007f, -888.843262f, 23.4748821f), new Vector3(0f, 0f, 1.93344378f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 57351, 924295337u, new Vector3(-684.4008f, -888.8365f, 23.4748821f), new Vector3(0f, 0f, 1.93344378f), 36871, 0, new Vector3(0.200000018f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 81671, 924295337u, new Vector3(-684.2009f, -888.8298f, 23.4748821f), new Vector3(0f, 0f, 1.93344378f), 36871, 0, new Vector3(0.400000036f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 107015, 924295337u, new Vector3(-684.001038f, -888.823f, 23.4748821f), new Vector3(0f, 0f, 1.93344378f), 36871, 0, new Vector3(0.6000001f, 1.49011612E-08f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 132615, 924295337u, new Vector3(-669.7481f, -892.227844f, 24.80195f), new Vector3(0f, 0f, 1.73152375f), 619526, 0, new Vector3(-2.89999938f, 2.79999948f, 1.70000017f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 200199, 938137634u, new Vector3(-668.059631f, -895.1232f, 24.2003021f), new Vector3(0f, -0f, -179.6616f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 217607, 1870138714u, new Vector3(-668.059631f, -895.1232f, 24.2003021f), new Vector3(0f, -0f, -179.6616f), 233991, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 233991, 2852049058u, new Vector3(-668.059631f, -895.1232f, 24.2003021f), new Vector3(0f, -0f, -179.6616f), 200199, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 234247, 924295337u, new Vector3(-668.8464f, -895.677856f, 23.4203014f), new Vector3(0f, -0f, -179.6616f), 200199, 0, new Vector3(0.789999962f, 0.55f, -0.78f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 268551, 2852049058u, new Vector3(-669.6296f, -895.1325f, 24.2003021f), new Vector3(0f, -0f, -179.6616f), 296455, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 268807, 924295337u, new Vector3(-670.4163f, -895.687134f, 23.4203014f), new Vector3(0f, -0f, -179.6616f), 296455, 0, new Vector3(0.789999962f, 0.55f, -0.78f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 296455, 938137634u, new Vector3(-669.6296f, -895.1325f, 24.2003021f), new Vector3(0f, -0f, -179.6616f), 200199, 0, new Vector3(1.56999886f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 296711, 2852049058u, new Vector3(-671.1896f, -895.1417f, 24.2003021f), new Vector3(0f, -0f, -179.6616f), 296967, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 296967, 938137634u, new Vector3(-671.1896f, -895.1417f, 24.2003021f), new Vector3(0f, -0f, -179.6616f), 200199, 0, new Vector3(3.12999749f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 297223, 924295337u, new Vector3(-671.966248f, -895.7063f, 23.4103031f), new Vector3(0f, -0f, -179.6616f), 296967, 0, new Vector3(0.779999554f, 0.559999764f, -0.789999545f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 327687, 1870138714u, new Vector3(-671.1896f, -895.1417f, 24.2003021f), new Vector3(0f, -0f, -179.6616f), 296967, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 327943, 924295337u, new Vector3(-672.9772f, -895.5523f, 24.6003017f), new Vector3(0f, -0f, 90.3384f), 327687, 0, new Vector3(1.7900002f, 0.399999976f, 0.3999999f), new Vector3(0f, 0f, -89.99999f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 353031, 2852049058u, new Vector3(-674.7695f, -895.172852f, 24.2003021f), new Vector3(0f, -0f, -179.6616f), 378375, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 353287, 924295337u, new Vector3(-675.5562f, -895.7275f, 23.4203014f), new Vector3(0f, -0f, -179.6616f), 378375, 0, new Vector3(0.789999962f, 0.55f, -0.78f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 377863, 938137634u, new Vector3(-676.3394f, -895.1821f, 24.2003021f), new Vector3(0f, -0f, -179.6616f), 378375, 0, new Vector3(1.56999886f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 378119, 938137634u, new Vector3(-677.8994f, -895.191345f, 24.2003021f), new Vector3(0f, -0f, -179.6616f), 378375, 0, new Vector3(3.12999749f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 378375, 938137634u, new Vector3(-674.7695f, -895.172852f, 24.2003021f), new Vector3(0f, -0f, -179.6616f), 327687, 0, new Vector3(3.579997f, 0.01f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 378631, 924295337u, new Vector3(-673.9862f, -895.718262f, 23.4103031f), new Vector3(0f, -0f, -179.6616f), 296967, 0, new Vector3(2.79999781f, 0.559999764f, -0.789999545f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 420871, 924295337u, new Vector3(-668.1163f, -888.923462f, 24.6003017f), new Vector3(0f, -0f, 90.3384f), 327943, 0, new Vector3(6.599999f, -4.89999771f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 451335, 924295337u, new Vector3(-669.8162f, -888.933533f, 24.6003017f), new Vector3(0f, -0f, 90.3384f), 327943, 0, new Vector3(6.599999f, -3.19999933f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 474631, 924295337u, new Vector3(-671.5162f, -888.943542f, 24.6003017f), new Vector3(0f, -0f, 90.3384f), 327943, 0, new Vector3(6.599999f, -1.50000036f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 503303, 924295337u, new Vector3(-673.2161f, -888.9536f, 24.6003017f), new Vector3(0f, -0f, 90.3384f), 327943, 0, new Vector3(6.599999f, 0.199999839f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 523271, 924295337u, new Vector3(-674.916138f, -888.9636f, 24.6003017f), new Vector3(0f, -0f, 90.3384f), 327943, 0, new Vector3(6.599999f, 1.89999986f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 567303, 924295337u, new Vector3(-676.4161f, -888.972534f, 24.6003017f), new Vector3(0f, -0f, 90.3384f), 327943, 0, new Vector3(6.599999f, 3.39999866f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 649735, 924295337u, new Vector3(-676.3451f, -892.4273f, 24.80195f), new Vector3(0f, 0f, 1.73152375f), 619526, 0, new Vector3(-9.5f, 2.79999948f, 1.70000017f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_mp_h_acc_artwalll_03"), 720391, 2642951151u, new Vector3(-669.641663f, -895.474f, 26.5313473f), new Vector3(0f, -0f, -179.426346f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_mp_h_acc_artwalll_02"), 735239, 2866697883u, new Vector3(-676.240845f, -895.5363f, 26.5372753f), new Vector3(0f, -0f, -179.426346f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 735495, 924295337u, new Vector3(-677.11615f, -895.7367f, 23.4203014f), new Vector3(0f, -0f, -179.6616f), 378375, 0, new Vector3(MathF.E * 134f / 155f, 0.55f, -0.78f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 760839, 924295337u, new Vector3(-678.566162f, -895.7453f, 23.4203014f), new Vector3(0f, -0f, -179.6616f), 378375, 0, new Vector3(3.799997f, 0.55f, -0.78f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sm_prop_smug_hgrdoors_01"), 14856, 3293339660u, new Vector3(-683.298767f, -881.098267f, 28.4115829f), new Vector3(90f, 0.5409485f, 0f), 447494, 0, new Vector3(5.79999971f, 3.39999914f, 4.89999962f), new Vector3(89.99999f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sm_prop_smug_hgrdoors_01"), 15112, 3293339660u, new Vector3(-685.146667f, -897.216431f, 28.3915844f), new Vector3(-90f, -0.5409485f, 0f), 447494, 0, new Vector3(3.79999971f, -12.7f, 4.880001f), new Vector3(-90f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sm_prop_smug_hgrdoors_01"), 15624, 3293339660u, new Vector3(-674.147156f, -897.112549f, 28.4115829f), new Vector3(-90f, -0.5409485f, 0f), 447494, 0, new Vector3(14.7999992f, -12.7f, 4.89999962f), new Vector3(-90f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_sign_gefangnis"), 37128, 735518121u, new Vector3(-666.512f, -892.05f, 28.52195f), new Vector3(6.830189E-06f, 1.36603776E-05f, 91.73152f), 37384, 0, new Vector3(0.190000013f, 0f, 1.48999894f), new Vector3(0f, -90.00001f, 90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 37384, 924295337u, new Vector3(-666.6569f, -893.545044f, 28.52195f), new Vector3(-90f, -1.73152542f, 0f), 752134, 0, new Vector3(0.150000036f, 1.39000082f, 0.220000088f), new Vector3(-90.0000153f, -1.90734863E-06f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 62472, 924295337u, new Vector3(-666.738831f, -890.8363f, 28.52195f), new Vector3(-90f, -1.73152542f, 0f), 37384, 0, new Vector3(3.7252903E-09f, -1.11758709E-08f, 2.709998f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 87816, 924295337u, new Vector3(-679.587036f, -895.5913f, 24.6003017f), new Vector3(0f, -0f, 90.3384f), 327687, 0, new Vector3(8.399996f, 0.399999976f, 0.3999999f), new Vector3(0f, 0f, -89.99999f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 113416, 924295337u, new Vector3(-683.7412f, -888.8143f, 23.4748821f), new Vector3(0f, 0f, 1.93344378f), 36871, 0, new Vector3(0.859999835f, 1.49011612E-08f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 142856, 924295337u, new Vector3(-683.491333f, -888.8058f, 23.4748821f), new Vector3(0f, 0f, 1.93344378f), 36871, 0, new Vector3(1.10999966f, 1.49011612E-08f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 241927, 924295337u, new Vector3(-683.251465f, -888.7977f, 23.4748821f), new Vector3(0f, 0f, 1.93344378f), 36871, 0, new Vector3(1.34999943f, 1.49011612E-08f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_res_sculpt_decf"), 196104, 3833108893u, new Vector3(-676.357f, -895.160156f, 24.3376732f), new Vector3(0f, -0f, -177.98671f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 196360, 924295337u, new Vector3(-682.2961f, -895.767334f, 23.4203014f), new Vector3(0f, -0f, -179.6616f), 378375, 0, new Vector3(7.530001f, 0.55f, -0.78f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 224008, 924295337u, new Vector3(-681.287048f, -895.6014f, 24.6003017f), new Vector3(0f, -0f, 90.3384f), 327687, 0, new Vector3(10.1000023f, 0.399999976f, 0.3999999f), new Vector3(0f, 0f, -89.99999f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 258568, 924295337u, new Vector3(-682.5561f, -895.76886f, 23.4203014f), new Vector3(0f, -0f, -179.6616f), 378375, 0, new Vector3(7.790007f, 0.55f, -0.78f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 292360, 924295337u, new Vector3(-682.8261f, -895.770447f, 23.4203014f), new Vector3(0f, -0f, -179.6616f), 378375, 0, new Vector3(8.060013f, 0.55f, -0.78f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_corp_bk_bust"), 340232, 918182878u, new Vector3(-667.343933f, -890.5236f, 24.483057f), new Vector3(0f, 0f, -89.49116f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_corp_bk_bust"), 340488, 918182878u, new Vector3(-667.229858f, -893.79895f, 24.483057f), new Vector3(0f, 0f, -89.49039f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_idol_01_error"), 360200, 3278675464u, new Vector3(-677.8642f, -895.137f, 24.4907f), new Vector3(0f, -0f, -178.133789f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_res_m_statue"), 374024, 1486357116u, new Vector3(-682.572937f, -895.168152f, 24.8191376f), new Vector3(0f, -0f, -179.6692f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_necklace_01a"), 625415, 2472404644u, new Vector3(-682.5531f, -895.138062f, 25.0991383f), new Vector3(-1.0422041E-10f, -0.200000033f, -179.6692f), 374024, 0, new Vector3(-0.0200000033f, -0.0300000049f, 0.280000061f), new Vector3(0f, -0.200000033f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_miniature_yacht_01c"), 274438, 4250530917u, new Vector3(-674.5535f, -895.30426f, 24.3376732f), new Vector3(0f, -0f, -174.2407f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_miniature_yacht_01b"), 459272, 3946074138u, new Vector3(-674.8626f, -895.1999f, 24.3376732f), new Vector3(0f, -0f, -170.606171f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_miniature_yacht_01a"), 473096, 2597105484u, new Vector3(-675.111145f, -895.0785f, 24.3376732f), new Vector3(0f, -0f, -164.360413f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_miniature_09a"), 491784, 1108211937u, new Vector3(-670.8711f, -895.115f, 24.3376732f), new Vector3(0f, -0f, -161.49321f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_miniature_05c"), 512008, 1194952728u, new Vector3(-671.2169f, -895.1525f, 24.3376732f), new Vector3(0f, -0f, 175.9868f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_car_12a"), 531208, 509893457u, new Vector3(-669.1229f, -895.117737f, 24.3376732f), new Vector3(0f, 0f, -87.68741f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_car_11a"), 540936, 1895038807u, new Vector3(-669.298645f, -895.1238f, 24.3376732f), new Vector3(0f, 0f, -87.58472f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_car_10a"), 554760, 1336492050u, new Vector3(-669.4689f, -895.13385f, 24.3376732f), new Vector3(0f, 0f, -88.09629f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_car_09a"), 574728, 419741070u, new Vector3(-669.6192f, -895.142761f, 24.3376732f), new Vector3(0f, 0f, -88.76149f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_car_08a"), 599304, 2376509436u, new Vector3(-669.777f, -895.128052f, 24.3376732f), new Vector3(0f, 0f, -88.6955643f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_car_07a"), 625928, 4144231117u, new Vector3(-669.9362f, -895.121948f, 24.3376732f), new Vector3(0f, 0f, -88.0811157f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_car_04a"), 644872, 187047509u, new Vector3(-670.1012f, -895.1317f, 24.3376732f), new Vector3(0f, 0f, -88.1249161f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_skull_01b"), 659208, 1925085104u, new Vector3(-671.53656f, -895.1409f, 24.3376732f), new Vector3(0f, -0f, 157.893784f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cs_power_cell"), 677128, 1456723945u, new Vector3(-667.672241f, -895.0875f, 24.43143f), new Vector3(0f, -0f, -131.167786f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "w_ar_assaultrifle_luxe"), 704776, 753361113u, new Vector3(-668.1549f, -895.0621f, 24.3826065f), new Vector3(-90f, -166.442276f, 0f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_res_m_urn"), 723464, 1256172027u, new Vector3(-677.1781f, -889.6784f, 23.61198f), new Vector3(0f, 0f, -88.59842f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xs_prop_ar_planter_c_02a_sf"), 749320, 1020671082u, new Vector3(-679.207764f, -886.868164f, 24.5120449f), new Vector3(1.33403146E-08f, -179.9999f, -0.8738389f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "0x2c7b71c5"), 763912, 746287557u, new Vector3(-679.1823f, -885.199341f, 23.722044f), new Vector3(-0.000112925496f, 1.3363012E-08f, -90.87382f), 749320, 0, new Vector3(0f, 1.66899872f, 0.7900002f), new Vector3(179.999985f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "0x2c7b71c5"), 764168, 746287557u, new Vector3(-679.213867f, -887.270142f, 23.7120438f), new Vector3(-0.000112925496f, 1.3363012E-08f, -90.87382f), 749320, 0, new Vector3(0f, -0.402000248f, 0.8000002f), new Vector3(179.999985f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_chair_02"), 779016, 3465683653u, new Vector3(-677.9451f, -886.1946f, 24.2126484f), new Vector3(0f, 0f, -55.48235f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_ret_gc_cashreg"), 18697, 3997648379u, new Vector3(-678.8825f, -886.6831f, 24.717226f), new Vector3(0f, -0f, 92.89141f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_computer_01"), 33801, 1613369486u, new Vector3(-678.8578f, -885.75885f, 24.5185261f), new Vector3(0f, 0f, 70.7807846f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_planter_02"), 300808, 1373401077u, new Vector3(-683.917053f, -888.8026f, 24.7385826f), new Vector3(0f, 0f, 0.100476496f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 49929, 924295337u, new Vector3(-685.680847f, -892.709534f, 24.80195f), new Vector3(0f, 0f, 1.73152375f), 619526, 0, new Vector3(-18.8400383f, 2.79999948f, 1.70000017f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 73225, 2852049058u, new Vector3(-668.059631f, -895.1232f, 24.2003021f), new Vector3(0f, -0f, -179.6616f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 73481, 924295337u, new Vector3(-668.8464f, -895.677856f, 23.4203014f), new Vector3(0f, -0f, -179.6616f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_wallart_08a"), 456967, 4266556866u, new Vector3(-677.4001f, -885.231f, 25.3777447f), new Vector3(0f, -0f, -90.10538f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_wallart_07a"), 175625, 2471700725u, new Vector3(-677.4026f, -886.221f, 25.3782673f), new Vector3(0f, -0f, -90.10538f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_wallart_06a"), 191753, 3598035941u, new Vector3(-677.3814f, -887.2112f, 25.3610153f), new Vector3(0f, -0f, -90.10538f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_res_fh_sculptmod"), 213257, 3903811192u, new Vector3(-679.390869f, -888.138062f, 24.5124416f), new Vector3(0f, -0f, -92.10025f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_toy_sculpture_01a"), 259081, 537692219u, new Vector3(-678.152954f, -895.126038f, 24.3376732f), new Vector3(0f, -0f, 169.837067f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 259337, 924295337u, new Vector3(-679.587036f, -895.5913f, 24.6003017f), new Vector3(0f, -0f, 90.3384f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 342281, 938137634u, new Vector3(-674.7695f, -895.172852f, 24.2003021f), new Vector3(0f, -0f, -179.6616f), 342793, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 342537, 938137634u, new Vector3(-674.7695f, -895.172852f, 24.2003021f), new Vector3(0f, -0f, -179.6616f), 342793, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 342793, 938137634u, new Vector3(-674.7695f, -895.172852f, 24.2003021f), new Vector3(0f, -0f, -179.6616f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 343049, 924295337u, new Vector3(-670.4163f, -895.687134f, 23.4203014f), new Vector3(0f, -0f, -179.6616f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 362505, 924295337u, new Vector3(-671.966248f, -895.7063f, 23.4103031f), new Vector3(0f, -0f, -179.6616f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 388617, 924295337u, new Vector3(-672.9772f, -895.5523f, 24.6003017f), new Vector3(0f, -0f, 90.3384f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 402185, 924295337u, new Vector3(-680.985535f, -884.566833f, 24.6003017f), new Vector3(0f, -0f, 90.7383957f), 442633, 0, new Vector3(2.715729f, -0.06545235f, 1.1799984f), new Vector3(0f, 0f, 90.71496f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 422153, 924295337u, new Vector3(-682.695435f, -884.5888f, 24.6003017f), new Vector3(0f, -0f, 90.7383957f), 442633, 0, new Vector3(1.00581944f, -0.08669556f, 1.1799984f), new Vector3(0f, 0f, 90.71496f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 442377, 938137634u, new Vector3(-684.471069f, -885.042847f, 24.2003021f), new Vector3(0f, 0f, 0.0234451182f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 442633, 924295337u, new Vector3(-683.7013f, -884.5025f, 23.4203033f), new Vector3(0f, 0f, 0.0234451182f), 442377, 0, new Vector3(0.770000041f, 0.539999843f, -0.7799996f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 468233, 924295337u, new Vector3(-685.2413f, -884.5032f, 23.4203033f), new Vector3(0f, 0f, 0.0234451182f), 442377, 0, new Vector3(-0.7699992f, 0.539999843f, -0.7799996f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 481801, 924295337u, new Vector3(-687.705444f, -884.61084f, 24.6003017f), new Vector3(0f, -0f, 90.7383957f), 442633, 0, new Vector3(-4.004177f, -0.106695563f, 1.1799984f), new Vector3(0f, 0f, 90.71496f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 506121, 924295337u, new Vector3(-685.9954f, -884.590149f, 24.6003017f), new Vector3(0f, -0f, 90.7383957f), 442633, 0, new Vector3(-2.29418087f, -0.08669556f, 1.1799984f), new Vector3(0f, 0f, 90.71496f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 673289, 924295337u, new Vector3(-672.9772f, -895.5523f, 24.6003017f), new Vector3(0f, -0f, 90.3384f), 388617, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 696841, 924295337u, new Vector3(-672.9772f, -895.5523f, 24.6003017f), new Vector3(0f, -0f, 90.3384f), 388617, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 720137, 924295337u, new Vector3(-672.9772f, -895.5523f, 24.6003017f), new Vector3(0f, -0f, 90.3384f), 388617, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 741385, 924295337u, new Vector3(-672.9772f, -895.5523f, 24.6003017f), new Vector3(0f, -0f, 90.3384f), 388617, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 756233, 924295337u, new Vector3(-672.9772f, -895.5523f, 24.6003017f), new Vector3(0f, -0f, 90.3384f), 388617, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sm_prop_smug_hgrdoors_01"), 781833, 3293339660u, new Vector3(-683.3779f, -881.189f, 23.4015827f), new Vector3(90f, 0.5409485f, 0f), 447494, 0, new Vector3(5.719998f, 3.30999923f, -0.110000744f), new Vector3(89.99999f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sm_prop_smug_hgrdoors_01"), 782089, 3293339660u, new Vector3(-685.146667f, -897.216431f, 23.4115677f), new Vector3(-90f, -0.5409485f, 0f), 447494, 0, new Vector3(3.79999971f, -12.7f, -0.100015387f), new Vector3(-90f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sm_prop_smug_hgrdoors_01"), 782345, 3293339660u, new Vector3(-674.147156f, -897.112549f, 23.4065666f), new Vector3(-90f, -0.5409485f, 0f), 447494, 0, new Vector3(14.7999992f, -12.7f, -0.105017304f), new Vector3(-90f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_colle_alien"), 134666, 1298470051u, new Vector3(-684.471741f, -885.0726f, 24.3390789f), new Vector3(0f, 0f, 1.81455815f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_power_cell"), 149770, 2235081574u, new Vector3(-684.111755f, -885.05f, 24.4273167f), new Vector3(0f, 0f, -20.1420765f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 121353, 113622690u, new Vector3(-676.3451f, -892.4273f, 26.1719513f), new Vector3(-1.70754765E-06f, -90f, -88.26848f), 649735, 0, new Vector3(0f, 0f, 1.37000012f), new Vector3(0f, -90.00001f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 239112, 113622690u, new Vector3(-685.6908f, -892.709839f, 26.1919518f), new Vector3(-1.70754765E-06f, -90f, -88.26848f), 121353, 0, new Vector3(0.0200000033f, -9.35f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 370696, 113622690u, new Vector3(-669.7481f, -892.2279f, 26.1719513f), new Vector3(-1.70754765E-06f, -90f, -88.26848f), 121353, 0, new Vector3(0f, 6.599997f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 156938, 938137634u, new Vector3(-688.376343f, -894.747559f, 24.1514931f), new Vector3(0f, -0f, 90.3137741f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 157194, 924295337u, new Vector3(-688.930542f, -893.9806f, 23.3814926f), new Vector3(0f, -0f, 90.3137741f), 156938, 0, new Vector3(0.7699996f, 0.549999833f, -0.7699996f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 758280, 924295337u, new Vector3(-688.938843f, -892.460632f, 23.3814926f), new Vector3(0f, -0f, 90.3137741f), 210186, 0, new Vector3(0.7699996f, 0.549999833f, -0.7699996f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 210186, 938137634u, new Vector3(-688.384644f, -893.2276f, 24.1514931f), new Vector3(0f, -0f, 90.3137741f), 156938, 0, new Vector3(1.51999891f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 210442, 924295337u, new Vector3(-688.9473f, -890.910645f, 23.3814926f), new Vector3(0f, -0f, 90.3137741f), 245770, 0, new Vector3(0.7699996f, 0.549999833f, -0.7699996f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 245770, 938137634u, new Vector3(-688.3931f, -891.6776f, 24.1514931f), new Vector3(0f, -0f, 90.3137741f), 210186, 0, new Vector3(1.54999888f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 246026, 924295337u, new Vector3(-688.9558f, -889.360657f, 23.3814926f), new Vector3(0f, -0f, 90.3137741f), 276490, 0, new Vector3(0.7699996f, 0.549999833f, -0.7699996f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 276490, 938137634u, new Vector3(-688.4016f, -890.1276f, 24.1514931f), new Vector3(0f, -0f, 90.3137741f), 210186, 0, new Vector3(3.09999752f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 276746, 924295337u, new Vector3(-688.9643f, -887.810669f, 23.3814926f), new Vector3(0f, -0f, 90.3137741f), 308234, 0, new Vector3(0.7699996f, 0.549999833f, -0.7699996f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 308234, 938137634u, new Vector3(-688.4101f, -888.577637f, 24.1514931f), new Vector3(0f, -0f, 90.3137741f), 210186, 0, new Vector3(4.65001154f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 308490, 924295337u, new Vector3(-688.9727f, -886.2707f, 23.3814926f), new Vector3(0f, -0f, 90.3137741f), 333834, 0, new Vector3(0.7699996f, 0.549999833f, -0.7699996f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 333834, 938137634u, new Vector3(-688.4185f, -887.037659f, 24.1514931f), new Vector3(0f, -0f, 90.3137741f), 210186, 0, new Vector3(6.190047f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 334090, 924295337u, new Vector3(-688.922241f, -895.500549f, 23.3814926f), new Vector3(0f, -0f, 90.3137741f), 156938, 0, new Vector3(-0.749999642f, 0.549999833f, -0.7699996f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_p_h_acc_artwalll_04"), 376586, 3462067242u, new Vector3(-688.7238f, -893.9489f, 26.3104477f), new Vector3(0f, -0f, 90.7724f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_p_h_acc_artwalll_03"), 392202, 3223345077u, new Vector3(-688.7519f, -890.9791f, 26.3165264f), new Vector3(0f, -0f, 90.7724f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_p_h_acc_artwalll_02"), 406538, 2915939088u, new Vector3(-688.7945f, -887.8194f, 26.3165264f), new Vector3(0f, -0f, 90.7724f), 392202, 0, new Vector3(3.15999913f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_temp_block_blocker"), 422922, 4143853297u, new Vector3(-670.3496f, -895.7766f, 18.8665676f), new Vector3(-90f, -0.5409485f, 0f), 782345, 0, new Vector3(3.809999f, 4.539999f, 1.30000019f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_temp_block_blocker"), 423178, 4143853297u, new Vector3(-677.3493f, -895.8427f, 18.8665676f), new Vector3(-90f, -0.5409485f, 0f), 782345, 0, new Vector3(-3.189999f, 4.539999f, 1.30000019f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_temp_block_blocker"), 81162, 4143853297u, new Vector3(-684.349f, -895.9088f, 18.8665676f), new Vector3(-90f, -0.5409485f, 0f), 782345, 0, new Vector3(-10.1900024f, 4.539999f, 1.30000019f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_temp_block_blocker"), 101130, 4143853297u, new Vector3(-680.7152f, -888.8742f, 18.86657f), new Vector3(-90f, -0.5409485f, 0f), 782345, 0, new Vector3(-6.489996f, 4.539999f, 8.299995f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_temp_block_blocker"), 424458, 4143853297u, new Vector3(-687.7149f, -888.9403f, 18.86657f), new Vector3(-90f, -0.5409485f, 0f), 782345, 0, new Vector3(-13.490015f, 4.539999f, 8.299995f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_temp_block_blocker"), 424714, 4143853297u, new Vector3(-691.2487f, -895.973938f, 18.8665676f), new Vector3(-90f, -0.5409485f, 0f), 782345, 0, new Vector3(-17.0900288f, 4.539999f, 1.30000019f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_console_02a"), 438794, 1896678985u, new Vector3(-688.3317f, -895.0219f, 24.2888641f), new Vector3(0f, -0f, 110.408684f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_grenade_01a"), 461066, 474251340u, new Vector3(-688.331665f, -894.528442f, 24.2951851f), new Vector3(0f, 0f, 68.4400253f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_gun_02a"), 480266, 176591061u, new Vector3(-679.1974f, -884.833069f, 24.5224419f), new Vector3(0f, 0f, 0.112476043f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_miniature_05a"), 495114, 558447676u, new Vector3(-688.372559f, -891.7019f, 24.2888641f), new Vector3(0f, -0f, 92.6462555f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_miniature_09a"), 495370, 1108211937u, new Vector3(-688.3278f, -891.9152f, 24.2888641f), new Vector3(0f, -0f, 105.646088f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_rocket_01a"), 510474, 3534536120u, new Vector3(-688.338f, -891.3773f, 24.2888641f), new Vector3(0f, 0f, 79.13395f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_skull_02b"), 525066, 947066522u, new Vector3(-688.406738f, -890.5065f, 24.2888641f), new Vector3(0f, -0f, 93.72007f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_skull_02a"), 565258, 1126181876u, new Vector3(-688.433f, -890.1344f, 24.2888641f), new Vector3(0f, -0f, 94.94784f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_skull_01b"), 565514, 1925085104u, new Vector3(-688.4425f, -889.7236f, 24.2888641f), new Vector3(0f, -0f, 92.95146f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_v_01b"), 554250, 4127071671u, new Vector3(-688.513062f, -888.5711f, 24.2888947f), new Vector3(0f, -0f, 91.77351f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_vase_10a"), 584970, 1977100271u, new Vector3(-688.3966f, -887.0098f, 24.2888756f), new Vector3(0f, -0f, 98.32441f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_panther_01b"), 605194, 3765710017u, new Vector3(-688.3749f, -893.248962f, 24.2922039f), new Vector3(0f, -0f, 125.296143f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> Loot = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01c"), 585479, 876512894u, new Vector3(-668.0669f, -889.0732f, 24.56203f), new Vector3(0f, 0f, 0.0991678759f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01e"), 600071, 397364576u, new Vector3(-669.8038f, -889.083435f, 24.5756512f), new Vector3(0f, 0f, -0.156740114f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01g"), 618503, 4261091832u, new Vector3(-671.453857f, -889.0932f, 24.5877037f), new Vector3(0f, 0f, -0.156740114f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01h"), 635655, 2992145076u, new Vector3(-673.174f, -889.1034f, 24.6042118f), new Vector3(0f, 0f, -1.94808912f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01i"), 649479, 3768180534u, new Vector3(-674.8253f, -889.1131f, 24.6597881f), new Vector3(0f, 0f, -1.94808912f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01b"), 420615, 645688058u, new Vector3(-672.9667f, -895.4022f, 24.6549416f), new Vector3(0f, -0f, -179.657074f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01g"), 691975, 2593687016u, new Vector3(-676.495361f, -892.419556f, 24.5698261f), new Vector3(0f, 0f, -88.06591f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01e"), 706567, 3221180597u, new Vector3(-676.1957f, -892.4075f, 24.6207485f), new Vector3(0f, -0f, 91.56917f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01d"), 168199, 158478566u, new Vector3(-669.8983f, -892.2227f, 24.6389713f), new Vector3(0f, 0f, -88.4047546f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01f"), 374534, 4020600141u, new Vector3(-669.599548f, -892.176941f, 24.5729637f), new Vector3(0f, -0f, 91.79529f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01f"), 119817, 3067067990u, new Vector3(-679.6933f, -895.4419f, 24.64751f), new Vector3(0f, -0f, 179.715729f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01d"), 134921, 3519083576u, new Vector3(-681.343445f, -895.45166f, 24.6327572f), new Vector3(0f, -0f, 179.715729f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01h"), 149257, 2325997055u, new Vector3(-685.5316f, -892.682861f, 24.63451f), new Vector3(0f, 0f, 89.22697f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01c"), 149513, 876512894u, new Vector3(-685.829956f, -892.7394f, 24.6561928f), new Vector3(0f, 0f, -87.29325f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01c"), 544777, 3259913511u, new Vector3(-682.6435f, -884.7381f, 24.5204353f), new Vector3(0f, 0f, -0.6826398f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01a"), 558601, 3967560110u, new Vector3(-680.9931f, -884.7169f, 24.5034313f), new Vector3(0f, 0f, -0.682635844f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01b"), 580105, 3704588885u, new Vector3(-686.064941f, -884.741f, 24.60232f), new Vector3(0f, 0f, 0.75044626f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01j"), 594697, 2174165298u, new Vector3(-687.714844f, -884.7609f, 24.6009369f), new Vector3(0f, 0f, 0.75044626f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<Prop> ArtFrames = new List<Prop>();

		public static List<ObjectPlacementMap> Guards = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Security Guard"), 484637, 3613962792u, new Vector3(-667.9178f, -889.836548f, 23.57144f), new Vector3(0f, -0f, 93.8567352f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Security Guard"), 561442, 3613962792u, new Vector3(-687.3556f, -885.710754f, 23.5719681f), new Vector3(0f, -0f, -133.7859f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> CCTV = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_07a"), 22794, 2954561821u, new Vector3(-688.405f, -895.3916f, 28.2588673f), new Vector3(0f, -0f, 111.846107f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_07a"), 48650, 2954561821u, new Vector3(-667.545837f, -895.140259f, 28.2788658f), new Vector3(0f, -0f, -105.664841f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_07a"), 72458, 2954561821u, new Vector3(-677.0628f, -889.8115f, 28.2788658f), new Vector3(0f, 0f, 4.93745756f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_07a"), 104202, 2954561821u, new Vector3(-677.730652f, -884.9829f, 28.278862f), new Vector3(0f, 0f, -16.968f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> MainDoors = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_bh1_44_door_01r"), 500230, 1596276849u, new Vector3(-666.8883f, -890.741f, 24.83519f), new Vector3(0f, -0f, 90.29577f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_bh1_44_door_01l"), 514822, 1523529669u, new Vector3(-666.873657f, -893.578f, 24.83519f), new Vector3(0f, -0f, 90.29577f), 500230, 0, new Vector3(-2.83699965f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> EnteranceMarker = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mk_money"), 669969, 1113676861u, new Vector3(-666.937f, -892.1536f, 23.8456631f), new Vector3(0f, 0f, -89.04651f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<Prop> Cameras = new List<Prop>();

		public static Blip MainBlip;

		public static Heiststage CurrentHeistStage;

		public static Vector3 Enterance_Inside;

		public static Vector3 Enterance_Outside;

		public static bool IsInInterior;

		public static bool ExitedStore;

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

		public TheGefangnisArtHeist()
		{
			base.Tick += OnTick;
		}

		public static float GetDistance(Vector3 _1, Vector3 _2)
		{
			return _1.DistanceTo(_2);
		}

		public unsafe static void ADD_DOOR_SYSTEM(Vector3 doorpos, int doorhash, string doorname)
		{
			int num = default(int);
			if (!Function.Call<bool>(Hash.DOOR_SYSTEM_FIND_EXISTING_DOOR, doorpos.X, doorpos.Y, doorpos.Z, (Hash)doorhash, &num))
			{
				num = Function.Call<int>(Hash.GET_HASH_KEY, doorname);
				Function.Call(Hash.ADD_DOOR_TO_SYSTEM, num, (Hash)doorhash, doorpos.X, doorpos.Y, doorpos.Z, true, false, false);
			}
			Function.Call<int>(Hash.DOOR_SYSTEM_GET_DOOR_STATE, num);
			int num2 = 1;
			if (!Function.Call<bool>(Hash.IS_DOOR_REGISTERED_WITH_SYSTEM, num))
			{
				return;
			}
			if (Function.Call<bool>(Hash.NETWORK_IS_DOOR_NETWORKED, num))
			{
				if (Function.Call<bool>(Hash.NETWORK_HAS_CONTROL_OF_DOOR, num))
				{
					Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, num, 1, true, false);
				}
			}
			else
			{
				Function.Call(Hash.DOOR_SYSTEM_SET_DOOR_STATE, num, 1, false, false);
			}
		}

		private void OnTick(object sender, EventArgs e)
		{
			if (!TheGefangnisArtHeistOn || !TheGefangnisArtHeistOn)
			{
				return;
			}
			if (IsInInterior)
			{
				Function.Call(Hash.SET_RADAR_ZOOM, 150);
			}
			else
			{
				Function.Call(Hash.SET_RADAR_ZOOM, 850);
			}
			if (MainDoors[0].Entity != null)
			{
				MainDoors[0].Entity.Position = new Vector3(MainDoors[0].Position.X, MainDoors[0].Position.Y, MainDoors[0].Position.Z);
				MainDoors[0].Entity.Rotation = new Vector3(MainDoors[0].Rotation.X, MainDoors[0].Rotation.Y, MainDoors[0].Entity.Rotation.Z);
			}
			if (MainDoors[1].Entity != null)
			{
				MainDoors[1].Entity.Position = new Vector3(MainDoors[1].Position.X, MainDoors[1].Position.Y, MainDoors[1].Position.Z);
				MainDoors[1].Entity.Rotation = new Vector3(MainDoors[1].Rotation.X, MainDoors[1].Rotation.Y, MainDoors[1].Entity.Rotation.Z);
			}
			SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Security;
			Heiststage currentHeistStage = CurrentHeistStage;
			UI.RenderQue = 0f;
			switch (currentHeistStage)
			{
			case Heiststage.HeadingToStore:
				if (GetDistance_VDIST2(Game.Player.Character.Position, Enterance_Inside, "<", 30f))
				{
					Screen.ShowSubtitle("Head over to ~y~The Gefangnis Art Store");
				}
				else
				{
					Screen.ShowSubtitle("Head over to ~y~The Gefangnis Art Store");
				}
				if (!GetDistance_VDIST2(Game.Player.Character.Position, Enterance_Inside, "<", 10f))
				{
					break;
				}
				if (MainBlip != null)
				{
					MainBlip.Alpha = 0;
				}
				foreach (ObjectPlacementMap item in Loot)
				{
					if (item.Entity != null)
					{
						ManageRobberyAnims.AddPropAsLoot((Prop)item.Entity);
					}
				}
				foreach (ObjectPlacementMap guard in Guards)
				{
					if (guard.Entity != null)
					{
						Ped ped = (Ped)guard.Entity;
						ped.IsPositionFrozen = false;
						SmartGuards.GuardGroup[13].GroupList.Add(new SmartGuards.SmartAiV2(ped, SmartGuards.EnemyGroup.Security));
					}
				}
				foreach (ObjectPlacementMap item2 in CCTV)
				{
					if (item2.Entity != null)
					{
						Prop prop = (Prop)item2.Entity;
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
				CurrentHeistStage = Heiststage.TakeOutCameras;
				IsInInterior = true;
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
				Screen.ShowSubtitle("Take out the ~r~Cameras and the Guards");
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
					CurrentHeistStage = Heiststage.LootStore;
				}
				break;
			}
			case Heiststage.LootStore:
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
				if (!ExitedStore)
				{
					List<Vector3> list = new List<Vector3>();
					list.Add(Enterance_Inside);
					list.Add(Enterance_Outside);
					float radius = 999f;
					Vector3 vector = default(Vector3);
					foreach (Vector3 item3 in list)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, item3, "<", radius))
						{
							radius = GetDistance(Game.Player.Character.Position, item3);
							if (item3 == Enterance_Inside)
							{
								vector = Enterance_Inside;
							}
							if (item3 == Enterance_Outside)
							{
								vector = Enterance_Outside;
							}
						}
					}
					if (vector == Enterance_Outside)
					{
						ExitedStore = true;
						drawText("Closest Enterance_Outside", 0.1f, 0.1f, 0.3f, 255, 255, 255);
						if (GetDistance_VDIST2(Game.Player.Character.Position, Enterance_Outside, "<", 200f))
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
							Game.Player.WantedLevel = 5;
							IsInInterior = false;
						}
					}
					if (vector == Enterance_Inside)
					{
						drawText("Closest Enterance_Inside", 0.1f, 0.1f, 0.3f, 255, 255, 255);
						Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
						Game.Player.WantedLevel = 0;
						IsInInterior = true;
					}
				}
				if (IsInInterior)
				{
					if (ManageRobberyAnims.LootLeft >= 0f)
					{
						Screen.ShowSubtitle("Keep looting ~g~Valuables~w~ or leave the ~y~The Gefangnis Art Store~w~");
					}
					else
					{
						Screen.ShowSubtitle("Leave the ~y~The Gefangnis Art Store~w~");
					}
				}
				else
				{
					Screen.ShowSubtitle("Escape the ~r~Cops");
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, Enterance_Inside, ">", 150f) && !IsInInterior && Game.Player.WantedLevel == 0)
				{
					Main.TotalCrewEarned += ManageRobberyAnims.CurrentPay;
					Main.TotalPlayerEarned += 0f;
					XP.CompleteHeist_TheMazebankSavingsBankHeist++;
					Main.SaveCuts();
					Class1.SetNoLongerInHeist();
					MissionScreen.SetPass("The Gefangnis Art Heist", ManageRobberyAnims.CurrentPay, 0, "The Player has died", 2, 0f, 5);
					int num = Game.GameTime + 30000;
					while (HKHModHelperNew.Class1.HeistPassedScreen != -1 && Game.GameTime <= num)
					{
						Script.Wait(0);
					}
					Script.Wait(2000);
					ManagePlayerFunds.AddFunds(ManageRobberyAnims.CurrentPay);
					XP.AddXP_Percent(XP.CompleteHeist_XPPercent_TheGefangnisArtHeist);
					EndHeist();
				}
				break;
			}
			if (!Game.Player.IsAlive)
			{
				MissionScreen.SetFail("The Gefangnis Art Heist", ManageRobberyAnims.CurrentPay, 0, "The Player has died", 2, 0f, 5);
				EndHeist();
			}
		}

		public static void EndHeist()
		{
			Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
			ExitedStore = false;
			if (MainBlip != null)
			{
				MainBlip.Delete();
			}
			Function.Call(Hash.SET_RADAR_ZOOM, 850);
			CurrentPay = 0;
			LocalTake = 0;
			ManageRobberyAnims.CurrentPay = 0;
			ManageRobberyAnims.DeleteLoot();
			CurrentHeistStage = Heiststage.None;
			IsInInterior = false;
			SmartGuards.ClearGenericGuards();
			TheGefangnisArtHeistOn = false;
			if (MainBlip != null)
			{
				MainBlip.Delete();
			}
			foreach (Prop artFrame in ArtFrames)
			{
				if (artFrame != null)
				{
					artFrame.Delete();
				}
			}
			foreach (ObjectPlacementMap item in Interior)
			{
				if (item.Entity != null)
				{
					item.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item2 in Loot)
			{
				if (item2.Entity != null)
				{
					item2.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap guard in Guards)
			{
				if (guard.Entity != null)
				{
					guard.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item3 in CCTV)
			{
				if (item3.Entity != null)
				{
					item3.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap mainDoor in MainDoors)
			{
				if (mainDoor.Entity != null)
				{
					mainDoor.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item4 in EnteranceMarker)
			{
				if (item4.Entity != null)
				{
					item4.Entity.Delete();
				}
			}
		}

		public static void StartHeist()
		{
			ManageRobberyAnims.CurrentPay = 0;
			ManageRobberyAnims.Heist_Average_Pay = (float)(HeistMinPay + HeistMaxPay) / 2f * 1.25f;
			Function.Call(Hash.BEGIN_TEXT_COMMAND_BUSYSPINNER_ON, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "Loading The Gefangnis Art Heist, Please Wait");
			Function.Call(Hash.END_TEXT_COMMAND_BUSYSPINNER_ON, 5);
			EndHeist();
			foreach (ObjectPlacementMap item in Interior)
			{
				item.CreateObjatCoords();
			}
			foreach (ObjectPlacementMap item2 in Loot)
			{
				item2.CreateObjatCoords();
			}
			foreach (ObjectPlacementMap guard in Guards)
			{
				guard.CreateObjatCoords();
			}
			foreach (ObjectPlacementMap item3 in CCTV)
			{
				item3.CreateObjatCoords();
			}
			foreach (ObjectPlacementMap mainDoor in MainDoors)
			{
				mainDoor.CreateObjatCoords();
			}
			foreach (ObjectPlacementMap item4 in EnteranceMarker)
			{
				item4.CreateObjatCoords();
			}
			Enterance_Inside = EnteranceMarker[0].Entity.GetOffsetPosition(new Vector3(0f, -5f, 0f));
			Enterance_Outside = EnteranceMarker[0].Entity.GetOffsetPosition(new Vector3(0f, 5f, 0f));
			EnteranceMarker[0].Entity.Opacity = 0;
			foreach (ObjectPlacementMap item5 in Loot)
			{
				if (item5.Entity != null)
				{
					Prop prop = GlobalHelper.CreateProp(RequestModel("h4_prop_office_painting_01a"), new Vector3(0f, 0f, 0f), false, false);
					prop.AttachTo(item5.Entity);
					ArtFrames.Add(prop);
				}
			}
			CurrentHeistStage = Heiststage.HeadingToStore;
			TheGefangnisArtHeistOn = true;
			foreach (ObjectPlacementMap mainDoor2 in MainDoors)
			{
				if (mainDoor2.Entity != null)
				{
					mainDoor2.Entity.IsPositionFrozen = false;
				}
			}
			if (MainBlip != null)
			{
				MainBlip.Delete();
			}
			MainBlip = GlobalHelper.CreateBlip(EnteranceMarker[0].Entity.Position);
			MainBlip.Color = BlipColor.Yellow2;
			MainBlip.IsShortRange = true;
			MainBlip.Name = "Enter the Gefangnis Art Store";
			Function.Call(Hash.BUSYSPINNER_OFF);
		}
	}
}
