using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using CayoPericoHeistInSP;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using HKHModHelperNew;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Menus;
using SinglePlayerHeists.Freeform;
using SinglePlayerHeists.HitNRun;
using SinglePlayerHeists.UtilsMisc;

namespace SinglePlayerHeists
{
	public class Main : Script
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

		public enum HeistID
		{
			None = -1,
			The247Job,
			TheSilthouseJob,
			TheStashHouse,
			Fleeca,
			Paleto,
			JewelStore,
			PacificStandard,
			UnionDepository,
			LaFuenteBlanca,
			BikerClubhouse,
			ThePrisonBreak,
			TheHumaineLabs,
			LaoubPrincessCargoshipHeist,
			TheDiamondCasino,
			CayoPericoHeist,
			TheBillionairesParty,
			TheLuxigntonRaid,
			TheCartelYachtHeist,
			TheFederalReserveTrainHeist,
			TheBelkraneJob,
			TheMazeBankSavingsBankHeist,
			ThePrinthouseHeist,
			TheGefangnisArtHeist,
			TheBettaBankHeist,
			TheDiamondJewelStoreHeist
		}

		public class VariableTimer
		{
			public delegate void TimerExpired(object sender);

			private int TimerMax;

			private decimal TimerCounter;

			private bool IsRunning;

			public bool AutoReset;

			public int Counter => (int)TimerCounter;

			public event TimerExpired OnTimerExpired;

			public VariableTimer(int interval)
			{
				TimerCounter = interval;
				TimerMax = interval;
			}

			public void AddTime(decimal amount)
			{
				TimerCounter += amount;
			}

			public void RemoveTime(decimal amount)
			{
				TimerCounter -= amount;
				if (TimerCounter < 0m)
				{
					TimerCounter = default(decimal);
				}
			}

			public void Update(float timescale)
			{
				if (!IsRunning)
				{
					return;
				}
				float num = Game.LastFrameTime * 1000f;
				TimerCounter -= (decimal)(num * timescale);
				if (TimerCounter <= 0m)
				{
					this.OnTimerExpired?.Invoke(this);
					if (AutoReset)
					{
						TimerCounter += (decimal)TimerMax;
					}
					else
					{
						Stop();
					}
				}
			}

			public void Stop()
			{
				IsRunning = false;
			}

			public void Start()
			{
				IsRunning = true;
			}

			public void Reset()
			{
				TimerCounter = TimerMax;
			}
		}

		public enum LandVehicles
		{
			Compacts = 0,
			Sedans = 1,
			SUVs = 2,
			Coupes = 3,
			Muscle = 4,
			Sports_Classics = 5,
			Sports = 6,
			Super = 7,
			Motorcycles = 8,
			Off_Road = 9,
			Industrial = 10,
			Utility = 11,
			Vans = 12,
			Military = 19,
			Commercial = 20
		}

		public class SecCabinetProps : Script
		{
			public int SelectedIndex { get; set; }

			public Prop GlassDisplayCase { get; set; }

			public List<Prop> Props { get; set; }

			public SecCabinetProps()
			{
			}

			public SecCabinetProps(List<Prop> P)
			{
				Props = P;
			}
		}

		public class TrafficLightRoute : Script
		{
			public List<Vector2> Points { get; set; }

			public float RotationOnEnd { get; set; }

			public TrafficLightRoute()
			{
			}

			public TrafficLightRoute(List<Vector2> P, float R)
			{
				Points = P;
				RotationOnEnd = R;
			}
		}

		public class HeistData
		{
			public string HeistName { get; set; }

			public int AveragePay { get; set; }

			public List<string> Objectives { get; set; }

			public int Players { get; set; }

			public List<Vector2> IntelPoints { get; set; }

			public HeistID Heist { get; set; }

			public HeistData()
			{
			}

			public HeistData(HeistID hes, string H, float A, List<string> O, int P, List<Vector2> IP)
			{
				Heist = hes;
				IntelPoints = IP;
				HeistName = H;
				AveragePay = (int)A;
				Objectives = O;
				Players = P;
			}
		}

		public CargoShipHeist.MissionOjects ArcadeObjects_v2944 = new CargoShipHeist.MissionOjects(new List<CargoShipHeist.ObjectPlacementMap>
		{
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_safe_01a"), 403810, 1098122770u, new Vector3(2680.91577f, -364.7354f, -56.2122421f), new Vector3(-1.59027725E-15f, 9.548874E-07f, -179.900024f), 63580, 0, new Vector3(1.70000017f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_safe_01a"), 780896, 3616325110u, new Vector3(2679.21582f, -364.738373f, -56.2122421f), new Vector3(-1.59027725E-15f, 9.548874E-07f, -179.900024f), 403810, 0, new Vector3(1.70000029f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_planning_board_04"), 12295, 2177907976u, new Vector3(2700.78271f, -357.678345f, -54.2916756f), new Vector3(0f, 0f, 0.137797475f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_planning_board_01a"), 328965, 289013745u, new Vector3(2684.77832f, -364.851929f, -56.1856537f), new Vector3(0f, -0f, -179.683792f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 329477, 113622690u, new Vector3(2683.4502f, -364.843384f, -55.5820847f), new Vector3(0f, -0f, -165.239868f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 329733, 113622690u, new Vector3(8414f / MathF.PI, -364.8227f, -55.5820847f), new Vector3(0f, -0f, 167.735687f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_dlc_heist_board"), 329989, 695737472u, new Vector3(2679.22876f, -357.6965f, -54.17447f), new Vector3(0f, -0f, -179.5774f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 330245, 113622690u, new Vector3(2678.79126f, -357.7885f, -54.9159966f), new Vector3(0f, 0f, 61.0718f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_planning_board_01a"), 222729, 289013745u, new Vector3(2687.77832f, -364.835358f, -56.1856537f), new Vector3(0f, -0f, -179.683792f), 328965, 0, new Vector3(-2.99999976f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_planning_board_01a"), 36874, 289013745u, new Vector3(2690.77832f, -364.818817f, -56.1856537f), new Vector3(0f, -0f, -179.683792f), 328965, 0, new Vector3(-5.99999952f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 551177, 304964818u, new Vector3(2708.64551f, -358.169434f, -56.1867f), new Vector3(0f, 0f, -0.8927465f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 108813, 304964818u, new Vector3(2715.22f, -355.234558f, -56.1867f), new Vector3(0f, -0f, -90.9700851f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public CargoShipHeist.MissionOjects ArcadeObjects_v3095 = new CargoShipHeist.MissionOjects(new List<CargoShipHeist.ObjectPlacementMap>
		{
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_safe_01a"), 63580, 1157006711u, new Vector3(2682.61572f, -364.732452f, -56.2122421f), new Vector3(-1.59027725E-15f, 9.548874E-07f, -179.900024f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_safe_01a"), 403810, 1098122770u, new Vector3(2680.91577f, -364.7354f, -56.2122421f), new Vector3(-1.59027725E-15f, 9.548874E-07f, -179.900024f), 63580, 0, new Vector3(1.70000017f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_safe_01a"), 780896, 3616325110u, new Vector3(2679.21582f, -364.738373f, -56.2122421f), new Vector3(-1.59027725E-15f, 9.548874E-07f, -179.900024f), 403810, 0, new Vector3(1.70000029f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_planning_board_04"), 12295, 2177907976u, new Vector3(2700.78271f, -357.678345f, -54.2916756f), new Vector3(0f, 0f, 0.137797475f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_planning_board_01a"), 328965, 289013745u, new Vector3(2684.77832f, -364.851929f, -56.1856537f), new Vector3(0f, -0f, -179.683792f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 329477, 113622690u, new Vector3(2683.4502f, -364.843384f, -55.5820847f), new Vector3(0f, -0f, -165.239868f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 329733, 113622690u, new Vector3(8414f / MathF.PI, -364.8227f, -55.5820847f), new Vector3(0f, -0f, 167.735687f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_dlc_heist_board"), 329989, 695737472u, new Vector3(2679.22876f, -357.6965f, -54.17447f), new Vector3(0f, -0f, -179.5774f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_base_tripod_lampc"), 330245, 113622690u, new Vector3(2678.79126f, -357.7885f, -54.9159966f), new Vector3(0f, 0f, 61.0718f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_planning_board_01a"), 222729, 289013745u, new Vector3(2687.77832f, -364.835358f, -56.1856537f), new Vector3(0f, -0f, -179.683792f), 328965, 0, new Vector3(-2.99999976f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_planning_board_01a"), 36874, 289013745u, new Vector3(2690.77832f, -364.818817f, -56.1856537f), new Vector3(0f, -0f, -179.683792f), 328965, 0, new Vector3(-5.99999952f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 551177, 304964818u, new Vector3(2708.64551f, -358.169434f, -56.1867f), new Vector3(0f, 0f, -0.8927465f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 108813, 304964818u, new Vector3(2715.22f, -355.234558f, -56.1867f), new Vector3(0f, -0f, -90.9700851f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static CargoShipHeist.MissionOjects Tuner_Gear_Objects = new CargoShipHeist.MissionOjects(new List<CargoShipHeist.ObjectPlacementMap>
		{
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_prop_offchair_exec_01"), 699919, 2668900977u, new Vector3(2719.83f, -359.28f, -53.9846535f), new Vector3(0f, -0f, 133.62f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "bkr_prop_weed_table_01b"), 178517, 304964818u, new Vector3(2705.09448f, -364.876831f, -56.1867f), new Vector3(0f, -0f, -179.6958f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "w_ar_railgun"), 644686, 2418461061u, new Vector3(2705.98779f, -364.721039f, -55.3030243f), new Vector3(-90f, 87.72229f, 0f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "w_lr_homing"), 14676, 1901887007u, new Vector3(2705.8252f, -365.1347f, -55.2284927f), new Vector3(-90f, 72.36512f, 0f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "w_lr_rpg_rocket"), 633939, 2586970039u, new Vector3(2705.53345f, -364.4598f, -55.2873535f), new Vector3(0f, 0f, -75.1307f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "w_ex_grenadefrag"), 212819, 290600267u, new Vector3(2705.56641f, -364.6924f, -55.2807961f), new Vector3(0f, 0f, -59.366684f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_box_ammo01b"), 731220, 2251804373u, new Vector3(2705.328f, -365.094269f, -55.3472977f), new Vector3(0f, -0f, -174.164f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "w_lr_grenadelauncher"), 631381, 3688284050u, new Vector3(2705.55518f, -364.943237f, -55.00962f), new Vector3(-85.32525f, -14.3398495f, -96.2068939f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "w_ex_pe"), 46938, 3184763647u, new Vector3(2705.29272f, -364.7485f, -55.31568f), new Vector3(0f, -0f, -119.60318f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_health_pack"), 217678, 678958360u, new Vector3(2704.77515f, -364.5791f, -55.2455559f), new Vector3(0f, -0f, 167.2588f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_bodyarmour_05"), 342835, 2022153476u, new Vector3(2705.22681f, -365.1104f, -54.75001f), new Vector3(0f, -0f, -155.123154f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "hei_p_parachute_s_female"), 71261, 4139315959u, new Vector3(2704.72217f, -365.178253f, -55.1658173f), new Vector3(81.00116f, -0.9999946f, -169.115829f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ba_prop_battle_hacker_screen"), 858569759, Function.Call<uint>(Hash.GET_HASH_KEY, "ba_prop_battle_hacker_screen"), new Vector3(2716.13f, -358.47f, -53.53f), new Vector3(-65f, 0f, -90f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "gr_prop_gr_rsply_crate04b"), 441971, 1433923164u, new Vector3(301.248749f, 2945.33716f, 42.0018578f), new Vector3(3.66493726f, 1.03525686f, 10.2183533f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "v_corp_lidesk01"), 320015, 3876952021u, new Vector3(2720.463f, -358.215942f, -54.58016f), new Vector3(0f, 0f, 0.520044744f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "sf_prop_sf_monitor_b_02b"), 774405, 2288747747u, new Vector3(2720.02612f, -358.2842f, -53.6625137f), new Vector3(0f, 0f, 0.2790459f), 332554, 0, new Vector3(0f, 0.349999934f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_cs_keyboard_01"), 332554, 1355944948u, new Vector3(2720.02783f, -358.634216f, -53.6625137f), new Vector3(0f, 0f, 0.2790459f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new CargoShipHeist.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_mouse_01"), 292366, 3464378190u, new Vector3(2720.341f, -358.564636f, -53.6795654f), new Vector3(0f, 0f, 9.287555f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		private NativeMenu GunlockerMenu;

		public static Vector3 GunLockerMarker = new Vector3(2711.017f, -354.9473f, -56f);

		public Vector3 GetawayCarArcadeSpawn = new Vector3(2701.902f, -361.279f, -55.8f);

		public float GetawayCarArcadeHeading = 90f;

		public string AdminName = "CINNBARGNU";

		public static float TotalCrewEarned;

		public static float TotalPlayerEarned;

		public Ped CutsenePed1;

		public Ped CutsenePed2;

		public Ped CutsenePed3;

		public Ped CutsenePed4;

		public bool PlayingCutscene;

		public static Vector3 ArcadeGarageExitOnFootSpawn;

		public static float ArcadeGarageExitOnFootSpawnHeading;

		public static bool RequestedCutscene;

		public static Prop Computer;

		public List<VehicleColor> DefaultColours = new List<VehicleColor>
		{
			VehicleColor.MetallicTorinoRed,
			VehicleColor.Orange,
			VehicleColor.MetallicRacingGreen,
			VehicleColor.Blue,
			VehicleColor.MetallicYellowBird,
			VehicleColor.MetallicBlack,
			VehicleColor.MetallicWhite
		};

		public List<Prop> Props = new List<Prop>();

		public static ScriptSettings Config;

		public static Vector3 MenuMarker = new Vector3(2712.729f, -369.3823f, -55.7f);

		public Vector3 MenuMarker2 = new Vector3(2685.03f, -363.9302f, -56.2f);

		public static ObjectPool modLemonUIObjectPool;

		private NativeMenu mainMenu;

		private NativeMenu Heists;

		private NativeMenu Vehicles;

		public Vector3 DoorApos = new Vector3(233.12f, 215.95f, 105f);

		public Vector3 DoorBPos = new Vector3(259.755f, 204.6733f, 105f);

		public Vector3 DoorCPos = new Vector3(257.027f, 220.4003f, 106f);

		public Vector3 DoorDPos = new Vector3(261.93f, 221.79f, 106f);

		public Vector3 FleecaJammerPos = new Vector3(311.815f, -283.3f, 52f);

		public bool DoorCLocked;

		public bool DoorDLocked;

		public Prop DoorCProp;

		public Prop DoorDProp;

		public int FleecaBestTime;

		public int PacificStandardBestTime;

		public int PaletoBestTime;

		public int VangelicoBestTime;

		public int UnionDelpositioryBestTime;

		private Vector3 Vault = new Vector3(0f, 0f, 0f);

		public Vehicle HeistVehicle;

		public int CashCount;

		public NativeMenu All;

		public bool DebugTimeOn;

		public bool Active;

		public int Amt;

		public int iLocal_8077;

		public int GameTimeToTriggerUINotification;

		public static int LesterHasToldPlayerOfNewHeists = 0;

		private bool justrecieved = true;

		private int scaleformloading;

		private bool loadingin = true;

		private bool up;

		private bool up1;

		private bool up2;

		private bool up3;

		private bool up4 = true;

		private bool up5 = true;

		private bool up6 = true;

		private bool up7 = true;

		private bool active;

		private int indexx = -1;

		private int currentbar;

		private float colOneY = 0.4f;

		private float colTwoY = 0.4f;

		private float colThreeY = 0.4f;

		private float colFourY = 0.4f;

		private float colFiveY = 0.74f;

		private float colSixY = 0.74f;

		private float colSevenY = 0.74f;

		private float colEightY = 0.74f;

		private int GameTimeReference1 = Game.GameTime;

		private float fLocal_240 = 0.53f;

		private float fLocal_239 = 0.62f;

		private bool collumn1;

		private bool collumn2;

		private bool collumn3;

		private bool collumn4;

		private bool collumn5;

		private bool collumn6;

		private bool collumn7;

		private bool collumn8;

		private int sound = 2;

		private bool skip1;

		private bool skip2;

		private bool skip3;

		public int FleecaStage;

		public int PacificStandardStage;

		public int PaletoStage;

		public int VangelicoStage;

		public int UnionDelpositioryStage;

		public int LEXIGTONRAIDStage;

		public int SURVERDOWNStage;

		public int NUCLEAREXPANSIONHEISTStage;

		public int DEEPDEAPTHSHEISTStage;

		public int HUMANELABS2Stage;

		public int YachtHeistStage;

		public int BerkraneJobStage;

		public int DiamondHeistStage;

		public int PrisonBreakStage;

		public int ContainerHeistStage;

		public int HumainLabsRaid_CruelMaster;

		public int NorthYanktonStage;

		public List<dynamic> Scripts = new List<object>();

		public List<float> FleecaPay = new List<float>();

		public List<float> PacificStandardPay = new List<float>();

		public List<float> PaletoPay = new List<float>();

		public List<float> VangelicoPay = new List<float>();

		public List<float> UnionDelpositioryPay = new List<float>();

		public List<float> LEXIGTONRAIDPay = new List<float>();

		public List<float> SURVERDOWNPay = new List<float>();

		public List<float> NUCLEAREXPANSIONHEISTPay = new List<float>();

		public List<float> DEEPDEAPTHSHEISTPay = new List<float>();

		public List<float> HUMANELABS2Pay = new List<float>();

		public List<float> YachtHeistSPay = new List<float>();

		public List<float> BerkraneJobPay = new List<float>();

		public List<float> DiamondHeistPay = new List<float>();

		public List<float> PrisonBreakPay = new List<float>();

		public List<float> ContainerHeistPay = new List<float>();

		public List<float> NorthYanktonHeistPay = new List<float>();

		public List<float> CayoPericoHeistPay = new List<float>();

		public string[] StringSplit;

		public string pay;

		public float _GoldPickupPay;

		public float _DiamondsPickupPay;

		public float _CashPickupPay;

		public float _PaintingsPickupPay;

		public float _MiscPickupPay;

		public int ArcadePaintingsFinished;

		public Blip HeistBlip;

		public float HeistMinPay;

		public float HeistMaxPay;

		public List<Blip> ArcadeBlips = new List<Blip>();

		public bool CreatedArcadeBlips;

		public List<Arcade> Arcades = new List<Arcade>();

		public Scaleform ArcadeBoard;

		public Scaleform finaleBoardset;

		public Scaleform ArcadeBoardset;

		public string vaultcontstring = "Cash";

		public int gelelmese;

		public string hselection = "Aggressive";

		public int baz = 11;

		public int faz = 11;

		public string ent;

		public string ex;

		private bool targetgotten = true;

		public static bool CanInitializeYTD = true;

		public static bool HasInitializedYTD;

		public static int ToldPlayerHowToStartHeist;

		public static int RanTutorial;

		public static int TutorialStage;

		public static ScriptSettings ArcadeConfig;

		public bool CheckedCut;

		public NativeMenu mainMenu2;

		public string ChairPropModelArcadeA = "ex_prop_offchair_exec_01";

		public string ChairPropModelArcadeB = "ex_prop_offchair_exec_01";

		public NativeMenu MainStuffMenu;

		public NativeMenu DrinksMenu;

		private NativeMenu MoneymainMenu;

		public float MoneyStored;

		public float Commission;

		public NativeMenu MoneyMenu;

		public Vector3 MoneyStorageMarker;

		public DateTime Date;

		public bool read;

		public Prop P;

		public Prop P2;

		public Prop VaultD;

		public bool Firsttime;

		private Prop ElvDoor1;

		private Prop ElvDoor2;

		private Prop ElvDoor3;

		private Prop ElvDoor4;

		private Ped ElvOpen1;

		private Ped ElvOpen2;

		public int Scene1;

		public Prop bagProp;

		public Prop _cashGrabTray1;

		public Prop MoneyProp;

		public bool PlayingAnim;

		public Vector3 WardrobePos = new Vector3(2723.854f, -377.1916f, -48f);

		public Vector3 HeistTimePos = new Vector3(2712.338f, -362.6943f, -56.2f);

		public List<Prop> Champ = new List<Prop>();

		public Prop Champaine;

		public bool IsDrinking;

		public int DrinkTimer;

		public int Effect;

		public int BottleType;

		public Prop Bottle;

		public Prop Glass;

		public float DrunkLevel;

		public static Prop Wine;

		public static Prop WineGlass;

		public static Vector3 WinePosition;

		public static Vector3 WineGlassPosition;

		public static Vector3 WineRotation;

		public static Vector3 WineGlassRotation;

		public static int DrunkStage = 1;

		public static int WhiskeyTaskScriptStatus = -1;

		public static int WineTaskScriptStatus = -1;

		public static int WheatTaskScriptStatus = -1;

		public static List<Model> WineModels = new List<Model> { 21833643 };

		public static List<Model> WineGlassModels = new List<Model> { -35679191 };

		public static List<Model> WheatModels = new List<Model> { 469594741 };

		public static List<Model> WhiskeyModels = new List<Model> { 488156118 };

		public static List<Model> WhiskeyGlassModels = new List<Model> { -1533900808, 1480049515 };

		public static Vector3 WhiskeyPosition;

		public static Vector3 WhiskeyGlassPosition;

		public static Vector3 WhiskeyRotation;

		public static Vector3 WhiskeyGlassRotation;

		public static Vector3 WheatPosition;

		public static Vector3 WheatRotation;

		public ObjectPool WorderbePool;

		public NativeMenu WoredrobeMainMenu;

		public NativeMenu WoredrobeMenu;

		public int CompMax;

		public int DrawableMax;

		public bool InShower;

		public bool sitting;

		public WeaponTint Liv;

		public int ID_O;

		public string ID_C;

		public int Comp;

		public Model OldCharacter;

		public int Heist;

		public bool ActiveHack;

		private int hackingScalwform;

		private int uLocal_70;

		private int Hacking_SelectedIndex = -1;

		private float Hacking_Lives = 3f;

		private int FeedBackNumber = -1;

		private bool DrawHackScaleform;

		private int ErrorTimer = Game.GameTime;

		private bool TRYDL;

		private bool IpTry;

		private int IpTryTimer = Game.GameTime;

		private int IpTryFailedTimer = Game.GameTime;

		private bool PlayBoth = true;

		private int IPSound;

		private int MoveSound;

		private int MoveTimer = Game.GameTime;

		private int MoveSoundTimer = Game.GameTime;

		private int Anim_Timer = Game.GameTime;

		private VariableTimer MyHackingTimer;

		public bool HackingSuccess;

		public bool HackDone;

		private List<dynamic> HackingProps = new List<object>
		{
			"hei_prop_hei_securitypanel", "hei_prop_hei_keypad_01", "hei_prop_hei_keypad_02", "hei_prop_hei_keypad_03", "prop_ld_keypad_01", "prop_ld_keypad_01b", "prop_ld_keypad_01b_lod", "hei_prop_hei_cs_keyboard", "prop_cs_keyboard_01", "prop_keyboard_01a",
			"prop_keyboard_01b", "prop_laptop_jimmy", "prop_laptop_lester", "prop_laptop_lester2", "p_amb_lap_top_02", "p_cs_laptop_02", "p_laptop_02_s", "prop_laptop_01a"
		};

		public Prop Drill;

		public bool Isdrilliing;

		public int DrillFX;

		public int Sound;

		public float Zone;

		public int FailSound;

		public float Speed;

		public float Temp;

		public float Pos;

		public int scale;

		public bool triggerLockA;

		public bool triggerLockB;

		public bool triggerLockC;

		public bool triggerLockD;

		public int PinSound;

		public bool StartDrill;

		public bool DrillDone;

		public int EndTimer;

		public bool firsttime = true;

		public float drillspeed;

		public float temperature;

		public float discs = 4f;

		public float fvar0 = 1f;

		public float depth;

		public float paramf_12 = 1f;

		public float DrillPos = 1f;

		public bool activeCasLaser;

		public bool activeCasDrill;

		public bool frozen;

		public bool pin_one;

		public bool pin_two;

		public bool pin_three;

		public bool pin_four;

		public bool pin_five;

		public bool pin_six;

		public bool pin_seven;

		public bool pin_eight;

		public float NumOfDiscs = 6f;

		public int Drillstage = 1;

		public bool PlayDrillFXSound;

		public bool DrillingComplete;

		public bool UseCasinoDrill;

		public bool Intunnel;

		public Vector3 PracticeHackPos = new Vector3(2696.77f, -368.41f, -55.8f);

		public static Vector3 PracticeDrill = new Vector3(2688.67f, -368.558f, -55.8f);

		public int Hackerskill = 4;

		public List<Prop> PaintingHolder = new List<Prop>();

		public List<Prop> Paintings = new List<Prop>();

		public List<int> PaintingsFinished = new List<int>();

		public List<string> PaintingIDs = new List<string> { "ch_prop_vault_painting_01j", "ch_prop_vault_painting_01i", "ch_prop_vault_painting_01h", "ch_prop_vault_painting_01g", "ch_prop_vault_painting_01f", "ch_prop_vault_painting_01e", "ch_prop_vault_painting_01d", "ch_prop_vault_painting_01c", "ch_prop_vault_painting_01b", "ch_prop_vault_painting_01a" };

		public int CuttingStage;

		public int VaultType;

		public List<int> CurrentSDBType = new List<int>();

		public List<int> CurrentSDB = new List<int>();

		public List<string> CurrentSDBDict = new List<string>();

		public bool SetupPaintingsOrLockVault;

		public float nextTick;

		public float CurrentPay;

		public bool NotifyPlayerthatLesterWantsToSeeThem;

		public Prop ArcadePainting;

		public Prop ArcadePaintingHolder;

		public List<Prop> Wallart = new List<Prop>();

		public bool Firstime;

		public Ped Lester;

		public bool CreatedLester;

		public Prop ArcadeChairA;

		public Prop ArcadeChairB;

		public Vector3 ArcadeChairAPos = new Vector3(2726.5f, -377.5f, -47.8f);

		public Vector3 ArcadeChairBPos = new Vector3(2720.033f, -363.581f, -54.5f);

		public float ArcadeChairAHeading = 145f;

		public float ArcadeChairBHeading = -45f;

		public bool WCamOn;

		public bool SittingInArcadeChairA;

		public bool SittingInArcadeChairB;

		public int GarageOwned = 1;

		public Prop UndergroundDoor;

		public Vector3 RadioPos = new Vector3(2721.3f, -381.3f, -50f);

		public bool DeliveringVehicle;

		public Vehicle VehicleToDeliver;

		public Vehicle Vehicle1;

		public Vehicle Vehicle2;

		public Vehicle Vehicle3;

		public Vehicle ArcadeHauler1;

		public Vehicle ArcadeHauler2;

		public Vehicle ArcadeHauler3;

		public Prop ArcadeHauler1Cargo;

		public Prop ArcadeHauler2Cargo;

		public Prop ArcadeHauler3Cargo;

		public Ped TargetPed;

		public static Vector3 ArcadeBusinessEnter;

		public static Vector3 ArcadeBusinessExit = new Vector3(2737.7f, -375.9f, -49f);

		public static Vector3 ArcadeGarageEnter;

		public static float ArcadeGarageEnterHeading;

		public static Vector3 ArcadeGarageExit = new Vector3(2677.8f, -361.4f, -56.5f);

		public static int ArcadeOwned;

		public static Blip Arcade;

		public static int ArcadeVersion;

		public static bool IsInArcade;

		public List<string> RadioStat = new List<string>();

		public static Vector3 AVehPos_1 = new Vector3(2704.7f, -357.0222f, -55.8f);

		public static float AVehRot_1 = 180f;

		public static Vector3 AVehPos_2 = new Vector3(2696.91f, -356.6655f, -55.8f);

		public static float AVehRot_2 = 180f;

		public static Vector3 AVehPos_3 = new Vector3(2688.997f, -357.438f, -55.8f);

		public static float AVehRot_3 = 180f;

		public static Vector3 AVehPos_4 = new Vector3(2720.1f, -395.9f, -55.8f);

		public static float AVehRot_4 = 0f;

		public static Vector3 AVehPos_5 = new Vector3(2712.3f, -395.9f, -55.8f);

		public static float AVehRot_5 = 0f;

		public static Vector3 AVehPos_6 = new Vector3(2704.2f, -395.9f, -55.8f);

		public static float AVehRot_6 = 0f;

		public static Vector3 AVehPos_7 = new Vector3(2688.6f, -395.9f, -55.8f);

		public static float AVehRot_7 = 0f;

		public static Vector3 AVehPos_8 = new Vector3(2681.1f, -395.9f, -55.8f);

		public static float AVehRot_8 = 0f;

		public static Vector3 AVehPos_9 = new Vector3(2712.199f, -381.5f, -55.8f);

		public static float AVehRot_9 = -180f;

		public static Vector3 AVehPos_10 = new Vector3(2704.22f, -381.72f, -55.8f);

		public static float AVehRot_10 = -180f;

		public static Vector3 AVehPos_11 = new Vector3(2688.8f, -382.053f, -55.8f);

		public static float AVehRot_11 = -180f;

		public static int ArcadeFloodID;

		public static int MuralID;

		public static int PushieID;

		public static int NeonArtID;

		public static int ArcadeStyle;

		public static bool ArcadeClutter;

		public static int UnlockedDilllEquipment;

		public static int UnlockedHackingEquipment;

		public static int UnlockedBreachingEquipment;

		public static int UnlockedOutfitsEquipment;

		public static int UnlockedPlansEquipment;

		public static int UnlockedGarage;

		public static int UnlockedGunlocker;

		public static SaveCar SC = new SaveCar();

		public NativeMenu RemoveMenu;

		public NativeMenu RemoveMenu2;

		public NativeMenu GarageMenu;

		public NativeMenu GarageMenu2;

		public static Vehicle ArcadeVeh1;

		public static Vehicle ArcadeVeh2;

		public static Vehicle ArcadeVeh3;

		public static Vehicle ArcadeVeh4;

		public static Vehicle ArcadeVeh5;

		public static Vehicle ArcadeVeh6;

		public static Vehicle ArcadeVeh7;

		public static Vehicle ArcadeVeh8;

		public static Vehicle ArcadeVeh9;

		public static Vehicle ArcadeVeh10;

		public static Vehicle ArcadeVeh11;

		public static Vector3 ArcadeVehOptions = new Vector3(2697.018f, -384.5703f, -56f);

		public static string path = "scripts//HKH_SinglePlayerHeists//Arcade//";

		public static string ListPath = "scripts//HKHModHelper//AllVehicles.ini";

		public NativeMenu ArcadeC;

		public Ped Bartender;

		public bool PlayingArcadeGame;

		public int CurrentArcadeGame;

		public List<Ped> ArcadePeds = new List<Ped>();

		public Vector3 OriginalArcadePos;

		public List<Vector3> ArcadePos = new List<Vector3>();

		public List<Prop> ArcadeProps = new List<Prop>();

		public Camera ArcadeCamera;

		public NativeMenu UpgradeArcade;

		public NativeMenu ArcadeMissions;

		public List<Prop> ArcadeMissionProps = new List<Prop>();

		public List<Vehicle> ArcadeMissionVehicles = new List<Vehicle>();

		public List<Ped> ArcadeMissionPeds = new List<Ped>();

		public int DroneTimer1;

		public int DroneTimer2;

		public int DroneTimer3;

		public static int ArcadeMoneyVault;

		public Vector3 ArcadeMoneyMarker = new Vector3(2728.24f, -374.4655f, -48.5f);

		public int ArcadeLevel;

		public int CurrentArcadeValue;

		public int CurrentArcade;

		public bool BuyingArcades;

		public Camera ArcadeBuyCam;

		public NativeMenu ArcadeBuyMenu;

		public Prop FakeArcade;

		public List<Ped> ArcadeCarPed = new List<Ped>();

		public List<Ped> ArcadeBikePed = new List<Ped>();

		public int ArcadeCarPedTimer;

		public int ArcadeBikePedTimer;

		public int ArcadeMissionTimer;

		public List<RaceArcade> RacingArcades = new List<RaceArcade>();

		public List<string> ArcadePropNames = new List<string>
		{
			"null", "ch_prop_arcade_claw_01a", "ch_prop_arcade_degenatron_01a", "ch_prop_arcade_fortune_01a", "ch_prop_arcade_gun_01a", "ch_prop_arcade_love_01a", "ch_prop_arcade_space_01a", "ch_prop_arcade_street_02b", "ch_prop_arcade_street_01d", "ch_prop_arcade_street_01c",
			"ch_prop_arcade_street_01b", "ch_prop_arcade_street_01a", "ch_prop_arcade_invade_01a", "ch_prop_arcade_wizard_01a"
		};

		public Blip ArcadeMissionBlip;

		public Blip ArcadeMissionBlip1;

		public Blip ArcadeMissionBlip2;

		public Blip ArcadeMissionBlip3;

		public List<Ped> ArcadeInteriorPeds2 = new List<Ped>();

		public List<Ped> ArcadeInteriorPeds = new List<Ped>();

		public Vector3 ArcadeRandomLoc;

		public bool ArcadeMissionComplete;

		public List<Prop> SeconardyArcadeProps = new List<Prop>();

		public static Vector3 ArcadePos1 = new Vector3(2730.69f, -376.3358f, -50f);

		public static float ArcadeRot1 = 90f;

		public static int Arcade1Ver;

		public static Vector3 ArcadePos2 = new Vector3(ArcadePos1.X, ArcadePos1.Y - 1f, ArcadePos1.Z);

		public static float ArcadeRot2 = 90f;

		public static int Arcade2Ver;

		public static Vector3 ArcadePos3 = new Vector3(ArcadePos2.X, ArcadePos2.Y - 1f, ArcadePos2.Z);

		public static float ArcadeRot3 = 90f;

		public static int Arcade3Ver;

		public static Vector3 ArcadePos4 = new Vector3(ArcadePos2.X, ArcadePos2.Y + 2f, ArcadePos2.Z);

		public static float ArcadeRot4 = 90f;

		public static int Arcade4Ver;

		public static Vector3 ArcadePos5 = new Vector3(ArcadePos4.X, ArcadePos4.Y + 1f, ArcadePos4.Z);

		public static float ArcadeRot5 = 90f;

		public static int Arcade5Ver;

		public static Vector3 ArcadePos6 = new Vector3(2730.137f, -371.844f, -50f);

		public static float ArcadeRot6 = 0f;

		public static int Arcade6Ver;

		public static Vector3 ArcadePos7 = new Vector3(ArcadePos6.X + 1f, ArcadePos6.Y, ArcadePos6.Z);

		public static float ArcadeRot7 = 0f;

		public static int Arcade7Ver;

		public static Vector3 ArcadePos8 = new Vector3(ArcadePos7.X + 1f, ArcadePos7.Y, ArcadePos7.Z);

		public static float ArcadeRot8 = 0f;

		public static int Arcade8Ver;

		public static Vector3 ArcadePos9 = new Vector3(ArcadePos8.X + 1f, ArcadePos8.Y, ArcadePos8.Z);

		public static float ArcadeRot9 = 0f;

		public static int Arcade9Ver;

		public static Vector3 ArcadePos10 = new Vector3(ArcadePos9.X + 1f, ArcadePos9.Y, ArcadePos9.Z);

		public static float ArcadeRot10 = 0f;

		public static int Arcade10Ver;

		public static Vector3 ArcadePos11 = new Vector3(ArcadePos10.X + 1f, ArcadePos10.Y, ArcadePos10.Z);

		public static float ArcadeRot11 = 0f;

		public static int Arcade11Ver;

		public static Vector3 ArcadePos12 = new Vector3(2733.906f, -385.2436f, -50f);

		public static float ArcadeRot12 = -90f;

		public static int Arcade12Ver;

		public static Vector3 ArcadePos13 = new Vector3(ArcadePos12.X, ArcadePos12.Y + 1f, ArcadePos12.Z);

		public static float ArcadeRot13 = -90f;

		public static int Arcade13Ver;

		public static Vector3 ArcadePos14 = new Vector3(ArcadePos13.X, ArcadePos13.Y + 1f, ArcadePos13.Z);

		public static float ArcadeRot14 = -90f;

		public static int Arcade14Ver;

		public static Vector3 ArcadePos15 = new Vector3(ArcadePos14.X, ArcadePos14.Y + 1f, ArcadePos14.Z);

		public static float ArcadeRot15 = -90f;

		public static int Arcade15Ver;

		public static Vector3 ArcadePos16 = new Vector3(ArcadePos15.X, ArcadePos15.Y + 1f, ArcadePos15.Z);

		public static float ArcadeRot16 = -90f;

		public static int Arcade16Ver;

		public static Vector3 ArcadePos17 = new Vector3(2733.07f, -391.4859f, -49.5f);

		public static float ArcadeRot17 = 180f;

		public static int Arcade17Ver;

		public static Vector3 ArcadePos18 = new Vector3(ArcadePos17.X - 1f, ArcadePos17.Y, ArcadePos17.Z);

		public static float ArcadeRot18 = 180f;

		public static int Arcade18Ver;

		public static Vector3 ArcadePos19 = new Vector3(ArcadePos18.X - 1f, ArcadePos18.Y, ArcadePos18.Z);

		public static float ArcadeRot19 = 180f;

		public static int Arcade19Ver;

		public static Vector3 ArcadePos20 = new Vector3(ArcadePos19.X - 1f, ArcadePos19.Y, ArcadePos19.Z);

		public static float ArcadeRot20 = 180f;

		public static int Arcade20Ver;

		public static int UndergrondAreaLocked;

		public static int UsingHiddenDoor = 1;

		public static bool HiddenDoorOpen;

		public static bool HiddenDoorOpen_WaitForPlayerFar;

		public static bool HiddenDoorOpen_WaitForPlayerClose;

		public static bool UndergrondAreaLockedbool;

		public static bool UndergrondAreaLockedbool2;

		public static Vehicle CarToDelete;

		public static float M = 0f;

		public static string Price = "000";

		public static string Model = "";

		public static string man = "";

		public Blip missionBlip;

		public ScaledRectangle RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 108));

		public List<NativeMenu> GLemonUIMenusNativeMenus = new List<NativeMenu>();

		public Vehicle SaveVehicle;

		public string GarageNum;

		private string VehicleLoadPath = "Scripts\\HKH_SinglePlayerHeists\\Arcade\\";

		private List<string> ClubDancerList = new List<string>();

		public static bool PlayRadioMusic;

		public static bool RadioMusicOn;

		public static bool RadioOn;

		public static bool RadioIsOn = false;

		public static string DefaultRadioStation = "1_";

		public static int CurrentRadioStation;

		public bool RefreshingRadio;

		public int NextTutorialPhase;

		public int CuttingCabinet;

		public int CuttingScene1;

		public Vector3 CuttingVector3;

		public float Cuttingheading;

		public Prop CuttingGlassPlate;

		public int CabinetPropsSelectedIndex;

		public bool GotCash;

		public Prop Cabinet;

		public bool CreatedCabinet;

		public List<float> Crate_Type = new List<float>();

		public List<Blip> HubPointsBlip = new List<Blip>();

		public List<bool> HubPointsRobbed = new List<bool>();

		public List<Prop> Crate_Valuables = new List<Prop>();

		public List<SecCabinetProps> SecurityCabinetProps = new List<SecCabinetProps>();

		public int iParam0f_1;

		public int Local_26709f_13;

		public int Local_26709f_8 = -1;

		public int SelectedNum;

		public float InputedNumber1;

		public float InputedNumber2;

		public float InputedNumber3;

		public bool ShowingSafeScaleform;

		public bool ReadyForCode;

		public int SafeScene1;

		public Prop Safe;

		public bool PlayingIdle;

		public bool GotCorrectSafeCode;

		public bool GotIncorrectSafeCode;

		public Prop SafeValuables;

		public bool PlayingSafeFinalAnim;

		public bool FinishedSafeAnimations;

		public int CodeNumber1;

		public int CodeNumber2;

		public int CodeNumber3;

		public int iLocal_26708;

		public int Local_26709f_9;

		public bool DrillComplete;

		public bool VaultOpenedByDrill;

		public Scaleform SimeonText1;

		public Prop MoneyVaultDoor;

		public Prop ExtraValuable;

		public bool RobbedExtraValuable;

		public bool ExtraVaultOpen;

		public int ExtraValuableType;

		public bool SetRob;

		public bool InitialAnimDone;

		public int RobTimer;

		public bool RunningTutorial;

		public int GrabbingScene;

		public Scaleform Duf_WhiteBoard_Title;

		public Scaleform Duf_WhiteBoard_Title2;

		public Scaleform Duf_WhiteBoard_Title_1;

		public Scaleform Duf_WhiteBoard_Title_2;

		public Scaleform Duf_WhiteBoard_Title3;

		public Scaleform Duf_WhiteBoard_Title4;

		public bool RereadMoney;

		public int UnlockedDilllEquipmentTEMP;

		public int UnlockedHackingEquipmentTEMP;

		public int UnlockedBreachingEquipmentTEMP;

		public int UnlockedOutfitsEquipmentTEMP;

		public int UnlockedPlansEquipmentTEMP;

		public int UnlockedGarageTEMP;

		public int UnlockedGunlockerTEMP;

		public int ArcadeMoneyVaultTEMP;

		public int UndergrondAreaLockedTEMP;

		public static List<Model> TVModels = new List<Model> { -240931727 };

		public static int rendertargetid;

		public static int ex_rendertargetid;

		public static int rendertargetid2;

		public static bool TVOn = false;

		public static bool SubtitleOn = false;

		public static int TV_Volume = 0;

		public static int TV_Channel;

		public static List<int> TVChannelList = new List<int> { 0, 1 };

		public static Camera TVCamera;

		public Prop TV;

		public int tvscene;

		public bool FirstPlay;

		public int ONscreens;

		public int HackTimer;

		public int HackStage;

		public bool CustomhackOn;

		public float screenaspectratio;

		public int Number1;

		public bool GotNo1;

		public int Number2;

		public bool GotNo2;

		public int Number3;

		public bool GotNo3;

		public bool RunningPadLockMinigame;

		public float Rotation;

		public float ActualRotation;

		public float LeftMP = 1.01f;

		public float RightMP = 1.01f;

		public bool PDLHackComplete;

		public bool AllowUnlockonNonStop = true;

		public bool SuccessHotwire;

		public bool HotWireMinigameOn;

		public int SelectedLeftNode_1;

		public int SelectedRightNode_1;

		public int SelectedLeftNode_2;

		public int SelectedRightNode_2;

		public int SelectedLeftNode_3;

		public int SelectedRightNode_3;

		public int SelectedLeftNode_4;

		public int SelectedRightNode_4;

		public bool LeftSelected;

		public int LeftNode;

		public bool RightSelected;

		public int RightNode;

		public int _1ID;

		public int _2ID;

		public int _3ID;

		public int _4ID;

		public int Correct_1ID;

		public int Correct_2ID;

		public int Correct_3ID;

		public int Correct_4ID;

		public int RightNode_1Lives;

		public int RightNode_2Lives;

		public int RightNode_3Lives;

		public int RightNode_4Lives;

		public int TokensSelected;

		public float HotwireloadingDelay;

		public float HotwireloadingDelayTimer;

		public int LeftNode_1Locked;

		public int LeftNode_2Locked;

		public int LeftNode_3Locked;

		public int LeftNode_4Locked;

		public float HotwireTimeLeft;

		public float LastNodeX;

		public float LastNodeY;

		public float fLocal_564;

		public bool SelectingNode;

		public List<float> Node1Warnings = new List<float> { 255f, 255f, 255f, 255f };

		public List<float> Node2Warnings = new List<float> { 255f, 255f, 255f, 255f };

		public List<float> Node3Warnings = new List<float> { 255f, 255f, 255f, 255f };

		public List<float> Node4Warnings = new List<float> { 255f, 255f, 255f, 255f };

		public Scaleform scaleform;

		public Scaleform scaleform2;

		public bool PlayNewMinigames = true;

		public bool Circuithack;

		public bool TrafficLightHack;

		public Camera TRFCam;

		public int TRFLightStage_1;

		public int TRFLightStage_2 = 1;

		public int TRFLightStage_3;

		public int TRFLightStage_4 = 1;

		public int TRFLightStage_5;

		public int TRFLightStage_6;

		public int TRFLightStage_7;

		public int TRFLightStage_8;

		public int TRFLightStage_9 = 1;

		public int TRFLightStage_10;

		public int TRFLightStage_11;

		public Vector3 Truck1Pos = new Vector2(0.0997f, 0.0798f);

		public Vector3 Truck2Pos = new Vector2(0.10406f, 0.04722f);

		public Vehicle TrafficLightTruck1;

		public Vehicle TrafficLightTruck2;

		public List<Vehicle> TRFVehicles = new List<Vehicle>();

		public List<Ped> TRFPeds = new List<Ped>();

		public List<TrafficLightRoute> Routes = new List<TrafficLightRoute>();

		public bool RecordingRoute;

		public List<Vector2> Route = new List<Vector2>();

		public int RouteNum;

		public int NodeNum;

		public float RoundedNumber;

		public float NumberAtCheck;

		public bool PlayingVoltageMG;

		public string Number = "451";

		public int RNo1;

		public int RNo2;

		public int RNo3;

		public int LNo1;

		public int LNo2;

		public int LNo3;

		public int VTL1node;

		public int VTR1node;

		public int VTL2node;

		public int VTR2node;

		public int VTL3node;

		public int VTR3node;

		public int VTL1nodeSelected;

		public int VTR1nodeSelected;

		public int VTL2nodeSelected;

		public int VTR2nodeSelected;

		public int VTL3nodeSelected;

		public int VTR3nodeSelected;

		public int RightNodeOn;

		public int LeftNodeOn;

		public bool SelectedLN1;

		public bool SelectedRN1;

		public bool SelectedLN2;

		public bool SelectedRN2;

		public bool SelectedLN3;

		public bool SelectedRN3;

		public int NodeOn;

		public int Delay;

		public bool AwaitingDelay;

		public int WrongDelay;

		public bool VideoPlaying;

		public int BINKMovie = -1;

		public bool CopyPrintHackOn;

		public int Print;

		public string PrintDict;

		public int PrintNum;

		public List<string> Prints = new List<string>
		{
			"mpfclone_retro_printfull1,fingerprint_hacking_minigame_fingerprints_01_slice_01", "mpfclone_retro_printfull1,fingerprint_hacking_minigame_fingerprints_01_slice_02", "mpfclone_retro_printfull1,fingerprint_hacking_minigame_fingerprints_01_slice_03", "mpfclone_retro_printfull1,fingerprint_hacking_minigame_fingerprints_01_slice_04", "mpfclone_retro_printfull1,fingerprint_hacking_minigame_fingerprints_01_slice_05", "mpfclone_retro_printfull1,fingerprint_hacking_minigame_fingerprints_01_slice_06", "mpfclone_retro_printfull1,fingerprint_hacking_minigame_fingerprints_01_slice_07", "mpfclone_retro_printfull1,fingerprint_hacking_minigame_fingerprints_01_slice_08", "mpfclone_retro_printfull2,fingerprint_hacking_minigame_fingerprints_02_slice_01", "mpfclone_retro_printfull2,fingerprint_hacking_minigame_fingerprints_02_slice_02",
			"mpfclone_retro_printfull2,fingerprint_hacking_minigame_fingerprints_02_slice_03", "mpfclone_retro_printfull2,fingerprint_hacking_minigame_fingerprints_02_slice_04", "mpfclone_retro_printfull2,fingerprint_hacking_minigame_fingerprints_02_slice_05", "mpfclone_retro_printfull2,fingerprint_hacking_minigame_fingerprints_02_slice_06", "mpfclone_retro_printfull2,fingerprint_hacking_minigame_fingerprints_02_slice_07", "mpfclone_retro_printfull2,fingerprint_hacking_minigame_fingerprints_02_slice_08", "mpfclone_retro_printfull3,fingerprint_hacking_minigame_fingerprints_03_slice_01", "mpfclone_retro_printfull3,fingerprint_hacking_minigame_fingerprints_03_slice_02", "mpfclone_retro_printfull3,fingerprint_hacking_minigame_fingerprints_03_slice_03", "mpfclone_retro_printfull3,fingerprint_hacking_minigame_fingerprints_03_slice_04",
			"mpfclone_retro_printfull3,fingerprint_hacking_minigame_fingerprints_03_slice_05", "mpfclone_retro_printfull3,fingerprint_hacking_minigame_fingerprints_03_slice_06", "mpfclone_retro_printfull3,fingerprint_hacking_minigame_fingerprints_03_slice_07", "mpfclone_retro_printfull3,fingerprint_hacking_minigame_fingerprints_03_slice_08", "mpfclone_retro_printfull4,fingerprint_hacking_minigame_fingerprints_04_slice_01", "mpfclone_retro_printfull4,fingerprint_hacking_minigame_fingerprints_04_slice_02", "mpfclone_retro_printfull4,fingerprint_hacking_minigame_fingerprints_04_slice_03", "mpfclone_retro_printfull4,fingerprint_hacking_minigame_fingerprints_04_slice_04", "mpfclone_retro_printfull4,fingerprint_hacking_minigame_fingerprints_04_slice_05", "mpfclone_retro_printfull4,fingerprint_hacking_minigame_fingerprints_04_slice_06",
			"mpfclone_retro_printfull4,fingerprint_hacking_minigame_fingerprints_04_slice_07", "mpfclone_retro_printfull4,fingerprint_hacking_minigame_fingerprints_04_slice_08", "mpfclone_retro_printfull5,fingerprint_hacking_minigame_fingerprints_05_slice_01", "mpfclone_retro_printfull5,fingerprint_hacking_minigame_fingerprints_05_slice_02", "mpfclone_retro_printfull5,fingerprint_hacking_minigame_fingerprints_05_slice_03", "mpfclone_retro_printfull5,fingerprint_hacking_minigame_fingerprints_05_slice_04", "mpfclone_retro_printfull5,fingerprint_hacking_minigame_fingerprints_05_slice_05", "mpfclone_retro_printfull5,fingerprint_hacking_minigame_fingerprints_05_slice_06", "mpfclone_retro_printfull5,fingerprint_hacking_minigame_fingerprints_05_slice_07", "mpfclone_retro_printfull5,fingerprint_hacking_minigame_fingerprints_05_slice_08",
			"mpfclone_retro_printfull6,fingerprint_hacking_minigame_fingerprints_06_slice_01", "mpfclone_retro_printfull6,fingerprint_hacking_minigame_fingerprints_06_slice_02", "mpfclone_retro_printfull6,fingerprint_hacking_minigame_fingerprints_06_slice_03", "mpfclone_retro_printfull6,fingerprint_hacking_minigame_fingerprints_06_slice_04", "mpfclone_retro_printfull6,fingerprint_hacking_minigame_fingerprints_06_slice_05", "mpfclone_retro_printfull6,fingerprint_hacking_minigame_fingerprints_06_slice_06", "mpfclone_retro_printfull6,fingerprint_hacking_minigame_fingerprints_06_slice_07", "mpfclone_retro_printfull6,fingerprint_hacking_minigame_fingerprints_06_slice_08", "mpfclone_retro_printfull7,fingerprint_hacking_minigame_fingerprints_07_slice_01", "mpfclone_retro_printfull7,fingerprint_hacking_minigame_fingerprints_07_slice_02",
			"mpfclone_retro_printfull7,fingerprint_hacking_minigame_fingerprints_07_slice_03", "mpfclone_retro_printfull7,fingerprint_hacking_minigame_fingerprints_07_slice_04", "mpfclone_retro_printfull7,fingerprint_hacking_minigame_fingerprints_07_slice_05", "mpfclone_retro_printfull7,fingerprint_hacking_minigame_fingerprints_07_slice_06", "mpfclone_retro_printfull7,fingerprint_hacking_minigame_fingerprints_07_slice_07", "mpfclone_retro_printfull7,fingerprint_hacking_minigame_fingerprints_07_slice_08", "mpfclone_retro_printfull8,fingerprint_hacking_minigame_fingerprints_08_slice_01", "mpfclone_retro_printfull8,fingerprint_hacking_minigame_fingerprints_08_slice_02", "mpfclone_retro_printfull8,fingerprint_hacking_minigame_fingerprints_08_slice_03", "mpfclone_retro_printfull8,fingerprint_hacking_minigame_fingerprints_08_slice_04",
			"mpfclone_retro_printfull8,fingerprint_hacking_minigame_fingerprints_08_slice_05", "mpfclone_retro_printfull8,fingerprint_hacking_minigame_fingerprints_08_slice_06", "mpfclone_retro_printfull8,fingerprint_hacking_minigame_fingerprints_08_slice_07", "mpfclone_retro_printfull8,fingerprint_hacking_minigame_fingerprints_08_slice_08"
		};

		public List<string> Section1Print = new List<string>();

		public List<string> Section2Print = new List<string>();

		public List<string> Section3Print = new List<string>();

		public List<string> Section4Print = new List<string>();

		public List<string> Section5Print = new List<string>();

		public List<string> Section6Print = new List<string>();

		public List<string> Section7Print = new List<string>();

		public List<string> Section8Print = new List<string>();

		public int Selection1;

		public int Selection2;

		public int Selection3;

		public int Selection4;

		public int Selection5;

		public int Selection6;

		public int Selection7;

		public int Selection8;

		public float PHTimeLeft;

		public int SectionSelected = 1;

		public int HacksCompleted;

		public int HacksToComplete;

		public bool Checking;

		public int CheckDelay;

		public int CDPoint;

		public List<Vector2> LoadingBarPoints = new List<Vector2>
		{
			new Vector2(0.3755814f, 83f / 160f),
			new Vector2(0.3883721f, 83f / 160f),
			new Vector2(0.4020349f, 83f / 160f),
			new Vector2(0.4145349f, 0.5180556f),
			new Vector2(0.4264535f, 0.5173611f),
			new Vector2(0.4386628f, 83f / 160f),
			new Vector2(0.4514535f, 83f / 160f),
			new Vector2(0.4636628f, 0.5173611f),
			new Vector2(0.4761628f, 0.5173611f),
			new Vector2(0.490407f, 0.5173611f),
			new Vector2(0.5026163f, 0.5166667f),
			new Vector2(0.5159883f, 0.5166667f),
			new Vector2(0.5287791f, 0.5173611f),
			new Vector2(0.5421512f, 0.5173611f),
			new Vector2(0.5552326f, 0.5173611f),
			new Vector2(0.5677326f, 0.5166667f),
			new Vector2(0.5790698f, 0.5173611f),
			new Vector2(0.5915698f, 0.5180556f),
			new Vector2(0.6034884f, 0.5180556f),
			new Vector2(0.6156977f, 0.5180556f),
			new Vector2(0.6273256f, 0.5180556f)
		};

		public bool PHHackSuccess;

		public int BarOn;

		public int LoadingTimer;

		public bool PlayingAMinigame;

		public bool PlayerFirstAnim;

		public bool PlayerFinishedLastAnim;

		public bool PlayerWaitingForLastAnim;

		public int PlayerGrabbingStage;

		public int CurrentType;

		public Prop PlayerRobbingProp;

		public bool PlayerGotJewels;

		public int CuttingSound;

		public int CuttingTime;

		public int CuttingPTFX;

		public float PowerOutput;

		public int RequiredPowerOutput;

		public int TimerPowerOutput;

		public bool PlayerPlayingFinalAnim;

		public bool PlayerStartedCuttingAnim;

		public bool PlayingPTFX;

		public bool PlayerFinishedCutting;

		public Scaleform ECUJobBoardset;

		public Scaleform FleecaBoardset;

		public Scaleform PacBoardset;

		public int RenderTime;

		public float BoardRot;

		public bool CreatedTunerGear;

		public bool CleanedupHeistBoards;

		public static Vector3 Offset = new Vector3(0.78f, -0.0249f, 1.3f);

		public static Vector3 Size = new Vector3(4f, 1.9f, 1.2f);

		public static int XYZEdit = 1;

		public static int typeEdit;

		public static Scaleform PrepBoard1;

		public static Scaleform PrepBoard2;

		public static List<HeistData> Heists_Data = new List<HeistData>();

		public static int CurrentHeistData = 0;

		public Prop vegStand;

		public static Vector3 HireBodyguardPosition = new Vector3(2680.047f, -358.24f, -56.18f);

		public static int RenderedIntel;

		public static int RenderedBoardStage;

		public int Zoom;

		public static List<Blip> MainBlips_U = new List<Blip>();

		public static List<Blip> MainBlips_G = new List<Blip>();

		public static Scaleform CustomHeistBoard;

		public static int Damage;

		public static int CheckTime;

		public List<string> CutscenePed1Comp = new List<string>
		{
			"0_0_0", "1_0_0", "2_0_0", "3_0_0", "4_0_0", "5_0_0", "6_0_0", "7_0_0", "8_0_0", "9_0_0",
			"10_0_0", "11_0_0"
		};

		public List<string> CutscenePed2Comp = new List<string>
		{
			"0_0_0", "1_0_0", "2_0_0", "3_0_0", "4_0_0", "5_0_0", "6_0_0", "7_0_0", "8_0_0", "9_0_0",
			"10_0_0", "11_0_0"
		};

		public List<string> CutscenePed3Comp = new List<string>
		{
			"0_0_0", "1_0_0", "2_0_0", "3_0_0", "4_0_0", "5_0_0", "6_0_0", "7_0_0", "8_0_0", "9_0_0",
			"10_0_0", "11_0_0"
		};

		public List<string> CutscenePed4Comp = new List<string>
		{
			"0_0_0", "1_0_0", "2_0_0", "3_0_0", "4_0_0", "5_0_0", "6_0_0", "7_0_0", "8_0_0", "9_0_0",
			"10_0_0", "11_0_0"
		};

		public static bool DisableHacks = true;

		public static void ATTACH_ENTITY_TO_ENTITY(Entity entity1, Entity entity2, int boneIndex, float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, bool p9, bool useSoftPinning, bool collision, bool isPed, int vertexIndex, bool fixedRot)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, entity1, entity2, boneIndex, xPos, yPos, zPos, xRot, yRot, zRot, p9, useSoftPinning, collision, isPed, vertexIndex, fixedRot);
		}

		public float GetDistance(Vector3 _1, Vector3 _2)
		{
			return _1.DistanceTo(_2);
		}

		public static void SaveCuts()
		{
			Config.SetValue("Completed_Heists", "Fleeca", XP.CompleteHeist_Fleeca);
			Config.SetValue("Completed_Heists", "Paleto", XP.CompleteHeist_PaletoBank);
			Config.SetValue("Completed_Heists", "PacificStandard", XP.CompleteHeist_PacStandard);
			Config.SetValue("Completed_Heists", "JewelStore", XP.CompleteHeist_JewelStore);
			Config.SetValue("Completed_Heists", "UnionDepository", XP.CompleteHeist_UnionDepository);
			Config.SetValue("Completed_Heists", "BikerClubhouse", XP.CompleteHeist_BikerClubhouse);
			Config.SetValue("Completed_Heists", "LaFuentaBlanca", XP.CompleteHeist_LaFuentaBlanca);
			Config.SetValue("Completed_Heists", "LaoubPrincessCargoship", XP.CompleteHeist_LaoubPrincessCargoship);
			Config.SetValue("Completed_Heists", "HumaineLabsRaid", XP.CompleteHeist_HumaineLabsRaid);
			Config.SetValue("Completed_Heists", "TheDiamondCasino", XP.CompleteHeist_TheDiamondCasino);
			Config.SetValue("Completed_Heists", "CayoPerico", XP.CompleteHeist_CayoPerico);
			Config.SetValue("Completed_Heists", "PrisonBreak", XP.CompleteHeist_PrisonBreak);
			Config.SetValue("Completed_Heists", "TheECUJob", XP.CompleteHeist_TheECUJob);
			Config.SetValue("Completed_Heists", "TheBillionariesParty", XP.CompleteHeist_TheBillionariesParty);
			Config.SetValue("Completed_Heists", "FederalReserveTrain", XP.CompleteHeist_FederalReserveTrain);
			Config.SetValue("Completed_Heists", "TheLexingtonRaid", XP.CompleteHeist_TheLexingtonRaid);
			Config.SetValue("Completed_Heists", "TheCartelYacht", XP.CompleteHeist_TheCartelYacht);
			Config.SetValue("Completed_Heists", "The24/7Job", XP.CompleteHeist_The247Job);
			Config.SetValue("Completed_Heists", "TheSilthouseJob", XP.CompleteHeist_TheSilthouseJob);
			Config.SetValue("Completed_Heists", "ThePrinthouseHeist", XP.CompleteHeist_ThePrinthouseHeist);
			Config.SetValue("Completed_Heists", "TheMazebankSavingsBankHeist", XP.CompleteHeist_TheMazebankSavingsBankHeist);
			Config.SetValue("Completed_Heists", "TheBelkraneJob", XP.CompleteHeist_TheBelkraneJob);
			Config.SetValue("Completed_Heists", "TheGefangnisArtHeist", XP.CompleteHeist_TheGefangnisArtHeist);
			Config.SetValue("Completed_Heists", "TheBettaBankHeist", XP.CompleteHeist_TheBettaBankHeist);
			Config.SetValue("Completed_Heists", "TheDiamondJewelStoreHeist", XP.CompleteHeist_TheDiamondJewelStoreHeist);
			Config.SetValue("Misc", "TotalCrewEarned", TotalCrewEarned);
			Config.SetValue("Misc", "TotalPlayerEarned", TotalPlayerEarned);
			Config.Save();
		}

		public static void ChangeArcade()
		{
			if (ArcadeOwned != 0)
			{
				if (ArcadeVersion == 1)
				{
					ArcadeGarageExitOnFootSpawnHeading = 37f;
					ArcadeGarageExitOnFootSpawn = new Vector3(-231.7233f, 6235.084f, 30.7f);
					ArcadeGarageEnter = new Vector3(-239.8f, 6230.6f, 30.7f);
					ArcadeGarageEnterHeading = 52f;
					ArcadeBusinessEnter = new Vector3(-248.5227f, 6211.846f, 30.7f);
				}
				if (ArcadeVersion == 2)
				{
					ArcadeGarageExitOnFootSpawnHeading = 171f;
					ArcadeGarageExitOnFootSpawn = new Vector3(1719.45f, 4759.939f, 42.05f);
					ArcadeGarageEnter = new Vector3(1712.3f, 4756.4f, 40.5f);
					ArcadeGarageEnterHeading = 93f;
					ArcadeBusinessEnter = new Vector3(1694.762f, 4785.07f, 40.5f);
				}
				if (ArcadeVersion == 3)
				{
					ArcadeGarageExitOnFootSpawnHeading = 103f;
					ArcadeGarageExitOnFootSpawn = new Vector3(726.1102f, -828.4044f, 24.77f);
					ArcadeGarageEnter = new Vector3(723.3f, -822.1f, 23.2f);
					ArcadeGarageEnterHeading = -177f;
					ArcadeBusinessEnter = new Vector3(759.1611f, -815.9539f, 25.2f);
				}
				if (ArcadeVersion == 4)
				{
					ArcadeGarageExitOnFootSpawnHeading = -37f;
					ArcadeGarageExitOnFootSpawn = new Vector3(-99.68f, -1782.821f, 28.5f);
					ArcadeGarageEnter = new Vector3(-104.6f, -1776.4f, 28.1f);
					ArcadeGarageEnterHeading = -34f;
					ArcadeBusinessEnter = new Vector3(-116.6468f, -1772.006f, 28.5f);
				}
				if (ArcadeVersion == 5)
				{
					ArcadeGarageExitOnFootSpawnHeading = 33f;
					ArcadeGarageExitOnFootSpawn = new Vector3(-1291.58f, -280.289f, 37.8f);
					ArcadeGarageEnter = new Vector3(-1288.7f, -274.6f, 37.8f);
					ArcadeGarageEnterHeading = 28f;
					ArcadeBusinessEnter = new Vector3(-1269.125f, -305.5237f, 36f);
				}
				if (ArcadeVersion == 6)
				{
					ArcadeGarageExitOnFootSpawnHeading = 169f;
					ArcadeGarageExitOnFootSpawn = new Vector3(-613.0292f, 278.7012f, 81.7f);
					ArcadeGarageEnter = new Vector3(-618.2f, 282.6f, 80.5f);
					ArcadeGarageEnterHeading = 177f;
					ArcadeBusinessEnter = new Vector3(-600.9061f, 279.5053f, 81.7f);
				}
				if (ArcadeVersion == 7)
				{
					ArcadeGarageExitOnFootSpawnHeading = -90f;
					ArcadeGarageExitOnFootSpawn = new Vector3(6444.8f, -2902.3f, 21.3f);
					ArcadeGarageEnter = new Vector3(6446.91f, -2905.8f, 20.91f);
					ArcadeGarageEnterHeading = -90f;
					ArcadeBusinessEnter = new Vector3(6444.8f, -2902.3f, 21.3f);
				}
				if (Arcade != null)
				{
					Arcade.Delete();
				}
				Arcade = GlobalHelper.CreateBlip(ArcadeBusinessEnter);
				Arcade.Sprite = BlipSprite.RCWorkshop;
				Arcade.Color = BlipColor.Yellow;
				Arcade.Name = "Single Player Heists Arcade";
				Arcade.IsShortRange = true;
			}
		}

		public void ResetBoughtVehicle()
		{
			Random random = new Random();
			VehicleColor value = DefaultColours[random.Next(0, DefaultColours.Count)];
			SC.Config.SetValue("Configurations", "Roof", -1);
			SC.Config.SetValue("Configurations", "AerialsId", -1);
			SC.Config.SetValue("Configurations", "AirfilterId", -1);
			SC.Config.SetValue("Configurations", "ArchCoverId", -1);
			SC.Config.SetValue("Configurations", "ArmorId", -1);
			SC.Config.SetValue("Configurations", "BackWheelsId", -1);
			SC.Config.SetValue("Configurations", "BrakesId", -1);
			SC.Config.SetValue("Configurations", "ColumnShifterLeversId", -1);
			SC.Config.SetValue("Configurations", "DashboardId", -1);
			SC.Config.SetValue("Configurations", "DialDesignId", -1);
			SC.Config.SetValue("Configurations", "DoorSpeekersId", -1);
			SC.Config.SetValue("Configurations", "EngineId", -1);
			SC.Config.SetValue("Configurations", "EngineBlockId", -1);
			SC.Config.SetValue("Configurations", "ExaustId", -1);
			SC.Config.SetValue("Configurations", "FenderId", -1);
			SC.Config.SetValue("Configurations", "FrameId", -1);
			SC.Config.SetValue("Configurations", "FrontBumperId", -1);
			SC.Config.SetValue("Configurations", "FrontWheelsId", -1);
			SC.Config.SetValue("Configurations", "GrilleId", -1);
			SC.Config.SetValue("Configurations", "HoodId", -1);
			SC.Config.SetValue("Configurations", "HornsId", -1);
			SC.Config.SetValue("Configurations", "HydralicsId", -1);
			SC.Config.SetValue("Configurations", "LiveryId", -1);
			SC.Config.SetValue("Configurations", "SecondaryVehicleLivery", -1);
			SC.Config.SetValue("Configurations", "OrnamentsId", -1);
			SC.Config.SetValue("Configurations", "PlaquesId", -1);
			SC.Config.SetValue("Configurations", "Plateholder", -1);
			SC.Config.SetValue("Configurations", "RearBumperId", -1);
			SC.Config.SetValue("Configurations", "RightFenderId", -1);
			SC.Config.SetValue("Configurations", "SeatsId", -1);
			SC.Config.SetValue("Configurations", "speakers", -1);
			SC.Config.SetValue("Configurations", "speakers", -1);
			SC.Config.SetValue("Configurations", "SpoilersId", -1);
			SC.Config.SetValue("Configurations", "SteeringWheelId", -1);
			SC.Config.SetValue("Configurations", "StrutId", -1);
			SC.Config.SetValue("Configurations", "RoofId", -1);
			SC.Config.SetValue("Configurations", "SuspensionId", -1);
			SC.Config.SetValue("Configurations", "TransmissionId", -1);
			SC.Config.SetValue("Configurations", "TrimDesignId", -1);
			SC.Config.SetValue("Configurations", "TrimDesignId", -1);
			SC.Config.SetValue("Configurations", "TrunkId", -1);
			SC.Config.SetValue("Configurations", "VanityPlatesId", -1);
			SC.Config.SetValue("Configurations", "WindowId", -1);
			SC.Config.SetValue("Configurations", "TintId", VehicleWindowTint.None);
			SC.Config.SetValue("Configurations", "Wheelcolour", VehicleColor.MatteBlack);
			SC.Config.SetValue("Configurations", "DashBoardColour", VehicleColor.MatteBlack);
			SC.Config.SetValue("Configurations", "PearlCent", VehicleColor.MatteBlack);
			SC.Config.SetValue("Configurations", "NeonColour", Color.Black);
			SC.Config.SetValue("Configurations", "PlanteNo", "");
			SC.Config.SetValue("Configurations", "CustomTiresOn", false);
			SC.Config.SetValue("Configurations", "Blades", -1);
			SC.Config.SetValue("Configurations", "HasXenonLights", false);
			SC.Config.SetValue("Configurations", "TurboActive", false);
			SC.Config.SetValue("Configurations", "LightColor", -1);
			SC.Config.SetValue("Configurations", "NeonColor", Color.Black);
			SC.Config.SetValue("Configurations", "RightNeonOn", false);
			SC.Config.SetValue("Configurations", "LeftNeonOn", false);
			SC.Config.SetValue("Configurations", "FrontNeonOn", false);
			SC.Config.SetValue("Configurations", "BackNeonOn", false);
			SC.Config.SetValue("Configurations", "Wheelcolour", VehicleColor.MatteBlack);
			SC.Config.SetValue("Configurations", "DashBoardColour", VehicleColor.MatteBlack);
			SC.Config.SetValue("Configurations", "TireSmokeColor", Color.Black);
			SC.Config.SetValue("Configurations", "TireSmoke", false);
			SC.Config.SetValue("Configurations", "TrimColor", VehicleColor.MatteBlack);
			SC.Config.SetValue("Configurations", "BulletProofTires", false);
			SC.Config.SetValue("Configurations", "WheelType", -1);
			SC.Config.SetValue("Configurations", "UseCustomPrimaryColor", false);
			SC.Config.SetValue("Configurations", "PrimaryColor", value);
			SC.Config.SetValue("Configurations", "UseCustomSecondaryColor", false);
			SC.Config.SetValue("Configurations", "SecondaryColor", value);
			SC.Config.Save();
		}

		public Model RequestModel(string Name)
		{
			Model result = new Model(Name);
			result.Request(10000);
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

		public Model RequestModel(int Name)
		{
			Model result = new Model(Name);
			result.Request(2000);
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

		public Model RequestModel(VehicleHash Name)
		{
			Model result = new Model(Name);
			result.Request(10000);
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

		public Model RequestModel(PedHash Name)
		{
			Model result = new Model(Name);
			result.Request(10000);
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

		public void SpawnProp(string Prop, Vector3 Pos, Vector3 Rot)
		{
			Model model = new Model(Prop);
			model.Request(250);
			if (model.IsInCdImage && model.IsValid)
			{
				while (!model.IsLoaded)
				{
					Script.Wait(50);
				}
				GlobalHelper.CreateProp(model, Pos, Rot, true, false);
				Prop[] nearbyProps = World.GetNearbyProps(Pos, 60f, model);
				Prop[] array = nearbyProps;
				foreach (Prop prop in array)
				{
					prop.SetNoCollision(prop, false);
					prop.LodDistance = 3000;
					prop.IsInvincible = true;
					Props.Add(prop);
				}
				Script.Wait(1);
			}
			model.MarkAsNoLongerNeeded();
		}

		public int DrawFIBHack(string amt)
		{
			iLocal_8077 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "MORGUE_LAPTOP");
			if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, iLocal_8077))
			{
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, amt, "SET_PROGRESS_PERCENT");
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 0);
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
				return 1;
			}
			return 0;
		}

		private void drawSprite2(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha)
		{
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, screenX, screenY, width, height, 0, r, g, b, alpha, 0);
			}
		}

		private int check(float name)
		{
			if (name >= fLocal_240 && name <= fLocal_239)
			{
				return 1;
			}
			return 0;
		}

		private int activatescale(string scale)
		{
			scaleformloading = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, scale);
			if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, scaleformloading))
			{
				return 1;
			}
			return 0;
		}

		private void HeistBoardSetup()
		{
			ArcadeBoardset = new Scaleform("CASINO_HEIST_BOARD_SETUP");
			ArcadeBoardset.CallFunction("HIDE_OVERLAY");
			ArcadeBoardset.CallFunction("SET_BUTTON_VISIBLE", 12, true);
			ArcadeBoardset.CallFunction("SET_BUTTON_VISIBLE", 11, true);
			ArcadeBoardset.CallFunction("SET_BUTTON_VISIBLE", 10, true);
			ArcadeBoardset.CallFunction("SET_BUTTON_VISIBLE", 8, targetgotten);
			ArcadeBoardset.CallFunction("SET_BUTTON_VISIBLE", 2, true);
			ArcadeBoardset.CallFunction("SET_TICK", 10, targetgotten);
			if (vaultcontstring == "Gold")
			{
				ArcadeBoardset.CallFunction("SET_BUTTON_IMAGE", 8, 2);
				ArcadeBoardset.CallFunction("SET_TARGET_TYPE", "GOLD");
			}
			if (vaultcontstring == "Paintings")
			{
				ArcadeBoardset.CallFunction("SET_BUTTON_IMAGE", 8, 3);
				ArcadeBoardset.CallFunction("SET_TARGET_TYPE", "PAINTINGS");
			}
			if (vaultcontstring == "Cash")
			{
				ArcadeBoardset.CallFunction("SET_BUTTON_IMAGE", 8, 1);
				ArcadeBoardset.CallFunction("SET_TARGET_TYPE", "CASH");
			}
			if (vaultcontstring == "Diamonds")
			{
				ArcadeBoardset.CallFunction("SET_BUTTON_IMAGE", 8, 4);
				ArcadeBoardset.CallFunction("SET_TARGET_TYPE", "DIAMONDS");
			}
			if (!targetgotten)
			{
				ArcadeBoardset.CallFunction("SET_PADLOCK", 5, true);
				ArcadeBoardset.CallFunction("SET_PADLOCK", 6, true);
				ArcadeBoardset.CallFunction("SET_PADLOCK", 7, true);
			}
			if (targetgotten)
			{
				ArcadeBoardset.CallFunction("SET_PADLOCK", 5, false);
				ArcadeBoardset.CallFunction("SET_PADLOCK", 6, false);
				ArcadeBoardset.CallFunction("SET_PADLOCK", 7, false);
				if (hselection == "Stealth")
				{
					ArcadeBoardset.CallFunction("SET_TICK", 5, true);
					ArcadeBoardset.CallFunction("SET_TICK", 6, false);
					ArcadeBoardset.CallFunction("SET_TICK", 7, false);
					ArcadeBoardset.CallFunction("SET_PADLOCK", 5, false);
					ArcadeBoardset.CallFunction("SET_PADLOCK", 6, true);
					ArcadeBoardset.CallFunction("SET_PADLOCK", 7, true);
				}
				if (hselection == "Big Con")
				{
					ArcadeBoardset.CallFunction("SET_TICK", 5, false);
					ArcadeBoardset.CallFunction("SET_TICK", 6, true);
					ArcadeBoardset.CallFunction("SET_TICK", 7, false);
					ArcadeBoardset.CallFunction("SET_PADLOCK", 5, true);
					ArcadeBoardset.CallFunction("SET_PADLOCK", 6, false);
					ArcadeBoardset.CallFunction("SET_PADLOCK", 7, true);
				}
				if (hselection == "Aggressive")
				{
					ArcadeBoardset.CallFunction("SET_TICK", 5, false);
					ArcadeBoardset.CallFunction("SET_TICK", 6, false);
					ArcadeBoardset.CallFunction("SET_TICK", 7, true);
					ArcadeBoardset.CallFunction("SET_PADLOCK", 5, true);
					ArcadeBoardset.CallFunction("SET_PADLOCK", 6, true);
					ArcadeBoardset.CallFunction("SET_PADLOCK", 7, false);
				}
			}
			ArcadeBoardset.CallFunction("CLEAR_TODO_LIST");
			ArcadeBoardset.CallFunction("ADD_TODO_LIST_ITEM", "Scope Out Vault Contents", targetgotten);
			ArcadeBoardset.CallFunction("ADD_TODO_LIST_ITEM", "Select Approach", true);
		}

		private void HeistBoardPrep()
		{
			ArcadeBoard = new Scaleform("CASINO_HEIST_BOARD_PREP");
			ArcadeBoard.CallFunction("HIDE_OVERLAY");
			ArcadeBoard.CallFunction("SET_BUTTON_VISIBLE", 12, false);
			ArcadeBoard.CallFunction("SET_BUTTON_VISIBLE", 11, false);
			ArcadeBoard.CallFunction("SET_BUTTON_VISIBLE", 10, false);
			ArcadeBoard.CallFunction("SET_BUTTON_VISIBLE", 8, false);
			ArcadeBoard.CallFunction("SET_BUTTON_VISIBLE", 2, false);
			ArcadeBoard.CallFunction("SET_HEADINGS", "THE BIG CON", "VAULT CONTENTS");
			ArcadeBoard.CallFunction("SET_BUTTON_GREYED_OUT", 5, true);
			ArcadeBoard.CallFunction("SET_PADLOCK", 5, true);
			ArcadeBoard.CallFunction("SET_TICK", 5, true);
			ArcadeBoard.CallFunction("SET_BUTTON_GREYED_OUT", 6, true);
			ArcadeBoard.CallFunction("SET_PADLOCK", 6, true);
			ArcadeBoard.CallFunction("SET_TICK", 6, true);
			ArcadeBoard.CallFunction("SET_SECURITY_PASS_VISIBLE", 2);
			ArcadeBoard.CallFunction("SET_TICK", 9, true);
			ArcadeBoard.CallFunction("SET_TICK", 3, true);
			ArcadeBoard.CallFunction("SET_TICK", 4, true);
			ArcadeBoard.CallFunction("SET_TICK", 7, true);
			ArcadeBoard.CallFunction("SET_TICK", 15, true);
			ArcadeBoard.CallFunction("SET_TICK", 14, true);
			ArcadeBoard.CallFunction("SET_TICK", 16, true);
			ArcadeBoard.CallFunction("SET_BUTTON_IMAGE", 5, 27);
			ArcadeBoard.CallFunction("SET_BUTTON_IMAGE", 13, 2);
			ArcadeBoard.CallFunction("ADD_APPROACH", 14, 21, "Gruppe 6 Truck", false, true, "ENTRY DISGUISE");
			ArcadeBoard.CallFunction("ADD_APPROACH", 15, 22, "Gruppe 6 Outfit", false, true, "ENTRY DISGUISE");
			ArcadeBoard.CallFunction("ADD_APPROACH", 16, 17, "Noose Outfit", false, false, "EXIT DISGUISE");
			ArcadeBoard.CallFunction("CLEAR_TODO_LIST");
			ArcadeBoard.CallFunction("ADD_TODO_LIST_ITEM", "Hacking Device", true);
			ArcadeBoard.CallFunction("ADD_TODO_LIST_ITEM", "Vault Keycards", true);
			ArcadeBoard.CallFunction("ADD_TODO_LIST_ITEM", "Gruppe 6 Truck", true);
			ArcadeBoard.CallFunction("ADD_TODO_LIST_ITEM", "Gruppe 6 Outfit", true);
			ArcadeBoard.CallFunction("ADD_TODO_LIST_ITEM", "Exit Disguise", true);
			ArcadeBoard.CallFunction("ADD_TODO_LIST_ITEM", "Patrol Routes", true);
			ArcadeBoard.CallFunction("ADD_TODO_LIST_ITEM", "Duggan Shipments", true);
			ArcadeBoard.CallFunction("ADD_TODO_LIST_ITEM", "Security Pass", true);
			ArcadeBoard.CallFunction("SET_SELECTION_ARROWS_VISIBLE", 13, false);
			ArcadeBoard.CallFunction("SET_SELECTION_ARROWS_VISIBLE", 5, false);
			ArcadeBoard.CallFunction("SET_SELECTION_ARROWS_VISIBLE", 6, false);
			ArcadeBoard.CallFunction("SET_SELECTION_ARROWS_VISIBLE", 7, false);
		}

		private void HeistBoardFinale()
		{
			if (vaultcontstring == "Cash")
			{
				gelelmese = 3750000;
			}
			if (vaultcontstring == "Paintings")
			{
				gelelmese = 4750000;
			}
			if (vaultcontstring == "Gold")
			{
				gelelmese = 5750000;
			}
			if (vaultcontstring == "Diamonds")
			{
				gelelmese = 8990000;
			}
			if (hselection == "Aggressive")
			{
				baz = 11;
				string text = "AGGRESSIVE";
			}
			if (hselection == "Stealth")
			{
				baz = 11;
				string text = "STEALTH";
			}
			if (hselection == "Big Con")
			{
				baz = 6;
				faz = 11;
				string text = "BIG CON";
			}
			finaleBoardset = new Scaleform("CASINO_HEIST_BOARD_FINALE");
			finaleBoardset.CallFunction("SET_BUTTON_VISIBLE", 8, false);
			finaleBoardset.CallFunction("SET_BUTTON_VISIBLE", 14, false);
			finaleBoardset.CallFunction("SET_BUTTON_VISIBLE", 9, false);
			finaleBoardset.CallFunction("SET_BUTTON_VISIBLE", 10, false);
			finaleBoardset.CallFunction("SET_BUTTON_VISIBLE", 11, false);
			finaleBoardset.CallFunction("SET_BUTTON_VISIBLE", 7, false);
			finaleBoardset.CallFunction("SET_BUTTON_IMAGE", 2, baz);
			finaleBoardset.CallFunction("SET_BUTTON_IMAGE", 3, faz);
			finaleBoardset.CallFunction("HIDE_OVERLAY");
			finaleBoardset.CallFunction("CLEAR_TODO_LIST");
			finaleBoardset.CallFunction("CLEAR_OPTIONAL_LIST");
			finaleBoardset.CallFunction("ADD_TODO_LIST_ITEM", "Entrance", false);
			finaleBoardset.CallFunction("ADD_TODO_LIST_ITEM", "Exit", false);
			if (hselection == "Aggressive")
			{
				if (baz == 11)
				{
					ent = "STAFF DOORS";
				}
				if (baz == 1)
				{
					ent = "MAIN DOORS";
				}
				if (faz == 11)
				{
					ex = "STAFF DOORS";
				}
				if (faz == 1)
				{
					ex = "MAIN DOORS";
				}
				finaleBoardset.CallFunction("SET_HEADINGS", hselection, vaultcontstring, 0, gelelmese, 110000, ent, ex, "HIGH LEVEL", "GRUPPE SECHS", "d");
			}
			if (hselection == "Stealth")
			{
				if (baz == 11)
				{
					ent = "STAFF DOORS";
				}
				if (baz == 1)
				{
					ent = "MAIN DOORS";
				}
				if (faz == 11)
				{
					ex = "STAFF DOORS";
				}
				if (faz == 1)
				{
					ex = "MAIN DOORS";
				}
				finaleBoardset.CallFunction("SET_HEADINGS", hselection, vaultcontstring, 0, gelelmese, 110000, ent, ex, "HIGH LEVEL", "GRUPPE SECHS", "d");
			}
			if (hselection == "Big Con")
			{
				if (baz == 6)
				{
					ent = "LOADING BAY";
				}
				if (baz == 1)
				{
					ent = "STAFF DOORS";
				}
				if (faz == 11)
				{
					ex = "STAFF DOORS";
				}
				if (faz == 1)
				{
					ex = "STAFF DOORS";
				}
				finaleBoardset.CallFunction("SET_HEADINGS", hselection, vaultcontstring, 0, gelelmese, 110000, ent, ex, "HIGH LEVEL", "GRUPPE SECHS", "d");
			}
		}

		public Main()
		{
			SinglePlayerHeists.UtilsMisc.Memory.Initialize();
			LoadIniFile("scripts//HKH_SinglePlayerHeists//Main.ini");
			LoadIniFileArcade("scripts//HKH_SinglePlayerHeists//Arcade//Arcade.ini");
			Arcades.Add(new Arcade("Pixel Pete's", 1235000f, new Vector3(-239.8f, 6230.6f, 30.7f)));
			Arcades.Add(new Arcade("Wonderama", 1565000f, new Vector3(1712.3f, 4756.4f, 40.5f)));
			Arcades.Add(new Arcade("Videogeddon", 1875000f, new Vector3(723.3f, -822.1f, 23.2f)));
			Arcades.Add(new Arcade("Warehouse", 2135000f, new Vector3(-104.6f, -1776.4f, 28.1f)));
			Arcades.Add(new Arcade("Insert Coin", 2345000f, new Vector3(-1288.7f, -274.6f, 37.8f)));
			Arcades.Add(new Arcade("Eight Bit", 2530000f, new Vector3(-618.2f, 282.6f, 80.5f)));
			HeistBlip = GlobalHelper.CreateBlip(MenuMarker);
			HeistBlip.Sprite = BlipSprite.HeistSetup;
			HeistBlip.Name = "Single Player Heists";
			HeistBlip.IsShortRange = true;
			HeistBlip.Color = BlipColor.Yellow;
			HeistBlip.Alpha = 0;
			if (LesterHasToldPlayerOfNewHeists == 0)
			{
				Random random = new Random();
				GameTimeToTriggerUINotification = Game.GameTime + random.Next(22000, 125000);
			}
			HeistBoardFinale();
			HeistBoardPrep();
			HeistBoardSetup();
			base.Tick += OnTick;
			Setup();
		}

		public static bool CheckInHeist()
		{
			bool result = false;
			if (Class1.PlayingPacificStandard || Class1.PlayingFleeca || Class1.PlayingPaleto || Class1.PlayingUnion || TheDiamondHeist.MissionOn || CayoPericoHeist.MissionOn || Class1.JewelStoreRobberyOn || Class1.InLaFuetaBlancaHeist || Class1.PlayingBikerClubhouseHeist || TheMazebankSavingsBankHeistcs.TheMazeBankSavngsBankHeistOn || ThePrinthouseHeist.ThePrinthouseHeistOn || TheBillionaresParty.TheBiliionaresPartyHeistOn || TheFederalReserveTrainHeist.TheFederalReserveTrainHeistOn || TheLexingtonRaid.TheLuxingtonRaidHeistOn || CargoShipHeist.CargoshipHeistOn || The247Job._247RobberyOn || TheSilthouseJob.SilthouseRobberyOn || HumainLabsOnline.MissionOn || ThePrisonBreak.InPrisonBreakHeist)
			{
				result = true;
			}
			return result;
		}

		public static void Start_SPH_Heist(int Heist)
		{
			Notification.Show("Started Heist From Player Companion " + Heist);
			if (Heist == 0)
			{
				The247Job.StartHeist();
			}
			if (Heist == 1)
			{
				TheSilthouseJob.StartHeist();
			}
			if (Heist == 3)
			{
				Class1.RunFleeca = 1;
			}
			if (Heist == 4)
			{
				Class1.RunPaleto = 1;
			}
			if (Heist == 6)
			{
				Class1.RunPacificStandard = 1;
			}
			if (Heist == 5)
			{
				Class1.RunJewelryStore = 1;
			}
			if (Heist == 7)
			{
				Class1.RunUnionDepositoryHeist = 1;
			}
			if (Heist == 8)
			{
				Class1.RunLaFauenteBlanca = 1;
			}
			if (Heist == 12)
			{
				CargoShipHeist.StartHeist();
			}
			if (Heist == 9)
			{
				Class1.RunBikerClubhouseHeist = 1;
			}
			if (Heist == 13)
			{
				TheDiamondHeist.RunHeist = 1;
			}
			if (Heist == 14)
			{
				CayoPericoHeist.RunHeist = 1;
			}
			if (Heist == 10)
			{
				ThePrisonBreak.RunHeist = 1;
			}
			if (Heist == 11)
			{
				HumainLabsOnline.RunHeist = 1;
			}
			if (Heist == 16)
			{
				TheLexingtonRaid.StartHeist();
			}
			if (Heist == 17)
			{
				YachtHeist.StartHeist();
			}
			if (Heist == 15)
			{
				TheBillionaresParty.StartHeist();
			}
			if (Heist == 18)
			{
				TheFederalReserveTrainHeist.StartHeist();
			}
			if (Heist == 20)
			{
				TheMazebankSavingsBankHeistcs.StartHeist();
			}
			if (Heist == 21)
			{
				ThePrinthouseHeist.StartHeist();
			}
		}

		public static int ReturnGameBuild()
		{
			FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo("GTA5.exe");
			return versionInfo.FileBuildPart;
		}

		public static void SaveSettingsData()
		{
			Config.SetValue("Misc", "Heist_Blip_Type", Class1.Heist_Blip_Type);
			Config.SetValue("Misc", "Disable_HeistIcon_Blips", Class1.Disable_HeistIcon_Blips);
			Config.SetValue("Misc", "Disable_Bodyguard_AiIntergration", Class1.Disable_Bodyguard_AiIntergration);
			Config.SetValue("Misc", "Disable_Single_Player_Pause", Class1.Disable_Single_Player_Pause);
			Config.SetValue("Misc", "DisableHacks", DisableHacks);
			Config.SetValue("Misc", "Show_Chevron_Markers_On_Loot", ManageRobberyAnims.ShowChevronMarkersOnLoot);
			Config.SetValue("Misc", "Block_Bodyguard_Mantle", ManageBodyguards.Block_Bodyguard_Mantle);
			Config.SetValue("Misc", "Show_Secondary_Safe_UI", ManageRobberyAnims.ShowSecondarySafeUI);
			Config.SetValue("Misc", "PaintingCut_Disable_Require_Input", ManageRobberyAnims.PaintingCut_Disable_Require_Input);
			Config.Save();
		}

		public void LoadIniFile(string iniName)
		{
			try
			{
				Config = ScriptSettings.Load(iniName);
				LesterHasToldPlayerOfNewHeists = Config.GetValue("Misc", "LesterHasToldPlayerOfNewHeists", LesterHasToldPlayerOfNewHeists);
				ToldPlayerHowToStartHeist = Config.GetValue("Misc", "ToldPlayerHowToStartHeist", ToldPlayerHowToStartHeist);
				RanTutorial = 1;
				TotalCrewEarned = Config.GetValue("Misc", "TotalCrewEarned", TotalCrewEarned);
				TotalPlayerEarned = Config.GetValue("Misc", "TotalPlayerEarned", TotalPlayerEarned);
				XP.PlayerLevel = Config.GetValue("PLAYER_RANK", "PlayerRank", XP.PlayerLevel);
				XP.CurrentXP = Config.GetValue("PLAYER_RANK", "CurrentXP", XP.CurrentXP);
				XP.PreviousXP = Config.GetValue("PLAYER_RANK", "PreviousXP", XP.PreviousXP);
				Class1.Fleeca_HeistMaxPay = Config.GetValue("Fleeca", "HeistMinPay", Class1.Fleeca_HeistMaxPay);
				Class1.Fleeca_HeistMaxPay = Config.GetValue("Fleeca", "HeistMaxPay", Class1.Fleeca_HeistMaxPay);
				Class1.Paleto_HeistMinPay = Config.GetValue("Paleto", "HeistMinPay", Class1.Paleto_HeistMinPay);
				Class1.Paleto_HeistMaxPay = Config.GetValue("Paleto", "HeistMaxPay", Class1.Paleto_HeistMaxPay);
				Class1.PacificStandard_HeistMinPay = Config.GetValue("PacificStandard", "HeistMinPay", Class1.PacificStandard_HeistMinPay);
				Class1.PacificStandard_HeistMaxPay = Config.GetValue("PacificStandard", "HeistMaxPay", Class1.PacificStandard_HeistMaxPay);
				Class1.JewelStore_HeistMinPay = Config.GetValue("JewelStoreJob", "HeistMinPay", Class1.JewelStore_HeistMinPay);
				Class1.JewelStore_HeistMaxPay = Config.GetValue("JewelStoreJob", "HeistMaxPay", Class1.JewelStore_HeistMaxPay);
				Class1.UnionDepository_HeistMinPay = Config.GetValue("UnionDepository", "HeistMinPay", Class1.UnionDepository_HeistMinPay);
				Class1.UnionDepository_HeistMaxPay = Config.GetValue("UnionDepository", "HeistMaxPay", Class1.UnionDepository_HeistMaxPay);
				Class1.LaFuenteBlanca_HeistMinPay = Config.GetValue("LaFuenteBlanca", "HeistMinPay", Class1.LaFuenteBlanca_HeistMinPay);
				Class1.LaFuenteBlanca_HeistMaxPay = Config.GetValue("LaFuenteBlanca", "HeistMaxPay", Class1.LaFuenteBlanca_HeistMaxPay);
				Class1.BikerClubhouse_HeistMinPay = Config.GetValue("BikerClubhouse", "HeistMinPay", Class1.BikerClubhouse_HeistMinPay);
				Class1.BikerClubhouse_HeistMaxPay = Config.GetValue("BikerClubhouse", "HeistMaxPay", Class1.BikerClubhouse_HeistMaxPay);
				CargoShipHeist.HeistMinPay = Config.GetValue("LaoubPrincessCargoshipHeist", "HeistMinPay", CargoShipHeist.HeistMinPay);
				CargoShipHeist.HeistMaxPay = Config.GetValue("LaoubPrincessCargoshipHeist", "HeistMaxPay", CargoShipHeist.HeistMaxPay);
				ThePrisonBreak.HeistStage = Config.GetValue("THEPRISONBREAK", "HEISTSTAGE", ThePrisonBreak.HeistStage);
				ThePrisonBreak.HeistMinPay = Config.GetValue("THEPRISONBREAK", "HeistMinPay", ThePrisonBreak.HeistMinPay);
				ThePrisonBreak.HeistMaxPay = Config.GetValue("THEPRISONBREAK", "HeistMaxPay", ThePrisonBreak.HeistMaxPay);
				HumainLabsOnline.HeistStage = Config.GetValue("HUMAINELABS", "HEISTSTAGE", HumainLabsOnline.HeistStage);
				HumainLabsOnline.HeistMinPay = Config.GetValue("HUMAINELABS", "HeistMinPay", HumainLabsOnline.HeistMinPay);
				HumainLabsOnline.HeistMaxPay = Config.GetValue("HUMAINELABS", "HeistMaxPay", HumainLabsOnline.HeistMaxPay);
				TheBillionaresParty.HeistMinPay = Config.GetValue("TheBillionaresParty", "HeistMinPay", TheBillionaresParty.HeistMinPay);
				TheBillionaresParty.HeistMaxPay = Config.GetValue("TheBillionaresParty", "HeistMinPay", TheBillionaresParty.HeistMaxPay);
				TheFederalReserveTrainHeist.HeistMinPay = Config.GetValue("TheFederalReserveTrainHeist", "HeistMinPay", TheFederalReserveTrainHeist.HeistMinPay);
				TheFederalReserveTrainHeist.HeistMaxPay = Config.GetValue("TheFederalReserveTrainHeist", "HeistMinPay", TheFederalReserveTrainHeist.HeistMaxPay);
				TheLexingtonRaid.HeistMinPay = Config.GetValue("TheLuxingtonRaid", "HeistMinPay", TheLexingtonRaid.HeistMinPay);
				TheLexingtonRaid.HeistMaxPay = Config.GetValue("TheLuxingtonRaid", "HeistMinPay", TheLexingtonRaid.HeistMaxPay);
				YachtHeist.HeistMinPay = Config.GetValue("TheCartelYachtHeist", "HeistMinPay", YachtHeist.HeistMinPay);
				YachtHeist.HeistMaxPay = Config.GetValue("TheCartelYachtHeist", "HeistMinPay", YachtHeist.HeistMaxPay);
				The247Job.HeistMinPay = Config.GetValue("24/7Job", "HeistMinPay", The247Job.HeistMinPay);
				The247Job.HeistMaxPay = Config.GetValue("24/7Job", "HeistMaxPay", The247Job.HeistMaxPay);
				TheSilthouseJob.HeistMinPay = Config.GetValue("TheSilthouseJob", "HeistMinPay", TheSilthouseJob.HeistMinPay);
				TheSilthouseJob.HeistMaxPay = Config.GetValue("TheSilthouseJob", "HeistMaxPay", TheSilthouseJob.HeistMaxPay);
				ThePrinthouseHeist.HeistMinPay = Config.GetValue("TheSilthouseJob", "HeistMinPay", ThePrinthouseHeist.HeistMinPay);
				ThePrinthouseHeist.HeistMaxPay = Config.GetValue("TheSilthouseJob", "HeistMaxPay", ThePrinthouseHeist.HeistMaxPay);
				TheMazebankSavingsBankHeistcs.HeistMinPay = Config.GetValue("TheMazebankSavingsBank", "HeistMinPay", TheMazebankSavingsBankHeistcs.HeistMinPay);
				TheMazebankSavingsBankHeistcs.HeistMaxPay = Config.GetValue("TheMazebankSavingsBank", "HeistMaxPay", TheMazebankSavingsBankHeistcs.HeistMaxPay);
				TheBetaBankHeist.HeistMinPay = Config.GetValue("TheBettaBankHeist", "HeistMinPay", TheBetaBankHeist.HeistMinPay);
				TheBetaBankHeist.HeistMaxPay = Config.GetValue("TheBettaBankHeist", "HeistMaxPay", TheBetaBankHeist.HeistMaxPay);
				TheDiamondJewelStore.HeistMinPay = Config.GetValue("TheDiamondJewelStore", "HeistMinPay", TheDiamondJewelStore.HeistMinPay);
				TheDiamondJewelStore.HeistMaxPay = Config.GetValue("TheDiamondJewelStore", "HeistMaxPay", TheDiamondJewelStore.HeistMaxPay);
				TheGefangnisArtHeist.HeistMinPay = Config.GetValue("TheGefangnisArtHeist", "HeistMinPay", TheGefangnisArtHeist.HeistMinPay);
				TheGefangnisArtHeist.HeistMaxPay = Config.GetValue("TheGefangnisArtHeist", "HeistMaxPay", TheGefangnisArtHeist.HeistMaxPay);
				ATM_Robberies.Enable_ATM_Robberies = Config.GetValue("ATM_Robberies", "Enable_ATM_Robberies", ATM_Robberies.Enable_ATM_Robberies);
				ATM_Robberies.MinSpawnAmt = Config.GetValue("ATM_Robberies", "Min_Drop_Amt", ATM_Robberies.MinSpawnAmt);
				ATM_Robberies.MinSpawnAmt = Config.GetValue("ATM_Robberies", "Max_Drop_Amt", ATM_Robberies.MaxSpawnAmt);
				ATM_Robberies.MinMoneyAmt = Config.GetValue("ATM_Robberies", "Min_Cash_Per_Drop", ATM_Robberies.MinMoneyAmt);
				ATM_Robberies.MaxMoneyAmt = Config.GetValue("ATM_Robberies", "Max_Cash_Per_Drop", ATM_Robberies.MaxMoneyAmt);
				XP.CompleteHeist_Fleeca = Config.GetValue("Completed_Heists", "Fleeca", XP.CompleteHeist_Fleeca);
				XP.CompleteHeist_PaletoBank = Config.GetValue("Completed_Heists", "Paleto", XP.CompleteHeist_PaletoBank);
				XP.CompleteHeist_PacStandard = Config.GetValue("Completed_Heists", "PacificStandard", XP.CompleteHeist_PacStandard);
				XP.CompleteHeist_JewelStore = Config.GetValue("Completed_Heists", "JewelStore", XP.CompleteHeist_JewelStore);
				XP.CompleteHeist_UnionDepository = Config.GetValue("Completed_Heists", "UnionDepository", XP.CompleteHeist_UnionDepository);
				XP.CompleteHeist_BikerClubhouse = Config.GetValue("Completed_Heists", "BikerClubhouse", XP.CompleteHeist_BikerClubhouse);
				XP.CompleteHeist_LaFuentaBlanca = Config.GetValue("Completed_Heists", "LaFuentaBlanca", XP.CompleteHeist_LaFuentaBlanca);
				XP.CompleteHeist_LaoubPrincessCargoship = Config.GetValue("Completed_Heists", "LaoubPrincessCargoship", XP.CompleteHeist_LaoubPrincessCargoship);
				XP.CompleteHeist_HumaineLabsRaid = Config.GetValue("Completed_Heists", "HumaineLabsRaid", XP.CompleteHeist_HumaineLabsRaid);
				XP.CompleteHeist_TheDiamondCasino = Config.GetValue("Completed_Heists", "TheDiamondCasino", XP.CompleteHeist_TheDiamondCasino);
				XP.CompleteHeist_CayoPerico = Config.GetValue("Completed_Heists", "CayoPerico", XP.CompleteHeist_CayoPerico);
				XP.CompleteHeist_PrisonBreak = Config.GetValue("Completed_Heists", "PrisonBreak", XP.CompleteHeist_PrisonBreak);
				XP.CompleteHeist_TheECUJob = Config.GetValue("Completed_Heists", "TheECUJob", XP.CompleteHeist_TheECUJob);
				XP.CompleteHeist_TheBillionariesParty = Config.GetValue("Completed_Heists", "TheBillionariesParty", XP.CompleteHeist_TheBillionariesParty);
				XP.CompleteHeist_FederalReserveTrain = Config.GetValue("Completed_Heists", "FederalReserveTrain", XP.CompleteHeist_FederalReserveTrain);
				XP.CompleteHeist_TheLexingtonRaid = Config.GetValue("Completed_Heists", "TheLexingtonRaid", XP.CompleteHeist_TheLexingtonRaid);
				XP.CompleteHeist_TheCartelYacht = Config.GetValue("Completed_Heists", "TheCartelYacht", XP.CompleteHeist_TheCartelYacht);
				TheDiamondHeist.HeistMinPay = Config.GetValue("TheDiamondHiest", "HeistMinPay", TheDiamondHeist.HeistMinPay);
				TheDiamondHeist.HeistMaxPay = Config.GetValue("TheDiamondHiest", "HeistMaxPay", TheDiamondHeist.HeistMaxPay);
				Class1.Heist_Blip_Type = Config.GetValue("Misc", "Heist_Blip_Type", Class1.Heist_Blip_Type);
				Class1.Disable_HeistIcon_Blips = Config.GetValue("Misc", "Disable_HeistIcon_Blips", Class1.Disable_HeistIcon_Blips);
				Class1.Disable_Bodyguard_AiIntergration = Config.GetValue("Misc", "Disable_Bodyguard_AiIntergration", Class1.Disable_Bodyguard_AiIntergration);
				Class1.Disable_Single_Player_Pause = Config.GetValue("Misc", "Disable_Single_Player_Pause", Class1.Disable_Single_Player_Pause);
				DisableHacks = Config.GetValue("Misc", "DisableHacks", DisableHacks);
				ManageBodyguards.Block_Bodyguard_Mantle = Config.GetValue("Misc", "Block_Bodyguard_Mantle", ManageBodyguards.Block_Bodyguard_Mantle);
				ManageRobberyAnims.ShowChevronMarkersOnLoot = Config.GetValue("Misc", "Show_Chevron_Markers_On_Loot", ManageRobberyAnims.ShowChevronMarkersOnLoot);
				ManageRobberyAnims.ShowSecondarySafeUI = Config.GetValue("Misc", "Show_Secondary_Safe_UI", ManageRobberyAnims.ShowSecondarySafeUI);
				ManageRobberyAnims.PaintingCut_Disable_Require_Input = Config.GetValue("Misc", "PaintingCut_Disable_Require_Input", ManageRobberyAnims.PaintingCut_Disable_Require_Input);
				Heists_Data.Clear();
				Heists_Data.Add(new HeistData(HeistID.The247Job, "~y~The 24/7 Job", (float)(The247Job.HeistMinPay + The247Job.HeistMaxPay) / 2f, new List<string> { "Head to the ~y~24/7 Store~w~", "Destory the ~r~Cameras~w~", "Hold up the Cleark", "~p~(OPTIONAL)~w~ find the safe codes", "Open the ~y~Safe", "Loot the ~g~Vault", "Escape the ~r~Cops" }, 2, new List<Vector2>
				{
					new Vector2(24.09662f, -1345.61792f),
					new Vector2(-3040.45874f, 583.6604f),
					new Vector2(-3243.98145f, 999.754944f),
					new Vector2(549.6568f, 2669.714f),
					new Vector2(1957.11951f, 3744.01855f),
					new Vector2(2676.37866f, 3279.907f),
					new Vector2(1728.27454f, 6416.90527f),
					new Vector2(2555.50854f, 380.511017f),
					new Vector2(372.610565f, 328.153625f)
				}));
				Heists_Data.Add(new HeistData(HeistID.TheSilthouseJob, "~y~The Silthouse Job", (float)(TheSilthouseJob.HeistMinPay + TheSilthouseJob.HeistMaxPay) / 2f, new List<string> { "Head to the ~y~Silthouse~w~", "Enter the ~y~Silthouse~w~", "~p~(Optional)~w~ raid the ~y~Silthouse's Secure Vaults~w~", "Loot the rest of the ~y~Silthouse~w~ for ~g~Vauluables", "Leave the ~y~Silthouse~w~ when ready", "Escape the ~r~Cops" }, 2, new List<Vector2>
				{
					new Vector2(-733.149f, 593.7691f),
					new Vector2(-1294.28f, 454.289978f),
					new Vector2(46.7127075f, 440.694458f),
					new Vector2(119.416794f, 564.0475f),
					new Vector2(-174.7513f, 502.5596f)
				}));
				Heists_Data.Add(new HeistData(HeistID.Fleeca, "~y~The Fleeca Heist", (float)(Class1.Fleeca_HeistMinPay + Class1.Fleeca_HeistMaxPay) / 2f, new List<string> { "Head to the ~y~Fleeca Bank~w~", "Destory the ~r~Cameras~w~", "Hack the ~w~Keypad", "Wait for the Vault to open", "Loot the ~g~Vault", "Escape the ~r~Cops" }, 2, new List<Vector2>
				{
					new Vector2(317.18f, -269.97f),
					new Vector2(152.41f, -1032.45f),
					new Vector2(-345.69f, -33.2f),
					new Vector2(-1217.544f, -321.25f),
					new Vector2(-2970.65f, 482.4f),
					new Vector2(1175.379f, 2693.28f)
				}));
				Heists_Data.Add(new HeistData(HeistID.Paleto, "~y~The Paleto Job", (float)(Class1.Paleto_HeistMinPay + Class1.Paleto_HeistMaxPay) / 2f, new List<string> { "Head to the ~y~Paleto Bank~w~", "Place ~o~Thermite~w~ on the Vault door", "Wait for the Vault to open", "Loot the ~g~Vault", "Escape the ~r~Cops" }, 2, new List<Vector2>
				{
					new Vector2(-128f, 6443.71f)
				}));
				Heists_Data.Add(new HeistData(HeistID.PacificStandard, "~y~The Pacific Standard Heist", (float)(Class1.PacificStandard_HeistMinPay + Class1.PacificStandard_HeistMaxPay) / 2f, new List<string> { "Head to the ~y~Pacific Standard Bank~w~", "Find the ~b~Manager~w~", "Destory The ~r~Cameras~w~", "Hack the ~d~Keypad~w~", "Wait for the Vault to open", "Loot the ~g~Vault", "Escape the ~r~Cops" }, 2, new List<Vector2>
				{
					new Vector2(223.06f, 207.39f)
				}));
				Heists_Data.Add(new HeistData(HeistID.JewelStore, "~y~The Jewel Store Job", (float)(Class1.JewelStore_HeistMinPay + Class1.JewelStore_HeistMaxPay) / 2f, new List<string> { "Head to the ~y~Jewel Store~w~", "Take out the ~r~Guards", "Take out the ~r~Cameras", "Loot the ~g~Store", "Escape the ~r~Cops~w~" }, 2, new List<Vector2>
				{
					new Vector2(-673.6f, -227.4f)
				}));
				Heists_Data.Add(new HeistData(HeistID.UnionDepository, "~y~The Union Depository Heist", (float)(Class1.UnionDepository_HeistMinPay + Class1.UnionDepository_HeistMaxPay) / 2f, new List<string> { "Approach the ~y~Union Depository~w~", "Scout for ~g~Assets/Intel~w~", "Find a way into the ~y~Union Depository's Vault~w~", "Loot the ~g~Vault", "Escape the ~r~Cops" }, 2, new List<Vector2>
				{
					new Vector2(116.79f, -442.26f),
					new Vector2(-92.16f, -1032.582f),
					new Vector2(-2.13f, -625.113f),
					new Vector2(-1262.219f, -2294.15f),
					new Vector2(149.87f, -3105.002f),
					new Vector2(872.79f, -1350.214f),
					new Vector2(-112.76f, -10.87f)
				}));
				Heists_Data.Add(new HeistData(HeistID.LaFuenteBlanca, "~y~The La Fuenta Blanca Heist", (float)(Class1.LaFuenteBlanca_HeistMinPay + Class1.LaFuenteBlanca_HeistMaxPay) / 2f, new List<string> { "Approach the ~y~Martin Madrazo's House~w~", "Destroy the ~r~Cameras", "hack the ~d~Keypad~w~", "Loot the ~g~House", "Escape the ~r~Cops" }, 2, new List<Vector2>
				{
					new Vector2(1309.312f, 1109.406f)
				}));
				Heists_Data.Add(new HeistData(HeistID.LaoubPrincessCargoshipHeist, "~y~The La'oub Princess Cargoship Heist", (float)(CargoShipHeist.HeistMinPay + CargoShipHeist.HeistMaxPay) / 2f, new List<string> { "Approach the ~y~La'oub Princess Cargoship~w~", "Scout for ~g~Assets/Intel~w~", "Find a way to open the ~g~La'oub Princess' Containers~w~", "Find the ~g~Panthere Statue~w~", "Leave ~y~La'oub Princess Cargoship~w~ the Escape the ~r~Cops" }, 1, new List<Vector2>
				{
					new Vector2(-361.5388f, -4088.099f),
					new Vector2(1485.329f, 1164.12793f),
					new Vector2(-569.7184f, -1671.93567f),
					new Vector2(-44.0454521f, -2719.02124f),
					new Vector2(-490.266f, 6446.18f),
					new Vector2(5000.405f, -5163.70654f),
					new Vector2(5067.588f, -4590.59229f)
				}));
				Heists_Data.Add(new HeistData(HeistID.BikerClubhouse, "~y~The Biker Clubhouse Heist", (float)(Class1.BikerClubhouse_HeistMinPay + Class1.BikerClubhouse_HeistMaxPay) / 2f, new List<string> { "Head to the ~y~Biker Clubhouse~w~ ", "Hack The ~d~Keypad~w~", "Get past the ~d~Security Barrier", "Place ~o~Thermite on the vault door~w~", "Loot the ~g~Clubhouse's Vault", "Exit the ~y~Clubhouse", "Escape the ~r~Cops" }, 2, new List<Vector2>
				{
					new Vector2(291.87f, 2652.5f)
				}));
				Heists_Data.Add(new HeistData(HeistID.TheDiamondCasino, "~y~The Diamond Casino Heist", (float)(TheDiamondHeist.HeistMinPay + TheDiamondHeist.HeistMaxPay) / 2f, new List<string> { "Scout the ~y~Diamond Casino~w~", "Aquire ~g~C4~w~ for the ~y~Vault Door~w~~n~ -Aquire A ~d~Stockade~w~ if going in as ~o~Grouppe6~w~", "Aquire ~g~VIP Passes~w~ if entering/exiting throught the ~y~VIP follor", "Aquire a ~b~Helicopter~w~ or a ~b~Land Vehicle~w~", "Aquire a ~d~Bugstars Van~w~ if Entering as ~o~Bugsters~w~", "Enter the ~y~Diamond Casino~w~", "Head to the ~y~Vault~w~", "Loot the ~g~Vault~w~", "Leave ~y~Diamond Casino~w~ and escape the ~r~Cops" }, 4, new List<Vector2>
				{
					new Vector2(965.71f, 42.24f),
					new Vector2(461.5811f, -3257.675f),
					new Vector2(791.5f, -1388.8f),
					new Vector2(962.9f, 189.9f),
					new Vector2(-987.7f, -2992.6f),
					new Vector2(149.87f, -3105.002f)
				}));
				Heists_Data.Add(new HeistData(HeistID.CayoPericoHeist, "~y~The Cayo Perico Heist", (CayoPericoHeist.HeistMinPay + CayoPericoHeist.HeistMaxPay) / 2f, new List<string>
				{
					"Scout ~y~Cayo Perico~w~ for Intel for the Finale", "Take out the ~r~Loan Sharks~w~ in ~y~Vinewood Hills", "Find the ~g~Safe Codes~w~ at the ~y~Diamond Casino~w~", "Steal ~g~C4 Charges~w~ from the ~y~O'Neils Farm~w~", "Locate a ~g~Fingerprint Scanner~w~ in ~y~FIB Warehouse~w~", "Locate a ~g~Signal Jammer~w~ in a ~y~Merryweather Submarine~w~ in Paleto Bay", "Aquire a ~b~Vehicle~w~ to get to ~y~Cayo Perico", "Get to the ~y~Compound", "Get into the ~y~Vault", "Crack the ~y~Safe~w~ and Aquire the ~g~Documents",
					"Escape Cayo Perico"
				}, 4, new List<Vector2>
				{
					new Vector2(4421.143f, -4492.969f),
					new Vector2(187.55f, -1478.948f),
					new Vector2(661.3575f, -423.3103f),
					new Vector2(-1291.875f, 6859.449f),
					new Vector2(-66.47f, 490.83f),
					new Vector2(925.7f, 45.8f)
				}));
				Heists_Data.Add(new HeistData(HeistID.ThePrisonBreak, "~y~The Prison Break", (float)(ThePrisonBreak.HeistMinPay + ThePrisonBreak.HeistMaxPay) / 2f, new List<string> { "Head to ~y~McKenzie Airfield~w~ and steal the ~b~Velum", "Aquire the ~g~Prison Schedule~w~", "Steal the ~b~Prison Bus~w~ and deliver it to the ~y~Drop Off", "Recover ~y~Rashkovsky's Casco~w~ from the docks", "Take out the ~r~Lawyers~w~ who found ~y~Rashkovsky~w~ guilty", "Rescue ~y~Rashkovsky~w~ From Prison and Escort him to the ~b~Velum" }, 1, new List<Vector2>
				{
					new Vector2(1786.2f, 2608.6f),
					new Vector2(2210.764f, 2725.733f),
					new Vector2(438.744f, -995.92f),
					new Vector2(915.0944f, -2880.744f),
					new Vector2(235.74f, -328.3463f),
					new Vector2(-1813.144f, 395.5246f)
				}));
				Heists_Data.Add(new HeistData(HeistID.TheHumaineLabs, "~y~The Humaine Labs Raid ", (float)(HumainLabsOnline.HeistMinPay + HumainLabsOnline.HeistMaxPay) / 2f, new List<string> { "Aquire ~g~Keycards", "Aquire ~b~Insurgent", "Aquire ~b~Hydra", "Aquire ~y~EMP", "Head out to the Humaine Labs in the ~b~Insurgent", "????" }, 1, new List<Vector2>
				{
					new Vector2(3630.8f, 3771.4f),
					new Vector2(3095.733f, -4801.459f),
					new Vector2(2940.36f, 2809.564f),
					new Vector2(129.1287f, -1075.008f)
				}));
				string text = TheLexingtonRaid.PercentNeedTake.ToString("N");
				text = text.Replace(",", ".");
				Heists_Data.Add(new HeistData(HeistID.TheLuxigntonRaid, "~y~The Luxington Raid ", (float)(TheLexingtonRaid.HeistMinPay + TheLexingtonRaid.HeistMaxPay) / 2f, new List<string>
				{
					"~p~(Optional)~w~ Aquire ~g~Merryweather Keycard",
					"~p~(Optional)~w~ Aquire ~g~C4",
					"~p~(Optional)~w~ Aquire a ~g~Sam Jammer",
					"~p~(Optional)~w~ Aquire a ~g~Trojan Vehicle~w~ From a ~y~Merryweather Cargoship~w~",
					"Find and Aquire at least ~y~$" + text + " of sensetive data~w~ on the Aircraft Carrier"
				}, 1, new List<Vector2>
				{
					new Vector2(-2360.08862f, 3248.93433f),
					new Vector2(461.134064f, -988.7814f),
					new Vector2(331.65f, -1119.551f),
					new Vector2(1130.47f, -1304.74f),
					new Vector2(550.94f, -3052.301f),
					new Vector2(-469f, -2672f),
					new Vector2(3050f, -4650f)
				}));
				Heists_Data.Add(new HeistData(HeistID.TheCartelYachtHeist, "~y~The Cartel Yacht Heist ", (float)(YachtHeist.HeistMinPay + YachtHeist.HeistMaxPay) / 2f, new List<string> { "~p~(Optional)~w~ Aquire a ~g~Disposable Network Kecyard", "~p~(Optional)~w~ Aquire ~g~C4", "~p~(Optional)~w~ bribe the ~b~Pilot~w~ to access the Yacht as a VIP", "Aquire Access to the ~y~Yachts Primary Vault", "~p~(Optional)~w~ Aquire Access to the ~y~Yachts Secondary Vault", "~p~(Optional)~w~ Aquire Access to the ~y~Yachts Terciary Vault" }, 1, new List<Vector2>
				{
					new Vector2(-1418.21f, 6749.811f),
					new Vector2(-2573.35254f, 1861.861f),
					new Vector2(1560.25562f, 6678.969f),
					new Vector2(66.07f, 6654.66f),
					new Vector2(-388.814f, 4340.98f)
				}));
				Heists_Data.Add(new HeistData(HeistID.TheBillionairesParty, "~y~The Billionaries Party", (float)(TheBillionaresParty.HeistMinPay + TheBillionaresParty.HeistMaxPay) / 2f + 4240000f, new List<string> { "~p~(Optional)~w~ Hack the ~r~Head of Security's Phone~w~ to force them to export the vehicles in a ~y~Semi Truck~w~", "~p~(Optional)~w~ Aquire a ~g~VIP Pass", "~p~(Optional)~w~ Aquire a ~g~DJ Pass", "~p~(Optional)~w~ Aquire Head of Security's ~g~Vault Access Keycard & Biometric ID", "~p~(Optional)~w~ Arrange a deal with Cartel members", "~p~(Optional)~w~ Gain access to the ~y~Billionaires Vault", "Steal ~p~AT LEAST~w~ one of the ~y~Billionaires Rides", "~p~(Optional)~w~ Steal all four of the ~y~Billionaires Rides" }, 1, new List<Vector2>
				{
					new Vector2(-1570.428f, 97.79581f),
					new Vector2(-1804.7f, 428.6f),
					new Vector2(-1371.14954f, 735.5286f),
					new Vector2(373.75f, 427.84f),
					new Vector2(977.29f, 16.8f),
					new Vector2(728.2622f, -2912.65723f)
				}));
				Heists_Data.Add(new HeistData(HeistID.TheFederalReserveTrainHeist, "~y~The Federal Reserve Train Heist ", (float)(TheFederalReserveTrainHeist.HeistMinPay + TheFederalReserveTrainHeist.HeistMaxPay) / 2f, new List<string> { "~p~(Optional)~w~ Aquire a ~g~Grinder", "~p~(Optional)~w~ Aquire ~g~C4", "~p~(Optional)~w~ Aquire ~g~Thermite", "Find a way to get access to the Train Engineers", "~p~(Optional)~w~ Stop the train at a ~y~Designated Point~w~", "~p~(Optional)~w~ Destory the Trains ~r~Brake Cylinders~w~ to Stop the ~y~Train", "~p~(Optional)~w~ Force the ~y~Train~w~ to stop at ~b~Davis Quarry" }, 1, new List<Vector2>
				{
					new Vector2(1350.91f, 4357.684f),
					new Vector2(2444.424f, 4974.523f),
					new Vector2(2632.059f, 2934.051f),
					new Vector2(2618.774f, 1691.839f),
					new Vector2(-1001.92f, -414.39f),
					new Vector2(-160.06f, -990.12f)
				}));
				string text2 = ThePrinthouseHeist.PercentNeedTake.ToString("N");
				text2 = text2.Replace(",", ".");
				Heists_Data.Add(new HeistData(HeistID.ThePrinthouseHeist, "~y~The Printhouse Heist", (float)(ThePrinthouseHeist.HeistMinPay + ThePrinthouseHeist.HeistMaxPay) / 2f, new List<string>
				{
					"Head over to the ~y~Printhouse~w~",
					"Take out the ~r~Guards & Cameras~w~",
					"~p~(Optional)~w~ Find a way to Open the ~y~Vault.~W~",
					"~p~(Optional)~w~ Find a way to Open the ~y~Money Crate Storage.~W~",
					"~p~(Optional)~w~ Push a ~y~Money Crate~w~ into the ~d~Stockade~W~",
					"Loot at least $" + text2,
					"Leave the ~y~Printhouse",
					"Escape the ~r~Cops",
					"~p~(Exited With Stockade)~w~ Escape the ~r~Cops~w~ with the ~d~Stockade.~w~",
					"~p~(Exited With Stockade)~w~ return the ~d~Stockade~w~ to the ~y~Arcade"
				}, 1, new List<Vector2>
				{
					new Vector2(-247.423141f, -848.0635f)
				}));
				text2 = TheMazebankSavingsBankHeistcs.PercentNeedTake.ToString("N");
				text2 = text2.Replace(",", ".");
				Heists_Data.Add(new HeistData(HeistID.TheMazeBankSavingsBankHeist, "~y~The Mazebank Savings Bank Heist", (float)(TheMazebankSavingsBankHeistcs.HeistMinPay + TheMazebankSavingsBankHeistcs.HeistMaxPay) / 2f, new List<string>
				{
					"Head over to the ~y~Mazebank Saving Branch",
					"Take out the ~r~Guards & Cameras~w~",
					"~p~(Optional)~w~ Hack the ~y~Keypad~w~ to access the ~y~Vault.~W~",
					"~p~(Optional)~w~place a ~o~Thermite Charge~w~ on the ~y~Vault.~W~",
					"~p~(Optional)~w~ Deactivate the~y~Vaults Magnetic Locks~W~",
					"Loot at least $" + text2,
					"Leave the ~y~Mazebank Saving Branch",
					"Escape the ~r~Cops"
				}, 1, new List<Vector2>
				{
					new Vector2(-1310.78625f, -835.805969f)
				}));
				text2 = TheGefangnisArtHeist.PercentNeedTake.ToString("N");
				text2 = text2.Replace(",", ".");
				Heists_Data.Add(new HeistData(HeistID.TheGefangnisArtHeist, "~y~The Gefangnis Art Heist~w~", (float)(TheGefangnisArtHeist.HeistMinPay + TheGefangnisArtHeist.HeistMaxPay) / 2f, new List<string>
				{
					"Head over to the ~y~Gefangnis Art Museum",
					"Take out the ~r~Guards & Cameras~w~",
					"Loot at least $" + text2,
					"Leave the ~y~Gefangnis Art Museum",
					"Escape the ~r~Cops"
				}, 1, new List<Vector2>
				{
					new Vector2(-657.5f, -892.46f)
				}));
				text2 = TheDiamondJewelStore.PercentNeedTake.ToString("N");
				text2 = text2.Replace(",", ".");
				Heists_Data.Add(new HeistData(HeistID.TheDiamondJewelStoreHeist, "~y~The Diamond Jewel Store Heist", (float)(TheDiamondJewelStore.HeistMinPay + TheDiamondJewelStore.HeistMaxPay) / 2f, new List<string>
				{
					"Head over to the ~y~Diamond Jewel Store",
					"Take out the ~r~Guards & Cameras~w~",
					"~p~(Optional)~w~ Open the ~y~Vault~w~",
					"Loot at least $" + text2,
					"Leave the ~y~Gefangnis Art Museum",
					"Escape the ~r~Cops"
				}, 1, new List<Vector2>
				{
					new Vector2(366.28f, -836.18f)
				}));
				text2 = TheBetaBankHeist.PercentNeedTake.ToString("N");
				text2 = text2.Replace(",", ".");
				Heists_Data.Add(new HeistData(HeistID.TheBettaBankHeist, "~y~The Betta Bank Heist ", (float)(TheBetaBankHeist.HeistMinPay + TheBetaBankHeist.HeistMaxPay) / 2f, new List<string>
				{
					"Head over to the ~y~Betta Bank",
					"Take out the ~r~Cameras~w~",
					"Open the first ~y~Security Gate~w~",
					"~p~(Optional)~w~ Open the second ~y~Security Gate~w~",
					"Loot at least $" + text2,
					"Leave the ~y~Betta Bank",
					"Escape the ~r~Cops"
				}, 1, new List<Vector2>
				{
					new Vector2(-1231.9f, -334.3f)
				}));
				if (Game.Player.Name.ToUpper().Equals(AdminName))
				{
					Heists_Data.Add(new HeistData(HeistID.TheBelkraneJob, "~y~The Belkrane Job (Coming June 2024)", (float)(ThePrinthouseHeist.HeistMinPay + ThePrinthouseHeist.HeistMinPay) / 2f, new List<string> { "NO INTELL YET" }, 1, new List<Vector2>
					{
						new Vector2(0f, 0f),
						new Vector2(0f, 0f),
						new Vector2(0f, 0f)
					}));
				}
			}
			catch (Exception)
			{
				Notification.Show("~r~Error~w~: " + iniName + " Failed To Load.");
			}
		}

		public static void ResetGetHeistPays()
		{
			Class1.Fleeca_HeistMaxPay = Config.GetValue("Fleeca", "HeistMinPay", Class1.Fleeca_HeistMaxPay);
			Class1.Fleeca_HeistMaxPay = Config.GetValue("Fleeca", "HeistMaxPay", Class1.Fleeca_HeistMaxPay);
			Class1.Paleto_HeistMinPay = Config.GetValue("Paleto", "HeistMinPay", Class1.Paleto_HeistMinPay);
			Class1.Paleto_HeistMaxPay = Config.GetValue("Paleto", "HeistMaxPay", Class1.Paleto_HeistMaxPay);
			Class1.PacificStandard_HeistMinPay = Config.GetValue("PacificStandard", "HeistMinPay", Class1.PacificStandard_HeistMinPay);
			Class1.PacificStandard_HeistMaxPay = Config.GetValue("PacificStandard", "HeistMaxPay", Class1.PacificStandard_HeistMaxPay);
			Class1.JewelStore_HeistMinPay = Config.GetValue("JewelStoreJob", "HeistMinPay", Class1.JewelStore_HeistMinPay);
			Class1.JewelStore_HeistMaxPay = Config.GetValue("JewelStoreJob", "HeistMaxPay", Class1.JewelStore_HeistMaxPay);
			Class1.UnionDepository_HeistMinPay = Config.GetValue("UnionDepository", "HeistMinPay", Class1.UnionDepository_HeistMinPay);
			Class1.UnionDepository_HeistMaxPay = Config.GetValue("UnionDepository", "HeistMaxPay", Class1.UnionDepository_HeistMaxPay);
			Class1.LaFuenteBlanca_HeistMinPay = Config.GetValue("LaFuenteBlanca", "HeistMinPay", Class1.LaFuenteBlanca_HeistMinPay);
			Class1.LaFuenteBlanca_HeistMaxPay = Config.GetValue("LaFuenteBlanca", "HeistMaxPay", Class1.LaFuenteBlanca_HeistMaxPay);
			Class1.BikerClubhouse_HeistMinPay = Config.GetValue("BikerClubhouse", "HeistMinPay", Class1.BikerClubhouse_HeistMinPay);
			Class1.BikerClubhouse_HeistMaxPay = Config.GetValue("BikerClubhouse", "HeistMaxPay", Class1.BikerClubhouse_HeistMaxPay);
			CargoShipHeist.HeistMinPay = Config.GetValue("LaoubPrincessCargoshipHeist", "HeistMinPay", CargoShipHeist.HeistMinPay);
			CargoShipHeist.HeistMaxPay = Config.GetValue("LaoubPrincessCargoshipHeist", "HeistMaxPay", CargoShipHeist.HeistMaxPay);
			ThePrisonBreak.HeistStage = Config.GetValue("THEPRISONBREAK", "HEISTSTAGE", ThePrisonBreak.HeistStage);
			ThePrisonBreak.HeistMinPay = Config.GetValue("THEPRISONBREAK", "HeistMinPay", ThePrisonBreak.HeistMinPay);
			ThePrisonBreak.HeistMaxPay = Config.GetValue("THEPRISONBREAK", "HeistMaxPay", ThePrisonBreak.HeistMaxPay);
			HumainLabsOnline.HeistStage = Config.GetValue("HUMAINELABS", "HEISTSTAGE", HumainLabsOnline.HeistStage);
			HumainLabsOnline.HeistMinPay = Config.GetValue("HUMAINELABS", "HeistMinPay", HumainLabsOnline.HeistMinPay);
			HumainLabsOnline.HeistMaxPay = Config.GetValue("HUMAINELABS", "HeistMaxPay", HumainLabsOnline.HeistMaxPay);
			TheBillionaresParty.HeistMinPay = Config.GetValue("TheBillionaresParty", "HeistMinPay", TheBillionaresParty.HeistMinPay);
			TheBillionaresParty.HeistMaxPay = Config.GetValue("TheBillionaresParty", "HeistMinPay", TheBillionaresParty.HeistMaxPay);
			TheFederalReserveTrainHeist.HeistMinPay = Config.GetValue("TheFederalReserveTrainHeist", "HeistMinPay", TheFederalReserveTrainHeist.HeistMinPay);
			TheFederalReserveTrainHeist.HeistMaxPay = Config.GetValue("TheFederalReserveTrainHeist", "HeistMinPay", TheFederalReserveTrainHeist.HeistMaxPay);
			TheLexingtonRaid.HeistMinPay = Config.GetValue("TheLuxingtonRaid", "HeistMinPay", TheLexingtonRaid.HeistMinPay);
			TheLexingtonRaid.HeistMaxPay = Config.GetValue("TheLuxingtonRaid", "HeistMinPay", TheLexingtonRaid.HeistMaxPay);
			YachtHeist.HeistMinPay = Config.GetValue("TheCartelYachtHeist", "HeistMinPay", YachtHeist.HeistMinPay);
			YachtHeist.HeistMaxPay = Config.GetValue("TheCartelYachtHeist", "HeistMinPay", YachtHeist.HeistMaxPay);
			The247Job.HeistMinPay = Config.GetValue("24/7Job", "HeistMinPay", The247Job.HeistMinPay);
			The247Job.HeistMaxPay = Config.GetValue("24/7Job", "HeistMaxPay", The247Job.HeistMaxPay);
			TheSilthouseJob.HeistMinPay = Config.GetValue("TheSilthouseJob", "HeistMinPay", TheSilthouseJob.HeistMinPay);
			TheSilthouseJob.HeistMaxPay = Config.GetValue("TheSilthouseJob", "HeistMaxPay", TheSilthouseJob.HeistMaxPay);
			ThePrinthouseHeist.HeistMinPay = Config.GetValue("TheSilthouseJob", "HeistMinPay", ThePrinthouseHeist.HeistMinPay);
			ThePrinthouseHeist.HeistMaxPay = Config.GetValue("TheSilthouseJob", "HeistMaxPay", ThePrinthouseHeist.HeistMaxPay);
			TheMazebankSavingsBankHeistcs.HeistMinPay = Config.GetValue("TheMazebankSavingsBank", "HeistMinPay", TheMazebankSavingsBankHeistcs.HeistMinPay);
			TheMazebankSavingsBankHeistcs.HeistMaxPay = Config.GetValue("TheMazebankSavingsBank", "HeistMaxPay", TheMazebankSavingsBankHeistcs.HeistMaxPay);
			TheBetaBankHeist.HeistMinPay = Config.GetValue("TheBettaBankHeist", "HeistMinPay", TheBetaBankHeist.HeistMinPay);
			TheBetaBankHeist.HeistMaxPay = Config.GetValue("TheBettaBankHeist", "HeistMaxPay", TheBetaBankHeist.HeistMaxPay);
			TheDiamondJewelStore.HeistMinPay = Config.GetValue("TheDiamondJewelStore", "HeistMinPay", TheDiamondJewelStore.HeistMinPay);
			TheDiamondJewelStore.HeistMaxPay = Config.GetValue("TheDiamondJewelStore", "HeistMaxPay", TheDiamondJewelStore.HeistMaxPay);
			TheGefangnisArtHeist.HeistMinPay = Config.GetValue("TheGefangnisArtHeist", "HeistMinPay", TheGefangnisArtHeist.HeistMinPay);
			TheGefangnisArtHeist.HeistMaxPay = Config.GetValue("TheGefangnisArtHeist", "HeistMaxPay", TheGefangnisArtHeist.HeistMaxPay);
		}

		public static void LoadIniFileArcade(string iniName)
		{
			try
			{
				ArcadeConfig = ScriptSettings.Load(iniName);
				ArcadeOwned = ArcadeConfig.GetValue("Arcade", "ArcadeOwned", ArcadeOwned);
				ArcadeVersion = ArcadeConfig.GetValue("Arcade", "ArcadeVersion", ArcadeVersion);
				UsingHiddenDoor = 1;
				ArcadeFloodID = ArcadeConfig.GetValue("Arcade", "ArcadeFloodID", ArcadeFloodID);
				ArcadeStyle = ArcadeConfig.GetValue("Arcade", "ArcadeStyle", ArcadeStyle);
				MuralID = ArcadeConfig.GetValue("Arcade", "MuralID", MuralID);
				PushieID = ArcadeConfig.GetValue("Arcade", "PushieID", PushieID);
				NeonArtID = ArcadeConfig.GetValue("Arcade", "NeonArtID", NeonArtID);
				ArcadeClutter = ArcadeConfig.GetValue("Arcade", "ArcadeClutter", ArcadeClutter);
				UnlockedDilllEquipment = 1;
				UnlockedHackingEquipment = 1;
				UnlockedBreachingEquipment = 1;
				UnlockedOutfitsEquipment = 1;
				UnlockedPlansEquipment = 1;
				UnlockedGarage = ArcadeConfig.GetValue("Arcade", "UnlockedGarage", UnlockedGarage);
				UnlockedGunlocker = 1;
				ArcadeMoneyVault = 1;
				UndergrondAreaLocked = ArcadeConfig.GetValue("Arcade", "UndergrondAreaLocked", UndergrondAreaLocked);
				Arcade1Ver = ArcadeConfig.GetValue("Arcade", "Arcade1Ver", Arcade1Ver);
				Arcade2Ver = ArcadeConfig.GetValue("Arcade", "Arcade2Ver", Arcade2Ver);
				Arcade3Ver = ArcadeConfig.GetValue("Arcade", "Arcade3Ver", Arcade3Ver);
				Arcade4Ver = ArcadeConfig.GetValue("Arcade", "Arcade4Ver", Arcade4Ver);
				Arcade5Ver = ArcadeConfig.GetValue("Arcade", "Arcade5Ver", Arcade5Ver);
				Arcade6Ver = ArcadeConfig.GetValue("Arcade", "Arcade6Ver", Arcade6Ver);
				Arcade7Ver = ArcadeConfig.GetValue("Arcade", "Arcade7Ver", Arcade7Ver);
				Arcade8Ver = ArcadeConfig.GetValue("Arcade", "Arcade8Ver", Arcade8Ver);
				Arcade9Ver = ArcadeConfig.GetValue("Arcade", "Arcade9Ver", Arcade9Ver);
				Arcade10Ver = ArcadeConfig.GetValue("Arcade", "Arcade10Ver", Arcade10Ver);
				Arcade11Ver = ArcadeConfig.GetValue("Arcade", "Arcade11Ver", Arcade11Ver);
				Arcade12Ver = ArcadeConfig.GetValue("Arcade", "Arcade12Ver", Arcade12Ver);
				Arcade13Ver = ArcadeConfig.GetValue("Arcade", "Arcade13Ver", Arcade13Ver);
				Arcade14Ver = ArcadeConfig.GetValue("Arcade", "Arcade14Ver", Arcade14Ver);
				Arcade15Ver = ArcadeConfig.GetValue("Arcade", "Arcade15Ver", Arcade15Ver);
				Arcade16Ver = ArcadeConfig.GetValue("Arcade", "Arcade16Ver", Arcade16Ver);
				Arcade17Ver = ArcadeConfig.GetValue("Arcade", "Arcade17Ver", Arcade17Ver);
				Arcade18Ver = ArcadeConfig.GetValue("Arcade", "Arcade18Ver", Arcade18Ver);
				Arcade19Ver = ArcadeConfig.GetValue("Arcade", "Arcade19Ver", Arcade19Ver);
				Arcade20Ver = ArcadeConfig.GetValue("Arcade", "Arcade20Ver", Arcade20Ver);
				PlayRadioMusic = ArcadeConfig.GetValue("Arcade", "PlayRadioMusic", PlayRadioMusic);
				DefaultRadioStation = ArcadeConfig.GetValue("Arcade", "DefaultRadioStation", DefaultRadioStation);
				CurrentRadioStation = ArcadeConfig.GetValue("Arcade", "CurrentRadioStation", CurrentRadioStation);
				RadioOn = ArcadeConfig.GetValue("Arcade", "RadioOn", RadioOn);
				ChangeArcade();
			}
			catch (Exception)
			{
				Notification.Show("~r~Error~w~:  SinglePlayerHeists Failed To Load.");
			}
		}

		public void SetStage()
		{
			FleecaStage = Config.GetValue("FLEECA", "HEISTSTAGE", FleecaStage);
			PacificStandardStage = Config.GetValue("PACIFIC_STANDARD", "HEISTSTAGE", PacificStandardStage);
			PaletoStage = Config.GetValue("PALETOJOB", "HEISTSTAGE", PaletoStage);
			VangelicoStage = Config.GetValue("VANGELICO", "HEISTSTAGE", VangelicoStage);
			UnionDelpositioryStage = Config.GetValue("UNION_DEPOSITORY", "HEISTSTAGE", UnionDelpositioryStage);
			LEXIGTONRAIDStage = Config.GetValue("LEXIGTONRAID", "HEISTSTAGE", LEXIGTONRAIDStage);
			SURVERDOWNStage = Config.GetValue("SURVERDOWN", "HEISTSTAGE", SURVERDOWNStage);
			NUCLEAREXPANSIONHEISTStage = Config.GetValue("NUCLEAREXPANSIONHEIST", "HEISTSTAGE", NUCLEAREXPANSIONHEISTStage);
			DEEPDEAPTHSHEISTStage = Config.GetValue("DEEPDEAPTHSHEIST", "HEISTSTAGE", DEEPDEAPTHSHEISTStage);
			HUMANELABS2Stage = Config.GetValue("HUMANELABS#2", "HEISTSTAGE", HUMANELABS2Stage);
			YachtHeistStage = Config.GetValue("YACHTHEIST", "HEISTSTAGE", YachtHeistStage);
			BerkraneJobStage = Config.GetValue("BELKRANEJOB", "HEISTSTAGE", YachtHeistStage);
			DiamondHeistStage = Config.GetValue("THEDIAMONDHIEST", "HEISTSTAGE", DiamondHeistStage);
			PrisonBreakStage = Config.GetValue("THEDIAMONDHIEST", "HEISTSTAGE", PrisonBreakStage);
			ContainerHeistStage = Config.GetValue("THEDIAMONDHIEST", "HEISTSTAGE", ContainerHeistStage);
			HumainLabsRaid_CruelMaster = Config.GetValue("HUMANELABSONLINE", "HEISTSTAGE", HumainLabsRaid_CruelMaster);
			NorthYanktonStage = Config.GetValue("NORTHYANKTON", "HEISTSTAGE", NorthYanktonStage);
			TotalCrewEarned = Config.GetValue("Misc", "TotalCrewEarned", TotalCrewEarned);
			TotalPlayerEarned = Config.GetValue("Misc", "TotalPlayerEarned", TotalPlayerEarned);
		}

		public void Setup()
		{
			ChairPropModelArcadeA = "ex_prop_offchair_exec_01";
			ChairPropModelArcadeB = "ex_prop_offchair_exec_01";
			AddArcades();
			Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
			modLemonUIObjectPool = new ObjectPool();
			mainMenu = new NativeMenu("Single Player Heists", "Select an Option");
			modLemonUIObjectPool.Add(mainMenu);
			GLemonUIMenusNativeMenus.Add(mainMenu);
			Heists = new NativeMenu("", "Available Heists");
			modLemonUIObjectPool.Add(Heists);
			mainMenu.AddSubMenu(Heists);
			GLemonUIMenusNativeMenus.Add(Heists);
			Vehicles = new NativeMenu("", "Vehicles");
			modLemonUIObjectPool.Add(Vehicles);
			mainMenu.AddSubMenu(Vehicles);
			GLemonUIMenusNativeMenus.Add(Vehicles);
			SetupHeists();
			WorderbePool = new ObjectPool();
			WoredrobeMainMenu = new NativeMenu("Wardrobe", "Select an Option");
			WorderbePool.Add(WoredrobeMainMenu);
			WoredrobeMenu = new NativeMenu("", "Change Clothes");
			modLemonUIObjectPool.Add(WoredrobeMenu);
			WoredrobeMainMenu.AddSubMenu(WoredrobeMenu);
			mainMenu2 = new NativeMenu("Single Player Heists", "Select an Option");
			modLemonUIObjectPool.Add(mainMenu2);
			RemoveMenu2 = new NativeMenu("", "Remove A Vehicle");
			modLemonUIObjectPool.Add(RemoveMenu2);
			mainMenu2.AddSubMenu(RemoveMenu2);
			GLemonUIMenusNativeMenus.Add(RemoveMenu2);
			GarageMenu = new NativeMenu("", "Enter Garage");
			modLemonUIObjectPool.Add(GarageMenu);
			mainMenu2.AddSubMenu(GarageMenu);
			GLemonUIMenusNativeMenus.Add(GarageMenu);
			MainStuffMenu = new NativeMenu("Arcade", "Select an Option");
			modLemonUIObjectPool.Add(MainStuffMenu);
			ArcadeC = new NativeMenu("", "Customize Arcade");
			modLemonUIObjectPool.Add(ArcadeC);
			MainStuffMenu.AddSubMenu(ArcadeC);
			GLemonUIMenusNativeMenus.Add(ArcadeC);
			DrinksMenu = new NativeMenu("", "Drinks");
			modLemonUIObjectPool.Add(DrinksMenu);
			MainStuffMenu.AddSubMenu(DrinksMenu);
			GLemonUIMenusNativeMenus.Add(DrinksMenu);
			Function.Call(Hash.REMOVE_IPL, "gr_Heist_Yacht2");
			Function.Call(Hash.REMOVE_IPL, "gr_Heist_Yacht2_enginrm");
			Function.Call(Hash.REMOVE_IPL, "gr_Heist_Yacht2_Lounge");
			Function.Call(Hash.REMOVE_IPL, "gr_Heist_Yacht2_Bridge");
			Function.Call(Hash.REMOVE_IPL, "gr_Heist_Yacht2_Bar");
			Function.Call(Hash.REMOVE_IPL, "gr_Heist_Yacht2_Bedrm");
			Function.Call(Hash.REQUEST_IPL, "gr_Heist_Yacht2");
			Function.Call(Hash.REQUEST_IPL, "gr_Heist_Yacht2_enginrm");
			Function.Call(Hash.REQUEST_IPL, "gr_Heist_Yacht2_Lounge");
			Function.Call(Hash.REQUEST_IPL, "gr_Heist_Yacht2_Bridge");
			Function.Call(Hash.REQUEST_IPL, "gr_Heist_Yacht2_Bar");
			Function.Call(Hash.REQUEST_IPL, "gr_Heist_Yacht2_Bedrm");
			Function.Call(Hash.REMOVE_IPL, "xm_x17dlc_int_placement_interior_8_x17dlc_int_sub_milo_");
			Function.Call(Hash.REQUEST_IPL, "xm_x17dlc_int_placement_interior_8_x17dlc_int_sub_milo_");
			WareDrobe();
			ArcadeInteriorOptions();
			SetupArcadeGarageEnterMenu();
			SetupArcadeGarageBuySellMenu();
			AddRatioStations();
			Drinks();
			MoneymainMenu = new NativeMenu("Money Vault", "Select an Option");
			GLemonUIMenusNativeMenus.Add(MoneymainMenu);
			modLemonUIObjectPool.Add(MoneymainMenu);
			MoneyMenu = new NativeMenu("", "Money Options");
			modLemonUIObjectPool.Add(MoneyMenu);
			MoneymainMenu.AddSubMenu(MoneyMenu);
			GLemonUIMenusNativeMenus.Add(MoneyMenu);
			CreateBanner();
			SetupMoneyMenu();
			for (int i = 0; i < GLemonUIMenusNativeMenus.Count; i++)
			{
				NativeMenu banner = GLemonUIMenusNativeMenus[i];
				SetBanner(banner);
			}
		}

		public void SetBanner(NativeMenu menu)
		{
			menu.Banner = RectangleGUI;
		}

		public void CreateBanner()
		{
			RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 108));
			RectangleGUI.Color = Color.FromArgb(255, 0, 0, 0);
		}

		public void SetupMoneyMenu()
		{
			NativeMenu nativeMenu = new NativeMenu("", "Money Options");
			modLemonUIObjectPool.Add(nativeMenu);
			MoneyMenu.AddSubMenu(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			NativeItem nativeItem = new NativeItem("Withdraw");
			nativeMenu.Add(nativeItem);
			NativeItem nativeItem2 = new NativeItem("Deposit");
			nativeMenu.Add(nativeItem2);
			NativeItem nativeItem3 = new NativeItem("Show Balance");
			nativeMenu.Add(nativeItem3);
			nativeItem.Activated += delegate
			{
				Withdrawl();
			};
			nativeItem2.Activated += delegate
			{
				Deposit();
			};
			nativeItem3.Activated += delegate
			{
				MoneyStored = ArcadeConfig.GetValue("ADDON2", "MONEYSTORED", MoneyStored);
				Notification.Show("Offic Assistant : Money in bank ~g~$" + MoneyStored.ToString("N"));
			};
		}

		private void Deposit()
		{
			string userInput = Game.GetUserInput(WindowTitle.EnterEyefindMessage, "", 12);
			if (userInput != "")
			{
				if (int.Parse(userInput) <= Game.Player.Money)
				{
					MoneyStored = int.Parse(userInput);
					ManagePlayerFunds.RemoveFunds(int.Parse(userInput));
					float value = ArcadeConfig.GetValue("ADDON2", "MONEYSTORED", MoneyStored);
					ArcadeConfig.SetValue("ADDON2", "MONEYSTORED", MoneyStored + value);
					ArcadeConfig.Save();
				}
				else
				{
					Notification.Show(GetHostName() + " boss you dont have that amount of money to deposit: value: " + int.Parse(userInput));
				}
			}
		}

		private void Withdrawl()
		{
			string userInput = Game.GetUserInput(WindowTitle.EnterEyefindMessage, "", 12);
			if (userInput != "")
			{
				if (!((float)int.Parse(userInput) > MoneyStored))
				{
					MoneyStored = ArcadeConfig.GetValue("ADDON2", "MONEYSTORED", MoneyStored) - (float)int.Parse(userInput);
					ManagePlayerFunds.AddFunds(int.Parse(userInput));
					ArcadeConfig.SetValue("ADDON2", "MONEYSTORED", MoneyStored);
					ArcadeConfig.Save();
				}
				else
				{
					Notification.Show(GetHostName() + " boss you dont have that amount of money stored to withdraw, value: " + MoneyStored);
				}
			}
		}

		private void SetupHeists()
		{
			NativeMenu nativeMenu = new NativeMenu("", "Hit'n'Run Heists");
			modLemonUIObjectPool.Add(nativeMenu);
			Heists.AddSubMenu(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			nativeMenu.Description = "Heists with No Prep missions, start from Arcade Heist Marker, or from Marker nearby heist location.";
			NativeMenu nativeMenu2 = new NativeMenu("", "Freeform Heists");
			modLemonUIObjectPool.Add(nativeMenu2);
			Heists.AddSubMenu(nativeMenu2);
			GLemonUIMenusNativeMenus.Add(nativeMenu2);
			nativeMenu2.Description = "Heists with Optional Prep missions & Finale rolled into one mission, start from Arcade Heist Marker, or from Marker nearby heist location.";
			NativeMenu nativeMenu3 = new NativeMenu("", "Full Setup Heists");
			modLemonUIObjectPool.Add(nativeMenu3);
			Heists.AddSubMenu(nativeMenu3);
			GLemonUIMenusNativeMenus.Add(nativeMenu3);
			nativeMenu3.Description = "Heists with Seperate Prep missions & Finale (Also has AI used in The Payday Heist mod), start from Arcade Heist Marker, or from Marker nearby heist location.";
			NativeItem nativeItem = new NativeItem("The 24/7 Job");
			nativeMenu.Add(nativeItem);
			nativeItem.Activated += delegate
			{
				The247Job.StartHeist();
			};
			NativeItem nativeItem2 = new NativeItem("The Silthouse Job");
			nativeMenu.Add(nativeItem2);
			nativeItem2.Activated += delegate
			{
				TheSilthouseJob.StartHeist();
			};
			NativeItem nativeItem3 = new NativeItem("The Fleeca Heist (Random Location)");
			nativeMenu.Add(nativeItem3);
			nativeItem3.Activated += delegate
			{
				Class1.CurrentFleecaBank = -1;
				Class1.RunFleeca = 1;
			};
			NativeItem nativeItem4 = new NativeItem("The Paleto Job");
			nativeMenu.Add(nativeItem4);
			nativeItem4.Activated += delegate
			{
				Class1.RunPaleto = 1;
			};
			NativeItem nativeItem5 = new NativeItem("The Pacific Standard ");
			nativeMenu.Add(nativeItem5);
			nativeItem5.Activated += delegate
			{
				Class1.RunPacificStandard = 1;
			};
			NativeItem nativeItem6 = new NativeItem("The Jewel Store Job ");
			nativeMenu.Add(nativeItem6);
			nativeItem6.Activated += delegate
			{
				Class1.RunJewelryStore = 1;
			};
			NativeItem nativeItem7 = new NativeItem("The Union Depository ");
			nativeMenu2.Add(nativeItem7);
			nativeItem7.Activated += delegate
			{
				Class1.RunUnionDepositoryHeist = 1;
			};
			NativeItem nativeItem8 = new NativeItem("The Biker Clubhouse Heist");
			nativeMenu.Add(nativeItem8);
			nativeItem8.Activated += delegate
			{
				Class1.RunBikerClubhouseHeist = 1;
			};
			NativeItem nativeItem9 = new NativeItem("The La Fuente Blanca Heist");
			nativeMenu.Add(nativeItem9);
			nativeItem9.Activated += delegate
			{
				Class1.RunLaFauenteBlanca = 1;
			};
			NativeItem nativeItem10 = new NativeItem("The Mazebank Savings Bank Heist");
			nativeMenu.Add(nativeItem10);
			nativeItem10.Activated += delegate
			{
				TheMazebankSavingsBankHeistcs.StartHeist();
			};
			NativeItem nativeItem11 = new NativeItem("The Printhouse Heist");
			nativeMenu.Add(nativeItem11);
			nativeItem11.Activated += delegate
			{
				ThePrinthouseHeist.StartHeist();
			};
			NativeItem nativeItem12 = new NativeItem("The Gefangnis Art Heist");
			nativeMenu.Add(nativeItem12);
			nativeItem12.Activated += delegate
			{
				TheGefangnisArtHeist.StartHeist();
			};
			NativeItem nativeItem13 = new NativeItem("The Diamond Jewel Store Heist");
			nativeMenu.Add(nativeItem13);
			nativeItem13.Activated += delegate
			{
				TheDiamondJewelStore.StartHeist();
			};
			NativeItem nativeItem14 = new NativeItem("The Betta Bank Heist");
			nativeMenu.Add(nativeItem14);
			nativeItem14.Activated += delegate
			{
				TheBetaBankHeist.StartHeist();
			};
			NativeItem nativeItem15 = new NativeItem("The La'oub Princess Heist");
			nativeMenu2.Add(nativeItem15);
			nativeItem15.Activated += delegate
			{
				CargoShipHeist.StartHeist();
			};
			NativeItem nativeItem16 = new NativeItem("The USS Luxington Raid");
			nativeMenu2.Add(nativeItem16);
			nativeItem16.Activated += delegate
			{
				TheLexingtonRaid.StartHeist();
			};
			NativeItem nativeItem17 = new NativeItem("The Billionaires Party");
			nativeMenu2.Add(nativeItem17);
			nativeItem17.Activated += delegate
			{
				TheBillionaresParty.StartHeist();
			};
			NativeItem nativeItem18 = new NativeItem("The Federal Reserve Train Heist");
			nativeMenu2.Add(nativeItem18);
			nativeItem18.Activated += delegate
			{
				TheFederalReserveTrainHeist.StartHeist();
			};
			NativeItem nativeItem19 = new NativeItem("The Humaine Labs (Online)");
			nativeMenu3.Add(nativeItem19);
			nativeItem19.Activated += delegate
			{
				HumainLabsOnline.RunHeist = 1;
			};
			NativeItem nativeItem20 = new NativeItem("The Party Yacht Heist");
			nativeMenu2.Add(nativeItem20);
			nativeItem20.Activated += delegate
			{
				YachtHeist.StartHeist();
			};
			List<object> list = new List<object>();
			list.Add("Main Entrance");
			list.Add("Loading Bay - Using Stockade");
			list.Add("Sewer");
			list.Add("Rooftop Right - Using Helicopter ");
			list.Add("Rooftop Left - Using Helicopter ");
			list.Add("Loading Bay - Bugstars");
			list.Add("Main Enterance (Loud)");
			list.Add("Waste Disposal");
			list.Add("Offices Side Door");
			List<object> list2 = new List<object>();
			list2.Add("Main Entrance");
			list2.Add("Sewer");
			list2.Add("Rooftop Right - Using Helicopter ");
			list2.Add("Rooftop Left - Using Helicopter ");
			list2.Add("Loading Bay - Bugstars");
			list2.Add("Offices Side Door");
			List<dynamic> Loc3 = new List<object>();
			Loc3.Add("ALKONOST");
			Loc3.Add("ANNIHILATOR2");
			Loc3.Add("NIMBUS");
			Loc3.Add("VELUM");
			Loc3.Add("PATROLBOAT");
			Loc3.Add("LONGFIN");
			NativeMenu nativeMenu4 = new NativeMenu("", "The Diamond Casino Heist");
			modLemonUIObjectPool.Add(nativeMenu4);
			nativeMenu3.AddSubMenu(nativeMenu4);
			GLemonUIMenusNativeMenus.Add(nativeMenu4);
			nativeMenu4.Description = "The Diamond Casino Heist. Start from start and follow Setups, or Launch from Finale & Pick Enterance/Exit";
			NativeItem nativeItem21 = new NativeItem("The Diamond Casino Heist (Setup 1 to Finale)");
			nativeMenu4.Add(nativeItem21);
			nativeItem21.Activated += delegate
			{
				TheDiamondHeist.RunHeist = 1;
			};
			NativeMenu nativeMenu5 = new NativeMenu("", "The Diamond Casino Heist (Finale Only)");
			modLemonUIObjectPool.Add(nativeMenu5);
			nativeMenu4.AddSubMenu(nativeMenu5);
			GLemonUIMenusNativeMenus.Add(nativeMenu5);
			nativeMenu5.Description = "The Diamond Casino Heist. Launch from Finale & Pick Enterance/Exit";
			NativeListItem<dynamic> L = new NativeListItem<object>("Enterance : ", list.ToArray());
			nativeMenu5.Add(L);
			NativeListItem<dynamic> E = new NativeListItem<object>("Exit : ", list2.ToArray());
			nativeMenu5.Add(E);
			NativeItem nativeItem22 = new NativeItem("The Diamond Casino Heist (Finale)");
			nativeMenu5.Add(nativeItem22);
			nativeItem22.Activated += delegate
			{
				TheDiamondHeist.HeistStage = 4;
				TheDiamondHeist.EnteranceUsed = L.SelectedIndex;
				TheDiamondHeist.ExitUsed = E.SelectedIndex;
				TheDiamondHeist.RunHeist = 1;
			};
			NativeMenu nativeMenu6 = new NativeMenu("", "The Cayo Perico Heist");
			modLemonUIObjectPool.Add(nativeMenu6);
			nativeMenu3.AddSubMenu(nativeMenu6);
			GLemonUIMenusNativeMenus.Add(nativeMenu6);
			nativeMenu6.Description = "The Cayo Perico Heist. Start from start and follow Setups, or Launch from Finale & Pick Enterance/Exit";
			NativeItem nativeItem23 = new NativeItem("The Cayo Perico Heist (Setup 1 to Finale)");
			nativeMenu6.Add(nativeItem23);
			nativeItem23.Activated += delegate
			{
				CayoPericoHeist.RunHeist = 1;
			};
			NativeMenu nativeMenu7 = new NativeMenu("", "The Cayo Perico Heist (Finale Only)");
			modLemonUIObjectPool.Add(nativeMenu7);
			nativeMenu6.AddSubMenu(nativeMenu7);
			GLemonUIMenusNativeMenus.Add(nativeMenu7);
			nativeMenu6.Description = "The Cayo Perico Heist. Launch from Finale & Pick Enterance/Exit";
			NativeListItem<dynamic> CA = new NativeListItem<object>("Cayo Approach : ", Loc3.ToArray());
			nativeMenu7.Add(CA);
			NativeItem nativeItem24 = new NativeItem("The Cayo Perico Heist (Finale)");
			nativeMenu7.Add(nativeItem24);
			nativeItem24.Activated += delegate
			{
				CayoPericoHeist.HeistStage = 6;
				CayoPericoHeist.Config.SetValue("CayoPericoHeist", "HeistStage", CayoPericoHeist.HeistStage);
				CayoPericoHeist.Config.Save();
				CayoPericoHeist.OverrideHeistStage = 6;
				CayoPericoHeist.VehicleEnteranceMethod = Loc3[CA.SelectedIndex];
				CayoPericoHeist.RunHeist = 1;
				if (CayoPericoHeist.HeistStage == 6)
				{
					IsInArcade = false;
				}
			};
			NativeItem nativeItem25 = new NativeItem("The Prison Break");
			nativeMenu3.Add(nativeItem25);
			nativeItem25.Activated += delegate
			{
				ThePrisonBreak.RunHeist = 1;
			};
		}

		public static void DisplayHelpTextThisFrameNoSound(string helpText)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 0, 1);
		}

		public static void DisplayHelpTextThisFrame(string helpText)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 0, 1);
		}

		public void Drinks()
		{
			NativeMenu nativeMenu = new NativeMenu("Arcade", "Drinks");
			modLemonUIObjectPool.Add(nativeMenu);
			DrinksMenu.AddSubMenu(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			NativeItem nativeItem = new NativeItem("Pisswasser                                               $25");
			nativeMenu.Add(nativeItem);
			NativeItem nativeItem2 = new NativeItem("Vodka Shot                                              $75");
			nativeMenu.Add(nativeItem2);
			NativeItem nativeItem3 = new NativeItem("The Mount Whiskey Shot                       $250");
			nativeMenu.Add(nativeItem3);
			NativeItem nativeItem4 = new NativeItem("Richard's Whiskey Shot                       $1,000");
			nativeMenu.Add(nativeItem4);
			NativeItem nativeItem5 = new NativeItem("Macbeth Whiskey Shot                       $5,000");
			nativeMenu.Add(nativeItem5);
			NativeItem nativeItem6 = new NativeItem("Bleuter'd Champaine Slver                $30,000");
			nativeMenu.Add(nativeItem6);
			NativeItem nativeItem7 = new NativeItem("Bleuter'd Champaine Gold                $50,000");
			nativeMenu.Add(nativeItem7);
			NativeItem nativeItem8 = new NativeItem("Bleuter'd Champaine Diamond        $150,000");
			nativeMenu.Add(nativeItem8);
			nativeItem8.Activated += delegate
			{
				if (ManagePlayerFunds.AreFundsHigher(150000))
				{
					ManagePlayerFunds.RemoveFunds(150000);
					Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
					Prop prop8 = GlobalHelper.CreateProp(RequestModel("ba_prop_battle_champ_closed_03"), Game.Player.Character.GetOffsetPosition(new Vector3(5f, 0f, -1f)), false, false);
					prop8.IsPositionFrozen = true;
					prop8.Position = Game.Player.Character.Bones[Bone.IKRightHand].Position + new Vector3(0f, 0f, 0f);
					Champaine = prop8;
					Champ.Add(prop8);
					IsDrinking = true;
					Game.Player.Character.Task.PlayAnimation("anim@heists@heist_safehouse_intro@wine@window", "wine_window_part_one", 5f, 5f, 4500, AnimationFlags.UpperBodyOnly, 0f);
					Effect = 6;
					Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, Champaine, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					modLemonUIObjectPool.HideAll();
					Game.Player.Character.Weapons.Select(WeaponHash.BZGas);
					Game.Player.Character.IsPositionFrozen = true;
					Vector3 vector3 = Game.Player.Character.Bones[Bone.PHRightHand].Position + new Vector3(-0.05f, 0f, -0.05f);
					Script.Wait(5000);
					Game.Player.Character.HasGravity = true;
					Game.Player.Character.IsPositionFrozen = false;
					Champaine.IsPositionFrozen = false;
					Champaine.IsCollisionEnabled = true;
					DrinkTimer = 0;
					Game.Player.Character.Task.ClearAnimation("anim@heists@heist_safehouse_intro@wine@window", "wine_window_part_one");
					Game.Player.Character.Task.ClearAll();
					Game.Player.Character.Task.ClearAllImmediately();
					Champaine.Detach();
					DrunkLevel += 5f;
					if (DrunkLevel >= 8f && DrunkLevel < 16f)
					{
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_M@DRUNK@MODERATEDRUNK");
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "MOVE_M@DRUNK@VERYDRUNK", 1f);
					}
					if (DrunkLevel >= 16f && DrunkLevel < 24f)
					{
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_M@DRUNK@SLIGHTLYDRUNK");
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "MOVE_M@DRUNK@VERYDRUNK", 1f);
					}
					if (DrunkLevel >= 24f && DrunkLevel < 36f)
					{
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_M@DRUNK@VERYDRUNK");
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "MOVE_M@DRUNK@VERYDRUNK", 1f);
					}
				}
				else
				{
					Notification.Show("You do not have enough money for this Drink!");
				}
			};
			nativeItem7.Activated += delegate
			{
				if (ManagePlayerFunds.AreFundsHigher(50000))
				{
					ManagePlayerFunds.RemoveFunds(50000);
					Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
					Prop prop7 = GlobalHelper.CreateProp(RequestModel("ba_prop_battle_champ_closed_02"), Game.Player.Character.GetOffsetPosition(new Vector3(5f, 0f, -1f)), false, false);
					prop7.IsPositionFrozen = true;
					prop7.Position = Game.Player.Character.Bones[Bone.IKRightHand].Position + new Vector3(0f, 0f, 0f);
					Champaine = prop7;
					Champ.Add(prop7);
					IsDrinking = true;
					Game.Player.Character.Task.PlayAnimation("anim@heists@heist_safehouse_intro@wine@window", "wine_window_part_one", 5f, 5f, 4500, AnimationFlags.UpperBodyOnly, 0f);
					Effect = 6;
					Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, Champaine, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					modLemonUIObjectPool.HideAll();
					Game.Player.Character.Weapons.Select(WeaponHash.BZGas);
					Game.Player.Character.IsPositionFrozen = true;
					Vector3 vector2 = Game.Player.Character.Bones[Bone.PHRightHand].Position + new Vector3(-0.05f, 0f, -0.05f);
					Script.Wait(5000);
					Game.Player.Character.HasGravity = true;
					Game.Player.Character.IsPositionFrozen = false;
					Champaine.IsPositionFrozen = false;
					Champaine.IsCollisionEnabled = true;
					DrinkTimer = 0;
					Game.Player.Character.Task.ClearAnimation("anim@heists@heist_safehouse_intro@wine@window", "wine_window_part_one");
					Game.Player.Character.Task.ClearAll();
					Game.Player.Character.Task.ClearAllImmediately();
					Champaine.Detach();
					DrunkLevel += 5f;
					if (DrunkLevel >= 8f && DrunkLevel < 16f)
					{
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_M@DRUNK@MODERATEDRUNK");
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "MOVE_M@DRUNK@VERYDRUNK", 1f);
					}
					if (DrunkLevel >= 16f && DrunkLevel < 24f)
					{
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_M@DRUNK@SLIGHTLYDRUNK");
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "MOVE_M@DRUNK@VERYDRUNK", 1f);
					}
					if (DrunkLevel >= 24f && DrunkLevel < 36f)
					{
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_M@DRUNK@VERYDRUNK");
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "MOVE_M@DRUNK@VERYDRUNK", 1f);
					}
				}
				else
				{
					Notification.Show("You do not have enough money for this Drink!");
				}
			};
			nativeItem6.Activated += delegate
			{
				if (ManagePlayerFunds.AreFundsHigher(30000))
				{
					ManagePlayerFunds.RemoveFunds(30000);
					Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
					Prop prop6 = GlobalHelper.CreateProp(RequestModel("ba_prop_battle_champ_closed"), Game.Player.Character.GetOffsetPosition(new Vector3(5f, 0f, -1f)), false, false);
					prop6.IsPositionFrozen = true;
					prop6.Position = Game.Player.Character.Bones[Bone.IKRightHand].Position + new Vector3(0f, 0f, 0f);
					Champaine = prop6;
					Champ.Add(prop6);
					IsDrinking = true;
					Game.Player.Character.Task.PlayAnimation("anim@heists@heist_safehouse_intro@wine@window", "wine_window_part_one", 5f, 5f, 4500, AnimationFlags.UpperBodyOnly, 0f);
					Effect = 6;
					Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, Champaine, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
					modLemonUIObjectPool.HideAll();
					Game.Player.Character.Weapons.Select(WeaponHash.BZGas);
					Game.Player.Character.IsPositionFrozen = true;
					Vector3 vector = Game.Player.Character.Bones[Bone.PHRightHand].Position + new Vector3(-0.05f, 0f, -0.05f);
					Script.Wait(5000);
					Game.Player.Character.HasGravity = true;
					Game.Player.Character.IsPositionFrozen = false;
					Champaine.IsPositionFrozen = false;
					Champaine.IsCollisionEnabled = true;
					DrinkTimer = 0;
					Game.Player.Character.Task.ClearAnimation("anim@heists@heist_safehouse_intro@wine@window", "wine_window_part_one");
					Game.Player.Character.Task.ClearAll();
					Game.Player.Character.Task.ClearAllImmediately();
					Champaine.Detach();
					DrunkLevel += 5f;
					if (DrunkLevel >= 8f && DrunkLevel < 16f)
					{
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_M@DRUNK@MODERATEDRUNK");
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "MOVE_M@DRUNK@VERYDRUNK", 1f);
					}
					if (DrunkLevel >= 16f && DrunkLevel < 24f)
					{
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_M@DRUNK@SLIGHTLYDRUNK");
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "MOVE_M@DRUNK@VERYDRUNK", 1f);
					}
					if (DrunkLevel >= 24f && DrunkLevel < 36f)
					{
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_M@DRUNK@VERYDRUNK");
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "MOVE_M@DRUNK@VERYDRUNK", 1f);
					}
				}
				else
				{
					Notification.Show("You do not have enough money for this Drink!");
				}
			};
			nativeItem5.Activated += delegate
			{
				if (ManagePlayerFunds.AreFundsHigher(5000))
				{
					WineTaskScriptStatus = 0;
					WhiskeyTaskScriptStatus = 0;
					ManagePlayerFunds.RemoveFunds(5000);
					Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
					Prop prop5 = GlobalHelper.CreateProp(RequestModel("prop_cs_whiskey_bottle"), Game.Player.Character.GetOffsetPosition(new Vector3(5f, 0f, -1f)), false, false);
					prop5.IsPositionFrozen = true;
					prop5.Position = Game.Player.Character.Bones[Bone.IKRightHand].Position + new Vector3(0f, 0f, 0f);
					Champaine = prop5;
					Champ.Add(prop5);
					WhiskeyTaskScriptStatus = 0;
					Bottle = Champaine;
					Glass = GlobalHelper.CreateProp(RequestModel(1480049515), Game.Player.Character.Position, false, false);
					IsDrinking = true;
					Effect = 3;
					modLemonUIObjectPool.HideAll();
					BottleType = 1;
					DrunkLevel += 9f;
					if (DrunkLevel >= 8f && DrunkLevel < 16f)
					{
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_M@DRUNK@MODERATEDRUNK");
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "MOVE_M@DRUNK@VERYDRUNK", 1f);
					}
					if (DrunkLevel >= 16f && DrunkLevel < 24f)
					{
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_M@DRUNK@SLIGHTLYDRUNK");
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "MOVE_M@DRUNK@VERYDRUNK", 1f);
					}
					if (DrunkLevel >= 24f && DrunkLevel < 36f)
					{
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_M@DRUNK@VERYDRUNK");
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "MOVE_M@DRUNK@VERYDRUNK", 1f);
					}
				}
				else
				{
					Notification.Show(" : You do not have enough money for this Drink!");
				}
			};
			nativeItem4.Activated += delegate
			{
				if (ManagePlayerFunds.AreFundsHigher(1000))
				{
					WineTaskScriptStatus = 0;
					WhiskeyTaskScriptStatus = 0;
					ManagePlayerFunds.RemoveFunds(1000);
					Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
					Prop prop4 = GlobalHelper.CreateProp(RequestModel("prop_cs_whiskey_bottle"), Game.Player.Character.GetOffsetPosition(new Vector3(5f, 0f, -1f)), false, false);
					prop4.Position = Game.Player.Character.Bones[Bone.IKRightHand].Position + new Vector3(0f, 0f, 0f);
					Champaine = prop4;
					Champ.Add(prop4);
					WhiskeyTaskScriptStatus = 0;
					Bottle = Champaine;
					Glass = GlobalHelper.CreateProp(RequestModel(1480049515), Game.Player.Character.Position, false, false);
					IsDrinking = true;
					Effect = 3;
					modLemonUIObjectPool.HideAll();
					DrunkLevel += 12f;
					if (DrunkLevel >= 8f && DrunkLevel < 16f)
					{
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_M@DRUNK@MODERATEDRUNK");
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "MOVE_M@DRUNK@VERYDRUNK", 1f);
					}
					if (DrunkLevel >= 16f && DrunkLevel < 24f)
					{
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_M@DRUNK@SLIGHTLYDRUNK");
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "MOVE_M@DRUNK@VERYDRUNK", 1f);
					}
					if (DrunkLevel >= 24f && DrunkLevel < 36f)
					{
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_M@DRUNK@VERYDRUNK");
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "MOVE_M@DRUNK@VERYDRUNK", 1f);
					}
				}
				else
				{
					Notification.Show("You do not have enough money for this Drink!");
				}
			};
			nativeItem3.Activated += delegate
			{
				if (ManagePlayerFunds.AreFundsHigher(250))
				{
					WineTaskScriptStatus = 0;
					WhiskeyTaskScriptStatus = 0;
					ManagePlayerFunds.RemoveFunds(250);
					Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
					Prop prop3 = GlobalHelper.CreateProp(RequestModel("prop_cs_whiskey_bottle"), Game.Player.Character.GetOffsetPosition(new Vector3(5f, 0f, -1f)), false, false);
					prop3.Position = Game.Player.Character.Bones[Bone.IKRightHand].Position + new Vector3(0f, 0f, 0f);
					Champaine = prop3;
					Champ.Add(prop3);
					WhiskeyTaskScriptStatus = 0;
					Bottle = Champaine;
					Glass = GlobalHelper.CreateProp(RequestModel(1480049515), Game.Player.Character.Position, false, false);
					IsDrinking = true;
					Effect = 1;
					BottleType = 1;
					modLemonUIObjectPool.HideAll();
					DrunkLevel += 14f;
					if (DrunkLevel >= 8f && DrunkLevel < 16f)
					{
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_M@DRUNK@MODERATEDRUNK");
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "MOVE_M@DRUNK@VERYDRUNK", 1f);
					}
					if (DrunkLevel >= 16f && DrunkLevel < 24f)
					{
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_M@DRUNK@SLIGHTLYDRUNK");
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "MOVE_M@DRUNK@VERYDRUNK", 1f);
					}
					if (DrunkLevel >= 24f && DrunkLevel < 36f)
					{
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_M@DRUNK@VERYDRUNK");
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "MOVE_M@DRUNK@VERYDRUNK", 1f);
					}
				}
				else
				{
					Notification.Show("You do not have enough money for this Drink!");
				}
			};
			nativeItem2.Activated += delegate
			{
				if (ManagePlayerFunds.AreFundsHigher(75))
				{
					WineTaskScriptStatus = 0;
					WhiskeyTaskScriptStatus = 0;
					ManagePlayerFunds.RemoveFunds(75);
					Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
					Prop prop2 = GlobalHelper.CreateProp(RequestModel("prop_vodka_bottle"), Game.Player.Character.GetOffsetPosition(new Vector3(5f, 0f, -1f)), false, false);
					prop2.Position = Game.Player.Character.Bones[Bone.IKRightHand].Position + new Vector3(0f, 0f, 0f);
					Champaine = prop2;
					Champ.Add(prop2);
					WhiskeyTaskScriptStatus = 0;
					Bottle = Champaine;
					Glass = GlobalHelper.CreateProp(RequestModel(1480049515), Game.Player.Character.Position, false, false);
					IsDrinking = true;
					Effect = 0;
					modLemonUIObjectPool.HideAll();
					DrunkLevel += 16f;
					if (DrunkLevel >= 8f && DrunkLevel < 16f)
					{
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_M@DRUNK@MODERATEDRUNK");
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "MOVE_M@DRUNK@VERYDRUNK", 1f);
					}
					if (DrunkLevel >= 16f && DrunkLevel < 24f)
					{
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_M@DRUNK@SLIGHTLYDRUNK");
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "MOVE_M@DRUNK@VERYDRUNK", 1f);
					}
					if (DrunkLevel >= 24f && DrunkLevel < 36f)
					{
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_M@DRUNK@VERYDRUNK");
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "MOVE_M@DRUNK@VERYDRUNK", 1f);
					}
				}
				else
				{
					Notification.Show("You do not have enough money for this Drink!");
				}
			};
			nativeItem.Activated += delegate
			{
				if (ManagePlayerFunds.AreFundsHigher(25))
				{
					WineTaskScriptStatus = 0;
					WhiskeyTaskScriptStatus = 0;
					ManagePlayerFunds.RemoveFunds(25);
					Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
					Prop prop = GlobalHelper.CreateProp(RequestModel("prop_sh_beer_pissh_01"), Game.Player.Character.GetOffsetPosition(new Vector3(5f, 0f, -1f)), false, false);
					prop.Position = Game.Player.Character.Bones[Bone.IKRightHand].Position + new Vector3(0f, 0f, 0f);
					Champaine = prop;
					Champ.Add(prop);
					WhiskeyTaskScriptStatus = 0;
					Bottle = Champaine;
					Glass = GlobalHelper.CreateProp(RequestModel(1480049515), Game.Player.Character.Position, false, false);
					IsDrinking = true;
					Effect = 0;
					modLemonUIObjectPool.HideAll();
					DrunkLevel += 6f;
					if (DrunkLevel >= 8f && DrunkLevel < 16f)
					{
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_M@DRUNK@MODERATEDRUNK");
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "MOVE_M@DRUNK@VERYDRUNK", 1f);
					}
					if (DrunkLevel >= 16f && DrunkLevel < 24f)
					{
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_M@DRUNK@SLIGHTLYDRUNK");
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "MOVE_M@DRUNK@VERYDRUNK", 1f);
					}
					if (DrunkLevel >= 24f && DrunkLevel < 36f)
					{
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_M@DRUNK@VERYDRUNK");
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Game.Player.Character, "MOVE_M@DRUNK@VERYDRUNK", 1f);
					}
				}
				else
				{
					Notification.Show("You do not have enough money for this Drink!");
				}
			};
		}

		public int CheckClothes(int T, int RComp, int RDraw)
		{
			int result = 0;
			if (T == 1 && Function.Call<bool>(Hash.IS_PED_COMPONENT_VARIATION_VALID, Game.Player.Character, RComp, RDraw))
			{
				result = Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Game.Player.Character, RComp);
			}
			if (T == 2 && Function.Call<bool>(Hash.IS_PED_COMPONENT_VARIATION_VALID, Game.Player.Character, RComp, RDraw))
			{
				result = Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Game.Player.Character, RComp, RDraw);
			}
			return result;
		}

		public void Setoutfit(int i)
		{
			Function.Call(Hash.SET_PED_PROP_INDEX, Game.Player.Character, 0, 0, 0, 0);
			if (Game.Player.Character.Model != PedHash.FreemodeMale01)
			{
				OldCharacter = Game.Player.Character.Model;
			}
			Function.Call(Hash.RESET_PED_MOVEMENT_CLIPSET, Game.Player.Character, 1.0);
			Function.Call(Hash.RESET_PED_STRAFE_CLIPSET, Game.Player.Character);
			Model model = new Model(PedHash.FreemodeMale01);
			model.Request(500);
			if (model.IsInCdImage && model.IsValid)
			{
				while (!model.IsLoaded)
				{
					Script.Wait(100);
				}
				Function.Call(Hash.SET_PLAYER_MODEL, Game.Player, model.Hash);
			}
			model.MarkAsNoLongerNeeded();
			Ped character = Game.Player.Character;
			Function.Call(Hash.SET_PED_PROP_INDEX, character, -1, 0, 0, 17);
			bool flag = false;
			string iD_C = ID_C;
			if (i == 0)
			{
				if (iD_C.Equals("Outfit Default"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 125, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 128, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Green"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 125, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 128, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Purple"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 125, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 128, 3, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Pink"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 125, 7, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 128, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Orange"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 125, 5, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 128, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (!flag)
				{
					if (!flag && !iD_C.Equals("Outfit Default"))
					{
						Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
					}
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 125, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 128, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
			}
			if (i == 1)
			{
				if (iD_C.Equals("Outfit Default"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 89, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 55, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 54, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Black"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 89, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 55, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 54, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
			}
			if (i == 2 && iD_C.Equals("Outfit Default"))
			{
				if (!flag && !iD_C.Equals("Outfit Default"))
				{
					Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
				}
				flag = true;
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
				Random random = new Random();
				int num = random.Next(1, 100);
				if (num <= 25)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 38, 0, 1);
				}
				if (num > 25 && num <= 50)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 112, 0, 1);
				}
				if (num > 50 && num <= 75)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 46, 0, 1);
				}
				if (num > 75)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 104, 25, 1);
				}
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 125, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 68, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
			}
			if (i == 3)
			{
				if (iD_C.Equals("Blue"))
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 1, 91, 3, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 4, 84, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 8, 97, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 11, 186, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Green"))
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 1, 91, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 4, 84, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 8, 97, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 11, 186, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Red"))
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 1, 91, 5, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 4, 84, 5, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 8, 97, 5, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 11, 186, 5, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Orange"))
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 1, 91, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 4, 84, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 8, 97, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 11, 186, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Purple"))
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 1, 91, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 4, 84, 3, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 8, 97, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 11, 186, 3, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Pink"))
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 1, 91, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 4, 84, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 8, 97, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 11, 186, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("White"))
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 1, 91, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 4, 84, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 8, 97, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 11, 186, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Black") || iD_C.Equals("Outfit Default"))
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 1, 91, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 4, 84, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 8, 97, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 11, 186, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 12, 0, 0, 1);
				}
			}
			if (i == 4)
			{
				if (iD_C.Equals("Black"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 275, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("White"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 275, 7, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Green"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 7, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 12, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 275, 8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Orange"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 13, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 275, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Purple"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 14, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 275, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Pink"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 15, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 275, 11, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Red"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 275, 14, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Outfit Default") || !flag)
				{
					if (!flag && !iD_C.Equals("Outfit Default"))
					{
						Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
					}
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 275, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
			}
			if (i == 5)
			{
				if (iD_C.Equals("Black"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 276, 5, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("White"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 276, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Green"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 7, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 12, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 276, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Orange"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 13, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 276, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Purple"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 14, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 276, 11, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Pink"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 15, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 276, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Red"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 276, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Outfit Default") || !flag)
				{
					if (!flag && !iD_C.Equals("Outfit Default"))
					{
						Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
					}
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 142, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 19, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 107, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 84, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 3, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 276, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
			}
			if (i == 6)
			{
				if (iD_C.Equals("Green"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 134, 0, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 147, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 167, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 113, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 90, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 2, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 286, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 1, 0, 135, 0);
				}
				if (iD_C.Equals("Outfit Default") || !flag)
				{
					if (!flag && !iD_C.Equals("Outfit Default"))
					{
						Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
					}
					flag = true;
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 134, 0, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 147, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 167, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 113, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 90, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 2, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 286, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 1, 0, 135, 0);
				}
			}
			if (i == 7)
			{
				if (iD_C.Equals("Outfit Default"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 115, 0, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 128, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Green"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 115, 4, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 128, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Purple"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 115, 6, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 128, 3, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Pink"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 115, 7, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 128, 4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Orange"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 115, 5, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 128, 2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (!flag)
				{
					if (!flag && !iD_C.Equals("Outfit Default"))
					{
						Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
					}
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 115, 0, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 128, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
			}
			if (i == 8)
			{
				if (iD_C.Equals("Green"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 133, 8, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 108, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 166, 8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 110, 8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 88, 8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 2, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 283, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Purple"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 133, 10, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 108, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 166, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 110, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 88, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 2, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 283, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Pink"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 133, 11, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 108, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 166, 11, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 110, 11, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 88, 11, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 2, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 283, 11, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Orange"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 133, 9, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 108, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 166, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 110, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 88, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 2, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 283, 9, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (!flag)
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 133, 0, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 108, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 166, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 110, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 88, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 2, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 283, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Outfit Default") || !flag)
				{
					if (!flag && !iD_C.Equals("Outfit Default"))
					{
						Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
					}
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 133, 0, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 108, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 166, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 110, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 88, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 2, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 283, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
			}
			if (i == 9)
			{
				if (iD_C.Equals("Outfit Default"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 91, 0, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 0, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 77, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 55, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 178, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Green"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 91, 1, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 0, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 77, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 55, 1, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 178, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("White"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 91, 9, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 0, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 77, 7, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 55, 7, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 178, 7, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Blue"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 91, 3, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 0, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 77, 3, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 55, 3, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 178, 3, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (iD_C.Equals("Black"))
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 91, 10, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 0, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 77, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 55, 10, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 178, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (!flag)
				{
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 91, 0, 17);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 0, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 77, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 55, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 130, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 178, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
			}
		}

		public void WareDrobe()
		{
			List<object> list = new List<object>();
			list.Add("Save");
			list.Add("Load");
			List<dynamic> Slots = new List<object>();
			Slots.Add("Slot1.ini");
			Slots.Add("Slot2.ini");
			Slots.Add("Slot3.ini");
			Slots.Add("Slot4.ini");
			Slots.Add("Slot5.ini");
			Slots.Add("Slot6.ini");
			Slots.Add("Slot7.ini");
			Slots.Add("Slot8.ini");
			Slots.Add("Slot9.ini");
			Slots.Add("Slot10.ini");
			List<object> list2 = new List<object>();
			list2.Add(WeaponHash.Unarmed);
			list2.Add(WeaponHash.Knife);
			list2.Add(WeaponHash.Nightstick);
			list2.Add(WeaponHash.Hammer);
			list2.Add(WeaponHash.Hatchet);
			list2.Add(WeaponHash.KnuckleDuster);
			list2.Add(WeaponHash.Machete);
			list2.Add(WeaponHash.PoolCue);
			list2.Add(WeaponHash.Wrench);
			list2.Add(WeaponHash.SwitchBlade);
			list2.Add(WeaponHash.GolfClub);
			list2.Add(WeaponHash.Flashlight);
			List<dynamic> Colours = new List<object>();
			Colours.Add("Outfit Default");
			Colours.Add("Blue");
			Colours.Add("Green");
			Colours.Add("Red");
			Colours.Add("Orange");
			Colours.Add("Pink");
			Colours.Add("Purple");
			Colours.Add("White");
			Colours.Add("Black");
			List<dynamic> Outfits = new List<object>();
			Outfits.Add("Soldier");
			Outfits.Add("Cloaker");
			Outfits.Add("Hacker");
			Outfits.Add("Juggernaut");
			Outfits.Add("Arena War A");
			Outfits.Add("Arena War B");
			Outfits.Add("Space Marine");
			Outfits.Add("Commando");
			Outfits.Add("Space Suit");
			Outfits.Add("Tron");
			List<dynamic> Draw = new List<object>();
			List<dynamic> Tex = new List<object>();
			Draw.Add(0);
			Draw.Add(1);
			Tex.Add(0);
			List<object> list3 = new List<object>();
			list3.Add(" 0 FACE");
			list3.Add("1 BEARD");
			list3.Add("2 HAIRCUT");
			list3.Add("3 SHIRT");
			list3.Add("4 PANTS");
			list3.Add("5 Hands / Gloves");
			list3.Add("6 SHOES");
			list3.Add("7 Eyes");
			list3.Add("8 Accessories");
			list3.Add("9 Mission Items/ Tasks");
			list3.Add("10 Decals");
			list3.Add("11 Collars and Inner Shirts");
			NativeMenu nativeMenu = new NativeMenu("", "Change Outfit");
			modLemonUIObjectPool.Add(nativeMenu);
			WoredrobeMenu.AddSubMenu(nativeMenu);
			NativeListItem<dynamic> O = new NativeListItem<object>("", Outfits.ToArray());
			nativeMenu.Add(O);
			O.Description = "While Using this outfit you will not be able to purchase anything due to being the MP male";
			NativeListItem<dynamic> C = new NativeListItem<object>("Color : ", Colours.ToArray());
			nativeMenu.Add(C);
			C.Description = "Use this Colour Whenever possible or use Default";
			NativeItem nativeItem = new NativeItem("Wear Outfit ");
			nativeMenu.Add(nativeItem);
			nativeItem.Description = "~y~ Warning ~w~ Lag is normal while applying outfits, simple alt tab out to avoid crash";
			NativeItem nativeItem2 = new NativeItem("Remove Outift ");
			nativeMenu.Add(nativeItem2);
			NativeMenu nativeMenu2 = new NativeMenu("", "Change Clothes");
			modLemonUIObjectPool.Add(nativeMenu2);
			WoredrobeMenu.AddSubMenu(nativeMenu2);
			NativeMenu nativeMenu3 = new NativeMenu("", "Save/Load Outfit");
			modLemonUIObjectPool.Add(nativeMenu3);
			WoredrobeMenu.AddSubMenu(nativeMenu3);
			NativeListItem<dynamic> SVL = new NativeListItem<object>("Function ", list.ToArray());
			nativeMenu3.Add(SVL);
			NativeListItem<dynamic> Sl = new NativeListItem<object>("Slot ", Slots.ToArray());
			nativeMenu3.Add(Sl);
			NativeItem Get = new NativeItem("Save");
			nativeMenu3.Add(Get);
			Get.Activated += delegate
			{
				string text = "";
				if (Game.Player.Character.Model == PedHash.Franklin)
				{
					text = "scripts//HKH_SinglePlayerHeists//Arcade//Waredrobe//Franklin//";
				}
				if (Game.Player.Character.Model == PedHash.Michael)
				{
					text = "scripts//HKH_SinglePlayerHeists//Arcade//Waredrobe//Michael//";
				}
				if (Game.Player.Character.Model == PedHash.Trevor)
				{
					text = "scripts//HKH_SinglePlayerHeists//Arcade//Waredrobe//Trevor//";
				}
				if (Game.Player.Character.Model == PedHash.FreemodeFemale01 || Game.Player.Character.Model == PedHash.FreemodeMale01)
				{
					text = "scripts//HKH_SinglePlayerHeists//Arcade//Waredrobe//Mp//";
				}
				if (SVL.SelectedIndex == 0)
				{
					Ped character = Game.Player.Character;
					Get.Title = "Save";
					LoadIniFile(text + Slots[Sl.SelectedIndex]);
					int value3 = Function.Call<int>(Hash.GET_PED_PROP_INDEX, character, 0);
					int value4 = Function.Call<int>(Hash.GET_PED_PROP_TEXTURE_INDEX, character, 0);
					int value5 = Function.Call<int>(Hash.GET_PED_PALETTE_VARIATION, character, 0);
					Config.SetValue("-1 HAT", "Hat_Drawable", value3);
					Config.SetValue("-1 Hat", "Hat_Tex", value4);
					Config.SetValue("-1 Hat", "Hat_Palette", value5);
					int value6 = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, character, 0);
					int value7 = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, character, 0);
					Config.SetValue("0 FACE", "Head_Drawable", value6);
					Config.SetValue("0 FACE", "Head_Palette", value7);
					int value8 = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, character, 1);
					int value9 = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, character, 1);
					Config.SetValue("1 BEARD", "BEARD_Drawable", value8);
					Config.SetValue("1 BEARD", "BEARD_Palette", value9);
					int value10 = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, character, 2);
					int value11 = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, character, 2);
					Config.SetValue("2 HAIRCUT", "HAIRCUT_Drawable", value10);
					Config.SetValue("2 HAIRCUT", "HAIRCUT_Palette", value11);
					int value12 = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, character, 3);
					int value13 = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, character, 3);
					Config.SetValue("3 SHIRT", "SHIRT_Drawable", value12);
					Config.SetValue("3 SHIRT", "SHIRT_Palette", value13);
					int value14 = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, character, 4);
					int value15 = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, character, 4);
					Config.SetValue("4 PANTS", "PANTS_Drawable", value14);
					Config.SetValue("4 PANTS", "PANTS_Palette", value15);
					int value16 = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, character, 5);
					int value17 = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, character, 5);
					Config.SetValue("5 Hands / Gloves", "Gloves_Drawable", value16);
					Config.SetValue("5 Hands / Gloves", "Gloves_Palette", value17);
					int value18 = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, character, 6);
					int value19 = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, character, 6);
					Config.SetValue("6 SHOES", "SHOES_Drawable", value18);
					Config.SetValue("6 SHOES", "SHOES_Palette", value19);
					int value20 = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, character, 7);
					int value21 = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, character, 7);
					Config.SetValue("7 Eyes", "Eyes_Drawable", value20);
					Config.SetValue("7 Eyes", "Eyes_Palette", value21);
					int value22 = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, character, 8);
					int value23 = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, character, 8);
					Config.SetValue("8 Accessories", "Accessories_Drawable", value22);
					Config.SetValue("8 Accessories", "Accessories_Palette", value23);
					int value24 = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, character, 9);
					int value25 = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, character, 9);
					Config.SetValue("9 Mission Items/ Tasks", "Mission_Drawable", value24);
					Config.SetValue("9 Mission Items/ Tasks", "Mission_Palette", value25);
					int value26 = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, character, 10);
					int value27 = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, character, 10);
					Config.SetValue("10 Decals", "Decals_Drawable", value26);
					Config.SetValue("10 Decals", "Decals_Palette", value27);
					int value28 = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, character, 11);
					int value29 = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, character, 11);
					Config.SetValue("11 Collars and Inner Shirts", "InnerShirts_Drawable", value28);
					Config.SetValue("11 Collars and Inner Shirts", "InnerShirts_Palette", value29);
					Config.Save();
					Notification.Show("Outfit saved!");
				}
				if (SVL.SelectedIndex == 1)
				{
					Get.Title = "Load";
					Ped character2 = Game.Player.Character;
					LoadIniFile(text + Slots[Sl.SelectedIndex]);
					int defaultvalue = 0;
					defaultvalue = Config.GetValue("0 FACE", "Head_Drawable", defaultvalue);
					int defaultvalue2 = 0;
					defaultvalue2 = Config.GetValue("0 FACE", "Head_Palette", defaultvalue2);
					int defaultvalue3 = 0;
					defaultvalue3 = Config.GetValue("1 BEARD", "BEARD_Drawable", defaultvalue3);
					int defaultvalue4 = 0;
					defaultvalue4 = Config.GetValue("1 BEARD", "BEARD_Palette", defaultvalue4);
					int defaultvalue5 = 0;
					defaultvalue5 = Config.GetValue("2 HAIRCUT", "HAIRCUT_Drawable", defaultvalue5);
					int defaultvalue6 = 0;
					defaultvalue6 = Config.GetValue("2 HAIRCUT", "HAIRCUT_Palette", defaultvalue6);
					int defaultvalue7 = 0;
					defaultvalue7 = Config.GetValue("3 SHIRT", "SHIRT_Drawable", defaultvalue7);
					int defaultvalue8 = 0;
					defaultvalue8 = Config.GetValue("3 SHIRT", "SHIRT_Palette", defaultvalue8);
					int defaultvalue9 = 0;
					defaultvalue9 = Config.GetValue("4 PANTS", "PANTS_Drawable", defaultvalue9);
					int defaultvalue10 = 0;
					defaultvalue10 = Config.GetValue("4 PANTS", "PANTS_Palette", defaultvalue10);
					int defaultvalue11 = 0;
					defaultvalue11 = Config.GetValue("5 Hands / Gloves", "Gloves_Drawable", defaultvalue11);
					int defaultvalue12 = 0;
					defaultvalue12 = Config.GetValue("5 Hands / Gloves", "Gloves_Palette", defaultvalue12);
					int defaultvalue13 = 0;
					defaultvalue13 = Config.GetValue("6 SHOES", "SHOES_Drawable", defaultvalue13);
					int defaultvalue14 = 0;
					defaultvalue14 = Config.GetValue("6 SHOES", "SHOES_Palette", defaultvalue14);
					int defaultvalue15 = 0;
					defaultvalue15 = Config.GetValue("7 Eyes", "Eyes_Drawable", defaultvalue15);
					int defaultvalue16 = 0;
					defaultvalue16 = Config.GetValue("7 Eyes", "Eyes_Palette", defaultvalue16);
					int defaultvalue17 = 0;
					defaultvalue17 = Config.GetValue("8 Accessories", "Accessories_Drawable", defaultvalue17);
					int defaultvalue18 = 0;
					defaultvalue18 = Config.GetValue("8 Accessories", "Accessories_Palette", defaultvalue18);
					int defaultvalue19 = 0;
					defaultvalue19 = Config.GetValue("9 Mission Items/ Tasks", "Mission_Drawable", defaultvalue19);
					int defaultvalue20 = 0;
					defaultvalue20 = Config.GetValue("9 Mission Items/ Tasks", "Mission_Palette", defaultvalue20);
					int defaultvalue21 = 0;
					defaultvalue21 = Config.GetValue("10 Decals", "Decals_Drawable", defaultvalue21);
					int defaultvalue22 = 0;
					defaultvalue22 = Config.GetValue("10 Decals", "Decals_Palette", defaultvalue22);
					int defaultvalue23 = 0;
					defaultvalue23 = Config.GetValue("11 Collars and Inner Shirts", "InnerShirts_Drawable", defaultvalue23);
					int defaultvalue24 = 0;
					defaultvalue24 = Config.GetValue("11 Collars and Inner Shirts", "InnerShirts_Palette", defaultvalue24);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 0, defaultvalue, defaultvalue2, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 1, defaultvalue3, defaultvalue4, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 2, defaultvalue5, defaultvalue6, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 3, defaultvalue7, defaultvalue8, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 4, defaultvalue9, defaultvalue10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 5, defaultvalue11, defaultvalue12, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 6, defaultvalue13, defaultvalue14, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 7, defaultvalue15, defaultvalue16, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 8, defaultvalue17, defaultvalue18, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 9, defaultvalue19, defaultvalue20, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 10, defaultvalue21, defaultvalue22, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 11, defaultvalue23, defaultvalue24, 1);
					int defaultvalue25 = 0;
					defaultvalue25 = Config.GetValue("-1 HAT", "Hat_Drawable", defaultvalue25);
					int defaultvalue26 = 0;
					defaultvalue26 = Config.GetValue("-1 Hat", "Hat_Tex", defaultvalue26);
					int defaultvalue27 = 0;
					defaultvalue27 = Config.GetValue("-1 Hat", "Hat_Palette", defaultvalue27);
					if (defaultvalue25 >= 1)
					{
						Function.Call(Hash.SET_PED_PROP_INDEX, character2, 0, defaultvalue25, defaultvalue26, defaultvalue27);
					}
					else if (defaultvalue25 < 1)
					{
						Function.Call(Hash.CLEAR_ALL_PED_PROPS, Game.Player.Character);
					}
				}
			};
			if (SVL.SelectedIndex == 0)
			{
				Get.Title = "Save";
			}
			if (SVL.SelectedIndex == 1)
			{
				Get.Title = "Load";
			}
			NativeListItem<dynamic> Comp1 = new NativeListItem<object>("", list3.ToArray());
			nativeMenu2.Add(Comp1);
			NativeListItem<dynamic> Drawable = new NativeListItem<object>("Item : ", Draw.ToArray());
			nativeMenu2.Add(Drawable);
			NativeListItem<dynamic> Texture = new NativeListItem<object>("Texture : ", Tex.ToArray());
			nativeMenu2.Add(Texture);
			nativeItem.Activated += delegate
			{
				int num4 = 0;
				LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//Wardrobe//Weapons.ini");
				WeaponHash[] array3 = (WeaponHash[])Enum.GetValues(typeof(WeaponHash));
				for (int n = 0; n < array3.Length; n++)
				{
					WeaponHash weaponHash2 = array3[n];
					if (Game.Player.Character.Weapons.HasWeapon(weaponHash2))
					{
						Game.Player.Character.Weapons.Select(weaponHash2, false);
						Config.SetValue(weaponHash2.ToString(), "WeaponName", weaponHash2);
						WeaponHash hash2 = Game.Player.Character.Weapons.Current.Hash;
						Liv = Game.Player.Character.Weapons.Current.Tint;
						int num5 = 0;
						WeaponComponentHash[] array4 = (WeaponComponentHash[])Enum.GetValues(typeof(WeaponComponentHash));
						for (int num6 = 0; num6 < array4.Length; num6++)
						{
							WeaponComponentHash weaponComponentHash = array4[num6];
							try
							{
								if (Function.Call<bool>(Hash.DOES_WEAPON_TAKE_WEAPON_COMPONENT, weaponHash2.GetHashCode(), weaponComponentHash.GetHashCode()))
								{
									if (Game.Player.Character.Weapons.Current.Components[weaponComponentHash].Active)
									{
										Config.SetValue(weaponHash2.ToString(), "HasComponent" + num5, true);
										Config.SetValue(weaponHash2.ToString(), "Component" + num5, weaponComponentHash);
										num5++;
										Config.Save();
									}
									if (!Game.Player.Character.Weapons.Current.Components[weaponComponentHash].Active)
									{
										Config.SetValue(weaponHash2.ToString(), "HasComponent" + num5, false);
										Config.SetValue(weaponHash2.ToString(), "Component" + num5, weaponComponentHash);
										num5++;
										Config.Save();
									}
								}
							}
							catch
							{
								num5++;
								Notification.Show("~y~ Warning ~w~: Weapon : " + weaponHash2.ToString() + " Failed to save");
							}
						}
						Config.SetValue(weaponHash2.ToString(), "Tint", Liv);
						Config.Save();
					}
				}
				ID_O = O.SelectedIndex;
				ID_C = Colours[C.SelectedIndex];
				Setoutfit(O.SelectedIndex);
				Notification.Show("~y~ Warning ~w~ Lag is normal while applying outfits");
				Script.Wait(2000);
				Notification.Show("Player is wearing outfit : ~y~" + Outfits[O.SelectedIndex] + "~w~ with colour : ~y~" + Colours[C.SelectedIndex] + "~y~");
				LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//Wardrobe//Weapons.ini");
				WeaponHash[] array5 = (WeaponHash[])Enum.GetValues(typeof(WeaponHash));
				for (int num7 = 0; num7 < array5.Length; num7++)
				{
					WeaponHash weaponHash3 = array5[num7];
					WeaponHash value2 = Config.GetValue(weaponHash3.ToString(), "WeaponName", weaponHash3);
					if (weaponHash3 == value2)
					{
						Game.Player.Character.Weapons.Give(weaponHash3, 9999, true, true);
						Game.Player.Character.Weapons.Select(weaponHash3, true);
						Liv = Config.GetValue(weaponHash3.ToString(), "Tint", Liv);
						WeaponHash hash3 = Game.Player.Character.Weapons.Current.Hash;
						Game.Player.Character.Weapons.Current.Tint = Liv;
						Comp = 0;
						WeaponComponentHash[] array6 = (WeaponComponentHash[])Enum.GetValues(typeof(WeaponComponentHash));
						for (int num8 = 0; num8 < array6.Length; num8++)
						{
							WeaponComponentHash componentHash2 = array6[num8];
							try
							{
								if (Function.Call<bool>(Hash.DOES_WEAPON_TAKE_WEAPON_COMPONENT, weaponHash3.GetHashCode(), componentHash2.GetHashCode()))
								{
									if (Config.GetValue(weaponHash3.ToString(), "HasComponent" + Comp, true))
									{
										Game.Player.Character.Weapons.Current.Components[componentHash2].Active = true;
										Comp++;
									}
									else if (!Config.GetValue(weaponHash3.ToString(), "HasComponent" + Comp, true))
									{
										Comp++;
									}
								}
							}
							catch
							{
								Comp++;
							}
						}
					}
				}
				Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 177293209, 9999);
				Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 3686625920u, 9999);
				Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 4208062921u, 9999);
				Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 2024373456, 9999);
				Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 961495388, 9999);
				Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 3219281620u, 9999);
				Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 1432025498, 9999);
				Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, -2009644972, 9999);
				Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, -879347409, 9999);
				Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, -1768145561, 9999);
				Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, -2066285827, 9999);
				Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 1785463520, 9999);
				Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 9999, true, true);
			};
			nativeItem2.Activated += delegate
			{
				if (Game.Player.Character.Model == PedHash.FreemodeMale01)
				{
					Notification.Show("taking off Outfit, this may take some time, lag spikes are normal");
					Model model = new Model(OldCharacter.Hash);
					model.Request(500);
					if (model.IsInCdImage && model.IsValid)
					{
						while (!model.IsLoaded)
						{
							Script.Wait(100);
						}
						Function.Call(Hash.SET_PLAYER_MODEL, Game.Player, model.Hash);
					}
					model.MarkAsNoLongerNeeded();
					LoadIniFile("scripts//Juggernaut.ini");
					WeaponHash[] array = (WeaponHash[])Enum.GetValues(typeof(WeaponHash));
					for (int l = 0; l < array.Length; l++)
					{
						WeaponHash weaponHash = array[l];
						WeaponHash value = Config.GetValue(weaponHash.ToString(), "WeaponName", weaponHash);
						if (weaponHash == value)
						{
							Game.Player.Character.Weapons.Give(weaponHash, 9999, true, true);
							Game.Player.Character.Weapons.Select(weaponHash, true);
							Liv = Config.GetValue(weaponHash.ToString(), "Tint", Liv);
							WeaponHash hash = Game.Player.Character.Weapons.Current.Hash;
							Game.Player.Character.Weapons.Current.Tint = Liv;
							Comp = 0;
							WeaponComponentHash[] array2 = (WeaponComponentHash[])Enum.GetValues(typeof(WeaponComponentHash));
							for (int m = 0; m < array2.Length; m++)
							{
								WeaponComponentHash componentHash = array2[m];
								try
								{
									if (Function.Call<bool>(Hash.DOES_WEAPON_TAKE_WEAPON_COMPONENT, weaponHash.GetHashCode(), componentHash.GetHashCode()))
									{
										if (Config.GetValue(weaponHash.ToString(), "HasComponent" + Comp, true))
										{
											Game.Player.Character.Weapons.Current.Components[componentHash].Active = true;
											Comp++;
										}
										else if (!Config.GetValue(weaponHash.ToString(), "HasComponent" + Comp, true))
										{
											Comp++;
										}
									}
								}
								catch
								{
									Comp++;
								}
							}
						}
					}
					Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 177293209, 9999);
					Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 3686625920u, 9999);
					Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 4208062921u, 9999);
					Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 2024373456, 9999);
					Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 961495388, 9999);
					Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 3219281620u, 9999);
					Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 1432025498, 9999);
					Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, -2009644972, 9999);
					Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, -879347409, 9999);
					Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, -1768145561, 9999);
					Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, -2066285827, 9999);
					Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 1785463520, 9999);
					Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 9999, true, true);
					Notification.Show("Removed Outfit!");
				}
			};
			O.ItemChanged += delegate
			{
				ID_O = O.SelectedIndex;
			};
			C.ItemChanged += delegate
			{
				if (C == O)
				{
					ID_C = Colours[C.SelectedIndex];
				}
			};
			Comp1.ItemChanged += delegate
			{
				Draw.Clear();
				Tex.Clear();
				int num2 = CheckClothes(1, Comp1.SelectedIndex, 0);
				if (num2 > 0)
				{
					for (int j = 0; j < num2; j++)
					{
						Draw.Add(j);
					}
				}
				int num3 = CheckClothes(2, Comp1.SelectedIndex, 0);
				if (num3 > 0)
				{
					for (int k = 0; k < num3; k++)
					{
						Tex.Add(k);
					}
				}
			};
			Drawable.ItemChanged += delegate
			{
				Tex.Clear();
				int num = CheckClothes(2, Comp1.SelectedIndex, 0);
				if (num > 0)
				{
					for (int i = 0; i < num; i++)
					{
						Tex.Add(i);
					}
				}
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, Comp1.SelectedIndex, Drawable.SelectedIndex, Texture.SelectedIndex, 1);
			};
			Texture.ItemChanged += delegate
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, Comp1.SelectedIndex, Drawable.SelectedIndex, Texture.SelectedIndex, 1);
			};
		}

		public void DrillScreen(float z)
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int num = Convert.ToInt32(absoluteX / 2f);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, scale, 255, 255, 255, 255);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_TEMPERATURE", Temp, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_DRILL_POSITION", Pos, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_SPEED", Speed, -1082130432, -1082130432, -1082130432, -1082130432);
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 32, 0), "Pivot Up");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 33, 0), "Pivot Down");
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 76, 0), "Use Drill");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
			GTA.UI.Screen.ShowSubtitle("Pos: " + Pos + ", Speed: " + Speed + ", Temp: " + Temp + ", Temp_2: " + Temp);
		}

		public void DrillScreenStart()
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int num = Convert.ToInt32(absoluteX / 2f);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, scale, 255, 255, 255, 255);
			Speed = 0f;
			Temp = 0f;
			Pos = 0f;
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_TEMPERATURE", Temp, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_DRILL_POSITION", Pos, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_SPEED", Speed, -1082130432, -1082130432, -1082130432, -1082130432);
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 32, 0), "Pivot Up");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 33, 0), "Pivot Down");
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 76, 0), "Use Drill");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
			scaleform.Dispose();
			scaleform.Dispose();
		}

		public void SetupHeiststuff()
		{
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

		private void Loadehacker(bool playBoth)
		{
			PlayBoth = playBoth;
			IpTryFailedTimer = Game.GameTime;
			TRYDL = false;
			Hacking_Lives = 3f;
			IpTry = false;
			DrawHackScaleform = false;
			Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
			Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "HACK", 3);
			hackingScalwform = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_SKIP_RENDER_WHILE_PAUSED, "Hacking_PC");
			while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, hackingScalwform))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_CURSOR_VISIBILITY");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, false);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HACKING_PC_desktop_0", 0);
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HACKING_PC_desktop_0"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HACKING_PC_desktop_1", 0);
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HACKING_PC_desktop_1"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HACKING_PC_desktop_2", 0);
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HACKING_PC_desktop_2"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HACKING_PC_desktop_3", 0);
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HACKING_PC_desktop_3"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HACKING_PC_desktop_4", 0);
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HACKING_PC_desktop_4"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HACKING_PC_desktop_5", 0);
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HACKING_PC_desktop_5"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_ANIM_DICT, "anim@heists@keypad@");
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@keypad@"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel@hacking");
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel@hacking"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_ANIM_DICT, "anim@heists@ornate_bank@hack");
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@ornate_bank@hack"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_ANIM_DICT, "anim@heists@keypad@");
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@keypad@"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel@hacking");
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel@hacking"))
			{
				Script.Wait(10);
			}
			Hacking_SelectedIndex = 0;
		}

		private unsafe void UnloadLoadehacker()
		{
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, IPSound))
			{
				Function.Call(Hash.STOP_SOUND, IPSound);
				IPSound = -1;
			}
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, MoveSound))
			{
				Function.Call(Hash.STOP_SOUND, MoveSound);
				MoveSound = -1;
			}
			int num = hackingScalwform;
			Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &num);
			hackingScalwform = 0;
			Hacking_SelectedIndex = -1;
			Game.Player.CanControlCharacter = true;
			Script.Wait(1000);
			Game.Player.Character.Task.Wait(0);
		}

		public void runCasinoDrill(int scaleform)
		{
			if (firsttime)
			{
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD, scaleform, "REVEAL");
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_HOLE_DEPTH", DrillPos - 0.09f, -1082130432, -1082130432, -1082130432, -1082130432);
				firsttime = false;
			}
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_POSITION", 1f, 0.725f, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_NUM_DISCS", NumOfDiscs, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_SPEED", drillspeed, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_DRILL_POSITION", DrillPos, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_TEMPERATURE", temperature, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, scaleform, 255, 255, 255, 255, 0);
		}

		public void runCasinoLaser(int scaleform)
		{
			if (firsttime)
			{
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD, scaleform, "REVEAL");
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_HOLE_DEPTH", DrillPos - 0.09f, -1082130432, -1082130432, -1082130432, -1082130432);
				firsttime = false;
			}
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, scaleform, "SET_LASER_VISIBLE");
			if (drillspeed > 0.01f)
			{
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, true);
			}
			else
			{
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, false);
			}
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_POSITION", 1f, 0.725f, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_NUM_DISCS", NumOfDiscs, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_SPEED", 1f, -1082130432, -1082130432, -1082130432, -1082130432);
			if (drillspeed != 99f)
			{
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_LASER_WIDTH", DrillPos, -1082130432, -1082130432, -1082130432, -1082130432);
			}
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_DRILL_POSITION", DrillPos, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleform, "SET_TEMPERATURE", temperature, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, scaleform, 255, 255, 255, 255, 0);
		}

		public int activatescaleCas(string scale)
		{
			scaleformloading = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, scale);
			if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, scaleformloading))
			{
				return 1;
			}
			return 0;
		}

		public void DestroyScaleform(string scale)
		{
			Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, scale);
			scaleformloading = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, scale);
		}

		public void DestoryDrill()
		{
			if (scaleformloading != -1)
			{
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD, scaleformloading, "RESET");
				scaleformloading = -1;
			}
			Function.Call(Hash.STOP_SOUND, Sound);
			Function.Call(Hash.STOP_SOUND, FailSound);
			Function.Call(Hash.RELEASE_SOUND_ID, Sound);
			Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
			Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_HEIST3\\HEIST_FINALE_LASER_DRILL");
			Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL");
			Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2");
			PlayDrillFXSound = false;
			pin_one = false;
			pin_two = false;
			pin_three = false;
			pin_four = false;
			pin_five = false;
			pin_six = false;
			pin_seven = false;
			pin_eight = false;
			pin_six = false;
			pin_seven = false;
			pin_eight = false;
			UseCasinoDrill = false;
			activeCasDrill = false;
			activeCasLaser = false;
			triggerLockA = false;
			triggerLockB = false;
			triggerLockC = false;
			triggerLockD = false;
			Speed = 0f;
			Temp = 0f;
			Pos = 0f;
			Speed = 0f;
			Temp = 0f;
			Zone = 551f;
			Sound = -1;
			if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
			{
				Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
			}
			if (Drill != null)
			{
				Drill.Position = new Vector3(1000f, 1000f, -300f);
				Script.Wait(500);
				Drill.Delete();
			}
			if (Drill != null)
			{
				Drill.Delete();
			}
			Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 0, true, true);
			Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, false);
			Game.Player.Character.IsPositionFrozen = true;
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@fleeca_bank@drilling") && !Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "fm_mission_controler") && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SAFE_CRACK", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HUD_MINI_GAME_SOUNDSET", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "MissionFailedSounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "dlc_heist_fleeca_bank_door_sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "vault_door", 0, -1) && !Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "Alarms", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 0, -1))
			{
				Script.Wait(100);
			}
			Script.Wait(1);
			triggerLockA = false;
			triggerLockB = false;
			triggerLockC = false;
			triggerLockD = false;
			Speed = 0f;
			Temp = 0f;
			Pos = 0f;
			Speed = 0f;
			Temp = 0f;
			scale = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_INSTANCE, "DRILLING");
			while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, scale))
			{
				Script.Wait(100);
			}
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_SPEED", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_HOLE_DEPTH", 0.1f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_DRILL_POSITION", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_TEMPERATURE", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Zone = 551f;
			if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
			{
				Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
			}
			if (Drill != null)
			{
				Drill.Delete();
			}
			Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 0, true, true);
			Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, false);
			Game.Player.Character.IsPositionFrozen = true;
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@fleeca_bank@drilling") && !Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "fm_mission_controler") && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SAFE_CRACK", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HUD_MINI_GAME_SOUNDSET", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "MissionFailedSounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "dlc_heist_fleeca_bank_door_sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "vault_door", 0, -1) && !Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "Alarms", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 0, -1))
			{
				Script.Wait(100);
			}
			Model model = new Model("hei_prop_heist_drill");
			model.Request(10000);
			Prop prop = GlobalHelper.CreateProp(model, Game.Player.Character.Position, false, false);
			prop.IsVisible = false;
			Isdrilliing = true;
			prop.IsPositionFrozen = true;
			prop.IsCollisionEnabled = false;
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
			prop.IsInvincible = true;
			model.MarkAsNoLongerNeeded();
			Drill = prop;
			Vector3 vector = Game.Player.Character.Position + Game.Player.Character.ForwardVector * 1f;
			Script.Wait(1);
			if (Drill != null)
			{
				Drill.Position = new Vector3(1000f, 1000f, -300f);
				Script.Wait(500);
				if (Drill != null)
				{
					Drill.Delete();
				}
				if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
				{
					Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
				}
				Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 0, true, true);
				Speed = 0f;
				Temp = 0f;
				Pos = 0f;
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_TEMPERATURE", Temp, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_DRILL_POSITION", Pos, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_SPEED", Speed, -1082130432, -1082130432, -1082130432, -1082130432);
				Isdrilliing = false;
				Game.Player.Character.Task.ClearAll();
				Game.Player.Character.Task.ClearAllImmediately();
				Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, true);
				Game.Player.Character.IsPositionFrozen = false;
			}
			GTA.UI.Screen.ShowSubtitle("~b~ Drill Delete Complete!");
		}

		public void StartFleecaDrill()
		{
			Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
			Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
			DestroyScaleform("DRILLING");
			DestoryDrill();
			DrillingComplete = false;
			UseCasinoDrill = false;
			activeCasDrill = false;
			activeCasLaser = false;
			triggerLockA = false;
			triggerLockB = false;
			triggerLockC = false;
			triggerLockD = false;
			pin_one = false;
			pin_two = false;
			pin_three = false;
			pin_four = false;
			pin_five = false;
			pin_six = false;
			pin_seven = false;
			pin_eight = false;
			pin_six = false;
			pin_seven = false;
			pin_eight = false;
			temperature = 0f;
			drillspeed = 0f;
			drillspeed = 0f;
			Speed = 0f;
			Temp = 0f;
			Pos = 0f;
			if (Isdrilliing)
			{
				DrillScreenStart();
			}
			Speed = 0f;
			Temp = 0f;
			scale = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_INSTANCE, "DRILLING");
			while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, scale))
			{
				Script.Wait(100);
			}
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_SPEED", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_HOLE_DEPTH", 0.1f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_DRILL_POSITION", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_TEMPERATURE", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Zone = 551f;
			Function.Call(Hash.STOP_SOUND, Sound);
			Function.Call(Hash.STOP_SOUND, FailSound);
			Function.Call(Hash.RELEASE_SOUND_ID, Sound);
			Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
			if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
			{
				Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
			}
			Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 0, true, true);
			Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, false);
			Game.Player.Character.IsPositionFrozen = true;
			Sound = GlobalHelper.CreateSoundID();
			PinSound = GlobalHelper.CreateSoundID();
			FailSound = GlobalHelper.CreateSoundID();
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@fleeca_bank@drilling") && !Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "fm_mission_controler") && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SAFE_CRACK", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HUD_MINI_GAME_SOUNDSET", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "MissionFailedSounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "dlc_heist_fleeca_bank_door_sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "vault_door", 0, -1) && !Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "Alarms", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 0, -1))
			{
				Script.Wait(100);
			}
			if (Drill != null)
			{
				Drill.Delete();
			}
			Model model = new Model("hei_prop_heist_drill");
			model.Request(10000);
			Prop prop = GlobalHelper.CreateProp(model, Game.Player.Character.Position, false, false);
			Isdrilliing = true;
			prop.IsPositionFrozen = true;
			prop.IsCollisionEnabled = false;
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
			prop.IsInvincible = true;
			model.MarkAsNoLongerNeeded();
			Drill = prop;
			Vector3 vector = Game.Player.Character.Position + Game.Player.Character.ForwardVector * 1f;
			Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_right_end", 1f, 1f, -1, AnimationFlags.Loop, -1f);
			GTA.UI.Screen.ShowSubtitle("~b~ Drill Start Complete!");
		}

		public void StartCasinoDrill()
		{
			Drillstage = 0;
			Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST3\\HEIST_FINALE_LASER_DRILL", false, -1);
			Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
			Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
			DestroyScaleform("VAULT_DRILL");
			DestoryDrill();
			if (!Isdrilliing)
			{
				PlayDrillFXSound = false;
				DrillingComplete = false;
				Temp = 0f;
				firsttime = false;
				UseCasinoDrill = false;
				activeCasDrill = false;
				activeCasLaser = false;
				Isdrilliing = false;
				triggerLockA = false;
				triggerLockB = false;
				triggerLockC = false;
				triggerLockD = false;
				activatescaleCas("VAULT_DRILL");
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_NUM_DISCS", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_HOLE_DEPTH", 99f, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_SPEED", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_DRILL_POSITION", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_TEMPERATURE", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_LASER_WIDTH", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_POSITION", 1f, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, scaleformloading, 255, 255, 255, 255, 0);
				Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_LASER_4", 1);
				depth = DrillPos - 0.09f;
				drillspeed = 0f;
				DrillPos = 0f;
				temperature = 0f;
				activeCasDrill = !activeCasDrill;
				UseCasinoDrill = true;
			}
			triggerLockA = false;
			triggerLockB = false;
			triggerLockC = false;
			triggerLockD = false;
			pin_one = false;
			pin_two = false;
			pin_three = false;
			pin_four = false;
			pin_five = false;
			temperature = 0f;
			drillspeed = 0f;
			drillspeed = 0f;
			Speed = 0f;
			Temp = 0f;
			Pos = 0f;
			if (!UseCasinoDrill && Isdrilliing)
			{
				DrillScreenStart();
			}
			Speed = 0f;
			Temp = 0f;
			scale = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_INSTANCE, "DRILLING");
			while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, scale))
			{
				Script.Wait(0);
			}
			activatescaleCas("VAULT_DRILL");
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_NUM_DISCS", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_HOLE_DEPTH", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_SPEED", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_DRILL_POSITION", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_TEMPERATURE", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_LASER_WIDTH", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_POSITION", 1f, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, scaleformloading, 255, 255, 255, 255, 0);
			Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_DRILL_4", 1);
			Zone = 551f;
			Function.Call(Hash.STOP_SOUND, Sound);
			Function.Call(Hash.STOP_SOUND, FailSound);
			Function.Call(Hash.RELEASE_SOUND_ID, Sound);
			Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
			if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
			{
				Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
			}
			Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 0, true, true);
			Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, false);
			Game.Player.Character.IsPositionFrozen = true;
			Sound = GlobalHelper.CreateSoundID();
			PinSound = GlobalHelper.CreateSoundID();
			FailSound = GlobalHelper.CreateSoundID();
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@fleeca_bank@drilling") && !Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "fm_mission_controler") && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SAFE_CRACK", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HUD_MINI_GAME_SOUNDSET", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "MissionFailedSounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "dlc_heist_fleeca_bank_door_sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "vault_door", 0, -1) && !Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "Alarms", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 0, -1))
			{
				Script.Wait(0);
			}
			if (Drill != null)
			{
				Drill.Delete();
			}
			Model model = new Model("hei_prop_heist_drill");
			model.Request(10000);
			Prop prop = GlobalHelper.CreateProp(model, Game.Player.Character.Position, false, false);
			Isdrilliing = true;
			prop.IsPositionFrozen = true;
			prop.IsCollisionEnabled = false;
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
			prop.IsInvincible = true;
			model.MarkAsNoLongerNeeded();
			Drill = prop;
			Vector3 vector = Game.Player.Character.Position + Game.Player.Character.ForwardVector * 1f;
			Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_right_end", 1f, 1f, -1, AnimationFlags.Loop, -1f);
			firsttime = true;
			GTA.UI.Screen.ShowSubtitle("~b~ Drill Start Complete!");
		}

		public void StartCasinoLaser()
		{
			Drillstage = 0;
			Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST3\\HEIST_FINALE_LASER_DRILL", false, -1);
			Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST3\\HEIST_FINALE_LASER_DRILL", false, -1);
			Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
			DestroyScaleform("VAULT_LASER");
			DestoryDrill();
			if (!Isdrilliing)
			{
				PlayDrillFXSound = false;
				DrillingComplete = false;
				Temp = 0f;
				firsttime = false;
				UseCasinoDrill = false;
				activeCasDrill = false;
				activeCasLaser = false;
				Isdrilliing = false;
				triggerLockA = false;
				triggerLockB = false;
				triggerLockC = false;
				triggerLockD = false;
				activatescaleCas("VAULT_LASER");
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_NUM_DISCS", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_HOLE_DEPTH", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_SPEED", 0, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_DRILL_POSITION", 0, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_TEMPERATURE", 0, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_LASER_WIDTH", 0, -1082130432, -1082130432, -1082130432, -1082130432);
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_POSITION", 1f, -1082130432, -1082130432, -1082130432);
				depth = DrillPos - 0.09f;
				drillspeed = 0f;
				DrillPos = 0f;
				temperature = 0f;
				Game.Player.Character.IsPositionFrozen = true;
				activeCasLaser = !activeCasLaser;
				UseCasinoDrill = true;
			}
			triggerLockA = false;
			triggerLockB = false;
			triggerLockC = false;
			triggerLockD = false;
			pin_one = false;
			pin_two = false;
			pin_three = false;
			pin_four = false;
			pin_five = false;
			pin_six = false;
			pin_seven = false;
			pin_eight = false;
			temperature = 0f;
			drillspeed = 0f;
			drillspeed = 0f;
			Speed = 0f;
			Temp = 0f;
			Pos = 0f;
			if (!UseCasinoDrill && Isdrilliing)
			{
				DrillScreenStart();
			}
			Speed = 0f;
			Temp = 0f;
			scale = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_INSTANCE, "DRILLING");
			while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, scale))
			{
				Script.Wait(0);
			}
			activatescaleCas("VAULT_LASER");
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_NUM_DISCS", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_HOLE_DEPTH", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_SPEED", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_DRILL_POSITION", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_TEMPERATURE", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_LASER_WIDTH", -1f, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scaleformloading, "SET_POSITION", 1f, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, scaleformloading, 255, 255, 255, 255, 0);
			Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_LASER_4", 1);
			Zone = 551f;
			Function.Call(Hash.STOP_SOUND, Sound);
			Function.Call(Hash.STOP_SOUND, FailSound);
			Function.Call(Hash.RELEASE_SOUND_ID, Sound);
			Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
			if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
			{
				Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
			}
			Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 0, true, true);
			Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, false);
			Game.Player.Character.IsPositionFrozen = true;
			Sound = GlobalHelper.CreateSoundID();
			PinSound = GlobalHelper.CreateSoundID();
			FailSound = GlobalHelper.CreateSoundID();
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@fleeca_bank@drilling") && !Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "fm_mission_controler") && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SAFE_CRACK", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HUD_MINI_GAME_SOUNDSET", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "MissionFailedSounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "dlc_heist_fleeca_bank_door_sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "vault_door", 0, -1) && !Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "Alarms", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 0, -1))
			{
				Script.Wait(0);
			}
			if (Drill != null)
			{
				Drill.Delete();
			}
			Model model = new Model("ch_prop_laserdrill_01a");
			model.Request(10000);
			Prop prop = GlobalHelper.CreateProp(model, Game.Player.Character.Position, false, false);
			Isdrilliing = true;
			prop.IsPositionFrozen = true;
			prop.IsCollisionEnabled = false;
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
			prop.IsInvincible = true;
			model.MarkAsNoLongerNeeded();
			Drill = prop;
			Vector3 vector = Game.Player.Character.Position + Game.Player.Character.ForwardVector * 1f;
			Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_right_end", 1f, 1f, -1, AnimationFlags.Loop, -1f);
			firsttime = true;
		}

		public static void TextNotification(string avatar, string author, string title, string message)
		{
			Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "CONFIRM_BEEP", "HUD_MINI_GAME_SOUNDSET");
			Function.Call(Hash.BEGIN_TEXT_COMMAND_THEFEED_POST, new InputArgument[1] { "STRING" });
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { message });
			int num = Function.Call<int>(Hash.END_TEXT_COMMAND_THEFEED_POST_MESSAGETEXT, new InputArgument[6] { avatar, avatar, true, 0, title, author });
		}

		public void AddCash(int min, int max)
		{
			Random random = new Random();
			float num = random.Next(min, max);
			CurrentPay += (int)num;
			Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
			Audio.PlaySoundFrontend("LOCAL_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
			Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
			GTA.UI.Screen.ShowSubtitle("Collected : $" + num.ToString("N") + " Total : $" + CurrentPay.ToString("N"));
		}

		public bool InttoBool(int i)
		{
			bool result = false;
			if (i == 0)
			{
				result = false;
			}
			if (i == 1)
			{
				result = true;
			}
			return result;
		}

		public int BoolToInt(bool i)
		{
			int result = 0;
			if (!i)
			{
				result = 0;
			}
			if (i)
			{
				result = 1;
			}
			return result;
		}

		public Model SpawnProp(string Prop)
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

		public void SetupINDArcade(int Ver, int ArcNo)
		{
			Vector3 vector = new Vector3(0f, 0f, 0f);
			float z = 0f;
			if (ArcNo == 1)
			{
				vector = ArcadePos1;
				z = ArcadeRot1;
			}
			if (ArcNo == 2)
			{
				vector = ArcadePos2;
				z = ArcadeRot2;
			}
			if (ArcNo == 3)
			{
				vector = ArcadePos3;
				z = ArcadeRot3;
			}
			if (ArcNo == 4)
			{
				vector = ArcadePos4;
				z = ArcadeRot4;
			}
			if (ArcNo == 5)
			{
				vector = ArcadePos5;
				z = ArcadeRot5;
			}
			if (ArcNo == 6)
			{
				vector = ArcadePos6;
				z = ArcadeRot6;
			}
			if (ArcNo == 7)
			{
				vector = ArcadePos7;
				z = ArcadeRot7;
			}
			if (ArcNo == 8)
			{
				vector = ArcadePos8;
				z = ArcadeRot8;
			}
			if (ArcNo == 9)
			{
				vector = ArcadePos9;
				z = ArcadeRot9;
			}
			if (ArcNo == 10)
			{
				vector = ArcadePos10;
				z = ArcadeRot10;
			}
			if (ArcNo == 11)
			{
				vector = ArcadePos11;
				z = ArcadeRot11;
			}
			if (ArcNo == 12)
			{
				vector = ArcadePos12;
				z = ArcadeRot12;
			}
			if (ArcNo == 13)
			{
				vector = ArcadePos13;
				z = ArcadeRot13;
			}
			if (ArcNo == 14)
			{
				vector = ArcadePos14;
				z = ArcadeRot14;
			}
			if (ArcNo == 15)
			{
				vector = ArcadePos15;
				z = ArcadeRot15;
			}
			if (ArcNo == 16)
			{
				vector = ArcadePos16;
				z = ArcadeRot16;
			}
			if (ArcNo == 17)
			{
				vector = ArcadePos17;
				z = ArcadeRot17;
			}
			if (ArcNo == 18)
			{
				vector = ArcadePos18;
				z = ArcadeRot18;
			}
			if (ArcNo == 19)
			{
				vector = ArcadePos19;
				z = ArcadeRot19;
			}
			if (ArcNo == 20)
			{
				vector = ArcadePos20;
				z = ArcadeRot20;
			}
			if (Ver == 0 && ArcadeProps.Count > 0)
			{
				foreach (Prop arcadeProp in ArcadeProps)
				{
					if (GetDistance(arcadeProp.Position, vector) < 0.15f)
					{
						arcadeProp.Delete();
					}
				}
			}
			if (Ver != 0 && Ver < 14)
			{
				FakeArcade = GlobalHelper.CreateProp(RequestModel(ArcadePropNames[Ver]), vector, new Vector3(0f, 0f, z), true, false);
				ArcadeProps.Add(FakeArcade);
				if (ArcadeProps.Count > 0)
				{
					foreach (Prop arcadeProp2 in ArcadeProps)
					{
						if (Ver != 4 && GetDistance(arcadeProp2.Position, FakeArcade.Position) < 0.15f && arcadeProp2 != FakeArcade)
						{
							arcadeProp2.Delete();
						}
						if ((Ver == 4 || Ver == 5) && GetDistance(arcadeProp2.Position, FakeArcade.Position) < 2f && arcadeProp2 != FakeArcade)
						{
							arcadeProp2.Delete();
						}
					}
				}
			}
			if (Ver == 14)
			{
				FakeArcade = GlobalHelper.CreateProp(RequestModel("ch_prop_arcade_race_01a"), vector, new Vector3(0f, 0f, z), true, false);
				ArcadeProps.Add(FakeArcade);
				if (ArcadeProps.Count > 0)
				{
					foreach (Prop arcadeProp3 in ArcadeProps)
					{
						if (GetDistance(arcadeProp3.Position, FakeArcade.Position) < 2f && arcadeProp3 != FakeArcade)
						{
							arcadeProp3.Delete();
						}
					}
				}
				Vector3 offsetPosition = FakeArcade.GetOffsetPosition(new Vector3(-0.65f, 0f, 0.15f));
				Vector3 offsetPosition2 = FakeArcade.GetOffsetPosition(new Vector3(0.65f, 0f, 0.15f));
				Prop prop = GlobalHelper.CreateProp(RequestModel("ch_prop_arcade_race_car_01b"), offsetPosition, new Vector3(0f, 0f, FakeArcade.Rotation.Z), true, false);
				SeconardyArcadeProps.Add(prop);
				prop.IsPositionFrozen = true;
				Prop prop2 = GlobalHelper.CreateProp(RequestModel("ch_prop_arcade_race_car_01a"), offsetPosition2, new Vector3(0f, 0f, FakeArcade.Rotation.Z), true, false);
				SeconardyArcadeProps.Add(prop2);
				prop2.IsPositionFrozen = true;
				RaceArcade item = new RaceArcade(FakeArcade, prop, prop2, "Car");
				RacingArcades.Add(item);
			}
			if (Ver == 15)
			{
				FakeArcade = GlobalHelper.CreateProp(RequestModel("ch_prop_arcade_race_01b"), vector, new Vector3(0f, 0f, z), true, false);
				ArcadeProps.Add(FakeArcade);
				if (ArcadeProps.Count > 0)
				{
					foreach (Prop arcadeProp4 in ArcadeProps)
					{
						if (GetDistance(arcadeProp4.Position, FakeArcade.Position) < 2f && arcadeProp4 != FakeArcade)
						{
							arcadeProp4.Delete();
						}
					}
				}
				Vector3 offsetPosition3 = FakeArcade.GetOffsetPosition(new Vector3(-0.65f, 0f, 0.15f));
				Vector3 offsetPosition4 = FakeArcade.GetOffsetPosition(new Vector3(0.65f, 0f, 0.15f));
				Prop prop3 = GlobalHelper.CreateProp(RequestModel("ch_prop_arcade_race_truck_01a"), offsetPosition3, new Vector3(0f, 0f, FakeArcade.Rotation.Z), true, false);
				SeconardyArcadeProps.Add(prop3);
				prop3.IsPositionFrozen = true;
				Prop prop4 = GlobalHelper.CreateProp(RequestModel("ch_prop_arcade_race_truck_01b"), offsetPosition4, new Vector3(0f, 0f, FakeArcade.Rotation.Z), true, false);
				SeconardyArcadeProps.Add(prop4);
				prop4.IsPositionFrozen = true;
				RaceArcade item2 = new RaceArcade(FakeArcade, prop3, prop4, "Car");
				RacingArcades.Add(item2);
			}
			if (Ver != 16)
			{
				return;
			}
			FakeArcade = GlobalHelper.CreateProp(RequestModel("ch_prop_arcade_race_02a"), vector, new Vector3(0f, 0f, z), true, false);
			ArcadeProps.Add(FakeArcade);
			if (ArcadeProps.Count > 0)
			{
				foreach (Prop arcadeProp5 in ArcadeProps)
				{
					if (GetDistance(arcadeProp5.Position, FakeArcade.Position) < 2f && arcadeProp5 != FakeArcade)
					{
						arcadeProp5.Delete();
					}
				}
			}
			Vector3 offsetPosition5 = FakeArcade.GetOffsetPosition(new Vector3(-0.65f, 0f, 0.15f));
			Vector3 offsetPosition6 = FakeArcade.GetOffsetPosition(new Vector3(0.65f, 0f, 0.15f));
			Prop prop5 = GlobalHelper.CreateProp(RequestModel("ch_prop_arcade_race_bike_02a"), offsetPosition5, new Vector3(0f, 0f, FakeArcade.Rotation.Z), true, false);
			SeconardyArcadeProps.Add(prop5);
			prop5.IsPositionFrozen = true;
			Prop prop6 = GlobalHelper.CreateProp(RequestModel("ch_prop_arcade_race_bike_02a"), offsetPosition6, new Vector3(0f, 0f, FakeArcade.Rotation.Z), true, false);
			SeconardyArcadeProps.Add(prop6);
			prop6.IsPositionFrozen = true;
			RaceArcade item3 = new RaceArcade(FakeArcade, prop5, prop6, "Bike");
			RacingArcades.Add(item3);
		}

		public void SetupFakeArcade(int Ver, int ArcNo, string arcid)
		{
			try
			{
				Vector3 vector = new Vector3(0f, 0f, 0f);
				float z = 0f;
				if (ArcNo == 1)
				{
					vector = ArcadePos1;
					z = ArcadeRot1;
				}
				if (ArcNo == 2)
				{
					vector = ArcadePos2;
					z = ArcadeRot2;
				}
				if (ArcNo == 3)
				{
					vector = ArcadePos3;
					z = ArcadeRot3;
				}
				if (ArcNo == 4)
				{
					vector = ArcadePos4;
					z = ArcadeRot4;
				}
				if (ArcNo == 5)
				{
					vector = ArcadePos5;
					z = ArcadeRot5;
				}
				if (ArcNo == 6)
				{
					vector = ArcadePos6;
					z = ArcadeRot6;
				}
				if (ArcNo == 7)
				{
					vector = ArcadePos7;
					z = ArcadeRot7;
				}
				if (ArcNo == 8)
				{
					vector = ArcadePos8;
					z = ArcadeRot8;
				}
				if (ArcNo == 9)
				{
					vector = ArcadePos9;
					z = ArcadeRot9;
				}
				if (ArcNo == 10)
				{
					vector = ArcadePos10;
					z = ArcadeRot10;
				}
				if (ArcNo == 11)
				{
					vector = ArcadePos11;
					z = ArcadeRot11;
				}
				if (ArcNo == 12)
				{
					vector = ArcadePos12;
					z = ArcadeRot12;
				}
				if (ArcNo == 13)
				{
					vector = ArcadePos13;
					z = ArcadeRot13;
				}
				if (ArcNo == 14)
				{
					vector = ArcadePos14;
					z = ArcadeRot14;
				}
				if (ArcNo == 15)
				{
					vector = ArcadePos15;
					z = ArcadeRot15;
				}
				if (ArcNo == 16)
				{
					vector = ArcadePos16;
					z = ArcadeRot16;
				}
				if (ArcNo == 17)
				{
					vector = ArcadePos17;
					z = ArcadeRot17;
				}
				if (ArcNo == 18)
				{
					vector = ArcadePos18;
					z = ArcadeRot18;
				}
				if (ArcNo == 19)
				{
					vector = ArcadePos19;
					z = ArcadeRot19;
				}
				if (ArcNo == 20)
				{
					vector = ArcadePos20;
					z = ArcadeRot20;
				}
				if (Ver == 0 && ArcadeProps.Count > 0)
				{
					foreach (Prop arcadeProp in ArcadeProps)
					{
						if (GetDistance(arcadeProp.Position, vector) < 0.15f)
						{
							arcadeProp.Delete();
						}
					}
				}
				if (Ver != 0 && Ver < 14)
				{
					FakeArcade = GlobalHelper.CreateProp(RequestModel(ArcadePropNames[Ver]), vector, new Vector3(0f, 0f, z), true, false);
					ArcadeProps.Add(FakeArcade);
					if (ArcadeProps.Count > 0)
					{
						foreach (Prop arcadeProp2 in ArcadeProps)
						{
							if (Ver != 4 && GetDistance(arcadeProp2.Position, FakeArcade.Position) < 0.15f && arcadeProp2 != FakeArcade)
							{
								arcadeProp2.Delete();
							}
							if ((Ver == 4 || Ver == 5) && GetDistance(arcadeProp2.Position, FakeArcade.Position) < 2f && arcadeProp2 != FakeArcade)
							{
								arcadeProp2.Delete();
							}
						}
					}
				}
				if (Ver == 14)
				{
					FakeArcade = GlobalHelper.CreateProp(RequestModel("ch_prop_arcade_race_01a"), vector, new Vector3(0f, 0f, z), true, false);
					ArcadeProps.Add(FakeArcade);
					if (ArcadeProps.Count > 0)
					{
						foreach (Prop arcadeProp3 in ArcadeProps)
						{
							if (GetDistance(arcadeProp3.Position, FakeArcade.Position) < 2f && arcadeProp3 != FakeArcade)
							{
								arcadeProp3.Delete();
							}
						}
					}
					Vector3 offsetPosition = FakeArcade.GetOffsetPosition(new Vector3(-0.65f, 0f, 0.15f));
					Vector3 offsetPosition2 = FakeArcade.GetOffsetPosition(new Vector3(0.65f, 0f, 0.15f));
					Prop prop = GlobalHelper.CreateProp(RequestModel("ch_prop_arcade_race_car_01b"), offsetPosition, new Vector3(0f, 0f, FakeArcade.Rotation.Z), true, false);
					SeconardyArcadeProps.Add(prop);
					prop.IsPositionFrozen = true;
					Prop prop2 = GlobalHelper.CreateProp(RequestModel("ch_prop_arcade_race_car_01a"), offsetPosition2, new Vector3(0f, 0f, FakeArcade.Rotation.Z), true, false);
					SeconardyArcadeProps.Add(prop2);
					prop2.IsPositionFrozen = true;
					RacingArcades.Add(new RaceArcade(FakeArcade, prop, prop2, "Car"));
				}
				if (Ver == 15)
				{
					FakeArcade = GlobalHelper.CreateProp(RequestModel("ch_prop_arcade_race_01b"), vector, new Vector3(0f, 0f, z), true, false);
					ArcadeProps.Add(FakeArcade);
					if (ArcadeProps.Count > 0)
					{
						foreach (Prop arcadeProp4 in ArcadeProps)
						{
							if (GetDistance(arcadeProp4.Position, FakeArcade.Position) < 2f && arcadeProp4 != FakeArcade)
							{
								arcadeProp4.Delete();
							}
						}
					}
					Vector3 offsetPosition3 = FakeArcade.GetOffsetPosition(new Vector3(-0.65f, 0f, 0.15f));
					Vector3 offsetPosition4 = FakeArcade.GetOffsetPosition(new Vector3(0.65f, 0f, 0.15f));
					Prop prop3 = GlobalHelper.CreateProp(RequestModel("ch_prop_arcade_race_truck_01a"), offsetPosition3, new Vector3(0f, 0f, FakeArcade.Rotation.Z), true, false);
					SeconardyArcadeProps.Add(prop3);
					prop3.IsPositionFrozen = true;
					Prop prop4 = GlobalHelper.CreateProp(RequestModel("ch_prop_arcade_race_truck_01b"), offsetPosition4, new Vector3(0f, 0f, FakeArcade.Rotation.Z), true, false);
					SeconardyArcadeProps.Add(prop4);
					prop4.IsPositionFrozen = true;
					RacingArcades.Add(new RaceArcade(FakeArcade, prop3, prop4, "Car"));
				}
				if (Ver != 16)
				{
					return;
				}
				FakeArcade = GlobalHelper.CreateProp(RequestModel("ch_prop_arcade_race_02a"), vector, new Vector3(0f, 0f, z), true, false);
				ArcadeProps.Add(FakeArcade);
				if (ArcadeProps.Count > 0)
				{
					foreach (Prop arcadeProp5 in ArcadeProps)
					{
						if (GetDistance(arcadeProp5.Position, FakeArcade.Position) < 2f && arcadeProp5 != FakeArcade)
						{
							arcadeProp5.Delete();
						}
					}
				}
				Vector3 offsetPosition5 = FakeArcade.GetOffsetPosition(new Vector3(-0.65f, 0f, 0.15f));
				Vector3 offsetPosition6 = FakeArcade.GetOffsetPosition(new Vector3(0.65f, 0f, 0.15f));
				Prop prop5 = GlobalHelper.CreateProp(RequestModel("ch_prop_arcade_race_bike_02a"), offsetPosition5, new Vector3(0f, 0f, FakeArcade.Rotation.Z), true, false);
				SeconardyArcadeProps.Add(prop5);
				prop5.IsPositionFrozen = true;
				Prop prop6 = GlobalHelper.CreateProp(RequestModel("ch_prop_arcade_race_bike_02a"), offsetPosition6, new Vector3(0f, 0f, FakeArcade.Rotation.Z), true, false);
				SeconardyArcadeProps.Add(prop6);
				prop6.IsPositionFrozen = true;
				RacingArcades.Add(new RaceArcade(FakeArcade, prop5, prop6, "Bike"));
			}
			catch
			{
			}
		}

		public void AddArcades()
		{
			ArcadePos.Clear();
			ArcadePos.Add(new Vector3(2729.6f, -382.4f, -50f));
			ArcadePos.Add(new Vector3(2728.7f, -382.4f, -50f));
			ArcadePos.Add(new Vector3(2729.6f, -385.6f, -50f));
			ArcadePos.Add(new Vector3(2731.2f, -383.8f, -50f));
			foreach (Prop arcadeProp in ArcadeProps)
			{
				if (arcadeProp != null)
				{
					Vector3 offsetPosition = arcadeProp.GetOffsetPosition(new Vector3(0f, -1f, 0f));
					ArcadePos.Add(offsetPosition);
				}
			}
		}

		public string GetHostName()
		{
			return "Lester";
		}

		public static void CreateCars(string Garage)
		{
			Garage = "GarageA";
			SC.LoadIniFile(path + Garage + "//Slot1.ini");
			Model model = SC.CheckCar(path + Garage + "//Slot1.ini");
			SC.LoadIniFile(path + Garage + "//Slot2.ini");
			Model model2 = SC.CheckCar(path + Garage + "//Slot2.ini");
			SC.LoadIniFile(path + Garage + "//Slot3.ini");
			Model model3 = SC.CheckCar(path + Garage + "//Slot3.ini");
			SC.LoadIniFile(path + Garage + "//Slot4.ini");
			Model model4 = SC.CheckCar(path + Garage + "//Slot4.ini");
			SC.LoadIniFile(path + Garage + "//Slot5.ini");
			Model model5 = SC.CheckCar(path + Garage + "//Slot5.ini");
			SC.LoadIniFile(path + Garage + "//Slot6.ini");
			Model model6 = SC.CheckCar(path + Garage + "//Slot6.ini");
			SC.LoadIniFile(path + Garage + "//Slot7.ini");
			Model model7 = SC.CheckCar(path + Garage + "//Slot7.ini");
			SC.LoadIniFile(path + Garage + "//Slot8.ini");
			Model model8 = SC.CheckCar(path + Garage + "//Slot8.ini");
			SC.LoadIniFile(path + Garage + "//Slot9.ini");
			Model model9 = SC.CheckCar(path + Garage + "//Slot9.ini");
			SC.LoadIniFile(path + Garage + "//Slot10.ini");
			Model model10 = SC.CheckCar(path + Garage + "//Slot10.ini");
			SC.LoadIniFile(path + Garage + "//Slot11.ini");
			Model model11 = SC.CheckCar(path + Garage + "//Slot11.ini");
			Script.Wait(100);
			if (model != "null" && model != null && model != null && model != null && model != null)
			{
				try
				{
					SC.LoadIniFile(path + Garage + "//Slot1.ini");
					Model model12 = SC.CheckCar(path + Garage + "//Slot1.ini");
					ArcadeVeh1 = GlobalHelper.CreateVehicle(model12, AVehPos_1, AVehRot_1);
					SC.Load(ArcadeVeh1);
					ArcadeVeh1.IsDriveable = false;
				}
				catch
				{
				}
			}
			Script.Wait(100);
			if (model2 != "null" && model2 != null && model2 != null && model2 != null && model2 != null)
			{
				try
				{
					SC.LoadIniFile(path + Garage + "//Slot2.ini");
					Model model13 = SC.CheckCar(path + Garage + "//Slot2.ini");
					ArcadeVeh2 = GlobalHelper.CreateVehicle(model13, AVehPos_2, AVehRot_2);
					SC.Load(ArcadeVeh2);
					ArcadeVeh2.IsDriveable = false;
				}
				catch
				{
				}
			}
			Script.Wait(100);
			if (model3 != "null" && model3 != null && model3 != null && model3 != null && model3 != null)
			{
				try
				{
					SC.LoadIniFile(path + Garage + "//Slot3.ini");
					Model model14 = SC.CheckCar(path + Garage + "//Slot3.ini");
					ArcadeVeh3 = GlobalHelper.CreateVehicle(model14, AVehPos_3, AVehRot_3);
					SC.Load(ArcadeVeh3);
					ArcadeVeh3.IsDriveable = false;
				}
				catch
				{
				}
			}
			if (UnlockedGarage != 1)
			{
				return;
			}
			Script.Wait(100);
			if (model4 != "null" && model4 != null && model4 != null && model4 != null && model4 != null)
			{
				try
				{
					SC.LoadIniFile(path + Garage + "//Slot4.ini");
					Model model15 = SC.CheckCar(path + Garage + "//Slot4.ini");
					ArcadeVeh4 = GlobalHelper.CreateVehicle(model15, AVehPos_4, AVehRot_4);
					SC.Load(ArcadeVeh4);
					ArcadeVeh4.IsDriveable = false;
				}
				catch
				{
				}
			}
			Script.Wait(100);
			if (model5 != "null" && model5 != null && model5 != null && model5 != null && model5 != null)
			{
				try
				{
					SC.LoadIniFile(path + Garage + "//Slot5.ini");
					Model model16 = SC.CheckCar(path + Garage + "//Slot5.ini");
					ArcadeVeh5 = GlobalHelper.CreateVehicle(model16, AVehPos_5, AVehRot_5);
					SC.Load(ArcadeVeh5);
					ArcadeVeh5.IsDriveable = false;
				}
				catch
				{
				}
			}
			Script.Wait(100);
			if (model6 != "null" && model6 != null && model6 != null && model6 != null && model6 != null)
			{
				try
				{
					SC.LoadIniFile(path + Garage + "//Slot6.ini");
					Model model17 = SC.CheckCar(path + Garage + "//Slot6.ini");
					ArcadeVeh6 = GlobalHelper.CreateVehicle(model17, AVehPos_6, AVehRot_6);
					SC.Load(ArcadeVeh6);
					ArcadeVeh6.IsDriveable = false;
				}
				catch
				{
				}
			}
			Script.Wait(100);
			if (model7 != "null" && model7 != null && model7 != null && model7 != null && model7 != null)
			{
				try
				{
					SC.LoadIniFile(path + Garage + "//Slot7.ini");
					Model model18 = SC.CheckCar(path + Garage + "//Slot7.ini");
					ArcadeVeh7 = GlobalHelper.CreateVehicle(model18, AVehPos_7, AVehRot_7);
					SC.Load(ArcadeVeh7);
					ArcadeVeh7.IsDriveable = false;
				}
				catch
				{
				}
			}
			Script.Wait(100);
			if (model8 != "null" && model8 != null && model8 != null && model8 != null && model8 != null)
			{
				try
				{
					SC.LoadIniFile(path + Garage + "//Slot8.ini");
					Model model19 = SC.CheckCar(path + Garage + "//Slot8.ini");
					ArcadeVeh8 = GlobalHelper.CreateVehicle(model19, AVehPos_8, AVehRot_8);
					SC.Load(ArcadeVeh8);
					ArcadeVeh8.IsDriveable = false;
				}
				catch
				{
				}
			}
			Script.Wait(100);
			if (model9 != "null" && model9 != null && model9 != null && model9 != null && model9 != null)
			{
				try
				{
					SC.LoadIniFile(path + Garage + "//Slot9.ini");
					Model model20 = SC.CheckCar(path + Garage + "//Slot9.ini");
					ArcadeVeh9 = GlobalHelper.CreateVehicle(model20, AVehPos_9, AVehRot_9);
					SC.Load(ArcadeVeh9);
					ArcadeVeh9.IsDriveable = false;
				}
				catch
				{
				}
			}
			Script.Wait(100);
			if (model10 != "null" && model10 != null && model10 != null && model10 != null && model10 != null)
			{
				SC.LoadIniFile(path + Garage + "//Slot10.ini");
				Model model21 = SC.CheckCar(path + Garage + "//Slot10.ini");
				ArcadeVeh10 = GlobalHelper.CreateVehicle(model21, AVehPos_10, AVehRot_10);
				SC.Load(ArcadeVeh10);
				ArcadeVeh10.IsDriveable = false;
			}
			Script.Wait(100);
			if (model11 != "null" && model11 != null && model11 != null && model11 != null && model11 != null)
			{
				SC.LoadIniFile(path + Garage + "//Slot11.ini");
				Model model22 = SC.CheckCar(path + Garage + "//Slot11.ini");
				ArcadeVeh11 = GlobalHelper.CreateVehicle(model22, AVehPos_11, AVehRot_11);
				SC.Load(ArcadeVeh11);
				ArcadeVeh11.IsDriveable = false;
			}
		}

		public void SetupArcadeGarageEnterMenu()
		{
			List<object> list = new List<object>();
			list.Add("Slot1");
			list.Add("Slot2");
			list.Add("Slot3");
			list.Add("Slot4");
			list.Add("Slot5");
			list.Add("Slot6");
			list.Add("Slot7");
			list.Add("Slot8");
			list.Add("Slot9");
			list.Add("Slot10");
			list.Add("Slot11");
			NativeMenu nativeMenu = new NativeMenu("", "Enter Garage");
			modLemonUIObjectPool.Add(nativeMenu);
			GarageMenu.AddSubMenu(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			NativeItem nativeItem = new NativeItem("Save Current Car");
			nativeMenu.Add(nativeItem);
			nativeItem.Activated += delegate
			{
				string text = "scripts//HKH_SinglePlayerHeists//Arcade//";
				string slot = GetSlot(text + "Garage//", false, 10);
				int slotID = GetSlotID(text + "Garage//", false, 10);
				if (!slot.Equals("No Slot Free"))
				{
					bool flag = true;
					if (slotID >= 4 && UnlockedGarage == 0)
					{
						flag = false;
						Notification.Show(GetHostName() + " : You need to purchase Garage Slots 4 - 11, from Customize Arcade");
					}
					if (flag)
					{
						SaveLocalcar("Garage", Vehicle1, "Slot" + slotID);
					}
				}
				else
				{
					Notification.Show("No free slots in Arcade.");
				}
			};
		}

		public static void DestoryCars()
		{
			if (ArcadeVeh1 != null)
			{
				ArcadeVeh1.Delete();
			}
			if (ArcadeVeh2 != null)
			{
				ArcadeVeh2.Delete();
			}
			if (ArcadeVeh3 != null)
			{
				ArcadeVeh3.Delete();
			}
			if (ArcadeVeh4 != null)
			{
				ArcadeVeh4.Delete();
			}
			if (ArcadeVeh5 != null)
			{
				ArcadeVeh5.Delete();
			}
			if (ArcadeVeh6 != null)
			{
				ArcadeVeh6.Delete();
			}
			if (ArcadeVeh7 != null)
			{
				ArcadeVeh7.Delete();
			}
			if (ArcadeVeh8 != null)
			{
				ArcadeVeh8.Delete();
			}
			if (ArcadeVeh9 != null)
			{
				ArcadeVeh9.Delete();
			}
			if (ArcadeVeh10 != null)
			{
				ArcadeVeh10.Delete();
			}
			if (ArcadeVeh11 != null)
			{
				ArcadeVeh11.Delete();
			}
		}

		public static void RemoveFromMMI_And_PersistantRides(Vehicle V)
		{
			if (File.Exists("scripts//MorsMutualInsurance_HKH.dll"))
			{
				Assembly assembly = Assembly.LoadFrom("scripts/MorsMutualInsurance_HKH.dll");
				Type type = assembly.GetType("MorsMutualInsurance_HKH.MorsMutualInsurance");
				MethodInfo method = type.GetMethod("RemoveFromMMIAndPersistantRides");
				object[] parameters = new object[1] { V };
				method.Invoke(method, parameters);
			}
		}

		public static void SaveVehicleMMI(Vehicle V)
		{
			if (File.Exists("scripts//MorsMutualInsurance_HKH.dll"))
			{
				Assembly assembly = Assembly.LoadFrom("scripts/MorsMutualInsurance_HKH.dll");
				Type type = assembly.GetType("MorsMutualInsurance_HKH.MorsMutualInsurance");
				MethodInfo method = type.GetMethod("SaveVehicleToMMIAndRemoveFromGarage");
				int num = 20;
				object[] parameters = new object[3] { V, num, "" };
				method.Invoke(method, parameters);
			}
		}

		public void LoadCarinToRealWorld(Vehicle V)
		{
			SaveVehicleMMI(V);
			Vehicle currentVehicle = Game.Player.Character.CurrentVehicle;
			Vehicle vehicle = currentVehicle;
			IsInArcade = false;
			vehicle.Position = ArcadeGarageEnter;
			vehicle.Heading = ArcadeGarageEnterHeading;
			Game.Player.Character.SetIntoVehicle(vehicle, VehicleSeat.Driver);
			DestoryCars();
			vehicle.Heading = 147f;
			vehicle.IsDriveable = true;
			SaveVehicle = vehicle;
			SaveVehicle.IsDriveable = true;
			SaveVehicle.Repair();
			SaveVehicle.IsPositionFrozen = false;
			Function.Call(Hash.PUSH_TIMECYCLE_MODIFIER);
			Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
			Function.Call(Hash.POP_TIMECYCLE_MODIFIER);
			Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "default");
			if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
			{
				Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
			}
			Function.Call(Hash.SET_RADAR_ZOOM, 850);
			foreach (Blip item in MainBlips_U)
			{
				if (item != null)
				{
					item.Delete();
				}
			}
			foreach (Blip item2 in MainBlips_G)
			{
				if (item2 != null)
				{
					item2.Delete();
				}
			}
			CreatedTunerGear = false;
			Tuner_Gear_Objects.DeleteEntites();
			if (ReturnGameBuild() >= 3095)
			{
				ArcadeObjects_v3095.DeleteEntites();
			}
			else
			{
				ArcadeObjects_v2944.DeleteEntites();
			}
		}

		public static string GetSlot(string Path, bool IsEclipse, int MaximumSlots)
		{
			string text = "No Slot Free";
			for (int i = 0; i < MaximumSlots; i++)
			{
				int num = i + 1;
				SC.LoadIniFile(Path + "\\Slot" + num + ".ini");
				Model model = SC.CheckCar(Path + "\\Slot" + num + ".ini");
				if ((model == "null" || model == null) && text.Equals("No Slot Free"))
				{
					text = "Slot" + num + ".ini";
				}
			}
			return text;
		}

		public static int GetSlotID(string Path, bool IsEclipse, int MaximumSlots)
		{
			string text = "No Slot Free";
			int result = -1;
			for (int i = 0; i < MaximumSlots; i++)
			{
				int num = i + 1;
				SC.LoadIniFile(Path + "\\Slot" + num + ".ini");
				Model model = SC.CheckCar(Path + "\\Slot" + num + ".ini");
				if ((model == "null" || model == null) && text.Equals("No Slot Free"))
				{
					result = num;
					text = "Slot" + num + ".ini";
				}
			}
			return result;
		}

		public void SaveLocalcar(string G, Vehicle V, string Slot)
		{
			RemoveFromMMI_And_PersistantRides(V);
			if (Game.Player.Character.CurrentVehicle != null)
			{
				if (Game.Player.Character.CurrentVehicle.DisplayName != "RHINO" || Game.Player.Character.CurrentVehicle.DisplayName != "KHANJALI" || Game.Player.Character.CurrentVehicle.DisplayName != "CHERNOBOG")
				{
					if (SaveVehicle != null)
					{
						SaveVehicle.Delete();
					}
					GarageNum = G;
					Notification.Show(path + G + "//" + Slot + ".ini");
					SC.LoadIniFile(path + G + "//" + Slot + ".ini");
					SC.SaveWithoutDelete();
					Script.Wait(1000);
					if (Game.Player.Character.CurrentVehicle != null)
					{
						Game.Player.Character.CurrentVehicle.Delete();
					}
					if (Game.Player.Character.LastVehicle != null)
					{
						Game.Player.Character.LastVehicle.Delete();
					}
					Function.Call(Hash.PUSH_TIMECYCLE_MODIFIER);
					Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
					Function.Call(Hash.POP_TIMECYCLE_MODIFIER);
					Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "default");
					if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
					{
						Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
					}
				}
				else
				{
					DisplayHelpTextThisFrame("You cannot save this vehicle");
				}
			}
			else
			{
				DisplayHelpTextThisFrame("Bring a vehicle to save");
			}
		}

		public void DeleteCarinSlot(string Slot, Vehicle V)
		{
			SC.LoadIniFile(path + "GarageA//" + Slot + ".ini");
			if (SC.VehicleName != "null")
			{
				if (V.DisplayName == "ZTYPE")
				{
					Random random = new Random();
					int value = random.Next(10000000, 11000000);
					ManagePlayerFunds.AddFunds(value);
				}
				if (V.ClassType == VehicleClass.Coupes)
				{
					Random random2 = new Random();
					int value2 = random2.Next(85000, 125000);
					ManagePlayerFunds.AddFunds(value2);
				}
				else if (V.ClassType == VehicleClass.Military)
				{
					Random random3 = new Random();
					int value3 = random3.Next(500000, 750000);
					ManagePlayerFunds.AddFunds(value3);
				}
				else if (V.ClassType == VehicleClass.Super)
				{
					Random random4 = new Random();
					int value4 = random4.Next(345000, 400000);
					ManagePlayerFunds.AddFunds(value4);
				}
				else if (V.ClassType == VehicleClass.SUVs)
				{
					Random random5 = new Random();
					int value5 = random5.Next(75000, 150000);
					ManagePlayerFunds.AddFunds(value5);
				}
				else if (V.ClassType == VehicleClass.OffRoad)
				{
					Random random6 = new Random();
					int value6 = random6.Next(100000, 250000);
					ManagePlayerFunds.AddFunds(value6);
				}
				else if (V.ClassType == VehicleClass.Sports)
				{
					Random random7 = new Random();
					int value7 = random7.Next(150000, 400000);
					ManagePlayerFunds.AddFunds(value7);
				}
				else if (V.ClassType == VehicleClass.SportsClassics)
				{
					Random random8 = new Random();
					int value8 = random8.Next(240000, 350000);
					ManagePlayerFunds.AddFunds(value8);
				}
				else if (V.ClassType == VehicleClass.Sedans)
				{
					Random random9 = new Random();
					int value9 = random9.Next(50000, 200000);
					ManagePlayerFunds.AddFunds(value9);
				}
				else if (V.ClassType == VehicleClass.Muscle)
				{
					Random random10 = new Random();
					int value10 = random10.Next(225000, 300000);
					ManagePlayerFunds.AddFunds(value10);
				}
				else if (V.ClassType == VehicleClass.Motorcycles)
				{
					Random random11 = new Random();
					int value11 = random11.Next(60000, 200000);
					ManagePlayerFunds.AddFunds(value11);
				}
				else
				{
					Random random12 = new Random();
					int value12 = random12.Next(50000, 120000);
					ManagePlayerFunds.AddFunds(value12);
				}
				Notification.Show("Office Assistant : Vehicle Sold");
			}
			if (SC.VehicleName == "null")
			{
				Notification.Show("Office Assistant : There is no Vehicle in " + Slot);
			}
			SC.SaveName();
			VehicleToDeliver = null;
			V.Delete();
		}

		public void DeleteCarinSlot2(string Slot, Vehicle V)
		{
			SC.LoadIniFile(path + "GarageA//" + Slot + ".ini");
			if (SC.VehicleName != "null")
			{
				if (V.DisplayName == "ZTYPE")
				{
					Random random = new Random();
					int value = random.Next(10000000, 11000000);
					ManagePlayerFunds.AddFunds(value);
				}
				if (V.ClassType == VehicleClass.Coupes)
				{
					Random random2 = new Random();
					int value2 = random2.Next(140000, 400000);
					ManagePlayerFunds.AddFunds(value2);
				}
				else if (V.ClassType == VehicleClass.Military)
				{
					Random random3 = new Random();
					int value3 = random3.Next(750000, 1500000);
					ManagePlayerFunds.AddFunds(value3);
				}
				else if (V.ClassType == VehicleClass.Super)
				{
					Random random4 = new Random();
					int value4 = random4.Next(640000, 900000);
					ManagePlayerFunds.AddFunds(value4);
				}
				else if (V.ClassType == VehicleClass.SUVs)
				{
					Random random5 = new Random();
					int value5 = random5.Next(125000, 400000);
					ManagePlayerFunds.AddFunds(value5);
				}
				else if (V.ClassType == VehicleClass.OffRoad)
				{
					Random random6 = new Random();
					int value6 = random6.Next(200000, 600000);
					ManagePlayerFunds.AddFunds(value6);
				}
				else if (V.ClassType == VehicleClass.Sports)
				{
					Random random7 = new Random();
					int value7 = random7.Next(340000, 500000);
					ManagePlayerFunds.AddFunds(value7);
				}
				else if (V.ClassType == VehicleClass.SportsClassics)
				{
					Random random8 = new Random();
					int value8 = random8.Next(450000, 1200000);
					ManagePlayerFunds.AddFunds(value8);
				}
				else if (V.ClassType == VehicleClass.Sedans)
				{
					Random random9 = new Random();
					int value9 = random9.Next(120000, 400000);
					ManagePlayerFunds.AddFunds(value9);
				}
				else if (V.ClassType == VehicleClass.Muscle)
				{
					Random random10 = new Random();
					int value10 = random10.Next(450000, 800000);
					ManagePlayerFunds.AddFunds(value10);
				}
				else if (V.ClassType == VehicleClass.Motorcycles)
				{
					Random random11 = new Random();
					int value11 = random11.Next(100000, 400000);
					ManagePlayerFunds.AddFunds(value11);
				}
				else
				{
					Random random12 = new Random();
					int value12 = random12.Next(100000, 320000);
					ManagePlayerFunds.AddFunds(value12);
				}
				Notification.Show("Office Assistant : Vehicle Sold");
			}
			if (SC.VehicleName == "null")
			{
				Notification.Show("Office Assistant : There is no Vehicle in " + Slot);
			}
			if (Game.Player.Character.Model == PedHash.Franklin)
			{
				string text = "Clinton";
				Notification.Show("Buyer: Thank you Mr " + text + " For your donation");
			}
			if (Game.Player.Character.Model == PedHash.Michael)
			{
				string text2 = "De Santa";
				Notification.Show("Buyer: Thank you Mr " + text2 + " For your donation");
			}
			if (Game.Player.Character.Model == PedHash.Trevor)
			{
				string text3 = "Philips";
				Notification.Show("Buyer: Thank you Mr " + text3 + " For your donation");
			}
			Notification.Show("Buyer: Calculating Bonus");
			int num = 0;
			if (V.Mods[VehicleModType.Livery].Index >= 1)
			{
				num += 10000;
			}
			if (V.Mods[VehicleModType.Engine].Index > 0)
			{
				if (V.Mods[VehicleModType.Engine].Index == 1)
				{
					num += 5000;
				}
				if (V.Mods[VehicleModType.Engine].Index == 2)
				{
					num += 10000;
				}
				if (V.Mods[VehicleModType.Engine].Index == 3)
				{
					num += 15000;
				}
			}
			if (V.Mods[VehicleModType.Transmission].Index > 0)
			{
				if (V.Mods[VehicleModType.Transmission].Index == 1)
				{
					num += 5000;
				}
				if (V.Mods[VehicleModType.Transmission].Index == 2)
				{
					num += 10000;
				}
				if (V.Mods[VehicleModType.Transmission].Index == 3)
				{
					num += 15000;
				}
			}
			if (V.Mods[VehicleModType.Armor].Index > 0)
			{
				if (V.Mods[VehicleModType.Armor].Index == 1)
				{
					num += 5000;
				}
				if (V.Mods[VehicleModType.Armor].Index == 2)
				{
					num += 10000;
				}
				if (V.Mods[VehicleModType.Armor].Index == 3)
				{
					num += 15000;
				}
			}
			if (V.Mods[VehicleModType.Brakes].Index > 0)
			{
				if (V.Mods[VehicleModType.Brakes].Index == 1)
				{
					num += 5000;
				}
				if (V.Mods[VehicleModType.Brakes].Index == 2)
				{
					num += 10000;
				}
				if (V.Mods[VehicleModType.Brakes].Index == 3)
				{
					num += 15500;
				}
			}
			if (V.Mods[VehicleModType.Roof].Index == 1 || V.Mods[VehicleModType.Roof].Index == -1)
			{
				num += 35000;
			}
			Notification.Show("Buyer : Bonus for mods: $" + num);
			ManagePlayerFunds.AddFunds(num);
			num = 0;
			SC.SaveName();
			DeliveringVehicle = false;
			VehicleToDeliver = null;
			V.Delete();
		}

		public void BuyVehicleFromCatagory(VehicleClass C, NativeMenu ParentMenu, string SpawnName, string FullName, string Price, NativeListItem<dynamic> SelectedSlot, List<dynamic> listofSlots)
		{
			float FPrice = 0f;
			try
			{
				FPrice = int.Parse(Price);
			}
			catch (Exception)
			{
			}
			NativeItem nativeItem = new NativeItem(FullName + " : ~g~" + FPrice.ToString("N"));
			ParentMenu.Add(nativeItem);
			nativeItem.Activated += delegate
			{
				try
				{
					FPrice = int.Parse(Price);
				}
				catch (Exception)
				{
				}
				if (ManagePlayerFunds.AreFundsHigher(FPrice))
				{
					try
					{
						ManagePlayerFunds.RemoveFunds((int)FPrice);
						string text = "GarageA";
						DestoryCars();
						string text2 = listofSlots[SelectedSlot.SelectedIndex];
						SC.LoadIniFile(VehicleLoadPath + text + "//" + text2 + ".ini");
						Notification.Show(VehicleLoadPath + text + "//" + text2 + ".ini");
						Vector3 position = Game.Player.Character.Position;
						SC.Save();
						ResetBoughtVehicle();
						SC.LoadIniFile(VehicleLoadPath + text + "//" + text2 + ".ini");
						try
						{
							Vehicle vehicle = GlobalHelper.CreateVehicle(RequestModel(SpawnName), default(Vector3), 0f);
							string displayName = vehicle.DisplayName;
							SC.SaveName(displayName);
							CreateCars("GarageA");
							vehicle.Delete();
							return;
						}
						catch (NullReferenceException)
						{
							Notification.Show("~r~error ~w~ Invalid Vehicle Name!");
							return;
						}
					}
					catch (Exception)
					{
						return;
					}
				}
				Notification.Show(GetHostName() + " you do not have enough money to purchase this vehicle!");
			};
		}

		public void SetupArcadeGarageBuySellMenu()
		{
			List<dynamic> Slots = new List<object>();
			Slots.Add("Slot1");
			Slots.Add("Slot2");
			Slots.Add("Slot3");
			Slots.Add("Slot4");
			Slots.Add("Slot5");
			Slots.Add("Slot6");
			Slots.Add("Slot7");
			Slots.Add("Slot8");
			Slots.Add("Slot9");
			Slots.Add("Slot10");
			Slots.Add("Slot11");
			NativeMenu nativeMenu = new NativeMenu("", "Sell Vehicle Option A (Worse)");
			modLemonUIObjectPool.Add(nativeMenu);
			RemoveMenu2.AddSubMenu(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			NativeListItem<dynamic> s = new NativeListItem<object>("Slot : ", Slots.ToArray());
			nativeMenu.Add(s);
			NativeItem CarinSlot = new NativeItem("Car : ");
			nativeMenu.Add(CarinSlot);
			NativeItem nativeItem = new NativeItem("Sell and Remove");
			nativeMenu.Add(nativeItem);
			s.ItemChanged += delegate
			{
				Vehicle vehicle2 = null;
				string text4 = Slots[s.SelectedIndex];
				if (text4.Equals("Slot1"))
				{
					vehicle2 = ArcadeVeh1;
				}
				if (text4.Equals("Slot2"))
				{
					vehicle2 = ArcadeVeh2;
				}
				if (text4.Equals("Slot3"))
				{
					vehicle2 = ArcadeVeh3;
				}
				if (text4.Equals("Slot4"))
				{
					vehicle2 = ArcadeVeh4;
				}
				if (text4.Equals("Slot5"))
				{
					vehicle2 = ArcadeVeh5;
				}
				if (text4.Equals("Slot6"))
				{
					vehicle2 = ArcadeVeh6;
				}
				if (text4.Equals("Slot7"))
				{
					vehicle2 = ArcadeVeh7;
				}
				if (text4.Equals("Slot8"))
				{
					vehicle2 = ArcadeVeh8;
				}
				if (text4.Equals("Slot9"))
				{
					vehicle2 = ArcadeVeh9;
				}
				if (text4.Equals("Slot10"))
				{
					vehicle2 = ArcadeVeh10;
				}
				if (text4.Equals("Slot11"))
				{
					vehicle2 = ArcadeVeh11;
				}
				if (vehicle2 != null)
				{
					CarToDelete = vehicle2;
				}
				SC.LoadIniFile(VehicleLoadPath + Slots[s.SelectedIndex] + ".ini");
				if (!SC.VehicleName.Equals("null"))
				{
					CarinSlot.Title = SC.VehicleName;
				}
				else if (SC.VehicleName.Equals("null"))
				{
					CarinSlot.Title = "No car in slot";
				}
			};
			nativeItem.Activated += delegate
			{
				Vehicle vehicle = null;
				string text3 = Slots[s.SelectedIndex];
				if (text3.Equals("Slot1"))
				{
					vehicle = ArcadeVeh1;
				}
				if (text3.Equals("Slot2"))
				{
					vehicle = ArcadeVeh2;
				}
				if (text3.Equals("Slot3"))
				{
					vehicle = ArcadeVeh3;
				}
				if (text3.Equals("Slot4"))
				{
					vehicle = ArcadeVeh4;
				}
				if (text3.Equals("Slot5"))
				{
					vehicle = ArcadeVeh5;
				}
				if (text3.Equals("Slot6"))
				{
					vehicle = ArcadeVeh6;
				}
				if (text3.Equals("Slot7"))
				{
					vehicle = ArcadeVeh7;
				}
				if (text3.Equals("Slot8"))
				{
					vehicle = ArcadeVeh8;
				}
				if (text3.Equals("Slot9"))
				{
					vehicle = ArcadeVeh9;
				}
				if (text3.Equals("Slot10"))
				{
					vehicle = ArcadeVeh10;
				}
				if (text3.Equals("Slot11"))
				{
					vehicle = ArcadeVeh11;
				}
				if (vehicle != null)
				{
					if (text3.Equals("Slot1") || text3.Equals("Slot2") || text3.Equals("Slot3"))
					{
						DeleteCarinSlot(Slots[s.SelectedIndex], vehicle);
						SC.LoadIniFile(VehicleLoadPath + text3 + ".ini");
						SC.SaveName();
					}
					if (!text3.Equals("Slot1") && !text3.Equals("Slot2") && !text3.Equals("Slot3"))
					{
						if (UnlockedGarage == 1)
						{
							DeleteCarinSlot(Slots[s.SelectedIndex], vehicle);
							SC.LoadIniFile(VehicleLoadPath + text3 + ".ini");
							SC.SaveName();
						}
						else
						{
							Notification.Show(GetHostName() + " : You need to purchase Garage Slots 4 - 11, from Customize Arcade");
						}
					}
				}
			};
			NativeMenu nativeMenu2 = new NativeMenu("", "Add vehicle in Slot");
			modLemonUIObjectPool.Add(nativeMenu2);
			RemoveMenu2.AddSubMenu(nativeMenu2);
			GLemonUIMenusNativeMenus.Add(nativeMenu2);
			GLemonUIMenusNativeMenus.Add(nativeMenu2);
			List<object> list = new List<object>();
			string[] array = File.ReadAllLines(ListPath);
			string[] array2 = array;
			foreach (string item2 in array2)
			{
				list.Add(item2);
			}
			NativeMenu nativeMenu3 = new NativeMenu("", "Vehicle Catagories");
			modLemonUIObjectPool.Add(nativeMenu3);
			nativeMenu2.AddSubMenu(nativeMenu3);
			GLemonUIMenusNativeMenus.Add(nativeMenu3);
			LandVehicles[] array3 = (LandVehicles[])Enum.GetValues(typeof(LandVehicles));
			LandVehicles[] array4 = array3;
			for (int j = 0; j < array4.Length; j++)
			{
				LandVehicles landVehicles = array4[j];
				NativeMenu nativeMenu4 = new NativeMenu("", "~g~" + landVehicles);
				modLemonUIObjectPool.Add(nativeMenu4);
				nativeMenu3.AddSubMenu(nativeMenu4);
				GLemonUIMenusNativeMenus.Add(nativeMenu4);
				NativeListItem<object> nativeListItem = new NativeListItem<object>("Selected Slot : ", Slots.ToArray());
				nativeMenu4.Add(nativeListItem);
				foreach (object item3 in list)
				{
					string text = (string)(dynamic)item3;
					string[] separator = new string[2] { " = ", "," };
					string text2 = text;
					string[] array5 = text2.Split(separator, StringSplitOptions.RemoveEmptyEntries);
					if (array5.Length > 2)
					{
						for (int k = 0; k < array5.Length; k++)
						{
							Price = array5[1];
							Model = array5[2];
							man = array5[0];
						}
						VehicleClass vehicleClass = (VehicleClass)Function.Call<int>(Hash.GET_VEHICLE_CLASS_FROM_NAME, Function.Call<int>(Hash.GET_HASH_KEY, man));
						if (vehicleClass == (VehicleClass)landVehicles)
						{
							BuyVehicleFromCatagory((VehicleClass)landVehicles, nativeMenu4, man, Model, Price, nativeListItem, Slots);
						}
					}
				}
			}
		}

		public void SetRandomOutfit(Ped Dancer)
		{
			Random random = new Random();
			if (Dancer.Gender == Gender.Female)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 0, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 0)), 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 2, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 2)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 2)), 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 3)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 3)), 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 8, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 8)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 8)), 0);
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 4)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 4, 4, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 4)), 1);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) != 4)
				{
					int num = random.Next(0, 5);
					if (num == 4)
					{
						int num2 = random.Next(0, 4);
						if (num2 == 0)
						{
							num = 0;
						}
						if (num2 == 1)
						{
							num = 1;
						}
						if (num2 == 2)
						{
							num = 2;
						}
						if (num2 == 3)
						{
							num = 3;
						}
						if (num2 == 4)
						{
							num = 5;
						}
					}
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 4, num, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 4)), 1);
				}
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 0, Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3), 0, 0);
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 4) == 4)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 5, 4, 0, 1);
				}
			}
			if (Dancer.Gender == Gender.Male)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 0, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 0)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 0)), 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 2, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 2)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 2)), 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 3)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 3)), 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 8, random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Dancer, 8)), random.Next(0, Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, Dancer, 8)), 0);
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 0)
				{
					int num3 = random.Next(0, 4);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num3, 0);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 1)
				{
					int num4 = random.Next(0, 3);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num4, 0);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 2)
				{
					int num5 = random.Next(0, 3);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num5, 0);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 3)
				{
					int num6 = random.Next(0, 3);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num6, 0);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 4)
				{
					int num7 = random.Next(0, 2);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num7, 0);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 5)
				{
					int num8 = random.Next(0, 2);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num8, 0);
				}
				if (Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Dancer, 3) == 6)
				{
					int num9 = random.Next(0, 2);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Dancer, 3, 5, num9, 0);
				}
			}
		}

		public void RefreshArcadeInterior()
		{
			UndergrondAreaLockedbool2 = false;
			UndergrondAreaLockedbool = false;
			HiddenDoorOpen_WaitForPlayerFar = false;
			HiddenDoorOpen_WaitForPlayerClose = false;
			HiddenDoorOpen = false;
			try
			{
				LoadIniFileArcade("scripts//HKH_SinglePlayerHeists//Arcade//Arcade.ini");
				ArcadeOwned = ArcadeConfig.GetValue("Arcade", "ArcadeOwned", ArcadeOwned);
				ArcadeVersion = ArcadeConfig.GetValue("Arcade", "ArcadeVersion", ArcadeVersion);
				UsingHiddenDoor = ArcadeConfig.GetValue("Arcade", "UsingHiddenTellerDoor", UsingHiddenDoor);
				ArcadeFloodID = ArcadeConfig.GetValue("Arcade", "ArcadeFloodID", ArcadeFloodID);
				ArcadeStyle = ArcadeConfig.GetValue("Arcade", "ArcadeStyle", ArcadeStyle);
				MuralID = ArcadeConfig.GetValue("Arcade", "MuralID", MuralID);
				PushieID = ArcadeConfig.GetValue("Arcade", "PushieID", PushieID);
				NeonArtID = ArcadeConfig.GetValue("Arcade", "NeonArtID", NeonArtID);
				UnlockedDilllEquipment = ArcadeConfig.GetValue("Arcade", "UnlockedDilllEquipment", UnlockedDilllEquipment);
				UnlockedHackingEquipment = ArcadeConfig.GetValue("Arcade", "UnlockedHackingEquipment", UnlockedHackingEquipment);
				UnlockedBreachingEquipment = ArcadeConfig.GetValue("Arcade", "UnlockedBreachingEquipment", UnlockedBreachingEquipment);
				UnlockedOutfitsEquipment = ArcadeConfig.GetValue("Arcade", "UnlockedOutfitsEquipment", UnlockedOutfitsEquipment);
				UnlockedPlansEquipment = ArcadeConfig.GetValue("Arcade", "UnlockedPlansEquipment", UnlockedPlansEquipment);
				UnlockedGarage = ArcadeConfig.GetValue("Arcade", "UnlockedGarage", UnlockedGarage);
				UnlockedGunlocker = ArcadeConfig.GetValue("Arcade", "UnlockedGunlocker", UnlockedGunlocker);
				ArcadeMoneyVault = ArcadeConfig.GetValue("Arcade", "ArcadeMoneyVault", ArcadeMoneyVault);
				UndergrondAreaLocked = ArcadeConfig.GetValue("Arcade", "UndergrondAreaLocked", UndergrondAreaLocked);
				if (UsingHiddenDoor == 1)
				{
					if (UndergroundDoor != null)
					{
						UndergroundDoor.Delete();
					}
					UndergroundDoor = GlobalHelper.CreateProp(RequestModel("ch_prop_arcade_fortune_door_01a"), new Vector3(2727.911f, -371.9659f, -48.39819f), new Vector3(0f, 0f, 0f), false, false);
					UndergroundDoor.IsPositionFrozen = true;
				}
				if (UndergrondAreaLocked == 1 && UsingHiddenDoor != 1)
				{
					if (UndergroundDoor != null)
					{
						UndergroundDoor.Delete();
					}
					if (!UndergrondAreaLockedbool)
					{
						UndergrondAreaLockedbool = true;
						UndergroundDoor = GlobalHelper.CreateProp(RequestModel("ch_prop_mesa_door_01a"), new Vector3(2726.6f, -371.8277f, -48.4f), new Vector3(0f, 0f, -125f), false, false);
						UndergroundDoor.IsPositionFrozen = true;
					}
				}
				if (UndergrondAreaLocked == 0 && UsingHiddenDoor == 0 && !UndergrondAreaLockedbool)
				{
					if (UndergroundDoor != null)
					{
						UndergroundDoor.Delete();
					}
					UndergroundDoor = GlobalHelper.CreateProp(RequestModel("ch_prop_mesa_door_01a"), new Vector3(2726.6f, -371.8277f, -48.4f), new Vector3(0f, 0f, 0f), false, false);
					UndergroundDoor.IsPositionFrozen = true;
				}
				Arcade1Ver = ArcadeConfig.GetValue("Arcade", "Arcade1Ver", Arcade1Ver);
				Arcade2Ver = ArcadeConfig.GetValue("Arcade", "Arcade2Ver", Arcade2Ver);
				Arcade3Ver = ArcadeConfig.GetValue("Arcade", "Arcade3Ver", Arcade3Ver);
				Arcade4Ver = ArcadeConfig.GetValue("Arcade", "Arcade4Ver", Arcade4Ver);
				Arcade5Ver = ArcadeConfig.GetValue("Arcade", "Arcade5Ver", Arcade5Ver);
				Arcade6Ver = ArcadeConfig.GetValue("Arcade", "Arcade6Ver", Arcade6Ver);
				Arcade7Ver = ArcadeConfig.GetValue("Arcade", "Arcade7Ver", Arcade7Ver);
				Arcade8Ver = ArcadeConfig.GetValue("Arcade", "Arcade8Ver", Arcade8Ver);
				Arcade9Ver = ArcadeConfig.GetValue("Arcade", "Arcade9Ver", Arcade9Ver);
				Arcade10Ver = ArcadeConfig.GetValue("Arcade", "Arcade10Ver", Arcade10Ver);
				Arcade11Ver = ArcadeConfig.GetValue("Arcade", "Arcade11Ver", Arcade11Ver);
				Arcade12Ver = ArcadeConfig.GetValue("Arcade", "Arcade12Ver", Arcade12Ver);
				Arcade13Ver = ArcadeConfig.GetValue("Arcade", "Arcade13Ver", Arcade13Ver);
				Arcade14Ver = ArcadeConfig.GetValue("Arcade", "Arcade14Ver", Arcade14Ver);
				Arcade15Ver = ArcadeConfig.GetValue("Arcade", "Arcade15Ver", Arcade15Ver);
				Arcade16Ver = ArcadeConfig.GetValue("Arcade", "Arcade16Ver", Arcade16Ver);
				Arcade17Ver = ArcadeConfig.GetValue("Arcade", "Arcade17Ver", Arcade17Ver);
				Arcade18Ver = ArcadeConfig.GetValue("Arcade", "Arcade18Ver", Arcade18Ver);
				Arcade19Ver = ArcadeConfig.GetValue("Arcade", "Arcade19Ver", Arcade19Ver);
				Arcade20Ver = ArcadeConfig.GetValue("Arcade", "Arcade20Ver", Arcade20Ver);
				PlayRadioMusic = ArcadeConfig.GetValue("Arcade", "PlayRadioMusic", PlayRadioMusic);
				DefaultRadioStation = ArcadeConfig.GetValue("Arcade", "DefaultRadioStation", DefaultRadioStation);
				CurrentRadioStation = ArcadeConfig.GetValue("Arcade", "CurrentRadioStation", CurrentRadioStation);
				RadioOn = ArcadeConfig.GetValue("Arcade", "RadioOn", RadioOn);
				int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 2730.0, -380.0, -50.0);
				int num2 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 2712.1, -361.2773, -51.867);
				Function.Call(Hash.REFRESH_INTERIOR, num);
				Function.Call(Hash.REFRESH_INTERIOR, num2);
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_01");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_02");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_03");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_04");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_05");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_06");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_07");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_08");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_09");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Cockroaches");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Drone_Parts");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Electric_Drill");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Fireman_Helmetv");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Gruppe_Sechs_Outfits");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Hacking_Device");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Plastic_Explosives");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Stealth_Outfits");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Swipe_Card_01a");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Swipe_Card_01b");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Vault_Drill_Alt");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Vault_KeyCard_01a");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Vault_Laser");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Vault_Laser_Alt");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_ceiling_beams");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_ceiling_flat");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_ceiling_mirror");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_derelict");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_derelict_carpet");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_derelict_clean_up");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_streetx4");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_trophy_brawler");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_trophy_cabs");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_trophy_gunner");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_trophy_king");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_trophy_love");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_trophy_monkey");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_trophy_patriot");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_trophy_racer");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_trophy_retro");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_trophy_strife");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_trophy_teller");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_big_screen");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_constant_geometry");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_floor_option_01");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_floor_option_02");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_floor_option_03");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_floor_option_04");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_floor_option_05");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_floor_option_06");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_floor_option_07");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_floor_option_08");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_hip_light_no_neon");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_neon_option_01");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_neon_option_02");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_neon_option_03");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_neon_option_04");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_neon_option_05");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_neon_option_06");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_neon_option_07");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_neon_option_08");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_option_01");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_option_02");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_option_03");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_option_04");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_option_05");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_option_06");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_option_07");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_option_08");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_ret_light_no_neon");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_screens");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "set_plan_arcade_x4");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_bed");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_casino");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_computer");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_garage");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_hacker");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_keypad");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_mechanic");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_no_bed");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_plans");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_pre_setup");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_scribbles");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_setup");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_vault");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_wall");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_weapons");
				if (PushieID == 0)
				{
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_01");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_02");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_03");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_04");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_05");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_06");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_07");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_08");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_09");
				}
				if (PushieID == 1)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_01");
				}
				if (PushieID == 2)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_01");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_02");
				}
				if (PushieID == 3)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_01");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_02");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_03");
				}
				if (PushieID == 4)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_01");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_02");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_03");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_04");
				}
				if (PushieID == 5)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_01");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_02");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_03");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_04");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_05");
				}
				if (PushieID == 6)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_01");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_02");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_03");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_04");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_05");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_06");
				}
				if (PushieID == 7)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_01");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_02");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_03");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_04");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_05");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_06");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_07");
				}
				if (PushieID == 8)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_01");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_02");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_03");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_04");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_05");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_06");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_07");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_08");
				}
				if (PushieID == 9)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_01");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_02");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_03");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_04");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_05");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_06");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_07");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_08");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Entity_Set_Plushie_09");
				}
				if (ArcadeStyle == 0)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_derelict_clean_up");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_derelict");
				}
				if (ArcadeStyle == 1)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_ceiling_beams");
				}
				if (ArcadeStyle == 2)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_ceiling_flat");
				}
				if (ArcadeStyle == 3)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_ceiling_mirror");
				}
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_streetx4");
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_trophy_brawler");
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_trophy_cabs");
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_trophy_claw");
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_trophy_gunner");
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_trophy_king");
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_trophy_love");
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_trophy_monkey");
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_trophy_patriot");
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_trophy_racer");
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_trophy_retro");
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_trophy_strife");
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_trophy_teller");
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_big_screen");
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_constant_geometry");
				if (ArcadeFloodID == 0)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_arcade_set_derelict_carpet");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_floor_option_01");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_floor_option_02");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_floor_option_03");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_floor_option_04");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_floor_option_05");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_floor_option_06");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_floor_option_07");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_floor_option_08");
				}
				if (ArcadeFloodID == 1)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_floor_option_01");
				}
				if (ArcadeFloodID == 2)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_floor_option_02");
				}
				if (ArcadeFloodID == 3)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_floor_option_03");
				}
				if (ArcadeFloodID == 4)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_floor_option_04");
				}
				if (ArcadeFloodID == 5)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_floor_option_05");
				}
				if (ArcadeFloodID == 6)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_floor_option_06");
				}
				if (ArcadeFloodID == 7)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_floor_option_07");
				}
				if (ArcadeFloodID == 8)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_floor_option_08");
				}
				if (NeonArtID == 0)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_hip_light_no_neon");
				}
				if (NeonArtID == 0)
				{
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_neon_option_01");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_neon_option_02");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_neon_option_03");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_neon_option_04");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_neon_option_05");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_neon_option_06");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_neon_option_07");
				}
				if (NeonArtID == 8)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_neon_option_01");
				}
				if (NeonArtID == 7)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_neon_option_02");
				}
				if (NeonArtID == 6)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_neon_option_03");
				}
				if (NeonArtID == 5)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_neon_option_04");
				}
				if (NeonArtID == 4)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_neon_option_05");
				}
				if (NeonArtID == 3)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_neon_option_06");
				}
				if (NeonArtID == 2)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_neon_option_07");
				}
				if (NeonArtID == 1)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_neon_option_08");
				}
				if (MuralID == 0)
				{
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_option_01");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_option_02");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_option_03");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_option_04");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_option_05");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_option_06");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_option_07");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_option_08");
				}
				if (MuralID == 8)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_option_01");
				}
				if (MuralID == 7)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_option_02");
				}
				if (MuralID == 6)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_option_03");
				}
				if (MuralID == 5)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_option_04");
				}
				if (MuralID == 4)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_option_05");
				}
				if (MuralID == 3)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_option_06");
				}
				if (MuralID == 2)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_option_07");
				}
				if (MuralID == 1)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_mural_option_08");
				}
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "entity_set_ret_light_no_neon");
				Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Cockroaches");
				if (UnlockedBreachingEquipment == 1)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Hacking_Device");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Plastic_Explosives");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Swipe_Card_01a");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Swipe_Card_01bv");
				}
				if (UnlockedHackingEquipment == 1)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_arcade_x4");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Drone_Parts");
				}
				if (UnlockedHackingEquipment == 0)
				{
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_arcade_x4");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Drone_Parts");
				}
				if (UnlockedOutfitsEquipment == 1)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_arcade_x4");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Stealth_Outfits");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Fireman_Helmet");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Gruppe_Sechs_Outfits");
				}
				if (UnlockedOutfitsEquipment == 0)
				{
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Stealth_Outfits");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Fireman_Helmet");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Gruppe_Sechs_Outfits");
				}
				if (UnlockedDilllEquipment == 1)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Electric_Drill");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Vault_Drill");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Vault_Drill_Alt");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Vault_KeyCard_01a");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Vault_Laser");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_vault");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Vault_Laser_Alt");
				}
				if (UnlockedDilllEquipment == 0)
				{
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Electric_Drill");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Vault_Drill");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Vault_Drill_Alt");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Vault_KeyCard_01a");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Vault_Laser");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_vault");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "Set_Plan_Vault_Laser_Alt");
				}
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "entity_set_screens");
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_bed");
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_computer");
				if (UnlockedGarage == 1)
				{
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_wall");
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_garage");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_keypad");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_mechanic");
				}
				if (UnlockedGarage == 0)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_wall");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_garage");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_keypad");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_mechanic");
				}
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_no_bed");
				if (UnlockedPlansEquipment == 1)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_casino");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_hacker");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_plans");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_scribbles");
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_setup");
				}
				if (ArcadeClutter)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_pre_setup");
				}
				if (!ArcadeClutter)
				{
					Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_pre_setup");
				}
				if (UnlockedGarage == 0 && UnlockedGunlocker == 1)
				{
					Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num2, "set_plan_weapons");
				}
				Function.Call(Hash.REFRESH_INTERIOR, num2);
				Function.Call(Hash.REFRESH_INTERIOR, num);
				foreach (Ped arcadeInteriorPed in ArcadeInteriorPeds)
				{
					if (arcadeInteriorPed != null)
					{
						arcadeInteriorPed.Delete();
					}
				}
				foreach (Ped item in ArcadeInteriorPeds2)
				{
					if (item != null)
					{
						item.Delete();
					}
				}
				foreach (Prop arcadeProp in ArcadeProps)
				{
					if (arcadeProp != null)
					{
						arcadeProp.Delete();
					}
				}
				foreach (Prop seconardyArcadeProp in SeconardyArcadeProps)
				{
					if (seconardyArcadeProp != null)
					{
						seconardyArcadeProp.Delete();
					}
				}
				if (ArcadeProps.Count > 0)
				{
					foreach (Prop arcadeProp2 in ArcadeProps)
					{
						if (arcadeProp2 != null)
						{
							arcadeProp2.Delete();
						}
					}
				}
				foreach (Prop seconardyArcadeProp2 in SeconardyArcadeProps)
				{
					if (seconardyArcadeProp2 != null)
					{
						seconardyArcadeProp2.Delete();
					}
				}
				RacingArcades.Clear();
				SetupINDArcade(Arcade1Ver, 1);
				SetupINDArcade(Arcade2Ver, 2);
				SetupINDArcade(Arcade3Ver, 3);
				SetupINDArcade(Arcade4Ver, 4);
				SetupINDArcade(Arcade5Ver, 5);
				SetupINDArcade(Arcade6Ver, 6);
				SetupINDArcade(Arcade7Ver, 7);
				SetupINDArcade(Arcade8Ver, 8);
				SetupINDArcade(Arcade9Ver, 9);
				SetupINDArcade(Arcade10Ver, 10);
				SetupINDArcade(Arcade11Ver, 11);
				SetupINDArcade(Arcade12Ver, 12);
				SetupINDArcade(Arcade13Ver, 13);
				SetupINDArcade(Arcade14Ver, 14);
				SetupINDArcade(Arcade15Ver, 15);
				SetupINDArcade(Arcade16Ver, 16);
				SetupINDArcade(Arcade17Ver, 17);
				SetupINDArcade(Arcade18Ver, 18);
				SetupINDArcade(Arcade19Ver, 19);
				SetupINDArcade(Arcade20Ver, 20);
				AddArcades();
				if (Bartender != null)
				{
					Bartender.Delete();
				}
				Bartender = GlobalHelper.CreatePed(RequestModel(PedHash.Bartender01SFY), new Vector3(2723.7f, -385.2f, -50f), -90f);
				Bartender.BlockPermanentEvents = true;
				Bartender.IsPositionFrozen = true;
				Bartender.RelationshipGroup = 2;
				Bartender.Task.PlayAnimation("anim@amb@casino@mini@drinking@bar@drink_v2@heels@idle_a", "idle_a_bartender", 8f, 1f, -1, AnimationFlags.Loop, -1f);
				Random random = new Random();
				ClubDancerList.Add("a_m_y_clubcust_01");
				ClubDancerList.Add("a_m_y_clubcust_01");
				ClubDancerList.Add("a_m_y_clubcust_01");
				ClubDancerList.Add("a_f_y_clubcust_01");
				ClubDancerList.Add("a_f_y_clubcust_01");
				ClubDancerList.Add("a_f_y_clubcust_01");
				if (ArcadeProps.Count > 0)
				{
					for (int i = 0; i < ArcadeProps.Count; i++)
					{
						Prop prop = ArcadeProps[i];
						if (prop != null)
						{
							int num3 = random.Next(0, 100);
							if (num3 > 10 && prop.Model != RequestModel("ch_prop_arcade_race_01a") && prop.Model != RequestModel("ch_prop_arcade_race_01b") && prop.Model != RequestModel("ch_prop_arcade_race_02a"))
							{
								Ped ped = GlobalHelper.CreatePed(RequestModel(ClubDancerList[random.Next(0, ClubDancerList.Count)]), prop.GetOffsetPosition(new Vector3(0f, -0.8f, 0f)), prop.Heading);
								ped.BlockPermanentEvents = true;
								ped.RelationshipGroup = 2;
								ped.Task.PlayAnimation("anim_casino_a@amb@casino@games@arcadecabinet@maleleft", "idle", 8f, 1f, -1, AnimationFlags.Loop, -1f);
								ArcadeInteriorPeds.Add(ped);
							}
							Script.Wait(1);
						}
					}
				}
				if (RacingArcades.Count > 0)
				{
					for (int j = 0; j < RacingArcades.Count; j++)
					{
						int num4 = random.Next(0, 100);
						if (num4 > 10)
						{
							Prop @base = RacingArcades[j].Base;
							if (@base.Model == RequestModel("ch_prop_arcade_race_01a") || @base.Model == RequestModel("ch_prop_arcade_race_01b"))
							{
								Ped ped2 = GlobalHelper.CreatePed(RequestModel(ClubDancerList[random.Next(0, ClubDancerList.Count)]), RacingArcades[j].Seat1.GetOffsetPosition(new Vector3(0f, 0f, -0.6f)), RacingArcades[j].Seat1.Heading);
								ped2.BlockPermanentEvents = true;
								ped2.IsCollisionEnabled = false;
								ped2.IsPositionFrozen = true;
								ped2.RelationshipGroup = 2;
								ped2.Task.PlayAnimation("amb@code_human_in_car_idles@generic@ds@base", "base", 8f, 1f, -1, AnimationFlags.Loop, -1f);
								Ped ped3 = GlobalHelper.CreatePed(RequestModel(ClubDancerList[random.Next(0, ClubDancerList.Count)]), RacingArcades[j].Seat2.GetOffsetPosition(new Vector3(0f, 0f, -0.6f)), RacingArcades[j].Seat2.Heading);
								ped3.BlockPermanentEvents = true;
								ped3.IsCollisionEnabled = false;
								ped3.IsPositionFrozen = true;
								ped3.RelationshipGroup = 2;
								ped3.Task.PlayAnimation("amb@code_human_in_car_idles@generic@ds@base", "base", 8f, 1f, -1, AnimationFlags.Loop, -1f);
								ArcadeInteriorPeds2.Add(ped3);
								ArcadeInteriorPeds2.Add(ped2);
								ArcadeCarPed.Add(ped3);
								ArcadeCarPed.Add(ped2);
							}
							if (@base.Model == RequestModel("ch_prop_arcade_race_02a"))
							{
								Ped ped4 = GlobalHelper.CreatePed(RequestModel(ClubDancerList[random.Next(0, ClubDancerList.Count)]), RacingArcades[j].Seat1.GetOffsetPosition(new Vector3(0f, -0.45f, -0.4f)), RacingArcades[j].Seat1.Heading);
								ped4.BlockPermanentEvents = true;
								ped4.IsCollisionEnabled = false;
								ped4.IsPositionFrozen = true;
								ped4.RelationshipGroup = 2;
								ped4.Task.PlayAnimation("amb@code_human_on_bike_idles@sports@front@base", "base", 8f, 1f, -1, AnimationFlags.Loop, -1f);
								Ped ped5 = GlobalHelper.CreatePed(RequestModel(ClubDancerList[random.Next(0, ClubDancerList.Count)]), RacingArcades[j].Seat2.GetOffsetPosition(new Vector3(0f, -0.45f, -0.4f)), RacingArcades[j].Seat2.Heading);
								ped5.BlockPermanentEvents = true;
								ped5.IsCollisionEnabled = false;
								ped5.IsPositionFrozen = true;
								ped5.RelationshipGroup = 2;
								ped5.Task.PlayAnimation("amb@code_human_on_bike_idles@sports@front@base", "base", 8f, 1f, -1, AnimationFlags.Loop, -1f);
								ArcadeInteriorPeds2.Add(ped5);
								ArcadeInteriorPeds2.Add(ped4);
								ArcadeBikePed.Add(ped5);
								ArcadeBikePed.Add(ped4);
							}
						}
						Script.Wait(1);
					}
				}
				Ped ped6 = GlobalHelper.CreatePed(RequestModel(ClubDancerList[random.Next(0, ClubDancerList.Count)]), new Vector3(2727.15f, -387.4f, -50.5f), 173f);
				ped6.BlockPermanentEvents = true;
				ped6.RelationshipGroup = 2;
				ped6.Task.StartScenario("WORLD_HUMAN_PARTYING", ped6.Position.Z);
				ArcadeInteriorPeds2.Add(ped6);
				ped6 = GlobalHelper.CreatePed(RequestModel(ClubDancerList[random.Next(0, ClubDancerList.Count)]), new Vector3(2725.8f, -386.3f, -50.5f), 89f);
				ped6.BlockPermanentEvents = true;
				ped6.RelationshipGroup = 2;
				ped6.Task.StartScenario("WORLD_HUMAN_PARTYING", ped6.Position.Z);
				ArcadeInteriorPeds2.Add(ped6);
				ped6 = GlobalHelper.CreatePed(RequestModel(ClubDancerList[random.Next(0, ClubDancerList.Count)]), new Vector3(2725.299f, -383.89f, -49.9f), 89f);
				ped6.BlockPermanentEvents = true;
				ped6.RelationshipGroup = 2;
				ped6.IsPositionFrozen = true;
				ped6.Task.PlayAnimation("anim_casino_b@amb@casino@games@roulette@ped_female@seat_1@engaged@01b@idles", "idle_a", 8f, 1f, -1, AnimationFlags.Loop, -1f);
				ArcadeInteriorPeds2.Add(ped6);
				ped6 = GlobalHelper.CreatePed(RequestModel(ClubDancerList[random.Next(0, ClubDancerList.Count)]), new Vector3(2738.109f, -380.8021f, -49.9f), -102f);
				ped6.BlockPermanentEvents = true;
				ped6.RelationshipGroup = 2;
				ped6.IsPositionFrozen = true;
				ped6.Task.PlayAnimation("anim@amb@office@seating@male@var_e@base@", "base", 8f, 1f, -1, AnimationFlags.Loop, -1f);
				ArcadeInteriorPeds2.Add(ped6);
			}
			catch
			{
				Notification.Show("TEST CASE 0 Failed - Contact HKH191");
			}
			try
			{
				LoadIniFileArcade("scripts//HKH_SinglePlayerHeists//Arcade//Arcade.ini");
				foreach (Prop crate_Valuable in Crate_Valuables)
				{
					if (crate_Valuable != null)
					{
						crate_Valuable.Delete();
					}
				}
				if (Crate_Valuables.Count > 0)
				{
					Crate_Valuables.Clear();
				}
				foreach (Prop item2 in PaintingHolder)
				{
					if (item2 != null)
					{
						item2.Delete();
					}
				}
				foreach (Prop painting in Paintings)
				{
					if (painting != null)
					{
						painting.Delete();
					}
				}
				foreach (SecCabinetProps securityCabinetProp in SecurityCabinetProps)
				{
					foreach (Prop prop2 in securityCabinetProp.Props)
					{
						if (prop2 != null)
						{
							prop2.Delete();
						}
					}
				}
				foreach (SecCabinetProps securityCabinetProp2 in SecurityCabinetProps)
				{
					securityCabinetProp2.Props.Clear();
				}
				SecurityCabinetProps.Clear();
				PaintingHolder.Clear();
				Paintings.Clear();
				Crate_Type.Clear();
				if (ExtraValuable != null)
				{
					ExtraValuable.Delete();
				}
				if (CurrentSDBType.Count > 0)
				{
					CurrentSDBType.Clear();
				}
				if (CurrentSDB.Count > 0)
				{
					CurrentSDB.Clear();
				}
				if (CurrentSDBDict.Count > 0)
				{
					CurrentSDBDict.Clear();
				}
				if (CurrentSDBDict.Count > 0)
				{
					PaintingsFinished.Clear();
				}
				CuttingStage = -1;
				foreach (Prop item3 in PaintingHolder)
				{
					if (item3 != null)
					{
						item3.Delete();
					}
				}
				foreach (Prop painting2 in Paintings)
				{
					if (painting2 != null)
					{
						painting2.Delete();
					}
				}
				if (PaintingHolder.Count > 0)
				{
					PaintingHolder.Clear();
				}
				if (Paintings.Count > 0)
				{
					Paintings.Clear();
				}
				if (ArcadePainting != null)
				{
					ArcadePainting.Delete();
				}
				if (ArcadePaintingHolder != null)
				{
					ArcadePaintingHolder.Delete();
				}
				if (Safe != null)
				{
					Safe.Delete();
				}
			}
			catch
			{
				Notification.Show("TEST CASE 1 - 12 Failed - Contact HKH191");
			}
			try
			{
				if (Computer != null)
				{
					Computer.Delete();
				}
				Computer = GlobalHelper.CreateProp(RequestModel("ba_prop_battle_club_computer_01"), new Vector3(2726.51f, -376.4142f, -47.6f), new Vector3(0f, 0f, 0f), false, false);
				Computer.IsPositionFrozen = true;
			}
			catch
			{
				Notification.Show("TEST CASE 14 - 17 Failed - Contact HKH191");
			}
		}

		public void ShowSafeButtons()
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int num = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(0, 10), new Size(Convert.ToInt32(absoluteX), 450), 0f, Color.FromArgb(200, 255, 255, 255));
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 172, 0), "Number Up");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 173, 0), "Number Down");
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 175, 0), "Input Right");
			scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 174, 0), "Input Left");
			scaleform.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 24, 0), "Check");
			scaleform.CallFunction("SET_DATA_SLOT", 5, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Quit");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public void BEGIN_SCALEFORM_MOVIE_METHOD(int scaleform, string methodName)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, scaleform, methodName);
		}

		public void END_SCALEFORM_MOVIE_METHOD()
		{
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private void func_4450(int iParam0)
		{
			BEGIN_SCALEFORM_MOVIE_METHOD(Local_26709f_9, "SET_STATE");
			Add_param_int(iParam0);
			END_SCALEFORM_MOVIE_METHOD();
		}

		private void func_4436(int iParam0)
		{
			if (CodeNumber1 == 0 && CodeNumber2 == 0 && CodeNumber3 == 0)
			{
				func_4453();
			}
			if (Local_26709f_8 == -1)
			{
				func_4437(iParam0);
				return;
			}
			func_4450(1);
			if (ReadyForCode)
			{
				func_4443();
			}
			if (ReadyForCode)
			{
				func_4448(0, (int)InputedNumber1);
				func_4448(1, (int)InputedNumber2);
				func_4448(2, (int)InputedNumber3);
			}
			Function.Call(Hash.SET_TEXT_RENDER_ID, Local_26709f_8);
			Function.Call(Hash.SET_RANDOM_SEED, 4);
			Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, true);
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE, Local_26709f_9, 0.205f, 0.17f, 0.4f, 0.4f, 255, 255, 255, 255, 0);
			Function.Call(Hash.SET_TEXT_RENDER_ID, Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
		}

		private void func_4443()
		{
			BEGIN_SCALEFORM_MOVIE_METHOD(Local_26709f_9, "SET_CURSOR_POSITION");
			Add_param_int(SelectedNum);
			END_SCALEFORM_MOVIE_METHOD();
		}

		private void func_4437(int iParam0)
		{
			if (!Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "safe_01a"))
			{
				Function.Call(Hash.REGISTER_NAMED_RENDERTARGET, "safe_01a", false);
				return;
			}
			Function.Call(Hash.LINK_NAMED_RENDERTARGET, iParam0);
			if (!Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_LINKED, iParam0))
			{
				Function.Call(Hash.LINK_NAMED_RENDERTARGET, iParam0);
			}
			else
			{
				Local_26709f_8 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "safe_01a");
			}
		}

		private int func_4438()
		{
			Local_26709f_9 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "DIGITAL_SAFE_DISPLAY");
			if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Local_26709f_9))
			{
				return 1;
			}
			return 0;
		}

		private int func_4439(bool bParam0, int iParam1)
		{
			if (bParam0)
			{
				if (Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "safe_01a") || Local_26709f_8 > -1)
				{
					if (!Function.Call<bool>(Hash.RELEASE_NAMED_RENDERTARGET, "safe_01a"))
					{
						return 0;
					}
					Local_26709f_8 = -1;
				}
				Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, Local_26709f_9);
				Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_HEI4/DLCHEI4_GENERIC_01");
			}
			else
			{
				Function.Call(Hash.CLEAR_HELP, true);
				if (Function.Call<bool>(Hash.DOES_ENTITY_EXIST, iParam1))
				{
					Function.Call(Hash.FREEZE_ENTITY_POSITION, iParam1, true);
				}
			}
			return 1;
		}

		private void func_4453()
		{
			Random random = new Random();
			CodeNumber1 = random.Next(1, 99);
			CodeNumber2 = random.Next(1, 99);
			CodeNumber3 = random.Next(1, 99);
		}

		private void func_4454(int iParam0)
		{
			iLocal_26708 = iParam0;
		}

		private void func_4448(int iParam0, int iParam1)
		{
			BEGIN_SCALEFORM_MOVIE_METHOD(Local_26709f_9, "SET_VALUE");
			Add_param_int(iParam0);
			Add_param_int(iParam1);
			END_SCALEFORM_MOVIE_METHOD();
		}

		public void Add_param_int(int V)
		{
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, V);
		}

		public void AddRatioStations()
		{
			RadioStat.Add("Los Santos Rock Radio");
			RadioStat.Add("Non-Stop-Pop FM");
			RadioStat.Add("Radio Los Santos");
			RadioStat.Add("Channel X");
			RadioStat.Add("West Coast Talk Radio");
			RadioStat.Add("Rebel Radio");
			RadioStat.Add("Soulwax FM");
			RadioStat.Add("East Los FM");
			RadioStat.Add("West Coast Classics");
			RadioStat.Add("Blue Ark");
			RadioStat.Add("World Wide FM");
			RadioStat.Add("FlyLo FM");
			RadioStat.Add("The Lowdown 91.1");
			RadioStat.Add("The Lab");
			RadioStat.Add("Radio Mirror Park");
			RadioStat.Add("Space 103.2");
			RadioStat.Add("Vinewood Boulevard Radio");
			RadioStat.Add("Blonded Los Santos 97.8 FM");
			RadioStat.Add("Los Santos Underground Radio");
			RadioStat.Add("IFruit Radio");
			RadioStat.Add("Self Radio");
		}

		public void ArcadeInteriorOptions()
		{
			float StyleCost = 0f;
			float MuralCost = 0f;
			float NeonArtCost = 0f;
			float FloorCost = 0f;
			float PushieCost = 0f;
			float num = 0f;
			List<object> list = new List<object>();
			list.Add("Los Santos Rock Radio");
			list.Add("Non-Stop-Pop FM");
			list.Add("Radio Los Santos");
			list.Add("Channel X");
			list.Add("West Coast Talk Radio");
			list.Add("Rebel Radio");
			list.Add("Soulwax FM");
			list.Add("East Los FM");
			list.Add("West Coast Classics");
			list.Add("Blue Ark");
			list.Add("World Wide FM");
			list.Add("FlyLo FM");
			list.Add("The Lowdown 91.1");
			list.Add("The Lab");
			list.Add("Radio Mirror Park");
			list.Add("Space 103.2");
			list.Add("Vinewood Boulevard Radio");
			list.Add("Blonded Los Santos 97.8 FM");
			list.Add("Los Santos Underground Radio");
			list.Add("Self Radio");
			List<dynamic> TF = new List<object>();
			TF.Add(false);
			TF.Add(true);
			float ArcadesPrice = 0f;
			List<dynamic> ArcadeCosts = new List<object>();
			ArcadeCosts.Add(0);
			ArcadeCosts.Add(0);
			ArcadeCosts.Add(0);
			ArcadeCosts.Add(0);
			ArcadeCosts.Add(0);
			ArcadeCosts.Add(0);
			ArcadeCosts.Add(0);
			ArcadeCosts.Add(0);
			ArcadeCosts.Add(0);
			ArcadeCosts.Add(0);
			ArcadeCosts.Add(0);
			ArcadeCosts.Add(0);
			ArcadeCosts.Add(0);
			ArcadeCosts.Add(0);
			ArcadeCosts.Add(0);
			ArcadeCosts.Add(0);
			ArcadeCosts.Add(0);
			ArcadeCosts.Add(0);
			ArcadeCosts.Add(0);
			ArcadeCosts.Add(0);
			List<dynamic> ArcadeMachineCosts = new List<object>();
			ArcadeMachineCosts.Add(0);
			ArcadeMachineCosts.Add(95000);
			ArcadeMachineCosts.Add(125000);
			ArcadeMachineCosts.Add(225000);
			ArcadeMachineCosts.Add(217000);
			ArcadeMachineCosts.Add(200000);
			ArcadeMachineCosts.Add(213000);
			ArcadeMachineCosts.Add(67000);
			ArcadeMachineCosts.Add(137000);
			ArcadeMachineCosts.Add(140000);
			ArcadeMachineCosts.Add(92000);
			ArcadeMachineCosts.Add(176000);
			ArcadeMachineCosts.Add(234000);
			ArcadeMachineCosts.Add(244000);
			ArcadeMachineCosts.Add(422000);
			ArcadeMachineCosts.Add(440000);
			ArcadeMachineCosts.Add(454000);
			List<object> list2 = new List<object>();
			list2.Add("Derelict");
			list2.Add("Exposed Hipster");
			list2.Add("Total Focus");
			List<object> list3 = new List<object>();
			list3.Add("Naked");
			list3.Add("Another Realm");
			list3.Add("All The Colors");
			list3.Add("Wireframed");
			list3.Add("Spray Forever");
			list3.Add("Racing Stripes");
			list3.Add("Pixelated Thought");
			list3.Add("So Kawaii");
			list3.Add("O Edgy");
			List<object> list4 = new List<object>();
			list4.Add("Naked");
			list4.Add("Mighty Sword");
			list4.Add("Penatrator");
			list4.Add("LS Nights");
			list4.Add("Game Over");
			list4.Add("Emulator");
			list4.Add("BFFL");
			list4.Add("Crotch Rocket");
			list4.Add("Adult Content");
			List<object> list5 = new List<object>();
			list5.Add("Naked");
			list5.Add("Prim and Proper");
			list5.Add("Scale Up");
			list5.Add("Yay Rainbow");
			list5.Add("The Lab");
			list5.Add("Intergalactic");
			list5.Add("Understated Stars");
			list5.Add("Super Fun Playthings");
			list5.Add("Thunderclap");
			List<object> list6 = new List<object>();
			list6.Add("Naked");
			list6.Add("Plushie 1");
			list6.Add("Plushie 2");
			list6.Add("Plushie 3");
			list6.Add("Plushie 4");
			list6.Add("Plushie 5");
			list6.Add("Plushie 6");
			list6.Add("Plushie 7");
			list6.Add("Plushie 8");
			list6.Add("Plushie 9");
			List<dynamic> ArcadeMachines = new List<object>();
			ArcadeMachines.Add("None");
			ArcadeMachines.Add("Arcade : Claw");
			ArcadeMachines.Add("Arcade : Degenatron");
			ArcadeMachines.Add("Arcade : Fortune");
			ArcadeMachines.Add("Arcade : Gunslinger");
			ArcadeMachines.Add("Arcade : Love");
			ArcadeMachines.Add("Arcade : Space");
			ArcadeMachines.Add("Arcade : Street A");
			ArcadeMachines.Add("Arcade : Street B");
			ArcadeMachines.Add("Arcade : Street C");
			ArcadeMachines.Add("Arcade : Street D");
			ArcadeMachines.Add("Arcade : Street E");
			ArcadeMachines.Add("Arcade : Invade");
			ArcadeMachines.Add("Arcade : Wizard");
			ArcadeMachines.Add("Arcade : Race & Chase : Street Legal");
			ArcadeMachines.Add("Arcade : Race & Chase : Get Truckin");
			ArcadeMachines.Add("Arcade : Race & Chase : Crotch Rockets");
			float Cost = 0f;
			NativeMenu nativeMenu = new NativeMenu("", "Arcade Customization");
			modLemonUIObjectPool.Add(nativeMenu);
			ArcadeC.AddSubMenu(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			List<NativeListItem> list7 = new List<NativeListItem>();
			NativeListItem<dynamic> ST = new NativeListItem<object>("Style : ", list2.ToArray());
			nativeMenu.Add(ST);
			list7.Add(ST);
			NativeListItem<dynamic> F = new NativeListItem<object>("Floor : ", list5.ToArray());
			nativeMenu.Add(F);
			list7.Add(F);
			NativeListItem<dynamic> M = new NativeListItem<object>("Mural : ", list3.ToArray());
			nativeMenu.Add(M);
			list7.Add(M);
			NativeListItem<dynamic> P = new NativeListItem<object>("Pushie : ", list6.ToArray());
			nativeMenu.Add(P);
			list7.Add(P);
			NativeListItem<dynamic> NA = new NativeListItem<object>("Neon Art : ", list4.ToArray());
			nativeMenu.Add(NA);
			list7.Add(NA);
			NativeItem Buy = new NativeItem("Buy " + Cost.ToString("N"));
			nativeMenu.Add(Buy);
			NativeItem item2 = new NativeItem("-----------------------------------------------------------");
			nativeMenu.Add(item2);
			NativeItem nativeItem = new NativeItem("Underground Area : $350,000");
			nativeMenu.Add(nativeItem);
			nativeItem.Activated += delegate
			{
				if (Game.Player.Money >= 350000)
				{
					Game.Player.Money -= 350000;
					if (UndergrondAreaLocked == 1)
					{
						Notification.Show(GetHostName() + " : You have already purchased the Underground Area");
					}
					if (UndergrondAreaLocked == 0)
					{
						UndergrondAreaLocked = 1;
						ArcadeConfig.SetValue("Arcade", "UndergrondAreaLocked", UndergrondAreaLocked);
						ArcadeConfig.Save();
						RefreshArcadeInterior();
						Notification.Show(GetHostName() + " : Underground Area Bought");
					}
				}
			};
			NativeListItem<dynamic> HTD = new NativeListItem<object>("Hidden Teller Door : ", TF.ToArray());
			nativeMenu.Add(HTD);
			NativeItem item3 = new NativeItem("------------Underground Area Required-----------");
			nativeMenu.Add(item3);
			NativeItem nativeItem2 = new NativeItem("Arcade Money Vault: $1,000,000");
			nativeMenu.Add(nativeItem2);
			NativeItem nativeItem3 = new NativeItem("Garage Slots 4 - 11 : $600,000");
			nativeMenu.Add(nativeItem3);
			NativeItem nativeItem4 = new NativeItem("Gunlocker : $1,000,000");
			nativeMenu.Add(nativeItem4);
			NativeItem nativeItem5 = new NativeItem("Drilling Equipment : $450,000");
			nativeMenu.Add(nativeItem5);
			NativeItem nativeItem6 = new NativeItem("Hacking Equipment : $250,000");
			nativeMenu.Add(nativeItem6);
			NativeItem nativeItem7 = new NativeItem("Breaching Equipment : $250,000");
			nativeMenu.Add(nativeItem7);
			NativeItem nativeItem8 = new NativeItem("Heist Outfits : $150,000");
			nativeMenu.Add(nativeItem8);
			NativeItem nativeItem9 = new NativeItem("Heist Plans : $50,000");
			nativeMenu.Add(nativeItem9);
			NativeItem nativeItem10 = new NativeItem("Set Clutter On/Off");
			nativeMenu.Add(nativeItem10);
			NativeItem item4 = new NativeItem("-----------------------------------------------------------");
			nativeMenu.Add(item4);
			NativeItem item5 = new NativeItem("-----------------------------------------------------------");
			nativeMenu.Add(item5);
			ArcadeBuyMenu = new NativeMenu("", "Buy More Arcade Machines");
			modLemonUIObjectPool.Add(ArcadeBuyMenu);
			nativeMenu.AddSubMenu(ArcadeBuyMenu);
			GLemonUIMenusNativeMenus.Add(ArcadeBuyMenu);
			GLemonUIMenusNativeMenus.Add(ArcadeBuyMenu);
			List<NativeListItem> list8 = new List<NativeListItem>();
			NativeItem nativeItem11 = new NativeItem("Get Arcade Machines Active");
			ArcadeBuyMenu.Add(nativeItem11);
			NativeListItem<dynamic> Arc1 = new NativeListItem<object>("Arcade 1 Game : ", ArcadeMachines.ToArray());
			ArcadeBuyMenu.Add(Arc1);
			list8.Add(Arc1);
			NativeListItem<dynamic> Arc12 = new NativeListItem<object>("Arcade 2 Game : ", ArcadeMachines.ToArray());
			ArcadeBuyMenu.Add(Arc12);
			list8.Add(Arc12);
			NativeListItem<dynamic> Arc14 = new NativeListItem<object>("Arcade 3 Game : ", ArcadeMachines.ToArray());
			ArcadeBuyMenu.Add(Arc14);
			list8.Add(Arc14);
			NativeListItem<dynamic> Arc15 = new NativeListItem<object>("Arcade 4 Game : ", ArcadeMachines.ToArray());
			ArcadeBuyMenu.Add(Arc15);
			list8.Add(Arc15);
			NativeListItem<dynamic> Arc16 = new NativeListItem<object>("Arcade 5 Game : ", ArcadeMachines.ToArray());
			ArcadeBuyMenu.Add(Arc16);
			list8.Add(Arc16);
			NativeListItem<dynamic> Arc17 = new NativeListItem<object>("Arcade 6 Game : ", ArcadeMachines.ToArray());
			ArcadeBuyMenu.Add(Arc17);
			list8.Add(Arc17);
			NativeListItem<dynamic> Arc18 = new NativeListItem<object>("Arcade 7 Game : ", ArcadeMachines.ToArray());
			ArcadeBuyMenu.Add(Arc18);
			list8.Add(Arc18);
			NativeListItem<dynamic> Arc19 = new NativeListItem<object>("Arcade 8 Game : ", ArcadeMachines.ToArray());
			ArcadeBuyMenu.Add(Arc19);
			list8.Add(Arc19);
			NativeListItem<dynamic> Arc20 = new NativeListItem<object>("Arcade 9 Game : ", ArcadeMachines.ToArray());
			ArcadeBuyMenu.Add(Arc20);
			list8.Add(Arc20);
			NativeListItem<dynamic> Arc2 = new NativeListItem<object>("Arcade 10 Game : ", ArcadeMachines.ToArray());
			ArcadeBuyMenu.Add(Arc2);
			list8.Add(Arc2);
			NativeListItem<dynamic> Arc3 = new NativeListItem<object>("Arcade 11 Game : ", ArcadeMachines.ToArray());
			ArcadeBuyMenu.Add(Arc3);
			list8.Add(Arc3);
			NativeListItem<dynamic> Arc4 = new NativeListItem<object>("Arcade 12 Game : ", ArcadeMachines.ToArray());
			ArcadeBuyMenu.Add(Arc4);
			list8.Add(Arc4);
			NativeListItem<dynamic> Arc5 = new NativeListItem<object>("Arcade 13 Game : ", ArcadeMachines.ToArray());
			ArcadeBuyMenu.Add(Arc5);
			list8.Add(Arc5);
			NativeListItem<dynamic> Arc6 = new NativeListItem<object>("Arcade 14 Game : ", ArcadeMachines.ToArray());
			ArcadeBuyMenu.Add(Arc6);
			list8.Add(Arc6);
			NativeListItem<dynamic> Arc7 = new NativeListItem<object>("Arcade 15 Game : ", ArcadeMachines.ToArray());
			ArcadeBuyMenu.Add(Arc7);
			list8.Add(Arc7);
			NativeListItem<dynamic> Arc8 = new NativeListItem<object>("Arcade 16 Game : ", ArcadeMachines.ToArray());
			ArcadeBuyMenu.Add(Arc8);
			list8.Add(Arc8);
			NativeListItem<dynamic> Arc9 = new NativeListItem<object>("Arcade 17 Game : ", ArcadeMachines.ToArray());
			ArcadeBuyMenu.Add(Arc9);
			list8.Add(Arc9);
			NativeListItem<dynamic> Arc10 = new NativeListItem<object>("Arcade 18 Game : ", ArcadeMachines.ToArray());
			ArcadeBuyMenu.Add(Arc10);
			list8.Add(Arc10);
			NativeListItem<dynamic> Arc11 = new NativeListItem<object>("Arcade 19 Game : ", ArcadeMachines.ToArray());
			ArcadeBuyMenu.Add(Arc11);
			list8.Add(Arc11);
			NativeListItem<dynamic> Arc13 = new NativeListItem<object>("Arcade 20 Game : ", ArcadeMachines.ToArray());
			ArcadeBuyMenu.Add(Arc13);
			list8.Add(Arc13);
			NativeListItem<dynamic> RA = new NativeListItem<object>("Replace When Arcade : None : ", TF.ToArray());
			ArcadeBuyMenu.Add(RA);
			RA.Description = "If true, for the choosen spot, will replace the existing arcade with no arcade, turn off to save money";
			NativeItem SetARC = new NativeItem("Buy Arcade Machines ~g~$" + ArcadesPrice.ToString("N"));
			ArcadeBuyMenu.Add(SetARC);
			foreach (NativeListItem<object> item6 in list8)
			{
				item6.ItemChanged += delegate
				{
					foreach (Prop seconardyArcadeProp in SeconardyArcadeProps)
					{
						if (seconardyArcadeProp != null)
						{
							seconardyArcadeProp.Delete();
						}
					}
					if (Arc1.SelectedIndex != 0 && Arc1.SelectedIndex != ArcadeConfig.GetValue("Arcade", "Arcade1Ver", Arcade1Ver))
					{
						ArcadeCosts[0] = (object)ArcadeMachineCosts[Arc1.SelectedIndex];
					}
					else
					{
						ArcadeCosts[0] = 0;
					}
					if (Arc12.SelectedIndex != 0 && Arc12.SelectedIndex != ArcadeConfig.GetValue("Arcade", "Arcade2Ver", Arcade2Ver))
					{
						ArcadeCosts[1] = (object)ArcadeMachineCosts[Arc12.SelectedIndex];
					}
					else
					{
						ArcadeCosts[1] = 0;
					}
					if (Arc14.SelectedIndex != 0 && Arc14.SelectedIndex != ArcadeConfig.GetValue("Arcade", "Arcade3Ver", Arcade3Ver))
					{
						ArcadeCosts[2] = (object)ArcadeMachineCosts[Arc14.SelectedIndex];
					}
					else
					{
						ArcadeCosts[2] = 0;
					}
					if (Arc15.SelectedIndex != 0 && Arc15.SelectedIndex != ArcadeConfig.GetValue("Arcade", "Arcade4Ver", Arcade4Ver))
					{
						ArcadeCosts[3] = (object)ArcadeMachineCosts[Arc15.SelectedIndex];
					}
					else
					{
						ArcadeCosts[3] = 0;
					}
					if (Arc16.SelectedIndex != 0 && Arc16.SelectedIndex != ArcadeConfig.GetValue("Arcade", "Arcade5Ver", Arcade5Ver))
					{
						ArcadeCosts[4] = (object)ArcadeMachineCosts[Arc16.SelectedIndex];
					}
					else
					{
						ArcadeCosts[4] = 0;
					}
					if (Arc17.SelectedIndex != 0 && Arc17.SelectedIndex != ArcadeConfig.GetValue("Arcade", "Arcade6Ver", Arcade6Ver))
					{
						ArcadeCosts[5] = (object)ArcadeMachineCosts[Arc17.SelectedIndex];
					}
					else
					{
						ArcadeCosts[5] = 0;
					}
					if (Arc18.SelectedIndex != 0 && Arc18.SelectedIndex != ArcadeConfig.GetValue("Arcade", "Arcade7Ver", Arcade7Ver))
					{
						ArcadeCosts[6] = (object)ArcadeMachineCosts[Arc18.SelectedIndex];
					}
					else
					{
						ArcadeCosts[6] = 0;
					}
					if (Arc19.SelectedIndex != 0 && Arc19.SelectedIndex != ArcadeConfig.GetValue("Arcade", "Arcade8Ver", Arcade8Ver))
					{
						ArcadeCosts[7] = (object)ArcadeMachineCosts[Arc19.SelectedIndex];
					}
					else
					{
						ArcadeCosts[7] = 0;
					}
					if (Arc20.SelectedIndex != 0 && Arc20.SelectedIndex != ArcadeConfig.GetValue("Arcade", "Arcade9Ver", Arcade9Ver))
					{
						ArcadeCosts[8] = (object)ArcadeMachineCosts[Arc20.SelectedIndex];
					}
					else
					{
						ArcadeCosts[8] = 0;
					}
					if (Arc2.SelectedIndex != 0 && Arc2.SelectedIndex != ArcadeConfig.GetValue("Arcade", "Arcade10Ver", Arcade10Ver))
					{
						ArcadeCosts[9] = (object)ArcadeMachineCosts[Arc2.SelectedIndex];
					}
					else
					{
						ArcadeCosts[9] = 0;
					}
					if (Arc3.SelectedIndex != 0 && Arc1.SelectedIndex != ArcadeConfig.GetValue("Arcade", "Arcade11Ver", Arcade11Ver))
					{
						ArcadeCosts[10] = (object)ArcadeMachineCosts[Arc3.SelectedIndex];
					}
					else
					{
						ArcadeCosts[10] = 0;
					}
					if (Arc4.SelectedIndex != 0 && Arc4.SelectedIndex != ArcadeConfig.GetValue("Arcade", "Arcade12Ver", Arcade12Ver))
					{
						ArcadeCosts[11] = (object)ArcadeMachineCosts[Arc4.SelectedIndex];
					}
					else
					{
						ArcadeCosts[11] = 0;
					}
					if (Arc5.SelectedIndex != 0 && Arc5.SelectedIndex != ArcadeConfig.GetValue("Arcade", "Arcade13Ver", Arcade13Ver))
					{
						ArcadeCosts[12] = (object)ArcadeMachineCosts[Arc5.SelectedIndex];
					}
					else
					{
						ArcadeCosts[12] = 0;
					}
					if (Arc6.SelectedIndex != 0 && Arc6.SelectedIndex != ArcadeConfig.GetValue("Arcade", "Arcade14Ver", Arcade14Ver))
					{
						ArcadeCosts[13] = (object)ArcadeMachineCosts[Arc6.SelectedIndex];
					}
					else
					{
						ArcadeCosts[13] = 0;
					}
					if (Arc7.SelectedIndex != 0 && Arc7.SelectedIndex != ArcadeConfig.GetValue("Arcade", "Arcade15Ver", Arcade15Ver))
					{
						ArcadeCosts[14] = (object)ArcadeMachineCosts[Arc7.SelectedIndex];
					}
					else
					{
						ArcadeCosts[14] = 0;
					}
					if (Arc8.SelectedIndex != 0 && Arc8.SelectedIndex != ArcadeConfig.GetValue("Arcade", "Arcade16Ver", Arcade16Ver))
					{
						ArcadeCosts[15] = (object)ArcadeMachineCosts[Arc8.SelectedIndex];
					}
					else
					{
						ArcadeCosts[15] = 0;
					}
					if (Arc9.SelectedIndex != 0 && Arc9.SelectedIndex != ArcadeConfig.GetValue("Arcade", "Arcade17Ver", Arcade17Ver))
					{
						ArcadeCosts[16] = (object)ArcadeMachineCosts[Arc9.SelectedIndex];
					}
					else
					{
						ArcadeCosts[16] = 0;
					}
					if (Arc10.SelectedIndex != 0 && Arc10.SelectedIndex != ArcadeConfig.GetValue("Arcade", "Arcade18Ver", Arcade18Ver))
					{
						ArcadeCosts[17] = (object)ArcadeMachineCosts[Arc10.SelectedIndex];
					}
					else
					{
						ArcadeCosts[17] = 0;
					}
					if (Arc11.SelectedIndex != 0 && Arc11.SelectedIndex != ArcadeConfig.GetValue("Arcade", "Arcade19Ver", Arcade19Ver))
					{
						ArcadeCosts[18] = (object)ArcadeMachineCosts[Arc11.SelectedIndex];
					}
					else
					{
						ArcadeCosts[18] = 0;
					}
					if (Arc13.SelectedIndex != 0 && Arc13.SelectedIndex != ArcadeConfig.GetValue("Arcade", "Arcade20Ver", Arcade20Ver))
					{
						ArcadeCosts[19] = (object)ArcadeMachineCosts[Arc13.SelectedIndex];
					}
					else
					{
						ArcadeCosts[19] = 0;
					}
					ArcadesPrice = ArcadeCosts[0] + ArcadeCosts[1] + ArcadeCosts[2] + ArcadeCosts[3] + ArcadeCosts[4] + ArcadeCosts[5] + ArcadeCosts[6] + ArcadeCosts[7] + ArcadeCosts[8] + ArcadeCosts[9] + ArcadeCosts[10] + ArcadeCosts[11] + ArcadeCosts[12] + ArcadeCosts[13] + ArcadeCosts[14] + ArcadeCosts[15] + ArcadeCosts[16] + ArcadeCosts[17] + ArcadeCosts[18] + ArcadeCosts[19];
					SetARC.Title = "Buy Arcade Machines ~g~$" + ArcadesPrice.ToString("N");
					CurrentArcade = 1;
					CurrentArcadeValue = Arc1.SelectedIndex;
					CurrentArcade = 2;
					CurrentArcadeValue = Arc12.SelectedIndex;
					CurrentArcade = 3;
					CurrentArcadeValue = Arc14.SelectedIndex;
					CurrentArcade = 4;
					CurrentArcadeValue = Arc15.SelectedIndex;
					CurrentArcade = 5;
					CurrentArcadeValue = Arc16.SelectedIndex;
					CurrentArcade = 6;
					CurrentArcadeValue = Arc17.SelectedIndex;
					CurrentArcade = 7;
					CurrentArcadeValue = Arc18.SelectedIndex;
					CurrentArcade = 8;
					CurrentArcadeValue = Arc19.SelectedIndex;
					CurrentArcade = 9;
					CurrentArcadeValue = Arc20.SelectedIndex;
					CurrentArcade = 10;
					CurrentArcadeValue = Arc2.SelectedIndex;
					CurrentArcade = 11;
					CurrentArcadeValue = Arc3.SelectedIndex;
					CurrentArcade = 12;
					CurrentArcadeValue = Arc4.SelectedIndex;
					CurrentArcade = 13;
					CurrentArcadeValue = Arc5.SelectedIndex;
					CurrentArcade = 14;
					CurrentArcadeValue = Arc6.SelectedIndex;
					CurrentArcade = 15;
					CurrentArcadeValue = Arc7.SelectedIndex;
					CurrentArcade = 16;
					CurrentArcadeValue = Arc8.SelectedIndex;
					CurrentArcade = 17;
					CurrentArcadeValue = Arc9.SelectedIndex;
					CurrentArcade = 18;
					CurrentArcadeValue = Arc10.SelectedIndex;
					CurrentArcade = 19;
					CurrentArcadeValue = Arc11.SelectedIndex;
					CurrentArcade = 20;
					CurrentArcadeValue = Arc13.SelectedIndex;
					if (!BuyingArcades)
					{
						try
						{
							foreach (Ped arcadeInteriorPed in ArcadeInteriorPeds)
							{
								if (arcadeInteriorPed != null)
								{
									arcadeInteriorPed.Delete();
								}
							}
							foreach (Ped item7 in ArcadeInteriorPeds2)
							{
								if (item7 != null)
								{
									item7.Delete();
								}
							}
							foreach (Prop arcadeProp in ArcadeProps)
							{
								if (arcadeProp != null)
								{
									arcadeProp.Delete();
								}
							}
							foreach (Prop seconardyArcadeProp2 in SeconardyArcadeProps)
							{
								if (seconardyArcadeProp2 != null)
								{
									seconardyArcadeProp2.Delete();
								}
							}
							if (ArcadeProps.Count > 0)
							{
								foreach (Prop arcadeProp2 in ArcadeProps)
								{
									if (arcadeProp2 != null)
									{
										arcadeProp2.Delete();
									}
								}
							}
							foreach (Prop seconardyArcadeProp3 in SeconardyArcadeProps)
							{
								if (seconardyArcadeProp3 != null)
								{
									seconardyArcadeProp3.Delete();
								}
							}
							ArcadePos.Clear();
							RacingArcades.Clear();
							BuyingArcades = true;
							SetupFakeArcade(CurrentArcadeValue, 1, ArcadeMachines[CurrentArcadeValue]);
							ArcadeBuyCam = GlobalHelper.CreateCamera(FakeArcade.GetOffsetPosition(new Vector3(0f, -2f, 1f)), FakeArcade.Rotation, 60f);
							ArcadeBuyCam.IsActive = true;
							ArcadeBuyCam.FarClip = 2000f;
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
							World.RenderingCamera = ArcadeBuyCam;
						}
						catch
						{
							Notification.Show("Error on Cam Create");
						}
					}
					if (BuyingArcades)
					{
						try
						{
							foreach (Prop seconardyArcadeProp4 in SeconardyArcadeProps)
							{
								if (seconardyArcadeProp4 != null)
								{
									seconardyArcadeProp4.Delete();
								}
							}
							RacingArcades.Clear();
							SetupFakeArcade(CurrentArcadeValue, CurrentArcade, ArcadeMachines[CurrentArcadeValue]);
							ArcadeBuyCam.Position = FakeArcade.GetOffsetPosition(new Vector3(0f, -2f, 1f));
							ArcadeBuyCam.Rotation = FakeArcade.Rotation;
						}
						catch
						{
							Notification.Show("Error on Cam move");
						}
					}
				};
			}
			nativeItem11.Activated += delegate
			{
				Arc1.SelectedIndex = ArcadeConfig.GetValue("Arcade", "Arcade1Ver", Arcade1Ver);
				Arc12.SelectedIndex = ArcadeConfig.GetValue("Arcade", "Arcade2Ver", Arcade2Ver);
				Arc14.SelectedIndex = ArcadeConfig.GetValue("Arcade", "Arcade3Ver", Arcade3Ver);
				Arc15.SelectedIndex = ArcadeConfig.GetValue("Arcade", "Arcade4Ver", Arcade4Ver);
				Arc16.SelectedIndex = ArcadeConfig.GetValue("Arcade", "Arcade5Ver", Arcade5Ver);
				Arc17.SelectedIndex = ArcadeConfig.GetValue("Arcade", "Arcade6Ver", Arcade6Ver);
				Arc18.SelectedIndex = ArcadeConfig.GetValue("Arcade", "Arcade7Ver", Arcade7Ver);
				Arc19.SelectedIndex = ArcadeConfig.GetValue("Arcade", "Arcade8Ver", Arcade8Ver);
				Arc20.SelectedIndex = ArcadeConfig.GetValue("Arcade", "Arcade9Ver", Arcade9Ver);
				Arc2.SelectedIndex = ArcadeConfig.GetValue("Arcade", "Arcade10Ver", Arcade10Ver);
				Arc3.SelectedIndex = ArcadeConfig.GetValue("Arcade", "Arcade11Ver", Arcade11Ver);
				Arc4.SelectedIndex = ArcadeConfig.GetValue("Arcade", "Arcade12Ver", Arcade12Ver);
				Arc5.SelectedIndex = ArcadeConfig.GetValue("Arcade", "Arcade13Ver", Arcade13Ver);
				Arc6.SelectedIndex = ArcadeConfig.GetValue("Arcade", "Arcade14Ver", Arcade14Ver);
				Arc7.SelectedIndex = ArcadeConfig.GetValue("Arcade", "Arcade15Ver", Arcade15Ver);
				Arc8.SelectedIndex = ArcadeConfig.GetValue("Arcade", "Arcade16Ver", Arcade16Ver);
				Arc9.SelectedIndex = ArcadeConfig.GetValue("Arcade", "Arcade17Ver", Arcade17Ver);
				Arc10.SelectedIndex = ArcadeConfig.GetValue("Arcade", "Arcade18Ver", Arcade18Ver);
				Arc11.SelectedIndex = ArcadeConfig.GetValue("Arcade", "Arcade19Ver", Arcade19Ver);
				Arc13.SelectedIndex = ArcadeConfig.GetValue("Arcade", "Arcade20Ver", Arcade20Ver);
			};
			SetARC.Activated += delegate
			{
				if ((float)Game.Player.Character.Money <= ArcadesPrice)
				{
					ManagePlayerFunds.RemoveFunds((int)ArcadesPrice);
					Arcade1Ver = Arc1.SelectedIndex;
					if (TF[RA.SelectedIndex] == true && Arcade1Ver != ArcadeConfig.GetValue("Arcade", "Arcade1Ver", Arcade1Ver))
					{
						ArcadeConfig.SetValue("Arcade", "Arcade1Ver", Arcade1Ver);
					}
					else
					{
						ArcadeConfig.SetValue("Arcade", "Arcade1Ver", Arcade1Ver);
					}
					Arcade2Ver = Arc12.SelectedIndex;
					if (TF[RA.SelectedIndex] == true && Arcade2Ver != ArcadeConfig.GetValue("Arcade", "Arcade2Ver", Arcade2Ver))
					{
						ArcadeConfig.SetValue("Arcade", "Arcade2Ver", Arcade2Ver);
					}
					else
					{
						ArcadeConfig.SetValue("Arcade", "Arcade2Ver", Arcade2Ver);
					}
					Arcade3Ver = Arc14.SelectedIndex;
					if (TF[RA.SelectedIndex] == true && Arcade3Ver != ArcadeConfig.GetValue("Arcade", "Arcade3Ver", Arcade3Ver))
					{
						ArcadeConfig.SetValue("Arcade", "Arcade3Ver", Arcade3Ver);
					}
					else
					{
						ArcadeConfig.SetValue("Arcade", "Arcade3Ver", Arcade3Ver);
					}
					Arcade4Ver = Arc15.SelectedIndex;
					if (TF[RA.SelectedIndex] == true && Arcade4Ver != ArcadeConfig.GetValue("Arcade", "Arcade4Ver", Arcade4Ver))
					{
						ArcadeConfig.SetValue("Arcade", "Arcade4Ver", Arcade4Ver);
					}
					else
					{
						ArcadeConfig.SetValue("Arcade", "Arcade4Ver", Arcade4Ver);
					}
					Arcade5Ver = Arc16.SelectedIndex;
					if (TF[RA.SelectedIndex] == true && Arcade5Ver != ArcadeConfig.GetValue("Arcade", "Arcade5Ver", Arcade5Ver))
					{
						ArcadeConfig.SetValue("Arcade", "Arcade5Ver", Arcade5Ver);
					}
					else
					{
						ArcadeConfig.SetValue("Arcade", "Arcade5Ver", Arcade5Ver);
					}
					Arcade6Ver = Arc17.SelectedIndex;
					if (TF[RA.SelectedIndex] == true && Arcade6Ver != ArcadeConfig.GetValue("Arcade", "Arcade6Ver", Arcade6Ver))
					{
						ArcadeConfig.SetValue("Arcade", "Arcade6Ver", Arcade6Ver);
					}
					else
					{
						ArcadeConfig.SetValue("Arcade", "Arcade6Ver", Arcade6Ver);
					}
					Arcade7Ver = Arc18.SelectedIndex;
					if (TF[RA.SelectedIndex] == true && Arcade7Ver != ArcadeConfig.GetValue("Arcade", "Arcade7Ver", Arcade7Ver))
					{
						ArcadeConfig.SetValue("Arcade", "Arcade7Ver", Arcade7Ver);
					}
					else
					{
						ArcadeConfig.SetValue("Arcade", "Arcade7Ver", Arcade7Ver);
					}
					Arcade8Ver = Arc19.SelectedIndex;
					if (TF[RA.SelectedIndex] == true && Arcade8Ver != ArcadeConfig.GetValue("Arcade", "Arcade8Ver", Arcade8Ver))
					{
						ArcadeConfig.SetValue("Arcade", "Arcade8Ver", Arcade8Ver);
					}
					else
					{
						ArcadeConfig.SetValue("Arcade", "Arcade8Ver", Arcade8Ver);
					}
					Arcade9Ver = Arc20.SelectedIndex;
					if (TF[RA.SelectedIndex] == true && Arcade9Ver != ArcadeConfig.GetValue("Arcade", "Arcade9Ver", Arcade9Ver))
					{
						ArcadeConfig.SetValue("Arcade", "Arcade9Ver", Arcade9Ver);
					}
					else
					{
						ArcadeConfig.SetValue("Arcade", "Arcade9Ver", Arcade9Ver);
					}
					Arcade10Ver = Arc2.SelectedIndex;
					if (TF[RA.SelectedIndex] == true && Arcade10Ver != ArcadeConfig.GetValue("Arcade", "Arcade10Ver", Arcade10Ver))
					{
						ArcadeConfig.SetValue("Arcade", "Arcade10Ver", Arcade10Ver);
					}
					else
					{
						ArcadeConfig.SetValue("Arcade", "Arcade10Ver", Arcade10Ver);
					}
					Arcade11Ver = Arc3.SelectedIndex;
					if (TF[RA.SelectedIndex] == true && Arcade11Ver != ArcadeConfig.GetValue("Arcade", "Arcade11Ver", Arcade11Ver))
					{
						ArcadeConfig.SetValue("Arcade", "Arcade11Ver", Arcade11Ver);
					}
					else
					{
						ArcadeConfig.SetValue("Arcade", "Arcade11Ver", Arcade11Ver);
					}
					Arcade12Ver = Arc4.SelectedIndex;
					if (TF[RA.SelectedIndex] == true && Arcade12Ver != ArcadeConfig.GetValue("Arcade", "Arcade12Ver", Arcade12Ver))
					{
						ArcadeConfig.SetValue("Arcade", "Arcade12Ver", Arcade12Ver);
					}
					else
					{
						ArcadeConfig.SetValue("Arcade", "Arcade12Ver", Arcade12Ver);
					}
					Arcade13Ver = Arc5.SelectedIndex;
					if (TF[RA.SelectedIndex] == true && Arcade13Ver != ArcadeConfig.GetValue("Arcade", "Arcade13Ver", Arcade13Ver))
					{
						ArcadeConfig.SetValue("Arcade", "Arcade13Ver", Arcade13Ver);
					}
					else
					{
						ArcadeConfig.SetValue("Arcade", "Arcade13Ver", Arcade13Ver);
					}
					Arcade14Ver = Arc6.SelectedIndex;
					if (TF[RA.SelectedIndex] == true && Arcade14Ver != ArcadeConfig.GetValue("Arcade", "Arcade14Ver", Arcade14Ver))
					{
						ArcadeConfig.SetValue("Arcade", "Arcade14Ver", Arcade14Ver);
					}
					else
					{
						ArcadeConfig.SetValue("Arcade", "Arcade14Ver", Arcade14Ver);
					}
					Arcade15Ver = Arc7.SelectedIndex;
					if (TF[RA.SelectedIndex] == true && Arcade15Ver != ArcadeConfig.GetValue("Arcade", "Arcade15Ver", Arcade15Ver))
					{
						ArcadeConfig.SetValue("Arcade", "Arcade15Ver", Arcade15Ver);
					}
					else
					{
						ArcadeConfig.SetValue("Arcade", "Arcade15Ver", Arcade15Ver);
					}
					Arcade16Ver = Arc8.SelectedIndex;
					if (TF[RA.SelectedIndex] == true && Arcade16Ver != ArcadeConfig.GetValue("Arcade", "Arcade16Ver", Arcade16Ver))
					{
						ArcadeConfig.SetValue("Arcade", "Arcade16Ver", Arcade16Ver);
					}
					else
					{
						ArcadeConfig.SetValue("Arcade", "Arcade16Ver", Arcade16Ver);
					}
					Arcade17Ver = Arc9.SelectedIndex;
					if (TF[RA.SelectedIndex] == true && Arcade17Ver != ArcadeConfig.GetValue("Arcade", "Arcade17Ver", Arcade17Ver))
					{
						ArcadeConfig.SetValue("Arcade", "Arcade17Ver", Arcade17Ver);
					}
					else
					{
						ArcadeConfig.SetValue("Arcade", "Arcade17Ver", Arcade17Ver);
					}
					Arcade18Ver = Arc10.SelectedIndex;
					if (TF[RA.SelectedIndex] == true && Arcade18Ver != ArcadeConfig.GetValue("Arcade", "Arcade18Ver", Arcade18Ver))
					{
						ArcadeConfig.SetValue("Arcade", "Arcade18Ver", Arcade18Ver);
					}
					else
					{
						ArcadeConfig.SetValue("Arcade", "Arcade18Ver", Arcade18Ver);
					}
					Arcade19Ver = Arc11.SelectedIndex;
					if (TF[RA.SelectedIndex] == true && Arcade19Ver != ArcadeConfig.GetValue("Arcade", "Arcade19Ver", Arcade19Ver))
					{
						ArcadeConfig.SetValue("Arcade", "Arcade19Ver", Arcade19Ver);
					}
					else
					{
						ArcadeConfig.SetValue("Arcade", "Arcade19Ver", Arcade19Ver);
					}
					Arcade20Ver = Arc13.SelectedIndex;
					if (TF[RA.SelectedIndex] == true && Arcade20Ver != ArcadeConfig.GetValue("Arcade", "Arcade20Ver", Arcade20Ver))
					{
						ArcadeConfig.SetValue("Arcade", "Arcade20Ver", Arcade20Ver);
					}
					else
					{
						ArcadeConfig.SetValue("Arcade", "Arcade20Ver", Arcade20Ver);
					}
					ArcadeConfig.Save();
					foreach (Prop arcadeProp3 in ArcadeProps)
					{
						if (arcadeProp3 != null)
						{
							arcadeProp3.Delete();
						}
					}
					foreach (Prop seconardyArcadeProp5 in SeconardyArcadeProps)
					{
						if (seconardyArcadeProp5 != null)
						{
							seconardyArcadeProp5.Delete();
						}
					}
					RacingArcades.Clear();
					Notification.Show(GetHostName() + " : Arcades have been Set");
					RefreshArcadeInterior();
				}
				else
				{
					Notification.Show(GetHostName() + " : You do not have enough money for this Arcade Upgrade");
				}
			};
			HTD.ItemChanged += delegate
			{
				if (UndergrondAreaLocked == 0)
				{
					Notification.Show(GetHostName() + " : You need to purchase the Underground Area first");
				}
				if (UndergrondAreaLocked == 1)
				{
					UsingHiddenDoor = HTD.SelectedIndex;
					ArcadeConfig.SetValue("Arcade", "UsingHiddenTellerDoor", UsingHiddenDoor);
					ArcadeConfig.Save();
					RefreshArcadeInterior();
					Notification.Show(GetHostName() + " :  Hiden Teller Door has been set to " + TF[HTD.SelectedIndex]);
				}
			};
			foreach (NativeListItem<object> item8 in list7)
			{
				item8.ItemChanged += delegate
				{
					if (ST.SelectedIndex >= 1)
					{
						StyleCost = 155000 * (int)(1.25 * (double)ST.SelectedIndex);
					}
					else
					{
						StyleCost = 0f;
					}
					if (F.SelectedIndex >= 1)
					{
						FloorCost = 125000 * (int)(1.15 * (double)F.SelectedIndex);
					}
					else
					{
						FloorCost = 0f;
					}
					if (M.SelectedIndex >= 1)
					{
						MuralCost = 325000 * (int)(1.25 * (double)M.SelectedIndex);
					}
					else
					{
						MuralCost = 0f;
					}
					if (NA.SelectedIndex >= 1)
					{
						NeonArtCost = 155000 * (int)(1.25 * (double)NA.SelectedIndex);
					}
					else
					{
						NeonArtCost = 0f;
					}
					if (P.SelectedIndex >= 1)
					{
						PushieCost = 225000 * (int)(1.25 * (double)M.SelectedIndex);
					}
					else
					{
						PushieCost = 0f;
					}
					Cost = StyleCost + NeonArtCost + MuralCost + FloorCost + PushieCost;
					Buy.Title = "Buy " + Cost.ToString("N");
				};
			}
			nativeItem10.Activated += delegate
			{
				if (!ArcadeClutter)
				{
					ArcadeClutter = true;
					ArcadeConfig.SetValue("Arcade", "ArcadeClutter", ArcadeClutter);
					ArcadeConfig.Save();
					RefreshArcadeInterior();
					Notification.Show(GetHostName() + " : Underground Clutter Has been added");
				}
				else if (ArcadeClutter)
				{
					ArcadeClutter = false;
					ArcadeConfig.SetValue("Arcade", "ArcadeClutter", ArcadeClutter);
					ArcadeConfig.Save();
					RefreshArcadeInterior();
					Notification.Show(GetHostName() + " : Underground Clutter Has been removed");
				}
			};
			nativeItem6.Activated += delegate
			{
				if (Game.Player.Character.Money <= 250000)
				{
					ManagePlayerFunds.RemoveFunds(250000);
					UnlockedHackingEquipment = 1;
					ArcadeConfig.SetValue("Arcade", "UnlockedHackingEquipment", UnlockedHackingEquipment);
					ArcadeConfig.Save();
					RefreshArcadeInterior();
				}
				else
				{
					Notification.Show(GetHostName() + " : You do not have enough money for this Arcade Upgrade");
				}
			};
			nativeItem3.Activated += delegate
			{
				if (Game.Player.Character.Money <= 600000)
				{
					ManagePlayerFunds.RemoveFunds(600000);
					UnlockedGarage = 1;
					ArcadeConfig.SetValue("Arcade", "UnlockedGarage", UnlockedGarage);
					ArcadeConfig.Save();
					RefreshArcadeInterior();
				}
				else
				{
					Notification.Show(GetHostName() + " : You do not have enough money for this Arcade Upgrade");
				}
			};
			nativeItem4.Activated += delegate
			{
				if (Game.Player.Character.Money <= 1000000)
				{
					ManagePlayerFunds.RemoveFunds(1000000);
					UnlockedGunlocker = 1;
					ArcadeConfig.SetValue("Arcade", "UnlockedGunlocker", UnlockedGunlocker);
					ArcadeConfig.Save();
					RefreshArcadeInterior();
				}
				else
				{
					Notification.Show(GetHostName() + " : You do not have enough money for this Arcade Upgrade");
				}
			};
			nativeItem7.Activated += delegate
			{
				if (Game.Player.Character.Money <= 250000)
				{
					ManagePlayerFunds.RemoveFunds(250000);
					UnlockedBreachingEquipment = 1;
					ArcadeConfig.SetValue("Arcade", "UnlockedBreachingEquipment", UnlockedBreachingEquipment);
					ArcadeConfig.Save();
					RefreshArcadeInterior();
				}
				else
				{
					Notification.Show(GetHostName() + " : You do not have enough money for this Arcade Upgrade");
				}
			};
			nativeItem9.Activated += delegate
			{
				if (Game.Player.Character.Money <= 50000)
				{
					ManagePlayerFunds.RemoveFunds(50000);
					UnlockedPlansEquipment = 1;
					ArcadeConfig.SetValue("Arcade", "UnlockedPlansEquipment", UnlockedPlansEquipment);
					ArcadeConfig.Save();
					RefreshArcadeInterior();
				}
				else
				{
					Notification.Show(GetHostName() + " : You do not have enough money for this Arcade Upgrade");
				}
			};
			nativeItem8.Activated += delegate
			{
				if (Game.Player.Character.Money <= 150000)
				{
					ManagePlayerFunds.RemoveFunds(150000);
					UnlockedOutfitsEquipment = 1;
					ArcadeConfig.SetValue("Arcade", "UnlockedOutfitsEquipment", UnlockedOutfitsEquipment);
					ArcadeConfig.Save();
					RefreshArcadeInterior();
				}
				else
				{
					Notification.Show(GetHostName() + " : You do not have enough money for this Arcade Upgrade");
				}
			};
			nativeItem5.Activated += delegate
			{
				if (Game.Player.Character.Money <= 450000)
				{
					ManagePlayerFunds.RemoveFunds(450000);
					UnlockedDilllEquipment = 1;
					ArcadeConfig.SetValue("Arcade", "UnlockedDilllEquipment", UnlockedDilllEquipment);
					ArcadeConfig.Save();
					RefreshArcadeInterior();
				}
			};
			nativeItem2.Activated += delegate
			{
				if (ArcadeMoneyVault == 0)
				{
					if (Game.Player.Character.Money <= 1000000)
					{
						ManagePlayerFunds.RemoveFunds(1000000);
						ArcadeMoneyVault = 1;
						ArcadeConfig.SetValue("Arcade", "ArcadeMoneyVault", ArcadeMoneyVault);
						ArcadeConfig.Save();
						RefreshArcadeInterior();
					}
					else
					{
						Notification.Show(GetHostName() + " : You do not have enough money for this Arcade Upgrade");
					}
				}
			};
			Buy.Activated += delegate
			{
				if ((float)Game.Player.Character.Money <= Cost)
				{
					ManagePlayerFunds.RemoveFunds((int)Cost);
					ArcadeFloodID = F.SelectedIndex;
					ArcadeConfig.SetValue("Arcade", "ArcadeFloodID", ArcadeFloodID);
					MuralID = M.SelectedIndex;
					ArcadeConfig.SetValue("Arcade", "MuralID", MuralID);
					PushieID = P.SelectedIndex;
					ArcadeConfig.SetValue("Arcade", "PushieID", PushieID);
					ArcadeStyle = ST.SelectedIndex;
					ArcadeConfig.SetValue("Arcade", "ArcadeStyle", ArcadeStyle);
					NeonArtID = NA.SelectedIndex;
					ArcadeConfig.SetValue("Arcade", "NeonArtID", NeonArtID);
					ArcadeConfig.Save();
					RefreshArcadeInterior();
				}
				else
				{
					Notification.Show(GetHostName() + " : You do not have enough money for this Arcade Upgrade");
				}
			};
		}

		public int CheckArcadeOwned()
		{
			return ArcadeOwned;
		}

		public static Vector3 GetArcadeLoc()
		{
			Vector3 vector = new Vector3(0f, 0f, 0f);
			return ArcadeGarageEnter;
		}

		public void DrawSpotlight2(Color C, Vector3 pos, Vector3 Rot, float range, float brightness, float roundness)
		{
			Vector3 vector = pos;
			vector = new Vector3(vector.X, vector.Y, vector.Z);
			World.DrawSpotLightWithShadow(pos, new Vector3(0f, 90f, Rot.Z), C, range, brightness, 0f, 35f, 1f);
		}

		public void DrawSpotlight3(Color C, Vector3 pos, Vector3 Rot, float range, float brightness, float roundness)
		{
			Vector3 vector = pos;
			vector = new Vector3(vector.X, vector.Y, vector.Z);
			World.DrawSpotLightWithShadow(pos, new Vector3(Rot.X, Rot.Y, Rot.Z), C, range, brightness, 0f, 35f, 1f);
		}

		public void DrawSpotlight(Color C, Vector3 pos, Vector3 Rot, float range, float brightness, float roundness)
		{
			Vector3 vector = pos;
			vector = new Vector3(vector.X, vector.Y, vector.Z);
			World.DrawSpotLightWithShadow(pos, new Vector3(0f, -90f, Rot.Z), C, range, brightness, 0f, 35f, 1f);
			World.DrawSpotLightWithShadow(pos, new Vector3(0f, 90f, Rot.Z), C, range, brightness, 0f, 35f, 1f);
		}

		public float Get_RGB(Color C, string argb)
		{
			if (argb.Equals("a"))
			{
				return (int)C.A;
			}
			if (argb.Equals("r"))
			{
				return (int)C.R;
			}
			if (argb.Equals("g"))
			{
				return (int)C.G;
			}
			if (argb.Equals("b"))
			{
				return (int)C.B;
			}
			return 255f;
		}

		public void DrawLight(Color C, Vector3 pos, float range, float brightness)
		{
			float num = Get_RGB(C, "r");
			float num2 = Get_RGB(C, "g");
			float num3 = Get_RGB(C, "b");
			Vector3 vector = pos;
			vector = new Vector3(vector.X, vector.Y, vector.Z + 1f);
			Vector3 dir = pos - vector;
			dir.Normalize();
			World.DrawSpotLight(vector, dir, C, range, brightness, 0f, 10f, 1f);
		}

		public void CheckCrate(Ped Ped, Prop P, Vector3 Pos, Vector3 Rot, int type)
		{
			if (Ped == Game.Player.Character)
			{
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1024802024)
				{
					InitialAnimDone = true;
				}
				if (InitialAnimDone && !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim@heists@ornate_bank@grab_cash", "bag_intro", 1) && SetRob)
				{
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@heists@ornate_bank@grab_cash", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "bag_grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					MoneyProp = GlobalHelper.CreateProp(RequestModel("ch_prop_20dollar_pile_01a"), Game.Player.Character.Position, default(Vector3), false, false);
					MoneyProp.IsPositionFrozen = true;
					MoneyProp.IsCollisionEnabled = false;
					MoneyProp.IsInvincible = true;
					PlayingAnim = true;
					MoneyProp.Position = Game.Player.Character.Bones[Bone.IKLeftHand].Position;
					SetRob = false;
					InitialAnimDone = false;
				}
			}
		}

		public void RobCrate(Ped Ped, Prop P, Vector3 Pos, Vector3 Rot, int type)
		{
			if (Ped == Game.Player.Character)
			{
				GrabbingScene = 0;
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
				if (MoneyProp != null)
				{
					MoneyProp.Delete();
				}
				if (bagProp != null)
				{
					bagProp.Delete();
				}
				PlayingAnim = true;
				RobTimer = 0;
				bagProp = GlobalHelper.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
				bagProp.IsCollisionEnabled = false;
				bagProp.IsInvincible = true;
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "bag_intro", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@heists@ornate_bank@grab_cash", "intro", 1000.0, -4.0, 128, 0, 1148846080, 0);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, P, Scene1, "cart_cash_dissapear", LoadDict("anim@heists@ornate_bank@grab_cash"), 16f, -16f, 0, 1148846080);
				SetRob = true;
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

		public void TutorialStart()
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int x = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(0, 30), new Size(Convert.ToInt32(absoluteX), 450), 0f, Color.FromArgb(230, 255, 255, 255));
			TEXT("Tutorial Started", new Point(x, 100), 2.5f, Color.FromArgb(255, 0, 145, 255), GTA.UI.Font.Pricedown);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.Render2D();
		}

		public void TutorialEnd()
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int x = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(0, 30), new Size(Convert.ToInt32(absoluteX), 450), 0f, Color.FromArgb(230, 255, 255, 255));
			TEXT("Tutorial Finished", new Point(x, 100), 2.5f, Color.FromArgb(255, 0, 145, 255), GTA.UI.Font.Pricedown);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.Render2D();
		}

		private void drawSprite2(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha, int alpha2)
		{
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, screenX, screenY, width, height, 0, r, g, b, alpha, alpha2);
			}
		}

		private void drawSprite2(string textureDict, string textureName, float screenX, float screenY, float width, float height, float r, int g, int b, int alpha, int alpha2)
		{
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, screenX, screenY, width, height, 0, r, g, b, alpha, alpha2);
			}
		}

		private void drawSprite3(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha)
		{
			screenaspectratio = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
			float num = (float)(1.778 / (double)screenaspectratio);
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, GetRatio(screenX), screenY, width / 1920f * num, height / 1920f, 0, r, g, b, alpha, 0);
			}
		}

		private void drawSpriteRTA(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha, float Heading)
		{
			screenaspectratio = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
			float num = (float)(1.778 / (double)screenaspectratio);
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, GetRatio(screenX), screenY, width / 1280f * num, height / 1280f, Heading, r, g, b, alpha, 0);
			}
		}

		private float func_11081(float fParam0)
		{
			fLocal_564 = Function.Call<float>(Hash.GET_ASPECT_RATIO, false);
			if (Function.Call<bool>(Hash.IS_PC_VERSION) && fLocal_564 >= 4f)
			{
				fLocal_564 /= 3f;
			}
			fParam0 = 0.5f - (0.5f - fParam0) / fLocal_564;
			return fParam0;
		}

		public float GetRatio(float num)
		{
			return 0.5f - (0.5f - num) / screenaspectratio;
		}

		private Vector2 World3DToScreen2d2(Vector3 pos)
		{
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, pos.X, pos.Y, pos.Z, outputArgument, outputArgument2);
			return new Vector2(outputArgument.GetResult<float>(), outputArgument2.GetResult<float>());
		}

		public void ShowInstructionalButtonsVT()
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int num = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(0, 10), new Size(Convert.ToInt32(absoluteX), 2020), 0f, Color.FromArgb(200, 255, 255, 255));
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 176, 0), "Select Node");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 172, 0), "Selection Up");
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 173, 0), "Selection Down");
			scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Quit");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public string ReturnNFC(char C)
		{
			string result = "";
			int num = int.Parse(C.ToString());
			if (num == 0)
			{
				result = "0";
			}
			if (num == 1)
			{
				result = "1";
			}
			if (num == 2)
			{
				result = "2";
			}
			if (num == 3)
			{
				result = "3";
			}
			if (num == 4)
			{
				result = "4";
			}
			if (num == 5)
			{
				result = "5";
			}
			if (num == 6)
			{
				result = "6";
			}
			if (num == 7)
			{
				result = "7";
			}
			if (num == 8)
			{
				result = "8";
			}
			if (num == 9)
			{
				result = "9";
			}
			return result;
		}

		public string ReturnNFC(int C)
		{
			string result = "";
			if (C == 0)
			{
				result = "0";
			}
			if (C == 1)
			{
				result = "1";
			}
			if (C == 2)
			{
				result = "2";
			}
			if (C == 3)
			{
				result = "3";
			}
			if (C == 4)
			{
				result = "4";
			}
			if (C == 5)
			{
				result = "5";
			}
			if (C == 6)
			{
				result = "6";
			}
			if (C == 7)
			{
				result = "7";
			}
			if (C == 8)
			{
				result = "8";
			}
			if (C == 9)
			{
				result = "9";
			}
			return result;
		}

		public void TextNotification(string text)
		{
			InputArgument[] arguments = new InputArgument[1] { "STRING" };
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, arguments);
			InputArgument[] arguments2 = new InputArgument[1] { text };
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, arguments2);
			InputArgument[] arguments3 = new InputArgument[4] { 0, 0, 0, -1 };
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, arguments3);
		}

		private Vector2 World3DToScreen2d(Vector3 pos)
		{
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, pos.X, pos.Y, pos.Z, outputArgument, outputArgument2);
			return new Vector2(outputArgument.GetResult<float>(), outputArgument2.GetResult<float>());
		}

		public void ShowInstructionalButtons()
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int num = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(0, 10), new Size(Convert.ToInt32(absoluteX), 2020), 0f, Color.FromArgb(200, 255, 255, 255));
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 32, 0), "Pattern Up");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 33, 0), "Pattern Down");
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 35, 0), "Pattern Next");
			scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 34, 0), "Pattern Back");
			scaleform.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 37, 0), "Check");
			scaleform.CallFunction("SET_DATA_SLOT", 5, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Quit");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public string ReturnNFCPH(char C)
		{
			string result = "";
			int num = int.Parse(C.ToString());
			if (num == 0)
			{
				result = "fingerprint_hacking_minigame_numbers_00";
			}
			if (num == 1)
			{
				result = "fingerprint_hacking_minigame_numbers_01";
			}
			if (num == 2)
			{
				result = "fingerprint_hacking_minigame_numbers_02";
			}
			if (num == 3)
			{
				result = "fingerprint_hacking_minigame_numbers_03";
			}
			if (num == 4)
			{
				result = "fingerprint_hacking_minigame_numbers_04";
			}
			if (num == 5)
			{
				result = "fingerprint_hacking_minigame_numbers_05";
			}
			if (num == 6)
			{
				result = "fingerprint_hacking_minigame_numbers_06";
			}
			if (num == 7)
			{
				result = "fingerprint_hacking_minigame_numbers_07";
			}
			if (num == 8)
			{
				result = "fingerprint_hacking_minigame_numbers_08";
			}
			if (num == 9)
			{
				result = "fingerprint_hacking_minigame_numbers_09";
			}
			return result;
		}

		public string ReturnNFCPH(int C)
		{
			string result = "";
			if (C == 0)
			{
				result = "fingerprint_hacking_minigame_numbers_00";
			}
			if (C == 1)
			{
				result = "fingerprint_hacking_minigame_numbers_01";
			}
			if (C == 2)
			{
				result = "fingerprint_hacking_minigame_numbers_02";
			}
			if (C == 3)
			{
				result = "fingerprint_hacking_minigame_numbers_03";
			}
			if (C == 4)
			{
				result = "fingerprint_hacking_minigame_numbers_04";
			}
			if (C == 5)
			{
				result = "fingerprint_hacking_minigame_numbers_05";
			}
			if (C == 6)
			{
				result = "fingerprint_hacking_minigame_numbers_06";
			}
			if (C == 7)
			{
				result = "fingerprint_hacking_minigame_numbers_07";
			}
			if (C == 8)
			{
				result = "fingerprint_hacking_minigame_numbers_08";
			}
			if (C == 9)
			{
				result = "fingerprint_hacking_minigame_numbers_09";
			}
			return result;
		}

		private float progresswidth(float percent)
		{
			return 0.08f * percent;
		}

		private float progressxcoord(float percent)
		{
			float num = 0.04f * percent;
			return 0.9f + num;
		}

		private void drawText(string text, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		public void CheckCrate2(Ped Ped, Prop P, Vector3 Pos, Vector3 Rot, int type)
		{
			if (!(Ped == Game.Player.Character))
			{
				return;
			}
			if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
			{
				if (CurrentType == 4 && !PlayerFirstAnim)
				{
					PlayerFirstAnim = true;
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, PlayerRobbingProp, Scene1, "anim@scripted@heist@ig1_table_grab@cash@male@", "grab_cash", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerRobbingProp, Scene1, "grab_cash", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 16f, -16f, 0, 1148846080);
					InitialAnimDone = true;
				}
				if (CurrentType == 5 && !PlayerFirstAnim)
				{
					PlayerFirstAnim = true;
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, PlayerRobbingProp, Scene1, "anim@scripted@heist@ig1_table_grab@gold@male@", "grab_gold", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerRobbingProp, Scene1, "grab_gold", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 16f, -16f, 0, 1148846080);
					InitialAnimDone = true;
				}
				if (CurrentType != 4 && CurrentType != 5)
				{
					InitialAnimDone = true;
				}
			}
			if (InitialAnimDone && !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim@scripted@heist@ig1_table_grab@cash@male@", "Enter", 1) && SetRob)
			{
				if (CurrentType == 1)
				{
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@heists@ornate_bank@grab_cash", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "bag_grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					MoneyProp = GlobalHelper.CreateProp(RequestModel("ch_prop_20dollar_pile_01a"), Game.Player.Character.Position, default(Vector3), false, false);
					MoneyProp.IsPositionFrozen = true;
					MoneyProp.IsCollisionEnabled = false;
					MoneyProp.IsInvincible = true;
					PlayingAnim = true;
					MoneyProp.Position = Game.Player.Character.Bones[Bone.IKLeftHand].Position;
					SetRob = false;
					InitialAnimDone = false;
				}
				if (CurrentType == 2)
				{
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@heists@ornate_bank@grab_cash", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "bag_grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					MoneyProp = GlobalHelper.CreateProp(RequestModel("ch_prop_gold_bar_01a"), Game.Player.Character.Position, default(Vector3), false, false);
					MoneyProp.IsPositionFrozen = true;
					MoneyProp.IsCollisionEnabled = false;
					MoneyProp.IsInvincible = true;
					PlayingAnim = true;
					MoneyProp.Position = Game.Player.Character.Bones[Bone.IKLeftHand].Position;
					CurrentType = 2;
					SetRob = false;
					InitialAnimDone = false;
				}
				if (CurrentType == 3)
				{
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@heists@ornate_bank@grab_cash", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "bag_grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					MoneyProp = GlobalHelper.CreateProp(RequestModel("ch_prop_vault_dimaondbox_01a"), Game.Player.Character.Position, default(Vector3), false, false);
					MoneyProp.IsPositionFrozen = true;
					MoneyProp.IsCollisionEnabled = false;
					MoneyProp.IsInvincible = true;
					MoneyProp.Position = Game.Player.Character.Bones[Bone.IKLeftHand].Position;
					CurrentType = 3;
					SetRob = false;
					InitialAnimDone = false;
				}
				if (CurrentType == 4)
				{
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@scripted@heist@ig1_table_grab@cash@male@", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "grab_bag", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					PlayerGrabbingStage = 0;
					CurrentType = 4;
					SetRob = false;
					InitialAnimDone = false;
					PlayerWaitingForLastAnim = true;
					return;
				}
				if (CurrentType == 5)
				{
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@scripted@heist@ig1_table_grab@gold@male@", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "grab_bag", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					PlayerGrabbingStage = 0;
					CurrentType = 5;
					SetRob = false;
					InitialAnimDone = false;
					PlayerWaitingForLastAnim = true;
					return;
				}
			}
			if (CurrentType == 4 && !SetRob)
			{
				if (PlayerWaitingForLastAnim)
				{
					if (RobTimer >= 60)
					{
						RobTimer = 0;
					}
					if (RobTimer != 60)
					{
						RobTimer++;
					}
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
					{
						PlayerFinishedLastAnim = true;
						PlayerWaitingForLastAnim = false;
						Rot = Game.Player.Character.Rotation;
						Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, PlayerRobbingProp.Position.X, PlayerRobbingProp.Position.Y, PlayerRobbingProp.Position.Z, 0.0, 0.0, Rot.Z, 2);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, PlayerRobbingProp.Position.X, PlayerRobbingProp.Position.Y, PlayerRobbingProp.Position.Z, 0.0, 0.0, Rot.Z, true);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "exit_bag", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@scripted@heist@ig1_table_grab@cash@male@", "exit", 1000.0, -4.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "exit", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
						return;
					}
				}
				if (PlayerFinishedLastAnim && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
				{
					PlayerFinishedLastAnim = false;
					Game.Player.Character.Task.ClearAll();
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
					PlayingAnim = false;
					if (MoneyProp != null)
					{
						MoneyProp.Delete();
						MoneyProp.MarkAsNoLongerNeeded();
						MoneyProp = null;
					}
					if (bagProp != null)
					{
						bagProp.Delete();
						bagProp.MarkAsNoLongerNeeded();
						bagProp = null;
					}
					return;
				}
			}
			if (CurrentType == 5 && !SetRob)
			{
				if (PlayerWaitingForLastAnim)
				{
					if (RobTimer >= 60)
					{
						RobTimer = 0;
					}
					if (RobTimer != 60)
					{
						RobTimer++;
					}
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
					{
						PlayerFinishedLastAnim = true;
						PlayerWaitingForLastAnim = false;
						Rot = Game.Player.Character.Rotation;
						Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, PlayerRobbingProp.Position.X, PlayerRobbingProp.Position.Y, PlayerRobbingProp.Position.Z, 0.0, 0.0, Rot.Z, 2);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, PlayerRobbingProp.Position.X, PlayerRobbingProp.Position.Y, PlayerRobbingProp.Position.Z, 0.0, 0.0, Rot.Z, true);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "exit_bag", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z, 1148846080, 0);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@scripted@heist@ig1_table_grab@gold@male@", "exit", 1000.0, -4.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "exit", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z, 1148846080, 0);
						return;
					}
				}
				if (PlayerFinishedLastAnim && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
				{
					PlayerFinishedLastAnim = false;
					Game.Player.Character.Task.ClearAll();
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
					PlayingAnim = false;
					if (MoneyProp != null)
					{
						MoneyProp.Delete();
						MoneyProp.MarkAsNoLongerNeeded();
						MoneyProp = null;
					}
					if (bagProp != null)
					{
						bagProp.Delete();
						bagProp.MarkAsNoLongerNeeded();
						bagProp = null;
					}
					return;
				}
			}
			if (CurrentType != 8)
			{
				return;
			}
			DisplayHelpTextThisFrame("Hold ~INPUT_ATTACK~ to Cut Glass, Careful not to overheat the Cutter, ~INPUT_CONTEXT~ Exit");
			if (PlayerGotJewels && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CuttingScene1) >= 1057353216)
			{
				PlayerGotJewels = false;
				foreach (Prop prop in SecurityCabinetProps[CabinetPropsSelectedIndex].Props)
				{
					if (prop != null)
					{
						prop.Opacity = 0;
					}
				}
			}
			if (PlayerPlayingFinalAnim && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CuttingScene1) >= 1065353216)
			{
				if (CuttingGlassPlate != null)
				{
					CuttingGlassPlate.Delete();
				}
				Ped character = Game.Player.Character;
				if (Game.Player.Character.Model == PedHash.Trevor)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 1, 0, 1);
				}
				if (Game.Player.Character.Model == PedHash.Michael)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 2, 0, 1);
				}
				if (Game.Player.Character.Model == PedHash.Franklin)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 6, 0, 1);
				}
				SetRob = false;
				PowerOutput = 0f;
				if (MoneyProp != null)
				{
					MoneyProp.Delete();
					MoneyProp.MarkAsNoLongerNeeded();
					MoneyProp = null;
				}
				if (bagProp != null)
				{
					bagProp.Delete();
					bagProp.MarkAsNoLongerNeeded();
					bagProp = null;
				}
				CurrentType = 0;
				PlayingAnim = false;
				PlayerPlayingFinalAnim = false;
				Game.Player.Character.Task.ClearAll();
			}
			if (PlayerPlayingFinalAnim)
			{
				return;
			}
			float percent = PowerOutput / (float)(150 * Crate_Valuables.Count);
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.91f, 0.21f, 0.04f, 255, 255, 255, 130);
			drawSprite2("timerbars", "damagebarfill_128", 0.94f, 0.91f, 0.08f, 0.03f, 150, 191, 255, 130);
			drawSprite2("timerbars", "damagebarfill_128", progressxcoord(percent), 0.91f, progresswidth(percent), 0.03f, 120, 120, 120, 255);
			drawText("Heat ", 0.84f, 0.9f, 0.3f, 255, 255, 255);
			if (!PlayerFinishedCutting)
			{
				if (Game.IsControlPressed(Control.Context))
				{
					Random random = new Random();
					if (CuttingSound != -1)
					{
						Function.Call(Hash.RELEASE_SOUND_ID, CuttingSound);
						Function.Call(Hash.STOP_SOUND, CuttingSound);
						CuttingSound = -1;
					}
					LoadDict("anim@scripted@heist@ig16_glass_cut@male@");
					LoadDict("anim@scripted@heist@ig16_glass_cut@male@");
					CuttingScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CuttingVector3.X, CuttingVector3.Y, CuttingVector3.Z, 0.0, 0.0, Cuttingheading, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, CuttingScene1, CuttingVector3.X, CuttingVector3.Y, CuttingVector3.Z, 0.0, 0.0, Cuttingheading, true);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, CuttingScene1, "exit_bag", LoadDict("anim@scripted@heist@ig16_glass_cut@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, CuttingScene1, "exit_cutter", LoadDict("anim@scripted@heist@ig16_glass_cut@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CuttingScene1, "anim@scripted@heist@ig16_glass_cut@male@", "exit", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CuttingScene1, "exit", LoadDict("anim@scripted@heist@ig16_glass_cut@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					PlayerPlayingFinalAnim = true;
					PlayerStartedCuttingAnim = false;
				}
				if (CuttingTime >= RequiredPowerOutput)
				{
					PowerOutput += 1f;
					TimerPowerOutput = 0;
					PlayerFinishedCutting = true;
				}
				if (Game.IsControlPressed(Control.Attack))
				{
					Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ih_fin");
					Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ih_fin");
					Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ih_fin");
					Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ih_fin");
					Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ih_fin");
					Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ih_fin");
					if (!Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEI4/DLCHEI4_GENERIC_01", false, -1))
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEI4/DLCHEI4_GENERIC_01", false, -1);
					}
					if (!Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", false, -1))
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", false, -1);
					}
					if (!PlayingPTFX)
					{
						Function.Call(Hash.STOP_SOUND, CuttingSound);
						Function.Call(Hash.RELEASE_SOUND_ID, CuttingSound);
						Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, CuttingPTFX);
						CuttingPTFX = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_ih_fin_glass_cutter_cut", MoneyProp, 0f, 0f, 0f, 0f, 0f, 0f, 1f, false, false, false);
						PlayingPTFX = true;
						CuttingSound = GlobalHelper.CreateSoundID();
						Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, CuttingSound, "StartCutting", MoneyProp, "DLC_H4_anims_glass_cutter_Sounds", false, 80);
					}
					PowerOutput += 0.75f;
					if (PowerOutput >= (float)(150 * Crate_Valuables.Count) && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CuttingScene1) >= 1035353216)
					{
						PlayingPTFX = false;
						Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, CuttingPTFX);
						Function.Call(Hash.STOP_SOUND, CuttingSound);
						Function.Call(Hash.RELEASE_SOUND_ID, CuttingSound);
						PlayingPTFX = false;
						PlayerStartedCuttingAnim = false;
						CuttingScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CuttingVector3.X, CuttingVector3.Y, CuttingVector3.Z, 0.0, 0.0, Cuttingheading, 2);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, CuttingScene1, CuttingVector3.X, CuttingVector3.Y, CuttingVector3.Z, 0.0, 0.0, Cuttingheading, true);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, CuttingScene1, "overheat_react_bag", LoadDict("anim@scripted@heist@ig16_glass_cut@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, CuttingScene1, "overheat_react_cutter", LoadDict("anim@scripted@heist@ig16_glass_cut@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CuttingScene1, "anim@scripted@heist@ig16_glass_cut@male@", "overheat_react", 1000.0, -4.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CuttingScene1, "overheat_react", LoadDict("anim@scripted@heist@ig16_glass_cut@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
						PowerOutput = 0f;
						TimerPowerOutput = 0;
					}
					if (CuttingTime <= RequiredPowerOutput)
					{
						if (TimerPowerOutput < 10 * Crate_Valuables.Count)
						{
							TimerPowerOutput++;
						}
						if (TimerPowerOutput >= 10 * Crate_Valuables.Count)
						{
							CuttingTime += 3;
							TimerPowerOutput = 0;
						}
						if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CuttingScene1) >= 1065353216)
						{
							PlayingPTFX = false;
							PlayerStartedCuttingAnim = true;
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, CuttingScene1, "cutting_loop_bag", LoadDict("anim@scripted@heist@ig16_glass_cut@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, CuttingScene1, "cutting_loop_cutter", LoadDict("anim@scripted@heist@ig16_glass_cut@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CuttingScene1, "anim@scripted@heist@ig16_glass_cut@male@", "cutting_loop", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CuttingScene1, "cutting_loop", LoadDict("anim@scripted@heist@ig16_glass_cut@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
						}
					}
				}
				if (PlayerStartedCuttingAnim && !Game.IsControlPressed(Control.Attack))
				{
					if (PlayingPTFX)
					{
						PlayingPTFX = false;
						Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, CuttingPTFX);
						Function.Call(Hash.STOP_SOUND, CuttingSound);
						Function.Call(Hash.RELEASE_SOUND_ID, CuttingSound);
					}
					if (PowerOutput > 0f)
					{
						PowerOutput -= 1f;
						if (TimerPowerOutput >= 0 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CuttingScene1) >= 1065353216)
						{
							CuttingScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CuttingVector3.X, CuttingVector3.Y, CuttingVector3.Z, 0.0, 0.0, Cuttingheading, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, CuttingScene1, CuttingVector3.X, CuttingVector3.Y, CuttingVector3.Z, 0.0, 0.0, Cuttingheading, true);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, CuttingScene1, "idle_bag", LoadDict("anim@scripted@heist@ig16_glass_cut@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, CuttingScene1, "idle_cutter", LoadDict("anim@scripted@heist@ig16_glass_cut@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CuttingScene1, "anim@scripted@heist@ig16_glass_cut@male@", "idle", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CuttingScene1, "idle", LoadDict("anim@scripted@heist@ig16_glass_cut@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
						}
						if (TimerPowerOutput > 0 && TimerPowerOutput <= 100)
						{
							TimerPowerOutput = 0;
						}
					}
					if (PowerOutput <= 0f && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CuttingScene1) >= 1035353216)
					{
						PlayingPTFX = false;
						Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, CuttingPTFX);
						Function.Call(Hash.STOP_SOUND, CuttingSound);
						Function.Call(Hash.RELEASE_SOUND_ID, CuttingSound);
						PlayerStartedCuttingAnim = false;
						CuttingScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CuttingVector3.X, CuttingVector3.Y, CuttingVector3.Z, 0.0, 0.0, Cuttingheading, 2);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, CuttingScene1, CuttingVector3.X, CuttingVector3.Y, CuttingVector3.Z, 0.0, 0.0, Cuttingheading, true);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, CuttingScene1, "overheat_react_bag", LoadDict("anim@scripted@heist@ig16_glass_cut@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, CuttingScene1, "overheat_react_cutter", LoadDict("anim@scripted@heist@ig16_glass_cut@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CuttingScene1, "anim@scripted@heist@ig16_glass_cut@male@", "overheat_react", 1000.0, -4.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CuttingScene1, "overheat_react", LoadDict("anim@scripted@heist@ig16_glass_cut@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
						PowerOutput = 0f;
						TimerPowerOutput = 0;
					}
				}
			}
			if (PlayerFinishedCutting && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CuttingScene1) >= 1035353216)
			{
				PlayingPTFX = false;
				Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, CuttingPTFX);
				Function.Call(Hash.STOP_SOUND, CuttingSound);
				Function.Call(Hash.RELEASE_SOUND_ID, CuttingSound);
				Crate_Valuables[CuttingCabinet].IsVisible = false;
				if (SecurityCabinetProps[CabinetPropsSelectedIndex].GlassDisplayCase != null)
				{
					SecurityCabinetProps[CabinetPropsSelectedIndex].GlassDisplayCase.Delete();
				}
				SecurityCabinetProps[CabinetPropsSelectedIndex].GlassDisplayCase = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_glass_disp_01b"), Crate_Valuables[CuttingCabinet].Position, Crate_Valuables[CuttingCabinet].Rotation, false, false);
				SecurityCabinetProps[CabinetPropsSelectedIndex].GlassDisplayCase.IsPositionFrozen = true;
				HubPointsRobbed[CuttingCabinet] = true;
				CuttingScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, CuttingVector3.X, CuttingVector3.Y, CuttingVector3.Z, 0.0, 0.0, Cuttingheading, 2);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, CuttingScene1, CuttingVector3.X, CuttingVector3.Y, CuttingVector3.Z, 0.0, 0.0, Cuttingheading, true);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, CuttingScene1, "success_bag", LoadDict("anim@scripted@heist@ig16_glass_cut@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, CuttingScene1, "success_cutter", LoadDict("anim@scripted@heist@ig16_glass_cut@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, CuttingGlassPlate, CuttingScene1, "success_glass_piece", LoadDict("anim@scripted@heist@ig16_glass_cut@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CuttingScene1, "anim@scripted@heist@ig16_glass_cut@male@", "success", 1000.0, -4.0, 128, 0, 1148846080, 0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CuttingScene1, "success", LoadDict("anim@scripted@heist@ig16_glass_cut@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
				PlayerPlayingFinalAnim = true;
				PlayerStartedCuttingAnim = false;
				PlayerGotJewels = true;
			}
		}

		public void RobCrate2(Ped Ped, Prop P, Vector3 Pos, Vector3 Rot, int type)
		{
			LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@");
			LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@");
			if (!(Ped == Game.Player.Character))
			{
				return;
			}
			InitialAnimDone = false;
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
			if (type == 1)
			{
				PlayingAnim = true;
				RobTimer = 0;
				bagProp = GlobalHelper.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
				bagProp.IsCollisionEnabled = false;
				bagProp.IsInvincible = true;
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "bag_intro", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@heists@ornate_bank@grab_cash", "intro", 1000.0, -4.0, 128, 0, 1148846080, 0);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, P, Scene1, "cart_cash_dissapear", LoadDict("anim@heists@ornate_bank@grab_cash"), 16f, -16f, 0, 1148846080);
				SetRob = true;
				CurrentType = 1;
			}
			if (type == 2)
			{
				RobTimer = 0;
				if (MoneyProp != null)
				{
					MoneyProp.Delete();
					MoneyProp.MarkAsNoLongerNeeded();
					MoneyProp = null;
				}
				if (bagProp != null)
				{
					bagProp.Delete();
					bagProp.MarkAsNoLongerNeeded();
					bagProp = null;
				}
				bagProp = GlobalHelper.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
				bagProp.IsCollisionEnabled = false;
				bagProp.IsInvincible = true;
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "bag_intro", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@heists@ornate_bank@grab_cash", "intro", 1000.0, -4.0, 128, 0, 1148846080, 0);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, P, Scene1, "cart_cash_dissapear", LoadDict("anim@heists@ornate_bank@grab_cash"), 16f, -16f, 0, 1148846080);
				SetRob = true;
				CurrentType = 2;
			}
			if (type == 3)
			{
				RobTimer = 0;
				if (MoneyProp != null)
				{
					MoneyProp.Delete();
					MoneyProp.MarkAsNoLongerNeeded();
					MoneyProp = null;
				}
				if (bagProp != null)
				{
					bagProp.Delete();
					bagProp.MarkAsNoLongerNeeded();
					bagProp = null;
				}
				bagProp = GlobalHelper.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
				bagProp.IsCollisionEnabled = false;
				bagProp.IsInvincible = true;
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "bag_intro", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@heists@ornate_bank@grab_cash", "intro", 1000.0, -4.0, 128, 0, 1148846080, 0);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, P, Scene1, "cart_cash_dissapear", LoadDict("anim@heists@ornate_bank@grab_cash"), 16f, -16f, 0, 1148846080);
				SetRob = true;
				CurrentType = 3;
			}
			if (type == 4)
			{
				PlayerFirstAnim = false;
				PlayingAnim = true;
				RobTimer = 0;
				if (bagProp != null)
				{
					bagProp.Delete();
					bagProp.MarkAsNoLongerNeeded();
					bagProp = null;
				}
				bagProp = GlobalHelper.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
				bagProp.IsCollisionEnabled = false;
				bagProp.IsInvincible = true;
				PlayerRobbingProp = P;
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "enter_bag", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@scripted@heist@ig1_table_grab@cash@male@", "enter", 1000.0, -4.0, 128, 0, 1148846080, 0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "enter", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
				SetRob = true;
				CurrentType = 4;
			}
			if (type == 5)
			{
				PlayerFirstAnim = false;
				PlayingAnim = true;
				RobTimer = 0;
				if (bagProp != null)
				{
					bagProp.Delete();
					bagProp.MarkAsNoLongerNeeded();
					bagProp = null;
				}
				bagProp = GlobalHelper.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
				bagProp.IsCollisionEnabled = false;
				bagProp.IsInvincible = true;
				PlayerRobbingProp = P;
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "enter_bag", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@scripted@heist@ig1_table_grab@gold@male@", "enter", 1000.0, -4.0, 128, 0, 1148846080, 0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "enter", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
				SetRob = true;
				CurrentType = 5;
			}
			if (type == 8)
			{
				PlayingPTFX = false;
				PlayerStartedCuttingAnim = false;
				if (CuttingGlassPlate != null)
				{
					CuttingGlassPlate.Delete();
				}
				Random random = new Random();
				PlayerFirstAnim = false;
				PlayingAnim = true;
				RobTimer = 0;
				CuttingTime = 0;
				if (CuttingGlassPlate != null)
				{
					CuttingGlassPlate.Delete();
				}
				CuttingGlassPlate = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_glass_cut_01a"), new Vector3(CuttingVector3.X, CuttingVector3.Y, CuttingVector3.Z), false, false);
				CuttingGlassPlate.IsPositionFrozen = true;
				CuttingGlassPlate.IsCollisionEnabled = false;
				CuttingGlassPlate.IsInvincible = true;
				if (bagProp != null)
				{
					bagProp.Delete();
					bagProp.MarkAsNoLongerNeeded();
					bagProp = null;
				}
				bagProp = GlobalHelper.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
				bagProp.IsCollisionEnabled = false;
				bagProp.IsInvincible = true;
				if (MoneyProp != null)
				{
					MoneyProp.Delete();
					MoneyProp.MarkAsNoLongerNeeded();
					MoneyProp = null;
				}
				MoneyProp = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_cutter_01a"), Game.Player.Character.Position, default(Vector3), false, false);
				MoneyProp.IsPositionFrozen = true;
				MoneyProp.IsCollisionEnabled = false;
				MoneyProp.IsInvincible = true;
				CuttingVector3 = Pos;
				PlayerRobbingProp = P;
				Cuttingheading = Rot.Z;
				LoadDict("anim@scripted@heist@ig16_glass_cut@male@");
				CuttingScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, CuttingScene1, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, CuttingScene1, "enter_bag", LoadDict("anim@scripted@heist@ig16_glass_cut@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, CuttingScene1, "enter_cutter", LoadDict("anim@scripted@heist@ig16_glass_cut@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CuttingScene1, "anim@scripted@heist@ig16_glass_cut@male@", "enter", 1000.0, -4.0, 128, 0, 1148846080, 0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, CuttingScene1, "enter", LoadDict("anim@scripted@heist@ig16_glass_cut@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
				Script.Wait(100);
				PlayerFinishedCutting = false;
				SetRob = true;
				PowerOutput = 0f;
				RequiredPowerOutput = random.Next(40, 399);
				TimerPowerOutput = 100;
				CurrentType = 8;
			}
		}

		public void ShowInstructionalButtonsCFPH()
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int num = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(0, 10), new Size(Convert.ToInt32(absoluteX), 2020), 0f, Color.FromArgb(200, 255, 255, 255));
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 32, 0), "Pattern Up");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 33, 0), "Pattern Down");
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 35, 0), "Pattern Next");
			scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 34, 0), "Pattern Back");
			scaleform.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 37, 0), "Check");
			scaleform.CallFunction("SET_DATA_SLOT", 5, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Quit");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
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

		public Vector3 RotationToDirection(Vector3 rotation)
		{
			Vector3 vector = new Vector3(MathF.PI / 180f * rotation.X, MathF.PI / 180f * rotation.Y, MathF.PI / 180f * rotation.Z);
			return new Vector3((float)((0.0 - Math.Sin(vector.Z)) * Math.Abs(Math.Cos(vector.X))), (float)(Math.Cos(vector.Z) * Math.Abs(Math.Cos(vector.X))), (float)Math.Sin(vector.X));
		}

		public void RemoveHighlights()
		{
			PrepBoard2.CallFunction("REMOVE_HIGHLIGHT", 0);
			PrepBoard2.CallFunction("REMOVE_HIGHLIGHT", 1);
			PrepBoard2.CallFunction("REMOVE_HIGHLIGHT", 2);
			PrepBoard2.CallFunction("REMOVE_HIGHLIGHT", 3);
			PrepBoard2.CallFunction("REMOVE_HIGHLIGHT", 4);
			PrepBoard2.CallFunction("REMOVE_HIGHLIGHT", 5);
			PrepBoard2.CallFunction("REMOVE_HIGHLIGHT", 6);
			PrepBoard2.CallFunction("REMOVE_HIGHLIGHT", 7);
			PrepBoard2.CallFunction("REMOVE_HIGHLIGHT", 8);
			PrepBoard2.CallFunction("REMOVE_HIGHLIGHT", 9);
		}

		public void AddHighlight1(float xPos, float yPos, float radius)
		{
			PrepBoard2.CallFunction("ADD_HIGHLIGHT", 0, xPos, yPos, radius, 255, 0, 0, 255);
		}

		public void AddHighlight2(float xPos, float yPos, float radius)
		{
			PrepBoard2.CallFunction("ADD_HIGHLIGHT", 1, xPos, yPos, radius, 255, 0, 0, 255);
		}

		public void AddHighlight3(float xPos, float yPos, float radius)
		{
			PrepBoard2.CallFunction("ADD_HIGHLIGHT", 2, xPos, yPos, radius, 255, 0, 0, 255);
		}

		public void AddHighlight4(float xPos, float yPos, float radius)
		{
			PrepBoard2.CallFunction("ADD_HIGHLIGHT", 3, xPos, yPos, radius, 255, 0, 0, 255);
		}

		public void AddHighlight5(float xPos, float yPos, float radius)
		{
			PrepBoard2.CallFunction("ADD_HIGHLIGHT", 4, xPos, yPos, radius, 255, 0, 0, 255);
		}

		public void AddHighlight6(float xPos, float yPos, float radius)
		{
			PrepBoard2.CallFunction("ADD_HIGHLIGHT", 5, xPos, yPos, radius, 255, 0, 0, 255);
		}

		public void AddHighlight7(float xPos, float yPos, float radius)
		{
			PrepBoard2.CallFunction("ADD_HIGHLIGHT", 6, xPos, yPos, radius, 255, 0, 0, 255);
		}

		public void AddHighlight8(float xPos, float yPos, float radius)
		{
			PrepBoard2.CallFunction("ADD_HIGHLIGHT", 7, xPos, yPos, radius, 255, 0, 0, 255);
		}

		public void AddHighlight9(float xPos, float yPos, float radius)
		{
			PrepBoard2.CallFunction("ADD_HIGHLIGHT", 8, xPos, yPos, radius, 255, 0, 0, 255);
		}

		public void AddHighlight10(float xPos, float yPos, float radius)
		{
			PrepBoard2.CallFunction("ADD_HIGHLIGHT", 8, xPos, yPos, radius, 255, 0, 0, 255);
		}

		public static void CreateMainBlips()
		{
			foreach (Blip item in MainBlips_U)
			{
				if (item != null)
				{
					item.Delete();
				}
			}
			foreach (Blip item2 in MainBlips_G)
			{
				if (item2 != null)
				{
					item2.Delete();
				}
			}
			Blip blip = GlobalHelper.CreateBlip(new Vector3(2690.6f, -364.4f, -56f));
			blip.Sprite = BlipSprite.Heist;
			blip.Name = "Start Heist Here";
			blip.IsShortRange = true;
			blip.Scale = 0.8f;
			MainBlips_U.Add(blip);
			blip = GlobalHelper.CreateBlip(new Vector3(2712.77f, -369.45f, -55f));
			blip.Sprite = BlipSprite.Heist;
			blip.Name = "Start Heist Here";
			blip.IsShortRange = true;
			blip.Scale = 0.8f;
			MainBlips_U.Add(blip);
			blip = GlobalHelper.CreateBlip(HireBodyguardPosition);
			blip.Sprite = BlipSprite.Friend;
			blip.Name = "Hire Crew";
			blip.IsShortRange = true;
			blip.Scale = 0.8f;
			MainBlips_U.Add(blip);
			blip = GlobalHelper.CreateBlip(PurchaseVehicles.PurchaseVehicleMarker);
			blip.Sprite = BlipSprite.VehicleForSale;
			blip.Name = "Purchase A Vehicle";
			blip.IsShortRange = true;
			blip.Scale = 0.8f;
			MainBlips_U.Add(blip);
			blip = GlobalHelper.CreateBlip(ArcadeGarageExit);
			blip.Sprite = BlipSprite.Garage;
			blip.Name = "Exit";
			blip.IsShortRange = true;
			blip.Scale = 0.8f;
			MainBlips_U.Add(blip);
			blip = GlobalHelper.CreateBlip(new Vector3(2681.73f, -388.05f, 0f));
			blip.Sprite = BlipSprite.DollarBill;
			blip.Name = "Heist Earnings";
			blip.IsShortRange = true;
			blip.Scale = 0.8f;
			MainBlips_U.Add(blip);
			blip = GlobalHelper.CreateBlip(new Vector3(2722.344f, -390.42f, 0f));
			blip.Sprite = BlipSprite.Safe;
			blip.Name = "Money Vault";
			blip.IsShortRange = true;
			blip.Scale = 0.8f;
			MainBlips_U.Add(blip);
			blip = GlobalHelper.CreateBlip(new Vector3(2693.217f, -371.64f, -55f));
			blip.Sprite = BlipSprite.Keypad2;
			blip.Name = "Practice Hacking Minigames";
			blip.IsShortRange = true;
			blip.Scale = 0.8f;
			MainBlips_U.Add(blip);
			blip = GlobalHelper.CreateBlip(PracticeDrill);
			blip.Sprite = BlipSprite.BoltCutters;
			blip.Name = "Practice Drilling Minigames";
			blip.IsShortRange = true;
			blip.Scale = 0.8f;
			MainBlips_U.Add(blip);
			blip = GlobalHelper.CreateBlip(new Vector3(2680.91577f, -364.7354f, -56.2122421f));
			blip.Sprite = BlipSprite.Safe;
			blip.Name = "Practice Safes";
			blip.IsShortRange = true;
			blip.Scale = 0.8f;
			MainBlips_U.Add(blip);
			blip = GlobalHelper.CreateBlip(GunLockerMarker);
			blip.Sprite = BlipSprite.Pistol;
			blip.Name = "Gunlocker";
			blip.IsShortRange = true;
			blip.Scale = 0.8f;
			MainBlips_U.Add(blip);
			blip = GlobalHelper.CreateBlip(ArcadeBusinessExit);
			blip.Sprite = BlipSprite.Garage;
			blip.Name = "Exit";
			blip.IsShortRange = true;
			blip.Scale = 0.8f;
			MainBlips_G.Add(blip);
			blip = GlobalHelper.CreateBlip(new Vector3(2725.5f, -385f, -50f));
			blip.Sprite = BlipSprite.Bar;
			blip.Name = "Purchase A Drink";
			blip.IsShortRange = true;
			MainBlips_G.Add(blip);
			blip = GlobalHelper.CreateBlip(new Vector3(2726.6f, -371.8277f, -48.4f));
			blip.Sprite = BlipSprite.Stairs;
			blip.Name = "Underground Area";
			blip.IsShortRange = true;
			blip.Scale = 0.8f;
			MainBlips_G.Add(blip);
		}

		private void DrawRect(float screenX, float screenY, float width, float height, int r, int g, int b, int alpha)
		{
			screenaspectratio = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
			float num = (float)(1.778 / (double)screenaspectratio);
			Function.Call(Hash.DRAW_RECT, GetRatio(screenX), screenY, width / 1920f * num, height / 1920f, r, g, b, alpha, 0);
		}

		public unsafe static void DrawCustomHeistStartBoard(Vector3 Position, Vector3 Rotation, Vector3 Size)
		{
			if (CustomHeistBoard == null)
			{
				CustomHeistBoard = new Scaleform("HEIST_MP");
			}
			if (GetDistance_VDIST2(Game.Player.Character.Position, Position, "<", 3.5f))
			{
				if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
				{
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Start ~t~" + Heists_Data[CurrentHeistData].HeistName);
					if (Game.IsControlJustPressed(Control.Context))
					{
						if (Heists_Data[CurrentHeistData].Heist == HeistID.The247Job)
						{
							The247Job.StartHeist();
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.TheSilthouseJob)
						{
							TheSilthouseJob.StartHeist();
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.Fleeca)
						{
							Class1.RunFleeca = 1;
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.Paleto)
						{
							Class1.RunPaleto = 1;
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.PacificStandard)
						{
							Class1.RunPacificStandard = 1;
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.JewelStore)
						{
							Class1.RunJewelryStore = 1;
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.UnionDepository)
						{
							Class1.RunUnionDepositoryHeist = 1;
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.LaFuenteBlanca)
						{
							Class1.RunLaFauenteBlanca = 1;
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.LaoubPrincessCargoshipHeist)
						{
							CargoShipHeist.StartHeist();
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.BikerClubhouse)
						{
							Class1.RunBikerClubhouseHeist = 1;
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.TheDiamondCasino)
						{
							TheDiamondHeist.RunHeist = 1;
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.CayoPericoHeist)
						{
							CayoPericoHeist.RunHeist = 1;
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.ThePrisonBreak)
						{
							ThePrisonBreak.RunHeist = 1;
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.TheHumaineLabs)
						{
							HumainLabsOnline.RunHeist = 1;
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.TheLuxigntonRaid)
						{
							TheLexingtonRaid.StartHeist();
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.TheCartelYachtHeist)
						{
							YachtHeist.StartHeist();
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.TheBillionairesParty)
						{
							TheBillionaresParty.StartHeist();
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.TheFederalReserveTrainHeist)
						{
							TheFederalReserveTrainHeist.StartHeist();
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.TheMazeBankSavingsBankHeist)
						{
							TheMazebankSavingsBankHeistcs.StartHeist();
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.ThePrinthouseHeist)
						{
							ThePrinthouseHeist.StartHeist();
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.TheGefangnisArtHeist)
						{
							TheGefangnisArtHeist.StartHeist();
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.TheBettaBankHeist)
						{
							TheBetaBankHeist.StartHeist();
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.TheDiamondJewelStoreHeist)
						{
							TheDiamondJewelStore.StartHeist();
						}
					}
				}
				else
				{
					DisplayHelpTextThisFrame("Press ~INPUT_VEH_EXIT~ to Start ~t~" + Heists_Data[CurrentHeistData].HeistName);
					if (Game.IsControlJustPressed(Control.VehicleExit))
					{
						if (Heists_Data[CurrentHeistData].Heist == HeistID.The247Job)
						{
							The247Job.StartHeist();
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.TheSilthouseJob)
						{
							TheSilthouseJob.StartHeist();
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.Fleeca)
						{
							Class1.RunFleeca = 1;
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.Paleto)
						{
							Class1.RunPaleto = 1;
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.PacificStandard)
						{
							Class1.RunPacificStandard = 1;
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.JewelStore)
						{
							Class1.RunJewelryStore = 1;
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.UnionDepository)
						{
							Class1.RunUnionDepositoryHeist = 1;
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.LaFuenteBlanca)
						{
							Class1.RunLaFauenteBlanca = 1;
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.LaoubPrincessCargoshipHeist)
						{
							CargoShipHeist.StartHeist();
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.BikerClubhouse)
						{
							Class1.RunBikerClubhouseHeist = 1;
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.TheDiamondCasino)
						{
							TheDiamondHeist.RunHeist = 1;
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.CayoPericoHeist)
						{
							CayoPericoHeist.RunHeist = 1;
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.ThePrisonBreak)
						{
							ThePrisonBreak.RunHeist = 1;
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.TheHumaineLabs)
						{
							HumainLabsOnline.RunHeist = 1;
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.TheLuxigntonRaid)
						{
							TheLexingtonRaid.StartHeist();
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.TheCartelYachtHeist)
						{
							YachtHeist.StartHeist();
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.TheBillionairesParty)
						{
							TheBillionaresParty.StartHeist();
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.TheFederalReserveTrainHeist)
						{
							TheFederalReserveTrainHeist.StartHeist();
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.TheMazeBankSavingsBankHeist)
						{
							TheMazebankSavingsBankHeistcs.StartHeist();
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.ThePrinthouseHeist)
						{
							ThePrinthouseHeist.StartHeist();
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.TheGefangnisArtHeist)
						{
							TheGefangnisArtHeist.StartHeist();
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.TheBettaBankHeist)
						{
							TheBetaBankHeist.StartHeist();
						}
						if (Heists_Data[CurrentHeistData].Heist == HeistID.TheDiamondJewelStoreHeist)
						{
							TheDiamondJewelStore.StartHeist();
						}
					}
				}
				Scaleform scaleform = new Scaleform("instructional_buttons");
				scaleform.CallFunction("CLEAR_ALL");
				scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
				scaleform.CallFunction("CREATE_CONTAINER");
				scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 174, 0), "Prev Heist");
				scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 175, 0), "Next Heist");
				scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
				scaleform.Render2D();
				if (Game.IsControlJustPressed(Control.PhoneLeft))
				{
					RenderedBoardStage = 0;
					RenderedIntel = 0;
					CurrentHeistData--;
					if (CurrentHeistData <= -1)
					{
						CurrentHeistData = Heists_Data.Count - 1;
					}
				}
				if (Game.IsControlJustPressed(Control.PhoneRight))
				{
					RenderedBoardStage = 0;
					RenderedIntel = 0;
					CurrentHeistData++;
					if (CurrentHeistData >= Heists_Data.Count - 1)
					{
						CurrentHeistData = 0;
					}
				}
			}
			if (!GetDistance_VDIST2(Game.Player.Character.Position, Position, "<", 15f) || CustomHeistBoard == null)
			{
				return;
			}
			if (Game.IsControlJustPressed(Control.Context) && CustomHeistBoard != null)
			{
				int handle = CustomHeistBoard.Handle;
				Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &handle);
				CustomHeistBoard = new Scaleform("HEIST_MP");
			}
			if (RenderedIntel == 0)
			{
				CustomHeistBoard.CallFunction("SET_LABELS", Heists_Data[CurrentHeistData].HeistName, "Continue", "AVERAGE TAKE", "THE PLAN", "Reward", "", "ROLE", "CUT", "STATUS", "", "");
				CustomHeistBoard.CallFunction("ADD_CREW_MEMBER", 0, Game.Player.Name, XP.PlayerLevel.ToString(), "", "Driver/Driller", 0, "NOT READY", 7, Heists_Data[CurrentHeistData].AveragePay.ToString(), "100", 0, "", "");
				CustomHeistBoard.CallFunction("UPDATE_CREW_MEMBER", 0, Game.Player.Name, XP.PlayerLevel.ToString(), "", "Driver / Driller", 0, "NOT READY", 7, Heists_Data[CurrentHeistData].AveragePay.ToString(), "100", 0, "", "");
				CustomHeistBoard.CallFunction("ADD_PLAYERCARD", 0, Game.Player.Name, "7", "Driver/Driller", true, true, true, true, "4.0", "Heist Leader", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
				CustomHeistBoard.CallFunction("UPDATE_PLAYERCARD", 0, Game.Player.Name, "7", "Driver/Driller", true, true, true, true, "4.0", "Heist Leader", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
				CustomHeistBoard.CallFunction("ADD_PLAYERCARD", 1, "Optional AI 1", "0", "Gunner", false, false, false, false, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
				CustomHeistBoard.CallFunction("UPDATE_PLAYERCARD", 1, "Optional AI 1", "0", "Gunner", false, false, false, false, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
				CustomHeistBoard.CallFunction("ADD_PLAYERCARD", 2, "Optional AI 1", "0", "Gunner", false, false, false, false, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
				CustomHeistBoard.CallFunction("UPDATE_PLAYERCARD", 2, "Optional AI 1", "0", "Gunner", false, false, false, false, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
				CustomHeistBoard.CallFunction("ADD_PLAYERCARD", 3, "Optional AI 1", "0", "Gunner", false, false, false, false, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
				CustomHeistBoard.CallFunction("UPDATE_PLAYERCARD", 3, "Optional AI 1", "0", "Gunner", false, false, false, false, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
				if (Heists_Data[CurrentHeistData].Players >= 2)
				{
					CustomHeistBoard.CallFunction("ADD_CREW_MEMBER", 1, "Optional AI 1", "0", "", "Gunner", 0, "NOT READY", 7, "0", "0", 0, "", "");
					CustomHeistBoard.CallFunction("UPDATE_CREW_MEMBER", 1, "Optional AI 1", "0", "", "Gunner", 0, "NOT READY", 7, "0", "0", 0, "", "");
					CustomHeistBoard.CallFunction("ADD_PLAYERCARD", 1, "Optional AI 1", "0", "Gunner", true, true, true, true, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
					CustomHeistBoard.CallFunction("UPDATE_PLAYERCARD", 1, "Optional AI 1", "0", "Gunner", true, true, true, true, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
				}
				if (Heists_Data[CurrentHeistData].Players >= 3)
				{
					CustomHeistBoard.CallFunction("ADD_CREW_MEMBER", 2, "Optional AI 2", "0", "", "Gunner", 0, "NOT READY", 7, "0", "0", 0, "", "");
					CustomHeistBoard.CallFunction("UPDATE_CREW_MEMBER", 2, "Optional AI 2", "0", "", "Gunner", 0, "NOT READY", 7, "0", "0", 0, "", "");
					CustomHeistBoard.CallFunction("ADD_PLAYERCARD", 2, "Optional AI 1", "0", "Gunner", true, true, true, true, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
					CustomHeistBoard.CallFunction("UPDATE_PLAYERCARD", 2, "Optional AI 1", "0", "Gunner", true, true, true, true, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
				}
				if (Heists_Data[CurrentHeistData].Players >= 4)
				{
					CustomHeistBoard.CallFunction("ADD_CREW_MEMBER", 3, "Optional AI 3", "0", "", "Gunner", 0, "NOT READY", 7, "0", "0", 0, "", "");
					CustomHeistBoard.CallFunction("UPDATE_CREW_MEMBER", 3, "Optional AI 3", "0", "", "Gunner", 0, "NOT READY", 7, "0", "0", 0, "", "");
					CustomHeistBoard.CallFunction("ADD_PLAYERCARD", 3, "Optional AI 3", "0", "Gunner", true, true, true, true, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
					CustomHeistBoard.CallFunction("UPDATE_PLAYERCARD", 3, "Optional AI 3", "0", "Gunner", true, true, true, true, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
				}
				CustomHeistBoard.CallFunction("ADD_PIECHART", Heists_Data[CurrentHeistData].AveragePay, 100f, "0", "0", "0", "REWARD");
				CustomHeistBoard.CallFunction("UPDATE_PIECHART", Heists_Data[CurrentHeistData].AveragePay, 100f, "0", "0", "0", "REWARD");
				CustomHeistBoard.CallFunction("SET_HEIST_NAME", Heists_Data[CurrentHeistData].HeistName, " ", " ", " ");
				CustomHeistBoard.CallFunction("UPDATE_HEIST_NAME", Heists_Data[CurrentHeistData].HeistName, " ", " ", " ");
				if (Heists_Data[CurrentHeistData].Objectives.Count == 1)
				{
					CustomHeistBoard.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0]);
					CustomHeistBoard.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0]);
				}
				if (Heists_Data[CurrentHeistData].Objectives.Count == 2)
				{
					CustomHeistBoard.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1]);
					CustomHeistBoard.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1]);
				}
				if (Heists_Data[CurrentHeistData].Objectives.Count == 3)
				{
					CustomHeistBoard.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2]);
					CustomHeistBoard.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2]);
				}
				if (Heists_Data[CurrentHeistData].Objectives.Count == 4)
				{
					CustomHeistBoard.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3]);
					CustomHeistBoard.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3]);
				}
				if (Heists_Data[CurrentHeistData].Objectives.Count == 5)
				{
					CustomHeistBoard.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4]);
					CustomHeistBoard.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4]);
				}
				if (Heists_Data[CurrentHeistData].Objectives.Count == 6)
				{
					CustomHeistBoard.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5]);
					CustomHeistBoard.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5]);
				}
				if (Heists_Data[CurrentHeistData].Objectives.Count == 7)
				{
					CustomHeistBoard.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6]);
					CustomHeistBoard.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6]);
				}
				if (Heists_Data[CurrentHeistData].Objectives.Count == 8)
				{
					CustomHeistBoard.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6], Heists_Data[CurrentHeistData].Objectives[7]);
					CustomHeistBoard.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6], Heists_Data[CurrentHeistData].Objectives[7]);
				}
				if (Heists_Data[CurrentHeistData].Objectives.Count == 9)
				{
					CustomHeistBoard.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6], Heists_Data[CurrentHeistData].Objectives[7], Heists_Data[CurrentHeistData].Objectives[8]);
					CustomHeistBoard.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6], Heists_Data[CurrentHeistData].Objectives[7], Heists_Data[CurrentHeistData].Objectives[8]);
				}
				if (Heists_Data[CurrentHeistData].Objectives.Count == 10)
				{
					CustomHeistBoard.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6], Heists_Data[CurrentHeistData].Objectives[7], Heists_Data[CurrentHeistData].Objectives[8], Heists_Data[CurrentHeistData].Objectives[9]);
					CustomHeistBoard.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6], Heists_Data[CurrentHeistData].Objectives[7], Heists_Data[CurrentHeistData].Objectives[8], Heists_Data[CurrentHeistData].Objectives[9]);
				}
				if (Heists_Data[CurrentHeistData].Objectives.Count == 11)
				{
					CustomHeistBoard.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6], Heists_Data[CurrentHeistData].Objectives[7], Heists_Data[CurrentHeistData].Objectives[8], Heists_Data[CurrentHeistData].Objectives[9], Heists_Data[CurrentHeistData].Objectives[10]);
					CustomHeistBoard.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6], Heists_Data[CurrentHeistData].Objectives[7], Heists_Data[CurrentHeistData].Objectives[8], Heists_Data[CurrentHeistData].Objectives[9], Heists_Data[CurrentHeistData].Objectives[10]);
				}
				if (Heists_Data[CurrentHeistData].Objectives.Count == 12)
				{
					CustomHeistBoard.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6], Heists_Data[CurrentHeistData].Objectives[7], Heists_Data[CurrentHeistData].Objectives[8], Heists_Data[CurrentHeistData].Objectives[9], Heists_Data[CurrentHeistData].Objectives[10], Heists_Data[CurrentHeistData].Objectives[11]);
					CustomHeistBoard.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6], Heists_Data[CurrentHeistData].Objectives[7], Heists_Data[CurrentHeistData].Objectives[8], Heists_Data[CurrentHeistData].Objectives[9], Heists_Data[CurrentHeistData].Objectives[10], Heists_Data[CurrentHeistData].Objectives[11]);
				}
				if (Heists_Data[CurrentHeistData].Objectives.Count == 13)
				{
					CustomHeistBoard.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6], Heists_Data[CurrentHeistData].Objectives[7], Heists_Data[CurrentHeistData].Objectives[8], Heists_Data[CurrentHeistData].Objectives[9], Heists_Data[CurrentHeistData].Objectives[10], Heists_Data[CurrentHeistData].Objectives[11], Heists_Data[CurrentHeistData].Objectives[12]);
					CustomHeistBoard.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6], Heists_Data[CurrentHeistData].Objectives[7], Heists_Data[CurrentHeistData].Objectives[8], Heists_Data[CurrentHeistData].Objectives[9], Heists_Data[CurrentHeistData].Objectives[10], Heists_Data[CurrentHeistData].Objectives[11], Heists_Data[CurrentHeistData].Objectives[12]);
				}
				CustomHeistBoard.CallFunction("ADD_LAUNCH_BUTTON");
				CustomHeistBoard.CallFunction("INITIALISE_HEISTBOARD");
				CustomHeistBoard.CallFunction("LAUNCH_BUTTON_VISIBLE", true);
				CustomHeistBoard.CallFunction("LAUNCH_BUTTON_ENABLED", true);
				CustomHeistBoard.CallFunction("SET_LEADER_COST", "Leader Setup Cost:", 11500);
				CustomHeistBoard.CallFunction("SHOW_HEISTBOARD");
				RenderedBoardStage = 1;
			}
			CustomHeistBoard.Render3D(Position, Rotation, Size);
		}

		private unsafe void OnTick(object sender, EventArgs e)
		{
			if (LesterHasToldPlayerOfNewHeists == 0 && ArcadeOwned == 1)
			{
				ArcadeOwned = 0;
				ArcadeVersion = 0;
				if (ArcadeConfig != null)
				{
					ArcadeConfig.SetValue("Arcade", "ArcadeOwned", ArcadeOwned);
					ArcadeConfig.SetValue("Arcade", "ArcadeVersion", ArcadeVersion);
					ArcadeConfig.Save();
				}
			}
			if (SPH_Debug.ShowMainDebug)
			{
				drawText("SPH Main Class Running", 0.1f, 0.1f, 0.5f, 255, 255, 255);
			}
			if (ArcadeOwned != 0)
			{
				if (!IsInArcade && ArcadeOwned == 1 && GetDistance_VDIST2(Game.Player.Character.Position, ArcadeBusinessEnter, ">", 100f) && ArcadeOwned != 0)
				{
					return;
				}
				if (SPH_Debug.ShowRunningClassDebug)
				{
					drawText("Main/Arcade Running", 0.8f, 0.03f, 0.5f, 255, 255, 255);
				}
				if (SPH_Debug.ShowMainDebug)
				{
					drawText("Near/In SPH Arcade", 0.1f, 0.15f, 0.5f, 255, 255, 255);
				}
			}
			if (mainMenu.Visible)
			{
				modLemonUIObjectPool.HideAll();
			}
			if (modLemonUIObjectPool != null)
			{
				modLemonUIObjectPool.Process();
			}
			if (ArcadeOwned == 1)
			{
				if (SPH_Debug.ShowMainDebug)
				{
					drawText("In SPH Arcade " + IsInArcade, 0.1f, 0.2f, 0.5f, 255, 255, 255);
				}
				if (!IsInArcade)
				{
					switch (ArcadeOwned)
					{
					case 0:
						if (Arcade != null)
						{
							Arcade.Alpha = 0;
						}
						break;
					case 1:
						if (Arcade != null)
						{
							Arcade.Alpha = 255;
						}
						break;
					}
					if (LesterHasToldPlayerOfNewHeists == 1 && HeistBlip != null)
					{
						HeistBlip.Alpha = 0;
					}
					RereadMoney = false;
					int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 2730.0, -380.0, -50.0);
					int num2 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 2712.1, -361.2773, -51.867);
					Vector3 position = Game.Player.Character.Position;
					int num3 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position.X, position.Y, position.Z);
					if (num3 == num2 && num3 != 0)
					{
						RefreshArcadeInterior();
						IsInArcade = true;
						DestoryCars();
						CreateCars("GarageA");
					}
					if (num3 == num && num3 != 0)
					{
						RefreshArcadeInterior();
						IsInArcade = true;
						DestoryCars();
						CreateCars("GarageA");
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, ArcadeBusinessEnter, "<", 100f))
					{
						Vector3 arcadeBusinessEnter = ArcadeBusinessEnter;
						arcadeBusinessEnter = new Vector3(arcadeBusinessEnter.X, arcadeBusinessEnter.Y, arcadeBusinessEnter.Z + 10f);
						arcadeBusinessEnter = new Vector3(arcadeBusinessEnter.X, arcadeBusinessEnter.Y, World.GetGroundHeight(arcadeBusinessEnter));
						World.DrawMarker(MarkerType.Cylinder, arcadeBusinessEnter, Vector3.Zero, Vector3.Zero, new Vector3(1f, 1f, 0.45f), Color.Goldenrod);
						if (GetDistance_VDIST2(Game.Player.Character.Position, ArcadeBusinessEnter, "<", 2f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Arcade");
							if (GetDistance_VDIST2(Game.Player.Character.Position, ArcadeBusinessEnter, "<", 2f) && Game.IsControlPressed(Control.Context))
							{
								DestoryCars();
								CreateCars("GarageA");
								RefreshArcadeInterior();
								GTA.UI.Screen.FadeOut(300);
								Script.Wait(300);
								GTA.UI.Screen.FadeIn(300);
								Game.Player.Character.Position = ArcadeBusinessExit;
								IsInArcade = true;
								Function.Call(Hash.PUSH_TIMECYCLE_MODIFIER);
								Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
								Function.Call(Hash.POP_TIMECYCLE_MODIFIER);
								Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "default");
								if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
								{
									Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
								}
								if (ToldPlayerHowToStartHeist == 0)
								{
									ToldPlayerHowToStartHeist = 1;
									Config.SetValue("Misc", "ToldPlayerHowToStartHeist", ToldPlayerHowToStartHeist);
									Config.Save();
									TextNotification("CHAR_LESTER", "~g~ Lester", "Starting Heists", "To Start a Heist you will need to unlock the Underground Area from the Main Chair");
								}
								Function.Call(Hash.SET_RADAR_ZOOM, 5);
								CreatedTunerGear = true;
								Tuner_Gear_Objects.CreateEntites();
								if (ReturnGameBuild() >= 3095)
								{
									ArcadeObjects_v3095.CreateEntites();
									ManageRobberyAnims.AddPropAsLoot((Prop)ArcadeObjects_v3095.Entities[0].Entity);
									ManageRobberyAnims.AddPropAsLoot((Prop)ArcadeObjects_v3095.Entities[1].Entity);
									ManageRobberyAnims.AddPropAsLoot((Prop)ArcadeObjects_v3095.Entities[2].Entity);
								}
								else
								{
									ArcadeObjects_v2944.CreateEntites();
									ManageRobberyAnims.AddPropAsLoot((Prop)ArcadeObjects_v2944.Entities[0].Entity);
									ManageRobberyAnims.AddPropAsLoot((Prop)ArcadeObjects_v2944.Entities[1].Entity);
								}
								CreateMainBlips();
							}
						}
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, ArcadeGarageEnter, "<", 100f))
					{
						Vector3 arcadeGarageEnter = ArcadeGarageEnter;
						arcadeGarageEnter = new Vector3(arcadeGarageEnter.X, arcadeGarageEnter.Y, arcadeGarageEnter.Z + 10f);
						arcadeGarageEnter = new Vector3(arcadeGarageEnter.X, arcadeGarageEnter.Y, World.GetGroundHeight(arcadeGarageEnter));
						World.DrawMarker(MarkerType.Cylinder, arcadeGarageEnter, Vector3.Zero, Vector3.Zero, new Vector3(3f, 3f, 0.45f), Color.Goldenrod);
						if (GetDistance_VDIST2(Game.Player.Character.Position, ArcadeGarageEnter, "<", 2f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Arcade OR ~INPUT_COVER~ To Save a Vehicle");
							if (GetDistance_VDIST2(Game.Player.Character.Position, ArcadeGarageEnter, "<", 2f) && Game.IsControlPressed(Control.Cover) && !GarageMenu.Visible)
							{
								GarageMenu.Visible = true;
							}
							if (Game.IsControlPressed(Control.Context) && GetDistance_VDIST2(Game.Player.Character.Position, ArcadeGarageEnter, "<", 2f))
							{
								RefreshArcadeInterior();
								GTA.UI.Screen.FadeOut(300);
								Script.Wait(300);
								GTA.UI.Screen.FadeIn(300);
								Game.Player.Character.Position = ArcadeGarageExit;
								IsInArcade = true;
								DestoryCars();
								CreateCars("GarageA");
								Function.Call(Hash.SET_RADAR_ZOOM, 5);
								CreatedTunerGear = true;
								Tuner_Gear_Objects.CreateEntites();
								if (ReturnGameBuild() >= 3095)
								{
									ArcadeObjects_v3095.CreateEntites();
									ManageRobberyAnims.AddPropAsLoot((Prop)ArcadeObjects_v3095.Entities[0].Entity);
									ManageRobberyAnims.AddPropAsLoot((Prop)ArcadeObjects_v3095.Entities[1].Entity);
									ManageRobberyAnims.AddPropAsLoot((Prop)ArcadeObjects_v3095.Entities[2].Entity);
								}
								else
								{
									ArcadeObjects_v2944.CreateEntites();
									ManageRobberyAnims.AddPropAsLoot((Prop)ArcadeObjects_v2944.Entities[0].Entity);
									ManageRobberyAnims.AddPropAsLoot((Prop)ArcadeObjects_v2944.Entities[1].Entity);
								}
								CreateMainBlips();
							}
						}
					}
				}
				else if (IsInArcade)
				{
					float z = Game.Player.Character.Position.Z;
					int num4 = 278273;
					if ((double)z < -51.0)
					{
						num4 = 278529;
						foreach (Blip item in MainBlips_U)
						{
							if (item != null)
							{
								item.Alpha = 255;
							}
						}
						foreach (Blip item2 in MainBlips_G)
						{
							if (item2 != null)
							{
								item2.Alpha = 0;
							}
						}
					}
					else
					{
						foreach (Blip item3 in MainBlips_U)
						{
							if (item3 != null)
							{
								item3.Alpha = 0;
							}
						}
						foreach (Blip item4 in MainBlips_G)
						{
							if (item4 != null)
							{
								item4.Alpha = 255;
							}
						}
						num4 = 278273;
					}
					switch (num4)
					{
					case 278529:
					{
						Vector3 aVehPos_ = AVehPos_1;
						DrawSpotlight2(Color.CadetBlue, new Vector3(aVehPos_.X, aVehPos_.Y, aVehPos_.Z + 1.5f), new Vector3(0f, 0f, 0f), 20f, 10f, 1f);
						aVehPos_ = AVehPos_2;
						DrawSpotlight2(Color.CadetBlue, new Vector3(aVehPos_.X, aVehPos_.Y, aVehPos_.Z + 1.5f), new Vector3(0f, 0f, 0f), 20f, 10f, 1f);
						aVehPos_ = AVehPos_3;
						DrawSpotlight2(Color.CadetBlue, new Vector3(aVehPos_.X, aVehPos_.Y, aVehPos_.Z + 1.5f), new Vector3(0f, 0f, 0f), 20f, 10f, 1f);
						aVehPos_ = AVehPos_4;
						DrawSpotlight(Color.CadetBlue, new Vector3(aVehPos_.X, aVehPos_.Y, aVehPos_.Z + 1.5f), new Vector3(0f, 0f, 0f), 20f, 10f, 1f);
						aVehPos_ = AVehPos_5;
						DrawSpotlight(Color.CadetBlue, new Vector3(aVehPos_.X, aVehPos_.Y, aVehPos_.Z + 1.5f), new Vector3(0f, 0f, 0f), 20f, 10f, 1f);
						aVehPos_ = AVehPos_6;
						DrawSpotlight(Color.CadetBlue, new Vector3(aVehPos_.X, aVehPos_.Y, aVehPos_.Z + 1.5f), new Vector3(0f, 0f, 0f), 20f, 10f, 1f);
						aVehPos_ = AVehPos_7;
						DrawSpotlight(Color.CadetBlue, new Vector3(aVehPos_.X, aVehPos_.Y, aVehPos_.Z + 1.5f), new Vector3(0f, 0f, 0f), 20f, 10f, 1f);
						aVehPos_ = AVehPos_8;
						DrawSpotlight(Color.CadetBlue, new Vector3(aVehPos_.X, aVehPos_.Y, aVehPos_.Z + 1.5f), new Vector3(0f, 0f, 0f), 20f, 10f, 1f);
						aVehPos_ = AVehPos_9;
						DrawSpotlight(Color.CadetBlue, new Vector3(aVehPos_.X, aVehPos_.Y, aVehPos_.Z + 1.5f), new Vector3(0f, 0f, 0f), 20f, 10f, 1f);
						aVehPos_ = AVehPos_10;
						DrawSpotlight(Color.CadetBlue, new Vector3(aVehPos_.X, aVehPos_.Y, aVehPos_.Z + 1.5f), new Vector3(0f, 0f, 0f), 20f, 10f, 1f);
						aVehPos_ = AVehPos_11;
						DrawSpotlight(Color.CadetBlue, new Vector3(aVehPos_.X, aVehPos_.Y, aVehPos_.Z + 1.5f), new Vector3(0f, 0f, 0f), 20f, 10f, 1f);
						aVehPos_ = new Vector3(2696.348f, -383.3783f, -54.8f);
						DrawSpotlight(Color.White, new Vector3(aVehPos_.X, aVehPos_.Y, aVehPos_.Z + 1.5f), new Vector3(0f, 180f, 0f), 40f, 10f, 1f);
						aVehPos_ = new Vector3(2717.976f, -391.0815f, -54.8f);
						DrawSpotlight3(Color.CadetBlue, new Vector3(aVehPos_.X, aVehPos_.Y, aVehPos_.Z + 1.5f), new Vector3(180f, 0f, -90f), 120f, 7f, 1f);
						if (GetDistance_VDIST2(Game.Player.Character.Position, HireBodyguardPosition, "<", 100f))
						{
							World.DrawMarker(MarkerType.Cylinder, HireBodyguardPosition, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
							if (GetDistance_VDIST2(Game.Player.Character.Position, HireBodyguardPosition, "<", 1.25f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Hire A Crew");
								if (Game.IsControlJustPressed(Control.Context))
								{
									Game.Player.Character.Position = new Vector3(403f, -999.63f, -98.56f);
									Script.Wait(1);
									Game.Player.Character.Position = new Vector3(403f, -999.63f, -98.56f);
									Game.Player.Character.Rotation = new Vector3(0f, 0f, -73f);
									Function.Call(Hash.SET_FOCUS_POS_AND_VEL, 403f, -999.63f, -98.56, 0, 0, 0);
									BodyguardMenu.mainMenu.Visible = !BodyguardMenu.mainMenu.Visible;
									BodyguardMenu.CameraON = true;
									BodyguardMenu.HeistStartCam = GlobalHelper.CreateCamera(new Vector3(403f, -999.63f, -98.56f), new Vector3(0f, 0f, 0f), 60f);
									BodyguardMenu.HeistStartCam.IsActive = true;
									BodyguardMenu.HeistStartCam.FarClip = 2000f;
									Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
									Game.Player.Character.IsPositionFrozen = true;
									Game.Player.CanControlCharacter = false;
									Game.Player.Character.Opacity = 0;
									World.RenderingCamera = BodyguardMenu.HeistStartCam;
								}
							}
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, GunLockerMarker, "<", 100f))
						{
							World.DrawMarker(MarkerType.Cylinder, GunLockerMarker, Vector3.Zero, Vector3.Zero, new Vector3(1f, 1f, 1f), Color.Goldenrod);
						}
						if (CreatedTunerGear)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, GunLockerMarker, "<", 1.5f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Customize Weapons", 1);
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, GunLockerMarker, "<", 4f))
							{
								HKHModHelperNew.Class1.DisplayGun = true;
								HKHModHelperNew.Class1.GunOffset = new Vector3(0f, 0f, 0f);
								HKHModHelperNew.Class1.DisplayGun = true;
								HKHModHelperNew.Class1.GunHeading = 90f;
								HKHModHelperNew.Class1.GunLockerMarker = new Vector3(2709.43f, -353.99f, -54.33f);
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, GunLockerMarker, "<", 2f) && Game.IsControlJustPressed(Control.Context) && !Gunlocker.weaponsMenu.Visible)
							{
								Gunlocker.weaponsMenu.Visible = !Gunlocker.weaponsMenu.Visible;
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, Tuner_Gear_Objects.Entities[0].Position, "<", 2.5f))
							{
								Vector3 rotation = Function.Call<Vector3>(Hash.GET_GAMEPLAY_CAM_ROT, 0);
								Vector3 vector = Function.Call<Vector3>(Hash.GET_GAMEPLAY_CAM_COORD);
								Vector3 vector2 = RotationToDirection(rotation);
								int num6 = 100;
								Vector3 target = new Vector3(vector.X + vector2.X * (float)num6, vector.Y + vector2.Y * (float)num6, vector.Z + vector2.Z * (float)num6);
								bool flag = true;
								RaycastResult raycastResult = World.Raycast(GameplayCamera.Position, target, IntersectFlags.Everything);
								bool didHit = raycastResult.DidHit;
								foreach (CargoShipHeist.ObjectPlacementMap entity in Tuner_Gear_Objects.Entities)
								{
									if (!(entity.Entity != null))
									{
										continue;
									}
									if (entity.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "w_ar_railgun") && (raycastResult.HitPosition.DistanceTo(entity.Entity.Position) < 0.5f || raycastResult.HitEntity == entity.Entity))
									{
										flag = false;
										DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Pickup 10x Railgun Ammo", 1);
										if (Game.IsControlJustPressed(Control.Context))
										{
											Game.Player.Character.Weapons.Give(WeaponHash.Railgun, 10, true, true);
											Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 1834241177, 10);
										}
									}
									if (entity.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "w_lr_homing") && (raycastResult.HitPosition.DistanceTo(entity.Entity.Position) < 0.5f || raycastResult.HitEntity == entity.Entity))
									{
										flag = false;
										DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Pickup 10x Homing Launcher Ammo", 1);
										if (Game.IsControlJustPressed(Control.Context))
										{
											Game.Player.Character.Weapons.Give(WeaponHash.HomingLauncher, 10, true, true);
											Function.Call(Hash.ADD_AMMO_TO_PED, Game.Player.Character, 1672152130, 10);
										}
									}
									if (entity.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "w_lr_rpg_rocket") && (raycastResult.HitPosition.DistanceTo(entity.Entity.Position) < 0.25f || raycastResult.HitEntity == entity.Entity))
									{
										flag = false;
										DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Pickup 10x RPG Ammo", 1);
										if (Game.IsControlJustPressed(Control.Context))
										{
											Game.Player.Character.Weapons.Give(WeaponHash.RPG, 10, true, true);
										}
									}
									if (entity.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "w_ex_grenadefrag") && (raycastResult.HitPosition.DistanceTo(entity.Entity.Position) < 0.25f || raycastResult.HitEntity == entity.Entity))
									{
										flag = false;
										DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to 10x Grenades", 1);
										if (Game.IsControlJustPressed(Control.Context))
										{
											Game.Player.Character.Weapons.Give(WeaponHash.Grenade, 10, true, true);
										}
									}
									if (entity.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "w_lr_grenadelauncher") && (raycastResult.HitPosition.DistanceTo(entity.Entity.Position) < 0.25f || raycastResult.HitEntity == entity.Entity))
									{
										flag = false;
										DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Pickup Grenade Launcher Ammo", 1);
										if (Game.IsControlJustPressed(Control.Context))
										{
											Game.Player.Character.Weapons.Give(WeaponHash.GrenadeLauncher, 6, true, true);
										}
									}
									if (entity.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "w_ex_pe") && (raycastResult.HitPosition.DistanceTo(entity.Entity.Position) < 0.25f || raycastResult.HitEntity == entity.Entity))
									{
										flag = false;
										DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to 10x Sticky Bombs", 1);
										if (Game.IsControlJustPressed(Control.Context))
										{
											Game.Player.Character.Weapons.Give(WeaponHash.StickyBomb, 10, true, true);
										}
									}
									if (entity.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "prop_ld_health_pack") && (raycastResult.HitPosition.DistanceTo(entity.Entity.Position) < 0.35f || raycastResult.HitEntity == entity.Entity))
									{
										flag = false;
										DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Pickup 200 Health", 1);
										if (Game.IsControlJustPressed(Control.Context))
										{
											Game.Player.Character.Health = 200;
										}
									}
									if (entity.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "prop_bodyarmour_05") && (raycastResult.HitPosition.DistanceTo(entity.Entity.Position) < 0.35f || raycastResult.HitEntity == entity.Entity))
									{
										flag = false;
										DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Pickup 100 Armor", 1);
										if (Game.IsControlJustPressed(Control.Context))
										{
											Game.Player.Character.Armor = 200;
										}
									}
									if (entity.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "hei_p_parachute_s_female") && (raycastResult.HitPosition.DistanceTo(entity.Entity.Position) < 0.15f || raycastResult.HitEntity == entity.Entity))
									{
										flag = false;
										DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Pickup 1x Parachute", 1);
										if (Game.IsControlJustPressed(Control.Context))
										{
											Game.Player.Character.Weapons.Give(WeaponHash.Parachute, 1, true, true);
										}
									}
								}
							}
						}
						if (Game.Player.Character.CurrentVehicle != null)
						{
							if (Game.Player.Character.CurrentVehicle != null)
							{
								Vehicle currentVehicle = Game.Player.Character.CurrentVehicle;
								if (Game.IsControlJustPressed(Control.Cover) && currentVehicle != null)
								{
									currentVehicle.IsPositionFrozen = false;
								}
								if (GarageOwned == 1)
								{
									if (ArcadeVeh1 != null && currentVehicle == ArcadeVeh1)
									{
										currentVehicle.IsPositionFrozen = true;
										currentVehicle.IsEngineRunning = true;
										DisplayHelpTextThisFrameNoSound("Press ~INPUT_CONTEXT~ to use this vehicle or ~INPUT_COVER~ to save this vehicle ");
									}
									if (ArcadeVeh2 != null && currentVehicle == ArcadeVeh2)
									{
										currentVehicle.IsPositionFrozen = true;
										currentVehicle.IsEngineRunning = true;
										DisplayHelpTextThisFrameNoSound("Press ~INPUT_CONTEXT~ to use this vehicle or ~INPUT_COVER~ to save this vehicle ");
									}
									if (ArcadeVeh3 != null && currentVehicle == ArcadeVeh3)
									{
										currentVehicle.IsPositionFrozen = true;
										currentVehicle.IsEngineRunning = true;
										DisplayHelpTextThisFrameNoSound("Press ~INPUT_CONTEXT~ to use this vehicle or ~INPUT_COVER~ to save this vehicle ");
									}
								}
								if (UnlockedGarage == 1)
								{
									if (ArcadeVeh4 != null && currentVehicle == ArcadeVeh4)
									{
										currentVehicle.IsPositionFrozen = true;
										currentVehicle.IsEngineRunning = true;
										DisplayHelpTextThisFrameNoSound("Press ~INPUT_CONTEXT~ to use this vehicle or ~INPUT_COVER~ to save this vehicle ");
									}
									if (ArcadeVeh5 != null && currentVehicle == ArcadeVeh5)
									{
										currentVehicle.IsPositionFrozen = true;
										currentVehicle.IsEngineRunning = true;
										DisplayHelpTextThisFrameNoSound("Press ~INPUT_CONTEXT~ to use this vehicle or ~INPUT_COVER~ to save this vehicle ");
									}
									if (ArcadeVeh6 != null && currentVehicle == ArcadeVeh6)
									{
										currentVehicle.IsPositionFrozen = true;
										currentVehicle.IsEngineRunning = true;
										DisplayHelpTextThisFrameNoSound("Press ~INPUT_CONTEXT~ to use this vehicle or ~INPUT_COVER~ to save this vehicle ");
									}
									if (ArcadeVeh7 != null && currentVehicle == ArcadeVeh7)
									{
										currentVehicle.IsPositionFrozen = true;
										currentVehicle.IsEngineRunning = true;
										DisplayHelpTextThisFrameNoSound("Press ~INPUT_CONTEXT~ to use this vehicle or ~INPUT_COVER~ to save this vehicle ");
									}
									if (ArcadeVeh8 != null && currentVehicle == ArcadeVeh8)
									{
										currentVehicle.IsPositionFrozen = true;
										currentVehicle.IsEngineRunning = true;
										DisplayHelpTextThisFrameNoSound("Press ~INPUT_CONTEXT~ to use this vehicle or ~INPUT_COVER~ to save this vehicle ");
									}
									if (ArcadeVeh9 != null && currentVehicle == ArcadeVeh9)
									{
										currentVehicle.IsPositionFrozen = true;
										currentVehicle.IsEngineRunning = true;
										DisplayHelpTextThisFrameNoSound("Press ~INPUT_CONTEXT~ to use this vehicle or ~INPUT_COVER~ to save this vehicle ");
									}
									if (ArcadeVeh10 != null && currentVehicle == ArcadeVeh10)
									{
										currentVehicle.IsPositionFrozen = true;
										currentVehicle.IsEngineRunning = true;
										DisplayHelpTextThisFrameNoSound("Press ~INPUT_CONTEXT~ to use this vehicle or ~INPUT_COVER~ to save this vehicle ");
									}
									if (ArcadeVeh11 != null && currentVehicle == ArcadeVeh11)
									{
										currentVehicle.IsPositionFrozen = true;
										currentVehicle.IsEngineRunning = true;
										DisplayHelpTextThisFrameNoSound("Press ~INPUT_CONTEXT~ to use this vehicle or ~INPUT_COVER~ to save this vehicle ");
									}
								}
								if (Game.IsControlJustPressed(Control.Cover) && Game.Player.Character.CurrentVehicle != null)
								{
									currentVehicle = Game.Player.Character.CurrentVehicle;
									if (ArcadeVeh1 != null && currentVehicle == ArcadeVeh1)
									{
										SC.LoadIniFile(path + "GarageA//Slot1.ini");
										SC.SaveWithoutDelete();
										Notification.Show(GetHostName() + " : Saved Current vehicle");
									}
									if (ArcadeVeh2 != null && currentVehicle == ArcadeVeh2)
									{
										SC.LoadIniFile(path + "GarageA//Slot2.ini");
										SC.SaveWithoutDelete();
										Notification.Show(GetHostName() + " : Saved Current vehicle");
									}
									if (ArcadeVeh3 != null && currentVehicle == ArcadeVeh3)
									{
										SC.LoadIniFile(path + "GarageA//Slot3.ini");
										SC.SaveWithoutDelete();
										Notification.Show(GetHostName() + " : Saved Current vehicle");
									}
									if (ArcadeVeh4 != null && currentVehicle == ArcadeVeh4)
									{
										SC.LoadIniFile(path + "GarageA//Slot4.ini");
										SC.SaveWithoutDelete();
										Notification.Show(GetHostName() + " : Saved Current vehicle");
									}
									if (ArcadeVeh5 != null && currentVehicle == ArcadeVeh5)
									{
										SC.LoadIniFile(path + "GarageA//Slot5.ini");
										SC.SaveWithoutDelete();
										Notification.Show(GetHostName() + " : Saved Current vehicle");
									}
									if (ArcadeVeh6 != null && currentVehicle == ArcadeVeh6)
									{
										SC.LoadIniFile(path + "GarageA//Slot6.ini");
										SC.SaveWithoutDelete();
										Notification.Show(GetHostName() + " : Saved Current vehicle");
									}
									if (ArcadeVeh7 != null && currentVehicle == ArcadeVeh7)
									{
										SC.LoadIniFile(path + "GarageA//Slot7.ini");
										SC.SaveWithoutDelete();
										Notification.Show(GetHostName() + " : Saved Current vehicle");
									}
									if (ArcadeVeh8 != null && currentVehicle == ArcadeVeh8)
									{
										SC.LoadIniFile(path + "GarageA//Slot8.ini");
										SC.SaveWithoutDelete();
										Notification.Show(GetHostName() + " : Saved Current vehicle");
									}
									if (ArcadeVeh9 != null && currentVehicle == ArcadeVeh9)
									{
										SC.LoadIniFile(path + "GarageA//Slot9.ini");
										SC.SaveWithoutDelete();
										Notification.Show(GetHostName() + " : Saved Current vehicle");
									}
									if (ArcadeVeh10 != null && currentVehicle == ArcadeVeh10)
									{
										SC.LoadIniFile(path + "GarageA//Slot10.ini");
										SC.SaveWithoutDelete();
										Notification.Show(GetHostName() + " : Saved Current vehicle");
									}
									if (ArcadeVeh11 != null && currentVehicle == ArcadeVeh11)
									{
										SC.LoadIniFile(path + "GarageA//Slot11.ini");
										SC.SaveWithoutDelete();
										Notification.Show(GetHostName() + " : Saved Current vehicle");
									}
								}
								if (Game.IsControlJustPressed(Control.Context) && Game.Player.Character.CurrentVehicle != null)
								{
									currentVehicle = Game.Player.Character.CurrentVehicle;
									if (ArcadeVeh1 != null && currentVehicle == ArcadeVeh1)
									{
										ArcadeVeh1 = null;
										LoadCarinToRealWorld(ArcadeVeh1);
										Script.Wait(500);
									}
									if (ArcadeVeh2 != null && currentVehicle == ArcadeVeh2)
									{
										ArcadeVeh2 = null;
										LoadCarinToRealWorld(ArcadeVeh2);
										Script.Wait(500);
									}
									if (ArcadeVeh3 != null && currentVehicle == ArcadeVeh3)
									{
										ArcadeVeh3 = null;
										LoadCarinToRealWorld(ArcadeVeh3);
										Script.Wait(500);
									}
									if (UnlockedGarage == 1)
									{
										if (ArcadeVeh4 != null && currentVehicle == ArcadeVeh4)
										{
											ArcadeVeh4 = null;
											LoadCarinToRealWorld(ArcadeVeh4);
											Script.Wait(500);
										}
										if (ArcadeVeh5 != null && currentVehicle == ArcadeVeh5)
										{
											ArcadeVeh5 = null;
											LoadCarinToRealWorld(ArcadeVeh5);
											Script.Wait(500);
										}
										if (ArcadeVeh6 != null && currentVehicle == ArcadeVeh6)
										{
											ArcadeVeh6 = null;
											LoadCarinToRealWorld(ArcadeVeh6);
											Script.Wait(500);
										}
										if (ArcadeVeh7 != null && currentVehicle == ArcadeVeh7)
										{
											ArcadeVeh7 = null;
											LoadCarinToRealWorld(ArcadeVeh7);
											Script.Wait(500);
										}
										if (ArcadeVeh8 != null && currentVehicle == ArcadeVeh8)
										{
											ArcadeVeh8 = null;
											LoadCarinToRealWorld(ArcadeVeh8);
											Script.Wait(500);
										}
										if (ArcadeVeh9 != null && currentVehicle == ArcadeVeh9)
										{
											ArcadeVeh9 = null;
											LoadCarinToRealWorld(ArcadeVeh9);
											Script.Wait(500);
										}
										if (ArcadeVeh10 != null && currentVehicle == ArcadeVeh10)
										{
											ArcadeVeh10 = null;
											LoadCarinToRealWorld(ArcadeVeh10);
											Script.Wait(500);
										}
										if (ArcadeVeh11 != null && currentVehicle == ArcadeVeh11)
										{
											ArcadeVeh11 = null;
											LoadCarinToRealWorld(ArcadeVeh11);
											Script.Wait(500);
										}
									}
								}
							}
							Vehicle currentVehicle2 = Game.Player.Character.CurrentVehicle;
							if (currentVehicle2 != ArcadeVeh1 && currentVehicle2 != ArcadeVeh2 && currentVehicle2 != ArcadeVeh3 && currentVehicle2 != ArcadeVeh4 && currentVehicle2 != ArcadeVeh5 && currentVehicle2 != ArcadeVeh6 && currentVehicle2 != ArcadeVeh7 && currentVehicle2 != ArcadeVeh8 && currentVehicle2 != ArcadeVeh9 && currentVehicle2 != ArcadeVeh10 && currentVehicle2 != ArcadeVeh11 && GetDistance_VDIST2(Game.Player.Character.CurrentVehicle.Position, GetawayCarArcadeSpawn, "<", 10f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to exit Arcade with " + Game.Player.Character.CurrentVehicle.LocalizedName);
								if (Game.IsControlJustPressed(Control.Context))
								{
									GTA.UI.Screen.FadeOut(1500);
									Script.Wait(1500);
									IsInArcade = false;
									RadioOn = false;
									DestoryCars();
									Game.Player.Character.CurrentVehicle.Position = ArcadeGarageEnter;
									Game.Player.Character.CurrentVehicle.Heading = ArcadeGarageEnterHeading;
									Script.Wait(1500);
									GTA.UI.Screen.FadeIn(1500);
									Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
									Function.Call(Hash.POP_TIMECYCLE_MODIFIER);
									Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "default");
								}
							}
						}
						if (Safe == null)
						{
							Safe = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2700.826f, -367.2581f, -55.8f, 10f, 1098122770, 0, 0, 0);
						}
						if (Safe != null && !FinishedSafeAnimations && GetDistance_VDIST2(Game.Player.Character.Position, Safe.Position, "<", 3f))
						{
							if (!ShowingSafeScaleform)
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Crack Safe");
							}
							if (ShowingSafeScaleform)
							{
								ShowSafeButtons();
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Quit, ~INPUT_ATTACK~ to Check Code");
							}
							if (Game.IsControlJustPressed(Control.Context))
							{
								if (ShowingSafeScaleform)
								{
									Random random = new Random();
									CodeNumber1 = random.Next(1, 99);
									CodeNumber2 = random.Next(1, 99);
									CodeNumber3 = random.Next(1, 99);
									PlayingSafeFinalAnim = false;
									PlayingSafeFinalAnim = false;
									GotIncorrectSafeCode = false;
									GotCorrectSafeCode = false;
									PlayingIdle = false;
									ReadyForCode = false;
									SelectedNum = 0;
									InputedNumber1 = 0f;
									InputedNumber2 = 0f;
									InputedNumber3 = 0f;
									ShowingSafeScaleform = false;
									Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
									if (SafeValuables != null)
									{
										SafeValuables.Delete();
									}
									string dict2 = "anim@scripted@heist@ig15_safe_crack@male@";
									LoadDict(dict2);
									Vector3 rotation2 = Safe.Rotation;
									Vector3 offsetPosition4 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
									SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition4.X, offsetPosition4.Y, offsetPosition4.Z, 0.0, 0.0, rotation2.Z, 2);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition4.X, offsetPosition4.Y, offsetPosition4.Z, 0.0, 0.0, rotation2.Z, true);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "exit_safe", LoadDict(dict2), 1000.0, -8f, -8f, rotation2.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "exit_bag", LoadDict(dict2), 1000.0, -8f, -8f, rotation2.Z + 180f, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict2), "exit_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "exit_player", LoadDict(dict2), 1000.0, -8f, -8f, rotation2.Z + 180f, 1148846080, 0);
									Script.Wait(1000);
									Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
									Ped character = Game.Player.Character;
									if (Game.Player.Character.Model == PedHash.Trevor)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 1, 0, 1);
									}
									if (Game.Player.Character.Model == PedHash.Michael)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 2, 0, 1);
									}
									if (Game.Player.Character.Model == PedHash.Franklin)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 6, 0, 1);
									}
									Game.Player.Character.Task.ClearAllImmediately();
									if (bagProp != null)
									{
										bagProp.Delete();
										bagProp.MarkAsNoLongerNeeded();
										bagProp = null;
									}
								}
								else if (!ShowingSafeScaleform)
								{
									Random random2 = new Random();
									CodeNumber1 = random2.Next(1, 99);
									CodeNumber2 = random2.Next(1, 99);
									CodeNumber3 = random2.Next(1, 99);
									PlayingSafeFinalAnim = false;
									GotIncorrectSafeCode = false;
									if (SafeValuables != null)
									{
										SafeValuables.Delete();
									}
									PlayingIdle = false;
									if (bagProp != null)
									{
										bagProp.Delete();
										bagProp.MarkAsNoLongerNeeded();
										bagProp = null;
									}
									bagProp = GlobalHelper.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
									bagProp.IsCollisionEnabled = false;
									bagProp.IsInvincible = true;
									ReadyForCode = false;
									SelectedNum = 0;
									InputedNumber1 = 0f;
									InputedNumber2 = 0f;
									InputedNumber3 = 0f;
									ShowingSafeScaleform = true;
									string dict3 = "anim@scripted@heist@ig15_safe_crack@male@";
									LoadDict(dict3);
									Vector3 rotation3 = Safe.Rotation;
									Vector3 offsetPosition5 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
									SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition5.X, offsetPosition5.Y, offsetPosition5.Z, 0.0, 0.0, rotation3.Z, 2);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition5.X, offsetPosition5.Y, offsetPosition5.Z, 0.0, 0.0, rotation3.Z, true);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "enter_safe", LoadDict(dict3), 1000.0, -8f, -8f, rotation3.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "enter_bag", LoadDict(dict3), 1000.0, -8f, -8f, rotation3.Z + 180f, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict3), "enter_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "enter_player", LoadDict(dict3), 1000.0, -8f, -8f, rotation3.Z + 180f, 1148846080, 0);
									TextNotification("CHAR_LESTER", "~g~ Lester", "Safe Code", "Remember Safe Code is ~b~" + CodeNumber1 + "," + CodeNumber2 + "," + CodeNumber3);
								}
							}
						}
						if (ShowingSafeScaleform && !FinishedSafeAnimations)
						{
							Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
							bool flag2 = Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "safe_01a");
							bool flag3 = Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_LINKED, 1098122770);
							if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SafeScene1) >= 1065353216 && !ReadyForCode)
							{
								ReadyForCode = true;
							}
							if (func_4438() == 1)
							{
								if (ReadyForCode)
								{
									if (Game.IsControlJustPressed(Control.Attack))
									{
										if (CodeNumber1 == (int)InputedNumber1 && CodeNumber2 == (int)InputedNumber2 && CodeNumber3 == (int)InputedNumber3)
										{
											if (SafeValuables != null)
											{
												SafeValuables.Delete();
											}
											SafeValuables = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_file_cylinder_01a"), Safe.Position, new Vector3(0f, 0f, Safe.Heading), false, false);
											GotIncorrectSafeCode = false;
											GotCorrectSafeCode = true;
											PlayingIdle = false;
											string dict4 = "anim@scripted@heist@ig15_safe_crack@male@";
											LoadDict(dict4);
											Vector3 rotation4 = Safe.Rotation;
											Vector3 offsetPosition6 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
											SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition6.X, offsetPosition6.Y, offsetPosition6.Z, 0.0, 0.0, rotation4.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition6.X, offsetPosition6.Y, offsetPosition6.Z, 0.0, 0.0, rotation4.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "door_open_safe", LoadDict(dict4), 1000.0, -8f, -8f, rotation4.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "door_open_bag", LoadDict(dict4), 1000.0, -8f, -8f, rotation4.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, SafeValuables, SafeScene1, "door_open_file_cylinder", LoadDict(dict4), 1000.0, -8f, -8f, rotation4.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict4), "door_open_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "door_open_player", LoadDict(dict4), 1000.0, -8f, -8f, rotation4.Z + 180f, 1148846080, 0);
										}
										else
										{
											GotIncorrectSafeCode = true;
											GotCorrectSafeCode = false;
											PlayingIdle = false;
											string dict5 = "anim@scripted@heist@ig15_safe_crack@male@";
											LoadDict(dict5);
											Vector3 rotation5 = Safe.Rotation;
											Vector3 offsetPosition7 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
											SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition7.X, offsetPosition7.Y, offsetPosition7.Z, 0.0, 0.0, rotation5.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition7.X, offsetPosition7.Y, offsetPosition7.Z, 0.0, 0.0, rotation5.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "fail_safe", LoadDict(dict5), 1000.0, -8f, -8f, rotation5.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "fail_bag", LoadDict(dict5), 1000.0, -8f, -8f, rotation5.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict5), "fail_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "fail_player", LoadDict(dict5), 1000.0, -8f, -8f, rotation5.Z + 180f, 1148846080, 0);
										}
									}
									if (PlayingSafeFinalAnim && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SafeScene1) >= 1065353216)
									{
										Function.Call<int>(Hash.SET_SYNCHRONIZED_SCENE_PHASE, SafeScene1, 0f);
										FinishedSafeAnimations = true;
										GotIncorrectSafeCode = false;
										if (SafeValuables != null)
										{
											SafeValuables.Delete();
										}
										if (bagProp != null)
										{
											bagProp.Delete();
											bagProp.MarkAsNoLongerNeeded();
											bagProp = null;
										}
										GotCorrectSafeCode = false;
										PlayingIdle = false;
										ReadyForCode = false;
										SelectedNum = 0;
										InputedNumber1 = 0f;
										InputedNumber2 = 0f;
										InputedNumber3 = 0f;
										ShowingSafeScaleform = false;
										Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
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
										Game.Player.Character.Task.ClearAllImmediately();
									}
									if (GotCorrectSafeCode && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SafeScene1) >= 1065353216)
									{
										GotCorrectSafeCode = false;
										PlayingSafeFinalAnim = true;
										PlayingIdle = false;
										string dict6 = "anim@scripted@heist@ig15_safe_crack@male@";
										LoadDict(dict6);
										Vector3 rotation6 = Safe.Rotation;
										Vector3 offsetPosition8 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
										SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition8.X, offsetPosition8.Y, offsetPosition8.Z, 0.0, 0.0, rotation6.Z, 2);
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition8.X, offsetPosition8.Y, offsetPosition8.Z, 0.0, 0.0, rotation6.Z, true);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "success_with_metal_cylinder_safe", LoadDict(dict6), 1000.0, -8f, -8f, rotation6.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "success_with_metal_cylinder_bag", LoadDict(dict6), 1000.0, -8f, -8f, rotation6.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, SafeValuables, SafeScene1, "success_with_metal_cylinder_file_cylinder", LoadDict(dict6), 1000.0, -8f, -8f, rotation6.Z + 180f, 1148846080, 0);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict6), "success_with_metal_cylinder_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "success_with_metal_cylinder_player", LoadDict(dict6), 1000.0, -8f, -8f, rotation6.Z + 180f, 1148846080, 0);
									}
									if (GotIncorrectSafeCode && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SafeScene1) >= 1065353216)
									{
										GotCorrectSafeCode = false;
										GotIncorrectSafeCode = false;
									}
									if (!GotCorrectSafeCode && !GotIncorrectSafeCode && !PlayingSafeFinalAnim && !Game.IsControlPressed(Control.PhoneUp) && !Game.IsControlPressed(Control.PhoneDown) && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SafeScene1) >= 1065353216)
									{
										PlayingIdle = true;
										string dict7 = "anim@scripted@heist@ig15_safe_crack@male@";
										LoadDict(dict7);
										Vector3 rotation7 = Safe.Rotation;
										Vector3 offsetPosition9 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
										SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition9.X, offsetPosition9.Y, offsetPosition9.Z, 0.0, 0.0, rotation7.Z, 2);
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition9.X, offsetPosition9.Y, offsetPosition9.Z, 0.0, 0.0, rotation7.Z, true);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "idle_safe", LoadDict(dict7), 1000.0, -8f, -8f, rotation7.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "idle_bag", LoadDict(dict7), 1000.0, -8f, -8f, rotation7.Z + 180f, 1148846080, 0);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict7), "idle_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "idle_player", LoadDict(dict7), 1000.0, -8f, -8f, rotation7.Z + 180f, 1148846080, 0);
									}
									if (Game.IsControlPressed(Control.PhoneUp))
									{
										Game.DisableControlThisFrame(Control.PhoneUp);
										if (SelectedNum == 0)
										{
											InputedNumber1 += 0.25f;
											if (InputedNumber1 > 99f)
											{
												InputedNumber1 = 0f;
											}
										}
										if (SelectedNum == 1)
										{
											InputedNumber2 += 0.25f;
											if (InputedNumber2 > 99f)
											{
												InputedNumber2 = 0f;
											}
										}
										if (SelectedNum == 2)
										{
											InputedNumber3 += 0.25f;
											if (InputedNumber3 > 99f)
											{
												InputedNumber3 = 0f;
											}
										}
										if (!PlayingIdle && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SafeScene1) >= 1065353216)
										{
											string dict8 = "anim@scripted@heist@ig15_safe_crack@male@";
											LoadDict(dict8);
											Vector3 rotation8 = Safe.Rotation;
											Vector3 offsetPosition10 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
											SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition10.X, offsetPosition10.Y, offsetPosition10.Z, 0.0, 0.0, rotation8.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition10.X, offsetPosition10.Y, offsetPosition10.Z, 0.0, 0.0, rotation8.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "input_safe_code_left_safe", LoadDict(dict8), 1000.0, -8f, -8f, rotation8.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "input_safe_code_left_bag", LoadDict(dict8), 1000.0, -8f, -8f, rotation8.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict8), "input_safe_code_left_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "input_safe_code_left_player", LoadDict(dict8), 1000.0, -8f, -8f, rotation8.Z + 180f, 1148846080, 0);
										}
										if (PlayingIdle)
										{
											PlayingIdle = false;
											string dict9 = "anim@scripted@heist@ig15_safe_crack@male@";
											LoadDict(dict9);
											Vector3 rotation9 = Safe.Rotation;
											Vector3 offsetPosition11 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
											SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition11.X, offsetPosition11.Y, offsetPosition11.Z, 0.0, 0.0, rotation9.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition11.X, offsetPosition11.Y, offsetPosition11.Z, 0.0, 0.0, rotation9.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "input_safe_code_left_safe", LoadDict(dict9), 1000.0, -8f, -8f, rotation9.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "input_safe_code_left_bag", LoadDict(dict9), 1000.0, -8f, -8f, rotation9.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict9), "input_safe_code_left_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "input_safe_code_left_player", LoadDict(dict9), 1000.0, -8f, -8f, rotation9.Z + 180f, 1148846080, 0);
										}
									}
									if (Game.IsControlPressed(Control.PhoneDown))
									{
										Game.DisableControlThisFrame(Control.PhoneDown);
										if (SelectedNum == 0)
										{
											InputedNumber1 -= 0.25f;
											if (InputedNumber1 < 0f)
											{
												InputedNumber1 = 99f;
											}
										}
										if (SelectedNum == 1)
										{
											InputedNumber2 -= 0.25f;
											if (InputedNumber2 < 0f)
											{
												InputedNumber2 = 99f;
											}
										}
										if (SelectedNum == 2)
										{
											InputedNumber3 -= 0.25f;
											if (InputedNumber3 < 0f)
											{
												InputedNumber3 = 99f;
											}
										}
										if (!PlayingIdle && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SafeScene1) >= 1065353216)
										{
											string dict10 = "anim@scripted@heist@ig15_safe_crack@male@";
											LoadDict(dict10);
											Vector3 rotation10 = Safe.Rotation;
											Vector3 offsetPosition12 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
											SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition12.X, offsetPosition12.Y, offsetPosition12.Z, 0.0, 0.0, rotation10.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition12.X, offsetPosition12.Y, offsetPosition12.Z, 0.0, 0.0, rotation10.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "input_safe_code_right_safe", LoadDict(dict10), 1000.0, -8f, -8f, rotation10.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "input_safe_code_right_bag", LoadDict(dict10), 1000.0, -8f, -8f, rotation10.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict10), "input_safe_code_right_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "input_safe_code_right_player", LoadDict(dict10), 1000.0, -8f, -8f, rotation10.Z + 180f, 1148846080, 0);
										}
										if (PlayingIdle)
										{
											PlayingIdle = false;
											string dict11 = "anim@scripted@heist@ig15_safe_crack@male@";
											LoadDict(dict11);
											Vector3 rotation11 = Safe.Rotation;
											Vector3 offsetPosition13 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
											SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition13.X, offsetPosition13.Y, offsetPosition13.Z, 0.0, 0.0, rotation11.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition13.X, offsetPosition13.Y, offsetPosition13.Z, 0.0, 0.0, rotation11.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "input_safe_code_right_safe", LoadDict(dict11), 1000.0, -8f, -8f, rotation11.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "input_safe_code_right_bag", LoadDict(dict11), 1000.0, -8f, -8f, rotation11.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict11), "input_safe_code_right_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "input_safe_code_right_player", LoadDict(dict11), 1000.0, -8f, -8f, rotation11.Z + 180f, 1148846080, 0);
										}
									}
									if (Game.IsControlJustPressed(Control.PhoneLeft))
									{
										Game.DisableControlThisFrame(Control.PhoneLeft);
										SelectedNum--;
										if (SelectedNum < 0)
										{
											SelectedNum = 2;
										}
									}
									if (Game.IsControlJustPressed(Control.PhoneRight))
									{
										Game.DisableControlThisFrame(Control.PhoneRight);
										SelectedNum++;
										if (SelectedNum >= 3)
										{
											SelectedNum = 0;
										}
									}
								}
								func_4436(1098122770);
							}
						}
						if (!GetDistance_VDIST2(Game.Player.Character.Position, MenuMarker, "<", 50f))
						{
							break;
						}
						World.DrawMarker(MarkerType.Cylinder, MenuMarker, Vector3.Zero, Vector3.Zero, new Vector3(1f, 1f, 1f), Color.Goldenrod);
						if (GetDistance_VDIST2(Game.Player.Character.Position, MenuMarker, "<", 3f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to open the Heists Menu");
							if (LesterHasToldPlayerOfNewHeists != 0 && Game.IsControlJustPressed(Control.Context) && !Heists.Visible && !modLemonUIObjectPool.AreAnyVisible)
							{
								Heists.Visible = !Heists.Visible;
							}
						}
						break;
					}
					case 278273:
						if (GetDistance_VDIST2(Game.Player.Character.Position, ArcadeBusinessExit, "<", 100f))
						{
							World.DrawMarker(MarkerType.Cylinder, ArcadeBusinessExit, Vector3.Zero, Vector3.Zero, new Vector3(1f, 1f, 0.45f), Color.Goldenrod);
							if (GetDistance(Game.Player.Character.Position, ArcadeBusinessExit) < 2f)
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit Arcade");
								if (GetDistance(Game.Player.Character.Position, ArcadeBusinessExit) < 2f && Game.IsControlPressed(Control.Context))
								{
									DestoryCars();
									GTA.UI.Screen.FadeOut(300);
									Script.Wait(300);
									GTA.UI.Screen.FadeIn(300);
									Game.Player.Character.Position = ArcadeBusinessEnter;
									IsInArcade = false;
									Script.Wait(10);
									Function.Call(Hash.PUSH_TIMECYCLE_MODIFIER);
									Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
									Function.Call(Hash.POP_TIMECYCLE_MODIFIER);
									Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "default");
									if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
									{
										Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
									}
									Function.Call(Hash.SET_RADAR_ZOOM, 850);
									foreach (Blip item5 in MainBlips_U)
									{
										if (item5 != null)
										{
											item5.Delete();
										}
									}
									foreach (Blip item6 in MainBlips_G)
									{
										if (item6 != null)
										{
											item6.Delete();
										}
									}
									CreatedTunerGear = false;
									Tuner_Gear_Objects.DeleteEntites();
									if (ReturnGameBuild() >= 3095)
									{
										ArcadeObjects_v3095.DeleteEntites();
									}
									else
									{
										ArcadeObjects_v2944.DeleteEntites();
									}
								}
							}
						}
						if (IsDrinking && Champaine != null)
						{
							if (Effect == 6)
							{
								Game.Player.Character.Health += 10;
							}
							if (Effect == 5)
							{
								Game.Player.Character.Health += 5;
							}
							if (Effect == 4)
							{
								Game.Player.Character.Health += 2;
							}
							if (Effect == 3 && Game.Player.Character.Health > 10)
							{
								Game.Player.Character.Health--;
							}
							if (Effect == 2)
							{
								Game.Player.Character.HasGravity = false;
							}
							int effect = Effect;
							int num13 = 1;
							int effect2 = Effect;
							int effect3 = Effect;
							int num14 = -1;
							DrinkTimer++;
							int drinkTimer = DrinkTimer;
							int num15 = 100;
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, WardrobePos, "<", 50f))
						{
							World.DrawMarker(MarkerType.Cylinder, WardrobePos, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
							if (GetDistance_VDIST2(Game.Player.Character.Position, WardrobePos, "<", 1.25f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to open the Wardrobe");
								if (Game.IsControlJustPressed(Control.Context) && !WoredrobeMenu.Visible)
								{
									WoredrobeMenu.Visible = true;
								}
							}
						}
						if (!GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2726.6f, -371.8277f, -48.4f), "<", 20f))
						{
							break;
						}
						if (UsingHiddenDoor == 1 && UndergrondAreaLocked == 1)
						{
							if (HiddenDoorOpen)
							{
								if (HiddenDoorOpen_WaitForPlayerClose)
								{
									HiddenDoorOpen_WaitForPlayerClose = false;
									HiddenDoorOpen_WaitForPlayerFar = true;
									if (!UndergroundDoor.IsOnScreen)
									{
										if (UndergroundDoor != null)
										{
											UndergroundDoor.Delete();
										}
										UndergroundDoor = GlobalHelper.CreateProp(RequestModel("ch_prop_arcade_fortune_door_01a"), new Vector3(2727.911f, -371.9659f, -48.39819f), new Vector3(0f, 0f, 80f), false, false);
										UndergroundDoor.IsPositionFrozen = true;
									}
								}
								if (!HiddenDoorOpen_WaitForPlayerClose && !HiddenDoorOpen_WaitForPlayerFar && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
								{
									Game.Player.Character.Task.ClearAll();
									HiddenDoorOpen_WaitForPlayerFar = false;
									HiddenDoorOpen_WaitForPlayerClose = true;
								}
								bool hiddenDoorOpen_WaitForPlayerFar = HiddenDoorOpen_WaitForPlayerFar;
							}
							if (UndergroundDoor != null)
							{
								Vector3 offsetPosition = UndergroundDoor.GetOffsetPosition(new Vector3(-0.6f, 2.2f, 1f));
								if (GetDistance_VDIST2(Game.Player.Character.Position, UndergroundDoor.Position, ">", 3f) && GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition, ">", 3f) && UndergroundDoor != null)
								{
									if (HiddenDoorOpen && UndergroundDoor.Heading > 358f)
									{
										HiddenDoorOpen = false;
										UndergroundDoor.Heading = 1f;
									}
									if (UndergroundDoor.Heading > 0f && UndergroundDoor.Heading < 81f)
									{
										UndergroundDoor.Rotation -= new Vector3(0f, 0f, 0.4f);
									}
								}
								if (HiddenDoorOpen && GetDistance_VDIST2(Game.Player.Character.Position, UndergroundDoor.Position, "<", 3f) && UndergroundDoor != null && UndergroundDoor.Heading < 81f)
								{
									if (UndergroundDoor.Heading >= 80f)
									{
										UndergroundDoor.Heading = 80f;
									}
									UndergroundDoor.Rotation += new Vector3(0f, 0f, 0.4f);
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition, "<", 3f) && UndergroundDoor != null)
								{
									if (UndergroundDoor.Heading > 358f)
									{
										HiddenDoorOpen = false;
										UndergroundDoor.Heading = 1f;
									}
									if (UndergroundDoor.Heading < 81f)
									{
										if (UndergroundDoor.Heading >= 80f)
										{
											UndergroundDoor.Heading = 80f;
										}
										UndergroundDoor.Rotation += new Vector3(0f, 0f, 0.4f);
									}
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, UndergroundDoor.Position, "<", 4f) && !HiddenDoorOpen && (UndergroundDoor.Heading > 358f || UndergroundDoor.Heading < 1f))
								{
									DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to access the basement");
									if (Game.IsControlJustPressed(Control.Context))
									{
										ManageCellphoneCall.DialogueLines.Clear();
										ManageCellphoneCall.DialogueLines.Add("HS3LE_FWAA");
										ManageCellphoneCall.Speaker = "LESTER_FORTUNE";
										ManageCellphoneCall.StartVoiceOver();
										UndergroundDoor.Heading = 0f;
										Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
										HiddenDoorOpen_WaitForPlayerFar = false;
										HiddenDoorOpen_WaitForPlayerClose = false;
										string dict = "anim_heist@hs3f@ig13_fortune_teller_door@male@";
										LoadDict(dict);
										LoadDict(dict);
										Vector3 offsetPosition2 = UndergroundDoor.GetOffsetPosition(new Vector3(-0.6f, -2.2f, 1f));
										Vector3 offsetPosition3 = UndergroundDoor.GetOffsetPosition(new Vector3(0f, 0f, 0f));
										Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition2.X, offsetPosition2.Y, offsetPosition2.Z, 0.0, 0.0, UndergroundDoor.Rotation.Z, 2);
										int num5 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition3.X, offsetPosition3.Y, offsetPosition3.Z, 0.0, 0.0, UndergroundDoor.Rotation.Z, 2);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict), "coin_drop", 3000.0, -2.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "coin_drop", LoadDict(dict), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z - 180f, 1148846080, 0);
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
										Script.Wait(3500);
										HiddenDoorOpen = true;
									}
								}
							}
						}
						if (UsingHiddenDoor == 1 && !UndergrondAreaLockedbool2 && !UndergrondAreaLockedbool)
						{
							UndergrondAreaLockedbool2 = true;
							UndergrondAreaLockedbool = false;
							if (UndergroundDoor != null)
							{
								UndergroundDoor.Delete();
							}
							UndergroundDoor = GlobalHelper.CreateProp(RequestModel("ch_prop_arcade_fortune_door_01a"), new Vector3(2727.911f, -371.9659f, -48.39819f), new Vector3(0f, 0f, 0f), false, false);
							UndergroundDoor.IsPositionFrozen = true;
						}
						if (UndergrondAreaLocked == 1 && UsingHiddenDoor != 1)
						{
							if (!UndergrondAreaLockedbool2 && !UndergrondAreaLockedbool)
							{
								UndergrondAreaLockedbool2 = true;
								UndergrondAreaLockedbool = false;
								if (UndergroundDoor != null)
								{
									UndergroundDoor.Delete();
								}
								if (!UndergrondAreaLockedbool)
								{
									UndergrondAreaLockedbool = true;
									UndergroundDoor = GlobalHelper.CreateProp(RequestModel("ch_prop_mesa_door_01a"), new Vector3(2726.6f, -371.8277f, -48.4f), new Vector3(0f, 0f, -125f), false, false);
									UndergroundDoor.IsPositionFrozen = true;
								}
							}
							if (UndergrondAreaLockedbool)
							{
								UndergrondAreaLockedbool = false;
								if (UndergroundDoor != null)
								{
									UndergroundDoor.Delete();
								}
								if (!UndergrondAreaLockedbool)
								{
									UndergrondAreaLockedbool = true;
									UndergroundDoor = GlobalHelper.CreateProp(RequestModel("ch_prop_mesa_door_01a"), new Vector3(2726.6f, -371.8277f, -48.4f), new Vector3(0f, 0f, -125f), false, false);
									UndergroundDoor.IsPositionFrozen = true;
								}
							}
						}
						if (UndergrondAreaLocked == 0 && UsingHiddenDoor == 0 && !UndergrondAreaLockedbool)
						{
							UndergrondAreaLockedbool2 = false;
							UndergrondAreaLockedbool = true;
							UndergroundDoor = GlobalHelper.CreateProp(RequestModel("ch_prop_mesa_door_01a"), new Vector3(2726.6f, -371.8277f, -48.4f), new Vector3(0f, 0f, 0f), false, false);
							UndergroundDoor.IsPositionFrozen = true;
						}
						break;
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2681.6f, -364.8f, -56.1f), "<", 20f) || GetDistance_VDIST2(Game.Player.Character.Position, MenuMarker, "<", 20f))
					{
						Vector3 position2 = Game.Player.Character.Position;
						Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position2.X, position2.Y, position2.Z, 30f, 1237817995, 0, 0, 0);
						if (prop != null && ArcadeBoard != null)
						{
							ArcadeBoard.Render3D(prop.GetOffsetPosition(new Vector3(0f, -0.055f, 0f)), new Vector3(0f, 0f, prop.Rotation.Z), new Vector3(1.9f, 1.7f, 1.7f));
						}
						prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position2.X, position2.Y, position2.Z, 30f, 170998764, 0, 0, 0);
						if (prop != null && finaleBoardset != null)
						{
							finaleBoardset.Render3D(prop.GetOffsetPosition(new Vector3(0f, -0.055f, 0f)), new Vector3(0f, 0f, prop.Rotation.Z - 180f), new Vector3(2f, 1.7f, 1.7f));
						}
						prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position2.X, position2.Y, position2.Z, 30f, -1906424756, 0, 0, 0);
						if (prop != null && ArcadeBoardset != null)
						{
							ArcadeBoardset.Render3D(prop.GetOffsetPosition(new Vector3(0f, -0.055f, 0f)), new Vector3(0f, 0f, prop.Rotation.Z), new Vector3(1.9f, 1.7f, 1.7f));
						}
						prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2690.77832f, -364.818817f, -56.1856537f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_planning_board_01a"), 0, 0, 0);
						if (prop != null)
						{
							if (PrepBoard1 == null)
							{
								PrepBoard1 = new Scaleform("HEIST_MP");
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2690.77832f, -364.818817f, -56.185f), "<", 3.5f))
							{
								if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
								{
									DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Start ~t~" + Heists_Data[CurrentHeistData].HeistName);
									if (Game.IsControlJustPressed(Control.Context))
									{
										if (Heists_Data[CurrentHeistData].Heist == HeistID.The247Job)
										{
											The247Job.StartHeist();
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.TheSilthouseJob)
										{
											TheSilthouseJob.StartHeist();
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.Fleeca)
										{
											Class1.RunFleeca = 1;
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.Paleto)
										{
											Class1.RunPaleto = 1;
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.PacificStandard)
										{
											Class1.RunPacificStandard = 1;
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.JewelStore)
										{
											Class1.RunJewelryStore = 1;
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.UnionDepository)
										{
											Class1.RunUnionDepositoryHeist = 1;
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.LaFuenteBlanca)
										{
											Class1.RunLaFauenteBlanca = 1;
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.LaoubPrincessCargoshipHeist)
										{
											CargoShipHeist.StartHeist();
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.BikerClubhouse)
										{
											Class1.RunBikerClubhouseHeist = 1;
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.TheDiamondCasino)
										{
											TheDiamondHeist.RunHeist = 1;
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.CayoPericoHeist)
										{
											CayoPericoHeist.RunHeist = 1;
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.ThePrisonBreak)
										{
											ThePrisonBreak.RunHeist = 1;
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.TheHumaineLabs)
										{
											HumainLabsOnline.RunHeist = 1;
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.TheLuxigntonRaid)
										{
											TheLexingtonRaid.StartHeist();
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.TheCartelYachtHeist)
										{
											YachtHeist.StartHeist();
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.TheBillionairesParty)
										{
											TheBillionaresParty.StartHeist();
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.TheFederalReserveTrainHeist)
										{
											TheFederalReserveTrainHeist.StartHeist();
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.TheMazeBankSavingsBankHeist)
										{
											TheMazebankSavingsBankHeistcs.StartHeist();
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.ThePrinthouseHeist)
										{
											ThePrinthouseHeist.StartHeist();
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.TheGefangnisArtHeist)
										{
											TheGefangnisArtHeist.StartHeist();
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.TheBettaBankHeist)
										{
											TheBetaBankHeist.StartHeist();
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.TheDiamondJewelStoreHeist)
										{
											TheDiamondJewelStore.StartHeist();
										}
									}
								}
								else
								{
									DisplayHelpTextThisFrame("Press ~INPUT_VEH_EXIT~ to Start ~t~" + Heists_Data[CurrentHeistData].HeistName);
									if (Game.IsControlJustPressed(Control.VehicleExit))
									{
										if (Heists_Data[CurrentHeistData].Heist == HeistID.The247Job)
										{
											The247Job.StartHeist();
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.TheSilthouseJob)
										{
											TheSilthouseJob.StartHeist();
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.Fleeca)
										{
											Class1.RunFleeca = 1;
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.Paleto)
										{
											Class1.RunPaleto = 1;
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.PacificStandard)
										{
											Class1.RunPacificStandard = 1;
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.JewelStore)
										{
											Class1.RunJewelryStore = 1;
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.UnionDepository)
										{
											Class1.RunUnionDepositoryHeist = 1;
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.LaFuenteBlanca)
										{
											Class1.RunLaFauenteBlanca = 1;
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.LaoubPrincessCargoshipHeist)
										{
											CargoShipHeist.StartHeist();
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.BikerClubhouse)
										{
											Class1.RunBikerClubhouseHeist = 1;
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.TheDiamondCasino)
										{
											TheDiamondHeist.RunHeist = 1;
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.CayoPericoHeist)
										{
											CayoPericoHeist.RunHeist = 1;
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.ThePrisonBreak)
										{
											ThePrisonBreak.RunHeist = 1;
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.TheHumaineLabs)
										{
											HumainLabsOnline.RunHeist = 1;
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.TheLuxigntonRaid)
										{
											TheLexingtonRaid.StartHeist();
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.TheCartelYachtHeist)
										{
											YachtHeist.StartHeist();
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.TheBillionairesParty)
										{
											TheBillionaresParty.StartHeist();
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.TheFederalReserveTrainHeist)
										{
											TheFederalReserveTrainHeist.StartHeist();
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.TheMazeBankSavingsBankHeist)
										{
											TheMazebankSavingsBankHeistcs.StartHeist();
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.ThePrinthouseHeist)
										{
											ThePrinthouseHeist.StartHeist();
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.TheGefangnisArtHeist)
										{
											TheGefangnisArtHeist.StartHeist();
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.TheBettaBankHeist)
										{
											TheBetaBankHeist.StartHeist();
										}
										if (Heists_Data[CurrentHeistData].Heist == HeistID.TheDiamondJewelStoreHeist)
										{
											TheDiamondJewelStore.StartHeist();
										}
									}
								}
								Scaleform scaleform = new Scaleform("instructional_buttons");
								scaleform.CallFunction("CLEAR_ALL");
								scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
								scaleform.CallFunction("CREATE_CONTAINER");
								scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 174, 0), "Prev Heist");
								scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 175, 0), "Next Heist");
								scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
								scaleform.Render2D();
								if (Game.IsControlJustPressed(Control.PhoneLeft))
								{
									RenderedBoardStage = 0;
									RenderedIntel = 0;
									CurrentHeistData--;
									if (CurrentHeistData <= -1)
									{
										CurrentHeistData = Heists_Data.Count - 1;
									}
								}
								if (Game.IsControlJustPressed(Control.PhoneRight))
								{
									RenderedBoardStage = 0;
									RenderedIntel = 0;
									CurrentHeistData++;
									if (CurrentHeistData >= Heists_Data.Count - 1)
									{
										CurrentHeistData = 0;
									}
								}
							}
							if (PrepBoard1 != null)
							{
								if (Game.IsControlJustPressed(Control.Context) && PrepBoard1 != null)
								{
									int handle = PrepBoard1.Handle;
									Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &handle);
									PrepBoard1 = new Scaleform("HEIST_MP");
								}
								if (RenderedIntel == 0)
								{
									PrepBoard1.CallFunction("SET_LABELS", Heists_Data[CurrentHeistData].HeistName, "Continue", "AVERAGE TAKE", "THE PLAN", "Reward", "", "ROLE", "CUT", "STATUS", "", "");
									PrepBoard1.CallFunction("ADD_CREW_MEMBER", 0, Game.Player.Name, XP.PlayerLevel.ToString(), "", "Driver/Driller", 0, "NOT READY", 7, Heists_Data[CurrentHeistData].AveragePay.ToString(), "100", 0, "", "");
									PrepBoard1.CallFunction("UPDATE_CREW_MEMBER", 0, Game.Player.Name, XP.PlayerLevel.ToString(), "", "Driver / Driller", 0, "NOT READY", 7, Heists_Data[CurrentHeistData].AveragePay.ToString(), "100", 0, "", "");
									PrepBoard1.CallFunction("ADD_PLAYERCARD", 0, Game.Player.Name, "7", "Driver/Driller", true, true, true, true, "4.0", "Heist Leader", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
									PrepBoard1.CallFunction("UPDATE_PLAYERCARD", 0, Game.Player.Name, "7", "Driver/Driller", true, true, true, true, "4.0", "Heist Leader", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
									PrepBoard1.CallFunction("ADD_PLAYERCARD", 1, "Optional AI 1", "0", "Gunner", false, false, false, false, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
									PrepBoard1.CallFunction("UPDATE_PLAYERCARD", 1, "Optional AI 1", "0", "Gunner", false, false, false, false, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
									PrepBoard1.CallFunction("ADD_PLAYERCARD", 2, "Optional AI 1", "0", "Gunner", false, false, false, false, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
									PrepBoard1.CallFunction("UPDATE_PLAYERCARD", 2, "Optional AI 1", "0", "Gunner", false, false, false, false, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
									PrepBoard1.CallFunction("ADD_PLAYERCARD", 3, "Optional AI 1", "0", "Gunner", false, false, false, false, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
									PrepBoard1.CallFunction("UPDATE_PLAYERCARD", 3, "Optional AI 1", "0", "Gunner", false, false, false, false, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
									if (Heists_Data[CurrentHeistData].Players >= 2)
									{
										PrepBoard1.CallFunction("ADD_CREW_MEMBER", 1, "Optional AI 1", "0", "", "Gunner", 0, "NOT READY", 7, "0", "0", 0, "", "");
										PrepBoard1.CallFunction("UPDATE_CREW_MEMBER", 1, "Optional AI 1", "0", "", "Gunner", 0, "NOT READY", 7, "0", "0", 0, "", "");
										PrepBoard1.CallFunction("ADD_PLAYERCARD", 1, "Optional AI 1", "0", "Gunner", true, true, true, true, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
										PrepBoard1.CallFunction("UPDATE_PLAYERCARD", 1, "Optional AI 1", "0", "Gunner", true, true, true, true, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
									}
									if (Heists_Data[CurrentHeistData].Players >= 3)
									{
										PrepBoard1.CallFunction("ADD_CREW_MEMBER", 2, "Optional AI 2", "0", "", "Gunner", 0, "NOT READY", 7, "0", "0", 0, "", "");
										PrepBoard1.CallFunction("UPDATE_CREW_MEMBER", 2, "Optional AI 2", "0", "", "Gunner", 0, "NOT READY", 7, "0", "0", 0, "", "");
										PrepBoard1.CallFunction("ADD_PLAYERCARD", 2, "Optional AI 1", "0", "Gunner", true, true, true, true, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
										PrepBoard1.CallFunction("UPDATE_PLAYERCARD", 2, "Optional AI 1", "0", "Gunner", true, true, true, true, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
									}
									if (Heists_Data[CurrentHeistData].Players >= 4)
									{
										PrepBoard1.CallFunction("ADD_CREW_MEMBER", 3, "Optional AI 3", "0", "", "Gunner", 0, "NOT READY", 7, "0", "0", 0, "", "");
										PrepBoard1.CallFunction("UPDATE_CREW_MEMBER", 3, "Optional AI 3", "0", "", "Gunner", 0, "NOT READY", 7, "0", "0", 0, "", "");
										PrepBoard1.CallFunction("ADD_PLAYERCARD", 3, "Optional AI 3", "0", "Gunner", true, true, true, true, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
										PrepBoard1.CallFunction("UPDATE_PLAYERCARD", 3, "Optional AI 3", "0", "Gunner", true, true, true, true, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
									}
									PrepBoard1.CallFunction("ADD_PIECHART", Heists_Data[CurrentHeistData].AveragePay, 100f, "0", "0", "0", "REWARD");
									PrepBoard1.CallFunction("UPDATE_PIECHART", Heists_Data[CurrentHeistData].AveragePay, 100f, "0", "0", "0", "REWARD");
									PrepBoard1.CallFunction("SET_HEIST_NAME", Heists_Data[CurrentHeistData].HeistName, " ", " ", " ");
									PrepBoard1.CallFunction("UPDATE_HEIST_NAME", Heists_Data[CurrentHeistData].HeistName, " ", " ", " ");
									if (Heists_Data[CurrentHeistData].Objectives.Count == 1)
									{
										PrepBoard1.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0]);
										PrepBoard1.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0]);
									}
									if (Heists_Data[CurrentHeistData].Objectives.Count == 2)
									{
										PrepBoard1.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1]);
										PrepBoard1.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1]);
									}
									if (Heists_Data[CurrentHeistData].Objectives.Count == 3)
									{
										PrepBoard1.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2]);
										PrepBoard1.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2]);
									}
									if (Heists_Data[CurrentHeistData].Objectives.Count == 4)
									{
										PrepBoard1.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3]);
										PrepBoard1.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3]);
									}
									if (Heists_Data[CurrentHeistData].Objectives.Count == 5)
									{
										PrepBoard1.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4]);
										PrepBoard1.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4]);
									}
									if (Heists_Data[CurrentHeistData].Objectives.Count == 6)
									{
										PrepBoard1.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5]);
										PrepBoard1.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5]);
									}
									if (Heists_Data[CurrentHeistData].Objectives.Count == 7)
									{
										PrepBoard1.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6]);
										PrepBoard1.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6]);
									}
									if (Heists_Data[CurrentHeistData].Objectives.Count == 8)
									{
										PrepBoard1.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6], Heists_Data[CurrentHeistData].Objectives[7]);
										PrepBoard1.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6], Heists_Data[CurrentHeistData].Objectives[7]);
									}
									if (Heists_Data[CurrentHeistData].Objectives.Count == 9)
									{
										PrepBoard1.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6], Heists_Data[CurrentHeistData].Objectives[7], Heists_Data[CurrentHeistData].Objectives[8]);
										PrepBoard1.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6], Heists_Data[CurrentHeistData].Objectives[7], Heists_Data[CurrentHeistData].Objectives[8]);
									}
									if (Heists_Data[CurrentHeistData].Objectives.Count == 10)
									{
										PrepBoard1.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6], Heists_Data[CurrentHeistData].Objectives[7], Heists_Data[CurrentHeistData].Objectives[8], Heists_Data[CurrentHeistData].Objectives[9]);
										PrepBoard1.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6], Heists_Data[CurrentHeistData].Objectives[7], Heists_Data[CurrentHeistData].Objectives[8], Heists_Data[CurrentHeistData].Objectives[9]);
									}
									if (Heists_Data[CurrentHeistData].Objectives.Count == 11)
									{
										PrepBoard1.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6], Heists_Data[CurrentHeistData].Objectives[7], Heists_Data[CurrentHeistData].Objectives[8], Heists_Data[CurrentHeistData].Objectives[9], Heists_Data[CurrentHeistData].Objectives[10]);
										PrepBoard1.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6], Heists_Data[CurrentHeistData].Objectives[7], Heists_Data[CurrentHeistData].Objectives[8], Heists_Data[CurrentHeistData].Objectives[9], Heists_Data[CurrentHeistData].Objectives[10]);
									}
									if (Heists_Data[CurrentHeistData].Objectives.Count == 12)
									{
										PrepBoard1.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6], Heists_Data[CurrentHeistData].Objectives[7], Heists_Data[CurrentHeistData].Objectives[8], Heists_Data[CurrentHeistData].Objectives[9], Heists_Data[CurrentHeistData].Objectives[10], Heists_Data[CurrentHeistData].Objectives[11]);
										PrepBoard1.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6], Heists_Data[CurrentHeistData].Objectives[7], Heists_Data[CurrentHeistData].Objectives[8], Heists_Data[CurrentHeistData].Objectives[9], Heists_Data[CurrentHeistData].Objectives[10], Heists_Data[CurrentHeistData].Objectives[11]);
									}
									if (Heists_Data[CurrentHeistData].Objectives.Count == 13)
									{
										PrepBoard1.CallFunction("ADD_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6], Heists_Data[CurrentHeistData].Objectives[7], Heists_Data[CurrentHeistData].Objectives[8], Heists_Data[CurrentHeistData].Objectives[9], Heists_Data[CurrentHeistData].Objectives[10], Heists_Data[CurrentHeistData].Objectives[11], Heists_Data[CurrentHeistData].Objectives[12]);
										PrepBoard1.CallFunction("UPDATE_TODO_LIST", Heists_Data[CurrentHeistData].Objectives[0], Heists_Data[CurrentHeistData].Objectives[1], Heists_Data[CurrentHeistData].Objectives[2], Heists_Data[CurrentHeistData].Objectives[3], Heists_Data[CurrentHeistData].Objectives[4], Heists_Data[CurrentHeistData].Objectives[5], Heists_Data[CurrentHeistData].Objectives[6], Heists_Data[CurrentHeistData].Objectives[7], Heists_Data[CurrentHeistData].Objectives[8], Heists_Data[CurrentHeistData].Objectives[9], Heists_Data[CurrentHeistData].Objectives[10], Heists_Data[CurrentHeistData].Objectives[11], Heists_Data[CurrentHeistData].Objectives[12]);
									}
									PrepBoard1.CallFunction("ADD_LAUNCH_BUTTON");
									PrepBoard1.CallFunction("INITIALISE_HEISTBOARD");
									PrepBoard1.CallFunction("LAUNCH_BUTTON_VISIBLE", true);
									PrepBoard1.CallFunction("LAUNCH_BUTTON_ENABLED", true);
									PrepBoard1.CallFunction("SET_LEADER_COST", "Leader Setup Cost:", 11500);
									PrepBoard1.CallFunction("SHOW_HEISTBOARD");
									RenderedBoardStage = 1;
								}
								PrepBoard1.Render3D(prop.GetOffsetPosition(Offset), new Vector3(0f, 0f, prop.Rotation.Z + BoardRot), Size);
							}
							Vector3 offsetPosition14 = prop.GetOffsetPosition(new Vector3(-0.48f, -0.025f, 1.8f));
							Vector3 rotation12 = prop.Rotation;
						}
						prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2687.77832f, -364.835358f, -56.1856537f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_planning_board_01a"), 0, 0, 0);
						if (prop != null)
						{
							if (PrepBoard2 == null)
							{
								PrepBoard2 = new Scaleform("HEISTMAP_MP");
							}
							if (PrepBoard2 != null)
							{
								if (Game.IsControlJustPressed(Control.Context) && PrepBoard2 != null)
								{
									int handle2 = PrepBoard2.Handle;
									Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &handle2);
									PrepBoard2 = new Scaleform("HEISTMAP_MP");
								}
								PrepBoard2.CallFunction("SET_LABELS", "The Pacific Standard ", "Continue", "AVERAGE TAKE", "THE PLAN", "Reward", "", "ROLE", "CUT", "STATUS", "", "");
								PrepBoard2.CallFunction("ADD_CREW_MEMBER", 0, Game.Player.Name, "3", "", "Driver/Driller", 0, "NOT READY", 7, "143750", "100", 0, "", "");
								PrepBoard2.CallFunction("UPDATE_CREW_MEMBER", 0, Game.Player.Name, "3", "", "Driver / Driller", 0, "NOT READY", 7, "143750", "100", 0, "", "");
								PrepBoard2.CallFunction("ADD_PLAYERCARD", 0, Game.Player.Name, "7", "Driver/Driller", true, true, true, true, "4.0", "Heist Leader", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
								PrepBoard2.CallFunction("UPDATE_PLAYERCARD", 0, Game.Player.Name, "7", "Driver/Driller", true, true, true, true, "4.0", "Heist Leader", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
								PrepBoard2.CallFunction("ADD_CREW_MEMBER", 1, "Optional AI 1", "0", "", "Gunner", 0, "NOT READY", 7, "0", "0", 0, "", "");
								PrepBoard2.CallFunction("UPDATE_CREW_MEMBER", 1, "Optional AI 1", "0", "", "Gunner", 0, "NOT READY", 7, "0", "0", 0, "", "");
								PrepBoard2.CallFunction("ADD_PLAYERCARD", 1, "Optional AI 1", "0", "Gunner", true, true, true, true, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
								PrepBoard2.CallFunction("UPDATE_PLAYERCARD", 1, "Optional AI 1", "0", "Gunner", true, true, true, true, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
								PrepBoard2.CallFunction("ADD_CREW_MEMBER", 2, "Optional AI 2", "0", "", "Gunner", 0, "NOT READY", 7, "0", "0", 0, "", "");
								PrepBoard2.CallFunction("UPDATE_CREW_MEMBER", 2, "Optional AI 2", "0", "", "Gunner", 0, "NOT READY", 7, "0", "0", 0, "", "");
								PrepBoard2.CallFunction("ADD_PLAYERCARD", 2, "Optional AI 1", "0", "Gunner", true, true, true, true, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
								PrepBoard2.CallFunction("UPDATE_PLAYERCARD", 2, "Optional AI 1", "0", "Gunner", true, true, true, true, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
								PrepBoard2.CallFunction("ADD_CREW_MEMBER", 3, "Optional AI 3", "0", "", "Gunner", 0, "NOT READY", 7, "0", "0", 0, "", "");
								PrepBoard2.CallFunction("UPDATE_CREW_MEMBER", 3, "Optional AI 3", "0", "", "Gunner", 0, "NOT READY", 7, "0", "0", 0, "", "");
								PrepBoard2.CallFunction("ADD_PLAYERCARD", 3, "Optional AI 3", "0", "Gunner", true, true, true, true, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
								PrepBoard2.CallFunction("UPDATE_PLAYERCARD", 3, "Optional AI 3", "0", "Gunner", true, true, true, true, "4.0", "Gunner", "Clean Player", 100f, 100f, 100f, 100f, 100f, 100f, 100f);
								PrepBoard2.CallFunction("ADD_PIECHART", "560922", 100f, "0", "0", "0", "REWARD");
								PrepBoard2.CallFunction("UPDATE_PIECHART", "560922", 100f, "0", "0", "0", "REWARD");
								PrepBoard2.CallFunction("SET_HEIST_NAME", "The Pacific Standard", "HEIST", "HEIST", "HEIST");
								PrepBoard2.CallFunction("UPDATE_HEIST_NAME", "The Pacific Standard", "HEIST", "HEIST", "HEIST");
								PrepBoard2.CallFunction("ADD_TODO_LIST", "Go to ~y~Lester's factory.~w~~n~- Get in the Kuruma.", "Enter the ~y~Bank.~w~~n~- Take out the ~r~cameras.~w~~n~- Go to the ~g~safety deposit box.~w~", "Test Input 1 ~n~ Data Input 1", "Test Input 2 ~n~ Data Input 2", "Test Input 3 ~n~ Data Input 3", "Test Input 4 ~n~ Data Input 4", "Test Input 5 ~n~ Data Input 5", "Test Input 6 ~n~ Data Input 6");
								PrepBoard2.CallFunction("UPDATE_TODO_LIST", "Go to ~y~Lester's factory.~w~~n~- Get in the Kuruma.", "Enter the ~y~Bank.~w~~n~- Take out the ~r~cameras.~w~~n~- Go to the ~g~safety deposit box.~w~", "Test Input 1 ~n~ Data Input 1", "Test Input 2 ~n~ Data Input 2", "Test Input 3 ~n~ Data Input 3", "Test Input 4 ~n~ Data Input 4", "Test Input 5 ~n~ Data Input 5", "Test Input 6 ~n~ Data Input 6");
								PrepBoard2.CallFunction("ADD_LAUNCH_BUTTON");
								PrepBoard2.CallFunction("INITIALISE_HEISTBOARD");
								PrepBoard2.CallFunction("LAUNCH_BUTTON_VISIBLE", false);
								PrepBoard2.CallFunction("LAUNCH_BUTTON_ENABLED", true);
								PrepBoard2.CallFunction("SET_LEADER_COST", "Leader Setup Cost:", 11500);
								PrepBoard2.CallFunction("SHOW_HEISTBOARD");
								if (RenderedIntel == 0)
								{
									RenderedIntel = 1;
									RemoveHighlights();
									if (Heists_Data[CurrentHeistData].IntelPoints.Count >= 1)
									{
										AddHighlight1(Heists_Data[CurrentHeistData].IntelPoints[0].X, Heists_Data[CurrentHeistData].IntelPoints[0].Y, 500f);
									}
									if (Heists_Data[CurrentHeistData].IntelPoints.Count >= 2)
									{
										AddHighlight2(Heists_Data[CurrentHeistData].IntelPoints[1].X, Heists_Data[CurrentHeistData].IntelPoints[1].Y, 500f);
									}
									if (Heists_Data[CurrentHeistData].IntelPoints.Count >= 3)
									{
										AddHighlight3(Heists_Data[CurrentHeistData].IntelPoints[2].X, Heists_Data[CurrentHeistData].IntelPoints[2].Y, 500f);
									}
									if (Heists_Data[CurrentHeistData].IntelPoints.Count >= 4)
									{
										AddHighlight4(Heists_Data[CurrentHeistData].IntelPoints[3].X, Heists_Data[CurrentHeistData].IntelPoints[3].Y, 500f);
									}
									if (Heists_Data[CurrentHeistData].IntelPoints.Count >= 5)
									{
										AddHighlight5(Heists_Data[CurrentHeistData].IntelPoints[4].X, Heists_Data[CurrentHeistData].IntelPoints[4].Y, 500f);
									}
									if (Heists_Data[CurrentHeistData].IntelPoints.Count >= 6)
									{
										AddHighlight6(Heists_Data[CurrentHeistData].IntelPoints[5].X, Heists_Data[CurrentHeistData].IntelPoints[5].Y, 500f);
									}
									if (Heists_Data[CurrentHeistData].IntelPoints.Count >= 7)
									{
										AddHighlight7(Heists_Data[CurrentHeistData].IntelPoints[6].X, Heists_Data[CurrentHeistData].IntelPoints[6].Y, 500f);
									}
									if (Heists_Data[CurrentHeistData].IntelPoints.Count >= 8)
									{
										AddHighlight8(Heists_Data[CurrentHeistData].IntelPoints[7].X, Heists_Data[CurrentHeistData].IntelPoints[7].Y, 500f);
									}
									if (Heists_Data[CurrentHeistData].IntelPoints.Count >= 9)
									{
										AddHighlight9(Heists_Data[CurrentHeistData].IntelPoints[8].X, Heists_Data[CurrentHeistData].IntelPoints[8].Y, 500f);
									}
									if (Heists_Data[CurrentHeistData].IntelPoints.Count >= 10)
									{
										AddHighlight10(Heists_Data[CurrentHeistData].IntelPoints[9].X, Heists_Data[CurrentHeistData].IntelPoints[9].Y, 500f);
									}
								}
								int typeEdit2 = typeEdit;
								int num16 = 1;
								PrepBoard2.Render3D(prop.GetOffsetPosition(Offset), new Vector3(0f, 0f, prop.Rotation.Z + BoardRot), Size);
							}
							Vector3 offsetPosition15 = prop.GetOffsetPosition(new Vector3(-0.48f, -0.025f, 1.8f));
							Vector3 rotation13 = prop.Rotation;
						}
					}
					if (MoneyVaultDoor != null && GetDistance_VDIST2(Game.Player.Character.Position, MoneyVaultDoor.Position, "<", 20f) && ArcadeMoneyVault == 1)
					{
						Vector3 offsetPosition16 = MoneyVaultDoor.GetOffsetPosition(new Vector3(0.7f, -1.2f, -1.2f));
						if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition16, "<", 1.35f) && Game.IsControlJustPressed(Control.Context) && !MoneyMenu.Visible)
						{
							MoneyMenu.Visible = true;
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition16, "<", 1.35f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~  to Access Money Vault");
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition16, "<", 80f))
						{
							World.DrawMarker(MarkerType.Cylinder, offsetPosition16, Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.65f), Color.Goldenrod);
						}
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2681.801f, -388.2215f, -56f), "<", 30f))
					{
						World.DrawMarker(MarkerType.Cylinder, new Vector3(2681.801f, -388.2215f, -56f), Vector3.Zero, Vector3.Zero, new Vector3(6f, 6f, 0.7f), Color.Goldenrod);
						if (Duf_WhiteBoard_Title == null)
						{
							Duf_WhiteBoard_Title = new Scaleform("PLAYER_NAME_01");
						}
						if (Duf_WhiteBoard_Title2 == null)
						{
							Duf_WhiteBoard_Title2 = new Scaleform("PLAYER_NAME_02");
						}
						if (Duf_WhiteBoard_Title_1 == null)
						{
							Duf_WhiteBoard_Title_1 = new Scaleform("PLAYER_NAME_03");
						}
						if (Duf_WhiteBoard_Title_2 == null)
						{
							Duf_WhiteBoard_Title_2 = new Scaleform("PLAYER_NAME_04");
						}
						Duf_WhiteBoard_Title.CallFunction("SET_PLAYER_NAME", "Player Cash Earned");
						Duf_WhiteBoard_Title_1.CallFunction("SET_PLAYER_NAME", "~g~$" + TotalPlayerEarned.ToString("N"));
						Duf_WhiteBoard_Title2.CallFunction("SET_PLAYER_NAME", "Crew Cash Earned");
						Duf_WhiteBoard_Title_2.CallFunction("SET_PLAYER_NAME", "~g~$" + TotalCrewEarned.ToString("N"));
						Duf_WhiteBoard_Title.Render3D(new Vector3(2681.801f, -388.2215f, -56f) + new Vector3(0f, 0f, 4f), new Vector3(0f, 87f, World.RenderingCamera.Rotation.Z + 180f), new Vector3(12f, 6f, 4f));
						Duf_WhiteBoard_Title_1.Render3D(new Vector3(2681.801f, -388.2215f, -56f) + new Vector3(0f, 0f, 3.5f), new Vector3(0f, 87f, World.RenderingCamera.Rotation.Z + 180f), new Vector3(6f, 3f, 2f));
						Duf_WhiteBoard_Title2.Render3D(new Vector3(2681.801f, -388.2215f, -56f) + new Vector3(0f, 0f, 3f), new Vector3(0f, 87f, World.RenderingCamera.Rotation.Z + 180f), new Vector3(12f, 6f, 4f));
						Duf_WhiteBoard_Title_2.Render3D(new Vector3(2681.801f, -388.2215f, -56f) + new Vector3(0f, 0f, 2.5f), new Vector3(0f, 87f, World.RenderingCamera.Rotation.Z + 180f), new Vector3(6f, 3f, 2f));
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2725.5f, -385f, -50f), "<", 100f))
					{
						World.DrawMarker(MarkerType.Cylinder, new Vector3(2725.5f, -385f, -50f), Vector3.Zero, Vector3.Zero, new Vector3(0.7f, 0.7f, 0.4f), Color.Goldenrod);
						if (GetDistance(Game.Player.Character.Position, new Vector3(2725.5f, -385f, -50f)) < 2f && Game.IsControlJustPressed(Control.Context))
						{
							Game.Player.Character.Rotation = new Vector3(0f, 0f, 90f);
							if (!DrinksMenu.Visible)
							{
								DrinksMenu.Visible = !DrinksMenu.Visible;
							}
						}
					}
					if (UnlockedHackingEquipment == 1)
					{
						Prop prop2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2693.018f, -372.4265f, -54.4431f, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_fingerprint_scanner_01e"), 0, 0, 0);
						if (prop2 != null)
						{
							Vector3 offsetPosition17 = prop2.GetOffsetPosition(new Vector3(0f, -0.75f, -1.2f));
							if (GetDistance(Game.Player.Character.Position, offsetPosition17) < 1.25f)
							{
								if (!FingerprintHack.KeypadHackOn && !FingerprintHack.FingerprintHackOn && !FingerprintHack.CopyPrintHackOn && !FingerprintHack.PlayingVoltageMG && !FingerprintHack.RunningPadLockMinigame && !FingerprintHack.HotWireMinigameOn && !FingerprintHack.DataCrackOn && !FingerprintHack.FIBHackOn && !FingerprintHack.BruteForceHackOn && !FingerprintHack.CircuitHackOn)
								{
									DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Practice Hacking Minigames");
								}
								if (Game.IsControlJustPressed(Control.Context))
								{
									ManagePracticeMinigames.HackingmainMenu.Visible = !ManagePracticeMinigames.HackingmainMenu.Visible;
								}
							}
							if (GetDistance(Game.Player.Character.Position, offsetPosition17) < 80f)
							{
								World.DrawMarker(MarkerType.Cylinder, offsetPosition17, Vector3.Zero, Vector3.Zero, new Vector3(0.7f, 0.7f, 0.4f), Color.Goldenrod);
							}
						}
					}
					if (UnlockedDilllEquipment == 1)
					{
						if (DrillingMinigames.CurrentDrill == DrillingMinigames.DrillUsing.None && GetDistance(Game.Player.Character.Position, PracticeDrill) < 1.25f)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Practice Drilling Minigames");
							if (Game.IsControlJustPressed(Control.Context))
							{
								ManagePracticeMinigames.DrillingmainMenu.Visible = !ManagePracticeMinigames.DrillingmainMenu.Visible;
							}
						}
						if (GetDistance(Game.Player.Character.Position, PracticeDrill) < 80f)
						{
							World.DrawMarker(MarkerType.Cylinder, PracticeDrill, Vector3.Zero, Vector3.Zero, new Vector3(0.7f, 0.7f, 0.4f), Color.Goldenrod);
						}
					}
					if (SittingInArcadeChairA && !BuyingArcades && GetDistance(Game.Player.Character.Position, ArcadeChairAPos) > 4f && SittingInArcadeChairA)
					{
						if (!WCamOn)
						{
							modLemonUIObjectPool.HideAll();
						}
						SittingInArcadeChairA = false;
						Game.Player.Character.IsPositionFrozen = true;
						GTA.UI.Screen.FadeOut(500);
						Script.Wait(500);
						Game.Player.Character.Position = ArcadeChairAPos;
						Game.Player.Character.Heading = ArcadeChairA.Rotation.Z;
						Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_HEADING, 0f);
						Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_PITCH, 0f);
						Game.Player.Character.Task.ClearAnimation("anim@amb@office@laptops@male@var_c@base@", "base");
						Game.Player.Character.IsPositionFrozen = false;
						Game.Player.Character.Position = ArcadeChairA.GetOffsetPosition(new Vector3(2f, 0f, 0f));
						Script.Wait(500);
						GTA.UI.Screen.FadeIn(500);
					}
					if (SittingInArcadeChairB && GetDistance(Game.Player.Character.Position, ArcadeChairBPos) > 4f && SittingInArcadeChairB)
					{
						if (!WCamOn)
						{
							modLemonUIObjectPool.HideAll();
						}
						SittingInArcadeChairB = false;
						Game.Player.Character.IsPositionFrozen = true;
						GTA.UI.Screen.FadeOut(500);
						Script.Wait(500);
						Game.Player.Character.Position = ArcadeChairBPos;
						Game.Player.Character.Heading = ArcadeChairB.Rotation.Z;
						Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_HEADING, 0f);
						Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_PITCH, 0f);
						Game.Player.Character.Task.ClearAnimation("anim@amb@office@laptops@male@var_c@base@", "base");
						Game.Player.Character.IsPositionFrozen = false;
						Game.Player.Character.Position = ArcadeChairB.GetOffsetPosition(new Vector3(2f, 0f, 0f));
						Script.Wait(500);
						GTA.UI.Screen.FadeIn(500);
					}
					if (BuyingArcades && !ArcadeBuyMenu.Visible)
					{
						Notification.Show("Player Closed Menu, Returning back to seat");
						BuyingArcades = false;
						if (ArcadeBuyCam != null)
						{
							Game.Player.WantedLevel = 0;
							Game.Player.Character.IsInvincible = false;
							Game.Player.Character.IsPositionFrozen = false;
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
							ArcadeBuyCam.IsActive = false;
							ArcadeBuyCam.Delete();
							SittingInArcadeChairA = true;
							GTA.UI.Screen.FadeOut(500);
							Script.Wait(500);
							Game.Player.Character.IsPositionFrozen = true;
							Game.Player.Character.Position = new Vector3(2726.5f, -377.5f, -49f);
							Game.Player.Character.Heading = 0f;
							Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_HEADING, 0f);
							Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_PITCH, 0f);
							Game.Player.Character.Task.PlayAnimation("anim@amb@office@laptops@male@var_c@base@", "base", 8f, 1f, -1, AnimationFlags.Loop, -1f);
							Script.Wait(500);
							GTA.UI.Screen.FadeIn(500);
							foreach (Prop arcadeProp in ArcadeProps)
							{
								if (arcadeProp != null)
								{
									arcadeProp.Delete();
								}
							}
							foreach (Prop seconardyArcadeProp in SeconardyArcadeProps)
							{
								if (seconardyArcadeProp != null)
								{
									seconardyArcadeProp.Delete();
								}
							}
							RacingArcades.Clear();
							RefreshArcadeInterior();
						}
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, ArcadeChairAPos, "<", 2f))
					{
						if (!SittingInArcadeChairA && Game.IsControlJustPressed(Control.Cover))
						{
							Script.Wait(100);
							string text = "anim@amb@office@boss@male@";
							LoadDict("anim@amb@office@boss@male@");
							if (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, text))
							{
								if (ArcadeChairA == null)
								{
									Notification.Show("ArcadeChairA null");
								}
								int num7 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, ArcadeChairA.Position.X, ArcadeChairA.Position.Y, ArcadeChairA.Position.Z, 0.0, 0.0, ArcadeChairA.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, num7, LoadDict("anim@amb@office@boss@male@"), "enter", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, num7, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, num7));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, num7, "enter", LoadDict("anim@amb@office@boss@male@"), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ArcadeChairA, num7, "enter_chair", LoadDict("anim@amb@office@boss@male@"), 16f, -16f, 0, 1148846080);
								Script.Wait(5000);
								SittingInArcadeChairA = true;
							}
						}
						if (SittingInArcadeChairA)
						{
							if (Game.IsControlJustPressed(Control.Context))
							{
								modLemonUIObjectPool.RefreshAll();
								if (!ArcadeC.Visible)
								{
									ArcadeC.Visible = !ArcadeC.Visible;
								}
							}
							if (Game.IsControlJustPressed(Control.Cover))
							{
								modLemonUIObjectPool.HideAll();
								int num8 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, ArcadeChairA.Position.X, ArcadeChairA.Position.Y, ArcadeChairA.Position.Z, 0.0, 0.0, ArcadeChairA.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, num8, LoadDict("anim@amb@office@boss@male@"), "exit", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, num8, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, num8));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, num8, "exit", LoadDict("anim@amb@office@boss@male@"), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ArcadeChairA, num8, "exit_chair", LoadDict("anim@amb@office@boss@male@"), 16f, -16f, 0, 1148846080);
								Script.Wait(4000);
								Game.Player.Character.Task.ClearAll();
								SittingInArcadeChairA = false;
							}
						}
						if (SittingInArcadeChairA)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Open Menu, Press ~INPUT_COVER~ to Exit");
						}
						else
						{
							DisplayHelpTextThisFrame("Press ~INPUT_COVER~ to sit on chair A to access Menu");
						}
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, ArcadeChairBPos, "<", 2f))
					{
						if (!SittingInArcadeChairB && Game.IsControlJustPressed(Control.Cover))
						{
							Script.Wait(100);
							string text2 = "anim@amb@office@boss@male@";
							LoadDict("anim@amb@office@boss@male@");
							if (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, text2))
							{
								int num9 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, ArcadeChairB.Position.X, ArcadeChairB.Position.Y, ArcadeChairB.Position.Z, 0.0, 0.0, ArcadeChairB.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, num9, LoadDict("anim@amb@office@boss@male@"), "enter", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, num9, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, num9));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, num9, "enter", LoadDict("anim@amb@office@boss@male@"), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ArcadeChairB, num9, "enter_chair", LoadDict("anim@amb@office@boss@male@"), 16f, -16f, 0, 1148846080);
								Script.Wait(5000);
								SittingInArcadeChairB = true;
							}
						}
						if (SittingInArcadeChairB)
						{
							if ((Game.IsControlJustPressed(Control.FrontendPauseAlternate) || Game.IsControlJustPressed(Control.PhoneCancel)) && MainStuffMenu.Visible)
							{
								Prop arcadeChairB = ArcadeChairB;
								int num10 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, arcadeChairB.Position.X, arcadeChairB.Position.Y, arcadeChairB.Position.Z, 0.0, 0.0, arcadeChairB.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, num10, LoadDict("anim@amb@office@boss@male@"), "computer_exit", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, num10, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, num10));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, num10, "computer_exit", LoadDict("anim@amb@office@boss@male@"), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, arcadeChairB, num10, "computer_exit_chair", LoadDict("anim@amb@office@boss@male@"), 16f, -16f, 0, 1148846080);
							}
							if (Game.IsControlJustPressed(Control.Context))
							{
								Prop arcadeChairB2 = ArcadeChairB;
								int num11 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, arcadeChairB2.Position.X, arcadeChairB2.Position.Y, arcadeChairB2.Position.Z, 0.0, 0.0, arcadeChairB2.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, num11, LoadDict("anim@amb@office@boss@male@"), "computer_enter", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, num11, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, num11));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, num11, "computer_enter", LoadDict("anim@amb@office@boss@male@"), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, arcadeChairB2, num11, "computer_enter_chair", LoadDict("anim@amb@office@boss@male@"), 16f, -16f, 0, 1148846080);
								Script.Wait(100);
								HKHModHelperNew.Class1.GUIScreenActive = "Hub";
							}
							if (Game.IsControlJustPressed(Control.Cover))
							{
								modLemonUIObjectPool.HideAll();
								int num12 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, ArcadeChairB.Position.X, ArcadeChairB.Position.Y, ArcadeChairB.Position.Z, 0.0, 0.0, ArcadeChairB.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, num12, LoadDict("anim@amb@office@boss@male@"), "exit", 16f, -16f, 0, 1148846080);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, num12, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, num12));
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, num12, "exit", LoadDict("anim@amb@office@boss@male@"), 16f, -16f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ArcadeChairB, num12, "exit_chair", LoadDict("anim@amb@office@boss@male@"), 16f, -16f, 0, 1148846080);
								Script.Wait(4000);
								Game.Player.Character.Task.ClearAll();
								SittingInArcadeChairB = false;
							}
						}
						if (SittingInArcadeChairB)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Access Master Control Terminal Panel\n Press ~INPUT_COVER~ to Exit");
						}
						else
						{
							DisplayHelpTextThisFrame("Press ~INPUT_COVER~ to sit on chair B to access Menu");
						}
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, ArcadeGarageExit, "<", 2f) && Game.IsControlPressed(Control.Context))
					{
						DestoryCars();
						GTA.UI.Screen.FadeOut(300);
						Script.Wait(300);
						GTA.UI.Screen.FadeIn(300);
						Game.Player.Character.Position = ArcadeGarageExitOnFootSpawn;
						Game.Player.Character.Heading = ArcadeGarageExitOnFootSpawnHeading;
						IsInArcade = false;
						Script.Wait(10);
						Function.Call(Hash.ANIMPOSTFX_STOP_ALL);
						Function.Call(Hash.PUSH_TIMECYCLE_MODIFIER);
						Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
						Function.Call(Hash.POP_TIMECYCLE_MODIFIER);
						Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "default");
						if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
						{
							Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
						}
						Function.Call(Hash.SET_RADAR_ZOOM, 850);
						foreach (Blip item7 in MainBlips_U)
						{
							if (item7 != null)
							{
								item7.Delete();
							}
						}
						foreach (Blip item8 in MainBlips_G)
						{
							if (item8 != null)
							{
								item8.Delete();
							}
						}
						CreatedTunerGear = false;
						Tuner_Gear_Objects.DeleteEntites();
						if (ReturnGameBuild() >= 3095)
						{
							ArcadeObjects_v3095.DeleteEntites();
						}
						else
						{
							ArcadeObjects_v2944.DeleteEntites();
						}
					}
					if (UndergrondAreaLocked == 1 && GetDistance_VDIST2(Game.Player.Character.Position, ArcadeGarageExit, "<", 100f))
					{
						World.DrawMarker(MarkerType.Cylinder, ArcadeGarageExit, Vector3.Zero, Vector3.Zero, new Vector3(1f, 1f, 0.45f), Color.Goldenrod);
						if (GetDistance_VDIST2(Game.Player.Character.Position, ArcadeGarageExit, "<", 2f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit Arcade OR ~INPUT_COVER~ To Buy/Sell a Vehicle");
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, ArcadeGarageExit, "<", 2f) && Game.IsControlPressed(Control.Cover) && !RemoveMenu2.Visible)
						{
							RemoveMenu2.Visible = true;
						}
					}
					if (ToldPlayerHowToStartHeist == 1)
					{
						if (UndergrondAreaLocked == 0)
						{
							GTA.UI.Screen.ShowSubtitle("Unlock the ~b~ Underground Area~w~ to Start a ~y~Heist~w~");
							if (missionBlip == null)
							{
								missionBlip = GlobalHelper.CreateBlip(new Vector3(2726.469f, -377.168f, -47.3f));
								missionBlip.Position = new Vector3(2726.469f, -377.168f, -47.3f);
								missionBlip.Sprite = BlipSprite.Standard;
								missionBlip.Color = BlipColor.Blue;
							}
						}
						if (UndergrondAreaLocked == 1)
						{
							if (RanTutorial == 0 && TutorialStage == 0)
							{
								UnlockedDilllEquipmentTEMP = ArcadeConfig.GetValue("Arcade", "UnlockedDilllEquipment", UnlockedDilllEquipment);
								UnlockedHackingEquipmentTEMP = ArcadeConfig.GetValue("Arcade", "UnlockedHackingEquipment", UnlockedHackingEquipment);
								UnlockedBreachingEquipmentTEMP = ArcadeConfig.GetValue("Arcade", "UnlockedBreachingEquipment", UnlockedBreachingEquipment);
								UnlockedOutfitsEquipmentTEMP = ArcadeConfig.GetValue("Arcade", "UnlockedOutfitsEquipment", UnlockedOutfitsEquipment);
								UnlockedPlansEquipmentTEMP = ArcadeConfig.GetValue("Arcade", "UnlockedPlansEquipment", UnlockedPlansEquipment);
								UnlockedGarageTEMP = ArcadeConfig.GetValue("Arcade", "UnlockedGarage", UnlockedGarage);
								UnlockedGunlockerTEMP = ArcadeConfig.GetValue("Arcade", "UnlockedGunlocker", UnlockedGunlocker);
								ArcadeMoneyVaultTEMP = ArcadeConfig.GetValue("Arcade", "ArcadeMoneyVault", ArcadeMoneyVault);
								UndergrondAreaLockedTEMP = ArcadeConfig.GetValue("Arcade", "UndergrondAreaLocked", UndergrondAreaLocked);
								UnlockedDilllEquipment = 1;
								ArcadeConfig.SetValue("Arcade", "UnlockedDilllEquipment", UnlockedDilllEquipment);
								UnlockedHackingEquipment = 1;
								ArcadeConfig.SetValue("Arcade", "UnlockedHackingEquipment", UnlockedHackingEquipment);
								UnlockedBreachingEquipment = 1;
								ArcadeConfig.SetValue("Arcade", "UnlockedBreachingEquipment", UnlockedBreachingEquipment);
								UnlockedOutfitsEquipment = 1;
								ArcadeConfig.SetValue("Arcade", "UnlockedOutfitsEquipment", UnlockedOutfitsEquipment);
								UnlockedPlansEquipment = 1;
								ArcadeConfig.SetValue("Arcade", "UnlockedPlansEquipment", UnlockedPlansEquipment);
								UnlockedGarage = 1;
								ArcadeConfig.SetValue("Arcade", "UnlockedGarage", UnlockedGarage);
								UnlockedGunlocker = 1;
								ArcadeConfig.SetValue("Arcade", "UnlockedGunlocker", UnlockedGunlocker);
								ArcadeMoneyVault = 1;
								ArcadeConfig.SetValue("Arcade", "ArcadeMoneyVault", ArcadeMoneyVault);
								ArcadeConfig.Save();
								TextNotification("CHAR_LESTER", "~g~ Lester", "Single Player Heists Tutorial", "Lets go over a couple of important things shall we?");
								TutorialStage = 1;
								RefreshArcadeInterior();
								NextTutorialPhase = Game.GameTime + 5000;
								RunningTutorial = true;
								if (missionBlip != null)
								{
									missionBlip.Delete();
								}
								missionBlip = GlobalHelper.CreateBlip(new Vector3(2726.469f, -377.168f, -47.3f));
								missionBlip.Position = new Vector3(2726.469f, -377.168f, -47.3f);
								missionBlip.Sprite = BlipSprite.Standard;
								missionBlip.Color = BlipColor.Blue;
								for (int i = 0; i < 300; i++)
								{
									Script.Wait(1);
									TutorialStart();
								}
							}
							if (RanTutorial == 1 && missionBlip != null)
							{
								missionBlip.Delete();
							}
						}
					}
					if (UnlockedGarage == 0 && UnlockedGarage == 0)
					{
						Vector3 vector3 = new Vector3(2697.024f, -372.4954f, -54.7f);
						if (Duf_WhiteBoard_Title3 == null)
						{
							Duf_WhiteBoard_Title3 = new Scaleform("PLAYER_NAME_06");
						}
						if (Duf_WhiteBoard_Title4 == null)
						{
							Duf_WhiteBoard_Title4 = new Scaleform("PLAYER_NAME_07");
						}
						Duf_WhiteBoard_Title3.CallFunction("SET_PLAYER_NAME", "Area Locked by");
						Duf_WhiteBoard_Title4.CallFunction("SET_PLAYER_NAME", "~b~ Garage Space 4 - 11 Upgrade");
						Duf_WhiteBoard_Title3.Render3D(vector3 + new Vector3(0f, 0f, 1f), new Vector3(0f, 87f, 95f), new Vector3(12f, 6f, 4f));
						Duf_WhiteBoard_Title4.Render3D(vector3 + new Vector3(0f, 0f, 0.5f), new Vector3(0f, 87f, 95f), new Vector3(6f, 3f, 2f));
					}
					if (HeistVehicle == null)
					{
						return;
					}
					if (HeistVehicle != null)
					{
						HeistVehicle.IsPositionFrozen = true;
						if (Game.Player.Character.CurrentVehicle != null && Game.Player.Character.CurrentVehicle == HeistVehicle)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit from Arcade with this Vehicle");
							if (Game.IsControlJustPressed(Control.Context))
							{
								GTA.UI.Screen.FadeOut(500);
								Script.Wait(500);
								IsInArcade = false;
								HeistVehicle.Position = ArcadeGarageEnter;
								Script.Wait(500);
								GTA.UI.Screen.FadeIn(500);
								HeistVehicle = null;
								Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
								Function.Call(Hash.POP_TIMECYCLE_MODIFIER);
								Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "default");
								if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
								{
									Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
								}
							}
						}
					}
				}
			}
			if (!Firstime)
			{
				Function.Call(Hash.SET_RADAR_ZOOM, 850);
				Firstime = true;
				if (MoneyVaultDoor != null)
				{
					MoneyVaultDoor.Delete();
				}
				MoneyVaultDoor = GlobalHelper.CreateProp(RequestModel(-1264354268), new Vector3(2723.902f, -389.5595f, -56.3f), new Vector3(0f, 0f, -90f), false, false);
				MoneyVaultDoor.IsPositionFrozen = true;
				try
				{
					if (ArcadeChairA != null)
					{
						ArcadeChairA.Delete();
					}
					ArcadeChairA = GlobalHelper.CreateProp(SpawnProp(ChairPropModelArcadeA), ArcadeChairAPos, false, false);
					ArcadeChairA.Heading = ArcadeChairAHeading;
					ArcadeChairA.IsPositionFrozen = true;
					Script.Wait(1);
					ArcadeChairA.Position = new Vector3(ArcadeChairA.Position.X, ArcadeChairA.Position.Y, ArcadeChairA.Position.Z - 0.5f);
				}
				catch (Exception)
				{
				}
				try
				{
					if (ArcadeChairB != null)
					{
						ArcadeChairB.Delete();
					}
					ArcadeChairB = GlobalHelper.CreateProp(SpawnProp(ChairPropModelArcadeB), ArcadeChairBPos, false, false);
					ArcadeChairB.Heading = ArcadeChairBHeading;
					ArcadeChairB.IsPositionFrozen = true;
					Script.Wait(1);
					ArcadeChairB.Position = new Vector3(ArcadeChairB.Position.X, ArcadeChairB.Position.Y, ArcadeChairB.Position.Z - 0.05f);
				}
				catch (Exception)
				{
				}
			}
			if (!Firsttime)
			{
				Firsttime = true;
				SpawnProp("ch_prop_ch_tunnel_door01a", new Vector3(993.5f, -140.8f, 33.6f), new Vector3(0f, 0f, -36f));
			}
			if (IsInArcade && WorderbePool != null)
			{
				WorderbePool.Process();
			}
			if (LesterHasToldPlayerOfNewHeists == 0)
			{
				ArcadeOwned = 0;
			}
			if (ArcadeOwned == 1)
			{
				return;
			}
			if (SPH_Debug.ShowMainDebug)
			{
				drawText("SPH Time Till Lester Text " + (GameTimeToTriggerUINotification - Game.GameTime), 0.1f, 0.15f, 0.5f, 255, 255, 255);
			}
			if (LesterHasToldPlayerOfNewHeists == 0 && Game.GameTime < GameTimeToTriggerUINotification)
			{
				HeistBlip.Alpha = 0;
			}
			if (LesterHasToldPlayerOfNewHeists == 0 && Game.GameTime > GameTimeToTriggerUINotification && !Game.IsMissionActive && Game.Player.CanControlCharacter && !Function.Call<bool>(Hash.IS_CUTSCENE_ACTIVE, Array.Empty<InputArgument>()))
			{
				if (!NotifyPlayerthatLesterWantsToSeeThem)
				{
					ArcadeOwned = 0;
					ArcadeConfig.SetValue("Arcade", "ArcadeOwned", ArcadeOwned);
					ArcadeVersion = 0;
					ArcadeConfig.SetValue("Arcade", "ArcadeVersion", ArcadeVersion);
					ArcadeFloodID = 0;
					ArcadeConfig.SetValue("Arcade", "ArcadeFloodID", ArcadeFloodID);
					ArcadeStyle = 0;
					ArcadeConfig.SetValue("Arcade", "ArcadeStyle", ArcadeStyle);
					MuralID = 0;
					ArcadeConfig.SetValue("Arcade", "MuralID", MuralID);
					PushieID = 0;
					ArcadeConfig.SetValue("Arcade", "PushieID", PushieID);
					NeonArtID = 0;
					ArcadeConfig.SetValue("Arcade", "NeonArtID", NeonArtID);
					ArcadeClutter = false;
					ArcadeConfig.SetValue("Arcade", "ArcadeClutter", ArcadeClutter);
					UnlockedDilllEquipment = 0;
					ArcadeConfig.SetValue("Arcade", "UnlockedDilllEquipment", UnlockedDilllEquipment);
					UnlockedHackingEquipment = 0;
					ArcadeConfig.SetValue("Arcade", "UnlockedHackingEquipment", UnlockedHackingEquipment);
					UnlockedBreachingEquipment = 0;
					ArcadeConfig.SetValue("Arcade", "UnlockedBreachingEquipment", UnlockedBreachingEquipment);
					UnlockedOutfitsEquipment = 0;
					ArcadeConfig.SetValue("Arcade", "UnlockedOutfitsEquipment", UnlockedOutfitsEquipment);
					UnlockedPlansEquipment = 0;
					ArcadeConfig.SetValue("Arcade", "UnlockedPlansEquipment", UnlockedPlansEquipment);
					UnlockedGarage = 0;
					ArcadeConfig.SetValue("Arcade", "UnlockedGarage", UnlockedGarage);
					UnlockedGunlocker = 0;
					ArcadeConfig.SetValue("Arcade", "UnlockedGunlocker", UnlockedGunlocker);
					ArcadeMoneyVault = 0;
					ArcadeConfig.SetValue("Arcade", "ArcadeMoneyVault", ArcadeMoneyVault);
					UndergrondAreaLocked = 0;
					ArcadeConfig.SetValue("Arcade", "UndergrondAreaLocked", UndergrondAreaLocked);
					Arcade1Ver = 0;
					ArcadeConfig.SetValue("Arcade", "Arcade1Ver", Arcade1Ver);
					Arcade2Ver = 0;
					ArcadeConfig.SetValue("Arcade", "Arcade2Ver", Arcade2Ver);
					Arcade3Ver = 0;
					ArcadeConfig.SetValue("Arcade", "Arcade3Ver", Arcade3Ver);
					Arcade4Ver = 0;
					ArcadeConfig.SetValue("Arcade", "Arcade4Ver", Arcade4Ver);
					Arcade5Ver = 0;
					ArcadeConfig.SetValue("Arcade", "Arcade5Ver", Arcade5Ver);
					Arcade6Ver = 0;
					ArcadeConfig.SetValue("Arcade", "Arcade6Ver", Arcade6Ver);
					Arcade7Ver = 0;
					ArcadeConfig.SetValue("Arcade", "Arcade7Ver", Arcade7Ver);
					Arcade8Ver = 0;
					ArcadeConfig.SetValue("Arcade", "Arcade8Ver", Arcade8Ver);
					Arcade9Ver = 0;
					ArcadeConfig.SetValue("Arcade", "Arcade9Ver", Arcade9Ver);
					Arcade10Ver = 0;
					ArcadeConfig.SetValue("Arcade", "Arcade10Ver", Arcade10Ver);
					Arcade11Ver = 0;
					ArcadeConfig.SetValue("Arcade", "Arcade11Ver", Arcade11Ver);
					Arcade12Ver = 0;
					ArcadeConfig.SetValue("Arcade", "Arcade12Ver", Arcade12Ver);
					Arcade13Ver = 0;
					ArcadeConfig.SetValue("Arcade", "Arcade13Ver", Arcade13Ver);
					Arcade14Ver = 0;
					ArcadeConfig.SetValue("Arcade", "Arcade14Ver", Arcade14Ver);
					Arcade15Ver = 0;
					ArcadeConfig.SetValue("Arcade", "Arcade15Ver", Arcade15Ver);
					Arcade16Ver = 0;
					ArcadeConfig.SetValue("Arcade", "Arcade16Ver", Arcade16Ver);
					Arcade17Ver = 0;
					ArcadeConfig.SetValue("Arcade", "Arcade17Ver", Arcade17Ver);
					Arcade18Ver = 0;
					ArcadeConfig.SetValue("Arcade", "Arcade18Ver", Arcade18Ver);
					Arcade19Ver = 0;
					ArcadeConfig.SetValue("Arcade", "Arcade19Ver", Arcade19Ver);
					Arcade20Ver = 0;
					ArcadeConfig.SetValue("Arcade", "Arcade20Ver", Arcade20Ver);
					PlayRadioMusic = true;
					ArcadeConfig.SetValue("Arcade", "PlayRadioMusic", PlayRadioMusic);
					RadioOn = true;
					ArcadeConfig.SetValue("Arcade", "RadioOn", RadioOn);
					TextNotification("CHAR_LESTER", "~g~Lester", "A Business Opportunity", "I think i have something you might be interested in, meet me at my Factory..");
					ManageCellphoneCall.DialogueLines.Clear();
					ManageCellphoneCall.DialogueLines.Add("LC_AAAA");
					ManageCellphoneCall.DialogueLines.Add("LC_AAAB");
					ManageCellphoneCall.Speaker = "Lester";
					ManageCellphoneCall.StartPhoneCall();
					ManageCellphoneCall.ConvStarted = true;
					ManageCellphoneCall.ContactName = "Lester";
					ManageCellphoneCall.ContactIcon = "Char_Lester";
					NotifyPlayerthatLesterWantsToSeeThem = true;
				}
				HeistBlip.Position = new Vector3(706.65f, -965.0518f, 30.4f);
				HeistBlip.Sprite = BlipSprite.HeistSetup;
				HeistBlip.Name = "Single Player Heists Setup";
				HeistBlip.IsShortRange = true;
				HeistBlip.Color = BlipColor.Yellow;
				HeistBlip.Alpha = 255;
				if (Function.Call<bool>(Hash.IS_IPL_ACTIVE, "m24_2_garment_factory"))
				{
					Function.Call(Hash.REMOVE_IPL, "m24_2_garment_factory");
				}
				if (Function.Call<bool>(Hash.IS_IPL_ACTIVE, "m24_2_garment_factory_door"))
				{
					Function.Call(Hash.REMOVE_IPL, "m24_2_garment_factory_door");
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(717.2645f, -964.95f, 30f), "<", 5f) && !RequestedCutscene)
				{
					if (CutsenePed2 != null)
					{
						CutsenePed2.Delete();
					}
					if (CutsenePed3 != null)
					{
						CutsenePed3.Delete();
					}
					if (CutsenePed4 != null)
					{
						CutsenePed4.Delete();
					}
					Function.Call(Hash.REQUEST_CUTSCENE, "heist_int");
					CutsenePed2 = GlobalHelper.CreatePed(PedHash.Michael, new Vector3(0f, 0f, 0f));
					CutsenePed2.IsVisible = false;
					CutsenePed3 = GlobalHelper.CreatePed(PedHash.Michael, new Vector3(0f, 0f, 0f));
					CutsenePed3.IsVisible = false;
					CutsenePed4 = GlobalHelper.CreatePed(PedHash.Michael, new Vector3(0f, 0f, 0f));
					CutsenePed4.IsVisible = false;
					RequestedCutscene = true;
				}
				if (Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED) && RequestedCutscene)
				{
					if (!PlayingCutscene)
					{
						if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							CutsenePed1 = Game.Player.Character;
							SetPedOutfitCutscene("MP_1", Game.Player.Character);
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutsenePed1, "MP_1", 0, 0, 64);
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutsenePed2, "MP_2", 0, 0, 64);
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutsenePed3, "MP_3", 0, 0, 64);
							Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutsenePed4, "MP_4", 0, 0, 64);
							Vector3 vector4 = new Vector3(1867f, 1823f, 140f);
							Function.Call(Hash.START_CUTSCENE, 0);
							Script.Wait(50);
							GetPedOutfitCutscene("MP_1", CutsenePed1);
							GetPedOutfitCutscene("MP_2", CutsenePed2);
							GetPedOutfitCutscene("MP_3", CutsenePed3);
							GetPedOutfitCutscene("MP_4", CutsenePed4);
						}
						if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							PlayingCutscene = true;
						}
					}
					if (PlayingCutscene)
					{
						if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 1)
						{
							HeistBlip.Alpha = 0;
							LesterHasToldPlayerOfNewHeists = 1;
							Config.SetValue("Misc", "LesterHasToldPlayerOfNewHeists", LesterHasToldPlayerOfNewHeists);
							Config.Save();
							Notification.Show("~y~ Lester ~w~ Ok, to start a Heist your going to need to purchase an Arcade, ive marked several around your map");
							PlayingCutscene = false;
							RequestedCutscene = false;
							Function.Call(Hash.STOP_CUTSCENE, true);
						}
						if (!Function.Call<bool>(Hash.HAS_CUTSCENE_FINISHED) && Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING) && Game.IsControlPressed(Control.VehicleHandbrake))
						{
							HeistBlip.Alpha = 0;
							LesterHasToldPlayerOfNewHeists = 1;
							Config.SetValue("Misc", "LesterHasToldPlayerOfNewHeists", LesterHasToldPlayerOfNewHeists);
							Config.Save();
							Notification.Show("~y~ Lester ~w~ Ok, to start a Heist your going to need to purchase an Arcade, ive marked several around your map");
							PlayingCutscene = false;
							RequestedCutscene = false;
							Function.Call(Hash.STOP_CUTSCENE, true);
						}
						if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
						{
							HeistBlip.Alpha = 0;
							LesterHasToldPlayerOfNewHeists = 1;
							Config.SetValue("Misc", "LesterHasToldPlayerOfNewHeists", LesterHasToldPlayerOfNewHeists);
							Config.Save();
							Notification.Show("~y~ Lester ~w~ Ok, to start a Heist your going to need to purchase an Arcade, ive marked several around your map");
							PlayingCutscene = false;
							RequestedCutscene = false;
							if (CutsenePed2 != null)
							{
								CutsenePed2.Delete();
							}
							if (CutsenePed3 != null)
							{
								CutsenePed3.Delete();
							}
							if (CutsenePed4 != null)
							{
								CutsenePed4.Delete();
							}
						}
					}
				}
			}
			if (LesterHasToldPlayerOfNewHeists != 1)
			{
				return;
			}
			if (CreatedArcadeBlips)
			{
				foreach (Arcade arcade in Arcades)
				{
					if (!GetDistance_VDIST2(Game.Player.Character.Position, arcade.Location, "<", 40f))
					{
						continue;
					}
					World.DrawMarker(MarkerType.Cylinder, arcade.Location, Vector3.Zero, Vector3.Zero, new Vector3(6f, 6f, 1f), Color.AliceBlue);
					if (GetDistance_VDIST2(Game.Player.Character.Position, arcade.Location, "<", 4f))
					{
						DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to purchase " + arcade.Name + " for ~g~$" + arcade.Price.ToString("N"));
						if (Game.IsControlJustPressed(Control.Attack))
						{
							if ((float)Game.Player.Money >= arcade.Price)
							{
								TextNotification("CHAR_LESTER", "~g~ Lester", "Arcade Setup", "You now have your very own Arcade, head inside to Start A heist");
								ManagePlayerFunds.RemoveFunds((int)arcade.Price);
								if (arcade.Name.Equals("Pixel Pete's"))
								{
									ArcadeOwned = 1;
									ArcadeVersion = 1;
									ArcadeConfig.SetValue("Arcade", "ArcadeOwned", ArcadeOwned);
									ArcadeConfig.SetValue("Arcade", "ArcadeVersion", ArcadeVersion);
									ArcadeConfig.Save();
									Notification.Show(GetHostName() + " You now own " + arcade.Name + " Arcade");
									Class1.SetupActivities();
									foreach (Blip arcadeBlip in ArcadeBlips)
									{
										if (arcadeBlip != null)
										{
											arcadeBlip.Delete();
										}
									}
									ChangeArcade();
									Class1.SetupActivities();
								}
								if (arcade.Name.Equals("Wonderama"))
								{
									ArcadeOwned = 1;
									ArcadeVersion = 2;
									ArcadeConfig.SetValue("Arcade", "ArcadeOwned", ArcadeOwned);
									ArcadeConfig.SetValue("Arcade", "ArcadeVersion", ArcadeVersion);
									ArcadeConfig.Save();
									Notification.Show(GetHostName() + " You now own " + arcade.Name + " Arcade");
									Class1.SetupActivities();
									foreach (Blip arcadeBlip2 in ArcadeBlips)
									{
										if (arcadeBlip2 != null)
										{
											arcadeBlip2.Delete();
										}
									}
									ChangeArcade();
									Class1.SetupActivities();
								}
								if (arcade.Name.Equals("Videogeddon"))
								{
									ArcadeOwned = 1;
									ArcadeVersion = 3;
									ArcadeConfig.SetValue("Arcade", "ArcadeOwned", ArcadeOwned);
									ArcadeConfig.SetValue("Arcade", "ArcadeVersion", ArcadeVersion);
									ArcadeConfig.Save();
									Notification.Show(GetHostName() + " You now own " + arcade.Name + " Arcade");
									Class1.SetupActivities();
									foreach (Blip arcadeBlip3 in ArcadeBlips)
									{
										if (arcadeBlip3 != null)
										{
											arcadeBlip3.Delete();
										}
									}
									ChangeArcade();
									Class1.SetupActivities();
								}
								if (arcade.Name.Equals("Warehouse"))
								{
									ArcadeOwned = 1;
									ArcadeVersion = 4;
									ArcadeConfig.SetValue("Arcade", "ArcadeOwned", ArcadeOwned);
									ArcadeConfig.SetValue("Arcade", "ArcadeVersion", ArcadeVersion);
									ArcadeConfig.Save();
									Notification.Show(GetHostName() + " You now own " + arcade.Name + " Arcade");
									Class1.SetupActivities();
									foreach (Blip arcadeBlip4 in ArcadeBlips)
									{
										if (arcadeBlip4 != null)
										{
											arcadeBlip4.Delete();
										}
									}
									ChangeArcade();
									Class1.SetupActivities();
								}
								if (arcade.Name.Equals("Insert Coin"))
								{
									ArcadeOwned = 1;
									ArcadeVersion = 5;
									ArcadeConfig.SetValue("Arcade", "ArcadeOwned", ArcadeOwned);
									ArcadeConfig.SetValue("Arcade", "ArcadeVersion", ArcadeVersion);
									ArcadeConfig.Save();
									Notification.Show(GetHostName() + " You now own " + arcade.Name + " Arcade");
									Class1.SetupActivities();
									foreach (Blip arcadeBlip5 in ArcadeBlips)
									{
										if (arcadeBlip5 != null)
										{
											arcadeBlip5.Delete();
										}
									}
									ChangeArcade();
									Class1.SetupActivities();
								}
								if (arcade.Name.Equals("Eight Bit"))
								{
									ArcadeOwned = 1;
									ArcadeVersion = 6;
									ArcadeConfig.SetValue("Arcade", "ArcadeOwned", ArcadeOwned);
									ArcadeConfig.SetValue("Arcade", "ArcadeVersion", ArcadeVersion);
									ArcadeConfig.Save();
									Notification.Show(GetHostName() + " You now own " + arcade.Name + " Arcade");
									Class1.SetupActivities();
									foreach (Blip arcadeBlip6 in ArcadeBlips)
									{
										if (arcadeBlip6 != null)
										{
											arcadeBlip6.Delete();
										}
									}
									ChangeArcade();
									Class1.SetupActivities();
								}
							}
							else
							{
								Notification.Show("You dont have enough money to purchase this Arcade");
							}
						}
					}
					if (SimeonText1 == null)
					{
						SimeonText1 = new Scaleform("MP_MISSION_NAME_FREEMODE");
					}
					SimeonText1.CallFunction("SET_MISSION_INFO", arcade.Name + " || Price : $" + arcade.Price.ToString("N"), "~y~Single Player Heists~w~", 0, "", "", 4, 4);
					SimeonText1.Render3D(arcade.Location + new Vector3(0f, 0f, 0f), new Vector3(1f, 180f, 1f), new Vector3(3f, 2f, 2f));
					Scaleform scaleform2 = new Scaleform("MP_MISSION_NAME_FREEMODE");
					SimeonText1.Render3D(arcade.Location + new Vector3(0f, 0f, 1f), new Vector3(0f, 87f, World.RenderingCamera.Rotation.Z), new Vector3(5f, 5f, 5f));
					scaleform2.Render3D(arcade.Location + new Vector3(0f, 0f, 1f), new Vector3(0f, 87f, World.RenderingCamera.Rotation.Z), new Vector3(5f, 5f, 5f));
				}
			}
			if (!CreatedArcadeBlips)
			{
				foreach (Arcade arcade2 in Arcades)
				{
					Blip blip = GlobalHelper.CreateBlip(arcade2.Location);
					blip.Sprite = BlipSprite.Heist;
					blip.Color = BlipColor.Blue;
					blip.Name = "Purchase a Single Player Heists Arcade";
					ArcadeBlips.Add(blip);
				}
				CreatedArcadeBlips = true;
			}
			if (HeistBlip != null)
			{
				HeistBlip.Alpha = 0;
			}
		}

		public void SetPedOutfitCutscene(string MP, Ped NonCutscene)
		{
			if (MP.Equals("MP_1"))
			{
				CutscenePed1Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0);
				CutscenePed1Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1);
				CutscenePed1Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2);
				CutscenePed1Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3);
				CutscenePed1Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4);
				CutscenePed1Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5);
				CutscenePed1Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6);
				CutscenePed1Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7);
				CutscenePed1Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8);
				CutscenePed1Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9);
				CutscenePed1Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10);
				CutscenePed1Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11);
			}
			if (MP.Equals("MP_2"))
			{
				CutscenePed2Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0);
				CutscenePed2Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1);
				CutscenePed2Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2);
				CutscenePed2Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3);
				CutscenePed2Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4);
				CutscenePed2Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5);
				CutscenePed2Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6);
				CutscenePed2Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7);
				CutscenePed2Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8);
				CutscenePed2Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9);
				CutscenePed2Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10);
				CutscenePed2Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11);
			}
			if (MP.Equals("MP_3"))
			{
				CutscenePed3Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0);
				CutscenePed3Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1);
				CutscenePed3Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2);
				CutscenePed3Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3);
				CutscenePed3Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4);
				CutscenePed3Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5);
				CutscenePed3Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6);
				CutscenePed3Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7);
				CutscenePed3Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8);
				CutscenePed3Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9);
				CutscenePed3Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10);
				CutscenePed3Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11);
			}
			if (MP.Equals("MP_4"))
			{
				CutscenePed4Comp[0] = "0_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 0) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 0);
				CutscenePed4Comp[1] = "1_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 1) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 1);
				CutscenePed4Comp[2] = "2_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 2) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 2);
				CutscenePed4Comp[3] = "3_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 3) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 3);
				CutscenePed4Comp[4] = "4_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 4) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 4);
				CutscenePed4Comp[5] = "5_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 5) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 5);
				CutscenePed4Comp[6] = "6_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 6) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 6);
				CutscenePed4Comp[7] = "7_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 7) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 7);
				CutscenePed4Comp[8] = "8_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 8) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 8);
				CutscenePed4Comp[9] = "9_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 9) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 9);
				CutscenePed4Comp[10] = "10_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 10) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 10);
				CutscenePed4Comp[11] = "11_" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, NonCutscene, 11) + "_" + Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, NonCutscene, 11);
			}
		}

		public void GetPedOutfitCutscene(string MP, Ped NonCutscene)
		{
			if (MP.Equals("MP_1"))
			{
				string[] array = CutscenePed1Comp[0].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 0, int.Parse(array[1]), int.Parse(array[2]), 1);
				array = CutscenePed1Comp[1].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 1, int.Parse(array[1]), int.Parse(array[2]), 1);
				array = CutscenePed1Comp[2].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 2, int.Parse(array[1]), int.Parse(array[2]), 1);
				array = CutscenePed1Comp[3].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 3, int.Parse(array[1]), int.Parse(array[2]), 1);
				array = CutscenePed1Comp[4].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 4, int.Parse(array[1]), int.Parse(array[2]), 1);
				array = CutscenePed1Comp[5].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 5, int.Parse(array[1]), int.Parse(array[2]), 1);
				array = CutscenePed1Comp[6].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 6, int.Parse(array[1]), int.Parse(array[2]), 1);
				array = CutscenePed1Comp[7].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 7, int.Parse(array[1]), int.Parse(array[2]), 1);
				array = CutscenePed1Comp[8].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 8, int.Parse(array[1]), int.Parse(array[2]), 1);
				array = CutscenePed1Comp[9].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 9, int.Parse(array[1]), int.Parse(array[2]), 1);
				array = CutscenePed1Comp[10].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 10, int.Parse(array[1]), int.Parse(array[2]), 1);
				array = CutscenePed1Comp[11].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 11, int.Parse(array[1]), int.Parse(array[2]), 1);
			}
			if (MP.Equals("MP_2"))
			{
				string[] array2 = CutscenePed2Comp[0].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 0, int.Parse(array2[1]), int.Parse(array2[2]), 1);
				array2 = CutscenePed2Comp[1].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 1, int.Parse(array2[1]), int.Parse(array2[2]), 1);
				array2 = CutscenePed2Comp[2].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 2, int.Parse(array2[1]), int.Parse(array2[2]), 1);
				array2 = CutscenePed2Comp[3].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 3, int.Parse(array2[1]), int.Parse(array2[2]), 1);
				array2 = CutscenePed2Comp[4].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 4, int.Parse(array2[1]), int.Parse(array2[2]), 1);
				array2 = CutscenePed2Comp[5].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 5, int.Parse(array2[1]), int.Parse(array2[2]), 1);
				array2 = CutscenePed2Comp[6].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 6, int.Parse(array2[1]), int.Parse(array2[2]), 1);
				array2 = CutscenePed2Comp[7].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 7, int.Parse(array2[1]), int.Parse(array2[2]), 1);
				array2 = CutscenePed2Comp[8].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 8, int.Parse(array2[1]), int.Parse(array2[2]), 1);
				array2 = CutscenePed2Comp[9].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 9, int.Parse(array2[1]), int.Parse(array2[2]), 1);
				array2 = CutscenePed2Comp[10].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 10, int.Parse(array2[1]), int.Parse(array2[2]), 1);
				array2 = CutscenePed2Comp[11].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 11, int.Parse(array2[1]), int.Parse(array2[2]), 1);
			}
			if (MP.Equals("MP_3"))
			{
				string[] array3 = CutscenePed2Comp[0].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 0, int.Parse(array3[1]), int.Parse(array3[2]), 1);
				array3 = CutscenePed3Comp[1].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 1, int.Parse(array3[1]), int.Parse(array3[2]), 1);
				array3 = CutscenePed3Comp[2].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 2, int.Parse(array3[1]), int.Parse(array3[2]), 1);
				array3 = CutscenePed3Comp[3].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 3, int.Parse(array3[1]), int.Parse(array3[2]), 1);
				array3 = CutscenePed3Comp[4].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 4, int.Parse(array3[1]), int.Parse(array3[2]), 1);
				array3 = CutscenePed3Comp[5].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 5, int.Parse(array3[1]), int.Parse(array3[2]), 1);
				array3 = CutscenePed3Comp[6].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 6, int.Parse(array3[1]), int.Parse(array3[2]), 1);
				array3 = CutscenePed3Comp[7].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 7, int.Parse(array3[1]), int.Parse(array3[2]), 1);
				array3 = CutscenePed3Comp[8].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 8, int.Parse(array3[1]), int.Parse(array3[2]), 1);
				array3 = CutscenePed3Comp[9].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 9, int.Parse(array3[1]), int.Parse(array3[2]), 1);
				array3 = CutscenePed3Comp[10].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 10, int.Parse(array3[1]), int.Parse(array3[2]), 1);
				array3 = CutscenePed3Comp[11].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 11, int.Parse(array3[1]), int.Parse(array3[2]), 1);
			}
			if (MP.Equals("MP_4"))
			{
				string[] array4 = CutscenePed1Comp[0].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 0, int.Parse(array4[1]), int.Parse(array4[2]), 1);
				array4 = CutscenePed4Comp[1].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 1, int.Parse(array4[1]), int.Parse(array4[2]), 1);
				array4 = CutscenePed4Comp[2].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 2, int.Parse(array4[1]), int.Parse(array4[2]), 1);
				array4 = CutscenePed4Comp[3].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 3, int.Parse(array4[1]), int.Parse(array4[2]), 1);
				array4 = CutscenePed4Comp[4].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 4, int.Parse(array4[1]), int.Parse(array4[2]), 1);
				array4 = CutscenePed4Comp[5].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 5, int.Parse(array4[1]), int.Parse(array4[2]), 1);
				array4 = CutscenePed4Comp[6].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 6, int.Parse(array4[1]), int.Parse(array4[2]), 1);
				array4 = CutscenePed4Comp[7].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 7, int.Parse(array4[1]), int.Parse(array4[2]), 1);
				array4 = CutscenePed4Comp[8].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 8, int.Parse(array4[1]), int.Parse(array4[2]), 1);
				array4 = CutscenePed4Comp[9].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 9, int.Parse(array4[1]), int.Parse(array4[2]), 1);
				array4 = CutscenePed4Comp[10].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 10, int.Parse(array4[1]), int.Parse(array4[2]), 1);
				array4 = CutscenePed4Comp[11].Split(new char[1] { '_' });
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, NonCutscene, 11, int.Parse(array4[1]), int.Parse(array4[2]), 1);
			}
		}

		public void OnKeyDown(object sender, EventArgs e)
		{
		}
	}
}
