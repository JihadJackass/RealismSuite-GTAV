using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using GTAO_DiamondCasino.PodiumVehicle;
using HKHModHelperNew;
using iFruitAddon2;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Menus;

namespace GTAO_DiamondCasino.Penthouse
{
	internal class PenthouseGarage : Script
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

		public class InteriorGarage
		{
			public Vector3 Interior { get; set; }

			public Vector3 Elevator { get; set; }

			public Quaternion ElevatorInside { get; set; }

			public Quaternion SpawnInPos { get; set; }

			public Vector3 InteriorMenuPos { get; set; }

			public Vector3 GarageDoorL { get; set; }

			public float ExitHeading { get; set; }

			public Vector3 GarageDoorR { get; set; }

			public Vector3 MenuActivator { get; set; }

			public Quaternion Veh0Pos { get; set; }

			public Quaternion Veh1Pos { get; set; }

			public Quaternion Veh2Pos { get; set; }

			public Quaternion Veh3Pos { get; set; }

			public Quaternion Veh4Pos { get; set; }

			public Quaternion Veh5Pos { get; set; }

			public Quaternion Veh6Pos { get; set; }

			public Quaternion Veh7Pos { get; set; }

			public Quaternion Veh8Pos { get; set; }

			public Quaternion Veh9Pos { get; set; }

			public Tuple<Vector3, Vector3> MenuCam { get; set; }
		}

		public enum Nodetype
		{
			AnyRoad,
			Road,
			Offroad,
			Water
		}

		public enum GarageType
		{
			Two,
			Six,
			Ten,
			Fifty
		}

		public static List<Class1.GTAOGarage> Garages = new List<Class1.GTAOGarage>
		{
			new Class1.GTAOGarage(new Vector3(936.52f, 1.4f, 77.76f), new Vector3(934.55f, -1.76f, 77.7f), -34.89f, 10, 0f, 1, "scripts\\RealismSuite\\DiamondCasino\\CasinoGarage\\", "PenthouseGarage", "PenthouseGarage", "", new Tuple<Vector3, Vector3>(new Vector3(923.82f, -18.2f, 84.6f), new Vector3(-16.25f, 0f, -32.5f)))
		};

		public static InteriorGarage TwoCarGarage;

		public static InteriorGarage SixCarGarage;

		public static InteriorGarage TenCarGarage;

		public static InteriorGarage CurrentGarageInterior;

		public static InteriorGarage FiftyCarGarage;

		public static ObjectPool modLemonUIObjectPool;

		public static NativeMenu mainMenu;

