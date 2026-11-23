using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;

namespace SinglePlayerHeists.UtilsMisc
{
	internal class ManageCompleteHeistProps : Script
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

		public static List<ObjectPlacementMap> CompleteHeist_ThePrisonBreak = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_hei_pic_pb_break"), 158038, 630003835u, new Vector3(2709.056f, -358.331573f, -55.3372879f), new Vector3(-90f, -0.319549382f, 0f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_hei_pic_pb_bus"), 157270, 3097750313u, new Vector3(2709.25464f, -358.120453f, -55.32729f), new Vector3(-90f, 24.6804543f, 0f), 158038, 0, new Vector3(0.200000048f, -0.0100000016f, 0.210000038f), new Vector3(0f, 25.0000019f, 0f))
		};

		public static List<ObjectPlacementMap> CompleteHeist_CayoPerico = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "w_pi_singleshoth4"), 132694, 3178485894u, new Vector3(2707.73047f, -358.576538f, -55.3355751f), new Vector3(-90f, -2.6049006f, 0f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_diamond_disp_01a"), 173411, 167446900u, new Vector3(2715.59546f, -355.9432f, -55.3472977f), new Vector3(0f, -0f, -96.75598f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_diamond_01a"), 316987, 677732008u, new Vector3(2715.59546f, -355.9432f, -55.3472977f), new Vector3(0f, -0f, -96.75598f), 173411, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_neck_disp_01a"), 548464, 3466777742u, new Vector3(2715.51245f, -355.643768f, -55.34729f), new Vector3(0f, -0f, -92.14843f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_necklace_01a"), 708208, 2472404644u, new Vector3(2715.51245f, -355.643768f, -55.34729f), new Vector3(0f, -0f, -92.14843f), 548464, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_art_pant_01a"), 763477, 879836258u, new Vector3(2715.56763f, -356.174072f, -55.3472977f), new Vector3(0f, -0f, -112.034477f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> CompleteHeist_TheDiamondCasino = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_miniature_09a"), 583799, 1108211937u, new Vector3(2707.749f, -358.331238f, -55.3472977f), new Vector3(0f, 0f, 5.522217f), 681221, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_miniature_09b"), 766293, 273192279u, new Vector3(2707.749f, -358.331238f, -55.3472977f), new Vector3(0f, 0f, 5.522217f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_miniature_09c"), 84492, 1106835635u, new Vector3(2707.749f, -358.331238f, -55.3472977f), new Vector3(0f, 0f, 5.522217f), 583799, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> CompleteHeist_TheBillionaresParty = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_car_04a"), 136278, 187047509u, new Vector3(2708.01f, -358.4553f, -55.3472977f), new Vector3(0f, -0f, 90.02152f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_car_05a"), 234070, 3933166564u, new Vector3(2708.21f, -358.455231f, -55.3472977f), new Vector3(0f, -0f, 90.02152f), 136278, 0, new Vector3(0f, -0.199999988f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_car_06a"), 783445, 2542941451u, new Vector3(2708.41f, -358.45517f, -55.3472977f), new Vector3(0f, -0f, 90.02152f), 234070, 0, new Vector3(0f, -0.199999988f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_car_08a"), 770645, 2376509436u, new Vector3(2708.60986f, -358.4551f, -55.3472977f), new Vector3(0f, -0f, 90.02152f), 783445, 0, new Vector3(0f, -0.199999988f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> CompleteHeist_LaoubPrincessCargoship = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_panther_01a"), 766488, 842321989u, new Vector3(2715.0896f, -354.4653f, -55.343956f), new Vector3(-0f, -0f, -81.84448f), 581482, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_panther_01b"), 162646, 3765710017u, new Vector3(2715.0896f, -354.4653f, -55.343956f), new Vector3(0f, 0f, -81.84448f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_panther_01c"), 188486, 381753694u, new Vector3(2715.0896f, -354.4653f, -55.343956f), new Vector3(-0f, -0f, -81.84448f), 581482, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> CompleteHeist_TheCartelYachtHeist = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_miniature_yacht_01a"), 741717, 2597105484u, new Vector3(2708.27222f, -358.205658f, -55.3472977f), new Vector3(0f, 0f, 0.7617687f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_miniature_yacht_01b"), 773410, 3946074138u, new Vector3(2708.38647f, -358.066467f, -55.3472977f), new Vector3(0f, 0f, -0.0484910272f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_miniature_yacht_01c"), 272918, 4250530917u, new Vector3(2708.59985f, -357.9135f, -55.3472977f), new Vector3(0f, 0f, 0.4122017f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> CompleteHeist_TheHumaineLabsRaid = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_chem_vial_02"), 202582, 3964191746u, new Vector3(2707.804f, -357.954651f, -54.96333f), new Vector3(0f, 0f, 15.4596767f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_pic_10"), 246870, 3881580921u, new Vector3(2715.70947f, -355.348328f, -55.1872978f), new Vector3(0f, 0f, -89.65215f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> CompleteHeist_PacificStandardBank = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_hei_bust_01"), 726869, 3826822617u, new Vector3(2714.90527f, -356.897949f, -55.749157f), new Vector3(0f, 0f, -89.40237f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_hei_pic_ps_job"), 779349, 3351394425u, new Vector3(2708.3313f, -357.668335f, -55.12132f), new Vector3(0f, 0f, -0.0352958627f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> CompleteHeist_TheJewelStoreJob = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_j_heist_pic_04"), 133974, 3644118571u, new Vector3(2715.76367f, -354.856842f, -55.1898537f), new Vector3(0f, -0f, -90.3672943f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 262998, 2852049058u, new Vector3(2710.13843f, -357.396057f, -55.5983772f), new Vector3(0f, -0f, 90.39657f), 765013, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 771413, 1870138714u, new Vector3(2710.13843f, -357.396057f, -55.5983772f), new Vector3(0f, -0f, 90.39657f), 765013, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_03"), 765013, 2258924952u, new Vector3(2710.13843f, -357.396057f, -55.5983772f), new Vector3(0f, -0f, 90.39657f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_office_swag_jewelwatch"), 168058, 3553285577u, new Vector3(2715.439f, -358.144775f, -55.1189f), new Vector3(0f, 0f, -86.515976f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_office_swag_jewelwatch3"), 679034, 3629902606u, new Vector3(2715.72339f, -358.498566f, -55.1098137f), new Vector3(0f, 0f, -84.05864f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> CompleteHeist_Fleeca = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_corp_fleeca_display"), 153686, 3480755074u, new Vector3(2715.21216f, -354.818085f, -55.3471069f), new Vector3(0f, 0f, -74.66229f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_hei_pic_ub_prep02"), 764757, 2215433010u, new Vector3(2708.887f, -357.668335f, -55.1212959f), new Vector3(0f, 0f, -0.08650074f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> CompleteHeist_LaFeuntaBlanca = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_res_m_horsefig"), 140886, 3050641529u, new Vector3(2709.1958f, -357.848236f, -55.3472977f), new Vector3(0f, 0f, 1.64963818f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> CompleteHeist_TheECUJob = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_control_unit_01a"), 769365, 92049373u, new Vector3(2714.949f, -354.894562f, -55.3174477f), new Vector3(0f, 0f, -16.6729679f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 318305, 2271741578u, new Vector3(2715.32617f, -355.083466f, -55.2615967f), new Vector3(0f, 0f, -63.1927834f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_clipboard_ta_01a"), 50231, 1439772435u, new Vector3(2715.76367f, -355.142181f, -54.90512f), new Vector3(90f, -91.13553f, 0f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> CompleteHeist_TheBikerClubhouseHeist = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "w_me_knuckle"), 56153, 3005998612u, new Vector3(2714.94116f, -355.206757f, -55.3819237f), new Vector3(2.63873076f, -81.52331f, 97.1717f), 239181, 0, new Vector3(0f, 0.100000016f, 0f), new Vector3(129.999985f, 10.000001f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "w_me_knuckle_dlr"), 575833, 1548129954u, new Vector3(2714.88257f, -355.168823f, -55.37826f), new Vector3(-0.539982855f, -86.9998f, -32.8489647f), 239181, 0, new Vector3(0f, 0.100000016f, 0.07000003f), new Vector3(0f, 0f, 0.0100000016f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "w_me_knuckle_dmd"), 239181, 1902493692u, new Vector3(2714.887f, -355.290771f, -55.381f), new Vector3(-0.529996634f, -86.99981f, -32.84949f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> CompleteHeist_TheFederalReserveTrainHeist = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_miniature_05c"), 146518, 1194952728u, new Vector3(2715.08228f, -355.505066f, -55.3472977f), new Vector3(0f, 0f, -89.6689148f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> CompleteHeist_TheUnionDepository = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_gold_stack_01a"), 96609, 4114893066u, new Vector3(2715.08936f, -356.1227f, -55.34484f), new Vector3(0f, -0f, -91.5923157f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_gold_stack_01a"), 775520, 4114893066u, new Vector3(2715.092f, -356.022736f, -55.35484f), new Vector3(0f, -0f, -91.5923157f), 96609, 0, new Vector3(-0.100000024f, 0f, -0.0100000016f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_gold_stack_01a"), 755808, 4114893066u, new Vector3(2715.09351f, -355.932739f, -55.35484f), new Vector3(0f, -0f, -91.5923157f), 775520, 0, new Vector3(-0.09000002f, -0.00100000016f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> CompleteHeist_Paleto = new List<ObjectPlacementMap>();

		public static List<ObjectPlacementMap> CompleteHeist_TheLexigntonRaid = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_ser_storage_01a"), 692836, 1413173041u, new Vector3(2709.552f, -358.397247f, -55.3472977f), new Vector3(0f, 0f, 16.6620121f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cs_server_drive"), 753974, 977288393u, new Vector3(2709.38672f, -358.52298f, -55.3311539f), new Vector3(0f, 0f, 29.6108017f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_carry_box_01a"), 709916, 3969704473u, new Vector3(2709.589f, -358.11673f, -55.3453064f), new Vector3(0f, 0f, 22.5213f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static bool CreatedCashProps;

		public static void ATTACH_ENTITY_TO_ENTITY(Entity entity1, Entity entity2, int boneIndex, float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, bool p9, bool useSoftPinning, bool collision, bool isPed, int vertexIndex, bool fixedRot)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, entity1, entity2, boneIndex, xPos, yPos, zPos, xRot, yRot, zRot, p9, useSoftPinning, collision, isPed, vertexIndex, fixedRot);
		}

		public static void Arcade_UpdateCompleteHeistProps()
		{
			foreach (ObjectPlacementMap item in CompleteHeist_ThePrisonBreak)
			{
				if (item.Entity != null)
				{
					item.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item2 in CompleteHeist_CayoPerico)
			{
				if (item2.Entity != null)
				{
					item2.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item3 in CompleteHeist_TheDiamondCasino)
			{
				if (item3.Entity != null)
				{
					item3.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item4 in CompleteHeist_TheBillionaresParty)
			{
				if (item4.Entity != null)
				{
					item4.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item5 in CompleteHeist_LaoubPrincessCargoship)
			{
				if (item5.Entity != null)
				{
					item5.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item6 in CompleteHeist_TheCartelYachtHeist)
			{
				if (item6.Entity != null)
				{
					item6.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item7 in CompleteHeist_TheHumaineLabsRaid)
			{
				if (item7.Entity != null)
				{
					item7.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item8 in CompleteHeist_PacificStandardBank)
			{
				if (item8.Entity != null)
				{
					item8.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item9 in CompleteHeist_TheJewelStoreJob)
			{
				if (item9.Entity != null)
				{
					item9.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item10 in CompleteHeist_Fleeca)
			{
				if (item10.Entity != null)
				{
					item10.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item11 in CompleteHeist_LaFeuntaBlanca)
			{
				if (item11.Entity != null)
				{
					item11.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item12 in CompleteHeist_TheECUJob)
			{
				if (item12.Entity != null)
				{
					item12.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item13 in CompleteHeist_TheBikerClubhouseHeist)
			{
				if (item13.Entity != null)
				{
					item13.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item14 in CompleteHeist_TheFederalReserveTrainHeist)
			{
				if (item14.Entity != null)
				{
					item14.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item15 in CompleteHeist_TheUnionDepository)
			{
				if (item15.Entity != null)
				{
					item15.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item16 in CompleteHeist_Paleto)
			{
				if (item16.Entity != null)
				{
					item16.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item17 in CompleteHeist_TheLexigntonRaid)
			{
				if (item17.Entity != null)
				{
					item17.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item18 in CompleteHeist_ThePrisonBreak)
			{
				item18.CreateObjatCoords();
				if (item18.Entity != null)
				{
					item18.Entity.Opacity = 0;
				}
			}
			foreach (ObjectPlacementMap item19 in CompleteHeist_CayoPerico)
			{
				item19.CreateObjatCoords();
				if (item19.Entity != null)
				{
					item19.Entity.Opacity = 0;
				}
			}
			foreach (ObjectPlacementMap item20 in CompleteHeist_TheDiamondCasino)
			{
				item20.CreateObjatCoords();
				if (item20.Entity != null)
				{
					item20.Entity.Opacity = 0;
				}
			}
			foreach (ObjectPlacementMap item21 in CompleteHeist_TheBillionaresParty)
			{
				item21.CreateObjatCoords();
				if (item21.Entity != null)
				{
					item21.Entity.Opacity = 0;
				}
			}
			foreach (ObjectPlacementMap item22 in CompleteHeist_LaoubPrincessCargoship)
			{
				item22.CreateObjatCoords();
				if (item22.Entity != null)
				{
					item22.Entity.Opacity = 0;
				}
			}
			foreach (ObjectPlacementMap item23 in CompleteHeist_TheCartelYachtHeist)
			{
				item23.CreateObjatCoords();
				if (item23.Entity != null)
				{
					item23.Entity.Opacity = 0;
				}
			}
			foreach (ObjectPlacementMap item24 in CompleteHeist_TheHumaineLabsRaid)
			{
				item24.CreateObjatCoords();
				if (item24.Entity != null)
				{
					item24.Entity.Opacity = 0;
				}
			}
			foreach (ObjectPlacementMap item25 in CompleteHeist_PacificStandardBank)
			{
				item25.CreateObjatCoords();
				if (item25.Entity != null)
				{
					item25.Entity.Opacity = 0;
				}
			}
			foreach (ObjectPlacementMap item26 in CompleteHeist_TheJewelStoreJob)
			{
				item26.CreateObjatCoords();
				if (item26.Entity != null)
				{
					item26.Entity.Opacity = 0;
				}
			}
			foreach (ObjectPlacementMap item27 in CompleteHeist_Fleeca)
			{
				item27.CreateObjatCoords();
				if (item27.Entity != null)
				{
					item27.Entity.Opacity = 0;
				}
			}
			foreach (ObjectPlacementMap item28 in CompleteHeist_LaFeuntaBlanca)
			{
				item28.CreateObjatCoords();
				if (item28.Entity != null)
				{
					item28.Entity.Opacity = 0;
				}
			}
			foreach (ObjectPlacementMap item29 in CompleteHeist_TheECUJob)
			{
				item29.CreateObjatCoords();
				if (item29.Entity != null)
				{
					item29.Entity.Opacity = 0;
				}
			}
			foreach (ObjectPlacementMap item30 in CompleteHeist_TheBikerClubhouseHeist)
			{
				item30.CreateObjatCoords();
				if (item30.Entity != null)
				{
					item30.Entity.Opacity = 0;
				}
			}
			foreach (ObjectPlacementMap item31 in CompleteHeist_TheFederalReserveTrainHeist)
			{
				item31.CreateObjatCoords();
				if (item31.Entity != null)
				{
					item31.Entity.Opacity = 0;
				}
			}
			foreach (ObjectPlacementMap item32 in CompleteHeist_TheUnionDepository)
			{
				item32.CreateObjatCoords();
				if (item32.Entity != null)
				{
					item32.Entity.Opacity = 0;
				}
			}
			foreach (ObjectPlacementMap item33 in CompleteHeist_Paleto)
			{
				item33.CreateObjatCoords();
				if (item33.Entity != null)
				{
					item33.Entity.Opacity = 0;
				}
			}
			foreach (ObjectPlacementMap item34 in CompleteHeist_TheLexigntonRaid)
			{
				item34.CreateObjatCoords();
				if (item34.Entity != null)
				{
					item34.Entity.Opacity = 0;
				}
			}
			if (XP.CompleteHeist_PrisonBreak >= 1)
			{
				if (XP.CompleteHeist_PrisonBreak >= 1 && CompleteHeist_ThePrisonBreak[0].Entity != null)
				{
					CompleteHeist_ThePrisonBreak[0].Entity.Opacity = 255;
				}
				if (XP.CompleteHeist_PrisonBreak >= 2 && CompleteHeist_ThePrisonBreak[1].Entity != null)
				{
					CompleteHeist_ThePrisonBreak[1].Entity.Opacity = 255;
				}
			}
			if (XP.CompleteHeist_CayoPerico >= 1)
			{
				if (XP.CompleteHeist_CayoPerico >= 1 && CompleteHeist_CayoPerico[0].Entity != null)
				{
					CompleteHeist_CayoPerico[0].Entity.Opacity = 255;
				}
				if (XP.CompleteHeist_CayoPerico >= 2)
				{
					if (CompleteHeist_CayoPerico[1].Entity != null)
					{
						CompleteHeist_CayoPerico[1].Entity.Opacity = 255;
					}
					if (CompleteHeist_CayoPerico[2].Entity != null)
					{
						CompleteHeist_CayoPerico[2].Entity.Opacity = 255;
					}
				}
				if (XP.CompleteHeist_CayoPerico >= 3)
				{
					if (CompleteHeist_CayoPerico[3].Entity != null)
					{
						CompleteHeist_CayoPerico[3].Entity.Opacity = 255;
					}
					if (CompleteHeist_CayoPerico[4].Entity != null)
					{
						CompleteHeist_CayoPerico[4].Entity.Opacity = 255;
					}
				}
				if (XP.CompleteHeist_CayoPerico >= 4 && CompleteHeist_CayoPerico[5].Entity != null)
				{
					CompleteHeist_CayoPerico[5].Entity.Opacity = 255;
				}
			}
			if (XP.CompleteHeist_TheDiamondCasino >= 1)
			{
				if (XP.CompleteHeist_TheDiamondCasino >= 1 && CompleteHeist_TheDiamondCasino[0].Entity != null)
				{
					CompleteHeist_TheDiamondCasino[0].Entity.Opacity = 255;
				}
				if (XP.CompleteHeist_TheDiamondCasino == 2 && CompleteHeist_TheDiamondCasino[1].Entity != null)
				{
					CompleteHeist_TheDiamondCasino[1].Entity.Opacity = 255;
				}
				if (XP.CompleteHeist_TheDiamondCasino >= 3 && CompleteHeist_TheDiamondCasino[2].Entity != null)
				{
					CompleteHeist_TheDiamondCasino[2].Entity.Opacity = 255;
				}
			}
			if (XP.CompleteHeist_TheBillionariesParty >= 1)
			{
				if (XP.CompleteHeist_TheBillionariesParty >= 1 && CompleteHeist_TheBillionaresParty[0].Entity != null)
				{
					CompleteHeist_TheBillionaresParty[0].Entity.Opacity = 255;
				}
				if (XP.CompleteHeist_TheBillionariesParty >= 2 && CompleteHeist_TheBillionaresParty[1].Entity != null)
				{
					CompleteHeist_TheBillionaresParty[1].Entity.Opacity = 255;
				}
				if (XP.CompleteHeist_TheBillionariesParty >= 3 && CompleteHeist_TheBillionaresParty[2].Entity != null)
				{
					CompleteHeist_TheBillionaresParty[2].Entity.Opacity = 255;
				}
				if (XP.CompleteHeist_TheBillionariesParty >= 4 && CompleteHeist_TheBillionaresParty[3].Entity != null)
				{
					CompleteHeist_TheBillionaresParty[3].Entity.Opacity = 255;
				}
			}
			if (XP.CompleteHeist_LaoubPrincessCargoship >= 1)
			{
				if (XP.CompleteHeist_LaoubPrincessCargoship == 1 && CompleteHeist_LaoubPrincessCargoship[0].Entity != null)
				{
					CompleteHeist_LaoubPrincessCargoship[0].Entity.Opacity = 255;
				}
				if (XP.CompleteHeist_LaoubPrincessCargoship == 2 && CompleteHeist_LaoubPrincessCargoship[1].Entity != null)
				{
					CompleteHeist_LaoubPrincessCargoship[1].Entity.Opacity = 255;
				}
				if (XP.CompleteHeist_LaoubPrincessCargoship >= 3 && CompleteHeist_LaoubPrincessCargoship[2].Entity != null)
				{
					CompleteHeist_LaoubPrincessCargoship[2].Entity.Opacity = 255;
				}
			}
			if (XP.CompleteHeist_TheCartelYacht >= 1)
			{
				if (XP.CompleteHeist_TheCartelYacht >= 1 && CompleteHeist_TheCartelYachtHeist[0].Entity != null)
				{
					CompleteHeist_TheCartelYachtHeist[0].Entity.Opacity = 255;
				}
				if (XP.CompleteHeist_TheCartelYacht >= 2 && CompleteHeist_TheCartelYachtHeist[1].Entity != null)
				{
					CompleteHeist_TheCartelYachtHeist[1].Entity.Opacity = 255;
				}
				if (XP.CompleteHeist_TheCartelYacht >= 3 && CompleteHeist_TheCartelYachtHeist[2].Entity != null)
				{
					CompleteHeist_TheCartelYachtHeist[2].Entity.Opacity = 255;
				}
			}
			if (XP.CompleteHeist_HumaineLabsRaid >= 1)
			{
				if (XP.CompleteHeist_HumaineLabsRaid >= 1 && CompleteHeist_TheHumaineLabsRaid[0].Entity != null)
				{
					CompleteHeist_TheHumaineLabsRaid[0].Entity.Opacity = 255;
				}
				if (XP.CompleteHeist_HumaineLabsRaid >= 2 && CompleteHeist_TheHumaineLabsRaid[1].Entity != null)
				{
					CompleteHeist_TheHumaineLabsRaid[1].Entity.Opacity = 255;
				}
			}
			if (XP.CompleteHeist_PacStandard >= 1)
			{
				if (XP.CompleteHeist_PacStandard >= 1 && CompleteHeist_PacificStandardBank[0].Entity != null)
				{
					CompleteHeist_PacificStandardBank[0].Entity.Opacity = 255;
				}
				if (XP.CompleteHeist_PacStandard >= 2 && CompleteHeist_PacificStandardBank[1].Entity != null)
				{
					CompleteHeist_PacificStandardBank[1].Entity.Opacity = 255;
				}
			}
			if (XP.CompleteHeist_JewelStore >= 1)
			{
				if (XP.CompleteHeist_JewelStore >= 1 && CompleteHeist_TheJewelStoreJob[0].Entity != null)
				{
					CompleteHeist_TheJewelStoreJob[0].Entity.Opacity = 255;
				}
				if (XP.CompleteHeist_JewelStore >= 2)
				{
					if (CompleteHeist_TheJewelStoreJob[1].Entity != null)
					{
						CompleteHeist_TheJewelStoreJob[1].Entity.Opacity = 255;
					}
					if (CompleteHeist_TheJewelStoreJob[2].Entity != null)
					{
						CompleteHeist_TheJewelStoreJob[2].Entity.Opacity = 255;
					}
					if (CompleteHeist_TheJewelStoreJob[3].Entity != null)
					{
						CompleteHeist_TheJewelStoreJob[3].Entity.Opacity = 255;
					}
				}
				if (XP.CompleteHeist_JewelStore >= 3)
				{
					if (CompleteHeist_TheJewelStoreJob[4].Entity != null)
					{
						CompleteHeist_TheJewelStoreJob[4].Entity.Opacity = 255;
					}
					if (CompleteHeist_TheJewelStoreJob[5].Entity != null)
					{
						CompleteHeist_TheJewelStoreJob[5].Entity.Opacity = 255;
					}
				}
			}
			if (XP.CompleteHeist_Fleeca >= 1)
			{
				if (XP.CompleteHeist_Fleeca >= 1 && CompleteHeist_Fleeca[0].Entity != null)
				{
					CompleteHeist_Fleeca[0].Entity.Opacity = 255;
				}
				if (XP.CompleteHeist_Fleeca >= 2 && CompleteHeist_Fleeca[1].Entity != null)
				{
					CompleteHeist_Fleeca[1].Entity.Opacity = 255;
				}
			}
			if (XP.CompleteHeist_LaFuentaBlanca >= 1 && CompleteHeist_LaFeuntaBlanca[0].Entity != null)
			{
				CompleteHeist_LaFeuntaBlanca[0].Entity.Opacity = 255;
			}
			if (XP.CompleteHeist_TheECUJob >= 1)
			{
				if (XP.CompleteHeist_TheECUJob >= 1 && CompleteHeist_TheECUJob[0].Entity != null)
				{
					CompleteHeist_TheECUJob[0].Entity.Opacity = 255;
				}
				if (XP.CompleteHeist_TheECUJob >= 2 && CompleteHeist_TheECUJob[1].Entity != null)
				{
					CompleteHeist_TheECUJob[1].Entity.Opacity = 255;
				}
				if (XP.CompleteHeist_TheECUJob >= 3 && CompleteHeist_TheECUJob[1].Entity != null)
				{
					CompleteHeist_TheECUJob[2].Entity.Opacity = 255;
				}
			}
			if (XP.CompleteHeist_BikerClubhouse >= 1)
			{
				if (XP.CompleteHeist_BikerClubhouse >= 1 && CompleteHeist_TheBikerClubhouseHeist[0].Entity != null)
				{
					CompleteHeist_TheBikerClubhouseHeist[0].Entity.Opacity = 255;
				}
				if (XP.CompleteHeist_BikerClubhouse >= 2 && CompleteHeist_TheBikerClubhouseHeist[1].Entity != null)
				{
					CompleteHeist_TheBikerClubhouseHeist[1].Entity.Opacity = 255;
				}
				if (XP.CompleteHeist_BikerClubhouse >= 3 && CompleteHeist_TheBikerClubhouseHeist[2].Entity != null)
				{
					CompleteHeist_TheBikerClubhouseHeist[2].Entity.Opacity = 255;
				}
			}
			if (XP.CompleteHeist_FederalReserveTrain >= 1 && CompleteHeist_TheFederalReserveTrainHeist[0].Entity != null)
			{
				CompleteHeist_TheFederalReserveTrainHeist[0].Entity.Opacity = 255;
			}
			if (XP.CompleteHeist_UnionDepository >= 1)
			{
				if (XP.CompleteHeist_UnionDepository >= 1 && CompleteHeist_TheUnionDepository[0].Entity != null)
				{
					CompleteHeist_TheUnionDepository[0].Entity.Opacity = 255;
				}
				if (XP.CompleteHeist_UnionDepository >= 2 && CompleteHeist_TheUnionDepository[1].Entity != null)
				{
					CompleteHeist_TheUnionDepository[1].Entity.Opacity = 255;
				}
				if (XP.CompleteHeist_UnionDepository >= 3 && CompleteHeist_TheUnionDepository[2].Entity != null)
				{
					CompleteHeist_TheUnionDepository[2].Entity.Opacity = 255;
				}
			}
			if (XP.CompleteHeist_TheLexingtonRaid >= 1)
			{
				if (XP.CompleteHeist_TheLexingtonRaid >= 1 && CompleteHeist_TheLexigntonRaid[0].Entity != null)
				{
					CompleteHeist_TheLexigntonRaid[0].Entity.Opacity = 255;
				}
				if (XP.CompleteHeist_TheLexingtonRaid >= 2 && CompleteHeist_TheLexigntonRaid[1].Entity != null)
				{
					CompleteHeist_TheLexigntonRaid[1].Entity.Opacity = 255;
				}
				if (XP.CompleteHeist_TheLexingtonRaid >= 3 && CompleteHeist_TheLexigntonRaid[2].Entity != null)
				{
					CompleteHeist_TheLexigntonRaid[2].Entity.Opacity = 255;
				}
			}
		}

		public ManageCompleteHeistProps()
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
			if (Main.IsInArcade)
			{
				if (!CreatedCashProps)
				{
					CreatedCashProps = true;
					Arcade_UpdateCompleteHeistProps();
				}
			}
			else
			{
				CreatedCashProps = false;
			}
		}
	}
}
