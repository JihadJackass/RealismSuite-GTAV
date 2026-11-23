using System;
using System.Collections.Generic;
using System.Drawing;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using HKHModHelperNew;
using LemonUI;
using LemonUI.Elements;
using SinglePlayerHeists.UtilsMisc;
using SmartAIGuards;

namespace SinglePlayerHeists.Freeform
{
	internal class TheBillionaresParty : Script
	{
		public enum ActiveGroup
		{
			None,
			TheBillionariesParty_MansionGuards,
			TheBillionariesParty_HeadofSecurity,
			TheBillionariesParty_Penthouse,
			TheBillionariesParty_Cartel
		}

		public enum Nodetype
		{
			AnyRoad,
			Road,
			Offroad,
			Water
		}

		public class VectorRotation : Script
		{
			public Vector3 Position { get; set; }

			public Vector3 Rotation { get; set; }

			public float Heading { get; set; }

			public VectorRotation()
			{
			}

			public VectorRotation(Vector3 P, Vector3 R)
			{
				Position = P;
				Rotation = R;
			}

			public VectorRotation(Vector3 P, float H)
			{
				Position = P;
				Heading = H;
			}
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
			EnteringParty,
			Scout,
			EnterHoSoffice,
			VaultKeycard,
			HackHoSPhone,
			DJAccessPass,
			VipAccessPass,
			HackComBox,
			AccessingCameras,
			PhantomEscaping,
			CannotHackKeypad,
			HackKeypad,
			AwaitVaultOpen,
			VaultOpenLoot,
			DrivingPhantom,
			DrivingTargetVeh,
			TargetVehsEscaping,
			PartyMain,
			NoWayIntoParty,
			CartelPrePurchase,
			CartelAfterPurchase,
			SetupRoadblock,
			NoVehiclesToDeliver,
			DeliverVehicleToParkingSpace
		}

		public static ActiveGroup CurrentActiveGroup = ActiveGroup.None;

		public List<string> DanceAnimations = new List<string>();

		private static float OffsetRotX = 0f;

		private static float OffsetRotY = 0f;

		private static float OffsetRotZ = 0f;

		private static float FreeCamSpeed = 5f;

		private static bool Lock = false;

		public static float DefaultSpeed = 1.5f;

		public static float ShiftSpeed = 5f;

		public static float Precision = 1f;

		public static bool FreezeTime;

		public static List<VectorRotation> CamerasPosR = new List<VectorRotation>();

		public static float screenaspectratio;

		public static Camera SecCam;

		public static bool GotGrinder;

		public static bool GotC4 = false;

		public static int C4ChargesCollected = 20;

		public static bool GotThermite = true;

		public static int ThermiteChargesCollected = 20;

		public static bool GotKeycard;

		public static int HeistMinPay = 450000;

		public static int HeistMaxPay = 900000;

		public static int CurrentPay = 0;

		public static int LocalTake = 0;

		public static bool HackingKeypad = false;

		public static Prop LastKeypad;

		public static Prop USB;

		public static Prop Phone;

		public int Scene1;

		public static List<SmartGuards.SmartAiV2> CartelGuards = new List<SmartGuards.SmartAiV2>();

		public static List<SmartGuards.SmartAiV2> MansionGuards = new List<SmartGuards.SmartAiV2>();

		public static List<SmartGuards.SmartAiV2> HosGuards = new List<SmartGuards.SmartAiV2>();

		public static List<SmartGuards.SmartAiV2> PenthouseGuards = new List<SmartGuards.SmartAiV2>();

		public static List<int> TargetVehiclePrices = new List<int>
		{
			276500, 225000, 110000, 276500, 282000, 192500, 144000, 153500, 251500, 289000,
			232500
		};