		public static ScaledRectangle RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 0));

		public static List<NativeMenu> GLemonUIMenusNativeMenus = new List<NativeMenu>();

		public static int FloorB1_MultistoreyGarage_Style = 0;

		public static int FloorB2_MultistoreyGarage_Style = 0;

		public static int FloorB3_MultistoreyGarage_Style = 0;

		public static int FloorB4_MultistoreyGarage_Style = 0;

		public static int FloorB5_MultistoreyGarage_Style = 0;

		public static int FloorB1_MultistoreyGarage_Color = 0;

		public static int FloorB2_MultistoreyGarage_Color = 0;

		public static int FloorB3_MultistoreyGarage_Color = 0;

		public static int FloorB4_MultistoreyGarage_Color = 0;

		public static int FloorB5_MultistoreyGarage_Color = 0;

		public static bool SetupGarages;

		public static ObjectPool MechanicObjectPool;

		public static NativeMenu MechanicMenu;

		public static CustomiFruit ifruit;

		public static bool ConvStarted;

		public static Ped ConvPed;

		public static int RerouteTimer;

		public static bool SetupMenu;

		public static Vehicle CurrentCar;

		public static Ped MechanicPed;

		public static int CheckGarageTime;

		public static int CurrentGarage;

		public static bool SetupForSaleSigns;

		public static bool InGarage;

		public static GarageType GarageTypeIn;

		public static bool SavingVehicle;

		public static Vehicle Vehicle1;

		public static Vehicle Vehicle2;

		public static Vehicle Vehicle3;

		public static Vehicle Vehicle4;

		public static Vehicle Vehicle5;

		public static Vehicle Vehicle6;

		public static Vehicle Vehicle7;

		public static Vehicle Vehicle8;

		public static Vehicle Vehicle9;

		public static Vehicle Vehicle10;

		public static int LastGarage = -1;

		public static Camera MainCam;

		public static SaveCar SC = new SaveCar();

		public static bool GarageCamActive;

		public static bool DisplayingRemoveVehicleCam;

		public static string CurrentEclipseGarage;

		public static bool JustExitedWithVehicle;

		public static Vector3 SlideOffset;

		public static bool SlidingVehicle;

		public static Prop SlideObject;

		public static void ATTACH_ENTITY_TO_ENTITY(Entity entity1, Entity entity2, int boneIndex, float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, bool p9, bool useSoftPinning, bool collision, bool isPed, int vertexIndex, bool fixedRot)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, entity1, entity2, boneIndex, xPos, yPos, zPos, xRot, yRot, zRot, p9, useSoftPinning, collision, isPed, vertexIndex, fixedRot);
		}

		public static void SetBanner(NativeMenu menu)
		{
			menu.Banner = new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), "shopui_title_casino", "shopui_title_casino");
		}

		public static void CreateBanner()
		{
			RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 0));
			RectangleGUI.Color = Color.FromArgb(255, 0, 0, 0);
		}

		public static Vehicle GetVehicleFromID(int i)
		{
			Vehicle result = null;
			if (i == 1 && Vehicle1 != null)
			{
				result = Vehicle1;
			}
			if (i == 2 && Vehicle2 != null)
			{
				result = Vehicle2;
			}
			if (i == 3 && Vehicle3 != null)
			{
				result = Vehicle3;
			}
			if (i == 4 && Vehicle4 != null)
			{
				result = Vehicle4;
			}
			if (i == 5 && Vehicle5 != null)
			{
				result = Vehicle5;
			}
			if (i == 6 && Vehicle6 != null)
			{
				result = Vehicle6;
			}
			if (i == 7 && Vehicle7 != null)
			{
				result = Vehicle7;
			}
			if (i == 8 && Vehicle8 != null)
			{
				result = Vehicle8;
			}
			if (i == 9 && Vehicle9 != null)
			{
				result = Vehicle9;
			}
			if (i == 10 && Vehicle10 != null)
			{
				result = Vehicle10;
			}
			return result;
		}

		public static Vehicle GetCurrentGarageVehicle()
		{
			Vehicle result = null;
			if (Vehicle1 != null && Vehicle1.GetPedOnSeat(VehicleSeat.Driver) != null && Vehicle1.GetPedOnSeat(VehicleSeat.Driver) == Game.Player.Character)
			{
				result = Vehicle1;
			}
			if (Vehicle2 != null && Vehicle2.GetPedOnSeat(VehicleSeat.Driver) != null && Vehicle2.GetPedOnSeat(VehicleSeat.Driver) == Game.Player.Character)
			{
				result = Vehicle2;
			}
			if (Vehicle3 != null && Vehicle3.GetPedOnSeat(VehicleSeat.Driver) != null && Vehicle3.GetPedOnSeat(VehicleSeat.Driver) == Game.Player.Character)
			{
				result = Vehicle3;
			}
			if (Vehicle4 != null && Vehicle4.GetPedOnSeat(VehicleSeat.Driver) != null && Vehicle4.GetPedOnSeat(VehicleSeat.Driver) == Game.Player.Character)
			{
				result = Vehicle4;
			}
			if (Vehicle5 != null && Vehicle5.GetPedOnSeat(VehicleSeat.Driver) != null && Vehicle5.GetPedOnSeat(VehicleSeat.Driver) == Game.Player.Character)
			{
				result = Vehicle5;
			}
			if (Vehicle6 != null && Vehicle6.GetPedOnSeat(VehicleSeat.Driver) != null && Vehicle6.GetPedOnSeat(VehicleSeat.Driver) == Game.Player.Character)
			{
				result = Vehicle6;
			}
			if (Vehicle7 != null && Vehicle7.GetPedOnSeat(VehicleSeat.Driver) != null && Vehicle7.GetPedOnSeat(VehicleSeat.Driver) == Game.Player.Character)
			{
				result = Vehicle7;
			}
			if (Vehicle8 != null && Vehicle8.GetPedOnSeat(VehicleSeat.Driver) != null && Vehicle8.GetPedOnSeat(VehicleSeat.Driver) == Game.Player.Character)
			{
				result = Vehicle8;
			}
			if (Vehicle9 != null && Vehicle9.GetPedOnSeat(VehicleSeat.Driver) != null && Vehicle9.GetPedOnSeat(VehicleSeat.Driver) == Game.Player.Character)
			{
				result = Vehicle9;
			}
			if (Vehicle10 != null && Vehicle10.GetPedOnSeat(VehicleSeat.Driver) != null && Vehicle10.GetPedOnSeat(VehicleSeat.Driver) == Game.Player.Character)
			{
				result = Vehicle10;
			}
			return result;
		}

		public static void RequestMultistoryGarageFloor(int Floor, int Style, int Color)
		{
			Function.Call(Hash.REQUEST_IPL, "xm3_int_placement_xm3_interior_1_dlc_int_02_xm3_milo_");
			int num = 290561;
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_numbers_01");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_numbers_02");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_numbers_03");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_numbers_04");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_numbers_05");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_shell_01");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_shell_02");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_shell_03");
			Function.Call(Hash.REFRESH_INTERIOR, num);
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_tint_01");
			Function.Call((Hash)3590001984630001091uL, num, "entity_set_tint_01", Color);
			if (Style == 0)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_shell_01");
				Function.Call((Hash)3590001984630001091uL, num, "entity_set_shell_01", Color);
			}
			if (Style == 1)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_shell_02");
				Function.Call((Hash)3590001984630001091uL, num, "entity_set_shell_02", Color);
			}
			if (Style == 2)
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_shell_03");
				Function.Call((Hash)3590001984630001091uL, num, "entity_set_shell_03", Color);
			}
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_numbers_0" + Floor);
			Function.Call(Hash.REFRESH_INTERIOR, num);
		}

		public PenthouseGarage()
		{
			if (_Debug.PenthouseGarage != -1)
			{
				CreateBanner();
				modLemonUIObjectPool = new ObjectPool();
				mainMenu = new NativeMenu(" ", "Select an Option");
				GLemonUIMenusNativeMenus.Add(mainMenu);
				modLemonUIObjectPool.Add(mainMenu);
				MechanicObjectPool = new ObjectPool();
				MechanicMenu = new NativeMenu("Penthouse Garage", "Select an Option");
				GLemonUIMenusNativeMenus.Add(MechanicMenu);
				MechanicObjectPool.Add(MechanicMenu);
				ifruit = new CustomiFruit
				{
					CenterButtonColor = Color.Orange,
					LeftButtonColor = Color.LimeGreen,
					RightButtonColor = Color.Purple,
					CenterButtonIcon = SoftKeyIcon.Fire,
					LeftButtonIcon = SoftKeyIcon.Police,
					RightButtonIcon = SoftKeyIcon.Website
				};
				ifruit.SetWallpaper(new Wallpaper("char_facebook"));
				ifruit.SetWallpaper(Wallpaper.BadgerDefault);
				iFruitContact iFruitContact = new iFruitContact("Mechanic (Penthouse Garages)");
				iFruitContact.Answered += loadMenu;
				iFruitContact.DialTimeout = 3000;
				iFruitContact.Active = true;
				iFruitContact.Icon = ContactIcon.Mechanic;
				iFruitContact.Name = "Mechanic (Penthouse Garages)";
				ifruit.Contacts.Add(iFruitContact);
				TwoCarGarage = new InteriorGarage();
				TwoCarGarage.Interior = new Vector3(173.1176f, -1003.279f, -99f);
				TwoCarGarage.Elevator = new Vector3(179.1001f, -1005.655f, -99.9999f);
				TwoCarGarage.ElevatorInside = new Quaternion(179.0661f, -1002.717f, -99.99992f, 179.181f);
				TwoCarGarage.SpawnInPos = new Quaternion(176.6249f, -1007.737f, -99.9999f, 32.54933f);
				TwoCarGarage.GarageDoorL = new Vector3(172.9447f, -1008.339f, -99.9999f);
				TwoCarGarage.ExitHeading = 180f;
				TwoCarGarage.MenuActivator = new Vector3(178.9034f, -1007.407f, -99.99998f);
				TwoCarGarage.Veh0Pos = new Quaternion(175.2132f, -1004.104f, -99.99999f, -178.4487f);
				TwoCarGarage.Veh1Pos = new Quaternion(171.7141f, -1004.023f, -99.99999f, -178.4487f);
				TwoCarGarage.MenuCam = new Tuple<Vector3, Vector3>(new Vector3(179.6135f, -1008.107f, -97.29996f), new Vector3(-24.14237f, 0f, 57.91766f));
				SixCarGarage = new InteriorGarage();
				SixCarGarage.Interior = new Vector3(193.9493f, -1004.425f, -99.99999f);
				SixCarGarage.Elevator = new Vector3(207.1506f, -998.9948f, -99.9999f);
				SixCarGarage.ElevatorInside = new Quaternion(209.4215f, -999.0895f, -100f, 90.90394f);
				SixCarGarage.SpawnInPos = new Quaternion(198.3316f, -1007.227f, -99.99992f, 21.38214f);
				SixCarGarage.GarageDoorL = new Vector3(202.2906f, -1007.725f, -99.9999f);
				SixCarGarage.GarageDoorR = new Vector3(194.4465f, -1007.733f, -99.9999f);
				SixCarGarage.ExitHeading = 180f;
				SixCarGarage.MenuActivator = new Vector3(204.1768f, -995.3179f, -99.9999f);
				SixCarGarage.Veh0Pos = new Quaternion(197.5f, -1004.425f, -99.99999f, -4.035995f);
				SixCarGarage.Veh1Pos = new Quaternion(201.06f, -1004.425f, -99.99999f, -4.035995f);
				SixCarGarage.Veh2Pos = new Quaternion(204.62f, -1004.425f, -99.99999f, -4.035995f);
				SixCarGarage.Veh3Pos = new Quaternion(192.9262f, -996.3292f, -99.99999f, 146.2832f);
				SixCarGarage.Veh4Pos = new Quaternion(197.5f, -996.3292f, -99.99999f, 146.2832f);
				SixCarGarage.Veh5Pos = new Quaternion(203.9257f, -999.1467f, -99.99999f, 146.2832f);
				SixCarGarage.MenuCam = new Tuple<Vector3, Vector3>(new Vector3(205.5775f, -1006.326f, -97.99998f), new Vector3(-13.71649f, 0f, 48.66722f));
				TenCarGarage = new InteriorGarage();
				TenCarGarage.Interior = new Vector3(1290.662f, 232.7f, -50f);
				TenCarGarage.SpawnInPos = new Quaternion(1249.75f, 223.11f, -49f, -90f);
				TenCarGarage.GarageDoorL = new Vector3(1249.75f, 223.11f, -49f);
				TenCarGarage.GarageDoorR = new Vector3(1249.83f, 230.29f, -49f);
				TenCarGarage.ExitHeading = 82f;
				TenCarGarage.MenuActivator = new Vector3(1285.84f, 226.4f, -50f);
				TenCarGarage.Veh0Pos = new Quaternion(1280.879f, 240.9134f, -50f, -90f);
				TenCarGarage.Veh1Pos = new Quaternion(1281.198f, 249.7228f, -50f, -90f);
				TenCarGarage.Veh2Pos = new Quaternion(1281.198f, 249.7228f, -50f, -90f);
				TenCarGarage.Veh3Pos = new Quaternion(1295.508f, 250.2121f, -50f, 90f);
				TenCarGarage.Veh4Pos = new Quaternion(1296.03f, 241.113f, -50f, -90f);
				TenCarGarage.Veh5Pos = new Quaternion(1295.046f, 231.8767f, -50f, 90f);
				TenCarGarage.Veh6Pos = new Quaternion(1310.02f, 231.9766f, -50f, 90f);
				TenCarGarage.Veh7Pos = new Quaternion(1309.28f, 241.4474f, -50f, 90f);
				TenCarGarage.Veh8Pos = new Quaternion(1309.502f, 249.2502f, -50f, 90f);
				TenCarGarage.Veh9Pos = new Quaternion(1309.79f, 258.2268f, -50f, 90f);
				TenCarGarage.MenuCam = new Tuple<Vector3, Vector3>(new Vector3(1313.17f, 219.6f, -47.2f), new Vector3(-4.7f, 0f, 41f));
				FiftyCarGarage = new InteriorGarage();
				FiftyCarGarage.Interior = new Vector3(519.72f, -2639.8f, -50f);
				FiftyCarGarage.Elevator = new Vector3(529.87f, -2637.868f, 50f);
				FiftyCarGarage.ElevatorInside = new Quaternion(531.83f, -2637.72f, -50f, 91.21181f);
				FiftyCarGarage.SpawnInPos = new Quaternion(519.72f, -2639.8f, -50f, 359.533f);
				FiftyCarGarage.GarageDoorL = new Vector3(519.72f, -2639.8f, -50f);
				FiftyCarGarage.GarageDoorR = new Vector3(0f, 0f, 0f);
				FiftyCarGarage.ExitHeading = 180f;
				FiftyCarGarage.MenuActivator = new Vector3(514.44f, -2607.667f, -50f);
				FiftyCarGarage.Veh0Pos = new Quaternion(514.31f, -2630.35f, -50f, -118f);
				FiftyCarGarage.Veh1Pos = new Quaternion(514.41f, -2625.631f, -50f, -118f);
				FiftyCarGarage.Veh2Pos = new Quaternion(514.23f, -2621.597f, -50f, -118f);
				FiftyCarGarage.Veh3Pos = new Quaternion(514.18f, -2617.27f, -50f, -118f);
				FiftyCarGarage.Veh4Pos = new Quaternion(514.23f, -2613.2f, -50f, -118f);
				FiftyCarGarage.Veh5Pos = new Quaternion(524.36f, -2613.885f, -50f, 105f);
				FiftyCarGarage.Veh6Pos = new Quaternion(524.36f, -2617.27f, -50f, 105f);
				FiftyCarGarage.Veh7Pos = new Quaternion(524.36f, -2621.597f, -50f, 105f);
				FiftyCarGarage.Veh8Pos = new Quaternion(524.36f, -2625.631f, -50f, 105f);
				FiftyCarGarage.Veh9Pos = new Quaternion(524.36f, -2630.35f, -50f, 105f);
				FiftyCarGarage.MenuCam = new Tuple<Vector3, Vector3>(new Vector3(520.04f, -2639.491f, -47.16f), new Vector3(-8.18f, 0f, 0f));
				FiftyCarGarage.InteriorMenuPos = new Vector3(523.43f, -2639.613f, -50f);
				base.Tick += OnTick;
			}
		}

		public static void GetSlotContents(Class1.GTAOGarage G, NativeItem Item, int SlodID, string EclipseGarage)
		{
			Model model = SC.CheckCar(G.GarageVehicleSavePath + EclipseGarage + "Slot" + SlodID + ".ini");
			if (model == "null" || model == null)
			{
				Item.AltTitle = "EMPTY";
			}
			else
			{
				Item.AltTitle = SC.VehicleName;
			}
		}

		public static bool IsSlotFree(Class1.GTAOGarage G, int SlodID, string EclipseGarage)
		{
			bool flag = true;
			Model model = SC.CheckCar(G.GarageVehicleSavePath + EclipseGarage + "Slot" + SlodID + ".ini");
			if (model == "null" || model == null)
			{
				return true;
			}
			return false;
		}

		public static void OrderVehicleFromSlot(Class1.GTAOGarage G, int SlodID, string EclipseGarage)
		{
			if (!IsSlotFree(G, SlodID, EclipseGarage))
			{
				try
				{
					if (CurrentCar != null)
					{
						CurrentCar.Delete();
					}
					if (MechanicPed != null)
					{
						MechanicPed.Delete();
					}
					if (ConvPed != null)
					{
						ConvPed.Delete();
					}
					SC.LoadIniFile(G.GarageVehicleSavePath + EclipseGarage + "Slot" + SlodID + ".ini");
					ConvPed = GlobalHelper.CreatePed(RequestModel(PedHash.LesterCrest), Game.Player.Character.Position.Around(3f));
					ConvPed.Opacity = 0;
					ConvPed.IsInvincible = true;
					ConvPed.IsCollisionEnabled = false;
					ConvPed.AttachTo(Game.Player.Character);
					ConvPed.BlockPermanentEvents = true;
					ConvStarted = true;
					Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
					Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, ConvPed, "mechanic");
					Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "MPCT_DGAA", "mechanic", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
					Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
					if (MechanicPed != null)
					{
						MechanicPed.Delete();
					}
					if (CurrentCar != null)
					{
						if (Game.Player.Character.CurrentVehicle != CurrentCar && GetDistance_VDIST2(Game.Player.Character.Position, CurrentCar.Position, ">", 100f))
						{
							CurrentCar.Delete();
						}
						else
						{
							CurrentCar.IsPersistent = false;
						}
					}
					CurrentCar = GlobalHelper.CreateVehicle(RequestModel(SC.VehicleName), GenerateSpawnPos(Game.Player.Character.Position.Around(100f), Nodetype.AnyRoad, false), 147f);
					if (CurrentCar != null)
					{
						CurrentCar.PlaceOnGround();
						CurrentCar.PlaceOnNextStreet();
						SC.Load(CurrentCar);
						CurrentCar.IsDriveable = true;
					}
					if (CurrentCar == null)
					{
						Notification.Show("No Car Found!");
					}
					MechanicPed = GlobalHelper.CreatePed(RequestModel(PedHash.WareMechMale01), CurrentCar.Position, 0f);
					MechanicPed.SetIntoVehicle(CurrentCar, VehicleSeat.Driver);
					MechanicPed.Task.DriveTo(CurrentCar, Game.Player.Character.Position, 5f, 60f);
					MechanicPed.RelationshipGroup = 2;
					MechanicPed.BlockPermanentEvents = true;
					CurrentCar.AddBlip();
					CurrentCar.AttachedBlip.Sprite = BlipSprite.GetawayCar;
					CurrentCar.AttachedBlip.Name = CurrentCar.LocalizedName;
					Notification.Show("Mechanic : Im on my way with your " + CurrentCar.LocalizedName);
					return;
				}
				catch
				{
					if (CurrentCar != null)
					{
						CurrentCar.Delete();
					}
					if (MechanicPed != null)
					{
						MechanicPed.Delete();
					}
					return;
				}
			}
			ConvStarted = true;
			ConvPed = GlobalHelper.CreatePed(RequestModel(PedHash.LesterCrest), Game.Player.Character.Position.Around(3f));
			ConvPed.Opacity = 0;
			ConvPed.IsInvincible = true;
			ConvPed.IsCollisionEnabled = false;
			ConvPed.AttachTo(Game.Player.Character);
			ConvPed.BlockPermanentEvents = true;
			Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
			Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, ConvPed, "mechanic");
			Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "MPCT_KNAA", "mechanic", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
			Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
			ConvStarted = true;
			Notification.Show("No Vehicle In Slot!");
		}

		public static void loadMenu(iFruitContact contact)
		{
			ConvPed = GlobalHelper.CreatePed(RequestModel(PedHash.LesterCrest), Game.Player.Character.Position.Around(3f));
			ConvPed.Opacity = 0;
			ConvPed.IsInvincible = true;
			ConvPed.IsCollisionEnabled = false;
			ConvPed.AttachTo(Game.Player.Character);
			ConvPed.BlockPermanentEvents = true;
			Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
			Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, ConvPed, "mechanic");
			Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "MPCT_AKAA", "mechanic", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
			ConvStarted = true;
			ifruit.Close();
			MechanicMenu.Visible = !MechanicMenu.Visible;
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

		public static string LoadDict(string dict)
		{
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, dict))
			{
				Function.Call(Hash.REQUEST_ANIM_DICT, dict);
				Script.Yield();
			}
			return dict;
		}

		public static void DisplayHelpTextThisFrame(string helpText, int Shape)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, Shape);
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

		public static float GetDistance(Vector3 _1, Vector3 _2)
		{
			return (float)Math.Sqrt(Function.Call<float>(Hash.VDIST2, _1.X, _1.Y, _1.Z, _2.X, _2.Y, _2.Z));
		}

		public static void ResetVehicleUpgrades()
		{
			SaveCar sC = SC;
			sC.Config.SetValue("Configurations", "VehicleNameHash", -1);
			sC.Config.Save();
			sC.Config.SetValue("Configurations", "VehicleHash", -1);
			sC.Config.Save();
			sC.Config.SetValue("Configurations", "VehicleName", "null");
			sC.Config.Save();
			sC.Config.SetValue("Configurations", "Roof", -1);
			sC.Config.SetValue("Configurations", "AerialsId", -1);
			sC.Config.SetValue("Configurations", "AirfilterId", -1);
			sC.Config.SetValue("Configurations", "ArchCoverId", -1);
			sC.Config.SetValue("Configurations", "ArmorId", -1);
			sC.Config.SetValue("Configurations", "BackWheelsId", -1);
			sC.Config.SetValue("Configurations", "BrakesId", -1);
			sC.Config.SetValue("Configurations", "ColumnShifterLeversId", -1);
			sC.Config.SetValue("Configurations", "DashboardId", -1);
			sC.Config.SetValue("Configurations", "DialDesignId", -1);
			sC.Config.SetValue("Configurations", "DoorSpeekersId", -1);
			sC.Config.SetValue("Configurations", "EngineId", -1);
			sC.Config.SetValue("Configurations", "EngineBlockId", -1);
			sC.Config.SetValue("Configurations", "ExaustId", -1);
			sC.Config.SetValue("Configurations", "FenderId", -1);
			sC.Config.SetValue("Configurations", "FrameId", -1);
			sC.Config.SetValue("Configurations", "FrontBumperId", -1);
			sC.Config.SetValue("Configurations", "FrontWheelsId", -1);
			sC.Config.SetValue("Configurations", "GrilleId", -1);
			sC.Config.SetValue("Configurations", "HoodId", -1);
			sC.Config.SetValue("Configurations", "HornsId", -1);
			sC.Config.SetValue("Configurations", "HydralicsId", -1);
			sC.Config.SetValue("Configurations", "LiveryId", -1);
			sC.Config.SetValue("Configurations", "SecondaryVehicleLivery", -1);
			sC.Config.SetValue("Configurations", "OrnamentsId", -1);
			sC.Config.SetValue("Configurations", "PlaquesId", -1);
			sC.Config.SetValue("Configurations", "Plateholder", -1);
			sC.Config.SetValue("Configurations", "RearBumperId", -1);
			sC.Config.SetValue("Configurations", "RightFenderId", -1);
			sC.Config.SetValue("Configurations", "SeatsId", -1);
			sC.Config.SetValue("Configurations", "speakers", -1);
			sC.Config.SetValue("Configurations", "speakers", -1);
			sC.Config.SetValue("Configurations", "SpoilersId", -1);
			sC.Config.SetValue("Configurations", "SteeringWheelId", -1);
			sC.Config.SetValue("Configurations", "StrutId", -1);
			sC.Config.SetValue("Configurations", "RoofId", -1);
			sC.Config.SetValue("Configurations", "SuspensionId", -1);
			sC.Config.SetValue("Configurations", "TransmissionId", -1);
			sC.Config.SetValue("Configurations", "TrimDesignId", -1);
			sC.Config.SetValue("Configurations", "TrimDesignId", -1);
			sC.Config.SetValue("Configurations", "TrunkId", -1);
			sC.Config.SetValue("Configurations", "VanityPlatesId", -1);
			sC.Config.SetValue("Configurations", "WindowId", -1);
			sC.Config.SetValue("Configurations", "TintId", VehicleWindowTint.None);
			sC.Config.SetValue("Configurations", "Wheelcolour", VehicleColor.MatteBlack);
			sC.Config.SetValue("Configurations", "DashBoardColour", VehicleColor.MatteBlack);
			sC.Config.SetValue("Configurations", "PearlCent", VehicleColor.MatteBlack);
			sC.Config.SetValue("Configurations", "NeonColour", Color.Black);
			sC.Config.SetValue("Configurations", "PlanteNo", "");
			sC.Config.SetValue("Configurations", "CustomTiresOn", false);
			sC.Config.SetValue("Configurations", "Blades", -1);
			sC.Config.SetValue("Configurations", "HasXenonLights", false);
			sC.Config.SetValue("Configurations", "TurboActive", false);
			sC.Config.SetValue("Configurations", "LightColor", -1);
			sC.Config.SetValue("Configurations", "NeonColor", Color.Black);
			sC.Config.SetValue("Configurations", "RightNeonOn", false);
			sC.Config.SetValue("Configurations", "LeftNeonOn", false);
			sC.Config.SetValue("Configurations", "FrontNeonOn", false);
			sC.Config.SetValue("Configurations", "BackNeonOn", false);
			sC.Config.SetValue("Configurations", "Wheelcolour", VehicleColor.MatteBlack);
			sC.Config.SetValue("Configurations", "DashBoardColour", VehicleColor.MatteBlack);
			sC.Config.SetValue("Configurations", "TireSmokeColor", Color.Black);
			sC.Config.SetValue("Configurations", "TireSmoke", false);
			sC.Config.SetValue("Configurations", "TrimColor", VehicleColor.MatteBlack);
			sC.Config.SetValue("Configurations", "BulletProofTires", false);
			sC.Config.SetValue("Configurations", "WheelType", -1);
			sC.Config.SetValue("Configurations", "UseCustomPrimaryColor", false);
			sC.Config.SetValue("Configurations", "PrimaryColor", VehicleColor.DefaultAlloyColor);
			sC.Config.SetValue("Configurations", "UseCustomSecondaryColor", false);
			sC.Config.SetValue("Configurations", "SecondaryColor", VehicleColor.DefaultAlloyColor);
			sC.Config.Save();
		}

		public static Vector2 World3DToScreen2d(Vector3 pos)
		{
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, pos.X, pos.Y, pos.Z, outputArgument, outputArgument2);
			return new Vector2(outputArgument.GetResult<float>(), outputArgument2.GetResult<float>());
		}

		public static string GetSlot(string Path, bool IsEclipse, Class1.GTAOGarage G)
		{
			string text = "No Slot Free";
			if (G.GarageSize == 2)
			{
				SC.LoadIniFile(Path + "\\Slot1.ini");
				Model model = SC.CheckCar(Path + "\\Slot1.ini");
				if ((model == "null" || model == null) && text.Equals("No Slot Free"))
				{
					text = "Slot1.ini";
				}
				SC.LoadIniFile(Path + "\\Slot2.ini");
				Model model2 = SC.CheckCar(Path + "\\Slot2.ini");
				if ((model2 == "null" || model2 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot2.ini";
				}
			}
			if (G.GarageSize == 6)
			{
				SC.LoadIniFile(Path + "\\Slot1.ini");
				Model model3 = SC.CheckCar(Path + "\\Slot1.ini");
				if ((model3 == "null" || model3 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot1.ini";
				}
				SC.LoadIniFile(Path + "\\Slot2.ini");
				Model model4 = SC.CheckCar(Path + "\\Slot2.ini");
				if ((model4 == "null" || model4 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot2.ini";
				}
				SC.LoadIniFile(Path + "\\Slot3.ini");
				Model model5 = SC.CheckCar(Path + "\\Slot3.ini");
				if ((model5 == "null" || model5 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot3.ini";
				}
				SC.LoadIniFile(Path + "\\Slot4.ini");
				Model model6 = SC.CheckCar(Path + "\\Slot4.ini");
				if ((model6 == "null" || model6 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot4.ini";
				}
				SC.LoadIniFile(Path + "\\Slot5.ini");
				Model model7 = SC.CheckCar(Path + "\\Slot5.ini");
				if ((model7 == "null" || model7 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot5.ini";
				}
				SC.LoadIniFile(Path + "\\Slot6.ini");
				Model model8 = SC.CheckCar(Path + "\\Slot6.ini");
				if ((model8 == "null" || model8 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot6.ini";
				}
			}
			if (G.GarageSize == 10)
			{
				SC.LoadIniFile(Path + "\\Slot1.ini");
				Model model9 = SC.CheckCar(Path + "\\Slot1.ini");
				if ((model9 == "null" || model9 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot1.ini";
				}
				SC.LoadIniFile(Path + "\\Slot2.ini");
				Model model10 = SC.CheckCar(Path + "\\Slot2.ini");
				if ((model10 == "null" || model10 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot2.ini";
				}
				SC.LoadIniFile(Path + "\\Slot3.ini");
				Model model11 = SC.CheckCar(Path + "\\Slot3.ini");
				if ((model11 == "null" || model11 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot3.ini";
				}
				SC.LoadIniFile(Path + "\\Slot4.ini");
				Model model12 = SC.CheckCar(Path + "\\Slot4.ini");
				if ((model12 == "null" || model12 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot4.ini";
				}
				SC.LoadIniFile(Path + "\\Slot5.ini");
				Model model13 = SC.CheckCar(Path + "\\Slot5.ini");
				if ((model13 == "null" || model13 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot5.ini";
				}
				SC.LoadIniFile(Path + "\\Slot6.ini");
				Model model14 = SC.CheckCar(Path + "\\Slot6.ini");
				if ((model14 == "null" || model14 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot6.ini";
				}
				SC.LoadIniFile(Path + "\\Slot7.ini");
				Model model15 = SC.CheckCar(Path + "\\Slot7.ini");
				if ((model15 == "null" || model15 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot7.ini";
				}
				SC.LoadIniFile(Path + "\\Slot8.ini");
				Model model16 = SC.CheckCar(Path + "\\Slot8.ini");
				if ((model16 == "null" || model16 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot8.ini";
				}
				SC.LoadIniFile(Path + "\\Slot9.ini");
				Model model17 = SC.CheckCar(Path + "\\Slot9.ini");
				if ((model17 == "null" || model17 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot9.ini";
				}
				SC.LoadIniFile(Path + "\\Slot10.ini");
				Model model18 = SC.CheckCar(Path + "\\Slot10.ini");
				if ((model18 == "null" || model18 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot10.ini";
				}
			}
			if (G.GarageSize == 50)
			{
				SC.LoadIniFile(Path + "\\Slot1.ini");
				Model model19 = SC.CheckCar(Path + "\\Slot1.ini");
				if ((model19 == "null" || model19 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot1.ini";
				}
				SC.LoadIniFile(Path + "\\Slot2.ini");
				Model model20 = SC.CheckCar(Path + "\\Slot2.ini");
				if ((model20 == "null" || model20 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot2.ini";
				}
				SC.LoadIniFile(Path + "\\Slot3.ini");
				Model model21 = SC.CheckCar(Path + "\\Slot3.ini");
				if ((model21 == "null" || model21 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot3.ini";
				}
				SC.LoadIniFile(Path + "\\Slot4.ini");
				Model model22 = SC.CheckCar(Path + "\\Slot4.ini");
				if ((model22 == "null" || model22 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot4.ini";
				}
				SC.LoadIniFile(Path + "\\Slot5.ini");
				Model model23 = SC.CheckCar(Path + "\\Slot5.ini");
				if ((model23 == "null" || model23 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot5.ini";
				}
				SC.LoadIniFile(Path + "\\Slot6.ini");
				Model model24 = SC.CheckCar(Path + "\\Slot6.ini");
				if ((model24 == "null" || model24 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot6.ini";
				}
				SC.LoadIniFile(Path + "\\Slot7.ini");
				Model model25 = SC.CheckCar(Path + "\\Slot7.ini");
				if ((model25 == "null" || model25 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot7.ini";
				}
				SC.LoadIniFile(Path + "\\Slot8.ini");
				Model model26 = SC.CheckCar(Path + "\\Slot8.ini");
				if ((model26 == "null" || model26 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot8.ini";
				}
				SC.LoadIniFile(Path + "\\Slot9.ini");
				Model model27 = SC.CheckCar(Path + "\\Slot9.ini");
				if ((model27 == "null" || model27 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot9.ini";
				}
				SC.LoadIniFile(Path + "\\Slot10.ini");
				Model model28 = SC.CheckCar(Path + "\\Slot10.ini");
				if ((model28 == "null" || model28 == null) && text.Equals("No Slot Free"))
				{
					text = "Slot10.ini";
				}
			}
			return text;
		}

		public static int GetSlotID(string Path, bool IsEclipse, Class1.GTAOGarage G)
		{
			string text = "No Slot Free";
			int result = -1;
			if (G.GarageSize == 2)
			{
				SC.LoadIniFile(Path + "\\Slot1.ini");
				Model model = SC.CheckCar(Path + "\\Slot1.ini");
				if ((model == "null" || model == null) && text.Equals("No Slot Free"))
				{
					result = 1;
					text = result.ToString();
				}
				SC.LoadIniFile(Path + "\\Slot2.ini");
				Model model2 = SC.CheckCar(Path + "\\Slot2.ini");
				if ((model2 == "null" || model2 == null) && text.Equals("No Slot Free"))
				{
					result = 2;
					text = result.ToString();
				}
			}
			if (G.GarageSize == 6)
			{
				SC.LoadIniFile(Path + "\\Slot1.ini");
				Model model3 = SC.CheckCar(Path + "\\Slot1.ini");
				if ((model3 == "null" || model3 == null) && text.Equals("No Slot Free"))
				{
					result = 1;
					text = result.ToString();
				}
				SC.LoadIniFile(Path + "\\Slot2.ini");
				Model model4 = SC.CheckCar(Path + "\\Slot2.ini");
				if ((model4 == "null" || model4 == null) && text.Equals("No Slot Free"))
				{
					result = 2;
					text = result.ToString();
				}
				SC.LoadIniFile(Path + "\\Slot3.ini");
				Model model5 = SC.CheckCar(Path + "\\Slot3.ini");
				if ((model5 == "null" || model5 == null) && text.Equals("No Slot Free"))
				{
					result = 3;
					text = result.ToString();
				}
				SC.LoadIniFile(Path + "\\Slot4.ini");
				Model model6 = SC.CheckCar(Path + "\\Slot4.ini");
				if ((model6 == "null" || model6 == null) && text.Equals("No Slot Free"))
				{
					result = 4;
					text = result.ToString();
				}
				SC.LoadIniFile(Path + "\\Slot5.ini");
				Model model7 = SC.CheckCar(Path + "\\Slot5.ini");
				if ((model7 == "null" || model7 == null) && text.Equals("No Slot Free"))
				{
					result = 5;
					text = result.ToString();
				}
				SC.LoadIniFile(Path + "\\Slot6.ini");
				Model model8 = SC.CheckCar(Path + "\\Slot6.ini");
				if ((model8 == "null" || model8 == null) && text.Equals("No Slot Free"))
				{
					result = 6;
					text = result.ToString();
				}
			}
			if (G.GarageSize == 10)
			{
				SC.LoadIniFile(Path + "\\Slot1.ini");
				Model model9 = SC.CheckCar(Path + "\\Slot1.ini");
				if ((model9 == "null" || model9 == null) && text.Equals("No Slot Free"))
				{
					result = 1;
					text = result.ToString();
				}
				SC.LoadIniFile(Path + "\\Slot2.ini");
				Model model10 = SC.CheckCar(Path + "\\Slot2.ini");
				if ((model10 == "null" || model10 == null) && text.Equals("No Slot Free"))
				{
					result = 2;
					text = result.ToString();
				}
				SC.LoadIniFile(Path + "\\Slot3.ini");
				Model model11 = SC.CheckCar(Path + "\\Slot3.ini");
				if ((model11 == "null" || model11 == null) && text.Equals("No Slot Free"))
				{
					result = 3;
					text = result.ToString();
				}
				SC.LoadIniFile(Path + "\\Slot4.ini");
				Model model12 = SC.CheckCar(Path + "\\Slot4.ini");
				if ((model12 == "null" || model12 == null) && text.Equals("No Slot Free"))
				{
					result = 4;
					text = result.ToString();
				}
				SC.LoadIniFile(Path + "\\Slot5.ini");
				Model model13 = SC.CheckCar(Path + "\\Slot5.ini");
				if ((model13 == "null" || model13 == null) && text.Equals("No Slot Free"))
				{
					result = 5;
					text = result.ToString();
				}
				SC.LoadIniFile(Path + "\\Slot6.ini");
				Model model14 = SC.CheckCar(Path + "\\Slot6.ini");
				if ((model14 == "null" || model14 == null) && text.Equals("No Slot Free"))
				{
					result = 6;
					text = result.ToString();
				}
				SC.LoadIniFile(Path + "\\Slot7.ini");
				Model model15 = SC.CheckCar(Path + "\\Slot7.ini");
				if ((model15 == "null" || model15 == null) && text.Equals("No Slot Free"))
				{
					result = 7;
					text = result.ToString();
				}
				SC.LoadIniFile(Path + "\\Slot8.ini");
				Model model16 = SC.CheckCar(Path + "\\Slot8.ini");
				if ((model16 == "null" || model16 == null) && text.Equals("No Slot Free"))
				{
					result = 8;
					text = result.ToString();
				}
				SC.LoadIniFile(Path + "\\Slot9.ini");
				Model model17 = SC.CheckCar(Path + "\\Slot9.ini");
				if ((model17 == "null" || model17 == null) && text.Equals("No Slot Free"))
				{
					result = 9;
					text = result.ToString();
				}
				SC.LoadIniFile(Path + "\\Slot10.ini");
				Model model18 = SC.CheckCar(Path + "\\Slot10.ini");
				if ((model18 == "null" || model18 == null) && text.Equals("No Slot Free"))
				{
					result = 10;
					text = result.ToString();
				}
			}
			if (G.GarageSize == 50)
			{
				SC.LoadIniFile(Path + "\\Slot1.ini");
				Model model19 = SC.CheckCar(Path + "\\Slot1.ini");
				if ((model19 == "null" || model19 == null) && text.Equals("No Slot Free"))
				{
					result = 1;
					text = result.ToString();
				}
				SC.LoadIniFile(Path + "\\Slot2.ini");
				Model model20 = SC.CheckCar(Path + "\\Slot2.ini");
				if ((model20 == "null" || model20 == null) && text.Equals("No Slot Free"))
				{
					result = 2;
					text = result.ToString();
				}
				SC.LoadIniFile(Path + "\\Slot3.ini");
				Model model21 = SC.CheckCar(Path + "\\Slot3.ini");
				if ((model21 == "null" || model21 == null) && text.Equals("No Slot Free"))
				{
					result = 3;
					text = result.ToString();
				}
				SC.LoadIniFile(Path + "\\Slot4.ini");
				Model model22 = SC.CheckCar(Path + "\\Slot4.ini");
				if ((model22 == "null" || model22 == null) && text.Equals("No Slot Free"))
				{
					result = 4;
					text = result.ToString();
				}
				SC.LoadIniFile(Path + "\\Slot5.ini");
				Model model23 = SC.CheckCar(Path + "\\Slot5.ini");
				if ((model23 == "null" || model23 == null) && text.Equals("No Slot Free"))
				{
					result = 5;
					text = result.ToString();
				}
				SC.LoadIniFile(Path + "\\Slot6.ini");
				Model model24 = SC.CheckCar(Path + "\\Slot6.ini");
				if ((model24 == "null" || model24 == null) && text.Equals("No Slot Free"))
				{
					result = 6;
					text = result.ToString();
				}
				SC.LoadIniFile(Path + "\\Slot7.ini");
				Model model25 = SC.CheckCar(Path + "\\Slot7.ini");
				if ((model25 == "null" || model25 == null) && text.Equals("No Slot Free"))
				{
					result = 7;
					text = result.ToString();
				}
				SC.LoadIniFile(Path + "\\Slot8.ini");
				Model model26 = SC.CheckCar(Path + "\\Slot8.ini");
				if ((model26 == "null" || model26 == null) && text.Equals("No Slot Free"))
				{
					result = 8;
					text = result.ToString();
				}
				SC.LoadIniFile(Path + "\\Slot9.ini");
				Model model27 = SC.CheckCar(Path + "\\Slot9.ini");
				if ((model27 == "null" || model27 == null) && text.Equals("No Slot Free"))
				{
					result = 9;
					text = result.ToString();
				}
				SC.LoadIniFile(Path + "\\Slot10.ini");
				Model model28 = SC.CheckCar(Path + "\\Slot10.ini");
				if ((model28 == "null" || model28 == null) && text.Equals("No Slot Free"))
				{
					result = 10;
					text = result.ToString();
				}
			}
			return result;
		}

		public static bool isSlotFree(string Path, int SlotID)
		{
			bool result = true;
			SC.LoadIniFile(Path + "\\Slot" + SlotID + ".ini");
			Model model = SC.CheckCar(Path + "\\Slot" + SlotID + ".ini");
			if (model != "null" && model != null)
			{
				result = false;
			}
			return result;
		}

		public static void SaveVehicleAdv(bool IsEclipse, Class1.GTAOGarage G, string EclipseGarage, InteriorGarage GarageType)
		{
			string garageVehicleSavePath = G.GarageVehicleSavePath;
			string slot = GetSlot(garageVehicleSavePath + EclipseGarage, IsEclipse, G);
			int slotID = GetSlotID(garageVehicleSavePath + EclipseGarage, IsEclipse, G);
			if (!slot.Equals("No Slot Free"))
			{
				RemoveFromMMI_And_PersistantRides(Game.Player.Character.CurrentVehicle);
				Game.Player.CanControlCharacter = false;
				Vector3 position = Game.Player.Character.CurrentVehicle.Position;
				Game.Player.Character.CurrentVehicle.Position = G.Enterance;
				Game.Player.Character.CurrentVehicle.GetOffsetPosition(new Vector3(0f, -1f, 0f));
				Game.Player.Character.CurrentVehicle.Heading = G.EnterHeading;
				Vector3 offsetPosition = Game.Player.Character.CurrentVehicle.GetOffsetPosition(new Vector3(0f, 1f, 3f));
				Vector3 rotation = new Vector3(90f, 0f, Game.Player.Character.CurrentVehicle.Heading);
				Game.Player.Character.CurrentVehicle.Position = Game.Player.Character.CurrentVehicle.GetOffsetPosition(new Vector3(0f, -3f, 0f));
				Game.Player.Character.CurrentVehicle.IsCollisionEnabled = false;
				Game.Player.Character.CurrentVehicle.IsPositionFrozen = true;
				float fieldOfView = GameplayCamera.FieldOfView;
				if (MainCam != null)
				{
					World.RenderingCamera = MainCam;
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
					MainCam.IsActive = false;
					MainCam.Delete();
				}
				MainCam = GlobalHelper.CreateCamera(offsetPosition, rotation, fieldOfView);
				MainCam.PointAt(Game.Player.Character.Position);
				MainCam.IsActive = true;
				MainCam.FarClip = 2000f;
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
				if (SlideObject != null)
				{
					SlideObject.Delete();
				}
				SlideOffset = Game.Player.Character.CurrentVehicle.GetOffsetPosition(new Vector3(0f, 10f, 0f));
				SlideObject = GlobalHelper.CreateProp(RequestModel("prop_drop_armscrate_01b"), Game.Player.Character.CurrentVehicle.Position, false, false);
				SlideObject.Position = Game.Player.Character.CurrentVehicle.Position;
				SlideObject.Rotation = Game.Player.Character.CurrentVehicle.Rotation;
				SlidingVehicle = true;
				for (int i = 0; i < 200; i++)
				{
					if (SlidingVehicle && SlideObject != null)
					{
						Function.Call(Hash.SET_ENTITY_NO_COLLISION_ENTITY, Game.Player.Character.CurrentVehicle, SlideObject, false);
						Function.Call(Hash.SET_ENTITY_NO_COLLISION_ENTITY, SlideObject, Game.Player.Character.CurrentVehicle, false);
						SlideObject.Position = SlideObject.GetOffsetPosition(new Vector3(0f, 0.05f, 0f));
						bool flag = false;
						if (GetDistance_VDIST2(Game.Player.Character.CurrentVehicle.Position, SlideOffset, "<", 1.25f))
						{
							flag = true;
						}
						Game.Player.Character.CurrentVehicle.Position = SlideObject.Position;
						Game.Player.Character.CurrentVehicle.Rotation = SlideObject.Rotation;
						if (flag)
						{
							SlidingVehicle = false;
							if (SlideObject != null)
							{
								SlideObject.Delete();
							}
						}
					}
					Script.Wait(1);
				}
				SlidingVehicle = false;
				if (SlideObject != null)
				{
					SlideObject.Delete();
				}
				Audio.PlaySoundAt(Game.Player.Character, "GARAGE_DOOR_SCRIPTED_CLOSE");
				Script.Wait(500);
				Game.Player.Character.CurrentVehicle.IsCollisionEnabled = true;
				Game.Player.Character.CurrentVehicle.IsCollisionProof = true;
				Game.Player.Character.CurrentVehicle.IsPositionFrozen = false;
				if (MainCam != null)
				{
					World.RenderingCamera = MainCam;
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
					MainCam.IsActive = false;
					MainCam.Delete();
				}
				Function.Call(Hash.BUSYSPINNER_OFF);
				Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
				Function.Call(Hash.POP_TIMECYCLE_MODIFIER);
				Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "default");
				if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
				{
					Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
				}
				Game.Player.CanControlCharacter = true;
				string displayName = Game.Player.Character.CurrentVehicle.DisplayName;
				if (slotID != -1)
				{
					G.RemoveVehicleButtons[slotID - 1].Title = Game.Player.Character.CurrentVehicle.LocalizedName ?? "";
					G.OrderVehicleButton[slotID - 1].AltTitle = Game.Player.Character.CurrentVehicle.LocalizedName ?? "";
				}
				SC.LoadIniFile(garageVehicleSavePath + EclipseGarage + slot);
				SC.Save();
				Notification.Show("Vehicle has been Saved into Storage");
				DeleteVehicles();
				LoadVehicles(garageVehicleSavePath + EclipseGarage, "", G, GarageType);
				if (slot.Contains("Slot1"))
				{
					Game.Player.Character.SetIntoVehicle(Vehicle1, VehicleSeat.Driver);
				}
				if (slot.Contains("Slot2"))
				{
					Game.Player.Character.SetIntoVehicle(Vehicle2, VehicleSeat.Driver);
				}
				if (slot.Contains("Slot3"))
				{
					Game.Player.Character.SetIntoVehicle(Vehicle3, VehicleSeat.Driver);
				}
				if (slot.Contains("Slot4"))
				{
					Game.Player.Character.SetIntoVehicle(Vehicle4, VehicleSeat.Driver);
				}
				if (slot.Contains("Slot5"))
				{
					Game.Player.Character.SetIntoVehicle(Vehicle5, VehicleSeat.Driver);
				}
				if (slot.Contains("Slot6"))
				{
					Game.Player.Character.SetIntoVehicle(Vehicle6, VehicleSeat.Driver);
				}
				if (slot.Contains("Slot7"))
				{
					Game.Player.Character.SetIntoVehicle(Vehicle7, VehicleSeat.Driver);
				}
				if (slot.Contains("Slot8"))
				{
					Game.Player.Character.SetIntoVehicle(Vehicle8, VehicleSeat.Driver);
				}
				if (slot.Contains("Slot9"))
				{
					Game.Player.Character.SetIntoVehicle(Vehicle9, VehicleSeat.Driver);
				}
				if (slot.Contains("Slot10"))
				{
					Game.Player.Character.SetIntoVehicle(Vehicle10, VehicleSeat.Driver);
				}
				Game.Player.Character.Task.LeaveVehicle();
				SavingVehicle = false;
				InGarage = true;
			}
			else
			{
				Notification.Show("Cannot Enter Garage with Vehicle as Garage is full.");
			}
		}

		public static void DeleteVehicles()
		{
			if (Vehicle1 != null)
			{
				Vehicle1.Delete();
			}
			if (Vehicle2 != null)
			{
				Vehicle2.Delete();
			}
			if (Vehicle3 != null)
			{
				Vehicle3.Delete();
			}
			if (Vehicle4 != null)
			{
				Vehicle4.Delete();
			}
			if (Vehicle5 != null)
			{
				Vehicle5.Delete();
			}
			if (Vehicle6 != null)
			{
				Vehicle6.Delete();
			}
			if (Vehicle7 != null)
			{
				Vehicle7.Delete();
			}
			if (Vehicle8 != null)
			{
				Vehicle8.Delete();
			}
			if (Vehicle9 != null)
			{
				Vehicle9.Delete();
			}
			if (Vehicle10 != null)
			{
				Vehicle10.Delete();
			}
		}

		public static void LoadVehicles(string Path, string EclipseGarage, Class1.GTAOGarage G, InteriorGarage GarageType)
		{
			Path += EclipseGarage;
			if (G.GarageSize == 2)
			{
				SC.LoadIniFile(Path + "\\Slot1.ini");
				Model model = SC.CheckCar(Path + "\\Slot1.ini");
				if (model != "null" && model != null)
				{
					SC.LoadIniFile(Path + "//Slot1.ini");
					Vehicle1 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot1.ini"), new Vector3(GarageType.Veh0Pos.X, GarageType.Veh0Pos.Y, GarageType.Veh0Pos.Z), GarageType.Veh0Pos.W);
					SC.Load(Vehicle1);
					Vehicle1.IsDriveable = false;
				}
				SC.LoadIniFile(Path + "\\Slot2.ini");
				Model model2 = SC.CheckCar(Path + "\\Slot2.ini");
				if (model2 != "null" && model2 != null)
				{
					SC.LoadIniFile(Path + "//Slot2.ini");
					Vehicle2 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot2.ini"), new Vector3(GarageType.Veh1Pos.X, GarageType.Veh1Pos.Y, GarageType.Veh1Pos.Z), GarageType.Veh1Pos.W);
					SC.Load(Vehicle2);
					Vehicle2.IsDriveable = false;
				}
			}
			if (G.GarageSize == 6)
			{
				SC.LoadIniFile(Path + "\\Slot1.ini");
				Model model3 = SC.CheckCar(Path + "\\Slot1.ini");
				if (model3 != "null" && model3 != null)
				{
					SC.LoadIniFile(Path + "//Slot1.ini");
					Vehicle1 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot1.ini"), new Vector3(GarageType.Veh0Pos.X, GarageType.Veh0Pos.Y, GarageType.Veh0Pos.Z), GarageType.Veh0Pos.W);
					SC.Load(Vehicle1);
					Vehicle1.IsDriveable = false;
				}
				SC.LoadIniFile(Path + "\\Slot2.ini");
				Model model4 = SC.CheckCar(Path + "\\Slot2.ini");
				if (model4 != "null" && model4 != null)
				{
					SC.LoadIniFile(Path + "//Slot2.ini");
					Vehicle2 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot2.ini"), new Vector3(GarageType.Veh1Pos.X, GarageType.Veh1Pos.Y, GarageType.Veh1Pos.Z), GarageType.Veh1Pos.W);
					SC.Load(Vehicle2);
					Vehicle2.IsDriveable = false;
				}
				SC.LoadIniFile(Path + "\\Slot3.ini");
				Model model5 = SC.CheckCar(Path + "\\Slot3.ini");
				if (model5 != "null" && model5 != null)
				{
					SC.LoadIniFile(Path + "//Slot3.ini");
					Vehicle3 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot3.ini"), new Vector3(GarageType.Veh2Pos.X, GarageType.Veh2Pos.Y, GarageType.Veh2Pos.Z), GarageType.Veh2Pos.W);
					SC.Load(Vehicle3);
					Vehicle3.IsDriveable = false;
				}
				SC.LoadIniFile(Path + "\\Slot4.ini");
				Model model6 = SC.CheckCar(Path + "\\Slot4.ini");
				if (model6 != "null" && model6 != null)
				{
					SC.LoadIniFile(Path + "//Slot4.ini");
					Vehicle4 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot4.ini"), new Vector3(GarageType.Veh3Pos.X, GarageType.Veh3Pos.Y, GarageType.Veh3Pos.Z), GarageType.Veh3Pos.W);
					SC.Load(Vehicle4);
					Vehicle4.IsDriveable = false;
				}
				SC.LoadIniFile(Path + "\\Slot5.ini");
				Model model7 = SC.CheckCar(Path + "\\Slot5.ini");
				if (model7 != "null" && model7 != null)
				{
					SC.LoadIniFile(Path + "//Slot5.ini");
					Vehicle5 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot5.ini"), new Vector3(GarageType.Veh4Pos.X, GarageType.Veh4Pos.Y, GarageType.Veh4Pos.Z), GarageType.Veh4Pos.W);
					SC.Load(Vehicle5);
					Vehicle5.IsDriveable = false;
				}
				SC.LoadIniFile(Path + "\\Slot6.ini");
				Model model8 = SC.CheckCar(Path + "\\Slot6.ini");
				if (model8 != "null" && model8 != null)
				{
					SC.LoadIniFile(Path + "//Slot6.ini");
					Vehicle6 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot6.ini"), new Vector3(GarageType.Veh5Pos.X, GarageType.Veh5Pos.Y, GarageType.Veh5Pos.Z), GarageType.Veh5Pos.W);
					SC.Load(Vehicle6);
					Vehicle6.IsDriveable = false;
				}
			}
			if (G.GarageSize == 10)
			{
				SC.LoadIniFile(Path + "\\Slot1.ini");
				Model model9 = SC.CheckCar(Path + "\\Slot1.ini");
				if (model9 != "null" && model9 != null)
				{
					SC.LoadIniFile(Path + "//Slot1.ini");
					Vehicle1 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot1.ini"), new Vector3(GarageType.Veh0Pos.X, GarageType.Veh0Pos.Y, GarageType.Veh0Pos.Z), GarageType.Veh0Pos.W);
					SC.Load(Vehicle1);
					Vehicle1.IsDriveable = false;
				}
				SC.LoadIniFile(Path + "\\Slot2.ini");
				Model model10 = SC.CheckCar(Path + "\\Slot2.ini");
				if (model10 != "null" && model10 != null)
				{
					SC.LoadIniFile(Path + "//Slot2.ini");
					Vehicle2 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot2.ini"), new Vector3(GarageType.Veh1Pos.X, GarageType.Veh1Pos.Y, GarageType.Veh1Pos.Z), GarageType.Veh1Pos.W);
					SC.Load(Vehicle2);
					Vehicle2.IsDriveable = false;
				}
				SC.LoadIniFile(Path + "\\Slot3.ini");
				Model model11 = SC.CheckCar(Path + "\\Slot3.ini");
				if (model11 != "null" && model11 != null)
				{
					SC.LoadIniFile(Path + "//Slot3.ini");
					Vehicle3 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot3.ini"), new Vector3(GarageType.Veh2Pos.X, GarageType.Veh2Pos.Y, GarageType.Veh2Pos.Z), GarageType.Veh2Pos.W);
					SC.Load(Vehicle3);
					Vehicle3.IsDriveable = false;
				}
				SC.LoadIniFile(Path + "\\Slot4.ini");
				Model model12 = SC.CheckCar(Path + "\\Slot4.ini");
				if (model12 != "null" && model12 != null)
				{
					SC.LoadIniFile(Path + "//Slot4.ini");
					Vehicle4 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot4.ini"), new Vector3(GarageType.Veh3Pos.X, GarageType.Veh3Pos.Y, GarageType.Veh3Pos.Z), GarageType.Veh3Pos.W);
					SC.Load(Vehicle4);
					Vehicle4.IsDriveable = false;
				}
				SC.LoadIniFile(Path + "\\Slot5.ini");
				Model model13 = SC.CheckCar(Path + "\\Slot5.ini");
				if (model13 != "null" && model13 != null)
				{
					SC.LoadIniFile(Path + "//Slot5.ini");
					Vehicle5 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot5.ini"), new Vector3(GarageType.Veh4Pos.X, GarageType.Veh4Pos.Y, GarageType.Veh4Pos.Z), GarageType.Veh4Pos.W);
					SC.Load(Vehicle5);
					Vehicle5.IsDriveable = false;
				}
				SC.LoadIniFile(Path + "\\Slot6.ini");
				Model model14 = SC.CheckCar(Path + "\\Slot6.ini");
				if (model14 != "null" && model14 != null)
				{
					SC.LoadIniFile(Path + "//Slot6.ini");
					Vehicle6 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot6.ini"), new Vector3(GarageType.Veh5Pos.X, GarageType.Veh5Pos.Y, GarageType.Veh5Pos.Z), GarageType.Veh5Pos.W);
					SC.Load(Vehicle6);
					Vehicle6.IsDriveable = false;
				}
				SC.LoadIniFile(Path + "\\Slot7.ini");
				if (SC.CheckCar(Path + "\\Slot7.ini") != "null" && model14 != null)
				{
					SC.LoadIniFile(Path + "//Slot7.ini");
					Vehicle7 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot7.ini"), new Vector3(GarageType.Veh6Pos.X, GarageType.Veh6Pos.Y, GarageType.Veh6Pos.Z), GarageType.Veh6Pos.W);
					SC.Load(Vehicle7);
					Vehicle7.IsDriveable = false;
				}
				SC.LoadIniFile(Path + "\\Slot8.ini");
				Model model15 = SC.CheckCar(Path + "\\Slot8.ini");
				if (model15 != "null" && model15 != null)
				{
					SC.LoadIniFile(Path + "//Slot8.ini");
					Vehicle8 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot8.ini"), new Vector3(GarageType.Veh7Pos.X, GarageType.Veh7Pos.Y, GarageType.Veh7Pos.Z), GarageType.Veh7Pos.W);
					SC.Load(Vehicle8);
					Vehicle8.IsDriveable = false;
				}
				SC.LoadIniFile(Path + "\\Slot9.ini");
				Model model16 = SC.CheckCar(Path + "\\Slot9.ini");
				if (model16 != "null" && model16 != null)
				{
					SC.LoadIniFile(Path + "//Slot9.ini");
					Vehicle9 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot9.ini"), new Vector3(GarageType.Veh8Pos.X, GarageType.Veh8Pos.Y, GarageType.Veh8Pos.Z), GarageType.Veh8Pos.W);
					SC.Load(Vehicle9);
					Vehicle9.IsDriveable = false;
				}
				SC.LoadIniFile(Path + "\\Slot10.ini");
				Model model17 = SC.CheckCar(Path + "\\Slot10.ini");
				if (model17 != "null" && model17 != null)
				{
					SC.LoadIniFile(Path + "//Slot10.ini");
					Vehicle10 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot10.ini"), new Vector3(GarageType.Veh9Pos.X, GarageType.Veh9Pos.Y, GarageType.Veh9Pos.Z), GarageType.Veh9Pos.W);
					SC.Load(Vehicle10);
					Vehicle10.IsDriveable = false;
				}
			}
			if (G.GarageSize == 50)
			{
				SC.LoadIniFile(Path + "\\Slot1.ini");
				Model model18 = SC.CheckCar(Path + "\\Slot1.ini");
				if (model18 != "null" && model18 != null)
				{
					SC.LoadIniFile(Path + "//Slot1.ini");
					Vehicle1 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot1.ini"), new Vector3(GarageType.Veh0Pos.X, GarageType.Veh0Pos.Y, GarageType.Veh0Pos.Z), GarageType.Veh0Pos.W);
					SC.Load(Vehicle1);
					Vehicle1.IsDriveable = false;
				}
				SC.LoadIniFile(Path + "\\Slot2.ini");
				Model model19 = SC.CheckCar(Path + "\\Slot2.ini");
				if (model19 != "null" && model19 != null)
				{
					SC.LoadIniFile(Path + "//Slot2.ini");
					Vehicle2 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot2.ini"), new Vector3(GarageType.Veh1Pos.X, GarageType.Veh1Pos.Y, GarageType.Veh1Pos.Z), GarageType.Veh1Pos.W);
					SC.Load(Vehicle2);
					Vehicle2.IsDriveable = false;
				}
				SC.LoadIniFile(Path + "\\Slot3.ini");
				Model model20 = SC.CheckCar(Path + "\\Slot3.ini");
				if (model20 != "null" && model20 != null)
				{
					SC.LoadIniFile(Path + "//Slot3.ini");
					Vehicle3 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot3.ini"), new Vector3(GarageType.Veh2Pos.X, GarageType.Veh2Pos.Y, GarageType.Veh2Pos.Z), GarageType.Veh2Pos.W);
					SC.Load(Vehicle3);
					Vehicle3.IsDriveable = false;
				}
				SC.LoadIniFile(Path + "\\Slot4.ini");
				Model model21 = SC.CheckCar(Path + "\\Slot4.ini");
				if (model21 != "null" && model21 != null)
				{
					SC.LoadIniFile(Path + "//Slot4.ini");
					Vehicle4 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot4.ini"), new Vector3(GarageType.Veh3Pos.X, GarageType.Veh3Pos.Y, GarageType.Veh3Pos.Z), GarageType.Veh3Pos.W);
					SC.Load(Vehicle4);
					Vehicle4.IsDriveable = false;
				}
				SC.LoadIniFile(Path + "\\Slot5.ini");
				Model model22 = SC.CheckCar(Path + "\\Slot5.ini");
				if (model22 != "null" && model22 != null)
				{
					SC.LoadIniFile(Path + "//Slot5.ini");
					Vehicle5 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot5.ini"), new Vector3(GarageType.Veh4Pos.X, GarageType.Veh4Pos.Y, GarageType.Veh4Pos.Z), GarageType.Veh4Pos.W);
					SC.Load(Vehicle5);
					Vehicle5.IsDriveable = false;
				}
				SC.LoadIniFile(Path + "\\Slot6.ini");
				Model model23 = SC.CheckCar(Path + "\\Slot6.ini");
				if (model23 != "null" && model23 != null)
				{
					SC.LoadIniFile(Path + "//Slot6.ini");
					Vehicle6 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot6.ini"), new Vector3(GarageType.Veh5Pos.X, GarageType.Veh5Pos.Y, GarageType.Veh5Pos.Z), GarageType.Veh5Pos.W);
					SC.Load(Vehicle6);
					Vehicle6.IsDriveable = false;
				}
				SC.LoadIniFile(Path + "\\Slot7.ini");
				Model model24 = SC.CheckCar(Path + "\\Slot7.ini");
				if (model24 != "null" && model24 != null)
				{
					SC.LoadIniFile(Path + "//Slot7.ini");
					Vehicle7 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot7.ini"), new Vector3(GarageType.Veh6Pos.X, GarageType.Veh6Pos.Y, GarageType.Veh6Pos.Z), GarageType.Veh6Pos.W);
					SC.Load(Vehicle7);
					Vehicle7.IsDriveable = false;
				}
				SC.LoadIniFile(Path + "\\Slot8.ini");
				Model model25 = SC.CheckCar(Path + "\\Slot8.ini");
				if (model25 != "null" && model25 != null)
				{
					SC.LoadIniFile(Path + "//Slot8.ini");
					Vehicle8 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot8.ini"), new Vector3(GarageType.Veh7Pos.X, GarageType.Veh7Pos.Y, GarageType.Veh7Pos.Z), GarageType.Veh7Pos.W);
					SC.Load(Vehicle8);
					Vehicle8.IsDriveable = false;
				}
				SC.LoadIniFile(Path + "\\Slot9.ini");
				Model model26 = SC.CheckCar(Path + "\\Slot9.ini");
				if (model26 != "null" && model26 != null)
				{
					SC.LoadIniFile(Path + "//Slot9.ini");
					Vehicle9 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot9.ini"), new Vector3(GarageType.Veh8Pos.X, GarageType.Veh8Pos.Y, GarageType.Veh8Pos.Z), GarageType.Veh8Pos.W);
					SC.Load(Vehicle9);
					Vehicle9.IsDriveable = false;
				}
				SC.LoadIniFile(Path + "\\Slot10.ini");
				Model model27 = SC.CheckCar(Path + "\\Slot10.ini");
				if (model27 != "null" && model27 != null)
				{
					SC.LoadIniFile(Path + "//Slot10.ini");
					Vehicle10 = GlobalHelper.CreateVehicle(SC.CheckCar(Path + "//Slot10.ini"), new Vector3(GarageType.Veh9Pos.X, GarageType.Veh9Pos.Y, GarageType.Veh9Pos.Z), GarageType.Veh9Pos.W);
					SC.Load(Vehicle10);
					Vehicle10.IsDriveable = false;
				}
			}
		}

		public unsafe static void OpenSequenceTask(int* taskID)
		{
			Function.Call(Hash.OPEN_SEQUENCE_TASK, taskID);
		}

		public static void TaskPedSlideToCoord(Ped ped, Vector3 pos, float heading, float speed = 0.75f)
		{
			Function.Call(Hash.TASK_PED_SLIDE_TO_COORD, ped, pos.X, pos.Y, pos.Z, heading, speed);
		}

		public static void TaskPedSlideToCoord(int ped, Vector3 pos, float heading, float speed = 0.75f)
		{
			Function.Call(Hash.TASK_PED_SLIDE_TO_COORD, ped, pos.X, pos.Y, pos.Z, heading, speed);
		}

		public static void CloseSequenceTask(int taskID)
		{
			Function.Call(Hash.CLOSE_SEQUENCE_TASK, taskID);
		}

		public unsafe static void ClearSequenceTask(int* taskID)
		{
			Function.Call(Hash.CLEAR_SEQUENCE_TASK, taskID);
		}

		public static void TaskPerformSequence(Ped ped, int taskID)
		{
			Function.Call(Hash.TASK_PERFORM_SEQUENCE, ped, taskID);
		}

		public static void ManageRemoveVehicle(NativeItem Item, int ID)
		{
			Item.Activated += delegate
			{
				if (!isSlotFree(Garages[CurrentGarage].GarageVehicleSavePath + CurrentEclipseGarage, ID))
				{
					Notification.Show("~b~Remove~w~ " + Garages[CurrentGarage].GarageVehicleSavePath + CurrentEclipseGarage + "Slot" + ID + ".ini");
					SC.LoadIniFile(Garages[CurrentGarage].GarageVehicleSavePath + CurrentEclipseGarage + "Slot" + ID + ".ini");
					SC.RemoveVehicleHashName();
					if (GetVehicleFromID(ID) != null)
					{
						EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(GetVehicleFromID(ID), false, 0));
					}
					Item.Title = "Empty";
					Notification.Show("Vehicle Removed From Garage.");
				}
				else
				{
					Notification.Show("No Vehicle in Slot.");
				}
			};
		}

		public static void SaveVehicleMMI(Vehicle V, int GarageSize)
		{
			if (File.Exists("scripts//MorsMutualInsurance_HKH.dll"))
			{
				MethodInfo method = Assembly.LoadFrom("scripts/MorsMutualInsurance_HKH.dll").GetType("MorsMutualInsurance_HKH.MorsMutualInsurance").GetMethod("SaveVehicleToMMIAndRemoveFromGarage");
				object[] parameters = new object[3] { V, GarageSize, "" };
				method.Invoke(method, parameters);
			}
		}

		public static void RemoveFromMMI_And_PersistantRides(Vehicle V)
		{
			if (File.Exists("scripts//MorsMutualInsurance_HKH.dll"))
			{
				MethodInfo method = Assembly.LoadFrom("scripts/MorsMutualInsurance_HKH.dll").GetType("MorsMutualInsurance_HKH.MorsMutualInsurance").GetMethod("RemoveFromMMIAndPersistantRides");
				object[] parameters = new object[1] { V };
				method.Invoke(method, parameters);
			}
		}

		public void LoadCarinToRealWorld(Vehicle V)
		{
			SaveVehicleMMI(V, 10);
			Vehicle currentVehicle = Game.Player.Character.CurrentVehicle;
			Game.Player.Character.SetIntoVehicle(currentVehicle, VehicleSeat.Driver);
			if (Vehicle1 != null && Vehicle1.GetPedOnSeat(VehicleSeat.Driver) != null && Vehicle1.GetPedOnSeat(VehicleSeat.Driver) == Game.Player.Character)
			{
				Vehicle1 = null;
			}
			if (Vehicle2 != null && Vehicle2.GetPedOnSeat(VehicleSeat.Driver) != null && Vehicle2.GetPedOnSeat(VehicleSeat.Driver) == Game.Player.Character)
			{
				Vehicle2 = null;
			}
			if (Vehicle3 != null && Vehicle3.GetPedOnSeat(VehicleSeat.Driver) != null && Vehicle3.GetPedOnSeat(VehicleSeat.Driver) == Game.Player.Character)
			{
				Vehicle3 = null;
			}
			if (Vehicle4 != null && Vehicle4.GetPedOnSeat(VehicleSeat.Driver) != null && Vehicle4.GetPedOnSeat(VehicleSeat.Driver) == Game.Player.Character)
			{
				Vehicle4 = null;
			}
			if (Vehicle5 != null && Vehicle5.GetPedOnSeat(VehicleSeat.Driver) != null && Vehicle5.GetPedOnSeat(VehicleSeat.Driver) == Game.Player.Character)
			{
				Vehicle5 = null;
			}
			if (Vehicle6 != null && Vehicle6.GetPedOnSeat(VehicleSeat.Driver) != null && Vehicle6.GetPedOnSeat(VehicleSeat.Driver) == Game.Player.Character)
			{
				Vehicle6 = null;
			}
			if (Vehicle7 != null && Vehicle7.GetPedOnSeat(VehicleSeat.Driver) != null && Vehicle7.GetPedOnSeat(VehicleSeat.Driver) == Game.Player.Character)
			{
				Vehicle7 = null;
			}
			if (Vehicle8 != null && Vehicle8.GetPedOnSeat(VehicleSeat.Driver) != null && Vehicle8.GetPedOnSeat(VehicleSeat.Driver) == Game.Player.Character)
			{
				Vehicle8 = null;
			}
			if (Vehicle9 != null && Vehicle9.GetPedOnSeat(VehicleSeat.Driver) != null && Vehicle9.GetPedOnSeat(VehicleSeat.Driver) == Game.Player.Character)
			{
				Vehicle9 = null;
			}
			if (Vehicle10 != null && Vehicle10.GetPedOnSeat(VehicleSeat.Driver) != null && Vehicle10.GetPedOnSeat(VehicleSeat.Driver) == Game.Player.Character)
			{
				Vehicle10 = null;
			}
			currentVehicle.IsDriveable = true;
			currentVehicle = currentVehicle;
			currentVehicle.IsPersistent = false;
			currentVehicle.Repair();
			GTA.UI.Screen.FadeOut(500);
			Script.Wait(2500);
			LastGarage = -1;
			DeleteVehicles();
			InGarage = false;
			currentVehicle.Position = Garages[CurrentGarage].VehicleSpawn;
			currentVehicle.Rotation = new Vector3(0f, 0f, Garages[CurrentGarage].EnterHeading - 180f);
			JustExitedWithVehicle = true;
			Script.Wait(500);
			GTA.UI.Screen.FadeIn(500);
		}

		public static void LoadGarage()
		{
			LastGarage = CurrentGarage;
			if (Garages[CurrentGarage].GarageSize == 2)
			{
				GarageTypeIn = GarageType.Two;
				CurrentGarageInterior = TwoCarGarage;
				if (CurrentGarageInterior != null)
				{
					LoadVehicles(Garages[CurrentGarage].GarageVehicleSavePath, "", Garages[CurrentGarage], CurrentGarageInterior);
				}
			}
			if (Garages[CurrentGarage].GarageSize == 6)
			{
				GarageTypeIn = GarageType.Six;
				CurrentGarageInterior = SixCarGarage;
				if (CurrentGarageInterior != null)
				{
					LoadVehicles(Garages[CurrentGarage].GarageVehicleSavePath, "", Garages[CurrentGarage], CurrentGarageInterior);
				}
			}
			if (Garages[CurrentGarage].GarageSize == 10)
			{
				GarageTypeIn = GarageType.Ten;
				CurrentGarageInterior = TenCarGarage;
				if (CurrentGarageInterior != null)
				{
					LoadVehicles(Garages[CurrentGarage].GarageVehicleSavePath, "", Garages[CurrentGarage], CurrentGarageInterior);
				}
			}
		}

		public unsafe static void SetupGTAOGarages()
		{
			for (int i = 0; i < Garages.Count; i++)
			{
				Class1.GTAOGarage G = Garages[i];
				if (G.OrderVehicleButton == null)
				{
					G.OrderVehicleButton = new List<NativeItem>();
				}
				if (!G.MultiGarage)
				{
					NativeMenu nativeMenu = new NativeMenu(G.GarageName, G.GarageName);
					MechanicMenu.Add(nativeMenu);
					GLemonUIMenusNativeMenus.Add(nativeMenu);
					MechanicObjectPool.Add(nativeMenu);
					if (G.GarageSize == 2)
					{
						NativeItem nativeItem = new NativeItem("Slot 1", "", "EMPTY");
						nativeMenu.Add(nativeItem);
						G.OrderVehicleButton.Add(nativeItem);
						GetSlotContents(G, nativeItem, 1, "");
						nativeItem.Activated += delegate
						{
							OrderVehicleFromSlot(G, 1, "");
						};
						NativeItem nativeItem2 = new NativeItem("Slot 2", "", "EMPTY");
						nativeMenu.Add(nativeItem2);
						G.OrderVehicleButton.Add(nativeItem2);
						GetSlotContents(G, nativeItem2, 2, "");
						nativeItem2.Activated += delegate
						{
							OrderVehicleFromSlot(G, 2, "");
						};
					}
					if (G.GarageSize == 6)
					{
						NativeItem nativeItem3 = new NativeItem("Slot 1", "", "EMPTY");
						nativeMenu.Add(nativeItem3);
						G.OrderVehicleButton.Add(nativeItem3);
						GetSlotContents(G, nativeItem3, 1, "");
						nativeItem3.Activated += delegate
						{
							OrderVehicleFromSlot(G, 1, "");
						};
						NativeItem nativeItem4 = new NativeItem("Slot 2", "", "EMPTY");
						nativeMenu.Add(nativeItem4);
						G.OrderVehicleButton.Add(nativeItem4);
						GetSlotContents(G, nativeItem4, 2, "");
						nativeItem4.Activated += delegate
						{
							OrderVehicleFromSlot(G, 2, "");
						};
						NativeItem nativeItem5 = new NativeItem("Slot 3", "", "EMPTY");
						nativeMenu.Add(nativeItem5);
						G.OrderVehicleButton.Add(nativeItem5);
						GetSlotContents(G, nativeItem5, 3, "");
						nativeItem5.Activated += delegate
						{
							OrderVehicleFromSlot(G, 3, "");
						};
						NativeItem nativeItem6 = new NativeItem("Slot 4", "", "EMPTY");
						nativeMenu.Add(nativeItem6);
						G.OrderVehicleButton.Add(nativeItem6);
						GetSlotContents(G, nativeItem6, 4, "");
						nativeItem6.Activated += delegate
						{
							OrderVehicleFromSlot(G, 4, "");
						};
						NativeItem nativeItem7 = new NativeItem("Slot 5", "", "EMPTY");
						nativeMenu.Add(nativeItem7);
						G.OrderVehicleButton.Add(nativeItem7);
						GetSlotContents(G, nativeItem7, 5, "");
						nativeItem7.Activated += delegate
						{
							OrderVehicleFromSlot(G, 5, "");
						};
						NativeItem nativeItem8 = new NativeItem("Slot 6", "", "EMPTY");
						nativeMenu.Add(nativeItem8);
						G.OrderVehicleButton.Add(nativeItem8);
						GetSlotContents(G, nativeItem8, 6, "");
						nativeItem8.Activated += delegate
						{
							OrderVehicleFromSlot(G, 6, "");
						};
					}
					if (G.GarageSize == 10)
					{
						NativeItem nativeItem9 = new NativeItem("Slot 1", "", "EMPTY");
						nativeMenu.Add(nativeItem9);
						G.OrderVehicleButton.Add(nativeItem9);
						GetSlotContents(G, nativeItem9, 1, "");
						nativeItem9.Activated += delegate
						{
							OrderVehicleFromSlot(G, 1, "");
						};
						NativeItem nativeItem10 = new NativeItem("Slot 2", "", "EMPTY");
						nativeMenu.Add(nativeItem10);
						G.OrderVehicleButton.Add(nativeItem10);
						GetSlotContents(G, nativeItem10, 2, "");
						nativeItem10.Activated += delegate
						{
							OrderVehicleFromSlot(G, 2, "");
						};
						NativeItem nativeItem11 = new NativeItem("Slot 3", "", "EMPTY");
						nativeMenu.Add(nativeItem11);
						G.OrderVehicleButton.Add(nativeItem11);
						GetSlotContents(G, nativeItem11, 3, "");
						nativeItem11.Activated += delegate
						{
							OrderVehicleFromSlot(G, 3, "");
						};
						NativeItem nativeItem12 = new NativeItem("Slot 4", "", "EMPTY");
						nativeMenu.Add(nativeItem12);
						G.OrderVehicleButton.Add(nativeItem12);
						GetSlotContents(G, nativeItem12, 4, "");
						nativeItem12.Activated += delegate
						{
							OrderVehicleFromSlot(G, 4, "");
						};
						NativeItem nativeItem13 = new NativeItem("Slot 5", "", "EMPTY");
						nativeMenu.Add(nativeItem13);
						G.OrderVehicleButton.Add(nativeItem13);
						GetSlotContents(G, nativeItem13, 5, "");
						nativeItem13.Activated += delegate
						{
							OrderVehicleFromSlot(G, 5, "");
						};
						NativeItem nativeItem14 = new NativeItem("Slot 6", "", "EMPTY");
						nativeMenu.Add(nativeItem14);
						G.OrderVehicleButton.Add(nativeItem14);
						GetSlotContents(G, nativeItem14, 6, "");
						nativeItem14.Activated += delegate
						{
							OrderVehicleFromSlot(G, 6, "");
						};
						NativeItem nativeItem15 = new NativeItem("Slot 7", "", "EMPTY");
						nativeMenu.Add(nativeItem15);
						G.OrderVehicleButton.Add(nativeItem15);
						GetSlotContents(G, nativeItem15, 7, "");
						nativeItem15.Activated += delegate
						{
							OrderVehicleFromSlot(G, 7, "");
						};
						NativeItem nativeItem16 = new NativeItem("Slot 8", "", "EMPTY");
						nativeMenu.Add(nativeItem16);
						G.OrderVehicleButton.Add(nativeItem16);
						GetSlotContents(G, nativeItem16, 8, "");
						nativeItem16.Activated += delegate
						{
							OrderVehicleFromSlot(G, 8, "");
						};
						NativeItem nativeItem17 = new NativeItem("Slot 9", "", "EMPTY");
						nativeMenu.Add(nativeItem17);
						G.OrderVehicleButton.Add(nativeItem17);
						GetSlotContents(G, nativeItem17, 9, "");
						nativeItem17.Activated += delegate
						{
							OrderVehicleFromSlot(G, 9, "");
						};
						NativeItem nativeItem18 = new NativeItem("Slot 10", "", "EMPTY");
						nativeMenu.Add(nativeItem18);
						G.OrderVehicleButton.Add(nativeItem18);
						GetSlotContents(G, nativeItem18, 10, "");
						nativeItem18.Activated += delegate
						{
							OrderVehicleFromSlot(G, 10, "");
						};
					}
				}
				else
				{
					NativeMenu nativeMenu2 = new NativeMenu(G.GarageName, G.GarageName);
					MechanicMenu.Add(nativeMenu2);
					GLemonUIMenusNativeMenus.Add(nativeMenu2);
					MechanicObjectPool.Add(nativeMenu2);
					NativeMenu nativeMenu3 = new NativeMenu("Basement B1", "Basement B1");
					nativeMenu2.Add(nativeMenu3);
					GLemonUIMenusNativeMenus.Add(nativeMenu3);
					MechanicObjectPool.Add(nativeMenu3);
					NativeMenu nativeMenu4 = new NativeMenu("Basement B2", "Basement B2");
					nativeMenu2.Add(nativeMenu4);
					GLemonUIMenusNativeMenus.Add(nativeMenu4);
					MechanicObjectPool.Add(nativeMenu4);
					NativeMenu nativeMenu5 = new NativeMenu("Basement B3", "Basement B3");
					nativeMenu2.Add(nativeMenu5);
					GLemonUIMenusNativeMenus.Add(nativeMenu5);
					MechanicObjectPool.Add(nativeMenu5);
					NativeMenu nativeMenu6 = new NativeMenu("Basement B4", "Basement B4");
					nativeMenu2.Add(nativeMenu6);
					GLemonUIMenusNativeMenus.Add(nativeMenu6);
					MechanicObjectPool.Add(nativeMenu6);
					NativeMenu nativeMenu7 = new NativeMenu("Basement B5", "Basement B5");
					nativeMenu2.Add(nativeMenu7);
					GLemonUIMenusNativeMenus.Add(nativeMenu7);
					MechanicObjectPool.Add(nativeMenu7);
					for (int j = 1; j < 6; j++)
					{
						int ID = 0;
						NativeMenu nativeMenu8 = null;
						if (j == 1)
						{
							ID = 1;
							nativeMenu8 = nativeMenu3;
						}
						if (j == 2)
						{
							ID = 2;
							nativeMenu8 = nativeMenu4;
						}
						if (j == 3)
						{
							ID = 3;
							nativeMenu8 = nativeMenu5;
						}
						if (j == 4)
						{
							ID = 4;
							nativeMenu8 = nativeMenu6;
						}
						if (j == 5)
						{
							ID = 5;
							nativeMenu8 = nativeMenu7;
						}
						NativeItem nativeItem19 = new NativeItem("Slot 1", "", "EMPTY");
						nativeMenu8.Add(nativeItem19);
						G.OrderVehicleButton.Add(nativeItem19);
						GetSlotContents(G, nativeItem19, 1, "Garage_" + ID + "\\");
						nativeItem19.Activated += delegate
						{
							OrderVehicleFromSlot(G, 1, "Garage_" + ID + "\\");
						};
						NativeItem nativeItem20 = new NativeItem("Slot 2", "", "EMPTY");
						nativeMenu8.Add(nativeItem20);
						G.OrderVehicleButton.Add(nativeItem20);
						GetSlotContents(G, nativeItem20, 2, "Garage_" + ID + "\\");
						nativeItem20.Activated += delegate
						{
							OrderVehicleFromSlot(G, 2, "Garage_" + ID + "\\");
						};
						NativeItem nativeItem21 = new NativeItem("Slot 3", "", "EMPTY");
						nativeMenu8.Add(nativeItem21);
						G.OrderVehicleButton.Add(nativeItem21);
						GetSlotContents(G, nativeItem21, 3, "Garage_" + ID + "\\");
						nativeItem21.Activated += delegate
						{
							OrderVehicleFromSlot(G, 3, "Garage_" + ID + "\\");
						};
						NativeItem nativeItem22 = new NativeItem("Slot 4", "", "EMPTY");
						nativeMenu8.Add(nativeItem22);
						G.OrderVehicleButton.Add(nativeItem22);
						GetSlotContents(G, nativeItem22, 4, "Garage_" + ID + "\\");
						nativeItem22.Activated += delegate
						{
							OrderVehicleFromSlot(G, 4, "Garage_" + ID + "\\");
						};
						NativeItem nativeItem23 = new NativeItem("Slot 5", "", "EMPTY");
						nativeMenu8.Add(nativeItem23);
						G.OrderVehicleButton.Add(nativeItem23);
						GetSlotContents(G, nativeItem23, 5, "Garage_" + ID + "\\");
						nativeItem23.Activated += delegate
						{
							OrderVehicleFromSlot(G, 5, "Garage_" + ID + "\\");
						};
						NativeItem nativeItem24 = new NativeItem("Slot 6", "", "EMPTY");
						nativeMenu8.Add(nativeItem24);
						G.OrderVehicleButton.Add(nativeItem24);
						GetSlotContents(G, nativeItem24, 6, "Garage_" + ID + "\\");
						nativeItem24.Activated += delegate
						{
							OrderVehicleFromSlot(G, 6, "Garage_" + ID + "\\");
						};
						NativeItem nativeItem25 = new NativeItem("Slot 7", "", "EMPTY");
						nativeMenu8.Add(nativeItem25);
						G.OrderVehicleButton.Add(nativeItem25);
						GetSlotContents(G, nativeItem25, 7, "Garage_" + ID + "\\");
						nativeItem25.Activated += delegate
						{
							OrderVehicleFromSlot(G, 7, "Garage_" + ID + "\\");
						};
						NativeItem nativeItem26 = new NativeItem("Slot 8", "", "EMPTY");
						nativeMenu8.Add(nativeItem26);
						G.OrderVehicleButton.Add(nativeItem26);
						GetSlotContents(G, nativeItem26, 8, "Garage_" + ID + "\\");
						nativeItem26.Activated += delegate
						{
							OrderVehicleFromSlot(G, 8, "Garage_" + ID + "\\");
						};
						NativeItem nativeItem27 = new NativeItem("Slot 9", "", "EMPTY");
						nativeMenu8.Add(nativeItem27);
						G.OrderVehicleButton.Add(nativeItem27);
						GetSlotContents(G, nativeItem27, 9, "Garage_" + ID + "\\");
						nativeItem27.Activated += delegate
						{
							OrderVehicleFromSlot(G, 9, "Garage_" + ID + "\\");
						};
						NativeItem nativeItem28 = new NativeItem("Slot 10", "", "EMPTY");
						nativeMenu8.Add(nativeItem28);
						G.OrderVehicleButton.Add(nativeItem28);
						GetSlotContents(G, nativeItem28, 10, "Garage_" + ID + "\\");
						nativeItem28.Activated += delegate
						{
							OrderVehicleFromSlot(G, 10, "Garage_" + ID + "\\");
						};
					}
				}
				G.RemoveMenu = new NativeMenu(" ", "Remove A Vehicle");
				mainMenu.AddSubMenu(G.RemoveMenu);
				GLemonUIMenusNativeMenus.Add(G.RemoveMenu);
				modLemonUIObjectPool.Add(G.RemoveMenu);
				G.InteriorMenu = new NativeMenu(" ", "Manage Interior");
				mainMenu.AddSubMenu(G.InteriorMenu);
				GLemonUIMenusNativeMenus.Add(G.InteriorMenu);
				modLemonUIObjectPool.Add(G.InteriorMenu);
				if (G.MultiGarage)
				{
					G.EclipseMenu = new NativeMenu(" ", "ECLIPSE BLVD GARAGE");
					mainMenu.AddSubMenu(G.EclipseMenu);
					GLemonUIMenusNativeMenus.Add(G.EclipseMenu);
					modLemonUIObjectPool.Add(G.EclipseMenu);
					NativeItem nativeItem29 = new NativeItem("Basement Level 1");
					G.EclipseMenu.Add(nativeItem29);
					nativeItem29.Description = "Go to Garage Level B1";
					nativeItem29.Activated += delegate
					{
						if (Game.Player.Character.CurrentVehicle != null)
						{
							GarageTypeIn = GarageType.Fifty;
							CurrentGarageInterior = FiftyCarGarage;
							if (CurrentGarageInterior != null)
							{
								SaveVehicleAdv(true, Garages[CurrentGarage], "Garage_1\\", CurrentGarageInterior);
							}
						}
						else
						{
							int taskID5 = 0;
							OpenSequenceTask(&taskID5);
							TaskPedSlideToCoord(0, Garages[CurrentGarage].Enterance, Garages[CurrentGarage].EnterHeading, 8f);
							CloseSequenceTask(taskID5);
							TaskPerformSequence(Game.Player.Character, taskID5);
							ClearSequenceTask(&taskID5);
							Game.Player.Character.Position = Garages[CurrentGarage].Enterance;
							Game.Player.Character.Heading = Garages[CurrentGarage].EnterHeading;
							RaycastResult raycastResult5 = World.Raycast(Game.Player.Character.Position, Game.Player.Character.GetOffsetPosition(new Vector3(0f, 3f, 0f)), IntersectFlags.Map);
							if (raycastResult5.DidHit)
							{
								Game.Player.Character.Position = raycastResult5.HitPosition;
								Game.Player.Character.Heading = Garages[CurrentGarage].EnterHeading;
								Game.Player.Character.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, -1.15f, -1.45f));
							}
							Game.Player.Character.Task.PlayAnimation("anim@apt_trans@garage", "gar_open_1_left", 1f, -1, AnimationFlags.None);
							Script.Wait(1500);
							Audio.PlaySoundAt(Game.Player.Character, "GARAGE_DOOR_SCRIPTED_CLOSE");
							GTA.UI.Screen.FadeOut(500);
							Script.Wait(2500);
							LastGarage = CurrentGarage;
							GarageTypeIn = GarageType.Fifty;
							Game.Player.Character.Position = FiftyCarGarage.GarageDoorL;
							CurrentGarageInterior = FiftyCarGarage;
							if (CurrentGarageInterior != null)
							{
								LoadVehicles(Garages[CurrentGarage].GarageVehicleSavePath, "Garage_1\\", Garages[CurrentGarage], CurrentGarageInterior);
							}
							InGarage = true;
							Script.Wait(500);
							GTA.UI.Screen.FadeIn(500);
						}
						for (int num = 0; num < 10; num++)
						{
							int iD5 = num + 1;
							SC.LoadIniFile(G.GarageVehicleSavePath + "Garage_1\\Slot" + iD5 + ".ini");
							Model model5 = SC.CheckCar(G.GarageVehicleSavePath + "Garage_1\\Slot" + iD5 + ".ini");
							if (model5 == "null" || model5 == null)
							{
								G.RemoveVehicleButtons[num].Title = "Empty";
								G.OrderVehicleButton[num].Title = G.RemoveVehicleButtons[num].Title;
								ManageRemoveVehicle(G.RemoveVehicleButtons[num], iD5);
							}
							else
							{
								G.RemoveVehicleButtons[num].Title = SC.VehicleName;
								G.OrderVehicleButton[num].Title = G.RemoveVehicleButtons[num].Title;
								ManageRemoveVehicle(G.RemoveVehicleButtons[num], iD5);
							}
						}
						if (MainCam != null)
						{
							World.RenderingCamera = MainCam;
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
							MainCam.IsActive = false;
							MainCam.Delete();
						}
						CurrentGarageInterior = FiftyCarGarage;
						RequestMultistoryGarageFloor(1, FloorB1_MultistoreyGarage_Style, FloorB1_MultistoreyGarage_Color);
						modLemonUIObjectPool.HideAll();
						GarageCamActive = false;
						InGarage = true;
						LastGarage = 27;
						CurrentEclipseGarage = "Garage_1\\";
					};
					NativeItem nativeItem30 = new NativeItem("Basement Level 2");
					G.EclipseMenu.Add(nativeItem30);
					nativeItem30.Description = "Go to Garage Level B2";
					nativeItem30.Activated += delegate
					{
						if (Game.Player.Character.CurrentVehicle != null)
						{
							GarageTypeIn = GarageType.Fifty;
							CurrentGarageInterior = FiftyCarGarage;
							if (CurrentGarageInterior != null)
							{
								SaveVehicleAdv(true, Garages[CurrentGarage], "Garage_2\\", CurrentGarageInterior);
							}
						}
						else
						{
							int taskID4 = 0;
							OpenSequenceTask(&taskID4);
							TaskPedSlideToCoord(0, Garages[CurrentGarage].Enterance, Garages[CurrentGarage].EnterHeading, 8f);
							CloseSequenceTask(taskID4);
							TaskPerformSequence(Game.Player.Character, taskID4);
							ClearSequenceTask(&taskID4);
							Game.Player.Character.Position = Garages[CurrentGarage].Enterance;
							Game.Player.Character.Heading = Garages[CurrentGarage].EnterHeading;
							RaycastResult raycastResult4 = World.Raycast(Game.Player.Character.Position, Game.Player.Character.GetOffsetPosition(new Vector3(0f, 3f, 0f)), IntersectFlags.Map);
							if (raycastResult4.DidHit)
							{
								Game.Player.Character.Position = raycastResult4.HitPosition;
								Game.Player.Character.Heading = Garages[CurrentGarage].EnterHeading;
								Game.Player.Character.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, -1.15f, -1.45f));
							}
							Game.Player.Character.Task.PlayAnimation("anim@apt_trans@garage", "gar_open_1_left", 1f, -1, AnimationFlags.None);
							Script.Wait(1500);
							Audio.PlaySoundAt(Game.Player.Character, "GARAGE_DOOR_SCRIPTED_CLOSE");
							GTA.UI.Screen.FadeOut(500);
							Script.Wait(2500);
							LastGarage = CurrentGarage;
							GarageTypeIn = GarageType.Fifty;
							Game.Player.Character.Position = FiftyCarGarage.GarageDoorL;
							CurrentGarageInterior = FiftyCarGarage;
							if (CurrentGarageInterior != null)
							{
								LoadVehicles(Garages[CurrentGarage].GarageVehicleSavePath, "Garage_2\\", Garages[CurrentGarage], CurrentGarageInterior);
							}
							InGarage = true;
							Script.Wait(500);
							GTA.UI.Screen.FadeIn(500);
						}
						for (int n = 0; n < 10; n++)
						{
							int iD4 = n + 1;
							SC.LoadIniFile(G.GarageVehicleSavePath + "Garage_2\\Slot" + iD4 + ".ini");
							Model model4 = SC.CheckCar(G.GarageVehicleSavePath + "Garage_2\\Slot" + iD4 + ".ini");
							if (model4 == "null" || model4 == null)
							{
								G.RemoveVehicleButtons[n].Title = "Empty";
								G.OrderVehicleButton[n].Title = G.RemoveVehicleButtons[n].Title;
								ManageRemoveVehicle(G.RemoveVehicleButtons[n], iD4);
							}
							else
							{
								G.RemoveVehicleButtons[n].Title = SC.VehicleName;
								G.OrderVehicleButton[n].Title = G.RemoveVehicleButtons[n].Title;
								ManageRemoveVehicle(G.RemoveVehicleButtons[n], iD4);
							}
						}
						if (MainCam != null)
						{
							World.RenderingCamera = MainCam;
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
							MainCam.IsActive = false;
							MainCam.Delete();
						}
						CurrentGarageInterior = FiftyCarGarage;
						RequestMultistoryGarageFloor(2, FloorB2_MultistoreyGarage_Style, FloorB2_MultistoreyGarage_Color);
						modLemonUIObjectPool.HideAll();
						GarageCamActive = false;
						InGarage = true;
						LastGarage = 27;
						CurrentEclipseGarage = "Garage_2\\";
					};
					NativeItem nativeItem31 = new NativeItem("Basement Level 3");
					G.EclipseMenu.Add(nativeItem31);
					nativeItem31.Description = "Go to Garage Level B3";
					nativeItem31.Activated += delegate
					{
						if (Game.Player.Character.CurrentVehicle != null)
						{
							GarageTypeIn = GarageType.Fifty;
							CurrentGarageInterior = FiftyCarGarage;
							if (CurrentGarageInterior != null)
							{
								SaveVehicleAdv(true, Garages[CurrentGarage], "Garage_3\\", CurrentGarageInterior);
							}
						}
						else
						{
							int taskID3 = 0;
							OpenSequenceTask(&taskID3);
							TaskPedSlideToCoord(0, Garages[CurrentGarage].Enterance, Garages[CurrentGarage].EnterHeading, 8f);
							CloseSequenceTask(taskID3);
							TaskPerformSequence(Game.Player.Character, taskID3);
							ClearSequenceTask(&taskID3);
							Game.Player.Character.Position = Garages[CurrentGarage].Enterance;
							Game.Player.Character.Heading = Garages[CurrentGarage].EnterHeading;
							RaycastResult raycastResult3 = World.Raycast(Game.Player.Character.Position, Game.Player.Character.GetOffsetPosition(new Vector3(0f, 3f, 0f)), IntersectFlags.Map);
							if (raycastResult3.DidHit)
							{
								Game.Player.Character.Position = raycastResult3.HitPosition;
								Game.Player.Character.Heading = Garages[CurrentGarage].EnterHeading;
								Game.Player.Character.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, -1.15f, -1.45f));
							}
							Game.Player.Character.Task.PlayAnimation("anim@apt_trans@garage", "gar_open_1_left", 1f, -1, AnimationFlags.None);
							Script.Wait(1500);
							Audio.PlaySoundAt(Game.Player.Character, "GARAGE_DOOR_SCRIPTED_CLOSE");
							GTA.UI.Screen.FadeOut(500);
							Script.Wait(2500);
							LastGarage = CurrentGarage;
							GarageTypeIn = GarageType.Fifty;
							Game.Player.Character.Position = FiftyCarGarage.GarageDoorL;
							CurrentGarageInterior = FiftyCarGarage;
							if (CurrentGarageInterior != null)
							{
								LoadVehicles(Garages[CurrentGarage].GarageVehicleSavePath, "Garage_3\\", Garages[CurrentGarage], CurrentGarageInterior);
							}
							InGarage = true;
							Script.Wait(500);
							GTA.UI.Screen.FadeIn(500);
						}
						for (int m = 0; m < 10; m++)
						{
							int iD3 = m + 1;
							SC.LoadIniFile(G.GarageVehicleSavePath + "Garage_3\\Slot" + iD3 + ".ini");
							Model model3 = SC.CheckCar(G.GarageVehicleSavePath + "Garage_3\\Slot" + iD3 + ".ini");
							if (model3 == "null" || model3 == null)
							{
								G.RemoveVehicleButtons[m].Title = "Empty";
								G.OrderVehicleButton[m].Title = G.RemoveVehicleButtons[m].Title;
								ManageRemoveVehicle(G.RemoveVehicleButtons[m], iD3);
							}
							else
							{
								G.RemoveVehicleButtons[m].Title = SC.VehicleName;
								G.OrderVehicleButton[m].Title = G.RemoveVehicleButtons[m].Title;
								ManageRemoveVehicle(G.RemoveVehicleButtons[m], iD3);
							}
						}
						if (MainCam != null)
						{
							World.RenderingCamera = MainCam;
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
							MainCam.IsActive = false;
							MainCam.Delete();
						}
						CurrentGarageInterior = FiftyCarGarage;
						RequestMultistoryGarageFloor(3, FloorB3_MultistoreyGarage_Style, FloorB3_MultistoreyGarage_Color);
						modLemonUIObjectPool.HideAll();
						GarageCamActive = false;
						InGarage = true;
						LastGarage = 27;
						CurrentEclipseGarage = "Garage_3\\";
					};
					NativeItem nativeItem32 = new NativeItem("Basement Level 4");
					G.EclipseMenu.Add(nativeItem32);
					nativeItem32.Description = "Go to Garage Level B4";
					nativeItem32.Activated += delegate
					{
						if (Game.Player.Character.CurrentVehicle != null)
						{
							GarageTypeIn = GarageType.Fifty;
							CurrentGarageInterior = FiftyCarGarage;
							if (CurrentGarageInterior != null)
							{
								SaveVehicleAdv(true, Garages[CurrentGarage], "Garage_4\\", CurrentGarageInterior);
							}
						}
						else
						{
							int taskID2 = 0;
							OpenSequenceTask(&taskID2);
							TaskPedSlideToCoord(0, Garages[CurrentGarage].Enterance, Garages[CurrentGarage].EnterHeading, 8f);
							CloseSequenceTask(taskID2);
							TaskPerformSequence(Game.Player.Character, taskID2);
							ClearSequenceTask(&taskID2);
							Game.Player.Character.Position = Garages[CurrentGarage].Enterance;
							Game.Player.Character.Heading = Garages[CurrentGarage].EnterHeading;
							RaycastResult raycastResult2 = World.Raycast(Game.Player.Character.Position, Game.Player.Character.GetOffsetPosition(new Vector3(0f, 3f, 0f)), IntersectFlags.Map);
							if (raycastResult2.DidHit)
							{
								Game.Player.Character.Position = raycastResult2.HitPosition;
								Game.Player.Character.Heading = Garages[CurrentGarage].EnterHeading;
								Game.Player.Character.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, -1.15f, -1.45f));
							}
							Game.Player.Character.Task.PlayAnimation("anim@apt_trans@garage", "gar_open_1_left", 1f, -1, AnimationFlags.None);
							Script.Wait(1500);
							Audio.PlaySoundAt(Game.Player.Character, "GARAGE_DOOR_SCRIPTED_CLOSE");
							GTA.UI.Screen.FadeOut(500);
							Script.Wait(2500);
							LastGarage = CurrentGarage;
							GarageTypeIn = GarageType.Fifty;
							Game.Player.Character.Position = FiftyCarGarage.GarageDoorL;
							CurrentGarageInterior = FiftyCarGarage;
							if (CurrentGarageInterior != null)
							{
								LoadVehicles(Garages[CurrentGarage].GarageVehicleSavePath, "Garage_4\\", Garages[CurrentGarage], CurrentGarageInterior);
							}
							InGarage = true;
							Script.Wait(500);
							GTA.UI.Screen.FadeIn(500);
						}
						for (int l = 0; l < 10; l++)
						{
							int iD2 = l + 1;
							SC.LoadIniFile(G.GarageVehicleSavePath + "Garage_4\\Slot" + iD2 + ".ini");
							Model model2 = SC.CheckCar(G.GarageVehicleSavePath + "Garage_4\\Slot" + iD2 + ".ini");
							if (model2 == "null" || model2 == null)
							{
								G.RemoveVehicleButtons[l].Title = "Empty";
								G.OrderVehicleButton[l].Title = G.RemoveVehicleButtons[l].Title;
								ManageRemoveVehicle(G.RemoveVehicleButtons[l], iD2);
							}
							else
							{
								G.RemoveVehicleButtons[l].Title = SC.VehicleName;
								G.OrderVehicleButton[l].Title = G.RemoveVehicleButtons[l].Title;
								ManageRemoveVehicle(G.RemoveVehicleButtons[l], iD2);
							}
						}
						if (MainCam != null)
						{
							World.RenderingCamera = MainCam;
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
							MainCam.IsActive = false;
							MainCam.Delete();
						}
						CurrentGarageInterior = FiftyCarGarage;
						RequestMultistoryGarageFloor(4, FloorB4_MultistoreyGarage_Style, FloorB4_MultistoreyGarage_Color);
						modLemonUIObjectPool.HideAll();
						GarageCamActive = false;
						InGarage = true;
						LastGarage = 27;
						CurrentEclipseGarage = "Garage_4\\";
					};
					NativeItem nativeItem33 = new NativeItem("Basement Level 5");
					G.EclipseMenu.Add(nativeItem33);
					nativeItem33.Description = "Go to Garage Level B5";
					nativeItem33.Activated += delegate
					{
						if (Game.Player.Character.CurrentVehicle != null)
						{
							GarageTypeIn = GarageType.Fifty;
							CurrentGarageInterior = FiftyCarGarage;
							if (CurrentGarageInterior != null)
							{
								SaveVehicleAdv(true, Garages[CurrentGarage], "Garage_5\\", CurrentGarageInterior);
							}
						}
						else
						{
							int taskID = 0;
							OpenSequenceTask(&taskID);
							TaskPedSlideToCoord(0, Garages[CurrentGarage].Enterance, Garages[CurrentGarage].EnterHeading, 8f);
							CloseSequenceTask(taskID);
							TaskPerformSequence(Game.Player.Character, taskID);
							ClearSequenceTask(&taskID);
							Game.Player.Character.Position = Garages[CurrentGarage].Enterance;
							Game.Player.Character.Heading = Garages[CurrentGarage].EnterHeading;
							RaycastResult raycastResult = World.Raycast(Game.Player.Character.Position, Game.Player.Character.GetOffsetPosition(new Vector3(0f, 3f, 0f)), IntersectFlags.Map);
							if (raycastResult.DidHit)
							{
								Game.Player.Character.Position = raycastResult.HitPosition;
								Game.Player.Character.Heading = Garages[CurrentGarage].EnterHeading;
								Game.Player.Character.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, -1.15f, -1.45f));
							}
							Game.Player.Character.Task.PlayAnimation("anim@apt_trans@garage", "gar_open_1_left", 1f, -1, AnimationFlags.None);
							Script.Wait(1500);
							Audio.PlaySoundAt(Game.Player.Character, "GARAGE_DOOR_SCRIPTED_CLOSE");
							GTA.UI.Screen.FadeOut(500);
							Script.Wait(2500);
							LastGarage = CurrentGarage;
							GarageTypeIn = GarageType.Fifty;
							Game.Player.Character.Position = FiftyCarGarage.GarageDoorL;
							CurrentGarageInterior = FiftyCarGarage;
							if (CurrentGarageInterior != null)
							{
								LoadVehicles(Garages[CurrentGarage].GarageVehicleSavePath, "Garage_5\\", Garages[CurrentGarage], CurrentGarageInterior);
							}
							InGarage = true;
							Script.Wait(500);
							GTA.UI.Screen.FadeIn(500);
						}
						for (int k = 0; k < 10; k++)
						{
							int iD = k + 1;
							SC.LoadIniFile(G.GarageVehicleSavePath + "Garage_5\\Slot" + iD + ".ini");
							Model model = SC.CheckCar(G.GarageVehicleSavePath + "Garage_5\\Slot" + iD + ".ini");
							if (model == "null" || model == null)
							{
								G.RemoveVehicleButtons[k].Title = "Empty";
								G.OrderVehicleButton[k].Title = G.RemoveVehicleButtons[k].Title;
								ManageRemoveVehicle(G.RemoveVehicleButtons[k], iD);
							}
							else
							{
								G.RemoveVehicleButtons[k].Title = SC.VehicleName;
								G.OrderVehicleButton[k].Title = G.RemoveVehicleButtons[k].Title;
								ManageRemoveVehicle(G.RemoveVehicleButtons[k], iD);
							}
						}
						if (MainCam != null)
						{
							World.RenderingCamera = MainCam;
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
							MainCam.IsActive = false;
							MainCam.Delete();
						}
						CurrentGarageInterior = FiftyCarGarage;
						RequestMultistoryGarageFloor(5, FloorB5_MultistoreyGarage_Style, FloorB5_MultistoreyGarage_Color);
						modLemonUIObjectPool.HideAll();
						GarageCamActive = false;
						InGarage = true;
						LastGarage = 27;
						CurrentEclipseGarage = "Garage_5\\";
					};
				}
				G.GarageBlip = GlobalHelper.CreateBlip(G.Enterance);
				if (G.Owned == 0)
				{
					G.GarageBlip.Sprite = BlipSprite.GarageForSale;
					G.GarageBlip.Name = "Garage For Sale";
				}
				if (G.Owned == 1)
				{
					G.GarageBlip.Sprite = BlipSprite.Garage;
					G.GarageBlip.Name = G.GarageName + " Garage";
				}
				G.GarageBlip.IsShortRange = true;
				if (!Directory.Exists(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath))
				{
					Directory.CreateDirectory(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath);
				}
				if (G.GarageSize == 2)
				{
					if (!File.Exists(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot1.ini"))
					{
						File.Create(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot1.ini").Close();
						SC.LoadIniFile(G.GarageVehicleSavePath + "Slot1.ini");
						ResetVehicleUpgrades();
					}
					if (!File.Exists(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot2.ini"))
					{
						File.Create(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot2.ini").Close();
						SC.LoadIniFile(G.GarageVehicleSavePath + "Slot2.ini");
						ResetVehicleUpgrades();
					}
				}
				if (G.GarageSize == 6)
				{
					if (!File.Exists(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot1.ini"))
					{
						File.Create(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot1.ini").Close();
						SC.LoadIniFile(G.GarageVehicleSavePath + "Slot1.ini");
						ResetVehicleUpgrades();
					}
					if (!File.Exists(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot2.ini"))
					{
						File.Create(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot2.ini").Close();
						SC.LoadIniFile(G.GarageVehicleSavePath + "Slot2.ini");
						ResetVehicleUpgrades();
					}
					if (!File.Exists(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot3.ini"))
					{
						File.Create(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot3.ini").Close();
						SC.LoadIniFile(G.GarageVehicleSavePath + "Slot3.ini");
						ResetVehicleUpgrades();
					}
					if (!File.Exists(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot4.ini"))
					{
						File.Create(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot4.ini").Close();
						SC.LoadIniFile(G.GarageVehicleSavePath + "Slot4.ini");
						ResetVehicleUpgrades();
					}
					if (!File.Exists(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot5.ini"))
					{
						File.Create(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot5.ini").Close();
						SC.LoadIniFile(G.GarageVehicleSavePath + "Slot5.ini");
						ResetVehicleUpgrades();
					}
					if (!File.Exists(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot6.ini"))
					{
						File.Create(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot6.ini").Close();
						SC.LoadIniFile(G.GarageVehicleSavePath + "Slot6.ini");
						ResetVehicleUpgrades();
					}
				}
				if (G.GarageSize == 10)
				{
					if (!File.Exists(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot1.ini"))
					{
						File.Create(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot1.ini").Close();
						SC.LoadIniFile(G.GarageVehicleSavePath + "Slot1.ini");
						ResetVehicleUpgrades();
					}
					if (!File.Exists(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot2.ini"))
					{
						File.Create(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot2.ini").Close();
						SC.LoadIniFile(G.GarageVehicleSavePath + "Slot2.ini");
						ResetVehicleUpgrades();
					}
					if (!File.Exists(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot3.ini"))
					{
						File.Create(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot3.ini").Close();
						SC.LoadIniFile(G.GarageVehicleSavePath + "Slot3.ini");
						ResetVehicleUpgrades();
					}
					if (!File.Exists(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot4.ini"))
					{
						File.Create(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot4.ini").Close();
						SC.LoadIniFile(G.GarageVehicleSavePath + "Slot4.ini");
						ResetVehicleUpgrades();
					}
					if (!File.Exists(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot5.ini"))
					{
						File.Create(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot5.ini").Close();
						SC.LoadIniFile(G.GarageVehicleSavePath + "Slot5.ini");
						ResetVehicleUpgrades();
					}
					if (!File.Exists(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot6.ini"))
					{
						File.Create(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot6.ini").Close();
						SC.LoadIniFile(G.GarageVehicleSavePath + "Slot6.ini");
						ResetVehicleUpgrades();
					}
					if (!File.Exists(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot7.ini"))
					{
						File.Create(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot7.ini").Close();
						SC.LoadIniFile(G.GarageVehicleSavePath + "Slot7.ini");
						ResetVehicleUpgrades();
					}
					if (!File.Exists(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot8.ini"))
					{
						File.Create(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot8.ini").Close();
						SC.LoadIniFile(G.GarageVehicleSavePath + "Slot8.ini");
						ResetVehicleUpgrades();
					}
					if (!File.Exists(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot9.ini"))
					{
						File.Create(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot9.ini").Close();
						SC.LoadIniFile(G.GarageVehicleSavePath + "Slot9.ini");
						ResetVehicleUpgrades();
					}
					if (!File.Exists(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot10.ini"))
					{
						File.Create(Environment.CurrentDirectory + "\\" + G.GarageVehicleSavePath + "Slot10.ini").Close();
						SC.LoadIniFile(G.GarageVehicleSavePath + "Slot10.ini");
						ResetVehicleUpgrades();
					}
				}
			}
		}

		private unsafe void OnTick(object sender, EventArgs e)
		{
			if (!SetupGarages)
			{
				SetupGTAOGarages();
				SetupGarages = true;
			}
			else
			{
				if (ConvStarted && !Function.Call<bool>(Hash.IS_SCRIPTED_CONVERSATION_ONGOING))
				{
					ConvStarted = false;
					if (ConvPed != null)
					{
						ConvPed.Delete();
					}
				}
				if (MechanicObjectPool != null)
				{
					MechanicObjectPool.Process();
				}
				if (ifruit != null)
				{
					ifruit.Update();
				}
				if (CurrentCar != null && MechanicPed != null && MechanicPed.IsPersistent)
				{
					if (CurrentCar.IsUpsideDown)
					{
						CurrentCar.PlaceOnGround();
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, MechanicPed.Position, "<", 7f))
					{
						if (MechanicPed.CurrentVehicle != null)
						{
							MechanicPed.CurrentVehicle.Speed = 0f;
							if (CurrentCar.AttachedBlip != null)
							{
								CurrentCar.AttachedBlip.Delete();
							}
							MechanicPed.Task.LeaveVehicle(CurrentCar, true);
							Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
							Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, MechanicPed, "mechanic");
							Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "MPCT_GRAA", "mechanic", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
							Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
						}
						if (MechanicPed.CurrentVehicle == null)
						{
							MechanicPed.Task.WanderAround(MechanicPed.Position, 100f);
							MechanicPed.IsPersistent = false;
							MechanicPed.MarkAsNoLongerNeeded();
							CurrentCar.MarkAsNoLongerNeeded();
							CurrentCar = null;
						}
					}
					else if (RerouteTimer != 60)
					{
						RerouteTimer++;
					}
					else
					{
						RerouteTimer = 0;
						MechanicPed.Task.DriveTo(CurrentCar, Game.Player.Character.Position, 5f, 60f);
					}
				}
			}
			if (Class1.Purchased_Penthouse == 0)
			{
				if (Garages[0].GarageBlip != null)
				{
					Garages[0].GarageBlip.Alpha = 0;
				}
				return;
			}
			Garages[0].Owned = 1;
			if (GetDistance_VDIST2(Game.Player.Character.Position, Garages[0].Enterance, ">", 100f) && !InGarage)
			{
				if (Garages[0].GarageBlip != null)
				{
					Garages[0].GarageBlip.Alpha = 0;
				}
				_Debug.PenthouseGarage = 0;
				return;
			}
			if (Garages[0].GarageBlip != null)
			{
				Garages[0].GarageBlip.Alpha = 255;
			}
			if (InGarage && Garages[0].GarageBlip != null)
			{
				Garages[0].GarageBlip.Alpha = 0;
			}
			_Debug.PenthouseGarage = 1;
			if (Function.Call<bool>(Hash.IS_INTERIOR_DISABLED, 146433))
			{
				Function.Call(Hash.DISABLE_INTERIOR, 146433, false);
			}
			if (Function.Call<bool>(Hash.IS_INTERIOR_DISABLED, 148737))
			{
				Function.Call(Hash.DISABLE_INTERIOR, 148737, false);
			}
			if (Function.Call<bool>(Hash.IS_INTERIOR_DISABLED, 149249))
			{
				Function.Call(Hash.DISABLE_INTERIOR, 149249, false);
			}
			if (modLemonUIObjectPool != null)
			{
				modLemonUIObjectPool.Process();
			}
			if (mainMenu != null && mainMenu.Visible)
			{
				if (DisplayingRemoveVehicleCam)
				{
					if (MainCam != null)
					{
						World.RenderingCamera = MainCam;
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						MainCam.IsActive = false;
						MainCam.Delete();
					}
					DisplayingRemoveVehicleCam = false;
				}
				modLemonUIObjectPool.HideAll();
				Notification.Show("Hide ALl 2");
			}
			if (!SetupForSaleSigns)
			{
				foreach (Class1.GTAOGarage garage in Garages)
				{
					if (!garage.MultiGarage)
					{
						garage.RemoveVehicleButtons = new List<NativeItem>();
						if (garage.OrderVehicleButton == null)
						{
							garage.OrderVehicleButton = new List<NativeItem>();
						}
						for (int i = 0; i < garage.GarageSize; i++)
						{
							int iD = i + 1;
							SC.LoadIniFile(garage.GarageVehicleSavePath + "\\Slot" + iD + ".ini");
							Model model = SC.CheckCar(garage.GarageVehicleSavePath + "\\Slot" + iD + ".ini");
							if (model == "null" || model == null)
							{
								NativeItem item = new NativeItem("Empty", "Select Vehicle.");
								garage.RemoveMenu.Add(item);
								garage.RemoveVehicleButtons.Add(item);
								ManageRemoveVehicle(item, iD);
							}
							else
							{
								NativeItem item2 = new NativeItem(SC.VehicleName, "Select Vehicle.");
								garage.RemoveMenu.Add(item2);
								garage.RemoveVehicleButtons.Add(item2);
								ManageRemoveVehicle(item2, iD);
							}
						}
						for (int j = 0; j < GLemonUIMenusNativeMenus.Count; j++)
						{
							SetBanner(GLemonUIMenusNativeMenus[j]);
						}
						continue;
					}
					garage.RemoveVehicleButtons = new List<NativeItem>();
					if (garage.OrderVehicleButton == null)
					{
						garage.OrderVehicleButton = new List<NativeItem>();
					}
					for (int k = 0; k < 10; k++)
					{
						int iD2 = k + 1;
						SC.LoadIniFile(garage.GarageVehicleSavePath + "Garage_1\\Slot" + iD2 + ".ini");
						Model model2 = SC.CheckCar(garage.GarageVehicleSavePath + "Garage_1\\Slot" + iD2 + ".ini");
						if (model2 == "null" || model2 == null)
						{
							NativeItem item3 = new NativeItem("Empty", "Select Vehicle.");
							garage.RemoveMenu.Add(item3);
							garage.RemoveVehicleButtons.Add(item3);
							ManageRemoveVehicle(item3, iD2);
						}
						else
						{
							NativeItem item4 = new NativeItem(SC.VehicleName, "Select Vehicle.");
							garage.RemoveMenu.Add(item4);
							garage.RemoveVehicleButtons.Add(item4);
							ManageRemoveVehicle(item4, iD2);
						}
					}
				}
				SetupForSaleSigns = true;
			}
			CurrentGarage = 0;
			if (InGarage)
			{
				if (!InGarage || LastGarage == -1)
				{
					return;
				}
				CurrentGarage = LastGarage;
				if (CurrentGarage != -1)
				{
					if (GetCurrentGarageVehicle() != null)
					{
						if (!modLemonUIObjectPool.AreAnyVisible)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_MOVE_UP_ONLY~ to exit Garage with Vehicle", 1);
						}
						if (Game.IsControlJustPressed(Control.MoveUpOnly))
						{
							LoadCarinToRealWorld(GetCurrentGarageVehicle());
						}
					}
					if (PodiumVehicleMenu.PodiumVehicle != null && PodiumVehicleMenu.PodiumVehicle.GetPedOnSeat(VehicleSeat.Driver) != null)
					{
						if (!modLemonUIObjectPool.AreAnyVisible)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_MOVE_UP_ONLY~ to exit Garage with Vehicle", 1);
						}
						if (Game.IsControlJustPressed(Control.MoveUpOnly))
						{
							LoadCarinToRealWorld(PodiumVehicleMenu.PodiumVehicle);
						}
					}
					World.DrawMarker(MarkerType.Cylinder, CurrentGarageInterior.MenuActivator, Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 147, 255));
					if (GetDistance_VDIST2(Game.Player.Character.Position, CurrentGarageInterior.MenuActivator, "<", 2f))
					{
						if (!modLemonUIObjectPool.AreAnyVisible)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to access the vehicle management menu.", 1);
						}
						if (Game.IsControlJustPressed(Control.Context) && !DisplayingRemoveVehicleCam)
						{
							if (Garages[CurrentGarage] != null && Garages[CurrentGarage].RemoveMenu != null)
							{
								Garages[CurrentGarage].RemoveMenu.Visible = !Garages[CurrentGarage].RemoveMenu.Visible;
							}
							if (MainCam != null)
							{
								World.RenderingCamera = MainCam;
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
								MainCam.IsActive = false;
								MainCam.Delete();
							}
							MainCam = GlobalHelper.CreateCamera(CurrentGarageInterior.MenuCam.Item1, CurrentGarageInterior.MenuCam.Item2, GameplayCamera.FieldOfView);
							MainCam.IsActive = true;
							MainCam.FarClip = 2000f;
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 1);
							DisplayingRemoveVehicleCam = true;
						}
					}
					World.DrawMarker(MarkerType.Cylinder, CurrentGarageInterior.InteriorMenuPos, Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 147, 255));
					if (GetDistance_VDIST2(Game.Player.Character.Position, CurrentGarageInterior.InteriorMenuPos, "<", 2f))
					{
						if (!modLemonUIObjectPool.AreAnyVisible)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to access the interior management menu.", 1);
						}
						if (Game.IsControlJustPressed(Control.Context))
						{
							GTA.UI.Screen.ShowSubtitle("CurrentGarage " + CurrentGarage);
							if (!DisplayingRemoveVehicleCam)
							{
								if (Garages[CurrentGarage] != null && Garages[CurrentGarage].InteriorMenu != null)
								{
									Garages[CurrentGarage].InteriorMenu.Visible = !Garages[CurrentGarage].InteriorMenu.Visible;
								}
								if (MainCam != null)
								{
									World.RenderingCamera = MainCam;
									Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
									MainCam.IsActive = false;
									MainCam.Delete();
								}
								MainCam = GlobalHelper.CreateCamera(CurrentGarageInterior.MenuCam.Item1, CurrentGarageInterior.MenuCam.Item2, GameplayCamera.FieldOfView);
								MainCam.IsActive = true;
								MainCam.FarClip = 2000f;
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 1);
								DisplayingRemoveVehicleCam = true;
							}
						}
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, CurrentGarageInterior.GarageDoorL, "<", 2f))
					{
						if (!modLemonUIObjectPool.AreAnyVisible)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit " + Garages[CurrentGarage].GarageName + " Garage", 1);
						}
						if (Game.IsControlJustPressed(Control.Context))
						{
							int taskID = 0;
							OpenSequenceTask(&taskID);
							TaskPedSlideToCoord(0, CurrentGarageInterior.GarageDoorL, CurrentGarageInterior.ExitHeading, 8f);
							CloseSequenceTask(taskID);
							TaskPerformSequence(Game.Player.Character, taskID);
							ClearSequenceTask(&taskID);
							Game.Player.Character.Position = CurrentGarageInterior.GarageDoorL;
							Game.Player.Character.Heading = CurrentGarageInterior.ExitHeading;
							RaycastResult raycastResult = World.Raycast(Game.Player.Character.Position, Game.Player.Character.GetOffsetPosition(new Vector3(0f, 3f, 0f)), IntersectFlags.Map);
							if (raycastResult.DidHit)
							{
								Game.Player.Character.Position = raycastResult.HitPosition;
								Game.Player.Character.Heading = CurrentGarageInterior.ExitHeading;
								Game.Player.Character.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, -1.15f, -1.45f));
							}
							Game.Player.Character.Task.PlayAnimation("anim@apt_trans@garage", "gar_open_1_left", 1f, -1, AnimationFlags.None);
							Script.Wait(1500);
							Audio.PlaySoundAt(Game.Player.Character, "GARAGE_DOOR_SCRIPTED_CLOSE");
							GTA.UI.Screen.FadeOut(500);
							Script.Wait(2500);
							LastGarage = -1;
							Game.Player.Character.Position = Garages[CurrentGarage].Enterance;
							InGarage = false;
							DeleteVehicles();
							Script.Wait(500);
							GTA.UI.Screen.FadeIn(500);
							return;
						}
					}
					if (!GetDistance_VDIST2(Game.Player.Character.Position, CurrentGarageInterior.GarageDoorR, "<", 2f))
					{
						return;
					}
					if (!modLemonUIObjectPool.AreAnyVisible)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit " + Garages[CurrentGarage].GarageName + " Garage", 1);
					}
					if (Game.IsControlJustPressed(Control.Context))
					{
						int taskID2 = 0;
						OpenSequenceTask(&taskID2);
						TaskPedSlideToCoord(0, CurrentGarageInterior.GarageDoorR, CurrentGarageInterior.ExitHeading, 8f);
						CloseSequenceTask(taskID2);
						TaskPerformSequence(Game.Player.Character, taskID2);
						ClearSequenceTask(&taskID2);
						Game.Player.Character.Position = CurrentGarageInterior.GarageDoorR;
						Game.Player.Character.Heading = CurrentGarageInterior.ExitHeading;
						RaycastResult raycastResult2 = World.Raycast(Game.Player.Character.Position, Game.Player.Character.GetOffsetPosition(new Vector3(0f, 3f, 0f)), IntersectFlags.Map);
						if (raycastResult2.DidHit)
						{
							Game.Player.Character.Position = raycastResult2.HitPosition;
							Game.Player.Character.Heading = CurrentGarageInterior.ExitHeading;
							Game.Player.Character.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, -1.15f, -1.45f));
						}
						Game.Player.Character.Task.PlayAnimation("anim@apt_trans@garage", "gar_open_1_left", 1f, -1, AnimationFlags.None);
						Script.Wait(1500);
						Audio.PlaySoundAt(Game.Player.Character, "GARAGE_DOOR_SCRIPTED_CLOSE");
						GTA.UI.Screen.FadeOut(500);
						Script.Wait(2500);
						LastGarage = -1;
						Game.Player.Character.Position = Garages[CurrentGarage].Enterance;
						InGarage = false;
						DeleteVehicles();
						Script.Wait(500);
						GTA.UI.Screen.FadeIn(500);
					}
				}
				else
				{
					GTA.UI.Screen.ShowSubtitle("CurrentGarage " + CurrentGarage);
				}
			}
			else
			{
				if (CurrentGarage == -1 || CurrentGarage > Garages.Count || !GetDistance_VDIST2(Game.Player.Character.Position, Garages[CurrentGarage].Enterance, "<", 100f) || Garages[CurrentGarage].Owned != 1)
				{
					return;
				}
				Vector3 enterance = Garages[CurrentGarage].Enterance;
				World.DrawMarker(MarkerType.Cylinder, enterance, Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 147, 255));
				float radius = 2f;
				if (Game.Player.Character.CurrentVehicle != null)
				{
					radius = 5f;
				}
				if (Garages[CurrentGarage].GarageSize != 50)
				{
					radius = 2f;
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, enterance, "<", radius))
				{
					if (Garages[CurrentGarage].GarageSize != 50)
					{
						if (!modLemonUIObjectPool.AreAnyVisible)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to enter Penthouse Garage", 1);
						}
						if (Game.IsControlJustPressed(Control.Context))
						{
							CurrentEclipseGarage = "";
							int taskID3 = 0;
							OpenSequenceTask(&taskID3);
							TaskPedSlideToCoord(0, Garages[CurrentGarage].Enterance, Garages[CurrentGarage].EnterHeading, 8f);
							CloseSequenceTask(taskID3);
							TaskPerformSequence(Game.Player.Character, taskID3);
							ClearSequenceTask(&taskID3);
							Game.Player.Character.Position = Garages[CurrentGarage].Enterance;
							Game.Player.Character.Heading = Garages[CurrentGarage].EnterHeading;
							RaycastResult raycastResult3 = World.Raycast(Game.Player.Character.Position, Game.Player.Character.GetOffsetPosition(new Vector3(0f, 3f, 0f)), IntersectFlags.Map);
							if (raycastResult3.DidHit)
							{
								Game.Player.Character.Position = raycastResult3.HitPosition;
								Game.Player.Character.Heading = Garages[CurrentGarage].EnterHeading;
								Game.Player.Character.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, -1.15f, -1.45f));
							}
							Game.Player.Character.Task.PlayAnimation("anim@apt_trans@garage", "gar_open_1_left", 1f, -1, AnimationFlags.None);
							Script.Wait(1500);
							Audio.PlaySoundAt(Game.Player.Character, "GARAGE_DOOR_SCRIPTED_CLOSE");
							GTA.UI.Screen.FadeOut(500);
							Script.Wait(2500);
							LastGarage = CurrentGarage;
							if (Garages[CurrentGarage].GarageSize == 2)
							{
								GarageTypeIn = GarageType.Two;
								Game.Player.Character.Position = TwoCarGarage.GarageDoorL;
								CurrentGarageInterior = TwoCarGarage;
								if (CurrentGarageInterior != null)
								{
									LoadVehicles(Garages[CurrentGarage].GarageVehicleSavePath, "", Garages[CurrentGarage], CurrentGarageInterior);
								}
							}
							if (Garages[CurrentGarage].GarageSize == 6)
							{
								GarageTypeIn = GarageType.Six;
								Game.Player.Character.Position = SixCarGarage.GarageDoorL;
								CurrentGarageInterior = SixCarGarage;
								if (CurrentGarageInterior != null)
								{
									LoadVehicles(Garages[CurrentGarage].GarageVehicleSavePath, "", Garages[CurrentGarage], CurrentGarageInterior);
								}
							}
							if (Garages[CurrentGarage].GarageSize == 10)
							{
								GarageTypeIn = GarageType.Ten;
								Game.Player.Character.Position = TenCarGarage.GarageDoorL;
								CurrentGarageInterior = TenCarGarage;
								if (CurrentGarageInterior != null)
								{
									LoadVehicles(Garages[CurrentGarage].GarageVehicleSavePath, "", Garages[CurrentGarage], CurrentGarageInterior);
								}
							}
							InGarage = true;
							Script.Wait(500);
							GTA.UI.Screen.FadeIn(500);
						}
					}
					else if (!GarageCamActive)
					{
						GarageCamActive = true;
						if (MainCam != null)
						{
							World.RenderingCamera = MainCam;
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
							MainCam.IsActive = false;
							MainCam.Delete();
						}
						MainCam = GlobalHelper.CreateCamera(Garages[CurrentGarage].CameraPosRot.Item1, Garages[CurrentGarage].CameraPosRot.Item2, GameplayCamera.FieldOfView);
						MainCam.PointAt(Game.Player.Character.Position);
						MainCam.IsActive = true;
						MainCam.FarClip = 2000f;
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 1);
						Garages[CurrentGarage].EclipseMenu.Visible = !Garages[CurrentGarage].EclipseMenu.Visible;
					}
				}
				if (GarageCamActive)
				{
					if (Game.Player.Character.CurrentVehicle != null && GetDistance_VDIST2(Game.Player.Character.Position, enterance, ">", 5f))
					{
						modLemonUIObjectPool.HideAll();
						GarageCamActive = false;
						if (MainCam != null)
						{
							World.RenderingCamera = MainCam;
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 1);
							MainCam.IsActive = false;
							MainCam.Delete();
						}
					}
					if (Game.Player.Character.CurrentVehicle == null && GetDistance_VDIST2(Game.Player.Character.Position, enterance, ">", 2f))
					{
						modLemonUIObjectPool.HideAll();
						GarageCamActive = false;
						if (MainCam != null)
						{
							World.RenderingCamera = MainCam;
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 1);
							MainCam.IsActive = false;
							MainCam.Delete();
						}
					}
				}
				if (!(Game.Player.Character.CurrentVehicle != null) || Garages[CurrentGarage].GarageSize == 50)
				{
					return;
				}
				if (!JustExitedWithVehicle && GetDistance_VDIST2(Game.Player.Character.Position, Garages[CurrentGarage].VehicleSpawn, "<", 3f))
				{
					if (Garages[CurrentGarage].GarageSize != 50)
					{
						if (!SavingVehicle)
						{
							LastGarage = CurrentGarage;
							SavingVehicle = true;
							if (Garages[CurrentGarage].GarageSize == 2)
							{
								GarageTypeIn = GarageType.Two;
								CurrentGarageInterior = TwoCarGarage;
								if (CurrentGarageInterior != null)
								{
									SaveVehicleAdv(false, Garages[CurrentGarage], "", CurrentGarageInterior);
								}
							}
							if (Garages[CurrentGarage].GarageSize == 6)
							{
								GarageTypeIn = GarageType.Six;
								CurrentGarageInterior = SixCarGarage;
								if (CurrentGarageInterior != null)
								{
									SaveVehicleAdv(false, Garages[CurrentGarage], "", CurrentGarageInterior);
								}
							}
							if (Garages[CurrentGarage].GarageSize == 10)
							{
								GarageTypeIn = GarageType.Ten;
								CurrentGarageInterior = TenCarGarage;
								if (CurrentGarageInterior != null)
								{
									SaveVehicleAdv(false, Garages[CurrentGarage], "", CurrentGarageInterior);
								}
							}
							InGarage = true;
						}
					}
					else if (!GarageCamActive)
					{
						GarageCamActive = true;
						if (MainCam != null)
						{
							World.RenderingCamera = MainCam;
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
							MainCam.IsActive = false;
							MainCam.Delete();
						}
						MainCam = GlobalHelper.CreateCamera(Garages[CurrentGarage].CameraPosRot.Item1, Garages[CurrentGarage].CameraPosRot.Item2, GameplayCamera.FieldOfView);
						MainCam.PointAt(Game.Player.Character.Position);
						MainCam.IsActive = true;
						MainCam.FarClip = 2000f;
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 1);
						Garages[CurrentGarage].EclipseMenu.Visible = !Garages[CurrentGarage].EclipseMenu.Visible;
					}
				}
				if (JustExitedWithVehicle && GetDistance_VDIST2(Game.Player.Character.Position, Garages[CurrentGarage].VehicleSpawn, ">", 7f))
				{
					JustExitedWithVehicle = false;
				}
			}
		}
	}
}
