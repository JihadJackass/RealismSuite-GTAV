using System;
using System.Collections.Generic;
using System.Drawing;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace SmartAIGuards
{
	public class SmartGuards : Script
	{
		public enum EnemyGroup
		{
			None = -1,
			Enemy,
			Eberhard_Munitions,
			Merryweather,
			Diamond_Casino_Security,
			Cayo_Perico_Cartel,
			Cartel,
			Professionals,
			Military,
			Marabaunte_Grande,
			Aztecas,
			Gang,
			Hired_Gunmen,
			FIB,
			Security,
			Lost_MC,
			Vagos,
			Ballas,
			Families,
			Triads,
			Hippies,
			Rednecks,
			Kkangpae,
			Altruist_Cult,
			Armenian_Mob,
			High_End_Security,
			Swat,
			LSPD,
			Gruppe_Sechs,
			IAA,
			Street_Racers
		}

		public class EnemyGroupData
		{
			public static EnemyGroup Group { get; set; }

			public SmartAI_Settings Settings { get; set; } = CreateSettingsForGroup(Group);


			public List<SmartAiV2> GroupList { get; set; } = new List<SmartAiV2>();


			public List<SmartAiV2> ReinforcementsList { get; set; } = new List<SmartAiV2>();


			public EnemyGroupData()
			{
			}

			public EnemyGroupData(EnemyGroup G, List<SmartAiV2> List)
			{
				Group = G;
				GroupList = List;
				ReinforcementsList = new List<SmartAiV2>();
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

		public class SmartAiV2 : Script
		{
			public enum VehicleType
			{
				Land,
				Heli,
				Plane,
				Boat
			}

			public bool SpawnedDeadBodyMarker { get; set; }

			public bool CanSeeDeadBody { get; set; }

			public int SeenDeadBodyTimemeTillAlert { get; set; }

			public bool HaveFoundDeadBody { get; set; }

			public int TimeSinceStartCombat { get; set; }

			public bool IsInvestigatingSightingOfPlayer { get; set; }

			public int StopInvestigateTime { get; set; }

			public FiringPattern DefaultFiringPattern { get; set; } = FiringPattern.BurstFire;


			public bool ForceStayInVehicle { get; set; }

			public bool IsinCombatAgainstTarget { get; set; }

			public int SpawnSafeTimer { get; set; }

			public bool WasDriver { get; set; }

			public int PlayerInVisualRangeTimer { get; set; }

			public EnemyGroup EnemyGroupName { get; set; }

			public bool TargetImVisibleZone { get; set; }

			public bool MarkasElite { get; set; }

			public int AIAnimScenePhase { get; set; }

			public int AIAnimSceneStep { get; set; }

			public bool TurnOffVisibleDetection { get; set; }

			public bool GroupActive { get; set; }

			public bool IsCayoPericoJugg { get; set; }

			public bool isDead { get; set; }

			public int ShootDelay { get; set; }

			public int DelayToShoot { get; set; }

			public int TimeSinceLastCombat { get; set; }

			public bool EngagedPlayer { get; set; }

			public bool InvestigatingBody { get; set; }

			public bool HeardShots { get; set; }

			public int HeardShotsTimer { get; set; }

			public Vector3 HeardShotsMovePoint { get; set; }

			public bool MovingToPlayer { get; set; }

			public bool ArrestingPlayer { get; set; }

			public int RunTimer { get; set; }

			public dynamic PedHash { get; set; }

			public Ped AIPed { get; set; }

			public Prop AiProp { get; set; }

			public int NextTalkTime { get; set; }

			public Vector3 SpawnLocation { get; set; }

			public float SpawnHeading { get; set; }

			public bool Alive { get; set; }

			public int Health { get; set; }

			public int Armour { get; set; }

			public int SetHealth { get; set; }

			public int SetArmour { get; set; }

			public int ActiveTask { get; set; }

			public float VisualDetectionRange { get; set; }

			public float HearingDetectionRange { get; set; }

			public WeaponHash Weapon { get; set; }

			public string StringVehicleHash { get; set; }

			public VehicleHash VHVehicleHash { get; set; }

			public int SpawnType { get; set; }

			public Vector3 SpawnGroupArea { get; set; }

			public float DistToSpawn { get; set; }

			public bool InvestigatingCamera { get; set; }

			public VehicleType VehType { get; set; }

			public Vehicle LastVehicle { get; set; }

			public int ShootAtTimer { get; set; }

			public int HeliAttackTimer { get; set; }

			public int HeliRerouteTimer { get; set; }

			public int HeliAttackingStage { get; set; }

			public int PlaneTaskUpdateTime { get; set; }

			public int NextHeliAttackTimer { get; set; }

			public bool MarkAsInvestigated { get; set; }

			public bool InvestigatingDeadBody { get; set; }

			public bool SetupWanderLoop { get; set; }

			public bool UseOldWanderLoop { get; set; }

			public PedTaskLoop WanderLoop { get; set; }

			public Vector3 InvestigatePoint { get; set; }

			public Vector3 OriginalPosition { get; set; }

			public float OriginalHeading { get; set; }

			public bool ShowingPedBlipCone { get; set; }

			public bool HasSeenPlayer { get; set; }

			public bool InvestigatingGunshot { get; set; }

			public int InvestigationTimeout { get; set; }

			public bool MovingBacktoOriginalPoint { get; set; }

			public int Timer { get; set; }

			public SmartAiV2()
			{
			}

			public SmartAiV2(Ped P, EnemyGroup GroupName)
			{
				EnemyGroupName = GroupName;
				SpawnGroupArea = P.Position;
				DistToSpawn = 250f;
				GroupActive = true;
				AIPed = P;
				PedHash = P.Model.Hash;
				SpawnLocation = P.Position;
				SpawnHeading = P.Heading;
				OriginalPosition = SpawnLocation;
				OriginalHeading = SpawnHeading;
				GiveRandomWeaponToPed(P, GroupName);
				GetFiringPattern(P);
				Blip blip = AIPed.AddBlip();
				blip.Scale = 0.75f;
				blip.Sprite = BlipSprite.Enemy;
				blip.Name = GetStringNameFromEnemyGroup(EnemyGroupName);
				blip.Color = BlipColor.Red;
				blip.IsShortRange = true;
				blip.Alpha = 0;
				RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
				Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, P, relationshipGroup);
				Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, P, 0, 0);
				Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, P, 1);
			}

			public SmartAiV2(int ST, string Pedhash, Vector3 SpawnLoc, float Heading, bool IsAlive, int HP, int ARM, int AT, PedTaskLoop WL, float VDR, float HDR, WeaponHash W, EnemyGroup GroupName)
			{
				EnemyGroupName = GroupName;
				DistToSpawn = 250f;
				SpawnType = ST;
				PedHash = Pedhash;
				SpawnLocation = SpawnLoc;
				SpawnHeading = Heading;
				Alive = IsAlive;
				Health = HP;
				Armour = ARM;
				ActiveTask = AT;
				WanderLoop = WL;
				VisualDetectionRange = 30f;
				HearingDetectionRange = 50f;
				Weapon = W;
				SetHealth = HP;
				SetArmour = ARM;
				OriginalPosition = SpawnLocation;
				OriginalHeading = SpawnHeading;
				SpawnGroupArea = SpawnLoc;
			}

			public SmartAiV2(int ST, uint Pedhash, Vector3 SpawnLoc, float Heading, bool IsAlive, int HP, int ARM, int AT, PedTaskLoop WL, float VDR, float HDR, WeaponHash W, EnemyGroup GroupName)
			{
				EnemyGroupName = GroupName;
				DistToSpawn = 250f;
				SpawnType = ST;
				PedHash = Pedhash;
				SpawnLocation = SpawnLoc;
				SpawnHeading = Heading;
				Alive = IsAlive;
				Health = HP;
				Armour = ARM;
				ActiveTask = AT;
				WanderLoop = WL;
				VisualDetectionRange = 30f;
				HearingDetectionRange = 50f;
				Weapon = W;
				SetHealth = HP;
				SetArmour = ARM;
				OriginalPosition = SpawnLocation;
				OriginalHeading = SpawnHeading;
				SpawnGroupArea = SpawnLoc;
			}

			public SmartAiV2(int ST, int Pedhash, Vector3 SpawnLoc, float Heading, bool IsAlive, int HP, int ARM, int AT, PedTaskLoop WL, float VDR, float HDR, WeaponHash W, EnemyGroup GroupName)
			{
				EnemyGroupName = GroupName;
				DistToSpawn = 250f;
				SpawnType = ST;
				PedHash = Pedhash;
				SpawnLocation = SpawnLoc;
				SpawnHeading = Heading;
				Alive = IsAlive;
				Health = HP;
				Armour = ARM;
				ActiveTask = AT;
				WanderLoop = WL;
				VisualDetectionRange = 30f;
				HearingDetectionRange = 50f;
				Weapon = W;
				SetHealth = HP;
				SetArmour = ARM;
				OriginalPosition = SpawnLocation;
				OriginalHeading = SpawnHeading;
				SpawnGroupArea = SpawnLoc;
			}

			public SmartAiV2(int ST, string Pedhash, Vector3 SpawnLoc, float Heading, bool IsAlive, int HP, int ARM, int AT, float VDR, float HDR, WeaponHash W, EnemyGroup GroupName)
			{
				EnemyGroupName = GroupName;
				DistToSpawn = 250f;
				PedHash = Pedhash;
				SpawnType = ST;
				SpawnLocation = SpawnLoc;
				SpawnHeading = Heading;
				Alive = IsAlive;
				Health = HP;
				Armour = ARM;
				ActiveTask = AT;
				VisualDetectionRange = 30f;
				HearingDetectionRange = 50f;
				Weapon = W;
				SetHealth = HP;
				SetArmour = ARM;
				OriginalPosition = SpawnLocation;
				OriginalHeading = SpawnHeading;
				SpawnGroupArea = SpawnLoc;
			}

			public SmartAiV2(int ST, PedHash Pedhash, Vector3 SpawnLoc, float Heading, bool IsAlive, int HP, int ARM, int AT, float VDR, float HDR, WeaponHash W, EnemyGroup GroupName)
			{
				EnemyGroupName = GroupName;
				DistToSpawn = 250f;
				PedHash = Pedhash;
				SpawnType = ST;
				SpawnLocation = SpawnLoc;
				SpawnHeading = Heading;
				Alive = IsAlive;
				Health = HP;
				Armour = ARM;
				ActiveTask = AT;
				VisualDetectionRange = 30f;
				HearingDetectionRange = 50f;
				Weapon = W;
				SetHealth = HP;
				SetArmour = ARM;
				OriginalPosition = SpawnLocation;
				OriginalHeading = SpawnHeading;
				SpawnGroupArea = SpawnLoc;
			}

			public SmartAiV2(bool Jugg, int ST, PedHash Pedhash, Vector3 SpawnLoc, float Heading, bool IsAlive, int HP, int ARM, int AT, float VDR, float HDR, WeaponHash W, EnemyGroup GroupName)
			{
				EnemyGroupName = GroupName;
				DistToSpawn = 250f;
				PedHash = Pedhash;
				SpawnType = ST;
				SpawnLocation = SpawnLoc;
				SpawnHeading = Heading;
				Alive = IsAlive;
				Health = HP;
				Armour = ARM;
				ActiveTask = AT;
				VisualDetectionRange = 30f;
				HearingDetectionRange = 50f;
				Weapon = W;
				SetHealth = HP;
				SetArmour = ARM;
				OriginalPosition = SpawnLocation;
				OriginalHeading = SpawnHeading;
				SpawnGroupArea = SpawnLoc;
			}

			public SmartAiV2(int ST, string Pedhash, Vector3 SpawnLoc, float Heading, bool IsAlive, int HP, int ARM, int AT, PedTaskLoop WL, int VDR, int HDR, WeaponHash W, EnemyGroup GroupName)
			{
				EnemyGroupName = GroupName;
				DistToSpawn = 250f;
				SpawnType = ST;
				PedHash = Pedhash;
				SpawnLocation = SpawnLoc;
				SpawnHeading = Heading;
				Alive = IsAlive;
				Health = HP;
				Armour = ARM;
				ActiveTask = AT;
				WanderLoop = WL;
				VisualDetectionRange = 30f;
				HearingDetectionRange = 50f;
				Weapon = W;
				SetHealth = HP;
				SetArmour = ARM;
				OriginalPosition = SpawnLocation;
				OriginalHeading = SpawnHeading;
				SpawnGroupArea = SpawnLoc;
			}

			public SmartAiV2(int ST, PedHash Pedhash, Vector3 SpawnLoc, float Heading, bool IsAlive, int HP, int ARM, int AT, PedTaskLoop WL, float VDR, float HDR, WeaponHash W, EnemyGroup GroupName)
			{
				EnemyGroupName = GroupName;
				DistToSpawn = 250f;
				PedHash = Pedhash;
				SpawnType = ST;
				SpawnLocation = SpawnLoc;
				SpawnHeading = Heading;
				Alive = IsAlive;
				Health = HP;
				Armour = ARM;
				ActiveTask = AT;
				WanderLoop = WL;
				VisualDetectionRange = 30f;
				HearingDetectionRange = 50f;
				Weapon = W;
				SetHealth = HP;
				SetArmour = ARM;
				OriginalPosition = SpawnLocation;
				OriginalHeading = SpawnHeading;
				SpawnGroupArea = SpawnLoc;
			}

			public SmartAiV2(int ST, string Vehiclehash, Vector3 SpawnLoc, float Heading, EnemyGroup GroupName)
			{
				EnemyGroupName = GroupName;
				DistToSpawn = 250f;
				StringVehicleHash = Vehiclehash;
				SpawnType = ST;
				SpawnLocation = SpawnLoc;
				SpawnHeading = Heading;
				OriginalPosition = SpawnLocation;
				OriginalHeading = SpawnHeading;
				SpawnGroupArea = SpawnLoc;
			}

			public SmartAiV2(int ST, VehicleHash Vehiclehash, Vector3 SpawnLoc, float Heading, EnemyGroup GroupName)
			{
				EnemyGroupName = GroupName;
				DistToSpawn = 250f;
				VHVehicleHash = Vehiclehash;
				SpawnType = ST;
				SpawnLocation = SpawnLoc;
				SpawnHeading = Heading;
				OriginalPosition = SpawnLocation;
				OriginalHeading = SpawnHeading;
				SpawnGroupArea = SpawnLoc;
			}

			public SmartAiV2(Vector3 AreaLoc, float SpawnDist, bool Active, EnemyGroup GroupName)
			{
				EnemyGroupName = GroupName;
				SpawnGroupArea = AreaLoc;
				DistToSpawn = SpawnDist;
				GroupActive = Active;
			}

			public SmartAiV2(Ped P, Vector3 AreaLoc, float SpawnDist, bool Active, EnemyGroup GroupName)
			{
				EnemyGroupName = GroupName;
				SpawnGroupArea = AreaLoc;
				DistToSpawn = SpawnDist;
				GroupActive = Active;
				AIPed = P;
				OriginalPosition = SpawnLocation;
				OriginalHeading = SpawnHeading;
			}
		}

		public class SmartAI_Settings
		{
			public List<Entity> DeadBodyMarkers = new List<Entity>();

			public Entity AI_Target = Game.Player.Character;

			public List<int> AircraftWithMissiles = new List<int>
			{
				Function.Call<int>(Hash.GET_HASH_KEY, "lazer"),
				Function.Call<int>(Hash.GET_HASH_KEY, "buzzard"),
				Function.Call<int>(Hash.GET_HASH_KEY, "Conada2"),
				Function.Call<int>(Hash.GET_HASH_KEY, "hydra")
			};

			public float TooFarAwayDist = 1000f;

			public bool CansSpawnDeadBodyMarkers { get; set; }

			public bool HaveFoundDeadBody { get; set; }

			public BlipColor AIBlipColor { get; set; } = BlipColor.Red;


			public bool IgnoreRandomAnimation { get; set; }

			public bool IgnoreRagdollBulletImpact { get; set; }

			public int SmartAI_SpawnedVehicles { get; set; }

			public List<WeaponHash> WeaponsForGroup { get; set; } = new List<WeaponHash>();


			public bool HasSeenPlayer { get; set; }

			public bool IsShootingAtPlayer { get; set; }

			public bool AllowInvestigateBeforeShoot { get; set; }

			public int BlipDeleteCheck { get; set; }

			public int DetectionTimer { get; set; }

			public int CombatTimer { get; set; }

			public int Health { get; set; }

			public int Armor { get; set; }

			public int MinAccuracy { get; set; }

			public int MaxAccuracy { get; set; }

			public bool UseFollowNavMeshRoute { get; set; }

			public Ped CurrentArrestPed { get; set; }

			public bool GuardInvestigatingCamera { get; set; }

			public Vector3 CameraPos { get; set; }

			public bool CameraSeePlayer { get; set; }

			public int CameraTrigger { get; set; }

			public int SpawnTrigger { get; set; }

			public bool IngorePlayer { get; set; }

			public bool HasTargettedAI { get; set; }

			public bool DisableAiBlip { get; set; }

			public int SmartAI_SpawnedHelis { get; set; }

			public int SmartAI_SpawnedCars { get; set; }

			public int SmartAI_SpawnedPlanes { get; set; }

			public int SmartAI_SpawnedBoats { get; set; }

			public bool CanAttackPlayerInBoat { get; set; }

			public bool CanAttackPlayerInPlane { get; set; }

			public bool CanAttackPlayerInCar { get; set; }

			public bool CanAttackPlayerInHeli { get; set; }

			public bool AllowDespawnIfTooFarAwayThisFrame { get; set; }

			public bool LockMissileUseThisFrame { get; set; }

			public int MerryweatherCheck { get; set; }

			public int MerryweatherSpawnCheck { get; set; }

			public Vector3 Player_LastWeaponImpactPosition { get; set; }

			public float Radius { get; set; }

			public float HearingRange { get; set; }

			public float ShowBlipRadius { get; set; }

			public float HitRadius { get; set; }

			public bool ShowCone { get; set; }

			public bool AllowInvestigate { get; set; }

			public float InvestigateDist { get; set; }

			public float SeeingAngle { get; set; }

			public bool ArrestInsteadOfShoot { get; set; }

			public bool AimAtInsteadOfShoot { get; set; }

			public string GuardSubtitleIfAiming { get; set; }

			public bool UseShowBlipWhenSameHeightWithPlayer { get; set; }

			public float ShowBlipHeightAdd { get; set; }

			public float ShowBlipHeightMinus { get; set; }

			public bool RunPatrolRoute { get; set; }

			public void ClearDeadBodyMarkers()
			{
				if (!CansSpawnDeadBodyMarkers)
				{
					return;
				}
				foreach (Entity deadBodyMarker in DeadBodyMarkers)
				{
					if (deadBodyMarker != null)
					{
						deadBodyMarker.Delete();
					}
				}
				DeadBodyMarkers.Clear();
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

		public static List<Entity> EntitiesToDeletePostMission = new List<Entity>();

		public static bool ShowingAimingSutitleThisFrame;

		public static bool ShowAiCone;

		public static List<EnemyGroupData> GuardGroup = new List<EnemyGroupData>
		{
			new EnemyGroupData(EnemyGroup.Enemy, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.Eberhard_Munitions, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.Merryweather, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.Diamond_Casino_Security, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.Cayo_Perico_Cartel, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.Cartel, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.Professionals, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.Military, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.Marabaunte_Grande, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.Aztecas, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.Gang, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.Hired_Gunmen, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.FIB, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.Security, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.Lost_MC, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.Vagos, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.Ballas, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.Families, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.Triads, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.Hippies, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.Rednecks, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.Kkangpae, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.Altruist_Cult, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.Armenian_Mob, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.High_End_Security, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.Swat, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.LSPD, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.Gruppe_Sechs, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.IAA, new List<SmartAiV2>()),
			new EnemyGroupData(EnemyGroup.Street_Racers, new List<SmartAiV2>())
		};

		public static EnemyGroup CurrentActiveGroup = EnemyGroup.None;

		public static List<EnemyGroup> ActiveGroups = new List<EnemyGroup>();

		public static List<EnemyGroup> CurrentSecondaryGroups = new List<EnemyGroup>();

		public static List<SmartAiV2> ActiveSpawnGroup = new List<SmartAiV2>();

		public bool IsGrouchedSprinting;

		public int CrouchedKeypress = 0;

		public int NextKeypressDelay;

		public int NextNoiseCheck;

		public static List<Vehicle> MerryweatherVehicles = new List<Vehicle>();

		public static List<Ped> MerryweatherPeds = new List<Ped>();

		public static List<Ped> AiBackup = new List<Ped>();

		public static List<Vehicle> SmartAI_SpawnedVehicles = new List<Vehicle>();

		public static int Mod_Difficulty = 50;

		public static List<SmartAiV2> SpawnGroupsUse = new List<SmartAiV2>();

		public static List<SmartAiV2> SpawnGroupsUseActive = new List<SmartAiV2>();

		public static Vector3 LastHitCoords = Game.Player.Character.LastWeaponImpactPosition;

		public static List<Ped> DeadPeds = new List<Ped>();

		public static Model LastPlayerModel = Game.Player.Character.Model;

		public static float NoiseThisFrame;

		public static Blip NoiseBlip;

		public static SmartAI_Settings GetCurrentGroupSettings()
		{
			SmartAI_Settings result = new SmartAI_Settings();
			if (CurrentActiveGroup != EnemyGroup.None)
			{
				result = GuardGroup[(int)CurrentActiveGroup].Settings;
			}
			return result;
		}

		public static BlipSprite GetBlipSpriteForVehicle(Vehicle V)
		{
			BlipSprite result = BlipSprite.Enemy;
			if (V.Model.IsHelicopter)
			{
				result = BlipSprite.HelicopterAnimated;
			}
			if (V.Model.IsPlane)
			{
				result = BlipSprite.Jet;
			}
			if (V.Model == VehicleHash.Avenger)
			{
				result = BlipSprite.Avenger;
			}
			if (V.Model == VehicleHash.AlphaZ1)
			{
				result = BlipSprite.AlphaZ1;
			}
			if (V.Model == VehicleHash.DeathBike)
			{
				result = BlipSprite.Deathbike;
			}
			if (V.Model == VehicleHash.DeathBike2)
			{
				result = BlipSprite.Deathbike;
			}
			if (V.Model == VehicleHash.DeathBike3)
			{
				result = BlipSprite.Deathbike;
			}
			if (V.Model == VehicleHash.Avenger2)
			{
				result = BlipSprite.Avenger;
			}
			if (V.Model == VehicleHash.Avenger3)
			{
				result = BlipSprite.Avenger;
			}
			if (V.Model == VehicleHash.Avenger4)
			{
				result = BlipSprite.Avenger;
			}
			if (V.Model == VehicleHash.Akula)
			{
				result = BlipSprite.Akula;
			}
			if (V.Model == VehicleHash.AlphaZ1)
			{
				result = BlipSprite.AlphaZ1;
			}
			if (V.Model == VehicleHash.Apc)
			{
				result = BlipSprite.APC;
			}
			if (V.Model == VehicleHash.Strikeforce)
			{
				result = BlipSprite.B11StrikeForce;
			}
			if (V.Model == VehicleHash.Barrage)
			{
				result = BlipSprite.Barrage;
			}
			if (V.Model == VehicleHash.Bombushka)
			{
				result = BlipSprite.Bombushka;
			}
			if (V.Model == VehicleHash.Brutus)
			{
				result = BlipSprite.Brutus;
			}
			if (V.Model == VehicleHash.Brutus2)
			{
				result = BlipSprite.Brutus;
			}
			if (V.Model == VehicleHash.Brutus3)
			{
				result = BlipSprite.Brutus;
			}
			if (V.Model == VehicleHash.Caracara)
			{
				result = BlipSprite.Caracara;
			}
			if (V.Model == VehicleHash.Cerberus)
			{
				result = BlipSprite.Cerberus;
			}
			if (V.Model == VehicleHash.Cerberus2)
			{
				result = BlipSprite.Cerberus;
			}
			if (V.Model == VehicleHash.Cerberus3)
			{
				result = BlipSprite.Cerberus;
			}
			if (V.Model == VehicleHash.Chernobog)
			{
				result = BlipSprite.Chernobog;
			}
			if (V.Model == VehicleHash.Deluxo)
			{
				result = BlipSprite.Deluxo;
			}
			if (V.Model == VehicleHash.Dominator4)
			{
				result = BlipSprite.Dominator;
			}
			if (V.Model == VehicleHash.Dominator5)
			{
				result = BlipSprite.Dominator;
			}
			if (V.Model == VehicleHash.Dominator6)
			{
				result = BlipSprite.Dominator;
			}
			if (V.Model == VehicleHash.Dune3)
			{
				result = BlipSprite.DuneFAV;
			}
			if (V.Model == VehicleHash.HalfTrack)
			{
				result = BlipSprite.HalfTrack;
			}
			if (V.Model == VehicleHash.Havok)
			{
				result = BlipSprite.Havok;
			}
			if (V.Model == VehicleHash.Hunter)
			{
				result = BlipSprite.Hunter;
			}
			if (V.Model == VehicleHash.Microlight)
			{
				result = BlipSprite.Ultralight;
			}
			if (V.Model == VehicleHash.Howard)
			{
				result = BlipSprite.HowardNX25;
			}
			if (V.Model == VehicleHash.Impaler)
			{
				result = BlipSprite.Impaler;
			}
			if (V.Model == VehicleHash.Impaler2)
			{
				result = BlipSprite.Impaler;
			}
			if (V.Model == VehicleHash.Impaler3)
			{
				result = BlipSprite.Impaler;
			}
			if (V.Model == VehicleHash.Impaler3)
			{
				result = BlipSprite.Impaler;
			}
			if (V.Model == VehicleHash.Imperator)
			{
				result = BlipSprite.Imperator;
			}
			if (V.Model == VehicleHash.Imperator2)
			{
				result = BlipSprite.Imperator;
			}
			if (V.Model == VehicleHash.Imperator3)
			{
				result = BlipSprite.Imperator;
			}
			if (V.Model == VehicleHash.Issi4)
			{
				result = BlipSprite.Issi;
			}
			if (V.Model == VehicleHash.Issi5)
			{
				result = BlipSprite.Issi;
			}
			if (V.Model == VehicleHash.Issi6)
			{
				result = BlipSprite.Issi;
			}
			if (V.Model == VehicleHash.Khanjali)
			{
				result = BlipSprite.Khanjali;
			}
			if (V.Model == VehicleHash.Riot2)
			{
				result = BlipSprite.RCV;
			}
			if (V.Model == VehicleHash.Mogul)
			{
				result = BlipSprite.Mogul;
			}
			if (V.Model == VehicleHash.Mule4)
			{
				result = BlipSprite.MuleCustom;
			}
			if (V.Model == VehicleHash.Nokota)
			{
				result = BlipSprite.P45Nokota;
			}
			if (V.Model == VehicleHash.Oppressor)
			{
				result = BlipSprite.Oppressor;
			}
			if (V.Model == VehicleHash.Oppressor2)
			{
				result = BlipSprite.OppressorMkII;
			}
			if (V.Model == VehicleHash.Pyro)
			{
				result = BlipSprite.Pyro;
			}
			if (V.Model == VehicleHash.Rogue)
			{
				result = BlipSprite.Rogue;
			}
			if (V.Model == VehicleHash.Ruiner2)
			{
				result = BlipSprite.Ruiner2000;
			}
			if (V.Model == VehicleHash.Scarab)
			{
				result = BlipSprite.Scarab;
			}
			if (V.Model == VehicleHash.Scarab2)
			{
				result = BlipSprite.Scarab;
			}
			if (V.Model == VehicleHash.Scarab3)
			{
				result = BlipSprite.Scarab;
			}
			if (V.Model == VehicleHash.Scramjet)
			{
				result = BlipSprite.Scramjet;
			}
			if (V.Model == VehicleHash.Seabreeze)
			{
				result = BlipSprite.Seabreeze;
			}
			if (V.Model == VehicleHash.SeaSparrow)
			{
				result = BlipSprite.SeaSparrow;
			}
			if (V.Model == VehicleHash.SlamVan4)
			{
				result = BlipSprite.Slamvam;
			}
			if (V.Model == VehicleHash.SlamVan5)
			{
				result = BlipSprite.Slamvam;
			}
			if (V.Model == VehicleHash.SlamVan6)
			{
				result = BlipSprite.Slamvam;
			}
			if (V.Model == VehicleHash.Starling)
			{
				result = BlipSprite.Starling;
			}
			if (V.Model == VehicleHash.Stromberg)
			{
				result = BlipSprite.Stromberg;
			}
			if (V.Model == VehicleHash.Technical)
			{
				result = BlipSprite.Caracara;
			}
			if (V.Model == VehicleHash.Technical2)
			{
				result = BlipSprite.TechnicalAqua;
			}
			if (V.Model == VehicleHash.Technical3)
			{
				result = BlipSprite.Caracara;
			}
			if (V.Model == VehicleHash.Thruster)
			{
				result = BlipSprite.Thruster;
			}
			if (V.Model == VehicleHash.Tula)
			{
				result = BlipSprite.Tula;
			}
			if (V.Model == VehicleHash.Limo2)
			{
				result = BlipSprite.TurretedLimo;
			}
			if (V.Model == VehicleHash.Microlight)
			{
				result = BlipSprite.Ultralight;
			}
			if (V.Model == VehicleHash.Molotok)
			{
				result = BlipSprite.V65Molotok;
			}
			if (V.Model == VehicleHash.Volatol)
			{
				result = BlipSprite.Volatol;
			}
			if (V.Model == VehicleHash.Tampa3)
			{
				result = BlipSprite.WeaponizedTampa;
			}
			if (V.Model == VehicleHash.ZR380)
			{
				result = BlipSprite.ZR380;
			}
			if (V.Model == VehicleHash.ZR3802)
			{
				result = BlipSprite.ZR380;
			}
			if (V.Model == VehicleHash.ZR3803)
			{
				result = BlipSprite.ZR380;
			}
			if (V.Model == VehicleHash.Caracara)
			{
				result = BlipSprite.Caracara;
			}
			if (V.Model == VehicleHash.Caracara2)
			{
				result = BlipSprite.Caracara;
			}
			if (V.Model == VehicleHash.Terrorbyte)
			{
				result = BlipSprite.Terrorbyte;
			}
			if (V.Model == VehicleHash.Menacer)
			{
				result = BlipSprite.Menacer;
			}
			if (V.Model == VehicleHash.Pounder2)
			{
				result = BlipSprite.PounderCustom;
			}
			if (V.Model == VehicleHash.Speedo4)
			{
				result = BlipSprite.SpeedoCustom;
			}
			if (V.Model == VehicleHash.Zhaba)
			{
				result = BlipSprite.Zhaba;
			}
			if (V.Model == VehicleHash.Winky)
			{
				result = BlipSprite.Winky;
			}
			if (V.Model == VehicleHash.PatrolBoat)
			{
				result = BlipSprite.PatrolBoat;
			}
			if (V.Model == VehicleHash.Dinghy5)
			{
				result = BlipSprite.Dinghy2;
			}
			if (V.Model == VehicleHash.Annihilator2)
			{
				result = BlipSprite.Valkyrie2;
			}
			if (V.Model == VehicleHash.Kosatka)
			{
				result = BlipSprite.Kosatka;
			}
			if (V.Model == VehicleHash.Patriot3)
			{
				result = BlipSprite.PatriotMilSpec;
			}
			if (V.Model == VehicleHash.Champion)
			{
				result = BlipSprite.DewbaucheeChampion;
			}
			if (V.Model == VehicleHash.Buffalo4)
			{
				result = BlipSprite.BuffaloSTX;
			}
			if (V.Model == VehicleHash.Granger2)
			{
				result = BlipSprite.Granger3600LX;
			}
			if (V.Model == VehicleHash.Jubilee)
			{
				result = BlipSprite.Jubilee;
			}
			if (V.Model == VehicleHash.Deity)
			{
				result = BlipSprite.Deity;
			}
			if (V.Model == VehicleHash.Raiju)
			{
				result = BlipSprite.F160Raiju;
			}
			if (V.Model == VehicleHash.Conada2)
			{
				result = BlipSprite.BuckinghamWeaponizedConada;
			}
			if (V.Model == VehicleHash.Streamer216)
			{
				result = BlipSprite.Streamer216;
			}
			if (V.Model == VehicleHash.Insurgent3)
			{
				result = BlipSprite.GunCar;
			}
			if (V.Model == VehicleHash.Terminus)
			{
				result = BlipSprite.Crusader;
			}
			if (V.Model == VehicleHash.Mesa3)
			{
				result = BlipSprite.Crusader;
			}
			if (V.Model == VehicleHash.Crusader)
			{
				result = BlipSprite.Crusader;
			}
			if (V.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "TITAN2"))
			{
				result = (BlipSprite)917;
			}
			if (V.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "CARGOBOB5"))
			{
				result = (BlipSprite)905;
			}
			return result;
		}

		public static string GetStringNameFromEnemyGroup(EnemyGroup G)
		{
			return G.ToString().Replace("_", " ");
		}

		public static void RemoveGenericGuardsGroup(EnemyGroup Group)
		{
			List<SmartAiV2> groupList = GuardGroup[(int)Group].GroupList;
			if (groupList.Count > 0)
			{
				foreach (SmartAiV2 item in groupList)
				{
					if (item.AIPed != null)
					{
						item.AIPed.Delete();
					}
					if (item.AiProp != null)
					{
						item.AiProp.Delete();
					}
				}
			}
			groupList.Clear();
			groupList = GuardGroup[(int)Group].ReinforcementsList;
			if (groupList.Count > 0)
			{
				foreach (SmartAiV2 item2 in groupList)
				{
					if (item2.AIPed != null)
					{
						item2.AIPed.Delete();
					}
					if (item2.AiProp != null)
					{
						item2.AiProp.Delete();
					}
				}
			}
			groupList.Clear();
		}

		public static void SetGenericGuardsGroup(EnemyGroup Group, List<SmartAiV2> AIList)
		{
			if (GuardGroup[(int)Group].GroupList.Count > 0)
			{
				foreach (SmartAiV2 group in GuardGroup[(int)Group].GroupList)
				{
					if (group.AIPed != null)
					{
						group.AIPed.Delete();
					}
					if (group.AiProp != null)
					{
						group.AiProp.Delete();
					}
				}
			}
			GuardGroup[(int)Group].GroupList.Clear();
			foreach (SmartAiV2 AI in AIList)
			{
				GuardGroup[(int)Group].GroupList.Add(AI);
			}
		}

		public static void CombineGenericGuardsGroup(EnemyGroup Group, List<SmartAiV2> AIList1, List<SmartAiV2> AIList2)
		{
			List<SmartAiV2> groupList = GuardGroup[(int)Group].GroupList;
			if (groupList.Count > 0)
			{
				foreach (SmartAiV2 item in groupList)
				{
					if (item.AIPed != null)
					{
						item.AIPed.Delete();
					}
					if (item.AiProp != null)
					{
						item.AiProp.Delete();
					}
				}
			}
			groupList.Clear();
			groupList.AddRange(AIList1);
			groupList.AddRange(AIList2);
		}

		public static SmartAI_Settings GetSettingsDefaultsForGroup(EnemyGroup Group)
		{
			SmartAI_Settings settings = GuardGroup[(int)Group].Settings;
			settings.IsShootingAtPlayer = false;
			switch (Group)
			{
			case EnemyGroup.Enemy:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 20f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 200;
				settings.Armor = 0;
				settings.MinAccuracy = 35;
				settings.MaxAccuracy = 70;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 35f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 30f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Eberhard_Munitions:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 20f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 300;
				settings.Armor = 100;
				settings.MinAccuracy = 45;
				settings.MaxAccuracy = 80;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 35f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 30f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Merryweather:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 20f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 400;
				settings.Armor = 500;
				settings.MinAccuracy = 45;
				settings.MaxAccuracy = 75;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 35f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 30f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Diamond_Casino_Security:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 10f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 300;
				settings.Armor = 400;
				settings.MinAccuracy = 35;
				settings.MaxAccuracy = 70;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 25f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 10f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Cayo_Perico_Cartel:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 20f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 400;
				settings.Armor = 300;
				settings.MinAccuracy = 35;
				settings.MaxAccuracy = 75;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 35f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 30f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Cartel:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 20f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 300;
				settings.Armor = 200;
				settings.MinAccuracy = 35;
				settings.MaxAccuracy = 70;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 35f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 30f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Professionals:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 20f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 200;
				settings.Armor = 300;
				settings.MinAccuracy = 35;
				settings.MaxAccuracy = 60;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 35f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 30f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Military:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 20f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 300;
				settings.Armor = 500;
				settings.MinAccuracy = 55;
				settings.MaxAccuracy = 80;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 35f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 30f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Marabaunte_Grande:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 20f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 200;
				settings.Armor = 50;
				settings.MinAccuracy = 25;
				settings.MaxAccuracy = 60;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 35f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 30f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Aztecas:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 20f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 250;
				settings.Armor = 50;
				settings.MinAccuracy = 35;
				settings.MaxAccuracy = 55;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 35f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 30f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Gang:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 20f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 250;
				settings.Armor = 50;
				settings.MinAccuracy = 35;
				settings.MaxAccuracy = 65;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 35f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 30f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Hired_Gunmen:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 20f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 250;
				settings.Armor = 50;
				settings.MinAccuracy = 35;
				settings.MaxAccuracy = 70;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 35f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 30f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.FIB:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 20f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 250;
				settings.Armor = 50;
				settings.MinAccuracy = 45;
				settings.MaxAccuracy = 80;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 35f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 30f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Security:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 15f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 350;
				settings.Armor = 150;
				settings.MinAccuracy = 45;
				settings.MaxAccuracy = 75;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 90f;
				settings.SeeingAngle = 35f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 5f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Lost_MC:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 20f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 250;
				settings.Armor = 50;
				settings.MinAccuracy = 45;
				settings.MaxAccuracy = 65;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 35f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 30f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Vagos:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 20f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 250;
				settings.Armor = 50;
				settings.MinAccuracy = 35;
				settings.MaxAccuracy = 65;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 35f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 30f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Ballas:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 20f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 250;
				settings.Armor = 50;
				settings.MinAccuracy = 35;
				settings.MaxAccuracy = 65;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 35f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 30f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Families:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 20f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 250;
				settings.Armor = 50;
				settings.MinAccuracy = 35;
				settings.MaxAccuracy = 65;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 35f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 30f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Triads:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 20f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 250;
				settings.Armor = 50;
				settings.MinAccuracy = 35;
				settings.MaxAccuracy = 65;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 35f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 30f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Hippies:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 20f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 250;
				settings.Armor = 50;
				settings.MinAccuracy = 25;
				settings.MaxAccuracy = 55;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 35f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 30f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Rednecks:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 20f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 250;
				settings.Armor = 50;
				settings.MinAccuracy = 25;
				settings.MaxAccuracy = 55;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 35f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 30f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Kkangpae:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 20f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 250;
				settings.Armor = 50;
				settings.MinAccuracy = 25;
				settings.MaxAccuracy = 55;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 35f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 30f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Altruist_Cult:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 20f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 250;
				settings.Armor = 50;
				settings.MinAccuracy = 15;
				settings.MaxAccuracy = 45;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 35f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 30f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Armenian_Mob:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 20f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 250;
				settings.Armor = 50;
				settings.MinAccuracy = 25;
				settings.MaxAccuracy = 55;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 35f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 30f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.High_End_Security:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 10f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 300;
				settings.Armor = 400;
				settings.MinAccuracy = 40;
				settings.MaxAccuracy = 80;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 25f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 10f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Swat:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 10f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 300;
				settings.Armor = 400;
				settings.MinAccuracy = 40;
				settings.MaxAccuracy = 80;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 25f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 10f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.LSPD:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 10f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 300;
				settings.Armor = 400;
				settings.MinAccuracy = 30;
				settings.MaxAccuracy = 70;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 25f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 10f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Gruppe_Sechs:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 10f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 300;
				settings.Armor = 400;
				settings.MinAccuracy = 30;
				settings.MaxAccuracy = 70;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 25f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 10f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.IAA:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 20f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 250;
				settings.Armor = 50;
				settings.MinAccuracy = 40;
				settings.MaxAccuracy = 80;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 35f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 30f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Street_Racers:
				settings.HasSeenPlayer = false;
				settings.AIBlipColor = BlipColor.Red;
				settings.HearingRange = 20f;
				settings.IgnoreRagdollBulletImpact = true;
				settings.ShowCone = ShowAiCone;
				settings.Health = 250;
				settings.Armor = 50;
				settings.MinAccuracy = 30;
				settings.MaxAccuracy = 55;
				settings.ShowBlipRadius = 100f;
				settings.Radius = 120f;
				settings.SeeingAngle = 35f;
				settings.AllowInvestigate = true;
				settings.InvestigateDist = 30f;
				settings.HitRadius = 40f;
				settings.ArrestInsteadOfShoot = false;
				settings.IngorePlayer = false;
				settings.AllowDespawnIfTooFarAwayThisFrame = false;
				settings.TooFarAwayDist = 15000f;
				settings.CanAttackPlayerInBoat = true;
				settings.CanAttackPlayerInCar = true;
				settings.CanAttackPlayerInHeli = true;
				settings.CanAttackPlayerInPlane = true;
				settings.ShowBlipHeightAdd = 25f;
				settings.ShowBlipHeightMinus = 25f;
				break;
			}
			return settings;
		}

		public static SmartAI_Settings CreateSettingsForGroup(EnemyGroup Group)
		{
			SmartAI_Settings smartAI_Settings = new SmartAI_Settings();
			smartAI_Settings.IsShootingAtPlayer = false;
			switch (Group)
			{
			case EnemyGroup.Enemy:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 20f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 200;
				smartAI_Settings.Armor = 0;
				smartAI_Settings.MinAccuracy = 35;
				smartAI_Settings.MaxAccuracy = 70;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 35f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 30f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Eberhard_Munitions:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 20f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 300;
				smartAI_Settings.Armor = 100;
				smartAI_Settings.MinAccuracy = 45;
				smartAI_Settings.MaxAccuracy = 80;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 35f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 30f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Merryweather:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 20f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 400;
				smartAI_Settings.Armor = 500;
				smartAI_Settings.MinAccuracy = 45;
				smartAI_Settings.MaxAccuracy = 75;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 35f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 30f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Diamond_Casino_Security:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 10f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 300;
				smartAI_Settings.Armor = 400;
				smartAI_Settings.MinAccuracy = 35;
				smartAI_Settings.MaxAccuracy = 70;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 25f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 10f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Cayo_Perico_Cartel:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 20f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 400;
				smartAI_Settings.Armor = 300;
				smartAI_Settings.MinAccuracy = 35;
				smartAI_Settings.MaxAccuracy = 75;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 35f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 30f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Cartel:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 20f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 300;
				smartAI_Settings.Armor = 200;
				smartAI_Settings.MinAccuracy = 35;
				smartAI_Settings.MaxAccuracy = 70;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 35f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 30f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Professionals:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 20f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 200;
				smartAI_Settings.Armor = 300;
				smartAI_Settings.MinAccuracy = 35;
				smartAI_Settings.MaxAccuracy = 60;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 35f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 30f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Military:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 20f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 300;
				smartAI_Settings.Armor = 500;
				smartAI_Settings.MinAccuracy = 55;
				smartAI_Settings.MaxAccuracy = 80;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 35f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 30f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Marabaunte_Grande:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 20f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 200;
				smartAI_Settings.Armor = 50;
				smartAI_Settings.MinAccuracy = 25;
				smartAI_Settings.MaxAccuracy = 60;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 35f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 30f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Aztecas:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 20f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 250;
				smartAI_Settings.Armor = 50;
				smartAI_Settings.MinAccuracy = 35;
				smartAI_Settings.MaxAccuracy = 55;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 35f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 30f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Gang:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 20f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 250;
				smartAI_Settings.Armor = 50;
				smartAI_Settings.MinAccuracy = 35;
				smartAI_Settings.MaxAccuracy = 65;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 35f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 30f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Hired_Gunmen:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 20f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 250;
				smartAI_Settings.Armor = 50;
				smartAI_Settings.MinAccuracy = 35;
				smartAI_Settings.MaxAccuracy = 70;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 35f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 30f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.FIB:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 20f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 250;
				smartAI_Settings.Armor = 50;
				smartAI_Settings.MinAccuracy = 45;
				smartAI_Settings.MaxAccuracy = 80;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 35f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 30f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Security:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 15f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 350;
				smartAI_Settings.Armor = 150;
				smartAI_Settings.MinAccuracy = 45;
				smartAI_Settings.MaxAccuracy = 75;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 90f;
				smartAI_Settings.SeeingAngle = 35f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 5f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Lost_MC:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 20f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 250;
				smartAI_Settings.Armor = 50;
				smartAI_Settings.MinAccuracy = 45;
				smartAI_Settings.MaxAccuracy = 65;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 35f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 30f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Vagos:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 20f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 250;
				smartAI_Settings.Armor = 50;
				smartAI_Settings.MinAccuracy = 35;
				smartAI_Settings.MaxAccuracy = 65;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 35f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 30f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Ballas:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 20f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 250;
				smartAI_Settings.Armor = 50;
				smartAI_Settings.MinAccuracy = 35;
				smartAI_Settings.MaxAccuracy = 65;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 35f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 30f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Families:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 20f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 250;
				smartAI_Settings.Armor = 50;
				smartAI_Settings.MinAccuracy = 35;
				smartAI_Settings.MaxAccuracy = 65;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 35f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 30f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Triads:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 20f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 250;
				smartAI_Settings.Armor = 50;
				smartAI_Settings.MinAccuracy = 35;
				smartAI_Settings.MaxAccuracy = 65;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 35f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 30f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Hippies:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 20f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 250;
				smartAI_Settings.Armor = 50;
				smartAI_Settings.MinAccuracy = 25;
				smartAI_Settings.MaxAccuracy = 55;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 35f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 30f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Rednecks:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 20f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 250;
				smartAI_Settings.Armor = 50;
				smartAI_Settings.MinAccuracy = 25;
				smartAI_Settings.MaxAccuracy = 55;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 35f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 30f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Kkangpae:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 20f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 250;
				smartAI_Settings.Armor = 50;
				smartAI_Settings.MinAccuracy = 25;
				smartAI_Settings.MaxAccuracy = 55;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 35f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 30f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Altruist_Cult:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 20f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 250;
				smartAI_Settings.Armor = 50;
				smartAI_Settings.MinAccuracy = 15;
				smartAI_Settings.MaxAccuracy = 45;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 35f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 30f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Armenian_Mob:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 20f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 250;
				smartAI_Settings.Armor = 50;
				smartAI_Settings.MinAccuracy = 25;
				smartAI_Settings.MaxAccuracy = 55;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 35f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 30f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.High_End_Security:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 10f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 300;
				smartAI_Settings.Armor = 400;
				smartAI_Settings.MinAccuracy = 40;
				smartAI_Settings.MaxAccuracy = 80;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 25f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 10f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Swat:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 10f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 300;
				smartAI_Settings.Armor = 400;
				smartAI_Settings.MinAccuracy = 40;
				smartAI_Settings.MaxAccuracy = 80;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 25f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 10f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.LSPD:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 10f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 300;
				smartAI_Settings.Armor = 400;
				smartAI_Settings.MinAccuracy = 30;
				smartAI_Settings.MaxAccuracy = 70;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 25f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 10f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Gruppe_Sechs:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 10f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 300;
				smartAI_Settings.Armor = 400;
				smartAI_Settings.MinAccuracy = 30;
				smartAI_Settings.MaxAccuracy = 70;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 25f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 10f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.IAA:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 20f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 250;
				smartAI_Settings.Armor = 50;
				smartAI_Settings.MinAccuracy = 40;
				smartAI_Settings.MaxAccuracy = 80;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 35f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 30f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			case EnemyGroup.Street_Racers:
				smartAI_Settings.HasSeenPlayer = false;
				smartAI_Settings.AIBlipColor = BlipColor.Red;
				smartAI_Settings.HearingRange = 20f;
				smartAI_Settings.IgnoreRagdollBulletImpact = true;
				smartAI_Settings.ShowCone = ShowAiCone;
				smartAI_Settings.Health = 250;
				smartAI_Settings.Armor = 50;
				smartAI_Settings.MinAccuracy = 30;
				smartAI_Settings.MaxAccuracy = 55;
				smartAI_Settings.ShowBlipRadius = 100f;
				smartAI_Settings.Radius = 120f;
				smartAI_Settings.SeeingAngle = 35f;
				smartAI_Settings.AllowInvestigate = true;
				smartAI_Settings.InvestigateDist = 30f;
				smartAI_Settings.HitRadius = 40f;
				smartAI_Settings.ArrestInsteadOfShoot = false;
				smartAI_Settings.IngorePlayer = false;
				smartAI_Settings.AllowDespawnIfTooFarAwayThisFrame = false;
				smartAI_Settings.TooFarAwayDist = 15000f;
				smartAI_Settings.CanAttackPlayerInBoat = true;
				smartAI_Settings.CanAttackPlayerInCar = true;
				smartAI_Settings.CanAttackPlayerInHeli = true;
				smartAI_Settings.CanAttackPlayerInPlane = true;
				smartAI_Settings.ShowBlipHeightAdd = 25f;
				smartAI_Settings.ShowBlipHeightMinus = 25f;
				break;
			}
			return smartAI_Settings;
		}

		public static void MatchSettingsFromGroup(SmartAI_Settings ToSettings, SmartAI_Settings FromSettings)
		{
			FromSettings.HaveFoundDeadBody = ToSettings.HaveFoundDeadBody;
			FromSettings.CansSpawnDeadBodyMarkers = ToSettings.CansSpawnDeadBodyMarkers;
			FromSettings.HasSeenPlayer = ToSettings.HasSeenPlayer;
			FromSettings.IsShootingAtPlayer = ToSettings.IsShootingAtPlayer;
			FromSettings.AllowInvestigateBeforeShoot = ToSettings.AllowInvestigateBeforeShoot;
			FromSettings.IsShootingAtPlayer = ToSettings.IsShootingAtPlayer;
			FromSettings.HasSeenPlayer = ToSettings.HasSeenPlayer;
			FromSettings.AIBlipColor = ToSettings.AIBlipColor;
			FromSettings.HearingRange = ToSettings.HearingRange;
			FromSettings.IgnoreRagdollBulletImpact = ToSettings.IgnoreRagdollBulletImpact;
			FromSettings.ShowCone = ToSettings.ShowCone;
			FromSettings.Health = ToSettings.Health;
			FromSettings.Armor = ToSettings.Armor;
			FromSettings.MinAccuracy = ToSettings.MinAccuracy;
			FromSettings.MaxAccuracy = ToSettings.MaxAccuracy;
			FromSettings.ShowBlipRadius = ToSettings.ShowBlipRadius;
			FromSettings.Radius = ToSettings.Radius;
			FromSettings.SeeingAngle = ToSettings.SeeingAngle;
			FromSettings.AllowInvestigate = ToSettings.AllowInvestigate;
			FromSettings.InvestigateDist = ToSettings.InvestigateDist;
			FromSettings.HitRadius = ToSettings.HitRadius;
			FromSettings.ArrestInsteadOfShoot = ToSettings.ArrestInsteadOfShoot;
			FromSettings.IngorePlayer = ToSettings.IngorePlayer;
			FromSettings.AllowDespawnIfTooFarAwayThisFrame = ToSettings.AllowDespawnIfTooFarAwayThisFrame;
			FromSettings.TooFarAwayDist = ToSettings.TooFarAwayDist;
			FromSettings.CanAttackPlayerInBoat = ToSettings.CanAttackPlayerInBoat;
			FromSettings.CanAttackPlayerInCar = ToSettings.CanAttackPlayerInCar;
			FromSettings.CanAttackPlayerInHeli = ToSettings.CanAttackPlayerInHeli;
			FromSettings.CanAttackPlayerInPlane = ToSettings.CanAttackPlayerInPlane;
			FromSettings.ShowBlipHeightAdd = ToSettings.ShowBlipHeightAdd;
			FromSettings.ShowBlipHeightMinus = ToSettings.ShowBlipHeightMinus;
		}

		public static void ClearAllDeadBodies(SmartAI_Settings Settings)
		{
			Settings?.ClearDeadBodyMarkers();
		}

		public static void ClearAllDeadBodies()
		{
			foreach (EnemyGroupData item in GuardGroup)
			{
				if (item.Settings != null)
				{
					item.Settings.ClearDeadBodyMarkers();
				}
			}
		}

		public static void ClearGenericGuards()
		{
			ClearAllDeadBodies();
			foreach (Entity item in EntitiesToDeletePostMission)
			{
				if (item != null)
				{
					item.Delete();
				}
			}
			EntitiesToDeletePostMission.Clear();
			CurrentSecondaryGroups.Clear();
			CurrentActiveGroup = EnemyGroup.None;
			foreach (EnemyGroupData item2 in GuardGroup)
			{
				foreach (SmartAiV2 group in item2.GroupList)
				{
					if (group.AIPed != null)
					{
						group.AIPed.Delete();
					}
					if (group.AiProp != null)
					{
						group.AiProp.Delete();
					}
				}
				item2.GroupList.Clear();
				foreach (SmartAiV2 reinforcements in item2.ReinforcementsList)
				{
					if (reinforcements.AIPed != null)
					{
						reinforcements.AIPed.Delete();
					}
					if (reinforcements.AiProp != null)
					{
						reinforcements.AiProp.Delete();
					}
				}
				item2.ReinforcementsList.Clear();
			}
			Function.Call(Hash.SET_RADAR_ZOOM, 850);
		}

		public static void ChangeActiveGroup(EnemyGroup NextGroup)
		{
			if (NextGroup != CurrentActiveGroup)
			{
				RemoveAIGuards();
				CurrentActiveGroup = NextGroup;
			}
		}

		private Vector2 World3DToScreen2d(Vector3 pos)
		{
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, pos.X, pos.Y, pos.Z, outputArgument, outputArgument2);
			return new Vector2(outputArgument.GetResult<float>(), outputArgument2.GetResult<float>());
		}

		private static void drawText(string text, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		public static string ConvertRGBColortoHex(Vector3 RGB)
		{
			string text = "";
			Color c = Color.FromArgb((int)RGB.X, (int)RGB.Y, (int)RGB.Z);
			return text + ColorTranslator.ToHtml(c);
		}

		public static string StartCustomColor(string HTMLColor)
		{
			string text = HTMLColor;
			if (text.Contains("#"))
			{
				text = text.Replace("#", "");
			}
			text = "#" + text;
			return "~w~<font color=\"" + text + "\">";
		}

		public static string EndCustomColor()
		{
			return "</font>";
		}

		public SmartGuards()
		{
			base.Tick += OnTick;
			Notification.Show(StartCustomColor(ConvertRGBColortoHex(new Vector3(0f, 1f, 210f))) + "Smart Guards" + EndCustomColor() + " Version ~y~12.0.0");
		}

		public static bool IsPedUsingMountedVehicleWeapon(Vehicle Vehicle, Ped Gunner)
		{
			bool result = false;
			VehicleHash hash = (VehicleHash)Vehicle.Model.Hash;
			if (Gunner.CurrentVehicle == Vehicle)
			{
				switch (hash)
				{
				case VehicleHash.Insurgent2:
					if (Vehicle.GetPedOnSeat(VehicleSeat.ExtraSeat1) == Gunner)
					{
						result = true;
					}
					break;
				case VehicleHash.Insurgent3:
					if (Vehicle.GetPedOnSeat(VehicleSeat.ExtraSeat1) == Gunner)
					{
						result = true;
					}
					break;
				case VehicleHash.Technical:
					if (Vehicle.GetPedOnSeat(VehicleSeat.LeftRear) == Gunner)
					{
						result = true;
					}
					break;
				case VehicleHash.Technical2:
					if (Vehicle.GetPedOnSeat(VehicleSeat.LeftRear) == Gunner)
					{
						result = true;
					}
					break;
				case VehicleHash.Technical3:
					if (Vehicle.GetPedOnSeat(VehicleSeat.LeftRear) == Gunner)
					{
						result = true;
					}
					break;
				case VehicleHash.Barrage:
					if (Vehicle.GetPedOnSeat(VehicleSeat.LeftRear) == Gunner)
					{
						result = true;
					}
					if (Vehicle.GetPedOnSeat(VehicleSeat.RightRear) == Gunner)
					{
						result = true;
					}
					break;
				case VehicleHash.Menacer:
					if (Vehicle.GetPedOnSeat(VehicleSeat.ExtraSeat1) == Gunner)
					{
						result = true;
					}
					break;
				case VehicleHash.Caracara:
					if (Vehicle.GetPedOnSeat(VehicleSeat.ExtraSeat1) == Gunner)
					{
						result = true;
					}
					break;
				}
			}
			return result;
		}

		public void OnTick(object sender, EventArgs e)
		{
			if (LastPlayerModel != Game.Player.Character.Model)
			{
				LastPlayerModel = Game.Player.Character.Model;
				foreach (EnemyGroupData item in GuardGroup)
				{
					item.Settings.AI_Target = Game.Player.Character;
				}
			}
			if (CurrentActiveGroup != EnemyGroup.None)
			{
			}
			if (CurrentActiveGroup == EnemyGroup.None && ActiveGroups.Count < 0)
			{
				return;
			}
			if (CurrentActiveGroup != EnemyGroup.None)
			{
			}
			if (ActiveGroups.Count > 0)
			{
				foreach (EnemyGroup activeGroup in ActiveGroups)
				{
					if (GuardGroup[(int)activeGroup].GroupList.Count > 0)
					{
						RunSmartAIFuncs(GuardGroup[(int)activeGroup].GroupList, GuardGroup[(int)activeGroup].Settings);
					}
					if (GuardGroup[(int)activeGroup].ReinforcementsList.Count > 0)
					{
						RunSmartAIFuncs(GuardGroup[(int)activeGroup].ReinforcementsList, GuardGroup[(int)activeGroup].Settings);
					}
				}
			}
			else if (CurrentActiveGroup != EnemyGroup.None)
			{
				if (GuardGroup[(int)CurrentActiveGroup].GroupList.Count > 0)
				{
					RunSmartAIFuncs(GuardGroup[(int)CurrentActiveGroup].GroupList, GuardGroup[(int)CurrentActiveGroup].Settings);
				}
				if (GuardGroup[(int)CurrentActiveGroup].ReinforcementsList.Count > 0)
				{
					RunSmartAIFuncs(GuardGroup[(int)CurrentActiveGroup].ReinforcementsList, GuardGroup[(int)CurrentActiveGroup].Settings);
				}
			}
			if (CurrentSecondaryGroups.Count > 0)
			{
				foreach (EnemyGroup currentSecondaryGroup in CurrentSecondaryGroups)
				{
					if (GuardGroup[(int)currentSecondaryGroup].GroupList.Count > 0)
					{
						RunSmartAIFuncs_SecondaryOnly(GuardGroup[(int)currentSecondaryGroup].GroupList, GuardGroup[(int)currentSecondaryGroup].Settings);
					}
					if (GuardGroup[(int)currentSecondaryGroup].ReinforcementsList.Count > 0)
					{
						RunSmartAIFuncs_SecondaryOnly(GuardGroup[(int)currentSecondaryGroup].ReinforcementsList, GuardGroup[(int)currentSecondaryGroup].Settings);
					}
				}
			}
			if (CurrentActiveGroup != EnemyGroup.None)
			{
				if (NoiseBlip == null)
				{
					NoiseBlip = GlobalHelper.CreateBlip(Game.Player.Character.Position, 1f);
					NoiseBlip.Color = BlipColor.Red;
					NoiseBlip.Alpha = 144;
					return;
				}
				if (NoiseBlip != null)
				{
					NoiseBlip.Color = BlipColor.Red;
					NoiseBlip.Position = Game.Player.Character.Position;
					NoiseBlip.Alpha = 144;
				}
				if (NoiseThisFrame != Function.Call<float>(Hash.GET_PLAYER_CURRENT_STEALTH_NOISE, Game.Player) * 0.25f)
				{
					if (NoiseBlip != null)
					{
						NoiseBlip.Delete();
					}
					NoiseThisFrame = Function.Call<float>(Hash.GET_PLAYER_CURRENT_STEALTH_NOISE, Game.Player) * 0.25f;
					NoiseBlip = GlobalHelper.CreateBlip(Game.Player.Character.Position, Function.Call<float>(Hash.GET_PLAYER_CURRENT_STEALTH_NOISE, Game.Player) * 0.25f);
					NoiseBlip.Color = BlipColor.Red;
					NoiseBlip.Alpha = 144;
				}
			}
			else if (NoiseBlip != null)
			{
				NoiseBlip.Delete();
				NoiseBlip = null;
			}
		}

		public static void ForceAIAttackPlayer(SmartAiV2 P)
		{
			RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
			Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, P.AIPed, relationshipGroup);
			Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, P.AIPed, 0, 0);
			Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, P.AIPed, 1);
			FightAgainst(P, Game.Player.Character);
			P.HasSeenPlayer = true;
			P.IsinCombatAgainstTarget = true;
			P.CanSeeDeadBody = false;
		}

		public static void TASK_GO_TO_COORD_WHILE_AIMING_AT_ENTITY(Ped ped, Vector3 Pos, Entity aimAtID, float moveBlendRatio, bool shoot, float targetRadius, float slowDistance, bool useNavMesh, int navFlags, bool instantBlendToAim, uint firingPattern, int time)
		{
			Function.Call(Hash.TASK_GO_TO_COORD_WHILE_AIMING_AT_ENTITY, ped, Pos.X, Pos.Y, Pos.Z, aimAtID, moveBlendRatio, shoot, targetRadius, slowDistance, useNavMesh, navFlags, instantBlendToAim, firingPattern, time);
		}

		public static void TASK_GO_TO_COORD_WHILE_AIMING_AT_ENTITY(int ped, Vector3 Pos, Entity aimAtID, float moveBlendRatio, bool shoot, float targetRadius, float slowDistance, bool useNavMesh, int navFlags, bool instantBlendToAim, uint firingPattern, int time)
		{
			Function.Call(Hash.TASK_GO_TO_COORD_WHILE_AIMING_AT_ENTITY, ped, Pos.X, Pos.Y, Pos.Z, aimAtID, moveBlendRatio, shoot, targetRadius, slowDistance, useNavMesh, navFlags, instantBlendToAim, firingPattern, time);
		}

		public static void FightAgainst(SmartAiV2 Ai, Ped Target)
		{
			if (Ai.AiProp != null)
			{
				Ai.AiProp.Delete();
			}
			if (Ai.AIPed != null)
			{
				Ai.AIPed.Task.ClearAllImmediately();
				Ai.AIPed.Task.Combat(Game.Player.Character);
			}
		}

		public static void ClearDeadPeds(bool FadeOut)
		{
			if (FadeOut)
			{
				for (int i = 0; i < DeadPeds.Count; i++)
				{
					Ped ped = DeadPeds[i];
					if (ped != null)
					{
						ped.MarkAsNoLongerNeeded();
						SmartEntityFlickerEffect.VehicleFadeEffects.Add(new SmartEntityFlickerEffect.FadeEffect(ped, false, 0));
					}
					DeadPeds.RemoveAt(i);
				}
			}
			else
			{
				foreach (Ped deadPed in DeadPeds)
				{
					if (deadPed != null)
					{
						deadPed.Delete();
					}
				}
			}
			DeadPeds.Clear();
		}

		public static void RemoveAIGuards()
		{
			ActiveGroups.Clear();
			CurrentActiveGroup = EnemyGroup.None;
			foreach (Ped deadPed in DeadPeds)
			{
				if (deadPed != null)
				{
					deadPed.Delete();
				}
			}
			DeadPeds.Clear();
			foreach (EnemyGroupData item in GuardGroup)
			{
				foreach (SmartAiV2 group in item.GroupList)
				{
					if (group.AIPed != null)
					{
						group.AIPed.Delete();
					}
					if (group.AiProp != null)
					{
						group.AiProp.Delete();
					}
					if (group.LastVehicle != null && group.LastVehicle.GetPedOnSeat(VehicleSeat.Driver) != Game.Player.Character)
					{
						group.LastVehicle.Delete();
					}
				}
				item.GroupList.Clear();
				foreach (SmartAiV2 reinforcements in item.ReinforcementsList)
				{
					if (reinforcements.AIPed != null)
					{
						reinforcements.AIPed.Delete();
					}
					if (reinforcements.AiProp != null)
					{
						reinforcements.AiProp.Delete();
					}
					if (reinforcements.LastVehicle != null && reinforcements.LastVehicle.GetPedOnSeat(VehicleSeat.Driver) != Game.Player.Character)
					{
						reinforcements.LastVehicle.Delete();
					}
				}
				item.ReinforcementsList.Clear();
			}
			CurrentActiveGroup = EnemyGroup.None;
			if (NoiseBlip != null)
			{
				NoiseBlip.Delete();
				NoiseBlip = null;
			}
			foreach (SmartAiV2 item2 in ActiveSpawnGroup)
			{
				if (item2.AIPed != null)
				{
					item2.AIPed.Delete();
				}
				if (item2.AiProp != null)
				{
					item2.AiProp.Delete();
				}
				if (item2.LastVehicle != null && item2.LastVehicle.GetPedOnSeat(VehicleSeat.Driver) != Game.Player.Character)
				{
					item2.LastVehicle.Delete();
				}
			}
			foreach (Ped merryweatherPed in MerryweatherPeds)
			{
				if (merryweatherPed != null)
				{
					merryweatherPed.Delete();
				}
			}
			MerryweatherPeds.Clear();
			foreach (Vehicle merryweatherVehicle in MerryweatherVehicles)
			{
				if (merryweatherVehicle != null)
				{
					merryweatherVehicle.Delete();
				}
			}
			MerryweatherVehicles.Clear();
		}

		public void OnShutdown(object sender, EventArgs e)
		{
			ClearGenericGuards();
			RemoveAIGuards();
			if (NoiseBlip != null)
			{
				NoiseBlip.Delete();
			}
			foreach (SmartAiV2 item in SpawnGroupsUse)
			{
				if (item.AIPed != null)
				{
					item.AIPed.Delete();
				}
				if (item.AiProp != null)
				{
					item.AiProp.Delete();
				}
				if (item.LastVehicle != null && item.LastVehicle.GetPedOnSeat(VehicleSeat.Driver) != Game.Player.Character)
				{
					item.LastVehicle.Delete();
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

		public static float GetDistance(Vector3 _1, Vector3 _2)
		{
			return _1.DistanceTo(_2);
		}

		public static Model RequestModel(string Prop)
		{
			Model result = new Model(Prop);
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

		public static Model RequestModel(uint Prop)
		{
			Model result = new Model((int)Prop);
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

		public static Model RequestModel(int Prop)
		{
			Model result = new Model(Prop);
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

		public static Model RequestModel(VehicleHash Prop)
		{
			Model result = new Model(Prop);
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

		public static Model RequestModel(PedHash Prop)
		{
			Model result = new Model(Prop);
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

		public static void GiveRandomWeaponToPed(Ped AIPed, EnemyGroup EnemyGroup)
		{
			if (EnemyGroup == EnemyGroup.Eberhard_Munitions)
			{
				Random random = new Random();
				int num = random.Next(0, 100);
				if (num < 10)
				{
					AIPed.Weapons.Give(WeaponHash.SMG, 9999, true, true);
				}
				if (num >= 10 && num <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.CombatShotgun, 9999, true, true);
				}
				if (num >= 20 && num <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
				}
				if (num >= 30 && num <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.BullpupShotgun, 9999, true, true);
				}
				if (num >= 40 && num <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.SpecialCarbineMk2, 9999, true, true);
				}
				if (num >= 50 && num <= 60)
				{
					AIPed.Weapons.Give(WeaponHash.CarbineRifleMk2, 9999, true, true);
				}
				if (num >= 60 && num <= 70)
				{
					AIPed.Weapons.Give(WeaponHash.SMG, 9999, true, true);
				}
				if (num >= 70 && num <= 80)
				{
					AIPed.Weapons.Give(WeaponHash.CeramicPistol, 9999, true, true);
				}
				if (num >= 80 && num <= 90)
				{
					AIPed.Weapons.Give(WeaponHash.CombatShotgun, 9999, true, true);
				}
				if (num >= 90 && num <= 100)
				{
					AIPed.Weapons.Give(WeaponHash.ServiceCarbine, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.Aztecas)
			{
				Random random2 = new Random();
				int num2 = random2.Next(0, 80);
				if (num2 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
				if (num2 >= 10 && num2 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.MicroSMG, 9999, true, true);
				}
				if (num2 >= 20 && num2 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.MachinePistol, 9999, true, true);
				}
				if (num2 >= 30 && num2 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.SawnOffShotgun, 9999, true, true);
				}
				if (num2 >= 40 && num2 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.AssaultRifle, 9999, true, true);
				}
				if (num2 >= 50 && num2 <= 60)
				{
					AIPed.Weapons.Give(WeaponHash.CompactRifle, 9999, true, true);
				}
				if (num2 >= 60 && num2 <= 70)
				{
					AIPed.Weapons.Give(WeaponHash.CombatPistol, 9999, true, true);
				}
				if (num2 >= 70 && num2 <= 80)
				{
					AIPed.Weapons.Give(WeaponHash.DoubleBarrelShotgun, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.Gang)
			{
				Random random3 = new Random();
				int num3 = random3.Next(0, 80);
				if (num3 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
				if (num3 >= 10 && num3 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.MicroSMG, 9999, true, true);
				}
				if (num3 >= 20 && num3 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.MachinePistol, 9999, true, true);
				}
				if (num3 >= 30 && num3 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.SawnOffShotgun, 9999, true, true);
				}
				if (num3 >= 40 && num3 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.AssaultRifle, 9999, true, true);
				}
				if (num3 >= 50 && num3 <= 60)
				{
					AIPed.Weapons.Give(WeaponHash.CompactRifle, 9999, true, true);
				}
				if (num3 >= 60 && num3 <= 70)
				{
					AIPed.Weapons.Give(WeaponHash.CombatPistol, 9999, true, true);
				}
				if (num3 >= 70 && num3 <= 80)
				{
					AIPed.Weapons.Give(WeaponHash.DoubleBarrelShotgun, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.Marabaunte_Grande)
			{
				Random random4 = new Random();
				int num4 = random4.Next(0, 80);
				if (num4 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
				if (num4 >= 10 && num4 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.MicroSMG, 9999, true, true);
				}
				if (num4 >= 20 && num4 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.MachinePistol, 9999, true, true);
				}
				if (num4 >= 30 && num4 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.SawnOffShotgun, 9999, true, true);
				}
				if (num4 >= 40 && num4 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.AssaultRifle, 9999, true, true);
				}
				if (num4 >= 50 && num4 <= 60)
				{
					AIPed.Weapons.Give(WeaponHash.CompactRifle, 9999, true, true);
				}
				if (num4 >= 60 && num4 <= 70)
				{
					AIPed.Weapons.Give(WeaponHash.CombatPistol, 9999, true, true);
				}
				if (num4 >= 70 && num4 <= 80)
				{
					AIPed.Weapons.Give(WeaponHash.DoubleBarrelShotgun, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.Professionals)
			{
				Random random5 = new Random();
				int num5 = random5.Next(0, 100);
				if (num5 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.TacticalSMG, 9999, true, true);
				}
				if (num5 >= 10 && num5 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
				if (num5 >= 20 && num5 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.APPistol, 9999, true, true);
				}
				if (num5 >= 30 && num5 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.CombatShotgun, 9999, true, true);
				}
				if (num5 >= 40 && num5 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.HeavyRifle, 9999, true, true);
				}
				if (num5 >= 50 && num5 <= 60)
				{
					AIPed.Weapons.Give(WeaponHash.SMG, 9999, true, true);
				}
				if (num5 >= 60 && num5 <= 70)
				{
					AIPed.Weapons.Give(WeaponHash.PumpShotgunMk2, 9999, true, true);
				}
				if (num5 >= 70 && num5 <= 80)
				{
					AIPed.Weapons.Give(WeaponHash.APPistol, 9999, true, true);
				}
				if (num5 >= 80 && num5 <= 90)
				{
					AIPed.Weapons.Give(WeaponHash.AssaultSMG, 9999, true, true);
				}
				if (num5 >= 90 && num5 <= 100)
				{
					AIPed.Weapons.Give(WeaponHash.BattleRifle, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.Cartel)
			{
				Random random6 = new Random();
				int num6 = random6.Next(0, 100);
				if (num6 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.MicroSMG, 9999, true, true);
				}
				if (num6 >= 10 && num6 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
				if (num6 >= 20 && num6 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.APPistol, 9999, true, true);
				}
				if (num6 >= 30 && num6 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.SawnOffShotgun, 9999, true, true);
				}
				if (num6 >= 40 && num6 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.MachinePistol, 9999, true, true);
				}
				if (num6 >= 50 && num6 <= 60)
				{
					AIPed.Weapons.Give(WeaponHash.SMG, 9999, true, true);
				}
				if (num6 >= 60 && num6 <= 70)
				{
					AIPed.Weapons.Give(WeaponHash.SawnOffShotgun, 9999, true, true);
				}
				if (num6 >= 70 && num6 <= 80)
				{
					AIPed.Weapons.Give(WeaponHash.MicroSMG, 9999, true, true);
				}
				if (num6 >= 80 && num6 <= 90)
				{
					AIPed.Weapons.Give(WeaponHash.MachinePistol, 9999, true, true);
				}
				if (num6 >= 90 && num6 <= 100)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.Military || EnemyGroup == EnemyGroup.Merryweather)
			{
				Random random7 = new Random();
				int num7 = random7.Next(0, 60);
				if (num7 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.BattleRifle, 9999, true, true);
				}
				if (num7 >= 10 && num7 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
				}
				if (num7 >= 20 && num7 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.CombatShotgun, 9999, true, true);
				}
				if (num7 >= 30 && num7 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.CombatMG, 9999, true, true);
				}
				if (num7 >= 40 && num7 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.HeavyPistol, 9999, true, true);
				}
				if (num7 >= 50 && num7 <= 60)
				{
					AIPed.Weapons.Give(WeaponHash.SpecialCarbine, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.Cayo_Perico_Cartel)
			{
				Random random8 = new Random();
				int num8 = random8.Next(0, 100);
				if (num8 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.SMG, 9999, true, true);
				}
				if (num8 >= 10 && num8 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.CombatShotgun, 9999, true, true);
				}
				if (num8 >= 20 && num8 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
				}
				if (num8 >= 30 && num8 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.BullpupShotgun, 9999, true, true);
				}
				if (num8 >= 40 && num8 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.SpecialCarbineMk2, 9999, true, true);
				}
				if (num8 >= 50 && num8 <= 60)
				{
					AIPed.Weapons.Give(WeaponHash.CarbineRifleMk2, 9999, true, true);
				}
				if (num8 >= 60 && num8 <= 70)
				{
					AIPed.Weapons.Give(WeaponHash.SMG, 9999, true, true);
				}
				if (num8 >= 70 && num8 <= 80)
				{
					AIPed.Weapons.Give(WeaponHash.CeramicPistol, 9999, true, true);
				}
				if (num8 >= 80 && num8 <= 90)
				{
					AIPed.Weapons.Give(WeaponHash.CombatShotgun, 9999, true, true);
				}
				if (num8 >= 90 && num8 <= 100)
				{
					AIPed.Weapons.Give(WeaponHash.ServiceCarbine, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.Diamond_Casino_Security)
			{
				Random random9 = new Random();
				int num9 = random9.Next(0, 50);
				if (num9 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
				if (num9 >= 10 && num9 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.SMG, 9999, true, true);
				}
				if (num9 >= 20 && num9 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.PumpShotgun, 9999, true, true);
				}
				if (num9 >= 30 && num9 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.BullpupRifle, 9999, true, true);
				}
				if (num9 >= 40 && num9 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.BullpupShotgun, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.Enemy)
			{
				Random random10 = new Random();
				int num10 = random10.Next(0, 100);
				if (num10 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.SMG, 9999, true, true);
				}
				if (num10 >= 10 && num10 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.CombatShotgun, 9999, true, true);
				}
				if (num10 >= 20 && num10 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
				}
				if (num10 >= 30 && num10 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.BullpupShotgun, 9999, true, true);
				}
				if (num10 >= 40 && num10 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.SpecialCarbineMk2, 9999, true, true);
				}
				if (num10 >= 50 && num10 <= 60)
				{
					AIPed.Weapons.Give(WeaponHash.CarbineRifleMk2, 9999, true, true);
				}
				if (num10 >= 60 && num10 <= 70)
				{
					AIPed.Weapons.Give(WeaponHash.SMG, 9999, true, true);
				}
				if (num10 >= 70 && num10 <= 80)
				{
					AIPed.Weapons.Give(WeaponHash.CeramicPistol, 9999, true, true);
				}
				if (num10 >= 80 && num10 <= 90)
				{
					AIPed.Weapons.Give(WeaponHash.CombatShotgun, 9999, true, true);
				}
				if (num10 >= 90 && num10 <= 100)
				{
					AIPed.Weapons.Give(WeaponHash.ServiceCarbine, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.Security)
			{
				Random random11 = new Random();
				int num11 = random11.Next(0, 100);
				if (num11 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.TacticalSMG, 9999, true, true);
				}
				if (num11 >= 10 && num11 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
				if (num11 >= 20 && num11 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.APPistol, 9999, true, true);
				}
				if (num11 >= 30 && num11 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.CombatShotgun, 9999, true, true);
				}
				if (num11 >= 40 && num11 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.HeavyRifle, 9999, true, true);
				}
				if (num11 >= 50 && num11 <= 60)
				{
					AIPed.Weapons.Give(WeaponHash.SMG, 9999, true, true);
				}
				if (num11 >= 60 && num11 <= 70)
				{
					AIPed.Weapons.Give(WeaponHash.PumpShotgunMk2, 9999, true, true);
				}
				if (num11 >= 70 && num11 <= 80)
				{
					AIPed.Weapons.Give(WeaponHash.APPistol, 9999, true, true);
				}
				if (num11 >= 80 && num11 <= 90)
				{
					AIPed.Weapons.Give(WeaponHash.AssaultSMG, 9999, true, true);
				}
				if (num11 >= 90 && num11 <= 100)
				{
					AIPed.Weapons.Give(WeaponHash.BattleRifle, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.Hired_Gunmen)
			{
				Random random12 = new Random();
				int num12 = random12.Next(0, 100);
				if (num12 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.TacticalSMG, 9999, true, true);
				}
				if (num12 >= 10 && num12 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
				if (num12 >= 20 && num12 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.APPistol, 9999, true, true);
				}
				if (num12 >= 30 && num12 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.CombatShotgun, 9999, true, true);
				}
				if (num12 >= 40 && num12 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.HeavyRifle, 9999, true, true);
				}
				if (num12 >= 50 && num12 <= 60)
				{
					AIPed.Weapons.Give(WeaponHash.SMG, 9999, true, true);
				}
				if (num12 >= 60 && num12 <= 70)
				{
					AIPed.Weapons.Give(WeaponHash.PumpShotgunMk2, 9999, true, true);
				}
				if (num12 >= 70 && num12 <= 80)
				{
					AIPed.Weapons.Give(WeaponHash.APPistol, 9999, true, true);
				}
				if (num12 >= 80 && num12 <= 90)
				{
					AIPed.Weapons.Give(WeaponHash.AssaultSMG, 9999, true, true);
				}
				if (num12 >= 90 && num12 <= 100)
				{
					AIPed.Weapons.Give(WeaponHash.BattleRifle, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.Lost_MC)
			{
				AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				Random random13 = new Random();
				int num13 = random13.Next(0, 80);
				if (num13 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
				if (num13 >= 10 && num13 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.MicroSMG, 9999, true, true);
				}
				if (num13 >= 20 && num13 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.MachinePistol, 9999, true, true);
				}
				if (num13 >= 30 && num13 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.SawnOffShotgun, 9999, true, true);
				}
				if (num13 >= 40 && num13 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.MiniSMG, 9999, true, true);
				}
				if (num13 >= 50 && num13 <= 60)
				{
					AIPed.Weapons.Give(WeaponHash.CompactRifle, 9999, true, true);
				}
				if (num13 >= 60 && num13 <= 70)
				{
					AIPed.Weapons.Give(WeaponHash.CombatPistol, 9999, true, true);
				}
				if (num13 >= 70 && num13 <= 80)
				{
					AIPed.Weapons.Give(WeaponHash.DoubleBarrelShotgun, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.Vagos)
			{
				Random random14 = new Random();
				int num14 = random14.Next(0, 80);
				if (num14 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
				if (num14 >= 10 && num14 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.MicroSMG, 9999, true, true);
				}
				if (num14 >= 20 && num14 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.MachinePistol, 9999, true, true);
				}
				if (num14 >= 30 && num14 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.SawnOffShotgun, 9999, true, true);
				}
				if (num14 >= 40 && num14 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.AssaultRifle, 9999, true, true);
				}
				if (num14 >= 50 && num14 <= 60)
				{
					AIPed.Weapons.Give(WeaponHash.CompactRifle, 9999, true, true);
				}
				if (num14 >= 60 && num14 <= 70)
				{
					AIPed.Weapons.Give(WeaponHash.CombatPistol, 9999, true, true);
				}
				if (num14 >= 70 && num14 <= 80)
				{
					AIPed.Weapons.Give(WeaponHash.DoubleBarrelShotgun, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.Families)
			{
				Random random15 = new Random();
				int num15 = random15.Next(0, 80);
				if (num15 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
				if (num15 >= 10 && num15 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.MicroSMG, 9999, true, true);
				}
				if (num15 >= 20 && num15 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.MachinePistol, 9999, true, true);
				}
				if (num15 >= 30 && num15 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.SawnOffShotgun, 9999, true, true);
				}
				if (num15 >= 40 && num15 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.AssaultRifle, 9999, true, true);
				}
				if (num15 >= 50 && num15 <= 60)
				{
					AIPed.Weapons.Give(WeaponHash.CompactRifle, 9999, true, true);
				}
				if (num15 >= 60 && num15 <= 70)
				{
					AIPed.Weapons.Give(WeaponHash.CombatPistol, 9999, true, true);
				}
				if (num15 >= 70 && num15 <= 80)
				{
					AIPed.Weapons.Give(WeaponHash.DoubleBarrelShotgun, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.Ballas)
			{
				Random random16 = new Random();
				int num16 = random16.Next(0, 80);
				if (num16 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
				if (num16 >= 10 && num16 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.MicroSMG, 9999, true, true);
				}
				if (num16 >= 20 && num16 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.MachinePistol, 9999, true, true);
				}
				if (num16 >= 30 && num16 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.SawnOffShotgun, 9999, true, true);
				}
				if (num16 >= 40 && num16 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.AssaultRifle, 9999, true, true);
				}
				if (num16 >= 50 && num16 <= 60)
				{
					AIPed.Weapons.Give(WeaponHash.CompactRifle, 9999, true, true);
				}
				if (num16 >= 60 && num16 <= 70)
				{
					AIPed.Weapons.Give(WeaponHash.CombatPistol, 9999, true, true);
				}
				if (num16 >= 70 && num16 <= 80)
				{
					AIPed.Weapons.Give(WeaponHash.DoubleBarrelShotgun, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.FIB)
			{
				Random random17 = new Random();
				int num17 = random17.Next(0, 100);
				if (num17 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.SMG, 9999, true, true);
				}
				if (num17 >= 10 && num17 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
				}
				if (num17 >= 20 && num17 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.PumpShotgun, 9999, true, true);
				}
				if (num17 >= 30 && num17 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.SpecialCarbine, 9999, true, true);
				}
				if (num17 >= 40 && num17 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
				if (num17 >= 50 && num17 <= 60)
				{
					AIPed.Weapons.Give(WeaponHash.CombatPDW, 9999, true, true);
				}
				if (num17 >= 60 && num17 <= 70)
				{
					AIPed.Weapons.Give(WeaponHash.BullpupRifle, 9999, true, true);
				}
				if (num17 >= 70 && num17 <= 80)
				{
					AIPed.Weapons.Give(WeaponHash.CeramicPistol, 9999, true, true);
				}
				if (num17 >= 80 && num17 <= 90)
				{
					AIPed.Weapons.Give(WeaponHash.CombatShotgun, 9999, true, true);
				}
				if (num17 >= 90 && num17 <= 100)
				{
					AIPed.Weapons.Give(WeaponHash.BullpupShotgun, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.Triads)
			{
				Random random18 = new Random();
				int num18 = random18.Next(0, 80);
				if (num18 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
				if (num18 >= 10 && num18 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.MicroSMG, 9999, true, true);
				}
				if (num18 >= 20 && num18 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.MachinePistol, 9999, true, true);
				}
				if (num18 >= 30 && num18 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.SawnOffShotgun, 9999, true, true);
				}
				if (num18 >= 40 && num18 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.AssaultRifle, 9999, true, true);
				}
				if (num18 >= 50 && num18 <= 60)
				{
					AIPed.Weapons.Give(WeaponHash.CompactRifle, 9999, true, true);
				}
				if (num18 >= 60 && num18 <= 70)
				{
					AIPed.Weapons.Give(WeaponHash.CombatPistol, 9999, true, true);
				}
				if (num18 >= 70 && num18 <= 80)
				{
					AIPed.Weapons.Give(WeaponHash.DoubleBarrelShotgun, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.Hippies)
			{
				Random random19 = new Random();
				int num19 = random19.Next(0, 80);
				if (num19 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
				if (num19 >= 10 && num19 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.MicroSMG, 9999, true, true);
				}
				if (num19 >= 20 && num19 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.MachinePistol, 9999, true, true);
				}
				if (num19 >= 30 && num19 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.SawnOffShotgun, 9999, true, true);
				}
				if (num19 >= 40 && num19 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.AssaultRifle, 9999, true, true);
				}
				if (num19 >= 50 && num19 <= 60)
				{
					AIPed.Weapons.Give(WeaponHash.CompactRifle, 9999, true, true);
				}
				if (num19 >= 60 && num19 <= 70)
				{
					AIPed.Weapons.Give(WeaponHash.CombatPistol, 9999, true, true);
				}
				if (num19 >= 70 && num19 <= 80)
				{
					AIPed.Weapons.Give(WeaponHash.DoubleBarrelShotgun, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.Rednecks)
			{
				Random random20 = new Random();
				int num20 = random20.Next(0, 80);
				if (num20 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
				if (num20 >= 10 && num20 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.MicroSMG, 9999, true, true);
				}
				if (num20 >= 20 && num20 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.MachinePistol, 9999, true, true);
				}
				if (num20 >= 30 && num20 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.SawnOffShotgun, 9999, true, true);
				}
				if (num20 >= 40 && num20 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.AssaultRifle, 9999, true, true);
				}
				if (num20 >= 50 && num20 <= 60)
				{
					AIPed.Weapons.Give(WeaponHash.CompactRifle, 9999, true, true);
				}
				if (num20 >= 60 && num20 <= 70)
				{
					AIPed.Weapons.Give(WeaponHash.CombatPistol, 9999, true, true);
				}
				if (num20 >= 70 && num20 <= 80)
				{
					AIPed.Weapons.Give(WeaponHash.DoubleBarrelShotgun, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.Kkangpae)
			{
				Random random21 = new Random();
				int num21 = random21.Next(0, 80);
				if (num21 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
				if (num21 >= 10 && num21 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.MicroSMG, 9999, true, true);
				}
				if (num21 >= 20 && num21 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.MachinePistol, 9999, true, true);
				}
				if (num21 >= 30 && num21 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.SawnOffShotgun, 9999, true, true);
				}
				if (num21 >= 40 && num21 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.AssaultRifle, 9999, true, true);
				}
				if (num21 >= 50 && num21 <= 60)
				{
					AIPed.Weapons.Give(WeaponHash.CompactRifle, 9999, true, true);
				}
				if (num21 >= 60 && num21 <= 70)
				{
					AIPed.Weapons.Give(WeaponHash.CombatPistol, 9999, true, true);
				}
				if (num21 >= 70 && num21 <= 80)
				{
					AIPed.Weapons.Give(WeaponHash.DoubleBarrelShotgun, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.Altruist_Cult)
			{
				Random random22 = new Random();
				int num22 = random22.Next(0, 80);
				if (num22 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
				if (num22 >= 10 && num22 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.MicroSMG, 9999, true, true);
				}
				if (num22 >= 20 && num22 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.MachinePistol, 9999, true, true);
				}
				if (num22 >= 30 && num22 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.SawnOffShotgun, 9999, true, true);
				}
				if (num22 >= 40 && num22 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.AssaultRifle, 9999, true, true);
				}
				if (num22 >= 50 && num22 <= 60)
				{
					AIPed.Weapons.Give(WeaponHash.CompactRifle, 9999, true, true);
				}
				if (num22 >= 60 && num22 <= 70)
				{
					AIPed.Weapons.Give(WeaponHash.CombatPistol, 9999, true, true);
				}
				if (num22 >= 70 && num22 <= 80)
				{
					AIPed.Weapons.Give(WeaponHash.DoubleBarrelShotgun, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.Armenian_Mob)
			{
				Random random23 = new Random();
				int num23 = random23.Next(0, 80);
				if (num23 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
				if (num23 >= 10 && num23 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.MicroSMG, 9999, true, true);
				}
				if (num23 >= 20 && num23 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.MachinePistol, 9999, true, true);
				}
				if (num23 >= 30 && num23 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.SawnOffShotgun, 9999, true, true);
				}
				if (num23 >= 40 && num23 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.AssaultRifle, 9999, true, true);
				}
				if (num23 >= 50 && num23 <= 60)
				{
					AIPed.Weapons.Give(WeaponHash.CompactRifle, 9999, true, true);
				}
				if (num23 >= 60 && num23 <= 70)
				{
					AIPed.Weapons.Give(WeaponHash.CombatPistol, 9999, true, true);
				}
				if (num23 >= 70 && num23 <= 80)
				{
					AIPed.Weapons.Give(WeaponHash.DoubleBarrelShotgun, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.High_End_Security)
			{
				Random random24 = new Random();
				int num24 = random24.Next(0, 50);
				if (num24 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
				if (num24 >= 10 && num24 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.SMG, 9999, true, true);
				}
				if (num24 >= 20 && num24 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.PumpShotgun, 9999, true, true);
				}
				if (num24 >= 30 && num24 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.BullpupRifle, 9999, true, true);
				}
				if (num24 >= 40 && num24 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.BullpupShotgun, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.Swat)
			{
				Random random25 = new Random();
				int num25 = random25.Next(0, 50);
				if (num25 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
				if (num25 >= 10 && num25 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.SMG, 9999, true, true);
				}
				if (num25 >= 20 && num25 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.PumpShotgun, 9999, true, true);
				}
				if (num25 >= 30 && num25 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
				}
				if (num25 >= 40 && num25 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.BullpupShotgun, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.LSPD)
			{
				Random random26 = new Random();
				int num26 = random26.Next(0, 50);
				if (num26 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
				if (num26 >= 10 && num26 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.SMG, 9999, true, true);
				}
				if (num26 >= 20 && num26 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.PumpShotgun, 9999, true, true);
				}
				if (num26 >= 30 && num26 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.BullpupRifle, 9999, true, true);
				}
				if (num26 >= 40 && num26 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.BullpupShotgun, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.Gruppe_Sechs)
			{
				Random random27 = new Random();
				int num27 = random27.Next(0, 50);
				if (num27 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
				if (num27 >= 10 && num27 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.SMG, 9999, true, true);
				}
				if (num27 >= 20 && num27 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.PumpShotgun, 9999, true, true);
				}
				if (num27 >= 30 && num27 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
				}
				if (num27 >= 40 && num27 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.BullpupShotgun, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.IAA)
			{
				Random random28 = new Random();
				int num28 = random28.Next(0, 100);
				if (num28 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.SMG, 9999, true, true);
				}
				if (num28 >= 10 && num28 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
				}
				if (num28 >= 20 && num28 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.PumpShotgun, 9999, true, true);
				}
				if (num28 >= 30 && num28 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.SpecialCarbine, 9999, true, true);
				}
				if (num28 >= 40 && num28 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
				if (num28 >= 50 && num28 <= 60)
				{
					AIPed.Weapons.Give(WeaponHash.CombatPDW, 9999, true, true);
				}
				if (num28 >= 60 && num28 <= 70)
				{
					AIPed.Weapons.Give(WeaponHash.BullpupRifle, 9999, true, true);
				}
				if (num28 >= 70 && num28 <= 80)
				{
					AIPed.Weapons.Give(WeaponHash.CeramicPistol, 9999, true, true);
				}
				if (num28 >= 80 && num28 <= 90)
				{
					AIPed.Weapons.Give(WeaponHash.CombatShotgun, 9999, true, true);
				}
				if (num28 >= 90 && num28 <= 100)
				{
					AIPed.Weapons.Give(WeaponHash.BullpupShotgun, 9999, true, true);
				}
			}
			if (EnemyGroup == EnemyGroup.Street_Racers)
			{
				Random random29 = new Random();
				int num29 = random29.Next(0, 80);
				if (num29 < 10)
				{
					AIPed.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
				}
				if (num29 >= 10 && num29 <= 20)
				{
					AIPed.Weapons.Give(WeaponHash.MicroSMG, 9999, true, true);
				}
				if (num29 >= 20 && num29 <= 30)
				{
					AIPed.Weapons.Give(WeaponHash.MachinePistol, 9999, true, true);
				}
				if (num29 >= 30 && num29 <= 40)
				{
					AIPed.Weapons.Give(WeaponHash.SawnOffShotgun, 9999, true, true);
				}
				if (num29 >= 40 && num29 <= 50)
				{
					AIPed.Weapons.Give(WeaponHash.AssaultRifle, 9999, true, true);
				}
				if (num29 >= 50 && num29 <= 60)
				{
					AIPed.Weapons.Give(WeaponHash.CompactRifle, 9999, true, true);
				}
				if (num29 >= 60 && num29 <= 70)
				{
					AIPed.Weapons.Give(WeaponHash.CombatPistol, 9999, true, true);
				}
				if (num29 >= 70 && num29 <= 80)
				{
					AIPed.Weapons.Give(WeaponHash.DoubleBarrelShotgun, 9999, true, true);
				}
			}
		}

		public static int GetAccuracy(SmartAI_Settings settings)
		{
			return Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, settings.MinAccuracy, settings.MaxAccuracy);
		}

		public static void SetupEberhard_Terminus(Vehicle vehicle)
		{
			Function.Call(Hash.SET_VEHICLE_MOD_KIT, vehicle.Handle, 0);
			vehicle.Mods[VehicleModType.Transmission].Index = 2;
			vehicle.Mods[VehicleModType.Windows].Index = 0;
			vehicle.Mods[VehicleModType.Roof].Index = 4;
			vehicle.Mods[VehicleModType.ArchCover].Index = 4;
			vehicle.Mods[VehicleModType.Armor].Index = 4;
			vehicle.Mods[VehicleModType.Brakes].Index = 2;
			vehicle.Mods[VehicleModType.Engine].Index = 3;
			vehicle.Mods[VehicleModType.Exhaust].Index = 3;
			vehicle.Mods[VehicleModType.Fender].Index = 3;
			vehicle.Mods[VehicleModType.Frame].Index = 0;
			vehicle.Mods[VehicleModType.FrontBumper].Index = 5;
			vehicle.Mods[VehicleModType.Grille].Index = 6;
			vehicle.Mods[VehicleModType.Hood].Index = 8;
			vehicle.Mods[VehicleModType.RearBumper].Index = 3;
			vehicle.Mods[VehicleModType.SideSkirt].Index = 0;
			vehicle.Mods[VehicleModType.Spoilers].Index = 4;
			vehicle.Mods.PearlescentColor = VehicleColor.MetallicBlack;
			int num = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100);
			if (num <= 60)
			{
				vehicle.Mods.PrimaryColor = VehicleColor.MetallicBlue;
				vehicle.Mods.SecondaryColor = VehicleColor.MetallicBlue;
			}
			if (num > 60)
			{
				vehicle.Mods.PrimaryColor = VehicleColor.MetallicSilver;
				vehicle.Mods.SecondaryColor = VehicleColor.MetallicSilver;
			}
		}

		public static Vehicle SpawnMerryweatherCar_Reinforcements(Vector3 Pos, float H, string Model, bool SpawnDriverOnly, List<uint> EnemyHashes, EnemyGroup enemyGroup, int RelationshipGroup = 0, int MaxSeats = -1, bool IsReinforcements = false)
		{
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedVehicles++;
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedCars++;
			List<Ped> list = new List<Ped>();
			Pos = GenerateSpawnPos(Pos, Nodetype.AnyRoad, false);
			Vehicle vehicle = GlobalHelper.CreateVehicle(RequestModel(Model), Pos, H);
			SmartAI_SpawnedVehicles.Add(vehicle);
			vehicle.LodDistance = 3000;
			MerryweatherVehicles.Add(vehicle);
			int passengerCapacity = vehicle.PassengerCapacity;
			passengerCapacity = ((MaxSeats != -1) ? MaxSeats : vehicle.PassengerCapacity);
			if (SpawnDriverOnly)
			{
				passengerCapacity = 0;
			}
			for (int i = -1; i < passengerCapacity; i++)
			{
				uint prop = EnemyHashes[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, EnemyHashes.Count)];
				Ped ped = GlobalHelper.CreatePed(RequestModel(prop), Pos, H);
				ped.Health = GuardGroup[(int)enemyGroup].Settings.Health;
				ped.Armor = GuardGroup[(int)enemyGroup].Settings.Armor;
				ped.Accuracy = GetAccuracy(GuardGroup[(int)enemyGroup].Settings);
				list.Add(ped);
				ped.SetIntoVehicle(vehicle, (VehicleSeat)i);
				SmartAiV2 smartAiV = new SmartAiV2(ped, enemyGroup);
				smartAiV.EnemyGroupName = enemyGroup;
				smartAiV.LastVehicle = vehicle;
				smartAiV.HasSeenPlayer = true;
				if (!IsReinforcements)
				{
					GuardGroup[(int)enemyGroup].GroupList.Add(smartAiV);
				}
				else
				{
					GuardGroup[(int)enemyGroup].ReinforcementsList.Add(smartAiV);
				}
				ped.Task.FightAgainst(Game.Player.Character);
				GiveRandomWeaponToPed(ped, enemyGroup);
				GetFiringPattern(ped);
				if (RelationshipGroup == 0)
				{
					RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
					Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped, relationshipGroup);
					Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 0, 0);
					Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, ped, 1);
				}
				else
				{
					RelationshipGroup relationshipGroup2 = RelationshipGroup;
					Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped, relationshipGroup2);
					Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 0, 0);
					Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, ped, 1);
				}
				AiBackup.Add(ped);
			}
			if (list[0] != null)
			{
				Function.Call(Hash.TASK_COMBAT_PED, list[0], Game.Player.Character, 0, 16);
			}
			return vehicle;
		}

		public static Vehicle SpawnMerryweatherCar_Reinforcements(Vector3 Pos, float H, VehicleHash Model, bool SpawnDriverOnly, List<uint> EnemyHashes, EnemyGroup enemyGroup, int RelationshipGroup = 0, int MaxSeats = -1, bool IsReinforcements = false)
		{
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedVehicles++;
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedCars++;
			List<Ped> list = new List<Ped>();
			Pos = GenerateSpawnPos(Pos, Nodetype.AnyRoad, false);
			Vehicle vehicle = GlobalHelper.CreateVehicle(RequestModel(Model), Pos, H);
			SmartAI_SpawnedVehicles.Add(vehicle);
			vehicle.LodDistance = 3000;
			MerryweatherVehicles.Add(vehicle);
			int passengerCapacity = vehicle.PassengerCapacity;
			passengerCapacity = ((MaxSeats != -1) ? MaxSeats : vehicle.PassengerCapacity);
			if (SpawnDriverOnly)
			{
				passengerCapacity = 0;
			}
			for (int i = -1; i < passengerCapacity; i++)
			{
				uint prop = EnemyHashes[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, EnemyHashes.Count)];
				Ped ped = GlobalHelper.CreatePed(RequestModel(prop), Pos, H);
				ped.Health = GuardGroup[(int)enemyGroup].Settings.Health;
				ped.Armor = GuardGroup[(int)enemyGroup].Settings.Armor;
				ped.Accuracy = GetAccuracy(GuardGroup[(int)enemyGroup].Settings);
				list.Add(ped);
				ped.SetIntoVehicle(vehicle, (VehicleSeat)i);
				SmartAiV2 smartAiV = new SmartAiV2(ped, enemyGroup);
				smartAiV.EnemyGroupName = enemyGroup;
				smartAiV.LastVehicle = vehicle;
				smartAiV.HasSeenPlayer = true;
				if (!IsReinforcements)
				{
					GuardGroup[(int)enemyGroup].GroupList.Add(smartAiV);
				}
				else
				{
					GuardGroup[(int)enemyGroup].ReinforcementsList.Add(smartAiV);
				}
				ped.Task.FightAgainst(Game.Player.Character);
				GiveRandomWeaponToPed(ped, enemyGroup);
				GetFiringPattern(ped);
				if (RelationshipGroup == 0)
				{
					RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
					Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped, relationshipGroup);
					Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 0, 0);
					Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, ped, 1);
				}
				else
				{
					RelationshipGroup relationshipGroup2 = RelationshipGroup;
					Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped, relationshipGroup2);
					Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 0, 0);
					Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, ped, 1);
				}
				AiBackup.Add(ped);
			}
			if (list[0] != null)
			{
				Function.Call(Hash.TASK_COMBAT_PED, list[0], Game.Player.Character, 0, 16);
			}
			return vehicle;
		}

		public static Vehicle SpawnEberhardTerminus_Reinforcements(Vector3 Pos, float H, bool SpawnDriverOnly, List<uint> EnemyHashes, EnemyGroup enemyGroup, bool IsReinforcements = false)
		{
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedVehicles++;
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedCars++;
			List<Ped> list = new List<Ped>();
			Pos = GenerateSpawnPos(Pos, Nodetype.AnyRoad, false);
			Vehicle vehicle = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Terminus), Pos, H);
			SetupEberhard_Terminus(vehicle);
			SmartAI_SpawnedVehicles.Add(vehicle);
			vehicle.LodDistance = 3000;
			MerryweatherVehicles.Add(vehicle);
			int num = vehicle.PassengerCapacity;
			if (SpawnDriverOnly)
			{
				num = 0;
			}
			for (int i = -1; i < num; i++)
			{
				uint prop = EnemyHashes[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, EnemyHashes.Count)];
				Ped ped = GlobalHelper.CreatePed(RequestModel(prop), Pos, H);
				ped.Health = GuardGroup[(int)enemyGroup].Settings.Health;
				ped.Armor = GuardGroup[(int)enemyGroup].Settings.Armor;
				ped.Accuracy = GetAccuracy(GuardGroup[(int)enemyGroup].Settings);
				list.Add(ped);
				ped.SetIntoVehicle(vehicle, (VehicleSeat)i);
				SmartAiV2 smartAiV = new SmartAiV2(ped, enemyGroup);
				smartAiV.LastVehicle = vehicle;
				smartAiV.EnemyGroupName = enemyGroup;
				smartAiV.HasSeenPlayer = true;
				if (!IsReinforcements)
				{
					GuardGroup[(int)enemyGroup].GroupList.Add(smartAiV);
				}
				else
				{
					GuardGroup[(int)enemyGroup].ReinforcementsList.Add(smartAiV);
				}
				ped.Task.FightAgainst(Game.Player.Character);
				GiveRandomWeaponToPed(ped, enemyGroup);
				GetFiringPattern(ped);
				RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
				Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped, relationshipGroup);
				Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 0, 0);
				Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, ped, 1);
				AiBackup.Add(ped);
			}
			if (list[0] != null)
			{
				Function.Call(Hash.TASK_COMBAT_PED, list[0], Game.Player.Character, 0, 16);
			}
			return vehicle;
		}

		public static Vehicle SpawnMerryweatherHeli_Reinforcements(Vector3 Pos, float H, string Model, bool SpawnDriverOnly, List<uint> EnemyHashes, EnemyGroup enemyGroup, bool IsReinforcements = false)
		{
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedVehicles++;
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedHelis++;
			List<Ped> list = new List<Ped>();
			Vehicle vehicle = GlobalHelper.CreateVehicle(RequestModel(Model), Pos, H);
			SmartAI_SpawnedVehicles.Add(vehicle);
			vehicle.LodDistance = 3000;
			MerryweatherVehicles.Add(vehicle);
			int num = vehicle.PassengerCapacity;
			if (SpawnDriverOnly)
			{
				num = 0;
			}
			for (int i = -1; i < num; i++)
			{
				uint prop = EnemyHashes[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, EnemyHashes.Count)];
				Ped ped = GlobalHelper.CreatePed(RequestModel(prop), Pos, H);
				ped.Health = GuardGroup[(int)enemyGroup].Settings.Health;
				ped.Armor = GuardGroup[(int)enemyGroup].Settings.Armor;
				ped.Accuracy = GetAccuracy(GuardGroup[(int)enemyGroup].Settings);
				list.Add(ped);
				ped.SetIntoVehicle(vehicle, (VehicleSeat)i);
				SmartAiV2 smartAiV = new SmartAiV2(ped, enemyGroup);
				smartAiV.LastVehicle = vehicle;
				smartAiV.EnemyGroupName = enemyGroup;
				smartAiV.HasSeenPlayer = true;
				if (!IsReinforcements)
				{
					GuardGroup[(int)enemyGroup].GroupList.Add(smartAiV);
				}
				else
				{
					GuardGroup[(int)enemyGroup].ReinforcementsList.Add(smartAiV);
				}
				GiveRandomWeaponToPed(ped, enemyGroup);
				GetFiringPattern(ped);
				RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
				Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped, relationshipGroup);
				Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 0, 0);
				Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, ped, 1);
				AiBackup.Add(ped);
			}
			if (list[0] != null)
			{
				Vector3 position = Game.Player.Character.Position;
				Function.Call(Hash.TASK_HELI_MISSION, vehicle.Driver.Handle, vehicle.Handle, 0, Game.Player.Character, 0f, 0f, 0f, 6, 50f, 60f, -1, 120, 60, -1, 64);
			}
			return vehicle;
		}

		public static Vehicle SpawnMerryweatherBoat_Reinforcements(Vector3 Pos, float H, string Model, bool SpawnDriverOnly, List<uint> EnemyHashes, EnemyGroup enemyGroup, bool IsReinforcements = false)
		{
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedVehicles++;
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedBoats++;
			List<Ped> list = new List<Ped>();
			Pos = GenerateSpawnPos(Pos, Nodetype.Water, false);
			Vehicle vehicle = GlobalHelper.CreateVehicle(RequestModel(Model), Pos, H);
			SmartAI_SpawnedVehicles.Add(vehicle);
			vehicle.LodDistance = 3000;
			MerryweatherVehicles.Add(vehicle);
			int num = vehicle.PassengerCapacity;
			if (SpawnDriverOnly)
			{
				num = 0;
			}
			for (int i = -1; i < num; i++)
			{
				Ped ped = GlobalHelper.CreatePed(RequestModel(PedHash.Blackops01SMY), Pos, H);
				ped.Health = GuardGroup[(int)enemyGroup].Settings.Health;
				ped.Armor = GuardGroup[(int)enemyGroup].Settings.Armor;
				ped.Accuracy = GetAccuracy(GuardGroup[(int)enemyGroup].Settings);
				list.Add(ped);
				ped.SetIntoVehicle(vehicle, (VehicleSeat)i);
				SmartAiV2 smartAiV = new SmartAiV2(ped, enemyGroup);
				smartAiV.LastVehicle = vehicle;
				smartAiV.EnemyGroupName = enemyGroup;
				smartAiV.HasSeenPlayer = true;
				if (!IsReinforcements)
				{
					GuardGroup[(int)enemyGroup].GroupList.Add(smartAiV);
				}
				else
				{
					GuardGroup[(int)enemyGroup].ReinforcementsList.Add(smartAiV);
				}
				ped.Task.FightAgainst(Game.Player.Character);
				GiveRandomWeaponToPed(ped, enemyGroup);
				GetFiringPattern(ped);
				RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
				Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped, relationshipGroup);
				Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 0, 0);
				Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, ped, 1);
				AiBackup.Add(ped);
			}
			if (list[0] != null)
			{
				Function.Call(Hash.TASK_COMBAT_PED, list[0], Game.Player.Character, 0, 16);
				Vector3 position = Game.Player.Character.Position;
			}
			return vehicle;
		}

		public static Vehicle SpawnMerryweatherPlane_Reinforcements(Vector3 Pos, float H, string Model, bool SpawnDriverOnly, List<uint> EnemyHashes, EnemyGroup enemyGroup, bool IsReinforcements = false)
		{
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedVehicles++;
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedPlanes++;
			List<Ped> list = new List<Ped>();
			Vehicle vehicle = GlobalHelper.CreateVehicle(RequestModel(Model), Pos, H);
			SmartAI_SpawnedVehicles.Add(vehicle);
			vehicle.LodDistance = 3000;
			vehicle.ForwardSpeed = 100f;
			MerryweatherVehicles.Add(vehicle);
			int num = vehicle.PassengerCapacity;
			if (SpawnDriverOnly)
			{
				num = 0;
			}
			for (int i = -1; i < num; i++)
			{
				uint prop = EnemyHashes[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, EnemyHashes.Count)];
				Ped ped = GlobalHelper.CreatePed(RequestModel(prop), Pos, H);
				ped.Health = GuardGroup[(int)enemyGroup].Settings.Health;
				ped.Armor = GuardGroup[(int)enemyGroup].Settings.Armor;
				ped.Accuracy = GetAccuracy(GuardGroup[(int)enemyGroup].Settings);
				list.Add(ped);
				ped.SetIntoVehicle(vehicle, (VehicleSeat)i);
				SmartAiV2 smartAiV = new SmartAiV2(ped, enemyGroup);
				smartAiV.EnemyGroupName = enemyGroup;
				smartAiV.LastVehicle = vehicle;
				smartAiV.HasSeenPlayer = true;
				if (!IsReinforcements)
				{
					GuardGroup[(int)enemyGroup].GroupList.Add(smartAiV);
				}
				else
				{
					GuardGroup[(int)enemyGroup].ReinforcementsList.Add(smartAiV);
				}
				GiveRandomWeaponToPed(ped, enemyGroup);
				GetFiringPattern(ped);
				RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
				Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped, relationshipGroup);
				Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 0, 0);
				Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, ped, 1);
				AiBackup.Add(ped);
			}
			vehicle.LandingGearState = VehicleLandingGearState.Retracted;
			if (Model.Equals("RAIJU"))
			{
				Function.Call((Hash)865331629040904805uL, vehicle, true);
			}
			if (list[0] != null)
			{
				Vector3 position = Game.Player.Character.Position;
				Vector3 vector = position;
				Function.Call(Hash.TASK_PLANE_MISSION, list[0], list[0].CurrentVehicle.Handle, 0, 0, vector.X, vector.Y, vector.Z, 6, 100f, 0f, 90f, 0, 150f);
			}
			return vehicle;
		}

		public static Vehicle SpawnEberhardPyro_Reinforcements(Vector3 Pos, float H, string Model, bool SpawnDriverOnly, List<uint> EnemyHashes, EnemyGroup enemyGroup, bool IsReinforcements = false)
		{
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedVehicles++;
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedPlanes++;
			List<Ped> list = new List<Ped>();
			Vehicle vehicle = GlobalHelper.CreateVehicle(RequestModel(Model), Pos, H);
			SmartAI_SpawnedVehicles.Add(vehicle);
			vehicle.LodDistance = 3000;
			vehicle.ForwardSpeed = 100f;
			MerryweatherVehicles.Add(vehicle);
			int num = vehicle.PassengerCapacity;
			vehicle.LandingGearState = VehicleLandingGearState.Retracted;
			vehicle.ForwardSpeed = 100f;
			Function.Call(Hash.SET_VEHICLE_MOD_KIT, vehicle.Handle, 0);
			vehicle.Mods.PrimaryColor = VehicleColor.MetallicBlack;
			vehicle.Mods.SecondaryColor = VehicleColor.MetallicBlack;
			vehicle.Mods[VehicleModType.Roof].Index = 0;
			if (SpawnDriverOnly)
			{
				num = 0;
			}
			for (int i = -1; i < num; i++)
			{
				uint prop = EnemyHashes[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, EnemyHashes.Count)];
				Ped ped = GlobalHelper.CreatePed(RequestModel(prop), Pos, H);
				ped.Health = GuardGroup[(int)enemyGroup].Settings.Health;
				ped.Armor = GuardGroup[(int)enemyGroup].Settings.Armor;
				ped.Accuracy = GetAccuracy(GuardGroup[(int)enemyGroup].Settings);
				list.Add(ped);
				ped.SetIntoVehicle(vehicle, (VehicleSeat)i);
				SmartAiV2 smartAiV = new SmartAiV2(ped, enemyGroup);
				smartAiV.LastVehicle = vehicle;
				smartAiV.EnemyGroupName = enemyGroup;
				smartAiV.HasSeenPlayer = true;
				if (!IsReinforcements)
				{
					GuardGroup[(int)enemyGroup].GroupList.Add(smartAiV);
				}
				else
				{
					GuardGroup[(int)enemyGroup].ReinforcementsList.Add(smartAiV);
				}
				GiveRandomWeaponToPed(ped, enemyGroup);
				GetFiringPattern(ped);
				RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
				Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped, relationshipGroup);
				Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 0, 0);
				Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, ped, 1);
				ped.CombatRange = CombatRange.Near;
				ped.CombatAbility = CombatAbility.Professional;
				ped.VehicleWeapon = VehicleWeaponHash.PlaneRocket;
				AiBackup.Add(ped);
			}
			if (Model.Equals("RAIJU"))
			{
				Function.Call((Hash)865331629040904805uL, vehicle, true);
			}
			if (list[0] != null)
			{
				Vector3 position = Game.Player.Character.Position;
				Vector3 vector = position;
				Function.Call(Hash.TASK_PLANE_MISSION, list[0], list[0].CurrentVehicle.Handle, 0, 0, vector.X, vector.Y, vector.Z, 6, 100f, 0f, 90f, 0, 150f);
			}
			return vehicle;
		}

		public static Vehicle SpawnMerryweatherCar(Vector3 Pos, float H, string Model, bool SpawnDriverOnly, List<uint> EnemyHashes, EnemyGroup enemyGroup, int RelationshipGroup = 0, int MaxSeats = -1)
		{
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedVehicles++;
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedCars++;
			List<Ped> list = new List<Ped>();
			Pos = GenerateSpawnPos(Pos, Nodetype.AnyRoad, false);
			Vehicle vehicle = GlobalHelper.CreateVehicle(RequestModel(Model), Pos, H);
			SmartAI_SpawnedVehicles.Add(vehicle);
			vehicle.LodDistance = 3000;
			MerryweatherVehicles.Add(vehicle);
			int passengerCapacity = vehicle.PassengerCapacity;
			passengerCapacity = ((MaxSeats != -1) ? MaxSeats : vehicle.PassengerCapacity);
			if (SpawnDriverOnly)
			{
				passengerCapacity = 0;
			}
			for (int i = -1; i < passengerCapacity; i++)
			{
				uint prop = EnemyHashes[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, EnemyHashes.Count)];
				Ped ped = GlobalHelper.CreatePed(RequestModel(prop), Pos, H);
				ped.Health = GuardGroup[(int)enemyGroup].Settings.Health;
				ped.Armor = GuardGroup[(int)enemyGroup].Settings.Armor;
				ped.Accuracy = GetAccuracy(GuardGroup[(int)enemyGroup].Settings);
				list.Add(ped);
				ped.SetIntoVehicle(vehicle, (VehicleSeat)i);
				SmartAiV2 smartAiV = new SmartAiV2(ped, enemyGroup);
				smartAiV.EnemyGroupName = enemyGroup;
				smartAiV.LastVehicle = vehicle;
				smartAiV.HasSeenPlayer = true;
				GuardGroup[(int)enemyGroup].GroupList.Add(smartAiV);
				ped.Task.FightAgainst(Game.Player.Character);
				GiveRandomWeaponToPed(ped, enemyGroup);
				GetFiringPattern(ped);
				if (RelationshipGroup == 0)
				{
					RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
					Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped, relationshipGroup);
					Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 0, 0);
					Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, ped, 1);
				}
				else
				{
					RelationshipGroup relationshipGroup2 = RelationshipGroup;
					Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped, relationshipGroup2);
					Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 0, 0);
					Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, ped, 1);
				}
				AiBackup.Add(ped);
			}
			if (list[0] != null)
			{
				Function.Call(Hash.TASK_COMBAT_PED, list[0], Game.Player.Character, 0, 16);
			}
			return vehicle;
		}

		public static Vehicle SpawnMerryweatherCar(Vector3 Pos, float H, VehicleHash Model, bool SpawnDriverOnly, List<uint> EnemyHashes, EnemyGroup enemyGroup, int RelationshipGroup = 0, int MaxSeats = -1)
		{
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedVehicles++;
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedCars++;
			List<Ped> list = new List<Ped>();
			Pos = GenerateSpawnPos(Pos, Nodetype.AnyRoad, false);
			Vehicle vehicle = GlobalHelper.CreateVehicle(RequestModel(Model), Pos, H);
			SmartAI_SpawnedVehicles.Add(vehicle);
			vehicle.LodDistance = 3000;
			MerryweatherVehicles.Add(vehicle);
			int passengerCapacity = vehicle.PassengerCapacity;
			passengerCapacity = ((MaxSeats != -1) ? MaxSeats : vehicle.PassengerCapacity);
			if (SpawnDriverOnly)
			{
				passengerCapacity = 0;
			}
			for (int i = -1; i < passengerCapacity; i++)
			{
				uint prop = EnemyHashes[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, EnemyHashes.Count)];
				Ped ped = GlobalHelper.CreatePed(RequestModel(prop), Pos, H);
				ped.Health = GuardGroup[(int)enemyGroup].Settings.Health;
				ped.Armor = GuardGroup[(int)enemyGroup].Settings.Armor;
				ped.Accuracy = GetAccuracy(GuardGroup[(int)enemyGroup].Settings);
				list.Add(ped);
				ped.SetIntoVehicle(vehicle, (VehicleSeat)i);
				SmartAiV2 smartAiV = new SmartAiV2(ped, enemyGroup);
				smartAiV.EnemyGroupName = enemyGroup;
				smartAiV.LastVehicle = vehicle;
				smartAiV.HasSeenPlayer = true;
				GuardGroup[(int)enemyGroup].GroupList.Add(smartAiV);
				ped.Task.FightAgainst(Game.Player.Character);
				GiveRandomWeaponToPed(ped, enemyGroup);
				GetFiringPattern(ped);
				if (RelationshipGroup == 0)
				{
					RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
					Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped, relationshipGroup);
					Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 0, 0);
					Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, ped, 1);
				}
				else
				{
					RelationshipGroup relationshipGroup2 = RelationshipGroup;
					Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped, relationshipGroup2);
					Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 0, 0);
					Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, ped, 1);
				}
				AiBackup.Add(ped);
			}
			if (list[0] != null)
			{
				Function.Call(Hash.TASK_COMBAT_PED, list[0], Game.Player.Character, 0, 16);
			}
			return vehicle;
		}

		public static Vehicle SpawnEberhardTerminus(Vector3 Pos, float H, bool SpawnDriverOnly, List<uint> EnemyHashes, EnemyGroup enemyGroup)
		{
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedVehicles++;
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedCars++;
			List<Ped> list = new List<Ped>();
			Pos = GenerateSpawnPos(Pos, Nodetype.AnyRoad, false);
			Vehicle vehicle = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Terminus), Pos, H);
			SetupEberhard_Terminus(vehicle);
			SmartAI_SpawnedVehicles.Add(vehicle);
			vehicle.LodDistance = 3000;
			MerryweatherVehicles.Add(vehicle);
			int num = vehicle.PassengerCapacity;
			if (SpawnDriverOnly)
			{
				num = 0;
			}
			for (int i = -1; i < num; i++)
			{
				uint prop = EnemyHashes[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, EnemyHashes.Count)];
				Ped ped = GlobalHelper.CreatePed(RequestModel(prop), Pos, H);
				ped.Health = GuardGroup[(int)enemyGroup].Settings.Health;
				ped.Armor = GuardGroup[(int)enemyGroup].Settings.Armor;
				ped.Accuracy = GetAccuracy(GuardGroup[(int)enemyGroup].Settings);
				list.Add(ped);
				ped.SetIntoVehicle(vehicle, (VehicleSeat)i);
				SmartAiV2 smartAiV = new SmartAiV2(ped, enemyGroup);
				smartAiV.LastVehicle = vehicle;
				smartAiV.EnemyGroupName = enemyGroup;
				smartAiV.HasSeenPlayer = true;
				GuardGroup[(int)enemyGroup].GroupList.Add(smartAiV);
				ped.Task.FightAgainst(Game.Player.Character);
				GiveRandomWeaponToPed(ped, enemyGroup);
				GetFiringPattern(ped);
				RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
				Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped, relationshipGroup);
				Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 0, 0);
				Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, ped, 1);
				AiBackup.Add(ped);
			}
			if (list[0] != null)
			{
				Function.Call(Hash.TASK_COMBAT_PED, list[0], Game.Player.Character, 0, 16);
			}
			return vehicle;
		}

		public static Vehicle SpawnMerryweatherHeli(Vector3 Pos, float H, string Model, bool SpawnDriverOnly, List<uint> EnemyHashes, EnemyGroup enemyGroup)
		{
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedVehicles++;
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedHelis++;
			List<Ped> list = new List<Ped>();
			Vehicle vehicle = GlobalHelper.CreateVehicle(RequestModel(Model), Pos, H);
			SmartAI_SpawnedVehicles.Add(vehicle);
			vehicle.LodDistance = 3000;
			MerryweatherVehicles.Add(vehicle);
			int num = vehicle.PassengerCapacity;
			if (SpawnDriverOnly)
			{
				num = 0;
			}
			for (int i = -1; i < num; i++)
			{
				uint prop = EnemyHashes[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, EnemyHashes.Count)];
				Ped ped = GlobalHelper.CreatePed(RequestModel(prop), Pos, H);
				ped.Health = GuardGroup[(int)enemyGroup].Settings.Health;
				ped.Armor = GuardGroup[(int)enemyGroup].Settings.Armor;
				ped.Accuracy = GetAccuracy(GuardGroup[(int)enemyGroup].Settings);
				list.Add(ped);
				ped.SetIntoVehicle(vehicle, (VehicleSeat)i);
				SmartAiV2 smartAiV = new SmartAiV2(ped, enemyGroup);
				smartAiV.LastVehicle = vehicle;
				smartAiV.EnemyGroupName = enemyGroup;
				smartAiV.HasSeenPlayer = true;
				GuardGroup[(int)enemyGroup].GroupList.Add(smartAiV);
				GiveRandomWeaponToPed(ped, enemyGroup);
				GetFiringPattern(ped);
				RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
				Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped, relationshipGroup);
				Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 0, 0);
				Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, ped, 1);
				AiBackup.Add(ped);
			}
			if (list[0] != null)
			{
				Vector3 position = Game.Player.Character.Position;
				Function.Call(Hash.TASK_HELI_MISSION, vehicle.Driver.Handle, vehicle.Handle, 0, Game.Player.Character, 0f, 0f, 0f, 6, 50.0, 10.0, -1, 120, 60, -1, 64);
			}
			return vehicle;
		}

		public static Vehicle SpawnMerryweatherBoat(Vector3 Pos, float H, string Model, bool SpawnDriverOnly, List<uint> EnemyHashes, EnemyGroup enemyGroup)
		{
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedVehicles++;
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedBoats++;
			List<Ped> list = new List<Ped>();
			Pos = GenerateSpawnPos(Pos, Nodetype.Water, false);
			Vehicle vehicle = GlobalHelper.CreateVehicle(RequestModel(Model), Pos, H);
			SmartAI_SpawnedVehicles.Add(vehicle);
			vehicle.LodDistance = 3000;
			MerryweatherVehicles.Add(vehicle);
			int num = vehicle.PassengerCapacity;
			if (SpawnDriverOnly)
			{
				num = 0;
			}
			for (int i = -1; i < num; i++)
			{
				Ped ped = GlobalHelper.CreatePed(RequestModel(PedHash.Blackops01SMY), Pos, H);
				ped.Health = GuardGroup[(int)enemyGroup].Settings.Health;
				ped.Armor = GuardGroup[(int)enemyGroup].Settings.Armor;
				ped.Accuracy = GetAccuracy(GuardGroup[(int)enemyGroup].Settings);
				list.Add(ped);
				ped.SetIntoVehicle(vehicle, (VehicleSeat)i);
				SmartAiV2 smartAiV = new SmartAiV2(ped, enemyGroup);
				smartAiV.LastVehicle = vehicle;
				smartAiV.EnemyGroupName = enemyGroup;
				smartAiV.HasSeenPlayer = true;
				GuardGroup[(int)enemyGroup].GroupList.Add(smartAiV);
				ped.Task.FightAgainst(Game.Player.Character);
				GiveRandomWeaponToPed(ped, enemyGroup);
				GetFiringPattern(ped);
				RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
				Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped, relationshipGroup);
				Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 0, 0);
				Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, ped, 1);
				AiBackup.Add(ped);
			}
			if (list[0] != null)
			{
				Function.Call(Hash.TASK_COMBAT_PED, list[0], Game.Player.Character, 0, 16);
				Vector3 position = Game.Player.Character.Position;
			}
			return vehicle;
		}

		public static Vehicle SpawnMerryweatherPlane(Vector3 Pos, float H, string Model, bool SpawnDriverOnly, List<uint> EnemyHashes, EnemyGroup enemyGroup)
		{
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedVehicles++;
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedPlanes++;
			List<Ped> list = new List<Ped>();
			Vehicle vehicle = GlobalHelper.CreateVehicle(RequestModel(Model), Pos, H);
			SmartAI_SpawnedVehicles.Add(vehicle);
			vehicle.LodDistance = 3000;
			vehicle.ForwardSpeed = 100f;
			MerryweatherVehicles.Add(vehicle);
			int num = vehicle.PassengerCapacity;
			if (SpawnDriverOnly)
			{
				num = 0;
			}
			for (int i = -1; i < num; i++)
			{
				uint prop = EnemyHashes[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, EnemyHashes.Count)];
				Ped ped = GlobalHelper.CreatePed(RequestModel(prop), Pos, H);
				ped.Health = GuardGroup[(int)enemyGroup].Settings.Health;
				ped.Armor = GuardGroup[(int)enemyGroup].Settings.Armor;
				ped.Accuracy = GetAccuracy(GuardGroup[(int)enemyGroup].Settings);
				list.Add(ped);
				ped.SetIntoVehicle(vehicle, (VehicleSeat)i);
				SmartAiV2 smartAiV = new SmartAiV2(ped, enemyGroup);
				smartAiV.EnemyGroupName = enemyGroup;
				smartAiV.LastVehicle = vehicle;
				smartAiV.HasSeenPlayer = true;
				GuardGroup[(int)enemyGroup].GroupList.Add(smartAiV);
				GiveRandomWeaponToPed(ped, enemyGroup);
				GetFiringPattern(ped);
				RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
				Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped, relationshipGroup);
				Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 0, 0);
				Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, ped, 1);
				AiBackup.Add(ped);
			}
			vehicle.LandingGearState = VehicleLandingGearState.Retracted;
			if (Model.Equals("RAIJU"))
			{
				Function.Call((Hash)865331629040904805uL, vehicle, true);
			}
			if (list[0] != null)
			{
				Vector3 position = Game.Player.Character.Position;
				Vector3 vector = position;
				Function.Call(Hash.TASK_PLANE_MISSION, list[0], list[0].CurrentVehicle.Handle, 0, 0, vector.X, vector.Y, vector.Z, 6, 100f, 0f, 90f, 0, 150f);
			}
			return vehicle;
		}

		public static Vehicle SpawnEberhardPyro(Vector3 Pos, float H, string Model, bool SpawnDriverOnly, List<uint> EnemyHashes, EnemyGroup enemyGroup)
		{
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedVehicles++;
			GuardGroup[(int)enemyGroup].Settings.SmartAI_SpawnedPlanes++;
			List<Ped> list = new List<Ped>();
			Vehicle vehicle = GlobalHelper.CreateVehicle(RequestModel(Model), Pos, H);
			SmartAI_SpawnedVehicles.Add(vehicle);
			vehicle.LodDistance = 3000;
			vehicle.ForwardSpeed = 100f;
			MerryweatherVehicles.Add(vehicle);
			int num = vehicle.PassengerCapacity;
			vehicle.LandingGearState = VehicleLandingGearState.Retracted;
			vehicle.ForwardSpeed = 100f;
			Function.Call(Hash.SET_VEHICLE_MOD_KIT, vehicle.Handle, 0);
			vehicle.Mods.PrimaryColor = VehicleColor.MetallicBlack;
			vehicle.Mods.SecondaryColor = VehicleColor.MetallicBlack;
			vehicle.Mods[VehicleModType.Roof].Index = 0;
			if (SpawnDriverOnly)
			{
				num = 0;
			}
			for (int i = -1; i < num; i++)
			{
				uint prop = EnemyHashes[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, EnemyHashes.Count)];
				Ped ped = GlobalHelper.CreatePed(RequestModel(prop), Pos, H);
				ped.Health = GuardGroup[(int)enemyGroup].Settings.Health;
				ped.Armor = GuardGroup[(int)enemyGroup].Settings.Armor;
				ped.Accuracy = GetAccuracy(GuardGroup[(int)enemyGroup].Settings);
				list.Add(ped);
				ped.SetIntoVehicle(vehicle, (VehicleSeat)i);
				SmartAiV2 smartAiV = new SmartAiV2(ped, enemyGroup);
				smartAiV.LastVehicle = vehicle;
				smartAiV.EnemyGroupName = enemyGroup;
				smartAiV.HasSeenPlayer = true;
				GuardGroup[(int)enemyGroup].GroupList.Add(smartAiV);
				GiveRandomWeaponToPed(ped, enemyGroup);
				GetFiringPattern(ped);
				RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
				Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped, relationshipGroup);
				Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 0, 0);
				Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, ped, 1);
				ped.CombatRange = CombatRange.Near;
				ped.CombatAbility = CombatAbility.Professional;
				ped.VehicleWeapon = VehicleWeaponHash.PlaneRocket;
				AiBackup.Add(ped);
			}
			if (Model.Equals("RAIJU"))
			{
				Function.Call((Hash)865331629040904805uL, vehicle, true);
			}
			if (list[0] != null)
			{
				Vector3 position = Game.Player.Character.Position;
				Vector3 vector = position;
				Function.Call(Hash.TASK_PLANE_MISSION, list[0], list[0].CurrentVehicle.Handle, 0, 0, vector.X, vector.Y, vector.Z, 6, 100f, 0f, 90f, 0, 150f);
			}
			return vehicle;
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

		public static Vector3 GetOffsetPosition(Vector3 Coord, float Heading, Vector3 Offset)
		{
			return Function.Call<Vector3>(Hash.GET_OFFSET_FROM_COORD_AND_HEADING_IN_WORLD_COORDS, Coord.X, Coord.Y, Coord.Z, Heading, Offset.X, Offset.Y, Offset.Z);
		}

		public static bool GetDistance_VDIST2(Vector3 a, Vector3 b, string sign, float radius)
		{
			float num = a.DistanceToSquared(b);
			float num2 = radius * radius;
			switch (sign)
			{
			case "<=":
				return num <= num2;
			case "<":
				return num < num2;
			case ">=":
				return num >= num2;
			case ">":
				return num > num2;
			default:
				return false;
			}
		}

		public static void ATTACH_ENTITY_TO_ENTITY(Entity entity1, Entity entity2, int boneIndex, float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, bool p9, bool useSoftPinning, bool collision, bool isPed, int vertexIndex, bool fixedRot)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, entity1, entity2, boneIndex, xPos, yPos, zPos, xRot, yRot, zRot, p9, useSoftPinning, collision, isPed, vertexIndex, fixedRot);
		}

		public static void TaskLeaveVehicle(Ped ped, Vehicle vehicle, int flags)
		{
			Function.Call(Hash.TASK_LEAVE_VEHICLE, ped, vehicle, flags);
		}

		public static void TaskLeaveVehicle(int ped, Vehicle vehicle, int flags)
		{
			Function.Call(Hash.TASK_LEAVE_VEHICLE, ped, vehicle, flags);
		}

		public static void TaskFollowNavMeshToCoord(Ped ped, float x, float y, float z, float moveBlendRatio, int time, float targetRadius, int flags, float targetHeading)
		{
			Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, ped, x, y, z, moveBlendRatio, time, targetRadius, flags, targetHeading);
		}

		public static void TaskFollowNavMeshToCoord(int ped, float x, float y, float z, float moveBlendRatio, int time, float targetRadius, int flags, float targetHeading)
		{
			Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, ped, x, y, z, moveBlendRatio, time, targetRadius, flags, targetHeading);
		}

		public static void TaskCombatHatedTargetsAroundPed(Ped Ped, float range, int combatFlags)
		{
			Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, Ped, range, combatFlags);
		}

		public static void TaskCombatHatedTargetsAroundPed(int Ped, float range, int combatFlags)
		{
			Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, Ped, range, combatFlags);
		}

		public static void TaskAchieveHeading(Ped ped, float heading, int timeout)
		{
			Function.Call(Hash.TASK_ACHIEVE_HEADING, ped, heading, timeout);
		}

		public static void TaskAchieveHeading(int ped, float heading, int timeout)
		{
			Function.Call(Hash.TASK_ACHIEVE_HEADING, ped, heading, timeout);
		}

		public static void TaskStandStill(Ped ped, int time)
		{
			Function.Call(Hash.TASK_STAND_STILL, ped, time);
		}

		public static void TaskStandStill(int ped, int time)
		{
			Function.Call(Hash.TASK_STAND_STILL, ped, time);
		}

		public static void TaskCower(Ped Ped, int time)
		{
			Function.Call(Hash.TASK_COWER, Ped, time);
		}

		public static void TaskCower(int Ped, int time)
		{
			Function.Call(Hash.TASK_COWER, Ped, time);
		}

		public static void TaskTurnPedToFaceEntity(Ped ped, Entity entity, int duration)
		{
			Function.Call(Hash.TASK_TURN_PED_TO_FACE_ENTITY, ped, entity, duration);
		}

		public static void TaskTurnPedToFaceEntity(int ped, Entity entity, int duration)
		{
			Function.Call(Hash.TASK_TURN_PED_TO_FACE_ENTITY, ped, entity, duration);
		}

		public static void TaskPlayAnim(Ped Ped, string Dict, string Anim, float unk1, float unk2, int Duration, int unk3, float unk4, bool unk5, bool unk6, bool unk7)
		{
			Function.Call(Hash.TASK_PLAY_ANIM, Ped, Dict, Anim, unk1, unk2, Duration, unk3, unk4, unk5, unk6, unk7);
		}

		public static void TaskPlayAnim(int Ped, string Dict, string Anim, float unk1, float unk2, int Duration, int unk3, float unk4, bool unk5, bool unk6, bool unk7)
		{
			Function.Call(Hash.TASK_PLAY_ANIM, Ped, Dict, Anim, unk1, unk2, Duration, unk3, unk4, unk5, unk6, unk7);
		}

		public static void TaskAimGunAtCoord(Ped Ped, float X, float Y, float Z, int Duration, bool unk1, bool unk2)
		{
			Function.Call(Hash.TASK_AIM_GUN_AT_COORD, Ped, X, Y, Z, Duration, unk1, unk2);
		}

		public static void TaskAimGunAtCoord(int Ped, float X, float Y, float Z, int Duration, bool unk1, bool unk2)
		{
			Function.Call(Hash.TASK_AIM_GUN_AT_COORD, Ped, X, Y, Z, Duration, unk1, unk2);
		}

		public static void TaskLookAtEntity(Ped ped, Entity lookAt, int duration, int flags, int priority)
		{
			Function.Call(Hash.TASK_LOOK_AT_ENTITY, ped, lookAt, duration, flags, priority);
		}

		public static void TaskLookAtEntity(int ped, Entity lookAt, int duration, int flags, int priority)
		{
			Function.Call(Hash.TASK_LOOK_AT_ENTITY, ped, lookAt, duration, flags, priority);
		}

		public static void TaskPause(int Ped, int Time)
		{
			Function.Call(Hash.TASK_PAUSE, Ped, Time);
		}

		public static void TaskPause(Ped Ped, int Time)
		{
			Function.Call(Hash.TASK_PAUSE, Ped, Time);
		}

		public static void TaskHandsUp(Ped ped, int duration, Ped facingPed, int timeToFacePed, int flags)
		{
			Function.Call(Hash.TASK_HANDS_UP, ped, duration, facingPed, timeToFacePed, flags);
		}

		public static void TaskHandsUp(int ped, int duration, Ped facingPed, int timeToFacePed, int flags)
		{
			Function.Call(Hash.TASK_HANDS_UP, ped, duration, facingPed, timeToFacePed, flags);
		}

		public static void TaskPutDirectlyIntoCover(Ped ped, float x, float y, float z, int time, bool allowPeekingAndFiring, float blendInDuration, bool forceInitialFacingDirection, bool forceFaceLeft, int identifier, bool doEntry)
		{
			Function.Call(Hash.TASK_PUT_PED_DIRECTLY_INTO_COVER, ped, x, y, z, time, allowPeekingAndFiring, blendInDuration, forceInitialFacingDirection, forceFaceLeft, identifier, doEntry);
		}

		public static void TaskPutDirectlyIntoCover(int ped, float x, float y, float z, int time, bool allowPeekingAndFiring, float blendInDuration, bool forceInitialFacingDirection, bool forceFaceLeft, int identifier, bool doEntry)
		{
			Function.Call(Hash.TASK_PUT_PED_DIRECTLY_INTO_COVER, ped, x, y, z, time, allowPeekingAndFiring, blendInDuration, forceInitialFacingDirection, forceFaceLeft, identifier, doEntry);
		}

		public static void TaskAimGunAtEntity(Ped ped, Entity entity, int duration, bool instantBlendToAim)
		{
			Function.Call(Hash.TASK_AIM_GUN_AT_ENTITY, ped, entity, duration, instantBlendToAim);
		}

		public static void TaskAimGunAtEntity(int ped, Entity entity, int duration, bool instantBlendToAim)
		{
			Function.Call(Hash.TASK_AIM_GUN_AT_ENTITY, ped, entity, duration, instantBlendToAim);
		}

		public static void TaskGoToCoordWhileAimingAtCoord(Ped ped, float x, float y, float z, float aimAtX, float aimAtY, float aimAtZ, float moveBlendRatio, bool shoot, float targetRadius, float slowDistance, bool useNavMesh, int navFlags, bool instantBlendToAim, int firingPattern)
		{
			Function.Call(Hash.TASK_GO_TO_COORD_WHILE_AIMING_AT_COORD, ped, x, y, z, aimAtX, aimAtY, aimAtZ, moveBlendRatio, shoot, targetRadius, slowDistance, useNavMesh, navFlags, instantBlendToAim, firingPattern);
		}

		public static void TaskGoToCoordWhileAimingAtCoord(int ped, float x, float y, float z, float aimAtX, float aimAtY, float aimAtZ, float moveBlendRatio, bool shoot, float targetRadius, float slowDistance, bool useNavMesh, int navFlags, bool instantBlendToAim, int firingPattern)
		{
			Function.Call(Hash.TASK_GO_TO_COORD_WHILE_AIMING_AT_COORD, ped, x, y, z, aimAtX, aimAtY, aimAtZ, moveBlendRatio, shoot, targetRadius, slowDistance, useNavMesh, navFlags, instantBlendToAim, firingPattern);
		}

		public static void TaskGoTOEntityWhileAimingAtEntity(Ped ped, Entity entityToWalkTo, Entity entityToAimAt, float speed, bool shootatEntity, float targetRadius, float slowDistance, bool useNavMesh, bool instantBlendToAim, int firingPattern)
		{
			Function.Call(Hash.TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY, ped, entityToWalkTo, entityToAimAt, speed, shootatEntity, targetRadius, slowDistance, useNavMesh, instantBlendToAim, firingPattern);
		}

		public static void TaskGoTOEntityWhileAimingAtEntity(int ped, Entity entityToWalkTo, Entity entityToAimAt, float speed, bool shootatEntity, float targetRadius, float slowDistance, bool useNavMesh, bool instantBlendToAim, int firingPattern)
		{
			Function.Call(Hash.TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY, ped, entityToWalkTo, entityToAimAt, speed, shootatEntity, targetRadius, slowDistance, useNavMesh, instantBlendToAim, firingPattern);
		}

		public static void TaskWander(Ped ped, float heading, int flags)
		{
			Function.Call(Hash.TASK_WANDER_STANDARD, ped, heading, flags);
		}

		public static void TaskWander(int ped, float heading, int flags)
		{
			Function.Call(Hash.TASK_WANDER_STANDARD, ped, heading, flags);
		}

		public static void TaskPedSlideToCoord(Ped ped, Vector3 pos, float heading, float speed = 0.75f)
		{
			Function.Call(Hash.TASK_PED_SLIDE_TO_COORD, ped, pos.X, pos.Y, pos.Z, heading, speed);
		}

		public static void TaskPedSlideToCoord(int ped, Vector3 pos, float heading, float speed = 0.75f)
		{
			Function.Call(Hash.TASK_PED_SLIDE_TO_COORD, ped, pos.X, pos.Y, pos.Z, heading, speed);
		}

		public unsafe static void OpenSequenceTask(int* taskID)
		{
			Function.Call(Hash.OPEN_SEQUENCE_TASK, taskID);
		}

		public static void CloseSequenceTask(int taskID)
		{
			Function.Call(Hash.CLOSE_SEQUENCE_TASK, taskID);
		}

		public static void SetSequenceRepeat(int taskID, bool repeat)
		{
			Function.Call(Hash.SET_SEQUENCE_TO_REPEAT, taskID, repeat);
		}

		public unsafe static void ClearSequenceTask(int* taskID)
		{
			Function.Call(Hash.CLEAR_SEQUENCE_TASK, taskID);
		}

		public static void TaskPerformSequence(Ped ped, int taskID)
		{
			Function.Call(Hash.TASK_PERFORM_SEQUENCE, ped, taskID);
		}

		public static FiringPattern GetFiringPattern(Ped Ped)
		{
			WeaponHash weaponHash = WeaponHash.Pistol;
			if (Ped.Weapons.Current != null)
			{
				weaponHash = Ped.Weapons.Current.Hash;
			}
			switch (weaponHash)
			{
			case WeaponHash.SNSPistolMk2:
			case WeaponHash.NavyRevolver:
			case WeaponHash.DoubleActionRevolver:
			case WeaponHash.SNSPistol:
			case WeaponHash.PistolMk2:
			case WeaponHash.Revolver:
			case WeaponHash.RevolverMk2:
			case WeaponHash.HeavyPistol:
			case WeaponHash.MarksmanPistol:
			case WeaponHash.VintagePistol:
			case WeaponHash.Pistol:
			case WeaponHash.APPistol:
			case WeaponHash.CeramicPistol:
			case WeaponHash.CombatPistol:
				return FiringPattern.SingleShot;
			case WeaponHash.MiniSMG:
			case WeaponHash.MachinePistol:
			case WeaponHash.AssaultSMG:
			case WeaponHash.CombatPDW:
			case WeaponHash.MicroSMG:
			case WeaponHash.SMG:
			case WeaponHash.SMGMk2:
				return FiringPattern.BurstFireSMG;
			case WeaponHash.CarbineRifle:
			case WeaponHash.BullpupRifleMk2:
			case WeaponHash.SpecialCarbineMk2:
			case WeaponHash.MilitaryRifle:
			case WeaponHash.AdvancedRifle:
			case WeaponHash.AssaultRifle:
			case WeaponHash.SpecialCarbine:
			case WeaponHash.HeavyRifle:
			case WeaponHash.ServiceCarbine:
			case WeaponHash.CarbineRifleMk2:
			case WeaponHash.AssaultrifleMk2:
			case WeaponHash.CompactRifle:
			case WeaponHash.BullpupRifle:
				return FiringPattern.BurstFireRifle;
			case WeaponHash.MG:
			case WeaponHash.CombatMGMk2:
			case WeaponHash.CombatMG:
				return FiringPattern.BurstFireMG;
			case WeaponHash.BullpupShotgun:
			case WeaponHash.AssaultShotgun:
			case WeaponHash.DoubleBarrelShotgun:
			case WeaponHash.CombatShotgun:
			case WeaponHash.PumpShotgun:
			case WeaponHash.HeavyShotgun:
			case WeaponHash.PumpShotgunMk2:
			case WeaponHash.SawnOffShotgun:
				return FiringPattern.SingleShot;
			case WeaponHash.MarksmanRifle:
			case WeaponHash.SniperRifle:
			case WeaponHash.HeavySniperMk2:
			case WeaponHash.HeavySniper:
			case WeaponHash.MarksmanRifleMk2:
			case WeaponHash.PrecisionRifle:
				return FiringPattern.SingleShot;
			case WeaponHash.GrenadeLauncher:
			case WeaponHash.RPG:
			case WeaponHash.CompactEMPLauncher:
			case WeaponHash.RailgunXmas3:
			case WeaponHash.CompactGrenadeLauncher:
			case WeaponHash.GrenadeLauncherSmoke:
			case WeaponHash.HomingLauncher:
			case WeaponHash.Railgun:
				return FiringPattern.SingleShot;
			case WeaponHash.Crowbar:
			case WeaponHash.Flashlight:
			case WeaponHash.Dagger:
			case WeaponHash.PoolCue:
			case WeaponHash.Bat:
			case WeaponHash.Knife:
			case WeaponHash.BattleAxe:
			case WeaponHash.Machete:
			case WeaponHash.SwitchBlade:
			case WeaponHash.Hatchet:
			case WeaponHash.Bottle:
			case WeaponHash.Wrench:
			case WeaponHash.StoneHatchet:
			case WeaponHash.GolfClub:
			case WeaponHash.Hammer:
			case WeaponHash.Nightstick:
				return FiringPattern.SingleShot;
			case WeaponHash.Grenade:
			case WeaponHash.BZGas:
			case WeaponHash.ProximityMine:
			case WeaponHash.PipeBomb:
			case WeaponHash.SmokeGrenade:
			case WeaponHash.Snowball:
			case WeaponHash.Molotov:
			case WeaponHash.StickyBomb:
			case WeaponHash.Flare:
				return FiringPattern.SingleShot;
			case WeaponHash.StunGun:
			case WeaponHash.StunGunMultiplayer:
				return FiringPattern.SingleShot;
			case WeaponHash.FireExtinguisher:
			case WeaponHash.FertilizerCan:
			case WeaponHash.PetrolCan:
				return FiringPattern.Default;
			default:
				return FiringPattern.Default;
			}
		}

		public static void SetupCayoJuggernautSuit(Ped Ped)
		{
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Ped, 0, 0, 0, 1);
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Ped, 1, 57, 0, 1);
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Ped, 2, 1, 0, 1);
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Ped, 3, 40, 0, 1);
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Ped, 4, 125, 0, 1);
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Ped, 5, 0, 0, 1);
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Ped, 6, 112, 0, 1);
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Ped, 7, 0, 0, 1);
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Ped, 8, 21, 0, 1);
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Ped, 9, 22, 9, 1);
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Ped, 10, 0, 0, 1);
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Ped, 11, 422, 0, 1);
			Function.Call(Hash.SET_PED_PROP_INDEX, Ped, 0, 125, 0, 0);
			Function.Call(Hash.SET_PED_USING_ACTION_MODE, Ped, true, -1, 0);
			Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Ped, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
			Function.Call(Hash.SET_PED_STRAFE_CLIPSET, Ped, "MOVE_STRAFE_BALLISTIC");
			Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, Ped, 1429513766);
			Ped.Weapons.Give(WeaponHash.Minigun, 9999, true, true);
			Ped.FiringPattern = FiringPattern.BurstFireMG;
			Ped.CanSufferCriticalHits = false;
			Ped.Health = 1000;
			Ped.Armor = 500;
			Function.Call(Hash.SET_PED_CONFIG_FLAG, Ped, 107, true);
			Function.Call(Hash.SET_PED_CONFIG_FLAG, Ped, 48, true);
		}

		public static void DrawLine(Vector3 From, Vector3 To, Vector3 RGB)
		{
			Function.Call(Hash.DRAW_LINE, From.X, From.Y, From.Z, To.X, To.Y, To.Z, RGB.X, RGB.Y, RGB.Z, 255);
		}

		public static float LerpAngle(float a, float b, float t)
		{
			float num = Math.Abs(b - a);
			if (num > 180f)
			{
				if (b > a)
				{
					a += 360f;
				}
				else
				{
					b += 360f;
				}
			}
			float num2 = a + (b - a) * t;
			return num2 % 360f;
		}

		public static float ASin(float value)
		{
			return (float)Math.Asin(value);
		}

		public static float ATan2(float value, float value2)
		{
			return (float)Math.Atan2(value, value2);
		}

		public static float ToDeg(float value)
		{
			return value * 57.29578f;
		}

		public static void RotateVeh(Vehicle Veh, Vector3 forward)
		{
			forward.Normalize();
			Vector3 normalized = Vector3.Cross(forward, Vector3.RelativeTop).Normalized;
			Vector3 vector = Vector3.Cross(normalized, forward);
			Vector3 vector2 = default(Vector3);
			vector2.X = ToDeg(ASin(forward.Z));
			vector2.Y = ToDeg(ATan2(0f - normalized.Z, vector.Z));
			vector2.Z = ToDeg(ATan2(0f - forward.X, forward.Y));
			Vector3 vector3 = vector2;
			float t = 1f;
			float z = LerpAngle(Veh.Rotation.Z, vector3.Z, t);
			float x = LerpAngle(Veh.Rotation.X, vector3.X, t);
			float y = LerpAngle(Veh.Rotation.Y, vector3.Y, t);
			Veh.Rotation = new Vector3(x, y, z);
		}

		public static void SetupRandomAnimation(SmartAiV2 AI)
		{
			if (AI.AIPed != null)
			{
				if (AI.AiProp != null)
				{
					AI.AiProp.Delete();
				}
				int num = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 200);
				if (num <= 10)
				{
					AI.AiProp = GlobalHelper.CreateProp(RequestModel("p_amb_clipboard_01"), Game.Player.Character.Position, false, false);
					AI.AiProp.AttachTo(AI.AIPed.Bones[Bone.SkelLeftHand], new Vector3(0.190000013f, 0.09999998f, 0.08f), new Vector3(-159.999985f, 60f, 50f));
					AI.AIPed.Task.PlayAnimation("amb@world_human_clipboard@male@idle_a", "idle_a", 1f, -1, AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary | AnimationFlags.ForceStart);
				}
				if (num > 10 && num <= 20)
				{
					AI.AiProp = GlobalHelper.CreateProp(RequestModel("p_amb_clipboard_01"), Game.Player.Character.Position, false, false);
					AI.AiProp.AttachTo(AI.AIPed.Bones[Bone.SkelLeftHand], new Vector3(0.190000013f, 0.09999998f, 0.08f), new Vector3(-159.999985f, 60f, 50f));
					AI.AIPed.Task.PlayAnimation("amb@world_human_clipboard@male@idle_b", "idle_d", 1f, -1, AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary | AnimationFlags.ForceStart);
				}
				if (num > 20 && num <= 30)
				{
					AI.AiProp = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_npc_phone"), Game.Player.Character.Position, false, false);
					AI.AiProp.AttachTo(AI.AIPed.Bones[Bone.SkelRightHand], new Vector3(0.149999991f, 0.03f, -0.0200004578f), new Vector3(109.999985f, 80f, 69.99999f));
					AI.AIPed.Task.PlayAnimation("amb@world_human_stand_mobile@male@standing@call@idle_a", "idle_a", 1f, -1, AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary | AnimationFlags.ForceStart);
				}
				if (num > 30 && num <= 40)
				{
					AI.AiProp = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_npc_phone"), Game.Player.Character.Position, false, false);
					AI.AiProp.AttachTo(AI.AIPed.Bones[Bone.SkelRightHand], new Vector3(0.149999991f, 0.03f, -0.0200004578f), new Vector3(109.999985f, 80f, 69.99999f));
					AI.AIPed.Task.PlayAnimation("amb@world_human_stand_mobile@male@standing@call@idle_a", "idle_b", 1f, -1, AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary | AnimationFlags.ForceStart);
				}
				if (num > 40 && num <= 50)
				{
					AI.AiProp = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_npc_phone"), Game.Player.Character.Position, false, false);
					AI.AiProp.AttachTo(AI.AIPed.Bones[Bone.SkelRightHand], new Vector3(0.149999991f, 0.03f, -0.0200004578f), new Vector3(109.999985f, 80f, 69.99999f));
					AI.AIPed.Task.PlayAnimation("amb@world_human_stand_mobile@male@standing@call@idle_a", "idle_c", 1f, -1, AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary | AnimationFlags.ForceStart);
				}
				if (num > 50 && num <= 60)
				{
					AI.AiProp = GlobalHelper.CreateProp(RequestModel("ng_proc_coffee_01a"), Game.Player.Character.Position, false, false);
					AI.AiProp.AttachTo(AI.AIPed.Bones[Bone.SkelRightHand], new Vector3(0.13f, -0.07f, -0.04f), new Vector3(-89.9999847f, -5.722046E-06f, -9.999999f));
					AI.AIPed.Task.PlayAnimation("amb@world_human_drinking@coffee@male@idle_a", "idle_a", 1f, -1, AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary | AnimationFlags.ForceStart);
				}
				if (num > 60 && num <= 70)
				{
					AI.AiProp = GlobalHelper.CreateProp(RequestModel("ng_proc_coffee_01a"), Game.Player.Character.Position, false, false);
					AI.AiProp.AttachTo(AI.AIPed.Bones[Bone.SkelRightHand], new Vector3(0.13f, -0.07f, -0.04f), new Vector3(-89.9999847f, -5.722046E-06f, -9.999999f));
					AI.AIPed.Task.PlayAnimation("amb@world_human_drinking@coffee@male@idle_a", "idle_b", 1f, -1, AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary | AnimationFlags.ForceStart);
				}
				if (num > 70 && num <= 80)
				{
					AI.AiProp = GlobalHelper.CreateProp(RequestModel("ng_proc_coffee_01a"), Game.Player.Character.Position, false, false);
					AI.AiProp.AttachTo(AI.AIPed.Bones[Bone.SkelRightHand], new Vector3(0.13f, -0.07f, -0.04f), new Vector3(-89.9999847f, -5.722046E-06f, -9.999999f));
					AI.AIPed.Task.PlayAnimation("amb@world_human_drinking@coffee@male@idle_a", "idle_c", 1f, -1, AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary | AnimationFlags.ForceStart);
				}
				if (num > 80 && num <= 90)
				{
					AI.AiProp = GlobalHelper.CreateProp(RequestModel("ng_proc_cigar01a"), Game.Player.Character.Position, false, false);
					AI.AiProp.AttachTo(AI.AIPed.Bones[Bone.SkelRightHand], new Vector3(0.169999972f, -0.01f, 0f), new Vector3(0f, -89.99999f, -89.99999f));
					AI.AIPed.Task.PlayAnimation("amb@world_human_aa_smoke@male@idle_a", "idle_a", 1f, -1, AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary | AnimationFlags.ForceStart);
				}
				if (num > 90 && num <= 100)
				{
					AI.AiProp = GlobalHelper.CreateProp(RequestModel("ng_proc_cigar01a"), Game.Player.Character.Position, false, false);
					AI.AiProp.AttachTo(AI.AIPed.Bones[Bone.SkelRightHand], new Vector3(0.169999972f, -0.01f, 0f), new Vector3(0f, -89.99999f, -89.99999f));
					AI.AIPed.Task.PlayAnimation("amb@world_human_aa_smoke@male@idle_a", "idle_b", 1f, -1, AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary | AnimationFlags.ForceStart);
				}
				if (num > 100 && num <= 110)
				{
					AI.AiProp = GlobalHelper.CreateProp(RequestModel("ng_proc_cigar01a"), Game.Player.Character.Position, false, false);
					AI.AiProp.AttachTo(AI.AIPed.Bones[Bone.SkelRightHand], new Vector3(0.169999972f, -0.01f, 0f), new Vector3(0f, -89.99999f, -89.99999f));
					AI.AIPed.Task.PlayAnimation("amb@world_human_aa_smoke@male@idle_a", "idle_c", 1f, -1, AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary | AnimationFlags.ForceStart);
				}
				if (num > 110 && num <= 120)
				{
					AI.AiProp = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_npc_phone"), Game.Player.Character.Position, false, false);
					AI.AiProp.AttachTo(AI.AIPed.Bones[Bone.SkelLeftHand], new Vector3(0.149000049f, 0.0160000082f, 0.032f), new Vector3(-49.9999924f, -49.9999962f, -69.99999f));
					AI.AIPed.Task.PlayAnimation("amb@world_human_stand_mobile_fat@male@text@idle_a", "idle_a", 1f, -1, AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary | AnimationFlags.ForceStart);
				}
				if (num > 120 && num <= 130)
				{
					AI.AiProp = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_npc_phone"), Game.Player.Character.Position, false, false);
					AI.AiProp.AttachTo(AI.AIPed.Bones[Bone.SkelLeftHand], new Vector3(0.149000049f, 0.0160000082f, 0.032f), new Vector3(-49.9999924f, -49.9999962f, -69.99999f));
					AI.AIPed.Task.PlayAnimation("amb@world_human_stand_mobile_fat@male@text@idle_a", "idle_b", 1f, -1, AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary | AnimationFlags.ForceStart);
				}
				if (num > 130 && num <= 140)
				{
					AI.AiProp = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_npc_phone"), Game.Player.Character.Position, false, false);
					AI.AiProp.AttachTo(AI.AIPed.Bones[Bone.SkelLeftHand], new Vector3(0.149000049f, 0.0160000082f, 0.032f), new Vector3(-49.9999924f, -49.9999962f, -69.99999f));
					AI.AIPed.Task.PlayAnimation("amb@world_human_stand_mobile_fat@male@text@idle_a", "idle_c", 1f, -1, AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary | AnimationFlags.ForceStart);
				}
				if (num > 140 && num <= 150)
				{
					AI.AiProp = GlobalHelper.CreateProp(RequestModel("p_amb_clipboard_01"), Game.Player.Character.Position, false, false);
					AI.AiProp.AttachTo(AI.AIPed.Bones[Bone.SkelLeftHand], new Vector3(0.210000083f, 0.08000001f, 0.0300000049f), new Vector3(-140.000015f, 30f, 30.0000038f));
					AI.AIPed.Task.PlayAnimation("anim@amb@business@bgen@bgen_inspecting@", "inspecting_look_left_01_inspector", 1f, -1, AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary | AnimationFlags.ForceStart);
				}
				if (num > 150 && num <= 160)
				{
					AI.AiProp = GlobalHelper.CreateProp(RequestModel("p_amb_clipboard_01"), Game.Player.Character.Position, false, false);
					AI.AiProp.AttachTo(AI.AIPed.Bones[Bone.SkelLeftHand], new Vector3(0.210000083f, 0.08000001f, 0.0300000049f), new Vector3(-140.000015f, 30f, 30.0000038f));
					AI.AIPed.Task.PlayAnimation("anim@amb@business@bgen@bgen_inspecting@", "inspecting_look_right_01_inspector", 1f, -1, AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary | AnimationFlags.ForceStart);
				}
				if (num > 160 && num <= 170)
				{
					AI.AIPed.Task.PlayAnimation("oddjobs@assassinate@guard", "unarmed_fold_arms", 1f, -1, AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary | AnimationFlags.ForceStart);
				}
				if (num > 170 && num <= 180)
				{
					AI.AIPed.Task.PlayAnimation("oddjobs@assassinate@guard", "unarmed_earpiece_a", 1f, -1, AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary | AnimationFlags.ForceStart);
				}
				if (num > 180 && num <= 190)
				{
					AI.AIPed.Task.PlayAnimation("oddjobs@assassinate@guard", "unarmed_earpiece_a", 1f, -1, AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary | AnimationFlags.ForceStart);
				}
				if (num > 190 && num <= 200)
				{
					AI.AIPed.Task.PlayAnimation("oddjobs@assassinate@guard", "unarmed_earpiece_b", 1f, -1, AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary | AnimationFlags.ForceStart);
				}
			}
		}

		public unsafe static void RunSmartAIFuncs(List<SmartAiV2> Merryweather, SmartAI_Settings Settings)
		{
			if (Game.Player.Character.LastWeaponImpactPosition != Vector3.Zero)
			{
				LastHitCoords = Game.Player.Character.LastWeaponImpactPosition;
			}
			Vector3 lastWeaponImpactPosition2 = Game.Player.Character.LastWeaponImpactPosition;
			if (true)
			{
				Vector3 lastWeaponImpactPosition = Game.Player.Character.LastWeaponImpactPosition;
				if (GetDistance_VDIST2(new Vector3(0f, 0f, 0f), lastWeaponImpactPosition, ">", 5f))
				{
					Settings.Player_LastWeaponImpactPosition = lastWeaponImpactPosition;
				}
			}
			if (Settings.MerryweatherSpawnCheck < Game.GameTime)
			{
				Settings.MerryweatherSpawnCheck = Game.GameTime + 5000;
				for (int i = 0; i < Merryweather.Count; i++)
				{
					SmartAiV2 smartAiV = Merryweather[i];
					if (smartAiV == null)
					{
						continue;
					}
					SmartAiV2 smartAiV2 = smartAiV;
					if (!smartAiV2.GroupActive)
					{
						if (!(GetDistance(Settings.AI_Target.Position, smartAiV2.SpawnGroupArea) < smartAiV2.DistToSpawn))
						{
							continue;
						}
						SpawnGroupsUseActive.Add(smartAiV2);
						smartAiV2.GroupActive = true;
						if (smartAiV2.SpawnType == 0)
						{
							smartAiV2.AIPed = GlobalHelper.CreatePed(SmartGuards.RequestModel(smartAiV2.PedHash), smartAiV2.SpawnLocation, smartAiV2.SpawnHeading);
							smartAiV2.AIPed.IsEnemy = true;
							smartAiV2.AIPed.Health = Settings.Health;
							smartAiV2.AIPed.Armor = Settings.Armor;
							smartAiV2.AIPed.Accuracy = GetAccuracy(Settings);
							if (smartAiV2.IsCayoPericoJugg)
							{
								SetupCayoJuggernautSuit(smartAiV2.AIPed);
							}
							GiveRandomWeaponToPed(smartAiV2.AIPed, smartAiV2.EnemyGroupName);
							GetFiringPattern(smartAiV2.AIPed);
							smartAiV2.AIPed.BlockPermanentEvents = true;
							smartAiV2.AIPed.AlwaysKeepTask = true;
							if (smartAiV2.AIPed.AttachedBlip == null)
							{
								Blip blip = smartAiV2.AIPed.AddBlip();
								blip.Scale = 0.75f;
								blip.Sprite = BlipSprite.Enemy;
								blip.Name = GetStringNameFromEnemyGroup(smartAiV2.EnemyGroupName);
								blip.Color = Settings.AIBlipColor;
								blip.IsShortRange = true;
								blip.Alpha = 0;
							}
							Random random = new Random();
							int num = random.Next(0, 100);
							RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
							Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, smartAiV2.AIPed, relationshipGroup);
							Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, smartAiV2.AIPed, 0, 0);
							Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, smartAiV2.AIPed, 1);
							smartAiV.SpawnSafeTimer = Game.GameTime + 2000;
							smartAiV.AIPed = smartAiV2.AIPed;
							if (!Settings.IgnoreRandomAnimation)
							{
								SetupRandomAnimation(smartAiV);
							}
						}
						if (smartAiV2.SpawnType == 1)
						{
							if (!string.IsNullOrWhiteSpace(smartAiV2.StringVehicleHash))
							{
								smartAiV2.LastVehicle = GlobalHelper.CreateVehicle(RequestModel(smartAiV2.StringVehicleHash), smartAiV2.SpawnLocation, smartAiV2.SpawnHeading);
								smartAiV2.LastVehicle.IsInvincible = true;
							}
							else
							{
								smartAiV2.LastVehicle = GlobalHelper.CreateVehicle(RequestModel(smartAiV2.VHVehicleHash), smartAiV2.SpawnLocation, smartAiV2.SpawnHeading);
								smartAiV2.LastVehicle.IsInvincible = true;
							}
							if (smartAiV2.LastVehicle.Model.IsBoat)
							{
								smartAiV2.LastVehicle.IsPositionFrozen = true;
							}
							if (smartAiV2.LastVehicle.Model.IsHelicopter)
							{
								smartAiV2.LastVehicle.IsInvincible = true;
							}
							else
							{
								smartAiV2.LastVehicle.IsInvincible = false;
							}
						}
						if (smartAiV2.SpawnType == 2)
						{
							smartAiV2.LastVehicle = GlobalHelper.CreateVehicle(RequestModel(smartAiV2.VHVehicleHash), smartAiV2.SpawnLocation, smartAiV2.SpawnHeading);
							smartAiV2.LastVehicle.IsInvincible = true;
							if (smartAiV2.LastVehicle.Model.IsBoat)
							{
								smartAiV2.LastVehicle.IsPositionFrozen = true;
							}
							if (smartAiV2.LastVehicle.Model.IsHelicopter)
							{
								smartAiV2.LastVehicle.IsInvincible = true;
							}
							else
							{
								smartAiV2.LastVehicle.IsInvincible = false;
							}
						}
						continue;
					}
					if (smartAiV2.GroupActive && Settings.AllowDespawnIfTooFarAwayThisFrame && GetDistance_VDIST2(Game.Player.Character.Position, smartAiV.AIPed.Position, ">", Settings.TooFarAwayDist))
					{
						if (smartAiV.LastVehicle.AttachedBlip != null && smartAiV.LastVehicle.AttachedBlip.Alpha == 255)
						{
							if (SmartAI_SpawnedVehicles.Contains(smartAiV.LastVehicle))
							{
								SmartAI_SpawnedVehicles.Remove(smartAiV.LastVehicle);
							}
							smartAiV.LastVehicle.AttachedBlip.Alpha = 0;
							if (smartAiV.LastVehicle.Model.IsHelicopter)
							{
								Settings.SmartAI_SpawnedVehicles--;
								Settings.SmartAI_SpawnedHelis--;
							}
							else if (smartAiV.LastVehicle.Model.IsPlane)
							{
								Settings.SmartAI_SpawnedVehicles--;
								Settings.SmartAI_SpawnedPlanes--;
							}
							else if (smartAiV.LastVehicle.Model.IsBoat)
							{
								Settings.SmartAI_SpawnedVehicles--;
								Settings.SmartAI_SpawnedBoats--;
							}
							else
							{
								Settings.SmartAI_SpawnedVehicles--;
								Settings.SmartAI_SpawnedCars--;
							}
						}
						if (smartAiV.LastVehicle != null)
						{
							smartAiV.LastVehicle.Delete();
						}
						if (smartAiV.AIPed != null)
						{
							smartAiV.AIPed.Delete();
							if (AiBackup.Contains(smartAiV2.AIPed))
							{
								AiBackup.Remove(smartAiV2.AIPed);
							}
						}
						if (smartAiV.AiProp != null)
						{
							smartAiV.AiProp.Delete();
						}
					}
					if (!smartAiV2.GroupActive || smartAiV2.isDead)
					{
						continue;
					}
					if (smartAiV2.AIPed != null)
					{
						smartAiV2.AIPed.Accuracy = GetAccuracy(Settings);
						smartAiV2.SpawnGroupArea = smartAiV2.AIPed.Position;
					}
					if (!GetDistance_VDIST2(Settings.AI_Target.Position, smartAiV2.SpawnGroupArea, ">", smartAiV2.DistToSpawn))
					{
						continue;
					}
					smartAiV2.GroupActive = false;
					SpawnGroupsUseActive.Remove(smartAiV2);
					if (smartAiV2.AIPed != null)
					{
						smartAiV2.AIPed.Delete();
						if (AiBackup.Contains(smartAiV2.AIPed))
						{
							AiBackup.Remove(smartAiV2.AIPed);
						}
					}
					if (smartAiV2.AiProp != null)
					{
						smartAiV2.AiProp.Delete();
					}
					if (!(smartAiV2.LastVehicle != null) || !(Game.Player.Character.CurrentVehicle != smartAiV2.LastVehicle))
					{
						continue;
					}
					if (SmartAI_SpawnedVehicles.Contains(smartAiV.LastVehicle))
					{
						SmartAI_SpawnedVehicles.Remove(smartAiV.LastVehicle);
					}
					if (smartAiV.LastVehicle.AttachedBlip != null && smartAiV.LastVehicle.AttachedBlip.Alpha == 255)
					{
						smartAiV.LastVehicle.AttachedBlip.Alpha = 0;
						if (smartAiV.LastVehicle.Model.IsHelicopter)
						{
							Settings.SmartAI_SpawnedVehicles--;
							Settings.SmartAI_SpawnedHelis--;
						}
						else if (smartAiV.LastVehicle.Model.IsPlane)
						{
							Settings.SmartAI_SpawnedVehicles--;
							Settings.SmartAI_SpawnedPlanes--;
						}
						else if (smartAiV.LastVehicle.Model.IsBoat)
						{
							Settings.SmartAI_SpawnedVehicles--;
							Settings.SmartAI_SpawnedBoats--;
						}
						else
						{
							Settings.SmartAI_SpawnedVehicles--;
							Settings.SmartAI_SpawnedCars--;
						}
					}
					smartAiV2.LastVehicle.Delete();
				}
			}
			if (Settings.MerryweatherCheck < Game.GameTime)
			{
				Settings.MerryweatherCheck = Game.GameTime + 1000;
				for (int j = 0; j < Merryweather.Count; j++)
				{
					SmartAiV2 smartAiV3 = Merryweather[j];
					if (smartAiV3 == null || !(smartAiV3.AIPed != null))
					{
						continue;
					}
					if ((Function.Call<bool>(Hash.HAS_ENTITY_BEEN_DAMAGED_BY_WEAPON, smartAiV3.AIPed, 911657153, 0) || Function.Call<bool>(Hash.HAS_ENTITY_BEEN_DAMAGED_BY_WEAPON, smartAiV3.AIPed, 1171102963, 0) || Function.Call<bool>(Hash.HAS_ENTITY_BEEN_DAMAGED_BY_WEAPON, smartAiV3.AIPed, 0, 1)) && smartAiV3.AIPed.IsAlive)
					{
						smartAiV3.AIPed.Kill();
					}
					smartAiV3.AIPed.IsInvincible = false;
					if (smartAiV3.WanderLoop != null && !smartAiV3.HasSeenPlayer)
					{
						if (!smartAiV3.SetupWanderLoop)
						{
							int taskID = 0;
							OpenSequenceTask(&taskID);
							for (int k = 0; k < smartAiV3.WanderLoop.Loop.Count; k++)
							{
								if (k + 1 > smartAiV3.WanderLoop.Loop.Count - 1)
								{
									float targetHeading = (smartAiV3.WanderLoop.Loop[k] - smartAiV3.WanderLoop.Loop[0]).ToHeading();
									TaskFollowNavMeshToCoord(0, smartAiV3.WanderLoop.Loop[k].X, smartAiV3.WanderLoop.Loop[k].Y, smartAiV3.WanderLoop.Loop[k].Z, 1f, 120000, 1f, 512, targetHeading);
									TaskStandStill(0, Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 500, 2000));
								}
								else
								{
									float targetHeading2 = (smartAiV3.WanderLoop.Loop[k] - smartAiV3.WanderLoop.Loop[k + 1]).ToHeading();
									TaskFollowNavMeshToCoord(0, smartAiV3.WanderLoop.Loop[k].X, smartAiV3.WanderLoop.Loop[k].Y, smartAiV3.WanderLoop.Loop[k].Z, 1f, 120000, 1f, 512, targetHeading2);
									TaskStandStill(0, Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 500, 2000));
								}
							}
							SetSequenceRepeat(taskID, true);
							CloseSequenceTask(taskID);
							TaskPerformSequence(smartAiV3.AIPed, taskID);
							ClearSequenceTask(&taskID);
							if (!Settings.IgnoreRandomAnimation)
							{
								SetupRandomAnimation(smartAiV3);
							}
							smartAiV3.SetupWanderLoop = true;
						}
						if (smartAiV3.UseOldWanderLoop && smartAiV3.WanderLoop.Loop.Count > 0)
						{
							int num2 = smartAiV3.WanderLoop.AtPoint;
							int num3 = smartAiV3.WanderLoop.AtPoint + 1;
							int num4 = smartAiV3.WanderLoop.AtPoint + 2;
							if (num2 + 1 > smartAiV3.WanderLoop.Loop.Count - 1)
							{
								num2 = smartAiV3.WanderLoop.Loop.Count - 1;
								num3 = 0;
							}
							if (num2 + 2 > smartAiV3.WanderLoop.Loop.Count - 1)
							{
								num4 = 0;
							}
							if (smartAiV3.WanderLoop.Timerdelay == 0)
							{
								Random random2 = new Random();
								smartAiV3.WanderLoop.Timerdelay = Game.GameTime + random2.Next(1000, 15000);
							}
							if (smartAiV3.WanderLoop.Timerdelay != 0 && Game.GameTime >= smartAiV3.WanderLoop.Timerdelay)
							{
								int num5 = smartAiV3.WanderLoop.AtPoint;
								if (num5 > smartAiV3.WanderLoop.Loop.Count - 1)
								{
									num5 = 0;
								}
								if (num5 == 0)
								{
									Vector3 vector = smartAiV3.WanderLoop.Loop[num5];
									if (Settings.UseFollowNavMeshRoute)
									{
										Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, smartAiV3.AIPed, vector.X, vector.Y, vector.Z, 1f, 20000, 1f, 512, smartAiV3.AIPed.Rotation.Z);
									}
									else
									{
										Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, smartAiV3.AIPed, vector.X, vector.Y, vector.Z, 1f, -1, smartAiV3.AIPed.Heading + 100f, -0.1f);
									}
								}
								if (GetDistance_VDIST2(smartAiV3.AIPed.Position, smartAiV3.WanderLoop.Loop[num5], "<", 2f))
								{
									num5++;
									if (num5 > smartAiV3.WanderLoop.Loop.Count - 1)
									{
										num5 = 0;
									}
									Random random3 = new Random();
									int num6 = random3.Next(0, 100);
									if (num6 < 50)
									{
										smartAiV3.WanderLoop.AllowDelay = false;
									}
									smartAiV3.AIPed.Task.ClearAll();
									Random random4 = new Random();
									smartAiV3.WanderLoop.Timerdelay = 0;
									smartAiV3.WanderLoop.AtPoint = num5;
									Vector3 vector2 = smartAiV3.WanderLoop.Loop[num5];
									if (Settings.UseFollowNavMeshRoute)
									{
										Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, smartAiV3.AIPed, vector2.X, vector2.Y, vector2.Z, 1f, 20000, 1f, 512, smartAiV3.AIPed.Rotation.Z);
									}
									else
									{
										Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, smartAiV3.AIPed, vector2.X, vector2.Y, vector2.Z, 1f, -1, smartAiV3.AIPed.Heading + 100f, -0.1f);
									}
								}
							}
						}
					}
					if (smartAiV3.LastVehicle != null)
					{
						bool flag = true;
						if (smartAiV3.LastVehicle.AttachedBlip == null)
						{
							flag = false;
						}
						if (smartAiV3.LastVehicle.AttachedBlip != null && !smartAiV3.LastVehicle.AttachedBlip.Exists())
						{
							flag = false;
						}
						if (flag)
						{
							smartAiV3.LastVehicle.AddBlip();
							if (smartAiV3.LastVehicle.AttachedBlip != null)
							{
								smartAiV3.LastVehicle.AttachedBlip.Sprite = GetBlipSpriteForVehicle(smartAiV3.LastVehicle);
								smartAiV3.LastVehicle.AttachedBlip.Color = Settings.AIBlipColor;
								smartAiV3.LastVehicle.AttachedBlip.Name = GetStringNameFromEnemyGroup(smartAiV3.EnemyGroupName) + "'s " + smartAiV3.LastVehicle.LocalizedName;
								smartAiV3.LastVehicle.AttachedBlip.IsShortRange = true;
							}
						}
						int num7 = Function.Call<int>(Hash.GET_VEHICLE_NUMBER_OF_PASSENGERS, smartAiV3.LastVehicle, true, false);
						if (num7 <= 0 || !smartAiV3.LastVehicle.IsAlive)
						{
							if (SmartAI_SpawnedVehicles.Contains(smartAiV3.LastVehicle))
							{
								SmartAI_SpawnedVehicles.Remove(smartAiV3.LastVehicle);
							}
							if (smartAiV3.LastVehicle.AttachedBlip != null && smartAiV3.LastVehicle.AttachedBlip.Alpha == 255)
							{
								smartAiV3.LastVehicle.AttachedBlip.Alpha = 0;
								if (smartAiV3.LastVehicle.Model.IsHelicopter)
								{
									Settings.SmartAI_SpawnedVehicles--;
									Settings.SmartAI_SpawnedHelis--;
								}
								else if (smartAiV3.LastVehicle.Model.IsPlane)
								{
									Settings.SmartAI_SpawnedVehicles--;
									Settings.SmartAI_SpawnedPlanes--;
								}
								else if (smartAiV3.LastVehicle.Model.IsBoat)
								{
									Settings.SmartAI_SpawnedVehicles--;
									Settings.SmartAI_SpawnedBoats--;
								}
								else
								{
									Settings.SmartAI_SpawnedVehicles--;
									Settings.SmartAI_SpawnedCars--;
								}
							}
						}
					}
					if (smartAiV3.AIPed.CurrentVehicle != null)
					{
						if (smartAiV3.AIPed.AttachedBlip != null)
						{
							smartAiV3.AIPed.AttachedBlip.Alpha = 0;
						}
						smartAiV3.LastVehicle = smartAiV3.AIPed.CurrentVehicle;
						if (smartAiV3.LastVehicle.Driver != null)
						{
							if (!(smartAiV3.LastVehicle.Driver == smartAiV3.AIPed) || !smartAiV3.AIPed.IsAlive || Settings.IngorePlayer)
							{
								continue;
							}
							if (smartAiV3.LastVehicle.Model.IsHelicopter && Settings.CanAttackPlayerInHeli)
							{
								if (smartAiV3.HeliAttackTimer <= smartAiV3.NextHeliAttackTimer)
								{
									smartAiV3.HeliAttackTimer++;
								}
								else
								{
									if (GetDistance_VDIST2(smartAiV3.AIPed.Position, Settings.AI_Target.Position, "<", 250f))
									{
										Vector3 position = Settings.AI_Target.Position;
										int num8 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100);
										if (Game.Player.Character.CurrentVehicle != null)
										{
											Vehicle currentVehicle = Game.Player.Character.CurrentVehicle;
											if (currentVehicle.Model.IsHelicopter)
											{
												num8 += 35;
											}
											num8 = ((!currentVehicle.Model.IsPlane) ? (num8 + 25) : (num8 + 55));
										}
										if (num8 > 60 && Settings.AircraftWithMissiles.Contains(smartAiV3.AIPed.CurrentVehicle.Model.Hash) && !Settings.LockMissileUseThisFrame && Mod_Difficulty > 10)
										{
											Function.Call(Hash.SET_CURRENT_PED_VEHICLE_WEAPON, smartAiV3.AIPed, VehicleWeaponHash.PlaneRocket);
											smartAiV3.NextHeliAttackTimer = 5;
											Function.Call(Hash.SET_VEHICLE_SHOOT_AT_TARGET, smartAiV3.AIPed, Settings.AI_Target, position.X, position.Y, position.Z);
										}
										else
										{
											smartAiV3.NextHeliAttackTimer = 0;
											Function.Call(Hash.SET_CURRENT_PED_VEHICLE_WEAPON, smartAiV3.AIPed, VehicleWeaponHash.PlayerBuzzard);
										}
									}
									smartAiV3.HeliAttackTimer = 0;
								}
								if (smartAiV3.HeliRerouteTimer < Game.GameTime)
								{
									if (GetDistance_VDIST2(smartAiV3.AIPed.Position, Settings.AI_Target.Position, "<", 60f))
									{
										smartAiV3.HeliRerouteTimer = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 15000, 45000);
										int num9 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 35, 95);
										Vector3 offsetPosition = GetOffsetPosition(Game.Player.Character.Position, Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0f, 360f), new Vector3(0f, num9, 60f));
										Vector3 vector3 = offsetPosition;
										float num10 = (smartAiV3.AIPed.CurrentVehicle.Position - vector3).ToHeading() - 180f;
										Vector3 vector4 = GenerateSpawnPos(Settings.AI_Target.Position.Around(Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 50)), Nodetype.Water, false);
										Function.Call(Hash.TASK_HELI_MISSION, smartAiV3.AIPed.Handle, smartAiV3.AIPed.CurrentVehicle.Handle, 0, 0, vector3.X, vector3.Y, vector3.Z, 4, 50.0, 10.0, -1, 120, 60, -1, 64);
									}
									else
									{
										smartAiV3.HeliRerouteTimer = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 5000, 25000);
										Vector3 position2 = Settings.AI_Target.Position;
										float num11 = (smartAiV3.AIPed.CurrentVehicle.Position - position2).ToHeading() - 180f;
										if (Settings.AI_Target.Model.IsPed)
										{
											Ped ped = (Ped)Settings.AI_Target;
											if (ped.IsSwimmingUnderWater)
											{
												Vector3 vector5 = GenerateSpawnPos(Settings.AI_Target.Position.Around(Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 50)), Nodetype.Water, false);
												Function.Call(Hash.TASK_HELI_MISSION, smartAiV3.AIPed.Handle, smartAiV3.AIPed.CurrentVehicle.Handle, 0, Game.Player.Character, 0f, 0f, 0f, 6, 50.0, 10.0, -1, 120, 60, -1, 64);
											}
											else
											{
												Function.Call(Hash.TASK_HELI_MISSION, smartAiV3.AIPed.Handle, smartAiV3.AIPed.CurrentVehicle.Handle, 0, Game.Player.Character, 0f, 0f, 0f, 6, 50.0, 10.0, -1, 120, 60, -1, 64);
											}
										}
										if (Settings.AI_Target.Model.IsVehicle)
										{
											Function.Call(Hash.TASK_HELI_MISSION, smartAiV3.AIPed.Handle, smartAiV3.AIPed.CurrentVehicle.Handle, 0, Game.Player.Character, 0f, 0f, 0f, 6, 50.0, 10.0, -1, 120, 60, -1, 64);
										}
									}
								}
							}
							if (smartAiV3.LastVehicle.Model.IsPlane && Settings.CanAttackPlayerInPlane)
							{
								if (smartAiV3.HeliAttackTimer <= 5)
								{
									smartAiV3.HeliAttackTimer++;
								}
								else
								{
									if (GetDistance_VDIST2(smartAiV3.AIPed.Position, Settings.AI_Target.Position, "<", 250f))
									{
										smartAiV3.NextHeliAttackTimer = 5;
										Vector3 position3 = Settings.AI_Target.Position;
										if (Mod_Difficulty < 90)
										{
											Function.Call(Hash.DISABLE_VEHICLE_WEAPON, true, Function.Call<int>(Hash.GET_HASH_KEY, "VEHICLE_WEAPON_PLAYER_LAZER"), smartAiV3.AIPed.CurrentVehicle, smartAiV3.AIPed);
											Function.Call(Hash.SET_CURRENT_PED_VEHICLE_WEAPON, smartAiV3.AIPed, VehicleWeaponHash.PlaneRocket);
										}
										Function.Call(Hash.SET_VEHICLE_SHOOT_AT_TARGET, smartAiV3.AIPed, Settings.AI_Target, position3.X, position3.Y, position3.Z);
									}
									smartAiV3.HeliAttackTimer = 0;
								}
								if (smartAiV3.HeliAttackTimer < Game.GameTime)
								{
									smartAiV3.HeliAttackTimer = Game.GameTime + 2000;
									Vector3 position4 = Settings.AI_Target.Position;
									Vector3 vector6 = position4;
									if (Settings.AI_Target.Model.IsPed)
									{
										if (Mod_Difficulty < 90)
										{
											Function.Call(Hash.DISABLE_VEHICLE_WEAPON, true, Function.Call<int>(Hash.GET_HASH_KEY, "VEHICLE_WEAPON_PLAYER_LAZER"), smartAiV3.LastVehicle, smartAiV3.AIPed);
											Function.Call(Hash.SET_CURRENT_PED_VEHICLE_WEAPON, smartAiV3.AIPed, VehicleWeaponHash.PlaneRocket);
										}
										smartAiV3.AIPed.CombatRange = CombatRange.Near;
										smartAiV3.AIPed.CombatAbility = CombatAbility.Professional;
										smartAiV3.AIPed.VehicleWeapon = VehicleWeaponHash.PlaneRocket;
										float targetReachedDist = 25f;
										int minHeightAboveTerrain = 40;
										float num12 = Function.Call<float>(Hash.GET_VEHICLE_ESTIMATED_MAX_SPEED, smartAiV3.LastVehicle.Handle);
										float cruiseSpeed = num12 * 0.9f;
										smartAiV3.AIPed.BlockPermanentEvents = true;
										smartAiV3.AIPed.Task.StartPlaneMission(smartAiV3.LastVehicle, (Ped)Settings.AI_Target, VehicleMissionType.Attack, cruiseSpeed, targetReachedDist, -1, minHeightAboveTerrain);
									}
									if (Settings.AI_Target.Model.IsVehicle)
									{
										if (Mod_Difficulty < 90)
										{
											Function.Call(Hash.DISABLE_VEHICLE_WEAPON, true, Function.Call<int>(Hash.GET_HASH_KEY, "VEHICLE_WEAPON_PLAYER_LAZER"), smartAiV3.AIPed.CurrentVehicle, smartAiV3.AIPed);
											Function.Call(Hash.SET_CURRENT_PED_VEHICLE_WEAPON, smartAiV3.AIPed, VehicleWeaponHash.PlaneRocket);
										}
										smartAiV3.AIPed.CombatRange = CombatRange.Near;
										smartAiV3.AIPed.CombatAbility = CombatAbility.Professional;
										smartAiV3.AIPed.VehicleWeapon = VehicleWeaponHash.PlaneRocket;
										float num13 = 25f;
										int num14 = 40;
										float num15 = Function.Call<float>(Hash.GET_VEHICLE_ESTIMATED_MAX_SPEED, smartAiV3.LastVehicle.Handle);
										float num16 = num15 * 0.9f;
										smartAiV3.AIPed.BlockPermanentEvents = true;
										vector6 = vector6.Around(Function.Call<float>(Hash.GET_RANDOM_INT_IN_RANGE, 10f, 50f));
										vector6 += new Vector3(0f, 0f, Function.Call<float>(Hash.GET_RANDOM_INT_IN_RANGE, 10f, 50f));
										Function.Call(Hash.TASK_PLANE_MISSION, smartAiV3.AIPed, smartAiV3.AIPed.CurrentVehicle, (Vehicle)Settings.AI_Target, null, 0f, 0f, 0f, 6, 30f, 5, -1, 0, 80, 50);
									}
								}
							}
							if (smartAiV3.LastVehicle.Model.IsBoat && Settings.CanAttackPlayerInBoat)
							{
								Vector3 position5 = Settings.AI_Target.Position;
								Vector3 vector7 = position5;
								if (Settings.AI_Target.Model.IsPed)
								{
									Ped ped2 = (Ped)Settings.AI_Target;
									if (ped2.IsSwimmingUnderWater)
									{
										if (smartAiV3.AIPed.IsInCombatAgainst(Game.Player.Character))
										{
											smartAiV3.AIPed.Task.ClearAllImmediately();
										}
										Vector3 vector8 = GenerateSpawnPos(Settings.AI_Target.Position.Around(Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 75)), Nodetype.Water, false);
										if (smartAiV3.LastVehicle != null)
										{
											Function.Call(Hash.TASK_BOAT_MISSION, smartAiV3.AIPed, smartAiV3.LastVehicle, 0, 0, vector8.X, vector8.Y, vector8.Z, 6, 100f, 786469, -1, 0, 7);
											smartAiV3.AIPed.BlockPermanentEvents = true;
										}
									}
									else if (smartAiV3.AIPed.CurrentVehicle != null)
									{
										Function.Call(Hash.TASK_BOAT_MISSION, smartAiV3.AIPed, smartAiV3.AIPed.CurrentVehicle, 0, Settings.AI_Target, 0f, 0f, 0f, 4, 100f, 786469, -1, 0, 7);
										smartAiV3.AIPed.BlockPermanentEvents = true;
									}
								}
							}
							if (smartAiV3.LastVehicle.Model.IsBoat || smartAiV3.LastVehicle.Model.IsPlane || smartAiV3.LastVehicle.Model.IsHelicopter || !Settings.CanAttackPlayerInCar)
							{
								continue;
							}
							Vector3 position6 = Settings.AI_Target.Position;
							if (smartAiV3.HeliAttackTimer > 2)
							{
								continue;
							}
							if (GetDistance_VDIST2(Settings.AI_Target.Position, smartAiV3.AIPed.Position, ">", 10f))
							{
								if (Settings.AI_Target.Model.IsPed)
								{
									smartAiV3.AIPed.Task.ChaseWithGroundVehicle((Ped)Settings.AI_Target);
									smartAiV3.HeliAttackTimer++;
								}
								if (Settings.AI_Target.Model.IsVehicle)
								{
									Function.Call(Hash.TASK_VEHICLE_CHASE, Settings.AI_Target);
									smartAiV3.HeliAttackTimer++;
								}
							}
							else if (!smartAiV3.IsinCombatAgainstTarget)
							{
								if (smartAiV3.AIPed.CurrentVehicle != null)
								{
									smartAiV3.AIPed.Task.LeaveVehicle();
								}
								if (Settings.AI_Target == Game.Player.Character)
								{
									FightAgainst(smartAiV3, Game.Player.Character);
									smartAiV3.HasSeenPlayer = true;
									smartAiV3.TimeSinceStartCombat = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 3500, 7500);
									smartAiV3.IsinCombatAgainstTarget = true;
								}
								else
								{
									Function.Call(Hash.TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY, smartAiV3.AIPed, Settings.AI_Target, Settings.AI_Target, 90f, true, 0.8f, 0.8f, true, false, FiringPattern.FullAuto);
								}
							}
						}
						else
						{
							if (Settings.IngorePlayer || smartAiV3.ForceStayInVehicle || IsPedUsingMountedVehicleWeapon(smartAiV3.LastVehicle, smartAiV3.AIPed))
							{
								continue;
							}
							if (smartAiV3.HeliAttackTimer <= 2)
							{
								smartAiV3.HeliAttackTimer++;
							}
							else
							{
								if (Settings.IngorePlayer && !Game.Player.IsTargeting(smartAiV3.AIPed))
								{
									continue;
								}
								if (Settings.ArrestInsteadOfShoot)
								{
									if (GetDistance_VDIST2(Settings.AI_Target.Position, smartAiV3.AIPed.Position, ">", 10f) && smartAiV3.AIPed.CurrentVehicle != null)
									{
										smartAiV3.AIPed.Task.LeaveVehicle();
									}
									smartAiV3.HeliAttackTimer = 0;
									smartAiV3.AIPed.Task.GoTo(Settings.AI_Target.Position);
								}
								if (Settings.ArrestInsteadOfShoot)
								{
									continue;
								}
								if (GetDistance_VDIST2(Settings.AI_Target.Position, smartAiV3.AIPed.Position, "<", 20f) && smartAiV3.AIPed.CurrentVehicle != null)
								{
									smartAiV3.AIPed.Task.LeaveVehicle();
								}
								smartAiV3.HeliAttackTimer = 0;
								if (!smartAiV3.IsinCombatAgainstTarget)
								{
									if (Settings.AI_Target == Game.Player.Character)
									{
										FightAgainst(smartAiV3, Game.Player.Character);
										smartAiV3.HasSeenPlayer = true;
										smartAiV3.TimeSinceStartCombat = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 3500, 7500);
										smartAiV3.IsinCombatAgainstTarget = true;
									}
									else
									{
										Function.Call(Hash.TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY, smartAiV3.AIPed, Settings.AI_Target, Settings.AI_Target, 90f, true, 0.8f, 0.8f, true, false, FiringPattern.FullAuto);
									}
								}
							}
						}
					}
					else
					{
						if (!smartAiV3.AIPed.IsAlive || !smartAiV3.AIPed.IsAlive || smartAiV3.AIPed.IsInCombatAgainst(Game.Player.Character))
						{
							continue;
						}
						if (Game.Player.IsTargeting(smartAiV3.AIPed))
						{
							Settings.HasTargettedAI = true;
						}
						bool targetImVisibleZone = smartAiV3.TargetImVisibleZone;
						if (Settings.CameraSeePlayer && !smartAiV3.ArrestingPlayer && GetDistance_VDIST2(smartAiV3.AIPed.Position, Settings.CameraPos, "<", 100f))
						{
							if (smartAiV3.InvestigatingCamera && targetImVisibleZone)
							{
								smartAiV3.InvestigatingCamera = false;
							}
							if (!Settings.GuardInvestigatingCamera)
							{
								Settings.GuardInvestigatingCamera = true;
								smartAiV3.InvestigatePoint = Settings.CameraPos;
								smartAiV3.InvestigatingGunshot = true;
								smartAiV3.AIPed.Task.GoTo(smartAiV3.InvestigatePoint);
								smartAiV3.InvestigatingCamera = true;
							}
						}
						if (0 == 0 || smartAiV3.HasSeenPlayer || !Settings.AllowInvestigate || !smartAiV3.AIPed.IsAlive || smartAiV3.ArrestingPlayer)
						{
							continue;
						}
						if (smartAiV3.MovingBacktoOriginalPoint)
						{
							if (smartAiV3.Timer != 5)
							{
								smartAiV3.Timer++;
							}
							else
							{
								smartAiV3.Timer = 0;
								if (GetDistance_VDIST2(smartAiV3.AIPed.Position, smartAiV3.OriginalPosition, ">", 1.35f))
								{
									smartAiV3.AIPed.Task.GoTo(smartAiV3.OriginalPosition);
								}
								if (GetDistance_VDIST2(smartAiV3.AIPed.Position, smartAiV3.OriginalPosition, "<", 1.35f))
								{
									if (new Vector3(0f, 0f, smartAiV3.OriginalHeading).DistanceTo(new Vector3(0f, 0f, smartAiV3.AIPed.Heading)) >= 5f)
									{
										Function.Call(Hash.TASK_ACHIEVE_HEADING, smartAiV3.AIPed, smartAiV3.OriginalHeading, 20000);
									}
									else
									{
										smartAiV3.MovingBacktoOriginalPoint = false;
									}
								}
							}
						}
						if (smartAiV3.InvestigatingDeadBody)
						{
							smartAiV3.AIPed.BlockPermanentEvents = true;
							if (smartAiV3.InvestigatingGunshot && !smartAiV3.MovingBacktoOriginalPoint)
							{
								if (smartAiV3.AIPed.Position.DistanceTo(smartAiV3.InvestigatePoint) < 3.5f)
								{
									smartAiV3.InvestigationTimeout++;
								}
								if (smartAiV3.InvestigationTimeout >= 5)
								{
									smartAiV3.InvestigationTimeout = 0;
									smartAiV3.InvestigatingGunshot = false;
									smartAiV3.InvestigatingDeadBody = false;
									smartAiV3.MovingBacktoOriginalPoint = true;
								}
								if (smartAiV3.Timer != 5)
								{
									smartAiV3.Timer++;
								}
								else
								{
									smartAiV3.Timer = 0;
									if (smartAiV3.AIPed.Position.DistanceTo(smartAiV3.InvestigatePoint) > 3.5f)
									{
										smartAiV3.AIPed.Task.GoTo(smartAiV3.InvestigatePoint);
									}
									else
									{
										smartAiV3.AIPed.Task.GuardCurrentPosition();
									}
								}
							}
						}
						if (!smartAiV3.InvestigatingGunshot && GetDistance_VDIST2(Settings.Player_LastWeaponImpactPosition, smartAiV3.AIPed.Position, "<", Settings.InvestigateDist))
						{
							bool flag2 = false;
							Vector3 investigatePoint = Settings.AI_Target.Position;
							float investigateDist = Settings.InvestigateDist;
							if (Function.Call<bool>(Hash.IS_PED_CURRENT_WEAPON_SILENCED, Settings.AI_Target))
							{
								investigateDist = Settings.InvestigateDist / 2f;
								if (GetDistance_VDIST2(smartAiV3.AIPed.Position, Settings.AI_Target.Position, "<", 5f))
								{
									investigatePoint = Settings.AI_Target.Position;
								}
								else if (GetDistance_VDIST2(smartAiV3.AIPed.Position, Settings.Player_LastWeaponImpactPosition, "<", investigateDist))
								{
									investigatePoint = Settings.Player_LastWeaponImpactPosition;
								}
								else
								{
									flag2 = true;
								}
							}
							else
							{
								investigatePoint = Settings.AI_Target.Position;
							}
							if (!flag2)
							{
								smartAiV3.InvestigatePoint = investigatePoint;
								smartAiV3.InvestigatingGunshot = true;
								smartAiV3.AIPed.Task.GoTo(smartAiV3.InvestigatePoint);
							}
						}
						if (!smartAiV3.InvestigatingGunshot || !smartAiV3.AIPed.IsAlive)
						{
							continue;
						}
						smartAiV3.AIPed.BlockPermanentEvents = true;
						if (!smartAiV3.InvestigatingGunshot || smartAiV3.MovingBacktoOriginalPoint)
						{
							continue;
						}
						if (smartAiV3.AIPed.Position.DistanceTo(smartAiV3.InvestigatePoint) < 3.5f)
						{
							smartAiV3.InvestigationTimeout++;
						}
						if (smartAiV3.InvestigationTimeout >= 5)
						{
							smartAiV3.InvestigationTimeout = 0;
							smartAiV3.InvestigatingGunshot = false;
							smartAiV3.MovingBacktoOriginalPoint = true;
						}
						if (smartAiV3.Timer != 5)
						{
							smartAiV3.Timer++;
							continue;
						}
						smartAiV3.Timer = 0;
						if (smartAiV3.AIPed.Position.DistanceTo(smartAiV3.InvestigatePoint) > 3.5f)
						{
							smartAiV3.AIPed.Task.GoTo(smartAiV3.InvestigatePoint);
						}
						else
						{
							smartAiV3.AIPed.Task.GuardCurrentPosition();
						}
					}
				}
				Settings.Player_LastWeaponImpactPosition = new Vector3(0f, 0f, 0f);
			}
			if (Settings.DetectionTimer < Game.GameTime)
			{
				Settings.DetectionTimer = Game.GameTime + 100;
				for (int l = 0; l < Merryweather.Count; l++)
				{
					SmartAiV2 smartAiV4 = Merryweather[l];
					if (smartAiV4 == null || !(smartAiV4.AIPed != null) || !smartAiV4.AIPed.IsAlive)
					{
						continue;
					}
					if (Settings.CansSpawnDeadBodyMarkers && !smartAiV4.HasSeenPlayer && !smartAiV4.HaveFoundDeadBody)
					{
						bool flag3 = false;
						Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, smartAiV4.AIPed.Position.X, smartAiV4.AIPed.Position.Y, smartAiV4.AIPed.Position.Z, 20f, Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_x17_corpse_01"), 0, 0, 0);
						if (prop != null)
						{
							if (GetDistance_VDIST2(smartAiV4.AIPed.Position, prop.Position, "<", Settings.Radius / 2f))
							{
								RaycastResult raycastResult = World.Raycast(smartAiV4.AIPed.GetOffsetPosition(new Vector3(0f, 0.5f, 0f)), prop.Position, IntersectFlags.Everything);
								float num17 = (prop.Position - smartAiV4.AIPed.Position).ToHeading();
								float num18 = 0f;
								if (num17 > 360f - Settings.SeeingAngle)
								{
									num18 = num17 - smartAiV4.AIPed.Heading;
									if (num18 > 360f - Settings.SeeingAngle)
									{
										num18 -= 360f;
									}
								}
								else
								{
									num18 = num17 - smartAiV4.AIPed.Heading;
								}
								if (num18 >= 0f - Settings.SeeingAngle && num18 <= Settings.SeeingAngle)
								{
									flag3 = true;
								}
								if (flag3)
								{
									Vector3 offsetPosition2 = smartAiV4.AIPed.GetOffsetPosition(new Vector3(0f, 0.1f, 1f));
									RaycastResult raycastResult2 = World.Raycast(offsetPosition2, Settings.AI_Target.Position, IntersectFlags.Everything);
									flag3 = raycastResult2.DidHit && (GetDistance_VDIST2(raycastResult2.HitPosition, Settings.AI_Target.Position, "<", 0.5f) ? true : false);
								}
							}
							if (flag3)
							{
								if (!smartAiV4.CanSeeDeadBody)
								{
									Function.Call(Hash.TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY, smartAiV4.AIPed, prop, prop, 1f, false, 0.8f, 0.8f, true, false, FiringPattern.FullAuto);
									smartAiV4.SeenDeadBodyTimemeTillAlert = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 3500, 7500);
									smartAiV4.CanSeeDeadBody = true;
								}
								else if (Game.GameTime >= smartAiV4.SeenDeadBodyTimemeTillAlert)
								{
									smartAiV4.CanSeeDeadBody = false;
									smartAiV4.HaveFoundDeadBody = true;
									Settings.HaveFoundDeadBody = true;
									smartAiV4.HaveFoundDeadBody = true;
								}
							}
						}
					}
					Vector3 position7 = smartAiV4.AIPed.Position;
					Vector3 position8 = Settings.AI_Target.Position;
					float num19 = position7.Z - Settings.ShowBlipHeightMinus;
					float num20 = position7.Z + Settings.ShowBlipHeightAdd;
					if (!Settings.IngorePlayer)
					{
						if (Settings.IgnoreRagdollBulletImpact)
						{
							Function.Call(Hash.SET_RAGDOLL_BLOCKING_FLAGS, smartAiV4.AIPed, 0);
							Function.Call(Hash.SET_PED_CONFIG_FLAG, smartAiV4.AIPed, 107, true);
						}
						smartAiV4.TargetImVisibleZone = false;
						if (GetDistance_VDIST2(smartAiV4.AIPed.Position, Settings.AI_Target.Position, "<", Settings.Radius / 2f))
						{
							if (smartAiV4.AIPed.CurrentVehicle == null && smartAiV4.AIPed.AttachedBlip != null)
							{
								smartAiV4.AIPed.AttachedBlip.Alpha = 255;
							}
							RaycastResult raycastResult3 = World.Raycast(smartAiV4.AIPed.GetOffsetPosition(new Vector3(0f, 0.5f, 0f)), Settings.AI_Target.Position, IntersectFlags.Everything);
							float num21 = (Settings.AI_Target.Position - smartAiV4.AIPed.Position).ToHeading();
							float num22 = 0f;
							if (num21 > 360f - Settings.SeeingAngle)
							{
								num22 = num21 - smartAiV4.AIPed.Heading;
								if (num22 > 360f - Settings.SeeingAngle)
								{
									num22 -= 360f;
								}
							}
							else
							{
								num22 = num21 - smartAiV4.AIPed.Heading;
							}
							if (num22 >= 0f - Settings.SeeingAngle && num22 <= Settings.SeeingAngle)
							{
								smartAiV4.TargetImVisibleZone = true;
							}
							Vector3 offsetPosition3 = smartAiV4.AIPed.GetOffsetPosition(new Vector3(0f, 0.1f, 1f));
							RaycastResult raycastResult4 = World.Raycast(offsetPosition3, Settings.AI_Target.Position, IntersectFlags.Everything);
							if (raycastResult4.DidHit)
							{
								if (GetDistance_VDIST2(raycastResult4.HitPosition, Settings.AI_Target.Position, "<", 0.5f))
								{
									smartAiV4.TargetImVisibleZone = true;
								}
								else
								{
									smartAiV4.TargetImVisibleZone = false;
								}
							}
							else
							{
								smartAiV4.TargetImVisibleZone = false;
							}
						}
						bool flag4 = Function.Call<bool>(Hash.CAN_PED_HEAR_PLAYER, Game.Player, smartAiV4.AIPed);
						if (!flag4)
						{
						}
						if (Game.Player.Character.IsShooting)
						{
							if (Function.Call<bool>(Hash.IS_PED_CURRENT_WEAPON_SILENCED, Game.Player.Character))
							{
								if (GetDistance_VDIST2(Settings.AI_Target.Position, smartAiV4.AIPed.Position, "<", 15f))
								{
									flag4 = true;
								}
							}
							else if (GetDistance_VDIST2(Settings.AI_Target.Position, smartAiV4.AIPed.Position, "<", 65f))
							{
								flag4 = true;
							}
						}
						if (flag4 && smartAiV4.AIPed.IsAlive && !smartAiV4.ArrestingPlayer && !smartAiV4.HasSeenPlayer && !smartAiV4.AIPed.IsInCombat)
						{
							float radius = Settings.HearingRange / 2.5f;
							if (Game.Player.Character.IsShooting)
							{
								if (Function.Call<bool>(Hash.IS_PED_CURRENT_WEAPON_SILENCED, Game.Player.Character))
								{
									if (GetDistance_VDIST2(Settings.AI_Target.Position, smartAiV4.AIPed.Position, "<", 5f))
									{
										radius = Settings.HearingRange / 2.5f;
									}
								}
								else if (GetDistance_VDIST2(Settings.AI_Target.Position, smartAiV4.AIPed.Position, "<", 65f))
								{
									radius = 65f;
								}
							}
							if (GetDistance_VDIST2(smartAiV4.AIPed.Position, Settings.AI_Target.Position, "<", radius))
							{
								float num23 = (Settings.AI_Target.Position - smartAiV4.AIPed.Position).ToHeading();
								if (!Settings.IngorePlayer)
								{
									if (smartAiV4.AiProp != null && smartAiV4.AiProp.IsAttached())
									{
										smartAiV4.AiProp.Detach();
										smartAiV4.AiProp.HasGravity = true;
										smartAiV4.AiProp.IsPositionFrozen = false;
									}
									if (!Settings.AllowInvestigateBeforeShoot)
									{
										FightAgainst(smartAiV4, Game.Player.Character);
										smartAiV4.HasSeenPlayer = true;
										smartAiV4.TimeSinceStartCombat = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 3500, 7500);
									}
									else if (!smartAiV4.IsInvestigatingSightingOfPlayer)
									{
										if (Settings.HasSeenPlayer)
										{
											smartAiV4.StopInvestigateTime = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 500, 1500);
										}
										if (!Settings.HasSeenPlayer)
										{
											smartAiV4.StopInvestigateTime = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 1500, 3500);
										}
										smartAiV4.IsInvestigatingSightingOfPlayer = true;
										Function.Call(Hash.TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY, smartAiV4.AIPed, Settings.AI_Target, Settings.AI_Target, 1f, false, 0.8f, 0.8f, true, false, FiringPattern.FullAuto);
									}
								}
							}
						}
						if (GetDistance_VDIST2(smartAiV4.AIPed.Position, LastHitCoords, "<", 3.5f) && !smartAiV4.AIPed.IsInCombat)
						{
							float num24 = (Settings.AI_Target.Position - smartAiV4.AIPed.Position).ToHeading();
							if (!Settings.IngorePlayer)
							{
								if (smartAiV4.AiProp != null && smartAiV4.AiProp.IsAttached())
								{
									smartAiV4.AiProp.Detach();
									smartAiV4.AiProp.HasGravity = true;
									smartAiV4.AiProp.IsPositionFrozen = false;
								}
								if (!Settings.AllowInvestigateBeforeShoot)
								{
									FightAgainst(smartAiV4, Game.Player.Character);
									smartAiV4.HasSeenPlayer = true;
									smartAiV4.TimeSinceStartCombat = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 3500, 7500);
								}
								else if (!smartAiV4.IsInvestigatingSightingOfPlayer)
								{
									if (Settings.HasSeenPlayer)
									{
										smartAiV4.StopInvestigateTime = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 500, 1500);
									}
									if (!Settings.HasSeenPlayer)
									{
										smartAiV4.StopInvestigateTime = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 1500, 3500);
									}
									smartAiV4.IsInvestigatingSightingOfPlayer = true;
									Function.Call(Hash.TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY, smartAiV4.AIPed, Settings.AI_Target, Settings.AI_Target, 1f, false, 0.8f, 0.8f, true, false, FiringPattern.FullAuto);
								}
							}
						}
						bool flag5 = true;
						if (Settings.ArrestInsteadOfShoot)
						{
							if (Settings.ArrestInsteadOfShoot && smartAiV4.TargetImVisibleZone)
							{
								smartAiV4.ArrestingPlayer = true;
								smartAiV4.InvestigatingCamera = false;
								if (GetDistance_VDIST2(Settings.AI_Target.Position, smartAiV4.AIPed.Position, ">", 1f))
								{
									if (Settings.CurrentArrestPed == null)
									{
										Settings.CurrentArrestPed = smartAiV4.AIPed;
									}
									else if (Settings.CurrentArrestPed != null && Settings.CurrentArrestPed == smartAiV4.AIPed)
									{
										Function.Call(Hash.TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY, smartAiV4.AIPed, Settings.AI_Target, Settings.AI_Target, 90f, false, 0.8f, 0.8f, true, false, FiringPattern.FullAuto);
									}
								}
								if (Settings.CurrentArrestPed != null && !Settings.CurrentArrestPed.IsAlive)
								{
									Settings.CurrentArrestPed = null;
								}
								if (GetDistance_VDIST2(Settings.AI_Target.Position, smartAiV4.AIPed.Position, "<", 1.5f))
								{
									Settings.CurrentArrestPed = null;
									smartAiV4.MovingBacktoOriginalPoint = true;
									smartAiV4.HasSeenPlayer = false;
									smartAiV4.AIPed.Position = smartAiV4.OriginalPosition;
									smartAiV4.AIPed.Heading = smartAiV4.OriginalHeading;
									Settings.GuardInvestigatingCamera = false;
									smartAiV4.ArrestingPlayer = false;
								}
							}
						}
						else if ((!Settings.ArrestInsteadOfShoot && !smartAiV4.HasSeenPlayer && flag5 && smartAiV4.TargetImVisibleZone) || GetDistance_VDIST2(Settings.AI_Target.Position, smartAiV4.AIPed.Position, "<=", 3.5f))
						{
							RaycastResult raycastResult5 = World.Raycast(smartAiV4.AIPed.GetOffsetPosition(new Vector3(0f, 0.5f, 0f)), Settings.AI_Target.Position, IntersectFlags.Everything);
							if ((raycastResult5.DidHit || GetDistance_VDIST2(Settings.AI_Target.Position, smartAiV4.AIPed.Position, "<=", 4.5f)) && GetDistance_VDIST2(Settings.AI_Target.Position, raycastResult5.HitPosition, "<", Settings.HitRadius) && (!Settings.IngorePlayer || Game.Player.IsTargeting(smartAiV4.AIPed)) && GetDistance_VDIST2(smartAiV4.AIPed.Position, Settings.AI_Target.Position, "<", Settings.Radius) && !smartAiV4.AIPed.IsInCombatAgainst(Game.Player.Character) && smartAiV4.AIPed.IsAlive)
							{
								if (!Settings.IngorePlayer && !Settings.ArrestInsteadOfShoot)
								{
									if (!Settings.AllowInvestigateBeforeShoot)
									{
										if (!smartAiV4.AIPed.IsInCombatAgainst(Game.Player.Character))
										{
											if (flag4 || smartAiV4.TargetImVisibleZone)
											{
												if (Settings.HasSeenPlayer)
												{
													smartAiV4.PlayerInVisualRangeTimer = 40;
												}
												smartAiV4.PlayerInVisualRangeTimer++;
												if (smartAiV4.PlayerInVisualRangeTimer >= 5 && smartAiV4.PlayerInVisualRangeTimer <= 10)
												{
													float heading = (Settings.AI_Target.Position - smartAiV4.AIPed.Position).ToHeading();
													float heading2 = smartAiV4.AIPed.Heading;
													Vector3 position9 = smartAiV4.AIPed.Position;
													int taskID2 = 0;
													OpenSequenceTask(&taskID2);
													TaskPedSlideToCoord(0, position9, heading, 8f);
													TaskStandStill(0, Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 1000, 4500));
													TaskPedSlideToCoord(0, position9, heading2, 8f);
													CloseSequenceTask(taskID2);
													TaskPerformSequence(smartAiV4.AIPed, taskID2);
													ClearSequenceTask(&taskID2);
												}
												if (smartAiV4.TargetImVisibleZone && smartAiV4.PlayerInVisualRangeTimer >= 35 && !smartAiV4.IsinCombatAgainstTarget)
												{
													if (smartAiV4.AiProp != null && smartAiV4.AiProp.IsAttached())
													{
														smartAiV4.AiProp.Detach();
														smartAiV4.AiProp.HasGravity = true;
														smartAiV4.AiProp.IsPositionFrozen = false;
													}
													RelationshipGroup relationshipGroup2 = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
													Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, smartAiV4.AIPed, relationshipGroup2);
													Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, smartAiV4.AIPed, 0, 0);
													Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, smartAiV4.AIPed, 1);
													FightAgainst(smartAiV4, Game.Player.Character);
													smartAiV4.HasSeenPlayer = true;
													smartAiV4.TimeSinceStartCombat = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 3500, 7500);
													smartAiV4.IsinCombatAgainstTarget = true;
													foreach (SmartAiV2 item in Merryweather)
													{
														if (item != null && item.AIPed != null && GetDistance_VDIST2(item.AIPed.Position, smartAiV4.AIPed.Position, "<", 10f) && item.AIPed.IsAlive && !item.AIPed.IsInCombatAgainst(Game.Player.Character) && ((!Settings.IngorePlayer && !Settings.ArrestInsteadOfShoot) || Game.Player.IsTargeting(item.AIPed)))
														{
															item.HasSeenPlayer = true;
															item.AIPed.Task.ClearAllImmediately();
															relationshipGroup2 = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
															Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, item.AIPed, relationshipGroup2);
															Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, item.AIPed, 0, 0);
															Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, item.AIPed, 1);
															FightAgainst(smartAiV4, Game.Player.Character);
															item.HasSeenPlayer = true;
															smartAiV4.TimeSinceStartCombat = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 3500, 7500);
															item.AIPed.CanWrithe = false;
														}
													}
												}
												if (flag4 && smartAiV4.PlayerInVisualRangeTimer >= 30 && !smartAiV4.IsinCombatAgainstTarget)
												{
													if (smartAiV4.AiProp != null && smartAiV4.AiProp.IsAttached())
													{
														smartAiV4.AiProp.Detach();
														smartAiV4.AiProp.HasGravity = true;
														smartAiV4.AiProp.IsPositionFrozen = false;
													}
													RelationshipGroup relationshipGroup3 = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
													Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, smartAiV4.AIPed, relationshipGroup3);
													Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, smartAiV4.AIPed, 0, 0);
													Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, smartAiV4.AIPed, 1);
													FightAgainst(smartAiV4, Game.Player.Character);
													smartAiV4.HasSeenPlayer = true;
													smartAiV4.TimeSinceStartCombat = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 3500, 7500);
													smartAiV4.IsinCombatAgainstTarget = true;
													foreach (SmartAiV2 item2 in Merryweather)
													{
														if (item2 != null && item2.AIPed != null && GetDistance_VDIST2(item2.AIPed.Position, smartAiV4.AIPed.Position, "<", 10f) && item2.AIPed.IsAlive && !item2.AIPed.IsInCombatAgainst(Game.Player.Character) && ((!Settings.IngorePlayer && !Settings.ArrestInsteadOfShoot) || Game.Player.IsTargeting(item2.AIPed)))
														{
															item2.HasSeenPlayer = true;
															item2.AIPed.Task.ClearAllImmediately();
															relationshipGroup3 = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
															Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, item2.AIPed, relationshipGroup3);
															Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, item2.AIPed, 0, 0);
															Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, item2.AIPed, 1);
															FightAgainst(smartAiV4, Game.Player.Character);
															item2.HasSeenPlayer = true;
															item2.AIPed.CanWrithe = false;
														}
													}
												}
											}
											else
											{
												smartAiV4.PlayerInVisualRangeTimer = 0;
											}
										}
									}
									else if (smartAiV4.IsInvestigatingSightingOfPlayer && !smartAiV4.AIPed.IsInCombatAgainst(Game.Player.Character))
									{
										if (flag4 || smartAiV4.TargetImVisibleZone)
										{
											if (smartAiV4.TargetImVisibleZone && Game.GameTime > smartAiV4.StopInvestigateTime && !smartAiV4.IsinCombatAgainstTarget)
											{
												if (smartAiV4.AiProp != null && smartAiV4.AiProp.IsAttached())
												{
													smartAiV4.AiProp.Detach();
													smartAiV4.AiProp.HasGravity = true;
													smartAiV4.AiProp.IsPositionFrozen = false;
												}
												RelationshipGroup relationshipGroup4 = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
												Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, smartAiV4.AIPed, relationshipGroup4);
												Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, smartAiV4.AIPed, 0, 0);
												Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, smartAiV4.AIPed, 1);
												FightAgainst(smartAiV4, Game.Player.Character);
												smartAiV4.HasSeenPlayer = true;
												smartAiV4.IsInvestigatingSightingOfPlayer = false;
												smartAiV4.TimeSinceStartCombat = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 3500, 7500);
												smartAiV4.IsinCombatAgainstTarget = true;
												foreach (SmartAiV2 item3 in Merryweather)
												{
													if (item3 != null && item3.AIPed != null && GetDistance_VDIST2(item3.AIPed.Position, smartAiV4.AIPed.Position, "<", 10f) && item3.AIPed.IsAlive && !item3.AIPed.IsInCombatAgainst(Game.Player.Character) && ((!Settings.IngorePlayer && !Settings.ArrestInsteadOfShoot) || Game.Player.IsTargeting(item3.AIPed)))
													{
														item3.HasSeenPlayer = true;
														item3.AIPed.Task.ClearAllImmediately();
														relationshipGroup4 = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
														Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, item3.AIPed, relationshipGroup4);
														Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, item3.AIPed, 0, 0);
														Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, item3.AIPed, 1);
														FightAgainst(smartAiV4, Game.Player.Character);
														item3.HasSeenPlayer = true;
														smartAiV4.TimeSinceStartCombat = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 3500, 7500);
														item3.AIPed.CanWrithe = false;
													}
												}
											}
											if (flag4 && smartAiV4.PlayerInVisualRangeTimer >= 30 && !smartAiV4.IsinCombatAgainstTarget)
											{
												if (smartAiV4.AiProp != null && smartAiV4.AiProp.IsAttached())
												{
													smartAiV4.AiProp.Detach();
													smartAiV4.AiProp.HasGravity = true;
													smartAiV4.AiProp.IsPositionFrozen = false;
												}
												RelationshipGroup relationshipGroup5 = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
												Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, smartAiV4.AIPed, relationshipGroup5);
												Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, smartAiV4.AIPed, 0, 0);
												Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, smartAiV4.AIPed, 1);
												FightAgainst(smartAiV4, Game.Player.Character);
												smartAiV4.HasSeenPlayer = true;
												smartAiV4.TimeSinceStartCombat = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 3500, 7500);
												smartAiV4.IsinCombatAgainstTarget = true;
												foreach (SmartAiV2 item4 in Merryweather)
												{
													if (item4 != null && item4.AIPed != null && GetDistance_VDIST2(item4.AIPed.Position, smartAiV4.AIPed.Position, "<", 10f) && item4.AIPed.IsAlive && !item4.AIPed.IsInCombatAgainst(Game.Player.Character) && ((!Settings.IngorePlayer && !Settings.ArrestInsteadOfShoot) || Game.Player.IsTargeting(item4.AIPed)))
													{
														item4.HasSeenPlayer = true;
														item4.AIPed.Task.ClearAllImmediately();
														relationshipGroup5 = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
														Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, item4.AIPed, relationshipGroup5);
														Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, item4.AIPed, 0, 0);
														Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, item4.AIPed, 1);
														FightAgainst(smartAiV4, Game.Player.Character);
														item4.HasSeenPlayer = true;
														item4.AIPed.CanWrithe = false;
													}
												}
											}
										}
										else
										{
											smartAiV4.PlayerInVisualRangeTimer = 0;
										}
									}
								}
								smartAiV4.AIPed.CanWrithe = false;
							}
						}
					}
					if (position8.Z > num19 && position8.Z < num20)
					{
						if (smartAiV4.AIPed.AttachedBlip != null)
						{
							Function.Call(Hash.SET_BLIP_SHOW_CONE, smartAiV4.AIPed.AttachedBlip.Handle, Settings.ShowCone, 11);
							if (!smartAiV4.ShowingPedBlipCone)
							{
								if (Settings.DisableAiBlip)
								{
									smartAiV4.ShowingPedBlipCone = true;
									smartAiV4.AIPed.AttachedBlip.Alpha = 0;
								}
								else
								{
									smartAiV4.ShowingPedBlipCone = true;
									smartAiV4.AIPed.AttachedBlip.Alpha = 255;
								}
							}
						}
						if (smartAiV4.AIPed.AttachedBlip == null)
						{
							Function.Call(Hash.SET_PED_CONFIG_FLAG, 324, true);
							Function.Call(Hash.SET_PED_VISUAL_FIELD_CENTER_ANGLE, smartAiV4.AIPed, Settings.SeeingAngle - 5f);
							Function.Call(Hash.SET_PED_SEEING_RANGE, smartAiV4.AIPed, Settings.Radius / 3f);
							Function.Call(Hash.SET_PED_HEARING_RANGE, smartAiV4.AIPed, Settings.HearingRange);
							Function.Call(Hash.SET_PED_VISUAL_FIELD_MAX_ANGLE, smartAiV4.AIPed, 5f);
							Function.Call(Hash.SET_PED_VISUAL_FIELD_MIN_ANGLE, smartAiV4.AIPed, -5f);
							smartAiV4.AIPed.AddBlip();
							smartAiV4.AIPed.AttachedBlip.Scale = 0.75f;
							smartAiV4.AIPed.AttachedBlip.Sprite = BlipSprite.Enemy;
							smartAiV4.AIPed.AttachedBlip.Name = GetStringNameFromEnemyGroup(smartAiV4.EnemyGroupName);
							smartAiV4.AIPed.AttachedBlip.Color = Settings.AIBlipColor;
							smartAiV4.AIPed.AttachedBlip.IsShortRange = true;
							RelationshipGroup relationshipGroup6 = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
							Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, smartAiV4.AIPed, relationshipGroup6);
							Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, smartAiV4.AIPed, 0, 0);
							Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, smartAiV4.AIPed, 1);
							continue;
						}
						smartAiV4.AIPed.AttachedBlip.Color = Settings.AIBlipColor;
						Function.Call(Hash.SET_PED_CONFIG_FLAG, 324, true);
						Function.Call(Hash.SET_PED_VISUAL_FIELD_CENTER_ANGLE, smartAiV4.AIPed, Settings.SeeingAngle - 5f);
						float num25 = Settings.Radius / 3f;
						Function.Call(Hash.SET_PED_SEEING_RANGE, smartAiV4.AIPed, num25);
						if (smartAiV4.AIPed.AttachedBlip != null)
						{
							smartAiV4.AIPed.AttachedBlip.Scale = 0.75f;
							smartAiV4.AIPed.AttachedBlip.Sprite = BlipSprite.Enemy;
							smartAiV4.AIPed.AttachedBlip.Name = GetStringNameFromEnemyGroup(smartAiV4.EnemyGroupName);
							smartAiV4.AIPed.AttachedBlip.Color = Settings.AIBlipColor;
						}
						if (smartAiV4.IsInvestigatingSightingOfPlayer || smartAiV4.CanSeeDeadBody)
						{
							if (smartAiV4.AIPed.AttachedBlip != null)
							{
								smartAiV4.AIPed.AttachedBlip.Scale = 0.75f;
								smartAiV4.AIPed.AttachedBlip.Sprite = BlipSprite.StrangersAndFreaks;
								smartAiV4.AIPed.AttachedBlip.Name = GetStringNameFromEnemyGroup(smartAiV4.EnemyGroupName);
								smartAiV4.AIPed.AttachedBlip.Color = Settings.AIBlipColor;
							}
							Function.Call(Hash.SET_PED_SEEING_RANGE, smartAiV4.AIPed, num25 *= 1.35f);
						}
						if (smartAiV4.HasSeenPlayer)
						{
							if (smartAiV4.AIPed.AttachedBlip != null)
							{
								smartAiV4.AIPed.AttachedBlip.Scale = 0.95f;
								smartAiV4.AIPed.AttachedBlip.Sprite = BlipSprite.Enemy;
								smartAiV4.AIPed.AttachedBlip.Name = GetStringNameFromEnemyGroup(smartAiV4.EnemyGroupName);
								smartAiV4.AIPed.AttachedBlip.Color = Settings.AIBlipColor;
							}
							Function.Call(Hash.SET_PED_SEEING_RANGE, smartAiV4.AIPed, num25 *= 1.5f);
						}
						Function.Call(Hash.SET_PED_HEARING_RANGE, smartAiV4.AIPed, Settings.HearingRange);
						Function.Call(Hash.SET_PED_VISUAL_FIELD_MAX_ANGLE, smartAiV4.AIPed, 5f);
						Function.Call(Hash.SET_PED_VISUAL_FIELD_MIN_ANGLE, smartAiV4.AIPed, -5f);
					}
					else if (smartAiV4.ShowingPedBlipCone && smartAiV4.AIPed.AttachedBlip != null)
					{
						smartAiV4.ShowingPedBlipCone = false;
						Function.Call(Hash.SET_BLIP_SHOW_CONE, smartAiV4.AIPed.AttachedBlip.Handle, false, 11);
						smartAiV4.AIPed.AttachedBlip.Alpha = 0;
					}
				}
			}
			if (Settings.CombatTimer < Game.GameTime)
			{
				Settings.CombatTimer = Game.GameTime + 100;
				for (int m = 0; m < Merryweather.Count; m++)
				{
					SmartAiV2 smartAiV5 = Merryweather[m];
					if (smartAiV5 != null && !Settings.IngorePlayer && smartAiV5.AIPed != null && !Settings.HasSeenPlayer && smartAiV5.TimeSinceStartCombat >= 1 && smartAiV5.IsinCombatAgainstTarget && Game.GameTime >= smartAiV5.TimeSinceStartCombat)
					{
						Settings.HasSeenPlayer = true;
					}
				}
			}
			if (Settings.BlipDeleteCheck < Game.GameTime)
			{
				Settings.BlipDeleteCheck = Game.GameTime + 150;
				for (int n = 0; n < Merryweather.Count; n++)
				{
					SmartAiV2 smartAiV6 = Merryweather[n];
					if (smartAiV6 != null)
					{
						if (!(smartAiV6.AIPed != null) || !smartAiV6.GroupActive)
						{
							continue;
						}
						if (smartAiV6.AIPed.Exists())
						{
							if (smartAiV6.SpawnSafeTimer >= Game.GameTime || !smartAiV6.AIPed.IsDead)
							{
								continue;
							}
							if (Settings.CansSpawnDeadBodyMarkers && !smartAiV6.SpawnedDeadBodyMarker)
							{
								smartAiV6.SpawnedDeadBodyMarker = true;
								Prop prop2 = World.CreateProp(RequestModel("xm_prop_x17_corpse_01"), smartAiV6.AIPed.Position, smartAiV6.AIPed.Rotation, false, false);
								Function.Call(Hash.PLACE_OBJECT_ON_GROUND_OR_OBJECT_PROPERLY, prop2);
								prop2.IsCollisionEnabled = false;
								prop2.IsVisible = false;
								Settings.DeadBodyMarkers.Add(prop2);
							}
							smartAiV6.isDead = true;
							if (smartAiV6.LastVehicle != null)
							{
								if (smartAiV6.LastVehicle.AttachedBlip != null)
								{
									smartAiV6.LastVehicle.AttachedBlip.Delete();
								}
								if (!EntitiesToDeletePostMission.Contains(smartAiV6.LastVehicle))
								{
									EntitiesToDeletePostMission.Add(smartAiV6.LastVehicle);
								}
							}
							if (smartAiV6.AIPed.AttachedBlip != null)
							{
								Function.Call(Hash.SET_BLIP_SHOW_CONE, smartAiV6.AIPed.AttachedBlip.Handle, false, 11);
								if (smartAiV6.AIPed.AttachedBlip != null)
								{
									smartAiV6.AIPed.AttachedBlip.Delete();
								}
								if (!EntitiesToDeletePostMission.Contains(smartAiV6.AIPed))
								{
									EntitiesToDeletePostMission.Add(smartAiV6.AIPed);
								}
								if (AiBackup.Contains(smartAiV6.AIPed))
								{
									AiBackup.Remove(smartAiV6.AIPed);
								}
								DeadPeds.Add(smartAiV6.AIPed);
								Merryweather.RemoveAt(n);
							}
						}
						else
						{
							Merryweather.RemoveAt(n);
						}
					}
					else
					{
						Merryweather.RemoveAt(n);
					}
				}
			}
			Settings.AllowDespawnIfTooFarAwayThisFrame = false;
			Settings.LockMissileUseThisFrame = false;
		}

		public static void RunSmartAIFuncs_SecondaryOnly(List<SmartAiV2> Merryweather, SmartAI_Settings Settings)
		{
			if (Settings.MerryweatherSpawnCheck < Game.GameTime)
			{
				Settings.MerryweatherSpawnCheck = Game.GameTime + 2000;
				for (int i = 0; i < Merryweather.Count; i++)
				{
					SmartAiV2 smartAiV = Merryweather[i];
					if (smartAiV == null)
					{
						continue;
					}
					SmartAiV2 smartAiV2 = smartAiV;
					if (smartAiV2.GroupActive && !smartAiV2.isDead)
					{
						if (smartAiV.LastVehicle != null)
						{
							if (smartAiV.LastVehicle.AttachedBlip == null)
							{
								smartAiV.LastVehicle.AddBlip();
								if (smartAiV.LastVehicle.AttachedBlip != null)
								{
									smartAiV.LastVehicle.AttachedBlip.Sprite = GetBlipSpriteForVehicle(smartAiV.LastVehicle);
									smartAiV.LastVehicle.AttachedBlip.Color = Settings.AIBlipColor;
									smartAiV.LastVehicle.AttachedBlip.Name = GetStringNameFromEnemyGroup(smartAiV.EnemyGroupName) + "'s " + smartAiV.LastVehicle.LocalizedName;
									smartAiV.LastVehicle.AttachedBlip.IsShortRange = true;
								}
							}
							int num = Function.Call<int>(Hash.GET_VEHICLE_NUMBER_OF_PASSENGERS, smartAiV.LastVehicle, true, false);
							if (num <= 0 || !smartAiV.LastVehicle.IsAlive)
							{
								if (SmartAI_SpawnedVehicles.Contains(smartAiV.LastVehicle))
								{
									SmartAI_SpawnedVehicles.Remove(smartAiV.LastVehicle);
								}
								if (smartAiV.LastVehicle.AttachedBlip != null && smartAiV.LastVehicle.AttachedBlip.Alpha == 255)
								{
									smartAiV.LastVehicle.AttachedBlip.Alpha = 0;
									if (smartAiV.LastVehicle.Model.IsHelicopter)
									{
										Settings.SmartAI_SpawnedVehicles--;
										Settings.SmartAI_SpawnedHelis--;
									}
									else if (smartAiV.LastVehicle.Model.IsPlane)
									{
										Settings.SmartAI_SpawnedVehicles--;
										Settings.SmartAI_SpawnedPlanes--;
									}
									else if (smartAiV.LastVehicle.Model.IsBoat)
									{
										Settings.SmartAI_SpawnedVehicles--;
										Settings.SmartAI_SpawnedBoats--;
									}
									else
									{
										Settings.SmartAI_SpawnedVehicles--;
										Settings.SmartAI_SpawnedCars--;
									}
								}
							}
						}
						if (GetDistance_VDIST2(Settings.AI_Target.Position, smartAiV2.SpawnGroupArea, ">", smartAiV2.DistToSpawn))
						{
							smartAiV2.GroupActive = false;
							SpawnGroupsUseActive.Remove(smartAiV2);
							if (smartAiV2.AIPed != null)
							{
								smartAiV2.AIPed.Delete();
								if (AiBackup.Contains(smartAiV2.AIPed))
								{
									AiBackup.Remove(smartAiV2.AIPed);
								}
							}
							if (smartAiV2.AiProp != null)
							{
								smartAiV2.AiProp.Delete();
							}
							if (smartAiV2.LastVehicle != null && Game.Player.Character.CurrentVehicle != smartAiV2.LastVehicle)
							{
								if (SmartAI_SpawnedVehicles.Contains(smartAiV.LastVehicle))
								{
									SmartAI_SpawnedVehicles.Remove(smartAiV.LastVehicle);
								}
								if (smartAiV.LastVehicle.AttachedBlip != null && smartAiV.LastVehicle.AttachedBlip.Alpha == 255)
								{
									smartAiV.LastVehicle.AttachedBlip.Alpha = 0;
									if (smartAiV.LastVehicle.Model.IsHelicopter)
									{
										Settings.SmartAI_SpawnedVehicles--;
										Settings.SmartAI_SpawnedHelis--;
									}
									else if (smartAiV.LastVehicle.Model.IsPlane)
									{
										Settings.SmartAI_SpawnedVehicles--;
										Settings.SmartAI_SpawnedPlanes--;
									}
									else if (smartAiV.LastVehicle.Model.IsBoat)
									{
										Settings.SmartAI_SpawnedVehicles--;
										Settings.SmartAI_SpawnedBoats--;
									}
									else
									{
										Settings.SmartAI_SpawnedVehicles--;
										Settings.SmartAI_SpawnedCars--;
									}
								}
								smartAiV2.LastVehicle.Delete();
							}
						}
					}
					if (!Settings.AllowDespawnIfTooFarAwayThisFrame || !GetDistance_VDIST2(Game.Player.Character.Position, smartAiV.AIPed.Position, ">", Settings.TooFarAwayDist))
					{
						continue;
					}
					if (smartAiV.LastVehicle.AttachedBlip != null && smartAiV.LastVehicle.AttachedBlip.Alpha == 255)
					{
						if (SmartAI_SpawnedVehicles.Contains(smartAiV.LastVehicle))
						{
							SmartAI_SpawnedVehicles.Remove(smartAiV.LastVehicle);
						}
						smartAiV.LastVehicle.AttachedBlip.Alpha = 0;
						if (smartAiV.LastVehicle.Model.IsHelicopter)
						{
							Settings.SmartAI_SpawnedVehicles--;
							Settings.SmartAI_SpawnedHelis--;
						}
						else if (smartAiV.LastVehicle.Model.IsPlane)
						{
							Settings.SmartAI_SpawnedVehicles--;
							Settings.SmartAI_SpawnedPlanes--;
						}
						else if (smartAiV.LastVehicle.Model.IsBoat)
						{
							Settings.SmartAI_SpawnedVehicles--;
							Settings.SmartAI_SpawnedBoats--;
						}
						else
						{
							Settings.SmartAI_SpawnedVehicles--;
							Settings.SmartAI_SpawnedCars--;
						}
					}
					if (smartAiV.LastVehicle != null)
					{
						smartAiV.LastVehicle.Delete();
					}
					if (smartAiV.AIPed != null)
					{
						smartAiV.AIPed.Delete();
						if (AiBackup.Contains(smartAiV2.AIPed))
						{
							AiBackup.Remove(smartAiV2.AIPed);
						}
					}
					if (smartAiV.AiProp != null)
					{
						smartAiV.AiProp.Delete();
					}
				}
			}
			if (Settings.BlipDeleteCheck < Game.GameTime)
			{
				Settings.BlipDeleteCheck = Game.GameTime + 150;
				for (int j = 0; j < Merryweather.Count; j++)
				{
					SmartAiV2 smartAiV3 = Merryweather[j];
					if (smartAiV3 == null || !(smartAiV3.AIPed != null) || !smartAiV3.GroupActive || !smartAiV3.AIPed.Exists() || smartAiV3.SpawnSafeTimer >= Game.GameTime || !smartAiV3.AIPed.IsDead)
					{
						continue;
					}
					smartAiV3.isDead = true;
					if (smartAiV3.LastVehicle != null)
					{
						if (smartAiV3.LastVehicle.AttachedBlip != null)
						{
							smartAiV3.LastVehicle.AttachedBlip.Delete();
						}
						if (!EntitiesToDeletePostMission.Contains(smartAiV3.LastVehicle))
						{
							EntitiesToDeletePostMission.Add(smartAiV3.LastVehicle);
						}
					}
					if (smartAiV3.AIPed.AttachedBlip != null)
					{
						Function.Call(Hash.SET_BLIP_SHOW_CONE, smartAiV3.AIPed.AttachedBlip.Handle, false, 11);
						if (smartAiV3.AIPed.AttachedBlip != null)
						{
							smartAiV3.AIPed.AttachedBlip.Delete();
						}
						if (!EntitiesToDeletePostMission.Contains(smartAiV3.AIPed))
						{
							EntitiesToDeletePostMission.Add(smartAiV3.AIPed);
						}
						if (AiBackup.Contains(smartAiV3.AIPed))
						{
							AiBackup.Remove(smartAiV3.AIPed);
						}
						Merryweather.RemoveAt(j);
					}
				}
			}
			Settings.AllowDespawnIfTooFarAwayThisFrame = false;
			Settings.LockMissileUseThisFrame = false;
		}
	}
}
