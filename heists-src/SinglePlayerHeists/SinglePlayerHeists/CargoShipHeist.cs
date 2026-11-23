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

namespace SinglePlayerHeists
{
	public class CargoShipHeist : Script
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

		public enum Nodetype
		{
			AnyRoad,
			Road,
			Offroad,
			Water
		}

		public class AircraftWithWeapons
		{
			public Ped TargetPed { get; set; }

			public Ped Pilot { get; set; }

			public Vehicle Heli { get; set; }

			public int RoundsToFire { get; set; }

			public int RoundsFired { get; set; }

			public int NextFireTime { get; set; }

			public AircraftWithWeapons()
			{
			}

			public AircraftWithWeapons(Ped T, Ped P, Vehicle H, int R)
			{
				TargetPed = T;
				Pilot = P;
				Heli = H;
				RoundsToFire = R;
			}
		}

		public class AircraftWeaponController : Script
		{
			public static List<AircraftWithWeapons> AircraftWithWeapons = new List<AircraftWithWeapons>();

			public AircraftWeaponController()
			{
				base.Tick += update;
			}

			private static void drawText(string text, float x, float y, float scale, int r, int g, int b)
			{
				Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
				Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
				Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
				Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
			}

			public void update(object sender, EventArgs e)
			{
				for (int i = 0; i < AircraftWithWeapons.Count; i++)
				{
					AircraftWithWeapons aircraftWithWeapons = AircraftWithWeapons[i];
					if (aircraftWithWeapons.Heli != null && aircraftWithWeapons.Heli.IsAlive && aircraftWithWeapons.Pilot != null && aircraftWithWeapons.Pilot.IsAlive && aircraftWithWeapons.RoundsFired < aircraftWithWeapons.RoundsToFire && Game.GameTime >= aircraftWithWeapons.NextFireTime)
					{
						aircraftWithWeapons.NextFireTime = Game.GameTime + 2500;
						Vector3 position = Game.Player.Character.Position;
						aircraftWithWeapons.RoundsFired++;
						Function.Call(Hash.SET_VEHICLE_SHOOT_AT_TARGET, aircraftWithWeapons.Heli, Game.Player.Character, position.X, position.Y, position.Z);
						Function.Call(Hash.SET_VEHICLE_SHOOT_AT_TARGET, aircraftWithWeapons.Heli, Game.Player.Character, position.X, position.Y, position.Z);
						Function.Call(Hash.SET_VEHICLE_SHOOT_AT_TARGET, aircraftWithWeapons.Heli, Game.Player.Character, position.X, position.Y, position.Z);
					}
					if (aircraftWithWeapons.RoundsFired >= aircraftWithWeapons.RoundsToFire)
					{
						AircraftWithWeapons.RemoveAt(i);
					}
				}
			}
		}

		public class PedTaskLoop : Script
		{
			public List<Vector3> Loop { get; set; }

			public int AtPoint { get; set; }

			public bool AllowDelay { get; set; }

			public bool OverrideDelayOff { get; set; }

			public int Timerdelay { get; set; }

			public int Timer { get; set; }

			public PedTaskLoop()
			{
			}

			public PedTaskLoop(List<Vector3> L, int Point)
			{
				Loop = L;
			}

			public PedTaskLoop(List<Vector3> L, int Point, bool AD)
			{
				OverrideDelayOff = AD;
				Loop = L;
			}
		}

		public class CayoPericoSpawnGroup : Script
		{
			public Vector3 SpawnGroupArea { get; set; }

			public float DistToSpawn { get; set; }

			public bool GroupActive { get; set; }

			public List<SmartGuards.SmartAiV2> AIS { get; set; }

			public CayoPericoSpawnGroup()
			{
			}

			public CayoPericoSpawnGroup(Vector3 AreaLoc, float SpawnDist, bool Active, List<SmartGuards.SmartAiV2> ais)
			{
				SpawnGroupArea = AreaLoc;
				DistToSpawn = SpawnDist;
				GroupActive = Active;
			}
		}

		public class SamSite
		{
			public bool IsDead { get; set; }

			public Prop Sam { get; set; }

			public int roundsFired { get; set; }

			public int fireDelay { get; set; }

			public SamSite()
			{
			}

			public SamSite(Prop S)
			{
				roundsFired = 0;
				Sam = S;
				fireDelay = 0;
			}
		}

		public enum CargoShipStage
		{
			Scout,
			Grinder,
			Savage,
			Kosatka,
			KosatkaC4,
			SamJammer,
			NearCargoShip,
			Manifest,
			Thermite,
			CannotOpenContainer
		}

		public class Container
		{
			public int PTFX1 { get; set; }

			public int PTFX2 { get; set; }

			public Prop Bomb { get; set; }

			public int plantBombStage { get; set; }

			public int plantThermalChargeStage { get; set; }

			public Prop Bag { get; set; }

			public int containerswitch { get; set; }

			public Prop Container1 { get; set; }

			public int BombExplodeTimer { get; set; }

			public Prop Lock { get; set; }

			public Prop Saw { get; set; }

			public Prop FakeContainer { get; set; }

			public int ParticleFx { get; set; }

			public int SyncScene { get; set; }

			public int SyncScene2 { get; set; }

			public int ParticleTimer { get; set; }

			public Container()
			{
			}

			public Container(Prop C)
			{
				if (Lock != null)
				{
					Lock.Delete();
				}
				Container1 = C;
			}
		}

		public class LargeContainer
		{
			public MissionOjects CargoHeist_JuggernautLootContainer = new MissionOjects(new List<ObjectPlacementMap>());

			private List<Prop> proplist = new List<Prop>();

			public bool isPlayerMoving { get; set; }

			public bool disablewalking { get; set; }

			public int disablewalkingint { get; set; }

			public Prop Container { get; set; }

			public Prop FakeContainer { get; set; }

			public int avicci2 { get; set; }

			public int avicci { get; set; }

			public int unstucktimer { get; set; }

			public Prop swipecard { get; set; }

			public int mainindex { get; set; }

			public int quickanimtestint { get; set; }

			public Prop propcreate(Vector3 pos, Vector3 rot, string name, bool isfrozen, bool hascollision)
			{
				Model model = new Model(name);
				model.Request(10000);
				Prop prop = null;
				if (model.IsValid && model.IsInCdImage)
				{
					while (!model.IsLoaded)
					{
						Script.Wait(50);
					}
					prop = GlobalHelper.CreateProp(model, pos, rot, false, false);
					prop.Position = pos;
					prop.Rotation = rot;
					prop.IsPositionFrozen = isfrozen;
					prop.IsCollisionEnabled = hascollision;
					proplist.Add(prop);
				}
				return prop;
			}

