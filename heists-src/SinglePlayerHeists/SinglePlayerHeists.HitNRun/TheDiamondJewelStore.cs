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
	internal class TheDiamondJewelStore : Script
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

		public enum Heiststage
		{
			None,
			HeadingToStore,
			TakeOutCameras,
			LootStore,
			EscapeCops
		}

		public static int HeistMinPay = 650000;

		public static int HeistMaxPay = 350000;

		public static float PercentNeedTake = (float)(HeistMinPay + HeistMaxPay) / 1.5f / 100f * 65f;

		public static int CurrentPay = 0;

		public static int LocalTake = 0;

		public static bool TheDiamondJewelStoreHeistOn;

		public static List<Prop> CabinetGlass = new List<Prop>();

		public static List<Prop> Loot = new List<Prop>();

		public static List<ObjectPlacementMap> Interior = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_pillar"), 753986, 223971722u, new Vector3(360.855f, -833.556152f, 28.39202f), new Vector3(0f, 0f, 57.24416f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_pillar"), 593969, 223971722u, new Vector3(371.854919f, -833.5093f, 25.19202f), new Vector3(0f, -0f, 90.24416f), 219720, 0, new Vector3(0f, -0.0999999344f, -3.199999f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xs_prop_arena_wall_02a_sf"), 780097, 1897672472u, new Vector3(360.855f, -833.556152f, 28.39202f), new Vector3(0f, -0f, 90.24416f), 753986, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 33.0000038f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xs_prop_arena_wall_02a_sf"), 219720, 1897672472u, new Vector3(371.7549f, -833.5097f, 28.39202f), new Vector3(0f, -0f, 90.24416f), 780097, 0, new Vector3(0f, -10.9000006f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xs_prop_arena_wall_02a_sf"), 551751, 1897672472u, new Vector3(360.8124f, -823.5562f, 28.39202f), new Vector3(0f, -0f, 90.24416f), 780097, 0, new Vector3(10.000001f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xs_prop_arena_wall_02a_sf"), 284743, 1897672472u, new Vector3(371.7127f, -823.6098f, 28.39202f), new Vector3(0f, -0f, 90.24416f), 780097, 0, new Vector3(9.900002f, -10.9000006f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xs_prop_arena_wall_02a_sf"), 245576, 1897672472u, new Vector3(365.878235f, -815.534546f, 28.39202f), new Vector3(0f, 0f, 0.244169757f), 780097, 0, new Vector3(18.0000324f, -5.09999275f, 0f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 255304, 924295337u, new Vector3(366.137054f, -829.3336f, 28.1920185f), new Vector3(0f, -0f, 90.24416f), 780097, 0, new Vector3(4.20000029f, -5.3f, -0.199999988f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 373059, 924295337u, new Vector3(365.403748f, -828.556763f, 28.1920185f), new Vector3(0f, 0f, 0.244169757f), 255304, 0, new Vector3(0.78000015f, 0.7300001f, 0f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 340040, 924295337u, new Vector3(366.863861f, -828.5805f, 28.1920185f), new Vector3(0f, 0f, 0.244169757f), 255304, 0, new Vector3(0.7500002f, -0.7300001f, 0f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 379720, 924295337u, new Vector3(366.130737f, -827.853638f, 28.1920185f), new Vector3(-0f, -0f, -89.7558441f), 255304, 0, new Vector3(1.4799999f, 0f, 0f), new Vector3(0f, 0f, 179.999985f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 777031, 924295337u, new Vector3(368.884827f, -817.0718f, 28.22202f), new Vector3(-0f, -0f, -89.755806f), 336199, 0, new Vector3(0.7499999f, 0.49999997f, -0.769999564f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xs_prop_arena_wall_02a_sf"), 244258, 1897672472u, new Vector3(368.91275f, -823.6217f, 28.39202f), new Vector3(0f, 0f, 0.244169757f), 780097, 0, new Vector3(9.900036f, -8.099993f, 0f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xs_prop_arena_wall_02a_sf"), 244514, 1897672472u, new Vector3(368.91275f, -823.6217f, 28.39202f), new Vector3(0f, -0f, 90.2441559f), 780097, 0, new Vector3(9.900036f, -8.099993f, 0f), new Vector3(0f, 0f, -9.536743E-06f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_pillar"), 291106, 223971722u, new Vector3(368.91275f, -823.6217f, 25.19202f), new Vector3(0f, -0f, 90.2441559f), 244514, 0, new Vector3(0f, 0f, -3.199999f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 259085, 924295337u, new Vector3(360.844116f, -831.006165f, 28.22202f), new Vector3(0f, -0f, 90.24416f), 213261, 0, new Vector3(0.7499999f, 0.49999997f, -0.769999564f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 605198, 924295337u, new Vector3(360.8376f, -829.4762f, 28.22202f), new Vector3(0f, -0f, 90.24416f), 705605, 0, new Vector3(0.7499999f, 0.49999997f, -0.769999564f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 2622, 924295337u, new Vector3(360.831116f, -827.9562f, 28.22202f), new Vector3(0f, -0f, 90.24416f), 96071, 0, new Vector3(0.7499999f, 0.49999997f, -0.769999564f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 705095, 924295337u, new Vector3(360.8246f, -826.4262f, 28.22202f), new Vector3(0f, -0f, 90.24416f), 244002, 0, new Vector3(0.7499999f, 0.49999997f, -0.769999564f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 174663, 924295337u, new Vector3(365.3815f, -823.3368f, 28.1920185f), new Vector3(0f, 0f, 0.244169757f), 596234, 0, new Vector3(0.78000015f, 0.7300001f, 0f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 768832, 924295337u, new Vector3(366.8416f, -823.360535f, 28.1920185f), new Vector3(0f, 0f, 0.244169757f), 596234, 0, new Vector3(0.7500002f, -0.7300001f, 0f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 337480, 924295337u, new Vector3(366.1085f, -822.633667f, 28.1920185f), new Vector3(-0f, -0f, -89.7558441f), 596234, 0, new Vector3(1.4799999f, 0f, 0f), new Vector3(0f, 0f, 179.999985f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 596234, 924295337u, new Vector3(366.1148f, -824.113647f, 28.1920185f), new Vector3(0f, -0f, 90.24416f), 780097, 0, new Vector3(9.420019f, -5.3f, -0.199999988f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 192840, 924295337u, new Vector3(368.8913f, -818.591736f, 28.22202f), new Vector3(-0f, -0f, -89.755806f), 336199, 0, new Vector3(2.27f, 0.49999997f, -0.769999564f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 617288, 924295337u, new Vector3(368.897858f, -820.1318f, 28.22202f), new Vector3(-0f, -0f, -89.755806f), 336199, 0, new Vector3(3.80999851f, 0.49999997f, -0.769999564f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 44342, 924295337u, new Vector3(368.9043f, -821.6418f, 28.22202f), new Vector3(-0f, -0f, -89.755806f), 336199, 0, new Vector3(5.32002831f, 0.5f, -0.769999564f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 382023, 924295337u, new Vector3(371.71933f, -825.1598f, 28.22202f), new Vector3(-0f, -0f, -89.755806f), 483399, 0, new Vector3(0.7499999f, 0.49999997f, -0.769999564f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstplanter_02"), 696134, 316900990u, new Vector3(369.555359f, -833.6191f, 27.49202f), new Vector3(0f, -0f, -179.7558f), 569667, 0, new Vector3(-1.99999976f, 0f, MathF.PI * -226f / 355f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstplanter_02"), 693829, 316900990u, new Vector3(362.955444f, -833.6473f, 27.49202f), new Vector3(0f, 0f, 0.244205713f), 202564, 0, new Vector3(MathF.PI * -226f / 355f, 0f, MathF.PI * -226f / 355f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstpillar_01"), 294983, 1405006221u, new Vector3(367.8344f, -833.3964f, 29.49202f), new Vector3(0f, -0f, -179.7558f), 569667, 0, new Vector3(-0.28f, -0.229999974f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstpillar_01"), 390181, 1405006221u, new Vector3(364.7145f, -833.429749f, 29.49202f), new Vector3(0f, 0f, 0.244205713f), 202564, 0, new Vector3(-0.240000024f, 0.210000023f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 100111, 924295337u, new Vector3(368.953674f, -833.2217f, 28.09202f), new Vector3(0f, -0f, 90.244194f), 569667, 0, new Vector3(-1.40000021f, -0.399999976f, -1.4000001f), new Vector3(0f, 0f, -89.99999f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 658243, 924295337u, new Vector3(363.653717f, -833.244263f, 28.09202f), new Vector3(-0f, -0f, -89.7557755f), 202564, 0, new Vector3(-1.30000007f, 0.399999976f, -1.4000001f), new Vector3(0f, 0f, -89.99999f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 739144, 924295337u, new Vector3(370.653656f, -833.2144f, 28.09202f), new Vector3(0f, -0f, 90.244194f), 569667, 0, new Vector3(-3.09999919f, -0.399999976f, -1.4000001f), new Vector3(0f, 0f, -89.99999f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 46124, 924295337u, new Vector3(361.953735f, -833.2515f, 28.09202f), new Vector3(-0f, -0f, -89.7557755f), 202564, 0, new Vector3(-2.99999928f, 0.399999976f, -1.4000001f), new Vector3(0f, 0f, -89.99999f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 379456, 924295337u, new Vector3(366.264923f, -833.523132f, 31.59202f), new Vector3(-90f, 89.75579f, 0f), 202564, 0, new Vector3(1.30999815f, 0.109999985f, 2.10000014f), new Vector3(-89.99999f, 5.722046E-06f, -89.99999f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstpillar_01"), 371785, 1405006221u, new Vector3(364.714478f, -833.419739f, 31.35202f), new Vector3(0f, 0f, 0.244205713f), 202564, 0, new Vector3(-0.240000024f, 0.220000014f, 1.86000037f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstpillar_01"), 379975, 1405006221u, new Vector3(367.834351f, -833.3864f, 31.39202f), new Vector3(0f, -0f, -179.7558f), 569667, 0, new Vector3(-0.28f, -0.240000024f, 1.90000033f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 99857, 2068850345u, new Vector3(364.768036f, -813.1393f, 27.7920189f), new Vector3(90f, 0.244169757f, 0f), 415817, 0, new Vector3(5.96046448E-08f, 2.39999986f, -0.599999964f), new Vector3(89.99999f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sm_prop_smug_hgrdoors_01"), 464707, 3293339660u, new Vector3(359.6856f, -826.6611f, 33.002018f), new Vector3(90f, 90.24416f, 0f), 780097, 0, new Vector3(6.899996f, 1.14000046f, 4.609998f), new Vector3(90.0000153f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sm_prop_smug_hgrdoors_01"), 731720, 3293339660u, new Vector3(374.095459f, -826.5997f, 33.0120163f), new Vector3(90f, -89.755806f, 0f), 464707, 0, new Vector3(2.98023224E-07f, 0.01f, 14.4099884f), new Vector3(0f, -179.999985f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_casino_diamonds_02a"), 378124, 3086855352u, new Vector3(366.2675f, -834.1231f, 31.59202f), new Vector3(8.00001049f, 10.000001f, 0.244236216f), 379456, 0, new Vector3(0.599999964f, 0f, 0f), new Vector3(-179.999985f, 100.000015f, -81.99999f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_go_window"), 149262, 2310905709u, new Vector3(369.733551f, -833.188354f, 30.89202f), new Vector3(0f, -0f, -179.755768f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_go_window"), 554255, 2310905709u, new Vector3(362.833618f, -833.2177f, 30.89202f), new Vector3(0f, -0f, -179.755768f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 466761, 924295337u, new Vector3(371.725922f, -826.7098f, 28.22202f), new Vector3(-0f, -0f, -89.755806f), 483399, 0, new Vector3(2.3f, 0.49999997f, -0.769999564f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 38957, 924295337u, new Vector3(371.73233f, -828.2098f, 28.22202f), new Vector3(-0f, -0f, -89.755806f), 483399, 0, new Vector3(3.79999852f, 0.49999997f, -0.769999564f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 114500, 924295337u, new Vector3(371.738922f, -829.759766f, 28.22202f), new Vector3(-0f, -0f, -89.755806f), 483399, 0, new Vector3(5.34999847f, 0.49999997f, -0.769999564f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_temp_block_blocker"), 699454, 4143853297u, new Vector3(368.435333f, -828.9238f, 21.3920174f), new Vector3(0f, 0f, 0.244169757f), 780097, 0, new Vector3(4.6f, -7.599999f, -7.000001f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_temp_block_blocker"), 224584, 4143853297u, new Vector3(361.4354f, -828.9536f, 21.3920174f), new Vector3(0f, 0f, 0.244169757f), 780097, 0, new Vector3(4.6f, -0.600003958f, -7.000001f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_temp_block_blocker"), 529481, 4143853297u, new Vector3(368.3953f, -819.523865f, 21.3920174f), new Vector3(0f, 0f, 0.244169757f), 780097, 0, new Vector3(14.0000191f, -7.599999f, -7.000001f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_temp_block_blocker"), 658493, 4143853297u, new Vector3(361.395355f, -819.5537f, 21.3920174f), new Vector3(0f, 0f, 0.244169757f), 780097, 0, new Vector3(14.0000191f, -0.600003958f, -7.000001f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstpillar_01"), 330515, 1405006221u, new Vector3(369.834381f, -833.387939f, 32.69202f), new Vector3(0f, -90f, -179.7558f), 569667, 0, new Vector3(-2.28f, -0.229999989f, 3.199999f), new Vector3(0f, -90.00001f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstpillar_01"), 523593, 1405006221u, new Vector3(366.434418f, -833.4024f, 32.69202f), new Vector3(0f, -90f, -179.7558f), 569667, 0, new Vector3(1.12000048f, -0.229999989f, 3.199999f), new Vector3(0f, -90.00001f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstpillar_01"), 534089, 1405006221u, new Vector3(362.934448f, -833.4173f, 32.69202f), new Vector3(0f, -90f, -179.7558f), 569667, 0, new Vector3(4.61999846f, -0.229999989f, 3.199999f), new Vector3(0f, -90.00001f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 390965, 924295337u, new Vector3(360.804016f, -821.596252f, 28.22202f), new Vector3(0f, -0f, 90.24416f), 580680, 0, new Vector3(0.7499999f, 0.49999997f, -0.769999564f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 540488, 924295337u, new Vector3(360.810425f, -823.096252f, 28.22202f), new Vector3(0f, -0f, 90.24416f), 580680, 0, new Vector3(-0.7500004f, 0.49999997f, -0.769999564f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 687942, 924295337u, new Vector3(360.850525f, -832.506165f, 28.22202f), new Vector3(0f, -0f, 90.24416f), 213261, 0, new Vector3(-0.7500004f, 0.49999997f, -0.769999564f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_casino_diamonds_02a"), 714569, 3086855352u, new Vector3(361.0142f, -824.6343f, 31.1756172f), new Vector3(0f, -0f, 91.3764954f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xs_prop_arena_wall_02a_sf"), 386632, 1897672472u, new Vector3(363.6787f, -815.6439f, 28.39202f), new Vector3(0f, -0f, -179.755844f), 780097, 0, new Vector3(17.900032f, -2.899993f, 0f), new Vector3(0f, 0f, 89.9999847f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 88394, 924295337u, new Vector3(361.722473f, -825.922363f, 28.02202f), new Vector3(0f, -0f, 90.24416f), 244002, 0, new Vector3(1.25f, -0.4000002f, -0.9699996f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 760131, 924295337u, new Vector3(362.448181f, -824.91925f, 28.02202f), new Vector3(0f, 0f, 0.244169757f), 244002, 0, new Vector3(2.25f, -1.13000035f, -0.9699996f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 725833, 924295337u, new Vector3(362.71817f, -824.9181f, 28.02202f), new Vector3(0f, 0f, 0.244169757f), 760131, 0, new Vector3(0.27000007f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_biker_boardchair01"), 490054, 3824151676u, new Vector3(361.681335f, -825.2263f, 28.9942131f), new Vector3(0f, -0f, 106.359528f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_computer_01"), 766788, 1613369486u, new Vector3(362.637146f, -825.382751f, 29.4005966f), new Vector3(0f, -0f, -105.8881f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 681032, 2068850345u, new Vector3(366.758026f, -813.1308f, 27.7920189f), new Vector3(90f, 0.244169757f, 0f), 415817, 0, new Vector3(1.98999858f, 2.39999986f, -0.599999964f), new Vector3(89.99999f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 507202, 2068850345u, new Vector3(364.7574f, -810.6393f, 27.7920189f), new Vector3(90f, 0.244169757f, 0f), 415817, 0, new Vector3(5.96046448E-08f, 4.89999771f, -0.599999964f), new Vector3(89.99999f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 685375, 2068850345u, new Vector3(366.757355f, -810.6308f, 27.7920189f), new Vector3(90f, 0.244169757f, 0f), 415817, 0, new Vector3(2.00000048f, 4.89999771f, -0.599999964f), new Vector3(89.99999f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xs_prop_arena_wall_02a_sf"), 204106, 1897672472u, new Vector3(363.6787f, -815.6439f, 28.39202f), new Vector3(0f, -0f, 90.24415f), 780097, 0, new Vector3(17.900032f, -2.899993f, 0f), new Vector3(0f, 0f, -1.71661377E-05f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xs_prop_arena_wall_02a_sf"), 200772, 1897672472u, new Vector3(367.978638f, -815.6256f, 28.39202f), new Vector3(0f, -0f, 90.24415f), 780097, 0, new Vector3(17.900032f, -7.199989f, 0f), new Vector3(0f, 0f, -1.71661377E-05f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 176712, 924295337u, new Vector3(364.4787f, -815.6405f, 32.09202f), new Vector3(-90f, -90.24415f, 0f), 204106, 0, new Vector3(1.49011612E-08f, -0.8000001f, 3.69999862f), new Vector3(-90.00001f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xs_prop_arena_wall_02a_sf"), 657464, 1897672472u, new Vector3(363.656952f, -810.543945f, 28.39202f), new Vector3(0f, 0f, 0.244142428f), 780097, 0, new Vector3(23.0000515f, -2.899993f, 0f), new Vector3(0f, 0f, -90.00002f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xs_prop_arena_wall_02a_sf"), 667192, 1897672472u, new Vector3(363.636932f, -805.844f, 28.39202f), new Vector3(0f, 0f, 0.244142428f), 780097, 0, new Vector3(27.70007f, -2.899993f, 0f), new Vector3(0f, 0f, -90.00002f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 633161, 113622690u, new Vector3(362.63382f, -825.837463f, 28.9966755f), new Vector3(0f, 0f, 36.05757f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 694089, 113622690u, new Vector3(364.459442f, -833.1663f, 30.0805531f), new Vector3(0f, 0f, -31.19433f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 702025, 113622690u, new Vector3(364.4594f, -833.1713f, 31.7228451f), new Vector3(0f, 0f, -52.6389656f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 292170, 113622690u, new Vector3(368.099548f, -833.170654f, 30.0805531f), new Vector3(0f, 0f, 40.04977f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 706888, 113622690u, new Vector3(368.087982f, -833.2028f, 31.77874f), new Vector3(0f, 0f, 40.9197578f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 559688, 113622690u, new Vector3(366.132233f, -833.6737f, 31.73901f), new Vector3(0f, 0f, 3.25040627f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 258121, 113622690u, new Vector3(366.013519f, -833.674255f, 31.7609425f), new Vector3(0f, 0f, 10.2110081f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 754239, 113622690u, new Vector3(366.249847f, -833.6732f, 31.7226658f), new Vector3(0f, 0f, -3.914958f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_res_mvasechinese"), 772930, 1855795498u, new Vector3(361.2235f, -832.4766f, 29.6062f), new Vector3(0f, -0f, 90.40948f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_res_mvasechinese"), 493640, 1855795498u, new Vector3(361.196655f, -826.4065f, 29.6062f), new Vector3(0f, -0f, 90.1531f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_res_mvasechinese"), 175170, 1855795498u, new Vector3(361.1791f, -823.0854f, 29.6062f), new Vector3(0f, -0f, 90.6133041f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_res_mvasechinese"), 65858, 1855795498u, new Vector3(361.1808f, -817.0251f, 29.6062f), new Vector3(0f, -0f, 90.35681f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_res_mvasechinese"), 160532, 1855795498u, new Vector3(371.3933f, -829.7738f, 29.6062f), new Vector3(0f, -0f, -90.35432f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_battle_lights_ceiling_l_b"), 358730, 4058744475u, new Vector3(366.1765f, -830.1342f, 31.5361652f), new Vector3(0f, -0f, 179.71965f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_battle_lights_ceiling_l_b"), 358986, 4058744475u, new Vector3(366.1912f, -827.1343f, 31.5361652f), new Vector3(0f, -0f, 179.71965f), 358730, 0, new Vector3(-1.49011612E-08f, -2.99999928f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_battle_lights_ceiling_l_b"), 773698, 4058744475u, new Vector3(366.205872f, -824.1343f, 31.5361652f), new Vector3(0f, -0f, 179.71965f), 358730, 0, new Vector3(-1.49011612E-08f, -5.99999666f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_battle_lights_ceiling_l_b"), 5447, 4058744475u, new Vector3(366.220551f, -821.134338f, 31.5361652f), new Vector3(0f, -0f, 179.71965f), 358730, 0, new Vector3(-1.49011612E-08f, -8.999998f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_battle_lights_ceiling_l_b"), 572740, 4058744475u, new Vector3(366.235229f, -818.134338f, 31.5361652f), new Vector3(0f, -0f, 179.71965f), 358730, 0, new Vector3(-1.49011612E-08f, -12.00001f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_exec_award_diamond"), 462665, 1485377863u, new Vector3(362.7602f, -824.1918f, 29.4059715f), new Vector3(0f, -0f, 113.626694f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_res_mflowers"), 388623, 355465634u, new Vector3(368.492767f, -821.690552f, 29.6079f), new Vector3(0f, 0f, -48.1787567f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_miniature_yacht_01c"), 402191, 4250530917u, new Vector3(361.352417f, -827.936462f, 29.604702f), new Vector3(0f, -0f, -179.757858f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_miniature_yacht_01c"), 422159, 4250530917u, new Vector3(361.3601f, -829.4548f, 29.604702f), new Vector3(0f, -0f, -179.143143f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_miniature_yacht_01c"), 442383, 4250530917u, new Vector3(361.372955f, -830.9822f, 29.604702f), new Vector3(0f, -0f, -179.85968f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_miniature_yacht_01c"), 442639, 4250530917u, new Vector3(371.209381f, -828.2353f, 29.604702f), new Vector3(0f, 0f, 0.5457038f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_miniature_yacht_01c"), 468239, 4250530917u, new Vector3(371.218f, -826.728333f, 29.604702f), new Vector3(0f, 0f, 0.6480463f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_miniature_yacht_01c"), 481807, 4250530917u, new Vector3(371.204926f, -825.1897f, 29.604702f), new Vector3(0f, 0f, 0.443319619f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_miniature_yacht_01c"), 737589, 4250530917u, new Vector3(361.32077f, -821.5928f, 29.6047f), new Vector3(0f, -0f, 179.012878f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_miniature_yacht_01c"), 708935, 4250530917u, new Vector3(361.2972f, -820.063f, 29.604702f), new Vector3(0f, -0f, 178.80722f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_miniature_yacht_01c"), 56376, 4250530917u, new Vector3(361.299957f, -818.5373f, 29.604702f), new Vector3(0f, -0f, -179.452454f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_miniature_yacht_01c"), 558607, 4250530917u, new Vector3(368.3682f, -817.0948f, 29.604702f), new Vector3(0f, 0f, -0.574252844f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_miniature_yacht_01c"), 580111, 4250530917u, new Vector3(368.3713f, -818.634155f, 29.604702f), new Vector3(0f, 0f, 0.500580668f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_miniature_yacht_01c"), 714054, 4250530917u, new Vector3(368.373383f, -820.148132f, 29.604702f), new Vector3(0f, 0f, -0.2670771f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_pogo_gold_01a"), 657973, 3395499597u, new Vector3(361.214417f, -825.9382f, 29.4059467f), new Vector3(0f, 0f, 76.52057f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01a"), 457802, 2257894849u, new Vector3(366.561646f, -828.595032f, 28.7232838f), new Vector3(0f, 0f, -89.83043f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01a"), 458826, 2257894849u, new Vector3(365.741638f, -828.5975f, 28.7232838f), new Vector3(-0f, -0f, -89.83043f), 457802, 0, new Vector3(0f, -0.8199996f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01a"), 352837, 2257894849u, new Vector3(365.7261f, -823.3475f, 28.7232838f), new Vector3(-0f, -0f, -89.83043f), 715322, 0, new Vector3(0f, -0.8199996f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01a"), 715322, 2257894849u, new Vector3(366.5461f, -823.345032f, 28.7232838f), new Vector3(-0f, -0f, -89.83043f), 457802, 0, new Vector3(-5.24999762f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_planter_02"), 723785, 1373401077u, new Vector3(366.132019f, -828.680664f, 29.5730362f), new Vector3(0f, -0f, 179.253586f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_planter_02"), 652870, 1373401077u, new Vector3(366.160645f, -823.3603f, 29.5730362f), new Vector3(0f, -0f, 135.083542f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sm_prop_smug_hgrdoors_01"), 630089, 3293339660u, new Vector3(374.0358f, -812.599731f, 33.0120163f), new Vector3(90f, -89.755806f, 0f), 476477, 0, new Vector3(2.98023224E-07f, 0.01f, 14.4099884f), new Vector3(0f, -179.999985f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sm_prop_smug_hgrdoors_01"), 476477, 3293339660u, new Vector3(359.625946f, -812.661133f, 33.002018f), new Vector3(90f, 90.24416f, 0f), 780097, 0, new Vector3(20.9000435f, 1.14000046f, 4.609998f), new Vector3(90.0000153f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xs_prop_arena_wall_02a_sf"), 399944, 1897672472u, new Vector3(363.636932f, -805.844f, 28.39202f), new Vector3(0f, -0f, -179.755859f), 780097, 0, new Vector3(27.70007f, -2.899993f, 0f), new Vector3(0f, 0f, 89.99997f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 688198, 113622690u, new Vector3(366.14566f, -823.3487f, 30.6059284f), new Vector3(0f, 0f, -76.03794f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 551241, 113622690u, new Vector3(366.120148f, -828.6837f, 30.1774464f), new Vector3(0f, -0f, 122.799789f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 464970, 113622690u, new Vector3(363.7205f, -815.7438f, 30.0238247f), new Vector3(0f, 0f, 32.36236f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 465482, 113622690u, new Vector3(365.8352f, -815.734741f, 30.0558624f), new Vector3(0f, 0f, 8.153689f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 465738, 113622690u, new Vector3(361.695435f, -823.092346f, 29.0183487f), new Vector3(0f, 0f, 74.27857f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 467274, 113622690u, new Vector3(361.671631f, -817.0141f, 28.9966755f), new Vector3(0f, 0f, 70.6441345f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 467530, 113622690u, new Vector3(368.0193f, -821.645264f, 29.0149689f), new Vector3(0f, 0f, -71.94545f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 655945, 113622690u, new Vector3(370.854f, -829.783f, 29.0013065f), new Vector3(0f, -0f, -98.20092f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 539462, 113622690u, new Vector3(361.746735f, -826.436646f, 28.9966755f), new Vector3(0f, 0f, 79.49954f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 742729, 113622690u, new Vector3(361.7356f, -832.5164f, 29.012888f), new Vector3(0f, -0f, 105.9087f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 327233, 924295337u, new Vector3(360.761f, -820.0459f, 28.22202f), new Vector3(0f, -0f, 90.24416f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_dstsign_01"), 356166, 924295337u, new Vector3(360.751068f, -818.546265f, 28.22202f), new Vector3(0f, -0f, 90.2441559f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> Guards = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Security Guard"), 9248, 3613962792u, new Vector3(364.592224f, -832.7731f, 28.3952236f), new Vector3(0f, 0f, 1.843054f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Security Guard"), 663869, 3613962792u, new Vector3(367.0381f, -816.2186f, 28.39653f), new Vector3(0f, -0f, 119.814789f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> Cabinets = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 162887, 938137634u, new Vector3(361.3007f, -820.814148f, 28.99202f), new Vector3(0f, -0f, 90.24416f), 580680, 0, new Vector3(1.5299989f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 433481, 938137634u, new Vector3(361.29422f, -819.2941f, 28.99202f), new Vector3(0f, -0f, 90.24416f), 580680, 0, new Vector3(3.04999828f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 733507, 938137634u, new Vector3(361.2877f, -817.76416f, 28.99202f), new Vector3(0f, -0f, 90.24416f), 580680, 0, new Vector3(4.58001041f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 580680, 938137634u, new Vector3(361.30722f, -822.3441f, 28.99202f), new Vector3(0f, -0f, 90.24416f), 780097, 0, new Vector3(11.2100067f, -0.4999999f, 0.5999999f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 395062, 938137634u, new Vector3(366.138916f, -829.7736f, 28.962019f), new Vector3(0f, 0f, 0.244169757f), 255304, 0, new Vector3(-0.440000117f, 0f, 0.770000041f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 394551, 938137634u, new Vector3(364.963745f, -828.558655f, 28.97202f), new Vector3(-0f, -0f, -89.75583f), 373059, 0, new Vector3(-0.440000027f, 0f, 0.78000015f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 47418, 938137634u, new Vector3(366.1288f, -827.4036f, 28.962019f), new Vector3(0f, -0f, -179.755844f), 379720, 0, new Vector3(-0.4500001f, 0f, 0.7699996f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 47162, 938137634u, new Vector3(367.303864f, -828.5786f, 28.97202f), new Vector3(0f, -0f, 90.2441559f), 340040, 0, new Vector3(0.440000117f, 0f, 0.780000031f), new Vector3(0f, 0f, 90f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 533770, 938137634u, new Vector3(366.116669f, -824.55365f, 28.962019f), new Vector3(0f, 0f, 0.244169757f), 596234, 0, new Vector3(-0.440000117f, 0f, 0.770000041f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 456971, 938137634u, new Vector3(364.9415f, -823.3387f, 28.9520187f), new Vector3(-0f, -0f, -89.75583f), 174663, 0, new Vector3(-0.440000176f, 0f, 0.760000169f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 175629, 938137634u, new Vector3(366.106567f, -822.183655f, 28.89202f), new Vector3(0f, -0f, -179.755844f), 337480, 0, new Vector3(-0.4500001f, -1.49011612E-08f, 0.7000001f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 191757, 938137634u, new Vector3(367.2716f, -823.3587f, 28.8120174f), new Vector3(0f, -0f, 90.24414f), 768832, 0, new Vector3(0.4300002f, 0f, 0.619999766f), new Vector3(0f, 0f, 89.9999847f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 213261, 938137634u, new Vector3(361.347321f, -831.754f, 28.99202f), new Vector3(0f, -0f, 90.24416f), 780097, 0, new Vector3(1.80000019f, -0.4999999f, 0.5999999f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 705605, 938137634u, new Vector3(361.3408f, -830.22406f, 28.99202f), new Vector3(0f, -0f, 90.24416f), 213261, 0, new Vector3(1.5299989f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 96071, 938137634u, new Vector3(361.33432f, -828.704041f, 28.99202f), new Vector3(0f, -0f, 90.24416f), 213261, 0, new Vector3(3.04999828f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 244002, 938137634u, new Vector3(361.3278f, -827.1741f, 28.99202f), new Vector3(0f, -0f, 90.24416f), 213261, 0, new Vector3(4.58001041f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 630855, 938137634u, new Vector3(371.222656f, -825.9419f, 28.99202f), new Vector3(-0f, -0f, -89.755806f), 483399, 0, new Vector3(1.5299989f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 298311, 938137634u, new Vector3(371.229126f, -827.4619f, 28.99202f), new Vector3(-0f, -0f, -89.755806f), 483399, 0, new Vector3(3.04999828f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 12858, 938137634u, new Vector3(371.235657f, -828.9919f, 28.99202f), new Vector3(-0f, -0f, -89.755806f), 483399, 0, new Vector3(4.58001041f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 483399, 938137634u, new Vector3(371.216125f, -824.4119f, 28.99202f), new Vector3(-0f, -0f, -89.755806f), 780097, 0, new Vector3(9.100002f, -10.3999987f, 0.5999999f), new Vector3(0f, 0f, -179.999985f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 238401, 938137634u, new Vector3(368.388153f, -817.8539f, 28.99202f), new Vector3(-0f, -0f, -89.755806f), 336199, 0, new Vector3(1.5299989f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 567112, 938137634u, new Vector3(368.394623f, -819.3739f, 28.99202f), new Vector3(-0f, -0f, -89.755806f), 336199, 0, new Vector3(3.04999828f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 566856, 938137634u, new Vector3(368.401154f, -820.9039f, 28.99202f), new Vector3(-0f, -0f, -89.755806f), 336199, 0, new Vector3(4.58001041f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 336199, 938137634u, new Vector3(368.381622f, -816.3239f, 28.99202f), new Vector3(-0f, -0f, -89.755806f), 780097, 0, new Vector3(17.2000332f, -7.59999037f, 0.5999999f), new Vector3(0f, 0f, -179.999985f))
		};

		public static List<ObjectPlacementMap> CCTV = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_bank_cctv_02"), 455240, 2452560208u, new Vector3(371.21347f, -824.091736f, 32.86694f), new Vector3(0f, 0f, -48.98632f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_bank_cctv_02"), 509258, 2452560208u, new Vector3(361.305f, -832.907837f, 32.85694f), new Vector3(0f, -0f, 135.871628f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_bank_cctv_02"), 157460, 2452560208u, new Vector3(361.3024f, -816.175232f, 32.85694f), new Vector3(0f, 0f, 46.611515f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> LootDiamonds = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_diamond_trolly_01c"), 267333, 3489641539u, new Vector3(367.2593f, -814.732f, 28.9022789f), new Vector3(0f, -0f, -270.825684f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_diamond_trolly_01c"), 322377, 3489641539u, new Vector3(365.714325f, -811.1507f, 28.9022789f), new Vector3(0f, 0f, -177.932129f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_diamond_trolly_01c"), 291914, 3489641539u, new Vector3(367.323181f, -813.244141f, 28.7411938f), new Vector3(-0f, -0f, -269.59668f), 736070, 0, new Vector3(0f, 0f, 0.370000064f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> MainDoors = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_prop_hei_bankdoor_new"), 569667, 1240350830u, new Vector3(367.5554f, -833.6276f, 29.49202f), new Vector3(0f, -0f, -179.7558f), 780097, 0, new Vector3(-0.100000143f, -6.69999838f, 1.09999943f), new Vector3(0f, 0f, 90.0000153f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_prop_hei_bankdoor_new"), 202564, 1240350830u, new Vector3(364.9554f, -833.638733f, 29.49202f), new Vector3(0f, 0f, 0.244205713f), 569667, 0, new Vector3(2.59999967f, 0f, 0f), new Vector3(0f, 0f, -179.999985f))
		};

		public static List<ObjectPlacementMap> VaultDoorObjects = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_vault_door"), 105545, 3030613028u, new Vector3(363.9401f, -815.7907f, 29.6471424f), new Vector3(0f, 0f, -1.52573514f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_fingerprint_scanner_01c"), 92487, 1624948650u, new Vector3(366.422821f, -815.729431f, 29.7900372f), new Vector3(0f, 0f, 0.316793054f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 415817, 3294791577u, new Vector3(364.778259f, -815.539246f, 28.39202f), new Vector3(0f, 0f, 0.244169757f), 245576, 0, new Vector3(-1.1f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> JewelCaseA = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_glass_disp_01a"), 5944, 2580434079u, new Vector3(371.143921f, -831.613953f, 28.3020153f), new Vector3(0f, 0f, -89.75739f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_diamond_disp_01a"), 735038, 167446900u, new Vector3(371.143921f, -831.613953f, 29.3220158f), new Vector3(-0f, -0f, -89.75739f), 5944, 0, new Vector3(0f, 0f, 1.0200001f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_diamond_01a"), 752140, 677732008u, new Vector3(371.143921f, -831.613953f, 29.3220158f), new Vector3(-0f, -0f, -89.75739f), 735038, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> JewelCaseB = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_glass_disp_01a"), 401468, 2580434079u, new Vector3(362.705017f, -816.266541f, 28.2720146f), new Vector3(0f, 0f, -0.5999173f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_neck_disp_01a"), 62478, 3466777742u, new Vector3(362.705017f, -816.266541f, 29.3120136f), new Vector3(-0f, -0f, -0.5999173f), 401468, 0, new Vector3(0f, 0f, 1.03999937f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_necklace_01a"), 87822, 2472404644u, new Vector3(362.705017f, -816.266541f, 29.3120136f), new Vector3(-0f, -0f, -0.5999173f), 62478, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> EnteranceMarker = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mk_money"), 726098, 1113676861u, new Vector3(366.241058f, -833.6034f, 28.6944122f), new Vector3(0f, -0f, -179.664276f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<Prop> Cameras = new List<Prop>();

		public static Prop _thermiteProp;

		public static Prop BagProp;

		public static bool VaultDoorOpen;

		public static bool PlayeringThirmiteDoor1;

		public static string DictToUse;

		public static string DictToUse2;

		public static int PlantThermiteSyncScene;

		public static bool ThermiteCuttingThroughVault;

		public static int PTFX1;

		public static int PTFX2;

		public static bool ThermiteDone;

		public static Prop VaultDoor;

		public static bool HackedKeypad;

		public static KeypadClass MainKeypad;

		public static Blip MainBlip;

		public static Heiststage CurrentHeistStage;

		public static Vector3 Enterance_Inside;

		public static Vector3 Enterance_Outside;

		public static bool IsInInterior;

		public static bool HackingKeypad = false;

		public static Prop LastKeypad;

		public static Prop USB;

		public static Prop Phone;

		public int Scene1;

		public static bool ExitedStore;

		public static Ped ConvPed;

		public static bool ConvStarted;

		public static int CopsArriveIn;

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

		public TheDiamondJewelStore()
		{
			base.Tick += OnTick;
		}

		public static float GetDistance(Vector3 _1, Vector3 _2)
		{
			return _1.DistanceTo(_2);
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
				if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition, "<", 1.25f))
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
								if (KeyPad == MainKeypad)
								{
									HackedKeypad = true;
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
								foreach (ObjectPlacementMap lootDiamond in LootDiamonds)
								{
									if (lootDiamond.Entity != null)
									{
										ManageRobberyAnims.AddPropAsLoot((Prop)lootDiamond.Entity);
									}
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
					if (KeyPad == MainKeypad)
					{
						foreach (ObjectPlacementMap lootDiamond2 in LootDiamonds)
						{
							if (lootDiamond2.Entity != null)
							{
								ManageRobberyAnims.AddPropAsLoot((Prop)lootDiamond2.Entity);
							}
						}
						HackedKeypad = true;
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

		public void VaultDoorInteract()
		{
			if (!HackedKeypad && !ThermiteDone)
			{
				if (MainKeypad != null && MainKeypad.Keypad != null)
				{
					CheckKeypad(MainKeypad);
				}
			}
			else if (VaultDoor != null && VaultDoor.Rotation.Z > -105f)
			{
				if (VaultDoor.IsAttached())
				{
					VaultDoor.Detach();
				}
				VaultDoor.IsPositionFrozen = true;
				float z = VaultDoor.Rotation.Z;
				VaultDoor.Rotation = new Vector3(VaultDoor.Rotation.X, VaultDoor.Rotation.Y, z -= 0.25f);
			}
			if (HackedKeypad || ThermiteDone || !(VaultDoor != null))
			{
				return;
			}
			Vector3 offsetPosition = VaultDoor.GetOffsetPosition(new Vector3(1.05f, -0.18f, 0f));
			World.DrawMarker(MarkerType.Sphere, offsetPosition, Vector3.Zero, Vector3.Zero, new Vector3(0.01f, 0.01f, 0.01f), Color.AliceBlue);
			if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition, "<", 1.25f))
			{
				DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Plant Thermal Charge");
				if (Game.IsControlJustPressed(Control.Context) && !PlayeringThirmiteDoor1)
				{
					PlayeringThirmiteDoor1 = true;
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
					DictToUse = "anim_heist@hs3f@ig13_thermal_charge@thermal_charge@male@";
					DictToUse2 = "anim@heists@ornate_bank@thermal_charge";
					LoadDict(DictToUse);
					LoadDict(DictToUse);
					LoadDict(DictToUse2);
					LoadDict(DictToUse2);
					if (_thermiteProp != null)
					{
						_thermiteProp.Delete();
					}
					if (BagProp != null)
					{
						BagProp.Delete();
					}
					BagProp = GlobalHelper.CreateProp(RequestModel("ch_p_m_bag_var02_arm_s"), new Vector3(-105.03f, 6471.854f, 31.6f), new Vector3(1f, 0f, 0f), false, false);
					PlantThermiteSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.0, 0.0, VaultDoor.Heading, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, PlantThermiteSyncScene, LoadDict("anim@heists@ornate_bank@thermal_charge"), "thermal_charge", 1f, -1f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BagProp, PlantThermiteSyncScene, "bag_thermal_charge", LoadDict("anim@heists@ornate_bank@thermal_charge"), 1f, -1f, 0, 1148846080);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, BagProp);
					_thermiteProp = GlobalHelper.CreateProp(RequestModel("hei_prop_heist_thermite"), Game.Player.Character.Position, false, false);
					Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, _thermiteProp, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, -90f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					Function.Call(Hash.PLAY_ENTITY_ANIM, _thermiteProp, "bomb_thermal_charge", "anim@heists@ornate_bank@thermal_charge", 1000.0, 1, 1, 1, 0, 0);
				}
			}
			if (!PlayeringThirmiteDoor1)
			{
				return;
			}
			if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, PlantThermiteSyncScene) >= 1056353216 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, PlantThermiteSyncScene) <= 1057353216 && !ThermiteCuttingThroughVault)
			{
				Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ornate_heist");
				Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ornate_heist");
				PTFX1 = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_heist_ornate_thermal_burn", _thermiteProp, 0f, 0f, 0f, 1f, 0, 0, 0);
				Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
				Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
				PTFX2 = GlobalHelper.CreatePTFX_LoopedOnEntity("proj_flare_fuse_fp", _thermiteProp, 0f, 0f, 0f, 10f, 0, 0, 0);
				Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
				ThermiteCuttingThroughVault = true;
				Random random = new Random();
			}
			if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, PlantThermiteSyncScene) < 1065353216 || ThermiteDone)
			{
				return;
			}
			foreach (ObjectPlacementMap lootDiamond in LootDiamonds)
			{
				if (lootDiamond.Entity != null)
				{
					ManageRobberyAnims.AddPropAsLoot((Prop)lootDiamond.Entity);
				}
			}
			ThermiteDone = true;
			Game.Player.Character.Task.ClearAll();
			if (BagProp != null)
			{
				BagProp.Delete();
			}
			if (_thermiteProp != null)
			{
				_thermiteProp.Delete();
			}
			Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX2, false);
			Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX1, false);
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

		private void OnTick(object sender, EventArgs e)
		{
			if (!TheDiamondJewelStoreHeistOn || !TheDiamondJewelStoreHeistOn)
			{
				return;
			}
			if (IsInInterior)
			{
				Function.Call(Hash.SET_RADAR_ZOOM, 25);
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
			if (currentHeistStage >= Heiststage.TakeOutCameras)
			{
				if (Game.GameTime < CopsArriveIn)
				{
					UI.DrawTime("COPS ARRIVE", CopsArriveIn - Game.GameTime);
					int num = CopsArriveIn - Game.GameTime;
					if (num >= 79900 && num <= 80100 && !ConvStarted)
					{
						ConvPed = GlobalHelper.CreatePed(RequestModel(PedHash.Paige), Game.Player.Character.Position.Around(3f));
						ConvPed.Opacity = 0;
						ConvPed.IsInvincible = true;
						ConvPed.IsCollisionEnabled = false;
						ConvPed.AttachTo(Game.Player.Character);
						ConvPed.BlockPermanentEvents = true;
						Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
						Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, ConvPed, "Paige");
						Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "JH_LGAA", "Paige", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
						Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
						ConvStarted = true;
					}
					if (num >= 69900 && num <= 70100 && !ConvStarted)
					{
						ConvPed = GlobalHelper.CreatePed(RequestModel(PedHash.Paige), Game.Player.Character.Position.Around(3f));
						ConvPed.Opacity = 0;
						ConvPed.IsInvincible = true;
						ConvPed.IsCollisionEnabled = false;
						ConvPed.AttachTo(Game.Player.Character);
						ConvPed.BlockPermanentEvents = true;
						Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
						Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, ConvPed, "Paige");
						Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "JH_LHAA", "Paige", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
						Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
						ConvStarted = true;
					}
					if (num >= 59900 && num <= 60100 && !ConvStarted)
					{
						ConvPed = GlobalHelper.CreatePed(RequestModel(PedHash.Paige), Game.Player.Character.Position.Around(3f));
						ConvPed.Opacity = 0;
						ConvPed.IsInvincible = true;
						ConvPed.IsCollisionEnabled = false;
						ConvPed.AttachTo(Game.Player.Character);
						ConvPed.BlockPermanentEvents = true;
						Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
						Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, ConvPed, "Paige");
						Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "JH_LIAA", "Paige", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
						Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
						ConvStarted = true;
					}
					if (num >= 49900 && num <= 50100 && !ConvStarted)
					{
						ConvPed = GlobalHelper.CreatePed(RequestModel(PedHash.Paige), Game.Player.Character.Position.Around(3f));
						ConvPed.Opacity = 0;
						ConvPed.IsInvincible = true;
						ConvPed.IsCollisionEnabled = false;
						ConvPed.AttachTo(Game.Player.Character);
						ConvPed.BlockPermanentEvents = true;
						Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
						Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, ConvPed, "Paige");
						Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "JH_AUAA", "Paige", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
						Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
						ConvStarted = true;
					}
					if (num >= 39900 && num <= 40100 && !ConvStarted)
					{
						ConvPed = GlobalHelper.CreatePed(RequestModel(PedHash.Paige), Game.Player.Character.Position.Around(3f));
						ConvPed.Opacity = 0;
						ConvPed.IsInvincible = true;
						ConvPed.IsCollisionEnabled = false;
						ConvPed.AttachTo(Game.Player.Character);
						ConvPed.BlockPermanentEvents = true;
						Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
						Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, ConvPed, "Paige");
						Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "JH_AVAA", "Paige", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
						Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
						ConvStarted = true;
					}
					if (num >= 29900 && num <= 30100 && !ConvStarted)
					{
						ConvPed = GlobalHelper.CreatePed(RequestModel(PedHash.Paige), Game.Player.Character.Position.Around(3f));
						ConvPed.Opacity = 0;
						ConvPed.IsInvincible = true;
						ConvPed.IsCollisionEnabled = false;
						ConvPed.AttachTo(Game.Player.Character);
						ConvPed.BlockPermanentEvents = true;
						Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
						Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, ConvPed, "Paige");
						Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "JH_AWAA", "Paige", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
						Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
						ConvStarted = true;
					}
					if (num >= 19900 && num <= 20100 && !ConvStarted)
					{
						ConvPed = GlobalHelper.CreatePed(RequestModel(PedHash.Paige), Game.Player.Character.Position.Around(3f));
						ConvPed.Opacity = 0;
						ConvPed.IsInvincible = true;
						ConvPed.IsCollisionEnabled = false;
						ConvPed.AttachTo(Game.Player.Character);
						ConvPed.BlockPermanentEvents = true;
						Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
						Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, ConvPed, "Paige");
						Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "JH_AXAA", "Paige", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
						Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
						ConvStarted = true;
					}
					if (num >= 9975 && num <= 10025)
					{
						if (ConvPed != null)
						{
							ConvPed.Delete();
						}
						ConvPed = GlobalHelper.CreatePed(RequestModel(PedHash.Paige), Game.Player.Character.Position.Around(3f));
						ConvPed.Opacity = 0;
						ConvPed.IsInvincible = true;
						ConvPed.IsCollisionEnabled = false;
						ConvPed.AttachTo(Game.Player.Character);
						ConvPed.BlockPermanentEvents = true;
						Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
						Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, ConvPed, "Paige");
						Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "JH_LJAA", "Paige", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
						Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "JH_LJAB", "Paige", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
						Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "JH_LJAC", "Paige", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
						Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "JH_LJAD", "Paige", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
						Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "JH_LJAE", "Paige", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
						Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "JH_LJAF", "Paige", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
						Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "JH_LJAG", "Paige", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
						Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "JH_LJAH", "Paige", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
						Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "JH_LJAI", "Paige", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
						Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "JH_LJAJ", "Paige", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
						Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "JH_LKAA", "Paige", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
						Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "JH_LLAA", "Paige", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
						Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
						Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
						ConvStarted = true;
					}
				}
				else
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, Enterance_Inside, "<", 80f))
					{
						CopsArriveIn = 0;
						Game.Player.WantedLevel = 3;
						Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, Enterance_Inside, ">", 80f) && CopsArriveIn == 0)
					{
						CopsArriveIn = -1;
						Game.Player.WantedLevel = 5;
						Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
					}
				}
			}
			switch (currentHeistStage)
			{
			case Heiststage.HeadingToStore:
				if (GetDistance_VDIST2(Game.Player.Character.Position, Enterance_Inside, "<", 30f))
				{
					Screen.ShowSubtitle("Head into the ~y~The Diamond Jewel Store");
				}
				else
				{
					Screen.ShowSubtitle("Head over to ~y~The Diamond Jewel Store");
				}
				if (!GetDistance_VDIST2(Game.Player.Character.Position, Enterance_Inside, "<", 10f))
				{
					break;
				}
				if (MainBlip != null)
				{
					MainBlip.Alpha = 0;
				}
				foreach (Prop item in Loot)
				{
					if (item != null)
					{
						ManageRobberyAnims.AddPropAsLoot(item);
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
				VaultDoor = (Prop)VaultDoorObjects[0].Entity;
				MainKeypad = new KeypadClass((Prop)VaultDoorObjects[1].Entity, 1);
				HackedKeypad = false;
				ManageRobberyAnims.AddPropAsLoot((Prop)JewelCaseA[0].Entity);
				ManageRobberyAnims.GlassCabinetPropsList.Add(new ManageRobberyAnims.GlassCabinetProps((Prop)JewelCaseA[0].Entity, new List<Prop>
				{
					(Prop)JewelCaseA[1].Entity,
					(Prop)JewelCaseA[2].Entity
				}));
				ManageRobberyAnims.AddPropAsLoot((Prop)JewelCaseB[0].Entity);
				ManageRobberyAnims.GlassCabinetPropsList.Add(new ManageRobberyAnims.GlassCabinetProps((Prop)JewelCaseB[0].Entity, new List<Prop>
				{
					(Prop)JewelCaseB[1].Entity,
					(Prop)JewelCaseB[2].Entity
				}));
				CopsArriveIn = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 85000, 100000);
				ConvPed = GlobalHelper.CreatePed(RequestModel(PedHash.Paige), Game.Player.Character.Position.Around(3f));
				ConvPed.Opacity = 0;
				ConvPed.IsInvincible = true;
				ConvPed.IsCollisionEnabled = false;
				ConvPed.AttachTo(Game.Player.Character);
				ConvPed.BlockPermanentEvents = true;
				Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
				Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, ConvPed, "Paige");
				Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "JH_ATAA", "Paige", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
				Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
				ConvStarted = true;
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
				VaultDoorInteract();
				if (Game.GameTime < CopsArriveIn)
				{
					Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
					Game.Player.WantedLevel = 0;
				}
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
				VaultDoorInteract();
				if (Game.GameTime < CopsArriveIn)
				{
					Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
					Game.Player.WantedLevel = 0;
				}
				int num3 = (int)PercentNeedTake;
				if (!HackedKeypad && !ThermiteDone)
				{
					Screen.ShowSubtitle("Loot the store for ~g~$" + num3.ToString("N") + "~w~, or find a way to open the ~y~Vault");
				}
				else
				{
					Screen.ShowSubtitle("Loot the store for ~g~$" + num3.ToString("N"));
				}
				if ((float)ManageRobberyAnims.CurrentPay >= PercentNeedTake)
				{
					CurrentHeistStage = Heiststage.EscapeCops;
				}
				break;
			}
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
				VaultDoorInteract();
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
						if (GetDistance_VDIST2(Game.Player.Character.Position, Enterance_Outside, "<", 200f))
						{
							if (Game.GameTime < CopsArriveIn)
							{
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
								Game.Player.WantedLevel = 0;
							}
							IsInInterior = false;
						}
					}
					if (vector == Enterance_Inside)
					{
						if (Game.GameTime < CopsArriveIn)
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
							Game.Player.WantedLevel = 0;
						}
						IsInInterior = true;
					}
				}
				if (IsInInterior)
				{
					if (ManageRobberyAnims.LootLeft >= 0f)
					{
						Screen.ShowSubtitle("Keep looting ~g~Valuables~w~ or leave the ~y~The Diamond Jewel Store~w~");
					}
					else
					{
						Screen.ShowSubtitle("Leave the ~y~The Diamond Jewel Store~w~");
					}
				}
				else
				{
					Screen.ShowSubtitle("Escape the ~r~Cops");
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, Enterance_Inside, ">", 150f) && !IsInInterior && Game.Player.WantedLevel == 0 && !IsInInterior)
				{
					Main.TotalCrewEarned += ManageRobberyAnims.CurrentPay;
					Main.TotalPlayerEarned += 0f;
					XP.CompleteHeist_TheDiamondJewelStoreHeist++;
					Main.SaveCuts();
					Class1.SetNoLongerInHeist();
					MissionScreen.SetPass("The Diamond Jewel Store Heist", ManageRobberyAnims.CurrentPay, 0, "The Player has died", 2, 0f, 5);
					int num2 = Game.GameTime + 30000;
					while (HKHModHelperNew.Class1.HeistPassedScreen != -1 && Game.GameTime <= num2)
					{
						Script.Wait(0);
					}
					Script.Wait(2000);
					ManagePlayerFunds.AddFunds(ManageRobberyAnims.CurrentPay);
					XP.AddXP_Percent(XP.CompleteHeist_XPPercent_TheDiamondJewelStoreHeist);
					EndHeist();
				}
				break;
			}
			if (!Game.Player.IsAlive)
			{
				MissionScreen.SetFail("The Diamond Jewel Store Heist", ManageRobberyAnims.CurrentPay, 0, "The Player has died", 2, 0f, 5);
				EndHeist();
			}
		}

		public static void EndHeist()
		{
			Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
			ExitedStore = false;
			if (MainKeypad != null && MainKeypad.Keypad != null && MainKeypad.Keypad.AttachedBlip != null)
			{
				MainKeypad.Keypad.AttachedBlip.Delete();
			}
			if (MainBlip != null)
			{
				MainBlip.Delete();
			}
			Function.Call(Hash.SET_RADAR_ZOOM, 850);
			CurrentPay = 0;
			LocalTake = 0;
			ManageRobberyAnims.CurrentPay = 0;
			if (Phone != null)
			{
				Phone.Delete();
			}
			if (USB != null)
			{
				USB.Delete();
			}
			HackingKeypad = false;
			HackedKeypad = false;
			VaultDoorOpen = false;
			ThermiteDone = false;
			ThermiteCuttingThroughVault = false;
			PlayeringThirmiteDoor1 = false;
			HackedKeypad = false;
			IsInInterior = false;
			CurrentHeistStage = Heiststage.None;
			SmartGuards.ClearGenericGuards();
			TheDiamondJewelStoreHeistOn = false;
			if (MainBlip != null)
			{
				MainBlip.Delete();
			}
			if (_thermiteProp != null)
			{
				_thermiteProp.Delete();
			}
			if (BagProp != null)
			{
				BagProp.Delete();
			}
			foreach (Prop item in CabinetGlass)
			{
				if (item != null)
				{
					item.Delete();
				}
			}
			foreach (Prop item2 in Loot)
			{
				if (item2 != null)
				{
					item2.Delete();
				}
			}
			foreach (ObjectPlacementMap lootDiamond in LootDiamonds)
			{
				if (lootDiamond.Entity != null)
				{
					lootDiamond.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item3 in Interior)
			{
				if (item3.Entity != null)
				{
					item3.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap cabinet in Cabinets)
			{
				if (cabinet.Entity != null)
				{
					cabinet.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item4 in CCTV)
			{
				if (item4.Entity != null)
				{
					item4.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap guard in Guards)
			{
				if (guard.Entity != null)
				{
					guard.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap mainDoor in MainDoors)
			{
				if (mainDoor.Entity != null)
				{
					mainDoor.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap vaultDoorObject in VaultDoorObjects)
			{
				if (vaultDoorObject.Entity != null)
				{
					vaultDoorObject.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item5 in EnteranceMarker)
			{
				if (item5.Entity != null)
				{
					item5.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item6 in JewelCaseA)
			{
				if (item6.Entity != null)
				{
					item6.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap item7 in JewelCaseB)
			{
				if (item7.Entity != null)
				{
					item7.Entity.Delete();
				}
			}
		}

		public static void StartHeist()
		{
			ManageRobberyAnims.CurrentPay = 0;
			ManageRobberyAnims.Heist_Average_Pay = (float)(HeistMinPay + HeistMaxPay) / 2f * 1.8f;
			Function.Call(Hash.BEGIN_TEXT_COMMAND_BUSYSPINNER_ON, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "Loading The Diamond Jewel Store Heist, Please Wait");
			Function.Call(Hash.END_TEXT_COMMAND_BUSYSPINNER_ON, 5);
			EndHeist();
			foreach (ObjectPlacementMap item in Interior)
			{
				item.CreateObjatCoords();
			}
			foreach (ObjectPlacementMap cabinet in Cabinets)
			{
				cabinet.CreateObjatCoords();
			}
			foreach (ObjectPlacementMap item2 in CCTV)
			{
				item2.CreateObjatCoords();
			}
			foreach (ObjectPlacementMap guard in Guards)
			{
				guard.CreateObjatCoords();
			}
			foreach (ObjectPlacementMap mainDoor in MainDoors)
			{
				mainDoor.CreateObjatCoords();
			}
			foreach (ObjectPlacementMap vaultDoorObject in VaultDoorObjects)
			{
				vaultDoorObject.CreateObjatCoords();
			}
			foreach (ObjectPlacementMap lootDiamond in LootDiamonds)
			{
				lootDiamond.CreateObjatCoords();
			}
			foreach (ObjectPlacementMap item3 in EnteranceMarker)
			{
				item3.CreateObjatCoords();
			}
			foreach (ObjectPlacementMap item4 in JewelCaseA)
			{
				item4.CreateObjatCoords();
			}
			foreach (ObjectPlacementMap item5 in JewelCaseB)
			{
				item5.CreateObjatCoords();
			}
			foreach (ObjectPlacementMap cabinet2 in Cabinets)
			{
				if (cabinet2.Entity != null)
				{
					Prop prop = GlobalHelper.CreateProp(RequestModel("p_counter_04_glass"), new Vector3(0f, 0f, 0f), false, false);
					prop.AttachTo(cabinet2.Entity);
					Prop prop2 = GlobalHelper.CreateProp(RequestModel("p_counter_02_glass"), new Vector3(0f, 0f, 0f), false, false);
					prop2.AttachTo(cabinet2.Entity);
					CabinetGlass.Add(prop);
					CabinetGlass.Add(prop2);
					string name = "des_jewel_cab2_start";
					int num = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 1000);
					Vector3 vector = new Vector3(0f, 0f, 0f);
					if (num < 250)
					{
						name = "des_jewel_cab2_start";
						vector = new Vector3(0f, 0f, 0f);
					}
					if (num >= 250 && num < 500)
					{
						name = "des_jewel_cab3_start";
						vector = new Vector3(0f, 0f, 0f);
					}
					if (num >= 500 && num < 750)
					{
						name = "des_jewel_cab4_start";
						vector = new Vector3(0f, 0f, -90f);
					}
					if (num >= 750 && num <= 1000)
					{
						name = "des_jewel_cab_start";
						vector = new Vector3(0f, 0f, 0f);
					}
					Prop prop3 = GlobalHelper.CreateProp(RequestModel(name), new Vector3(0f, 0f, 0f), false, false);
					prop3.AttachTo(cabinet2.Entity, new Vector3(0f, 0f, 0f), new Vector3(vector.X, vector.Y, vector.Z));
					Loot.Add(prop3);
				}
			}
			Enterance_Inside = EnteranceMarker[0].Entity.GetOffsetPosition(new Vector3(0f, -5f, 0f));
			Enterance_Outside = EnteranceMarker[0].Entity.GetOffsetPosition(new Vector3(0f, 5f, 0f));
			EnteranceMarker[0].Entity.Opacity = 0;
			CurrentHeistStage = Heiststage.HeadingToStore;
			TheDiamondJewelStoreHeistOn = true;
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
			MainBlip.Name = "Enter the Diamond Jewel Store";
			Function.Call(Hash.BUSYSPINNER_OFF);
		}
	}
}
