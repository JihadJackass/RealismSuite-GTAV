using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using SmartAIGuards;

namespace SinglePlayerHeists
{
	public class DiamdondCasinoSmartGuards : Script
	{
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

		public static List<MissionOjects> DiamondCasinoNav = new List<MissionOjects>
		{
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 741681, 1803116220u, new Vector3(2513.95459f, -268.994568f, -59.39349f), new Vector3(0f, 0f, -74.24148f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 713522, 1803116220u, new Vector3(2518.38281f, -269.2346f, -59.39349f), new Vector3(0f, 0f, -87.59951f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 203052, 1803116220u, new Vector3(2530.4292f, -269.1597f, -59.39349f), new Vector3(0f, 0f, -88.92997f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 13358, 1803116220u, new Vector3(2541.029f, -268.394f, -59.39349f), new Vector3(0f, -0f, 169.987427f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 172594, 1803116220u, new Vector3(2541.368f, -287.597076f, -59.39349f), new Vector3(0f, -0f, 93.26613f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 73011, 1803116220u, new Vector3(2541.05469f, -268.174927f, -59.39349f), new Vector3(0f, -0f, 94.6475143f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 176436, 1803116220u, new Vector3(2530.38062f, -268.9688f, -59.39349f), new Vector3(0f, -0f, 94.44225f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 143415, 1803116220u, new Vector3(2514.10083f, -269.05423f, -59.39349f), new Vector3(0f, 0f, 79.65055f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 630321, 3200789669u, new Vector3(2513.19531f, -268.8413f, -58.71781f), new Vector3(0f, -0f, -95.2271652f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 489264, 1803116220u, new Vector3(2522.70361f, -275.0871f, -65.39331f), new Vector3(0f, 0f, 4.21057463f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 120887, 1803116220u, new Vector3(2533.5603f, -274.724854f, -65.39331f), new Vector3(0f, 0f, -1.5728904f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 206135, 1803116220u, new Vector3(2546.09424f, -275.069031f, -65.39351f), new Vector3(0f, 0f, -1.5728904f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 492080, 1803116220u, new Vector3(2533.836f, -274.704956f, -65.39331f), new Vector3(0f, 0f, -0.498087049f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 619321, 1803116220u, new Vector3(2522.2207f, -274.840424f, -65.39331f), new Vector3(0f, 0f, 0.7814389f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 406330, 3200789669u, new Vector3(2521.22119f, -274.813324f, -64.72706f), new Vector3(0f, 0f, -88.32469f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 619834, 1803116220u, new Vector3(2538.03076f, -283.675629f, -65.39331f), new Vector3(0f, -0f, -179.521439f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 178491, 1803116220u, new Vector3(2522.05347f, -283.9418f, -65.39331f), new Vector3(0f, -0f, 178.942429f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 97852, 1803116220u, new Vector3(2538.40625f, -283.780334f, -65.39331f), new Vector3(0f, -0f, 177.8679f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 554814, 1803116220u, new Vector3(2545.21167f, -283.771332f, -65.39331f), new Vector3(0f, -0f, -177.423065f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 111166, 3200789669u, new Vector3(2546.456f, -283.503418f, -64.72779f), new Vector3(0f, -0f, 91.98595f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 226625, 1803116220u, new Vector3(2508.74072f, -275.8255f, -59.39349f), new Vector3(0f, -0f, -160.9436f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 720706, 1803116220u, new Vector3(2521.431f, -275.985565f, -59.39349f), new Vector3(0f, -0f, -91.18337f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 721723, 1803116220u, new Vector3(2521.752f, -282.822845f, -59.39349f), new Vector3(0f, -0f, 108.568062f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 78145, 1803116220u, new Vector3(2512.97681f, -283.4017f, -59.39349f), new Vector3(0f, -0f, 100.480591f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 263488, 1803116220u, new Vector3(2521.74976f, -282.5089f, -59.39349f), new Vector3(0f, -0f, 107.440613f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 278596, 1803116220u, new Vector3(2521.52759f, -275.70285f, -59.39349f), new Vector3(0f, 0f, 86.81427f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 190015, 1803116220u, new Vector3(2508.98413f, -275.8247f, -59.39349f), new Vector3(0f, 0f, 89.52644f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 699192, 3200789669u, new Vector3(2507.735f, -275.8027f, -58.7196426f), new Vector3(0f, 0f, -89.4967041f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 359773, 1803116220u, new Vector3(2512.24756f, -258.372528f, -55.7936668f), new Vector3(0f, -0f, -108.115807f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 122972, 1803116220u, new Vector3(2520.554f, -258.464966f, -55.7936668f), new Vector3(0f, -0f, -95.01304f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 74842, 1803116220u, new Vector3(2512.33154f, -258.20343f, -55.7936668f), new Vector3(0f, -0f, -104.327667f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 360796, 1803116220u, new Vector3(2506.26465f, -258.0347f, -55.7936668f), new Vector3(0f, -0f, 99.05795f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 18775, 3200789669u, new Vector3(2506.50562f, -258.065369f, -54.7605629f), new Vector3(-3.20165071E-07f, -2.66804445E-08f, -86.83184f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 26155, 1803116220u, new Vector3(2549.76782f, -228.862808f, -60.9935379f), new Vector3(0f, -0f, 178.228317f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 380212, 1803116220u, new Vector3(2549.79761f, -244.891983f, -61.03712f), new Vector3(0f, -0f, 179.302765f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 7986, 1803116220u, new Vector3(2549.68848f, -214.579285f, -60.99345f), new Vector3(0f, 0f, 6.515016f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 666419, 3200789669u, new Vector3(2550.04077f, -214.154892f, -60.328f), new Vector3(0f, -0f, 176.8454f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 620600, 1803116220u, new Vector3(2513.48f, -291.157043f, -59.39349f), new Vector3(0f, -0f, -93.28154f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 414519, 1803116220u, new Vector3(2532.00952f, -291.222351f, -59.39349f), new Vector3(0f, -0f, -90.87577f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 662393, 1803116220u, new Vector3(2541.86572f, -290.869751f, -59.39349f), new Vector3(0f, 0f, -85.60393f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 226872, 1803116220u, new Vector3(2531.781f, -291.1789f, -59.39349f), new Vector3(0f, 0f, -65.2845459f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 227196, 1803116220u, new Vector3(2513.63745f, -291.232025f, -59.39349f), new Vector3(0f, -0f, 102.281944f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 379980, 1803116220u, new Vector3(2501.15112f, -291.176361f, -59.39349f), new Vector3(0f, 0f, 88.2068253f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 88126, 3200789669u, new Vector3(2500.598f, -291.283447f, -58.7180252f), new Vector3(0f, 0f, -86.3199844f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 187205, 1803116220u, new Vector3(2490.65771f, -260.705322f, -71.36473f), new Vector3(0f, -0f, -174.037766f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 780359, 1803116220u, new Vector3(2490.89771f, -276.0177f, -71.36474f), new Vector3(0f, -0f, -178.643173f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 348993, 1803116220u, new Vector3(2490.52661f, -288.626923f, -71.36474f), new Vector3(0f, -0f, 178.183472f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 755523, 1803116220u, new Vector3(2486.79126f, -288.522552f, -71.36474f), new Vector3(0f, -0f, 110.265511f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 547913, 1803116220u, new Vector3(2490.57739f, -288.4271f, -71.36474f), new Vector3(0f, -0f, -150.39183f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 518217, 1803116220u, new Vector3(2490.77539f, -276.240967f, -71.36474f), new Vector3(0f, 0f, -4.16721535f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 451652, 1803116220u, new Vector3(2490.65234f, -261.049835f, -71.36473f), new Vector3(0f, 0f, -0.7892465f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 616780, 1803116220u, new Vector3(2490.67041f, -252.187332f, -71.36474f), new Vector3(0f, -0f, -178.489777f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 764745, 3200789669u, new Vector3(2490.65381f, -251.245911f, -70.68958f), new Vector3(0f, -0f, -178.13118f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 185679, 1803116220u, new Vector3(2477.4f, -272.96347f, -71.36468f), new Vector3(0f, -0f, 96.59863f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 599111, 1803116220u, new Vector3(2477.007f, -262.637054f, -71.33156f), new Vector3(0f, -0f, 90.14941f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 575822, 1803116220u, new Vector3(2477.46143f, -273.174164f, -71.14237f), new Vector3(0f, 0f, 88.86959f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 306448, 1803116220u, new Vector3(2476.989f, -283.6998f, -71.36468f), new Vector3(0f, -0f, 108.317566f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 300369, 3200789669u, new Vector3(2477.16577f, -284.3739f, -70.69985f), new Vector3(0f, 0f, 1.89369977f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 271455, 1803116220u, new Vector3(2488.133f, -257.619751f, -39.7933731f), new Vector3(0f, -0f, 164.5495f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 170838, 1803116220u, new Vector3(2488.2937f, -289.50116f, -39.7934227f), new Vector3(0f, -0f, -177.43428f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 295523, 1803116220u, new Vector3(2523.407f, -289.812317f, -39.7934227f), new Vector3(0f, 0f, -73.7399445f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 713761, 1803116220u, new Vector3(2523.22656f, -257.876434f, -39.79344f), new Vector3(0f, 0f, 5.38586235f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 670240, 1803116220u, new Vector3(2515.26978f, -258.520935f, -39.7934036f), new Vector3(0f, -0f, 90.90915f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 101220, 1803116220u, new Vector3(2503.28442f, -258.030182f, -39.79368f), new Vector3(0f, 0f, 88.19611f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 507493, 1803116220u, new Vector3(2487.90869f, -257.31012f, -39.7933731f), new Vector3(0f, 0f, 87.88862f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 378716, 3200789669u, new Vector3(2488.57f, -256.762f, -39.1193657f), new Vector3(0f, -0f, -179.268051f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 641897, 1803116220u, new Vector3(2523.453f, -258.617218f, -39.79344f), new Vector3(0f, 0f, 0.789853334f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 761191, 1803116220u, new Vector3(2523.361f, -232.532776f, -39.79339f), new Vector3(0f, 0f, 0.0221354738f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 468841, 1803116220u, new Vector3(2503.83f, -233.5578f, -39.7933731f), new Vector3(0f, -0f, 102.946945f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 279657, 1803116220u, new Vector3(2503.673f, -257.554016f, -39.79368f), new Vector3(0f, -0f, 178.847885f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 313707, 1803116220u, new Vector3(2488.31543f, -257.72403f, -39.7933731f), new Vector3(0f, -0f, 93.16996f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 615977, 1803116220u, new Vector3(2488.25f, -289.494781f, -39.7934227f), new Vector3(0f, -0f, -179.566681f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 37166, 1803116220u, new Vector3(2523.25269f, -289.718719f, -39.7934227f), new Vector3(0f, 0f, -85.3412247f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 82284, 3200789669u, new Vector3(2523.34546f, -290.349884f, -39.1189537f), new Vector3(0f, 0f, 1.10452545f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 558718, 3200789669u, new Vector3(2537.40356f, -280.893341f, -58.71817f), new Vector3(0f, 0f, -74.12705f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 126282, 3200789669u, new Vector3(2501.38f, -273.26828f, -58.7234268f), new Vector3(0f, 0f, -89.88917f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 32523, 3200789669u, new Vector3(2503.56543f, -233.786774f, -55.1184921f), new Vector3(0f, 0f, -88.20022f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 29708, 3200789669u, new Vector3(2483.1145f, -273.448273f, -70.6997757f), new Vector3(0f, -0f, 99.15952f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 267276, 3200789669u, new Vector3(2472.028f, -270.541077f, -70.69933f), new Vector3(0f, -0f, -170.506271f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 640016, 3200789669u, new Vector3(2501.41455f, -277.2771f, -70.6904449f), new Vector3(0f, 0f, -70.21979f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 538896, 3200789669u, new Vector3(2520.83887f, -280.397339f, -64.72855f), new Vector3(-0.00600631442f, -1.88574049E-05f, -90.1792755f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 342048, 3200789669u, new Vector3(2533.7605f, -276.075226f, -58.7185135f), new Vector3(0f, 0f, -3.98133659f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 449319, 3200789669u, new Vector3(2556.561f, -284.767059f, -64.72825f), new Vector3(0f, 0f, -0.705678165f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 239648, 3200789669u, new Vector3(2552.94165f, -288.320526f, -64.7184448f), new Vector3(0f, -0f, -166.429291f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 97579, 3200789669u, new Vector3(2549.51782f, -290.615875f, -64.7178345f), new Vector3(0f, 0f, -77.01572f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 179497, 3200789669u, new Vector3(2502.016f, -279.7679f, -58.71828f), new Vector3(0f, 0f, -41.4911575f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 45104, 3200789669u, new Vector3(2533.14038f, -296.444244f, -58.7177277f), new Vector3(4.26805428E-07f, 3.13584678E-07f, 78.91628f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 591678, 3200789669u, new Vector3(2519.80273f, -271.245f, -39.11879f), new Vector3(0f, 0f, 37.40926f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 679740, 1803116220u, new Vector3(2514.83618f, -258.369019f, -39.7934036f), new Vector3(0f, -0f, -178.608322f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 216129, 1803116220u, new Vector3(2503.87329f, -258.0592f, -39.79368f), new Vector3(0f, -0f, 179.241837f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 544067, 1803116220u, new Vector3(2487.763f, -257.651672f, -39.79338f), new Vector3(0f, -0f, 174.840149f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 521794, 1803116220u, new Vector3(2503.87256f, -257.8403f, -39.79368f), new Vector3(0f, -0f, 141.776886f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 137740, 1803116220u, new Vector3(2515.08813f, -258.293579f, -39.7934036f), new Vector3(0f, -0f, 155.595764f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 400394, 1803116220u, new Vector3(2523.082f, -258.373138f, -39.79344f), new Vector3(0f, -0f, -176.970764f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 332619, 3200789669u, new Vector3(2524.55127f, -258.633484f, -39.11861f), new Vector3(0f, 0f, 86.1942f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 738635, 3200789669u, new Vector3(2512.13379f, -259.560059f, -55.1184959f), new Vector3(0f, -0f, -109.7198f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 698702, 3200789669u, new Vector3(2532.80322f, -272.245667f, -58.7177f), new Vector3(0f, 0f, 43.81716f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 667427, 1803116220u, new Vector3(2536.29565f, -287.022522f, -59.39349f), new Vector3(0f, 0f, -87.8140945f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 434784, 1803116220u, new Vector3(2541.57739f, -286.999756f, -59.39349f), new Vector3(0f, -0f, -91.2941055f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 178785, 1803116220u, new Vector3(2540.9585f, -268.51004f, -59.39349f), new Vector3(0f, 0f, 0.524877846f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 334435, 1803116220u, new Vector3(2530.96143f, -269.0782f, -59.39349f), new Vector3(0f, -0f, 92.39457f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 586591, 1803116220u, new Vector3(2518.37061f, -269.142029f, -59.39349f), new Vector3(0f, -0f, 91.83132f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 143718, 1803116220u, new Vector3(2514.34424f, -269.029633f, -59.39349f), new Vector3(0f, -0f, 92.24056f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 717155, 1803116220u, new Vector3(2518.5354f, -269.281647f, -59.32003f), new Vector3(0f, 0f, 86.75534f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 569957, 1803116220u, new Vector3(2530.23071f, -269.129669f, -59.2583542f), new Vector3(0f, 0f, -83.7721252f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 496741, 1803116220u, new Vector3(2540.84644f, -268.3347f, -59.27556f), new Vector3(0f, 0f, -85.10278f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 655969, 1803116220u, new Vector3(2541.313f, -286.976044f, -59.39349f), new Vector3(0f, -0f, 178.4722f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 100140, 1803116220u, new Vector3(2536.70215f, -286.9229f, -59.39349f), new Vector3(0f, -0f, 98.7314f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 717410, 1803116220u, new Vector3(2530.38257f, -287.2545f, -59.39349f), new Vector3(0f, -0f, 102.9279f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 769384, 3200789669u, new Vector3(2529.96338f, -287.280975f, -58.71797f), new Vector3(0f, 0f, -86.22932f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 692591, 1803116220u, new Vector3(2522.70117f, -283.539276f, -71.38149f), new Vector3(0f, -0f, 90.5482559f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 557936, 1803116220u, new Vector3(2503.73071f, -283.5578f, -71.37925f), new Vector3(0f, 0f, 89.16586f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 490603, 1803116220u, new Vector3(2503.60669f, -275.726685f, -71.37854f), new Vector3(0f, 0f, 33.9925156f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 41328, 1803116220u, new Vector3(2522.16577f, -274.9278f, -71.38292f), new Vector3(0f, -0f, 177.2409f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 594033, 1803116220u, new Vector3(2522.62964f, -283.300079f, -71.38267f), new Vector3(0f, -0f, 171.8158f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 718196, 3200789669u, new Vector3(2522.86084f, -285.198273f, -70.69546f), new Vector3(0f, 0f, 1.17777872f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 84856, 1803116220u, new Vector3(2487.34058f, -272.816f, -71.36474f), new Vector3(0f, 0f, -16.8388062f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 552814, 1803116220u, new Vector3(2487.74634f, -275.233856f, -71.36474f), new Vector3(0f, 0f, -47.1891136f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 214840, 1803116220u, new Vector3(2481.938f, -275.202179f, -71.36468f), new Vector3(0f, -0f, 94.32626f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 675692, 1803116220u, new Vector3(2487.275f, -275.17392f, -71.36474f), new Vector3(0f, -0f, 102.054192f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 155512, 1803116220u, new Vector3(2487.19336f, -273.017731f, -71.36474f), new Vector3(0f, 0f, 46.2154427f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 445047, 3200789669u, new Vector3(2485.7312f, -273.663116f, -70.68879f), new Vector3(0f, 0f, -79.22946f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			}),
			new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 603516, 1803116220u, new Vector3(2490.71216f, -270.1472f, -71.36473f), new Vector3(0f, -0f, -179.587921f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 79107, 1803116220u, new Vector3(2475.77441f, -269.6485f, -71.36471f), new Vector3(0f, 0f, 89.51405f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"), 60418, 1803116220u, new Vector3(2490.63428f, -269.94223f, -71.36473f), new Vector3(0f, -0f, 171.147217f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Duggan Security 2"), 687482, 3200789669u, new Vector3(2492.30566f, -269.835938f, -70.6997452f), new Vector3(0f, -0f, 91.3064041f), 2725352035u, 200, 0, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
			})
		};

		public static List<CayoPericoSpawnGroup> SpawnGroups = new List<CayoPericoSpawnGroup>();

		public static List<Vehicle> MerryweatherVehicles = new List<Vehicle>();

		public static List<Ped> MerryweatherPeds = new List<Ped>();

		public static int Mod_Difficulty = 50;

		public static List<SmartGuards.SmartAiV2> SpawnGroupsUse = new List<SmartGuards.SmartAiV2>();

		public static List<SmartGuards.SmartAiV2> SpawnGroupsUseActive = new List<SmartGuards.SmartAiV2>();

		private Vector2 World3DToScreen2d(Vector3 pos)
		{
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, pos.X, pos.Y, pos.Z, outputArgument, outputArgument2);
			return new Vector2(outputArgument.GetResult<float>(), outputArgument2.GetResult<float>());
		}

		private void drawText(string text, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		public DiamdondCasinoSmartGuards()
		{
			base.Tick += OnTick;
			List<SmartGuards.SmartAiV2> list = new List<SmartGuards.SmartAiV2>();
			foreach (MissionOjects item in DiamondCasinoNav)
			{
				List<Vector3> list2 = new List<Vector3>();
				for (int i = 0; i < item.Entities.Count; i++)
				{
					ObjectPlacementMap objectPlacementMap = item.Entities[i];
					if (objectPlacementMap.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"))
					{
						list2.Add(objectPlacementMap.Position);
					}
					if (objectPlacementMap.Hash != Function.Call<int>(Hash.GET_HASH_KEY, "prop_alien_egg_01"))
					{
						if (list2.Count > 0)
						{
							SmartGuards.PedTaskLoop wL = new SmartGuards.PedTaskLoop(list2, 0, true);
							list.Add(new SmartGuards.SmartAiV2(0, "s_m_y_westsec_02", objectPlacementMap.Position, objectPlacementMap.Rotation.Z, true, 200, 100, 1, wL, 40, 50, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Diamond_Casino_Security));
							SpawnGroups.Add(new CayoPericoSpawnGroup(objectPlacementMap.Position, 100f, false, list));
						}
						else
						{
							SmartGuards.PedTaskLoop pedTaskLoop = new SmartGuards.PedTaskLoop(list2, 0, true);
							list.Add(new SmartGuards.SmartAiV2(0, "s_m_y_westsec_02", objectPlacementMap.Position, objectPlacementMap.Rotation.Z, true, 200, 100, 1, 40f, 50f, WeaponHash.MilitaryRifle, SmartGuards.EnemyGroup.Diamond_Casino_Security));
							SpawnGroups.Add(new CayoPericoSpawnGroup(objectPlacementMap.Position, 100f, false, list));
						}
					}
				}
			}
			foreach (SmartGuards.SmartAiV2 item2 in list)
			{
				int index = SpawnGroups.Count - 1;
				item2.GroupActive = SpawnGroups[index].GroupActive;
				item2.DistToSpawn = SpawnGroups[index].DistToSpawn;
				item2.SpawnGroupArea = item2.SpawnLocation;
				SpawnGroupsUse.Add(item2);
			}
		}

		public void OnTick(object sender, EventArgs e)
		{
			if ((!TheDiamondHeist.MissionOn && TheDiamondHeist.HeistStage != 4) || !TheDiamondHeist.MissionOn)
			{
				return;
			}
			if (!TheDiamondHeist.TriggerFullResponce)
			{
				SmartGuards.GetCurrentGroupSettings().ShowCone = true;
				SmartGuards.GetCurrentGroupSettings().ShowBlipRadius = 100f;
				SmartGuards.GetCurrentGroupSettings().Radius = 40f;
				SmartGuards.GetCurrentGroupSettings().SeeingAngle = 25f;
				SmartGuards.GetCurrentGroupSettings().AllowInvestigate = true;
				SmartGuards.GetCurrentGroupSettings().InvestigateDist = 20f;
				SmartGuards.GetCurrentGroupSettings().HitRadius = 30f;
				SmartGuards.GetCurrentGroupSettings().ArrestInsteadOfShoot = false;
				SmartGuards.GetCurrentGroupSettings().IngorePlayer = true;
				if (TheDiamondHeist.EnteranceUsed == 1 && TheDiamondHeist.CurrentFloor != TheDiamondHeist.FloorOn.Vault && TheDiamondHeist.CurrentFloor != 0 && TheDiamondHeist.CurrentFloor != TheDiamondHeist.FloorOn.Offices)
				{
					SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
				}
				if (TheDiamondHeist.EnteranceUsed == 2)
				{
					SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
				}
				if (TheDiamondHeist.EnteranceUsed == 3)
				{
					SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
				}
				if (TheDiamondHeist.EnteranceUsed == 4)
				{
					SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
				}
				if (TheDiamondHeist.EnteranceUsed == 5 && TheDiamondHeist.ExitUsed == 4 && TheDiamondHeist.CurrentFloor != TheDiamondHeist.FloorOn.Vault && TheDiamondHeist.CurrentFloor != 0)
				{
					SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
				}
				if (TheDiamondHeist.EnteranceUsed == 6)
				{
					SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
				}
				if (TheDiamondHeist.EnteranceUsed == 7)
				{
					SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
				}
				if (TheDiamondHeist.EnteranceUsed == 8)
				{
					SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
				}
				if (Game.Player.WantedLevel >= 2)
				{
					TheDiamondHeist.PoliceAlearted = true;
				}
				if (TheDiamondHeist.PoliceAlearted)
				{
					SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
				}
				SmartGuards.GetCurrentGroupSettings().ShowBlipHeightAdd = 5f;
				SmartGuards.GetCurrentGroupSettings().ShowBlipHeightMinus = 5f;
			}
			if (TheDiamondHeist.TriggerFullResponce)
			{
				SmartGuards.GetCurrentGroupSettings().ShowCone = true;
				if (TheDiamondHeist.IsInterior)
				{
					SmartGuards.GetCurrentGroupSettings().ShowBlipRadius = 100f;
					SmartGuards.GetCurrentGroupSettings().Radius = 50f;
					SmartGuards.GetCurrentGroupSettings().SeeingAngle = 45f;
				}
				else
				{
					SmartGuards.GetCurrentGroupSettings().ShowBlipRadius = 200f;
					SmartGuards.GetCurrentGroupSettings().Radius = 300f;
					SmartGuards.GetCurrentGroupSettings().SeeingAngle = 25f;
				}
				SmartGuards.GetCurrentGroupSettings().AllowInvestigate = true;
				SmartGuards.GetCurrentGroupSettings().InvestigateDist = 40f;
				SmartGuards.GetCurrentGroupSettings().HitRadius = 30f;
				SmartGuards.GetCurrentGroupSettings().ArrestInsteadOfShoot = false;
				SmartGuards.GetCurrentGroupSettings().ShowBlipHeightAdd = 15f;
				SmartGuards.GetCurrentGroupSettings().ShowBlipHeightMinus = 15f;
				SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
			}
		}

		public static void RemoveAIGuards()
		{
			foreach (TheDiamondHeist.ObjectPlacementMap bugStarVent in TheDiamondHeist.BugStarVents)
			{
				if (bugStarVent.Entity != null)
				{
					bugStarVent.Entity.Delete();
				}
			}
			foreach (TheDiamondHeist.ObjectPlacementMap groupe6Cam in TheDiamondHeist.Groupe6Cams)
			{
				if (groupe6Cam.Entity != null)
				{
					groupe6Cam.Entity.Delete();
				}
			}
			foreach (SmartGuards.SmartAiV2 item in SpawnGroupsUse)
			{
				if (item.AIPed != null)
				{
					item.AIPed.Delete();
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

		public float GetDistance(Vector3 _1, Vector3 _2)
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

		public static void ATTACH_ENTITY_TO_ENTITY(Entity entity1, Entity entity2, int boneIndex, float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, bool p9, bool useSoftPinning, bool collision, bool isPed, int vertexIndex, bool fixedRot)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, entity1, entity2, boneIndex, xPos, yPos, zPos, xRot, yRot, zRot, p9, useSoftPinning, collision, isPed, vertexIndex, fixedRot);
		}
	}
}
