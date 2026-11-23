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
	internal class TheSilthouseJob : Script
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

		public enum HeistStage
		{
			EnterSilthouse,
			LootTheHouse,
			EscapeTheCops
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

		public static int HeistMinPay = 90000;

		public static int HeistMaxPay = 212000;

		public static Vector3 Enterance;

		public static Vector3 Exit;

		public static List<MissionOjects> Silthouses = new List<MissionOjects>
		{
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mk_money"), 278331, 1113676861u, new Vector3(-733.149f, 593.7691f, 141.761826f), new Vector3(0f, -0f, 156.92955f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mk_money"), 613697, 1113676861u, new Vector3(-739.999451f, 596.4164f, 145.539383f), new Vector3(0f, 0f, -29.819191f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_fingerprint_scanner_01e"), 45921, 996668613u, new Vector3(-740.441345f, 599.414f, 139.194611f), new Vector3(0f, 0f, -31.73512f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_fingerprint_scanner_01e"), 653407, 996668613u, new Vector3(-742.410156f, 580.7317f, 142.932037f), new Vector3(0f, 0f, -28.9693584f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 304, 3294791577u, new Vector3(-743.815247f, 581.727234f, 141.46051f), new Vector3(0f, 0f, -28.1471958f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 163611, 3294791577u, new Vector3(-739.138f, 598.7128f, 137.660385f), new Vector3(0f, 0f, -29.33169f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_door_01a"), 382326, 2244758654u, new Vector3(-739.9596f, 598.945068f, 138.870392f), new Vector3(-0f, -0f, -29.33169f), 163611, 0, new Vector3(-0.829999566f, -0.200000063f, 1.2099992f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_door_01a"), 273409, 2244758654u, new Vector3(-744.6833f, 581.885437f, 142.6405f), new Vector3(-0f, -0f, -28.1471958f), 304, 0, new Vector3(-0.839999557f, -0.27000007f, 1.17999923f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 314465, 938137634u, new Vector3(-743.614258f, 591.889648f, 145.853531f), new Vector3(0f, 0f, 61.35793f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab4_start"), 738922, 2414797517u, new Vector3(-743.614258f, 591.889648f, 145.853531f), new Vector3(-0f, -0f, -28.642065f), 314465, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, -90.00001f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 569469, 1870138714u, new Vector3(-743.614258f, 591.889648f, 145.853531f), new Vector3(0f, 0f, 61.35793f), 576372, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 576372, 2852049058u, new Vector3(-743.614258f, 591.889648f, 145.853531f), new Vector3(0f, 0f, 61.35793f), 314465, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 686093, 2852049058u, new Vector3(-751.100037f, 577.9751f, 145.453537f), new Vector3(0f, 0f, 61.35793f), 770319, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 770319, 938137634u, new Vector3(-751.100037f, 577.9751f, 145.453537f), new Vector3(0f, 0f, 61.35793f), 314465, 0, new Vector3(-15.8000183f, -0.100000016f, -0.400000066f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab_start"), 707867, 37228785u, new Vector3(-751.100037f, 577.9751f, 145.453537f), new Vector3(0f, 0f, 61.35793f), 770319, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01i"), 447787, 3768180534u, new Vector3(-746.438354f, 587.3919f, 146.078217f), new Vector3(0f, 0f, 59.61162f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01j"), 204864, 2174165298u, new Vector3(-745.7004f, 588.709656f, 146.268219f), new Vector3(0f, 0f, 59.61162f), 447787, 0, new Vector3(1.51000023f, 0.0300000049f, 0.189999983f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 447565, 1228147776u, new Vector3(-741.5627f, 587.9035f, 146.225372f), new Vector3(0f, -0f, -98.07038f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 259661, 1228147776u, new Vector3(-736.266f, 586.58075f, 146.227127f), new Vector3(0f, -0f, -119.668037f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_weed_stack_01a"), 624205, 822904642u, new Vector3(-738.9452f, 586.4592f, 146.224548f), new Vector3(0f, -0f, 164.428345f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 696659, 2852049058u, new Vector3(-738.435059f, 582.554749f, 145.63353f), new Vector3(0f, -0f, -118.64209f), 555356, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 555356, 938137634u, new Vector3(-738.435059f, 582.554749f, 145.63353f), new Vector3(0f, -0f, -118.64209f), 314465, 0, new Vector3(-5.709991f, -9.019999f, -0.220000193f), new Vector3(0f, 0f, 179.999985f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab_start"), 732216, 37228785u, new Vector3(-738.435059f, 582.554749f, 145.63353f), new Vector3(0f, -0f, -118.64209f), 555356, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01d"), 766061, 158478566u, new Vector3(-734.9083f, 594.0617f, 144.476685f), new Vector3(0f, 0f, -30.1959343f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 785, 938137634u, new Vector3(-742.365234f, 594.087341f, 142.04866f), new Vector3(0f, 0f, 60.70943f), 475147, 0, new Vector3(-1.33999908f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 475147, 938137634u, new Vector3(-741.709656f, 595.256042f, 142.04866f), new Vector3(0f, 0f, 60.70943f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 29975, 1870138714u, new Vector3(-742.365234f, 594.087341f, 142.04866f), new Vector3(0f, 0f, 60.70943f), 785, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 196896, 1870138714u, new Vector3(-741.709656f, 595.256042f, 142.04866f), new Vector3(0f, 0f, 60.70943f), 475147, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 128553, 2852049058u, new Vector3(-742.365234f, 594.087341f, 142.04866f), new Vector3(0f, 0f, 60.70943f), 785, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 716831, 2852049058u, new Vector3(-741.709656f, 595.256042f, 142.04866f), new Vector3(0f, 0f, 60.70943f), 475147, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab_start"), 150568, 37228785u, new Vector3(-741.709656f, 595.256042f, 142.04866f), new Vector3(0f, 0f, 60.70943f), 475147, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab3_start"), 745767, 1768229041u, new Vector3(-742.365234f, 594.087341f, 142.04866f), new Vector3(0f, 0f, 60.70943f), 785, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01d"), 245825, 158478566u, new Vector3(-744.051147f, 591.7122f, 142.481476f), new Vector3(0f, 0f, 60.6441956f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01h"), 595515, 2992145076u, new Vector3(-745.521851f, 589.0975f, 142.481476f), new Vector3(0f, 0f, 60.6441956f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 65087, 938137634u, new Vector3(-741.9818f, 587.547058f, 142.048828f), new Vector3(0f, 0f, 61.1235046f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 518985, 1870138714u, new Vector3(-741.9818f, 587.547058f, 142.048828f), new Vector3(0f, 0f, 61.1235046f), 65087, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 269387, 2852049058u, new Vector3(-741.9818f, 587.547058f, 142.048828f), new Vector3(0f, 0f, 61.1235046f), 65087, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab3_start"), 556864, 1768229041u, new Vector3(-741.9818f, 587.547058f, 142.048828f), new Vector3(0f, 0f, 61.1235046f), 65087, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 505172, 938137634u, new Vector3(-744.2576f, 585.326843f, 142.048828f), new Vector3(0f, -0f, -119.674759f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 311913, 938137634u, new Vector3(-745.092163f, 583.8144f, 142.048828f), new Vector3(0f, -0f, -118.599304f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab3_start"), 525150, 1768229041u, new Vector3(-745.092163f, 583.8144f, 142.048828f), new Vector3(0f, -0f, -118.599304f), 311913, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab4_start"), 479826, 2414797517u, new Vector3(-744.2576f, 585.326843f, 142.048828f), new Vector3(0f, 0f, 150.325226f), 505172, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, -90.00001f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 143967, 1870138714u, new Vector3(-745.092163f, 583.8144f, 142.048828f), new Vector3(0f, -0f, -118.599304f), 311913, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 225631, 1870138714u, new Vector3(-744.2576f, 585.326843f, 142.048828f), new Vector3(0f, -0f, -119.674759f), 505172, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 43613, 2852049058u, new Vector3(-744.2576f, 585.326843f, 142.048828f), new Vector3(0f, -0f, -119.674759f), 505172, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 380223, 2852049058u, new Vector3(-745.092163f, 583.8144f, 142.048828f), new Vector3(0f, -0f, -118.599304f), 311913, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_safe_01a"), 642428, 1098122770u, new Vector3(-740.8606f, 579.0433f, 141.46051f), new Vector3(0f, 0f, -89.4082947f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01b"), 437512, 2413416523u, new Vector3(-743.265137f, 582.8218f, 141.46051f), new Vector3(0f, 0f, -29.57672f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_gold_stack_01a"), 264264, 4114893066u, new Vector3(-743.3409f, 582.705566f, 142.312469f), new Vector3(0f, 0f, -28.7577286f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01c"), 462864, 876512894u, new Vector3(-734.8214f, 594.0209f, 140.626678f), new Vector3(0f, 0f, -28.377861f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01b"), 283173, 2413416523u, new Vector3(-736.3487f, 603.6745f, 137.660385f), new Vector3(0f, 0f, -30.4638f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01b"), 369982, 2413416523u, new Vector3(-733.5345f, 602.0659f, 137.660385f), new Vector3(0f, 0f, -29.8490524f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 92725, 938137634u, new Vector3(-734.8796f, 602.9074f, 138.2487f), new Vector3(0f, 0f, -29.9506721f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 545846, 1870138714u, new Vector3(-734.8796f, 602.9074f, 138.2487f), new Vector3(-0f, -0f, -29.9506721f), 92725, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 540474, 2852049058u, new Vector3(-734.8796f, 602.9074f, 138.2487f), new Vector3(-0f, -0f, -29.9506721f), 92725, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab3_start"), 702516, 1768229041u, new Vector3(-734.8796f, 602.9074f, 138.2487f), new Vector3(-0f, -0f, -29.9506721f), 92725, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_gold_stack_01a"), 110128, 4114893066u, new Vector3(-736.4272f, 603.5733f, 138.512344f), new Vector3(0f, 0f, -29.1825581f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_gold_stack_01a"), 37440, 4114893066u, new Vector3(-733.530334f, 601.9132f, 138.512344f), new Vector3(0f, 0f, -27.4421158f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_gold_trolly_01c"), 248896, 3559648747u, new Vector3(-736.0052f, 598.2616f, 138.134262f), new Vector3(0f, -0f, 150.30835f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_gold_trolly_01c"), 363080, 3559648747u, new Vector3(-737.0812f, 598.869751f, 138.134262f), new Vector3(0f, -0f, 151.381989f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mk_money"), 22022, 1113676861u, new Vector3(-1294.28f, 454.289978f, 96.8550339f), new Vector3(0f, -0f, -177.125488f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mk_money"), 268811, 1113676861u, new Vector3(-1289.842f, 451.398865f, 97.17365f), new Vector3(0f, 0f, 28.2045937f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_fingerprint_scanner_01c"), 459570, 1624948650u, new Vector3(-1288.48523f, 443.690552f, 94.64536f), new Vector3(0f, 0f, -89.89419f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_fingerprint_scanner_01c"), 87904, 1624948650u, new Vector3(-1284.36865f, 437.131165f, 94.65232f), new Vector3(0f, 0f, 0.287204862f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 708383, 3294791577u, new Vector3(-1289.67664f, 441.318329f, 93.09478f), new Vector3(0f, -0f, 179.897919f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_door_01a"), 479345, 2244758654u, new Vector3(-1288.84631f, 441.526855f, 94.33478f), new Vector3(0f, -0f, 179.897919f), 708383, 0, new Vector3(-0.829999566f, -0.2100001f, 1.23999918f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 597007, 3294791577u, new Vector3(-1286.17334f, 437.254669f, 93.09478f), new Vector3(0f, 0f, 0.00460007228f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_door_01a"), 117520, 2244758654u, new Vector3(-1286.98328f, 437.0846f, 94.33478f), new Vector3(0f, 0f, 0.00460007228f), 597007, 0, new Vector3(-0.8099996f, -0.170000061f, 1.23999918f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 283990, 1228147776u, new Vector3(-1287.145f, 443.732849f, 97.85964f), new Vector3(0f, 0f, -67.7312546f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 499554, 1228147776u, new Vector3(-1284.23389f, 443.79303f, 97.85964f), new Vector3(0f, -0f, -166.457565f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 712501, 1228147776u, new Vector3(-1281.889f, 445.112885f, 97.8614f), new Vector3(0f, 0f, -89.37697f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_safe_01a"), 174398, 1098122770u, new Vector3(-1289.25586f, 429.1263f, 96.4950562f), new Vector3(0f, -0f, 179.935867f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01j"), 632938, 2174165298u, new Vector3(-1291.17932f, 441.298523f, 97.68921f), new Vector3(0f, -0f, 90.22293f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01c"), 498045, 876512894u, new Vector3(-1288.46362f, 447.714661f, 97.87039f), new Vector3(0f, -0f, -90.03033f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01j"), 11024, 2174165298u, new Vector3(-1285.47852f, 436.47406f, 97.7312f), new Vector3(0f, -0f, 179.178055f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 401437, 938137634u, new Vector3(-1281.87537f, 441.881f, 97.28286f), new Vector3(0f, 0f, -89.837265f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab4_start"), 109335, 2414797517u, new Vector3(-1281.87537f, 441.881f, 97.28286f), new Vector3(-0f, -0f, -179.837265f), 401437, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, -90.00001f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 157562, 1870138714u, new Vector3(-1281.87537f, 441.881f, 97.28286f), new Vector3(-0f, -0f, -89.837265f), 401437, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 204592, 2852049058u, new Vector3(-1281.87537f, 441.881f, 97.28286f), new Vector3(-0f, -0f, -89.837265f), 401437, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 104780, 938137634u, new Vector3(-1290.85376f, 446.3557f, 97.4877548f), new Vector3(0f, 0f, 89.74682f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab_start"), 455209, 37228785u, new Vector3(-1290.85376f, 446.3557f, 97.4877548f), new Vector3(0f, 0f, 89.74682f), 104780, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 777795, 1870138714u, new Vector3(-1290.85376f, 446.3557f, 97.4877548f), new Vector3(0f, 0f, 89.74682f), 104780, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 387652, 2852049058u, new Vector3(-1290.85376f, 446.3557f, 97.4877548f), new Vector3(0f, 0f, 89.74682f), 104780, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01i"), 466752, 3768180534u, new Vector3(-1282.2478f, 430.235626f, 97.49027f), new Vector3(0f, -0f, -92.28627f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_corp_conftable2"), 340783, 52605645u, new Vector3(-1290.84033f, 449.344116f, 93.09953f), new Vector3(0f, -0f, 90.68011f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 145486, 1228147776u, new Vector3(-1290.52087f, 448.3662f, 93.92406f), new Vector3(0f, 0f, 89.27372f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 54329, 1228147776u, new Vector3(-1290.55884f, 450.273163f, 93.92406f), new Vector3(0f, -0f, 92.2420349f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01d"), 409931, 158478566u, new Vector3(-1291.19556f, 444.3053f, 94.13033f), new Vector3(0f, 0f, 89.0924454f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 463467, 938137634u, new Vector3(-1284.13367f, 428.861755f, 93.76435f), new Vector3(0f, -0f, 179.526123f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 199509, 938137634u, new Vector3(-1285.78345f, 428.875336f, 93.76435f), new Vector3(0f, -0f, 179.526123f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 302594, 938137634u, new Vector3(-1287.43323f, 428.888916f, 93.76435f), new Vector3(0f, -0f, 179.526123f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01b"), 43121, 2413416523u, new Vector3(-1284.862f, 433.213379f, 93.12031f), new Vector3(0f, -0f, -90.18968f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_gold_stack_01a"), 349703, 4114893066u, new Vector3(-1284.9812f, 433.235779f, 93.97227f), new Vector3(0f, 0f, -89.3193f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 578839, 1870138714u, new Vector3(-1284.13367f, 428.861755f, 93.76435f), new Vector3(0f, -0f, 179.526123f), 463467, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 693015, 1870138714u, new Vector3(-1285.78345f, 428.875336f, 93.76435f), new Vector3(0f, -0f, 179.526123f), 199509, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 320023, 1870138714u, new Vector3(-1287.43323f, 428.888916f, 93.76435f), new Vector3(0f, -0f, 179.526123f), 302594, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 96780, 2852049058u, new Vector3(-1284.13367f, 428.861755f, 93.76435f), new Vector3(0f, -0f, 179.526123f), 463467, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 487683, 2852049058u, new Vector3(-1285.78345f, 428.875336f, 93.76435f), new Vector3(0f, -0f, 179.526123f), 199509, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 218146, 2852049058u, new Vector3(-1287.43323f, 428.888916f, 93.76435f), new Vector3(0f, -0f, 179.526123f), 302594, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab4_start"), 429826, 2414797517u, new Vector3(-1287.43323f, 428.888916f, 93.76435f), new Vector3(0f, 0f, 89.5261154f), 302594, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, -90.00001f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab_start"), 114047, 37228785u, new Vector3(-1285.78345f, 428.875336f, 93.76435f), new Vector3(0f, -0f, 179.526123f), 199509, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab3_start"), 406085, 1768229041u, new Vector3(-1284.13367f, 428.861755f, 93.76435f), new Vector3(0f, -0f, 179.526123f), 463467, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01b"), 562203, 645688058u, new Vector3(-1284.32617f, 452.42334f, 92.25724f), new Vector3(0f, 0f, -1.19429517f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01h"), 528484, 2992145076u, new Vector3(-1290.42969f, 460.607117f, 90.34058f), new Vector3(0f, 0f, 0.534897f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01f"), 554875, 4020600141u, new Vector3(-1286.79468f, 460.607117f, 90.33326f), new Vector3(0f, 0f, 0.3813533f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01b"), 654436, 2413416523u, new Vector3(-1288.62585f, 460.161133f, 89.29465f), new Vector3(0f, 0f, -0.5910609f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 775281, 1228147776u, new Vector3(-1288.624f, 460.035828f, 90.14743f), new Vector3(0f, 0f, -0.744604051f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mk_money"), 766061, 1113676861u, new Vector3(346.7127f, 440.694458f, 146.98645f), new Vector3(0f, -0f, 142.946609f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mk_money"), 717676, 1113676861u, new Vector3(344.295471f, 439.391052f, 148.65979f), new Vector3(0f, 0f, -40.2224541f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_fingerprint_scanner_01c"), 237136, 1624948650u, new Vector3(338.239166f, 433.21048f, 146.13736f), new Vector3(0f, -0f, -138.899612f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_fingerprint_scanner_01c"), 31075, 1624948650u, new Vector3(345.1091f, 438.999054f, 142.305649f), new Vector3(0f, 0f, -64.09578f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 353137, 3294791577u, new Vector3(335.6857f, 433.082855f, 144.5708f), new Vector3(0f, -0f, 117.512306f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_door_01a"), 20481, 2244758654u, new Vector3(336.26886f, 432.439484f, 145.8108f), new Vector3(0f, -0f, 117.512306f), 353137, 0, new Vector3(-0.839999557f, -0.220000073f, 1.23999918f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 201305, 3294791577u, new Vector3(340.42746f, 435.357971f, 140.770676f), new Vector3(0f, -0f, 115.808174f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_door_01a"), 180003, 2244758654u, new Vector3(340.9825f, 434.715546f, 142.010681f), new Vector3(0f, -0f, 115.808174f), 201305, 0, new Vector3(-0.8199996f, -0.220000073f, 1.23999918f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 112160, 1228147776u, new Vector3(340.309845f, 429.311462f, 149.334183f), new Vector3(0f, -0f, 130.081512f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_weed_stack_01a"), 681521, 822904642u, new Vector3(342.510925f, 427.853027f, 149.335144f), new Vector3(0f, -0f, -154.784134f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_weed_stack_01a"), 725091, 822904642u, new Vector3(339.517639f, 431.80957f, 149.333359f), new Vector3(0f, 0f, 46.912365f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01b"), 553242, 2413416523u, new Vector3(339.619019f, 426.496765f, 148.170639f), new Vector3(0f, -0f, -153.503815f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01b"), 737295, 2413416523u, new Vector3(325.32254f, 429.152679f, 147.971237f), new Vector3(0f, 0f, 26.440361f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01b"), 408117, 2413416523u, new Vector3(336.0474f, 437.593f, 148.380692f), new Vector3(0f, -0f, 116.057465f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 87108, 2151061176u, new Vector3(336.162567f, 437.652283f, 149.232651f), new Vector3(0f, -0f, 115.954765f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 244050, 2151061176u, new Vector3(325.3933f, 429.066956f, 148.8232f), new Vector3(0f, 0f, 25.9779243f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 528152, 2151061176u, new Vector3(339.522552f, 426.568329f, 149.0226f), new Vector3(0f, -0f, -153.146118f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01e"), 612657, 397364576u, new Vector3(344.500946f, 440.217926f, 146.370682f), new Vector3(0f, 0f, -62.8766747f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01b"), 560445, 2413416523u, new Vector3(334.651245f, 428.803f, 144.5708f), new Vector3(0f, 0f, -63.5182571f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01b"), 563027, 2413416523u, new Vector3(326.821136f, 422.8343f, 144.5708f), new Vector3(0f, -0f, 116.075577f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01b"), 186423, 2413416523u, new Vector3(326.0959f, 424.316376f, 144.5708f), new Vector3(0f, -0f, 116.075577f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_coke_trolly"), 755766, 3031213828u, new Vector3(325.370636f, 425.798462f, 145.0438f), new Vector3(0f, -0f, 116.075577f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_coke_trolly"), 747616, 3031213828u, new Vector3(330.364166f, 425.2202f, 145.069733f), new Vector3(0f, -0f, -154.918869f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01d"), 97591, 158478566u, new Vector3(335.823669f, 440.601532f, 145.575668f), new Vector3(0f, 0f, 25.5003281f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 772428, 938137634u, new Vector3(335.261658f, 432.599152f, 141.359f), new Vector3(0f, -0f, 116.391106f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 22391, 938137634u, new Vector3(334.528168f, 434.077179f, 141.359f), new Vector3(0f, -0f, 116.391106f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 572271, 938137634u, new Vector3(333.794678f, 435.555084f, 141.359f), new Vector3(0f, -0f, 116.391106f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab3_start"), 635468, 1768229041u, new Vector3(335.261658f, 432.599152f, 141.359f), new Vector3(0f, -0f, 116.391106f), 772428, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab4_start"), 715116, 2414797517u, new Vector3(334.528168f, 434.077179f, 141.359f), new Vector3(0f, 0f, 26.3911076f), 22391, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, -90.00001f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab_start"), 736538, 37228785u, new Vector3(333.794678f, 435.555084f, 141.359f), new Vector3(0f, -0f, 116.391106f), 572271, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 663605, 1870138714u, new Vector3(335.261658f, 432.599152f, 141.359f), new Vector3(0f, -0f, 116.391106f), 772428, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 498476, 1870138714u, new Vector3(334.528168f, 434.077179f, 141.359f), new Vector3(0f, -0f, 116.391106f), 22391, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 455538, 1870138714u, new Vector3(333.794678f, 435.555084f, 141.359f), new Vector3(0f, -0f, 116.391106f), 572271, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 405366, 2852049058u, new Vector3(333.794678f, 435.555084f, 141.359f), new Vector3(0f, -0f, 116.391106f), 572271, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 119893, 2852049058u, new Vector3(334.528168f, 434.077179f, 141.359f), new Vector3(0f, -0f, 116.391106f), 22391, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 482127, 2852049058u, new Vector3(335.261658f, 432.599152f, 141.359f), new Vector3(0f, -0f, 116.391106f), 772428, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_coke_trolly"), 391174, 3031213828u, new Vector3(336.2108f, 437.975983f, 141.243668f), new Vector3(0f, 0f, 22.5352764f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_coke_trolly"), 330090, 3031213828u, new Vector3(338.159332f, 436.906158f, 141.243668f), new Vector3(0f, 0f, -67.23606f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mk_money"), 172896, 1113676861u, new Vector3(-174.7513f, 502.5596f, 136.699524f), new Vector3(0f, -0f, -100.792976f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mk_money"), 136791, 1113676861u, new Vector3(-175.236755f, 500.148438f, 136.932709f), new Vector3(0f, 0f, 53.7153778f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_fingerprint_scanner_01c"), 110673, 1624948650u, new Vector3(-170.840652f, 492.723755f, 134.64772f), new Vector3(0f, 0f, -60.0274849f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_fingerprint_scanner_01c"), 339469, 1624948650u, new Vector3(-174.81424f, 500.9547f, 130.438385f), new Vector3(0f, 0f, 10.75718f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 485140, 3294791577u, new Vector3(-171.44f, 490.3185f, 132.843719f), new Vector3(0f, -0f, -167.264664f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_door_01a"), 240655, 2244758654u, new Vector3(-170.712585f, 490.728973f, 134.093719f), new Vector3(0f, -0f, -167.264664f), 485140, 0, new Vector3(-0.7999996f, -0.2400001f, 1.24999917f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 709904, 3294791577u, new Vector3(-172.3773f, 495.578979f, 129.0436f), new Vector3(0f, -0f, -168.636673f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_door_01a"), 763499, 2244758654u, new Vector3(-171.6187f, 495.966034f, 130.2936f), new Vector3(0f, -0f, -168.636673f), 709904, 0, new Vector3(-0.820000052f, -0.230000079f, 1.24999917f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 334163, 1228147776u, new Vector3(-164.567017f, 495.55426f, 137.608887f), new Vector3(0f, 0f, -78.75037f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 430141, 1228147776u, new Vector3(-166.567764f, 493.795349f, 137.6071f), new Vector3(0f, -0f, -155.980209f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 517905, 1228147776u, new Vector3(-169.166611f, 493.647278f, 137.6071f), new Vector3(0f, -0f, 121.668465f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01j"), 338519, 2174165298u, new Vector3(-173.0234f, 490.198425f, 137.407516f), new Vector3(0f, -0f, 100.579964f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 581992, 938137634u, new Vector3(-174.877075f, 491.934143f, 137.241913f), new Vector3(0f, -0f, -170.108673f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 428882, 938137634u, new Vector3(-176.258423f, 491.68335f, 137.241928f), new Vector3(0f, -0f, -169.800446f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab_start"), 592435, 37228785u, new Vector3(-174.877075f, 491.934143f, 137.241913f), new Vector3(0f, -0f, -170.108673f), 581992, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab3_start"), 251495, 1768229041u, new Vector3(-176.258423f, 491.68335f, 137.241928f), new Vector3(0f, -0f, -169.800446f), 428882, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 432652, 1870138714u, new Vector3(-174.877075f, 491.934143f, 137.241913f), new Vector3(0f, -0f, -170.108673f), 581992, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 244780, 1870138714u, new Vector3(-176.258423f, 491.68335f, 137.241928f), new Vector3(0f, -0f, -169.800446f), 428882, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 5422, 2852049058u, new Vector3(-174.877075f, 491.934143f, 137.241913f), new Vector3(0f, -0f, -170.108673f), 581992, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 218491, 2852049058u, new Vector3(-176.258423f, 491.68335f, 137.241928f), new Vector3(0f, -0f, -169.800446f), 428882, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_corp_conftable3"), 775513, 4270762401u, new Vector3(-165.332489f, 478.976379f, 132.843765f), new Vector3(0f, -0f, -169.168579f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 692740, 1228147776u, new Vector3(-164.43837f, 479.487579f, 133.668289f), new Vector3(0f, -0f, -169.679413f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 572692, 1228147776u, new Vector3(-166.333069f, 479.13208f, 133.668289f), new Vector3(0f, -0f, -167.375259f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01b"), 129363, 2413416523u, new Vector3(-167.476532f, 488.151337f, 132.843719f), new Vector3(0f, 0f, 11.7076521f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 497430, 1228147776u, new Vector3(-167.445221f, 488.048462f, 133.6965f), new Vector3(0f, 0f, 11.45173f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01b"), 149612, 2413416523u, new Vector3(-171.375931f, 489.818268f, 129.0436f), new Vector3(0f, -0f, -169.5722f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01b"), 327717, 2413416523u, new Vector3(-173.972366f, 489.340485f, 129.0436f), new Vector3(0f, -0f, -169.5722f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01b"), 596742, 2413416523u, new Vector3(-176.159943f, 492.301483f, 129.0436f), new Vector3(0f, -0f, 100.654816f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_gold_stack_01a"), 153165, 4114893066u, new Vector3(-176.028244f, 492.334442f, 129.895554f), new Vector3(0f, -0f, 101.268631f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_gold_stack_01a"), 650047, 4114893066u, new Vector3(-173.961349f, 489.4803f, 129.895554f), new Vector3(0f, -0f, -169.98262f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_gold_stack_01a"), 664668, 4114893066u, new Vector3(-171.371521f, 489.949615f, 129.895554f), new Vector3(0f, -0f, -169.572052f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01j"), 305992, 2174165298u, new Vector3(-178.648529f, 492.4238f, 133.843185f), new Vector3(0f, -0f, 100.684448f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01i"), 774162, 3768180534u, new Vector3(-166.398956f, 486.3471f, 137.490875f), new Vector3(0f, -0f, -168.974411f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01f"), 4377, 4020600141u, new Vector3(-162.01767f, 480.823242f, 137.197128f), new Vector3(0f, 0f, -80.29103f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 437829, 938137634u, new Vector3(-170.531235f, 479.374878f, 136.832474f), new Vector3(0f, -0f, 101.802246f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab3_start"), 456797, 1768229041u, new Vector3(-170.531235f, 479.374878f, 136.832474f), new Vector3(0f, -0f, 101.802246f), 437829, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 290356, 1870138714u, new Vector3(-170.531235f, 479.374878f, 136.832474f), new Vector3(0f, -0f, 101.802246f), 437829, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 180306, 2852049058u, new Vector3(-170.531235f, 479.374878f, 136.832474f), new Vector3(0f, -0f, 101.802246f), 437829, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mk_money"), 449910, 1113676861u, new Vector3(119.416794f, 564.0475f, 183.2435f), new Vector3(0f, -0f, 176.329254f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mk_money"), 129908, 1113676861u, new Vector3(116.992332f, 562.239441f, 183.576065f), new Vector3(0f, 0f, 26.0185032f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_fingerprint_scanner_01a"), 100711, 278208236u, new Vector3(123.853348f, 548.029f, 181.12088f), new Vector3(0f, 0f, 4.73678064f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_fingerprint_scanner_01a"), 100711, 278208236u, new Vector3(119.07f, 563.95f, 177.13f), new Vector3(0f, 0f, -83.93f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 129584, 3294791577u, new Vector3(122.127083f, 547.979858f, 179.4972f), new Vector3(0f, 0f, 5.401992f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_door_01a"), 734984, 2244758654u, new Vector3(121.331436f, 547.683655f, 180.7472f), new Vector3(0f, 0f, 5.401992f), 129584, 0, new Vector3(-0.8199996f, -0.220000073f, 1.24999917f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 724542, 3294791577u, new Vector3(116.255264f, 564.599854f, 175.697067f), new Vector3(0f, 0f, 6.642249f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_door_01a"), 294980, 2244758654u, new Vector3(115.457443f, 564.2754f, 176.947067f), new Vector3(0f, 0f, 6.642249f), 724542, 0, new Vector3(-0.829999566f, -0.230000079f, 1.24999917f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_weed_stack_01a"), 15480, 822904642u, new Vector3(123.286263f, 554.664368f, 184.26123f), new Vector3(0f, -0f, -160.300323f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 30742, 2151061176u, new Vector3(125.557266f, 556.35376f, 184.262985f), new Vector3(0f, 0f, -84.85826f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 238688, 1228147776u, new Vector3(120.848061f, 554.77124f, 184.262054f), new Vector3(0f, -0f, 116.530754f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01b"), 481914, 2413416523u, new Vector3(118.364441f, 540.6851f, 182.897461f), new Vector3(0f, -0f, 95.54483f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01b"), 133987, 2413416523u, new Vector3(120.291977f, 539.3997f, 182.897461f), new Vector3(0f, -0f, -175.24585f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 692000, 1228147776u, new Vector3(120.277351f, 539.5023f, 183.750244f), new Vector3(0f, -0f, -174.886734f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 602947, 1228147776u, new Vector3(118.467293f, 540.697144f, 183.750244f), new Vector3(0f, -0f, 95.59542f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01i"), 64324, 3768180534u, new Vector3(123.014984f, 547.2801f, 184.140152f), new Vector3(0f, -0f, -174.0752f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 294441, 938137634u, new Vector3(125.934593f, 553.0145f, 183.685272f), new Vector3(0f, 0f, -83.87365f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 578602, 938137634u, new Vector3(126.075455f, 551.702f, 183.685272f), new Vector3(0f, 0f, -83.87365f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab3_start"), 258062, 1768229041u, new Vector3(125.934593f, 553.0145f, 183.685272f), new Vector3(-0f, -0f, -83.87365f), 294441, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab4_start"), 145259, 2414797517u, new Vector3(126.075455f, 551.702f, 183.685272f), new Vector3(-0f, -0f, -173.873642f), 578602, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, -90.00001f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 13086, 1870138714u, new Vector3(125.934593f, 553.0145f, 183.685272f), new Vector3(-0f, -0f, -83.87365f), 294441, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 404267, 1870138714u, new Vector3(126.075455f, 551.702f, 183.685272f), new Vector3(-0f, -0f, -83.87365f), 578602, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 679540, 2852049058u, new Vector3(125.934593f, 553.0145f, 183.685272f), new Vector3(-0f, -0f, -83.87365f), 294441, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 201783, 2852049058u, new Vector3(126.075455f, 551.702f, 183.685272f), new Vector3(-0f, -0f, -83.87365f), 578602, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01i"), 601907, 3768180534u, new Vector3(116.732948f, 551.5551f, 184.0496f), new Vector3(0f, -0f, 96.31272f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01i"), 720991, 3768180534u, new Vector3(122.478539f, 563.250854f, 182.4962f), new Vector3(0f, 0f, 4.57567644f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 694377, 938137634u, new Vector3(116.291557f, 558.7897f, 180.085342f), new Vector3(0f, -0f, 95.3309f), 391490, 0, new Vector3(-1.59999883f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 391490, 938137634u, new Vector3(116.142906f, 560.382751f, 180.085342f), new Vector3(0f, -0f, 95.3309f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 754950, 1870138714u, new Vector3(116.142906f, 560.382751f, 180.085342f), new Vector3(0f, -0f, 95.3309f), 391490, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab_start"), 505971, 37228785u, new Vector3(116.291557f, 558.7897f, 180.085342f), new Vector3(0f, -0f, 95.3309f), 694377, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab3_start"), 387196, 1768229041u, new Vector3(116.142906f, 560.382751f, 180.085342f), new Vector3(0f, -0f, 95.3309f), 391490, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 449635, 1870138714u, new Vector3(116.142906f, 560.382751f, 180.085342f), new Vector3(0f, -0f, 95.3309f), 391490, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 549889, 2852049058u, new Vector3(116.142906f, 560.382751f, 180.085342f), new Vector3(0f, -0f, 95.3309f), 391490, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 472124, 2852049058u, new Vector3(116.291557f, 558.7897f, 180.085342f), new Vector3(0f, -0f, 95.3309f), 694377, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01h"), 34334, 2992145076u, new Vector3(117.0076f, 548.933533f, 180.4485f), new Vector3(0f, -0f, 96.2854f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01g"), 502279, 4261091832u, new Vector3(116.854324f, 550.325134f, 180.4485f), new Vector3(0f, -0f, 96.2854f), 34334, 0, new Vector3(1.399999f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01b"), 107049, 2413416523u, new Vector3(121.985809f, 549.241f, 179.4972f), new Vector3(0f, 0f, 5.284645f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_gold_stack_01a"), 413310, 4114893066u, new Vector3(121.994331f, 549.099854f, 180.349152f), new Vector3(0f, 0f, 5.69407463f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01f"), 510292, 4020600141u, new Vector3(122.442047f, 563.237f, 178.658844f), new Vector3(0f, 0f, 5.38022232f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01b"), 749407, 2413416523u, new Vector3(115.696907f, 570.3049f, 175.697067f), new Vector3(0f, 0f, 6.56796f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01b"), 230739, 2413416523u, new Vector3(118.952209f, 570.6521f, 175.697067f), new Vector3(0f, 0f, 5.4932847f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01b"), 571219, 2413416523u, new Vector3(120.294975f, 567.6495f, 175.697067f), new Vector3(0f, 0f, -84.07277f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_gold_stack_01a"), 588084, 4114893066u, new Vector3(120.177124f, 567.6309f, 176.549026f), new Vector3(0f, 0f, -84.3285f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_gold_stack_01a"), 775807, 4114893066u, new Vector3(118.944214f, 570.5301f, 176.549026f), new Vector3(0f, 0f, 5.187268f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_gold_stack_01a"), 1353, 4114893066u, new Vector3(115.666115f, 570.1837f, 176.549026f), new Vector3(0f, 0f, 5.03370667f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_diamond_trolly_01c"), 269137, 3489641539u, new Vector3(117.332573f, 570.4526f, 176.170944f), new Vector3(0f, 0f, 6.773834f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_diamond_trolly_01c"), 755989, 3489641539u, new Vector3(119.081856f, 566.1098f, 176.170944f), new Vector3(0f, -0f, -176.043961f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			})
		};

		public static float MainVaultDoorOpenAmt;

		public static float SecondaryVaultDoorOpenAmt;

		public static Prop PrimaryVaultDoor;

		public static Prop SecondaryVaultDoor;

		public static Prop LastKeypad;

		public static Prop USB;

		public static Prop Phone;

		public static bool HackingKeypad = false;

		public static int Scene1;

		public static int LocalTake;

		public static HeistStage CurrentHeistStage = HeistStage.EnterSilthouse;

		public static KeypadClass MainVaultKeypad;

		public static bool HackedMainKeypad;

		public static KeypadClass SecondaryVaultKeypad;

		public static bool HackedSecondaryKeypad;

		public static MissionOjects CurrentSilthouse;

		public List<Entity> SpawnedEntities = new List<Entity>();

		public static bool SilthouseRobberyOn;

		public static Blip TargetBlip;

		public static bool InSilthouse;

		public static void ATTACH_ENTITY_TO_ENTITY(Entity entity1, Entity entity2, int boneIndex, float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, bool p9, bool useSoftPinning, bool collision, bool isPed, int vertexIndex, bool fixedRot)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, entity1, entity2, boneIndex, xPos, yPos, zPos, xRot, yRot, zRot, p9, useSoftPinning, collision, isPed, vertexIndex, fixedRot);
		}

		public TheSilthouseJob()
		{
			base.Tick += onTick;
			int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 343.85f, -999.08f, -99.1977f);
			Function.Call(Hash.REFRESH_INTERIOR, num);
			Function.Call(Hash.DISABLE_INTERIOR, num, false);
			Function.Call(Hash.CAP_INTERIOR, num, false);
			num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 343.85f, -999.08f, -99.1977f);
			Function.Call(Hash.REFRESH_INTERIOR, num);
			Function.Call(Hash.DISABLE_INTERIOR, num, false);
			Function.Call(Hash.CAP_INTERIOR, num, false);
			num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 261.4586, -998.8196, -99.00863);
			Function.Call(Hash.REFRESH_INTERIOR, num);
			Function.Call(Hash.DISABLE_INTERIOR, num, false);
			Function.Call(Hash.CAP_INTERIOR, num, false);
			num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 261.4586, -998.8196, -99.00863);
			Function.Call(Hash.REFRESH_INTERIOR, num);
			Function.Call(Hash.DISABLE_INTERIOR, num, false);
			Function.Call(Hash.CAP_INTERIOR, num, false);
			num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 347.2686, -999.2955, -99.19622);
			Function.Call(Hash.REFRESH_INTERIOR, num);
			Function.Call(Hash.DISABLE_INTERIOR, num, false);
			Function.Call(Hash.CAP_INTERIOR, num, false);
			num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -18.07856, -583.6725, 79.46569);
			Function.Call(Hash.REFRESH_INTERIOR, num);
			Function.Call(Hash.DISABLE_INTERIOR, num, false);
			Function.Call(Hash.CAP_INTERIOR, num, false);
			num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -35.31277, -580.4199, 88.71221);
			Function.Call(Hash.REFRESH_INTERIOR, num);
			Function.Call(Hash.DISABLE_INTERIOR, num, false);
			Function.Call(Hash.CAP_INTERIOR, num, false);
			num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -1468.14, -541.815, 73.4442);
			Function.Call(Hash.REFRESH_INTERIOR, num);
			Function.Call(Hash.DISABLE_INTERIOR, num, false);
			Function.Call(Hash.CAP_INTERIOR, num, false);
			num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -1477.14, -538.7499, 55.5264);
			Function.Call(Hash.REFRESH_INTERIOR, num);
			Function.Call(Hash.DISABLE_INTERIOR, num, false);
			Function.Call(Hash.CAP_INTERIOR, num, false);
			num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -915.811, -379.432, 113.6748);
			Function.Call(Hash.REFRESH_INTERIOR, num);
			Function.Call(Hash.DISABLE_INTERIOR, num, false);
			Function.Call(Hash.CAP_INTERIOR, num, false);
			num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -614.86, 40.6783, 97.60007);
			Function.Call(Hash.REFRESH_INTERIOR, num);
			Function.Call(Hash.DISABLE_INTERIOR, num, false);
			Function.Call(Hash.CAP_INTERIOR, num, false);
			num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -773.407, 341.766, 211.397);
			Function.Call(Hash.REFRESH_INTERIOR, num);
			Function.Call(Hash.DISABLE_INTERIOR, num, false);
			Function.Call(Hash.CAP_INTERIOR, num, false);
			num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -169.286, 486.4938, 137.4436);
			Function.Call(Hash.REFRESH_INTERIOR, num);
			Function.Call(Hash.DISABLE_INTERIOR, num, false);
			Function.Call(Hash.CAP_INTERIOR, num, false);
			num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 340.9412, 437.1798, 149.3925);
			Function.Call(Hash.REFRESH_INTERIOR, num);
			Function.Call(Hash.DISABLE_INTERIOR, num, false);
			Function.Call(Hash.CAP_INTERIOR, num, false);
			num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 373.023, 416.105, 145.7006);
			Function.Call(Hash.REFRESH_INTERIOR, num);
			Function.Call(Hash.DISABLE_INTERIOR, num, false);
			Function.Call(Hash.CAP_INTERIOR, num, false);
			num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -676.127, 588.612, 145.1698);
			Function.Call(Hash.REFRESH_INTERIOR, num);
			Function.Call(Hash.DISABLE_INTERIOR, num, false);
			Function.Call(Hash.CAP_INTERIOR, num, false);
			num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -763.107, 615.906, 144.1401);
			Function.Call(Hash.REFRESH_INTERIOR, num);
			Function.Call(Hash.DISABLE_INTERIOR, num, false);
			Function.Call(Hash.CAP_INTERIOR, num, false);
			num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -857.798, 682.563, 152.6529);
			Function.Call(Hash.REFRESH_INTERIOR, num);
			Function.Call(Hash.DISABLE_INTERIOR, num, false);
			Function.Call(Hash.CAP_INTERIOR, num, false);
			num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 120.5, 549.952, 184.097);
			Function.Call(Hash.REFRESH_INTERIOR, num);
			Function.Call(Hash.DISABLE_INTERIOR, num, false);
			Function.Call(Hash.CAP_INTERIOR, num, false);
			num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -1288, 440.748, 97.69459);
			Function.Call(Hash.REFRESH_INTERIOR, num);
			Function.Call(Hash.DISABLE_INTERIOR, num, false);
			Function.Call(Hash.CAP_INTERIOR, num, false);
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

		public void onTick(object sender, EventArgs e)
		{
			if (!SilthouseRobberyOn)
			{
				return;
			}
			UI.RenderQue = 0f;
			SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.None;
			switch (CurrentHeistStage)
			{
			case HeistStage.EnterSilthouse:
				Screen.ShowSubtitle("Enter the ~y~Silthouse");
				break;
			case HeistStage.LootTheHouse:
			{
				Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 117.7951f, 560.5258f, 184.4464f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "apa_p_mp_door_apartfrt_door"), 0, 0, 0);
				if (prop != null)
				{
					prop.IsPositionFrozen = false;
				}
				Prop prop2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, -173.7538f, 498.3235f, 137.8034f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "apa_p_mp_door_apartfrt_door"), 0, 0, 0);
				if (prop2 != null)
				{
					prop2.IsPositionFrozen = false;
				}
				Prop prop3 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 342.7773f, 437.4438f, 149.5304f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "apa_p_mp_door_apartfrt_door"), 0, 0, 0);
				if (prop3 != null)
				{
					prop3.IsPositionFrozen = false;
				}
				Prop prop4 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, -1289.193f, 450.2027f, 98.04399f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "apa_p_mp_door_apartfrt_door"), 0, 0, 0);
				if (prop4 != null)
				{
					prop4.IsPositionFrozen = false;
				}
				Prop prop5 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, -740.2452f, 594.4656f, 146.4097f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "apa_p_mp_door_apartfrt_door"), 0, 0, 0);
				if (prop5 != null)
				{
					prop5.IsPositionFrozen = false;
				}
				Screen.ShowSubtitle("Loot the ~y~Silthouse~w~ for ~g~Valuables~w~ or leave the house.");
				if (LocalTake != ManageRobberyAnims.CurrentPay)
				{
					if (LocalTake < ManageRobberyAnims.CurrentPay)
					{
						LocalTake += (int)((float)ManageRobberyAnims.CurrentPay / 10f);
					}
					if (LocalTake > ManageRobberyAnims.CurrentPay)
					{
						LocalTake = ManageRobberyAnims.CurrentPay;
					}
				}
				UI.DrawPayment("TAKE", LocalTake);
				if (MainVaultKeypad != null && !HackedMainKeypad && MainVaultKeypad.Keypad != null)
				{
					CheckKeypad(MainVaultKeypad);
				}
				if (PrimaryVaultDoor != null)
				{
					if (HackedMainKeypad && PrimaryVaultDoor != null)
					{
						if (PrimaryVaultDoor.IsAttached())
						{
							PrimaryVaultDoor.Detach();
						}
						if (MainVaultDoorOpenAmt <= 160f)
						{
							MainVaultDoorOpenAmt += 0.25f;
							Vector3 rotation = new Vector3(0f, 0f, PrimaryVaultDoor.Rotation.Z - 0.25f);
							PrimaryVaultDoor.Rotation = rotation;
						}
					}
				}
				else if (MainVaultKeypad != null && MainVaultKeypad.Keypad != null)
				{
					Vector3 position = MainVaultKeypad.Keypad.Position;
					PrimaryVaultDoor = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_door_01a"), 0, 0, 0);
				}
				if (SecondaryVaultKeypad != null && !HackedSecondaryKeypad && SecondaryVaultKeypad.Keypad != null)
				{
					CheckKeypad(SecondaryVaultKeypad);
				}
				if (SecondaryVaultDoor != null)
				{
					if (HackedSecondaryKeypad && SecondaryVaultDoor != null)
					{
						if (SecondaryVaultDoor.IsAttached())
						{
							SecondaryVaultDoor.Detach();
						}
						if (SecondaryVaultDoorOpenAmt <= 160f)
						{
							SecondaryVaultDoorOpenAmt += 0.25f;
							Vector3 rotation2 = new Vector3(0f, 0f, SecondaryVaultDoor.Rotation.Z - 0.25f);
							SecondaryVaultDoor.Rotation = rotation2;
						}
					}
				}
				else if (SecondaryVaultKeypad != null && SecondaryVaultKeypad.Keypad != null)
				{
					Vector3 position2 = SecondaryVaultKeypad.Keypad.Position;
					SecondaryVaultDoor = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position2.X, position2.Y, position2.Z, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_door_01a"), 0, 0, 0);
				}
				break;
			}
			case HeistStage.EscapeTheCops:
				if (LocalTake != ManageRobberyAnims.CurrentPay)
				{
					if (LocalTake < ManageRobberyAnims.CurrentPay)
					{
						LocalTake += (int)((float)ManageRobberyAnims.CurrentPay / 10f);
					}
					if (LocalTake > ManageRobberyAnims.CurrentPay)
					{
						LocalTake = ManageRobberyAnims.CurrentPay;
					}
				}
				UI.DrawPayment("TAKE", LocalTake);
				if (Game.Player.WantedLevel != 0)
				{
					Screen.ShowSubtitle("Escape the ~r~Cops");
				}
				if (Game.Player.WantedLevel != 0)
				{
					break;
				}
				Screen.ShowSubtitle("Leave the area.");
				if (GetDistance_VDIST2(Game.Player.Character.Position, Enterance, ">", 100f))
				{
					SilthouseRobberyOn = false;
					Main.TotalCrewEarned += ManageRobberyAnims.CurrentPay;
					Main.TotalPlayerEarned += 0f;
					XP.CompleteHeist_TheSilthouseJob++;
					Main.SaveCuts();
					Class1.SetNoLongerInHeist();
					MissionScreen.SetPass("The Silthouse Job", ManageRobberyAnims.CurrentPay, 0, "The Player has died", 2, 0f, 5);
					int num = Game.GameTime + 30000;
					while (HKHModHelperNew.Class1.HeistPassedScreen != -1 && Game.GameTime <= num)
					{
						Script.Wait(0);
					}
					Script.Wait(2000);
					ManagePlayerFunds.AddFunds(ManageRobberyAnims.CurrentPay);
					XP.AddXP_Percent(XP.CompleteHeist_XPPercent_TheSilthouseJob);
					EndHeist();
				}
				break;
			}
			if (!InSilthouse)
			{
				if (GetDistance_VDIST2(Game.Player.Character.Position, Enterance, "<", 100f))
				{
					World.DrawMarker(MarkerType.Cylinder, Enterance, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
					if (GetDistance_VDIST2(Game.Player.Character.Position, Enterance, "<", 2f))
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter The Silthouse");
						if (Game.IsControlJustPressed(Control.Context))
						{
							Screen.FadeOut(1500);
							Script.Wait(1500);
							InSilthouse = true;
							Game.Player.Character.Position = Exit;
							CurrentHeistStage = HeistStage.LootTheHouse;
							foreach (ObjectPlacementMap entity in CurrentSilthouse.Entities)
							{
								if (entity.Entity != null)
								{
									ManageRobberyAnims.AddPropAsLoot((Prop)entity.Entity);
								}
							}
							Script.Wait(1500);
							Screen.FadeIn(1500);
						}
					}
				}
			}
			else if (InSilthouse && GetDistance_VDIST2(Game.Player.Character.Position, Exit, "<", 100f))
			{
				World.DrawMarker(MarkerType.Cylinder, Exit, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
				if (GetDistance_VDIST2(Game.Player.Character.Position, Exit, "<", 2f))
				{
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit The Silthouse");
					if (Game.IsControlJustPressed(Control.Context))
					{
						Screen.FadeOut(1500);
						Script.Wait(1500);
						InSilthouse = false;
						Game.Player.Character.Position = Enterance;
						if (ManageRobberyAnims.CurrentPay >= 25000)
						{
							Game.Player.WantedLevel = 3;
							CurrentHeistStage = HeistStage.EscapeTheCops;
						}
						else
						{
							CurrentHeistStage = HeistStage.EnterSilthouse;
						}
						Script.Wait(1500);
						Screen.FadeIn(1500);
					}
				}
			}
			if (!Game.Player.IsAlive)
			{
				MissionScreen.SetFail("The Silthouse Job", ManageRobberyAnims.CurrentPay, 0, "The Player has died", 2, 0f, 5);
				EndHeist();
			}
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
					if (Game.IsControlJustPressed(Control.Context))
					{
						bool disableHack = Main.DisableHacks;
						if (!HackingKeypad)
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
								if (Phone != null)
								{
									Phone.Delete();
								}
								if (USB != null)
								{
									USB.Delete();
								}
								Game.Player.Character.Task.ClearAllImmediately();
							}
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
				if (FingerprintHack.HackFinishedID == 1)
				{
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
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353200)
				{
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

		public static string LoadDict(string dict)
		{
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, dict))
			{
				Function.Call(Hash.REQUEST_ANIM_DICT, dict);
				Script.Yield();
			}
			return dict;
		}

		public Model RequestModel(string Name)
		{
			Model result = new Model(Name);
			result.Request(10000);
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
			result.Request(2000);
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
			result.Request(10000);
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
			result.Request(10000);
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
			HackingKeypad = false;
			if (Phone != null)
			{
				Phone.Delete();
			}
			if (USB != null)
			{
				USB.Delete();
			}
			if (TargetBlip != null)
			{
				TargetBlip.Delete();
			}
			Scene1 = -1;
			HackingKeypad = false;
			LocalTake = 0;
			HackedMainKeypad = false;
			HackedMainKeypad = false;
			foreach (MissionOjects silthouse in Silthouses)
			{
				foreach (ObjectPlacementMap entity in silthouse.Entities)
				{
					if (entity.Entity != null)
					{
						entity.Entity.Delete();
					}
				}
			}
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

		public void DisplayHelpTextThisFrame(string helpText, int Shape)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, Shape);
		}

		public static float GetDistance(Vector3 _1, Vector3 _2)
		{
			return _1.DistanceTo(_2);
		}

		public static void GetClosestSilthouse()
		{
			float radius = 9999f;
			foreach (MissionOjects silthouse in Silthouses)
			{
				if (GetDistance_VDIST2(silthouse.Entities[0].Position, Game.Player.Character.Position, "<", radius))
				{
					radius = GetDistance(silthouse.Entities[0].Position, Game.Player.Character.Position);
					CurrentSilthouse = silthouse;
				}
			}
		}

		public static void GetRandomSilthouse()
		{
			CurrentSilthouse = Silthouses[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, Silthouses.Count)];
		}

		public static void StartHeist()
		{
			Function.Call(Hash.REQUEST_IPL, "apa_stilt_ch2_04_ext1");
			Function.Call(Hash.REQUEST_IPL, "apa_ch2_04_interior_0_v_mp_stilts_b_milo_");
			Function.Call(Hash.REQUEST_IPL, "apa_ch2_04_interior_1_v_mp_stilts_a_milo_");
			Function.Call(Hash.REQUEST_IPL, "apa_ch2_05c_interior_1_v_mp_stilts_a_milo_");
			Function.Call(Hash.REQUEST_IPL, "apa_ch2_05e_interior_0_v_mp_stilts_b_milo_");
			Function.Call(Hash.REQUEST_IPL, "apa_ch2_09b_interior_0_v_mp_stilts_a_milo_");
			Function.Call(Hash.REQUEST_IPL, "apa_ch2_09b_interior_1_v_mp_stilts_b_milo_");
			Function.Call(Hash.REQUEST_IPL, "apa_ch2_09c_interior_0_v_mp_stilts_a_milo_");
			Function.Call(Hash.REQUEST_IPL, "apa_ch2_09c_interior_2_v_mp_stilts_b_milo_");
			Function.Call(Hash.REQUEST_IPL, "apa_ch2_12b_interior_0_v_mp_stilts_a_milo_");
			Function.Call(Hash.REQUEST_IPL, "apa_stilt_ch2_09c_int");
			Function.Call(Hash.REQUEST_IPL, "apa_v_mp_h_01_a");
			Function.Call(Hash.REQUEST_IPL, "apa_v_mp_h_01_b");
			Function.Call(Hash.REQUEST_IPL, "apa_v_mp_h_01_c");
			Function.Call(Hash.REQUEST_IPL, "apa_v_mp_h_02_a");
			Function.Call(Hash.REQUEST_IPL, "apa_v_mp_h_02_c");
			Function.Call(Hash.REQUEST_IPL, "apa_v_mp_h_03_a");
			Function.Call(Hash.REQUEST_IPL, "apa_v_mp_h_03_b");
			Function.Call(Hash.REQUEST_IPL, "apa_v_mp_h_03_c");
			Function.Call(Hash.REQUEST_IPL, "apa_v_mp_h_04_a");
			Function.Call(Hash.REQUEST_IPL, "apa_v_mp_h_04_b");
			Function.Call(Hash.REQUEST_IPL, "apa_v_mp_h_04_c");
			Function.Call(Hash.REQUEST_IPL, "apa_v_mp_h_05_a");
			Function.Call(Hash.REQUEST_IPL, "apa_v_mp_h_05_b");
			Function.Call(Hash.REQUEST_IPL, "apa_v_mp_h_05_c");
			Function.Call(Hash.REQUEST_IPL, "apa_v_mp_h_06_a");
			Function.Call(Hash.REQUEST_IPL, "apa_v_mp_h_06_b");
			Function.Call(Hash.REQUEST_IPL, "apa_v_mp_h_06_c");
			Function.Call(Hash.REQUEST_IPL, "apa_v_mp_h_07_a");
			Function.Call(Hash.REQUEST_IPL, "apa_v_mp_h_07_b");
			Function.Call(Hash.REQUEST_IPL, "apa_v_mp_h_07_c");
			Function.Call(Hash.REQUEST_IPL, "apa_v_mp_h_08_a");
			Function.Call(Hash.REQUEST_IPL, "apa_v_mp_h_08_b");
			Function.Call(Hash.REQUEST_IPL, "apa_v_mp_h_08_c");
			MainVaultDoorOpenAmt = 0f;
			SecondaryVaultDoorOpenAmt = 0f;
			ManageRobberyAnims.CurrentPay = 0;
			ManageRobberyAnims.Heist_Average_Pay = (float)(HeistMinPay + HeistMaxPay) / 2f * 0.6f;
			EndHeist();
			GetRandomSilthouse();
			CurrentSilthouse.CreateEntites();
			Enterance = CurrentSilthouse.Entities[0].Position;
			CurrentSilthouse.Entities[0].Entity.Opacity = 0;
			Exit = CurrentSilthouse.Entities[1].Position;
			CurrentSilthouse.Entities[1].Entity.Opacity = 0;
			MainVaultKeypad = new KeypadClass((Prop)CurrentSilthouse.Entities[2].Entity, 2);
			SecondaryVaultKeypad = new KeypadClass((Prop)CurrentSilthouse.Entities[3].Entity, 2);
			CurrentHeistStage = HeistStage.EnterSilthouse;
			SilthouseRobberyOn = true;
			TargetBlip = GlobalHelper.CreateBlip(CurrentSilthouse.Entities[0].Position);
			TargetBlip.Sprite = BlipSprite.Safehouse;
			TargetBlip.Color = BlipColor.Yellow;
			TargetBlip.Name = "Target Silthouse";
			TargetBlip.IsShortRange = true;
		}
	}
}
