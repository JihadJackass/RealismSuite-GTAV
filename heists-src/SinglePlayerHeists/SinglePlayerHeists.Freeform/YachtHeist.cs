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
	internal class YachtHeist : Script
	{
		public enum ActiveGroup
		{
			None,
			YachtHeist_YachtGuards,
			YachtHeist_LongfinGuards,
			YachtHeist_VIPMansion,
			YachtHeist_Bunker,
			YachtHeist_IAAFacility,
			YachtHeist_DrillIntel
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
			VIPEnterPrePurchase,
			VIPEnterAfterPurchase,
			InSuperVolito,
			InLongfin,
			C4,
			Drill,
			NoWaytoOpenVaults,
			DisposableKeycard,
			SearchYachtForLoot,
			longfin,
			HackKeypad
		}

		public static ActiveGroup CurrentActiveGroup = ActiveGroup.None;

		public static bool GotGrinder;

		public static bool GotC4 = false;

		public static bool GotDrill = false;

		public static int C4ChargesCollected = 20;

		public static bool GotThermite = true;

		public static int ThermiteChargesCollected = 20;

		public static bool GotKeycard;

		public static int HeistMinPay = 450000;

		public static int HeistMaxPay = 2250000;

		public static int CurrentPay = 0;

		public static int LocalTake = 0;

		public static bool HackingKeypad = false;

		public static Prop LastKeypad;

		public static Prop USB;

		public static Prop Phone;

		public int Scene1;

		public static List<SmartGuards.SmartAiV2> YachtGuards = new List<SmartGuards.SmartAiV2>();

		public static List<SmartGuards.SmartAiV2> LongfinGuards = new List<SmartGuards.SmartAiV2>();

		public static List<SmartGuards.SmartAiV2> MansionGuards = new List<SmartGuards.SmartAiV2>();

		public static List<SmartGuards.SmartAiV2> C4Guards = new List<SmartGuards.SmartAiV2>();

		public static List<SmartGuards.SmartAiV2> IAAGuards = new List<SmartGuards.SmartAiV2>();

		public static List<SmartGuards.SmartAiV2> DrillGuards = new List<SmartGuards.SmartAiV2>();

		public static MissionOjects YachtObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_fingerprint_scanner_01a"), 510498, 278208236u, new Vector3(-1375.39282f, 6737.02344f, 2.866519f), new Vector3(0f, 0f, 73.7149048f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_fingerprint_scanner_01a"), 76094, 278208236u, new Vector3(-1406.71362f, 6745.82f, 12.2875662f), new Vector3(0f, -0f, 162.40239f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_fingerprint_scanner_01a"), 222026, 278208236u, new Vector3(-1434.28479f, 6753.58154f, 6.28414869f), new Vector3(-0f, -0f, -17.7319241f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_door_01a"), 365333, 2244758654u, new Vector3(-1375.27258f, 6737.137f, 2.82252884f), new Vector3(0f, 0f, 72.17691f), 582164, 0, new Vector3(-0.8399995f, -0.280000031f, 1.22999918f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_door_01a"), 523022, 2244758654u, new Vector3(-1433.17322f, 6753.13232f, 6.13414335f), new Vector3(0f, 0f, -18.4643917f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_door_01a"), 683791, 2244758654u, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, -19.2310467f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_garage_door_01a"), 13573, 141297241u, new Vector3(-1407.75525f, 6748.645f, 12.4241133f), new Vector3(0f, 0f, 73.10363f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_garage_door_01a"), 270853, 141297241u, new Vector3(-1417.62231f, 6751.87158f, 12.41988f), new Vector3(0f, 0f, 72.07943f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_door_01a"), 431880, 2244758654u, new Vector3(-1417.91418f, 6746.388f, 12.157361f), new Vector3(0f, 0f, -17.84645f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_door_01a"), 726795, 2244758654u, new Vector3(-1412.97266f, 6755.77539f, 12.1573763f), new Vector3(0f, -0f, 161.847641f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 582164, 3294791577u, new Vector3(-1375.282f, 6738.02246f, 1.59252965f), new Vector3(0f, 0f, 72.17691f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_fnclink_01d"), 670234, 4086366786u, new Vector3(-1374.6698f, 6739.92627f, 0.4925295f), new Vector3(0f, -90f, 72.17691f), 582164, 0, new Vector3(2.00000048f, 2.23517418E-07f, -1.10000014f), new Vector3(0f, -90.00001f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_fnclink_01d"), 127773, 4086366786u, new Vector3(-1375.588f, 6737.07031f, 0.4925295f), new Vector3(0f, -90f, 72.17691f), 582164, 0, new Vector3(-1f, 2.23517418E-07f, -1.10000014f), new Vector3(0f, -90.00001f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 561214, 304964818u, new Vector3(-1412.69836f, 6746.7915f, 10.9092016f), new Vector3(0f, -0f, 161.529068f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 200511, 304964818u, new Vector3(-1417.88855f, 6748.69775f, 10.9109507f), new Vector3(0f, 0f, 71.0400162f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 705087, 304964818u, new Vector3(-1415.96741f, 6754.375f, 10.9109325f), new Vector3(0f, 0f, 72.47234f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 332608, 304964818u, new Vector3(-1410.83154f, 6752.98828f, 10.90925f), new Vector3(0f, 0f, -18.1692028f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 502100, 304964818u, new Vector3(-1386.79761f, 6739.97f, 1.5651468f), new Vector3(0f, -0f, 162.088f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 509780, 304964818u, new Vector3(-1389.3092f, 6740.78369f, 1.56530333f), new Vector3(0f, -0f, 162.088f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 168021, 304964818u, new Vector3(-1394.29932f, 6741.45654f, 1.56558216f), new Vector3(0f, 0f, 72.31521f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 554336, 304964818u, new Vector3(-1409.843f, 6742.1123f, 2.0614624f), new Vector3(0f, -0f, 161.731064f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 355429, 304964818u, new Vector3(-1385.95935f, 6734.37842f, 1.58960247f), new Vector3(0f, -0f, 161.575745f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_sec_cabinet_dum"), 233734, 1036306241u, new Vector3(-1434.69629f, 6753.608f, 4.8841486f), new Vector3(0f, 0f, -17.7319241f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_sec_cabinet_dum"), 172813, 1036306241u, new Vector3(-1434.48315f, 6754.27441f, 4.8841486f), new Vector3(0f, -0f, 162.268082f), 233734, 0, new Vector3(0f, 0.6999999f, 0f), new Vector3(0f, 0f, -179.999985f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_sec_cabinet_dum"), 408862, 1036306241u, new Vector3(-1434.02954f, 6753.39453f, 4.8841486f), new Vector3(-0f, -0f, -17.7319241f), 233734, 0, new Vector3(0.7000001f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_sec_cabinet_dum"), 479016, 1036306241u, new Vector3(-1433.81641f, 6754.061f, 4.8841486f), new Vector3(0f, -0f, 162.268051f), 408862, 0, new Vector3(0f, 0.700000167f, 0f), new Vector3(0f, 0f, 179.999985f))
		});

		public static MissionOjects YachtLoot = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_diamond_trolly_01c"), 1860, 3489641539u, new Vector3(-1412.70251f, 6753.566f, 11.3837519f), new Vector3(0f, 0f, -205.947662f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_diamond_trolly_01c"), 659268, 3489641539u, new Vector3(-1414.3573f, 6747.35645f, 11.3836374f), new Vector3(0f, -0f, -8.244003f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_diamond_trolly_01c"), 517446, 3489641539u, new Vector3(-1431.41614f, 6763.65137f, 5.358012f), new Vector3(0f, 0f, -198.474533f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_gold_trolly_01c"), 300359, 3559648747u, new Vector3(-1434.52881f, 6764.318f, 5.358012f), new Vector3(0f, 0f, -107.782227f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_gold_trolly_01c"), 556359, 3559648747u, new Vector3(-1435.89307f, 6760.076f, 5.358012f), new Vector3(0f, 0f, -107.373444f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_diamond_trolly_01c"), 495432, 3489641539u, new Vector3(-1429.73608f, 6760.0293f, 5.358012f), new Vector3(0f, 0f, -165.668243f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_cash_trolly_01c"), 633929, 412463629u, new Vector3(-1429.723f, 6753.72656f, 5.358012f), new Vector3(0f, -0f, -288.347046f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_cash_trolly_01c"), 63818, 412463629u, new Vector3(-1428.99609f, 6755.91748f, 5.358012f), new Vector3(0f, -0f, -288.347046f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_cash_trolly_01c"), 232266, 412463629u, new Vector3(-1426.8739f, 6762.126f, 5.358012f), new Vector3(0f, 0f, -198.0119f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_cash_trolly_01c"), 402252, 412463629u, new Vector3(-1392.6875f, 6743.76f, 2.04045248f), new Vector3(0f, 0f, -197.24527f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_diamond_trolly_01c"), 574796, 3489641539u, new Vector3(-1401.16772f, 6745.197f, 2.06509447f), new Vector3(0f, 0f, -108.447083f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_cash_low_trolly_01c"), 202576, 3781553190u, new Vector3(-1400.50977f, 6747.66846f, 2.064911f), new Vector3(0f, 0f, -107.730965f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_cash_low_trolly_01c"), 774480, 3781553190u, new Vector3(-1416.46716f, 6755.32764f, 2.52532554f), new Vector3(0f, 0f, -108.552055f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_diamond_trolly_01c"), 529747, 3489641539u, new Vector3(-1377.23853f, 6734.11768f, 2.03738713f), new Vector3(0f, -0f, -18.4812317f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 424024, 2151061176u, new Vector3(-1394.14026f, 6741.38965f, 2.40744f), new Vector3(0f, 0f, 71.90521f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 291929, 2151061176u, new Vector3(-1386.76953f, 6740.13232f, 2.40700459f), new Vector3(0f, -0f, 161.724457f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 391257, 2151061176u, new Vector3(-1389.27612f, 6740.96045f, 2.407161f), new Vector3(0f, -0f, 161.724457f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 336474, 2151061176u, new Vector3(-1412.65869f, 6746.9707f, 11.75106f), new Vector3(0f, -0f, 161.566757f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 411482, 2151061176u, new Vector3(-1410.92761f, 6752.835f, 11.7511082f), new Vector3(0f, 0f, -18.07082f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_gold_trolly_01c"), 732765, 3559648747u, new Vector3(-1383.90979f, 6743.54053f, 2.03817248f), new Vector3(0f, 0f, -107.072548f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 19809, 2151061176u, new Vector3(-1409.79358f, 6742.33643f, 2.90332f), new Vector3(0f, -0f, 160.911743f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_diamond_trolly_01c"), 701025, 3489641539u, new Vector3(-1412.09924f, 6746.48438f, 2.53534317f), new Vector3(0f, 0f, -108.65612f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_gold_trolly_01c"), 705634, 3559648747u, new Vector3(-1405.81738f, 6755.046f, 2.53534317f), new Vector3(0f, 0f, -198.5816f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 396390, 2151061176u, new Vector3(-1385.90784f, 6734.51953f, 2.43146014f), new Vector3(0f, -0f, 161.677338f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 278120, 2151061176u, new Vector3(-1415.77673f, 6754.343f, 11.75279f), new Vector3(0f, 0f, 72.0135f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 474985, 2151061176u, new Vector3(-1417.70068f, 6748.67236f, 11.7528086f), new Vector3(0f, 0f, 72.21707f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects LongfinObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Longfin"), 437251, 1861786828u, new Vector3(1568.51575f, 6678.82471f, 0.959015f), new Vector3(4.2960515f, 2.41922545f, 4.258594f), 38, 1, 3, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_byard_float_02b"), 749830, 3317047649u, new Vector3(1565.17065f, 6668.295f, 0.5311283f), new Vector3(0f, 0f, -84.142334f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_byard_float_02b"), 132103, 3317047649u, new Vector3(1564.7196f, 6672.692f, 0.5311283f), new Vector3(-0f, -0f, -84.142334f), 749830, 0, new Vector3(-4.41999769f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_byard_float_02b"), 401416, 3317047649u, new Vector3(1564.27051f, 6677.069f, 0.5311283f), new Vector3(-0f, -0f, -84.142334f), 749830, 0, new Vector3(-8.819998f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_byard_float_02b"), 49162, 3317047649u, new Vector3(1563.82141f, 6681.446f, 0.5311283f), new Vector3(-0f, -0f, -84.142334f), 749830, 0, new Vector3(-13.2200146f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_boxcoke_01"), 400910, 3348973628u, new Vector3(1569.48755f, 6676.977f, 1.08106077f), new Vector3(-2.41242361f, 4.29987f, -85.56005f), 437251, 0, new Vector3(0.820000052f, -1.90000033f, 0.299999982f), new Vector3(0f, 0f, -89.99999f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_boxcoke_01"), 755727, 3348973628u, new Vector3(1569.58386f, 6675.68457f, 0.9836778f), new Vector3(-2.41242361f, 4.29987f, -85.56005f), 437251, 0, new Vector3(0.820000052f, -3.199999f, 0.299999982f), new Vector3(0f, 0f, -89.99999f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_boxcoke_01"), 461839, 3348973628u, new Vector3(1567.99817f, 6676.854f, 1.24382937f), new Vector3(-2.41242361f, 4.29987f, -85.56005f), 437251, 0, new Vector3(-0.679999948f, -1.90000021f, 0.399999976f), new Vector3(0f, 0f, -89.99999f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_boxcoke_01"), 516880, 3348973628u, new Vector3(1568.09436f, 6675.56152f, 1.14644647f), new Vector3(-2.41242361f, 4.29987f, -85.56005f), 437251, 0, new Vector3(-0.679999948f, -3.199999f, 0.399999976f), new Vector3(0f, 0f, -89.99999f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Merryweather Mesa"), 602901, 2230595153u, new Vector3(1570.19409f, 6666.559f, 1.73028541f), new Vector3(1.217282f, -12.4746275f, -70.39733f), 152, 153, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Merryweather Mesa"), 65813, 2230595153u, new Vector3(1567.54871f, 6663.09961f, 2.055884f), new Vector3(4.870491f, -5.983212f, -96.9444046f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 87574, 1821116645u, new Vector3(1572.66748f, 6665.572f, 2.38152814f), new Vector3(0f, -0f, -154.559845f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 502806, 1821116645u, new Vector3(1565.49707f, 6661.778f, 2.30851579f), new Vector3(0f, -0f, 170.022369f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 775450, 1821116645u, new Vector3(1563.53589f, 6683.23047f, 1.684012f), new Vector3(0f, 0f, 30.29755f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_byard_float_02b"), 306969, 3317047649u, new Vector3(1561.9314f, 6681.252f, 0.5311283f), new Vector3(-0f, -0f, -84.142334f), 749830, 0, new Vector3(-13.2200146f, -1.90000021f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_byard_float_02b"), 454937, 3317047649u, new Vector3(1562.38037f, 6676.875f, 0.5311283f), new Vector3(-0f, -0f, -84.142334f), 749830, 0, new Vector3(-8.819998f, -1.90000045f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_byard_float_02b"), 269340, 3317047649u, new Vector3(1560.49036f, 6676.681f, 0.5311283f), new Vector3(-0f, -0f, -84.142334f), 749830, 0, new Vector3(-8.819998f, -3.79999876f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mb_hesco_06"), 746782, 1354899844u, new Vector3(1560.25562f, 6678.969f, 0.5311283f), new Vector3(-0f, -0f, -84.142334f), 269340, 0, new Vector3(-2.3f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_byard_float_02b"), 226847, 3317047649u, new Vector3(1558.60022f, 6676.4873f, 0.5311283f), new Vector3(-0f, -0f, -84.142334f), 749830, 0, new Vector3(-8.819998f, -5.699997f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_byard_float_02b"), 610335, 3317047649u, new Vector3(1556.61072f, 6676.283f, 0.5311283f), new Vector3(-0f, -0f, -84.142334f), 749830, 0, new Vector3(-8.819998f, -7.699995f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_byard_float_02b"), 677916, 3317047649u, new Vector3(1560.04126f, 6681.058f, 0.5311283f), new Vector3(-0f, -0f, -84.142334f), 749830, 0, new Vector3(-13.2200146f, -3.79999876f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_byard_float_02b"), 384545, 3317047649u, new Vector3(1558.15125f, 6680.86426f, 0.5311283f), new Vector3(-0f, -0f, -84.142334f), 749830, 0, new Vector3(-13.2200146f, -5.699997f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_byard_float_02b"), 479775, 3317047649u, new Vector3(1556.16162f, 6680.66f, 0.5311283f), new Vector3(-0f, -0f, -84.142334f), 749830, 0, new Vector3(-13.2200146f, -7.699995f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 537893, 2127932792u, new Vector3(1558.72412f, 6677.16943f, 4.145856f), new Vector3(0f, -0f, -177.80426f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_boxcoke_01"), 179752, 3348973628u, new Vector3(1561.82263f, 20978f / MathF.PI, 3.14562964f), new Vector3(0f, -0f, -169.819717f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_coke_block_01a"), 709671, 3896957328u, new Vector3(1558.59888f, 6679.966f, 3.14213014f), new Vector3(0f, -0f, 96.16008f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_coke_block_01a"), 428840, 3896957328u, new Vector3(1558.57f, 6679.9707f, 3.472331f), new Vector3(0f, -0f, 96.00642f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_coke_block_01a"), 658728, 3896957328u, new Vector3(1558.57227f, 6679.96338f, 3.802532f), new Vector3(0f, -0f, 96.15986f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_coke_block_01a"), 474663, 3896957328u, new Vector3(1558.59888f, 6675.544f, 0.914279163f), new Vector3(0f, 0f, 13.9117546f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static bool CreatedLongfinObjects;

		public static MissionOjects DrillObjets = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_drill"), 39503, 3851537501u, new Vector3(-576.1073f, 5353.245f, 70.54714f), new Vector3(0f, -0f, -163.4412f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_drill"), 751440, 3851537501u, new Vector3(-571.397339f, 5330.93652f, 70.17608f), new Vector3(0f, -0f, 142.61348f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_drill"), 490338, 3851537501u, new Vector3(-558.283752f, 5323.071f, 73.83012f), new Vector3(0f, 0f, -74.12875f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_drill"), 304720, 3851537501u, new Vector3(-582.192932f, 5291.46436f, 70.7674942f), new Vector3(0f, 0f, -8.00157452f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_drill"), 39002, 3851537501u, new Vector3(-583.279846f, 5338.368f, 70.62145f), new Vector3(0f, -0f, -113.178085f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_drill"), 714793, 3851537501u, new Vector3(-580.6708f, 5338.17773f, 70.30627f), new Vector3(0f, -0f, -104.476921f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_drill"), 162865, 3851537501u, new Vector3(-573.8214f, 5361.935f, 70.090416f), new Vector3(0f, 0f, 74.81044f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_drill"), 113998, 3851537501u, new Vector3(-572.453735f, 5356.887f, 69.52199f), new Vector3(0f, -0f, 100.860229f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_drill"), 374607, 3851537501u, new Vector3(-556.7622f, 5361.523f, 70.462616f), new Vector3(0f, 0f, -88.55271f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_drill"), 154978, 3851537501u, new Vector3(-592.733f, 5328.536f, 70.32566f), new Vector3(0f, 0f, 4.12714529f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static bool CreatedDrillObjects;

		public static Prop Drill;

		public static Vector3 DrillIntelLoc = new Vector3(-572.713f, 5341.14f, 69.21f);

		public static List<MissionOjects> DrillNavs = new List<MissionOjects>
		{
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 356442, 1803116220u, new Vector3(-581.665649f, 5311.285f, 69.54395f), new Vector3(0f, 0f, -24.226738f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 594267, 1803116220u, new Vector3(-573.002747f, 5331.567f, 69.54395f), new Vector3(0f, 0f, -22.07705f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 43119, 1803116220u, new Vector3(-564.208f, 5354.64648f, 69.56453f), new Vector3(0f, 0f, -29.2935f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 328313, 1803116220u, new Vector3(-554.51355f, 5375.863f, 69.699295f), new Vector3(0f, -0f, -111.489906f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 115285, 1803116220u, new Vector3(-564.1854f, 5354.86572f, 69.5645142f), new Vector3(0f, -0f, -113.383339f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 406894, 1803116220u, new Vector3(-573.148f, 5331.577f, 69.54395f), new Vector3(0f, -0f, -108.264847f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 287016, 1803116220u, new Vector3(-581.7501f, 5311.05029f, 69.54395f), new Vector3(0f, -0f, -104.579506f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 378370, 1803116220u, new Vector3(-589.2331f, 5287.40137f, 69.54395f), new Vector3(0f, -0f, -109.799355f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 749891, 2127932792u, new Vector3(-589.547f, 5286.901f, 70.2103f), new Vector3(0f, 0f, -27.9604378f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 622438, 1803116220u, new Vector3(-594.693542f, 5349.0293f, 69.68228f), new Vector3(0f, -0f, -107.802193f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 775782, 1803116220u, new Vector3(-601.6104f, 5333.499f, 69.80052f), new Vector3(0f, -0f, -102.683914f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 423427, 1803116220u, new Vector3(-602.255859f, 5311.828f, 69.6467056f), new Vector3(0f, -0f, -112.1009f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 284471, 1803116220u, new Vector3(-597.9718f, 5289.79f, 69.54395f), new Vector3(0f, -0f, -130.93515f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 596603, 1803116220u, new Vector3(-602.635742f, 5311.745f, 69.66016f), new Vector3(0f, -0f, -97.66688f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 247357, 1803116220u, new Vector3(-601.7788f, 5333.65332f, 69.80175f), new Vector3(0f, -0f, -104.012871f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 52074, 1803116220u, new Vector3(-594.8619f, 5349.047f, 69.68723f), new Vector3(0f, -0f, -105.701256f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 687122, 1803116220u, new Vector3(-587.454834f, 5363.427f, 69.60842f), new Vector3(0f, -0f, -101.401184f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 412004, 2127932792u, new Vector3(-587.0676f, 5364.01758f, 70.31285f), new Vector3(0f, -0f, 162.377914f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 745835, 1803116220u, new Vector3(-579.0463f, 5371.235f, 69.63976f), new Vector3(0f, -0f, 158.845978f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 432459, 1803116220u, new Vector3(-557.4039f, 5364.64258f, 69.5439f), new Vector3(0f, -0f, 164.987534f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 532000, 1803116220u, new Vector3(-572.876038f, 5368.883f, 69.5583344f), new Vector3(0f, -0f, 163.912643f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 453639, 1803116220u, new Vector3(-586.9915f, 5373.52539f, 70.10821f), new Vector3(0f, -0f, 163.912643f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 692341, 1821116645u, new Vector3(-587.666931f, 5373.69043f, 70.83675f), new Vector3(0f, -0f, -117.114464f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 237877, 1803116220u, new Vector3(-536.644165f, 5375.46045f, 69.8248f), new Vector3(0f, -0f, 161.455368f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 102668, 1803116220u, new Vector3(-552.4812f, 5381.647f, 69.42616f), new Vector3(0f, 0f, -18.6006126f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 207889, 1803116220u, new Vector3(-561.966553f, 5385.54f, 68.4366f), new Vector3(0f, 0f, -18.6006126f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 333902, 1803116220u, new Vector3(-552.783447f, 5381.784f, 69.37605f), new Vector3(0f, 0f, -18.6006126f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 757027, 1803116220u, new Vector3(-536.9017f, 5375.492f, 69.8246f), new Vector3(0f, 0f, -14.7107859f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 492564, 1803116220u, new Vector3(-526.0759f, 5372.54053f, 69.98887f), new Vector3(0f, 0f, -14.7107859f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 480088, 2127932792u, new Vector3(-525.4523f, 5372.336f, 70.70995f), new Vector3(0f, 0f, 60.1157646f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 512304, 1803116220u, new Vector3(-589.1873f, 5335.5376f, 69.54395f), new Vector3(0f, 0f, -16.1455956f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 111688, 1803116220u, new Vector3(-573.9736f, 5331.13135f, 69.54395f), new Vector3(0f, 0f, -16.1455956f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 21853, 1803116220u, new Vector3(-581.2812f, 5311.731f, 69.54395f), new Vector3(0f, 0f, 68.30283f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 724527, 1803116220u, new Vector3(-602.070068f, 5293.81055f, 69.59747f), new Vector3(0f, 0f, 53.7671f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 539463, 1803116220u, new Vector3(-580.9795f, 5311.395f, 69.54395f), new Vector3(0f, 0f, 38.05434f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 554105, 1803116220u, new Vector3(-573.5242f, 5331.218f, 69.54395f), new Vector3(0f, 0f, 72.08934f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 241258, 1803116220u, new Vector3(-588.761169f, 5335.4707f, 69.54395f), new Vector3(0f, 0f, 68.09685f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 545602, 1821116645u, new Vector3(-589.8784f, 5335.438f, 70.21071f), new Vector3(0f, -0f, -104.485016f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 530000, 1803116220u, new Vector3(-569.582947f, 5305.32f, 73.16378f), new Vector3(0f, 0f, -21.1087475f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 534044, 1803116220u, new Vector3(-562.8322f, 5324.202f, 72.92913f), new Vector3(0f, 0f, -14.7605829f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 550226, 1803116220u, new Vector3(-556.9994f, 5339.77246f, 72.4596558f), new Vector3(0f, 0f, -20.1345215f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 303443, 1803116220u, new Vector3(-550.3337f, 5358.057f, 71.52937f), new Vector3(0f, 0f, -30.0634556f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 658206, 1803116220u, new Vector3(-557.0178f, 5339.989f, 72.44263f), new Vector3(0f, -0f, 126.951111f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 30840, 1803116220u, new Vector3(-562.7272f, 5324.40137f, 72.92913f), new Vector3(0f, -0f, 155.611679f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 490778, 1803116220u, new Vector3(-569.5367f, 5305.547f, 73.14574f), new Vector3(0f, 0f, 74.84695f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 535161, 2127932792u, new Vector3(-569.7104f, 5304.24854f, 73.92103f), new Vector3(0f, 0f, -11.39327f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			})
		};

		public static MissionOjects MansionObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Pilot"), 659213, 2253313678u, new Vector3(-2577.51514f, 1867.77344f, 167.488846f), new Vector3(0f, 0f, 84.85149f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SuperVolito"), 712230, 710198397u, new Vector3(-2573.35254f, 1861.861f, 168.175323f), new Vector3(0.3786784f, 0.010195869f, -5.9934845f), 111, 63, 5, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_helipad_01"), 63539, 3789865418u, new Vector3(-2573.76367f, 1862.83679f, 164.61499f), new Vector3(0f, -0f, 147.754257f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_pillar"), 665676, 223971722u, new Vector3(-2566.44385f, 1861.78845f, 157.726028f), new Vector3(0f, 0f, 25.68755f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_pillar"), 381281, 223971722u, new Vector3(-2569.398f, 1856.77307f, 157.698608f), new Vector3(0f, 0f, 47.5928459f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_pillar"), 379944, 223971722u, new Vector3(-2575.1272f, 1855.32666f, 157.6729f), new Vector3(0f, 0f, 37.71454f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_pillar"), 680258, 223971722u, new Vector3(-2579.88135f, 1858.3678f, 157.67276f), new Vector3(0f, 0f, 30.958477f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_pillar"), 1613, 223971722u, new Vector3(-2581.18872f, 1864.47559f, 158.224609f), new Vector3(0f, 0f, 72.61937f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_pillar"), 302450, 223971722u, new Vector3(-2566.44385f, 1861.78845f, 149.925552f), new Vector3(0f, 0f, 25.6875477f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_pillar"), 117557, 223971722u, new Vector3(-2569.398f, 1856.77307f, 149.898132f), new Vector3(0f, 0f, 47.5928459f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_pillar"), 508467, 223971722u, new Vector3(-2575.1272f, 1855.32666f, 149.872421f), new Vector3(0f, 0f, 37.71454f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_pillar"), 734520, 223971722u, new Vector3(-2579.88135f, 1858.3678f, 149.872284f), new Vector3(0f, 0f, 30.958477f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_pillar"), 635729, 223971722u, new Vector3(-2581.18872f, 1864.47559f, 150.424133f), new Vector3(0f, 0f, 72.61937f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_pillar"), 158254, 223971722u, new Vector3(-2581.18872f, 1864.47559f, 142.823669f), new Vector3(0f, 0f, 72.61937f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_pillar"), 529718, 223971722u, new Vector3(-2579.88135f, 1858.3678f, 142.27182f), new Vector3(0f, 0f, 30.958477f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_pillar"), 567071, 223971722u, new Vector3(-2575.1272f, 1855.32666f, 142.271957f), new Vector3(0f, 0f, 37.71454f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_pillar"), 439372, 223971722u, new Vector3(-2569.398f, 1856.77307f, 142.297668f), new Vector3(0f, 0f, 47.5928459f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_pillar"), 782918, 223971722u, new Vector3(-2566.44385f, 1861.78845f, 142.325089f), new Vector3(0f, 0f, 25.6875458f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 2"), 499327, 3324988722u, new Vector3(-2581.58276f, 1869.13318f, 167.304871f), new Vector3(0f, -0f, -93.66654f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 3"), 150361, 3572886207u, new Vector3(-2580.84058f, 1869.81519f, 167.3054f), new Vector3(0f, -0f, -109.327438f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "studioparty2"), 276805, 1984313950u, new Vector3(-2582.09375f, 1870.02612f, 167.315384f), new Vector3(0f, -0f, -92.84688f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "studioparty1"), 441964, 1152702280u, new Vector3(-2582.964f, 1869.52551f, 167.314865f), new Vector3(-0f, -0f, -89.8775f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "studioparty2"), 141919, 1984313950u, new Vector3(-2583.357f, 1870.83594f, 167.315216f), new Vector3(0f, -0f, -99.65367f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static bool CreatedVIPMansionObjects;

		public static MissionOjects MansionGuardsObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "studioparty2"), 141919, 1984313950u, new Vector3(-2583.357f, 1870.83594f, 167.315216f), new Vector3(0f, -0f, -99.65367f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "High Security"), 637499, 4049719826u, new Vector3(-2575.04077f, 1860.85034f, 167.491516f), new Vector3(0f, 0f, 74.21096f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "High Security"), 160326, 4049719826u, new Vector3(-2556.20117f, 1915.48938f, 168.919f), new Vector3(0f, -0f, -132.509811f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "High Security"), 122707, 4049719826u, new Vector3(-2590.19141f, 1927.702f, 167.316055f), new Vector3(-0f, -0f, -77.336174f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "High Security"), 442191, 4049719826u, new Vector3(-2571.90112f, 1894.11243f, 167.238434f), new Vector3(0f, 0f, 31.7740288f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "High Security"), 382572, 4049719826u, new Vector3(-2600.15112f, 1862.94861f, 167.317719f), new Vector3(0f, 0f, 40.9864769f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects C4Objects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "gr_prop_gr_tunnel_gate"), 260147, 1098542403u, new Vector3(947.639f, -3249.49f, -95.76218f), new Vector3(0f, -0f, -90.01005f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_rock_lrg_10"), 268600, 4133344966u, new Vector3(948.194f, -3241.663f, -88.97202f), new Vector3(0f, 0f, -49.62671f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_rock_lrg_10"), 489783, 4133344966u, new Vector3(948.552246f, -3250.44775f, -88.74357f), new Vector3(0f, -0f, -135.200714f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_rock_lrg_10"), 688952, 4133344966u, new Vector3(947.40094f, -3246.18481f, -89.09612f), new Vector3(0f, 0f, -86.0663452f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_rock_lrg_12"), 121657, 2573901025u, new Vector3(942.961548f, -3250.23364f, -88.26949f), new Vector3(0f, -0f, -113.242081f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_rock_lrg_12"), 599605, 2573901025u, new Vector3(944.1647f, -3249.14478f, -88.02548f), new Vector3(0f, -0f, -156.1822f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_rock_lrg_07"), 753979, 703371055u, new Vector3(947.7796f, -3241.32959f, -91.49555f), new Vector3(0f, 0f, -51.3624077f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_rock_lrg_07"), 129853, 703371055u, new Vector3(947.7796f, -3241.32959f, -93.9955139f), new Vector3(0f, -0f, -110.462082f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_rock_lrg_12"), 125497, 2573901025u, new Vector3(947.4441f, -3250.54175f, -92.01691f), new Vector3(-90f, 118.304993f, 0f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Merryweather Mesa"), 652864, 2230595153u, new Vector3(890.1119f, -3245.97266f, -98.50233f), new Vector3(0.5113998f, -0.105086073f, -2.23554826f), 152, 153, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Merryweather Mesa"), 613172, 2230595153u, new Vector3(886.1556f, -3245.81372f, -98.5051651f), new Vector3(0.433813423f, -0.0443281f, -2.23547125f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_container_ld_pu"), 288066, 3595011691u, new Vector3(883.092651f, -3239.48218f, -99.27899f), new Vector3(0f, 0f, 0.424880266f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_box_wood02a_mws"), 211005, 3262175592u, new Vector3(872.0424f, -3239.64966f, -99.04713f), new Vector3(0f, 0f, -0.240500867f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mb_cargo_04a"), 690498, 897366637u, new Vector3(874.293762f, -3239.449f, -99.00866f), new Vector3(0f, 0f, 0.37367332f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_box_wood04a"), 649791, 2972783418u, new Vector3(880.972656f, -3238.807f, -99.2799f), new Vector3(0f, -0f, -91.18884f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mb_crate_01b"), 80452, 788248216u, new Vector3(893.9647f, -3237.26587f, -99.28289f), new Vector3(0f, 0f, -4.641404f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_weed_set_01c"), 568369, 694724550u, new Vector3(933.3334f, -3222.07178f, -99.26936f), new Vector3(0f, 0f, 2.62657523f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_weed_set_01a"), 660036, 4122558568u, new Vector3(934.1787f, -3203.56665f, -99.26379f), new Vector3(0f, 0f, 1.34734726f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_weed_set_01a"), 630589, 4122558568u, new Vector3(934.3087f, -3208.67285f, -99.26737f), new Vector3(0f, 0f, 1.24498177f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_weed_set_01a"), 537405, 4122558568u, new Vector3(933.342651f, -3213.926f, -99.2596f), new Vector3(0f, -0f, -178.137573f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_weed_set_01a"), 43847, 4122558568u, new Vector3(918.048f, -3198.405f, -99.26213f), new Vector3(0f, -0f, 92.14967f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_weed_set_01a"), 328255, 4122558568u, new Vector3(921.047363f, -3198.24683f, -99.26206f), new Vector3(0f, -0f, 91.4842148f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_weed_set_01a"), 647983, 4122558568u, new Vector3(923.575867f, -3198.22754f, -99.26229f), new Vector3(0f, -0f, 93.3778152f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_weed_set_01a"), 527176, 4122558568u, new Vector3(930.003357f, -3196.10083f, -99.2631454f), new Vector3(0f, 0f, 15.428813f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_weed_set_01c"), 435779, 694724550u, new Vector3(945.136963f, -3201.914f, -99.2563f), new Vector3(0f, 0f, -88.05821f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_weed_set_01c"), 101188, 694724550u, new Vector3(941.6097f, -3225.50439f, -99.2781143f), new Vector3(0f, -0f, -101.10804f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_weed_set_01c"), 149321, 694724550u, new Vector3(942.710632f, -3233.6582f, -99.28374f), new Vector3(0f, -0f, -97.37089f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_weed_set_01c"), 557359, 694724550u, new Vector3(934.7794f, -3237.443f, -99.28467f), new Vector3(0f, 0f, 88.91985f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_weed_set_01a"), 29770, 4122558568u, new Vector3(943.644653f, -3239.81738f, -99.30002f), new Vector3(0f, -0f, -175.8833f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_weed_set_01a"), 399935, 4122558568u, new Vector3(918.887146f, -3229.67529f, -99.28042f), new Vector3(0f, 0f, 87.58804f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_weed_set_01a"), 281409, 4122558568u, new Vector3(924.016541f, -3230.53174f, -99.2834244f), new Vector3(0f, -0f, 92.39887f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_weed_set_01a"), 24395, 4122558568u, new Vector3(915.4523f, -3218.77515f, -99.25955f), new Vector3(0f, -0f, 104.988968f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_weed_set_01a"), 460364, 4122558568u, new Vector3(922.096863f, -3221.12378f, -99.2709961f), new Vector3(0f, -0f, -90.67693f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_weed_set_01a"), 383563, 4122558568u, new Vector3(946.8762f, -3210.66431f, -99.2781143f), new Vector3(0f, 0f, -84.89291f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_weed_set_01a"), 591179, 4122558568u, new Vector3(950.021f, -3221.00635f, -99.290596f), new Vector3(0f, 0f, -89.34518f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_labvats"), 523342, 661161633u, new Vector3(851.9112f, -3232.37915f, -99.51799f), new Vector3(0f, 0f, -4.854552f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_labvats"), 772430, 661161633u, new Vector3(849.283142f, -3232.12744f, -99.53385f), new Vector3(0f, 0f, -4.854552f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "gr_prop_gr_rsply_crate04b"), 305477, 1433923164u, new Vector3(847.3177f, -3232.08447f, -99.4746f), new Vector3(0f, -0f, -91.60621f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "gr_prop_gr_rsply_crate04b"), 745804, 1433923164u, new Vector3(845.871f, -3232.08667f, -99.38532f), new Vector3(0f, -0f, -92.27146f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "gr_prop_gr_rsply_crate04b"), 69199, 1433923164u, new Vector3(833.639465f, -3240.085f, -99.6991348f), new Vector3(0f, 0f, 88.33826f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "gr_prop_gr_rsply_crate04b"), 342859, 1433923164u, new Vector3(835.0755f, -3239.2915f, -99.6991348f), new Vector3(0f, 0f, 88.38846f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 359501, 304964818u, new Vector3(833.0901f, -3238.35547f, -99.6991348f), new Vector3(0f, -0f, 177.49324f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 104018, 304964818u, new Vector3(830.7822f, -3238.255f, -99.6991348f), new Vector3(0f, -0f, 177.49324f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_02a"), 569161, 2104596125u, new Vector3(832.39386f, -3236.32886f, -99.69766f), new Vector3(0f, 0f, 88.03644f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_pallet_ch_01a"), 286025, 414057361u, new Vector3(836.730835f, -3239.656f, -99.5750046f), new Vector3(0f, 0f, 88.9059753f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Merryweather Mesa"), 623188, 2230595153u, new Vector3(854.828857f, -3241.87085f, -98.86004f), new Vector3(3.18237853f, -1.6291393f, -88.69795f), 152, 153, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Merryweather Mesa"), 351051, 2230595153u, new Vector3(854.82f, -3238.23853f, -98.96298f), new Vector3(-1.197331f, -1.72124374f, -88.73305f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_auto_salvage_elegy"), 183380, 3760858856u, new Vector3(844.248169f, -3245.95459f, -99.67496f), new Vector3(0f, 0f, 80.5711441f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_01a"), 784219, 2202902697u, new Vector3(846.4146f, -3238.946f, -99.6991348f), new Vector3(0f, 0f, 87.525444f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_staff_desk_02"), 120669, 2084153992u, new Vector3(842.4314f, -3238.821f, -99.6991348f), new Vector3(0f, -0f, -92.42697f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_mp_h_din_chair_09"), 754782, 987641063u, new Vector3(841.32074f, -3238.7373f, -99.69787f), new Vector3(0f, -0f, 102.155159f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects C4PickupObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 233314, 277707989u, new Vector3(831.482666f, -3238.45361f, -98.7352142f), new Vector3(0f, 0f, 32.19022f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 689248, 277707989u, new Vector3(842.158264f, -3237.74561f, -98.77261f), new Vector3(0f, 0f, -19.860939f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 481890, 277707989u, new Vector3(880.8429f, -3238.52466f, -98.00319f), new Vector3(0f, 0f, -27.5889282f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 592738, 277707989u, new Vector3(885.9846f, -3204.12134f, -98.2804947f), new Vector3(0f, 0f, -48.47045f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 367203, 277707989u, new Vector3(892.689453f, -3211.79346f, -98.27002f), new Vector3(0f, 0f, -85.7297058f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 774239, 277707989u, new Vector3(907.608337f, -3208.38745f, -97.25915f), new Vector3(0f, -0f, -99.90578f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 332385, 277707989u, new Vector3(910.5634f, -3222.45142f, -98.32403f), new Vector3(0f, 0f, -71.4480743f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 238418, 277707989u, new Vector3(892.013733f, -3223.32349f, -98.71089f), new Vector3(0f, -0f, 93.86626f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 125285, 277707989u, new Vector3(893.9706f, -3237.16162f, -97.98143f), new Vector3(0f, 0f, 23.0824718f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 438118, 277707989u, new Vector3(893.0498f, -3175.16284f, -97.20168f), new Vector3(0f, 0f, 18.7323246f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 198758, 277707989u, new Vector3(840.666443f, -3245.11865f, -98.47769f), new Vector3(0f, -0f, 156.510727f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 432476, 277707989u, new Vector3(845.786865f, -3232.57959f, -98.11945f), new Vector3(0f, 0f, -36.7489853f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static List<MissionOjects> C4BunkerNavs = new List<MissionOjects>
		{
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 658481, 1803116220u, new Vector3(890.0938f, -3242.4873f, -98.9326553f), new Vector3(0f, 0f, 87.42817f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 69423, 1803116220u, new Vector3(884.3114f, -3242.43018f, -98.94006f), new Vector3(0f, 0f, 88.29805f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 738865, 1803116220u, new Vector3(863.2504f, -3246.3606f, -98.96491f), new Vector3(0f, -0f, 96.6913147f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 4625, 1803116220u, new Vector3(850.5037f, -3245.16968f, -99.36964f), new Vector3(0f, 0f, 70.17922f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 478831, 1803116220u, new Vector3(837.332031f, -3241.69f, -99.36964f), new Vector3(0f, 0f, 73.76171f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 489244, 1803116220u, new Vector3(854.031f, -3246.194f, -99.25937f), new Vector3(0f, 0f, 61.4780045f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 549488, 1803116220u, new Vector3(863.475647f, -3246.35986f, -98.96491f), new Vector3(0f, 0f, -15.0888948f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 398636, 1803116220u, new Vector3(884.098755f, -3242.46265f, -98.7595444f), new Vector3(0f, 0f, -76.60833f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 456314, 1803116220u, new Vector3(889.8765f, -3242.4563f, -98.70751f), new Vector3(0f, 0f, -85.36006f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier"), 151348, 3019107892u, new Vector3(890.6872f, -3242.38354f, -98.2577f), new Vector3(0f, -0f, 93.41642f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 541459, 1803116220u, new Vector3(867.4647f, -3248.68848f, -98.96408f), new Vector3(0f, 0f, 87.98975f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 545844, 1803116220u, new Vector3(867.7303f, -3235.67017f, -98.96474f), new Vector3(0f, 0f, 0.52116996f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 517170, 1803116220u, new Vector3(870.6834f, -3217.45483f, -98.2680359f), new Vector3(0f, 0f, -12.01816f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 472372, 1803116220u, new Vector3(873.0309f, -3202.40137f, -97.49755f), new Vector3(0f, 0f, -7.872469f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 748827, 1803116220u, new Vector3(874.789551f, -3183.66016f, -97.26229f), new Vector3(0f, 0f, -5.41575336f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 771374, 1803116220u, new Vector3(870.675537f, -3217.05151f, -98.2338943f), new Vector3(0f, -0f, 170.536926f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 127534, 1803116220u, new Vector3(867.7916f, -3235.144f, -98.96491f), new Vector3(0f, -0f, 170.485275f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier"), 650293, 3019107892u, new Vector3(867.504456f, -3249.26953f, -98.28867f), new Vector3(0f, 0f, 3.43010116f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 317239, 1803116220u, new Vector3(856.991f, -3217.958f, -99.14961f), new Vector3(0f, -0f, 179.542725f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 433968, 1803116220u, new Vector3(855.8866f, -3229.42944f, -99.1549f), new Vector3(0f, -0f, 175.9596f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 642097, 1803116220u, new Vector3(855.0208f, -3235.26318f, -99.36964f), new Vector3(0f, 0f, 87.0574341f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 364331, 1803116220u, new Vector3(837.240234f, -3234.56226f, -99.36964f), new Vector3(0f, 0f, 87.6713638f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 119353, 1803116220u, new Vector3(855.134338f, -3235.33423f, -98.83407f), new Vector3(0f, 0f, 89.1042557f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 517923, 1803116220u, new Vector3(855.9276f, -3229.65161f, -99.15274f), new Vector3(0f, 0f, 10.2340889f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 113699, 1803116220u, new Vector3(856.824646f, -3218.51416f, -99.15459f), new Vector3(0f, 0f, 0.458489239f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 668976, 1803116220u, new Vector3(856.7612f, -3213.238f, -99.14903f), new Vector3(0f, 0f, 0.5096703f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier 2"), 251444, 2047212121u, new Vector3(856.5842f, -3212.5603f, -98.4777f), new Vector3(0f, -0f, -172.218491f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 101145, 1803116220u, new Vector3(898.7174f, -3229.709f, -98.9648361f), new Vector3(0f, 0f, 7.22179842f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 248924, 1803116220u, new Vector3(895.940552f, -3223.88843f, -98.93015f), new Vector3(0f, 0f, 21.39887f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 420922, 1803116220u, new Vector3(896.5678f, -3220.76147f, -98.90997f), new Vector3(0f, 0f, -26.2506657f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 154905, 1803116220u, new Vector3(899.134f, -3216.17529f, -98.89808f), new Vector3(0f, 0f, 5.481649f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 187196, 1803116220u, new Vector3(896.6023f, -3207.45776f, -98.86677f), new Vector3(0f, 0f, 7.32414055f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 2359, 1803116220u, new Vector3(894.8581f, -3199.42383f, -98.85539f), new Vector3(0f, 0f, 3.02489138f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 156185, 1803116220u, new Vector3(896.4916f, -3207.53638f, -98.86677f), new Vector3(0f, -0f, -132.2976f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 39512, 1803116220u, new Vector3(899.0298f, -3216.19434f, -98.89789f), new Vector3(0f, -0f, -156.403412f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 449571, 1803116220u, new Vector3(896.493164f, -3220.64038f, -98.90892f), new Vector3(0f, -0f, 134.7577f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 674393, 1803116220u, new Vector3(895.9093f, -3223.731f, -98.85545f), new Vector3(0f, -0f, 154.001328f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 72757, 1803116220u, new Vector3(898.690063f, -3229.65356f, -98.9648361f), new Vector3(0f, -0f, 175.394241f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 576568, 1803116220u, new Vector3(898.7256f, -3239.24634f, -99.00385f), new Vector3(0f, -0f, 91.30295f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier"), 36635, 3019107892u, new Vector3(898.928f, -3239.7688f, -98.33154f), new Vector3(0f, 0f, 5.83158064f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 521024, 1803116220u, new Vector3(872.9486f, -3203.8374f, -97.55792f), new Vector3(0f, -0f, -173.347153f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 641304, 1803116220u, new Vector3(881.336365f, -3204.907f, -98.86677f), new Vector3(0f, -0f, -96.93345f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 225850, 1803116220u, new Vector3(886.090149f, -3207.544f, -98.86677f), new Vector3(0f, -0f, -132.248077f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 624958, 1803116220u, new Vector3(891.895752f, -3208.30786f, -98.86677f), new Vector3(0f, -0f, -90.1769f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 705341, 1803116220u, new Vector3(896.5353f, -3207.63843f, -98.86677f), new Vector3(0f, 0f, -78.7120743f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 104482, 1803116220u, new Vector3(891.7797f, -3208.30029f, -98.86677f), new Vector3(0f, 0f, -74.36157f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 135743, 1803116220u, new Vector3(886.305847f, -3207.51318f, -98.61611f), new Vector3(0f, 0f, 46.63054f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 132927, 1803116220u, new Vector3(881.3611f, -3204.55078f, -98.86677f), new Vector3(0f, 0f, 54.9217033f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 107072, 1803116220u, new Vector3(873.093567f, -3203.6875f, -97.4024658f), new Vector3(0f, 0f, 87.26789f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier 2"), 100646, 2047212121u, new Vector3(872.517639f, -3204.00317f, -96.87899f), new Vector3(0f, -0f, -92.17201f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 423508, 1803116220u, new Vector3(930.4194f, -3198.89136f, -98.93375f), new Vector3(0f, -0f, 143.157608f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 460603, 1803116220u, new Vector3(925.8279f, -3203.57031f, -98.9317856f), new Vector3(0f, -0f, 136.350174f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 435741, 1803116220u, new Vector3(921.637451f, -3205.99683f, -98.930336f), new Vector3(0f, -0f, 137.782974f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 207659, 1803116220u, new Vector3(920.774536f, -3211.43457f, -98.93004f), new Vector3(0f, -0f, 172.176239f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 612418, 1803116220u, new Vector3(920.3422f, -3221.01929f, -98.93953f), new Vector3(0f, -0f, 175.809616f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 276032, 1803116220u, new Vector3(920.698059f, -3236.25537f, -98.96295f), new Vector3(0f, -0f, 179.903519f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 417561, 1803116220u, new Vector3(920.3381f, -3220.808f, -98.77496f), new Vector3(0f, -0f, 175.3482f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 313410, 1803116220u, new Vector3(920.509033f, -3211.50659f, -98.92983f), new Vector3(0f, 0f, -81.6237f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 394525, 1803116220u, new Vector3(921.5735f, -3205.72559f, -98.93051f), new Vector3(0f, 0f, -86.74158f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 89375, 1803116220u, new Vector3(925.6756f, -3203.78979f, -98.9316254f), new Vector3(0f, 0f, -63.8633461f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier"), 569661, 3019107892u, new Vector3(930.4192f, -3198.27124f, -98.26438f), new Vector3(0f, -0f, 179.498489f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 19217, 1803116220u, new Vector3(929.593f, -3236.332f, -98.96455f), new Vector3(0f, 0f, 89.77514f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 259403, 1803116220u, new Vector3(903.1718f, -3237.1936f, -98.9648361f), new Vector3(0f, -0f, 91.66861f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Black Ops Soldier"), 241992, 3019107892u, new Vector3(930.1636f, -3236.29468f, -98.2993851f), new Vector3(0f, 0f, 88.75405f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			})
		};

		public static List<MissionOjects> YachtNavs = new List<MissionOjects>
		{
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 107577, 1803116220u, new Vector3(-1414.818f, 6744.453f, 11.2369089f), new Vector3(0f, 0f, -19.0897484f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 206649, 1803116220u, new Vector3(-1407.33276f, 6742.12646f, 11.2369957f), new Vector3(0f, 0f, -17.2983742f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 783416, 1803116220u, new Vector3(-1399.45581f, 6739.673f, 11.2370977f), new Vector3(0f, 0f, -17.2983742f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 536633, 1803116220u, new Vector3(-1416.46973f, 6744.97266f, 11.2368908f), new Vector3(0f, 0f, -17.2983742f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 566542, 1803116220u, new Vector3(-1420.56543f, 6746.248f, 11.2368469f), new Vector3(0f, 0f, -17.2983742f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 119354, 2127932792u, new Vector3(-1421.86035f, 6746.65137f, 11.9022951f), new Vector3(0f, -0f, -105.022423f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 281143, 1803116220u, new Vector3(-1387.337f, 6735.221f, 5.212219f), new Vector3(0f, 0f, 69.75985f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 722234, 1803116220u, new Vector3(-1401.56689f, 6739.70459f, 5.212467f), new Vector3(0f, 0f, 73.29102f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 620346, 1803116220u, new Vector3(-1425.18542f, 6747.511f, 5.21227741f), new Vector3(0f, 0f, 72.216095f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 428346, 1803116220u, new Vector3(-1408.57581f, 6742.332f, 5.21196556f), new Vector3(0f, 0f, -18.83512f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 616507, 1803116220u, new Vector3(-1381.4585f, 6734.3584f, 5.211872f), new Vector3(0f, 0f, 38.18066f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 760891, 1803116220u, new Vector3(-1375.74475f, 6734.137f, 5.21242332f), new Vector3(0f, 0f, -10.3902073f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 581650, 2127932792u, new Vector3(-1374.68787f, 6733.895f, 5.88748074f), new Vector3(0f, 0f, 74.87728f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 305468, 1803116220u, new Vector3(-1402.74756f, 6753.60742f, 11.2370062f), new Vector3(0f, 0f, 75.23464f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 351293, 1803116220u, new Vector3(-1418.7915f, 6758.42139f, 11.23717f), new Vector3(0f, 0f, 72.00999f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 223547, 1803116220u, new Vector3(-1399.47058f, 6751.914f, 11.23705f), new Vector3(0f, -0f, 123.292725f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 668988, 1803116220u, new Vector3(-1401.32861f, 6747.69727f, 11.2370443f), new Vector3(0f, 0f, 71.24099f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 196414, 1803116220u, new Vector3(-1403.4502f, 6741.447f, 11.2370434f), new Vector3(0f, 0f, 71.24099f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 182333, 1803116220u, new Vector3(-1401.22253f, 6748.00928f, 11.2370443f), new Vector3(0f, 0f, 71.24099f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 162622, 2127932792u, new Vector3(-1398.2561f, 6751.856f, 11.9021244f), new Vector3(0f, 0f, 80.0952454f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 144968, 1803116220u, new Vector3(-1462.09082f, 6766.404f, 7.15772724f), new Vector3(0f, -0f, -108.654083f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 276808, 1803116220u, new Vector3(-1451.39832f, 6762.64551f, 5.216504f), new Vector3(0f, -0f, -111.366264f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 642376, 1803116220u, new Vector3(-1443.4823f, 6755.4917f, 5.21367359f), new Vector3(0f, -0f, -111.058174f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 578885, 1803116220u, new Vector3(-1431.59375f, 6751.4f, 5.212413f), new Vector3(0f, -0f, -108.601051f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 15943, 1803116220u, new Vector3(-1423.73938f, 6749.762f, 5.217046f), new Vector3(0f, -0f, -101.742424f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 28999, 1803116220u, new Vector3(-1431.82153f, 6750.98926f, 5.212284f), new Vector3(0f, 0f, 89.98228f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 472649, 1803116220u, new Vector3(-1443.71f, 6755.05273f, 5.21367645f), new Vector3(0f, 0f, 68.53698f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 594760, 1803116220u, new Vector3(-1452.189f, 6758.67871f, 5.216504f), new Vector3(0f, 0f, 54.30825f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 75848, 1803116220u, new Vector3(-1461.97375f, 6766.526f, 7.154134f), new Vector3(0f, 0f, 70.99257f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 156233, 1803116220u, new Vector3(-1467.69543f, 6768.018f, 7.37497234f), new Vector3(0f, 0f, 73.60269f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 449354, 2127932792u, new Vector3(-1468.88062f, 6768.30957f, 8.092336f), new Vector3(0f, -0f, -108.34594f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 692811, 2127932792u, new Vector3(-1382.06458f, 6743.978f, 8.966266f), new Vector3(0f, 0f, -89.61088f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 584782, 2127932792u, new Vector3(-1430.939f, 6756.264f, 12.7762957f), new Vector3(0f, 0f, 80.55693f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 396111, 1803116220u, new Vector3(-1447.34351f, 6759.801f, 8.299416f), new Vector3(0f, 0f, -16.67145f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 288847, 1803116220u, new Vector3(-1444.815f, 6767.01f, 8.301484f), new Vector3(0f, 0f, -18.2580261f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 103760, 1803116220u, new Vector3(-1439.62915f, 6765.203f, 8.30187f), new Vector3(0f, -0f, -105.572517f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 501328, 1803116220u, new Vector3(-1436.7915f, 6759.06055f, 8.30068f), new Vector3(0f, -0f, -156.957687f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 45853, 1803116220u, new Vector3(-1424.45215f, 6754.19f, 8.300988f), new Vector3(0f, -0f, -110.740463f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 208721, 1803116220u, new Vector3(-1436.84009f, 6758.421f, 8.300685f), new Vector3(0f, 0f, 68.07853f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 275537, 1803116220u, new Vector3(-1440.176f, 6765.30566f, 8.301714f), new Vector3(0f, 0f, 28.7712269f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 80654, 1803116220u, new Vector3(-1444.80151f, 6766.441f, 8.301219f), new Vector3(0f, -0f, 174.5852f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 283985, 1803116220u, new Vector3(-1447.24792f, 6759.809f, 8.299397f), new Vector3(0f, -0f, 162.1476f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 26705, 1803116220u, new Vector3(-1448.8728f, 6755.39551f, 8.301596f), new Vector3(0f, -0f, 165.42244f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 349522, 2127932792u, new Vector3(-1449.28357f, 6755.47559f, 8.966975f), new Vector3(-1.06718709E-07f, -3.9756932E-13f, -36.9396362f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 685394, 1803116220u, new Vector3(-1402.87048f, 6753.48438f, 8.300988f), new Vector3(0f, -0f, 159.128311f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 185172, 1803116220u, new Vector3(-1413.77319f, 6756.982f, 8.300988f), new Vector3(0f, -0f, 161.073013f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 323668, 1803116220u, new Vector3(-1422.3689f, 6759.58057f, 8.300988f), new Vector3(0f, -0f, 130.1082f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 731945, 1803116220u, new Vector3(-1414.10864f, 6757.071f, 8.300988f), new Vector3(0f, -0f, 159.741608f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 313683, 1803116220u, new Vector3(-1397.12292f, 6751.59f, 8.300988f), new Vector3(0f, -0f, 175.350952f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 196690, 2127932792u, new Vector3(-1396.45032f, 6751.85449f, 8.967286f), new Vector3(0f, 0f, 73.09007f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 599081, 1803116220u, new Vector3(-1388.71582f, 6743.434f, 8.300988f), new Vector3(0f, -0f, -177.792267f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 666154, 1803116220u, new Vector3(-1393.15234f, 6737.30762f, 8.300988f), new Vector3(0f, -0f, 133.533951f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 304470, 1803116220u, new Vector3(-1407.152f, 6741.70361f, 8.300988f), new Vector3(0f, 0f, 71.8091f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 663381, 1803116220u, new Vector3(-1424.46631f, 6747.268f, 8.300988f), new Vector3(0f, 0f, 71.29691f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 532562, 1803116220u, new Vector3(-1407.55981f, 6741.921f, 8.300988f), new Vector3(0f, 0f, -19.4982262f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 441174, 1803116220u, new Vector3(-1393.39343f, 6737.60059f, 8.300988f), new Vector3(0f, 0f, -44.26974f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 519765, 1803116220u, new Vector3(-1388.30652f, 6743.234f, 8.300988f), new Vector3(0f, 0f, -26.6632881f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 692566, 1803116220u, new Vector3(-1389.198f, 6749.27f, 8.300988f), new Vector3(0f, 0f, 5.529639f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 371286, 2127932792u, new Vector3(-1389.6825f, 6749.74756f, 8.967554f), new Vector3(0f, -0f, -158.190292f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 675160, 1803116220u, new Vector3(-1375.16f, 6742.035f, 5.21200943f), new Vector3(0f, -0f, 164.5478f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 24409, 1803116220u, new Vector3(-1364.149f, 18318f / MathF.E, 1.77475047f), new Vector3(0f, -0f, 164.1884f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 559406, 1803116220u, new Vector3(-1363.55933f, 6734.268f, 1.77563262f), new Vector3(0f, -0f, 93.60924f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 254809, 1803116220u, new Vector3(-1367.40076f, 6730.634f, 1.77465034f), new Vector3(0f, 0f, 60.3411674f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 530263, 1803116220u, new Vector3(-1377.18372f, 6734.05273f, 5.21159554f), new Vector3(0f, 0f, -17.4542751f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 59989, 2127932792u, new Vector3(-1375.58276f, 6742.181f, 5.886067f), new Vector3(0f, -0f, -107.941521f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			})
		};

		public static MissionOjects KeycardPickupsObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_card_hack"), 456214, 2467775808u, new Vector3(2102.40015f, 2940.67822f, -65.69253f), new Vector3(2.6680425E-08f, 90f, -87.7661743f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_card_hack"), 9561, 2467775808u, new Vector3(2092.01855f, 2940.14f, -65.69253f), new Vector3(-1.6008255E-07f, 90f, 1.90310407f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_card_hack"), 727114, 2467775808u, new Vector3(2085.75049f, 2943.46924f, -65.69253f), new Vector3(2.134436E-07f, 90.00001f, 16.7967358f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_card_hack"), 273254, 2467775808u, new Vector3(2079.6228f, 2943.94019f, -65.69253f), new Vector3(-4.268868E-07f, 89.9999847f, 29.6431465f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_card_hack"), 26725, 2467775808u, new Vector3(2073.33643f, 2945.4856f, -65.69253f), new Vector3(8.537736E-07f, 90f, 32.50922f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_card_hack"), 127562, 2467775808u, new Vector3(2060.57129f, 2947.093f, -65.69253f), new Vector3(4.268868E-07f, 90f, 34.9656563f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_card_hack"), 646468, 2467775808u, new Vector3(2055.701f, 2951.59f, -65.69253f), new Vector3(0f, 90.0000153f, 12.3946009f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_card_hack"), 5213, 2467775808u, new Vector3(2044.26587f, 2958.2085f, -65.69253f), new Vector3(-4.268868E-07f, 89.9999847f, 55.2329826f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_card_hack"), 225122, 2467775808u, new Vector3(2038.78882f, 2962.192f, -65.69253f), new Vector3(1.2806604E-06f, 89.9999847f, 37.67776f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_card_hack"), 708611, 2467775808u, new Vector3(2033.955f, 2966.70532f, -65.69253f), new Vector3(-1.7075464E-06f, 89.9999847f, 43.4099426f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_card_hack"), 43846, 2467775808u, new Vector3(2031.13879f, 2972.63037f, -65.69253f), new Vector3(3.84198074E-06f, 90f, 48.2720261f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_card_hack"), 533063, 2467775808u, new Vector3(2024.10791f, 2974.80273f, -65.69253f), new Vector3(8.53773543E-07f, 90f, 67.20893f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_card_hack"), 598632, 2467775808u, new Vector3(2030.04541f, 2973.54858f, -65.69253f), new Vector3(8.53773543E-07f, 90f, -63.6097679f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_card_hack"), 444775, 2467775808u, new Vector3(2032.92566f, 2967.25073f, -65.69253f), new Vector3(-1.28066006E-06f, 90f, -51.63325f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_card_hack"), 491619, 2467775808u, new Vector3(2042.56909f, 2958.625f, -65.69253f), new Vector3(-2.98820646E-06f, 89.9999847f, -50.81424f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_card_hack"), 648809, 2467775808u, new Vector3(2053.522f, 2950.19849f, -65.69253f), new Vector3(-1.707548E-06f, 90.0000153f, -28.49909f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_card_hack"), 751931, 2467775808u, new Vector3(2065.364f, 2945.32935f, -65.69253f), new Vector3(-2.56132034E-06f, 90f, -54.2942352f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_card_hack"), 694604, 2467775808u, new Vector3(2072.533f, 2947.714f, -65.69253f), new Vector3(0f, 90f, -19.5933571f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_card_hack"), 459370, 2467775808u, new Vector3(2084.285f, 2943.16235f, -65.69253f), new Vector3(-8.537736E-07f, 90f, -11.6090469f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_card_hack"), 217157, 2467775808u, new Vector3(2090.84155f, 2940.58472f, -65.69253f), new Vector3(-4.07110977E-13f, 89.9999847f, -30.29012f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_card_hack"), 106591, 2467775808u, new Vector3(2104.10767f, 2942.8396f, -65.69253f), new Vector3(-1.06721494E-07f, 90.00002f, 5.12734175f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_card_hack"), 697905, 2467775808u, new Vector3(2108.312f, 2942.90625f, -65.69253f), new Vector3(2.13443371E-07f, 90f, 6.6627636f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_card_hack"), 107873, 2467775808u, new Vector3(2115.82739f, 2944.87451f, -65.69253f), new Vector3(-3.201651E-07f, 90.00002f, 5.383238f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_card_hack"), 272462, 2467775808u, new Vector3(2114.24487f, 2942.46875f, -65.69253f), new Vector3(6.670104E-08f, 90f, -1.27031493f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static List<MissionOjects> IAAFacNavs = new List<MissionOjects>
		{
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 467270, 1803116220u, new Vector3(2149.85327f, 2920.29175f, -62.5724144f), new Vector3(0f, 0f, 56.68479f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 758326, 1803116220u, new Vector3(2132.705f, 2919.70117f, -62.4623032f), new Vector3(0f, -0f, 90.15682f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 327468, 1803116220u, new Vector3(2116.09424f, 2920.956f, -62.5724144f), new Vector3(0f, 0f, 86.98331f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 466761, 1803116220u, new Vector3(2097.12671f, 2925.22559f, -62.5724144f), new Vector3(0f, 0f, 77.25866f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 446538, 1803116220u, new Vector3(2076.50952f, 2930.185f, -62.57227f), new Vector3(0f, 0f, 78.89627f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 70732, 1803116220u, new Vector3(2055.52075f, 2932.057f, -62.5722656f), new Vector3(0f, 0f, 86.16369f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 605994, 1803116220u, new Vector3(2039.94543f, 2939.354f, -62.57227f), new Vector3(0f, 0f, 69.0178452f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 732742, 1803116220u, new Vector3(2055.22388f, 2932.38232f, -62.57227f), new Vector3(0f, -0f, -153.816788f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 176944, 1803116220u, new Vector3(2076.49878f, 2930.34326f, -62.57227f), new Vector3(0f, -0f, -101.151039f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 298546, 1803116220u, new Vector3(2096.59082f, 2925.28052f, -62.5724144f), new Vector3(0f, -0f, -104.170494f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 157245, 1803116220u, new Vector3(2115.91821f, 2921.07f, -62.4124451f), new Vector3(0f, -0f, -106.422134f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 128056, 1803116220u, new Vector3(2132.532f, 2919.74023f, -62.3671074f), new Vector3(0f, -0f, -98.53997f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 12361, 1803116220u, new Vector3(2149.59839f, 2920.24048f, -62.5724144f), new Vector3(0f, 0f, -86.56324f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "High Security 4"), 478589, 1442749254u, new Vector3(2150.35376f, 2920.45313f, -61.89752f), new Vector3(0f, 0f, 87.7085953f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 431137, 1803116220u, new Vector3(2115.26147f, 2932.51025f, -62.5724144f), new Vector3(0f, 0f, 2.49179745f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 369232, 1803116220u, new Vector3(2115.27466f, 2927.049f, -62.5724144f), new Vector3(0f, -0f, -92.6027f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 218448, 1803116220u, new Vector3(2114.71631f, 2923.24976f, -62.5724144f), new Vector3(0f, 0f, -30.571106f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 119359, 1803116220u, new Vector3(2098.63477f, 2927.30469f, -62.5724144f), new Vector3(0f, 0f, -15.8309164f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 360245, 1803116220u, new Vector3(2099.48462f, 2933.02637f, -62.5724144f), new Vector3(0f, 0f, -10.8151369f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 666677, 1803116220u, new Vector3(2085.297f, 2933.678f, -62.5724144f), new Vector3(0f, 0f, 0.291169435f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 730449, 1803116220u, new Vector3(2099.74756f, 2933.12524f, -62.5724144f), new Vector3(0f, 0f, 1.46833444f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 512593, 1803116220u, new Vector3(2098.61182f, 2927.64624f, -62.5724144f), new Vector3(0f, 0f, 0.1887865f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 511037, 1803116220u, new Vector3(2114.56372f, 2923.21021f, -62.1954575f), new Vector3(0f, 0f, -11.7363482f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 89382, 1803116220u, new Vector3(2115.15552f, 2926.85352f, -62.5724144f), new Vector3(0f, 0f, -10.4056044f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 7448, 1803116220u, new Vector3(2115.26538f, 2932.03955f, -62.5724144f), new Vector3(0f, 0f, -5.082753f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "High Security 4"), 128824, 1442749254u, new Vector3(2115.08325f, 2932.97021f, -61.89868f), new Vector3(0f, -0f, -168.708282f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 200718, 1803116220u, new Vector3(2116.73877f, 2936.13086f, -66.17236f), new Vector3(0f, -0f, -173.825912f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 449848, 1803116220u, new Vector3(2105.631f, 2935.47583f, -66.17236f), new Vector3(0f, -0f, -177.510712f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 316493, 1803116220u, new Vector3(2097.439f, 2935.50049f, -62.5724144f), new Vector3(0f, -0f, 105.205566f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 294461, 1803116220u, new Vector3(2097.3938f, 2944.3147f, -62.57241f), new Vector3(0f, 0f, 81.30362f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 481598, 1803116220u, new Vector3(2097.54175f, 2935.517f, -62.5724144f), new Vector3(0f, -0f, 111.500122f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 607571, 1803116220u, new Vector3(2105.28149f, 2935.57f, -66.17236f), new Vector3(0f, -0f, -178.228333f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 96086, 1803116220u, new Vector3(2116.926f, 2936.32861f, -66.17236f), new Vector3(0f, -0f, -175.771362f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "High Security 4"), 2370, 1442749254u, new Vector3(2117.702f, 2936.31934f, -65.5069962f), new Vector3(0f, 0f, 86.70058f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 445269, 1803116220u, new Vector3(2047.39966f, 2957.28955f, -62.5722656f), new Vector3(0f, -0f, 148.117157f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 260691, 1803116220u, new Vector3(2061.58154f, 2950.1936f, -62.57227f), new Vector3(0f, -0f, 155.384155f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 359479, 1803116220u, new Vector3(2097.06641f, 2944.03271f, -62.5724144f), new Vector3(0f, -0f, -169.198883f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 779349, 1803116220u, new Vector3(2118.20361f, 2944.22f, -62.5724144f), new Vector3(0f, -0f, -170.426361f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 627782, 1803116220u, new Vector3(2097.41431f, 2943.87939f, -62.5724144f), new Vector3(0f, -0f, 178.875931f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 554819, 1803116220u, new Vector3(2061.22266f, 2950.61279f, -62.57227f), new Vector3(0f, -0f, 171.249359f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 51033, 1803116220u, new Vector3(2047.491f, 2957.41528f, -62.5722656f), new Vector3(0f, -0f, 151.3906f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "High Security 4"), 17497, 1442749254u, new Vector3(2046.91956f, 2957.60571f, -61.903f), new Vector3(0f, -0f, -124.723404f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 94811, 1803116220u, new Vector3(2041.32043f, 2962.09766f, -62.57227f), new Vector3(0f, 0f, -54.20369f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 57918, 1803116220u, new Vector3(2036.85474f, 2965.64453f, -62.57227f), new Vector3(0f, 0f, -53.2309952f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 438330, 1803116220u, new Vector3(2030.11633f, 2972.109f, -62.5722656f), new Vector3(0f, 0f, -51.0300827f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 419928, 1803116220u, new Vector3(2024.6239f, 2979.66943f, -62.57227f), new Vector3(0f, 0f, -44.5811424f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 398172, 1803116220u, new Vector3(2029.96082f, 2971.9668f, -62.286808f), new Vector3(0f, 0f, -52.4117355f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 642369, 1803116220u, new Vector3(2036.68811f, 2965.7915f, -62.57227f), new Vector3(0f, 0f, -36.9037476f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 683865, 1803116220u, new Vector3(2041.07532f, 2962.1814f, -62.57227f), new Vector3(0f, 0f, -38.49032f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "High Security 4"), 259419, 1442749254u, new Vector3(2041.51648f, 2961.75854f, -61.8967323f), new Vector3(-1.115799f, 0.1902071f, 40.2255249f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 75611, 1803116220u, new Vector3(2017.72388f, 2975.40259f, -66.17256f), new Vector3(0f, -0f, 128.206421f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 255060, 1803116220u, new Vector3(2030.256f, 2962.79858f, -66.17256f), new Vector3(0f, -0f, 135.319748f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 122450, 1803116220u, new Vector3(2046.89258f, 2949.2876f, -66.17256f), new Vector3(0f, -0f, -110.085266f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 277340, 1803116220u, new Vector3(2061.403f, 2943.08032f, -66.17256f), new Vector3(0f, -0f, -115.817123f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 649755, 1803116220u, new Vector3(2087.10278f, 2937.10767f, -66.17236f), new Vector3(0f, 0f, -89.86733f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 152413, 1803116220u, new Vector3(2102.4585f, 2937.63354f, -66.17236f), new Vector3(0f, 0f, -85.7726059f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 30273, 1803116220u, new Vector3(2116.42358f, 2937.24072f, -66.17236f), new Vector3(0f, -0f, -90.53229f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 140381, 1803116220u, new Vector3(2102.15454f, 2937.55566f, -66.17236f), new Vector3(0f, -0f, 179.337692f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 524639, 1803116220u, new Vector3(2086.83887f, 2937.10278f, -66.17236f), new Vector3(0f, -0f, -167.969269f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 247350, 1803116220u, new Vector3(2060.90845f, 2943.31763f, -66.17256f), new Vector3(0f, -0f, 158.250427f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 366891, 1803116220u, new Vector3(2046.86255f, 2949.49536f, -66.17256f), new Vector3(0f, -0f, 150.675461f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 487775, 1803116220u, new Vector3(2030.48682f, 2962.28345f, -66.17256f), new Vector3(0f, -0f, 119.40345f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 561493, 1803116220u, new Vector3(2017.8634f, 2974.953f, -66.17256f), new Vector3(0f, -0f, 113.36348f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "High Security 4"), 654666, 1442749254u, new Vector3(2017.46313f, 2975.99414f, -65.50648f), new Vector3(0f, -0f, -138.234436f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			})
		};

		public static bool CreatedKeycardObjects;

		public static bool CreatedC4IntelObjects;

		public static Vector3 YachtLoc = new Vector3(-1418.21f, 6749.811f, 10.98968f);

		public static Vector3 VIPInteLoc = new Vector3(-2573.35254f, 1861.861f, 168.175323f);

		public static Vector3 LongfinLoc = new Vector3(1560.25562f, 6678.969f, 0.5311283f);

		public static Vector3 IntelKeycardEnter = new Vector3(66.07f, 6654.66f, 31.2f);

		public static Vector3 IntelKeycardExit = new Vector3(2034.01f, 2942.14f, -62.9f);

		public static Vector3 BunkerC4Enter = new Vector3(-388.814f, 4340.98f, 55.15f);

		public static Vector3 BunkerC4Exit = new Vector3(896.07f, -3245.97f, -99.24f);

		public static Prop PrimaryVaultDoor;

		public static Prop SecondaryVaultDoorA;

		public static Prop SecondaryVaultDoorB;

		public static Prop TerciaryVaultDoorA;

		public static Prop TerciaryVaultDoorB;

		public static bool GotLongfin;

		public static bool isEnteringAsVIP;

		public static bool StartedSuperVolitoRoute;

		public static bool PrimaryVaultOpen;

		public static bool SecondaryVaultOpen;

		public static bool TerciaryVaultOpen;

		public static bool SpawnedYacht;

		public static Prop Keycard;

		public static List<Blip> IntelMarkers = new List<Blip>();

		public static List<Blip> IntelMarkersRadiusArea = new List<Blip>();

		public static Blip TargetBlip;

		public static Blip TargetBlipArea;

		public Vector3 ShipUIPos = new Vector3(-1411.312f, 6749.511f, 0f);

		public float ShipUIHeading;

		public static KeypadClass MainVaultKeypad;

		public static bool HackedMainKeypad;

		public static KeypadClass SecondaryVaultKeypad;

		public static bool HackedSecondaryKeypad;

		public static KeypadClass TerciaryVaultKeypad;

		public static bool HackedTerciaryKeypad;

		public static bool TheCartelYachtHeistOn;

		public static Vehicle Longfin;

		public static Vehicle SuperVolito;

		public static Ped Pilot;

		public static bool UsingLongfin;

		public static bool UsingSuperVolito;

		public static string GStyle = "bunker_style_c";

		public static string Upgrade1 = "upgrade_bunker_set";

		public static string Upgrade2 = "standard_bunker_set_more";

		public static string Upgrade3 = "standard_security_set";

		public static int PercentNeedTake = (int)((float)(HeistMaxPay + HeistMinPay) / 1.5f / 100f * 35f);

		public bool InBunker_C4;

		public bool InIAABase;

		public static HeistStage CurrentHeistStage;

		public static int plantBombStage;

		public static Prop Bomb;

		public static Prop Bag;

		public static int BombExplodeTimer;

		public static int BombSyncScene;

		public Vector3 BombPlacementLoc1 = new Vector3(-1374.96f, 6737.84f, 2.566f);

		public Vector3 BombPlacementRot1 = new Vector3(0f, 0f, 77.08f);

		public Vector3 BombPlacementLoc2 = new Vector3(-1434.561f, 6753.652f, 5.88f);

		public Vector3 BombPlacementRot2 = new Vector3(0f, 0f, -13.51f);

		public Vector3 BombPlacementLoc3 = new Vector3(-1407.7f, 6748.616f, 11.9f);

		public Vector3 BombPlacementRot3 = new Vector3(0f, 0f, 77.08f);

		public static bool heistCompleteCalled;

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

		public static void ConvertNavToSmartAIV2(List<MissionOjects> Navs, List<SmartGuards.SmartAiV2> AIGroup)
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
						SmartGuards.SmartAiV2 smartAiV = new SmartGuards.SmartAiV2(0, objectPlacementMap.Inputtedhash, objectPlacementMap.Position, objectPlacementMap.Rotation.Z, true, 200, 100, 1, wL, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel);
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

		public static void ATTACH_ENTITY_TO_ENTITY(Entity entity1, Entity entity2, int boneIndex, float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, bool p9, bool useSoftPinning, bool collision, bool isPed, int vertexIndex, bool fixedRot)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, entity1, entity2, boneIndex, xPos, yPos, zPos, xRot, yRot, zRot, p9, useSoftPinning, collision, isPed, vertexIndex, fixedRot);
		}

		public YachtHeist()
		{
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
			if (MainVaultKeypad != null && MainVaultKeypad.Keypad != null && MainVaultKeypad.Keypad.AttachedBlip != null)
			{
				MainVaultKeypad.Keypad.AttachedBlip.Delete();
			}
			if (SecondaryVaultKeypad != null && SecondaryVaultKeypad.Keypad != null && SecondaryVaultKeypad.Keypad.AttachedBlip != null)
			{
				SecondaryVaultKeypad.Keypad.AttachedBlip.Delete();
			}
			if (TerciaryVaultKeypad != null && TerciaryVaultKeypad.Keypad != null && TerciaryVaultKeypad.Keypad.AttachedBlip != null)
			{
				TerciaryVaultKeypad.Keypad.AttachedBlip.Delete();
			}
			ManageRobberyAnims.DeleteLoot();
			Function.Call(Hash.SET_RADAR_ZOOM, 850);
			ManageRobberyAnims.CurrentPay = 0;
			TheCartelYachtHeistOn = false;
			heistCompleteCalled = false;
			StartedSuperVolitoRoute = false;
			TerciaryVaultOpen = false;
			foreach (SmartGuards.SmartAiV2 c4Guard in C4Guards)
			{
				if (c4Guard != null && c4Guard.AIPed != null)
				{
					c4Guard.AIPed.Delete();
				}
			}
			foreach (SmartGuards.SmartAiV2 iAAGuard in IAAGuards)
			{
				if (iAAGuard != null && iAAGuard.AIPed != null)
				{
					iAAGuard.AIPed.Delete();
				}
			}
			foreach (SmartGuards.SmartAiV2 longfinGuard in LongfinGuards)
			{
				if (longfinGuard != null && longfinGuard.AIPed != null)
				{
					longfinGuard.AIPed.Delete();
				}
			}
			foreach (SmartGuards.SmartAiV2 mansionGuard in MansionGuards)
			{
				if (mansionGuard != null && mansionGuard.AIPed != null)
				{
					mansionGuard.AIPed.Delete();
				}
			}
			foreach (SmartGuards.SmartAiV2 yachtGuard in YachtGuards)
			{
				if (yachtGuard != null && yachtGuard.AIPed != null)
				{
					yachtGuard.AIPed.Delete();
				}
			}
			plantBombStage = 0;
			if (Bomb != null)
			{
				Bomb.Delete();
			}
			if (Bag != null)
			{
				Bag.Delete();
			}
			BombExplodeTimer = 0;
			BombSyncScene = 0;
			CurrentHeistStage = HeistStage.Scout;
			UsingLongfin = false;
			UsingSuperVolito = false;
			LongfinObjects.DeleteEntites();
			DrillObjets.DeleteEntites();
			foreach (SmartGuards.SmartAiV2 drillGuard in DrillGuards)
			{
				if (drillGuard != null && drillGuard.AIPed != null)
				{
					drillGuard.AIPed.Delete();
				}
			}
			CreatedKeycardObjects = false;
			C4PickupObjects.DeleteEntites();
			CreatedVIPMansionObjects = false;
			C4Objects.DeleteEntites();
			MansionGuardsObjects.DeleteEntites();
			YachtObjects.DeleteEntites();
			MansionObjects.DeleteEntites();
			KeycardPickupsObjects.DeleteEntites();
			GotGrinder = false;
			GotC4 = false;
			GotDrill = false;
			C4ChargesCollected = 0;
			GotThermite = false;
			ThermiteChargesCollected = 0;
			GotKeycard = false;
			CurrentPay = 0;
			LocalTake = 0;
			CreatedC4IntelObjects = false;
			CreatedLongfinObjects = false;
			CreatedVIPMansionObjects = false;
			GotLongfin = false;
			isEnteringAsVIP = false;
			PrimaryVaultOpen = false;
			SecondaryVaultOpen = false;
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
			YachtLoot.DeleteEntites();
			YachtObjects.DeleteEntites();
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
			HackedMainKeypad = false;
			HackedSecondaryKeypad = false;
			HackedTerciaryKeypad = false;
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
								if (KeyPad == TerciaryVaultKeypad)
								{
									HackedTerciaryKeypad = true;
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
					if (KeyPad == TerciaryVaultKeypad)
					{
						HackedTerciaryKeypad = true;
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

		public static void LOADIPL(string IPL)
		{
			Function.Call(Hash.REQUEST_IPL, IPL);
		}

		public static void StartHeist()
		{
			PercentNeedTake = (int)((float)(HeistMinPay + HeistMaxPay) / 1.5f / 100f * 35f);
			LOADIPL("gr_grdlc_interior_placement_interior_1_grdlc_int_02_milo_");
			LOADIPL("gr_Heist_Yacht2");
			LOADIPL("gr_Heist_Yacht2_enginrm");
			LOADIPL("gr_Heist_Yacht2_Lounge");
			LOADIPL("gr_Heist_Yacht2_Bridge");
			LOADIPL("gr_Heist_Yacht2_Bar");
			LOADIPL("gr_Heist_Yacht2_Bedrm");
			LOADIPL("gunrunning_yacht_jacuzzi");
			LOADIPL("gr_grdlc_interior_placement_interior_1_grdlc_int_01_milo_");
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, 258561, GStyle);
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, 258561, Upgrade1);
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, 258561, Upgrade2);
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, 258561, Upgrade3);
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, 258561, "Office_Upgrade_set");
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, 258561, "gun_range_lights");
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, 258561, "gun_schematic_set");
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, 258561, "gun_locker_upgrade");
			Function.Call(Hash.REFRESH_INTERIOR, 258561);
			LOADIPL("gr_case0_bunkerclosed");
			LOADIPL("gr_case1_bunkerclosed");
			LOADIPL("gr_case2_bunkerclosed");
			LOADIPL("gr_case3_bunkerclosed");
			LOADIPL("gr_case4_bunkerclosed");
			LOADIPL("gr_case5_bunkerclosed");
			LOADIPL("gr_case6_bunkerclosed");
			LOADIPL("gr_case7_bunkerclosed");
			LOADIPL("gr_case9_bunkerclosed");
			LOADIPL("gr_case10_bunkerclosed");
			LOADIPL("gr_case11_bunkerclosed");
			ManageRobberyAnims.Heist_Average_Pay = (float)(HeistMinPay + HeistMaxPay) / 2f;
			YachtGuards.Clear();
			ConvertNavToSmartAIV2(YachtNavs, YachtGuards);
			Function.Call(Hash.BEGIN_TEXT_COMMAND_BUSYSPINNER_ON, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "Loading The Cartel Yacht Heist, Please Wait");
			Function.Call(Hash.END_TEXT_COMMAND_BUSYSPINNER_ON, 5);
			EndHeist();
			if (TargetBlip == null)
			{
				TargetBlip = GlobalHelper.CreateBlip(YachtLoc);
				TargetBlip.Sprite = BlipSprite.Yacht;
				TargetBlip.Color = BlipColor.Yellow;
				TargetBlip.Name = "Cartel Yacht";
				TargetBlip.IsShortRange = true;
			}
			if (TargetBlipArea == null)
			{
				TargetBlipArea = GlobalHelper.CreateBlip(YachtLoc, 100f);
				TargetBlipArea.Color = BlipColor.Yellow;
				TargetBlipArea.Name = "Cartel Yacht";
				TargetBlipArea.IsShortRange = true;
				TargetBlipArea.Alpha = 144;
			}
			YachtLoot.CreateEntites();
			YachtObjects.CreateEntites();
			SpawnedYacht = true;
			foreach (ObjectPlacementMap entity in YachtLoot.Entities)
			{
				if (entity.Entity != null)
				{
					ManageRobberyAnims.AddPropAsLoot((Prop)entity.Entity);
				}
			}
			MainVaultKeypad = new KeypadClass((Prop)YachtObjects.Entities[0].Entity, 1);
			SecondaryVaultKeypad = new KeypadClass((Prop)YachtObjects.Entities[1].Entity, 1);
			TerciaryVaultKeypad = new KeypadClass((Prop)YachtObjects.Entities[2].Entity, 1);
			PrimaryVaultDoor = (Prop)YachtObjects.Entities[3].Entity;
			SecondaryVaultDoorA = (Prop)YachtObjects.Entities[4].Entity;
			SecondaryVaultDoorB = (Prop)YachtObjects.Entities[5].Entity;
			TerciaryVaultDoorA = (Prop)YachtObjects.Entities[6].Entity;
			TerciaryVaultDoorB = (Prop)YachtObjects.Entities[7].Entity;
			Blip blip = GlobalHelper.CreateBlip(VIPInteLoc);
			blip.Sprite = BlipSprite.HeistSetup;
			blip.Color = BlipColor.Green;
			blip.IsShortRange = true;
			blip.Scale = 0.5f;
			blip.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip);
			blip = GlobalHelper.CreateBlip(VIPInteLoc, 100f);
			blip.Name = "Possible Intel/Asset";
			blip.Color = BlipColor.Green;
			blip.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip);
			blip = GlobalHelper.CreateBlip(LongfinLoc);
			blip.Sprite = BlipSprite.HeistSetup;
			blip.Color = BlipColor.Green;
			blip.IsShortRange = true;
			blip.Scale = 0.5f;
			blip.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip);
			blip = GlobalHelper.CreateBlip(LongfinLoc, 100f);
			blip.Name = "Possible Intel/Asset";
			blip.Color = BlipColor.Green;
			blip.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip);
			blip = GlobalHelper.CreateBlip(BunkerC4Enter);
			blip.Sprite = BlipSprite.HeistSetup;
			blip.Color = BlipColor.Green;
			blip.IsShortRange = true;
			blip.Scale = 0.5f;
			blip.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip);
			blip = GlobalHelper.CreateBlip(BunkerC4Enter, 100f);
			blip.Name = "Possible Intel/Asset";
			blip.Color = BlipColor.Green;
			blip.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip);
			blip = GlobalHelper.CreateBlip(IntelKeycardEnter);
			blip.Sprite = BlipSprite.HeistSetup;
			blip.Color = BlipColor.Green;
			blip.IsShortRange = true;
			blip.Scale = 0.5f;
			blip.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip);
			blip = GlobalHelper.CreateBlip(IntelKeycardEnter, 100f);
			blip.Name = "Possible Intel/Asset";
			blip.Color = BlipColor.Green;
			blip.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip);
			blip = GlobalHelper.CreateBlip(DrillIntelLoc);
			blip.Sprite = BlipSprite.HeistSetup;
			blip.Color = BlipColor.Green;
			blip.IsShortRange = true;
			blip.Scale = 0.5f;
			blip.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip);
			blip = GlobalHelper.CreateBlip(DrillIntelLoc, 100f);
			blip.Name = "Possible Intel/Asset";
			blip.Color = BlipColor.Green;
			blip.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip);
			int index = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, DrillObjets.Entities.Count);
			DrillObjets.Entities[index].CreateObjatCoords();
			if (DrillObjets.Entities[index].Entity != null)
			{
				Drill = (Prop)DrillObjets.Entities[index].Entity;
			}
			ConvertNavToSmartAIV2(DrillNavs, DrillGuards);
			Function.Call(Hash.BUSYSPINNER_OFF);
			CurrentHeistStage = HeistStage.Scout;
			TheCartelYachtHeistOn = true;
		}

		public static void CompleteHeist()
		{
			Main.TotalCrewEarned += 0f;
			Main.TotalPlayerEarned += CurrentPay;
			XP.CompleteHeist_TheCartelYacht++;
			Main.SaveCuts();
			Class1.SetNoLongerInHeist();
			MissionScreen.SetPass("The Cartel Yacht Heist", CurrentPay, 0, "The Player has died", 2, 0f, 5);
			int num = Game.GameTime + 30000;
			while (HKHModHelperNew.Class1.HeistPassedScreen != -1 && Game.GameTime <= num)
			{
				Script.Wait(0);
			}
			Script.Wait(2000);
			XP.AddXP_Percent(XP.CompleteHeist_XPPercent_TheCartelYacht);
			Game.Player.Money += CurrentPay;
			EndHeist();
			CurrentPay = 0;
			LocalTake = 0;
			EndHeist();
			TheCartelYachtHeistOn = false;
		}

		public static void FailHeist(string reason)
		{
			Class1.SetNoLongerInHeist();
			HKHModHelperNew.Class1.scaleformDescription = "The Cartel Yacht Heist was failed " + reason;
			HKHModHelperNew.Class1.scaleformTitle = "The Cartel Yacht Heist was Failed";
			HKHModHelperNew.Class1.ScaleformColor = HKHModHelperNew.Class1.ScaleformColour.Red;
			HKHModHelperNew.Class1.ShowScaleform = true;
			HKHModHelperNew.Class1.Passed_Index = 0;
			EndHeist();
			TheCartelYachtHeistOn = false;
		}

		private void OnTick(object sender, EventArgs e)
		{
			Vector3 offsetPosition = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0.45f, 0f));
			Vector3 rotation = Game.Player.Character.Rotation;
			if (!TheCartelYachtHeistOn)
			{
				return;
			}
			switch (CurrentHeistStage)
			{
			case HeistStage.Scout:
				if (Game.Player.Character.CurrentVehicle == Longfin && UsingLongfin)
				{
					CurrentHeistStage = HeistStage.InLongfin;
				}
				if (Game.Player.Character.CurrentVehicle == SuperVolito && UsingSuperVolito)
				{
					CurrentHeistStage = HeistStage.InSuperVolito;
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, VIPInteLoc, "<", 400f))
				{
					CurrentHeistStage = HeistStage.VIPEnterPrePurchase;
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, YachtLoc, "<", 250f))
				{
					if (!GotC4 && !GotDrill && !GotKeycard)
					{
						CurrentHeistStage = HeistStage.NoWaytoOpenVaults;
					}
					else
					{
						CurrentHeistStage = HeistStage.SearchYachtForLoot;
					}
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, DrillIntelLoc, "<", 350f) && !GotDrill)
				{
					CurrentHeistStage = HeistStage.Drill;
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, LongfinLoc, "<", 150f))
				{
					CurrentHeistStage = HeistStage.longfin;
				}
				break;
			case HeistStage.C4:
				if (C4Guards.Count > 1 && CurrentActiveGroup == ActiveGroup.YachtHeist_Bunker)
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
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightAdd = 10f;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightMinus = 10f;
					SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
				}
				break;
			case HeistStage.DisposableKeycard:
				if (IAAGuards.Count > 1 && CurrentActiveGroup == ActiveGroup.YachtHeist_IAAFacility)
				{
					CurrentHeistStage = HeistStage.DisposableKeycard;
					SmartGuards.GetCurrentGroupSettings().ShowCone = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipRadius = 100f;
					SmartGuards.GetCurrentGroupSettings().Radius = 80f;
					SmartGuards.GetCurrentGroupSettings().SeeingAngle = 35f;
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
			case HeistStage.Drill:
				if (DrillGuards.Count > 1 && CurrentActiveGroup == ActiveGroup.YachtHeist_DrillIntel)
				{
					CurrentHeistStage = HeistStage.Drill;
					SmartGuards.GetCurrentGroupSettings().ShowCone = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipRadius = 100f;
					SmartGuards.GetCurrentGroupSettings().Radius = 80f;
					SmartGuards.GetCurrentGroupSettings().SeeingAngle = 35f;
					SmartGuards.GetCurrentGroupSettings().AllowInvestigate = true;
					SmartGuards.GetCurrentGroupSettings().InvestigateDist = 40f;
					SmartGuards.GetCurrentGroupSettings().HitRadius = 30f;
					SmartGuards.GetCurrentGroupSettings().ArrestInsteadOfShoot = false;
					SmartGuards.GetCurrentGroupSettings().UseShowBlipWhenSameHeightWithPlayer = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightAdd = 10f;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightMinus = 10f;
					SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, DrillIntelLoc, "<", 350f))
				{
					if (!GotDrill)
					{
						CurrentHeistStage = HeistStage.Drill;
					}
					if (CurrentActiveGroup != ActiveGroup.YachtHeist_DrillIntel)
					{
						SmartGuards.GuardGroup[5].GroupList.Clear();
						SmartGuards.GuardGroup[5].GroupList.AddRange(DrillGuards);
						SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Cartel;
						CurrentActiveGroup = ActiveGroup.YachtHeist_DrillIntel;
					}
					if (Drill != null && !GotDrill)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, Drill.Position, "<", 30f))
						{
							World.DrawMarker(MarkerType.Chevron1, Drill.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), Vector3.Zero, new Vector3(0f, 180f, 0f), new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 255, 0), true, true);
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, Drill.Position, "<", 10f) && Drill.AttachedBlip == null)
						{
							Drill.AddBlip();
							Drill.AttachedBlip.Sprite = BlipSprite.BoltCutters;
							Drill.AttachedBlip.Name = "Drill";
							Drill.AttachedBlip.Color = BlipColor.Green;
							Drill.AttachedBlip.IsShortRange = true;
							Drill.AttachedBlip.IsFlashing = true;
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, Drill.Position, "<", 2.5f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Pickup Drill");
							if (Game.IsControlJustPressed(Control.Context))
							{
								IntelMarkers[4].Alpha = 0;
								IntelMarkersRadiusArea[4].Alpha = 0;
								if (Drill.AttachedBlip != null)
								{
									Drill.AttachedBlip.Alpha = 0;
								}
								EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(Drill, 0));
								TextNotification("CHAR_HUMANDEFAULT", "~y~The Cartel Yacht Heist~w~", "Intel Found", "You have found the Drill to open Cartel Yacht's Vault Doors");
								GotDrill = true;
							}
						}
					}
					if (GotDrill && CurrentHeistStage == HeistStage.Drill)
					{
						CurrentHeistStage = HeistStage.Scout;
					}
				}
				else
				{
					if (CurrentActiveGroup == ActiveGroup.YachtHeist_DrillIntel)
					{
						SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.None;
						CurrentActiveGroup = ActiveGroup.None;
					}
					if (CurrentHeistStage == HeistStage.Drill)
					{
						CurrentHeistStage = HeistStage.Scout;
					}
				}
				break;
			case HeistStage.InLongfin:
				if (GetDistance_VDIST2(Game.Player.Character.Position, YachtLoc, "<", 150f))
				{
					if (!GotDrill && !GotC4 && !GotGrinder)
					{
						CurrentHeistStage = HeistStage.NoWaytoOpenVaults;
					}
					else
					{
						CurrentHeistStage = HeistStage.SearchYachtForLoot;
					}
				}
				break;
			case HeistStage.InSuperVolito:
				if (Game.Player.Character.CurrentVehicle != SuperVolito && GetDistance_VDIST2(Game.Player.Character.Position, YachtLoc, ">", 150f))
				{
					CurrentHeistStage = HeistStage.Scout;
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, YachtLoc, "<", 150f))
				{
					if (!GotDrill && !GotC4 && !GotGrinder)
					{
						CurrentHeistStage = HeistStage.NoWaytoOpenVaults;
					}
					else
					{
						CurrentHeistStage = HeistStage.SearchYachtForLoot;
					}
				}
				break;
			case HeistStage.longfin:
				if (Game.Player.Character.CurrentVehicle != Longfin && GetDistance_VDIST2(Game.Player.Character.Position, YachtLoc, ">", 150f))
				{
					CurrentHeistStage = HeistStage.Scout;
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, LongfinLoc, ">", 250f))
				{
					CurrentHeistStage = HeistStage.Scout;
				}
				if (LongfinGuards.Count > 1 && CurrentActiveGroup == ActiveGroup.YachtHeist_LongfinGuards)
				{
					if (!UsingLongfin)
					{
						CurrentHeistStage = HeistStage.longfin;
					}
					else
					{
						CurrentHeistStage = HeistStage.InLongfin;
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
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightAdd = 10f;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightMinus = 10f;
					SmartGuards.GetCurrentGroupSettings().IngorePlayer = true;
				}
				break;
			case HeistStage.NoWaytoOpenVaults:
				if (GetDistance_VDIST2(Game.Player.Character.Position, YachtLoc, ">", 250f))
				{
					CurrentHeistStage = HeistStage.Scout;
				}
				break;
			case HeistStage.SearchYachtForLoot:
				if (YachtGuards.Count > 1 && CurrentActiveGroup == ActiveGroup.YachtHeist_YachtGuards)
				{
					SmartGuards.GetCurrentGroupSettings().ShowCone = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipRadius = 100f;
					SmartGuards.GetCurrentGroupSettings().Radius = 80f;
					SmartGuards.GetCurrentGroupSettings().SeeingAngle = 35f;
					SmartGuards.GetCurrentGroupSettings().AllowInvestigate = true;
					SmartGuards.GetCurrentGroupSettings().InvestigateDist = 40f;
					SmartGuards.GetCurrentGroupSettings().HitRadius = 30f;
					SmartGuards.GetCurrentGroupSettings().ArrestInsteadOfShoot = false;
					SmartGuards.GetCurrentGroupSettings().AimAtInsteadOfShoot = true;
					SmartGuards.GetCurrentGroupSettings().GuardSubtitleIfAiming = "~r~Guard~w~ : Sir Please lower the firearm if you wish to enter the property";
					SmartGuards.GetCurrentGroupSettings().UseShowBlipWhenSameHeightWithPlayer = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightAdd = 6f;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightMinus = 1.5f;
					SmartGuards.GetCurrentGroupSettings().UseFollowNavMeshRoute = false;
					if (isEnteringAsVIP || !HackingKeypad)
					{
						SmartGuards.GetCurrentGroupSettings().IngorePlayer = true;
					}
					else
					{
						SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
					}
				}
				if (GotDrill)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, BombPlacementLoc1, "<", 20f) && !PrimaryVaultOpen)
					{
						Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, BombPlacementLoc1.X, BombPlacementLoc1.Y, BombPlacementLoc1.Z, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_door_01a"), 0, 0, 0);
						if (DrillingMinigames.FleecaDrillFullyDone)
						{
							DrillingMinigames.FleecaDrillFullyDone = false;
							PrimaryVaultOpen = true;
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, BombPlacementLoc1, "<", 2f) && !PrimaryVaultOpen)
						{
							if (!DrillingMinigames.FleecaDrillActive && DrillingMinigames.CurrentDrill == DrillingMinigames.DrillUsing.None)
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Drill Open  ~r~Vault Door.", 1);
							}
							if (Game.IsControlJustPressed(Control.Context) && !DrillingMinigames.FleecaDrillActive)
							{
								Game.Player.Character.Position = prop.GetOffsetPosition(new Vector3(1.3f, -1f, -1f));
								Game.Player.Character.Heading = prop.Heading;
								DrillingMinigames.StartFleecaDrill();
							}
						}
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, BombPlacementLoc2, "<", 20f) && !TerciaryVaultOpen)
					{
						Prop prop2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, BombPlacementLoc2.X, BombPlacementLoc2.Y, BombPlacementLoc2.Z, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_door_01a"), 0, 0, 0);
						if (DrillingMinigames.FleecaDrillFullyDone)
						{
							DrillingMinigames.FleecaDrillFullyDone = false;
							SecondaryVaultOpen = true;
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, BombPlacementLoc2, "<", 2f) && !TerciaryVaultOpen)
						{
							if (!DrillingMinigames.FleecaDrillActive && DrillingMinigames.CurrentDrill == DrillingMinigames.DrillUsing.None)
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Drill Open ~r~Vault Door.", 1);
							}
							if (Game.IsControlJustPressed(Control.Context) && !DrillingMinigames.FleecaDrillActive)
							{
								Game.Player.Character.Position = prop2.GetOffsetPosition(new Vector3(1.3f, -1f, -1f));
								Game.Player.Character.Heading = prop2.Heading;
								DrillingMinigames.StartFleecaDrill();
							}
						}
					}
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, YachtLoc, ">", 250f))
				{
					CurrentHeistStage = HeistStage.Scout;
				}
				if (MainVaultKeypad != null && !HackedMainKeypad && GotKeycard && !GotDrill && MainVaultKeypad != null && MainVaultKeypad.Keypad != null)
				{
					CheckKeypad(MainVaultKeypad);
				}
				if (SecondaryVaultKeypad != null && !HackedSecondaryKeypad && GotKeycard && !GotDrill && SecondaryVaultKeypad != null && SecondaryVaultKeypad.Keypad != null)
				{
					CheckKeypad(SecondaryVaultKeypad);
				}
				if (TerciaryVaultKeypad != null && !HackedTerciaryKeypad && GotKeycard && !GotDrill && TerciaryVaultKeypad != null && TerciaryVaultKeypad.Keypad != null)
				{
					CheckKeypad(TerciaryVaultKeypad);
				}
				if ((HackedMainKeypad || PrimaryVaultOpen) && PrimaryVaultDoor != null)
				{
					if (PrimaryVaultDoor.IsAttached())
					{
						PrimaryVaultDoor.Detach();
					}
					if (PrimaryVaultDoor.Rotation.Z >= -100f)
					{
						Vector3 rotation2 = new Vector3(0f, 0f, PrimaryVaultDoor.Rotation.Z - 0.25f);
						PrimaryVaultDoor.Rotation = rotation2;
					}
				}
				if ((HackedSecondaryKeypad || SecondaryVaultOpen) && SecondaryVaultDoorA != null && SecondaryVaultDoorB != null)
				{
					if (SecondaryVaultDoorA.IsAttached())
					{
						SecondaryVaultDoorA.Detach();
					}
					if (SecondaryVaultDoorB.IsAttached())
					{
						SecondaryVaultDoorB.Detach();
					}
					if (SecondaryVaultDoorA.Rotation.Z >= -130f)
					{
						Vector3 rotation3 = new Vector3(0f, 0f, SecondaryVaultDoorA.Rotation.Z - 0.25f);
						SecondaryVaultDoorA.Rotation = rotation3;
					}
				}
				if ((HackedTerciaryKeypad || TerciaryVaultOpen) && TerciaryVaultDoorA != null && TerciaryVaultDoorB != null)
				{
					if (TerciaryVaultDoorA.IsAttached())
					{
						TerciaryVaultDoorA.Detach();
					}
					if (TerciaryVaultDoorB.IsAttached())
					{
						TerciaryVaultDoorB.Detach();
					}
					if ((double)TerciaryVaultDoorA.Position.Z >= 9.4)
					{
						Vector3 position = new Vector3(TerciaryVaultDoorA.Position.X, TerciaryVaultDoorA.Position.Y, TerciaryVaultDoorA.Position.Z - 0.05f);
						TerciaryVaultDoorA.Position = position;
					}
					if ((double)TerciaryVaultDoorB.Position.Z >= 9.4)
					{
						Vector3 position2 = new Vector3(TerciaryVaultDoorB.Position.X, TerciaryVaultDoorB.Position.Y, TerciaryVaultDoorB.Position.Z - 0.05f);
						TerciaryVaultDoorB.Position = position2;
					}
				}
				if (!GotC4 || GotKeycard || GotDrill)
				{
					break;
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, BombPlacementLoc1, "<", 20f) && !PrimaryVaultOpen)
				{
					switch (plantBombStage)
					{
					case 0:
					{
						if (!GetDistance_VDIST2(Game.Player.Character.Position, BombPlacementLoc1, "<", 2f) || PrimaryVaultOpen || C4ChargesCollected < 1)
						{
							break;
						}
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to plant a ~r~bomb.", 1);
						if (!Game.IsControlJustPressed(Control.Context))
						{
							break;
						}
						Game.Player.Character.Position = BombPlacementLoc1;
						C4ChargesCollected--;
						LoadDict("anim@scripted@heist@ig11_bomb_plant@male@");
						LoadDict("anim@scripted@player@mission@tun_bomb_plant@heeled@");
						Model model = new Model("ch_prop_ch_ld_bomb_01a");
						model.Request(250);
						if (model.IsValid && model.IsInCdImage)
						{
							while (!model.IsLoaded)
							{
								Script.Wait(50);
							}
							Bomb = GlobalHelper.CreateProp(model, Game.Player.Character.Position, false, false);
							Bomb.IsCollisionEnabled = false;
							plantBombStage = 10;
						}
						Model model2 = new Model("hei_p_m_bag_var22_arm_s");
						model2.Request(250);
						if (model2.IsValid && model2.IsInCdImage)
						{
							while (!model2.IsLoaded)
							{
								Script.Wait(50);
							}
							Bag = GlobalHelper.CreateProp(model2, Game.Player.Character.Position, false, false);
							Bag.IsCollisionEnabled = false;
							plantBombStage = 10;
						}
						break;
					}
					case 10:
						LoadDict("anim@scripted@heist@ig11_bomb_plant@male@");
						LoadDict("anim@scripted@player@mission@tun_bomb_plant@heeled@");
						BombSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z + 0.8f, 0f, 0f, rotation.Z, 2);
						Notification.Show("Position " + offsetPosition.ToString() + "/" + rotation.Z);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, BombSyncScene, "anim@scripted@player@mission@tun_bomb_plant@heeled@", "enter", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bomb, BombSyncScene, "enter_bomb", "anim@scripted@heist@ig11_bomb_plant@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bag, BombSyncScene, "enter_bag", "anim@scripted@heist@ig11_bomb_plant@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						plantBombStage = 20;
						break;
					case 20:
						if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, BombSyncScene) > 0.99f)
						{
							Bag.Delete();
							Game.Player.Character.Task.ClearAll();
							BombExplodeTimer = Game.GameTime + 5000;
							plantBombStage = -1;
						}
						break;
					case -1:
						if (Game.GameTime >= BombExplodeTimer)
						{
							World.AddExplosion(Bomb.Position, ExplosionType.PipeBomb, 1f, 1f, Game.Player.Character);
							if (Bomb != null)
							{
								Bomb.Delete();
							}
							plantBombStage = -2;
						}
						break;
					case -2:
						PrimaryVaultOpen = true;
						plantBombStage = 0;
						break;
					}
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, BombPlacementLoc3, "<", 20f) && !SecondaryVaultOpen)
				{
					switch (plantBombStage)
					{
					case 0:
					{
						if (!GetDistance_VDIST2(Game.Player.Character.Position, BombPlacementLoc3, "<", 2f) || SecondaryVaultOpen || C4ChargesCollected < 1)
						{
							break;
						}
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to plant a ~r~bomb.", 1);
						if (!Game.IsControlJustPressed(Control.Context))
						{
							break;
						}
						Game.Player.Character.Position = BombPlacementLoc3;
						C4ChargesCollected--;
						LoadDict("anim@scripted@heist@ig11_bomb_plant@male@");
						LoadDict("anim@scripted@player@mission@tun_bomb_plant@heeled@");
						Model model3 = new Model("ch_prop_ch_ld_bomb_01a");
						model3.Request(250);
						if (model3.IsValid && model3.IsInCdImage)
						{
							while (!model3.IsLoaded)
							{
								Script.Wait(50);
							}
							Bomb = GlobalHelper.CreateProp(model3, Game.Player.Character.Position, false, false);
							Bomb.IsCollisionEnabled = false;
							plantBombStage = 10;
						}
						Model model4 = new Model("hei_p_m_bag_var22_arm_s");
						model4.Request(250);
						if (model4.IsValid && model4.IsInCdImage)
						{
							while (!model4.IsLoaded)
							{
								Script.Wait(50);
							}
							Bag = GlobalHelper.CreateProp(model4, Game.Player.Character.Position, false, false);
							Bag.IsCollisionEnabled = false;
							plantBombStage = 10;
						}
						break;
					}
					case 10:
						LoadDict("anim@scripted@heist@ig11_bomb_plant@male@");
						LoadDict("anim@scripted@player@mission@tun_bomb_plant@heeled@");
						BombSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z + 0.8f, 0f, 0f, rotation.Z, 2);
						Notification.Show("Position " + offsetPosition.ToString() + "/" + rotation.Z);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, BombSyncScene, "anim@scripted@player@mission@tun_bomb_plant@heeled@", "enter", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bomb, BombSyncScene, "enter_bomb", "anim@scripted@heist@ig11_bomb_plant@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bag, BombSyncScene, "enter_bag", "anim@scripted@heist@ig11_bomb_plant@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						plantBombStage = 20;
						break;
					case 20:
						if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, BombSyncScene) > 0.99f)
						{
							Bag.Delete();
							Game.Player.Character.Task.ClearAll();
							BombExplodeTimer = Game.GameTime + 5000;
							plantBombStage = -1;
						}
						break;
					case -1:
						if (Game.GameTime >= BombExplodeTimer)
						{
							World.AddExplosion(Bomb.Position, ExplosionType.PipeBomb, 1f, 1f, Game.Player.Character);
							if (Bomb != null)
							{
								Bomb.Delete();
							}
							plantBombStage = -2;
						}
						break;
					case -2:
						SecondaryVaultOpen = true;
						plantBombStage = 0;
						break;
					}
				}
				if (!GetDistance_VDIST2(Game.Player.Character.Position, BombPlacementLoc2, "<", 20f) || TerciaryVaultOpen)
				{
					break;
				}
				switch (plantBombStage)
				{
				case 0:
				{
					if (!GetDistance_VDIST2(Game.Player.Character.Position, BombPlacementLoc2, "<", 2f) || TerciaryVaultOpen || C4ChargesCollected < 1)
					{
						break;
					}
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to plant a ~r~bomb.", 1);
					if (!Game.IsControlJustPressed(Control.Context))
					{
						break;
					}
					Game.Player.Character.Position = BombPlacementLoc2;
					C4ChargesCollected--;
					LoadDict("anim@scripted@heist@ig11_bomb_plant@male@");
					LoadDict("anim@scripted@player@mission@tun_bomb_plant@heeled@");
					Model model5 = new Model("ch_prop_ch_ld_bomb_01a");
					model5.Request(250);
					if (model5.IsValid && model5.IsInCdImage)
					{
						while (!model5.IsLoaded)
						{
							Script.Wait(50);
						}
						Bomb = GlobalHelper.CreateProp(model5, Game.Player.Character.Position, false, false);
						Bomb.IsCollisionEnabled = false;
						plantBombStage = 10;
					}
					Model model6 = new Model("hei_p_m_bag_var22_arm_s");
					model6.Request(250);
					if (model6.IsValid && model6.IsInCdImage)
					{
						while (!model6.IsLoaded)
						{
							Script.Wait(50);
						}
						Bag = GlobalHelper.CreateProp(model6, Game.Player.Character.Position, false, false);
						Bag.IsCollisionEnabled = false;
						plantBombStage = 10;
					}
					break;
				}
				case 10:
					LoadDict("anim@scripted@heist@ig11_bomb_plant@male@");
					LoadDict("anim@scripted@player@mission@tun_bomb_plant@heeled@");
					BombSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z + 0.8f, 0f, 0f, rotation.Z, 2);
					Notification.Show("Position " + offsetPosition.ToString() + "/" + rotation.Z);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, BombSyncScene, "anim@scripted@player@mission@tun_bomb_plant@heeled@", "enter", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bomb, BombSyncScene, "enter_bomb", "anim@scripted@heist@ig11_bomb_plant@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bag, BombSyncScene, "enter_bag", "anim@scripted@heist@ig11_bomb_plant@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					plantBombStage = 20;
					break;
				case 20:
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, BombSyncScene) > 0.99f)
					{
						Bag.Delete();
						Game.Player.Character.Task.ClearAll();
						BombExplodeTimer = Game.GameTime + 5000;
						plantBombStage = -1;
					}
					break;
				case -1:
					if (Game.GameTime >= BombExplodeTimer)
					{
						World.AddExplosion(Bomb.Position, ExplosionType.PipeBomb, 1f, 1f, Game.Player.Character);
						if (Bomb != null)
						{
							Bomb.Delete();
						}
						plantBombStage = -2;
					}
					break;
				case -2:
					TerciaryVaultOpen = true;
					plantBombStage = 0;
					break;
				}
				break;
			case HeistStage.VIPEnterAfterPurchase:
				if (GetDistance_VDIST2(Game.Player.Character.Position, VIPInteLoc, ">", 400f))
				{
					CurrentHeistStage = HeistStage.Scout;
				}
				if (SuperVolito != null && Game.Player.Character.CurrentVehicle == SuperVolito)
				{
					IntelMarkers[0].Alpha = 0;
					IntelMarkersRadiusArea[0].Alpha = 0;
					UsingSuperVolito = true;
				}
				if (Pilot != null)
				{
					if (isEnteringAsVIP)
					{
						if (Pilot.VehicleTryingToEnter != SuperVolito)
						{
							Pilot.Task.EnterVehicle(SuperVolito, VehicleSeat.Driver);
						}
						if (SuperVolito != null)
						{
							if (Game.IsControlPressed(Control.VehicleExit) && Game.Player.Character.CurrentVehicle != SuperVolito)
							{
								Notification.Show("Enter SuperVolito LeftRear Seat");
								Game.Player.Character.Task.EnterVehicle(SuperVolito, VehicleSeat.LeftRear, -1, 1f, EnterVehicleFlags.DontJackAnyone);
							}
							if (SuperVolito.Driver == Pilot && Game.Player.Character.CurrentVehicle == SuperVolito && !StartedSuperVolitoRoute)
							{
								VehicleRecording.CurrentRoute = VehicleRecording.SuperVolitoReplay;
								VehicleRecording.RouteVeh = SuperVolito;
								VehicleRecording.RouteStage = 0;
								VehicleRecording.PlayingRecordedRoute = true;
								StartedSuperVolitoRoute = true;
							}
						}
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, Pilot.Position, "<", 2f) && !isEnteringAsVIP)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to pay Pilot $60,000 to take you to the Yacht as a VIP Guest");
						if (Game.IsControlJustPressed(Control.Context) && Game.Player.Money >= 60000)
						{
							isEnteringAsVIP = true;
							Game.Player.Money -= 60000;
							Pilot.Task.EnterVehicle(SuperVolito, VehicleSeat.Driver);
						}
					}
				}
				if (MansionGuards.Count > 1 && CurrentActiveGroup == ActiveGroup.YachtHeist_VIPMansion)
				{
					if (!UsingSuperVolito && !isEnteringAsVIP)
					{
						CurrentHeistStage = HeistStage.VIPEnterPrePurchase;
					}
					else if (Game.Player.Character.CurrentVehicle == SuperVolito)
					{
						CurrentHeistStage = HeistStage.InSuperVolito;
					}
					else
					{
						CurrentHeistStage = HeistStage.VIPEnterAfterPurchase;
					}
					SmartGuards.GetCurrentGroupSettings().ShowCone = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipRadius = 100f;
					SmartGuards.GetCurrentGroupSettings().Radius = 80f;
					SmartGuards.GetCurrentGroupSettings().SeeingAngle = 35f;
					SmartGuards.GetCurrentGroupSettings().AllowInvestigate = true;
					SmartGuards.GetCurrentGroupSettings().InvestigateDist = 40f;
					SmartGuards.GetCurrentGroupSettings().HitRadius = 30f;
					SmartGuards.GetCurrentGroupSettings().ArrestInsteadOfShoot = false;
					SmartGuards.GetCurrentGroupSettings().AimAtInsteadOfShoot = true;
					SmartGuards.GetCurrentGroupSettings().GuardSubtitleIfAiming = "~r~Guard~w~ : Sir Please lower the firearm if you wish to enter the property";
					SmartGuards.GetCurrentGroupSettings().UseShowBlipWhenSameHeightWithPlayer = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightAdd = 10f;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightMinus = 10f;
					if (Game.Player.Character.Weapons.Current.Hash == WeaponHash.Unarmed)
					{
						SmartGuards.GetCurrentGroupSettings().IngorePlayer = true;
					}
					else
					{
						SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
					}
				}
				break;
			case HeistStage.VIPEnterPrePurchase:
				if (GetDistance_VDIST2(Game.Player.Character.Position, VIPInteLoc, ">", 400f))
				{
					CurrentHeistStage = HeistStage.Scout;
				}
				if (SuperVolito != null && Game.Player.Character.CurrentVehicle == SuperVolito)
				{
					IntelMarkers[0].Alpha = 0;
					IntelMarkersRadiusArea[0].Alpha = 0;
					UsingSuperVolito = true;
					CurrentHeistStage = HeistStage.InSuperVolito;
				}
				if (Pilot != null)
				{
					if (isEnteringAsVIP)
					{
						if (Pilot.VehicleTryingToEnter != SuperVolito)
						{
							Pilot.Task.EnterVehicle(SuperVolito, VehicleSeat.Driver);
						}
						if (SuperVolito != null)
						{
							if (Game.IsControlPressed(Control.VehicleExit) && Game.Player.Character.CurrentVehicle != SuperVolito)
							{
								Notification.Show("Enter SuperVolito LeftRear Seat");
								Game.Player.Character.Task.EnterVehicle(SuperVolito, VehicleSeat.LeftRear, -1, 1f, EnterVehicleFlags.DontJackAnyone);
							}
							if (SuperVolito.Driver == Pilot && Game.Player.Character.CurrentVehicle == SuperVolito && !StartedSuperVolitoRoute)
							{
								VehicleRecording.CurrentRoute = VehicleRecording.SuperVolitoReplay;
								VehicleRecording.RouteVeh = SuperVolito;
								VehicleRecording.RouteStage = 0;
								VehicleRecording.PlayingRecordedRoute = true;
								StartedSuperVolitoRoute = true;
							}
						}
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, Pilot.Position, "<", 2f) && !isEnteringAsVIP)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to pay Pilot $60,000 to take you to the Yacht as a VIP Guest");
						if (Game.IsControlJustPressed(Control.Context) && Game.Player.Money >= 60000)
						{
							isEnteringAsVIP = true;
							Game.Player.Money -= 60000;
							Pilot.Task.EnterVehicle(SuperVolito, VehicleSeat.Driver);
						}
					}
				}
				if (MansionGuards.Count > 1 && CurrentActiveGroup == ActiveGroup.YachtHeist_VIPMansion)
				{
					if (!UsingSuperVolito && !isEnteringAsVIP)
					{
						CurrentHeistStage = HeistStage.VIPEnterPrePurchase;
					}
					else if (Game.Player.Character.CurrentVehicle == SuperVolito)
					{
						CurrentHeistStage = HeistStage.InSuperVolito;
					}
					else
					{
						CurrentHeistStage = HeistStage.VIPEnterAfterPurchase;
					}
					SmartGuards.GetCurrentGroupSettings().ShowCone = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipRadius = 100f;
					SmartGuards.GetCurrentGroupSettings().Radius = 80f;
					SmartGuards.GetCurrentGroupSettings().SeeingAngle = 35f;
					SmartGuards.GetCurrentGroupSettings().AllowInvestigate = true;
					SmartGuards.GetCurrentGroupSettings().InvestigateDist = 40f;
					SmartGuards.GetCurrentGroupSettings().HitRadius = 30f;
					SmartGuards.GetCurrentGroupSettings().ArrestInsteadOfShoot = false;
					SmartGuards.GetCurrentGroupSettings().AimAtInsteadOfShoot = true;
					SmartGuards.GetCurrentGroupSettings().GuardSubtitleIfAiming = "~r~Guard~w~ : Sir Please lower the firearm if you wish to enter the property";
					SmartGuards.GetCurrentGroupSettings().UseShowBlipWhenSameHeightWithPlayer = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightAdd = 10f;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightMinus = 10f;
					if (Game.Player.Character.Weapons.Current.Hash == WeaponHash.Unarmed)
					{
						SmartGuards.GetCurrentGroupSettings().IngorePlayer = true;
					}
					else
					{
						SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
					}
				}
				break;
			}
			if (CurrentHeistStage == HeistStage.Scout)
			{
				Screen.ShowSubtitle("Approach the ~y~Cartel Yacht~w~ or Scout for more ~g~Assets/Intel~w~");
			}
			if (CurrentHeistStage == HeistStage.NoWaytoOpenVaults)
			{
				Screen.ShowSubtitle("Keep scouting for more ~g~Assets/Intel~w~ to find a way to open the ~y~Cartel Yacht's Vaults~w~");
			}
			if (CurrentHeistStage == HeistStage.C4)
			{
				Screen.ShowSubtitle("Search the ~d~Bunker~w~ for a ~g~C4~w~ ");
			}
			if (CurrentHeistStage == HeistStage.Drill)
			{
				Screen.ShowSubtitle("Search the ~d~Area~w~ for a ~g~Drill~w~ ");
			}
			if (CurrentHeistStage == HeistStage.DisposableKeycard)
			{
				Screen.ShowSubtitle("Search the ~d~IAA Facility~w~ for a ~y~Disposable Network Keycard~w~ ");
			}
			if (CurrentHeistStage == HeistStage.SearchYachtForLoot)
			{
				if (ManageRobberyAnims.NearestCrateId != -1)
				{
					Screen.ShowSubtitle("Steal at least ~y~$" + PercentNeedTake.ToString("N") + "~w~ of ~g~Valuables~w~");
				}
				else
				{
					Screen.ShowSubtitle("Search the ~y~Cartel Yacht~w~ for ~g~Valuables~w~");
				}
			}
			if (CurrentHeistStage == HeistStage.VIPEnterPrePurchase)
			{
				Screen.ShowSubtitle("Talk to the ~d~Pilot~w~ about getting a ride in the ~b~SuperVolito~w~ as a VIP to the ~y~Cartel Yacht~w~");
			}
			if (CurrentHeistStage == HeistStage.VIPEnterAfterPurchase)
			{
				Screen.ShowSubtitle("Enter the ~b~SuperVolito~w~ to get a ride as a VIP to the ~y~Cartel Yacht~w~");
			}
			if (CurrentHeistStage == HeistStage.InSuperVolito)
			{
				Screen.ShowSubtitle("Wait for the ~b~SuperVolito~w~ to land at the ~y~Cartel Yacht~w~");
			}
			if (CurrentHeistStage == HeistStage.longfin)
			{
				Screen.ShowSubtitle("Steal the ~b~Longfin~w~ to board the ~y~Cartel Yacht~w~ as a drug courier");
			}
			if (CurrentHeistStage == HeistStage.InLongfin)
			{
				Screen.ShowSubtitle("Drive the ~b~Longfin~w~ to the ~y~Cartel Yacht~w~");
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
			if (GetDistance_VDIST2(Game.Player.Character.Position, YachtLoc, "<", 150f))
			{
				if (CurrentActiveGroup != ActiveGroup.YachtHeist_YachtGuards)
				{
					SmartGuards.GuardGroup[5].GroupList.Clear();
					SmartGuards.GuardGroup[5].GroupList.AddRange(YachtGuards);
					SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Cartel;
					CurrentActiveGroup = ActiveGroup.YachtHeist_YachtGuards;
				}
				if (!GotC4 && !GotKeycard && !GotDrill)
				{
					CurrentHeistStage = HeistStage.NoWaytoOpenVaults;
				}
				if (CurrentHeistStage == HeistStage.NoWaytoOpenVaults)
				{
					CurrentHeistStage = HeistStage.Scout;
				}
			}
			else
			{
				if (CurrentPay >= PercentNeedTake && !heistCompleteCalled)
				{
					heistCompleteCalled = true;
					CompleteHeist();
				}
				if (CurrentHeistStage == HeistStage.NoWaytoOpenVaults)
				{
					CurrentHeistStage = HeistStage.Scout;
				}
			}
			if (!InBunker_C4)
			{
				if (GetDistance_VDIST2(Game.Player.Character.Position, BunkerC4Enter, "<", 100f))
				{
					World.DrawMarker(MarkerType.Cylinder, BunkerC4Enter, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
					if (GetDistance_VDIST2(Game.Player.Character.Position, BunkerC4Enter, "<", 2f))
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Bunker");
						if (Game.IsControlJustPressed(Control.Context))
						{
							Function.Call(Hash.BEGIN_TEXT_COMMAND_BUSYSPINNER_ON, "STRING");
							Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "Loading Yacht Heist Assets");
							Function.Call(Hash.END_TEXT_COMMAND_BUSYSPINNER_ON, 5);
							C4Objects.CreateEntites();
							C4PickupObjects.CreateEntites();
							ConvertNavToSmartAIV2(C4BunkerNavs, C4Guards);
							Function.Call(Hash.BUSYSPINNER_OFF);
							Screen.FadeOut(1500);
							Script.Wait(1500);
							InBunker_C4 = true;
							Game.Player.Character.Position = BunkerC4Exit;
							Script.Wait(1500);
							Screen.FadeIn(1500);
							CurrentHeistStage = HeistStage.C4;
						}
					}
				}
			}
			else if (InBunker_C4)
			{
				Vector3 position3 = Game.Player.Character.Position;
				Prop prop3 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position3.X, position3.Y, position3.Z, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "stt_prop_c4_stack"), 0, 0, 0);
				if (prop3 != null)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, prop3.Position, "<", 30f))
					{
						World.DrawMarker(MarkerType.Chevron1, prop3.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), Vector3.Zero, new Vector3(0f, 180f, 0f), new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 255, 0), true, true);
					}
					if (prop3.Exists())
					{
						if (prop3.AttachedBlip == null)
						{
							prop3.AddBlip();
							prop3.AttachedBlip.Sprite = BlipSprite.Bomb;
							prop3.AttachedBlip.Name = "C4 Crate";
							prop3.AttachedBlip.Color = BlipColor.Green;
							prop3.AttachedBlip.IsShortRange = true;
							prop3.AttachedBlip.IsFlashing = true;
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, prop3.Position, "<", 2.5f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Pickup C4");
							if (Game.IsControlJustPressed(Control.Context))
							{
								IntelMarkers[2].Alpha = 0;
								IntelMarkersRadiusArea[2].Alpha = 0;
								C4ChargesCollected += 3;
								if (prop3.AttachedBlip != null)
								{
									prop3.AttachedBlip.Alpha = 0;
								}
								EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(prop3, 0));
								TextNotification("CHAR_HUMANDEFAULT", "~y~The Cartel Yacht Heist ~w~", "Intel Found", "You have found the C4 to blow open Vaults on the Cartel Yacht");
								GotC4 = true;
							}
						}
					}
				}
				if (CurrentActiveGroup != ActiveGroup.YachtHeist_Bunker)
				{
					SmartGuards.GuardGroup[5].GroupList.Clear();
					SmartGuards.GuardGroup[5].GroupList.AddRange(C4Guards);
					SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Cartel;
					CurrentActiveGroup = ActiveGroup.YachtHeist_Bunker;
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, BunkerC4Exit, "<", 100f))
				{
					World.DrawMarker(MarkerType.Cylinder, BunkerC4Exit, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
					if (GetDistance_VDIST2(Game.Player.Character.Position, BunkerC4Exit, "<", 2f))
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit Bunker");
						if (Game.IsControlJustPressed(Control.Context))
						{
							Screen.FadeOut(1500);
							Script.Wait(1500);
							InBunker_C4 = false;
							Game.Player.Character.Position = BunkerC4Enter;
							C4Objects.DeleteEntites();
							C4PickupObjects.DeleteEntites();
							CurrentActiveGroup = ActiveGroup.None;
							CurrentHeistStage = HeistStage.Scout;
							Script.Wait(1500);
							Screen.FadeIn(1500);
						}
					}
				}
			}
			if (!InIAABase)
			{
				if (GetDistance_VDIST2(Game.Player.Character.Position, IntelKeycardEnter, "<", 100f))
				{
					World.DrawMarker(MarkerType.Cylinder, IntelKeycardEnter, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
					if (GetDistance_VDIST2(Game.Player.Character.Position, IntelKeycardEnter, "<", 2f))
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter IAA Base");
						if (Game.IsControlJustPressed(Control.Context))
						{
							Function.Call(Hash.BEGIN_TEXT_COMMAND_BUSYSPINNER_ON, "STRING");
							Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "Loading Yacht Heist Assets");
							Function.Call(Hash.END_TEXT_COMMAND_BUSYSPINNER_ON, 5);
							ConvertNavToSmartAIV2(IAAFacNavs, IAAGuards);
							int index = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, KeycardPickupsObjects.Entities.Count);
							KeycardPickupsObjects.Entities[index].CreateObjatCoords();
							if (KeycardPickupsObjects.Entities[index].Entity != null)
							{
								Keycard = (Prop)KeycardPickupsObjects.Entities[index].Entity;
							}
							Function.Call(Hash.BUSYSPINNER_OFF);
							Screen.FadeOut(1500);
							Script.Wait(1500);
							InIAABase = true;
							Game.Player.Character.Position = IntelKeycardExit;
							Script.Wait(1500);
							Screen.FadeIn(1500);
							CurrentHeistStage = HeistStage.DisposableKeycard;
						}
					}
				}
			}
			else if (InIAABase)
			{
				if (Keycard != null && !GotKeycard)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, Keycard.Position, "<", 30f))
					{
						World.DrawMarker(MarkerType.Chevron1, Keycard.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), Vector3.Zero, new Vector3(0f, 180f, 0f), new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 255, 0), true, true);
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, Keycard.Position, "<", 10f) && Keycard.AttachedBlip == null)
					{
						Keycard.AddBlip();
						Keycard.AttachedBlip.Sprite = BlipSprite.Keycard;
						Keycard.AttachedBlip.Name = "Disposable Network Keycard";
						Keycard.AttachedBlip.Color = BlipColor.Green;
						Keycard.AttachedBlip.IsShortRange = true;
						Keycard.AttachedBlip.IsFlashing = true;
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, Keycard.Position, "<", 2f))
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Pickup Disposable Network Keycard");
						if (Game.IsControlJustPressed(Control.Context))
						{
							IntelMarkersRadiusArea[3].Alpha = 0;
							IntelMarkers[3].Alpha = 0;
							C4ChargesCollected = 3;
							if (Keycard.AttachedBlip != null)
							{
								Keycard.AttachedBlip.Alpha = 0;
							}
							EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(Keycard, 0));
							TextNotification("CHAR_HUMANDEFAULT", "~y~The Cartel Yacht Heist ~w~", "Intel Found", "You have found the Keycard to open the Vaults on the Cartel Yacht");
							GotKeycard = true;
						}
					}
				}
				if (CurrentActiveGroup != ActiveGroup.YachtHeist_IAAFacility)
				{
					SmartGuards.GuardGroup[28].GroupList.Clear();
					SmartGuards.GuardGroup[28].GroupList.AddRange(IAAGuards);
					SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.IAA;
					CurrentActiveGroup = ActiveGroup.YachtHeist_IAAFacility;
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, IntelKeycardExit, "<", 100f))
				{
					World.DrawMarker(MarkerType.Cylinder, IntelKeycardExit, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
					if (GetDistance_VDIST2(Game.Player.Character.Position, IntelKeycardExit, "<", 2f))
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit IAA Base");
						if (Game.IsControlJustPressed(Control.Context))
						{
							Screen.FadeOut(1500);
							Script.Wait(1500);
							InIAABase = false;
							Game.Player.Character.Position = IntelKeycardEnter;
							CurrentActiveGroup = ActiveGroup.None;
							CurrentHeistStage = HeistStage.Scout;
							KeycardPickupsObjects.DeleteEntites();
							Script.Wait(1500);
							Screen.FadeIn(1500);
						}
					}
				}
			}
			if (Longfin != null && Game.Player.Character.CurrentVehicle == Longfin)
			{
				IntelMarkers[1].Alpha = 0;
				IntelMarkersRadiusArea[1].Alpha = 0;
				if (GetDistance_VDIST2(YachtLoc, LongfinLoc, "<", 200f))
				{
					isEnteringAsVIP = true;
				}
				UsingLongfin = true;
			}
			if (GetDistance_VDIST2(Game.Player.Character.Position, LongfinLoc, "<", 500f))
			{
				if (!UsingLongfin)
				{
					CurrentHeistStage = HeistStage.longfin;
				}
				else
				{
					CurrentHeistStage = HeistStage.InLongfin;
				}
				if (!CreatedLongfinObjects)
				{
					LongfinObjects.CreateEntites();
					LongfinGuards.Clear();
					foreach (ObjectPlacementMap entity in LongfinObjects.Entities)
					{
						if (entity.Entity != null && entity.Entity.Model.IsPed)
						{
							LongfinGuards.Add(new SmartGuards.SmartAiV2((Ped)entity.Entity, SmartGuards.EnemyGroup.Cartel));
						}
					}
					Longfin = (Vehicle)LongfinObjects.Entities[0].Entity;
					CreatedLongfinObjects = true;
				}
				if (CurrentActiveGroup != ActiveGroup.YachtHeist_LongfinGuards)
				{
					SmartGuards.GuardGroup[5].GroupList.Clear();
					SmartGuards.GuardGroup[5].GroupList.AddRange(LongfinGuards);
					SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Cartel;
					CurrentActiveGroup = ActiveGroup.YachtHeist_LongfinGuards;
				}
			}
			else
			{
				foreach (ObjectPlacementMap entity2 in LongfinObjects.Entities)
				{
					if (!(entity2.Entity != null))
					{
						continue;
					}
					if (entity2.Entity == Longfin)
					{
						if (!UsingLongfin && entity2.Entity != null)
						{
							entity2.Entity.Delete();
						}
					}
					else if (entity2.Entity != null)
					{
						entity2.Entity.Delete();
					}
				}
				if (CurrentActiveGroup == ActiveGroup.YachtHeist_LongfinGuards)
				{
					CurrentHeistStage = HeistStage.Scout;
					SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.None;
					CurrentActiveGroup = ActiveGroup.None;
				}
				CreatedLongfinObjects = false;
			}
			if (GetDistance_VDIST2(Game.Player.Character.Position, VIPInteLoc, "<", 400f))
			{
				if (!CreatedVIPMansionObjects)
				{
					MansionObjects.CreateEntites();
					Pilot = (Ped)MansionObjects.Entities[0].Entity;
					SuperVolito = (Vehicle)MansionObjects.Entities[1].Entity;
					MansionGuardsObjects.CreateEntites();
					MansionGuards.Clear();
					foreach (ObjectPlacementMap entity3 in MansionGuardsObjects.Entities)
					{
						if (entity3.Entity != null && entity3.Entity.Model.IsPed)
						{
							MansionGuards.Add(new SmartGuards.SmartAiV2((Ped)entity3.Entity, SmartGuards.EnemyGroup.Cartel));
						}
					}
					CreatedVIPMansionObjects = true;
				}
				if (CurrentActiveGroup != ActiveGroup.YachtHeist_VIPMansion)
				{
					SmartGuards.GuardGroup[24].GroupList.Clear();
					SmartGuards.GuardGroup[24].GroupList.AddRange(MansionGuards);
					SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.High_End_Security;
					CurrentActiveGroup = ActiveGroup.YachtHeist_VIPMansion;
				}
			}
			else
			{
				foreach (ObjectPlacementMap entity4 in MansionObjects.Entities)
				{
					if (!(entity4.Entity != null))
					{
						continue;
					}
					if (entity4.Entity == SuperVolito || entity4.Entity == Pilot)
					{
						if (!UsingSuperVolito && entity4.Entity != null)
						{
							entity4.Entity.Delete();
						}
					}
					else if (entity4.Entity != null)
					{
						entity4.Entity.Delete();
					}
				}
				if (CurrentActiveGroup == ActiveGroup.YachtHeist_VIPMansion)
				{
					CurrentActiveGroup = ActiveGroup.None;
					SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.None;
				}
				CreatedVIPMansionObjects = false;
			}
			if (GetDistance_VDIST2(Game.Player.Character.Position, YachtLoc, "<", 300f))
			{
				Vector3 position4 = Game.Player.Character.Position;
				if (position4.Z >= 0f)
				{
					Function.Call(Hash.SET_RADAR_AS_INTERIOR_THIS_FRAME, Function.Call<int>(Hash.GET_HASH_KEY, "V_FakeHeistYacht"), ShipUIPos.X, ShipUIPos.Y, ShipUIPos.Z, 0);
					Function.Call(Hash.SET_RADAR_ZOOM_PRECISE, 70);
				}
				if (position4.Z >= 4f)
				{
					Function.Call(Hash.SET_RADAR_AS_INTERIOR_THIS_FRAME, Function.Call<int>(Hash.GET_HASH_KEY, "V_FakeHeistYacht"), ShipUIPos.X, ShipUIPos.Y, ShipUIPos.Z, 1);
					Function.Call(Hash.SET_RADAR_ZOOM_PRECISE, 70);
				}
				if (position4.Z >= 8f)
				{
					Function.Call(Hash.SET_RADAR_AS_INTERIOR_THIS_FRAME, Function.Call<int>(Hash.GET_HASH_KEY, "V_FakeHeistYacht"), ShipUIPos.X, ShipUIPos.Y, ShipUIPos.Z, 2);
					Function.Call(Hash.SET_RADAR_ZOOM_PRECISE, 70);
				}
				if (position4.Z >= 12f)
				{
					Function.Call(Hash.SET_RADAR_AS_INTERIOR_THIS_FRAME, Function.Call<int>(Hash.GET_HASH_KEY, "V_FakeHeistYacht"), ShipUIPos.X, ShipUIPos.Y, ShipUIPos.Z, 3);
					Function.Call(Hash.SET_RADAR_ZOOM_PRECISE, 70);
				}
			}
			if (!Game.Player.IsAlive)
			{
				EndHeist();
				FailHeist("cause the Player Died");
			}
		}
	}
}
