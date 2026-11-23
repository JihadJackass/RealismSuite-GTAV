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
	internal class The247Job : Script
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
			EnterStore,
			HoldUpCleark,
			TakeOutCameras,
			OpenTheSafe,
			EscapeTheCops
		}

		public static int HeistMinPay = 16000;

		public static int HeistMaxPay = 62000;

		public static List<ObjectPlacementMap> SafeCodes = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_code_01_23_45"), 504149, 4221637939u, new Vector3(290.0316f, -432.176666f, 43.074894f), new Vector3(0f, 0f, 82.39746f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_code_02_12_87"), 729428, 1433270535u, new Vector3(290.0316f, -432.176666f, 43.074894f), new Vector3(0f, 0f, 82.39746f), 504149, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_code_05_02_91"), 319793, 944906360u, new Vector3(290.0316f, -432.176666f, 43.074894f), new Vector3(0f, 0f, 82.39746f), 504149, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_code_24_10_81"), 209234, 3046060548u, new Vector3(290.0316f, -432.176666f, 43.074894f), new Vector3(0f, 0f, 82.39746f), 504149, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_code_28_03_98"), 359508, 1626709912u, new Vector3(290.0316f, -432.176666f, 43.074894f), new Vector3(0f, 0f, 82.39746f), 504149, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_code_28_11_97"), 605278, 921471402u, new Vector3(290.0316f, -432.176666f, 43.074894f), new Vector3(0f, 0f, 82.39746f), 504149, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_code_44_23_37"), 171833, 3648550039u, new Vector3(290.0316f, -432.176666f, 43.074894f), new Vector3(0f, 0f, 82.39746f), 504149, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_code_72_68_83"), 461405, 4136820571u, new Vector3(290.0316f, -432.176666f, 43.074894f), new Vector3(0f, 0f, 82.39746f), 504149, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_code_73_27_38"), 763232, 1083248297u, new Vector3(290.0316f, -432.176666f, 43.074894f), new Vector3(0f, 0f, 82.39746f), 504149, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_code_77_79_73"), 317271, 2104921722u, new Vector3(290.0316f, -432.176666f, 43.074894f), new Vector3(0f, 0f, 82.39746f), 504149, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<MissionOjects> _247Robberies = new List<MissionOjects>
		{
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Shopkeeper (Male)"), 317955, 416176080u, new Vector3(24.0962944f, -1345.61829f, 29.4818668f), new Vector3(0f, -0f, -89.99999f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 35350, 168901740u, new Vector3(23.90182f, -1342.3551f, 32.02748f), new Vector3(0f, 0f, 44.9999352f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 301346, 168901740u, new Vector3(34.4217072f, -1348.766f, 32.02748f), new Vector3(0f, -0f, -135.000061f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 390163, 168901740u, new Vector3(31.3480721f, -1341.42615f, 32.02748f), new Vector3(0f, -0f, -135.000061f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_safe_01a"), 357668, 1098122770u, new Vector3(31.034914f, -1339.976f, 28.4970322f), new Vector3(0f, -0f, -90.12318f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Security Guard"), 254844, 3613962792u, new Vector3(33.6763153f, -1343.518f, 29.5017147f), new Vector3(0f, -0f, 108.401405f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_code_01_23_45"), 691249, 4221637939u, new Vector3(28.7505264f, -1338.86011f, 29.3002548f), new Vector3(0f, 0f, 19.507164f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Shopkeeper (Male)"), 143203, 416176080u, new Vector3(372.610138f, 328.1533f, 103.551147f), new Vector3(0f, -0f, -104.114983f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 259112, 168901740u, new Vector3(373.21698f, 331.364532f, 106.096855f), new Vector3(0f, 0f, 30.8846283f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 191009, 168901740u, new Vector3(381.8558f, 322.5817f, 106.096855f), new Vector3(0f, -0f, -149.115372f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 707866, 168901740u, new Vector3(380.664978f, 330.449524f, 106.096855f), new Vector3(0f, -0f, -149.115372f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_safe_01a"), 213290, 1098122770u, new Vector3(380.690521f, 331.96814f, 102.566414f), new Vector3(0f, -0f, -105.573532f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Security Guard"), 352052, 3613962792u, new Vector3(379.75946f, 332.307953f, 103.560753f), new Vector3(0f, 0f, 82.3083344f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_code_01_23_45"), 325419, 4221637939u, new Vector3(373.077057f, 325.14035f, 103.565659f), new Vector3(0f, 0f, 85.43232f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Shopkeeper (Male)"), 65297, 416176080u, new Vector3(-3040.46f, 583.660339f, 7.90391064f), new Vector3(0f, -0f, 17.759f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 352864, 168901740u, new Vector3(-3043.51367f, 582.4797f, 10.4393787f), new Vector3(0f, 0f, 152.75914f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 323113, 168901740u, new Vector3(-3040.617f, 594.4537f, 10.4393787f), new Vector3(0f, -0f, -27.2408562f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 224554, 168901740u, new Vector3(-3046.66968f, 589.2877f, 10.4393787f), new Vector3(0f, -0f, -27.2408562f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_safe_01a"), 539494, 1098122770u, new Vector3(-3047.90747f, 588.488f, 6.90893173f), new Vector3(0f, 0f, 16.526247f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Security Guard"), 155424, 3613962792u, new Vector3(-3041.27881f, 592.9496f, 7.903351f), new Vector3(0f, -0f, -169.198578f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_code_01_23_45"), 628752, 4221637939u, new Vector3(-3044.56079f, 586.2873f, 7.90817451f), new Vector3(0f, -0f, 103.642914f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Shopkeeper (Male)"), 676219, 416176080u, new Vector3(-3243.98145f, 999.7551f, 12.8253269f), new Vector3(0f, -0f, -4.9260006f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 474686, 168901740u, new Vector3(-3247.25269f, 999.840942f, 15.3611584f), new Vector3(0f, -0f, 130.0744f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 181114, 168901740u, new Vector3(-3239.96216f, 1009.77155f, 15.3611584f), new Vector3(-0f, -0f, -49.9255867f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 98869, 168901740u, new Vector3(-3247.53882f, 1007.33948f, 15.3611584f), new Vector3(-0f, -0f, -49.9255867f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_safe_01a"), 140591, 1098122770u, new Vector3(-3248.97974f, 1007.24359f, 11.8307114f), new Vector3(0f, 0f, -4.953893f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Security Guard"), 142387, 3613962792u, new Vector3(-3246.39282f, 1004.82874f, 12.8261271f), new Vector3(0f, -0f, -91.65054f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_code_01_23_45"), 73256, 4221637939u, new Vector3(-3244.87085f, 1000.59705f, 12.8299522f), new Vector3(0f, 0f, -21.5861969f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Shopkeeper (Male)"), 773148, 416176080u, new Vector3(549.6566f, 2669.71338f, 42.1519356f), new Vector3(0f, -0f, 97.49999f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 497785, 168901740u, new Vector3(550.275635f, 2666.50049f, 44.6869621f), new Vector3(-0f, -0f, -127.500076f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 337696, 168901740u, new Vector3(539.009f, 2671.4834f, 44.6869621f), new Vector3(0f, -0f, 52.4999123f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 566560, 168901740u, new Vector3(543.0144f, 2664.60742f, 44.6869621f), new Vector3(0f, -0f, 52.4999123f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_safe_01a"), 432511, 1098122770u, new Vector3(543.500366f, 2663.18945f, 41.1565132f), new Vector3(0f, -0f, 96.49102f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Security Guard"), 281427, 3613962792u, new Vector3(549.6496f, 2663.092f, 42.17226f), new Vector3(0f, 0f, 7.446648f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_code_01_23_45"), 702576, 4221637939u, new Vector3(545.279846f, 2662.358f, 41.9597321f), new Vector3(0f, -0f, 177.567947f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Shopkeeper (Male)"), 579922, 416176080u, new Vector3(1958.91589f, 3741.28271f, 32.3284836f), new Vector3(0f, -0f, -60f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 694088, 168901740u, new Vector3(1957.11951f, 3744.01855f, 34.87421f), new Vector3(0f, 0f, 74.99992f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 78668, 168901740u, new Vector3(1969.43542f, 3743.72656f, 34.87421f), new Vector3(0f, -0f, -105.000069f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 781154, 168901740u, new Vector3(1963.10364f, 3748.54614f, 34.87421f), new Vector3(0f, -0f, -105.000069f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_safe_01a"), 785985, 1098122770u, new Vector3(1962.16821f, 3749.71069f, 31.3437614f), new Vector3(0f, 0f, -60.3508873f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Security Guard"), 220272, 3613962792u, new Vector3(1961.81152f, 3740.03247f, 32.34892f), new Vector3(-0f, -0f, -5.89348269f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_code_01_23_45"), 392266, 4221637939u, new Vector3(1955.468f, 3746.62427f, 32.0686f), new Vector3(0f, -0f, 99.5895f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Shopkeeper (Male)"), 196941, 416176080u, new Vector3(2676.37866f, 3279.903f, 55.23659f), new Vector3(0f, -0f, -29.130003f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 462166, 168901740u, new Vector3(2673.43359f, 3281.32324f, 57.7716f), new Vector3(0f, -0f, 105.869995f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 145749, 168901740u, new Vector3(2684.15479f, 3287.3916f, 57.7716f), new Vector3(-0f, -0f, -74.13f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 353876, 168901740u, new Vector3(2676.247f, 3288.27979f, 57.7716f), new Vector3(-0f, -0f, -74.13f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_safe_01a"), 630638, 1098122770u, new Vector3(2674.95557f, 3288.79761f, 54.24115f), new Vector3(0f, 0f, -29.0305157f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Security Guard"), 740711, 3613962792u, new Vector3(2674.56934f, 3288.176f, 55.2453f), new Vector3(0f, -0f, 153.543091f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_code_01_23_45"), 540726, 4221637939u, new Vector3(2674.91431f, 3280.43433f, 54.9659843f), new Vector3(0f, -0f, 176.315811f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Shopkeeper (Male)"), 442743, 416176080u, new Vector3(1728.27515f, 6416.90527f, 35.02234f), new Vector3(0f, -0f, -116.359f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 414312, 168901740u, new Vector3(1729.54822f, 6419.91553f, 37.56769f), new Vector3(0f, 0f, 18.640749f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 64097, 168901740u, new Vector3(1736.12793f, 6409.50049f, 37.56769f), new Vector3(0f, -0f, -161.359238f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 394581, 168901740u, new Vector3(1736.63281f, 6417.442f, 37.56769f), new Vector3(0f, -0f, -161.359238f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_safe_01a"), 665462, 1098122770u, new Vector3(1737.10815f, 6418.75049f, 34.0372429f), new Vector3(0f, -0f, -116.447807f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Security Guard"), 563562, 3613962792u, new Vector3(1735.70178f, 6411.064f, 35.0416222f), new Vector3(0f, 0f, 69.02489f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_code_01_23_45"), 148484, 4221637939u, new Vector3(1734.7345f, 6421.33154f, 34.9035263f), new Vector3(0f, 0f, -53.0859642f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Shopkeeper (Male)"), 135033, 416176080u, new Vector3(2555.50854f, 380.511658f, 108.617722f), new Vector3(0f, -0f, -2.28402877f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 266034, 168901740u, new Vector3(2552.23633f, 380.446381f, 111.153427f), new Vector3(0f, -0f, 132.715927f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 259960, 168901740u, new Vector3(2559.06128f, 390.702423f, 111.153427f), new Vector3(-0f, -0f, -47.28407f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"), 373565, 168901740u, new Vector3(2551.60474f, 387.923737f, 111.153427f), new Vector3(-0f, -0f, -47.28407f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_safe_01a"), 16759, 1098122770u, new Vector3(2550.19653f, 387.799774f, 107.622986f), new Vector3(0f, 0f, -2.89401555f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Security Guard"), 461941, 3613962792u, new Vector3(2552.48853f, 380.977478f, 108.61882f), new Vector3(0.2447072f, 0.08756178f, -26.7423477f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_code_01_23_45"), 91140, 4221637939u, new Vector3(2558.92017f, 389.023926f, 108.600731f), new Vector3(0f, 0f, -49.6220932f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			})
		};

		public static HeistStage CurrentHeistStage = HeistStage.EnterStore;

		public static int LocalTake = 0;

		public static int Num1;

		public static int Num2;

		public static int Num3;

		public static MissionOjects CurrentStore;

		public static List<Entity> Cameras = new List<Entity>();

		public static List<Entity> SpawnedEntities = new List<Entity>();

		public static bool _247RobberyOn;

		public static Blip TargetBlip;

		public static List<Blip> Storeblip = new List<Blip>();

		public static int CodeNumber1;

		public static int CodeNumber2;

		public static int CodeNumber3;

		public static void ATTACH_ENTITY_TO_ENTITY(Entity entity1, Entity entity2, int boneIndex, float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, bool p9, bool useSoftPinning, bool collision, bool isPed, int vertexIndex, bool fixedRot)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, entity1, entity2, boneIndex, xPos, yPos, zPos, xRot, yRot, zRot, p9, useSoftPinning, collision, isPed, vertexIndex, fixedRot);
		}

		public The247Job()
		{
			base.Tick += onTick;
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

		public void onTick(object sender, EventArgs e)
		{
			if (!_247RobberyOn || !_247RobberyOn)
			{
				return;
			}
			switch (CurrentHeistStage)
			{
			case HeistStage.EnterStore:
			{
				Ped[] nearbyPeds = World.GetNearbyPeds(CurrentStore.Entities[0].Entity.Position, 3f, 416176080);
				foreach (Ped ped in nearbyPeds)
				{
					if (ped != null && !SpawnedEntities.Contains(ped))
					{
						ped.Delete();
					}
				}
				for (int j = 1; j < CurrentStore.Entities.Count; j++)
				{
					Prop[] nearbyProps = World.GetNearbyProps(CurrentStore.Entities[j].Position, 3f, CurrentStore.Entities[j].Hash);
					foreach (Prop prop in nearbyProps)
					{
						if (prop != null && !SpawnedEntities.Contains(prop))
						{
							prop.Delete();
						}
					}
				}
				if (TargetBlip != null)
				{
					TargetBlip.ShowRoute = true;
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, CurrentStore.Entities[0].Position, "<", 20f))
				{
					Screen.ShowSubtitle("Enter the ~y~24/7 Store");
				}
				else
				{
					Screen.ShowSubtitle("Head to the ~y~24/7 Store");
				}
				if (!GetDistance_VDIST2(Game.Player.Character.Position, CurrentStore.Entities[0].Position, "<", 10f))
				{
					break;
				}
				Ped p = (Ped)CurrentStore.Entities[5].Entity;
				SmartGuards.GuardGroup[13].GroupList.Add(new SmartGuards.SmartAiV2(p, SmartGuards.EnemyGroup.Security));
				foreach (ObjectPlacementMap entity in CurrentStore.Entities)
				{
					if (entity.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "prop_cctv_cam_06a"))
					{
						Cameras.Add(entity.Entity);
					}
					if (entity.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_safe_01a"))
					{
						ManageRobberyAnims.AddPropAsLoot((Prop)entity.Entity);
					}
				}
				foreach (Entity camera in Cameras)
				{
					if (camera != null)
					{
						camera.Opacity = 255;
						camera.AddBlip();
						camera.AttachedBlip.Sprite = BlipSprite.Standard;
						camera.AttachedBlip.Color = BlipColor.Red2;
						camera.AttachedBlip.Name = "Camera";
						camera.AttachedBlip.Scale = 0.5f;
					}
				}
				CurrentHeistStage = HeistStage.HoldUpCleark;
				break;
			}
			case HeistStage.HoldUpCleark:
			{
				Game.Player.WantedLevel = 0;
				SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Security;
				Ped[] nearbyPeds2 = World.GetNearbyPeds(CurrentStore.Entities[0].Entity.Position, 3f, 416176080);
				foreach (Ped ped2 in nearbyPeds2)
				{
					if (ped2 != null && !SpawnedEntities.Contains(ped2))
					{
						ped2.Delete();
					}
				}
				for (int m = 1; m < CurrentStore.Entities.Count; m++)
				{
					Prop[] nearbyProps2 = World.GetNearbyProps(CurrentStore.Entities[m].Position, 3f, CurrentStore.Entities[m].Hash);
					foreach (Prop prop2 in nearbyProps2)
					{
						if (prop2 != null && !SpawnedEntities.Contains(prop2))
						{
							prop2.Delete();
						}
					}
				}
				if (TargetBlip != null)
				{
					TargetBlip.ShowRoute = true;
				}
				if (TargetBlip != null)
				{
					TargetBlip.ShowRoute = false;
				}
				Screen.ShowSubtitle("Hold Up the ~y~Cleark");
				Ped ped3 = (Ped)CurrentStore.Entities[0].Entity;
				ped3.BlockPermanentEvents = true;
				ped3.RelationshipGroup = 2;
				if (Game.Player.IsTargeting(ped3) && !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, ped3, "mp_am_hold_up", "handsup_enter", 3))
				{
					Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 2);
					ped3.Task.PlayAnimation("mp_am_hold_up", "handsup_enter", 1f, -1, AnimationFlags.None);
					CurrentHeistStage = HeistStage.TakeOutCameras;
				}
				break;
			}
			case HeistStage.TakeOutCameras:
			{
				Game.Player.WantedLevel = 0;
				SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Security;
				Ped ped4 = (Ped)CurrentStore.Entities[0].Entity;
				ped4.BlockPermanentEvents = true;
				ped4.RelationshipGroup = 2;
				if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, ped4, "mp_am_hold_up", "handsup_enter", 3) && !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, ped4, "mp_am_hold_up", "handsup_base", 3))
				{
					Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 3);
					ped4.Task.PlayAnimation("mp_am_hold_up", "handsup_base", 1f, -1, AnimationFlags.Loop);
				}
				Screen.ShowSubtitle("Take out the ~r~Cameras");
				bool flag = true;
				foreach (Entity camera2 in Cameras)
				{
					if (camera2.Health <= 996 && camera2.AttachedBlip != null)
					{
						camera2.AttachedBlip.Alpha = 0;
					}
					if (camera2.Health >= 997)
					{
						flag = false;
						if (camera2.AttachedBlip != null)
						{
							camera2.AttachedBlip.Alpha = 255;
						}
					}
				}
				if (flag)
				{
					Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 4);
					CurrentHeistStage = HeistStage.OpenTheSafe;
				}
				break;
			}
			case HeistStage.OpenTheSafe:
				ManageRobberyAnims.CodeNumber1 = CodeNumber1;
				ManageRobberyAnims.CodeNumber2 = CodeNumber2;
				ManageRobberyAnims.CodeNumber3 = CodeNumber3;
				Game.Player.WantedLevel = 0;
				SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Security;
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
				Screen.ShowSubtitle("Attempt to Open ~y~Safe~w~ or find the ~p~Safe Codes~w~");
				if (ManageRobberyAnims.CurrentPay >= 1500)
				{
					Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
					if (TargetBlip != null)
					{
						TargetBlip.Delete();
					}
					Game.Player.WantedLevel = 3;
					CurrentHeistStage = HeistStage.EscapeTheCops;
				}
				break;
			case HeistStage.EscapeTheCops:
			{
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
					break;
				}
				Main.TotalCrewEarned += ManageRobberyAnims.CurrentPay;
				Main.TotalPlayerEarned += 0f;
				XP.CompleteHeist_The247Job++;
				Main.SaveCuts();
				Class1.SetNoLongerInHeist();
				MissionScreen.SetPass("The 24/7 Robbery", ManageRobberyAnims.CurrentPay, 0, "The Player has died", 2, 0f, 5);
				int num = Game.GameTime + 30000;
				while (HKHModHelperNew.Class1.HeistPassedScreen != -1 && Game.GameTime <= num)
				{
					Script.Wait(0);
				}
				Script.Wait(2000);
				ManagePlayerFunds.AddFunds(ManageRobberyAnims.CurrentPay);
				XP.AddXP_Percent(XP.CompleteHeist_XPPercent_The247Job);
				EndHeist();
				break;
			}
			}
			bool isAlive = Game.Player.IsAlive;
		}

		public static void EndHeist()
		{
			SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.None;
			foreach (Blip item in Storeblip)
			{
				if (item != null)
				{
					item.Delete();
				}
			}
			foreach (ObjectPlacementMap safeCode in SafeCodes)
			{
				if (safeCode.Entity != null)
				{
					safeCode.Entity.Delete();
				}
			}
			Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
			ManageRobberyAnims.OverridePay = false;
			ManageRobberyAnims.UseSetSafeCode = false;
			LocalTake = 0;
			if (TargetBlip != null)
			{
				TargetBlip.Delete();
			}
			ManageRobberyAnims.CurrentPay = 0;
			_247RobberyOn = false;
			foreach (MissionOjects _247Robbery in _247Robberies)
			{
				foreach (ObjectPlacementMap entity in _247Robbery.Entities)
				{
					if (entity.Entity != null)
					{
						entity.Entity.Delete();
					}
				}
			}
			CurrentHeistStage = HeistStage.EnterStore;
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

		public static void GetClosestStore()
		{
			float radius = 9999f;
			foreach (MissionOjects _247Robbery in _247Robberies)
			{
				if (GetDistance_VDIST2(_247Robbery.Entities[0].Position, Game.Player.Character.Position, "<", radius))
				{
					radius = GetDistance(_247Robbery.Entities[0].Position, Game.Player.Character.Position);
					CurrentStore = _247Robbery;
				}
			}
		}

		public static void GetRandomStore()
		{
			CurrentStore = _247Robberies[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, _247Robberies.Count)];
		}

		public static void StartHeist()
		{
			ManageRobberyAnims.CurrentPay = 0;
			ManageRobberyAnims.Heist_Average_Pay = (float)(HeistMinPay + HeistMaxPay) / 2f * 1.35f;
			EndHeist();
			CurrentHeistStage = HeistStage.EnterStore;
			GetRandomStore();
			int num = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, SafeCodes.Count);
			CurrentStore.Entities[6].Hash = SafeCodes[num].Hash;
			CurrentStore.Entities[6].Inputtedhash = SafeCodes[num].Inputtedhash;
			CurrentStore.CreateEntites();
			if (num == 0)
			{
				CodeNumber1 = 1;
				CodeNumber2 = 23;
				CodeNumber3 = 45;
			}
			if (num == 1)
			{
				CodeNumber1 = 2;
				CodeNumber2 = 12;
				CodeNumber3 = 87;
			}
			if (num == 2)
			{
				CodeNumber1 = 5;
				CodeNumber2 = 2;
				CodeNumber3 = 91;
			}
			if (num == 3)
			{
				CodeNumber1 = 24;
				CodeNumber2 = 10;
				CodeNumber3 = 81;
			}
			if (num == 4)
			{
				CodeNumber1 = 28;
				CodeNumber2 = 3;
				CodeNumber3 = 98;
			}
			if (num == 5)
			{
				CodeNumber1 = 28;
				CodeNumber2 = 11;
				CodeNumber3 = 97;
			}
			if (num == 6)
			{
				CodeNumber1 = 44;
				CodeNumber2 = 23;
				CodeNumber3 = 37;
			}
			if (num == 8)
			{
				CodeNumber1 = 72;
				CodeNumber2 = 58;
				CodeNumber3 = 83;
			}
			if (num == 9)
			{
				CodeNumber1 = 73;
				CodeNumber2 = 27;
				CodeNumber3 = 38;
			}
			if (num == 10)
			{
				CodeNumber1 = 77;
				CodeNumber2 = 79;
				CodeNumber3 = 73;
			}
			Notification.Show("SafeCode " + CodeNumber1 + "/" + CodeNumber2 + "/" + CodeNumber3);
			foreach (ObjectPlacementMap entity in CurrentStore.Entities)
			{
				SpawnedEntities.Add(entity.Entity);
			}
			TargetBlip = GlobalHelper.CreateBlip(CurrentStore.Entities[0].Position);
			TargetBlip.Sprite = BlipSprite.Store;
			TargetBlip.Color = BlipColor.Yellow;
			TargetBlip.Name = "Target 24/7 Store";
			TargetBlip.IsShortRange = true;
			_247RobberyOn = true;
			ManageRobberyAnims.OverridePay = true;
		}
	}
}
