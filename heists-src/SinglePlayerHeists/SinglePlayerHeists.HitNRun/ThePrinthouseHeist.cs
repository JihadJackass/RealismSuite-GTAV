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
	internal class ThePrinthouseHeist : Script
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
			HeadingToPrinthouse,
			VaultNotOpenAndCrateStorageNotOpen,
			VaultOpenAndCrateStorageNotOpen,
			VaultNotOpenAndCrateStorageOpen,
			VaultOpenAndCrateStorageOpen,
			PushingCrate,
			LoadedAtLeastOneCrate,
			LeaveWithStockade,
			StealCCTVHDD,
			EscapeCops
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

		public static int PercentNeedTake = (int)((float)(HeistMaxPay + HeistMinPay) / 1.5f / 100f * 45f);

		public static int TakePerMoneyCrate = (int)((float)(HeistMaxPay + HeistMinPay) / 1.5f / 100f * 45f);

		public static int HeistMinPay = 750000;

		public static int HeistMaxPay = 2500000;

		public static int CurrentPay = 0;

		public static int LocalTake = 0;

		public static Prop VaultDoor;

		public static Prop HDDCrate;

		public static Prop MCStorage_DoorL;

		public static Prop MCStorage_DoorR;

		public static Prop LeverBox;

		public static Vehicle Stockade;

		public static Blip MainBlip;

		public static bool ThePrinthouseHeistOn = false;

		public static Heiststage CurrentHeistStage = Heiststage.None;

		public static Vector3 EnterMarker;

		public static Vector3 ExitMarker;

		public static bool IsInInterior;

		public static bool VaultOpen;

		public static bool MoneyCrateStorageOpen;

		public static Prop MoneyCrate1;

		public static bool MoneyCrate1inStockade;

		public static Prop MoneyCrate2;

		public static bool MoneyCrate2inStockade;

		public static Prop GateA;

		public static Prop GateB;

		public static Prop MoneyStorageLever;

		public static ManageRobberyAnims.PushableTrolley PushableTrolleyA;

		public static ManageRobberyAnims.PushableTrolley PushableTrolleyB;

		public static MissionOjects PrinthouseInterior = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mk_money"), 337705, 1113676861u, new Vector3(-247.423141f, -848.0635f, 30.2830639f), new Vector3(0f, 0f, -3.65389562f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mk_money"), 389984, 1113676861u, new Vector3(519.7555f, -2639.65625f, -49.7208138f), new Vector3(0f, -0f, -179.294662f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_door_01a"), 552058, 2244758654u, new Vector3(518.1654f, -2637.51245f, -48.74991f), new Vector3(0f, 0f, 89.43008f), 736004, 0, new Vector3(-0.823488f, -0.236425251f, 1.25f), new Vector3(0f, -0f, 0.06181687f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_gold_vault_gate_01"), 737286, 4019746726u, new Vector3(515.669067f, -2624.931f, -48.7997742f), new Vector3(-0f, -0f, -0.09997028f), 736518, 0, new Vector3(0f, -0.02f, 1.2f), new Vector3(0f, 0f, -5.722046E-06f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_gold_vault_gate_01"), 732934, 4019746726u, new Vector3(515.6631f, -2628.39087f, -48.7997742f), new Vector3(0f, -0f, 179.90004f), 737542, 0, new Vector3(0f, 0.0199999921f, 1.2f), new Vector3(0f, 0f, -179.999985f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_fingerprint_scanner_01e"), 353313, 996668613u, new Vector3(518.1487f, -2635.7207f, -48.3491936f), new Vector3(0f, 0f, 89.05868f), 684320, 0, new Vector3(0.968084f, -0.199970737f, 1.65071487f), new Vector3(0f, 0f, -0.309585571f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_ser_storage_01a"), 35123, 1413173041u, new Vector3(514.8478f, -2620.87061f, -49.19791f), new Vector3(0f, 0f, 81.57917f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_large_gold_empty"), 501786, 2815367108u, new Vector3(513.39386f, -2627.60449f, -49.611515f), new Vector3(0.0880195051f, -0.004820165f, -0.2311624f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_large_gold_empty"), 520218, 2815367108u, new Vector3(513.3991f, -2625.70435f, -49.6117973f), new Vector3(0f, 0f, -0.23175402f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Stockade"), 718086, 1747439474u, new Vector3(525.0827f, -2614.23633f, -49.39676f), new Vector3(0.03445339f, 0.011107374f, -1.49572313f), 111, 125, 111, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_printmachine_4rollerpress"), 615706, 1325846017u, new Vector3(525.4145f, -2634.84766f, -49.9975357f), new Vector3(0f, -0f, 179.179886f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_printmachine_4rollerpress"), 447769, 1325846017u, new Vector3(525.4187f, -2632.10767f, -49.9975357f), new Vector3(0f, -0f, 179.537476f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_prtmachine_dryer_spin"), 505626, 769275872u, new Vector3(529.4655f, -2639.3667f, -49.99991f), new Vector3(0f, -0f, 179.1314f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_prtmachine_dryer_op"), 193542, 1558153977u, new Vector3(526.895935f, -2639.329f, -49.99977f), new Vector3(0f, -0f, 179.1314f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_prtmachine_dryer_spin"), 715526, 769275872u, new Vector3(525.5861f, -2639.31f, -49.99991f), new Vector3(0f, -0f, 179.1314f), 193542, 0, new Vector3(1.30995548f, 0.000796288252f, -0.0001373291f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_prtmachine_dryer_spin"), 715782, 769275872u, new Vector3(524.2563f, -2639.29053f, -49.99991f), new Vector3(0f, -0f, 179.1314f), 715526, 0, new Vector3(1.32996392f, 0.0005792491f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 300570, 3294791577u, new Vector3(517.938049f, -2636.68652f, -49.99991f), new Vector3(0f, 0f, 89.36826f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 760326, 3294791577u, new Vector3(517.9137f, -2638.89648f, -49.99991f), new Vector3(0f, 0f, 89.36826f), 300570, 0, new Vector3(-2.21019888f, -1.975894E-05f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 719110, 3294791577u, new Vector3(517.9623f, -2634.48682f, -49.99991f), new Vector3(0f, 0f, 89.36826f), 300570, 0, new Vector3(2.19989657f, -5.074544E-06f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_metal_plates01"), 719366, 2067252279u, new Vector3(518.0293f, -2638.85571f, -50.5999069f), new Vector3(-90f, -89.00542f, 0f), 760326, 0, new Vector3(0.0420240164f, -0.115150332f, -0.599999964f), new Vector3(-89.99999f, 0f, -0.362838745f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_metal_plates01"), 102932, 2067252279u, new Vector3(518.0789f, -2634.356f, -50.5999069f), new Vector3(-90f, -89.00541f, 0f), 719110, 0, new Vector3(0.132107973f, -0.1151824f, -0.5999985f), new Vector3(-90f, -89.00542f, -89.36826f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Stockade"), 718854, 1747439474u, new Vector3(521.4248f, -2614.13818f, -49.3235741f), new Vector3(0.9374f, 0.730801f, 177.45813f), 111, 125, 111, -1, -1, 0, new Vector3(-3.65925169f, 0.0026011765f, 0.07247281f), new Vector3(0.971644938f, 0.742537439f, 178.953659f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_metal_plates01"), 718342, 2067252279u, new Vector3(518.0966f, -2632.756f, -50.5999069f), new Vector3(-90f, -89.00541f, 0f), 719622, 0, new Vector3(-0.367892f, -0.1151824f, -0.5999985f), new Vector3(-90f, -89.00542f, -89.36826f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 719622, 3294791577u, new Vector3(517.9855f, -2632.387f, -49.99991f), new Vector3(0f, 0f, 89.36826f), 300570, 0, new Vector3(4.299895f, -5.074544E-06f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_metal_plates01"), 249350, 2067252279u, new Vector3(516.767944f, -2631.058f, -50.5999069f), new Vector3(-90f, -179.0054f, 0f), 743449, 0, new Vector3(0.332107961f, -0.1151824f, -0.5999985f), new Vector3(-90f, -89.00542f, -89.36826f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 743449, 3294791577u, new Vector3(517.098755f, -2631.177f, -49.99991f), new Vector3(0f, -0f, 179.36824f), 300570, 0, new Vector3(5.499896f, 0.89999485f, 0f), new Vector3(0f, 0f, 89.99999f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_metal_plates01"), 502554, 2067252279u, new Vector3(514.568054f, -2631.034f, -50.5999069f), new Vector3(-90f, -179.0054f, 0f), 723718, 0, new Vector3(0.332107961f, -0.1151824f, -0.5999985f), new Vector3(-90f, -89.00542f, -89.36826f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 723718, 3294791577u, new Vector3(514.898865f, -2631.15283f, -49.99991f), new Vector3(0f, -0f, 179.36824f), 300570, 0, new Vector3(5.499896f, 3.09999418f, 0f), new Vector3(0f, 0f, 89.99999f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_metal_plates01"), 723974, 2067252279u, new Vector3(512.3682f, -2631.00952f, -50.5999069f), new Vector3(-90f, -179.0054f, 0f), 724742, 0, new Vector3(0.332107961f, -0.1151824f, -0.5999985f), new Vector3(-90f, -89.00542f, -89.36826f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 724742, 3294791577u, new Vector3(512.699036f, -2631.12842f, -49.99991f), new Vector3(0f, -0f, 179.36824f), 300570, 0, new Vector3(5.499896f, 5.299992f, 0f), new Vector3(0f, 0f, 89.99999f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_fnccorgm_01b"), 723462, 1042000049u, new Vector3(517.9066f, -2639.92969f, -47.089016f), new Vector3(0f, 0f, 89.0011749f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_fnccorgm_01b"), 747014, 1042000049u, new Vector3(517.9546f, -2637.6333f, -47.089016f), new Vector3(0f, 0f, 89.51276f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_fnccorgm_01b"), 747270, 1042000049u, new Vector3(517.952f, -2635.324f, -47.089016f), new Vector3(0f, 0f, 89.56387f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_fnccorgm_01b"), 247063, 1042000049u, new Vector3(518.0453f, -2633.37378f, -47.0388565f), new Vector3(0f, 0f, 89.56387f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_fnccorgm_01b"), 728326, 1042000049u, new Vector3(518.061462f, -2631.184f, -47.089016f), new Vector3(0f, -0f, 178.771729f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_fnccorgm_01b"), 724230, 1042000049u, new Vector3(515.752258f, -2631.13452f, -47.089016f), new Vector3(0f, -0f, 178.771729f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_fnccorgm_01b"), 724486, 1042000049u, new Vector3(513.442932f, -2631.085f, -47.089016f), new Vector3(0f, -0f, 178.771729f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01a"), 746758, 2257894849u, new Vector3(516.382935f, -2639.5415f, -49.99865f), new Vector3(0f, -0f, 179.741684f), 41498, 0, new Vector3(-1.5287714f, 0.0468817726f, -0.472621918f), new Vector3(0f, 0f, 359.695679f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01a"), 747526, 2257894849u, new Vector3(513.412659f, -2639.52832f, -49.99865f), new Vector3(0f, -0f, 179.741684f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01a"), 748038, 2257894849u, new Vector3(512.097961f, -2637.93115f, -49.99865f), new Vector3(0f, -0f, 90.0716f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01a"), 728070, 2257894849u, new Vector3(512.102f, -2634.95068f, -49.99865f), new Vector3(0f, 0f, 89.04792f), 728838, 0, new Vector3(1.459035f, -0.02407403f, -0.472621918f), new Vector3(0f, 0f, -1.0719223f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01a"), 17178, 2257894849u, new Vector3(512.1455f, -2632.01172f, -49.99865f), new Vector3(0f, 0f, 89.04792f), 163590, 0, new Vector3(1.45702446f, -0.07467698f, -0.472621918f), new Vector3(0f, 0f, -1.0719223f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_prtmachine_dryer_spin"), 419865, 769275872u, new Vector3(513.6519f, -2630.25586f, -49.99991f), new Vector3(0f, -0f, 179.930908f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_prtmachine_dryer"), 112409, 2076733045u, new Vector3(514.960938f, -2630.25049f, -49.99977f), new Vector3(0f, -0f, 179.418655f), 419865, 0, new Vector3(-1.30902326f, -0.00701213954f, 0.0001373291f), new Vector3(0f, 0f, -0.5122528f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_corp_cashtrolley_2"), 641306, 2276369134u, new Vector3(514.940735f, -2631.57739f, -49.99917f), new Vector3(0f, 0f, 0.4861929f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_corp_banktrolley"), 530458, 2820874023u, new Vector3(515.9308f, -2631.56958f, -49.99917f), new Vector3(0f, 0f, 0.4861929f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01a"), 750361, 2257894849u, new Vector3(526.820862f, -2628.2478f, -49.99865f), new Vector3(0f, -0f, 179.812241f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01a"), 57863, 2257894849u, new Vector3(524.255432f, -2628.26465f, -49.99865f), new Vector3(0f, -0f, -178.908325f), 750361, 0, new Vector3(2.56538343f, 0.0253337529f, 0f), new Vector3(0f, 0f, 1.279419f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01a"), 57607, 2257894849u, new Vector3(521.6858f, -2628.32056f, -49.99865f), new Vector3(0f, -0f, -178.908325f), 57863, 0, new Vector3(2.570218f, 0.00693608774f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_printmachine_6rollerpress"), 59399, 3242431923u, new Vector3(512.2687f, -2612.95044f, -49.9975357f), new Vector3(0f, 0f, -89.84899f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_printmachine_6rollerpress"), 59143, 3242431923u, new Vector3(513.777344f, -2612.9458f, -49.9975357f), new Vector3(0f, -0f, -90.00216f), 59399, 0, new Vector3(-0.000662486535f, 1.508674f, 0f), new Vector3(0f, -0f, -0.153167725f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_staff_desk_02"), 641562, 2084153992u, new Vector3(513.3954f, -2620.55981f, -49.99991f), new Vector3(0f, 0f, -0.488318324f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_staff_desk_02"), 56327, 2084153992u, new Vector3(512.363037f, -2623.067f, -49.99991f), new Vector3(0f, -0f, 90.10166f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_offchair_exec_03"), 740614, 1580642483u, new Vector3(513.694946f, -2621.68213f, -49.3977127f), new Vector3(0f, -0f, -131.561966f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_offchair_exec_03"), 740870, 1580642483u, new Vector3(513.4015f, -2623.03247f, -49.3977127f), new Vector3(0f, 0f, -68.4552155f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01b"), 55303, 2413416523u, new Vector3(514.434937f, -2606.69629f, -49.99991f), new Vector3(0f, -0f, 179.360641f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_idol_case"), 741894, 4166900065u, new Vector3(513.0222f, -2624.41748f, -49.1314163f), new Vector3(0f, -0f, 102.741173f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_casefile_01a"), 754950, 2159865087u, new Vector3(514.175049f, -2621.072f, -49.1909065f), new Vector3(0f, 0f, 10.4614182f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01b"), 777748, 2413416523u, new Vector3(517.375f, -2634.46167f, -49.99991f), new Vector3(0f, -0f, -90.2007f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_prtmachine_dryer_spin"), 53767, 769275872u, new Vector3(527.442749f, -2633.59839f, -49.99991f), new Vector3(0f, -0f, -90.60698f), 615706, 0, new Vector3(-2.01016974f, -1.27808f, -0.0023727417f), new Vector3(0f, 0f, -269.786865f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_prtmachine_dryer_spin"), 741126, 769275872u, new Vector3(527.58136f, -2630.90015f, -49.99991f), new Vector3(0f, -0f, -90.60698f), 615706, 0, new Vector3(-2.11016965f, -3.97807837f, -0.0023727417f), new Vector3(0f, 0f, -269.786865f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_printmachine_4rollerpress"), 741382, 1325846017u, new Vector3(525.4473f, -2629.40771f, -49.9975357f), new Vector3(0f, -0f, 179.537476f), 741126, 0, new Vector3(-1.46963787f, -2.149745f, 0.0023727417f), new Vector3(0f, 0f, 270.14447f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_bkr_cashpile_01"), 785158, 2596205386u, new Vector3(513.8849f, -2618.17529f, -49.4213753f), new Vector3(0f, -0f, 90.6482544f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_prtmachine_moneyream"), 771590, 3279188378u, new Vector3(513.3998f, -2625.666f, -49.7416267f), new Vector3(0f, 0f, 89.87892f), 520218, 0, new Vector3(0.000486577163f, 0.03834588f, -0.129830718f), new Vector3(0f, 0f, 90.11068f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_prtmachine_moneyream"), 670982, 3279188378u, new Vector3(513.4036f, -2625.66553f, -49.05382f), new Vector3(0f, 0f, 89.87892f), 520218, 0, new Vector3(0.004364157f, 0.0387279019f, 0.5579772f), new Vector3(0f, 0f, 90.11068f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_prtmachine_moneyream"), 742406, 3279188378u, new Vector3(513.394531f, -2627.566f, -49.7412872f), new Vector3(-0.004990174f, -0.08801002f, 89.8795f), 501786, 0, new Vector3(0.000486577163f, 0.03834588f, -0.129830718f), new Vector3(0f, 0f, 90.11068f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_prtmachine_moneyream"), 1814, 3279188378u, new Vector3(513.3983f, -2627.56665f, -49.05348f), new Vector3(-0.004990174f, -0.08801002f, 89.8795f), 501786, 0, new Vector3(0.004364157f, 0.0387279019f, 0.5579772f), new Vector3(0f, 0f, 90.11068f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ss1_mpint_garage"), 238609, 3082022299u, new Vector3(-239.415054f, -850.460449f, 31.2850647f), new Vector3(0f, 0f, -20.17889f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ss1_mpint_garage"), 785414, 3082022299u, new Vector3(-247.081635f, -847.626038f, 31.7751179f), new Vector3(0f, 0f, -20.2788963f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_slide_door_sm"), 736518, 3540268861u, new Vector3(515.6691f, -2624.911f, -49.9997749f), new Vector3(-0f, -0f, -0.09996456f), 736774, 0, new Vector3(1.35000014f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_slide_door_sm"), 736774, 3540268861u, new Vector3(514.319153f, -2624.90845f, -49.9997749f), new Vector3(-0f, -0f, -0.09996456f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_slide_door_sm"), 737030, 3540268861u, new Vector3(514.363037f, -2628.40869f, -49.9997749f), new Vector3(0f, 0f, -0.09996462f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_slide_door_sm"), 737542, 3540268861u, new Vector3(515.663f, -2628.411f, -49.9997749f), new Vector3(-0f, -0f, -0.09996462f), 737030, 0, new Vector3(1.30000007f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstpillar_04"), 739590, 189702314u, new Vector3(516.014343f, -2626.64746f, -48.209938f), new Vector3(0f, -0f, 90.53715f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstpillar_04"), 3866, 189702314u, new Vector3(516.014343f, -2626.64746f, -47.209938f), new Vector3(0f, -0f, 90.53715f), 739590, 0, new Vector3(0f, 0.001f, MathF.PI * 113f / 355f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sf_prop_sf_table_office_01a"), 287259, 3057578855u, new Vector3(515.055237f, -2617.70728f, -49.9953766f), new Vector3(0f, 0f, 89.78171f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sf_prop_sf_table_office_01a"), 487709, 3057578855u, new Vector3(515.0668f, -2614.73657f, -49.9953766f), new Vector3(0f, 0f, 89.78171f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sf_prop_sf_table_office_01a"), 422173, 3057578855u, new Vector3(515.0809f, -2611.10571f, -49.9953766f), new Vector3(0f, 0f, 89.78171f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_slide_door_sm"), 723237, 3540268861u, new Vector3(512.997864f, -2624.90845f, -49.9997749f), new Vector3(0f, 0f, -0.09996456f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_slide_door_sm"), 521255, 3540268861u, new Vector3(513.0017f, -2628.40869f, -49.9997749f), new Vector3(0f, 0f, -0.09996462f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01a"), 462888, 2257894849u, new Vector3(514.891357f, -2624.44434f, -49.99865f), new Vector3(0f, -0f, -179.679611f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects Objects_VaultDoor = new MissionOjects(new List<ObjectPlacementMap>());

		public static MissionOjects Objects_Cameras = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_07a"), 737798, 2954561821u, new Vector3(527.8434f, -2607.64551f, -45.94616f), new Vector3(0f, 0f, -51.3464737f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_07a"), 738054, 2954561821u, new Vector3(511.9583f, -2630.488f, -45.94616f), new Vector3(0f, -0f, 147.439713f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_07a"), 735750, 2954561821u, new Vector3(527.374451f, -2625.52783f, -45.94616f), new Vector3(0f, 0f, -84.76054f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_07a"), 736006, 2954561821u, new Vector3(529.95105f, -2635.90381f, -45.94616f), new Vector3(0f, 0f, -13.0036831f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_07a"), 736262, 2954561821u, new Vector3(518.5378f, -2639.59766f, -45.94616f), new Vector3(0f, -0f, 99.8996353f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects Objects_Guards = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Security Guard"), 569094, 3613962792u, new Vector3(515.5979f, -2608.78857f, -49.00007f), new Vector3(0f, -0f, -110.553207f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Security Guard"), 698905, 3613962792u, new Vector3(514.4613f, -2622.30835f, -49.0000763f), new Vector3(0f, 0f, 29.22215f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Security Guard"), 733702, 3613962792u, new Vector3(526.0451f, -2625.80542f, -49.00008f), new Vector3(0f, -0f, -115.568382f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Security Guard"), 734214, 3613962792u, new Vector3(516.514343f, -2626.64282f, -49.0099373f), new Vector3(-0f, -0f, -89.46282f), 2725352035u, 200, 0, "false", "None", "None", 739590, 0, new Vector3(0f, -0.4999998f, -0.799999952f), new Vector3(0f, 0f, -179.999985f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Security Guard"), 735238, 3613962792u, new Vector3(527.5914f, -2637.386f, -49.00008f), new Vector3(0f, -0f, -128.720078f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Security Guard"), 734470, 3613962792u, new Vector3(513.8016f, -2623.697f, -49.0000954f), new Vector3(0f, -0f, 113.671524f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Security Guard"), 98586, 3613962792u, new Vector3(519.7803f, -2615.07471f, -49.00007f), new Vector3(0f, -0f, -115.159294f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Security Guard"), 734726, 3613962792u, new Vector3(523.760254f, -2627.23022f, -49.00007f), new Vector3(0f, -0f, -163.269073f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Security Guard"), 734982, 3613962792u, new Vector3(513.3773f, -2631.7688f, -49.0000954f), new Vector3(0f, -0f, -170.985641f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects Objects_Levers = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_lever_box_01b"), 733190, 1882470308u, new Vector3(522.307068f, -2639.272f, -48.5756149f), new Vector3(0f, -0f, -92.732f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_lever_box_01b"), 738310, 1882470308u, new Vector3(511.7393f, -2619.446f, -48.2227974f), new Vector3(0f, 0f, 89.31799f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_lever_box_01b"), 738566, 1882470308u, new Vector3(529.0666f, -2635.58154f, -48.468483f), new Vector3(0f, 0f, 0.526546f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_lever_box_01b"), 738822, 1882470308u, new Vector3(528.054443f, -2613.85718f, -48.6867638f), new Vector3(0f, -0f, -90.62641f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects Objects_SecondaryLoot = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_prtmachine_dryer_op"), 772614, 2410146589u, new Vector3(528.199f, -2639.3584f, -49.99977f), new Vector3(0f, -0f, 179.66423f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_prtmachine_dryer_op"), 465431, 2410146589u, new Vector3(522.948059f, -2639.257f, -49.99977f), new Vector3(0f, -0f, 178.537262f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_prtmachine_dryer_op"), 773638, 2410146589u, new Vector3(512.412f, -2630.25879f, -49.99977f), new Vector3(0f, -0f, -179.578934f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_prtmachine_dryer_op"), 227866, 2410146589u, new Vector3(527.7325f, -2626.933f, -49.99977f), new Vector3(0f, -0f, -90.512764f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_prtmachine_dryer_op"), 499481, 2410146589u, new Vector3(527.7659f, -2624.31348f, -49.99977f), new Vector3(0f, -0f, -90.512764f), 735494, 0, new Vector3(-1.30000007f, 0.01f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_prtmachine_dryer_01a"), 735494, 788498225u, new Vector3(527.7443f, -2625.61328f, -49.99977f), new Vector3(0f, -0f, -90.512764f), 227866, 0, new Vector3(-1.3199991f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects Objects_SecondaryLootFake = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_prtmachine_dryer_spin"), 772614, 769275872u, new Vector3(528.199f, -2639.3584f, -49.99977f), new Vector3(0f, -0f, 179.66423f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_prtmachine_dryer_spin"), 465431, 769275872u, new Vector3(522.948059f, -2639.257f, -49.99977f), new Vector3(0f, -0f, 178.537262f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_prtmachine_dryer_spin"), 773638, 769275872u, new Vector3(512.412f, -2630.25879f, -49.99977f), new Vector3(0f, -0f, -179.578934f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_prtmachine_dryer_spin"), 227866, 769275872u, new Vector3(527.7325f, -2626.933f, -49.99977f), new Vector3(0f, -0f, -90.512764f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_prtmachine_dryer_spin"), 499481, 769275872u, new Vector3(527.7659f, -2624.31348f, -49.99977f), new Vector3(0f, -0f, -90.512764f), 735494, 0, new Vector3(-1.30000007f, 0.01f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_prtmachine_dryer_spin"), 735494, 769275872u, new Vector3(527.7443f, -2625.61328f, -49.99977f), new Vector3(0f, -0f, -90.512764f), 227866, 0, new Vector3(-1.3199991f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects Objects_Loot = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 229678, 1228147776u, new Vector3(514.9029f, -2624.35547f, -49.1458664f), new Vector3(0f, -0f, -179.167389f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 89631, 1228147776u, new Vector3(515.145447f, -2617.70435f, -49.19203f), new Vector3(0f, -0f, 90.49789f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 55050, 1228147776u, new Vector3(515.1982f, -2614.66919f, -49.19203f), new Vector3(0f, -0f, 90.65112f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 217374, 1228147776u, new Vector3(515.1711f, -2610.98169f, -49.19203f), new Vector3(0f, -0f, 91.00908f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_cash_trolly_01c"), 41498, 412463629u, new Vector3(514.8541f, -2639.49585f, -49.5260277f), new Vector3(0f, -0f, -359.953979f), 747526, 0, new Vector3(-1.44130623f, -0.03896907f, 0.472621918f), new Vector3(0f, 0f, -359.695679f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_cash_trolly_01c"), 728838, 412463629u, new Vector3(512.081f, -2636.40967f, -49.5260277f), new Vector3(0f, -0f, -89.88016f), 748038, 0, new Vector3(1.521602f, 0.0150633166f, 0.472621918f), new Vector3(0f, 0f, 0.04824066f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_cash_trolly_01c"), 163590, 412463629u, new Vector3(512.073853f, -2633.469f, -49.5260277f), new Vector3(0f, -0f, -89.88016f), 728070, 0, new Vector3(1.48112845f, 0.05276879f, 0.472621918f), new Vector3(0f, -0f, 1.0719223f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 165398, 1228147776u, new Vector3(516.380554f, -2639.4292f, -49.1458664f), new Vector3(0f, -0f, 179.0697f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 728582, 1228147776u, new Vector3(513.3527f, -2639.393f, -49.1458664f), new Vector3(0f, -0f, 178.966812f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 747782, 1228147776u, new Vector3(512.2278f, -2637.897f, -49.190834f), new Vector3(0f, -0f, 90.1147f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 225048, 1228147776u, new Vector3(512.2362f, -2634.91357f, -49.1458664f), new Vector3(0f, 0f, 89.9097061f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 306707, 1228147776u, new Vector3(512.269165f, -2631.95044f, -49.1458664f), new Vector3(0f, 0f, 89.39669f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 55559, 1228147776u, new Vector3(517.2409f, -2634.50269f, -49.1471252f), new Vector3(0f, -0f, -90.14906f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_cash_trolly_01c"), 740358, 412463629u, new Vector3(512.9231f, -2606.7168f, -49.5260277f), new Vector3(0f, -0f, -0.894943237f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 741638, 1228147776u, new Vector3(514.382446f, -2606.583f, -49.1471252f), new Vector3(0f, -0f, 179.463135f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_bkr_cashpile_01"), 56583, 2596205386u, new Vector3(527.274536f, -2628.28174f, -49.14616f), new Vector3(-5.08888721E-14f, -9.316806E-07f, 171.113312f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_bkr_cashpile_04"), 56071, 4097844811u, new Vector3(526.4244f, -2628.17944f, -49.1423721f), new Vector3(0f, 1.09708719E-06f, 163.076965f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_bkr_cashpile_04"), 52231, 4097844811u, new Vector3(524.2873f, -2628.195f, -49.1423721f), new Vector3(0f, -0f, 178.789322f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_cashtrolley_01a"), 385305, 3178809378u, new Vector3(516.920837f, -2631.56128f, -49.99917f), new Vector3(0f, 0f, 0.4861929f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_cashtrolley_01a"), 483098, 3178809378u, new Vector3(517.0802f, -2630.47217f, -49.99917f), new Vector3(0f, 0f, 88.77407f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_safe_01a"), 89369, 1098122770u, new Vector3(520.387634f, -2606.045f, -49.99991f), new Vector3(0f, -0f, -90.4068756f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"), 54023, 1228147776u, new Vector3(521.6353f, -2628.3042f, -49.1458664f), new Vector3(0f, -0f, -176.2497f), 57607, 0, new Vector3(0.05018145f, -0.0173665583f, 0.8527832f), new Vector3(0f, 0f, -357.34137f))
		});

		public static List<Prop> Cameras = new List<Prop>();

		public static bool StolenCCTVHDD;

		public static bool RequireEscapeWithStockade;

		public static bool LostWantedLevelWithStockade;

		public int LeverSyncScene;

		public int pullLeverStage;

		public bool CreateInt;

		public static bool HackingKeypad = false;

		public static Prop LastKeypad;

		public static Prop USB;

		public static Prop Phone;

		public int Scene1;

		public static KeypadClass MainVaultKeypad;

		public static bool HackedMainKeypad;

		public static Prop MoneyProp;

		public static int GrabHDDSyncScene;

		public static Vector3 DeliverTrolleyLoc;

		public int LSDW_MultistoreyGarage_Style = 2;

		public int LSDW_MultistoreyGarage_SecondaryColor = 6;

		public int LSDW_MultistoreyGarage_PrimaryColor = 1;

		public int LSDW_MultistoreyGarage_Number;

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

		public ThePrinthouseHeist()
		{
			base.Tick += OnTick;
		}

		public static void StartHeist()
		{
			PercentNeedTake = (int)((float)(HeistMaxPay + HeistMinPay) / 1.5f / 100f * 85f);
			TakePerMoneyCrate = (int)((float)(HeistMinPay + HeistMaxPay) / 1.5f / 100f * 100f);
			ManageRobberyAnims.CurrentPay = 0;
			ManageRobberyAnims.Heist_Average_Pay = (float)(HeistMinPay + HeistMaxPay) / 2f * 1.2f;
			Function.Call(Hash.BEGIN_TEXT_COMMAND_BUSYSPINNER_ON, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "Loading The Printhouse Heist, Please Wait");
			Function.Call(Hash.END_TEXT_COMMAND_BUSYSPINNER_ON, 5);
			EndHeist();
			PrinthouseInterior.CreateEntites();
			Objects_VaultDoor.CreateEntites();
			Objects_Cameras.CreateEntites();
			Objects_Loot.CreateEntites();
			if (Main.ReturnGameBuild() >= 3095)
			{
				Objects_SecondaryLoot.CreateEntites();
			}
			else
			{
				Objects_SecondaryLootFake.CreateEntites();
			}
			ThePrinthouseHeistOn = true;
			CurrentHeistStage = Heiststage.HeadingToPrinthouse;
			EnterMarker = PrinthouseInterior.Entities[0].Position;
			PrinthouseInterior.Entities[0].Entity.Opacity = 0;
			ExitMarker = PrinthouseInterior.Entities[1].Position;
			PrinthouseInterior.Entities[1].Entity.Opacity = 0;
			VaultDoor = (Prop)PrinthouseInterior.Entities[2].Entity;
			MCStorage_DoorL = (Prop)PrinthouseInterior.Entities[3].Entity;
			MCStorage_DoorR = (Prop)PrinthouseInterior.Entities[4].Entity;
			MainVaultKeypad = new KeypadClass((Prop)PrinthouseInterior.Entities[5].Entity, 1);
			HDDCrate = (Prop)PrinthouseInterior.Entities[6].Entity;
			int index = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, Objects_Levers.Entities.Count);
			Objects_Levers.Entities[index].CreateObjatCoords();
			LeverBox = (Prop)Objects_Levers.Entities[index].Entity;
			PushableTrolleyA = new ManageRobberyAnims.PushableTrolley((Prop)PrinthouseInterior.Entities[7].Entity);
			foreach (ObjectPlacementMap entity in PrinthouseInterior.Entities)
			{
				if (entity.Entity != null && entity.Entity.IsAttachedTo(PrinthouseInterior.Entities[7].Entity))
				{
					PushableTrolleyA.AttachedEntities.Add(entity.Entity);
				}
			}
			PushableTrolleyB = new ManageRobberyAnims.PushableTrolley((Prop)PrinthouseInterior.Entities[8].Entity);
			foreach (ObjectPlacementMap entity2 in PrinthouseInterior.Entities)
			{
				if (entity2.Entity != null && entity2.Entity.IsAttachedTo(PrinthouseInterior.Entities[8].Entity))
				{
					PushableTrolleyB.AttachedEntities.Add(entity2.Entity);
				}
			}
			Stockade = (Vehicle)PrinthouseInterior.Entities[9].Entity;
			Function.Call(Hash.BUSYSPINNER_OFF);
			Stockade.AddBlip();
			Stockade.AttachedBlip.Color = BlipColor.BlueDark;
			Stockade.AttachedBlip.IsShortRange = true;
			Stockade.AttachedBlip.Name = "Stockade";
			if (MainBlip != null)
			{
				MainBlip.Delete();
			}
			MainBlip = GlobalHelper.CreateBlip(EnterMarker);
			MainBlip.Color = BlipColor.Yellow2;
			MainBlip.IsShortRange = true;
			MainBlip.Name = "Enter the Printhouse";
		}

		public static void EndHeist()
		{
			Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
			if (MainVaultKeypad != null && MainVaultKeypad.Keypad != null && MainVaultKeypad.Keypad.AttachedBlip != null)
			{
				MainVaultKeypad.Keypad.AttachedBlip.Delete();
			}
			ManageRobberyAnims.DeleteLoot();
			SmartGuards.ClearGenericGuards();
			ThePrinthouseHeistOn = false;
			VaultOpen = false;
			MoneyCrateStorageOpen = false;
			Objects_SecondaryLoot.DeleteEntites();
			Objects_SecondaryLootFake.DeleteEntites();
			MoneyCrate1inStockade = false;
			MoneyCrate2inStockade = false;
			if (Phone != null)
			{
				Phone.Delete();
			}
			if (USB != null)
			{
				USB.Delete();
			}
			if (MoneyProp != null)
			{
				MoneyProp.Delete();
			}
			StolenCCTVHDD = false;
			HackedMainKeypad = false;
			if (MainBlip != null)
			{
				MainBlip.Delete();
			}
			PrinthouseInterior.DeleteEntites();
			Objects_VaultDoor.DeleteEntites();
			Objects_Guards.DeleteEntites();
			Objects_Levers.DeleteEntites();
			Objects_Cameras.DeleteEntites();
			Objects_Loot.DeleteEntites();
		}

		public void ManageCameras()
		{
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
		}

		public bool CamerasDestroyed()
		{
			bool result = true;
			foreach (Prop camera in Cameras)
			{
				if (camera.Health >= 997)
				{
					result = false;
				}
			}
			return result;
		}

		public void ManageKeypad()
		{
			if (!HackedMainKeypad)
			{
				if (MainVaultKeypad != null && MainVaultKeypad.Keypad != null)
				{
					CheckKeypad(MainVaultKeypad);
				}
			}
			else if (VaultDoor != null && VaultDoor.Rotation.Z > -90f)
			{
				if (VaultDoor.IsAttached())
				{
					VaultDoor.Detach();
				}
				VaultDoor.IsPositionFrozen = true;
				float z = VaultDoor.Rotation.Z;
				VaultDoor.Rotation = new Vector3(VaultDoor.Rotation.X, VaultDoor.Rotation.Y, z -= 0.25f);
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
							HackingKeypad = false;
							KeyPad.AwaitingHackGUI = false;
							LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
							Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, LastKeypad.Position.X, LastKeypad.Position.Y, LastKeypad.Position.Z, LastKeypad.Rotation.X, LastKeypad.Rotation.Y, LastKeypad.Rotation.Z, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit", 0.0, 0.0, 0, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "exit_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "exit_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
							Script.Wait(4000);
							Game.Player.Character.Task.ClearAllImmediately();
							if (Phone != null)
							{
								Phone.Delete();
							}
							if (USB != null)
							{
								USB.Delete();
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

		public float GetDistance(Vector3 _1, Vector3 _2)
		{
			return _1.DistanceTo(_2);
		}

		public static void LOADIPL(string IPL)
		{
			Function.Call(Hash.REQUEST_IPL, IPL);
		}

		private void OnTick(object sender, EventArgs e)
		{
			TakePerMoneyCrate = (int)((float)(HeistMinPay + HeistMaxPay) / 1.5f / 100f * 45f);
			if (!ThePrinthouseHeistOn || !ThePrinthouseHeistOn)
			{
				return;
			}
			if (!MoneyCrateStorageOpen)
			{
				Vector3 position = Game.Player.Character.Position;
				LeverBox = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_lever_box_01b"), 0, 0, 0);
				if (LeverBox != null && GetDistance_VDIST2(Game.Player.Character.Position, LeverBox.Position, "<", 1.5f))
				{
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Open Money Crate Storage", 1);
					if (Game.IsControlJustPressed(Control.Context))
					{
						LoadDict("anim@scripted@heist@ig2_hand_lever@male@");
						LeverSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, LeverBox.Position.X, LeverBox.Position.Y, LeverBox.Position.Z, 0.0, 0.0, LeverBox.Rotation.Z, 2);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, LeverSyncScene, 0.0);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, LeverSyncScene, LoadDict("anim@scripted@heist@ig2_hand_lever@male@"), "enter", 1000.0, -4.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, LeverSyncScene, "enter", LoadDict("anim@scripted@heist@ig2_hand_lever@male@"), 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, LeverBox, LeverSyncScene, "enter_lever", LoadDict("anim@scripted@heist@ig2_hand_lever@male@"), 16f, -16f, 0, 1148846080);
						pullLeverStage = 1;
					}
				}
				if (pullLeverStage == 1 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, LeverSyncScene) >= 1065353216)
				{
					LeverSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, LeverBox.Position.X, LeverBox.Position.Y, LeverBox.Position.Z, 0.0, 0.0, LeverBox.Rotation.Z, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, LeverSyncScene, 0.0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, LeverSyncScene, LoadDict("anim@scripted@heist@ig2_hand_lever@male@"), "exit", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, LeverSyncScene, "exit", LoadDict("anim@scripted@heist@ig2_hand_lever@male@"), 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, LeverBox, LeverSyncScene, "exit_lever", LoadDict("anim@scripted@heist@ig2_hand_lever@male@"), 16f, -16f, 0, 1148846080);
					pullLeverStage = 2;
				}
				if (pullLeverStage == 2 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, LeverSyncScene) >= 1065353216)
				{
					if (VaultOpen && !MoneyCrateStorageOpen)
					{
						CurrentHeistStage = Heiststage.VaultOpenAndCrateStorageNotOpen;
					}
					if (!VaultOpen && MoneyCrateStorageOpen)
					{
						CurrentHeistStage = Heiststage.VaultNotOpenAndCrateStorageOpen;
					}
					if (VaultOpen && MoneyCrateStorageOpen)
					{
						CurrentHeistStage = Heiststage.VaultOpenAndCrateStorageOpen;
					}
					MoneyCrateStorageOpen = true;
					pullLeverStage = 0;
					Game.Player.Character.Task.ClearAllImmediately();
				}
			}
			if (HackedMainKeypad)
			{
				VaultOpen = true;
			}
			if (HDDCrate != null && !StolenCCTVHDD)
			{
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, GrabHDDSyncScene) >= 1065353216)
				{
					StolenCCTVHDD = true;
					Game.Player.Character.Task.ClearAllImmediately();
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, HDDCrate.Position, "<", 2f))
				{
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Grab CCTV HDD");
					if (Game.IsControlJustPressed(Control.Context))
					{
						LoadDict("anim@GangOps@VAN@DRIVE_GRAB@");
						if (MoneyProp != null)
						{
							MoneyProp.Delete();
						}
						MoneyProp = GlobalHelper.CreateProp(RequestModel("prop_cs_server_drive"), Game.Player.Character.Position, false, false);
						MoneyProp.IsCollisionEnabled = false;
						MoneyProp.IsInvincible = true;
						string dict = "anim@scripted@player@fix_club_ig1_hdd_remove@male@";
						LoadDict(dict);
						Vector3 position2 = HDDCrate.Position;
						float heading = HDDCrate.Heading;
						GrabHDDSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position2.X, position2.Y, position2.Z, 0.0, 0.0, heading, 2);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, GrabHDDSyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, GrabHDDSyncScene));
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, GrabHDDSyncScene, LoadDict("anim@scripted@player@fix_club_ig1_hdd_remove@male@"), "action", 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, GrabHDDSyncScene, "action", LoadDict("anim@scripted@player@fix_club_ig1_hdd_remove@male@"), 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, GrabHDDSyncScene, "action_drive", LoadDict("anim@scripted@player@fix_club_ig1_hdd_remove@male@"), 16f, -16f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, HDDCrate, GrabHDDSyncScene, "action_Server", LoadDict("anim@scripted@player@fix_club_ig1_hdd_remove@male@"), 16f, -16f, 0, 1148846080);
					}
				}
			}
			if (MoneyCrateStorageOpen)
			{
				if (MCStorage_DoorL != null)
				{
					if (MCStorage_DoorL.IsAttached())
					{
						MCStorage_DoorL.Detach();
					}
					if (MCStorage_DoorL.Rotation.Z < 90f)
					{
						if (MCStorage_DoorL.IsAttached())
						{
							MCStorage_DoorL.Detach();
						}
						MCStorage_DoorL.IsPositionFrozen = true;
						float z = VaultDoor.Rotation.Z;
						MCStorage_DoorL.Rotation = new Vector3(MCStorage_DoorL.Rotation.X, MCStorage_DoorL.Rotation.Y, z -= 0.025f);
					}
				}
				if (MCStorage_DoorR != null)
				{
					if (MCStorage_DoorR.IsAttached())
					{
						MCStorage_DoorR.Detach();
					}
					if (MCStorage_DoorR.Rotation.Z > 90f)
					{
						if (MCStorage_DoorR.IsAttached())
						{
							MCStorage_DoorR.Detach();
						}
						MCStorage_DoorR.IsPositionFrozen = true;
						float z2 = VaultDoor.Rotation.Z;
						MCStorage_DoorR.Rotation = new Vector3(MCStorage_DoorR.Rotation.X, MCStorage_DoorR.Rotation.Y, z2 -= 0.025f);
					}
				}
			}
			else
			{
				bool flag = MCStorage_DoorL != null;
				bool flag2 = MCStorage_DoorR != null;
			}
			if (Stockade != null)
			{
				DeliverTrolleyLoc = Stockade.GetOffsetPosition(new Vector3(0f, -5.25f, -0.4f));
				World.DrawMarker(MarkerType.Cylinder, DeliverTrolleyLoc, Vector3.Zero, Vector3.Zero, new Vector3(2.75f, 2.75f, 0.25f), Color.Goldenrod);
			}
			if (MoneyCrateStorageOpen)
			{
				if (PushableTrolleyA != null)
				{
					PushableTrolleyA.Update();
				}
				if (PushableTrolleyB != null)
				{
					PushableTrolleyB.Update();
				}
			}
			SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Gruppe_Sechs;
			Heiststage currentHeistStage = CurrentHeistStage;
			UI.RenderQue = 0f;
			switch (currentHeistStage)
			{
			case Heiststage.HeadingToPrinthouse:
				if (Stockade != null && Stockade.AttachedBlip != null)
				{
					Stockade.AttachedBlip.Alpha = 0;
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, EnterMarker, "<", 30f))
				{
					Screen.ShowSubtitle("Enter the ~y~Printhouse");
					if (!GetDistance_VDIST2(Game.Player.Character.Position, EnterMarker, "<", 100f))
					{
						break;
					}
					World.DrawMarker(MarkerType.Cylinder, EnterMarker, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
					if (!GetDistance_VDIST2(Game.Player.Character.Position, EnterMarker, "<", 2f))
					{
						break;
					}
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter The Printhouse");
					if (!Game.IsControlJustPressed(Control.Context))
					{
						break;
					}
					StolenCCTVHDD = false;
					LOADIPL("xm3_int_placement_xm3_interior_1_dlc_int_02_xm3_milo_");
					int num2 = 290561;
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "entity_set_numbers_01");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "entity_set_numbers_02");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "entity_set_numbers_03");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "entity_set_numbers_04");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "entity_set_numbers_05");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "entity_set_shell_01");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "entity_set_shell_02");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "entity_set_shell_03");
					Function.Call(Hash.REFRESH_INTERIOR, num2);
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "entity_set_tint_01");
					Function.Call((Hash)3590001984630001091uL, num2, "entity_set_tint_01", LSDW_MultistoreyGarage_SecondaryColor);
					if (LSDW_MultistoreyGarage_Style == 0)
					{
						Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "entity_set_shell_01");
						Function.Call((Hash)3590001984630001091uL, num2, "entity_set_shell_01", LSDW_MultistoreyGarage_PrimaryColor);
					}
					if (LSDW_MultistoreyGarage_Style == 1)
					{
						Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "entity_set_shell_02");
						Function.Call((Hash)3590001984630001091uL, num2, "entity_set_shell_02", LSDW_MultistoreyGarage_PrimaryColor);
					}
					if (LSDW_MultistoreyGarage_Style == 2)
					{
						Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "entity_set_shell_03");
						Function.Call((Hash)3590001984630001091uL, num2, "entity_set_shell_03", LSDW_MultistoreyGarage_PrimaryColor);
					}
					if (LSDW_MultistoreyGarage_Number == 0)
					{
						Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "entity_set_numbers_01");
					}
					if (LSDW_MultistoreyGarage_Number == 1)
					{
						Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "entity_set_numbers_02");
					}
					if (LSDW_MultistoreyGarage_Number == 2)
					{
						Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "entity_set_numbers_03");
					}
					if (LSDW_MultistoreyGarage_Number == 3)
					{
						Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "entity_set_numbers_04");
					}
					if (LSDW_MultistoreyGarage_Number == 4)
					{
						Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "entity_set_numbers_05");
					}
					Function.Call(Hash.REFRESH_INTERIOR, num2);
					if (Main.ReturnGameBuild() >= 3095)
					{
						foreach (ObjectPlacementMap entity in Objects_SecondaryLoot.Entities)
						{
							if (entity.Entity != null)
							{
								ManageRobberyAnims.AddPropAsLoot((Prop)entity.Entity);
							}
						}
					}
					foreach (ObjectPlacementMap entity2 in Objects_Loot.Entities)
					{
						if (entity2.Entity != null)
						{
							ManageRobberyAnims.AddPropAsLoot((Prop)entity2.Entity);
						}
					}
					foreach (ObjectPlacementMap entity3 in Objects_Cameras.Entities)
					{
						if (entity3.Entity != null)
						{
							Prop prop = (Prop)entity3.Entity;
							Cameras.Add(prop);
							prop.AddBlip();
							prop.AttachedBlip.Sprite = BlipSprite.Standard;
							prop.AttachedBlip.Color = BlipColor.Red2;
							prop.AttachedBlip.Name = "Camera";
							prop.AttachedBlip.Scale = 0.5f;
						}
					}
					if (MainBlip != null)
					{
						MainBlip.Alpha = 0;
					}
					Screen.FadeOut(1500);
					Script.Wait(1500);
					IsInInterior = true;
					Game.Player.Character.Position = ExitMarker;
					CurrentHeistStage = Heiststage.VaultNotOpenAndCrateStorageNotOpen;
					Script.Wait(1500);
					Screen.FadeIn(1500);
					SmartGuards.GetCurrentGroupSettings().AllowDespawnIfTooFarAwayThisFrame = false;
					for (int i = 0; i < Objects_Guards.Entities.Count; i++)
					{
						int num3 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100);
						if (num3 >= 50)
						{
							Objects_Guards.Entities[i].CreateObjatCoords();
							Ped ped = (Ped)Objects_Guards.Entities[i].Entity;
							ped.IsPositionFrozen = false;
							SmartGuards.GuardGroup[27].GroupList.Add(new SmartGuards.SmartAiV2(ped, SmartGuards.EnemyGroup.Gruppe_Sechs));
						}
					}
				}
				else
				{
					Screen.ShowSubtitle("Make your way to the ~y~Printhouse");
				}
				break;
			case Heiststage.VaultNotOpenAndCrateStorageNotOpen:
				if (Stockade != null && Stockade.AttachedBlip != null)
				{
					Stockade.AttachedBlip.Alpha = 0;
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
				ManageCameras();
				ManageKeypad();
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
				Game.Player.WantedLevel = 0;
				Screen.ShowSubtitle("Find a way to open the ~y~Vault~w~ or ~y~Money Crate Storage");
				if (VaultOpen && !MoneyCrateStorageOpen)
				{
					CurrentHeistStage = Heiststage.VaultOpenAndCrateStorageNotOpen;
				}
				if (!VaultOpen && MoneyCrateStorageOpen)
				{
					CurrentHeistStage = Heiststage.VaultNotOpenAndCrateStorageOpen;
				}
				if (VaultOpen && MoneyCrateStorageOpen)
				{
					CurrentHeistStage = Heiststage.VaultOpenAndCrateStorageOpen;
				}
				break;
			case Heiststage.VaultOpenAndCrateStorageNotOpen:
				if (Stockade != null && Stockade.AttachedBlip != null)
				{
					Stockade.AttachedBlip.Alpha = 0;
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
				ManageCameras();
				ManageKeypad();
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
				Game.Player.WantedLevel = 0;
				Screen.ShowSubtitle("Steal at least ~g~" + PercentNeedTake.ToString("N") + "~w~ or find a way to open the ~y~Money Crate Storage");
				if (VaultOpen && MoneyCrateStorageOpen)
				{
					CurrentHeistStage = Heiststage.VaultOpenAndCrateStorageOpen;
				}
				if (PushableTrolleyA != null && PushableTrolleyB != null && (PushableTrolleyA.IsBeingPushed || PushableTrolleyB.IsBeingPushed))
				{
					CurrentHeistStage = Heiststage.PushingCrate;
				}
				if (ManageRobberyAnims.CurrentPay >= PercentNeedTake && !MoneyCrateStorageOpen)
				{
					CurrentHeistStage = Heiststage.StealCCTVHDD;
				}
				break;
			case Heiststage.VaultNotOpenAndCrateStorageOpen:
				if (Stockade != null && Stockade.AttachedBlip != null)
				{
					Stockade.AttachedBlip.Alpha = 255;
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
				ManageCameras();
				ManageKeypad();
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
				Game.Player.WantedLevel = 0;
				Screen.ShowSubtitle("Find a way to open the ~y~Vault~w~ or push one/both ~y~Money Crate(s)~w~ into the ~d~Stockade");
				if (VaultOpen && MoneyCrateStorageOpen)
				{
					CurrentHeistStage = Heiststage.VaultOpenAndCrateStorageOpen;
				}
				if (PushableTrolleyA != null && PushableTrolleyB != null && (PushableTrolleyA.IsBeingPushed || PushableTrolleyB.IsBeingPushed))
				{
					CurrentHeistStage = Heiststage.PushingCrate;
				}
				break;
			case Heiststage.VaultOpenAndCrateStorageOpen:
				if (Stockade != null && Stockade.AttachedBlip != null)
				{
					Stockade.AttachedBlip.Alpha = 255;
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
				ManageCameras();
				ManageKeypad();
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
				Game.Player.WantedLevel = 0;
				Screen.ShowSubtitle("Loot the ~y~Vault~w~ for ~g~Vaulables~w~ or push one/both ~y~Money Crate(s)~w~ into the ~d~Stockade");
				if (PushableTrolleyA != null && PushableTrolleyB != null && (PushableTrolleyA.IsBeingPushed || PushableTrolleyB.IsBeingPushed))
				{
					CurrentHeistStage = Heiststage.PushingCrate;
				}
				if (PushableTrolleyA.CannotBePushed || PushableTrolleyB.CannotBePushed)
				{
					CurrentHeistStage = Heiststage.LoadedAtLeastOneCrate;
				}
				break;
			case Heiststage.PushingCrate:
				if (Stockade != null && Stockade.AttachedBlip != null)
				{
					Stockade.AttachedBlip.Alpha = 255;
					Stockade.AttachedBlip.IsFlashing = true;
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
				ManageCameras();
				ManageKeypad();
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
				Game.Player.WantedLevel = 0;
				Screen.ShowSubtitle("push one/both ~y~Money Crate(s)~w~ into the ~d~Stockade");
				if (PushableTrolleyA != null && PushableTrolleyA.Trolley != null && !PushableTrolleyA.CannotBePushed && PushableTrolleyA.IsBeingPushed && GetDistance_VDIST2(PushableTrolleyA.Trolley.Position, DeliverTrolleyLoc, "<", 1f))
				{
					PushableTrolleyA.IsBeingPushed = false;
					EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(PushableTrolleyA.Trolley, 1));
					foreach (Entity attachedEntity in PushableTrolleyA.AttachedEntities)
					{
						EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(attachedEntity, 1));
					}
					PushableTrolleyA.CannotBePushed = true;
					Game.Player.Character.Detach();
					Game.Player.Character.Task.PlayAnimation("missbigscore2aig_5", "push_trolly_exit", 1f, 5000, AnimationFlags.None);
					ManageRobberyAnims.CurrentPay += TakePerMoneyCrate;
					MoneyCrate1inStockade = true;
				}
				if (PushableTrolleyB != null && PushableTrolleyB.Trolley != null && PushableTrolleyB.IsBeingPushed && !PushableTrolleyB.CannotBePushed && GetDistance_VDIST2(PushableTrolleyB.Trolley.Position, DeliverTrolleyLoc, "<", 1f))
				{
					PushableTrolleyB.IsBeingPushed = false;
					EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(PushableTrolleyB.Trolley, 1));
					foreach (Entity attachedEntity2 in PushableTrolleyB.AttachedEntities)
					{
						EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(attachedEntity2, 1));
					}
					PushableTrolleyB.CannotBePushed = true;
					Game.Player.Character.Detach();
					Game.Player.Character.Task.PlayAnimation("missbigscore2aig_5", "push_trolly_exit", 1f, 5000, AnimationFlags.None);
					ManageRobberyAnims.CurrentPay += TakePerMoneyCrate;
					MoneyCrate2inStockade = true;
				}
				if (!PushableTrolleyA.IsBeingPushed && !PushableTrolleyB.IsBeingPushed)
				{
					if (!MoneyCrate1inStockade && !MoneyCrate2inStockade)
					{
						CurrentHeistStage = Heiststage.VaultOpenAndCrateStorageOpen;
					}
					else
					{
						CurrentHeistStage = Heiststage.LoadedAtLeastOneCrate;
					}
				}
				break;
			case Heiststage.LoadedAtLeastOneCrate:
				if (!StolenCCTVHDD)
				{
					if (MainBlip != null)
					{
						MainBlip.Position = HDDCrate.Position;
						MainBlip.Color = BlipColor.Green;
						MainBlip.IsShortRange = true;
						MainBlip.Name = "Steal CCTV HDD";
						MainBlip.Alpha = 255;
					}
				}
				else if (MainBlip != null)
				{
					MainBlip.Alpha = 0;
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
				ManageCameras();
				ManageKeypad();
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
				Game.Player.WantedLevel = 0;
				if (MoneyCrateStorageOpen)
				{
					if (Stockade != null && Stockade.AttachedBlip != null)
					{
						Stockade.AttachedBlip.Alpha = 255;
						Stockade.AttachedBlip.IsFlashing = false;
					}
					if (PushableTrolleyA.IsBeingPushed || PushableTrolleyB.IsBeingPushed)
					{
						CurrentHeistStage = Heiststage.PushingCrate;
					}
					if (VaultOpen && !StolenCCTVHDD)
					{
						Screen.ShowSubtitle("Loot the ~y~Vault~w~ for ~g~Vaulables~w~ or push the second ~y~Money Crate(s)~w~ into the ~d~Stockade~w~ or steal the ~g~CCTVs HDD~w~");
					}
					if (VaultOpen && StolenCCTVHDD)
					{
						Screen.ShowSubtitle("Loot the ~y~Vault~w~ for ~g~Vaulables~w~ or push the second ~y~Money Crate(s)~w~ into the ~d~Stockade~w~ or leave the ~y~Printhouse");
					}
					if (!VaultOpen && !StolenCCTVHDD)
					{
						Screen.ShowSubtitle("Find a way to open the ~y~Vault~w~ or push the second ~y~Money Crate(s)~w~ into the ~d~Stockade~w~ or steal the ~g~CCTVs HDD~w~");
					}
					if (!VaultOpen && StolenCCTVHDD)
					{
						Screen.ShowSubtitle("Find a way to open the ~y~Vault~w~ or push the second ~y~Money Crate(s)~w~ into the ~d~Stockade~w~ or leave the ~y~Printhouse");
					}
				}
				else
				{
					Screen.ShowSubtitle("Steal the ~g~CCTVs HDD~w~ to cover your tracks");
				}
				if (HDDCrate != null && !StolenCCTVHDD && GetDistance_VDIST2(Game.Player.Character.Position, HDDCrate.Position, "<", 5f))
				{
					CurrentHeistStage = Heiststage.StealCCTVHDD;
				}
				if (PushableTrolleyA != null && PushableTrolleyB != null && (PushableTrolleyA.IsBeingPushed || PushableTrolleyB.IsBeingPushed))
				{
					CurrentHeistStage = Heiststage.PushingCrate;
				}
				if (StolenCCTVHDD)
				{
					CurrentHeistStage = Heiststage.StealCCTVHDD;
				}
				break;
			case Heiststage.StealCCTVHDD:
				if (Stockade != null && Stockade.AttachedBlip != null)
				{
					Stockade.AttachedBlip.Alpha = 0;
					Stockade.AttachedBlip.IsFlashing = false;
				}
				ManageCameras();
				ManageKeypad();
				if (!StolenCCTVHDD)
				{
					if (MainBlip != null)
					{
						MainBlip.Position = HDDCrate.Position;
						MainBlip.Color = BlipColor.Green;
						MainBlip.IsShortRange = true;
						MainBlip.Name = "Steal CCTV HDD";
						MainBlip.Alpha = 255;
					}
				}
				else if (MainBlip != null)
				{
					MainBlip.Alpha = 0;
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
				if (GetDistance_VDIST2(Game.Player.Character.Position, HDDCrate.Position, ">", 5f))
				{
					if (!StolenCCTVHDD && !MoneyCrateStorageOpen)
					{
						CurrentHeistStage = Heiststage.LoadedAtLeastOneCrate;
					}
					else
					{
						Screen.ShowSubtitle("Steal the ~g~CCTVs HDD~w~ to cover your tracks");
					}
				}
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
				Game.Player.WantedLevel = 0;
				if (!StolenCCTVHDD)
				{
					Screen.ShowSubtitle("Steal the ~g~CCTVs HDD~w~ to cover your tracks");
				}
				else if (MoneyCrate1inStockade || MoneyCrate2inStockade)
				{
					CurrentHeistStage = Heiststage.LeaveWithStockade;
				}
				else if (CamerasDestroyed())
				{
					Screen.ShowSubtitle("Leave the ~y~Printhouse");
					if (!GetDistance_VDIST2(Game.Player.Character.Position, ExitMarker, "<", 100f))
					{
						break;
					}
					World.DrawMarker(MarkerType.Cylinder, ExitMarker, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
					if (GetDistance_VDIST2(Game.Player.Character.Position, ExitMarker, "<", 2f))
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit The Printhouse");
						if (Game.IsControlJustPressed(Control.Context))
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
							Game.Player.WantedLevel = 5;
							Screen.FadeOut(1500);
							Script.Wait(1500);
							IsInInterior = true;
							Game.Player.Character.Position = EnterMarker;
							CurrentHeistStage = Heiststage.EscapeCops;
							Script.Wait(1500);
							Screen.FadeIn(1500);
						}
					}
				}
				else
				{
					Screen.ShowSubtitle("Destroy All the ~r~CCTV Cameras");
				}
				break;
			case Heiststage.LeaveWithStockade:
				ManageCameras();
				ManageKeypad();
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
				if (!CamerasDestroyed())
				{
					if (Stockade != null && Stockade.AttachedBlip != null)
					{
						Stockade.AttachedBlip.Alpha = 0;
						Stockade.AttachedBlip.IsFlashing = false;
					}
					Screen.ShowSubtitle("Destroy All the ~r~CCTV Cameras");
					return;
				}
				if (Stockade != null && Stockade.AttachedBlip != null)
				{
					Stockade.AttachedBlip.Alpha = 255;
					Stockade.AttachedBlip.IsFlashing = false;
				}
				if (Game.Player.Character.CurrentVehicle != null)
				{
					if (Game.Player.Character.CurrentVehicle == Stockade)
					{
						Screen.ShowSubtitle("Exit the ~y~Printhouse~w~ with the ~d~Stockade");
						RequireEscapeWithStockade = true;
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit The Printhouse With ~d~Stockade");
						if (Game.IsControlJustPressed(Control.Context))
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
							Game.Player.WantedLevel = 5;
							Screen.FadeOut(1500);
							Script.Wait(1500);
							IsInInterior = false;
							Stockade.Position = new Vector3(-247.68f, -853.04f, 29.96f);
							Stockade.Heading = 162f;
							CurrentHeistStage = Heiststage.EscapeCops;
							Script.Wait(1500);
							Screen.FadeIn(1500);
						}
					}
					else
					{
						Screen.ShowSubtitle("Enter the ~d~Stockade");
					}
				}
				else
				{
					Screen.ShowSubtitle("Enter the ~d~Stockade");
				}
				break;
			case Heiststage.EscapeCops:
				if (Stockade != null && Stockade.AttachedBlip != null)
				{
					Stockade.AttachedBlip.Alpha = 255;
					Stockade.AttachedBlip.IsFlashing = false;
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
				if (RequireEscapeWithStockade)
				{
					Screen.ShowSubtitle("Escape the ~r~cops~w~ and deliver the ~d~Stockade~w~ to the ~y~Arcade");
					if (Stockade != null && GetDistance_VDIST2(Stockade.Position, Main.GetArcadeLoc(), "<", 10f))
					{
						Main.TotalCrewEarned += ManageRobberyAnims.CurrentPay;
						Main.TotalPlayerEarned += 0f;
						XP.CompleteHeist_ThePrinthouseHeist++;
						Main.SaveCuts();
						Class1.SetNoLongerInHeist();
						MissionScreen.SetPass("The Printhouse Heist", ManageRobberyAnims.CurrentPay, 0, "The Player has died", 2, 0f, 5);
						int num = Game.GameTime + 30000;
						while (HKHModHelperNew.Class1.HeistPassedScreen != -1 && Game.GameTime <= num)
						{
							Script.Wait(0);
						}
						Script.Wait(2000);
						ManagePlayerFunds.AddFunds(ManageRobberyAnims.CurrentPay);
						XP.AddXP_Percent(XP.CompleteHeist_XPPercent_ThePrinthouseHeist);
						EndHeist();
					}
					break;
				}
				Screen.ShowSubtitle("Escape the ~r~cops");
				if (Game.Player.WantedLevel == 0)
				{
					Main.TotalCrewEarned += ManageRobberyAnims.CurrentPay;
					Main.TotalPlayerEarned += 0f;
					XP.CompleteHeist_ThePrinthouseHeist++;
					Main.SaveCuts();
					MissionScreen.SetPass("The Printhouse Heist", ManageRobberyAnims.CurrentPay, 0, "The Player has died", 2, 0f, 5);
					while (HKHModHelperNew.Class1.HeistPassedScreen != -1)
					{
						Script.Wait(0);
					}
					Script.Wait(2000);
					ManagePlayerFunds.AddFunds(ManageRobberyAnims.CurrentPay);
					XP.AddXP_Percent(XP.CompleteHeist_XPPercent_ThePrinthouseHeist);
					EndHeist();
				}
				break;
			}
			if (!Game.Player.IsAlive)
			{
				MissionScreen.SetFail("The Printhouse Heist", ManageRobberyAnims.CurrentPay, 0, "The Player has died", 2, 0f, 5);
				EndHeist();
			}
		}
	}
}