			public static void DisplayHelpTextThisFrame(string text)
			{
				Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "STRING");
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
				Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, -1);
			}

			private void AddDuffel()
			{
				if (Game.Player.Character.Model == PedHash.Franklin)
				{
					Ped character = Game.Player.Character;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[5] { character, 9, 1, 0, 2 });
				}
				if (Game.Player.Character.Model == PedHash.Michael)
				{
					Ped character2 = Game.Player.Character;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[5] { character2, 9, 1, 0, 2 });
				}
				if (Game.Player.Character.Model == PedHash.Trevor)
				{
					Ped character3 = Game.Player.Character;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[5] { character3, 9, 1, 0, 2 });
				}
				if (Game.Player.Character.Model == PedHash.FreemodeMale01)
				{
					Ped character4 = Game.Player.Character;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[5] { character4, 5, 45, 0, 2 });
				}
				if (Game.Player.Character.Model == PedHash.FreemodeFemale01)
				{
					Ped character5 = Game.Player.Character;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[5] { character5, 5, 45, 0, 2 });
				}
			}

			private void disablewalkingvoid()
			{
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 32, true);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 33, true);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 34, true);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 35, true);
				Game.DisableControlThisFrame(Control.MoveUpOnly);
				Game.DisableControlThisFrame(Control.MoveDownOnly);
				Game.DisableControlThisFrame(Control.MoveLeftOnly);
				Game.DisableControlThisFrame(Control.MoveRightOnly);
			}

			private void DelDuffel()
			{
				if (Game.Player.Character.Model == PedHash.Franklin)
				{
					Ped character = Game.Player.Character;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[5] { character, 9, 0, 0, 2 });
				}
				if (Game.Player.Character.Model == PedHash.Michael)
				{
					Ped character2 = Game.Player.Character;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[5] { character2, 9, 0, 0, 2 });
				}
				if (Game.Player.Character.Model == PedHash.Trevor)
				{
					Ped character3 = Game.Player.Character;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[5] { character3, 9, 0, 0, 2 });
				}
				if (Game.Player.Character.Model == PedHash.FreemodeMale01)
				{
					Ped character4 = Game.Player.Character;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[5] { character4, 5, 0, 0, 2 });
				}
				if (Game.Player.Character.Model == PedHash.FreemodeFemale01)
				{
					Ped character5 = Game.Player.Character;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[5] { character5, 5, 0, 0, 2 });
				}
			}

			public void quickanimtest(LargeContainer C)
			{
				if (!(C.Container != null))
				{
					return;
				}
				switch (C.quickanimtestint)
				{
				case 0:
				{
					Prop container = C.Container;
					if (Game.Player.Character.Position.DistanceTo(new Vector3(container.Position.X, container.Position.Y, container.Position.Z)) < 4.5f)
					{
						Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@freemode@ig1_open_container@male@");
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to swipe the card");
						if (Game.IsControlJustPressed(Control.Context))
						{
							LoadDict("anim@scripted@freemode@ig1_open_container@male@");
							container.Detach();
							Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
							Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "anim@scripted@freemode@ig1_open_container@male@", "action", container.Position.X, container.Position.Y, container.Position.Z, 0f, 0f, container.Rotation.Z, 0f, 2);
							Function.Call(argument6: Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "anim@scripted@freemode@ig1_open_container@male@", "action", container.Position.X, container.Position.Y, container.Position.Z, 0f, 0f, container.Rotation.Z, 0f, 2).Z, hash: Hash.TASK_GO_STRAIGHT_TO_COORD, argument0: Game.Player.Character, argument1: vector.X, argument2: vector.Y, argument3: vector.Z, argument4: 1f, argument5: 3000, argument7: 0f);
							Function.Call(Hash.SET_PED_MOVE_RATE_OVERRIDE, Game.Player.Character.Handle, 0f);
							C.unstucktimer = Game.GameTime + 2000;
							C.quickanimtestint = 1;
						}
					}
					break;
				}
				case 1:
				{
					disablewalkingvoid();
					if (Function.Call<bool>(Hash.IS_PED_WALKING, Game.Player.Character) || Game.GameTime <= C.unstucktimer)
					{
						break;
					}
					Model model = new Model("m23_1_prop_m31_swipe_card_01a");
					model.Request(10000);
					if (model.IsValid && model.IsInCdImage)
					{
						while (!model.IsLoaded)
						{
							Script.Wait(50);
						}
						C.swipecard = GlobalHelper.CreateProp(model, Game.Player.Character.Position, false, false);
						C.swipecard.IsCollisionEnabled = false;
						C.quickanimtestint = 10;
					}
					break;
				}
				case 10:
					Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, C.swipecard, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 90f, 0f, 0f, false, false, false, false, 0, true);
					Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@freemode@ig1_open_container@male@");
					C.avicci2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, C.Container.Position.X, C.Container.Position.Y, C.Container.Position.Z, 0f, 0f, C.Container.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, C.avicci2, "anim@scripted@freemode@ig1_open_container@male@", "action", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, C.Container, C.avicci2, "action_container", "anim@scripted@freemode@ig1_open_container@male@", 1f, -1f, 0, 1148846080);
					C.quickanimtestint = 20;
					break;
				case 20:
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, C.avicci2) > 0.99f)
					{
						Function.Call(Hash.SET_PED_MOVE_RATE_OVERRIDE, Game.Player.Character.Handle, -1f);
						C.swipecard.Delete();
						C.disablewalking = false;
						AddDuffel();
						Game.Player.Character.Task.ClearAll();
						Prop fakeContainer = propcreate(C.Container.Position, C.Container.Rotation, "m23_1_prop_m31_container_01b", true, true);
						C.Container.Delete();
						C.FakeContainer = fakeContainer;
						C.quickanimtestint = 0;
					}
					break;
				}
			}

			public string LoadDict(string dict)
			{
				int num = 0;
				while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, dict))
				{
					Function.Call(Hash.REQUEST_ANIM_DICT, dict);
					Script.Yield();
					num++;
					if (num >= 1000)
					{
						Notification.Show("Animation Not Found " + dict);
						break;
					}
				}
				return dict;
			}

			public LargeContainer()
			{
			}

			public LargeContainer(Prop C)
			{
				Container = C;
			}
		}

		public static List<SmartGuards.SmartAiV2> SpawnGroupsUse = new List<SmartGuards.SmartAiV2>();

		public static int RunHeist = 0;

		public static Vector3 EventLoc;

		public static string EventText;

		public int Key_HeistGrabLoot = 51;

		public static List<Vehicle> MerryweatherVehicles = new List<Vehicle>();

		public static List<Ped> MerryweatherPeds = new List<Ped>();

		public static int Mod_Difficulty = 50;

		public static List<MissionOjects> CargoshipNav = new List<MissionOjects>
		{
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 263750, 1803116220u, new Vector3(-377.0176f, -4101.358f, 8.6482f), new Vector3(0f, 0f, 39.7033234f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 327488, 1803116220u, new Vector3(-383.924438f, -4092.78613f, 8.648715f), new Vector3(0f, -0f, -110.512695f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 445272, 1803116220u, new Vector3(-378.8507f, -4088.0127f, 8.648216f), new Vector3(0f, -0f, -122.795036f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 139342, 1803116220u, new Vector3(-370.4983f, -4095.906f, 8.648199f), new Vector3(0f, -0f, -131.64743f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 87871, 1803116220u, new Vector3(-364.507843f, -4105.39355f, 8.648198f), new Vector3(0f, -0f, -145.312027f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 290905, 1803116220u, new Vector3(-369.9193f, -4109.19238f, 8.648198f), new Vector3(0f, 0f, 48.6049843f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Billionaire"), 422234, 28135809u, new Vector3(-369.109161f, -4110.04443f, 9.31440449f), new Vector3(0f, 0f, 50.14023f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 227934, 1803116220u, new Vector3(-370.6502f, -4095.90552f, 8.648198f), new Vector3(0f, -0f, -137.73642f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 282188, 1803116220u, new Vector3(-364.694336f, -4105.285f, 8.831405f), new Vector3(0f, -0f, -142.905151f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 737806, 1803116220u, new Vector3(-370.531219f, -4095.79517f, 8.648198f), new Vector3(0f, -0f, -136.814056f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 597077, 1803116220u, new Vector3(-378.699921f, -4087.834f, 8.648216f), new Vector3(0f, -0f, -128.762817f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Billionaire"), 187484, 28135809u, new Vector3(-380.0555f, -4086.46753f, 9.314165f), new Vector3(0f, -0f, -136.848877f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 595536, 1803116220u, new Vector3(-393.398743f, -4111.78369f, 8.648715f), new Vector3(0f, -0f, -139.56041f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 744031, 1803116220u, new Vector3(-384.634674f, -4122.71143f, 8.648685f), new Vector3(0f, -0f, -140.429916f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 43091, 1803116220u, new Vector3(-393.408966f, -4111.6626f, 8.648714f), new Vector3(0f, -0f, -139.2525f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 628026, 1803116220u, new Vector3(-399.9323f, -4104.10059f, 8.648714f), new Vector3(0f, -0f, -139.354614f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Billionaire"), 586594, 28135809u, new Vector3(-400.33493f, -4103.64844f, 9.314379f), new Vector3(0f, -0f, -139.71257f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 286040, 1803116220u, new Vector3(-344.805969f, -4073.8645f, 8.648269f), new Vector3(0f, -0f, -142.27034f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 629075, 1803116220u, new Vector3(-352.852234f, -4064.6394f, 8.648269f), new Vector3(0f, 0f, -40.9959526f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 86082, 1803116220u, new Vector3(-346.1256f, -4072.3396f, 8.648269f), new Vector3(0f, 0f, 40.3306274f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 468811, 1803116220u, new Vector3(-336.875946f, -4082.94f, 8.648269f), new Vector3(0f, -0f, 132.455154f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Billionaire"), 600370, 28135809u, new Vector3(-336.5434f, -4083.483f, 9.313821f), new Vector3(0f, 0f, 39.40911f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 110436, 1803116220u, new Vector3(-423.7212f, -4141.41748f, 8.649068f), new Vector3(0f, 0f, 48.2629967f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 734239, 1803116220u, new Vector3(-428.655273f, -4132.89551f, 8.649069f), new Vector3(0f, 0f, 40.943985f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 474698, 1803116220u, new Vector3(-414.688629f, -4117.3833f, 8.648858f), new Vector3(0f, 0f, -55.7874f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 726343, 1803116220u, new Vector3(-399.763153f, -4104.41455f, 8.648714f), new Vector3(0f, 0f, -74.722084f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 573257, 1803116220u, new Vector3(-384.669922f, -4122.49561f, 8.893214f), new Vector3(0f, -0f, -138.9021f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 156765, 1803116220u, new Vector3(-395.627136f, -4131.524f, 8.648707f), new Vector3(0f, -0f, 129.994537f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 459854, 1803116220u, new Vector3(-407.513123f, -4140.99365f, 8.649068f), new Vector3(0f, 0f, 25.686409f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 666957, 1803116220u, new Vector3(-417.14682f, -4146.77148f, 8.649067f), new Vector3(0f, 0f, 40.6822929f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Billionaire"), 234833, 28135809u, new Vector3(-416.658478f, -4147.29443f, 9.315304f), new Vector3(0f, 0f, 39.30026f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 421454, 1803116220u, new Vector3(-356.54895f, -4085.49927f, 8.64827251f), new Vector3(0f, 0f, 33.2601662f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 192868, 1803116220u, new Vector3(-365.4569f, -4074.7146f, 8.648215f), new Vector3(0f, 0f, 37.8151131f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 561206, 1803116220u, new Vector3(-358.358032f, -4083.40259f, 8.648271f), new Vector3(0f, 0f, 35.8189735f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 84829, 1803116220u, new Vector3(-350.38385f, -4092.35767f, 8.648217f), new Vector3(0f, 0f, -58.9168243f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Billionaire"), 10070, 28135809u, new Vector3(-348.795471f, -4093.831f, 9.314204f), new Vector3(0f, 0f, 40.937336f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 42010, 1803116220u, new Vector3(-321.624542f, -4064.44482f, 8.648257f), new Vector3(0f, 0f, 41.60253f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 440677, 1803116220u, new Vector3(-326.804749f, -4059.08569f, 8.648271f), new Vector3(0f, 0f, 42.93315f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 509287, 1803116220u, new Vector3(-332.1183f, -4051.835f, 8.648101f), new Vector3(0f, 0f, 40.2715836f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 776038, 1803116220u, new Vector3(-326.491974f, -4058.94629f, 8.648269f), new Vector3(0f, 0f, 41.5510178f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Billionaire"), 473187, 28135809u, new Vector3(-318.1616f, -4068.01929f, 9.313527f), new Vector3(0f, 0f, 40.06584f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 516430, 1803116220u, new Vector3(-304.027069f, -4052.399f, 8.697504f), new Vector3(0f, -0f, 133.309f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 731470, 1803116220u, new Vector3(-310.954254f, -4060.36328f, 8.647233f), new Vector3(0f, -0f, 137.044617f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 710251, 1803116220u, new Vector3(-318.9317f, -4066.99976f, 8.648257f), new Vector3(0f, -0f, 134.178055f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 155718, 1803116220u, new Vector3(-333.2288f, -4050.82739f, 8.6481f), new Vector3(0f, -0f, -135.128571f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 554549, 1803116220u, new Vector3(-323.523163f, -4043.0188f, 8.648501f), new Vector3(0f, -0f, -174.9441f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 192105, 1803116220u, new Vector3(-314.731873f, -4037.20264f, 8.694086f), new Vector3(0f, -0f, -119.053841f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 770392, 1803116220u, new Vector3(-308.777283f, -4044.97485f, 8.648534f), new Vector3(0f, -0f, 108.029022f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Billionaire"), 784487, 28135809u, new Vector3(-302.171631f, -4048.24121f, 9.359119f), new Vector3(0f, -0f, 130.137451f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 321899, 1803116220u, new Vector3(-398.088348f, -4110.40674f, 16.6650314f), new Vector3(0f, -0f, -155.752518f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 313452, 1803116220u, new Vector3(-393.841431f, -4115.378f, 16.6652222f), new Vector3(0f, -0f, -144.338654f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 86889, 1803116220u, new Vector3(-388.896118f, -4121.384f, 16.6650352f), new Vector3(0f, -0f, -142.597824f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 609113, 1803116220u, new Vector3(-386.271332f, -4124.24854f, 16.66499f), new Vector3(0f, -0f, -171.822449f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 85099, 1803116220u, new Vector3(-393.434265f, -4115.9126f, 16.665205f), new Vector3(0f, -0f, 129.676346f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 23377, 1803116220u, new Vector3(-400.724243f, -4107.18066f, 16.6651459f), new Vector3(0f, -0f, 133.770508f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Billionaire"), 766828, 28135809u, new Vector3(-401.182861f, -4106.797f, 17.339922f), new Vector3(0f, -0f, -135.1771f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 479079, 1803116220u, new Vector3(-429.0647f, -4132.1416f, 8.649068f), new Vector3(0f, 0f, -44.99437f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 42352, 1803116220u, new Vector3(-415.0812f, -4117.57764f, 8.648857f), new Vector3(0f, 0f, -44.38006f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 572008, 1803116220u, new Vector3(-400.073242f, -4104.371f, 8.648714f), new Vector3(0f, 0f, -55.5884f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 594280, 1803116220u, new Vector3(-379.310669f, -4087.18921f, 8.648216f), new Vector3(0f, 0f, -57.2766533f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 186467, 1803116220u, new Vector3(-365.009979f, -4074.909f, 8.648216f), new Vector3(0f, 0f, -45.70934f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 680286, 1803116220u, new Vector3(-352.431458f, -4064.679f, 8.648269f), new Vector3(0f, 0f, -52.3625832f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 491068, 1803116220u, new Vector3(-323.700684f, -4043.12769f, 8.648341f), new Vector3(0f, 0f, -76.8263f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 494188, 1803116220u, new Vector3(-315.147827f, -4037.374f, 8.694874f), new Vector3(0f, 0f, -73.345665f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 701297, 1803116220u, new Vector3(-333.1339f, -4050.7373f, 8.695905f), new Vector3(0f, -0f, 151.22879f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 767069, 1803116220u, new Vector3(-352.619537f, -4064.56f, 8.648269f), new Vector3(0f, -0f, 129.885086f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 741738, 1803116220u, new Vector3(-379.050537f, -4087.096f, 8.648216f), new Vector3(0f, -0f, 125.226105f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 443496, 1803116220u, new Vector3(-399.716064f, -4104.22852f, 8.648714f), new Vector3(0f, -0f, 136.689636f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 37969, 1803116220u, new Vector3(-422.0824f, -4124.409f, 8.649068f), new Vector3(0f, -0f, 130.801956f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 617074, 1803116220u, new Vector3(-428.801941f, -4132.619f, 8.649068f), new Vector3(0f, -0f, 122.867928f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Billionaire"), 467280, 28135809u, new Vector3(-430.418121f, -4133.35f, 9.31539f), new Vector3(0f, 0f, -53.54528f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 405346, 1803116220u, new Vector3(-323.733673f, -4042.7915f, 8.648763f), new Vector3(0f, -0f, 132.2898f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 11876, 1803116220u, new Vector3(-332.915039f, -4051.03687f, 8.648101f), new Vector3(0f, 0f, -49.78812f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 458610, 1803116220u, new Vector3(-319.107056f, -4067.219f, 8.648256f), new Vector3(0f, -0f, -137.9715f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 247385, 1803116220u, new Vector3(-349.401337f, -4093.30347f, 8.648215f), new Vector3(0f, 0f, 0f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 233827, 1803116220u, new Vector3(-366.7278f, -4107.865f, 8.648198f), new Vector3(0f, 0f, 40.9955864f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 493156, 1803116220u, new Vector3(-384.2995f, -4122.521f, 8.648686f), new Vector3(0f, 0f, 38.53873f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 353346, 1803116220u, new Vector3(-407.155853f, -4140.6f, 8.649068f), new Vector3(0f, 0f, 36.0818977f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 163949, 1803116220u, new Vector3(-417.041046f, -4146.36865f, 8.649068f), new Vector3(0f, 0f, 54.8139534f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 521331, 1803116220u, new Vector3(-395.8432f, -4131.689f, 8.648706f), new Vector3(0f, 0f, -41.2021523f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 147538, 1803116220u, new Vector3(-384.557526f, -4122.239f, 8.648685f), new Vector3(0f, 0f, 78.91952f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 423260, 1803116220u, new Vector3(-367.1249f, -4107.82227f, 8.648198f), new Vector3(0f, 0f, 27.738018f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 212852, 1803116220u, new Vector3(-336.6765f, -4082.71436f, 8.648269f), new Vector3(0f, 0f, 63.35949f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 327739, 1803116220u, new Vector3(-318.8261f, -4067.31665f, 8.648256f), new Vector3(0f, 0f, -29.687973f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 508517, 1803116220u, new Vector3(-332.7712f, -4050.78f, 8.648101f), new Vector3(0f, 0f, 27.4297829f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 662581, 1803116220u, new Vector3(-314.947754f, -4036.94f, 8.698064f), new Vector3(0f, 0f, -62.5973358f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Billionaire"), 47461, 28135809u, new Vector3(-312.757721f, -4035.97827f, 9.370208f), new Vector3(0f, -0f, 126.050072f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			})
		};

		public List<CayoPericoSpawnGroup> SpawnGroups = new List<CayoPericoSpawnGroup>();

		public static bool HeistOn;

		public static List<SamSite> SamSites = new List<SamSite>();

		public static CargoShipStage HeistStage = CargoShipStage.Scout;

		public static Vector3 CurrentContainer;

		public static bool CreatedKosatkaInterior;

		public static bool CreatedLootPlusMain;

		public static bool GotC4Crate = false;

		public static bool GotThermalCharges = false;

		public static bool GotGrinder;

		public static bool GotManifest = false;

		public static Blip ManifestContainerBlip;

		public static bool GotJammer;

		public static bool GotSavage;

		public static bool GotPanthereStatue;

		public static ManageRobberyAnims.Lootable PathereStatueLoot;

		public static Prop Grinder;

		public static Vehicle Savage;

		public static Prop C4Crate;

		public static bool CargoshipHeistOn = false;

		public static Vector3 CargoShipPos = new Vector3(-361.5388f, -4088.099f, 15.38693f);

		public static Vector3 CayoPericoPos = new Vector3(5103.374f, -5172.456f, 18f);

		public static bool GotJuggKeyCard;

		public static Prop Keycard;

		public static Ped Juggernaut;

		public static int ThermalChargesCollected;

		public static int C4ChargesCollected;

		public static List<Container> Containers = new List<Container>();

		public static List<Prop> ContainersProps = new List<Prop>();

		public static int HeistMinPay = 400000;

		public static int HeistMaxPay = 2000000;

		public static int CurrentPay = 0;

		public static int LocalTake = 0;

		public static MissionOjects CargoHeist_Main = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_container_01a"), 172563, 3230155168u, new Vector3(-343.5477f, -4085.04175f, 11.1925449f), new Vector3(0f, -0f, 129.4484f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_container_01a"), 444952, 3230155168u, new Vector3(-358.574738f, -4097.704f, 13.9970036f), new Vector3(-4.26886515E-07f, -3.81666541E-14f, -50.0427971f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_container_01a"), 23571, 3230155168u, new Vector3(-355.754181f, -4095.32422f, 13.9970016f), new Vector3(0f, -0f, 130.365784f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_container_01a"), 764945, 3230155168u, new Vector3(-350.063049f, -4076.69922f, 11.1925449f), new Vector3(0f, -0f, 130.1106f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_container_01a"), 625170, 3230155168u, new Vector3(-352.8651f, -4079.06641f, 11.1925449f), new Vector3(0f, 0f, -50.25338f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_container_01a"), 759056, 3230155168u, new Vector3(-369.073761f, -4085.91724f, 13.9970036f), new Vector3(0f, -0f, 130.153885f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_container_01a"), 272664, 3230155168u, new Vector3(-361.868f, -4086.67432f, 13.9970036f), new Vector3(0f, 0f, -49.9015541f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_container_01a"), 632591, 3230155168u, new Vector3(-337.5811f, -4066.22778f, 13.9847317f), new Vector3(0f, -0f, 129.997269f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_container_01a"), 700694, 3230155168u, new Vector3(-340.4165f, -4068.61816f, 13.9847317f), new Vector3(0f, 0f, -49.5933571f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_container_01a"), 420121, 3230155168u, new Vector3(-340.924316f, -4065.61157f, 13.9847307f), new Vector3(0f, -0f, 130.559052f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_container_01a"), 157716, 3230155168u, new Vector3(-356.768646f, -4068.552f, 13.9847317f), new Vector3(0f, -0f, 129.481918f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_container_01a"), 671509, 3230155168u, new Vector3(-389.452728f, -4106.292f, 16.8333035f), new Vector3(0f, 0f, -49.60017f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_container_01a"), 678664, 3230155168u, new Vector3(-386.6497f, -4103.89746f, 16.8333015f), new Vector3(0f, -0f, 130.199524f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_container_01a"), 574486, 3230155168u, new Vector3(-309.931549f, -4034.33057f, 13.2960625f), new Vector3(0f, -0f, 130.047241f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_container_01a"), 561174, 3230155168u, new Vector3(-299.312225f, -4046.96753f, 13.2960625f), new Vector3(0f, -0f, 129.995972f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_container_01a"), 526607, 3230155168u, new Vector3(-327.823822f, -4066.92f, 8.31877f), new Vector3(0f, -0f, -139.976868f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_container_01a"), 382487, 3230155168u, new Vector3(-335.04f, -4058.91333f, 8.318769f), new Vector3(0f, 0f, 40.6415443f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_container_01a"), 550677, 3230155168u, new Vector3(-381.699219f, -4113.58447f, 16.8333035f), new Vector3(0f, 0f, -50.3108139f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_container_01a"), 196118, 3230155168u, new Vector3(-417.900757f, -4130.64258f, 16.3359871f), new Vector3(0f, -0f, 130.918213f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_container_01a"), 506645, 3230155168u, new Vector3(-412.9292f, -4136.38135f, 16.3359871f), new Vector3(0f, -0f, 130.9182f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_container_01a"), 365843, 3230155168u, new Vector3(-392.101837f, -4098.276f, 13.9970036f), new Vector3(0f, -0f, 129.998184f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_container_01a"), 689934, 3230155168u, new Vector3(-394.941559f, -4100.64453f, 13.9970036f), new Vector3(0f, 0f, -50.11113f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_sam_turret_01"), 411923, 3711724380u, new Vector3(-305.195526f, -4041.4397f, 13.2960615f), new Vector3(0f, -0f, -139.824982f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_sam_turret_01"), 506898, 3711724380u, new Vector3(-372.299652f, -4101.69f, 11.1903248f), new Vector3(0f, -0f, -140.181122f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_sam_turret_01"), 590870, 3711724380u, new Vector3(-403.432434f, -4111.51025f, 25.46056f), new Vector3(0f, -0f, -139.207611f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_sam_turret_01"), 498712, 3711724380u, new Vector3(-392.004944f, -4125.6167f, 25.4605446f), new Vector3(0f, 0f, -48.6679077f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 602384, 304964818u, new Vector3(-344.385437f, -4085.77f, 11.3823509f), new Vector3(0f, -0f, 128.773941f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 695307, 304964818u, new Vector3(-356.664429f, -4096.106f, 14.1868076f), new Vector3(0f, -0f, 131.176788f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 650773, 304964818u, new Vector3(-357.66922f, -4096.89746f, 14.18681f), new Vector3(0f, 0f, -51.1294022f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 627472, 304964818u, new Vector3(-361.024658f, -4085.97681f, 14.18681f), new Vector3(0f, 0f, -49.5418777f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 438800, 304964818u, new Vector3(-369.882172f, -4086.57031f, 14.18681f), new Vector3(0f, -0f, 130.760147f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 357136, 304964818u, new Vector3(-387.591125f, -4104.697f, 17.02311f), new Vector3(0f, -0f, 130.604965f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 125962, 304964818u, new Vector3(-380.872162f, -4112.868f, 17.0231113f), new Vector3(0f, 0f, -50.1158257f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 209162, 304964818u, new Vector3(-351.93515f, -4078.296f, 11.3823509f), new Vector3(0f, 0f, -49.4491463f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 675087, 304964818u, new Vector3(-351.005219f, -4077.4856f, 11.3823509f), new Vector3(0f, -0f, 131.106888f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 435472, 304964818u, new Vector3(-341.7108f, -4066.38062f, 14.1745367f), new Vector3(0f, -0f, 129.363464f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 764175, 304964818u, new Vector3(-338.5043f, -4067.02344f, 14.1745377f), new Vector3(0f, -0f, 128.899658f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 684816, 304964818u, new Vector3(-339.493561f, -4067.82861f, 14.1745377f), new Vector3(0f, 0f, -48.7414856f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 584208, 304964818u, new Vector3(-300.249817f, -4047.77832f, 13.4858685f), new Vector3(0f, -0f, 131.0448f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 174608, 304964818u, new Vector3(-327.283051f, -4067.59473f, 8.608577f), new Vector3(0f, 0f, 40.4008179f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 552208, 304964818u, new Vector3(-335.90094f, -4057.99341f, 8.508575f), new Vector3(0f, 0f, 40.6049f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 658447, 304964818u, new Vector3(-357.604218f, -4069.22119f, 14.1745377f), new Vector3(0f, -0f, 130.37323f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 353296, 304964818u, new Vector3(-393.014954f, -4099.09326f, 14.18681f), new Vector3(0f, -0f, 128.834183f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 781839, 304964818u, new Vector3(-394.041931f, -4099.867f, 14.18681f), new Vector3(0f, 0f, -48.9094162f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 350224, 304964818u, new Vector3(-418.749329f, -4131.443f, 16.525795f), new Vector3(0f, -0f, 129.814835f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 631822, 304964818u, new Vector3(-413.8382f, -4137.246f, 16.525795f), new Vector3(0f, -0f, 131.246811f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 359191, 304964818u, new Vector3(-388.497864f, -4105.50537f, 17.0231113f), new Vector3(0f, 0f, -50.0870056f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 359191, 304964818u, new Vector3(-310.74f, -4035.03f, 13.48f), new Vector3(0f, 0f, 130.46f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_crate_03a"), 331801, 3235515268u, new Vector3(-375.5952f, -4084.72681f, 10.9955616f), new Vector3(0f, 0f, -49.756916f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_crate_03a"), 19467, 3235515268u, new Vector3(-360.361725f, -4102.87158f, 10.9955616f), new Vector3(0f, 0f, -49.0787125f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_crate_03a"), 649748, 3235515268u, new Vector3(-345.367767f, -4086.52954f, 11.1925449f), new Vector3(0f, 0f, -50.40919f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_crate_03a"), 597278, 3235515268u, new Vector3(-358.4721f, -4070.99683f, 13.9847307f), new Vector3(0f, -0f, -140.126373f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_crate_03a"), 177950, 3235515268u, new Vector3(-350.6448f, -4063.42f, 10.9953794f), new Vector3(0f, 0f, -50.1072845f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_crate_03a"), 675871, 3235515268u, new Vector3(-335.255066f, -4081.74023f, 10.9953794f), new Vector3(0f, 0f, -50.05607f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_crate_03a"), 481570, 3235515268u, new Vector3(-372.452545f, -4112.76074f, 10.9955606f), new Vector3(0f, -0f, 129.998062f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_crate_03a"), 17699, 3235515268u, new Vector3(-387.723541f, -4094.55225f, 10.99556f), new Vector3(0f, -0f, 129.998062f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_metal_plates01"), 494373, 2067252279u, new Vector3(-346.700043f, -4070.76563f, 13.9431858f), new Vector3(0f, 0f, -55.03315f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_metal_plates01"), 749089, 2067252279u, new Vector3(-364.598f, -4075.21216f, 13.9422283f), new Vector3(0f, 0f, -52.17309f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "port_xr_stairs_01"), 14610, 3774217136u, new Vector3(-330.415527f, -4082.27246f, 1.48857355f), new Vector3(0f, 0f, 39.9709358f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "port_xr_stairs_01"), 156456, 3774217136u, new Vector3(-390.9712f, -4133.163f, 0.9885738f), new Vector3(0f, 0f, 39.9709358f), 14610, 0, new Vector3(-79.1000061f, -0.100000292f, -0.4999998f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "port_xr_stairs_01"), 296235, 3774217136u, new Vector3(-334.783844f, -4085.934f, -2.51142478f), new Vector3(0f, 0f, 39.9709358f), 14610, 0, new Vector3(-5.699997f, 0f, -3.99999833f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "port_xr_stairs_01"), 595749, 3774217136u, new Vector3(-395.3395f, -4136.82471f, -3.01142478f), new Vector3(0f, 0f, 39.9709358f), 156456, 0, new Vector3(-5.699997f, 0f, -3.99999857f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_metal_plates01"), 594473, 2067252279u, new Vector3(-394.201752f, -4110.10059f, 16.7560253f), new Vector3(0f, -0f, 126.8325f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects CargoHeist_Loot = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 747323, 2151061176u, new Vector3(4962.60156f, -5106.743f, 2.91721463f), new Vector3(0f, 0f, -25.297226f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 681796, 2151061176u, new Vector3(4963.88232f, -5109.248f, 2.91721463f), new Vector3(0f, -0f, -114.965721f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 681018, 2151061176u, new Vector3(5001.333f, -5165.45557f, 2.70359659f), new Vector3(0f, 0f, -63.118145f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 35453, 2151061176u, new Vector3(4924.3584f, -5245.99f, 2.46651363f), new Vector3(0f, -0f, -147.076416f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 637248, 2151061176u, new Vector3(4926.34f, -5244.60254f, 2.46651363f), new Vector3(0f, -0f, -146.256561f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 142129, 2151061176u, new Vector3(4506.09961f, -4555.50244f, 4.10114f), new Vector3(0f, -0f, -162.545837f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 395839, 2151061176u, new Vector3(4503.701f, -4556.327f, 4.10114f), new Vector3(0f, -0f, -155.84082f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 746590, 2151061176u, new Vector3(4438.2876f, -4445.719f, 4.27232265f), new Vector3(0f, 0f, -70.1815f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 727878, 2151061176u, new Vector3(4435.646f, -4446.69f, 4.25600767f), new Vector3(0f, -0f, 110.589806f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 431433, 2151061176u, new Vector3(-300.197021f, -4047.62183f, 14.3277264f), new Vector3(0f, -0f, 130.703812f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 95556, 2151061176u, new Vector3(-310.7876f, -4034.99927f, 14.3277264f), new Vector3(0f, -0f, 132.647751f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 200521, 2151061176u, new Vector3(-335.801331f, -4058.09814f, 9.350433f), new Vector3(0f, 0f, 39.4970055f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 132933, 2151061176u, new Vector3(-338.384827f, -4066.9375f, 15.0163956f), new Vector3(0f, -0f, 127.680984f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 504380, 2151061176u, new Vector3(-339.614441f, -4067.9856f, 15.0163956f), new Vector3(0f, 0f, -48.43298f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 309057, 2151061176u, new Vector3(-341.638153f, -4066.30518f, 15.0163946f), new Vector3(0f, -0f, 130.641739f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 227647, 2151061176u, new Vector3(-357.553223f, -4069.17358f, 15.0163956f), new Vector3(0f, -0f, 129.207413f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 718661, 2151061176u, new Vector3(-369.7834f, -4086.44946f, 15.0286674f), new Vector3(0f, -0f, 130.127411f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 340253, 2151061176u, new Vector3(-356.638275f, -4096.065f, 15.0286655f), new Vector3(0f, -0f, 132.736786f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 85801, 2151061176u, new Vector3(-357.7913f, -4097.007f, 15.0286674f), new Vector3(0f, 0f, -51.8733253f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 480294, 2151061176u, new Vector3(-360.975281f, -4086.008f, 15.0286674f), new Vector3(0f, 0f, -50.7983322f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 323907, 2151061176u, new Vector3(-392.9904f, -4098.91064f, 15.0286674f), new Vector3(0f, -0f, 131.0898f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 140608, 2151061176u, new Vector3(-387.507141f, -4104.55273f, 17.8649673f), new Vector3(0f, -0f, 132.9306f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 720925, 2151061176u, new Vector3(-380.904846f, -4112.914f, 17.86497f), new Vector3(0f, 0f, -49.7351379f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 205377, 2151061176u, new Vector3(-388.5947f, -4105.614f, 17.86497f), new Vector3(0f, 0f, -49.6318321f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 530500, 2151061176u, new Vector3(-394.107361f, -4099.90771f, 15.0286674f), new Vector3(0f, 0f, -48.7102547f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 408605, 2151061176u, new Vector3(-418.665436f, -4131.32959f, 17.3676529f), new Vector3(0f, -0f, 129.492874f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 303173, 2151061176u, new Vector3(-413.6996f, -4137.17725f, 17.3676529f), new Vector3(0f, -0f, 130.10614f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 551450, 2151061176u, new Vector3(-350.905975f, -4077.32227f, 12.2242088f), new Vector3(0f, -0f, 132.4494f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 244537, 2151061176u, new Vector3(-352.026f, -4078.503f, 12.2242088f), new Vector3(0f, 0f, -46.5226822f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 58393, 2151061176u, new Vector3(-344.2096f, -4085.713f, 12.2242088f), new Vector3(0f, -0f, 127.995461f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 347465, 2151061176u, new Vector3(-327.3406f, -4067.53931f, 9.450435f), new Vector3(0f, -0f, -138.693451f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects CargoHeist_Loot_ShipOnly = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 431433, 2151061176u, new Vector3(-300.197021f, -4047.62183f, 14.3277264f), new Vector3(0f, -0f, 130.703812f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 95556, 2151061176u, new Vector3(-310.7876f, -4034.99927f, 14.3277264f), new Vector3(0f, -0f, 132.647751f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 200521, 2151061176u, new Vector3(-335.801331f, -4058.09814f, 9.350433f), new Vector3(0f, 0f, 39.4970055f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 132933, 2151061176u, new Vector3(-338.384827f, -4066.9375f, 15.0163956f), new Vector3(0f, -0f, 127.680984f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 504380, 2151061176u, new Vector3(-339.614441f, -4067.9856f, 15.0163956f), new Vector3(0f, 0f, -48.43298f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 309057, 2151061176u, new Vector3(-341.638153f, -4066.30518f, 15.0163946f), new Vector3(0f, -0f, 130.641739f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 227647, 2151061176u, new Vector3(-357.553223f, -4069.17358f, 15.0163956f), new Vector3(0f, -0f, 129.207413f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 718661, 2151061176u, new Vector3(-369.7834f, -4086.44946f, 15.0286674f), new Vector3(0f, -0f, 130.127411f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 340253, 2151061176u, new Vector3(-356.638275f, -4096.065f, 15.0286655f), new Vector3(0f, -0f, 132.736786f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 85801, 2151061176u, new Vector3(-357.7913f, -4097.007f, 15.0286674f), new Vector3(0f, 0f, -51.8733253f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 480294, 2151061176u, new Vector3(-360.975281f, -4086.008f, 15.0286674f), new Vector3(0f, 0f, -50.7983322f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 323907, 2151061176u, new Vector3(-392.9904f, -4098.91064f, 15.0286674f), new Vector3(0f, -0f, 131.0898f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 140608, 2151061176u, new Vector3(-387.507141f, -4104.55273f, 17.8649673f), new Vector3(0f, -0f, 132.9306f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 720925, 2151061176u, new Vector3(-380.904846f, -4112.914f, 17.86497f), new Vector3(0f, 0f, -49.7351379f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 205377, 2151061176u, new Vector3(-388.5947f, -4105.614f, 17.86497f), new Vector3(0f, 0f, -49.6318321f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 530500, 2151061176u, new Vector3(-394.107361f, -4099.90771f, 15.0286674f), new Vector3(0f, 0f, -48.7102547f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 408605, 2151061176u, new Vector3(-418.665436f, -4131.32959f, 17.3676529f), new Vector3(0f, -0f, 129.492874f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 303173, 2151061176u, new Vector3(-413.6996f, -4137.17725f, 17.3676529f), new Vector3(0f, -0f, 130.10614f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 551450, 2151061176u, new Vector3(-350.905975f, -4077.32227f, 12.2242088f), new Vector3(0f, -0f, 132.4494f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 244537, 2151061176u, new Vector3(-352.026f, -4078.503f, 12.2242088f), new Vector3(0f, 0f, -46.5226822f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 58393, 2151061176u, new Vector3(-344.2096f, -4085.713f, 12.2242088f), new Vector3(0f, -0f, 127.995461f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"), 347465, 2151061176u, new Vector3(-327.3406f, -4067.53931f, 9.450435f), new Vector3(0f, -0f, -138.693451f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects CargoHeist_JuggernautLootSpawns = new MissionOjects(new List<ObjectPlacementMap>());

		public static MissionOjects CargoHeist_Manifest_Jammer = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_clipboard_sh_01a"), 448370, 2896824542u, new Vector3(5000.405f, -5163.70654f, 2.718029f), new Vector3(0f, 0f, -47.5053444f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_jammer_01a"), 483338, 3188649882u, new Vector3(5067.588f, -4590.59229f, 2.79774213f), new Vector3(0f, 0f, -18.9135437f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects CargoHeist_Savage = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "savage"), 242546, 4212341271u, new Vector3(1485.329f, 1164.12793f, 115.582306f), new Vector3(0.658594847f, -1.00712025f, -0.376320273f), 154, 24, 13, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Merryweather Mesa"), 245095, 2230595153u, new Vector3(1476.409f, 1173.59753f, 114.107651f), new Vector3(0.174871147f, 0.002235383f, 57.0650673f), 152, 153, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Merryweather Mesa"), 23078, 2230595153u, new Vector3(1480.373f, 1177.71521f, 114.102615f), new Vector3(-4.49022675f, -4.89623356f, -27.735548f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 616052, 1821116645u, new Vector3(1480.98828f, 1170.44067f, 114.820107f), new Vector3(0f, 0f, 54.3009148f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 671840, 2127932792u, new Vector3(1481.97156f, 1177.35254f, 114.348274f), new Vector3(0f, -0f, 153.8473f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 534638, 2127932792u, new Vector3(1482.44507f, 1175.93689f, 114.742569f), new Vector3(0f, 0f, 5.3195734f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_container_01a"), 175222, 2202902697u, new Vector3(1497.88928f, 1163.0116f, 113.07444f), new Vector3(0f, -0f, -176.622131f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mb_hesco_06"), 594807, 1354899844u, new Vector3(1496.35681f, 1169.276f, 113.166214f), new Vector3(0f, -0f, -177.695755f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 421165, 2127932792u, new Vector3(1497.02063f, 1167.80664f, 116.771332f), new Vector3(0f, -0f, -162.4426f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_carrier_cargo_03a"), 427128, 102012783u, new Vector3(1477.16333f, 1160.52161f, 113.274208f), new Vector3(0f, -0f, 178.313385f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_carrier_cargo_04a"), 546938, 2585086902u, new Vector3(1474.83228f, 1160.66663f, 113.27951f), new Vector3(0f, -0f, 178.108566f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_carrier_cargo_04c"), 322422, 1056511355u, new Vector3(1496.67566f, 1173.72632f, 113.334061f), new Vector3(0f, -0f, -178.826691f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_carrier_cargo_04c"), 784250, 1056511355u, new Vector3(1493.706f, 1173.66638f, 113.334816f), new Vector3(0f, -0f, -178.826691f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_carrier_cargo_05b_s"), 91515, 2266496104u, new Vector3(1492.23828f, 1154.01782f, 113.246666f), new Vector3(0f, -0f, 91.65708f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_cargo_01a"), 418170, 3395017374u, new Vector3(1494.274f, 1156.70959f, 113.120445f), new Vector3(0f, -0f, 91.55456f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_1_prop_m31_cargo_03a"), 618102, 4148572722u, new Vector3(1481.29724f, 1151.53625f, 113.344162f), new Vector3(0f, -0f, 91.14491f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 626556, 1821116645u, new Vector3(1496.141f, 1169.13818f, 114.164124f), new Vector3(0f, 0f, -76.882515f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 106609, 1821116645u, new Vector3(1492.32947f, 1158.33789f, 114.211807f), new Vector3(0f, -0f, -171.413635f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 295805, 1821116645u, new Vector3(1480.257f, 1155.22253f, 114.296349f), new Vector3(0f, -0f, -138.913712f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 230774, 1821116645u, new Vector3(1477.74524f, 1163.74585f, 114.319626f), new Vector3(0f, -0f, 140.987411f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 310141, 1821116645u, new Vector3(1476.59338f, 1164.13892f, 114.325005f), new Vector3(0f, -0f, -154.114761f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static Vector3 SavageIntelLoc = new Vector3(1485.329f, 1164.12793f, 115.582306f);

		public static bool CreatedSavageIntel;

		public static MissionOjects CargoHeist_Grinder = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_grinder_01a"), 681021, 2271741578u, new Vector3(-569.7184f, -1671.93567f, 19.386652f), new Vector3(0f, -0f, 104.321281f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_bag_grinder_01a"), 613380, 339736694u, new Vector3(-570.233765f, -1671.7334f, 19.2932148f), new Vector3(0f, -0f, -114.162651f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 223818, 1821116645u, new Vector3(-563.78125f, -1666.2616f, 19.2046432f), new Vector3(0f, -0f, -93.9458f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 502352, 2127932792u, new Vector3(-590.1917f, -1657.42981f, 19.4620533f), new Vector3(0f, 0f, 22.542408f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 538183, 2127932792u, new Vector3(-593.518555f, -1655.29626f, 20.62847f), new Vector3(0f, -0f, -131.256485f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 219193, 1821116645u, new Vector3(-601.8271f, -1669.17371f, 19.6106663f), new Vector3(0f, -0f, 110.054543f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 156244, 1821116645u, new Vector3(-594.639465f, -1676.93628f, 19.4277668f), new Vector3(0f, -0f, 176.589661f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 403023, 1821116645u, new Vector3(-595.032654f, -1678.022f, 19.4276466f), new Vector3(0f, 0f, 26.1680679f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 589649, 1821116645u, new Vector3(-570.4137f, -1670.33252f, 19.206234f), new Vector3(0f, -0f, -153.272217f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 738103, 1821116645u, new Vector3(-576.493958f, -1680.68677f, 19.2278023f), new Vector3(0f, -0f, 145.617432f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 419441, 2127932792u, new Vector3(-577.3451f, -1680.17493f, 19.22312f), new Vector3(0f, -0f, -175.68988f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_coke_pallet_01a"), 74798, 3854390996u, new Vector3(-573.095764f, -1677.80908f, 18.6459541f), new Vector3(0f, -0f, -151.685577f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_coke_pallet_01a"), 657229, 3854390996u, new Vector3(-575.0542f, -1679.04419f, 18.4070759f), new Vector3(0f, -0f, -151.685577f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_coke_pallet_01a"), 191316, 3854390996u, new Vector3(-563.334961f, -1667.72913f, 18.2126522f), new Vector3(0f, 0f, -60.9404831f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_coke_pallet_01a"), 606276, 3854390996u, new Vector3(-561.9716f, -1670.16968f, 18.2230759f), new Vector3(0f, 0f, -71.22774f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_coke_pallet_01a"), 181072, 3854390996u, new Vector3(-564.1101f, -1671.33679f, 18.2636337f), new Vector3(0f, -0f, -142.573792f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_coke_pallet_01a"), 564052, 3854390996u, new Vector3(-574.1989f, -1659.32568f, 18.7179451f), new Vector3(0f, 0f, -36.0144157f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_coke_pallet_01a"), 110086, 3854390996u, new Vector3(-571.7654f, -1660.468f, 18.336256f), new Vector3(0f, 0f, -36.0144157f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_coke_pallet_01a"), 247885, 3854390996u, new Vector3(-583.371948f, -1671.20789f, 18.2958317f), new Vector3(0f, -0f, 154.6856f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_boxcoke_01"), 312394, 3348973628u, new Vector3(-583.003662f, -1671.4762f, 18.5499763f), new Vector3(0f, -0f, -110.628319f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_boxcoke_01"), 58706, 3348973628u, new Vector3(-583.691f, -1671.02966f, 18.5499763f), new Vector3(0f, -0f, -120.352478f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_boxcoke_01"), 637524, 3348973628u, new Vector3(-563.019043f, -1668.11353f, 18.4667969f), new Vector3(0f, -0f, -153.772827f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_boxcoke_01"), 327278, 3348973628u, new Vector3(-563.4125f, -1667.30908f, 18.4667969f), new Vector3(0f, -0f, -153.516586f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_boxcoke_01"), 536626, 3348973628u, new Vector3(-561.8355f, -1670.71179f, 18.47722f), new Vector3(0f, -0f, -164.36615f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_boxcoke_01"), 750670, 3348973628u, new Vector3(-562.030334f, -1669.882f, 18.47722f), new Vector3(0f, -0f, -162.369919f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_boxcoke_01"), 65856, 3348973628u, new Vector3(-564.4616f, -1671.57593f, 18.5177784f), new Vector3(0f, -0f, 122.5471f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_boxcoke_01"), 688713, 3348973628u, new Vector3(-563.789063f, -1671.05933f, 18.5177784f), new Vector3(0f, -0f, 132.015411f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_boxcoke_01"), 51787, 3348973628u, new Vector3(-575.377f, -1679.22314f, 18.66122f), new Vector3(0f, -0f, 126.896332f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_boxcoke_01"), 63567, 3348973628u, new Vector3(-574.679565f, -1678.83386f, 18.66122f), new Vector3(0f, -0f, 129.762283f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_boxcoke_01"), 304458, 3348973628u, new Vector3(-573.4962f, -1678.07715f, 18.9000988f), new Vector3(0f, -0f, 118.297424f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_boxcoke_01"), 234830, 3348973628u, new Vector3(-572.850037f, -1677.6593f, 18.9000988f), new Vector3(0f, -0f, 116.147636f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_boxcoke_01"), 723542, 3348973628u, new Vector3(-572.0446f, -1660.28052f, 18.5904f), new Vector3(0f, 0f, 47.66715f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_boxcoke_01"), 778305, 3348973628u, new Vector3(-571.413452f, -1660.72852f, 18.5904f), new Vector3(0f, 0f, 54.8324242f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_boxcoke_01"), 286274, 3348973628u, new Vector3(-573.836f, -1659.69006f, 18.97209f), new Vector3(0f, -0f, -126.245865f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_boxcoke_01"), 608575, 3348973628u, new Vector3(-574.5116f, -1659.1864f, 18.97209f), new Vector3(0f, 0f, 49.2028923f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Merryweather Mesa"), 293189, 2230595153u, new Vector3(-579.6752f, -1671.54443f, 19.0176983f), new Vector3(-0.297895342f, -0.5228741f, 152.896912f), 152, 153, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Merryweather Mesa"), 301645, 2230595153u, new Vector3(-585.10675f, -1659.147f, 19.125349f), new Vector3(0.6017133f, 0.34855172f, -108.762756f), 12, 12, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Merryweather Mesa"), 550234, 2230595153u, new Vector3(-586.052368f, -1661.98474f, 19.1509933f), new Vector3(-0.913756967f, 1.853574f, -108.735039f), 154, 153, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static Vector3 GrinderIntelLoc = new Vector3(-601.8271f, -1669.17371f, 19.6106663f);

		public static bool CreatedGrinderIntel;

		public static MissionOjects CargoHeist_Thermite = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 568877, 2127932792u, new Vector3(-52.59339f, -2756.30054f, 6.07675028f), new Vector3(0f, -0f, 112.443085f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 709678, 2127932792u, new Vector3(-69.08759f, -2779.223f, 6.076495f), new Vector3(0f, -0f, -166.588623f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 183097, 2127932792u, new Vector3(-43.2833633f, -2728.41138f, 6.142561f), new Vector3(0f, 0f, 42.6309052f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 152888, 2127932792u, new Vector3(-65.91942f, -2699.93f, 6.20182037f), new Vector3(0f, 0f, 49.28079f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 335674, 1821116645u, new Vector3(-31.37199f, -2698.22974f, 6.10744667f), new Vector3(0f, -0f, -92.43917f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 409145, 1821116645u, new Vector3(-36.5000076f, -2728.28345f, 11.38953f), new Vector3(0f, 0f, -41.8203163f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 193853, 1821116645u, new Vector3(-52.7878532f, -2722.92432f, 11.4919958f), new Vector3(0f, 0f, 79.58085f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 434748, 2127932792u, new Vector3(-52.47597f, -2748.2793f, 11.4826632f), new Vector3(0f, -0f, -146.426376f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 738364, 2127932792u, new Vector3(-36.4588242f, -2744.69482f, 6.14364147f), new Vector3(0f, -0f, -169.968765f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 287806, 1821116645u, new Vector3(-79.78217f, -2729.37866f, 6.64672327f), new Vector3(0f, -0f, -103.850975f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 250157, 1821116645u, new Vector3(-79.48712f, -2705.00659f, 6.891923f), new Vector3(0f, 0f, -20.57919f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 139579, 1821116645u, new Vector3(-93.1748657f, -2713.6355f, 6.01682663f), new Vector3(0f, 0f, 54.5544853f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 318269, 2127932792u, new Vector3(-69.34264f, -2701.859f, 12.0229139f), new Vector3(0f, 0f, 40.6329f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 692283, 1821116645u, new Vector3(-68.47706f, -2701.09131f, 12.0129576f), new Vector3(0f, 0f, 56.6012573f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 706106, 1821116645u, new Vector3(-27.5745964f, -2715.48364f, 10.042285f), new Vector3(0f, 0f, -43.76445f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static bool CreatedThermiteIntel;

		public static MissionOjects CargoHeist_ThermitePickups = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 751619, 865563579u, new Vector3(-59.86373f, -2705.90063f, 6.07325363f), new Vector3(0f, -0f, 149.4962f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 232452, 865563579u, new Vector3(-60.0465927f, -2705.882f, 6.07325363f), new Vector3(0f, -0f, 158.247162f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 135684, 865563579u, new Vector3(-59.8711739f, -2706.09619f, 6.07325363f), new Vector3(0f, -0f, 110.903389f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 400133, 865563579u, new Vector3(-44.095623f, -2719.2832f, 6.17798853f), new Vector3(0f, -0f, 101.887733f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 127493, 865563579u, new Vector3(-44.173378f, -2719.14673f, 6.17807531f), new Vector3(0f, -0f, 94.61981f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 206853, 865563579u, new Vector3(-44.0454521f, -2719.02124f, 6.17798853f), new Vector3(0f, 0f, 88.3243256f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 772613, 865563579u, new Vector3(-43.39607f, -2727.39014f, 6.340384f), new Vector3(0f, -0f, 140.323151f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 190214, 865563579u, new Vector3(-43.5449371f, -2727.40454f, 6.340384f), new Vector3(0f, -0f, 136.177292f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 58886, 865563579u, new Vector3(-43.42973f, -2727.21265f, 6.34038448f), new Vector3(0f, -0f, 100.706779f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 462342, 865563579u, new Vector3(-49.6746254f, -2745.329f, 5.989128f), new Vector3(0f, -0f, 136.378372f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 361479, 865563579u, new Vector3(-49.6195869f, -2745.44873f, 5.989128f), new Vector3(0f, -0f, 138.270645f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 526087, 865563579u, new Vector3(-28.378521f, -2710.285f, 6.07325363f), new Vector3(0f, -0f, 160.013382f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 538117, 865563579u, new Vector3(-28.2417965f, -2710.23071f, 6.07325363f), new Vector3(0f, -0f, 162.776917f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 247560, 865563579u, new Vector3(-28.423008f, -2710.10864f, 6.07325363f), new Vector3(0f, -0f, 121.882179f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 165641, 865563579u, new Vector3(-40.77237f, -2734.933f, 6.106081f), new Vector3(0f, -0f, -109.139786f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 20745, 865563579u, new Vector3(-40.73447f, -2735.06323f, 6.106081f), new Vector3(0f, 0f, -76.7928162f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 747274, 865563579u, new Vector3(5002.16748f, -5190.88672f, 2.69746f), new Vector3(0f, -0f, 137.040451f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 40459, 865563579u, new Vector3(5002.04f, -5190.84131f, 2.69627357f), new Vector3(0f, -0f, 129.3631f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 126475, 865563579u, new Vector3(5002.06934f, -5190.654f, 2.69916368f), new Vector3(0f, -0f, 120.201477f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 452619, 865563579u, new Vector3(4885.98975f, -5192.448f, 2.55626917f), new Vector3(0f, -0f, 92.45951f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 710411, 865563579u, new Vector3(4886.05029f, -5192.329f, 2.55626917f), new Vector3(0f, 0f, 85.2939758f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 285194, 865563579u, new Vector3(5159.339f, -4693.526f, 3.038757f), new Vector3(0f, -0f, 137.921982f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 578315, 865563579u, new Vector3(5159.225f, -4693.5127f, 3.038757f), new Vector3(0f, -0f, 152.456955f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 304144, 865563579u, new Vector3(5175.096f, -4650.57666f, 2.41779113f), new Vector3(0f, -0f, -154.628815f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 465939, 865563579u, new Vector3(5074.6543f, -4598.97852f, 2.51662755f), new Vector3(0f, 0f, 38.3260841f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 483603, 865563579u, new Vector3(5074.69775f, -4598.842f, 2.51647782f), new Vector3(0f, 0f, 33.82204f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 562193, 865563579u, new Vector3(5074.86f, -4598.97754f, 2.52149987f), new Vector3(0f, 0f, -29.5401745f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 375573, 865563579u, new Vector3(5035.10059f, -4631.1f, 4.56701326f), new Vector3(0f, -0f, 103.784248f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 162069, 865563579u, new Vector3(5035.014f, -4631.007f, 4.56701326f), new Vector3(0f, -0f, 100.406067f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_thermite"), 246038, 865563579u, new Vector3(5035.15234f, -4630.881f, 4.56701326f), new Vector3(0f, 0f, 50.60657f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects CargoHeist_KosatkaC4 = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_supp_01a"), 480292, 1195735753u, new Vector3(1557.64063f, 415.4366f, -50.6644058f), new Vector3(0f, 0f, 89.38417f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_supp_01a"), 218167, 1195735753u, new Vector3(1561.06384f, 439.733032f, -53.8630371f), new Vector3(0f, 0f, 0.686893f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_supp_01a"), 289834, 1195735753u, new Vector3(1564.57642f, 384.037476f, -50.6816177f), new Vector3(0f, 0f, -88.6728745f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_supp_01a"), 134665, 1195735753u, new Vector3(1561.45679f, 364.747437f, -57.0863f), new Vector3(0f, -0f, -179.569031f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_supp_01a"), 192027, 1195735753u, new Vector3(1559.52429f, 381.55603f, -57.0848351f), new Vector3(0f, -0f, 91.53827f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_supp_01a"), 168757, 1195735753u, new Vector3(1560.87329f, 391.0197f, -57.08533f), new Vector3(0f, -0f, -179.458954f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_supp_01a"), 354353, 1195735753u, new Vector3(1557.433f, 420.321747f, -55.17366f), new Vector3(0f, -0f, 90.71743f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_supp_01a"), 725303, 1195735753u, new Vector3(1557.9f, 425.5461f, -50.85586f), new Vector3(0f, -0f, 90.51133f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_supp_01a"), 35634, 1195735753u, new Vector3(1558.90393f, 397.6782f, -50.6839943f), new Vector3(0f, 0f, -0.8987972f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static MissionOjects CargoHeist_KosatkaInterior = new MissionOjects(new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 574769, 1821116645u, new Vector3(1559.86548f, 398.027557f, -49.6713371f), new Vector3(0f, -0f, -170.715576f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 342072, 1821116645u, new Vector3(1559.994f, 416.9055f, -49.65338f), new Vector3(0f, -0f, -140.415421f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 231989, 2127932792u, new Vector3(1559.47961f, 415.789825f, -49.6524734f), new Vector3(0f, 0f, -79.76557f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 525883, 2127932792u, new Vector3(1558.85278f, 410.356445f, -54.161705f), new Vector3(0f, 0f, -25.0010242f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 568894, 2127932792u, new Vector3(1565.5321f, 424.239532f, -53.2813072f), new Vector3(0f, -0f, 179.999969f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 226873, 2127932792u, new Vector3(1562.893f, 425.683624f, -53.28313f), new Vector3(-0.0103325648f, 0.009930464f, -136.13678f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 166209, 2127932792u, new Vector3(1561.67871f, 438.0884f, -52.88614f), new Vector3(0.04039802f, -0.1430026f, 164.193115f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 307511, 2127932792u, new Vector3(1560.49316f, 425.5495f, -56.0924149f), new Vector3(0f, -0f, 99.03827f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 451643, 2127932792u, new Vector3(1564.38184f, 420.2668f, -54.16064f), new Vector3(0f, 0f, 34.5489273f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard"), 241678, 2127932792u, new Vector3(1562.67383f, 404.950623f, -56.12289f), new Vector3(0.0818319246f, -0.000300364976f, -179.789688f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 241180, 1821116645u, new Vector3(1562.00745f, 393.300354f, -56.0734367f), new Vector3(0f, -0f, 143.000763f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 308541, 1821116645u, new Vector3(1563.01648f, 388.375977f, -56.07278f), new Vector3(0f, 0f, 7.779429f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 596540, 1821116645u, new Vector3(1562.23022f, 383.887024f, -56.07298f), new Vector3(0f, 0f, -79.4249f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 263479, 1821116645u, new Vector3(1561.51367f, 366.483063f, -56.1048f), new Vector3(0f, -0f, 175.960983f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 570685, 1821116645u, new Vector3(1557.83667f, 374.6935f, -53.2701569f), new Vector3(0f, 0f, -45.65248f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Cartel Guard 2"), 725054, 1821116645u, new Vector3(1557.10583f, 375.589935f, -49.7011833f), new Vector3(3.05225476E-05f, 5.015944E-05f, -84.18509f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static List<Blip> IntelMarkers = new List<Blip>();

		public static List<Blip> IntelMarkersRadiusArea = new List<Blip>();

		public static Blip CargoshipBlip;

		public static Blip CargoshipArea;

		public Vector2 ShipUIPos = new Vector2(-397.11f, -4117.617f);

		public int ShipUIHeading = 40;

		public Vector3 ThermiteIntelLoc = new Vector3(-44.97f, -2715.92f, 5f);

		public static List<LargeContainer> RobbableContainer = new List<LargeContainer>();

		public void DisplayHelpTextThisFrameNoSound(string helpText)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 0, 1);
		}

		public static void ATTACH_ENTITY_TO_ENTITY(Entity entity1, Entity entity2, int boneIndex, float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, bool p9, bool useSoftPinning, bool collision, bool isPed, int vertexIndex, bool fixedRot)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, entity1, entity2, boneIndex, xPos, yPos, zPos, xRot, yRot, zRot, p9, useSoftPinning, collision, isPed, vertexIndex, fixedRot);
		}

		public float GetDistance(Vector3 _1, Vector3 _2)
		{
			return _1.DistanceTo(_2);
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

		public static float GetPercentageModDiff()
		{
			return (float)Mod_Difficulty / 100f;
		}

		public static void SetPedStatsFromDiff(Ped Ped)
		{
			Ped.CanWrithe = false;
			if (Ped.CurrentVehicle == null)
			{
				Ped.Accuracy = (int)(30f * GetPercentageModDiff());
			}
			else
			{
				if (Ped.CurrentVehicle.Model.IsBoat)
				{
					Ped.Accuracy = (int)(16f * GetPercentageModDiff());
				}
				if (Ped.CurrentVehicle.Model.IsHelicopter)
				{
					Ped.Accuracy = (int)(20f * GetPercentageModDiff());
				}
				if (Ped.CurrentVehicle.Model.IsCar)
				{
					Ped.Accuracy = (int)(30f * GetPercentageModDiff());
				}
			}
			if (Mod_Difficulty < 10)
			{
				Function.Call(Hash.SET_PED_COMBAT_ABILITY, Ped, 0);
				Ped.FiringPattern = FiringPattern.SingleShot;
			}
			if (Mod_Difficulty >= 10)
			{
				Function.Call(Hash.SET_PED_COMBAT_ABILITY, Ped, 0);
				Ped.FiringPattern = FiringPattern.BurstFireMicro;
			}
			if (Mod_Difficulty >= 25)
			{
				Function.Call(Hash.SET_PED_COMBAT_ABILITY, Ped, 1);
				Ped.FiringPattern = FiringPattern.BurstFireSMG;
			}
			if (Mod_Difficulty >= 50)
			{
				Function.Call(Hash.SET_PED_COMBAT_ABILITY, Ped, 1);
				Ped.FiringPattern = FiringPattern.BurstFireRifle;
			}
			if (Mod_Difficulty >= 75)
			{
				Function.Call(Hash.SET_PED_COMBAT_ABILITY, Ped, 2);
				Ped.FiringPattern = FiringPattern.BurstFire;
			}
			if (Mod_Difficulty >= 90)
			{
				Function.Call(Hash.SET_PED_COMBAT_ABILITY, Ped, 2);
				Ped.FiringPattern = FiringPattern.FullAuto;
			}
			Ped.Health = (int)(300f * GetPercentageModDiff());
			if (Mod_Difficulty >= 50)
			{
				Ped.Armor = (int)(200f * GetPercentageModDiff());
			}
			if (Mod_Difficulty >= 25)
			{
				Function.Call(Hash.SET_PED_CONFIG_FLAG, Ped, 107, true);
				Function.Call(Hash.SET_PED_CAN_RAGDOLL_FROM_PLAYER_IMPACT, Ped, false);
			}
			if (Mod_Difficulty >= 75)
			{
				Ped.CanRagdoll = false;
			}
			if (Mod_Difficulty >= 90)
			{
				Ped.CanSufferCriticalHits = false;
			}
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

		public static void drawText(string text, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		public static Vector2 World3DToScreen2d(Vector3 pos)
		{
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, pos.X, pos.Y, pos.Z, outputArgument, outputArgument2);
			return new Vector2(outputArgument.GetResult<float>(), outputArgument2.GetResult<float>());
		}

		public CargoShipHeist()
		{
			SmartGuards.PedTaskLoop wL = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5151.016f, -4975.786f, 13.12f),
				new Vector3(5140.747f, -5008.774f, 8.6f),
				new Vector3(5147.927f, -5031.267f, 5.04f),
				new Vector3(5153.173f, -5052.555f, 4.17f),
				new Vector3(5147.927f, -5031.267f, 5.04f),
				new Vector3(5140.747f, -5008.774f, 8.6f)
			}, 0);
			SmartGuards.PedTaskLoop pedTaskLoop = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5146.226f, -5055.785f, 20.39f),
				new Vector3(5146.489f, -5050.74f, 20.39f),
				new Vector3(5151.038f, -5051.177f, 20.39f),
				new Vector3(5150.979f, -5055.955f, 17.7f),
				new Vector3(5151.038f, -5051.177f, 20.39f),
				new Vector3(5146.489f, -5050.74f, 20.39f)
			}, 0);
			SmartGuards.PedTaskLoop pedTaskLoop2 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5149.686f, -4940.591f, 14.45f),
				new Vector3(5157.669f, -4932.769f, 16.8f),
				new Vector3(5154.074f, -4929.133f, 21.18f),
				new Vector3(5150.428f, -4933.045f, 22.6f),
				new Vector3(5154.192f, -4936.5f, 26f),
				new Vector3(5157.509f, -4932.542f, 28.27f),
				new Vector3(5154.15f, -4929.525f, 31f),
				new Vector3(5150.935f, -4933.051f, 31.18f),
				new Vector3(5154.111f, -4936.013f, 30.86f),
				new Vector3(5156.17f, -4933.726f, 30.8f),
				new Vector3(5154.111f, -4936.013f, 30.86f),
				new Vector3(5150.935f, -4933.051f, 31.18f),
				new Vector3(5154.15f, -4929.525f, 31f),
				new Vector3(5157.509f, -4932.542f, 28.27f),
				new Vector3(5154.192f, -4936.5f, 26f),
				new Vector3(5150.428f, -4933.045f, 22.6f),
				new Vector3(5154.074f, -4929.133f, 21.18f),
				new Vector3(5157.669f, -4932.769f, 16.8f),
				new Vector3(5149.686f, -4940.591f, 14.45f),
				new Vector3(5145.67f, -4950.823f, 13.9f)
			}, 0);
			List<SmartGuards.SmartAiV2> list = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5153.283f, -4968.175f, 13.8f), 165f, true, 200, 100, 1, wL, 20f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5184.818f, -5001.994f, 13.8f), 19f, true, 200, 100, 3, 20f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5149.484f, -4937.957f, 14.39f), -115f, true, 200, 100, 3, 20f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(5199.55f, -4948.955f, 14.8f), 200f, false, list));
			foreach (SmartGuards.SmartAiV2 item in list)
			{
				int index = SpawnGroups.Count - 1;
				item.GroupActive = SpawnGroups[index].GroupActive;
				item.DistToSpawn = SpawnGroups[index].DistToSpawn;
				item.SpawnGroupArea = SpawnGroups[index].SpawnGroupArea;
				SpawnGroupsUse.Add(item);
			}
			SmartGuards.PedTaskLoop wL2 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5174f, -5127.102f, 2.84f),
				new Vector3(5167.066f, -5241.179f, 8f)
			}, 0);
			SmartGuards.PedTaskLoop wL3 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5107.687f, -5143.086f, 2.34f),
				new Vector3(5107.865f, -5204.367f, 2.34f)
			}, 0);
			SmartGuards.PedTaskLoop wL4 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5155.891f, -5142.585f, 2.52f),
				new Vector3(5119.042f, -5143.743f, 2.52f)
			}, 0);
			SmartGuards.PedTaskLoop pedTaskLoop3 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5119.042f, -5143.743f, 2.52f),
				new Vector3(5155.891f, -5142.585f, 2.52f)
			}, 0);
			SmartGuards.PedTaskLoop wL5 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5122.286f, -5080.573f, 2.33f),
				new Vector3(5139.946f, -5076.867f, 2.46f),
				new Vector3(5150.859f, -5088.48f, 2.41f),
				new Vector3(5174.893f, -5105.088f, 3.04f),
				new Vector3(5208.417f, -5104.614f, 4.9f),
				new Vector3(5227.589f, -5123.206f, 6.83f),
				new Vector3(5231.911f, -5176.148f, 12.38f),
				new Vector3(5227.589f, -5123.206f, 6.83f),
				new Vector3(5208.417f, -5104.614f, 4.9f),
				new Vector3(5174.893f, -5105.088f, 3.04f),
				new Vector3(5150.859f, -5088.48f, 2.41f),
				new Vector3(5139.946f, -5076.867f, 2.46f),
				new Vector3(5122.286f, -5080.573f, 2.33f),
				new Vector3(5115.024f, -5109.537f, 2.22f)
			}, 0);
			List<SmartGuards.SmartAiV2> list2 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5174.893f, -5127.102f, 2.8f), 165f, true, 200, 100, 1, wL2, 40, 50, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5107.865f, -5204.367f, 2.34f), 165f, true, 200, 100, 1, wL3, 40, 50, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5119.042f, -5143.743f, 2.52f), 165f, true, 200, 100, 1, wL4, 40, 50, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5115.024f, -5109.537f, 2.22f), 148f, true, 200, 100, 1, wL5, 40, 50, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "patrolboat", new Vector3(5103.356f, -5175.002f, 0f), 0f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(5184.443f, -5132.702f, 3.63f), 78f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(5129.108f, -5147.676f, 2.22f), 88f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(5168f, -5145f, 4f), 400f, false, list2));
			foreach (SmartGuards.SmartAiV2 item2 in list2)
			{
				int index2 = SpawnGroups.Count - 1;
				item2.GroupActive = SpawnGroups[index2].GroupActive;
				item2.DistToSpawn = SpawnGroups[index2].DistToSpawn;
				item2.SpawnGroupArea = SpawnGroups[index2].SpawnGroupArea;
				SpawnGroupsUse.Add(item2);
			}
			SmartGuards.PedTaskLoop wL6 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(4931.902f, -5193.525f, 2.4f),
				new Vector3(4878.176f, -5170.083f, 2.4f)
			}, 0);
			SmartGuards.PedTaskLoop wL7 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(4931.902f, -5193.525f, 2.4f),
				new Vector3(4943.588f, -5166.77f, 2.4f)
			}, 0);
			SmartGuards.PedTaskLoop wL8 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(4937.284f, -5236.161f, 2.43f),
				new Vector3(4940.345f, -5207.021f, 2.43f),
				new Vector3(4921.317f, -5220.168f, 2.49f),
				new Vector3(4937.284f, -5236.161f, 2.43f),
				new Vector3(4940.345f, -5207.021f, 2.43f),
				new Vector3(4921.317f, -5220.168f, 2.49f),
				new Vector3(4937.284f, -5236.161f, 2.43f),
				new Vector3(4940.345f, -5207.021f, 2.43f),
				new Vector3(4999.81f, -5184.073f, 2.51f),
				new Vector3(4937.284f, -5236.161f, 2.43f),
				new Vector3(4948.256f, -5218.934f, 2.43f),
				new Vector3(4883.902f, -5183.017f, 2.43f)
			}, 0);
			SmartGuards.PedTaskLoop wL9 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(4948.871f, -5172.254f, 2.5f),
				new Vector3(4922.952f, -5229.563f, 2.5f)
			}, 0, true);
			SmartGuards.PedTaskLoop wL10 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(4967.808f, -5119.88f, 3.12f),
				new Vector3(4972.74f, -5148.011f, 3.129f),
				new Vector3(4993.674f, -5167.642f, 3.129f),
				new Vector3(4985.001f, -5192.309f, 3.129f),
				new Vector3(4993.674f, -5167.642f, 3.129f),
				new Vector3(4972.74f, -5148.011f, 3.129f),
				new Vector3(4967.808f, -5119.88f, 3.12f),
				new Vector3(4949.185f, -5109.204f, 3.129f)
			}, 0);
			SmartGuards.PedTaskLoop wL11 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5020.615f, -5121.322f, 3.44f),
				new Vector3(5014.339f, -5136.071f, 2.95f),
				new Vector3(5039.455f, -5190.143f, 2.74f),
				new Vector3(5048.095f, -5217.907f, 3.32f),
				new Vector3(5039.455f, -5190.143f, 2.74f),
				new Vector3(5014.339f, -5136.071f, 2.95f),
				new Vector3(5020.615f, -5121.322f, 3.44f),
				new Vector3(5038.394f, -5114.313f, 5.78f)
			}, 0);
			List<SmartGuards.SmartAiV2> list3 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(1, "patrolboat", new Vector3(4901.029f, -5172.623f, 0f), -22f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "dinghy5", new Vector3(4893.445f, -5167.81f, 0f), -22f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "dinghy5", new Vector3(4885.385f, -5164.491f, 0f), -22f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "vetir", new Vector3(4957.544f, -5205.249f, 2f), -22f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(5018.947f, -5192.001f, 2.5f), -22f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(4902.67f, -5148.147f, 6f), 500f, false, list3));
			foreach (SmartGuards.SmartAiV2 item3 in list3)
			{
				int index3 = SpawnGroups.Count - 1;
				item3.GroupActive = SpawnGroups[index3].GroupActive;
				item3.DistToSpawn = SpawnGroups[index3].DistToSpawn;
				item3.SpawnGroupArea = SpawnGroups[index3].SpawnGroupArea;
				SpawnGroupsUse.Add(item3);
			}
			SmartGuards.PedTaskLoop wL12 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(4931.883f, -5294.656f, 5.6f),
				new Vector3(4961.429f, -5288.576f, 6f)
			}, 0, false);
			List<SmartGuards.SmartAiV2> list4 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4961.429f, -5288.576f, 6f), -37f, true, 200, 100, 1, wL12, 40, 50, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4895.62f, -5285.487f, 8.5f), 171f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4889.189f, -5405.8f, 17.4f), 104f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4887.69f, -5407.363f, 17.4f), -65f, true, 200, 100, 4, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4948.856f, -5321.222f, 8f), -145f, true, 200, 100, 5, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5038.394f, -5114.313f, 5.78f), 103f, true, 200, 100, 1, wL11, 40, 50, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4949.185f, -5109.204f, 3.129f), -112f, true, 200, 100, 1, wL10, 40, 50, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4878.176f, -5170.083f, 2.4f), 53f, true, 200, 100, 1, wL6, 40, 50, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4943.588f, -5166.77f, 2.4f), -23f, true, 200, 100, 1, wL7, 40, 50, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4950.761f, -5090.455f, 3f), 67f, true, 200, 100, 3, 80f, 70f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4869.723f, -5172.888f, 2.49f), -41f, true, 200, 100, 1, wL8, 40, 50, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5015.859f, -5200.25f, 2.5f), -97f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4922.952f, -5229.563f, 2.5f), -37f, true, 200, 100, 1, wL9, 40, 50, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4976.125f, -5207.028f, 2.5f), 53f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "valkyrie", new Vector3(4884.037f, -5281.112f, 7.8f), -95f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "manchez2", new Vector3(4886.279f, -5409.838f, 16.6f), 57f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "manchez2", new Vector3(4884.631f, -5410.174f, 16.8f), 81f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(4938.254f, -5295.794f, 13f), 650f, false, list4));
			foreach (SmartGuards.SmartAiV2 item4 in list4)
			{
				int index4 = SpawnGroups.Count - 1;
				item4.GroupActive = SpawnGroups[index4].GroupActive;
				item4.DistToSpawn = SpawnGroups[index4].DistToSpawn;
				item4.SpawnGroupArea = SpawnGroups[index4].SpawnGroupArea;
				SpawnGroupsUse.Add(item4);
			}
			SmartGuards.PedTaskLoop pedTaskLoop4 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(4886.47f, -5459.564f, 47.5f),
				new Vector3(4885.62f, -5455.1f, 47.5f),
				new Vector3(4889.8f, -5454.634f, 44.9f),
				new Vector3(4885.014f, -5460.717f, 39.29f),
				new Vector3(4889.8f, -5454.634f, 44.9f),
				new Vector3(4885.62f, -5455.1f, 47.5f),
				new Vector3(4886.47f, -5459.564f, 47.5f),
				new Vector3(4888.48f, -5456.858f, 2.4f)
			}, 0, true);
			List<SmartGuards.SmartAiV2> list5 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(4897.999f, -5462.323f, 30.7f), 153f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(4888.48f, -5456.858f, 2.4f), 100f, false, list5));
			foreach (SmartGuards.SmartAiV2 item5 in list5)
			{
				int index5 = SpawnGroups.Count - 1;
				item5.GroupActive = SpawnGroups[index5].GroupActive;
				item5.DistToSpawn = SpawnGroups[index5].DistToSpawn;
				item5.SpawnGroupArea = SpawnGroups[index5].SpawnGroupArea;
				SpawnGroupsUse.Add(item5);
			}
			List<SmartGuards.SmartAiV2> list6 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(1, "manchez2", new Vector3(5308.958f, -5107.955f, 13.6f), -40f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5308.5f, -5106.048f, 14.14f), 0f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5326.579f, -5101.905f, 14.61f), 111f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5324.674f, -5103.23f, 14.5f), -50f, true, 200, 100, 4, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5321.348f, -5109.379f, 14.57f), 78f, true, 200, 100, 5, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5319.34f, -5108.955f, 14.5f), -95f, true, 200, 100, 5, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(5324.14f, -5087.861f, 13.26f), -21f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(5322.625f, -5100.301f, 14.37f), -130f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(5328.312f, -5100.741f, 14.37f), -132f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "manchez2", new Vector3(5322.647f, -5109.523f, 14.07f), -46f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(5308.958f, -5107.955f, 13.6f), 200f, false, list6));
			foreach (SmartGuards.SmartAiV2 item6 in list6)
			{
				int index6 = SpawnGroups.Count - 1;
				item6.GroupActive = SpawnGroups[index6].GroupActive;
				item6.DistToSpawn = SpawnGroups[index6].DistToSpawn;
				item6.SpawnGroupArea = SpawnGroups[index6].SpawnGroupArea;
				SpawnGroupsUse.Add(item6);
			}
			SmartGuards.PedTaskLoop wL13 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5481.039f, -5144.213f, 13.45f),
				new Vector3(5442.991f, -5117.292f, 13.13f)
			}, 0, false);
			List<SmartGuards.SmartAiV2> list7 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5442.408f, -5115.691f, 13.06f), 106f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5441.292f, -5117.364f, 13.2f), -48f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5442.991f, -5117.292f, 13.13f), -145f, true, 200, 100, 1, wL13, 40, 50, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(5442.408f, -5115.691f, 13.06f), 250f, false, list7));
			foreach (SmartGuards.SmartAiV2 item7 in list7)
			{
				int index7 = SpawnGroups.Count - 1;
				item7.GroupActive = SpawnGroups[index7].GroupActive;
				item7.DistToSpawn = SpawnGroups[index7].DistToSpawn;
				item7.SpawnGroupArea = SpawnGroups[index7].SpawnGroupArea;
				SpawnGroupsUse.Add(item7);
			}
			List<SmartGuards.SmartAiV2> list8 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5551.766f, -5242.25f, 13.27f), 111f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5549.708f, -5243.144f, 13.21f), -53f, true, 200, 100, 4, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(5549.708f, -5243.144f, 13.21f), 100f, false, list8));
			foreach (SmartGuards.SmartAiV2 item8 in list8)
			{
				int index8 = SpawnGroups.Count - 1;
				item8.GroupActive = SpawnGroups[index8].GroupActive;
				item8.DistToSpawn = SpawnGroups[index8].DistToSpawn;
				item8.SpawnGroupArea = SpawnGroups[index8].SpawnGroupArea;
				SpawnGroupsUse.Add(item8);
			}
			List<SmartGuards.SmartAiV2> list9 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5566.907f, -5187.353f, 11.14f), -17f, true, 200, 100, 5, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(5566.907f, -5187.353f, 11.14f), 100f, false, list9));
			foreach (SmartGuards.SmartAiV2 item9 in list9)
			{
				int index9 = SpawnGroups.Count - 1;
				item9.GroupActive = SpawnGroups[index9].GroupActive;
				item9.DistToSpawn = SpawnGroups[index9].DistToSpawn;
				item9.SpawnGroupArea = SpawnGroups[index9].SpawnGroupArea;
				SpawnGroupsUse.Add(item9);
			}
			SmartGuards.PedTaskLoop pedTaskLoop5 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5360.78f, -5439.507f, 66f),
				new Vector3(5356.889f, -5437.166f, 66f),
				new Vector3(5360.78f, -5439.507f, 66f),
				new Vector3(5362.79f, -5437.161f, 66f)
			}, 0, false);
			List<SmartGuards.SmartAiV2> list10 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5355.799f, -5420.054f, 49.2f), 59f, true, 200, 100, 5, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5354.018f, -5419.626f, 49.3f), -103f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(5355.799f, -5420.054f, 49.2f), 100f, false, list10));
			foreach (SmartGuards.SmartAiV2 item10 in list10)
			{
				int index10 = SpawnGroups.Count - 1;
				item10.GroupActive = SpawnGroups[index10].GroupActive;
				item10.DistToSpawn = SpawnGroups[index10].DistToSpawn;
				item10.SpawnGroupArea = SpawnGroups[index10].SpawnGroupArea;
				SpawnGroupsUse.Add(item10);
			}
			List<SmartGuards.SmartAiV2> list11 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5361.893f, -5546.622f, 52.2f), 122f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(5362.886f, -5554.072f, 52.1934f), -116f, SmartGuards.EnemyGroup.Merryweather)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(5362.886f, -5554.072f, 52.1934f), 100f, false, list11));
			foreach (SmartGuards.SmartAiV2 item11 in list11)
			{
				int index11 = SpawnGroups.Count - 1;
				item11.GroupActive = SpawnGroups[index11].GroupActive;
				item11.DistToSpawn = SpawnGroups[index11].DistToSpawn;
				item11.SpawnGroupArea = SpawnGroups[index11].SpawnGroupArea;
				SpawnGroupsUse.Add(item11);
			}
			List<SmartGuards.SmartAiV2> list12 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5399.522f, -5661.797f, 49.9f), -79f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5391.164f, -5661.391f, 50.5f), 49f, true, 200, 100, 2, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "manchez2", new Vector3(5390.825f, -5654.927f, 50.8f), -116f, SmartGuards.EnemyGroup.Merryweather),
				new SmartGuards.SmartAiV2(1, "manchez2", new Vector3(5389.355f, -5657.081f, 50.64f), -116f, SmartGuards.EnemyGroup.Merryweather)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(5389.355f, -5657.081f, 50.64f), 250f, false, list12));
			foreach (SmartGuards.SmartAiV2 item12 in list12)
			{
				int index12 = SpawnGroups.Count - 1;
				item12.GroupActive = SpawnGroups[index12].GroupActive;
				item12.DistToSpawn = SpawnGroups[index12].DistToSpawn;
				item12.SpawnGroupArea = SpawnGroups[index12].SpawnGroupArea;
				SpawnGroupsUse.Add(item12);
			}
			List<SmartGuards.SmartAiV2> list13 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5469.927f, -5828.084f, 19.03f), 12f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5474.904f, -5838.497f, 19.7f), 119f, true, 200, 100, 5, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5472.862f, -5839.763f, 19.8f), -61f, true, 200, 100, 4, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "vetir", new Vector3(5469.792f, -5829.657f, 19.5f), -43f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "manchez2", new Vector3(5477.431f, -5836.858f, 19.08f), -126f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(5477.431f, -5836.858f, 19.08f), 250f, false, list13));
			foreach (SmartGuards.SmartAiV2 item13 in list13)
			{
				int index13 = SpawnGroups.Count - 1;
				item13.GroupActive = SpawnGroups[index13].GroupActive;
				item13.DistToSpawn = SpawnGroups[index13].DistToSpawn;
				item13.SpawnGroupArea = SpawnGroups[index13].SpawnGroupArea;
				SpawnGroupsUse.Add(item13);
			}
			List<SmartGuards.SmartAiV2> list14 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5206.201f, -5514.118f, 47.04f), -94f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "vetir", new Vector3(5209.341f, -5511.062f, 47.4f), -43f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(5209.341f, -5511.062f, 47.4f), 100f, false, list14));
			foreach (SmartGuards.SmartAiV2 item14 in list14)
			{
				int index14 = SpawnGroups.Count - 1;
				item14.GroupActive = SpawnGroups[index14].GroupActive;
				item14.DistToSpawn = SpawnGroups[index14].DistToSpawn;
				item14.SpawnGroupArea = SpawnGroups[index14].SpawnGroupArea;
				SpawnGroupsUse.Add(item14);
			}
			List<SmartGuards.SmartAiV2> list15 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5331.229f, -5486.263f, 53.35f), 43f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "winky", new Vector3(5331.442f, -5484.315f, 52.84f), 64f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "winky", new Vector3(5369.083f, -5440f, 47.99f), -149f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(5371.648f, -5454.639f, 48.9f), 144f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(5383.117f, -5441.91f, 46.9f), 51.96f, SmartGuards.EnemyGroup.Merryweather),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5383.083f, -5444.88f, 47.55f), -114f, true, 200, 100, 4, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5384.453f, -5445.551f, 47.39f), 53f, true, 200, 100, 4, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(5331.442f, -5484.315f, 52.84f), 250f, false, list15));
			foreach (SmartGuards.SmartAiV2 item15 in list15)
			{
				int index15 = SpawnGroups.Count - 1;
				item15.GroupActive = SpawnGroups[index15].GroupActive;
				item15.DistToSpawn = SpawnGroups[index15].DistToSpawn;
				item15.SpawnGroupArea = SpawnGroups[index15].SpawnGroupArea;
				SpawnGroupsUse.Add(item15);
			}
			List<SmartGuards.SmartAiV2> list16 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5050.159f, -5588.584f, 30.65f), 37f, true, 200, 100, 5, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5048.395f, -5586.64f, 30.24f), -132f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5058.022f, -5591.854f, 32.16f), -107f, true, 200, 100, 2, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "winky", new Vector3(5047.978f, -5590.575f, 29.67f), 64f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(5054.616f, -5590.533f, 31.26f), 73f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(5054.616f, -5590.533f, 31.26f), 350f, false, list16));
			foreach (SmartGuards.SmartAiV2 item16 in list16)
			{
				int index16 = SpawnGroups.Count - 1;
				item16.GroupActive = SpawnGroups[index16].GroupActive;
				item16.DistToSpawn = SpawnGroups[index16].DistToSpawn;
				item16.SpawnGroupArea = SpawnGroups[index16].SpawnGroupArea;
				SpawnGroupsUse.Add(item16);
			}
			List<SmartGuards.SmartAiV2> list17 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5309.791f, -5607.369f, 64.31f), -174f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "winky", new Vector3(5311.185f, -5605.141f, 63.9f), 132f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(5311.185f, -5605.141f, 63.9f), 250f, false, list17));
			foreach (SmartGuards.SmartAiV2 item17 in list17)
			{
				int index17 = SpawnGroups.Count - 1;
				item17.GroupActive = SpawnGroups[index17].GroupActive;
				item17.DistToSpawn = SpawnGroups[index17].DistToSpawn;
				item17.SpawnGroupArea = SpawnGroups[index17].SpawnGroupArea;
				SpawnGroupsUse.Add(item17);
			}
			SmartGuards.PedTaskLoop wL14 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5286.606f, -5430.195f, 64.12f),
				new Vector3(5275.327f, -5413.118f, 65.87f),
				new Vector3(5255.528f, -5410.199f, 65.64f),
				new Vector3(5246.161f, -5431.603f, 64.06f),
				new Vector3(5269.363f, -5448.14f, 63.57f)
			}, 0, true);
			List<SmartGuards.SmartAiV2> list18 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5269.363f, -5448.14f, 63.57f), -145f, true, 200, 100, 1, wL14, 40, 50, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(5269.363f, -5448.14f, 63.57f), 100f, false, list18));
			foreach (SmartGuards.SmartAiV2 item18 in list18)
			{
				int index18 = SpawnGroups.Count - 1;
				item18.GroupActive = SpawnGroups[index18].GroupActive;
				item18.DistToSpawn = SpawnGroups[index18].DistToSpawn;
				item18.SpawnGroupArea = SpawnGroups[index18].SpawnGroupArea;
				SpawnGroupsUse.Add(item18);
			}
			List<SmartGuards.SmartAiV2> list19 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5002.048f, -4815.477f, 13.8f), 128f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5002.165f, -4816.754f, 13.8f), 1.2f, true, 200, 100, 4, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "manchez2", new Vector3(4999.054f, -4813.345f, 13.4f), 35f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "manchez2", new Vector3(5000.36f, -4811.817f, 13.79f), 48f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(5000.36f, -4811.817f, 13.79f), 250f, false, list19));
			foreach (SmartGuards.SmartAiV2 item19 in list19)
			{
				int index19 = SpawnGroups.Count - 1;
				item19.GroupActive = SpawnGroups[index19].GroupActive;
				item19.DistToSpawn = SpawnGroups[index19].DistToSpawn;
				item19.SpawnGroupArea = SpawnGroups[index19].SpawnGroupArea;
				SpawnGroupsUse.Add(item19);
			}
			List<SmartGuards.SmartAiV2> list20 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4995.001f, -4539.409f, 9f), 40f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4997.818f, -4542.147f, 9.13f), -154f, true, 200, 100, 2, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5000.071f, -4548.942f, 8.75f), 35f, true, 200, 100, 4, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(4992.227f, -4538.992f, 8.77f), 35f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "vetir", new Vector3(4997.462f, -4545.529f, 9.19f), 35f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(4992.227f, -4538.992f, 8.77f), 350f, false, list20));
			foreach (SmartGuards.SmartAiV2 item20 in list20)
			{
				int index20 = SpawnGroups.Count - 1;
				item20.GroupActive = SpawnGroups[index20].GroupActive;
				item20.DistToSpawn = SpawnGroups[index20].DistToSpawn;
				item20.SpawnGroupArea = SpawnGroups[index20].SpawnGroupArea;
				SpawnGroupsUse.Add(item20);
			}
			SmartGuards.PedTaskLoop pedTaskLoop6 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5104.528f, -4582.101f, 29.71f),
				new Vector3(5110.157f, -4579.732f, 29.71f)
			}, 0, true);
			SmartGuards.PedTaskLoop wL15 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5166.87f, -4648.147f, 2.64f),
				new Vector3(5160.882f, -4641.606f, 2.66f),
				new Vector3(5166.87f, -4648.147f, 2.64f),
				new Vector3(5160.501f, -4674.514f, 1.44f)
			}, 0, true);
			SmartGuards.PedTaskLoop wL16 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5084.44f, -4688.142f, 2.711f),
				new Vector3(5059.892f, -4630.58f, 2.57f),
				new Vector3(5084.44f, -4688.142f, 2.711f),
				new Vector3(5097.086f, -4701.024f, 3.24f)
			}, 0, false);
			SmartGuards.PedTaskLoop wL17 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5173.082f, -4637.42f, 2.54f),
				new Vector3(5165.366f, -5633.551f, 2.38f)
			}, 0, false);
			List<SmartGuards.SmartAiV2> list21 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5063.497f, -4604.626f, 2.8f), -39f, true, 200, 100, 5, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5160.501f, -4674.514f, 1.44f), -21f, true, 200, 100, 1, wL15, 40, 50, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5097.086f, -4701.024f, 3.24f), 40f, true, 200, 100, 1, wL16, 40, 50, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5067.736f, -4633.865f, 2.43f), 87f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5065.993f, -4633.869f, 2.46f), -94f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5165.366f, -5633.551f, 2.38f), -7f, true, 200, 100, 1, wL17, 40, 50, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5128.901f, -4619.169f, 2.26f), 128f, true, 200, 100, 5, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "patrolboat", new Vector3(5059.192f, -4648.697f, 0f), -105f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "patrolboat", new Vector3(5156.462f, -4660.665f, 0f), -17f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "dinghy5", new Vector3(5101.702f, -4634.489f, 0f), -150f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "dinghy5", new Vector3(5129.324f, -4640.23f, 0f), 168f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "valkyrie", new Vector3(5125.661f, -4717.553f, 2.9f), -2.37f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(5156.462f, -4660.665f, 0f), 450f, false, list21));
			foreach (SmartGuards.SmartAiV2 item21 in list21)
			{
				int index21 = SpawnGroups.Count - 1;
				item21.GroupActive = SpawnGroups[index21].GroupActive;
				item21.DistToSpawn = SpawnGroups[index21].DistToSpawn;
				item21.SpawnGroupArea = SpawnGroups[index21].SpawnGroupArea;
				SpawnGroupsUse.Add(item21);
			}
			List<SmartGuards.SmartAiV2> list22 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4872.794f, -4480.707f, 10.05f), 96.84f, true, 200, 100, 5, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4843.367f, -4429.712f, 9.77f), 160f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4909.41f, -4494.056f, 9.67f), -30f, true, 200, 100, 4, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4891.08f, -4487.284f, 9.96f), 62f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4887.873f, -4485.665f, 10.01f), -101f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "winky", new Vector3(4875f, -4480.77f, 9.46f), 35f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "vetir", new Vector3(4845.473f, -4428.648f, 10f), 74f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(4903.358f, -4494.569f, 9.55f), -100f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(4909.548f, -4496.251f, 9.4f), -98f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(4875f, -4480.77f, 9.46f), 300f, false, list22));
			foreach (SmartGuards.SmartAiV2 item22 in list22)
			{
				int index22 = SpawnGroups.Count - 1;
				item22.GroupActive = SpawnGroups[index22].GroupActive;
				item22.DistToSpawn = SpawnGroups[index22].DistToSpawn;
				item22.SpawnGroupArea = SpawnGroups[index22].SpawnGroupArea;
				SpawnGroupsUse.Add(item22);
			}
			List<SmartGuards.SmartAiV2> list23 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4824.124f, -4303.023f, 5.38f), 96.84f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4819.797f, -4302.064f, 5.33f), -85.49f, true, 200, 100, 5, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "winky", new Vector3(4815.128f, -4300.158f, 4.74f), -73f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "winky", new Vector3(4824.56f, -4306.379f, 4.74f), -137f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(4824.56f, -4306.379f, 4.74f), 250f, false, list23));
			foreach (SmartGuards.SmartAiV2 item23 in list23)
			{
				int index23 = SpawnGroups.Count - 1;
				item23.GroupActive = SpawnGroups[index23].GroupActive;
				item23.DistToSpawn = SpawnGroups[index23].DistToSpawn;
				item23.SpawnGroupArea = SpawnGroups[index23].SpawnGroupArea;
				SpawnGroupsUse.Add(item23);
			}
			List<SmartGuards.SmartAiV2> list24 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5584.315f, -5213.009f, 14.43f), -98f, true, 200, 100, 5, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(5581.542f, -5213.692f, 14.2f), 6f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(5581.542f, -5213.692f, 14.2f), 100f, false, list24));
			foreach (SmartGuards.SmartAiV2 item24 in list24)
			{
				int index24 = SpawnGroups.Count - 1;
				item24.GroupActive = SpawnGroups[index24].GroupActive;
				item24.DistToSpawn = SpawnGroups[index24].DistToSpawn;
				item24.SpawnGroupArea = SpawnGroups[index24].SpawnGroupArea;
				SpawnGroupsUse.Add(item24);
			}
			List<SmartGuards.SmartAiV2> list25 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4769.671f, -5574.947f, 23.58f), -119f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4771.549f, -5574.818f, 23.617f), 71f, true, 200, 100, 5, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(4769.715f, -5572.384f, 23.28f), 80f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(4769.715f, -5572.384f, 23.28f), 200f, false, list25));
			foreach (SmartGuards.SmartAiV2 item25 in list25)
			{
				int index25 = SpawnGroups.Count - 1;
				item25.GroupActive = SpawnGroups[index25].GroupActive;
				item25.DistToSpawn = SpawnGroups[index25].DistToSpawn;
				item25.SpawnGroupArea = SpawnGroups[index25].SpawnGroupArea;
				SpawnGroupsUse.Add(item25);
			}
			List<SmartGuards.SmartAiV2> list26 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4901.88f, -5742.602f, 26.36f), -16f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4902.487f, -5739.525f, 26.35f), -166f, true, 200, 100, 4, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4878.358f, -5741.538f, 26.35f), 66f, true, 200, 100, 2, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(4864.112f, -5730.098f, 27.37f), 39f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "annihilator2", new Vector3(4892.115f, -5739.667f, 26.4f), -153f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(4892.115f, -5739.667f, 26.4f), 450f, false, list26));
			foreach (SmartGuards.SmartAiV2 item26 in list26)
			{
				int index26 = SpawnGroups.Count - 1;
				item26.GroupActive = SpawnGroups[index26].GroupActive;
				item26.DistToSpawn = SpawnGroups[index26].DistToSpawn;
				item26.SpawnGroupArea = SpawnGroups[index26].SpawnGroupArea;
				SpawnGroupsUse.Add(item26);
			}
			SmartGuards.PedTaskLoop pedTaskLoop7 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5124.227f, -5529.414f, 70.97f),
				new Vector3(5127.968f, -5527.064f, 70.97f),
				new Vector3(5126.669f, -5524.266f, 70.97f),
				new Vector3(5127.968f, -5527.064f, 70.97f),
				new Vector3(5124.227f, -5529.414f, 70.97f),
				new Vector3(5124.7f, -5525.248f, 70.9f)
			}, 0, false);
			List<SmartGuards.SmartAiV2> list27 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5117.043f, -5524.464f, 54.029f), -2f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5117.289f, -5521.579f, 54.129f), 173f, true, 200, 100, 4, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(5115.56f, -5530.499f, 53.77f), -148f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(5106.731f, -5511.961f, 53.46f), 80f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(5106.731f, -5511.961f, 53.46f), 250f, false, list27));
			foreach (SmartGuards.SmartAiV2 item27 in list27)
			{
				int index27 = SpawnGroups.Count - 1;
				item27.GroupActive = SpawnGroups[index27].GroupActive;
				item27.DistToSpawn = SpawnGroups[index27].DistToSpawn;
				item27.SpawnGroupArea = SpawnGroups[index27].SpawnGroupArea;
				SpawnGroupsUse.Add(item27);
			}
			List<SmartGuards.SmartAiV2> list28 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4762.918f, -4780.705f, 3.79f), 160f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "patrolboat", new Vector3(4767.458f, -4783.812f, 0f), 136f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(4767.458f, -4783.812f, 0f), 100f, false, list28));
			foreach (SmartGuards.SmartAiV2 item28 in list28)
			{
				int index28 = SpawnGroups.Count - 1;
				item28.GroupActive = SpawnGroups[index28].GroupActive;
				item28.DistToSpawn = SpawnGroups[index28].DistToSpawn;
				item28.SpawnGroupArea = SpawnGroups[index28].SpawnGroupArea;
				SpawnGroupsUse.Add(item28);
			}
			List<SmartGuards.SmartAiV2> list29 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4907.774f, -5844.542f, 27.75f), 177f, true, 200, 100, 5, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4907.235f, -5845.948f, 27.59f), 8f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(4907.305f, -4842.456f, 27.7f), 88f, SmartGuards.EnemyGroup.Merryweather)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(4907.305f, -4842.456f, 27.7f), 150f, false, list29));
			foreach (SmartGuards.SmartAiV2 item29 in list29)
			{
				int index29 = SpawnGroups.Count - 1;
				item29.GroupActive = SpawnGroups[index29].GroupActive;
				item29.DistToSpawn = SpawnGroups[index29].DistToSpawn;
				item29.SpawnGroupArea = SpawnGroups[index29].SpawnGroupArea;
				SpawnGroupsUse.Add(item29);
			}
			List<SmartGuards.SmartAiV2> list30 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4957.255f, -5893.962f, 13.9f), 134f, true, 200, 100, 2, 80f, 90f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4971.218f, -5863.754f, 19.75f), 56f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4969.887f, -5862.628f, 19.82f), -132f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "manchez2", new Vector3(4976.56f, -5860.568f, 19.19f), -1.8f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "manchez2", new Vector3(4974.397f, -5860.001f, 19.27f), 23f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(4907.305f, -4842.456f, 27.7f), 250f, false, list30));
			foreach (SmartGuards.SmartAiV2 item30 in list30)
			{
				int index30 = SpawnGroups.Count - 1;
				item30.GroupActive = SpawnGroups[index30].GroupActive;
				item30.DistToSpawn = SpawnGroups[index30].DistToSpawn;
				item30.SpawnGroupArea = SpawnGroups[index30].SpawnGroupArea;
				SpawnGroupsUse.Add(item30);
			}
			SmartGuards.PedTaskLoop wL18 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5443.502f, -5243.547f, 28.5f),
				new Vector3(5429.743f, -5271.194f, 31.77f),
				new Vector3(5406.802f, -5296.94f, 34.06f),
				new Vector3(5386.241f, -5307.513f, 35.8f),
				new Vector3(5319.058f, -5246.041f, 32.58f),
				new Vector3(5386.241f, -5307.513f, 35.8f),
				new Vector3(5406.802f, -5296.94f, 34.06f),
				new Vector3(5429.743f, -5271.194f, 31.77f),
				new Vector3(5443.502f, -5243.547f, 28.5f),
				new Vector3(5464.997f, -5226.345f, 27.28f)
			}, 0, false);
			SmartGuards.PedTaskLoop wL19 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5314.49f, -5256.158f, 32.57f),
				new Vector3(5387.932f, -5180.298f, 31.08f),
				new Vector3(5396.946f, -5182.153f, 31.44f),
				new Vector3(5409.65f, -5195.354f, 32.92f),
				new Vector3(5396.946f, -5182.153f, 31.44f),
				new Vector3(5387.932f, -5180.298f, 31.08f),
				new Vector3(5314.49f, -5256.158f, 32.57f),
				new Vector3(5271.892f, -5302.779f, 32.227f)
			}, 0, false);
			List<SmartGuards.SmartAiV2> list31 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5464.997f, -5226.345f, 27.28f), 131f, true, 200, 100, 1, wL18, 40, 50, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5271.892f, -5302.779f, 32.227f), -39f, true, 200, 100, 1, wL19, 40, 50, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5304.229f, -5173.93f, 28.67f), 76f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5302.86f, -5173.57f, 28.6f), -132f, true, 200, 100, 5, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5241.163f, -5263.519f, 24.02f), -132f, true, 200, 100, 4, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5331.385f, -5244.814f, 32.5f), -22f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5313.813f, -5247.381f, 32.6f), 102f, true, 200, 100, 4, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5310.461f, -5248.209f, 32.704f), -76f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5399.777f, -5180.042f, 31.35f), -70f, true, 200, 100, 4, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5402.006f, -5181.866f, 31.45f), 51.74f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "winky", new Vector3(5394.879f, -5187.828f, 31.23f), 30.8f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(5305.25f, -5172.196f, 28.25f), 47f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "vetir", new Vector3(5242.063f, -5267.505f, 24.78f), 23f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "vetir", new Vector3(5313.648f, -5262.086f, 32.9f), 129f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(5313.294f, -5251.136f, 32.3f), -11f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(5331.074f, -5246.893f, 32.38f), -112f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(5395.329f, -5175.102f, 31.05f), 160f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "winky", new Vector3(5402.716f, -5179.509f, 30.93f), -130.8f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(5337.429f, -5242.6f, 33f), 350f, false, list31));
			foreach (SmartGuards.SmartAiV2 item31 in list31)
			{
				int index31 = SpawnGroups.Count - 1;
				item31.GroupActive = SpawnGroups[index31].GroupActive;
				item31.DistToSpawn = SpawnGroups[index31].DistToSpawn;
				item31.SpawnGroupArea = SpawnGroups[index31].SpawnGroupArea;
				SpawnGroupsUse.Add(item31);
			}
			List<SmartGuards.SmartAiV2> list32 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5237.189f, -5176.944f, 12.76f), 25f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5236.677f, -5175.808f, 12.66f), -158f, true, 200, 100, 4, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "winky", new Vector3(5237.85f, -5179.27f, 12.3f), 30.8f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(5237.85f, -5179.27f, 12.3f), 150f, false, list32));
			foreach (SmartGuards.SmartAiV2 item32 in list32)
			{
				int index32 = SpawnGroups.Count - 1;
				item32.GroupActive = SpawnGroups[index32].GroupActive;
				item32.DistToSpawn = SpawnGroups[index32].DistToSpawn;
				item32.SpawnGroupArea = SpawnGroups[index32].SpawnGroupArea;
				SpawnGroupsUse.Add(item32);
			}
			List<SmartGuards.SmartAiV2> list33 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5160.79f, -5103.677f, 2.26f), -155f, true, 200, 100, 2, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(5159.561f, -5100.531f, 2.06f), 24.55f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(5159.561f, -5100.531f, 2.06f), 100f, false, list33));
			foreach (SmartGuards.SmartAiV2 item33 in list33)
			{
				int index33 = SpawnGroups.Count - 1;
				item33.GroupActive = SpawnGroups[index33].GroupActive;
				item33.DistToSpawn = SpawnGroups[index33].DistToSpawn;
				item33.SpawnGroupArea = SpawnGroups[index33].SpawnGroupArea;
				SpawnGroupsUse.Add(item33);
			}
			List<SmartGuards.SmartAiV2> list34 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5131.403f, -5282.977f, 7.8f), -99f, true, 200, 100, 5, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5132.596f, -5283.01f, 7.9f), 84f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(5130.82f, -5279.448f, 7.27f), -171f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(5130.82f, -5279.448f, 7.27f), 150f, false, list34));
			foreach (SmartGuards.SmartAiV2 item34 in list34)
			{
				int index34 = SpawnGroups.Count - 1;
				item34.GroupActive = SpawnGroups[index34].GroupActive;
				item34.DistToSpawn = SpawnGroups[index34].DistToSpawn;
				item34.SpawnGroupArea = SpawnGroups[index34].SpawnGroupArea;
				SpawnGroupsUse.Add(item34);
			}
			List<SmartGuards.SmartAiV2> list35 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4841.483f, -5423.151f, 18.9f), -125f, true, 200, 100, 5, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "manchez2", new Vector3(4838.377f, -5428.401f, 18.46f), -129f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(4838.377f, -5428.401f, 18.46f), 100f, false, list35));
			foreach (SmartGuards.SmartAiV2 item35 in list35)
			{
				int index35 = SpawnGroups.Count - 1;
				item35.GroupActive = SpawnGroups[index35].GroupActive;
				item35.DistToSpawn = SpawnGroups[index35].DistToSpawn;
				item35.SpawnGroupArea = SpawnGroups[index35].SpawnGroupArea;
				SpawnGroupsUse.Add(item35);
			}
			List<SmartGuards.SmartAiV2> list36 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5443.191f, -5303.719f, 23.92f), -63f, true, 200, 100, 5, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5444.488f, -5302.624f, 23.67f), 138f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(5443.887f, -5306.96f, 23.7f), 22f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(5443.887f, -5306.96f, 23.7f), 150f, false, list36));
			foreach (SmartGuards.SmartAiV2 item36 in list36)
			{
				int index36 = SpawnGroups.Count - 1;
				item36.GroupActive = SpawnGroups[index36].GroupActive;
				item36.DistToSpawn = SpawnGroups[index36].DistToSpawn;
				item36.SpawnGroupArea = SpawnGroups[index36].SpawnGroupArea;
				SpawnGroupsUse.Add(item36);
			}
			List<SmartGuards.SmartAiV2> list37 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4690.851f, -4460.313f, 5.88f), 163f, true, 200, 100, 5, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4699.388f, -4460.02f, 6.27f), 75f, true, 200, 100, 4, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4697.441f, -4459.669f, 6.127f), -100f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(4697.972f, -4457.797f, 5.9f), 87f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(4690.96f, -4458.212f, 5.6f), 73f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(4697.972f, -4457.797f, 5.9f), 250f, false, list37));
			foreach (SmartGuards.SmartAiV2 item37 in list37)
			{
				int index37 = SpawnGroups.Count - 1;
				item37.GroupActive = SpawnGroups[index37].GroupActive;
				item37.DistToSpawn = SpawnGroups[index37].DistToSpawn;
				item37.SpawnGroupArea = SpawnGroups[index37].SpawnGroupArea;
				SpawnGroupsUse.Add(item37);
			}
			SmartGuards.PedTaskLoop wL20 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(4415.019f, -4536.857f, 4.46f),
				new Vector3(4519.185f, -4499.57f, 4.46f)
			}, 0, false);
			List<SmartGuards.SmartAiV2> list38 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4516.96f, -4510.139f, 4.45f), 101f, true, 200, 100, 5, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4519.185f, -4499.57f, 4.46f), 111f, true, 200, 100, 1, wL20, 40, 50, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4489.653f, -4450.595f, 4.131f), 173f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4486.118f, -4457.148f, 4.24f), 70f, true, 200, 100, 4, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4483.678f, -4456.176f, 4.23f), 70f, true, 200, 100, 3, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4511.939f, -4525.089f, 4.16f), 25f, true, 200, 100, 5, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4516.581f, -4522.99f, 4.15f), 50f, true, 200, 100, 5, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(4519.825f, -4504.113f, 3.9f), 52f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "manchez2", new Vector3(4519.202f, -4511.391f, 3.98f), 140f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "manchez2", new Vector3(4518.317f, -4509.425f, 3.918f), 73f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "vetir", new Vector3(4473.373f, -4532.9f, 4.76f), -4f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(4475.369f, -14008f / MathF.PI, 3.96f), -34f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(4485.356f, -4454.638f, 3.96f), -110f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "manchez2", new Vector3(4488.028f, -4446.386f, 3.46f), -27f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "manchez2", new Vector3(4484.699f, -4445.71f, 3.481f), 157f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "manchez2", new Vector3(4489.294f, -4449.131f, 3.48f), 161f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(4454.591f, -4496.119f, 13f), 450f, false, list38));
			foreach (SmartGuards.SmartAiV2 item38 in list38)
			{
				int index38 = SpawnGroups.Count - 1;
				item38.GroupActive = SpawnGroups[index38].GroupActive;
				item38.DistToSpawn = SpawnGroups[index38].DistToSpawn;
				item38.SpawnGroupArea = SpawnGroups[index38].SpawnGroupArea;
				SpawnGroupsUse.Add(item38);
			}
			SmartGuards.PedTaskLoop wL21 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(4977.179f, -4934.698f, 6.45f),
				new Vector3(4951.796f, -4916.376f, 4.55f),
				new Vector3(4921.513f, -4908.26f, 3.44f),
				new Vector3(4951.796f, -4916.376f, 4.55f),
				new Vector3(4977.179f, -4934.698f, 6.45f),
				new Vector3(5002.41f, -4929.406f, 9.069f)
			}, 0, false);
			List<SmartGuards.SmartAiV2> list39 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4916.985f, -4907.726f, 3.37f), -54f, true, 200, 100, 5, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4943.404f, -4876.599f, 3.843f), -174f, true, 200, 100, 2, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5002.41f, -4929.406f, 9.069f), 143f, true, 200, 100, 1, wL21, 40, 50, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "WINKY", new Vector3(4916.543f, -4900.232f, 2.86f), -128f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(4916.543f, -4900.232f, 2.86f), 250f, false, list39));
			foreach (SmartGuards.SmartAiV2 item39 in list39)
			{
				int index39 = SpawnGroups.Count - 1;
				item39.GroupActive = SpawnGroups[index39].GroupActive;
				item39.DistToSpawn = SpawnGroups[index39].DistToSpawn;
				item39.SpawnGroupArea = SpawnGroups[index39].SpawnGroupArea;
				SpawnGroupsUse.Add(item39);
			}
			List<SmartGuards.SmartAiV2> list40 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4977.098f, -5597.204f, 23.67f), -82f, true, 200, 100, 5, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4982.202f, -5601.98f, 23.84f), 14.24f, true, 200, 100, 2, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "WINKY", new Vector3(4977.014f, -5603.429f, 23.21f), 140.37f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(4977.014f, -5603.429f, 23.21f), 150f, false, list40));
			foreach (SmartGuards.SmartAiV2 item40 in list40)
			{
				int index40 = SpawnGroups.Count - 1;
				item40.GroupActive = SpawnGroups[index40].GroupActive;
				item40.DistToSpawn = SpawnGroups[index40].DistToSpawn;
				item40.SpawnGroupArea = SpawnGroups[index40].SpawnGroupArea;
				SpawnGroupsUse.Add(item40);
			}
			List<SmartGuards.SmartAiV2> list41 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4965.865f, -5451.282f, 33.36f), -102f, true, 200, 100, 5, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4969.68f, -5454.226f, 33.3f), -157f, true, 200, 100, 2, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4968.058f, -5452.317f, 33.39f), 50f, true, 200, 100, 2, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(4972.352f, -5458.841f, 33.77f), 2.4f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(4965.112f, -5453.115f, 32.95f), -109f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(4972.352f, -5458.841f, 33.77f), 150f, false, list41));
			foreach (SmartGuards.SmartAiV2 item41 in list41)
			{
				int index41 = SpawnGroups.Count - 1;
				item41.GroupActive = SpawnGroups[index41].GroupActive;
				item41.DistToSpawn = SpawnGroups[index41].DistToSpawn;
				item41.SpawnGroupArea = SpawnGroups[index41].SpawnGroupArea;
				SpawnGroupsUse.Add(item41);
			}
			List<SmartGuards.SmartAiV2> list42 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5242.542f, -5426.69f, 64.31f), 155f, true, 200, 100, 2, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(5242.358f, -5413.287f, 64.44f), 11f, true, 200, 100, 2, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(5245.521f, -5414.129f, 65.15f), 68f, SmartGuards.EnemyGroup.Cartel),
				new SmartGuards.SmartAiV2(1, "squaddie", new Vector3(5244.428f, -5423.62f, 65.27f), 164f, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(5244.428f, -5423.62f, 65.27f), 150f, false, list42));
			foreach (SmartGuards.SmartAiV2 item42 in list42)
			{
				int index42 = SpawnGroups.Count - 1;
				item42.GroupActive = SpawnGroups[index42].GroupActive;
				item42.DistToSpawn = SpawnGroups[index42].DistToSpawn;
				item42.SpawnGroupArea = SpawnGroups[index42].SpawnGroupArea;
				SpawnGroupsUse.Add(item42);
			}
			SmartGuards.PedTaskLoop pedTaskLoop8 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(4957.263f, -5845.488f, 21.01f),
				new Vector3(4986.514f, -5877.633f, 20.5f),
				new Vector3(4957.263f, -5845.488f, 21.01f),
				new Vector3(4915.578f, -5830.547f, 27.9f)
			}, 0, false);
			SmartGuards.PedTaskLoop pedTaskLoop9 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(4939.182f, -5750.378f, 20.8f),
				new Vector3(4946.851f, -5732.348f, 20.8f),
				new Vector3(4959.426f, -5704.615f, 20.8f),
				new Vector3(5001.963f, -5659.204f, 20.8f),
				new Vector3(4959.426f, -5704.615f, 20.8f),
				new Vector3(4946.851f, -5732.348f, 20.8f),
				new Vector3(4939.182f, -5750.378f, 20.8f),
				new Vector3(4948.092f, -5777.671f, 20.8f)
			}, 0, false);
			SmartGuards.PedTaskLoop wL22 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(4965.754f, -5786.165f, 20.8f),
				new Vector3(4976.193f, -5756.029f, 20.8f)
			}, 0, false);
			SmartGuards.PedTaskLoop pedTaskLoop10 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(4998.979f, -5797.086f, 20.8f),
				new Vector3(4994.052f, -5805.889f, 20.8f),
				new Vector3(4971.527f, -5792.862f, 20.8f),
				new Vector3(4975.412f, -5782.708f, 20.8f)
			}, 0, false);
			SmartGuards.PedTaskLoop pedTaskLoop11 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(4988.177f, -5774.72f, 17f),
				new Vector3(4984.884f, -5783.001f, 17f),
				new Vector3(5005.608f, -5796.728f, 17.4f),
				new Vector3(5015.063f, -5810.096f, 17.4f),
				new Vector3(5005.608f, -5796.728f, 17.4f),
				new Vector3(4984.884f, -5783.001f, 17f),
				new Vector3(4988.177f, -5774.72f, 17f),
				new Vector3(4994.376f, -5767.435f, 16.27f)
			}, 0, false);
			SmartGuards.PedTaskLoop pedTaskLoop12 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5022.116f, -5761.093f, 16.27f),
				new Vector3(5000.106f, -5778.007f, 16.27f),
				new Vector3(5022.116f, -5761.093f, 16.27f),
				new Vector3(5036.047f, -5782.886f, 16.27f)
			}, 0, false);
			SmartGuards.PedTaskLoop pedTaskLoop13 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5024.944f, -5745.248f, 16.27f),
				new Vector3(5007.439f, -5768.745f, 16.27f)
			}, 0, false);
			SmartGuards.PedTaskLoop pedTaskLoop14 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5021.095f, -5679.711f, 19.8f),
				new Vector3(5033.095f, -5689.465f, 19.8f),
				new Vector3(5039.881f, -5699.631f, 19.8f),
				new Vector3(5022.109f, -5711.606f, 19.8f),
				new Vector3(5006.271f, -5697.691f, 19.8f)
			}, 0, false);
			SmartGuards.PedTaskLoop pedTaskLoop15 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5010.991f, -5727.457f, 19.6f),
				new Vector3(4993.834f, -5712.688f, 10.8f),
				new Vector3(5010.991f, -5727.457f, 19.6f),
				new Vector3(5015.684f, -5731.442f, 17.8f)
			}, 0, false);
			SmartGuards.PedTaskLoop pedTaskLoop16 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5002.34f, -5699.356f, 20.06f),
				new Vector3(4996.445f, -5706.013f, 20.077f),
				new Vector3(5002.34f, -5699.356f, 20.06f),
				new Vector3(5019.56f, -5714.661f, 20.06f)
			}, 0, false);
			SmartGuards.PedTaskLoop pedTaskLoop17 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5094.902f, -5746.622f, 15.6f),
				new Vector3(5083.584f, -5738.125f, 15.6f),
				new Vector3(5094.902f, -5746.622f, 15.6f),
				new Vector3(5088.221f, -5756.504f, 15.6f)
			}, 0, false);
			SmartGuards.PedTaskLoop pedTaskLoop18 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5040.43f, -5695.56f, 19.8f),
				new Vector3(5048.577f, -5706.4f, 14.54f),
				new Vector3(5050.367f, -5713.17f, 14.5f),
				new Vector3(5065.554f, -5724.533f, 14.45f),
				new Vector3(5075.698f, -5732.241f, 15.8f),
				new Vector3(5061.745f, -5751.189f, 15.8f),
				new Vector3(5075.698f, -5732.241f, 15.8f),
				new Vector3(5065.554f, -5724.533f, 14.45f),
				new Vector3(5050.367f, -5713.17f, 14.5f),
				new Vector3(5048.577f, -5706.4f, 14.54f),
				new Vector3(5040.43f, -5695.56f, 19.8f),
				new Vector3(5021.516f, -5680.59f, 19.8f)
			}, 0, false);
			SmartGuards.PedTaskLoop pedTaskLoop19 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5041.019f, -5753.539f, 15.6f),
				new Vector3(5037.438f, -5774.375f, 15.6f),
				new Vector3(5041.019f, -5753.539f, 15.6f),
				new Vector3(5059.316f, -5733.689f, 15.67f)
			}, 0, false);
			SmartGuards.PedTaskLoop pedTaskLoop20 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5021.251f, -5761.355f, 16.22f),
				new Vector3(5029.257f, -5749.613f, 16.22f),
				new Vector3(5018.711f, -5736.339f, 17.77f),
				new Vector3(5032.62f, -5723.378f, 17.72f),
				new Vector3(5037.127f, -5728.883f, 16.86f),
				new Vector3(5044.186f, -5742.81f, 16.86f),
				new Vector3(5051.365f, -5735.258f, 16.86f),
				new Vector3(5048.352f, -5732.67f, 16.86f),
				new Vector3(5049.861f, -5727.781f, 16.86f),
				new Vector3(5046.306f, -5723.5f, 16.86f),
				new Vector3(5042.125f, -5725.585f, 16.86f),
				new Vector3(5036.488f, -5718.138f, 16.86f),
				new Vector3(5031.55f, -5720.989f, 17.9f),
				new Vector3(5028.33f, -5716.528f, 17.9f),
				new Vector3(5018.469f, -5724.694f, 17.9f),
				new Vector3(5013.553f, -5733.864f, 17.9f),
				new Vector3(5031.708f, -5749.236f, 16.49f)
			}, 0, false);
			SmartGuards.PedTaskLoop pedTaskLoop21 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(4982.826f, -5750.097f, 19.8f),
				new Vector3(4972.482f, -5748.227f, 19.8f),
				new Vector3(4987.542f, -5725.668f, 19.8f),
				new Vector3(4994.501f, -5730.038f, 19.9f)
			}, 0, false);
			SmartGuards.PedTaskLoop pedTaskLoop22 = new SmartGuards.PedTaskLoop(new List<Vector3>
			{
				new Vector3(5021.21f, -5749.88f, 19.8f),
				new Vector3(5008.58f, -5768.787f, 19.8f),
				new Vector3(4994.607f, -5761.077f, 19.8f),
				new Vector3(4999.951f, -5751.766f, 19.8f),
				new Vector3(5011.267f, -5758.816f, 22.089f),
				new Vector3(5018.531f, -5747.007f, 24.26f),
				new Vector3(5010.122f, -5741.738f, 26.49f),
				new Vector3(5004.323f, -5750.077f, 28.68f),
				new Vector3(5010.122f, -5741.738f, 26.49f),
				new Vector3(5018.531f, -5747.007f, 24.26f),
				new Vector3(5011.267f, -5758.816f, 22.089f),
				new Vector3(4999.464f, -5752.064f, 19.8f),
				new Vector3(5005.738f, -5740.406f, 19.8f)
			}, 0, false);
			Vector3 vector = default(Vector3);
			float num = 0f;
			int num2 = -1;
			List<SmartGuards.SmartAiV2> list43 = new List<SmartGuards.SmartAiV2>();
			foreach (MissionOjects item43 in CargoshipNav)
			{
				List<Vector3> list44 = new List<Vector3>();
				for (int i = 0; i < item43.Entities.Count; i++)
				{
					ObjectPlacementMap objectPlacementMap = item43.Entities[i];
					if (objectPlacementMap.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"))
					{
						list44.Add(objectPlacementMap.Position);
					}
					if (objectPlacementMap.Hash != Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"))
					{
						SmartGuards.PedTaskLoop wL23 = new SmartGuards.PedTaskLoop(list44, 0, true);
						list43.Add(new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", objectPlacementMap.Position, objectPlacementMap.Rotation.Z, true, 200, 100, 1, wL23, 40, 50, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel));
					}
				}
			}
			SpawnGroups.Add(new CayoPericoSpawnGroup(CargoShipPos, 300f, false, list43));
			foreach (SmartGuards.SmartAiV2 item44 in list43)
			{
				int index43 = SpawnGroups.Count - 1;
				item44.GroupActive = SpawnGroups[index43].GroupActive;
				item44.DistToSpawn = SpawnGroups[index43].DistToSpawn;
				item44.SpawnGroupArea = SpawnGroups[index43].SpawnGroupArea;
				SpawnGroupsUse.Add(item44);
			}
			List<SmartGuards.SmartAiV2> list45 = new List<SmartGuards.SmartAiV2>
			{
				new SmartGuards.SmartAiV2(0, "g_m_m_cartelguards_01", new Vector3(4976.193f, -5756.029f, 20.8f), 9f, true, 200, 100, 1, wL22, 40, 50, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Cartel)
			};
			SpawnGroups.Add(new CayoPericoSpawnGroup(new Vector3(4693.926f, -5726.41f, 2.122f), 250f, false, list45));
			foreach (SmartGuards.SmartAiV2 item45 in list45)
			{
				int index44 = SpawnGroups.Count - 1;
				item45.GroupActive = SpawnGroups[index44].GroupActive;
				item45.DistToSpawn = SpawnGroups[index44].DistToSpawn;
				item45.SpawnGroupArea = SpawnGroups[index44].SpawnGroupArea;
				SpawnGroupsUse.Add(item45);
			}
			base.Tick += OnTick;
		}

		public static void LOADIPL(string IPL)
		{
			try
			{
				Function.Call(Hash.REQUEST_IPL, IPL);
			}
			catch
			{
				Screen.ShowSubtitle("~r~IPL CAll OUSIDE MAIN LOOP~w~{" + IPL + "}");
			}
		}

		public static void StartHeist()
		{
			ManageRobberyAnims.Heist_Average_Pay = (int)((float)(HeistMinPay + HeistMaxPay) / 2f * 0.8f);
			Function.Call(Hash.BEGIN_TEXT_COMMAND_BUSYSPINNER_ON, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "Loading La'oub Princess Cargoship Heist, Please Wait");
			Function.Call(Hash.END_TEXT_COMMAND_BUSYSPINNER_ON, 5);
			EndHeist();
			SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Cartel;
			if (!CreatedLootPlusMain)
			{
				CargoHeist_Main.CreateEntites();
				CargoHeist_ThermitePickups.CreateEntites();
				CargoHeist_Manifest_Jammer.CreateEntites();
				Blip blip = GlobalHelper.CreateBlip(SavageIntelLoc);
				blip.Sprite = BlipSprite.HeistSetup;
				blip.Color = BlipColor.Green;
				blip.IsShortRange = true;
				blip.Scale = 0.5f;
				blip.Name = "Possible Intel/Asset";
				IntelMarkers.Add(blip);
				blip = GlobalHelper.CreateBlip(SavageIntelLoc, 100f);
				blip.Name = "Possible Intel/Asset";
				blip.Color = BlipColor.Green;
				blip.Alpha = 140;
				IntelMarkersRadiusArea.Add(blip);
				blip = GlobalHelper.CreateBlip(GrinderIntelLoc);
				blip.Sprite = BlipSprite.HeistSetup;
				blip.Color = BlipColor.Green;
				blip.IsShortRange = true;
				blip.Scale = 0.5f;
				blip.Name = "Possible Intel/Asset";
				IntelMarkers.Add(blip);
				blip = GlobalHelper.CreateBlip(GrinderIntelLoc, 100f);
				blip.Name = "Possible Intel/Asset";
				blip.Color = BlipColor.Green;
				blip.Alpha = 140;
				IntelMarkersRadiusArea.Add(blip);
				blip = GlobalHelper.CreateBlip(new Vector3(-490.266f, 6446.18f, 7.8f));
				blip.Sprite = BlipSprite.HeistSetup;
				blip.Color = BlipColor.Green;
				blip.IsShortRange = true;
				blip.Scale = 0.5f;
				blip.Name = "Possible Intel/Asset";
				IntelMarkers.Add(blip);
				blip = GlobalHelper.CreateBlip(new Vector3(-490.266f, 6446.18f, 7.8f), 100f);
				blip.Name = "Possible Intel/Asset";
				blip.Color = BlipColor.Green;
				blip.Alpha = 140;
				IntelMarkersRadiusArea.Add(blip);
				blip = GlobalHelper.CreateBlip(CargoHeist_Manifest_Jammer.Entities[0].Position);
				blip.Sprite = BlipSprite.HeistSetup;
				blip.Color = BlipColor.Green;
				blip.IsShortRange = true;
				blip.Scale = 0.5f;
				blip.Name = "Possible Intel/Asset";
				IntelMarkers.Add(blip);
				blip = GlobalHelper.CreateBlip(CargoHeist_Manifest_Jammer.Entities[0].Position, 100f);
				blip.Name = "Possible Intel/Asset";
				blip.Color = BlipColor.Green;
				blip.Alpha = 140;
				IntelMarkersRadiusArea.Add(blip);
				blip = GlobalHelper.CreateBlip(CargoHeist_Manifest_Jammer.Entities[1].Position);
				blip.Sprite = BlipSprite.HeistSetup;
				blip.Color = BlipColor.Green;
				blip.IsShortRange = true;
				blip.Scale = 0.5f;
				blip.Name = "Possible Intel/Asset";
				IntelMarkers.Add(blip);
				blip = GlobalHelper.CreateBlip(CargoHeist_Manifest_Jammer.Entities[1].Position, 100f);
				blip.Name = "Possible Intel/Asset";
				blip.Color = BlipColor.Green;
				blip.Alpha = 140;
				IntelMarkersRadiusArea.Add(blip);
				blip = GlobalHelper.CreateBlip(new Vector3(-44.97f, -2715.92f, 5f));
				blip.Sprite = BlipSprite.HeistSetup;
				blip.Color = BlipColor.Green;
				blip.IsShortRange = true;
				blip.Scale = 0.5f;
				blip.Name = "Possible Intel/Asset";
				IntelMarkers.Add(blip);
				blip = GlobalHelper.CreateBlip(new Vector3(-44.97f, -2715.92f, 5f), 100f);
				blip.Name = "Possible Intel/Asset";
				blip.Color = BlipColor.Green;
				blip.Alpha = 140;
				IntelMarkersRadiusArea.Add(blip);
				if (CargoshipBlip == null)
				{
					CargoshipBlip = GlobalHelper.CreateBlip(CargoShipPos);
					CargoshipBlip.Sprite = BlipSprite.Yacht;
					CargoshipBlip.Color = BlipColor.Yellow;
					CargoshipBlip.Name = "La'oub Princess Cargoship";
					CargoshipBlip.IsShortRange = true;
				}
				if (CargoshipArea == null)
				{
					CargoshipArea = GlobalHelper.CreateBlip(CargoShipPos, 100f);
					CargoshipArea.Color = BlipColor.Yellow;
					CargoshipArea.Name = "La'oub Princess Cargoship";
					CargoshipArea.IsShortRange = true;
					CargoshipArea.Alpha = 144;
				}
				Script.Wait(1);
				foreach (Container container in Containers)
				{
					Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, container.PTFX2, false);
					Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, container.PTFX1, false);
					container.containerswitch = -100;
					if (container.Bomb != null)
					{
						container.Bomb.Delete();
					}
					container.BombExplodeTimer = 0;
					container.plantBombStage = 0;
					if (container.Bag != null)
					{
						container.Bag.Delete();
					}
					if (container.Saw != null)
					{
						container.Saw.Delete();
					}
					if (container.Container1 != null)
					{
						container.Container1.Delete();
					}
					if (container.FakeContainer != null)
					{
						container.FakeContainer.Delete();
					}
					if (container.Lock != null)
					{
						container.Lock.Delete();
					}
					Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, container.ParticleFx);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, container.SyncScene, 0f);
				}
				Containers.Clear();
				foreach (ObjectPlacementMap entity in CargoHeist_Main.Entities)
				{
					if (entity.Entity != null)
					{
						if (entity.Entity.Model.IsPed)
						{
							SpawnGroupsUse.Add(new SmartGuards.SmartAiV2((Ped)entity.Entity, SmartGuards.EnemyGroup.Cartel));
						}
						if (entity.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_sam_turret_01"))
						{
							SamSites.Add(new SamSite((Prop)entity.Entity));
						}
						if (entity.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_container_01a"))
						{
							Containers.Add(new Container((Prop)entity.Entity));
							ContainersProps.Add((Prop)entity.Entity);
							entity.Entity.AddBlip();
							entity.Entity.AttachedBlip.Sprite = BlipSprite.ShippingContainer;
							entity.Entity.AttachedBlip.Color = BlipColor.White;
							entity.Entity.AttachedBlip.Alpha = 255;
							entity.Entity.AttachedBlip.Name = "Robbable Container";
						}
					}
				}
				CreatedLootPlusMain = true;
			}
			LOADIPL("h4_int_placement_h4_interior_0_int_sub_h4_milo_");
			LOADIPL("m23_2_cargoship_lod");
			LOADIPL("m23_2_cargoship_bridge_lod");
			LOADIPL("m23_2_cargoship_bridge");
			LOADIPL("m23_2_cargoship");
			LOADIPL("m23_2_submarine_sand");
			Function.Call(Hash.BUSYSPINNER_OFF);
			HeistStage = CargoShipStage.Scout;
			CargoshipHeistOn = true;
		}

		public static string LoadDict(string dict)
		{
			int num = 0;
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, dict))
			{
				Function.Call(Hash.REQUEST_ANIM_DICT, dict);
				Script.Yield();
				num++;
				if (num >= 1000)
				{
					Notification.Show("Animation Not Found " + dict);
					break;
				}
			}
			return dict;
		}

		public static void EndHeist()
		{
			ManageRobberyAnims.DeleteLoot();
			SmartGuards.RemoveAIGuards();
			RobberyAI.CurrentPay = 0;
			CurrentPay = 0;
			if (RobberyAI.PanthereStatue != null)
			{
				RobberyAI.PanthereStatue.Delete();
			}
			if (RobberyAI.RobberACurrentLootBagProp != null)
			{
				RobberyAI.RobberACurrentLootBagProp.Delete();
			}
			if (RobberyAI.RobberBCurrentLootBagProp != null)
			{
				RobberyAI.RobberBCurrentLootBagProp.Delete();
			}
			if (RobberyAI.RobberCCurrentLootBagProp != null)
			{
				RobberyAI.RobberCCurrentLootBagProp.Delete();
			}
			if (RobberyAI.RobberACurrentLootBagProp != null)
			{
				RobberyAI.RobberACurrentLootBagProp.Delete();
			}
			if (RobberyAI.PlayerCurrentLootBagProp != null)
			{
				RobberyAI.PlayerCurrentLootBagProp.Delete();
			}
			if (RobberyAI.RobberA_bagProp != null)
			{
				RobberyAI.RobberA_bagProp.Delete();
			}
			if (RobberyAI.RobberB_bagProp != null)
			{
				RobberyAI.RobberB_bagProp.Delete();
			}
			if (RobberyAI.RobberC_bagProp != null)
			{
				RobberyAI.RobberC_bagProp.Delete();
			}
			if (RobberyAI.RobberA_HoldingLootProp != null)
			{
				RobberyAI.RobberA_HoldingLootProp.Delete();
			}
			if (RobberyAI.RobberB_HoldingLootProp != null)
			{
				RobberyAI.RobberB_HoldingLootProp.Delete();
			}
			if (RobberyAI.RobberC_HoldingLootProp != null)
			{
				RobberyAI.RobberC_HoldingLootProp.Delete();
			}
			foreach (Prop item in RobberyAI.PaintingHolder)
			{
				if (item != null)
				{
					item.Delete();
				}
			}
			foreach (Prop painting in RobberyAI.Paintings)
			{
				if (painting != null)
				{
					painting.Delete();
				}
			}
			foreach (Prop smokeEmitter in RobberyAI.SmokeEmitters)
			{
				if (smokeEmitter != null)
				{
					smokeEmitter.Delete();
				}
			}
			Function.Call(Hash.REMOVE_PARTICLE_FX_FROM_ENTITY, Game.Player.Character);
			if (RobberyAI.MoneyProp != null)
			{
				RobberyAI.MoneyProp.Delete();
			}
			if (RobberyAI._cashGrabTray1 != null)
			{
				RobberyAI._cashGrabTray1.Delete();
			}
			if (RobberyAI.bagProp != null)
			{
				RobberyAI.bagProp.Delete();
			}
			foreach (Prop crate_Valuable in RobberyAI.Crate_Valuables)
			{
				if (crate_Valuable != null)
				{
					crate_Valuable.Delete();
				}
			}
			CurrentPay = 0;
			LocalTake = 0;
			C4ChargesCollected = 0;
			ThermalChargesCollected = 0;
			if (Keycard != null)
			{
				Keycard.Delete();
			}
			GotJuggKeyCard = false;
			if (Juggernaut != null)
			{
				Juggernaut.Delete();
			}
			foreach (LargeContainer item2 in RobbableContainer)
			{
				if (item2.CargoHeist_JuggernautLootContainer != null)
				{
					item2.CargoHeist_JuggernautLootContainer.DeleteEntites();
				}
				if (item2.FakeContainer != null)
				{
					item2.FakeContainer.Delete();
				}
				if (item2.Container != null)
				{
					item2.Container.Delete();
				}
				if (item2.swipecard != null)
				{
					item2.swipecard.Delete();
				}
			}
			if (Savage != null)
			{
				Savage.Delete();
			}
			GotThermalCharges = false;
			GotPanthereStatue = false;
			CurrentPay = 0;
			LocalTake = 0;
			if (CargoshipBlip != null)
			{
				CargoshipBlip.Delete();
			}
			if (CargoshipArea != null)
			{
				CargoshipArea.Delete();
			}
			foreach (Blip intelMarker in IntelMarkers)
			{
				if (intelMarker != null)
				{
					intelMarker.Alpha = 0;
				}
			}
			foreach (Blip item3 in IntelMarkersRadiusArea)
			{
				if (item3 != null)
				{
					item3.Alpha = 0;
				}
			}
			CreatedThermiteIntel = false;
			GotSavage = false;
			CreatedSavageIntel = false;
			CreatedGrinderIntel = false;
			if (BeachedKosatkaScript.Kosatka != null)
			{
				BeachedKosatkaScript.Kosatka.Delete();
			}
			CreatedKosatkaInterior = false;
			CargoshipHeistOn = false;
			GotManifest = false;
			GotJammer = false;
			GotGrinder = false;
			GotC4Crate = false;
			CreatedLootPlusMain = false;
			if (CargoHeist_Main != null)
			{
				CargoHeist_Main.DeleteEntites();
			}
			if (CargoHeist_Loot != null)
			{
				CargoHeist_Loot.DeleteEntites();
			}
			if (CargoHeist_Manifest_Jammer != null)
			{
				CargoHeist_Manifest_Jammer.DeleteEntites();
			}
			if (CargoHeist_Savage != null)
			{
				CargoHeist_Savage.DeleteEntites();
			}
			if (CargoHeist_Grinder != null)
			{
				CargoHeist_Grinder.DeleteEntites();
			}
			if (CargoHeist_Thermite != null)
			{
				CargoHeist_Thermite.DeleteEntites();
			}
			if (CargoHeist_ThermitePickups != null)
			{
				CargoHeist_ThermitePickups.DeleteEntites();
			}
			if (CargoHeist_KosatkaC4 != null)
			{
				CargoHeist_KosatkaC4.DeleteEntites();
			}
			if (CargoHeist_KosatkaInterior != null)
			{
				CargoHeist_KosatkaInterior.DeleteEntites();
			}
			foreach (Container container in Containers)
			{
				Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, container.PTFX2, false);
				Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, container.PTFX1, false);
				container.containerswitch = -100;
				if (container.Bomb != null)
				{
					container.Bomb.Delete();
				}
				container.BombExplodeTimer = 0;
				container.plantBombStage = 0;
				if (container.Bag != null)
				{
					container.Bag.Delete();
				}
				if (container.Saw != null)
				{
					container.Saw.Delete();
				}
				if (container.Container1 != null)
				{
					container.Container1.Delete();
				}
				if (container.FakeContainer != null)
				{
					container.FakeContainer.Delete();
				}
				if (container.Lock != null)
				{
					container.Lock.Delete();
				}
				Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, container.ParticleFx);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, container.SyncScene, 0f);
			}
			Containers.Clear();
			foreach (SmartGuards.SmartAiV2 item4 in SpawnGroupsUse)
			{
				if (item4.AIPed != null)
				{
					item4.AIPed.Delete();
				}
				if (item4.LastVehicle != null && item4.LastVehicle.GetPedOnSeat(VehicleSeat.Driver) != Game.Player.Character)
				{
					item4.LastVehicle.Delete();
				}
			}
			foreach (Ped merryweatherPed in MerryweatherPeds)
			{
				if (merryweatherPed != null)
				{
					merryweatherPed.Delete();
				}
			}
			foreach (Vehicle merryweatherVehicle in MerryweatherVehicles)
			{
				if (merryweatherVehicle != null)
				{
					merryweatherVehicle.Delete();
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
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, Shape);
		}

		private static void drawSprite2(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha)
		{
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, screenX, screenY, width, height, 0, r, g, b, alpha, 0);
			}
		}

		public static void TextNotification(string avatar, string author, string title, string message)
		{
			Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "CONFIRM_BEEP", "HUD_MINI_GAME_SOUNDSET");
			Function.Call(Hash.BEGIN_TEXT_COMMAND_THEFEED_POST, new InputArgument[1] { "STRING" });
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { message });
			int num = Function.Call<int>(Hash.END_TEXT_COMMAND_THEFEED_POST_MESSAGETEXT, new InputArgument[6] { avatar, avatar, true, 0, title, author });
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

		public void ShowTake(int Waittime)
		{
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f, 0.21f, 0.04f, 255, 255, 255, 130);
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f, 0.21f, 0.04f, 255, 255, 255, 130);
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.97f, 0.21f, 0.04f, 255, 255, 255, 130);
			drawText("Take ~g~" + Waittime.ToString("N"), 0.905f, 0.961f, 0.3f, 255, 255, 255);
		}

		private void OnTick(object sender, EventArgs e)
		{
			if (!CargoshipHeistOn)
			{
				return;
			}
			if (CargoshipHeistOn)
			{
				CargoShipStage heistStage = HeistStage;
				if (PathereStatueLoot != null)
				{
					GotPanthereStatue = PathereStatueLoot.IsRobbed;
				}
				if (GotPanthereStatue && GetDistance_VDIST2(Game.Player.Character.Position, CargoShipPos, ">", 300f))
				{
					Main.TotalCrewEarned += 0f;
					Main.TotalPlayerEarned += CurrentPay;
					XP.CompleteHeist_LaoubPrincessCargoship++;
					Main.SaveCuts();
					Class1.SetNoLongerInHeist();
					MissionScreen.SetPass("La'oub Princess Cargoship Heist", CurrentPay, 0, "The Player has died", 2, 0f, 5);
					XP.AddXP_Percent(XP.CompleteHeist_XPPercent_LaoubPrincessCargoship);
					int num = Game.GameTime + 30000;
					while (HKHModHelperNew.Class1.HeistPassedScreen != -1 && Game.GameTime <= num)
					{
						Script.Wait(0);
					}
					Script.Wait(2000);
					ManagePlayerFunds.AddFunds(CurrentPay);
					EndHeist();
					CurrentPay = 0;
					LocalTake = 0;
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, CargoShipPos, "<", 200f))
				{
					Function.Call(Hash.SET_RADAR_AS_INTERIOR_THIS_FRAME, Function.Call<int>(Hash.GET_HASH_KEY, "M23_2_DLC_INT_Ship"), ShipUIPos.X, ShipUIPos.Y, ShipUIHeading, 0);
				}
				if (Game.Player.IsAlive)
				{
					foreach (ObjectPlacementMap entity in CargoHeist_ThermitePickups.Entities)
					{
						if (!(entity.Entity != null) || !entity.Entity.IsVisible)
						{
							continue;
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, entity.Entity.Position, "<", 30f))
						{
							World.DrawMarker(MarkerType.Chevron1, entity.Entity.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), Vector3.Zero, new Vector3(0f, 180f, 0f), new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 255, 0), true, true);
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, entity.Entity.Position, "<", 2f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Pickup Thermite Charges", 1);
							if (Game.IsControlJustPressed(Control.Context))
							{
								if (entity.Entity.AttachedBlip != null)
								{
									entity.Entity.AttachedBlip.Alpha = 0;
								}
								EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(entity.Entity, 0));
								if (ThermalChargesCollected == 0)
								{
									IntelMarkers[5].Alpha = 0;
									IntelMarkersRadiusArea[5].Alpha = 0;
								}
								ThermalChargesCollected++;
								TextNotification("CHAR_HUMANDEFAULT", "~y~La'oub Princess Heist~w~", "Intel Found", "You have found Thermite Charges");
								GotThermalCharges = true;
								Script.Wait(1);
							}
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, entity.Entity.Position, "<", 6f) && entity.Entity.AttachedBlip == null)
						{
							entity.Entity.AddBlip();
							entity.Entity.AttachedBlip.Sprite = BlipSprite.FirePit;
							entity.Entity.AttachedBlip.Name = "Thermite";
							entity.Entity.AttachedBlip.Color = BlipColor.Green;
							entity.Entity.AttachedBlip.IsShortRange = true;
							entity.Entity.AttachedBlip.IsFlashing = true;
						}
					}
					if (HeistStage == CargoShipStage.Scout)
					{
						Screen.ShowSubtitle("Approach the ~y~La'oub Princess~w~ or Scout for more ~g~Assets/Intel~w~");
						if (GetDistance_VDIST2(Game.Player.Character.Position, CargoShipPos, "<", 300f))
						{
							HeistStage = CargoShipStage.NearCargoShip;
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, CargoHeist_Manifest_Jammer.Entities[0].Position, "<", 300f))
						{
							HeistStage = CargoShipStage.Manifest;
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, CargoHeist_Manifest_Jammer.Entities[1].Position, "<", 300f))
						{
							HeistStage = CargoShipStage.SamJammer;
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, GrinderIntelLoc, "<", 300f))
						{
							HeistStage = CargoShipStage.Grinder;
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, ThermiteIntelLoc, "<", 300f))
						{
							HeistStage = CargoShipStage.Thermite;
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, SavageIntelLoc, "<", 300f))
						{
							HeistStage = CargoShipStage.Savage;
						}
						if (BeachedKosatkaScript.Kosatka != null && GetDistance_VDIST2(Game.Player.Character.Position, BeachedKosatkaScript.Kosatka.Position, "<", 300f))
						{
							HeistStage = CargoShipStage.Kosatka;
						}
						if (BeachedKosatkaScript.IsInInterior)
						{
							HeistStage = CargoShipStage.KosatkaC4;
						}
					}
					if (HeistStage == CargoShipStage.Kosatka)
					{
						Screen.ShowSubtitle("Find a Way to enter the ~y~Kosatka~w~ to search for ~g~Assets/Intel~w~");
						if (BeachedKosatkaScript.Kosatka != null && GetDistance_VDIST2(Game.Player.Character.Position, BeachedKosatkaScript.Kosatka.Position, ">", 300f))
						{
							HeistStage = CargoShipStage.Scout;
						}
						if (BeachedKosatkaScript.IsInInterior)
						{
							HeistStage = CargoShipStage.KosatkaC4;
						}
					}
					if (HeistStage == CargoShipStage.KosatkaC4)
					{
						Screen.ShowSubtitle("Find the ~g~C4 Crate~w~ To blow open the ~y~Containers");
						if (!BeachedKosatkaScript.IsInInterior)
						{
							HeistStage = CargoShipStage.Kosatka;
						}
						if (!GotC4Crate && C4Crate != null)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, C4Crate.Position, "<", 30f))
							{
								World.DrawMarker(MarkerType.Chevron1, C4Crate.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), Vector3.Zero, new Vector3(0f, 180f, 0f), new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 255, 0), true, true);
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, C4Crate.Position, "<", 6f) && C4Crate.AttachedBlip == null)
							{
								C4Crate.AddBlip();
								C4Crate.AttachedBlip.Sprite = BlipSprite.Bomb;
								C4Crate.AttachedBlip.Name = "C4 Crate";
								C4Crate.AttachedBlip.Color = BlipColor.Green;
								C4Crate.AttachedBlip.IsShortRange = true;
								C4Crate.AttachedBlip.IsFlashing = true;
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, C4Crate.Position, "<", 2f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Pickup C4 Charges", 1);
								if (Game.IsControlJustPressed(Control.Context))
								{
									C4ChargesCollected = 20;
									if (C4Crate.AttachedBlip != null)
									{
										C4Crate.AttachedBlip.Alpha = 0;
									}
									EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(C4Crate, 0));
									IntelMarkers[2].Alpha = 0;
									IntelMarkersRadiusArea[2].Alpha = 0;
									TextNotification("CHAR_HUMANDEFAULT", "~y~La'oub Princess Heist~w~", "Intel Found", "You have found the C4 to blow open La'oub Princess Containers");
									GotC4Crate = true;
								}
							}
						}
					}
					if (HeistStage == CargoShipStage.CannotOpenContainer)
					{
						Screen.ShowSubtitle("Find the way to open the ~y~Container");
					}
					if (HeistStage == CargoShipStage.Manifest)
					{
						Screen.ShowSubtitle("Search the Area for a ~g~Manifest~w~ for the ~y~La'oub Princess Containers~w~ to figure out where the ~g~Panthere Statue~w~ is being kept.");
						if (GetDistance_VDIST2(Game.Player.Character.Position, CargoHeist_Manifest_Jammer.Entities[0].Position, ">", 300f) || GotManifest)
						{
							HeistStage = CargoShipStage.Scout;
						}
						if (!GotManifest && CargoHeist_Manifest_Jammer.Entities[0].Entity != null)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, CargoHeist_Manifest_Jammer.Entities[0].Entity.Position, "<", 30f))
							{
								World.DrawMarker(MarkerType.Chevron1, CargoHeist_Manifest_Jammer.Entities[0].Entity.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), Vector3.Zero, new Vector3(0f, 180f, 0f), new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 255, 0), true, true);
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, CargoHeist_Manifest_Jammer.Entities[0].Entity.Position, "<", 6f) && CargoHeist_Manifest_Jammer.Entities[0].Entity.AttachedBlip == null)
							{
								CargoHeist_Manifest_Jammer.Entities[0].Entity.AddBlip();
								CargoHeist_Manifest_Jammer.Entities[0].Entity.AttachedBlip.Sprite = BlipSprite.Document;
								CargoHeist_Manifest_Jammer.Entities[0].Entity.AttachedBlip.Name = "Manifest";
								CargoHeist_Manifest_Jammer.Entities[0].Entity.AttachedBlip.Color = BlipColor.Green;
								CargoHeist_Manifest_Jammer.Entities[0].Entity.AttachedBlip.IsShortRange = true;
								CargoHeist_Manifest_Jammer.Entities[0].Entity.AttachedBlip.IsFlashing = true;
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, CargoHeist_Manifest_Jammer.Entities[0].Entity.Position, "<", 2f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Pickup Manifest", 1);
								if (Game.IsControlJustPressed(Control.Context))
								{
									if (CargoHeist_Manifest_Jammer.Entities[0].Entity.AttachedBlip != null)
									{
										CargoHeist_Manifest_Jammer.Entities[0].Entity.AttachedBlip.Alpha = 0;
									}
									EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(CargoHeist_Manifest_Jammer.Entities[0].Entity, 0));
									IntelMarkers[3].Alpha = 0;
									IntelMarkersRadiusArea[3].Alpha = 0;
									TextNotification("CHAR_HUMANDEFAULT", "~y~La'oub Princess Heist~w~", "Intel Found", "You have found the Manifest for the La'oub Princess Containers");
									GotManifest = true;
								}
							}
						}
					}
					if (HeistStage == CargoShipStage.Savage)
					{
						Screen.ShowSubtitle("Search the Area for a ~b~Savage~w~ to board the ~y~La'oub Princess~w~ as a trojan horse.");
						if (GetDistance_VDIST2(Game.Player.Character.Position, SavageIntelLoc, ">", 300f) || GotSavage)
						{
							HeistStage = CargoShipStage.Scout;
						}
						if (Savage != null && !GotSavage && Game.Player.Character.CurrentVehicle == Savage)
						{
							GotSavage = true;
							IntelMarkers[0].Alpha = 0;
							IntelMarkersRadiusArea[0].Alpha = 0;
							TextNotification("CHAR_HUMANDEFAULT", "~y~La'oub Princess Heist~w~", "Intel Found", "You can use the Savage to board the  ~y~La'oub Princess~w~");
						}
					}
					if (HeistStage == CargoShipStage.Grinder)
					{
						Screen.ShowSubtitle("Search the Area for a ~g~Grinder~w~ to Open the ~y~La'oub Princess' Containers~w~ ");
						if (GetDistance_VDIST2(Game.Player.Character.Position, GrinderIntelLoc, ">", 300f) || GotGrinder)
						{
							HeistStage = CargoShipStage.Scout;
						}
						if (!GotGrinder && CargoHeist_Grinder.Entities[0].Entity != null)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, CargoHeist_Grinder.Entities[0].Entity.Position, "<", 30f))
							{
								World.DrawMarker(MarkerType.Chevron1, CargoHeist_Grinder.Entities[0].Entity.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), Vector3.Zero, new Vector3(0f, 180f, 0f), new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 255, 0), true, true);
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, CargoHeist_Grinder.Entities[0].Entity.Position, "<", 6f) && CargoHeist_Grinder.Entities[0].Entity.AttachedBlip == null)
							{
								CargoHeist_Grinder.Entities[0].Entity.AddBlip();
								CargoHeist_Grinder.Entities[0].Entity.AttachedBlip.Sprite = BlipSprite.BoltCutters;
								CargoHeist_Grinder.Entities[0].Entity.AttachedBlip.Name = "Grinder";
								CargoHeist_Grinder.Entities[0].Entity.AttachedBlip.Color = BlipColor.Green;
								CargoHeist_Grinder.Entities[0].Entity.AttachedBlip.IsShortRange = true;
								CargoHeist_Grinder.Entities[0].Entity.AttachedBlip.IsFlashing = true;
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, CargoHeist_Grinder.Entities[0].Entity.Position, "<", 2f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Pickup Grinder", 1);
								if (Game.IsControlJustPressed(Control.Context))
								{
									if (CargoHeist_Grinder.Entities[0].Entity.AttachedBlip != null)
									{
										CargoHeist_Grinder.Entities[0].Entity.AttachedBlip.Alpha = 0;
									}
									EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(CargoHeist_Grinder.Entities[0].Entity, 0));
									IntelMarkers[1].Alpha = 0;
									IntelMarkersRadiusArea[1].Alpha = 0;
									TextNotification("CHAR_HUMANDEFAULT", "~y~La'oub Princess Heist~w~", "Intel Found", "You have found the Grinder");
									GotGrinder = true;
								}
							}
						}
					}
					if (HeistStage == CargoShipStage.Thermite)
					{
						int thermalChargesCollected = ThermalChargesCollected;
						Screen.ShowSubtitle("Search the Area for ~g~Thermite Charges~w~ to Open the ~y~La'oub Princess' Containers~w~ ");
						if (GetDistance_VDIST2(Game.Player.Character.Position, ThermiteIntelLoc, ">", 300f) || GotThermalCharges)
						{
							HeistStage = CargoShipStage.Scout;
						}
					}
					if (HeistStage == CargoShipStage.NearCargoShip)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, CargoShipPos, ">", 300f))
						{
							HeistStage = CargoShipStage.Scout;
						}
						if (GotPanthereStatue)
						{
							Screen.ShowSubtitle("Search the ~y~La'oub Princess' Containers~w~ for the more ~g~Valuables~w~\nor Leave the Area");
						}
						else if (!GotC4Crate && !GotThermalCharges && !GotGrinder)
						{
							Screen.ShowSubtitle("Keep searching for more ~g~Assets/Intel~w~ as you have no way to open the ~y~La'oub Princess' Containers~w~ for the ~g~Panthere Statue");
						}
						else
						{
							Screen.ShowSubtitle("Search the ~y~La'oub Princess' Containers~w~ for the ~g~Panthere Statue");
						}
					}
					if (HeistStage == CargoShipStage.SamJammer)
					{
						Screen.ShowSubtitle("Search the Area for a ~g~Sam Jammer~w~ To disable ~r~La'oub Princess' SAMs");
						if (GetDistance_VDIST2(Game.Player.Character.Position, CargoHeist_Manifest_Jammer.Entities[1].Position, ">", 300f) || GotJammer)
						{
							HeistStage = CargoShipStage.Scout;
						}
						if (!GotJammer && CargoHeist_Manifest_Jammer.Entities[1].Entity != null)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, CargoHeist_Manifest_Jammer.Entities[0].Entity.Position, "<", 30f))
							{
								World.DrawMarker(MarkerType.Chevron1, CargoHeist_Manifest_Jammer.Entities[0].Entity.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), Vector3.Zero, new Vector3(0f, 180f, 0f), new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 255, 0), true, true);
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, CargoHeist_Manifest_Jammer.Entities[0].Position, "<", 6f) && CargoHeist_Manifest_Jammer.Entities[1].Entity.AttachedBlip == null)
							{
								CargoHeist_Manifest_Jammer.Entities[1].Entity.AddBlip();
								CargoHeist_Manifest_Jammer.Entities[1].Entity.AttachedBlip.Sprite = BlipSprite.ControlTower;
								CargoHeist_Manifest_Jammer.Entities[1].Entity.AttachedBlip.Name = "SAM Jammer";
								CargoHeist_Manifest_Jammer.Entities[1].Entity.AttachedBlip.Color = BlipColor.Green;
								CargoHeist_Manifest_Jammer.Entities[1].Entity.AttachedBlip.IsShortRange = true;
								CargoHeist_Manifest_Jammer.Entities[1].Entity.AttachedBlip.IsFlashing = true;
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, CargoHeist_Manifest_Jammer.Entities[1].Entity.Position, "<", 2f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Pickup SAM Jammer", 1);
								if (Game.IsControlJustPressed(Control.Context))
								{
									if (CargoHeist_Manifest_Jammer.Entities[1].Entity.AttachedBlip != null)
									{
										CargoHeist_Manifest_Jammer.Entities[1].Entity.AttachedBlip.Alpha = 0;
									}
									EntityFlickerEffect.VehicleFadeEffects.Add(new EntityFlickerEffect.FadeEffect(CargoHeist_Manifest_Jammer.Entities[1].Entity, 0));
									IntelMarkers[4].Alpha = 0;
									IntelMarkersRadiusArea[4].Alpha = 0;
									TextNotification("CHAR_HUMANDEFAULT", "~y~La'oub Princess Heist~w~", "Intel Found", "You have found the SAM Jammer");
									GotJammer = true;
								}
							}
						}
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
						ShowTextInfoBar("CurrentPay ", CurrentPay, 1, new Vector2(-0.05f, 0f), new Vector2(-0.05f, 0f), true);
						if (ThermalChargesCollected >= 1 && C4ChargesCollected == 0)
						{
							ShowTextInfoBar("Thermal Charges ", ThermalChargesCollected, 2, new Vector2(-0.05f, 0f), new Vector2(-0.05f, 0f), false);
						}
						if (ThermalChargesCollected == 0 && C4ChargesCollected >= 1)
						{
							ShowTextInfoBar("C4 Charges ", C4ChargesCollected, 2, new Vector2(-0.05f, 0f), new Vector2(-0.05f, 0f), false);
						}
						if (ThermalChargesCollected >= 1 && C4ChargesCollected >= 1)
						{
							ShowTextInfoBar("Thermal Charges ", ThermalChargesCollected, 2, new Vector2(-0.05f, 0f), new Vector2(-0.05f, 0f), false);
							ShowTextInfoBar("C4 Charges ", C4ChargesCollected, 3, new Vector2(-0.05f, 0f), new Vector2(-0.05f, 0f), false);
						}
					}
					if (GotManifest && ManifestContainerBlip != null)
					{
						ManifestContainerBlip.IsFlashing = true;
					}
					if (BeachedKosatkaScript.IsInInterior)
					{
						if (!CreatedKosatkaInterior)
						{
							CreatedKosatkaInterior = true;
							int index = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, CargoHeist_KosatkaC4.Entities.Count);
							CargoHeist_KosatkaC4.Entities[index].CreateObjatCoords();
							C4Crate = (Prop)CargoHeist_KosatkaC4.Entities[index].Entity;
							CargoHeist_KosatkaInterior.CreateEntites();
							foreach (ObjectPlacementMap entity2 in CargoHeist_KosatkaInterior.Entities)
							{
								if (entity2.Entity != null && entity2.Entity.Model.IsPed)
								{
									SpawnGroupsUse.Add(new SmartGuards.SmartAiV2((Ped)entity2.Entity, SmartGuards.EnemyGroup.Merryweather));
								}
							}
							SmartGuards.GuardGroup[2].GroupList.Clear();
							SmartGuards.GuardGroup[2].GroupList.AddRange(SpawnGroupsUse);
						}
						SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Merryweather;
					}
					else if (CreatedKosatkaInterior)
					{
						CreatedKosatkaInterior = false;
						CargoHeist_KosatkaC4.DeleteEntites();
						CargoHeist_KosatkaInterior.DeleteEntites();
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, SavageIntelLoc, "<", 400f))
					{
						if (!CreatedSavageIntel && !GotSavage)
						{
							Savage = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Savage), CargoHeist_Savage.Entities[0].Position, CargoHeist_Savage.Entities[0].Rotation.Z);
							for (int i = 1; i < CargoHeist_Savage.Entities.Count; i++)
							{
								CargoHeist_Savage.Entities[i].CreateObjatCoords();
							}
							Savage.IsPersistent = true;
							CreatedSavageIntel = true;
							foreach (ObjectPlacementMap entity3 in CargoHeist_Savage.Entities)
							{
								if (entity3.Entity != null && entity3.Entity.Model.IsPed)
								{
									entity3.Entity.IsPositionFrozen = false;
									SpawnGroupsUse.Add(new SmartGuards.SmartAiV2((Ped)entity3.Entity, SmartGuards.EnemyGroup.Cartel));
								}
							}
							SmartGuards.GuardGroup[5].GroupList.Clear();
							SmartGuards.GuardGroup[5].GroupList.AddRange(SpawnGroupsUse);
						}
						SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Cartel;
					}
					else if (CreatedSavageIntel && !GotSavage)
					{
						CargoHeist_Savage.DeleteEntites();
						CreatedSavageIntel = false;
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, GrinderIntelLoc, "<", 400f))
					{
						if (!CreatedGrinderIntel && !GotGrinder)
						{
							CargoHeist_Grinder.CreateEntites();
							CreatedGrinderIntel = true;
							foreach (ObjectPlacementMap entity4 in CargoHeist_Grinder.Entities)
							{
								if (entity4.Entity != null && entity4.Entity.Model.IsPed)
								{
									SpawnGroupsUse.Add(new SmartGuards.SmartAiV2((Ped)entity4.Entity, SmartGuards.EnemyGroup.Cartel));
								}
							}
							SmartGuards.GuardGroup[5].GroupList.Clear();
							SmartGuards.GuardGroup[5].GroupList.AddRange(SpawnGroupsUse);
						}
						SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Cartel;
					}
					else if (CreatedGrinderIntel)
					{
						CargoHeist_Grinder.DeleteEntites();
						CreatedGrinderIntel = false;
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, ThermiteIntelLoc, "<", 400f))
					{
						if (!CreatedThermiteIntel && !GotThermalCharges)
						{
							CargoHeist_Thermite.CreateEntites();
							CreatedThermiteIntel = true;
							foreach (ObjectPlacementMap entity5 in CargoHeist_Thermite.Entities)
							{
								if (entity5.Entity != null && entity5.Entity.Model.IsPed)
								{
									SpawnGroupsUse.Add(new SmartGuards.SmartAiV2((Ped)entity5.Entity, SmartGuards.EnemyGroup.Cartel));
								}
							}
							SmartGuards.GuardGroup[5].GroupList.Clear();
							SmartGuards.GuardGroup[5].GroupList.AddRange(SpawnGroupsUse);
						}
						SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Cartel;
					}
					else if (CreatedThermiteIntel)
					{
						CargoHeist_Thermite.DeleteEntites();
						CreatedThermiteIntel = false;
					}
					Vector3 position = Game.Player.Character.Position;
					foreach (Container container in Containers)
					{
						Vector3 offsetPosition = container.Container1.GetOffsetPosition(new Vector3(0f, -1.8f, 1.2f));
						if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition, "<", 3f))
						{
							if (GetDistance_VDIST2(CurrentContainer, offsetPosition, ">", 3f))
							{
								CurrentContainer = offsetPosition;
							}
							else if (ThermalChargesCollected == 0 && !GotGrinder && ThermalChargesCollected == 0)
							{
								HeistStage = CargoShipStage.CannotOpenContainer;
							}
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, CurrentContainer, ">", 3f) && HeistStage == CargoShipStage.CannotOpenContainer)
						{
							HeistStage = CargoShipStage.NearCargoShip;
						}
						if (container.containerswitch >= 0 && container.plantBombStage >= 0)
						{
							if (container.Container1 != null)
							{
								if (container.Lock == null)
								{
									container.Lock = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_lock_01a"), container.Container1.GetOffsetPosition(new Vector3(0f, -1.8f, 1.2f)), new Vector3(0f, 0f, container.Container1.Rotation.Z), false, false);
								}
								if (GetDistance_VDIST2(container.Container1.Position, Game.Player.Character.Position, "<", 30f))
								{
									if (container.Container1.AttachedBlip != null)
									{
										container.Container1.AttachedBlip.Alpha = 255;
									}
								}
								else if (container.Container1.AttachedBlip != null)
								{
									container.Container1.AttachedBlip.Alpha = 0;
								}
							}
						}
						else if (container.Container1.AttachedBlip != null)
						{
							container.Container1.AttachedBlip.Alpha = 0;
						}
						if (GotGrinder)
						{
							switch (container.containerswitch)
							{
							case 0:
							{
								if (!(Game.Player.Character.Position.DistanceTo(new Vector3(container.Lock.Position.X, container.Lock.Position.Y, container.Lock.Position.Z)) < 2f))
								{
									break;
								}
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to break lock", 1);
								if (!Game.IsControlJustPressed(Control.Context))
								{
									break;
								}
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
								Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_CM2022/CM2022_GENERIC_01");
								Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_TUNER/DLC_Tuner_Generic");
								Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_CM2022/CM2022_GENERIC_01", false);
								Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_TUNER/DLC_Tuner_Generic", false);
								Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", "action", container.Container1.Position.X, container.Container1.Position.Y, container.Container1.Position.Z, 0f, 0f, container.Container1.Rotation.Z, 0f, 2);
								Vector3 vector2 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", "action", container.Container1.Position.X, container.Container1.Position.Y, container.Container1.Position.Z, 0f, 0f, container.Container1.Rotation.Z, 0f, 2);
								Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@");
								Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_CM2022/CM2022_GENERIC_01");
								Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_TUNER/DLC_Tuner_Generic");
								Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_CM2022/CM2022_GENERIC_01", false);
								Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_TUNER/DLC_Tuner_Generic", false);
								Model model = new Model("ch_p_m_bag_var04_arm_s");
								model.Request(10000);
								if (model.IsValid && model.IsInCdImage)
								{
									while (!model.IsLoaded)
									{
										Script.Wait(50);
									}
									container.Bag = GlobalHelper.CreateProp(model, Game.Player.Character.Position, false, false);
									container.Bag.IsCollisionEnabled = false;
									container.containerswitch = 10;
								}
								Model model2 = new Model("tr_prop_tr_grinder_01a");
								model2.Request(100);
								if (model2.IsValid && model2.IsInCdImage)
								{
									while (!model2.IsLoaded)
									{
										Script.Wait(50);
									}
									container.Saw = GlobalHelper.CreateProp(model2, Game.Player.Character.Position, false, false);
									container.Saw.IsCollisionEnabled = false;
									container.containerswitch = 10;
								}
								Model model3 = new Model("tr_prop_tr_cont_coll_01a");
								model3.Request(100);
								if (model3.IsValid && model3.IsInCdImage)
								{
									while (!model3.IsLoaded)
									{
										Script.Wait(50);
									}
									container.FakeContainer = GlobalHelper.CreateProp(model3, container.Container1.Position, false, false);
									container.FakeContainer.Rotation = new Vector3(0f, 0f, container.Container1.Rotation.X);
									container.FakeContainer.IsCollisionEnabled = false;
									container.containerswitch = 10;
								}
								break;
							}
							case 10:
								Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@");
								while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@"))
								{
									Script.Wait(0);
								}
								Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_CM2022/CM2022_GENERIC_01");
								Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_TUNER/DLC_Tuner_Generic");
								Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_CM2022/CM2022_GENERIC_01", false);
								Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_TUNER/DLC_Tuner_Generic", false);
								Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@");
								container.SyncScene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, container.Container1.Position.X, container.Container1.Position.Y, container.Container1.Position.Z, 0f, 0f, container.Container1.Rotation.Z, 2);
								container.SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, container.Container1.Position.X, container.Container1.Position.Y, container.Container1.Position.Z, 0f, 0f, container.Container1.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, container.SyncScene2, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", "action", 4f, -15f, 3341, 16, 1148846080, 0);
								Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container.Bag, container.SyncScene2, "action_bag", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container.Lock, container.SyncScene, "action_lock", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container.Saw, container.SyncScene2, "action_angle_grinder", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container.FakeContainer, container.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container.Container1, container.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
								container.ParticleTimer = Game.GameTime + 4000;
								Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_tn_tr");
								container.containerswitch = 20;
								break;
							case 20:
								if (Game.GameTime > container.ParticleTimer)
								{
									Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_tn_tr");
									Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_tn_tr");
									container.ParticleFx = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_tn_tr_angle_grinder_sparks", container.Saw, 0f, 0.25f, 0f, 0f, 0f, 0f, 1f, false, false, false);
									container.ParticleTimer = Game.GameTime + 1000;
									container.containerswitch = 21;
								}
								break;
							case 21:
								if (Game.GameTime > container.ParticleTimer)
								{
									Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, container.ParticleFx);
									container.containerswitch = 30;
								}
								break;
							case 30:
								if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, container.SyncScene2) > 0.99f)
								{
									container.containerswitch = -100;
									if (Game.Player.Character.Model == PedHash.Trevor)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 1, 0, 1);
									}
									if (Game.Player.Character.Model == PedHash.Michael)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 2, 0, 1);
									}
									if (Game.Player.Character.Model == PedHash.Franklin)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 6, 0, 1);
									}
									Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, container.ParticleFx);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, container.SyncScene, 0f);
									container.Bag.Delete();
									container.Saw.Delete();
									container.Container1.IsCollisionEnabled = false;
									container.FakeContainer.IsCollisionEnabled = true;
									Game.Player.Character.Task.ClearAll();
								}
								break;
							case -100:
								if (container.Container1.AttachedBlip != null)
								{
									container.Container1.AttachedBlip.Alpha = 0;
								}
								container.containerswitch = -1;
								break;
							}
						}
						else if (!GotGrinder && GotC4Crate)
						{
							Vector3 offsetPosition2 = container.Container1.GetOffsetPosition(new Vector3(0f, -1.8f, 1.5f));
							Vector3 rotation = container.Container1.Rotation;
							switch (container.plantBombStage)
							{
							case 0:
							{
								if (!(Game.Player.Character.Position.DistanceTo(new Vector3(offsetPosition2.X, offsetPosition2.Y, offsetPosition2.Z)) < 1.5f) || C4ChargesCollected < 1)
								{
									break;
								}
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to plant a ~r~bomb.", 1);
								if (!Game.IsControlJustPressed(Control.Context))
								{
									break;
								}
								C4ChargesCollected--;
								LoadDict("anim@scripted@heist@ig11_bomb_plant@male@");
								LoadDict("anim@scripted@player@mission@tun_bomb_plant@heeled@");
								Model model4 = new Model("ch_prop_ch_ld_bomb_01a");
								model4.Request(250);
								if (model4.IsValid && model4.IsInCdImage)
								{
									while (!model4.IsLoaded)
									{
										Script.Wait(50);
									}
									container.Bomb = GlobalHelper.CreateProp(model4, Game.Player.Character.Position, false, false);
									container.Bomb.IsCollisionEnabled = false;
									container.plantBombStage = 10;
								}
								Model model5 = new Model("hei_p_m_bag_var22_arm_s");
								model5.Request(250);
								if (model5.IsValid && model5.IsInCdImage)
								{
									while (!model5.IsLoaded)
									{
										Script.Wait(50);
									}
									container.Bag = GlobalHelper.CreateProp(model5, Game.Player.Character.Position, false, false);
									container.Bag.IsCollisionEnabled = false;
									container.plantBombStage = 10;
								}
								break;
							}
							case 10:
								LoadDict("anim@scripted@heist@ig11_bomb_plant@male@");
								LoadDict("anim@scripted@player@mission@tun_bomb_plant@heeled@");
								container.SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition2.X, offsetPosition2.Y, offsetPosition2.Z + 0.15f, 0f, 0f, rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, container.SyncScene, "anim@scripted@player@mission@tun_bomb_plant@heeled@", "enter", 4f, -15f, 3341, 16, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container.Bomb, container.SyncScene, "enter_bomb", "anim@scripted@heist@ig11_bomb_plant@male@", 1f, -1f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container.Bag, container.SyncScene, "enter_bag", "anim@scripted@heist@ig11_bomb_plant@male@", 1f, -1f, 0, 1148846080);
								Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
								Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
								container.plantBombStage = 20;
								break;
							case 20:
								if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, container.SyncScene) > 0.99f)
								{
									container.Bag.Delete();
									Game.Player.Character.Task.ClearAll();
									container.BombExplodeTimer = Game.GameTime + 5000;
									container.plantBombStage = -1;
								}
								break;
							case -1:
								if (Game.GameTime >= container.BombExplodeTimer)
								{
									World.AddExplosion(container.Bomb.Position, ExplosionType.PipeBomb, 1f, 1f, Game.Player.Character);
									if (container.Bomb != null)
									{
										container.Bomb.Delete();
									}
									container.FakeContainer = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_cont_coll_01a"), container.Container1.Position, false, false);
									container.FakeContainer.Rotation = new Vector3(0f, 0f, container.Container1.Rotation.X);
									container.FakeContainer.IsCollisionEnabled = false;
									LoadDict("anim@scripted@player@mission@tunf_train_ig1_container_p1@male@");
									Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@");
									container.SyncScene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, container.Container1.Position.X, container.Container1.Position.Y, container.Container1.Position.Z, 0f, 0f, container.Container1.Rotation.Z, 2);
									container.SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, container.Container1.Position.X, container.Container1.Position.Y, container.Container1.Position.Z, 0f, 0f, container.Container1.Rotation.Z, 2);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container.FakeContainer, container.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container.Container1, container.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
									if (container.Lock != null)
									{
										container.Lock.Delete();
									}
									if (container.Bomb != null)
									{
										container.Bomb.Delete();
									}
									container.plantBombStage = -2;
								}
								break;
							case -2:
								container.Container1.IsCollisionEnabled = false;
								container.FakeContainer.IsCollisionEnabled = true;
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, container.SyncScene, 1f);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, container.SyncScene2, 1f);
								break;
							}
						}
						else
						{
							if (GotGrinder || GotC4Crate || !GotThermalCharges)
							{
								continue;
							}
							Vector3 offsetPosition3 = container.Container1.GetOffsetPosition(new Vector3(0f, -1.85f, 1.2f));
							Vector3 rotation2 = container.Container1.Rotation;
							switch (container.plantBombStage)
							{
							case 0:
								if (!(Game.Player.Character.Position.DistanceTo(new Vector3(offsetPosition3.X, offsetPosition3.Y, offsetPosition3.Z)) < 1.5f) || ThermalChargesCollected < 1)
								{
									break;
								}
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to plant a ~r~Thermal Charge.", 1);
								if (Game.IsControlJustPressed(Control.Context))
								{
									ThermalChargesCollected--;
									Ped character2 = Game.Player.Character;
									if (Game.Player.Character.Model == PedHash.Trevor)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 9, 0, 0, 1);
									}
									if (Game.Player.Character.Model == PedHash.Michael)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 9, 0, 0, 1);
									}
									if (Game.Player.Character.Model == PedHash.Franklin)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 9, 0, 0, 1);
									}
									string dict = "anim_heist@hs3f@ig13_thermal_charge@thermal_charge@male@";
									string dict2 = "anim@heists@ornate_bank@thermal_charge";
									LoadDict(dict);
									LoadDict(dict);
									LoadDict(dict2);
									LoadDict(dict2);
									if (container.Bomb != null)
									{
										container.Bomb.Delete();
									}
									if (container.Bag != null)
									{
										container.Bag.Delete();
									}
									container.Bag = GlobalHelper.CreateProp(RequestModel("ch_p_m_bag_var02_arm_s"), offsetPosition3, new Vector3(1f, 0f, 0f), false, false);
									container.SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition3.X, offsetPosition3.Y, offsetPosition3.Z, 0.0, 0.0, rotation2.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, container.SyncScene, LoadDict("anim@heists@ornate_bank@thermal_charge"), "thermal_charge", 1f, -1f, 3341, 16, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container.Bag, container.SyncScene, "bag_thermal_charge", LoadDict("anim@heists@ornate_bank@thermal_charge"), 1f, -1f, 0, 1148846080);
									Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, container.Bag);
									container.Bomb = GlobalHelper.CreateProp(RequestModel("hei_prop_heist_thermite"), Game.Player.Character.Position, false, false);
									Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, container.Bomb, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, -90f, 0f, 0f, 0, 0, 0, 0, 2, 1);
									Function.Call(Hash.PLAY_ENTITY_ANIM, container.Bomb, "bomb_thermal_charge", "anim@heists@ornate_bank@thermal_charge", 1000.0, 1, 1, 1, 0, 0);
									container.plantBombStage = 10;
								}
								break;
							case 10:
								if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, container.SyncScene) > 0.5f)
								{
									container.Bomb.Detach();
									container.Bomb.IsPositionFrozen = true;
									Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ornate_heist");
									Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ornate_heist");
									container.PTFX1 = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_heist_ornate_thermal_burn", container.Bomb, 0f, 0f, 0f, 1f, 0, 0, 0);
									Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
									Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
									container.PTFX2 = GlobalHelper.CreatePTFX_LoopedOnEntity("proj_flare_fuse_fp", container.Bomb, 0f, 0f, 0f, 10f, 0, 0, 0);
									Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
									container.plantBombStage = 20;
								}
								break;
							case 20:
								if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, container.SyncScene) > 0.99f)
								{
									container.Bag.Delete();
									container.Bomb.IsPositionFrozen = true;
									Game.Player.Character.Task.ClearAll();
									container.BombExplodeTimer = Game.GameTime + 7500;
									container.plantBombStage = -1;
								}
								break;
							case -1:
								if (Game.GameTime >= container.BombExplodeTimer)
								{
									Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, container.PTFX2, false);
									Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, container.PTFX1, false);
									Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "Gate_Lock_Break", container.Bomb, "DLC_HEISTS_ORNATE_BANK_FINALE_SOUNDS", 0, 0);
									if (container.Bomb != null)
									{
										container.Bomb.Delete();
									}
									container.FakeContainer = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_cont_coll_01a"), container.Container1.Position, false, false);
									container.FakeContainer.Rotation = new Vector3(0f, 0f, container.Container1.Rotation.X);
									container.FakeContainer.IsCollisionEnabled = false;
									LoadDict("anim@scripted@player@mission@tunf_train_ig1_container_p1@male@");
									Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@");
									container.SyncScene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, container.Container1.Position.X, container.Container1.Position.Y, container.Container1.Position.Z, 0f, 0f, container.Container1.Rotation.Z, 2);
									container.SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, container.Container1.Position.X, container.Container1.Position.Y, container.Container1.Position.Z, 0f, 0f, container.Container1.Rotation.Z, 2);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container.FakeContainer, container.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, container.Container1, container.SyncScene, "action_container", "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@", 1f, -1f, 0, 1148846080);
									if (container.Lock != null)
									{
										container.Lock.Delete();
									}
									if (container.Bomb != null)
									{
										container.Bomb.Delete();
									}
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, container.SyncScene, 0.65f);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, container.SyncScene2, 0.65f);
									container.plantBombStage = -2;
								}
								break;
							case -2:
								container.Container1.IsCollisionEnabled = false;
								container.FakeContainer.IsCollisionEnabled = true;
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, container.SyncScene, 0.65f);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, container.SyncScene2, 0.65f);
								break;
							}
						}
					}
					if (!GotJammer)
					{
						foreach (SamSite samSite in SamSites)
						{
							if (samSite.Sam != null)
							{
								if (samSite.Sam.AttachedBlip == null)
								{
									samSite.Sam.AddBlip();
									if (samSite.Sam.AttachedBlip != null)
									{
										samSite.Sam.AttachedBlip.Sprite = BlipSprite.SAMTurret;
										samSite.Sam.AttachedBlip.Color = BlipColor.Red;
										samSite.Sam.AttachedBlip.Name = "Surface to Air Missile Site";
										samSite.Sam.AttachedBlip.IsShortRange = true;
									}
								}
								if (samSite.Sam.AttachedBlip != null)
								{
									if (GetDistance_VDIST2(Game.Player.Character.Position, samSite.Sam.Position, "<", 300f))
									{
										if (!samSite.IsDead)
										{
											samSite.Sam.AttachedBlip.Alpha = 255;
										}
										if (samSite.IsDead)
										{
											samSite.Sam.AttachedBlip.Alpha = 0;
										}
									}
									else
									{
										samSite.Sam.AttachedBlip.Alpha = 0;
									}
								}
							}
							if (!GetDistance_VDIST2(Game.Player.Character.Position, samSite.Sam.Position, "<", 350f))
							{
								continue;
							}
							if (samSite.Sam.Health <= 350 && samSite.Sam.AttachedBlip != null)
							{
								samSite.IsDead = true;
								samSite.Sam.AttachedBlip.Alpha = 0;
							}
							if (!(Game.Player.Character.CurrentVehicle != null))
							{
								continue;
							}
							if (!samSite.IsDead)
							{
								if (!(Game.Player.Character.CurrentVehicle != null) || !(Game.Player.Character.CurrentVehicle != Savage) || !(Game.Player.Character.CurrentVehicle != null))
								{
									continue;
								}
								if (Game.Player.Character.CurrentVehicle.Position.Z > samSite.Sam.Position.Z - 5f && (Game.Player.Character.CurrentVehicle.Model.IsPlane || Game.Player.Character.CurrentVehicle.Model.IsHelicopter) && samSite.fireDelay < Game.GameTime)
								{
									samSite.roundsFired++;
									if (samSite.roundsFired < 6)
									{
										samSite.fireDelay = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 2500, 7500);
									}
									if (samSite.roundsFired >= 6)
									{
										samSite.roundsFired = 0;
										samSite.fireDelay = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 10000, 30000);
									}
									Vector3 offsetPosition4 = samSite.Sam.GetOffsetPosition(new Vector3(0f, 0f, 10f));
									Vector3 position2 = Game.Player.Character.CurrentVehicle.Position;
									Function.Call(Hash.SHOOT_SINGLE_BULLET_BETWEEN_COORDS_IGNORE_ENTITY_NEW, offsetPosition4.X, offsetPosition4.Y, offsetPosition4.Z, position2.X, position2.Y, position2.Z, 1000, false, Function.Call<int>(Hash.GET_HASH_KEY, "VEHICLE_WEAPON_PLANE_ROCKET"), 0, true, true, -1f, 0, true, true, Game.Player.Character.CurrentVehicle, false, 0, 0, 0);
								}
								Function.Call(Hash.SET_VEHICLE_WEAPON_CAN_TARGET_OBJECTS, Game.Player.Character.CurrentVehicle, true);
							}
							else
							{
								Function.Call(Hash.SET_OBJECT_TARGETTABLE, samSite.Sam, false);
								Function.Call(Hash.SET_ENTITY_IS_TARGET_PRIORITY, samSite.Sam, false, 0f);
							}
						}
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, CayoPericoPos, "<", 3000f))
					{
						SmartGuards.GetCurrentGroupSettings().ShowCone = true;
						SmartGuards.GetCurrentGroupSettings().ShowBlipRadius = 100f;
						SmartGuards.GetCurrentGroupSettings().Radius = 150f;
						SmartGuards.GetCurrentGroupSettings().SeeingAngle = 35f;
						SmartGuards.GetCurrentGroupSettings().AllowInvestigate = true;
						SmartGuards.GetCurrentGroupSettings().InvestigateDist = 20f;
						SmartGuards.GetCurrentGroupSettings().HitRadius = 30f;
						SmartGuards.GetCurrentGroupSettings().ArrestInsteadOfShoot = false;
						SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
						SmartGuards.GetCurrentGroupSettings().ShowBlipHeightAdd = 15f;
						SmartGuards.GetCurrentGroupSettings().ShowBlipHeightMinus = 15f;
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, CargoShipPos, "<", 300f))
					{
						SmartGuards.GetCurrentGroupSettings().ShowCone = true;
						SmartGuards.GetCurrentGroupSettings().ShowBlipRadius = 100f;
						SmartGuards.GetCurrentGroupSettings().Radius = 50f;
						SmartGuards.GetCurrentGroupSettings().SeeingAngle = 45f;
						SmartGuards.GetCurrentGroupSettings().AllowInvestigate = true;
						SmartGuards.GetCurrentGroupSettings().InvestigateDist = 10f;
						SmartGuards.GetCurrentGroupSettings().HitRadius = 20f;
						SmartGuards.GetCurrentGroupSettings().ArrestInsteadOfShoot = false;
						SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
						SmartGuards.GetCurrentGroupSettings().ShowBlipHeightAdd = 15f;
						SmartGuards.GetCurrentGroupSettings().ShowBlipHeightMinus = 15f;
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, CayoPericoPos, ">", 3000f) && GetDistance_VDIST2(Game.Player.Character.Position, CargoShipPos, ">", 300f) && !BeachedKosatkaScript.IsInInterior)
					{
						SmartGuards.GetCurrentGroupSettings().ShowCone = true;
						SmartGuards.GetCurrentGroupSettings().ShowBlipRadius = 100f;
						SmartGuards.GetCurrentGroupSettings().Radius = 120f;
						SmartGuards.GetCurrentGroupSettings().SeeingAngle = 35f;
						SmartGuards.GetCurrentGroupSettings().AllowInvestigate = true;
						SmartGuards.GetCurrentGroupSettings().InvestigateDist = 10f;
						SmartGuards.GetCurrentGroupSettings().HitRadius = 30f;
						SmartGuards.GetCurrentGroupSettings().ArrestInsteadOfShoot = false;
						SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
						SmartGuards.GetCurrentGroupSettings().ShowBlipHeightAdd = 15f;
						SmartGuards.GetCurrentGroupSettings().ShowBlipHeightMinus = 15f;
					}
					if (BeachedKosatkaScript.IsInInterior)
					{
						SmartGuards.GetCurrentGroupSettings().ShowCone = true;
						SmartGuards.GetCurrentGroupSettings().ShowBlipRadius = 100f;
						SmartGuards.GetCurrentGroupSettings().Radius = 20f;
						SmartGuards.GetCurrentGroupSettings().SeeingAngle = 35f;
						SmartGuards.GetCurrentGroupSettings().AllowInvestigate = false;
						SmartGuards.GetCurrentGroupSettings().InvestigateDist = 2f;
						SmartGuards.GetCurrentGroupSettings().HitRadius = 10f;
						SmartGuards.GetCurrentGroupSettings().ArrestInsteadOfShoot = false;
						SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
						SmartGuards.GetCurrentGroupSettings().UseFollowNavMeshRoute = true;
						SmartGuards.GetCurrentGroupSettings().ShowBlipHeightAdd = 15f;
						SmartGuards.GetCurrentGroupSettings().ShowBlipHeightMinus = 15f;
					}
				}
				else
				{
					HKHModHelperNew.Class1.scaleformDescription = "La'oub Princess cargoship Heist was failed cause the player died";
					HKHModHelperNew.Class1.scaleformTitle = "La'oub Princess cargoship Heist was Failed";
					HKHModHelperNew.Class1.ScaleformColor = HKHModHelperNew.Class1.ScaleformColour.Red;
					HKHModHelperNew.Class1.ShowScaleform = true;
					HKHModHelperNew.Class1.Passed_Index = 0;
					EndHeist();
				}
			}
			if (SpawnGroupsUse.Count > 0)
			{
				if (GetDistance_VDIST2(Game.Player.Character.Position, CayoPericoPos, "<", 3000f))
				{
					SmartGuards.GetCurrentGroupSettings().ShowCone = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipRadius = 100f;
					SmartGuards.GetCurrentGroupSettings().Radius = 150f;
					SmartGuards.GetCurrentGroupSettings().SeeingAngle = 35f;
					SmartGuards.GetCurrentGroupSettings().AllowInvestigate = true;
					SmartGuards.GetCurrentGroupSettings().InvestigateDist = 20f;
					SmartGuards.GetCurrentGroupSettings().HitRadius = 30f;
					SmartGuards.GetCurrentGroupSettings().ArrestInsteadOfShoot = false;
					SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightAdd = 15f;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightMinus = 15f;
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, CargoShipPos, "<", 300f))
				{
					SmartGuards.GetCurrentGroupSettings().ShowCone = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipRadius = 100f;
					SmartGuards.GetCurrentGroupSettings().Radius = 50f;
					SmartGuards.GetCurrentGroupSettings().SeeingAngle = 45f;
					SmartGuards.GetCurrentGroupSettings().AllowInvestigate = true;
					SmartGuards.GetCurrentGroupSettings().InvestigateDist = 10f;
					SmartGuards.GetCurrentGroupSettings().HitRadius = 20f;
					SmartGuards.GetCurrentGroupSettings().ArrestInsteadOfShoot = false;
					SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightAdd = 15f;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightMinus = 15f;
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, CayoPericoPos, ">", 3000f) && GetDistance_VDIST2(Game.Player.Character.Position, CargoShipPos, ">", 300f) && !BeachedKosatkaScript.IsInInterior)
				{
					SmartGuards.GetCurrentGroupSettings().ShowCone = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipRadius = 100f;
					SmartGuards.GetCurrentGroupSettings().Radius = 120f;
					SmartGuards.GetCurrentGroupSettings().SeeingAngle = 35f;
					SmartGuards.GetCurrentGroupSettings().AllowInvestigate = true;
					SmartGuards.GetCurrentGroupSettings().InvestigateDist = 10f;
					SmartGuards.GetCurrentGroupSettings().HitRadius = 30f;
					SmartGuards.GetCurrentGroupSettings().ArrestInsteadOfShoot = false;
					SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightAdd = 15f;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightMinus = 15f;
				}
				if (BeachedKosatkaScript.IsInInterior)
				{
					SmartGuards.GetCurrentGroupSettings().ShowCone = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipRadius = 100f;
					SmartGuards.GetCurrentGroupSettings().Radius = 20f;
					SmartGuards.GetCurrentGroupSettings().SeeingAngle = 35f;
					SmartGuards.GetCurrentGroupSettings().AllowInvestigate = false;
					SmartGuards.GetCurrentGroupSettings().InvestigateDist = 2f;
					SmartGuards.GetCurrentGroupSettings().HitRadius = 10f;
					SmartGuards.GetCurrentGroupSettings().ArrestInsteadOfShoot = false;
					SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
					SmartGuards.GetCurrentGroupSettings().UseFollowNavMeshRoute = true;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightAdd = 15f;
					SmartGuards.GetCurrentGroupSettings().ShowBlipHeightMinus = 15f;
				}
			}
		}
	}
}
