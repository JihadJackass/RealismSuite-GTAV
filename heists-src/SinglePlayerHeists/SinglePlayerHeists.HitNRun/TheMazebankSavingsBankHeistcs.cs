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
	internal class TheMazebankSavingsBankHeistcs : Script
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
			PlaceThermiteOnVault,
			UnlockMagneticLocks,
			LootTheVault,
			LeaveTheBank,
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

		public List<Prop> PaintingHolder = new List<Prop>();

		public static int PercentNeedTake = (int)((float)(HeistMaxPay + HeistMinPay) / 1.5f / 100f * 30f);

		public static int HeistMinPay = 850000;

		public static int HeistMaxPay = 350000;

		public static int CurrentPay = 0;

		public static int LocalTake = 0;

		public static MissionOjects MazebankInterior = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mk_money"), 84057, 1113676861u, new Vector3(-1310.78625f, -835.805969f, 3.69682312f), new Vector3(-6.532076E-07f, -5.371989E-07f, 31.2169914f), 180527, 0, new Vector3(-11.9598789f, 10.2940941f, -2.85776353f), new Vector3(2.38417527E-07f, -9.556378E-07f, -50.85089f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mk_money"), 523377, 1113676861u, new Vector3(-1299.39966f, -824.410461f, 6.64056f), new Vector3(1.06673065E-06f, -1.4885394E-06f, -141.1681f), 180527, 0, new Vector3(0.8979739f, 0.5889802f, 0.0859737545f), new Vector3(2.38417527E-07f, -9.556378E-07f, -223.236f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_v_ilev_fh_heistdoor1"), 271662, 509504073u, new Vector3(-1302.102f, -814.8533f, 7.46363544f), new Vector3(0.0009794367f, 0.0353660956f, 125.251236f), 627800, 0, new Vector3(8.2f, -6.299999f, 1.1f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Security Guard"), 87826, 3613962792u, new Vector3(-1302.03f, -813.87f, 7.361496f), new Vector3(-0.485290617f, -4.695925E-05f, 6.34394073f), 2725352035u, 199, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "A_M_M_Business_01"), 123004, 2120901815u, new Vector3(-1305.4707f, -817.0613f, 7.361503f), new Vector3(0f, -0f, -146.95842f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_c_panel_on_01a"), 758049, 2085650565u, new Vector3(-1306.005f, -818.107239f, 7.212307f), new Vector3(0f, -0f, 124.676193f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_fingerprint_scanner_01e"), 309001, 996668613u, new Vector3(-1304.01282f, -823.13446f, 7.92927027f), new Vector3(0f, -0f, 124.268936f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 90200, 2068850345u, new Vector3(-1297.31128f, -820.6428f, 5.725086f), new Vector3(90f, 34.1583443f, 0f), 180527, 0, new Vector3(4.91774559f, -0.959463358f, -0.8295002f), new Vector3(90f, -120.831f, 72.92147f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 80728, 2068850345u, new Vector3(-1298.96631f, -821.7658f, 5.725086f), new Vector3(90f, 34.1583443f, 0f), 90200, 0, new Vector3(-2.00000024f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 144225, 2068850345u, new Vector3(-1300.62122f, -822.888733f, 5.725086f), new Vector3(90f, 34.1583443f, 0f), 90200, 0, new Vector3(-3.99999857f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 677973, 2068850345u, new Vector3(-1302.27625f, -824.0117f, 5.725086f), new Vector3(90f, 34.1583443f, 0f), 90200, 0, new Vector3(-5.99999666f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 196618, 2068850345u, new Vector3(-1300.37f, -819.6971f, 5.725086f), new Vector3(90f, 34.1583443f, 0f), 269409, 0, new Vector3(-2.00000024f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 503637, 2068850345u, new Vector3(-1302.0249f, -820.82f, 5.725086f), new Vector3(90f, 34.1583443f, 0f), 269409, 0, new Vector3(-3.99999857f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 329043, 2068850345u, new Vector3(-1303.67993f, -821.943f, 5.725086f), new Vector3(90f, 34.1583443f, 0f), 269409, 0, new Vector3(-5.99999666f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 269409, 2068850345u, new Vector3(-1298.715f, -818.5741f, 5.725086f), new Vector3(90f, 34.1583443f, 0f), 90200, 0, new Vector3(0f, -1.49011612E-08f, MathF.PI * -113f / 142f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_prop_hei_bankdoor_new"), 705856, 1240350830u, new Vector3(-1300.20593f, -825.4938f, 7.448241f), new Vector3(-8.6711384E-07f, 0.3308818f, 34.1583481f), 90200, 0, new Vector3(-5.118977f, 1.72315478f, 2.38883567f), new Vector3(-90f, -120.168961f, -120.499832f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_prop_hei_bankdoor_new"), 269828, 1240350830u, new Vector3(-1298.05444f, -824.034058f, 7.43322659f), new Vector3(9.871758E-07f, -0.330881774f, -145.84169f), 180527, 0, new Vector3(1.45639765f, -0.691358864f, 0.8786402f), new Vector3(1.20061912E-07f, -0.330881327f, 132.090424f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 385839, 2068850345u, new Vector3(-1301.71753f, -817.71106f, 5.725086f), new Vector3(90f, 34.1583443f, 0f), 180527, 0, new Vector3(7.213385f, 3.80917454f, -0.8295002f), new Vector3(90f, -120.831009f, 72.92147f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 738896, 2068850345u, new Vector3(-1303.37244f, -818.834045f, 5.72508574f), new Vector3(90f, 34.1583443f, 0f), 29008, 0, new Vector3(-3.99999857f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 76122, 2068850345u, new Vector3(-1305.02747f, -819.957031f, 5.72508574f), new Vector3(90f, 34.1583443f, 0f), 29008, 0, new Vector3(-5.99999666f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 50024, 2068850345u, new Vector3(-1301.46619f, -814.5194f, 5.72508574f), new Vector3(90f, 34.1583443f, 0f), 29008, 0, new Vector3(0f, -1.49011612E-08f, MathF.PI * -113f / 142f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 29008, 2068850345u, new Vector3(-1300.0625f, -816.588135f, 5.72508574f), new Vector3(90f, 34.1583443f, 0f), 90200, 0, new Vector3(0f, -2.23517418E-07f, -4.89999771f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 772177, 2068850345u, new Vector3(-1302.27625f, -824.0117f, 5.725086f), new Vector3(90f, 34.1583443f, 0f), 90200, 0, new Vector3(-5.99999666f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 749168, 2068850345u, new Vector3(-1307.83215f, -822.9477f, 5.725086f), new Vector3(90f, 34.1583633f, 0f), 104201, 0, new Vector3(-2.00000024f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 59719, 2068850345u, new Vector3(-1309.487f, -824.0706f, 5.725086f), new Vector3(90f, 34.1583633f, 0f), 104201, 0, new Vector3(-3.99999857f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 128113, 2068850345u, new Vector3(-1311.14209f, -825.1936f, 5.725086f), new Vector3(90f, 34.1583633f, 0f), 104201, 0, new Vector3(-5.99999666f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 736785, 2068850345u, new Vector3(-1307.58081f, -819.756f, 5.725086f), new Vector3(90f, 34.1583633f, 0f), 104201, 0, new Vector3(0f, -1.49011612E-08f, MathF.PI * -113f / 142f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 611961, 2068850345u, new Vector3(-1311.14209f, -825.1936f, 5.725086f), new Vector3(90f, 34.1583633f, 0f), 104201, 0, new Vector3(-5.99999666f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 104201, 2068850345u, new Vector3(-1306.17712f, -821.8247f, 5.725086f), new Vector3(90f, 34.1583633f, 0f), 90200, 0, new Vector3(-8.000001f, 0f, -4.00000048f), new Vector3(0f, 1.52587891E-05f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 181881, 3294791577u, new Vector3(-1305.05811f, -821.8429f, 6.372797f), new Vector3(-0.3307838f, -0.008136813f, -57.25041f), 180527, 0, new Vector3(2.66010737f, 6.547667f, -0.1817894f), new Vector3(-0.330783427f, -0.008135946f, 220.6817f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_service_pillar_02a"), 683611, 2002969729u, new Vector3(-1304.73608f, -822.9055f, 6.238344f), new Vector3(0.005929311f, 0.330828667f, 35.18524f), 269828, 0, new Vector3(4.88838959f, -4.685516f, -1.22313309f), new Vector3(0f, 0f, -178.973053f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_lobby_pillar_04a"), 311048, 15989799u, new Vector3(-1304.63977f, -823.328857f, 6.22561359f), new Vector3(0.003575257f, -0.330862463f, -146.460617f), 683611, 0, new Vector3(-0.165129468f, -0.4014745f, -0.0136427432f), new Vector3(0f, 0f, 178.354111f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_lobby_pillar_04a"), 496140, 15989799u, new Vector3(-1307.3009f, -825.0879f, 6.244034f), new Vector3(0.00357523025f, -0.330862463f, -146.460617f), 106848, 0, new Vector3(1.99448133f, 0.2549319f, -0.08010864f), new Vector3(0f, 0f, 0.07543945f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_lobby_pillar_04a"), 732766, 15989799u, new Vector3(-1309.95972f, -826.850342f, 6.262455f), new Vector3(0.00357523025f, -0.330862463f, -146.460617f), 496140, 0, new Vector3(3.18999934f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 384636, 2068850345u, new Vector3(-1312.79712f, -826.3166f, 5.725086f), new Vector3(90f, 34.1583633f, 0f), 128113, 0, new Vector3(-2.00000024f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 501509, 2068850345u, new Vector3(-1314.20081f, -824.247864f, 5.725086f), new Vector3(90f, 34.1583633f, 0f), 128113, 0, new Vector3(-2.00000024f, 0f, MathF.PI * -113f / 142f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 181522, 2068850345u, new Vector3(-1315.54834f, -822.2619f, 5.725086f), new Vector3(90f, 34.1583633f, 0f), 128113, 0, new Vector3(-2.00000024f, 0f, -4.89999771f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_lobby_pillar_04a"), 169071, 15989799u, new Vector3(-1312.50183f, -828.535461f, 6.28006744f), new Vector3(0.3308625f, 0.00357530615f, 123.539345f), 496140, 0, new Vector3(6.23999548f, 0f, 0f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_lobby_pillar_04a"), 10872, 15989799u, new Vector3(-1314.2699f, -825.8682f, 6.27986765f), new Vector3(0.3308625f, 0.00357530615f, 123.539345f), 169071, 0, new Vector3(3.19999743f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_lobby_pillar_04a"), 105324, 15989799u, new Vector3(-1315.94946f, -823.334351f, 6.279678f), new Vector3(-0.00357523025f, 0.3308625f, 33.53938f), 169071, 0, new Vector3(6.24005032f, 0f, 0f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_lobby_pillar_04a"), 562434, 15989799u, new Vector3(-1313.28235f, -821.566345f, 6.26119947f), new Vector3(-0.00357523025f, 0.3308625f, 33.53938f), 169071, 0, new Vector3(6.24005032f, -3.19999743f, 0f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_lobby_pillar_04a"), 422268, 15989799u, new Vector3(-1310.61511f, -819.79834f, 6.24272f), new Vector3(-0.00357523025f, 0.3308625f, 33.53938f), 169071, 0, new Vector3(6.24005032f, -6.40005159f, 0f), new Vector3(0f, 0f, -90.00001f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_lobby_pillar_04a"), 775495, 15989799u, new Vector3(-1306.32019f, -820.598267f, 6.22479f), new Vector3(-0.3308817f, -0.000110232075f, -55.8606033f), 683611, 0, new Vector3(0.0348705724f, 2.79852533f, -0.0136427432f), new Vector3(0f, 0f, -91.04588f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_lobby_pillar_04a"), 498293, 15989799u, new Vector3(-1308.12817f, -818.029846f, 6.22510147f), new Vector3(-0.330862463f, -0.00357541326f, -56.46066f), 169071, 0, new Vector3(6.34005f, -9.450055f, 0f), new Vector3(0f, 0f, 179.999985f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_mp_h_acc_rugwoolm_02"), 630543, 1598361869u, new Vector3(-1300.26f, -822.49f, 6.3219f), new Vector3(0.325207323f, -0.6825795f, 124.041718f), 360293, 0, new Vector3(-2.4666326f, -5.45590067f, -0.08024731f), new Vector3(-0.534147263f, -1.46710587f, 88.76228f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 295685, 938137634u, new Vector3(-1304.73865f, -819.9245f, 6.892581f), new Vector3(-0.00691671344f, -0.6617081f, -144.54538f), 180527, 0, new Vector3(4.604212f, 6.49604845f, 0.337994576f), new Vector3(-0.00691762054f, -0.6617077f, 133.386719f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 20740, 938137634u, new Vector3(-1303.71387f, -819.2097f, 6.878722f), new Vector3(-0.00691609969f, -0.661712766f, -145.670364f), 180527, 0, new Vector3(5.453578f, 5.57960558f, 0.32413578f), new Vector3(-0.00691706035f, -0.661712348f, 132.261749f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 66060, 938137634u, new Vector3(-1302.7229f, -818.533f, 6.8648634f), new Vector3(-0.00691604661f, -0.6617127f, -145.67038f), 180527, 0, new Vector3(6.260572f, 4.69150543f, 0.310277f), new Vector3(-0.006917007f, -0.661712348f, 132.261734f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_new_j_counter_02"), 728443, 938137634u, new Vector3(-1301.732f, -817.8563f, 6.851005f), new Vector3(-0.00691609969f, -0.661712766f, -145.670364f), 20740, 0, new Vector3(-2.39999986f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_service_pillar_02a"), 230486, 2002969729u, new Vector3(-1301.39636f, -816.611755f, 6.20150471f), new Vector3(89.32507f, -117.809731f, 60.32278f), 281100, 0, new Vector3(5.798534f, -1.29663837f, 0.116205841f), new Vector3(88.828f, 169.667908f, 97.56055f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_lab_booth_glass01"), 267270, 3924590261u, new Vector3(-1304.078f, -819.80426f, 7.6358614f), new Vector3(0.0179911442f, 0.713364f, 35.8145447f), 295685, 0, new Vector3(-0.5993253f, 0.2851947f, 0.7502863f), new Vector3(0.006917007f, 0.0517124236f, -179.639954f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_lab_booth_glass01"), 223865, 3924590261u, new Vector3(-1302.132f, -818.399963f, 7.605981f), new Vector3(0.0179911442f, 0.713364f, 35.8145447f), 267270, 0, new Vector3(2.39999986f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_hei_cs_keyboard"), 424714, 1338930512u, new Vector3(-1304.82434f, -819.8253f, 7.070845f), new Vector3(9.224796E-05f, -4.116088E-07f, -145.613052f), 477699, 0, new Vector3(0.028682474f, -0.2194023f, -0.2555071f), new Vector3(0.0007443572f, 0.7299917f, -0.00717926025f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_hei_bank_mon"), 477699, 79209609u, new Vector3(-1304.67407f, -819.988342f, 7.325963f), new Vector3(-0.000743476732f, -0.729992f, -145.605881f), 180527, 0, new Vector3(4.54990959f, 6.423213f, 0.7713766f), new Vector3(-0.0007444372f, -0.729991555f, 132.326233f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_hei_cs_keyboard"), 402808, 1338930512u, new Vector3(-1303.83337f, -819.14856f, 7.05698633f), new Vector3(9.224796E-05f, -4.116088E-07f, -145.613052f), 247679, 0, new Vector3(0.028682474f, -0.2194023f, -0.2555071f), new Vector3(0.0007443572f, 0.7299917f, -0.00717926025f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_hei_bank_mon"), 247679, 79209609u, new Vector3(-1303.68311f, -819.3116f, 7.312104f), new Vector3(-0.000743476732f, -0.729992f, -145.605881f), 180527, 0, new Vector3(5.356903f, 5.535113f, 0.7575178f), new Vector3(-0.0007444372f, -0.729991555f, 132.326233f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_hei_cs_keyboard"), 368404, 1338930512u, new Vector3(-1302.84241f, -818.471863f, 7.04312849f), new Vector3(9.23824045E-05f, -3.09606349E-07f, -145.613068f), 497507, 0, new Vector3(0.028682474f, -0.2194023f, -0.2555071f), new Vector3(0.0007443572f, 0.7299917f, -0.00717926025f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_hei_bank_mon"), 497507, 79209609u, new Vector3(-1302.69214f, -818.6349f, 7.29824638f), new Vector3(-0.000743316661f, -0.729991734f, -145.6059f), 180527, 0, new Vector3(6.163896f, 4.64701271f, 0.74366f), new Vector3(-0.0007443572f, -0.729991436f, 132.326218f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_hei_cs_keyboard"), 119593, 1338930512u, new Vector3(-1301.76892f, -817.7388f, 7.028115f), new Vector3(9.224796E-05f, -4.116088E-07f, -145.613052f), 387409, 0, new Vector3(0.028682474f, -0.2194023f, -0.2555071f), new Vector3(0.0007443572f, 0.7299917f, -0.00717926025f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_hei_bank_mon"), 387409, 79209609u, new Vector3(-1301.61865f, -817.9018f, 7.28323269f), new Vector3(-0.000743476732f, -0.729992f, -145.605881f), 180527, 0, new Vector3(7.038088f, 3.6850152f, 0.7286463f), new Vector3(-0.0007444372f, -0.729991555f, 132.326233f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_lab_booth_glass01"), 717436, 3924590261u, new Vector3(-1302.12524f, -818.3953f, 8.305934f), new Vector3(0.0179911442f, 0.713364f, 35.8145447f), 533250, 0, new Vector3(2.39999986f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_lab_booth_glass01"), 533250, 3924590261u, new Vector3(-1304.07129f, -819.7996f, 8.335814f), new Vector3(0.0179911442f, 0.713364f, 35.8145447f), 295685, 0, new Vector3(-0.5993253f, 0.285194725f, 1.45028639f), new Vector3(0.006917007f, 0.0517124236f, -179.639954f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_metal_detector_01a"), 564837, 1548832211u, new Vector3(-1300.14612f, -816.439f, 6.32624626f), new Vector3(0.006876026f, 0.6617128f, 34.32268f), 180527, 0, new Vector3(8.690124f, 2.428363f, -0.228340149f), new Vector3(0.006876853f, 0.6617126f, -47.74521f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_service_pillar_02a"), 185664, 2002969729u, new Vector3(-1300.96533f, -817.079468f, 6.201432f), new Vector3(-0.005928137f, -0.330830842f, -144.814743f), 180527, 0, new Vector3(7.94275665f, 3.15141535f, -0.353154182f), new Vector3(-0.005929004f, -0.330830365f, 133.117371f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 583179, 2068850345u, new Vector3(-1313.66882f, -821.469849f, 5.725086f), new Vector3(90f, 34.15837f, 0f), 688226, 0, new Vector3(-9.059906E-05f, 5.60052165E-08f, -2.50011373f), new Vector3(0f, 7.62939453E-06f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 76816, 2068850345u, new Vector3(-1312.01379f, -820.3469f, 5.725086f), new Vector3(90f, 34.15837f, 0f), 117001, 0, new Vector3(0f, 0f, -1.99999976f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 117001, 2068850345u, new Vector3(-1310.89087f, -822.0019f, 5.725086f), new Vector3(90f, 34.15837f, 0f), 688226, 0, new Vector3(1.999964f, 8.62702336E-08f, -0.5000871f), new Vector3(0f, 7.62939453E-06f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 294916, 2068850345u, new Vector3(-1309.23584f, -820.8788f, 5.725086f), new Vector3(90f, 34.15837f, 0f), 688226, 0, new Vector3(4.00010967f, 1.61345824E-07f, -0.5001637f), new Vector3(0f, 7.62939453E-06f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 310897, 2068850345u, new Vector3(-1310.35889f, -819.223938f, 5.725086f), new Vector3(90f, 34.15837f, 0f), 117001, 0, new Vector3(1.99999976f, 0f, -1.99999952f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 745320, 2068850345u, new Vector3(-1308.70386f, -818.101f, 5.725086f), new Vector3(90f, 34.15837f, 0f), 117001, 0, new Vector3(3.99999952f, 0f, -1.99999952f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 784251, 113622690u, new Vector3(-1302.04956f, -825.2087f, 9.696206f), new Vector3(-89.6689758f, -1.09308243f, 124.158478f), 269828, 0, new Vector3(3.97840357f, -1.271196f, 2.24004173f), new Vector3(-90f, 0.330883026f, -88.57587f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 619782, 113622690u, new Vector3(-1301.06958f, -824.5161f, 9.689278f), new Vector3(-89.6689758f, -1.09308243f, 124.158478f), 784251, 0, new Vector3(0f, 0f, -1.2f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 278279, 113622690u, new Vector3(-1300.08972f, -823.8235f, 9.682349f), new Vector3(-89.6689758f, -1.09308243f, 124.158478f), 784251, 0, new Vector3(0f, 0f, -2.39999986f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 657409, 113622690u, new Vector3(-1299.10974f, -823.1309f, 9.67542f), new Vector3(-89.6689758f, -1.09308243f, 124.158478f), 784251, 0, new Vector3(0f, 0f, -3.59999943f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 593516, 113622690u, new Vector3(-1298.12988f, -822.4383f, 9.668491f), new Vector3(-89.6689758f, -1.09308243f, 124.158478f), 784251, 0, new Vector3(0f, 0f, -4.79999828f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 528133, 113622690u, new Vector3(-1297.1499f, -821.7457f, 9.661563f), new Vector3(-89.6689758f, -1.09308243f, 124.158478f), 784251, 0, new Vector3(0f, 0f, -5.999997f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 23355, 113622690u, new Vector3(-1296.16992f, -821.053162f, 9.654634f), new Vector3(-89.6689758f, -1.09308243f, 124.158478f), 784251, 0, new Vector3(0f, 0f, -7.199996f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_battle_lights_ceiling_l_f"), 709379, 3402741872u, new Vector3(-1312.90479f, -827.0013f, 9.792787f), new Vector3(-0.000163335892f, 0.00737411529f, 122.262062f), 169071, 0, new Vector3(1.50117886f, -0.491437137f, 3.51570916f), new Vector3(-0.330864131f, -0.00357571966f, -1.27733231f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_battle_lights_ceiling_l_f"), 474733, 3402741872u, new Vector3(-1311.2135f, -825.9337f, 9.792792f), new Vector3(-0.000163335892f, 0.00737411529f, 122.262062f), 709379, 0, new Vector3(0f, -1.99999976f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_battle_lights_ceiling_l_f"), 564038, 3402741872u, new Vector3(-1309.52234f, -824.8661f, 9.792798f), new Vector3(-0.000163335892f, 0.00737411529f, 122.262062f), 709379, 0, new Vector3(0f, -3.99999976f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_battle_lights_ceiling_l_f"), 103181, 3402741872u, new Vector3(-1307.831f, -823.7985f, 9.792804f), new Vector3(-0.000163335892f, 0.00737411529f, 122.262062f), 709379, 0, new Vector3(0f, -5.99999952f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_battle_lights_ceiling_l_f"), 701700, 3402741872u, new Vector3(-1306.13977f, -822.7309f, 9.79280949f), new Vector3(-0.000163335892f, 0.00737411529f, 122.262062f), 709379, 0, new Vector3(5.4814177E-06f, -8.000095f, 2.38183929E-07f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_battle_lights_ceiling_l_f"), 277767, 3402741872u, new Vector3(-1312.8877f, -823.4443f, 9.792721f), new Vector3(-0.000163335892f, 0.00737411529f, 122.262062f), 499315, 0, new Vector3(0f, -1.99999976f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_battle_lights_ceiling_l_f"), 667502, 3402741872u, new Vector3(-1311.19653f, -822.376648f, 9.79272652f), new Vector3(-0.000163335892f, 0.00737411529f, 122.262062f), 499315, 0, new Vector3(0f, -3.99999976f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_battle_lights_ceiling_l_f"), 212753, 3402741872u, new Vector3(-1309.50525f, -821.3091f, 9.792732f), new Vector3(-0.000163335892f, 0.00737411529f, 122.262062f), 499315, 0, new Vector3(0f, -5.99999952f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_battle_lights_ceiling_l_f"), 708674, 3402741872u, new Vector3(-1307.814f, -820.241455f, 9.792738f), new Vector3(-0.000163335892f, 0.00737411529f, 122.262062f), 499315, 0, new Vector3(5.4814177E-06f, -8.000095f, 2.38183929E-07f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_battle_lights_ceiling_l_f"), 499315, 3402741872u, new Vector3(-1314.579f, -824.511841f, 9.792715f), new Vector3(-0.000163335892f, 0.00737411529f, 122.262062f), 169071, 0, new Vector3(4.50117874f, -0.471437126f, 3.51570916f), new Vector3(-0.330864131f, -0.00357571966f, -1.27733231f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_biker_boardchair01"), 100199, 3824151676u, new Vector3(-1305.21509f, -819.28186f, 6.963667f), new Vector3(-0.00685327174f, -0.005095285f, 51.530838f), 499315, 0, new Vector3(-0.575504f, -10.7100163f, -2.8291533f), new Vector3(0.000161653777f, -0.007374562f, -70.73123f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_biker_boardchair01"), 327980, 3824151676u, new Vector3(-1304.1698f, -818.7316f, 6.963667f), new Vector3(-0.00716631627f, -0.006535005f, 40.0669632f), 180527, 0, new Vector3(5.864172f, 6.0972023f, 0.409080863f), new Vector3(-0.0071655f, -0.0065355557f, -42.0009232f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_biker_boardchair01"), 121448, 3824151676u, new Vector3(-1303.13892f, -817.9328f, 6.963668f), new Vector3(-0.00710297376f, -0.006098055f, 43.49692f), 499315, 0, new Vector3(-0.542885959f, -13.1857338f, -2.829155f), new Vector3(0.000161653777f, -0.007374562f, -78.76515f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_biker_boardchair01"), 386877, 3824151676u, new Vector3(-1302.04614f, -817.0891f, 6.963667f), new Vector3(-0.007182914f, -0.00805398f, 28.2457428f), 499315, 0, new Vector3(-0.412830532f, -14.5601692f, -2.829143f), new Vector3(0.000161653777f, -0.007374562f, -94.01634f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_mp_h_acc_plant_palm_01"), 397181, 3906934191u, new Vector3(-1301.39709f, -825.581238f, 6.361719f), new Vector3(0.003205547f, 0.0180841256f, 144.887817f), 386877, 0, new Vector3(-3.44721079f, -7.788043f, -0.6024397f), new Vector3(0.007183476f, 0.008052334f, 116.642075f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_mp_h_acc_plant_palm_01"), 623195, 3906934191u, new Vector3(-1300.3446f, -817.9665f, 6.361719f), new Vector3(-0.0210909471f, -0.0222654045f, 31.6757317f), 397181, 0, new Vector3(3.51888824f, -6.83445072f, 0.00149322022f), new Vector3(-0.00320727169f, -0.0180838928f, -113.21209f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 688226, 2068850345u, new Vector3(-1312.265f, -823.538635f, 5.725086f), new Vector3(90f, 34.1583633f, 0f), 104201, 0, new Vector3(-5.99999666f, 0f, -2.00000024f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 371310, 113622690u, new Vector3(-1302.1925f, -822.8611f, 9.689278f), new Vector3(-89.6689758f, -1.09308243f, 124.158478f), 270082, 0, new Vector3(0f, 0f, -1.2f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 128573, 113622690u, new Vector3(-1301.21265f, -822.168457f, 9.682349f), new Vector3(-89.6689758f, -1.09308243f, 124.158478f), 270082, 0, new Vector3(0f, 0f, -2.39999986f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 611446, 113622690u, new Vector3(-1300.23267f, -821.4759f, 9.67542f), new Vector3(-89.6689758f, -1.09308243f, 124.158478f), 270082, 0, new Vector3(0f, 0f, -3.59999943f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 536939, 113622690u, new Vector3(-1299.25281f, -820.783264f, 9.668491f), new Vector3(-89.6689758f, -1.09308243f, 124.158478f), 270082, 0, new Vector3(0f, 0f, -4.79999828f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 460157, 113622690u, new Vector3(-1298.27283f, -820.0907f, 9.661563f), new Vector3(-89.6689758f, -1.09308243f, 124.158478f), 270082, 0, new Vector3(0f, 0f, -5.999997f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 508030, 113622690u, new Vector3(-1297.29285f, -819.398132f, 9.654634f), new Vector3(-89.6689758f, -1.09308243f, 124.158478f), 270082, 0, new Vector3(0f, 0f, -7.199996f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 270082, 113622690u, new Vector3(-1303.17249f, -823.55365f, 9.696206f), new Vector3(-89.6689758f, -1.09308243f, 124.158478f), 269828, 0, new Vector3(3.97840357f, -3.271196f, 2.24004173f), new Vector3(-90f, 0.330883026f, -88.57587f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 374785, 113622690u, new Vector3(-1303.31543f, -821.2061f, 9.689278f), new Vector3(-89.6689758f, -1.09308243f, 124.158478f), 196884, 0, new Vector3(0f, 0f, -1.2f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 100977, 113622690u, new Vector3(-1302.33557f, -820.5135f, 9.682349f), new Vector3(-89.6689758f, -1.09308243f, 124.158478f), 196884, 0, new Vector3(0f, 0f, -2.39999986f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 782711, 113622690u, new Vector3(-1301.35559f, -819.8209f, 9.67542f), new Vector3(-89.6689758f, -1.09308243f, 124.158478f), 196884, 0, new Vector3(0f, 0f, -3.59999943f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 700783, 113622690u, new Vector3(-1300.37573f, -819.1283f, 9.668491f), new Vector3(-89.6689758f, -1.09308243f, 124.158478f), 196884, 0, new Vector3(0f, 0f, -4.79999828f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 690183, 113622690u, new Vector3(-1299.39575f, -818.4357f, 9.661563f), new Vector3(-89.6689758f, -1.09308243f, 124.158478f), 196884, 0, new Vector3(0f, 0f, -5.999997f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 613243, 113622690u, new Vector3(-1298.41577f, -817.743164f, 9.654634f), new Vector3(-89.6689758f, -1.09308243f, 124.158478f), 196884, 0, new Vector3(0f, 0f, -7.199996f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 196884, 113622690u, new Vector3(-1304.29541f, -821.8987f, 9.696206f), new Vector3(-89.6689758f, -1.09308243f, 124.158478f), 269828, 0, new Vector3(3.97840357f, -5.271196f, 2.24004173f), new Vector3(-90f, 0.330883026f, -88.57587f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_mp_h_stn_chairarm_03"), 189284, 197077615u, new Vector3(-1303.25476f, -824.0446f, 6.36800241f), new Vector3(0.0009794367f, 0.0353660956f, 125.251236f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_mp_h_stn_chairarm_03"), 627800, 197077615u, new Vector3(-1302.50439f, -825.106262f, 6.368805f), new Vector3(0.0009794367f, 0.0353660956f, 125.251236f), 189284, 0, new Vector3(-1.30000007f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 586042, 2068850345u, new Vector3(-1303.12122f, -815.6424f, 5.72508574f), new Vector3(90f, 34.1583443f, 0f), 29008, 0, new Vector3(-2.00000024f, -1.49011612E-08f, MathF.PI * -113f / 142f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 281702, 2068850345u, new Vector3(-1304.77625f, -816.7654f, 5.72508574f), new Vector3(90f, 34.1583443f, 0f), 586042, 0, new Vector3(-2.00000024f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 302647, 2068850345u, new Vector3(-1306.43115f, -817.8883f, 5.72508574f), new Vector3(90f, 34.1583443f, 0f), 586042, 0, new Vector3(-3.99999857f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 545138, 2068850345u, new Vector3(-1302.70142f, -812.6989f, 5.72508574f), new Vector3(90f, 34.1583443f, 0f), 29008, 0, new Vector3(0f, -1.49011612E-08f, -4.699998f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 452154, 2068850345u, new Vector3(-1304.35645f, -813.8219f, 5.72508574f), new Vector3(90f, 34.1583443f, 0f), 29008, 0, new Vector3(-2.00000024f, -1.49011612E-08f, -4.699998f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 170246, 2068850345u, new Vector3(-1306.01147f, -814.9449f, 5.72508574f), new Vector3(90f, 34.1583443f, 0f), 29008, 0, new Vector3(-3.99999857f, -1.49011612E-08f, -4.699998f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 125244, 2068850345u, new Vector3(-1307.6665f, -816.068f, 5.725086f), new Vector3(90f, 34.1583443f, 0f), 180527, 0, new Vector3(8.019806f, 9.92797852f, -0.8295002f), new Vector3(90f, -120.831009f, 72.92147f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_service_pillar_02a"), 505214, 2002969729u, new Vector3(-1303.1582f, -814.122131f, 6.202492f), new Vector3(0.0058556865f, 0.330772281f, 35.27347f), 281100, 0, new Vector3(5.797796f, 1.75304186f, 0.157948926f), new Vector3(0.7716923f, -0.5452982f, 0.00141906738f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_service_pillar_02a"), 712194, 2002969729u, new Vector3(-1306.66052f, -821.2989f, 9.642387f), new Vector3(0.3308024f, -89.99297f, 122.944832f), 269828, 0, new Vector3(5.598387f, -7.095514f, 2.17686725f), new Vector3(0f, -89.99999f, -91.21345f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sm_prop_smug_hgrdoors_01"), 43374, 3293339660u, new Vector3(-1299.96094f, -825.6051f, 1.93621731f), new Vector3(0.6495673f, 0.09964037f, -145.649582f), 180527, 0, new Vector3(-0.362718165f, 0.980038643f, -4.618369f), new Vector3(0.6495663f, 0.09964081f, 132.282532f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sm_prop_smug_hgrdoors_01"), 162411, 3293339660u, new Vector3(-1300.5011f, -828.692566f, 1.973176f), new Vector3(0.652703166f, 0.752044f, 125.244858f), 180527, 0, new Vector3(-3.495152f, 1.08897126f, -4.58141041f), new Vector3(0.6527026f, 0.7520433f, 43.17698f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sm_prop_smug_hgrdoors_01"), 700929, 3293339660u, new Vector3(-1297.9657f, -818.1268f, 1.88648891f), new Vector3(-0.09963308f, 0.6495679f, 125.351547f), 180527, 0, new Vector3(7.31940651f, 0.0359302759f, -4.6680975f), new Vector3(-0.09963346f, 0.6495672f, 43.2836838f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sm_prop_smug_hgrdoors_01"), 740625, 3293339660u, new Vector3(-1315.25708f, -822.009949f, 1.78627717f), new Vector3(-0.645694137f, -0.122249924f, 32.3503f), 43374, 0, new Vector3(10.6000013f, -11.5999985f, 0f), new Vector3(0f, 0f, 178.000015f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sm_prop_smug_hgrdoors_01"), 611701, 3293339660u, new Vector3(-1313.63708f, -821.865051f, 1.53220844f), new Vector3(-0.6456948f, -0.122250296f, 33.3503227f), 180527, 0, new Vector3(1.45426333f, 15.0415583f, -5.022378f), new Vector3(-0.6456939f, -0.12225078f, -48.71756f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sm_prop_smug_hgrdoors_01"), 232980, 3293339660u, new Vector3(-1310.00879f, -827.122f, 1.66363239f), new Vector3(-0.6456948f, -0.122250296f, 33.3503227f), 180527, 0, new Vector3(-3.25173759f, 10.7225161f, -4.890954f), new Vector3(-0.6456939f, -0.12225078f, -48.71756f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sm_prop_smug_hgrdoors_01"), 26957, 3293339660u, new Vector3(-1312.45569f, -828.8578f, 1.65841579f), new Vector3(0.122241691f, -0.645699f, -56.6482849f), 180527, 0, new Vector3(-5.308571f, 12.9064054f, -4.89617062f), new Vector3(0.122242145f, -0.645698249f, -138.716171f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_service_pillar_02a"), 583688, 2002969729u, new Vector3(-1306.187f, -819.8832f, 6.28025341f), new Vector3(-0.7657679f, 0.8761296f, 35.2838554f), 26957, 0, new Vector3(-3.99812126f, 10.1801291f, 4.64547968f), new Vector3(-0.116313383f, 0.9765246f, -268.0664f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sm_prop_smug_hgrdoors_01"), 614771, 3293339660u, new Vector3(-1308.53284f, -818.508667f, 1.60519886f), new Vector3(-0.645694f, -0.122249879f, 33.2503357f), 180527, 0, new Vector3(5.482915f, 10.4493027f, -4.94938755f), new Vector3(-0.6456932f, -0.122250363f, -48.81756f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sm_prop_smug_hgrdoors_01"), 339970, 3293339660u, new Vector3(-1308.98291f, -817.6854f, 1.799188f), new Vector3(-0.645694137f, -0.122249924f, 32.3503f), 43374, 0, new Vector3(2.97985148f, -11.6299992f, 0f), new Vector3(0f, 0f, 178.000015f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sm_prop_smug_hgrdoors_01"), 742265, 3293339660u, new Vector3(-1304.38513f, -818.8824f, 9.921869f), new Vector3(-89.24515f, 7.57208538f, -137.077011f), 180527, 0, new Vector3(5.68512154f, 6.28963041f, 3.367283f), new Vector3(-89.24463f, 7.57206f, -219.144928f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sm_prop_smug_hgrdoors_01"), 200805, 3293339660u, new Vector3(-1314.24561f, -821.0529f, 9.680368f), new Vector3(-89.24515f, 7.57208538f, -137.077011f), 180527, 0, new Vector3(2.174628f, 15.7562447f, 3.125782f), new Vector3(-89.24463f, 7.57206f, -219.144928f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_service_pillar_02a"), 564602, 2002969729u, new Vector3(-1305.6676f, -820.6251f, 9.635458f), new Vector3(-0.330811f, 89.9930038f, -57.0551567f), 269828, 0, new Vector3(4.398388f, -7.095514f, 2.17686725f), new Vector3(180f, -89.9999847f, -91.21345f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_service_pillar_02a"), 281100, 2002969729u, new Vector3(-1306.87927f, -818.903748f, 6.15664959f), new Vector3(-0.7657679f, 0.8761296f, 35.2838554f), 26957, 0, new Vector3(-5.19812f, 10.1401281f, 4.535477f), new Vector3(-0.116313383f, 0.9765246f, -268.0664f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_service_pillar_02a"), 175388, 2002969729u, new Vector3(-1307.56946f, -817.909546f, 6.142949f), new Vector3(-0.7657679f, 0.8761296f, 35.2838554f), 26957, 0, new Vector3(-6.408148f, 10.1101274f, 4.535477f), new Vector3(-0.116313383f, 0.9765246f, -268.0664f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_service_pillar_02a"), 605968, 2002969729u, new Vector3(-1302.98364f, -814.1763f, 10.0330267f), new Vector3(89.96573f, 35.05125f, 0f), -1, 0, new Vector3(1f, 0.3829999f, 2.57000232f), new Vector3(89.9999847f, -89.99999f, -0.199999988f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sm_prop_smug_hgrdoors_01"), 404232, 3293339660u, new Vector3(-1306.94409f, -815.7934f, 9.905996f), new Vector3(-89.34448f, 8.720132f, -135.928864f), 180527, 0, new Vector3(8.391431f, 9.250459f, 3.35141015f), new Vector3(-89.34358f, 8.720347f, -217.996521f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_service_pillar_02a"), 360293, 2002969729u, new Vector3(-1301.39722f, -816.610962f, 6.29150057f), new Vector3(-0.7657679f, 0.8761296f, 35.2838554f), 26957, 0, new Vector3(-4.098121f, 15.9801512f, 4.64547968f), new Vector3(-0.116313383f, 0.9765246f, -268.0664f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sm_prop_smug_hgrdoors_01"), 313862, 3293339660u, new Vector3(-1308.53284f, -818.508667f, 1.80519867f), new Vector3(-0.645694f, -0.122249879f, 33.2503357f), 180527, 0, new Vector3(5.482915f, 10.4493027f, -4.74938774f), new Vector3(-0.6456932f, -0.122250363f, -48.81756f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 180527, 1803116220u, new Vector3(-1298.94019f, -825.3811f, 6.55458641f), new Vector3(-2.38417527E-07f, 9.556377E-07f, 82.06788f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sm_prop_smug_hgrdoors_01"), 485753, 3293339660u, new Vector3(-1307.12842f, -815.675354f, 1.80519867f), new Vector3(-0.645694f, -0.122249879f, 33.2503357f), 180527, 0, new Vector3(8.482915f, 9.449303f, -4.74938774f), new Vector3(-0.6456932f, -0.122250363f, -48.81756f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_table_vault_01b"), 432657, 2413416523u, new Vector3(-1306.145f, -818.3317f, 6.361465f), new Vector3(0f, -0f, 124.421043f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_02_sm"), 597286, 1924666731u, new Vector3(-1306.1366f, -818.7745f, 7.374174f), new Vector3(0f, -0f, 147.349182f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_02_sm"), 745501, 1924666731u, new Vector3(-1306.42419f, -818.4728f, 7.374174f), new Vector3(0f, -0f, 127.644234f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_02_sm"), 228872, 1924666731u, new Vector3(-1306.60144f, -818.0835f, 7.374174f), new Vector3(0f, -0f, 106.966888f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_02_sm"), 299300, 1924666731u, new Vector3(-1306.57629f, -818.097534f, 7.70058346f), new Vector3(0f, -0f, 107.990372f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_02_sm"), 752147, 1924666731u, new Vector3(-1306.373f, -818.4528f, 7.70058346f), new Vector3(0f, -0f, 128.411377f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_02_sm"), 450320, 1924666731u, new Vector3(-1306.12732f, -818.7423f, 7.70058346f), new Vector3(0f, -0f, 146.88736f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects Objects_VaultDoor = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_vault_door"), 569862, 3030613028u, new Vector3(-1304.39343f, -822.4342f, 7.5694766f), new Vector3(0.3308626f, 0.00375391915f, 123.508652f), 181881, 0, new Vector3(0.857087f, 0.232263863f, 1.19791889f), new Vector3(0f, 0f, -179.240936f))
		});

		public static MissionOjects Objects_Cameras = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_bank_cctv_02"), 675432, 2452560208u, new Vector3(-1295.59631f, -822.12616f, 9.897005f), new Vector3(5.589579E-07f, -1.886949E-06f, -102.916161f), 180527, 0, new Vector3(3.6852746f, -2.86265326f, 3.34241867f), new Vector3(2.38417527E-07f, -9.556378E-07f, -184.984055f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_bank_cctv_02"), 475224, 2452560208u, new Vector3(-1305.193f, -820.936157f, 9.79935f), new Vector3(7.206771E-07f, -9.685249E-08f, 125.393196f), 180527, 0, new Vector3(3.53953362f, 6.806339f, 3.24476337f), new Vector3(2.38417527E-07f, -9.556378E-07f, 43.3253174f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_bank_cctv_02"), 165752, 2452560208u, new Vector3(-1300.52185f, -817.7638f, 9.810663f), new Vector3(-6.231745E-07f, -4.78410357E-07f, 35.0580063f), 180527, 0, new Vector3(7.32613f, 2.61772537f, 3.256077f), new Vector3(2.38417527E-07f, -9.556378E-07f, -47.00988f))
		});

		public static MissionOjects Objects_Loot = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_cash_trolly_01c"), 99085, 412463629u, new Vector3(-1310.20288f, -820.436768f, 6.835346f), new Vector3(1.1089079E-06f, -1.41643386E-06f, -146.028992f), 180527, 0, new Vector3(3.34279871f, 11.8372469f, 0.280759931f), new Vector3(2.38417527E-07f, -9.556378E-07f, -228.0969f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_cash_trolly_01c"), 40714, 412463629u, new Vector3(-1307.66174f, -824.4562f, 6.835346f), new Vector3(-6.41332E-07f, -5.12775046E-07f, 32.79689f), 180527, 0, new Vector3(-0.287490249f, 8.765713f, 0.280759841f), new Vector3(2.38417527E-07f, -9.556378E-07f, -49.271f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_cash_trolly_01c"), 442126, 412463629u, new Vector3(-1310.36975f, -826.2454f, 6.835346f), new Vector3(-6.401457E-07f, -5.104264E-07f, 32.9499474f), 180527, 0, new Vector3(-2.43334532f, 11.2009611f, 0.2807598f), new Vector3(2.38417527E-07f, -9.556378E-07f, -49.1179352f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_cash_trolly_01c"), 298362, 412463629u, new Vector3(-1313.67419f, -825.458435f, 6.83974266f), new Vector3(-2.17349779E-07f, -1.82877181E-06f, -56.35952f), 180527, 0, new Vector3(-2.10986948f, 14.5823812f, 0.285156339f), new Vector3(2.38417527E-07f, -9.556378E-07f, -138.427414f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_cash_trolly_01c"), 134419, 412463629u, new Vector3(-1312.91174f, -2583f / MathF.PI, 6.83534527f), new Vector3(2.0641105E-06f, 1.63833511E-06f, -146.794922f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects Objects_SafetyDepositBoxes = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 670059, 2068850345u, new Vector3(-1307.53564f, -819.518f, 6.36146641f), new Vector3(0f, 0f, -56.05711f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 27989, 2068850345u, new Vector3(-1309.05139f, -819.2478f, 6.36146641f), new Vector3(-2.04890966E-08f, 1.91765537E-08f, 32.2804947f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 534841, 2068850345u, new Vector3(-1311.71448f, -821.0204f, 6.36146641f), new Vector3(-3.001547E-08f, 2.28174017E-07f, 32.3414536f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 755295, 2068850345u, new Vector3(-1306.20129f, -823.828f, 6.36146641f), new Vector3(0f, -0f, -146.730209f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 414306, 2068850345u, new Vector3(-1308.85f, -825.618469f, 6.36146641f), new Vector3(0f, -0f, -146.730209f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 21861, 2068850345u, new Vector3(-1311.59875f, -827.3286f, 6.36146641f), new Vector3(0f, -0f, -146.730209f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 217893, 2068850345u, new Vector3(-1313.04944f, -826.949951f, 6.381467f), new Vector3(0f, -0f, 124.008392f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 732988, 2068850345u, new Vector3(-1314.70044f, -824.2528f, 6.351466f), new Vector3(0f, 0.0100000007f, 122.318466f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_02a"), 473708, 2068850345u, new Vector3(-1314.4436f, -822.788269f, 6.36146641f), new Vector3(0f, 0f, 32.8021164f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static Ped Manager;

		public static List<Prop> SafetyDepositBoxes = new List<Prop>();

		public static Prop OfficeDoor;

		public static Prop _thermiteProp;

		public static Prop BagProp;

		public static Blip MainBlip;

		public static bool ManagerHandsUp;

		public static bool TheMazeBankSavngsBankHeistOn = false;

		public static Heiststage CurrentHeistStage = Heiststage.None;

		public static Vector3 EnterMarker;

		public static Vector3 ExitMarker;

		public static bool IsInInterior;

		public static bool VaultOpen;

		public static bool VaultOpenning;

		public static List<Prop> Cameras = new List<Prop>();

		public static Prop VaultDoor;

		public static bool HackedKeypad;

		public static KeypadClass MainKeypad;

		public static bool PlayeringThirmiteDoor1;

		public static bool ThermiteDone;

		public static bool ThermiteCuttingThroughVault;

		public static string DictToUse;

		public static string DictToUse2;

		public static int PlantThermiteSyncScene;

		public static int PTFX1;

		public static int PTFX2;

		public static int PushButtonScene;

		public static int PushButtonStage;

		public static bool MagneticLocksUnlocked;

		public static bool HackingKeypad = false;

		public static Prop LastKeypad;

		public static Prop USB;

		public static Prop Phone;

		public int Scene1;

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

		public TheMazebankSavingsBankHeistcs()
		{
			base.Tick += OnTick;
		}

		public static void StartHeist()
		{
			ManageRobberyAnims.Heist_Average_Pay = (int)((float)(HeistMinPay + HeistMaxPay) / 2f * 0.8f);
			ManageRobberyAnims.CurrentPay = 0;
			ManageRobberyAnims.Heist_Average_Pay = (float)(HeistMinPay + HeistMaxPay) / 2f * 0.9f;
			Function.Call(Hash.BEGIN_TEXT_COMMAND_BUSYSPINNER_ON, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "Loading The Mazebank Savings Bank Heist, Please Wait");
			Function.Call(Hash.END_TEXT_COMMAND_BUSYSPINNER_ON, 5);
			EndHeist();
			CurrentHeistStage = Heiststage.HeadingToBank;
			TheMazeBankSavngsBankHeistOn = true;
			MazebankInterior.CreateEntites();
			Objects_VaultDoor.CreateEntites();
			Objects_Cameras.CreateEntites();
			Objects_Loot.CreateEntites();
			Objects_SafetyDepositBoxes.CreateEntites();
			PercentNeedTake = (int)((float)(HeistMinPay + HeistMaxPay) / 1.5f / 100f * 30f);
			EnterMarker = new Vector3(-1307.68f, -832.74f, 16.13f);
			MazebankInterior.Entities[0].Entity.Opacity = 0;
			ExitMarker = MazebankInterior.Entities[1].Position;
			MazebankInterior.Entities[1].Entity.Opacity = 0;
			MazebankInterior.Entities[2].Entity.IsPositionFrozen = false;
			OfficeDoor = (Prop)MazebankInterior.Entities[2].Entity;
			Function.Call(Hash.BUSYSPINNER_OFF);
			if (MainBlip != null)
			{
				MainBlip.Delete();
			}
			MainBlip = GlobalHelper.CreateBlip(EnterMarker);
			MainBlip.Color = BlipColor.Yellow2;
			MainBlip.IsShortRange = true;
			MainBlip.Name = "Enter the Mazebank Branch";
			Manager = (Ped)MazebankInterior.Entities[4].Entity;
		}

		public static void EndHeist()
		{
			Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
			if (MainKeypad != null && MainKeypad.Keypad != null && MainKeypad.Keypad.AttachedBlip != null)
			{
				MainKeypad.Keypad.AttachedBlip.Delete();
			}
			CurrentPay = 0;
			LocalTake = 0;
			ManageRobberyAnims.CurrentPay = 0;
			ManageRobberyAnims.DeleteLoot();
			ManagerHandsUp = false;
			SmartGuards.ClearGenericGuards();
			TheMazeBankSavngsBankHeistOn = false;
			if (MainBlip != null)
			{
				MainBlip.Delete();
			}
			MazebankInterior.DeleteEntites();
			Objects_VaultDoor.DeleteEntites();
			Objects_Cameras.DeleteEntites();
			Objects_Loot.DeleteEntites();
			Objects_SafetyDepositBoxes.DeleteEntites();
			Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX2, false);
			Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX1, false);
			foreach (Prop safetyDepositBox in SafetyDepositBoxes)
			{
				if (safetyDepositBox != null)
				{
					safetyDepositBox.Delete();
				}
			}
			if (_thermiteProp != null)
			{
				_thermiteProp.Delete();
			}
			if (BagProp != null)
			{
				BagProp.Delete();
			}
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

		private void OnTick(object sender, EventArgs e)
		{
			if (!TheMazeBankSavngsBankHeistOn || !TheMazeBankSavngsBankHeistOn)
			{
				return;
			}
			if (Manager != null && (Game.Player.IsTargeting(Manager) || ManagerHandsUp) && !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Manager, "mp_am_hold_up", "handsup_enter", 3))
			{
				ManagerHandsUp = true;
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 2);
				Manager.Task.PlayAnimation("mp_am_hold_up", "handsup_enter", 1f, -1, AnimationFlags.None);
			}
			SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Security;
			Heiststage currentHeistStage = CurrentHeistStage;
			UI.RenderQue = 0f;
			switch (currentHeistStage)
			{
			case Heiststage.HeadingToBank:
				if (GetDistance_VDIST2(Game.Player.Character.Position, EnterMarker, "<", 30f))
				{
					Screen.ShowSubtitle("Enter the ~y~Mazebank Savings Branch~w~");
					if (!GetDistance_VDIST2(Game.Player.Character.Position, EnterMarker, "<", 100f))
					{
						break;
					}
					World.DrawMarker(MarkerType.Cylinder, EnterMarker, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
					if (!GetDistance_VDIST2(Game.Player.Character.Position, EnterMarker, "<", 2f))
					{
						break;
					}
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter The Mazebank Savings Branch");
					if (!Game.IsControlJustPressed(Control.Context))
					{
						break;
					}
					if (MainBlip != null)
					{
						MainBlip.Alpha = 0;
					}
					Screen.FadeOut(1500);
					Script.Wait(1500);
					IsInInterior = true;
					Game.Player.Character.Position = ExitMarker;
					CurrentHeistStage = Heiststage.TakeOutCameras;
					Script.Wait(1500);
					Screen.FadeIn(1500);
					foreach (ObjectPlacementMap entity in Objects_Loot.Entities)
					{
						if (entity.Entity != null)
						{
							ManageRobberyAnims.AddPropAsLoot((Prop)entity.Entity);
						}
					}
					foreach (ObjectPlacementMap entity2 in Objects_SafetyDepositBoxes.Entities)
					{
						if (entity2.Entity != null)
						{
							entity2.Entity.Opacity = 0;
							Random random2 = new Random();
							int num2 = random2.Next(1, 9);
							if (num2 == 1)
							{
								Prop prop3 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), entity2.Entity.Position, new Vector3(0f, 0f, entity2.Entity.Rotation.Z), false, false);
								Prop prop4 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01a"), entity2.Entity.Position, new Vector3(0f, 0f, entity2.Entity.Rotation.Z), false, false);
								prop4.Position = prop3.GetOffsetPosition(new Vector3(0.4f, 0f, 0f));
								ManageRobberyAnims.AddPropAsLoot(prop4);
								prop3.IsVisible = false;
								PaintingHolder.Add(prop3);
							}
							if (num2 == 2)
							{
								Prop prop5 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), entity2.Entity.Position, new Vector3(0f, 0f, entity2.Entity.Rotation.Z), false, false);
								Prop prop6 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01b"), entity2.Entity.Position, new Vector3(0f, 0f, entity2.Entity.Rotation.Z), false, false);
								prop6.PositionNoOffset = entity2.Entity.Position;
								prop6.Rotation = new Vector3(0f, 0f, entity2.Entity.Rotation.Z);
								prop6.Position = prop5.Position;
								ManageRobberyAnims.AddPropAsLoot(prop6);
								prop5.IsVisible = false;
								PaintingHolder.Add(prop5);
							}
							if (num2 == 3)
							{
								Prop prop7 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), entity2.Entity.Position, new Vector3(0f, 0f, entity2.Entity.Rotation.Z), false, false);
								Prop prop8 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01c"), entity2.Entity.Position, new Vector3(0f, 0f, entity2.Entity.Rotation.Z), false, false);
								prop8.PositionNoOffset = entity2.Entity.Position;
								prop8.Rotation = new Vector3(0f, 0f, entity2.Entity.Rotation.Z);
								prop8.Position = prop7.GetOffsetPosition(new Vector3(-0.4f, 0f, 0f));
								ManageRobberyAnims.AddPropAsLoot(prop8);
								prop7.IsVisible = false;
								PaintingHolder.Add(prop7);
							}
							if (num2 == 4)
							{
								Prop prop9 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), entity2.Entity.Position, new Vector3(0f, 0f, entity2.Entity.Rotation.Z), false, false);
								Prop prop10 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01b"), entity2.Entity.Position, new Vector3(0f, 0f, entity2.Entity.Rotation.Z), false, false);
								prop10.PositionNoOffset = entity2.Entity.Position;
								prop10.Rotation = new Vector3(0f, 0f, entity2.Entity.Rotation.Z);
								prop10.Position = prop9.Position;
								ManageRobberyAnims.AddPropAsLoot(prop10);
								prop9.IsVisible = false;
								PaintingHolder.Add(prop9);
							}
							if (num2 == 5)
							{
								Prop prop11 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), entity2.Entity.Position, new Vector3(0f, 0f, entity2.Entity.Rotation.Z), false, false);
								Prop prop12 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01e"), entity2.Entity.Position, new Vector3(0f, 0f, entity2.Entity.Rotation.Z), false, false);
								prop12.PositionNoOffset = entity2.Entity.Position;
								prop12.Rotation = new Vector3(0f, 0f, entity2.Entity.Rotation.Z);
								prop12.Position = prop11.Position;
								ManageRobberyAnims.AddPropAsLoot(prop12);
								prop11.IsVisible = false;
								PaintingHolder.Add(prop11);
							}
							if (num2 == 6)
							{
								Prop prop13 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), entity2.Entity.Position, new Vector3(0f, 0f, entity2.Entity.Rotation.Z), false, false);
								Prop prop14 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01f"), entity2.Entity.Position, new Vector3(0f, 0f, entity2.Entity.Rotation.Z), false, false);
								prop14.PositionNoOffset = entity2.Entity.Position;
								prop14.Rotation = new Vector3(0f, 0f, entity2.Entity.Rotation.Z);
								prop14.Position = prop13.GetOffsetPosition(new Vector3(-0.4f, 0f, 0f));
								ManageRobberyAnims.AddPropAsLoot(prop14);
								prop13.IsVisible = false;
								PaintingHolder.Add(prop13);
							}
							if (num2 == 7)
							{
								Prop prop15 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), entity2.Entity.Position, new Vector3(0f, 0f, entity2.Entity.Rotation.Z), false, false);
								Prop prop16 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01g"), entity2.Entity.Position, new Vector3(0f, 0f, entity2.Entity.Rotation.Z), false, false);
								prop16.PositionNoOffset = entity2.Entity.Position;
								prop16.Rotation = new Vector3(0f, 0f, entity2.Entity.Rotation.Z);
								prop16.Position = prop15.GetOffsetPosition(new Vector3(0.4f, 0f, 0f));
								ManageRobberyAnims.AddPropAsLoot(prop16);
								prop15.IsVisible = false;
								PaintingHolder.Add(prop15);
							}
							if (num2 == 8)
							{
								Prop prop17 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), entity2.Entity.Position, new Vector3(0f, 0f, entity2.Entity.Rotation.Z), false, false);
								Prop prop18 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01h"), entity2.Entity.Position, new Vector3(0f, 0f, entity2.Entity.Rotation.Z), false, false);
								prop18.PositionNoOffset = entity2.Entity.Position;
								prop18.Rotation = new Vector3(0f, 0f, entity2.Entity.Rotation.Z);
								prop18.Position = prop17.Position;
								ManageRobberyAnims.AddPropAsLoot(prop18);
								prop17.IsVisible = false;
								PaintingHolder.Add(prop17);
							}
							if (num2 == 9)
							{
								Prop prop19 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), entity2.Entity.Position, new Vector3(0f, 0f, entity2.Entity.Rotation.Z), false, false);
								Prop prop20 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01i"), entity2.Entity.Position, new Vector3(0f, 0f, entity2.Entity.Rotation.Z), false, false);
								prop20.PositionNoOffset = entity2.Entity.Position;
								prop20.Rotation = new Vector3(0f, 0f, entity2.Entity.Rotation.Z);
								prop20.Position = prop19.Position;
								ManageRobberyAnims.AddPropAsLoot(prop20);
								prop19.IsVisible = false;
								PaintingHolder.Add(prop19);
							}
							Script.Wait(1);
						}
					}
					foreach (ObjectPlacementMap entity3 in Objects_Cameras.Entities)
					{
						if (entity3.Entity != null)
						{
							Prop prop21 = (Prop)entity3.Entity;
							Cameras.Add(prop21);
							prop21.AddBlip();
							prop21.AttachedBlip.Sprite = BlipSprite.Standard;
							prop21.AttachedBlip.Color = BlipColor.Red2;
							prop21.AttachedBlip.Name = "Camera";
							prop21.AttachedBlip.Scale = 0.5f;
						}
					}
					Ped ped = (Ped)MazebankInterior.Entities[3].Entity;
					ped.IsPositionFrozen = false;
					SmartGuards.GuardGroup[13].GroupList.Add(new SmartGuards.SmartAiV2(ped, SmartGuards.EnemyGroup.Security));
				}
				else
				{
					Screen.ShowSubtitle("Make your way to the ~y~Mazebank Branch~w~");
				}
				break;
			case Heiststage.TakeOutCameras:
			{
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
				Game.Player.WantedLevel = 0;
				Screen.ShowSubtitle("Take out the ~r~Cameras~w~ and the ~r~Security Guard~w~");
				bool flag2 = true;
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
						flag2 = false;
					}
				}
				if (flag2)
				{
					CurrentHeistStage = Heiststage.PlaceThermiteOnVault;
				}
				break;
			}
			case Heiststage.PlaceThermiteOnVault:
			{
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
				Game.Player.WantedLevel = 0;
				Screen.ShowSubtitle("Place ~o~Thermite~w~ on the ~y~Vault door~w~ or ~y~Hack~w~ the Keypad");
				if (ThermiteDone)
				{
					CurrentHeistStage = Heiststage.UnlockMagneticLocks;
				}
				if (HackedKeypad)
				{
					CurrentHeistStage = Heiststage.LootTheVault;
				}
				Vector3 position = Game.Player.Character.Position;
				Prop prop = (Prop)MazebankInterior.Entities[6].Entity;
				if (prop != null && prop.Exists() && MainKeypad == null)
				{
					MainKeypad = new KeypadClass(prop, 2);
				}
				if (HackedKeypad)
				{
					CurrentHeistStage = Heiststage.LootTheVault;
				}
				bool flag = false;
				Prop prop2 = (Prop)Objects_VaultDoor.Entities[0].Entity;
				if (prop2 != null)
				{
					Vector3 offsetPosition = prop2.GetOffsetPosition(new Vector3(1.05f, -0.18f, 0f));
					World.DrawMarker(MarkerType.Sphere, offsetPosition, Vector3.Zero, Vector3.Zero, new Vector3(0.01f, 0.01f, 0.01f), Color.AliceBlue);
					if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition, "<", 1.25f))
					{
						if (!PlayeringThirmiteDoor1)
						{
							flag = true;
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Plant Thermal Charge");
						}
						if (Game.IsControlJustPressed(Control.Context) && !PlayeringThirmiteDoor1)
						{
							if (OfficeDoor != null)
							{
								OfficeDoor.Detach();
								OfficeDoor.IsPositionFrozen = true;
								OfficeDoor.Rotation = new Vector3(OfficeDoor.Rotation.X, OfficeDoor.Rotation.Y, OfficeDoor.Rotation.Z - 90f);
							}
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
							PlantThermiteSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.0, 0.0, prop2.Heading, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, PlantThermiteSyncScene, LoadDict("anim@heists@ornate_bank@thermal_charge"), "thermal_charge", 1f, -1f, 3341, 16, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BagProp, PlantThermiteSyncScene, "bag_thermal_charge", LoadDict("anim@heists@ornate_bank@thermal_charge"), 1f, -1f, 0, 1148846080);
							Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, BagProp);
							_thermiteProp = GlobalHelper.CreateProp(RequestModel("hei_prop_heist_thermite"), Game.Player.Character.Position, false, false);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, _thermiteProp, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, -90f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							Function.Call(Hash.PLAY_ENTITY_ANIM, _thermiteProp, "bomb_thermal_charge", "anim@heists@ornate_bank@thermal_charge", 1000.0, 1, 1, 1, 0, 0);
						}
					}
					if (PlayeringThirmiteDoor1)
					{
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
						if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, PlantThermiteSyncScene) >= 1065353216 && !ThermiteDone)
						{
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
					}
				}
				if (MainKeypad != null && !flag && MainKeypad.Keypad != null && !PlayeringThirmiteDoor1)
				{
					CheckKeypad(MainKeypad);
				}
				break;
			}
			case Heiststage.UnlockMagneticLocks:
			{
				if (MainKeypad != null && MainKeypad.Keypad != null && MainKeypad.Keypad.Exists() && MainKeypad.Keypad.AttachedBlip != null)
				{
					MainKeypad.Keypad.AttachedBlip.Alpha = 0;
				}
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
				Game.Player.WantedLevel = 0;
				Screen.ShowSubtitle("Figure out a way to unlock the ~O~Magnetic Locks~w~ on the ~y~Vault");
				Vector3 position3 = Game.Player.Character.Position;
				Prop prop23 = (Prop)MazebankInterior.Entities[5].Entity;
				if (!(prop23 != null))
				{
					break;
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, prop23.Position, "<", 1.35f) && !MagneticLocksUnlocked)
				{
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To release the magnetic locks on the vault door", 1);
					if (Game.IsControlJustPressed(Control.Context))
					{
						LoadDict("anim@scripted@charlie_missions@mission_6@ig1_button_press@male@");
						Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
						Vector3 offsetPosition2 = prop23.GetOffsetPosition(new Vector3(0f, -0.01f, 0f));
						PushButtonScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition2.X, offsetPosition2.Y, offsetPosition2.Z, prop23.Rotation.X, prop23.Rotation.Y, prop23.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, PushButtonScene, LoadDict("anim@scripted@charlie_missions@mission_6@ig1_button_press@male@"), "push_button", 1000.0, -4.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, PushButtonScene, "push_button", LoadDict("anim@scripted@charlie_missions@mission_6@ig1_button_press@male@"), 1000.0, -8f, -8f, prop23.Rotation.Z, 1148846080, 0);
						PushButtonStage = 1;
					}
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, PushButtonScene) >= 1065353216 && PushButtonStage == 1)
				{
					CurrentHeistStage = Heiststage.LootTheVault;
					MagneticLocksUnlocked = true;
					PushButtonStage = 0;
					Game.Player.Character.Task.ClearAllImmediately();
				}
				break;
			}
			case Heiststage.LootTheVault:
			{
				if (MainKeypad != null && MainKeypad.Keypad != null && MainKeypad.Keypad.Exists() && MainKeypad.Keypad.AttachedBlip != null)
				{
					MainKeypad.Keypad.AttachedBlip.Alpha = 0;
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
				Screen.ShowSubtitle("Loot at least ~g~$" + PercentNeedTake.ToString("N") + "~w~ from the ~y~Vault");
				if (ManageRobberyAnims.CurrentPay >= PercentNeedTake)
				{
					CurrentHeistStage = Heiststage.LeaveTheBank;
				}
				Vector3 position2 = Game.Player.Character.Position;
				Prop prop22 = (Prop)Objects_VaultDoor.Entities[0].Entity;
				if (prop22 != null && prop22.Rotation.Z > 10f)
				{
					if (prop22.IsAttached())
					{
						prop22.Detach();
					}
					prop22.IsPositionFrozen = true;
					float z = prop22.Rotation.Z;
					prop22.Rotation = new Vector3(prop22.Rotation.X, prop22.Rotation.Y, z -= 0.25f);
				}
				break;
			}
			case Heiststage.LeaveTheBank:
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
				Screen.ShowSubtitle("Leave the ~y~Mazebank Savings Branch");
				if (!GetDistance_VDIST2(Game.Player.Character.Position, ExitMarker, "<", 100f))
				{
					break;
				}
				World.DrawMarker(MarkerType.Cylinder, ExitMarker, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
				if (GetDistance_VDIST2(Game.Player.Character.Position, ExitMarker, "<", 2f))
				{
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit The Mazebank Savings Branch");
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
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
				Screen.ShowSubtitle("Escape the ~r~Cops");
				if (Game.Player.WantedLevel == 0)
				{
					Main.TotalCrewEarned += ManageRobberyAnims.CurrentPay;
					Main.TotalPlayerEarned += 0f;
					XP.CompleteHeist_TheMazebankSavingsBankHeist++;
					Main.SaveCuts();
					Class1.SetNoLongerInHeist();
					MissionScreen.SetPass("The Mazebank Savings Heist", ManageRobberyAnims.CurrentPay, 0, "The Player has died", 2, 0f, 5);
					int num = Game.GameTime + 30000;
					while (HKHModHelperNew.Class1.HeistPassedScreen != -1 && Game.GameTime <= num)
					{
						Script.Wait(0);
					}
					Script.Wait(2000);
					ManagePlayerFunds.AddFunds(ManageRobberyAnims.CurrentPay);
					XP.AddXP_Percent(XP.CompleteHeist_XPPercent_TheMazebankSavingsBankHeist);
					EndHeist();
				}
				break;
			}
			if (!Game.Player.IsAlive)
			{
				MissionScreen.SetFail("The Mazebank Savings Heist", ManageRobberyAnims.CurrentPay, 0, "The Player has died", 2, 0f, 5);
				EndHeist();
			}
		}
	}
}