		public static MissionOjects TargetVehicles = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Ignus"), 457550, 2850852987u, new Vector3(-1541.04f, 125.020088f, 56.08081f), new Vector3(-0.6239403f, -0.05956087f, 171.991577f), 9, 27, 62, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Visione"), 359000, 3296789504u, new Vector3(-1560.50684f, 123.213356f, 56.5768852f), new Vector3(-2.282622f, 3.04150486f, -63.8312645f), 34, 1, 158, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Super Diamond"), 358223, 1123216662u, new Vector3(-1557.63074f, 118.561951f, 56.38016f), new Vector3(-1.412437f, -6.014324f, -149.760437f), 1, 1, 7, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Ignus"), 24661, 2850852987u, new Vector3(-1561.634f, 114.77758f, 56.3154449f), new Vector3(-4.12476969f, -2.01226163f, -128.454453f), 117, 55, 73, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Zeno"), 734035, 655665811u, new Vector3(-1555.44788f, 110.252846f, 56.0686646f), new Vector3(-0.5356336f, -0.159231216f, -112.986237f), 0, 1, 2, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Zorrusso"), 585221, 3612858749u, new Vector3(-1546.147f, 115.213882f, 56.3103561f), new Vector3(0.203104347f, 0.6422417f, -165.535889f), 88, 112, 134, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Nero"), 251145, 1034187331u, new Vector3(-1545.97693f, 108.80452f, 56.3674355f), new Vector3(-0.0622807629f, -0.0002024328f, -68.69216f), 88, 1, 18, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Vagner"), 37890, 1939284556u, new Vector3(-1555.944f, 124.6771f, 56.3647957f), new Vector3(-1.42712283f, 0.7448842f, -32.3386726f), 38, 8, 5, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Tyrant"), 14883, 3918533058u, new Vector3(-1587.02283f, 129.024689f, 58.3689423f), new Vector3(-1.66811359f, 3.30524f, 15.371068f), 112, 126, 78, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Torero XO"), 665121, 4129572538u, new Vector3(-1586.9425f, 135.820511f, 58.3436966f), new Vector3(-0.508941f, -0.0209312644f, -10.0879784f), 38, 1, 89, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Thrax"), 21283, 1044193113u, new Vector3(-1584.79724f, 141.695313f, 58.2257042f), new Vector3(-0.989059269f, -0.06407263f, -27.4898129f), 27, 33, 18, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects TargetVehicleEscapePositions = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Ignus"), 476782, 2850852987u, new Vector3(-1590.281f, 122.426735f, 59.0005264f), new Vector3(5.51584339f, 0.299138963f, 91.04703f), 9, 27, 62, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Ignus"), 783982, 2850852987u, new Vector3(-1584.30688f, 122.198204f, 58.42272f), new Vector3(5.365566f, 0.470900744f, 87.14888f), 59, 88, 94, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Ignus"), 212336, 2850852987u, new Vector3(-1579.24377f, 122.049118f, 57.95907f), new Vector3(5.00295973f, 0.388072222f, 86.47986f), 117, 55, 73, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Ignus"), 69740, 2850852987u, new Vector3(-1573.91162f, 121.675056f, 57.5863f), new Vector3(3.59161472f, 0.198802456f, 86.8450241f), 9, 27, 62, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects TargetVehicleParked = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Ignus"), 30248, 2850852987u, new Vector3(1292.08923f, -3232.55273f, 5.27546f), new Vector3(0.269311339f, -0.0547415875f, -90.53796f), 117, 55, 73, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Ignus"), 665388, 2850852987u, new Vector3(1291.83154f, -3236.48853f, 5.274806f), new Vector3(0.248825431f, -0.0005486747f, -90.52422f), 9, 27, 62, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Ignus"), 765740, 2850852987u, new Vector3(1291.607f, -3241.045f, 5.269638f), new Vector3(0.03912201f, -0.003763801f, -89.92566f), 59, 88, 94, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Ignus"), 333101, 2850852987u, new Vector3(1291.63159f, -3245.39038f, 5.26955271f), new Vector3(0.0445963964f, 0.00362870912f, -90.53333f), 117, 55, 73, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static Vehicle TargetVehicleA;

		public static Ped TargetVehicleADriver;

		public static bool AttachedVehicleAtoTrailer;

		public static bool TargetVehicleA_NoLongerAvailable;

		public static bool TargetVehicleA_Delivered;

		public static Vehicle TargetVehicleB;

		public static Ped TargetVehicleBDriver;

		public static bool AttachedVehicleBtoTrailer;

		public static bool TargetVehicleB_NoLongerAvailable;

		public static bool TargetVehicleB_Delivered;

		public static Vehicle TargetVehicleC;

		public static Ped TargetVehicleCDriver;

		public static bool AttachedVehicleCtoTrailer;

		public static bool TargetVehicleC_NoLongerAvailable;

		public static bool TargetVehicleC_Delivered;

		public static Vehicle TargetVehicleD;

		public static Ped TargetVehicleDDriver;

		public static bool AttachedVehicleDtoTrailer;

		public static bool TargetVehicleD_NoLongerAvailable;

		public static bool TargetVehicleD_Delivered;

		public static bool PhantomDelivered;

		public static bool PhantomTaken;

		public static MissionOjects MansionSpeakerObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sf_prop_sf_dj_desk_01a"), 334890, 4134478150u, new Vector3(-1560.43f, 108.28f, 55.77009f), new Vector3(0f, -0f, 139.999985f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_dj_set_wbeach"), 172844, 1222991438u, new Vector3(-1560.38916f, 108.16761f, 56.5793381f), new Vector3(0f, 0f, -41.5409126f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_large"), 10031, 3473132757u, new Vector3(-1559.73157f, 105.111366f, 55.7814827f), new Vector3(0f, -0f, 150.891281f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_large"), 440109, 3473132757u, new Vector3(-1563.63757f, 108.925323f, 55.7814522f), new Vector3(0f, -0f, 118.083611f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_dj"), 197924, 2595533326u, new Vector3(-1537.58093f, 119.246605f, 55.7801132f), new Vector3(0f, 0f, -41.6449051f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_large"), 325421, 3473132757u, new Vector3(-1543.54248f, 106.203674f, 55.7819138f), new Vector3(0f, -0f, -134.74411f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_large"), 757806, 3473132757u, new Vector3(-1538.95313f, 110.673782f, 55.781723f), new Vector3(0f, -0f, -91.90503f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_large"), 124192, 3473132757u, new Vector3(-1570.76233f, 118.9077f, 57.1486168f), new Vector3(0f, -0f, -128.755035f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_large"), 656679, 3473132757u, new Vector3(-1572.23853f, 127.86438f, 57.3696136f), new Vector3(0f, 0f, -24.08946f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_med"), 558121, 540206593u, new Vector3(-1583.09375f, 131.886887f, 57.7933846f), new Vector3(0f, 0f, -24.0382442f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_med"), 146994, 540206593u, new Vector3(-1580.9436f, 130.935959f, 57.710556f), new Vector3(0f, 0f, -25.26649f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_small"), 186672, 1040358431u, new Vector3(-1553.37268f, 107.530975f, 55.7805023f), new Vector3(0f, -0f, -159.106659f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_small"), 282675, 1040358431u, new Vector3(-1547.73755f, 116.0703f, 55.780735f), new Vector3(0f, 0f, 20.948822f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_small"), 618795, 1040358431u, new Vector3(-1553.9176f, 117.257988f, 55.780735f), new Vector3(0f, 0f, -62.8856926f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_small"), 722736, 1040358431u, new Vector3(-1560.8634f, 127.411636f, 56.0088959f), new Vector3(0f, 0f, 32.51601f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_med"), 77875, 540206593u, new Vector3(-1560.35229f, 127.837807f, 55.9527321f), new Vector3(0f, 0f, 32.51601f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_small"), 532267, 1040358431u, new Vector3(-1559.89282f, 128.151352f, 55.90287f), new Vector3(0f, 0f, 54.5748062f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_large"), 711984, 3473132757u, new Vector3(-1553.16223f, 135.330475f, 55.7822876f), new Vector3(0f, 0f, 20.69277f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_med"), 687920, 540206593u, new Vector3(-1553.8717f, 134.909454f, 55.7822876f), new Vector3(0f, 0f, 23.8147945f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_med"), 634927, 540206593u, new Vector3(-1552.33984f, 135.468277f, 55.7822876f), new Vector3(0f, 0f, 16.7005939f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_large"), 529196, 3473132757u, new Vector3(-1538.74377f, 130.814133f, 56.3733559f), new Vector3(0f, 0f, -27.4173412f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_large"), 550692, 3473132757u, new Vector3(-1536.4718f, 128.672638f, 56.3733559f), new Vector3(0f, 0f, -80.6454239f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_large"), 300592, 3473132757u, new Vector3(-1579.51f, 142.907532f, 57.47143f), new Vector3(0f, -0f, -111.761971f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_med"), 226604, 540206593u, new Vector3(-1579.79443f, 142.310532f, 57.501915f), new Vector3(0f, -0f, -111.761971f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_large"), 644660, 3473132757u, new Vector3(-1580.07922f, 141.713409f, 57.53247f), new Vector3(0f, -0f, -111.761971f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_large"), 161844, 3473132757u, new Vector3(-1561.32373f, 111.746109f, 55.78145f), new Vector3(0f, -0f, 101.450417f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_large"), 8755, 3473132757u, new Vector3(-1548.06372f, 100.752396f, 59.3660851f), new Vector3(0f, -0f, -133.15506f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects MansionObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "phantom3"), 551255, Function.Call<uint>(Hash.GET_HASH_KEY, "phantom3"), new Vector3(-1498.903f, 86.49f, 55.28f), new Vector3(0f, 0f, -94.3f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "TR2"), 528210, Function.Call<uint>(Hash.GET_HASH_KEY, "TR2"), new Vector3(-1508.47f, 86.4f, 55.91f), new Vector3(0f, 0f, -87.2f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Schafter LWB (Armored)"), 551255, 1922255844u, new Vector3(-1601.47009f, 115.661537f, 60.2117043f), new Vector3(3.70356655f, 1.79440546f, 112.937019f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Schafter LWB (Armored)"), 528210, 1922255844u, new Vector3(-1593.58093f, 116.768753f, 59.75909f), new Vector3(1.72564375f, 5.50487328f, 73.71756f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 10834, 1744231373u, new Vector3(-1584.54468f, 131.432251f, 58.9270668f), new Vector3(0f, 0f, 52.1289444f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 3"), 582999, 3572886207u, new Vector3(-1584.29749f, 129.339966f, 58.98546f), new Vector3(0f, -0f, 31.7678261f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 2"), 12377, 357447289u, new Vector3(-1585.09741f, 131.1549f, 58.9958344f), new Vector3(0f, 0f, 64.50681f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 2"), 472663, 3324988722u, new Vector3(-1578.364f, 126.57679f, 58.54826f), new Vector3(3.09171155E-15f, -7.46486537E-16f, -152.851685f), 966099553u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 2"), 411977, 357447289u, new Vector3(-1578.08777f, 128.154861f, 58.6062355f), new Vector3(0f, -0f, -124.1233f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 683856, 2813792322u, new Vector3(-1574.68567f, 127.702217f, 58.44951f), new Vector3(2.526467E-28f, -2.58596419E-28f, 89.340744f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 3"), 416601, 10751269u, new Vector3(-1575.01672f, 126.768517f, 58.4229355f), new Vector3(1.86370525E-17f, -2.62321951E-17f, -101.860283f), 966099553u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 2"), 194391, 357447289u, new Vector3(-1573.349f, 127.5381f, 58.38536f), new Vector3(0f, -0f, 107.131561f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 513109, 1744231373u, new Vector3(-1573.46472f, 126.119133f, 58.2907448f), new Vector3(0f, 0f, 35.9893341f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 3"), 198475, 10751269u, new Vector3(-1560.83484f, 119.065529f, 56.9312668f), new Vector3(1.09285671E-25f, 1.792307E-25f, -164.264755f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 2"), 292433, 357447289u, new Vector3(-1562.49158f, 118.429634f, 57.0732079f), new Vector3(1.77346364E-24f, -2.05833256E-24f, -147.443573f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 3"), 741967, 10751269u, new Vector3(-1561.06726f, 117.267052f, 56.91137f), new Vector3(1.766509E-17f, 6.211854E-18f, 126.939018f), 966099553u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 2"), 716372, 3324988722u, new Vector3(-1559.04663f, 125.428589f, 56.96464f), new Vector3(0f, -0f, -160.006714f), 2725352035u, 195, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Tale of Us 2"), 244569, 1182156569u, new Vector3(-1560.86023f, 107.698563f, 56.7796326f), new Vector3(0f, 0f, -41.9932632f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sf_prop_sf_dj_desk_01a"), 429142, 4134478150u, new Vector3(-1560.43f, 108.28f, 55.77009f), new Vector3(0f, -0f, 139.999985f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 2"), 164441, 357447289u, new Vector3(-1558.77368f, 110.1248f, 56.77866f), new Vector3(0f, 0f, 85.6585f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 420693, 1744231373u, new Vector3(-1559.96045f, 110.9895f, 56.7786026f), new Vector3(0f, -0f, 137.513229f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 208969, 2813792322u, new Vector3(-1561.25366f, 110.600868f, 56.77746f), new Vector3(0f, -0f, -146.062668f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 302169, 1744231373u, new Vector3(-1558.14221f, 111.13932f, 56.7793961f), new Vector3(0f, -0f, 139.121841f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 359761, 2813792322u, new Vector3(-1558.854f, 112.189377f, 56.7794571f), new Vector3(0f, -0f, -158.073074f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 2"), 374868, 3324988722u, new Vector3(-1557.52393f, 113.284172f, 56.778595f), new Vector3(0f, -0f, 93.0758057f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 376151, 2813792322u, new Vector3(-1558.24683f, 114.631523f, 56.81945f), new Vector3(-3.14248419E-15f, 4.90632347E-16f, -162.2522f), 966099553u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 441433, 2813792322u, new Vector3(-1556.8783f, 121.4796f, 56.8440781f), new Vector3(0f, 0f, 43.32958f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 2"), 565843, 3324988722u, new Vector3(-1553.60291f, 127.729385f, 56.7864342f), new Vector3(-2.1479855E-21f, -6.112517E-21f, -100.164192f), 966099553u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 3"), 178776, 3572886207u, new Vector3(-1552.56714f, 125.18309f, 56.7902222f), new Vector3(0f, -0f, 179.019653f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 341842, 2813792322u, new Vector3(-1551.78833f, 124.507912f, 56.7902f), new Vector3(-6.020535E-21f, -1.07029473E-20f, -103.522156f), 966099553u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 2"), 213322, 357447289u, new Vector3(-1551.33459f, 128.935272f, 56.789505f), new Vector3(0f, -0f, -104.689133f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Super Diamond"), 642392, 1123216662u, new Vector3(-1555.01746f, 131.239334f, 56.3215065f), new Vector3(-0.06121108f, 0.00528026046f, -15.9984674f), 1, 1, 7, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Patriot Stretch"), 14681, 3874056184u, new Vector3(-1546.15076f, 125.677048f, 56.51145f), new Vector3(-0.07567058f, 0.180144027f, 67.15393f), 73, 0, 67, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 714841, 2813792322u, new Vector3(-1550.977f, 125.262535f, 56.78652f), new Vector3(0f, -0f, 148.5656f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 2"), 769111, 3324988722u, new Vector3(-1552.27283f, 126.218071f, 56.79076f), new Vector3(0f, -0f, 170.314056f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 286040, 1744231373u, new Vector3(-1553.48f, 124.36351f, 56.799942f), new Vector3(0f, -0f, 150.692f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 78680, 1744231373u, new Vector3(-1542.64722f, 126.63826f, 56.780098f), new Vector3(0f, 0f, 54.00041f), 2725352035u, 199, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "0x76463a5e"), 19030, 1984313950u, new Vector3(-1542.97131f, 121.560722f, 56.778183f), new Vector3(0f, -0f, -125.577682f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 152663, 1744231373u, new Vector3(-1542.10974f, 121.350479f, 56.77946f), new Vector3(4.509267E-17f, -1.1360908E-18f, -164.899292f), 966099553u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 2"), 650840, 3324988722u, new Vector3(-1541.60217f, 120.24054f, 56.7799568f), new Vector3(0f, 0f, 26.0213f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "0x76463a5e"), 504653, 1984313950u, new Vector3(-1542.39172f, 119.7718f, 56.7799568f), new Vector3(0f, 0f, 21.5559731f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 3"), 213836, 10751269u, new Vector3(-1543.11157f, 116.883972f, 56.77993f), new Vector3(0f, -0f, 129.566071f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 58458, 1744231373u, new Vector3(-1544.43274f, 115.873085f, 56.7799339f), new Vector3(8.723102E-25f, -1.50130424E-25f, -46.6252022f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 2"), 9561, 3324988722u, new Vector3(-1542.94324f, 115.658195f, 56.77797f), new Vector3(0f, 0f, 4.67872047f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 3"), 332116, 3572886207u, new Vector3(-1544.031f, 114.165642f, 56.77864f), new Vector3(-6.06093451E-15f, -1.931E-15f, 144.656479f), 966099553u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 118098, 2813792322u, new Vector3(-1545.4646f, 111.381554f, 56.7778549f), new Vector3(0f, 0f, 7.82757235f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 416340, 1744231373u, new Vector3(-1546.86548f, 110.733452f, 56.779026f), new Vector3(0f, 0f, 45.0635719f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "0x44b4d748"), 663379, 1152702280u, new Vector3(-1547.07251f, 112.198578f, 56.7797966f), new Vector3(-0f, -0f, -71.42238f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 3"), 752727, 10751269u, new Vector3(-1547.95569f, 111.718788f, 56.77972f), new Vector3(0f, 0f, 68.8731f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Patriot Stretch"), 13912, 3874056184u, new Vector3(-1549.64392f, 106.154938f, 56.51531f), new Vector3(-0.1576924f, -1.67660964f, 54.2794456f), 111, 0, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "0x44b4d748"), 698969, 1152702280u, new Vector3(-1550.47278f, 112.8183f, 56.7796669f), new Vector3(0f, -0f, -161.383041f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 2"), 423254, 3324988722u, new Vector3(-1551.05627f, 111.235489f, 56.7778664f), new Vector3(0f, -0f, 94.10385f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 2"), 561750, 3324988722u, new Vector3(-1552.61755f, 110.887283f, 56.7802277f), new Vector3(1.50676883E-07f, -1.8284193E-07f, 156.1688f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 3"), 728144, 10751269u, new Vector3(-1551.661f, 113.927795f, 56.7797241f), new Vector3(0f, -0f, 179.852112f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "0x44b4d748"), 190281, 1152702280u, new Vector3(-1552.44727f, 112.752151f, 56.7789841f), new Vector3(0f, -0f, 177.489563f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "0x44b4d748"), 405336, 1152702280u, new Vector3(-1553.78711f, 113.239906f, 56.7798767f), new Vector3(-1.66825425E-24f, -2.02653771E-24f, 79.14327f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "0x76463a5e"), 34896, 1984313950u, new Vector3(-1554.08728f, 114.548019f, 56.77816f), new Vector3(0f, -0f, 95.99232f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 2"), 78935, 357447289u, new Vector3(-1555.606f, 113.617332f, 56.7796059f), new Vector3(0f, -0f, 164.986725f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "0x76463a5e"), 503128, 1984313950u, new Vector3(-1554.93359f, 115.963684f, 56.77735f), new Vector3(-2.469173E-17f, -9.118795E-18f, -140.711365f), 966099553u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 54872, 1744231373u, new Vector3(-1555.79944f, 114.862267f, 56.77986f), new Vector3(0f, -0f, 141.940842f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "0x44b4d748"), 186712, 1152702280u, new Vector3(-1553.42444f, 115.240677f, 56.7797241f), new Vector3(0f, -0f, 116.832062f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 2"), 706899, 3324988722u, new Vector3(-1555.41248f, 118.385132f, 56.7905922f), new Vector3(0f, -0f, -158.916122f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 2"), 691538, 357447289u, new Vector3(-1557.84961f, 121.275841f, 56.8691521f), new Vector3(0f, -0f, 132.787064f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 729176, 2813792322u, new Vector3(-1560.13562f, 116.404053f, 56.86948f), new Vector3(0f, -0f, -136.49704f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 602967, 1744231373u, new Vector3(-1557.399f, 112.267891f, 56.77931f), new Vector3(0f, -0f, 177.299576f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Z-Type"), 511572, 758895617u, new Vector3(-1560.2052f, 100.831894f, 57.0413628f), new Vector3(-1.330896f, -0.0041009807f, -43.96682f), 0, 33, 4, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Schafter LWB (Armored)"), 670039, 1922255844u, new Vector3(-1534.68311f, 96.41974f, 56.3255539f), new Vector3(0.242753074f, -0.8999585f, 150.1214f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Z-Type"), 536921, 758895617u, new Vector3(-1568.24219f, 107.845589f, 57.0533752f), new Vector3(-1.75861561f, -0.0441783f, -46.00606f), 0, 33, 4, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Billionaire"), 4426, 28135809u, new Vector3(-1553.4519f, 103.17556f, 59.1827469f), new Vector3(0f, 0f, 15.0250034f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 4951, 113622690u, new Vector3(-1561.571f, 111.54422f, 56.8393326f), new Vector3(0f, 0f, 56.48786f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 281433, 113622690u, new Vector3(-1557.10754f, 107.406059f, 56.8915062f), new Vector3(0f, -0f, -144.952118f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 258136, 113622690u, new Vector3(-1546.90576f, 117.766693f, 56.3846169f), new Vector3(0f, 0f, -61.781292f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 666196, 113622690u, new Vector3(-1547.03809f, 121.078156f, 56.3846169f), new Vector3(0f, -0f, -96.89068f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 198996, 113622690u, new Vector3(-1549.08862f, 123.021652f, 56.3853531f), new Vector3(0f, 0f, 6.648764f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 250969, 113622690u, new Vector3(-1554.15735f, 117.471375f, 56.3854f), new Vector3(0f, 0f, 82.60089f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 319577, 113622690u, new Vector3(-1552.16577f, 115.65966f, 56.3846169f), new Vector3(0f, -0f, -155.331985f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 406615, 113622690u, new Vector3(-1548.72949f, 115.701439f, 56.3846169f), new Vector3(0f, -0f, -174.370071f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 686168, 113622690u, new Vector3(-1570.16675f, 118.893379f, 57.8187981f), new Vector3(0f, -0f, 142.437515f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 577875, 113622690u, new Vector3(-1566.90857f, 125.7044f, 57.5395966f), new Vector3(0f, 0f, 8.291422f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 547672, 113622690u, new Vector3(-1583.63806f, 132.33075f, 58.41109f), new Vector3(0f, 0f, -75.08228f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 430416, 113622690u, new Vector3(-1538.40527f, 111.206184f, 56.38434f), new Vector3(0f, -0f, -110.959091f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 528978, 113622690u, new Vector3(-1531.4519f, 104.25721f, 56.4371033f), new Vector3(0f, 0f, 6.195193f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 181330, 113622690u, new Vector3(-1534.54749f, 100.929848f, 56.45706f), new Vector3(0f, 0f, 72.06483f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "lts_prop_lts_elecbox_24b"), 266602, 19408745u, new Vector3(-1550.24365f, 91.59162f, 60.3182f), new Vector3(0f, -0f, -132.001038f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_elecbox_01a"), 739930, 1981815996u, new Vector3(-1495.7948f, 122.751434f, 54.6527748f), new Vector3(0f, 0f, 46.7235f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Swift Deluxe"), 246560, 1075432268u, new Vector3(-1603.85754f, 104.629845f, 63.08402f), new Vector3(-0.289108723f, 0.000153394387f, -108.584419f), 159, 0, 160, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_helipad_01"), 784675, 3789865418u, new Vector3(-1603.92078f, 102.990776f, 60.302536f), new Vector3(0f, 0f, 57.729126f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects MansionGuardsObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "PartyGuard"), 28956, 985359552u, new Vector3(-1598.62024f, 108.343018f, 63.1759224f), new Vector3(0f, -0f, -18.47779f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "PartyGuard"), 406823, 985359552u, new Vector3(-1601.86523f, 110.378227f, 63.17626f), new Vector3(0f, -0f, -28.66256f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "PartyGuard"), 190557, 985359552u, new Vector3(-1567.10693f, 125.233528f, 57.9075775f), new Vector3(0f, -0f, 163.952774f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "PartyGuard"), 456779, 985359552u, new Vector3(-1603.40442f, 116.875824f, 60.6763725f), new Vector3(0f, 0f, 46.798027f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "PartyGuard"), 288077, 985359552u, new Vector3(-1592.04382f, 117.91806f, 59.98584f), new Vector3(0f, 0f, -20.4027157f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "PartyGuard"), 105569, 985359552u, new Vector3(-1546.33679f, 105.682579f, 56.7836876f), new Vector3(0f, 0f, -41.12987f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "PartyGuard"), 702818, 985359552u, new Vector3(-1556.86523f, 108.060005f, 56.7758636f), new Vector3(0f, 0f, 19.6219158f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "PartyGuard"), 197205, 985359552u, new Vector3(-1567.64087f, 105.846634f, 57.6451073f), new Vector3(0f, 0f, -62.31889f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "PartyGuard"), 460887, 985359552u, new Vector3(-1562.69116f, 101.302231f, 57.64253f), new Vector3(0f, 0f, -24.1376476f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "PartyGuard"), 45921, 985359552u, new Vector3(-1529.56726f, 103.702385f, 56.76919f), new Vector3(0f, -0f, -133.363419f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "PartyGuard"), 172387, 985359552u, new Vector3(-1579.235f, 143.838333f, 58.46162f), new Vector3(0f, 0f, 59.9907379f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "PartyGuard"), 758881, 985359552u, new Vector3(-1612.13354f, 75.12043f, 61.4957466f), new Vector3(0f, -0f, 156.620392f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "PartyGuard"), 8282, 985359552u, new Vector3(-1617.12451f, 78.31489f, 61.56691f), new Vector3(0.00012414412f, 0.000354422751f, 151.2452f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "PartyGuard"), 677215, 985359552u, new Vector3(-1537.19556f, 130.267517f, 57.3766556f), new Vector3(0f, -0f, 137.574036f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "PartyGuard"), 303207, 985359552u, new Vector3(-1546.75732f, 124.465385f, 56.78352f), new Vector3(0f, -0f, 158.912628f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "PartyGuard"), 579176, 985359552u, new Vector3(-1518.42627f, 80.42436f, 56.60681f), new Vector3(0f, 0f, 10.596591f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects MansionCCTVsObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_cctv_s"), 449582, 289451089u, new Vector3(-1558.93408f, 88.84842f, 61.402298f), new Vector3(4.268819E-07f, -179.999756f, -173.739334f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_cctv_s"), 102446, 289451089u, new Vector3(-1583.56677f, 132.162628f, 60.7587471f), new Vector3(0f, 0f, -79.03027f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_cctv_s"), 295215, 289451089u, new Vector3(-1518.425f, 79.98424f, 60.2250481f), new Vector3(0f, -0f, -179.403931f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_cctv_s"), 716849, 289451089u, new Vector3(-1564.79041f, 145.997772f, 63.0437431f), new Vector3(0f, -0f, -107.601616f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_cctv_s"), 387114, 289451089u, new Vector3(-1542.27759f, 132.885818f, 62.63083f), new Vector3(-1.2722218E-14f, 2.980209E-07f, -32.0775f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_cctv_s"), 150074, 289451089u, new Vector3(-1517.02612f, 69.82781f, 61.4023666f), new Vector3(5.336085E-08f, -179.999847f, 87.62905f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects MansionVaultObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_fingerprint_scanner_01d"), 671306, 1913446926u, new Vector3(-1569.89771f, 103.772331f, 58.3080521f), new Vector3(-2.130866E-06f, -3.56850649E-07f, 138.060181f), 361033, 0, new Vector3(-3.57627869E-07f, 0.499999762f, 1.40000021f), new Vector3(0f, 0f, 179.999985f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_door_01a"), 284996, 2244758654u, new Vector3(-1566.722f, 100.528992f, 58.78805f), new Vector3(-2.1308656E-06f, -3.568513E-07f, 138.060165f), 143178, 0, new Vector3(-0.8299999f, -0.21f, 1.24000013f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 143178, 3294791577u, new Vector3(-1567.47974f, 100.927513f, 57.54805f), new Vector3(-2.1308656E-06f, -3.568513E-07f, 138.060165f), 616766, 0, new Vector3(-0.100000016f, -4.29999828f, 7.000001f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 507719, 3294791577u, new Vector3(-1565.9176f, 99.52398f, 57.54805f), new Vector3(2.1308656E-06f, 3.56852155E-07f, -41.93985f), 143178, 0, new Vector3(-2.10000014f, 0f, 0f), new Vector3(0f, 0f, 179.999985f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 402241, 3294791577u, new Vector3(-1569.11621f, 102.397881f, 57.54805f), new Vector3(2.13086582E-06f, 3.56850421E-07f, -41.9398041f), 143178, 0, new Vector3(2.2f, 0f, 0f), new Vector3(0f, 0f, -179.999985f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_lobby_pillar_03a"), 554560, 2689055672u, new Vector3(-1564.72742f, 98.45462f, 56.84805f), new Vector3(-2.13086582E-06f, -3.56851274E-07f, 138.060165f), 507719, 0, new Vector3(1.59999955f, 0f, -0.6999999f), new Vector3(0f, 0f, -179.999985f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_lobby_pillar_03a"), 509501, 2689055672u, new Vector3(-1570.23193f, 103.400406f, 56.90805f), new Vector3(-2.130866E-06f, -3.56849654E-07f, 138.060211f), 402241, 0, new Vector3(-1.50000012f, 0f, -0.639999866f), new Vector3(0f, 0f, -179.999985f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_lobby_pillar_03a"), 560454, 2689055672u, new Vector3(-1564.72742f, 98.45462f, 56.84805f), new Vector3(2.1308656E-06f, 3.56851558E-07f, -41.93983f), 507719, 0, new Vector3(1.59999955f, 0f, -0.6999999f), new Vector3(0f, 0f, 1.71661377E-05f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_lobby_pillar_03a"), 361033, 2689055672u, new Vector3(-1570.23193f, 103.400406f, 56.90805f), new Vector3(2.13086582E-06f, 3.56849824E-07f, -41.93979f), 402241, 0, new Vector3(-1.50000012f, 0f, -0.639999866f), new Vector3(0f, 0f, 1.71661377E-05f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 645693, 3294791577u, new Vector3(-1571.07813f, 102.009743f, 57.50805f), new Vector3(3.56849256E-07f, -2.130866E-06f, 48.06022f), 361033, 0, new Vector3(0.300000042f, -1.6f, 0.599999964f), new Vector3(0f, 0f, 90.0000153f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 458057, 3294791577u, new Vector3(-1566.09436f, 97.53181f, 57.54805f), new Vector3(-3.56850876E-07f, 2.13086582E-06f, -131.9398f), 560454, 0, new Vector3(-0.400000066f, -1.60000014f, 0.6999999f), new Vector3(0f, 0f, -89.99999f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_fnclink_03d"), 301639, 729940451u, new Vector3(-1565.62646f, 98.0525f, 57.24805f), new Vector3(-3.56850876E-07f, 2.13086582E-06f, -131.9398f), 458057, 0, new Vector3(-0.6999999f, 0f, -0.299999982f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_fnclink_03d"), 181058, 729940451u, new Vector3(-1571.546f, 101.489052f, 57.1080475f), new Vector3(3.56849256E-07f, -2.130866E-06f, 48.06022f), 645693, 0, new Vector3(-0.6999999f, 0f, -0.399999976f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_fnclink_03d"), 538951, 729940451u, new Vector3(-1569.65771f, 102.884438f, 57.14805f), new Vector3(2.13086582E-06f, 3.56850421E-07f, -41.9398041f), 402241, 0, new Vector3(-0.7280009f, 0f, -0.399999976f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_fnclink_03d"), 256584, 729940451u, new Vector3(-1566.43835f, 99.99182f, 57.04805f), new Vector3(2.1308656E-06f, 3.56852155E-07f, -41.93985f), 507719, 0, new Vector3(-0.7000001f, 0f, -0.50000006f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_fnclink_03d"), 535878, 729940451u, new Vector3(-1567.0968f, 96.41603f, 57.24805f), new Vector3(-3.56850876E-07f, 2.13086582E-06f, -131.9398f), 268606, 0, new Vector3(-0.6999999f, 0f, -0.299999982f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 268606, 3294791577u, new Vector3(-1567.5647f, 95.89534f, 57.54805f), new Vector3(-3.56850876E-07f, 2.13086582E-06f, -131.9398f), 560454, 0, new Vector3(-0.400000066f, -3.79999852f, 0.6999999f), new Vector3(0f, 0f, -89.99999f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_fnclink_03d"), 333383, 729940451u, new Vector3(-1568.50037f, 94.85396f, 57.24805f), new Vector3(-3.56850876E-07f, 2.13086582E-06f, -131.9398f), 456518, 0, new Vector3(-0.6999999f, 0f, -0.299999982f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 456518, 3294791577u, new Vector3(-1568.96826f, 94.33327f, 57.54805f), new Vector3(-3.56850876E-07f, 2.13086582E-06f, -131.9398f), 560454, 0, new Vector3(-0.400000066f, -5.89999676f, 0.6999999f), new Vector3(0f, 0f, -89.99999f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_fnclink_03d"), 603978, 729940451u, new Vector3(-1573.01636f, 99.85259f, 57.1080475f), new Vector3(3.56849256E-07f, -2.130866E-06f, 48.06022f), 741704, 0, new Vector3(-0.6999999f, 0f, -0.399999976f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 741704, 3294791577u, new Vector3(-1572.54846f, 100.373283f, 57.50805f), new Vector3(3.56849256E-07f, -2.130866E-06f, 48.06022f), 361033, 0, new Vector3(0.300000042f, -3.79999828f, 0.599999964f), new Vector3(0f, 0f, 90.0000153f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 234567, 3294791577u, new Vector3(-1574.01892f, 98.73682f, 57.50805f), new Vector3(3.56849256E-07f, -2.130866E-06f, 48.06022f), 361033, 0, new Vector3(0.300000042f, -5.999996f, 0.599999964f), new Vector3(0f, 0f, 90.0000153f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_temp_block_blocker"), 616766, 4143853297u, new Vector3(-1570.428f, 97.79581f, 50.54805f), new Vector3(-2.1308656E-06f, -3.568513E-07f, 138.060165f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_fnclink_03d"), 411461, 729940451u, new Vector3(-1574.48682f, 98.2161255f, 57.1080475f), new Vector3(3.56849256E-07f, -2.130866E-06f, 48.06022f), 234567, 0, new Vector3(-0.7000001f, 0f, -0.400000066f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_fnclink_03d"), 489285, 729940451u, new Vector3(-1575.82349f, 96.72843f, 57.1080475f), new Vector3(3.56849256E-07f, -2.130866E-06f, 48.06022f), 759881, 0, new Vector3(-0.7000001f, 0f, -0.400000066f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 759881, 3294791577u, new Vector3(-1575.35559f, 97.24912f, 57.50805f), new Vector3(3.56849256E-07f, -2.130866E-06f, 48.06022f), 361033, 0, new Vector3(0.300000042f, -7.99999475f, 0.599999964f), new Vector3(0f, 0f, 90.0000153f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_fnclink_03d"), 71747, 729940451u, new Vector3(-1569.90381f, 93.29188f, 57.24805f), new Vector3(-3.56850876E-07f, 2.13086582E-06f, -131.9398f), 299848, 0, new Vector3(-0.6999999f, 0f, -0.299999982f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 299848, 3294791577u, new Vector3(-1570.3717f, 92.77119f, 57.54805f), new Vector3(-3.56850876E-07f, 2.13086582E-06f, -131.9398f), 560454, 0, new Vector3(-0.400000036f, -7.99999475f, 0.6999999f), new Vector3(0f, 0f, -89.99999f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_fnclink_03d"), 192841, 729940451u, new Vector3(-1571.412f, 92.36152f, 57.24805f), new Vector3(-2.13086582E-06f, -3.56851274E-07f, 138.060165f), 255814, 0, new Vector3(-0.6999999f, 0f, -0.299999982f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 255814, 3294791577u, new Vector3(-1571.93274f, 92.82936f, 57.54805f), new Vector3(-2.13086582E-06f, -3.56851274E-07f, 138.060165f), 560454, 0, new Vector3(-1.60000026f, -8.999998f, 0.6999999f), new Vector3(0f, 0f, 180f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_fnclink_03d"), 345162, 729940451u, new Vector3(-1573.04846f, 93.83189f, 57.24805f), new Vector3(-2.13086582E-06f, -3.56851274E-07f, 138.060165f), 211773, 0, new Vector3(-0.6999999f, 0f, -0.299999982f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 211773, 3294791577u, new Vector3(-1573.56921f, 94.29973f, 57.54805f), new Vector3(-2.13086582E-06f, -3.56851274E-07f, 138.060165f), 560454, 0, new Vector3(-3.79999852f, -8.999998f, 0.6999999f), new Vector3(0f, 0f, 180f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_fnclink_03d"), 88384, 729940451u, new Vector3(-1574.68494f, 95.30225f, 57.24805f), new Vector3(-2.13086582E-06f, -3.56851274E-07f, 138.060165f), 33600, 0, new Vector3(-0.6999999f, 0f, -0.299999982f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_d_frame_01a"), 33600, 3294791577u, new Vector3(-1575.20569f, 95.7700958f, 57.54805f), new Vector3(-2.13086582E-06f, -3.56851274E-07f, 138.060165f), 560454, 0, new Vector3(-5.99999666f, -8.999998f, 0.6999999f), new Vector3(0f, 0f, 180f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "gr_prop_gr_ramproof_gate"), 85826, 1056616439u, new Vector3(-1567.6803f, 100.704361f, 60.54805f), new Vector3(90f, -41.9398537f, 0f), 143178, 0, new Vector3(0f, 0.3f, 2.99999952f), new Vector3(89.9999847f, 179.999985f, -1.90734863E-06f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "gr_prop_gr_ramproof_gate"), 342601, 1056616439u, new Vector3(-1573.29443f, 94.4560547f, 60.5480461f), new Vector3(90f, 138.0601f, 0f), 85826, 0, new Vector3(0f, 0f, 8.399996f), new Vector3(0f, 179.999985f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_byard_rampold"), 667978, 3739923095u, new Vector3(-1564.21228f, 104.713585f, 55.8480453f), new Vector3(-3.56852E-07f, 2.1308656E-06f, -131.939835f), 616766, 0, new Vector3(0f, -9.299999f, 5.29999638f), new Vector3(0f, 0f, 89.9999847f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 696902, 304964818u, new Vector3(-1574.45007f, 97.21605f, 57.54805f), new Vector3(0f, 0f, 47.4811859f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 542536, 304964818u, new Vector3(-1570.457f, 101.383934f, 57.54805f), new Vector3(0f, 0f, 47.02027f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 749130, 304964818u, new Vector3(-1568.93982f, 95.6665f, 57.54805f), new Vector3(0f, -0f, -132.827316f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 309308, 304964818u, new Vector3(-1570.73108f, 93.6179352f, 57.54805f), new Vector3(0f, -0f, -132.007721f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects VaultLootObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 304185, 2151061176u, new Vector3(-1570.30249f, 101.314621f, 58.3899078f), new Vector3(0f, 0f, 46.502758f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 382023, 2151061176u, new Vector3(-1574.346f, 97.07396f, 58.3899078f), new Vector3(0f, 0f, 46.09306f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 313419, 2151061176u, new Vector3(-1570.88965f, 93.72767f, 58.3899078f), new Vector3(0f, -0f, -132.367477f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 648517, 2151061176u, new Vector3(-1569.0769f, 95.79866f, 58.3899078f), new Vector3(0f, -0f, -131.547913f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 180f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_cash_trolly_01c"), 506692, 412463629u, new Vector3(-1573.1051f, 98.5683746f, 58.02193f), new Vector3(0f, 0f, -131.9628f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 180f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_diamond_trolly_01c"), 70733, 3489641539u, new Vector3(-1572.00159f, 99.79529f, 58.02193f), new Vector3(0f, 0f, -131.9628f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 180f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_gold_trolly_01c"), 368707, 3559648747u, new Vector3(-1573.35217f, 94.9331741f, 58.02193f), new Vector3(0f, -0f, -42.8596649f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 180f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_diamond_trolly_01c"), 277068, 3489641539u, new Vector3(-1572.3844f, 94.03536f, 58.02193f), new Vector3(0f, -0f, -42.8596649f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 180f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_diamond_trolly_01c"), 477002, 3489641539u, new Vector3(-1566.2688f, 98.49079f, 58.02193f), new Vector3(0f, -0f, -313.1374f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 180f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_cash_trolly_01c"), 38467, 412463629u, new Vector3(-1567.39673f, 97.28677f, 58.02193f), new Vector3(0f, -0f, -313.1374f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 180f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_cash_trolly_01c"), 668493, 412463629u, new Vector3(-1574.1958f, 95.77543f, 58.02193f), new Vector3(0f, -0f, -42.4948273f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 180f))
		});

		public static MissionOjects HeadofSecurityObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_elecbox_01a"), 15433, 1981815996u, new Vector3(-1784.46643f, 428.023621f, 121.641533f), new Vector3(0f, 0f, 0.179885775f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "PartyGuard"), 673301, 985359552u, new Vector3(-1797.92285f, 469.934082f, 133.715271f), new Vector3(0f, 0f, -51.5642128f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "PartyGuard"), 92190, 985359552u, new Vector3(-1786.20239f, 459.004761f, 128.31337f), new Vector3(0f, -0f, 103.743027f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "PartyGuard"), 291127, 985359552u, new Vector3(-1785.50476f, 420.2143f, 116.829842f), new Vector3(0f, -0f, -151.181732f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "PartyGuard"), 715327, 985359552u, new Vector3(-1822.1073f, 423.3763f, 127.911133f), new Vector3(0f, -0f, 110.285629f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "PartyGuard"), 336190, 985359552u, new Vector3(-1846.09729f, 457.119659f, 126.870888f), new Vector3(0f, -0f, 113.200928f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "PartyGuard"), 269377, 985359552u, new Vector3(-1820.60449f, 448.66394f, 128.658081f), new Vector3(0f, 0f, 81.6726761f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "PartyGuard"), 273220, 985359552u, new Vector3(-1787.80713f, 439.4114f, 128.303543f), new Vector3(0f, 0f, -83.99095f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "PartyGuard"), 11841, 985359552u, new Vector3(-1807.17542f, 425.311554f, 132.303284f), new Vector3(0f, -0f, -165.21521f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Schafter V12 (Armored)"), 328506, 3406724313u, new Vector3(-1796.3363f, 455.166473f, 127.875252f), new Vector3(0.646358848f, 0.283791274f, 89.58481f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Schafter V12 (Armored)"), 683082, 3406724313u, new Vector3(-1790.98938f, 462.398651f, 127.874084f), new Vector3(0.584333837f, 0.454205573f, 105.601929f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Schafter V12 (Armored)"), 255286, 3406724313u, new Vector3(-1811.81946f, 455.2317f, 127.859108f), new Vector3(0.318927884f, 0.8261952f, 35.4781265f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cognoscenti (Armored)"), 556364, 3690124666u, new Vector3(-1808.31042f, 454.768616f, 127.915016f), new Vector3(-0.30267188f, 0.234545022f, 35.9986267f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static bool CreatedHoSInteriorObjects;

		public static bool CreatedHoSExteriorObjects;

		public static MissionOjects CartelObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 67160, 2127932792u, new Vector3(-1371.14954f, 735.5286f, 183.51442f), new Vector3(0f, 0f, 38.2495575f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Merryweather Mesa"), 103238, 2230595153u, new Vector3(-1367.5929f, 736.3837f, 183.417328f), new Vector3(2.75216126f, -2.08631158f, -86.70735f), 152, 153, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Merryweather Mesa"), 343885, 2230595153u, new Vector3(-1373.01978f, 734.5376f, 183.173645f), new Vector3(5.455262f, 0.836736858f, -120.871368f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Merryweather Mesa"), 262987, 2230595153u, new Vector3(-1362.12878f, 736.3993f, 183.681717f), new Vector3(1.86388636f, -3.44578242f, -65.40855f), 154, 153, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static bool CreatedCartelObjects;

		public static MissionOjects RoadBlockObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Merryweather Mesa"), 637498, 2230595153u, new Vector3(731.8646f, -2913.02124f, 5.651374f), new Vector3(-0.12349496f, -0.782650948f, 90.59377f), 152, 153, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mc_conc_barrier_01"), 466746, 2139919312u, new Vector3(730.2929f, -2911.45483f, 4.92079449f), new Vector3(0f, -0f, -166.9581f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mc_conc_barrier_01"), 356664, 2139919312u, new Vector3(722.296143f, -2914.30347f, 4.90315962f), new Vector3(0f, -0f, -132.049973f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mc_conc_barrier_01"), 229944, 2139919312u, new Vector3(721.8001f, -2917.55786f, 4.932735f), new Vector3(0f, -0f, -167.825027f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mc_conc_barrier_01"), 620087, 2139919312u, new Vector3(727.7172f, -2912.34155f, 4.925557f), new Vector3(0f, -0f, -149.552155f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Merryweather Mesa"), 63290, 2230595153u, new Vector3(725.375061f, -2914.70972f, 5.64577341f), new Vector3(0.07231162f, -0.6261712f, -81.46676f), 154, 153, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mp_conc_barrier_01"), 540472, 682373179u, new Vector3(724.8419f, -2912.83936f, 4.928094f), new Vector3(0f, -0f, -169.601379f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mp_conc_barrier_01"), 278314, 682373179u, new Vector3(732.9156f, -2910.50269f, 4.91578f), new Vector3(0f, -0f, 173.355408f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 29758, 1821116645u, new Vector3(732.3195f, -2911.025f, 5.89045858f), new Vector3(0f, 0f, 15.4612341f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 296237, 2127932792u, new Vector3(728.2622f, -2912.65723f, 5.89546442f), new Vector3(0f, 0f, 17.6108265f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 56125, 2127932792u, new Vector3(724.3369f, -2913.47217f, 5.867099f), new Vector3(0f, 0f, 2.614739f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static bool CreatedRoadBlockObjects;

		public static MissionOjects HeadofSecurityVehicleSpawnsObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cognoscenti 55 (Armored)"), 756244, 704435172u, new Vector3(-1726.45007f, 42.46945f, 66.71223f), new Vector3(2.4531312f, 2.08035827f, 48.6154976f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cognoscenti 55 (Armored)"), 202256, 704435172u, new Vector3(-1924.16516f, -263.631378f, 39.5465126f), new Vector3(5.509858f, 5.17396641f, 158.999313f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cognoscenti 55 (Armored)"), 493077, 704435172u, new Vector3(-1341.45776f, -460.783447f, 33.2225723f), new Vector3(-1.65117526f, -0.388021231f, -144.2902f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cognoscenti 55 (Armored)"), 248599, 704435172u, new Vector3(-1311.4574f, -316.456848f, 36.759346f), new Vector3(3.63136983f, 2.06030846f, 28.93614f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cognoscenti 55 (Armored)"), 605974, 704435172u, new Vector3(-1434.341f, -185.772385f, 47.0023842f), new Vector3(-0.0315234549f, 2.18151617f, 138.434418f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cognoscenti 55 (Armored)"), 408344, 704435172u, new Vector3(-1720.86963f, -551.2728f, 36.9224548f), new Vector3(-1.193235f, 0.384491861f, 124.002068f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cognoscenti 55 (Armored)"), 110616, 704435172u, new Vector3(-1695.44409f, -703.8596f, 11.0318356f), new Vector3(0f, -0f, -127.238411f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cognoscenti 55 (Armored)"), 65043, 704435172u, new Vector3(-1468.22559f, -736.822f, 24.2260838f), new Vector3(-3.67991614f, 0.3891507f, -127.79715f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cognoscenti 55 (Armored)"), 551952, 704435172u, new Vector3(-1155.1123f, -943.8066f, 2.1032443f), new Vector3(0.0495472625f, 3.27631831f, 119.839279f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cognoscenti 55 (Armored)"), 5399, 704435172u, new Vector3(-1059.9895f, -1034.78784f, 1.64626718f), new Vector3(-0.4718859f, 2.736615f, -152.402069f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cognoscenti 55 (Armored)"), 400410, 704435172u, new Vector3(-1253.96985f, -897.6402f, 11.15632f), new Vector3(-2.03316879f, 0.138315856f, 125.655418f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cognoscenti 55 (Armored)"), 184596, 704435172u, new Vector3(-1397.681f, -898.745667f, 10.9942293f), new Vector3(-5.753512f, 1.82575381f, 134.1292f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cognoscenti 55 (Armored)"), 45851, 704435172u, new Vector3(-1589.13013f, -791.450134f, 11.5102968f), new Vector3(2.53564763f, 1.88396907f, -111.0197f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cognoscenti 55 (Armored)"), 28697, 704435172u, new Vector3(-1124.198f, -664.5544f, 11.2478771f), new Vector3(3.18711829f, 1.52816284f, -56.35805f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cognoscenti 55 (Armored)"), 782619, 704435172u, new Vector3(-1195.66309f, -611.3913f, 25.4809246f), new Vector3(6.0117507f, 1.97751808f, 40.221f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cognoscenti 55 (Armored)"), 11036, 704435172u, new Vector3(-995.44104f, -702.830933f, 21.28642f), new Vector3(2.38420248f, 2.017896f, -51.186882f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cognoscenti 55 (Armored)"), 74269, 704435172u, new Vector3(-830.062134f, -839.6262f, 19.12664f), new Vector3(2.45442653f, 3.414853f, -89.63706f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cognoscenti 55 (Armored)"), 361499, 704435172u, new Vector3(-901.58136f, -527.303833f, 34.49806f), new Vector3(0f, 0f, 30.7655373f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cognoscenti 55 (Armored)"), 399902, 704435172u, new Vector3(-1067.06677f, -263.488159f, 37.3051643f), new Vector3(-0.2500853f, 1.78495276f, 116.334183f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cognoscenti 55 (Armored)"), 34844, 704435172u, new Vector3(-1137.56f, -401.4043f, 35.916996f), new Vector3(-0.673677444f, 2.32730246f, 102.475792f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cognoscenti 55 (Armored)"), 512297, 704435172u, new Vector3(-1074.14514f, -784.9653f, 18.80769f), new Vector3(0.257331461f, 2.06748152f, -137.769363f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cognoscenti 55 (Armored)"), 170535, 704435172u, new Vector3(-844.033264f, -998.6015f, 13.3067579f), new Vector3(0f, -0f, -151.073853f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static bool CreatedHeadofSecurityVehicle;

		public static MissionOjects VinewoodHouseObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_acc_guitar_01_d1"), 269572, 4173164723u, new Vector3(373.9574f, 409.574219f, 146.521912f), new Vector3(0f, -0f, -103.004387f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_el_guitar_01"), 486148, 2766659751u, new Vector3(370.6226f, 417.252625f, 146.537384f), new Vector3(0f, 0f, 75.31054f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_el_guitar_02"), 509956, 916292624u, new Vector3(370.479248f, 416.607849f, 146.593f), new Vector3(0f, 0f, 75.31054f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_el_guitar_03"), 337925, 61087258u, new Vector3(370.362152f, 416.1191f, 146.542267f), new Vector3(0f, 0f, 77.101f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sf_prop_sf_guitars_rack_01a"), 378374, 2557269496u, new Vector3(370.640442f, 415.117859f, 144.702118f), new Vector3(0f, -0f, 150.442688f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_res_fh_guitaramp"), 548102, 2541036517u, new Vector3(369.1043f, 410.034882f, 144.7974f), new Vector3(0f, 0f, 80.57995f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_casino_art_guitar_01a"), 742662, 3206382347u, new Vector3(370.214447f, 415.502747f, 146.560867f), new Vector3(0f, 0f, 75.1032639f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_el_guitar_03"), 519431, 61087258u, new Vector3(375.385223f, 421.8939f, 146.2715f), new Vector3(0f, -0f, 164.771576f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_el_guitar_03"), 779015, 61087258u, new Vector3(376.346039f, 421.654327f, 146.294189f), new Vector3(0f, -0f, 164.771576f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_el_guitar_03"), 108551, 61087258u, new Vector3(377.306854f, 421.414764f, 146.316879f), new Vector3(0f, -0f, 164.771576f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_el_guitar_03"), 448264, 61087258u, new Vector3(378.250336f, 421.179535f, 146.324326f), new Vector3(0f, -0f, 164.259491f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_club_speaker_dj"), 643078, 2595533326u, new Vector3(373.001953f, 424.729858f, 141.100021f), new Vector3(0f, 0f, 76.95182f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_dj_deck_01a"), 626441, 992125567u, new Vector3(373.001068f, 424.343353f, 141.8821f), new Vector3(0f, 0f, 88.8257141f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_dj_kit_mixer"), 740361, 2447292705u, new Vector3(373.063354f, 424.998444f, 141.882217f), new Vector3(0f, 0f, 68.60903f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_dj_stand"), 644362, 976772591u, new Vector3(373.807037f, 402.048f, 141.113235f), new Vector3(0f, -0f, 165.542175f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_dj_set_wbeach"), 756746, 1222991438u, new Vector3(370.9311f, 402.789551f, 142.004349f), new Vector3(0f, -0f, 165.542175f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_battle_dj_deck_01a"), 711947, 3636477461u, new Vector3(369.488f, 403.070618f, 142.009827f), new Vector3(0f, -0f, 142.970474f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_battle_dj_mixer_01a"), 436236, 2310235814u, new Vector3(372.470154f, 402.326965f, 141.990692f), new Vector3(0f, -0f, -176.5452f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_dj_set_wbeach"), 661517, 1222991438u, new Vector3(377.323364f, 433.6111f, 138.204224f), new Vector3(0f, 0f, -13.28455f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_battle_dj_stand"), 8712, 528082922u, new Vector3(375.3375f, 433.13504f, 137.31311f), new Vector3(0f, 0f, 34.6208076f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_bag_djlp_01a"), 150287, 338809937u, new Vector3(379.1717f, 432.528717f, 137.3019f), new Vector3(0f, 0f, -67.2803345f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_mic_dj_01a"), 564239, 1886461028u, new Vector3(376.124329f, 433.383972f, 137.300064f), new Vector3(0f, 0f, 26.8931561f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sf_prop_sf_headphones_dj"), 342033, 2172241942u, new Vector3(369.560364f, 412.1618f, 145.792664f), new Vector3(0f, 0f, 75.88236f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sf_prop_sf_dj_desk_01a"), 711441, 4134478150u, new Vector3(369.573578f, 403.228577f, 144.500549f), new Vector3(0f, -0f, 166.726883f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_dj_deck_02"), 249876, 411094673u, new Vector3(374.781281f, 417.800751f, 145.9147f), new Vector3(0f, 0f, -85.12666f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_dj_deck_01"), 598546, 1164617828u, new Vector3(368.617981f, 407.93634f, 142.0363f), new Vector3(0f, -0f, 163.656235f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_bag_djlp_01a"), 611095, 2261812063u, new Vector3(375.131927f, 415.849121f, 145.930328f), new Vector3(0f, 0f, -31.3353157f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_acc_guitar_01"), 756246, 3586178055u, new Vector3(379.468475f, 412.818237f, 146.456314f), new Vector3(0f, -0f, -101.452477f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_el_guitar_01"), 785686, 2766659751u, new Vector3(380.5734f, 417.410431f, 146.719772f), new Vector3(0f, -0f, -104.215782f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_el_guitar_01"), 783894, 2766659751u, new Vector3(380.4216f, 416.7922f, 146.723267f), new Vector3(0f, -0f, -104.215782f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects DJKeycardObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_acc_pass_01a"), 337424, 4027835392u, new Vector3(379.4237f, 418.6183f, 141.792984f), new Vector3(0f, 0f, -0.8375021f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects CarCarrierObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Trailer"), 128818, 2078290630u, new Vector3(-1049.39673f, -3209.44385f, 14.1953831f), new Vector3(1.03406048f, 0.0003321187f, 57.17048f), 22, 112, 4, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Ignus"), 547675, 2850852987u, new Vector3(-1053.09338f, -3207.0708f, 15.1848211f), new Vector3(1.03406048f, 0.0003321187f, 57.17048f), 117, 55, 73, -1, 128818, 0, new Vector3(-0.009999998f, 4.409998f, 0.9100001f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Ignus"), 18026, 2850852987u, new Vector3(-1052.37878f, -3207.51978f, 17.0598946f), new Vector3(3.13406062f, 0.0003325086f, 57.1704674f), 9, 27, 62, -1, 128818, 0, new Vector3(0f, 3.59999871f, 2.79999948f), new Vector3(2.10000014f, -5.96046448E-08f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Ignus"), 144753, 2850852987u, new Vector3(-1048.70032f, -3209.893f, 15.2305946f), new Vector3(-1.76593924f, 0.000332115567f, 57.1704941f), 59, 88, 94, -1, 128818, 0, new Vector3(0f, -1.31f, 1.05000007f), new Vector3(-2.79999948f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Ignus"), 401012, 2850852987u, new Vector3(-1047.53589f, -3210.64453f, 17.1959038f), new Vector3(-1.465939f, 0.000332119933f, 57.17049f), 117, 55, 73, -1, 128818, 0, new Vector3(0f, -2.76f, 3.03999758f), new Vector3(MathF.PI * -113f / 142f, 2.98023224E-08f, 0f))
		});

		public static MissionOjects PenthouseObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 511796, 2813792322u, new Vector3(975.920959f, 57.178833f, 116.161407f), new Vector3(0f, 0f, -63.75561f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 334352, 2813792322u, new Vector3(975.543152f, 55.8777657f, 116.161674f), new Vector3(0f, 0f, -81.47547f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 89613, 1744231373u, new Vector3(977.2537f, 56.41491f, 116.167862f), new Vector3(0f, 0f, 89.92413f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 107316, 1744231373u, new Vector3(971.0123f, 48.6338348f, 116.162888f), new Vector3(-0f, -0f, -73.4621048f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 327219, 2813792322u, new Vector3(972.5787f, 48.44172f, 116.162048f), new Vector3(0f, 0f, 53.2055f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 390197, 2813792322u, new Vector3(968.429443f, 49.45838f, 116.173409f), new Vector3(0f, 0f, 55.6747131f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 748083, 2813792322u, new Vector3(966.664734f, 49.8775635f, 116.174049f), new Vector3(0f, 0f, -67.12143f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 351539, 1744231373u, new Vector3(967.712158f, 51.3017235f, 116.163071f), new Vector3(0f, -0f, 162.550232f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 304691, 1744231373u, new Vector3(963.6891f, 48.83548f, 116.174995f), new Vector3(0f, -0f, 122.071747f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 141877, 2813792322u, new Vector3(963.135254f, 47.5327148f, 116.17411f), new Vector3(0f, 0f, 23.336607f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 218677, 2813792322u, new Vector3(964.917053f, 45.09533f, 116.173981f), new Vector3(0f, 0f, 71.03541f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 210704, 2813792322u, new Vector3(963.449158f, 44.12058f, 116.174149f), new Vector3(0f, 0f, -51.63864f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 594965, 1744231373u, new Vector3(967.9947f, 44.22169f, 116.162994f), new Vector3(0f, -0f, -134.856186f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 687668, 2813792322u, new Vector3(968.180359f, 42.37984f, 116.161896f), new Vector3(0f, 0f, -9.522388f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 505138, 1744231373u, new Vector3(964.7507f, 42.8963165f, 116.163116f), new Vector3(0f, 0f, 13.2174387f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 81452, 2813792322u, new Vector3(962.6148f, 39.1124077f, 116.161995f), new Vector3(0f, -0f, 147.0888f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 754995, 2813792322u, new Vector3(963.041748f, 37.6993752f, 116.162048f), new Vector3(0f, 0f, 57.2535324f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 218384, 1744231373u, new Vector3(961.3184f, 38.3363f, 116.162964f), new Vector3(0f, -0f, -95.39942f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 453684, 2813792322u, new Vector3(959.111f, 41.3933449f, 116.162056f), new Vector3(0f, -0f, 107.295387f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 290100, 1744231373u, new Vector3(957.558533f, 41.5813446f, 116.163086f), new Vector3(-0f, -0f, -24.482748f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 364595, 2813792322u, new Vector3(965.499451f, 34.1127777f, 116.161972f), new Vector3(0f, -0f, -94.94281f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 588341, 1744231373u, new Vector3(967.743835f, 31.7598438f, 116.17495f), new Vector3(-0f, -0f, -29.282423f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 154639, 2813792322u, new Vector3(966.25415f, 35.52155f, 116.1619f), new Vector3(0f, -0f, 124.691193f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 613136, 2813792322u, new Vector3(965.7286f, 31.85518f, 116.173927f), new Vector3(0f, -0f, -153.402481f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 306737, 1744231373u, new Vector3(964.3633f, 32.5824127f, 116.163017f), new Vector3(0f, -0f, -179.397659f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 706344, 1744231373u, new Vector3(964.197754f, 31.1623821f, 116.162971f), new Vector3(0f, -0f, -128.332092f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 196624, 1744231373u, new Vector3(958.0536f, 28.12741f, 116.174927f), new Vector3(-0f, -0f, -44.4689751f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 179728, 2813792322u, new Vector3(958.7996f, 29.5098515f, 116.173874f), new Vector3(0f, -0f, 110.529785f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 58419, 2813792322u, new Vector3(960.5164f, 32.7953644f, 116.161674f), new Vector3(0f, 0f, -41.812294f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 208912, 2813792322u, new Vector3(961.0259f, 33.8712654f, 116.161972f), new Vector3(0f, -0f, -153.334671f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 221712, 1744231373u, new Vector3(953.8201f, 21.6049042f, 116.163086f), new Vector3(0f, 0f, 68.8094559f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 670260, 2813792322u, new Vector3(952.268066f, 21.91498f, 116.161827f), new Vector3(0f, -0f, -115.109787f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 152371, 1744231373u, new Vector3(952.467468f, 23.6788845f, 116.16317f), new Vector3(0f, 0f, 49.21453f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 527156, 1744231373u, new Vector3(951.26f, 24.4557571f, 116.163208f), new Vector3(0f, -0f, 155.022491f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 319248, 1744231373u, new Vector3(950.8047f, 23.0212364f, 116.1631f), new Vector3(-0f, -0f, -54.5468864f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 769842, 1744231373u, new Vector3(954.2025f, 25.03298f, 116.163086f), new Vector3(-0f, -0f, -10.4998779f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 33075, 1744231373u, new Vector3(953.4429f, 26.4563332f, 116.163086f), new Vector3(0f, -0f, 169.618484f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 669744, 1744231373u, new Vector3(947.3065f, 20.20687f, 116.163139f), new Vector3(0f, -0f, 138.955643f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 439605, 1744231373u, new Vector3(946.2741f, 19.7833481f, 116.163139f), new Vector3(0f, -0f, -148.4444f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 752690, 2813792322u, new Vector3(948.5615f, 18.840229f, 116.161842f), new Vector3(0f, -0f, 142.569092f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 601140, 1744231373u, new Vector3(944.8018f, 20.6731758f, 116.162941f), new Vector3(0f, 0f, 59.5586929f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 698933, 1744231373u, new Vector3(943.866f, 19.6367168f, 116.162987f), new Vector3(-0f, -0f, -17.6733379f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 238352, 2813792322u, new Vector3(945.8878f, 17.099987f, 116.162346f), new Vector3(0f, 0f, 78.21075f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 136499, 1744231373u, new Vector3(944.294f, 16.83947f, 116.163216f), new Vector3(-0f, -0f, -62.25535f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 617524, 2813792322u, new Vector3(941.5722f, 14.70077f, 116.161865f), new Vector3(0f, 0f, 89.6291351f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 268083, 2813792322u, new Vector3(941.4226f, 13.5811672f, 116.161842f), new Vector3(0f, 0f, 38.23603f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 291123, 1744231373u, new Vector3(942.8442f, 11.6530046f, 116.163277f), new Vector3(-0f, -0f, -80.91504f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 388145, 1744231373u, new Vector3(943.8015f, 11.5519943f, 116.163208f), new Vector3(0f, -0f, 136.2508f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Casino Staff"), 7201, 3163733717u, new Vector3(945.3591f, 13.4878511f, 116.162178f), new Vector3(0f, 0f, 51.9999924f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 743220, 2813792322u, new Vector3(945.262451f, 8.351254f, 116.16188f), new Vector3(0f, -0f, -166.505981f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 222773, 2813792322u, new Vector3(944.0798f, 6.64124155f, 116.162094f), new Vector3(0f, -0f, 160.258f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 759845, 1744231373u, new Vector3(939.962769f, 5.947944f, 116.163017f), new Vector3(0f, 0f, 40.7252541f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 711457, 1744231373u, new Vector3(940.4257f, 3.536891f, 116.163261f), new Vector3(0f, -0f, 149.19043f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 252208, 2813792322u, new Vector3(939.070068f, 3.771157f, 116.162254f), new Vector3(0f, -0f, 170.992569f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 82735, 2813792322u, new Vector3(937.459351f, 3.00691462f, 116.162354f), new Vector3(0f, -0f, -106.767387f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 266034, 2813792322u, new Vector3(938.6281f, 7.333149f, 116.162071f), new Vector3(0f, -0f, -135.743881f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 763955, 1744231373u, new Vector3(935.84436f, 7.04731035f, 116.162964f), new Vector3(-0f, -0f, -25.1315937f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 529461, 1744231373u, new Vector3(937.16864f, 8.634564f, 116.162933f), new Vector3(0f, -0f, 115.079308f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 784945, 1744231373u, new Vector3(946.742432f, 3.69355774f, 116.162895f), new Vector3(0f, -0f, 102.851822f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 325647, 2813792322u, new Vector3(945.272339f, 3.14634275f, 116.1619f), new Vector3(0f, 0f, -75.85231f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 28206, 1744231373u, new Vector3(943.2348f, 5.57546854f, 116.162933f), new Vector3(-0f, -0f, -89.67265f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 288822, 1744231373u, new Vector3(944.8546f, 5.2891264f, 116.163063f), new Vector3(0f, 0f, 52.44903f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 353077, 1744231373u, new Vector3(942.0176f, 5.58166647f, 116.163078f), new Vector3(0f, 0f, 5.29229832f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Male 1"), 576784, 2813792322u, new Vector3(958.189941f, 22.9336128f, 116.162086f), new Vector3(0f, -0f, 109.390785f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Club Customer Female 1"), 677682, 1744231373u, new Vector3(957.496765f, 21.5844574f, 116.163162f), new Vector3(-0f, -0f, -34.8519058f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sf_prop_sf_dj_desk_02a"), 357942, 2767137151u, new Vector3(939.6764f, 12.8714266f, 115.164459f), new Vector3(0f, 0f, -83.830574f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "DJ Ryan S"), 621626, 3057799231u, new Vector3(939.2004f, 12.88689f, 116.160507f), new Vector3(-2.13445048E-07f, 1.06722183E-07f, -80.1961746f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_champ_closed_03"), 628801, 3129152371u, new Vector3(945.0363f, 14.1255407f, 116.418831f), new Vector3(0f, -0f, -97.96006f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_coke_block_01a"), 446021, 3896957328u, new Vector3(976.65f, 46.4957f, 115.965263f), new Vector3(0f, 0f, -48.31259f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_coke_block_01a"), 514370, 3896957328u, new Vector3(974.2928f, 46.88767f, 115.165253f), new Vector3(0f, 0f, 57.9392433f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_coke_block_01a"), 267590, 3896957328u, new Vector3(974.4369f, 46.7974319f, 115.495453f), new Vector3(0f, 0f, 57.9392433f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_coke_block_01a"), 775485, 3896957328u, new Vector3(974.8813f, 47.8721237f, 115.165253f), new Vector3(0f, 0f, 65.8722f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_coke_block_01a"), 49214, 3896957328u, new Vector3(974.9409f, 47.9419441f, 115.495453f), new Vector3(0f, 0f, 61.7776451f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_coke_block_01a"), 461126, 3896957328u, new Vector3(975.5467f, 48.8802567f, 115.165253f), new Vector3(0f, 0f, 55.63584f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_coke_block_01a"), 149054, 3896957328u, new Vector3(975.5193f, 48.8526535f, 115.495453f), new Vector3(0f, 0f, 57.5294724f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects PenthouseGuardsObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "High Security 2"), 516412, 691061163u, new Vector3(948.2907f, 5.237101f, 116.1661f), new Vector3(0f, -0f, 147.048065f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "High Security 2"), 331798, 691061163u, new Vector3(944.776855f, 22.0317745f, 116.1619f), new Vector3(0f, -0f, -115.613579f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "High Security 2"), 43571, 691061163u, new Vector3(962.503662f, 30.5627174f, 116.162712f), new Vector3(0f, 0f, 12.0321293f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "High Security 2"), 609589, 691061163u, new Vector3(969.1866f, 46.66825f, 116.162071f), new Vector3(0f, 0f, 62.6506844f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "High Security 2"), 656181, 691061163u, new Vector3(979.9797f, 61.466507f, 116.162292f), new Vector3(0f, -0f, 147.047729f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "High Security 2"), 685111, 691061163u, new Vector3(959.2283f, 39.4434662f, 116.161514f), new Vector3(-0f, -0f, -35.10769f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "High Security 2"), 405822, 691061163u, new Vector3(952.446655f, 26.6422539f, 116.166359f), new Vector3(0f, -0f, -128.924072f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "High Security 2"), 482609, 691061163u, new Vector3(967.6795f, 52.91208f, 116.162148f), new Vector3(0f, -0f, 150.877441f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects VIPKeycardObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_acc_pass_01a"), 610380, 4027835392u, new Vector3(976.575439f, 47.2235756f, 115.972435f), new Vector3(0f, -0f, -105.68335f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_acc_pass_01a"), 22613, 4027835392u, new Vector3(973.667236f, 70.0165939f, 116.225067f), new Vector3(0f, -0f, 111.426659f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_acc_pass_01a"), 487253, 4027835392u, new Vector3(949.651062f, 17.6728573f, 116.212624f), new Vector3(0f, -0f, -176.306839f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_acc_pass_01a"), 14166, 4027835392u, new Vector3(944.266357f, 13.7204943f, 116.212624f), new Vector3(0f, -0f, -95.7470856f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_acc_pass_01a"), 687438, 4027835392u, new Vector3(939.3374f, 11.6918316f, 116.075783f), new Vector3(0f, 0f, 60.85885f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_acc_pass_01a"), 503895, 4027835392u, new Vector3(949.9046f, 7.5905714f, 116.372536f), new Vector3(0f, 0f, 53.7014275f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_acc_pass_01a"), 661067, 4027835392u, new Vector3(976.7042f, 46.28458f, 116.302635f), new Vector3(0f, 0f, -20.714674f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_acc_pass_01a"), 131120, 4027835392u, new Vector3(954.0629f, 32.17262f, 115.768837f), new Vector3(0f, 0f, 29.1349049f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects HeadOfSecurityInteriorHouseObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_ld_id_card_01"), 407057, 292851939u, new Vector3(-1008.55664f, -477.166748f, 49.8617477f), new Vector3(-90f, -41.395504f, 0f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_mp_h_acc_artwalll_02"), 686874, 2866697883u, new Vector3(-1011.33185f, -475.399231f, 51.1736946f), new Vector3(0f, 0f, 29.5397511f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_mp_h_acc_artwalll_02"), 350740, 2866697883u, new Vector3(-1007.50653f, -473.21875f, 51.0263443f), new Vector3(0f, -0.01f, 29.8978767f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_mp_h_acc_artwalll_02"), 175132, 2866697883u, new Vector3(-1011.86511f, -477.4626f, 50.8882866f), new Vector3(0f, -0f, 119.4635f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_mp_h_acc_artwalll_02"), 518171, 2866697883u, new Vector3(-1009.67828f, -481.539124f, 51.19425f), new Vector3(0f, -0f, 119.718063f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_v_ilev_fh_heistdoor1"), 691493, 509504073u, new Vector3(-1010.76025f, -479.718567f, 50.50286f), new Vector3(0f, -0f, 119.561623f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_v_ilev_fh_heistdoor1"), 102436, 509504073u, new Vector3(-1009.28882f, -474.3086f, 50.47188f), new Vector3(0f, 0f, 27.9486561f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_v_ilev_fh_heistdoor1"), 452645, 509504073u, new Vector3(-1010.76025f, -479.718567f, 50.50286f), new Vector3(-0f, -0f, -61.43838f), 691493, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 178.999985f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "apa_v_ilev_fh_heistdoor1"), 217387, 509504073u, new Vector3(-1009.28882f, -474.3086f, 50.47188f), new Vector3(0f, -0f, -149.951218f), 102436, 0, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, -177.899887f))
		});

		public static MissionOjects BuyerObjects = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Martin Madrazo"), 779842, 1129928304u, new Vector3(1373.404f, 1147.536f, 113.755394f), new Vector3(0f, -0f, 93.37351f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Ignus"), 685117, 2850852987u, new Vector3(1365.483f, 1154.94434f, 113.12291f), new Vector3(0.0280211288f, 0.000513317238f, -123.839188f), 59, 88, 94, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Ignus"), 19265, 2850852987u, new Vector3(1369.04f, 1149.42224f, 113.122726f), new Vector3(0.02798262f, 0.0005350614f, -9.7051f), 117, 55, 73, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Ignus"), 777277, 2850852987u, new Vector3(1367.62476f, 1143.81116f, 113.122757f), new Vector3(0.02793009f, 0.00053891883f, -35.85855f), 9, 27, 62, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Ignus"), 146227, 2850852987u, new Vector3(1363.794f, 1139.79688f, 113.122665f), new Vector3(0.0275711324f, 0.000512044935f, -59.2993164f), 59, 88, 94, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static Vector3 MansionLoc = new Vector3(-1570.428f, 97.79581f, 50.54805f);

		public static Vector3 HeadOfSecurityHouseEnter = new Vector3(-1804.7f, 428.6f, 127.73f);

		public static Vector3 HeadOfSecurityHouseExit = new Vector3(-1002.828f, -477.98f, 49.02f);

		public static Vector3 CartelIntelLoc = new Vector3(-1371.14954f, 735.5286f, 183.51442f);

		public static Vector3 DjHouseEnter = new Vector3(373.75f, 427.84f, 144.6f);

		public static Vector3 DjHouseExit = new Vector3(373.74f, 423.54f, 144.9f);

		public static Vector3 PenthouseEnter = new Vector3(977.29f, 16.8f, 79.99f);

		public static Vector3 PenthouseExit = new Vector3(980.6f, 56.5f, 115.16f);

		public static Vector3 RoadblockLocationLoc = new Vector3(728.2622f, -2912.65723f, 5.89546442f);

		public int Penthouse_Style = 1;

		public int Penthouse_Media = 1;

		public int Penthouse_Spa = 1;

		public int Penthouse_Bar = 1;

		public int Penthouse_Colour = 1;

		public int Penthouse_Arcade;

		public int Penthouse_BarLight;

		public int Penthouse_PokerDealer;

		public int Penthouse_ExtraBedroom = 1;

		public static int CurrentCamera;

		public static Vehicle Phantom;

		public static Ped PhantomDriver;

		public static int PointOfNoReturn;

		public static bool StartedPointOfNoReturn;

		public static bool PhantomIsEscapingParty;

		public static int PhantomReRouteTimer;

		public static Vehicle Trailer;

		public static Ped CartelLeader;

		public static bool PayedforRoadBlock;

		public static bool CreatedCartelMeet;

		public static Vehicle HeadOfSecurityVeh;

		public static Ped HeadOfSecurity;

		public static Ped HeadOfSecurityDriver;

		public static int HosNewRouteTime;

		public static float HoSHackPerc;

		public static bool HackingHoSphone;

		public static bool HackedHoSPhone;

		public static int HackHoSTimer;

		public static bool AleartedPolice;

		public static Ped VehicleADriver;

		public static float VehicleAPrice;

		public static Ped VehicleBDriver;

		public static float VehicleBPrice;

		public static Ped VehicleCDriver;

		public static float VehicleCPrice;

		public static Ped VehicleDDriver;

		public static float VehicleDPrice;

		public static bool SpookedParty;

		public static bool InParty;

		public static bool ChangedVehiclesEscape;

		public static bool SpawnedParty;

		public static List<Blip> IntelMarkers = new List<Blip>();

		public static List<Blip> IntelMarkersRadiusArea = new List<Blip>();

		public static Blip TargetBlip;

		public static Blip TargetBlipArea;

		public static KeypadClass MainVaultKeypad;

		public static bool HackedMainKeypad;

		public static bool AccessingCameras;

		public static bool HackedComStation;

		public static bool VTMinigameOn;

		public static Prop ComStation;

		public static Prop VaultDoor;

		public static bool TheBiliionaresPartyHeistOn;

		public static Prop VaultKeycard;

		public static Prop VIP_Access_Pass;

		public static Prop DJ_Access_Pass;

		public static bool GotVaultKeycard = false;

		public static bool GotDJPass = false;

		public static bool GotVIP_Access_Pass = false;

		public static Blip DeliverVehBlip;

		public static Blip DeliverVehArea;

		public static List<string> DancesList = new List<string>
		{
			"anim@amb@nightclub_island@dancers@beachdance@,mi_loop_m03", "anim@amb@nightclub_island@dancers@beachdance@,mi_loop_m04", "anim@amb@nightclub_island@dancers@beachdance@,mi_loop_m05", "anim@amb@nightclub_island@dancers@beachdance@,mi_loop_m04", "anim@amb@nightclub_island@dancers@beachdance@,ti_loop_m01", "anim@amb@nightclub_island@dancers@beachdance@,ti_loop_m02", "anim@amb@nightclub_island@dancers@beachdance@,ti_loop_m03", "anim@amb@nightclub_island@dancers@beachdance@,ti_loop_m04", "anim@amb@nightclub_island@dancers@beachdance@,hi_loop_f01", "anim@amb@nightclub_island@dancers@beachdance@,hi_loop_f02",
			"anim@amb@nightclub_island@dancers@beachdance@,li_loop_m01", "anim@amb@nightclub_island@dancers@beachdance@,li_loop_m02", "anim@amb@nightclub_island@dancers@beachdance@,li_loop_m03", "anim@amb@nightclub_island@dancers@beachdance@,li_loop_m04", "anim@amb@nightclub_island@dancers@beachdance@,li_loop_f01", "anim@amb@nightclub_island@dancers@beachdance@,li_loop_f02"
		};

		public bool inDjHouse;

		public bool inPenthouse;

		public bool inHoSHouse;

		public static HeistStage CurrentHeistStage;

		public static int EscapeStage = 0;

		public static bool TargetVehiclesEscaping;

		public List<string> Animsfound = new List<string>();

		public void DrawProgressBar_Advanced(string Name, int Slot, float CrntValue, float MaxValue, Color MarkerColor, bool ShowBarLines)
		{
			float num = 0f;
			float num2 = 0f;
			if (Slot == 1)
			{
				num = 0.89f;
				num2 = 0.97f;
			}
			if (Slot == 2)
			{
				num = 0.89f;
				num2 = 0.94f;
			}
			if (Slot == 3)
			{
				num = 0.89f;
				num2 = 0.91f;
			}
			if (Slot == 4)
			{
				num = 0.89f;
				num2 = 0.88f;
			}
			if (Slot == 5)
			{
				num = 0.89f;
				num2 = 0.85f;
			}
			if (Slot == 6)
			{
				num = 0.89f;
				num2 = 0.82f;
			}
			if (Slot == 7)
			{
				num = 0.89f;
				num2 = 0.79f;
			}
			if (Slot == 8)
			{
				num = 0.89f;
				num2 = 0.76f;
			}
			drawSprite2("timerbars", "all_black_bg", num, num2, 0.21f, 0.04f, 255, 255, 255, 255);
			drawSprite2("timerbars", "all_black_bg", num - 0.1f, num2, 0.21f, 0.04f, 255, 255, 255, 255);
			drawSprite2("timerbars", "damagebarfill_128", num + 0.05f, num2, 0.08f, 0.03f, MarkerColor.R, MarkerColor.G, MarkerColor.B, 130);
			drawSprite2("timerbars", "damagebarfill_128", progressxcoord(CrntValue / MaxValue), num2, progresswidth(CrntValue / MaxValue), 0.03f, MarkerColor.R, MarkerColor.G, MarkerColor.B, 255);
			if (ShowBarLines)
			{
				drawSprite2("timerbar_lines", "LineMarker20_128", num + 0.05f, num2, 0.08f, 0.03f, 0, 0, 0, 255);
				drawSprite2("timerbar_lines", "LineMarker40_128", num + 0.05f, num2, 0.08f, 0.03f, 0, 0, 0, 255);
				drawSprite2("timerbar_lines", "LineMarker60_128", num + 0.05f, num2, 0.08f, 0.03f, 0, 0, 0, 255);
				drawSprite2("timerbar_lines", "LineMarker80_128", num + 0.05f, num2, 0.08f, 0.03f, 0, 0, 0, 255);
			}
			drawText(Name, num - 0.1f, num2 - 0.016f, 0.3f, 255, 255, 255);
		}

		private float progressxcoord(float percent)
		{
			float num = 0.04f * percent;
			return 0.9f + num;
		}

		private float progressxcoord(float X, float percent)
		{
			float num = 0.04f * percent;
			return X + num;
		}

		private float progresswidth(float percent)
		{
			return 0.08f * percent;
		}

		public static Vector3 GenerateSpawnPos(Vector3 desiredPos, Nodetype roadtype, bool sidewalk)
		{
			Vector3 zero = Vector3.Zero;
			bool flag = false;
			OutputArgument outputArgument = new OutputArgument();
			int num = 1;
			int num2 = 0;
			if (roadtype == Nodetype.AnyRoad)
			{
				num2 = 1;
			}
			if (roadtype == Nodetype.Road)
			{
				num2 = 0;
			}
			if (roadtype == Nodetype.Offroad)
			{
				num2 = 1;
				flag = true;
			}
			if (roadtype == Nodetype.Water)
			{
				num2 = 3;
			}
			int num3 = Function.Call<int>(Hash.GET_NTH_CLOSEST_VEHICLE_NODE_ID, desiredPos.X, desiredPos.Y, desiredPos.Z, num, num2, 300f, 300f);
			if (flag)
			{
				while (!Function.Call<bool>(Hash.GET_VEHICLE_NODE_IS_SWITCHED_OFF, num3) && num < 500)
				{
					num++;
					num3 = Function.Call<int>(Hash.GET_NTH_CLOSEST_VEHICLE_NODE_ID, desiredPos.X, desiredPos.Y, desiredPos.Z, num, num2, 300f, 300f);
				}
			}
			Function.Call(Hash.GET_VEHICLE_NODE_POSITION, num3, outputArgument);
			zero = outputArgument.GetResult<Vector3>();
			if (sidewalk)
			{
				zero = World.GetNextPositionOnSidewalk(zero);
			}
			return zero;
		}

		public static Vector3 ProcessNewPos(Vector3 CurrentPos)
		{
			if (Function.Call<bool>(Hash.IS_USING_KEYBOARD_AND_MOUSE, 0) && !Lock)
			{
				FreeCamSpeed = ((!Game.IsControlPressed(Control.Sprint)) ? DefaultSpeed : ShiftSpeed);
				OffsetRotX -= (float)((double)Game.GetControlValueNormalized(Control.LookUpDown) * (double)Precision * 8.0);
				OffsetRotZ -= (float)((double)Game.GetControlValueNormalized(Control.LookLeftRight) * (double)Precision * 8.0);
			}
			if ((double)OffsetRotX > 90.0)
			{
				OffsetRotX = 90f;
			}
			else if ((double)OffsetRotX < -90.0)
			{
				OffsetRotX = -90f;
			}
			return CurrentPos;
		}

		public void ShowCameraControlls()
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int num = Convert.ToInt32(absoluteX / 2f);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 175, 0), "");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 174, 0), "    Switch Cameras");
			if (Game.LastInputMethod == InputMethod.GamePad)
			{
				scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 1, 0), "");
				scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 2, 0), "");
				scaleform.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 198, 0), "Controll Camera");
			}
			if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
			{
				scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 35, 0), "");
				scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 33, 0), "");
				scaleform.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 34, 0), "");
				scaleform.CallFunction("SET_DATA_SLOT", 5, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 32, 0), "Controll Camera");
			}
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public float GetRatio(float num)
		{
			return 0.5f - (0.5f - num) / screenaspectratio;
		}

		private void drawSpriteRTA2(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha, float Heading)
		{
			screenaspectratio = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
			float num = (float)(2.778 / (double)screenaspectratio);
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, GetRatio(screenX), screenY, width / 1280f * num, height / 1280f, Heading, r, g, b, alpha, 0);
			}
		}

		public void TEXT(string text, PointF Pos, float Size, Color Colour, GTA.UI.Font font)
		{
			ScaledText scaledText = new ScaledText(Pos, "");
			scaledText.Text = text;
			scaledText.Color = Colour;
			scaledText.Font = font;
			scaledText.Scale = Size;
			scaledText.Alignment = Alignment.Center;
			scaledText.Draw();
		}

		public void TEXTURE(string Textdict, string Textname, PointF Pos, SizeF Size, float Rot, Color Colour)
		{
			ScaledTexture scaledTexture = new ScaledTexture(Textdict, Textname);
			scaledTexture.Dictionary = Textdict;
			scaledTexture.Texture = Textname;
			scaledTexture.Position = Pos;
			scaledTexture.Size = Size;
			scaledTexture.Color = Colour;
			scaledTexture.Draw();
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

		public static void ShowTextInfoBar(string Action, string Amt, int Slot, Vector2 TextOffset1, Vector2 TextOffset2, bool Money)
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

		public static Model RequestModel(uint Name)
		{
			Model result = new Model((int)Name);
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

		public void LOADIPL(string IPL)
		{
			try
			{
				Function.Call(Hash.REQUEST_IPL, IPL);
			}
			catch
			{
				GTA.UI.Screen.ShowSubtitle("~r~IPL CAll OUSIDE MAIN LOOP~w~{" + IPL + "}");
			}
		}

		public void RefreshPenthouse()
		{
			Function.Call(Hash.REMOVE_IPL, "vw_int_placement_vw_interior_1_dlc_casino_apart_milo_");
			LOADIPL("vw_int_placement_vw_interior_1_dlc_casino_apart_milo_");
			int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 976.6364, 70.29476, 115.1641);
			Function.Call(Hash.REFRESH_INTERIOR, num);
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Tint_Shell");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_01");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_02");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_03");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_04");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_05");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_06");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_07");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_08");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_09");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Spa_Bar_Open");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Spa_Bar_Closed");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Media_Bar_Open");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Media_Bar_Closed");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Dealer");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_NoDealer");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Arcade_Retro");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Arcade_Modern");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Bar_Clutter");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Clutter_01");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Clutter_02");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Clutter_03");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_light_0");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_light_01");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_light_02");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_party_0");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_party_1");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_party_2");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_party_after");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_GUEST_BLOCKER");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_LOUNGE_BLOCKER");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_OFFICE_BLOCKER");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_CINE_BLOCKER");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_SPA_BLOCKER");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_BAR_BLOCKER");
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Tint_Shell");
			if (Penthouse_Style == 0)
			{
				Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Tint_Shell", Penthouse_Colour);
			}
			if (Penthouse_Style == 1)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_01");
				if (Penthouse_Style != 0)
				{
					Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_01", Penthouse_Colour);
					Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Tint_Shell", Penthouse_Colour);
				}
			}
			if (Penthouse_Style == 2)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_02");
				if (Penthouse_Style != 0)
				{
					Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_02", Penthouse_Colour);
					Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Tint_Shell", Penthouse_Colour);
				}
			}
			if (Penthouse_Style == 3)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_03");
				if (Penthouse_Style != 0)
				{
					Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_03", Penthouse_Colour);
					Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Tint_Shell", Penthouse_Colour);
				}
			}
			if (Penthouse_Style == 4)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_04");
				if (Penthouse_Style != 0)
				{
					Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_04", Penthouse_Colour);
					Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Tint_Shell", Penthouse_Colour);
				}
			}
			if (Penthouse_Style == 5)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_05");
				if (Penthouse_Style != 0)
				{
					Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_05", Penthouse_Colour);
					Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Tint_Shell", Penthouse_Colour);
				}
			}
			if (Penthouse_Style == 6)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_06");
				if (Penthouse_Style != 0)
				{
					Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_06", Penthouse_Colour);
					Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Tint_Shell", Penthouse_Colour);
				}
			}
			if (Penthouse_Style == 7)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_07");
				if (Penthouse_Style != 0)
				{
					Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_07", Penthouse_Colour);
					Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Tint_Shell", Penthouse_Colour);
				}
			}
			if (Penthouse_Style == 8)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_08");
				if (Penthouse_Style != 0)
				{
					Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_08", Penthouse_Colour);
					Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Tint_Shell", Penthouse_Colour);
				}
			}
			if (Penthouse_Style == 9)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Pattern_09");
				if (Penthouse_Style != 0)
				{
					Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Pattern_09", Penthouse_Colour);
					Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Tint_Shell", Penthouse_Colour);
				}
			}
			if (Penthouse_Spa == 1)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Spa_Bar_Open");
			}
			if (Penthouse_Spa == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Spa_Bar_Closed");
			}
			if (Penthouse_Media == 1)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Media_Bar_Open");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Media_Bar_Open", Penthouse_Colour);
			if (Penthouse_Media == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Media_Bar_Closed");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Media_Bar_Closed", Penthouse_Colour);
			if (Penthouse_PokerDealer == 1)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Dealer");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_Dealer", Penthouse_Colour);
			if (Penthouse_PokerDealer == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_NoDealer");
			}
			Function.Call((Hash)3590001984630001091uL, num, "Set_Pent_NoDealer", Penthouse_Colour);
			if (Penthouse_Arcade == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Arcade_Retro");
			}
			if (Penthouse_Arcade == 1)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_Arcade_Modern");
			}
			if (Penthouse_BarLight == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_light_0");
			}
			if (Penthouse_BarLight == 1)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_light_01");
			}
			if (Penthouse_BarLight == 2)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_light_02");
			}
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_party_0");
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "set_pent_bar_party_0");
			if (Penthouse_ExtraBedroom == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_GUEST_BLOCKER");
			}
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_LOUNGE_BLOCKER");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_OFFICE_BLOCKER");
			if (Penthouse_Media == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_CINE_BLOCKER");
			}
			if (Penthouse_Spa == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_SPA_BLOCKER");
			}
			if (Penthouse_Bar == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Pent_BAR_BLOCKER");
			}
		}

		public void UnlockpenthouseDoors()
		{
			Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_lounge_01a"), 958.5333f, 24.8082f, 116.81f, true, 90f, false);
			Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_lounge_01a"), 956.444f, 26.1138f, 116.81f, true, -90f, false);
			Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_lounge_01a"), 973.8256f, 54.3764f, 116.81f, true, 90f, false);
			Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_door_lounge_01a"), 975.9149f, 53.0709f, 116.81f, true, -90f, false);
			Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, Function.Call<int>(Hash.GET_HASH_KEY, "vw_prop_vw_panel_off_door_01"), 969.5818f, 47.0427f, 116.2636f, true, -90f, false);
		}

		public static void ATTACH_ENTITY_TO_ENTITY(Entity entity1, Entity entity2, int boneIndex, float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, bool p9, bool useSoftPinning, bool collision, bool isPed, int vertexIndex, bool fixedRot)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, entity1, entity2, boneIndex, xPos, yPos, zPos, xRot, yRot, zRot, p9, useSoftPinning, collision, isPed, vertexIndex, fixedRot);
		}

		public TheBillionaresParty()
		{
			base.Tick += OnTick;
		}

		public void DrawLine(Vector3 From, Vector3 To, Vector3 RGB)
		{
			Function.Call(Hash.DRAW_LINE, From.X, From.Y, From.Z, To.X, To.Y, To.Z, RGB.X, RGB.Y, RGB.Z, 255);
		}

		public void DrawBox(Vehicle Base, Vector3 OriginalPos, float SizeX, float SizeY)
		{
			Vector3 offsetPosition = Base.GetOffsetPosition(new Vector3(OriginalPos.X + SizeX, OriginalPos.Y + SizeY, OriginalPos.Z));
			offsetPosition = new Vector3(offsetPosition.X, offsetPosition.Y, offsetPosition.Z - 0.05f);
			Vector3 offsetPosition2 = Base.GetOffsetPosition(new Vector3(OriginalPos.X - SizeX, OriginalPos.Y + SizeY, OriginalPos.Z));
			offsetPosition2 = new Vector3(offsetPosition2.X, offsetPosition2.Y, offsetPosition2.Z - 0.05f);
			Vector3 offsetPosition3 = Base.GetOffsetPosition(new Vector3(OriginalPos.X + SizeX, OriginalPos.Y - SizeY, OriginalPos.Z));
			offsetPosition3 = new Vector3(offsetPosition3.X, offsetPosition3.Y, offsetPosition3.Z - 0.05f);
			Vector3 offsetPosition4 = Base.GetOffsetPosition(new Vector3(OriginalPos.X - SizeX, OriginalPos.Y - SizeY, OriginalPos.Z));
			offsetPosition4 = new Vector3(offsetPosition4.X, offsetPosition4.Y, offsetPosition4.Z - 0.05f);
			World.DrawMarker(MarkerType.Sphere, offsetPosition, Vector3.Zero, Vector3.Zero, new Vector3(0.025f, 0.025f, 0.025f), Color.FromArgb(0, 0, 255));
			World.DrawMarker(MarkerType.Sphere, offsetPosition2, Vector3.Zero, Vector3.Zero, new Vector3(0.025f, 0.025f, 0.025f), Color.FromArgb(0, 0, 255));
			World.DrawMarker(MarkerType.Sphere, offsetPosition3, Vector3.Zero, Vector3.Zero, new Vector3(0.025f, 0.025f, 0.025f), Color.FromArgb(0, 0, 255));
			World.DrawMarker(MarkerType.Sphere, offsetPosition4, Vector3.Zero, Vector3.Zero, new Vector3(0.025f, 0.025f, 0.025f), Color.FromArgb(0, 0, 255));
			DrawLine(offsetPosition3, offsetPosition4, new Vector3(0f, 0f, 255f));
			DrawLine(offsetPosition, offsetPosition2, new Vector3(0f, 0f, 255f));
			DrawLine(offsetPosition, offsetPosition3, new Vector3(0f, 0f, 255f));
			DrawLine(offsetPosition2, offsetPosition4, new Vector3(0f, 0f, 255f));
		}

		public static void EndHeist()
		{
			if (MainVaultKeypad != null && MainVaultKeypad.Keypad != null && MainVaultKeypad.Keypad.AttachedBlip != null)
			{
				MainVaultKeypad.Keypad.AttachedBlip.Delete();
			}
			ManageRobberyAnims.DeleteLoot();
			TheBiliionaresPartyHeistOn = false;
			if (Function.Call<bool>(Hash.IS_STREAM_PLAYING))
			{
				Function.Call(Hash.STOP_STREAM);
			}
			SpookedParty = false;
			ChangedVehiclesEscape = false;
			InParty = false;
			StartedPointOfNoReturn = false;
			PhantomDelivered = false;
			PhantomTaken = false;
			ManageRobberyAnims.CurrentPay = 0;
			TargetVehicleA_Delivered = false;
			TargetVehicleB_Delivered = false;
			TargetVehicleC_Delivered = false;
			TargetVehicleD_Delivered = false;
			if (DeliverVehBlip != null)
			{
				DeliverVehBlip.Delete();
			}
			if (DeliverVehArea != null)
			{
				DeliverVehArea.Delete();
			}
			BuyerObjects.DeleteEntites();
			PayedforRoadBlock = false;
			TargetVehiclesEscaping = false;
			TargetVehicleA_NoLongerAvailable = false;
			TargetVehicleB_NoLongerAvailable = false;
			TargetVehicleC_NoLongerAvailable = false;
			TargetVehicleD_NoLongerAvailable = false;
			EscapeStage = 0;
			if (TargetVehicleADriver != null)
			{
				TargetVehicleADriver.Delete();
			}
			if (TargetVehicleBDriver != null)
			{
				TargetVehicleBDriver.Delete();
			}
			if (TargetVehicleCDriver != null)
			{
				TargetVehicleCDriver.Delete();
			}
			if (TargetVehicleDDriver != null)
			{
				TargetVehicleDDriver.Delete();
			}
			SpookedParty = false;
			foreach (SmartGuards.SmartAiV2 cartelGuard in CartelGuards)
			{
				if (cartelGuard != null && cartelGuard.AIPed != null)
				{
					cartelGuard.AIPed.Delete();
				}
			}
			foreach (SmartGuards.SmartAiV2 hosGuard in HosGuards)
			{
				if (hosGuard != null && hosGuard.AIPed != null)
				{
					hosGuard.AIPed.Delete();
				}
			}
			foreach (SmartGuards.SmartAiV2 mansionGuard in MansionGuards)
			{
				if (mansionGuard != null && mansionGuard.AIPed != null)
				{
					mansionGuard.AIPed.Delete();
				}
			}
			foreach (SmartGuards.SmartAiV2 penthouseGuard in PenthouseGuards)
			{
				if (penthouseGuard != null && penthouseGuard.AIPed != null)
				{
					penthouseGuard.AIPed.Delete();
				}
			}
			CurrentHeistStage = HeistStage.Scout;
			PhantomReRouteTimer = 0;
			PhantomIsEscapingParty = false;
			if (VehicleADriver != null)
			{
				VehicleADriver.Delete();
			}
			if (VehicleBDriver != null)
			{
				VehicleBDriver.Delete();
			}
			if (VehicleCDriver != null)
			{
				VehicleCDriver.Delete();
			}
			if (VehicleDDriver != null)
			{
				VehicleDDriver.Delete();
			}
			if (PhantomDriver != null)
			{
				PhantomDriver.Delete();
			}
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
			if (TargetBlip != null)
			{
				TargetBlip.Delete();
			}
			if (TargetBlipArea != null)
			{
				TargetBlipArea.Delete();
			}
			MansionVaultObjects.DeleteEntites();
			VaultLootObjects.DeleteEntites();
			AttachedVehicleAtoTrailer = false;
			AttachedVehicleBtoTrailer = false;
			AttachedVehicleCtoTrailer = false;
			AttachedVehicleDtoTrailer = false;
			AleartedPolice = false;
			HackedHoSPhone = false;
			FingerprintHack.SecuroServ_UploadProgress = 0f;
			HackHoSTimer = 0;
			HackingHoSphone = false;
			HoSHackPerc = 0f;
			HosNewRouteTime = 0;
			if (HeadOfSecurity != null)
			{
				HeadOfSecurity.Delete();
			}
			if (HeadOfSecurityDriver != null)
			{
				HeadOfSecurityDriver.Delete();
			}
			CreatedCartelMeet = false;
			Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "default");
			if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
			{
				Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
			}
			Function.Call(Hash.CLEAR_FOCUS);
			VTMinigameOn = false;
			if (SecCam != null)
			{
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
				Game.Player.Character.IsInvincible = false;
				Game.Player.Character.IsVisible = true;
				SecCam.Delete();
			}
			CurrentCamera = 0;
			AccessingCameras = false;
			HackedComStation = false;
			CreatedCartelObjects = false;
			CreatedHoSExteriorObjects = false;
			CreatedHoSInteriorObjects = false;
			CreatedRoadBlockObjects = false;
			CreatedHeadofSecurityVehicle = false;
			GotVaultKeycard = false;
			GotDJPass = false;
			GotVIP_Access_Pass = false;
			GotGrinder = false;
			GotC4 = false;
			C4ChargesCollected = 0;
			GotThermite = false;
			ThermiteChargesCollected = 0;
			GotKeycard = false;
			CurrentPay = 0;
			LocalTake = 0;
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
			RoadBlockObjects.DeleteEntites();
			PenthouseGuardsObjects.DeleteEntites();
			VaultLootObjects.DeleteEntites();
			MansionCCTVsObjects.DeleteEntites();
			MansionGuardsObjects.DeleteEntites();
			MansionObjects.DeleteEntites();
			MansionSpeakerObjects.DeleteEntites();
			TargetVehicles.DeleteEntites();
			MansionVaultObjects.DeleteEntites();
			DJKeycardObjects.DeleteEntites();
			CartelObjects.DeleteEntites();
			HeadofSecurityObjects.DeleteEntites();
			PenthouseObjects.DeleteEntites();
			VIPKeycardObjects.DeleteEntites();
			CarCarrierObjects.DeleteEntites();
			HeadOfSecurityInteriorHouseObjects.DeleteEntites();
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

		public void CheckKeypad(KeypadClass KeyPad)
		{
			if (!KeyPad.SpawnedKeypad && !KeyPad.SpawnedKeypad)
			{
				KeyPad.SpawnedKeypad = true;
				KeyPad.Keypad = GlobalHelper.CreateProp(RequestModel("ch_prop_fingerprint_scanner_01c"), KeyPad.KeypadPos, new Vector3(0f, 0f, KeyPad.KeypadRot), false, false);
				KeyPad.Keypad.IsPositionFrozen = true;
				if (KeyPad.Keypad.AttachedBlip == null)
				{
					KeyPad.Keypad.AddBlip();
					KeyPad.Keypad.AttachedBlip.Sprite = BlipSprite.Keypad2;
					KeyPad.Keypad.AttachedBlip.Scale = 0.75f;
					KeyPad.Keypad.AttachedBlip.IsShortRange = true;
				}
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
						KeyPad.AwaitingHackGUI = true;
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

		public bool IsInTargetVehicle()
		{
			bool result = false;
			if (Game.Player.Character.CurrentVehicle != null && (Game.Player.Character.CurrentVehicle == TargetVehicleA || Game.Player.Character.CurrentVehicle == TargetVehicleB || Game.Player.Character.CurrentVehicle == TargetVehicleC || Game.Player.Character.CurrentVehicle == TargetVehicleD))
			{
				result = true;
			}
			if (Game.Player.Character.CurrentVehicle != null && !Game.Player.Character.CurrentVehicle.Exists())
			{
				result = false;
			}
			return result;
		}

		public static void StartHeist()
		{
			ManageRobberyAnims.Heist_Average_Pay = (float)(HeistMinPay + HeistMaxPay) / 2f;
			Function.Call(Hash.BEGIN_TEXT_COMMAND_BUSYSPINNER_ON, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "Loading The Billionaires Party Heist, Please Wait");
			Function.Call(Hash.END_TEXT_COMMAND_BUSYSPINNER_ON, 5);
			EndHeist();
			BuyerObjects.CreateEntites();
			for (int i = 1; i < BuyerObjects.Entities.Count; i++)
			{
				BuyerObjects.Entities[i].Entity.IsPositionFrozen = true;
				BuyerObjects.Entities[i].Entity.Position = BuyerObjects.Entities[i].Position;
				BuyerObjects.Entities[i].Entity.Rotation = BuyerObjects.Entities[i].Rotation;
				BuyerObjects.Entities[i].Entity.Opacity = 0;
				BuyerObjects.Entities[i].Entity.IsCollisionEnabled = false;
			}
			Ped ped = (Ped)BuyerObjects.Entities[0].Entity;
			ped.BlockPermanentEvents = true;
			ped.RelationshipGroup = 2;
			ped.IsInvincible = true;
			ped.CanBeTargetted = false;
			TargetBlip = GlobalHelper.CreateBlip(MansionLoc);
			TargetBlip.Sprite = BlipSprite.BeachParty;
			TargetBlip.Color = BlipColor.Yellow;
			TargetBlip.Name = "Billionaires Party";
			TargetBlip.IsShortRange = true;
			TargetBlipArea = GlobalHelper.CreateBlip(MansionLoc, 100f);
			TargetBlipArea.Color = BlipColor.Yellow;
			TargetBlipArea.Name = "Billionaires Party";
			TargetBlipArea.IsShortRange = true;
			TargetBlipArea.Alpha = 144;
			DeliverVehBlip = GlobalHelper.CreateBlip(BuyerObjects.Entities[0].Position);
			DeliverVehBlip.Sprite = BlipSprite.VehicleWarehouse;
			DeliverVehBlip.Color = BlipColor.Yellow;
			DeliverVehBlip.Name = "Deliver Target Vehicles";
			DeliverVehBlip.Scale = 0.75f;
			DeliverVehBlip.IsShortRange = true;
			DeliverVehArea = GlobalHelper.CreateBlip(BuyerObjects.Entities[0].Position, 20f);
			DeliverVehArea.Color = BlipColor.Yellow;
			DeliverVehArea.Name = "Deliver Target Vehicles";
			DeliverVehArea.IsShortRange = true;
			DeliverVehArea.Alpha = 0;
			DeliverVehBlip.Alpha = 0;
			SpawnedParty = true;
			MansionCCTVsObjects.CreateEntites();
			foreach (ObjectPlacementMap entity in MansionCCTVsObjects.Entities)
			{
				CamerasPosR.Add(new VectorRotation(entity.Position, entity.Rotation.Z));
			}
			MansionGuardsObjects.CreateEntites();
			MansionObjects.CreateEntites();
			foreach (ObjectPlacementMap entity2 in MansionObjects.Entities)
			{
				if (entity2.Entity != null && entity2.Entity.Model.IsPed)
				{
					Ped ped2 = (Ped)entity2.Entity;
					string[] array = DancesList[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, DancesList.Count)].Split(new char[1] { ',' });
					ped2.Task.PlayAnimation(array[0], array[1], 1f, -1, AnimationFlags.Loop);
				}
			}
			MansionSpeakerObjects.CreateEntites();
			foreach (ObjectPlacementMap entity3 in MansionSpeakerObjects.Entities)
			{
				if (entity3.Entity != null)
				{
					string text = "Mix1_Sample";
					Random random = new Random();
					int num = random.Next(0, 100);
					if (num <= 25)
					{
						text = "Mix1_Sample";
					}
					if (num > 25 && num <= 50)
					{
						text = "Mix2_Sample";
					}
					if (num > 50 && num <= 75)
					{
						text = "Mix3_Sample";
					}
					if (num > 75 && num <= 100)
					{
						text = "Mix4_Sample";
					}
					Function.Call(Hash.LOAD_STREAM, text, "DLC_Battle_Blimp_Promotion_Sounds");
					Function.Call(Hash.SET_AUDIO_FLAG, "DisableReplayScriptStreamRecording", true);
					Function.Call(Hash.PLAY_STREAM_FROM_OBJECT, (Prop)entity3.Entity);
					Function.Call(Hash.SET_AUDIO_FLAG, "DisableReplayScriptStreamRecording", false);
				}
			}
			TargetVehicles.CreateEntites();
			Phantom = (Vehicle)MansionObjects.Entities[0].Entity;
			Trailer = (Vehicle)MansionObjects.Entities[1].Entity;
			foreach (ObjectPlacementMap entity4 in MansionGuardsObjects.Entities)
			{
				if (entity4.Entity != null)
				{
					MansionGuards.Add(new SmartGuards.SmartAiV2((Ped)entity4.Entity, SmartGuards.EnemyGroup.High_End_Security));
				}
			}
			Function.Call(Hash.ATTACH_VEHICLE_TO_TRAILER, Phantom, Trailer, 1065353216);
			Trailer.Doors[VehicleDoorIndex.Trunk].Open(false, true);
			MansionVaultObjects.CreateEntites();
			VaultLootObjects.CreateEntites();
			int index = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, VIPKeycardObjects.Entities.Count);
			if (VIPKeycardObjects.Entities[index].Entity != null)
			{
				VIP_Access_Pass = (Prop)VIPKeycardObjects.Entities[index].Entity;
			}
			int num2 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, TargetVehicles.Entities.Count);
			if (TargetVehicles.Entities[num2].Entity != null)
			{
				TargetVehicleA = (Vehicle)TargetVehicles.Entities[num2].Entity;
				TargetVehicleA.AddBlip();
				TargetVehicleA.AttachedBlip.Sprite = BlipSprite.SportsCar;
				TargetVehicleA.AttachedBlip.Color = BlipColor.Blue;
				TargetVehicleA.AttachedBlip.Name = "Target Vehicle";
				VehicleAPrice = TargetVehiclePrices[num2];
			}
			int num3 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, TargetVehicles.Entities.Count);
			while (num2 == num3)
			{
				num3 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, TargetVehicles.Entities.Count);
			}
			if (TargetVehicles.Entities[num3].Entity != null)
			{
				TargetVehicleB = (Vehicle)TargetVehicles.Entities[num3].Entity;
				TargetVehicleB.AddBlip();
				TargetVehicleB.AttachedBlip.Sprite = BlipSprite.SportsCar;
				TargetVehicleB.AttachedBlip.Color = BlipColor.Blue;
				TargetVehicleB.AttachedBlip.Name = "Target Vehicle";
				VehicleBPrice = TargetVehiclePrices[num3];
			}
			int num4 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, TargetVehicles.Entities.Count);
			while (num4 == num3 || num4 == num2)
			{
				num4 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, TargetVehicles.Entities.Count);
			}
			if (TargetVehicles.Entities[num4].Entity != null)
			{
				TargetVehicleC = (Vehicle)TargetVehicles.Entities[num4].Entity;
				TargetVehicleC.AddBlip();
				TargetVehicleC.AttachedBlip.Sprite = BlipSprite.SportsCar;
				TargetVehicleC.AttachedBlip.Color = BlipColor.Blue;
				TargetVehicleC.AttachedBlip.Name = "Target Vehicle";
				VehicleCPrice = TargetVehiclePrices[num4];
			}
			int num5 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, TargetVehicles.Entities.Count);
			while (num5 == num4 || num5 == num3 || num5 == num2)
			{
				num5 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, TargetVehicles.Entities.Count);
			}
			if (TargetVehicles.Entities[num5].Entity != null)
			{
				TargetVehicleD = (Vehicle)TargetVehicles.Entities[num5].Entity;
				TargetVehicleD.AddBlip();
				TargetVehicleD.AttachedBlip.Sprite = BlipSprite.SportsCar;
				TargetVehicleD.AttachedBlip.Color = BlipColor.Blue;
				TargetVehicleD.AttachedBlip.Name = "Target Vehicle";
				VehicleDPrice = TargetVehiclePrices[num5];
			}
			foreach (ObjectPlacementMap entity5 in VaultLootObjects.Entities)
			{
				if (entity5.Entity != null)
				{
					ManageRobberyAnims.AddPropAsLoot((Prop)entity5.Entity);
				}
			}
			MainVaultKeypad = new KeypadClass((Prop)MansionVaultObjects.Entities[0].Entity, 2);
			VaultDoor = (Prop)MansionVaultObjects.Entities[1].Entity;
			index = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, HeadofSecurityVehicleSpawnsObjects.Entities.Count);
			HeadofSecurityVehicleSpawnsObjects.Entities[index].CreateObjatCoords();
			if (HeadofSecurityVehicleSpawnsObjects.Entities[index].Entity != null)
			{
				HeadOfSecurityVeh = (Vehicle)HeadofSecurityVehicleSpawnsObjects.Entities[index].Entity;
				HeadOfSecurityVeh.Position = HeadofSecurityVehicleSpawnsObjects.Entities[index].Position;
			}
			HeadOfSecurity = GlobalHelper.CreatePed(RequestModel("CSB_Security_A"), HeadOfSecurityVeh.Position, 0f);
			HeadOfSecurityDriver = GlobalHelper.CreatePed(RequestModel("IG_Paper"), HeadOfSecurityVeh.Position, 0f);
			HeadOfSecurityDriver.SetIntoVehicle(HeadOfSecurityVeh, VehicleSeat.Driver);
			HeadOfSecurity.BlockPermanentEvents = true;
			HeadOfSecurity.SetIntoVehicle(HeadOfSecurityVeh, VehicleSeat.RightRear);
			HeadOfSecurityDriver.BlockPermanentEvents = true;
			HeadOfSecurityDriver.Task.CruiseWithVehicle(HeadOfSecurityVeh, 60f);
			HeadOfSecurityVeh.IsPositionFrozen = false;
			Blip blip = GlobalHelper.CreateBlip(HeadOfSecurityVeh.Position);
			blip.Sprite = BlipSprite.HeistSetup;
			blip.Color = BlipColor.Green;
			blip.IsShortRange = true;
			blip.Scale = 0.5f;
			blip.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip);
			blip = GlobalHelper.CreateBlip(HeadOfSecurityVeh.Position, 100f);
			blip.Name = "Possible Intel/Asset";
			blip.Color = BlipColor.Green;
			blip.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip);
			blip = GlobalHelper.CreateBlip(DjHouseEnter);
			blip.Sprite = BlipSprite.HeistSetup;
			blip.Color = BlipColor.Green;
			blip.IsShortRange = true;
			blip.Scale = 0.5f;
			blip.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip);
			blip = GlobalHelper.CreateBlip(DjHouseEnter, 100f);
			blip.Name = "Possible Intel/Asset";
			blip.Color = BlipColor.Green;
			blip.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip);
			blip = GlobalHelper.CreateBlip(CartelIntelLoc);
			blip.Sprite = BlipSprite.HeistSetup;
			blip.Color = BlipColor.Green;
			blip.IsShortRange = true;
			blip.Scale = 0.5f;
			blip.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip);
			blip = GlobalHelper.CreateBlip(CartelIntelLoc, 100f);
			blip.Name = "Possible Intel/Asset";
			blip.Color = BlipColor.Green;
			blip.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip);
			blip = GlobalHelper.CreateBlip(HeadOfSecurityHouseEnter);
			blip.Sprite = BlipSprite.HeistSetup;
			blip.Color = BlipColor.Green;
			blip.IsShortRange = true;
			blip.Scale = 0.5f;
			blip.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip);
			blip = GlobalHelper.CreateBlip(HeadOfSecurityHouseEnter, 100f);
			blip.Name = "Possible Intel/Asset";
			blip.Color = BlipColor.Green;
			blip.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip);
			blip = GlobalHelper.CreateBlip(PenthouseEnter);
			blip.Sprite = BlipSprite.HeistSetup;
			blip.Color = BlipColor.Green;
			blip.IsShortRange = true;
			blip.Scale = 0.5f;
			blip.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip);
			blip = GlobalHelper.CreateBlip(PenthouseEnter, 100f);
			blip.Name = "Possible Intel/Asset";
			blip.Color = BlipColor.Green;
			blip.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip);
			blip = GlobalHelper.CreateBlip(RoadblockLocationLoc);
			blip.Sprite = BlipSprite.HeistSetup;
			blip.Color = BlipColor.Green;
			blip.IsShortRange = true;
			blip.Scale = 0.5f;
			blip.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip);
			blip = GlobalHelper.CreateBlip(RoadblockLocationLoc, 100f);
			blip.Name = "Possible Intel/Asset";
			blip.Color = BlipColor.Green;
			blip.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip);
			Function.Call(Hash.BUSYSPINNER_OFF);
			CurrentHeistStage = HeistStage.Scout;
			TheBiliionaresPartyHeistOn = true;
		}

		public static void StartTargetVehicleEscape()
		{
			if (TargetVehicleADriver != null)
			{
				TargetVehicleADriver.Delete();
			}
			if (TargetVehicleBDriver != null)
			{
				TargetVehicleBDriver.Delete();
			}
			if (TargetVehicleCDriver != null)
			{
				TargetVehicleCDriver.Delete();
			}
			if (TargetVehicleDDriver != null)
			{
				TargetVehicleDDriver.Delete();
			}
			if (TargetVehicleA != null && Game.Player.Character.CurrentVehicle != TargetVehicleA)
			{
				TargetVehicleA.Position = TargetVehicleEscapePositions.Entities[0].Position;
				TargetVehicleA.Rotation = TargetVehicleEscapePositions.Entities[0].Rotation;
				TargetVehicleADriver = GlobalHelper.CreatePed(RequestModel("IG_Paper"), default(Vector3), 0f);
				TargetVehicleADriver.SetIntoVehicle(TargetVehicleA, VehicleSeat.Driver);
				VehicleRecording.Playing_TargetVehA_Route = true;
				VehicleRecording.Route_TargetVehA_Stage = 0;
				VehicleRecording.TargetVehA_DriveTimer = 0;
			}
			if (TargetVehicleB != null && Game.Player.Character.CurrentVehicle != TargetVehicleB)
			{
				TargetVehicleB.Position = TargetVehicleEscapePositions.Entities[1].Position;
				TargetVehicleB.Rotation = TargetVehicleEscapePositions.Entities[1].Rotation;
				TargetVehicleBDriver = GlobalHelper.CreatePed(RequestModel("IG_Paper"), default(Vector3), 0f);
				TargetVehicleBDriver.SetIntoVehicle(TargetVehicleB, VehicleSeat.Driver);
				VehicleRecording.Playing_TargetVehB_Route = true;
				VehicleRecording.Route_TargetVehB_Stage = 0;
				VehicleRecording.TargetVehB_DriveTimer = 0;
			}
			if (TargetVehicleC != null && Game.Player.Character.CurrentVehicle != TargetVehicleC)
			{
				TargetVehicleC.Position = TargetVehicleEscapePositions.Entities[2].Position;
				TargetVehicleC.Rotation = TargetVehicleEscapePositions.Entities[2].Rotation;
				TargetVehicleCDriver = GlobalHelper.CreatePed(RequestModel("IG_Paper"), default(Vector3), 0f);
				TargetVehicleCDriver.SetIntoVehicle(TargetVehicleC, VehicleSeat.Driver);
				VehicleRecording.Playing_TargetVehC_Route = true;
				VehicleRecording.Route_TargetVehC_Stage = 0;
				VehicleRecording.TargetVehC_DriveTimer = 0;
			}
			if (TargetVehicleD != null && Game.Player.Character.CurrentVehicle != TargetVehicleD)
			{
				TargetVehicleD.Position = TargetVehicleEscapePositions.Entities[3].Position;
				TargetVehicleD.Rotation = TargetVehicleEscapePositions.Entities[3].Rotation;
				TargetVehicleDDriver = GlobalHelper.CreatePed(RequestModel("IG_Paper"), default(Vector3), 0f);
				TargetVehicleDDriver.SetIntoVehicle(TargetVehicleD, VehicleSeat.Driver);
				VehicleRecording.Playing_TargetVehD_Route = true;
				VehicleRecording.Route_TargetVehD_Stage = 0;
				VehicleRecording.TargetVehD_DriveTimer = 0;
			}
			TargetVehiclesEscaping = true;
		}

		public static void CompleteHeist()
		{
			Main.TotalCrewEarned += 0f;
			Main.TotalPlayerEarned += CurrentPay;
			XP.CompleteHeist_TheBillionariesParty++;
			Main.SaveCuts();
			Class1.SetNoLongerInHeist();
			MissionScreen.SetPass("The Billionaires Party Heist", CurrentPay, 0, "The Player has died", 2, 0f, 5);
			int num = Game.GameTime + 30000;
			while (HKHModHelperNew.Class1.HeistPassedScreen != -1 && Game.GameTime <= num)
			{
				Script.Wait(0);
			}
			Script.Wait(2000);
			XP.AddXP_Percent(XP.CompleteHeist_XPPercent_TheBillionariesParty);
			Game.Player.Money += CurrentPay;
			EndHeist();
			CurrentPay = 0;
			LocalTake = 0;
			EndHeist();
			TheBiliionaresPartyHeistOn = false;
		}

		public static void FailHeist(string reason)
		{
			Class1.SetNoLongerInHeist();
			HKHModHelperNew.Class1.scaleformDescription = "The Billionaires Party Heist was failed " + reason;
			HKHModHelperNew.Class1.scaleformTitle = "The Billionaires Party Heist was Failed";
			HKHModHelperNew.Class1.ScaleformColor = HKHModHelperNew.Class1.ScaleformColour.Red;
			HKHModHelperNew.Class1.ShowScaleform = true;
			HKHModHelperNew.Class1.Passed_Index = 0;
			EndHeist();
			TheBiliionaresPartyHeistOn = false;
		}

		private void OnTick(object sender, EventArgs e)
		{
			if (!TheBiliionaresPartyHeistOn || !TheBiliionaresPartyHeistOn)
			{
				return;
			}
			switch (CurrentHeistStage)
			{
			}
			if (SpookedParty && !PhantomIsEscapingParty && !ChangedVehiclesEscape)
			{
				foreach (Blip intelMarker in IntelMarkers)
				{
					if (intelMarker != null)
					{
						intelMarker.Alpha = 0;
					}
				}
				foreach (Blip item in IntelMarkersRadiusArea)
				{
					if (item != null)
					{
						item.Alpha = 0;
					}
				}
				TextNotification("CHAR_LESTER", "~y~The Billionaires Party~w~", "Lester", "Party has been Spooked!, The Billionaire has ordered that the target vehicles be moved out");
				StartTargetVehicleEscape();
				ChangedVehiclesEscape = true;
			}
			InParty = GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(-1550.65f, 119.07f, 56.44f), "<", 60f) || GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(-1605.38f, 116.7f, 59.77f), "<", 40f);
			if (TargetVehicleA_NoLongerAvailable && TargetVehicleB_NoLongerAvailable && TargetVehicleC_NoLongerAvailable && TargetVehicleD_NoLongerAvailable)
			{
				EndHeist();
				FailHeist("cause the all target vehicles were exported out of LS");
			}
			if (PhantomDelivered && BuyerObjects.Entities[0].Entity != null && GetDistance_VDIST2(Game.Player.Character.Position, BuyerObjects.Entities[0].Entity.Position, "<", 2f))
			{
				DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Complete Contract for Martin Madrazzo");
				if (Game.IsControlJustPressed(Control.Context))
				{
					CompleteHeist();
				}
			}
			if (TargetVehiclesEscaping)
			{
				if (BuyerObjects.Entities[0].Entity != null && GetDistance_VDIST2(Game.Player.Character.Position, BuyerObjects.Entities[0].Entity.Position, "<", 2f) && (TargetVehicleA_Delivered || TargetVehicleB_Delivered || TargetVehicleC_Delivered || TargetVehicleD_Delivered))
				{
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Complete Contract for Martin Madrazzo");
					if (Game.IsControlJustPressed(Control.Context))
					{
						CompleteHeist();
					}
				}
				if (Game.Player.Character.CurrentVehicle == TargetVehicleA || Game.Player.Character.CurrentVehicle == TargetVehicleB || Game.Player.Character.CurrentVehicle == TargetVehicleC || Game.Player.Character.CurrentVehicle == TargetVehicleD)
				{
					if (DeliverVehArea != null)
					{
						DeliverVehArea.Alpha = 144;
					}
					if (DeliverVehBlip != null)
					{
						DeliverVehBlip.ShowRoute = true;
						DeliverVehBlip.Alpha = 255;
					}
					CurrentHeistStage = HeistStage.DrivingTargetVeh;
					if (BuyerObjects.Entities[0].Entity != null)
					{
						if (BuyerObjects.Entities[1].Entity != null && !TargetVehicleA_Delivered && GetDistance_VDIST2(TargetVehicleA.Position, BuyerObjects.Entities[1].Position, "<", 30f))
						{
							CurrentHeistStage = HeistStage.DeliverVehicleToParkingSpace;
							DrawBox((Vehicle)BuyerObjects.Entities[1].Entity, new Vector3(0f, 0f, 0f), 1.25f, 2.25f);
							if (GetDistance_VDIST2(TargetVehicleA.Position, BuyerObjects.Entities[1].Position, "<", 2.5f))
							{
								Game.Player.Character.Task.LeaveVehicle();
								TargetVehicleA.LockStatus = VehicleLockStatus.CannotEnter;
								TargetVehicleA_Delivered = true;
								TargetVehicleA.AttachedBlip.Alpha = 0;
								ManageRobberyAnims.CurrentPay += (int)VehicleAPrice;
								Main.TextNotification("CHAR_MARTIN", "Martin Madrazo", "Vehicle Delivered", "Delivery Received, collect more vehicles, or come see me to complete contract.");
								CurrentHeistStage = HeistStage.TargetVehsEscaping;
							}
						}
						if (BuyerObjects.Entities[2].Entity != null && !TargetVehicleB_Delivered && GetDistance_VDIST2(TargetVehicleB.Position, BuyerObjects.Entities[2].Position, "<", 30f))
						{
							CurrentHeistStage = HeistStage.DeliverVehicleToParkingSpace;
							DrawBox((Vehicle)BuyerObjects.Entities[2].Entity, new Vector3(0f, 0f, 0f), 1.25f, 2.25f);
							if (GetDistance_VDIST2(TargetVehicleB.Position, BuyerObjects.Entities[2].Position, "<", 2.5f))
							{
								Game.Player.Character.Task.LeaveVehicle();
								TargetVehicleB.LockStatus = VehicleLockStatus.CannotEnter;
								TargetVehicleB_Delivered = true;
								TargetVehicleB.AttachedBlip.Alpha = 0;
								ManageRobberyAnims.CurrentPay += (int)VehicleBPrice;
								Main.TextNotification("CHAR_MARTIN", "Martin Madrazo", "Vehicle Delivered", "Delivery Received, collect more vehicles, or come see me to complete contract.");
								CurrentHeistStage = HeistStage.TargetVehsEscaping;
							}
						}
						if (BuyerObjects.Entities[3].Entity != null && !TargetVehicleC_Delivered && GetDistance_VDIST2(TargetVehicleC.Position, BuyerObjects.Entities[3].Position, "<", 30f))
						{
							CurrentHeistStage = HeistStage.DeliverVehicleToParkingSpace;
							DrawBox((Vehicle)BuyerObjects.Entities[3].Entity, new Vector3(0f, 0f, 0f), 1.25f, 2.25f);
							if (GetDistance_VDIST2(TargetVehicleC.Position, BuyerObjects.Entities[3].Position, "<", 2.5f))
							{
								Game.Player.Character.Task.LeaveVehicle();
								TargetVehicleC.LockStatus = VehicleLockStatus.CannotEnter;
								TargetVehicleC_Delivered = true;
								TargetVehicleC.AttachedBlip.Alpha = 0;
								ManageRobberyAnims.CurrentPay += (int)VehicleCPrice;
								Main.TextNotification("CHAR_MARTIN", "Martin Madrazo", "Vehicle Delivered", "Delivery Received, collect more vehicles, or come see me to complete contract.");
								CurrentHeistStage = HeistStage.TargetVehsEscaping;
							}
						}
						if (BuyerObjects.Entities[4].Entity != null && !TargetVehicleD_Delivered && GetDistance_VDIST2(TargetVehicleD.Position, BuyerObjects.Entities[4].Position, "<", 30f))
						{
							CurrentHeistStage = HeistStage.DeliverVehicleToParkingSpace;
							DrawBox((Vehicle)BuyerObjects.Entities[4].Entity, new Vector3(0f, 0f, 0f), 1.25f, 2.25f);
							if (GetDistance_VDIST2(TargetVehicleD.Position, BuyerObjects.Entities[4].Position, "<", 2.5f))
							{
								Game.Player.Character.Task.LeaveVehicle();
								TargetVehicleD.LockStatus = VehicleLockStatus.CannotEnter;
								TargetVehicleD_Delivered = true;
								TargetVehicleD.AttachedBlip.Alpha = 0;
								ManageRobberyAnims.CurrentPay += (int)VehicleDPrice;
								Main.TextNotification("CHAR_MARTIN", "Martin Madrazo", "Vehicle Delivered", "Delivery Received, collect more vehicles, or come see me to complete contract.");
								CurrentHeistStage = HeistStage.TargetVehsEscaping;
							}
						}
					}
				}
				else
				{
					if (DeliverVehArea != null)
					{
						DeliverVehArea.Alpha = 0;
					}
					if (DeliverVehBlip != null)
					{
						DeliverVehBlip.ShowRoute = false;
						DeliverVehBlip.Alpha = 0;
					}
					CurrentHeistStage = HeistStage.TargetVehsEscaping;
				}
			}
			else
			{
				if (BuyerObjects.Entities[0].Entity != null && GetDistance_VDIST2(Game.Player.Character.Position, BuyerObjects.Entities[0].Entity.Position, "<", 2f) && (TargetVehicleA_Delivered || TargetVehicleB_Delivered || TargetVehicleC_Delivered || TargetVehicleD_Delivered))
				{
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Complete Contract for Martin Madrazzo");
					if (Game.IsControlJustPressed(Control.Context))
					{
						CompleteHeist();
					}
				}
				if (Game.Player.Character.CurrentVehicle == Phantom)
				{
					PhantomTaken = true;
					if (BuyerObjects.Entities[1].Entity != null && !PhantomDelivered && GetDistance_VDIST2(Phantom.Position, BuyerObjects.Entities[1].Position, "<", 100f))
					{
						CurrentHeistStage = HeistStage.DeliverVehicleToParkingSpace;
						DrawBox((Vehicle)BuyerObjects.Entities[1].Entity, new Vector3(0f, 0f, 0f), 2.25f, 4.25f);
						if (GetDistance_VDIST2(Phantom.Position, BuyerObjects.Entities[1].Position, "<", 2.5f))
						{
							Game.Player.Character.Task.LeaveVehicle();
							Phantom.LockStatus = VehicleLockStatus.CannotEnter;
							PhantomDelivered = true;
							if (Phantom.AttachedBlip != null)
							{
								Phantom.AttachedBlip.Alpha = 0;
							}
							ManageRobberyAnims.CurrentPay += (int)VehicleAPrice;
							ManageRobberyAnims.CurrentPay += (int)VehicleBPrice;
							ManageRobberyAnims.CurrentPay += (int)VehicleCPrice;
							ManageRobberyAnims.CurrentPay += (int)VehicleDPrice;
							Main.TextNotification("CHAR_MARTIN", "Martin Madrazo", "Vehicles Delivered", "4/4 Deliveries Received, Come see me to complete contract.");
							CurrentHeistStage = HeistStage.TargetVehsEscaping;
						}
					}
					if (DeliverVehArea != null)
					{
						DeliverVehArea.Alpha = 144;
					}
					if (DeliverVehBlip != null)
					{
						DeliverVehBlip.Alpha = 255;
					}
				}
				else
				{
					if (DeliverVehArea != null)
					{
						DeliverVehArea.Alpha = 0;
					}
					if (DeliverVehBlip != null)
					{
						DeliverVehBlip.Alpha = 0;
					}
				}
			}
			if (CurrentHeistStage == HeistStage.Scout)
			{
				GTA.UI.Screen.ShowSubtitle("Approach the ~y~Billionaires Party~w~ or Scout for more ~g~Assets/Intel~w~");
			}
			if (CurrentHeistStage == HeistStage.SetupRoadblock)
			{
				GTA.UI.Screen.ShowSubtitle("Cartel members can setup a roadblock here");
			}
			if (CurrentHeistStage == HeistStage.AccessingCameras)
			{
				GTA.UI.Screen.ShowSubtitle("Look Around to locate where the ~b~Target Vehicles~w~ are parked~w~");
			}
			if (CurrentHeistStage == HeistStage.AwaitVaultOpen)
			{
				GTA.UI.Screen.ShowSubtitle("Wait for the ~y~Vault~w~ to Open");
			}
			if (CurrentHeistStage == HeistStage.CannotHackKeypad)
			{
				GTA.UI.Screen.ShowSubtitle("Aquire the ~y~Vault Keycard~w~ to gain access to the vault");
			}
			if (CurrentHeistStage == HeistStage.DJAccessPass)
			{
				GTA.UI.Screen.ShowSubtitle("Find the ~y~DJ's Access Pass~w~ to get unrestricted access to the party");
			}
			if (CurrentHeistStage == HeistStage.DrivingPhantom)
			{
				GTA.UI.Screen.ShowSubtitle("Deliver the ~b~Target Vehicles~w~ to ~y~Martin Madrazo~w~");
			}
			if (CurrentHeistStage == HeistStage.DrivingTargetVeh)
			{
				GTA.UI.Screen.ShowSubtitle("Deliver the ~b~Target Vehicle~w~ to ~y~Martin Madrazo~w~");
			}
			if (CurrentHeistStage == HeistStage.TargetVehsEscaping)
			{
				GTA.UI.Screen.ShowSubtitle("Steal ~p~At Least~w~ One ~b~Target Vehicle~w~ for ~y~Marin Madrazo~w~");
			}
			if (CurrentHeistStage == HeistStage.HackComBox)
			{
				GTA.UI.Screen.ShowSubtitle("Hack the ~y~Combox~w~ to gain access to the ~d~Party's CCTV");
			}
			if (CurrentHeistStage == HeistStage.HackHoSPhone)
			{
				GTA.UI.Screen.ShowSubtitle("Hack the ~r~Head of Security's Phone~w~ to aquire the ~y~Biometric Pass");
			}
			if (CurrentHeistStage == HeistStage.HackKeypad)
			{
				GTA.UI.Screen.ShowSubtitle("Hack the ~y~Keypad~w~ to gain Access to the vault");
			}
			if (CurrentHeistStage == HeistStage.PhantomEscaping && Phantom != null)
			{
				if (GetDistance_VDIST2(Game.Player.Character.Position, Phantom.Position, "<", 100f))
				{
					if (!PhantomDelivered)
					{
						if (!PhantomTaken)
						{
							GTA.UI.Screen.ShowSubtitle("Steal the ~r~Phantom~w~ with the ~b~Target Vehicles~w~ On it");
						}
						else
						{
							GTA.UI.Screen.ShowSubtitle("Deliver the ~b~Phantom~w~ to ~y~Martin Madrazo~w~");
						}
					}
					else
					{
						GTA.UI.Screen.ShowSubtitle("Talk to ~y~Martin Madrazo~w~ to Complete the Heist");
					}
				}
				else
				{
					GTA.UI.Screen.ShowSubtitle("Find the ~r~Phantom~w~ before it escapes");
				}
			}
			if (CurrentHeistStage == HeistStage.VaultKeycard)
			{
				GTA.UI.Screen.ShowSubtitle("Aquire the ~y~Head of Security's Vault Keycard~w~ to access the Vault");
			}
			if (CurrentHeistStage == HeistStage.VaultOpenLoot)
			{
				GTA.UI.Screen.ShowSubtitle("Loot the ~g~Billionaires Vault~w~");
			}
			if (CurrentHeistStage == HeistStage.VipAccessPass)
			{
				GTA.UI.Screen.ShowSubtitle("Find the ~y~VIP Access Pass~w~ to get unrestricted access to the party");
			}
			if (CurrentHeistStage == HeistStage.PartyMain)
			{
				GTA.UI.Screen.ShowSubtitle("Enjoy the Party and find a way to steal the ~b~Target Vehicles~w~");
			}
			if (CurrentHeistStage == HeistStage.NoWayIntoParty)
			{
				GTA.UI.Screen.ShowSubtitle("Enter the party as a ~r~Uninvited Guest~w~ or find another way in");
			}
			if (CurrentHeistStage == HeistStage.CartelPrePurchase)
			{
				GTA.UI.Screen.ShowSubtitle("Talk to the ~y~Cartel Leader~w~ to arrange a deal");
			}
			if (CurrentHeistStage == HeistStage.DeliverVehicleToParkingSpace)
			{
				GTA.UI.Screen.ShowSubtitle("Park the ~b~Vehicle~w~ in the ~y~Blue Parking Spot~w~");
			}
			if (CurrentHeistStage == HeistStage.EnteringParty)
			{
				if (!GotDJPass && !GotVIP_Access_Pass)
				{
					GTA.UI.Screen.ShowSubtitle("~r~Guard:~w~ Sorry Sir, this is a private party, you are not allowed access.");
				}
				else
				{
					GTA.UI.Screen.ShowSubtitle("~r~Guard:~w~ Welcome Sir, Please come in.");
				}
			}
			if (PhantomIsEscapingParty && !TargetVehiclesEscaping)
			{
				if (Game.Player.Character.CurrentVehicle == Phantom)
				{
					CurrentHeistStage = HeistStage.DrivingPhantom;
				}
				else if (!PhantomDelivered && !PhantomTaken)
				{
					CurrentHeistStage = HeistStage.PhantomEscaping;
				}
				if (!PhantomTaken)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, Phantom.Position, ">", 100f))
					{
						if (!StartedPointOfNoReturn)
						{
							PointOfNoReturn = Game.GameTime + 180000;
							StartedPointOfNoReturn = true;
						}
						else
						{
							TimeSpan timeSpan = TimeSpan.FromMilliseconds(PointOfNoReturn - Game.GameTime);
							if (timeSpan.Seconds >= 10)
							{
								string amt = timeSpan.Minutes + "." + timeSpan.Seconds;
								ShowTextInfoBar("Fail In: ", amt, 2, new Vector2(-0.05f, 0f), new Vector2(-0.05f, 0f), false);
							}
							else
							{
								string amt2 = timeSpan.Minutes + ".0" + timeSpan.Seconds;
								ShowTextInfoBar("Fail In: ", amt2, 2, new Vector2(-0.05f, 0f), new Vector2(-0.05f, 0f), false);
							}
							if (Game.GameTime >= PointOfNoReturn)
							{
								EndHeist();
								FailHeist("cause the Phantom Escaped");
							}
						}
					}
					else
					{
						PointOfNoReturn = Game.GameTime + 180000;
					}
					if (Game.Player.Character.CurrentVehicle == Phantom)
					{
						CurrentHeistStage = HeistStage.DrivingPhantom;
					}
					else
					{
						CurrentHeistStage = HeistStage.PhantomEscaping;
					}
					if (Phantom != null && PhantomDriver != null)
					{
						if (PhantomReRouteTimer <= 48)
						{
							PhantomReRouteTimer++;
						}
						else
						{
							Random random = new Random();
							Vector3 vector = GenerateSpawnPos(Phantom.Position.Around(random.Next(150, 300)), Nodetype.AnyRoad, false);
							PhantomDriver.Task.FleeFrom(Game.Player.Character);
							PhantomReRouteTimer = 0;
						}
					}
				}
			}
			if (IntelMarkersRadiusArea.Count >= 1 && IntelMarkers.Count >= 1 && !HackedHoSPhone && !TargetVehiclesEscaping && IntelMarkersRadiusArea[0] != null && IntelMarkers[0] != null && HeadOfSecurityVeh != null)
			{
				IntelMarkersRadiusArea[0].Position = HeadOfSecurityVeh.Position;
				IntelMarkers[0].Position = HeadOfSecurityVeh.Position;
				if (GetDistance_VDIST2(Game.Player.Character.Position, HeadOfSecurityVeh.Position, "<", 140f))
				{
					CurrentHeistStage = HeistStage.HackHoSPhone;
					DrawProgressBar_Advanced("Hack Progress", 2, HoSHackPerc, 100f, Color.RoyalBlue, false);
					if (GetDistance_VDIST2(Game.Player.Character.Position, HeadOfSecurityVeh.Position, "<", 40f))
					{
						if (!AleartedPolice)
						{
							TextNotification("CHAR_HUMANDEFAULT", "Lester", "Hack Detected", "The Head of Security Knows your hacking his phone, he has alerted the police.");
							AleartedPolice = true;
						}
						Game.Player.WantedLevel = 3;
						if (HosNewRouteTime <= 48)
						{
							HosNewRouteTime++;
						}
						else
						{
							Random random2 = new Random();
							Vector3 vector2 = GenerateSpawnPos(HeadOfSecurityDriver.Position.Around(random2.Next(150, 300)), Nodetype.AnyRoad, false);
							HeadOfSecurityDriver.Task.FleeFrom(Game.Player.Character);
							HosNewRouteTime = 0;
						}
					}
					if (!HackingHoSphone)
					{
						DisplayHelpTextThisFrame("Open your phone to start hacking the Head of Security's Phone");
						if (Function.Call<bool>(Hash.CAN_PHONE_BE_SEEN_ON_SCREEN))
						{
							HackingHoSphone = true;
							if (!FingerprintHack.SecuroServPhoneHackOn)
							{
								FingerprintHack.SecuroServPhoneHackOn = false;
								FingerprintHack.RunSecuroServPhoneHack();
							}
						}
					}
					else
					{
						FingerprintHack.SecuroServ_UploadProgress_ViaHelper = false;
						DisplayHelpTextThisFrame("Aim at the Head of Security's Car to hack into his phone\nClose you phone to hide hacking UI");
						if (!Function.Call<bool>(Hash.CAN_PHONE_BE_SEEN_ON_SCREEN))
						{
							FingerprintHack.SecuroServPhoneHackOn = false;
							HackingHoSphone = false;
						}
						if (Game.Player.Character.CurrentVehicle != null)
						{
							RaycastResult raycastResult = World.Raycast(GameplayCamera.Position, GameplayCamera.GetOffsetPosition(new Vector3(0f, 200f, 0f)), IntersectFlags.Everything, Game.Player.Character.CurrentVehicle);
							if (raycastResult.DidHit)
							{
								DrawLine(Game.Player.Character.Position, raycastResult.HitPosition, new Vector3(255f, 0f, 0f));
								if (GetDistance_VDIST2(raycastResult.HitPosition, HeadOfSecurityVeh.Position, "<", 5f))
								{
									if (HackHoSTimer <= 30)
									{
										HackHoSTimer++;
									}
									else
									{
										HackHoSTimer = 0;
										HoSHackPerc++;
										FingerprintHack.SecuroServ_UploadProgress++;
									}
								}
							}
						}
						if (HoSHackPerc == 100f)
						{
							if (TargetVehicleA != null)
							{
								Vector3 offsetPosition = Trailer.GetOffsetPosition(new Vector3(0f, 4.85f, 2.85f));
								TargetVehicleA.AttachTo(Trailer, CarCarrierObjects.Entities[4].EntityToAttachOffset, CarCarrierObjects.Entities[4].EntityToAttachRotation);
							}
							if (TargetVehicleB != null)
							{
								Vector3 offsetPosition2 = Trailer.GetOffsetPosition(new Vector3(0f, -2.5f, 0.7f));
								TargetVehicleB.AttachTo(Trailer, CarCarrierObjects.Entities[1].EntityToAttachOffset, CarCarrierObjects.Entities[1].EntityToAttachRotation);
							}
							if (TargetVehicleC != null)
							{
								Vector3 offsetPosition3 = Trailer.GetOffsetPosition(new Vector3(0f, -7.5f, 0.91f));
								TargetVehicleC.AttachTo(Trailer, CarCarrierObjects.Entities[2].EntityToAttachOffset, CarCarrierObjects.Entities[2].EntityToAttachRotation);
							}
							if (TargetVehicleD != null)
							{
								Vector3 offsetPosition4 = Trailer.GetOffsetPosition(new Vector3(0f, 4.8f, 0.92f));
								TargetVehicleD.AttachTo(Trailer, CarCarrierObjects.Entities[3].EntityToAttachOffset, CarCarrierObjects.Entities[3].EntityToAttachRotation);
							}
							Trailer.Doors[VehicleDoorIndex.Trunk].Close(true);
							if (TargetVehicleA.AttachedBlip != null)
							{
								TargetVehicleA.AttachedBlip.Alpha = 0;
							}
							if (TargetVehicleB.AttachedBlip != null)
							{
								TargetVehicleB.AttachedBlip.Alpha = 0;
							}
							if (TargetVehicleC.AttachedBlip != null)
							{
								TargetVehicleC.AttachedBlip.Alpha = 0;
							}
							if (TargetVehicleD.AttachedBlip != null)
							{
								TargetVehicleD.AttachedBlip.Alpha = 0;
							}
							if (PhantomDriver != null)
							{
								PhantomDriver.Delete();
							}
							Phantom.Position = new Vector3(-1461.412f, 60f, 51.66f);
							Phantom.Heading = -83.24f;
							Phantom.AddBlip();
							Phantom.AttachedBlip.Sprite = BlipSprite.ArmoredTruck;
							Phantom.AttachedBlip.Color = BlipColor.Red;
							Phantom.AttachedBlip.Name = "Steal Phantom Truck";
							Phantom.AttachedBlip.IsShortRange = true;
							PhantomDriver = GlobalHelper.CreatePed(RequestModel("IG_Paper"), Phantom.Position, 0f);
							PhantomDriver.SetIntoVehicle(Phantom, VehicleSeat.Driver);
							PhantomIsEscapingParty = true;
							HackingHoSphone = false;
							IntelMarkers[0].Alpha = 0;
							IntelMarkersRadiusArea[0].Alpha = 0;
							TextNotification("CHAR_HUMANDEFAULT", "Lester", "Sent Fake Message to Billionaire", "Ive sent a fake msg to the Billionaire, their loading the vehicles onto a semi, and transporting it out of the city.");
							HackedHoSPhone = true;
							HoSHackPerc = 101f;
						}
					}
				}
				else if (HackingHoSphone)
				{
					HackHoSTimer = 0;
					HoSHackPerc = 0f;
					FingerprintHack.SecuroServ_UploadProgress = 0f;
					Notification.Show("Hack Cancelled, too far away from Head of Security's Car");
					FingerprintHack.SecuroServPhoneHackOn = false;
					HackingHoSphone = false;
				}
			}
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
			if (ManageRobberyAnims.CurrentType != ManageRobberyAnims.LootType.GlassCut_Cabinet)
			{
				ShowTextInfoBar("CurrentPay ", LocalTake, 1, new Vector2(-0.05f, 0f), new Vector2(-0.05f, 0f), true);
			}
			if (HosGuards.Count > 1 && CurrentActiveGroup == ActiveGroup.TheBillionariesParty_HeadofSecurity)
			{
				CurrentHeistStage = HeistStage.VaultKeycard;
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
			if (MansionGuards.Count > 1 && CurrentActiveGroup == ActiveGroup.TheBillionariesParty_MansionGuards)
			{
				CurrentHeistStage = HeistStage.PartyMain;
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
				if (!GotVIP_Access_Pass && !GotVIP_Access_Pass)
				{
					if (InParty)
					{
						SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
					}
					else
					{
						SmartGuards.GetCurrentGroupSettings().IngorePlayer = true;
					}
				}
				else
				{
					SmartGuards.GetCurrentGroupSettings().IngorePlayer = true;
					if (IsInTargetVehicle())
					{
						SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
					}
					if (HackingKeypad || HackedMainKeypad)
					{
						SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
					}
				}
				if (SpookedParty)
				{
					SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
				}
			}
			if (PenthouseGuards.Count > 1 && CurrentActiveGroup == ActiveGroup.TheBillionariesParty_Penthouse)
			{
				CurrentHeistStage = HeistStage.VipAccessPass;
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
			if (CartelGuards.Count > 1 && CurrentActiveGroup == ActiveGroup.TheBillionariesParty_Cartel)
			{
				CurrentHeistStage = HeistStage.CartelPrePurchase;
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
			if (GetDistance_VDIST2(Game.Player.Character.Position, MansionLoc, "<", 150f))
			{
				if (CurrentActiveGroup != ActiveGroup.TheBillionariesParty_MansionGuards)
				{
					SmartGuards.GuardGroup[24].GroupList.Clear();
					SmartGuards.GuardGroup[24].GroupList.AddRange(MansionGuards);
					SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.High_End_Security;
					CurrentActiveGroup = ActiveGroup.TheBillionariesParty_MansionGuards;
				}
			}
			else if (CurrentActiveGroup == ActiveGroup.TheBillionariesParty_MansionGuards)
			{
				SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.None;
				CurrentActiveGroup = ActiveGroup.None;
			}
			if (!TargetVehiclesEscaping)
			{
				if (GetDistance_VDIST2(Game.Player.Character.Position, MansionLoc, "<", 150f))
				{
					if (CurrentActiveGroup != ActiveGroup.TheBillionariesParty_MansionGuards)
					{
						SmartGuards.GuardGroup[24].GroupList.Clear();
						SmartGuards.GuardGroup[24].GroupList.AddRange(MansionGuards);
						SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.High_End_Security;
						CurrentActiveGroup = ActiveGroup.TheBillionariesParty_MansionGuards;
					}
					if (!GotVIP_Access_Pass && !GotDJPass)
					{
						CurrentHeistStage = HeistStage.NoWayIntoParty;
						if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(-1615.422f, 75.72f, 60.4f), "<", 20f))
						{
							CurrentHeistStage = HeistStage.EnteringParty;
						}
						else
						{
							CurrentHeistStage = HeistStage.NoWayIntoParty;
						}
					}
					else
					{
						CurrentHeistStage = HeistStage.PartyMain;
						if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(-1615.422f, 75.72f, 60.4f), "<", 20f))
						{
							CurrentHeistStage = HeistStage.EnteringParty;
						}
						else
						{
							CurrentHeistStage = HeistStage.PartyMain;
						}
					}
				}
				else if (CurrentHeistStage == HeistStage.NoWayIntoParty || CurrentHeistStage == HeistStage.PartyMain)
				{
					CurrentHeistStage = HeistStage.Scout;
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, RoadblockLocationLoc, "<", 20f))
				{
					if (!PayedforRoadBlock)
					{
						CurrentHeistStage = HeistStage.SetupRoadblock;
					}
				}
				else if (CurrentHeistStage == HeistStage.SetupRoadblock)
				{
					CurrentHeistStage = HeistStage.Scout;
				}
			}
			if (!inDjHouse)
			{
				if (GetDistance_VDIST2(Game.Player.Character.Position, DjHouseEnter, "<", 100f))
				{
					World.DrawMarker(MarkerType.Cylinder, DjHouseEnter, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
					if (GetDistance_VDIST2(Game.Player.Character.Position, DjHouseEnter, "<", 2f))
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter DJs House");
						if (Game.IsControlJustPressed(Control.Context))
						{
							Function.Call(Hash.BEGIN_TEXT_COMMAND_BUSYSPINNER_ON, "STRING");
							Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "Loading The Billionaires Party Assets");
							Function.Call(Hash.END_TEXT_COMMAND_BUSYSPINNER_ON, 5);
							VinewoodHouseObjects.CreateEntites();
							DJKeycardObjects.CreateEntites();
							DJ_Access_Pass = (Prop)DJKeycardObjects.Entities[0].Entity;
							Function.Call(Hash.BUSYSPINNER_OFF);
							GTA.UI.Screen.FadeOut(1500);
							Script.Wait(1500);
							inDjHouse = true;
							Game.Player.Character.Position = DjHouseExit;
							CurrentHeistStage = HeistStage.DJAccessPass;
							Script.Wait(1500);
							GTA.UI.Screen.FadeIn(1500);
						}
					}
				}
			}
			else if (inDjHouse)
			{
				if (!GotDJPass)
				{
					CurrentHeistStage = HeistStage.DJAccessPass;
				}
				if (DJ_Access_Pass != null && !GotDJPass)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, DJ_Access_Pass.Position, "<", 30f))
					{
						World.DrawMarker(MarkerType.Chevron1, DJ_Access_Pass.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), Vector3.Zero, new Vector3(0f, 180f, 0f), new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 255, 0), true, true);
					}
					if (DJ_Access_Pass.AttachedBlip == null)
					{
						DJ_Access_Pass.AddBlip();
						DJ_Access_Pass.AttachedBlip.Sprite = BlipSprite.Document;
						DJ_Access_Pass.AttachedBlip.Name = "DJ Access Pass";
						DJ_Access_Pass.AttachedBlip.Color = BlipColor.Green;
						DJ_Access_Pass.AttachedBlip.IsShortRange = true;
						DJ_Access_Pass.AttachedBlip.IsFlashing = true;
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, DJ_Access_Pass.Position, "<", 2.5f))
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Pickup DJ Access Pass");
						if (Game.IsControlJustPressed(Control.Context))
						{
							IntelMarkers[1].Alpha = 0;
							IntelMarkersRadiusArea[1].Alpha = 0;
							if (DJ_Access_Pass.AttachedBlip != null)
							{
								DJ_Access_Pass.AttachedBlip.Alpha = 0;
							}
							EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(DJ_Access_Pass, 0));
							CurrentHeistStage = HeistStage.Scout;
							TextNotification("CHAR_HUMANDEFAULT", "~y~The Billionaires Party~w~", "Intel Found", "You have picked up the DJ's Access Pass to get authorized access to the party");
							GotDJPass = true;
						}
					}
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, DjHouseExit, "<", 100f))
				{
					World.DrawMarker(MarkerType.Cylinder, DjHouseExit, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
					if (GetDistance_VDIST2(Game.Player.Character.Position, DjHouseExit, "<", 2f))
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit DJs House");
						if (Game.IsControlJustPressed(Control.Context))
						{
							CurrentHeistStage = HeistStage.Scout;
							GTA.UI.Screen.FadeOut(1500);
							Script.Wait(1500);
							inDjHouse = false;
							Game.Player.Character.Position = DjHouseEnter;
							VinewoodHouseObjects.DeleteEntites();
							DJKeycardObjects.DeleteEntites();
							Script.Wait(1500);
							GTA.UI.Screen.FadeIn(1500);
						}
					}
				}
			}
			if (!inPenthouse)
			{
				if (GetDistance_VDIST2(Game.Player.Character.Position, PenthouseEnter, "<", 100f))
				{
					World.DrawMarker(MarkerType.Cylinder, PenthouseEnter, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
					if (GetDistance_VDIST2(Game.Player.Character.Position, PenthouseEnter, "<", 2f))
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Penthouse");
						if (Game.IsControlJustPressed(Control.Context))
						{
							RefreshPenthouse();
							Function.Call(Hash.BEGIN_TEXT_COMMAND_BUSYSPINNER_ON, "STRING");
							Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "Loading The Billionaires Party Assets");
							Function.Call(Hash.END_TEXT_COMMAND_BUSYSPINNER_ON, 5);
							PenthouseObjects.CreateEntites();
							PenthouseGuardsObjects.CreateEntites();
							PenthouseGuards.Clear();
							foreach (ObjectPlacementMap entity in PenthouseGuardsObjects.Entities)
							{
								if (entity.Entity != null && entity.Entity.Model.IsPed)
								{
									PenthouseGuards.Add(new SmartGuards.SmartAiV2((Ped)entity.Entity, SmartGuards.EnemyGroup.High_End_Security));
								}
							}
							Function.Call(Hash.BUSYSPINNER_OFF);
							GTA.UI.Screen.FadeOut(1500);
							Script.Wait(1500);
							inPenthouse = true;
							Game.Player.Character.Position = PenthouseExit;
							CurrentHeistStage = HeistStage.VipAccessPass;
							Script.Wait(1500);
							GTA.UI.Screen.FadeIn(1500);
						}
					}
				}
			}
			else if (inPenthouse)
			{
				if (VIP_Access_Pass != null && !GotVIP_Access_Pass)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, VIP_Access_Pass.Position, "<", 30f))
					{
						World.DrawMarker(MarkerType.Chevron1, VIP_Access_Pass.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), Vector3.Zero, new Vector3(0f, 180f, 0f), new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 255, 0), true, true);
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, VIP_Access_Pass.Position, "<", 10f) && VIP_Access_Pass.AttachedBlip == null)
					{
						VIP_Access_Pass.AddBlip();
						VIP_Access_Pass.AttachedBlip.Sprite = BlipSprite.Document;
						VIP_Access_Pass.AttachedBlip.Name = "VIP Access Pass";
						VIP_Access_Pass.AttachedBlip.Color = BlipColor.Green;
						VIP_Access_Pass.AttachedBlip.IsShortRange = true;
						VIP_Access_Pass.AttachedBlip.IsFlashing = true;
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, VIP_Access_Pass.Position, "<", 2.5f))
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Pickup VIP Access Pass");
						if (Game.IsControlJustPressed(Control.Context))
						{
							IntelMarkers[4].Alpha = 0;
							IntelMarkersRadiusArea[4].Alpha = 0;
							if (VIP_Access_Pass.AttachedBlip != null)
							{
								VIP_Access_Pass.AttachedBlip.Alpha = 0;
							}
							EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(VIP_Access_Pass, 0));
							CurrentHeistStage = HeistStage.Scout;
							TextNotification("CHAR_HUMANDEFAULT", "~y~The Billionaires Party~w~", "Intel Found", "You have picked up a VIP Access Pass to get authorized access to the party");
							GotVIP_Access_Pass = true;
						}
					}
				}
				if (CurrentActiveGroup != ActiveGroup.TheBillionariesParty_Penthouse)
				{
					SmartGuards.GuardGroup[24].GroupList.Clear();
					SmartGuards.GuardGroup[24].GroupList.AddRange(PenthouseGuards);
					CurrentActiveGroup = ActiveGroup.TheBillionariesParty_Penthouse;
					SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.High_End_Security;
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, PenthouseExit, "<", 100f))
				{
					World.DrawMarker(MarkerType.Cylinder, PenthouseExit, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
					if (GetDistance_VDIST2(Game.Player.Character.Position, PenthouseExit, "<", 2f))
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit The Penthouse");
						if (Game.IsControlJustPressed(Control.Context))
						{
							GTA.UI.Screen.FadeOut(1500);
							Script.Wait(1500);
							inPenthouse = false;
							Game.Player.Character.Position = PenthouseEnter;
							PenthouseObjects.DeleteEntites();
							PenthouseGuardsObjects.DeleteEntites();
							CurrentActiveGroup = ActiveGroup.None;
							CurrentHeistStage = HeistStage.Scout;
							Script.Wait(1500);
							GTA.UI.Screen.FadeIn(1500);
						}
					}
				}
			}
			if (!inHoSHouse)
			{
				if (GetDistance_VDIST2(Game.Player.Character.Position, HeadOfSecurityHouseEnter, "<", 250f))
				{
					CurrentHeistStage = HeistStage.VaultKeycard;
					if (!CreatedHoSExteriorObjects)
					{
						CreatedHoSExteriorObjects = true;
						HeadofSecurityObjects.CreateEntites();
						ComStation = (Prop)HeadofSecurityObjects.Entities[0].Entity;
						HosGuards.Clear();
						foreach (ObjectPlacementMap entity2 in HeadofSecurityObjects.Entities)
						{
							if (entity2.Entity != null && entity2.Entity.Model.IsPed)
							{
								HosGuards.Add(new SmartGuards.SmartAiV2((Ped)entity2.Entity, SmartGuards.EnemyGroup.High_End_Security));
							}
						}
					}
					else
					{
						SmartGuards.GuardGroup[24].GroupList.Clear();
						SmartGuards.GuardGroup[24].GroupList.AddRange(HosGuards);
						SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.High_End_Security;
						CurrentActiveGroup = ActiveGroup.TheBillionariesParty_HeadofSecurity;
					}
					if (ComStation != null && !HackedComStation)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, ComStation.Position, "<", 5f))
						{
							CurrentHeistStage = HeistStage.HackComBox;
						}
						else if (CurrentHeistStage == HeistStage.HackComBox)
						{
							if (GotKeycard)
							{
								CurrentHeistStage = HeistStage.Scout;
							}
							else
							{
								CurrentHeistStage = HeistStage.VaultKeycard;
							}
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, ComStation.Position, "<", 2f))
						{
							if (!FingerprintHack.PlayingVoltageMG && !VTMinigameOn)
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Hack the Signal Box");
							}
							if (Game.IsControlPressed(Control.Context))
							{
								if (!VTMinigameOn)
								{
									Script.Wait(100);
									LoadDict("anim@scripted@heist@ig9_control_tower@male@");
									Vector3 rotation = ComStation.Rotation;
									Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, ComStation.Position.X, ComStation.Position.Y, ComStation.Position.Z, 0.0, 0.0, rotation.Z, 2);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, ComStation.Position.X, ComStation.Position.Y, ComStation.Position.Z, 0.0, 0.0, rotation.Z, true);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ComStation, Scene1, "enter_electric_box", LoadDict("anim@scripted@heist@ig9_control_tower@male@"), 1000.0, -8f, -8f, rotation.Z + 180f, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@scripted@heist@ig9_control_tower@male@", "enter", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "enter", LoadDict("anim@scripted@heist@ig9_control_tower@male@"), 1000.0, -8f, -8f, rotation.Z + 180f, 1148846080, 0);
									Script.Wait(2500);
									Game.Player.CanControlCharacter = false;
									VTMinigameOn = true;
									FingerprintHack.HackFinishedID = 0;
									FingerprintHack.PlayingVoltageMG = false;
									FingerprintHack.RunVoltageHack();
								}
								else if (VTMinigameOn)
								{
									Script.Wait(100);
									LoadDict("anim@scripted@heist@ig9_control_tower@male@");
									FingerprintHack.HackFinishedID = 0;
									FingerprintHack.PlayingVoltageMG = false;
									Game.Player.CanControlCharacter = true;
									Vector3 rotation2 = ComStation.Rotation;
									Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, ComStation.Position.X, ComStation.Position.Y, ComStation.Position.Z, 0.0, 0.0, rotation2.Z, 2);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, ComStation.Position.X, ComStation.Position.Y, ComStation.Position.Z, 0.0, 0.0, rotation2.Z, true);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ComStation, Scene1, "exit_electric_box", LoadDict("anim@scripted@heist@ig9_control_tower@male@"), 1000.0, -8f, -8f, rotation2.Z + 180f, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@scripted@heist@ig9_control_tower@male@", "exit", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "exit", LoadDict("anim@scripted@heist@ig9_control_tower@male@"), 1000.0, -8f, -8f, rotation2.Z + 180f, 1148846080, 0);
									Script.Wait(2000);
									Game.Player.Character.Task.ClearAllImmediately();
									VTMinigameOn = false;
									if (GotKeycard)
									{
										CurrentHeistStage = HeistStage.Scout;
									}
									else
									{
										CurrentHeistStage = HeistStage.VaultKeycard;
									}
								}
							}
						}
					}
					if (!HackedComStation && VTMinigameOn)
					{
						Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1);
						int num7 = 1064802024;
						if (FingerprintHack.PlayingVoltageMG && FingerprintHack.HackFinishedID == 1)
						{
							Game.Player.CanControlCharacter = true;
							Game.Player.CanControlCharacter = true;
							Vector3 rotation3 = ComStation.Rotation;
							Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, ComStation.Position.X, ComStation.Position.Y, ComStation.Position.Z, 0.0, 0.0, rotation3.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, ComStation.Position.X, ComStation.Position.Y, ComStation.Position.Z, 0.0, 0.0, rotation3.Z, true);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ComStation, Scene1, "exit_electric_box", LoadDict("anim@scripted@heist@ig9_control_tower@male@"), 1000.0, -8f, -8f, rotation3.Z + 180f, 1148846080, 0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@scripted@heist@ig9_control_tower@male@", "exit", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "exit", LoadDict("anim@scripted@heist@ig9_control_tower@male@"), 1000.0, -8f, -8f, rotation3.Z + 180f, 1148846080, 0);
							Script.Wait(1000);
							FingerprintHack.HackFinishedID = 0;
							HackedComStation = true;
							VTMinigameOn = false;
							Game.Player.Character.Task.ClearAllImmediately();
						}
					}
					if (HackedComStation)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, ComStation.Position, "<", 2f))
						{
							if (!AccessingCameras && !FingerprintHack.PlayingVoltageMG)
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Access Party CCTV");
							}
							if (Game.IsControlPressed(Control.Context))
							{
								if (!AccessingCameras)
								{
									Scene1 = -1;
									Script.Wait(100);
									LoadDict("anim@scripted@heist@ig9_control_tower@male@");
									Vector3 rotation4 = ComStation.Rotation;
									Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, ComStation.Position.X, ComStation.Position.Y, ComStation.Position.Z, 0.0, 0.0, rotation4.Z, 2);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, ComStation.Position.X, ComStation.Position.Y, ComStation.Position.Z, 0.0, 0.0, rotation4.Z, true);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ComStation, Scene1, "enter_electric_box", LoadDict("anim@scripted@heist@ig9_control_tower@male@"), 1000.0, -8f, -8f, rotation4.Z + 180f, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@scripted@heist@ig9_control_tower@male@", "enter", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "enter", LoadDict("anim@scripted@heist@ig9_control_tower@male@"), 1000.0, -8f, -8f, rotation4.Z + 180f, 1148846080, 0);
									Game.Player.Character.IsPositionFrozen = true;
									Game.Player.Character.IsInvincible = true;
									SecCam = GlobalHelper.CreateCamera(CamerasPosR[0].Position, new Vector3(0f, 0f, CamerasPosR[0].Heading), 60f);
									Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
									World.RenderingCamera = SecCam;
									Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "default");
									if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
									{
										Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
									}
									Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "traffic_skycam");
									Function.Call(Hash.SET_TIMECYCLE_MODIFIER_STRENGTH, 0.7f);
									Function.Call(Hash.SET_TRANSITION_TIMECYCLE_MODIFIER, "traffic_skycam", 0.7f);
									AccessingCameras = true;
								}
								else if (AccessingCameras)
								{
									Scene1 = -1;
									Function.Call(Hash.CLEAR_FOCUS);
									Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "default");
									if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
									{
										Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
									}
									AccessingCameras = false;
									Game.Player.Character.IsPositionFrozen = false;
									Game.Player.Character.IsInvincible = false;
									if (SecCam != null)
									{
										Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
										Game.Player.Character.IsInvincible = false;
										Game.Player.Character.IsVisible = true;
										SecCam.Delete();
									}
									Script.Wait(100);
									LoadDict("anim@scripted@heist@ig9_control_tower@male@");
									FingerprintHack.HackFinishedID = 0;
									FingerprintHack.PlayingVoltageMG = false;
									Game.Player.CanControlCharacter = true;
									Vector3 rotation5 = ComStation.Rotation;
									Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, ComStation.Position.X, ComStation.Position.Y, ComStation.Position.Z, 0.0, 0.0, rotation5.Z, 2);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, ComStation.Position.X, ComStation.Position.Y, ComStation.Position.Z, 0.0, 0.0, rotation5.Z, true);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ComStation, Scene1, "exit_electric_box", LoadDict("anim@scripted@heist@ig9_control_tower@male@"), 1000.0, -8f, -8f, rotation5.Z + 180f, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@scripted@heist@ig9_control_tower@male@", "exit", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "exit", LoadDict("anim@scripted@heist@ig9_control_tower@male@"), 1000.0, -8f, -8f, rotation5.Z + 180f, 1148846080, 0);
									Script.Wait(2000);
									Game.Player.Character.IsPositionFrozen = false;
									Game.Player.Character.IsInvincible = false;
									Game.Player.Character.Task.ClearAllImmediately();
								}
							}
						}
						if (AccessingCameras && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1064802024 && AccessingCameras)
						{
							CurrentHeistStage = HeistStage.AccessingCameras;
							while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "trafficcam"))
							{
								Script.Wait(10);
								Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "trafficcam", 0);
							}
							while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "securitycam"))
							{
								Script.Wait(10);
								Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "securitycam", 0);
							}
							while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpkillquota"))
							{
								Script.Wait(10);
								Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpkillquota", 0);
							}
							while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpaircraft"))
							{
								Script.Wait(10);
								Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpaircraft", 0);
							}
							while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "digitaloverlay"))
							{
								Script.Wait(10);
								Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "digitaloverlay", 0);
							}
							float num = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
							float num2 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
							float absoluteX;
							float absoluteY;
							LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
							int num3 = Convert.ToInt32(absoluteX / 2f);
							SecCam.Position = MansionCCTVsObjects.Entities[CurrentCamera].Entity.GetOffsetPosition(new Vector3(0f, 0.25f, 0f));
							Vector3 position = SecCam.Position;
							Vector3 rotation6 = SecCam.Rotation;
							Vector3 vector3 = ProcessNewPos(SecCam.Position);
							SecCam.Rotation = new Vector3(OffsetRotX, OffsetRotY, OffsetRotZ);
							Vector3 rotation7 = MansionCCTVsObjects.Entities[CurrentCamera].Entity.Rotation;
							if (CurrentCamera < MansionCCTVsObjects.Entities.Count)
							{
								MansionCCTVsObjects.Entities[CurrentCamera].Entity.Rotation = new Vector3(rotation7.X, rotation7.Y, OffsetRotZ);
							}
							float num4 = rotation6.Z * (MathF.PI / 180f);
							float num5 = rotation6.X * (MathF.PI / 180f);
							float num6 = (float)Math.Abs(Math.Cos(num5));
							Vector3 vector4 = new Vector3((float)Math.Sin(num4) * num6 * -1f, (float)Math.Cos(num4) * num6, (float)Math.Sin(num5));
							RaycastResult raycastResult2 = World.Raycast(position, position + vector4 * 25000f, IntersectFlags.Everything);
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To exit CCTV Feed\nUse your Mouse to control the Camera and Observe the Party", 1);
							TEXT("Camera : " + (CurrentCamera + 1), new Point(num3 + 500, 80), 0.65f, Color.White, GTA.UI.Font.HouseScript);
							drawSpriteRTA2("securitycam", "securitycam_scanlines", 0.5f, 0.5f, 1320f, 1920f, 50, 50, 50, 255, 0f);
							drawSpriteRTA2("securitycam", "securitycam_box", 0.5f, 0.5f, 105f, 210f, 180, 180, 180, 255, 0f);
							drawSpriteRTA2("securitycam", "securitycam_scanlines", 0.96359f, 0.11968f, 145f, 150f, 180, 180, 180, 255, 0f);
							drawSpriteRTA2("securitycam", "securitycam_scanlines", 0.96359f, 0.11968f, 145f, 150f, 180, 180, 180, 255, 0f);
							drawSpriteRTA2("securitycam", "securitycam_scanlines", 0.96359f, 0.11968f, 145f, 150f, 180, 180, 180, 255, 0f);
							drawSpriteRTA2("securitycam", "securitycam_scanlines", 0.96359f, 0.11968f, 145f, 150f, 180, 180, 180, 255, 0f);
							ShowCameraControlls();
							if (Game.IsControlJustPressed(Control.PhoneLeft))
							{
								CurrentCamera--;
								if (CurrentCamera > CamerasPosR.Count - 1)
								{
									CurrentCamera = 0;
								}
								if (CurrentCamera < 0)
								{
									CurrentCamera = CamerasPosR.Count - 1;
								}
								Function.Call(Hash.SET_FOCUS_POS_AND_VEL, CamerasPosR[CurrentCamera].Position.X, CamerasPosR[CurrentCamera].Position.Y, CamerasPosR[CurrentCamera].Position.Z, 0, 0, 0);
								SecCam.Position = CamerasPosR[CurrentCamera].Position;
								SecCam.Rotation = new Vector3(0f, 0f, CamerasPosR[CurrentCamera].Heading);
								SecCam.FieldOfView = 80f;
							}
							if (Game.IsControlJustPressed(Control.PhoneRight))
							{
								CurrentCamera++;
								if (CurrentCamera > CamerasPosR.Count - 1)
								{
									CurrentCamera = 0;
								}
								if (CurrentCamera < 0)
								{
									CurrentCamera = CamerasPosR.Count - 1;
								}
								Function.Call(Hash.SET_FOCUS_POS_AND_VEL, CamerasPosR[CurrentCamera].Position.X, CamerasPosR[CurrentCamera].Position.Y, CamerasPosR[CurrentCamera].Position.Z, 0, 0, 0);
								SecCam.Position = CamerasPosR[CurrentCamera].Position;
								SecCam.Rotation = new Vector3(0f, 0f, CamerasPosR[CurrentCamera].Heading);
								SecCam.FieldOfView = 80f;
							}
							if (Game.IsControlPressed(Control.PrevWeapon))
							{
								Notification.Show("Cam Zoom In");
								SecCam.FieldOfView -= 2.5f;
							}
							if (Game.IsControlPressed(Control.NextWeapon))
							{
								Notification.Show("Cam Zoom In");
								SecCam.FieldOfView += 2.5f;
							}
						}
					}
				}
				else if (CurrentHeistStage == HeistStage.VaultKeycard)
				{
					if (CurrentActiveGroup != ActiveGroup.TheBillionariesParty_HeadofSecurity)
					{
						SmartGuards.GuardGroup[24].GroupList.Clear();
						SmartGuards.GuardGroup[24].GroupList.AddRange(HosGuards);
						SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.None;
						CurrentActiveGroup = ActiveGroup.None;
					}
					CurrentHeistStage = HeistStage.Scout;
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, HeadOfSecurityHouseEnter, "<", 100f))
				{
					if (!CreatedHoSInteriorObjects)
					{
						CreatedHoSInteriorObjects = true;
						HeadOfSecurityInteriorHouseObjects.CreateEntites();
						VaultKeycard = (Prop)HeadOfSecurityInteriorHouseObjects.Entities[0].Entity;
					}
					if (CurrentActiveGroup != ActiveGroup.TheBillionariesParty_HeadofSecurity)
					{
						SmartGuards.GuardGroup[24].GroupList.Clear();
						SmartGuards.GuardGroup[24].GroupList.AddRange(HosGuards);
						SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.High_End_Security;
						CurrentActiveGroup = ActiveGroup.TheBillionariesParty_HeadofSecurity;
					}
					World.DrawMarker(MarkerType.Cylinder, HeadOfSecurityHouseEnter, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
					if (GetDistance_VDIST2(Game.Player.Character.Position, HeadOfSecurityHouseEnter, "<", 2f))
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter The Head of Security's House");
						if (Game.IsControlJustPressed(Control.Context))
						{
							Function.Call(Hash.BEGIN_TEXT_COMMAND_BUSYSPINNER_ON, "STRING");
							Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "Loading The Billionaires Party Assets");
							Function.Call(Hash.END_TEXT_COMMAND_BUSYSPINNER_ON, 5);
							Function.Call(Hash.BUSYSPINNER_OFF);
							GTA.UI.Screen.FadeOut(1500);
							Script.Wait(1500);
							inHoSHouse = true;
							Game.Player.Character.Position = HeadOfSecurityHouseExit;
							CurrentActiveGroup = ActiveGroup.TheBillionariesParty_HeadofSecurity;
							Script.Wait(1500);
							GTA.UI.Screen.FadeIn(1500);
						}
					}
				}
			}
			else if (inHoSHouse)
			{
				if (CurrentActiveGroup != ActiveGroup.TheBillionariesParty_HeadofSecurity)
				{
					SmartGuards.GuardGroup[24].GroupList.Clear();
					SmartGuards.GuardGroup[24].GroupList.AddRange(HosGuards);
					SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.High_End_Security;
					CurrentActiveGroup = ActiveGroup.TheBillionariesParty_HeadofSecurity;
				}
				CurrentHeistStage = HeistStage.VaultKeycard;
				if (VaultKeycard != null && !GotVaultKeycard)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, VaultKeycard.Position, "<", 30f))
					{
						World.DrawMarker(MarkerType.Chevron1, VaultKeycard.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), Vector3.Zero, new Vector3(0f, 180f, 0f), new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 255, 0), true, true);
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, VaultKeycard.Position, "<", 10f) && VaultKeycard.AttachedBlip == null)
					{
						VaultKeycard.AddBlip();
						VaultKeycard.AttachedBlip.Sprite = BlipSprite.Document;
						VaultKeycard.AttachedBlip.Name = "VIP Access Pass";
						VaultKeycard.AttachedBlip.Color = BlipColor.Green;
						VaultKeycard.AttachedBlip.IsShortRange = true;
						VaultKeycard.AttachedBlip.IsFlashing = true;
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, VaultKeycard.Position, "<", 2.5f))
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Pickup Vault Keycard");
						if (Game.IsControlJustPressed(Control.Context))
						{
							IntelMarkers[3].Alpha = 0;
							IntelMarkersRadiusArea[3].Alpha = 0;
							if (VaultKeycard.AttachedBlip != null)
							{
								VaultKeycard.AttachedBlip.Alpha = 0;
							}
							EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(VaultKeycard, 0));
							CurrentHeistStage = HeistStage.Scout;
							TextNotification("CHAR_HUMANDEFAULT", "~y~The Billionaires Party~w~", "Intel Found", "You have picked up a Vault Keycard to get open the Billionaires Vault");
							GotVaultKeycard = true;
						}
					}
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, HeadOfSecurityHouseExit, "<", 100f))
				{
					World.DrawMarker(MarkerType.Cylinder, HeadOfSecurityHouseExit, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.65f), Color.FromArgb(0, 147, 255));
					if (GetDistance_VDIST2(Game.Player.Character.Position, HeadOfSecurityHouseExit, "<", 2f))
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit The Head of Security's House");
						if (Game.IsControlJustPressed(Control.Context))
						{
							GTA.UI.Screen.FadeOut(1500);
							Script.Wait(1500);
							inHoSHouse = false;
							CurrentHeistStage = HeistStage.Scout;
							Game.Player.Character.Position = HeadOfSecurityHouseEnter;
							CurrentActiveGroup = ActiveGroup.None;
							Script.Wait(1500);
							GTA.UI.Screen.FadeIn(1500);
						}
					}
				}
			}
			if (!HackedMainKeypad && MainVaultKeypad != null && MainVaultKeypad.Keypad != null && MainVaultKeypad.Keypad.Exists())
			{
				if (GotVaultKeycard)
				{
					CheckKeypad(MainVaultKeypad);
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, MainVaultKeypad.Keypad.Position, "<", 10f))
				{
					if (!HackedMainKeypad)
					{
						if (GotVaultKeycard)
						{
							CurrentHeistStage = HeistStage.HackKeypad;
						}
						else
						{
							CurrentHeistStage = HeistStage.CannotHackKeypad;
						}
					}
					else if (VaultDoor.Rotation.Z >= -30f)
					{
						CurrentHeistStage = HeistStage.AwaitVaultOpen;
					}
					else
					{
						CurrentHeistStage = HeistStage.VaultOpenLoot;
					}
				}
				else if (CurrentHeistStage == HeistStage.HackKeypad || CurrentHeistStage == HeistStage.VaultOpenLoot || CurrentHeistStage == HeistStage.AwaitVaultOpen)
				{
					CurrentHeistStage = HeistStage.PartyMain;
				}
			}
			if (HackedMainKeypad && VaultDoor != null)
			{
				if (VaultDoor.IsAttached())
				{
					VaultDoor.Detach();
				}
				if (VaultDoor.Rotation.Z >= -30f)
				{
					Vector3 rotation8 = new Vector3(0f, 0f, VaultDoor.Rotation.Z - 0.1f);
					VaultDoor.Rotation = rotation8;
				}
			}
			if (CartelLeader != null)
			{
				if (GetDistance_VDIST2(Game.Player.Character.Position, CartelLeader.Position, "<", 10f))
				{
					CurrentHeistStage = HeistStage.CartelPrePurchase;
				}
				else if (CurrentHeistStage == HeistStage.CartelPrePurchase)
				{
					CurrentHeistStage = HeistStage.Scout;
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, CartelLeader.Position, "<", 2f) && !PayedforRoadBlock)
				{
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to pay Cartel Leader $120,000 to setup a roadblock down by terminal docks");
					if (Game.IsControlJustPressed(Control.Context) && Game.Player.Money >= 120000)
					{
						IntelMarkers[2].Alpha = 0;
						IntelMarkersRadiusArea[2].Alpha = 0;
						Game.Player.Money -= 120000;
						PayedforRoadBlock = true;
						RoadBlockObjects.CreateEntites();
					}
				}
			}
			if (GetDistance_VDIST2(Game.Player.Character.Position, CartelIntelLoc, "<", 300f))
			{
				if (!CreatedCartelMeet)
				{
					CartelObjects.CreateEntites();
					CartelLeader = (Ped)CartelObjects.Entities[0].Entity;
					CartelGuards.Clear();
					foreach (ObjectPlacementMap entity3 in CartelObjects.Entities)
					{
						if (entity3.Entity != null && entity3.Entity.Model.IsPed)
						{
							CartelGuards.Add(new SmartGuards.SmartAiV2((Ped)entity3.Entity, SmartGuards.EnemyGroup.Cartel));
						}
					}
					CreatedCartelMeet = true;
				}
				if (CurrentActiveGroup != ActiveGroup.TheBillionariesParty_Cartel)
				{
					CurrentActiveGroup = ActiveGroup.TheBillionariesParty_Cartel;
				}
			}
			else
			{
				CartelObjects.DeleteEntites();
				if (CurrentActiveGroup == ActiveGroup.TheBillionariesParty_Cartel)
				{
					CurrentActiveGroup = ActiveGroup.None;
				}
				CreatedCartelMeet = false;
			}
			if (!Game.Player.IsAlive)
			{
				EndHeist();
				FailHeist("cause the Player Died");
			}
		}
	}
}
